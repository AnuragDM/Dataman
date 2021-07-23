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
using System.Text.RegularExpressions;

namespace AstralFFMS
{
    public partial class HolidayMaster : System.Web.UI.Page
    {
        BAL.HolidayMaster.HolidayMasterBAL dp = new BAL.HolidayMaster.HolidayMasterBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["Id"] = parameter;
                FillControls(Convert.ToInt32(parameter));
              //  HiddenUnderID.Value = string.Empty;
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
                txtDate.Attributes.Add("readonly", "readonly");
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
               // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
          //  btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                txtDate.Attributes.Add("readonly", "readonly");
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                //DesName.Focus();
                // fillRepeter();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["Id"] != null)
                {
                    FillControls(Convert.ToInt32(Request.QueryString["Id"]));
                }
            }
        }
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    // Register controls for event validation
        //    foreach (Control c in this.Controls)
        //    {
        //        this.Page.ClientScript.RegisterForEventValidation(
        //                c.UniqueID.ToString()
        //        );
        //    }
        //    base.Render(writer);
        //}
        private void fillRepeter()
        {
            string str = @"select T1.Id,Convert(varchar(15),CONVERT(date,T1.holiday_date,103),106) AS holiday_date,
                            T1.description,CASE WHEN T1.GeoTypeId = 1 THEN 'COUNTRY ' WHEN T1.GeoTypeId = 2 THEN 'STATE ' ELSE 'CITY'  END as GeoType 
                            ,T2.AreaName AS GeoName
                            ,T1.SyncId,                               
                             CASE WHEN T1.Active = 1 THEN 'Yes' ELSE 'No'  END as active 
                              from MastHoliday AS T1 LEFT JOIN MastArea AS T2 ON T1.Area_id=T2.AreaId";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();

            dt.Dispose();
        }

        private void FillControls(int Id)
        {
            try
            {
                string Query = @"select * from MastHoliday where Id=" + Id;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {
                    //txtDate.Text = string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["holiday_date"]);
                    txtDate.Text = string.Format("{0:dd/MMM/yyyy}", dt.Rows[0]["holiday_date"]);
                    description.Value = dt.Rows[0]["description"].ToString();
                    ddlGeoType.SelectedValue = dt.Rows[0]["GeoTypeId"].ToString();

                    //DataTable dt1 = new DataTable();
                    //if (dt.Rows[0]["GeoTypeId"].ToString() == "1")
                    //{
                    //    string str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='COUNTRY' ORDER BY T1.AreaName";
                    //    dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    //}
                    //else if (dt.Rows[0]["GeoTypeId"].ToString() == "2")
                    //{
                    //    string str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='STATE' ORDER BY T1.AreaName";
                    //    dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    //}
                    //else if (dt.Rows[0]["GeoTypeId"].ToString() == "3")
                    //{
                    //    string str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='CITY' ORDER BY T1.AreaName";
                    //    dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    //}

                    //ddlGeoName.DataSource = dt1;
                    //ddlGeoName.DataTextField = "AreaName";
                    //ddlGeoName.DataValueField = "AreaId";
                    //ddlGeoName.DataBind();
                    //ddlGeoName.Items.Insert(0, new ListItem("Select", "0")); 
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FillDDL", "FillDDL();", true);
                    HiddenUnderID.Value = dt.Rows[0]["Area_id"].ToString();
                    FillGeoName();
                   
                  //  ddlGeoName.SelectedValue = dt.Rows[0]["Area_id"].ToString();
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
                if (txtDate.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Holiday Date.');", true);
                    return;
                }
                if (description.Value == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Description.');", true);
                    return;
                }
                if (ddlGeoType.SelectedValue == "0")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Geo Type.');", true);
                    return;
                }
                if (ddlGeoName.SelectedValue == "0")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Geo Name.');", true);
                    return;
                }               
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
            string GeoName = Request.Form[ddlGeoName.UniqueID];
            string str = @"select Count(*) from MastHoliday AS T1 where T1.description='" + description.Value.ToUpper() + "' and  Convert(VARCHAR(10),T1.holiday_date,103)='" + Convert.ToDateTime(txtDate.Text).ToString("dd/M/yyyy") + "' AND T1.Area_id=" + Convert.ToInt32(GeoName) + "";

            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastHoliday where SyncId='" + SyncId.Value + "'";
                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }

                if (val1 == 0)
                {
                    int retsave = dp.Insert(txtDate.Text.Trim(), description.Value.ToUpper(), ddlGeoType.SelectedItem.Value, GeoName, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());
                    if (retsave != 0)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastHoliday set SyncId='" + retsave + "' where id=" + retsave + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        ClearControls();
                        btnDelete.Visible = false;
                        txtDate.Focus();
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
                txtDate.Focus();
            }
        }

        private void ClearControls()
        {           
            txtDate.Text = "";
            description.Value = string.Empty;
            ddlGeoType.SelectedValue = "0";
          //  ddlGeoName.SelectedValue = "0";    
            HiddenUnderID.Value = string.Empty;
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";

        }

        private void Update()
        {
            string GeoName = Request.Form[ddlGeoName.UniqueID];
            string strupd = @"select Count(*) from MastHoliday AS T1 where T1.description='" + description.Value.ToUpper() + "' and  Convert(VARCHAR(10),T1.holiday_date,103)='" + Convert.ToDateTime(txtDate.Text).ToString("dd/M/yyyy") + "' AND T1.Area_id=" + Convert.ToInt32(GeoName) + " and T1.Id<>" + Convert.ToInt32(ViewState["Id"]) + "";

            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));

            if (valupd == 0)
            {
                int valupd1 = 0;
                if (SyncId.Value != "" || SyncId.Value != string.Empty)
                {
                    string strupd1 = @"select Count(*) from MastHoliday where SyncId='" + SyncId.Value + "' and Id<>" + Convert.ToInt32(ViewState["Id"]) + "";
                    valupd1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                }
                if (valupd1 == 0)
                {
                    int retsave = dp.Update(ViewState["Id"].ToString(), txtDate.Text.Trim(), description.Value.ToUpper(), ddlGeoType.SelectedItem.Value, GeoName, SyncId.Value.ToUpper(), chkIsActive.Checked, BusinessLayer.Settings.GetUTCTime());
                    if (retsave == 1)
                    {
                        if (SyncId.Value == "")
                        {
                            string syncid = "update MastHoliday set SyncId='" + ViewState["Id"] + "' where id=" + ViewState["Id"] + "";
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
                txtDate.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/HolidayMaster.aspx");
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
                    txtDate.Focus();
                }              
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
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
        //protected void ddlGeoType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    HiddenUnderID.Value = "";
        //    FillGeoName();

        //}
        private void FillGeoName()
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindGeoName", "BindGeoName(0);", true);
               
                //DataTable dt = new DataTable();
                //if(ddlGeoType.SelectedItem.Value=="1")
                //{
                //    string str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='COUNTRY' ORDER BY T1.AreaName";
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //}
                //else if (ddlGeoType.SelectedItem.Value == "2")
                //{
                //    string str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='STATE' ORDER BY T1.AreaName";
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //}
                //else if (ddlGeoType.SelectedItem.Value == "3")
                //{
                //    string str = @"SELECT DISTINCT T1.AreaId,T1.AreaName FROM MastArea AS T1 WHERE T1.AreaType='CITY' ORDER BY T1.AreaName";
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //}

                //ddlGeoName.DataSource = dt;
                //ddlGeoName.DataTextField = "AreaName";
                //ddlGeoName.DataValueField = "AreaId";
                //ddlGeoName.DataBind();
                //ddlGeoName.Items.Insert(0, new ListItem("Select", "0"));            
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}