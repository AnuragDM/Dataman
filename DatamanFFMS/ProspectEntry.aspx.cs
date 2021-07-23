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
    public partial class ProspectEntry : System.Web.UI.Page
    {
        BAL.Prospects.ProspectsBAL dp = new BAL.Prospects.ProspectsBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            parameter = Request["__EVENTARGUMENT"];

            if (parameter != "")
            {
                ViewState["VisId"] = parameter;
              //  divdocid.Visible = false;
                Settings.Instance.VistID = Convert.ToString(ViewState["VisId"]);
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);            
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
          //  btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if(!IsPostBack)
            {
                this.fillCountry();
                ddlstate.Items.Insert(0, new ListItem("-- Select --", "0"));
                ddlcity.Items.Insert(0, new ListItem("-- Select --", "0"));
                btnDelete.Visible = false;

                //txtmDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MMM/yyyy");
                //txttodate.Text = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MMM/yyyy");
            }
        }
        private void fillRepeter()
        {
            string str = "";
            if (txtmDate.Text != "" && txttodate.Text != "")
            {
                str = @"select * from Temp_Prospects where UserId=" + Settings.Instance.UserID + " and Created_Date>='" + Settings.dateformat1(txtmDate.Text) + " 00:00' and Created_Date<='" + Settings.dateformat1(txttodate.Text) + " 23:59'";
            }
            else if(txtmDate.Text!=""&& txttodate.Text=="")
            {
                str = @"select * from Temp_Prospects where UserId=" + Settings.Instance.UserID + " and Created_Date>='" + Settings.dateformat1(txtmDate.Text) + " 00:00'";
            }
            else
            {
                str = @"select * from Temp_Prospects where UserId=" + Settings.Instance.UserID;
            }
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
        private void fillCountry()
        {
            string str = "select distinct countryid, countryName countryName from ViewGeo";
            DataTable countrydt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (countrydt.Rows.Count > 0)
            {
                ddlcountry.DataSource = countrydt;
                ddlcountry.DataTextField = "countryName";
                ddlcountry.DataValueField = "countryid";
                ddlcountry.DataBind();
            }
            ddlcountry.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void fillstate()
        {
            ddlstate.Items.Clear();
            string str = "select distinct stateid, stateName from ViewGeo where countryid=" + ddlcountry.SelectedValue;
            DataTable countrydt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (countrydt.Rows.Count > 0)
            {
                ddlstate.DataSource = countrydt;
                ddlstate.DataTextField ="stateName";
                ddlstate.DataValueField ="stateid";
                ddlstate.DataBind();
            }
            ddlstate.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void fillcity()
        {
            ddlcity.Items.Clear();
            string str = "select distinct cityid,cityName from ViewGeo where stateid="+ddlstate.SelectedValue;
            DataTable countrydt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (countrydt.Rows.Count > 0)
            {
                ddlcity.DataSource = countrydt;
                ddlcity.DataTextField = "cityName";
                ddlcity.DataValueField = "cityid";
                ddlcity.DataBind();
            }
            ddlcity.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void FillDeptControls(int ProspectId)
        {
            try
            {
                string str = @"select * from Temp_Prospects  where ProspectId=" + ProspectId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";
                    txtName.Text = deptValueDt.Rows[0]["Name"].ToString();
                    txtAddress.Text = deptValueDt.Rows[0]["Address1"].ToString();
                    ddlcountry.SelectedValue = deptValueDt.Rows[0]["Country"].ToString();
                    fillstate();
                    ddlstate.SelectedValue = deptValueDt.Rows[0]["state"].ToString();
                    fillcity();
                    ddlcity.SelectedValue = deptValueDt.Rows[0]["CityId"].ToString();

                    txtEmail.Text = deptValueDt.Rows[0]["Email"].ToString();
                    txtMobileNo.Text = deptValueDt.Rows[0]["Mobile"].ToString();
                    txtpin.Text = deptValueDt.Rows[0]["pin"].ToString();
                    txtRemarks.Text = deptValueDt.Rows[0]["Remark"].ToString();
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

         //   fillRepeter();
            rpt.DataSource = null;
            rpt.DataBind();
            btnDelete.Visible = true;

            //Added on 16-Dec-2015
            txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            //End
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlcountry.SelectedIndex>0)
            {
                this.fillstate();
            }
            else
            {
                ddlstate.Items.Clear();
            }
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlcountry.SelectedIndex>0 && ddlstate.SelectedIndex>0)
            {
                this.fillcity();
            }
            else
            {
                ddlcity.Items.Clear();
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
            //string docID = Settings.GetDocID("VISSN", DateTime.Now);
            //Settings.SetDocID("VISSN", docID);
            if(txtEmail.Text!="")
            {
                if (checkUserExistsEmail(txtMobileNo.Text) > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('EmailID already Exists');", true);
                }
            }
            if (checkUserExists(txtMobileNo.Text) > 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Mobile No already Exists');", true);
            }
            else
            {
                int retsave = dp.Insert(txtName.Text, txtAddress.Text, txtpin.Text, txtEmail.Text, txtMobileNo.Text, txtRemarks.Text, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(ddlstate.SelectedValue), Convert.ToInt32(ddlcountry.SelectedValue),Settings.GetUTCTime().ToString());
                if (retsave != 0)
                {
                    ClearControls();
                    btnDelete.Visible = false;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "success", "Successmessage('Record Inserted Successfully-" + retsave + "');", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                }
            }
        }
        private int checkUserExists(string MobNo)
        {
            string str = "select count(*) from Temp_Prospects  where Mobile='"+MobNo+"'";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }

        private int checkUserExistsEmail(string EmailID)
        {
            string str = "select count(*) from Temp_Prospects  where Email='" + EmailID + "'";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }

        private int checkUserExistsUpdate(string MobNo)
        {
            string str = "select count(*) from Temp_Prospects  where Mobile='" + MobNo + "' and ProspectId !="+Convert.ToInt32(ViewState["VisId"]);
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }

        private int checkUserExistsUpdateEmail(string Email)
        {
            string str = "select count(*) from Temp_Prospects  where Email='" + Email + "' and ProspectId !=" + Convert.ToInt32(ViewState["VisId"]);
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }

        private void ClearControls()
        {
            txtRemarks.Text = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            txtRemarks.Text = "";
          //  divdocid.Visible = false;
            txtmDate.Text = "";
            txttodate.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtpin.Text = "";
            ddlcountry.SelectedIndex = 0;
            ddlstate.Items.Clear();
            ddlcity.Items.Clear();
            txtEmail.Text = "";
            txtMobileNo.Text = "";
            ddlstate.Items.Insert(0, new ListItem("-- Select --", "0"));
            ddlcity.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        private void UpdateRecord()
        {
            if (txtEmail.Text != "")
            {
                if (checkUserExistsUpdateEmail(txtEmail.Text) > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Mobile No already Exists');", true);
                }
            }
            if (checkUserExistsUpdate(txtMobileNo.Text) > 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Mobile No already Exists');", true);
            }
            else
            {
                int retsave = dp.Update(Convert.ToInt32(ViewState["VisId"]), txtName.Text, txtAddress.Text, txtpin.Text, txtEmail.Text, txtMobileNo.Text, txtRemarks.Text, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(ddlstate.SelectedValue), Convert.ToInt32(ddlcountry.SelectedValue), Settings.GetUTCTime().ToString());
                if (retsave == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Updated');", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // ClearControls();
           
            Response.Redirect("~/ProspectEntry.aspx");
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
            btnDelete.Visible = false;
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillRepeter();
        }
    }
}