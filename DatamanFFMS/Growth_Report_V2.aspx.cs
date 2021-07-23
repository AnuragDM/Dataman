﻿using BusinessLayer;
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
    public partial class Growth_Report_V2 : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        DataTable gvdt = new DataTable("Party Month Wise Achievement");
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// 
                //List<Products> Products = new List<Products>();
                //Products.Add(new Products());
                //rptRPWiseGrowth.DataSource = Products;
                //rptRPWiseGrowth.DataBind();
                BindMaterialGroup();
                BindMaterialGroup();
                roleType = Settings.Instance.RoleType;
                //BindDDLMonth();
                //monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                //btnExport.Visible = true;
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";
                BindTreeViewControl();

            }
        }

        //private void BindDDLMonth()
        //{
        //    try
        //    {
        //        for (int month = 1; month <= 12; month++)
        //        {
        //            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
        //            monthDDL.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

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
            //dtItem.Dispose();
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
            spinner.Visible = true;
            DataSet dataSet = new DataSet("Retailer Growth Report");
            string smIDStr = "", smIDStr1 = "", Query = "", Qrychk = "", Qrychk1 = "", Q1 = "", matcitystrnew = "", matareastrnew = "",
            matbeatstrnew = "", matpartystrnew = "", matGrpStrNew = "", matProStrNew = "", matBeatNew = "";

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

            //if (monthDDL.SelectedValue == "0")
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please Select Month');", true);
            //    return;
            //}

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

            try
            {
                //"01/" + (monthDDL.SelectedValue) + "/" + DateTime.Now.Year.ToString();

                //string Curr = 
                string das = Convert.ToDateTime(txtfmDate.Text).ToString("dd/MMM/yyyy");
                DateTime mm = Convert.ToDateTime(txttodate.Text);
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
                Query = "select * from (select a.StateName,a.City,a.Area,a.Beat,a.Party AS Outlet,a.Mobile,a.Item,sum(a.Avg_Value) as Avg_Value,sum(a.Value) Value,case when (SUM(a.Avg_Value)=0 and SUM(a.Value)>0) then 100 when sum(a.Value-a.Avg_Value)> 0 then sum((a.Value-a.Avg_Value)*100)/SUM(a.Avg_Value) when SUM(a.Value)=0 then 0 when sum(a.Value-a.Avg_Value)< 0 then 0 end as Percentage from (select (p.PartyName+'-'+isnull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.AreaName+'-'+isnull(b.syncid,'')) AS Beat,(ab.AreaName+'-'+isnull(ab.syncid,'')) AS Area,(ac.AreaName+'-'+isnull(ac.syncid,'')) AS City,(stt.AreaName+'-'+isnull(stt.syncid,'')) AS StateName,(i.itemname+'-'+isnull(i.syncid,'')) as Item,((os.Qty*os.Rate)/3) as Avg_Value,0 as Value from TransOrder1 os Left JOIN mastparty p on p.PartyId =os.PartyId Left join MastArea b on b.AreaId=p.BeatId Left join MastArea ab on ab.AreaId=b.UnderId Left join MastArea ac on ac.AreaId=ab.UnderId left join MastArea dst on dst.AreaId=ac.UnderId left join MastArea stt on stt.AreaId=dst.UnderId Left join MastItem i on i.ItemId=os.ItemId " + Qrychk + "  "+

                  "  union all select (p.PartyName+'-'+isnull(p.syncid,'')) as Party,p.Mobile as Mobile,(b.AreaName+'-'+isnull(b.syncid,'')) AS Beat,(ab.AreaName+'-'+isnull(ab.syncid,'')) AS Area,(ac.AreaName+'-'+isnull(ac.syncid,'')) AS City,(stt.AreaName+'-'+isnull(stt.syncid,'')) AS StateName,(i.itemname+'-'+isnull(i.syncid,'')) as Item,0 as Avg_Value,((os.Qty*os.Rate)) as Value from TransOrder1 os Left join mastparty p on p.PartyId =os.PartyId Left join MastArea b on b.AreaId=p.BeatId Left join MastArea ab on ab.AreaId=b.UnderId Left join MastArea ac on ac.AreaId=ab.UnderId left join MastArea dst on dst.AreaId=ac.UnderId left join MastArea stt on stt.AreaId=dst.UnderId Left join MastItem i on i.ItemId=os.ItemId " + Q1 + " )a group by a.Party,a.Item,a.Beat,a.Area,a.City,a.StateName,a.Mobile ) a order by a.Outlet ";

                DataTable dt = new DataTable("Retailer Product Wise Growth Report");
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                DateTime curr_dt = Convert.ToDateTime(txttodate.Text);
                DateTime prev_dt = curr_dt.AddMonths(-1);
                DateTime last_dt = prev_dt.AddMonths(-2);


                int yr = Convert.ToInt32((DateTime.ParseExact(last_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
                int mnth = Convert.ToInt32((DateTime.ParseExact(last_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
                DateTime ldate = new DateTime(yr, mnth, 1);

                yr = Convert.ToInt32((DateTime.ParseExact(prev_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
                mnth = Convert.ToInt32((DateTime.ParseExact(prev_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
                DateTime pdate = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));

                string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtdesig.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                    }
                }
                gvdt.Columns.Add(new DataColumn("ReportPerson", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SalePerson", typeof(String)));
                gvdt.Columns.Add(new DataColumn("City", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Area", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Beat", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Outlet", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Mobile", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Pendency", typeof(String)));
                gvdt.Columns.Add(new DataColumn("PrevMonth", typeof(String)));
                gvdt.Columns.Add(new DataColumn("CurrentMnth", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Percentage", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Conper", typeof(String)));

                //for (int i = 0; i < dtdesig.Rows.Count; i++)
                //{
                //    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                //    {
                //        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                //    }
                //}

                Qrychk1 = " WHERE os.VDate <='" + Settings.dateformat(txttodate.Text) + "' ";
                Qrychk1 = Qrychk1 + " and cp.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                string Qty2 = " WHERE os.VDate BETWEEN '" + ldate.ToString("dd/MMM/yyyy") + "' AND '" + pdate.ToString("dd/MMM/yyyy") + "' ";
                Qty2 = Qty2 + " and cp.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                string Qty3 = " WHERE os.VDate BETWEEN '" + Settings.dateformat(txtfmDate.Text) + "' AND '" + Settings.dateformat(txttodate.Text) + "' ";
                Qty3 = Qty3 + " and cp.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                //   Qrychk = " os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                string query = @"SELECT SMID,ReportPerson,SalePerson,(a.areaname+'-'+max(IsNull(a.SyncId,''))) AS Area,q.Beat,q.City,PartyName,q.Mobile, sum(Pendency) [Pendency], sum(PrevMonth) AS PrevMonth,sum(CurrentMnth) AS CurrentMnth, ISNULL( SUM((CurrentMnth*100)/nullif(prevmonth,0)),100) as Percentage,q.Conper FROM ( select p.PartyId,p.PartyName+'-'+max(Isnull(p.SyncId,'')) As PartyName ,p.AreaId,SUM(os.Qty * os.Rate) as Pendency,max(cp.SMId) as SMID,max(cp.SMName)+'-'+max(Isnull(cp.SyncId,''))  as SalePerson,max(crp.SMName)+'-'+max(Isnull(crp.SyncId,'')) as ReportPerson,0 AS PrevMonth,0 AS CurrentMnth,0 Percentage,p.Mobile, p.ContactPerson as Conper,b.AreaName+'-'+max(Isnull(b.SyncId,'')) as Beat,ab.AreaName+'-'+max(Isnull(ab.SyncId,'')) as City from TransOrder1 os inner join MastParty p on P.PartyId=os.PartyId left JOIN mastarea b on b.AreaId=p.BeatId left JOIN mastarea ab on ab.AreaId=p.CityId left join MastSalesRep cp on cp.SMId=os.SMId left join MastSalesRep crp on cp.UnderId=crp.SMId " + Qrychk1 + " " + beat_Filter + " Group by p.PartyId,p.PartyName,p.AreaId,p.Mobile,p.ContactPerson,b.AreaName,ab.AreaName union all select p.PartyId,p.PartyName+'-'+max(Isnull(p.SyncId,'')) As PartyName, p.AreaId,0 Pendency,max(cp.SMId) as SMID,max(cp.SMName)+'-'+max(Isnull(cp.SyncId,''))  as SalePerson,max(crp.SMName)+'-'+max(Isnull(crp.SyncId,''))  as ReportPerson,sum(((isnull(os.Qty,0))*isnull(os.Rate,0))/3) as PrevMonth,0 AS CurrentMnth,  0 Percentage,p.Mobile,p.ContactPerson as Conper,b.AreaName+'-'+max(Isnull(b.SyncId,'')) as Beat ,ab.AreaName+'-'+max(Isnull(ab.SyncId,'')) as City from TransOrder1 os inner join MastParty p on P.PartyId=os.PartyId left join MastSalesRep  cp on cp.SMId=os.SMId left join MastSalesRep crp on cp.UnderId=crp.SMId left JOIN mastarea b on b.AreaId=p.BeatId left JOIN mastarea ab on ab.AreaId=p.CityId " + Qty2 + " " + beat_Filter + " Group by p.PartyId,p.PartyName,p.AreaId,p.Mobile,p.ContactPerson,b.AreaName,ab.AreaName union all  select p.PartyId,p.PartyName+'-'+max(Isnull(p.SyncId,'')) As PartyName, p.AreaId,0 AS Pendency,max(cp.SMId) as SMID,max(cp.SMName)+'-'+max(Isnull(cp.SyncId,'')) as SalePerson,max(crp.SMName)+'-'+max(Isnull(crp.SyncId,'')) as ReportPerson,0 AS PrevMonth,sum((isnull(os.Qty,0))*isnull(os.Rate,0)) as CurrentMnth, 0 Percentage ,p.Mobile,p.ContactPerson as Conper,b.AreaName+'-'+max(Isnull(b.SyncId,'')) as Beat ,ab.AreaName+'-'+max(Isnull(ab.SyncId,'')) as City from TransOrder1 os inner join MastParty p on p.PartyId=os.PartyId left join MastSalesRep  cp on cp.SMId =os.SMId left join MastSalesRep crp on cp.UnderId=crp.SMId left JOIN mastarea b on b.AreaId=p.BeatId left JOIN mastarea ab on ab.AreaId=p.CityId " + Qty3 + " " + beat_Filter + " Group by p.PartyId,p.PartyName,p.AreaId,p.Mobile,p.ContactPerson,b.AreaName,ab.AreaName  ) q LEFT JOIN MastArea a ON q.areaid = a.AreaId GROUP BY PartyName, a.AreaName,SMID, SalePerson,ReportPerson,q.Mobile,q.Conper,q.Beat,q.City ,A.areaid order by PartyName";

                DataTable dt1 = new DataTable();
                dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                DataView dvSales = new DataView(dt1);

                foreach (DataRow drvst in dt1.Rows)
                {

                    dvSales.RowFilter = "SMID=" + drvst["SMID"].ToString();
                    if (dvSales.ToTable().Rows.Count > 0)
                    {
                        DataTable dtsp = dvSales.ToTable();
                        DataRow dr = dtsp.Rows[0];
                        DataRow mDataRow = gvdt.NewRow();

                        str = "select msp.SMName+'-'+Isnull(msp.SyncId,'') As SMName,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMID"].ToString() + "  and maingrp<>" + dr["SMID"].ToString() + ")";
                        DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtsr.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtsr.Rows.Count; j++)
                            {
                                if (dtsr.Rows[j]["DesName"].ToString() != "")
                                {
                                    mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                                }
                            }
                        }

                        for (int i = 0; i < dtdesig.Rows.Count; i++)
                        {
                            if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                            {
                                mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                            }
                        }

                        mDataRow["ReportPerson"] = dr["ReportPerson"].ToString();
                        mDataRow["SalePerson"] = dr["SalePerson"].ToString();
                        mDataRow["Area"] = dr["Area"].ToString();
                        mDataRow["Beat"] = dr["Beat"].ToString();
                        mDataRow["City"] = dr["City"].ToString();
                        mDataRow["Outlet"] = dr["PartyName"].ToString();
                        mDataRow["Mobile"] = dr["Mobile"].ToString();
                        mDataRow["Pendency"] = dr["Pendency"].ToString();
                        mDataRow["PrevMonth"] = dr["PrevMonth"].ToString();
                        mDataRow["CurrentMnth"] = dr["CurrentMnth"].ToString();
                        mDataRow["Percentage"] = dr["Percentage"].ToString();
                        mDataRow["Conper"] = dr["Conper"].ToString();

                        dvSales.RowFilter = null;
                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();

                        dtsr.Dispose();
                        dtsp.Dispose();
                    }
                }

                dataSet.Tables.Add(dt);
                dataSet.Tables.Add(gvdt);

                try
                {
                    Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "Retailer Growth Report.xlsx";

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

                        if (i - 1 == 1)
                        {
                            xlWorksheet.Name = "Party Monthly Achievement";
                        }
                        else if (i - 1 == 0)
                        {
                            xlWorksheet.Name = "Product Growth Report";
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
                        format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                    }
                    xlWorkbook.SaveAs(strPath);
                    xlWorkbook.Close();
                    ExcelApp.Quit();
                    Response.ContentType = "Application/xlsx";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.TransmitFile(strPath);
                    Response.End();
                    //xlWorkbook.Close();
                    //ExcelApp.Quit();
                    //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                    //ExcelApp.Visible = true;
                }
                catch (Exception ex)
                {

                }

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record File Generated Successfully');", true);

                dataSet.Dispose();
                dt.Dispose();
                dt1.Dispose();
                dtdesig.Dispose();
                dvSales.Dispose();
                gvdt.Dispose();
                //if (dtItem.Rows.Count > 0)
                //{
                //    rptmain.Style.Add("display", "block");
                //    rptRPWiseGrowth.DataSource = dtItem;
                //    rptRPWiseGrowth.DataBind();
                //    btnExport.Visible = true;
                //}
                //else
                //{
                //    rptmain.Style.Add("display", "block");
                //    rptRPWiseGrowth.DataSource = dtItem;
                //    rptRPWiseGrowth.DataBind();
                //    btnExport.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            
            spinner.Visible = false;
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Growth_Report_V2.aspx");
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
                string str = @"select PartyId,(partyName+'-'+Mobile) AS partyName from Mastparty where Beatid in (" + strparty + ") order by partyname";
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