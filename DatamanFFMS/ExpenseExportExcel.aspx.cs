using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DAL;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Text;
using Excel=Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Drawing;


namespace AstralFFMS
{
    public partial class ExpenseExportExcel : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ExpenseGroupId"] != null)
            {
                int ExpGrpId = Convert.ToInt32(Request.QueryString["ExpenseGroupId"]);
                ExportExcel(ExpGrpId);
            }
           
        }

        private void ExportExcel(int ExpGrpId)
        {
            string str = @"SELECT 1 AS Seq,Convert(VARCHAR(10),T1.DateOfSubmission,103) AS DocDt,Convert(VARCHAR(10),T1.DateOfSubmission,103) AS PostingDt,
                            'SA' AS doctype,'1000' AS cocode,'INR' AS currency,isnull(T1.VoucherNo,0) AS ReferenceID,
                            CAST(Convert(VARCHAR(3),datename(MONTH,T1.DateOfSubmission))+Substring(cast(datepart(Year,T1.DateOfSubmission) AS CHAR(4)),3,4) AS CHAR(10))  AS DocHeader,
                            '40' AS PstKy,ISnull(T5.SyncID,'') AS Account,Amount =(Case When isnull(T2.CGSTAmt,0) != 0.00 then Isnull(T2.ApprovedAmount,0) - (isnull(t2.SGSTAmt,0) + isnull(t2.SGSTAmt,0)) 
	                         When isnull(T2.IGSTAmt,0) != 0.00 then Isnull(T2.ApprovedAmount,0) - (isnull(t2.IGSTAmt,0)) else Isnull(T2.ApprovedAmount,0) end),'S' AS DebitCredit,
                            Isnull(CAST(ma2.CostCentre AS VARCHAR(10) ) + CAST(right(t3.CostCentre, 9) AS VARCHAR(20) ),0) AS CostCentre,T4.SyncId AS [Order],
                            Convert(VARCHAR(10),T1.FromDate,103) +' To '+ Convert(VARCHAR(10),T1.ToDate,103) +',VOU NO-'+CAST(VoucherNo AS VARCHAR) AS Text,
                            BuisnessPlace=(CASE WHEN mb.Name is NULL THEN 'GJAP' ELSE mb.Name END),
							Section=(CASE WHEN ms.Name is null THEN 'GJAP' ELSE ms.Name END),
                            CASE Isnull(T2.IsGSTNNo,0) WHEN 1 THEN 'Registered' WHEN 0 THEN 'Unregistered' ELSE NULL END AS IsGSTIN,
                            T2.GSTNo,T2.Vendor,mt.Name as taxCode,isnull(T2.CGSTAmt,0) AS CGSTAmt,isnull(t2.SGSTAmt,0) AS SGSTAmt,isnull(t2.IGSTAmt,0) AS IGSTAmt
                            FROM ExpenseGroup AS T1
                            LEFT JOIN ExpenseDetails AS T2
                            ON T1.ExpenseGroupID = T2.ExpenseGroupID
                            LEFT JOIN MastLogin AS T3
                            ON T1.UserID = T3.Id
                            LEFT JOIN MastSalesRep AS T4
                            ON T1.SMID=T4.SMId
                            LEFT JOIN MastExpenseType AS T5
                            ON T2.ExpenseTypeID=T5.Id
                            LEFT join MastArea ma ON ma.AreaId=t2.CityID or ma.areaid=t2.fromcity
                            LEFT JOIN mastarea ma1 ON ma.UnderId=ma1.AreaId
                            LEFT JOIN mastarea ma2 ON ma1.UnderId=ma2.AreaId   
                            LEFT JOIN mastbuisnessplace mb ON mb.Id=ma2.BuisnessPlace
                            LEFT JOIN mastsection ms ON ms.Id=ma2.Section
                            LEFT JOIN MastTextCode mt ON mt.Id=t2.TaxCode                  
                            WHERE T1.ExpenseGroupID = " + ExpGrpId + "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            DataTable dt2 = new DataTable();
            dt2 = dt.Copy();          
            //To Export in excel in folder
            //if(dt!=null && dt.Rows.Count>0)
            //{
            //    dt.Rows.Add();
            //    //gvData.DataSource = dt;
            //    //gvData.DataBind();
            //    string strPath = Server.MapPath("TextFileFolder");

               
            //    //ExportToExcel(dt, strPath);
            //    DataSet ds = new DataSet();
            //    ds.Tables.Add(dt);
            //    ExportDataSetToExcel(ds);

            //    //Export("ExpenseExcelExport", gvData);
            //   // ExportToCSV("abc", dt, true);
            //}
            if (dt != null && dt.Rows.Count > 0)
            {
                int counter = 0;                
                for (int i = 0; i < dt.Rows.Count; i++ )
                {                    
                    if (dt.Rows[i]["CGSTAmt"].ToString() != "0.00")
                    {
                        counter = counter + 1;                           
                        DataRow dr = dt2.NewRow();
                        DataRow dr1 = dt2.NewRow();
                        dr["Seq"] = dt.Rows[i]["Seq"];
                        dr["DocDt"] = dt.Rows[i]["DocDt"];
                        dr["PostingDt"] = dt.Rows[i]["PostingDt"];
                        dr["doctype"] = dt.Rows[i]["doctype"];
                        dr["cocode"] = dt.Rows[i]["cocode"];
                        dr["currency"] = dt.Rows[i]["currency"];
                        dr["DocHeader"] = dt.Rows[i]["DocHeader"];
                        dr["ReferenceID"] = dt.Rows[i]["ReferenceID"];
                        dr["PstKy"] = dt.Rows[i]["PstKy"];
                        dr["Account"] = "38070009";
                        dr["Amount"] = dt.Rows[i]["CGSTAmt"];
                        dr["DebitCredit"] = dt.Rows[i]["DebitCredit"];
                        dr["CostCentre"] = dt.Rows[i]["CostCentre"];
                        dr["Order"] = dt.Rows[i]["Order"];
                        dr["Text"] = dt.Rows[i]["Text"];
                        dr["BuisnessPlace"] = dt.Rows[i]["BuisnessPlace"];
                        dr["Section"] = dt.Rows[i]["Section"];
                        dr["IsGSTIN"] = dt.Rows[i]["IsGSTIN"];
                        dr["GSTNo"] = dt.Rows[i]["GSTNo"];
                        dr["Vendor"] = dt.Rows[i]["Vendor"];
                        dr["taxCode"] = dt.Rows[i]["taxCode"];
                        dr["CGSTAmt"] = dt.Rows[i]["CGSTAmt"];
                        dr["SGSTAmt"] = dt.Rows[i]["SGSTAmt"];
                        dr["IGSTAmt"] = dt.Rows[i]["IGSTAmt"];
                        dt2.Rows.InsertAt(dr, i + counter);
                        counter = counter + 1; 
                        dr1["Seq"] = dt.Rows[i]["Seq"];
                        dr1["DocDt"] = dt.Rows[i]["DocDt"];
                        dr1["PostingDt"] = dt.Rows[i]["PostingDt"];
                        dr1["doctype"] = dt.Rows[i]["doctype"];
                        dr1["cocode"] = dt.Rows[i]["cocode"];
                        dr1["currency"] = dt.Rows[i]["currency"];
                        dr1["DocHeader"] = dt.Rows[i]["DocHeader"];
                        dr1["ReferenceID"] = dt.Rows[i]["ReferenceID"];
                        dr1["PstKy"] = dt.Rows[i]["PstKy"];
                        dr1["Account"] = "38070010";
                        dr1["Amount"] = dt.Rows[i]["SGSTAmt"]; 
                        dr1["DebitCredit"] = dt.Rows[i]["DebitCredit"];
                        dr1["CostCentre"] = dt.Rows[i]["CostCentre"];
                        dr1["Order"] = dt.Rows[i]["Order"];
                        dr1["Text"] = dt.Rows[i]["Text"];
                        dr1["BuisnessPlace"] = dt.Rows[i]["BuisnessPlace"];
                        dr1["Section"] = dt.Rows[i]["Section"];
                        dr1["IsGSTIN"] = dt.Rows[i]["IsGSTIN"];
                        dr1["GSTNo"] = dt.Rows[i]["GSTNo"];
                        dr1["Vendor"] = dt.Rows[i]["Vendor"];
                        dr1["taxCode"] = dt.Rows[i]["taxCode"];
                        dr1["CGSTAmt"] = dt.Rows[i]["CGSTAmt"];
                        dr1["SGSTAmt"] = dt.Rows[i]["SGSTAmt"];
                        dr1["IGSTAmt"] = dt.Rows[i]["IGSTAmt"];
                        dt2.Rows.InsertAt(dr1, i + counter);
                        dt2.AcceptChanges();
                                  
                    }
                  else  if (dt.Rows[i]["IGSTAmt"].ToString() != "0.00")
                    {
                        counter = counter + 1;
                        DataRow drIgst = dt2.NewRow();

                        drIgst["Seq"] = dt.Rows[i]["Seq"];
                        drIgst["DocDt"] = dt.Rows[i]["DocDt"];
                        drIgst["PostingDt"] = dt.Rows[i]["PostingDt"];
                        drIgst["doctype"] = dt.Rows[i]["doctype"];
                        drIgst["cocode"] = dt.Rows[i]["cocode"];
                        drIgst["currency"] = dt.Rows[i]["currency"];
                        drIgst["DocHeader"] = dt.Rows[i]["DocHeader"];
                        drIgst["ReferenceID"] = dt.Rows[i]["ReferenceID"];
                        drIgst["PstKy"] = dt.Rows[i]["PstKy"];
                        drIgst["Account"] = "38070011";
                        drIgst["Amount"] = dt.Rows[i]["IGSTAmt"];
                        drIgst["DebitCredit"] = dt.Rows[i]["DebitCredit"];
                        drIgst["CostCentre"] = dt.Rows[i]["CostCentre"];
                        drIgst["Order"] = dt.Rows[i]["Order"];
                        drIgst["Text"] = dt.Rows[i]["Text"];
                        drIgst["BuisnessPlace"] = dt.Rows[i]["BuisnessPlace"];
                        drIgst["Section"] = dt.Rows[i]["Section"];
                        drIgst["IsGSTIN"] = dt.Rows[i]["IsGSTIN"];
                        drIgst["GSTNo"] = dt.Rows[i]["GSTNo"];
                        drIgst["Vendor"] = dt.Rows[i]["Vendor"];
                        drIgst["taxCode"] = dt.Rows[i]["taxCode"];
                        drIgst["CGSTAmt"] = dt.Rows[i]["CGSTAmt"];
                        drIgst["SGSTAmt"] = dt.Rows[i]["SGSTAmt"];
                        drIgst["IGSTAmt"] = dt.Rows[i]["IGSTAmt"];
                        dt2.Rows.InsertAt(drIgst, i + counter);
                        dt2.AcceptChanges();
                    }
                    else
                    { }
                }

                string strbuisness = "SELECT CostCentre FROM MastLogin WHERE empsyncid='" + dt2.Rows[dt2.Rows.Count - 1]["Order"] + "' ";
                DataTable dtbuisness = DbConnectionDAL.GetDataTable(CommandType.Text, strbuisness);
                dt2.Rows.Add();
                dt2.Rows[dt2.Rows.Count - 1]["Seq"] = 1;
                dt2.Rows[dt2.Rows.Count - 1]["Docdt"] = dt2.Rows[dt2.Rows.Count - 2]["Docdt"];
                dt2.Rows[dt2.Rows.Count - 1]["Postingdt"] = dt2.Rows[dt2.Rows.Count - 2]["Postingdt"];
                dt2.Rows[dt2.Rows.Count - 1]["doctype"] = dt2.Rows[dt2.Rows.Count - 2]["doctype"];
                dt2.Rows[dt2.Rows.Count - 1]["cocode"] = dt2.Rows[dt2.Rows.Count - 2]["cocode"];
                dt2.Rows[dt2.Rows.Count - 1]["currency"] = dt2.Rows[dt2.Rows.Count - 2]["currency"];
                dt2.Rows[dt2.Rows.Count - 1]["DocHeader"] = dt2.Rows[dt2.Rows.Count - 2]["DocHeader"];
                dt2.Rows[dt2.Rows.Count - 1]["ReferenceID"] = dt2.Rows[dt2.Rows.Count - 2]["ReferenceID"];
                dt2.Rows[dt2.Rows.Count - 1]["PstKy"] = "31";
                dt2.Rows[dt2.Rows.Count - 1]["Account"] = dt2.Rows[dt2.Rows.Count - 2]["Order"];
                decimal decAmt = 0M;
                for (int i = 0; i < dt2.Rows.Count - 1; i++)
                {
                    decAmt += Convert.ToDecimal(dt2.Rows[i]["Amount"]);
                }
                dt2.Rows[dt2.Rows.Count - 1]["Amount"] = decAmt;
                dt2.Rows[dt2.Rows.Count - 1]["DebitCredit"] = "H";
                //dt2.Rows[dt2.Rows.Count - 1]["CostCentre"] = dtbuisness.Rows[0]["CostCentre"];
                //dt2.Rows[dt2.Rows.Count - 1]["Order"] = dt2.Rows[dt2.Rows.Count - 2]["Order"];
                dt2.Rows[dt2.Rows.Count - 1]["Text"] = dt2.Rows[dt2.Rows.Count - 2]["Text"];
                dt2.Rows[dt2.Rows.Count - 1]["BuisnessPlace"] = "GJAP";
                dt2.Rows[dt2.Rows.Count - 1]["Section"] = "GJAP";
                dt2.AcceptChanges();
                gvData.DataSource = dt2;
                gvData.DataBind();
                string strPath = Server.MapPath("ExpenseReport");            
                DataSet ds = new DataSet();
                ds.Tables.Add(dt2);
                ExportToExecl(ds, "ExpenseReport");               
            }
        }

        private string ExportToCSVFile(DataTable dtTable)
        {
            StringBuilder sbldr = new StringBuilder();
            if (dtTable.Columns.Count != 0)
            {
                foreach (DataColumn col in dtTable.Columns)
                {
                    sbldr.Append(col.ColumnName + ',');
                }
                sbldr.Append("\r\n");
                foreach (DataRow row in dtTable.Rows)
                {
                    foreach (DataColumn column in dtTable.Columns)
                    {
                        sbldr.Append(row[column].ToString().Replace(",", ";") + ',');
                    }
                    sbldr.Append("\r\n");
                }
            }
            return sbldr.ToString();
        }

        public void ExportToExecl(object DS, string fileName)
        {
            fileName += ".xls";
            string worksheetName = fileName;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);

            //htmlWrite.WriteLine("<b><u><font size='4'><text-align='center'> " + "OLAM" + "</br>");
            //htmlWrite.WriteLine("<b><u><font size='3'><horizontalalign='center'> " + "CourseName" + "");
            DataGrid dataExportExcel = new DataGrid();
            //dataExportExcel.ItemDataBound += new DataGridItemEventHandler(dataExportExcel_ItemDataBound);
            dataExportExcel.DataSource = DS;
            dataExportExcel.DataBind();
            dataExportExcel.RenderControl(htmlWrite);
            StringBuilder sbResponseString = new StringBuilder();
            sbResponseString.Append("<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"> <head><meta http-equiv=\"Content-Type\" content=\"text/html;charset=windows-1252\"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>" + worksheetName + "</x:Name><x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head> <body>");
            sbResponseString.Append(stringWriter + "</body></html>");

            HttpContext.Current.Response.Write(sbResponseString.ToString());
            HttpContext.Current.Response.End();
        }

        private void ExportDataSetToExcel(DataSet ds)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();

            string FilePathToDel = Server.MapPath("TextFileFolder//1.xls");

            if (System.IO.File.Exists(FilePathToDel))
            {
                System.IO.File.Delete(FilePathToDel);
            }
            
                System.IO.File.Create(FilePathToDel);
          

            string strPath = Server.MapPath("TextFileFolder//1.xls");
            //FileStream fs = new FileStream(strPath, FileMode.CreateNew);           
            //Create an Excel workbook instance and open it from the predefined location

            //using (System.IO.StreamWriter OrderFile = new System.IO.StreamWriter(strPath, false))
            //{
                Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(strPath);

                foreach (DataTable table in ds.Tables)
                {
                    //Add a new worksheet to workbook with the Datatable name
                    Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                    excelWorkSheet.Name = table.TableName;

                    for (int i = 1; i < table.Columns.Count + 1; i++)
                    {
                        excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                    }

                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        for (int k = 0; k < table.Columns.Count; k++)
                        {
                            excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                        }
                    }
                }

                excelWorkBook.Save();
                excelWorkBook.Close();
                excelApp.Quit();
            //}
            

        }

        //public static void ExportToCSV(string Filename, DataTable dtTable, bool ShowColumnHeader)
        //{
        //    StringBuilder sbContent = new StringBuilder(String.Empty);
        //    HttpContext.Current.Response.Clear();
        //    int i = 0;
        //    //Output Column Headers
        //    if (ShowColumnHeader)
        //    {
        //        int iCol = 0;
        //        for (iCol = 0; iCol <= dtTable.Columns.Count - 1; iCol++)
        //            sbContent.Append("\"" + dtTable.Columns[iCol].ToString() + "\"|");
        //        sbContent.Replace("|", "\r\n", sbContent.Length - 1, 1);
        //    }
        //    foreach (DataRow dr in dtTable.Rows)
        //    {
        //        for (i = 0; i < dtTable.Columns.Count; i++)
        //        {
        //            sbContent.Append("\"" + Convert.ToString(dr[i]).Replace("\"", "\"\"") + "\"|");
        //        }
        //        sbContent.Replace("|", "\r\n", sbContent.Length - 1, 1);
        //    }
        //    HttpContext.Current.Response.ContentType = "text/txt";
        //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Filename);
        //    HttpContext.Current.Response.Write(sbContent.ToString());
        //    HttpContext.Current.Response.End();
        //}

        public static void ExportToExcel(DataTable Tbl, string ExcelFilePath)
        {
            try
            {
                if (Tbl == null || Tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // column headings
                for (int i = 0; i < Tbl.Columns.Count; i++)
                {
                    workSheet.Cells[1, (i + 1)] = Tbl.Columns[i].ColumnName;
                }

                // rows
                for (int i = 0; i < Tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < Tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[(i + 2), (j + 1)] = Tbl.Rows[i][j];
                    }
                }
                workSheet.Name = "abc.xls";
                // check fielpath
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        workSheet.SaveAs(ExcelFilePath);
                        excelApp.Quit();
                        //MessageBox.Show("Excel file saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excelApp.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }
    

        //protected void ExportExcel(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable("gvData");
        //    foreach (TableCell cell in gvData.HeaderRow.Cells)
        //    {
        //        dt.Columns.Add(cell.Text);
        //    }
        //    foreach (GridViewRow row in gvData.Rows)
        //    {
        //        dt.Rows.Add();
        //        for (int i = 0; i < row.Cells.Count; i++)
        //        {
        //            dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
        //        }
        //    }
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);

        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //    }
        //}


       

        public static void Export(string fileName, GridView gv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName + "-" + DateTime.Now.ToShortDateString()));
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a form to contain the grid
                    Table table = new Table();
                    //table.BackColor = System.Drawing.Color.Azure;

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {

                        PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);
                    table.GridLines = GridLines.Both;
                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        /// <summary>
        /// Replace any of the contained controls with literals
        /// </summary>
        /// <param name="control"></param>
        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }
    }
}