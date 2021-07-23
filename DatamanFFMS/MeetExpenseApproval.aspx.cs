using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using DAL;
using System.IO;

namespace AstralFFMS
{
    public partial class MeetExpenseApproval : System.Web.UI.Page
    {
        string parameter = "";
        string roleType = "";
        string sr = ""; int isExist = 0;
        BAL.Meet.MeetExpenseMaster ME = new BAL.Meet.MeetExpenseMaster();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtmeetdate.Attributes.Add("readonly", "readonly");
            sr = "select Count(*) from [MastMeetLogin] where userId in(" + Convert.ToInt32(Settings.Instance.UserID) + ")";
            isExist = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));

            //if (txtFinalApproveamount.Text != "")
            //{
            //    string Venue = txtFinalApproveamount.Text;
            //    txtFinalApproveamount.Attributes.Add("value", Venue);
            //}

            if (txtFinalApproveremark.Text != "")
            {
                string Comments = txtFinalApproveremark.Text;
                txtFinalApproveremark.Attributes.Add("value", Comments);
            }

            if (Request.QueryString["MeetPlanId"] != null)
            {
                parameter = (Request.QueryString["MeetPlanId"]);
                Button1.Visible = false;
            }
            else
            {
                //parameter = Request["__EVENTARGUMENT"];
                if (MeetPlanId.Value != "") parameter = MeetPlanId.Value;
                else  parameter ="0";
            }
            if (parameter != "")
            {
                ViewState["CollId"] = parameter;
              
                mainDiv.Style.Add("display", "none");
                rptmain.Style.Add("display", "block");
            }
        
            //if (parameter!= null)
            //{
            //    if (parameter != "")
            //    {
            //        FillDeptControls(Convert.ToInt32(parameter));
            //    }
            //}
           // if (parameter != null) FillDeptControls(Convert.ToInt32(parameter));
            if (!IsPostBack)
            {
                if (parameter != null)
                {
                    if (parameter != "")
                    {
                        FillDeptControls(Convert.ToInt32(parameter));
                    }
                }
                rptmain.Style.Add("display", "block");
                mainDiv.Style.Add("display", "none");
               // fillInitialRecords();
                GetRoleType(Settings.Instance.RoleID);
                fillUnderUsers();
                fillMeetType();
               // this.fillGrid(parameter);

                this.fillGrid(Convert.ToInt32(parameter));
                ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        private void fillMeetType()
        {
            string query = "select * from MastMeetType order by Name ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillUnderUsers()
        {
            if (roleType == "CityHead" || roleType == "DistrictHead")
            {
                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                if (d.Rows.Count > 0)
                {
                    try
                    {
                        DataView dv = new DataView(d);
                        dv.RowFilter = "RoleType='AreaIncharge'";
                        ddlunderUser.DataSource = dv;
                        ddlunderUser.DataTextField = "SMName";
                        ddlunderUser.DataValueField = "SMId";
                        ddlunderUser.DataBind();
                    }
                    catch { }

                }
            }
            else if (roleType == "StateHead" || roleType == "RegionHead")
            {
                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(d);
                dv.RowFilter = "RoleType='CityHead' OR RoleType='DistrictHead' or RoleType='AreaIncharge'";
                ddlunderUser.DataSource = dv;
                ddlunderUser.DataTextField = "SMName";
                ddlunderUser.DataValueField = "SMId";
                ddlunderUser.DataBind();

            }
            else
            {
                string str1 = @"select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in(select MastSalesRep.SmId from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastRole.RoleType in ('AreaIncharge','CityHead','DistrictHead','RegionHead','StateHead'))"; 
                //DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
               DataTable  d = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                DataView dv = new DataView(d);
              //  dv.RowFilter = "RoleType='CityHead' OR RoleType='DistrictHead' or RoleType='AreaIncharge' or RoleType='RegionHead' or RoleType='StateHead' ";
                ddlunderUser.DataSource = dv;
                ddlunderUser.DataTextField = "SMName";
                ddlunderUser.DataValueField = "SMId";
                ddlunderUser.DataBind();
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

        private void FillDeptControls(int id)
        {
            string str = @"select p.*,e.FinalApprovedAmount,e.FinalApprovedRemark,p. MeetPlanId,e.ExpenseApprovedRemark,e.ExpenseApprovedAmount,e.CCEmailID,e.Status  from TransMeetPlanEntry  p  inner join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]   where AppStatus='Approved'  and p.MeetPlanId=" + id + " order by p. MeetPlanId desc";
            DataTable dtP = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dtP.Rows.Count > 0)
            {
                try
                {
                    txtapprovalremark.Text = dtP.Rows[0]["AppRemark"].ToString();
                    txtapprovedBudget.Text = dtP.Rows[0]["AppAmount"].ToString();
                    DateTime todt = DateTime.Parse(dtP.Rows[0]["Appdate"].ToString());
                    txtapprovedDate.Text = todt.ToString("dd/MMM/yyyy");
                    txtapproxBudget.Text = dtP.Rows[0]["LambBudget"].ToString();
                    DateTime todt1 = DateTime.Parse(dtP.Rows[0]["MeetDate"].ToString());
                    txtmeetdate.Text = todt1.ToString("dd/MMM/yyyy");
                    txtfinalRemark.Text = dtP.Rows[0]["ExpenseApprovedRemark"].ToString();
                    txtfinalbudget.Text = dtP.Rows[0]["ExpenseApprovedAmount"].ToString();
                  
                    txtFinalApproveremark.Text = dtP.Rows[0]["FinalApprovedRemark"].ToString();
                    txtFinalApproveamount.Text = dtP.Rows[0]["FinalApprovedAmount"].ToString();
                   
               //     ddlmeet.SelectedValue = dtP.Rows[0]["MeetPlanId"].ToString();
                   // txtemail.Text = dtP.Rows[0]["CCEmailID"].ToString();
                    ddlmeet.Enabled = false;

                    fillMeetType();
                    ddlmeetType.SelectedValue = dtP.Rows[0]["MeetTypeId"].ToString();
                    this.fillUnderUsers();
                    ddlunderUser.SelectedValue = dtP.Rows[0]["SMId"].ToString();
                    fillInitialRecords();

                    ddlmeet.SelectedValue = dtP.Rows[0]["MeetPlanId"].ToString();

                    btnSave.Text = "Update";
                    //Added on 19-12-2015
                    if (dtP.Rows[0]["Status"].ToString() == "Approved")
                    {

                        btnSave.Enabled = false;
                        btnSave.CssClass = "btn btn-primary disableonclick";
                    }
                    //if (dtP.Rows[0]["AppStatus"].ToString() == "Approved")
                    //{
                    //    btnSave.Enabled = false;
                    //    btnSave.CssClass = "btn btn-primary";
                    //}
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "btn btn-primary disableonclick";
                    }
                    //End
                    checkMeetAdmin();
                    ddlunderUser.Enabled = false;
                    ddlmeetType.Enabled = false;
                    ddlmeet.Enabled = false;
                }
                catch { }
            }
        }

        private void fillInitialRecords()
        {
            ddlmeet.Items.Clear();
            string str = @"select * from TransMeetPlanEntry P inner join [TransMeetExpense] E on P.[MeetPlanId]=E.[MeetPlanId]  where P.AppStatus='Approved' and P.SMId=" + ddlunderUser.SelectedValue + " and P.MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + "  order by MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlmeet.DataSource = dt;
                ddlmeet.DataTextField = "MeetName";
                ddlmeet.DataValueField = "MeetPlanId";
                ddlmeet.DataBind();
            }
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void LoadRecords(int TMeetExpId)
        {
            string str = @"select * from TransMeetExpense P inner join [TransMeetExpense] E on P.[MeetPlanId]=E.[MeetPlanId]  where P.AppStatus='Approved' and P.SMId=" + ddlunderUser.SelectedValue + " and P.MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + "  order by MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
            }

        }
        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlmeetType.SelectedIndex = 0;
            ddlmeet.Items.Clear();
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillInitialRecords();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    InsertRecord();
                }
                else
                {
                    InsertRecord();
                }
                btnSave.CssClass.Replace("disableonclickcls", "");
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private void InsertRecord()
        {
            try
            {
                string s = "update TransMeetExpense set FinalApprovedAmount=" + Settings.DMDecimal(txtFinalApproveamount.Text) + ",FinalApprovedRemark='" + txtFinalApproveremark.Text + "',Status='Approved' where MeetPlanId="+ddlmeet.SelectedValue+"";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s);
                
               
                this.InsertNotification();
                if (btnSave.Text == "Save")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record inserted Successfully');", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                }
                clearcontrol();
                ddlunderUser.Enabled = true;
              //  this.fillGrid(ddlmeet.SelectedValue);
            }
            catch
            {
                if (btnSave.Text == "Save")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
                   
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while updating the records');", true);
                }
            }
        }

        private void InsertNotification()
        {
            try
            {
                string strRetrun = "select TMeetExpId from [dbo].[TransMeetExpense] Where  MeetPlanId=" + ddlmeet.SelectedValue + "";
                int TMeetExpId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strRetrun));

                string salesRepqueryNew = @"select UnderId,SMName from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);

                string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);

                string getSMId = @"select SMId from TransMeetExpense r where r.TMeetExpId=" + Convert.ToInt32(TMeetExpId) + " ";
                DataTable USMID = DbConnectionDAL.GetDataTable(CommandType.Text, getSMId);

                string salesRepqueryNew1 = "";
                if (salesRepqueryNewdt.Rows.Count > 0)
                {
                    salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                }

                string getSeniorSMId = @"select UnderId,UserId,SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(USMID.Rows[0]["SMId"]) + "";
                DataTable salesRepqryForSManNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId);
                int senSMid = 0; string seniorName = string.Empty;
                if (salesRepqryForSManNewdt.Rows.Count > 0)
                {
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

                string msgUrl = "MeetExpenseApproval.aspx?SMId=" + senSMid + "&TMeetExpId=" + TMeetExpId + "&MeetPlanId=" + ddlmeet.SelectedValue;
                string meetstr = "select MeetName from TransMeetPlanEntry where MeetPlanId=" + ddlmeet.SelectedValue + "";
                string MeetName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, meetstr));
                //Check Is Senior

                int smiDDDl = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
                //string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                //DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);
                int IsSenior = 0, ISSenSmId = 0;
                if (smIDNewDDlQryDT.Rows.Count > 0)
                {
                    ISSenSmId = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
                }
                //string updateLeaveStatusQry = @"update TransMeetPlanEntry set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ",AppRemark='Approved By Senior' where MeetPlanId='" + txtmeetId.Text + "' ";
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateLeaveStatusQry);

               // if (ddlApp.Text == "Approved")

                ME.InsertTransNotification("MEETEXPENSEAPPROVED", Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]), newDate12, msgUrl, "Meet Expense Approved By - " + salesRepqueryNewdt.Rows[0]["SMName"] + " " + " " + "MeetName-" + MeetName + " ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(USMID.Rows[0]["SMId"]));
                //else ME.InsertTransNotification("MEETREJECTED", Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]), newDate12, msgUrl, "Meet  Rejected By - " + salesRepqueryNewdt.Rows[0]["SMName"], 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(USMID.Rows[0]["SMId"]));

                string msgUrl1 = "MeetExpenseApproval.aspx?SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]) + "&TMeetExpId=" + Convert.ToInt64(TMeetExpId) + "&MeetPlanId=" + ddlmeet.SelectedValue;
               
                int salesRepID = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]);
                int UserId = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]);
                //

                string senSMNameQryL3 = @"select UnderId from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                int UnderID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQryL3));

                string senQryL3 = @"select SMId from MastSalesRep where SMId=" + UnderID + "";
                int SMIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senQryL3));

                //string senUserQryL3 = @"select UserId from MastSalesRep where SMId=" + SMIDL3 + "";
                //int UserIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL3));

                string senUserQryL3 = @"select UserId from MastSalesRep where SMId=" + senSMid + "";
                int UserIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL3));

               // if (ddlApp.Text == "Approved") 
                ME.InsertTransNotificationL3("MEETEXPENSEAPPROVED", UserIDL3, newDate12, msgUrl1, "Meet Expense Approved By - " + salesRepqueryNewdt.Rows[0]["SMName"] + " " + " " + "MeetName-" + MeetName + " ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), senSMid, Convert.ToInt32(salesRepID));
                //else ME.InsertTransNotificationL3("MEETREJECTED", UserIDL3, newDate12, msgUrl1, "Meet Rejected By - " + salesRepqueryNewdt.Rows[0]["SMName"], 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), senSMid, Convert.ToInt32(salesRepID));
            }
            catch { }
        }
        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
        private void ClearControls()
        {
            txtapprovalremark.Text = "";
            txtapprovedBudget.Text = "";
            txtapprovedDate.Text = "";
            txtapproxBudget.Text = "";
            txtmeetdate.Text = "";
            txtfinalRemark.Text = "";
            txtfinalbudget.Text = "";
            txtFinalApproveremark.Text = "";
            txtFinalApproveamount.Text = "";
            ddlmeet.SelectedValue = "";
            ddlunderUser.SelectedValue = "";
            ddlunderUser.Items.Clear();
          //  txtemail.Text = "";
            ddlmeetType.SelectedIndex = 0;
            ddlmeet.Items.Clear();
          //  txtemail.Text = "";
            txtapproxBudget.Text = "";
            txtfinalbudget.Text = "";
            txtfinalRemark.Text = "";
            
            ddlunderUser.Enabled = true;
            ddlmeetType.Enabled = true;
            ddlmeet.Enabled = true;
            btnSave.Text = "Save";
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void UpdateRecord()
        {
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetExpenseApproval.aspx");
        }

        private void clearcontrol()
        {
            txtapprovalremark.Text = "";
            txtfinalbudget.Text = "";
            txtfinalbudget.Text = null;
            txtapprovedBudget.Text = "";
            txtapprovedDate.Text = "";
            txtapproxBudget.Text = "";
            txtmeetdate.Text = "";
            txtFinalApproveamount.Text = "";
            txtFinalApproveremark.Text = "";
            txtfinalRemark.Text = "";
            txtfinalRemark.Text = null;
            ddlmeet.Enabled = true;
            fillInitialRecords();
            ddlmeet.SelectedIndex = 0;
            ddlmeet.Items.Clear();
            ddlmeetType.SelectedIndex = 0;
            ddlunderUser.SelectedIndex = 0;
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        protected void ddlmeet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlmeet.SelectedIndex > 0)
                {
                    string str = @"select p.*,e.ExpenseApprovedAmount,e.ExpenseApprovedRemark,e.FinalApprovedRemark,e.FinalApprovedAmount,e.CCEmailID  from TransMeetPlanEntry p  left join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]
                            where  p.MeetPlanId=" + ddlmeet.SelectedValue;
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt.Rows.Count > 0)
                    {
                        txtapprovalremark.Text = dt.Rows[0]["AppRemark"].ToString();
                        txtapprovedBudget.Text = dt.Rows[0]["AppAmount"].ToString();
                        DateTime todt = DateTime.Parse(dt.Rows[0]["Appdate"].ToString());
                        txtapprovedDate.Text = todt.ToString("dd/MMM/yyyy");
                        txtapproxBudget.Text = dt.Rows[0]["LambBudget"].ToString();
                        DateTime todt1 = DateTime.Parse(dt.Rows[0]["MeetDate"].ToString());
                        txtmeetdate.Text = todt1.ToString("dd/MMM/yyyy");
                        txtfinalRemark.Text = dt.Rows[0]["ExpenseApprovedRemark"].ToString();
                        txtfinalbudget.Text = dt.Rows[0]["ExpenseApprovedAmount"].ToString();

                        txtFinalApproveremark.Text = dt.Rows[0]["FinalApprovedRemark"].ToString();
                        txtFinalApproveamount.Text = dt.Rows[0]["FinalApprovedAmount"].ToString();
                       // txtemail.Text = dt.Rows[0]["CCEmailID"].ToString();
                        this.checkMeetAdmin();
                    }
                    else
                    {
                        clearcontrol();
                    }

                }
                else
                {
                    clearcontrol();
                }
            }
            catch
            {
            }
        }
        private void checkMeetAdmin()
        {
            try
            {
                string pageName12 = Path.GetFileName(Request.Path);
                string PermAll = Settings.Instance.CheckPagePermissions(pageName12, Convert.ToString(Session["user_name"]));
                string[] SplitPerm = PermAll.Split(',');

                if (btnSave.Text == "Save")
                {
                    //  btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName12, Convert.ToString(Session["user_name"]));
                    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                    btnSave.CssClass = "btn btn-primary disableonclick";
                }
                else
                {
                    //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName12, Convert.ToString(Session["user_name"]));
                    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                    btnSave.CssClass = "btn btn-primary disableonclick";
                }

                string sr = "select UnderId from mastsalesrep where SMID In(Select SMId from TransMeetPlanEntry where [MeetPlanId]=" + Settings.DMInt32(ddlmeet.SelectedValue) + ")";
                int pUserId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));

                sr = "select Count(*) from [MastMeetLogin] where userId in(" + Convert.ToInt32(Settings.Instance.UserID) + ")";
                int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));

                //if (pUserId == Convert.ToInt32(Settings.Instance.SMID)) btnUpdate.Visible = true;
                if (cnt > 0)
                {
                    sr = "select Count(*) from [TransMeetExpense] where MeetPlanId=" + Settings.DMInt32(ddlmeet.SelectedValue) + " And Status='Approved' ";
                    int statuscnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));
                    if (statuscnt == 0){ btnSave.Enabled = true;
                    btnSave.CssClass = "btn btn-primary disableonclick"; btnSave.Visible = true;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Meet expense already approved');", true);
                        btnSave.Enabled = false;
                        btnSave.CssClass = "btn btn-primary disableonclick";
                    }
                }
                else {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "btn btn-primary disableonclick";
                   // btnSave.Visible = false;
                }
            }
            catch { }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {

            string str = @"select p.*,e.FinalApprovedAmount,e.FinalApprovedRemark,e.CCEmailID,msp.SMName, case e.FinalApprovedRemark when '' then 'Pending' else 'Approved' end as EStatus  from TransMeetPlanEntry  p  inner join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId] inner join [MastSalesRep] msp on msp.[SMId]= e.[SMId]   where AppStatus='Approved'  and e.SMId=" + ddlunderUser.SelectedValue + " order by MeetPlanId desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else {
                rpt.DataSource = null;
                rpt.DataBind();
            }
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        public void fillGrid(int id)
        {
            string str="";
            string str1 = "";
            if (isExist > 0)
            {
                str1 = "select MastSalesRep.SmId from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastRole.RoleType in ('AreaIncharge','CityHead','DistrictHead','RegionHead','StateHead')";
            }
            else
            {
                str1 = @"select MastSalesRep.SmId from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level> (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
            }

          
            DataTable d = new DataTable();
            d = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            //  DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            string sm = "";
            for (int i = 0; i < d.Rows.Count; i++)
            {
                sm += d.Rows[i]["SmId"].ToString() + ",";
            }
            string sm1 = sm.TrimStart(',').TrimEnd(',');

            if (id != 0) str = @"select p.*,e.FinalApprovedAmount,e.FinalApprovedRemark,e.CCEmailID,msp.SMName, e.Status as EStatus,mp.PartyName + '-' + [city].areaName + '-' +[state].areaName  as Distributor  from TransMeetPlanEntry  p  inner join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId] inner join [MastSalesRep] msp on msp.[SMId]= e.[SMId]    Left Join MastParty mp on p.DistId=mp.PartyId left join mastarea as [city] on [city].areaid=mp.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
  where AppStatus='Approved'  and p.MeetPlanId=" + Convert.ToInt32(id) + "  order by MeetPlanId desc";
            else str = @"select p.*,e.FinalApprovedAmount,e.FinalApprovedRemark,e.CCEmailID,msp.SMName, e.Status as EStatus,mp.PartyName + '-' + [city].areaName + '-' +[state].areaName  as Distributor  from TransMeetPlanEntry  p  inner join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId] inner join [MastSalesRep] msp on msp.[SMId]= e.[SMId]    Left Join MastParty mp on p.DistId=mp.PartyId left join mastarea as [city] on [city].areaid=mp.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
  where  msp.[SMId] In (" + sm1 + ") And AppStatus='Approved' and e.Status !='Approved'  order by MeetPlanId desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
            
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void custombutton_Click(object sender, EventArgs e)
        {
            var y = Convert.ToInt32(customval.Value);
            this.FillDeptControls(y);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
    }
}




