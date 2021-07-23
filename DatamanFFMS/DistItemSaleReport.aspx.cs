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

namespace AstralFFMS
{
    public partial class DistItemSaleReport : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());
                distitemsalerpt.DataSource = distributors;
                distitemsalerpt.DataBind();
                BindMaterialGroup();
                BindMaterialGroup();
                //rptmain.Style.Add("display", "block");
                //Ankita - 13/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //fill_TreeArea();
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }

        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
                //    DataSet ds = GetDataSet("Select smid,smname,underid,lvl from mastsalesrep where active=1 and underid<>0 order by smname");


                DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    root.CollapseAll();
                    CreateNode(root, St);
                    trview.Nodes.Add(root);
                }
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
        }


        public class Distributors
        {
            public string VDate { get; set; }
            public string Distributor { get; set; }
            public string MaterialGROUP { get; set; }
            public string Item { get; set; }
            public string Qty { get; set; }
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

            string query = @"SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
             t1.DistId in (" + Distid + ") and " + Qrychk + "  " + qry + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName order BY  t1.VDate desc";

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


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
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    ListBox1.DataSource = null;
                    ListBox1.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

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
                    string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
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

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
            //foreach (ListItem item in salespersonListBox.Items)
            //{
            //    if(item.Selected)
            //    {
            //        smIDStr += item.Value + ", ";
            //    }
            //    smIDStr = smIDStr.TrimStart(',').TrimEnd(',');
            //}
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
                    string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
                                  on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                                  t1.DistId in (" + distIdStr1 + ") and " + Qrychk + "  " + QueryMatGrp + " group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";

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

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                    distitemsalerpt.DataSource = null;
                    distitemsalerpt.DataBind();
                }
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                distitemsalerpt.DataSource = null;
                distitemsalerpt.DataBind();
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistItemSaleReport.aspx");
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
            }
            else
            {
                ClearControls();
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorItemSaleReport.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Distributor Sync Id".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

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

            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
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
                    string query = @"SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
                                       on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                                       t1.DistId in (" + distIdStr1 + ") and " + Qrychk + "  " + QueryMatGrp + " and da.partydist=1 group BY  t1.VDate,da.syncid,da.PartyName, mi.ItemName order BY t1.VDate desc";

                    DataTable dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, query);

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
                    }
                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment;filename=DistributorItemSaleReport.csv");
                    Response.Write(sb.ToString());
                    Response.End();
                }
            }

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
                    string compcode = DbConnectionDAL.GetStringScalarVal("select compcode from mastenviro");
                    if (!string.IsNullOrEmpty(compcode))
                    {
                        if (compcode == "selzer")
                        {
                            BindDistributorSalesmanWise(smIDStr12);
                        }
                        else
                        {
                            BindDistributorDDl(smIDStr12);
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No CompCode Exist');", true);

                        ListBox1.Items.Clear();
                        ListBox1.DataBind();
                    }
                    //string query = @"select CompCode from MastEnviro";
                    //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    //if (dt.Rows[0]["CompCode"].ToString() == "selzer")
                    //{
                    //    BindDistributorSalesmanWise(smIDStr12);
                    //}
                    //else
                    //{

                    //    BindDistributorDDl(smIDStr12);
                    //}

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
                    string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
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
                        Response.AddHeader("content-disposition", "attachment;filename=DistItemSaleReport.csv");
                        Response.Charset = "";
                        Response.ContentType = "application/text";
                        Response.Output.Write(csv);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }



    }
}