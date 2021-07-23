using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class BrandSaleReport : System.Web.UI.Page
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
                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());
                brandSalerpt.DataSource = distributors;
                brandSalerpt.DataBind();
                //trview.Attributes.Add("onclick", "postBackByObject()");
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
                //Ankita - 17/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalePersonDDl();
                //fill_TreeArea();
                BindTreeViewControl();
                BindMaterialGroup();
                BindStaffChkList(CheckBoxList1);
                BindProductClass();
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

        public class Distributors
        {
            public string distributor { get; set; }
            public string ProductClass { get; set; }
            public string MaterialGROUP { get; set; }
            public string Year { get; set; }
            public string t1Value { get; set; }
            public string t2Value { get; set; }
            public string t3Value { get; set; }
            public string t4Value { get; set; }
            public string t5Value { get; set; }
            public string t6Value { get; set; }
            public string t7Value { get; set; }
            public string t8Value { get; set; }
            public string t9Value { get; set; }
            public string t10Value { get; set; }
            public string t11Value { get; set; }
            public string t12Value { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetBrandSale(string Distid, string ProductClass, string ProductGroup, string Product, string year, string View)
        {

            string query, Querystr = "";

            if (ProductClass != "")
            { Querystr = " and mi.ClassId in (" + ProductClass + ")"; }

            if (ProductGroup != "")
            { Querystr += " and mi1.ItemId in (" + ProductGroup + ")"; }

            if (Product != "")
            { Querystr += " and mi.ItemId in (" + Product + ")"; }

            if (View == "Quantity")
            {
                query = @"select a.Year,a. SyncId,a.distributor,a.ProductClass, a.MaterialGROUP,SUM(a.t1_Value) as t1Value,SUM(a.t2_value) as t2Value,SUM(a.t3_value) as t3Value,SUM(a.t4_value) as t4Value,
                       SUM(a.t5_value) as t5Value,SUM(a.t6_value) as t6Value,SUM(a.t7_value) as t7Value,SUM(a.t7_value) as t7Value,
                       SUM(a.t8_value) as t8Value,SUM(a.t9_value) as t9Value,SUM(a.t10_value) as t10Value,SUM(a.t11_value) as t11Value,SUM(a.t12_value) as t12Value from (

                       SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid, da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='01') " + Querystr + "  group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,SUM (ts1.Qty) as t2_Qty,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ")  and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='02') " + Querystr + "  group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_Qty,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='03') " + Querystr + "   group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_Qty,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='04') " + Querystr + "   group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_Qty,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='05') " + Querystr + "   group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_Qty,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='06') " + Querystr + "   group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_Qty,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='07') " + Querystr + "  group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_Qty,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='08') " + Querystr + "   group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_Qty,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='09') " + Querystr + "   group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_Qty,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='10') " + Querystr + "  group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='11') " + Querystr + "  group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                     " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='12') " + Querystr + "  group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName )a group by a.Year,a.SyncId,a.productClass,a.MaterialGROUP,a.distributor Order by a.Year DESC,a.distributor,a.productClass,a.MaterialGROUP";
            }
            else
            {
                query = @" select a.Year,a.SyncId,a.distributor,a.ProductClass,a.MaterialGROUP,SUM(a.t1_Value) as t1Value,SUM(a.t2_value) as t2Value,SUM(a.t3_value) as t3Value,SUM(a.t4_value) as t4Value,
                      SUM(a.t5_value) as t5Value,SUM(a.t6_value) as t6Value,SUM(a.t7_value) as t7Value,SUM(a.t7_value) as t7Value,
                      SUM(a.t8_value) as t8Value,SUM(a.t9_value) as t9Value,SUM(a.t10_value) as t10Value,SUM(a.t11_value) as t11Value,SUM(a.t12_value) as t12Value from ( 

             SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],SUM (ts1.Net_Total) as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='01') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value , SUM (ts1.Net_Total) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='02')  " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='03') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='04') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='05') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='06') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='07') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and  month(ts1.Vdate)='08') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='09') " + Querystr + "  group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='10') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='11') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
             " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + Distid + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='12') " + Querystr + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName )a group by a.Year,a.SyncId,a.productClass,a.MaterialGROUP,a.distributor Order by a.Year DESC,a.distributor,productClass,a.MaterialGROUP";

            }


            DataTable dtbrandsale = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtbrandsale);


        }

        private void BindMaterialGroup()
        {
            try
            {//Ankita - 17/may/2016- (For Optimization)
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
        private void BindProductClass()
        {
            try
            {//Ankita - 17/may/2016- (For Optimization)
                //string quey = @"select * from MastItemClass mic where mic.Active=1 order by mic.Name";
                string quey = @"select Id,Name from MastItemClass mic where mic.Active=1 order by mic.Name";
                DataTable dtClassQry = DbConnectionDAL.GetDataTable(CommandType.Text, quey);
                if (dtClassQry.Rows.Count > 0)
                {
                    productClassListBox.DataSource = dtClassQry;
                    productClassListBox.DataTextField = "Name";
                    productClassListBox.DataValueField = "Id";
                    productClassListBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        void fill_TreeArea()
        {
            //int lowestlvl = 0;
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {

                //string strrole = "select SMID,SMName from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                //St = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                //    lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,1);
                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {//Ankita - 17/may/2016- (For Optimization)
                //lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID, lowestlvl);
                St = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT mastrole.rolename,mastsalesrep.smid,smname + ' (' + mastsalesrep.syncid + ' - '+ mastrole.rolename + ')' AS smname, mastsalesrep.lvl,mastrole.roletype FROM   mastsalesrep LEFT JOIN mastrole ON mastrole.roleid = mastsalesrep.roleid WHERE  smid =" + Settings.Instance.SMID + "");
            }


            trview.Nodes.Clear();

            if (St.Rows.Count <= 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found !');", true);
                return;
            }
            foreach (DataRow row in St.Rows)
            {
                TreeNode tnParent = new TreeNode();
                tnParent.Text = row["SMName"].ToString();
                tnParent.Value = row["SMId"].ToString();
                trview.Nodes.Add(tnParent);
                //tnParent.ExpandAll();
                tnParent.CollapseAll();
                //Ankita - 17/may/2016- (For Optimization)
                //FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
                getchilddata(tnParent, tnParent.Value);
            }
        }
        //Ankita - 17/may/2016- (For Optimization)
        private void getchilddata(TreeNode parent, string ParentId)
        {

            string SmidVar = string.Empty;
            string GetFirstChildData = string.Empty;
            int levelcnt = 0;
            if (Settings.Instance.RoleType == "Admin")
                //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
                levelcnt = Convert.ToInt16("0") + 2;
            else
                levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 1;


            GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 order by SMName,lvl desc ";
            DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

            if (FirstChildDataDt.Rows.Count > 0)
            {

                for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
                {
                    SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
                    FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
                }
                SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

                // for (int j = levelcnt + 1; j <= 5; j++)
                for (int j = levelcnt + 1; j <= 9; j++)
                {
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
                    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
                    if (dtChild.Rows.Count > 0)
                    {
                        SmidVar = string.Empty;
                        int mTotRows = dtChild.Rows.Count;
                        if (mTotRows > 0)
                        {
                            var str = "";
                            for (int k = 0; k < mTotRows; k++)
                            {
                                SmidVar += dtChild.Rows[k]["smid"].ToString() + ",";
                            }

                            TreeNode Oparent = parent;
                            switch (j)
                            {
                                case 3:
                                    if (Oparent.Parent != null)
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    break;
                                case 4:
                                    if (Oparent.Parent != null)
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }
                                    break;
                                case 5:
                                    if (Oparent.Parent != null)
                                    {
                                        foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                        {
                                            foreach (TreeNode child in Pchild.ChildNodes)
                                            {
                                                str += child.Value + ","; parent = child;
                                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                for (int l = 0; l < dr.Length; l++)
                                                {
                                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                }
                                                dtChild.Select();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }


                                    break;
                                case 6:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode Qchild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode child in Qchild.ChildNodes)
                                                    {
                                                        str += child.Value + ","; parent = child;
                                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                        for (int l = 0; l < dr.Length; l++)
                                                        {
                                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                        }
                                                        dtChild.Select();
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }
                                    break;
                                case 7:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode schild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode Qchild in schild.ChildNodes)
                                                    {
                                                        foreach (TreeNode child in Qchild.ChildNodes)
                                                        {
                                                            str += child.Value + ","; parent = child;
                                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                            for (int l = 0; l < dr.Length; l++)
                                                            {
                                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                            }
                                                            dtChild.Select();
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }


                                    break;
                                case 8:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode Qchild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode child in Qchild.ChildNodes)
                                                    {
                                                        str += child.Value + ","; parent = child;
                                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                        for (int l = 0; l < dr.Length; l++)
                                                        {
                                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                        }
                                                        dtChild.Select();
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }


                                    break;
                                case 9:
                                    if (Oparent.Parent != null)
                                    {
                                        if (Settings.Instance.RoleType == "Admin")
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode Qchild in Pchild.ChildNodes)
                                                {
                                                    foreach (TreeNode child in Qchild.ChildNodes)
                                                    {
                                                        str += child.Value + ","; parent = child;
                                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                        for (int l = 0; l < dr.Length; l++)
                                                        {
                                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                        }
                                                        dtChild.Select();
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
                                            {
                                                foreach (TreeNode child in Pchild.ChildNodes)
                                                {
                                                    str += child.Value + ","; parent = child;
                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                                    for (int l = 0; l < dr.Length; l++)
                                                    {
                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                                    }
                                                    dtChild.Select();
                                                }
                                            }
                                        }

                                    }

                                    else
                                    {
                                        foreach (TreeNode child in Oparent.ChildNodes)
                                        {
                                            str += child.Value + ","; parent = child;
                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
                                            for (int l = 0; l < dr.Length; l++)
                                            {
                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
                                            }
                                            dtChild.Select();
                                        }
                                    }


                                    break;
                            }

                            SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);
                        }
                    }
                }
            }


        }

        public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        {
            TreeNode child = new TreeNode();
            child.Text = SMName;
            child.Value = Smid;
            child.SelectAction = TreeNodeSelectAction.Expand;
            parent.ChildNodes.Add(child);
            child.CollapseAll();
        }
        //public void FillChildArea(TreeNode parent, string ParentId, int LVL, int SMId)
        //{
        //    //var AreaQueryChild = "select * from Mastsalesrep where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " order by SMName,lvl";
        //    var AreaQueryChild = "SELECT SMId,Smname +' ('+ ms.Syncid + ' - ' + mr.RoleName + ')' as smname,Lvl from Mastsalesrep ms LEFT JOIN mastrole mr ON mr.RoleId=ms.RoleId where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " and ms.Active=1 order by SMName,lvl";
        //    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
        //    parent.ChildNodes.Clear();
        //    foreach (DataRow dr in dtChild.Rows)
        //    {
        //        TreeNode child = new TreeNode();
        //        child.Text = dr["SMName"].ToString().Trim();
        //        child.Value = dr["SMId"].ToString().Trim();
        //        child.SelectAction = TreeNodeSelectAction.Expand;
        //        parent.ChildNodes.Add(child);
        //        //child.ExpandAll();
        //        child.CollapseAll();
        //        FillChildArea(child, child.Value, (Convert.ToInt32(dr["Lvl"])), Convert.ToInt32(dr["SMId"].ToString()));
        //    }

        //}

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
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    ListBox1.DataSource = null;
                    ListBox1.DataBind();
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
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();

                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    brandSalerpt.DataSource = null;
                    brandSalerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        //private void GetRoleType(string p)
        //{
        //    try
        //    {
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
        private void BindStaffChkList(CheckBoxList accStaffCheckBoxList)
        {

            try
            {
                for (int i = System.DateTime.Now.Year; i >= (System.DateTime.Now.Year - 2); i--)
                {
                    accStaffCheckBoxList.Items.Add(new ListItem(i.ToString()));
                    //        ddlYearSecSale.Items.Add(new ListItem(i.ToString()));
                }

                //accStaffCheckBoxList.DataSource = dtNew;
                //accStaffCheckBoxList.DataTextField = "SMName";
                //accStaffCheckBoxList.DataValueField = "SMId";
                accStaffCheckBoxList.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        //protected void salespersonListBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string smIDStr = "";
        //    string smIDStr12 = "", Qrychk = "", Query = "";
        //    //         string message = "";
        //    foreach (ListItem saleperson in salespersonListBox.Items)
        //    {
        //        if (saleperson.Selected)
        //        {
        //            smIDStr12 += saleperson.Value + ",";
        //        }
        //    }
        //    smIDStr12 = smIDStr12.TrimStart(',').TrimEnd(',');
        //    BindDistributorDDl(smIDStr12);
        //}

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string smIDStr = "", prodClassStr = "", matGrpStr = "", prodStr = "";
            string distIdStr1 = "", QueryProdClass = "", QueryMatGrp = "", QueryProd = "", Query = "", smIDStr1 = "";

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
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        distIdStr1 += item.Value + ",";
                    }
                }
                distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');

                if (distIdStr1 != "")
                {
                    //For CheckBoxList
                    string year = "";
                    foreach (ListItem item in CheckBoxList1.Items)
                    {
                        if (item.Selected)
                        {
                            year += item.Text + ",";
                        }
                    }
                    year = year.TrimStart(',').TrimEnd(',');

                    //
                    if (year != "")
                    {
                        foreach (ListItem prodClass in productClassListBox.Items)
                        {
                            if (prodClass.Selected)
                            {
                                prodClassStr += prodClass.Value + ",";
                            }
                        }
                        prodClassStr = prodClassStr.TrimStart(',').TrimEnd(',');
                        if (prodClassStr != "")
                        {
                            QueryProdClass = " and mi.ClassId in (" + prodClassStr + ")";
                        }

                        //
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
                            QueryMatGrp = " and mi1.ItemId in (" + matGrpStr + ")";
                        }
                        //
                        foreach (ListItem prodList in productListBox.Items)
                        {
                            if (prodList.Selected)
                            {
                                prodStr += prodList.Value + ",";
                            }
                        }
                        prodStr = prodStr.TrimStart(',').TrimEnd(',');
                        if (prodStr != "")
                        {
                            if (prodClassStr != "" && matGrpStr != "")
                            {
                                QueryProdClass = "";
                                QueryMatGrp = "";
                                QueryProd = " and mi.ItemId in (" + prodStr + ")";
                            }
                            else if (matGrpStr != "")
                            {
                                QueryMatGrp = "";
                                QueryProd = " and mi.ItemId in (" + prodStr + ")";
                            }
                            else
                            {
                                QueryProd = " and mi.ItemId in (" + prodStr + ")";
                            }
                        }

                        if (viewasRadioButtonList.SelectedValue == "Quantity")
                        {
                            Query = @"select a.Year,a.distributor,a.ProductClass, a.MaterialGROUP,SUM(a.t1_Value) as t1Value,SUM(a.t2_value) as t2Value,SUM(a.t3_value) as t3Value,SUM(a.t4_value) as t4Value,
                       SUM(a.t5_value) as t5Value,SUM(a.t6_value) as t6Value,SUM(a.t7_value) as t7Value,SUM(a.t7_value) as t7Value,
                       SUM(a.t8_value) as t8Value,SUM(a.t9_value) as t9Value,SUM(a.t10_value) as t10Value,SUM(a.t11_value) as t11Value,SUM(a.t12_value) as t12Value from (

                       SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='01') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,SUM (ts1.Qty) as t2_Qty,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ")  and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='02') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_Qty,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='03') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_Qty,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='04') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_Qty,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='05') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_Qty,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='06') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_Qty,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='07') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_Qty,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='08') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_Qty,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='09') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_Qty,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='10') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='11') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='12') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName )a group by a.Year, a.productClass,a.MaterialGROUP,a.distributor Order by a.Year DESC,a.distributor,a.productClass,a.MaterialGROUP";
                        }

                        else
                        {

                            Query = @" select a.Year, a.distributor,a.ProductClass,a.MaterialGROUP,SUM(a.t1_Value) as t1Value,SUM(a.t2_value) as t2Value,SUM(a.t3_value) as t3Value,SUM(a.t4_value) as t4Value,
                      SUM(a.t5_value) as t5Value,SUM(a.t6_value) as t6Value,SUM(a.t7_value) as t7Value,SUM(a.t7_value) as t7Value,
                      SUM(a.t8_value) as t8Value,SUM(a.t9_value) as t9Value,SUM(a.t10_value) as t10Value,SUM(a.t11_value) as t11Value,SUM(a.t12_value) as t12Value from ( 

                      SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],SUM (ts1.Net_Total) as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='01') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value , SUM (ts1.Net_Total) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='02')  " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,SUM (ts1.Net_Total) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='03') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Net_Total) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='04') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Net_Total) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='05') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Net_Total) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='06') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Net_Total) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='07') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Net_Total) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and  month(ts1.Vdate)='08') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Net_Total) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='09') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Net_Total) as t10_value,0 as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='10') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Net_Total) as t11_value,0 as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='11') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Net_Total) as t12_value,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='12') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate, da.PartyName,mc.Name, mi.ItemName )a group by a.Year, a.productClass,a.MaterialGROUP,a.distributor Order by a.Year DESC,a.distributor,productClass,a.MaterialGROUP";

                        }

                        DataTable dtBrandSaleRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        if (dtBrandSaleRep.Rows.Count > 0)
                        {
                            rptDiv.Style.Add("display", "block");
                            brandSalerpt.DataSource = dtBrandSaleRep;
                            brandSalerpt.DataBind();
                            btnExport.Visible = true;

                        }
                        else
                        {
                            rptDiv.Style.Add("display", "block");
                            brandSalerpt.DataSource = dtBrandSaleRep;
                            brandSalerpt.DataBind();
                            btnExport.Visible = false;
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select year');", true);
                        brandSalerpt.DataSource = null;
                        brandSalerpt.DataBind();
                    }


                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                    brandSalerpt.DataSource = null;
                    brandSalerpt.DataBind();
                }


            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                brandSalerpt.DataSource = null;
                brandSalerpt.DataBind();
            }


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BrandSaleReport.aspx");
        }

        protected void productClassListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string podClassStr = "";            
            //foreach (ListItem prodClass in productClassListBox.Items)
            //{
            //    if (prodClass.Selected)
            //    {
            //        podClassStr += prodClass.Value + ",";
            //    }
            //}
            //podClassStr = podClassStr.TrimStart(',').TrimEnd(',');
            //BindProductListBox(podClassStr);
        }

        private void BindProductListBox(string prodClassStr)
        {
            try
            {
                if (prodClassStr != "")
                {//Ankita - 17/may/2016- (For Optimization)
                    //string productListQry = @"select * from MastItem where ClassId in (" + prodClassStr + ") and ItemType='ITEM' and Active=1";
                    string productListQry = @"select ItemId,ItemName from MastItem where ClassId in (" + prodClassStr + ") and ItemType='ITEM' and Active=1";
                    DataTable dtProdList = DbConnectionDAL.GetDataTable(CommandType.Text, productListQry);

                    if (dtProdList.Rows.Count > 0)
                    {
                        ViewState["dtProducList"] = dtProdList;
                        productListBox.DataSource = dtProdList;
                        productListBox.DataTextField = "ItemName";
                        productListBox.DataValueField = "ItemId";
                        productListBox.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select product class');", true);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void matGrpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string matGrpStr = "";
            //foreach (ListItem matGrp in matGrpListBox.Items)
            //{
            //    if (matGrp.Selected)
            //    {
            //        matGrpStr += matGrp.Value + ",";
            //    }
            //}
            //matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
            //if (matGrpStr != "")
            //{
            //    string productListQry1 = @"select ItemId, ItemName from MastItem where UnderId in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
            //    DataTable dtProdList1 = DbConnectionDAL.GetDataTable(CommandType.Text, productListQry1);

            //    if (dtProdList1.Rows.Count > 0)
            //    {
            //        productListBox.DataSource = dtProdList1;
            //        productListBox.DataTextField = "ItemName";
            //        productListBox.DataValueField = "ItemId";
            //        productListBox.DataBind();
            //    }
            //}

            //try
            //{
            //    string matGrpStr = "";
            //    foreach (ListItem matGrp in matGrpListBox.Items)
            //    {
            //        if (matGrp.Selected)
            //        {
            //            matGrpStr += matGrp.Value + ",";
            //        }
            //    }
            //    matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
            //    if (ViewState["dtProducList"] != null)
            //    {
            //        DataTable dt = (DataTable)ViewState["dtProducList"];
            //        DataView dv = new DataView(dt);
            //        dv.RowFilter = "UnderId in (" + matGrpStr + ") and ItemType='ITEM' and  Active=1";
            //        productListBox.DataSource = dv.ToTable();
            //        productListBox.DataTextField = "ItemName";
            //        productListBox.DataValueField = "ItemId";
            //        productListBox.DataBind();
            //    }
            //    else
            //    {
            //        if (matGrpStr != "")
            //        {
            //            string productListQry1 = @"select * from MastItem where UnderId in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
            //            DataTable dtProdList1 = DbConnectionDAL.GetDataTable(CommandType.Text, productListQry1);

            //            if (dtProdList1.Rows.Count > 0)
            //            {
            //                productListBox.DataSource = dtProdList1;
            //                productListBox.DataTextField = "ItemName";
            //                productListBox.DataValueField = "ItemId";
            //                productListBox.DataBind();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BrandSaleReport.csv");
            string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Grand Total".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProductClass", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProductGroup", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Year", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Jan", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Feb", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mar", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Apr", typeof(String)));
            dtParams.Columns.Add(new DataColumn("May", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Jun", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Jul", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Aug", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Sep", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Oct", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Nov", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Dec", typeof(String)));
            dtParams.Columns.Add(new DataColumn("GrandTotal", typeof(String)));

            foreach (RepeaterItem item in brandSalerpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label distributorLabel = item.FindControl("distributorLabel") as Label;
                dr["DistributorName"] = distributorLabel.Text;
                Label ProductClassLabel = item.FindControl("ProductClassLabel") as Label;
                dr["ProductClass"] = ProductClassLabel.Text.ToString();
                Label MaterialGROUPLabel = item.FindControl("MaterialGROUPLabel") as Label;
                dr["ProductGroup"] = MaterialGROUPLabel.Text.ToString();
                Label YearLabel = item.FindControl("YearLabel") as Label;
                dr["Year"] = YearLabel.Text.ToString();
                Label t1ValueLabel = item.FindControl("t1ValueLabel") as Label;
                dr["Jan"] = t1ValueLabel.Text.ToString();
                Label t2ValueLabel = item.FindControl("t2ValueLabel") as Label;
                dr["Feb"] = t2ValueLabel.Text.ToString();
                Label t3ValueLabel = item.FindControl("t3ValueLabel") as Label;
                dr["Mar"] = t3ValueLabel.Text.ToString();
                Label t4ValueLabel = item.FindControl("t4ValueLabel") as Label;
                dr["Apr"] = t4ValueLabel.Text.ToString();
                Label t5ValueLabel = item.FindControl("t5ValueLabel") as Label;
                dr["May"] = t5ValueLabel.Text.ToString();
                Label t6ValueLabel = item.FindControl("t6ValueLabel") as Label;
                dr["Jun"] = t6ValueLabel.Text.ToString();
                Label t7ValueLabel = item.FindControl("t7ValueLabel") as Label;
                dr["Jul"] = t7ValueLabel.Text.ToString();
                Label t8ValueLabel = item.FindControl("t8ValueLabel") as Label;
                dr["Aug"] = t8ValueLabel.Text.ToString();
                Label t9ValueLabel = item.FindControl("t9ValueLabel") as Label;
                dr["Sep"] = t9ValueLabel.Text.ToString();
                Label t10ValueLabel = item.FindControl("t10ValueLabel") as Label;
                dr["Oct"] = t10ValueLabel.Text.ToString();
                Label t11ValueLabel = item.FindControl("t11ValueLabel") as Label;
                dr["Nov"] = t11ValueLabel.Text.ToString();
                Label t12ValueLabel = item.FindControl("t12ValueLabel") as Label;
                dr["Dec"] = t12ValueLabel.Text.ToString();
                Label GrandTotalLabel = item.FindControl("GrandTotalLabel") as Label;
                dr["GrandTotal"] = GrandTotalLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
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
            Response.AddHeader("content-disposition", "attachment;filename=BrandSaleReport.csv");
            Response.Write(sb.ToString());
            Response.End();

            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=BrandSaleReport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //brandSalerpt.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();

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

                        ListBox1.Items.Clear();
                        ListBox1.DataBind();
                    }
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

        protected void ExportCSV(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BrandSaleReport.csv");
            string headertext = "Distributor SyncId".TrimStart('"').TrimEnd('"') + ", " + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Grand Total".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            DataTable dtParams = new DataTable();
            string smIDStr = "", prodClassStr = "", matGrpStr = "", prodStr = "";
            string distIdStr1 = "", QueryProdClass = "", QueryMatGrp = "", QueryProd = "", Query = "", smIDStr1 = "";

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
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        distIdStr1 += item.Value + ",";
                    }
                }
                distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');

                if (distIdStr1 != "")
                {
                    //For CheckBoxList
                    string year = "";
                    foreach (ListItem item in CheckBoxList1.Items)
                    {
                        if (item.Selected)
                        {
                            year += item.Text + ",";
                        }
                    }
                    year = year.TrimStart(',').TrimEnd(',');

                    //
                    if (year != "")
                    {
                        foreach (ListItem prodClass in productClassListBox.Items)
                        {
                            if (prodClass.Selected)
                            {
                                prodClassStr += prodClass.Value + ",";
                            }
                        }
                        prodClassStr = prodClassStr.TrimStart(',').TrimEnd(',');
                        if (prodClassStr != "")
                        {
                            QueryProdClass = " and mi.ClassId in (" + prodClassStr + ")";
                        }

                        //
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
                            QueryMatGrp = " and mi1.ItemId in (" + matGrpStr + ")";
                        }
                        //
                        foreach (ListItem prodList in productListBox.Items)
                        {
                            if (prodList.Selected)
                            {
                                prodStr += prodList.Value + ",";
                            }
                        }
                        prodStr = prodStr.TrimStart(',').TrimEnd(',');
                        if (prodStr != "")
                        {
                            if (prodClassStr != "" && matGrpStr != "")
                            {
                                QueryProdClass = "";
                                QueryMatGrp = "";
                                QueryProd = " and mi.ItemId in (" + prodStr + ")";
                            }
                            else if (matGrpStr != "")
                            {
                                QueryMatGrp = "";
                                QueryProd = " and mi.ItemId in (" + prodStr + ")";
                            }
                            else
                            {
                                QueryProd = " and mi.ItemId in (" + prodStr + ")";
                            }
                        }

                        if (viewasRadioButtonList.SelectedValue == "Quantity")
                        {
                            Query = @"select a.SyncId,a.distributor,a.ProductClass, a.MaterialGROUP,a.Year,SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as March,SUM(a.t4_value) as April,
                       SUM(a.t5_value) as May,SUM(a.t6_value) as June,SUM(a.t7_value) as July,
                       SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                       (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d  from (

                       SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],SUM (ts1.Qty) as t1_Value,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='01') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,SUM (ts1.Qty) as t2_Qty,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ")  and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='02') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,SUM (ts1.Qty) as t3_Qty,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='03') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Qty) as t4_Qty,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='04') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Qty) as t5_Qty,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='05') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Qty) as t6_Qty,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='06') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Qty) as t7_Qty,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='07') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Qty) as t8_Qty,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='08') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Qty) as t9_Qty,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='09') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Qty) as t10_Qty,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='10') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Qty) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='11') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName " +
                            " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Qty) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='12') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid, da.PartyName,mc.Name, mi.ItemName )a group by a.Year, a.productClass,a.MaterialGROUP,a.Syncid,a.distributor Order by a.Year DESC,a.SyncId,a.distributor,a.productClass,a.MaterialGROUP";
                        }

                        else
                        {

                            Query = @"select a.SyncId, a.distributor,a.ProductClass, a.MaterialGROUP,a.Year,SUM(a.t1_Value) as Jan,SUM(a.t2_value) as Feb,SUM(a.t3_value) as March,SUM(a.t4_value) as April,
                       SUM(a.t5_value) as May,SUM(a.t6_value) as June,SUM(a.t7_value) as July,
                       SUM(a.t8_value) as Aug,SUM(a.t9_value) as Sep,SUM(a.t10_value) as Oct,SUM(a.t11_value) as Nov,SUM(a.t12_value) as Dec,
                       (SUM(a.t1_Value) + SUM(a.t2_value)+SUM(a.t3_value)+SUM(a.t4_value)+SUM(a.t5_value)+SUM(a.t6_value)+SUM(a.t7_value)+SUM(a.t8_value)+SUM(a.t9_value)+SUM(a.t10_value)+SUM(a.t11_value)+SUM(a.t12_value) ) as d   from (

                      SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],SUM (ts1.Amount) as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='01') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value , SUM (ts1.Amount) as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='02')  " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,SUM (ts1.Amount) as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where  ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='03') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,SUM (ts1.Amount) as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='04') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,SUM (ts1.Amount) as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='05') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,SUM (ts1.Amount) as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='06') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,SUM (ts1.Amount) as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='07') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,SUM (ts1.Amount) as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and  month(ts1.Vdate)='08') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,SUM (ts1.Amount) as t9_value,0 as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='09') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,SUM (ts1.Amount) as t10_value,0 as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='10') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,SUM (ts1.Amount) as t11_value,0 as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='11') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName " +
                           " UNION ALL SELECT DATEPART(yyyy,ts1.vdate) AS Year, mc.Name AS productClass, max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],0 as t1_Value ,0 as t2_value,0 as t3_value,0 as t4_value,0 as t5_value,0 as t6_value,0 as t7_value,0 as t8_value,0 as t9_value,0 as t10_value,0 as t11_value,SUM (ts1.Amount) as t12_value,da.Syncid,da.PartyName AS Distributor from TransDistInv1 ts1 left outer join MastItem mi on mi.ItemId=ts1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid LEFT JOIN mastitemclass mc ON mc.Id = mi.ClassId left outer join MastParty da on da.PartyId=ts1.DistId where ts1.DistId in (" + distIdStr1 + ") and (year(ts1.Vdate) in (" + year + ") and month(ts1.Vdate)='12') " + QueryProdClass + " " + QueryProd + " " + QueryMatGrp + " group BY ts1.VDate,da.Syncid,da.PartyName,mc.Name, mi.ItemName )a group by a.Year, a.productClass,a.MaterialGROUP,a.SyncId,a.distributor Order by a.Year DESC,a.distributor,productClass,a.MaterialGROUP";

                        }

                        dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        for (int j = 0; j < dtParams.Rows.Count; j++)
                        {
                            for (int k = 0; k < dtParams.Columns.Count; k++)
                            {
                                if (dtParams.Rows[j][k].ToString().Contains(","))
                                {
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
                        Response.AddHeader("content-disposition", "attachment;filename=BrandSaleReport.csv");
                        Response.Write(sb.ToString());
                        Response.End();
                    }
                }
            }
        }
    }
}