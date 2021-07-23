using AstralFFMS.ClassFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BusinessLayer;
namespace AstralFFMS
{
    public partial class UploadEmployee : System.Web.UI.Page
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
        private void GetDataTableFromCsv(string path)
        {
            DataTable dt = new DataTable();
            string CSVFilePathName = path;
            string[] Lines = File.ReadAllLines(CSVFilePathName);
            string[] Fields;
            Fields = Lines[0].Split(new char[] { ',' });
            int Cols = Fields.GetLength(0);

            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
                dt.Columns.Add(Fields[i].ToLower().Trim(), typeof(string));
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
                InsertEmployee(dt);
                Row = null;
            }
            else
            {
                lblcols.Visible = true;
                ShowAlert("Incorrect Column Names.Please Check Uploaded File.");
            }
            dt.Dispose();
        }
        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LoginID have No value";
            }
            if (c == 1)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Password have No value";
            }

            if (c == 3)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Email have No value";
            }

            if (c == 4)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:RoleName have No value";
            }
            if (c == 5)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Department have No value";
            }
            if (c == 6)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Designation have No value";
            }
            if (c == 7)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:EmpSyncId have No value";
            }
            if (c == 8)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:EmployeeName have No value";
            }
            if (c == 9)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:CostCentre have No value";
            }
            if (c == 2)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
            }
            return _errorMessage;
        }
        private void InsertEmployee(DataTable dtrows)
        {

            ImportEmployee IE = new ImportEmployee();
            string _fMessage = "";
            lblcols.InnerText = "";
            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                try
                {

                    //                    if (string.IsNullOrEmpty(dtrows.Rows[i]["LoginID*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Password*"].ToString()) || 
                    //                        string.IsNullOrEmpty(dtrows.Rows[i]["Email*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["RoleName*"].ToString()) || 
                    //                        string.IsNullOrEmpty(dtrows.Rows[i]["Department*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Designation*"].ToString()) || 
                    //                        string.IsNullOrEmpty(dtrows.Rows[i]["EmpSyncId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["EmployeeName*"].ToString())
                    //                        || string.IsNullOrEmpty(dtrows.Rows[i]["CostCentre*"].ToString()))
                    //                    { ShowAlert("At LoginID -" + dtrows.Rows[i]["LoginID*"].ToString() + @" Please supply values for mandatory fields - 
                    //                        LoginID*,Password*,Email*,RoleName*,Department*,Designation*,EmpSyncId*,EmployeeName*,CostCentre*"); return;
                    //                    }
                    for (int c = 0; c < dtrows.Columns.Count; c++)
                    {
                        _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                    }
                    if (!string.IsNullOrEmpty(_fMessage))
                    {
                        string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Employee','" + dtrows.Rows[i]["EmpSyncId*"].ToString().Replace("'", string.Empty) + "','UploadEmployee.aspx')";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                        _fMessage = string.Empty;
                    }
                    else
                    {
                        string Msg = IE.UploadEmployee(dtrows.Rows[i]["LoginID*"].ToString(), dtrows.Rows[i]["Password*"].ToString(),
                            dtrows.Rows[i]["Active"].ToString(), dtrows.Rows[i]["Email*"].ToString(), dtrows.Rows[i]["RoleName*"].ToString(),
                            dtrows.Rows[i]["Department*"].ToString(), dtrows.Rows[i]["Designation*"].ToString(), dtrows.Rows[i]["EmpSyncId*"].ToString(),
                            dtrows.Rows[i]["EmployeeName*"].ToString(), dtrows.Rows[i]["CostCentre*"].ToString());
                        if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                    }
                }

                catch (Exception ex) { ShowAlert("Incorrect Data at LoginID : " + dtrows.Rows[i]["LoginID*"].ToString()); ex.ToString(); }
            }

            string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='MaterialGroup' Order by [Created_At] desc";
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Employees Imported Successfully');", true);
            // ShowAlert("Employees Imported Successfully");
            dt.Dispose();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (Fupd.HasFile)
            {
                lblcols.InnerText = "";

                try
                {
                    String ext = System.IO.Path.GetExtension(Fupd.FileName);
                    if (ext.ToLower() != ".csv")
                    {
                        ShowAlert("Please Upload Only .csv File.");
                        return;
                    }
                    string filename = Path.GetFileName(Fupd.FileName);
                    String csv_file_path = Path.Combine(Server.MapPath("~/FileUploads"), filename);
                    Fupd.SaveAs(csv_file_path);
                    GetDataTableFromCsv(csv_file_path);


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
        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            if (dtcols.Columns[0].ToString().Trim() != "LoginID*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Password*".ToLower().Trim() ||
                dtcols.Columns[2].ToString().Trim() != "Active".ToLower().Trim() || dtcols.Columns[3].ToString().Trim() != "Email*".ToLower() ||
                dtcols.Columns[4].ToString().Trim() != "RoleName*".ToLower() || dtcols.Columns[5].ToString().Trim() != "Department*".ToLower() ||
                dtcols.Columns[6].ToString().Trim() != "Designation*".ToLower() || dtcols.Columns[7].ToString().Trim() != "EmpSyncId*".ToLower() ||
                dtcols.Columns[8].ToString().Trim() != "EmployeeName*".ToLower() || dtcols.Columns[9].ToString().Trim() != "CostCentre*".ToLower())
                Valid = false;
            lblcols.InnerText += "LoginID*,Password*,Active,Email*,RoleName*,Department*,Designation*,EmpSyncId*,EmployeeName*,CostCentre*" + spaces;

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
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='Employee' Order by [Created_At] desc";
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