using DAL;
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
using System.IO;
namespace AstralFFMS
{
    public partial class GradeMast : System.Web.UI.Page
    {
        GradeBAL GB = new GradeBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnSave.UniqueID;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["Id"] = parameter;
                FillGradeControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
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
              //  btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
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
            if (!IsPostBack)
            {
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillGradeControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }
        private void fillRepeter()
        {
            string str = @"select *,case Active when 1 then 'Yes' else 'No' end as Active1 from MastGrade order by Name";
            DataTable Locdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = Locdt;
            rpt.DataBind();

            Locdt.Dispose();
        }
        private void FillGradeControls(int Id)
        {
            try
            {
                string GradeQry = @"select * from MastGrade where Id=" + Id;
                DataTable GradeValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, GradeQry);
                if (GradeValueDt.Rows.Count > 0)
                {
                    Grade.Value = GradeValueDt.Rows[0]["Name"].ToString();
                    SyncId.Value = GradeValueDt.Rows[0]["SyncId"].ToString();
                    ImprestLimit.Value = GradeValueDt.Rows[0]["ImprestLimit"].ToString();
                    Remarks.Value = GradeValueDt.Rows[0]["Remarks"].ToString();
                    if (Convert.ToBoolean(GradeValueDt.Rows[0]["Active"]) == true)
                    {
                        chkIsActive.Checked = true;
                    }
                    else
                    {
                        chkIsActive.Checked = false;
                    }
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }

                GradeValueDt.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private void InsertGrade()
        {
            bool active = false;
            if (chkIsActive.Checked)
                active = true;
            int retval = GB.InsertUpdateGrade(0, Grade.Value, Convert.ToDecimal(ImprestLimit.Value), Remarks.Value, SyncId.Value, active);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Grade Exists');", true);
                Grade.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastGrade set SyncId='" + retval + "' where id=" + retval + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                Grade.Focus();
            }
        }
        private void UpdateGrade()
        {
            bool active = false;
            if (chkIsActive.Checked)
                active = true;
            int retval = GB.InsertUpdateGrade(Convert.ToInt32(ViewState["Id"]), Grade.Value, Convert.ToDecimal(ImprestLimit.Value), Remarks.Value, SyncId.Value, active);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Grade Exists');", true);
                Grade.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastGrade set SyncId='" + retval + "' where id=" + Convert.ToInt32(ViewState["Id"]) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                Grade.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        private void ClearControls()
        {
            Grade.Value = "";
            SyncId.Value = "";
            chkIsActive.Checked = true;
            ImprestLimit.Value = "";
            Remarks.Value = "";
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = GB.delete(Convert.ToString(ViewState["Id"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Grade.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    Grade.Focus();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/GradeMast.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateGrade();
                }
                else
                {
                    InsertGrade();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");

        }
    }
}