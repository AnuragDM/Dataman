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
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class rptAnnual_Growth_V2 : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        DataTable YrTable = new DataTable("Year");
        DataColumn dtColumn;
        DataRow myDataRow;
        //DataSet dataSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;


            YrTable.Columns.Add("YrId", typeof(int));
            YrTable.Columns.Add("YrName", typeof(string));
            if (!IsPostBack)
            {
                BindDDLMonth();
                List<DistributorsMonthlyItem> distMonthlyItem = new List<DistributorsMonthlyItem>();
                distMonthlyItem.Add(new DistributorsMonthlyItem());
                //monthlyItemRpt.DataSource = distMonthlyItem;
                //monthlyItemRpt.DataBind();
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                BindStaffChkList();
                BindItmGrp();
                //btnExport.Visible = true;
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";


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
            public string Total { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetAnnualGrowthSale(string areaid, string beatid, string RetailerId, int year, string PrdGrp, string MonthText, string Month, string View)
        {
            string Query = "", smIDStr1 = "", Qry = "", ProdGr = "";
            string beat_id = "";
            smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
            int daysinFeb = DateTime.DaysInMonth(year, 2);

            if (smIDStr1 != "")
            {

            }

            if (beatid != "" && beatid != "0")
                Qry = Qry + " and b.AreaId in (" + beatid + ")";

            if (RetailerId != "" && RetailerId != "0")
                Qry = Qry + " and p.PartyId in ('" + RetailerId + "')";
            if (PrdGrp != "" && PrdGrp != "0")
                Qry = Qry + " and itm.ItemId in ('" + PrdGrp + "')";


            //Label1.Text = Qry.ToString();
            if (View == "Quantity")
            {
                Query = @"select a.StateName,a.City,a.Area,a.Beat,a.Party As Outlet,a.Mobile,a.ItemGroup,a.Item,a.Year,sum(a.Jan_Qty) as Jan,sum(a.Feb_Qty)as Feb,sum(a.Mar_Qty)as Mar,sum(a.Apr_Qty)as Apr,sum(a.May_Qty)as May,sum(a.Jun_Qty)as Jun,sum(a.Jul_Qty)as Jul,sum(a.Aug_Qty) as Aug,sum(a.Sep_Qty)as Sep,sum(a.Oct_Qty) as Oct,sum(a.Nov_Qty) as Nov,sum(a.Dec_Qty)as Dec,(SUM(a.Jan_Qty) + SUM(a.Feb_Qty)+SUM(a.Mar_Qty)+SUM(a.Apr_Qty)+SUM(a.May_Qty)+SUM(a.Jun_Qty)+SUM(a.Jul_Qty)+SUM(a.Aug_Qty)+SUM(a.Sep_Qty)+SUM(a.Oct_Qty)+SUM(a.Nov_Qty)+SUM(a.Dec_Qty)) as d from ( select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Item,itm.ItemName as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,os.Qty as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Jan/" + year + " 00:00' and '31/Jan/" + year + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,os.Qty as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Feb/" + year + " 00:00' and '" + daysinFeb + "/Feb/" + year + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,os.Qty as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Mar/" + year + " 00:00' and '31/Mar/" + year + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,os.Qty as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Apr/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Apr/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,os.Qty as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/May/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/May/" + (Convert.ToInt32(year) - 1) + " 23:59' " + beat_id + " " +

               "union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "'AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,os.Qty  as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jun/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Jun/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,os.Qty as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jul/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Jul/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,os.Qty as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Aug/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Aug/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,os.Qty as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Sep/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Sep/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,os.Qty as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Oct/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Oct/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,os.Qty as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Nov/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Nov/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,os.Qty as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Dec/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Dec/" + (Convert.ToInt32(year) - 1) + " 23:59' " + beat_id + ") a group by a.Party,a.Beat,a.Item,a.Year,a.ItemGroup,a.Area,a.Mobile,a.City,a.StateName Order by a.party";
            }
            else
            {
                Query = @"select a.StateName,a.City,a.Area,a.Beat,a.Party As Outlet,a.Mobile,a.ItemGroup,a.Item,a.Year,convert(varchar,convert(numeric (18,2),sum(a.Jan_Value))) as Jan,convert(varchar,convert(numeric (18,2),sum(a.Feb_Value))) as Feb,convert(varchar,convert(numeric (18,2),sum(a.Mar_value))) as Mar,convert(varchar,convert(numeric (18,2),sum(a.Apr_Value))) as Apr,convert(varchar,convert(numeric (18,2),sum(a.May_Value))) as May,convert(varchar,convert(numeric (18,2),sum(a.Jun_Value))) as Jun,convert(varchar,convert(numeric (18,2),sum(a.Jul_Value))) as Jul,convert(varchar,convert(numeric (18,2),sum(a.Aug_Value))) as Aug,convert(varchar,convert(numeric (18,2),sum(a.Sep_Value))) as Sep,convert(varchar,convert(numeric (18,2),sum(a.Oct_Value))) as Oct,convert(varchar,convert(numeric (18,2),sum(a.Nov_Value))) as Nov,convert(varchar,convert(numeric (18,2),sum(a.Dec_Value))) as Dec,(SUM(a.Jan_Value) + SUM(a.Feb_Value)+SUM(a.Mar_value)+SUM(a.Apr_Value)+SUM(a.May_Value)+SUM(a.Jun_Value)+SUM(a.Jul_Value)+SUM(a.Aug_Value)+SUM(a.Sep_Value)+SUM(a.Oct_Value)+SUM(a.Nov_Value)+SUM(a.Dec_Value) ) as Total from ( select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Item,itm.ItemName AS ItemGroup ,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,os.Qty as Jan_Qty,(os.Qty*os.Rate) as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jan/" + year + " 00:00' and '31/Jan/" + year + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,os.Qty as Feb_Qty,(os.Qty*os.Rate) as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Feb/" + year + " 00:00' and '" + daysinFeb + "/Feb/" + year + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,os.Qty as Mar_Qty,(os.Qty*os.Rate) as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Mar/" + year + " 00:00' and '31/Mar/" + year + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,os.Qty as Apr_Qty,(os.Qty*os.Rate) as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Apr/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Apr/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,os.Qty as May_Qty,(os.Qty*os.Rate) as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/May/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/May/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,os.Qty  as Jun_Qty,(os.Qty*os.Rate) as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Jun/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Jun/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,os.Qty as Jul_Qty,(os.Qty*os.Rate) as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jul/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Jul/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,os.Qty as Aug_Qty,(os.Qty*os.Rate) as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Aug/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Aug/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, os.Qty as Sep_Qty,(os.Qty*os.Rate) as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Sep/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Sep/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,os.Qty as Oct_Qty,(os.Qty*os.Rate) as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Oct/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Oct/" + (Convert.ToInt32(year) - 1) + " 23:59'  " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,os.Qty as Nov_Qty,(os.Qty*os.Rate) as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Nov/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Nov/" + (Convert.ToInt32(year) - 1) + " 23:59'  " +

                " union all select os.partyid,p.partyname as Party,p.Mobile as Mobile,b.areaname as Beat,mb.areaname as Area,ms.areaname as City,msp.AreaName AS StateName,i.Itemname as Mon1_Item,itm.ItemName as Mon1_ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value ,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,os.Qty as Dec_Qty,(os.Qty*os.Rate) as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Dec/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Dec/" + (Convert.ToInt32(year) - 1) + " 23:59' ) a group by a.Party,a.Beat,a.Item,a.Year,a.ItemGroup,a.Area,a.Mobile,a.City,a.StateName Order by a.party";
            }

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


            // Create a DataSet and put both tables in it.
            DataSet dataSet = new DataSet("General");
            dataSet.Tables.Add(dtItem);

            try
            {
                Excel.Application ExcelApp = new Excel.Application();
                string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                {
                    Directory.CreateDirectory(path);
                }

                string filename = "Annual Growth Report " + (Convert.ToInt32(year) - 1).ToString() + "-" + year.ToString() + ".xlsx";


                if (File.Exists(path + filename))
                {
                    File.Delete(path + filename);
                }

                //Excel.Application excelApp = new Excel.Application();
                string strPath = HttpContext.Current.Server.MapPath("ExportedFiles//" + filename);
                Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Range chartRange;
                Excel.Range range;
                Excel.Sheets xlSheets = null;
                Excel.Worksheet xlWorksheet = null;
                // Loop over DataTables in DataSet.
                DataTableCollection collection = dataSet.Tables;

                for (int i = collection.Count; i > 0; i--)
                {

                    //Create Excel Sheets
                    xlSheets = ExcelApp.Sheets;
                    xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                                   Type.Missing, Type.Missing, Type.Missing);

                    System.Data.DataTable table = collection[i - 1];

                    if (View == "Quantity")
                    {
                        xlWorksheet.Name = "AnnualGrowthQtyWise";
                    }
                    else
                    {
                        xlWorksheet.Name = "AnnualGrowthAmoWise";
                    }


                    for (int j = 1; j < table.Columns.Count + 1; j++)
                    {
                        ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;

                        range = xlWorksheet.Cells[1, j] as Excel.Range;
                        range.Cells.Font.Name = "Calibri";
                        range.Cells.Font.Bold = true;
                        range.Cells.Font.Size = 11;
                        range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                    }

                    // Storing Each row and column value to excel sheet
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        for (int l = 0; l < table.Columns.Count; l++)
                        {
                            ExcelApp.Cells[k + 2, l + 1] =
                            table.Rows[k].ItemArray[l].ToString();
                        }
                    }
                    ExcelApp.Columns.AutoFit();
                    xlWorksheet.Activate();
                    xlWorksheet.Application.ActiveWindow.SplitRow = 1;
                    xlWorksheet.Application.ActiveWindow.FreezePanes = true;

                    Excel.Range last = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                    chartRange = xlWorksheet.get_Range("A1", last);
                    foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                    {
                        cell.BorderAround2();
                    }
                    Excel.FormatConditions fcs = chartRange.FormatConditions;
                    Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
    (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                    //Excel.FormatCondition format = xlWorksheet.Rows.FormatConditions.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                    format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                }
                xlWorkbook.SaveAs(strPath);
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/xlsx";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.Write(strPath);
                //context.Response.TransmitFile(strPath);
                context.Response.End();
                //xlWorkbook.Close();
                //ExcelApp.Quit();
                //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                //ExcelApp.Visible = true;
            }
            catch (Exception ex)
            {
                //MessagBox.Show(ex.Message);
            }

            return JsonConvert.SerializeObject(dtItem);
        }

        private void BindDDLMonth()
        {
            try
            {
                for (int month = 4; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));

                }
                for (int month = 1; month <= 3; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));

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

                    //ListBox1.DataSource = dv1.ToTable();
                    //ListBox1.DataTextField = "SMName";
                    //ListBox1.DataValueField = "SMId";
                    //ListBox1.DataBind();
                    dtcheckrole.Dispose();
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
                        //ListBox1.DataSource = dv.ToTable();
                        //ListBox1.DataTextField = "SMName";
                        //ListBox1.DataValueField = "SMId";
                        //ListBox1.DataBind();
                    }
                    dt.Dispose();
                    dv.Dispose();
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindStaffChkList()
        {
            try
            {


                ListBox lstyr = new ListBox();
                for (int i = System.DateTime.Now.Year; i >= (System.DateTime.Now.Year - 4); i--)
                {
                    DataRow row = YrTable.NewRow();
                    //int CurrentYear = i;
                    // int NextYear = i + 1;
                    // string FinYear = null;
                    // FinYear = CurrentYear + "-" + NextYear;
                    lstyr.Items.Add(new ListItem(i.ToString()));
                    row["YrId"] = i.ToString();
                    row["YrName"] = (i - 1).ToString() + " - " + i.ToString();
                    YrTable.Rows.Add(row);
                }
                LstYear.DataSource = YrTable;
                LstYear.DataTextField = "YrName";
                LstYear.DataValueField = "YrId";
                LstYear.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            spinner.Visible = true;
            string Query = "", smIDStr1 = "", Qry = "", ProdGr = "", year = "", areaid = "", beatid = "", RetailerId = "", PrdGrp = "";
            string beat_id = "";
            //smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
           
            string view = approveStatusRadioButtonList.SelectedValue;
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 += node.Value + ",";
            }
            smIDStr1 = smIDStr1.TrimEnd(',');

            // Items collection
            foreach (ListItem item in LstYear.Items)
            {
                if (item.Selected)
                {
                    year += item.Value + ",";
                }
            }
            year = year.TrimEnd(',');
            int daysinFeb = DateTime.DaysInMonth(Convert.ToInt32(year), 2);
            // Items collection
            foreach (ListItem item in lstAreaBox.Items)
            {
                if (item.Selected)
                {
                    areaid += item.Value + ",";
                }
            }
            areaid = areaid.TrimEnd(',');

            // Items collection
            foreach (ListItem item in Lstpartybox.Items)
            {
                if (item.Selected)
                {
                    RetailerId += item.Value + ",";
                }
            }
            RetailerId = RetailerId.TrimEnd(',');

            // Items collection
            foreach (ListItem item in Lstbeatbox.Items)
            {
                if (item.Selected)
                {
                    beatid += item.Value + ",";
                }
            }
            beatid = beatid.TrimEnd(',');

            // Items collection
            foreach (ListItem item in LstPrdGrp.Items)
            {
                if (item.Selected)
                {
                    PrdGrp += item.Value + ",";
                }
            }
            PrdGrp = PrdGrp.TrimEnd(',');

            if (beatid != "" && beatid != "0")
                Qry = Qry + " and b.AreaId in (" + beatid + ")";

            if (RetailerId != "" && RetailerId != "0")
                Qry = Qry + " and p.PartyId in ('" + RetailerId + "')";
            if (PrdGrp != "" && PrdGrp != "0")
                Qry = Qry + " and itm.ItemId in ('" + PrdGrp + "')";


            //Label1.Text = Qry.ToString();
            if (view == "Quantity")
            {
                Query = @"select a.StateName,a.City,a.Area,a.Beat,a.Party As Outlet,a.Mobile,a.ItemGroup,a.Item,a.Year,sum(a.Jan_Qty) as Jan,sum(a.Feb_Qty)as Feb,sum(a.Mar_Qty)as Mar,sum(a.Apr_Qty)as Apr,sum(a.May_Qty)as May,sum(a.Jun_Qty)as Jun,sum(a.Jul_Qty)as Jul,sum(a.Aug_Qty) as Aug,sum(a.Sep_Qty)as Sep,sum(a.Oct_Qty) as Oct,sum(a.Nov_Qty) as Nov,sum(a.Dec_Qty)as Dec,(SUM(a.Jan_Qty) + SUM(a.Feb_Qty)+SUM(a.Mar_Qty)+SUM(a.Apr_Qty)+SUM(a.May_Qty)+SUM(a.Jun_Qty)+SUM(a.Jul_Qty)+SUM(a.Aug_Qty)+SUM(a.Sep_Qty)+SUM(a.Oct_Qty)+SUM(a.Nov_Qty)+SUM(a.Dec_Qty)) as Total from (  select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,os.Qty as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Jan/" + year + " 00:00' and '31/Jan/" + year + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,os.Qty as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Feb/" + year + " 00:00' and '" + daysinFeb + "/Feb/" + year + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,os.Qty as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Mar/" + year + " 00:00' and '31/Mar/" + year + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,os.Qty as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Apr/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Apr/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,os.Qty as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/May/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/May/" + (Convert.ToInt32(year) - 1) + " 23:59' " + beat_id + " " +

               "union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "'AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,os.Qty  as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jun/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Jun/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,os.Qty as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jul/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Jul/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,os.Qty as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Aug/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Aug/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,os.Qty as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Sep/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Sep/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,os.Qty as Oct_Qty,0 as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Oct/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Oct/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,os.Qty as Nov_Qty,0 as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Nov/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Nov/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

               " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Feb_Qty,0 as Mar_Qty,0 as Apr_Qty,0 as May_Qty,0 as Jun_Qty,0 as Jul_Qty,0 as Aug_Qty,0 as Sep_Qty,0 as Oct_Qty,0 as Nov_Qty,os.Qty as Dec_Qty from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Dec/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Dec/" + (Convert.ToInt32(year) - 1) + " 23:59' " + beat_id + ") a group by a.Party,a.Beat,a.Item,a.Year,a.ItemGroup,a.Area,a.Mobile,a.City,a.StateName Order by a.party";
            }
            else
            {
                Query = @"select a.StateName,a.City,a.Area,a.Beat,a.Party As Outlet,a.Mobile,a.ItemGroup,a.Item,a.Year,convert(varchar,convert(numeric (18,2),sum(a.Jan_Value))) as Jan,convert(varchar,convert(numeric (18,2),sum(a.Feb_Value))) as Feb,convert(varchar,convert(numeric (18,2),sum(a.Mar_value))) as Mar,convert(varchar,convert(numeric (18,2),sum(a.Apr_Value))) as Apr,convert(varchar,convert(numeric (18,2),sum(a.May_Value))) as May,convert(varchar,convert(numeric (18,2),sum(a.Jun_Value))) as Jun,convert(varchar,convert(numeric (18,2),sum(a.Jul_Value))) as Jul,convert(varchar,convert(numeric (18,2),sum(a.Aug_Value))) as Aug,convert(varchar,convert(numeric (18,2),sum(a.Sep_Value))) as Sep,convert(varchar,convert(numeric (18,2),sum(a.Oct_Value))) as Oct,convert(varchar,convert(numeric (18,2),sum(a.Nov_Value))) as Nov,convert(varchar,convert(numeric (18,2),sum(a.Dec_Value))) as Dec,(SUM(a.Jan_Value) + SUM(a.Feb_Value)+SUM(a.Mar_value)+SUM(a.Apr_Value)+SUM(a.May_Value)+SUM(a.Jun_Value)+SUM(a.Jul_Value)+SUM(a.Aug_Value)+SUM(a.Sep_Value)+SUM(a.Oct_Value)+SUM(a.Nov_Value)+SUM(a.Dec_Value) ) as Total from ( select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup ,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,os.Qty as Jan_Qty,(os.Qty*os.Rate) as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jan/" + year + " 00:00' and '31/Jan/" + year + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,os.Qty as Feb_Qty,(os.Qty*os.Rate) as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Feb/" + year + " 00:00' and '" + daysinFeb + "/Feb/" + year + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,os.Qty as Mar_Qty,(os.Qty*os.Rate) as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Mar/" + year + " 00:00' and '31/Mar/" + year + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,os.Qty as Apr_Qty,(os.Qty*os.Rate) as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Apr/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Apr/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,os.Qty as May_Qty,(os.Qty*os.Rate) as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/May/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/May/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,os.Qty  as Jun_Qty,(os.Qty*os.Rate) as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Jun/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Jun/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,os.Qty as Jul_Qty,(os.Qty*os.Rate) as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Jul/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Jul/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,os.Qty as Aug_Qty,(os.Qty*os.Rate) as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Aug/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Aug/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, os.Qty as Sep_Qty,(os.Qty*os.Rate) as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Sep/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Sep/" + (Convert.ToInt32(year) - 1) + " 23:59' " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,os.Qty as Oct_Qty,(os.Qty*os.Rate) as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Oct/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Oct/" + (Convert.ToInt32(year) - 1) + " 23:59'  " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,os.Qty as Nov_Qty,(os.Qty*os.Rate) as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ")  " + Qry + " and os.vdate between '01/Nov/" + (Convert.ToInt32(year) - 1) + " 00:00' and '30/Nov/" + (Convert.ToInt32(year) - 1) + " 23:59'  " +

                " union all select os.partyid,(p.partyname+'-'+IsNull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.areaname+'-'+IsNull(b.syncid,'')) as Beat,(mb.areaname+'-'+IsNull(mb.syncid,'')) as Area,(ms.areaname+'-'+IsNull(ms.syncid,'')) as City,(msp.AreaName+'-'+IsNull(msp.syncid,'')) AS StateName,(i.Itemname+'-'+IsNull(i.syncid,'')) as Item,(itm.ItemName+'-'+IsNull(itm.syncid,'')) as ItemGroup,'" + (Convert.ToInt32(year) - 1) + "-" + year + "' AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value ,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,os.Qty as Dec_Qty,(os.Qty*os.Rate) as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join MastItem itm on itm.Itemid=i.Underid left join mastarea b on b.areaid=p.beatid left join mastarea mb on mb.areaid=p.AreaId left join MastArea ms on ms.AreaId=mb.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea msp on msp.AreaId=mst.UnderId where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid and ua.PrimCode in (" + smIDStr1 + ")) and p.areaid in (" + areaid + ") " + Qry + " and os.vdate between '01/Dec/" + (Convert.ToInt32(year) - 1) + " 00:00' and '31/Dec/" + (Convert.ToInt32(year) - 1) + " 23:59' ) a group by a.Party,a.Beat,a.Item,a.Year,a.ItemGroup,a.Area,a.Mobile,a.City,a.StateName Order by a.party";
            }

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


            // Create a DataSet and put both tables in it.
            DataSet dataSet = new DataSet("General");
            dataSet.Tables.Add(dtItem);

            try
            {
                Excel.Application ExcelApp = new Excel.Application();
                string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                {
                    Directory.CreateDirectory(path);
                }

                string filename = "Annual Growth Report " + (Convert.ToInt32(year) - 1).ToString() + "-" + year.ToString() + ".xlsx";


                if (File.Exists(path + filename))
                {
                    File.Delete(path + filename);
                }

                //Excel.Application excelApp = new Excel.Application();
                string strPath = HttpContext.Current.Server.MapPath("ExportedFiles//" + filename);
                Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Range chartRange;
                Excel.Range range;
                Excel.Sheets xlSheets = null;
                Excel.Worksheet xlWorksheet = null;
                // Loop over DataTables in DataSet.
                DataTableCollection collection = dataSet.Tables;

                for (int i = collection.Count; i > 0; i--)
                {

                    //Create Excel Sheets
                    xlSheets = ExcelApp.Sheets;
                    xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                                   Type.Missing, Type.Missing, Type.Missing);

                    System.Data.DataTable table = collection[i - 1];

                    if (view == "Quantity")
                    {
                        xlWorksheet.Name = "AnnualGrowthQtyWise";
                    }
                    else
                    {
                        xlWorksheet.Name = "AnnualGrowthAmoWise";
                    }


                    for (int j = 1; j < table.Columns.Count + 1; j++)
                    {
                        ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;

                        range = xlWorksheet.Cells[1, j] as Excel.Range;
                        range.Cells.Font.Name = "Calibri";
                        range.Cells.Font.Bold = true;
                        range.Cells.Font.Size = 11;
                        range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                    }

                    // Storing Each row and column value to excel sheet
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        for (int l = 0; l < table.Columns.Count; l++)
                        {
                            ExcelApp.Cells[k + 2, l + 1] =
                            table.Rows[k].ItemArray[l].ToString();
                        }
                    }
                    ExcelApp.Columns.AutoFit();
                    xlWorksheet.Activate();
                    xlWorksheet.Application.ActiveWindow.SplitRow = 1;
                    xlWorksheet.Application.ActiveWindow.FreezePanes = true;

                    Excel.Range last = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                    chartRange = xlWorksheet.get_Range("A1", last);
                    foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                    {
                        cell.BorderAround2();
                    }
                    Excel.FormatConditions fcs = chartRange.FormatConditions;
                    Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
    (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                    //Excel.FormatCondition format = xlWorksheet.Rows.FormatConditions.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                    format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                }
                xlWorkbook.SaveAs(strPath);
                xlWorkbook.Close();
                ExcelApp.Quit();
                Response.ContentType = "Application/xlsx";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.TransmitFile(strPath);
                Response.End();
                //HttpContext context = HttpContext.Current;
                //context.Response.ContentType = "Application/x-msexcel";
                ////context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                //context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + "");
                //context.Response.Write(strPath);
                ////context.Response.TransmitFile(strPath);
                //context.Response.End();
                //xlWorkbook.Close();
                //ExcelApp.Quit();
                //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                //ExcelApp.Visible = true;
            }
            catch (Exception ex)
            {
                //MessagBox.Show(ex.Message);
            }
            dtItem.Dispose();
            dataSet.Dispose();
            spinner.Visible = false;
            //return JsonConvert.SerializeObject(dtItem);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptAnnual_Growth_V2.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //string MonthText = "", str = "";
            //Response.Clear();
            //Response.ContentType = "text/csv";
            //Response.AddHeader("content-disposition", "attachment;filename=AnnualGrowthReport.csv");
            //string headertext = "Retailer Name".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + ", " + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Total".TrimStart('"').TrimEnd('"');

            //StringBuilder sb = new StringBuilder();
            //sb.Append(headertext);
            //sb.AppendLine(System.Environment.NewLine);
            //string Month = "", smIDStr = "", smIDStr1 = "", Query = "", areastr = "",
            //areastr1 = "", beatstr = "", beatstr1 = "", partystr = "", partystr1 = "";
            //DataTable dtParams = new DataTable();
            //DataTable dt = new DataTable();


            //foreach (TreeNode node in trview.CheckedNodes)
            //{
            //    smIDStr1 = node.Value;
            //    {
            //        smIDStr += node.Value + ",";
            //    }
            //}
            //smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            //foreach (ListItem item in lstAreaBox.Items)
            //{
            //    if (item.Selected)
            //    {
            //        areastr += item.Value + ",";
            //    }
            //}
            //areastr1 = areastr.TrimStart(',').TrimEnd(',');

            //foreach (ListItem li in Lstbeatbox.Items)
            //{
            //    if (li.Selected == true)
            //    {
            //        beatstr += li.Value + ",";
            //    }
            //}
            //beatstr1 = beatstr.TrimStart(',').TrimEnd(',');

            //foreach (ListItem item in Lstpartybox.Items)
            //{
            //    if (item.Selected == true)
            //    {
            //        partystr += item.Value + ",";
            //    }
            //}
            //partystr1 = partystr.TrimStart(',').TrimEnd(',');

            //if (smIDStr1 != "")
            //{
            //    string year = "";
            //    //foreach (ListItem item in CheckBoxList1.Items)
            //    //{
            //    //    if (item.Selected)
            //    //    {
            //    //        year += item.Text + ",";
            //    //    }
            //    //}
            //    year = year.TrimStart(',').TrimEnd(',');
            //    if (year != "")
            //    {

            //        string Qry = "";
            //        string beat_id = "";
            //        int daysinFeb = DateTime.DaysInMonth(Convert.ToInt32(year), 2);

            //        if (approveStatusRadioButtonList.SelectedValue == "Quantity")
            //        {
            //            Query = @"select a.party,a.Beat,a.Item,a.Year,sum(a.Jan_Qty) as Jan,sum(a.Feb_Qty)as Feb,sum(a.Mar_Qty)as Mar,sum(a.Apr_Qty)as Apr,sum(a.May_Qty)as May,sum(a.Jun_Qty)as Jun,sum(a.Jul_Qty)as Jul,sum(a.Aug_Qty) as Aug,sum(a.Sep_Qty)as Sep,sum(a.Oct_Qty) as Oct,sum(a.Nov_Qty) as Nov,sum(a.Dec_Qty)as Dec,(SUM(a.Jan_Qty) + SUM(a.Feb_Qty)+SUM(a.Mar_Qty)+SUM(a.Apr_Qty)+SUM(a.May_Qty)+SUM(a.Jun_Qty)+SUM(a.Jul_Qty)+SUM(a.Aug_Qty)+SUM(a.Sep_Qty)+SUM(a.Oct_Qty)+SUM(a.Nov_Qty)+SUM(a.Dec_Qty)) as d from (" +
            //            " select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Item,Datepart(yyyy,os.VDate) AS Year,os.Qty as Jan_Qty,(os.Qty*os.Rate) as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Jan/" + year + " 00:00' and '31/Jan/" + year + " 23:59' " + beat_id + "  " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,os.Qty as Feb_Qty,(os.Qty*os.Rate) as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Feb/" + year + " 00:00' and '" + daysinFeb + "/Feb/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,os.Qty as Mar_Qty,(os.Qty*os.Rate) as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Mar/" + year + " 00:00' and '31/Mar/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,os.Qty as Apr_Qty,(os.Qty*os.Rate) as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Apr/" + year + " 00:00' and '30/Apr/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,os.Qty as May_Qty,(os.Qty*os.Rate) as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/May/" + year + " 00:00' and '31/May/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,os.Qty  as Jun_Qty,(os.Qty*os.Rate) as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Jun/" + year + " 00:00' and '30/Jun/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,os.Qty as Jul_Qty,(os.Qty*os.Rate) as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Jul/" + year + " 00:00' and '31/Jul/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,os.Qty as Aug_Qty,(os.Qty*os.Rate) as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Aug/" + year + " 00:00' and '31/Aug/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, os.Qty as Sep_Qty,(os.Qty*os.Rate) as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Sep/" + year + " 00:00' and '30/Sep/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,os.Qty as Oct_Qty,(os.Qty*os.Rate) as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Oct/" + year + " 00:00' and '31/Oct/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,os.Qty as Nov_Qty,(os.Qty*os.Rate) as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Nov/" + year + " 00:00' and '30/Nov/" + year + " 23:59' " + beat_id + " " +
            //            " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value ,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,os.Qty as Dec_Qty,(os.Qty*os.Rate) as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Dec/" + year + " 00:00' and '31/Dec/" + year + " 23:59' " + beat_id + ") a group by a.Party,a.Beat,a.Item,a.Year";

            //        }
            //        else
            //        {

            //            Query = @"select a.party,a.Beat,a.Item,a.Year,convert(varchar,convert(numeric (18,2),sum(a.Jan_Value))) as Jan,convert(varchar,convert(numeric (18,2),sum(a.Feb_Value))) as Feb,convert(varchar,convert(numeric (18,2),sum(a.Mar_value))) as Mar,convert(varchar,convert(numeric (18,2),sum(a.Apr_Value))) as Apr,convert(varchar,convert(numeric (18,2),sum(a.May_Value))) as May ,convert(varchar,convert(numeric (18,2),sum(a.Jun_Value))) as Jun,convert(varchar,convert(numeric (18,2),sum(a.Jul_Value))) as Jul,convert(varchar,convert(numeric (18,2),sum(a.Aug_Value))) as Aug,convert(varchar,convert(numeric (18,2),sum(a.Sep_Value))) as Sep,convert(varchar,convert(numeric (18,2),sum(a.Oct_Value))) as Oct,convert(varchar,convert(numeric (18,2),sum(a.Nov_Value))) as Nov,convert(varchar,convert(numeric (18,2),sum(a.Dec_Value))) as Dec,(SUM(a.Jan_Value) + SUM(a.Feb_Value)+SUM(a.Mar_value)+SUM(a.Apr_Value)+SUM(a.May_Value)+SUM(a.Jun_Value)+SUM(a.Jul_Value)+SUM(a.Aug_Value)+SUM(a.Sep_Value)+SUM(a.Oct_Value)+SUM(a.Nov_Value)+SUM(a.Dec_Value) ) as d from (" +
            //           " select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Item,Datepart(yyyy,os.VDate) AS Year,os.Qty as Jan_Qty,(os.Qty*os.Rate) as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Jan/" + year + " 00:00' and '31/Jan/" + year + " 23:59' " + beat_id + "  " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,os.Qty as Feb_Qty,(os.Qty*os.Rate) as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value, 0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Feb/" + year + " 00:00' and '" + daysinFeb + "/Feb/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,os.Qty as Mar_Qty,(os.Qty*os.Rate) as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Mar/" + year + " 00:00' and '31/Mar/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,os.Qty as Apr_Qty,(os.Qty*os.Rate) as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Apr/" + year + " 00:00' and '30/Apr/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,os.Qty as May_Qty,(os.Qty*os.Rate) as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/May/" + year + " 00:00' and '31/May/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,os.Qty  as Jun_Qty,(os.Qty*os.Rate) as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Jun/" + year + " 00:00' and '30/Jun/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,os.Qty as Jul_Qty,(os.Qty*os.Rate) as Jul_Value,0 as Aug_Qty,0 as Aug_Value,0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Jul/" + year + " 00:00' and '31/Jul/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,os.Qty as Aug_Qty,(os.Qty*os.Rate) as Aug_Value,0 as Sep_Qty,0 as Sep_Value, 0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Aug/" + year + " 00:00' and '31/Aug/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, os.Qty as Sep_Qty,(os.Qty*os.Rate) as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Sep/" + year + " 00:00' and '30/Sep/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,os.Qty as Oct_Qty,(os.Qty*os.Rate) as Oct_Value,0 as Nov_Qty,0 as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Oct/" + year + " 00:00' and '31/Oct/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,os.Qty as Nov_Qty,(os.Qty*os.Rate) as Nov_Value,0 as Dec_Qty,0 as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Nov/" + year + " 00:00' and '30/Nov/" + year + " 23:59' " + beat_id + " " +
            //           " union all select os.partyid,p.partyname as Party,b.areaname as Beat,i.Itemname as Mon1_Item,Datepart(yyyy,os.VDate) AS Year,0 as Jan_Qty,0 as Jan_Value,0 as Feb_Qty,0 as Feb_Value,0 as Mar_Qty,0 as Mar_value,0 as Apr_Qty,0 as Apr_Value,0 as May_Qty,0 as May_Value ,0 as Jun_Qty,0 as Jun_Value,0 as Jul_Qty,0 as Jul_Value,0 as Aug_Qty,0 as Aug_Value, 0 as Sep_Qty,0 as Sep_Value,0 as Oct_Qty,0 as Oct_Value,0 as Nov_Qty,0 as Nov_Value,os.Qty as Dec_Qty,(os.Qty*os.Rate) as Dec_Value from Transorder1 os left join MastParty p on p.partyid =os.partyid left join MastItem i on i.Itemid=os.itemid left join mastarea b on b.areaid=p.beatid where os.partyid in (Select distinct p.partyid as Party from mastparty p left join mastlink ua on ua.linkcode=p.areaid " + Qry + ") and os.vdate between '01/Dec/" + year + " 00:00' and '31/Dec/" + year + " 23:59' " + beat_id + ") a group by a.Party,a.Beat,a.Item,a.Year";
            //        }
            //    }

            //    dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

            //    for (int j = 0; j < dtParams.Rows.Count; j++)
            //    {
            //        for (int k = 0; k < dtParams.Columns.Count; k++)
            //        {
            //            if (dtParams.Rows[j][k].ToString().Contains(","))
            //            {
            //                string h = dtParams.Rows[j][k].ToString();
            //                string d = h.Replace(",", " ");
            //                dtParams.Rows[j][k] = "";
            //                dtParams.Rows[j][k] = d;

            //                dtParams.AcceptChanges();
            //                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
            //            }
            //            else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
            //            {
            //                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
            //            }
            //            else
            //            {
            //                sb.Append(dtParams.Rows[j][k].ToString() + ',');
            //            }
            //        }
            //        sb.Append(Environment.NewLine);
            //    }
            //    Response.Clear();
            //    Response.ContentType = "text/csv";
            //    Response.AddHeader("content-disposition", "attachment;filename=AnnualGrowthReport.csv");
            //    Response.Write(sb.ToString());
            //    Response.End();

            //}
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
                Session["treenodes"] = smIDStr12;
                string smiMStr = smIDStr12;
                if (smIDStr12 != "")
                {
                    BindArea(smIDStr12);
                }
                else
                {
                    lstAreaBox.Items.Clear();
                    lstAreaBox.DataBind();
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

        private void BindArea(string SMIDStr)
        {
            try
            {

                string areaqry = @"select AreaId,AreaName from mastarea where AreaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Area' and Active=1 order by AreaName";
                DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaqry);
                if (dtArea.Rows.Count > 0)
                {
                    lstAreaBox.DataSource = dtArea;
                    lstAreaBox.DataTextField = "AreaName";
                    lstAreaBox.DataValueField = "AreaId";
                    lstAreaBox.DataBind();
                }
                else
                {

                }

                dtArea.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindItmGrp()
        {
            try
            {
                string itmqry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' order by ItemName";
                DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, itmqry);
                if (dtArea.Rows.Count > 0)
                {
                    LstPrdGrp.DataSource = dtArea;
                    LstPrdGrp.DataTextField = "ItemName";
                    LstPrdGrp.DataValueField = "ItemId";
                    LstPrdGrp.DataBind();
                }
                else
                {

                }

                dtArea.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void lstAreaBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string areastr = "";
                //         string message = "";
                foreach (ListItem areagrp in lstAreaBox.Items)
                {
                    if (areagrp.Selected)
                    {
                        areastr += areagrp.Value + ",";
                    }
                }
                areastr = areastr.TrimStart(',').TrimEnd(',');
                if (areastr != "")
                {
                    string beatqry = @"select AreaId,AreaName from mastarea where Underid in ( " + areastr + " ) and areatype='Beat' and Active=1 order by AreaName";
                    DataTable dtbeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatqry);
                    if (dtbeat.Rows.Count > 0)
                    {
                        Lstbeatbox.DataSource = dtbeat;
                        Lstbeatbox.DataTextField = "AreaName";
                        Lstbeatbox.DataValueField = "AreaId";
                        Lstbeatbox.DataBind();
                    }

                    dtbeat.Dispose();
                }
                else
                {
                    Lstbeatbox.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
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
                string str = @"select PartyId,(partyName+'-'+Mobile) AS partyName from Mastparty where Beatid in (" + strparty + ") and partydist=0 and active=1";
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
                Lstpartybox.Items.Clear();
            }

        }
    }
}