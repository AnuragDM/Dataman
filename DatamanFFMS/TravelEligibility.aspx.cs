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
    public partial class TravelEligibility : System.Web.UI.Page
    {
        TravelEligibilityBAL TEB = new TravelEligibilityBAL();
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
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
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
                    FillTMControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
                else { BindTravelMode(0); BindDesignation(0); }
            }
        }
        #region Bind Dropdowns
        private void BindTravelMode(int TMId)
        {
            string strQ = "";
            if (TMId > 0)
                strQ = "select Id,Name from MastTravelMode where  (Active='1' or Id in (" + TMId + "))  order by Name";

            else
                strQ = "select Id,Name from MastTravelMode where Active='1' order by Name";
            fillDDLDirect(ddltravelMode, strQ, "ID", "Name", 1);
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
            string str = @"select MTE.Id,MTM.Name,MD.DesName,MTE.SyncId,case MTE.Active when 1 then 'Yes' else 'No' end as Active1 from MastTravelEligibility MTE left join MastDesignation MD on MTE.DesId=MD.DesId left join MastTravelMode MTM on MTE.TravelModeId=MTM.Id order by MTE.Active";
            DataTable TravelModedt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = TravelModedt;
            rpt.DataBind();
        }
        private void FillTMControls(int Id)
        {
            try
            {
                string Qry = @"select * from MastTravelEligibility where Id=" + Id;
                DataTable TravelEldt = DbConnectionDAL.GetDataTable(CommandType.Text, Qry);
                if (TravelEldt.Rows.Count > 0)
                {
                    BindDesignation(Convert.ToInt32(TravelEldt.Rows[0]["DesID"]));
                    ddldesignation.SelectedValue = TravelEldt.Rows[0]["DesID"].ToString();
                    BindTravelMode(Convert.ToInt32(TravelEldt.Rows[0]["TravelModeId"]));
                    ddltravelMode.SelectedValue = TravelEldt.Rows[0]["TravelModeId"].ToString();
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
            int retval = TEB.InsertUpdateTravelEligibility(0,Convert.ToInt32(ddltravelMode.SelectedValue), Convert.ToInt32(ddldesignation.SelectedValue),SyncId.Value, chkIsActive.Checked);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Entry Exists');", true);
                ddltravelMode.Focus();
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
                ddltravelMode.Focus();
            }
        }
        private void UpdateTM()
        {
            int retval = TEB.InsertUpdateTravelEligibility(Convert.ToInt32(ViewState["Id"]), Convert.ToInt32(ddltravelMode.SelectedValue), Convert.ToInt32(ddldesignation.SelectedValue), SyncId.Value, chkIsActive.Checked);
         
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Entry Exists');", true);
                ddltravelMode.Focus();
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
                ddltravelMode.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        private void ClearControls()
        {
            ddltravelMode.SelectedIndex = 0;
            ddldesignation.SelectedIndex = 0;
            SyncId.Value = "";
            chkIsActive.Checked = true;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            BindTravelMode(0); BindDesignation(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = TEB.delete(Convert.ToString(ViewState["Id"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ddltravelMode.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    ddltravelMode.Focus();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/TravelEligibility.aspx");
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