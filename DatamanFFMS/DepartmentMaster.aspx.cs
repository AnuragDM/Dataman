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
    public partial class DepartmentMaster : System.Web.UI.Page
    {
        BAL.Department.deptAll dp = new BAL.Department.deptAll();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["depId"] = parameter;
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 18/may/2016- (For Optimization)
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
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                //DepName.Focus();
               // fillRepeter();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["DepId"] != null)
                {
                    FillDeptControls(Convert.ToInt32(Request.QueryString["DepId"]));
                }
            }
        }
        private void fillRepeter()
        {

            string str = @"select DepId,DepName,SyncId,CASE WHEN Active = 1 
        THEN 'Yes' ELSE 'No'  END as active from MastDepartment";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();

            depdt.Dispose();
        }
        private void FillDeptControls(int depId)
        {
            try
            { //Ankita - 18/may/2016- (For Optimization)
                //string deptquery = @"select * from MastDepartment where DepId=" + depId;
                string deptquery = @"select DepName,SyncId,Active from MastDepartment where DepId=" + depId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, deptquery);
                if (deptValueDt.Rows.Count > 0)
                {
                    DepName.Value = deptValueDt.Rows[0]["DepName"].ToString();
                    SyncId.Value = deptValueDt.Rows[0]["SyncId"].ToString();
                    if (Convert.ToBoolean(deptValueDt.Rows[0]["Active"]) == true)
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
                deptValueDt.Dispose();
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
                    UpdateDepartment();
                }
                else
                {
                    InsertDepartment();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertDepartment()
        {
            string str = @"select Count(*) from MastDepartment where DepName='" + DepName.Value + "'";

            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastDepartment where SyncId='" + SyncId.Value + "'";

                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }

                if (val1 == 0)
                {
                    int retsave = dp.Insert(DepName.Value.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked,BusinessLayer.Settings.GetUTCTime());

                    if (retsave != 1)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastDepartment set SyncId='" + retsave + "' where depId=" + retsave + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        // ClearControls();
                        DepName.Value = string.Empty;
                        SyncId.Value = string.Empty;
                        chkIsActive.Checked = true;
                        btnDelete.Visible = false;
                        DepName.Focus();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                    SyncId.Focus();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exist');", true);
                DepName.Focus();
            }
        }

        private void ClearControls()
        {
            DepName.Value = string.Empty;
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            Response.Redirect("~/DepartmentMaster.aspx");
        }

        private void UpdateDepartment()
        {
            string strupd = @"select Count(*) from MastDepartment where DepName='" + DepName.Value + "' and DepId<>" + Convert.ToInt32(ViewState["depId"]) + "";

            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));

            if (valupd == 0)
            {
                int valupd1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string strupd1 = @"select Count(*) from MastDepartment where SyncId='" + SyncId.Value + "' and DepId<>" + Convert.ToInt32(ViewState["depId"]) + "";

                    valupd1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                }
                if (valupd1 == 0)
                {
                    int retsave = dp.Update(ViewState["depId"].ToString(), DepName.Value.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());
                    if (retsave == 1)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastDepartment set SyncId='" + ViewState["depId"] + "' where Depid=" + Convert.ToInt32(ViewState["depId"]) + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        DepName.Value = string.Empty;
                        SyncId.Value = string.Empty;
                        chkIsActive.Checked = true;
                        btnDelete.Visible = false;
                        btnSave.Text = "Save";
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                    SyncId.Focus();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                DepName.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/DepartmentMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                int retdel = dp.delete(Convert.ToString(ViewState["depId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    DepName.Value = string.Empty;
                    SyncId.Value = string.Empty;
                    chkIsActive.Checked = true;
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    DepName.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    
                }
            }
            else
            {
                //      this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
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
            DepName.Value = string.Empty;
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}