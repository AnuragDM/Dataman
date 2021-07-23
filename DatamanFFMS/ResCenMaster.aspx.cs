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
    public partial class ResCenMaster : System.Web.UI.Page
    {
        BAL.ResponsibilityCenter.ResCenter dp = new BAL.ResponsibilityCenter.ResCenter();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["ResCenId"] = parameter;
                FillResCenterControls(Convert.ToInt32(parameter));
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
                //DesName.Focus();
                // fillRepeter();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["ResCenId"] != null)
                {
                    FillResCenterControls(Convert.ToInt32(Request.QueryString["ResCenId"]));
                }
            }
        }
        private void fillRepeter()
        {
            string str = @"select ResCenId,ResCenName,PlantCode,OrderType,DivisionCode,SyncId,
                                CASE WHEN PlantType = 'W' THEN 'Warehouse' ELSE 'Factory'  END as PlantType ,
                                CASE WHEN Active = 1 THEN 'Yes' ELSE 'No'  END as active 
                                from MastResCentre";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
            depdt.Dispose();
        }

        private void FillResCenterControls(int ResCenId)
        {
            try
            {
                string Query = @"select ResCenName,PlantCode,OrderType,DivisionCode,PlantType,SyncId,Active from MastResCentre where ResCenId=" + ResCenId;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {   
                    txtPlantName.Text = dt.Rows[0]["ResCenName"].ToString();
                    txtPlantCode.Text = dt.Rows[0]["PlantCode"].ToString();
                    txtOrderType.Text = dt.Rows[0]["OrderType"].ToString();
                    txtDivisionCode.Text = dt.Rows[0]["DivisionCode"].ToString();
                    if (dt.Rows[0]["PlantType"].ToString() == "W")
                    { radPlantType.Items[0].Selected = true; }
                    else if (dt.Rows[0]["PlantType"].ToString() == "F")
                    { radPlantType.Items[0].Selected = true; }
                    SyncId.Value = dt.Rows[0]["SyncId"].ToString();
                    if (Convert.ToBoolean(dt.Rows[0]["Active"]) == true)
                    { chkIsActive.Checked = true; }
                    else
                    { chkIsActive.Checked = false; }
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
                string PlantType = "";
                if (radPlantType.Items[0].Selected)
                { PlantType = "W"; }
                else if (radPlantType.Items[1].Selected)
                { PlantType = "F"; }
                if (txtPlantName.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Plant Name.');", true);
                    return;
                }
                if (txtPlantCode.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Plant Code.');", true);
                    return;
                }
                if (PlantType=="")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Plant Type.');", true);
                    return;
                }
                if (txtOrderType.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Order Type.');", true);
                    return;
                }
                if (txtDivisionCode.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Division Code.');", true);
                    return;
                }
                if (btnSave.Text == "Update")
                {
                    UpdateResCenMaster();
                }
                else
                {
                    InsertResCenMaster();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertResCenMaster()
        {
            string str = @"select Count(*) from MastResCentre where ResCenName='" + txtPlantName.Text + "'";

            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastResCentre where SyncId='" + SyncId.Value + "'";
                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }

                if (val1 == 0)
                {
                    string PlantType = "";
                    if(radPlantType.Items[0].Selected)
                    { PlantType = "W"; }
                    else if (radPlantType.Items[1].Selected)
                    { PlantType = "F"; }
                    int retsave = dp.Insert(txtPlantName.Text.ToUpper(), txtPlantCode.Text.ToUpper(), PlantType, txtOrderType.Text.ToUpper(), txtDivisionCode.Text, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());
                    if (retsave != 0)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastResCentre set SyncId='" + retsave + "' where ResCenId=" + retsave + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        ClearControls();                       
                        btnDelete.Visible = false;
                        txtPlantName.Focus();
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
                txtPlantName.Focus();
            }
        }

        private void ClearControls()
        {
            txtPlantName.Text = string.Empty;
            txtPlantCode.Text = string.Empty;
            txtOrderType.Text = string.Empty;
            txtDivisionCode.Text = string.Empty;
            if (radPlantType.Items[0].Selected)
            { radPlantType.Items[0].Selected = false; }
            else if (radPlantType.Items[1].Selected)
            { radPlantType.Items[0].Selected = false; }
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";           
        }

        private void UpdateResCenMaster()
        {
            string strupd = @"select Count(*) from MastResCentre where ResCenName='" + txtPlantName.Text + "' and ResCenId<>" + Convert.ToInt32(ViewState["ResCenId"]) + "";
            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));
            if (valupd == 0)
            {
                int valupd1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string strupd1 = @"select Count(*) from MastResCentre where SyncId='" + SyncId.Value + "' and ResCenId<>" + Convert.ToInt32(ViewState["ResCenId"]) + "";
                    valupd1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                }
                if (valupd1 == 0)
                {
                    string PlantType = "";
                    if (radPlantType.Items[0].Selected)
                    {
                        PlantType = "W";
                    }
                    else if (radPlantType.Items[1].Selected)
                    {
                        PlantType = "F";
                    }
                    int retsave = dp.Update(ViewState["ResCenId"].ToString(), txtPlantName.Text.ToUpper(), txtPlantCode.Text.ToUpper(), PlantType, txtOrderType.Text.ToUpper(), txtDivisionCode.Text, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());

                    if (retsave == 1)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastResCentre set SyncId='" + ViewState["ResCenId"] + "' where ResCenId=" + ViewState["ResCenId"] + "";
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
                txtPlantName.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/ResCenMaster.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                int retdel = dp.delete(Convert.ToString(ViewState["ResCenId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    txtPlantName.Text = string.Empty;
                    txtPlantCode.Text = string.Empty;
                    txtOrderType.Text = string.Empty;
                    txtDivisionCode.Text = string.Empty;
                    if (radPlantType.Items[0].Selected)
                    { radPlantType.Items[0].Selected = false; }
                    else if (radPlantType.Items[1].Selected)
                    { radPlantType.Items[0].Selected = false; }
                    SyncId.Value = string.Empty;
                    chkIsActive.Checked = true;
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    txtPlantName.Focus();
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
            fillRepeter();
            txtPlantName.Text = string.Empty;
            txtPlantCode.Text = string.Empty;
            txtOrderType.Text = string.Empty;
            txtDivisionCode.Text = string.Empty;
            if (radPlantType.Items[0].Selected)
            { radPlantType.Items[0].Selected = false; }
            else if (radPlantType.Items[1].Selected)
            { radPlantType.Items[0].Selected = false; }
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
    }
}