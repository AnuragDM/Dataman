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
    public partial class CityType : System.Web.UI.Page
    {
        CityTypeBAL CTB = new CityTypeBAL();
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
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            
            if (!IsPostBack)
            { //Ankita - 11/may/2016- (For Optimization)              

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
                btnDelete.Visible = false;
                CityTypeName.Focus();
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
           // string str = @"select * from MastCityType order by Name";
            string str = @"select Id,Name,isnull(FareAmt,0.00) as FareAmt from MastCityType order by Name";
            DataTable CityTypedt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = CityTypedt;
            rpt.DataBind();
        }
        private void FillCityTypeControls(int Id)
        {
            try
            { //Ankita - 11/may/2016- (For Optimization)
                //string citytypequery = @"select * from MastCityType where Id=" + Id;
                string citytypequery = @"select Name,isnull(FareAmt,0.00) as FareAmt from MastCityType where Id=" + Id;
                DataTable CityTypeValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, citytypequery);
                if (CityTypeValueDt.Rows.Count > 0)
                {
                    CityTypeName.Value = CityTypeValueDt.Rows[0]["Name"].ToString();
                    txtFareAmt.Value = CityTypeValueDt.Rows[0]["FareAmt"].ToString();
                
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
            int retsave = CTB.InsertUpdateCityType(0, CityTypeName.Value, Convert.ToDecimal(txtFareAmt.Value));

            if (retsave == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate City Type Name Exists');", true);
                CityTypeName.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                 ClearControls();
                CityTypeName.Value = string.Empty;
                txtFareAmt.Value = string.Empty;
                btnDelete.Visible = false;
                CityTypeName.Focus();
            }
        }
        private void UpdateCityType()
        {
            int retsave = CTB.InsertUpdateCityType(Convert.ToInt32(ViewState["Id"]), CityTypeName.Value, Convert.ToDecimal(txtFareAmt.Value));

            if (retsave == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate City Type Name Exists');", true);
                CityTypeName.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                CityTypeName.Value = string.Empty;
                txtFareAmt.Value = string.Empty;
                btnDelete.Visible = false;
                CityTypeName.Focus();
            }
        }
        private void ClearControls()
        {
            CityTypeName.Value = string.Empty;
            txtFareAmt.Value = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save"; 
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/CityType.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = CTB.delete(Convert.ToString(ViewState["Id"]));
                if (retdel > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    CityTypeName.Value = string.Empty;
                    txtFareAmt.Value = string.Empty;
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    CityTypeName.Focus();
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
            CityTypeName.Value = string.Empty;
            txtFareAmt.Value = string.Empty;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}