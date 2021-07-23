using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DAL;
using System.Web.UI.HtmlControls;

namespace AstralFFMS
{
    public partial class PurchaseOrderApproval : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        string parameter = "";
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter!=null)
            {
                Session["POID"] = null;
                Session["POID"] = Convert.ToInt32(parameter);
                Response.Redirect("~/ConfirmPurchaseOrder.aspx");
            }
            if (!Page.IsPostBack)
            {
                BindDistributor();
                //Ankita - 21/may/2016- (For Optimization)
               // GetRoleType(Settings.Instance.RoleID); 
                roleType = Settings.Instance.RoleType;
                //Added By - Abhishek 02/12/2015 UAT. Dated-04-12-2015
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                //End


                fillRepeter();

                
                txtmDate.Attributes.Add("ReadOnly", "true");
                txttodate.Attributes.Add("ReadOnly", "true");
            }
        }

        private void BindDistributor()
        {
            try
            {
                string str = @"SELECT T1.PartyId,T1.PartyName FROM MastParty AS T1 WHERE T1.PartyDist=1 AND T1.Active=1  and T1.CityId in (select AreaId from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp " +
                    " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) ORDER BY PartyName";

                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    //ddlDist.DataSource = obj;
                    //ddlDist.DataTextField = "PartyName";
                    //ddlDist.DataValueField = "PartyId";
                    //ddlDist.DataBind();
                    //ddlDist.Items.Insert(0, new ListItem("--Select--", "0"));
                    //ddlDist.Attributes.Add("CssClass", "form-control select2");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            int DistID = 0;
            string StatusVal = "0";

            if (ddlStatus.SelectedItem.Value != "0")
            {
                StatusVal = ddlStatus.SelectedItem.Value;
            }

            //if (hfCustomerId1.Value != "")
            //{
            //    DistID = Convert.ToInt32(hfCustomerId1.Value);
            //}
            if (txtDist1.Text != "")
            {
                if (hfCustomerId1.Value != "")
                {
                    DistID = Convert.ToInt32(hfCustomerId1.Value);

                }
                else
                {
                    DistID = 0;
                }
            }
            else
            {
                DistID = 0;
            }

            if (Convert.ToDateTime(txtmDate.Text) > Convert.ToDateTime(txttodate.Text))
            {
                rpt.DataSource = new DataTable();
                rpt.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }

            string mainQry = " and T1.Vdate between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

            if (txtmDate.Text != "" && txttodate.Text != "")
            {
                if (StatusVal == "A")
                {
                    StatusVal = "0";
                    str = @"SELECT T1.POrdId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId                           
                            where T2.Active=1 "+mainQry+" "+
                         @" and (T1.DistId=" + DistID + " OR " + DistID + "=0) and (T1.OrderStatus='" + StatusVal + "' OR " + StatusVal + "='0') ORDER BY T1.VDate desc";
                }
                else
                {
                    str = @"SELECT T1.POrdId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId                           
                            where T2.Active=1  " + mainQry + " " +
                         @" and (T1.DistId=" + DistID + " OR " + DistID + "=0) and (T1.OrderStatus='" + StatusVal + "') ORDER BY T1.VDate desc";
                }
             
                //str = @"select * from TransVisit where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' and VDate<='" + Settings.dateformat1(txttodate.Text) + "' order by VDate desc";
            }
            else if (txtmDate.Text != "")
            {
                //str = @"select * from TransVisit where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";

                if (StatusVal == "A")
                {
                    StatusVal = "0";
                    str = @"SELECT T1.POrdId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId                           
                            where T2.Active=1 "+mainQry+""+
                           " ORDER BY T1.VDate desc";
                }
                else
                {
                    str = @"SELECT T1.POrdId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId                           
                            where T2.Active=1 "+mainQry+" ORDER BY T1.VDate desc";
                }
               
            }
            else
            {
                if (StatusVal == "A")
                {
                    StatusVal = "0";
                    str = @"SELECT T1.POrdId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId                           
                            where T2.Active=1   and (T1.DistId=" + DistID + " OR " + DistID + "=0)  and (T1.OrderStatus='" + StatusVal + "' OR " + StatusVal + "='0') ORDER BY T1.VDate desc";
                }
                else
                {
                    str = @"SELECT T1.POrdId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId                           
                            where T2.Active=1 and (T1.DistId=" + DistID + " OR " + DistID + "=0)  and (T1.OrderStatus='" + StatusVal + "') ORDER BY T1.VDate desc";
                }
            }

            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (depdt != null && depdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("POrdId");
                dt.Columns.Add("VDate");
                dt.Columns.Add("PODocId");
                dt.Columns.Add("IGandDispatchTo");
                dt.Columns.Add("OrderStatus");
                dt.Columns.Add("PartyName");

                for (int i = 0; i < depdt.Rows.Count; i++)
                {
                    string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + Convert.ToInt32(depdt.Rows[i]["POrdId"]) + "";
                    DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                    if (dtUID != null && dtUID.Rows.Count > 0)
                    {
                        string strIG = @"SELECT T1.ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[dtUID.Rows.Count - 1]["Underid"]) + "";
                        DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);

                        if (dtIG != null && dtIG.Rows.Count > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["POrdId"] = depdt.Rows[i]["POrdId"];
                            dt.Rows[dt.Rows.Count - 1]["VDate"] = depdt.Rows[i]["VDate"];
                            dt.Rows[dt.Rows.Count - 1]["PODocId"] = depdt.Rows[i]["PODocId"];
                            dt.Rows[dt.Rows.Count - 1]["IGandDispatchTo"] = dtIG.Rows[0]["ItemName"].ToString() + "-(" + depdt.Rows[i]["PartyName"] + " " + depdt.Rows[i]["AreaName"] + ")";
                            dt.Rows[dt.Rows.Count - 1]["OrderStatus"] = depdt.Rows[i]["OrderStatus"];
                            dt.Rows[dt.Rows.Count - 1]["PartyName"] = depdt.Rows[i]["PartyName"];
                        }
                    }
                }

                rpt.DataSource = dt;
                rpt.DataBind();

                if(ddlStatus.SelectedValue=="M")
                {
                   
                    foreach (RepeaterItem ri in rpt.Items)
                    {
                        //HtmlTableCell th = (HtmlTableCell)ri.FindControl("tdDownload1");
                        ImageButton ImgDownload = (ImageButton)ri.FindControl("ImgDownload");
                        //HtmlTableRow th = (HtmlTableRow)ri.FindControl("tdDownload1");
                        ImgDownload.Visible = true;
                        //th.Visible = true;                       
                    }

                    //lblControl = rpt.Controls[0].Controls[0].FindControl("tdDownload1");
                   
                }
                else
                {
                    foreach (RepeaterItem ri in rpt.Items)
                    {
                        ImageButton ImgDownload = (ImageButton)ri.FindControl("ImgDownload");
                        //HtmlTableRow th = (HtmlTableRow)ri.FindControl("tdDownload1");
                        ImgDownload.Visible = false;                       
                        //th.Visible = false;  
                      
                    }
                }
              
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }
        private void fillRepeter()
        {
            string mainQry = " and T1.Vdate between '" + Settings.dateformat(txtmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

            string str = @"SELECT T1.POrdId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                                 WHEN T1.OrderStatus='P' THEN 'Open'
                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                            from TransPurchOrder T1 Left join MastParty T2 
                            on T1.DistId=T2.PartyId  
                            LEFT JOIN MastArea AS T3
                            ON T2.CityId = T3.AreaId                           
                            where T2.Active=1 "+mainQry+" and (T1.OrderStatus='P') ORDER BY T1.VDate desc";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (depdt != null && depdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("POrdId");
                dt.Columns.Add("VDate");
                dt.Columns.Add("PODocId");
                dt.Columns.Add("IGandDispatchTo");
                dt.Columns.Add("OrderStatus");
                dt.Columns.Add("PartyName");
                //dt.Columns.Add("Src");

                for (int i = 0; i < depdt.Rows.Count; i++)
                {
                    string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + Convert.ToInt32(depdt.Rows[i]["POrdId"]) + "";
                    DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                    if (dtUID != null && dtUID.Rows.Count > 0)
                    {
                        string strIG = @"SELECT T1.ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[dtUID.Rows.Count - 1]["Underid"]) + "";
                        DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);

                        if (dtIG != null && dtIG.Rows.Count > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["POrdId"] = depdt.Rows[i]["POrdId"];
                            dt.Rows[dt.Rows.Count - 1]["VDate"] = depdt.Rows[i]["VDate"];
                            dt.Rows[dt.Rows.Count - 1]["PODocId"] = depdt.Rows[i]["PODocId"];
                            dt.Rows[dt.Rows.Count - 1]["IGandDispatchTo"] = dtIG.Rows[0]["ItemName"].ToString() + "-(" + depdt.Rows[i]["PartyName"] + " " + depdt.Rows[i]["AreaName"] + ")";
                            dt.Rows[dt.Rows.Count - 1]["OrderStatus"] = depdt.Rows[i]["OrderStatus"];
                            dt.Rows[dt.Rows.Count - 1]["PartyName"] = depdt.Rows[i]["PartyName"];
                            //dt.Rows[dt.Rows.Count - 1]["Src"] = Convert.ToString("~/TextFileFolder/Dealer_Order" + Convert.ToString(dt.Rows[dt.Rows.Count - 1]["PODocId"]+".txt"));
                        }
                    }
                }

                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
           
        }

        protected void ImgDownload_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField POID = (HiddenField)item.FindControl("HiddenField1");
            HiddenField PODocID = (HiddenField)item.FindControl("HiddenField2");
            string strDocId = PODocID.Value;
            strDocId = strDocId.Replace(" ", "_");
            string strFileName = "Dealer_Order_" + strDocId;

            Response.Clear();
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName + "");
            Response.WriteFile(Server.MapPath(@"~/TextFileFolder/Dealer_Order_" + strDocId + ".txt"));
            Response.End();
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDistributor(string prefixText)
        {           
            if (Settings.Instance.RoleType == "Admin")
            { //Ankita - 21/may/2016- (For Optimization)
//                string str = @"select T1.*,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
//                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1 ORDER BY PartyName";
                string str = @"select T1.PartyName,T1.SyncId,T1.PartyId,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1 ORDER BY PartyName";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                List<string> customers = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["AreaName"].ToString() + ")" + " " + dt.Rows[i]["PartyName"].ToString() + " " + "(" + dt.Rows[i]["SyncId"].ToString() + ")", dt.Rows[i]["PartyId"].ToString());
                    customers.Add(item);
                }
                return customers;
            }
            else
            {
//                string str = @"select T1.*,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
//                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1  " +
//                              " and T1.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
//                      " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
                string str = @"select T1.PartyName,T1.SyncId,T1.PartyId,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1  " +
                            " and T1.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                    " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                List<string> customers = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["AreaName"].ToString() + ")" + " " + dt.Rows[i]["PartyName"].ToString() + " " + "(" + dt.Rows[i]["SyncId"].ToString() + ")", dt.Rows[i]["PartyId"].ToString());
                    customers.Add(item);
                }
                return customers;

            }
                //DataTable dt = new DataTable();
                //dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //List<string> customers = new List<string>();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["AreaName"].ToString() + ")" + " " + dt.Rows[i]["PartyName"].ToString() + " " + "(" + dt.Rows[i]["SyncId"].ToString() + ")", dt.Rows[i]["PartyId"].ToString());
                //    customers.Add(item);
                //}
                //return customers;
           
        }
        //private void GetRoleType(string p)
        //{
        //    try
        //    {               
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}


        protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField POID = (HiddenField)item.FindControl("HiddenField1");
            string str = POID.Value;
            Session["POIDPrint"] = str;
            Response.Redirect("~/PurchaseOrderReportPrint.aspx");

        }
      
    }
}