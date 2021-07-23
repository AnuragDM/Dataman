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
using System.Web.Services;

namespace AstralFFMS
{
    public partial class BeatPlanVsActualReport : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {//Ankita - 13/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
                //BindSalesPerson();
                List<Distributors> distributors = new List<Distributors>();
                distributors.Add(new Distributors());
                tourvsactrpt.DataSource = distributors;
                tourvsactrpt.DataBind();
                //fill_TreeArea();
                BindTreeViewControl();
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                BindDDLMonth();
                ddlMonthSecSale.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlYearSecSale.SelectedValue = System.DateTime.Now.Year.ToString();
                btnExport.Visible = true;
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
        public class Distributors
        {


            public string Date { get; set; }
            public string SalesRepName { get; set; }
            public string SyncId { get; set; }
            public string BeatPlanBeat { get; set; }
            public string BeatPlansyncid { get; set; }
            public string VisitBeat { get; set; }
            public string VisitBeatsyncid { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public static string GetBeatVsActual(string Fromdate, string Todate)
        {
            string smIDStr1 = "";
            smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
            string[] smid = smIDStr1.Split(',');
            DataTable dtsmid = new DataTable();
            dtsmid.Columns.Add("SMid");
            for (int i = 0; i < smid.Length; i++)
            {
                DataRow dr = dtsmid.NewRow();
                dr[0] = smid[i];
                dtsmid.Rows.Add(dr);
                //string sql = "insert into temp_SmidStore (UserId,smid)Values("+Settings.Instance.UserID+","+smid[i]+")";
                //DAL.DbConnectionDAL.ExecuteQuery(sql);
            }
            dtsmid.AcceptChanges();
            string query, Qrychk = "";

            Qrychk = "om.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59'";

            //            query = @"select case when [VDate1] is null then [VDate2] else [VDate1] end [Date],BeatPlanBeat, VisitBeat,[Remark] ,emp_id, SalesRepName,SyncId,BeatPlansyncid,VisitBeatsyncid from (
            //                            select a.[VDate] as [VDate1], a.[Beat] [BeatPlanBeat],a.BeatPlansyncid, b.[VDate] [VDate2],b.Beat AS [VisitBeat],b.visitBeatsyncid, b.[Remark], CASE WHEN a.SalesRepName IS NULL THEN b.SalesRepName ELSE a.SalesRepName END 
            //                            SalesRepName,case when a.[emp_id] is null then b.[emp_id] else a.[emp_id] end emp_id,case when a.[syncid] is null then b.[syncid] else a.[syncid] end syncid from ( 
            //
            //                            select tbp.PlannedDate [VDate], c.AreaName [Beat],c.Syncid as BeatPlansyncid, '' [Remark],tbp.SMId [emp_id], msr.SMName as SalesRepName,msr.SyncId from TransBeatPlan tbp LEFT join MastArea c on c.AreaId=tbp.BeatId LEFT join MastSalesRep msr on msr.SMId=tbp.SMId
            //                            where tbp.SMId IN (" + smIDStr1 + ") and tbp.PlannedDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' and tbp.Appstatus='Approve' )" +
            //                        " a FULL JOIN (select  x.VDate,x.Beat,x.visitBeatsyncid,max(x.Remark) as Remark,x.[emp_id],x.SalesRepName,x.SyncId from (SELECT om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS Remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransOrder om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid in (" + smIDStr1 + ") and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
            //                        " union all SELECT om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId IN (" + smIDStr1 + ") and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
            //                        " union All select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "   group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
            //                        " UNION ALL select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,'Competitor' AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCompetitor om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId,b.syncid " +
            //                        " UNION ALL select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId,b.syncid )x Group by  x.VDate,x.Beat,x.visitBeatsyncid,x.[emp_id],x.SalesRepName,x.SyncId " +
            //                        " ) b ON a.VDate = b.VDate and a.emp_id=b.emp_id) main  Order by Date,SalesRepName,[BeatPlanBeat],[VisitBeat],[VisitBeatsyncid],[Remark]";
            //                          DataTable dtbrandsale = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            //                         return Newtonsoft.Json.JsonConvert.SerializeObject(dtbrandsale);         

            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@DateFrom", DbParameter.DbType.DateTime, 1, Fromdate);
            dbParam[1] = new DbParameter("@DateTo", DbParameter.DbType.DateTime, 1, Todate);
            dbParam[2] = new DbParameter("@ExClientItemGrptbl", DbParameter.DbType.Datatable, 8000, dtsmid);
            DataTable dtbrandsale = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_Beatplanactual", dbParam);
            dtbrandsale.Columns.Add("Weekday");
            dtbrandsale.AcceptChanges();
            for (int i = 0; i < dtbrandsale.Rows.Count; i++)
            {
                dtbrandsale.Rows[i]["Weekday"] = (Convert.ToDateTime(dtbrandsale.Rows[i]["Date"].ToString())).ToString("ddd");

            }
            dtbrandsale.AcceptChanges();
            return Newtonsoft.Json.JsonConvert.SerializeObject(dtbrandsale);



        }
        //private void BindSalesPerson()
        //{
        //    try
        //    {

        //        if (roleType == "Admin")
        //        {
        //            //Ankita - 13/may/2016- (For Optimization)
        //            //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            DataTable dtcheckrole = new DataTable();
        //            dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //            DataView dv1 = new DataView(dtcheckrole);
        //            //dv1.RowFilter = "RoleType='CityHead' or RoleType='DistrictHead' or RoleType='AreaIncharge' and SMName<>'.'";
        //            dv1.RowFilter = "SMName<>'.'";
        //            dv1.Sort = "SMName asc";

        //            ListBox1.DataSource = dv1.ToTable();
        //            ListBox1.DataTextField = "SMName";
        //            ListBox1.DataValueField = "SMId";
        //            ListBox1.DataBind();
        //        }
        //        else
        //        {
        //            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
        //            DataView dv = new DataView(dt);
        //            //dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
        //            dv.RowFilter = "SMName<>'.'";
        //            dv.Sort = "SMName asc";
        //            ListBox1.DataSource = dv.ToTable();
        //            ListBox1.DataTextField = "SMName";
        //            ListBox1.DataValueField = "SMId";
        //            ListBox1.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
        //void fill_TreeArea()
        //{
        //    int lowestlvl = 0;
        //    DataTable St = new DataTable();
        //    if (roleType == "Admin")
        //    {

        //        //string strrole = "select SMID,SMName from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //        //St = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //        //    lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
        //        //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,1);
        //        St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
        //    }
        //    else
        //    {//Ankita - 17/may/2016- (For Optimization)
        //        //lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
        //        //St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID, lowestlvl);
        //        St = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT mastrole.rolename,mastsalesrep.smid,smname + ' (' + mastsalesrep.syncid + ' - '+ mastrole.rolename + ')' AS smname, mastsalesrep.lvl,mastrole.roletype FROM   mastsalesrep LEFT JOIN mastrole ON mastrole.roleid = mastsalesrep.roleid WHERE  smid =" + Settings.Instance.SMID + "");
        //    }


        //    trview.Nodes.Clear();

        //    if (St.Rows.Count <= 0)
        //    {
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found !');", true);
        //        return;
        //    }
        //    foreach (DataRow row in St.Rows)
        //    {
        //        TreeNode tnParent = new TreeNode();
        //        tnParent.Text = row["SMName"].ToString();
        //        tnParent.Value = row["SMId"].ToString();
        //        trview.Nodes.Add(tnParent);
        //        //tnParent.ExpandAll();
        //        tnParent.CollapseAll();
        //        //Ankita - 17/may/2016- (For Optimization)
        //        //FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
        //        getchilddata(tnParent, tnParent.Value);
        //    }
        //}
        ////Ankita - 17/may/2016- (For Optimization)
        //private void getchilddata(TreeNode parent, string ParentId)
        //{

        //    string SmidVar = string.Empty;
        //    string GetFirstChildData = string.Empty;
        //    int levelcnt = 0;
        //    if (Settings.Instance.RoleType == "Admin")
        //        //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
        //        levelcnt = Convert.ToInt16("0") + 2;
        //    else
        //        levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 1;


        //    GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 order by SMName,lvl desc ";
        //    DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

        //    if (FirstChildDataDt.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
        //        {
        //            SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
        //            FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
        //        }
        //        SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

        //        // for (int j = levelcnt + 1; j <= 5; j++)
        //        for (int j = levelcnt + 1; j <= 9; j++)
        //        {
        //            string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
        //            DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
        //            if (dtChild.Rows.Count > 0)
        //            {
        //                SmidVar = string.Empty;
        //                int mTotRows = dtChild.Rows.Count;
        //                if (mTotRows > 0)
        //                {
        //                    var str = "";
        //                    for (int k = 0; k < mTotRows; k++)
        //                    {
        //                        SmidVar += dtChild.Rows[k]["smid"].ToString() + ",";
        //                    }

        //                    TreeNode Oparent = parent;
        //                    switch (j)
        //                    {
        //                        case 3:
        //                            if (Oparent.Parent != null)
        //                            {
        //                                foreach (TreeNode child in Oparent.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                foreach (TreeNode child in Oparent.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }
        //                            break;
        //                        case 4:
        //                            if (Oparent.Parent != null)
        //                            {
        //                                foreach (TreeNode child in Oparent.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                foreach (TreeNode child in Oparent.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }
        //                            break;
        //                        case 5:
        //                            if (Oparent.Parent != null)
        //                            {
        //                                foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                                {
        //                                    foreach (TreeNode child in Pchild.ChildNodes)
        //                                    {
        //                                        str += child.Value + ","; parent = child;
        //                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                        for (int l = 0; l < dr.Length; l++)
        //                                        {
        //                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                        }
        //                                        dtChild.Select();
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                foreach (TreeNode child in Oparent.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }


        //                            break;
        //                        case 6:
        //                            if (Oparent.Parent != null)
        //                            {
        //                                if (Settings.Instance.RoleType == "Admin")
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode Qchild in Pchild.ChildNodes)
        //                                        {
        //                                            foreach (TreeNode child in Qchild.ChildNodes)
        //                                            {
        //                                                str += child.Value + ","; parent = child;
        //                                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                                for (int l = 0; l < dr.Length; l++)
        //                                                {
        //                                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                                }
        //                                                dtChild.Select();
        //                                            }
        //                                        }
        //                                    }
        //                                }

        //                                else
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode child in Pchild.ChildNodes)
        //                                        {
        //                                            str += child.Value + ","; parent = child;
        //                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                            for (int l = 0; l < dr.Length; l++)
        //                                            {
        //                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                            }
        //                                            dtChild.Select();
        //                                        }
        //                                    }
        //                                }

        //                            }
        //                            break;
        //                        case 7:
        //                            if (Oparent.Parent != null)
        //                            {
        //                                if (Settings.Instance.RoleType == "Admin")
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.Parent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode schild in Pchild.ChildNodes)
        //                                        {
        //                                            foreach (TreeNode Qchild in schild.ChildNodes)
        //                                            {
        //                                                foreach (TreeNode child in Qchild.ChildNodes)
        //                                                {
        //                                                    str += child.Value + ","; parent = child;
        //                                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                                    for (int l = 0; l < dr.Length; l++)
        //                                                    {
        //                                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                                    }
        //                                                    dtChild.Select();
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }

        //                                else
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode child in Pchild.ChildNodes)
        //                                        {
        //                                            str += child.Value + ","; parent = child;
        //                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                            for (int l = 0; l < dr.Length; l++)
        //                                            {
        //                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                            }
        //                                            dtChild.Select();
        //                                        }
        //                                    }
        //                                }

        //                            }


        //                            break;
        //                        case 8:
        //                            if (Oparent.Parent != null)
        //                            {
        //                                if (Settings.Instance.RoleType == "Admin")
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode Qchild in Pchild.ChildNodes)
        //                                        {
        //                                            foreach (TreeNode child in Qchild.ChildNodes)
        //                                            {
        //                                                str += child.Value + ","; parent = child;
        //                                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                                for (int l = 0; l < dr.Length; l++)
        //                                                {
        //                                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                                }
        //                                                dtChild.Select();
        //                                            }
        //                                        }
        //                                    }
        //                                }

        //                                else
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode child in Pchild.ChildNodes)
        //                                        {
        //                                            str += child.Value + ","; parent = child;
        //                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                            for (int l = 0; l < dr.Length; l++)
        //                                            {
        //                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                            }
        //                                            dtChild.Select();
        //                                        }
        //                                    }
        //                                }

        //                            }


        //                            break;
        //                        case 9:
        //                            if (Oparent.Parent != null)
        //                            {
        //                                if (Settings.Instance.RoleType == "Admin")
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode Qchild in Pchild.ChildNodes)
        //                                        {
        //                                            foreach (TreeNode child in Qchild.ChildNodes)
        //                                            {
        //                                                str += child.Value + ","; parent = child;
        //                                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                                for (int l = 0; l < dr.Length; l++)
        //                                                {
        //                                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                                }
        //                                                dtChild.Select();
        //                                            }
        //                                        }
        //                                    }
        //                                }

        //                                else
        //                                {
        //                                    foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode child in Pchild.ChildNodes)
        //                                        {
        //                                            str += child.Value + ","; parent = child;
        //                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                            for (int l = 0; l < dr.Length; l++)
        //                                            {
        //                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                            }
        //                                            dtChild.Select();
        //                                        }
        //                                    }
        //                                }

        //                            }

        //                            else
        //                            {
        //                                foreach (TreeNode child in Oparent.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }


        //                            break;
        //                    }

        //                    SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);
        //                }
        //            }
        //        }
        //    }


        //}

        //public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        //{
        //    TreeNode child = new TreeNode();
        //    child.Text = SMName;
        //    child.Value = Smid;
        //    child.SelectAction = TreeNodeSelectAction.Expand;
        //    parent.ChildNodes.Add(child);
        //    child.CollapseAll();
        //}
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
        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    ddlMonthSecSale.Items.Add(new ListItem(monthName.Substring(0, 3), month.ToString().PadLeft(2, '0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    ddlYearSecSale.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatPlanVsActualReport.aspx");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr = "";
                string smIDStr1 = "", filter = "";

                //foreach (ListItem item in ListBox1.Items)
                //{
                //    if (item.Selected)
                //    {
                //        smIDStr1 += item.Value + ",";
                //    }
                //}
                //smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                string Qrychk = "", getComplQry = "", query = "";
                string frmdate = txtfmDate.Text;
                string ToDate = txttodate.Text;

                Qrychk = "om.VDate between '" + Settings.dateformat(frmdate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59'";


                if (ddlFilter.SelectedIndex != 0 && ddlFilter.SelectedValue != "1")
                {
                    filter = "where isnull(TourDistributor,'')<>isnull(VisitDistributor,'')";
                }

                if (smIDStr1 != "")
                {
                    if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                    {
                        if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                        {
                            // Nishu 13/10/2016   BeatSync id requirment

                            //                            query = @"select case when [VDate1] is null then [VDate2] else [VDate1] end [Date],BeatPlanBeat, VisitBeat,[Remark] ,emp_id, SalesRepName,SyncId from (
                            //                            select a.[VDate] as [VDate1], a.[Beat] [BeatPlanBeat], b.[VDate] [VDate2],b.Beat AS [VisitBeat], b.[Remark], CASE WHEN a.SalesRepName IS NULL THEN b.SalesRepName ELSE a.SalesRepName END 
                            //                            SalesRepName,case when a.[emp_id] is null then b.[emp_id] else a.[emp_id] end emp_id,case when a.[syncid] is null then b.[syncid] else a.[syncid] end syncid from ( 
                            //
                            //                            select tbp.PlannedDate [VDate], c.AreaName [Beat], '' [Remark],tbp.SMId [emp_id], msr.SMName as SalesRepName,msr.SyncId from TransBeatPlan tbp LEFT join MastArea c on c.AreaId=tbp.BeatId LEFT join MastSalesRep msr on msr.SMId=tbp.SMId
                            //                            where (tbp.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))) and tbp.PlannedDate between '" + Settings.dateformat(frmdate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and tbp.Appstatus='Approve' )" +
                            //                            " a FULL JOIN (select  x.VDate,x.Beat,max(x.Remark) as Remark,x.[emp_id],x.SalesRepName,x.SyncId from (SELECT om.VDate,(b.AreaName) as Beat,max(om.Remarks) AS Remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransOrder om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid in (" + smIDStr1 + ") and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId " +
                            //                            " union all SELECT om.VDate,(b.AreaName) as Beat,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId " +
                            //                            " union All select om.VDate,(b.AreaName) as Beat,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) AND p.PartyDist=0 and " + Qrychk + "   group BY om.VDate, b.AreaName,b.AreaId,p.BeatId " +
                            //                            " UNION ALL select om.VDate,(b.AreaName) as Beat,'Competitor' AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCompetitor om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId " +
                            //                            " UNION ALL select om.VDate,(b.AreaName) as Beat,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId )x Group by  x.VDate,x.Beat,x.[emp_id],x.SalesRepName,x.SyncId " +
                            //                            " ) b ON a.VDate = b.VDate and a.emp_id=b.emp_id) main  Order by Date,SalesRepName,[BeatPlanBeat],[VisitBeat],[Remark]";

                            query = @"select case when [VDate1] is null then [VDate2] else [VDate1] end [Date],BeatPlanBeat, VisitBeat,[Remark] ,emp_id, SalesRepName,SyncId,BeatPlansyncid,VisitBeatsyncid from (
                            select a.[VDate] as [VDate1], a.[Beat] [BeatPlanBeat],a.BeatPlansyncid, b.[VDate] [VDate2],b.Beat AS [VisitBeat],b.visitBeatsyncid, b.[Remark], CASE WHEN a.SalesRepName IS NULL THEN b.SalesRepName ELSE a.SalesRepName END 
                            SalesRepName,case when a.[emp_id] is null then b.[emp_id] else a.[emp_id] end emp_id,case when a.[syncid] is null then b.[syncid] else a.[syncid] end syncid from ( 

                            select tbp.PlannedDate [VDate], c.AreaName [Beat],c.Syncid as BeatPlansyncid, '' [Remark],tbp.SMId [emp_id], msr.SMName as SalesRepName,msr.SyncId from TransBeatPlan tbp LEFT join MastArea c on c.AreaId=tbp.BeatId LEFT join MastSalesRep msr on msr.SMId=tbp.SMId
                            where tbp.SMId IN (" + smIDStr1 + ") and tbp.PlannedDate between '" + Settings.dateformat(frmdate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and tbp.Appstatus='Approve' )" +
                           " a FULL JOIN (select  x.VDate,x.Beat,x.visitBeatsyncid,max(x.Remark) as Remark,x.[emp_id],x.SalesRepName,x.SyncId from (SELECT om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS Remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransOrder om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid in (" + smIDStr1 + ") and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                           " union all SELECT om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId IN (" + smIDStr1 + ") and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                           " union All select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "   group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                           " UNION ALL select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,'Competitor' AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCompetitor om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                           " UNION ALL select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId,b.syncid )x Group by  x.VDate,x.Beat,x.visitBeatsyncid,x.[emp_id],x.SalesRepName,x.SyncId " +
                           " ) b ON a.VDate = b.VDate and a.emp_id=b.emp_id) main  Order by Date desc,SalesRepName,[BeatPlanBeat],[VisitBeat],[VisitBeatsyncid],[Remark]";

                            DataTable udt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                            if (udt.Rows.Count > 0)
                            {
                                rptmain.Style.Add("display", "block");
                                tourvsactrpt.DataSource = udt;
                                tourvsactrpt.DataBind();
                                btnExport.Visible = true;
                            }
                            else
                            {
                                rptmain.Style.Add("display", "block");
                                tourvsactrpt.DataSource = udt;
                                tourvsactrpt.DataBind();
                                btnExport.Visible = false;
                            }

                            udt.Dispose();
                        }
                        else
                        { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true); }
                    }
                }

                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select salespesron');", true);
                    tourvsactrpt.DataSource = null;
                    tourvsactrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BeatPlanvsActualReport.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Day".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Emp.Sync Id".TrimStart('"').TrimEnd('"') + "," + "Planned Beat".TrimStart('"').TrimEnd('"') + "," + "Planned Beat Sync Id".TrimStart('"').TrimEnd('"') + "," + "Visited Beat".TrimStart('"').TrimEnd('"') + "," + "Visited Beat Sync Id".TrimStart('"').TrimEnd('"') + "," + "Remark".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            //dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Day", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("PlannedBeat", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("PlannedBeatSyncId", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("VisitedBeat", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("VisitedBeatSyncId", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Remark", typeof(String)));           

            //foreach (RepeaterItem item in tourvsactrpt.Items)
            //{
            //    DataRow dr = dtParams.NewRow();
            //    Label DateLabel = item.FindControl("DateLabel") as Label;
            //    dr["Date"] = DateLabel.Text;
            //    Label DayLabel = item.FindControl("DayLabel") as Label;
            //    dr["Day"] = DayLabel.Text.ToString();
            //    Label SalesRepNameLabel = item.FindControl("SalesRepNameLabel") as Label;
            //    dr["SalesPerson"] = SalesRepNameLabel.Text.ToString();
            //    Label SyncIdLabel = item.FindControl("SyncIdLabel") as Label;
            //    dr["SyncId"] = SyncIdLabel.Text.ToString();
            //    Label BeatPlanBeatLabel = item.FindControl("BeatPlanBeatLabel") as Label;
            //    dr["PlannedBeat"] = BeatPlanBeatLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
            //    Label BeatPlansyncidLabel = item.FindControl("BeatPlansyncidLabel") as Label;
            //    dr["PlannedBeatSyncId"] = BeatPlansyncidLabel.Text.ToString();
            //    Label VisitBeatLabel = item.FindControl("VisitBeatLabel") as Label;
            //    dr["VisitedBeat"] = VisitBeatLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
            //    Label VisitBeatsyncidLabel = item.FindControl("VisitBeatsyncidLabel") as Label;
            //    dr["VisitedBeatSyncId"] = VisitBeatsyncidLabel.Text.ToString();
            //    Label RemarkLabel = item.FindControl("RemarkLabel") as Label;
            //    dr["Remark"] = RemarkLabel.Text.ToString();


            //    dtParams.Rows.Add(dr);
            //}
            string smIDStr = "";
            string smIDStr1 = "";

            //foreach (ListItem item in ListBox1.Items)
            //{
            //    if (item.Selected)
            //    {
            //        smIDStr1 += item.Value + ",";
            //    }
            //}
            //smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            string Qrychk = "";
            string frmdate = txtfmDate.Text;
            string ToDate = txttodate.Text;

            Qrychk = "om.VDate between '" + Settings.dateformat(frmdate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59'";

            string[] smid = smIDStr1.Split(',');
            DataTable dtsmid = new DataTable();
            dtsmid.Columns.Add("SMid");
            for (int i = 0; i < smid.Length; i++)
            {
                DataRow dr = dtsmid.NewRow();
                dr[0] = smid[i];
                dtsmid.Rows.Add(dr);
                //string sql = "insert into temp_SmidStore (UserId,smid)Values("+Settings.Instance.UserID+","+smid[i]+")";
                //DAL.DbConnectionDAL.ExecuteQuery(sql);
            }
            dtsmid.AcceptChanges();

            if (smIDStr1 != "")
            {
                if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                    {


                        //                        query = @"select case when [VDate1] is null then [VDate2] else [VDate1] end [Date], SalesRepName,SyncId,REPLACE(REPLACE(BeatPlanBeat, CHAR(13), ''), CHAR(10), '') as BeatPlanBeat,REPLACE(BeatPlansyncid, CHAR(13) + CHAR(10), ''),REPLACE(REPLACE(VisitBeat, CHAR(13), ''), CHAR(10), '') as VisitBeat,REPLACE(VisitBeatsyncid, CHAR(13) + CHAR(10), ''),REPLACE(Remark, CHAR(13) + CHAR(10), '') as [Remark] from  (
                        //                        select a.[VDate] as [VDate1], a.[Beat] [BeatPlanBeat],a.BeatPlansyncid, b.[VDate] [VDate2],b.Beat AS [VisitBeat],b.visitBeatsyncid, b.[Remark], CASE WHEN a.SalesRepName IS NULL THEN b.SalesRepName ELSE a.SalesRepName END 
                        //                        SalesRepName,case when a.[emp_id] is null then b.[emp_id] else a.[emp_id] end emp_id,case when a.[syncid] is null then b.[syncid] else a.[syncid] end syncid from ( 
                        //
                        //                        select tbp.PlannedDate [VDate], c.AreaName [Beat],c.Syncid as BeatPlansyncid, '' [Remark],tbp.SMId [emp_id], msr.SMName as SalesRepName,msr.SyncId from TransBeatPlan tbp LEFT join MastArea c on c.AreaId=tbp.BeatId LEFT join MastSalesRep msr on msr.SMId=tbp.SMId
                        //                        where tbp.SMId IN (" + smIDStr1 + ") and tbp.PlannedDate between '" + Settings.dateformat(frmdate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and tbp.Appstatus='Approve' )" +
                        //                      " a FULL JOIN (select  x.VDate,x.Beat,x.visitBeatsyncid,max(x.Remark) as Remark,x.[emp_id],x.SalesRepName,x.SyncId from (SELECT om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS Remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransOrder om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid in (" + smIDStr1 + ") and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                        //                      " union all SELECT om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId IN (" + smIDStr1 + ") and " + Qrychk + "  group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                        //                      " union All select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.SMId IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "   group BY om.VDate, b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                        //                      " UNION ALL select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,'Competitor' AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCompetitor om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId,b.syncid " +
                        //                      " UNION ALL select om.VDate,(b.AreaName) as Beat,b.syncid as visitBeatsyncid,max(om.Remarks) AS remark,max(tv.SMId) AS [emp_id],max(msr.SMName) AS SalesRepName,max(msr.SyncId) AS SyncId from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.BeatId LEFT JOIN Transvisit tv ON tv.SMId=om.SMId LEFT JOIN Mastsalesrep msr ON msr.SMId=tv.SMId where om.smid IN (" + smIDStr1 + ") AND p.PartyDist=0 and " + Qrychk + "  group BY om.VDate,b.AreaName,b.AreaId,p.BeatId,b.syncid )x Group by  x.VDate,x.Beat,x.visitBeatsyncid,x.[emp_id],x.SalesRepName,x.SyncId " +
                        //                      " ) b ON a.VDate = b.VDate and a.emp_id=b.emp_id) main  Order by Date desc,SalesRepName,[BeatPlanBeat],[VisitBeat],[VisitBeatsyncid],[Remark]";
                        DbParameter[] dbParam = new DbParameter[3];

                        dbParam[0] = new DbParameter("@DateFrom", DbParameter.DbType.DateTime, 1, frmdate);
                        dbParam[1] = new DbParameter("@DateTo", DbParameter.DbType.DateTime, 1, ToDate);
                        dbParam[2] = new DbParameter("@ExClientItemGrptbl", DbParameter.DbType.Datatable, 8000, dtsmid);
                        dtParams = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_Beatplanactual", dbParam);
                        dtParams.Columns.Add("Weekday");
                        dtParams.AcceptChanges();
                        for (int i = 0; i < dtParams.Rows.Count; i++)
                        {
                            dtParams.Rows[i]["Weekday"] = (Convert.ToDateTime(dtParams.Rows[i]["Date"].ToString())).ToString("ddd");

                        }

                        dtParams.Columns[8].SetOrdinal(1);

                        dtParams.AcceptChanges();
                    }
                    else
                    { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true); }
                }
            }


            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().TrimEnd().TrimStart().Contains(","))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().TrimEnd().TrimStart().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                            sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                        }
                        else
                        {
                            string h = dtParams.Rows[j][k].ToString();
                            string d = h.Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                            dtParams.Rows[j][k] = "";
                            dtParams.Rows[j][k] = d;
                            dtParams.AcceptChanges();
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BeatPlanvsActualReport.csv");
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();

            dtsmid.Dispose();
            sb.Clear();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=TourPlanvsActualReport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //tourvsactrpt.RenderControl(hw);
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

            }
        }
    }
}
