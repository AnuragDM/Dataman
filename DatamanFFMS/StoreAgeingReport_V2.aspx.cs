using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class StoreAgeingReport_V2 : System.Web.UI.Page
    {     
         
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {               
                fillDDLDirect(ddlretailer, "select partyid,partyname from mastparty where active=1 and partydist=1 order by partyname asc", "partyid", "partyname", 1);
                fillDDLDirect(ddlbeat, "select areaid,areaname from mastarea where active=1 and areatype='BEAT' order by areaname asc", "areaid", "areaname", 1);
                fillDDLDirect(ddlitem, "select Itemid,Itemname from mastitem where active =1 and ItemType='ITEM' order by Itemname asc", "Itemid", "Itemname", 1);
            }
        }      

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/StoreAgeingReport_V2.aspx");
        }        
       
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }

        protected void btnsaleperson_Click(object sender, EventArgs e)
        {
            GetDetail("D");
        }

        protected void btndist_Click(object sender, EventArgs e)
        {
            GetDetail("B");
        }

        protected void btnbeat_Click(object sender, EventArgs e)
        {
            GetDetail("I");
        }
        private void GetDetail(string flag)
        {
            #region Variable Declaration

            DataTable dt = new DataTable(); string sql = ""; 
            DataTable gvdt = new DataTable();

            #endregion
           
            
            if (flag == "I")
            {
                gvdt.Columns.Add(new DataColumn("Retailer Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Item Name", typeof(String)));               
                gvdt.Columns.Add(new DataColumn("Last Order Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Days", typeof(String)));

                sql = "	Select ROW_NUMBER()  OVER (ORDER BY   PartyName asc) As SrNo,* ,'" + ddlitem.SelectedItem.Text + "' as itname,case when DayGap is null then 'Never Ordered' else  CAST(DayGap AS varchar) end as DayGap1 from (";
                sql += "select mp.partyname,mp.partyid, (select itemname from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) as ItemName,";
                sql += "(select partyname from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) as PartyNameOreder,";
                sql += "(select partyid from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) PartyidOrder,";
                sql += "(select [last order] from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) DayGap,";
                sql += "(select [Last order date] from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) LastOrderDate";
                sql += " from mastparty mp where mp.partydist=0 and mp.active=1  )tbl order by partyname asc";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow mDataRow = gvdt.NewRow();

                        mDataRow["Retailer Name"] = dr["PartyName"].ToString();
                        mDataRow["Item Name"] = dr["itname"].ToString();
                        if (!string.IsNullOrEmpty(dr["LastOrderDate"].ToString()))
                            mDataRow["Last Order Date"] = Convert.ToDateTime(dr["LastOrderDate"].ToString()).ToString("dd/MMM/yyyy").Trim();
                        mDataRow["Days"] = dr["DayGap1"].ToString();

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
                else
                {
                   
                }
            }
            else if (flag == "B")
            {
                gvdt.Columns.Add(new DataColumn("Retailer Name", typeof(String)));              
                gvdt.Columns.Add(new DataColumn("Address", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Last Order Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Days", typeof(String)));

                sql = "select ROW_NUMBER()  OVER (ORDER BY   PartyName asc) As SrNo,* , case when [LastOrder] is null then 'Never Ordered' else CAST([LastOrder] AS varchar) + ' days ago' end Gap1 from ( ";
                sql += "select mp.beatid,mp.partyid,mp.PartyName,max(mp.Address) [add],max(tro.OrderAmount) [amt],max(tro.vdate) [date]";
                sql += "        , DATEDIFF(day, max(tro.vdate), getdate()) AS [LastOrder]";
                sql += "        from mastparty mp ";
                sql += "        left join transorder tro on mp.partyid=tro.PartyId";
                sql += "        left join transorder1 tro1 on tro1.ordid=tro.ordid";
                sql += "         where PartyDist=0 and Active=1 and mp.beatid in (" + ddlbeat.SelectedItem.Value + ") and mp.partyid in (select partyid from mastparty where beatid=" + ddlbeat.SelectedItem.Value + " and partydist=0)";
                sql += "            group by  mp.partyid,mp.PartyName,mp.beatid ) tbl ";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow mDataRow = gvdt.NewRow();

                        mDataRow["Retailer Name"] = dr["PartyName"].ToString();
                        mDataRow["Address"] = dr["add"].ToString();
                        if (!string.IsNullOrEmpty(dr["date"].ToString()))
                            mDataRow["Last Order Date"] = Convert.ToDateTime(dr["date"].ToString()).ToString("dd/MMM/yyyy").Trim();
                        mDataRow["Days"] = dr["Gap1"].ToString();

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
                else
                {
                   
                }
            }
            if (flag == "D")
            {
                gvdt.Columns.Add(new DataColumn("Retailer Name", typeof(String)));               
                gvdt.Columns.Add(new DataColumn("Address", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Last Order Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Days", typeof(String)));
               
                sql = "select ROW_NUMBER()  OVER (ORDER BY   PartyName asc) As SrNo,* , case when [LastOrder] is null then 'Never Ordered' else CAST([LastOrder] AS varchar) + ' days ago' end Gap1 from ( ";
                sql += "select mp.beatid,mp.partyid,mp.PartyName,max(mp.Address) [add],max(tro.OrderAmount) [amt],max(tro.vdate) [date]";
                sql += "        , DATEDIFF(day, max(tro.vdate), getdate()) AS [LastOrder]";
                sql += "        from mastparty mp ";
                sql += "        left join transorder tro on mp.partyid=tro.PartyId";
                sql += "        left join transorder1 tro1 on tro1.ordid=tro.ordid";
                sql += "         where PartyDist=0 and Active=1  and mp.partyid in (select partyid from mastparty where beatid in (select areaid  from mastarea where underid in (select areaid from mastparty where partyid=" + ddlretailer.SelectedItem.Value + "))  and partydist=0)";
                sql += "            group by  mp.partyid,mp.PartyName,mp.beatid ) tbl";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow mDataRow = gvdt.NewRow();

                        mDataRow["Retailer Name"] = dr["PartyName"].ToString();
                        mDataRow["Address"] = dr["add"].ToString();
                        if (!string.IsNullOrEmpty(dr["date"].ToString()))
                        mDataRow["Last Order Date"] = Convert.ToDateTime(dr["date"].ToString()).ToString("dd/MMM/yyyy").Trim();
                        mDataRow["Days"] = dr["Gap1"].ToString();

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
                else
                {
                   
                }
            }
            if (flag == "Distributor")
            {
                gvdt.Columns.Add(new DataColumn("Distributor", typeof(String)));              
                gvdt.Columns.Add(new DataColumn("Last Order Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Days Since LAst Order", typeof(String)));

                sql = "select partyname,orderdate,case when DATEDIFF(day,orderdate,getdate())=0 then 'Order Placed Today' else cast ( DATEDIFF(day,orderdate,getdate()) as varchar) end  diff  from ( ";
                sql += "select mp.partyname,distid,max(vdate) orderdate from TransPurchOrder left join mastparty mp on mp.PartyId=TransPurchOrder.distid group by mp.partyname,distid) tbl order by diff desc";               
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow mDataRow = gvdt.NewRow();

                        mDataRow["Distributor"] = dr["partyname"].ToString();

                        if (!string.IsNullOrEmpty(dr["orderdate"].ToString()))
                            mDataRow["Last Order Date"] = Convert.ToDateTime(dr["orderdate"].ToString()).ToString("dd/MMM/yyyy").Trim();

                        mDataRow["Days Since LAst Order"] = dr["diff"].ToString();

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
                else
                {

                }
            }
            sql = null;
            // return dt;
        }

         public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }


        private void ExportDataSetToExcel(DataTable table)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();
            string path = Server.MapPath("ExportedFiles//");

            string StrVar = string.Empty;

            StrVar = "Distributor Last Order";

            if(ddlretailer.SelectedValue != "0")
            {
                StrVar = "Distributor wise";
            }
            if (ddlbeat.SelectedValue != "0")
            {
                StrVar = "Beat wise";
            }
            if (ddlitem.SelectedValue != "0")
            {
                StrVar = "Item wise";
            }

            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
            {
                Directory.CreateDirectory(path);
            }
            string filename = "Store Ageing Report " + "-" + StrVar + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            if (table.Rows.Count > 0)
            {
                //table.Columns.Remove("SMID");

                //Add a new worksheet to workbook with the Datatable name

                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = "Store Ageing Report";

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

                Excel.Worksheet worksheet = (Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
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

        protected void btnDistributor_Click(object sender, EventArgs e)
        {
            GetDetail("Distributor");
        }

    }
}