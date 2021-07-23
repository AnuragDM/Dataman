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
    public partial class SegmentMaster : System.Web.UI.Page
    {
        BAL.SegmentMaster.SegmentBAL dp = new BAL.SegmentMaster.SegmentBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["Id"] = parameter;
                FillControls(Convert.ToInt32(parameter));
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
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }

            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                //Name.Focus();
                // fillRepeter();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }
        private void fillRepeter()
        {
            string str = @"SELECT Id,Name,SyncCode,CASE WHEN Active = 1 
                            THEN 'Yes' ELSE 'No'  END as active from MastItemSegment";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
            depdt.Dispose();
        }

        private void FillControls(int Id)
        {
            try
            {
                string Query = @"select Name,SyncCode,Active from MastItemSegment where Id=" + Id;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {
                    Name.Text = dt.Rows[0]["Name"].ToString();
                    SyncId.Value = dt.Rows[0]["SyncCode"].ToString();
                    if (dt.Rows[0]["Active"] != null)
                    {
                        if (Convert.ToBoolean(dt.Rows[0]["Active"]) == true)
                        {
                            chkIsActive.Checked = true;
                        }
                        else
                        {
                            chkIsActive.Checked = false;
                        }
                    }
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    Update();
                }
                else
                {
                    Insert();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void Insert()
        {
            string str = @"select Count(*) from MastItemSegment where Name='" + Name.Text + "'";

            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastItemSegment where SyncCode='" + SyncId.Value + "'";
                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }

                if (val1 == 0)
                {
                    int retsave = dp.Insert(Name.Text.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());

                    if (retsave != 0)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastItemSegment set SyncCode='" + retsave + "' where id=" + retsave + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);                       
                        Name.Text = string.Empty;
                        SyncId.Value = string.Empty;
                        chkIsActive.Checked = true;
                        btnDelete.Visible = false;
                        Name.Focus();
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
                Name.Focus();
            }
        }

        private void ClearControls()
        {
            Name.Text = string.Empty;
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
           
        }

        private void Update()
        {
            string strupd = @"select Count(*) from MastItemSegment where Name='" + Name.Text + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";

            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));

            if (valupd == 0)
            {
                int valupd1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string strupd1 = @"select Count(*) from MastItemSegment where SyncCode='" + SyncId.Value + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";
                    valupd1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                }
                if (valupd1 == 0)
                {
                    int retsave = dp.Update(ViewState["Id"].ToString(), Name.Text.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());
                    if (retsave == 1)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastItemSegment set SyncCode='" + ViewState["Id"].ToString() + "' where id=" + Convert.ToInt32(ViewState["Id"]) + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        Name.Text = string.Empty;
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
                Name.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/SegmentMaster.aspx");
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
                    Name.Text = string.Empty;
                    SyncId.Value = string.Empty;
                    chkIsActive.Checked = true;
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Name.Focus();
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
            Name.Text = string.Empty;
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}