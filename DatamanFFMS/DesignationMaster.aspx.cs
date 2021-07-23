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
    public partial class DesignationMaster : System.Web.UI.Page
    {
        BAL.DesignationMaster.DesignationBAL dp = new BAL.DesignationMaster.DesignationBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["desId"] = parameter;
                FillDesigControls(Convert.ToInt32(parameter));
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
                // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
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
                //DesName.Focus();
                // fillRepeter();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["desId"] != null)
                {
                    FillDesigControls(Convert.ToInt32(Request.QueryString["desId"]));
                }
                else
                {
                    //  BindGrade(0);
                }
            }
        }
        private void BindGrade(int GradeId)
        {
            string str = "";
            if (GradeId > 0)
                str = "select Id,Name from MastGrade where (Active='1' or Id in (" + GradeId + ")) order by name";
            else
                str = "select Id,Name from MastGrade where Active='1' order by name";
            fillDDLDirect(ddlgrade, str, "Id", "Name", 1);
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

        //changes for V2 14-07-2021---------- anurag
        private void fillRepeter()
        {
            string str = @"select DesId,DesName,MastDesignation.SyncId,isnull(DesType,'')DesType,isnull(SortType,'')SortType,CASE WHEN MastDesignation.Active = 1 THEN 'Yes' ELSE 'No'  END as active,MastGrade.Name As Grade,CASE WHEN EligibleForConveyance = 1 THEN 'Yes' ELSE 'No'  END as EligibleForConveyance  from MastDesignation left join MastGrade on MastDesignation.GradeId=MastGrade.Id order by DesName";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();

            depdt.Dispose();
        }

        //changes for V2 14-07-2021---------- anurag
        private void FillDesigControls(int desId)
        {
            try
            {
                string Query = @"select * from MastDesignation where desId=" + desId;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {
                    DesName.Text = dt.Rows[0]["DesName"].ToString();
                    SyncId.Value = dt.Rows[0]["SyncId"].ToString();
                    // BindGrade(Convert.ToInt32(dt.Rows[0]["GradeId"]));
                    HiddenGradeUnderID.Value = dt.Rows[0]["GradeId"].ToString();
                    ddlgrade.SelectedValue = dt.Rows[0]["GradeId"].ToString();

                    HiddenType.Value = dt.Rows[0]["DesType"].ToString();
                    ddltype.SelectedValue = dt.Rows[0]["DesType"].ToString();
                    SrtTyp.Value = dt.Rows[0]["SortType"].ToString();

                    if (Convert.ToBoolean(dt.Rows[0]["EligibleForConveyance"]) == true)
                    { chkElgbleForConv.Checked = true; }
                    else { chkElgbleForConv.Checked = false; }
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
                if (btnSave.Text == "Update")
                {
                    UpdateDesignation();
                }
                else
                {
                    InsertDesignation();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        //changes for V2 14-07-2021---------- anurag
        private void InsertDesignation()
        {
            string str = @"select Count(*) from MastDesignation where DesName='" + DesName.Text + "'";
            string GradeID = Request.Form[HiddenGradeUnderID.UniqueID];
            //string DesigTyp = Request.Form[HiddenType.UniqueID];

            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastDesignation where SyncId='" + SyncId.Value + "'";
                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }
                if (val1 == 0)
                {
                    int val2 = 0;
                    if (ddltype.SelectedValue != "" && ddltype.Text != String.Empty && SrtTyp.Value != "" && SrtTyp.Value != String.Empty)
                    {
                        string str1 = @"select Count(*) from MastDesignation where DesType='" + ddltype.SelectedValue + "' and SortType='" + SrtTyp.Value + "'";
                        val2 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                    }
                    if (val2 == 0)
                    {
                        //  int retsave = dp.Insert(DesName.Text.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime(), chkElgbleForConv.Checked, Convert.ToInt32(ddlgrade.SelectedValue));
                        int retsave = dp.Insert(DesName.Text.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime(), chkElgbleForConv.Checked, Convert.ToInt32(GradeID), ddltype.SelectedValue.ToUpper(), Convert.ToInt32(SrtTyp.Value));
                        if (retsave != 0)
                        {
                            if (SyncId.Value == "")
                            {
                                string syncid = "update MastDesignation set SyncId='" + retsave + "' where Desid=" + retsave + "";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                            }
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            //ClearControls();
                            DesName.Text = string.Empty;
                            SyncId.Value = string.Empty;
                            HiddenGradeUnderID.Value = string.Empty;
                            chkIsActive.Checked = true;
                            btnDelete.Visible = false;
                            ddltype.SelectedIndex = 0;
                            SrtTyp.Value = string.Empty;
                            // ddlgrade.SelectedIndex = 0;
                            chkElgbleForConv.Checked = false;
                            DesName.Focus();
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Sort Type Exists');", true);
                        SrtTyp.Focus();
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
                DesName.Focus();
            }
        }

        private void ClearControls()
        {
            DesName.Text = string.Empty;
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            HiddenGradeUnderID.Value = string.Empty;
            chkElgbleForConv.Checked = false;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
        }

        //changes for V2 14-07-2021---------- anurag
        private void UpdateDesignation()
        {
            string strupd = @"select Count(*) from MastDesignation where DesName='" + DesName.Text + "' and desId<>" + Convert.ToInt32(ViewState["desId"]) + "";
            string GradeID = Request.Form[HiddenGradeUnderID.UniqueID];

            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));

            if (valupd == 0)
            {
                int valupd1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string strupd1 = @"select Count(*) from MastDesignation where SyncId='" + SyncId.Value + "' and desId<>" + Convert.ToInt32(ViewState["desId"]) + "";

                    valupd1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                }
                if (valupd1 == 0)
                {
                    int valupd2 = 0;

                    string strupd1 = @"select COUNT(*) FROM MastSalesRep where UnderId IN (select SMId FROM MastSalesRep where UserId in(select ID from MastLogin where DesigId in (select DesId from MastDesignation where DesId=" + Convert.ToInt32(ViewState["desId"]) + " and DesType IS NOT NULL and SortType IS NOT NULL)))";

                    valupd2 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                    if (valupd2 == 0)
                    {
                        int valupd3 = 0;
                        string strupd2 = @"select Count(*) from MastDesignation where DesType='" + ddltype.SelectedValue + "' and SortType='" + SrtTyp.Value + "' and desId<>" + Convert.ToInt32(ViewState["desId"]) + "";

                        valupd3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd2));
                        if (valupd3 == 0)
                        {
                            // int retsave = dp.Update(ViewState["desId"].ToString(), DesName.Text.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime(),chkElgbleForConv.Checked,Convert.ToInt32(ddlgrade.SelectedValue));
                            int retsave = dp.Update(ViewState["desId"].ToString(), DesName.Text.ToUpper(), SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime(), chkElgbleForConv.Checked, Convert.ToInt32(GradeID), ddltype.SelectedValue.ToUpper(), Convert.ToInt32(SrtTyp.Value));
                            if (retsave == 1)
                            {
                                if (SyncId.Value == "")
                                {
                                    string syncid = "update MastDesignation set SyncId='" + ViewState["desId"] + "' where Desid=" + Convert.ToInt32(ViewState["desId"]) + "";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                                }
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                                DesName.Text = string.Empty;
                                SyncId.Value = string.Empty;
                                chkIsActive.Checked = true;
                                HiddenGradeUnderID.Value = string.Empty;
                                ddltype.SelectedIndex = 0;
                                SrtTyp.Value = string.Empty;
                                // ddlgrade.SelectedIndex = 0;
                                chkElgbleForConv.Checked = false;
                                btnDelete.Visible = false;
                                btnSave.Text = "Save";
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Sort Type Exists');", true);
                            SrtTyp.Focus();
                        }


                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Sort Type cannot be changed as this Designation is used');", true);
                        SrtTyp.Focus();
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
                DesName.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/DesignationMaster.aspx");
        }

        //changes for V2 14-07-2021---------- anurag
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int valupd2 = 0;
                string strupd1 = @"select COUNT(*) FROM MastSalesRep where UnderId IN (select SMId FROM MastSalesRep where UserId in(select ID from MastLogin where DesigId in (select DesId from MastDesignation where DesId=" + Convert.ToInt32(ViewState["desId"]) + " )))";

                valupd2 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                if (valupd2 == 0)
                {
                    //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                    int retdel = dp.delete(Convert.ToString(ViewState["desId"]));
                    if (retdel == 1)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                        DesName.Text = string.Empty;
                        SyncId.Value = string.Empty;
                        chkIsActive.Checked = true;
                        //  ddlgrade.SelectedIndex = 0;
                        chkElgbleForConv.Checked = false;
                        btnDelete.Visible = false;
                        btnSave.Text = "Save";
                        DesName.Focus();
                        HiddenGradeUnderID.Value = "";
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Sort Type cannot be delete as this Designation is used');", true);
                        SrtTyp.Focus();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Sort Type cannot be delete as this Designation is used');", true);
                    SrtTyp.Focus();
                }

            }
            else
            {
                //      this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // BindGrade(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            DesName.Text = string.Empty;
            SyncId.Value = string.Empty;
            HiddenGradeUnderID.Value = string.Empty;
            // ddlgrade.SelectedIndex = 0;
            chkElgbleForConv.Checked = false;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}