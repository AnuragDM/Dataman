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
    public partial class MastMessage : System.Web.UI.Page
    {
        int msg = 0;
         int uid = 0;
         int smID = 0;
         int dsrDays = 0;
        string VisitID = "0";
        BAL.Message.MessageBAL up = new BAL.Message.MessageBAL();
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
            //Ankita - 20/may/2016- (For Optimization)
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
              //  btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
           // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                chkIsAdmin.Checked = true;
                mainDiv.Style.Add("display", "block");
               // BindDDlCity();
               // BindDDLRole();

                //Added 16-12-2015
                btnDelete.Visible = false;
                //End
            }
        }

        private void BindDDLRole()
        {
            try
            { //Ankita - 20/may/2016- (For Optimization)
                //string mastroleqry = @"select * from MastRole order by RoleName";
                string mastroleqry = @"select RoleId,RoleName from MastRole order by RoleName";
                DataTable dtrole = DbConnectionDAL.GetDataTable(CommandType.Text, mastroleqry);
                if (dtrole.Rows.Count > 0)
                {
                    ddlrole.DataSource = dtrole;
                    ddlrole.DataTextField = "RoleName";
                    ddlrole.DataValueField = "RoleId";
                    ddlrole.DataBind();
                }
                ddlrole.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void fillRepeter()
        {
            string str = @"select M.Id,M.RoleId,M.Message,M.AreaID,R.RoleName,CASE WHEN M.Active = 1 
        THEN 'Yes' ELSE 'No'  END as active from MastMessage M inner join MastRole R on M.RoleId=R.RoleId";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
                rpt.DataBind();
            }
        }
        private void BindDDlCity()
        {
            ddlstate.Items.Clear();
            //string str = @"select * from mastarea where areatype='COUNTRY' OR areatype='REGION' OR areatype='STATE' OR areatype='CITY' and Active=1 order by AreaName";
            string str = @"select AreaId,DisplayName from mastarea where areatype='COUNTRY' OR areatype='REGION' OR areatype='STATE' OR areatype='CITY' and Active=1 order by AreaName";
            //            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.SMID+ ")) and areatype='STATE' and Active=1 order by AreaName ";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
                ddlstate.DataSource = obj;
                ddlstate.DataTextField = "DisplayName";
                ddlstate.DataValueField = "AreaId";
                ddlstate.DataBind();
            }
            ddlstate.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private string FillDistributorByID(int Partyid)
        {
            string str = "select PartyName from MastParty where PartyId=" + Partyid;
            return Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
        }

        private void FillDeptControls(int VisId)
        {
            try
            {
                string str = @"select * from MastMessage  where Id=" + VisId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    HiddenRoleID.Value = deptValueDt.Rows[0]["RoleId"].ToString();
                    ddlrole.SelectedValue = deptValueDt.Rows[0]["RoleId"].ToString();
                    txtmessage.Text = deptValueDt.Rows[0]["Message"].ToString();

                    HiddenGeolocationID.Value = deptValueDt.Rows[0]["AreaId"].ToString();
                    ddlstate.SelectedValue = deptValueDt.Rows[0]["AreaId"].ToString();
                    chkIsAdmin.Checked = Convert.ToBoolean(deptValueDt.Rows[0]["Active"]);
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

            string RoleId = Request.Form[HiddenRoleID.UniqueID], GeolocationID = Request.Form[HiddenGeolocationID.UniqueID];
            int retsave = up.Insert(Convert.ToInt32(RoleId), txtmessage.Text, Convert.ToInt32(GeolocationID), chkIsAdmin.Checked);

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
            //ddlrole.SelectedIndex = 0;
            //ddlstate.SelectedIndex = 0;
            HiddenGeolocationID.Value = string.Empty;
            HiddenRoleID.Value = string.Empty;
            txtmessage.Text = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";

        }

        private void UpdateRecord()
        {
            string RoleId = Request.Form[HiddenRoleID.UniqueID], GeolocationID = Request.Form[HiddenGeolocationID.UniqueID];
            int retsave = up.Update(Convert.ToInt32(ViewState["VisId"]), Convert.ToInt32(RoleId), txtmessage.Text, Convert.ToInt32(GeolocationID), chkIsAdmin.Checked);
            if (retsave == 1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                btnDelete.Visible = false;
                btnSave.Text = "Save";
                ClearControls();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be update');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MastMessage.aspx");
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

        protected void ddlUndeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDlCity();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            str = @"select * from MastMessage M inner join MastRole R on M.RoleId=R.RoleId";
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
                else {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
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

    }
}








