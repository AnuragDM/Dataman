using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using System.Net;
using System.Web.Script.Serialization;


namespace AstralFFMS
{
    public partial class DistributerDispatchOrderForm : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        string doc_id = "";

        BAL.LeaveRequest.LeaveAll lvAll = new BAL.LeaveRequest.LeaveAll();

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");

            myModal.Style.Add("display", "none");
            myModal1.Style.Add("display", "none");
            if (!IsPostBack)
            {
                //List<Distributors> distributors = new List<Distributors>();
                //distributors.Add(new Distributors());
                //distitemsalerpt.DataSource = distributors;
                //distitemsalerpt.DataBind();

                BindMaterialGroup();
                BindMaterialGroup();
                BindType();
                //rptmain.Style.Add("display", "block");
                //Ankita - 13/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //fill_TreeArea();
                //Added By - Nishu 06/12/2015 
                //txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";

                if (Request.QueryString["Docid"] != null)
                {
                    string PODOCID = Request.QueryString["Docid"];
                    GetDistributorItemSale1(PODOCID);
                }
            }
        }



        public void GetDistributorItemSale1(string PODOCID)
        {
            string qry = "";

            string query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(18,2)) as Amount from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
            on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where t1.PODocId in ('" + PODOCID + "') and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dtItem.Rows.Count > 0)
            {
                rptmain.Style.Add("display", "block");
                distitemsalerpt.DataSource = dtItem;
                distitemsalerpt.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                rptmain.Style.Add("display", "block");
                distitemsalerpt.DataSource = dtItem;
                distitemsalerpt.DataBind();
                btnExport.Visible = false;
            }
            dtItem.Dispose();

        }

        private void BindType()
        {
            string str = "select Text,Value from MastDistributorType order by sort";
            fillDDLDirect(ddltype, str, "Value", "Text", 1);
        }


        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }

        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    //string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    string query = "select msr.smid,msr.Smname as smname,msr.underid ,msr.lvl from mastsalesrep msr LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }

                DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    root.CollapseAll();
                    CreateNode(root, St);
                    trview.Nodes.Add(root);
                }

                St.Dispose();
                Rows = null;
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, DataTable Dt)
        {
            DataRow[] Rows = Dt.Select("underid =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(Childnode);
                Childnode.CollapseAll();
                CreateNode(Childnode, Dt);
            }

            Rows = null;
        }


        public class Distributors
        {
            public string VDate { get; set; }
            public string Distributor { get; set; }
            public string MaterialGROUP { get; set; }
            public string Item { get; set; }
            public string Qty { get; set; }
            public string rate { get; set; }
            public string Amount { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetDistributorItemSale(string Distid, string ProductGroup, string Product, string Fromdate, string Todate)
        {
            string qry = "";
            string Qrychk = " t1.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and t1.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            //            string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
            //                           on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where (" + Qrychk + ") group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";
            if (ProductGroup != "" && Product == "")
            {
                qry = "and mi1.itemid in(" + ProductGroup + ")";

            }
            if (Product != "" && Product != "")
            {
                qry = qry + "and mi.itemid in (" + Product + ")";

            }

            //            string query = @"SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
            //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
            //             t1.DistId in (" + Distid + ") and " + Qrychk + "  " + qry + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName order BY  t1.VDate desc";
            string query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
             t1.DistId in (" + Distid + ") and " + Qrychk + "  " + qry + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId and t2.OrderType is null group by tbl.DocId order by VDate desc";
            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


        }


        protected void distitemsalerpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // if (!IsPostBack)
            //   {
            //if (e.CommandName == "select")
            //{
            //    string[] commandArgs = new string[2];
            //    commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            //    visHdf.Value = commandArgs[0];
            //    dateHdf.Value = commandArgs[1];
            //    AjaxControlToolkit.ModalPopupExtender mp1 = ((AjaxControlToolkit.ModalPopupExtender)(e.Item.FindControl("ModalPopupExtender4")));
            //    TextArea1.Value = string.Empty;
            //    mp1.Show();
            //}


            if (e.CommandName == "selectDate")
            {
                RepeaterItem gvRow = (RepeaterItem)(((LinkButton)e.CommandSource).NamingContainer);
                int index = gvRow.ItemIndex;
                RepeaterItem row = distitemsalerpt.Items[index];

                HiddenField hdnDate = (HiddenField)row.FindControl("hdnDate");
                HiddenField hdnDistributorName = (HiddenField)row.FindControl("hdnDistributorName");
                HiddenField hdnDocId = (HiddenField)row.FindControl("hdnDocId");

                string dname = hdnDistributorName.Value.Replace(' ', '-');
                doc_id = hdnDocId.Value.ToString();
                string docid = hdnDocId.Value.Replace(' ', '-');
                //doc_id = hdnDocId.Value;
                //           int smId = Convert.ToInt32(gvData.DataKeys[index].Values[0]);
                //           string date = gvData.DataKeys[index].Values[0].ToString();
                //Response.Redirect("DistributerDispatchOrderDetails.aspx?DocId=" + docid + "&DistName=" + dname + "&VDate=" + hdnDate.Value + "&OrderType=D");
                // rptmain.Style.Add("display", "block");
                distname.Text = dname.Replace('-', ' ');
                vdate.Text = hdnDate.Value;
                string query = @"SELECT mi.ItemId as ItemId,t1.PODocId as DocId,mi.ItemName as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(18,2)) as OrderAmount,t.remarks FROM TransPurchOrder1 t1 left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId where t1.PODocId = '" + docid.Replace('-', ' ') + "'";
                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    lblRemarkViwPunch.Text = Convert.ToString(dtItem.Rows[0]["remarks"]);
                    myModal1.Style.Add("display", "none");
                    myModal.Style.Add("display", "block");
                    Div2.Style.Add("display", "block");
                    rpt.DataSource = dtItem;
                    rpt.DataBind();
                    // btnExport.Visible = true;
                }
                else
                {
                    myModal1.Style.Add("display", "none");
                    myModal.Style.Add("display", "block");
                    Div2.Style.Add("display", "block");
                    rpt.DataSource = dtItem;
                    rpt.DataBind();
                    //btnExport.Visible = false;
                }


            }


            if (e.CommandName == "selectDate1")
            {
                RepeaterItem gvRow = (RepeaterItem)(((LinkButton)e.CommandSource).NamingContainer);
                int index = gvRow.ItemIndex;
                RepeaterItem row = distitemsalerpt.Items[index];

                HiddenField hdnDate = (HiddenField)row.FindControl("hdnDate");
                HiddenField hdnDistributorName = (HiddenField)row.FindControl("hdnDistributorName");
                HiddenField hdnDocId = (HiddenField)row.FindControl("hdnDocId");

                string dname = hdnDistributorName.Value.Replace(' ', '-');
                HiddenField_ID.Value = hdnDocId.Value;
                doc_id = hdnDocId.Value.ToString();
                string docid = hdnDocId.Value.Replace(' ', '-');
                //           int smId = Convert.ToInt32(gvData.DataKeys[index].Values[0]);
                //           string date = gvData.DataKeys[index].Values[0].ToString();
                //Response.Redirect("DistributerDispatchOrderDetails.aspx?DocId=" + docid + "&DistName=" + dname + "&VDate=" + hdnDate.Value + "&OrderType=D");
                // rptmain.Style.Add("display", "block");
                Label1.Text = dname.Replace('-', ' ');
                Label2.Text = hdnDate.Value;
                string query = @"SELECT mi.ItemId as ItemId,mi.ItemName as ItemName,t1.Qty as OrderQty,t.remarks FROM TransPurchOrder1 t1 left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId where t1.PODocId = '" + docid.Replace('-', ' ') + "'";
                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                { lblRemarkViewOrderCancel.Text = Convert.ToString(dtItem.Rows[0]["remarks"]); }
                myModal1.Style.Add("display", "block");
                myModal.Style.Add("display", "none");
                //Div2.Style.Add("display", "none");
                //rpt.DataSource = dtItem;
                //rpt.DataBind();
                // btnExport.Visible = true;
                //}
                //else
                //{
                //    myModal.Style.Add("display", "block");
                //    Div2.Style.Add("display", "none");
                //rpt.DataSource = dtItem;
                //rpt.DataBind();
                //btnExport.Visible = false;
                // }

            }
        }

        // }

        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string citystr = "";
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');
                    //string distqry = @"select * from MastParty where CityId in (" + citystr + ")  and PartyDist=1 and Active=1 order by PartyName";

                    string condition = "";
                    if (ddltype.SelectedIndex > 0)
                        condition = " and DistType='" + ddltype.SelectedValue + "' ";


                    string distqry = @"select mp.PartyId,mp.PartyName + ' - ' + md.Text + ' ( ' + ma.AreaName + ' )' as PartyName from MastParty mp left join MastDistributorType md on md.Value=mp.DistType left join MastArea ma on ma.AreaId=mp.CityId where mp.CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  " + condition + " and mp.PartyDist=1 and mp.Active=1 order by mp.PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        ListBox1.DataSource = dt;
                        ListBox1.DataBind();
                    }

                    dtCity.Dispose();
                    dtDist.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    distitemsalerpt.DataSource = null;
                    distitemsalerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltype.SelectedIndex > 0)
            {
                string disttype = ddltype.SelectedValue;

                string smIDStr12 = "", smIDStr = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                string smiMStr = smIDStr12;
                if (smIDStr12 != "")
                {
                    BindDistributorDDl(smIDStr12);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);

                    distitemsalerpt.DataSource = null;
                    distitemsalerpt.DataBind();
                    ddltype.SelectedIndex = 0;
                }
            }
            else
            {
                DataTable dt = new DataTable();

                ListBox1.DataSource = dt;
                ListBox1.DataBind();
            }

        }

        private void BindDistributorSalesmanWise(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    //string citystr = "";
                    //string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    //DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    //for (int i = 0; i < dtCity.Rows.Count; i++)
                    //{
                    //    citystr += dtCity.Rows[i]["AreaId"] + ",";
                    //}
                    //citystr = citystr.TrimStart(',').TrimEnd(',');                  

                    //string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";

                    string distqry = @"select PartyId,PartyName from MastParty where smid in (SELECT smid FROM MastSalesRepGrp WHERE maingrp in (" + SMIDStr + "))  and PartyDist=1 and Active=1 order by PartyName";

                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }

                    dtDist.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    distitemsalerpt.DataSource = null;
                    distitemsalerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindMaterialGroup()
        {
            try
            { //Ankita - 13/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {

                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();

                }
                dtProdRep.Dispose();
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void ClearControls()
        {
            try
            {
                productListBox.Items.Clear();
                distitemsalerpt.DataSource = null;
                distitemsalerpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        //protected void btnDispatchClose_Click(object sender, EventArgs e)
        //{
        //    TextArea1.InnerText = "";
        //    myModal.Style.Add("display", "none");
        //    fillRepeater();
        //}
        protected void btnDispatchSave_Click(object sender, EventArgs e)
        {
            try
            {
                string updatesql = "";
                string dname = distname.Text;
                string date = vdate.Text;
                //TextArea1.Value = null;
                int cnt = 0;
                string docid = "";
                Common cs = new Common();
                if (TextArea1.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill Remarks.');", true);
                    return;
                }

                foreach (RepeaterItem item in rpt.Items)
                {
                    HiddenField hdField2 = (HiddenField)item.FindControl("HiddenField2");

                    HiddenField hdField = (HiddenField)item.FindControl("HiddenField1");
                    HtmlInputText txtQty = (HtmlInputText)item.FindControl("dispatchQty");

                    if (cnt == 0)
                    {
                        docid = hdField2.Value;
                        updatesql = "update [dbo].[TransPurchOrder] set DispatchRemarks= '" + TextArea1.Value.Replace("'", "''") + "', OrderType = 'D',DispatchCancel_Smid=" + Settings.Instance.UserID + ",dispatchcanceldatetime=GetDate()  where PODocId = '" + hdField2.Value + "'";
                        // updatesql = "update [dbo].[TransPurchOrder] set DispatchRemarks= '" + TextArea1.Value + "', OrderType = 'D' where PODocId = '" + hdField2.Value + "' and  VDate = '" + date + "'";
                        DbConnectionDAL.ExecuteQuery(updatesql);
                        cnt++;
                    }


                    //updatesql = "update [dbo].[TransPurchOrder1] set DispatchQty= " + Convert.ToDouble(txtQty.Value) + " where PODocId = '" + doc_id + "' and  VDate = '" + date + "' and ItemId=" + Convert.ToInt32(hdField.Value) + "";
                    updatesql = "update [dbo].[TransPurchOrder1] set DispatchQty= " + Convert.ToDouble(txtQty.Value) + " where PODocId = '" + hdField2.Value + "' and ItemId=" + Convert.ToInt32(hdField.Value) + "";
                    DbConnectionDAL.ExecuteQuery(updatesql);
                }

                cs.pushnotificationforOrderDispatchCancel(docid, "D", TextArea1.Value.Replace("'", "''"));


                fillRepeater();


                //Send Notification to Distributor when order dispatch
                //try
                //{


                //    string selectquery = @"select UserId,SMId,DistId from TransPurchOrder where PODocId='" + docid + "'";
                //    string str="Distributer Dispatch Order for DocId: " + docid + ".";

                //    DataTable dtItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, selectquery);
                //    if (dtItem1.Rows.Count > 0)
                //    {
                //         string msgurl = "SendNotification.aspx?SMId=" + Convert.ToInt32(dtItem1.Rows[0]["UserId"].ToString()) + "";
                //        string insertsql = "INSERT INTO [dbo].[TransNotification]([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[Distid]) VALUES ('DISTRIBUTOR DISPATCH ORDER'," + Convert.ToInt32(dtItem1.Rows[0]["UserId"].ToString()) + ",DateAdd(minute,330,getutcdate()),'" + msgurl + "','" + str + "',0," + Settings.Instance.UserID + "," + Convert.ToInt32(dtItem1.Rows[0]["SMId"].ToString()) + "," + Convert.ToInt32(dtItem1.Rows[0]["DistId"].ToString()) + ")";
                //        DbConnectionDAL.ExecuteQuery(insertsql);
                //    }

                //}
                //catch(Exception ex)
                //{

                //}

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order Dispatched Successfully.');", true);
                TextArea1.Value = string.Empty;
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Order was not Dispatched.');", true);
            }

        }

        protected void btnCancelOrderSave_Click(object sender, EventArgs e)
        {
            try
            {
                Common CS = new Common();
                if (TextArea2.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill Remarks.');", true);
                    return;
                }

                string dname = distname.Text;
                string date = vdate.Text;

                string did = HiddenField_ID.Value;

                string deletesql = "update [dbo].[TransPurchOrder] set DispatchRemarks= '" + TextArea2.Value.Replace("'", "''") + "', OrderType = 'C',DispatchCancel_Smid=" + Settings.Instance.UserID + ",dispatchcanceldatetime=GetDate()  where PODocId = '" + did + "'";
                // updatesql = "update [dbo].[TransPurchOrder] set DispatchRemarks= '" + TextArea1.Value + "', OrderType = 'D' where PODocId = '" + hdField2.Value + "' and  VDate = '" + date + "'";
                DbConnectionDAL.ExecuteQuery(deletesql);

                CS.pushnotificationforOrderDispatchCancel(did, "C", TextArea2.Value.Replace("'", "''"));

                fillRepeater();


                //Send Notification to Distributor when order cancel
                //try
                //{


                //    string selectquery = @"select UserId,SMId,DistId from TransPurchOrder where PODocId='" + did + "'";
                //    string str = "Distributer Dispatch Cancel Order for DocId: " + did + ".";

                //    DataTable dtItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, selectquery);
                //    if (dtItem1.Rows.Count > 0)
                //    {
                //        string msgurl = "SendNotification.aspx?SMId=" + Convert.ToInt32(dtItem1.Rows[0]["UserId"].ToString()) + "";
                //        string insertsql = "INSERT INTO [dbo].[TransNotification]([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[Distid]) VALUES ('DISTRIBUTOR DISPATCH CANCEL ORDER'," + Convert.ToInt32(dtItem1.Rows[0]["UserId"].ToString()) + ",DateAdd(minute,330,getutcdate()),'" + msgurl + "','" + str + "',0," + Settings.Instance.UserID + "," + Convert.ToInt32(dtItem1.Rows[0]["SMId"].ToString()) + "," + Convert.ToInt32(dtItem1.Rows[0]["DistId"].ToString()) + ")";
                //        DbConnectionDAL.ExecuteQuery(insertsql);
                //    }

                //}
                //catch (Exception ex)
                //{

                //}

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Order Cancelled Successfully.');", true);
                TextArea2.Value = string.Empty;
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Order was not Cancelled.');", true);
            }
        }

        private void fillRepeater()
        {
            try
            {
                string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "";

                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        distIdStr1 += item.Value + ",";
                    }
                }
                distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');

                Qrychk = " t1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                string query = "";
                if (distIdStr1 != "")
                {
                    query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(18,2)) as Amount from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
            on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
            t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                }
                else
                {
                    query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(18,2)) as Amount from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
            on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                }

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    distitemsalerpt.DataSource = dtItem;
                    distitemsalerpt.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    distitemsalerpt.DataSource = dtItem;
                    distitemsalerpt.DataBind();
                    btnExport.Visible = false;
                }
                dtItem.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "";

                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        distIdStr1 += item.Value + ",";
                    }
                }
                distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');


                if (smIDStr1 != "")
                {
                    //if (distIdStr1 != "")
                    //{
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        distitemsalerpt.DataSource = new DataTable();
                        distitemsalerpt.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }

                    Qrychk = " t1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                    string query = "";
                    //                    if (distIdStr1 != "")
                    //                    {
                    //                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
                    //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                    //             t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId   group by tbl.DocId order by VDate desc";
                    //                    }
                    //                    else
                    //                    {
                    //                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
                    //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                    //                    }


                    string condition = "";
                    if (ddltype.SelectedIndex > 0)
                        condition = " and da.DistType='" + ddltype.SelectedValue + "' ";

                    if (distIdStr1 != "")
                    {
                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(18,2)) as Amount from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
            on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
            t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " " + condition + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                    }
                    else
                    {
                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(18,2)) as Amount from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
            on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where " + Qrychk + " " + condition + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                    }


                    DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    if (dtItem.Rows.Count > 0)
                    {
                        rptmain.Style.Add("display", "block");
                        distitemsalerpt.DataSource = dtItem;
                        distitemsalerpt.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        rptmain.Style.Add("display", "block");
                        distitemsalerpt.DataSource = dtItem;
                        distitemsalerpt.DataBind();
                        btnExport.Visible = false;
                    }
                    dtItem.Dispose();
                }

                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                    distitemsalerpt.DataSource = null;
                    distitemsalerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Error While Getting Records.');", true);
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistributerDispatchOrderForm.aspx");
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            distitemsalerpt.DataSource = null;
            distitemsalerpt.DataBind();
            btnExport.Visible = false;
        }

        protected void matGrpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            //         string message = "";
            foreach (ListItem matGrp in matGrpListBox.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

            if (matGrpStr != "")
            { //Ankita - 13/may/2016- (For Optimization)
                //string mastItemQry1 = @"select * from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
                dtMastItem1.Dispose();
            }
            else
            {
                ClearControls();
            }

        }



        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Response.Clear();
            //Response.ContentType = "text/csv";
            //Response.AddHeader("content-disposition", "attachment; filename=distributerdispatchorder.csv");
            //string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Distributor Sync Id".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Doc Id".TrimStart('"').TrimEnd('"') + "," + "Total Quantity".TrimStart('"').TrimEnd('"');

            //StringBuilder sb = new StringBuilder();
            //sb.Append(headertext);
            //sb.AppendLine(System.Environment.NewLine);
            //string dataText = string.Empty;

            //DataTable dtParams = new DataTable();
            //dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("ProductGroup", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));

            //foreach (RepeaterItem item in distitemsalerpt.Items)
            //{
            //    DataRow dr = dtParams.NewRow();
            //    Label VdateLabel = item.FindControl("VdateLabel") as Label;
            //    dr["Date"] = VdateLabel.Text;
            //    Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
            //    dr["DistributorName"] = DistributorLabel.Text.ToString();
            //    Label MaterialGROUPLabel = item.FindControl("MaterialGROUPLabel") as Label;
            //    dr["ProductGroup"] = MaterialGROUPLabel.Text.ToString();
            //    Label ItemLabel = item.FindControl("ItemLabel") as Label;
            //    dr["Product"] = ItemLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
            //    Label QtyLabel = item.FindControl("QtyLabel") as Label;
            //    dr["Qty"] = QtyLabel.Text.ToString();
            //    Label AmountLabel = item.FindControl("AmountLabel") as Label;
            //    dr["Amount"] = AmountLabel.Text.ToString();

            //    dtParams.Rows.Add(dr);
            //}

            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "", totalStr = "", DispatchQty = "";
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    distIdStr1 += item.Value + ",";
                }
            }
            distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');

            ////For MatGrp
            //foreach (ListItem matGrpItems in matGrpListBox.Items)
            //{
            //    if (matGrpItems.Selected)
            //    {
            //        matGrpStrNew += matGrpItems.Value + ",";
            //    }
            //}
            //matGrpStrNew = matGrpStrNew.TrimStart(',').TrimEnd(',');

            ////For Product
            //foreach (ListItem product in productListBox.Items)
            //{
            //    if (product.Selected)
            //    {
            //        matProStrNew += product.Value + ",";
            //    }
            //}
            //matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');

            //if (matGrpStrNew != "" && matProStrNew != "")
            //{
            //    QueryMatGrp = " and mi.ItemId in (" + matProStrNew + ") ";
            //}
            //if (matGrpStrNew != "" && matProStrNew == "")
            //{
            //    QueryMatGrp = " and mi1.ItemId in (" + matGrpStrNew + ") ";
            //}
            if (smIDStr1 != "")
            {

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment; filename=distributerdispatchorder.csv");
                string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Distributor Sync Id".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Doc Id".TrimStart('"').TrimEnd('"') + "," + "Total Quantity".TrimStart('"').TrimEnd('"') + "," + "TotalAmount".TrimStart('"').TrimEnd('"') + "," + "ItemName".TrimStart('"').TrimEnd('"') + "," + "OrderQty".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;
                // if (distIdStr1 != "")
                // {
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    distitemsalerpt.DataSource = new DataTable();
                    distitemsalerpt.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }


                string condition = "";
                if (ddltype.SelectedIndex > 0)
                    condition = " and da.DistType='" + ddltype.SelectedValue + "' ";

                Qrychk = " t1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                //string query = @"SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
                //on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                //t1.DistId in (" + distIdStr1 + ") and " + Qrychk + "  " + QueryMatGrp + " and da.partydist=1 group BY  t1.VDate,da.syncid,da.PartyName, mi.ItemName order BY t1.VDate desc";


                // string query = @"SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty from TransPurchOrder1 t1 left outer join MastItem mi
                // on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                //  t1.DistId in (" + distIdStr1 + ") and " + Qrychk + "  " + QueryMatGrp + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName order BY  t1.VDate desc";

                string query = "";
                //                    if (distIdStr1 != "")
                //                    {
                //                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,max(t2.OrderType) as OrderType from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
                //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                //             t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId  group by tbl.DocId order by VDate desc";
                //                    }//where t2.OrderType is null
                //                    else
                //                    {
                //                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty ,max(t2.OrderType) as OrderType from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
                //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                //                    }
                if (distIdStr1 != "")
                {
                    query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(18,2)) as Amount from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
            on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
            t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " and da.partydist=1 " + condition + " group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                }
                else
                {
                    query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(18,2)) as Amount from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
            on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where " + Qrychk + " and da.partydist=1 " + condition + " group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId where t2.OrderType is null group by tbl.DocId order by VDate desc";
                }
                DataTable dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                string _docId = "";
                foreach (DataRow dr in dtParams.Rows)
                {
                    _docId += "'" + Convert.ToString(dr["DocId"]) + "'" + ',';
                }
                _docId = _docId.TrimEnd(',');
                string _query = @" select tp.PODocId, mi.ItemName,tp1.Qty as ItemQty,tp1.DispatchQty,case OrderType when 'C' then 'Cancel' when 'D'                then 'Dispatch' else 'N/A' end as OrderStatus,OrderType,cast((tp1.Qty*tp1.Rate) as decimal(18,2)) as Amount from TransPurchOrder1 tp1
                                        inner join MastItem mi   on mi.ItemId=tp1.ItemId
                                        inner join  TransPurchOrder tp on tp1.PODocId= tp.PODocId  where tp.PODocId IN(" + _docId + ")";
                DataTable dtParams1 = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                DataTable dtResult = new DataTable();
                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            string h = dtParams.Rows[j][k].ToString();
                            string d = h.Replace(",", " ");
                            dtParams.Rows[j][k] = "";
                            dtParams.Rows[j][k] = d;
                            dtParams.AcceptChanges();
                            if (k == 0)
                            {
                                //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                    var result = from r in dtParams1.AsEnumerable()
                                 where r.Field<string>("PODocId") == Convert.ToString(dtParams.Rows[j]["DocId"])
                                 select r;
                    dtResult = result.CopyToDataTable();
                    int l = 5;
                    foreach (DataRow dr1 in dtResult.Rows)
                    {
                        if (Convert.ToString(dr1["OrderType"]) == "C") DispatchQty = "0";
                        else if (Convert.ToString(dr1["OrderType"]) == "D") DispatchQty = Convert.ToString(dr1["DispatchQty"]);
                        else DispatchQty = "";
                        totalStr += "," + ',' + ',' + ',' + ',' + ',' + Convert.ToString(dr1["ItemName"]) + ',' + Convert.ToString(dr1["ItemQty"]) + ',' + Convert.ToString(dr1["Amount"]);
                        sb.Append(totalStr);
                        sb.Append(Environment.NewLine);
                        totalStr = string.Empty;
                    }
                    sb.Append(Environment.NewLine);
                }
                try
                {


                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment; filename=distributerdispatchorder.csv");
                    Response.Write(sb.ToString());
                    Response.End();

                    sb.Clear();
                    dtParams.Dispose();
                    dtParams1.Dispose();
                    dtResult.Dispose();
                }
                catch (Exception ex)
                {
                    {
                        ex.ToString();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);



                    }
                }


                //}

                //            //Added- Abhishek - 04-06-2016
                //            string smIDStrNewCSV = "", smIDStr1NewCSV = "";
                //            string distIdStr1NewCSV = "", QrychkNewCSV = "", QueryMatGrpNewCSV = "", QueryProdNewCSV = "", QueryNewCSV = "", matGrpStrNewCSV = "", matProStrNewCSV = "";
                //            foreach (TreeNode node in trview.CheckedNodes)
                //            {
                //                smIDStr1NewCSV = node.Value;
                //                {
                //                    smIDStrNewCSV += node.Value + ",";
                //                }
                //            }
                //            smIDStr1NewCSV = smIDStrNewCSV.TrimStart(',').TrimEnd(',');

                //            foreach (ListItem item in ListBox1.Items)
                //            {
                //                if (item.Selected)
                //                {
                //                    distIdStr1NewCSV += item.Value + ",";
                //                }
                //            }
                //            distIdStr1NewCSV = distIdStr1NewCSV.TrimStart(',').TrimEnd(',');

                //            //For MatGrp
                //            foreach (ListItem matGrpItems in matGrpListBox.Items)
                //            {
                //                if (matGrpItems.Selected)
                //                {
                //                    matGrpStrNewCSV += matGrpItems.Value + ",";
                //                }
                //            }
                //            matGrpStrNewCSV = matGrpStrNewCSV.TrimStart(',').TrimEnd(',');

                //            //For Product
                //            foreach (ListItem product in productListBox.Items)
                //            {
                //                if (product.Selected)
                //                {
                //                    matProStrNewCSV += product.Value + ",";
                //                }
                //            }
                //            matProStrNewCSV = matProStrNewCSV.TrimStart(',').TrimEnd(',');

                //            if (matGrpStrNewCSV != "" && matProStrNewCSV != "")
                //            {
                //                QueryMatGrpNewCSV = " and mi.ItemId in (" + matProStrNewCSV + ") ";
                //            }
                //            if (matGrpStrNewCSV != "" && matProStrNewCSV == "")
                //            {
                //                QueryMatGrpNewCSV = " and mi1.ItemId in (" + matGrpStrNewCSV + ") ";
                //            }
                //            if (smIDStr1NewCSV != "")
                //            {
                //                if (distIdStr1NewCSV != "")
                //                {
                //                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                //                    {
                //                        distitemsalerpt.DataSource = new DataTable();
                //                        distitemsalerpt.DataBind();
                //                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                //                        return;
                //                    }

                //                    QrychkNewCSV = " t1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                //                    string queryNewCSV = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
                //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                //             t1.DistId in (" + distIdStr1NewCSV + ") and " + QrychkNewCSV + "  " + QueryProdNewCSV + " group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";

                //                    DataTable dtItemNewCSV = DbConnectionDAL.GetDataTable(CommandType.Text, queryNewCSV);
                //                    string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');
                //                    StringBuilder sb = new StringBuilder();
                //                    sb.Append(headertext);
                //                    sb.Append(System.Environment.NewLine);
                //                    for (int j = 0; j < dtItemNewCSV.Rows.Count; j++)
                //                    {
                //                        for (int k = 0; k < dtItemNewCSV.Columns.Count; k++)
                //                        {
                //                            if (dtItemNewCSV.Rows[j][k].ToString().Contains(","))
                //                            {
                //                                if (k == 0)
                //                                {
                //                                    sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtItemNewCSV.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                //                                }
                //                                else
                //                                {
                //                                    sb.Append(String.Format("\"{0}\"", dtItemNewCSV.Rows[j][k].ToString()) + ',');
                //                                }
                //                            }
                //                            else if (dtItemNewCSV.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                //                            {
                //                                if (k == 0)
                //                                {
                //                                    sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtItemNewCSV.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                //                                }
                //                                else
                //                                {
                //                                    sb.Append(String.Format("\"{0}\"", dtItemNewCSV.Rows[j][k].ToString()) + ',');
                //                                }
                //                            }
                //                            else
                //                            {
                //                                if (k == 0)
                //                                {
                //                                    sb.Append(Convert.ToDateTime(dtItemNewCSV.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                //                                }
                //                                else
                //                                {
                //                                    sb.Append(dtItemNewCSV.Rows[j][k].ToString() + ',');
                //                                }
                //                            }
                //                        }
                //                        sb.Append(Environment.NewLine);
                //                    }
                //                    Response.Clear();
                //                    Response.ContentType = "text/csv";
                //                    Response.AddHeader("content-disposition", "attachment;filename=DistributorItemSaleReport.csv");
                //                    Response.Write(sb.ToString());
                //                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                //                    Response.End();
                //                }
                //                else
                //                {
                //                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                //                    distitemsalerpt.DataSource = null;
                //                    distitemsalerpt.DataBind();
                //                }
                //            }
                //            else
                //            {
                //                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                //                distitemsalerpt.DataSource = null;
                //                distitemsalerpt.DataBind();
                //            }

                //End
                //Response.Clear();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment;filename=DistributorItemSaleReport.xls");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                //distitemsalerpt.RenderControl(hw);
                //Response.Output.Write(sw.ToString());
                //Response.Flush();
                //Response.End();

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                distitemsalerpt.DataSource = null;
                distitemsalerpt.DataBind();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            string smIDStr = "", smIDStr12 = "";
            if (cnt == 0)
            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                string smiMStr = smIDStr12;
                if (smIDStr12 != "")
                {
                    BindDistributorDDl(smIDStr12);
                    //BindDistributorSalesmanWise(smIDStr12);
                }
                else
                {
                    ListBox1.Items.Clear();
                    ListBox1.DataBind();

                }
                ViewState["tree"] = smiMStr;

            }

            cnt = cnt + 1;
            return;
        }
        protected void ExportCSV(object sender, EventArgs e)
        {
            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
            try
            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        distIdStr1 += item.Value + ",";
                    }
                }
                distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');

                //For MatGrp
                foreach (ListItem matGrpItems in matGrpListBox.Items)
                {
                    if (matGrpItems.Selected)
                    {
                        matGrpStrNew += matGrpItems.Value + ",";
                    }
                }
                matGrpStrNew = matGrpStrNew.TrimStart(',').TrimEnd(',');

                //For Product
                foreach (ListItem product in productListBox.Items)
                {
                    if (product.Selected)
                    {
                        matProStrNew += product.Value + ",";
                    }
                }
                matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');

                if (matGrpStrNew != "" && matProStrNew != "")
                {
                    QueryMatGrp = " and mi.ItemId in (" + matProStrNew + ") ";
                }
                if (matGrpStrNew != "" && matProStrNew == "")
                {
                    QueryMatGrp = " and mi1.ItemId in (" + matGrpStrNew + ") ";
                }
                if (smIDStr1 != "")
                {
                    if (distIdStr1 != "")
                    {
                        if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                        {
                            distitemsalerpt.DataSource = new DataTable();
                            distitemsalerpt.DataBind();
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                            return;
                        }

                        Qrychk = " t1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                        string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty from TransDistInv1 t1 left outer join MastItem mi
                                       on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                                       t1.DistId in (" + distIdStr1 + ") and " + Qrychk + "  " + QueryMatGrp + " group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";

                        DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                        {


                            //Build the CSV file data as a Comma separated string.
                            string csv = string.Empty;

                            foreach (DataColumn column in dt.Columns)
                            {
                                //Add the Header row for CSV file.
                                csv += column.ColumnName + ',';
                            }

                            //Add new line.
                            csv += "\r\n";

                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    //Add the Data rows.
                                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                                }

                                //Add new line.
                                csv += "\r\n";
                            }

                            //Download the CSV file.
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment; filename=distributerdispatchorder.csv");
                            Response.Charset = "";
                            Response.ContentType = "application/text";
                            Response.Output.Write(csv);
                            Response.Flush();
                            Response.End();
                        }

                        dt.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);



            }
        }







    }
}