using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class rptBuisnessReport_V2 : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        DataTable YrTable = new DataTable("Type");
        DataColumn dtColumn;
        DataRow myDataRow;
        DataTable gvdt = new DataTable("Sales Person Business Report");

        protected void Page_Load(object sender, EventArgs e)
        {
            spinner.Visible = false;
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");

            trview.Attributes.Add("onclick", "fireCheckChanged(event)");

            YrTable.Columns.Add("TypId", typeof(int));
            YrTable.Columns.Add("TypName", typeof(string));
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                //btnExport.Visible = true;
                BindStaffChkList();
                BindTreeViewControl();
                fillDDLDirect(lstPrmAreaBox, "select Distinct AreaId,AreaName from MastArea where AreaType='Area' order by AreaName asc", "AreaId", "AreaName", 1);

                fillDDLDirect1(LstSecArea, "select Distinct AreaId,AreaName from MastArea where AreaType='Area' order by AreaName asc", "AreaId", "AreaName", 1);
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";
            }
        }


        public static void fillDDLDirect(ListBox xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            //if (sele == 1)
            //{
            //    if (xdt.Rows.Count >= 1)
            //        xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            //    else if (xdt.Rows.Count == 0)
            //        xddl.Items.Insert(0, new ListItem("---", "0"));
            //}
            //else if (sele == 2)
            //{
            //    xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            //}
            xdt.Dispose();
        }

        public static void fillDDLDirect1(ListBox xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            //if (sele == 1)
            //{
            //    if (xdt.Rows.Count >= 1)
            //        xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            //    else if (xdt.Rows.Count == 0)
            //        xddl.Items.Insert(0, new ListItem("---", "0"));
            //}
            //else if (sele == 2)
            //{
            //    xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            //}
            xdt.Dispose();
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            spinner.Visible = true;
            if (LstType.SelectedIndex == 0)
            {
                lstPrmAreaBox.SelectedIndex = -1;
                lstPrmAreaBox.SelectedIndex = -1;
                lstPrmPrsn.SelectedIndex = -1;
                getsales();
            }
            else if (LstType.SelectedIndex == 1)
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }
                //trview_TreeNodeCheckChanged(trview, null);
                LstSecArea.SelectedIndex = -1;
                LstSecBeat.SelectedIndex = -1;
                LstSecPrsn.SelectedIndex = -1;
                getdist();
            }
            else if (LstType.SelectedIndex == 2)
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }
                trview_TreeNodeCheckChanged(trview, null);
                lstPrmAreaBox.SelectedIndex = -1;
                lstPrmPrsn.SelectedIndex = -1;
                getretl();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Type');", true);
            }
        }

        private void CheckItems(TreeNode node)
        {
            node.Checked = false;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                childNode.Checked = false;
                CheckItems(childNode);
            }
        }

        public void getsales()
        {

            string smIDStr = "", smIDStr1 = "", Qrychk = "", matBeatNew = "";
            DataSet dataSet = new DataSet("Sales Person Business Report");
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            Qrychk = "(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

            if (smIDStr1 != "")
            {
                string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtdesig.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                    }
                }

                gvdt.Columns.Add(new DataColumn("ReportPerson", typeof(String)));
                gvdt.Columns.Add(new DataColumn("EmpName", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Amt", typeof(String)));
                gvdt.Columns.Add(new DataColumn("NewOutlet", typeof(String)));
                gvdt.Columns.Add(new DataColumn("TotCallR", typeof(String)));
                gvdt.Columns.Add(new DataColumn("TotCallE", typeof(String)));
                gvdt.Columns.Add(new DataColumn("ProdCallsR", typeof(String)));
                gvdt.Columns.Add(new DataColumn("ProdCallsE", typeof(String)));


                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    TopRetailer.DataSource = new DataTable();
                    TopRetailer.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                string query = "SELECT (max(cp1.SMName)+'-'+max(cast(ISnull(cp1.SyncId,'') as varchar))) [ReportPerson],Max(cp.SMID) [SMID],Max(ISnull(cp.SyncId,'')) [SyncId], cp.SmName [EmpName], sum([Amt]) [Amt], sum([NewParties]) [NewParties], sum([TotCallR]) [TotCallR],SUM([TotCallE])[TotCallE], sum([ProdCallsR]) [ProdCallsR],sum([ProdCallsE]) [ProdCallsE] FROM (" +
                              "select om.smid,sum(Amount) as Amt,0 [NewParties], 0 [TotCallR],0 [ProdCallsR],0 [TotCallE], 0 [ProdCallsE] from Transorder1 om where smid in " + Qrychk + " and vDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' group by om.smid " +
                              " UNION ALL select ms.SMId,0 as Amt,count(p.partyid) as [NewParties], 0 [TotCallR], 0 [ProdCallsR],0 [TotCallE], 0 [ProdCallsE] from MastParty p LEFT JOIN mastlogin ml ON p.Created_User_id=ml.Id LEFT JOIN mastsalesrep ms ON ms.UserId=ml.Id where p.Created_Date  BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59'and p.Created_User_Id in (SELECT ml.Id FROM MastSalesRep ms LEFT JOIN mastlogin ml ON ms.UserId=ml.Id WHERE ms.SMId IN " + Qrychk + ") group by ms.SMId UNION ALL  " +
                              " SELECT fv.smid [ConPer_Id],0 [Amt],0 [NewParties], count(fv.FvId) [TotCallR], 0 [ProdCallsR],0 [TotCallE],0 [ProdCallsE] FROM TransFailedVisit fv LEFT JOIN MastParty p ON fv.PartyId = p.PartyId LEFT JOIN MastArea on p.AreaId=MastArea.AreaId where fv.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59'and fv.smid in " + Qrychk + " GROUP BY fv.smid UNION ALL " +
                              " SELECT d.smid  [ConPer_Id],0 [Amt],0 [NewParties],count(d.DemoId) [TotCallR], 0 [ProdCallsR],0 [TotCallE],0 [ProdCallsE] FROM TransDemo d LEFT JOIN MastParty p ON d.PartyId = p.PartyId left JOIN MastArea on p.AreaId=MastArea.AreaId where d.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' and d.smid in " + Qrychk + " GROUP BY d.smid union all " +
                              " SELECT om.smid, 0 [Amt], 0 [NewParties],case when (count(om.OrdDocId))>1 then 1 else 1 end [TotCallR],0 [ProdCallsR],0 [TotCallE],0 [ProdCallsE] from((Transorder om LEFT JOIN mastparty p ON om.partyid = p.partyid))left JOIN mastArea on p.AreaId=mastArea.AreaId where om.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' and om.smid in " + Qrychk + " GROUP BY om.smid,om.areaid,mastArea.AreaName,om.partyid,om.VDate UNION ALL " +
                              " SELECT om.smid, 0 [Amt], 0 [NewParties],0 [TotCallR],case when (count(om.OrdDocId))>1 then 1 else 1 end [ProdCallsR],0 [TotCallE],0 [ProdCallsE] from((Transorder om LEFT JOIN MastParty p ON om.PartyId = p.PartyId))left JOIN MastArea on p.AreaId=MastArea.AreaId where om.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' and om.smid in " + Qrychk + " GROUP BY om.smid,om.AreaId,MastArea.AreaName,om.partyid,om.VDate ) t " +
                              " LEFT JOIN Mastsalesrep cp ON t.smid = cp.smid left join MastSalesRep cp1 on cp1.SMId=cp.UnderId GROUP BY cp.smname ORDER BY [Amt] DESC, [EmpName]";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                DataView dvSales = new DataView(dtItem);
                foreach (DataRow drvst in dtItem.Rows)
                {
                    dvSales.RowFilter = "SMID=" + drvst["SMID"].ToString();
                    if (dvSales.ToTable().Rows.Count > 0)
                    {
                        DataTable dtsp = dvSales.ToTable();
                        DataRow dr = dtsp.Rows[0];
                        DataRow mDataRow = gvdt.NewRow();

                        str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMID"].ToString() + "  and maingrp<>" + dr["SMID"].ToString() + ")";
                        DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtsr.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtsr.Rows.Count; j++)
                            {
                                if (dtsr.Rows[j]["DesName"].ToString() != "")
                                {
                                    mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString() + "-" + dtsr.Rows[j]["SyncId"].ToString();
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

                        mDataRow["ReportPerson"] = dr["ReportPerson"].ToString();
                        mDataRow["EmpName"] = dr["EmpName"].ToString() + "-" + dr["SyncId"].ToString();
                        mDataRow["Amt"] = dr["Amt"].ToString();
                        mDataRow["NewOutlet"] = dr["NewParties"].ToString();
                        mDataRow["TotCallR"] = dr["TotCallR"].ToString();
                        mDataRow["TotCallE"] = dr["TotCallE"].ToString();
                        mDataRow["ProdCallsR"] = dr["ProdCallsR"].ToString();
                        mDataRow["ProdCallsE"] = dr["ProdCallsE"].ToString();

                        dvSales.RowFilter = null;
                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();
                    }
                }
                dataSet.Tables.Add(gvdt);

                try
                {
                    Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "SM Business Report.xlsx";

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
                        xlWorksheet.Name = "SM Business Report";

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
                    xlWorkbook.SaveAs(strPath);
                    xlWorkbook.Close();
                    ExcelApp.Quit();
                    Response.ContentType = "Application/xlsx";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.TransmitFile(strPath);
                    Response.End();

                    //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                    //ExcelApp.Visible = true;
                }
                catch (Exception ex)
                {

                }

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record File Generated Successfully');", true);

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                TopRetailer.DataSource = null;
                TopRetailer.DataBind();
            }
            spinner.Visible = false;
        }

        public void getdist()
        {
            spinner.Visible = true;
            DataSet dataSet = new DataSet("Distributor Business Report");
            DataTable dt1 = new DataTable();
            string distrb = "", Qry = "", Qry1 = "", beat = "", area = "", itm = "";

            // Items collection
            foreach (ListItem item in lstPrmAreaBox.Items)
            {
                if (item.Selected)
                {
                    area += item.Value + ",";
                }
            }
            area = area.TrimEnd(',');

            foreach (ListItem item in lstPrmPrsn.Items)
            {
                if (item.Selected)
                {
                    distrb += item.Value + ",";
                }
            }
            distrb = distrb.TrimEnd(',');

            if (distrb != "" && distrb != "0")
            {
                Qry = Qry + " and srp.PartyId in (" + distrb + ")";
                Qry1 = Qry1 + " and Tpr.DistId in (" + distrb + ")";
            }

            //if (distrb != "" && distrb != "0")
            //    Qry = Qry + " and srp.SMId in (" + distrb + ")";
            if (area != "")
            {
                string sql = "  select (Max(msp.AreaName)+'-'+Max(Isnull(msp.SyncId,''))) AS StateName,(ma1.AreaName+'-'+Max(Isnull(ma1.SyncId,''))) AS City,(mar.AreaName+'-'+Max(Isnull(mar.SyncId,''))) AS AreaName,(srp.PartyName+'-'+Max(Isnull(srp.SyncId,''))) AS PartyName, Sum(Tpr.OrderValue) as Amt from [TransPurchOrder] Tpr Left join MastParty srp on Tpr.DistId=srp.PartyId left join MastArea mar on mar.AreaId=srp.AreaId left join mastarea ma1 on ma1.areaid = srp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId where mar.AreaId in (" + area + ") " + Qry + " and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and Tpr.OrderValue is not null Group by srp.PartyName,mar.AreaName,ma1.AreaName";

                DataTable dt = new DataTable("Total Amount");
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                //if (dt.Rows.Count > 0)
                //{
                string qry = "SELECT STUFF( (SELECT ', ' +quotename(t2.COL) FROM ( select (Itemname+'-'+Isnull(SyncId,'')) AS COL from MastItem ms where ms.ItemId in ( select DISTINCT Tpr.ItemId from [TransPurchOrder1] Tpr where Tpr.DistId in (select PartyId from MastParty where AreaId in (" + area + ") and Active=1 and PartyDist=1 " + Qry1 + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "')) t2 FOR XML PATH ('')), 1, 1, '') as name";
                itm = DbConnectionDAL.GetStringScalarVal(qry);

                string str = "   select * from( select (Max(msp.AreaName)+'-'+Max(ISNULL(msp.SyncId,''))) AS StateName,(ma1.AreaName+'-'+Max(ISNULL(ma1.SyncId,''))) AS City,(mar.AreaName+'-'+Max(ISNULL(mar.SyncId,''))) AS AreaName,(srp.PartyName+'-'+Max(ISNULL(srp.SyncId,''))) AS PartyName,Tpr.ItemId,(max(mat.ItemName)+'-'+Max(ISNULL(mat.SyncId,''))) as Item, convert(numeric(18, 2), sum(Tpr.Qty)) AS qty from [TransPurchOrder1] Tpr Left join MastParty srp on Tpr.DistId=srp.PartyId left join MastArea mar on mar.AreaId=srp.AreaId left join mastarea ma1 on ma1.areaid = srp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId left join MastItem mat on tpr.ItemId= mat.ItemId WHERE mat.ItemName is not null and mar.AreaId in  (" + area + ") " + Qry + " and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' Group by srp.PartyName,mar.AreaName,ma1.AreaName,tpr.ItemId) s PIVOT (SUM(qty) FOR Item in (" + itm + ")) as PivotTable";
                DataTable dt2 = new DataTable("Quantity wise Item");
                dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                string str1 = " select * from( select (Max(msp.AreaName)+'-'+Max(ISNULL(msp.SyncId,''))) AS StateName,(ma1.AreaName+'-'+Max(ISNULL(ma1.SyncId,''))) AS City,(mar.AreaName+'-'+Max(ISNULL(mar.SyncId,''))) AS AreaName,(srp.PartyName+'-'+Max(ISNULL(srp.SyncId,''))) AS PartyName,Tpr.ItemId,(max(mat.ItemName)+'-'+Max(ISNULL(mat.SyncId,''))) as Item, convert(numeric(18, 2), sum((Tpr.Qty) * (Tpr.Rate))) AS Amo from [TransPurchOrder1] Tpr Left join MastParty srp on Tpr.DistId=srp.PartyId left join MastArea mar on mar.AreaId=srp.AreaId left join mastarea ma1 on ma1.areaid = srp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId left join MastItem mat on tpr.ItemId= mat.ItemId WHERE  mat.ItemName is not null and mar.AreaId in (" + area + ") " + Qry + " and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' Group by srp.PartyName,mar.AreaName,ma1.AreaName,tpr.ItemId) s PIVOT (SUM(Amo) FOR Item in (" + itm + ")) as PivotTable";
                DataTable dt3 = new DataTable("Amount wise Item");
                dt3 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                dataSet.Tables.Add(dt);
                dataSet.Tables.Add(dt2);
                dataSet.Tables.Add(dt3);
                try
                {
                    Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "Distributor Business Report.xlsx";

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

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('" + collection[i - 1] + "');", true);
                        xlWorksheet.Name = table.TableName;
                        if (i - 1 == 2)
                        {
                            xlWorksheet.Name = "Amount wise Item";
                        }
                        else if (i - 1 == 1)
                        {
                            xlWorksheet.Name = "Qty wise Item";
                        }
                        else if (i - 1 == 0)
                        {
                            xlWorksheet.Name = "Total Amount";
                        }

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
                //}
                //else
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Recod Found');", true);
                //    //leavereportrpt.DataSource = null;
                //    //leavereportrpt.DataBind();
                //}
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please Select Area');", true);
                //leavereportrpt.DataSource = null;
                //leavereportrpt.DataBind();
            }
            spinner.Visible = false;
        }

        public void getretl()
        {
            spinner.Visible = true;
            DataSet dataSet = new DataSet("Retailer Business Report");

            DataTable dt1 = new DataTable();


            string retailer = "", Qry = "", Qry1 = "", Qry2 = "", beat = "", area = "", itm = "";

            // Items collection
            foreach (ListItem item in LstSecArea.Items)
            {
                if (item.Selected)
                {
                    area += item.Value + ",";
                }
            }
            area = area.TrimEnd(',');

            foreach (ListItem item in LstSecBeat.Items)
            {
                if (item.Selected)
                {
                    beat += item.Value + ",";
                }
            }
            beat = beat.TrimEnd(',');

            foreach (ListItem item in LstSecPrsn.Items)
            {
                if (item.Selected)
                {
                    retailer += item.Value + ",";
                }
            }
            retailer = retailer.TrimEnd(',');

            if (beat != "" && beat != "0")
            {
                Qry = Qry + " and bt.AreaId in (" + beat + ")";
                Qry1 = Qry1 + " and BeatId in (" + beat + ")";
                Qry2 = Qry1 + " and mp.BeatId in (" + beat + ")";
            }

            if (retailer != "" && retailer != "0")
            {
                Qry = Qry + " and srp.PartyId in (" + retailer + ")";
                Qry1 = Qry1 + " and PartyId in (" + retailer + ")";
                Qry2 = Qry1 + " and os.PartyId in (" + retailer + ")";
            }

            if (area != "")
            {
                //if (retailer != "" && retailer != "0")
                //    Qry = Qry + " and srp.SMId in (" + distrb + ")";

                string sql = "select * from (select (Max(msp.AreaName)+'-'+Max(msp.SyncId)) as StateName,(Max(ma1.AreaName)+'-'+Max(ma1.SyncId)) as City,(Max(arp.AreaName)+'-'+Max(arp.SyncId)) as Area,(Max(bt.AreaName)+'-'+Max(bt.SyncId)) as Beat, (srp.PartyName+'-'+cast(Max(IsNull(srp.SyncId,'')) as varchar)) As Outlet,Max(srp.Mobile) AS Mobile, Sum(Tpr.OrderAmount) as Amt from TransOrder Tpr Left join MastParty srp on Tpr.PartyId=srp.PartyId Left join MastArea arp on srp.AreaId=arp.AreaId Left join MastArea bt on srp.BeatId=bt.AreaId left join mastarea ma1 on ma1.areaid = srp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId where  Tpr.AreaId in (" + area + ") " + Qry + " and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and Tpr.OrderAmount is not null Group by srp.PartyName" +

                    " UNION select (Max(msp.AreaName)+'-'+Max(msp.SyncId)) as StateName,(Max(ma1.AreaName)+'-'+Max(ma1.SyncId)) as City,(Max(arp.AreaName)+'-'+Max(arp.SyncId)) as Area,(Max(bt.AreaName)+'-'+Max(bt.SyncId)) as Beat, (srp.PartyName+'-'+cast(Max(IsNull(srp.SyncId,'')) as varchar)) As Outlet,Max(srp.Mobile) AS Mobile, Sum(Tpr.OrderAmount) as Amt from Temp_TransOrder Tpr Left join MastParty srp on Tpr.PartyId = srp.PartyId Left join MastArea arp on srp.AreaId = arp.AreaId Left join MastArea bt on srp.BeatId = bt.AreaId left join mastarea ma1 on ma1.areaid = srp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId where Tpr.AreaId in (" + area + ") " + Qry + " and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and Tpr.OrderAmount is not null Group by srp.PartyName) s";
                DataTable dt = new DataTable("Total Amount");
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                //if (dt.Rows.Count > 0)
                //{
                DataTable ItemNames = new DataTable();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{

                //    ItemNames = DbConnectionDAL.GetDataTable(CommandType.Text, "Select ItemName from MastItem where ItemId in (" + dtProducts.Rows[i]["itemid"].ToString() + ") ");

                //    inames = inames + ",[" + ItemNames.Rows[0]["ItemName"].ToString() + "]";
                //    i_id = i_id + "," + dtProducts.Rows[i]["itemid"].ToString();
                //}

                string qry = "SELECT STUFF( (SELECT distinct ', ' +quotename(t2.COL) FROM (select distinct  (Itemname+'-'+SyncId) as COL from MastItem ms where ms.ItemId in ( select DISTINCT Tpr.ItemId from TransOrder1 Tpr where Tpr.PartyId in (select PartyId from MastParty where AreaId in(" + area + ") and Active=1 and PartyDist=0 " + Qry1 + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') Union select distinct  (Itemname+'-'+SyncId) as COL from MastItem ms where ms.ItemId in ( select DISTINCT Tpr.ItemId from Temp_TransOrder1 Tpr where Tpr.PartyId in (select PartyId from MastParty where AreaId in(" + area + ") and Active=1 and PartyDist=0 " + Qry1 + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "')) t2 FOR XML PATH ('')), 1, 1, '') as name";
                itm = DbConnectionDAL.GetStringScalarVal(qry);

                string str = "  select * from (SELECT (Max(msp.AreaName)+'-'+Max(Isnull(msp.SyncId,''))) as StateName,(ma1.AreaName+'-'+Max(Isnull(ma1.SyncId,''))) as City, (ma.AreaName+'-'+Max(Isnull(ma.SyncId,''))) as Area, (ma2.AreaName+'-'+Max(Isnull(ma2.SyncId,''))) as Beat,(mp.PartyName+'-'+Max(Isnull(mp.SyncId,''))) As Outlet, mp.Mobile,(mi.itemname+'-'+Max(Isnull(mi.SyncId,''))) as itemname, convert(numeric(18, 2), sum(os.Qty)) AS qty   FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId = mp.PartyId left join mastarea ma on ma.areaid = mp.areaid left join mastarea ma1 on ma1.areaid = mp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId left join mastarea ma2 on ma2.areaid = mp.beatid  left join mastitem mi on mi.itemid = os.Itemid WHERE os.ItemId in (select DISTINCT ItemId from TransOrder1 where AreaId in  (" + area + ")) and os.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' AND mp.PartyDist = 0 and os.AreaId in (" + area + ") " + Qry2 + "  GROUP BY mp.PartyName,mi.itemname,mp.Mobile,ma1.AreaName,ma.AreaName,ma2.AreaName" +

                    " Union ALl SELECT (Max(msp.AreaName)+'-'+Max(Isnull(msp.SyncId,''))) as StateName,(ma1.AreaName+'-'+Max(Isnull(ma1.SyncId,''))) as City, (ma.AreaName+'-'+Max(Isnull(ma.SyncId,''))) as Area, (ma2.AreaName+'-'+Max(Isnull(ma2.SyncId,''))) as Beat,(mp.PartyName+'-'+Max(Isnull(mp.SyncId,''))) As Outlet,mp.Mobile,(mi.itemname+'-'+Max(Isnull(mi.SyncId,''))) as itemname,convert(int, sum(os.Qty)) AS qty   FROM Temp_TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId = mp.PartyId left join mastarea ma on ma.areaid = mp.areaid left join mastarea ma1 on ma1.areaid = mp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId left join mastarea ma2 on ma2.areaid = mp.beatid left join mastitem mi on mi.itemid = os.Itemid WHERE os.ItemId in (select DISTINCT ItemId from Temp_TransOrder1 where AreaId in  (" + area + "))  and os.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' AND mp.PartyDist = 0 and os.AreaId in (" + area + ") " + Qry2 + " GROUP BY mp.PartyName,mi.itemname,mp.Mobile,ma1.AreaName,ma.AreaName,ma2.AreaName) s PIVOT (SUM(qty) FOR itemname in (" + itm + ")) as PivotTable";
                DataTable dt2 = new DataTable("Quantity wise Item");
                dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                string str1 = " select * from (SELECT (Max(msp.AreaName)+'-'+Max(Isnull(msp.SyncId,''))) as StateName,(ma1.AreaName+'-'+Max(Isnull(ma1.SyncId,''))) as City, (ma.AreaName+'-'+Max(Isnull(ma.SyncId,''))) as Area, (ma2.AreaName+'-'+Max(Isnull(ma2.SyncId,''))) as Beat,(mp.PartyName+'-'+Max(Isnull(mp.SyncId,''))) As Outlet, mp.Mobile,(mi.itemname+'-'+Max(Isnull(mi.SyncId,''))) as itemname, convert(numeric(18, 2), sum(os.amount)) AS Amo  FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId = mp.PartyId left join mastarea ma on ma.areaid = mp.areaid left join mastarea ma1 on ma1.areaid = mp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId left join mastarea ma2 on ma2.areaid = mp.beatid left join mastitem mi on mi.itemid = os.Itemid WHERE os.ItemId in (select DISTINCT ItemId from TransOrder1 where AreaId in  (" + area + ")) and os.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' AND mp.PartyDist = 0 and os.AreaId in (" + area + ") " + Qry2 + "  GROUP BY mp.PartyName,mi.itemname,mp.Mobile,ma1.AreaName,ma.AreaName,ma2.AreaName" +
                    " Union ALl SELECT (Max(msp.AreaName)+'-'+Max(Isnull(msp.SyncId,''))) as StateName,(ma1.AreaName+'-'+Max(Isnull(ma1.SyncId,''))) as City, (ma.AreaName+'-'+Max(Isnull(ma.SyncId,''))) as Area, (ma2.AreaName+'-'+Max(Isnull(ma2.SyncId,''))) as Beat,(mp.PartyName+'-'+Max(Isnull(mp.SyncId,''))) As Outlet,mp.Mobile,(mi.itemname+'-'+Max(Isnull(mi.SyncId,''))) as itemname,convert(int, sum(os.amount)) AS Amo  FROM Temp_TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId = mp.PartyId left join mastarea ma on ma.areaid = mp.areaid left join mastarea ma1 on ma1.areaid = mp.cityid left join MastArea msp on msp.AreaId=ma1.UnderId left join mastarea ma2 on ma2.areaid = mp.beatid left join mastitem mi on mi.itemid = os.Itemid WHERE os.ItemId in (select DISTINCT ItemId from TransOrder1 where AreaId in  (" + area + "))  and os.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' AND mp.PartyDist = 0 and os.AreaId in (" + area + ") " + Qry2 + "  GROUP BY mp.PartyName,mi.itemname,mp.Mobile,ma1.AreaName,ma.AreaName,ma2.AreaName) s PIVOT (SUM(Amo) FOR itemname in (" + itm + ")) as PivotTable";
                DataTable dt3 = new DataTable("Amount wise Item");
                dt3 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                dataSet.Tables.Add(dt);
                dataSet.Tables.Add(dt2);
                dataSet.Tables.Add(dt3);
                try
                {
                    Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "Retailer Business Report.xlsx";

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
                        xlWorksheet.Name = table.TableName;

                        if (i - 1 == 2)
                        {
                            xlWorksheet.Name = "Amount wise Item";
                        }
                        else if (i - 1 == 1)
                        {
                            xlWorksheet.Name = "Qty wise Item";
                        }
                        else if (i - 1 == 0)
                        {
                            xlWorksheet.Name = "Total Amount";
                        }

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
                //}
                //else
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please Select Retailer');", true);
                //    //leavereportrpt.DataSource = null;
                //    //leavereportrpt.DataBind();
                //}
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please Select Area');", true);
                //leavereportrpt.DataSource = null;
                //leavereportrpt.DataBind();
            }
            spinner.Visible = false;
        }

        //public void itmretlqty()
        //{
        //    DataSet dataSet = new DataSet("Retailer Business Report Item Qty");
        //    DataTable dt = new DataTable();
        //    DataTable dt1 = new DataTable();
        //    string retailer = "", Qry = "", beat = "", area = "", itm = "";

        //    foreach (ListItem item in LstSecPrsn.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            retailer += item.Value + ",";
        //        }
        //    }
        //    retailer = retailer.TrimEnd(',');

        //    if (retailer != "")
        //    {
        //        string qry = "select Itemname from MastItem ms where ms.ItemId in ( select DISTINCT Tpr.ItemId from TransOrder1 Tpr where Tpr.PartyId in (" + retailer + ") and Tpr.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "')";
        //        dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                itm += "[" + row["Itemname"].ToString() + "]" + ",";
        //            }
        //            itm = itm.TrimEnd(',');
        //            //string[] columnNames = dt.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

        //            string str = " select * from (SELECT mi.itemname, convert(numeric(18, 2), sum(os.Qty)) AS qty, mp.PartyName, mp.Email, mp.Mobile, mp.ContactPerson, ma1.AreaName as City, ma.AreaName as Area, ma2.AreaName as Beat FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId = mp.PartyId left join mastarea ma on ma.areaid = mp.areaid left join mastarea ma1 on ma1.areaid = mp.cityid left join mastarea ma2 on ma2.areaid = mp.beatid left join mastitem mi on mi.itemid = os.Itemid WHERE os.ItemId in (select DISTINCT ItemId from TransOrder1 where PartyId in (" + retailer + ")) and os.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' AND mp.PartyDist = 0 and os.PartyId in (" + retailer + ") GROUP BY mp.PartyName,mi.itemname,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName Union ALl SELECT TOP 99 mi.itemname,convert(int, sum(os.Qty)) AS qty, mp.PartyName,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat  FROM Temp_TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId = mp.PartyId left join mastarea ma on ma.areaid = mp.areaid left join mastarea ma1 on ma1.areaid = mp.cityid left join mastarea ma2 on ma2.areaid = mp.beatid left join mastitem mi on mi.itemid = os.Itemid WHERE os.ItemId in (select DISTINCT ItemId from TransOrder1 where PartyId in (" + retailer + "))  and os.VDate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' AND mp.PartyDist = 0 and os.PartyId in (" + retailer + ")  GROUP BY mp.PartyName,mi.itemname,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName) s PIVOT (SUM(qty) FOR itemname in (" + itm + ")) as PivotTable";

        //            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //            if (dt1.Rows.Count > 0)
        //            {
        //            }
        //        }
        //    }
        //}

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptBuisnessReport_V2.aspx");
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
                ViewState["tree"] = smiMStr;
            }

            cnt = cnt + 1;
            return;
        }

        private void BindStaffChkList()
        {
            try
            {


                ListBox lstyr = new ListBox();
                for (int i = 1; i <= 3; i++)
                {
                    DataRow row = YrTable.NewRow();
                    row["TypId"] = i.ToString();
                    if (i == 1)
                    {
                        row["TypName"] = "Sales Person";
                    }
                    else if (i == 2)
                    {
                        row["TypName"] = "Primary";
                    }
                    else if (i == 3)
                    {
                        row["TypName"] = "Secondary";
                    }
                    YrTable.Rows.Add(row);
                }
                LstType.DataSource = YrTable;
                LstType.DataTextField = "TypName";
                LstType.DataValueField = "TypId";
                LstType.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BuisnessReport.csv");
            string headertext = "Employee name".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"') + "," + "New Parties".TrimStart('"').TrimEnd('"') + "," + "Total Call".TrimStart('"').TrimEnd('"') + "," + "Productive Call".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);


            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("EmpName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amt", typeof(String)));
            dtParams.Columns.Add(new DataColumn("NewParties", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotCallR", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProdCallsR", typeof(string)));


            foreach (RepeaterItem item in TopRetailer.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblEmpName = item.FindControl("lblEmpName") as Label;
                dr["EmpName"] = lblEmpName.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblAmt = item.FindControl("lblAmt") as Label;
                dr["Amt"] = lblAmt.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblNewParties = item.FindControl("lblNewParties") as Label;
                dr["NewParties"] = lblNewParties.Text.ToString();
                Label lblTotCallR = item.FindControl("lblTotCallR") as Label;
                dr["TotCallR"] = lblTotCallR.Text.ToString();
                Label lblProdCallsR = item.FindControl("lblProdCallsR") as Label;
                dr["ProdCallsR"] = lblProdCallsR.Text.ToString();
                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        string h = dtParams.Rows[j][k].ToString();
                        string d = h.Replace(",", " ");
                        dtParams.Rows[j][k] = "";
                        dtParams.Rows[j][k] = d;
                        dtParams.AcceptChanges();
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 0)
                        {
                            //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }
                        else
                        {
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BuisnessReport.csv");
            Response.Write(sb.ToString());
            Response.End();

        }

        protected void LstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstType.SelectedIndex == 0)
            {
                lstPrmAreaBox.SelectedIndex = -1;
                lstPrmAreaBox.SelectedIndex = -1;
                lstPrmPrsn.SelectedIndex = -1;

                trvw.Visible = true;
                rwprm.Visible = false;
                rwsec.Visible = false;
            }
            else if (LstType.SelectedIndex == 1)
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }

                LstSecArea.SelectedIndex = -1;
                LstSecBeat.SelectedIndex = -1;
                LstSecPrsn.SelectedIndex = -1;

                trvw.Visible = false;
                rwprm.Visible = true;
                rwsec.Visible = false;
            }
            else if (LstType.SelectedIndex == 2)
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }
                trview_TreeNodeCheckChanged(trview, null);
                lstPrmAreaBox.SelectedIndex = -1;
                lstPrmPrsn.SelectedIndex = -1;

                trvw.Visible = false;
                rwprm.Visible = false;
                rwsec.Visible = true;
            }
        }

        protected void lstPrmAreaBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string areastr = "";
                //         string message = "";
                foreach (ListItem areagrp in lstPrmAreaBox.Items)
                {
                    if (areagrp.Selected)
                    {
                        areastr += areagrp.Value + ",";
                    }
                }
                areastr = areastr.TrimStart(',').TrimEnd(',');
                if (areastr != "")
                {
                    string beatqry = @" select PartyId,(PartyName+'-'+Mobile) AS partyName from MastParty where AreaId in(" + areastr + ") and Active=1 and PartyDist=1  order by PartyName";
                    DataTable dtbeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatqry);
                    if (dtbeat.Rows.Count > 0)
                    {
                        lstPrmPrsn.DataSource = dtbeat;
                        lstPrmPrsn.DataTextField = "PartyName";
                        lstPrmPrsn.DataValueField = "PartyId";
                        lstPrmPrsn.DataBind();
                    }
                }
                else
                {
                    lstPrmPrsn.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void LstSecArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string areastr = "";
                //         string message = "";
                foreach (ListItem areagrp in LstSecArea.Items)
                {
                    if (areagrp.Selected)
                    {
                        areastr += areagrp.Value + ",";
                    }
                }
                areastr = areastr.TrimStart(',').TrimEnd(',');
                if (areastr != "")
                {
                    string beatqry = @"select AreaId,AreaName from mastarea where Underid in ( " + areastr + " ) and areatype='Beat' and Active=1 order by AreaName";
                    DataTable dtbeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatqry);
                    if (dtbeat.Rows.Count > 0)
                    {
                        LstSecBeat.DataSource = dtbeat;
                        LstSecBeat.DataTextField = "AreaName";
                        LstSecBeat.DataValueField = "AreaId";
                        LstSecBeat.DataBind();
                    }
                }
                else
                {
                    LstSecBeat.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void LstSecBeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strparty = string.Empty;
            foreach (ListItem party in LstSecBeat.Items)
            {
                if (party.Selected)
                {
                    strparty += party.Value + ",";
                }
            }
            strparty = strparty.TrimStart(',').TrimEnd(',');
            if (strparty != "")
            {
                string str = @"select PartyId,(partyName+'-'+Mobile) AS partyName from Mastparty where Beatid in (" + strparty + ") and partydist=0 and active=1 order by PartyName asc";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    LstSecPrsn.DataSource = dt;
                    LstSecPrsn.DataTextField = "partyName";
                    LstSecPrsn.DataValueField = "PartyId";
                    LstSecPrsn.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                LstSecPrsn.Items.Clear();
            }
        }

        protected void btnGoDist_Click(object sender, EventArgs e)
        {

        }

        protected void btnGoRet_Click(object sender, EventArgs e)
        {

        }
    }
}