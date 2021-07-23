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
using System.Text.RegularExpressions;
using System.IO;

namespace AstralFFMS
{
    public partial class ComplaintNatureMaster : System.Web.UI.Page
    {
        BAL.ComplaintNature.ComplaintNature dp = new BAL.ComplaintNature.ComplaintNature();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["Id"] = parameter;
                FillCompNatureControls(Convert.ToInt32(parameter));
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
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
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
                //DesName.Focus();
                // fillRepeter();
                //    Settings.Instance.BindDepartment(ddldept);
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillCompNatureControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }
        private void fillRepeter()
        {
            string str = @"select mcn.Id,mcn.Name,mcn.EmailTo,mcn.EmailCC,mcn.SyncId,mcn.NatureType,md.DepName,                                
                                CASE WHEN mcn.Active = 1 THEN 'Yes' ELSE 'No'  END as active 
                                from MastComplaintNature mcn left join MastDepartment md on mcn.DeptId=md.DepId";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();

            depdt.Dispose();
        }

        private void FillCompNatureControls(int Id)
        {
            try
            {
                string Query = @"select mcn.*,mdp.DepName from MastComplaintNature mcn left join MastDepartment mdp on mdp.DepId=mcn.DeptId where mcn.Id=" + Id;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {
                    ComplNatName.Value = dt.Rows[0]["Name"].ToString();
                    EmailTo.Value = dt.Rows[0]["EmailTo"].ToString();
                    EmailCC.Value = dt.Rows[0]["EmailCC"].ToString();
                    SyncId.Value = dt.Rows[0]["SyncId"].ToString();
                    //Added By- Abhishek 05-12-2015 UAT Point
                    if (dt.Rows[0]["DeptId"].ToString() != "")
                    {
                        HiddenUnderID.Value = dt.Rows[0]["DeptId"].ToString();
                    }
                    else
                    {
                        HiddenUnderID.Value = "0";
                    }

                    if (dt.Rows[0]["DepName"].ToString() != "")
                    {
                        ddldept.SelectedValue = dt.Rows[0]["DepName"].ToString();
                    }
                    else
                    {
                        ddldept.SelectedIndex = 0;
                    }

                    if (dt.Rows[0]["NatureType"].ToString() != "")
                    {
                        ddlNatreType.SelectedValue = dt.Rows[0]["NatureType"].ToString();
                    }
                    else
                    {
                        ddlNatreType.SelectedIndex = 0;
                    }
                    //End
                    if (Convert.ToBoolean(dt.Rows[0]["Active"]) == true)
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
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidEmailAddress(EmailTo.Value))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter valid email address.');", true);
                    return;
                }
                if (EmailCC.Value != "")
                {
                    if (!IsValidEmailAddress(EmailCC.Value))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter valid email address.');", true);
                        return;
                    }
                }

                if (btnSave.Text == "Update")
                {
                    UpdateCompNatMaster();
                }
                else
                {
                    InsertCompNatMaster();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertCompNatMaster()
        {
            string str = @"select Count(*) from MastComplaintNature where Name='" + ComplNatName.Value + "' and naturetype='" + ddlNatreType.SelectedValue + "'";
            string DeptId = Request.Form[ddldept.UniqueID];
            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastComplaintNature where SyncId='" + SyncId.Value + "'";
                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }

                if (val1 == 0)
                {

                    //            int retsave = dp.Insert(ComplNatName.Value.ToUpper(), EmailTo.Value, EmailCC.Value,SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());
                    //Added By Abhishek UAT Point 
                    int retsave = dp.Insert(ComplNatName.Value.ToUpper(), Convert.ToInt32(DeptId), ddlNatreType.SelectedValue, EmailTo.Value, EmailCC.Value, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());

                    if (retsave != 0)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastComplaintNature set SyncId='" + retsave + "' where id=" + retsave + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        ClearControls();
                        btnDelete.Visible = false;
                        ComplNatName.Focus();
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
                ComplNatName.Focus();
            }
        }

        private void ClearControls()
        {
            ComplNatName.Value = string.Empty;
            EmailTo.Value = string.Empty;
            EmailCC.Value = string.Empty;
            //Added
            // ddldept.SelectedIndex = 0;
            HiddenUnderID.Value = string.Empty;
            ddlNatreType.SelectedIndex = 0;
            //End
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";

        }

        private void UpdateCompNatMaster()
        {
            string strupd = @"select Count(*) from MastComplaintNature where Name='" + ComplNatName.Value + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";
            string DeptId = Request.Form[ddldept.UniqueID];
            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));

            if (valupd == 0)
            {
                int valupd1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string strupd1 = @"select Count(*) from MastComplaintNature where SyncId='" + SyncId.Value + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";

                    valupd1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                }



                if (valupd1 == 0)
                {

                    int retsave = dp.Update(ViewState["Id"].ToString(), ComplNatName.Value.ToUpper(), Convert.ToInt32(DeptId), ddlNatreType.SelectedValue, EmailTo.Value, EmailCC.Value, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());

                    if (retsave == 1)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastComplaintNature set SyncId='" + ViewState["Id"] + "' where id=" + Convert.ToInt32(ViewState["Id"]) + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        ClearControls();
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
                ComplNatName.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/ComplaintNatureMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                int retdel = dp.delete(Convert.ToString(ViewState["Id"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    ComplNatName.Focus();
                }
            }
            else
            {
                //   this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            ClearControls();
            fillRepeter();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}