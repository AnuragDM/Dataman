using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using BusinessLayer;
using DAL;
using System.Web.UI.HtmlControls;
using System.IO;

namespace AstralFFMS
{
    public partial class BeatPlanEntry : System.Web.UI.Page
    {
        int msg = 0;
        int smID = 0;
        int isCurrentUser = 0;
        int Flag = 0;
        int Flag1 = 0;
        DataTable leavedt;
        DataTable holidaydt;
        int viewFlag = 0;
        int gridRowCount = 0;
        int smIDSen = 0;
        int showSaveBtn = 0;
        int showUpdateBtn = 0;
        int chkRefreshStatus = 0;
        string RtypeSp = "";
        DataTable dt;
        DataTable dtHoliday;
        BAL.TransBeatPlan.BeatPlan btAll = new BAL.TransBeatPlan.BeatPlan();
        string pageName = string.Empty;
        string pageName1 = string.Empty;
        string roleType = "";
        string parameter = "";
     
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack) { Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString()); }
            calendarTextBox.Attributes.Add("readonly", "readonly");
            //Ankita - 04/may/2016- (For Optimization)
            roleType = Settings.Instance.RoleType;
            // GetRoleType(Settings.Instance.RoleID);
            pageName = string.Empty;
            pageName1 = string.Empty;
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["BeatDocId"] = parameter;
                if (ViewState["BeatDocId"] != null)
                {
                    FillBeatControls((ViewState["BeatDocId"].ToString()));
                    mainDiv.Style.Add("display", "block");
                    rptmain.Style.Add("display", "none");
                }
            }        

            string pageName12 = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName12);
            lblPageHeader.Text = Pageheader;
            string PermAll= Settings.Instance.CheckPagePermissions(pageName12, Convert.ToString(Session["user_name"]));
            string []SplitPerm=PermAll.Split(',');
          
            if (Btnsave.Text == "Save")
            {
             //   Btnsave.Enabled = Settings.Instance.CheckAddPermission(pageName12, Convert.ToString(Session["user_name"]));
                Btnsave.Enabled =Convert.ToBoolean(SplitPerm[1]);
                Btnsave.CssClass = "btn btn-primary";
            }
            else
            {
                Btnsave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //Btnsave.Enabled = Settings.Instance.CheckEditPermission(pageName12, Convert.ToString(Session["user_name"]));
                Btnsave.CssClass = "btn btn-primary";
            }
               btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
         //   btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName12, Convert.ToString(Session["user_name"]));
               btnDelete.CssClass = "btn btn-primary";


            if (Request.QueryString["Page"] != null)
            {
                pageName = Request.QueryString["Page"];
            }
            if (Request.QueryString["PageV"] != null)
            {
                pageName1 = Request.QueryString["PageV"];
            }
            smID = Convert.ToInt32(Settings.Instance.SMID);

            if (!IsPostBack)
            {
                smIDSen = Convert.ToInt32(Settings.Instance.SMID);
                BindSalePersonDDl();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["SMId"] != null && Request.QueryString["DocId"] != null)
                {
                   
                    showSaveBtn = 1;
                    userIDHiddenField.Value = Request.QueryString["SMId"];
                    docIDHiddenField.Value = Request.QueryString["DocId"];
                    CheckBeatPlanData(Request.QueryString["SMId"], Request.QueryString["DocId"]);
                    CheckIsCurrentUser();
                    Flag = 1;
                    ShowBeatPlanData();
                }
                else
                {
                    if (!Convert.ToBoolean(SplitPerm[0])) { ShowAlert("You do not have Permission to view this form. Please Contact System Admin.!!"); return; }
                    Flag = 0;
                    BtnSubmit.Visible = false;
                    Btnsave.Visible = false;
                    btnDelete.Visible = false;
                    BtnCancel.Visible = false;
                    //         BtnCancel.Visible = false;
                }
            }
            //parameter = Request["__EVENTARGUMENT"];
            //if (parameter != "")
            //{
            //    ViewState["BeatDocId"] = parameter;
            //    if (ViewState["BeatDocId"] != null)
            //    {
            //        FillBeatControls((ViewState["BeatDocId"].ToString()));
            //        mainDiv.Style.Add("display", "block");
            //        rptmain.Style.Add("display", "none");
            //    }
            //}           

        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        public string  GetRoleSP() {
            string str = "SELECT mr.Roletype FROM MastSalesRep msr INNER JOIN MastRole mr ON msr.RoleId=mr.RoleId where msr.Smid="+DdlSalesPerson.SelectedValue+"";
        return  RtypeSp = DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();
        }
        private static int GetSalesPerId(int uid)
        {            
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
        private void FillBeatControls(string bDocID)
        {
            try
            {
                //string showbeatPlanquery = @"select * from TransBeatPlan where DocId='" + bDocID + "'";
                string showbeatPlanquery = @"select StartDate from TransBeatPlan where DocId='" + bDocID + "'";
                DataTable beatPlandt = DbConnectionDAL.GetDataTable(CommandType.Text, showbeatPlanquery);
                if (beatPlandt.Rows.Count > 0)
                {
                    Flag = 1;
                    calendarTextBox.Text = Convert.ToDateTime(beatPlandt.Rows[0]["StartDate"]).ToString("dd/MMM/yyyy");
                    ShowBeatPlanDataViaGridClick(1);
                    Flag = 0;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void ShowBeatPlanDataViaGridClick(int value)
        {
            try
            {
                string showbeatDtquery1 = string.Empty;
                if (value == 1)
                {
                    showbeatDtquery1 = @"select TransBeat.BeatPlanId, TransBeat.StartDate,TransBeat.SMId, TransBeat.DocId, TransBeat.AppStatus, TransBeat.AppRemark from TransBeatPlan TransBeat where TransBeat.DocId='" + ViewState["BeatDocId"] + "'";
                }
                DataTable showbeatQryDtNew = DbConnectionDAL.GetDataTable(CommandType.Text, showbeatDtquery1);
                if (showbeatQryDtNew.Rows.Count > 0)
                {
                    DateTime todt = DateTime.Parse(showbeatQryDtNew.Rows[0]["StartDate"].ToString());
                    startDateHiddenField.Value = todt.ToString("dd/MMM/yyyy");
                    DdlSalesPerson.SelectedValue = showbeatQryDtNew.Rows[0]["SMId"].ToString();
                    if (showbeatQryDtNew.Rows[0]["AppStatus"].ToString() == "Approve" || showbeatQryDtNew.Rows[0]["AppStatus"].ToString() == "Reject")
                    {
                        approveStatusRadioButtonList.SelectedValue = showbeatQryDtNew.Rows[0]["AppStatus"].ToString();
                        conditonaldiv.Style.Add("display", "block");
                    }
                    else
                    {
                        approveStatusRadioButtonList.SelectedValue = "Approve";
                        conditonaldiv.Style.Add("display", "none");
                    }
                    if (showbeatQryDtNew.Rows[0]["AppRemark"].ToString() != "")
                    {
                        RemarkArea.Value = showbeatQryDtNew.Rows[0]["AppRemark"].ToString();
                    }
                    Btnsave.Visible = true;
                    Btnsave.Text = "Update";
                    btnDelete.Visible = true;
                    btnFind.Visible = true;
                    BtnCancel.Visible = true;
                    if (showbeatQryDtNew.Rows[0]["AppStatus"].ToString() == "Approve" || showbeatQryDtNew.Rows[0]["AppStatus"].ToString() == "Reject")
                    {
                        Btnsave.Enabled = false;
                        Btnsave.Visible = false;
                        showUpdateBtn = 1;
                        btnDelete.Enabled = false;
                        btnDelete.Visible = false;
                        Btnsave.CssClass = "btn btn-primary";
                        btnDelete.CssClass = "btn btn-primary";
                        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
                    }
                    else
                    {
                        Btnsave.Enabled = true;
                        Btnsave.Visible = true;
                        btnDelete.Visible = true;
                        btnDelete.Enabled = true;
                    }
                }
                BindGridViewData(ViewState["BeatDocId"].ToString());
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
        public void ViewP()
        {
            string pageName = Path.GetFileName(Request.Path);
            bool addP = Settings.Instance.CheckViewPermission(pageName, Convert.ToString(Session["user_name"]));
            if (addP == true)
            {
                viewFlag = 1;
            }
        }
        private void BindSalePersonDDl()
        {  //Ankita - 03/may/2016- (For Optimization)
            //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
            //DataTable dtRole = new DataTable();
            //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
            string RoleTy = Settings.Instance.RoleType;
            if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
            {
                DataTable dt1 = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt1);
                dv.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge'   and SMName<>'.'";
                DdlSalesPerson.DataSource = dv;
                DdlSalesPerson.DataTextField = "SMName";
                DdlSalesPerson.DataValueField = "SMId";
                DdlSalesPerson.DataBind();
                //Add Default Item in the DropDownList
                DropDownList1.DataSource = dv;
                DropDownList1.DataTextField = "SMName";
                DropDownList1.DataValueField = "SMId";
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("--Please select--"));
                DdlSalesPerson.SelectedValue = Settings.Instance.SMID;
                DropDownList1.SelectedValue = Settings.Instance.SMID;
            }
            else if (RoleTy == "AreaIncharge")
            {
                DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt);
                dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
                //  dv.Sort = "SMName asc";
                DdlSalesPerson.DataSource = dv;
                DdlSalesPerson.DataTextField = "SMName";
                DdlSalesPerson.DataValueField = "SMId";
                DdlSalesPerson.DataBind();
                //Add Default Item in the DropDownList
                DropDownList1.DataSource = dv;
                DropDownList1.DataTextField = "SMName";
                DropDownList1.DataValueField = "SMId";
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                DataTable dt1 = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt1);
                dv.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge' and SMName<>'.'";
                dv.Sort = "SMName asc";
                DdlSalesPerson.DataSource = dv;
                DdlSalesPerson.DataTextField = "SMName";
                DdlSalesPerson.DataValueField = "SMId";
                DdlSalesPerson.DataBind();
                DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));

                DropDownList1.DataSource = dv;
                DropDownList1.DataTextField = "SMName";
                DropDownList1.DataValueField = "SMId";
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("--Please select--"));

            }
        }
        private void CheckBeatPlanData(string userID, string docID)
        {//Ankita - 03/may/2016- (For Optimization)
            //string beatquery = @"select * from TransBeatPlan where SMId=" + Convert.ToInt32(userID) + " and DocId='" + docID + "'";
            string beatquery = @"select count(*) from TransBeatPlan where SMId=" + Convert.ToInt32(userID) + " and DocId='" + docID + "'";
            int cntval =Convert.ToInt32( DbConnectionDAL.GetScalarValue(CommandType.Text, beatquery));
           // DataTable beatQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, beatquery);

            //if (beatQryDt.Rows.Count > 0)
            if (cntval > 0)
            {
                chkBeatDataHdf.Value = "1";
            }
            else
            {
                chkBeatDataHdf.Value = "0";
            }
        }


      

        private void ShowBeatPlanData()
        {
            string showbeatDtquery = @"select TransBeat.BeatPlanId, TransBeat.StartDate, TransBeat.DocId,TransBeat.SMId, TransBeat.AppStatus, TransBeat.AppRemark from TransBeatPlan TransBeat where TransBeat.SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and TransBeat.DocId='" + Request.QueryString["DocId"] + "'";

            DataTable showbeatQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, showbeatDtquery);
            if (showbeatQryDt.Rows.Count > 0)
            {
                DateTime todt = DateTime.Parse(showbeatQryDt.Rows[0]["StartDate"].ToString());
                startDateHiddenField.Value = todt.ToString("dd/MMM/yyyy");
                approveStatusRadioButtonList.SelectedValue = showbeatQryDt.Rows[0]["AppStatus"].ToString();
                appStatusHiddenField.Value = showbeatQryDt.Rows[0]["AppStatus"].ToString();
                DdlSalesPerson.SelectedValue = showbeatQryDt.Rows[0]["SMId"].ToString();
                if (appStatusHiddenField.Value == "Approve" || appStatusHiddenField.Value == "Reject")
                {
                    BtnSubmit.Enabled = false;
                    BtnSubmit.Visible = false;
                    btnDelete.Visible = false;
                    BtnSubmit.CssClass = "btn btn-primary";
                    Btnsave.Enabled = false;
                    Btnsave.CssClass = "btn btn-primary";
                }
                else
                {
                    Btnsave.Visible = false;
                    BtnSubmit.Visible = true;
                    BtnSubmit.Enabled = true;
                    btnDelete.Visible = false;
                    btnDelete.Visible = false;
                }
                RemarkArea.Value = showbeatQryDt.Rows[0]["AppRemark"].ToString();
            }
            BindGridViewData(Request.QueryString["DocId"]);
        }

        private void BindGridViewData(string p)
        {
            try
            {
                //            string str1 = @"select t.PlannedDate as Date,t.CityId,t.AreaId,t.BeatId,t.SMId,t.BeatPlanId,t.StartDate,t.AppStatus,t.AppRemark,a.AreaName as CityName,b.AreaName as AreaName,c.AreaName as BeatName from TransBeatPlan t
                //              left join MastArea a on t.CityId=a.AreaId
                //              left join MastArea b on t.AreaId=b.AreaId
                //              left join MastArea c on t.BeatId=c.AreaId where t.DocId ='" + p + "'";
                string str1 = @"select t.PlannedDate as Date,t.CityId,t.AreaId,t.BeatId,t.SMId,t.BeatPlanId,t.StartDate,t.AppStatus,t.AppRemark from TransBeatPlan t
               where t.DocId ='" + p + "'";
                DataTable St = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                GridView1.DataSource = St;
                GridView1.DataBind();
                // Fill Dropdowns for grid for AREA
                //Ankita - 03/may/2016- (For Optimization)
                string areaQuery2 = " Select AreaId,AreaName from  [View_Area_SalesPerson_Permission] where PrimCode=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";

                DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaQuery2);
                DataRow dr = dtArea.NewRow();
                dr[0] = "-7";
                dr[1] = "Week Off";
                dtArea.Rows.Add(dr);

                DropDownList ddlareaDP;
                for (int i = 0; i < 7; i++)
                {
                    ddlareaDP = (GridView1.Rows[i].FindControl("ddlArea") as DropDownList);
                    if (ddlareaDP.SelectedValue != "Holiday")
                    {
                        ddlareaDP.DataTextField = "AreaName";
                        ddlareaDP.DataValueField = "AreaId";
                        ddlareaDP.DataSource = dtArea;

                        ddlareaDP.DataBind(); 
                        ddlareaDP.Items.Insert(0, new ListItem("Please select"));
                    }
                }
            }
            catch(Exception ex)
            { ex.ToString(); }
        }

        public int CheckIsCurrentUser()
        {//Ankita - 09/may/2016- (For Optimization)
            //string getqry = @"select * from MastSalesRep where UserId=" + Settings.Instance.UserID + "";
            //string getqry = @"select SMId from MastSalesRep where UserId=" + Settings.Instance.UserID + "";
           // int SmIDFromSession = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, getqry));
            int SmIDFromSession =Convert.ToInt32(Settings.Instance.SMID);
            if (Convert.ToInt32(Request.QueryString["SMId"]) == SmIDFromSession)
            {
                IsCurrUserHiddenField.Value = "1";
            }
            return isCurrentUser;
        }
        private static int GetSMIDFromUserId(string uid)
        {
            try
            {
                string envObj = @"select SMId from MastSalesRep where UserId=" + Convert.ToInt32(uid) + "";
                DataTable dtenvObj = DbConnectionDAL.GetDataTable(CommandType.Text, envObj);
                if (dtenvObj.Rows.Count > 0)
                {
                    return Convert.ToInt32(dtenvObj.Rows[0]["SMId"]);
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
         {
            //Ankita - 03/may/2016- (For Optimization)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Flag == 1)
                {
                    var areaID = DataBinder.Eval(e.Row.DataItem, "AreaId");
                    var beatID = DataBinder.Eval(e.Row.DataItem, "BeatId");
                    var smIDNew = DataBinder.Eval(e.Row.DataItem, "SMId");
                    var date = DataBinder.Eval(e.Row.DataItem, "StartDate");

                    calendarTextBox.Text = Convert.ToDateTime(date).ToString("dd/MMM/yyyy");
                    DateTime GetDate = (DateTime)DataBinder.Eval(e.Row.DataItem, "Date");
                    GetHolidayData(smIDNew.ToString());

                  //  string areaqre1 = @"select  area.AreaId, area.AreaName  from MastArea area where area.AreaId=" + areaID + "";
                 //   DataTable areatQuery1 = DbConnectionDAL.GetDataTable(CommandType.Text, areaqre1);


                    //string beatqre1 = @"select  beat.AreaId, beat.AreaName from MastArea beat where beat.AreaId=" + beatID + "";
                 //DataTable beatQuery1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatqre1);

                    int smiDUser = (int)smIDNew; // GetSMIDFromUserId(Request.QueryString["UserID"]);
                                 
                    //string areaQuery2 = @"select * from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + smiDUser + ") and areatype='Area' and Active=1 order by AreaName";
                  //  string areaQuery2 = " Select AreaId,AreaName from  [View_Area_SalesPerson_Permission] where PrimCode=" + smiDUser + "";             

                    //Ankita - 06/may/2016- (For Optimization)
                  //  string beatQuery2 = "select * from mastarea where UnderId=" + areaID + " order by AreaName";
                    string beatQuery2 = "select AreaId,AreaName from mastarea where UnderId=" + areaID + " order by AreaName";
                   // DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaQuery2);
                    DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatQuery2);
                    DropDownList ddlareaDP = (e.Row.FindControl("ddlArea") as DropDownList);
                    DropDownList ddlbeatDP = (e.Row.FindControl("ddlBeat") as DropDownList);
                    
                    //ddlareaDP.DataTextField = "AreaName";
                    //ddlareaDP.DataValueField = "AreaId";
                    //ddlareaDP.DataSource = dtArea;
                    //ddlareaDP.DataBind();
                    if (Convert.ToInt32(areaID) > 0)
                    {
                    //    ddlareaDP.Items.Insert(0, new ListItem("--Please select--"));
                       ddlareaDP.SelectedValue = areaID.ToString();
                    }
                    if (Convert.ToInt32(areaID) < 0)
                    {
                        //    ddlareaDP.Items.Insert(0, new ListItem("--Please select--"));
                        ddlareaDP.SelectedValue = "-7";
                    }
                    //else
                    //{
                    //    ddlareaDP.Items.Insert(0, new ListItem("--Please select--"));
                    //    ddlareaDP.SelectedIndex = 0;
                    //}

                    if (Convert.ToInt32(beatID) > 0)
                    {
                        ddlbeatDP.Items.Clear();
                        ddlbeatDP.DataTextField = "AreaName";
                        ddlbeatDP.DataValueField = "AreaId";
                        ddlbeatDP.DataSource = dtBeat;
                        ddlbeatDP.DataBind();
                        ddlbeatDP.Items.Insert(0, new ListItem("--Please select--"));
                        ddlbeatDP.SelectedValue = beatID.ToString();
                    }
                    else if (Convert.ToInt32(areaID) < 0)
                    {
                        ddlbeatDP.Items.Clear();
                        ddlbeatDP.Items.Add(new ListItem("Week Off", "-7"));
                        ddlbeatDP.Items.Insert(0, new ListItem("Please select"));
                        ddlbeatDP.SelectedValue = "-7";
                    }
                    else
                    {
                        ddlbeatDP.Items.Insert(0, new ListItem("--Please select--"));
                        ddlbeatDP.SelectedIndex = 0;
                    }


                    //
                    string getDay = GetDate.ToString("dddd");
                    //

                    //Holiday Check
                    DataRow[] drHoliday = dtHoliday.Select("holiday_date='" + GetDate + "'");
                    if (drHoliday.Count() > 0)
                    {
                        ddlareaDP.Items.Clear();
                        ddlbeatDP.Items.Clear();
                        ddlareaDP.Items.Insert(0, new ListItem("Holiday"));
                        ddlbeatDP.Items.Insert(0, new ListItem("Holiday"));
                        ddlareaDP.ForeColor = System.Drawing.Color.Red;
                        ddlbeatDP.ForeColor = System.Drawing.Color.Red;
                    }

                    //if (getDay == "Sunday")
                    //{
                    //    ddlareaDP.Items.Clear();
                    //    ddlbeatDP.Items.Clear();
                    //    ddlareaDP.Items.Insert(0, new ListItem("Week Off"));
                    //    ddlbeatDP.Items.Insert(0, new ListItem("Week Off"));
                    //    ddlareaDP.Enabled = false;
                    //    ddlbeatDP.Enabled = false;
                    //}
                }
                else
                {
                    if (Flag1 == 1)
                    {
                        var areaID = DataBinder.Eval(e.Row.DataItem, "AreaId");
                        var beatID = DataBinder.Eval(e.Row.DataItem, "BeatId");
                        var smIDNew = DataBinder.Eval(e.Row.DataItem, "SMId");

                        int smiDUser1 = (int)smIDNew;
                        GetHolidayData(smIDNew.ToString());
                      //  string areaqre3 = @"select  area.AreaId, area.AreaName  from MastArea area where area.AreaId=" + areaID + "";
                      //  DataTable areatQuery3 = DbConnectionDAL.GetDataTable(CommandType.Text, areaqre3);

                       // string beatqre3 = @"select  beat.AreaId, beat.AreaName  from MastArea beat where beat.AreaId=" + beatID + "";
                     //   DataTable beatQuery3 = DbConnectionDAL.GetDataTable(CommandType.Text, beatqre3);

                        //string areaQuery4 = @"select * from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + smiDUser1 + ") and areatype='Area' and Active=1 order by AreaName";
                        //string areaQuery4 = " Select AreaId,AreaName from  [View_Area_SalesPerson_Permission] where PrimCode=" + smiDUser1 + "";
                        //Ankita - 06/may/2016- (For Optimization)
                        //string beatQuery4 = "select * from mastarea where UnderId=" + areaID + " order by AreaName";
                        string beatQuery4 = "select AreaId,Areaname from mastarea where UnderId=" + areaID + " order by AreaName";
                       // DataTable dtArea1 = DbConnectionDAL.GetDataTable(CommandType.Text, areaQuery4);
                        DataTable dtBeat1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatQuery4);
                        DropDownList ddlareaDP1 = (e.Row.FindControl("ddlArea") as DropDownList);
                        DropDownList ddlbeatDP1 = (e.Row.FindControl("ddlBeat") as DropDownList);
                        //ddlareaDP1.DataTextField = "AreaName";
                        //ddlareaDP1.DataValueField = "AreaId";
                        //ddlareaDP1.DataSource = dtArea1;
                        //ddlareaDP1.DataBind();
                        if (Convert.ToInt32(areaID) > 0)
                        {
                          //  ddlareaDP1.Items.Insert(0, new ListItem("Please select"));
                            ddlareaDP1.SelectedValue = areaID.ToString();
                        }
                        if (Convert.ToInt32(areaID) < 0)
                        {
                            //    ddlareaDP.Items.Insert(0, new ListItem("--Please select--"));
                            ddlareaDP1.SelectedValue = "-7";
                        }
                        //else
                        //{
                        //    ddlareaDP1.Items.Insert(0, new ListItem("Please select"));
                        //}
                        if (Convert.ToInt32(beatID) > 0)
                        {
                            ddlbeatDP1.Items.Clear();
                            ddlbeatDP1.DataTextField = "AreaName";
                            ddlbeatDP1.DataValueField = "AreaId";
                            ddlbeatDP1.DataSource = dtBeat1;
                            ddlbeatDP1.DataBind();
                            ddlbeatDP1.Items.Insert(0, new ListItem("Please select"));
                            ddlbeatDP1.SelectedValue = beatID.ToString();
                        }
                        else if (Convert.ToInt32(areaID) < 0)
                        {
                            ddlbeatDP1.Items.Clear();
                            ddlbeatDP1.Items.Add(new ListItem("Week Off", "-7"));
                            ddlbeatDP1.Items.Insert(0, new ListItem("Please select"));
                            ddlbeatDP1.SelectedValue = "-7";
                        }
                        else
                        {
                            ddlbeatDP1.Items.Insert(0, new ListItem("Please select"));
                            ddlbeatDP1.SelectedIndex = 0;
                        }
                        DateTime GetDate = (DateTime)DataBinder.Eval(e.Row.DataItem, "Date");

                        //
                        string getDay = GetDate.ToString("dddd");
                        //

                        //Holiday Check
                        DataRow[] drHoliday = dtHoliday.Select("holiday_date='" + GetDate + "'");
                        if (drHoliday.Count() > 0)
                        {
                            ddlareaDP1.Items.Clear();
                            ddlbeatDP1.Items.Clear();
                            ddlareaDP1.Items.Insert(0, new ListItem("Holiday"));
                            ddlbeatDP1.Items.Insert(0, new ListItem("Holiday"));
                            ddlareaDP1.ForeColor = System.Drawing.Color.Red;
                            ddlbeatDP1.ForeColor = System.Drawing.Color.Red;
                        }

                        //if (getDay == "Sunday")
                        //{
                        //    ddlareaDP1.Items.Clear();
                        //    ddlbeatDP1.Items.Clear();
                        //    ddlareaDP1.Items.Insert(0, new ListItem("Week Off"));
                        //    ddlbeatDP1.Items.Insert(0, new ListItem("Week Off"));
                        //    ddlareaDP1.Enabled = false;
                        //    ddlbeatDP1.Enabled = false;
                        //}

                        //Label areaLabel = (e.Row.FindControl("lblArea") as Label);
                        //Label beatLabel = (e.Row.FindControl("lblBeat") as Label);
                        Label dateLabel = (e.Row.FindControl("lblDat1") as Label);
                        dateLabel.Enabled = false;
                        //areaLabel.Visible = false;
                        //beatLabel.Visible = false;

                    }
                    else
                    {

                        //string areaNewQuery = @"select * from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + ") and areatype='Area' and Active=1 order by AreaName";
                       // string areaNewQuery = " Select AreaId,AreaName from  [View_Area_SalesPerson_Permission] where PrimCode=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                      //  DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaNewQuery);

                        DropDownList ddlAreas = (e.Row.FindControl("ddlArea") as DropDownList);
                        DropDownList ddlBeats = (e.Row.FindControl("ddlBeat") as DropDownList);

                        //ddlAreas.DataSource = dtArea;
                        //ddlAreas.DataTextField = "AreaName";
                        //ddlAreas.DataValueField = "AreaId";
                        //ddlAreas.DataBind();
                        //ddlAreas.Items.Insert(0, new ListItem("--Please select--"));
                        ddlBeats.Items.Insert(0, new ListItem("--Please select--"));
                        DateTime GetDate = (DateTime)DataBinder.Eval(e.Row.DataItem, "Date");

                        //
                        string getDay = GetDate.ToString("dddd");
                        //

                        //Holiday Check
                        DataRow[] drHoliday = dtHoliday.Select("holiday_date='" + GetDate + "'");
                        if (drHoliday.Count() > 0)
                        {
                            ddlAreas.Items.Clear();
                            ddlBeats.Items.Clear();
                            ddlAreas.Items.Insert(0, new ListItem("Holiday"));
                            ddlBeats.Items.Insert(0, new ListItem("Holiday"));
                            ddlAreas.ForeColor = System.Drawing.Color.Red;
                            ddlBeats.ForeColor = System.Drawing.Color.Red;
                        }

                        //if (getDay == "Sunday")
                        //{
                        //    ddlAreas.Items.Clear();
                        //    ddlBeats.Items.Clear();
                        //    ddlAreas.Items.Insert(0, new ListItem("Week Off"));
                        //    ddlBeats.Items.Insert(0, new ListItem("Week Off"));
                        //    ddlAreas.Enabled = false;
                        //    ddlBeats.Enabled = false;
                        //}
                        //Ankita - 06/may/2016- (For Optimization)
                      //  string query = @"select * from TransBeatPlan where UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " and PlannedDate='" + Settings.dateformat(GetDate.ToShortDateString()) + "'";
                     //   DataTable dtCity1 = DbConnectionDAL.GetDataTable(CommandType.Text, query);

                     //   var MastQuery = query.ToList();
                     //   if (dtCity1.Rows.Count != 0)
                     //   {
                            //                 ddlCities.SelectedValue = Convert.ToString(dtCity1.Rows[0]["AreaId"]);
                     //   }
                    }
                }

            }
        }

        private void GetHolidayData(string p)
        {
            holidaydt = new DataTable();
            dtHoliday = new DataTable();
            holidaydt.Columns.Clear();
            dtHoliday.Columns.Clear();

            string holidayquery = "SELECT View_Holiday.holiday_date, View_Holiday.Reason as description FROM View_Holiday where View_Holiday.smid in (" + Convert.ToInt32(p) + ") order by holiday_date";  
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

        protected void fill_Click(object sender, EventArgs e)
        {
            chkRefreshStatus = 0;
            GetHolidayData(DdlSalesPerson.SelectedValue);
            if (calendarTextBox.Text != "")
            {
                Hdate.Value = calendarTextBox.Text;
                ViewState["Hdate"] = Hdate.Value;
                //Ankita - 06/may/2016- (For Optimization)
                string chkBeatPlanQry = @"select count(*) from TransBeatPlan beat where beat.SMId =" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and beat.StartDate='" + Settings.dateformat(Hdate.Value) + "' and beat.AppStatus<>'Reject'";
              //  DataTable dtBeatPlan = DbConnectionDAL.GetDataTable(CommandType.Text, chkBeatPlanQry);
                // int count = dtBeatPlan.Rows.Count;
                int count =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, chkBeatPlanQry));
                ViewState["BeatDataCount"] = count;
                if (count > 0)
                {
                    Flag1 = 1;

                    //                    string str = @"select t.PlannedDate as Date,t.CityId,t.AreaId,t.BeatId,t.BeatPlanId,t.AppStatus,t.AppRemark,t.SMId,a.AreaName as CityName,b.AreaName as AreaName,c.AreaName as BeatName from TransBeatPlan t
                    //                    left join MastArea a on t.CityId=a.AreaId
                    //                    left join MastArea b on t.AreaId=b.AreaId
                    //                    left join MastArea c on t.BeatId=c.AreaId where t.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and t.StartDate='" + Settings.dateformat(Hdate.Value) + "' and t.AppStatus<>'Reject'";
                    //                    DataTable valueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                   
                    string str = @"select t.PlannedDate as Date,t.CityId,t.AreaId,t.BeatId,t.BeatPlanId,t.AppStatus,t.AppRemark,t.SMId from TransBeatPlan t
                      where t.SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and t.StartDate='" + Settings.dateformat(Hdate.Value) + "' and t.AppStatus<>'Reject'";
                    DataTable valueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (valueDt.Rows.Count > 0)
                    {
                        appStatusHiddenField.Value = valueDt.Rows[0]["AppStatus"].ToString();
                        approveStatusRadioButtonList.SelectedValue = valueDt.Rows[0]["AppStatus"].ToString();
                        ViewState["appstatus"] = valueDt.Rows[0]["AppStatus"].ToString();
                        RemarkArea.Value = valueDt.Rows[0]["AppRemark"].ToString();// queryNew.FirstOrDefault().AppRemark;
                        GridView1.DataSource = valueDt;
                        GridView1.DataBind();
                        datepresentHiddenField.Value = "1";

                        string areaQuery2 = " Select AreaId,AreaName from  [View_Area_SalesPerson_Permission] where PrimCode=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " order by AreaName";

                        DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaQuery2);
                        DataRow dr = dtArea.NewRow();
                        dr[0] = "-7";
                        dr[1] = "Week Off";
                        dtArea.Rows.Add(dr);
                        DropDownList ddlareaDP;
                        for (int i = 0; i < 7; i++)
                        {
                            ddlareaDP = (GridView1.Rows[i].FindControl("ddlArea") as DropDownList);
                            if (ddlareaDP.SelectedValue != "Holiday")
                            {
                                ddlareaDP.DataTextField = "AreaName";
                                ddlareaDP.DataValueField = "AreaId";
                                ddlareaDP.DataSource = dtArea;

                                ddlareaDP.DataBind();                               
                                ddlareaDP.Items.Insert(0, new ListItem("Please select"));
                            }
                        }

                        if (appStatusHiddenField.Value == "Approve")
                        {
                            Btnsave.Visible = true;
                            Btnsave.Enabled = false;
                            Btnsave.CssClass = "btn btn-primary";
                        }
                        else
                        {
                            Btnsave.Visible = true;
                            Btnsave.Enabled = true;
                        }
                        BtnCancel.Visible = true;
                        cancelBtnHiddenField.Value = "1";
                        Flag1 = 0;
                    }
                }
                else
                {
                    if (Convert.ToDateTime(Hdate.Value) < Convert.ToDateTime(Settings.GetUTCTime().ToShortDateString()))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Back Date Entry Not Allowed');", true);
                        ClearControls();
                    }
                    else
                    {
                        DateTime h = Convert.ToDateTime(Hdate.Value);
                        if (h.DayOfWeek.ToString() != "Monday")
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage2();", true);
                            GridView1.DataSource = null;
                            GridView1.DataBind();
                            BtnCancel.Visible = false;
                            return;
                        }

                        DateTime Dt11 = Settings.GetUTCTime();
                        if (Convert.ToDateTime(Hdate.Value) > Convert.ToDateTime(Settings.GetUTCTime().ToShortDateString()) || Convert.ToDateTime(Hdate.Value) == Convert.ToDateTime(Settings.GetUTCTime().ToShortDateString()))
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                            new DataColumn("Date", typeof(DateTime)),new DataColumn("AreaName", typeof(string)),new DataColumn("BeatName", typeof(string))
                             });
                            if (Hdate.Value != "")
                            {
                                DateTime Dt = Convert.ToDateTime(Hdate.Value);
                                dt.Rows.Add(0, Dt);
                                for (int i = 1; i < 7; i++)
                                {
                                    dt.Rows.Add(i, Dt.AddDays(i));
                                }

                                GridView1.DataSource = dt;
                                GridView1.DataBind();
                                Btnsave.Visible = true;
                                BtnCancel.Visible = true;
                                cancelBtnHiddenField.Value = "1";
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage();", true);
                            GridView1.DataSource = null;
                            GridView1.DataBind();
                            Hdate.Value = "";
                            Btnsave.Visible = false;
                            BtnCancel.Visible = false;
                        }
                        //    }
                        //}


                        // Fill Dropdowns for grid for AREA
                        //Ankita - 03/may/2016- (For Optimization)
                        string areaQuery2 = " Select AreaId,AreaName from  [View_Area_SalesPerson_Permission] where PrimCode=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " order by AreaName";

                        DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaQuery2);
                        DropDownList ddlareaDP;
                        for (int i = 0; i < 7; i++)
                        {
                            ddlareaDP = (GridView1.Rows[i].FindControl("ddlArea") as DropDownList);
                            if (ddlareaDP.SelectedValue != "Holiday")
                            {
                                ddlareaDP.DataTextField = "AreaName";
                                ddlareaDP.DataValueField = "AreaId";
                                ddlareaDP.DataSource = dtArea;

                                ddlareaDP.DataBind();
                                ddlareaDP.Items.Add(new ListItem("Week Off", "-7"));
                                ddlareaDP.Items.Insert(0, new ListItem("Please select"));
                            }
                        }
                    }
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage1", "errormessage1();", true);
            }



        }

        protected void Btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkRefreshStatus == 1) {ClearControls();return; }
                if (Btnsave.Text == "Update")
                {
                    UpdateBeatPlan();
                }
                else
                {
                    if (Request.QueryString["SMId"] != null && Request.QueryString["DocId"] != null)
                    {
                        // Validation Check
                        int counter2 = CheckBeatEntry();
                        //
                        if (gridRowCount >= 1)
                        {
                            if (!(counter2 >= 1))
                            {
                                foreach (GridViewRow control in GridView1.Rows)
                                {
                                    if (control.RowType == DataControlRowType.DataRow)
                                    {
                                        string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                                        DropDownList ddlareaS = control.FindControl("ddlArea") as DropDownList;
                                        DropDownList ddlbeatS = control.FindControl("ddlBeat") as DropDownList;

                                        //string beatPlanQry = @"select * from TransBeatPlan where SMId =" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId ='" + Request.QueryString["DocId"] + "' and PlannedDate ='" + Convert.ToDateTime(Date) + "'";
                                        //DataTable valueDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatPlanQry);
                                        int CityId = 0, AreaId = 0, BeatId = 0;

                                        if (ddlareaS.SelectedIndex > 0) {AreaId = Convert.ToInt32(ddlareaS.SelectedValue);}
                                        if (ddlbeatS.SelectedIndex > 0) {BeatId = Convert.ToInt32(ddlbeatS.SelectedValue);}

                                        String upDateBeatQry = @"update TransBeatPlan set CityId=" + CityId + ",AreaId=" + AreaId + ",BeatId=" + BeatId + " where SMId =" + Convert.ToInt32(Request.QueryString["SMId"]) + " and DocId ='" + Request.QueryString["DocId"] + "' and PlannedDate ='" + Convert.ToDateTime(Date) + "'";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, upDateBeatQry);
                                    }
                                }
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Beat Plan Updated Successfully');", true);
                                chkRefreshStatus = 1;
                                if (roleType == "AreaIncharge")
                                {
                                    SendNotification(Request.QueryString["DocId"]);
                                }
                                GridView1.DataSource = null;
                                GridView1.DataBind();
                                calendarTextBox.Text = string.Empty;
                                Btnsave.Visible = false;
                                BtnCancel.Visible = false;
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select beat');", true);
                                return;
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select atleast one beat');", true);
                            return;
                        }
                        //End Else Counter By QueryString
                    }
                    
                    else if (Convert.ToInt32(ViewState["BeatDataCount"]) > 0)
                    {
                        // Validation Check
                        int counter1 = CheckBeatEntry();
                        //
                        if (gridRowCount >= 1)
                        {
                            if (!(counter1 >= 1))
                            {
                                foreach (GridViewRow control in GridView1.Rows)
                                {
                                    if (control.RowType == DataControlRowType.DataRow)
                                    {
                                        string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                                        //                     DropDownList ddlcityS = control.FindControl("ddlCity") as DropDownList;
                                        DropDownList ddlareaS = control.FindControl("ddlArea") as DropDownList;
                                        DropDownList ddlbeatS = control.FindControl("ddlBeat") as DropDownList;
                                        //Ankita - 06/may/2016- (For Optimization)
                                        string beatPlQry = @"select DocId from TransBeatPlan where SMId =" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and StartDate ='" + Settings.dateformat(ViewState["Hdate"].ToString()) + "' and PlannedDate ='" + Settings.dateformat(Date) + "'";
                                        //string beatPlQry = @"select * from TransBeatPlan where SMId =" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and StartDate ='" + Settings.dateformat(ViewState["Hdate"].ToString()) + "' and PlannedDate ='" + Settings.dateformat(Date) + "'";
                                     //   DataTable valueBeatDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatPlQry);
                                        ViewState["BeatDocid"] = DbConnectionDAL.GetScalarValue(CommandType.Text,beatPlQry);
                                        int CityId1 = 0, AreaId1 = 0, BeatId1 = 0;
                                      //  ViewState["BeatDocid"] = valueBeatDt1.Rows[0]["DocId"];

                                        if (ddlareaS.SelectedIndex > 0) {AreaId1 = Convert.ToInt32(ddlareaS.SelectedValue);}
                                        if (ddlbeatS.SelectedIndex > 0) {BeatId1 = Convert.ToInt32(ddlbeatS.SelectedValue);}

                                        String upDateBeatQry1 = @"update TransBeatPlan set CityId=" + CityId1 + ",AreaId=" + AreaId1 + ",BeatId=" + BeatId1 + " where SMId =" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and StartDate ='" + Settings.dateformat(ViewState["Hdate"].ToString()) + "' and PlannedDate ='" + Settings.dateformat(Date) + "'";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, upDateBeatQry1);

                                    }
                                }
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Beat Plan Updated Successfully');", true);
                                chkRefreshStatus = 1;
                                if (roleType == "AreaIncharge")
                                {
                                    SendNotification(Convert.ToString(ViewState["BeatDocid"]));
                                }
                                GridView1.DataSource = null;
                                GridView1.DataBind();
                                calendarTextBox.Text = string.Empty;
                                Btnsave.Visible = false;
                                BtnCancel.Visible = false;
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select atleast one beat');", true);
                                return;
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select atleast one beat');", true);
                            return;
                        }
                        //End Else Counter Check
                    }

                    else
                    {
                        //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Insert into testtest (smid,rem) values (" + Convert.ToInt32(Settings.Instance.SMID) + ",'CHK-7')");

                        string BeatP = string.Empty;

                        DateTime newDate = Settings.GetUTCTime();



                        ////Addedd
                        //string createText = "smid = '" + Settings.Instance.SMID + "' BEFORE DOC ID";
                        //using (System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/BeatTest.txt"), true))
                        //{
                        //    TextFileCID.WriteLine(createText);
                        //    TextFileCID.Close();
                        //}
                        ////end
                        

                        //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Insert into testtest (smid,rem) values (" + Convert.ToInt32(Settings.Instance.SMID) + ",'Before docId')");

                        string docId = btAll.GetDociId(newDate);

                        //DbConnectionDAL.ExecuteNonQuery(CommandType.Text,"Insert into testtest (smid,rem) values (" + Convert.ToInt32(Settings.Instance.SMID) + ",'" + docId + "')");

                        //string createText = "smid = '" + Settings.Instance.SMID + "' and DocId =" + docId + " and SeniorSmid=" + senSMid + " and SeniorName=" + seniorName + "";



                        // Validation Check
                        int counter = CheckBeatEntry();
                        //

                        if (gridRowCount >= 1)
                        {
                            if (!(counter >= 1))
                            {
                                foreach (GridViewRow control in GridView1.Rows)
                                {
                                    if (control.RowType == DataControlRowType.DataRow)
                                    {
                                        string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                                        //                DropDownList ddlcityS = control.FindControl("ddlCity") as DropDownList;
                                        DropDownList ddlareaS = control.FindControl("ddlArea") as DropDownList;
                                        DropDownList ddlbeatS = control.FindControl("ddlBeat") as DropDownList;

                                        int cityId = 0;
                                        int areaId = 0;
                                        int beatId = 0;

                                        if (ddlareaS.SelectedIndex > 0) {areaId = Convert.ToInt32(ddlareaS.SelectedValue);}
                                        if (ddlbeatS.SelectedIndex > 0) {beatId = Convert.ToInt32(ddlbeatS.SelectedValue);}

                                        btAll.Insert(docId, Convert.ToInt32(Settings.Instance.UserID), Convert.ToDateTime(Date), cityId, areaId, beatId, "Pending", 0, String.Empty, Convert.ToDateTime(ViewState["Hdate"]), DdlSalesPerson.SelectedValue);

                                    }
                                }
                                btAll.SetDociId(docId);
                                int senSMid = 0; string seniorName = string.Empty;
                                string query12 = @"select BeatPlanApproval from MastEnviro";
                                DataTable getBeatAppStatdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, query12);

                                if (Convert.ToBoolean(getBeatAppStatdt12.Rows[0]["BeatPlanApproval"]) == true)
                                {
                                    string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                                    DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);
                                    string salesRepqueryNew1 = "";

                                    //Ankita - 06/may/2016- (For Optimization)
                                    //string getSeniorSMId1 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                                    string getSeniorSMId1 = @"select UnderId,UserId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                                    DataTable salesRepqryForSManNewTP1dt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId1);
                                    //int senSMid = 0; string seniorName = string.Empty;
                                    if (salesRepqryForSManNewTP1dt.Rows.Count > 0)
                                    {

                                        string getSeniorSMId12 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewTP1dt.Rows[0]["UnderId"]) + "";
                                        DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                                        if (salesRepqryForSManNewdt12.Rows.Count > 0)
                                        {
                                            senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
                                            seniorName = salesRepqryForSManNewdt12.Rows[0]["SMName"].ToString();
                                        }
                                    }
                                    //

                                    if (salesRepqueryNewdt.Rows.Count > 0)
                                    {
                                        salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                                    }
                                    DataTable salesRepqueryNewdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew1);
                                    string msgUrl = "BeatPlanEntry.aspx?SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "&DocId=" + docId;
                                    if (roleType == "AreaIncharge" || roleType == "CityHead" || roleType == "DistrictHead")
                                    {
                                        if (GetRoleSP() == "AreaIncharge" && (roleType == "CityHead" || roleType == "DistrictHead"))
                                        {
                                            string updateBeatStatusQry = @"update TransBeatPlan set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ",AppRemark='Approved By Senior' where DocId='" + docId + "' ";
                                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateBeatStatusQry);
                                            //string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smId, int toSMId
                                            btAll.InsertTransNotification("BEATAPPROVED", Convert.ToInt32(salesRepqryForSManNewTP1dt.Rows[0]["UserId"]), Settings.GetUTCTime(), msgUrl, "Beat Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID),senSMid, Convert.ToInt32(DdlSalesPerson.SelectedValue));
                                        }
                                        else{
                                            btAll.InsertTransNotification("BEATPLANREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), Settings.GetUTCTime(), msgUrl, "Beat Plan By - " + salesRepqryForSManNewTP1dt.Rows[0]["SMName"] + " " + " " + " StartDate -" + calendarTextBox.Text + "", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(DdlSalesPerson.SelectedValue), senSMid);
                                        }
                                    }
                                    else
                                    {
                                        //string updateBeatStatusQry = @"update TransBeatPlan set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + smIDSen + ",AppRemark='Approved By Senior' where DocId='" + docId + "' ";
                                        //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateBeatStatusQry);
                                    }
                                }

                                //Addedd
                                //string createText = "smid = '" + Settings.Instance.SMID + "' and DocId =" + docId + " and SeniorSmid=" + senSMid + " and SeniorName="+seniorName+"";
                                //using (System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/BeatTest.txt"), true))
                                //{
                                //    TextFileCID.WriteLine(createText);
                                //    TextFileCID.Close();
                                //}
                                //end

                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully <br/> DocNo-" + docId + "');", true);
                                chkRefreshStatus = 1;
                                ClearControls();
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select atleast one beat');", true);
                                return;
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select atleast one beat');", true);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private int CheckBeatEntry()
        {
            int counter = 0;
            foreach (GridViewRow control in GridView1.Rows)
            {
                if (control.RowType == DataControlRowType.DataRow)
                {
                    string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                    DropDownList ddlareaS = control.FindControl("ddlArea") as DropDownList;
                    DropDownList ddlbeatS = control.FindControl("ddlBeat") as DropDownList;

                    int cityId = 0;
                    int areaId = 0;
                    int beatId = 0;

                    if (ddlareaS.SelectedIndex > 0)
                    {
                        areaId = Convert.ToInt32(ddlareaS.SelectedValue);

                        if (ddlbeatS.SelectedIndex > 0)
                        {
                            beatId = Convert.ToInt32(ddlbeatS.SelectedValue);
                            if(beatId != -7)
                            gridRowCount = gridRowCount + 1;
                        }
                        else
                        {
                            string beattext = ddlareaS.SelectedItem.Text;
                            if (beattext != "Leave is Approved for This Day" || beattext != "Week Off" || beattext != "Holiday")
                            {
                                counter = counter + 1;
                            }
                        }
                    }
                    else
                    {
                        //string beattext = ddlareaS.SelectedItem.Text;
                        //if (beattext == "Leave is Approved for This Day" || beattext == "Week Off" || beattext == "Holiday")
                        //{
                           
                        //}
                        //else
                        //{
                        //    counter = counter + 1;
                        //}
                    }
                }
            }
            return counter;
        }

        private void UpdateBeatPlan()
        {
            try
            {
                // Validation Check
                int counterNew = CheckBeatEntry();
                //
                if (gridRowCount >= 1)
                {
                    if (!(counterNew >= 1))
                    {
                        foreach (GridViewRow control in GridView1.Rows)
                        {
                            if (control.RowType == DataControlRowType.DataRow)
                            {
                                string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                                //                     DropDownList ddlcityS = control.FindControl("ddlCity") as DropDownList;
                                DropDownList ddlareaS = control.FindControl("ddlArea") as DropDownList;
                                DropDownList ddlbeatS = control.FindControl("ddlBeat") as DropDownList;
                                //Ankita - 06/may/2016- (For Optimization)
                             //   string beatPlQry = @"select * from TransBeatPlan where DocId ='" + ViewState["BeatDocId"] + "'";
                            //    DataTable valueBeatDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatPlQry);
                                int CityId1 = 0, AreaId1 = 0, BeatId1 = 0;

                                if (ddlareaS.SelectedIndex > 0)
                                {
                                    AreaId1 = Convert.ToInt32(ddlareaS.SelectedValue);
                                }
                                if (ddlbeatS.SelectedIndex > 0)
                                {
                                    BeatId1 = Convert.ToInt32(ddlbeatS.SelectedValue);
                                }
                                String upDateBeatQry1 = @"update TransBeatPlan set CityId=" + CityId1 + ",AreaId=" + AreaId1 + ",BeatId=" + BeatId1 + " where SMId =" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + " and DocId ='" + ViewState["BeatDocId"] + "' and PlannedDate ='" + Settings.dateformat(Date) + "'";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, upDateBeatQry1);

                            }
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Beat Plan Updated Successfully');", true);
                        if (roleType == "AreaIncharge")
                        {
                            SendNotification(Convert.ToString(ViewState["BeatDocId"]));
                        }
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                        calendarTextBox.Text = string.Empty;
                        Btnsave.Visible = false;
                        BtnCancel.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select all area and beat');", true);
                        return;
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please select atleast one beat');", true);
                    return;
                }
                //End Else
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void SendNotification(string docID)
        {
            string query12 = @"select BeatPlanApproval from MastEnviro";
            DataTable getBeatAppStatdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, query12);

            if (Convert.ToBoolean(getBeatAppStatdt12.Rows[0]["BeatPlanApproval"]) == true)
            {
                string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);
                string salesRepqueryNew1 = "";
                if (salesRepqueryNewdt.Rows.Count > 0)
                {
                    salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                }

                  //Ankita - 06/may/2016- (For Optimization)
               // string getSeniorSMId1 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                string getSeniorSMId1 = @"select UnderId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                DataTable salesRepqryForSManNewTP1dt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId1);
                int senSMid = 0;
                if (salesRepqryForSManNewTP1dt.Rows.Count > 0)
                {

                    string getSeniorSMId12 = @"select SMId from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewTP1dt.Rows[0]["UnderId"]) + "";
                    DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                    if (salesRepqryForSManNewdt12.Rows.Count > 0)
                    {
                        senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
                    }
                }
                //
               
                    DataTable salesRepqueryNewdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew1);
                    string msgUrl = "BeatPlanEntry.aspx?SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "&DocId=" + docID;
                    if (GetRoleSP() == "AreaIncharge" && (roleType == "CityHead" || roleType == "DistrictHead"))
                    { }
                    else
                    {
                    btAll.InsertTransNotification("BEATPLANREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), Settings.GetUTCTime(), msgUrl, "Beat Plan By - " + salesRepqryForSManNewTP1dt.Rows[0]["SMName"] + " " + " " + " StartDate -" + calendarTextBox.Text + "", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(DdlSalesPerson.SelectedValue), senSMid);
                }
            }

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (RemarkArea.Value != string.Empty)
            {
                string BeatDocid = Request.QueryString["DocId"]; //docIDHiddenField.Value;
                int UserId = 0;
                foreach (GridViewRow control in GridView1.Rows)
                {
                    if (control.RowType == DataControlRowType.DataRow)
                    {
                        string Date = (control.Cells[0].FindControl("lblDat1") as Label).Text;
                        DropDownList ddl1 = control.FindControl("ddlBeat") as DropDownList;
                        //         TBP = context.TransBeatPlans.FirstOrDefault(u => u.DocId == BeatDocid && u.PlannedDate == Convert.ToDateTime(Date));
                        //Ankita - 06/may/2016- (For Optimization)
                     //   string TBQry = @"select * from TransBeatPlan where DocId ='" + BeatDocid + "' and PlannedDate ='" + Settings.dateformat(Date) + "'";
                       // DataTable getBeatAppStdt = DbConnectionDAL.GetDataTable(CommandType.Text, TBQry);
                        string TBQry = @"select count(*) from TransBeatPlan where DocId ='" + BeatDocid + "' and PlannedDate ='" + Settings.dateformat(Date) + "'";
                        int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, TBQry));

                        //
                        DropDownList ddlareaS = control.FindControl("ddlArea") as DropDownList;
                        DropDownList ddlbeatS = control.FindControl("ddlBeat") as DropDownList;

                        int areaId = 0;
                        int beatId = 0;
                        if (ddlareaS.SelectedIndex > 0)
                        {
                            areaId = Convert.ToInt32(ddlareaS.SelectedValue);
                        }

                        if (ddlbeatS.SelectedIndex > 0)
                        {
                            beatId = Convert.ToInt32(ddlbeatS.SelectedValue);
                        }
                        //
                        if (cnt != 0)
                        {

                            string beatPUpdQry = @"update TransBeatPlan set AppStatus='" + approveStatusRadioButtonList.SelectedValue + "',AreaId=" + areaId + ", BeatId=" + beatId + ",AppRemark='" + RemarkArea.Value + "',AppBySMId=" + Settings.Instance.SMID + ",AppBy=" + Convert.ToInt32(Settings.Instance.UserID) + " where DocId ='" + BeatDocid + "' and PlannedDate ='" + Settings.dateformat(Date) + "'";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, beatPUpdQry);
                        }
                    }
                }

                //Commented 07N
                //string getSeniorSMId1 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                //DataTable salesRepqryForSManNewTP1dt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId1);
                //int senSMid = 0; string senSMName = "";
                //if (salesRepqryForSManNewTP1dt.Rows.Count > 0)
                //{
                //    UserId = Convert.ToInt32(salesRepqryForSManNewTP1dt.Rows[0]["UserId"].ToString());
                //    string getSeniorSMId12 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewTP1dt.Rows[0]["UnderId"]) + "";
                //    DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                //    if (salesRepqryForSManNewdt12.Rows.Count > 0)
                //    {
                //        senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
                //        senSMName = salesRepqryForSManNewdt12.Rows[0]["SMName"].ToString();
                //    }
                //}
                //
                //Ankita - 06/may/2016- (For Optimization)
               // string updateBeat1 = @"select * from TransBeatPlan where DocId ='" + BeatDocid + "'";
                string updateBeat1 = @"select AppStatus,SMId,UserId from TransBeatPlan where DocId ='" + BeatDocid + "'";
                DataTable updateBeatdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, updateBeat1);
                string AppStatus = updateBeatdt1.Rows[0]["AppStatus"].ToString();

                //

               // string senSMNameQry = @"select SMName from MastSalesRep where SMId=" + Convert.ToInt32(smIDSen) + "";
                //Ankita - 06/may/2016- (For Optimization)
                //string senSMNameQry = @"select SMName from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                //string senSMName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQry));
                string senSMName = Settings.Instance.SmLogInName;
                //

                int salesRepID = Convert.ToInt32(updateBeatdt1.Rows[0]["SMId"]);
                UserId = Convert.ToInt32(updateBeatdt1.Rows[0]["UserId"]);
                string pro_id = string.Empty;
                string displayTitle = string.Empty;
                if (AppStatus == "Approve")
                {
                    pro_id = "BEATAPPROVED";
                    displayTitle = "Beat Approved By - " + senSMName + "  ";
                }
                else
                {
                    pro_id = "BEATREJECTED";
                    displayTitle = "Beat Rejected By - " + senSMName + "  ";
                }



                DateTime newDate1 = Settings.GetUTCTime();
                //string msgUrl1 = "BeatPlanEntry.aspx?SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "&DocId=" + BeatDocid;
                string msgUrl1 = "BeatPlanEntry.aspx?SMId=" + salesRepID + "&DocId=" + BeatDocid;
                string Role = "SELECT mr.Roletype FROM MastSalesRep msr INNER JOIN MastRole mr ON msr.RoleId=mr.RoleId where msr.Smid=" + salesRepID + "";
                //Ankita - 06/may/2016- (For Optimization)
                //DataTable dtRole = new DataTable();
                //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                string RoleTy = DbConnectionDAL.GetScalarValue(CommandType.Text, Role).ToString();
                if (RoleTy == "AreaIncharge" && (RoleTy == "CityHead" || RoleTy == "DistrictHead"))
                { }
                else
                {
                    btAll.InsertTransNotification(pro_id, Convert.ToInt32(UserId), newDate1, msgUrl1, displayTitle, 0, Convert.ToInt32(Settings.Instance.UserID),  Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(salesRepID));
                }

                //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                // "alert('Record Updated Successfully'); window.location='" + Request.ApplicationPath + "BeatPlanApproval.aspx';", true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                        "alert('Record Updated Successfully'); window.location='" + Request.ApplicationPath + "BeatPlanApproval.aspx?SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + "';", true);

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessageNew('Please Enter Remark');", true);
                return;
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            chkRefreshStatus = 1;
            if (pageName == "BEATAPPROVAL")
            {
                Response.Redirect("~/BeatPlanApproval.aspx");
            }
            else if (pageName1 == "VIEWMSG")
            {
                Response.Redirect("~/ViewAllMessages.aspx");
            }
            else
            {
                Response.Redirect("~/BeatPlanEntry.aspx", true);
            }
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Btnsave.Visible = true;
            BtnCancel.Visible = true;
            DropDownList ddl = sender as DropDownList;

            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;
            DropDownList ddlarea = (row.FindControl("ddlArea") as DropDownList);
            if (Convert.ToInt32(ViewState["BeatDataCount"]) > 0)
            {
                appStatusHiddenField.Value = (ViewState["appstatus"]).ToString();
                datepresentHiddenField.Value = "1";
            }
            if (ddl.SelectedIndex > 0)
            {
                int areaID = Convert.ToInt32(ddl.SelectedItem.Value);
                //Ankita - 06/may/2016- (For Optimization)
               // string areaQuery = "select * from mastarea where areaid in (select distinct underid from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + smID + "))and Active=1) and areatype='Area' and Active=1  and UnderId=" + areaID + " order by AreaName";
                string areaQuery = "select AreaId,AreaName from mastarea where areaid in (select distinct underid from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + smID + "))and Active=1) and areatype='Area' and Active=1  and UnderId=" + areaID + " order by AreaName";
                //        DataTable dtarea = DataAccessLayer.DAL.getFromDataTable(areaQuery);
                DataTable dtarea = DbConnectionDAL.GetDataTable(CommandType.Text, areaQuery);
                if (dtarea.Rows.Count > 0)
                {
                    ddlarea.DataSource = dtarea;
                    ddlarea.DataTextField = "AreaName";
                    ddlarea.DataValueField = "AreaId";
                    ddlarea.DataBind();
                }
                ddlarea.Items.Insert(0, new ListItem("Please select Area"));
            }
            else
            {
                ddlarea.Items.Clear();
                ddlarea.Items.Insert(0, new ListItem("Please select Area"));
            }
            //Ankita - 06/may/2016- (For Optimization)
          //  if (Convert.ToInt32(ViewState["BeatDataCount"]) > 0)
          //  {

          //  }

        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Btnsave.Visible = true;
            BtnCancel.Visible = true;
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            DropDownList ddlbeat = (row.FindControl("ddlBeat") as DropDownList);
            ddlbeat.Items.Clear();
            if (Convert.ToInt32(ViewState["BeatDataCount"]) > 0)
            {
                appStatusHiddenField.Value = (ViewState["appstatus"]).ToString();
                datepresentHiddenField.Value = "1";
            }
            if (ddl.SelectedIndex > 0)
            {
                int areaID = Convert.ToInt32(ddl.SelectedItem.Value);
                int idx = row.RowIndex;
                //string areaQuery = "select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + ")) and areatype='Beat' and UnderId=" + areaID + " order by AreaName";

                //string areaQueryNEw = "select * from mastarea where UnderId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + ") and areatype='beat' and Active=1 and UnderId=" + areaID + " order by AreaName";

                string areaQuery = "select AreaId, AreaName from mastarea where UnderId=" + areaID + " and areatype='beat' and Active=1 order by AreaName";

                DataTable dtbeat = DbConnectionDAL.GetDataTable(CommandType.Text, areaQuery);
                //             DataTable dtbeat = DataAccessLayer.DAL.getFromDataTable(areaQuery);
                if (dtbeat.Rows.Count > 0)
                {
                    ddlbeat.DataSource = dtbeat;
                    ddlbeat.DataTextField = "AreaName";
                    ddlbeat.DataValueField = "AreaId";
                    ddlbeat.DataBind();
                    ddlbeat.Items.Insert(0, new ListItem("Please select Beat"));
                }
                else
                {
                    ddlbeat.Items.Clear();
                    if (areaID == -7)
                        ddlbeat.Items.Add(new ListItem("Week Off", "-7"));
                    ddlbeat.Items.Insert(0, new ListItem("Please select Beat"));
                }

            }
            else
            {
                ddlbeat.Items.Clear();
                ddlbeat.Items.Insert(0, new ListItem("Please select Beat"));
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            rpt.DataSource = null;
            rpt.DataBind();

            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End

            //txtfmDate.Text = string.Empty;
            //txttodate.Text = string.Empty;
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string smiD = "";
            DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
            string smIDStr = "", qrychk = "";
            string smIDStr1 = "";
            if (dtSMId.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSMId.Rows)
                {
                    smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            }
            //Added as per UAT Date-08-12-2015 
            if (DropDownList1.SelectedIndex != 0)
            {
                smiD = DropDownList1.SelectedValue;
                qrychk = "and TransBeatPlan.SMId=" + smiD + "";
            }
            else
            {
                qrychk = "and TransBeatPlan.SMId in (" + smIDStr1 + ")";
            }
            //End


     //       if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty && DropDownList1.SelectedIndex != 0)
     //       {
                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
                {
                  //  smiD = DropDownList1.SelectedValue;
                    string beatplanquery = @"select distinct DocId,StartDate,TransBeatPlan.SMId,TransBeatPlan.AppStatus,msr.SMName from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.StartDate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' "+qrychk+" order by TransBeatPlan.StartDate desc";
                    DataTable beatqrydt1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatplanquery);
                    if (beatqrydt1.Rows.Count > 0)
                    {
                        rpt.DataSource = beatqrydt1;
                        rpt.DataBind();
                    }
                    else
                    {
                        rpt.DataSource = beatqrydt1;
                        rpt.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
       //     }
            //else if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty)
            //{
            //    if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            //    {
            //        string btquery = @"select distinct DocId,StartDate,TransBeatPlan.SMId,TransBeatPlan.AppStatus,msr.SMName from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.StartDate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' and TransBeatPlan.SMId in (" + smIDStr1 + ") order by TransBeatPlan.StartDate desc";
            //        DataTable beatqrydt2 = DbConnectionDAL.GetDataTable(CommandType.Text, btquery);
            //        if (beatqrydt2.Rows.Count > 0)
            //        {
            //            rpt.DataSource = beatqrydt2;
            //            rpt.DataBind();
            //        }
            //        else
            //        {
            //            rpt.DataSource = beatqrydt2;
            //            rpt.DataBind();
            //        }
            //    }
            //    else
            //    {
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
            //        rpt.DataSource = null;
            //        rpt.DataBind();
            //    }
            //}
            //else if (txtfmDate.Text != string.Empty)
            //{
            //    string tourqueryNew = @"select distinct DocId,StartDate,TransBeatPlan.SMId,TransBeatPlan.AppStatus,msr.SMName from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.StartDate >= '" + Settings.dateformat(txtfmDate.Text) + "' and TransBeatPlan.SMId in (" + smIDStr1 + ") order by TransBeatPlan.StartDate desc";
            //    DataTable tourqrydt3 = DbConnectionDAL.GetDataTable(CommandType.Text, tourqueryNew);
            //    if (tourqrydt3.Rows.Count > 0)
            //    {
            //        rpt.DataSource = tourqrydt3;
            //        rpt.DataBind();
            //    }
            //    else
            //    {
            //        rpt.DataSource = tourqrydt3;
            //        rpt.DataBind();
            //    }
            //}
            //else
            //{
            //    if (DropDownList1.SelectedIndex > 0)
            //    {
            //        smiD = DropDownList1.SelectedValue;
            //        string notQry = @"select distinct DocId,StartDate,TransBeatPlan.SMId,TransBeatPlan.AppStatus,msr.SMName from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.SMId=" + smiD + " order by TransBeatPlan.StartDate desc";
            //        DataTable dtNot = DbConnectionDAL.GetDataTable(CommandType.Text, notQry);
            //        rptmain.Style.Add("display", "block");
            //        rpt.DataSource = dtNot;
            //        rpt.DataBind();
            //    }
            //    else
            //    {
            //        string notQry1 = @"select distinct DocId,StartDate,TransBeatPlan.SMId,TransBeatPlan.AppStatus,msr.SMName from TransBeatPlan left join MastSalesRep msr on msr.SMId=TransBeatPlan.SMId where TransBeatPlan.SMId in (" + smIDStr1 + ") order by TransBeatPlan.StartDate desc";
            //        DataTable dtNot1 = DbConnectionDAL.GetDataTable(CommandType.Text, notQry1);
            //        rptmain.Style.Add("display", "block");
            //        rpt.DataSource = dtNot1;
            //        rpt.DataBind();
            //    }
            //}
        }

        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            //txtfmDate.Text = string.Empty;
            //txttodate.Text = string.Empty;

            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End

            DropDownList1.SelectedIndex = 0;
            rpt.DataSource = null;
            rpt.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // calendarTextBox.Text = string.Empty;
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                { //Ankita - 06/may/2016- (For Optimization)
                   // string beatStatus = @"select * from TransBeatPlan where DocId='" + Convert.ToString(ViewState["BeatDocId"]) + "'";
                    string beatStatus = @"select AppStatus from TransBeatPlan where DocId='" + Convert.ToString(ViewState["BeatDocId"]) + "'";
                    DataTable deleteBeatdt = DbConnectionDAL.GetDataTable(CommandType.Text, beatStatus);

                    if (deleteBeatdt.Rows.Count > 0)
                    {
                        if (deleteBeatdt.Rows[0]["AppStatus"].ToString() == "Pending")
                        {
                            string deletequery = @"delete from TransBeatPlan where DocId='" + ViewState["BeatDocId"] + "'";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, deletequery);
                            //              int retdel = btAll.delete(Convert.ToString(ViewState["TourPlanId"]));
                            string msgUrl = "BeatPlanEntry.aspx?SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "&DocId=" + ViewState["BeatDocId"];
                            deletequery = "delete from TransNotification where msgURL='" + msgUrl + "'";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, deletequery);
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                            ClearControls();
                            Btnsave.Visible = true; Btnsave.Enabled = true; Btnsave.Text = "Save";
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('BeatPlan is under process');", true);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void ClearControls()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            calendarTextBox.Text = string.Empty;
            Btnsave.Visible = false;
            BtnCancel.Visible = false;
            btnDelete.Visible = false;
            ViewState["BeatDataCount"] = null;
            ViewState["appstatus"] = null;
            ViewState["BeatDocId"] = null;
            ViewState["Hdate"] = null;
        }

        protected void ddlBeat_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            if (ddl != null)
            {
                foreach (ListItem li in ddl.Items)
                {
                    li.Attributes["title"] = li.Text;
                }
            }
        }


    }
}