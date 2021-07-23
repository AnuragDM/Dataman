using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data;
using System.Globalization;
using System.IO;
using DAL;
using System.Text.RegularExpressions;
using BusinessLayer;
namespace FFMS
{
    public partial class UploadArea : System.Web.UI.Page
    {
        DataTable dtError;
        string _errorMessage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            dtError = new DataTable();
            dtError.Columns.Add("Row", typeof(string));
            dtError.Columns.Add("Column", typeof(string));
            dtError.Columns.Add("Error", typeof(string));
            txtfmDate.Attributes.Add("readonly", "readonly");
            if (!Page.IsPostBack)
            {
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                //dtError.Columns.Add("Row", typeof(string));
                //dtError.Columns.Add("Column", typeof(string));
                //dtError.Columns.Add("Error", typeof(string));
            }
        }
        private DataTable GetDataTableFromCsv(string path)
        {
            string _specialChar = "", _sqlQuery = "";
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
                    {
                        //if (checkSpecialChar(Fields[f].Replace(" ","").Trim()) == true)
                        //{
                        //    string _schar = getColumnName(i, f);
                        //    _specialChar += _schar;
                        //}

                        //_errorMessage= this.getMandatory(i, f, Convert.ToString(Fields[f].Replace(" ", "").Trim()));
                        //if (!string.IsNullOrEmpty(_errorMessage))
                        //{

                        //    DataRow dr = dtError.NewRow();
                        //    dr["Row"] = Convert.ToString(i);
                        //    dr["Column"] = Convert.ToString(f);
                        //    dr["Error"] = Convert.ToString(_errorMessage);
                        //    dtError.Rows.Add(dr);
                        //    _sqlQuery = "Insert into ";

                        //}
                        Row[f] = Fields[f].Trim();
                    }
                    dt.Rows.Add(Row);
                }
                dt.AcceptChanges();
                InsertArea(dt);
                //if (dtError.Rows.Count > 0)
                //{
                //    rpt.DataSource = dtError;
                //    rpt.DataBind();
                //}
                ////    if(!string.IsNullOrEmpty(_specialChar))
                ////         ShowAlert(_specialChar);
                ////    else
                ////
                //    else InsertArea(dt);

            }
            else
            {
                lblcols.Visible = true;
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Incorrect Column Names.Please Check Uploaded File.');", true);
                //  ShowAlert("Incorrect Column Names.Please Check Uploaded File."); 
            }
            return dt;
        }
        public bool checkSpecialChar(string value)
        {
            var regx = new Regex("[^a-zA-Z0-9_.]");
            if (regx.IsMatch(value))
            {
                return true;
            }
            return false;
        }
        public string getColumnName(int r, int c)
        {
            string _colname = "";
            int idx = Convert.ToInt32(ddlArea.SelectedValue);
            switch (idx)
            {
                case 3:
                    if (c == 0) _colname = " Row: " + r + " And Column:Region have Special Char";
                    if (c == 1) _colname = " Row: " + r + " And Column:State have Special Char";
                    if (c == 2) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    if (c == 3) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    if (c == 4) _colname = " Row: " + r + " And Column:Active have Special Char";
                    break;
                case 5:
                    if (c == 0) _colname = " Row: " + r + " And Column:State have Special Char";
                    if (c == 1) _colname = " Row: " + r + " And Column:District have Special Char";
                    if (c == 2) _colname = " Row: " + r + " And Column:City have Special Char";
                    if (c == 3) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    if (c == 4) _colname = " Row: " + r + " And Column:CityType have Special Char";
                    if (c == 5) _colname = " Row: " + r + " And Column:ConveyanceType have Special Char";
                    if (c == 6) _colname = " Row: " + r + " And Column:STD have Special Char";
                    if (c == 7) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    if (c == 8) _colname = " Row: " + r + " And Column:Active have Special Char";
                    break;
                case 6:
                    if (c == 0) _colname = " Row: " + r + " And Column:District have Special Char";
                    if (c == 1) _colname = " Row: " + r + " And Column:City have Special Char";
                    if (c == 2) _colname = " Row: " + r + " And Column:Area have Special Char";
                    if (c == 3) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    if (c == 4) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    if (c == 5) _colname = " Row: " + r + " And Column:Active have Special Char";
                    break;
            }
            return _colname;
        }

        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            int idx = Convert.ToInt32(ddlArea.SelectedValue);
            switch (idx)
            {
                case 1:
                    if (c == 1)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Country have No value";
                    }
                    if (c == 2)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationType have No value";
                    }
                    //if (c == 3)
                    //{
                    //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
                    //}
                    if (c == 4)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
                    }
                    if (c == 5)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ISD have No value";
                    }
                    break;

                case 2:
                    if (c == 0)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Country have No value";
                    }
                    if (c == 1)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Region have No value";
                    }
                    if (c == 2)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationType have No value";
                    }
                    //if (c == 3)
                    //{
                    //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
                    //}
                    if (c == 4)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
                    }

                    break;

                case 3:
                    //if (c == 0) _colname = " Row: " + r + " And Column:Region have Special Char";
                    //if (c == 1) _colname = " Row: " + r + " And Column:State have Special Char";
                    //if (c == 2) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    //if (c == 3) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    //if (c == 4) _colname = " Row: " + r + " And Column:Active have Special Char";
                    if (c == 0)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Region have No value";
                    }
                    if (c == 1)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:State have No value";
                    }
                    if (c == 2)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationType have No value";
                    }
                    //if (c == 3)
                    //{
                    //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
                    //}
                    if (c == 4)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
                    }

                    break;

                case 4:
                    //if (c == 0) _colname = " Row: " + r + " And Column:Region have Special Char";
                    //if (c == 1) _colname = " Row: " + r + " And Column:State have Special Char";
                    //if (c == 2) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    //if (c == 3) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    //if (c == 4) _colname = " Row: " + r + " And Column:Active have Special Char";
                    if (c == 0)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:State have No value";
                    }
                    if (c == 1)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:District have No value";
                    }
                    if (c == 2)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationType have No value";
                    }
                    //if (c == 3)
                    //{
                    //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
                    //}
                    if (c == 4)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
                    }

                    break;

                case 5:
                    //if (c == 0) _colname = " Row: " + r + " And Column:State have Special Char";
                    //if (c == 1) _colname = " Row: " + r + " And Column:District have Special Char";
                    //if (c == 2) _colname = " Row: " + r + " And Column:City have Special Char";
                    //if (c == 3) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    //if (c == 4) _colname = " Row: " + r + " And Column:CityType have Special Char";
                    //if (c == 5) _colname = " Row: " + r + " And Column:ConveyanceType have Special Char";
                    //if (c == 6) _colname = " Row: " + r + " And Column:STD have Special Char";
                    //if (c == 7) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    //if (c == 8) _colname = " Row: " + r + " And Column:Active have Special Char";
                    if (c == 0)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:State have No value";
                    }
                    if (c == 1)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:District have No value";
                    }
                    if (c == 2)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:City have No value";
                    }
                    if (c == 3)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationType have No value";
                    }
                    if (c == 4)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:CityType have No value";
                    }
                    if (c == 5)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ConveyanceType have No value";
                    }
                    if (c == 6)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:STD have No value";
                    }
                    //if (c == 7)
                    //{
                    //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
                    //}
                    if (c == 8)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
                    }
                    break;
                case 6:
                    //if (c == 0) _colname = " Row: " + r + " And Column:District have Special Char";
                    //if (c == 1) _colname = " Row: " + r + " And Column:City have Special Char";
                    //if (c == 2) _colname = " Row: " + r + " And Column:Area have Special Char";
                    //if (c == 3) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    //if (c == 4) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    //if (c == 5) _colname = " Row: " + r + " And Column:Active have Special Char";

                    if (c == 0)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:District have No value";
                    }
                    if (c == 1)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:City have No value";
                    }
                    if (c == 2)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Area have No value";
                    }
                    if (c == 3)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationType have No value";
                    }
                    //if (c == 4)
                    //{
                    //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
                    //}
                    if (c == 5)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
                    }
                    break;
                case 7:
                    //if (c == 0) _colname = " Row: " + r + " And Column:Region have Special Char";
                    //if (c == 1) _colname = " Row: " + r + " And Column:State have Special Char";
                    //if (c == 2) _colname = " Row: " + r + " And Column:LocationType have Special Char";
                    //if (c == 3) _colname = " Row: " + r + " And Column:SyncId have Special Char";
                    //if (c == 4) _colname = " Row: " + r + " And Column:Active have Special Char";
                    if (c == 0)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Area have No value";
                    }
                    if (c == 1)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Beat have No value";
                    }
                    if (c == 3)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:LocationType have No value";
                    }
                    //if (c == 3)
                    //{
                    //    if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:SyncId have No value";
                    //}
                    if (c == 5)
                    {
                        if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:Active have No value";
                    }

                    break;
            }
            return _errorMessage;
        }
        private void InsertArea(DataTable dtrows)
        {
            int idx = Convert.ToInt32(ddlArea.SelectedValue);
            UploadData upd = new UploadData();
            ImportAreas IMPA = new ImportAreas();
            lblcols.InnerText = "";
            string Msg = "", _errorMessage = " With Incorrect Data at Row No=", _fMessage = "", _sql = "", _filter = "";
            switch (idx)
            {
                case 1:
                    #region Case Country
                    {

                        for (int i = 0; i < dtrows.Rows.Count; i++)
                        {

                            try
                            {
                                for (int c = 0; c < dtrows.Columns.Count; c++)
                                {
                                    _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                                }
                                if (!string.IsNullOrEmpty(_fMessage))
                                {
                                    _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Country','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                    _filter = "Country";
                                    _fMessage = string.Empty;
                                }
                                else
                                {
                                    string syncid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SyncId from mastarea where SyncId='" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "'"));

                                    if (syncid == "")
                                    {
                                        Msg = IMPA.InsertAreas("", dtrows.Rows[i]["Parent"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Country"].ToString().Replace("'", string.Empty), "", dtrows.Rows[i]["LocationType"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Active"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["ISD"].ToString().Replace("'", string.Empty), "", "", "", ".");
                                        _filter = "Country";
                                        if (!string.IsNullOrEmpty(Msg))
                                        {
                                            //ShowAlert(Msg); 
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'Duplicate  SyncId Exist for " + dtrows.Rows[i]["Parent"].ToString().Replace("'", string.Empty)+"-"+dtrows.Rows[i]["Country"].ToString().Replace("'", string.Empty) + " Type " + dtrows.Rows[i]["LocationType"].ToString() + "','Country','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        _filter = "Country";
                                        _fMessage = string.Empty;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //ShowAlert("Incorrect Data at Country : " + dtrows.Rows[i]["Country"].ToString()); 
                                ex.ToString();
                            }

                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Countries Imported Successfully');", true);

                        break;
                    }
                    #endregion

                case 2:
                    #region Case Region
                    {

                        for (int i = 0; i < dtrows.Rows.Count; i++)
                        {

                            try
                            {
                                for (int c = 0; c < dtrows.Columns.Count; c++)
                                {
                                    _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                                }
                                if (!string.IsNullOrEmpty(_fMessage))
                                {
                                    _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Region','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                    _filter = "Region";
                                    _fMessage = string.Empty;
                                }
                                else
                                {
                                    string syncid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SyncId from mastarea where SyncId='" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "'"));

                                    if (syncid == "")
                                    {
                                        Msg = IMPA.InsertAreas("", dtrows.Rows[i]["Country"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Region"].ToString().Replace("'", string.Empty), "", dtrows.Rows[i]["LocationType"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Active"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty), "", "", "", "", "COUNTRY");
                                        _filter = "Region";
                                        if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                                    }
                                    else
                                    {
                                        _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'Duplicate  SyncId Exist for " + dtrows.Rows[i]["Country"].ToString().Replace("'", string.Empty)+"-"+dtrows.Rows[i]["Region"].ToString().Replace("'", string.Empty) + " Type " + dtrows.Rows[i]["LocationType"].ToString() + "','Region','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        _filter = "Region";
                                        _fMessage = string.Empty;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                //ShowAlert("Incorrect Data at Region : " + dtrows.Rows[i]["Region"].ToString()); 
                                ex.ToString();
                            }

                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Regions Imported Successfully');", true);

                        break;
                    }
                    #endregion
                case 3:
                    #region Case State
                    {

                        for (int i = 0; i < dtrows.Rows.Count; i++)
                        {

                            try
                            {
                                for (int c = 0; c < dtrows.Columns.Count; c++)
                                {
                                    _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                                }
                                if (!string.IsNullOrEmpty(_fMessage))
                                {
                                    _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','State','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                    _filter = "State";
                                    _fMessage = string.Empty;
                                }
                                else
                                {
                                    string syncid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SyncId from mastarea where SyncId='" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "'"));

                                    if (syncid == "")
                                    {
                                        Msg = IMPA.InsertAreas("", dtrows.Rows[i]["Region"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["State"].ToString().Replace("'", string.Empty), "", dtrows.Rows[i]["LocationType"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Active"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty), "", "", "", "", "REGION");
                                        _filter = "State";
                                        if (!string.IsNullOrEmpty(Msg))
                                        {
                                            //ShowAlert(Msg); 
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'Duplicate  SyncId Exist for " + dtrows.Rows[i]["Region"].ToString().Replace("'", string.Empty)+"-"+dtrows.Rows[i]["State"].ToString().Replace("'", string.Empty) + " Type " + dtrows.Rows[i]["LocationType"].ToString() + "','State','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        _filter = "State";
                                        _fMessage = string.Empty;
                                    }
                                }

                            }

                            catch (Exception ex)
                            {

                                _fMessage += i + ",";
                                //DataRow dr = dtError.NewRow();
                                //dr["Row"] = i;
                                //dr["Error"] = ex.ToString();
                                //dtError.Rows.Add(dr);
                                //   ShowAlert("Incorrect Data at State : " + dtrows.Rows[i]["State"].ToString()); 
                                ex.ToString();
                            }

                        }
                        if (!string.IsNullOrEmpty(_fMessage)) _fMessage = _errorMessage + _fMessage;
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('States Imported Successfully');", true);

                        break;
                    }
                    #endregion
                case 4:
                    #region Case District
                    {

                        for (int i = 0; i < dtrows.Rows.Count; i++)
                        {

                            try
                            {
                                for (int c = 0; c < dtrows.Columns.Count; c++)
                                {
                                    _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                                }
                                if (!string.IsNullOrEmpty(_fMessage))
                                {
                                    _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','District','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                    _filter = "District";
                                    _fMessage = string.Empty;
                                }
                                else
                                {
                                    string syncid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SyncId from mastarea where SyncId='" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "'"));

                                    if (syncid == "")
                                    {
                                        Msg = IMPA.InsertAreas("", dtrows.Rows[i]["State"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["District"].ToString().Replace("'", string.Empty), "", dtrows.Rows[i]["LocationType"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Active"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty), "", "", "", "", "STATE");
                                        _filter = "District";
                                        if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                                    }
                                    else
                                    {
                                        _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'Duplicate  SyncId Exist for " + dtrows.Rows[i]["State"].ToString().Replace("'", string.Empty)+"-"+dtrows.Rows[i]["District"].ToString().Replace("'", string.Empty) + " Type " + dtrows.Rows[i]["LocationType"].ToString() + "','District','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        _filter = "District";
                                        _fMessage = string.Empty;
                                    }
                                }

                            }

                            catch (Exception ex)
                            {
                                //ShowAlert("Incorrect Data at District : " + dtrows.Rows[i]["State"].ToString());
                                ex.ToString();
                            }

                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Districts Imported Successfully');", true);

                        break;
                    }
                    #endregion
                case 5:
                    #region Case City
                    {
                        for (int i = 0; i < dtrows.Rows.Count; i++)
                        {

                            try
                            {
                                for (int c = 0; c < dtrows.Columns.Count; c++)
                                {
                                    _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                                }
                                if (!string.IsNullOrEmpty(_fMessage))
                                {
                                    _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','City','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                    _filter = "City";
                                    _fMessage = string.Empty;
                                }
                                else
                                {
                                    string syncid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SyncId from mastarea where SyncId='" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "'"));

                                    if (syncid == "")
                                    {
                                        Msg = IMPA.InsertAreas(dtrows.Rows[i]["State"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["District"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["City"].ToString().Replace("'", string.Empty), "", dtrows.Rows[i]["LocationType"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Active"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty), "", dtrows.Rows[i]["STD"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["CityType"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["ConveyanceType"].ToString().Replace("'", string.Empty), "DISTRICT");
                                        _filter = "City";
                                        if (!string.IsNullOrEmpty(Msg))
                                        {
                                            // ShowAlert(Msg); 
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'Duplicate  SyncId Exist for " + dtrows.Rows[i]["State"].ToString().Replace("'", string.Empty) + "-" + dtrows.Rows[i]["District"].ToString().Replace("'", string.Empty) + "-" + dtrows.Rows[i]["City"].ToString().Replace("'", string.Empty) + " Type " + dtrows.Rows[i]["LocationType"].ToString() + "','City','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        _filter = "City";
                                        _fMessage = string.Empty;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                _errorMessage += i + ",";
                                //ShowAlert("Incorrect Data at City : " + dtrows.Rows[i]["City"].ToString()); 
                                ex.ToString();
                            }

                        }
                        if (!string.IsNullOrEmpty(_fMessage)) _fMessage = _errorMessage + _fMessage;
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Cities Imported Successfully');", true);

                        break;
                    }
                    #endregion
                case 6:
                    #region Case Area
                    {

                        for (int i = 0; i < dtrows.Rows.Count; i++)
                        {

                            try
                            {
                                for (int c = 0; c < dtrows.Columns.Count; c++)
                                {
                                    _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                                }
                                if (!string.IsNullOrEmpty(_fMessage))
                                {
                                    _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Area','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                    _filter = "Area";
                                    _fMessage = string.Empty;
                                }
                                else
                                {
                                    string syncid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SyncId from mastarea where SyncId='" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "'"));

                                    if (syncid == "")
                                    {
                                        Msg = IMPA.InsertAreas(dtrows.Rows[i]["District"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["City"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Area"].ToString().Replace("'", string.Empty), "", dtrows.Rows[i]["LocationType"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["Active"].ToString().Replace("'", string.Empty), dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty), "", "", "", "", "CITY");
                                        _filter = "Area";
                                        if (!string.IsNullOrEmpty(Msg))
                                        {
                                            //ShowAlert(Msg); 
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'Duplicate  SyncId Exist for " + dtrows.Rows[i]["District"].ToString().Replace("'", string.Empty) + "-" + dtrows.Rows[i]["City"].ToString().Replace("'", string.Empty) + "-" + dtrows.Rows[i]["Area"].ToString().Replace("'", string.Empty) + " Type " + dtrows.Rows[i]["LocationType"].ToString() + "','Area','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        _filter = "Area";
                                        _fMessage = string.Empty;
                                    }
                                }

                            }

                            catch (Exception ex)
                            {

                                _errorMessage += i + ",";
                                // ShowAlert("Incorrect Data at Area : " + dtrows.Rows[i]["Area"].ToString()); 

                                ex.ToString();


                            }

                        }
                        if (!string.IsNullOrEmpty(_fMessage)) _fMessage = _errorMessage + _fMessage;
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Areas Imported Successfully');", true);

                        break;
                    }
                    #endregion
                case 7:
                    #region Case Beat
                    {

                        for (int i = 0; i < dtrows.Rows.Count; i++)
                        {

                            try
                            {
                                //Msg = IMPA.InsertAreas(dtrows.Rows[i]["Area"].ToString(), dtrows.Rows[i]["Beat"].ToString(), dtrows.Rows[i]["Description"].ToString(), dtrows.Rows[i]["LocationType"].ToString(), dtrows.Rows[i]["Active"].ToString(), dtrows.Rows[i]["SyncId"].ToString(), "", "", "", "","AREA");
                                for (int c = 0; c < dtrows.Columns.Count; c++)
                                {
                                    _fMessage += getMandatory(i, c, dtrows.Rows[i][c].ToString());
                                }
                                if (!string.IsNullOrEmpty(_fMessage))
                                {
                                    _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'" + _fMessage + "','Country','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                    _filter = "Beat";
                                    _fMessage = string.Empty;
                                }
                                else
                                {
                                    string syncid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SyncId from mastarea where SyncId='" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "'"));

                                    if (syncid == "")
                                    {
                                        Msg = IMPA.InsertAreas(dtrows.Rows[i]["City"].ToString().Trim().Replace("'", string.Empty), dtrows.Rows[i]["Area"].ToString().Trim().Replace("'", string.Empty), dtrows.Rows[i]["Beat"].ToString().Trim().Replace("'", string.Empty), dtrows.Rows[i]["Description"].ToString().Trim().Replace("'", string.Empty), dtrows.Rows[i]["LocationType"].ToString().Trim().Replace("'", string.Empty), dtrows.Rows[i]["Active"].ToString().Trim().Replace("'", string.Empty), dtrows.Rows[i]["SyncId"].ToString().Trim().Replace("'", string.Empty), "", "", "", "", "AREA");
                                        _filter = "Beat";
                                        if (!string.IsNullOrEmpty(Msg)) { ShowAlert(Msg); return; }
                                    }
                                    else
                                    {
                                        _sql = "Insert into Import_Excel_log([Created_At],[Error_desc],filter,syncid,form_name)values(getdate(),'Duplicate  SyncId Exist for " + dtrows.Rows[i]["City"].ToString().Trim().Replace("'", string.Empty) + "-" + dtrows.Rows[i]["Area"].ToString().Trim().Replace("'", string.Empty) + "-" + dtrows.Rows[i]["Beat"].ToString().Trim().Replace("'", string.Empty) + " Type " + dtrows.Rows[i]["LocationType"].ToString() + "','Beat','" + dtrows.Rows[i]["SyncId"].ToString().Replace("'", string.Empty) + "','UploadArea.aspx')";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, _sql);
                                        _filter = "Beat";
                                        _fMessage = string.Empty;
                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                //ShowAlert("Incorrect Data at Beat : " + dtrows.Rows[i]["Beat"].ToString()); 
                                ex.ToString();
                            }

                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Beats Imported Successfully');", true);

                        break;
                    }
                    #endregion
                default:
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Please Select a Location');", true);
                    break;
            }
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='" + _filter + "' Order by [Created_At] desc";
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


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Fupd.HasFile)
            {
                lblcols.InnerText = "";
                int idx = Convert.ToInt32(ddlArea.SelectedValue);
                if (idx == 7)
                {
                    try
                    {
                        String ext = System.IO.Path.GetExtension(Fupd.FileName);
                        if (ext.ToLower() != ".xls")
                        {
                            ShowAlert("Please Upload Only .xls File.");
                            return;
                        }
                        Data data = new Data();
                        DataTable dt = new DataTable();
                        string filename = Path.GetFileName(Fupd.FileName);
                        FileInfo file = new FileInfo(filename);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        data.UploadFile(Fupd, filename, "~/FileUploads/");
                        dt = data.ReadXLS("~/FileUploads/", filename, "Beat$", true);
                        DataTable dtcols = new DataTable();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dtcols.Columns.Add(dt.Columns[i].ToString().ToLower(), typeof(string));
                        }
                        dtcols.AcceptChanges();
                        if (GetValidCols(dtcols)) { lblcols.Visible = false; }
                        InsertArea(dt);

                        dt.Dispose();
                    }
                    catch (Exception ex)
                    {
                        ShowAlert("ERROR: " + ex.Message.ToString());
                    }
                }
                else
                {
                    try
                    {
                        String ext = System.IO.Path.GetExtension(Fupd.FileName);
                        if (ext.ToLower() != ".csv")
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Upload Only .csv File.');", true);
                            //ShowAlert("Please Upload Only .csv File.");
                            return;
                        }
                        string filename = Path.GetFileName(Fupd.FileName);
                        String csv_file_path = Path.Combine(Server.MapPath("~/FileUploads"), filename);
                        FileInfo file = new FileInfo(csv_file_path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        Fupd.SaveAs(csv_file_path);
                        GetDataTableFromCsv(csv_file_path);
                    }
                    catch (Exception ex)
                    {
                        ShowAlert("ERROR: " + ex.Message.ToString());
                    }
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You have not specified a file.');", true);
                // ShowAlert("You have not specified a file.");
            }
        }
        private Boolean GetValidCols(DataTable dtcols)
        {
            int idx = Convert.ToInt32(ddlArea.SelectedValue);
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces in between columns names";
            switch (idx)
            {
                //----Case Country ----
                case 1:
                    #region Case Country
                    {
                        if (dtcols.Columns[0].ToString().Trim() != "Parent".ToLower() || dtcols.Columns[1].ToString().Trim() != "Country".ToLower() || dtcols.Columns[2].ToString().Trim() != "LocationType".ToLower() || dtcols.Columns[3].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[4].ToString().Trim() != "Active".ToLower() || dtcols.Columns[5].ToString().Trim() != "ISD".ToLower())
                            Valid = false;
                        lblcols.InnerText += "Parent,Country,LocationType,SyncId,Active,ISD" + spaces;
                        break;
                    }
                    #endregion
                case 2:
                    #region Case Region
                    {
                        if (dtcols.Columns[0].ToString().Trim() != "Country".ToLower() || dtcols.Columns[1].ToString().Trim() != "Region".ToLower() || dtcols.Columns[2].ToString().Trim() != "LocationType".ToLower() || dtcols.Columns[3].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[4].ToString().Trim() != "Active".ToLower())
                            Valid = false;
                        lblcols.InnerText += "Country,Region,LocationType,SyncId,Active" + spaces;
                        break;
                    }
                    #endregion
                case 3:
                    #region Case State
                    {
                        if (dtcols.Columns[0].ToString().Trim() != "Region".ToLower() || dtcols.Columns[1].ToString().ToLower().Trim() != "State".ToLower() || dtcols.Columns[2].ToString().Trim() != "LocationType".ToLower() || dtcols.Columns[3].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[4].ToString().Trim() != "Active".ToLower())
                            Valid = false;
                        lblcols.InnerText += "Region,State,LocationType,SyncId,Active" + spaces;
                        break;
                    }
                    #endregion
                case 4:
                    #region Case District
                    {
                        if (dtcols.Columns[0].ToString().ToLower().Trim() != "State".ToLower() || dtcols.Columns[1].ToString().ToLower().Trim() != "District".ToLower() || dtcols.Columns[2].ToString().Trim() != "LocationType".ToLower() || dtcols.Columns[3].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[4].ToString().Trim() != "Active".ToLower())
                            Valid = false;
                        lblcols.InnerText += "State,District,LocationType,SyncId,Active" + spaces;
                        break;
                    }
                    #endregion
                case 5:
                    #region Case City
                    {
                        if (dtcols.Columns[0].ToString().ToLower().Trim() != "State".ToLower() || dtcols.Columns[1].ToString().ToLower().Trim() != "District".ToLower() || dtcols.Columns[2].ToString().ToLower().Trim() != "City".ToLower() || dtcols.Columns[3].ToString().Trim() != "LocationType".ToLower() || dtcols.Columns[4].ToString().Trim() != "CityType".ToLower() || dtcols.Columns[5].ToString().Trim() != "ConveyanceType".ToLower() || dtcols.Columns[6].ToString().Trim() != "STD".ToLower() || dtcols.Columns[7].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[8].ToString().Trim() != "Active".ToLower())
                            Valid = false;
                        lblcols.InnerText += "District,City,LocationType,CityType,ConveyanceType,STD,SyncId,Active" + spaces;
                        break;
                    }
                    #endregion

                case 6:
                    #region Case Area
                    {
                        if (dtcols.Columns[0].ToString() != "District".ToLower().Trim() || dtcols.Columns[1].ToString() != "City".ToLower().Trim() || dtcols.Columns[2].ToString().Trim() != "Area".ToLower() || dtcols.Columns[3].ToString().Trim() != "LocationType".ToLower() || dtcols.Columns[4].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[5].ToString().Trim() != "Active".ToLower())
                            Valid = false;
                        lblcols.InnerText += "City,Area,LocationType,SyncId,Active" + spaces;
                        break;
                    }
                    #endregion
                case 7:
                    #region Case Beat
                    {
                        if (dtcols.Columns[0].ToString() != "City" || dtcols.Columns[1].ToString() != "Area".ToLower().Trim() || dtcols.Columns[2].ToString().Trim() != "Beat".ToLower() || dtcols.Columns[3].ToString().Trim() != "Description".ToLower() || dtcols.Columns[4].ToString().Trim() != "LocationType".ToLower() || dtcols.Columns[5].ToString().Trim() != "SyncId".ToLower() || dtcols.Columns[6].ToString().Trim() != "Active".ToLower())
                            Valid = false;
                        lblcols.InnerText += "Area,Beat,Description,LocationType,SyncId,Active" + spaces;
                        break;
                    }
                    #endregion
                default:
                    ShowAlert("Please Select Area");
                    break;
            }
            return Valid;
        }


        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedIndex == 1)
            {
                hpldownload.NavigateUrl = "~/SampleImportSheets/Country.csv"; hpldownload.Visible = true;
            }
            else if (ddlArea.SelectedIndex == 2)
            { hpldownload.NavigateUrl = "~/SampleImportSheets/Region.csv"; hpldownload.Visible = true; }
            else if (ddlArea.SelectedIndex == 3)
            { hpldownload.NavigateUrl = "~/SampleImportSheets/State.csv"; hpldownload.Visible = true; }
            else if (ddlArea.SelectedIndex == 4)
            { hpldownload.NavigateUrl = "~/SampleImportSheets/District.csv"; hpldownload.Visible = true; }
            else if (ddlArea.SelectedIndex == 5)
            { hpldownload.NavigateUrl = "~/SampleImportSheets/City.csv"; hpldownload.Visible = true; }
            else if (ddlArea.SelectedIndex == 6)
            { hpldownload.NavigateUrl = "~/SampleImportSheets/Area.csv"; hpldownload.Visible = true; }
            else if (ddlArea.SelectedIndex == 7)
            { hpldownload.NavigateUrl = "~/SampleImportSheets/Beat.xls"; hpldownload.Visible = true; }
            else
            {
                hpldownload.NavigateUrl = ""; hpldownload.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string _sql = "";
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='" + ddlArea.SelectedItem.Text + "' Order by [Created_At] desc";
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

