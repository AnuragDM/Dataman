using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.IO;
using System.Data;
using DAL;

namespace AstralFFMS
{
    public partial class CRMStatusMaster : System.Web.UI.Page
    {

        CRMBAL CB = new CRMBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                Page.Form.DefaultButton = btnSave.UniqueID;
                this.Form.DefaultButton = this.btnSave.UniqueID;
                ViewState["status_Id"] = parameter;
                FillControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
       
            string pageName = Path.GetFileName(Request.Path);         
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
             
                btnDelete.Visible = false;

                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["status_id"] != null)
                {
                    FillControls(Convert.ToInt32(Request.QueryString["status_id"]));
                }
            }
        }
        private void fillRepeter()
        {
            string str = @"select * from CRM_MastStatus order by Status asc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        private void FillControls(int status_id)
        {
            try
            {
                string Qry = @"select * from CRM_MastStatus where status_id=" + status_id;
                DataTable ValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (ValueDt.Rows.Count > 0)
                {
                    Status.Value = ValueDt.Rows[0]["Status"].ToString();
                    Description.Value = ValueDt.Rows[0]["Description"].ToString();
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }
        private void ClearControls()
        {
            Status.Value = "";
            Description.Value = "";
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
        private void Insert()
        {
            int retval = CB.InsertUpdateStatus(0, Status.Value, Description.Value);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Status Exists');", true);
                Status.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                Status.Focus();
            }
        }
        private void Update()
        {
            int retval = CB.InsertUpdateStatus(Convert.ToInt32(ViewState["status_Id"]), Status.Value, Description.Value);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Status Exists');", true);
                Status.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                Status.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    Update();
                }
                else
                {
                    Insert();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/CRMStatusMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = CB.deleteStatus(Convert.ToString(ViewState["status_Id"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Status.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    Status.Focus();
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
    }
}