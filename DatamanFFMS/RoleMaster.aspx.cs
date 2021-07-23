using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class RoleMaster : System.Web.UI.Page
    {
        BAL.MastRole.mastroleAll mstall = new BAL.MastRole.mastroleAll();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Visible = false;
                //DepName.Focus();
                // fillRepeter();
                mainDiv.Style.Add("display", "block");
            }
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["RoleId"] = parameter;
                FillRoleControls(Convert.ToInt32(parameter));
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
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";

        }


        private void FillRoleControls(int roleId)
        {
            try
            { //Ankita - 20/may/2016- (For Optimization)
                //string Query = @"select * from MastRole where RoleId=" + roleId;
                string Query = @"select RoleName,RoleType,isAdmin from MastRole where RoleId=" + roleId;
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {
                    RoleName.Value = dt.Rows[0]["RoleName"].ToString();
                    ddlDashboard.SelectedValue = dt.Rows[0]["RoleType"].ToString();
                    if (Convert.ToBoolean(dt.Rows[0]["isAdmin"]) == true)
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
            }
        }
        private void fillRepeter()
        {
            try
            {
                string mastRolequery = @"select RoleId, RoleName,isAdmin,RoleType,CASE WHEN isAdmin = 1 
        THEN 'Yes' ELSE 'No'  END as admin from MastRole order by RoleName ";
                DataTable mastRoledt = DbConnectionDAL.GetDataTable(CommandType.Text, mastRolequery);
                rpt.DataSource = mastRoledt;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateRole();
                }
                else
                {
                    InsertRole();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertRole()
        {
            try
            {
                string str = @"select Count(*) from MastRole where RoleName='" + RoleName.Value + "'";

                int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                if (val == 0)
                {
                    int retsave = mstall.Insert(RoleName.Value.ToUpper(), chkIsActive.Checked, ddlDashboard.SelectedValue);
                    if (retsave != 1)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        ClearControls();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exist');", true);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void ClearControls()
        {
            try
            {
                RoleName.Value = string.Empty;
                chkIsActive.Checked = false;
                ddlDashboard.SelectedIndex = 0;
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void UpdateRole()
        {
            try
            {
                string strupd = @"select Count(*) from MastRole where RoleName='" + RoleName.Value.ToUpper() + "' and RoleId<>" + Convert.ToInt32(ViewState["RoleId"]) + "";

                int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));
                if (valupd == 0)
                {
                    string isAdmin = "";
                    if (chkIsActive.Checked == true)
                    {
                        isAdmin = "1";
                    }
                    else
                    {
                        isAdmin = "0";
                    }
                    string mastRoleUpdQuery = @"update MastRole set RoleName='" + RoleName.Value.ToUpper() + "',isAdmin=" + isAdmin + ",RoleType='" + ddlDashboard.SelectedValue + "' where RoleId=" + ViewState["RoleId"];
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, mastRoleUpdQuery);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    ClearControls();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RoleMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);

                //string mastroledelQry = @"delete from MastRole where RoleId=" + Convert.ToInt32(ViewState["RoleId"]) + "";
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, mastroledelQry);
                int retdel = mstall.delete(Convert.ToString(ViewState["RoleId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record is in use');", true);
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
    }
}