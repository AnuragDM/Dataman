using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using DAL;
using BusinessLayer;
using System.Web.Services;

using System.Text;

namespace FFMS
{
    public partial class UploadPriceListNew : System.Web.UI.Page
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
                InsertPriceList(dt);
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

        public string getMandatory(int r, int c, string value)
        {
            string _errorMessage = "";
            if (c == 0)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:WefDate have No value";
            }
            if (c == 1)
            {
                if (string.IsNullOrEmpty(value)) _errorMessage = " Row: " + r + " And Column:ItemSyncId have No value";
            }

            return _errorMessage;
        }
        private void InsertPriceList(DataTable dtrows)
        {

            ImportPriceList IMPrL = new ImportPriceList();
            lblcols.InnerText = "";
            string _fMessage = "";
            DataTable dt = new DataTable();
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("Result");
            dtresult.Columns.Add("ResultType");
            dtresult.Columns.Add("Remark");
            dtresult.AcceptChanges();
            DataRow row;
            string str = "";
            for (int i = 0; i < dtrows.Rows.Count; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(dtrows.Rows[i]["ItemSyncId*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["PriceListApplicability*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["WefDate*"].ToString()) || string.IsNullOrEmpty(dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString()))
                    {
                        row = dtresult.NewRow();
                        row["Remark"] = "At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() +" Please supply values for mandatory fields - ItemSyncId*,WefDate*,PriceListApplicability*,Country_State_City_Dist_SyncId*";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtresult.Rows.Add(row);
                        continue;
                    }
                    if (dtrows.Rows[i]["ItemSyncId*"].ToString().Length > 50 || dtrows.Rows[i]["PriceListApplicability*"].ToString().Length > 7 || dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString().Length > 50)
                    {
                        row = dtresult.NewRow();
                        row["Remark"] = "At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() +" Please supply values for mandatory field as per mentioned size - ItemSyncId*,PriceListApplicability*,Country_State_City_Dist_SyncId*";
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtresult.Rows.Add(row);
                        continue;
                    }

                    bool isValid = true;
                    row = dtresult.NewRow();
                    row["Result"] = "Error";
                    row["ResultType"] = "Error";
                    row["Remark"] = "At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() +" Please supply values as Per DataType - ";

                    DateTime Dob;
                    if (!DateTime.TryParse(dtrows.Rows[i]["WefDate*"].ToString(), out Dob))
                    {
                        isValid = false;
                        row["Remark"] += " Invalid Date Format of WefDate*";
                    }
                    double Value;
                    if (!double.TryParse(dtrows.Rows[i]["MRP"].ToString(), out Value))
                    {
                        isValid = false;
                        row["Remark"] += " , Invalid MRP";
                    }

                    if (!double.TryParse(dtrows.Rows[i]["DP"].ToString(), out Value))
                    {
                        isValid = false;
                        row["Remark"] += " , Invalid DP";
                    }

                    if (!double.TryParse(dtrows.Rows[i]["RP"].ToString(), out Value))
                    {
                        isValid = false;
                        row["Remark"] += " , Invalid RP";
                    }

                    if (dtrows.Rows[i]["PriceListApplicability*"].ToString() == "")
                    {
                        isValid = false;
                        row["Remark"] += " , Invalid PriceListApplicability ,Please fill as given.";
                    }

                    if (!isValid)
                    {
                        dtresult.Rows.Add(row);
                        continue;
                    }
                    
                    if (dtrows.Rows[i]["PriceListApplicability*"].ToString().ToLower() == "country")
                    {
                        str = "select AreaId from MastArea where AreaType='COUNTRY' and SyncId='" + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() + "'";

                    }
                    else if (dtrows.Rows[i]["PriceListApplicability*"].ToString().ToLower() == "state")
                    {
                        str = "select AreaId from MastArea where AreaType='STATE' and SyncId='" + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() + "'";
                    }

                    else if (dtrows.Rows[i]["PriceListApplicability*"].ToString().ToLower() == "city")
                    {
                        str = "select AreaId from MastArea where AreaType='CITY'  and SyncId='" + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() + "'";
                    }
                    else if (dtrows.Rows[i]["PriceListApplicability*"].ToString().ToLower() == "dist")
                    {
                        str = "select PartyId as AreaId from MastParty where ISNUll(partydist,0)=1  and SyncId='" + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() + "'";
                    }
                    string Country_State_City_Dist_id = "";
                     dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if(dt.Rows.Count>0)
                    {
                        Country_State_City_Dist_id = dt.Rows[0]["AreaId"].ToString();
                    }
                    else
                    {
                        row = dtresult.NewRow();
                        row["Remark"] = "Given Country_State_City_Dist_SyncId is not found in database,At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString();
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtresult.Rows.Add(row);
                        continue;
                    }



                    decimal Mrp = 0;
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Mrp"].ToString()))
                    { Mrp = Convert.ToDecimal(dtrows.Rows[i]["Mrp"]); }
                    decimal Dp = 0;
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Dp"].ToString()))
                    { Dp = Convert.ToDecimal(dtrows.Rows[i]["Dp"]); }
                    decimal Rp = 0;
                    if (!string.IsNullOrEmpty(dtrows.Rows[i]["Rp"].ToString()))
                    { Rp = Convert.ToDecimal(dtrows.Rows[i]["Rp"]); }

                    if(Mrp<Rp || Dp>Rp)
                    {
                        row = dtresult.NewRow();
                        row["Remark"] = "MRP can't be smaller than RP and RP can't be smaller than RP - At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString();
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtresult.Rows.Add(row);
                        continue;
                    }



                 string resval=  IMPrL.InsertUpdatePriceList_V_1_0(Convert.ToDateTime(dtrows.Rows[i]["WefDate*"]), dtrows.Rows[i]["ItemSyncId*"].ToString().Replace("'", string.Empty), Mrp, Dp, Rp, dtrows.Rows[i]["PriceListApplicability*"].ToString(), Country_State_City_Dist_id);
                    if(Convert.ToInt32(resval)>0)
                    {
                        row = dtresult.NewRow();
                        row["Remark"] = "Inserted SuccessFully,At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString();
                        row["Result"] = "Success";
                        row["ResultType"] = "Insert";
                        dtresult.Rows.Add(row);
                    }
                    else if (Convert.ToInt32(resval)==-1)
                    {
                        row = dtresult.NewRow();
                        row["Remark"] = "At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString() + " already listed on  WefDate* " + dtrows.Rows[i]["WefDate*"].ToString();
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtresult.Rows.Add(row);
                    }
                    else
                    {
                        row = dtresult.NewRow();
                        row["Remark"] = "Error !! while inserting,At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString();
                        row["Result"] = "Error";
                        row["ResultType"] = "Error";
                        dtresult.Rows.Add(row);
                    }


                }
                catch (Exception ex)
                {
                    row = dtresult.NewRow();
                    row["Remark"] = "Error while inserting,At ItemPrice (row " + (i + 2) + ") - ItemName  - " + dtrows.Rows[i]["ItemName"].ToString() + dtrows.Rows[i]["ItemSyncId*"].ToString() + " - PriceListApplicability- " + dtrows.Rows[i]["PriceListApplicability*"].ToString() + " Country_State_City_DistSyncid - " + dtrows.Rows[i]["Country_State_City_Dist_SyncId*"].ToString();
                    row["Result"] = "Error";
                    row["ResultType"] = "Error";
                    dtresult.Rows.Add(row);
                }
            }

            //string _sql1 = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 00:00' and '" + (Settings.GetUTCTime()).ToString("dd/MMM/yyyy") + " 23:59' And filter='PriceList' Order by [Created_At] desc";
            // dt = DbConnectionDAL.GetDataTable(CommandType.Text, _sql1);

            if (dtresult.Rows.Count > 0)
            {
                ViewState["dtResult"] = dtresult;
                rptDatabase.DataSource = dtresult;
                rptDatabase.DataBind();
                rptmain.Style.Add("display", "block");
            }
            else
            {
                ViewState["dtResult"] = null;
                rptDatabase.DataSource = null;
                rptDatabase.DataBind();
                rptmain.Style.Add("display", "none");
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Price List Imported Successfully');", true);
            //ShowAlert("Price List Imported Successfully");

        }

        private Boolean GetValidCols(DataTable dtcols)
        {
            bool Valid = true;
            lblcols.InnerText = "Required columns format :";
            string spaces = ". * Please discard blank spaces after/before columns names";
            if (dtcols.Columns[1].ToString().Trim() != "ItemName".ToLower() || dtcols.Columns[2].ToString().Trim() != "ItemSyncId*".ToLower() || dtcols.Columns[3].ToString().Trim() != "WefDate*".ToLower() || dtcols.Columns[4].ToString().Trim() != "MRP".ToLower() || dtcols.Columns[5].ToString().Trim() != "DP".ToLower() || dtcols.Columns[6].ToString().Trim() != "RP".ToLower() || dtcols.Columns[7].ToString().Trim() != "PriceListApplicability*".ToLower() || dtcols.Columns[8].ToString().Trim() != "Country_State_City_Dist_SyncId*".ToLower())
                Valid = false;
            lblcols.InnerText += "WefDate*,ItemSyncId*,Mrp,Dp,Rp,PriceListApplicability*,Country_State_City_Dist_SyncId*" + spaces;

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
            _sql = "select [Created_At] as CreatedDate,[Error_desc] as Error from Import_Excel_log where  [Created_At] between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txtfmDate.Text) + " 23:59' And filter='PriceList' Order by [Created_At] desc";
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



        protected void btnGenrate_Click(object sender, EventArgs e)
        {
            #region Variable Declaration

            string url = "", str = "";
            DataTable dt = new DataTable();

            #endregion

            try
            {
                string Isactive = lstActive.Value;
                if (Isactive == "1") str = "select Row_number() over(order by itemname ) as Sno, itemname,syncid from mastitem where isnull(active,0)=1 and itemtype='ITEM' order by itemname";
                else if (Isactive == "0") str = "select Row_number() over(order by itemname ) as Sno,itemname,syncid from mastitem where isnull(active,0)=0 and itemtype='ITEM' order by itemname";
                else str = "select Row_number() over(order by itemname ) as Sno,itemname,syncid from mastitem where  itemtype='ITEM' order by itemname ";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment;filename=ItemPriceList.csv");
                    StringBuilder sb = new StringBuilder();
                    string headertext = "SNo,ItemName,ItemSyncId*,WefDate*,MRP,DP,RP,PriceListApplicability*,Country_State_City_Dist_SyncId*";
                    sb.Append(headertext);
                    sb.Append(System.Environment.NewLine);
                    headertext = ",varchar(100),varchar(50),Datetime(dd/mm/yyyy),Numeric(18_2),Numeric(18_2),Numeric(18_2),Country/State/City/Dist,varchar(50)";
                    sb.Append(headertext);
                    sb.Append(System.Environment.NewLine);

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            sb.Append(String.Format("\"{0}\"", dt.Rows[j][k].ToString().Replace(",", "-")) + ',');
                        }
                        sb.Append(Environment.NewLine);
                    }
                    // Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment;filename=ItemPriceList.csv");
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();

                }

            }
            catch (Exception ex)
            {
                url = ex.ToString();
            }
        }

        protected void btnDwnldPriceList_Click(object sender, EventArgs e)
        {
            #region Variable Declaration

            string url = "", str = "";
            DataTable dt = new DataTable();

            #endregion

            try
            {
                str = "SELECT i.ItemName,i.SyncId, format(P.WEFDATE,'dd/MM/yyyy')  WEFDATE,P.MRP,P.DP,P.RP, isnull(P.PriceListApplicability,'') PriceListApplicability, case when ISNULL(p.PriceListApplicability,'')='Country' then mCNT.AreaName when ISNULL(p.PriceListApplicability,'')='State' then mCNT.SyncId when ISNULL(p.PriceListApplicability,'')='City' then mCITY.SyncId when ISNULL(p.PriceListApplicability,'')='Dist' then mDIST.SyncId else '' end Country_State_City_Dist_SyncId   FROM PRICELIST P LEFT JOIN MASTITEM I ON I.ITEMID=P.ITEMID left join MastArea mCNT on mCNT.AreaId=p.Country_State_City_Dist_id  left join MastArea mST on mST.AreaId=p.Country_State_City_Dist_id  left join MastArea mCITY on mCITY.AreaId=p.Country_State_City_Dist_id  left join MastParty mDIST on mDIST.PartyId=p.Country_State_City_Dist_id ORDER BY WEFDATE DESC,i.ItemName";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment;filename=UploadedItemPriceList.csv");
                    StringBuilder sb = new StringBuilder();
                    string headertext = "SNo,ItemName,ItemSyncId*,WefDate*,MRP,DP,RP,PriceListApplicability*,Country_State_City_Dist_SyncId*";
                    sb.Append(headertext);
                    sb.Append(System.Environment.NewLine);
                    headertext = ",varchar(100),varchar(50),Datetime(dd/mm/yyyy),Numeric(18_2),Numeric(18_2),Numeric(18_2),Country/State/City/Dist,varchar(50)";
                    sb.Append(headertext);
                    sb.Append(System.Environment.NewLine);

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if(k==0)
                            {
                                sb.Append(String.Format("\"{0}\"", j+1) + ',');
                                sb.Append(String.Format("\"{0}\"", dt.Rows[j][k].ToString().Replace(",", "-")) + ',');
                            }
                            else sb.Append(String.Format("\"{0}\"", dt.Rows[j][k].ToString().Replace(",", "-")) + ',');
                        }
                        sb.Append(Environment.NewLine);
                    }
                    // Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment;filename=UploadedItemPriceList.csv");
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();

                }

            }
            catch (Exception ex)
            {
                url = ex.ToString();
            }
        }

        private void GenerateExcel(DataTable dtresult)
        {
            try
            {
                #region ExcelExportRetailerImportInfo
               
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=ItemPriceListImportDetailReport.csv");
                string headertext = "SNo,Result,ResultType,Remark";

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.Append(System.Environment.NewLine);

                for (int j = 0; j < dtresult.Rows.Count; j++)
                {
                    for (int k = 0; k < dtresult.Columns.Count; k++)
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", j + 1) + ',');
                            sb.Append(String.Format("\"{0}\"", dtresult.Rows[j][k].ToString().Replace(",", "-")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtresult.Rows[j][k].ToString().Replace(",", "-")) + ',');
                        }


                    }
                    sb.Append(Environment.NewLine);
                }

                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=ItemPriceListImportDetailReport.csv");
                Response.Write(sb.ToString());
                Response.Flush();               
                Response.End();
                #endregion
            }
            catch (System.Threading.ThreadAbortException)
            {
                Page_Load(null, null);
            }
            catch (Exception)
            {

                throw;
            }

        }
       

        protected void btnDownload_Click1(object sender, EventArgs e)
        {
            try
            {
                DataTable dtresult = (DataTable)ViewState["dtResult"];
                GenerateExcel(dtresult);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}