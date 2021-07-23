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
    public partial class UploadDistributorInvoice : System.Web.UI.Page
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
                InsertDistributorInvoice(dt);
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
        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:DistInvDocId have No value";
            }
            if (c == 1)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:VDate have No value";
            }
            if (c == 2)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
            }
            if (c == 4)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ItemCodeSyncCode have No value";
            }
            if (c == 12)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:EmployeeSyncId have No value";
            }
            if (c == 13)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationSyncID have No value";
            }
            return _errorMessage;
        }
        private void InsertDistributorInvoice(DataTable dtrows)
        {
            string _fMessage = "";
            ImportParty IMPARTY = new ImportParty();
            lblcols.InnerText = "";
            int intDel = 1;
            //Code for Update (delete)

            //  int intDel = IMPARTY.DeleteInvoiceData(dtrows);

            if (intDel == 1)
            {
                for (int i = 0; i < dtrows.Rows.Count; i++)
                {

                    //                    if (string.IsNullOrEmpty(dtrows.Rows[i]["DistInvDocId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["VDate*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["SyncId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["ItemCodeSyncCode*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["EmployeeSyncId*"].ToString()))
                    //                    {
                    //                        ShowAlert("At DistInvDocId -" + dtrows.Rows[i]["DistInvDocId*"].ToString() + @" Please supply values for mandatory fields - 
                    //            DistInvDocId*,VDate*,SyncId*,ItemCodeSyncCode*,SyncID*,EmployeeSyncId*,LocationSyncID*"); return;
                    //                    }

                    decimal Qty = 0, Rate = 0, Amt = 0, TaxPer = 0, TaxAmt = 0, ExciseAmt = 0, netTotal = 0, QtyinKg = 0;

                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Qty"].ToString()))
                        Qty = Convert.ToDecimal(dtrows.Rows[i]["Qty"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Rate"].ToString()))
                        Rate = Convert.ToDecimal(dtrows.Rows[i]["Rate"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Amount"].ToString()))
                        Amt = Convert.ToDecimal(dtrows.Rows[i]["Amount"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["taxPer"].ToString()))
                        TaxPer = Convert.ToDecimal(dtrows.Rows[i]["taxPer"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Tax_Amt"].ToString()))
                        TaxAmt = Convert.ToDecimal(dtrows.Rows[i]["Tax_Amt"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["ExciseAmount"].ToString()))
                        ExciseAmt = Convert.ToDecimal(dtrows.Rows[i]["ExciseAmount"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Net_Total"].ToString()))
                        netTotal = Convert.ToDecimal(dtrows.Rows[i]["Net_Total"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Qty (KG)"].ToString()))
                        QtyinKg = Convert.ToDecimal(dtrows.Rows[i]["Qty (KG)"]);
                    try
                    {
                        for (int c = 0; c < dtrows.Columns.Count; c++)
                        {
                            _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                        }
                        if (!string.IsNullOrEmpty(_fMessage))
                        {
                            string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','DistributorInvoice','" + dtrows.Rows[i]["Syncid*"].ToString().Replace("'", string.Empty) + "','UploadDistributorInvoice.aspx')";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                            _fMessage = string.Empty;
                        }
                        else
                        {
                            string Msg = IMPARTY.ImportDistributorInvoice(dtrows.Rows[i]["DistInvDocId*"].ToString(),
                                dtrows.Rows[i]["VDate*"].ToString(), dtrows.Rows[i]["SyncId*"].ToString(), dtrows.Rows[i]["PriceList"].ToString(),
                                dtrows.Rows[i]["ItemCodeSyncCode*"].ToString(), Qty, Rate, Amt, TaxPer, TaxAmt, ExciseAmt, netTotal,
                                dtrows.Rows[i]["EmployeeSyncId*"].ToString(), dtrows.Rows[i]["LocationSyncID*"].ToString(), dtrows.Rows[i]["ItemRemarks"].ToString(), dtrows.Rows[i]["Porder"].ToString(), dtrows.Rows[i]["Category"].ToString(), QtyinKg);
                            if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                        }
                    }
                    catch (Exception ex) { ShowAlert("Incorrect Data at Distributor Invoice : " + dtrows.Rows[i]["DistInvDocId"].ToString()); ex.ToString(); }
                }
                string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='DistributorInvoice' Order by [Created_At] desc";
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
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Distributors Invoice Imported Successfully');", true);
                dt.Dispose();
                //ShowAlert("Distributors Invoice Imported Successfully");
            }
        }

        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces in between columns names";
            if (dtcols.Columns[0].ToString().Trim() != "DistInvDocId*".ToLower() || dtcols.Columns[1].ToString().Trim() != "VDate*".ToLower() || dtcols.Columns[2].ToString().Trim() != "SyncId*".ToLower().Trim() || dtcols.Columns[3].ToString().Trim() != "PriceList".ToLower() || dtcols.Columns[4].ToString().Trim() != "ItemCodeSyncCode*".ToLower() ||
                dtcols.Columns[5].ToString().Trim() != "Qty".ToLower() ||
              dtcols.Columns[6].ToString().Trim() != "Rate".ToLower() || dtcols.Columns[7].ToString().Trim() != "Amount".ToLower() ||
              dtcols.Columns[8].ToString().Trim() != "taxPer".ToLower() || dtcols.Columns[9].ToString().Trim() != "Tax_Amt".ToLower() ||
              dtcols.Columns[10].ToString().Trim() != "ExciseAmount".ToLower() || dtcols.Columns[11].ToString().Trim() != "Net_Total".ToLower() ||
              dtcols.Columns[12].ToString().Trim() != "EmployeeSyncId*".ToLower() || dtcols.Columns[13].ToString().Trim() != "LocationSyncID*".ToLower()
                || dtcols.Columns[14].ToString().Trim() != "ItemRemarks".ToLower() || dtcols.Columns[15].ToString().Trim() != "Porder".ToLower() || dtcols.Columns[16].ToString().Trim() != "Category".ToLower() || dtcols.Columns[17].ToString().Trim() != "Qty (KG)".ToLower())
                Valid = false;
            lblcols.InnerText += "DistInvDocId*,VDate*,SyncId*,PriceList,ItemCodeSyncCode*,Qty,Rate,Amount,taxPer,Tax_Amt,ExciseAmount,Net_Total,EmployeeSyncId*,LocationSyncID*,Porder,Category,Qty (KG)" + spaces;

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
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='DistributorInvoice' Order by [Created_At] desc";
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
    }
}