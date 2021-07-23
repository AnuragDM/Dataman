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
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace AstralFFMS
{
    public partial class CreateNewPurchaseOrder : System.Web.UI.Page
    {

        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        string parameter = "";
        string roleType = "";
        string VisitorsIPAddr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {           
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["POID"] = parameter;
                ViewState["POIDDel"] = parameter;
                BindProjects();
                //Ankita - 21/may/2016- (For Optimization)
               // GetRoleType(Settings.Instance.RoleID); 
                roleType = Settings.Instance.RoleType;
                FillPOData(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }

            if (!IsPostBack)
            {
                BindDistributor();
                BindItem();
                //BindCountry();
                BindTransPorter();
                BindCity();
                HttpContext.Current.Session["ItemData"] = null;
                //divdocid.Visible = false;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["POID"] != null)
                {
                    FillPOData(Convert.ToInt32(Request.QueryString["POID"]));
                }
                else
                {
                    btnSave.Text = "Save";
                }

                ViewState["POIDDel"] = Convert.ToInt32(Request.QueryString["POID"]);
                lblDate.Text = (Settings.GetUTCTime()).ToString("dd/MMM/yyyy");
                BindItemGrid();
                BindSchemes();
                BindProjects();                

                gvDetails.HeaderRow.Cells[1].Text = "Product" + "<span style='color:red;'>  *</span>";
                gvDetails.HeaderRow.Cells[3].Text = "Qty." + "<span style='color:red;'>  *</span>";
               
            }

            //Ankita - 13/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);         
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
               // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
           // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            
        }

        private void BindProjects()
        {
            try
            {
                string str = @"SELECT Id,Name FROM MastProject WHERE Active=1 Order By Name";
                DataTable obj = new DataTable();

                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlProject.DataSource = obj;
                    ddlProject.DataTextField = "Name";
                    ddlProject.DataValueField = "Id";
                    ddlProject.DataBind();

                }
                ddlProject.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {

            }
        }

        private void BindSchemes()
        {
            try
            {
                string str = @"SELECT Id,Name FROM MastScheme WHERE Active=1";
                DataTable obj = new DataTable();

                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlScheme.DataSource = obj;
                    ddlScheme.DataTextField = "Name";
                    ddlScheme.DataValueField = "Id";
                    ddlScheme.DataBind();

                }
                ddlScheme.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {

            }
        }

        private void BindCity()
        {
            try
            {
                //string str = @"SELECT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType ='CITY' AND T1.UnderId =" + Convert.ToInt32(ddlState.SelectedItem.Value) + "ORDER BY T1.AreaName";

                //string str = @"SELECT DISTINCT cityid,cityName FROM ViewGeo  where cityid!=0 and cityName!='' and CityId in (select AreaId from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp " +
                //   " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1)  ORDER BY cityName";

                string str = "";
                if (Settings.Instance.UserName.ToUpper() == "SA")
                {
                    //str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and T.cityAct=1"; 
                    str = "SELECT DISTINCT T.cityid,T.cityName FROM ViewGeo AS T WHERE T.cityid>0 and T.cityAct=1"; 
                }
                else
                {
                    str = @"SELECT DISTINCT cityid,cityName FROM ViewGeo  where cityid!=0 and cityAct=1 and cityName!='' and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                 " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1)  ORDER BY cityName";
                }
                

                //string str = @"SELECT DISTINCT cityid,cityName FROM ViewGeo  where cityid!=0 and cityName!='' ORDER BY cityName";

                DataTable obj = new DataTable();

                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlCity.DataSource = obj;
                    ddlCity.DataTextField = "cityName";
                    ddlCity.DataValueField = "cityid";
                    ddlCity.DataBind();

                }
                ddlCity.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {

            }
        }

        private void FillPOData(int POID)
        {
            string str = @"select * from TransPurchOrder T1 INNER JOIN MastParty T2 on T1.DistId=T2.PartyId where T1.POrdId=" + POID + "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt != null && dt.Rows.Count > 0)
            {
                BindTransPorter();
                if (Convert.ToString(dt.Rows[0]["Transporter"]).Trim() != "--Select--")
                {
                    ddlTransporter.SelectedValue = Convert.ToString(dt.Rows[0]["Transporter"]);
                }
                else
                {
                    ddlTransporter.SelectedValue = "0";
                }
                lblOrderSt.Text = Convert.ToString(dt.Rows[0]["PODocId"]);
                //BindCountry();
                txtDist.Text = Convert.ToString(dt.Rows[0]["PartyName"]);
                hfCustomerId.Value = Convert.ToString(dt.Rows[0]["DistId"]);
                DispName.Text = Convert.ToString(dt.Rows[0]["DispName"]);
                DispAdd1.Text = Convert.ToString(dt.Rows[0]["DispAdd1"]);
                DispAdd2.Text = Convert.ToString(dt.Rows[0]["DispAdd2"]);
                Pin.Text = Convert.ToString(dt.Rows[0]["DispPin"]);
                Phone.Text = Convert.ToString(dt.Rows[0]["DispPhone"]);
                Mobile.Text = Convert.ToString(dt.Rows[0]["DispMobile"]);
                Email.Text = Convert.ToString(dt.Rows[0]["DispEmail"]);
                txtCountry.Text = Convert.ToString(dt.Rows[0]["DispCountry"]);
                txtState.Text = Convert.ToString(dt.Rows[0]["DispState"]);
                BindCity();
                ddlCity.SelectedValue = Convert.ToString(dt.Rows[0]["DispCity"]);
                BindSchemes();

                try
                {
                    ddlScheme.SelectedValue = Convert.ToString(dt.Rows[0]["SchemeID"]);
                }
                catch (Exception ex)
                {

                    ddlScheme.SelectedValue = "0";
                }


                FillDetails(Convert.ToInt32(dt.Rows[0]["DistId"]));
                lblDate.Text = (Convert.ToDateTime(dt.Rows[0]["CreatedDate"])).ToString("dd/MMM/yyyy");
                Remarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]);

                if (Convert.ToString(dt.Rows[0]["OrderStatus"]) == "P")
                {
                    CancelOrder.Visible = true;
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                }
                else
                {
                    CancelOrder.Visible = false;
                    btnSave.Attributes.Add("disabled", "disabled");
                    btnDelete.Attributes.Add("disabled", "disabled");

                }

                if (Convert.ToString(dt.Rows[0]["ProjectType"]) == "N")
                {
                    ProjectType.Items[0].Selected = true;
                    ProjectDiv.Visible = false;
                    ddlProject.SelectedValue = "0";
                }
                if (Convert.ToString(dt.Rows[0]["ProjectType"]) == "P")
                {
                    string strProject = @"select T1.Name from MastProject T1 where T1.Id=" + Convert.ToInt32(dt.Rows[0]["ProjectID"]) + " ORDER BY T1.Name";
                    DataTable dtProject = DbConnectionDAL.GetDataTable(CommandType.Text, strProject);

                    ProjectType.Items[1].Selected = true;
                    ProjectDiv.Visible = true;
                    ddlProject.SelectedValue = Convert.ToString(dt.Rows[0]["ProjectID"]);
                    //txtProject.Text = Convert.ToString(dtProject.Rows[0]["Name"]);
                    //hfProjectID.Value = Convert.ToString(dt.Rows[0]["ProjectID"]);
                }

            }

            string str3 = @"select * from TransPurchOrder1 T1 
                            INNER JOIN MastItem T2
                            ON T1.ItemId=T2.ItemId where T1.POrdId=" + POID + "";

            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str3);

            if (dt1 != null && dt1.Rows.Count > 0)
            {
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("ItemID");
                dtItem.Columns.Add("ItemName");
                dtItem.Columns.Add("Qty");
                dtItem.Columns.Add("Unit");
                dtItem.Columns.Add("Rate");
                dtItem.Columns.Add("Amount");
                dtItem.Columns.Add("Remarks");
                dtItem.Columns.Add("StdPack");
                dtItem.Columns.Add("PriceGroup");

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemID"] = dt1.Rows[i]["ItemId"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["ItemName"] = "(" + dt1.Rows[i]["SyncId"].ToString() + ")" + " " + dt1.Rows[i]["ItemName"].ToString() + " " + "(" + dt1.Rows[i]["ItemCode"].ToString() + ")";
                    dtItem.Rows[dtItem.Rows.Count - 1]["Qty"] = dt1.Rows[i]["Qty"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Unit"] = dt1.Rows[i]["Unit"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Rate"] = dt1.Rows[i]["Rate"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["Amount"] = Math.Round((Convert.ToDecimal(dt1.Rows[i]["Qty"]) * Convert.ToDecimal(dt1.Rows[i]["MRP"])), 2);
                    dtItem.Rows[dtItem.Rows.Count - 1]["Remarks"] = dt1.Rows[i]["Remarks"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["StdPack"] = dt1.Rows[i]["StdPack"];
                    dtItem.Rows[dtItem.Rows.Count - 1]["PriceGroup"] = dt1.Rows[i]["PriceGroup"];
                }

                if (dtItem != null && dtItem.Rows.Count > 0)
                {
                    gvDetails.DataSource = dtItem;
                    gvDetails.DataBind();
                }

                decimal decAmtTotal = 0M;
                foreach (GridViewRow grow in gvDetails.Rows)
                {
                    Label lblAmt = (Label)grow.FindControl("lblAmount");
                    if (lblAmt.Text != "")
                    {
                        decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                    }
                }
                Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
            }

            btnSave.Text = "Update";
            btnDelete.Visible = true;

        }

        private void FillDetails(int DistID)
        {
            string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + DistID + "";
            DataTable obj1 = new DataTable();
            obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

            DataTable dt = new DataTable();
            string Query = "SELECT * FROM MastArea WHERE areaid IN(SELECT Maingrp FROM MastAreaGrp WHERE AreaId=" + Convert.ToInt32(obj1.Rows[0]["AreaId"]) + ")";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

            DataTable dtFinal = new DataTable();
            dtFinal.Columns.Add("Address1");
            dtFinal.Columns.Add("Address2");
            dtFinal.Columns.Add("CITY");
            dtFinal.Columns.Add("STATE");
            dtFinal.Columns.Add("COUNTRY");
            dtFinal.Columns.Add("Pin");
            dtFinal.Columns.Add("Mobile");
            dtFinal.Columns.Add("Email");
            dtFinal.Columns.Add("CreditLimit");
            dtFinal.Columns.Add("Outstanding");
            dtFinal.Columns.Add("OpenOrder");

            dtFinal.Rows.Add();

            dtFinal.Rows[0]["Address1"] = obj1.Rows[0]["Address1"];
            dtFinal.Rows[0]["Address2"] = obj1.Rows[0]["Address2"];

            dtFinal.Rows[0]["CreditLimit"] = obj1.Rows[0]["CreditLimit"];
            dtFinal.Rows[0]["Outstanding"] = obj1.Rows[0]["OutStanding"];
            dtFinal.Rows[0]["OpenOrder"] = obj1.Rows[0]["OpenOrder"];

            DataRow[] drow = dt.Select("AreaType='CITY'");
            if (drow != null && drow.Length > 0)
            {
                dtFinal.Rows[0]["CITY"] = drow[0]["AreaName"];
            }

            DataRow[] drow1 = dt.Select("AreaType='STATE'");

            if (drow != null && drow.Length > 0)
            {
                dtFinal.Rows[0]["STATE"] = drow1[0]["AreaName"];
            }

            DataRow[] drow2 = dt.Select("AreaType='COUNTRY'");

            if (drow != null && drow.Length > 0)
            {
                dtFinal.Rows[0]["COUNTRY"] = drow2[0]["AreaName"];
            }

            dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
            dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
            dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];

            if (dtFinal != null && dtFinal.Rows.Count > 0)
            {
                lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString();

                if (dtFinal.Rows[0]["Address2"].ToString() != "")
                {
                    lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString() + "," + dtFinal.Rows[0]["Address2"].ToString();
                }
                if (dtFinal.Rows[0]["CITY"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["CITY"].ToString();
                }
                if (dtFinal.Rows[0]["Pin"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + "-" + dtFinal.Rows[0]["Pin"].ToString();
                }
                if (dtFinal.Rows[0]["Mobile"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["Mobile"].ToString();
                }
                if (dtFinal.Rows[0]["Email"].ToString() != "")
                {
                    lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["Email"].ToString();
                }

                if (dtFinal.Rows[0]["CreditLimit"] != null || Convert.ToDecimal(dtFinal.Rows[0]["CreditLimit"]) != 0M)
                {
                    lblCreditLimit.Text = Convert.ToString(dtFinal.Rows[0]["CreditLimit"]);
                }
                else
                {
                    lblCreditLimit.Text = "0.00";
                }
                if (dtFinal.Rows[0]["Outstanding"] != null || Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) != 0M)
                {
                    if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) < 0)
                    {
                        lblOutstanding.Text = Convert.ToString(Math.Abs(Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]))) + " Cr.";
                    }
                    if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) > 0)
                    {
                        lblOutstanding.Text = Convert.ToString(dtFinal.Rows[0]["Outstanding"]) + " Dr.";
                    }
                    if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) == 0M)
                    {
                        lblOutstanding.Text = "0.00";
                    }

                }
                else
                {
                    lblOutstanding.Text = "0.00";
                }
                if (dtFinal.Rows[0]["OpenOrder"] != null || Convert.ToDecimal(dtFinal.Rows[0]["OpenOrder"]) != 0M)
                {
                    lblOpenOrders.Text = Convert.ToString(dtFinal.Rows[0]["OpenOrder"]);
                }
                else
                {
                    lblOpenOrders.Text = "0.00";
                }

            }
        }

        private void BindItemGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("StdPack");
            dt.Columns.Add("PriceGroup");
            dt.Rows.Add();

            if (dt != null && dt.Rows.Count > 0)
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
            }

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            BindDistributor();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
            //rpt.DataSource = new DataTable();
            //rpt.DataBind();
            ddlStatus.SelectedValue = "0";

        }

        private void fillRepeter()
        {//Ankita - 13/may/2016- (For Optimization)
            //            string str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
            //                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
            //                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
            //                                 WHEN T1.OrderStatus='P' THEN 'Open'
            //                                 WHEN T1.OrderStatus='M' THEN 'Processed'
            //                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
            //                            from TransPurchOrder T1 Left join MastParty T2 
            //                            on T1.DistId=T2.PartyId  
            //                            LEFT JOIN MastArea AS T3
            //                            ON T2.CityId = T3.AreaId                           
            //                            where T2.Active=1   and T1.SMId IN
            //                            (select SMId from MastSalesRep where 
            //                            smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
            //                              @" and (T1.OrderStatus='P') ORDER BY T1.PODocId ASC,T1.VDate desc";
            string str = "select * from [View_CreatePurchOrder]  where Active=1   and SMId IN  (select SMId from MastSalesRep where " +
                " smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
                              @" and (OrderStatus='P') ORDER BY PODocId ASC,VDate desc";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (depdt != null && depdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("POrdId");
                dt.Columns.Add("VDate");
                dt.Columns.Add("PODocId");
                dt.Columns.Add("IGandDispatchTo");
                dt.Columns.Add("OrderStatus");

                for (int i = 0; i < depdt.Rows.Count; i++)
                {
                    string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + Convert.ToInt32(depdt.Rows[i]["POrdId"]) + "";
                    DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                    if (dtUID != null && dtUID.Rows.Count > 0)
                    {
                        string strIG = @"SELECT T1.ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                        DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);

                        if (dtIG != null && dtIG.Rows.Count > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["POrdId"] = depdt.Rows[i]["POrdId"];
                            dt.Rows[dt.Rows.Count - 1]["VDate"] = depdt.Rows[i]["VDate"];
                            dt.Rows[dt.Rows.Count - 1]["PODocId"] = depdt.Rows[i]["PODocId"];
                            dt.Rows[dt.Rows.Count - 1]["IGandDispatchTo"] = dtIG.Rows[0]["ItemName"].ToString() + "-(" + depdt.Rows[i]["PartyName"] + " " + depdt.Rows[i]["AreaName"] + ")";
                            dt.Rows[dt.Rows.Count - 1]["OrderStatus"] = depdt.Rows[i]["OrderStatus"];
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

        private void BindTransPorter()
        {
            try
            {
                string str = @"SELECT T1.Name,T1.Id FROM MastTransporter AS T1 ORDER BY T1.Name";
                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    ddlTransporter.DataSource = obj;
                    ddlTransporter.DataTextField = "Name";
                    ddlTransporter.DataValueField = "Name";
                    ddlTransporter.DataBind();
                    ddlTransporter.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlTransporter.Items.Insert(ddlTransporter.Items.Count, new ListItem("Other", "Other"));
                }
                else
                {
                    ddlTransporter.DataSource = new DataTable();
                    ddlTransporter.DataBind();
                    ddlTransporter.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlTransporter.Items.Insert(1, new ListItem("Other", "Other"));
                }

            }
            catch (Exception ex)
            {

            }
        }

        //private void BindCountry()
        //{
        //    try
        //    {
        //        string str = @"SELECT DISTINCT countryid,countryName FROM ViewGeo ORDER BY countryName";
        //        DataTable obj = new DataTable();
        //        obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

        //        if (obj != null && obj.Rows.Count > 0)
        //        {
        //            ddlCountry.DataSource = obj;
        //            ddlCountry.DataTextField = "countryName";
        //            ddlCountry.DataValueField = "countryid";
        //            ddlCountry.DataBind();
        //            ddlCountry.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private void BindItem()
        {
            try
            {
                string str1 = @"SELECT T1.LastLvlItem FROM MastEnviro AS T1";
                DataTable obj1 = new DataTable();
                obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                string str = @"SELECT T1.ItemId,T1.ItemName FROM MastItem AS T1 WHERE T1.Lvl = " + Convert.ToInt32(obj1.Rows[0][0]) + " ORDER BY T1.ItemName";
                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (obj != null && obj.Rows.Count > 0)
                {
                    //ddlItem.DataSource = obj;
                    //ddlItem.DataTextField = "ItemName";
                    //ddlItem.DataValueField = "ItemId";
                    //ddlItem.DataBind();
                    //ddlItem.Items.Insert(0, new ListItem("--Select--", "0"));
                }

            }
            catch (Exception ex)
            {

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
                    ddlDist.DataSource = obj;
                    ddlDist.DataTextField = "PartyName";
                    ddlDist.DataValueField = "PartyId";
                    ddlDist.DataBind();
                    ddlDist.Items.Insert(0, new ListItem("--Select--", "0"));
                }



            }
            catch (Exception ex)
            {

            }
        }
        private void GetRoleType(string p)
        {
            try
            {
                string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
                DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
                if (roledt.Rows.Count > 0)
                {
                    roleType = roledt.Rows[0]["RoleType"].ToString();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        [WebMethod]
        public static string GetCustDetailsUP(int DistID)
        {
            string data1 = string.Empty;

            try
            {//Ankita - 13/may/2016- (For Optimization)
               // string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(DistID) + "";
                string str1 = @"SELECT T1.AreaId,T1.Address1,T1.Address2 ,T1.CreditLimit ,T1.OutStanding,T1.OpenOrder,T1.Pin,T1.Mobile,T1.Email FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(DistID) + "";
                DataTable obj1 = new DataTable();
                obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                DataTable dt = new DataTable();
             //   string Query = "SELECT * FROM MastArea WHERE areaid IN(SELECT Maingrp FROM MastAreaGrp WHERE AreaId=" + Convert.ToInt32(obj1.Rows[0]["AreaId"]) + ")";
                string Query = "SELECT AreaType FROM MastArea WHERE areaid IN(SELECT Maingrp FROM MastAreaGrp WHERE AreaId=" + Convert.ToInt32(obj1.Rows[0]["AreaId"]) + ")";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                DataTable dtFinal = new DataTable();
                dtFinal.Columns.Add("Address1");
                dtFinal.Columns.Add("Address2");
                dtFinal.Columns.Add("CITY");
                dtFinal.Columns.Add("STATE");
                dtFinal.Columns.Add("COUNTRY");
                dtFinal.Columns.Add("Pin");
                dtFinal.Columns.Add("Mobile");
                dtFinal.Columns.Add("Email");
                dtFinal.Columns.Add("CreditLimit");
                dtFinal.Columns.Add("Outstanding");
                dtFinal.Columns.Add("OpenOrder");

                dtFinal.Rows.Add();

                dtFinal.Rows[0]["Address1"] = obj1.Rows[0]["Address1"];
                dtFinal.Rows[0]["Address2"] = obj1.Rows[0]["Address2"];

                dtFinal.Rows[0]["CreditLimit"] = obj1.Rows[0]["CreditLimit"];
                dtFinal.Rows[0]["Outstanding"] = obj1.Rows[0]["OutStanding"];
                dtFinal.Rows[0]["OpenOrder"] = obj1.Rows[0]["OpenOrder"];

                DataRow[] drow = dt.Select("AreaType='CITY'");
                if (drow != null && drow.Length > 0)
                {
                    dtFinal.Rows[0]["CITY"] = drow[0]["AreaName"];
                }

                DataRow[] drow1 = dt.Select("AreaType='STATE'");

                if (drow != null && drow.Length > 0)
                {
                    dtFinal.Rows[0]["STATE"] = drow1[0]["AreaName"];
                }

                DataRow[] drow2 = dt.Select("AreaType='COUNTRY'");

                if (drow != null && drow.Length > 0)
                {
                    dtFinal.Rows[0]["COUNTRY"] = drow2[0]["AreaName"];
                }

                dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
                dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
                dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];

                var lst = dtFinal.AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>().Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])).ToDictionary(z => z.Key, z => z.Value)).ToList();
                //now serialize it
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                data1 = serializer.Serialize(lst);

                return data1;
            }
            catch
            {
                return data1;
            }


        }

        protected void ddlTransporter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransporter.SelectedItem.Value == "Other")
            {
                TP.Visible = true;
            }
            else
            {
                TP.Visible = false;
            }
        }

        protected void chkDispatchTo_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDist.Text != "")
            {

                if (chkDispatchTo.Checked)
                {//Ankita - 13/may/2016- (For Optimization)
                    //string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(hfCustomerId.Value) + "";
                    string str1 = @"SELECT T1.CityId,T1.Address1,T1.Address2,T1.CreditLimit,T1.OutStanding,T1.OpenOrder,T1.Pin,T1.Mobile,T1.Email FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(hfCustomerId.Value) + "";
                    DataTable obj1 = new DataTable();
                    obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                    DataTable dt = new DataTable();
                    string Query = "SELECT DISTINCT T.countryid,T.countryName,T.stateid,T.stateName,T.cityid,T.cityName FROM ViewGeo AS T WHERE T.cityid=" + Convert.ToInt32(obj1.Rows[0]["CityId"]) + "";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                    DataTable dtFinal = new DataTable();
                    dtFinal.Columns.Add("Address1");
                    dtFinal.Columns.Add("Address2");
                    dtFinal.Columns.Add("CITY");
                    dtFinal.Columns.Add("STATE");
                    dtFinal.Columns.Add("COUNTRY");
                    dtFinal.Columns.Add("Pin");
                    dtFinal.Columns.Add("Mobile");
                    dtFinal.Columns.Add("Email");
                    dtFinal.Columns.Add("CreditLimit");
                    dtFinal.Columns.Add("Outstanding");
                    dtFinal.Columns.Add("OpenOrder");

                    dtFinal.Rows.Add();

                    dtFinal.Rows[0]["Address1"] = obj1.Rows[0]["Address1"];
                    dtFinal.Rows[0]["Address2"] = obj1.Rows[0]["Address2"];

                    dtFinal.Rows[0]["CreditLimit"] = obj1.Rows[0]["CreditLimit"];
                    dtFinal.Rows[0]["Outstanding"] = obj1.Rows[0]["OutStanding"];
                    dtFinal.Rows[0]["OpenOrder"] = obj1.Rows[0]["OpenOrder"];

                    dtFinal.Rows[0]["CITY"] = dt.Rows[0]["cityid"];
                    dtFinal.Rows[0]["STATE"] = dt.Rows[0]["stateName"];
                    dtFinal.Rows[0]["COUNTRY"] = dt.Rows[0]["countryName"];
                    dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
                    dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
                    dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];


                    if (dtFinal != null && dtFinal.Rows.Count > 0)
                    {
                        DispName.Text = txtDist.Text.Trim();
                        DispAdd1.Text = Convert.ToString(dtFinal.Rows[0]["Address1"]);
                        DispAdd2.Text = Convert.ToString(dtFinal.Rows[0]["Address2"]);
                        Pin.Text = Convert.ToString(dtFinal.Rows[0]["Pin"]);
                        //Phone.Text = Convert.ToString(dtFinal.Rows[0]["Phone"]);
                        Mobile.Text = Convert.ToString(dtFinal.Rows[0]["Mobile"]);
                        Email.Text = Convert.ToString(dtFinal.Rows[0]["Email"]);
                        BindCity();

                        try
                        {
                            ddlCity.SelectedValue = Convert.ToString(dtFinal.Rows[0]["CITY"]);
                        }
                        catch (Exception ex)
                        {

                            ddlCity.SelectedValue = "0";
                        }

                        txtState.Text = Convert.ToString(dtFinal.Rows[0]["STATE"]);
                        txtCountry.Text = Convert.ToString(dtFinal.Rows[0]["COUNTRY"]);

                    }
                }
                else
                {
                    DispName.Text = "";
                    DispAdd1.Text = "";
                    DispAdd2.Text = "";
                    Pin.Text = "";
                    //Phone.Text = "";
                    Mobile.Text = "";
                    Email.Text = "";
                    //ddlCountry.SelectedValue = "0";
                    //ddlState.SelectedValue = "0";
                    txtState.Text = "";
                    txtCountry.Text = "";
                    ddlCity.SelectedValue = "0";
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select Distributor Name!');", true);
            }
        }

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDist.Text != "")
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("ItemID");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Qty");
                dt.Columns.Add("Unit");
                dt.Columns.Add("Rate");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("StdPack");
                dt.Columns.Add("PriceGroup");

                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                    TextBox txtItem = (TextBox)gvrow.FindControl("txtItem");
                    TextBox txtQty = (TextBox)gvrow.FindControl("txtQty");
                    Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                    Label lblRate = (Label)gvrow.FindControl("lblRate");
                    Label lblAmount = (Label)gvrow.FindControl("lblAmount");
                    Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                    TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");
                    Label lblPG = (Label)gvrow.FindControl("lblPG");

                    if (lblItemID.Text != "" && txtQty.Text != "")
                    {
                        dt.Rows.Add();

                        dt.Rows[dt.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["ItemName"] = txtItem.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Qty"] = txtQty.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Unit"] = lblUnit.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Rate"] = lblRate.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Amount"] = lblAmount.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Remarks"] = txtRemarks.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["StdPack"] = lblPacking.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["PriceGroup"] = lblPG.Text.Trim();
                    }
                }

                //if (ddlTransporter.SelectedItem.Value == "0")
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Transporter.');", true);
                //    return;
                //}

                if (ddlTransporter.SelectedValue == "Other")
                {
                    if (Transporter.Text == "")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Transporter.');", true);
                        return;
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (DispName.Text == "")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Dispatch Name.');", true);
                        return;
                    }
                    if (DispAdd1.Text == "")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Address 1.');", true);
                        return;
                    }
                    if (ddlCity.SelectedItem.Value == "0")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select City.');", true);
                        return;
                    }
                    if (Mobile.Text == "")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Mobile No.');", true);
                        return;
                    }

                    int MobLength = Mobile.Text.Length;

                    if (MobLength != 10)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter 10 digit Mobile No.');", true);
                        return;
                    }

                    if (Email.Text.Trim() != "")
                    {
                        if (!IsValidEmailAddress(Email.Text.Trim()))
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter valid email address.');", true);
                            return;
                        }
                    }

                    DateTime Currdt = Settings.GetUTCTime();
                    //context.sp_Getdocid("PORD", Currdt, ref DocID);
                    string docID = Settings.GetDocID("PORD", Currdt);
                    Settings.SetDocID("DISTP", docID);


                    int TransPorterID = 0;
                    string TransP = "";

                    if (ddlTransporter.SelectedItem.Value == "Other")
                    {//Ankita - 13/may/2016- (For Optimization)
                        //string strTP = @"SELECT T1.Name FROM MastTransporter AS T1 WHERE T1.Name='" + Transporter.Text.Trim() + "'";
                        string strTP = @"SELECT count(*) FROM MastTransporter AS T1 WHERE T1.Name='" + Transporter.Text.Trim() + "'";
                      //  DataTable dtTP = DbConnectionDAL.GetDataTable(CommandType.Text, strTP);
                        int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strTP));
                        //if (dtTP != null)
                        //{
                            if (cnt == 0)
                            {
                                TransPorterID = dp.InsertTP(Transporter.Text);
                                TransP = Transporter.Text;
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Transporter already exist.');", true);
                                return;
                            }
                        //}
                    }
                    else
                    {
                        TransPorterID = 0;
                        TransP = ddlTransporter.SelectedItem.Text;
                    }
                    int retSave = 0;

                    //string stateID = "0";
                    //string CityID = "0";

                    //if(ddlCountry.SelectedItem.Value!="0")
                    //{
                    //    stateID = ddlState.SelectedItem.Value;
                    //}
                    //if (stateID != "0")
                    //{
                    //    CityID = ddlCity.SelectedItem.Value;
                    //}

                    string PType = "";
                    int ProjectID = 0;
                    if (ProjectType.Items[0].Selected)
                    {
                        PType = "N";
                    }
                    else if (ProjectType.Items[1].Selected)
                    {
                        PType = "P";
                        ProjectID = Convert.ToInt32(ddlProject.SelectedItem.Value);
                    }

                    Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                    decimal decAmtTotal = Convert.ToDecimal(lblAmtTotal.Text.Trim());

                    int CityID = Convert.ToInt32(ViewState["CityID"]);
                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                    {
                        VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
                    }
                    if (btnSave.Text == "Save")
                    {
                        retSave = dp.Insert(Currdt, docID, Settings.Instance.UserID, Settings.Instance.SMID,
                            hfCustomerId.Value, TransPorterID, DispName.Text, DispAdd1.Text, DispAdd2.Text,
                            txtCountry.Text.Trim(), txtState.Text.Trim(), ddlCity.SelectedItem.Value, Pin.Text,
                            Phone.Text, Mobile.Text, Email.Text, dt, Remarks.Text, PType, ProjectID, decAmtTotal, CityID, TransP, Convert.ToInt32(ddlScheme.SelectedItem.Value.Trim()), VisitorsIPAddr);

                        if (retSave != 0)
                        {

                            SendEmailMessage(retSave, Currdt, docID, Settings.Instance.UserID, Settings.Instance.SMID,
                            hfCustomerId.Value, TransPorterID, DispName.Text, DispAdd1.Text, DispAdd2.Text,
                            txtCountry.Text.Trim(), txtState.Text.Trim(), ddlCity.SelectedItem.Value, Pin.Text,
                            Phone.Text, Mobile.Text, Email.Text, dt, Remarks.Text, PType, ProjectID, decAmtTotal, CityID, TransP, Convert.ToInt32(ddlScheme.SelectedItem.Value.Trim()));

                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully -" + docID + "');", true);
                            ClearControls();
                            btnDelete.Visible = false;
                           
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                        }
                    }
                    else
                    {
                        retSave = dp.Update(Convert.ToInt32(ViewState["POID"]), Currdt, docID, Settings.Instance.UserID, Settings.Instance.SMID, hfCustomerId.Value,
                            TransPorterID, DispName.Text, DispAdd1.Text, DispAdd2.Text, txtCountry.Text.Trim(), txtState.Text.Trim(),
                            ddlCity.SelectedItem.Value, Pin.Text, Phone.Text, Mobile.Text, Email.Text, dt, Remarks.Text, PType, ProjectID, decAmtTotal, CityID, TransP, Convert.ToInt32(ddlScheme.SelectedItem.Value.Trim()), VisitorsIPAddr);


                        if (retSave != 0)
                        { 
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully -" + lblOrderSt.Text.Trim() + "');", true);
                            ClearControls();
                            btnDelete.Visible = false;

                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                        }
                    }

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please add Product!');", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select Distributor Name!');", true);
            }
        }

        private void SendEmailMessage(int POID,DateTime Currdt, string docID, string UserID, string SMID, string CustID,
            int TransPorterID, string DispName, string DispAdd1, string DispAdd2, string Country, string State,
            string City, string Pin, string Phone, string Mobile, string Email, DataTable dtItem, string Remarks,
            string PType, int ProjectID, decimal decAmtTotal, int CityID, string TransPorter, int SchemeID)
        {
           string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.PurchaseEmailId,T1.PurchaseEmailCC,
                            T1.ChangePasswordEmailId,T1.ChangePasswordCC,T1.ForgetPasswordEmailId,T1.ForgetPasswordCC,
                            T1.OrderListEmailId,t1.OrderListEmailCC,T1.MailServer 
                            FROM MastEnviro AS T1";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt != null && dt.Rows.Count > 0)
            {
                string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='NewOrderCreated'";
                DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='NewOrderCreated'";
                DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                string[] emailToAll = new string[20];
                string[] emailCCAll = new string[20];
                string emailNew = "";
                string emailCC = "";

                emailNew = dt.Rows[0]["PurchaseEmailId"].ToString();
                emailToAll = emailNew.Split(';');
                emailCC = dt.Rows[0]["PurchaseEmailCC"].ToString();
                emailCCAll = emailCC.Split(';');

                string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                if (dtVar != null && dtVar.Rows.Count > 0)
                {
                    for (int j = 0; j < dtVar.Rows.Count; j++)
                    {
                        if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                        {
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderId}")
                            {
                                strSubject = strSubject.Replace("{OrderId}", docID);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strSubject = strSubject.Replace("{OrderDate}", Convert.ToString(Currdt));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strSubject = strSubject.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[0]["ItemCode"]));
                                //}

                                string strUID = @"SELECT isnull(T2.PriceGroup,'') AS PriceGroup,T2.ItemId,T1.POrd1Id FROM TransPurchOrder1 AS T1 
                                                    LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + " Order by T1.POrd1Id";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                strSubject = strSubject.Replace("{ItemGroupCode}", Convert.ToString(dtUID.Rows[dtUID.Rows.Count - 1]["PriceGroup"]));

                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupName}")
                            {
                                string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                if (dtUID != null && dtUID.Rows.Count > 0)
                                {
                                    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                    strSubject = strSubject.Replace("{ItemGroupName}", Convert.ToString(dtIG.Rows[0]["ItemName"]));
                                }
                            }

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(CustID) + "";
                            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyCode}")
                            {
                                strSubject = strSubject.Replace("{PartyCode}", Convert.ToString(dtDist.Rows[0]["SyncId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyName}")
                            {
                                strSubject = strSubject.Replace("{PartyName}", Convert.ToString(dtDist.Rows[0]["PartyName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add1}")
                            {
                                strSubject = strSubject.Replace("{Add1}", DispAdd1);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strSubject = strSubject.Replace("{Add2}", DispAdd2);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strSubject = strSubject.Replace("{City}", ddlCity.SelectedItem.Text.Trim());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strSubject = strSubject.Replace("{Pin}", Pin);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strSubject = strSubject.Replace("{State}", State);
                            }                           
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strSubject = strSubject.Replace("{OrderAmount}", Convert.ToString(decAmtTotal));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strSubject = strSubject.Replace("{Remarks}", Remarks);
                            }
                        }

                        ///////////////////////////////////////////
                        if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                        {
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderId}")
                            {
                                strMailBody = strMailBody.Replace("{OrderId}", docID);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderDate}")
                            {
                                strMailBody = strMailBody.Replace("{OrderDate}", Convert.ToString(Currdt));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupCode}")
                            {
                                //string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                //DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                //if (dtUID != null && dtUID.Rows.Count > 0)
                                //{
                                //    string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                //    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                //    strMailBody = strMailBody.Replace("{ItemGroupCode}", Convert.ToString(dtIG.Rows[0]["ItemCode"]));
                                //}

                                string strUID = @"SELECT isnull(T2.PriceGroup,'') AS PriceGroup,T2.ItemId,T1.POrd1Id FROM TransPurchOrder1 AS T1 
                                                    LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + " Order by T1.POrd1Id";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                strMailBody = strMailBody.Replace("{ItemGroupCode}", Convert.ToString(dtUID.Rows[dtUID.Rows.Count - 1]["PriceGroup"]));

                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemGroupName}")
                            {
                                string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + POID + "";
                                DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                                if (dtUID != null && dtUID.Rows.Count > 0)
                                { //Ankita - 21/may/2016- (For Optimization)
                                    //string strIG = @"SELECT * FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                    string strIG = @"SELECT ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                                    DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);
                                    strMailBody = strMailBody.Replace("{ItemGroupName}", Convert.ToString(dtIG.Rows[0]["ItemName"]));
                                }
                            }

                            string strDist = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(CustID) + "";
                            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, strDist);
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyCode}")
                            {
                                strMailBody = strMailBody.Replace("{PartyCode}", Convert.ToString(dtDist.Rows[0]["SyncId"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{PartyName}")
                            {
                                strMailBody = strMailBody.Replace("{PartyName}", Convert.ToString(dtDist.Rows[0]["PartyName"]));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add1}")
                            {
                                strMailBody = strMailBody.Replace("{Add1}", DispAdd1);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Add2}")
                            {
                                strMailBody = strMailBody.Replace("{Add2}", DispAdd2);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{City}")
                            {
                                strMailBody = strMailBody.Replace("{City}", ddlCity.SelectedItem.Text.Trim());
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Pin}")
                            {
                                strMailBody = strMailBody.Replace("{Pin}", Pin);
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{State}")
                            {
                                strMailBody = strMailBody.Replace("{State}", State);
                            }                            
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{OrderAmount}")
                            {
                                strMailBody = strMailBody.Replace("{OrderAmount}", Convert.ToString(decAmtTotal));
                            }
                            if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remarks}")
                            {
                                strMailBody = strMailBody.Replace("{Remarks}", Remarks);
                            }
                        }
                    }
                }

                //if(strMailBody.Contains(","))
                //{
                //    strMailBody = strMailBody.Replace(",","<br/>");
                //}

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Convert.ToString(dt.Rows[0]["SenderEmailId"]));
                mail.Subject = strSubject;
                mail.Body = strMailBody;
                // mail.Attachments.Add(new Attachment(Server.MapPath("TextFileFolder/Dealer_Order.txt")));
                NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(dt.Rows[0]["SenderEmailId"]), Convert.ToString(dt.Rows[0]["SenderPassword"]));

                if (emailToAll.Length > 0)
                {
                    for (int i = 0; i < emailToAll.Length; i++)
                    {
                        mail.To.Add(new MailAddress(emailToAll[i]));
                    }
                }
                if (emailCCAll.Length > 0)
                {
                    for (int j = 0; j < emailCCAll.Length; j++)
                    {
                        mail.CC.Add(new MailAddress(emailToAll[j]));
                    }
                }

                //Ankita - 13/may/2016- (For Optimization)
                // string strDistEmailID = @"SELECT * FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(CustID) + "";
                string strDistEmailID = @"SELECT Email,SMID FROM MastParty AS T WHERE T.PartyId=" + Convert.ToInt32(CustID) + "";
                DataTable dtDistEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistEmailID);
                mail.To.Add(new MailAddress(Convert.ToString(dtDistEmailID.Rows[0]["Email"])));

                string strSPEmailID = @"SELECT Email FROM MastSalesRep WHERE SMId=" + Convert.ToInt32(dtDistEmailID.Rows[0]["SMID"]) + "";
                //string strSPEmailID = @"SELECT * FROM MastSalesRep WHERE SMId=" + Convert.ToInt32(dtDistEmailID.Rows[0]["SMID"]) + "";

                //Ankita - 30/may/2016
                //DataTable dtSPEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strDistEmailID);
                DataTable dtSPEmailID = DbConnectionDAL.GetDataTable(CommandType.Text, strSPEmailID);
                mail.To.Add(new MailAddress(Convert.ToString(dtSPEmailID.Rows[0]["Email"])));

                try
                {
                    SmtpClient mailclient = new SmtpClient(Convert.ToString(dt.Rows[0]["MailServer"]), Convert.ToInt32(dt.Rows[0]["Port"]));
                    mailclient.EnableSsl = false;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);
                }

                catch(Exception ex)

                {
                    
                }
             
            }
        }

        private void ClearControls()
        {
            txtDist.Text = "";
            BindItemGrid();
            DispName.Text = "";
            DispAdd1.Text = "";
            DispAdd2.Text = "";
            Pin.Text = "";
            Phone.Text = "";
            Mobile.Text = "";
            Email.Text = "";
            //ddlCountry.SelectedValue = "0";
            //ddlState.SelectedValue = "0";
            txtCountry.Text = "";
            txtState.Text = "";
            ddlCity.SelectedValue = "0";
            lblDistDetails.Text = "";
            lblCreditLimit.Text = "";
            lblOutstanding.Text = "";
            lblOpenOrders.Text = "";
            lblDate.Text = (Settings.GetUTCTime()).ToString("dd/MMM/yyyy");
            Remarks.Text = "";
            BindTransPorter();
            TP.Visible = false;
            Transporter.Text = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            chkDispatchTo.Checked = false;
            lblOrderSt.Text = "New";
            ProjectType.Items[0].Selected = true;
            //txtProject.Text = "";
            ddlProject.SelectedValue = "0";
            ProjectDiv.Visible = false;
            ddlScheme.SelectedValue = "0";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Convert.ToInt32(ViewState["POIDDel"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();

                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDistributor(string prefixText)
        {        

             string str = "";
             //if (Settings.Instance.UserName.ToUpper() == "SA")
             //Ankita - 21/may/2016- (For Optimization)
             if (Settings.Instance.RoleType == "Admin")
             {
//                 str = @"select T1.*,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
//                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1 ORDER BY PartyName ";
                 str = @"select T1.PartyName,T1.SyncId,T1.PartyId,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2 ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1 ORDER BY PartyName ";   
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
//                 str = @"select T1.*,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
//                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1  " +
//                         " and T1.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
//                 " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
                 str = @"select T1.PartyName,T1.SyncId,T1.PartyId,T2.AreaName  FROM MastParty AS T1 INNER JOIN MastArea AS T2
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
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchProject(string prefixText)
        {
            //Ankita - 13/may/2016- (For Optimization)
            //  string str = "select * FROM MastProject where (Name like '%" + prefixText + "%') and Active=1 ORDER BY Name";
            string str = "select Id,Name FROM MastProject where (Name like '%" + prefixText + "%') and Active=1 ORDER BY Name";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> Projects = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Id"].ToString());
                Projects.Add(item);
            }
            return Projects;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {
            //string str1 = @"SELECT T1.LastLvlItem FROM MastEnviro AS T1";
            //DataTable obj1 = new DataTable();
            //obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

            //string str = @"SELECT T1.ItemId,T1.ItemName FROM MastItem AS T1 WHERE T1.Lvl = " + Convert.ToInt32(obj1.Rows[0][0]) + " and (T1.ItemName like '%" + prefixText + "%') ORDER BY T1.ItemName";

            //Ankita - 13/may/2016- (For Optimization)
            string str = "select SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";

            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> Items = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                Items.Add(item);
                //customers.Add("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")");
            }
            return Items;
        }

        protected void txtDist_TextChanged(object sender, EventArgs e)
        {
            if (hfCustomerId.Value != "")
            {
                lblDistDetails.Text = "";
                lblCreditLimit.Text = "";
                lblOutstanding.Text = "";
                lblOpenOrders.Text = "";
                lblDate.Text = "";
                //Ankita - 13/may/2016- (For Optimization)
                //string str1 = @"SELECT T1.* FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(hfCustomerId.Value) + "";
                string str1 = @"SELECT T1.CityId,T1.Address1,T1.Address2,T1.CreditLimit,T1.OutStanding,T1.OpenOrder,T1.Pin,T1.Mobile,T1.Email FROM MastParty AS T1 WHERE T1.PartyId=" + Convert.ToInt32(hfCustomerId.Value) + "";
                DataTable obj1 = new DataTable();
                obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                ViewState["CityID"] = obj1.Rows[0]["CityId"];

                DataTable dt = new DataTable();
                string Query = "SELECT DISTINCT T.countryid,T.countryName,T.stateid,T.stateName,T.cityid,T.cityName FROM ViewGeo AS T WHERE T.cityid=" + Convert.ToInt32(obj1.Rows[0]["CityId"]) + "";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
			

                DataTable dtFinal = new DataTable();
                dtFinal.Columns.Add("Address1");
                dtFinal.Columns.Add("Address2");
                dtFinal.Columns.Add("CITY");
                dtFinal.Columns.Add("STATE");
                dtFinal.Columns.Add("COUNTRY");
                dtFinal.Columns.Add("Pin");
                dtFinal.Columns.Add("Mobile");
                dtFinal.Columns.Add("Email");
                dtFinal.Columns.Add("CreditLimit");
                dtFinal.Columns.Add("Outstanding");
                dtFinal.Columns.Add("OpenOrder");

                dtFinal.Rows.Add();

                dtFinal.Rows[0]["Address1"] = obj1.Rows[0]["Address1"];
                dtFinal.Rows[0]["Address2"] = obj1.Rows[0]["Address2"];

                dtFinal.Rows[0]["CreditLimit"] = obj1.Rows[0]["CreditLimit"];
                dtFinal.Rows[0]["Outstanding"] = obj1.Rows[0]["OutStanding"];
                dtFinal.Rows[0]["OpenOrder"] = obj1.Rows[0]["OpenOrder"];

                dtFinal.Rows[0]["CITY"] = dt.Rows[0]["cityName"];
                dtFinal.Rows[0]["STATE"] = dt.Rows[0]["stateName"];
                dtFinal.Rows[0]["COUNTRY"] = dt.Rows[0]["countryName"];
                dtFinal.Rows[0]["Pin"] = obj1.Rows[0]["Pin"];
                dtFinal.Rows[0]["Mobile"] = obj1.Rows[0]["Mobile"];
                dtFinal.Rows[0]["Email"] = obj1.Rows[0]["Email"];

                if (dtFinal != null && dtFinal.Rows.Count > 0)
                {
                    lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString();

                    if (dtFinal.Rows[0]["Address2"].ToString() != "")
                    {
                        lblDistDetails.Text = dtFinal.Rows[0]["Address1"].ToString() + ", " + dtFinal.Rows[0]["Address2"].ToString();
                    }
                    if (dtFinal.Rows[0]["CITY"].ToString() != "")
                    {
                        lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["CITY"].ToString();
                    }
                    if (dtFinal.Rows[0]["Pin"].ToString() != "")
                    {
                        lblDistDetails.Text = lblDistDetails.Text + "-" + dtFinal.Rows[0]["Pin"].ToString();
                    }
                    if (dtFinal.Rows[0]["Mobile"].ToString() != "")
                    {
                        lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["Mobile"].ToString();
                    }
                    if (dtFinal.Rows[0]["Email"].ToString() != "")
                    {
                        lblDistDetails.Text = lblDistDetails.Text + ", " + dtFinal.Rows[0]["Email"].ToString();
                    }

                    if (dtFinal.Rows[0]["CreditLimit"] != null || Convert.ToDecimal(dtFinal.Rows[0]["CreditLimit"]) != 0M)
                    {
                        lblCreditLimit.Text = Convert.ToString(dtFinal.Rows[0]["CreditLimit"]);
                    }
                    else
                    {
                        lblCreditLimit.Text = "0.00";
                    }
                    if (dtFinal.Rows[0]["Outstanding"] != null || Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) != 0M)
                    {
                        if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) < 0)
                        {
                            lblOutstanding.Text = Convert.ToString(Math.Abs(Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]))) + " Cr.";
                        }
                        if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) > 0)
                        {
                            lblOutstanding.Text = Convert.ToString(dtFinal.Rows[0]["Outstanding"]) + " Dr.";
                        }
                        if (Convert.ToDecimal(dtFinal.Rows[0]["Outstanding"]) == 0M)
                        {
                            lblOutstanding.Text = "0.00";
                        }

                    }
                    else
                    {
                        lblOutstanding.Text = "0.00";
                    }
                    if (dtFinal.Rows[0]["OpenOrder"] != null || Convert.ToDecimal(dtFinal.Rows[0]["OpenOrder"]) != 0M)
                    {
                        lblOpenOrders.Text = Convert.ToString(dtFinal.Rows[0]["OpenOrder"]);
                    }
                    else
                    {
                        lblOpenOrders.Text = "0.00";
                    }

                    lblDate.Text = (Settings.GetUTCTime()).ToString("dd/MMM/yyyy");

                    chkDispatchTo.Checked = true;
                    DispName.Text = txtDist.Text.Trim();
                    DispAdd1.Text = Convert.ToString(dtFinal.Rows[0]["Address1"]);
                    DispAdd2.Text = Convert.ToString(dtFinal.Rows[0]["Address2"]);
                    Pin.Text = Convert.ToString(dtFinal.Rows[0]["Pin"]);
                    //Phone.Text = Convert.ToString(dtFinal.Rows[0]["Phone"]);
                    Mobile.Text = Convert.ToString(dtFinal.Rows[0]["Mobile"]);
                    Email.Text = Convert.ToString(dtFinal.Rows[0]["Email"]);
                    BindCity();

                    try
                    {
                        ddlCity.SelectedValue = Convert.ToString(dt.Rows[0]["cityid"]);
                    }
                    catch (Exception ex)
                    {

                        ddlCity.SelectedValue = "0";
                    }

                    txtState.Text = Convert.ToString(dtFinal.Rows[0]["STATE"]);
                    txtCountry.Text = Convert.ToString(dtFinal.Rows[0]["COUNTRY"]);


                }
            }
            else
            {
                txtDist.Focus();
            }

        }

        protected void txtItem_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            bool flag = true;
            if (hfItemId.Value != "")
            {
                if (txt.Text != "")
                {
                    foreach (GridViewRow gvrow in gvDetails.Rows)
                    {
                        Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                        if (lblItemID.Text != "")
                        {
                            if (Convert.ToDecimal(hfItemId.Value) == (Convert.ToDecimal(lblItemID.Text)))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        string str1 = @"SELECT T1.ItemId,T1.ItemName,T1.Unit,T1.MRP,T1.StdPack,T1.PriceGroup  
                                    FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(hfItemId.Value) + "";
                        DataTable obj1 = new DataTable();
                        obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                        if (obj1 != null && obj1.Rows.Count > 0)
                        {

                            GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
                            Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                            Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                            Label lblRate = (Label)gvrow.FindControl("lblRate");
                            Label lblUnit1 = (Label)gvrow.FindControl("lblUnit1");
                            Label lblRate1 = (Label)gvrow.FindControl("lblRate1");
                            Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                            Label lblPG = (Label)gvrow.FindControl("lblPG");
                            TextBox txtQty = (TextBox)gvrow.FindControl("txtQty");


                            lblUnit.Text = Convert.ToString(obj1.Rows[0]["Unit"]);
                            lblRate.Text = Convert.ToString(obj1.Rows[0]["MRP"]);
                            lblUnit1.Text = Convert.ToString(obj1.Rows[0]["Unit"]);
                            lblRate1.Text = Convert.ToString(obj1.Rows[0]["MRP"]);
                            lblPacking.Text = Convert.ToString(Math.Round(Convert.ToDecimal(obj1.Rows[0]["StdPack"]),0));
                            lblPG.Text = Convert.ToString(obj1.Rows[0]["PriceGroup"]);
                            lblItemID.Text = hfItemId.Value;
                            hfItemId.Value = "";
                            txtQty.Focus();
                        }

                    }
                    else
                    {
                        txt.Text = "";
                        hfItemId.Value = "";

                        GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
                        Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                        Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                        Label lblRate = (Label)gvrow.FindControl("lblRate");
                        Label lblUnit1 = (Label)gvrow.FindControl("lblUnit1");
                        Label lblRate1 = (Label)gvrow.FindControl("lblRate1");
                        Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                        Label lblPG = (Label)gvrow.FindControl("lblPG");
                        txt.Focus();
                        lblItemID.Text = "";

                        if (lblUnit.Text == "")
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Product already exist!');", true);
                        }

                    }
                }
                else
                {
                    GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
                    Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                    Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                    Label lblRate = (Label)gvrow.FindControl("lblRate");
                    Label lblUnit1 = (Label)gvrow.FindControl("lblUnit1");
                    Label lblRate1 = (Label)gvrow.FindControl("lblRate1");
                    Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                    Label lblPG = (Label)gvrow.FindControl("lblPG");
                    TextBox txtQty = (TextBox)gvrow.FindControl("txtQty");

                    lblUnit.Text = "";
                    lblRate.Text = "";
                    lblUnit1.Text = "";
                    lblRate1.Text = "";
                    lblPacking.Text = "";
                    lblItemID.Text = "";
                    hfItemId.Value = "";
                    lblPG.Text = "";
                    txtQty.Text = "";
                    txt.Focus();
                }
            }
            else
            {
                GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
                Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                Label lblRate = (Label)gvrow.FindControl("lblRate");
                Label lblUnit1 = (Label)gvrow.FindControl("lblUnit1");
                Label lblRate1 = (Label)gvrow.FindControl("lblRate1");
                Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                Label lblAmount = (Label)gvrow.FindControl("lblAmount");
                Label lblPG = (Label)gvrow.FindControl("lblPG");
                TextBox txtQty = (TextBox)gvrow.FindControl("txtQty");
                Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                lblAmtTotal.Text = "";
                lblUnit.Text = "";
                lblRate.Text = "";
                lblUnit1.Text = "";
                lblRate1.Text = "";
                lblPacking.Text = "";
                lblItemID.Text = "";
                hfItemId.Value = "";
                lblPG.Text = "";
                txtQty.Text = "";
                lblAmount.Text = "";
                txt.Focus();
            }

        }

        protected void Add_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("StdPack");
            dt.Columns.Add("PriceGroup");
            bool flag = false;

            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                Label lblItemID = (Label)gvrow.FindControl("lblItemID");
                TextBox txtItem = (TextBox)gvrow.FindControl("txtItem");
                TextBox txtQty = (TextBox)gvrow.FindControl("txtQty");
                Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                Label lblRate = (Label)gvrow.FindControl("lblRate");
                Label lblUnit1 = (Label)gvrow.FindControl("lblUnit1");
                Label lblRate1 = (Label)gvrow.FindControl("lblRate1");
                Label lblAmount = (Label)gvrow.FindControl("lblAmount");
                TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");
                Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                Label lblPG = (Label)gvrow.FindControl("lblPG");


                if (lblItemID.Text != "")
                {
                    if (txtQty.Text != "")
                    {

                        int QtyCheck = 0;
                        decimal decQtyCheck = 0M;

                        if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
                        {
                            QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txtQty.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
                            decQtyCheck = Convert.ToDecimal(txtQty.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
                        }

                        if ((QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
                        {
                            flag = true;
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["ItemName"] = txtItem.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Qty"] = txtQty.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Unit"] = lblUnit.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Rate"] = lblRate.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Amount"] = lblAmount.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Remarks"] = txtRemarks.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["StdPack"] = lblPacking.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["PriceGroup"] = lblPG.Text.Trim();
                        }
                        else
                        {
                            txtQty.Focus();
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
                        }
                    }
                    else
                    {
                        dt.Rows.Add();

                        dt.Rows[dt.Rows.Count - 1]["ItemID"] = lblItemID.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["ItemName"] = txtItem.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Qty"] = txtQty.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Unit"] = lblUnit.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Rate"] = lblRate.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Amount"] = lblAmount.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["Remarks"] = txtRemarks.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["StdPack"] = lblPacking.Text.Trim();
                        dt.Rows[dt.Rows.Count - 1]["PriceGroup"] = lblPG.Text.Trim();

                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();
                        flag = false;
                        decimal decAmtTotal = 0M;
                        foreach (GridViewRow grow in gvDetails.Rows)
                        {
                            Label lblAmt = (Label)grow.FindControl("lblAmount");
                            if (lblAmt.Text != "")
                            {
                                decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                            }
                        }
                        Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                        lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity for selected Product!');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please add Product!');", true);
                }

            }

            if (flag)
            {

                dt.Rows.Add();
                gvDetails.DataSource = dt;
                gvDetails.DataBind();

                Session["ItemData"] = dt;

                decimal decAmtTotal = 0M;
                foreach (GridViewRow grow in gvDetails.Rows)
                {
                    Label lblAmt = (Label)grow.FindControl("lblAmount");
                    if (lblAmt.Text != "")
                    {
                        decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                    }
                }
                Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
            }
        }

        protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ImgDelete = (ImageButton)sender;
            GridViewRow grow = (GridViewRow)ImgDelete.NamingContainer;
            Label lblItemID1 = (Label)grow.FindControl("lblItemID");

            if (lblItemID1.Text != "")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemID");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("Qty");
                dt.Columns.Add("Unit");
                dt.Columns.Add("Rate");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("StdPack");
                dt.Columns.Add("PriceGroup");


                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    Label lblEachItemID = (Label)gvrow.FindControl("lblItemID");
                    TextBox txtItem = (TextBox)gvrow.FindControl("txtItem");
                    TextBox txtQty = (TextBox)gvrow.FindControl("txtQty");
                    Label lblUnit = (Label)gvrow.FindControl("lblUnit");
                    Label lblRate = (Label)gvrow.FindControl("lblRate");
                    Label lblAmount = (Label)gvrow.FindControl("lblAmount");
                    TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");
                    Label lblPacking = (Label)gvrow.FindControl("lblPacking");
                    Label lblPG = (Label)gvrow.FindControl("lblPG");

                    if (lblEachItemID.Text != "")
                    {
                        if (Convert.ToDecimal(lblEachItemID.Text.Trim()) != Convert.ToDecimal(lblItemID1.Text.Trim()))
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["ItemID"] = lblEachItemID.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["ItemName"] = txtItem.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Qty"] = txtQty.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Unit"] = lblUnit.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Rate"] = lblRate.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Amount"] = lblAmount.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["Remarks"] = txtRemarks.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["StdPack"] = lblPacking.Text.Trim();
                            dt.Rows[dt.Rows.Count - 1]["PriceGroup"] = lblPG.Text.Trim();

                            gvDetails.DataSource = dt;
                            gvDetails.DataBind();

                            Session["ItemData"] = dt;
                            decimal decAmtTotal = 0M;
                            foreach (GridViewRow grow1 in gvDetails.Rows)
                            {
                                Label lblAmt = (Label)grow1.FindControl("lblAmount");
                                if (lblAmt.Text != "")
                                {
                                    decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                                }
                            }
                            Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                            lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        BindItemGrid();
                    }
                }

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please add Product before performing deletion!');", true);
            }

        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)txt.NamingContainer;
            Label lblUnit = (Label)gvrow.FindControl("lblUnit");
            Label lblRate = (Label)gvrow.FindControl("lblRate");
            Label lblAmount = (Label)gvrow.FindControl("lblAmount");
            Label lblPacking = (Label)gvrow.FindControl("lblPacking");
            TextBox txtRemarks = (TextBox)gvrow.FindControl("txtRemarks");
            TextBox txtItem = (TextBox)gvrow.FindControl("txtItem");


            if (lblRate.Text != "")
            {
                if (txt.Text != "")
                {
                    int QtyCheck = 0;
                    decimal decQtyCheck = 0M;

                    if (Convert.ToInt32(lblPacking.Text.Trim()) != 0)
                    {
                        QtyCheck = Convert.ToInt32(Math.Round(Convert.ToDecimal(txt.Text.Trim()), 0)) / Convert.ToInt32(lblPacking.Text.Trim());
                        decQtyCheck = Convert.ToDecimal(txt.Text.Trim()) / Convert.ToDecimal(lblPacking.Text.Trim());
                    }

                    if ((QtyCheck > 0 && decQtyCheck > 0M && QtyCheck == decQtyCheck) || Convert.ToInt32(lblPacking.Text.Trim()) == 0)
                    {
                        decimal decamt = Math.Round((Convert.ToDecimal(txt.Text.Trim()) * Convert.ToDecimal(lblRate.Text.Trim())), 2);
                        lblAmount.Text = Convert.ToString(decamt);

                        decimal decAmtTotal = 0M;
                        foreach (GridViewRow grow in gvDetails.Rows)
                        {
                            Label lblAmt = (Label)grow.FindControl("lblAmount");
                            if (lblAmt.Text != "")
                            {
                                decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                            }
                        }
                        Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                        lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
                        txtRemarks.Focus();

                    }
                    else
                    {
                        int PackingQty = Convert.ToInt32(lblPacking.Text.Trim());
                        int OrdQty = Convert.ToInt32(txt.Text.Trim());
                        int EnterQty = Convert.ToInt32(txt.Text.Trim()) - Convert.ToInt32(lblPacking.Text.Trim());

                        if(EnterQty<=0)
                        {
                            txt.Text = Convert.ToString(lblPacking.Text.Trim());
                        }
                        else
                        {
                            txt.Text = Convert.ToString((Convert.ToInt32(lblPacking.Text.Trim()) - Convert.ToInt32(txt.Text.Trim()) % Convert.ToInt32(lblPacking.Text.Trim())) + Convert.ToInt32(txt.Text.Trim()));
                            //return (10 - toRound % 10) + toRound;
                        }
                        decimal decamt = Math.Round((Convert.ToDecimal(txt.Text.Trim()) * Convert.ToDecimal(lblRate.Text.Trim())), 2);
                        lblAmount.Text = Convert.ToString(decamt);

                        decimal decAmtTotal = 0M;
                        foreach (GridViewRow grow in gvDetails.Rows)
                        {
                            Label lblAmt = (Label)grow.FindControl("lblAmount");
                            if (lblAmt.Text != "")
                            {
                                decAmtTotal += Convert.ToDecimal(lblAmt.Text);
                            }
                        }
                        Label lblAmtTotal = (Label)gvDetails.FooterRow.FindControl("lblAmtTotal");
                        lblAmtTotal.Text = Convert.ToString(Math.Round(decAmtTotal, 2));
                        txtRemarks.Focus();                        
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity in multiple of packing!');", true);
                    }
                }
                else
                {
                    txt.Focus();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please enter quantity value!');", true);
                }
            }
            else
            {
                txtItem.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "errormessage('Please select Product!');", true);
            }

        }

        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string str = @"SELECT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType ='STATE' AND T1.UnderId ="+Convert.ToInt32(ddlCountry.SelectedItem.Value)+"ORDER BY T1.AreaName";

        //        string str = @"SELECT DISTINCT stateid,stateName FROM ViewGeo WHERE countryid=" + Convert.ToInt32(ddlCountry.SelectedItem.Value) + "  ORDER BY stateName";

        //        DataTable obj = new DataTable();

        //        obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

        //        if (obj != null && obj.Rows.Count > 0)
        //        {
        //            ddlState.DataSource = obj;
        //            ddlState.DataTextField = "stateName";
        //            ddlState.DataValueField = "stateid";
        //            ddlState.DataBind();
        //            ddlState.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string str = @"SELECT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType ='CITY' AND T1.UnderId =" + Convert.ToInt32(ddlState.SelectedItem.Value) + "ORDER BY T1.AreaName";

        //        string str = @"SELECT DISTINCT cityid,cityName FROM ViewGeo WHERE stateid=" + Convert.ToInt32(ddlState.SelectedItem.Value) + " ORDER BY cityName";

        //        DataTable obj = new DataTable();

        //        obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

        //        if (obj != null && obj.Rows.Count > 0)
        //        {
        //            ddlCity.DataSource = obj;
        //            ddlCity.DataTextField = "cityName";
        //            ddlCity.DataValueField = "cityid";
        //            ddlCity.DataBind();
        //            ddlCity.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = @"SELECT DISTINCT T1.cityid,T1.cityName,T1.stateid,T1.stateName,T1.countryid,T1.countryName
                                                                            FROM ViewGeo AS T1 WHERE T1.cityid=" + Convert.ToInt32(ddlCity.SelectedItem.Value) + " and T1.cityid!=0 and T1.cityName!=''";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt != null && dt.Rows.Count > 0)
            {
                txtState.Text = Convert.ToString(dt.Rows[0]["stateName"]);
                txtCountry.Text = Convert.ToString(dt.Rows[0]["countryName"]);
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

            if (ddlDist.SelectedItem.Value != "0")
            {
                DistID = Convert.ToInt32(ddlDist.SelectedItem.Value);
            }

            if (txtmDate.Text != "" && txttodate.Text != "")
            {
                //Ankita - 13/may/2016- (For Optimization)
                //                str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                //                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                //                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                //                                 WHEN T1.OrderStatus='P' THEN 'Open'
                //                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                //                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                //                            from TransPurchOrder T1 Left join MastParty T2 
                //                            on T1.DistId=T2.PartyId  
                //                            LEFT JOIN MastArea AS T3
                //                            ON T2.CityId = T3.AreaId                           
                //                            where T2.Active=1 and T1.SMId IN
                //                            (select SMId from MastSalesRep where 
                //                            smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
                //                            " Convert(VARCHAR(10),T1.VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' and Convert(VARCHAR(10),T1.VDate,101)<='" + Settings.dateformat1(txttodate.Text) + "'" +
                //                            @" and (T1.DistId=" + DistID + " OR " + DistID + "=0) ORDER BY T1.PODocId ASC,T1.VDate desc";

                str = "select * from [View_CreatePurchOrder]  where Active=1 and SMId IN (select SMId from MastSalesRep where  smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
                            " Convert(VARCHAR(10),VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' and Convert(VARCHAR(10),T1.VDate,101)<='" + Settings.dateformat1(txttodate.Text) + "'" +
                            @" and (DistId=" + DistID + " OR " + DistID + "=0) ORDER BY PODocId ASC,VDate desc";
            }
            else if (txtmDate.Text != "")
            {
                //str = @"select * from TransVisit where  SMId=" + ddlUndeUser.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";
                //Ankita - 13/may/2016- (For Optimization)
                //                str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                //                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                //                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                //                                 WHEN T1.OrderStatus='P' THEN 'Open'
                //                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                //                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                //                            from TransPurchOrder T1 Left join MastParty T2 
                //                            on T1.DistId=T2.PartyId  
                //                            LEFT JOIN MastArea AS T3
                //                            ON T2.CityId = T3.AreaId                           
                //                            where T2.Active=1
                //                            and T1.SMId IN
                //                            (select SMId from MastSalesRep where 
                //                            smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
                //                            " and Convert(VARCHAR(10),T1.VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' ORDER BY T1.PODocId ASC,T1.VDate desc";

                str = "select * from [View_CreatePurchOrder]  where Active=1  and SMId IN (select SMId from MastSalesRep where smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and Convert(VARCHAR(10),VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' ORDER BY PODocId ASC,VDate desc";
            }
            else
            {
                if (StatusVal == "A")
                {
                    StatusVal = "0";

                    //Ankita - 13/may/2016- (For Optimization)
                    //                    str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                    //                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                    //                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                    //                                 WHEN T1.OrderStatus='P' THEN 'Open'
                    //                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                    //                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                    //                            from TransPurchOrder T1 Left join MastParty T2 
                    //                            on T1.DistId=T2.PartyId  
                    //                            LEFT JOIN MastArea AS T3
                    //                            ON T2.CityId = T3.AreaId                           
                    //                            where T2.Active=1   and T1.SMId IN
                    //                            (select SMId from MastSalesRep where 
                    //                            smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
                    //                                @" and (T1.DistId=" + DistID + " OR " + DistID + "=0)  and (T1.OrderStatus='" + StatusVal + "' OR " + StatusVal + "='0') ORDER BY T1.PODocId ASC,T1.VDate desc";
                    str = "select * from [View_CreatePurchOrder]  where Active=1   and SMId IN (select SMId from MastSalesRep where smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
 @" and (DistId=" + DistID + " OR " + DistID + "=0)  and (OrderStatus='" + StatusVal + "' OR " + StatusVal + "='0') ORDER BY PODocId ASC,VDate desc";
                }
                else
                {
                    //Ankita - 13/may/2016- (For Optimization)
                    //                    str = @"SELECT T1.POrdId,T1.VDate,T1.PODocId,T2.PartyName,T3.AreaName,
                    //                                CASE WHEN T1.OrderStatus='H' THEN 'On Hold'
                    //                                 WHEN T1.OrderStatus='R' THEN 'CustomerCanceled'
                    //                                 WHEN T1.OrderStatus='P' THEN 'Open'
                    //                                 WHEN T1.OrderStatus='M' THEN 'Processed'
                    //                                 WHEN T1.OrderStatus='C' THEN 'CompanyCanceled' END AS OrderStatus
                    //                            from TransPurchOrder T1 Left join MastParty T2 
                    //                            on T1.DistId=T2.PartyId  
                    //                            LEFT JOIN MastArea AS T3
                    //                            ON T2.CityId = T3.AreaId                           
                    //                            where T2.Active=1  and T1.SMId IN
                    //                            (select SMId from MastSalesRep where 
                    //                            smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + "))" +
                    //                                @" and (T1.DistId=" + DistID + " OR " + DistID + "=0)  and (T1.OrderStatus='" + StatusVal + "') ORDER BY T1.PODocId ASC,T1.VDate desc";
                    str = "select * from [View_CreatePurchOrder] where Active=1 and SMId IN (select SMId from MastSalesRep where smid in (select SMId from MastSalesRepGrp where maingrp=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and (DistId=" + DistID + " OR " + DistID + "=0)  and (OrderStatus='" + StatusVal + "') ORDER BY PODocId ASC,VDate desc";
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

                for (int i = 0; i < depdt.Rows.Count; i++)
                {
                    string strUID = @"SELECT T2.Underid FROM TransPurchOrder1 AS T1 LEFT JOIN MastItem AS T2 ON T1.ItemId=T2.ItemId WHERE T1.POrdId=" + Convert.ToInt32(depdt.Rows[i]["POrdId"]) + "";
                    DataTable dtUID = DbConnectionDAL.GetDataTable(CommandType.Text, strUID);
                    if (dtUID != null && dtUID.Rows.Count > 0)
                    {
                        string strIG = @"SELECT T1.ItemName FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(dtUID.Rows[0]["Underid"]) + "";
                        DataTable dtIG = DbConnectionDAL.GetDataTable(CommandType.Text, strIG);

                        if (dtIG != null && dtIG.Rows.Count > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["POrdId"] = depdt.Rows[i]["POrdId"];
                            dt.Rows[dt.Rows.Count - 1]["VDate"] = depdt.Rows[i]["VDate"];
                            dt.Rows[dt.Rows.Count - 1]["PODocId"] = depdt.Rows[i]["PODocId"];
                            dt.Rows[dt.Rows.Count - 1]["IGandDispatchTo"] = dtIG.Rows[0]["ItemName"].ToString() + "-(" + depdt.Rows[i]["PartyName"] + " " + depdt.Rows[i]["AreaName"] + ")";
                            dt.Rows[dt.Rows.Count - 1]["OrderStatus"] = depdt.Rows[i]["OrderStatus"];
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

        protected void ProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectType.Items[1].Selected)
            {
                ProjectDiv.Visible = true;
            }
            else
            {
                ddlProject.SelectedValue = "0";
                //txtProject.Text = "";
                ProjectDiv.Visible = false;
            }
        }

        protected void CancelOrder_Click(object sender, EventArgs e)
        {
            //Cancel Order
            int retdel = dp.CancelOwnOrder(Convert.ToInt32(ViewState["POIDDel"]));
            if (retdel == 1)
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record " + lblOrderSt.Text + " Canceled Successfully');", true);

            }
        }
    }
}