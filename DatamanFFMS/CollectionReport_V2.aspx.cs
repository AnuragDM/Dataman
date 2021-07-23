using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class CollectionReport_V2 : System.Web.UI.Page
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

            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {               
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End               
               
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {

            #region Variable Declaration

            string smIDStr = "", smIDStr1 = "", Qrychk = "", Query = "";
            DataTable gvdt = new DataTable();

            #endregion

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }

            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
            {                
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }

            gvdt.Columns.Add(new DataColumn("Collection Date", typeof(String)));

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
            gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));           

            gvdt.Columns.Add(new DataColumn("Retailer/Distributor", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Retailer/Distributor Sync Id", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Mobile No", typeof(String)));
            gvdt.Columns.Add(new DataColumn("State", typeof(String)));
            gvdt.Columns.Add(new DataColumn("City", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Area", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Beat", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Payment Mode", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Invoice", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Amount", typeof(String)));


            Qrychk = "  and dc.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and dc.VDate<='" + Settings.dateformat(txttodate.Text) + "'";
            if (smIDStr1 != "")
            {
                Qrychk = Qrychk + " and dc.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
            }
            if (ddlCollectiontype.SelectedValue == "0")
            {
                Query = @"Select dc.VDate,ms.smid,ms.smname,ms.SyncId as SalesPersonSyncId,ms.mobile as SalesPersonMobile,hq.HeadquarterName,case  when ISNULL(ms.ACTIVE,0)=1 THEN 'Active' else 'Unactive' end  as Active,(sr.smname+'-'+sr.syncid) as reportingPerson,mdst.DesName,mp.partyName,mp.SyncId,mp.Mobile,dc.Mode,Isnull(dc.DistInvDocId,'') as InvNo,dc.Amount,(vg.Areaname+'-'+vg.SyncId) as StateName,(vg1.Areaname+'-'+vg1.SyncId) as City,(vg2.AreaName+'-'+vg2.SyncId) as Area,'' as Beat From DistributerCollection dc Left Join MastParty mp on dc.DistId=Mp.PartyId Left Join MastSalesRep ms on dc.smid=ms.smid left join MastHeadquarter hq on hq.Id=ms.HeadquarterId left join mastsalesrep sr on sr.smid=ms.underid left join MastLogin ml on ml.id=ms.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId Left Join mastarea vg1 on mp.CityId=vg1.areaid Left Join mastarea vg2 on mp.AreaId=vg2.areaid Left Join MastArea msd on msd.AreaId = vg1.UnderId Left Join MastArea vg on vg.AreaId = msd.UnderId where mp.partyDist=1 " + Qrychk + " order by dc.VDate desc";
            }
            else
            {
                Query = @"Select * from (Select dc.VDate,ms.smid,ms.smname,ms.SyncId as SalesPersonSyncId,ms.mobile as SalesPersonMobile,hq.HeadquarterName,case  when ISNULL(ms.ACTIVE,0)=1 THEN 'Active' else 'Unactive' end  as Active,(sr.smname+'-'+cast(sr.SMID AS Varchar)) as reportingPerson,mdst.DesName,mp.partyName,IsNull(mp.SyncId,'') as SyncId,mp.Mobile,dc.Mode,Isnull(dc.RetInvDocId,'') as InvNo,dc.Amount,(vg.AreaName+'-'+Isnull(vg.SyncId,'')) As StateName,(vg1.AreaName+'-'+Isnull(vg1.SyncId,'')) As City,(vg2.AreaName+'-'+Isnull(vg2.SyncId,'')) As Area,(vg3.AreaName+'-'+Isnull(vg3.SyncId,'')) As Beat From Temp_Transcollection dc Left Join MastParty mp on dc.PartyId=Mp.PartyId 
                        Left Join MastSalesRep ms on dc.smid=ms.smid left join MastHeadquarter hq on hq.Id=ms.HeadquarterId left join mastsalesrep sr on sr.smid=ms.underid left join MastLogin ml on ml.id=ms.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId Left Join MastArea vg1 on mp.CityId=vg1.AreaID	
Left Join MastArea vg2 on mp.AreaId=vg2.AreaId Left Join MastArea vg3 on mp.BeatId=vg3.AreaID Left Join MastArea msd on msd.AreaId = vg1.UnderId Left Join MastArea vg on vg.AreaId = msd.UnderId where mp.partyDist=0 " + Qrychk + " " +

                       " UNION ALL Select dc.VDate,ms.smid,ms.smname,ms.SyncId as SalesPersonSyncId,ms.mobile as SalesPersonMobile,hq.HeadquarterName,case  when ISNULL(ms.ACTIVE,0)=1 THEN 'Active' else 'Unactive' end  as Active,(sr.smname+'-'+cast(sr.SMID AS Varchar)) as reportingPerson,mdst.DesName,mp.partyName,IsNull(mp.SyncId,'') as SyncId,mp.Mobile,dc.Mode,Isnull(dc.RetInvDocId,'') as InvNo,dc.Amount,(vg.AreaName+'-'+Isnull(vg.SyncId,'')) As StateName,(vg1.AreaName+'-'+Isnull(vg1.SyncId,'')) As City,(vg2.AreaName+'-'+Isnull(vg2.SyncId,'')) As Area,(vg3.AreaName+'-'+Isnull(vg3.SyncId,'')) As Beat From Transcollection dc Left Join MastParty mp on dc.PartyId=Mp.PartyId  Left Join MastSalesRep ms on dc.smid=ms.smid left join MastHeadquarter hq on hq.Id=ms.HeadquarterId left join mastsalesrep sr on sr.smid=ms.underid left join MastLogin ml on ml.id=ms.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId Left Join MastArea vg1 on mp.CityId=vg1.AreaId Left Join MastArea vg2 on mp.AreaId=vg2.AreaId Left Join MastArea vg3 on mp.BeatId=vg3.AreaId Left Join MastArea msd on msd.AreaId = vg1.UnderId Left Join MastArea vg on vg.AreaId = msd.UnderId where mp.partyDist=0 " + Qrychk + " ) a order by a.VDATE desc";
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
             if (dt.Rows.Count > 0)
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     DataRow mDataRow = gvdt.NewRow();
                     //
                     Query = "select msp.SMName,msp.SMId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                     DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                     if (dtsr.Rows.Count > 0)
                     {
                         for (int j = 0; j < dtsr.Rows.Count; j++)
                         {
                             if (dtsr.Rows[j]["DesName"].ToString() != "")
                             {
                                 mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SMId"].ToString();
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
                     mDataRow["Collection Date"] = Convert.ToDateTime(dr["VDate"].ToString()).ToString("dd/MMM/yyyy").Trim();
                     mDataRow["SMId"] = dr["SMId"].ToString();
                     mDataRow["Reporting Manager"] = dr["reportingPerson"].ToString();
                     mDataRow["Name"] = dr["SMName"].ToString();
                     mDataRow["SyncId"] = dr["SalesPersonSyncId"].ToString();
                     mDataRow["Status"] = dr["Active"].ToString();
                     mDataRow["Designation"] = dr["DesName"].ToString();
                     mDataRow["HeadQuater"] = dr["HeadquarterName"].ToString();
                     mDataRow["Contact No"] = dr["SalesPersonMobile"].ToString();

                     mDataRow["Retailer/Distributor"] = dr["partyName"].ToString();
                     mDataRow["Retailer/Distributor Sync Id"] = dr["SyncId"].ToString();
                     mDataRow["Mobile No"] = dr["Mobile"].ToString();
                     mDataRow["State"] = dr["StateName"].ToString();
                     mDataRow["City"] = dr["City"].ToString();
                     mDataRow["Area"] = dr["Area"].ToString();
                     if (!string.IsNullOrEmpty(dr["Beat"].ToString()))
                         mDataRow["Beat"] = dr["Beat"].ToString();
                     mDataRow["Payment Mode"] = dr["Mode"].ToString();
                    mDataRow["Invoice"] = dr["InvNo"].ToString();
                    mDataRow["Amount"] = dr["Amount"].ToString();


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

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CollectionReport_V2.aspx");
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
            string filename = "Collection Report " + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("SMID");

                //Add a new worksheet to workbook with the Datatable name

                Excel.Worksheet excelWorkSheet =(Microsoft.Office.Interop.Excel.Worksheet) excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = "Collection Report";

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

                Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
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