using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL;
using BusinessLayer;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Mime;

namespace AstralFFMS
{
    public partial class TransLeadInq : System.Web.UI.Page
    {
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];

            txtLeadInqDate.Attributes.Add("readonly", "readonly");
            txtFindLeadInqDate.Attributes.Add("readonly", "readonly");
            txtFindLeadInqToDate.Attributes.Add("readonly", "readonly");
            if (parameter != "")
            {
                ViewState["LeadInqId"] = parameter;
                FillComplControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (!IsPostBack)
            {            
                BindLeadInqTypeDDl();
                BindCallerTypeDDl();
                BindNatureDDl();
                BindSourceDDl();
                BindSourceInfoDDl();
                BindCountryDDl();
                BindSalesPersonDDl();

                BindCallerTypeFilter();
                BindContactPersonFilter();
                BindLeadInqFilter();
                BindProductTypeFilter();
                BindSalesPersonFilter();
                BindSorceInfoFilter();
                BindStatusFilter();
                BindNatureFilter();
                BindSourceFilter();

                btnDelete.Visible = false;
                rptmain.Style.Add("display", "block");
                mainDiv.Style.Add("display", "none");
                // mainDiv.Style.Add("display", "block");
            }
            string pageName = Path.GetFileName(Request.Path);          
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');           
            btnExport.CssClass = "btn btn-primary";
         //   btnExport.Visible = false;
          //  btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            btnExport.Enabled = Convert.ToBoolean(SplitPerm[4]);
            ContactPersonDiv.Visible = Convert.ToBoolean(SplitPerm[1]);
            CallertypeDiv.Visible = Convert.ToBoolean(SplitPerm[1]);
            LeadInqDescDiv.Visible = Convert.ToBoolean(SplitPerm[1]);
            NatureDiv.Visible = Convert.ToBoolean(SplitPerm[1]);
            ProductTypeDiv.Visible = Convert.ToBoolean(SplitPerm[1]);
            SourceInfoDiv.Visible = Convert.ToBoolean(SplitPerm[1]);
            SourceDiv.Visible = Convert.ToBoolean(SplitPerm[1]);
            SalesPersonDiv.Visible = Convert.ToBoolean(SplitPerm[1]);

            ddlFindContactPerson.Visible = Convert.ToBoolean(SplitPerm[1]);
            ddlFindCallerType.Visible = Convert.ToBoolean(SplitPerm[1]);
            txtFindLeadInqDesc.Visible = Convert.ToBoolean(SplitPerm[1]);

            ddlFindNature.Visible = Convert.ToBoolean(SplitPerm[1]);
            ddlFindProductType.Visible = Convert.ToBoolean(SplitPerm[1]);
            ddlFindSourceInfo.Visible = Convert.ToBoolean(SplitPerm[1]);
            ddlFindSource.Visible = Convert.ToBoolean(SplitPerm[1]);
            ddlFindSalesPerson.Visible = Convert.ToBoolean(SplitPerm[1]);


            btnBack.Visible = Convert.ToBoolean(SplitPerm[1]);
            btnBack.Enabled = Convert.ToBoolean(SplitPerm[1]);
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";

            }
            else
            {
                // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
        }
        private void FillComplControls(int LeadInqId)
        {
            try
            {
                string sql = @"select * from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    txtLeadInqDate.Text = dt.Rows[0]["LeadInqDate"] != "" ? Convert.ToDateTime(dt.Rows[0]["LeadInqDate"]).ToString("dd/MMM/yyyy") : "";
                    ddlLeadInqType.ClearSelection();
                    if (ddlLeadInqType.Items.FindByText(Convert.ToString(dt.Rows[0]["LeadInqType"])) != null)
                        ddlLeadInqType.Items.FindByText(Convert.ToString(dt.Rows[0]["LeadInqType"])).Selected = true;
                    ddlCallerType.ClearSelection();
                    if (ddlCallerType.Items.FindByText(Convert.ToString(dt.Rows[0]["CallerType"])) != null)
                        ddlCallerType.Items.FindByText(Convert.ToString(dt.Rows[0]["CallerType"])).Selected = true;
                    ddlNature.ClearSelection();
                    if (ddlNature.Items.FindByText(Convert.ToString(dt.Rows[0]["Nature"])) != null)
                        ddlNature.Items.FindByText(Convert.ToString(dt.Rows[0]["Nature"])).Selected = true;
                    ddlSource.ClearSelection();
                    if (ddlSource.Items.FindByText(Convert.ToString(dt.Rows[0]["LeadInqSource"])) != null)
                        ddlSource.Items.FindByText(Convert.ToString(dt.Rows[0]["LeadInqSource"])).Selected = true;
                    ddlSourceInfo.ClearSelection();
                    if (ddlSourceInfo.Items.FindByText(Convert.ToString(dt.Rows[0]["LeadInqSourceinfo"])) != null)
                        ddlSourceInfo.Items.FindByText(Convert.ToString(dt.Rows[0]["LeadInqSourceinfo"])).Selected = true;
                    ddlSalesPerson.ClearSelection();
                    string SalesPersonSyncId = DbConnectionDAL.GetStringScalarVal("select SyncId from MastSalesRep with (nolock) where SMId=" + dt.Rows[0]["SalesPersonId"]);
                    if (ddlSalesPerson.Items.FindByValue(SalesPersonSyncId) != null)
                        ddlSalesPerson.Items.FindByValue(SalesPersonSyncId).Selected = true;
                    ddlCountry.ClearSelection();
                    if (ddlCountry.Items.FindByText(Convert.ToString(dt.Rows[0]["Country"])) != null)
                        ddlCountry.Items.FindByText(Convert.ToString(dt.Rows[0]["Country"])).Selected = true;
                    txtProductType.Value = dt.Rows[0]["ProductType"] != "" ? Convert.ToString(dt.Rows[0]["ProductType"]) : "";
                    txtApprxOrderVal.Value = dt.Rows[0]["ApprxOrderVal"] != "" ? Convert.ToString(dt.Rows[0]["ApprxOrderVal"]) : "";
                    txtAvgOrderVal.Value = dt.Rows[0]["AvgOrderVal"] != "" ? Convert.ToString(dt.Rows[0]["AvgOrderVal"]) : "";
                    txtLeadInqDesc.Text = dt.Rows[0]["LeadInqDesc"] != "" ? Convert.ToString(dt.Rows[0]["LeadInqDesc"]) : "";
                    txtContactPerson.Value = dt.Rows[0]["ContactPerson"] != "" ? Convert.ToString(dt.Rows[0]["ContactPerson"]) : "";
                    txtFirmName.Value = dt.Rows[0]["FirmName"] != "" ? Convert.ToString(dt.Rows[0]["FirmName"]) : "";
                    txtAddress.Value = dt.Rows[0]["Address"] != "" ? Convert.ToString(dt.Rows[0]["Address"]) : "";
                    txtMobile.Value = dt.Rows[0]["Mobile"] != "" ? Convert.ToString(dt.Rows[0]["Mobile"]) : "";
                    txtPhone.Value = dt.Rows[0]["PhoneNo"] != "" ? Convert.ToString(dt.Rows[0]["PhoneNo"]) : "";
                    txtEmail.Value = dt.Rows[0]["Email"] != "" ? Convert.ToString(dt.Rows[0]["Email"]) : "";
                    txtFax.Value = dt.Rows[0]["Fax"] != "" ? Convert.ToString(dt.Rows[0]["Fax"]) : "";
                    if (dt.Rows[0]["Country"] != null)
                    {
                        int cId = DbConnectionDAL.GetIntScalarVal("select AreaID from mastarea where AreaType='Country' and Active='1' and AreaName='" + dt.Rows[0]["Country"] + "'");
                        FillStateByCountry(cId);
                        ddlState.ClearSelection();
                        if (ddlState.Items.FindByText(Convert.ToString(dt.Rows[0]["State"])) != null)
                            ddlState.Items.FindByText(Convert.ToString(dt.Rows[0]["State"])).Selected = true;
                    }
                    if (dt.Rows[0]["State"] != null)
                    {
                        FillCitybyState(Convert.ToInt32(ddlState.SelectedValue));
                        ddlCity.ClearSelection();
                        if (ddlCity.Items.FindByText(Convert.ToString(dt.Rows[0]["City"])) != null)
                            ddlCity.Items.FindByText(Convert.ToString(dt.Rows[0]["City"])).Selected = true;
                    }

                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while populating records');", true);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            rpt.DataSource = null;
            rpt.DataBind();
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
        private void BindSalesPersonDDl()
        {
            bool isAdmin = false; string sql = "";
            string isAdminstr = DbConnectionDAL.GetStringScalarVal("select b.IsAdmin from MastSalesRep a Left Join MastRole b on b.RoleId=a.RoleId where SMId=" + Settings.Instance.SMID);
            if (!String.IsNullOrEmpty(isAdminstr))
            {
                isAdmin = Convert.ToBoolean(isAdminstr);
            }
            if (isAdmin == false)
            {
                sql = String.Format("select SyncId,SMName+IsNull('('+SyncId+')','')[SMName] from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1 and smid<> {0} order by smname", Settings.Instance.SMID);
            }
            else
            {
                sql = String.Format("select SyncId,SMName+IsNull('('+SyncId+')','')[SMName] from mastsalesrep a Left Join MastRole b on b.RoleId=a.RoleId where a.active=1 and a.smid<> {0} and b.IsAdmin<>1 order by smname", Settings.Instance.SMID);
            }
            fillDDLDirect(ddlSalesPerson, sql, "SyncId", "SMName");
        }
        private void BindCountryDDl()
        {
            string strC = "select AreaID,AreaName from mastarea where AreaType='Country' and Active='1' order by AreaName";
            fillDDLDirect(ddlCountry, strC, "AreaID", "AreaName");
        }
        private void BindLeadInqTypeDDl()
        {
            string strC = "select * from Lead_MastLeadInqType with (nolock) order by LeadInqTypeName";
            fillDDLDirect(ddlLeadInqType, strC, "LeadInqTypeId", "LeadInqTypeName");
        }
        private void BindCallerTypeDDl()
        {
            string strC = "select * from Lead_MastLeadInqCaller with (nolock) order by CallerName";
            fillDDLDirect(ddlCallerType, strC, "CallerId", "CallerName");
        }
        private void BindNatureDDl()
        {
            string strC = "select * from Lead_MastLeadInqNature with (nolock) order by NatureName";
            fillDDLDirect(ddlNature, strC, "NatureId", "NatureName");
        }
        private void BindSourceDDl()
        {
            string strC = "select * from Lead_MastLeadInqSource with (nolock) order by SourceName";
            fillDDLDirect(ddlSource, strC, "SourceId", "SourceName");
        }
        private void BindSourceInfoDDl()
        {
            string strC = "select * from Lead_MastLeadInqSourceInfo with (nolock) order by SInfoname";
            fillDDLDirect(ddlSourceInfo, strC, "SInfoId", "SInfoname");
        }
        private void BindContactPersonFilter()
        {
           // string strC = "select distinct ContactPerson from Lead_MastLeadInq with (nolock) where (IsDeleted<>'True' or IsDeleted is null)  and ContactPerson!='' and ImportBy=" + Settings.Instance.SMID + "  order by ContactPerson";
            string strC = "select distinct ContactPerson from Lead_MastLeadInq with (nolock) where (IsDeleted<>'True' or IsDeleted is null)  and ContactPerson!='' order by ContactPerson";
            fillDDLDirect(ddlFindContactPerson, strC, "ContactPerson", "ContactPerson");
        }
        private void BindLeadInqFilter()
        {
            string strC = "select * from Lead_MastLeadInqType with (nolock) order by LeadInqTypeName";
            fillDDLDirect(ddlFindLeadInqType, strC, "LeadInqTypeId", "LeadInqTypeName");
        }
        private void BindCallerTypeFilter()
        {
            string strC = "select * from Lead_MastLeadInqCaller with (nolock) order by CallerName";
            fillDDLDirect(ddlFindCallerType, strC, "CallerId", "CallerName");
        }
        private void BindStatusFilter()
        {
            string strC = "select * from Lead_MastLeadInqStatus with (nolock)  order by StatusName";
            fillDDLDirect(ddlFindStatus, strC, "StatusId", "StatusName");
        }
        private void BindNatureFilter()
        {
            string strC = "select * from Lead_MastLeadInqNature with (nolock) order by NatureName";
            fillDDLDirect(ddlFindNature, strC, "NatureId", "NatureName");
        }
        private void BindProductTypeFilter()
        {
            //string strC = "select distinct ProductType from Lead_MastLeadInq with (nolock) where (IsDeleted<>'True' or IsDeleted is null) and ProductType!='' and ImportBy=" + Settings.Instance.SMID + " order by ProductType";
            string strC = "select distinct ProductType from Lead_MastLeadInq with (nolock) where (IsDeleted<>'True' or IsDeleted is null) and ProductType!='' order by ProductType";
            fillDDLDirect(ddlFindProductType, strC, "ProductType", "ProductType");
        }
        private void BindSourceFilter()
        {
            string strC = "select * from Lead_MastLeadInqSource with (nolock) order by SourceName";
            fillDDLDirect(ddlFindSource, strC, "SourceId", "SourceName");
        }
        private void BindSorceInfoFilter()
        {
            string strC = "select * from Lead_MastLeadInqSourceInfo with (nolock) order by SInfoname";
            fillDDLDirect(ddlFindSourceInfo, strC, "SInfoId", "SInfoname");
        }
        private void BindSalesPersonFilter()
        {
            bool isAdmin = false; string sql = "";
            string isAdminstr = DbConnectionDAL.GetStringScalarVal("select b.IsAdmin from MastSalesRep a Left Join MastRole b on b.RoleId=a.RoleId where SMId=" + Settings.Instance.SMID);
            if (!String.IsNullOrEmpty(isAdminstr))
            {
                isAdmin = Convert.ToBoolean(isAdminstr);
            }
            if (isAdmin == false)
            {
                sql = String.Format("select SyncId,SMName+IsNull('('+SyncId+')','')[SMName] from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1 and smid<> {0} order by smname", Settings.Instance.SMID);
            }
            else
            {
                sql = String.Format("select SyncId,SMName+IsNull('('+SyncId+')','')[SMName] from mastsalesrep a Left Join MastRole b on b.RoleId=a.RoleId where a.active=1 and a.smid<> {0} and b.IsAdmin<>1 order by smname", Settings.Instance.SMID);
            }
            fillDDLDirect(ddlFindSalesPerson, sql, "SyncId", "SMName");
        }
        private void FillStateByCountry(int i)
        {
            string sql = "select AreaID,AreaName from mastarea where UnderId IN (select AreaID from mastarea where UnderId=" + i + " and AreaType='REGION') AND  AreaType='STATE' order by AreaName";
            fillDDLDirect(ddlState, sql, "AreaID", "AreaName");
        }
        private void FillCitybyState(int i)
        {
            string sql = "select AreaID,AreaName from mastarea where UnderId IN (select AreaId from mastarea where UnderId = " + i + " and AreaType='DISTRICT') and AreaType='CITY' order by AreaName";
            fillDDLDirect(ddlCity, sql, "AreaID", "AreaName");
        }
        public static void fillDDLDirect(ListBox xddl, string xmQry, string xvalue, string xtext)
        {
            xddl.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string LeadInqDate = txtLeadInqDate.Text;
                string LeadInqType = ddlLeadInqType.SelectedItem != null ? ddlLeadInqType.SelectedItem.Text : "";
                string CallerType = ddlCallerType.SelectedItem != null ? ddlCallerType.SelectedItem.Text : "";
                string nature = ddlNature.SelectedItem != null ? ddlNature.SelectedItem.Text : "";
                string productType = txtProductType.Value;
               // double apprxOrderValue = txtApprxOrderVal.Value != "" ? Convert.ToDouble(txtApprxOrderVal.Value) : 0;
                string apprxOrderValue = txtApprxOrderVal.Value;
                double avgOrderValue = txtAvgOrderVal.Value != "" ? Convert.ToDouble(txtAvgOrderVal.Value) : 0;
                string source = ddlSource.SelectedItem != null ? ddlSource.SelectedItem.Text : "";
                string sourceInfo = ddlSourceInfo.SelectedItem != null ? ddlSourceInfo.SelectedItem.Text : "";
                string salesPersonSyncId = ddlSalesPerson.SelectedValue;
                string salesPerson = DbConnectionDAL.GetStringScalarVal("Select SMId from MastSalesRep with (nolock) where SyncId='" + salesPersonSyncId + "'");
                string LeadInqDesc = txtLeadInqDesc.Text;
                string ContactPerson = txtContactPerson.Value;
                string FirmName = txtFirmName.Value;
                string Country = ddlCountry.SelectedItem != null ? ddlCountry.SelectedItem.Text : "";
                string State = ddlState.SelectedItem != null ? ddlState.SelectedItem.Text : "";
                string City = ddlCity.SelectedItem != null ? ddlCity.SelectedItem.Text : "";
                string Address = txtAddress.Value;
                string Mobile = txtMobile.Value;
                string Phone = txtPhone.Value;
                string Email = txtEmail.Value;
                string Fax = txtFax.Value;
                if (btnSave.Text == "Update")
                {

                    string sql = "update Lead_MastLeadInq set LeadInqDate='" + LeadInqDate + "', ContactPerson='" + ContactPerson + "' ,FirmName='" + FirmName + "' ,Country='" + Country + "' ,State='" + State + "' ,City='" + City + "' ,Address='" + Address + "' ,Mobile='" + Mobile + "' ,PhoneNo='" + Phone + "' ,Email='" + Email + "' ,Fax='" + Fax + "' ,LeadInqType='" + LeadInqType + "' ,CallerType='" + CallerType + "' ,LeadInqDesc='" + LeadInqDesc + "' ,Nature='" + nature + "' ,ProductType='" + productType + "' ,ApprxOrderVal='" + apprxOrderValue + "' ,AvgOrderVal='" + avgOrderValue + "' ,LeadInqSource='" + source + "' ,LeadInqSourceinfo='" + sourceInfo + "' ,SalesPersonId='" + salesPerson + "' where LeadInqId=" + Convert.ToInt64(ViewState["LeadInqId"]);
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);

                    if (ddlSalesPerson.SelectedIndex > -1)
                    {
                        DateTime newDate = GetUTCTime();
                        string DocId = DbConnectionDAL.GetStringScalarVal("Select DocId from Lead_MastLeadInq with (nolock) where LeadInqId=" + ViewState["LeadInqId"]);
                        string status = DbConnectionDAL.GetStringScalarVal("Select Status from Lead_MastLeadInq with (nolock) where LeadInqId=" + ViewState["LeadInqId"]);
                        string time = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                        string pro_id = "LEADINQRESPOND";
                        string msg = "Lead/Inq " + DocId + " Status-" + status + " is assigned to -" + ddlSalesPerson.SelectedItem.Text;
                        int assignedUser = DbConnectionDAL.GetIntScalarVal("Select SalesPersonId from Lead_MastLeadInq with (nolock) where LeadInqId=" + ViewState["LeadInqId"]);
                        if (assignedUser == Convert.ToInt32(salesPerson))
                        {
                            pro_id = "LEADINQUPDATE";
                            msg = "Lead/Inq " + DocId + " Status-" + status + " is updated which is assigned to -" + ddlSalesPerson.SelectedItem.Text;
                        }

                        int SalesPersonId = DbConnectionDAL.GetIntScalarVal("Select UserId from MastSalesRep with (nolock) where SMId=" + salesPerson);
                        string url = "TransLeadInqList.aspx?val=" + ViewState["LeadInqId"];
                        NotifyUser(Convert.ToInt32(salesPerson), SalesPersonId, url, time, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));

                        int reportingperson = DbConnectionDAL.GetIntScalarVal("select UnderId from MastSalesRep with (nolock) where SMId =" + salesPerson);
                        int reportingpersonId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + reportingperson);
                        if (reportingperson != 0)
                        {

                            NotifyUser(reportingperson, reportingpersonId, url, time, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));
                        }

                        DataTable dt = new DataTable();
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT ms.SMId FROM MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId WHERE mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead') AND SMId IN (SELECT Maingrp FROM MastSalesRepGrp WHERE SMId=" + reportingperson + ") and ms.SMId<>" + reportingperson + " ORDER BY ms.SMName");
                        int recpId = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            recpId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + dt.Rows[i][0]);
                            NotifyUser(Convert.ToInt32(dt.Rows[i][0]), recpId, url, time, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));
                        }

                    }

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Update Successfully!!');", true);
                }
                else
                {
                    DateTime newDate = GetUTCTime();
                    string DocId = GetDociId(newDate);
                    string time = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    string pro_id = "LEADINQRESPOND";
                    string sql = "insert into Lead_MastLeadInq  (DocId,LeadInqDate,ContactPerson,FirmName,Country,State,City,Address,Mobile,PhoneNo,Email,Fax,LeadInqType,CallerType,LeadInqDesc,Nature,ProductType,ApprxOrderVal,AvgOrderVal,LeadInqSource,LeadInqSourceinfo,SalesPersonId,IsImport,ImportBy,Status,IsDeleted) values ('" + DocId + "','" + LeadInqDate + "','" + ContactPerson + "','" + FirmName + "','" + Country + "','" + State + "','" + City + "','" + Address + "','" + Mobile + "','" + Phone + "','" + Email + "','" + Fax + "','" + LeadInqType + "','" + CallerType + "','" + LeadInqDesc + "','" + nature + "','" + productType + "','" + apprxOrderValue + "'," + avgOrderValue + ",'" + source + "','" + sourceInfo + "','" + salesPerson + "','" + false + "'," + Settings.Instance.SMID + ",'Open','False')";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                    if (ddlSalesPerson.SelectedIndex > -1)
                    {
                        int SalesPersonId = DbConnectionDAL.GetIntScalarVal("Select UserId from MastSalesRep with (nolock) where SMId=" + salesPerson);
                        string LeadInqId = DbConnectionDAL.GetStringScalarVal("Select LeadInqId from Lead_MastLeadInq with (nolock) where DocId='" + DocId + "'");
                        string Status = DbConnectionDAL.GetStringScalarVal("Select Status from Lead_MastLeadInq with (nolock) where DocId='" + DocId + "'");
                        string url = "TransLeadInqList.aspx?val=" + LeadInqId;
                        string msg = "Lead/Inq " + DocId + " Status-" + Status + " is assigned to -" + ddlSalesPerson.SelectedItem.Text;
                        NotifyUser(Convert.ToInt32(salesPerson), SalesPersonId, url, time, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));

                        int reportingperson = DbConnectionDAL.GetIntScalarVal("select UnderId from MastSalesRep with (nolock) where SMId =" + salesPerson);
                        int reportingpersonId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + reportingperson);
                        if (reportingperson != 0)
                        {

                            NotifyUser(reportingperson, reportingpersonId, url, time, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));
                        }

                        DataTable dt = new DataTable();
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT ms.SMId FROM MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId WHERE mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead') AND SMId IN (SELECT Maingrp FROM MastSalesRepGrp WHERE SMId=" + reportingperson + ") and ms.SMId<>" + reportingperson + " ORDER BY ms.SMName");
                        int recpId = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            recpId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + dt.Rows[i][0]);
                            NotifyUser(Convert.ToInt32(dt.Rows[i][0]), recpId, url, time, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));
                        }

                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully <br/> DocNo-" + DocId + "');", true);
                }
                ClearControls();
            }
            catch (Exception ex)
            {
                string errormsg = "ERROR: " + ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + errormsg + "');", true);
            }
        }
        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
        public string GetDociId(DateTime newDate)
        {
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "LDINQ");
            dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime, 8, newDate);
            dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
            return Convert.ToString(dbParam[2].Value);
        }
        private void ClearControls()
        {
            txtLeadInqDate.Text = "";
            ddlLeadInqType.SelectedIndex = -1;
            ddlCallerType.SelectedIndex = -1;
            ddlNature.SelectedIndex = -1;
            txtProductType.Value = "";
            txtApprxOrderVal.Value = "";
            txtAvgOrderVal.Value = "";
            ddlSource.SelectedIndex = -1;
            ddlSourceInfo.SelectedIndex = -1;
            ddlSalesPerson.SelectedIndex = -1;
            txtLeadInqDesc.Text = "";
            txtContactPerson.Value = "";
            ddlCountry.SelectedIndex = -1;
            ddlState.SelectedIndex = -1;
            ddlCity.SelectedIndex = -1;
            txtAddress.Value = "";
            txtMobile.Value = "";
            txtPhone.Value = "";
            txtEmail.Value = "";
            txtFax.Value = "";
            btnDelete.Visible = false;
            btnSave.Text = "Save";
        }
        private void ClearFilterControls()
        {
            txtFindLeadInqDate.Text = "";
            txtFindLeadInqToDate.Text = "";
            ddlFindContactPerson.SelectedIndex = -1;
            ddlFindLeadInqType.SelectedIndex = -1;
            ddlFindCallerType.SelectedIndex = -1;
            ddlFindStatus.SelectedIndex = -1;
            ddlFindNature.SelectedIndex = -1;
            ddlFindProductType.SelectedIndex = -1;
            ddlFindSource.SelectedIndex = -1;
            ddlFindSourceInfo.SelectedIndex = -1;
            ddlFindSalesPerson.SelectedIndex = -1;
            txtFindLeadInqDesc.Value = "";
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateByCountry(Convert.ToInt32(ddlCountry.SelectedValue));
        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCitybyState(Convert.ToInt32(ddlState.SelectedValue));
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {           
            if (txtFindLeadInqToDate.Text != "" && txtFindLeadInqDate.Text != "")
            {
                if (!(Convert.ToDateTime(txtFindLeadInqToDate.Text) >= Convert.ToDateTime(txtFindLeadInqDate.Text)))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                    return;
                }
            }
            string filter = "", tempStr = "";
            tempStr = string.Join(",", ddlFindContactPerson.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Value + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "ContactPerson in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindLeadInqType.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Text + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "LeadInqType in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindCallerType.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Text + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "CallerType in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindStatus.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Text + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "Status in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindNature.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Text + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "Nature in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindProductType.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Value + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "ProductType in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindSource.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Text + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "LeadInqSource in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindSourceInfo.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Text + "'"));
            filter += !String.IsNullOrEmpty(tempStr) ? "LeadInqSourceinfo in (" + tempStr + ") and " : "";
            tempStr = string.Join(",", ddlFindSalesPerson.Items.Cast<ListItem>().Where(item => item.Selected).Select(it => "'" + it.Value + "'"));
            if (!String.IsNullOrEmpty(tempStr))
            {
                string strsql ="SELECT distinct STUFF((SELECT distinct ',' +  cast(p1.SMID as varchar) FROM MastSalesRep p1 WHERE p1.SyncId in ("+tempStr+") FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,'') SMId FROM MastSalesRep p";
                string strVal = DbConnectionDAL.GetStringScalarVal(strsql);
                filter += !String.IsNullOrEmpty(strVal) ? "SalesPersonId in (" + strVal + ") and " : "";
            }
           // filter += !String.IsNullOrEmpty(tempStr) ? "SalesPersonId in (" + tempStr + ") and " : "";

            filter += txtFindLeadInqDate.Text != "" ? "Cast(LeadInqDate as date) >='" + Convert.ToDateTime(txtFindLeadInqDate.Text).ToString("dd/MMM/yyyy") + "' and " : "";
            filter += txtFindLeadInqToDate.Text != "" ? "Cast(LeadInqDate as date) <='" + Convert.ToDateTime(txtFindLeadInqToDate.Text).ToString("dd/MMM/yyyy") + "' and " : "";
            filter += txtFindLeadInqDesc.Value != "" ? "LeadInqDesc like '%" + txtFindLeadInqDesc.Value + "%' and " : "";
            bool isAdmin = false;
            string isAdminstr = DbConnectionDAL.GetStringScalarVal("select b.IsAdmin from MastSalesRep a Left Join MastRole b on b.RoleId=a.RoleId where SMId=" + Settings.Instance.SMID);
            if (!String.IsNullOrEmpty(isAdminstr))
            {
                isAdmin = Convert.ToBoolean(isAdminstr);
            }
            if (isAdmin == false)
            {
                filter += "(IsDeleted<>'True' or IsDeleted is null) and (ImportBy in(" + String.Format("select SMId from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1", Settings.Instance.SMID) + ") or SalesPersonId in (" + String.Format("select SMId from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1", Settings.Instance.SMID) + "))";
            }
            else
            {
                filter += "(IsDeleted<>'True' or IsDeleted is null)";
            }

            //filter = !String.IsNullOrEmpty(filter) ?filter.Remove(filter.LastIndexOf(" and "), 5):"";
            //string sql = "select LeadInqId,Cast(LeadInqDate as date) as Date, DocId as [Lead/Inq Id], Status, ContactPerson as [Contact Person], FirmName as [Firm Name] , Address, City, State, Country, a.Mobile, PhoneNo, a.Email, Fax,LeadInqType as [Lead/Inq Type],CallerType as [Caller Type],LeadInqDesc as [Description], Nature, Producttype as [Product Type] ,ApprxOrderVal as [Apprx Order Value],AvgOrderVal as [Avg Order Value],LeadInqSource as Source, LeadInqSourceinfo as [Source Info], b.SMName as [Sales Person], case IsImport when 1 then 'Yes' else 'No' End as [By Import] from [Lead_MastLeadInq] a left join MastSalesRep b on b.SMId=a.SalesPersonId";
            string sql = "select LeadInqId,Cast(LeadInqDate as date) as Date, DocId as [Lead/Inq Id], Status,(SELECT TOP 1 Remarks FROM Lead_TransMastLeadInq WHERE LeadInqId=a.LeadInqId ORDER BY id desc) AS remarks,(SELECT TOP 1 RespondDate FROM Lead_TransMastLeadInq WHERE LeadInqId=a.LeadInqId ORDER BY id desc) AS RespondDate,case Status when 'Resolved' then (SELECT TOP 1 Remarks FROM Lead_TransMastLeadInq WHERE LeadInqId=a.LeadInqId ORDER BY id desc) else '' End as Resolvedremarks,case Status when 'Resolved' then (SELECT TOP 1 RespondDate FROM Lead_TransMastLeadInq WHERE LeadInqId=a.LeadInqId ORDER BY id desc) else null End as ResolvedRespondDate,case Status when 'Closed' then (SELECT TOP 1 Remarks FROM Lead_TransMastLeadInq WHERE LeadInqId=a.LeadInqId ORDER BY id desc) else '' End as Closedremarks,case Status when 'Closed' then (SELECT TOP 1 RespondDate FROM Lead_TransMastLeadInq WHERE LeadInqId=a.LeadInqId ORDER BY id desc) else null End as ClosedRespondDate,(SELECT TOP 1 orderValue FROM Lead_TransMastLeadInq WHERE LeadInqId=a.LeadInqId ORDER BY id desc) AS orderValue,ContactPerson as [Contact Person], FirmName as [Firm Name] , Address, City, State, Country, a.Mobile, PhoneNo, a.Email, Fax,LeadInqType as [Lead/Inq Type],CallerType as [Caller Type],LeadInqDesc as [Description], Nature, Producttype as [Product Type] ,ApprxOrderVal as [Apprx Order Value],AvgOrderVal as [Avg Order Value],LeadInqSource as Source, LeadInqSourceinfo as [Source Info], b.SMName as [Sales Person], case IsImport when 1 then 'Yes' else 'No' End as [By Import] from [Lead_MastLeadInq] a left join MastSalesRep b on b.SMId=a.SalesPersonId";
            sql = !String.IsNullOrEmpty(filter) ? @"" + sql + " where " + filter + " order by Date desc" : @"" + sql + " order by Date desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
              //  btnExport.Visible = true;
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TransLeadInq.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                if (Convert.ToInt32(ViewState["LeadInqId"]) != 0)
                {
                    try
                    {
                        string sql = "update Lead_MastLeadInq set IsDeleted= 'true' where LeadInqId=" + Convert.ToInt32(ViewState["LeadInqId"]);
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                        ClearControls();
                    }
                    catch (Exception ex)
                    {
                        string errormsg = "ERROR: " + ex.ToString();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + errormsg + "');", true);
                    }
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

        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            ClearFilterControls();
            rpt.DataSource = null;
            rpt.DataBind();
        }


        private static void NotifyUser(int ToSMId, int ToId, string url, string notifyTime, string pro_id, string msg, int FromSMId, int FromId)
        {
            string sql = @"INSERT into [TransNotification] ([pro_id], [userid], [VDate], [msgURL], [displayTitle], [Status], [FromUserId], [SMId], [ToSMId], [ToSMIdL1]) VALUES ('" + pro_id + "', " + ToId + ", '" + notifyTime + "', '" + url + "', '" + msg + "', 0, " + FromId + ", " + FromSMId + ", " + ToSMId + ", NULL)";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
          
            string headertxt = "Date".TrimStart('"').TrimEnd('"') + "," + "Lead/Inq Id".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"') + "," + "Contact Person".TrimStart('"').TrimEnd('"') + "," + "Firm Name".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "State".TrimStart('"').TrimEnd('"') + "," + "Country".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "PhoneNo".TrimStart('"').TrimEnd('"') + "," + "Email".TrimStart('"').TrimEnd('"') + "," + "Fax".TrimStart('"').TrimEnd('"') + "," + "Lead/Inq Type".TrimStart('"').TrimEnd('"') + "," + "Caller Type".TrimStart('"').TrimEnd('"') + "," + "Description".TrimStart('"').TrimEnd('"') + "," + "Nature".TrimStart('"').TrimEnd('"') + "," + "Product Type".TrimStart('"').TrimEnd('"') + "," + "Apprx Order Value".TrimStart('"').TrimEnd('"') + "," + "Avg Order Value".TrimStart('"').TrimEnd('"') + "," + "Source.".TrimStart('"').TrimEnd('"') + "," + "Source Info.".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Status Remarks".TrimStart('"').TrimEnd('"') + "," + "Status Date".TrimStart('"').TrimEnd('"') + "," + "Resolved Remarks".TrimStart('"').TrimEnd('"') + "," + "Resolved Date".TrimStart('"').TrimEnd('"') + "," + "Closed Remarks".TrimStart('"').TrimEnd('"') + "," + "Closed Date".TrimStart('"').TrimEnd('"') + "," + "Order Value".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertxt);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dtParams.Columns.Add(new DataColumn("Lead/Inq Id", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Status", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Contact Person", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Firm Name", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Address", typeof(String)));

            dtParams.Columns.Add(new DataColumn("City", typeof(String)));
            dtParams.Columns.Add(new DataColumn("State", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Country", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PhoneNo", typeof(String)));

            dtParams.Columns.Add(new DataColumn("Email", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Fax", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Lead/Inq Type", typeof(String)));

            dtParams.Columns.Add(new DataColumn("Caller Type", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Description", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Nature", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Product Type", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Apprx Order Value", typeof(String)));

            dtParams.Columns.Add(new DataColumn("Avg Order Value", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Source", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Source Info", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Sales Person", typeof(String)));
            dtParams.Columns.Add(new DataColumn("remarks", typeof(String)));
            dtParams.Columns.Add(new DataColumn("RespondDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Resolvedremarks", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ResolvedRespondDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Closedremarks", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ClosedRespondDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("orderValue", typeof(String)));

            foreach (RepeaterItem item in rpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label dateLabel = item.FindControl("dateLabel") as Label;
                dr["Date"] = dateLabel.Text;
                Label lblLeadId = item.FindControl("lblLeadId") as Label;
                dr["Lead/Inq Id"] = lblLeadId.Text.ToString();
                Label lblStatus = item.FindControl("lblStatus") as Label;
                dr["Status"] = lblStatus.Text.ToString();
                Label lblCp = item.FindControl("lblCp") as Label;
                dr["Contact Person"] = lblCp.Text.ToString();
                Label lblFNm = item.FindControl("lblFNm") as Label;
                dr["Firm Name"] = lblFNm.Text.ToString();
                Label lblAddress = item.FindControl("lblAddress") as Label;
                dr["Address"] = lblAddress.Text.ToString();
                Label lblCity = item.FindControl("lblCity") as Label;
                dr["City"] = lblCity.Text.ToString();
                Label lblState = item.FindControl("lblState") as Label;
                dr["State"] = lblState.Text.ToString();
                Label lblCountry = item.FindControl("lblCountry") as Label;
                dr["Country"] = lblCountry.Text.ToString();
                Label lblMobile = item.FindControl("lblMobile") as Label;
                dr["Mobile"] = lblMobile.Text.ToString();
                Label lblPhone = item.FindControl("lblPhone") as Label;
                dr["PhoneNo"] = lblPhone.Text.ToString();
                Label lblEmail = item.FindControl("lblEmail") as Label;
                dr["Email"] = lblEmail.Text.ToString();
                Label lblFax = item.FindControl("lblFax") as Label;
                dr["Fax"] = lblFax.Text.ToString();
                Label lblInqType = item.FindControl("lblInqType") as Label;
                dr["Lead/Inq Type"] = lblInqType.Text.ToString();
                Label lblCallerType = item.FindControl("lblCallerType") as Label;
                dr["Caller Type"] = lblCallerType.Text.ToString();
                Label lblDescription = item.FindControl("lblDescription") as Label;
                dr["Description"] = lblDescription.Text.ToString();
                Label lblnature = item.FindControl("lblnature") as Label;
                dr["Nature"] = lblnature.Text.ToString();
                Label lblproductType = item.FindControl("lblproductType") as Label;
                dr["Product Type"] = lblproductType.Text.ToString();
                Label lblAproxOValue = item.FindControl("lblAproxOValue") as Label;
                dr["Apprx Order Value"] = lblAproxOValue.Text.ToString();
                Label lblAvgOValue = item.FindControl("lblAvgOValue") as Label;
                dr["Avg Order Value"] = lblAvgOValue.Text.ToString();
                Label lblSource = item.FindControl("lblSource") as Label;
                dr["Source"] = lblSource.Text.ToString();
                Label lblSourceInfo = item.FindControl("lblSourceInfo") as Label;
                dr["Source Info"] = lblSourceInfo.Text.ToString();
                Label lblSalesPerson = item.FindControl("lblSalesPerson") as Label;
                dr["Sales Person"] = lblSalesPerson.Text.ToString();
                Label lblRemarks = item.FindControl("lblRemarks") as Label;
                dr["remarks"] = lblRemarks.Text.ToString();
                Label lblrespondDate = item.FindControl("lblrespondDate") as Label;
                dr["RespondDate"] = lblrespondDate.Text.ToString();
                Label lblRRemarks = item.FindControl("lblRRemarks") as Label;
                dr["Resolvedremarks"] = lblRRemarks.Text.ToString();
                Label lblRRespondDate = item.FindControl("lblRRespondDate") as Label;
                dr["ResolvedRespondDate"] = lblRRespondDate.Text.ToString();
                Label lblClosedRemarks = item.FindControl("lblClosedRemarks") as Label;
                dr["Closedremarks"] = lblClosedRemarks.Text.ToString();
                Label lblClosedResponddate = item.FindControl("lblClosedResponddate") as Label;
                dr["ClosedRespondDate"] = lblClosedResponddate.Text.ToString();
                Label lblorderValue = item.FindControl("lblorderValue") as Label;
                dr["orderValue"] = lblorderValue.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {

                        if (k == 0)
                        {
                             sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');                           
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }

                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {

                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');                           
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }

                    }
                    else
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');                           
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=Lead/Inq.List.csv");
            Response.Write(sb.ToString());
            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

    }
}