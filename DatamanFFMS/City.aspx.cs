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
using System.Text;


namespace AstralFFMS
{
    public partial class City : System.Web.UI.Page
    {
        LocationBAL LB = new LocationBAL();
        string parameter = "";
        bool _exportp = false; 
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnSave.UniqueID;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["AreaId"] = parameter;
                FillLocControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 18/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);          
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            btnExport.CssClass = "btn btn-primary";
            btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            _exportp = Convert.ToBoolean(SplitPerm[4]);
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
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
            if (!IsPostBack)
            {
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
               BindCityType(); BindCityConveyanceType();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["AreaId"] != null)
                {
                    FillLocControls(Convert.ToInt32(Request.QueryString["AreaId"]));
                }
                else { 
                   /// BindParent(0);
                }
            }
        }
        private void BindParent(int DistrictId)
        {
            string strQ = "";
            if (DistrictId > 0)
                strQ = "select AreaID,AreaName from mastarea where AreaType='District' and (Active='1' or Areaid in (" + DistrictId + "))  order by AreaName";

            else
                strQ = "select AreaID,AreaName from mastarea where AreaType='District' and Active='1' order by AreaName";
            fillDDLDirect(ddlParentLoc, strQ, "AreaID", "AreaName", 1);
          
        }
        private void BindCityType()
        { //Ankita - 18/may/2016- (For Optimization)
            //string strQ = "select * from MastCityType order by Name";
            string strQ = "select ID,Name from MastCityType order by Name";
            fillDDLDirect(ddlcityType, strQ, "ID", "Name", 1);
        }
        private void BindCityConveyanceType()
        {//Ankita - 18/may/2016- (For Optimization)
            //string strQ = "select * from MastCityConveyanceType order by Name";
            string strQ = "select ID,Name from MastCityConveyanceType order by Name";
            fillDDLDirect(ddlcityconveyancetype, strQ, "ID", "Name", 1);
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
        private void fillRepeter()
        {
            //Query Optimized - Abhishek - 28-05-2016
            //string str = @"select ma.*,ma1.DisplayName as DisplayName1,case ma.Active when 1 then 'Yes' else 'No' end as Active1,ma1.AreaName as Parent,mct.Name as CityTypeName,mcct.Name as CityConveyance from MastArea ma inner join MastArea ma1 on ma.UnderId=ma1.AreaID left join MastCityType mct on ma.CityType=mct.Id left join MastCityConveyanceType mcct on ma.CityConveyanceType=mcct.Id where ma.AreaType='CITY'";
            string str = @"select ma.AreaId,ma.AreaName,ma.STDCode,ma.SyncId,ma1.DisplayName as DisplayName1,case ma.Active when 1 then 'Yes' else 'No' end as Active1,ma1.AreaName as Parent,mct.Name as CityTypeName,mcct.Name as CityConveyance from MastArea ma inner join MastArea ma1 on ma.UnderId=ma1.AreaID left join MastCityType mct on ma.CityType=mct.Id left join MastCityConveyanceType mcct on ma.CityConveyanceType=mcct.Id where ma.AreaType='CITY'";
            DataTable Locdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = Locdt;
            rpt.DataBind();

            Locdt.Dispose();
        }
        private void FillLocControls(int AreaId)
         {
            try
            {
                string MatGrpQry = @"select * from MastArea where AreaId=" + AreaId;
                DataTable LocValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, MatGrpQry);
                if (LocValueDt.Rows.Count > 0)
                {
                    Location.Value = LocValueDt.Rows[0]["AreaName"].ToString();
                    SyncId.Value = LocValueDt.Rows[0]["SyncId"].ToString();
                    StdCode.Value = LocValueDt.Rows[0]["StdCode"].ToString();
                    //BindParent(Convert.ToInt32(LocValueDt.Rows[0]["UnderId"]));
                    HiddeDisCitynUnderID.Value = LocValueDt.Rows[0]["UnderId"].ToString();
                    ddlParentLoc.SelectedValue = LocValueDt.Rows[0]["UnderId"].ToString();
                    BindCityType();
                    BindCityConveyanceType();
                    if (!string.IsNullOrEmpty(LocValueDt.Rows[0]["CityConveyanceType"].ToString()))
                    ddlcityconveyancetype.SelectedValue = LocValueDt.Rows[0]["CityConveyanceType"].ToString();
                    if (!string.IsNullOrEmpty(LocValueDt.Rows[0]["CityType"].ToString()))
                    ddlcityType.SelectedValue = LocValueDt.Rows[0]["CityType"].ToString();
                    if (Convert.ToBoolean(LocValueDt.Rows[0]["Active"]) == true)
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

                LocValueDt.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }
        private void InsertLoc()
        {
            string active = "0", DistrictID = Request.Form[HiddeDisCitynUnderID.UniqueID];

            if (chkIsActive.Checked)
                active = "1";

           // int retval = LB.InsertLocation(Convert.ToInt32(ddlParentLoc.SelectedValue), Location.Value, SyncId.Value, active, "", "CITY", Convert.ToInt32(ddlcityType.SelectedValue), Convert.ToInt32(ddlcityconveyancetype.SelectedValue), StdCode.Value, "");

            int retval = LB.InsertLocation(Convert.ToInt32(DistrictID), Location.Value, SyncId.Value, active, "", "CITY", Convert.ToInt32(ddlcityType.SelectedValue), Convert.ToInt32(ddlcityconveyancetype.SelectedValue), StdCode.Value, "",0,0,0);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate City Exists');", true);
                Location.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastArea set SyncId='" + retval + "' where areaid=" + retval + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                Location.Focus();
            }
        }
        private void UpdateLoc()
        {
            string active = "0", DistrictID = Request.Form[HiddeDisCitynUnderID.UniqueID];
            if (chkIsActive.Checked)
                active = "1";

          //  int retval = LB.UpdateLocation(Convert.ToInt32(ViewState["AreaId"]), Convert.ToInt32(ddlParentLoc.SelectedValue), Location.Value, SyncId.Value, active, "", "CITY", Convert.ToInt32(ddlcityType.SelectedValue), Convert.ToInt32(ddlcityconveyancetype.SelectedValue), StdCode.Value, "");

            int retval = LB.UpdateLocation(Convert.ToInt32(ViewState["AreaId"]), Convert.ToInt32(DistrictID), Location.Value, SyncId.Value, active, "", "CITY", Convert.ToInt32(ddlcityType.SelectedValue), Convert.ToInt32(ddlcityconveyancetype.SelectedValue), StdCode.Value, "",0,0,0);

            if (retval == -1)
            {                
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate City Exists');", true);
                Location.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (active == "0")
                {
                    string query = "insert into DeleteLog (EntityName,EntityKey,DeletedDate) values('MastCity','" + ViewState["AreaId"] + "',GETDATE())";
                    DbConnectionDAL.ExecuteQuery(query);
                }
                if (SyncId.Value == "")
                {
                    string syncid = "update MastArea set SyncId='" + retval + "' where areaid=" + Convert.ToInt32(ViewState["AreaId"]) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                Location.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        private void ClearControls()
        {
            Location.Value = "";
            SyncId.Value = "";
            ddlcityType.SelectedIndex = 0;
            StdCode.Value = "";
            ddlcityconveyancetype.SelectedIndex = 0;
            chkIsActive.Checked = true;
            HiddeDisCitynUnderID.Value = string.Empty;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
           // BindParent(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = LB.delete(Convert.ToString(ViewState["AreaId"]));
                if (retdel == 1)
                {
                    string query = "insert into DeleteLog (EntityName,EntityKey,DeletedDate) values('MastCity','" + ViewState["AreaId"] + "',GETDATE())";
                    DbConnectionDAL.ExecuteQuery(query);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Location.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                     ClearControls();
                     Location.Focus();
                }
             
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/City.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateLoc();
                }
                else
                {
                    InsertLoc();
                }
             
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");

        }

        private void filldt()
        {
            string str = "", straddqry = ""; StringBuilder sb = new StringBuilder(); string col = "";
            if (_exportp == true)
            {
                str = @"select ROW_NUMBER() over (order by ma.AreaId desc ) as [Sr. No],ma.AreaName,ma1.DisplayName as Parent,ma.STDCode,ma.SyncId,mct.Name as CityTypeName,mcct.Name as CityConveyance,case ma.Active when 1 then 'Yes' else 'No' end as Active from MastArea ma inner join MastArea ma1 on ma.UnderId=ma1.AreaID left join MastCityType mct on ma.CityType=mct.Id left join MastCityConveyanceType mcct on ma.CityConveyanceType=mcct.Id where ma.AreaType='CITY'";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                foreach (DataColumn dc in dt.Columns)
                {
                    col += dc.ColumnName + ",";
                }

                sb.AppendLine(col.Trim(','));
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                      string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sb.AppendLine(string.Join(",", fields));
                }

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=CityMaster.csv");
                Response.Write(sb.ToString());

                sb.Clear();
                dt.Dispose();

                Response.End();
            }
            else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You do not have permission to Export');", true);


        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            filldt();
        }
    }
}