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
    public partial class DSRNarrationMaster : System.Web.UI.Page
    {
        BAL.DSRNarrMaster.DSRNarrBAL dp = new BAL.DSRNarrMaster.DSRNarrBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["Id"] = parameter;
                FillDSRNarrationControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 19/may/2016- (For Optimization)
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
                BindNarrationType();
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                //DesName.Focus();
                // fillRepeter();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillDSRNarrationControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }

        private void BindNarrationType()
        {
            string Query = @"SELECT T1.Id,T1.NarrationType FROM MastDsrNarrationType AS T1";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

            if (dt != null && dt.Rows.Count > 0)
            {
                DdlNarrationType.DataTextField = "NarrationType";
                DdlNarrationType.DataValueField = "Id";
                DdlNarrationType.DataSource = dt;
                DdlNarrationType.DataBind();
                DdlNarrationType.Items.Insert(0, new ListItem("-- Select Narration Type --", "0"));
            }

            dt.Dispose();
        }
        private void fillRepeter()
        {
            string str = @"SELECT T1.Id,T1.Name,T1.SortOrder,T1.SyncId,CASE WHEN T1.Active = 1 
                                    THEN 'Yes' ELSE 'No'  END as Active ,T2.NarrationType FROM MastDSRNarr AS T1
                                    LEFT JOIN MastDsrNarrationType AS T2 ON T1.NarrType=T2.Id";

            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();

            depdt.Dispose();
        }

        private void FillDSRNarrationControls(int Id)
        {
            try
            {
                string Query = @"select * from MastDSRNarr where Id=" + Id;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {
                    // BindNarrationType();
                    Name.Text = dt.Rows[0]["Name"].ToString();
                    HiddenNarID.Value = dt.Rows[0]["NarrType"].ToString();
                    DdlNarrationType.SelectedValue = dt.Rows[0]["NarrType"].ToString();
                    SortOrder.Text = dt.Rows[0]["SortOrder"].ToString();
                    SyncId.Value = dt.Rows[0]["SyncId"].ToString();
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
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Name.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter DSR Name.');", true);
                    return;
                }
                if (Request.Form[DdlNarrationType.UniqueID] == "0")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Narration Type.');", true);
                    return;
                }
                if (SortOrder.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Sort Order.');", true);
                    return;
                }
                if (btnSave.Text == "Update")
                {
                    UpdateDSRNarraction();
                }
                else
                {
                    InsertDSRNarration();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertDSRNarration()
        {
            string str = @"select Count(*) from MastDSRNarr where Name='" + Name.Text + "'";
            string NarrationType = Request.Form[DdlNarrationType.UniqueID];
            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastDSRNarr where SyncId='" + SyncId.Value + "'";
                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }

                if (val1 == 0)
                {
                    int retsave = dp.Insert(Name.Text.ToUpper(), NarrationType, SortOrder.Text, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());

                    if (retsave != 0)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastDSRNarr set SyncId='" + retsave + "' where id=" + retsave + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        //         ClearControls();
                        Name.Text = string.Empty;
                        // DdlNarrationType.SelectedValue = "0";
                        HiddenNarID.Value = string.Empty;
                        SortOrder.Text = "";
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
            DdlNarrationType.SelectedValue = "0";
            SortOrder.Text = "";
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";

        }

        private void UpdateDSRNarraction()
        {
            string strupd = @"select Count(*) from MastDSRNarr where Name='" + Name.Text + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";
            string NarrationType = Request.Form[DdlNarrationType.UniqueID];
            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));

            if (valupd == 0)
            {
                int valupd1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string strupd1 = @"select Count(*) from MastDSRNarr where SyncId='" + SyncId.Value + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";
                    valupd1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                }
                if (valupd1 == 0)
                {
                    int retsave = dp.Update(ViewState["Id"].ToString(), Name.Text.ToUpper(), NarrationType, SortOrder.Text, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());
                    if (retsave == 1)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastDSRNarr set SyncId='" + ViewState["Id"] + "' where Id=" + Convert.ToInt32(ViewState["Id"]) + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        Name.Text = string.Empty;
                        // DdlNarrationType.SelectedValue = "0";
                        HiddenNarID.Value = string.Empty;
                        SortOrder.Text = "";
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
            Response.Redirect("~/DSRNarrationMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Convert.ToString(ViewState["Id"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    Name.Text = string.Empty;
                    //   DdlNarrationType.SelectedValue = "0";
                    SortOrder.Text = "";
                    HiddenNarID.Value = string.Empty;
                    SyncId.Value = string.Empty;
                    chkIsActive.Checked = true;
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Name.Focus();
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
            HiddenNarID.Value = string.Empty;
            //   DdlNarrationType.SelectedValue = "0";
            SortOrder.Text = "";
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}