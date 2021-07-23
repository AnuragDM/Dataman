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
    public partial class CityConveyanceType : System.Web.UI.Page
    {
        CityConveyanceTypeBAL CCTB = new CityConveyanceTypeBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnSave.UniqueID;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["Id"] = parameter;
                FillCityTypeControls(Convert.ToInt32(parameter));
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
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }

            // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            { 
                btnDelete.Visible = false;
                CityConveyanceTypeName.Focus();
                fillRepeter();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillCityTypeControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }
        private void fillRepeter()
        {
            //Ankita - 11/may/2016- (For Optimization)
            //string str = @"select * from MastCityConveyanceType order by Name";
            string str = @"select Id,Name from MastCityConveyanceType order by Name";
            DataTable CityTypedt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = CityTypedt;
            rpt.DataBind();
        }
        private void FillCityTypeControls(int Id)
        {
            try
            {//Ankita - 11/may/2016- (For Optimization)
               // string citytypequery = @"select * from MastCityConveyanceType where Id=" + Id;
                string citytypequery = @"select Name from MastCityConveyanceType where Id=" + Id;
                DataTable CityTypeValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, citytypequery);
                if (CityTypeValueDt.Rows.Count > 0)
                {
                    CityConveyanceTypeName.Value = CityTypeValueDt.Rows[0]["Name"].ToString();

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateCityType();
                }
                else
                {
                    InsertCityType();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
            }
        }

        private void InsertCityType()
        {
            int retsave = CCTB.InsertUpdateCityConveyanceType(0, CityConveyanceTypeName.Value);

            if (retsave == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate City Conveyance Type Name Exists');", true);
                CityConveyanceTypeName.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                CityConveyanceTypeName.Value = string.Empty;
                btnDelete.Visible = false;
                CityConveyanceTypeName.Focus();
            }
        }
        private void UpdateCityType()
        {
            int retsave = CCTB.InsertUpdateCityConveyanceType(Convert.ToInt32(ViewState["Id"]), CityConveyanceTypeName.Value);

            if (retsave == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate City Conveyance Type Name Exists');", true);
                CityConveyanceTypeName.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                CityConveyanceTypeName.Value = string.Empty;
                btnDelete.Visible = false;
                CityConveyanceTypeName.Focus();
            }
        }
        private void ClearControls()
        {
            CityConveyanceTypeName.Value = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/CityConveyanceType.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = CCTB.delete(Convert.ToString(ViewState["Id"]));
                if (retdel > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    CityConveyanceTypeName.Value = string.Empty;
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    CityConveyanceTypeName.Focus();
                }
            }
           
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            CityConveyanceTypeName.Value = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}