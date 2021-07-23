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
using BusinessLayer;

namespace FFMS
{
    public partial class UploadGRData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
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
                InsertItems(dt);
            }
            else
            {
                lblcols.Visible = true;
                ShowAlert("Incorrect Column Names.Please Check Uploaded File.");
            }

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
        private void InsertItems(DataTable dtrows)
        {
            ImportGrData Grdata = new ImportGrData();
            lblcols.InnerText = "";

        
                for (int i = 0; i < dtrows.Rows.Count; i++)
                {    try
                      {
                          if (string.IsNullOrEmpty(dtrows.Rows[i]["GrDocid*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Invoiceno*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Invoicedate*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Biltydate*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["DistributorSynchID*"].ToString()))
                          { ShowAlert("At GrDocid -" + dtrows.Rows[i]["GrDocid*"].ToString() + " Please supply values for mandatory fields - GrDocid*,Invoiceno*,Invoicedate*,Biltydate*,DistributorSynchID*"); return; }

                    decimal BillAmt = 0;
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Biltyamount"].ToString())) 
                    {BillAmt=Convert.ToDecimal(dtrows.Rows[i]["Biltyamount"]); }

                  string Msg=  Grdata.InsertGRData(dtrows.Rows[i]["GrDocid*"].ToString(), dtrows.Rows[i]["Invoiceno*"].ToString(), Convert.ToDateTime(dtrows.Rows[i]["Invoicedate*"]), dtrows.Rows[i]["Transpotername"].ToString(), dtrows.Rows[i]["Cartonno"].ToString(), dtrows.Rows[i]["Biltyno"].ToString(), Convert.ToDateTime(dtrows.Rows[i]["Biltydate*"]), BillAmt, dtrows.Rows[i]["DistributorSynchID*"].ToString());
                  if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                       }
                catch (Exception ex) { ShowAlert("Incorrect Data at GrDocId : " + dtrows.Rows[i]["GrDocid*"].ToString()); ex.ToString(); }
                 }
                ShowAlert("GR Data Imported Successfully");
          
        }
        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            if (dtcols.Columns[0].ToString().Trim() != "GrDocid*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Invoiceno*".ToLower() || dtcols.Columns[2].ToString().Trim() != "Invoicedate*".ToLower() || dtcols.Columns[3].ToString().Trim() != "Transpotername".ToLower() || dtcols.Columns[4].ToString().Trim() != "Cartonno".ToLower().Trim() || dtcols.Columns[5].ToString().Trim() != "Biltyno".ToLower() || dtcols.Columns[6].ToString().Trim() != "Biltydate*".ToLower() || dtcols.Columns[7].ToString().Trim() != "Biltyamount".ToLower() || dtcols.Columns[8].ToString().Trim() != "DistributorSynchID*".ToLower())
                Valid = false;
            lblcols.InnerText += "GrDocid,Invoiceno,Invoicedate(dd/mm/yyyy),Transpotername,Cartonno,Biltyno,Biltydate(dd/mm/yyyy),Biltyamount,DistributorSynchID*" + spaces;

            return Valid;
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
    }
}