using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using DAL;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
//using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
//using Microsoft.WindowsAPICodePack.Shell;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
namespace FFMS
{
    public partial class ImportMeetUsers : System.Web.UI.Page
    {
        BAL.Meet.AddMeetUsersBAL dp = new BAL.Meet.AddMeetUsersBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if(!IsPostBack)
            {
                fillUnderUsers();
                fillMeetType();
                this.fillPartyType();
               // lblm.InnerHtml = "NameLable";
               // fillMeet();
                ddlMeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }


        private void fillMeetType()
        {
            string query = "select * from MastMeetType order by Name";
            System.Data.DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillPartyType()
        {
            string query = "select PartyTypeId,PartyTypeName from PartyType order by PartyTypeName";
            System.Data.DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlPartyType.DataSource = dt;
                ddlPartyType.DataTextField = "PartyTypeName";
                ddlPartyType.DataValueField = "PartyTypeId";
                ddlPartyType.DataBind();
            }
         
        }
        private void BindDDlCity(int MeetPlanId)
        {//Ankita - 10/may/2016- (For Optimization)
            ddlCity.Items.Clear();
          
            string str = @"select MeetLoaction,ma.AreaName,case WHEN  Beat.AreaName is null then '-' else Beat.AreaName End  as Beat,tp.AreaId,* from [dbo].[TransMeetPlanEntry] tp left join MastArea ma on tp.MeetLoaction=ma.AreaId left join MastArea beat on tp.AreaId=beat.AreaId where MeetPlanId ="+MeetPlanId+"";

            System.Data.DataTable obj = new System.Data.DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                try
                {
                    ddlCity.DataSource = obj;
                    ddlCity.DataTextField = "AreaName";
                    ddlCity.DataValueField = "MeetLoaction";
                    ddlCity.DataBind();
                }
                catch { }

              

            }
           // ddlCity.Items.Add(new ListItem("OTHER CITY", "-1"));
            //ddlcity.Items.Insert(0, new ListItem("-- Select --", "0"));
            //ddlnextcity.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private void BindBeat(int MeetPlanId)
        {//Ankita - 10/may/2016- (For Optimization)
            ddlBeat.Items.Clear();

            string str = @"select MeetLoaction,ma.AreaName,case WHEN  Beat.AreaName is null then '-' else Beat.AreaName End  as Beat,tp.AreaId,* from [dbo].[TransMeetPlanEntry] tp left join MastArea ma on tp.MeetLoaction=ma.AreaId left join MastArea beat on tp.AreaId=beat.AreaId where MeetPlanId =" + MeetPlanId + "";

            System.Data.DataTable obj = new System.Data.DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                try
                {
                    ddlBeat.DataSource = obj;
                    ddlBeat.DataTextField = "Beat";
                    ddlBeat.DataValueField = "AreaId";
                    ddlBeat.DataBind();
                }
                catch { }



            }
            // ddlCity.Items.Add(new ListItem("OTHER CITY", "-1"));
            ddlBeat.Items.Insert(0, new ListItem("-- Select --", "0"));
            //ddlnextcity.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        //private void fillUnderUsers()
        //{
        //    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
        //    if (d.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            DataView dv = new DataView(d);
        //            dv.RowFilter = "RoleType='AreaIncharge'";
        //            ddlunderUser.DataSource = dv;
        //            ddlunderUser.DataTextField = "SMName";
        //            ddlunderUser.DataValueField = "SMId";
        //            ddlunderUser.DataBind();
        //        }
        //        catch { }

        //    }
        //}
        private void fillUnderUsers()
        {
            System.Data.DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                    System.Data.DataTable dtRole = new System.Data.DataTable();
                    dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                    string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                    if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                    {
                        ddlunderUser.SelectedValue = Settings.Instance.SMID;
                    }
                }
                catch { }

            }
        }
        private void fillMeet()
        {
            ddlMeet.Items.Clear();
            //string s = "select * from [TransMeetPlanEntry] where SMID=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and  [AppBy] is Null and   MeetTypeId=" + ddlmeetType.SelectedValue+" order by MeetPlanId desc";
            //Nishu 14/05/2015
            //string s = "select * from [TransMeetPlanEntry] where SMID=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId=" + ddlmeetType.SelectedValue + " order by MeetPlanId desc";
            string s = "select mp.MeetPlanId,mp.MeetName from [TransMeetPlanEntry] mp LEFT JOIN TransMeetExpense me  ON mp.MeetPlanId = me.MeetPlanId where mp.AppStatus IN ('Pending','Approved') AND mp.SMId=" + ddlunderUser.SelectedValue + " and mp.MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " AND me.FinalApprovedAmount IS null order by mp.MeetPlanId desc";

            System.Data.DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if (dt.Rows.Count > 0)
            {
                ddlMeet.DataSource = dt;
                ddlMeet.DataTextField = "MeetName";
                ddlMeet.DataValueField = "MeetPlanId";
                ddlMeet.DataBind();
            }
            ddlMeet.Items.Insert(0, new ListItem("-- Select --", "0"));
           
        }

        private string  GetAreaID(string AName)
        {
             string AID = "";
             try
             {
                 string s = @"select AreaId from MastArea where AreaName='" + AName + "'";
                  AID = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
             }
             catch
             {
             }
             return AID;
        }

        private string  PartyTypeID(string AName)
        {
            string AID = "";
            try
            {
                string s = @"select PartyTypeId from PartyType where PartyTypeName='" + AName + "'";
                 AID = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
            }
            catch { 
            }
            return AID;
        }

        public System.Data.DataTable RemoveDuplicateRows(System.Data.DataTable table, string DistinctColumn)
        {
            try
            {
                ArrayList UniqueRecords = new ArrayList();
                ArrayList DuplicateRecords = new ArrayList();

                // Check if records is already added to UniqueRecords otherwise,
                // Add the records to DuplicateRecords
                foreach (DataRow dRow in table.Rows)
                {
                    if (DistinctColumn == "MobileNo*")
                    {
                        if (Convert.ToString(dRow[DistinctColumn]) != "9999999999")
                        {
                            if (UniqueRecords.Contains(dRow[DistinctColumn]))
                                DuplicateRecords.Add(dRow);
                            else
                                UniqueRecords.Add(dRow[DistinctColumn]);
                        }
                    }
                    else
                    {
                        if (UniqueRecords.Contains(dRow[DistinctColumn]))
                            DuplicateRecords.Add(dRow);
                        else
                            UniqueRecords.Add(dRow[DistinctColumn]);
                    }
                    
                }

                // Remove dupliate rows from DataTable added to DuplicateRecords
                foreach (DataRow dRow in DuplicateRecords)
                {
                    table.Rows.Remove(dRow);
                }

                // Return the clean DataTable which contains unique records.
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void Uploadusers()
        {
            try
            {
                
                uploaddata();
           
           }
            catch
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Records cannot be  Insert Please check the Excel sheet format');", true);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                //insertTable();
               Uploadusers();
                //ddlBeat.SelectedValue = "0";
                //ddlCity.SelectedValue = "0";
                //ddlmeetType.SelectedValue = "0";
                //ddlMeet.SelectedValue = "0";
                //ddlunderUser.SelectedValue = "0";

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please choose the Excel File');", true);
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ImportMeetUsers.aspx");
        }

        protected void lnkdownload_Click(object sender, EventArgs e)
        {
            
            string PTypeStr1 = "";
        //     Response.ClearContent();
        //Response.Buffer = true;
        ////Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xls"));
        //Response.ContentType = "application/ms-excel";
        //string str1 = "select AreaId,MeetLoaction from  TransMeetPlanEntry where MeetPlanId in (" + ddlMeet.SelectedValue + ")";
        //        System.Data.DataTable getId = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
        //        if (getId.Rows[0]["AreaId"] != "0") str1 = "select ma.AreaName as BeatName,mp.ContactPerson,mp.Address1+ mp.Address2 as [Address],mp.Mobile,mp.Email,mp.PartyName,isnull(mp.DOB,0),mp.Potential from MastParty mp Left Join MastArea ma on mp.BeatId=ma.AreaId Where BeatId=" + Convert.ToInt32(getId.Rows[0]["AreaId"]) + "";
        //        else str1 = "select ma.AreaName as BeatName,mp.PartyName,mp.ContactPerson,mp.Address1+ mp.Address2 as [Address],mp.Mobile,mp.Email,mp.PartyName,isnull(mp.DOB,0),mp.Potential from MastParty mp Left Join MastArea ma on mp.BeatId=ma.AreaId Where CityId=" + Convert.ToInt32(getId.Rows[0]["CityId"]) + "";

        //     DataTable   dt = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
    
        //string str = string.Empty;
        //foreach (DataColumn dtcol in dt.Columns)
        //{
        //    Response.Write(str + dtcol.ColumnName);
        //    str = "\t";
        //}
        //Response.Write("\n");
        //foreach (DataRow dr in dt.Rows)
        //{
        //    str = "";
        //    for (int j = 0; j < dt.Columns.Count; j++)
        //    {
        //        Response.Write(str + Convert.ToString(dr[j]));
        //        str = "\t";
        //    }
        //    Response.Write("\n");
        //}
        //Response.End();
           // getSheet();
            string searchCriteria = "";

            foreach (ListItem item in ddlPartyType.Items)
            {

                if (item.Selected)
                {
                    PTypeStr1 += item.Value + ",";
                   // PTypeStr1 += item.Text + ",";
                }
            }

            PTypeStr1 = PTypeStr1.TrimStart(',').TrimEnd(',');
            string str1 = "select AreaId,MeetLoaction from  TransMeetPlanEntry where MeetPlanId in (" + ddlMeet.SelectedValue + ")";
            System.Data.DataTable getId = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            //if (getId.Rows[0]["AreaId"] != "0")
            //{
            //    if (ddlPartyType.SelectedValue != "0") str1 = "select ma.AreaName,mb.AreaName as BeatName,mp.ContactPerson,mp.Address1+ mp.Address2 as [Address],mp.Mobile,mp.Email,mp.PartyName,mp.Potential,case  WHEN mp.DOB IS NULL THEN '01/01/1800' else mp.DOB end as DOB,0 as PartyType from MastParty mp Left Join MastArea mb on mp.BeatId=mb.AreaId Left Join MastArea ma on mp.AreaId=ma.AreaId Where BeatId=" + Convert.ToInt32(getId.Rows[0]["AreaId"]) + " And PartyType=" + Settings.DMInt32(ddlPartyType.SelectedValue) + "";
            //    else str1 = "select ma.AreaName,mb.AreaName as BeatName,mp.ContactPerson,mp.Address1+ mp.Address2 as [Address],mp.Mobile,mp.Email,mp.PartyName,mp.Potential,case  WHEN mp.DOB IS NULL THEN '01/01/1800' else mp.DOB end as DOB,0 as PartyType from MastParty mp Left Join MastArea mb on mp.BeatId=mb.AreaId Left Join MastArea ma on mp.AreaId=ma.AreaId Where BeatId=" + Convert.ToInt32(getId.Rows[0]["AreaId"]) + ""; 
            //}
            //else
            //{ 
            //    if (ddlPartyType.SelectedValue != "0") str1 = "select ma.AreaName,mb.AreaName as BeatName,mp.ContactPerson,mp.Address1+ mp.Address2 as [Address],mp.Mobile,mp.Email,mp.PartyName,mp.Potential,case  WHEN mp.DOB IS NULL THEN '01/01/1800' else mp.DOB end as DOB,0 as PartyType from MastParty mp Left Join MastArea mb on mp.BeatId=mb.AreaId Left Join MastArea ma on mp.AreaId=ma.AreaId Where CityId=" + Convert.ToInt32(getId.Rows[0]["CityId"]) + " And PartyType=" + Settings.DMInt32(ddlPartyType.SelectedValue) + "";
            //    else str1 = "select ma.AreaName,mb.AreaName as BeatName,mp.ContactPerson,mp.Address1+ mp.Address2 as [Address],mp.Mobile,mp.Email,mp.PartyName,mp.Potential,case  WHEN mp.DOB IS NULL THEN '01/01/1800' else mp.DOB end as DOB,0 as PartyType from MastParty mp Left Join MastArea mb on mp.BeatId=mb.AreaId Left Join MastArea ma on mp.AreaId=ma.AreaId Where CityId=" + Convert.ToInt32(getId.Rows[0]["CityId"]) + "";
            //}

            if (ddlPartyType.SelectedValue != "") searchCriteria += " And mp.PartyType IN(" + PTypeStr1+")";
            if (ddlBeat.SelectedValue != "0") searchCriteria += " And mp.BeatId=" + Settings.DMInt32(ddlBeat.SelectedValue);
            str1 = "select ma.AreaName,mb.AreaName as BeatName,mp.ContactPerson as [ContactPerson*],mp.Address1+ mp.Address2 as [Address],mp.Mobile as [Mobile*],mp.Email,mp.PartyName,mp.Potential,case  WHEN mp.DOB IS NULL THEN '-' else replace(convert(VARCHAR,cast((mp.DOB) as date), 106), ' ', '/')  end as DOB from MastParty mp Left Join MastArea mb on mp.BeatId=mb.AreaId Left Join MastArea ma on mp.AreaId=ma.AreaId Where CityId=" + Convert.ToInt32(getId.Rows[0]["MeetLoaction"]) + " And mp.PartyDist=0" + searchCriteria + "";
            //"select ma.AreaName,mb.AreaName as BeatName,mp.ContactPerson,mp.Address1+ mp.Address2 as [Address],mp.Mobile as [Mobile*],mp.Email,mp.PartyName,mp.Potential,case  WHEN mp.DOB IS NULL THEN '-' else replace(convert(VARCHAR,cast((mp.DOB) as date), 106), ' ', '/')  end as DOB from MastParty mp Left Join MastArea mb on mp.BeatId=mb.AreaId Left Join MastArea ma on mp.AreaId=ma.AreaId Where CityId=" + Convert.ToInt32(getId.Rows[0]["MeetLoaction"]) + "" + searchCriteria + "";

            System.Data.DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            DataRow newRow = dt.NewRow();
  
            newRow[0] = "Varchar(100)";
            newRow[1] = "Varchar(100)";
            newRow[2] = "Varchar(100)";
            newRow[3] = "Varchar(150)";
            newRow[4] = "Varchar(35)";
            newRow[5] = "Varchar(50)";
            newRow[6] = "Varchar(150)";
            newRow[7] = 18;
            newRow[8] = "DateTime";
          
            dt.Rows.InsertAt(newRow, 0);

            
            DataRow newRow1 = dt.NewRow();
            newRow1[0] = "Example : AHMEDABAD";
            newRow1[1] = "Example : AHMEDABAD";
            newRow1[2] = "Example : YOGESH";
            newRow1[3] = "Example : 201, 2ND FLOOR, PUSHPAM COMPLEX";
            newRow1[4] = "Example : 9999999999";
            newRow1[5] = "Example : a@gmail.com";
            newRow1[6] = "Example : ACE";
            newRow1[7] = 18;
            newRow1[8] = "Example : 24/10/2010";
            dt.Rows.InsertAt(newRow1, 1);
            DataSet ds=new DataSet();
            ds.Tables.Add(dt);
            //GetSheetName(ds);
             fillExcel(ds);
            //ExportToExcel(dt, "Sheet1");
           // this.UploadDataTableToExcel(dt);
           
           // createExcel(dt, "Sheet1");
        }
        public void ExportToExcel(System.Data.DataTable dt, string filename)
        {
            string excelPath = Server.MapPath("~/Files/");
            string pathFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
           // StreamWriter wr = new StreamWriter(excelPath + filename + ".xls");
            StreamWriter wr = new StreamWriter(pathFile+filename + ".xls");
            try
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }

                wr.WriteLine();

                //write rows to excel file
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }
                    //go to next line
                    wr.WriteLine();
                }
                //close file
                wr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static class KnownFolder
        {
            public static readonly Guid Downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);
        public void createExcel(System.Data.DataTable dt, string filename)
        {
            string downloads;
            string strFilePath =Server.MapPath("~\\Files\\Sheet1.xls");
            SHGetKnownFolderPath(KnownFolder.Downloads, 0, IntPtr.Zero, out downloads);
               //string excelPath = "C:\\Users\\Vikram\\Downloads";
            string fileName = "Sheet1.xls";
            string sourcePath = strFilePath;
            string targetPath = strFilePath;
           // string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string sourceFile = System.IO.Path.Combine(strFilePath);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            //if (System.IO.File.Exists(sourceFile))
            //{
            
            //    try
            //    {
            //        System.IO.File.Delete(sourceFile);
            //    }
            //    catch (System.IO.IOException e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return;
            //    }
            //}

            //if (!System.IO.Directory.Exists(sourceFile))
            //{
            //    System.IO.File.Copy(sourceFile, destFile, true);
            //}

       
            string attachment = "attachment; filename=Sheet1.xls";
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(strFilePath));
            Response.TransmitFile(Server.MapPath("~/Files/Sheet1.xls"));
            Response.ContentType = "Application/x-msexcel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeetType.SelectedIndex > 0)
            {
                ddlMeet.Enabled = true;
                fillMeet();
              
            }
            else
            {
                ddlMeet.Items.Clear();
                ddlMeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }
        public void getSheet()
        {
            FileStream sourceFile = null;
            string fileName = "~/Files/MeetUserList.xls";
            try
            {
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName(fileName));
                sourceFile = new FileStream(fileName, FileMode.Open);
                long FileSize; FileSize = sourceFile.Length;
                byte[] getContent = new byte[(int)FileSize];
                sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                sourceFile.Close();
                Response.BinaryWrite(getContent);
               // this.UploadDataTableToExcel(dt);
            }
            catch (Exception ex) { throw ex; }
        }
        protected void UploadDataTableToExcel(System.Data.DataTable dtRecords)
        {
            string XlsPath = Server.MapPath(@"~/Files/MeetUser.xls");
            string attachment = string.Empty;
            string strColumn = "SET FMTONLY ON; select CASE WHEN b.TABLE_NAME is not null then 'view' else 'table' end OBJECT_TYPE,COLUMN_NAME,DATA_TYPE as datatype,NUMERIC_PRECISION as dFirstIndex,NUMERIC_SCALE as dlIndex,CHARACTER_OCTET_LENGTH AS cSize from INFORMATION_SCHEMA.COLUMNS a LEFT OUTER JOIN INFORMATION_SCHEMA.VIEWS b ON a.TABLE_CATALOG = b.TABLE_CATALOG AND a.TABLE_SCHEMA = b.TABLE_SCHEMA AND a.TABLE_NAME = b.TABLE_NAME where b.TABLE_NAME='View_ImportUser'; SET FMTONLY OFF";
            SqlCommand cmd = new System.Data.SqlClient.SqlCommand();

          System.Data.DataTable dtColumn = DbConnectionDAL.GetDataTable(CommandType.Text, strColumn);
            if (XlsPath.IndexOf("\\") != -1)
            {
                string[] strFileName = XlsPath.Split(new char[] { '\\' });
                attachment = "attachment; filename=" + strFileName[strFileName.Length - 1];
            }
            else
                attachment = "attachment; filename=" + XlsPath;
            try
            {
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Sheet1.xls");
                string SheetName = "Sheet1";
                //ExcelWorksheet ws = pack.Workbook.Worksheets["" + File + ""];
                //ExcelWorksheet ws = pack.Workbook.Worksheets.Add("" + SheetName);
               
                string tab = string.Empty;

                foreach (DataColumn datacol in dtRecords.Columns)
                {
                    Response.Write(tab + datacol.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");

                //foreach (DataColumn datacol in dtRecords.Columns)
                //{
                //    tab = "";
                //    DataRow[] dr = dtColumn.Select("COLUMN_NAME = " + datacol.ColumnName);

                //    if (dr[0]["cSize"].ToString() != null) Response.Write(tab + "" + dr[0]["datatype"].ToString() + "(" + "" + dr[0]["cSize"].ToString() + ")");
                //    else Response.Write(tab + "" + dr[0]["datatype"].ToString() + "(" + "" + dr[0]["dFirstIndex"].ToString() + "," + dr[0]["dlIndex"].ToString() + ")");
                //    tab = "\t";
                //}
                //Response.Write("\n");
                tab = "";
                foreach (DataColumn datacol in dtRecords.Columns)
                {
                    //tab = "";
                    if (datacol.ColumnName == "AreaName") Response.Write(tab + "Varchar(100)");
                    if (datacol.ColumnName == "BeatName") Response.Write(tab + "Varchar(100)");
                    if (datacol.ColumnName == "ContactPerson") Response.Write(tab + "Varchar(100)");
                    if (datacol.ColumnName == "Address") Response.Write(tab + "Varchar(150)");
                    if (datacol.ColumnName == "Mobile*") Response.Write(tab + "Varchar(35)");
                    if (datacol.ColumnName == "Email") Response.Write(tab + "Varchar(50)");
                    if (datacol.ColumnName == "PartyName") Response.Write(tab + "Varchar(150)");
                    if (datacol.ColumnName == "Potential") Response.Write(tab + "decimal(18,2)");
                    if (datacol.ColumnName == "DOB") Response.Write(tab + "DateTime"); 
                  //  Response.Write(tab + datacol.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                tab = "";
                foreach (DataColumn datacol in dtRecords.Columns)
                {
                     if (datacol.ColumnName == "AreaName") Response.Write(tab + "Example : AHMEDABAD");
                    if (datacol.ColumnName == "BeatName") Response.Write(tab + "Example : AHMEDABAD");
                    if (datacol.ColumnName == "ContactPerson") Response.Write(tab + "Example : YOGESH");
                    if (datacol.ColumnName == "Address") Response.Write(tab + "Example : 201, 2ND FLOOR, PUSHPAM COMPLEX");
                    if (datacol.ColumnName == "Mobile*") Response.Write(tab + "Example : 9999999999");
                    if (datacol.ColumnName == "Email") Response.Write(tab + "Example : a@gmail.com");
                    if (datacol.ColumnName == "PartyName") Response.Write(tab + "Example : ACE");
                    if (datacol.ColumnName == "Potential") Response.Write(tab + "Example : 23.10");
                    if (datacol.ColumnName == "DOB") Response.Write(tab + "Example : 24/10/2010");
                    //  Response.Write(tab + datacol.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                foreach (DataRow dr in dtRecords.Rows)
                {
                    tab = "";
                    for (int j = 0; j < dtRecords.Columns.Count; j++)
                    {
                        Response.Write(tab + Convert.ToString(dr[j]));
                        tab = "\t";
                    }

                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
        }

        public void GetSheetName(DataSet ds)
        {
            string fileName = Server.MapPath(@"~/Files/MeetUser.xls"); 
           // Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(Server.MapPath("UploadedExcel/" + lblTestCode.Text.Trim() + ".xls"));
            var application = new Application();
            var workbook = application.Workbooks.Open(fileName);
         
         //   FieldInfo myf = typeof(TEST).GetField("abcd");
            foreach (System.Data.DataTable table in ds.Tables)
            {
                //Add a new worksheet to workbook with the Datatable name
                var worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                worksheet.Name = "Sheet1";

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = table.Columns[i - 1].ColumnName;




                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        worksheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();



                    }
                }
            }

            workbook.Save();
            workbook.Close();
            Response.Redirect("~/Files/MeetUser.xls");
           // excelApp.Quit();
        }
        public void uploaddata()
         {
            //Vikram
             string strSql = @"select MeetDate from TransMeetPlanEntry where MeetPlanId=" + ddlMeet.SelectedValue + "";
             DateTime MeetDate = Convert.ToDateTime(DbConnectionDAL.GetScalarValue(CommandType.Text, strSql));
             //  DateTime startdate = DateTime.ParseExact(MeetDate.Trim(), "dd/MM/yyyy", provider);
             if (MeetDate > DateTime.Today)
             {
                 System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Meet Date Should be less than or Equal to Current Date');", true);
                 return;
             }
             else
             {
                 DataSet objds = new DataSet();
                 System.Data.DataTable dt = null;
                 string err = ""; string duplicate = "";
                 //  string excelPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                 string excelPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                 //Q1`1  FileUpload1.SaveAs(excelPath);
                 //  dt.Columns.Add("MobileNo", typeof(string));    
                 string ConnStr = "";
                 Data data = new Data();
                 System.Data.DataTable dtExcelData = new System.Data.DataTable();
                 string filename = Path.GetFileName(FileUpload1.FileName);
                 //string filename = "MeetUser.xls";
                 string name = FileUpload1.PostedFile.FileName;
                 string IH = Path.GetFullPath(FileUpload1.PostedFile.FileName);
                 string FolderToSearch = System.IO.Directory.GetParent(FileUpload1.PostedFile.FileName).ToString();
                 string FileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                 if (System.IO.File.Exists(Server.MapPath("\\Files") + "\\" + FileUpload1.FileName))
                 {
                     try
                     {
                         string filePath = "Files/" + FileUpload1.FileName;

                         System.IO.File.Delete(Server.MapPath(filePath));
                        

                         //System.IO.File.Delete(Server.MapPath("\\Files") + "\\" + FileUpload1.FileName);
                     }
                     catch (System.IO.IOException e)
                     {
                         Console.WriteLine(e.Message);
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File is open');window.location ='ImportMeetUsers.aspx';", true);
                         System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(" + e.Message + ");", true);
                         return;
                     }
                 }
                 FileUpload1.SaveAs(Server.MapPath("\\Files") + "\\" + FileUpload1.FileName);
                 //data.UploadFile(FileUpload1, filename, "~/Files/");
                 if (FileExtension == ".xlsx")
                 {
                      ConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + HttpContext.Current.Server.MapPath("\\Files" + "\\" + filename) + ";Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";";
                      //ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("\\Files" + "\\" + filename) + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";";
                 }
                 else
                 {
                     //ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("\\Files" + "\\" + filename) + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";";
                     ConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + HttpContext.Current.Server.MapPath("\\Files" + "\\" + filename) + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\";";
                     //ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + (FolderToSearch + "\\" + filename) + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";";
                 }
                 //ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("\\Files" + "\\" + filename) + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";";
                 // ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "Downloads" + "\\" + filename + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";";

                 try
                 {
                   

                     //Application app = new Application();
                     ////Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                     //string fName = Server.MapPath("\\Files") + "\\" + FileUpload1.FileName;
                     //Microsoft.Office.Interop.Excel.Workbook excelWorkbook = app.Workbooks.Open(fName);
                     //Microsoft.Office.Interop.Excel._Worksheet sheet = excelWorkbook.Sheets[1];
                     //var LastRow = sheet.UsedRange.Rows.Count;
                     //LastRow = LastRow + sheet.UsedRange.Row - 1;
                     //for (int i = 1; i <= LastRow; i++)
                     //{
                     //    if (i == 60) 
                     //    { }
                     //    if (app.WorksheetFunction.CountA(sheet.Rows[i]) == 0)
                     //        (sheet.Rows[i] as Microsoft.Office.Interop.Excel.Range).Delete();
                     //}

                     OleDbConnection connection = new OleDbConnection();
                     OleDbDataAdapter adapter = new OleDbDataAdapter();
                     System.Data.DataTable dtNew = new System.Data.DataTable();
                     connection.ConnectionString = ConnStr;
                     string strSQL = "SELECT * FROM [Sheet1$]";
                     if (connection.State != ConnectionState.Open)
                         connection.Open();
                     OleDbCommand selectCommand = new OleDbCommand(strSQL, connection);
                     OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                     OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                     //da.Fill(objds);
                     da.Fill(objds);
                     da.Fill(dtNew);


                     object value = objds.Tables[0].Rows[0][0];

                     var duplicates = objds.Tables[0].AsEnumerable()
                            .GroupBy(r => r[4])//Using Column Index
                            .Where(gr => gr.Count() > 1)
                            .Select(g => g.Key);


                     foreach (var d in duplicates)
                     {
                         if (d.ToString() != "9999999999") if (d.ToString() != "") duplicate += d.ToString() + ",";


                     }

                     if (duplicate != "")
                     {
                         System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('These mobile are Duplicate" + duplicate + "');", true);
                         return;
                     }
                     objds.Tables[0].Rows.RemoveAt(0);


                     objds.AcceptChanges();
                     string nameslist = string.Empty;

                     foreach (DataRow dr in objds.Tables[0].Rows)
                     {
                         Object[] cells = dr.ItemArray;
                         string AreaName = cells[0].ToString();
                         string mo2b = cells[4].ToString();
                         string Contect2 = cells[2].ToString();
                         if (AreaName == "Varchar(100)")
                         {
                             dr.Delete();
                             continue;
                         }
                         if (AreaName == "Example : AHMEDABAD")
                         {
                             dr.Delete();
                             continue;
                         }
                         if ((cells[0].ToString() == "") && (cells[1].ToString() == "") && (cells[2].ToString() == "") && (cells[4].ToString() == "") && (cells[8].ToString() == ""))
                         {
                             dr.Delete();
                             continue;
                         }

                         nameslist += "'" + cells[4].ToString() + "'" + ",";
                     }
                     int indexOfFirst = nameslist.IndexOf('\'');
                     string temp = nameslist.Remove(indexOfFirst, 1);
                     int indexOfLast = temp.LastIndexOf('\'');
                     temp = temp.Remove(indexOfLast, 1);
                     int count = 0;
                     //int countindex = 1;
                     foreach (DataRow myrow in objds.Tables[0].Rows)
                     {
                         
                         if (myrow.RowState != DataRowState.Deleted)
                         {
                            
                             Object[] cells = myrow.ItemArray;
                             string mob = cells[4].ToString();
                             decimal myDec;
                             var Result = decimal.TryParse(mob, out myDec);
                             string Contect = cells[2].ToString();
                             string areaName = cells[0].ToString();
                                if (Contect != "")
                             
                             {
                            // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Insert Area Name" + err + "');", true);
                                 if (mob == "")
                                 {
                                     System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Enter 10 digits Mobile No at Row " + (count + 2) + "');", true);
                                     connection.Close();
                                     return;
                                 }
                                 if (mob.ToString().Length < 10)
                                 {
                                     System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('mobile length should be 10 digits at Row " + (count + 2) + "');", true);
                                     connection.Close();
                                     return;
                                 }
                                 if (mob.ToString().Length > 10)
                                 {
                                     System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('mobile length should be 10 digits at Row " + (count + 2) + "');", true);
                                     connection.Close();
                                     return;
                                 }
                                 if(Result == false)
                                 {
                                     System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('mobile No. should be numeric digit at Row " + (count + 2) + "');", true);
                                     connection.Close();
                                     return;
                                 }
                                 if (Contect == "")
                                 {
                                     System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Insert Contact Person" + err + " at Row " + (count + 2) + "');", true);
                                     connection.Close();
                                     return;
                                 }
                                 if (mob == "")
                                 {
                                     System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Insert MobileNo" + err + " at Row " + (count + 2) + "');", true);
                                     connection.Close();
                                     return;
                                 }
                                 
                             }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Insert Contact Person" + err + " at Row " + (count + 2) + "');", true); connection.Close();
                                    return;
                                }
                         }
                         count++;
                     }

                     string mobile = temp.TrimEnd(',');
                     string str = "select count(*) from TransAddMeetUser where MeetId=" + ddlMeet.SelectedValue + " and MobileNo IN ('" + mobile + "' ) and MobileNo!='9999999999'";
                     int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                     if (exists > 0)
                     {
                         System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This mobile no already Exists-" + mobile + "');", true);
                         connection.Close();
                         return;
                       //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "alert('These mobile no already Exists" + mobile + "');", true);
                     }
                     else
                     {
                         foreach (DataRow myrow in objds.Tables[0].Rows)
                         {
                             string name1 = Convert.ToString(myrow.Table.Columns["AreaName"]);
                             string consString = DAL.DbConnectionDAL.sqlConnectionstring;
                             //string err = "";
                             int retsave = -1;
                             if (myrow.RowState != DataRowState.Deleted)
                             {
                                 Object[] cells = myrow.ItemArray;

                                 if (cells[0].ToString().Length > 0 || cells[1].ToString().Length > 0 || cells[2].ToString().Length > 0 || cells[3].ToString().Length > 0 || cells[4].ToString().Length > 0 || cells[5].ToString().Length > 0 || cells[6].ToString().Length > 0 || cells[7].ToString().Length > 0 || cells[8].ToString().Length > 0 || cells[9].ToString().Length > 0)
                                 {
                                     string AreaName = cells[0].ToString();
                                     if (AreaName == "Varchar(100)")
                                     {
                                         myrow.Delete();
                                         continue;
                                     }
                                     if (AreaName == "Example : AHMEDABAD")
                                     {
                                         myrow.Delete();
                                         continue;
                                     }

                                     string DOB = cells[8].ToString();

                                     if ((cells[2].ToString() != " ") && (cells[2].ToString() != " ") && (cells[4].ToString() != " ") && (cells[4].ToString() != " "))
                                     {
                                         if (ddlMeet.SelectedValue != "0")
                                         {
                                             retsave = dp.InsertMeetParty(Settings.DMInt32(ddlMeet.SelectedValue), Settings.DMInt32(GetAreaID(cells[0].ToString())), Settings.DMInt32(GetAreaID(cells[1].ToString())), cells[3].ToString(), cells[2].ToString(), cells[4].ToString(), cells[5].ToString(), cells[6].ToString(), Settings.DMDecimal(cells[7].ToString()), cells[8].ToString(), 0);
                                         }
                                         if (retsave != -1)
                                         {

                                             System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully-" + retsave + "');", true);
                                         }
                                     }
                                     
                                 }
                             }
                         }
                     }
                     connection.Close();
                 }
                 catch { }
                 finally
                 {
                     ddlBeat.SelectedIndex = -1;
                     ddlMeet.SelectedIndex = -1;
                     ddlmeetType.SelectedIndex = -1;
                     ddlPartyType.SelectedIndex = -1;
                     // Response.Redirect("~/ImportMeetUsers.aspx");
                     //connection.Close();
                 }

             }
            //Vikram 

           
        }

       

        /// <summary>
        /// Deletes empty rows from the end of the given worksheet
        /// </summary>
      
        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void insertTable()
        {
            // Read the file and convert it to Byte Array
            string filePath = Path.GetFullPath(FileUpload1.PostedFile.FileName);
            string filename = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename);
            string contenttype = String.Empty;
            FileUpload1.SaveAs(Server.MapPath("\\Files") + "\\" + FileUpload1.FileName);
            //Set the contenttype based on File Extension
           SqlBulkCopy oSqlBulk = null;

           // SET A CONNECTION WITH THE EXCEL FILE.Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("\\Files" + "\\" + filename) + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";";
                OleDbConnection myExcelConn = new OleDbConnection
                    ("Provider=Microsoft.Jet.OLEDB.4.0; " +
                        "Data Source=" + Server.MapPath(".") + "\\" + FileUpload1.FileName +
                        ";Extended Properties=\"Excel 8.0;");
                try
                {
                    OleDbCommand objOleDB =
                       new OleDbCommand("SELECT *FROM [Sheet1$]", myExcelConn);

                    // READ THE DATA EXTRACTED FROM THE EXCEL FILE.
                    OleDbDataReader objBulkReader = null;
                    objBulkReader = objOleDB.ExecuteReader();
                }
                catch { }
        }
        protected void ddlMeet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            this.BindDDlCity(Convert.ToInt32(ddl.SelectedValue));
            this.BindBeat(Convert.ToInt32(ddl.SelectedValue));
            //lblm.InnerHtml = "Name";
            fillNote();
        }

        public void fillNote()
        { 
            string PTypeStr1 = "";
            foreach (ListItem item in ddlPartyType.Items)
            {

                if (item.Selected)
                {
                    PTypeStr1 += item.Value + ",";
                    PTypeStr1 += item.Text + ",";
                }
            }
            PTypeStr1 = PTypeStr1.TrimStart(',').TrimEnd(',');
           
            //ContentPlaceHolder cph;
            //HtmlGenericControl lit;
            //Literal lst;
            //cph = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            //lit = (HtmlGenericControl)cph.FindControl("lastDiv");
            //lst = (Literal)lit.FindControl("lst");
            //lblm.InnerHtml = "NameLable";
            //lst.Text = "Name";
            //lit.Controls.Add(lst);
           // cph.Controls.Add(lit);
           // HtmlGenericControl div =new  HtmlGenericControl();
           // div.TagName = "div"; //specify the tag here
           // //then specify the attributes
           // //div.Attributes["height"] = "100%";
           // //div.Attributes["width"] = "100%";
           // //div.Attributes["class"] = "someclass";
           // div.Attributes["id"] = "someid";
           // div.InnerHtml = "Name";
           //// div.InnerHtml = " Note(3) : User can download Party list from Party Master   of Selected Meet City (" + ddlCity.SelectedItem.Text + ") , Selected Beat (" + ddlBeat.SelectedItem.Text + ") and Selected Partytype(" + PTypeStr1 + ")";
           // lastDiv.Controls.Add(div);
           
           // lastDiv.InnerHtml = " Note(3) : User can download Party list from Party Master   of Selected Meet City (" + ddlCity.SelectedItem.Text + ") , Selected Beat (" + ddlBeat.SelectedItem.Text + ") and Selected Partytype(" + PTypeStr1 + ")";
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fillNote();
        }

        protected void ddlBeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fillNote();
        }

        protected void ddlPartyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fillNote();
        }

        public void fillExcel(DataSet ds)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Sheet1.xls");
            
            using (ExcelPackage pack = new ExcelPackage())
            {
                foreach (System.Data.DataTable dt in ds.Tables)
                {
                    //System.Data.DataTable dt1 = new System.Data.DataTable();
                    //sqlStr = "Select * from " + dt.TableName + "";
                    //using (SqlDataAdapter da = new SqlDataAdapter(sqlStr, con))
                    //{

                    //    da.Fill(dt1);
                    //}
                   
                    string SheetName = "Sheet1";
                    //ExcelWorksheet ws = pack.Workbook.Worksheets["" + File + ""];
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("" + SheetName);
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                }
                string XlsPath = Server.MapPath(@"~/Files/MeetUser.xls");
                using (FileStream aFile = new FileStream(XlsPath, FileMode.Create))
                {
                    aFile.Seek(0, SeekOrigin.Begin);
                    pack.SaveAs(aFile);
                    aFile.Close();
                }
                Response.Redirect("~/Files/MeetUser.xls");
                //var ms = new System.IO.MemoryStream(XlsPath);
                //pack.SaveAs(ms);
                //ms.WriteTo(HttpContext.Current.Response.OutputStream);
            }

        }
    }
}