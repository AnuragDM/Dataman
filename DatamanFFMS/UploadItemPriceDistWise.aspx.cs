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
using FFMS;
using DAL;
using BusinessLayer;

namespace AstralFFMS
{
    public partial class UploadItemPriceDistWise : System.Web.UI.Page
    {
        string pageName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            pageName = Path.GetFileName(Request.Path);           
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                
            }
            fillRepeter();
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
                InsertItemPrice(dt);
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

        private void InsertItemPrice(DataTable dtrows)
        {
            string insertsql = "", result = "", updatesql = "", _Query = "", _QueryItem = "";
            int index = 2; int distid = 0, itemid = 0;
            ImportDistLedger DistLedger = new ImportDistLedger();
            lblcols.InnerText = "";

            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                string errorMsg = "", addError = ""; 
                if (!string.IsNullOrEmpty(dtrows.Rows[i]["DistributorPrice*"].ToString()))
                {
                    addError = " - Distributor Price:" + dtrows.Rows[i]["DistributorPrice*"].ToString();
                }
                else
                {
                    addError = "";
                }
                index++;
                if (string.IsNullOrEmpty(dtrows.Rows[i]["DistributorSynchid*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["ItemSyncCode*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["DistributorPrice*"].ToString()))
                {
                    string error = "At Data Row No -" + index.ToString() + " Please supply values for mandatory fields - DistributorSynchid*,ItemSyncCode*,DistributorPrice*";
                   // ShowAlert("At DistributorSynchid -" + dtrows.Rows[i]["DistributorSynchid*"].ToString() + " Please supply values for mandatory fields - DistributorSynchid*,ItemSyncCode*,DistributorPrice*"); return; 
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage3", "errormessage3('" + error + "');", true);
                    return;
                }

                if (Convert.ToString(dtrows.Rows[i]["DistributorSynchid*"]) != "")
                {
                     distid = DbConnectionDAL.GetIntScalarVal("select PartyId from MastParty where partydist=1 and syncid = '" + dtrows.Rows[i]["DistributorSynchid*"].ToString() + "'");
                    if (distid <= 0)
                    {                     
                        errorMsg += "Invalid Distributor at Data Row No " + index.ToString() + addError + ".<br>";                       
                    }
                }
                if (Convert.ToString(dtrows.Rows[i]["ItemSyncCode*"]) != "")
                {
                     itemid = DbConnectionDAL.GetIntScalarVal("select ItemId from MastItem where ItemType='Item' and syncid = '" + dtrows.Rows[i]["ItemSyncCode*"].ToString() + "'");
                    if (itemid <= 0)
                    {                      
                        errorMsg += "Invalid Item at Data Row No " + index.ToString() + addError + ".<br>";                     
                    }
                }  
                
                if (String.IsNullOrEmpty(errorMsg))
                {
                    try
                    {
                        result = DbConnectionDAL.GetStringScalarVal("select count(*) from [dbo].[MastItemPriceDistWise] where DistId = " + distid + " and  ItemId = " + itemid + "");
                        int productGroupId = DbConnectionDAL.GetIntScalarVal("Select UnderId from MastItem where ItemType='Item' and  ItemId = " + itemid + "");
                        if (Convert.ToInt32(result) == 0)                      
                        {

                            insertsql = "INSERT INTO [dbo].[MastItemPriceDistWise] ([DistId],[ItemId],[DistPrice],[CreatedByUserId],[CreateDate],[ProdGrpId],[LastModifiedDate])" +
                                                    " VALUES(" + distid + "," + itemid + "," + Convert.ToDouble(dtrows.Rows[i]["DistributorPrice*"]) + "," + Settings.Instance.UserID + ",DateAdd(minute,330,getutcdate())," + productGroupId + ",DateAdd(minute,330,getutcdate()))";
                            DbConnectionDAL.ExecuteQuery(insertsql);
                        }
                        else if (Convert.ToInt32(result) == 1)
                        {
                            updatesql = "update [dbo].[MastItemPriceDistWise] set DistPrice= " + Convert.ToDouble(dtrows.Rows[i]["DistributorPrice*"]) + ",[ProdGrpId]=" + productGroupId + ", LastModifiedDate = DateAdd(minute, 330, getutcdate()) where DistId = " + distid + " and  ItemId = " + itemid + "";
                            DbConnectionDAL.ExecuteQuery(updatesql);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ex.ToString();
                        errorMsg += "Invalid Data at Row No " + index.ToString() + addError + ".<br>";
                    }
                }
                else
                {
                    insertsql = "INSERT INTO [dbo].[Import_Excel_log] ([Created_At],[Error_Desc],[Filter],[SyncId],[Form_Name])" +
                                                " VALUES(DateAdd(minute,330,getutcdate()),'" + errorMsg + "','ItemPrice','" + dtrows.Rows[i]["DistributorSynchid*"].ToString() + "','" + pageName + "')";
                    DbConnectionDAL.ExecuteQuery(insertsql);
                }               
            }
            txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
            fillRepeter();
            ShowAlert("Item Price Imported Successfully");
        }
        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces in between column names";
            if (dtcols.Columns[0].ToString().Trim() != "DistributorSynchid*".ToLower() || dtcols.Columns[1].ToString().Trim() != "ItemSyncCode*".ToLower() || dtcols.Columns[2].ToString().Trim() != "DistributorPrice*".ToLower())
                Valid = false;
            lblcols.InnerText += "DistributorSynchid*,ItemSyncCode*,DistributorPrice*" + spaces;

            return Valid;
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        private void fillRepeter()
        {
            string str = @"Select Error_desc,Created_At from Import_Excel_log where form_name='" + pageName + "' and Created_At between '" + Settings.dateformat(txtfmDate.Text) + " 00:01' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59'  order by Created_At desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = @"Select Error_desc,Created_At from Import_Excel_log where form_name='" + pageName + "' and Created_At between '" + Settings.dateformat(txtfmDate.Text) + " 00:01' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59'  order by Created_At desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }

    }
}