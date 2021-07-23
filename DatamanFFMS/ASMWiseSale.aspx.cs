using BusinessLayer;
using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class ASMWiseSale : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        string orderType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            string DashBoardDate = Request.QueryString["Date"];
            orderType = Request.QueryString["type"];
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

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
              //  btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
                this.checkNode();
                if (!string.IsNullOrEmpty(DashBoardDate) && !string.IsNullOrEmpty(orderType))
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



            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


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

        private void ClearControls()
        {
            try
            {
                productListBox.Items.Clear();
                Topsaleproduct.DataSource = null;
                Topsaleproduct.DataBind();
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
        protected void btnGo_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/TopProductReport.aspx");
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
            if (smIDStr1 != "")
            {
                if (txt_noofrecords.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill no of records');", true);
                    Topsaleproduct.DataSource = null;
                    Topsaleproduct.DataBind();
                    return;

                }
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    Topsaleproduct.DataSource = new DataTable();
                    Topsaleproduct.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }
                //  Qrychk = " where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                Qrychk = " where os.VDate>='" + txtfmDate.Text + "' and os.VDate<='" + txttodate.Text + "'";

                if (smIDStr1 != "")
                {
                    Qrychk = Qrychk + " and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                }
                //ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                //ViewState["Todate"] = Settings.dateformat(txttodate.Text);

                if (orderType == "T") order = "desc";
                else if (orderType == "B") order = "asc";
                else order = "desc";

                string query = "SELECT TOP " + txt_noofrecords.Text + " mp.smid,mp.smname,sum(b.qty) AS qty,sum(b.Amount) AS Amount,b.roleid from (SELECT ProductGroup,itemid,Product,a.roleid,a.smid,a.underid,convert(int,sum(Qty)) [Qty],convert(decimal, sum(Amount)) [Amount] FROM ( SELECT pg.ItemName [ProductGroup],i.itemid,i.ItemName [Product],ms.RoleId,ms.SMId,ms.UnderId,convert(int,(os.Qty)) [Qty],convert(decimal, (os.Qty * os.Rate)) [Amount] From  TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left JOIN MastItem pg ON pg.ItemId = i.Underid ";
                query = query + "LEFT JOIN MastSalesRep ms ON ms.SMId=os.smid " + Qrychk + " " + QueryMatGrp + " Union All SELECT pg.ItemName [ProductGroup],i.itemid,i.ItemName [Product],ms.RoleId,ms.SMId,ms.UnderId,convert(int,(os.Qty)) [Qty],convert(decimal, (os.Qty * os.Rate)) [Amount] From Temp_TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left JOIN MastItem pg ON pg.ItemId = i.Underid ";
                query = query + "LEFT JOIN MastSalesRep ms ON ms.SMId=os.smid " + Qrychk + " " + QueryMatGrp + ") a  GROUP BY a.ItemId, a.ProductGroup, a.Product,a.roleid,a.smid,a.underid)b LEFT JOIN mastsalesrep mp ON mp.SMId=b.underid LEFT JOIN mastrole rm ON mp.RoleId=rm.roleid WHERE roletype IN ('DistrictHead','CityHead')GROUP BY mp.smid,mp.SMName,b.roleid ORDER BY sum(b.Amount) " + order + "";
                            

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    Topsaleproduct.DataSource = dtItem;
                    Topsaleproduct.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    Topsaleproduct.DataSource = dtItem;
                    Topsaleproduct.DataBind();
                    btnExport.Visible = false;
                }

            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                Topsaleproduct.DataSource = null;
                Topsaleproduct.DataBind();
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopProductReport.aspx");
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=ASMWiseSales.csv");
            string headertext = "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("ProductGroup", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));

            foreach (RepeaterItem item in Topsaleproduct.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblProductGroup = item.FindControl("lblProductGroup") as Label;
                dr["ProductGroup"] = lblProductGroup.Text.ToString();
                //Label lblProduct = item.FindControl("lblProduct") as Label;
                //dr["Product"] = lblProduct.Text.ToString();
                Label lblQty = item.FindControl("lblQty") as Label;
                dr["Qty"] = lblQty.Text.ToString();
                Label lblAmount = item.FindControl("lblAmount") as Label;
                dr["Amount"] = lblAmount.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
            dv.Sort = "Amount desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[3];
            try
            {
                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            string h = dtParams.Rows[j][k].ToString();
                            string d = h.Replace(",", " ");
                            dtParams.Rows[j][k] = "";
                            dtParams.Rows[j][k] = d;
                            dtParams.AcceptChanges();
                            if (k == 0)
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                if (k == 1 || k == 2)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                if (k == 1 || k == 2)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                                //Total For Columns
                                if (k == 1 || k == 2)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                                //End
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                string totalStr = string.Empty;
                for (int x = 1; x < totalVal.Count(); x++)
                {
                    if (totalVal[x] == 0)
                    {
                        totalStr += "0" + ',';
                    }
                    else
                    {
                        totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                    }
                }
                sb.Append("Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

            }
            catch (Exception ex)
            {
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
                HiddenField hdnsmid = (HiddenField)e.Item.FindControl("hdnsmid");
                //  string gh = ViewState["viewsmidstr"].ToString();
                Response.Redirect("ASMWiseSaleDetails.aspx?smid=" + hdnsmid.Value + "&FromDate=" + txtfmDate.Text + "&ToDate=" + txttodate.Text + "&smidstr='" + ViewState["smIDStr1"].ToString() + "'");
            }
        }
    }
}