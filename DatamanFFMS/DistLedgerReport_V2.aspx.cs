using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;
using Excel = Microsoft.Office.Interop.Excel;


namespace AstralFFMS
{
    public partial class DistLedgerReport_V2 : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            //if (Request.QueryString["DistId"] != null)
            //{
            //    string DistId = Convert.ToString(Request.QueryString["DistId"]);
            //    string fromdate = HttpContext.Current.Session["aaa"].ToString();
            //    string todate = HttpContext.Current.Session["bbb"].ToString();
            //    GetDetailLedgerData(Convert.ToInt32(DistId), fromdate, todate);

            //}
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {


                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());
                distreportrpt.DataSource = distributors;
                distreportrpt.DataBind();
                //if (Request.QueryString["DistId"] != null)
                //{
                //    string DistId = Convert.ToString(Request.QueryString["DistId"]);
                //    string fromdate = HttpContext.Current.Session["aaa"].ToString();
                //    string todate = HttpContext.Current.Session["bbb"].ToString();
                //    GetDetailLedgerData(Convert.ToInt32(DistId), fromdate, todate);
                //    // rptmain1.Style.Add("display", "none");
                //}
                CalendarExtender3.EndDate = Settings.GetUTCTime();
                //Added By - Nishu 06/12/2015 
                if (Request.QueryString["DistId"] != null)
                {
                    // BindTreeViewControl();
                    BindDistributorDDl(Session["treenodes"].ToString());
                    string[] accStaffAll = new string[500];
                    string distid = HttpContext.Current.Session["DistId"].ToString();
                    string accStaff = distid;
                    accStaffAll = accStaff.Split(',');
                    if (accStaffAll.Length > 0)
                    {
                        foreach (ListItem item in ListBox1.Items)
                        {
                            for (int i = 0; i < accStaffAll.Length; i++)
                            {
                                if (item.Value == accStaffAll[i].ToString())
                                {
                                    item.Selected = true;

                                }

                            }
                        }
                    }
                    txtfmDate.Text = HttpContext.Current.Session["aaa"].ToString();
                    txttodate.Text = HttpContext.Current.Session["bbb"].ToString();
                }
                else
                {
                    txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                    txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                }
                //End
                //Ankita - 13/may/2016- (For Optimization)
                roleType = Settings.Instance.RoleType;
                //  GetRoleType(Settings.Instance.RoleID);
                //fill_TreeArea();
                BindTreeViewControl();
                //btnExport.Visible = true;
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";
            }
        }

        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
                //    DataSet ds = GetDataSet("Select smid,smname,underid,lvl from mastsalesrep where active=1 and underid<>0 order by smname");


                DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    root.CollapseAll();
                    CreateNode(root, St);
                    trview.Nodes.Add(root);
                }
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, DataTable Dt)
        {
            DataRow[] Rows = Dt.Select("underid =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(Childnode);
                Childnode.CollapseAll();
                CreateNode(Childnode, Dt);
            }
        }
        public class Distributors
        {

            public string Distributor { get; set; }
            public string Debit { get; set; }
            public string Credit { get; set; }
            public string Closing { get; set; }

        }
        //[WebMethod(EnableSession = true)]
        public void getdistributorledger()
        {
            DataSet dataSet = new DataSet("Distributor Ledger Report");
            string qry = "";

            String smIDStr1 = "", dist = "", dist1 = "", prdgrp = "", prd = "";
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 += node.Value + ",";
            }
            smIDStr1 = smIDStr1.TrimEnd(',');

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    dist += item.Value + ",";
                }
            }
            dist = dist.TrimEnd(',');

            string Qrychk = " tds.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

            if (smIDStr1 != "")
            {
                if (dist != "")
                {
                    string query = @"select Max(msp.SMName)+'-'+max(Isnull(msp.SyncId,'')) AS SMNAME,Max(mss.AreaName)+'-'+max(Isnull(mss.SyncId,'')) AS State,Max(msc.AreaName)+'-'+max(Isnull(msc.SyncId,'')) AS City,Max(msa.AreaName)+'-'+max(Isnull(msa.SyncId,'')) AS Area, tds.DistId, mp.partyname+'-'+Isnull(mp.SyncId,'') as Distributor, sum(Amtdr) AS Debit, Sum(Amtcr) AS Credit,(sum(AmtDr)-sum(AmtCr))  as Closing from TransDistributerLedger tds left join MastParty mp on mp.PartyId=tds.DistId Left Join MastArea msa on msa.AreaId=mp.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId left join MastSalesRep msp on msp.SMId=mp.SMID WHERE mp.SMID in (" + smIDStr1 + ") and tds.DistId in(" + dist + ") AND mp.PartyDist=1 and " + Qrychk + " group by DistId,PartyName order by Partyname"; 

                    DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    dtItem.TableName = "Dist Ledger";
                    foreach(DataRow row in dtItem.Rows)
                    {
                        dist1 += row["DistId"] + ","; 
                    }
                    dist1 = dist1.TrimEnd(',');
                    dataSet.Tables.Add(dtItem);
                    string[] values = dist1.Split(',');

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        string str = " select SyncId from MastParty where partyid= '" + values[i] + "'";
                        string partyname = DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();
                        System.Data.DataTable dt1 = new System.Data.DataTable(partyname.ToString());
                        dt1 = DetailDistLedger(Convert.ToInt32(values[i]), txtfmDate.Text, txttodate.Text);
                        dt1.TableName = partyname.ToString();
                        dataSet.Tables.Add(dt1);
                    }
                   int count1= dataSet.Tables.Count;

                    try
                    {
                        Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                        string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                        if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = "Distributor Ledger Report.xlsx";

                        if (File.Exists(path + filename))
                        {
                            File.Delete(path + filename);
                        }

                        //Excel.Application excelApp = new Excel.Application();
                        string strPath = HttpContext.Current.Server.MapPath("ExportedFiles//" + filename);
                        Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                        Microsoft.Office.Interop.Excel.Range chartRange;
                        Excel.Range range;
                        Excel.Sheets xlSheets = null;
                        Excel.Worksheet xlWorksheet = null;
                        // Loop over DataTables in DataSet.
                        DataTableCollection collection = dataSet.Tables;

                        for (int i = collection.Count; i > 0; i--)
                        {
                            //Create Excel Sheets
                            xlSheets = ExcelApp.Sheets;
                            xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                                           Type.Missing, Type.Missing, Type.Missing);

                            System.Data.DataTable table = collection[i - 1];

                            //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('" + collection[i - 1] + "');", true);
                            //xlWorksheet.Name = table.TableName;

                            xlWorksheet.Name = table.TableName;

                            for (int j = 1; j < table.Columns.Count + 1; j++)
                            {
                                ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;

                                range = xlWorksheet.Cells[1, j] as Excel.Range;
                                range.Cells.Font.Name = "Calibri";
                                range.Cells.Font.Bold = true;
                                range.Cells.Font.Size = 11;
                                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                            }

                            // Storing Each row and column value to excel sheet
                            for (int k = 0; k < table.Rows.Count; k++)
                            {
                                for (int l = 0; l < table.Columns.Count; l++)
                                {
                                    ExcelApp.Cells[k + 2, l + 1] =
                                    table.Rows[k].ItemArray[l].ToString();
                                }
                            }
                            ExcelApp.Columns.AutoFit();
                            xlWorksheet.Activate();
                            xlWorksheet.Application.ActiveWindow.SplitRow = 1;
                            xlWorksheet.Application.ActiveWindow.FreezePanes = true;

                            Excel.Range last = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                            chartRange = xlWorksheet.get_Range("A1", last);
                            foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                            {
                                cell.BorderAround2();
                            }
                            Excel.FormatConditions fcs = chartRange.FormatConditions;
                            Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
            (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                            format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                        }
                        xlWorksheet.PageSetup.CenterFooter = "&B Copyright © 2016 e-iceblue. All Rights Reserved.";
                        xlWorkbook.SaveAs(strPath);
                        xlWorkbook.Close();
                        ExcelApp.Quit();
                        Response.ContentType = "Application/xlsx";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.TransmitFile(strPath);
                        Response.End();
                        //xlWorkbook.Close();
                        //ExcelApp.Quit();
                        //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                        //ExcelApp.Visible = true;
                    }
                    catch (Exception ex)
                    {

                    }

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record File Generated Successfully');", true);
                    dtItem.Dispose();
                    dataSet.Dispose();
                    spinner.Visible = false;
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Distributor');", true);
                } 
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Sales Person');", true);
            }
        }
        private void BindSalePersonDDl()
        {
            try
            {
                if (roleType == "Admin")
                {//Ankita - 17/may/2016- (For Optimization)
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    DataTable dtcheckrole = new DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";

                    //salespersonListBox.DataSource = dv1.ToTable();
                    //salespersonListBox.DataTextField = "SMName";
                    //salespersonListBox.DataValueField = "SMId";
                    //salespersonListBox.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    //     dv.RowFilter = "RoleName='Level 1'";
                    dv.RowFilter = "SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        //salespersonListBox.DataSource = dv.ToTable();
                        //salespersonListBox.DataTextField = "SMName";
                        //salespersonListBox.DataValueField = "SMId";
                        //salespersonListBox.DataBind();
                    }
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string citystr = "";
                    //string cityQry = @"  select * from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (" + SMIDStr + ")) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);

                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');
                    string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";

                    //              string distqry = @"select * from MastParty where CityId=" + dtCity.Rows[0]["AreaId"] + " and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    distreportrpt.DataSource = null;
                    distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            getdistributorledger();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistLedgerReport.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");
            GetDetailLedgerData(Convert.ToInt32(hdnVisItCode.Value), txtfmDate.Text, txttodate.Text);
        }
        private void GetDetailLedgerData(int distId, string fromDate, string toDate)
        {
            try
            {
                string str = " select PartyName from MastParty where partyid= '" + distId + "'";
                //Ankita - 13/may/2016- (For Optimization)
                string partyname = DbConnectionDAL.GetScalarValue(CommandType.Text, str).ToString();
                // DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                // if (dt1.Rows.Count > 0)
                // lblDist.Text = dt1.Rows[0]["PartyName"].ToString();
                lblDist.Text = partyname;

                //  lblDist.Text = Convert.ToString(distId);
                DataTable dt = DetailDistLedger(distId, fromDate, toDate);
                if (dt.Rows.Count > 0)
                {
                    detailDiv.Style.Add("display", "block");
                    detailDistrpt.Visible = true;
                    lblDist.Visible = true;
                    detailDistrpt.DataSource = dt;
                    detailDistrpt.DataBind();

                }
                else
                {
                    detailDiv.Style.Add("display", "block");
                    detailDistrpt.DataSource = dt;
                    detailDistrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public static DataTable DetailDistLedger(int DistId, string fromDate, string toDate)
        {

            DateTime fromTime = Convert.ToDateTime(fromDate);
            DateTime toTime = Convert.ToDateTime(toDate);
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@Distributor_Id", DbParameter.DbType.Int, 1, DistId);
            dbParam[1] = new DbParameter("@From_Date", DbParameter.DbType.DateTime, 1, fromTime);
            dbParam[2] = new DbParameter("@To_Date", DbParameter.DbType.DateTime, 1, toTime);
            return DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_selectDistributorLedger", dbParam);
        }

        protected void salespersonListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string smIDStr12 = "";
            //foreach (ListItem saleperson in salespersonListBox.Items)
            //{
            //    if (saleperson.Selected)
            //    {
            //        smIDStr12 += saleperson.Value + ",";
            //    }
            //}
            //smIDStr12 = smIDStr12.TrimStart(',').TrimEnd(',');
            //BindDistributorDDl(smIDStr12);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorLedgerReport.csv");
            string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Debit".TrimStart('"').TrimEnd('"') + "," + "Credit".TrimStart('"').TrimEnd('"') + "," + "Closing Balance".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Debit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Credit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ClosingBalance", typeof(String)));

            foreach (RepeaterItem item in distreportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
                dr["DistributorName"] = DistributorLabel.Text;
                Label drLabel = item.FindControl("drLabel") as Label;
                dr["Debit"] = drLabel.Text.ToString();
                Label CrLabel = item.FindControl("CrLabel") as Label;
                dr["Credit"] = CrLabel.Text.ToString();
                Label cBalanceLabel = item.FindControl("cBalanceLabel") as Label;
                dr["ClosingBalance"] = cBalanceLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {

                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {


                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else
                    {

                        sb.Append(dtParams.Rows[j][k].ToString() + ',');


                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorLedgerReport.csv");
            Response.Write(sb.ToString());
            Response.End();

            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=DistributorLedgerReport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //distreportrpt.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            string smIDStr = "", smIDStr12 = "";
            if (cnt == 0)
            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                string smiMStr = smIDStr12;
                if (smIDStr12 != "")
                {
                    BindDistributorDDl(smIDStr12);
                }
                else
                {
                    ListBox1.Items.Clear();
                    ListBox1.DataBind();
                }
                Session["treenodes"] = smIDStr12;

            }
            cnt = cnt + 1;
            return;
        }


        protected void ExportCSV(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorLedgerReport.csv");
            string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Debit".TrimStart('"').TrimEnd('"') + "," + "Credit".TrimStart('"').TrimEnd('"') + "," + "Closing Balance".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            string smIDStr1 = "", Qrychk = "", Query = "";
            DataTable dt = new DataTable();
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

            Qrychk = " TransDistributerLedger.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            if (smIDStr1 != "")
            {
                if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                    {
                        //Query = "select q.DistId,q.Distributor,case when q.[oBalence]=0 then '-' when q.oBalence > 0 then CONVERT(varchar, q.oBalence) + ' Cr' else CONVERT(varchar, abs(q.oBalence)) + ' Dr' end [oBalance],sum (q.dr) as dr,sum(q.Cr) as cr,case when ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0)) = 0 then '-' when ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - q.dr) > 0 then CONVERT(varchar, ((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0))) + ' Cr' else CONVERT(varchar, abs((isnull(q.oBalence, 0) + isnull(q.Cr, 0)) - isnull(q.dr, 0))) + ' Dr' end [cBalance] from (select a.DistId,a.Distributor [Distributor],sum(a.oBalence) as [oBalence],SUM(a.dr) as dr,SUM(a.Cr) as Cr,SUM(a.cBalance) as CBalence FROM(select dl.DistId,d.PartyName as [Distributor],(select SUM(amtCr-amtDr) from TransDistributerLedger where VDate < '" + Settings.dateformat(txtfmDate.Text) + "' and d.PartyId= dl.DistId)  as [oBalence],0 as dr,0 as Cr,0 as cBalance from TransDistributerLedger dl inner join MastParty d  on d.PartyId=dl.DistId where (dl.DistId in (" + smIDStr1 + ") ) and VDate < '" + Settings.dateformat(txtfmDate.Text) + "'  group by dl.DistId,d.PartyName,d.PartyId union all select dl.DistId, d.PartyName as [Distributor], 0 as [oBalence],(select SUM( dl.AmtDr) from TransDistributerLedger dl where " + Qrychk + ") as dr,(select SUM( dl.Amtcr) from TransDistributerLedger dl where " + Qrychk + ") as Cr,(select sum(dl.AmtDr-dl.AmtCr) from TransDistributerLedger dl ) as cBalance from TransDistributerLedger dl inner join MastParty d on d.PartyId=dl.DistId where (dl.DistId in (" + smIDStr1 + ")) and " + Qrychk + " group by dl.DistId, d.PartyName) a group by a.DistId,a.Distributor) q group by q.DistId,q.Distributor,q.oBalence ,q.Cr,q.dr";
                        Query = "select  partyname as Distributor, sum(Amtdr) AS Debit, Sum(Amtcr) AS Credit,(sum(AmtDr)-sum(AmtCr)) as Closing from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId WHERE TransDistributerLedger.DistId IN (" + smIDStr1 + ") AND mastparty.PartyDist=1 and " + Qrychk + " group by DistId,PartyName order by Partyname";

                        dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


                    }

                }
            }
            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {

                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {


                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                    }
                    else
                    {

                        sb.Append(dtParams.Rows[j][k].ToString() + ',');


                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DistributorLedgerReport.csv");
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}