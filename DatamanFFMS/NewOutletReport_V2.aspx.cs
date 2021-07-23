using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
namespace AstralFFMS
{
    public partial class NewOutletReport : System.Web.UI.Page
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
            string DashBoardDate = Request.QueryString["Date"];
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End

                BindTreeViewControl();

                this.checkNode();
                if (!string.IsNullOrEmpty(DashBoardDate))
                {
                    //  orderType = Request.QueryString["type"];
                    txtfmDate.Text = Convert.ToDateTime(DashBoardDate).ToString("dd/MMM/yyyy");
                    txttodate.Text = Convert.ToDateTime(DashBoardDate).ToString("dd/MMM/yyyy");
                    btnGo_Click(null, null);
                }
            }
        }
        private TreeNode FindRootNode(TreeNode treeNode)
        {
            while (treeNode.Parent != null)
            {
                treeNode = treeNode.Parent;
            }
            return treeNode;
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
        protected void checkNode()
        {
            //foreach (TreeNode tr in trview.)
            //{


            //}
            foreach (var node in Collect(trview.Nodes))
            {
                node.Checked = true;
                // you will see every child node here
            }
        }
        IEnumerable<TreeNode> Collect(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (var child in Collect(node.ChildNodes))
                    yield return child;
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", Qrychk = "", matBeatNew = "";
            spinner.Visible = true;
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            //   foreach (ListItem Beat in Lstbeatbox.Items)
            {
                //  if (Beat.Selected)
                {
                    //    matBeatNew += Beat.Value + ",";
                }
            }
            matBeatNew = matBeatNew.TrimStart(',').TrimEnd(',');
            string beat_Filter = "";
            if (matBeatNew != "" && matBeatNew != "0")
                beat_Filter = " and p.BeatId in (" + matBeatNew + ") ";
            else
                beat_Filter = "";

            if (smIDStr1 != "")
            {

                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }
                string Qty2 = "";
                string Qty1 = "";
                //string Qty2 = " and om.VDate BETWEEN '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";
                if (roleType != "Admin")
                {
                    Qty2 = Qty2 + " and om.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                    Qty1 = Qty1 + " and p.Created_User_id in ( SELECT userid FROM MastSalesRep ms LEFT JOIN mastlogin ml ON ms.UserId=ml.Id WHERE ms.SMId IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1))";
                }
                Qrychk = " and P.insert_date between '" + Settings.dateformat(txtfmDate.Text) + " 00:00:01' and '" + Settings.dateformat(txttodate.Text) + " 23:59:59'";


                string query = @"select a.Name As Outlet,a.Address,a.Mobile As Mobile,max (a.beat) as beat,max (a.[By]) as [By],sum(a.Business) as Business,sum(a.Pendency) as Pendency,max(a.Date) as Date,max(a.AppStatus) as [Approved Status],max(a.AppRemark) as [Approved Remark],max(a.Approvedby) As [Approved by]  from (
                  select distinct p.PartyId, (p.PartyName+'-'+max(isnull(p.syncid,''))) as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,(max(b.AreaName)+'-'+max(isnull(b.syncid,''))) AS beat,(max(ms.SMName)+'-'+max(isnull(ms.syncid,''))) AS [By],0 AS Business,0 AS Pendency,max(p.created_date) as Date ,Isnull(max(p.AppStatus),'') As AppStatus,Isnull(max(p.AppRemark),'')  AS AppRemark,Isnull((select (SMName+'-'+isnull(msa.syncid,'')) From mastsalesrep msa where msa.smid=Max(p.AppBySMId)  ),'') As Approvedby
                  FROM MastParty p left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id 
                  WHERE p.partydist=0 " + Qrychk + " " + Qty1 + " " + beat_Filter + "  group by p.PartyName,p.PartyId " +
              " union all select distinct p.PartyId,(p.PartyName+'-'+max(isnull(p.syncid,''))) as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,(max(b.AreaName)+'-'+max(isnull(b.syncid,''))) AS beat,(max(ms.SMName)+'-'+max(isnull(ms.syncid,''))) AS [By],sum(isnull(om.OrderAmount,0)) AS Business,sum(isnull(om.OrderAmount,0)) AS Pendency,max(p.Created_Date) as Date ,Isnull(max(p.AppStatus),'') As AppStatus,Isnull(max(p.AppRemark),'')  AS AppRemark,Isnull((select (SMName+'-'+isnull(msa.syncid,'')) From mastsalesrep msa where msa.smid=Max(p.AppBySMId)  ),'') As Approvedby from TransOrder om inner join MastParty p on p.PartyId=om.PartyId left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id WHERE p.PartyDist=0 " + Qrychk + " " + Qty2 + " " + beat_Filter + " group by p.PartyId,p.PartyName) a Group by a.PartyId,a.Name,a.Address,a.Mobile Order by Business desc,MAX( a.Date) DESC,name";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    try
                    {


                        Excel.Application excelApp = new Excel.Application();
                        string path = Server.MapPath("ExportedFiles//");

                        if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = "NewOutletReport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";

                        if (File.Exists(path + filename))
                        {
                            File.Delete(path + filename);
                        }


                        string strPath = Server.MapPath("ExportedFiles//" + filename);
                        Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
                        Microsoft.Office.Interop.Excel.Range chartRange;
                        Excel.Range range;





                        //Add a new worksheet to workbook with the Datatable name
                        Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();

                        excelWorkSheet.Name = "New OutLet Report";



                        for (int j = 1; j < dtItem.Columns.Count + 1; j++)
                        {
                            excelWorkSheet.Cells[1, j] = dtItem.Columns[j - 1].ColumnName;

                            range = excelWorkSheet.Cells[1, j] as Excel.Range;
                            range.Cells.Font.Name = "Calibri";
                            range.Cells.Font.Bold = true;
                            range.Cells.Font.Size = 11;
                            range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                        }



                        for (int j = 0; j < dtItem.Rows.Count; j++)
                        {
                            for (int l = 0; l < dtItem.Columns.Count; l++)
                            {
                                excelWorkSheet.Cells[j + 2, l + 1] = dtItem.Rows[j].ItemArray[l].ToString();

                            }
                        }


                        Excel.Range last = excelWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                        chartRange = excelWorkSheet.get_Range("A1", last);
                        foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                        {
                            cell.BorderAround2();
                        }
                        excelWorkSheet.Columns.AutoFit();
                        excelWorkSheet.Application.ActiveWindow.SplitRow = 1;
                        excelWorkSheet.Application.ActiveWindow.FreezePanes = true;

                        Excel.FormatConditions fcs = chartRange.FormatConditions;
                        Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
        (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                        //Excel.FormatCondition format = xlWorksheet.Rows.FormatConditions.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                        format.Interior.Color = Excel.XlRgbColor.rgbLightGray;





                        excelWorkBook.SaveAs(strPath);
                        excelWorkBook.Close();
                        excelApp.Quit();
                        // excelApp.Visible = true;
                        Response.ContentType = "application/x-msexcel";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.TransmitFile(strPath);
                        Response.End();

                    }
                    catch (Exception)
                    {


                    }

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found');", true);
                }
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);

            }
            spinner.Visible = false;
        }
        private void BindbeatDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string cityQry = @"  select AreaId,AreaName from mastarea where underId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Beat' and Active=1 order by AreaName";
                    DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    if (dtBeat.Rows.Count > 0)
                    {
                        //  Lstbeatbox.DataSource = dtBeat;
                        //  Lstbeatbox.DataTextField = "AreaName";
                        //  Lstbeatbox.DataValueField = "AreaId";
                        //  Lstbeatbox.DataBind();
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

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/NewOutletReport.aspx");
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
                    BindbeatDDl(smIDStr12);
                }
                else
                {
                    // Lstbeatbox.Items.Clear();
                    //  Lstbeatbox.DataBind();

                }
                ViewState["tree"] = smiMStr;

            }
            cnt = cnt + 1;
            return;
        }


    }
}