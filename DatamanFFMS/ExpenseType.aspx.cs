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
    public partial class ExpenseType : System.Web.UI.Page
    {
        ExpenseBAL EB = new ExpenseBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnSave.UniqueID;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["Id"] = parameter;
                FillTMControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 11/may/2016- (For Optimization)
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
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";

            if (!IsPostBack)
            {
                chkIsActive.Checked = true;
                btnDelete.Visible = false;

                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillTMControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }
        
        private void fillRepeter()
        {  //Ankita - 11/may/2016- (For Optimization)
            //string str = @"select *, case Active when 1 then 'Yes' else 'No' end as Active1 from MastExpenseType order by Name";
            string str = @"select Id,Name,ExpenseTypeCode,Remarks,SyncId,case Active when 1 then 'Yes' else 'No' end as Active1 from MastExpenseType order by Name";
            DataTable TravelModedt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = TravelModedt;
            rpt.DataBind();
        }
        private void FillTMControls(int Id)
        {
            try
            {//Ankita - 11/may/2016- (For Optimization)
               // string Qry = @"select * from MastExpenseType where Id=" + Id;
                string Qry = @"select Id,Name,ExpenseTypeCode,Remarks,SyncId,Active from MastExpenseType where Id=" + Id;
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (dt.Rows.Count > 0)
                {
                    ExpName.Value = dt.Rows[0]["Name"].ToString();
                    Remarks.Value = dt.Rows[0]["Remarks"].ToString();
                    SyncId.Value = dt.Rows[0]["SyncId"].ToString();
                    ddlExpCat.SelectedValue = dt.Rows[0]["ExpenseTypeCode"].ToString();
                    if (Convert.ToBoolean(dt.Rows[0]["Active"]) == true)
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
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private void InsertTM()
        {
            int retval = EB.InsertUpdateExpenseType(0, ExpName.Value, Remarks.Value, SyncId.Value, chkIsActive.Checked,ddlExpCat.SelectedValue);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Name Exists');", true);
                ExpName.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                ExpName.Focus();
            }
        }
        private void UpdateTM()
        {
            int retval = EB.InsertUpdateExpenseType(Convert.ToInt32(ViewState["Id"]), ExpName.Value, Remarks.Value,  SyncId.Value,chkIsActive.Checked,ddlExpCat.SelectedValue);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Name Exists');", true);
                ExpName.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                ExpName.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        private void ClearControls()
        {
            ExpName.Value = "";
            SyncId.Value = "";
            Remarks.Value = "";
            chkIsActive.Checked = true;
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
                int retdel = EB.delete(Convert.ToString(ViewState["Id"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    ExpName.Focus();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/ExpenseType.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateTM();
                }
                else
                {
                    InsertTM();
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