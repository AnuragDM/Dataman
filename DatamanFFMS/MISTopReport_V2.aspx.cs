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


namespace AstralFFMS
{
    public partial class MISTopReport : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        string orderType = "";
        string conString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            conString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            string DashBoardDate = Request.QueryString["Date"];
            orderType = Request.QueryString["type"];

            if (!IsPostBack)
            {
                List<Products> Products = new List<Products>();
                Products.Add(new Products());
                BindMaterialGroup();
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();             
                btnExport.Visible = true;
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
                this.checkNode();
                if (!string.IsNullOrEmpty(DashBoardDate) && !string.IsNullOrEmpty(orderType))
                {
                    //  orderType = Request.QueryString["type"];
                    txtfmDate.Text = Convert.ToDateTime(DashBoardDate).ToString("dd/MMM/yyyy");
                    txttodate.Text = Convert.ToDateTime(DashBoardDate).ToString("dd/MMM/yyyy");
                    // btnGo_Click(null, null);
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
        public class Products
        {
            public string ProductGroup { get; set; }
            public string Product { get; set; }

            public string Qty { get; set; }
            // public string Rate { get; set; }
            public string Amount { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetTopproduct(string ProductGroup, string Product, string Fromdate, string Todate, string noofrecords, string salesPerson)
        {
            string qry = "";
            string Qrychk = " os.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and os.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            //            string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
            //                           on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where (" + Qrychk + ") group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";
            if (ProductGroup != "" && Product == "")
            {
                qry = "and pg.itemid in(" + ProductGroup + ")";

            }
            if (Product != "" && Product != "")
            {
                qry = qry + "and os.itemid in (" + Product + ")";

            }
            if (salesPerson != "")
            {
                qry = qry + "and os.SMId in (" + salesPerson + ")";
            }

            string query = "SELECT TOP " + noofrecords + " pg.ItemName [ProductGroup],i.ItemName [Product],convert(int,sum(os.Qty)) [Qty],convert(decimal, sum(os.Qty * os.Rate)) [Amount] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId ";
            query = query + "left JOIN MastItem pg ON pg.ItemId = i.Underid   where  " + Qrychk + " " + qry + " GROUP BY i.ItemId, i.ItemName, pg.ItemName Order by sum(os.Qty * os.Rate) desc";



            SQL.DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


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

        private void BindMaterialGroup()
        {
            try
            { //Ankita - 13/may/2016- (For Optimization)
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
                dtProdRep.Dispose();
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindbeatDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string cityQry = @"  select AreaId,AreaName from mastarea where underId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Beat' and Active=1 order by AreaName";
                    SQL.DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    if (dtBeat.Rows.Count > 0)
                    {
                        Lstbeatbox.DataSource = dtBeat;
                        Lstbeatbox.DataTextField = "AreaName";
                        Lstbeatbox.DataValueField = "AreaId";
                        Lstbeatbox.DataBind();
                    }
                    dtBeat.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    //TopRetailer.DataSource = null;
                    //TopRetailer.DataBind();
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
                pgroup.Attributes.Add("style", "display:none;");
                pronme.Attributes.Add("style", "display:none;");
                CategoryType.SelectedIndex = -1;
                Lstbeatbox.Items.Clear();
                productListBox.Items.Clear();
                // Topsaleproduct.DataSource = null;
                //Topsaleproduct.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
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


        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MISTopReport.aspx");
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
                SQL.DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
                dtMastItem1.Dispose();
            }
            else
            {
                ClearControls();
            }

        }


        protected void CategoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CType = CategoryType.SelectedItem.ToString();

            if (CType == "Product")
            {
                pgroup.Attributes.Add("style", "display:block;");
                pronme.Attributes.Add("style", "display:block;");
                beat.Attributes.Add("style", "display:none;");
            }
            else if (CType == "Distributor")
            {
                pgroup.Attributes.Add("style", "display:none;");
                pronme.Attributes.Add("style", "display:none;");
                beat.Attributes.Add("style", "display:none;");
            }
            else if (CType == "Retailer")
            {
                string smIDStr12 = "", smIDStr = "";
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
                pgroup.Attributes.Add("style", "display:none;");
                pronme.Attributes.Add("style", "display:none;");
                beat.Attributes.Add("style", "display:block;");
            }
            else if (CType == "Sales Person")
            {
                pgroup.Attributes.Add("style", "display:none;");
                pronme.Attributes.Add("style", "display:none;");
                beat.Attributes.Add("style", "display:none;");
            }
            else if (CType == "Potential Customer")
            {
                string smIDStr12 = "", smIDStr = "";
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
                pgroup.Attributes.Add("style", "display:none;");
                pronme.Attributes.Add("style", "display:none;");
                beat.Attributes.Add("style", "display:block;");
            }

        }



        public SQL.DataSet TopProductSummary()
        {
            SQL.DataSet ds = new SQL.DataSet();
            SQL.DataTable dtProducts = new SQL.DataTable();

            SQL.DataTable dtProductsQtyDetails = new SQL.DataTable();
            SQL.DataTable dtProductsAmtDetails = new SQL.DataTable();
            try
            {
                //--------------------Read data from SQL Server------------------------------------
                // string conString = "Data Source=103.121.204.34,3314;Initial Catalog=TestMergeFieldV2;user id=TestMergeFieldV2; pwd=TestMergeFieldV2!@#123;";
                StringBuilder query = new StringBuilder();

                StringBuilder query1 = new StringBuilder();
                StringBuilder query2 = new StringBuilder();

                string smIDStr = "", smIDStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
                string order = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                ViewState["smIDStr1"] = smIDStr1;

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
                { QueryMatGrp = " and i.ItemId in (" + matProStrNew + ") "; }
                if (matGrpStrNew != "" && matProStrNew == "")
                { QueryMatGrp = " and pg.ItemId in (" + matGrpStrNew + ") "; }

                Qrychk = " where os.VDate>='" + txtfmDate.Text + "' and os.VDate<='" + txttodate.Text + "'";

                if (smIDStr1 != "")
                {
                    Qrychk = Qrychk + " and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                }
                ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                ViewState["Todate"] = Settings.dateformat(txttodate.Text);

                if (orderType == "T") order = "desc";
                else if (orderType == "B") order = "asc";
                else order = "desc";

                query.Append("SELECT TOP " + txt_noofrecords.Text + " ProductGroup,itemid,Product,ProductClass,PriceGroup,Segment,SyncId,unit,Mrp,Rp,StdPack,Dp,convert(numeric(18,2),sum(Qty)) [Qty],convert(numeric(18,2), sum(Amount)) [Amount] FROM(SELECT pg.ItemName [ProductGroup],i.itemid,i.ItemName [Product],Mic.Name as ProductClass,i.PriceGroup,Mis.Name as Segment,i.SyncId,i.unit,i.Mrp,i.Rp,i.StdPack,i.Dp,convert(int,(os.Qty)) [Qty],convert(decimal, (os.Qty * os.Rate)) [Amount] From  TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId ");
                query.Append("left JOIN MastItem pg ON pg.ItemId = i.Underid left join MastItemClass Mic on i.ClassId=Mic.Id left Join MastItemSegment Mis on i.SegmentId=Mis.Id " + Qrychk + " " + QueryMatGrp + " Union All SELECT pg.ItemName [ProductGroup],i.itemid,i.ItemName [Product],Mic.Name as ProductClass,i.PriceGroup,Mis.Name as Segment,i.SyncId,i.unit,i.Mrp,i.Rp,i.StdPack,i.Dp,convert(int,(os.Qty)) [Qty],convert(decimal, (os.Qty * os.Rate)) [Amount] From Temp_TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId ");
                query.Append("left JOIN MastItem pg ON pg.ItemId = i.Underid left join MastItemClass Mic on i.ClassId=Mic.Id left Join MastItemSegment Mis on i.SegmentId=Mis.Id " + Qrychk + " " + QueryMatGrp + ") a  GROUP BY a.ItemId, a.ProductGroup, a.Product,a.ProductClass,a.PriceGroup,a.Segment,a.SyncId,a.unit,a.Mrp,a.Rp,a.StdPack,a.Dp Order by sum(a.Amount) " + order + "");

                //string query = "SELECT TOP " + txt_noofrecords.Text + " ProductGroup, itemid, Product, ProductClass, PriceGroup, Segment, SyncId, unit, Mrp, Rp, StdPack, Dp, convert(numeric(18,2),sum(Qty)) [Qty], convert(numeric(18,2), sum(Amount)) [Amount] FROM(SELECT pg.ItemName [ProductGroup],i.itemid,i.ItemName [Product],Mic.Name as ProductClass,i.PriceGroup,Mis.Name as Segment,i.SyncId,i.unit,i.Mrp,i.Rp,i.StdPack,i.Dp,convert(int,(os.Qty)) [Qty],convert(decimal, (os.Qty * os.Rate)) [Amount] From  TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId ";
                //query = query + "left JOIN MastItem pg ON pg.ItemId = i.Underid left join MastItemClass Mic on i.ClassId=Mic.Id left Join MastItemSegment Mis on i.SegmentId=Mis.Id " + Qrychk + " " + QueryMatGrp + " Union All SELECT pg.ItemName [ProductGroup],i.itemid,i.ItemName [Product],convert(int,(os.Qty)) [Qty],convert(decimal, (os.Qty * os.Rate)) [Amount] From Temp_TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId ";
                //query = query + "left JOIN MastItem pg ON pg.ItemId = i.Underid " + Qrychk + " " + QueryMatGrp + ") a  GROUP BY a.ItemId, a.ProductGroup, a.Product Order by sum(a.Amount) " + order + "";


                using (SqlConnection cn = new SqlConnection(conString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query.ToString(), cn))
                    {
                        da.Fill(dtProducts);
                        if (dtProducts.Rows.Count > 0)
                        {

                            string inames = "";
                            string i_id = "";
                            SQL.DataTable ItemNames = new SQL.DataTable();
                            for (int i = 0; i < dtProducts.Rows.Count; i++)
                            {

                                ItemNames = DbConnectionDAL.GetDataTable(CommandType.Text, "Select ItemName from MastItem where ItemId in (" + dtProducts.Rows[i]["itemid"].ToString() + ") ");

                                inames = inames + ",[" + ItemNames.Rows[0]["ItemName"].ToString() + "]";
                                i_id = i_id + "," + dtProducts.Rows[i]["itemid"].ToString();
                            }

                            ItemNames.Dispose();

                            query1.Append("select  * from (SELECT TOP 99  mi.itemname,convert(numeric(18,2),sum(os.Qty)) AS qty,mp.PartyName as Retailer,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName as State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId left join mastitem mi on mi.itemid=os.Itemid WHERE os.ItemId in (" + i_id.Trim(',') + ") and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)  GROUP BY mp.PartyName,mi.itemname,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName  Union ALl SELECT TOP 99 mi.itemname,convert(int,sum(os.Qty)) AS qty,mp.PartyName,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName as State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId  FROM Temp_TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId left join mastitem mi on mi.itemid=os.Itemid WHERE os.ItemId in (" + i_id.Trim(',') + ")  and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)  GROUP BY mp.PartyName,mi.itemname,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName  ) s PIVOT (SUM(qty) FOR itemname in (" + inames.Trim(',') + ")) as PivotTable");

                            query2.Append("select  * from (SELECT TOP 99  mi.itemname,convert(numeric(18,2),sum(os.Qty*os.Rate)) AS amount,mp.PartyName as Retailer,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName as State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId  FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId left join mastitem mi on mi.itemid=os.Itemid WHERE os.ItemId in (" + i_id.Trim(',') + ") and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)  GROUP BY mp.PartyName,mi.itemname,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName  Union ALl SELECT TOP 99 mi.itemname,convert(decimal,sum(os.Qty*os.Rate)) AS amount,mp.PartyName,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName as State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId  FROM Temp_TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId left join mastitem mi on mi.itemid=os.Itemid WHERE os.ItemId in (" + i_id.Trim(',') + ")  and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)  GROUP BY mp.PartyName,rate,mi.itemname,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName  ) s PIVOT (SUM(amount) FOR itemname in (" + inames.Trim(',') + ")) as PivotTable");

                            using (SqlDataAdapter da1 = new SqlDataAdapter(query1.ToString(), cn))
                            {
                                da1.Fill(dtProductsQtyDetails);

                            }

                            using (SqlDataAdapter da2 = new SqlDataAdapter(query2.ToString(), cn))
                            {
                                da2.Fill(dtProductsAmtDetails);

                            }

                            ds.Tables.Add(dtProductsAmtDetails);
                            ds.Tables.Add(dtProductsQtyDetails);
                            ds.Tables.Add(dtProducts);
                        }
                    }



                }

                dtProducts.Dispose();
                dtProductsQtyDetails.Dispose();
                dtProductsAmtDetails.Dispose();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return ds;
        }

        public void TopProductExcel(SQL.DataSet ds)
        {

            if (ds.Tables.Count > 0)
            {
                SQL.DataTable dt = new SQL.DataTable();
                SQL.DataTable dtProducts = new SQL.DataTable();
                //----------------------Create Excel objects--------------------------------
                Excel.Application oXL;
                Excel._Workbook oWB;
                Excel._Worksheet oSheet;

                oXL = new Excel.Application();
                //oXL.Visible = true;

                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                for (int k = 0; k < ds.Tables.Count; k++)
                {
                    dt = ds.Tables[k];
                    if (dt.Rows.Count == 0 && k == 2)
                    {
                        oWB.Close();
                        oXL.Quit();
                        dt.Dispose();
                        dtProducts.Dispose();
                        ds.Dispose();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found.');", true);
                        return;

                    }
                    dtProducts = new SQL.DataTable();

                    oSheet = (Excel._Worksheet)oXL.Worksheets.Add();

                    //-------------------Create Category wise excel Worksheet------------------------------
                    if (k == 2)
                    {
                        oSheet.Name = "Top Product Details";
                        dtProducts = dt.Copy();
                        dtProducts.Columns.Remove("itemid");
                        dtProducts.Columns.Remove("PriceGroup");
                        dtProducts.Columns.Remove("Mrp");
                        dtProducts.Columns.Remove("Rp");
                        dtProducts.Columns.Remove("StdPack");
                        dtProducts.Columns.Remove("Dp");
                        dtProducts.AcceptChanges();
                    }
                    else if (k == 1)
                    {
                        oSheet.Name = "Top Product Qty Retailer Wise";
                        dtProducts = dt.Copy();
                        dtProducts.AcceptChanges();
                    }

                    else if (k == 0)
                    {
                        oSheet.Name = "Top Product Amt Retailer Wise";
                        dtProducts = dt.Copy();

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
                    //range.FreezePanes();

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


                    if (k == 2)
                    {
                        oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
                 Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    }
                    else if (k == 1)
                    {
                        oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
                 Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    }
                    else if (k == 0)
                    {
                        oSheet.get_Range("G2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
                 Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    }

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
                string filename = "TopProducts_" + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
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
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                ds.Dispose();
            }
        }


        public SQL.DataSet TopDistributorSummary()
        {
            SQL.DataSet ds = new SQL.DataSet();
            SQL.DataTable dtDistributor = new SQL.DataTable();
            string des = "";
            SQL.DataTable dtDistributorSalesPersonTransVisitDetails = new SQL.DataTable();
            SQL.DataTable dtDistributorSalesPersonTransFailedVisitDetails = new SQL.DataTable();
            SQL.DataTable dtDistributorSalesPersonTransPurchOrderVisitDetails = new SQL.DataTable();
            SQL.DataTable dtDistributorProductQtyDetails = new SQL.DataTable();
            SQL.DataTable dtDistributorProductAmtDetails = new SQL.DataTable();
            try
            {
                //--------------------Read data from SQL Server------------------------------------
                // string conString = "Data Source=103.121.204.34,3314;Initial Catalog=TestMergeFieldV2;user id=TestMergeFieldV2; pwd=TestMergeFieldV2!@#123;";
                StringBuilder query = new StringBuilder();
                StringBuilder query1 = new StringBuilder();
                StringBuilder query2 = new StringBuilder();
                StringBuilder query3 = new StringBuilder();
                StringBuilder query4 = new StringBuilder();
                StringBuilder query5 = new StringBuilder();

                string smIDStr = "", smIDStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
                string order = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                ViewState["smIDStr1"] = smIDStr1;

                Qrychk = " and om.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and om.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                if (smIDStr1 != "")
                {
                    Qrychk = Qrychk + " and om.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                }

                ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                ViewState["Todate"] = Settings.dateformat(txttodate.Text);
                query.Append("Select  top " + txt_noofrecords.Text + " visit, distributor_id as Distributor_Id ,[DistName] as Distributor,Email,Mobile,ContactPerson,SyncId ,[areaid], max(State) as State,max(DCity) [City] , max([DArea]) [Area],max(Beat) as Beat ,sum([DSale]) [DistributorSecondarySale] , sum([primsale]) [Distributorprimarysale]  From ( ");
                query.Append(" select distinct da.PartyId as Distributor_Id,da.PartyName as [DistName],da.Email,da.Mobile,da.ContactPerson,da.SyncId,vg.stateName as State,da.areaid,a.AreaName as DArea, ma.AreaName as DCity,ma2.AreaName as Beat,convert(decimal, sum(om.Qty * om.Rate)) [DSale],0 AS primsale,dbo.getVisitCount(da.PartyId,'" + Settings.dateformat(txtfmDate.Text) + "','" + Settings.dateformat(txttodate.Text) + "') as visit from TransOrder1 om left join MastParty da on da.PartyId=om.DistId left join mastarea ma2 on ma2.areaid =da.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId left join MastArea a on a.AreaId=da.AreaId ");
                query.Append(" LEFT JOIN mastarea ma ON ma.AreaId=a.UnderId WHERE da.PartyDist=1 " + Qrychk + " group by da.PartyId,da.areaid,da.PartyName,a.AreaName,ma.AreaName,da.Email,da.Mobile,da.ContactPerson,da.SyncId,ma2.AreaName,vg.stateName");
                query.Append(" union all select DISTINCT ts1.DistId as Distributor_Id,d.PartyName as [DistName],d.Email,d.Mobile,d.ContactPerson,d.SyncId,vg.stateName as State,d.areaid,'' AS DArea, '' AS DCity,ma2.AreaName as Beat,0 AS [DSale],SUM(ts1.Amount) AS primsale,0 as visit  FROM TransDistInv1 ts1 left join MastParty d on d.PartyId=ts1.DistId left join mastarea ma2 on ma2.areaid =d.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId ");
                query.Append(" WHERE d.PartyDist=1 AND ts1.VDate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' and ts1.DistId in (select da.PartyId from MastParty da where da.AreaId in (select distinct ua.LinkCode from MastLink ua where ua.LinkCode in (select  distinct ua.LinkCode from MastLink ua  where ua.PrimCode in  (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1))) ) ");
                query.Append(" group by ts1.DistId ,d.areaid,d.PartyName,d.Email,d.Mobile,d.ContactPerson,d.SyncId,ma2.AreaName,vg.stateName) main group by distributor_id,[DistName],[areaid],visit,Email,Mobile,ContactPerson,SyncId order by DistributorSecondarySale desc,Distributor");



                using (SqlConnection cn = new SqlConnection(conString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query.ToString(), cn))
                    {
                        da.Fill(dtDistributor);


                        string dnames = "";
                        string d_id = "";
                        string a_id = "";
                        SQL.DataTable DistributorNames = new SQL.DataTable();
                        for (int i = 0; i < dtDistributor.Rows.Count; i++)
                        {

                            DistributorNames = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName from MastParty where PartyId in (" + dtDistributor.Rows[i]["Distributor_Id"].ToString() + ") ");

                            dnames = dnames + ",[" + DistributorNames.Rows[0]["PartyName"].ToString() + "]";
                            d_id = d_id + "," + dtDistributor.Rows[i]["Distributor_Id"].ToString();
                            a_id = a_id + "," + dtDistributor.Rows[i]["areaid"].ToString();
                        }

                        DistributorNames.Dispose();
                        string str_des = "";

                        string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                        SQL.DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtdesig.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtdesig.Rows.Count; i++)
                            {
                                str_des = str_des + ",' ' as [" + dtdesig.Rows[i]["DesName"].ToString() + "]";
                                des = des + "," + dtdesig.Rows[i]["DesName"].ToString() ;
                            }
                        }

                        dtdesig.Dispose();

                        des = des.Trim(',');
                        Qrychk = " and VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                        query1.Append("select  * from (SELECT mp.PartyName as Distributor" + str_des + ",ms.smid,ms.SMName as SalesPersonName,ms.Mobile,ms.SyncId,(Case ms.Active when 1 then 'Yes' else 'No' end) as Status,md.desname as Designation,mh.HeadquarterName as HeadQuarter,Replace(Convert(varchar,ms.Joining,106), ' ', '/') as Joining,Replace(Convert(varchar,ms.Leaving,106), ' ', '/') as Leaving,Replace(Convert(varchar,VDate,106), ' ', '/') as VDate FROM TransVisitDist om LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId LEFT JOIN mastparty mp ON om.DistId=mp.PartyId left join mastlogin ml on ml.id=ms.userid left join mastdesignation md on md.desid=ml.desigid left join MastHeadquarter mh on mh.id=ms.HeadquarterId WHERE DistId in (" + d_id.Trim(',') + ")  " + Qrychk + ") s PIVOT(Max(VDate) FOR Distributor in (" + dnames.Trim(',') + ")) as PivotTable");

                        query2.Append("select  * from (SELECT mp.PartyName as Distributor" + str_des + ",ms.smid,ms.SMName as SalesPersonName,ms.Mobile,ms.SyncId,(Case ms.Active when 1 then 'Yes' else 'No' end) as Status,md.desname as Designation,mh.HeadquarterName as HeadQuarter,Replace(Convert(varchar,ms.Joining,106), ' ', '/') as Joining,Replace(Convert(varchar,ms.Leaving,106), ' ', '/') as Leaving,Replace(Convert(varchar,VDate,106), ' ', '/') as VDate FROM TransFailedVisit om LEFT JOIN mastparty mp ON om.PartyId=mp.PartyId  LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId left join mastlogin ml on ml.id=ms.userid left join mastdesignation md on md.desid=ml.desigid left join MastHeadquarter mh on mh.id=ms.HeadquarterId WHERE mp.PartyId in (" + d_id.Trim(',') + ")  " + Qrychk + ") s PIVOT(Max(VDate) FOR Distributor in (" + dnames.Trim(',') + ")) as PivotTable");
                        query3.Append("select  * from (SELECT mp.PartyName as Distributor" + str_des + ",ms.smid,ms.SMName as SalesPersonName,ms.Mobile,ms.SyncId,(Case ms.Active when 1 then 'Yes' else 'No' end) as Status,md.desname as Designation,mh.HeadquarterName as HeadQuarter,Replace(Convert(varchar,ms.Joining,106), ' ', '/') as Joining,Replace(Convert(varchar,ms.Leaving,106), ' ', '/') as Leaving,Replace(Convert(varchar,VDate,106), ' ', '/') as VDate FROM TransPurchOrder om LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId LEFT JOIN mastparty mp ON om.DistId=mp.PartyId left join mastlogin ml on ml.id=ms.userid left join mastdesignation md on md.desid=ml.desigid left join MastHeadquarter mh on mh.id=ms.HeadquarterId WHERE DistId in (" + d_id.Trim(',') + ")  " + Qrychk + ") s PIVOT(Max(VDate) FOR Distributor in (" + dnames.Trim(',') + ")) as PivotTable");

                        string strquery = "SELECT distinct TOP 99 i.ItemName [Product]  FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.AreaId in (" + a_id.Trim(',') + ") and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemName ";
                        SQL.DataTable ProductNames = DbConnectionDAL.GetDataTable(CommandType.Text, strquery);

                        string inames = "";
                        for (int i = 0; i < ProductNames.Rows.Count; i++)
                        {
                            inames = inames + ",[" + ProductNames.Rows[i]["Product"].ToString() + "]";
                        }


                        ProductNames.Dispose();

                        query4.Append(" select  * from (SELECT TOP 99  mp.PartyName as Distributor,mp.Email,mp.Mobile,mp.ContactPerson,vg.StateName as State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId,i.ItemName [Product], convert(numeric(18,2),sum(os.Qty)) [Qty]  FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left join mastparty mp on mp.partyid=os.distid left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.AreaId in (" + a_id.Trim(',') + ") and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate, mp.PartyName,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.StateName Order by sum(os.Qty * os.Rate) DESC ) s PIVOT (Sum(Qty) FOR Product in (" + inames.Trim(',') + ")) as PivotTable");


                        query5.Append(" select  * from (SELECT TOP 99  mp.PartyName as Distributor,mp.Email,mp.Mobile,mp.ContactPerson,vg.StateName as State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId,i.ItemName [Product],convert(numeric(18,2), sum(os.Qty * os.Rate)) [Amount]  FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left join mastparty mp on mp.partyid=os.distid left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.AreaId in (" + a_id.Trim(',') + ") and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate, mp.PartyName, mp.PartyName,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.StateName  Order by sum(os.Qty * os.Rate) DESC ) s PIVOT (Sum(Amount) FOR Product in (" + inames.Trim(',') + ")) as PivotTable");

                    }

                    using (SqlDataAdapter da1 = new SqlDataAdapter(query1.ToString(), cn))
                    {
                        da1.Fill(dtDistributorSalesPersonTransVisitDetails);

                        SQL.DataTable dtsr = new SQL.DataTable();
                        for (int i = 0; i < dtDistributorSalesPersonTransVisitDetails.Rows.Count; i++)
                        {
                            string str = "select msp.SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtDistributorSalesPersonTransVisitDetails.Rows[i]["smid"].ToString() + ")";
                            dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    dtDistributorSalesPersonTransVisitDetails.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();

                                }
                            }

                            string[] arr = des.Split(',');
                            for (int l = 0; l < arr.Length; l++)
                            {
                                if (dtDistributorSalesPersonTransVisitDetails.Rows[i][arr[l]].ToString() == " ")
                                {
                                    dtDistributorSalesPersonTransVisitDetails.Rows[i][arr[l]] = "Vacant";
                                }
                            }
                        }

                        dtsr.Dispose();



                    }

                    using (SqlDataAdapter da2 = new SqlDataAdapter(query2.ToString(), cn))
                    {
                        da2.Fill(dtDistributorSalesPersonTransFailedVisitDetails);
                        SQL.DataTable dtsr = new SQL.DataTable();
                        for (int i = 0; i < dtDistributorSalesPersonTransFailedVisitDetails.Rows.Count; i++)
                        {
                            string str = "select msp.SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtDistributorSalesPersonTransFailedVisitDetails.Rows[i]["smid"].ToString() + ")";
                            dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    dtDistributorSalesPersonTransFailedVisitDetails.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();

                                }
                            }

                            string[] arr = des.Split(',');
                            for (int l = 0; l < arr.Length; l++)
                            {
                                if (dtDistributorSalesPersonTransFailedVisitDetails.Rows[i][arr[l]].ToString() == " ")
                                {
                                    dtDistributorSalesPersonTransFailedVisitDetails.Rows[i][arr[l]] = "Vacant";
                                }
                            }
                        }

                        dtsr.Dispose();
                    }

                    using (SqlDataAdapter da3 = new SqlDataAdapter(query3.ToString(), cn))
                    {
                        da3.Fill(dtDistributorSalesPersonTransPurchOrderVisitDetails);
                        SQL.DataTable dtsr = new SQL.DataTable();
                        for (int i = 0; i < dtDistributorSalesPersonTransPurchOrderVisitDetails.Rows.Count; i++)
                        {
                            string str = "select msp.SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtDistributorSalesPersonTransPurchOrderVisitDetails.Rows[i]["smid"].ToString() + ")";
                            dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    dtDistributorSalesPersonTransPurchOrderVisitDetails.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();

                                }
                            }

                            string[] arr = des.Split(',');
                            for (int l = 0; l < arr.Length; l++)
                            {
                                if (dtDistributorSalesPersonTransPurchOrderVisitDetails.Rows[i][arr[l]].ToString() == " ")
                                {
                                    dtDistributorSalesPersonTransPurchOrderVisitDetails.Rows[i][arr[l]] = "Vacant";
                                }
                            }
                        }

                        dtsr.Dispose();

                    }

                    using (SqlDataAdapter da4 = new SqlDataAdapter(query4.ToString(), cn))
                    {
                        da4.Fill(dtDistributorProductQtyDetails);

                    }

                    using (SqlDataAdapter da5 = new SqlDataAdapter(query5.ToString(), cn))
                    {
                        da5.Fill(dtDistributorProductAmtDetails);

                    }

                    ds.Tables.Add(dtDistributorProductAmtDetails);
                    ds.Tables.Add(dtDistributorProductQtyDetails);
                    ds.Tables.Add(dtDistributorSalesPersonTransPurchOrderVisitDetails);
                    ds.Tables.Add(dtDistributorSalesPersonTransFailedVisitDetails);
                    ds.Tables.Add(dtDistributorSalesPersonTransVisitDetails);
                    ds.Tables.Add(dtDistributor);

                }

                dtDistributor.Dispose();
                dtDistributorSalesPersonTransVisitDetails.Dispose();
                dtDistributorSalesPersonTransFailedVisitDetails.Dispose();
                dtDistributorSalesPersonTransPurchOrderVisitDetails.Dispose();
                dtDistributorProductQtyDetails.Dispose();
                dtDistributorProductAmtDetails.Dispose();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return ds;
        }


        public void TopDistributorExcel(SQL.DataSet ds)
        {

            SQL.DataTable dt = new SQL.DataTable();
            SQL.DataTable dtProducts = new SQL.DataTable();
            //----------------------Create Excel objects--------------------------------
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            oXL = new Excel.Application();
            // oXL.Visible = true;

            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            for (int k = 0; k < ds.Tables.Count; k++)
            {
                dt = ds.Tables[k];
                if (dt.Rows.Count == 0 && k == 5)
                {
                    oWB.Close();
                    oXL.Quit();
                    dt.Dispose();
                    dtProducts.Dispose();
                    ds.Dispose();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found.');", true);
                    return;

                }
                dtProducts = new SQL.DataTable();

                oSheet = (Excel._Worksheet)oXL.Worksheets.Add();

                //-------------------Create Category wise excel Worksheet------------------------------
                if (k == 5)
                {
                    oSheet.Name = "Top Distributor Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("visit");
                    dtProducts.Columns.Remove("Distributor_Id");
                    dtProducts.Columns.Remove("areaid");
                    dtProducts.AcceptChanges();
                }
                if (k == 4)
                {
                    oSheet.Name = "SalesPerson TransVisit Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.AcceptChanges();
                }
                else if (k == 3)
                {
                    oSheet.Name = "SalesPerson TransFailedVisit";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.AcceptChanges();
                }
                if (k == 2)
                {
                    oSheet.Name = "SalesPerson TransPurchOrder";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.AcceptChanges();
                }
                else if (k == 1)
                {
                    oSheet.Name = "Top Dist. Qty Product Wise";
                    dtProducts = dt.Copy();
                    dtProducts.AcceptChanges();
                }

                else if (k == 0)
                {
                    oSheet.Name = "Top Dist. Amt Product Wise";
                    dtProducts = dt.Copy();
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

                if (k == 0)
                {
                    oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }
                else if (k == 1)
                {
                    oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }
                else if (k == 5)
                {
                    oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }

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

            //--------------------------Save Product Excel sheet-------------------------------------
            // oXL.Visible = true;
            oXL.UserControl = true;
            string filename = "TopDistributor_" + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
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


        public SQL.DataSet TopRetailerSummary()
        {
            SQL.DataSet ds = new SQL.DataSet();
            SQL.DataTable dtRetailers = new SQL.DataTable();
            SQL.DataTable dtRetailersQtyDetails = new SQL.DataTable();
            SQL.DataTable dtRetailersAvgRateDetails = new SQL.DataTable();
            SQL.DataTable dtRetailersAmtDetails = new SQL.DataTable();
            SQL.DataTable dtRetailersDateDetails = new SQL.DataTable();
            try
            {
                //--------------------Read data from SQL Server------------------------------------
                //string conString = "Data Source=103.121.204.34,3314;Initial Catalog=TestMergeFieldV2;user id=TestMergeFieldV2; pwd=TestMergeFieldV2!@#123;";
                StringBuilder query = new StringBuilder();
                StringBuilder query1 = new StringBuilder();
                StringBuilder query2 = new StringBuilder();
                StringBuilder query3 = new StringBuilder();
                StringBuilder query4 = new StringBuilder();

                string smIDStr = "", smIDStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "", matBeatNew = "", Querybeat = "";
                string order = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                ViewState["smIDStr1"] = smIDStr1;

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

                foreach (ListItem Beat in Lstbeatbox.Items)
                {
                    if (Beat.Selected)
                    {
                        matBeatNew += Beat.Value + ",";
                    }
                }
                matBeatNew = matBeatNew.TrimStart(',').TrimEnd(',');


                if (matGrpStrNew != "" && matProStrNew != "")
                { QueryMatGrp = " and i.ItemId in (" + matProStrNew + ") "; }
                if (matGrpStrNew != "" && matProStrNew == "")
                { QueryMatGrp = " and pg.ItemId in (" + matGrpStrNew + ") "; }

                Qrychk = " where os.VDate>='" + txtfmDate.Text + "' and os.VDate<='" + txttodate.Text + "'";



                if (smIDStr1 != "")
                {
                    Qrychk = Qrychk + " and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                }

                if (matBeatNew != "")
                {
                    Querybeat = "AND ma.AreaId in (" + matBeatNew + ")";
                }
                ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                ViewState["Todate"] = Settings.dateformat(txttodate.Text);

                query.Append("SELECT TOP " + txt_noofrecords.Text + " mp.partyid, mp.PartyName as Retailer,Max(pt.PartyTypeName) As Type,Max(mp.Address1) As RetailerAddress,max(vg.stateName) AS State,max(vg.cityName) as City,max(vg.areaName) AS Area,Max(ma.AreaName) AS Beat,mp.Email,mp.Mobile,mp.ContactPerson,mp.SyncId,sum(isnull(os.Qty,0)*isnull(os.Rate,0)) AS Amount,Max(ms.SMName) As SalesPerson,ma.AreaId ");
                query.Append("FROM MastParty mp LEFT JOIN TransOrder1 os ON mp.PartyId=os.PartyId ");
                query.Append("Inner JOIN mastsalesrep ms ON ms.SMId=os.SMId LEFT JOIN PartyType pt ON pt.PartyTypeId=mp.PartyType ");
                query.Append("LEFT JOIN Mastarea ma ON ma.AreaId=mp.BeatId LEFT JOIN viewgeo vg ON vg.beatid=ma.AreaId " + Qrychk + " " + Querybeat + " GROUP BY mp.partyid, mp.PartyName,ma.AreaId,mp.Email,mp.Mobile,mp.ContactPerson,mp.SyncId ORDER BY Amount desc ");

                using (SqlConnection cn = new SqlConnection(conString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query.ToString(), cn))
                    {
                        da.Fill(dtRetailers);


                        string dnames = "";
                        string d_id = "";

                        for (int i = 0; i < dtRetailers.Rows.Count; i++)
                        {

                            // SQL.DataTable PartyNames = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName from MastParty where PartyId in (" + dtRetailers.Rows[i]["partyid"].ToString() + ") ");

                            dnames = dnames + ",[" + dtRetailers.Rows[i]["Retailer"].ToString() + "]";
                            d_id = d_id + "," + dtRetailers.Rows[i]["partyid"].ToString();
                        }


                        string strquery = "SELECT distinct TOP 99 i.ItemName [Product] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.PartyId in (" + d_id.Trim(',') + ") and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate ";
                        SQL.DataTable ProductNames = DbConnectionDAL.GetDataTable(CommandType.Text, strquery);

                        string inames = "";
                        for (int i = 0; i < ProductNames.Rows.Count; i++)
                        {
                            inames = inames + ",[" + ProductNames.Rows[i]["Product"].ToString() + "]";
                        }

                        ProductNames.Dispose();

                        query1.Append("select  * from (SELECT TOP 99 mp.PartyName as Retailer,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName AS State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId,i.ItemName [Product], convert(numeric(18,2),sum(os.Qty)) [Qty] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left join mastparty mp on mp.partyid=os.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid  LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.PartyId in (" + d_id.Trim(',') + ") and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate,mp.PartyName ,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName Order by sum(os.Qty * os.Rate) DESC ) s PIVOT (Sum(Qty) FOR Product in (" + inames.Trim(',') + ")) as PivotTable");

                        query2.Append("select  * from (SELECT TOP 99 mp.PartyName as Retailer,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName AS State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId,i.ItemName [Product],  ((sum(os.Qty * os.Rate))/(convert(numeric(18,2),sum(os.Qty)))) as  [rate] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left join mastparty mp on mp.partyid=os.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid  LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.PartyId in (" + d_id.Trim(',') + ") and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate,mp.PartyName ,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName Order by sum(os.Qty * os.Rate) DESC ) s PIVOT (Sum(rate) FOR Product in (" + inames.Trim(',') + ")) as PivotTable");

                        query3.Append("select  * from (SELECT TOP 99 mp.PartyName as Retailer,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName AS State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId,i.ItemName [Product], convert(numeric(18,2), sum(os.Qty * os.Rate)) [Amount] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left join mastparty mp on mp.partyid=os.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid  LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.PartyId in (" + d_id.Trim(',') + ") and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate,mp.PartyName ,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName Order by sum(os.Qty * os.Rate) DESC ) s PIVOT (Sum(Amount) FOR Product in (" + inames.Trim(',') + ")) as PivotTable");

                        query4.Append(" select  * from (SELECT TOP 99  mp.PartyName as Retailer,mp.Email,mp.Mobile,mp.ContactPerson,vg.stateName AS State,ma1.AreaName as City,ma.AreaName as Area,ma2.AreaName as Beat,mp.SyncId,i.ItemName [Product],  Replace(Convert(varchar,os.VDate,106), ' ', '/') [VDate] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left join mastparty mp on mp.partyid=os.PartyId left join mastarea ma on ma.areaid =mp.areaid left join mastarea ma1 on ma1.areaid =mp.cityid left join mastarea ma2 on ma2.areaid =mp.beatid  LEFT JOIN viewgeo vg ON vg.beatid=ma2.AreaId where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "' AND os.PartyId in (" + d_id.Trim(',') + ")  and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate,vdate,mp.PartyName ,mp.Email,mp.Mobile,mp.ContactPerson,ma1.AreaName,ma.AreaName,ma2.AreaName,mp.SyncId,vg.stateName Order by os.VDate DESC ) s PIVOT (Max(VDate) FOR Product in (" + inames.Trim(',') + ")) as PivotTable");

                    }

                    using (SqlDataAdapter da1 = new SqlDataAdapter(query1.ToString(), cn))
                    {
                        da1.Fill(dtRetailersQtyDetails);

                    }

                    using (SqlDataAdapter da2 = new SqlDataAdapter(query2.ToString(), cn))
                    {
                        da2.Fill(dtRetailersAvgRateDetails);

                    }

                    using (SqlDataAdapter da3 = new SqlDataAdapter(query3.ToString(), cn))
                    {
                        da3.Fill(dtRetailersAmtDetails);

                    }

                    using (SqlDataAdapter da4 = new SqlDataAdapter(query4.ToString(), cn))
                    {
                        da4.Fill(dtRetailersDateDetails);

                    }

                    ds.Tables.Add(dtRetailersDateDetails);
                    ds.Tables.Add(dtRetailersAmtDetails);
                    ds.Tables.Add(dtRetailersAvgRateDetails);
                    ds.Tables.Add(dtRetailersQtyDetails);
                    ds.Tables.Add(dtRetailers);

                }

                dtRetailers.Dispose();
                dtRetailersQtyDetails.Dispose();
                dtRetailersAvgRateDetails.Dispose();
                dtRetailersAmtDetails.Dispose();
                dtRetailersDateDetails.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return ds;
        }

        public void TopRetailerExcel(SQL.DataSet ds)
        {
            SQL.DataTable dt = new SQL.DataTable();
            SQL.DataTable dtProducts = new SQL.DataTable();
            //----------------------Create Excel objects--------------------------------
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            oXL = new Excel.Application();
            //oXL.Visible = true;

            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            for (int k = 0; k < ds.Tables.Count; k++)
            {
                dt = ds.Tables[k];
                if (dt.Rows.Count == 0 && k == 4)
                {
                    oWB.Close();
                    oXL.Quit();
                    dt.Dispose();
                    dtProducts.Dispose();
                    ds.Dispose();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found.');", true);
                    return;

                }
                dtProducts = new SQL.DataTable();

                oSheet = (Excel._Worksheet)oXL.Worksheets.Add();

                //-------------------Create Category wise excel Worksheet------------------------------
                if (k == 4)
                {
                    oSheet.Name = "Top Retailer Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("partyid");
                    dtProducts.Columns.Remove("AreaId");
                    dtProducts.Columns.Remove("SalesPerson");
                    dtProducts.AcceptChanges();
                }
                if (k == 3)
                {
                    oSheet.Name = "TR Product Qty Wise";
                    dtProducts = dt.Copy();
                    dtProducts.AcceptChanges();
                }
                else if (k == 2)
                {
                    oSheet.Name = "TR Product Avg. Rate Wise";
                    dtProducts = dt.Copy();
                    dtProducts.AcceptChanges();
                }
                else if (k == 1)
                {
                    oSheet.Name = "TR Product Amt Wise";
                    dtProducts = dt.Copy();
                    dtProducts.AcceptChanges();
                }
                else if (k == 0)
                {
                    oSheet.Name = "TR Product Date Wise";
                    dtProducts = dt.Copy();
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

                if (k == 0)
                {
                    oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }
                else if (k == 1)
                {
                    oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }
                else if (k == 2)
                {
                    oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }
                else if (k == 3)
                {
                    oSheet.get_Range("J2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }
                else if (k == 4)
                {
                    oSheet.get_Range("L2", "G" + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }

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
            //--------------------------Save Product Excel sheet-------------------------------------
            // oXL.Visible = true;
            oXL.UserControl = true;
            string filename = "TopRetailer_" + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
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

        public SQL.DataSet TopSalesPersonSummary()
        {
            SQL.DataSet ds = new SQL.DataSet();
            SQL.DataTable dtSalesPerson = new SQL.DataTable();
            string des = "";
            SQL.DataTable dtSalesPersonOrdQtyWise = new SQL.DataTable();
            SQL.DataTable dtSalesPersonOrdAmtWise = new SQL.DataTable();
            SQL.DataTable dtSalesPersonNewParty = new SQL.DataTable();
            SQL.DataTable dtSalesPersonTotalCalls = new SQL.DataTable();
            SQL.DataTable dtSalesPersonProdCalls = new SQL.DataTable();

            try
            {
                //--------------------Read data from SQL Server------------------------------------
                //string conString = "Data Source=103.121.204.34,3314;Initial Catalog=TestMergeFieldV2;user id=TestMergeFieldV2; pwd=TestMergeFieldV2!@#123;";
                StringBuilder query = new StringBuilder();
                StringBuilder query1 = new StringBuilder();
                StringBuilder query2 = new StringBuilder();
                StringBuilder query3 = new StringBuilder();
                StringBuilder query4 = new StringBuilder();
                StringBuilder query5 = new StringBuilder();

                string smIDStr = "", smIDStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "", Qrychk1 = "";
                string order = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                ViewState["smIDStr1"] = smIDStr1;

                Qrychk = " where d.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and d.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                Qrychk = Qrychk + " and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                ViewState["Todate"] = Settings.dateformat(txttodate.Text);
                Qrychk1 = " d.created_date>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and d.created_date<='" + Settings.dateformat(txttodate.Text) + " 23:59'";


                string str_des = "";

                string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                SQL.DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtdesig.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        str_des = str_des + ",' ' as [" + dtdesig.Rows[i]["DesName"].ToString() + "]";
                        des = des + "," + dtdesig.Rows[i]["DesName"].ToString() ;
                    }
                }

                dtdesig.Dispose();
                des = des.Trim(',');


                query.Append("SELECT top " + txt_noofrecords.Text + " " + str_des.Trim(',') + ",  cp1.smname as ReportingManager,  cp.smid,Isnull(cp.smname,'') [SalesPersonName],cp.Mobile,cp.SyncId,(Case cp.Active when 1 then 'Active' else 'InActive' end) as Status,md.desname as Designation,mh.HeadquarterName as HeadQuarter,ma.areaname as City,Replace(Convert(varchar,cp.Joining,106), ' ', '/') as DateOfJoining,Replace(Convert(varchar,cp.Leaving,106), ' ', '/') as DateOfLeaving, sum([RecOrdr]) [OrderAmount],sum([TotalNewParty]) [TotalNewParty], ");
                query.Append("sum([TotalCalls]) [TotalCalls],sum([ProdCalls]) [ProdCalls] FROM ( ");

                query.Append("SELECT d.smid,0 as [RecOrdr],0 [TotalNewParty], 0 [TotalCalls], 0 [ProdCalls] FROM transpurchorder d Left Join transpurchorder1 tp on d.PODocId=tp.podocid  " + Qrychk + " GROUP BY d.smid UNION ALL ");

                query.Append("SELECT d.smid,sum(d.Amount) [RecOrdr],0 [TotalNewParty],");
                query.Append(" 0 [TotalCalls], 0 [ProdCalls] FROM Transorder1 d  " + Qrychk + " GROUP BY d.smid ");

                query.Append(" UNION ALL SELECT ms.SMId, 0 [RecOrdr], count(d.PartyId) [TotalNewParty],0 [TotalCalls],0 [ProdCalls] FROM MastParty d ");
                query.Append(" LEFT JOIN mastsalesrep ms ON d.Created_User_id=ms.UserId WHERE  " + Qrychk1 + "  and ms.smid in (" + smIDStr1 + ")  and d.PartyDist=0 Group By ms.SMId ");

                query.Append(" UNION ALL SELECT mQ.smid, 0 [RecOrdr], 0 [TotalNewParty],sum([Count]) [CallsVisited],sum(proCount) [Retailer] from ( ");
                query.Append(" SELECT iQ.smid,isnull(sum([Count]), 0) [Count],isnull(sum([proCount]), 0) [proCount] FROM ( SELECT d.SMId,count(d.OrdDocId) [Count],count(d.OrdDocId) [ProCount] FROM TransOrder d LEFT JOIN MastParty p ON d.PartyId = p.PartyId");
                query.Append(" " + Qrychk + " GROUP BY d.SMId");

                query.Append(" UNION ALL SELECT d.SMId,(select case when Count(*)>1 then 1 else 1 end FROM TransDemo om1 where om1.VDate=d.VDate  and om1.smid in (" + smIDStr1 + ") ");

                query.Append(" and om1.PartyId=p.PartyId) as [Count],0 [ProCount] FROM TransDemo d LEFT JOIN MastParty p ON d.PartyId = p.PartyId");
                query.Append(" " + Qrychk + " GROUP BY d.SMId,d.VDate,p.PartyId ");

                query.Append(" union all  SELECT d.SMId,count(*) [Count],0 [ProCount] FROM TransFailedVisit d LEFT JOIN MastParty p ON d.PartyId = p.PartyId");
                query.Append(" " + Qrychk + " GROUP BY d.SMId) iQ GROUP BY smid ) mQ GROUP BY smid  ");

                query.Append(" ) a LEFT JOIN MastSalesRep cp ON a.smid = cp.SMId left join MastSalesRep cp1 on cp1.smid=cp.underid left join mastarea ma on ma.areaid=cp.cityid left join mastlogin ml on ml.id=cp.userid left join mastdesignation md on md.desid=ml.desigid left join MastHeadquarter mh on mh.id=cp.HeadquarterId GROUP BY cp.smname,cp.smid,mh.HeadquarterName,cp.Mobile,cp.SyncId,cp.Active,cp.Joining,cp.Leaving,md.desname,cp1.smname,ma.areaname  ORDER BY OrderAmount DESC");



                using (SqlConnection cn = new SqlConnection(conString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query.ToString(), cn))
                    {
                        da.Fill(dtSalesPerson);
                        string sid = "";
                        string snames = "";
                        SQL.DataTable dtsr = new SQL.DataTable();
                        for (int i = 0; i < dtSalesPerson.Rows.Count; i++)
                        {
                            if (!snames.Contains(dtSalesPerson.Rows[i]["SalesPersonName"].ToString()))
                                snames = snames + ",[" + dtSalesPerson.Rows[i]["SalesPersonName"].ToString() + "]";
                            sid = sid + "," + dtSalesPerson.Rows[i]["smid"].ToString();
                            str = "select msp.SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtSalesPerson.Rows[i]["smid"].ToString() + ")";
                            dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    dtSalesPerson.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                                }
                            }

                            string[] arr = des.Split(',');
                            for (int l = 0; l < arr.Length; l++)
                            {
                                if (dtSalesPerson.Rows[i][arr[l]].ToString() == " ")
                                {
                                    dtSalesPerson.Rows[i][arr[l]] = "Vacant";
                                }
                            }
                        }

                        dtsr.Dispose();

                        string strquery = "select * from(SELECT distinct mi.ItemName FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN MastSalesRep ms ON os.SMId=ms.smid LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId WHERE os.SMId in (" + sid.Trim(',') + ") )a ";
                        //and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "'
                        SQL.DataTable ProductNames = DbConnectionDAL.GetDataTable(CommandType.Text, strquery);

                        string inames = "";
                        for (int i = 0; i < ProductNames.Rows.Count; i++)
                        {
                            inames = inames + ",[" + ProductNames.Rows[i]["ItemName"].ToString() + "]";
                        }
                        ProductNames.Dispose();
                        inames = inames.Trim(',');

                        query1.Append("select  * from (select * from(SELECT Replace(Convert(varchar,os.VDate,106), ' ', '/') as [Date]," + str_des.Trim(',') + ",ms1.smname as ReportingManager,ms.smid,ms.SMName as SalesPersonName,ms.Mobile as SalesPersonMobile,ms.Email as SalesPersonEmail,ms.SyncId as SalePersonSyncId,(Case ms.Active when 1 then 'Yes' else 'No' end) as SalesPersonStatus,md.desname as SalesPersonDesignation,mh.HeadquarterName as SalesPersonHeadQuarter,ma.areaname as SalesPersonCity,Replace(Convert(varchar,ms.Joining,106), ' ', '/') as DateOfJoining,Replace(Convert(varchar,ms.Leaving,106), ' ', '/') as DateOfLeaving,mp.PartyName as RetailerName,vg.stateName AS RetailerState,vg.cityName AS RetailerCity,vg.areaName AS RetailerArea,b.AreaName as RetailerBeat,mp.SyncId as RetailerSyncId,mp.ContactPerson as RetailerContactPerson,mp.Mobile as RetailerMobile,mp.Email as RetailerEmail,mi.ItemName,os.Qty FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN MastSalesRep ms ON os.SMId=ms.smid left join MastSalesRep ms1 on ms1.smid=ms.underid left join mastarea ma on ma.areaid=ms.cityid left join mastlogin ml on ml.id=ms.userid left join mastdesignation md on md.desid=ml.desigid left join MastHeadquarter mh on mh.id=ms.HeadquarterId INNER JOIN mastarea b ON b.AreaId=mp.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId WHERE os.SMId in (" + sid.Trim(',') + ") and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "')a  ) s PIVOT (Sum(Qty) FOR ItemName in (" + inames.Trim(',') + ")) as PivotTable");

                        query2.Append("select  * from (select * from(SELECT Replace(Convert(varchar,os.VDate,106), ' ', '/') as [Date]," + str_des.Trim(',') + ",ms1.smname as ReportingManager,ms.smid,ms.SMName as SalesPersonName,ms.Mobile as SalesPersonMobile,ms.Email as SalesPersonEmail,ms.SyncId as SalePersonSyncId,(Case ms.Active when 1 then 'Yes' else 'No' end) as SalesPersonStatus,md.desname as SalesPersonDesignation,mh.HeadquarterName as SalesPersonHeadQuarter,ma.areaname as SalesPersonCity,Replace(Convert(varchar,ms.Joining,106), ' ', '/') as DateOfJoining,Replace(Convert(varchar,ms.Leaving,106), ' ', '/') as DateOfLeaving,mp.PartyName as RetailerName,vg.stateName AS RetailerState,vg.cityName AS RetailerCity,vg.areaName AS RetailerArea,b.AreaName as RetailerBeat,mp.SyncId as RetailerSyncId,mp.ContactPerson as RetailerContactPerson,mp.Mobile as RetailerMobile,mp.Email as RetailerEmail,mi.ItemName,os.amount FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN MastSalesRep ms ON os.SMId=ms.smid left join MastSalesRep ms1 on ms1.smid=ms.underid left join mastarea ma on ma.areaid=ms.cityid left join mastlogin ml on ml.id=ms.userid left join mastdesignation md on md.desid=ml.desigid left join MastHeadquarter mh on mh.id=ms.HeadquarterId INNER JOIN mastarea b ON b.AreaId=mp.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId WHERE os.SMId in (" + sid.Trim(',') + ") and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "')a  ) s PIVOT (Sum(amount) FOR ItemName in (" + inames.Trim(',') + ")) as PivotTable");

                        //query2.Append("select  * from (select * from(SELECT os.VDate,ms.SMName,mp.PartyName,mi.ItemName,os.amount FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN MastSalesRep ms ON os.SMId=ms.smid LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId WHERE os.SMId in (" + sid.Trim(',') + ") and os.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and os.VDate<='" + Settings.dateformat(txttodate.Text) + "')a  ) s PIVOT (Sum(amount) FOR ItemName in (" + inames.Trim(',') + ")) as PivotTable");






                        string Qty1 = "";
                        Qrychk = " and P.Created_Date between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

                        Qty1 = Qty1 + " and p.Created_User_id in ( SELECT userid FROM MastSalesRep ms LEFT JOIN mastlogin ml ON ms.UserId=ml.Id WHERE ms.SMId IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + sid.Trim(',') + ")) and Active=1)";

                        //strquery = "select distinct max (a.[By]) as [CreatedBy] from (select distinct p.PartyId, p.PartyName as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,max(vg.stateName) AS State,max(vg.cityName) as City,max(vg.areaName) AS Area,max(b.AreaName) AS beat,max(p.Email) as Email,max(p.ContactPerson) as ContactPerson,max(p.SyncId) as SyncId,max(ms.SMName) AS [By],(SELECT sum(OrderAmount) FROM TransOrder WHERE PartyId=p.PartyId) AS Business,0 AS Pendency,max(p.created_date) as Date FROM MastParty p left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id WHERE p.partydist=0 " + Qrychk + " " + Qty1 + " ) group by p.PartyName,p.PartyId  ) a Group by a.PartyId,a.Name,a.Address,a.Mobile,a.Email,a.ContactPerson,a.SyncId";
                        //SQL.DataTable SNames = DbConnectionDAL.GetDataTable(CommandType.Text, strquery);

                        //string snames = "";
                        //for (int i = 0; i < SNames.Rows.Count; i++)
                        //{
                        //    snames = snames + ",[" + SNames.Rows[i]["CreatedBy"].ToString() + "]";
                        //}


                        query3.Append("select  * from (select a.PartyId,a.Name as Retailer,a.Address,a.Mobile,a.Email,a.ContactPerson,a.SyncId,max (a.State) as State,max (a.City) as City,max (a.Area) as Area,max (a.beat) as Beat,max (a.[By]) as [CreatedBy],sum(a.Business) as Business,sum(a.Pendency) as Pendency,Replace(Convert(varchar,max(a.Date),106), ' ', '/') as [Date] from (select distinct p.PartyId, p.PartyName as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,max(vg.stateName) AS State,max(vg.cityName) as City,max(vg.areaName) AS Area,max(b.AreaName) AS beat,max(p.Email) as Email,max(p.ContactPerson) as ContactPerson,max(p.SyncId) as SyncId,max(ms.SMName) AS [By],(SELECT sum(OrderAmount) FROM TransOrder WHERE PartyId=p.PartyId) AS Business,0 AS Pendency,max(p.created_date) as Date FROM MastParty p left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id WHERE p.partydist=0 " + Qrychk + " " + Qty1 + " ) group by p.PartyName,p.PartyId  ) a Group by a.PartyId,a.Name,a.Address,a.Mobile,a.Email,a.ContactPerson,a.SyncId ) s PIVOT (Max([Date]) FOR [CreatedBy] in (" + snames.Trim(',') + ")) as PivotTable");

                        query4.Append("select  * from (SELECT a.smid,Replace(Convert(varchar,a.vdate,106), ' ', '/') as [vdate],a.Visid,a.partyid,a.beatid,a.SMName as SalesPersonName,a.PartyName as Retailer,a.Address1 as Address,vg.stateName AS State,vg.cityName as City,vg.areaName AS Area,b.AreaName AS Beat,a.Mobile,mp.Email,mp.ContactPerson,mp.SyncId FROM (SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransOrder om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId UNION ALL SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM Temp_TransOrder om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId UNION SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransDemo om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId UNION ALL SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM Temp_TransDemo om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId UNION ALL SELECT om.SMID,om.VDate,om.VisId,om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransFailedVisit om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE p.PartyDist=0 UNION ALL SELECT om.SMID,om.VDate,om.VisId,om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM Temp_TransFailedVisit om LEFT JOIN mastparty p ON om.PartyId=p.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE p.PartyDist=0)a left join mastparty mp on mp.partyid=a.partyid left JOIN MASTAREA b on b.AreaId=a.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId LEFT JOIN mastsalesrep ms ON a.smid=ms.SMId WHERE a.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59' AND a.smid IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + sid.Trim(',') + ")) and Active=1)) s PIVOT (Max([vdate]) FOR SalesPersonName in (" + snames.Trim(',') + ")) as PivotTable");

                        query5.Append("select  * from (SELECT a.smid,Replace(Convert(varchar,a.vdate,106), ' ', '/') as [vdate],a.Visid,a.partyid,a.beatid,a.SMName as SalesPersonName,a.PartyName as Retailer,a.Address1 as Address,vg.stateName AS State,vg.cityName as City,vg.areaName AS Area,b.AreaName AS Beat,a.Mobile,mp.Email,mp.ContactPerson,mp.SyncId FROM (SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransOrder om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId UNION ALL SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM Temp_TransOrder om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId)a left join mastparty mp on mp.partyid=a.partyid left JOIN MASTAREA b on b.AreaId=a.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId LEFT JOIN mastsalesrep ms ON a.smid=ms.SMId WHERE a.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59' AND a.smid IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + sid.Trim(',') + ")) and Active=1) ) s PIVOT (Max([vdate]) FOR SalesPersonName in (" + snames.Trim(',') + ")) as PivotTable");


                    }

                    using (SqlDataAdapter da1 = new SqlDataAdapter(query1.ToString(), cn))
                    {
                        da1.Fill(dtSalesPersonOrdQtyWise);

                        SQL.DataTable dtsr = new SQL.DataTable();
                        for (int i = 0; i < dtSalesPersonOrdQtyWise.Rows.Count; i++)
                        {
                            str = "select msp.SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtSalesPersonOrdQtyWise.Rows[i]["smid"].ToString() + ")";
                            dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    dtSalesPersonOrdQtyWise.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                                }
                            }

                            string[] arr = des.Split(',');
                            for (int l = 0; l < arr.Length; l++)
                            {
                                if (dtSalesPersonOrdQtyWise.Rows[i][arr[l]].ToString() == " ")
                                {
                                    dtSalesPersonOrdQtyWise.Rows[i][arr[l]] = "Vacant";
                                }
                            }
                        }

                        dtsr.Dispose();
                    }



                    using (SqlDataAdapter da2 = new SqlDataAdapter(query2.ToString(), cn))
                    {
                        da2.Fill(dtSalesPersonOrdAmtWise);
                        SQL.DataTable dtsr = new SQL.DataTable();
                        for (int i = 0; i < dtSalesPersonOrdAmtWise.Rows.Count; i++)
                        {
                            str = "select msp.SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dtSalesPersonOrdAmtWise.Rows[i]["smid"].ToString() + ")";
                            dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    dtSalesPersonOrdAmtWise.Rows[i][dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                                }
                            }

                            string[] arr = des.Split(',');
                            for (int l = 0; l < arr.Length; l++)
                            {
                                if (dtSalesPersonOrdAmtWise.Rows[i][arr[l]].ToString() == " ")
                                {
                                    dtSalesPersonOrdAmtWise.Rows[i][arr[l]] = "Vacant";
                                }
                            }
                        }
                        dtsr.Dispose();
                    }

                    using (SqlDataAdapter da3 = new SqlDataAdapter(query3.ToString(), cn))
                    {
                        da3.Fill(dtSalesPersonNewParty);

                    }

                    using (SqlDataAdapter da4 = new SqlDataAdapter(query4.ToString(), cn))
                    {
                        da4.Fill(dtSalesPersonTotalCalls);

                    }

                    using (SqlDataAdapter da5 = new SqlDataAdapter(query5.ToString(), cn))
                    {
                        da5.Fill(dtSalesPersonProdCalls);

                    }
                    ds.Tables.Add(dtSalesPersonProdCalls);
                    ds.Tables.Add(dtSalesPersonTotalCalls);
                    ds.Tables.Add(dtSalesPersonNewParty);
                    ds.Tables.Add(dtSalesPersonOrdAmtWise);
                    ds.Tables.Add(dtSalesPersonOrdQtyWise);
                    ds.Tables.Add(dtSalesPerson);

                }

                dtSalesPerson.Dispose();
                dtSalesPersonOrdQtyWise.Dispose();
                dtSalesPersonOrdAmtWise.Dispose();
                dtSalesPersonNewParty.Dispose();
                dtSalesPersonTotalCalls.Dispose();
                dtSalesPersonProdCalls.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return ds;
        }

        public void TopSalesPersonExcel(SQL.DataSet ds)
        {
            int colindex = 0;
            SQL.DataTable dt = new SQL.DataTable();
            SQL.DataTable dtProducts = new SQL.DataTable();
            //----------------------Create Excel objects--------------------------------
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            oXL = new Excel.Application();
            // oXL.Visible = true;

            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            for (int k = 0; k < ds.Tables.Count; k++)
            {
                dt = ds.Tables[k];
                if (dt.Rows.Count == 0 && k == 5)
                {
                    oWB.Close();
                    oXL.Quit();
                    dt.Dispose();
                    dtProducts.Dispose();
                    ds.Dispose();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found.');", true);
                    return;

                }
                dtProducts = new SQL.DataTable();

                oSheet = (Excel._Worksheet)oXL.Worksheets.Add();

                //-------------------Create Category wise excel Worksheet------------------------------
                if (k == 5)
                {
                    oSheet.Name = "Top SalesPerson Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.AcceptChanges();
                    colindex = dtProducts.Columns.IndexOf("OrderAmount");

                }
                if (k == 4)
                {
                    oSheet.Name = "Top SalesPerson OrdQty Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.AcceptChanges();
                    colindex = dtProducts.Columns.IndexOf("RetailerEmail");
                }
                if (k == 3)
                {
                    oSheet.Name = "Top SalesPerson OrdAmt Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.AcceptChanges();
                    colindex = dtProducts.Columns.IndexOf("RetailerEmail");
                }
                else if (k == 2)
                {
                    oSheet.Name = "TS New Party Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("PartyId");
                    dtProducts.Columns.Remove("Pendency");
                    dtProducts.AcceptChanges();
                    colindex = dtProducts.Columns.IndexOf("Business");
                }
                else if (k == 1)
                {
                    oSheet.Name = "TS Total Calls Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.Columns.Remove("Visid");
                    dtProducts.Columns.Remove("partyid");
                    dtProducts.Columns.Remove("beatid");
                    dtProducts.AcceptChanges();
                    colindex = dtProducts.Columns.IndexOf("SyncId");
                }
                else if (k == 0)
                {
                    oSheet.Name = "TS Prod Calls Details";
                    dtProducts = dt.Copy();
                    dtProducts.Columns.Remove("smid");
                    dtProducts.Columns.Remove("Visid");
                    dtProducts.Columns.Remove("partyid");
                    dtProducts.Columns.Remove("beatid");
                    dtProducts.AcceptChanges();
                    colindex = dtProducts.Columns.IndexOf("SyncId");
                }

                //-------------------------------------Add column names to excel sheet--------------------
                string[] colNames = new string[dtProducts.Columns.Count];

                int col = 0;

                foreach (DataColumn dc in dtProducts.Columns)
                    colNames[col++] = dc.ColumnName;

                string lcol = getlastcol(dtProducts.Columns.Count);

                //int cnt =dtProducts.Columns.Count;
                //int c = 0;
                //int h = 0;
                //int c1 = 0;

                //while (cnt >26)
                //{
                //    int d = cnt - 26;
                //    cnt = d;
                //    h = d;
                //    c = c + 1;
                //} c1++;


                //char lc = '9';
                //char lc1 = '9';
                //if(c >0)
                //{
                //    lc = (char)(65 + c - 1);
                //}
                //else
                //    lc = (char)(65 + dtProducts.Columns.Count - 1);

                //if(h > 0)
                //    lc1 = (char)(65 + h - 1);
                //string lcol = "";
                //if (lc1 != '9')
                //    lcol = lc.ToString() + lc1.ToString();
                //else
                //    lcol = lc.ToString();

                // char lastColumn = (char)(65 + dtProducts.Columns.Count - 1);


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


                string startcol = getlastcol((colindex + 1));

                if (k == 0 || k == 1 || k == 2 || k == 3 || k == 4 || k == 5)
                {
                    oSheet.get_Range(startcol + "2", lcol + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }


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

            //--------------------------Save Product Excel sheet-------------------------------------
            // oXL.Visible = true;
            oXL.UserControl = true;
            string filename = "TopSalesPerson_" + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
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
        public SQL.DataSet TopPotentialCustomerSummary()
        {
            SQL.DataSet ds = new SQL.DataSet();
            SQL.DataTable dtPotentialCustomers = new SQL.DataTable();
            try
            {
                //--------------------Read data from SQL Server------------------------------------
                StringBuilder query = new StringBuilder();
                StringBuilder query1 = new StringBuilder();
                StringBuilder query2 = new StringBuilder();

                string smIDStr = "", smIDStr1 = "", Qrychk = "", matBeatNew = "";
                string order = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                ViewState["smIDStr1"] = smIDStr1;

                foreach (ListItem Beat in Lstbeatbox.Items)
                {
                    if (Beat.Selected)
                    {
                        matBeatNew += Beat.Value + ",";
                    }
                }
                matBeatNew = matBeatNew.TrimStart(',').TrimEnd(',');

                string beat_Filter = "";
                if (matBeatNew != "" && matBeatNew != "0")
                    beat_Filter = " and p.BeatId in (" + matBeatNew + ") ";
                else
                    beat_Filter = "";

                Qrychk = " where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                if (smIDStr1 != "")
                {
                    Qrychk = Qrychk + " and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                }
                query.Append("select top " + txt_noofrecords.Text + " max(a.PartyId) as PartyId,CustName as Retailer,max(State) as State,max(City) as City,max(area) as Area,max(Beat) as Beat,max(mp.SyncId) as SyncId,max(mp.ContactPerson) as ContactPerson,max(mp.Mobile) as Mobile,max(mp.Email) as Email,a.Potential,sum(TotSale) [TotSale],sum(a.Potential-a.TotSale) [Diff]  from ( select CustName,max(PartyId) as PartyId,sum(b.Potential) as Potential,");

                query.Append("sum(b.TotSale) [TotSale],sum(b.Diff) as  [Diff],max(City) as City,max(State) as State,max(area) as area,max(Beat) as Beat from ( select os.PartyId as PartyId,p.PartyName AS CustName,0 As Potential,sum(ISNULL(os.Qty*os.Rate,0)) AS TotSale,isnull(p.Potential,0)-(SUM(isnull(os.Qty*os.Rate ,0))) AS Diff,");

                query.Append(" '' AS City,'' AS State,'' as area,'' as Beat FROM TransOrder1 os left join MastParty p on p.PartyId =os.PartyId " + Qrychk + " " + beat_Filter + " Group by p.PartyName,p.Potential,os.PartyId union all ");

                query.Append("select p.PartyId as PartyId,p.PartyName AS CustName,p.Potential As Potential,0 AS TotSale,0 AS Diff,vg.cityName AS City,vg.stateName AS State,vg.areaName AS area,b.AreaName as Beat  FROM MastParty p left JOIN mastlink ua on ua.LinkCode=p.AreaId INNER JOIN mastsalesrep cp ON cp.SMId=ua.PrimCode INNER JOIN mastarea b ON b.AreaId=p.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId");

                query.Append(" Where ua.PrimCode in ( " + smIDStr1 + " ) " + beat_Filter + "  )b Group by b.CustName ) a left join mastparty mp on mp.partyid=a.partyid  group by a.CustName,a.Potential order by a.Potential desc,a.CustName ");

                using (SqlConnection cn = new SqlConnection(conString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query.ToString(), cn))
                    {
                        da.Fill(dtPotentialCustomers);
                    }
                }

                ds.Tables.Add(dtPotentialCustomers);

                dtPotentialCustomers.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return ds;
        }

        public void TopPotentialCustomerExcel(SQL.DataSet ds)
        {
            int colindex = 0;
            SQL.DataTable dt = new SQL.DataTable();
            SQL.DataTable dtProducts = new SQL.DataTable();
            //----------------------Create Excel objects--------------------------------
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            oXL = new Excel.Application();
            // oXL.Visible = true;

            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            for (int k = 0; k < ds.Tables.Count; k++)
            {
                dt = ds.Tables[k];
                if (dt.Rows.Count == 0)
                {
                    oWB.Close();
                    oXL.Quit();
                    dt.Dispose();
                    dtProducts.Dispose();
                    ds.Dispose();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found.');", true);
                    return;

                }
                dtProducts = new SQL.DataTable();

                oSheet = (Excel._Worksheet)oXL.Worksheets.Add();

                //-------------------Create Category wise excel Worksheet------------------------------
                if (k == 0)
                {
                    oSheet.Name = "Top Potential Customer Details";
                    dtProducts = dt.Copy();
                    dtProducts.AcceptChanges();
                    dtProducts.Columns.Remove("PartyId");
                    dtProducts.AcceptChanges();
                    colindex = dtProducts.Columns.IndexOf("Potential");
                }

                //-------------------------------------Add column names to excel sheet--------------------
                string[] colNames = new string[dtProducts.Columns.Count];

                int col = 0;

                foreach (DataColumn dc in dtProducts.Columns)
                    colNames[col++] = dc.ColumnName;

                char lastColumn = (char)(65 + dtProducts.Columns.Count - 1);
                char startColumn = (char)(65 + (colindex + 1) - 1);

                oSheet.get_Range("A1", lastColumn + "1").Value2 = colNames;
                oSheet.get_Range("A1", lastColumn + "1").Font.Bold = true;
                oSheet.get_Range("A1", lastColumn + "1").VerticalAlignment
                            = Excel.XlVAlign.xlVAlignCenter;

                Range range = oSheet.get_Range("A1", lastColumn + "1");
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


                if (k == 0)
                {
                    oSheet.get_Range(startColumn + "2", lastColumn + (rowCnt + 1).ToString()).Cells.HorizontalAlignment =
             Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                }

                oSheet.get_Range("A2", lastColumn + (rowCnt + 1).ToString()).Value2 = rowData;
                Microsoft.Office.Interop.Excel.Range chartRange;
                chartRange = oSheet.get_Range("A1", lastColumn + (rowCnt + 1).ToString());
                foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                {
                    cell.BorderAround2();
                }
                oSheet.Columns.AutoFit();

            }

            oSheet.Application.ActiveWindow.SplitRow = 1;
            oSheet.Application.ActiveWindow.FreezePanes = true;
            //--------------------------Save Product Excel sheet-------------------------------------
            // oXL.Visible = true;
            oXL.UserControl = true;

            string filename = "TopPotentialCustomers_" + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                btnExport.Enabled = false;
                // spinnerload.Style.Add("display", "block");
                // spinnerload.Attributes.Add("style", "display:block;");
                string smIDStr = "", smIDStr1 = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                ViewState["smIDStr1"] = smIDStr1;

                if (smIDStr1 != "")
                {
                    if (CategoryType.SelectedIndex == -1)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Category Type');", true);
                        btnExport.Enabled = true;
                        // spinner.Attributes.Add("style", "display:none;");

                        return;

                    }
                    if (txt_noofrecords.Text == "")
                    {
                        btnExport.Enabled = true;
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill no of records');", true);
                        // spinner.Attributes.Add("style", "display:none;");

                        return;

                    }
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        btnExport.Enabled = true;
                        // spinner.Attributes.Add("style", "display:none;");
                        return;
                    }

                    SQL.DataSet ds = new DataSet();

                    string CType = CategoryType.SelectedItem.ToString();

                    if (CType == "Product")
                    {
                        ds = TopProductSummary();
                        TopProductExcel(ds);
                    }
                    else if (CType == "Distributor")
                    {
                        ds = TopDistributorSummary();
                        TopDistributorExcel(ds);

                    }
                    else if (CType == "Retailer")
                    {
                        ds = TopRetailerSummary();
                        TopRetailerExcel(ds);
                    }
                    else if (CType == "Sales Person")
                    {
                        ds = TopSalesPersonSummary();
                        TopSalesPersonExcel(ds);
                    }
                    else if (CType == "Potential Customer")
                    {
                        ds = TopPotentialCustomerSummary();
                        TopPotentialCustomerExcel(ds);
                    }

                    // spinner.Attributes.Add("style", "display:none;");
                    btnExport.Enabled = true;
                    ds.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                    // spinner.Attributes.Add("style", "display:none;");
                    btnExport.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                // spinner.Attributes.Add("style", "display:none;");
                btnExport.Enabled = true;
                ex.ToString();
            }
        }


        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopProductDetails.aspx");
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
                    BindbeatDDl(smIDStr12);
                }
                else
                {
                    Lstbeatbox.Items.Clear();
                    Lstbeatbox.DataBind();

                }

                ViewState["tree"] = smiMStr;
                hidsalesperson.Value = smiMStr;
            }

            cnt = cnt + 1;
            return;
        }
        protected void Topsaleproduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                HiddenField hdnitemid = (HiddenField)e.Item.FindControl("hdnitemid");
                //  string gh = ViewState["viewsmidstr"].ToString();
                Response.Redirect("TopProductDetails.aspx?itemid=" + hdnitemid.Value + "&FromDate=" + ViewState["Fromdate"] + "&ToDate=" + ViewState["Todate"] + "&smidstr='" + ViewState["smIDStr1"].ToString() + "'");
            }
        }
    }
}