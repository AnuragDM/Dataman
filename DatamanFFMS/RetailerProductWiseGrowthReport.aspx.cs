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
using System.Globalization;

namespace AstralFFMS
{
    public partial class RetailerProductWiseGrowthReport : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                //List<Products> Products = new List<Products>();
                //Products.Add(new Products());
                //rptRPWiseGrowth.DataSource = Products;
                //rptRPWiseGrowth.DataBind();
                BindMaterialGroup();
                BindMaterialGroup();
                roleType = Settings.Instance.RoleType;
                BindDDLMonth();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                btnExport.Visible = true;
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
                BindTreeViewControl();

            }

        }

        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    monthDDL.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
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

        public class Products
        {
            public string ProductGroup { get; set; }
            public string Product { get; set; }
            public string Qty { get; set; }
            public string Amount { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetTopproduct(string ProductGroup, string Product, string Fromdate, string Todate, string noofrecords, string salesPerson)
        {
            string qry = "";
            string Qrychk = " os.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and os.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
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
                dtProdRep.Dispose();
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void Bindcity(string SMIDStr)
        {
            try
            {
                string cityQry = @"  select AreaId,Areaname from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    CityListbox.DataSource = dtProdRep;
                    CityListbox.DataTextField = "AreaName";
                    CityListbox.DataValueField = "AreaId";
                    CityListbox.DataBind();
                }
                dtProdRep.Dispose();
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
                rptRPWiseGrowth.DataSource = null;
                rptRPWiseGrowth.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", Query = "", Qrychk = "", Q1 = "", matcitystrnew = "", matareastrnew = "",
            matbeatstrnew = "", matpartystrnew = "", matGrpStrNew = "", matProStrNew = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (smIDStr1 == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                rptRPWiseGrowth.DataSource = null;
                rptRPWiseGrowth.DataBind();
                return;
            }

            if (monthDDL.SelectedValue == "0")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please Select Month');", true);
                return;
            }

            foreach (ListItem city in CityListbox.Items)
            {
                if (city.Selected)
                {
                    matcitystrnew += city.Value + ",";
                }
            }
            matcitystrnew = matcitystrnew.TrimStart(',').TrimEnd(',');

            foreach (ListItem area in AreaListBox.Items)
            {
                if (area.Selected)
                {
                    matareastrnew += area.Value + ",";
                }
            }
            matareastrnew = matareastrnew.TrimStart(',').TrimEnd(',');

            foreach (ListItem beat in Lstbeatbox.Items)
            {
                if (beat.Selected)
                {
                    matbeatstrnew += beat.Value + ",";
                }
            }
            matbeatstrnew = matbeatstrnew.TrimStart(',').TrimEnd(',');

            foreach (ListItem party in Lstpartybox.Items)
            {
                if (party.Selected)
                {
                    matpartystrnew += party.Value + ",";
                }
            }
            matpartystrnew = matpartystrnew.TrimStart(',').TrimEnd(',');

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

            try
            {
                string Curr = "01/" + (monthDDL.SelectedValue) + "/" + DateTime.Now.Year.ToString();
                string das = Convert.ToDateTime(Curr).ToString("dd/MMM/yyyy");
                DateTime mm = Convert.ToDateTime(das).AddMonths(1);
                string a = mm.AddDays(-1).ToString();
                string aaa = Convert.ToDateTime(a).ToString("dd/MMM/yyyy");
                string abc = Convert.ToDateTime(das).AddMonths(-3).ToString("dd/MMM/yyyy");
                string ss = Convert.ToDateTime(das).AddDays(-1).ToString("dd/MMM/yyyy");

                Qrychk = " where os.VDate between '" + abc + " 00:00' and '" + ss + " 23:59'";
                Q1 = " where os.VDate between '" + das + " 00:00' and '" + aaa + " 23:59'";

                Qrychk = Qrychk + " and os.Smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) ";
                Q1 = Q1 + " and os.Smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) ";

                if (matGrpStrNew != "" && matProStrNew != "")
                {
                    Qrychk = Qrychk + " and i.ItemId in (" + matProStrNew + ") ";
                    Q1 = Q1 + " and i.ItemId in (" + matProStrNew + ") ";
                }
                if (matGrpStrNew != "" && matProStrNew == "")
                {
                    Qrychk = Qrychk + " and pg.ItemId in (" + matGrpStrNew + ") ";
                    Q1 = Q1 + " and pg.ItemId in (" + matGrpStrNew + ") ";
                }

                if (matcitystrnew != "")
                {
                    Qrychk = Qrychk + " and p.cityid in (" + matcitystrnew + ") ";
                    Q1 = Q1 + " and p.cityid  in (" + matcitystrnew + ") ";
                }
                if (matareastrnew != "")
                {
                    Qrychk = Qrychk + " and p.areaid in (" + matareastrnew + ") ";
                    Q1 = Q1 + " and p.areaid in (" + matareastrnew + ") ";
                }
                if (matbeatstrnew != "" && matbeatstrnew != "0")
                {
                    Qrychk = Qrychk + " and p.BeatId in ( " + matbeatstrnew + ")";
                    Q1 = Q1 + " and p.BeatId in ( " + matbeatstrnew + ")";
                }
                if (matpartystrnew != "" && matpartystrnew != "0")
                {
                    Qrychk = Qrychk + " and p.partyid in ( " + matpartystrnew + ")";
                    Q1 = Q1 + " and p.partyid in ( " + matpartystrnew + ")";

                }
                Query = "select DENSE_RANK () Over (Order by Party) as Row, * from (select a.Party,a.Item ,sum(a.Avg_Value) as Avg_Value,sum(a.Value) Value,case when (SUM(a.Avg_Value)=0 and SUM(a.Value)>0) then 100 when sum(a.Value-a.Avg_Value)> 0 then sum((a.Value-a.Avg_Value)*100)/SUM(a.Avg_Value) when SUM(a.Value)=0 then 0 when sum(a.Value-a.Avg_Value)< 0 then 0 end as Percentage from (" +
                "select (p.PartyName+' (Beat--> '+ b.areaname + ')') as Party,i.itemname as Item,((os.Qty*os.Rate)/3) as Avg_Value,0 as Value from TransOrder1 os inner JOIN mastparty p on p.PartyId =os.PartyId inner join MastArea b on b.AreaId=p.BeatId inner join MastItem i on i.ItemId=os.ItemId " + Qrychk + "  union all select (p.PartyName+' (Beat--> '+ b.areaname + ')') as Party,i.itemname as Item,0 as Avg_Value,((os.Qty*os.Rate)) as Value from TransOrder1 os inner join mastparty p on p.PartyId =os.PartyId inner join MastArea b on b.AreaId=p.BeatId inner join MastItem i on i.ItemId=os.ItemId " + Q1 + " )a group by a.Party,a.Item ) a order by a.party ";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    rptRPWiseGrowth.DataSource = dtItem;
                    rptRPWiseGrowth.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    rptRPWiseGrowth.DataSource = dtItem;
                    rptRPWiseGrowth.DataBind();
                    btnExport.Visible = false;
                }
                dtItem.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
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
                dtMastItem1.Dispose();
            }
            else
            {
                ClearControls();
            }
        }

        protected void Lstbeatbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strparty = string.Empty;
            foreach (ListItem party in Lstbeatbox.Items)
            {
                if (party.Selected)
                {
                    strparty += party.Value + ",";
                }
            }
            strparty = strparty.TrimStart(',').TrimEnd(',');
            if (strparty != "")
            {
                string str = @"select PartyId,partyName from Mastparty where Beatid in (" + strparty + ") order by partyname";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    Lstpartybox.DataSource = dt;
                    Lstpartybox.DataTextField = "partyName";
                    Lstpartybox.DataValueField = "PartyId";
                    Lstpartybox.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
                dt.Dispose();
            }
            else
            {
                ClearControls();
            }
        }

        protected void CityListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matcityStr = "";
            foreach (ListItem matcity in CityListbox.Items)
            {
                if (matcity.Selected)
                {
                    matcityStr += matcity.Value + ",";
                }
            }
            matcityStr = matcityStr.TrimStart(',').TrimEnd(',');

            if (matcityStr != "")
            {
                string mastItemQry1 = @"SELECT AreaId,areaname,areatype FROM MastArea WHERE UnderId IN (" + matcityStr + ") and AreaType='Area' and Active=1 order by areaname";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    AreaListBox.DataSource = dtMastItem1;
                    AreaListBox.DataTextField = "AreaName";
                    AreaListBox.DataValueField = "AreaId";
                    AreaListBox.DataBind();
                }
                dtMastItem1.Dispose();
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
            Response.AddHeader("content-disposition", "attachment;filename=RetailerProductWiseGrowth.csv");
            string headertext = "Retailer".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Avg Monthly Sale In last 3 Months.".TrimStart('"').TrimEnd('"') + "," + "Current Month Sale".TrimStart('"').TrimEnd('"') + "," + "Percentage Growth".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Party", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Item", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Avg_Value", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Value", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Percentage", typeof(String)));

            foreach (RepeaterItem item in rptRPWiseGrowth.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblparty = item.FindControl("lblparty") as Label;
                dr["Party"] = lblparty.Text.ToString();
                Label lblitem = item.FindControl("lblitem") as Label;
                dr["Item"] = lblitem.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblavgvalue = item.FindControl("lblavgvalue") as Label;
                dr["Avg_Value"] = lblavgvalue.Text.ToString();
                Label lblvalue = item.FindControl("lblvalue") as Label;
                dr["Value"] = lblvalue.Text.ToString();
                Label lblpercentage = item.FindControl("lblpercentage") as Label;
                dr["Percentage"] = lblpercentage.Text.ToString();
                dtParams.Rows.Add(dr);
            }
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
            Response.AddHeader("content-disposition", "attachment;filename=RetailerProductWiseGrowth.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();

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

                if (smIDStr12 != "")
                {
                    Bindcity(smIDStr12);
                }
                else
                {
                    CityListbox.Items.Clear();
                    CityListbox.DataBind();

                }
                string smiMStr = smIDStr12;
                ViewState["tree"] = smiMStr;
            }
            cnt = cnt + 1;
            return;
        }

        protected void AreaListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matareaStr = "";
            foreach (ListItem matarea in AreaListBox.Items)
            {
                if (matarea.Selected)
                {
                    matareaStr += matarea.Value + ",";
                }
            }
            matareaStr = matareaStr.TrimStart(',').TrimEnd(',');

            if (matareaStr != "")
            {
                string mastItemQry1 = @"SELECT AreaId,areaname,areatype FROM MastArea WHERE UnderId IN (" + matareaStr + ") and AreaType='Beat' and Active=1 order by areaname";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    Lstbeatbox.DataSource = dtMastItem1;
                    Lstbeatbox.DataTextField = "AreaName";
                    Lstbeatbox.DataValueField = "AreaId";
                    Lstbeatbox.DataBind();
                }

                dtMastItem1.Dispose();

            }
            else
            {
                ClearControls();
            }
        }
    }
}
