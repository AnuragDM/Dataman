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
    public partial class RetailerLedgerReport : System.Web.UI.Page
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
            //if (Request.QueryString["DistId"] != null)
            //{
            //    string DistId = Convert.ToString(Request.QueryString["DistId"]);
            //    string fromdate = HttpContext.Current.Session["aaa"].ToString();
            //    string todate = HttpContext.Current.Session["bbb"].ToString();
            //    GetDetailLedgerData(Convert.ToInt32(DistId), fromdate, todate);

            //}
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {


                //List<Distributors> distributors = new List<Distributors>();
                //distributors.Add(new Distributors());
                //distreportrpt.DataSource = distributors;
                //distreportrpt.DataBind();
                if (Request.QueryString["Retailerid"] != null)
                {
                    string DistId = Convert.ToString(Request.QueryString["Retailerid"]);
                    string fromdate = HttpContext.Current.Session["aaa"].ToString();
                    string todate = HttpContext.Current.Session["bbb"].ToString();
                    GetDetailLedgerData(Convert.ToInt32(DistId), fromdate, todate);
                    // rptmain1.Style.Add("display", "none");
                }
                CalendarExtender3.EndDate = Settings.GetUTCTime();
                //Added By - Nishu 06/12/2015 
                if (Request.QueryString["Retailerid"] != null)
                {
                    // BindTreeViewControl();
                    BindAreaDDl(Session["treenodes"].ToString());
                    string[] accStaffAll = new string[500];
                    string retailerid = HttpContext.Current.Session["RetailerId"].ToString();
                    string accStaff = retailerid;
                    accStaffAll = accStaff.Split(',');
                    if (accStaffAll.Length > 0)
                    {
                        foreach (ListItem item in RetailerListBox.Items)
                        {
                            for (int i = 0; i < accStaffAll.Length; i++)
                            {
                                if (item.Value == accStaffAll[i].ToString())
                                {
                                    item.Selected = true;

                                }

                            }
                        }
                    }
                    txtfmDate.Text = HttpContext.Current.Session["aaa"].ToString();
                    txttodate.Text = HttpContext.Current.Session["bbb"].ToString();
                }
                else
                {
                    txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                    txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                }
                //End
                //Ankita - 13/may/2016- (For Optimization)
                roleType = Settings.Instance.RoleType;
                //  GetRoleType(Settings.Instance.RoleID);
                //fill_TreeArea();
                BindTreeViewControl();
                btnExport.Visible = true;
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

            public string Distributor { get; set; }
            public string Debit { get; set; }
            public string Credit { get; set; }
            public string Closing { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public static string getretailerledger(string Areaid, string Retailerid, string Fromdate, string Todate)
        {        
            string smIDStr1 = "";          
            smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
            string Qrychk = " TransRetailerLedger.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            HttpContext.Current.Session["aaa"] = Fromdate;
            HttpContext.Current.Session["bbb"] = Todate;
            HttpContext.Current.Session["RetailerId"] = Retailerid;
            //string query = @"select TransDistributerLedger.DistId, partyname as Distributor, sum(Amtdr) AS Debit, Sum(Amtcr) AS Credit,(sum(AmtDr)-sum(AmtCr))  as Closing from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId WHERE TransDistributerLedger.DistId IN (" + Distid + ") AND mastparty.PartyDist=1 and " + Qrychk + " group by DistId,PartyName order by Partyname";
            string query = @"select TransRetailerLedger.DistId, partyname as Distributor, sum(Amtdr) AS Debit, Sum(Amtcr) AS Credit,(sum(AmtDr)-sum(AmtCr))  as Closing from TransRetailerLedger left join MastParty on MastParty.PartyId=TransRetailerLedger.DistId WHERE TransRetailerLedger.DistId IN (" + Retailerid + ") AND mastparty.PartyDist=0 and " + Qrychk + " group by DistId,PartyName order by Partyname";
            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


        }       

        //private void BindDistributorDDl(string SMIDStr)
        //{
        //    try
        //    {
        //        if (SMIDStr != "")
        //        {
        //            string citystr = "";                  
        //            string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
        //            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);

        //            for (int i = 0; i < dtCity.Rows.Count; i++)
        //            {
        //                citystr += dtCity.Rows[i]["AreaId"] + ",";
        //            }
        //            citystr = citystr.TrimStart(',').TrimEnd(',');
        //            string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                 
        //            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
        //            if (dtDist.Rows.Count > 0)
        //            {
        //                ListBox1.DataSource = dtDist;
        //                ListBox1.DataTextField = "PartyName";
        //                ListBox1.DataValueField = "PartyId";
        //                ListBox1.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
        //            distreportrpt.DataSource = null;
        //            distreportrpt.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        private void BindAreaDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {                  
                    string AreaQry = @"  select AreaId,AreaName from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and areatype='Area' and Active=1 order by AreaName";
                    //DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQry);

                    //for (int i = 0; i < dtArea.Rows.Count; i++)
                    //{
                    //    Areastr += dtArea.Rows[i]["AreaId"] + ",";
                    //}
                    //Areastr = Areastr.TrimStart(',').TrimEnd(',');
                    //string distqry = @"select PartyId,PartyName from MastParty where AreaId in (" + Areastr + ") and PartyDist=1 and Active=1 order by PartyName";

                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQry);
                    if (dtDist.Rows.Count > 0)
                    {
                        AreaListBox.DataSource = dtDist;
                        AreaListBox.DataTextField = "AreaName";
                        AreaListBox.DataValueField = "AreaId";
                        AreaListBox.DataBind();
                    }
                    else
                    {
                        AreaListBox.DataSource = null;
                        AreaListBox.DataBind();
                    }
                }
                else
                {
                   // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    //distreportrpt.DataSource = null;
                    //distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindRetailerDDl(string AreaStr)
        {
            try
            {
                if (AreaStr != "")
                {
                    string distqry = @"select PartyId,PartyName from MastParty where AreaId in (" + AreaStr + ") and PartyDist=0 and Active=1 order by PartyName";

                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        RetailerListBox.DataSource = dtDist;
                        RetailerListBox.DataTextField = "PartyName";
                        RetailerListBox.DataValueField = "PartyId";
                        RetailerListBox.DataBind();
                    }
                    else
                    {
                        RetailerListBox.DataSource = null;
                        RetailerListBox.DataBind();
                    }
                }
                else
                {
                   // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Area');", true);
                    //distreportrpt.DataSource = null;
                    //distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void AreaListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string AreaStr = "";
            foreach (ListItem area in AreaListBox.Items)
            {
                if (area.Selected)
                {
                    AreaStr += area.Value + ",";
                }
            }
            AreaStr = AreaStr.TrimStart(',').TrimEnd(',');
            BindRetailerDDl(AreaStr);
        }

        //protected void salespersonListBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string smIDStr12 = "";
        //    foreach (ListItem saleperson in salespersonListBox.Items)
        //    {
        //        if (saleperson.Selected)
        //        {
        //            smIDStr12 += saleperson.Value + ",";
        //        }
        //    }
        //    smIDStr12 = smIDStr12.TrimStart(',').TrimEnd(',');
        //    BindDistributorDDl(smIDStr12);
        //}
      

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string AreaStr="", RetailerStr1 = "", Qrychk = "", qrytr="", Query = "";

                foreach (ListItem item in AreaListBox.Items)
                {
                    if (item.Selected)
                    {
                        AreaStr += item.Value + ",";
                    }
                }
                AreaStr = AreaStr.TrimStart(',').TrimEnd(',');

                foreach (ListItem item in RetailerListBox.Items)
                {
                    if (item.Selected)
                    {
                        RetailerStr1 += item.Value + ",";
                    }
                }
                RetailerStr1 = RetailerStr1.TrimStart(',').TrimEnd(',');

                if(RetailerStr1 !="")
                {
                     qrytr = " WHERE TransRetailerLedger.DistId IN (" + RetailerStr1 + ") ";
                }
                else if (AreaStr !="")
                {
                    qrytr = " WHERE TransRetailerLedger.DistId IN (select PartyId from MastParty where AreaId in (" + AreaStr + ")) ";
                }
                else if (Session["treenodes"]!=null)
                {
                    qrytr = " WHERE TransRetailerLedger.DistId IN (select PartyId from MastParty where AreaId in (select AreaId from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Session["treenodes"] + ")))) and areatype='Area' and Active=1 ) ) ";
                }
                else
                {
                    qrytr = " WHERE TransRetailerLedger.DistId IN (select PartyId from MastParty where AreaId in (select AreaId from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")))) and areatype='Area' and Active=1 ) ) ";
                }

                Qrychk = " TransRetailerLedger.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                //if (RetailerStr1 != "")
                //{
                    if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                    {
                        if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                        {
                            //Query = "select q.DistId,q.Distributor,case when q.[oBalence]=0 then '-' when q.oBalence > 0 then CONVERT(varchar, q.oBalence) + ' Cr' else CONVERT(varchar, abs(q.oBalence)) + ' Dr' end [oBalance],sum (q.dr) as dr,sum(q.Cr) as cr,case when ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0)) = 0 then '-' when ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - q.dr) > 0 then CONVERT(varchar, ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0))) + ' Cr' else CONVERT(varchar, abs((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0))) + ' Dr' end [cBalance] from (select a.DistId,a.Distributor [Distributor],sum(a.oBalence) as [oBalence],SUM(a.dr) as dr,SUM(a.Cr) as Cr,SUM(a.cBalance) as CBalence FROM(select dl.DistId,d.PartyName as [Distributor],(select SUM(amtCr-amtDr) from TransDistributerLedger where VDate < '" + Settings.dateformat(txtfmDate.Text) + "' and d.PartyId= dl.DistId)  as [oBalence],0 as dr,0 as Cr,0 as cBalance from TransDistributerLedger dl inner join MastParty d  on d.PartyId=dl.DistId where (dl.DistId in (" + smIDStr1 + ") ) and VDate < '" + Settings.dateformat(txtfmDate.Text) + "'  group by dl.DistId,d.PartyName,d.PartyId union all select dl.DistId, d.PartyName as [Distributor], 0 as [oBalence],(select SUM( dl.AmtDr) from TransDistributerLedger dl where " + Qrychk + ") as dr,(select SUM( dl.Amtcr) from TransDistributerLedger dl where " + Qrychk + ") as Cr,(select sum(dl.AmtDr-dl.AmtCr) from TransDistributerLedger dl ) as cBalance from TransDistributerLedger dl inner join MastParty d on d.PartyId=dl.DistId where (dl.DistId in (" + smIDStr1 + ")) and " + Qrychk + " group by dl.DistId, d.PartyName) a group by a.DistId,a.Distributor) q group by q.DistId,q.Distributor,q.oBalence ,q.Cr,q.dr";
                            Query = "select TransRetailerLedger.DistId, partyname as Distributor, sum(Amtdr) AS dr, Sum(Amtcr) AS cr,(sum(AmtDr)-sum(AmtCr)) [cBalance] from TransRetailerLedger left join MastParty on MastParty.PartyId=TransRetailerLedger.DistId " + qrytr + " AND mastparty.PartyDist=0 and " + Qrychk + " group by DistId,PartyName order by Partyname";
                         //   string query = @"select TransRetailerLedger.DistId, partyname as Distributor, sum(Amtdr) AS Debit, Sum(Amtcr) AS Credit,(sum(AmtDr)-sum(AmtCr))  as Closing from TransRetailerLedger left join MastParty on MastParty.PartyId=TransRetailerLedger.DistId WHERE TransRetailerLedger.DistId IN (" + RetailerStr1 + ") AND mastparty.PartyDist=0 and " + Qrychk + " group by DistId,PartyName order by Partyname";
                            DataTable dtDistInvRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                            if (dtDistInvRep.Rows.Count > 0)
                            {
                                distreportrpt.DataSource = dtDistInvRep;
                                distreportrpt.DataBind();
                                detailDistrpt.Visible = false;
                                lblDist.Visible = false;
                                btnExport.Visible = true;

                            }
                            else
                            {
                                distreportrpt.DataSource = dtDistInvRep;
                                distreportrpt.DataBind();
                                detailDistrpt.Visible = false;
                                lblDist.Visible = false;
                                btnExport.Visible = false;
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
                            distreportrpt.DataSource = null;
                            distreportrpt.DataBind();
                        }
                    }
                //}
                //else
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Retailer');", true);
                //    distreportrpt.DataSource = null;
                //    distreportrpt.DataBind();
                //}
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RetailerLedgerReport.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");
            GetDetailLedgerData(Convert.ToInt32(hdnVisItCode.Value), txtfmDate.Text, txttodate.Text);
        }
        private void GetDetailLedgerData(int distId, string fromDate, string toDate)
        {
            try
            {
                string str = " select PartyName from MastParty where partyid= '" + distId + "'";
                //Ankita - 13/may/2016- (For Optimization)
                string partyname = DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();
                // DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                // if (dt1.Rows.Count > 0)
                // lblDist.Text = dt1.Rows[0]["PartyName"].ToString();
                lblDist.Text = partyname;

                //  lblDist.Text = Convert.ToString(distId);
                DataTable dt = DetailDistLedger(distId, fromDate, toDate);
                if (dt.Rows.Count > 0)
                {
                    detailDiv.Style.Add("display", "block");
                    detailDistrpt.Visible = true;
                    lblDist.Visible = true;
                    detailDistrpt.DataSource = dt;
                    detailDistrpt.DataBind();

                }
                else
                {
                    detailDiv.Style.Add("display", "block");
                    detailDistrpt.DataSource = dt;
                    detailDistrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public static DataTable DetailDistLedger(int DistId, string fromDate, string toDate)
        {

            DateTime fromTime = Convert.ToDateTime(fromDate);
            DateTime toTime = Convert.ToDateTime(toDate);
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@Distributor_Id", DbParameter.DbType.Int, 1, DistId);
            dbParam[1] = new DbParameter("@From_Date", DbParameter.DbType.DateTime, 1, fromTime);
            dbParam[2] = new DbParameter("@To_Date", DbParameter.DbType.DateTime, 1, toTime);
            return DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_selectRetailerLedger", dbParam);
        }       

        protected void btnExport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerLedgerReport.csv");
            string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Debit".TrimStart('"').TrimEnd('"') + "," + "Credit".TrimStart('"').TrimEnd('"') + "," + "Closing Balance".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Debit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Credit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ClosingBalance", typeof(String)));

            foreach (RepeaterItem item in distreportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
                dr["DistributorName"] = DistributorLabel.Text;
                Label drLabel = item.FindControl("drLabel") as Label;
                dr["Debit"] = drLabel.Text.ToString();
                Label CrLabel = item.FindControl("CrLabel") as Label;
                dr["Credit"] = CrLabel.Text.ToString();
                Label cBalanceLabel = item.FindControl("cBalanceLabel") as Label;
                dr["ClosingBalance"] = cBalanceLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {

                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {


                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else
                    {

                        sb.Append(dtParams.Rows[j][k].ToString() + ',');


                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerLedgerReport.csv");
            Response.Write(sb.ToString());
            Response.End();            
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
                    BindAreaDDl(smIDStr12);
                }
                else
                {
                    AreaListBox.Items.Clear();
                    AreaListBox.DataBind();
                }
                Session["treenodes"] = smIDStr12;

            }
            cnt = cnt + 1;
            return;
        }
        
        protected void ExportCSV(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerLedgerReport.csv");
            string headertext = "Retailer Name".TrimStart('"').TrimEnd('"') + "," + "Debit".TrimStart('"').TrimEnd('"') + "," + "Credit".TrimStart('"').TrimEnd('"') + "," + "Closing Balance".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
           
            DataTable dtParams = new DataTable();
            string AreaStr1 = "", RetailerStr1 = "", Qrychk = "", Query = "";
            DataTable dt = new DataTable();

            foreach (ListItem item in AreaListBox.Items)
            {
                if (item.Selected)
                {
                    AreaStr1 += item.Value + ",";
                }
            }
            AreaStr1 = AreaStr1.TrimStart(',').TrimEnd(',');

            foreach (ListItem item in RetailerListBox.Items)
            {
                if (item.Selected)
                {
                    RetailerStr1 += item.Value + ",";
                }
            }
            RetailerStr1 = RetailerStr1.TrimStart(',').TrimEnd(',');

            Qrychk = " TransRetailerLedger.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            if (AreaStr1 != "")
            {
                if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                    {
                        //Query = "select q.DistId,q.Distributor,case when q.[oBalence]=0 then '-' when q.oBalence > 0 then CONVERT(varchar, q.oBalence) + ' Cr' else CONVERT(varchar, abs(q.oBalence)) + ' Dr' end [oBalance],sum (q.dr) as dr,sum(q.Cr) as cr,case when ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0)) = 0 then '-' when ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - q.dr) > 0 then CONVERT(varchar, ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0))) + ' Cr' else CONVERT(varchar, abs((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0))) + ' Dr' end [cBalance] from (select a.DistId,a.Distributor [Distributor],sum(a.oBalence) as [oBalence],SUM(a.dr) as dr,SUM(a.Cr) as Cr,SUM(a.cBalance) as CBalence FROM(select dl.DistId,d.PartyName as [Distributor],(select SUM(amtCr-amtDr) from TransDistributerLedger where VDate < '" + Settings.dateformat(txtfmDate.Text) + "' and d.PartyId= dl.DistId)  as [oBalence],0 as dr,0 as Cr,0 as cBalance from TransDistributerLedger dl inner join MastParty d  on d.PartyId=dl.DistId where (dl.DistId in (" + smIDStr1 + ") ) and VDate < '" + Settings.dateformat(txtfmDate.Text) + "'  group by dl.DistId,d.PartyName,d.PartyId union all select dl.DistId, d.PartyName as [Distributor], 0 as [oBalence],(select SUM( dl.AmtDr) from TransDistributerLedger dl where " + Qrychk + ") as dr,(select SUM( dl.Amtcr) from TransDistributerLedger dl where " + Qrychk + ") as Cr,(select sum(dl.AmtDr-dl.AmtCr) from TransDistributerLedger dl ) as cBalance from TransDistributerLedger dl inner join MastParty d on d.PartyId=dl.DistId where (dl.DistId in (" + smIDStr1 + ")) and " + Qrychk + " group by dl.DistId, d.PartyName) a group by a.DistId,a.Distributor) q group by q.DistId,q.Distributor,q.oBalence ,q.Cr,q.dr";
                        Query = "select  partyname as Distributor, sum(Amtdr) AS Debit, Sum(Amtcr) AS Credit,(sum(AmtDr)-sum(AmtCr)) as Closing from TransRetailerLedger left join MastParty on MastParty.PartyId=TransRetailerLedger.DistId WHERE TransRetailerLedger.DistId IN (" + RetailerStr1 + ") AND mastparty.PartyDist=1 and " + Qrychk + " group by DistId,PartyName order by Partyname";

                        dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


                    }

                }
            }
            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {

                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {


                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else
                    {

                        sb.Append(dtParams.Rows[j][k].ToString() + ',');


                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=RetailerLedgerReport.csv");
            Response.Write(sb.ToString());
            Response.End();
        }       
       

    }
}