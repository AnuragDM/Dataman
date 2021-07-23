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
    public partial class MeetExpense : System.Web.UI.Page
    {
        BAL.Meet.MeetTypeBAL dp = new BAL.Meet.MeetTypeBAL();
        BAL.Meet.MeetExpenseMaster ME = new BAL.Meet.MeetExpenseMaster();
        string parameter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            txtmeetdate.Attributes.Add("readonly", "readonly");
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
               // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";

            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["VisId"] = parameter;
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
                fillRecord(Convert.ToInt32(ViewState["VisId"]));
               
            }
            if (!IsPostBack)
            {
                fillUnderUsers();
                fillMeetType();
                ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
                btnDelete.Enabled = false;
               // fillInitialRecords();
            }

        }

        private void fillRecord(int ExpenseID)
        {
            try
            {
                string str = @"select p.*,e.ExpenseApprovedAmount,e.ExpenseApprovedRemark,e.Status from TransMeetPlanEntry p  left join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]
                            where  e.TMeetExpId=" + ExpenseID;
                DataTable dtGA = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtGA.Rows.Count > 0)
                {
                    txtapprovalremark.Text = dtGA.Rows[0]["AppRemark"].ToString();
                    txtapprovedBudget.Text = dtGA.Rows[0]["AppAmount"].ToString();
                    DateTime todt = DateTime.Parse(dtGA.Rows[0]["Appdate"].ToString());
                    txtapprovedDate.Text = todt.ToString("dd/MMM/yyyy");

                    //=string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["Appdate"].ToString());
                    txtapproxBudget.Text = dtGA.Rows[0]["LambBudget"].ToString();

                    DateTime todt1 = DateTime.Parse(dtGA.Rows[0]["MeetDate"].ToString());
                    txtmeetdate.Text = todt1.ToString("dd/MMM/yyyy");
                    //                        txtmeetdate.Text = dt.Rows[0]["MeetDate"].ToString();
                    txtfinalRemark.Text = dtGA.Rows[0]["ExpenseApprovedRemark"].ToString();
                    txtfinalbudget.Text = dtGA.Rows[0]["ExpenseApprovedAmount"].ToString();

                    fillMeetType();
                    ddlmeetType.SelectedValue = dtGA.Rows[0]["MeetTypeId"].ToString();

                    fillInitialRecords();

                    ddlmeet.SelectedValue = dtGA.Rows[0]["MeetPlanId"].ToString();

                    btnSave.Text = "Update";
                    //Added on 19-12-2015
                    if (dtGA.Rows[0]["Status"].ToString() != "Pending")
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "btn btn-primary";
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "btn btn-primary";
                        btnDelete.Enabled = true;
                    }

                    string pageName = Path.GetFileName(Request.Path);
                    string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
                    string[] SplitPerm = PermAll.Split(',');

                    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                    btnSave.CssClass = "btn btn-primary";
                    btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
                    btnDelete.CssClass = "btn btn-primary";
                    //End
                  
                    ddlunderUser.Enabled = false;
                    ddlmeetType.Enabled = false;
                    ddlmeet.Enabled = false;
                    btnDelete.Visible = true;
                  
                }
            }
            catch
            { }

        }

        //private void fillUnderUsers()
        //{
        //    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
        //    if (d.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            DataView dv = new DataView(d);
        //            dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead'";
        //            ddlunderUser.DataSource = dv;
        //            ddlunderUser.DataTextField = "SMName";
        //            ddlunderUser.DataValueField = "SMId";
        //            ddlunderUser.DataBind();
        //        }
        //        catch { }

        //    }
        //}
        private void fillUnderUsers()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    ddlunderUser.SelectedValue = Settings.Instance.SMID;
                }
                catch { }

            }
        }

        private void fillInitialRecords()
        { //Ankita - 20/may/2016- (For Optimization)
            ddlmeet.Items.Clear();            
            //string str = @"select MeetPlanId,MeetName from TransMeetPlanEntry  where AppStatus='Approved' and SMId=" + ddlunderUser.SelectedValue + " and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + "  order by MeetDate desc";
            // Nishu 25/08/2016    meet expense check
            string str = @"select mp.MeetPlanId,mp.MeetName from TransMeetPlanEntry mp LEFT JOIN TransMeetExpense me  ON mp.MeetPlanId = me.MeetPlanId where mp.AppStatus='Approved' and mp.SMId=" + ddlunderUser.SelectedValue + " and mp.MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " AND me.FinalApprovedAmount IS null  order by mp.MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if(dt.Rows.Count>0)
            {
                ddlmeet.DataSource = dt;
                ddlmeet.DataTextField = "MeetName";
                ddlmeet.DataValueField = "MeetPlanId";
                ddlmeet.DataBind();
            }
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
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
                string s1 = "delete from TransMeetExpense where MeetPlanId="+ddlmeet.SelectedValue+"";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s1);

                string s = "insert into TransMeetExpense([MeetPlanId],[UserId] ,SMId ,ExpenseApprovedAmount ,ExpenseApprovedRemark,Status) values('" + Settings.DMInt32(ddlmeet.SelectedValue) + "','" + Settings.DMInt32(Settings.Instance.UserID) + "','" + Settings.DMInt32(ddlunderUser.SelectedValue) + "','" + Settings.DMDecimal(txtfinalbudget.Text) + "','" + txtfinalRemark.Text + "','Pending')";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s);

                string strRetrun = "select Top 1(TMeetExpId)   from [dbo].[TransMeetExpense] order by TMeetExpId desc ";
                int TMeetExpId =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strRetrun));
                if (btnSave.Text == "Save") this.InsertMeetRequest(TMeetExpId);
               
                if (btnSave.Text == "Save")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record inserted Successfully');", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                }
                clearcontrol();
            }
            catch
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
        private void InsertMeetRequest(int RetSave)
        {
            try
            {
                //Int64 retsave = lvAll.Insert(docId, Convert.ToInt32(Settings.Instance.UserID), newDate, Convert.ToDecimal(NoOfDays.Text), Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text), Reason.Value, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), 0, strLF, leaveString);

                string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);


                string salesRepqueryNew1 = "";
                if (salesRepqueryNewdt.Rows.Count > 0)
                {
                    salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                }

                string getSeniorSMId = @"select UnderId,UserId,SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(ddlunderUser.SelectedValue) + "";
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

                string msgUrl = "MeetExpenseApproval.aspx?SMId=" + ddlunderUser.SelectedValue + "&TMeetExpId=" + RetSave+"&MeetPlanId="+ddlmeet.SelectedValue;

                //Check Is Senior

                int smiDDDl = Convert.ToInt32(ddlunderUser.SelectedValue);
                string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);
                int IsSenior = 0, ISSenSmId = 0;
                if (smIDNewDDlQryDT.Rows.Count > 0)
                {
                    ISSenSmId = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
                }
                string meetAdmin = @"select UserId, SMId from MastMeetLogin Where Active=1";
                DataTable dtMeetAdmin = DbConnectionDAL.GetDataTable(CommandType.Text, meetAdmin);
                if (dtMeetAdmin.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMeetAdmin.Rows)
                    {
                        if (dr["SMId"] != null) ME.InsertTransNotification("MEETEXPENSEREQUEST", Convert.ToInt32(dr["UserId"]), newDate12, msgUrl, "Meet Expense Request By - " + salesRepqryForSManNewdt.Rows[0]["SMName"] + " " + ",Meet Name -" + ddlmeet.SelectedItem.Text , 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlunderUser.SelectedValue), Convert.ToInt32(dr["SMId"]));
                        else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Sales Id not found');", true);
                    }
                }

                ME.InsertTransNotification("MEETEXPENSEREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), newDate12, msgUrl, "Meet Expense Request By - " + salesRepqryForSManNewdt.Rows[0]["SMName"] + " " + ",Meet Name -" + ddlmeet.SelectedItem.Text, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlunderUser.SelectedValue), senSMid);
               
            }
            catch { }
        }

        private void ClearControls()
        {
            btnSave.Text = "Save";
        }
        private void UpdateRecord()
        {
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetExpenseEntry.aspx");
        }

        private void clearcontrol()
        {
            txtfinalRemark.Text = "";
            txtapprovalremark.Text = "";
            txtapprovedBudget.Text = "";
            txtapprovedDate.Text = "";
            txtapproxBudget.Text = "";
            txtmeetdate.Text = "";
            ddlmeet.SelectedIndex = 0;
            txtfinalbudget.Text = "";
            txtapprovalremark.Text = "";
            ddlunderUser.Enabled = true;
            ddlmeet.Enabled = true;
            ddlmeetType.Enabled = true;
            ddlmeetType.SelectedIndex = 0;
            ddlmeet.Items.Clear();
            ddlunderUser.SelectedIndex = 0;
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            btnSave.Text = "Save";

        }
        protected void ddlmeet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlmeet.SelectedIndex > 0)
                {
                    string str = @"select p.*,e.ExpenseApprovedAmount,e.ExpenseApprovedRemark from TransMeetPlanEntry p  left join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]
                            where  p.MeetPlanId=" + ddlmeet.SelectedValue;
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt.Rows.Count > 0)
                    {
                        txtapprovalremark.Text = dt.Rows[0]["AppRemark"].ToString();
                        txtapprovedBudget.Text = dt.Rows[0]["AppAmount"].ToString();

                        DateTime todt = DateTime.Parse(dt.Rows[0]["Appdate"].ToString());
                        txtapprovedDate.Text = todt.ToString("dd/MMM/yyyy");

                        //=string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["Appdate"].ToString());
                        txtapproxBudget.Text = dt.Rows[0]["LambBudget"].ToString();

                        DateTime todt1 = DateTime.Parse(dt.Rows[0]["MeetDate"].ToString());
                        txtmeetdate.Text = todt1.ToString("dd/MMM/yyyy");
//                        txtmeetdate.Text = dt.Rows[0]["MeetDate"].ToString();
                        txtfinalRemark.Text = dt.Rows[0]["ExpenseApprovedRemark"].ToString();
                        txtfinalbudget.Text = dt.Rows[0]["ExpenseApprovedAmount"].ToString();
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
            catch { 
            }
        }

        private void fillMeetType()
        { //Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetType order by Name ";
            string query = "select Id,Name from MastMeetType order by Name ";
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

        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
                fillInitialRecords();

        }

        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlmeetType.SelectedIndex = 0;
            ddlmeet.Items.Clear();
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            txtfinalRemark.Text = "";
            txtapprovalremark.Text = "";
            txtapprovedBudget.Text = "";
            txtapprovedDate.Text = "";
            txtapproxBudget.Text = "";
            txtmeetdate.Text = "";
            ddlmeet.SelectedIndex = 0;
            txtfinalbudget.Text = "";
            txtapprovalremark.Text = "";
            ddlunderUser.Enabled = true;
            ddlmeet.Enabled = true;
            ddlmeetType.Enabled = true;
            ddlmeetType.SelectedIndex = 0;
            btnSave.Text = "Save";
      
            
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");

        }

        private void fillgrid()
        {
            string str = string.Empty;
//            string str = @"select p.*,e.ExpenseApprovedAmount,e.ExpenseApprovedRemark,e.TMeetExpId from TransMeetPlanEntry p  inner join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]
//                          where  p.SMId=" + ddlunderUser.SelectedValue + " order by p.MeetDate desc";

            if (Settings.Instance.RoleType == "Admin")
            {
                str = @"select p.*,e.ExpenseApprovedAmount,e.ExpenseApprovedRemark,e.TMeetExpId, e.Status as EStatus from TransMeetPlanEntry p  inner join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]
                         order by p.MeetDate desc";
            }
            else
            {
                str = @"select p.*,e.ExpenseApprovedAmount,e.ExpenseApprovedRemark,e.TMeetExpId, e.Status as EStatus from TransMeetPlanEntry p  inner join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]
                         where  p.SMId=" + ddlunderUser.SelectedValue + " order by p.MeetDate desc";
            }
           
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

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillgrid();
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                string str = @"select p.*,e.ExpenseApprovedAmount,e.ExpenseApprovedRemark from TransMeetPlanEntry p  left join [TransMeetExpense] e on e.[MeetPlanId]= p.[MeetPlanId]
                            where e.Status='Approved' and e.TMeetExpId=" + Convert.ToString(ViewState["VisId"]);
                DataTable dtGA = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtGA.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Expense Entry is Approved');", true);
                }
                else
                {
                    string delQry = @"delete from TransMeetExpense where TMeetExpId=" + Convert.ToString(ViewState["VisId"]);
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, delQry);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                }

            }
            else
            {
                //      this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            
            }
        }
    }
}





