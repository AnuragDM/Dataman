using BusinessLayer;
using DAL;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using SQL = System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace AstralFFMS
{
    public partial class TourPlanVsActualReport_V2 : System.Web.UI.Page
    {
        string roleType = "";
        string rptTemp = "rptTemp_TourActual";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {//Ankita - 16/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalesPerson();
                BindTreeViewControl();
                //fill_TreeArea();
                BindDDLMonth();
                ddlMonthSecSale.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlYearSecSale.SelectedValue = System.DateTime.Now.Year.ToString();
                btnExport.Visible = true;

                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";

            }
        }

        private void BindTreeViewControl()
        {
            try
            {
                SQL.DataTable St = new SQL.DataTable();
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
                St.Dispose();
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, SQL.DataTable Dt)
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
            Dt.Dispose();
        }
        private void BindSalesPerson()
        {
            try
            {

                if (roleType == "Admin")
                {
                    //Ankita - 16/may/2016- (For Optimization)
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    SQL.DataTable dtcheckrole = new SQL.DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "((RoleType='RegionHead' or RoleType='StateHead')  or (RoleType='CityHead' or RoleType='DistrictHead')) and SMName<>'.'";
                    dv1.Sort = "SMName asc";

                    //ListBox1.DataSource = dv1.ToTable();
                    //ListBox1.DataTextField = "SMName";
                    //ListBox1.DataValueField = "SMId";
                    //ListBox1.DataBind();

                    dtcheckrole.Dispose();
                }
                else
                {
                    SQL.DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "((RoleType='RegionHead' or RoleType='StateHead')  or (RoleType='CityHead' or RoleType='DistrictHead')) and SMName<>'.'";
                    //ListBox1.DataSource = dv.ToTable();
                    //ListBox1.DataTextField = "SMName";
                    //ListBox1.DataValueField = "SMId";
                    //ListBox1.DataBind();

                    dt.Dispose();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
      
        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    ddlMonthSecSale.Items.Add(new ListItem(monthName.Substring(0, 3), month.ToString().PadLeft(2, '0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    ddlYearSecSale.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TourPlanVsActualReport_V2.aspx");
        }

        public DataSet getTourPlanVsActualReport(string smIDStr1)
        {

            DataSet ds = new DataSet();
            string str_des = "", des = "";
           
            SQL.DataTable dtProducts = new SQL.DataTable();

            string filter = "";
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                string Qrychk = "", query = "";

                Qrychk = " year(vl.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(vl.VDate)='" + ddlMonthSecSale.SelectedValue + "'";


                if (ddlFilter.SelectedIndex != 0 && ddlFilter.SelectedValue != "1")
                {
                    filter = "where isnull(TourDistributor,'')<>isnull(VisitDistributor,'')";
                }

                if (smIDStr1 != "")
                {
                    
                    string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                    SQL.DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtdesig.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdesig.Rows.Count; i++)
                        {
                            str_des = str_des + ",' ' as " + dtdesig.Rows[i]["DesName"].ToString();
                            des = des + "," + dtdesig.Rows[i]["DesName"].ToString();
                        }
                    }
                    dtdesig.Dispose();
                    des = des.Trim(',');


                    Create_temp(rptTemp, "transtourplan", " SMId in (" + smIDStr1 + ") AND AppStatus NOT IN ('Reject') and year(VDate)='" + ddlYearSecSale.SelectedValue + "' and month(VDate)='" + ddlMonthSecSale.SelectedValue + "'");

                    query = "alter table " + rptTemp + " add visid varchar(50)null ,SNAME varchar(200) null,VDIST varchar(max) null,VCITY varchar(max) null,VREMK varchar(max) null,Vdate1 varchar(50) null";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, query);

                    query = "update temp set vdist=dbo.getdistributor(tv.visid),VCITY=dbo.getVisitedCity(tv.cityIdS),VREMK=tv.Remark from " + rptTemp + " as temp inner join transvisit as tv on temp.smid=tv.smid and temp.vdate=tv.vdate";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, query);

                    query = "update tp set SNAME=ms.smname + '[' + ms.syncid  FROM " + rptTemp + " tp   left join   MastSalesRep ms on tp.smid=ms.smid";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, query);

                    query = "select   ms.smid,Replace(Convert(varchar,vdate,106), ' ', '/') as  [Date],mpurposename as [Purpose],mdistname as [TourDistributor],mcityname as [TourCity],vdist as [VisitDistributor],VCITY as [VisitCity],'' as [Srep],VREMK as [Remarks] ," + rptTemp + ".SMId as emp_id," + str_des.Trim(',') + ",(cp1.smname+'-'+Isnull(cp1.SyncId,'')) as ReportingManager, substring(SNAME,0,charindex('[',SNAME)) as  SalesPerson,ms.Mobile,substring(SNAME,charindex('[',SNAME)+1,len(SNAME)) as SyncId,(Case ms.Active when 1 then 'Active' else 'InActive' end) as Status,md.desname as Designation,mh.HeadquarterName as HeadQuarter,(ma.areaname+'-'+IsNull(ms.SyncId,'')) as City,Replace(Convert(varchar,ms.Joining,106), ' ', '/') as DateOfJoining,Replace(Convert(varchar,ms.Leaving,106), ' ', '/') as DateOfLeaving, " + rptTemp + ".Remarks as TourRemark from " + rptTemp + " Left Join MastSalesRep ms on " + rptTemp + ".SMId=ms.smid left join MastSalesRep cp1 on cp1.smid=ms.underid left join mastarea ma on ma.areaid=ms.cityid left join mastlogin ml on ml.id=ms.userid left join mastdesignation md on md.desid=ml.desigid  Left Join MastHeadquarter mh on mh.Id= ms.HeadquarterId Order by Date, [TourDistributor],[Purpose],[TourCity],[VisitDistributor],[VisitCity],[Remarks]";

                    dtProducts  = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
                Delete_temp(rptTemp);
                SQL.DataTable dtsr = new SQL.DataTable();
                for (int i = 0; i < dtProducts.Rows.Count; i++)
                {

                    string str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtProducts.Rows[i]["smid"].ToString() + ")";
                    dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dtsr.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsr.Rows.Count; j++)
                        {
                            dtProducts.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SyncId"].ToString();
                        }
                    }

                    string[] arr = des.Split(',');
                    for (int l = 0; l < arr.Length; l++)
                    {
                        if (dtProducts.Rows[i][arr[l]].ToString() == " ")
                        {
                            dtProducts.Rows[i][arr[l]] = "Vacant";
                        }
                    }


                }

                dtsr.Dispose();

                ds.Tables.Add(dtProducts);
                dtProducts.Dispose();

            return ds;
        }

        public void TourPlanVsActualReportExcel(DataSet ds)
        {

            SQL.DataTable dt = new SQL.DataTable();
            SQL.DataTable dtProducts = new SQL.DataTable();
            //----------------------Create Excel objects--------------------------------
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            oXL = new Excel.Application();

            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            for (int k = 0; k < ds.Tables.Count; k++)
            {
                dt = ds.Tables[k];
                if (dt.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found.');", true);
                    return;

                }
                dtProducts = new SQL.DataTable();

                oSheet = (Excel._Worksheet)oXL.Worksheets.Add();

                //-------------------Create Category wise excel Worksheet------------------------------
                if (k == 0)
                {
                    oSheet.Name = "Tour Plan Vs Actual Report";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.Columns.Remove("Srep");
                    dtProducts.Columns.Remove("emp_id");
                    dtProducts.AcceptChanges();
                }
                //-------------------------------------Add column names to excel sheet--------------------
                string[] colNames = new string[dtProducts.Columns.Count];

                int col = 0;

                foreach (DataColumn dc in dtProducts.Columns)
                    colNames[col++] = dc.ColumnName;

                string lcol = getlastcol(dtProducts.Columns.Count);

                char lastColumn = (char)(65 + dtProducts.Columns.Count - 1);

                oSheet.get_Range("A1", lcol + "1").Value2 = colNames;
                oSheet.get_Range("A1", lcol + "1").Font.Bold = true;
                oSheet.get_Range("A1", lcol + "1").VerticalAlignment
                            = Excel.XlVAlign.xlVAlignCenter;

                Range range = oSheet.get_Range("A1", lcol + "1");
                range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);

                //---------------------------Add DataRows data to Excel---------------------------
                string[,] rowData =
                        new string[dtProducts.Rows.Count, dtProducts.Columns.Count];

                int rowCnt = 0;
                int redRows = 2;
                foreach (SQL.DataRow row in dtProducts.Rows)
                {
                    for (col = 0; col < dtProducts.Columns.Count; col++)
                    {
                        rowData[rowCnt, col] = row[col].ToString();
                    }
                    redRows++;
                    rowCnt++;
                }

                //   if (k == 0)
                //   {
                //       oSheet.get_Range("G2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
                //Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                //   }

                oSheet.get_Range("A2", lcol + (rowCnt + 1).ToString()).Value2 = rowData;
                Microsoft.Office.Interop.Excel.Range chartRange;
                chartRange = oSheet.get_Range("A1", lcol + (rowCnt + 1).ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                {
                    cell.BorderAround2();
                }
                oSheet.Columns.AutoFit();
            }

            oSheet.Application.ActiveWindow.SplitRow = 1;
            oSheet.Application.ActiveWindow.FreezePanes = true;

            oXL.UserControl = true;
            string filename = "TourPlanVsActualReport_" + ddlMonthSecSale.SelectedValue + "-" + ddlYearSecSale.SelectedValue + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
            string strPath = Server.MapPath("ExportedFiles//" + filename);
            oWB.SaveAs(strPath);

            oWB.Close();
            oXL.Quit();
            dt.Dispose();
            dtProducts.Dispose();
            ds.Dispose();
            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.TransmitFile(strPath);
            Response.End();
        }


        public string getlastcol(int cnt)
        {

            int c = 0;
            int h = 0;
            int c1 = 0;

            while (cnt > 26)
            {
                int d = cnt - 26;
                cnt = d;
                h = d;
                c = c + 1;
            } c1++;


            char lc = '9';
            char lc1 = '9';
            if (c > 0)
            {
                lc = (char)(65 + c - 1);
            }
            else
                lc = (char)(65 + cnt - 1);

            if (h > 0)
                lc1 = (char)(65 + h - 1);
            string lcol = "";
            if (lc1 != '9')
                lcol = lc.ToString() + lc1.ToString();
            else
                lcol = lc.ToString();

            return lcol;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string smIDStr = "";
            string smIDStr1 = "";
            DataSet ds = new DataSet();
            try
            {
                btnExport.Enabled = false;

                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                if (smIDStr1 != "")
                {               

                    ds = getTourPlanVsActualReport(smIDStr1);
                    TourPlanVsActualReportExcel(ds);

                    btnExport.Enabled = true;
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                    btnExport.Enabled = true;
                }

                ds.Dispose();
            }
            catch (Exception ex)
            {
                btnExport.Enabled = true;
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Something went wrong.');", true);
                ex.ToString();
            }
        }
       
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        private void Create_temp(string tempTable, string mainTable, string _condition = "")
        {
            string str = "";
            str = "if OBJECT_ID('" + tempTable + "') is not null  drop table " + tempTable + "";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            if (_condition == "")
            {
                str = "select * into " + tempTable + " from " + mainTable + "";
            }
            else
            {
                str = "select * into " + tempTable + " from " + mainTable + " where " + _condition + " ";
            }
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
        }

        private void Delete_temp(string tempTable)
        {
            string str = "";
            str = "if OBJECT_ID('" + tempTable + "') is not null  drop table " + tempTable + "";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
        }

        

    }
}