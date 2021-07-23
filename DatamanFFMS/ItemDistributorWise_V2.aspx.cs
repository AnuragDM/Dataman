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
using System.Web.Services;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class ItemDistributorWise_V2 : System.Web.UI.Page
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
                BindMaterialGroup();
               
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
              
                BindTreeViewControl();
               
               
            }
        }
        public class Products
        {
            public string Name { get; set; }
            public string DName { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Qty { get; set; }
            public string Amount { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetItemDistWise(string Distid, string ProductGroup, string Product, string Fromdate, string Todate)
        {
            string qry = "";
            string Qrychk = " os.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and os.VDate<='" + Settings.dateformat(Todate) + " 23:59'";

            if (ProductGroup != "" && Product == "")
            {
                qry = "and pg.itemid in(" + ProductGroup + ")";

            }
            if (Product != "" && Product != "")
            {
                qry = qry + "and i.itemid in (" + Product + ")";

            }
            string query = "select (i.ItemId) as Code,i.ItemName as Name,psg.ItemId AS productgrpid,da.PartyName as DName,vg.cityName as City,vg.stateName as State,sum(os.Qty) as Qty,sum(os.Qty*os.Rate) AS Amount from Transorder1 os " +
           " left join MastParty da on da.AreaId=os.AreaId LEFT JOIN ViewGeo vg ON vg.areaId=da.AreaId inner join mastitem i on i.ItemId=os.ItemId left JOIN mastitem  psg ON psg.ItemId=i.Underid" +
           " WHERE da.PartyDist=1 and da.Partyid in (" + Distid + ") and " + Qrychk + " " + qry + " group by da.PartyName,i.ItemId,i.ItemName,psg.ItemId,vg.cityName,vg.stateName order by i.itemname";

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);
        }

        private void BindMaterialGroup()
        {
            try
            { //Ankita - 13/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {

                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();

                }
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

        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string citystr = "";
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');
                    //string distqry = @"select * from MastParty where CityId in (" + citystr + ")  and PartyDist=1 and Active=1 order by PartyName";
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
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                  
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
            { //Ankita - 13/may/2016- (For Optimization)
                //string mastItemQry1 = @"select * from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1 order by itemname";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                ClearControls();
            }

        }

        private void ClearControls()
        {
            try
            {
                productListBox.Items.Clear();               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
           
            foreach (TreeNode node in trview.CheckedNodes)
            {
               if(node.Checked)
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    distIdStr1 += item.Value + ",";
                }
            }
            distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');

            //For MatGrp
            foreach (ListItem matGrpItems in matGrpListBox.Items)
            {
                if (matGrpItems.Selected)
                {
                    matGrpStrNew += matGrpItems.Value + ",";
                }
            }
            matGrpStrNew = matGrpStrNew.TrimStart(',').TrimEnd(',');

            //For Product
            foreach (ListItem product in productListBox.Items)
            {
                if (product.Selected)
                {
                    matProStrNew += product.Value + ",";
                }
            }
            matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');

            if (matGrpStrNew != "" && matProStrNew != "")
            {
                QueryMatGrp = " and i.ItemId in (" + matProStrNew + ") ";
            }
            if (matGrpStrNew != "" && matProStrNew == "")
            {
                QueryMatGrp = " and psg.ItemId in (" + matGrpStrNew + ") ";
            }
            if (smIDStr1 != "")
            {
                if (distIdStr1 != "")
                {
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                       
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }

                    Qrychk = " os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                    string query = "select (i.ItemId) as Code,i.ItemName+'-'+Max(Isnull(i.SyncId,'')) as [Product Name],psg.ItemId AS productgrpid,da.PartyName+'-'+Max(Isnull(da.SyncId,'')) as [Distributor Name],(vg.stateName+'-'+max(IsNull(mss.SyncId,''))) as State,(vg.cityName+'-'+max(IsNull(msc.SyncId,''))) as City,(max(msa.AreaName)+'-'+max(IsNull(msa.SyncId,''))) as Area,sum(os.Qty) as Qty,sum(os.Qty*os.Rate) AS Amount from Transorder1 os  left join MastParty da on da.AreaId=os.AreaId LEFT JOIN ViewGeo vg ON vg.areaId=da.AreaId Left Join MastArea msa on msa.AreaId=da.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId inner join mastitem i on i.ItemId=os.ItemId left JOIN mastitem  psg ON psg.ItemId=i.Underid WHERE da.PartyDist=1 and da.Partyid in (" + distIdStr1 + ") and " + Qrychk + " " + QueryMatGrp + " group by da.PartyName,i.ItemId,i.ItemName,psg.ItemId,vg.cityName,vg.stateName order by i.itemname";


                    DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    if (dtItem.Rows.Count > 0)
                    {
                        dtItem.Columns.Remove("Code");
                        dtItem.Columns.Remove("productgrpid");
                        ExportDataSetToExcel(dtItem);
                        
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found');", true);
                       
                    }

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                    
                }
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ItemDistributorWise_V2.aspx");
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
            string filename = "Product Sell Distributor wise Report  " + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            if (table.Rows.Count > 0)
            {
                //Add a new worksheet to workbook with the Datatable name

                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = "Product Sell Distributor wise";

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