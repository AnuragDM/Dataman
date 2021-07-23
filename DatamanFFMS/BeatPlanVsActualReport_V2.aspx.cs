using BusinessLayer;
using DAL;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
    public partial class BeatPlanVsActualReport_V2 : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());            
                BindTreeViewControl();
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                BindDDLMonth();
                ddlMonthSecSale.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlYearSecSale.SelectedValue = System.DateTime.Now.Year.ToString();
                btnExport.Visible = true;
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
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }

                DataRow[] Rows = St.Select("lvl=MIN(lvl)");
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
        public class Distributors
        {
            public string Date { get; set; }
            public string SalesRepName { get; set; }
            public string HeadQuarter { get; set; }
            public string SyncId { get; set; }
            public string BeatPlanBeat { get; set; }
            public string BeatPlansyncid { get; set; }
            public string VisitBeat { get; set; }
            public string VisitBeatsyncid { get; set; }
        }
       
        private void BindSalesPerson()
        {
            try
            {

                if (roleType == "Admin")
                {

                    string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    SQL.DataTable dtcheckrole = new SQL.DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>'.'";
                    dv1.Sort = "SMName asc";
                    ListBox1.DataSource = dv1.ToTable();
                    ListBox1.DataTextField = "SMName";
                    ListBox1.DataValueField = "SMId";
                    ListBox1.DataBind();

                    dtcheckrole.Dispose();
                }
                else
                {
                    SQL.DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "SMName<>'.'";
                    dv.Sort = "SMName asc";
                    ListBox1.DataSource = dv.ToTable();
                    ListBox1.DataTextField = "SMName";
                    ListBox1.DataValueField = "SMId";
                    ListBox1.DataBind();

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
            Response.Redirect("~/BeatPlanVsActualReport.aspx");
        }


        public DataSet getBeatPlanVsActualReport(string smIDStr1)
        {
            DataSet ds = new DataSet();
            SQL.DataTable dtProducts = new SQL.DataTable();
            string Qrychk = "", getComplQry = "", query = "";
            string frmdate = txtfmDate.Text;
            string ToDate = txttodate.Text;
            string[] smid = smIDStr1.Split(',');
            SQL.DataTable dtsmid = new SQL.DataTable();
            dtsmid.Columns.Add("SMid");
            for (int i = 0; i < smid.Length; i++)
            {
                DataRow dr = dtsmid.NewRow();
                dr[0] = smid[i];
                dtsmid.Rows.Add(dr);
            }
            dtsmid.AcceptChanges();

            Qrychk = "om.VDate between '" + Settings.dateformat(frmdate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59'";
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@DateFrom", DbParameter.DbType.DateTime, 1, frmdate);
            dbParam[1] = new DbParameter("@DateTo", DbParameter.DbType.DateTime, 1, ToDate);
            dbParam[2] = new DbParameter("@ExClientItemGrptbl", DbParameter.DbType.Datatable, 8000, dtsmid);
            SQL.DataTable dtbrandsale = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_BeatplanactualV2", dbParam);

            string des="";
            dtbrandsale.Columns.Add("Weekday").SetOrdinal(1);
            int k = 2;
            string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";
            SQL.DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dtdesig.Rows.Count > 0)
            {
                for (int i = 0; i < dtdesig.Rows.Count; i++)
                {
                    //str_des = str_des + ",' ' as " + dtdesig.Rows[i]["DesName"].ToString();
                    des = des + "," + dtdesig.Rows[i]["DesName"].ToString();
                    dtbrandsale.Columns.Add(dtdesig.Rows[i]["DesName"].ToString()).SetOrdinal(k);
                    k = k + 1;
                }
            }

            des = des.Trim(',');            
            //dtbrandsale.Columns.Add("HeadQuarter");
            dtbrandsale.AcceptChanges();

            SQL.DataTable smiddetails = new SQL.DataTable();
            SQL.DataTable dtsr = new SQL.DataTable();
            for (int i = 0; i < dtbrandsale.Rows.Count; i++)
            {
                dtbrandsale.Rows[i]["Weekday"] = (Convert.ToDateTime(dtbrandsale.Rows[i]["Date"].ToString())).ToString("dddd");


                str = "select cp.smid,cp1.smname+'-'+Isnull(cp1.SyncId,'') as ReportingManager,cp.Mobile,(Case cp.Active when 1 then 'Active' else 'InActive' end) as Status,md.desname as Designation,mh.HeadquarterName as HeadQuarter,ma.areaname+'-'+Isnull(ma.SyncId,'')  as City,Replace(Convert(varchar,cp.Joining,106), ' ', '/') as DateOfJoining,Replace(Convert(varchar,cp.Leaving,106), ' ', '/') as DateOfLeaving from MastSalesRep cp left join MastSalesRep cp1 on cp1.smid=cp.underid left join mastarea ma on ma.areaid=cp.cityid left join mastlogin ml on ml.id=cp.userid left join mastdesignation md on md.desid=ml.desigid left join MastHeadquarter mh on mh.id=cp.HeadquarterId where cp.smid=" + dtbrandsale.Rows[i]["smid"].ToString() + "";
                smiddetails = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if(smiddetails.Rows.Count > 0)
                {
                    dtbrandsale.Rows[i]["ReportingManager"] = smiddetails.Rows[0]["ReportingManager"].ToString();
                    dtbrandsale.Rows[i]["Mobile"] = smiddetails.Rows[0]["Mobile"].ToString();
                    dtbrandsale.Rows[i]["Status"] = smiddetails.Rows[0]["Status"].ToString();
                    dtbrandsale.Rows[i]["Designation"] = smiddetails.Rows[0]["Designation"].ToString();
                    dtbrandsale.Rows[i]["HeadQuarter"] = smiddetails.Rows[0]["HeadQuarter"].ToString();
                    dtbrandsale.Rows[i]["City"] = smiddetails.Rows[0]["City"].ToString();
                    dtbrandsale.Rows[i]["DateOfJoining"] = smiddetails.Rows[0]["DateOfJoining"].ToString();
                    dtbrandsale.Rows[i]["DateOfLeaving"] = smiddetails.Rows[0]["DateOfLeaving"].ToString();
                }



                str = "select msp.SMName+'-'+Isnull(msp.SyncId,'') As  SMName,mdst.DesName As DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtbrandsale.Rows[i]["smid"].ToString() + ")";
                dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dtsr.Rows.Count > 0)
                {
                    for (int j = 0; j < dtsr.Rows.Count; j++)
                    {
                        dtbrandsale.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                    }
                }

                string[] arr = des.Split(',');
                for (int l = 0; l < arr.Length; l++)
                {
                    if (dtbrandsale.Rows[i][arr[l]].ToString() == " ")
                    {
                        dtbrandsale.Rows[i][arr[l]] = "Vacant";
                    }
                }


                //string headquarterName = DbConnectionDAL.GetStringScalarVal("select HeadquarterName as HeadQuarter from MastHeadquarter where Id in (select HeadquarterId from MastSalesRep where SMId = " + Convert.ToInt32(dtbrandsale.Rows[i]["SyncId"].ToString()) + ")").ToString();
               // dtbrandsale.Rows[i]["HeadQuarter"] = headquarterName.ToString();
            }

            
            smiddetails.Dispose();
            dtsr.Dispose();

            dtbrandsale.AcceptChanges();
            ds.Tables.Add(dtbrandsale);

            dtsmid.Dispose();
            dtProducts.Dispose();
            dtbrandsale.Dispose();
            return ds;
        }

        public void BeatPlanVsActualReportExcel(DataSet ds)
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
                    oSheet.Name = "Beat Plan Vs Actual Report";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.Columns.Remove("BeatPlansyncid");
                    dtProducts.Columns.Remove("VisitBeatsyncid");
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
            string filename = "BeatPlanVsActualReport_" + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
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
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        btnExport.Enabled = true;
                        return;
                    }

                    ds = getBeatPlanVsActualReport(smIDStr1);
                    BeatPlanVsActualReportExcel(ds);

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



        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {

            string smIDStr = "", smIDStr12 = "";

            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                Session["treenodes"] = smIDStr12;

            }


        }
    }
}