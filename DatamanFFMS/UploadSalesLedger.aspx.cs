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
    public partial class UploadSalesLedger : System.Web.UI.Page
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
                for (int i = 1; i < Lines.GetLength(0); i++)
                {
                    Fields = Lines[i].Split(new char[] { ',' });
                    Row = dt.NewRow();
                    for (int f = 0; f < Cols; f++)
                        Row[f] = Fields[f].Trim();
                    dt.Rows.Add(Row);
                }
                dt.AcceptChanges();
                InsertsalesLedger(dt);
                Row = null;
            }
            else
            {
                lblcols.Visible = true;
                ShowAlert("Incorrect Column Names.Please Check Uploaded File.");
            }
            dt.Dispose();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
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
        }

        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:DocId have No value";
            }
            if (c == 1)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SalesrepSyncID have No value";
            }
         
            if (c == 2)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PostingDate have No value";
            }
          
            return _errorMessage;
        }

        private void InsertsalesLedger(DataTable dtrows)
        {
            string _fMessage = "";
            ImportSalesLedger SalesLedger = new ImportSalesLedger();
            lblcols.InnerText = "";

            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                //if (string.IsNullOrEmpty(dtrows.Rows[i]["DocId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["SalesrepSyncID*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["PostingDate*"].ToString()))
                //{ ShowAlert("At DocId -" + dtrows.Rows[i]["DocId*"].ToString() + " Please supply values for mandatory fields - DocId*,SalesrepSyncID*,PostingDate*"); return; }

                decimal Amount = 0, crAmount = 0, DrAmount = 0;
                if (!string.IsNullOrEmpty(dtrows.Rows[i]["Amount"].ToString()))
                    Amount = Convert.ToDecimal(dtrows.Rows[i]["Amount"]);
                if (!string.IsNullOrEmpty(dtrows.Rows[i]["crAmount"].ToString()))
                    crAmount = Convert.ToDecimal(dtrows.Rows[i]["crAmount"]);
                if (!string.IsNullOrEmpty(dtrows.Rows[i]["DrAmount"].ToString()))
                    DrAmount = Convert.ToDecimal(dtrows.Rows[i]["DrAmount"]);
                try
                {
                    for (int c = 0; c < dtrows.Columns.Count; c++)
                        {
                        _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                        }
                    if (!string.IsNullOrEmpty(_fMessage))
                    {
                        string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','MaterialGroup','" + dtrows.Rows[i]["SalesrepSyncID*"].ToString().Replace("'", string.Empty) + "','UploadSalesLedger.aspx')";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                        _fMessage = string.Empty;
                    }
                    else
                    {
                        string Msg = SalesLedger.InsertSalesLedger(dtrows.Rows[i]["DocId*"].ToString(), dtrows.Rows[i]["SalesrepSyncID*"].ToString(), Convert.ToDateTime(dtrows.Rows[i]["PostingDate*"]), dtrows.Rows[i]["Narration"].ToString(), Amount, crAmount, DrAmount, dtrows.Rows[i]["CompanyCode"].ToString());
                        if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                    }
                }
                catch (Exception ex) { ShowAlert("Incorrect Data at DocID : " + dtrows.Rows[i]["DocId*"].ToString()); ex.ToString(); }
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Salesperson Ledger Imported Successfully');", true);
            dt.Dispose();
            //  ShowAlert("Salesperson Ledger Imported Successfully");
        }
        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces in between column names";
            if (dtcols.Columns[0].ToString().Trim() != "DocId*".ToLower() || dtcols.Columns[1].ToString().Trim() != "SalesrepSyncID*".ToLower() || dtcols.Columns[2].ToString().Trim() != "PostingDate*".ToLower() || dtcols.Columns[3].ToString().Trim() != "Narration".ToLower() || dtcols.Columns[4].ToString().Trim() != "Amount".ToLower() || dtcols.Columns[5].ToString().Trim() != "crAmount".ToLower() || dtcols.Columns[6].ToString().Trim() != "DrAmount".ToLower() || dtcols.Columns[7].ToString().Trim() != "CompanyCode".ToLower())
                
                Valid = false;
            lblcols.InnerText += "DocId*,SalesrepSyncID*,PostingDate*(dd/mm/yyyy),Narration,Amount,crAmount,DrAmount,CompanyCode" + spaces;

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
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='MaterialGroup' Order by [Created_At] desc";
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