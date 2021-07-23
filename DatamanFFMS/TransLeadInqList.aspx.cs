using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL;
using BusinessLayer;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Net.Mime;

namespace AstralFFMS
{
    public partial class TransLeadInqList : System.Web.UI.Page
    {
        string LeadInqId = "";
        string LeadInqDocId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["val"]))
            {
                LeadInqId = Convert.ToString(Request.QueryString["val"]);
                string sql = "";
                bool isAdmin = false;
                string isAdminstr = DbConnectionDAL.GetStringScalarVal("select b.IsAdmin from MastSalesRep a Left Join MastRole b on b.RoleId=a.RoleId where SMId=" + Settings.Instance.SMID);
                if (!String.IsNullOrEmpty(isAdminstr))
                {
                    isAdmin = Convert.ToBoolean(isAdminstr);
                }
                if (isAdmin == false)
                {
                    sql = "select count(*) from Lead_MastLeadInq where (IsDeleted<>'True' or IsDeleted is null) and LeadInqId=" + LeadInqId + " and (ImportBy in(" + String.Format("select SMId from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1", Settings.Instance.SMID) + ") or SalesPersonId in (" + String.Format("select SMId from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1", Settings.Instance.SMID) + "))";
                }
                else
                {
                    sql = "select count(*) from Lead_MastLeadInq where (IsDeleted<>'True' or IsDeleted is null) and LeadInqId=" + LeadInqId;
                }
                
                int i = DbConnectionDAL.GetIntScalarVal(sql);
                if (i <= 0)
                {
                    Response.Redirect("/Home.aspx", true);
                }
                string filter = "(IsDeleted<>'True' or IsDeleted is null) and (ImportBy in(" + String.Format("select SMId from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1", Settings.Instance.SMID) + ") or SalesPersonId in (" + String.Format("select SMId from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1", Settings.Instance.SMID) + "))";


                LeadInqDocId = DbConnectionDAL.GetStringScalarVal("Select DocId from Lead_MastLeadInq with (nolock) where LeadInqId="+LeadInqId);
            }
            lblCSID.Text = LeadInqDocId;
            if (!IsPostBack)
            {
                BindData();
                FillDetails();
            }
        }
        private void FillDetails()
        {
            string sql = "select LeadInqId,Cast(LeadInqDate as date) as Date, DocId as [Lead/Inq Id], Status, ContactPerson as [Contact Person], FirmName as [Firm Name] , Address, City, State, Country, a.Mobile, PhoneNo, a.Email, Fax,LeadInqType as [Lead/Inq Type],CallerType as [Caller Type],LeadInqDesc as [Description], Nature, Producttype as [Product Type] ,ApprxOrderVal as [Apprx Order Value],AvgOrderVal as [Avg Order Value],LeadInqSource as Source, LeadInqSourceinfo as [Source Info], b.SMName as [Sales Person], case IsImport when 1 then 'Yes' else 'No' End as [By Import] from [Lead_MastLeadInq] a left join MastSalesRep b on b.SMId=a.SalesPersonId where (IsDeleted<>'True' or IsDeleted is null) and LeadInqId=" + LeadInqId; 

            DataTable dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                lbldate.InnerText = Convert.ToDateTime(dt.Rows[0]["Date"]).ToString("dd/MMM/yyyy");
                lblLeadInqType.InnerText = Convert.ToString(dt.Rows[0]["Lead/Inq Type"]);
                lblCallertype.InnerText = Convert.ToString(dt.Rows[0]["Caller Type"]);
                lblNature.InnerText = Convert.ToString(dt.Rows[0]["Nature"]);
                lblProductType.InnerText = Convert.ToString(dt.Rows[0]["Product Type"]);
                lblApprxOrderVal.InnerText = Convert.ToString(dt.Rows[0]["Apprx Order Value"]);
                lblAvgOrderInLac.InnerText = Convert.ToString(dt.Rows[0]["Avg Order Value"]);
                lblSource.InnerText = Convert.ToString(dt.Rows[0]["Source"]);
                lblSourceInfo.InnerText = Convert.ToString(dt.Rows[0]["Source Info"]);
                lblSalesPerson.InnerText = Convert.ToString(dt.Rows[0]["Sales Person"]);
                lblLeadInqDesc.InnerText = Convert.ToString(dt.Rows[0]["Description"]);
                lblContactPerson.InnerText = Convert.ToString(dt.Rows[0]["Contact Person"]);
                lblFirmName.InnerText = Convert.ToString(dt.Rows[0]["Firm Name"]);
                lblAddress.InnerText = Convert.ToString(dt.Rows[0]["Address"]);
                lblCity.InnerText = Convert.ToString(dt.Rows[0]["City"]);
                lblState.InnerText = Convert.ToString(dt.Rows[0]["State"]);
                lblCountry.InnerText = Convert.ToString(dt.Rows[0]["Country"]);
                lblMobile.InnerText = Convert.ToString(dt.Rows[0]["Mobile"]);
                lblPhoneNo.InnerText = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                lblEmail.InnerText = Convert.ToString(dt.Rows[0]["Email"]);
                lblFax.InnerText = Convert.ToString(dt.Rows[0]["Fax"]);

            }
        }
        private void BindData()
        {
            try
            {
                int importBy = DbConnectionDAL.GetIntScalarVal("select ImportBy from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId);
                int assignedTo = DbConnectionDAL.GetIntScalarVal("select SalesPersonId from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId);
                string status = DbConnectionDAL.GetStringScalarVal("select LOWER(status) from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId);
                if (Convert.ToInt32(Settings.Instance.SMID) != importBy)
                {
                    RdbStatus.Items.FindByValue("Closed").Enabled = false;
                    RdbStatus.Items.FindByValue("Closed").Attributes.Add("style", "display:none!important;");
                }
                if (!(Convert.ToInt32(Settings.Instance.SMID) == importBy || Convert.ToInt32(Settings.Instance.SMID) == assignedTo))
                {
                    RdbStatus.Enabled = false;
                    btnSave.Enabled = false;
                }
                if (Convert.ToInt32(Settings.Instance.SMID) != importBy && status=="closed")
                {
                    RdbStatus.Enabled = false;
                    btnSave.Enabled = false;
                }
                lblstatus.Text = DbConnectionDAL.GetStringScalarVal("select Status from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId);
                string sql = "select tcr.RespondDate,tcr.Remarks,b.Empname from Lead_TransMastLeadInq tcr inner join MastSalesRep Ml on tcr.respondedby=Ml.SMId inner join MastLogin b on b.Id = Ml.UserId where LeadInqId=" + LeadInqId+" order by tcr.id desc";

                DataTable dt = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    rpt.DataSource = dt;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            catch (Exception ex) { ex.ToString(); }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Remarks = txtremarks.Text.Trim(), status = ""; bool flag = true;
                string DocId = DbConnectionDAL.GetStringScalarVal("select DocId from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId);
                int ImportBy = DbConnectionDAL.GetIntScalarVal("select ImportBy from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId);
                int SalesPerson = DbConnectionDAL.GetIntScalarVal("select SalesPersonId from Lead_MastLeadInq with (nolock) where LeadInqId=" + LeadInqId);
                int reportingperson = 0, reportingpersonId = 0, SalesPersonId = 0, ImportUserId = 0;
                if (RdbStatus.SelectedValue != "")
                {
                    status = RdbStatus.SelectedValue;
                }
                decimal ordervalue = 0;
                if(txtOrderValue.Text !="")
                {
                    ordervalue = Convert.ToDecimal(txtOrderValue.Text);
                }

                string sql = "insert into Lead_TransMastLeadInq (LeadInqId, RespondedBy, RespondDate, Remarks,OrderValue) values (" + LeadInqId + "," + Settings.Instance.SMID + ", '" + DateTime.Now.ToString("dd/MMM/yyyy") + "', '" + Remarks + "'," + ordervalue + ")";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                if (status == "Closed")
                {
                    if (Convert.ToInt32(Settings.Instance.SMID) != ImportBy)
                    {
                        flag = false;
                    }

                }
                if (flag)
                {
                    sql = "update Lead_MastLeadInq set Status='" + status + "' where LeadInqId=" + LeadInqId;
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                    txtremarks.Text = "";
                    txtOrderValue.Text = "";
                }
                if (SalesPerson != 0)
                {
                    reportingperson = DbConnectionDAL.GetIntScalarVal("select UnderId from MastSalesRep with (nolock) where SMId =" + SalesPerson);
                    SalesPersonId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + SalesPerson);
                    reportingpersonId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + reportingperson);
                }
                string url = "TransLeadInqList.aspx?val=" + LeadInqId + "#example1_wrapper";
                string notifyTime = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                string pro_id = "LEADINQRESPOND";
                string msg = "Lead/Inq " + DocId + " - Status("+status+") is responded by -" + Settings.Instance.EmpName;
                if (Convert.ToInt32(Settings.Instance.SMID) == ImportBy)
                {
                    if (SalesPerson != 0 && SalesPersonId!=0)
                    {
                        NotifyUser(SalesPerson, SalesPersonId, url, notifyTime, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));

                        if (reportingperson != 0)
                        {
                            NotifyUser(reportingperson, reportingpersonId, url, notifyTime, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));

                            //sql = @"INSERT into [TransNotification] ([pro_id], [userid], [VDate], [msgURL], [displayTitle], [Status], [FromUserId], [SMId], [ToSMId], [ToSMIdL1]) VALUES ('" + pro_id + "', " + reportingpersonId + ", '" + notifyTime + "', '" + url + "', '" + msg + "', 0, " + Settings.Instance.UserID + ", " + Settings.Instance.SMID + ", " + reportingperson + ", NULL)";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                            DataTable dt = new DataTable();
                            dt = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT ms.SMId FROM MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId WHERE mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead') AND SMId IN (SELECT Maingrp FROM MastSalesRepGrp WHERE SMId=" + reportingperson + ") and ms.SMId<>" + reportingperson + " ORDER BY ms.SMName");
                            int recpId = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                recpId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + dt.Rows[i][0]);
                                NotifyUser(Convert.ToInt32(dt.Rows[i][0]), recpId, url, notifyTime, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));
                            }

                        }
                    }
                }
                if (Convert.ToInt32(Settings.Instance.SMID) == SalesPerson)
                {
                    ImportUserId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + ImportBy);
                    NotifyUser(ImportBy, ImportUserId, url, notifyTime, pro_id, msg, SalesPerson, SalesPersonId);

                    //sql = @"INSERT into [TransNotification] ([pro_id], [userid], [VDate], [msgURL], [displayTitle], [Status], [FromUserId], [SMId], [ToSMId], [ToSMIdL1]) VALUES ('" + pro_id + "', " + ImportUserId + ", '" + notifyTime + "', '" + url + "', '" + msg + "', 0, " + SalesPersonId + ", " + Settings.Instance.SMID + ", " + ImportBy + ", NULL)";
                    //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                    if (reportingperson != 0 && reportingpersonId!=0)
                    {
                        NotifyUser(reportingperson, reportingpersonId, url, notifyTime, pro_id, msg, SalesPerson, SalesPersonId);
                        DataTable dt = new DataTable();
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT ms.SMId FROM MastSalesRep ms LEFT JOIN mastrole mr ON ms.RoleId=mr.RoleId WHERE mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead') AND SMId IN (SELECT Maingrp FROM MastSalesRepGrp WHERE SMId=" + reportingperson + ") and ms.SMId<>" + reportingperson + " ORDER BY ms.SMName");
                        int recpId = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            recpId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep with (nolock) where SMId =" + dt.Rows[i][0]);
                            NotifyUser(Convert.ToInt32(dt.Rows[i][0]), recpId, url, notifyTime, pro_id, msg, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID));
                        }
                    }
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                BindData();
            }
            catch (Exception xe)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private static void NotifyUser(int ToSMId, int ToId, string url, string notifyTime, string pro_id, string msg, int FromSMId, int FromId)
        {
            string sql = @"INSERT into [TransNotification] ([pro_id], [userid], [VDate], [msgURL], [displayTitle], [Status], [FromUserId], [SMId], [ToSMId], [ToSMIdL1]) VALUES ('" + pro_id + "', " + ToId + ", '" + notifyTime + "', '" + url + "', '" + msg + "', 0, " + FromId + ", " + FromSMId + ", " + ToSMId + ", NULL)";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
        }


        public void SendEmail(string ComplaintDocID, string EmpName, string RespondedBy, string Remark)
        {
        }



    }
}