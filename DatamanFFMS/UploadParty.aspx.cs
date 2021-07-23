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
    public partial class UploadParty : System.Web.UI.Page
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
        //        InsertParty(dt);
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
                    dt = data.ReadXLSWithDataTypeCols("~/FileUploads/", filename, "Party$", true);
                    DataTable dtcols = new DataTable();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtcols.Columns.Add(dt.Columns[i].ToString().ToLower(), typeof(string));
                    }
                    dtcols.AcceptChanges();
                    if (GetValidCols(dtcols))
                    {
                        lblcols.Visible = false;
                        InsertParty(dt);
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
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PartyName have No value";
            }
            if (c == 11)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PartyType have No value";
            }

            if (c == 12)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:State have No value";
            }
            if (c == 13)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:District have No value";
            }
            if (c == 23)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:PartyId have No value";
                else if (!string.IsNullOrEmpty(value))
                {
                    int value1;
                    Int32 PartyId;
                    string Msg = "";
                    if (!Int32.TryParse(value.ToString(), out PartyId))
                    {
                        _errorMessage = " Row: " + r + " And Column:PartyId Should be an Integer value";
                    }
                }
            }
            if (c == 14)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:City have No value";
            }
            if (c == 15)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Area have No value";
            }
            if (c == 16)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Beat have No value";
            }

            if (c == 3)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Pin have No value";
            }
            if (c == 13)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:EMail have No value";
            }


            return _errorMessage;
        }
        private void InsertParty(DataTable dtrows)
        {
            string _fMessage = "";
            ImportParty IMPARTY = new ImportParty();
            lblcols.InnerText = "";
            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                try
                {

                    //if (string.IsNullOrEmpty(dtrows.Rows[i]["PartyName*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["PartyType*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["State*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["District*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["City*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Area*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Beat*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Pin*"].ToString()))
                    //{ ShowAlert("At Party -" + dtrows.Rows[i]["PartyName*"].ToString() + " Please supply values for mandatory fields - PartyName*,PartyType*,State*,District*,City*,Area*,Beat*,PartyId*,Pin*,EMail"); return; }

                    //Int32 PartyId;
                    string Msg = "";
                    //if (!Int32.TryParse(dtrows.Rows[i]["PartyId*"].ToString(), out PartyId)) { ShowAlert("At Party Name -" + dtrows.Rows[i]["PartyName*"].ToString() + " - PartyId Should be an Integer value"); return; }
                    for (int c = 0; c < dtrows.Columns.Count; c++)
                    {
                        _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                    }
                    if (!string.IsNullOrEmpty(_fMessage))
                    {
                        string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Party','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadParty.aspx')";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                        _fMessage = string.Empty;
                    }
                    else
                    {
                        Msg = IMPARTY.InsertParties(dtrows.Rows[i]["PartyName*"].ToString(), dtrows.Rows[i]["Address1"].ToString(), dtrows.Rows[i]["Address2"].ToString(), dtrows.Rows[i]["Pin*"].ToString(), dtrows.Rows[i]["Mobile*"].ToString(), dtrows.Rows[i]["Industry*"].ToString(), dtrows.Rows[i]["Potential"].ToString(), dtrows.Rows[i]["Remark"].ToString(), dtrows.Rows[i]["DistributorSynchId*"].ToString().Trim(), dtrows.Rows[i]["SyncId"].ToString(), dtrows.Rows[i]["Active"].ToString(), dtrows.Rows[i]["PartyType*"].ToString(), dtrows.Rows[i]["State*"].ToString(), dtrows.Rows[i]["District*"].ToString(), dtrows.Rows[i]["City*"].ToString(), dtrows.Rows[i]["Area*"].ToString(), dtrows.Rows[i]["Beat*"].ToString(), dtrows.Rows[i]["ContactPerson"].ToString(), dtrows.Rows[i]["Phone"].ToString(), dtrows.Rows[i]["CSTNo"].ToString(), dtrows.Rows[i]["VATTIN"].ToString(), dtrows.Rows[i]["ServiceTaxRegNo"].ToString(), dtrows.Rows[i]["PanNo"].ToString(), dtrows.Rows[i]["PartyId*"].ToString(), dtrows.Rows[i]["EMail"].ToString(), dtrows.Rows[i]["GSTINNo"].ToString());
                        if (!string.IsNullOrEmpty(Msg))
                        {
                            // ShowAlert(Msg); 
                            //return; 
                        }
                    }
                }
                catch (Exception ex) { ShowAlert("Incorrect Data at Party Name : " + dtrows.Rows[i]["PartyName*"].ToString()); ex.ToString(); }
            }
            string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='Party' Order by [Created_At] desc";
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Parties Imported Successfully');", true);
            //  ShowAlert("Parties Imported Successfully");      
            dt.Dispose();
        }

        private Boolean GetValidCols(DataTable dtcols)
        {

            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            if (dtcols.Columns[0].ToString().Trim() != "PartyName*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Address1".ToLower()
                || dtcols.Columns[2].ToString().Trim() != "Address2".ToLower() || dtcols.Columns[3].ToString().Trim() != "Pin*".ToLower().Trim() ||
                dtcols.Columns[4].ToString().Trim() != "Mobile*".ToLower() || dtcols.Columns[5].ToString().Trim() != "Industry*".ToLower() ||
                dtcols.Columns[6].ToString().Trim() != "Potential".ToLower() || dtcols.Columns[7].ToString().Trim() != "Remark".ToLower() ||
                dtcols.Columns[8].ToString().Trim() != "DistributorSynchId*".ToLower() || dtcols.Columns[9].ToString().Trim() != "SyncId".ToLower() ||
                dtcols.Columns[10].ToString().Trim() != "Active".ToLower() || dtcols.Columns[11].ToString().Trim() != "PartyType*".ToLower() ||
             dtcols.Columns[12].ToString().Trim() != "State*".ToLower() || dtcols.Columns[13].ToString().Trim() != "District*".ToLower() ||
             dtcols.Columns[14].ToString().Trim() != "City*".ToLower() || dtcols.Columns[15].ToString().Trim() != "Area*".ToLower() ||
             dtcols.Columns[16].ToString().Trim() != "Beat*".ToLower() ||
             dtcols.Columns[17].ToString().Trim() != "ContactPerson".ToLower() || dtcols.Columns[18].ToString().Trim() != "Phone".ToLower()
             || dtcols.Columns[19].ToString().Trim() != "CSTNo".ToLower() || dtcols.Columns[20].ToString().Trim() != "VATTIN".ToLower() ||
             dtcols.Columns[21].ToString().Trim() != "ServiceTaxRegNo".ToLower() || dtcols.Columns[22].ToString().Trim() != "PanNo".ToLower() || dtcols.Columns[23].ToString().Trim() != "PartyId*".ToLower() || dtcols.Columns[24].ToString().Trim() != "EMail".ToLower() || dtcols.Columns[25].ToString().Trim() != "GSTINNo".ToLower())
                Valid = false;
            lblcols.InnerText += "PartyName*,Address1,Address2,Pin,Mobile,Industry*,Potential,Remark,DistributorSynchId*,SyncID,Active," +
                "PartyType*,State*,District*,City*,Area*,Beat*,ContactPerson,Phone,CSTNo,VATTIN,ServiceTaxRegNo,PanNo,PartyId*,EMail,GSTINNo" + spaces;

            return Valid;

            //if (Settings.Instance.OrderEntryType != "1")
            //{
            //    bool Valid = true;
            //    lblcols.InnerText = "Required columns format :";
            //    string spaces = ". * Please discard blank spaces after/before columns names";
            //    if (dtcols.Columns[0].ToString().Trim() != "PartyName*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Address1".ToLower()
            //        || dtcols.Columns[2].ToString().Trim() != "Address2".ToLower() || dtcols.Columns[3].ToString().Trim() != "Pin*".ToLower().Trim() ||
            //        dtcols.Columns[4].ToString().Trim() != "Mobile*".ToLower() || dtcols.Columns[5].ToString().Trim() != "Industry*".ToLower() ||
            //        dtcols.Columns[6].ToString().Trim() != "Potential".ToLower() || dtcols.Columns[7].ToString().Trim() != "Remark".ToLower() ||
            //        dtcols.Columns[8].ToString().Trim() != "DistributorSynchId*".ToLower() || dtcols.Columns[9].ToString().Trim() != "SyncId".ToLower() ||
            //        dtcols.Columns[10].ToString().Trim() != "Active".ToLower() || dtcols.Columns[11].ToString().Trim() != "PartyType*".ToLower() ||
            //     dtcols.Columns[12].ToString().Trim() != "State*".ToLower() || dtcols.Columns[13].ToString().Trim() != "District*".ToLower() ||
            //     dtcols.Columns[14].ToString().Trim() != "City*".ToLower() || dtcols.Columns[15].ToString().Trim() != "Area*".ToLower() ||
            //     dtcols.Columns[16].ToString().Trim() != "Beat*".ToLower() ||
            //     dtcols.Columns[17].ToString().Trim() != "ContactPerson".ToLower() || dtcols.Columns[18].ToString().Trim() != "Phone".ToLower()
            //     || dtcols.Columns[19].ToString().Trim() != "CSTNo".ToLower() || dtcols.Columns[20].ToString().Trim() != "VATTIN".ToLower() ||
            //     dtcols.Columns[21].ToString().Trim() != "ServiceTaxRegNo".ToLower() || dtcols.Columns[22].ToString().Trim() != "PanNo".ToLower() || dtcols.Columns[23].ToString().Trim() != "PartyId*".ToLower() || dtcols.Columns[24].ToString().Trim() != "EMail".ToLower() || dtcols.Columns[25].ToString().Trim() != "GSTINNo".ToLower())
            //        Valid = false;
            //    lblcols.InnerText += "PartyName*,Address1,Address2,Pin,Mobile,Industry*,Potential,Remark,DistributorSynchId*,SyncID,Active," +
            //        "PartyType*,State*,District*,City*,Area*,Beat*,ContactPerson,Phone,CSTNo,VATTIN,ServiceTaxRegNo,PanNo,PartyId*,EMail,GSTINNo" + spaces;

            //    return Valid;
            //}
            //else
            //{
            //    bool Valid = true;
            //    lblcols.InnerText = "Required columns format :";
            //    string spaces = ". * Please discard blank spaces after/before columns names";
            //    if (dtcols.Columns[0].ToString().Trim() != "PartyName*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Address1".ToLower()
            //        || dtcols.Columns[2].ToString().Trim() != "Address2".ToLower() || dtcols.Columns[3].ToString().Trim() != "Pin*".ToLower().Trim() ||
            //        dtcols.Columns[4].ToString().Trim() != "Mobile*".ToLower() || dtcols.Columns[5].ToString().Trim() != "Industry*".ToLower() ||
            //        dtcols.Columns[6].ToString().Trim() != "Potential".ToLower() || dtcols.Columns[7].ToString().Trim() != "Remark".ToLower() ||
            //        dtcols.Columns[8].ToString().Trim() != "DistributorSynchId*".ToLower() || dtcols.Columns[9].ToString().Trim() != "SyncId".ToLower() ||
            //        dtcols.Columns[10].ToString().Trim() != "Active".ToLower() || dtcols.Columns[11].ToString().Trim() != "PartyType*".ToLower() ||
            //     dtcols.Columns[12].ToString().Trim() != "State*".ToLower() || dtcols.Columns[13].ToString().Trim() != "District*".ToLower() ||
            //     dtcols.Columns[14].ToString().Trim() != "City*".ToLower() || dtcols.Columns[15].ToString().Trim() != "Area*".ToLower() ||
            //     dtcols.Columns[16].ToString().Trim() != "Beat*".ToLower() ||
            //     dtcols.Columns[17].ToString().Trim() != "ContactPerson".ToLower() || dtcols.Columns[18].ToString().Trim() != "Phone".ToLower()
            //     || dtcols.Columns[19].ToString().Trim() != "CSTNo".ToLower() || dtcols.Columns[20].ToString().Trim() != "VATTIN".ToLower() ||
            //     dtcols.Columns[21].ToString().Trim() != "ServiceTaxRegNo".ToLower() || dtcols.Columns[22].ToString().Trim() != "PanNo".ToLower() || dtcols.Columns[23].ToString().Trim() != "PartyId*".ToLower() || dtcols.Columns[24].ToString().Trim() != "EMail".ToLower() || dtcols.Columns[25].ToString().Trim() != "GSTINNo".ToLower())
            //        Valid = false;
            //    lblcols.InnerText += "PartyName*,Address1,Address2,Pin,Mobile*,Industry*,Potential,Remark,DistributorSynchId*,SyncID,Active," +
            //        "PartyType*,State*,District*,City*,Area*,Beat*,ContactPerson,Phone,CSTNo,VATTIN,ServiceTaxRegNo,PanNo,PartyId*,EMail,GSTINNo" + spaces;

            //    return Valid;
            //}
        }

        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string _sql = "";
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='Party' Order by [Created_At] desc";
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