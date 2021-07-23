using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using System.Web.Services;

namespace AstralFFMS
{
    public partial class MonthlyItemSale : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                BindDDLMonth();
                List<DistributorsMonthlyItem> distMonthlyItem = new List<DistributorsMonthlyItem>();
                distMonthlyItem.Add(new DistributorsMonthlyItem());
                monthlyItemRpt.DataSource = distMonthlyItem;
                monthlyItemRpt.DataBind();
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
                //Ankita - 18/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalePersonDDl();
                //fill_TreeArea();
                BindTreeViewControl();
                BindStaffChkList(CheckBoxList1);
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

        public class DistributorsMonthlyItem
        {
            public string distributor { get; set; }
            public string MaterialGROUP { get; set; }
            public string Item { get; set; }
            public string Year { get; set; }
            public string Jan { get; set; }
            public string Feb { get; set; }
            public string Mar { get; set; }
            public string Apr { get; set; }
            public string May { get; set; }
            public string Jun { get; set; }
            public string Jul { get; set; }
            public string Aug { get; set; }
            public string Sep { get; set; }
            public string Oct { get; set; }
            public string Nov { get; set; }
            public string Dec { get; set; }
            public string d { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetMonthlyItemSale(string Distid, string year, string MonthText, string Month, string View)
        {
            string Query, smIDStr1 = "", str = "";



            // smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
            if (Month.Length != 26)
            {

                if (View == "Quantity")
                {

                    if (MonthText.Contains("Apr"))
                    {
                        if (str.Length != 0)
                        {
                            str = " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("May"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate ";
                        }

                    }
                    if (MonthText.Contains("Jun"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }

                    }
                    if (MonthText.Contains("Jul"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }

                    }
                    if (MonthText.Contains("Aug"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }

                    }
                    if (MonthText.Contains("Sep"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }

                    }
                    if (MonthText.Contains("Oct"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Nov"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Dec"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Jan"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Feb"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Mar"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }

                    Query = @"select a. SyncId,a.distributor,a.MaterialGROUP,a.Item,Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,
                    SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                    (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from ( " + str + " ) a group by a.MaterialGROUP,a.Item,a. SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year";

                }
                else
                {

                    if (MonthText.Contains("Apr"))
                    {
                        if (str.Length != 0)
                        {
                            str = " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";

                        }
                    }

                    if (MonthText.Contains("May"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";

                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Jun"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Jul"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Aug"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Sep"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Oct"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Nov"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Dec"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Jan"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                    }
                    if (MonthText.Contains("Feb"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }

                    }
                    if (MonthText.Contains("Mar"))
                    {
                        if (str.Length != 0)
                        {
                            str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }
                        else
                        {
                            str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                        }

                    }
                }
                Query = @"select a. SyncId,a.distributor,a.MaterialGROUP,a.Item,Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,
                    SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                    (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from ( " + str + " ) a group by a.MaterialGROUP,a.Item,a. SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year ";

            }
            else
            {
                if (View == "Quantity")
                {
                    Query = @"select a. SyncId,a.distributor,a.MaterialGROUP,a.Item,Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,
                 SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                 (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from (

                     SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate) a group by a.MaterialGROUP,a.Item,a. SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year ";
                }
                else
                {
                    Query = @"select Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),a. SyncId,a.distributor,a.MaterialGROUP,a.Item,SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                  (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from ( 

                       SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate )a group by a.MaterialGROUP,a.Item,a. SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year";
                }
            }

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            return JsonConvert.SerializeObject(dtItem);

        }

        private void BindDDLMonth()
        {
            try
            {

                //for (int month = 1; month <= 12; month++)
                //{
                //    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                //    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));  

                //}

                // listboxmonth.Items.Add(new ListItem("Select", "0", true));
                for (int month = 4; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    // listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3),monthName.Substring(0, 3)));                    
                }
                for (int month = 1; month <= 3; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    // listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3),monthName.Substring(0, 3)));                    
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindSalePersonDDl()
        {
            try
            {
                if (roleType == "Admin")
                {
                    string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    DataTable dtcheckrole = new DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";

                    ListBox1.DataSource = dv1.ToTable();
                    ListBox1.DataTextField = "SMName";
                    ListBox1.DataValueField = "SMId";
                    ListBox1.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    //     dv.RowFilter = "RoleName='Level 1'";
                    dv.RowFilter = "SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ListBox1.DataSource = dv.ToTable();
                        ListBox1.DataTextField = "SMName";
                        ListBox1.DataValueField = "SMId";
                        ListBox1.DataBind();
                    }
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindStaffChkList(CheckBoxList accStaffCheckBoxList)
        {

            try
            {
                for (int i = System.DateTime.Now.Year; i >= (System.DateTime.Now.Year - 2); i--)
                {
                    //int CurrentYear = i;
                    // int NextYear = i + 1;
                    // string FinYear = null;
                    // FinYear = CurrentYear + "-" + NextYear;
                    accStaffCheckBoxList.Items.Add(new ListItem(i.ToString()));
                }
                accStaffCheckBoxList.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr = "", smIDStr1 = "", Query = "";
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                    }
                }
                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
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
                    string year = "";
                    foreach (ListItem item in CheckBoxList1.Items)
                    {
                        if (item.Selected)
                        {
                            year += item.Text + ",";
                        }
                    }
                    year = year.TrimStart(',').TrimEnd(',');
                    if (year != "")
                    {
                        //DbParameter[] dbParam = new DbParameter[1];
                        //dbParam[0] = new DbParameter("@salesmanId", DbParameter.DbType.NVarChar, 8000, smIDStr1);
                        //DataTable dtMonthItemSale = new DataTable();
                        //if (approveStatusRadioButtonList.SelectedValue == "Quantity")
                        //{
                        //    dtMonthItemSale = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_QtymonthlyItemSale", dbParam);
                        //}
                        //else
                        //{
                        //    dtMonthItemSale = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_AmtmonthlyItemSale", dbParam);
                        //}                     

                        if (approveStatusRadioButtonList.SelectedValue == "Quantity")
                        {
                            Query = @"select a.Year,a.distributor,a.MaterialGROUP,a.Item,SUM(a.t1_Value) as t1Value,SUM(a.t2_value) as t2Value,SUM(a.t3_value) as t3Value,SUM(a.t4_value) as t4Value,SUM(a.t5_value) as t5Value,SUM(a.t6_value) as t6Value,SUM(a.t7_value) as t7Value,SUM(a.t7_value) as t7Value,SUM(a.t8_value) as t8Value,SUM(a.t9_value) as t9Value,SUM(a.t10_value) as t10Value,SUM(a.t11_value) as t11Value,SUM(a.t12_value) as t12Value from (

                             SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.PartyName, mi.ItemName,ts1.VDate    )a group by a.MaterialGROUP,a.Item,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year ";
                        }
                        else
                        {
                            Query = @"select a.Year,a.distributor,a.MaterialGROUP,a.Item,SUM(a.t1_Value) as t1Value,SUM(a.t2_value) as t2Value,SUM(a.t3_value) as t3Value,SUM(a.t4_value) as t4Value,SUM(a.t5_value) as t5Value,SUM(a.t6_value) as t6Value,SUM(a.t7_value) as t7Value,SUM(a.t7_value) as t7Value,SUM(a.t8_value) as t8Value,SUM(a.t9_value) as t9Value,SUM(a.t10_value) as t10Value,SUM(a.t11_value) as t11Value,SUM(a.t12_value) as t12Value from ( 

                              SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.PartyName, mi.ItemName,ts1.VDate )a group by a.MaterialGROUP,a.Item,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year";
                        }
                        DataTable dtMonthItemSale = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                        if (dtMonthItemSale.Rows.Count > 0)
                        {
                            rptDiv.Style.Add("display", "block");
                            monthlyItemRpt.DataSource = dtMonthItemSale;
                            monthlyItemRpt.DataBind();
                            btnExport.Visible = true;
                        }
                        else
                        {
                            rptDiv.Style.Add("display", "block");
                            monthlyItemRpt.DataSource = dtMonthItemSale;
                            monthlyItemRpt.DataBind();
                            btnExport.Visible = false;
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select year');", true);
                        monthlyItemRpt.DataSource = null;
                        monthlyItemRpt.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                    monthlyItemRpt.DataSource = null;
                    monthlyItemRpt.DataBind();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MonthlyItemSale.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string MonthText = "", str = "";


            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=MonthlyItemSaleReport.csv");
            string headertext = "Distributor SyncId".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + "," + "Grand Total".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            string Month = "";
            string Distid = "";
            string strQtyAmount = string.Empty;
            string strviewamount = string.Empty;
            DataTable dtParams = new DataTable();
            string smIDStr = "", smIDStr1 = "", Query = "";
            DataTable dt = new DataTable();
            foreach (ListItem li in listboxmonth.Items)
            {
                if (li.Selected == true)
                {
                    MonthText += li.Text + ",";
                    Month += li.Value + ",";
                }

            }
            MonthText = MonthText.TrimEnd(',');
            Month = Month.TrimEnd(',');
            foreach (ListItem li in ListBox2.Items)
            {
                if (li.Selected == true)
                {
                    // Distid += li.Text + ",";
                    Distid += li.Value + ",";
                }

            }
            Distid = Distid.TrimEnd(',');
            //Distid = Distid.TrimEnd(',');
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
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
                string year = "";
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                    {
                        year += item.Text + ",";
                    }
                }
                year = year.TrimStart(',').TrimEnd(',');
                if (year != "")
                {
                    // smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();

                    if (Month.Length != 26)
                    {

                        if (approveStatusRadioButtonList.SelectedValue == "Quantity")
                        {

                            if (MonthText.Contains("Apr"))
                            {
                                if (str.Length != 0)
                                {
                                    str = " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("May"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate ";
                                }

                            }
                            if (MonthText.Contains("Jun"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }

                            }
                            if (MonthText.Contains("Jul"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid, da.PartyName, mi.ItemName,ts1.VDate";
                                }

                            }
                            if (MonthText.Contains("Aug"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }

                            }
                            if (MonthText.Contains("Sep"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId   where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }

                            }
                            if (MonthText.Contains("Oct"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Nov"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Dec"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Jan"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Feb"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Mar"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }

                            Query = @"select a.SyncId,a.distributor,a.MaterialGROUP,a.Item,Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,
                    SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                    (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from ( " + str + " ) a group by a.MaterialGROUP,a.Item,a.SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year";

                        }
                        else
                        {

                            if (MonthText.Contains("Apr"))
                            {
                                if (str.Length != 0)
                                {
                                    str = " union all SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";

                                }
                            }

                            if (MonthText.Contains("May"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";

                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Jun"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Jul"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Aug"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ")and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Sep"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Oct"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Nov"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Dec"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Jan"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                            }
                            if (MonthText.Contains("Feb"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }

                            }
                            if (MonthText.Contains("Mar"))
                            {
                                if (str.Length != 0)
                                {
                                    str = str + " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }
                                else
                                {
                                    str = " SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate";
                                }

                            }
                        }
                        Query = @"select a.SyncId,a.distributor,a.MaterialGROUP,a.Item,Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,
                            SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                            (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from ( " + str + " ) a group by a.MaterialGROUP,a.Item,a.SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year ";

                    }

                    else
                    {

                        if (approveStatusRadioButtonList.SelectedValue == "Quantity")
                        {
                            Query = @"select a.SyncId,a.distributor,a.MaterialGROUP,a.Item,Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                           (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from (

                           SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate )a group by a.MaterialGROUP,a.Item,a.SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year ";
                        }
                        else
                        {
                            Query = @"select a.SyncId,a.distributor,a.MaterialGROUP,a.Item,Year=(CASE WHEN a.year = '2018' THEN '2018-2019' WHEN a.year = '2017' THEN '2017-2018' WHEN a.year = '2016' THEN '2016-2017' WHEN a.year = '2015' THEN '2015-2016' WHEN a.year = '2014' THEN '2014-2015' ELSE NULL END),SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,
                                   SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                                  (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d from (

                              SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId   where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ")and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate  " +
                             " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.Syncid,da.PartyName, mi.ItemName,ts1.VDate )a group by a.MaterialGROUP,a.Item,a.SyncId,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year";
                        }
                    }
                }

                dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, Query); ;



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
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                        else
                        {
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=MonthlyItemSaleReport.csv");
                Response.Write(sb.ToString());
                Response.End();

                //Response.Clear();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment;filename=MonthlyItemSaleReport.xls");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                //monthlyItemRpt.RenderControl(hw);
                //Response.Output.Write(sw.ToString());
                //Response.Flush();
                //Response.End();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void ExportCSV(object sender, EventArgs e)
        {
            string smIDStr = "", smIDStr1 = "", Query = "";
            DataTable dt = new DataTable();
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
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
                string year = "";
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                    {
                        year += item.Text + ",";
                    }
                }
                year = year.TrimStart(',').TrimEnd(',');
                if (year != "")
                {

                    if (approveStatusRadioButtonList.SelectedValue == "Quantity")
                    {
                        Query = @"select a.Year,a.distributor,a.MaterialGROUP,a.Item,SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec from (

                             SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Qty) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=07) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=10) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate)in (" + year + ") and month(ts1.Vdate)=11) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                     " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.PartyName, mi.ItemName,ts1.VDate    )a group by a.MaterialGROUP,a.Item,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year ";
                    }
                    else
                    {
                        Query = @"select a.Year,a.distributor,a.MaterialGROUP,a.Item,SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as Mar,SUM(a.t4_value) as Apr,SUM(a.t5_value) as May,SUM(a.t6_value) as Jun,SUM(a.t7_value) as Jul,SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec from ( 

                              SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,SUM (ts1.Amount) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=04) group BY da.PartyName, mi.ItemName,ts1.VDate " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value,SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=05) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=06) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=07) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=08) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=09) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=10) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=11) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)=12) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=01) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=02) group BY da.PartyName, mi.ItemName,ts1.VDate  " +
                         " UNION ALL SELECT max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],Datepart(yyyy,ts1.VDate) AS Year,0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=ts1.DistId  left join transdistinv tdin on tdin.distinvdocid=ts1.distinvdocid  where tdin.smid in (select smid from mastsalesrepgrp where maingrp IN (" + smIDStr1 + ")) and (year(ts1.Vdate) in ((" + year + ") + 1) and month(ts1.Vdate)=03) group BY da.PartyName, mi.ItemName,ts1.VDate )a group by a.MaterialGROUP,a.Item,a.distributor,a.Year Order by a.distributor,a.MaterialGROUP,a.Year";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


                }


            }
            {

                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Header row for CSV file.
                    csv += column.ColumnName + ',';
                }

                //Add new line.
                csv += "\r\n";

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        //Add the Data rows.
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }

                    //Add new line.
                    csv += "\r\n";
                }

                //Download the CSV file.
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=MonthlyItemSale.csv");
                // string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Grand Total".TrimStart('"').TrimEnd('"');
                //StringBuilder sb = new StringBuilder();
                //sb.Append();
                //sb.AppendLine(System.Environment.NewLine);
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();
            }
        }

        private void BindDistributorSalesmanWise(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    //string citystr = "";
                    //string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    //DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    //for (int i = 0; i < dtCity.Rows.Count; i++)
                    //{
                    //    citystr += dtCity.Rows[i]["AreaId"] + ",";
                    //}
                    //citystr = citystr.TrimStart(',').TrimEnd(',');                  

                    //string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";

                    string distqry = @"select PartyId,PartyName from MastParty where smid in (SELECT smid FROM MastSalesRepGrp WHERE maingrp in (" + SMIDStr + "))  and PartyDist=1 and Active=1 order by PartyName";

                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox2.DataSource = dtDist;
                        ListBox2.DataTextField = "PartyName";
                        ListBox2.DataValueField = "PartyId";
                        ListBox2.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    ListBox2.DataSource = null;
                    ListBox2.DataBind();
                }
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
                if (SMIDStr != "")
                {
                    string citystr = "";
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');
                    //string distqry = @"select * from MastParty where CityId in (" + citystr + ") and PartyDist=1 and Active=1 order by PartyName";       
                    string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox2.DataSource = dtDist;
                        ListBox2.DataTextField = "PartyName";
                        ListBox2.DataValueField = "PartyId";
                        ListBox2.DataBind();

                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    ListBox2.DataSource = null;
                    ListBox2.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {

            //string smIDStr = "", smIDStr12 = "";

            //{
            //    foreach (TreeNode node in trview.CheckedNodes)
            //    {
            //        smIDStr12 = node.Value;
            //        {
            //            smIDStr += node.Value + ",";
            //        }
            //    }
            //    smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
            //    Session["treenodes"] = smIDStr12;

            //}
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
                    string compcode = DbConnectionDAL.GetStringScalarVal("select compcode from mastenviro");
                    if (!string.IsNullOrEmpty(compcode))
                    {
                        if (compcode == "selzer")
                        {
                            BindDistributorSalesmanWise(smIDStr12);
                        }
                        else
                        {
                            BindDistributorDDl(smIDStr12);
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No CompCode Exist');", true);

                        ListBox2.Items.Clear();
                        ListBox2.DataBind();
                    }
                }
                else
                {
                    ListBox2.Items.Clear();
                    ListBox2.DataBind();
                }
                ViewState["tree"] = smiMStr;
            }
            cnt = cnt + 1;



        }

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem li in listboxmonth.Items)
            {
                li.Selected = true;

            }
        }
    }
}