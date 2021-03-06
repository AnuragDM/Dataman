using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using System.Web.Services;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class DIstInvRep_V2 : System.Web.UI.Page
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
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                List<DistributorsInvoice> distributorsinvoice = new List<DistributorsInvoice>();
                distributorsinvoice.Add(new DistributorsInvoice());
                distreportrpt.DataSource = distributorsinvoice;
                distreportrpt.DataBind();
                //Ankita - 13/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalePersonDDl();
                //fill_TreeArea();
                BindTreeViewControl();
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
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

        public class DistributorsInvoice
        {
            public string VDate { get; set; }
            public string PartyId { get; set; }
            public string PartyName { get; set; }
            public string BranchName { get; set; }
            public string Amount { get; set; }
        }

        //[WebMethod(EnableSession = true)]
        public void GetDistributorInvice()
        {
            try
            {
                DataSet dataSet = new DataSet("Distributor Invoice Report");
                String smIDStr1 = "", dist = "", prdgrp = "", prd = "";
                string qry = "";
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

                if (dist != "")
                {
                    qry = "and tdv1.DistId in(" + dist + ")";

                }
                if (smIDStr1 != "")
                {
                    string Qrychk = " tdv1.VDate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

                    //            string query = @"select tdv.DistInvDocId,mp.SyncId AS PartyId,mp.PartyName,Convert(varchar(15),CONVERT(date,tdv.VDate,103),106) AS VDate,Sum(tdv1.Amount) as Amount,
                    //                           mrc.ResCenName as BranchName from TransDistInv tdv Left join TransDistInv1 tdv1 on tdv.DistInvId=tdv1.DistInvId and tdv.DistInvDocId=tdv1.DistInvDocId LEFT JOIN mastparty mp 
                    //                           ON mp.PartyId=tdv.DistId left join MastResCentre mrc on mrc.ResCenId=tdv1.LocationID where " + Qrychk + " AND mp.PartyDist=1 and tdv.DistId in  (" + Distid + ")  group by tdv.DistInvDocId,mp.SyncId,mp.PartyName,tdv.VDate, mrc.ResCenName ORDER BY tdv.VDate desc ";

                    string query = @"select tdv1.DistInvDocId,max(mss.AreaName)+'-'+max(Isnull(mss.SyncId,'')) as State,max(msc.AreaName)+'-'+max(Isnull(msc.SyncId,'')) as City,max(msa.AreaName)+'-'+max(Isnull(msa.SyncId,''))  as Area,mp.PartyName+'-'+MAX(Isnull(mp.SyncId,'')) as Distributor,Convert(varchar(15),CONVERT(date,tdv1.VDate,103),106) AS VDate,Sum(tdv1.Amount) as Amount,mrc.ResCenName+'-'+MAX(Isnull(mrc.SyncId,'')) as BranchName from TransDistInv1 tdv1 LEFT JOIN mastparty mp ON mp.PartyId=tdv1.DistId left join MastResCentre mrc on mrc.ResCenId=tdv1.LocationID Left Join MastArea msa on msa.AreaId=mp.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId left join MastSalesRep msp on msp.SMId=mp.SMID where " + Qrychk + " AND mp.PartyDist=1 and mp.SMID in  (" + smIDStr1 + ") " + qry + "  group by tdv1.DistInvDocId,mp.PartyName,tdv1.VDate, mrc.ResCenName ORDER BY tdv1.VDate desc ";

                    DataTable dtInvoice = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    dataSet.Tables.Add(dtInvoice);

                    try
                    {
                        Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                        string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                        if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = "Distributor Invoice Report.xlsx";

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

                            xlWorksheet.Name = "Dist Invoice Sale";

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
                    dtInvoice.Dispose();
                    dataSet.Dispose();
                    spinner.Visible = false;
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Sales Person');", true);
                }
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
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');
                    //string distqry = @"select * from MastParty where CityId in (" + citystr + ") and PartyDist=1 and Active=1 order by PartyName";
                    string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
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
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    ListBox1.Items.Clear();
                    ListBox1.DataBind();
                    distreportrpt.DataSource = null;
                    distreportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindSalePersonDDl()
        {
            try
            {
                if (roleType == "Admin")
                { //Ankita - 13/may/2016- (For Optimization)
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
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
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        void fill_TreeArea()
        {
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {
                //string strrole = "select SMID,SMName from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                //St = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                //    lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,1);
                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {
                //Ankita - 17/may/2016- (For Optimization)
                //lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID, lowestlvl);
                St = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT mastrole.rolename,mastsalesrep.smid,smname + ' (' + mastsalesrep.syncid + ' - '+ mastrole.rolename + ')' AS smname, mastsalesrep.lvl,mastrole.roletype FROM   mastsalesrep LEFT JOIN mastrole ON mastrole.roleid = mastsalesrep.roleid WHERE  smid =" + Settings.Instance.SMID + "");
            }


            trview.Nodes.Clear();

            if (St.Rows.Count <= 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found !');", true);
                return;
            }
            foreach (DataRow row in St.Rows)
            {
                TreeNode tnParent = new TreeNode();
                tnParent.Text = row["SMName"].ToString();
                tnParent.Value = row["SMId"].ToString();
                trview.Nodes.Add(tnParent);
                //tnParent.ExpandAll();
                tnParent.CollapseAll();
                //Ankita - 17/may/2016- (For Optimization)
                //FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
                getchilddata(tnParent, tnParent.Value);
            }
        }
        //Ankita - 17/may/2016- (For Optimization)
        private void getchilddata(TreeNode parent, string ParentId)
        {

            string SmidVar = string.Empty;
            string GetFirstChildData = string.Empty;
            int levelcnt = 0;
            if (Settings.Instance.RoleType == "Admin")
                //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
                levelcnt = Convert.ToInt16("0") + 2;
            else
                levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 1;


            GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 order by SMName,lvl desc ";
            DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

            if (FirstChildDataDt.Rows.Count > 0)
            {

                for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
                {
                    SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
                    FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
                }
                SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

                // for (int j = levelcnt + 1; j <= 5; j++)
                for (int j = levelcnt + 1; j <= 9; j++)
                {
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
                    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
                    if (dtChild.Rows.Count > 0)
                    {
                        SmidVar = string.Empty;
                        int mTotRows = dtChild.Rows.Count;
                        if (mTotRows > 0)
                        {
                            var str = "";
                            for (int k = 0; k < mTotRows; k++)
                            {
                                SmidVar += dtChild.Rows[k]["smid"].ToString() + ",";
                            }

                            TreeNode Oparent = parent;
                            switch (j)
                            {
                                case 3:
                                    if (Oparent.Parent != null)
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    break;
                                case 4:
                                    if (Oparent.Parent != null)
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    break;
                                case 5:
                                    if (Oparent.Parent != null)
                                    {
                                        foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                        {
                                            foreach (TreeNode child in Pchild.ChildNodes)
                                            {
                                                str += child.Value + ","; parent = child;
                                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                for (int l = 0; l < dr.Length; l++)
                                                {
                                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                }
                                                dtChild.Select();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }


                                    break;
                                case 6:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode Qchild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode child in Qchild.ChildNodes)
                                                    {
                                                        str += child.Value + ","; parent = child;
                                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                        for (int l = 0; l < dr.Length; l++)
                                                        {
                                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                        }
                                                        dtChild.Select();
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }
                                    break;
                                case 7:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode schild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode Qchild in schild.ChildNodes)
                                                    {
                                                        foreach (TreeNode child in Qchild.ChildNodes)
                                                        {
                                                            str += child.Value + ","; parent = child;
                                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                            for (int l = 0; l < dr.Length; l++)
                                                            {
                                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                            }
                                                            dtChild.Select();
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }


                                    break;
                                case 8:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode Qchild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode child in Qchild.ChildNodes)
                                                    {
                                                        str += child.Value + ","; parent = child;
                                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                        for (int l = 0; l < dr.Length; l++)
                                                        {
                                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                        }
                                                        dtChild.Select();
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }


                                    break;
                                case 9:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode Qchild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode child in Qchild.ChildNodes)
                                                    {
                                                        str += child.Value + ","; parent = child;
                                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                        for (int l = 0; l < dr.Length; l++)
                                                        {
                                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                        }
                                                        dtChild.Select();
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }

                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }


                                    break;
                            }

                            SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);
                        }
                    }
                }
            }


        }

        public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        {
            TreeNode child = new TreeNode();
            child.Text = SMName;
            child.Value = Smid;
            child.SelectAction = TreeNodeSelectAction.Expand;
            parent.ChildNodes.Add(child);
            child.CollapseAll();
        }
        //public void FillChildArea(TreeNode parent, string ParentId, int LVL, int SMId)
        //{
        //    //var AreaQueryChild = "select * from Mastsalesrep where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " order by SMName,lvl";
        //    var AreaQueryChild = "SELECT SMId,Smname +' ('+ ms.Syncid + ' - ' + mr.RoleName + ')' as smname,Lvl from Mastsalesrep ms LEFT JOIN mastrole mr ON mr.RoleId=ms.RoleId where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " and ms.Active=1 order by SMName,lvl";
        //    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
        //    parent.ChildNodes.Clear();
        //    foreach (DataRow dr in dtChild.Rows)
        //    {
        //        TreeNode child = new TreeNode();
        //        child.Text = dr["SMName"].ToString().Trim();
        //        child.Value = dr["SMId"].ToString().Trim();
        //        child.SelectAction = TreeNodeSelectAction.Expand;
        //        parent.ChildNodes.Add(child);
        //        //child.ExpandAll();
        //        child.CollapseAll();
        //        FillChildArea(child, child.Value, (Convert.ToInt32(dr["Lvl"])), Convert.ToInt32(dr["SMId"].ToString()));
        //    }

        //}
        //private void GetRoleType(string p)
        //{
        //    try
        //    {
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        protected void btnGo_Click(object sender, EventArgs e)
        {
            GetDistributorInvice();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistInvoiceReport.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");
            GetDetails(hdnVisItCode.Value);
        }

        private void GetDetails(string p)
        {
            try
            {
                string detQry = @"select t1.DistInvDocId as Invoice_no,t1.ItemId as Item_Id,i.ItemName as Item,t1.Qty as Qty,t1.Amount as Amount
                                from TransDistInv1 t1 left join MastItem i on i.ItemId=t1.ItemId where DistInvDocId='" + p + "'";
                DataTable dtdetQry = DbConnectionDAL.GetDataTable(CommandType.Text, detQry);
                if (dtdetQry.Rows.Count > 0)
                {
                    detailDiv.Style.Add("display", "block");
                    detailDistrpt.DataSource = dtdetQry;
                    detailDistrpt.DataBind();
                }
                else
                {
                    detailDiv.Style.Add("display", "none");
                    detailDistrpt.DataSource = dtdetQry;
                    detailDistrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
            Response.AddHeader("content-disposition", "attachment;filename=DistributorInvoiceReport.csv");
            string headertext = "Invoice No".TrimStart('"').TrimEnd('"') + "," + "Invoice Dt".TrimStart('"').TrimEnd('"') + "," + "Distributor Id".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Branch Name".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            //DataTable dtParams = new DataTable();
            //dtParams.Columns.Add(new DataColumn("InvoiceNo", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("InvoiceDt", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("DistributorId", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("BranchName", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));

            //foreach (RepeaterItem item in distreportrpt.Items)
            //{
            //    DataRow dr = dtParams.NewRow();
            //    Label DistInvDocIdLabel = item.FindControl("DistInvDocIdLabel") as Label;
            //    dr["InvoiceNo"] = DistInvDocIdLabel.Text;
            //    Label VDateLabel = item.FindControl("VDateLabel") as Label;
            //    dr["InvoiceDt"] = VDateLabel.Text.ToString();
            //    Label PartyIdLabel = item.FindControl("PartyIdLabel") as Label;
            //    dr["DistributorId"] = PartyIdLabel.Text.ToString();
            //    Label PartyNameLabel = item.FindControl("PartyNameLabel") as Label;
            //    dr["DistributorName"] = PartyNameLabel.Text.ToString();

            //    Label BranchNameLabel = item.FindControl("BranchNameLabel") as Label;
            //    dr["BranchName"] = BranchNameLabel.Text.ToString();
            //    Label AmountLabel = item.FindControl("AmountLabel") as Label;
            //    dr["Amount"] = AmountLabel.Text.ToString();                 

            //    dtParams.Rows.Add(dr);
            //}

            string smIDStr1 = "", Qrychk = "", Query = "";
            DataTable dtParams = new DataTable();
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

            Qrychk = " tdv1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and tdv1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            if (smIDStr1 != "")
            {
                if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                    {

                        //                        Query = @"select tdv.DistInvDocId,Convert(varchar(15),CONVERT(date,tdv.VDate,103),106) AS VDate,mp.SyncId AS PartyId,mp.PartyName,mrc.ResCenName as BranchName,
                        //                        Sum(tdv1.Amount) as Amount from TransDistInv tdv Left join TransDistInv1 tdv1 on tdv.DistInvId=tdv1.DistInvId and tdv.DistInvDocId=tdv1.DistInvDocId LEFT JOIN mastparty mp 
                        //                        ON mp.PartyId=tdv.DistId left join MastResCentre mrc on mrc.ResCenId=tdv1.LocationID where " + Qrychk + " AND mp.PartyDist=1 and tdv.DistId in  (" + smIDStr1 + ")  group by tdv.DistInvDocId,mp.SyncId,mp.PartyName,tdv.VDate, mrc.ResCenName ORDER BY tdv.VDate desc ";
                        Query = @"select tdv1.DistInvDocId,Convert(varchar(15),CONVERT(date,tdv1.VDate,103),106) AS VDate,mp.SyncId AS PartyId,mp.PartyName,mrc.ResCenName as BranchName,
                        Sum(tdv1.Amount) as Amount from TransDistInv1 tdv1 LEFT JOIN mastparty mp ON mp.PartyId=tdv1.DistId left join MastResCentre mrc on mrc.ResCenId=tdv1.LocationID
                        where " + Qrychk + " AND mp.PartyDist=1 and tdv1.DistId in  (" + smIDStr1 + ")  group by tdv1.DistInvDocId,mp.SyncId,mp.PartyName,tdv1.VDate, mrc.ResCenName ORDER BY tdv1.VDate desc ";

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
                        if (k == 1)
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
                        if (k == 1)
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
                        if (k == 1 || k == 1)
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
            Response.AddHeader("content-disposition", "attachment;filename=DistributorInvoiceReport.csv");
            Response.Write(sb.ToString());
            Response.End();

            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=DistributorInvoiceReport.xls");
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
                ViewState["tree"] = smiMStr;
            }
            cnt = cnt + 1;
            return;
        }


        protected void ExportCSV(object sender, EventArgs e)
        {
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

            Qrychk = " tdv.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and tdv.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            if (smIDStr1 != "")
            {
                if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                    {

                        //Query = "select t.DistInvDocId as Invoice_No,convert (varchar,t.VDate,106) as Invoice_Date ,t.BillAmount as Invoice_Amt, gr.Bilty_No as GrNo,convert(varchar,gr.Bilty_Date,103) as GrDate,gr.Transpoter_Name as GrTransporterName from TransDistInv t Left Join TransGRData gr on gr.DistId=t.DistId and t.DistInvDocId=gr.Invoice_No where " + Qrychk + " and t.DistId in (" + smIDStr1 + ") ";
                        Query = @"select tdv.DistId,tdv.DistInvDocId,mp.SyncId AS PartyId,mp.PartyName,Convert(varchar(15),CONVERT(date,tdv.VDate,103),106) AS VDate,tdv1.Amount,mrc.ResCenName as BranchName from TransDistInv tdv inner join TransDistInv1 tdv1 on tdv.DistInvDocId=tdv1.DistInvDocId LEFT JOIN mastparty mp ON mp.PartyId=tdv.DistId left join MastResCentre mrc on mrc.ResCenId=tdv1.LocationID where " + Qrychk + " AND mp.PartyDist=1 and tdv.DistId in (" + smIDStr1 + ") ORDER BY tdv.VDate desc ";


                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


                    }

                }
            }

            {


                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Header row for CSV file.
                    csv += column.ColumnName + ',';
                }

                //Add new line.
                csv += "\r\n";

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        //Add the Data rows.
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }

                    //Add new line.
                    csv += "\r\n";
                }

                //Download the CSV file.
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DistInvoiceReport.csv");
                string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Grand Total".TrimStart('"').TrimEnd('"');
                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();
            }
        }
    }
}