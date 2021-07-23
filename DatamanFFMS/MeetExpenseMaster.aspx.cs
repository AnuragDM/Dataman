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

namespace AstralFFMS
{
    public partial class MeetExpenseMaster : System.Web.UI.Page
    {
        BAL.Meet.MeetExpenseMaster dp = new BAL.Meet.MeetExpenseMaster();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
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
            }

        }

        private void fillRepeter()
        {
            string str = @"  select * from MastMeetExpence";
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
            { //Ankita - 20/may/2016- (For Optimization)
               // string str = @"select * from MastMeetExpence  where Id=" + ProspectId;
                string str = @"select Name from MastMeetExpence  where Id=" + ProspectId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";
                    txtName.Text = deptValueDt.Rows[0]["Name"].ToString();


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
                    if (checkName1() == 0)
                    {
                        UpdateRecord();
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                    }
                }
                else
                {
                    if (checkName() == 0)
                    {
                        InsertRecord();

                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private int checkName()
        {
            string s = "select count(*) from MastMeetExpence where Name='" + txtName.Text + "'";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
            return exists;

        }
        private int checkName1()
        {
            string s = "select count(*) from MastMeetExpence where Name='" + txtName.Text + "' and Id !='" + Convert.ToInt32(ViewState["VisId"]) + "'";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
            return exists;

        }

        private void InsertRecord()
        {
            int retsave = dp.Insert(txtName.Text);
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
            txtName.Text = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";

            btnDelete.Visible = false;
        }

        private void UpdateRecord()
        {
            int retsave = dp.Update(Convert.ToInt32(ViewState["VisId"]), txtName.Text);
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
            Response.Redirect("~/MeetExpenseMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Convert.ToString(ViewState["VisId"]));
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



