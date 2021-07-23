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
using System.IO;
using System.Data;
using DAL;

namespace AstralFFMS
{
    public partial class LeaveRequest : System.Web.UI.Page
    {
        int msg = 0;
        string pageName = string.Empty;
        string pageName1 = string.Empty;
        int isCurrentUser = 0;
        int smIDSen = 0;
         DataTable holidaydt;
         DataTable dtHoliday;
        BAL.LeaveRequest.LeaveAll lvAll = new BAL.LeaveRequest.LeaveAll();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            pageName = string.Empty;
            pageName1 = string.Empty;
            calendarTextBox.Attributes.Add("readonly", "readonly");
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["lvrQId"] = parameter;
                FillLeaveControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Added
            //Ankita - 09/may/2016- (For Optimization)
            string pageName12 = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName12);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName12, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
          
            if (btnSave.Text == "Save")
            {
              //  btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName12, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName12, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }

          //  btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName12, Convert.ToString(Session["user_name"]));
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btnDelete.CssClass = "btn btn-primary";
            //End
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
                try
                {
                    ddlLF1.Attributes.Add("disabled", "disabled");
                    smIDSen = Convert.ToInt32(Settings.Instance.SMID); //GetSalesPerId(Convert.ToInt32(Settings.Instance.UserID));
                    btnDelete.Visible = false;
                    BindSalePersonDDl();

                    //Ankita - 09/may/2016- (For Optimization)
                    mainDiv.Style.Add("display", "block");
                    //string salesrepquery1 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue);
                    string salesrepquery1 = @"select SMId,SMName,UnderId from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue);
                    DataTable salesrepDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery1);
                    if (salesrepDt1.Rows.Count > 0)
                    {
                        string salesrepquery2 = @"select UserId, SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesrepDt1.Rows[0]["UnderId"]);
                        DataTable salesrepDt2 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery2);
                        ViewState["L1SMID"] = salesrepDt1.Rows[0]["SMId"].ToString();
                        lblappdate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");//DateTime.Now.AddSeconds(19800).ToShortDateString();
                        lblusername.Text = salesrepDt1.Rows[0]["SMName"].ToString(); //Settings.Instance.UserName;
                        lblapprby.Text = salesrepDt2.Rows[0]["SMName"].ToString();
                    }

                    if (Request.QueryString["SMId"] != null && Request.QueryString["LVRQId"] != null)
                    {
                        if (!Convert.ToBoolean(SplitPerm[0])) { ShowAlert("You do not have Permission to view this form. Please Contact System Admin.!!"); return; }
                        //if (Request.QueryString["Page"] != null)
                        //{
                        //    pageName = Request.QueryString["Page"];
                        //}
                        //if (Request.QueryString["PageV"] != null)
                        //{
                        //    pageName1 = Request.QueryString["PageV"];
                        //}
                        //        userIDHiddenField.Value = Request.QueryString["UserID"];
                        userIDHiddenField.Value = Request.QueryString["SMId"];
                        HiddenField1.Value = Request.QueryString["LVRQId"];
                        CheckLeavePlanData(Request.QueryString["SMId"], Request.QueryString["LVRQId"]);
                        ShowLeaveReqData();
                        GetLeaveDataUserWise(userIDHiddenField.Value, HiddenField1.Value);
                    }
                }
                catch(Exception ex)
                {
                    ex.ToString();
                }
            }
          
            //string pageName12 = Path.GetFileName(Request.Path);
            //if (btnSave.Text == "Save")
            //{
            //    btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName12, Convert.ToString(Session["user_name"]));
            //    btnSave.CssClass = "btn btn-primary";
            //}
            //else
            //{
            //    btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName12, Convert.ToString(Session["user_name"]));
            //    btnSave.CssClass = "btn btn-primary";
            //}

            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName12, Convert.ToString(Session["user_name"]));
            //btnDelete.CssClass = "btn btn-primary";
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
        private void BindSalePersonDDl()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            //     dv.RowFilter = "RoleName='Level 1'";
            dv.RowFilter = "SMName<>.";
            DdlSalesPerson.DataSource = dv.ToTable();
            DdlSalesPerson.DataTextField = "SMName";
            DdlSalesPerson.DataValueField = "SMId";
            DdlSalesPerson.DataBind();

            //Added as Per UAT - on 12-Dec-2015
           

            if (Request.QueryString["LeaveCase"] != null)
            {
                //DdlSalesPerson.SelectedValue = Settings.Instance.SMID;
                DdlSalesPerson.SelectedValue = smIDSen.ToString();
            }
            else
            {
                DdlSalesPerson.SelectedValue = smIDSen.ToString();
            }
            //End
            

            DropDownList1.DataSource = dv.ToTable();
            DropDownList1.DataTextField = "SMName";
            DropDownList1.DataValueField = "SMId";
            DropDownList1.DataBind();
            //Add Default Item in the DropDownList
            DropDownList1.Items.Insert(0, new ListItem("--Please select--"));
        }
        private void CheckLeavePlanData(string smID, string lvrQID)
        {//Ankita - 09/may/2016- (For Optimization)
            //string leavePlanDataCountQry = @"select * from TransLeaveRequest where SMId=" + smID + " and LVRQId='" + lvrQID + "'";
            //DataTable leavePlanDataCountdt = DbConnectionDAL.GetDataTable(CommandType.Text, leavePlanDataCountQry);
            string leavePlanDataCountQry = @"select count(*) from TransLeaveRequest where SMId=" + smID + " and LVRQId='" + lvrQID + "'";
            int cntval = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, leavePlanDataCountQry));
           // if (leavePlanDataCountdt.Rows.Count > 0)
            if (cntval > 0)
            {
                chkLeaveDataHdf.Value = "1";
            }
            else
            {
                chkLeaveDataHdf.Value = "0";
            }
        }

        private void FillLeaveControls(int lvrQID)
        {
            try
            {//Ankita - 09/may/2016- (For Optimization)
                //string leavequery = @"select * from TransLeaveRequest where LVRQId=" + lvrQID;
                string leavequery = @"select NoOfDays,L3RejectionRemark,FromDate,ToDate,VDate,SMId,AppStatus,AppRemark,Reason,isnull(LeaveFlag,0) as LeaveFlag from TransLeaveRequest where LVRQId=" + lvrQID;
                DataTable leaveValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, leavequery);

              //  string leavefl = @"select isnull(LeaveFlag,0) as LeaveFlag from TransLeaveRequest where LVRQId=" + lvrQID;
             //   DataTable dtfl = DbConnectionDAL.GetDataTable(CommandType.Text, leavefl);
                if (leaveValueDt.Rows.Count > 0)
                {
       //             NoOfDays.Text = Convert.ToInt32(leaveValueDt.Rows[0]["NoOfDays"]).ToString();
                    NoOfDays.Text = Convert.ToString(Math.Round(Convert.ToDecimal(leaveValueDt.Rows[0]["NoOfDays"]),1));
                    BindLeaveDDl();

                    if (leaveValueDt.Rows[0]["L3RejectionRemark"].ToString() != "")
                    {
                        txtRejectionRemark.Text = leaveValueDt.Rows[0]["L3RejectionRemark"].ToString();
                        txtRejectionRemark.Attributes.Add("disabled", "disabled");
                        RejectionDiv.Style.Add("display", "block");
                    }

                    if (Convert.ToDecimal(NoOfDays.Text) == 1M && Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]) != "C")
                    {
                        ddlLF.Visible = true;
                        ddlLF1.Visible = true;
                        ddlLF.SelectedValue = "S";
                        ddlLF1.SelectedValue = "F";
                        ddlLF1.Attributes.Add("disabled", "disabled");
                        ddlLF.Items.RemoveAt(1);

                    }
                    else if (Convert.ToDecimal(NoOfDays.Text) == 1M && Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]) == "C")
                    {
                        ddlLF.Visible = true;
                        ddlLF.SelectedValue = Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]);
                        ddlLF1.Visible = false;
                        ddlLF.Items.RemoveAt(1);                       
                    }

                    string str = NoOfDays.Text;
                    string[] str1 = str.Split('.');

                    if (Convert.ToDecimal(str1[1]) == 5M && Convert.ToDecimal(str1[0]) == 0M)
                    {
                        ddlLF.Visible = true;
                        ddlLF.SelectedValue = Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]);
                        ddlLF.Items.RemoveAt(3);
                        ddlLF1.Visible = false;
                    }                    
                    else
                    {
                        if (Convert.ToDecimal(NoOfDays.Text) > 1M)
                        {
                            if (Convert.ToDecimal(str1[1]) == 5M)
                            {
                                if (Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]) == "S")
                                {
                                    ddlLF.Visible = true;
                                    ddlLF1.Visible = true;
                                    ddlLF.SelectedValue = Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]);
                                    ddlLF.Items.RemoveAt(1);
                                    //ddlLF.Items.RemoveAt(2);
                                    ddlLF1.Items.RemoveAt(2);
                                    ddlLF1.SelectedValue = "C";
                                    //ddlLF1.Items.RemoveAt(2);
                                }
                                else if (Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]) == "C")
                                {
                                    ddlLF1.Visible = true;
                                    ddlLF.Visible = true;
                                    ddlLF.SelectedValue = "C";
                                    ddlLF1.SelectedValue = "F";
                                    //ddlLF1.SelectedValue = Convert.ToString(dtfl.Rows[0]["LeaveFlag"]);
                                    ddlLF.Items.RemoveAt(1);
                                    //ddlLF.Items.RemoveAt(2);
                                    ddlLF1.Items.RemoveAt(2);
                                    //ddlLF1.Items.RemoveAt(2);
                                }
                            }
                            if (Convert.ToDecimal(str1[1]) == 0M)
                            {
                                ddlLF.Visible = true;
                                ddlLF1.Visible = true;
                                if (Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]) == "C")
                                {
                                    ddlLF.SelectedValue = "C";
                                    ddlLF1.SelectedValue = "C";
                                }
                                if (Convert.ToString(leaveValueDt.Rows[0]["LeaveFlag"]) == "S")
                                {
                                    ddlLF.SelectedValue = "S";
                                    ddlLF1.SelectedValue = "F";
                                }                               
                                ddlLF.Items.RemoveAt(1);
                                //ddlLF.Items.RemoveAt(2);
                                ddlLF1.Items.RemoveAt(2);
                                
                            }
                        }
                    }

                   
                   
                    DateTime fromdt = DateTime.Parse(leaveValueDt.Rows[0]["FromDate"].ToString());
                    DateTime todt = DateTime.Parse(leaveValueDt.Rows[0]["ToDate"].ToString());
                    calendarTextBox.Text = fromdt.ToString("dd/MMM/yyyy");
                    //Reason1.Value = todt.ToString("dd/MMM/yyyy");
                    Reason1.Text = todt.ToString("dd/MMM/yyyy");
                    Reason.Value = leaveValueDt.Rows[0]["Reason"].ToString();
                    DateTime vdatedt1 = DateTime.Parse(leaveValueDt.Rows[0]["VDate"].ToString());
                    //     conditonaldiv.Style.Add("display", "block");
                    //         ddlApproveStatus.Enabled = false;
                    approveStatusRadioButtonList.Enabled = false;

                    if (Request.QueryString["LeaveCase"] != null)
                    {
                        //DdlSalesPerson.SelectedValue = Settings.Instance.SMID;
                    }
                    else
                    {
                        DdlSalesPerson.SelectedValue = leaveValueDt.Rows[0]["SMId"].ToString();
                    }

                    if (leaveValueDt.Rows[0]["AppStatus"].ToString() == "Approve" || leaveValueDt.Rows[0]["AppStatus"].ToString() == "Reject")
                    {
                        //           ddlApproveStatus.SelectedValue = leaveValueDt.Rows[0]["AppStatus"].ToString();
                        approveStatusRadioButtonList.SelectedValue = leaveValueDt.Rows[0]["AppStatus"].ToString();
                        conditonaldiv.Style.Add("display", "block");
                        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
                    }
                    else
                    {
                        //      ddlApproveStatus.SelectedValue = "Approve";
                        approveStatusRadioButtonList.SelectedValue = "Approve";
                        conditonaldiv.Style.Add("display", "none");
                    }

                    //
                    //string salesrepquery12 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(leaveValueDt.Rows[0]["SMId"]);
                    string salesrepquery12 = @"select UnderId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(leaveValueDt.Rows[0]["SMId"]);
                    DataTable salesrepDt123 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery12);
                    if (salesrepDt123.Rows.Count > 0)
                    {
                        string salesrepquery21 = @"select SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesrepDt123.Rows[0]["UnderId"]);
                        DataTable salesrepDt21 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery21);

                        lblappdate.Text = vdatedt1.ToString("dd/MMM/yyyy");  //DateTime.Now.AddSeconds(19800).ToShortDateString();
                        lblusername.Text = salesrepDt123.Rows[0]["SMName"].ToString(); //Settings.Instance.UserName;
                        lblapprby.Text = salesrepDt21.Rows[0]["SMName"].ToString();
                    }
                    //

                    if (leaveValueDt.Rows[0]["AppRemark"].ToString() != "")
                    {
                        TextBox1.Text = leaveValueDt.Rows[0]["AppRemark"].ToString();
                    }
                    if (leaveValueDt.Rows[0]["AppStatus"].ToString() == "Approve" || leaveValueDt.Rows[0]["AppStatus"].ToString() == "Reject")
                    {
                        btnSave.Enabled = false;
                        btnDelete.Visible = true;
                        btnDelete.Enabled = false;
                        btnSave.CssClass = "btn btn-primary";
                        btnDelete.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        //        btnSave.Enabled = false;
                        //Added 08-12-2015
                        btnSave.Enabled = true;
                        btnSave.Text = "Update";
                        //End

                        btnDelete.Enabled = true;
                        btnSave.CssClass = "btn btn-primary";
                        btnFind.Visible = true;
                        btnDelete.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public int ShowLeaveReqData()
        {//Ankita - 09/may/2016- (For Optimization)
          //  string getqry = @"select * from MastSalesRep where UserId=" + Settings.Instance.UserID + "";
            //int SmIDFromSession = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, getqry));
            int SmIDFromSession = Convert.ToInt32(Settings.Instance.SMID);
            if (Convert.ToInt32(Request.QueryString["SMId"]) == SmIDFromSession)
            {
                //        isCurrentUser = 1;
                IsCurrUserHiddenField.Value = "1";
            }
            return isCurrentUser;
        }

        private void fillRepeter()
        {
            try
            {
                string leaveQuery = @" select  r.LVRQId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus from TransLeaveRequest r
                where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " order by VDate desc";

                DataTable leavedt = DbConnectionDAL.GetDataTable(CommandType.Text, leaveQuery);
                rpt.DataSource = leavedt;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void GetLeaveDataUserWise(string smId, string lvrqid)
        {
            try
            {
                string getLeaveDataQuery = @"select  r.LVRQId, r.NoOfDays, r.FromDate, r.ToDate,r.SMId,r.L3RejectionRemark,                                 r.Reason,r.AppStatus,r.AppRemark,r.VDate,r.UserId,sales.SMName,salesnew.SMName as SeniorName 
                          from TransLeaveRequest r 
                          left join MastSalesRep sales on r.UserId=sales.UserId
                          left join MastSalesRep salesnew on sales.UnderId=salesnew.SMId
                          Where r.SMId =" + Convert.ToInt32(smId) + " and r.LVRQId =" + Convert.ToInt32(lvrqid) + "";

                DataTable getleavedt = DbConnectionDAL.GetDataTable(CommandType.Text, getLeaveDataQuery);
                string leavefl = @"select isnull(LeaveFlag,0) as LeaveFlag from TransLeaveRequest where LVRQId=" + lvrqid;
                DataTable dtfl = DbConnectionDAL.GetDataTable(CommandType.Text, leavefl);

                if (getleavedt.Rows.Count > 0)
                {
                    NoOfDays.Text = Convert.ToString(Math.Round(Convert.ToDecimal(getleavedt.Rows[0]["NoOfDays"]), 1));
                    BindLeaveDDl();
                    if (Convert.ToDecimal(NoOfDays.Text) == 1M && Convert.ToString(dtfl.Rows[0]["LeaveFlag"]) != "C")
                    {
                        ddlLF.Visible = true;
                        ddlLF1.Visible = true;
                        ddlLF.SelectedValue = "S";
                        ddlLF1.SelectedValue = "F";
                        ddlLF1.Attributes.Add("disabled", "disabled");
                        ddlLF.Items.RemoveAt(1);
                        
                    }
                    else if (Convert.ToDecimal(NoOfDays.Text) == 1M && Convert.ToString(dtfl.Rows[0]["LeaveFlag"]) == "C")
                    {
                        ddlLF.Visible = true;
                        ddlLF.SelectedValue = Convert.ToString(dtfl.Rows[0]["LeaveFlag"]);
                        ddlLF1.Visible = false;
                        ddlLF.Items.RemoveAt(1);
                    }

                    string str = NoOfDays.Text;
                    string[] str1 = str.Split('.');

                    if (Convert.ToDecimal(str1[1]) == 5M && Convert.ToDecimal(str1[0]) == 0M)
                    {
                        ddlLF.Visible = true;
                        ddlLF.SelectedValue = Convert.ToString(dtfl.Rows[0]["LeaveFlag"]);
                        ddlLF.Items.RemoveAt(3);
                        ddlLF1.Visible = false;
                    }
                    else
                    {
                        if (Convert.ToDecimal(NoOfDays.Text) > 1M)
                        {
                            if (Convert.ToDecimal(str1[1]) == 5M)
                            {
                                if (Convert.ToString(dtfl.Rows[0]["LeaveFlag"]) == "S")
                                {
                                    ddlLF.Visible = true;
                                    ddlLF1.Visible = true;
                                    ddlLF.SelectedValue = Convert.ToString(dtfl.Rows[0]["LeaveFlag"]);
                                    ddlLF.Items.RemoveAt(1);
                                    //ddlLF.Items.RemoveAt(2);
                                    ddlLF1.Items.RemoveAt(2);
                                    ddlLF1.SelectedValue = "C";
                                    //ddlLF1.Items.RemoveAt(2);
                                }
                                else if (Convert.ToString(dtfl.Rows[0]["LeaveFlag"]) == "C")
                                {
                                    ddlLF1.Visible = true;
                                    ddlLF.Visible = true;
                                    ddlLF.SelectedValue = "C";
                                    ddlLF1.SelectedValue = "F";
                                    //ddlLF1.SelectedValue = Convert.ToString(dtfl.Rows[0]["LeaveFlag"]);
                                    ddlLF.Items.RemoveAt(1);
                                    //ddlLF.Items.RemoveAt(2);
                                    ddlLF1.Items.RemoveAt(2);
                                    //ddlLF1.Items.RemoveAt(2);
                                }
                            }
                            if (Convert.ToDecimal(str1[1]) == 0M)
                            {
                                ddlLF.Visible = true;
                                ddlLF1.Visible = true;
                                if (Convert.ToString(dtfl.Rows[0]["LeaveFlag"]) == "C")
                                {
                                    ddlLF.SelectedValue = "C";
                                    ddlLF1.SelectedValue = "C";
                                }
                                if (Convert.ToString(dtfl.Rows[0]["LeaveFlag"]) == "S")
                                {
                                    ddlLF.SelectedValue = "S";
                                    ddlLF1.SelectedValue = "F";
                                }
                                ddlLF.Items.RemoveAt(1);
                                //ddlLF.Items.RemoveAt(2);
                                ddlLF1.Items.RemoveAt(2);

                            }
                        }
                    }
                   Session["hidddlf1"] = ddlLF1.SelectedValue;
                    DateTime fromdt = DateTime.Parse(getleavedt.Rows[0]["FromDate"].ToString());
                    DateTime todt = DateTime.Parse(getleavedt.Rows[0]["ToDate"].ToString());
                    DateTime vdatedt = DateTime.Parse(getleavedt.Rows[0]["VDate"].ToString());
                    calendarTextBox.Text = fromdt.ToString("dd/MMM/yyyy");
                    Reason1.Text = todt.ToString("dd/MMM/yyyy");
                    Reason.Value = getleavedt.Rows[0]["Reason"].ToString();

                    if (Request.QueryString["LeaveRejL3"] != null)
                    {
                        txtRejectionRemark.Text = getleavedt.Rows[0]["L3RejectionRemark"].ToString();
                        txtRejectionRemark.Attributes.Add("disabled", "disabled");
                        RejectionDiv.Style.Add("display", "block");
                    }
                    if (Request.QueryString["LeaveCase"] != null)
                    {
                       
                        //DdlSalesPerson.SelectedValue = Settings.Instance.SMID;
                        DdlSalesPerson.SelectedValue = getleavedt.Rows[0]["SMId"].ToString();
                        if (getleavedt.Rows[0]["AppStatus"].ToString() == "Approve")
                        {
                            RejectionDiv.Style.Add("display", "block");
                            txtRejectionRemark.Attributes.Remove("disabled");  
                        }
                        else
                        {
                            RejectionDiv.Style.Add("display", "none");
                            txtRejectionRemark.Attributes.Add("disabled", "disabled");
                        }

                        if (getleavedt.Rows[0]["L3RejectionRemark"].ToString() != "")
                        {
                            txtRejectionRemark.Text = getleavedt.Rows[0]["L3RejectionRemark"].ToString();
                            txtRejectionRemark.Attributes.Add("disabled", "disabled");
                            RejectionDiv.Style.Add("display", "block");
                        }
                    }
                    else
                    {
                        DdlSalesPerson.SelectedValue = getleavedt.Rows[0]["SMId"].ToString();
                    }
                    
                    //             ddlApproveStatus.SelectedValue = getleavedt.Rows[0]["AppStatus"].ToString();
                    approveStatusRadioButtonList.SelectedValue = getleavedt.Rows[0]["AppStatus"].ToString();
                    appStatusHiddenField.Value = getleavedt.Rows[0]["AppStatus"].ToString();

                    //Ankita - 09/may/2016- (For Optimization)
                    string salesrepquery12 = @"select UnderId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(getleavedt.Rows[0]["SMId"]);
                    DataTable salesrepDt12 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery12);
                    if (salesrepDt12.Rows.Count > 0)
                    {
                        string salesrepquery21 = @"select SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesrepDt12.Rows[0]["UnderId"]);
                        DataTable salesrepDt21 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery21);

                        lblappdate.Text = vdatedt.ToString("dd/MMM/yyyy");  //DateTime.Now.AddSeconds(19800).ToShortDateString();
                        lblusername.Text = salesrepDt12.Rows[0]["SMName"].ToString(); //Settings.Instance.UserName;
                        lblapprby.Text = salesrepDt21.Rows[0]["SMName"].ToString();
                    }
                    //

                    if (getleavedt.Rows[0]["AppRemark"].ToString() != "")
                    {
                        TextBox1.Text = getleavedt.Rows[0]["AppRemark"].ToString();
                    }
                    btnFind.Visible = false;
                    if (IsCurrUserHiddenField.Value == "1")
                    {
                        btnSave.Text = "Save";
                        btnSave.Enabled = false;
                        btnSave.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        if (appStatusHiddenField.Value == "Approve" || appStatusHiddenField.Value == "Reject")
                        {
                            if (Request.QueryString["LeaveCase"] != null)
                            {
                                btnSave.Text = "Submit";
                                btnSave.Enabled = true;
                                btnSave.CssClass = "btn btn-primary";

                               
                            }
                            else
                            {
                                btnSave.Text = "Submit";
                                btnSave.Enabled = false;
                                btnSave.CssClass = "btn btn-primary";
                            }

                        }
                        else
                        {
                            btnSave.Text = "Submit";
                            btnSave.Enabled = true;
                        }

                        if (getleavedt.Rows[0]["L3RejectionRemark"].ToString() != "")
                        {
                            btnSave.Text = "Submit";
                            btnSave.Enabled = false;
                            btnSave.CssClass = "btn btn-primary";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {             
                if (ddlLF.Visible)
                {
                    if (ddlLF.SelectedValue=="0")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Second Half/Full');", true);
                        return;
                    }
                    
                }

                //if (ddlLF1.Visible)
                //{
                //    if (ddlLF1.SelectedValue == "0")
                //    {
                //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select First Half');", true);
                //        return;
                //    }

                //}

                if (calendarTextBox.Text=="")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select date');", true);
                    return;
                }
                if (Reason.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select reason');", true);
                    return;
                }

                if (btnSave.Text == "Update")
                {
                    UpdateLeaveFromTable();
                }
                else if (btnSave.Text == "Submit")
                {
                    if (Request.QueryString["LeaveCase"] != null)
                    {                     
                        if (txtRejectionRemark.Text != string.Empty)
                        {
                            UpdateLeaveRequestL3();
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Rejection Remark');", true);
                            return;
                        }
                    }
                    else
                    {
                        if (TextBox1.Text != string.Empty)
                        {
                            UpdateLeaveRequest();
                        }
                        else
                        {
                            ddlLF1.SelectedValue = Session["hidddlf1"].ToString();
                            conditonaldiv.Style.Add("display", "block");
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Remark');", true);
                            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunctionNew();", true);
                        }
                    }
                    
                }
                else
                {
                  //int IsOff = IsWeekOff(Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Value));
                  //if (IsOff == 1)
                  //{
                  //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Leave is not allowed on Week off');", true);
                  //    return;
                  //}
                  int chkLastDay = IslastDaySunday(Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text));
                  if (chkLastDay == 1)
                  {
                      System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Leave is not allowed on Week off');", true);
                      return;
                  }
                  
                      InsertLeaveRequest();
                 
                  
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private int IslastDaySunday(DateTime fromdate, DateTime toDate)
        {
            if (toDate.DayOfWeek.ToString() == "Sunday")
            {
                return 1;
            }
            return 0;  
        }

        private int IsWeekOff(DateTime fromdate, DateTime toDate)
        {
            //string query = "select * from MastHoliday where Active = 1 and convert(varchar(10), holiday_date, 106) >= convert(varchar(10), '" + fromdate + "', 106) and convert(varchar(10), holiday_date, 106) <= convert(varchar(10), '" + toDate + "', 106)";
            string dayName;
            int diff = Convert.ToInt32((toDate - fromdate).TotalDays);
            if (diff >= 3)
            {
                for (int i = 0; i <= diff; i++)
                {
                    dayName = fromdate.DayOfWeek.ToString();
                    if (dayName == "Sunday")
                    {
                        return 1;
                    }
                    fromdate = fromdate.AddDays(1);
                }
            }
            return 0;           
        }

        private void UpdateLeaveRequestL3()
        {
            try
            {
                DateTime newDate1 = GetUTCTime();
                //string updateTLR = @"select * from TransLeaveRequest where UserId=" + Convert.ToInt32(Request.QueryString["UserID"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                //Ankita - 09/may/2016- (For Optimization)
                //string updateTLR = @"select * from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                string updateTLR = @"select UserId,AppRemark from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                DataTable updateTLRdt = DbConnectionDAL.GetDataTable(CommandType.Text, updateTLR);
                if (updateTLRdt.Rows.Count > 0)
                {
                    //int retsave = lvAll.UpdateLeaveStatus(Convert.ToInt64(Request.QueryString["LVRQId"]), Convert.ToInt32(Request.QueryString["UserID"]), approveStatusRadioButtonList.SelectedValue, Convert.ToInt32(Settings.Instance.UserID), TextBox1.Text, Convert.ToInt32(DdlSalesPerson.SelectedValue));
                    //int retsave = lvAll.UpdateLeaveStatus1(Convert.ToInt64(Request.QueryString["LVRQId"]), Convert.ToInt32(updateTLRdt.Rows[0]["UserId"]), "Reject", Convert.ToInt32(Settings.Instance.UserID), TextBox1.Text, Convert.ToInt32(DdlSalesPerson.SelectedValue), smIDSen, txtRejectionRemark.Text, Settings.GetUTCTime());
                    int retsave = lvAll.UpdateLeaveStatus1(Convert.ToInt64(Request.QueryString["LVRQId"]), Convert.ToInt32(updateTLRdt.Rows[0]["UserId"]), "Reject", Convert.ToInt32(Settings.Instance.UserID), updateTLRdt.Rows[0]["AppRemark"].ToString(), Convert.ToInt32(DdlSalesPerson.SelectedValue), Convert.ToInt32(Settings.Instance.SMID), txtRejectionRemark.Text, Settings.GetUTCTime());


                    if (retsave != 0)
                    {
                        //string updateTLR1 = @"select * from TransLeaveRequest where UserId=" + Convert.ToInt32(Request.QueryString["UserID"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                        //string updateTLR1 = @"select * from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                        string updateTLR1 = @"select AppStatus,SMId,UserId from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                        DataTable updateTLRdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, updateTLR1);

                        string AppStatus = updateTLRdt1.Rows[0]["AppStatus"].ToString();
                        string smID = updateTLRdt1.Rows[0]["SMId"].ToString();

                        //
                        string senSMNameQry = @"select SMName from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                        string senSMName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQry));
                        int salesRepID = Convert.ToInt32(updateTLRdt1.Rows[0]["SMId"]);
                        int UserId = Convert.ToInt32(updateTLRdt1.Rows[0]["UserId"]);
                        //

                        string senSMNameQryL2 = @"select UnderId from MastSalesRep where SMId=" + salesRepID + "";
                        int UnderID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQryL2));

                        string senQryL2 = @"select SMId from MastSalesRep where SMId=" + UnderID + "";
                        int SMIDL2 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senQryL2));

                        string senUserQryL2 = @"select UserId from MastSalesRep where SMId=" + SMIDL2 + "";
                        int UserIDL2 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL2));

                      
                        string displayTitle = string.Empty;
                       
                        displayTitle = "Leave Rejected By - " + senSMName + " ";

                        //               string msgUrl1 = "LeaveRequest.aspx?UserId=" + Convert.ToInt32(Request.QueryString["UserID"]) + "&LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]);
                        string msgUrl1 = "LeaveRequest.aspx?SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + "&LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "&LeaveRejL3=RL3";

                        lvAll.InsertTransNotification("LEAVEREJECTED", UserId, newDate1, msgUrl1, displayTitle, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(salesRepID));
                        lvAll.InsertTransNotification("LEAVEREJECTED", UserIDL2, newDate1, msgUrl1, displayTitle, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ViewState["L1SMID"]), SMIDL2);

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Leave Rejected Successfully!');", true);
                        ClearControls();


                    }

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void UpdateLeaveFromTable()
        {
            try
            {
//                string holidayCountQry = @" select count(*) FROM [dbo].[MastHoliday] as HolidayCount
//WHERE ((cast(holiday_date as date) >= '" + Settings.dateformat(calendarTextBox.Text) + "' AND cast(holiday_date as date) <= '" + Settings.dateformat(calendarTextBox.Text) + "')) AND area_id IN ( SELECT maingrp FROM   mastareagrp WHERE  areaid IN ( SELECT linkcode FROM   mastlink WHERE  primtable = 'SALESPERSON'                     AND    linktable = 'AREA' AND primcode IN (" + DdlSalesPerson.SelectedValue + ")))";

                string holidayCountQry = @" select count(*) FROM [dbo].[View_Holiday] WHERE ((cast(View_Holiday.holiday_date as date) >= '" + Settings.dateformat(calendarTextBox.Text) + "' AND cast(View_Holiday.holiday_date as date) <= '" + Settings.dateformat(calendarTextBox.Text) + "')) AND View_Holiday.smid IN (" + DdlSalesPerson.SelectedValue + ")";
                int holidayCount = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, holidayCountQry));

                if (holidayCount > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Holiday is declared for this day.');", true);
                }
                //
                else
                {
                    int Val1 = lvAll.CheckExistingLeaveOnUpdate(Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text), Convert.ToInt32(DdlSalesPerson.SelectedValue), ViewState["lvrQId"].ToString());
                    if (Val1 == 0)
                    {//Ankita - 09/may/2016- (For Optimization)
                        //string LeaveReqEditQry = @"select * from TransLeaveRequest where LVRQId='" + ViewState["lvrQId"] + "'";// ViewState["lvrQId"]
                        string LeaveReqEditQry = @"select count(*) from TransLeaveRequest where LVRQId='" + ViewState["lvrQId"] + "'";// ViewState["lvrQId"]
                       // DataTable LeavePlanEditdt = DbConnectionDAL.GetDataTable(CommandType.Text, LeaveReqEditQry);
                        int Lrcnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, LeaveReqEditQry));
                      //  if (LeavePlanEditdt.Rows.Count > 0)
                        if (Lrcnt > 0)
                        {
                            DateTime newDateT1 = GetUTCTime().Date;
                            string strLF = "";
                            if (ddlLF.Visible)
                            {
                                strLF = ddlLF.SelectedItem.Value;
                            }
                            
                            string leaveString = "";
                            int leaveLoop = Convert.ToInt32(Convert.ToDecimal(NoOfDays.Text) * 2);
                            if (ddlLF.Visible)
                            {
                                strLF = ddlLF.SelectedItem.Value;
                            }

                            //Added
                            // int dOutput = 0;
                            // if (Int32.TryParse(NoOfDays.Text, out dOutput))
                            //{
                            //    strLF = "";
                            //}
                            //End


                            if (strLF == "C")
                            {
                                for (int loop = 0; loop < leaveLoop; loop++)
                                {
                                    leaveString += "L";
                                }
                                if (leaveLoop % 2 == 1)
                                {
                                    leaveString += " ";
                                }
                            }
                            if (strLF == "S")
                            {
                                leaveString = " ";
                                for (int loop = 0; loop < leaveLoop; loop++)
                                {
                                    leaveString += "L";
                                }
                                if (leaveLoop % 2 == 0)
                                {
                                    leaveString += " ";
                                }
                            }

                            if (strLF == "F")
                            {
                                for (int loop = 0; loop < leaveLoop; loop++)
                                {
                                    leaveString += "L ";
                                }
                            }


                           
                            //Added
                            //int dOutput = 0;
                            //if (Int32.TryParse(NoOfDays.Text, out dOutput))
                            //{
                            //    strLF = "";
                            //}
                            //End
                            int retsave = lvAll.UpdateLeaveStatusTable(ViewState["lvrQId"].ToString(), Convert.ToInt32(Settings.Instance.UserID), newDateT1, Convert.ToDecimal(NoOfDays.Text), Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text), Reason.Value, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), 0,strLF,leaveString);

                            if (retsave == 2)
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                                ClearControls();
                            }
                        }
                    }
                    else
                    {
                        ddlLF.SelectedValue = "0";
                        ddlLF1.SelectedValue = "0";
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('  Leave Already Exists.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void InsertLeaveRequest()
        {
            try
            {
               // int Val1 = lvAll.CheckExistingDSR(Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text), Convert.ToInt32(DdlSalesPerson.SelectedValue));
                //if (ddlLF.Visible)
                //{
                //    if (ddlLF.SelectedValue == "C")
                //    {
                //        if (Val1 != 0)
                //        {
                //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('DSR already exist within from date and to date!');", true);
                //            return;
                //        }
                //    }
                //}

//                string holidayCountQry = @" select count(*) FROM [dbo].[View_Holiday] as HolidayCount
//WHERE ((cast(holiday_date as date) >= '" + Settings.dateformat(calendarTextBox.Text) + "' AND cast(holiday_date as date) <= '" + Settings.dateformat(calendarTextBox.Text) + "')) AND area_id IN ( SELECT maingrp FROM   mastareagrp WHERE  areaid IN ( SELECT linkcode FROM   mastlink WHERE  primtable = 'SALESPERSON'                     AND    linktable = 'AREA' AND primcode IN (" + DdlSalesPerson.SelectedValue + ")))";

                string holidayCountQry = @" select count(*) FROM [dbo].[View_Holiday] WHERE ((cast(View_Holiday.holiday_date as date) >= '" + Settings.dateformat(calendarTextBox.Text) + "' AND cast(View_Holiday.holiday_date as date) <= '" + Settings.dateformat(calendarTextBox.Text) + "')) AND View_Holiday.smid IN (" + DdlSalesPerson.SelectedValue + ")";

                int holidayCount = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, holidayCountQry));

                if (holidayCount > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Holiday is declared for this day.');", true);
                }
                //
                else
                {
                    int Val = lvAll.CheckExistingLeave(Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text), Convert.ToInt32(DdlSalesPerson.SelectedValue));
                    if (Val == 0)
                    {
                        DateTime newDate = GetUTCTime().Date;
                        string docId = lvAll.GetDociId(newDate);
                        string strLF = "";
                        string leaveString = "";
                        int leaveLoop = Convert.ToInt32(Convert.ToDecimal(NoOfDays.Text) * 2);
                        if(ddlLF.Visible)
                        {
                            strLF = ddlLF.SelectedItem.Value;
                        }
                        
                        //Added
                        // int dOutput = 0;
                        // if (Int32.TryParse(NoOfDays.Text, out dOutput))
                        //{
                        //    strLF = "";
                        //}
                        //End

                        if (strLF == "C")
                        {
                            for (int loop = 0; loop < leaveLoop; loop++)
                            {
                                leaveString += "L";
                            }
                            if (leaveLoop % 2 == 1)
                            {
                                leaveString += " ";
                            }
                        }
                        if (strLF == "S")
                        {
                            leaveString = " ";
                            for (int loop = 0; loop < leaveLoop; loop++)
                            {
                                leaveString += "L";
                            }
                            if (leaveLoop % 2 == 0)
                            {
                                leaveString += " ";
                            }
                        }

                        if (strLF == "F")
                        {
                            for (int loop = 0; loop < leaveLoop; loop++)
                            {
                                leaveString += "L ";
                            }
                        }
                       
                        Int64 retsave = lvAll.Insert(docId, Convert.ToInt32(Settings.Instance.UserID), newDate, Convert.ToDecimal(NoOfDays.Text), Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text), Reason.Value, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), 0, strLF, leaveString);

                        lvAll.SetDociId(docId);

                        string query = @"select LeaveApproval from MastEnviro";
                        DataTable getleaveAppStatdt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                        //         var me = from env in context.MastEnviros select new { env.LeaveApproval };

                        if (Convert.ToBoolean(getleaveAppStatdt.Rows[0]["LeaveApproval"]) == true)
                        {
                            string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                            DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);
                            string salesRepqueryNew1 = "";
                            if (salesRepqueryNewdt.Rows.Count > 0)
                            {
                                salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                            }//Ankita - 09/may/2016- (For Optimization)
                            //string getSeniorSMId = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                            string getSeniorSMId = @"select UnderId,UserId,SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue) + "";
                            DataTable salesRepqryForSManNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId);
                            int senSMid = 0;string seniorName = string.Empty;
                            if (salesRepqryForSManNewdt.Rows.Count > 0)
                            {
                                //Ankita - 09/may/2016- (For Optimization)
                                //string getSeniorSMId12 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UnderId"]) + "";
                                string getSeniorSMId12 = @"select SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UnderId"]) + "";
                                DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                                if (salesRepqryForSManNewdt12.Rows.Count > 0)
                                {
                                    senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
                                    seniorName = salesRepqryForSManNewdt12.Rows[0]["SMName"].ToString();
                                }
                            }
                            DateTime newDate12 = GetUTCTime();
                            DataTable salesRepqueryNewdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew1);
                            //               string msgUrl = "LeaveRequest.aspx?UserId=" + Settings.Instance.UserID + "&LVRQId=" + retsave;
                            string msgUrl = "LeaveRequest.aspx?SMId=" + DdlSalesPerson.SelectedValue + "&LVRQId=" + retsave;

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
                            //   if (DdlSalesPerson.SelectedIndex > 0)
                            if (IsSenior == 1)
                            {
                                //lvAll.InsertTransNotification("LEAVEREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), newDate12, msgUrl, "Leave Request By ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(DdlSalesPerson.SelectedValue), senSMid);
                                string updateLeaveStatusQry = @"update TransLeaveRequest set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ",AppRemark='Approved By Senior' where LVRQId='" + retsave + "' ";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateLeaveStatusQry);

                                //Added
                                //string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int toSMId
                                lvAll.InsertTransNotification("LEAVEAPPROVED", Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]), newDate12, msgUrl, "Leave Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), senSMid, Convert.ToInt32(DdlSalesPerson.SelectedValue));

                                string msgUrl1 = "LeaveRequest.aspx?SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]) + "&LVRQId=" + Convert.ToInt64(retsave) + "&LeaveCase=AC";

                               int salesRepID = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]);
                               int UserId = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]);
                               //

                               string senSMNameQryL3 = @"select UnderId from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                               int UnderID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQryL3));

                               string senQryL3 = @"select SMId from MastSalesRep where SMId=" + UnderID + "";
                               int SMIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senQryL3));

                               string senUserQryL3 = @"select UserId from MastSalesRep where SMId=" + SMIDL3 + "";
                               int UserIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL3));

                               lvAll.InsertTransNotificationL3("LEAVEAPPROVED", UserIDL3, newDate12, msgUrl1, "Leave Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), SMIDL3, Convert.ToInt32(salesRepID));

                                //End
                            }
                            else
                            {
                                lvAll.InsertTransNotification("LEAVEREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), newDate12, msgUrl, "Leave Request By - " + salesRepqryForSManNewdt.Rows[0]["SMName"] + " " + " " + " From -" + calendarTextBox.Text + " To -" + Reason1.Text + " ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(DdlSalesPerson.SelectedValue), senSMid);
                            }
                        }

                        if (retsave != 0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully  <br/> DocNo-" + docId + "');", true);
                            ClearControls();
                        }
                    }
                    else
                    {
                        ddlLF.SelectedValue = "0";
                        ddlLF1.SelectedValue = "0";
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('  Leave Already Exists.');", true);
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
            NoOfDays.Text = string.Empty;
            calendarTextBox.Text = string.Empty;
            Reason1.Text = string.Empty;
            Reason.Value = string.Empty;
            //       ddlApproveStatus.SelectedIndex = 0;
            DdlSalesPerson.SelectedIndex = 0;
            approveStatusRadioButtonList.SelectedValue = "Approve";
            TextBox1.Text = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            btnFind.Visible = true;
            btnSave.Enabled = true;
            ddlLF.Visible = false;
            ddlLF1.Visible = false;
            txtRejectionRemark.Text = "";
        }
        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        private void UpdateLeaveRequest()
        {
            try
            {
                DateTime newDate1 = GetUTCTime();
                //string updateTLR = @"select * from TransLeaveRequest where UserId=" + Convert.ToInt32(Request.QueryString["UserID"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                //Ankita - 09/may/2016- (For Optimization)
                //string updateTLR = @"select * from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                string updateTLR = @"select UserId from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                DataTable updateTLRdt = DbConnectionDAL.GetDataTable(CommandType.Text, updateTLR);
                if (updateTLRdt.Rows.Count > 0)
                {
                    //int retsave = lvAll.UpdateLeaveStatus(Convert.ToInt64(Request.QueryString["LVRQId"]), Convert.ToInt32(Request.QueryString["UserID"]), approveStatusRadioButtonList.SelectedValue, Convert.ToInt32(Settings.Instance.UserID), TextBox1.Text, Convert.ToInt32(DdlSalesPerson.SelectedValue));
                    int retsave = lvAll.UpdateLeaveStatus(Convert.ToInt64(Request.QueryString["LVRQId"]), Convert.ToInt32(updateTLRdt.Rows[0]["UserId"]), approveStatusRadioButtonList.SelectedValue, Convert.ToInt32(Settings.Instance.UserID), TextBox1.Text, Convert.ToInt32(DdlSalesPerson.SelectedValue), Convert.ToInt32(Settings.Instance.SMID));

                  
                    if (retsave != 0)
                    {
                        //string updateTLR1 = @"select * from TransLeaveRequest where UserId=" + Convert.ToInt32(Request.QueryString["UserID"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                       //string updateTLR1 = @"select * from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                        string updateTLR1 = @"select AppStatus,SMId,UserId from TransLeaveRequest where SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + " and LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "";
                        //Ankita - 09/may/2016- (For Optimization)
                        DataTable updateTLRdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, updateTLR1);

                        string AppStatus = updateTLRdt1.Rows[0]["AppStatus"].ToString();
                        string smID = updateTLRdt1.Rows[0]["SMId"].ToString();

                        //
                        string senSMNameQry = @"select SMName from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                        string senSMName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQry));
                        int salesRepID = Convert.ToInt32(updateTLRdt1.Rows[0]["SMId"]);
                        int UserId = Convert.ToInt32(updateTLRdt1.Rows[0]["UserId"]);
                        //

                        string senSMNameQryL3 = @"select UnderId from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                        int UnderID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQryL3));

                        string senQryL3 = @"select SMId from MastSalesRep where SMId=" + UnderID + "";
                        int SMIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senQryL3));

                        string senUserQryL3 = @"select UserId from MastSalesRep where SMId=" + SMIDL3 + "";
                        int UserIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL3));
                        

                        //     string toSMId = senSMid.ToString();
                        string pro_id = string.Empty;
                        string displayTitle = string.Empty;
                        if (AppStatus == "Approve")
                        {
                            pro_id = "LEAVEAPPROVED";
                            displayTitle = "Leave Approved By - " + senSMName + " ";
                        }
                        else
                        {
                            pro_id = "LEAVEREJECTED";
                            displayTitle = "Leave Rejected By - " + senSMName + " ";
                        }                       

                        //  string msgUrl1 = "LeaveRequest.aspx?UserId=" + Convert.ToInt32(Request.QueryString["UserID"]) + "&LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]);
                        string msgUrl1 = "LeaveRequest.aspx?SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + "&LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]);
                        //lvAll.InsertTransNotification(pro_id, Convert.ToInt32(Request.QueryString["UserID"]), newDate1, msgUrl1, displayTitle, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(toSMId), Convert.ToInt32(smID));
                        //string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int toSMId
                        lvAll.InsertTransNotification(pro_id, UserId, newDate1, msgUrl1, displayTitle, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(salesRepID));

                        msgUrl1 = "LeaveRequest.aspx?SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + "&LVRQId=" + Convert.ToInt64(Request.QueryString["LVRQId"]) + "&LeaveCase=AC";

                        lvAll.InsertTransNotificationL3(pro_id, UserIDL3, newDate1, msgUrl1, displayTitle, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), SMIDL3, Convert.ToInt32(salesRepID));

                        
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        if (pageName == "LVRAPPROVAL")
                        {
                            // Response.Redirect("~/LeaveApproval.aspx");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                       "alert('Record Updated Successfully'); window.location='" + Request.ApplicationPath + "LeaveApproval.aspx?SMId=" + Convert.ToInt32(Request.QueryString["SMId"]) + "';", true);
                        }
                        else if (pageName1 == "VIEWMSG")
                        {
                            //               Response.Redirect("~/ViewAllMessages.aspx");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                    "alert('Record Updated Successfully'); window.location='" + Request.ApplicationPath + "ViewAllMessages.aspx';", true);
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                             "alert('Record Updated Successfully'); window.location='" + Request.ApplicationPath + "LeaveApproval.aspx';", true);

                            //                            Response.Redirect("~/LeaveApproval.aspx");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (pageName == "LVRAPPROVAL")
            {
                Response.Redirect("~/LeaveApproval.aspx");
            }
            else if (pageName1 == "VIEWMSG")
            {
                Response.Redirect("~/ViewAllMessages.aspx");
            }
            else
            {
                Response.Redirect("~/LeaveRequest.aspx");
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                //Ankita - 09/may/2016- (For Optimization)
               // string appStatus = @"select * from TransLeaveRequest where LVRQId='" + Convert.ToString(ViewState["lvrQId"]) + "' and AppStatus='Pending'";
                //DataTable deleteTLRdt = DbConnectionDAL.GetDataTable(CommandType.Text, appStatus);
                string appStatus = @"select count(*) from TransLeaveRequest where LVRQId='" + Convert.ToString(ViewState["lvrQId"]) + "' and AppStatus='Pending'";
                int deleteTLR = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, appStatus));
             //   if (deleteTLRdt.Rows.Count > 0)
                if (deleteTLR > 0)
                {
                   
                    int retdel = lvAll.delete(Convert.ToString(ViewState["lvrQId"]));
                    if (retdel == 1)
                    {
                        string msgUrl = "LeaveRequest.aspx?SMId=" + DdlSalesPerson.SelectedValue + "&LVRQId=" + Convert.ToString(ViewState["lvrQId"]);
                        string qry = "delete from TransNotification where msgURL= '" + msgUrl + "'";
                       if(DAL.DbConnectionDAL.ExecuteQuery(qry)==1)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                            ClearControls();
                        }
                      
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Leave is either Approved or Rejected.');", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
                    FillLeaveControls(Convert.ToInt32(ViewState["lvrQId"]));
                }
            }
            else
            {
                //      this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                FillLeaveControls(Convert.ToInt32(ViewState["lvrQId"]));
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }


        protected void btnFind_Click(object sender, EventArgs e)
        {
            //     fillRepeter();
            rpt.DataSource = null;
            rpt.DataBind();

            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End

            //txtfmDate.Text = string.Empty;
            //txttodate.Text = string.Empty;

            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void NoOfDays_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BindLeaveDDl();
                //ddlLF1.Enabled = true;
                //if (Convert.ToDouble(NoOfDays.Text) < 1)
                //{
                //    NoOfDays.Text = "0.5";
                //}
                if (NoOfDays.Text.Contains('.'))
                {
                    string[] dayssplit = (NoOfDays.Text).Split('.');
                    if (dayssplit[1] != "5")
                    {
                        NoOfDays.Text = (Convert.ToDecimal(dayssplit[0]) + Convert.ToDecimal("0.5")).ToString();
                    }
                }
                string Date = string.Empty;
                string str = NoOfDays.Text;                           
                string[] str1 = str.Split('.');
                if(str1.Length==2)
                {
                    if (Convert.ToDecimal(str1[1]) > 0)
                    {
                        ddlLF.Visible = true;
                        if(str1[1]=="5" && str1[0]=="0")
                        {
                            ddlLF.Items.RemoveAt(3);
                            ddlLF1.Visible = false;
                        }
                        else
                        {
                            ddlLF.Items.RemoveAt(1);                            
                            //ddlLF1.Items.RemoveAt(2);                           
                            ddlLF.Visible = true;
                            ddlLF1.Visible = true;
                        }
                       
                    }
                    else
                    {
                        ddlLF.Visible = true;
                        ddlLF1.Visible = true;
                        ddlLF.Items.RemoveAt(1);
                        ddlLF1.Items.RemoveAt(2);
                    }
                }
                else
                {
                    if (Convert.ToDecimal(str) == 1M)
                    {
                        ddlLF.Visible = true;
                        ddlLF1.Visible = true;
                        ddlLF.Items.RemoveAt(1);
                        ddlLF1.Items.RemoveAt(2);

                        //ddlLF1.Items.RemoveAt(3);                        
                    }
                    else
                    {
                        ddlLF.Items.RemoveAt(1);                       
                        ddlLF1.Items.RemoveAt(2);                      
                        ddlLF.Visible = true;
                        ddlLF1.Visible = true;
                    }
                }
               
                if (calendarTextBox.Text != "")
                {
                    decimal dt1 = Convert.ToDecimal(NoOfDays.Text);
                    decimal fnum = Math.Floor(dt1);
                    decimal lnum = Math.Ceiling(dt1);
                    decimal num2 = (fnum + lnum) / 2;
                    decimal num = Math.Round(num2, 0, MidpointRounding.AwayFromZero);
            //        if (Convert.ToInt32(NoOfDays.Text) > 1)
                    if (num > 1)
                    {
                        Reason1.Text = string.Empty;
             //           DateTime Todate = Convert.ToDateTime(calendarTextBox.Text).AddDays(Convert.ToInt32(NoOfDays.Text) - 1);
                        DateTime Todate = Convert.ToDateTime(calendarTextBox.Text).AddDays(Convert.ToInt32(num) - 1);
                        NoOfDays.Text = num2.ToString();
                        Date = (Todate).ToString("dd/MMM/yyyy");
                        Reason1.Text = Convert.ToString(Date);
                    }
                    else
                    {
                        DateTime Todate = Convert.ToDateTime(calendarTextBox.Text);
                        Date = Todate.ToString("dd/MMM/yyyy");
                        Reason1.Text = Convert.ToString(Date);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindLeaveDDl()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Rows.Add();
            dt.Rows[0]["ID"] = "0";
            dt.Rows[0]["Name"] = "Select";

            dt.Rows.Add();
            dt.Rows[1]["ID"] = "F";
            dt.Rows[1]["Name"] = "First Half";

            dt.Rows.Add();
            dt.Rows[2]["ID"] = "S";
            dt.Rows[2]["Name"] = "Second Half";

            dt.Rows.Add();
            dt.Rows[3]["ID"] = "C";
            dt.Rows[3]["Name"] = "Full";

            ddlLF.DataValueField = "ID";
            ddlLF.DataTextField = "Name";
            ddlLF.DataSource = dt;
            ddlLF.DataBind();

            ddlLF1.DataValueField = "ID";
            ddlLF1.DataTextField = "Name";
            ddlLF1.DataSource = dt;
            ddlLF1.DataBind();
        }

        protected void calendarTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string Date = string.Empty;
                if (calendarTextBox.Text != "")
                {
                    decimal dt =Convert.ToDecimal(NoOfDays.Text);
                    decimal fnum = Math.Floor(dt);
                    decimal lnum = Math.Ceiling(dt);
                    decimal num2 = (fnum + lnum) / 2;
            //        decimal val = (Math.Round((dt * 2),MidpointRounding.AwayFromZero)) / 2;
                    //if (Convert.ToInt32(NoOfDays.Text) > 1)
                    decimal num = Math.Round(num2, 0, MidpointRounding.AwayFromZero);
                    if (num > 1)
                    {
                        Reason1.Text = string.Empty;
                        //DateTime Todate = Convert.ToDateTime(calendarTextBox.Text).AddDays(Convert.ToInt32(NoOfDays.Text) - 1);
                        DateTime Todate = Convert.ToDateTime(calendarTextBox.Text).AddDays(Convert.ToInt32(num) - 1);
                        NoOfDays.Text = num2.ToString();
                        Date = (Todate).ToString("dd/MMM/yyyy");
                        Reason1.Text = Convert.ToString(Date);
                    }
                    else
                    {
                        DateTime Todate = Convert.ToDateTime(calendarTextBox.Text);
                        Date = Todate.ToString("dd/MMM/yyyy");
                        Reason1.Text = Convert.ToString(Date);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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

            if (DropDownList1.SelectedIndex != 0)
            {
                qrychk = "and r.SMId=" + Convert.ToInt32(DropDownList1.SelectedValue) + "";
            }
            else
            {
                qrychk = "and r.SMId in (" + smIDStr1 + ")";
            }

            //         if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty && DropDownList1.SelectedIndex != 0)
            //         {
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            {
                string leavequery = @" select  r.LVRQId,r.LVRDocId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
                where r.VDate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' " + qrychk + " order by r.VDate desc";
                DataTable leaveqrydt1 = DbConnectionDAL.GetDataTable(CommandType.Text, leavequery);
                if (leaveqrydt1.Rows.Count > 0)
                {
                    rpt.DataSource = leaveqrydt1;
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
            //         }
            //            else if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty)
            //            {
            //                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            //                {
            //                    string compltquery = @" select  r.LVRQId,r.LVRDocId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
            //                where r.SMId in (" + smIDStr1 + ") and r.Vdate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' order by r.Vdate desc";
            //                    DataTable complaintdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, compltquery);
            //                    if (complaintdt1.Rows.Count > 0)
            //                    {
            //                        rpt.DataSource = complaintdt1;
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
            //                string leavequery2 = @" select  r.LVRQId,r.LVRDocId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
            //                where r.SMId in (" + smIDStr1 + ") and r.Vdate>='" + Settings.dateformat(txtfmDate.Text) + "' order by r.Vdate desc";
            //                DataTable leavetdt3 = DbConnectionDAL.GetDataTable(CommandType.Text, leavequery2);
            //                if (leavetdt3.Rows.Count > 0)
            //                {
            //                    rpt.DataSource = leavetdt3;
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
            //                string compltquery2 = @" select  r.LVRQId,r.LVRDocId,r.VDate, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
            //                where r.SMId=" + Convert.ToInt32(DropDownList1.SelectedValue) + "  order by r.Vdate desc";
            //                DataTable complaintdt3 = DbConnectionDAL.GetDataTable(CommandType.Text, compltquery2);
            //                if (complaintdt3.Rows.Count > 0)
            //                {
            //                    rpt.DataSource = complaintdt3;
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
            //                string leavequery3 = @" select  r.LVRQId,r.VDate,r.LVRDocId, r.NoOfDays, r.FromDate, r.ToDate, r.Reason,r.AppRemark,r.AppStatus,r.SMId,msr.SMName from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId
            //                where r.SMId in (" + smIDStr1 + ") order by r.VDate desc";
            //                DataTable leaveqrydt4 = DbConnectionDAL.GetDataTable(CommandType.Text, leavequery3);
            //                if (leaveqrydt4.Rows.Count > 0)
            //                {
            //                    rpt.DataSource = leaveqrydt4;
            //                    rpt.DataBind();
            //                }
            //                else
            //                {
            //                    rpt.DataSource = null;
            //                    rpt.DataBind();
            //                }
            //            }
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

        protected void DdlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
        {//Ankita - 09/may/2016- (For Optimization)
            //string salesrepqueryNew1 = @"select * from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue);
            string salesrepqueryNew1 = @"select UnderId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(DdlSalesPerson.SelectedValue);
            DataTable salesrepNewDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqueryNew1);
            if (salesrepNewDt1.Rows.Count > 0)
            {
                string salesrepquery2 = @"select UserId, SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesrepNewDt1.Rows[0]["UnderId"]);
                DataTable salesrepDt2 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepquery2);
                lblusername.Text = string.Empty; lblapprby.Text = string.Empty;
                lblappdate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// DateTime.Now.AddSeconds(19800).ToShortDateString();
                lblusername.Text = salesrepNewDt1.Rows[0]["SMName"].ToString(); //Settings.Instance.UserName;
                lblapprby.Text = salesrepDt2.Rows[0]["SMName"].ToString();
            }
        }

        protected void ddlLF_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Date = string.Empty;
            if (calendarTextBox.Text != "")
            {
                decimal dt1 = Convert.ToDecimal(NoOfDays.Text);
                decimal fnum = Math.Floor(dt1);
                decimal lnum = Math.Ceiling(dt1);
                decimal num2 = (fnum + lnum) / 2;
                decimal num = Math.Round(num2, 0, MidpointRounding.AwayFromZero);
                //        if (Convert.ToInt32(NoOfDays.Text) > 1)
                if (num > 1)
                {
                    Reason1.Text = string.Empty;
                    //           DateTime Todate = Convert.ToDateTime(calendarTextBox.Text).AddDays(Convert.ToInt32(NoOfDays.Text) - 1);
                    DateTime Todate = Convert.ToDateTime(calendarTextBox.Text).AddDays(Convert.ToInt32(num) - 1);
                    NoOfDays.Text = num2.ToString();
                    Date = (Todate).ToString("dd/MMM/yyyy");
                    Reason1.Text = Convert.ToString(Date);
                }
                else
                {
                    DateTime Todate = Convert.ToDateTime(calendarTextBox.Text);
                    Date = Todate.ToString("dd/MMM/yyyy");
                    Reason1.Text = Convert.ToString(Date);
                }
            }

            if (calendarTextBox.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select From Date First');", true);
                return;
            }
            if (Convert.ToDecimal(NoOfDays.Text) == 1M)
            {
                if (ddlLF.SelectedItem.Value == "S")
                {
                    ddlLF1.Visible = true;
                    ddlLF1.SelectedValue = "F";
                    ddlLF1.Attributes.Add("disabled", "disabled");
                    DateTime Todate = Convert.ToDateTime(calendarTextBox.Text).AddDays(1);
                    Reason1.Text = (Todate).ToString("dd/MMM/yyyy");
                }
                if (ddlLF.SelectedItem.Value == "C")
                {
                    DateTime Todate = Convert.ToDateTime(calendarTextBox.Text);
                    Reason1.Text = (Todate).ToString("dd/MMM/yyyy");
                    ddlLF1.SelectedValue = "C";
                }
            }
            else if(Convert.ToDecimal(NoOfDays.Text)>1M)
            {
                string str = NoOfDays.Text;
                 string[] str1 = str.Split('.');
                 if (str1.Length == 2)
                 {
                     if(Convert.ToDecimal(str1[1])==5M)
                     {
                         if (ddlLF.SelectedItem.Value == "S")
                         {
                             ddlLF1.SelectedValue = "C";
                             //DateTime Todate = Convert.ToDateTime(Reason1.Value).AddDays(1);
                             //Reason1.Value = (Todate).ToString("dd/MMM/yyyy");
                             ddlLF1.Attributes.Add("disabled", "disabled");
                         }
                         else if (ddlLF.SelectedItem.Value == "C")
                         {
                             ddlLF1.SelectedValue = "F";
                             //DateTime Todate = Convert.ToDateTime(Reason1.Value);
                             //Reason1.Value = (Todate).ToString("dd/MMM/yyyy");
                             ddlLF1.Attributes.Add("disabled", "disabled");
                         }
                     }
                     else
                     {
                         if (ddlLF.SelectedItem.Value == "S")
                         {
                             ddlLF1.SelectedValue = "F";
                             ddlLF1.Attributes.Add("disabled", "disabled");
                             DateTime Todate = Convert.ToDateTime(Reason1.Text).AddDays(1);
                             Reason1.Text = (Todate).ToString("dd/MMM/yyyy");
                         }
                         else if (ddlLF.SelectedItem.Value == "C")
                         {
                             ddlLF1.SelectedValue = "C";
                             ddlLF1.Attributes.Add("disabled", "disabled");
                             DateTime Todate = Convert.ToDateTime(Reason1.Text);
                             Reason1.Text = (Todate).ToString("dd/MMM/yyyy");
                         }
                     }
                 }
                 else
                 {
                     if (ddlLF.SelectedItem.Value == "S")
                     {
                         ddlLF1.SelectedValue = "F";
                         ddlLF1.Attributes.Add("disabled", "disabled");
                         DateTime Todate = Convert.ToDateTime(Reason1.Text).AddDays(1);
                         Reason1.Text = (Todate).ToString("dd/MMM/yyyy");
                     }
                     else if (ddlLF.SelectedItem.Value == "C")
                     {
                         ddlLF1.SelectedValue = "C";
                         ddlLF1.Attributes.Add("disabled", "disabled");
                         DateTime Todate = Convert.ToDateTime(Reason1.Text);
                         Reason1.Text = (Todate).ToString("dd/MMM/yyyy");
                     }
                 }
              
            }
        }

       
    }
}
