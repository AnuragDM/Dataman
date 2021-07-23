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
    public partial class UploadDistributor : System.Web.UI.Page
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
                InsertDistributor(dt);

                Row = null;
            }
            else
            {
                lblcols.Visible = true;
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Incorrect Column Names.Please Check Uploaded File.');", true);
               // ShowAlert("Incorrect Column Names.Please Check Uploaded File.");
            }
            dt.Dispose();
        }
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
                    dt = data.ReadXLSWithDataTypeCols("~/FileUploads/", filename, "Distributor$", true);
                    DataTable dtcols = new DataTable();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtcols.Columns.Add(dt.Columns[i].ToString().ToLower(), typeof(string));
                    }
                    dtcols.AcceptChanges();
                    if (GetValidCols(dtcols))
                    {
                        lblcols.Visible = false;
                        InsertDistributor(dt);
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
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You have not specified a file.');", true);
                //ShowAlert("You have not specified a file.");
            }

            dt.Dispose();
        }
        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:DistributorName have No value";
            }
            if (c == 3)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:City have No value";
            }
            if (c == 5)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Email have No value";
            }
            if (c == 6)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:MobileNo have No value";
            }
            if (c == 11)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
            }
            if (c == 12)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:RoleName have No value";
            }
            if (c == 13)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
            }
            //if (c == 14)
            //{
            //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:RoleName have No value";
            //}
            if (c == 20)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:EmployeeSyncId have No value";
            }
            if (c == 25)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Area have No value";
            }
            if (c == 26)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:DistributorType have No value";
            }
            return _errorMessage;
        }
        private void InsertDistributor(DataTable dtrows)
        {

            ImportParty IMPARTY = new ImportParty();
            lblcols.InnerText = "";
            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                string _fMessage = "";
                    decimal CreditLimit = 0, Outstanding = 0; decimal OpenOrder = 0;
                    int CreditDays = 0;
                    bool active = false;
                    string val = "";


                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["CreditLimit"].ToString()))
                        CreditLimit = Convert.ToDecimal(dtrows.Rows[i]["CreditLimit"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Outstanding"].ToString()))
                        Outstanding = Convert.ToDecimal(dtrows.Rows[i]["Outstanding"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["CreditDays"].ToString()))
                        CreditDays = Convert.ToInt32(dtrows.Rows[i]["CreditDays"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["OpenOrder"].ToString()))
                        OpenOrder = Convert.ToDecimal(dtrows.Rows[i]["OpenOrder"]);
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Active"].ToString()))
                        val = dtrows.Rows[i]["Active"].ToString();
                    if (val == "1")
                    {
                        active = true;
                        //       active = Convert.ToBoolean(dtrows.Rows[i]["Active"]);
                    }
                    try
                    {
                        for (int c = 0; c < dtrows.Columns.Count; c++)
                        {
                        _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                        }
                        if (!string.IsNullOrEmpty(_fMessage))
                        {
                            string _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Distributor','" + dtrows.Rows[i]["SyncID*"].ToString().Replace("'", string.Empty) + "','UploadDistributor.aspx')";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                            _fMessage = string.Empty;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(dtrows.Rows[i]["DistributorName*"].ToString()))
                            {
                                string DistType = "", SuperDistID = "";
                                SuperDistID = "0";

                                DistType = DbConnectionDAL.GetStringScalarVal("Select Value from MastDistributorType where Text='" + dtrows.Rows[i]["DistributorType*"].ToString().Replace("'", string.Empty) + "'");
                                //if(dtrows.Rows[i]["DistributorType*"].ToString().Replace("'", string.Empty) == "Distributor")
                                //{
                                //    DistType = "DIST";                                    
                                //}
                                //if (dtrows.Rows[i]["DistributorType*"].ToString().Replace("'", string.Empty) == "Super Distributor")
                                //{
                                //    DistType = "SUPERDIST";                                    
                                //}
                                if (DistType != "DEPO" && DistType != "DIST" && !string.IsNullOrEmpty(dtrows.Rows[i]["SuperDistributorSyncId"].ToString()))
                                {
                                   // DistType = "UNDERSD";
                                    SuperDistID = DbConnectionDAL.GetStringScalarVal("Select PartyId from MastParty where partydist=1 and SyncId='" + dtrows.Rows[i]["SuperDistributorSyncId"].ToString().Replace("'", string.Empty) + "'");
                                }
                                
                                //if (string.IsNullOrEmpty(dtrows.Rows[i]["DistributorName*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["City*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Email*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["MobileNo*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["SyncID*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["RoleName*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["EmployeeSyncId*"].ToString()))
                                //{ ShowAlert("At Distributor -" + dtrows.Rows[i]["DistributorName*"].ToString() + " Please supply values for mandatory fields - DistributorName*,City*,Email*,MobileNo*,SyncID*,RoleName*,EmployeeSyncId*"); return; }

                                string Msg = IMPARTY.ImportDistributors(dtrows.Rows[i]["DistributorName*"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Address1"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Address2"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["City*"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Area*"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Pin"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Email*"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["MobileNo*"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Phone"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["CSTNo"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["VatTIN"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["ServiceTAxRegNo"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["PanNo"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["SyncID*"].ToString().Trim().Replace("'", string.Empty), "", dtrows.Rows[i]["RoleName*"].ToString().Replace("'", string.Empty), active, CreditLimit, Outstanding, CreditDays, OpenOrder, dtrows.Rows[i]["Remarks"].ToString(), dtrows.Rows[i]["EmployeeSyncId*"].ToString(), dtrows.Rows[i]["DistributorName2"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["ContactPerson"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Telex"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Fax"].ToString().Replace("'", string.Empty), DistType, SuperDistID);
                                if (!string.IsNullOrEmpty(Msg)) { 
                                   // ShowAlert(Msg);
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(" + Msg + ");", true);
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception ex) { 
                        ShowAlert("Incorrect Data at Distributor : " + dtrows.Rows[i]["DistributorName*"].ToString()); 
                        
                        ex.ToString(); }
            
                }

            string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='Distributor' Order by [Created_At] desc";
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Distributors Imported Successfully');", true);
            //ShowAlert("Distributors Imported Successfully");
            dt.Dispose();
        }

        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces in between columns names";

            if (dtcols.Columns["distributorname*"].ToString().Trim() == "distributorname*")
            {
                string nm = "Name";
            }
            //if (dtcols.Columns[0].ToString().Trim() != "DistributorName*".ToLower() || dtcols.Columns[1].ToString().Trim() != "Address1".ToLower() || dtcols.Columns[2].ToString().Trim() != "Address2".ToLower().Trim() || dtcols.Columns[3].ToString().Trim() != "City*".ToLower() || dtcols.Columns[4].ToString().Trim() != "Pin".ToLower() ||
            //    dtcols.Columns[5].ToString().Trim() != "Email*".ToLower() ||
            //  dtcols.Columns[6].ToString().Trim() != "MobileNo*".ToLower() || dtcols.Columns[7].ToString().Trim() != "CSTNo".ToLower() || dtcols.Columns[8].ToString().Trim() != "VatTIN".ToLower() || dtcols.Columns[9].ToString().Trim() != "ServiceTAxRegNo".ToLower() || dtcols.Columns[10].ToString().Trim() != "PanNo".ToLower() || dtcols.Columns[11].ToString().Trim() != "SyncID*".ToLower() || dtcols.Columns[12].ToString().Trim() != "RoleName*".ToLower() || dtcols.Columns[13].ToString().Trim() != "Active".ToLower() || dtcols.Columns[14].ToString().Trim() != "CreditLimit".ToLower() || dtcols.Columns[15].ToString().Trim() != "Outstanding".ToLower() || dtcols.Columns[16].ToString().Trim() != "CreditDays".ToLower() || dtcols.Columns[17].ToString().Trim() != "OpenOrder".ToLower() || dtcols.Columns[18].ToString().Trim() != "Remarks".ToLower() || dtcols.Columns[19].ToString().Trim() != "Phone".ToLower() || dtcols.Columns[20].ToString().Trim() != "EmployeeSyncId*".ToLower() || dtcols.Columns[21].ToString().Trim() != "DistributorName2".ToLower() || dtcols.Columns[22].ToString().Trim() != "ContactPerson".ToLower() || dtcols.Columns[23].ToString().Trim() != "Telex".ToLower() || dtcols.Columns[24].ToString().Trim() != "Fax".ToLower() || dtcols.Columns[25].ToString().Trim() != "Area*".ToLower())
            if (dtcols.Columns["distributorname*"].ToString().Trim() != "distributorname*".ToLower() || dtcols.Columns["address1"].ToString().Trim() != "address1".ToLower() || dtcols.Columns["address2"].ToString().Trim() != "address2".ToLower().Trim() || dtcols.Columns["city*"].ToString().Trim() != "city*".ToLower() || dtcols.Columns["pin"].ToString().Trim() != "pin".ToLower() ||
              dtcols.Columns["email*"].ToString().Trim() != "email*".ToLower() ||
            dtcols.Columns["mobileNo*"].ToString().Trim() != "mobileNo*".ToLower() || dtcols.Columns["cstno"].ToString().Trim() != "cstno".ToLower() || dtcols.Columns["vattin"].ToString().Trim() != "vattin".ToLower() || dtcols.Columns["servicetaxregno"].ToString().Trim() != "servicetaxregno".ToLower() || dtcols.Columns["panno"].ToString().Trim() != "panno".ToLower() || dtcols.Columns["syncid*"].ToString().Trim() != "syncid*".ToLower() || dtcols.Columns["rolename*"].ToString().Trim() != "rolename*".ToLower() || dtcols.Columns["active"].ToString().Trim() != "active".ToLower() || dtcols.Columns["creditlimit"].ToString().Trim() != "creditlimit".ToLower() || dtcols.Columns["outstanding"].ToString().Trim() != "outstanding".ToLower() || dtcols.Columns[16].ToString().Trim() != "creditdays".ToLower() || dtcols.Columns[17].ToString().Trim() != "openorder".ToLower() || dtcols.Columns[18].ToString().Trim() != "remarks".ToLower() || dtcols.Columns[19].ToString().Trim() != "phone".ToLower() || dtcols.Columns[20].ToString().Trim() != "employeesyncid*".ToLower() || dtcols.Columns[21].ToString().Trim() != "distributorname2".ToLower() || dtcols.Columns[22].ToString().Trim() != "contactperson".ToLower() || dtcols.Columns[23].ToString().Trim() != "telex".ToLower() || dtcols.Columns[24].ToString().Trim() != "fax".ToLower() || dtcols.Columns[25].ToString().Trim() != "area*".ToLower() || dtcols.Columns[26].ToString().Trim() != "DistributorType*".ToLower() || dtcols.Columns[27].ToString().Trim() != "SuperDistributorSyncId".ToLower())
                Valid = false;
            lblcols.InnerText += "DistributorName*,Address1,Address2,City*,Pin,Email*,MobileNo.*,CSTNo.,VatTIN,ServiceTAxReg.No.,PanNO.,SyncID*, RoleName*,Active,CreditLimit,Outstanding,CreditDays,OpenOrder,Remarks,Phone,EmployeeSyncId*,DistributorName2,ContactPerson,Telex,Fax,Area*,DistributorType*,SuperDistributorSyncId" + spaces;

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
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='Distributor' Order by [Created_At] desc";
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