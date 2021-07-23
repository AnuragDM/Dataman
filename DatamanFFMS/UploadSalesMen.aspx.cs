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
using BusinessLayer;

namespace FFMS
{
    public partial class UploadSalesMen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            txtfmDate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            }
        }
        //private void GetDataTableFromCsv(string path)
        //{
        //    DataTable dt = new DataTable();
        //    string CSVFilePathName = path;
        //    string[] Lines = File.ReadAllLines(CSVFilePathName);
        //    string[] Fields;
        //    Fields = Lines[0].Split(new char[] { ',' });
        //    int Cols = Fields.GetLength(0);

        //    //1st row must be column names; force lower case to ensure matching later on.
        //    for (int i = 0; i < Cols; i++)
        //        dt.Columns.Add(Fields[i].ToLower().Trim(), typeof(string));
        //    dt.AcceptChanges();
        //    if (GetValidCols(dt))
        //    {
        //        DataRow Row;
        //        for (int i = 1; i < Lines.GetLength(0); i++)
        //        {
        //            Fields = Lines[i].Split(new char[] { ',' });
        //            Row = dt.NewRow();
        //            for (int f = 0; f < Cols; f++)
        //                Row[f] = Fields[f].Trim();
        //            dt.Rows.Add(Row);
        //        }
        //        dt.AcceptChanges();
        //        InsertSalePersons(dt);
        //    }
        //    else
        //    {
        //        lblcols.Visible = true;
        //        ShowAlert("Incorrect Column Names.Please Check Uploaded File.");
        //    }

        //}
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Fupd.HasFile)
            {
                lblcols.InnerText = "";

                try
                {
                    Data data = new Data();
                    DataTable dt = new DataTable();
                    String ext = System.IO.Path.GetExtension(Fupd.FileName);
                    if (ext.ToLower() != ".xls")
                    {
                        ShowAlert("Please Upload Only .xls File.");
                        return;
                    }
                    string filename = Path.GetFileName(Fupd.FileName);
                    data.UploadFile(Fupd, filename, "~/FileUploads/");
                    dt = data.ReadXLSWithDataTypeCols("~/FileUploads/", filename, "Salesmen$", true);
                    DataTable dtcols = new DataTable();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtcols.Columns.Add(dt.Columns[i].ToString().ToLower(), typeof(string));
                    }
                    dtcols.AcceptChanges();
                    if (GetValidCols(dtcols))
                    {
                        lblcols.Visible = false;
                        InsertSalePersons(dt);
                    }
                    else { lblcols.Visible = true; }

                    dtcols.Dispose();
                    dt.Dispose();
                }
                catch (Exception ex)
                {
                    ShowAlert("ERROR: " + ex.Message.ToString());
                }
            }
            else
            {
                ShowAlert("You have not specified a file.");
            }
        }
        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ReportingPerson have No value";
            }
            if (c == 1)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Name have No value";
            }

            if (c == 2)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:DeviceNo have No value";
            }
            if (c == 4)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
            }
            if (c == 3)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:DSRAllowDays have No value";
                else if (!string.IsNullOrEmpty(value))
                {
                    int value1;
                    if (!int.TryParse(value.ToString(), out value1))
                    {
                        _errorMessage = " Row: " + r + " And Column:DSRAllowDays Should be an Integer value";
                    }
                }
            }
            if (c == 7)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:City have No value";
            }
            if (c == 8)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Email have No value";
            }
            if (c == 9)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Mobile have No value";
            }
            if (c == 11)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:EmpSyncId have No value";
            }
            if (c == 12)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
            }
            if (c == 13)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SalesPersonType have No value";
            }

            if (c == 14)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ResponsibilityCentre have No value";
            }
            if (c == 18)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:EmployeeName have No value";
            }
            return _errorMessage;
        }
        private void InsertSalePersons(DataTable dtrows)
        {

            ImportSalesPersons SP = new ImportSalesPersons();
            lblcols.InnerText = "";
            string _fMessage = "";
            for (int i = 0; i < dtrows.Rows.Count; i++)
            {

                try
                {
                    //if (string.IsNullOrEmpty(dtrows.Rows[i]["ReportingPerson*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Name*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["DeviceNo*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["DSRAllowDays*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["City*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Email*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Mobile*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["EmpSyncId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["SyncId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["SalesPersonType*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["ResponsibilityCentre*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["EmployeeName*"].ToString()))
                    //{ ShowAlert("At SalesPerson -" + dtrows.Rows[i]["Name*"].ToString() + " Please supply values for mandatory fields - ReportingPerson*,Name*,DeviceNo*,DSRAllowDays*,City*,Email*,Mobile*,EmpSyncId*,SyncId*,SalesPersonType*,ResponsibilityCentre*,EmployeeName*"); return; }

                    //int value;
                    //if (!int.TryParse(dtrows.Rows[i]["DSRAllowDays*"].ToString(), out value)) { ShowAlert("At Name -" + dtrows.Rows[i]["Name*"].ToString() + " - DSRAllowDays Should be an Integer value"); return; }
                    for (int c = 0; c < dtrows.Columns.Count; c++)
                    {
                        _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                    }
                    if (!string.IsNullOrEmpty(_fMessage))
                    {
                        string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Salesperson','" + dtrows.Rows[i]["SyncId*"].ToString().Replace("'", string.Empty) + "','UploadSalesMen.aspx')";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                        _fMessage = string.Empty;
                    }
                    else
                    {
                        string Msg = SP.InsertSalePersons(dtrows.Rows[i]["ReportingPerson*"].ToString(), dtrows.Rows[i]["Name*"].ToString(), dtrows.Rows[i]["DeviceNo*"].ToString(), dtrows.Rows[i]["DSRAllowDays*"].ToString(), dtrows.Rows[i]["Active"].ToString(), dtrows.Rows[i]["Address1"].ToString(), dtrows.Rows[i]["Address2"].ToString(), dtrows.Rows[i]["City*"].ToString(), dtrows.Rows[i]["Email*"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), dtrows.Rows[i]["Remarks"].ToString(), dtrows.Rows[i]["EmpSyncId*"].ToString(), dtrows.Rows[i]["SyncId*"].ToString(), dtrows.Rows[i]["SalesPersonType*"].ToString(), dtrows.Rows[i]["ResponsibilityCentre*"].ToString(), dtrows.Rows[i]["DOB"].ToString(), dtrows.Rows[i]["DOA"].ToString(), dtrows.Rows[i]["Pincode"].ToString(), dtrows.Rows[i]["EmployeeName*"].ToString());
                        if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                    }
                }
                catch (Exception ex) { ShowAlert("Incorrect Data at SalesPerson : " + dtrows.Rows[i]["Name*"].ToString()); ex.ToString(); }

            }
            string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='Salesperson' Order by [Created_At] desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _sql1);
            if (dt.Rows.Count > 0)
            {
                rptDatabase.DataSource = dt;
                rptDatabase.DataBind();
                rptmain.Style.Add("display", "block");
            }
            else
            {
                rptDatabase.DataSource = null;
                rptDatabase.DataBind();
                rptmain.Style.Add("display", "none");
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Sale Persons Imported Successfully');", true);
            //  ShowAlert("Sale Persons Imported Successfully");
            dt.Dispose();
        }

        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            if (dtcols.Columns[0].ToString().Trim() != "ReportingPerson*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Name*".ToLower() || dtcols.Columns[2].ToString().Trim() != "DeviceNo*".ToLower() || dtcols.Columns[3].ToString().Trim() != "DSRAllowDays*".ToLower().Trim() || dtcols.Columns[4].ToString().Trim() != "Active".ToLower() || dtcols.Columns[5].ToString().Trim() != "Address1".ToLower() || dtcols.Columns[6].ToString().Trim() != "Address2".ToLower() || dtcols.Columns[7].ToString().Trim() != "City*".ToLower() || dtcols.Columns[8].ToString().Trim() != "Email*".ToLower() || dtcols.Columns[9].ToString().Trim() != "Mobile*".ToLower() || dtcols.Columns[10].ToString().Trim() != "Remarks".ToLower() || dtcols.Columns[11].ToString().Trim() != "EmpSyncId*".ToLower() || dtcols.Columns[12].ToString().Trim() != "SyncId*".ToLower() || dtcols.Columns[13].ToString().Trim() != "SalesPersonType*".ToLower() || dtcols.Columns[14].ToString().Trim() != "ResponsibilityCentre*".ToLower() || dtcols.Columns[15].ToString().Trim() != "DOB".ToLower() || dtcols.Columns[16].ToString().Trim() != "DOA".ToLower() || dtcols.Columns[17].ToString().Trim() != "Pincode".ToLower() || dtcols.Columns[18].ToString().Trim() != "EmployeeName*".ToLower())
                Valid = false;
            lblcols.InnerText += "ReportingPerson*,Name*,DeviceNo*,DSRAllowDays*,Active,Address1,Address2,City*,Email*,Mobile*,Remarks,EmpSyncId*,,SyncId*,SalesPersonType*,ResponsibilityCentre*,DOB,DOA,Pincode,EmployeeName*" + spaces;

            return Valid;
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string _sql = "";
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='Salesperson' Order by [Created_At] desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _sql);
            if (dt.Rows.Count > 0)
            {
                rptDatabase.DataSource = dt;
                rptDatabase.DataBind();
                rptmain.Style.Add("display", "block");
            }
            else
            {
                rptDatabase.DataSource = null;
                rptDatabase.DataBind();
                rptmain.Style.Add("display", "none");
            }
            dt.Dispose();
        }
    }
}