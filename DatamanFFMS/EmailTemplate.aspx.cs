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
    public partial class EmailTemplate : System.Web.UI.Page
    {
         int msg = 0;
         int uid = 0;
         int smID = 0;
         int dsrDays = 0;
        string VisitID = "0";
        BAL.EmailTemplate.EmailTemplateBAL up = new BAL.EmailTemplate.EmailTemplateBAL();
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
            string pageName = Path.GetFileName(Request.Path);           
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }

            btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                mainDiv.Style.Add("display", "block");
                BindKey();
                BindList();
                btnDelete.Visible = false;
            }
        }

        private void fillRepeter()
        {
            string str = @"select * from EmailTemplate";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
                rpt.DataBind();
            }
        }
        private void BindKey()
        {

            //string str = @"select * from MastEmailKey WHERE Id NOT IN (1,2)";
            string str = @"select * from MastEmailKey";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
                ddlkey.DataSource = obj;
                ddlkey.DataTextField = "EmailKey";
                ddlkey.DataValueField = "Id";
                ddlkey.DataBind();
            }
            ddlkey.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void BindList()
        {
            string str = @"select * from MastEmailVariable WHERE EmailKey='" + ddlkey.SelectedItem.Text + "'";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
              lstVariable.DataSource = obj;
              lstVariable.DataTextField = "Name";
              lstVariable.DataValueField = "Id";
              lstVariable.DataBind();
            }
        }

        private void FillDeptControls(int VisId)
        {
            try
            {
                string str = @"select * from EmailTemplate  where ID=" + ViewState["VisId"];
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    ddlkey.ClearSelection();
                    ddlkey.Items.FindByText(deptValueDt.Rows[0]["EmialKey"].ToString()).Selected = true;
                    txtsubject.Text = deptValueDt.Rows[0]["Subject"].ToString();
                    txtemail.Text = deptValueDt.Rows[0]["TemplateValue"].ToString();
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
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

           
            int retsave = up.Insert(ddlkey.SelectedItem.Text,txtsubject.Text,txtemail.Text);

            if (retsave != 0)
            {
                Settings.Instance.VistID = Convert.ToString(retsave);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
             
                btnDelete.Visible = false;
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
            }

        }

        private int checkDateForUserExists(DateTime Date)
        {
            string str = "select count(*) from MastMessage ";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }


        private void ClearControls()
        {
            ddlkey.SelectedIndex = 0;
            txtsubject.Text = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            txtemail.Text="";
            lstVariable.DataSource = new DataTable();            
            lstVariable.DataBind();

        }

        private void UpdateRecord()
        {
            int retsave = up.Update(Convert.ToInt32(ViewState["VisId"]),ddlkey.SelectedItem.Text,txtsubject.Text,txtemail.Text);
            if (retsave == 1)
            {
               // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                 btnDelete.Visible = false;
                btnSave.Text = "Save";
                ClearControls();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be update');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/EmailTemplate.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            //string str = "";
            //str = @"select * from MastMessage M inner join MastRole R on M.RoleId=R.RoleId"; 
            //DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //if (depdt.Rows.Count > 0)
            //{
            //    rpt.DataSource = depdt;
            //    rpt.DataBind();
            //}
            //else
            //{
            //    rpt.DataSource = null;
            //    rpt.DataBind();
            //}
        }

        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = up.delete(Convert.ToString(ViewState["VisId"]));
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

        protected void lstVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (HiddenField3.Value == "2")
                {
                    string va = txtemail.Text;
                    string p = va.Insert(Convert.ToInt32(hidcur.Value) + 1, "{" + lstVariable.SelectedItem.Text + "}");
                    txtemail.Text = p;
                }
                else if (HiddenField3.Value == "1")
                {
                    string va = txtsubject.Text;
                    string p = va.Insert(Convert.ToInt32(hidcur.Value) + 1, "{" + lstVariable.SelectedItem.Text + "}");
                    txtsubject.Text = p;
                }
                else
                { HiddenField3.Value = "0"; }
            }
            catch { }
         
        }

        protected void ddlkey_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindList();

            string str = @"select * from EmailTemplate  where EmialKey='" + Convert.ToString(ddlkey.SelectedItem.Text.Trim()) + "'";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ViewState["VisId"] = deptValueDt.Rows[0]["ID"].ToString();
                ddlkey.ClearSelection();
                ddlkey.Items.FindByText(deptValueDt.Rows[0]["EmialKey"].ToString()).Selected = true;
                txtsubject.Text = deptValueDt.Rows[0]["Subject"].ToString();
                txtemail.Text = deptValueDt.Rows[0]["TemplateValue"].ToString();
                btnSave.Text = "Update";
                btnDelete.Visible = true;
            }
        }

    }
}









        