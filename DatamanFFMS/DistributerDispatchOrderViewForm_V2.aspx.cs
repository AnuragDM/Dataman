using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using Newtonsoft.Json;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
namespace AstralFFMS
{
    public partial class DistributerDispatchOrderViewForm_V2 : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;

        string doc_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            //trview.Attributes.Add("onclick", "postBackByObject()");
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            //myModal.Style.Add("display", "none");
            // myModal1.Style.Add("display", "none");

            if (!IsPostBack)
            {

                roleType = Settings.Instance.RoleType;

                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // 
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// 

                BindTreeViewControl();

                BindType();
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));

                dvusd.Visible = false;
                dvsd.Visible = false;
                mlblusd.Visible = false;
                mlbltypsd.Visible = false;

            }
        }

        private void BindType()
        {
            string str = "select Text,Value from MastDistributorType order by sort";
            fillDDLDirect(ddltype, str, "Value", "Text", 1);
        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }

        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    //string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    string query = "select msr.smid,msr.Smname as smname,msr.underid ,msr.lvl from mastsalesrep msr LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
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


        public class Distributors
        {
            public string VDate { get; set; }
            public string Distributor { get; set; }
            public string MaterialGROUP { get; set; }
            public string Item { get; set; }
            public string Qty { get; set; }
            public string rate { get; set; }
            public string Amount { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetDistributorItemSale(string Distid, string ProductGroup, string Product, string Fromdate, string Todate)
        {
            string qry = "";
            string Qrychk = " t1.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and t1.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            //            string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
            //                           on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where (" + Qrychk + ") group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";
            if (ProductGroup != "" && Product == "")
            {
                qry = "and mi1.itemid in(" + ProductGroup + ")";

            }
            if (Product != "" && Product != "")
            {
                qry = qry + "and mi.itemid in (" + Product + ")";

            }

            //            string query = @"SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
            //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
            //             t1.DistId in (" + Distid + ") and " + Qrychk + "  " + qry + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName order BY  t1.VDate desc";
            string query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
             t1.DistId in (" + Distid + ") and " + Qrychk + "  " + qry + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId and t2.OrderType is null group by tbl.DocId order by VDate desc";
            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


        }



        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string citystr = "";
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');

                    string condition = "";
                    if (ddltype.SelectedIndex > 0)
                        if (ddltype.SelectedValue == "DEPOT" || ddltype.SelectedValue == "C&F" || ddltype.SelectedValue == "SUPERDIST")
                        {
                            condition = " and mp.DistType='" + ddltype.SelectedValue + "' ";
                        }
                        else
                        {
                            condition = " and mp.DistType='SUPERDIST' ";
                        }

                    //string distqry = @"select * from MastParty where CityId in (" + citystr + ")  and PartyDist=1 and Active=1 order by PartyName";
                    string distqry = @"select mp.PartyId,mp.PartyName + ' - ' + md.Text + ' ( ' + ma.AreaName + ' )' as PartyName from MastParty mp left join MastDistributorType md on md.Value=mp.DistType left join MastArea ma on ma.AreaId=mp.CityId where   mp.CityId in (" + citystr + ") " + condition + "  and mp.PartyDist=1 and mp.Active=1 order by mp.PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        LstSD.DataSource = dtDist;
                        LstSD.DataTextField = "PartyName";
                        LstSD.DataValueField = "PartyId";
                        LstSD.DataBind();
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        LstSD.DataSource = dt;
                        LstSD.DataBind();
                    }

                    dtCity.Dispose();
                    dtDist.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);

                    //distitemsalerpt.DataSource = null;
                    //distitemsalerpt.DataBind();
                    ddltype.SelectedIndex = 0;
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
                spinner.Visible = true;
                DataSet Ds = new DataSet();
                string smIDStr = "", smIDStr1 = "", SupdistIdStr1 = "", distIdStr1 = "", Qrychk = "", Qrychk1 = "";
                string orderdetail = "", Pendingorderdetail = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                foreach (ListItem item in LstSD.Items)
                {
                    if (item.Selected)
                    {
                        SupdistIdStr1 += item.Value + ",";
                    }
                }

                SupdistIdStr1 = SupdistIdStr1.TrimStart(',').TrimEnd(',');

                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        distIdStr1 += item.Value + ",";
                    }
                }
                distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');


                if (smIDStr1 != "")
                {
                    //if (distIdStr1 != "")
                    //{
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }

                    Qrychk = " P.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and P.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                    Qrychk1 = " t1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                    string condition = "";
                    if (ddltype.SelectedIndex > 0)
                        condition = " and MP.disttype='" + ddltype.SelectedValue + "' ";

                    string query = "";

                    if (ddltype.SelectedIndex > 0)
                    {
                        if (ddltype.SelectedIndex == 4 || ddltype.SelectedIndex == 5)
                        {
                            if (distIdStr1 != "" && SupdistIdStr1 != "")
                            {
                                query = "SELECT Max(T.VDate) VDate, Max(T.Superdistributor) Superdistributor,Max(T.DisSyncId) DisSyncId,Max(T.Distributor) Distributor,  T.DocId DocId, Sum(T.TotalQty) TotalQty, CAST( Sum(T.TotalAmount) AS NUMERIC(38,2)) TotalAmount , Max(T.DistId) DistId,Max(T.POrdId) POrdId,Max(T.partyid) partyid,Max(T.OrderPlaceBy) OrderPlaceBy,Max(T.OrderStatus) OrderStatus ,Max(T.Supersyncid) Supersyncid,Isnull((SELECT SMName  Name FROM MastSalesRep  WHERE UserId=Max(T.DispatchCancel_Smid) UNION ALL SELECT PartyName Name FROM MastParty WHERE UserId=Max(T.DispatchCancel_Smid)),'')  DispatchCancelby,Max(T.dispatchcanceldatetime) DispatchCancelDate   FROM(SELECT P.VDate VDate, MD.PartyName AS Superdistributor,MP.SyncId AS DisSyncId,Mp.partyname AS Distributor, P.PODocId DocId , tp1.Qty  AS TotalQty, rate*qty AS TotalAmount, P.DistId,P.POrdId,Mp.partyid,Mp.disttype,case when P.fromapp is null then ms.smname else  Mp.partyname end as OrderPlaceBy,(case P.OrderType when 'C' then 'Cancel' when 'D' then 'Dispatch' else 'Pending' end) as OrderStatus ,MD.SyncId Supersyncid,Isnull(DispatchCancel_Smid,0) DispatchCancel_Smid,P.dispatchcanceldatetime dispatchcanceldatetime FROM TransPurchOrder P LEFT JOIN	 TransPurchOrder1 tp1 ON tp1.PODocId=P.PODocId LEFT JOIN Mastparty MP ON MP.PartyId=P.DistId left join mastsalesrep ms on P.smid=ms.smid LEFT JOIN Mastparty MD ON MD.PartyId=MP.SD_Id WHERE  MP.disttype='" + ddltype.SelectedValue + "' and " + Qrychk + " and P.DistId in (" + distIdStr1 + ")  UNION ALL SELECT  P.VDate VDate, Mp.PartyName AS Superdistributor,'' AS DisSyncId,'' AS Distributor, P.PODocId DocId , tp1.Qty  AS TotalQty, rate*qty AS TotalAmount ,P.DistId,P.POrdId,0 AS partyid,Mp.disttype ,case when P.fromapp is null then ms.smname else  Mp.partyname end as OrderPlaceBy , (case P.OrderType when 'C'  then 'Cancel' when 'D' then 'Dispatch' else 'Pending' end) as OrderStatus ,MP.SyncId Supersyncid,Isnull(DispatchCancel_Smid,0) DispatchCancel_Smid,P.dispatchcanceldatetime dispatchcanceldatetime FROM TransPurchOrder P LEFT JOIN	 TransPurchOrder1 tp1 ON tp1.PODocId=P.PODocId left join mastsalesrep ms on P.smid=ms.smid LEFT JOIN Mastparty MP ON MP.PartyId=P.DistId WHERE  P.DistId in (" + SupdistIdStr1 + ") and " + Qrychk + " " + condition + "  )AS T  GROUP BY T.DocId ORDER BY Max(T.VDate) desc ";

                                orderdetail = @"SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where t1.DistId in (" + distIdStr1 + ") " + condition + "  and " + Qrychk1 + " and Isnull(t.OrderType,'') in ('D','C') UNION ALL SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where t1.DistId in (" + SupdistIdStr1 + ") " + condition + "  and " + Qrychk1 + " and Isnull(t.OrderType,'') in ('D','C') ";

                                Pendingorderdetail = @"SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(18,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,isnull(t.DispatchRemarks,'') as DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,isnull(t.DispatchCancel_Smid,'') as DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where t1.DistId in (" + distIdStr1 + ") " + condition + " and " + Qrychk1 + " and Isnull(t.OrderType,'') not  in ('D','C') UNION ALL SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(18,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,isnull(t.DispatchRemarks,'') as DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,isnull(t.DispatchCancel_Smid,'') as DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where t1.DistId in (" + SupdistIdStr1 + ") " + condition + " and " + Qrychk1 + " and Isnull(t.OrderType,'') not  in ('D','C')";
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Super Distributor and Distributor/Under Super Distributor');", true);
                            }
                        }
                        else
                        {
                            if (distIdStr1 == "" && SupdistIdStr1 != "")
                            {

                                query = "SELECT Max(T.VDate) VDate, Max(T.Superdistributor) Superdistributor,Max(T.DisSyncId) DisSyncId,Max(T.Distributor) Distributor,  T.DocId DocId, Sum(T.TotalQty) TotalQty, CAST( Sum(T.TotalAmount) AS NUMERIC(38,2)) TotalAmount , Max(T.DistId) DistId,Max(T.POrdId) POrdId,Max(T.partyid) partyid,Max(T.OrderPlaceBy) OrderPlaceBy,Max(T.OrderStatus) OrderStatus ,Max(T.Supersyncid) Supersyncid,Isnull((SELECT SMName  Name FROM MastSalesRep  WHERE UserId=Max(T.DispatchCancel_Smid) UNION ALL SELECT PartyName Name FROM MastParty WHERE UserId=Max(T.DispatchCancel_Smid)),'')  DispatchCancelby,Max(T.dispatchcanceldatetime) DispatchCancelDate   FROM( SELECT  P.VDate VDate, Mp.PartyName AS Superdistributor,'' AS DisSyncId,'' AS Distributor, P.PODocId DocId , tp1.Qty  AS TotalQty, rate*qty AS TotalAmount ,P.DistId,P.POrdId,0 AS partyid,Mp.disttype ,case when P.fromapp is null then ms.smname else  Mp.partyname end as OrderPlaceBy , (case P.OrderType when 'C'  then 'Cancel' when 'D' then 'Dispatch' else 'Pending' end) as OrderStatus ,MP.SyncId Supersyncid,Isnull(DispatchCancel_Smid,0) DispatchCancel_Smid,P.dispatchcanceldatetime dispatchcanceldatetime  FROM TransPurchOrder P LEFT JOIN	 TransPurchOrder1 tp1 ON tp1.PODocId=P.PODocId left join mastsalesrep ms on P.smid=ms.smid LEFT JOIN Mastparty MP ON MP.PartyId=P.DistId WHERE  P.DistId in (" + SupdistIdStr1 + ") and " + Qrychk + " " + condition + "  )AS T  GROUP BY T.DocId ORDER BY Max(T.VDate) desc ";

                                orderdetail = @"SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where t1.DistId in (" + SupdistIdStr1 + ") and " + Qrychk1 + " and Isnull(t.OrderType,'') in ('D','C')";

                                Pendingorderdetail = @"SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(18,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,isnull(t.DispatchRemarks,'') as DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,isnull(t.DispatchCancel_Smid,'') as DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where t1.DistId in (" + SupdistIdStr1 + ") and " + Qrychk1 + " and Isnull(t.OrderType,'') not  in ('D','C')";

                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Super Distributor');", true);
                            }
                        }
                    }
                    else
                    {

                        query = "SELECT Max(T.VDate) VDate, Max(T.Superdistributor) Superdistributor,Max(T.DisSyncId) DisSyncId,Max(T.Distributor) Distributor,  T.DocId DocId, Sum(T.TotalQty) TotalQty, CAST( Sum(T.TotalAmount) AS NUMERIC(38,2)) TotalAmount , Max(T.DistId) DistId,Max(T.POrdId) POrdId,Max(T.partyid) partyid,Max(T.OrderPlaceBy) OrderPlaceBy,Max(T.OrderStatus) OrderStatus ,Max(T.Supersyncid) Supersyncid,Isnull((SELECT SMName  Name FROM MastSalesRep  WHERE UserId=Max(T.DispatchCancel_Smid) UNION ALL SELECT PartyName Name FROM MastParty WHERE UserId=Max(T.DispatchCancel_Smid)),'')  DispatchCancelby ,Max(T.dispatchcanceldatetime) DispatchCancelDate    FROM(SELECT P.VDate VDate, MD.PartyName AS Superdistributor,MP.SyncId AS DisSyncId,Mp.partyname AS Distributor, P.PODocId DocId , tp1.Qty  AS TotalQty, rate*qty AS TotalAmount, P.DistId,P.POrdId,Mp.partyid,Mp.disttype,case when P.fromapp is null then ms.smname else  Mp.partyname end as OrderPlaceBy,(case P.OrderType when 'C' then 'Cancel' when 'D' then 'Dispatch' else 'Pending' end) as OrderStatus ,MD.SyncId Supersyncid,Isnull(DispatchCancel_Smid,0) DispatchCancel_Smid,P.dispatchcanceldatetime dispatchcanceldatetime FROM TransPurchOrder P LEFT JOIN	 TransPurchOrder1 tp1 ON tp1.PODocId=P.PODocId LEFT JOIN Mastparty MP ON MP.PartyId=P.DistId left join mastsalesrep ms on P.smid=ms.smid LEFT JOIN Mastparty MD ON MD.PartyId=MP.SD_Id WHERE  MP.areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'  and PrimCode in (SELECT smid FROM MastSalesRep  WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")))) and " + Qrychk + " " + condition + " )AS T  GROUP BY T.DocId ORDER BY Max(T.VDate) desc ";

                        orderdetail = @"SELECT t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where " + Qrychk1 + " and Isnull(t.OrderType,'') in ('D','C')";

                        Pendingorderdetail = @"SELECT t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(18,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,isnull(t.DispatchRemarks,'') as DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,isnull(t.DispatchCancel_Smid,'') as DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where " + Qrychk1 + " and Isnull(t.OrderType,'') not  in ('D','C')";
                    }

                    //                    if (distIdStr1 != "")
                    //                    {
                    //                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(10,2)) as TotalAmount,max(case t2.OrderType when 'C' then 'Cancel' when 'D' then 'Dispatch' else 'Pending' end) as OrderStatus,OrderPlaceBy from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount,case when t.fromapp is null then ms.smname
                    //            else  da.partyname end as OrderPlaceBy from TransPurchOrder1 t1 left join Transpurchorder t on t1.podocid=t.podocid left outer join MastItem mi
                    //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId left join mastsalesrep ms on t.smid=ms.smid where 
                    //             t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate,ms.smname,t.fromapp ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId   group by tbl.DocId,OrderPlaceBy order by VDate desc";


                    //                        orderdetail = @"SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId 
                    //                      left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where 
                    //             t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " and Isnull(t.OrderType,'') in ('D','C')";


                    //                        Pendingorderdetail = @"SELECT  t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId 
                    //                      left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where 
                    //             t1.DistId in (" + distIdStr1 + ") and " + Qrychk + " and Isnull(t.OrderType,'') not  in ('D','C')";

                    //                    }//where t2.OrderType is null
                    //                    else
                    //                    {
                    //                        query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty,cast(sum(amount) as decimal(10,2)) as TotalAmount,max(case t2.OrderType when 'C' then 'Cancel' when 'D' then 'Dispatch' else 'Pending' end) as OrderStatus,OrderPlaceBy from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount,case when t.fromapp is null then ms.smname
                    //            else  da.partyname end as OrderPlaceBy from TransPurchOrder1 t1 left join Transpurchorder t on t1.podocid=t.podocid left outer join MastItem mi
                    //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId left join mastsalesrep ms on t.smid=ms.smid where " + Qrychk + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate,ms.smname,t.fromapp ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId  group by tbl.DocId,OrderPlaceBy order by VDate desc";

                    //                        orderdetail = @"SELECT t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId 
                    //                      left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where 
                    //            " + Qrychk + " and Isnull(t.OrderType,'') in ('D','C')";


                    //                        Pendingorderdetail = @"SELECT t1.PODocId as DocId,mi.ItemName+'-'+Isnull(mi.SyncId,'') as ItemName,t1.Qty as OrderQty,cast((t1.Qty*t1.Rate) as decimal(10,2)) as OrderAmount,IsNull(t1.DispatchQty,0) as DispatchQty,t.DispatchRemarks,case when Isnull(t.OrderType,'')='D' then t.DispatchRemarks+'  - Dispatch Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Null then t.DispatchRemarks+'  - Cancel Remark By: '+msp.smname when Isnull(t.OrderType,'')='C' and OrderCancelBy_Distid is Not Null then t.DispatchRemarks+'  - Cancel Remark By: '+mp1.PartyName else 'Order Is Pending' end as Ordertype ,t.DispatchCancel_Smid,mp.partyname+'-'+Isnull(mp.SyncId,'') As partyname,t.vdate,t.remarks FROM TransPurchOrder1 t1  left join transpurchorder t on t.podocid=t1.podocid left join mastitem mi on t1.ItemId = mi.ItemId 
                    //                      left join mastsalesrep msp on msp.smid=t.DispatchCancel_Smid left join mastparty mp on mp.partyid=t.distid left join mastparty mp1 on mp1.partyid=t.OrderCancelBy_Distid where 
                    //              " + Qrychk + " and Isnull(t.OrderType,'') not  in ('D','C')";
                    //                    }

                    //                    string query = @"select max(tbl.VDate) as VDate,max(tbl.Syncid) as Syncid,max(tbl.Distributor) as Distributor,max(tbl.DocId) as DocId,sum(Qty) as TotalQty from ( SELECT t1.VDate,da.Syncid,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],max(t1.PODocId) as DocId,sum(qty) as Qty,isnull(rate,0) as rate, isnull(sum(rate*qty),0)as amount from TransPurchOrder1 t1 left outer join MastItem mi
                    //             on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where 
                    //             t1.DistId in (" + distIdStr1 + ") and " + Qrychk + "  " + QueryMatGrp + " and da.partydist=1 group BY t1.VDate,da.syncid,da.PartyName, mi.ItemName,rate ) tbl left join TransPurchOrder t2 on tbl.DocId= t2.PODocId and t2.OrderType is null group by tbl.DocId order by VDate desc";

                    DataTable dtordes = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    DataTable dtdispatchordes = DbConnectionDAL.GetDataTable(CommandType.Text, orderdetail);
                    DataTable dtpendingorders = DbConnectionDAL.GetDataTable(CommandType.Text, Pendingorderdetail);


                    Excel.Application excelApp = new Excel.Application();
                    string path = Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "DistributorOrderDetail" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";

                    if (File.Exists(path + filename))
                    {
                        File.Delete(path + filename);
                    }


                    string strPath = Server.MapPath("ExportedFiles//" + filename);
                    Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
                    Microsoft.Office.Interop.Excel.Range chartRange;
                    Excel.Range range;
                    if (dtordes.Rows.Count > 0)
                    {
                        Ds.Tables.Add(dtpendingorders);
                        Ds.Tables.Add(dtdispatchordes);
                        Ds.Tables.Add(dtordes);



                        if (Ds.Tables.Count > 0)
                        {
                            for (int i = 0; i < Ds.Tables.Count; i++)
                            {

                                DataTable table = Ds.Tables[i];



                                //Add a new worksheet to workbook with the Datatable name
                                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                                if (i == 0)
                                {
                                    excelWorkSheet.Name = "Pending Order";


                                }
                                if (i == 1)
                                {
                                    excelWorkSheet.Name = "DispatchCancel Order";
                                }
                                if (i == 2)
                                {
                                    excelWorkSheet.Name = "Total Order";
                                }
                                for (int j = 1; j < table.Columns.Count + 1; j++)
                                {
                                    excelWorkSheet.Cells[1, j] = table.Columns[j - 1].ColumnName;

                                    range = excelWorkSheet.Cells[1, j] as Excel.Range;
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
                                excelWorkSheet.Columns.AutoFit();
                                excelWorkSheet.Application.ActiveWindow.SplitRow = 1;
                                excelWorkSheet.Application.ActiveWindow.FreezePanes = true;

                                Excel.FormatConditions fcs = chartRange.FormatConditions;
                                Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
                (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                                //Excel.FormatCondition format = xlWorksheet.Rows.FormatConditions.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                                format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                            }

                        }




                        excelWorkBook.SaveAs(strPath);
                        excelWorkBook.Close();
                        excelApp.Quit();
                        // excelApp.Visible = true;
                        Response.ContentType = "application/x-msexcel";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.TransmitFile(strPath);
                        Response.End();


                    }
                    else
                    {
                        //rptmain.Style.Add("display", "block");
                        //distitemsalerpt.DataSource = dtItem;
                        //distitemsalerpt.DataBind();

                    }

                    //}
                    //else
                    //{
                    //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select distributor');", true);
                    //    distitemsalerpt.DataSource = null;
                    //    distitemsalerpt.DataBind();
                    //}
                }

                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                    //    distitemsalerpt.DataSource = null;
                    //    distitemsalerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Error While Getting Records.');", true);
            }
            spinner.Visible = false;
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistributerDispatchOrderViewForm_V2.aspx");
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //distitemsalerpt.DataSource = null;
            //distitemsalerpt.DataBind();

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
                    //  BindDistributorSalesmanWise(smIDStr12);
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

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltype.SelectedIndex > 0)
            {
                string disttype = ddltype.SelectedValue;

                string smIDStr12 = "", smIDStr = "", distIdStr1 = "";
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
                    if (disttype == "DEPOT" || disttype == "C&F" || disttype == "SUPERDIST")
                    {
                        dvsd.Visible = true;
                        mlbltypsd.Visible = true;
                        if (disttype == "DEPOT")
                        {
                            lbltypsd.Text = "DEPOT:";
                        }
                        else if (disttype == "C&F")
                        {
                            lbltypsd.Text = "C&F:";
                        }
                        else if (disttype == "SUPERDIST")
                        {
                            lbltypsd.Text = "Super Distributor:";
                        }
                        dvusd.Visible = false;
                        LstSD.Items.Clear();
                        ListBox1.Items.Clear();
                        LstSD.DataSource = null;
                        LstSD.DataBind();
                        ListBox1.DataSource = null;
                        ListBox1.DataBind();
                        BindDistributorDDl(smIDStr12);
                    }
                    else
                    {
                        dvsd.Visible = true;
                        mlbltypsd.Visible = true;
                        dvusd.Visible = true;
                        mlblusd.Visible = true;
                        lbltypsd.Text = "Super Distributor:";
                        if (disttype == "DIST")
                        {
                            lblusd.Text = "Distributor:";
                        }
                        else if (disttype == "UNDERSD")
                        {
                            lblusd.Text = "Under Super Distributor:";
                        }
                        foreach (ListItem item in LstSD.Items)
                        {
                            if (item.Selected)
                            {
                                distIdStr1 += item.Value + ",";
                            }
                        }
                        distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');
                        if (distIdStr1 == "")
                        {
                            BindDistributorDDl(smIDStr12);
                        }
                        else
                        {
                            ListBox1.Items.Clear();
                            ListBox1.DataSource = null;
                            ListBox1.DataBind();
                            Binddistributor();
                        }
                        //
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);

                    //distitemsalerpt.DataSource = null;
                    //distitemsalerpt.DataBind();
                    ddltype.SelectedIndex = 0;
                }


            }
            else
            {
                DataTable dt = new DataTable();
                LstSD.Items.Clear();
                ListBox1.Items.Clear();
                LstSD.DataSource = dt;
                LstSD.DataBind();
                ListBox1.DataSource = dt;
                ListBox1.DataBind();
                dvsd.Visible = false;
                dvusd.Visible = false;
            }
        }

        private void Binddistributor()
        {

            string strQ = "";
            string res = "", distIdStr1 = "";
            DataTable dt = new DataTable();

            foreach (ListItem item in LstSD.Items)
            {
                if (item.Selected)
                {
                    distIdStr1 += item.Value + ",";
                }
            }
            distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');
            try
            {
                string condition = "";
                if (distIdStr1 != "")
                {
                    if (ddltype.SelectedIndex > 0)
                        condition = " and MP.DistType='" + ddltype.SelectedValue + "' ";

                    strQ = "select Id,Name from (select MP.PartyId As Id,MP.PartyName + ' - ' + md.Text + ' ( ' + ma.AreaName + ' )' As Name from MastParty MP left join MastDistributorType md on md.Value=MP.DistType left join MastArea ma on ma.AreaId=MP.CityId where  MP.SD_ID in (" + distIdStr1 + ")  " + condition + " and PartyDist=1 and MP.Active=1 UNION select MP.PartyId As Id,MP.PartyName + ' - ' + md.Text + ' ( ' + ma.AreaName + ' )' As Name from MastParty MP left join MastDistributorType md on md.Value=MP.DistType left join MastArea ma on ma.AreaId=MP.CityId   where PartyDist=1 and MP.Active=1 and SD_id IS NULL " + condition + ")a order by a.Name ";
                }
                else
                {
                    if (ddltype.SelectedIndex > 0)
                        condition = " where MP.DistType='" + ddltype.SelectedValue + "' ";

                    strQ = "select MP.PartyId As Id,MP.PartyName + ' - ' + md.Text + ' ( ' + ma.AreaName + ' )' As Name from MastParty MP left join MastDistributorType md on md.Value=MP.DistType left join MastArea ma on ma.AreaId=MP.CityId   " + condition + " and PartyDist=1 and MP.Active=1  order by PartyName";
                }


                dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
                if (dt.Rows.Count > 0)
                {
                    ListBox1.DataSource = dt;
                    ListBox1.DataTextField = "Name";
                    ListBox1.DataValueField = "Id";
                    ListBox1.DataBind();

                }
                else
                {
                    //LstSD.Items.Clear();
                    ListBox1.Items.Clear();
                    ListBox1.DataSource = null;
                    ListBox1.DataBind();
                }


            }
            catch (Exception ex)
            {


            }


        }

        protected void LstSD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstSD.SelectedIndex != -1)
            {
                Binddistributor();
            }
            else
            {
                ListBox1.Items.Clear();
            }
            //ItemDetail.Style.Add("display", "none");
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
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
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
    }
}