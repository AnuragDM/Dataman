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
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class LeaveReportNew : System.Web.UI.Page
    {
        string roleType = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {

                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");

                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();

            }
        }


        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {

                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }


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
        void fill_TreeArea()
        {
            int lowestlvl = 0;
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {


                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {
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
                getchilddata(tnParent, tnParent.Value);
            }
        }

        private void getchilddata(TreeNode parent, string ParentId)
        {

            string SmidVar = string.Empty;
            string GetFirstChildData = string.Empty;
            int levelcnt = 0;
            if (Settings.Instance.RoleType == "Admin")

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

                for (int j = levelcnt + 1; j <= 6; j++)
                {
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
                    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
                    //  SmidVar = string.Empty;
                    int mTotRows = dtChild.Rows.Count;
                    if (mTotRows > 0)
                    {
                        SmidVar = string.Empty;
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
            //if (dtChild.Rows.Count > 0)
            //{

            //    for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
            //    {
            //        spnode = parent;
            //        DataRow[] dr = dtChild.Select("maingrp =" + FirstChildDataDt.Rows[i]["smid"].ToString() + " and smchild <> " + FirstChildDataDt.Rows[i]["smid"].ToString());
            //        for (int j = 0; j < dr.Length; j++)
            //        {
            //          //  if (spnode.Value == dr[j]["smchild"].ToString()) { }
            //            FillChildArea(spnode, FirstChildDataDt.Rows[i]["smid"].ToString(), dr[j]["smchild"].ToString(), dr[j]["smname"].ToString());
            //        }
            //    }
            //}

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


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LeaveReportNew.aspx");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            #region Variable Declaration

            string smIDStr = "", Qrychk = "", Query = "";
            string smIDStr1 = "";
            DataTable gvdt = new DataTable();
            DataTable dtdesig = new DataTable();

            #endregion

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            Qrychk = " (lr.FromDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.FromDate<='" + Settings.dateformat(txttodate.Text) + " 23:59' or lr.ToDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.ToDate<='" + Settings.dateformat(txttodate.Text) + " 23:59')";

            if (ddlLeavStatus.SelectedValue != "0" && ddlLeavStatus.SelectedValue != "")
                Qrychk = Qrychk + " and lr.AppStatus='" + ddlLeavStatus.SelectedValue + "'";

            if (smIDStr1 != "")
            {
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }


                Query = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";
                dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
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
                gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));

                gvdt.Columns.Add(new DataColumn("For Days", typeof(String)));
                gvdt.Columns.Add(new DataColumn("From Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("To Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reason", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Leave Status", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Approved By", typeof(String)));

                Query = "select lr.FromDate, lr.Reason, lr.AppStatus,lr.NoOfDays, (cp1.SMName+'-'+Isnull(cp1.SyncId,'')) as AppByName, lr.ToDate,cp.smid,cp.SMName,cp.SyncId,cp.mobile,hq.HeadquarterName,case  when ISNULL(CP.ACTIVE,0)=1 THEN 'Active' else 'Unactive' end  as Active,(sr.smname+'-'+isnull(sr.syncid,'')) as reportingPerson,(mdst.DesName+'-'+Isnull(mdst.syncid,'')) as DesName from TransLeaveRequest lr left join MastSalesRep cp on cp.SMId=lr.SMId left join MastSalesRep cp1 on cp1.SMId=lr.AppBySMId  left join MastHeadquarter hq on hq.Id=cp.HeadquarterId left join mastsalesrep sr on sr.smid=cp.underid left join MastLogin ml on ml.id=cp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId where " + Qrychk + " and lr.SMId in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))) order by SMname";
                DataTable dtLeaveRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dtLeaveRep.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeaveRep.Rows)
                    {
                        DataRow mDataRow = gvdt.NewRow();
                        //
                        Query = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                        DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        if (dtsr.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtsr.Rows.Count; j++)
                            {
                                if (dtsr.Rows[j]["DesName"].ToString() != "")
                                {
                                    mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SyncId"].ToString();
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
                        mDataRow["Name"] = dr["SMName"].ToString();
                        mDataRow["SyncId"] = dr["SyncId"].ToString();
                        mDataRow["Status"] = dr["Active"].ToString();
                        mDataRow["Designation"] = dr["DesName"].ToString();
                        mDataRow["HeadQuater"] = dr["HeadquarterName"].ToString();
                        mDataRow["Contact No"] = dr["mobile"].ToString();

                        mDataRow["For Days"] = dr["NoOfDays"].ToString();
                        mDataRow["From Date"] = Convert.ToDateTime(dr["FromDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                        mDataRow["To Date"] = Convert.ToDateTime(dr["ToDate"].ToString()).ToString("yyyy-MM-dd").Trim();
                        mDataRow["Reason"] = dr["Reason"].ToString();
                        mDataRow["Leave Status"] = dr["AppStatus"].ToString();
                        mDataRow["Approved By"] = dr["AppByName"].ToString();


                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();


                    }
                    dtdesig.Dispose();
                    dtLeaveRep.Dispose();

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
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select sales person');", true);
            }

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

            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
            {
                Directory.CreateDirectory(path);
            }
            string filename = "Leave Report " + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("SMID");

                //Add a new worksheet to workbook with the Datatable name

                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = "Leave Report";

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
    }
}