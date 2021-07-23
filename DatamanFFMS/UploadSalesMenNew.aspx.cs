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
using BAL;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace FFMS
{
    public partial class UploadSalesMenNew : System.Web.UI.Page
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
                ViewState["dtResult"] = new DataTable();
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
                    if (ext.ToLower() != ".xlsx")
                    {
                        ShowAlert("Please Upload Only .xlsx File.");
                        return;
                    }
                    string filename = Path.GetFileName(Fupd.FileName);
                    data.UploadFile(Fupd, filename, "~/FileUploads/");
                    dt = data.ReadXLSWithDataTypeCols("~/FileUploads/", filename, "SalesPersonImport$", true);
                    DataTable dtcols = new DataTable();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtcols.Columns.Add(dt.Columns[i].ToString().ToLower(), typeof(string));
                    }
                    dtcols.AcceptChanges();
                    
                    if (GetValidCols(dtcols))
                    {
                        lblcols.Visible = false;
                        DataTable dtresult = InsertSalePersons(dt);
                        if (dtresult.Rows.Count > 0)
                        {
                            ViewState["dtResult"] = dtresult;
                          
                            rptDatabase.DataSource = dtresult;
                            rptDatabase.DataBind();
                            rptmain.Style.Add("display", "block");
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('SalesPerson  Imported Successfully');", true);

                        }
                        else
                        {
                            rptDatabase.DataSource = null;
                            rptDatabase.DataBind();
                            rptmain.Style.Add("display", "none");
                            ViewState["dtResult"] = null;
                        }
                    }
                    else { lblcols.Visible = true; }
                }
               
                catch (Exception ex)
                {
                
                }
           
            }
            else
            {
                ShowAlert("You have not specified a file.");
            }
        }

        private void GenerateExcel(DataTable dtresult)
        {
            try
            {
                #region ExcelExportRetailerImportInfo
                //Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=SalesPersonImportDetailReport.csv");
                string headertext = "SNo,Result,ResultType,Remark";

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.Append(System.Environment.NewLine);

                for (int j = 0; j < dtresult.Rows.Count; j++)
                {
                    for (int k = 0; k < dtresult.Columns.Count; k++)
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", j + 1) + ',');
                            sb.Append(String.Format("\"{0}\"", dtresult.Rows[j][k].ToString().Replace(",", "-")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtresult.Rows[j][k].ToString().Replace(",", "-")) + ',');
                        }


                    }
                    sb.Append(Environment.NewLine);
                }

                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=SalesPersonImportDetailReport.csv");
                Response.Write(sb.ToString());
                Response.Flush();
                //  Response.Headers.Clear();
                Response.End();
                #endregion
            }
                catch(System.Threading.ThreadAbortException)
            {
                Page_Load(null, null);
            }
            catch (Exception)
            {
                
                throw;
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
        private DataTable InsertSalePersons(DataTable dtrows)
        {

            SalesPersonsBAL SP = new SalesPersonsBAL();
            BAL.UserMaster.userall usall = new BAL.UserMaster.userall();
            ImportData upd = new ImportData();

            lblcols.InnerText = "";

            string underid = "0", CityId = "0", RoleId = "0", deptId = "", desgId = "0", HqId = "0";
            int SMID = 0;
            bool Active = false;
            string str = "", FromTime = "10:00", ToTime = "19:00", RecordInterval = "900", UploadInterval = "300", Accuracy = "100", PushNoti = "1", BatteryRecord = "1";

            int val = 0, retsave = 0;

            DataTable dtuploadResult = new DataTable();

            dtuploadResult.Columns.Add("Result");
            dtuploadResult.Columns.Add("ResultType");
            dtuploadResult.Columns.Add("Remark");
            dtuploadResult.AcceptChanges();
            DataRow row;

            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                underid = "0"; CityId = "0"; RoleId = "0"; deptId = ""; desgId = "0";
                SMID = 0;
                Active = false;
                str = ""; FromTime = "10:00"; ToTime = "19:00"; RecordInterval = "900"; UploadInterval = "300"; Accuracy = "100"; PushNoti = "1"; BatteryRecord = "1";
                val = 0; retsave = 0;

                try
                {
                    #region Check All Fields

                    if (string.IsNullOrEmpty(dtrows.Rows[i]["Reporting Person*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Employee Name*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["User Name*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Role*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["SalesPersonType*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Department*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Designation*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["City*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Mobile*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Emalid*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["HeadQuarter*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["JoiningDate*"].ToString()))
                    {
                        row = dtuploadResult.NewRow();
                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Please supply values for mandatory fields - Reporting Person*,Employee Name*,User Name*,Role*,SalesPersonType*,Department*,Designation*,City*,Mobile*,Emalid*,HeadQuarter*,JoiningDate*";
                        if (dtrows.Rows[i]["DMT App"].ToString() == "1")
                        {
                            if (string.IsNullOrEmpty(dtrows.Rows[i]["From Time*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["To Time*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Record Interval (in seconds)*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Accuracy*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["PushNotification*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Battery Recording*"].ToString()))
                            {
                                row["Remark"] += ",From Time*,To Time*,Record Interval (in seconds)*,Upload Interval (in seconds)*,Accuracy*,PushNotification*,Battery Recording*";
                            }
                        }
                        if (dtrows.Rows[i]["Active"].ToString() == "0")
                        {
                            if (string.IsNullOrEmpty(dtrows.Rows[i]["BlockReason*"].ToString()))
                            {

                                row["Remark"] += ",BlockReason*";
                            }
                        }
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }
                    else
                    {

                        if (dtrows.Rows[i]["DMT App"].ToString() == "1" && dtrows.Rows[i]["Active"].ToString() == "0")
                        {
                            if (string.IsNullOrEmpty(dtrows.Rows[i]["From Time*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["To Time*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Record Interval (in seconds)*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Accuracy*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["PushNotification*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Battery Recording*"].ToString()))
                            {
                                row = dtuploadResult.NewRow();
                                row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Please supply values for mandatory fields - ";
                                row["Remark"] += "From Time*,To Time*,Record Interval (in seconds)*,Upload Interval (in seconds)*,Accuracy*,PushNotification*,Battery Recording*,";

                                if (dtrows.Rows[i]["Active"].ToString() == "0")
                                {
                                    if (string.IsNullOrEmpty(dtrows.Rows[i]["BlockReason*"].ToString()))
                                    {

                                        row["Remark"] += "BlockReason*";
                                    }
                                }


                                row["Result"] = "Error";
                                row["ResultType"] = "Error";
                                dtuploadResult.Rows.Add(row);
                                continue;
                            }
                        }
                        else if (dtrows.Rows[i]["DMT App"].ToString() == "1")
                        {
                            if (string.IsNullOrEmpty(dtrows.Rows[i]["From Time*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["To Time*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Record Interval (in seconds)*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Accuracy*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["PushNotification*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Battery Recording*"].ToString()))
                            {
                                row = dtuploadResult.NewRow();
                                row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Please supply values for mandatory fields - ";
                                row["Remark"] += "From Time*,To Time*,Record Interval (in seconds)*,Upload Interval (in seconds)*,Accuracy*,PushNotification*,Battery Recording*";
                                row["Result"] = "Error";
                                row["ResultType"] = "Error";
                                dtuploadResult.Rows.Add(row);
                                continue;
                            }
                        }
                        else if (dtrows.Rows[i]["Active"].ToString() == "0")
                        {

                            if (string.IsNullOrEmpty(dtrows.Rows[i]["BlockReason*"].ToString()))
                            {
                                row = dtuploadResult.NewRow();
                                row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Please supply values for mandatory fields - BlockReason* ";
                                row["Result"] = "Error";
                                row["ResultType"] = "Error";
                                dtuploadResult.Rows.Add(row);
                                continue;
                            }
                        }

                    }


                    if (dtrows.Rows[i]["Reporting Person*"].ToString().Length > 80 || dtrows.Rows[i]["Employee Name*"].ToString().Length > 80 || dtrows.Rows[i]["User Name*"].ToString().Length > 50 || dtrows.Rows[i]["Role*"].ToString().Length > 50 || dtrows.Rows[i]["SalesPersonType*"].ToString().Length > 30 || dtrows.Rows[i]["Department*"].ToString().Length > 50 || dtrows.Rows[i]["Designation*"].ToString().Length > 50 || dtrows.Rows[i]["City*"].ToString().Length > 50 || dtrows.Rows[i]["Mobile*"].ToString().Length != 10 || dtrows.Rows[i]["Emalid*"].ToString().Length > 50 || dtrows.Rows[i]["HeadQuarter*"].ToString().Length > 50)//
                    {

                        row = dtuploadResult.NewRow();
                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Please supply values for mandatory fields as per mentioned size- Reporting Person*,Employee Name*,User Name*,Role*,SalesPersonType*,Department*,Designation*,City*,Mobile*,Emalid*,HeadQuarter*";

                        if (dtrows.Rows[i]["DMT App"].ToString() == "1")
                        {

                            if (dtrows.Rows[i]["From Time*"].ToString().Length > 80 || dtrows.Rows[i]["To Time*"].ToString().Length > 80 || dtrows.Rows[i]["Record Interval (in seconds)*"].ToString().Length > 50 || dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString().Length > 50 || dtrows.Rows[i]["Accuracy*"].ToString().Length > 30 || dtrows.Rows[i]["PushNotification*"].ToString().Length > 50 || dtrows.Rows[i]["Battery Recording*"].ToString().Length > 50)
                            {
                                row["Remark"] += ",From Time*,To Time*,Record Interval (in seconds)*,Upload Interval (in seconds)*,Accuracy*,PushNotification*,Battery Recording*";
                            }
                        }

                        if (dtrows.Rows[i]["Active"].ToString() == "0")
                        {
                            if (dtrows.Rows[i]["BlockReason*"].ToString().Length > 90)
                            {
                                row["Remark"] += ",BlockReason*";
                            }
                        }
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }
                    else
                    {

                        if (dtrows.Rows[i]["DMT App"].ToString() == "1" && dtrows.Rows[i]["Active"].ToString() == "0")
                        {
                            if (dtrows.Rows[i]["From Time*"].ToString().Length > 80 || dtrows.Rows[i]["To Time*"].ToString().Length > 80 || dtrows.Rows[i]["Record Interval (in seconds)*"].ToString().Length > 50 || dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString().Length > 50 || dtrows.Rows[i]["Accuracy*"].ToString().Length > 30 || dtrows.Rows[i]["PushNotification*"].ToString().Length > 50 || dtrows.Rows[i]["Battery Recording*"].ToString().Length > 50)
                            {
                                row = dtuploadResult.NewRow();
                                row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Please supply values for mandatory fields as per mentioned size - ";
                                row["Remark"] += "From Time*,To Time*,Record Interval (in seconds)*,Upload Interval (in seconds)*,Accuracy*,PushNotification*,Battery Recording*,";

                                if (dtrows.Rows[i]["Active"].ToString() == "0")
                                {
                                    if (dtrows.Rows[i]["BlockReason*"].ToString().Length > 90)
                                    {

                                        row["Remark"] += "BlockReason*";
                                    }
                                }
                                row["Result"] = "Error";
                                row["ResultType"] = "Error";
                                dtuploadResult.Rows.Add(row);
                                continue;
                            }
                        }
                        else if (dtrows.Rows[i]["DMT App"].ToString() == "1")
                        {
                            if (dtrows.Rows[i]["From Time*"].ToString().Length > 80 || dtrows.Rows[i]["To Time*"].ToString().Length > 80 || dtrows.Rows[i]["Record Interval (in seconds)*"].ToString().Length > 50 || dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString().Length > 50 || dtrows.Rows[i]["Accuracy*"].ToString().Length > 30 || dtrows.Rows[i]["PushNotification*"].ToString().Length > 50 || dtrows.Rows[i]["Battery Recording*"].ToString().Length > 50)
                            {
                                row = dtuploadResult.NewRow();
                                row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Please supply values for mandatory fields as per mentioned size -";
                                row["Remark"] += "From Time*,To Time*,Record Interval (in seconds)*,Upload Interval (in seconds)*,Accuracy*,PushNotification*,Battery Recording*";
                                row["Result"] = "Error";
                                row["ResultType"] = "Error";
                                dtuploadResult.Rows.Add(row);
                                continue;
                            }
                        }
                        else if (dtrows.Rows[i]["Active"].ToString() == "0")
                        {

                            if (dtrows.Rows[i]["BlockReason*"].ToString().Length > 90)
                            {
                                row = dtuploadResult.NewRow();
                                row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Please supply values for mandatory fields as per mentioned size  - BlockReason* ";
                                row["Result"] = "Error";
                                row["ResultType"] = "Error";
                                dtuploadResult.Rows.Add(row);
                                continue;
                            }
                        }

                    }
                    #endregion


                    #region Insertion/Updation
                    #region CheckValidValue

                    bool isValid = true;
                    row = dtuploadResult.NewRow();
                    row["Result"] = "Error";
                    row["ResultType"] = "Error";
                    row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Please supply values as Per DataType - ";

                    if (!Char.IsLetter(dtrows.Rows[i]["Employee Name*"].ToString(), 0))
                    {
                        isValid = false;
                        row["Remark"] += " Employee Name* can't start with special character,";
                    }

                    //if(dtrows.Rows[i]["DOB"].ToString()!="")
                    //{
                    //    DateTime Dob;
                    //    if(!DateTime.TryParseExact(dtrows.Rows[i]["DOB"].ToString(),"yyyy-MM-dd",CultureInfo.InvariantCulture, DateTimeStyles.None, out Dob))
                    //    {
                    //        isValid = false;
                    //        row["Remark"] += " Invalid Date Format of DOB - " + dtrows.Rows[i]["DOB"].ToString()+",Please supply date as given Date format";
                    //    }
                    //    //if (!DateTime.TryParse(dtrows.Rows[i]["DOB"].ToString(), out Dob))
                    //    //{
                    //    //    isValid = false;
                    //    //    row["Remark"] += " Invalid Date Format of DOB - " + dtrows.Rows[i]["DOB"].ToString();
                    //    //}
                    //}

                    int Value;
                    if (!int.TryParse(dtrows.Rows[i]["Pincode"].ToString(), out Value))
                    {
                        isValid = false;
                        row["Remark"] += " , Invalid PinCode";
                    }

                    if (Regex.IsMatch(dtrows.Rows[i]["Mobile*"].ToString(), @"[a-zA-Z]"))
                    {
                        isValid = false;
                        row["Remark"] += " Invalid Mobile* ,";
                    }


                    var isValidMail = new EmailAddressAttribute();
                    if (!isValidMail.IsValid(dtrows.Rows[i]["Emalid*"].ToString()))
                    {
                        isValid = false;
                        row["Remark"] += " , Invalid Emalid* ";
                    }
                    if (dtrows.Rows[i]["DMT App"].ToString() == "1")
                    {

                        // isValid = false;
                        if (dtrows.Rows[i]["From Time*"].ToString() == "00:00" || dtrows.Rows[i]["To Time*"].ToString() == "00:00")
                            row["Remark"] += " , invalid From Time*,To Time* ";
                        else
                        {
                            TimeSpan ts;
                            if (!TimeSpan.TryParse(dtrows.Rows[i]["From Time*"].ToString(), out ts))
                            {
                                isValid = false;
                                row["Remark"] += " ,  From Time* Format(HH:MM) seems incorrect.";
                            }
                            if (!TimeSpan.TryParse(dtrows.Rows[i]["To Time*"].ToString(), out ts))
                            {
                                isValid = false;
                                row["Remark"] += " ,  To Time* Format(HH:MM) seems incorrect. ";
                            }

                            //var regexItem = new Regex(@"/^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/");
                            //if (!regexItem.IsMatch(dtrows.Rows[i]["From Time*"].ToString()))
                            //{
                            //    isValid = false;
                            //    row["Remark"] += " ,  From Time* Format(HH:MM) seems incorrect.";
                            //}
                            //if (!regexItem.IsMatch(dtrows.Rows[i]["To Time*"].ToString()))
                            //{
                            //    isValid = false;
                            //    row["Remark"] += " ,  To Time* Format(HH:MM) seems incorrect. ";
                            //}
                        }
                        if (dtrows.Rows[i]["Record Interval (in seconds)*"].ToString() == "0")
                        {
                            isValid = false;
                            row["Remark"] += " , Record Interval (in seconds)* Can't zero  ";
                        }
                        if (dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString() == "0")
                        {
                            isValid = false;
                            row["Remark"] += " , Upload Interval (in seconds)*  should be minimum of 5 minutes  ";
                        }
                        if (dtrows.Rows[i]["Accuracy*"].ToString() == "0")
                        {
                            isValid = false;
                            row["Remark"] += " , Accuracy* Can't zero  ";
                        }
                    }


                    if (!isValid)
                    {
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }


                    #endregion

                    #region CheckValue

                    row = dtuploadResult.NewRow();

                    if (Convert.ToDateTime("01/01/2007 " + dtrows.Rows[i]["From Time*"].ToString() + "") > Convert.ToDateTime("01/01/2007 " + dtrows.Rows[i]["From Time*"].ToString() + ""))
                    {

                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Given To Time Should be greater then From Time.";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }


                    if (dtrows.Rows[i]["DMT App"].ToString() == "1")
                    {
                        FromTime = dtrows.Rows[i]["From Time*"].ToString();
                        ToTime = dtrows.Rows[i]["To Time*"].ToString();
                        RecordInterval = dtrows.Rows[i]["Record Interval (in seconds)*"].ToString();
                        UploadInterval = dtrows.Rows[i]["Upload Interval (in seconds)*"].ToString();
                        Accuracy = dtrows.Rows[i]["Accuracy*"].ToString();
                        PushNoti = dtrows.Rows[i]["PushNotification*"].ToString();
                        BatteryRecord = dtrows.Rows[i]["Battery Recording*"].ToString();
                    }


                    underid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select smid from mastsalesrep where smname='" + dtrows.Rows[i]["Reporting Person*"].ToString().Replace("'", "''") + "'"));
                    if (underid == "") underid = "1";

                    RoleId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleId from MastRole where RoleName='" + dtrows.Rows[i]["Role*"].ToString().Replace("'", "''") + "'"));
                    if (RoleId == "")
                    {
                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Given Role " + dtrows.Rows[i]["Role*"].ToString() + " is not found.";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }

                    deptId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select DepId from MastDepartment where DepName='" + dtrows.Rows[i]["Department*"].ToString().Replace("'", "''") + "'"));
                    if (deptId == "")
                    {
                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Given Department " + dtrows.Rows[i]["Department*"].ToString() + " is not found.";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }

                    desgId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select DesId from MastDesignation where DesName='" + dtrows.Rows[i]["Designation*"].ToString().Replace("'", "''") + "'"));
                    if (desgId == "")
                    {
                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Given Designation " + dtrows.Rows[i]["Designation*"].ToString() + " is not found.";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }

                    CityId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select AreaId from MastArea where AreaType='CITY'  and AreaName='" + dtrows.Rows[i]["City*"].ToString() + "'"));
                    if (CityId == "")
                    {
                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Given City " + dtrows.Rows[i]["City*"].ToString() + " is not found.";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }

                    HqId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select id from MastHeadquarter where HeadquarterName='" + dtrows.Rows[i]["HeadQuarter*"].ToString() + "'"));
                    if (HqId == "")
                    {
                        row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Given HeadQuarter* " + dtrows.Rows[i]["HeadQuarter*"].ToString() + " is not found.";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }

                    string SalesPerType = "";
                    string roletype = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select roletype from mastrole where roleid='" + RoleId + "'"));
                    if ((roletype == "AreaIncharge") || (roletype == "CityHead") || (roletype == "DistrictHead") || (roletype == "RegionHead") || (roletype == "StateHead"))
                    {

                        SalesPerType = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select  rolevalue from mastroletype where rolevalue in ('AreaIncharge','CityHead','DistrictHead','RegionHead','StateHead') and roletype='" + dtrows.Rows[i]["SalesPersonType*"].ToString() + "'"));

                        if (SalesPerType == "")
                        {
                            row["Remark"] = "At SalesPerson Info (row " + (i + 2) + ") - SalesPesron  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Given SalesPersonType " + dtrows.Rows[i]["SalesPersonType*"].ToString() + " is not found.";
                            row["Result"] = "Error";
                            row["ResultType"] = "Error";
                            dtuploadResult.Rows.Add(row);
                            continue;
                        }


                    }
                    else
                    {
                        SalesPerType = "";
                    }



                    #endregion

                    try
                    {



                        str = @"select * from mastsalesrep where smname='" + dtrows.Rows[i]["Employee Name*"].ToString().ToUpper().Replace("'", "''") + "'";
                        DataTable dtSales = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtSales.Rows.Count > 0)
                        {
                            SMID = Convert.ToInt32(dtSales.Rows[0]["smid"].ToString());
                        }
                        if (SMID == 0)
                        {

                            #region Insert

                            str = @"select Count(*) from MastLogin where Name='" + dtrows.Rows[i]["User Name*"].ToString().ToUpper().Replace("'", "''") + "'";
                            string active = "0";
                            val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

                            if (val == 0)
                            {
                                if (dtrows.Rows[i]["Active"].ToString() != "0")
                                {
                                    active = "1"; Active = true;
                                }

                                retsave = usall.Insert(dtrows.Rows[i]["User Name*"].ToString(), "12345678", dtrows.Rows[i]["Emalid*"].ToString(), Convert.ToInt32(RoleId), Active, deptId, desgId, dtrows.Rows[i]["Employee Name*"].ToString(), Convert.ToDecimal(0), dtrows.Rows[i]["SyncId"].ToString());

                                if (retsave == -2)
                                {
                                    row = dtuploadResult.NewRow();
                                    row["Result"] = "Error";
                                    row["Remark"] = "Error! While Creating Login SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate EmpSyncId Exists ";
                                    row["ResultType"] = "Error";
                                    dtuploadResult.Rows.Add(row);
                                    continue;
                                }
                                if (retsave == -3)
                                {
                                    row = dtuploadResult.NewRow();
                                    row["Result"] = "Error";
                                    row["Remark"] = "Error! While Creating Login SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate Email Id Exists ";
                                    row["ResultType"] = "Error";
                                    dtuploadResult.Rows.Add(row);
                                    continue;
                                }
                                if (retsave > 0)
                                {

                                    string UserID = retsave.ToString(), EmailID = dtrows.Rows[i]["Emalid*"].ToString(), Employee = dtrows.Rows[i]["Employee Name*"].ToString(), Rescentre = "None";

                                    // string SalesPerType = dtrows.Rows[i]["SalesPersonType*"].ToString() != "" ? dtrows.Rows[i]["SalesPersonType*"].ToString() : "";

                                    string sendPush = dtrows.Rows[i]["PushNotification*"].ToString() == "1" ? "Y" : "N";
                                    string BatteryRec = dtrows.Rows[i]["Battery Recording*"].ToString() == "1" ? "Y" : "N";

                                    bool FieldApp = dtrows.Rows[i]["Field App"].ToString() == "1" ? true : false, DMTApp = dtrows.Rows[i]["DMT App"].ToString() == "1" ? true : false, ManagerApp = dtrows.Rows[i]["Manager App"].ToString() == "1" ? true : false, IshomeCity = dtrows.Rows[i]["Allow to change home city"].ToString() == "1" ? true : false;
                                    string dob = dtrows.Rows[i]["DOB"].ToString();// Convert.ToDateTime(dtrows.Rows[i]["DOB"].ToString()).ToString("dd/MMM/yyyy");

                                    string DOJ = dtrows.Rows[i]["JoiningDate*"].ToString();

                                    int retval = SP.InsertSalespersons(Employee, dtrows.Rows[i]["Pincode"].ToString(), SalesPerType, dtrows.Rows[i]["Mobile*"].ToString(), "0", active, dtrows.Rows[i]["Address1"].ToString(), dtrows.Rows[i]["Address2"].ToString(), CityId, EmailID, dtrows.Rows[i]["Mobile*"].ToString(), "", RoleId, Convert.ToInt32(UserID), dtrows.Rows[i]["SyncId"].ToString(), underid, Convert.ToInt32(0), Convert.ToInt32(deptId), Convert.ToInt32(desgId), dob, "", Rescentre, dtrows.Rows[i]["BlockReason*"].ToString(), Convert.ToInt32(Settings.Instance.UserID), Employee, IshomeCity, Convert.ToInt32(0), FieldApp, FromTime, ToTime, RecordInterval, UploadInterval, Accuracy, sendPush, BatteryRec, "0", "0", Convert.ToBoolean(1), Convert.ToBoolean(1), "N", "Y", 0, "Y", "Y", "", "", Convert.ToInt32(HqId), DOJ);
                                    if (active == "1")
                                    {
                                        str = @"update MastLogin set Active=1 where Id=" + UserID + "";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                                    }
                                    else
                                    {
                                        str = @"update MastLogin set Active=0 where Id=" + UserID + "";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                                    }
                                    if (retval == -1)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Inserting  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate SalesPersons Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");

                                        continue;
                                    }
                                    else if (retval == -2)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Inserting  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate SyncId Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");

                                        continue;
                                    }
                                    else if (retval == -3)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Inserting  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate  Device No Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                                        continue;
                                    }
                                    else if (retval == -4)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Inserting  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate  Email ID Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                                        continue;
                                    }
                                    else if (retval == -5)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Inserting  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate  Mobile No Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                                        continue;

                                    }
                                    else
                                    {
                                        if (dtrows.Rows[i]["SyncId"].ToString() == "")
                                        {
                                            string syncid = "update Mastsalesrep set SyncId='" + retval + "' where SMId=" + retval + "";
                                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);

                                            string empsyncid = "update MastLogin set EmpSyncId='" + retval + "' where Id=" + UserID + "";
                                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, empsyncid);
                                        }

                                        string mastenviro = "select * from mastenviro";
                                        DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                                        if (dtenviro.Rows.Count > 0)
                                        {
                                            if (FieldApp == true)
                                            {
                                                if (upd.Insert_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), Employee, dtenviro.Rows[0]["compurl"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), "FFMS"))
                                                {

                                                }
                                                else
                                                {
                                                    row = dtuploadResult.NewRow();
                                                    row["Result"] = "Error";
                                                    row["Remark"] = " Error! While Creating Field License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                                    row["ResultType"] = "Error";
                                                    dtuploadResult.Rows.Add(row);

                                                }
                                            }
                                            if (ManagerApp == true)
                                            {
                                                if (dtrows.Rows[i]["Role*"].ToString() == "L2" || dtrows.Rows[i]["Role*"].ToString() == "L3" || dtrows.Rows[i]["Role*"].ToString().ToLower() == "admin")
                                                {
                                                    if (upd.Insert_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), Employee, dtenviro.Rows[0]["compurl"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), "CRM MANAGER"))
                                                    {
                                                        int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set Managerapp='" + ManagerApp + "' where SMId=" + retval + "");
                                                    }
                                                    else
                                                    {
                                                        row = dtuploadResult.NewRow();
                                                        row["Result"] = "Error";
                                                        row["Remark"] = " Error! While Creating  MANAGER License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                                        row["ResultType"] = "Error";
                                                        dtuploadResult.Rows.Add(row);
                                                    }
                                                }
                                                else
                                                {
                                                    if (ManagerApp == true)
                                                    {
                                                        row = dtuploadResult.NewRow();
                                                        row["Result"] = "Error";
                                                        row["Remark"] = " Error! While Creating  MANAGER License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Error : For Manager license ,Role must be L2,L3 or Admin.";
                                                        row["ResultType"] = "Error";
                                                        dtuploadResult.Rows.Add(row);
                                                    }

                                                }
                                            }
                                            if (DMTApp == true)
                                            {
                                                if (upd.Insert_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), Employee, dtenviro.Rows[0]["compurl"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), "TRACKER"))
                                                {
                                                    int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set DMTApp='" + DMTApp + "' where SMId=" + retval + "");
                                                }
                                                else
                                                {
                                                    row = dtuploadResult.NewRow();
                                                    row["Result"] = "Error";
                                                    row["ResultType"] = "Error";
                                                    row["Remark"] = " Error! While Creating TRACKER License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                                    dtuploadResult.Rows.Add(row);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            row = dtuploadResult.NewRow();
                                            row["Result"] = "Error";
                                            row["ResultType"] = "Error";
                                            row["Remark"] = " Error! Enviro Table Have No Data - SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                            dtuploadResult.Rows.Add(row);

                                        }


                                    }
                                    row = dtuploadResult.NewRow();
                                    row["Result"] = "Success";
                                    row["Remark"] = " Inserted Successfully - SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                    row["ResultType"] = "Insert";
                                    dtuploadResult.Rows.Add(row);
                                }

                            }
                            else
                            {
                                row = dtuploadResult.NewRow();
                                row["Result"] = "Error";
                                row["ResultType"] = "Error";
                                row["Remark"] = "Error! UserName Already Exists - SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                dtuploadResult.Rows.Add(row);
                            }

                            #endregion
                        }
                        else
                        {
                            #region Update

                            string userid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select userid from mastsalesrep where smid='" + SMID + "'"));

                            str = @"select Count(*) from MastLogin where Name='" + dtrows.Rows[i]["User Name*"].ToString().ToUpper().Replace("'", "''") + "' and Id<>" + Convert.ToInt32(userid) + "";
                            string active = "0";
                            val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

                            if (val == 0)
                            {
                                if (dtrows.Rows[i]["Active"].ToString() != "0") { active = "1"; Active = true; }

                                retsave = usall.Update(Convert.ToInt32(userid), dtrows.Rows[i]["User Name*"].ToString(), "", dtrows.Rows[i]["Emalid*"].ToString(), Convert.ToInt32(RoleId), Active, deptId, desgId, dtrows.Rows[i]["Employee Name*"].ToString(), Convert.ToDecimal(0), dtrows.Rows[i]["SyncId"].ToString());

                                if (retsave == -2)
                                {
                                    row = dtuploadResult.NewRow();
                                    row["Result"] = "Error";
                                    row["Remark"] = "Error! While Updating Login SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate EmpSyncId Exists ";
                                    row["ResultType"] = "Error";
                                    dtuploadResult.Rows.Add(row);
                                    continue;
                                }
                                if (retsave == -3)
                                {
                                    row = dtuploadResult.NewRow();
                                    row["Result"] = "Error";
                                    row["Remark"] = "Error! While Updating Login SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate Email Id Exists ";
                                    row["ResultType"] = "Error";
                                    dtuploadResult.Rows.Add(row);
                                    continue;
                                }
                                if (retsave > 0)
                                {

                                    string UserID = userid, EmailID = dtrows.Rows[i]["Emalid*"].ToString(), Employee = dtrows.Rows[i]["Employee Name*"].ToString(), Rescentre = "None";

                                    // string SalesPerType = dtrows.Rows[i]["SalesPersonType*"].ToString() != "" ? dtrows.Rows[i]["SalesPersonType*"].ToString() : "";

                                    string sendPush = dtrows.Rows[i]["PushNotification*"].ToString() == "1" ? "Y" : "N";
                                    string BatteryRec = dtrows.Rows[i]["Battery Recording*"].ToString() == "1" ? "Y" : "N";
                                    bool FieldApp = dtrows.Rows[i]["Field App"].ToString() == "1" ? true : false, DMTApp = dtrows.Rows[i]["DMT App"].ToString() == "1" ? true : false, ManagerApp = dtrows.Rows[i]["Manager App"].ToString() == "1" ? true : false, IshomeCity = dtrows.Rows[i]["Allow to change home city"].ToString() == "1" ? true : false;
                                    string dob = dtrows.Rows[i]["DOB"].ToString();// Convert.ToDateTime(dtrows.Rows[i]["DOB"].ToString()).ToString("dd/MMM/yyyy");
                                    string DOJ = dtrows.Rows[i]["JoiningDate*"].ToString(), DOL = dtrows.Rows[i]["LeavingDate"].ToString();

                                    int retval = SP.UpdateSalespersons(Convert.ToInt32(SMID), Employee, dtrows.Rows[i]["Pincode"].ToString(), SalesPerType, dtrows.Rows[i]["Mobile*"].ToString(), "0", active, dtrows.Rows[i]["Address1"].ToString(), dtrows.Rows[i]["Address2"].ToString(), CityId, EmailID, dtrows.Rows[i]["Mobile*"].ToString(), "", RoleId, Convert.ToInt32(UserID), dtrows.Rows[i]["SyncId"].ToString(), underid, Convert.ToInt32(0), Convert.ToInt32(deptId), Convert.ToInt32(desgId), dob, "", Rescentre, dtrows.Rows[i]["BlockReason*"].ToString(), Convert.ToInt32(Settings.Instance.UserID), Employee, IshomeCity, Convert.ToInt32(0), FieldApp, FromTime, ToTime, RecordInterval, UploadInterval, Accuracy, sendPush, BatteryRec, "0", "0", Convert.ToBoolean(1), Convert.ToBoolean(1), "N", "Y", 0, "Y", "Y", "", "", Convert.ToInt32(HqId), DOJ, DOL);

                                    if (active == "1")
                                    {
                                        str = @"update MastLogin set Active=1 where Id=" + UserID + "";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                                    }
                                    else
                                    {
                                        str = @"update MastLogin set Active=0 where Id=" + UserID + "";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                                    }
                                    if (retval == -1)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";

                                        row["Remark"] = "Error! While Updating  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate SalesPersons Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");

                                        continue;
                                    }
                                    else if (retval == -2)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Updating  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate SyncId Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");

                                        continue;
                                    }
                                    else if (retval == -3)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Updating  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate  Device No Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                                        continue;
                                    }
                                    else if (retval == -4)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Updating  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate  Email ID Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                                        continue;
                                    }
                                    else if (retval == -5)
                                    {
                                        row = dtuploadResult.NewRow();
                                        row["Result"] = "Error";
                                        row["Remark"] = "Error! While Updating  SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : Duplicate  Mobile No Exists ";
                                        row["ResultType"] = "Error";
                                        dtuploadResult.Rows.Add(row);

                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                                        continue;

                                    }
                                    else
                                    {
                                        if (dtrows.Rows[i]["SyncId"].ToString() == "")
                                        {
                                            string syncid = "update Mastsalesrep set SyncId='" + retval + "' where SMId=" + retval + "";
                                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);

                                            string empsyncid = "update MastLogin set EmpSyncId='" + retval + "' where Id=" + UserID + "";
                                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, empsyncid);
                                        }

                                        string mastenviro = "select * from mastenviro";
                                        DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                                        if (dtenviro.Rows.Count > 0)
                                        {
                                            //if (Convert.ToBoolean(dtrows.Rows[i]["Field App"].ToString()) == true)
                                            //{

                                            if (upd.Update_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), Employee, dtenviro.Rows[0]["compurl"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), active, dtSales.Rows[0]["DeviceNo"].ToString(), FieldApp, "FFMS", dtSales.Rows[0]["Mobile"].ToString()))
                                            {
                                                if (active == "1")
                                                {
                                                    int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set mobileaccess='" + FieldApp + "' where SMId=" + retval + "");
                                                }
                                                else
                                                {
                                                    int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set mobileaccess='" + false + "' where SMId=" + retval + "");
                                                }
                                            }
                                            else
                                            {
                                                row = dtuploadResult.NewRow();
                                                row["Result"] = "Error";
                                                row["Remark"] = " Error! While Updating Field License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                                row["ResultType"] = "Error";
                                                dtuploadResult.Rows.Add(row);

                                            }
                                            // }

                                            //if (Convert.ToBoolean(dtrows.Rows[i]["Manager App"].ToString()) == true)
                                            //{
                                            if (dtrows.Rows[i]["Role*"].ToString() == "L2" || dtrows.Rows[i]["Role*"].ToString() == "L3" || dtrows.Rows[i]["Role*"].ToString().ToLower() == "admin")
                                            {

                                                if (upd.Update_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), Employee, dtenviro.Rows[0]["compurl"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), active, dtSales.Rows[0]["DeviceNo"].ToString(), ManagerApp, "CRM MANAGER", dtSales.Rows[0]["Mobile"].ToString()))
                                                {

                                                    if (active == "1")
                                                    {
                                                        int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set Managerapp='" + ManagerApp + "' where SMId=" + retval + "");
                                                    }
                                                    else
                                                    {
                                                        int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set Managerapp='" + false + "' where SMId=" + retval + "");
                                                    }

                                                }
                                                else
                                                {

                                                    row = dtuploadResult.NewRow();
                                                    row["Result"] = "Error";
                                                    row["ResultType"] = "Error";
                                                    row["Remark"] = " Error! While Updating  MANAGER License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                                    dtuploadResult.Rows.Add(row);
                                                }
                                            }
                                            else
                                            {
                                                if (ManagerApp == true)
                                                {
                                                    row = dtuploadResult.NewRow();
                                                    row["Result"] = "Error";
                                                    row["ResultType"] = "Error";
                                                    row["Remark"] = " Error! While Updating  MANAGER License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " Error : For Manager license ,Role must be L2,L3 or Admin.";
                                                    dtuploadResult.Rows.Add(row);
                                                }

                                            }
                                            //  }

                                            //if (Convert.ToBoolean(dtrows.Rows[i]["DMT App"].ToString()) == true)
                                            //{


                                            if (upd.Update_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), Employee, dtenviro.Rows[0]["compurl"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), active, dtSales.Rows[0]["DeviceNo"].ToString(), DMTApp, "TRACKER", dtSales.Rows[0]["Mobile"].ToString()))
                                            {
                                                if (active == "1")
                                                {
                                                    int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set DMTApp='" + DMTApp + "' where SMId=" + retval + "");
                                                }
                                                else
                                                {
                                                    int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set DMTApp='" + false + "' where SMId=" + retval + "");
                                                }


                                            }
                                            else
                                            {
                                                row = dtuploadResult.NewRow();
                                                row["Result"] = "Error";
                                                row["Remark"] = " Error! While Updating TRACKER License SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                                row["ResultType"] = "Error";
                                                dtuploadResult.Rows.Add(row);
                                            }
                                            // }
                                        }
                                        else
                                        {
                                            row = dtuploadResult.NewRow();
                                            row["Result"] = "Error";
                                            row["Remark"] = " Error! Enviro Table Have No Data - SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                            row["ResultType"] = "Error";
                                            dtuploadResult.Rows.Add(row);

                                        }


                                    }
                                    row = dtuploadResult.NewRow();
                                    row["Result"] = "Success";
                                    row["Remark"] = " Updated Successfully - SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                    row["ResultType"] = "Update";
                                    dtuploadResult.Rows.Add(row);
                                }

                            }
                            else
                            {
                                row = dtuploadResult.NewRow();
                                row["Result"] = "Error";
                                row["Remark"] = "Error! UserName Already Exists - SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + " ";
                                row["ResultType"] = "Error";
                                dtuploadResult.Rows.Add(row);
                            }

                            #endregion
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        row = dtuploadResult.NewRow();
                        row["Result"] = "Error";
                        row["Remark"] = " Error While Inserting/Updating SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : " + ex.ToString();
                        row["ResultType"] = "Error";
                        dtuploadResult.Rows.Add(row);
                        continue;
                    }
                    #endregion


                }
                catch (Exception ex)
                {
                    ex.ToString();
                    row = dtuploadResult.NewRow();
                    row["Result"] = "Error";
                    row["Result"] = " Error While Inserting/Updating SalesPerson Info Data at   (row " + (i + 2) + ")  : SalesPerson  - " + dtrows.Rows[i]["Employee Name*"].ToString() + "  Error : " + ex.ToString();
                    row["ResultType"] = "Error";
                    dtuploadResult.Rows.Add(row);
                    continue;
                }

            }

            return dtuploadResult;
        }

        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            if (dtcols.Columns[0].ToString().Trim() != "Reporting Person*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Employee Name*".ToLower() || dtcols.Columns[2].ToString().Trim() != "User Name*".ToLower() || dtcols.Columns[3].ToString().Trim() != "Role*".ToLower().Trim() || dtcols.Columns[4].ToString().Trim() != "SalesPersonType*".ToLower() || dtcols.Columns[5].ToString().Trim() != "Department*".ToLower() || dtcols.Columns[6].ToString().Trim() != "Designation*".ToLower() || dtcols.Columns[7].ToString().Trim() != "DOB".ToLower() || dtcols.Columns[8].ToString().Trim() != "Address1".ToLower() || dtcols.Columns[9].ToString().Trim() != "Address2".ToLower() || dtcols.Columns[10].ToString().Trim() != "City*".ToLower() || dtcols.Columns[11].ToString().Trim() != "Pincode".ToLower() || dtcols.Columns[12].ToString().Trim() != "Mobile*".ToLower() || dtcols.Columns[13].ToString().Trim() != "Emalid*".ToLower() || dtcols.Columns[14].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[15].ToString().Trim() != "From Time*".ToLower() || dtcols.Columns[16].ToString().Trim() != "To Time*".ToLower() || dtcols.Columns[17].ToString().Trim() != "Record Interval (in seconds)*".ToLower() || dtcols.Columns[18].ToString().Trim() != "Upload Interval (in seconds)*".ToLower() || dtcols.Columns[19].ToString().Trim() != "Accuracy*".ToLower() || dtcols.Columns[20].ToString().Trim() != "PushNotification*".ToLower() || dtcols.Columns[21].ToString().Trim() != "Battery Recording*".ToLower() || dtcols.Columns[22].ToString().Trim() != "Active".ToLower() || dtcols.Columns[23].ToString().Trim() != "BlockReason*".ToLower() || dtcols.Columns[24].ToString().Trim() != "Allow to change home city".ToLower() || dtcols.Columns[25].ToString().Trim() != "Field App".ToLower() || dtcols.Columns[26].ToString().Trim() != "DMT App".ToLower() || dtcols.Columns[27].ToString().Trim() != "Manager App".ToLower() || dtcols.Columns[28].ToString().Trim() != "HeadQuarter*".ToLower() || dtcols.Columns[28].ToString().Trim() != "JoiningDate*".ToLower() || dtcols.Columns[28].ToString().Trim() != "LeavingDate".ToLower())
                Valid = false;
            lblcols.InnerText += "Reporting Person*,Employee Name*,User Name*,Role*,SalesPersonType*,Department*,Designation*,DOB,Address1,Address2,City*,Pincode,Mobile*,Emalid*,SyncId,From Time*,To Time*,Record Interval (in seconds)*,Upload Interval (in seconds)*,Accuracy*,PushNotification*,Battery Recording*,Active,BlockReason*,Allow to change home city,Field App,DMT App,Manager App,HeadQuarter*,JoiningDate*,LeavingDate" + spaces;

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
        }

        
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtresult =(DataTable)ViewState["dtResult"];
                GenerateExcel(dtresult);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}