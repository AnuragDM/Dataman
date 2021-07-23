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
    public partial class UserMaster : System.Web.UI.Page
    {
        BAL.UserMaster.userall usall = new BAL.UserMaster.userall();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {           
           
            if (!IsPostBack)
            {
                chkIsAdmin.Checked = true;
                btnDelete.Visible = false;
                //Settings.Instance.BindRole(ddlRole);
                //Settings.Instance.BindDepartment(ddldept);
                //Settings.Instance.BindDesignation(ddldesg);
            }
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["Id"] = parameter;
                FillUserControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }

            btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";

        }

        private void FillUserControls(int loginID)
        {
            try
            {
                //var obj = (from r in context.MastLogins.OrderBy(x => x.Name)
                //          let admin = ""
                //          join role in context.MastRoles on r.RoleId equals role.RoleId into ps1
                //          from role in ps1.DefaultIfEmpty()
                //          join dept in context.MastDepartments on r.DeptId equals dept.DepId into dp
                //          from dept in dp.DefaultIfEmpty()
                //          join desg in context.MastDesignations on r.DesigId equals desg.DesId into dsg
                //          from desg in dsg.DefaultIfEmpty()
                //          select new { r.Id, r.Name, r.Email, r.Pwd, role.RoleId, role.RoleName, r.Active, role.isAdmin,r.DeptId,r.DesigId,desg.DesName,dept.DepName, admin = r.Active == true ? "Yes" : "No" }).ToList();

                string mastRolequery = @"select Id, Name,EmpName,EmpSyncId,Active,Pwd,RoleId,Email,DeptId,DesigId,CostCentre,CASE WHEN Active = 1 
        THEN 'Yes' ELSE 'No'  END as admin from MastLogin where Id=" + loginID + "";
                DataTable mastlogindt = DbConnectionDAL.GetDataTable(CommandType.Text, mastRolequery);
                if (mastlogindt.Rows.Count > 0)
                {
                    UserName.Value = mastlogindt.Rows[0]["Name"].ToString();
                    //           PasswordText.TextMode = TextBoxMode.SingleLine; 
                    PasswordText.Attributes["value"] = mastlogindt.Rows[0]["Pwd"].ToString();

                    ConfPasswordText.Attributes["value"] = mastlogindt.Rows[0]["Pwd"].ToString();
                    email.Value = mastlogindt.Rows[0]["Email"].ToString();
                    chkIsAdmin.Checked = Convert.ToBoolean(mastlogindt.Rows[0]["Active"]);

                    HiddenRoleID.Value = mastlogindt.Rows[0]["RoleId"].ToString();
                    ddlRole.SelectedValue = mastlogindt.Rows[0]["RoleId"].ToString();
                    EmpName.Value = mastlogindt.Rows[0]["EmpName"].ToString();
                    CostCentre1.Text = mastlogindt.Rows[0]["CostCentre"].ToString();

                    //Added 08-12-2015
                    EmpSyncId.Value = mastlogindt.Rows[0]["EmpSyncId"].ToString();
                    //End

                    string roleNameQry = @"select RoleName from MastRole where RoleId=" + mastlogindt.Rows[0]["RoleId"] + "";
                    string RoleName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, roleNameQry));
                    if (RoleName != "Distributor")
                    {
                        ddldept.Enabled = true;
                        ddldesg.Enabled = true;
                        if (mastlogindt.Rows[0]["DeptId"] != DBNull.Value)
                        {
                            HiddenDeptID.Value = mastlogindt.Rows[0]["DeptId"].ToString();
                            ddldept.SelectedValue = mastlogindt.Rows[0]["DeptId"].ToString();
                        }
                        else
                        {
                            ddldept.SelectedIndex = 0;
                        }
                        if (mastlogindt.Rows[0]["DesigId"] != DBNull.Value)
                        {
                            HiddenDesID.Value = mastlogindt.Rows[0]["DesigId"].ToString();
                            ddldesg.SelectedValue = mastlogindt.Rows[0]["DesigId"].ToString();
                        }
                        else
                        {
                            ddldesg.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ddldept.Enabled = false;
                        ddldesg.Enabled = false;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateMastLogin();
                }
                else
                {
                    InsertMastLogin();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertMastLogin()
        {
            string RoleID = Request.Form[HiddenRoleID.UniqueID], DeptID = Request.Form[HiddenDeptID.UniqueID], DesnID = Request.Form[HiddenDesID.UniqueID];
            try
            {
                if (UserName.Value != "")
                {
                    string str = @"select Count(*) from MastLogin where Name='" + UserName.Value.ToUpper() + "'";

                    int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                    //        MastLogin LogI = context.MastLogins.FirstOrDefault(u => u.Name.ToUpper() == UserName.ToUpper());//chkIsAdmin.Checked
                    if (val == 0)
                    {
                        int retsave = usall.Insert(UserName.Value, PasswordText.Text, email.Value, Convert.ToInt32(RoleID), chkIsAdmin.Checked, DeptID, DesnID, EmpName.Value, Convert.ToDecimal(CostCentre1.Text), EmpSyncId.Value);
                        if (retsave == -2)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate EmpSyncId Exists');", true);
                        }
                        if (retsave == -3)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email Id Exists');", true);
                        }
                        if (retsave > 0)
                        {
                            if (EmpSyncId.Value == "")
                            {
                                string syncid = "update MastLogin set EmpSyncId='" + retsave + "' where Id=" + retsave + "";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                            }
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            ClearControls();
                        }
                        
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exist');", true);
                    }

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void UpdateMastLogin()
        {
            try
            {
                string RoleID=Request.Form[HiddenRoleID.UniqueID],DeptID=Request.Form[HiddenDeptID.UniqueID],DesnID=Request.Form[HiddenDesID.UniqueID];
                if (UserName.Value != "")
                {
                    string strupd = @"select Count(*) from MastLogin where Name='" + UserName.Value.ToUpper() + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";

                    int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));
                    if (valupd == 0)
                    {
                        int retsave = usall.Update(Convert.ToInt32(ViewState["Id"]), UserName.Value.ToUpper(), PasswordText.Text, email.Value, Convert.ToInt32(RoleID), chkIsAdmin.Checked, DeptID, DesnID, EmpName.Value, Convert.ToDecimal(CostCentre1.Text), EmpSyncId.Value);
                        if (retsave == -2)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate EmpSyncId Exists');", true);
                        }
                        if (retsave == -3)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email Id Exists');", true);
                        }
                        if (retsave == 1)
                        {
                            if (EmpSyncId.Value == "")
                            {
                                string syncid = "update MastLogin set EmpSyncId='" + ViewState["Id"] + "' where Id=" + ViewState["Id"] + "";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                            }
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                            ClearControls();
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UserMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);

                //           string mastlogindelQry = @"delete from MastLogin where Id=" + Convert.ToInt32(ViewState["Id"]) + "";
                //           DbConnectionDAL.ExecuteNonQuery(CommandType.Text, mastlogindelQry);
                int retdel = usall.delete(Convert.ToString(ViewState["Id"]));
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

        private void ClearControls()
        {
            try
            {
                UserName.Value = string.Empty;
                PasswordText.Attributes["value"] = string.Empty;
                ConfPasswordText.Attributes["value"] = string.Empty;
                EmpName.Value = string.Empty;
                chkIsAdmin.Checked = true;
                email.Value = string.Empty;
                //ddlRole.SelectedIndex = 0;
                //ddldept.SelectedIndex = 0;
                //ddldesg.SelectedIndex = 0;
                HiddenDesID.Value = string.Empty;
                HiddenDeptID.Value = string.Empty;
                HiddenRoleID.Value = string.Empty;
                //Added 08-12-2015
                EmpSyncId.Value = string.Empty;
                //End

                btnDelete.Visible = false;
                ddldept.Enabled = true;
                ddldesg.Enabled = true;
                btnSave.Text = "Save";
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
                string query = @"select r.Id, r.Name, r.Email, r.Pwd,r.EmpSyncId, role.RoleId, role.RoleName, r.Active,r.EmpName, role.isAdmin,r.DeptId,r.DesigId,desg.DesName,dept.DepName,CASE WHEN r.Active = 1 
        THEN 'Yes' ELSE 'No'  END as admin from MastLogin r left join MastRole role on r.RoleId=role.RoleId left join MastDepartment dept on r.DeptId=dept.DepId 
              left join MastDesignation desg on r.DesigId=desg.DesId order by r.Name";
                DataTable mastlogindt1 = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (mastlogindt1.Rows.Count > 0)
                {
                    rpt.DataSource = mastlogindt1;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = mastlogindt1;
                    rpt.DataBind();
                }

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

        protected void btnbacksales_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SalesPersons.aspx");
        }

        //protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlRole.SelectedItem.Text == "Distributor")
        //    {
        //        //     ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
        //        ddldesg.SelectedIndex = 0;
        //        ddldept.SelectedIndex = 0;
        //        ddldept.Enabled = false;
        //        ddldesg.Enabled = false;
        //    }
        //    else
        //    {
        //        ddldept.Enabled = true;
        //        ddldesg.Enabled = true;
        //    }
        //}
    }
}