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
    public partial class AddContactType : System.Web.UI.Page
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
                ViewState["id"] = parameter;
                FillControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
        }
        private void fillRepeter()
        {
            string str = @"select * from CRM_MastContactType order by type,data";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        private void FillControls(int Tag_Id)
        {
            try
            {
                string Qry = @"select * from CRM_MastContactType where id=" + Tag_Id;
                DataTable ValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (ValueDt.Rows.Count > 0)
                {
                    ddlphonetype1.SelectedValue = ValueDt.Rows[0]["value"].ToString();
                    Contactstype.Value = ValueDt.Rows[0]["Data"].ToString();
                    Txtsort.Value = ValueDt.Rows[0]["Sort"].ToString();
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
       
        private void Insert()
        {

            Contactstype.Focus();
            int retval = CB.InsertMastContacsType(0,ddlphonetype1.SelectedValue.ToString(),Contactstype.Value,Convert.ToInt32(Txtsort.Value),"I");
            if (retval == 2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Enter Different Sorting No.');", true);

                ClearControls();
            }
            else if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Status Exists');", true);
               
                ClearControls();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
               ClearControls();
                Contactstype.Focus();
            }
        }
        private void Update()
        {
            int retval = CB.InsertMastContacsType(Convert.ToInt32(ViewState["id"]), ddlphonetype1.SelectedValue.ToString(), Contactstype.Value, Convert.ToInt32(Txtsort.Value), "U");
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Status Exists');", true);
                Contactstype.Focus();
                ClearControls();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
               ClearControls();
                Contactstype.Focus();
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
            Response.Redirect("AddContactType.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retval = CB.InsertMastContacsType(Convert.ToInt32(ViewState["id"]), ddlphonetype1.SelectedValue.ToString(), Contactstype.Value, Convert.ToInt32(Txtsort.Value), "D");
                if (retval != -1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Contactstype.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                   ClearControls();
                    Contactstype.Focus();
                }
            }
        }
       
        protected void ClearControls()
        {
            ddlphonetype1.SelectedValue = "Email";
            Contactstype.Value = "";
            Txtsort.Value = "";

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            //ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}