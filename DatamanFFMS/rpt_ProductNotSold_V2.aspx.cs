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
    public partial class rpt_ProductNotSold : System.Web.UI.Page
    {

        string roleType = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            txtasondate.Attributes.Add("readonly", "readonly");
            txtfromdate.Attributes.Add("readonly", "readonly");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                BindMaterialGroup();
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                txtfromdate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); 
                txtasondate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");                  
               
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

        private void BindMaterialGroup()
        {
            try
            {
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
          

            string smIDStr = "", smIDStr1 = "",  Qrychk = "",Qry = "", QueryMatGrp = "",matGrpStrNew = "", matProStrNew = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

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

           
            if (smIDStr1 != "")
            {
                Qrychk =  " os1.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                Qry = " TransOrder1.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);              
                return;
            }

            if (matGrpStrNew != "" && matProStrNew != "")
            {
                Qrychk = Qrychk + " and i.ItemId in (" + matProStrNew + ") ";
                QueryMatGrp = " and i.ItemId in (" + matProStrNew + ") ";
            }
            if (matGrpStrNew != "" && matProStrNew == "")
            {
                Qrychk = Qrychk + " and psg.ItemId in (" + matGrpStrNew + ") ";
                QueryMatGrp = "and psg.ItemId in (" + matGrpStrNew + ")";
            }

            string query = @"SELECT  a.ProductGroup AS [Group],a.ItemId, a.ItemName AS [Product Name], isnull(convert(varchar, max(os1.VDate), 103), 'NA') AS LastSoldOn FROM ( " +
               " SELECT distinct i.ItemId, (i.ItemName+'-'+isnull(i.SyncId,'')) as ItemName,psg.ItemId AS ProductGroupId,(psg.ItemName+'-'+Isnull(psg.SyncId,'')) AS ProductGroup FROM mastitem i INNER JOIN mastitem psg ON psg.ItemId = i.Underid WHERE i.ItemId NOT IN " +
               " (SELECT distinct TransOrder1.ItemId FROM TransOrder1 WHERE TransOrder1.VDate BETWEEN '" + txtfromdate.Text + " 00:01' AND '" + txtasondate.Text + " 23:59'  and " + Qry + " ) " + QueryMatGrp + " ) a " +
               " left JOIN transorder1 Os1 ON Os1.ItemId = a.ItemId left JOIN mastitem i on i.ItemId=os1.ItemId INNER JOIN mastitem psg ON psg.ItemId =i.Underid where " + Qrychk + " GROUP BY a.ItemId, a.ItemName, a.ProductGroup Order by LastSoldOn desc,[Product Name]";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                 
                    dtItem.Columns.Remove("ItemId");
                    ExportDataSetToExcel(dtItem);
                    //  $('#spinner').show();
                  
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found');", true);
                }
           

        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rpt_ProductNotSold.aspx");
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
            { 
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
        private void ExportDataSetToExcel(DataTable table)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();
            string path = Server.MapPath("ExportedFiles//");

            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
            {
                Directory.CreateDirectory(path);
            }
            string filename = "Product Not Sold Report " + txtfromdate.Text.Replace('/', ' ') + "-" + txtasondate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            if (table.Rows.Count > 0)
            {
               
                //Add a new worksheet to workbook with the Datatable name

                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = "Product Not Sold";

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
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Stop", "Stop();", true);         
                Response.End();

            }



        }

  
        }
    }
