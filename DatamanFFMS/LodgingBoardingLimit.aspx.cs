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
    public partial class LodgingBoardingLimit : System.Web.UI.Page
    {
        LocalLodgingBoardingLimitBAL LBL = new LocalLodgingBoardingLimitBAL();
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
            //Ankita - 13/may/2016- (For Optimization)
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
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";

            if (!IsPostBack)
            {
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                BindCityType();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillTMControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
                else { BindDesignation(0); }
            }
        }
        #region Bind Dropdowns
        private void BindCityType()
        { //Ankita - 13/may/2016- (For Optimization)
            //string strQ = "select * from MastCityType order by Name";
            string strQ = "select ID,Name from MastCityType order by Name";
            fillDDLDirect(ddlcitytype, strQ, "ID", "Name", 1);
        }
        private void BindDesignation(int DesigId)
        {
            string str = "";
            if (DesigId > 0)
                str = "select DesId,DesName from MastDesignation where (active='1' or DesId in(" + DesigId + ")) order by Desname";
            else
                str = "select DesId,DesName from MastDesignation where Active='1' order by Desname";
            fillDDLDirect(ddldesignation, str, "DesId", "DesName", 1);
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }
        #endregion
        private void fillRepeter()
        {
            string str = @"select MTE.Id,MTM.Name,MD.DesName,MTE.Amount,MTE.SyncId,case MTE.Active when 1 then 'Yes' else 'No' end as Active1 from MastLocalLodgingBoardingLimit MTE left join MastDesignation MD on MTE.DesId=MD.DesId left join MastCityType MTM on MTE.CityTypeId=MTM.Id order by MTE.Active";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        private void FillTMControls(int Id)
        {
            try
            {
                string Qry = @"select * from MastLocalLodgingBoardingLimit where Id=" + Id;
                DataTable TravelEldt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (TravelEldt.Rows.Count > 0)
                {
                    BindDesignation(Convert.ToInt32(TravelEldt.Rows[0]["DesID"]));
                    ddldesignation.SelectedValue = TravelEldt.Rows[0]["DesID"].ToString();
                    ddlcitytype.SelectedValue = TravelEldt.Rows[0]["CityTypeId"].ToString();
                    Amount.Value = TravelEldt.Rows[0]["Amount"].ToString();
                    SyncId.Value = TravelEldt.Rows[0]["SyncId"].ToString();
                    if (Convert.ToBoolean(TravelEldt.Rows[0]["Active"]) == true)
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
            int retval = LBL.InsertUpdateLodgingBoardingLimit(0, Convert.ToInt32(ddlcitytype.SelectedValue), Convert.ToInt32(ddldesignation.SelectedValue), Convert.ToDecimal(Amount.Value), SyncId.Value, chkIsActive.Checked, Remarks.Value);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Entry Exists');", true);
                ddlcitytype.Focus();
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
                ddlcitytype.Focus();
            }
        }
        private void UpdateTM()
        {
            int retval = LBL.InsertUpdateLodgingBoardingLimit(Convert.ToInt32(ViewState["Id"]), Convert.ToInt32(ddlcitytype.SelectedValue), Convert.ToInt32(ddldesignation.SelectedValue), Convert.ToDecimal(Amount.Value), SyncId.Value, chkIsActive.Checked, Remarks.Value);

            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Entry Exists');", true);
                ddlcitytype.Focus();
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
                ddlcitytype.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        private void ClearControls()
        {
            ddlcitytype.SelectedIndex = 0;
            ddldesignation.SelectedIndex = 0;
            SyncId.Value = "";
            Amount.Value = "";
            Remarks.Value = "";
            chkIsActive.Checked = true;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            BindDesignation(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = LBL.delete(Convert.ToString(ViewState["Id"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ddlcitytype.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    ddlcitytype.Focus();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/LodgingBoardingLimit.aspx");
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