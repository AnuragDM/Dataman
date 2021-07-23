using BusinessLayer;
using DAL;
//using Microsoft.Office.Interop.Excel;
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
//using System.Reflection;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using SQL = System.Data;
using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace AstralFFMS
{
    public partial class ComplaintReport_V2 : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                //Added By - Nishu 06/12/2015 
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txtfmDate.Attributes.Add("ReadOnly", "true");
                txttodate.Attributes.Add("ReadOnly", "true");
                roleType = Settings.Instance.RoleType;
                //End 
                //BindProductClass();
                //BindSalePersonDDl();
                BindPartyType();
                BindMaterialGroup();
                BindDepartment();
                BindTreeViewControl();
                //Ankita - 13/may/2016- (For Optimization)
                roleType = Settings.Instance.RoleType;
                // GetRoleType(Settings.Instance.RoleID);
                //btnExport.Visible = true;

                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";

            }


        }
        private void BindPartyType()
        {
            try
            {//Ankita - 13/may/2016- (For Optimization)
                //string partytypequery = @"select * from PartyType order by PartyTypeName";
                string partytypequery = @"select PartyTypeId,PartyTypeName from PartyType order by PartyTypeName";
                SQL.DataTable partytypedt = DbConnectionDAL.GetDataTable(CommandType.Text, partytypequery);

                if (partytypedt.Rows.Count > 0)
                {
                    ddlpartytype.DataSource = partytypedt;
                    ddlpartytype.DataTextField = "PartyTypeName";
                    ddlpartytype.DataValueField = "PartyTypeId";
                    ddlpartytype.DataBind();
                }
                ddlpartytype.Items.Insert(0, new ListItem("-- Select --", "0"));
                ddlpartytype.Items.Insert(1, new ListItem("DISTRIBUTOR", null));

                partytypedt.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindPartyTypePersons()
        {
            SQL.DataTable dtptype = new SQL.DataTable(); string str = "";
            if (ddlpartytype.SelectedItem.Text.ToUpper() == "DISTRIBUTOR")
            {
                str = "select mp.PartyName,mp.PartyId FROM MastParty mp where mp.PartyDist=1 and mp.Active=1 ORDER BY PartyName";
            }
            else
            {
                str = "select mp.PartyName,mp.PartyId FROM MastParty mp where mp.PartyDist=0 and mp.Active=1 and mp.partytype=" + ddlpartytype.SelectedValue + " ORDER BY PartyName";
            }
            dtptype = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            ddlpartytypepersons.Items.Clear();
            if (dtptype.Rows.Count > 0)
            {
                ddlpartytypepersons.DataSource = dtptype;
                ddlpartytypepersons.DataTextField = "PartyName";
                ddlpartytypepersons.DataValueField = "PartyId";
                ddlpartytypepersons.DataBind();
            }
            ddlpartytypepersons.Items.Insert(0, new ListItem("-- Select --", "0"));

            dtptype.Dispose();
        }
        private void BindDepartment()
        {
            try
            {
                //25/01/2017 Nishu (Bind Department Nature wise)
                //string str = @"SELECT T1.DepId,T1.DepName FROM MastDepartment AS T1 WHERE T1.Active=1 order by T1.DepName";
                string str = @"select DepId,DepName from MastDepartment where depid in ( select deptid from MastComplaintNature) and Active=1 order by DepName";
                SQL.DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    LstDepartment.DataSource = dt;
                    LstDepartment.DataTextField = "DepName";
                    LstDepartment.DataValueField = "DepId";
                    LstDepartment.DataBind();
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {

            }
        }



        private void BindSalePersonDDl()
        {
            SQL.DataTable dtcheckrole = new SQL.DataTable();
            try
            {
                if (roleType == "Admin")
                {//Ankita - 13/may/2016- (For Optimization)
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";

                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";

                    //ListBox1.DataSource = dv1.ToTable();
                    //ListBox1.DataTextField = "SMName";
                    //ListBox1.DataValueField = "SMId";
                    //ListBox1.DataBind();
                }
                else
                {
                    SQL.DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    //     dv.RowFilter = "RoleName='Level 1'";
                    dv.RowFilter = "SMName<>.";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        //ListBox1.DataSource = dv.ToTable();
                        //ListBox1.DataTextField = "SMName";
                        //ListBox1.DataValueField = "SMId";
                        //ListBox1.DataBind();
                    }

                    dt.Dispose();
                    dv.Dispose();
                }

                dtcheckrole.Dispose();
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
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

        private void BindMaterialGroup()
        {
            try
            {//Ankita - 13/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                SQL.DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();
                }
                //   ddlMatGrp.Items.Insert(0, new ListItem("--Please select--"));
                dtProdRep.Dispose();
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
                roleType = Settings.Instance.RoleType;
                if (roleType == "Admin")
                {
                    string distqry1 = @"select PartyId,PartyName from MastParty where PartyDist=1 and Active=1 order by PartyName";
                    SQL.DataTable dtDist1 = DbConnectionDAL.GetDataTable(CommandType.Text, distqry1);
                    if (dtDist1.Rows.Count > 0)
                    {
                        DistListbox.DataSource = dtDist1;
                        DistListbox.DataTextField = "PartyName";
                        DistListbox.DataValueField = "PartyId";
                        DistListbox.DataBind();

                    }
                    dtDist1.Dispose();
                }
                else
                {
                    if (SMIDStr != "")
                    {
                        string citystr = "";
                        //string cityQry = @"  select * from mastarea where areaid in (select distinct underid from mastarea where areaid in (select distinct underid from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.SMID + ")) and Active=1 )) and areatype='City' and Active=1 order by AreaName";
                        string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                        SQL.DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                        for (int i = 0; i < dtCity.Rows.Count; i++)
                        {
                            citystr += dtCity.Rows[i]["AreaId"] + ",";
                        }

                        dtCity.Dispose();
                        citystr = citystr.TrimStart(',').TrimEnd(',');
                        //string distqry = @"select * from MastParty where CityId in (" + citystr + ") and PartyDist=1 and Active=1 order by PartyName";
                        string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                        SQL.DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                        if (dtDist.Rows.Count > 0)
                        {
                            DistListbox.DataSource = dtDist;
                            DistListbox.DataTextField = "PartyName";
                            DistListbox.DataValueField = "PartyId";
                            DistListbox.DataBind();
                        }
                        dtDist.Dispose();
                    }
                    else
                    {
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                        //rpt.DataSource = null;
                        //rpt.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        private void ClearControls()
        {
            try
            {
                //    ddlProduct.Items.Clear();
                productListBox.Items.Clear();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ComplaintReport_V2.aspx");
        }

        protected void matGrpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            //         string message = "";
            foreach (ListItem matGrp in matGrpListBox.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

            if (matGrpStr != "")
            {//Ankita - 13/may/2016- (For Optimization)
                //string mastItemQry1 = @"select * from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                SQL.DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //       ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                ClearControls();
            }
            //if (ddlComplaint.SelectedItem.Text == "Sales Person")
            //{
            //    divdist.Attributes.Add("style", "display:none;");
            //    divptype.Attributes.Remove("style");
            //    divsp.Attributes.Remove("style");
            //}
            //else if (ddlComplaint.SelectedItem.Text == "Distributor")
            //{
            //    divptype.Attributes.Add("style", "display:none;");
            //    divsp.Attributes.Add("style", "display:none;");
            //    divdist.Attributes.Remove("style");
            //}
        }

        protected void ddlComplaint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlComplaint.SelectedItem.Text == "Sales Person")
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }
                ddlStatus.SelectedIndex = 0;
                DistListbox.SelectedIndex = -1;
                ddlpartytype.SelectedIndex = 0;
                //ddlpartytypepersons.SelectedIndex = 0;
                matGrpListBox.SelectedIndex = -1;
                productListBox.SelectedIndex = -1;
                LstDepartment.SelectedIndex = -1;
                LstCompNature.SelectedIndex = -1;

                divptype.Visible = false;
                divsp.Visible = true;
            }
            else if (ddlComplaint.SelectedItem.Text == "Distributor")
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }
                ddlStatus.SelectedIndex = 0;
                DistListbox.SelectedIndex = -1;
                ddlpartytype.SelectedIndex = 0;
                //ddlpartytypepersons.SelectedIndex = 0;
                matGrpListBox.SelectedIndex = -1;
                productListBox.SelectedIndex = -1;
                LstDepartment.SelectedIndex = -1;
                LstCompNature.SelectedIndex = -1;

                string smIDStr12 = Settings.Instance.SMID;
                BindDistributorDDl(smIDStr12);

                divptype.Visible = true;
                divsp.Visible = false;
            }
            else
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }
                ddlStatus.SelectedIndex = 0;
                DistListbox.SelectedIndex = -1;
                ddlpartytype.SelectedIndex = 0;
                ddlpartytypepersons.SelectedIndex = 0;
                matGrpListBox.SelectedIndex = -1;
                productListBox.SelectedIndex = -1;
                LstDepartment.SelectedIndex = -1;
                LstCompNature.SelectedIndex = -1;
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


        public DataSet getSalesPersonComplaintReport(string smIDStr1, string Qrychk)
        {
            DataSet ds = new DataSet();
            SQL.DataTable dtProducts = new SQL.DataTable();
            string stradd = "";


            if (!string.IsNullOrEmpty(ddlpartytype.Text) && ddlpartytype.SelectedValue != "0")
            {
                if (ddlpartytype.SelectedItem.Text.ToUpper() == "DISTRIBUTOR") stradd += " and vc.PartyType IS NULL";
                else stradd += " and PartyType ='" + ddlpartytype.SelectedValue + "'";

            }
            if (!string.IsNullOrEmpty(ddlpartytypepersons.Text) && ddlpartytypepersons.SelectedValue != "0")
            {
                stradd += " and DistId ='" + ddlpartytypepersons.SelectedValue + "'";
            }

            string getComplQry = "select Replace(Convert(varchar,vc.CompDate,106), ' ', '/') as ComplainDate,vc.CompBY as ComplainBy,vc.SyncId as ComplainBySyncId,(vc.City+'-'+IsNull(ma.SyncId,'')) AS City,vc.Status as Status,vc.PartyTypeName as PartyTypeName,vc.Item as Item,vc.ItemId as ItemId,vc.DepName as DepartmentName,vc.ComplaintNature as ComplaintNature,vc.Complaint as Complaint,vc.URL as URL,vc.Distributor as Distributor,vc.DistId as DistId,vc.SMId as SMId,vc.PartyType as PartyType,mi.UnderId as UnderId,vc.DepId as DepId,vc.ComplNatId as ComplNatId from [View_ComplaintReport] vc left join mastitem mi on vc.ItemId=mi.ItemId left JOIN MastParty mp ON mp.PartyId=vc.DistId left JOIN MastArea ma ON ma.AreaId=mp.CityId where 1=1 and " + Qrychk + " and vc.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) " + stradd + " order by vc.CompDate desc ";
            dtProducts = DbConnectionDAL.GetDataTable(CommandType.Text, getComplQry);

            ds.Tables.Add(dtProducts);
            dtProducts.Dispose();
            return ds;
        }

        public DataSet getDistributorComplaintReport(string DistIdStr1, string Qrychk)
        {
            DataSet ds = new DataSet();
            SQL.DataTable dtProducts = new SQL.DataTable();
            string stradd = "";

            string getDistComplQry = @"select Replace(Convert(varchar,c.VDate,106), ' ', '/') AS ComplainDate,cp.PartyName as ComplainBY,cp.SyncId,(ma.AreaName +'-'+IsNull(ma.SyncId,'')) as City,Status = (Case when isnull(c.Status,'P') ='P' Then 'Pending' when isnull(c.Status,'P') ='W' Then 'WIP' else 'Resolved' end ),'' as PartyTypeName,i.ItemName as Item,md.DepName AS DepName,cn.Name AS ComplaintNature, c.Remark [Complaint],c.Imgurl as URL,isnull(cp.PartyName,'') AS Distributor,i.ItemId as ItemId,c.DistId as DistId,c.Smid as SMId,i.UnderId as UnderId,md.DepId as DepId,cn.Id as ComplNatId from TransComplaint c left join MastItem i on i.ItemId=c.ItemId left join MastParty cp on cp.PartyId=c.DistId left JOIN MastArea ma ON ma.AreaId=cp.CityId left JOIN MastComplaintNature cn ON cn.Id=c.ComplNatId  LEFT JOIN MastDepartment md  ON cn.DeptId=md.DepId  where " + Qrychk + " and c.DistId in (" + DistIdStr1 + ") order by c.Vdate desc ";
            //getDistComplQry = @"select * from [View_ComplaintReport] where " + Qrychk + " and DistId in (" + DistIdStr1 + ") and SMID = 0  order by CompDate desc ";
            dtProducts = DbConnectionDAL.GetDataTable(CommandType.Text, getDistComplQry);


            ds.Tables.Add(dtProducts);
            dtProducts.Dispose();
            return ds;
        }

        public void ComplaintReportExcel(DataSet ds)
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
                    oSheet.Name = "Complaint Report";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("ItemId");
                    dtProducts.Columns.Remove("URL");
                    dtProducts.Columns.Remove("Distributor");
                    dtProducts.Columns.Remove("SMId");
                    dtProducts.Columns.Remove("UnderId");
                    dtProducts.Columns.Remove("DepId");
                    dtProducts.Columns.Remove("ComplNatId");
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

                Excel.Range range = oSheet.get_Range("A1", lcol + "1");
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
            string filename = "ComplaintReport_" + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
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
            }
            c1++;


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
            string Qrychk = "", getComplQry = "", getDistComplQry = "", strDepartment = "", strCompNature = "";
            string smIDStr1 = "", smIDStr = "", matProStrNew = "", matGrpStr = "";
            string DistIdStr1 = "";
            DataSet ds = new DataSet();
            if (ddlComplaint.SelectedIndex > 0)
            {
                try
                {
                    //btnExport.Enabled = false;

                    foreach (TreeNode node in trview.CheckedNodes)
                    {
                        smIDStr1 = node.Value;
                        {
                            smIDStr += node.Value + ",";
                        }
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                    // For Distributor
                    foreach (ListItem DistId in DistListbox.Items)
                    {
                        if (DistId.Selected)
                        {
                            DistIdStr1 += DistId.Value + ",";
                        }
                    }
                    DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');

                    //For Product
                    foreach (ListItem product in productListBox.Items)
                    {
                        if (product.Selected)
                        {
                            matProStrNew += product.Value + ",";
                        }
                    }
                    matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');
                    //For Material Group
                    foreach (ListItem MatGrp in matGrpListBox.Items)
                    {
                        if (MatGrp.Selected)
                        {
                            matGrpStr += MatGrp.Value + ",";
                        }
                    }
                    matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

                    //For Department
                    foreach (ListItem itm in LstDepartment.Items)
                    {
                        if (itm.Selected)
                        {
                            strDepartment += itm.Value + ",";
                        }
                    }
                    strDepartment = strDepartment.TrimStart(',').TrimEnd(',');

                    //For ComplaintNature
                    foreach (ListItem itm in LstCompNature.Items)
                    {
                        if (itm.Selected)
                        {
                            strCompNature += itm.Value + ",";
                        }
                    }
                    strCompNature = strCompNature.TrimStart(',').TrimEnd(',');
                    if (smIDStr1 != "")
                    {
                        Qrychk = " vc.CompDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and vc.CompDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                    }
                    else
                    {
                        Qrychk = " VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                    }
                    if (smIDStr1 != "")
                    {
                        if (matProStrNew != "")
                            Qrychk = Qrychk + " and vc.ItemId in (" + matProStrNew + ") ";

                        if (matGrpStr != "")
                        {
                            Qrychk = Qrychk + " and mi.UnderId in (" + matGrpStr + ")";
                        }

                        if (strDepartment != "")
                        {
                            Qrychk = Qrychk + " and DepId in (" + strDepartment + ") ";
                        }

                        if (strCompNature != "")
                        {
                            Qrychk = Qrychk + " and ComplNatId  in (" + strCompNature + ") ";
                        }
                        if (ddlStatus.SelectedValue != "A")
                        {
                            // Qrychk = Qrychk + " and Status Is Null or  Status ='" + ddlStatus.SelectedItem.Text + "'";
                            Qrychk = Qrychk + " and isnull(Status,'P') ='" + ddlStatus.SelectedValue + "'";
                        }
                    }
                    else
                    {
                        if (matProStrNew != "")
                            Qrychk = Qrychk + " and c.ItemId in (" + matProStrNew + ") ";

                        if (matGrpStr != "")
                        {
                            Qrychk = Qrychk + " and i.UnderId in (" + matGrpStr + ")";
                        }

                        if (strDepartment != "")
                        {
                            Qrychk = Qrychk + " and md.DepId in (" + strDepartment + ") ";
                        }

                        if (strCompNature != "")
                        {
                            Qrychk = Qrychk + " and c.ComplNatId  in (" + strCompNature + ") ";
                        }
                        if (ddlStatus.SelectedValue != "A")
                        {
                            // Qrychk = Qrychk + " and Status Is Null or  Status ='" + ddlStatus.SelectedItem.Text + "'";
                            Qrychk = Qrychk + " and isnull(Status,'P') ='" + ddlStatus.SelectedValue + "'";
                        }
                    }
                    // string stradd = ""; 

                    if (smIDStr1 != "")
                    {
                        if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                            //btnExport.Enabled = true;
                            return;
                        }


                        ds = getSalesPersonComplaintReport(smIDStr1, Qrychk);
                        ComplaintReportExcel(ds);

                        //btnExport.Enabled = true;
                    }
                    else if (DistIdStr1 != "")
                    {

                        if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                            return;
                        }

                        ds = getDistributorComplaintReport(DistIdStr1, Qrychk);
                        ComplaintReportExcel(ds);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select sales person or Distributor');", true);
                        //btnExport.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    //btnExport.Enabled = true;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Something went wrong.');", true);
                    ex.ToString();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select complain by option');", true);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void LstDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            //         string message = "";
            foreach (ListItem itm in LstDepartment.Items)
            {
                if (itm.Selected)
                {
                    str += itm.Value + ",";
                }
            }
            str = str.TrimStart(',').TrimEnd(',');

            if (str != "")
            {
                string str1 = @"SELECT T1.Id,T1.Name FROM MastComplaintNature AS T1 WHERE T1.NatureType='Complaint' AND T1.Active=1 AND T1.DeptId IN (" + str + ")";
                SQL.DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                if (dt1.Rows.Count > 0)
                {
                    LstCompNature.DataSource = dt1;
                    LstCompNature.DataTextField = "Name";
                    LstCompNature.DataValueField = "Id";
                    LstCompNature.DataBind();
                }

            }
            //if (ddlComplaint.SelectedItem.Text == "Sales Person")
            //{
            //    divdist.Attributes.Add("style", "display:none;");
            //    divptype.Attributes.Remove("style");
            //    divsp.Attributes.Remove("style");
            //}
            //else if (ddlComplaint.SelectedItem.Text == "Distributor")
            //{
            //    divptype.Attributes.Add("style", "display:none;");
            //    divsp.Attributes.Add("style", "display:none;");
            //    divdist.Attributes.Remove("style");
            //}

        }

        protected void ddlpartytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpartytype.SelectedIndex > 0)
            {
                BindPartyTypePersons();// lblpartytypepersons.InnerText = "Distributor Name";
            }
            else { ddlpartytypepersons.Items.Clear(); }
            //if (ddlComplaint.SelectedItem.Text == "Sales Person")
            //{
            //    divdist.Attributes.Add("style", "display:none;");
            //    divptype.Attributes.Remove("style");
            //    divsp.Attributes.Remove("style");
            //}
            //else if (ddlComplaint.SelectedItem.Text == "Distributor")
            //{
            //    divptype.Attributes.Add("style", "display:none;");
            //    divsp.Attributes.Add("style", "display:none;");
            //    divdist.Attributes.Remove("style");
            //}
        }

        protected void complreportrpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                if (ddlComplaint.SelectedValue == "Distributor")
                {
                    var col = e.Item.FindControl("tdsname");
                    col.Visible = false;
                }
                else
                {
                    var col = e.Item.FindControl("tdsname");
                    col.Visible = true;
                }
            }
            if (e.Item.ItemType == ListItemType.Header)
            {

                if (ddlComplaint.SelectedValue == "Distributor")
                {
                    var col1 = e.Item.FindControl("thsname");
                    col1.Visible = false;
                }
                else
                {
                    var col1 = e.Item.FindControl("thsname");
                    col1.Visible = true;
                }
            }
        }
    }
}