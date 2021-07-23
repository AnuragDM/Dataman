using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data;
using System.Globalization;
using System.IO;
using DAL;
using System.Configuration;
using System.Data.SqlClient;
namespace FFMS
{
    public partial class UploadLeadInq : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
        }

        private DataTable GetDataTableFromCsv(string path)
        {
            DataTable dt = new DataTable();
            string CSVFilePathName = path;
            string[] Lines = File.ReadAllLines(CSVFilePathName);
            string[] Fields;
            Fields = (Lines[0].TrimEnd(',')).Split(new char[] { ',' });
            int Cols = Fields.GetLength(0);

            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
                dt.Columns.Add(Fields[i].Trim(), typeof(string));
            dt.AcceptChanges();
            if (GetValidCols(dt))
            {
                DataRow Row;
                for (int i = 2; i < Lines.GetLength(0); i++)
                {
                    Fields = Lines[i].Split(new char[] { ',' });
                    Row = dt.NewRow();
                    for (int f = 0; f < Cols; f++)
                        Row[f] = Fields[f].Trim();
                    dt.Rows.Add(Row);
                }
                dt.AcceptChanges();
            }
            else
            {
                lblcols.Visible = true;

                // ShowAlert("Incorrect Column Names.Please Check Uploaded File.");

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Incorrect Column Names.Please Check Uploaded File.');", true);
            }
            return dt;

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Data data = new Data();
            DataTable dt = new DataTable();
            if (Fupd.HasFile)
            {
                lblcols.InnerText = "";

                try
                {
                    String ext = System.IO.Path.GetExtension(Fupd.FileName);
                    if (!(ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx" || ext.ToLower() == ".csv"))
                    {
                        //ShowAlert("Please Upload Only .xls File.");

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Upload Only .xls File.');", true);
                        return;
                    }
                    string filename = Path.GetFileName(Fupd.FileName);
                    data.UploadFile(Fupd, filename, "~/FileUploads/");
                    if (ext.ToLower() == ".csv")
                    {
                        String csv_file_path = Path.Combine(Server.MapPath("~/FileUploads"), filename);
                        dt = GetDataTableFromCsv(csv_file_path);
                    }
                    else
                    {
                        dt = data.ReadXLSWithDataTypeCols("~/FileUploads/", filename, "LeadInq$", true);
                    }
                    DataTable dtcols = new DataTable();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtcols.Columns.Add(dt.Columns[i].ToString(), typeof(string));
                    }
                    dtcols.AcceptChanges();
                    if (GetValidCols(dtcols))
                    {
                        lblcols.Visible = false;
                        dt.Columns.Add("LeadInqId");
                        dt.Columns["LeadInqId"].SetOrdinal(0);
                        dt.Columns.Add("IsImport");
                        dt.Columns.Add("ImportBy");
                        dt.Columns.Add("Status");
                        dt.Columns.Add("IsDeleted");
                        InsertItems(dt);
                    }
                    else { lblcols.Visible = true; }
                }
                catch (Exception ex)
                {
                    string errormsg = "ERROR: " + ex.Message.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + errormsg + "');", true);
                    //ShowAlert("ERROR: " + ex.Message.ToString());
                }
            }
            else
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You have not specified a file.');", true);
                //ShowAlert("You have not specified a file.");
            }
        }
        private void InsertItems(DataTable dtrows)
        {
            lblcols.InnerText = ""; string errorMsg = ""; int index = 2; string salesPersonId = "", addError = "";
            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dtrows.Rows[i]["ContactPerson*"].ToString()))
                {
                    addError = " - Contact Person:" + dtrows.Rows[i]["ContactPerson*"].ToString();
                }
                else
                {
                    addError = "";
                }
                index++;
                if (string.IsNullOrEmpty(dtrows.Rows[i]["LeadInqDate*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["ContactPerson*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Country*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["State*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["City*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Address*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Mobile*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["LeadInqType*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["CallerType*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["LeadInqDesc*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Nature*"].ToString()))
                {
                    string error = "At Data Row No -" + index.ToString() + " Please supply values for mandatory fields - LeadInqDate*, ContactPerson*, Country*, State*, City*, Address*, Mobile*, LeadInqType*, CallerType*, LeadInqDesc*, Nature*";
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["ContactPerson*"].ToString()))
                    {
                        error = error + " - Contact Person:" + dtrows.Rows[i]["ContactPerson*"].ToString();
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage3", "errormessage3('" + error + "');", true);
                    return;
                }

                try
                {
                    bool isValid = false;
                    if (!((dtrows.Rows[i]["Country*"] == "" && dtrows.Rows[i]["State*"] != "") || (dtrows.Rows[i]["Country*"] == "" && dtrows.Rows[i]["City*"] != "") || (dtrows.Rows[i]["State*"] == "" && dtrows.Rows[i]["City*"] != "")))
                    {
                        isValid = true;
                        if (dtrows.Rows[i]["Country*"] != "")
                        {
                            int cId = DbConnectionDAL.GetIntScalarVal("select AreaID from mastarea where AreaType='Country' and Active='1' and AreaName='" + dtrows.Rows[i]["Country*"] + "'");
                            if (cId > 0)
                            {
                                if (dtrows.Rows[i]["State*"] != "")
                                {
                                    int sId = DbConnectionDAL.GetIntScalarVal("select AreaID from mastarea where UnderId IN (select AreaID from mastarea where UnderId=" + cId + " and AreaType='REGION') AND  AreaType='STATE' and AreaName='" + dtrows.Rows[i]["State*"] + "'");
                                    if (sId > 0)
                                    {
                                        if (dtrows.Rows[i]["City*"] != "")
                                        {
                                            int cityid = DbConnectionDAL.GetIntScalarVal("select AreaID,AreaName from mastarea where UnderId IN (select AreaId from mastarea where UnderId = " + sId + " and AreaType='DISTRICT') and AreaType='CITY' and AreaName='" + dtrows.Rows[i]["City*"] + "'");
                                            if (cityid > 0)
                                            {
                                            }
                                            else
                                            {
                                                isValid = false;
                                                errorMsg += "Invalid City at Data Row No " + index.ToString() + addError + ".<br>";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        isValid = false;
                                        errorMsg += "Invalid State at Data Row No " + index.ToString() + addError + ".<br>";
                                    }
                                }
                            }
                            else
                            {
                                isValid = false;
                                errorMsg += "Invalid City at Data Row No " + index.ToString() + addError + ".<br>";
                            }
                        }
                    }

                    if (Convert.ToString(dtrows.Rows[i]["LeadInqType*"]) != "")
                    {
                        int LeadInqType = DbConnectionDAL.GetIntScalarVal("select LeadInqTypeId from Lead_MastLeadInqType where LeadInqTypeName='" + dtrows.Rows[i]["LeadInqType*"] + "'");
                        if (LeadInqType <= 0)
                        {
                            isValid = false;
                            errorMsg += "Invalid LeadInqType* at Data Row No " + index.ToString() + addError + ".<br>";
                        }
                    }
                    if (Convert.ToString(dtrows.Rows[i]["CallerType*"]) != "")
                    {
                        int CallerType = DbConnectionDAL.GetIntScalarVal("select CallerId from Lead_MastLeadInqCaller where CallerName='" + dtrows.Rows[i]["CallerType*"] + "'");
                        if (CallerType <= 0)
                        {
                            isValid = false;
                            errorMsg += "Invalid CallerType* at Data Row No " + index.ToString() + addError + ".<br>";
                        }
                    }
                    if (Convert.ToString(dtrows.Rows[i]["Nature*"]) != "")
                    {
                        int Nature = DbConnectionDAL.GetIntScalarVal("select NatureId from Lead_MastLeadInqNature where NatureName='" + dtrows.Rows[i]["Nature*"] + "'");
                        if (Nature <= 0)
                        {
                            isValid = false;
                            errorMsg += "Invalid Nature* at Data Row No " + index.ToString() + addError + ".<br>";
                        }
                    }
                    if (Convert.ToString(dtrows.Rows[i]["LeadInqSource"]) != "")
                    {
                        int SourceName = DbConnectionDAL.GetIntScalarVal("select SourceId from Lead_MastLeadInqSource where SourceName='" + dtrows.Rows[i]["LeadInqSource"] + "'");
                        if (SourceName <= 0)
                        {
                            isValid = false;
                            errorMsg += "Invalid LeadInqSource at Data Row No " + index.ToString() + addError + ".<br>";
                        }
                    }
                    if (Convert.ToString(dtrows.Rows[i]["LeadInqSourceinfo"]) != "")
                    {
                        int LeadInqSourceinfo = DbConnectionDAL.GetIntScalarVal("select SInfoId from Lead_MastLeadInqSourceInfo where SInfoname='" + dtrows.Rows[i]["LeadInqSourceinfo"] + "'");
                        if (LeadInqSourceinfo <= 0)
                        {
                            isValid = false;
                            errorMsg += "Invalid LeadInqSourceinfo at Data Row No " + index.ToString() + addError + ".<br>";
                        }
                    }
                    if (Convert.ToString(dtrows.Rows[i]["SalesPersonId"]) != "")
                    {
                        int SalesPersonId = DbConnectionDAL.GetIntScalarVal("select SMId from MastSalesRep where SyncId='" + dtrows.Rows[i]["SalesPersonId"] + "'");
                        if (SalesPersonId <= 0)
                        {
                            isValid = false;
                            errorMsg += "Invalid SalesPersonId at Data Row No " + index.ToString() + addError + ".<br>";
                        }
                    }
                    if (isValid == true)
                    {
                        // DateTime f = Convert.ToDateTime(dtrows.Rows[i]["LeadInqDate*"]);
                        try
                        {
                            dtrows.Rows[i]["LeadInqDate*"] = Convert.ToString(dtrows.Rows[i]["LeadInqDate*"]) != "" ? Convert.ToDateTime(dtrows.Rows[i]["LeadInqDate*"]).ToString("dd/MMM/yyyy") : dtrows.Rows[i]["LeadInqDate*"];
                        }
                        catch (Exception ex)
                        {
                            errorMsg += "Invalid LeadInqDate* at Row No " + index.ToString() + addError + ".<br>";
                        }
                        //try
                        //{
                        //    dtrows.Rows[i]["ApprxOrderVal"] = Convert.ToString(dtrows.Rows[i]["ApprxOrderVal"]) != "" ? Convert.ToDouble(dtrows.Rows[i]["ApprxOrderVal"]) : 0;
                        //}
                        //catch (Exception ex)
                        //{
                        //    errorMsg += "Invalid ApprxOrderVal at Row No " + index.ToString() + addError + ".<br>";
                        //}
                        try
                        {
                            dtrows.Rows[i]["AvgOrderVal"] = Convert.ToString(dtrows.Rows[i]["AvgOrderVal"]) != "" ? Convert.ToDouble(dtrows.Rows[i]["AvgOrderVal"]) : 0;
                        }
                        catch (Exception ex)
                        {
                            errorMsg += "Invalid AvgOrderVal at Row No " + index.ToString() + addError + ".<br>";
                        }
                        try
                        {
                            salesPersonId = DbConnectionDAL.GetStringScalarVal("select SMId from MastSalesRep with (nolock) where SyncId='" + dtrows.Rows[i]["SalesPersonId"] + "'");
                            dtrows.Rows[i]["SalesPersonId"] = salesPersonId != "" ? Convert.ToInt32(salesPersonId) : 0;
                        }
                        catch (Exception ex)
                        {
                            errorMsg += "Invalid SalesPersonId at Row No " + index.ToString() + addError + ".<br>";
                        }
                        dtrows.Rows[i]["IsImport"] = true;
                        dtrows.Rows[i]["ImportBy"] = Settings.Instance.SMID;
                        dtrows.Rows[i]["Status"] = "Open";
                        dtrows.Rows[i]["IsDeleted"] = "False";
                    }
                    else
                    {
                        //dtrows.Rows.RemoveAt(i);
                        //i = i - 1;
                    }
                }

                catch (Exception ex)
                {
                    errorMsg += "Invalid Data at Row No " + index.ToString() + addError + ".<br>";
                    // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + errorMsg + "');", true); 
                    // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Incorrect Data at Row : " + index.ToString() + "');", true);
                }

            }
            if (!String.IsNullOrEmpty(errorMsg))
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + errorMsg + "');", true);
            else
            {
                if (dtrows.Rows.Count > 0)
                {
                    string sqlConnectionstring = DbConnectionDAL.sqlConnectionstring;
                    SqlConnection cn = new SqlConnection(sqlConnectionstring);
                    cn.Open();
                    //creating object of SqlBulkCopy  
                    SqlBulkCopy objbulk = new SqlBulkCopy(cn);
                    //assigning Destination table name  
                    objbulk.DestinationTableName = "Lead_MastLeadInq";
                    //Mapping Table column  
                    for (int i = 0; i < dtrows.Columns.Count; i++)
                    {
                        objbulk.ColumnMappings.Add(dtrows.Columns[i].ColumnName, dtrows.Columns[i].ColumnName.TrimEnd('*'));
                    }
                    //inserting bulk Records into DataBase   
                    string time = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss");
                    objbulk.WriteToServer(dtrows);

                    string id = UpdateDociId();
                    string[] ids = id.Split(',');
                    string url = "";
                    string pro_id = "LEADINQASSIGN";
                    string msg = "", salesPersonName = "", DocId = "";
                    int salesPerson = 0, SalesPersonId = 0;
                    foreach (var lId in ids)
                    {
                        salesPerson = DbConnectionDAL.GetIntScalarVal("select SalesPersonId from Lead_MastLeadInq where LeadInqId=" + lId);
                        if (salesPerson != 0)
                        {
                            SalesPersonId = DbConnectionDAL.GetIntScalarVal("select UserId from MastSalesRep where SMId=" + salesPerson);
                            salesPersonName = DbConnectionDAL.GetStringScalarVal("select EmpName from MastSalesRep where SMId=" + salesPerson);
                            DocId = DbConnectionDAL.GetStringScalarVal("select DocId from Lead_MastLeadInq where LeadInqId=" + lId);

                            url = "TransLeadInqList.aspx?val=" + lId;
                            msg = "Lead/Inq " + DocId + " Status-Open is assigned to -" + salesPersonName;

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
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Lead/Inq Imported Successfully');", true);
                    // ShowAlert("Lead/Inq Imported Successfully");
                }
            }
        }

        private static void NotifyUser(int ToSMId, int ToId, string url, string notifyTime, string pro_id, string msg, int FromSMId, int FromId)
        {
            string sql = @"INSERT into [TransNotification] ([pro_id], [userid], [VDate], [msgURL], [displayTitle], [Status], [FromUserId], [SMId], [ToSMId], [ToSMIdL1]) VALUES ('" + pro_id + "', " + ToId + ", '" + notifyTime + "', '" + url + "', '" + msg + "', 0, " + FromId + ", " + FromSMId + ", " + ToSMId + ", NULL)";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
        }
        public string UpdateDociId()
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "LDINQ");
            dbParam[1] = new DbParameter("@retval1", DbParameter.DbType.VarChar, -1, "LDINQ", ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_fillLeadInqDocId", dbParam);
            string ids = Convert.ToString(dbParam[1].Value);
            if (!String.IsNullOrEmpty(ids))
                ids = ids.TrimEnd(',');
            return ids;
        }
        public void connection()
        {
            //Stoting connection string   
            string sqlConnectionstring = DbConnectionDAL.sqlConnectionstring;
            SqlConnection cn = new SqlConnection(sqlConnectionstring);
            cn.Open();

        }
        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            if (dtcols.Columns[0].Caption.ToString().ToLower().Trim() != "LeadInqDate*".ToLower() || dtcols.Columns[1].ToString().ToLower().Trim() != "ContactPerson*".ToLower() || dtcols.Columns[2].ToString().ToLower().Trim() != "FirmName".ToLower() || dtcols.Columns[3].ToString().ToLower().Trim() != "Country*".ToLower() || dtcols.Columns[4].ToString().ToLower().Trim() != "State*".ToLower().Trim() || dtcols.Columns[5].ToString().ToLower().Trim() != "City*".ToLower() || dtcols.Columns[6].ToString().ToLower().Trim() != "Address*".ToLower() || dtcols.Columns[7].ToString().ToLower().Trim() != "Mobile*".ToLower() || dtcols.Columns[8].ToString().ToLower().Trim() != "PhoneNo".ToLower() || dtcols.Columns[9].ToString().ToLower().Trim() != "Email".ToLower() || dtcols.Columns[10].ToString().ToLower().Trim() != "Fax".ToLower() || dtcols.Columns[11].ToString().ToLower().Trim() != "LeadInqType*".ToLower() || dtcols.Columns[12].ToString().ToLower().Trim() != "CallerType*".ToLower() || dtcols.Columns[13].ToString().ToLower().Trim() != "LeadInqDesc*".ToLower() || dtcols.Columns[14].ToString().ToLower().Trim() != "Nature*".ToLower() || dtcols.Columns[15].ToString().ToLower().Trim() != "ProductType".ToLower() || dtcols.Columns[16].ToString().ToLower().Trim() != "ApprxOrderVal".ToLower() || dtcols.Columns[17].ToString().ToLower().Trim() != "AvgOrderVal".ToLower() || dtcols.Columns[18].ToString().ToLower().Trim() != "LeadInqSource".ToLower() || dtcols.Columns[19].ToString().ToLower().Trim() != "LeadInqSourceinfo".ToLower() || dtcols.Columns[20].ToString().ToLower().Trim() != "SalesPersonId".ToLower())
                Valid = false;
            lblcols.InnerText += "LeadInqDate*,ContactPerson*,FirmName,Country*,State*,City*,Address*,Mobile*,PhoneNo,Email,Fax,LeadInqType*,CallerType*,LeadInqDesc*,Nature*,ProductType,ApprxOrderVal,AvgOrderVal,LeadInqSource,LeadInqSourceinfo,SalesPersonId" + spaces;


            return Valid;
        }
        public void ShowAlert(string Message)
        {

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(" + Message + ");", true);
            //string script = "window.alert(\"" + Message.Normalize() + "\");";
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
    }
}