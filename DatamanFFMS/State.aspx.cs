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
using AstralFFMS.Models;
using System.Web.Services;


namespace AstralFFMS
{
    public partial class State : System.Web.UI.Page
    {
        LocationBAL LB = new LocationBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            //HiddenUnderID.Value = "";
            if (parameter != "")
            {
                Page.Form.DefaultButton = btnSave.UniqueID;
                this.Form.DefaultButton = this.btnSave.UniqueID;
                ViewState["AreaId"] = parameter;
                FillLocControls(Convert.ToInt32(parameter));
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
           //   btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
           // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                Response.Write(Request.Form[ddlParentLoc.UniqueID]);
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindState", "BindState();", true);
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["AreaId"] != null)
                {
                    FillLocControls(Convert.ToInt32(Request.QueryString["AreaId"]));
                }
                else { 
                    //BindParent(0);
                    BindBuisnessPlace();
                    BindSection();
                }
            }
        }
        /// <summary>
        /// //////////////
        /// 
        /// </summary>
        /// <param name="regionId"></param>

        protected override void Render(HtmlTextWriter writer)
       {
            DataTable dt = new DataTable();
            string strQ = "select AreaID,AreaName from mastarea where AreaType='Region' and Active='1' order by AreaName";
            dt = DbConnectionDAL.GetDataTable(System.Data.CommandType.Text, strQ);
            foreach (DataRow dtrow in dt.Rows)
            {
                Page.ClientScript.RegisterForEventValidation(ddlParentLoc.UniqueID, dtrow["AreaID"].ToString());

            }
            dt.Dispose();
           
            base.Render(writer);
        }


        private void BindParent(int regionId)
        {string strQ ="";
            if(regionId > 0)
                 strQ = "select AreaID,AreaName from mastarea where AreaType='Region' and (Active='1' or Areaid in ("+regionId+"))  order by AreaName";     
                   
            else
                strQ = "select AreaID,AreaName from mastarea where AreaType='Region' and Active='1' order by AreaName"; 
                fillDDLDirect(ddlParentLoc, strQ, "AreaID", "AreaName", 1);
        }
        private void BindBuisnessPlace ()
        {
            string str = "select Id,Name FROM MastBuisnessPlace";
            fillDDLDirect(ddlBuisness, str, "Id", "Name", 1);
        }
        private void BindSection()
        {
            string str = "select Id,Name FROM MastSection";
            fillDDLDirect(ddlSection, str, "Id", "Name", 1);
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
        private void fillRepeter()
        {
            //Query Optimized - Abhishek - 28-05-2016
            //string str = @"select ma.*,ma1.DisplayName as DisplayName1,case ma.Active when 1 then 'Yes' else 'No' end as Active1,ma1.AreaName as Parent from MastArea ma inner join MastArea ma1 on ma.UnderId=ma1.AreaID  where ma.AreaType='STATE'";
            string str = @"select ma.AreaId,ma.AreaName,ma.SyncId,ma1.DisplayName as DisplayName1,case ma.Active when 1 then 'Yes' else 'No' end as Active1,ma1.AreaName as Parent from MastArea ma inner join MastArea ma1 on ma.UnderId=ma1.AreaID  where ma.AreaType='STATE'";
            DataTable Locdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = Locdt;
            rpt.DataBind();
            Locdt.Dispose();
        }
        private void FillLocControls(int AreaId)
        {
            try
            {//Ankita - 20/may/2016- (For Optimization)
                //string MatGrpQry = @"select * from MastArea where AreaId=" + AreaId;
                string MatGrpQry = @"select AreaName,SyncId,UnderId,Active,BuisnessPlace,Section,CostCentre from MastArea where AreaId=" + AreaId;
                DataTable LocValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, MatGrpQry);
                if (LocValueDt.Rows.Count > 0)
                {
                    Location.Value = LocValueDt.Rows[0]["AreaName"].ToString();
                    SyncId.Value = LocValueDt.Rows[0]["SyncId"].ToString();
                    HiddenUnderID.Value = LocValueDt.Rows[0]["UnderId"].ToString();                    
                    //BindParent(Convert.ToInt32(LocValueDt.Rows[0]["UnderId"]));
                    ddlParentLoc.SelectedValue = LocValueDt.Rows[0]["UnderId"].ToString();
                    if (Convert.ToBoolean(LocValueDt.Rows[0]["Active"]) == true)
                    {
                        chkIsActive.Checked = true;
                    }
                    else
                    {
                        chkIsActive.Checked = false;
                    }
                    BindBuisnessPlace();
                    BindSection();                   
                    if (!string.IsNullOrEmpty(LocValueDt.Rows[0]["BuisnessPlace"].ToString()))
                        ddlBuisness.SelectedValue = LocValueDt.Rows[0]["BuisnessPlace"].ToString();
                    if (!string.IsNullOrEmpty(LocValueDt.Rows[0]["Section"].ToString()))                       
                        ddlSection.SelectedValue = LocValueDt.Rows[0]["Section"].ToString();
                    CostCentre1.Text = LocValueDt.Rows[0]["CostCentre"].ToString();
                   
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
                LocValueDt.Dispose();
              
            }

            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private void InsertLoc()
        {
            string active = "0", RegionID = Request.Form[HiddenUnderID.UniqueID];
            if (chkIsActive.Checked)
                active = "1";
            int buisnessplace = 0;
            int section = 0;
            int costcentre = 0;
            if(ddlBuisness.SelectedValue != "0")
            {
                buisnessplace = Convert.ToInt32(ddlBuisness.SelectedValue);
            }
            if(ddlSection.SelectedValue != "0")
            {
                section = Convert.ToInt32(ddlSection.SelectedValue);
            }
            if (CostCentre1.Text != "")
            {
                costcentre = Convert.ToInt32(ddlSection.SelectedValue);
            }

          //  int retval = LB.InsertLocation(Convert.ToInt32(ddlParentLoc.SelectedValue), Location.Value, SyncId.Value, active, "", "STATE", 0, 0, "", "");
            int retval = LB.InsertLocation(Convert.ToInt32(RegionID), Location.Value, SyncId.Value, active, "", "STATE", 0, 0, "", "", buisnessplace, section, costcentre);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate State Exists');", true);
                Location.Focus();
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
                    string syncid = "update MastArea set SyncId='" + retval + "' where areaid=" + retval + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                Location.Focus();
            }
        }
        private void UpdateLoc()
        {
            string active = "0", RegionID = Request.Form[HiddenUnderID.UniqueID];
            if (chkIsActive.Checked)
                active = "1";
            int buisnessplace = 0;
            int section = 0;
            int costcentre = 0;
            if (ddlBuisness.SelectedValue != "0")
            {
                buisnessplace = Convert.ToInt32(ddlBuisness.SelectedValue);
            }
            if (ddlSection.SelectedValue != "0")
            {
                section = Convert.ToInt32(ddlSection.SelectedValue);
            }
            if (CostCentre1.Text != "")
            {
                costcentre = Convert.ToInt32(ddlSection.SelectedValue);
            }
            //int retval = LB.UpdateLocation(Convert.ToInt32(ViewState["AreaId"]), Convert.ToInt32(ddlParentLoc.SelectedValue), Location.Value, SyncId.Value, active, "", "STATE", 0, 0, "", "");
            int retval = LB.UpdateLocation(Convert.ToInt32(ViewState["AreaId"]), Convert.ToInt32(RegionID), Location.Value, SyncId.Value, active, "", "STATE", 0, 0, "", "", buisnessplace,section,costcentre);
            ddlParentLoc.SelectedIndex = 0;
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Region Exists');", true);
                Location.Focus();
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
                    string syncid = "update MastArea set SyncId='" + retval + "' where areaid=" + Convert.ToInt32(ViewState["AreaId"]) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                Location.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        private void ClearControls()
        {
            Location.Value = "";
            SyncId.Value = "";
            HiddenUnderID.Value = string.Empty;
            chkIsActive.Checked = true;
            ddlParentLoc.SelectedIndex = 0;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //BindParent(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = LB.delete(Convert.ToString(ViewState["AreaId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    Location.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Location.Focus();
                }
                HiddenUnderID.Value = "";
                //ddlParentLoc.SelectedIndex = 0;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/State.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
         {
            try
            {
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindState", "BindState();", true);
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindState", "BindState();", true);
                if (btnSave.Text == "Update")
                {
                    UpdateLoc();
                }
                else
                {
                    InsertLoc();
                }
                HiddenUnderID.Value = "";
                //ddlParentLoc.SelectedIndex = 0;

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