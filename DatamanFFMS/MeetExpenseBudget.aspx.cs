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
    public partial class MeetExpEntry : System.Web.UI.Page
    {
        BAL.Meet.MeetTypeBAL dp = new BAL.Meet.MeetTypeBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["VisId"] = parameter;
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (!IsPostBack)
            {
                btnDelete.Visible = false;
                fillMeetType();
                fillMeetExpenseType();
                fillAreaType();
            }

        }

        private void fillMeetType()
        {//Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetType";
            string query = "select Id,Name from MastMeetType order by Name ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlMeetType.DataSource = dt;
                ddlMeetType.DataTextField = "Name";
                ddlMeetType.DataValueField = "Id";
                ddlMeetType.DataBind();
            }
            ddlMeetType.Items.Insert(0, new ListItem("-- Select Name --", "0"));
        }

        private void fillMeetExpenseType()
        {//Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetExpence";
            string query = "select Id,Name from MastMeetExpence";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
           
            if (dt.Rows.Count > 0)
            {
                ddlexpenseName.DataSource = dt;
                ddlexpenseName.DataTextField = "Name";
                ddlexpenseName.DataValueField = "Id";
                ddlexpenseName.DataBind();
            }
            ddlexpenseName.Items.Insert(0, new ListItem("-- Select Expense Name --", "0"));
        }
        private void fillAreaType()
        {//Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetAreaType";
            string query = "select Id,Name from MastMeetAreaType";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlAreaType.DataSource = dt;
                ddlAreaType.DataTextField = "Name";
                ddlAreaType.DataValueField = "Id";
                ddlAreaType.DataBind();
            }
            ddlAreaType.Items.Insert(0, new ListItem("-- Select Area Type --", "0"));
        }



        private void fillRepeter()
        {
         string str = @"select  b.Id, Mt.Name as MeetName,Mt.Id as MeetID,a.Name as Area, a.Id as AreaID,ME.Name as ExpenseName, Mt.Id as ExpenseID,b.ApproxCost,b.TotalQty,b.EstimatedCost 
                              from [MastMeetExpBudget] b inner join  MastMeetAreaType a
                             on b.AreaTypeId=a.Id inner join MastMeetType Mt on Mt.Id=b.MeetTypeId
                             inner join MastMeetExpence ME on ME.Id=b.MeetExpId";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }

        private void FillDeptControls(int ProspectId)
        {
            try
            {
                string str = @"select * from MastMeetExpBudget  where Id=" + ProspectId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";
                    ddlMeetType.SelectedValue = deptValueDt.Rows[0]["MeetTypeId"].ToString();
                    ddlAreaType.SelectedValue = deptValueDt.Rows[0]["AreaTypeId"].ToString();
                    ddlexpenseName.SelectedValue = deptValueDt.Rows[0]["MeetExpId"].ToString();
                    txtAPPROXcOST.Text = deptValueDt.Rows[0]["ApproxCost"].ToString();
                    txttotalQty.Text = deptValueDt.Rows[0]["TotalQty"].ToString();
                    txtEstimatedCost.Text = deptValueDt.Rows[0]["EstimatedCost"].ToString();
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateRecord();
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
             txtEstimatedCost.Text=Convert.ToString(Convert.ToDecimal(txtAPPROXcOST.Text) * Convert.ToDecimal(txttotalQty.Text));
             decimal co = 0;
            if(txtEstimatedCost.Text!="")
            {
                co =Convert.ToDecimal(txtEstimatedCost.Text);
            }

            int retsave = dp.InsertBudgetMaster(Convert.ToInt32(ddlMeetType.SelectedValue), Convert.ToInt32(ddlAreaType.SelectedValue), Convert.ToInt32(ddlexpenseName.SelectedValue), Convert.ToDecimal(txtAPPROXcOST.Text), Convert.ToDecimal(txttotalQty.Text), Convert.ToDecimal(co));
            if (retsave != 0)
            {
                ClearControls();
                btnDelete.Visible = false;
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "success", "Successmessage('Record Inserted Successfully');", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
            }
        }
        private void ClearControls()
        {
            ddlMeetType.SelectedIndex = 0;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            ddlAreaType.SelectedIndex = 0;
            ddlexpenseName.SelectedIndex = 0;
            txtAPPROXcOST.Text = "";
            txtEstimatedCost.Text = "";
            txttotalQty.Text = "";
            btnDelete.Visible = false;
        }
        private void UpdateRecord()
        {
            txtEstimatedCost.Text = Convert.ToString(Convert.ToDecimal(txtAPPROXcOST.Text) * Convert.ToDecimal(txttotalQty.Text));
            decimal co = 0;
            if (txtEstimatedCost.Text != "")
            {
                co = Convert.ToDecimal(txtEstimatedCost.Text);
            }
            int retsave = dp.UpdatBudgetMaster(Convert.ToInt32(ViewState["VisId"]), Convert.ToInt32(ddlMeetType.SelectedValue), Convert.ToInt32(ddlAreaType.SelectedValue), Convert.ToInt32(ddlexpenseName.SelectedValue), Convert.ToDecimal(txtAPPROXcOST.Text), Convert.ToDecimal(txttotalQty.Text), Convert.ToDecimal(co));
            if (retsave == 1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                btnDelete.Visible = false;
                btnSave.Text = "Save";
                ClearControls();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Update');", true);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetExpEntry.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.deleteBudgetMaster(Convert.ToString(ViewState["VisId"]));
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
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillRepeter();
        }
    }
}





