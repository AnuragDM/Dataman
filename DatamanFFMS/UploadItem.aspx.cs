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
    public partial class UploadItem : System.Web.UI.Page
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

        //   // DataTable dt = new DataTable();
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
        //        InsertItems(dt);
        //    }
        //    else
        //    {
        //        lblcols.Visible = true;
        //        ShowAlert("Incorrect Column Names.Please Check Uploaded File.");
        //    }

        //}
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
                    if (ext.ToLower() != ".xls")
                    {
                        ShowAlert("Please Upload Only .xls File.");
                        return;
                    }
                    string filename = Path.GetFileName(Fupd.FileName);
                    data.UploadFile(Fupd, filename, "~/FileUploads/");
                    dt = data.ReadXLSWithDataTypeCols("~/FileUploads/", filename, "Product$", true);
                    DataTable dtcols = new DataTable();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtcols.Columns.Add(dt.Columns[i].ToString().ToLower(), typeof(string));
                    }
                    dtcols.AcceptChanges();
                    if (GetValidCols(dtcols))
                    {
                        lblcols.Visible = false;
                        InsertItems(dt);
                    }
                    else { lblcols.Visible = true; }

                    dtcols.Dispose();
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
            dt.Dispose();
        }
        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ProductGroupSyncId have No value";
            }
            if (c == 1)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ProductName have No value";
            }
            if (c == 3)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Unit have No value";
            }
            if (c == 5)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:StdPack have No value";
            }
            if (c == 6)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
            }
            if (c == 8)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PriceGroup have No value";
            }
            if (c == 4)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
            }
            //if (c == 14)
            //{
            //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:RoleName have No value";
            //}
            if (c == 9)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ProductClassSyncCode have No value";
            }
            if (c == 10)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ProductSegmentSyncCode have No value";
            }
            return _errorMessage;
        }
        private void InsertItems(DataTable dtrows)
        {

            ImportItems Items = new ImportItems();
            lblcols.InnerText = "";

            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                string Msg = "", _fMessage = "";
                //if (string.IsNullOrEmpty(dtrows.Rows[i]["ProductGroupSyncId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["ProductName*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Unit*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["StdPack*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["SyncId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["PriceGroup*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["ProductClassSyncCode*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["ProductSegmentSyncCode*"].ToString()))
                //{ ShowAlert("At Product Name -" + dtrows.Rows[i]["ProductName*"].ToString() + " Please supply values for mandatory fields - ProductGroupSyncId*,ProductName*,Unit*,StdPack*,SyncId*,PriceGroup*,ProductClassSyncCode*,ProductSegmentSyncCode*"); return; }

                try
                {
                    decimal Mrp = 0;
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Mrp"].ToString()))
                    { Mrp = Convert.ToDecimal(dtrows.Rows[i]["Mrp"]); }
                    decimal Dp = 0;
                    //if (!string.IsNullOrEmpty(dtrows.Rows[i]["Dp"].ToString()))
                    //{ Dp = Convert.ToDecimal(dtrows.Rows[i]["Dp"]); }
                    decimal Rp = 0;
                    //if (!string.IsNullOrEmpty(dtrows.Rows[i]["Rp"].ToString()))
                    //{ Rp = Convert.ToDecimal(dtrows.Rows[i]["Rp"]); }
                    decimal primaryunitfactor = 0, secondaryunitfactor = 0, mimimunqty = 0, cgstper = 0, sgstper = 0, igstper = 0;
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Primaryunitfactor"].ToString()))
                        primaryunitfactor = Convert.ToDecimal(dtrows.Rows[i]["Primaryunitfactor"].ToString());
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Secondaryunitfactor"].ToString()))
                        secondaryunitfactor = Convert.ToDecimal(dtrows.Rows[i]["Secondaryunitfactor"].ToString());
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Minimumqty"].ToString()))
                        mimimunqty = Convert.ToDecimal(dtrows.Rows[i]["Minimumqty"].ToString());

                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["CGSTPer"].ToString()))
                        cgstper = Convert.ToDecimal(dtrows.Rows[i]["CGSTPer"].ToString());
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["SGSTPer"].ToString()))
                        sgstper = Convert.ToDecimal(dtrows.Rows[i]["SGSTPer"].ToString());
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["IGSTPer"].ToString()))
                        igstper = Convert.ToDecimal(dtrows.Rows[i]["IGSTPer"].ToString());
                    for (int c = 0; c < dtrows.Columns.Count; c++)
                    {
                        _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                    }
                    if (!string.IsNullOrEmpty(_fMessage))
                    {
                        string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Product','" + dtrows.Rows[i]["SyncID*"].ToString() + "','UploadItem.aspx')";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                        _fMessage = string.Empty;
                    }
                    else
                    {
                        Msg = Items.InsertItems(dtrows.Rows[i]["ProductGroupSyncId*"].ToString(), dtrows.Rows[i]["ProductName*"].ToString().TrimStart().TrimEnd(), dtrows.Rows[i]["ProductCode"].ToString(), dtrows.Rows[i]["Unit*"].ToString(), dtrows.Rows[i]["Active"].ToString(), dtrows.Rows[i]["StdPack*"].ToString(), dtrows.Rows[i]["SyncId*"].ToString(), Mrp, Dp, Rp, dtrows.Rows[i]["PriceGroup*"].ToString(), dtrows.Rows[i]["ProductClassSyncCode*"].ToString(), dtrows.Rows[i]["ProductSegmentSyncCode*"].ToString(), dtrows.Rows[i]["Primaryunit"].ToString(), primaryunitfactor, dtrows.Rows[i]["Secondayunit"].ToString(), secondaryunitfactor, mimimunqty, cgstper, sgstper, igstper, dtrows.Rows[i]["Promoted"].ToString());
                        if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                    }
                }

                catch (Exception ex) { ShowAlert("Incorrect Data at Product : " + dtrows.Rows[i]["ProductName*"].ToString()); ex.ToString(); }

            }
            string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='Product' Order by [Created_At] desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _sql1);
            rptDatabase.DataSource = dt;
            rptDatabase.DataBind();
            rptmain.Style.Add("display", "block");
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Products Imported Successfully');", true);
            //  ShowAlert("Products Imported Successfully");
            dt.Dispose();
        }

        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            //if (dtcols.Columns[0].Caption.ToString().Trim() != "ProductGroupSyncId*".ToLower() || dtcols.Columns[1].ToString().Trim() != "ProductName*".ToLower() || dtcols.Columns[2].ToString().Trim() != "ProductCode".ToLower() || dtcols.Columns[3].ToString().Trim() != "Unit*".ToLower().Trim() || dtcols.Columns[4].ToString().Trim() != "Active".ToLower() || dtcols.Columns[5].ToString().Trim() != "StdPack*".ToLower() || dtcols.Columns[6].ToString().Trim() != "SyncId*".ToLower() || dtcols.Columns[7].ToString().Trim() != "Mrp".ToLower() || dtcols.Columns[8].ToString().Trim() != "PriceGroup*".ToLower() || dtcols.Columns[9].ToString().Trim() != "ProductClassSyncCode*".ToLower() || dtcols.Columns[10].ToString().Trim() != "ProductSegmentSyncCode*".ToLower())
            //    Valid = false;
            //lblcols.InnerText += "ProductGroupSyncId*,ProductName*,ProductCode,Unit*,Active,StdPack*,SyncId*,Mrp,PriceGroup*,ProductClassSyncCode*,ProductSegmentSyncCode*" + spaces;

            if (dtcols.Columns[0].Caption.ToString().Trim() != "ProductGroupSyncId*".ToLower() || dtcols.Columns[1].ToString().Trim() != "ProductName*".ToLower() || dtcols.Columns[2].ToString().Trim() != "ProductCode".ToLower() || dtcols.Columns[3].ToString().Trim() != "Unit*".ToLower().Trim() || dtcols.Columns[4].ToString().Trim() != "Active".ToLower() || dtcols.Columns[5].ToString().Trim() != "StdPack*".ToLower() || dtcols.Columns[6].ToString().Trim() != "SyncId*".ToLower() || dtcols.Columns[7].ToString().Trim() != "Mrp".ToLower() || dtcols.Columns[8].ToString().Trim() != "PriceGroup*".ToLower() || dtcols.Columns[9].ToString().Trim() != "ProductClassSyncCode*".ToLower() || dtcols.Columns[10].ToString().Trim() != "ProductSegmentSyncCode*".ToLower() || dtcols.Columns[11].ToString().Trim() != "Primaryunit".ToLower() || dtcols.Columns[12].ToString().Trim() != "Primaryunitfactor".ToLower() || dtcols.Columns[13].ToString().Trim() != "Secondayunit".ToLower() || dtcols.Columns[14].ToString().Trim() != "Secondaryunitfactor".ToLower() || dtcols.Columns[15].ToString().Trim() != "Minimumqty".ToLower() || dtcols.Columns[16].ToString().Trim() != "CGSTPer".ToLower() || dtcols.Columns[17].ToString().Trim() != "SGSTPer".ToLower() || dtcols.Columns[18].ToString().Trim() != "IGSTPer".ToLower() || dtcols.Columns[19].ToString().Trim() != "Promoted".ToLower())
                Valid = false;
            lblcols.InnerText += "ProductGroupSyncId*,ProductName*,ProductCode,Unit*,Active,StdPack*,SyncId*,Mrp,PriceGroup*,ProductClassSyncCode*,ProductSegmentSyncCode*,Primaryunit,Primaryunitfactor,Secondayunit,Secondaryunitfactor,Minimumqty,CGSTPer,SGSTPer,IGSTPer,Promoted" + spaces;
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
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='Product' Order by [Created_At] desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _sql);
            rptDatabase.DataSource = dt;
            rptDatabase.DataBind();
            rptmain.Style.Add("display", "block");
            dt.Dispose();
        }
    }
}