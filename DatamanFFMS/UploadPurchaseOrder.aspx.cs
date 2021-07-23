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
    public partial class UploadPurchaseOrder : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();

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
                    dt = data.ReadXLSWithDataTypeCols("~/FileUploads/", filename, "PurchaseOrder$", true);
                    DataTable dtcols = new DataTable();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtcols.Columns.Add(dt.Columns[i].ToString().ToLower(), typeof(string));
                    }
                    dtcols.AcceptChanges();
                    if (GetValidCols(dtcols))
                    {
                        lblcols.Visible = false;
                        InsertPurchaseOrder(dt);
                    }
                    else { lblcols.Visible = true; }

                    dt.Dispose();
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
        }

        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PODocId have No value";
            }
            if (c == 1)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:VDate have No value";
            }
            if (c == 2)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:DistSyncId have No value";
            }
            if (c == 3)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PortalNo have No value";
            }
            if (c == 4)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ItemSyncID have No value";
            }
            if (c == 5)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Qty have No value";
                else if(!string.IsNullOrEmpty(value)){
                    int value1;
                    if (!int.TryParse(value.ToString(), out value1)) {
                        _errorMessage = " Row: " + r + " And Column:Qty Must have int value";
                    }
            }
            }
            if (c == 6)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationSyncID have No value";
            }
            if (c == 8)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ItemwiseTotal have No value";
            }
            if (c == 9)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PendingQty have No value";
            }
            if (c == 10)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Rate have No value";
            }
            return _errorMessage;
        }
        private void InsertPurchaseOrder(DataTable dtrows)
        {            
            lblcols.InnerText = "";
            string Query1 = "delete from PurchaseOrderImport", _fMessage="";
            DbConnectionDAL.ExecuteQuery(Query1);

            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                try
                {                          
                   

//                    if (string.IsNullOrEmpty(dtrows.Rows[i]["PODocId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Vdate*"].ToString()) ||
//                         string.IsNullOrEmpty(dtrows.Rows[i]["DistSyncId*"].ToString()) ||
//                        string.IsNullOrEmpty(dtrows.Rows[i]["ItemSyncID*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Qty*"].ToString())
//                         || string.IsNullOrEmpty(dtrows.Rows[i]["LocationSyncID*"].ToString())
//                         || string.IsNullOrEmpty(dtrows.Rows[i]["PendingQty*"].ToString())
//                        )

//                    {
//                        ShowAlert("At DocID -" + dtrows.Rows[i]["PODocId*"].ToString() + @" Please supply values for mandatory fields - 
//                                    PODocId*,Vdate*,EmployeeSyncId*,DistSyncId*,PortalNo*,ItemSyncID*,Qty*,LocationSyncID*,ItemwiseTotal*,PendingQty*,Rate*"); return; 
//                    }
                     for (int c = 0; c < dtrows.Columns.Count; c++)
                        {
                        _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                        }
                     if (!string.IsNullOrEmpty(_fMessage))
                     {
                         string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','PurchaseOrder','','UploadPurchaseOrder.aspx')";
                         DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                         _fMessage = string.Empty;
                     }
                     else
                     {
                         string Msg = dp.InsertPurchaseImportData(dtrows.Rows[i]["PODocId*"].ToString(), dtrows.Rows[i]["Vdate*"].ToString(),
                             "", dtrows.Rows[i]["DistSyncId*"].ToString(),
                             dtrows.Rows[i]["PortalNo*"].ToString(),
                             dtrows.Rows[i]["ItemSyncID*"].ToString(), dtrows.Rows[i]["Qty*"].ToString(),
                             dtrows.Rows[i]["LocationSyncID*"].ToString(), dtrows.Rows[i]["ItemRemarks"].ToString(),
                             dtrows.Rows[i]["ItemwiseTotal*"].ToString(), dtrows.Rows[i]["PendingQty*"].ToString(), dtrows.Rows[i]["Rate*"].ToString(), dtrows.Rows[i]["Shipped"].ToString());
                         if (!string.IsNullOrEmpty(Msg)) { 
                             //ShowAlert(Msg); return; 
                         }
                     }
                }
                catch (Exception ex) { ShowAlert("Incorrect Data at DocID : " + dtrows.Rows[i]["PODocId*"].ToString()); ex.ToString(); }
            }
            string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='PurchaseOrder' Order by [Created_At] desc";
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Purchase Order Invoice Imported Successfully');", true);
            //  ShowAlert("Purchase Order Imported Successfully");
            dt.Dispose();
        }

        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            //if (dtcols.Columns[0].ToString().Trim() != "PODocId*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Vdate*".ToLower() ||
            //    dtcols.Columns[2].ToString().Trim() != "DistSyncId*".ToLower() || dtcols.Columns[3].ToString().Trim() != "PortalNo*".ToLower() ||
            //    dtcols.Columns[4].ToString().Trim() != "ItemSyncID*".ToLower() || dtcols.Columns[5].ToString().Trim() != "Qty*".ToLower() ||               
            //    dtcols.Columns[6].ToString().Trim() != "LocationSyncID*".ToLower() || dtcols.Columns[7].ToString().Trim() != "ItemRemarks".ToLower()
            //    || dtcols.Columns[8].ToString().Trim() != "ItemwiseTotal*".ToLower() || dtcols.Columns[9].ToString().Trim() != "PendingQty*".ToLower()
            //   )
            if (dtcols.Columns[0].ToString().Trim() != "PODocId*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Vdate*".ToLower() ||
                dtcols.Columns[2].ToString().Trim() != "DistSyncId*".ToLower() || dtcols.Columns[3].ToString().Trim() != "PortalNo*".ToLower() ||
                dtcols.Columns[4].ToString().Trim() != "ItemSyncID*".ToLower() || dtcols.Columns[5].ToString().Trim() != "Qty*".ToLower() ||
                dtcols.Columns[6].ToString().Trim() != "LocationSyncID*".ToLower() || dtcols.Columns[7].ToString().Trim() != "ItemRemarks".ToLower()
                || dtcols.Columns[8].ToString().Trim() != "ItemwiseTotal*".ToLower() || dtcols.Columns[9].ToString().Trim() != "PendingQty*".ToLower()
               )
                Valid = false;
            lblcols.InnerText += @"PODocId*,Vdate*,DistSyncId*,
                                    PortalNo*,ItemSyncID*,Qty*,
                                   LocationSyncID*,ItemRemarks,ItemwiseTotal*,PendingQty*" + spaces;
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
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='PurchaseOrder' Order by [Created_At] desc";
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