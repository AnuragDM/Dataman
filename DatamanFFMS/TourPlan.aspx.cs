using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;
using BusinessLayer;
using System.Data;
using DAL;
using System.IO;

namespace AstralFFMS
{
    public partial class TourPlan : System.Web.UI.Page
    {
        int msg = 0;
        string pageName = string.Empty;
        string pageName1 = string.Empty;
        int smID = 0;
        int smID1 = 0;
        int smIDSen = 0;
        int showSaveBtn = 0;
        int showUpdateBtn = 0;
        int isCurrentUser = 0;
        DataTable leavedt;
        DataTable holidaydt;
        DataTable dt;
        DataTable dtHoliday;
        string isEdit = string.Empty;
        int gridRowCount = 0;
        BAL.TransTourPlan.TourAll tourAll = new BAL.TransTourPlan.TourAll();
        string parameter = "";
        string Date = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            pageName = string.Empty;
            pageName1 = string.Empty;
            calendarTextBox.Attributes.Add("readonly", "readonly");
            toCalendarTextBox.Attributes.Add("readonly", "readonly");
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            //        GetLeaveData(Settings.Instance.UserID);
            calendarTextBox_CalendarExtender.StartDate = DateTime.Now;
            CalendarExtender2.StartDate = DateTime.Now;
            //Ankita - 07/may/2016- (For Optimization)
            string pageName12 = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName12);
            lblPageHeader.Text = Pageheader;

            string PermAll = Settings.Instance.CheckPagePermissions(pageName12, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
           
            smID = Convert.ToInt32(Settings.Instance.SMID);// GetSalesPerId(Convert.ToInt32(Settings.Instance.UserID));
            if (Request.QueryString["Page"] != null)
            {
                pageName = Request.QueryString["Page"];
            }
            if (Request.QueryString["PageV"] != null)
            {
                pageName1 = Request.QueryString["PageV"];
            }
            if (!IsPostBack)
            {
                smIDSen = Convert.ToInt32(Settings.Instance.SMID);
                BindSalePersonDDl();
                DdlSalesPerson.SelectedValue = Settings.Instance.SMID;
                //Ankita - 07/may/2016- (For Optimization)[commented-BindPurposeVisist() function since it is used in grid-nor required here]
               // BindPurposeVisist();                  
           
                
                //BindAreaAsUser(DdlAreaName, smID);
                //BindStaffChkList(accStaffCheckBoxList, DdlSalesPerson.SelectedValue);
                btnSave.Style.Add("display", "inline");
                btnDelete.Visible = false;
                btnSubmit.Visible = false;               
                if (Request.QueryString["SMId"] != null && Request.QueryString["DocId"] != null)
                {
                    if (!Convert.ToBoolean(SplitPerm[0])) { ShowAlert("You do not have Permission to view this form. Please Contact System Admin.!!"); return; }               
                    showSaveBtn = 1;
                    userIDHiddenField.Value = Request.QueryString["SMId"];
                    //          smID1 = GetSalePersonId(Request.QueryString["UserID"]);
                    smID1 = Convert.ToInt32(Request.QueryString["SMId"]);
                    //BindAreaAsUser(DdlAreaName, smID1);
                    HiddenField1.Value = Request.QueryString["DocId"];
                    CheckTourPlanData(Request.QueryString["SMId"], Request.QueryString["DocId"]);
                    ShowTourPlanData();
                    GetTourPlanData(Request.QueryString["SMId"], Request.QueryString["DocId"]);
                }
            }
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                //        ViewState["TourPlanId"] = parameter;
                ViewState["TourPlanHId"] = parameter;
                FillTourControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");

            }
            //Ankita - 07/may/2016- (For Optimization)
           
            if (btnSave.Text == "Save")
            {
                if (showSaveBtn != 1)
                {
                    //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName12, Convert.ToString(Session["user_name"]));
                    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                    btnSave.CssClass = "btn btn-primary";
                }
            }
            else
            {
                if (showUpdateBtn != 1)
                {
                    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                    //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName12, Convert.ToString(Session["user_name"]));
                    btnSave.CssClass = "btn btn-primary";
                }
                else
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "btn btn-primary";
                }
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
           // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName12, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        private static int GetSalesPerId(int uid)
        {
            //var envObj = (from e in context.MastSalesReps
            //              where e.UserId == uid
            //              select new { e.SMId }).FirstOrDefault();
            try
            {
                string getsmIDqry = @"select SMId from MastSalesRep where UserId=" + Settings.Instance.SMID + "";
                DataTable dt_smID = DbConnectionDAL.GetDataTable(CommandType.Text, getsmIDqry);
                return Convert.ToInt32(dt_smID.Rows[0]["SMId"]);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }

        }

        private void BindPurposeVisist()
        {
            try
            {                //Ankita - 07/may/2016- (For Optimization)
                //string purpQry = @"select * from MastPurposeVisit order by PurposeName";
                string purpQry = @"select Id,PurposeName from MastPurposeVisit order by PurposeName";
                DataTable dtPurp = DbConnectionDAL.GetDataTable(CommandType.Text, purpQry);
                if (dtPurp.Rows.Count > 0)
                {
                    DDLPurposeVisit.DataSource = dtPurp;
                    DDLPurposeVisit.DataTextField = "PurposeName";
                    DDLPurposeVisit.DataValueField = "Id";
                    DDLPurposeVisit.DataBind();
                }
                DDLPurposeVisit.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindStaffChkList(CheckBoxList accStaffCheckBoxList, string smID)
        {
            try
            {
                string salerepUplineQuery = @"select * from MastSalesRep where smid in (select maingrp from MastSalesRepGrp where smid=" + Convert.ToInt32(smID) + ") and SMName<>'.' and SMId<>" + Convert.ToInt32(smID) + " order by SMId";
                DataTable dtChild1 = DbConnectionDAL.GetDataTable(CommandType.Text, salerepUplineQuery);
                //          string salerepDowlineQuery = @"select * from MastSalesRep where UnderId=" + Convert.ToInt32(dtChild.Rows[0]["SMId"]) + "";
                DataTable dtChild2 = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dtChild2);
                dv.RowFilter = "SMId<>" + Convert.ToInt32(smID) + " and SMName<>'.'";
                //          DataTable dtChild2 = DbConnectionDAL.GetDataTable(CommandType.Text, salerepDowlineQuery);
                dtChild1.Merge(dv.ToTable());
                DataView dv1 = new DataView(dtChild1);
                DataTable dtNew = new DataTable();
                dtNew.Merge(dv1.ToTable(true, "SMId", "SMName"));
                accStaffCheckBoxList.DataSource = dtNew;
                accStaffCheckBoxList.DataTextField = "SMName";
                accStaffCheckBoxList.DataValueField = "SMId";
                accStaffCheckBoxList.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindSalePersonDDl()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            //dv.RowFilter = "RoleType<>'AreaIncharge' and SMName<>'.'";
            dv.RowFilter = "SMName<>'.'";
            DdlSalesPerson.DataSource = dv;
            DdlSalesPerson.DataTextField = "SMName";
            DdlSalesPerson.DataValueField = "SMId";
            DdlSalesPerson.DataBind();
            //Add Default Item in the DropDownList
            DataView dv1 = new DataView(dt);
            //dv1.RowFilter = "SMName<>'.'";
            //Added Nshu
            //dv1.RowFilter = "RoleType<>'AreaIncharge' and SMName<>'.'";
            dv1.RowFilter = "SMName<>'.'";
            DropDownList1.DataSource = dv1;
            DropDownList1.DataTextField = "SMName";
            DropDownList1.DataValueField = "SMId";
            DropDownList1.DataBind();
            //Add Default Item in the DropDownList
            DropDownList1.Items.Insert(0, new ListItem("--Please select--"));
            //   DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
        }
        private int GetSalePersonId(string p)
        {
            try
            {
                string salesIdQry = @"select SMId from MastSalesRep where UserId=" + Convert.ToInt32(p) + "";
                //var envObj = from e in context.MastSalesReps
                //             where e.UserId == uid
                //             select new { e.SMId };
                DataTable dtsalesId = DbConnectionDAL.GetDataTable(CommandType.Text, salesIdQry);
                if (dtsalesId.Rows.Count > 0)
                {
                    return Convert.ToInt32(dtsalesId.Rows[0]["SMId"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }
        }
        private void BindAreaAsUser(DropDownList Dropdown, int smID)
        {
            string dropdowndata = string.Empty;
            try
            {
                //string custQuery2 = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode="
                //   + smID + ")) and areatype='City' order by AreaName";
                //Ankita - 07/may/2016- (For Optimization)
                //string custQuery2 = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + smID + " )) and  areatype='city' and Active=1 order by AreaName";
                string custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + smID + " )) and  areatype='city' and Active=1 order by AreaName";
                DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, custQuery2);
                Dropdown.DataSource = dtChild;
                Dropdown.DataTextField = "AreaName";
                Dropdown.DataValueField = "AreaId";
                Dropdown.DataBind();
                Dropdown.Items.Insert(0, new ListItem("-- Select City --", "0"));
            }
            catch
            {

            }
        }
        private void FillTourControls(int p)
        {
            try
            {  //Ankita - 09/may/2016- (For Optimization)
                if (p > 0)
                {
                    
                    //string tourquery = @"select * from TransTourPlan where TourPlanId=" + p;
                    //string tourHeaderquery = @"select * from TransTourPlanHeader where TourPlanHId=" + p;
                    string tourHeaderquery = @"select TourFromDt,TourToDt,FinalRemarks from TransTourPlanHeader where TourPlanHId=" + p;
                    //string tourquery = @"select * from TransTourPlan where TourPlanHId=" + p;
                    string tourquery = @"select SMId,AppStatus,AppRemark,TourPlanId,VDate,MCityId,MDistId,MPurposeId,Remarks,docid from TransTourPlan where TourPlanHId=" + p;
                    DataTable tourHeaderQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, tourHeaderquery);
                    if (tourHeaderQryDt.Rows.Count > 0)
                    {
                        DateTime tourFromDat = DateTime.Parse(tourHeaderQryDt.Rows[0]["TourFromDt"].ToString());
                        DateTime tourToDat = DateTime.Parse(tourHeaderQryDt.Rows[0]["TourToDt"].ToString());
                        calendarTextBox.Text = tourFromDat.ToString("dd/MMM/yyyy");
                        toCalendarTextBox.Text = tourToDat.ToString("dd/MMM/yyyy");
                        calendarTextBox.Enabled = false;
                        toCalendarTextBox.Enabled = false;
                        calendarTextBox.CssClass = "form-control";
                        toCalendarTextBox.CssClass = "form-control";
                        AddNewTour.Enabled = false;
                        AddNewTour.CssClass = "btn btn-primary";
                        fRemark.Style["display"] = "block";
                        finalRemarkTextarea.InnerText = tourHeaderQryDt.Rows[0]["FinalRemarks"].ToString();
                    }
                    DataTable tourQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, tourquery);
                    if (tourQryDt.Rows.Count > 0)
                    {
                        isEdit = "1";

                        if (tourQryDt.Rows[0]["SMId"] != DBNull.Value)
                        {
                            //BindAreaAsUser(ddlArea, Convert.ToInt32(tourQryDt.Rows[0]["SMId"]));
                        }
                     
                        DdlSalesPerson.SelectedValue = tourQryDt.Rows[0]["SMId"].ToString();
                        GridView1.DataSource = tourQryDt;
                        GridView1.DataBind();
                        string custQuery2 = "";
                        if (Request.QueryString["SMId"] != null)
                        {  
                            DdlSalesPerson.SelectedValue = Request.QueryString["SMId"].ToString();
                            custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Request.QueryString["SMId"].ToString() + " )) and  areatype='city' and Active=1 order by AreaName";
                        }
                        else
                        {
                            custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + DdlSalesPerson.SelectedValue + " )) and  areatype='city' and Active=1 order by AreaName";
                        }
                        TextBox remarkTxtBox ;
                        Label lblremarks,lblCityID,lblDistId,lblPurpose;
                        ListBox ddlareaDP, ddldistDP, ddlpurposeVisit;
                        DataTable dtChild1 = DbConnectionDAL.GetDataTable(CommandType.Text, custQuery2);
                        string purpQry = @"select Id,PurposeName from MastPurposeVisit order by PurposeName";
                        DataTable dtPurp = DbConnectionDAL.GetDataTable(CommandType.Text, purpQry);
                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                             remarkTxtBox = (GridView1.Rows[i].FindControl("remarkTextBox") as TextBox);
                             lblremarks = (GridView1.Rows[i].FindControl("lblRemark") as Label);
                             lblCityID = (GridView1.Rows[i].FindControl("lblCity") as Label);
                             lblDistId = (GridView1.Rows[i].FindControl("lblDist") as Label);
                             lblPurpose = (GridView1.Rows[i].FindControl("lblPurp") as Label);
                             ddlareaDP = (GridView1.Rows[i].FindControl("ddlArea") as ListBox);
                             ddldistDP = (GridView1.Rows[i].FindControl("ddlDistributor") as ListBox);
                             ddlpurposeVisit = (GridView1.Rows[i].FindControl("ddlPurposeVisit") as ListBox);
                             remarkTxtBox.Text = lblremarks.Text;

                             if (dtChild1.Rows.Count > 0)
                             {
                                 ddlareaDP.DataSource = dtChild1;
                                 ddlareaDP.DataTextField = "AreaName";
                                 ddlareaDP.DataValueField = "AreaId";
                                 ddlareaDP.DataBind();
                             }
                             string[] cityIdStrAll = new string[1000];
                             string cityIdStr = lblCityID.Text.ToString();
                             cityIdStrAll = cityIdStr.Split(',');
                             if (cityIdStrAll.Length > 0)
                             {
                                 for (int k = 0; k < cityIdStrAll.Length; k++)
                                 {
                                     ListItem currentCheckBox = ddlareaDP.Items.FindByValue(cityIdStrAll[k].ToString());
                                     if (currentCheckBox != null)
                                     {
                                         currentCheckBox.Selected = true;
                                     }
                                 }
                             }

                             //       For CheckBoxList Distributor
                             if (cityIdStr != "")
                             {
                                 string str = "select mp.PartyId,mp.PartyName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyDist=1 and mp.Active=1 and mp.CityId in (" + cityIdStr + ")";
                                 DataTable dt = new DataTable();
                                 dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                 ddldistDP.DataSource = dt;
                                 ddldistDP.DataTextField = "PartyName";
                                 ddldistDP.DataValueField = "PartyId";
                                 ddldistDP.DataBind();
                             }
                             string[] distIdStrAll = new string[1000];
                             string distIdStr = lblDistId.Text.ToString();
                             distIdStrAll = distIdStr.Split(',');
                             if (distIdStrAll.Length > 0)
                             {
                                 for (int j = 0; j < distIdStrAll.Length; j++)
                                 {
                                     ListItem currentCheckBox = ddldistDP.Items.FindByValue(distIdStrAll[j].ToString());
                                     if (currentCheckBox != null)
                                     {
                                         currentCheckBox.Selected = true;
                                     }
                                 }
                             }
                             if (dtPurp.Rows.Count > 0)
                             {
                                 ddlpurposeVisit.DataSource = dtPurp;
                                 ddlpurposeVisit.DataTextField = "PurposeName";
                                 ddlpurposeVisit.DataValueField = "Id";
                                 ddlpurposeVisit.DataBind();
                             }
                             string[] purpIdStrAll = new string[1000];
                             string purpIdStr = lblPurpose.Text.ToString();
                             purpIdStrAll = purpIdStr.Split(',');
                             if (purpIdStrAll.Length > 0)
                             {
                                 for (int l = 0; l < purpIdStrAll.Length; l++)
                                 {
                                     ListItem currentCheckBox = ddlpurposeVisit.Items.FindByValue(purpIdStrAll[l].ToString());
                                     if (currentCheckBox != null)
                                     {
                                         currentCheckBox.Selected = true;
                                     }
                                 }
                             }
                        }
                       

                    
                       

                        //if (tourQryDt.Rows[0]["DistId"] != DBNull.Value)
                        //{
                        //    int partyId = Convert.ToInt32(tourQryDt.Rows[0]["DistId"]);
                        //    hfDistId.Value = partyId.ToString();
                        //    string str = "select  mp.*,ma.AreaName FROM MastParty mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyId=" + partyId + "";
                        //    DataTable dt = new DataTable();

                        //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        //    txtDist.Text = "(" + dt.Rows[0]["PartyName"].ToString() + ")" + " " + dt.Rows[0]["SyncId"].ToString() + " " + "(" + dt.Rows[0]["AreaName"].ToString() + ")";
                        //}
                        //else { txtDist.Text = string.Empty; }

                        //DDLPurposeVisit.SelectedValue = tourQryDt.Rows[0]["Purpose"].ToString();
                        //DistStaff.Value = tourQryDt.Rows[0]["DistStaff"].ToString();
                        //DdlSalesPerson.SelectedValue = tourQryDt.Rows[0]["SMId"].ToString();

                        //For CheckBoxList
                        //string[] accStaffAll = new string[20];
                        //string accStaff = tourQryDt.Rows[0]["AccDistributor"].ToString();
                        //accStaffAll = accStaff.Split(',');
                        //if (accStaffAll.Length > 0)
                        //{
                        //    for (int i = 0; i < accStaffAll.Length; i++)
                        //    {
                        //        ListItem currentCheckBox = accStaffCheckBoxList.Items.FindByText(accStaffAll[i].ToString());
                        //        if (currentCheckBox != null)
                        //        {
                        //            currentCheckBox.Selected = true;
                        //        }
                        //    }
                        //}
                        approveStatusRadioButtonList.Enabled = false;
                        TextArea1.Disabled = true;
                        if (tourQryDt.Rows[0]["AppStatus"].ToString() == "Approve" || tourQryDt.Rows[0]["AppStatus"].ToString() == "Reject")
                        {
                            approveStatusRadioButtonList.SelectedValue = tourQryDt.Rows[0]["AppStatus"].ToString();
                            conditonaldiv.Style.Add("display", "block");
                        }
                        else
                        {
                            approveStatusRadioButtonList.SelectedValue = "Approve";
                            conditonaldiv.Style.Add("display", "none");
                        }
                        if (tourQryDt.Rows[0]["AppRemark"].ToString() != "")
                        {
                            TextArea1.Value = tourQryDt.Rows[0]["AppRemark"].ToString();
                        }
                        btnSave.Style.Add("display", "inline");
                        btnSave.Text = "Update";
                        btnFind.Visible = true;
                        btnDelete.Visible = true;
                        if (tourQryDt.Rows[0]["AppStatus"].ToString() == "Approve" || tourQryDt.Rows[0]["AppStatus"].ToString() == "Reject")
                        {
                            btnSave.Enabled = false;
                            showUpdateBtn = 1;
                            btnDelete.Enabled = false;
                            btnSave.CssClass = "btn btn-primary";
                            btnDelete.CssClass = "btn btn-primary";
                            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
                        }
                        else
                        {
                            btnSave.Enabled = true;
                            btnDelete.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        
        
        public int ShowTourPlanData()
        {    //Ankita - 07/may/2016- (For Optimization)
         //   string getqry = @"select * from MastSalesRep where UserId=" + Settings.Instance.UserID + "";
            string getqry = @"select SMId from MastSalesRep where UserId=" + Settings.Instance.UserID + "";
            int SmIDFromSession = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, getqry));
            if (Convert.ToInt32(Request.QueryString["SMId"]) == SmIDFromSession)
            {
                IsCurrUserHiddenField.Value = "1";
            }
            return isCurrentUser;
        }

        private void CheckTourPlanData(string userID, string docID)
        {  //Ankita - 07/may/2016- (For Optimization)
            //string tourPlanDataCountQry = @"select * from TransTourPlan where SMId=" + userID + " and DocId='" + docID + "'";
            //DataTable tourPlanDataCountdt = DbConnectionDAL.GetDataTable(CommandType.Text, tourPlanDataCountQry);
            int tourPlanDataCount = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from TransTourPlan where SMId=" + userID + " and DocId='" + docID + "'"));
            //if (tourPlanDataCountdt.Rows.Count > 0)
            if (tourPlanDataCount > 0)
            {
                chkTourDataHdf.Value = "1";
            }
            else
            {
                chkTourDataHdf.Value = "0";
            }
        }

        private void GetTourPlanData(string SMId, string docID)
        {

            string data = string.Empty;
            try
            { //Ankita - 07/may/2016- (For Optimization)
                //string getTourHeaderDataQry = @"select * from TransTourPlanHeader where DocId='" + docID + "' and Smid=" + SMId + "";
                string getTourHeaderDataQry = @"select TourFromDt,TourToDt,FinalRemarks from TransTourPlanHeader where DocId='" + docID + "' and Smid=" + SMId + "";
                DataTable gettourHeaderdt = DbConnectionDAL.GetDataTable(CommandType.Text, getTourHeaderDataQry);

                if (gettourHeaderdt.Rows.Count > 0)
                {
                    DateTime tourFromDat = DateTime.Parse(gettourHeaderdt.Rows[0]["TourFromDt"].ToString());
                    DateTime tourToDat = DateTime.Parse(gettourHeaderdt.Rows[0]["TourToDt"].ToString());
                    calendarTextBox.Text = tourFromDat.ToString("dd/MMM/yyyy");
                    toCalendarTextBox.Text = tourToDat.ToString("dd/MMM/yyyy");
                    calendarTextBox.Enabled = false;
                    toCalendarTextBox.Enabled = false;
                    calendarTextBox.CssClass = "form-control";
                    toCalendarTextBox.CssClass = "form-control";
                    AddNewTour.Enabled = false;
                    AddNewTour.CssClass = "btn btn-primary";
                    fRemark.Style["display"] = "block";                    
                    finalRemarkTextarea.InnerText = gettourHeaderdt.Rows[0]["FinalRemarks"].ToString();

                  //  string getTourLineDataQry = @"select * from TransTourPlan where DocId='" + docID + "' and Smid=" + SMId + "";
                    string getTourLineDataQry = @"select SMId,TourPlanId,VDate,MCityId,MDistId,MPurposeId,Remarks from TransTourPlan where DocId='" + docID + "' and Smid=" + SMId + "";
                    DataTable gettourLinedt = DbConnectionDAL.GetDataTable(CommandType.Text, getTourLineDataQry);
                    if (gettourLinedt.Rows.Count > 0)
                    {
                        isEdit = "1";
                        DdlSalesPerson.SelectedValue = gettourLinedt.Rows[0]["SMId"].ToString();
                        GridView1.DataSource = gettourLinedt;
                        GridView1.DataBind();
                        string custQuery2 = "";
                        if (Request.QueryString["SMId"] != null)
                        {
                            DdlSalesPerson.SelectedValue = Request.QueryString["SMId"].ToString();
                            custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Request.QueryString["SMId"].ToString() + " )) and  areatype='city' and Active=1 order by AreaName";
                        }
                        else
                        {
                            custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + DdlSalesPerson.SelectedValue + " )) and  areatype='city' and Active=1 order by AreaName";
                        }
                        TextBox remarkTxtBox;
                        Label lblremarks, lblCityID, lblDistId, lblPurpose;
                        ListBox ddlareaDP, ddldistDP, ddlpurposeVisit;
                        DataTable dtChild1 = DbConnectionDAL.GetDataTable(CommandType.Text, custQuery2);
                        string purpQry = @"select Id,PurposeName from MastPurposeVisit order by PurposeName";
                        DataTable dtPurp = DbConnectionDAL.GetDataTable(CommandType.Text, purpQry);
                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                            remarkTxtBox = (GridView1.Rows[i].FindControl("remarkTextBox") as TextBox);
                            lblremarks = (GridView1.Rows[i].FindControl("lblRemark") as Label);
                            lblCityID = (GridView1.Rows[i].FindControl("lblCity") as Label);
                            lblDistId = (GridView1.Rows[i].FindControl("lblDist") as Label);
                            lblPurpose = (GridView1.Rows[i].FindControl("lblPurp") as Label);
                            ddlareaDP = (GridView1.Rows[i].FindControl("ddlArea") as ListBox);
                            ddldistDP = (GridView1.Rows[i].FindControl("ddlDistributor") as ListBox);
                            ddlpurposeVisit = (GridView1.Rows[i].FindControl("ddlPurposeVisit") as ListBox);
                            remarkTxtBox.Text = lblremarks.Text;
                            if (dtChild1.Rows.Count > 0)
                            {
                                ddlareaDP.DataSource = dtChild1;
                                ddlareaDP.DataTextField = "AreaName";
                                ddlareaDP.DataValueField = "AreaId";
                                ddlareaDP.DataBind();
                            }
                            string[] cityIdStrAll = new string[1000];
                            string cityIdStr = lblCityID.Text.ToString();
                            cityIdStrAll = cityIdStr.Split(',');
                            if (cityIdStrAll.Length > 0)
                            {
                                for (int k = 0; k < cityIdStrAll.Length; k++)
                                {
                                    ListItem currentCheckBox = ddlareaDP.Items.FindByValue(cityIdStrAll[k].ToString());
                                    if (currentCheckBox != null)
                                    {
                                        currentCheckBox.Selected = true;
                                    }
                                }
                            }

                            //       For CheckBoxList Distributor
                            if (cityIdStr != "")
                            {
                                string str = "select mp.PartyId,mp.PartyName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyDist=1 and mp.CityId in (" + cityIdStr + ")";
                                DataTable dt = new DataTable();
                                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                ddldistDP.DataSource = dt;
                                ddldistDP.DataTextField = "PartyName";
                                ddldistDP.DataValueField = "PartyId";
                                ddldistDP.DataBind();
                            }
                            string[] distIdStrAll = new string[1000];
                            string distIdStr = lblDistId.Text.ToString();
                            distIdStrAll = distIdStr.Split(',');
                            if (distIdStrAll.Length > 0)
                            {
                                for (int j = 0; j < distIdStrAll.Length; j++)
                                {
                                    ListItem currentCheckBox = ddldistDP.Items.FindByValue(distIdStrAll[j].ToString());
                                    if (currentCheckBox != null)
                                    {
                                        currentCheckBox.Selected = true;
                                    }
                                }
                            }
                            if (dtPurp.Rows.Count > 0)
                            {
                                ddlpurposeVisit.DataSource = dtPurp;
                                ddlpurposeVisit.DataTextField = "PurposeName";
                                ddlpurposeVisit.DataValueField = "Id";
                                ddlpurposeVisit.DataBind();
                            }
                            string[] purpIdStrAll = new string[1000];
                            string purpIdStr = lblPurpose.Text.ToString();
                            purpIdStrAll = purpIdStr.Split(',');
                            if (purpIdStrAll.Length > 0)
                            {
                                for (int l = 0; l < purpIdStrAll.Length; l++)
                                {
                                    ListItem currentCheckBox = ddlpurposeVisit.Items.FindByValue(purpIdStrAll[l].ToString());
                                    if (currentCheckBox != null)
                                    {
                                        currentCheckBox.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                }


                string gettourDataQuery = @"select r.AccDistributor, r.MCityId, r.MDistId, r.DocId, r.MPurposeId, r.UserID, r.VDate, r.TourPlanId, r.AppRemark, r.AppStatus,r.DistStaff,r.SMId from TransTourPlan r Where r.SMId =" + Convert.ToInt32(SMId) + " and r.DocId ='" + docID + "'";

                DataTable gettourdt = DbConnectionDAL.GetDataTable(CommandType.Text, gettourDataQuery);
                if (gettourdt.Rows.Count > 0)
                {
                    //For CheckBoxList
                    //
                    //           AccDistributor.Value = gettourdt.Rows[0]["AccDistributor"].ToString();

                    approveStatusRadioButtonList.SelectedValue = gettourdt.Rows[0]["AppStatus"].ToString();
                    appStatusHiddenField.Value = gettourdt.Rows[0]["AppStatus"].ToString();
                    if (gettourdt.Rows[0]["AppRemark"].ToString() != "")
                    {
                        TextArea1.Value = gettourdt.Rows[0]["AppRemark"].ToString();
                    }
                    if (IsCurrUserHiddenField.Value == "1")
                    {
                        btnSubmit.Visible = false;
                        btnSubmit.Style.Add("display", "none");
                        btnFind.Visible = true;
                        btnSave.Style.Add("display", "inline");
                        btnSave.Enabled = false;
                        btnSave.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        if (appStatusHiddenField.Value == "Approve" || appStatusHiddenField.Value == "Reject")
                        {
                            btnSubmit.Visible = true;
                            btnSubmit.Enabled = false;
                            btnSubmit.CssClass = "btn btn-primary";
                            btnFind.Visible = false;
                            btnSubmit.Style.Add("display", "inline");
                            btnSave.Style.Add("display", "none");
                        }
                        else
                        {
                            btnSubmit.Visible = true;
                            btnSubmit.Enabled = true;
                            btnFind.Visible = false;
                            btnSubmit.Style.Add("display", "inline");
                            btnSave.Style.Add("display", "none");
                        }

                    }
                }
                //var obj = (from r in context.TransTourPlans.Where(x => x.UserID == Convert.ToInt32(UserId) && x.DocId == docID)
                //           select new { r.AccDistributor, r.CityId, r.DistId, r.DocId, r.Purpose, r.UserID, r.VDate, r.TourPlanId, r.AppRemark, r.AppStatus }).ToList();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void GetLeaveData(string p)
        {
            leavedt = new DataTable();
            dt = new DataTable();
            leavedt.Columns.Clear();
            dt.Columns.Clear();

            string leavequery = @"select leave.UserId, leave.NoOfDays,leave.FromDate as LeaveDate, leave.AppStatus  from TransLeaveRequest leave
                                where leave.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " order by leave.FromDate";

            leavedt = DbConnectionDAL.GetDataTable(CommandType.Text, leavequery);

            dt.Columns.Add("UserId");
            dt.Columns.Add("LeaveDate");
            dt.Columns.Add("AppStatus");

            int ou = -1;
            for (int i = 0; i < leavedt.Rows.Count; i++)
            {
                int leavedays = Convert.ToInt32((leavedt.Rows[i]["NoOfDays"]));
                DateTime dat = Convert.ToDateTime(leavedt.Rows[i]["LeaveDate"]);
                if (leavedays > 1)
                {
                    for (int j = 0; j < leavedays; j++)
                    {
                        ou++;
                        dt.Rows.Add();
                        dt.Rows[ou]["UserId"] = leavedt.Rows[i]["UserId"].ToString();
                        dt.Rows[ou]["LeaveDate"] = dat.AddDays(j);
                        dt.Rows[ou]["AppStatus"] = leavedt.Rows[i]["AppStatus"].ToString();

                    }
                }
                else
                {
                    ou++;
                    dt.Rows.Add();
                    dt.Rows[ou]["UserId"] = leavedt.Rows[i]["UserId"].ToString();
                    dt.Rows[ou]["LeaveDate"] = leavedt.Rows[i]["LeaveDate"].ToString();
                    dt.Rows[ou]["AppStatus"] = leavedt.Rows[i]["AppStatus"].ToString();
                }
            }

        }

        private void fillRepeter()
        {
            try
            {
                string tourPlanQry = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.MDistId, r.MPurposeId, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,mpv.PurposeName
                                    from TransTourPlan r
                                    left join MastPurposeVisit mpv on r.MPurposeId=mpv.ID
                                    left join MastArea Area on r.MCityId=Area.AreaId
                                    where r.UserID=" + Convert.ToInt32(Settings.Instance.UserID) + " order by VDate desc";
                DataTable tourPlandt = DbConnectionDAL.GetDataTable(CommandType.Text, tourPlanQry);
                rpt.DataSource = tourPlandt;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        private void GetHolidayData(string p)
        {
            holidaydt = new DataTable();
            dtHoliday = new DataTable();
            holidaydt.Columns.Clear();
            dtHoliday.Columns.Clear();
            //Ankita - 07/may/2016- (For Optimization)
//            string holidayquery = @"SELECT * FROM   mastholiday hm where hm.area_id IN 
//       (
//              SELECT maingrp
//              FROM   mastareagrp
//              WHERE  areaid IN
//                     (
//                            SELECT linkcode
//                            FROM   mastlink
//                            WHERE  primtable = 'SALESPERSON'
//                            AND    linktable = 'AREA'
//                            AND    primcode IN (" + Convert.ToInt32(p) + "))) order by holiday_date";
            string holidayquery = @"SELECT holiday_date, description FROM mastholiday hm where hm.area_id IN (SELECT distinct maingrp FROM   mastareagrp WHERE areaid IN 
            (SELECT linkcode FROM mastlink WHERE  primtable = 'SALESPERSON' AND linktable = 'AREA' AND    primcode IN (" + Convert.ToInt32(p) + "))) order by holiday_date";
           
            holidaydt = DbConnectionDAL.GetDataTable(CommandType.Text, holidayquery);
            dtHoliday.Columns.Add("holiday_date");
            dtHoliday.Columns.Add("description");

            int ou = -1;
            for (int i = 0; i < holidaydt.Rows.Count; i++)
            {
                ou++;
                dtHoliday.Rows.Add();
                dtHoliday.Rows[ou]["holiday_date"] = holidaydt.Rows[i]["holiday_date"].ToString();
                dtHoliday.Rows[ou]["description"] = holidaydt.Rows[i]["description"].ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //      GetLeaveData(DdlSalesPerson.SelectedValue);
                //       GetHolidayData(DdlSalesPerson.SelectedValue);
                if (btnSave.Text == "Update")
                {
                    UpdateTourPlan();
                }
                else
                {
                    InsertTourPlan();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void UpdateTourPlanByNotify()
        {
            try
            {
                //update tour records by abhishek
                {
                    foreach (GridViewRow control in GridView1.Rows)
                    {
                        if (control.RowType == DataControlRowType.DataRow)
                        {
                            string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                            Label dateLbl = control.FindControl("lblDat1") as Label;
                            HiddenField tourPlanID = control.FindControl("tourPlanHdf") as HiddenField;
                            TextBox remarkTxtBox = control.FindControl("remarkTextBox") as TextBox;
                            Label remarkLbl = control.FindControl("lblRemark") as Label;
                            ListBox ddlTourCity = control.FindControl("ddlArea") as ListBox;
                            ListBox ddlTourDist = control.FindControl("ddlDistributor") as ListBox;
                            ListBox ddlTourPurpose = control.FindControl("ddlPurposeVisit") as ListBox;
                            string cityIDStr = "", distStr = "", purposeStr = "";
                            string cityIDStrName = "", distStrName = "", puposeStrName = "";
                            foreach (ListItem tourCity in ddlTourCity.Items)
                            {
                                if (tourCity.Selected)
                                {
                                    cityIDStr += tourCity.Value + ",";
                                    cityIDStrName += tourCity.Text + ",";
                                }
                            }
                            cityIDStr = cityIDStr.TrimStart(',').TrimEnd(',');
                            cityIDStrName = cityIDStrName.TrimStart(',').TrimEnd(',');

                            if (cityIDStr != "")
                            {
                                foreach (ListItem dist in ddlTourDist.Items)
                                {
                                    if (dist.Selected)
                                    {
                                        distStr += dist.Value + ",";
                                        distStrName += dist.Text + ",";
                                    }
                                }
                                distStr = distStr.TrimStart(',').TrimEnd(',');
                                distStrName = distStrName.TrimStart(',').TrimEnd(',');

                                foreach (ListItem purpose in ddlTourPurpose.Items)
                                {
                                    if (purpose.Selected)
                                    {
                                        purposeStr += purpose.Value + ",";
                                        puposeStrName += purpose.Text + ",";
                                    }
                                }
                                purposeStr = purposeStr.TrimStart(',').TrimEnd(',');
                                puposeStrName = puposeStrName.TrimStart(',').TrimEnd(',');

                                //int retsave = tourAll.Insert(retTourPlanHId, docId, Convert.ToDateTime(dtTour.Rows[d]["TourDate"].ToString()), Convert.ToInt32(Settings.Instance.UserID), dtTour.Rows[d]["DistributorID"].ToString(), dtTour.Rows[d]["PurposeOfVisit"].ToString(), dtTour.Rows[d]["DistributorStaff"].ToString(), dtTour.Rows[d]["AccompanyingStaff"].ToString(), Convert.ToInt32(dtTour.Rows[d]["CityID"].ToString()), "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue));

                                int retsave = tourAll.Update(Convert.ToInt64(tourPlanID.Value), Convert.ToDateTime(dateLbl.Text.ToString()), Convert.ToInt32(Settings.Instance.UserID), "0", "0", string.Empty, string.Empty, "0", Convert.ToInt32(Request.QueryString["SMId"]), remarkTxtBox.Text.ToString(), cityIDStr, cityIDStrName, distStr, distStrName, purposeStr, puposeStrName);
                                // int retsave = tourAll.Insert(retTourPlanHId, docId, Convert.ToDateTime(Date), Convert.ToInt32(Settings.Instance.UserID), "0", "0", string.Empty, string.Empty, 0, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), remarkTxtBox.Text, cityIDStr, cityIDStrName, distStr, distStrName, purposeStr, puposeStrName);
                            }
                        }
                    }
                }
                if (TextArea1.Value != "")
                {    //Ankita - 07/may/2016- (For Optimization)
                    string troupHeaderQry = @"update TransTourPlanHeader set AppStatus='" + approveStatusRadioButtonList.SelectedValue + "',AppRemark='" + TextArea1.Value + "',AppBySMId=" + Settings.Instance.SMID + ",AppBy=" + Convert.ToInt32(Settings.Instance.UserID) + " where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId='" + Request.QueryString["DocId"] + "'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, troupHeaderQry);
                //    DataTable troupPlanHeaderdt = DbConnectionDAL.GetDataTable(CommandType.Text, troupHeaderQry);
                
                    //string troupQry = @"select * from TransTourPlan where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId='" + Request.QueryString["DocId"] + "'";
                    //DataTable troupPlandt = DbConnectionDAL.GetDataTable(CommandType.Text, troupQry);
                    int troupPlan = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select * from TransTourPlan where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId='" + Request.QueryString["DocId"] + "'"));
                    //if (troupPlandt.Rows.Count != 0)
                    if (troupPlan > 0)
                    {
                        string upToupQry = @"update TransTourPlan set AppStatus='" + approveStatusRadioButtonList.SelectedValue + "',AppRemark='" + TextArea1.Value + "',AppBySMId=" + Settings.Instance.SMID + ",AppBy=" + Convert.ToInt32(Settings.Instance.UserID) + " where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId='" + Request.QueryString["DocId"] + "'";

                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, upToupQry);

                        //string updateToup1 = @"select * from TransTourPlan where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId='" + Request.QueryString["DocId"] + "'";
                        string updateToup1 = @"select AppStatus,SMId,UserId from TransTourPlan where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId='" + Request.QueryString["DocId"] + "'";
                        DataTable updateToupdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, updateToup1);
                        string AppStatus = updateToupdt1.Rows[0]["AppStatus"].ToString();
                        string smID = updateToupdt1.Rows[0]["SMId"].ToString();
                        //string getSeniorSMId1 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(smID) + "";
                        //DataTable salesRepqryForSManNewAdt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId1);
                        //int senSMid = 0, UserId = 0; string senSMName = "";
                        //if (salesRepqryForSManNewAdt.Rows.Count > 0)
                        //{
                        //    UserId = Convert.ToInt32(salesRepqryForSManNewAdt.Rows[0]["UserId"].ToString());
                        //    string getSeniorSMId12 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewAdt.Rows[0]["UnderId"]) + "";
                        //    DataTable salesRepqryForSManNewAdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                        //    if (salesRepqryForSManNewAdt12.Rows.Count > 0)
                        //    {
                        //        senSMid = Convert.ToInt32(salesRepqryForSManNewAdt12.Rows[0]["SMId"]);
                        //        senSMName = salesRepqryForSManNewAdt12.Rows[0]["SMName"].ToString();
                        //    }
                        //}
                        //string toSMId = senSMid.ToString();

                        //
                        //string senSMNameQry = @"select SMName from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                        //string senSMName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQry));
                        string senSMName = Settings.Instance.SmLogInName;
                        int salesRepID = Convert.ToInt32(updateToupdt1.Rows[0]["SMId"]);
                        int UserId = Convert.ToInt32(updateToupdt1.Rows[0]["UserId"]);
                        //

                        string pro_id = string.Empty;
                        string displayTitle = string.Empty;
                        if (AppStatus == "Approve")
                        {
                            pro_id = "TOURPLANAPPROVED";
                            displayTitle = "Tour Plan Approved By - " + senSMName + " ";
                        }
                        else
                        {
                            pro_id = "TOURPLANREJECTED";
                            displayTitle = "Tour Plan Rejected By - " + senSMName + " ";
                        }
                        DateTime newDate1 = Settings.GetUTCTime();
                        string msgUrl1 = "TourPlan.aspx?SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + "&DocId=" + Request.QueryString["DocId"];
                        tourAll.InsertTransNotification(pro_id, Convert.ToInt32(UserId), newDate1, msgUrl1, displayTitle, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(salesRepID));

                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                         "alert('Record Updated Successfully'); window.location='" + Request.ApplicationPath + "TourPlanApproval.aspx?SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + "';", true);

                        //        Response.Redirect("~/TourPlanApproval.aspx");

                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void InsertTourPlan()
        {
            try
            {              
                DateTime GetDate1 = Convert.ToDateTime(calendarTextBox.Text);              
                int counterNew = CheckTourPlanEntry();
                if (!(counterNew >= 1))
                {

                    if (GridView1.Rows.Count > 0)
                    {
                        DateTime newDate = Settings.GetUTCTime();
                        string docId = tourAll.GetDociId(newDate);
                        DataTable dtTour = (DataTable)Session["TourData"];
                        DateTime fromdt1 = DateTime.Parse(dtTour.Rows[0]["VDate"].ToString());
                        DateTime todt = DateTime.Parse(dtTour.Rows[dtTour.Rows.Count - 1]["VDate"].ToString());
                        int retTourPlanHId = tourAll.InsertTourPlanHeader(docId, Convert.ToDateTime(Settings.GetUTCTime()), Convert.ToInt32(Settings.Instance.UserID), string.Empty, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), string.Empty, fromdt1, todt, finalRemarkTextarea.Value);

                        if (retTourPlanHId.ToString() != string.Empty)
                        {
                            foreach (GridViewRow control in GridView1.Rows)
                            {
                                if (control.RowType == DataControlRowType.DataRow)
                                {
                                    string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                                    TextBox remarkTxtBox = control.FindControl("remarkTextBox") as TextBox;
                                    Label remarkLbl = control.FindControl("lblRemark") as Label;
                                    ListBox ddlTourCity = control.FindControl("ddlArea") as ListBox;
                                    ListBox ddlTourDist = control.FindControl("ddlDistributor") as ListBox;
                                    ListBox ddlTourPurpose = control.FindControl("ddlPurposeVisit") as ListBox;
                                    string cityIDStr = "", distStr = "", purposeStr = "", cityIDStrName = "", distStrName = "",puposeStrName="" ;
                                    foreach (ListItem tourCity in ddlTourCity.Items)
                                    {
                                        if (tourCity.Selected)
                                        {
                                            cityIDStr += tourCity.Value + ",";
                                            cityIDStrName += tourCity.Text + ",";
                                        }
                                    }
                                    cityIDStr = cityIDStr.TrimStart(',').TrimEnd(',');
                                    cityIDStrName = cityIDStrName.TrimStart(',').TrimEnd(',');
                                    if (cityIDStr != "")
                                    {
                                        foreach (ListItem dist in ddlTourDist.Items)
                                        {
                                            if (dist.Selected)
                                            {
                                                distStr += dist.Value + ",";
                                                distStrName += dist.Text + ",";
                                            }
                                        }
                                        distStr = distStr.TrimStart(',').TrimEnd(',');
                                        distStrName = distStrName.TrimStart(',').TrimEnd(',');

                                        foreach (ListItem purpose in ddlTourPurpose.Items)
                                        {
                                            if (purpose.Selected)
                                            {
                                                purposeStr += purpose.Value + ",";
                                                puposeStrName += purpose.Text + ",";
                                            }
                                        }
                                        purposeStr = purposeStr.TrimStart(',').TrimEnd(',');
                                        puposeStrName = puposeStrName.TrimStart(',').TrimEnd(',');

                                        int retsave = tourAll.Insert(retTourPlanHId, docId, Convert.ToDateTime(Date), Convert.ToInt32(Settings.Instance.UserID), "0", "0", string.Empty, string.Empty, 0, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), remarkTxtBox.Text, cityIDStr, cityIDStrName, distStr, distStrName, purposeStr,puposeStrName);
                                    }
                                }
                            }

                            tourAll.SetDociId(docId);

                            string query12 = @"select TourExpApproval from MastEnviro";
                            DataTable getleaveAppStatdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, query12);

                            if (Convert.ToBoolean(getleaveAppStatdt12.Rows[0]["TourExpApproval"]) == true)
                            {
                                string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                                DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);
                                string salesRepqueryNew1 = "";
                                if (salesRepqueryNewdt.Rows.Count > 0)
                                {
                                    salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                                }
                                //Ankita - 07/may/2016- (For Optimization)
                             //   string getSeniorSMId = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                                string getSeniorSMId = @"select UnderId,UserId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                                DataTable salesRepqryForSManNewTPdt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId);
                                int senSMid = 0; string seniorName = string.Empty;
                                if (salesRepqryForSManNewTPdt.Rows.Count > 0)
                                {
                                    //string getSeniorSMId12 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewTPdt.Rows[0]["UnderId"]) + "";
                                    string getSeniorSMId12 = @"select SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewTPdt.Rows[0]["UnderId"]) + "";
                                    DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                                    if (salesRepqryForSManNewdt12.Rows.Count > 0)
                                    {
                                        senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
                                        seniorName = Convert.ToString(salesRepqryForSManNewdt12.Rows[0]["SMName"]);
                                    }
                                }

                                DataTable salesRepqueryNewdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew1);
                                string msgUrl = "TourPlan.aspx?SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "&DocId=" + docId;

                                //Check Is Senior

                                int smiDDDl = Convert.ToInt32(DdlSalesPerson.SelectedValue);
                                string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                                DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);
                                int IsSenior = 0, ISSenSmId = 0;
                                if (smIDNewDDlQryDT.Rows.Count > 0)
                                {
                                    ISSenSmId = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
                                }
                                if (smiDDDl != ISSenSmId)
                                {
                                    IsSenior = 1;
                                }
                                //

                                //            if (DdlSalesPerson.SelectedIndex > 0)
                                if (IsSenior == 1)
                                {
                                    string updateTourHeaderStatQry = @"update TransTourPlanHeader set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ",AppRemark='Approved By Senior' where TourPlanHId='" + retTourPlanHId + "' ";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateTourHeaderStatQry);

                                    string updateTourStatusQry = @"update TransTourPlan set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ",AppRemark='Approved By Senior' where TourPlanHId='" + retTourPlanHId + "' ";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateTourStatusQry);
                                    //string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smId, int toSMId
                                    tourAll.InsertTransNotification("TOURPLANAPPROVED", Convert.ToInt32(salesRepqryForSManNewTPdt.Rows[0]["UserId"]), Settings.GetUTCTime(), msgUrl, "Tour Plan Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), senSMid, Convert.ToInt32(DdlSalesPerson.SelectedValue));

                                    //string updateTourStatusQry = @"update TransTourPlan set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + smIDSen + ",AppRemark='Approved By Senior' where TourPlanId='" + retsave + "' ";
                                    //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateTourStatusQry);
                                }
                                else
                                {
                                    tourAll.InsertTransNotification("TOURPLANREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), Settings.GetUTCTime(), msgUrl, "Tour Plan Request By - " + salesRepqryForSManNewTPdt.Rows[0]["SMName"] + "  " + " " + " From -" + calendarTextBox.Text + " To -" + toCalendarTextBox.Text + "  ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(DdlSalesPerson.SelectedValue), senSMid);
                                }
                            }
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully<br/> DocNo-" + docId + "');", true);
                            ClearControls();
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please add tour plan.');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill all entries in Tour Plan');", true);
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private int CheckTourPlanEntry()
        {
            int counter = 0;
            foreach (GridViewRow control in GridView1.Rows)
            {
                if (control.RowType == DataControlRowType.DataRow)
                {
                    string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                    ListBox ddlTourCity = control.FindControl("ddlArea") as ListBox;
                    ListBox ddlTourDist = control.FindControl("ddlDistributor") as ListBox;
                    ListBox ddlTourPurpose = control.FindControl("ddlPurposeVisit") as ListBox;

                    string cityIDStr = "", distStr = "", purposeStr = "";
                    foreach (ListItem tourCity in ddlTourCity.Items)
                    {
                        if (tourCity.Selected)
                        {
                            cityIDStr += tourCity.Value + ",";
                        }
                    }
                    cityIDStr = cityIDStr.TrimStart(',').TrimEnd(',');
                    if (cityIDStr != "")
                    {
                        foreach (ListItem dist in ddlTourDist.Items)
                        {
                            if (dist.Selected)
                            {
                                distStr += dist.Value + ",";
                            }
                        }
                        distStr = distStr.TrimStart(',').TrimEnd(',');
                        if (cityIDStr != "")
                        {

                            foreach (ListItem purpose in ddlTourPurpose.Items)
                            {
                                if (purpose.Selected)
                                {
                                    purposeStr += purpose.Value + ",";
                                }
                            }
                            purposeStr = purposeStr.TrimStart(',').TrimEnd(',');
                            if (purposeStr == "")
                            {
                                counter = counter + 1;
                            }
                        }
                        else
                        {
                            counter = counter + 1;
                        }

                    }
                    else
                    {
                        counter = counter + 1;
                    }
                }
            }
            return counter;
        }

        private void ClearControls()
        {
            calendarTextBox.Text = string.Empty;
            calendarTextBox.Enabled = true;            
            //DdlAreaName.SelectedIndex = 0;
            //DdlAreaName.SelectedValue = "0";
            //txtDist.Text = string.Empty;
            //DistStaff.Value = string.Empty;
           

            //Added
            fRemark.Style["display"] = "none";
            AddNewTour.Enabled = true;
            toCalendarTextBox.Text = string.Empty;
            finalRemarkTextarea.InnerText = "";
            Session["TourData"] = null;
            GridView1.DataSource = null;
            GridView1.DataBind();
            //End

            DDLPurposeVisit.SelectedIndex = 0;
            //        AccDistributor.Value = string.Empty;
            //accStaffCheckBoxList.ClearSelection();
            //           ddlApproveStatus.SelectedIndex = 0;
            approveStatusRadioButtonList.SelectedValue = "Approve";
            btnDelete.Visible = false;
            conditonaldiv.Style.Add("display", "none");
            btnSave.Style.Add("display", "inline");
            btnSave.Text = "Save";
            btnFind.Visible = true;
            btnSave.Enabled = true;
        }
        private void UpdateTourPlan()
        {
            try
            {
                if (GridView1.Rows.Count > 0)//calendarTextBox.Text != ""
                {
                    //           string TorPlanEditHeaderQry = @"select * from TransTourPlanHeader where SMId='" + Settings.dateformat(DdlSalesPerson.SelectedValue) + "' and TourPlanHId='" + ViewState["TourPlanHId"] + "'";

                    string updateTourHeaderStatQry = @"update TransTourPlanHeader set FinalRemarks='" + finalRemarkTextarea.InnerText + "' where TourPlanHId='" + ViewState["TourPlanHId"] + "' ";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateTourHeaderStatQry);

                    //              string TorPlanEditQry = @"select * from TransTourPlan where VDate='" + Settings.dateformat(calendarTextBox.Text) + "' and TourPlanId='" + ViewState["TourPlanId"] + "'";
                    foreach (GridViewRow control in GridView1.Rows)
                    {
                        if (control.RowType == DataControlRowType.DataRow)
                        {
                            string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                            Label dateLbl = control.FindControl("lblDat1") as Label;
                            HiddenField tourPlanID = control.FindControl("tourPlanHdf") as HiddenField;
                            TextBox remarkTxtBox = control.FindControl("remarkTextBox") as TextBox;
                            Label remarkLbl = control.FindControl("lblRemark") as Label;
                            ListBox ddlTourCity = control.FindControl("ddlArea") as ListBox;
                            ListBox ddlTourDist = control.FindControl("ddlDistributor") as ListBox;
                            ListBox ddlTourPurpose = control.FindControl("ddlPurposeVisit") as ListBox;
                            string cityIDStr = "", distStr = "", purposeStr = "";
                            string cityIDStrName = "", distStrName = "", puposeStrName = "";
                            foreach (ListItem tourCity in ddlTourCity.Items)
                            {
                                if (tourCity.Selected)
                                {
                                    cityIDStr += tourCity.Value + ",";
                                    cityIDStrName += tourCity.Text + ",";
                                }
                            }
                            cityIDStr = cityIDStr.TrimStart(',').TrimEnd(',');
                            cityIDStrName = cityIDStrName.TrimStart(',').TrimEnd(',');

                            if (cityIDStr != "")
                            {
                                foreach (ListItem dist in ddlTourDist.Items)
                                {
                                    if (dist.Selected)
                                    {
                                        distStr += dist.Value + ",";
                                        distStrName += dist.Text + ",";
                                    }
                                }
                                distStr = distStr.TrimStart(',').TrimEnd(',');
                                distStrName = distStrName.TrimStart(',').TrimEnd(',');

                                foreach (ListItem purpose in ddlTourPurpose.Items)
                                {
                                    if (purpose.Selected)
                                    {
                                        purposeStr += purpose.Value + ",";
                                        puposeStrName += purpose.Text + ",";
                                    }
                                }
                                purposeStr = purposeStr.TrimStart(',').TrimEnd(',');
                                puposeStrName = puposeStrName.TrimStart(',').TrimEnd(',');

                                //int retsave = tourAll.Insert(retTourPlanHId, docId, Convert.ToDateTime(dtTour.Rows[d]["TourDate"].ToString()), Convert.ToInt32(Settings.Instance.UserID), dtTour.Rows[d]["DistributorID"].ToString(), dtTour.Rows[d]["PurposeOfVisit"].ToString(), dtTour.Rows[d]["DistributorStaff"].ToString(), dtTour.Rows[d]["AccompanyingStaff"].ToString(), Convert.ToInt32(dtTour.Rows[d]["CityID"].ToString()), "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue));

                                int retsave = tourAll.Update(Convert.ToInt64(tourPlanID.Value), Convert.ToDateTime(dateLbl.Text.ToString()), Convert.ToInt32(Settings.Instance.UserID), "0", "0", string.Empty, string.Empty, "0", Convert.ToInt32(DdlSalesPerson.SelectedValue), remarkTxtBox.Text.ToString(), cityIDStr, cityIDStrName, distStr, distStrName, purposeStr, puposeStrName);
                               // int retsave = tourAll.Insert(retTourPlanHId, docId, Convert.ToDateTime(Date), Convert.ToInt32(Settings.Instance.UserID), "0", "0", string.Empty, string.Empty, 0, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), remarkTxtBox.Text, cityIDStr, cityIDStrName, distStr, distStrName, purposeStr, puposeStrName);
                            }
                        }
                    }

                    //int retsave = tourAll.Update(Convert.ToInt64(ViewState["TourPlanId"]), Convert.ToDateTime(calendarTextBox.Text), Convert.ToInt32(Settings.Instance.UserID), disId, DDLPurposeVisit.SelectedValue, DistStaff.Value, accDistStaff1, Convert.ToInt32(DdlAreaName.SelectedValue), Convert.ToInt32(DdlSalesPerson.SelectedValue));
                    //Ankita - 07/may/2016- (For Optimization)
                    string TorPlanEditQry1 = @"select SMId,DocId from TransTourPlan where TourPlanHId=" + ViewState["TourPlanHId"] + "";
                    DataTable TorPlanEditdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, TorPlanEditQry1);

                    string query = @"select TourExpApproval from MastEnviro";
                    DataTable getleaveAppStatdt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    //        var me = from env in context.MastEnviros select new { env.TourExpApproval };

                    if (Convert.ToBoolean(getleaveAppStatdt.Rows[0]["TourExpApproval"]) == true)
                    {
                        string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                        DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);
                        string salesRepqueryNew1 = "";
                        if (salesRepqueryNewdt.Rows.Count > 0)
                        {
                            salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                        }
                        //Ankita - 07/may/2016- (For Optimization)
                        //string getSeniorQSMId = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                        string getSeniorQSMId = @"select SMId from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                        DataTable salesRepqryForSManNewQdt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorQSMId);
                        int senSMid = 0;
                        if (salesRepqryForSManNewQdt.Rows.Count > 0)
                        {
                            string getSeniorSMId12 = @"select SmId from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewQdt.Rows[0]["UnderId"]) + "";
                            //string getSeniorSMId12 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewQdt.Rows[0]["UnderId"]) + "";
                            DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                            if (salesRepqryForSManNewdt12.Rows.Count > 0)
                            {
                                senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
                            }
                        }
                        DataTable salesRepqueryNewdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew1);
                        string msgUrl = "TourPlan.aspx?SMId=" + TorPlanEditdt1.Rows[0]["SMId"] + "&DocId=" + TorPlanEditdt1.Rows[0]["DocId"];

                        if (DdlSalesPerson.SelectedIndex > 0)
                        {
                        }
                        else
                        {
                            tourAll.InsertTransNotification("TOURPLANREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), Settings.GetUTCTime(), msgUrl, "Tour Plan Request By -" + salesRepqryForSManNewQdt.Rows[0]["SMName"] + "  " + " " + " Date -" + calendarTextBox.Text + " ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(DdlSalesPerson.SelectedValue), senSMid);
                        }
                    }

                    //    if (retsave != 0)
                    //      {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    ClearControls();
                    //      }

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (pageName == "TOURAPPROVAL")
            {
                Response.Redirect("~/TourPlanApproval.aspx");
            }
            else if (pageName1 == "VIEWMSG")
            {
                Response.Redirect("~/ViewAllMessages.aspx");
            }
            else
            {
                Response.Redirect("~/TourPlan.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {//Ankita - 07/may/2016- (For Optimization)
                   
                    string tourStatus = @"select AppStatus,docid,tourplanid from TransTourPlan where TourPlanHId=" + Convert.ToString( ViewState["TourPlanHId"]) + "";
                    DataTable deleteTourdt = DbConnectionDAL.GetDataTable(CommandType.Text, tourStatus);

                    if (deleteTourdt.Rows.Count > 0)
                    {
                        if (deleteTourdt.Rows[0]["AppStatus"].ToString() == "Pending")
                        {
                            int retdel = tourAll.delete(deleteTourdt.Rows[0]["docid"].ToString());
                           
                            if (retdel == 1)
                            {
                               // string getdocid=(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text,"select docid from TransTourPlan where  TourPlanHId=" + Convert.ToString( ViewState["TourPlanHId"])).ToString());
                                string msgUrl1 = "TourPlan.aspx?SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "&DocId=" + deleteTourdt.Rows[0]["docid"].ToString();
                                string deleteqry = "delete from TransNotification where msgURL='" + msgUrl1 + "'";
                                DAL.DbConnectionDAL.ExecuteNonQuery(CommandType.Text, deleteqry);
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                                ClearControls();
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('TourPlan is under process');", true);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            //      fillRepeter();
            rpt.DataSource = null;
            rpt.DataBind();
            //txtfmDate.Text = string.Empty;
            //txttodate.Text = string.Empty;
            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End

            btnSave.Style.Add("display", "inline");
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
            BindSalePersonDDl();
            
        }

        //        protected void calendarTextBox_TextChanged(object sender, EventArgs e)
        //        {
        //            DateTime fromdt = DateTime.Parse(calendarTextBox.Text);
        //            string val = fromdt.ToString("dd/MMM/yyyy");
        //            string getTourDatqry = @"select  tour.TourPlanId,
        //                                tour.DocId,
        //                                tour.VDate,
        //                                tour.DistId,
        //                                tour.Purpose,
        //                                tour.AccDistributor,
        //                                tour.CityId,
        //                                tour.AppStatus,
        //                                tour.DistStaff,tour.SMId,
        //                                tour.AppRemark from TransTourPlan tour where tour.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and tour.VDate='" + Settings.dateformat(calendarTextBox.Text) + "'";
        //            DataTable getTourDatqrydt = DbConnectionDAL.GetDataTable(CommandType.Text, getTourDatqry);
        //            if (getTourDatqrydt.Rows.Count > 0)
        //            {
        //                //ViewState["tourForDateCount"] = getTourDatqrydt.Rows.Count;
        //                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Tour is already planned for this day');", true);

        //                TextArea1.Value = getTourDatqrydt.Rows[0]["AppRemark"].ToString();
        //                DdlAreaName.SelectedValue = getTourDatqrydt.Rows[0]["CityId"].ToString();
        //                DDLPurposeVisit.SelectedValue = getTourDatqrydt.Rows[0]["Purpose"].ToString();
        //                if (getTourDatqrydt.Rows[0]["DistId"] != DBNull.Value)
        //                {
        //                    int partyId = Convert.ToInt32(getTourDatqrydt.Rows[0]["DistId"]);
        //                    string str = "select * FROM MastParty where PartyId=" + partyId + "";
        //                    DataTable dt2 = new DataTable();

        //                    dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //                    txtDist.Text = dt2.Rows[0]["PartyName"].ToString();
        //                }

        //                TextArea1.Value = getTourDatqrydt.Rows[0]["AppRemark"].ToString();
        //                DistStaff.Value = getTourDatqrydt.Rows[0]["DistStaff"].ToString();

        //                //For CheckBoxList
        //                string[] accStaffAll1 = new string[20];
        //                string accStaff = getTourDatqrydt.Rows[0]["AccDistributor"].ToString();
        //                accStaffAll1 = accStaff.Split(',');
        //                if (accStaffAll1.Length > 0)
        //                {
        //                    for (int i = 0; i < accStaffAll1.Length; i++)
        //                    {
        //                        ListItem currentCheckBox = accStaffCheckBoxList.Items.FindByText(accStaffAll1[i].ToString());
        //                        if (currentCheckBox != null)
        //                        {
        //                            currentCheckBox.Selected = true;
        //                        }
        //                    }
        //                }

        //                approveStatusRadioButtonList.Enabled = false;
        //                TextArea1.Disabled = true;
        //                if (getTourDatqrydt.Rows[0]["AppStatus"].ToString() == "Approve" || getTourDatqrydt.Rows[0]["AppStatus"].ToString() == "Reject")
        //                {
        //                    approveStatusRadioButtonList.SelectedValue = getTourDatqrydt.Rows[0]["AppStatus"].ToString();
        //                    conditonaldiv.Style.Add("display", "block");
        //                }
        //                else
        //                {
        //                    approveStatusRadioButtonList.SelectedValue = "Approve";
        //                    conditonaldiv.Style.Add("display", "none");
        //                }
        //                if (getTourDatqrydt.Rows[0]["AppRemark"].ToString() != "")
        //                {
        //                    TextArea1.Value = getTourDatqrydt.Rows[0]["AppRemark"].ToString();
        //                }
        //                if (getTourDatqrydt.Rows[0]["AppStatus"].ToString() == "Approve" || getTourDatqrydt.Rows[0]["AppStatus"].ToString() == "Reject")
        //                {
        //                    //btnSave.Style.Add("display", "none");
        //                    //btnSave.Attributes["disabled"] = "disabled";
        //                    //btnSave.Enabled = false;
        //                    //btnSave.CssClass = "btn btn-primary";

        //                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);    
        //                }
        //                else
        //                {
        //                    btnSave.Style.Add("display", "inline");
        //                }
        //            }
        //            else
        //            {
        //                TextArea1.Value = string.Empty;
        //                DdlAreaName.SelectedIndex = 0;
        //                DDLPurposeVisit.SelectedIndex = 0;
        //                //         DdlAreaName_SelectedIndexChanged(null, null);
        //                //         Ddldistributor.Items.Clear();
        //                txtDist.Text = string.Empty;
        //                conditonaldiv.Style.Add("display", "none");
        //            }
        //        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (TextArea1.Value != string.Empty)
            {
                UpdateTourPlanByNotify();
            }
            else
            {
                conditonaldiv.Style.Add("display", "block");
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Remark');", true);
                GetTourPlanData(Request.QueryString["SMId"], Request.QueryString["DocId"]);
            }
        }

        //protected void DdlAreaName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string dataDistributorByArea = string.Empty;
        //    try
        //    {
        //        string getDisQry = @"select dist.PartyId, dist.PartyName from MastParty dist where dist.PartyDist =1 and  dist.CityId =" + Convert.ToInt32(DdlAreaName.SelectedValue) + " order by dist.PartyId";
        //        DataTable getDisQrydt = DbConnectionDAL.GetDataTable(CommandType.Text, getDisQry);
        //        if (getDisQrydt.Rows.Count > 0)
        //        {
        //            Ddldistributor.DataSource = getDisQrydt;
        //            Ddldistributor.DataTextField = "PartyName";
        //            Ddldistributor.DataValueField = "PartyId";
        //            Ddldistributor.DataBind();
        //        }
        //        Ddldistributor.Items.Insert(0, new ListItem("-- Select Distributor --", "0"));
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //    //return dataDistributorByArea;
        //}
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDist(string prefixText, string contextKey)
        { //Ankita - 07/may/2016- (For Optimization)
            //string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and Active=1 and CityId=" + Convert.ToInt32(contextKey) + "";
            string str = "select mp.PartyName,mp.SyncId,mp.PartyId,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId=" + Convert.ToInt32(contextKey) + "";

            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["PartyName"].ToString() + ")" + " " + dt.Rows[i]["SyncId"].ToString() + " " + "(" + dt.Rows[i]["AreaName"].ToString() + ")", dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

        protected void DdlAreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtDist.Text = string.Empty;
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
            string smIDStr = "", qrychk = "";
            string smIDStr1 = "";
            if (dtSMId.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSMId.Rows)
                {
                    smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                    //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            }

            //Added as per UAT Date-08-12-2015 
            if (DropDownList1.SelectedIndex != 0)
            {
                qrychk = "and r.SMId=" + Convert.ToInt32(DropDownList1.SelectedValue) + "";
            }
            else
            {
                qrychk = "and r.SMId in (" + smIDStr1 + ")";
            }
            //End

            //        if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty && DropDownList1.SelectedIndex != 0)
            //        {
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            {
                string tourquery = @"select r.TourPlanHId,r.DocId, r.VDate,r.AppStatus,msr.SMName
                                    from TransTourPlanHeader r left join MastSalesRep msr on msr.SMId =r.SMId
                                    where r.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59' " + qrychk + " order by r.VDate desc";

                //                string tourquery = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.DistId, r.Purpose, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,msr.SMName,mpv.PurposeName
                //                                    from TransTourPlan r 
                //                                    left join MastPurposeVisit mpv on r.Purpose=Convert(varchar,mpv.ID)
                //                                    left join MastArea Area on r.CityId=Area.AreaId left join MastSalesRep msr on msr.SMId=r.SMId
                //                                    where VDate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' " + qrychk + " order by VDate desc";
                DataTable tourqrydt1 = DbConnectionDAL.GetDataTable(CommandType.Text, tourquery);
                if (tourqrydt1.Rows.Count > 0)
                {
                    rpt.DataSource = tourqrydt1;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                rpt.DataSource = null;
                rpt.DataBind();
            }
            //      }
            //            else if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty)
            //            {
            //                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            //                {
            //                    string tourquery = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.DistId, r.Purpose, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,msr.SMName,mpv.PurposeName
            //                                    from TransTourPlan r 
            //                                    left join MastPurposeVisit mpv on r.Purpose=Convert(varchar,mpv.ID)
            //                                    left join MastArea Area on r.CityId=Area.AreaId left join MastSalesRep msr on msr.SMId=r.SMId
            //                                    where r.SMId in (" + smIDStr1 + ") and VDate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' order by VDate desc";
            //                    DataTable tourqrydt2 = DbConnectionDAL.GetDataTable(CommandType.Text, tourquery);
            //                    if (tourqrydt2.Rows.Count > 0)
            //                    {
            //                        rpt.DataSource = tourqrydt2;
            //                        rpt.DataBind();
            //                    }
            //                    else
            //                    {
            //                        rpt.DataSource = null;
            //                        rpt.DataBind();
            //                    }
            //                }
            //                else
            //                {
            //                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
            //                    rpt.DataSource = null;
            //                    rpt.DataBind();
            //                }
            //            }
            //            else if (txtfmDate.Text != string.Empty)
            //            {
            //                string tourqueryNew = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.DistId, r.Purpose, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,msr.SMName,mpv.PurposeName
            //                                    from TransTourPlan r
            //                                    left join MastPurposeVisit mpv on r.Purpose=Convert(varchar,mpv.ID)
            //                                    left join MastArea Area on r.CityId=Area.AreaId left join MastSalesRep msr on msr.SMId=r.SMId
            //                                    where r.SMId in (" + smIDStr1 + ") and VDate >= '" + Settings.dateformat(txtfmDate.Text) + "' order by VDate desc";
            //                DataTable tourqrydt3 = DbConnectionDAL.GetDataTable(CommandType.Text, tourqueryNew);
            //                if (tourqrydt3.Rows.Count > 0)
            //                {
            //                    rpt.DataSource = tourqrydt3;
            //                    rpt.DataBind();
            //                }
            //                else
            //                {
            //                    rpt.DataSource = null;
            //                    rpt.DataBind();
            //                }
            //            }
            //            else if (DropDownList1.SelectedIndex != 0)
            //            {
            //                string tourqueryNew = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.DistId, r.Purpose, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,msr.SMName,mpv.PurposeName
            //                                    from TransTourPlan r
            //                                    left join MastPurposeVisit mpv on r.Purpose=Convert(varchar,mpv.ID)
            //                                    left join MastArea Area on r.CityId=Area.AreaId left join MastSalesRep msr on msr.SMId=r.SMId
            //                                    where r.SMId=" + Convert.ToInt32(DropDownList1.SelectedValue) + " order by VDate desc";
            //                DataTable tourqrydt4 = DbConnectionDAL.GetDataTable(CommandType.Text, tourqueryNew);
            //                if (tourqrydt4.Rows.Count > 0)
            //                {
            //                    rpt.DataSource = tourqrydt4;
            //                    rpt.DataBind();
            //                }
            //                else
            //                {
            //                    rpt.DataSource = null;
            //                    rpt.DataBind();
            //                }
            //            }
            //            else
            //            {
            //                string tourquery3 = @"select r.TourPlanId,r.DocId, r.VDate, Area.AreaName, Area.AreaId, r.DistId, r.Purpose, r.AccDistributor,r.AppRemark,r.AppStatus,r.SMId,msr.SMName,mpv.PurposeName
            //                                    from TransTourPlan r
            //                                    left join MastPurposeVisit mpv on r.Purpose=Convert(varchar,mpv.ID)
            //                                    left join MastArea Area on r.CityId=Area.AreaId left join MastSalesRep msr on msr.SMId=r.SMId
            //                                    where r.SMId in (" + smIDStr1 + ") order by VDate desc";
            //                DataTable tourqrydt5 = DbConnectionDAL.GetDataTable(CommandType.Text, tourquery3);
            //                if (tourqrydt5.Rows.Count > 0)
            //                {
            //                    rpt.DataSource = tourqrydt5;
            //                    rpt.DataBind();
            //                }
            //                else
            //                {
            //                    rpt.DataSource = null;
            //                    rpt.DataBind();
            //                }
            //            }
        }

        protected void DdlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sMId1 = DdlSalesPerson.SelectedValue;
            //BindStaffChkList(accStaffCheckBoxList, sMId1);
            //BindAreaAsUser(DdlAreaName, Convert.ToInt32(sMId1));
        }

        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            //txtfmDate.Text = string.Empty;
            //txttodate.Text = string.Empty;

            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End
            rpt.DataSource = null;
            rpt.DataBind();
        }

        protected void AddNewTour_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(toCalendarTextBox.Text) >= Convert.ToDateTime(calendarTextBox.Text))//calendarTextBox.Text != "" && toCalendarTextBox.Text != "")
                {
                    //string tourPlanqry = @"select * from TransTourPlan tour where tour.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and tour.VDate='" + Settings.dateformat(calendarTextBox.Text) + "' and tour.AppStatus<>'Reject'";

                    //DataTable gettorPlandt = DbConnectionDAL.GetDataTable(CommandType.Text, tourPlanqry);

                    //if (gettorPlandt.Rows.Count > 0)
                    //{
                    //    if (gettorPlandt.Rows[0]["AppStatus"].ToString() == "Approve")
                    //    {
                    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Tour is already approved for this day.');", true);
                    //    }
                    //    else if (gettorPlandt.Rows[0]["AppStatus"].ToString() == "Reject")
                    //    {
                    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Tour is already rejected for this Day.');", true);
                    //    }
                    //    else
                    //    {
                    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists.');", true);
                    //    }
                    //    ClearControls();
                    //}
                    //               else
                    //               {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[6] { new DataColumn("TourPlanId", typeof(int)),
                            new DataColumn("VDate", typeof(DateTime)),new DataColumn("MCityId", typeof(string)),new DataColumn("MDistId", typeof(string)),new DataColumn("MPurposeId", typeof(string)),new DataColumn("Remarks", typeof(string))});
                    DateTime Dt = Convert.ToDateTime(calendarTextBox.Text);
                    DateTime toDt = Convert.ToDateTime(toCalendarTextBox.Text);
                    double diff = (Convert.ToDateTime(toCalendarTextBox.Text) - Convert.ToDateTime(calendarTextBox.Text)).TotalDays;
                    dt.Rows.Add(0, Dt);
                    if (diff==0)
                    {  //Ankita - 07/may/2016- (For Optimization)
                        //string str1 = @"select * from TransTourPlan tour where tour.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and tour.VDate='" + Settings.dateformat(dt.Rows[0]["VDate"].ToString()) + "' and tour.AppStatus<>'Reject'";
                        //DataTable dtcheck = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                        int dtcheck = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from TransTourPlan tour where tour.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and tour.VDate='" + Settings.dateformat(dt.Rows[0]["VDate"].ToString()) + "' and tour.AppStatus<>'Reject'"));
                        if (dtcheck > 0) 
                        {
                                //   if(dtcheck.Rows.Count > 0)
                                DateTime tourDat = DateTime.Parse(dt.Rows[0]["VDate"].ToString());
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists for Date:" + tourDat.ToString("dd/MMM/yyyy") + "');", true);
                            return;}
                    }
                    for (int i = 1; i < diff + 1; i++)
                    {//Ankita - 07/may/2016- (For Optimization)
                      //  string tourPlanqry = @"select * from TransTourPlan tour where tour.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and tour.VDate='" + Settings.dateformat(dt.Rows[i - 1]["VDate"].ToString()) + "' and tour.AppStatus<>'Reject'";
                      // DataTable gettorPlandt = DbConnectionDAL.GetDataTable(CommandType.Text, tourPlanqry);
                        int gettorPlandt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select count(*) from TransTourPlan tour where tour.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and tour.VDate='" + Settings.dateformat(dt.Rows[i - 1]["VDate"].ToString()) + "' and tour.AppStatus<>'Reject'"));
                  //      if (!(gettorPlandt.Rows.Count > 0))
                        if (!(gettorPlandt > 0))
                        {
                            dt.Rows.Add(i, Dt.AddDays(i));
                        }
                        else
                        {
                            DateTime tourDat1 = DateTime.Parse(dt.Rows[i - 1]["VDate"].ToString());
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists for Date:" + tourDat1.ToString("dd/MMM/yyyy") + "');", true);
                            return;
                        }
                    }
                   
                    if (dt.Rows.Count > 0)
                    {
                        fRemark.Style["display"] = "block";
                    }
                    DdlSalesPerson.Enabled = false;
                    Session["TourData"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    ListBox ddlareaDP, ddlpurposeVisit;
                    string custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + DdlSalesPerson.SelectedValue + " )) and  areatype='city' and Active=1 order by AreaName";
                    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, custQuery2);
                    string purpQry = @"select Id,PurposeName from MastPurposeVisit order by PurposeName";
                    DataTable dtPurp = DbConnectionDAL.GetDataTable(CommandType.Text, purpQry);
                    for (int j = 0; j < diff+1 ; j++)
                    {
                         ddlareaDP = (GridView1.Rows[j].FindControl("ddlArea") as ListBox);
                         ddlpurposeVisit = (GridView1.Rows[j].FindControl("ddlPurposeVisit") as ListBox);
                         if (dtChild.Rows.Count > 0)
                         {
                             ddlareaDP.DataSource = dtChild;
                             ddlareaDP.DataTextField = "AreaName";
                             ddlareaDP.DataValueField = "AreaId";
                             ddlareaDP.DataBind();
                           //  ddlareaDP.Items.Insert(0, new ListItem("Please select"));
                         }
                         if (dtPurp.Rows.Count > 0)
                         {
                             ddlpurposeVisit.DataSource = dtPurp;
                             ddlpurposeVisit.DataTextField = "PurposeName";
                             ddlpurposeVisit.DataValueField = "Id";
                             ddlpurposeVisit.DataBind();
                           //  ddlpurposeVisit.Items.Insert(0, new ListItem("Please select"));
                         }
                    }
                   
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (isEdit == "1")
                    {
                        string sMId1 = DdlSalesPerson.SelectedValue;
                        //string custQuery2 = "";
                        //if (Request.QueryString["SMId"] != null)
                        //{  //Ankita - 07/may/2016- (For Optimization)
                        //    DdlSalesPerson.SelectedValue = Request.QueryString["SMId"].ToString();
                        //   // custQuery2 = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Request.QueryString["SMId"].ToString() + " )) and  areatype='city' and Active=1 order by AreaName";
                        //    custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Request.QueryString["SMId"].ToString() + " )) and  areatype='city' and Active=1 order by AreaName";
                        //}
                        //else
                        //{ // custQuery2 = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Request.QueryString["SMId"].ToString() + " )) and  areatype='city' and Active=1 order by AreaName";
                        //    custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + DdlSalesPerson.SelectedValue + " )) and  areatype='city' and Active=1 order by AreaName";
                        //}
                        //TextBox remarkTxtBox = (e.Row.FindControl("remarkTextBox") as TextBox);
                        //Label lblremarks = (e.Row.FindControl("lblRemark") as Label);
                        //Label lblCityID = (e.Row.FindControl("lblCity") as Label);
                        //Label lblDistId = (e.Row.FindControl("lblDist") as Label);
                        //Label lblPurpose = (e.Row.FindControl("lblPurp") as Label);
                        //ListBox ddlareaDP = (e.Row.FindControl("ddlArea") as ListBox);
                        //ListBox ddldistDP = (e.Row.FindControl("ddlDistributor") as ListBox);
                        //ListBox ddlpurposeVisit = (e.Row.FindControl("ddlPurposeVisit") as ListBox);

                        ////       For CheckBoxList City
                        //remarkTxtBox.Text = lblremarks.Text;
                        //DataTable dtChild1 = DbConnectionDAL.GetDataTable(CommandType.Text, custQuery2);
                        //if (dtChild1.Rows.Count > 0)
                        //{
                        //    ddlareaDP.DataSource = dtChild1;
                        //    ddlareaDP.DataTextField = "AreaName";
                        //    ddlareaDP.DataValueField = "AreaId";
                        //    ddlareaDP.DataBind();
                        //}
                        //string[] cityIdStrAll = new string[1000];
                        //string cityIdStr = lblCityID.Text.ToString();
                        //cityIdStrAll = cityIdStr.Split(',');
                        //if (cityIdStrAll.Length > 0)
                        //{
                        //    for (int i = 0; i < cityIdStrAll.Length; i++)
                        //    {
                        //        ListItem currentCheckBox = ddlareaDP.Items.FindByValue(cityIdStrAll[i].ToString());
                        //        if (currentCheckBox != null)
                        //        {
                        //            currentCheckBox.Selected = true;
                        //        }
                        //    }
                        //}

                        ////       For CheckBoxList Distributor
                        //if (cityIdStr != "")
                        //{
                        //    string str = "select mp.PartyId,mp.PartyName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyDist=1 and mp.CityId in (" + cityIdStr + ")";
                        //    DataTable dt = new DataTable();
                        //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        //    ddldistDP.DataSource = dt;
                        //    ddldistDP.DataTextField = "PartyName";
                        //    ddldistDP.DataValueField = "PartyId";
                        //    ddldistDP.DataBind();
                        //}
                        //string[] distIdStrAll = new string[1000];
                        //string distIdStr = lblDistId.Text.ToString();
                        //distIdStrAll = distIdStr.Split(',');
                        //if (distIdStrAll.Length > 0)
                        //{
                        //    for (int i = 0; i < distIdStrAll.Length; i++)
                        //    {
                        //        ListItem currentCheckBox = ddldistDP.Items.FindByValue(distIdStrAll[i].ToString());
                        //        if (currentCheckBox != null)
                        //        {
                        //            currentCheckBox.Selected = true;
                        //        }
                        //    }
                        //}
                        ////Ankita - 07/may/2016- (For Optimization)
                        ////       For CheckBoxList Purpose Of Visit
                        ////string purpQry = @"select * from MastPurposeVisit order by PurposeName";
                        //string purpQry = @"select Id,PurposeName from MastPurposeVisit order by PurposeName";
                        //DataTable dtPurp = DbConnectionDAL.GetDataTable(CommandType.Text, purpQry);
                        //if (dtPurp.Rows.Count > 0)
                        //{
                        //    ddlpurposeVisit.DataSource = dtPurp;
                        //    ddlpurposeVisit.DataTextField = "PurposeName";
                        //    ddlpurposeVisit.DataValueField = "Id";
                        //    ddlpurposeVisit.DataBind();
                        //}
                        //string[] purpIdStrAll = new string[1000];
                        //string purpIdStr = lblPurpose.Text.ToString();
                        //purpIdStrAll = purpIdStr.Split(',');
                        //if (purpIdStrAll.Length > 0)
                        //{
                        //    for (int i = 0; i < purpIdStrAll.Length; i++)
                        //    {
                        //        ListItem currentCheckBox = ddlpurposeVisit.Items.FindByValue(purpIdStrAll[i].ToString());
                        //        if (currentCheckBox != null)
                        //        {
                        //            currentCheckBox.Selected = true;
                        //        }
                        //    }
                        //}


                        //DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, custQuery2);
                        //if (dtChild.Rows.Count > 0)
                        //{
                        //    ddlareaDP.DataSource = dtChild;
                        //    ddlareaDP.DataTextField = "AreaName";
                        //    ddlareaDP.DataValueField = "AreaId";
                        //    ddlareaDP.DataBind();
                        //}

                    }
                    else
                    {
                        string sMId1 = DdlSalesPerson.SelectedValue;
                        //           BindAreaAsUser(DdlAreaName, Convert.ToInt32(sMId1));
                        //Ankita - 09/may/2016- (For Optimization)
                       // ListBox ddlareaDP = (e.Row.FindControl("ddlArea") as ListBox);
                       // ListBox ddlpurposeVisit = (e.Row.FindControl("ddlPurposeVisit") as ListBox);
                       // //string custQuery2 = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + DdlSalesPerson.SelectedValue + " )) and  areatype='city' and Active=1 order by AreaName";
                       // string custQuery2 = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + DdlSalesPerson.SelectedValue + " )) and  areatype='city' and Active=1 order by AreaName";

                       // DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, custQuery2);
                       // if (dtChild.Rows.Count > 0)
                       // {
                       //     ddlareaDP.DataSource = dtChild;
                       //     ddlareaDP.DataTextField = "AreaName";
                       //     ddlareaDP.DataValueField = "AreaId";
                       //     ddlareaDP.DataBind();
                       // }

                       //// string purpQry = @"select * from MastPurposeVisit order by PurposeName";
                       // string purpQry = @"select Id,PurposeName from MastPurposeVisit order by PurposeName";
                       // DataTable dtPurp = DbConnectionDAL.GetDataTable(CommandType.Text, purpQry);
                       // if (dtPurp.Rows.Count > 0)
                       // {
                       //     ddlpurposeVisit.DataSource = dtPurp;
                       //     ddlpurposeVisit.DataTextField = "PurposeName";
                       //     ddlpurposeVisit.DataValueField = "Id";
                       //     ddlpurposeVisit.DataBind();
                       // }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string cityIDStr = "";
                ListBox ddl = sender as ListBox;
                GridViewRow row = (GridViewRow)ddl.Parent.Parent;
                ListBox ddlDistList = (row.FindControl("ddlDistributor") as ListBox);
                foreach (ListItem saleperson in ddl.Items)
                {
                    if (saleperson.Selected)
                    {
                        cityIDStr += saleperson.Value + ",";
                    }
                }
                cityIDStr = cityIDStr.TrimStart(',').TrimEnd(',');
                BindDistributorDDl(Settings.Instance.SMID,cityIDStr,ddlDistList);
                //if (cityIDStr != "")
                //{
                //    string str = "select mp.PartyId,mp.PartyName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyDist=1 and mp.CityId in (" + cityIDStr + ")";
                //    DataTable dt = new DataTable();
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //    ddlDistList.DataSource = dt;
                //    ddlDistList.DataTextField = "PartyName";
                //    ddlDistList.DataValueField = "PartyId";
                //    ddlDistList.DataBind();
                //}
                //else
                //{
                //    ddlDistList.Items.Clear();
                //}
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindDistributorDDl(string SMIDStr, string cityid, ListBox ddlDistList)
        {
            try
            {
                if (cityid != "")
                {                    
                    string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + cityid + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ddlDistList.DataSource = dtDist;
                        ddlDistList.DataTextField = "PartyName";
                        ddlDistList.DataValueField = "PartyId";
                        ddlDistList.DataBind();
                    }
                }
               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}