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

namespace AstralFFMS
{
    public partial class ItemDistributorWise : System.Web.UI.Page
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
                List<Products> Products = new List<Products>();
                Products.Add(new Products());
                TopItemdistributor.DataSource = Products;
                TopItemdistributor.DataBind();
                roleType = Settings.Instance.RoleType;
                BindMaterialGroup();
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
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
           // string query = "select (i.ItemId) as Code,i.ItemName as Name,psg.ItemId AS productgrpid,da.PartyName as DName,vg.cityName as City,vg.stateName as State,sum(os.Qty) as Qty,sum(os.Qty*os.Rate) AS Amount from Transorder1 os " +
           //" left join MastParty da on da.AreaId=os.AreaId LEFT JOIN ViewGeo vg ON vg.areaId=da.AreaId inner join mastitem i on i.ItemId=os.ItemId left JOIN mastitem  psg ON psg.ItemId=i.Underid" +
           //" WHERE da.PartyDist=1 and da.Partyid in (" + Distid + ") and " + Qrychk + " " + qry + " group by da.PartyName,i.ItemId,i.ItemName,psg.ItemId,vg.cityName,vg.stateName order by i.itemname";

            string query = "select (i.ItemId) as Code,i.ItemName as Name,psg.ItemId AS productgrpid,da.PartyName as DName,max(mc.areaname) as City ,max(ms.areaname) as State,sum(os.Qty) as Qty,sum(os.Qty*os.Rate) AS Amount from Transorder1 os left join MastParty da on da.PartyId=os.DistId left join mastarea ma on ma.AreaId=da.AreaId left join mastarea mc on mc.areaid=ma.underid left join mastarea md on md.areaid=mc.underid left join mastarea ms on ms.areaid=md.underid left join mastitem i on i.ItemId=os.ItemId left JOIN mastitem  psg ON psg.ItemId=i.Underid WHERE da.PartyDist=1 and da.Partyid in (" + Distid + ") and " + Qrychk + " " + qry + " group by da.PartyName,i.ItemId,i.ItemName,psg.ItemId order by i.itemname";

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

                dtProdRep.Dispose();
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
                St.Dispose();
                Rows = null;
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
            Rows = null;
        }

        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string citystr = "";
                    //string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";

                    string cityQry = @"select AreaId from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');
                    //string distqry = @"select * from MastParty where CityId in (" + citystr + ")  and PartyDist=1 and Active=1 order by PartyName";
                    //string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";

                    string distqry = @"select PartyId,PartyName from MastParty where AreaId in (" + citystr + ")   and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }

                    dtCity.Dispose();
                    dtDist.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    TopItemdistributor.DataSource = null;
                    TopItemdistributor.DataBind();
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
                dtMastItem1.Dispose();
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
                TopItemdistributor.DataSource = null;
                TopItemdistributor.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
            //foreach (ListItem item in salespersonListBox.Items)
            //{
            //    if(item.Selected)
            //    {
            //        smIDStr += item.Value + ", ";
            //    }
            //    smIDStr = smIDStr.TrimStart(',').TrimEnd(',');
            //}
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
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
                QueryMatGrp = " and mi.ItemId in (" + matProStrNew + ") ";
            }
            if (matGrpStrNew != "" && matProStrNew == "")
            {
                QueryMatGrp = " and mi1.ItemId in (" + matGrpStrNew + ") ";
            }
            if (smIDStr1 != "")
            {
                if (distIdStr1 != "")
                {
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        TopItemdistributor.DataSource = new DataTable();
                        TopItemdistributor.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }

                    Qrychk = " t1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                    string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
                                  on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                                  t1.DistId in (" + distIdStr1 + ") and " + Qrychk + "  " + QueryMatGrp + " group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";

                    DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    if (dtItem.Rows.Count > 0)
                    {
                        rptmain.Style.Add("display", "block");
                        TopItemdistributor.DataSource = dtItem;
                        TopItemdistributor.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        rptmain.Style.Add("display", "block");
                        TopItemdistributor.DataSource = dtItem;
                        TopItemdistributor.DataBind();
                        btnExport.Visible = false;
                    }
                    dtItem.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                    TopItemdistributor.DataSource = null;
                    TopItemdistributor.DataBind();
                }
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                TopItemdistributor.DataSource = null;
                TopItemdistributor.DataBind();
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ItemDistributorWise.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string smIDStr1 = "", distIdStr1 = "", Qrychk = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=ProductDistributorWise.csv");
            string headertext = "Product".TrimStart('"').TrimEnd('"') + "," + "Distributor".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "State".TrimStart('"').TrimEnd('"') + "," + "Order Qty".TrimStart('"').TrimEnd('"') + "," + "Order Amt".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);

            //DataTable dtParams = new DataTable();
            //dtParams.Columns.Add(new DataColumn("Name", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Group", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("LastSoldOn", typeof(String)));

            //foreach (RepeaterItem item in TopItemdistributor.Items)
            //{
            //    DataRow dr = dtParams.NewRow();
            //    Label lblName = item.FindControl("lblName") as Label;
            //    dr["Name"] = lblName.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
            //    Label lblGroup = item.FindControl("lblGroup") as Label;
            //    dr["Group"] = lblGroup.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
            //    Label lblLSoldOn = item.FindControl("lblLSoldOn") as Label;
            //    dr["LastSoldOn"] = lblLSoldOn.Text.ToString();
            //    dtParams.Rows.Add(dr);
            //}

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
                QueryMatGrp = " and pg.ItemId in (" + matGrpStrNew + ") ";
            }

            if (distIdStr1 != "")
            {
                Qrychk = " os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                string query = "select i.ItemName as Name,da.PartyName as DName,vg.cityName as City,vg.stateName as State,sum(os.Qty) as Qty,sum(os.Qty*os.Rate) AS Amount from Transorder1 os " +
                " left join MastParty da on da.AreaId=os.AreaId  LEFT JOIN ViewGeo vg ON vg.areaId=da.AreaId" +
                " inner join mastitem i on i.ItemId=os.ItemId left JOIN mastitem  psg ON psg.ItemId=i.Underid WHERE da.PartyDist=1 and da.Partyid in (" + distIdStr1 + ") and " + Qrychk + " " + QueryMatGrp + " group by da.PartyName,i.ItemName,vg.cityName,vg.stateName Order by i.itemname";

                DataTable dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, query);

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
                                //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=ProductDistributorWise.csv");
                Response.Write(sb.ToString());
                Response.End();

                sb.Clear();
                dtParams.Dispose();
            }
        }
    }
}