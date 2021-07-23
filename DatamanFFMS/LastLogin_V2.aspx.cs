using DAL;
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
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class LastLogin_V2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                string currentMonth = DateTime.Now.Month.ToString();
                DropDownList1.SelectedValue = currentMonth;

            }
            string sMonth = DropDownList1.SelectedValue;
            DropDownList1.SelectedValue = sMonth;
            //sMonth = "select Column1 from TableName where month(DateColumn) = month(dateadd(dd, -1, GetDate()))";
          //  fillRepeter();
            //string pageName = Path.GetFileName(Request.Path);
            //string Headername = fillDisplayname(pageName);
            // lbllastlogin.Text = Headername;
        }
        private int Month
        {
            get
            {
                return int.Parse(DropDownList1.SelectedItem.Value);
            }
            set
            {
                //this.PopulateMonth();
                DropDownList1.ClearSelection();
                DropDownList1.Items.FindByValue(value.ToString()).Selected = true;
            }
        }
        private string fillDisplayname(string pgname)
        {
            string displayname = string.Empty;
            string mastPartyQry1 = string.Empty;

            mastPartyQry1 = @"select DisplayName from mastpage where PageName in ('" + pgname + "') ";
            displayname = DbConnectionDAL.GetStringScalarVal(mastPartyQry1);
            return displayname;

        }

        private void fillRepeter()
        {
            #region Variable Declaration

            string Query = "";
            DataTable dt = new DataTable();
            DataTable gvdt = new DataTable();

            #endregion            


            string sMonth = DropDownList1.SelectedValue;
            string sql = string.Empty;
            if (DdlSalesPerson.SelectedValue == "Distributor")
            {
                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Distributor", typeof(String)));              
                gvdt.Columns.Add(new DataColumn("Application", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Version", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Login Date & Time", typeof(String)));

                sql = "select Partyname as EmpName,(select [LoginDateTime] from [LastLoginDetails] where Product='Grahaak_Distributor' and UserId=Mastparty.Userid and  month(logindatetime)=" + sMonth + " ) as LastLogin,(select ApkVersion from [LastLoginDetails] where Product='Grahaak_Distributor' and UserId=Mastparty.Userid and month(logindatetime)=" + sMonth + " ) as Version,'Grahaak-Distributor' App from mastparty where partydist=1 and active=1 order by LastLogin desc,partyname asc";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow mDataRow = gvdt.NewRow();


                        mDataRow["Distributor"] = dr["EmpName"].ToString();                      
                        mDataRow["Application"] = dr["App"].ToString();
                        mDataRow["Version"] = dr["Version"].ToString();
                        if (!string.IsNullOrEmpty(dr["LastLogin"].ToString()))
                            mDataRow["Login Date & Time"] = Convert.ToDateTime(dr["LastLogin"].ToString()).ToString("dd/MMM/yyyy").Trim();

                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();


                    }
                    if (gvdt.Rows.Count > 0)
                    {
                        ExportDataSetToExcel(gvdt);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found');", true);
                    }
                }
            }
            else
            {
                string filter = string.Empty;

                Query = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";
                DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dtdesig.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                    }
                }

                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reporting Manager", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Status", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Designation", typeof(String)));
                gvdt.Columns.Add(new DataColumn("HeadQuater", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Application", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Version", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Login Date & Time", typeof(String)));

                //if(DdlSalesPerson.SelectedValue=="Manager")
                //{
                //    filter = " and roleid in (select roleid from mastrole where roletype in ('DistrictHead','StateHead'))";
                //}
                //if (DdlSalesPerson.SelectedValue == "Field")
                {
                    filter = " and ms.roleid in (select roleid from mastrole where roletype in ('DistrictHead','AreaIncharge','StateHead'))";
                }
                sql = "select ms.SMId,ms.SyncId, ms.Smname+' - '+ms.mobile  as EmpName,(select [LoginDateTime] from [LastLoginDetails] where Product='" + DdlSalesPerson.SelectedValue + "'  and UserId=ms.Userid  and  month(logindatetime)=" + sMonth + " ) as LastLogin,(select ApkVersion from [LastLoginDetails] where Product='" + DdlSalesPerson.SelectedValue + "' and UserId=ms.Userid  and  month(logindatetime)=" + sMonth + ") as Version,'Grahaak-" + DdlSalesPerson.SelectedValue + "' App,hq.HeadquarterName,case  when ISNULL(ms.ACTIVE,0)=1 THEN 'Active' else 'Unactive' end  as Active,sr.smname as reportingPerson,mdst.DesName   from mastsalesrep ms left join MastHeadquarter hq on hq.Id=ms.HeadquarterId left join mastsalesrep sr on sr.smid=ms.underid left join MastLogin ml on ml.id=ms.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId where ms.active=1 and ms.smname not in ('.')  " + filter + "  order by LastLogin desc,ms.Smname asc";

                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow mDataRow = gvdt.NewRow();
                        //
                        Query = "select msp.SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                        DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        if (dtsr.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtsr.Rows.Count; j++)
                            {
                                if (dtsr.Rows[j]["DesName"].ToString() != "")
                                {
                                    mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                                }
                            }
                        }

                        for (int i = 0; i < dtdesig.Rows.Count; i++)
                        {
                            if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                            {
                                mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                            }
                        }

                        mDataRow["SMId"] = dr["SMId"].ToString();
                        mDataRow["Reporting Manager"] = dr["reportingPerson"].ToString();
                        mDataRow["Name"] = dr["EmpName"].ToString();
                        mDataRow["SyncId"] = dr["SyncId"].ToString();
                        mDataRow["Status"] = dr["Active"].ToString();
                        mDataRow["Designation"] = dr["DesName"].ToString();
                        mDataRow["HeadQuater"] = dr["HeadquarterName"].ToString();
                      //  mDataRow["Contact No"] = dr["mobile"].ToString();
                        mDataRow["Application"] = dr["App"].ToString();
                        mDataRow["Version"] = dr["Version"].ToString();
                        if (!string.IsNullOrEmpty(dr["LastLogin"].ToString()))
                        mDataRow["Login Date & Time"] = Convert.ToDateTime(dr["LastLogin"].ToString()).ToString("dd/MMM/yyyy").Trim();  

                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();


                    }
                    if (gvdt.Rows.Count > 0)
                    {
                        ExportDataSetToExcel(gvdt);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found');", true);
                    }
                }
            }
         
        }

        protected void DdlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
           // fillRepeter();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // fillRepeter();

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillRepeter();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LastLogin_V2.aspx");
        }
        private void ExportDataSetToExcel(DataTable table)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();
            string path = Server.MapPath("ExportedFiles//");

            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
            {
                Directory.CreateDirectory(path);
            }
            string filename = "Last Login Report " + "-" + DropDownList1.SelectedValue + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("SMID");

                //Add a new worksheet to workbook with the Datatable name

                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = "Last Login Report";

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                    range = excelWorkSheet.Cells[1, i] as Excel.Range;
                    range.Cells.Font.Name = "Calibri";
                    range.Cells.Font.Bold = true;
                    range.Cells.Font.Size = 11;
                    range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int l = 0; l < table.Columns.Count; l++)
                    {
                        excelWorkSheet.Cells[j + 2, l + 1] = table.Rows[j].ItemArray[l].ToString();
                    }
                }

                Excel.Range last = excelWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                chartRange = excelWorkSheet.get_Range("A1", last);
                foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                {
                    cell.BorderAround2();
                }


                Excel.FormatConditions fcs = chartRange.FormatConditions;
                Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                format.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                excelWorkSheet.Columns.AutoFit();

                // excelWorkSheet.Activate();
                excelWorkSheet.Application.ActiveWindow.SplitRow = 1;
                excelWorkSheet.Application.ActiveWindow.FreezePanes = true;

                Excel.Worksheet worksheet =(Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
                worksheet.Delete();

                excelWorkBook.SaveAs(strPath);
                excelWorkBook.Close();
                excelApp.Quit();
                // excelApp.Visible = true;
                Response.ContentType = "application/xlsx";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.TransmitFile(strPath);
                Response.End();

            }



        }
    }
}