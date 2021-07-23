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
    public partial class MeetCheckListMaster : System.Web.UI.Page
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
            }

        }

        private void fillMeetType()
        {//Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetType";
            string query = "select Id,Name from MastMeetType";
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



        private void fillRepeter()
        {
            string str = @"select l.Id,t.Name,l.Narration from [MastMeetCheckList] l left join MastMeetType t on l.MeetTypeId=t.Id";
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
            {//Ankita - 20/may/2016- (For Optimization)
                //string str = @"select * from MastMeetCheckList  where Id=" + ProspectId;
                string str = @"select MeetTypeId,Narration from MastMeetCheckList  where Id=" + ProspectId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";
                    ddlMeetType.SelectedValue = deptValueDt.Rows[0]["MeetTypeId"].ToString();
                   txtNarration.Text = deptValueDt.Rows[0]["Narration"].ToString();
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
            int retsave = dp.InsertCheckLIstMaster(ddlMeetType.SelectedValue,txtNarration.Text);
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
            txtNarration.Text="";
            btnDelete.Visible = false;
        }

        private void UpdateRecord()
        {
            int retsave = dp.UpdateCheckLIstMaster(Convert.ToInt32(ViewState["VisId"]),ddlMeetType.SelectedValue,txtNarration.Text);
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
            Response.Redirect("~/MeetCheckListMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.deleteCheckLIstMaster(Convert.ToString(ViewState["VisId"]));
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





