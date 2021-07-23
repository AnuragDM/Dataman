﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Text;

namespace AstralFFMS
{
    public partial class RptDailyWorkingSummaryVw_V2 : System.Web.UI.Page
    {
        public decimal runningPartyTotal = 0;
        public decimal runningRcallvisitedTotal = 0;
        public decimal runningDistDisccTotal = 0;
        public decimal runningDistFailedvisitTotal = 0;
        public decimal runningDistCollTotal = 0;
        public decimal runningorderTotal = 0;
        public decimal runningDemoTotal = 0;
        public decimal runningfailedvisitTotal = 0;
        public decimal runningCompTotal = 0;
        public decimal runningpartycollTotal = 0;
        public decimal runningRperCallAvgTotal = 0;
        public decimal runningNewpartyTotal = 0;
        public decimal runningClaimexpTotal = 0;
        public decimal runningApprexpTotal = 0;

        int smID = 0;
        int msg = 0;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            frmTextBox.Attributes.Add("readonly", "readonly");
            toTextBox.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary button1";

                //frmTextBox.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                frmTextBox.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                toTextBox.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                //Ankita - 18/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                // BindSalePersonDDl();
                // fill_TreeArea();
                BindTreeViewControl();
                btnExport.Visible = false;
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
        //private void BindSalePersonDDl()
        //{

        //    if (roleType == "Admin")
        //    {

        //        string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //        DataTable dtcheckrole = new DataTable();
        //        dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //        DataView dv1 = new DataView(dtcheckrole);
        //        //dv1.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
        //        dv1.RowFilter = "SMName<>'.'";
        //        dv1.Sort = "SMName asc";

        //        ListBox1.DataSource = dv1.ToTable();
        //        ListBox1.DataTextField = "SMName";
        //        ListBox1.DataValueField = "SMId";
        //        ListBox1.DataBind();
        //    }
        //    else
        //    {
        //     DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
        //        DataView dv = new DataView(dt);
        //        //dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";
        //        dv.RowFilter = "SMName<>'.'";
        //        dv.Sort = "SMName asc";

        //        ListBox1.DataSource = dv.ToTable();
        //        ListBox1.DataTextField = "SMName";
        //        ListBox1.DataValueField = "SMId";
        //        ListBox1.DataBind();
        //    }

        //}

        void fill_TreeArea()
        {
            int lowestlvl = 0;
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {
                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {
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
                //Ankita - 18/may/2016- (For Optimization)s
                // FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
                getchilddata(tnParent, tnParent.Value);
            }
        }
        //Ankita - 18/may/2016- (For Optimization)
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

                //   for (int j = levelcnt + 1; j <= 6; j++)  real code  commented for testing 2 feb17
                for (int j = levelcnt + 1; j <= 9; j++)
                {
                    //test 2 feb
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
                    //string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  order by SMName,lvl desc ";
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

        private void GetDailyWorkingSummaryL1(string SPID, string FromDate, string ToDate, string userID, string isExport)
        {
            try
            {

                string query = "", QryChk = "";
                string gettotalparty = "";
                int totalworkingDays = 0;

                if (ddlDsrType.SelectedItem.Text == "Lock")
                {
                    QryChk = "where a.lock1 =1 and a.Lock2=1";
                }
                if (ddlDsrType.SelectedItem.Text == "UnLock")
                {
                    QryChk = "where a.lock1 = 0 and a.Lock2 =0";
                }
                if (ddlType.SelectedValue != "3")
                {
                    if (QryChk == "")
                    {
                        QryChk += "where a.Type='" + ddlType.SelectedValue + "'";
                    }
                    else
                    {
                        QryChk += " and a.Type='" + ddlType.SelectedValue + "'";
                    }
                }
                if (ddlStatus.SelectedValue != "3")
                {
                    if (QryChk == "")
                    {
                        QryChk += " where a.AType='" + ddlStatus.SelectedValue + "'";
                    }
                    else
                    {
                        QryChk += " and a.AType='" + ddlStatus.SelectedValue + "'";
                    }
                }
                if (ddlDsrType.SelectedItem.Text == "All")
                {
                    gettotalparty = "(SELECT Count(*) FROM MastParty WHERE BeatId IN ( SELECT Distinct T.BeatId FROM GetSalesDatewiseBeat AS T WHERE T.VDate = a.VDate AND T.SMId= a.SMID) AND active=1 AND partydist=0)";

                    query = "select a.SMID as Id,convert(varchar,a.VDate,106) as [VDate],(case when max(a.RoleType) = 'AreaIncharge' then  a.Level1 + ' - L1' when  max(a.RoleType) in ('CityHead', 'DistrictHead') then a.Level1 + ' - L2' when max(a.RoleType) in ('StateHead', 'RegionHead') then a.Level1 + ' - L3'else a.Level1 end ) as Level1,(a.SyncId) as SyncId,Format(sum(a.TotalOrder),'N2') as TotalOrder,sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],sum(a.DistDiscuss) as DistDiscuss,sum(a.retailerdiscuss) as retailerdiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],sum(a.Competitor) [Competitor],0 as [Collections]," + gettotalparty + " as TotalParty, max(a.Remarks) as Remarks,CASE WHEN max(a.type)='9' THEN max(a.beat) Else '' END  AS beatname,max(a.cityname) as cityname,max(a.AppRemark) as AppRemark11,AType= (case when max(a.atype) is null then 'Pending' WHEN max(a.atype) ='Approve' THEN 'Approved' WHEN max(a.atype) ='Reject' THEN 'Rejected'  else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,a.EmpName, max(a.type) AS type,Max(CASE WHEN Type='9' THEN CASE WHEN ( a.lock1 IS NULL OR a.lock1 = 0 ) THEN 'UnLock' WHEN ( a.lock1 = 1 ) THEN 'Lock' END ELSE  NULL END)  AS lock,max(a.RoleType) as RoleType,a.Dsr_Time,a.FirstCall,a.LastCall,isnull(a.WithWhom,'') as WithWhom,isnull(a.Dsr_Status,'') as Dsr_Status ,isnull(Max(a.logintime),'') as logintime,isnull(Max(a.logouttime),'') as logouttime,isnull(Max(a.EndRemark),'') as EndRemark,a.TPD as ProsDist from (  " +

                    " SELECT View_DSR.SMID,View_DSR.VDate,View_DSR.Level1 AS Level1,View_DSR.SyncId,View_DSR.TotalOrder as TotalOrder,View_DSR.OrderAmountMail as OrderAmountMail, View_DSR.OrderAmountPhone as OrderAmountPhone,View_DSR.DistributorCollection as DistributorCollection,View_DSR.PartyCollection AS PartyCollection,View_DSR.PerCallAvgCell as [PerCallAvgCell],View_DSR.CallsVisited as [CallsVisited],View_DSR.RetailerProCalls as [RetailerProCalls],View_DSR.FailedVisit as [FailedVisit],View_DSR.DistFailVisit as DistFailVisit,View_DSR.DistDiscuss as DistDiscuss,View_DSR.RetailerDiscuss as RetailerDiscuss,View_DSR.NewParties as NewParties,View_DSR.LocalExpenses as [LocalExpenses], View_DSR.TourExpenses as [TourExpenses],View_DSR.Demo as [Demo],View_DSR.Competitor AS Competitor,0 as TotalParty,View_DSR.Remarks as Remarks,View_DSR.cityname,View_DSR.AppRemark as AppRemark,View_DSR.AType as AType,View_DSR.EmpName as EmpName, View_DSR.Type AS Type,View_DSR.Lock1,View_DSR.Lock2,View_DSR.RoleType,(Select Isnull(frtime1,'') from Transvisit where smid=View_DSR.SMID and vdate=View_DSR.VDate) as FirstCall,(Select Isnull(totime1,'') from Transvisit where smid=View_DSR.SMID and vdate=View_DSR.VDate and lock=1) as LastCall,(Select ms.smname from Transvisit tr left join MastSalesRep ms on ms.smid=tr.WithUserId where tr.smid=View_DSR.SMID and tr.vdate=View_DSR.VDate) as WithWhom,(Select Isnull(DSR_Type,'') from Transvisit where smid=View_DSR.SMID and vdate=View_DSR.VDate) as Dsr_Status,(Select isnull(CONVERT(char(5), Mobile_Created_date,108),'') from Transvisit where smid=View_DSR.SMID and vdate=View_DSR.VDate) as logintime,(Select isnull(convert(char(5), Mobile_End_date, 108), '') from Transvisit where smid = View_DSR.SMID and vdate = View_DSR.VDate) as logouttime,(Select isnull(EndRemark, '') from Transvisit where smid = View_DSR.SMID and vdate = View_DSR.VDate) as EndRemark ,(Select convert(char(5), tr.Mobile_Created_date, 108) from Transvisit tr where tr.smid=View_DSR.SMID and tr.vdate=View_DSR.VDate) as Dsr_Time," +

                     "isnull((select STUFF((SELECT ', ' +ANM from(select distinct ma.AreaName as ANM from mastparty mp left join mastarea ma on mp.beatid=ma.areaid where partyid in (select DISTINCT PartyId from transorder where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransCollection where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransCompetitor where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransDemo where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransFailedVisit where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_transorder where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransCollection where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransCompetitor where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransDemo where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransFailedVisit where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT DistId from TransVisitDist where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT DistId from temp_TransVisitDist where smid = View_DSR.smid and vdate = View_DSR.vdate))tbl FOR XML PATH ('')) , 1, 1, '') ),'') AS Beat," +

                     "(Select Count(md.PartyId) from MastProspect_Distributor md left join mastsalesrep msp on msp.userid=md.[Created UserId] where msp.Smid=View_DSR.SMID and CONVERT(DATE, md.Insert_Date)=CONVERT(DATE, View_DSR.VDate)) as TPD FROM View_DSR WHERE View_DSR.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND View_DSR.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead')) " +

                    "UNION ALL select [View_Holiday].smid as SMID,CONVERT (varchar, [View_Holiday].holiday_date,106) as [VisitDate] ,[View_Holiday].smname as [Level1],'' as SyncId,0 as [TotalOrder],0 as OrderAmountMail, 0 as OrderAmountPhone,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited],0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as RetailerDiscuss,0 as [NewParties],0 as [LocalExpenses],0 as [TourExpenses],0 as [Demo],'' AS Competitor,0 as TotalParty, [View_Holiday].Reason as Remarks,'' as Cityname,''  as AppRemark,'' as AType,'' as EmpName,'HOLIDAY' AS Type,0 as lock1,0 as lock2,'' as RoleType,'' as FirstCall,'' as LastCall,'' as WithWhom,'' as Dsr_Status,'' as logintime,'' as logouttime,'' as EndRemark ,'' as Dsr_Time,'' as Beat,0 as TPD from [View_Holiday] where [View_Holiday].holiday_date Between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and [View_Holiday].smid IN (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead'))) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName,a.FirstCall,a.LastCall,a.WithWhom,a.Dsr_Time,a.TPD,a.Dsr_Status Order by a.VDate";
                }
                else if (ddlDsrType.SelectedItem.Text == "Lock")
                {

                    gettotalparty = "(SELECT Count(*) FROM MastParty WHERE BeatId IN ( SELECT Distinct T.BeatId FROM GetSalesDatewiseBeat AS T WHERE T.VDate = a.VDate AND T.SMId= a.SMID) AND active=1 AND partydist=0)";

                    query = "select a.SMID as Id,convert(varchar,a.VDate,106) as [VDate],(case when max(a.RoleType) = 'AreaIncharge' then  a.Level1 + ' - L1' when  max(a.RoleType) in ('CityHead', 'DistrictHead') then a.Level1 + ' - L2' when max(a.RoleType) in ('StateHead', 'RegionHead') then a.Level1 + ' - L3'else a.Level1 end ) as Level1,(a.SyncId) as SyncId,Format(sum(a.TotalOrder),'N2') as TotalOrder,sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],sum(a.DistDiscuss) as DistDiscuss,sum(a.retailerdiscuss) as retailerdiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],sum(a.Competitor) [Competitor],0 as [Collections]," + gettotalparty + " as TotalParty, max(a.Remarks) as Remarks,CASE WHEN max(a.type)='9' THEN max(a.beat) Else '' END  AS beatname,max(a.cityname) as cityname,max(a.AppRemark) as AppRemark11,AType= (case when max(a.atype) is null then 'Pending' WHEN max(a.atype) ='Approve' THEN 'Approved' WHEN max(a.atype) ='Reject' THEN 'Rejected'  else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,a.EmpName, max(a.type) AS type,Max(CASE WHEN Type='9' THEN CASE WHEN ( a.lock1 IS NULL OR a.lock1 = 0 ) THEN 'UnLock' WHEN ( a.lock1 = 1 ) THEN 'Lock' END ELSE  NULL END)  AS lock,max(a.RoleType) as RoleType,a.Dsr_Time,a.FirstCall,a.LastCall,isnull(a.WithWhom,'') as WithWhom,isnull(a.Dsr_Status,'') as Dsr_Status ,isnull(Max(a.logintime),'') as logintime,isnull(Max(a.logouttime),'') as logouttime,isnull(Max(a.EndRemark),'') as EndRemark,a.TPD as ProsDist from (  " +

                       " SELECT View_DSR_Lock.SMID,View_DSR_Lock.VDate,View_DSR_Lock.Level1 AS Level1,View_DSR_Lock.SyncId,View_DSR_Lock.TotalOrder as TotalOrder,View_DSR_Lock.OrderAmountMail as OrderAmountMail, View_DSR_Lock.OrderAmountPhone as OrderAmountPhone,View_DSR_Lock.DistributorCollection as DistributorCollection,View_DSR_Lock.PartyCollection AS PartyCollection,View_DSR_Lock.PerCallAvgCell as [PerCallAvgCell],View_DSR_Lock.CallsVisited as [CallsVisited],View_DSR_Lock.RetailerProCalls as [RetailerProCalls],View_DSR_Lock.FailedVisit as [FailedVisit],View_DSR_Lock.DistFailVisit as DistFailVisit,View_DSR_Lock.DistDiscuss as DistDiscuss,View_DSR_Lock.RetailerDiscuss as RetailerDiscuss,View_DSR_Lock.NewParties as NewParties,View_DSR_Lock.LocalExpenses as [LocalExpenses], View_DSR_Lock.TourExpenses as [TourExpenses],View_DSR_Lock.Demo as [Demo],View_DSR_Lock.Competitor AS Competitor,0 as TotalParty,View_DSR_Lock.Remarks as Remarks,View_DSR_Lock.cityname,View_DSR_Lock.AppRemark as AppRemark,View_DSR_Lock.AType as AType,View_DSR_Lock.EmpName as EmpName, View_DSR_Lock.Type AS Type,View_DSR_Lock.Lock1,View_DSR_Lock.Lock2,View_DSR_Lock.RoleType,(Select Isnull(frtime1,'') from Transvisit where smid=View_DSR_Lock.SMID and vdate=View_DSR_Lock.VDate) as FirstCall,(Select Isnull(totime1,'') from Transvisit where smid=View_DSR_Lock.SMID and vdate=View_DSR_Lock.VDate and lock=1) as LastCall,(Select ms.smname from Transvisit tr left join MastSalesRep ms on ms.smid=tr.WithUserId where tr.smid=View_DSR_Lock.SMID and tr.vdate=View_DSR_Lock.VDate) as WithWhom,(Select Isnull(DSR_Type,'') from Transvisit where smid=View_DSR_Lock.SMID and vdate=View_DSR_Lock.VDate) as Dsr_Status,(Select isnull(CONVERT(char(5), Mobile_Created_date,108),'') from Transvisit where smid=View_DSR_Lock.SMID and vdate=View_DSR_Lock.VDate) as logintime,(Select isnull(convert(char(5), Mobile_End_date, 108), '') from Transvisit where smid = View_DSR_Lock.SMID and vdate = View_DSR_Lock.VDate) as logouttime,(Select isnull(EndRemark, '') from Transvisit where smid = View_DSR_Lock.SMID and vdate = View_DSR_Lock.VDate) as EndRemark ,(Select convert(char(5), tr.Mobile_Created_date, 108) from Transvisit tr where tr.smid=View_DSR_Lock.SMID and tr.vdate=View_DSR_Lock.VDate) as Dsr_Time," +

                     "isnull((select STUFF((SELECT ', ' +ANM from(select distinct ma.AreaName as ANM from mastparty mp left join mastarea ma on mp.beatid=ma.areaid where partyid in (select DISTINCT PartyId from transorder where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransCollection where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransCompetitor where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransDemo where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransFailedVisit where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_transorder where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransCollection where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransCompetitor where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransDemo where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransFailedVisit where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT DistId from TransVisitDist where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT DistId from temp_TransVisitDist where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate))tbl FOR XML PATH ('')) , 1, 1, '') ),'') AS Beat," +

                     "(Select Count(md.PartyId) from MastProspect_Distributor md left join mastsalesrep msp on msp.userid=md.[Created UserId] where msp.Smid=View_DSR_Lock.SMID and CONVERT(DATE, md.Insert_Date)=CONVERT(DATE, View_DSR_Lock.VDate)) as TPD FROM View_DSR_Lock WHERE View_DSR_Lock.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND View_DSR_Lock.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead')) " +

                    "UNION ALL select [View_Holiday].smid as SMID,CONVERT (varchar, [View_Holiday].holiday_date,106) as [VisitDate] ,[View_Holiday].smname as [Level1],'' as SyncId,0 as [TotalOrder],0 as OrderAmountMail, 0 as OrderAmountPhone,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited],0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as RetailerDiscuss,0 as [NewParties],0 as [LocalExpenses],0 as [TourExpenses],0 as [Demo],'' AS Competitor,0 as TotalParty, [View_Holiday].Reason as Remarks,'' as Cityname,''  as AppRemark,'' as AType,'' as EmpName,'HOLIDAY' AS Type,0 as lock1,0 as lock2,'' as RoleType,'' as FirstCall,'' as LastCall,'' as WithWhom,'' as Dsr_Status,'' as logintime,'' as logouttime,'' as EndRemark ,'' as Dsr_Time,'' as Beat,0 as TPD from [View_Holiday] where [View_Holiday].holiday_date Between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and [View_Holiday].smid IN (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead'))) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName,a.FirstCall,a.LastCall,a.WithWhom,a.Dsr_Time,a.TPD,a.Dsr_Status Order by a.VDate";
                }
                else
                {
                    gettotalparty = "(SELECT Count(*) FROM MastParty WHERE BeatId IN ( SELECT Distinct T.BeatId FROM GetSalesDatewiseBeat AS T WHERE T.VDate = a.VDate AND T.SMId= a.SMID) AND active=1 AND partydist=0)";

                    query = "select a.SMID as Id,convert(varchar,a.VDate,106) as [VDate],(case when max(a.RoleType) = 'AreaIncharge' then  a.Level1 + ' - L1' when  max(a.RoleType) in ('CityHead', 'DistrictHead') then a.Level1 + ' - L2' when max(a.RoleType) in ('StateHead', 'RegionHead') then a.Level1 + ' - L3'else a.Level1 end ) as Level1,(a.SyncId) as SyncId,Format(sum(a.TotalOrder),'N2') as TotalOrder,sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],sum(a.DistDiscuss) as DistDiscuss,sum(a.retailerdiscuss) as retailerdiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],sum(a.Competitor) [Competitor],0 as [Collections]," + gettotalparty + " as TotalParty, max(a.Remarks) as Remarks,CASE WHEN max(a.type)='9' THEN max(a.beat) Else '' END  AS beatname,max(a.cityname) as cityname,max(a.AppRemark) as AppRemark11,AType= (case when max(a.atype) is null then 'Pending' WHEN max(a.atype) ='Approve' THEN 'Approved' WHEN max(a.atype) ='Reject' THEN 'Rejected'  else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,a.EmpName, max(a.type) AS type,Max(CASE WHEN Type='9' THEN CASE WHEN ( a.lock1 IS NULL OR a.lock1 = 0 ) THEN 'UnLock' WHEN ( a.lock1 = 1 ) THEN 'Lock' END ELSE  NULL END)  AS lock,max(a.RoleType) as RoleType,a.Dsr_Time,a.FirstCall,a.LastCall,isnull(a.WithWhom,'') as WithWhom,isnull(a.Dsr_Status,'') as Dsr_Status ,isnull(Max(a.logintime),'') as logintime,isnull(Max(a.logouttime),'') as logouttime,isnull(Max(a.EndRemark),'') as EndRemark,a.TPD as ProsDist from (  " +

                         " SELECT View_DSR_UnLock.SMID,View_DSR_UnLock.VDate,View_DSR_UnLock.Level1 AS Level1,View_DSR_UnLock.SyncId,View_DSR_UnLock.TotalOrder as TotalOrder,View_DSR_UnLock.OrderAmountMail as OrderAmountMail, View_DSR_UnLock.OrderAmountPhone as OrderAmountPhone,View_DSR_UnLock.DistributorCollection as DistributorCollection,View_DSR_UnLock.PartyCollection AS PartyCollection,View_DSR_UnLock.PerCallAvgCell as [PerCallAvgCell],View_DSR_UnLock.CallsVisited as [CallsVisited],View_DSR_UnLock.RetailerProCalls as [RetailerProCalls],View_DSR_UnLock.FailedVisit as [FailedVisit],View_DSR_UnLock.DistFailVisit as DistFailVisit,View_DSR_UnLock.DistDiscuss as DistDiscuss,View_DSR_UnLock.RetailerDiscuss as RetailerDiscuss,View_DSR_UnLock.NewParties as NewParties,View_DSR_UnLock.LocalExpenses as [LocalExpenses], View_DSR_UnLock.TourExpenses as [TourExpenses],View_DSR_UnLock.Demo as [Demo],View_DSR_UnLock.Competitor AS Competitor,0 as TotalParty,View_DSR_UnLock.Remarks as Remarks,View_DSR_UnLock.cityname,View_DSR_UnLock.AppRemark as AppRemark,View_DSR_UnLock.AType as AType,View_DSR_UnLock.EmpName as EmpName, View_DSR_UnLock.Type AS Type,View_DSR_UnLock.Lock1,View_DSR_UnLock.Lock2,View_DSR_UnLock.RoleType,(Select Isnull(frtime1,'') from Transvisit where smid=View_DSR_UnLock.SMID and vdate=View_DSR_UnLock.VDate) as FirstCall,(Select Isnull(totime1,'') from Transvisit where smid=View_DSR_UnLock.SMID and vdate=View_DSR_UnLock.VDate and lock=1) as LastCall,(Select ms.smname from Transvisit tr left join MastSalesRep ms on ms.smid=tr.WithUserId where tr.smid=View_DSR_UnLock.SMID and tr.vdate=View_DSR_UnLock.VDate) as WithWhom,(Select Isnull(DSR_Type,'') from Transvisit where smid=View_DSR_UnLock.SMID and vdate=View_DSR_UnLock.VDate) as Dsr_Status,(Select isnull(CONVERT(char(5), Mobile_Created_date,108),'') from Transvisit where smid=View_DSR_UnLock.SMID and vdate=View_DSR_UnLock.VDate) as logintime,(Select isnull(convert(char(5), Mobile_End_date, 108), '') from Transvisit where smid = View_DSR_UnLock.SMID and vdate = View_DSR_UnLock.VDate) as logouttime,(Select isnull(EndRemark, '') from Transvisit where smid = View_DSR_UnLock.SMID and vdate = View_DSR_UnLock.VDate) as EndRemark ,(Select convert(char(5), tr.Mobile_Created_date, 108) from Transvisit tr where tr.smid=View_DSR_UnLock.SMID and tr.vdate=View_DSR_UnLock.VDate) as Dsr_Time," +

                     "isnull((select STUFF((SELECT ', ' +ANM from(select distinct ma.AreaName as ANM from mastparty mp left join mastarea ma on mp.beatid=ma.areaid where partyid in (select DISTINCT PartyId from transorder where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransCollection where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransCompetitor where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransDemo where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransFailedVisit where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_transorder where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransCollection where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransCompetitor where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransDemo where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransFailedVisit where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT DistId from TransVisitDist where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT DistId from temp_TransVisitDist where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate))tbl FOR XML PATH ('')) , 1, 1, '') ),'') AS Beat," +

                     "(Select Count(md.PartyId) from MastProspect_Distributor md left join mastsalesrep msp on msp.userid=md.[Created UserId] where msp.Smid=View_DSR_UnLock.SMID and CONVERT(DATE, md.Insert_Date)=CONVERT(DATE, View_DSR_UnLock.VDate)) as TPD FROM View_DSR_UnLock WHERE View_DSR_UnLock.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND View_DSR_UnLock.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead')) " +

                     "UNION ALL select [View_Holiday].smid as SMID,CONVERT (varchar, [View_Holiday].holiday_date,106) as [VisitDate] ,[View_Holiday].smname as [Level1],'' as SyncId,0 as [TotalOrder],0 as OrderAmountMail, 0 as OrderAmountPhone,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited],0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as RetailerDiscuss,0 as [NewParties],0 as [LocalExpenses],0 as [TourExpenses],0 as [Demo],'' AS Competitor,0 as TotalParty, [View_Holiday].Reason as Remarks,'' as Cityname,''  as AppRemark,'' as AType,'' as EmpName,'HOLIDAY' AS Type,0 as lock1,0 as lock2,'' as RoleType,'' as FirstCall,'' as LastCall,'' as WithWhom,'' as Dsr_Status,'' as logintime,'' as logouttime,'' as EndRemark ,'' as Dsr_Time,'' as Beat,0 as TPD from [View_Holiday] where [View_Holiday].holiday_date Between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and [View_Holiday].smid IN (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead'))) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName,a.FirstCall,a.LastCall,a.WithWhom,a.Dsr_Time,a.TPD,a.Dsr_Status Order by a.VDate";
                }
                DataTable udt = new DataTable();
                // udt.Columns.Add("NewColumn", type(System.Int32));
                udt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                DataView dvSales = new DataView(udt);

                //udt.Columns.Add("NewColumn", typeof(System.String));
                if (udt.Rows.Count > 0)
                {
                    udt.AsEnumerable().Where(r => r.Field<String>("AType") == "Rejected" && r.Field<String>("type") == "8").ToList().ForEach(row => row.Delete());

                    string No_Days = "select cast(NoOfDays as int) NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId IN (" + SPID + ") and (lr.FromDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' OR lr.ToDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59') and lr.appstatus in ('Approve') order by smid,FromDate";
                    DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
                    DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);

                    //foreach (DataRow drvst in dsrData.Rows)
                    //{
                    //    dvSales.RowFilter = "SMId=" + drvst["SMId"].ToString();

                    //    if (dvSales.ToTable().Rows.Count > 0)
                    //    {
                    //        DataTable dtsp = dvSales.ToTable();
                    //        DataRow dr = dtsp.Rows[0];
                    //    }
                    //}

                    udt.AcceptChanges();

                    string date_new = "", date_Old = "";
                    for (int td = 0; td < udt.Rows.Count; td++)
                    {
                        date_Old = udt.Rows[td]["VDate"].ToString();
                        udt.Rows[td]["Dsr_Status"] = udt.Rows[td]["Dsr_Status"].ToString().Trim() == "P" ? "Retailing" : udt.Rows[td]["Dsr_Status"].ToString().Trim() == "W" ? "Week Off" : udt.Rows[td]["Dsr_Status"].ToString().Trim() == "H" ? "Holiday" : "";

                        if (udt.Rows[td]["Dsr_Status"].ToString().Trim() == "")
                        {
                            #region Leave
                            dvdt_NoOfdays.RowFilter = string.Empty;
                            dvdt_NoOfdays.RowFilter = "smid=" + udt.Rows[td]["ID"] + " and (FromDate>= '" + date_Old + "' and FromDate<= '" + date_Old + "') or (NoDays>='" + date_Old + "' and NoDays<='" + date_Old + "')";
                            if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
                            {
                                udt.Rows[td]["Dsr_Status"] = "Leave";
                            }
                            else
                            {
                                udt.Rows[td]["Dsr_Status"] = "Absent";
                            }

                            #endregion
                        }


                        if (date_new != date_Old && udt.Rows[td]["Level1"].ToString() != "" && udt.Rows[td]["Type"].ToString() == Convert.ToInt32(9).ToString())
                        {
                            date_new = udt.Rows[td]["VDate"].ToString();
                            totalworkingDays = totalworkingDays + 1;
                        }

                        //tanvi 31/dec/2020
                        //string tds = "";
                        //if (udt.Rows[td]["Lock"].ToString() == "Lock")
                        //{
                        //    tds = GetDailyWorkingReport(udt.Rows[td]["VDate"].ToString().Replace(' ', '/'), udt.Rows[td]["Id"].ToString());
                        //}
                        //else
                        //{
                        //    tds = GetDailyWorkingReport1(udt.Rows[td]["VDate"].ToString().Replace(' ', '/'), udt.Rows[td]["Id"].ToString());
                        //}

                        //udt.Rows[td]["NewColumn"] = tds;



                    }
                    udt.AcceptChanges();
                    lblTotalWorkingdays.Text = Convert.ToString(totalworkingDays);

                    rpt.DataSource = udt;
                    rpt.DataBind();
                    udt.Select();
                    btnExport.Visible = true;
                }
                else
                {

                    rpt.DataSource = null;
                    rpt.DataBind();
                    lblTotalWorkingdays.Text = "";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        //tanvi 31/dec/2020
        public string GetDailyWorkingReport(string VDate, string smId)
        {
            string result = "";
            string data = string.Empty, beat = "", Query = "", QryDemo = "", QryDistCollection = "", QryPartyCollection = "", QryFv = "", QryComp = "", QryDistFv = "", QryOrder = "", QryDistDisc = "", QryDistStock = "", QryRetailerDisc = "", QryOrder1 = "", QryPartyCollection1 = "", QryDistCollection1 = "", QryDistStock1 = "";
            string QryOtherActivity = "", Query1 = "", QrySample = "", QrySample1 = "", QrySalesReturn1 = "", QrySalesReturn = "";
            DataTable dtLocTraRep = new DataTable();
            try
            {
                if (smId != "")
                {
                    string str = @"select  a.City_Name,a.Beat_Id,a.Description from (
                      select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransOrder  om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                       " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransSample om inner join MastParty p on      p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'    group by b.AreaName,b.AreaId,p.AreaId " +
                                                                                                                                                                                                  " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransSalesReturn om inner join MastParty p on                          p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'                             group by b.AreaName,b.AreaId,p.AreaId " +
                     " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +

                     " union All select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName ,p.AreaId " +

                     " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=0 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId  " +

                     " UNION All select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +

                      " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Transcompetitor om inner join MastParty p on p.PartyId=om.partyid inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and om.vdate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +

                     " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +

                      " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=0 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +

                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from DistributerCollection om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +

                      " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransDistStock om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +

                     " UNION all select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from MastParty p inner join MastArea b on b.AreaId=p.AreaId where p.UserId in (" + smId + ") and p.Created_Date='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName,p.AreaId )a Group by a.City_Name,a.Beat_Id,a.Description Order by a.Description";

                    DataTable dtbeats = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dtbeats.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtbeats.Rows.Count; i++)
                        {
                            beat += dtbeats.Rows[i]["Beat_Id"].ToString() + ",";
                        }
                        beat = beat.TrimStart(',').TrimEnd(',');

                        if (Settings.Instance.OrderEntryType == "1")
                        {

                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.OrderAmount)) as Value,
                            os.Remarks,'' as CompItem,0 as CompQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from TransOrder os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId
                            left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName, os.VDate,os.PartyId,os.Remarks ,p.PartyId,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,mb.areaname";
                        }
                        else
                        {
                            QryOrder1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,
Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end as DescriptionQty,
Isnull(os.MarginPercentage,0) as Margin,ISNULL(os.DiscountType,'') as DiscountType,Isnull(os.DiscountAmount,0) as DiscountAmount,
(sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) -  Isnull(os.DiscountAmount,0)) as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,mb.areaname,os.Discount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.MarginPercentage,os.DiscountType,os.DiscountAmount,os1.OrderTakenType,os1.ExpectedDD";


                            QryOrder = @"select os.OrdId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,
'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by os.OrdId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname,os1.OrderTakenType,os1.ExpectedDD";


                        }

                        QrySample1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransSample1 os LEFT JOIN transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,os.Discount,mb.areaname";


                        QrySample = @"select os.SampleId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os1.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransSample1 os LEFT JOIN transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                       " group by os.SampleId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname";

                        QrySalesReturn1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD  from TransSalesReturn1 os LEFT JOIN TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                         " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,os.Discount,mb.areaname";


                        QrySalesReturn = @"select os.SRetId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os1.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransSalesReturn1 os LEFT JOIN TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                       " group by os.SRetId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Discount,mb.areaname";



                        QryDemo = @"select DemoId AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
                   '' as CompItem,0 as compQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address,d.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId
                   left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate='" + Settings.dateformat(VDate) + "' and d.SMId in (" + smId + ")";

                        QryFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
                   (b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") and pp.PartyDist=0) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryComp = @"select TC.COMPTID, CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,tc.Remarks AS Remarks,
                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,tc.OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId 
                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and tc.VDate='" + Settings.dateformat(VDate) + "' and tc.SMId in (" + smId + ")";

                        QryPartyCollection1 = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=1 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,mb.areaname ";

                        QryPartyCollection = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,max(tc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=1 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Latitude,tc.Longitude,tc.Address,mb.areaname";

                        QryRetailerDisc = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Retailer Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD  from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=0) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";


                        QryDistCollection1 = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,Dc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=1 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,dc.Amount,Dc.Latitude,Dc.Longitude,Dc.Address,DC.Mobile_Created_date ";

                        QryDistCollection = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,max(Dc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=1 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,Dc.Latitude,Dc.Longitude,Dc.Address";

                        QryDistFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistDisc = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistStock1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                   " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl ";

                        QryDistStock = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,max(os.ImgUrl) as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                 " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";


                        string qw = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as OrderTakenType,'' as ExpectedDD FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
               " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl ";

                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,'' as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                 CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD  from TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL";

                        Query1 = @"select *,'' as Match from (select * from (" + QryOrder1 + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection1 + " union all " + QryDistCollection1 + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock1 + " union all " + QryOtherActivity + " union all " + QrySample1 + " union all " + QrySalesReturn1 + " ) a )b Order by b.Mobile_Created_date";

                        Query = @"select *,'' as Match from (select * from (" + QryOrder + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection + " union all " + QryDistCollection + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock + " union all " + QryOtherActivity + " union all " + QrySample + " union all " + QrySalesReturn + " ) a )b Order by b.Mobile_Created_date";

                        dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        Double totaldistance = 0;
                        if (dtLocTraRep.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtLocTraRep.Rows.Count; i++)
                            {
                                if (i != dtLocTraRep.Rows.Count - 1)
                                {
                                    if (!string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Longitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()))
                                    {
                                        if ((Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()) != 0))
                                        {
                                            Double abc = Calculate(Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()));
                                            totaldistance = totaldistance + Math.Round(abc * 1609 / 1000, 3);
                                        }
                                    }
                                }


                            }

                            result = totaldistance.ToString();
                            string qry = @"update TransVisit set TotalDistance=" + Convert.ToDouble(totaldistance) + " where SMId=" + smId + " and VDate='" + Settings.dateformat(VDate) + "'";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, qry);
                            //rpt.DataSource = dtLocTraRep;
                            //rpt.DataBind();
                            //lblTotalDistance.Text = totaldistance.ToString();
                        }
                        else
                        {
                            // rpt.DataSource = null;
                            // rpt.DataBind();
                            // lblTotalDistance.Text = totaldistance.ToString();
                        }
                    }
                    else
                    {
                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,'' as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                        '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                        CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                         '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                         and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL";

                        Query = @"select *,'' as Match from (select * from (" + QryOtherActivity + " ) a )b Order by b.Mobile_Created_date";
                        Query1 = @"select *,'' as Match from (select * from (" + QryOtherActivity + " ) a )b Order by b.Mobile_Created_date";
                        dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        Double totaldistance = 0;
                        if (dtLocTraRep.Rows.Count > 0)
                        {

                            for (int i = 0; i < dtLocTraRep.Rows.Count - 1; i++)
                            {
                                if (i != dtLocTraRep.Rows.Count - 1)
                                {
                                    if (!string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Longitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()))
                                    {
                                        if ((Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()) != 0))
                                        {
                                            Double abc = Calculate(Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()));
                                            totaldistance = totaldistance + Math.Round(abc * 1609 / 1000, 3);
                                        }
                                    }
                                }

                            }
                            // rpt.DataSource = dtLocTraRep;
                            // rpt.DataBind();
                            // lblTotalDistance.Text = totaldistance.ToString();
                            result = totaldistance.ToString();
                            string qry = @"update TransVisit set TotalDistance=" + Convert.ToDouble(totaldistance) + " where SMId=" + smId + " and VDate='" + Settings.dateformat(VDate) + "'";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, qry);
                        }
                        else
                        {
                            // rpt.DataSource = null;
                            //rpt.DataBind();
                            // lblTotalDistance.Text = totaldistance.ToString();
                        }
                    }
                    Session["Query"] = Query1;
                    Session["MainQuery"] = Query;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }
        //tanvi 31/dec/2020
        public static double Calculate(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
            var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
            var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
            var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
        }
        //tanvi 31/dec/2020
        public string GetDailyWorkingReport1(string VDate, string smId)
        {
            string result = "";
            string data = string.Empty, beat = "", Query = "", QryDemo = "", QryDistCollection = "", QryPartyCollection = "", QryFv = "", QryComp = "", QryDistFv = "", QryOrder = "", QryDistDisc = "", QryDistStock = "", QryRetailerDisc = "",
           QryOrder1 = "", QryPartyCollection1 = "", QryDistCollection1 = "", QryDistStock1 = "";
            string QryOtherActivity = "", Query1 = "", QrySample = "", QrySample1 = "", QrySalesReturn = "", QrySalesReturn1 = "";
            DataTable dtLocTraRep = new DataTable();
            try
            {
                if (smId != "")
                {
                    string str = @"select  a.City_Name,a.Beat_Id,a.Description from (
                      select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransOrder  om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                                                                                                                                                                                                  " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransSample om inner join MastParty p on                               p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'                             group by b.AreaName,b.AreaId,p.AreaId " +
                                                                                                                                                                                                  " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransSalesReturn om inner join MastParty p on                               p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'                             group by b.AreaName,b.AreaId,p.AreaId " +
                     " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                     " union All select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName ,p.AreaId " +
                     " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=0 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId  " +
                     " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_Transcompetitor om inner join MastParty p on p.PartyId=om.partyid inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and om.vdate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                     " UNION All select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                     " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=0 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +


                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from DistributerCollection om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransDistStock om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransDistStock om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
                     " UNION all select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from MastParty p inner join MastArea b on b.AreaId=p.AreaId where p.UserId in (" + smId + ") and p.Created_Date='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName,p.AreaId )a Group by a.City_Name,a.Beat_Id,a.Description Order by a.Description";

                    DataTable dtbeats = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dtbeats.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtbeats.Rows.Count; i++)
                        {
                            beat += dtbeats.Rows[i]["Beat_Id"].ToString() + ",";
                        }
                        beat = beat.TrimStart(',').TrimEnd(',');

                        if (Settings.Instance.OrderEntryType == "1")
                        {
                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.OrderAmount)) as Value,
                   os.Remarks,'' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from Temp_TransOrder os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId 
                   left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os.Remarks ,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date ";
                        }
                        else
                        {
                            QryOrder1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,
Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end as DescriptionQty,
Isnull(os.MarginPercentage,0) as Margin,ISNULL(os.DiscountType,'') as DiscountType,Isnull(os.DiscountAmount,0) as DiscountAmount,
(sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) -  Isnull(os.DiscountAmount,0)) as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.OrdId=os1.OrdId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                          " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os1.ImgUrl,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,mb.areaname,os.Discount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.MarginPercentage,os.DiscountType,os.DiscountAmount,os1.OrderTakenType,os1.ExpectedDD";

                            QryOrder = @"select os.OrdId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                          " group by os.OrdId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname,os1.OrderTakenType,os1.ExpectedDD";

                        }


                        QrySample1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSample1 os LEFT JOIN Temp_transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                         " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os1.ImgUrl,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,mb.areaname";

                        QrySample = @"select os.SampleId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSample1 os LEFT JOIN Temp_transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                      " group by os.SampleId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname";



                        QrySalesReturn1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSalesReturn1 os LEFT JOIN Temp_TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                        " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os1.ImgUrl,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,os.Discount,mb.areaname";

                        QrySalesReturn = @"select os.SRetId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os1.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSalesReturn1 os LEFT JOIN Temp_TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                      " group by os.SRetId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,mb.areaname";

                        QryDemo = @"select DemoId AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
                   '' as CompItem,0 as compQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as  Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address,d.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate='" + Settings.dateformat(VDate) + "' and d.SMId in (" + smId + ")";

                        QryFv = @"select FVId AS COMPTID,CONVERT (varchar,fv.VDate,106) as VisitDate,'Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
                   (b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") and pp.PartyDist=0) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryComp = @"select TC.COMPTID,CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,tc.Remarks AS Remarks,
                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,OtherActivity=(Case when tc.OtherActivity=1 then 'Yes' else 'No' end), 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId
                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and tc.VDate='" + Settings.dateformat(VDate) + "' and tc.SMId in (" + smId + ")";


                        QryPartyCollection1 = @" SELECT '' AS COMPTID,convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile AS partyname,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=0 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,mb.areaname";

                        QryPartyCollection = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,max(tc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=0 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Latitude,tc.Longitude,tc.Address,mb.areaname";

                        QryRetailerDisc = @"select VisDistId AS COMPTID,CONVERT (varchar,tv.VDate,106) as VisitDate,'Retailer Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity,tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=0) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistCollection1 = @" SELECT '' AS COMPTID,convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile AS partyname,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,Dc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=0 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,dc.Amount,Dc.Latitude,Dc.Longitude,Dc.Address,Dc.Mobile_Created_date";

                        QryDistCollection = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,max(Dc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=0 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,Dc.Latitude,Dc.Longitude,Dc.Address";


                        QryDistFv = @"select FVId AS COMPTID,CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistDisc = @"select VisDistId AS COMPTID,CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity,tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistStock1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                  " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl ";

                        QryDistStock = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,max(os.ImgUrl) as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                 " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";

                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,'' as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                 CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL ";

                        Query1 = @"select *,'' as Match from (select * from (" + QryOrder1 + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection1 + " union all " + QryDistCollection1 + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock1 + " union all " + QryOtherActivity + " union all " + QrySample1 + " union all " + QrySalesReturn1 + ") a ) b Order by b.Mobile_Created_date";

                        Query = @"select *,'' as Match from (select * from (" + QryOrder + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection + " union all " + QryDistCollection + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock + " union all " + QryOtherActivity + " union all " + QrySample + " union all " + QrySalesReturn + ") a ) b Order by b.Mobile_Created_date";


                        dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        Double totaldistance = 0;
                        if (dtLocTraRep.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtLocTraRep.Rows.Count - 1; i++)
                            {
                                if (i != dtLocTraRep.Rows.Count - 1)
                                {
                                    if (!string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Longitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()))
                                    {
                                        if ((Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()) != 0))
                                        {
                                            Double abc = Calculate(Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()));
                                            totaldistance = totaldistance + Math.Round(abc * 1609 / 1000, 3);
                                        }
                                    }
                                }

                            }
                            result = totaldistance.ToString();
                        }
                        else
                        {
                            //rpt.DataSource = null;
                            //rpt.DataBind();
                            //lblTotalDistance.Text = totaldistance.ToString();
                        }
                    }
                    else
                    {
                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,mp.partyname as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                        '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                        CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                         '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                         and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId  left join mastparty mp on mp.partyid=tv.distid  where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL";

                        Query = @"select *,'' as Match from (select * from (" + QryOtherActivity + " ) a )b Order by b.Mobile_Created_date";
                        Query1 = @"select *,'' as Match from (select * from (" + QryOtherActivity + " ) a )b Order by b.Mobile_Created_date";
                        dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                        Double totaldistance = 0;
                        if (dtLocTraRep.Rows.Count > 0)
                        {

                            for (int i = 0; i < dtLocTraRep.Rows.Count - 1; i++)
                            {
                                if (i != dtLocTraRep.Rows.Count - 1)
                                {
                                    if (!string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i]["Longitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) && !string.IsNullOrEmpty(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()))
                                    {
                                        if ((Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()) != 0) && (Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()) != 0))
                                        {
                                            Double abc = Calculate(Convert.ToDouble(dtLocTraRep.Rows[i]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i]["Longitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Latitude"].ToString()), Convert.ToDouble(dtLocTraRep.Rows[i + 1]["Longitude"].ToString()));
                                            totaldistance = totaldistance + Math.Round(abc * 1609 / 1000, 3);
                                        }
                                    }
                                }

                            }

                            result = totaldistance.ToString();
                            //rpt.DataSource = dtLocTraRep;
                            //rpt.DataBind();
                            //lblTotalDistance.Text = totaldistance.ToString();
                        }
                        else
                        {
                            //rpt.DataSource = null;
                            //rpt.DataBind();
                            //lblTotalDistance.Text = totaldistance.ToString();
                        }
                    }
                    Session["Query"] = Query1;
                    Session["MainQuery"] = Query;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }

        decimal totalsale = 0M;
        decimal totalDistCollection = 0M;
        decimal totalPartyCollection = 0M;
        decimal totalParty = 0M;
        decimal totalPerCallAvgSale = 0M;
        decimal totalcallvisited = 0M;
        decimal totalRetailerCalls = 0M;
        decimal totalEndUserCalls = 0M;
        decimal totalRetailerProCalls = 0M;
        decimal totalEndUserProCalls = 0M;
        decimal totalNewParties = 0M;
        decimal totalCollections = 0M;
        decimal totalLocalExpenses = 0M;
        decimal totalTourExpenses = 0M;
        decimal totalFailedVisit = 0M;
        decimal totalComp = 0M;
        decimal totalDemo = 0M;

        decimal totalDistFV = 0M;
        decimal totalDistDiscuss = 0M;


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDailyWorkingSummaryVw_V2.aspx", true);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            rptmain.Style.Add("display", "block");
            string smIDStr = "";
            string smIDStr1 = "", userIdStr = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr += node.Value + ",";
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (smIDStr1 == "")
            {
                DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dtSMId);
                dv.RowFilter = "RoleType in ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead') and SMName<>'.'";

                if (dv.ToTable().Rows.Count > 0)
                {
                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                }
            }

            //if (smIDStr1 != "")
            //{
                //string salesRepUserIdQry = @"select UserId from MastSalesRep where SMId in (" + smIDStr1 + ")";
                string salesRepUserIdQry = @"select UserId from MastSalesRep where SMId in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) AND mr.RoleType IN ('AreaIncharge','CityHead','DistrictHead','StateHead','RegionHead'))";
                DataTable userdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepUserIdQry);
                if (userdt.Rows.Count > 0)
                {
                    for (int i = 0; i < userdt.Rows.Count; i++)
                    {
                        userIdStr += userdt.Rows[i]["UserId"] + ",";
                    }
                }
                userIdStr = userIdStr.TrimStart(',').TrimEnd(',');
                //

                if (frmTextBox.Text != string.Empty && toTextBox.Text != string.Empty)
                {
                    if (Convert.ToDateTime(frmTextBox.Text) <= Convert.ToDateTime(toTextBox.Text))
                    {
                        GetDailyWorkingSummaryL1(smIDStr1, frmTextBox.Text, toTextBox.Text, userIdStr, "0");
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
                        rptmain.Style.Add("display", "none");
                    }
                }
                else
                {
                    rptmain.Style.Add("display", "none");
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('Please fill dates');", true);
                }
            //}
            //else
            //{
            //    rptmain.Style.Add("display", "none");
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
            //}
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DailyWorkingSummary.csv");
            string headertxt = "Visit Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Type".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"') + "," + "DSR Type".TrimStart('"').TrimEnd('"') + "," + "DSR Status".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Total Retailer".TrimStart('"').TrimEnd('"') + "," + "Retailer Visited".TrimStart('"').TrimEnd('"') + "," + "Retailer Productive Calls".TrimStart('"').TrimEnd('"') + "," + "Total Order".TrimStart('"').TrimEnd('"') + "," + "Demo".TrimStart('"').TrimEnd('"') + "," + "Non-Productive".TrimStart('"').TrimEnd('"') + "," + "Competitor".TrimStart('"').TrimEnd('"') + "," + "Retai. Discuss".TrimStart('"').TrimEnd('"') + "," + "Retailer Collection".TrimStart('"').TrimEnd('"') + "," + "New Retailer".TrimStart('"').TrimEnd('"') + "," + "Retailer Per Call Avg Sale".TrimStart('"').TrimEnd('"') + "," + "Claim Exp.".TrimStart('"').TrimEnd('"') + "," + "Approved Exp.".TrimStart('"').TrimEnd('"') + "," + "Dist. Discuss".TrimStart('"').TrimEnd('"') + "," + "Dist. Non-Productive".TrimStart('"').TrimEnd('"') + "," + "Dist. Collection".TrimStart('"').TrimEnd('"') + "," + "Prospect Distributor".TrimStart('"').TrimEnd('"') + "," + "Start DSR Time".TrimStart('"').TrimEnd('"') + "," + "First Call".TrimStart('"').TrimEnd('"') + "," + "Last Call".TrimStart('"').TrimEnd('"') + "," + "Approved Remarks".TrimStart('"').TrimEnd('"') + "," + "With Whom".TrimStart('"').TrimEnd('"') + "," + "Login Time".TrimStart('"').TrimEnd('"') + "," + "Logout Time".TrimStart('"').TrimEnd('"') + "," + "End Visit Remark".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertxt);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Vdate", typeof(DateTime)));
            dtParams.Columns.Add(new DataColumn("level", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("empName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Type", typeof(String)));
            dtParams.Columns.Add(new DataColumn("remarks", typeof(String)));
            dtParams.Columns.Add(new DataColumn("atype", typeof(String)));
            dtParams.Columns.Add(new DataColumn("lock", typeof(String)));
            dtParams.Columns.Add(new DataColumn("dsrstat", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CityName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("BeatName", typeof(String)));

            dtParams.Columns.Add(new DataColumn("totalParty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CallsVisited", typeof(String)));
            dtParams.Columns.Add(new DataColumn("RetailerProCalls", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));
            dtParams.Columns.Add(new DataColumn("demo", typeof(String)));
            dtParams.Columns.Add(new DataColumn("failvist", typeof(String)));
            dtParams.Columns.Add(new DataColumn("competitor", typeof(String)));
            dtParams.Columns.Add(new DataColumn("retailerdiscuss", typeof(String)));
            dtParams.Columns.Add(new DataColumn("partyCollection", typeof(String)));
            dtParams.Columns.Add(new DataColumn("newParty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("percallSale", typeof(String)));
            dtParams.Columns.Add(new DataColumn("localExpense", typeof(String)));
            dtParams.Columns.Add(new DataColumn("tourExpense", typeof(String)));

            dtParams.Columns.Add(new DataColumn("distDiscuss", typeof(String)));

            dtParams.Columns.Add(new DataColumn("distFailVisit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("distCollection", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProsDist", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DsrTime", typeof(String)));
            dtParams.Columns.Add(new DataColumn("FirstCall", typeof(String)));
            dtParams.Columns.Add(new DataColumn("LastCall", typeof(String)));




            //dtParams.Columns.Add(new DataColumn("OrderMail", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("OrderPhone", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("syncId", typeof(String)));

            dtParams.Columns.Add(new DataColumn("appRemark", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("TotalDistanceTravelled", typeof(String)));
            dtParams.Columns.Add(new DataColumn("wthwhm", typeof(String)));
            dtParams.Columns.Add(new DataColumn("lgntm", typeof(String)));
            dtParams.Columns.Add(new DataColumn("lgottm", typeof(String)));
            dtParams.Columns.Add(new DataColumn("endrem", typeof(String)));

            foreach (RepeaterItem item in rpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label dateLabel = item.FindControl("dateLabel") as Label;
                dr["Vdate"] = Convert.ToDateTime(dateLabel.Text).ToShortDateString();
                Label levelLabel = item.FindControl("levelLabel") as Label;
                dr["level"] = levelLabel.Text.ToString();
                //Label empNameLabel = item.FindControl("empNameLabel") as Label;
                //dr["empName"] = empNameLabel.Text.ToString(); 
                Label lbltypeLabel = item.FindControl("lblType") as Label;
                dr["Type"] = lbltypeLabel.Text.ToString();

                Label remarksLabel = item.FindControl("lblremarks") as Label;
                dr["remarks"] = remarksLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label atypeLabel = item.FindControl("atypeLabel") as Label;
                dr["atype"] = atypeLabel.Text.ToString();
                Label lockLabel = item.FindControl("lockLabel") as Label;
                dr["lock"] = lockLabel.Text.ToString();

                Label dsrstats = item.FindControl("lbldsrstat") as Label;
                dr["dsrstat"] = dsrstats.Text.ToString();

                Label Citynamelabel = item.FindControl("lblCityname") as Label;
                dr["CityName"] = Citynamelabel.Text.ToString();
                Label Beatnamelabel = item.FindControl("lblbeat") as Label;
                dr["BeatName"] = Beatnamelabel.Text.ToString();
                Label totalPartyLabel = item.FindControl("totalPartyLabel") as Label;
                dr["totalParty"] = totalPartyLabel.Text.ToString();
                Label lblCallsVisited = item.FindControl("lblCallsVisited") as Label;
                dr["CallsVisited"] = lblCallsVisited.Text.ToString();
                Label lblRetailerProCalls = item.FindControl("lblRetailerProCalls") as Label;
                dr["RetailerProCalls"] = lblRetailerProCalls.Text.ToString();
                Label distDiscussLabel = item.FindControl("distDiscussLabel") as Label;
                dr["distDiscuss"] = distDiscussLabel.Text.ToString();
                Label retailerdiscuss = item.FindControl("retailerdiscuss") as Label;
                dr["retailerdiscuss"] = retailerdiscuss.Text.ToString();
                Label distFailVisitLabel = item.FindControl("distFailVisitLabel") as Label;
                dr["distFailVisit"] = distFailVisitLabel.Text.ToString();
                Label distCollectionLabel = item.FindControl("distCollectionLabel") as Label;
                dr["distCollection"] = distCollectionLabel.Text.ToString();
                Label lblTotalOrder = item.FindControl("lblTotalOrder") as Label;
                dr["TotalOrder"] = lblTotalOrder.Text.ToString();
                //Label lblOrderMail = item.FindControl("lblOrderMail") as Label;
                //dr["OrderMail"] = lblOrderMail.Text.ToString(); 

                //Label lblOrderPhone = item.FindControl("lblOrderPhone") as Label;
                //dr["OrderPhone"] = lblOrderPhone.Text.ToString(); 
                Label demoLabel = item.FindControl("demoLabel") as Label;
                dr["demo"] = demoLabel.Text.ToString();
                Label failvistLabel = item.FindControl("failvistLabel") as Label;
                dr["failvist"] = failvistLabel.Text.ToString();
                Label competitorLabel = item.FindControl("competitorLabel") as Label;
                dr["competitor"] = competitorLabel.Text.ToString();

                Label partyCollectionLabel = item.FindControl("partyCollectionLabel") as Label;
                dr["partyCollection"] = partyCollectionLabel.Text.ToString();
                Label percallSaleLabel = item.FindControl("percallSaleLabel") as Label;
                dr["percallSale"] = percallSaleLabel.Text.ToString();
                Label newPartyLabel = item.FindControl("newPartyLabel") as Label;
                dr["newParty"] = newPartyLabel.Text.ToString();
                Label localExpenseLabel = item.FindControl("localExpenseLabel") as Label;
                dr["localExpense"] = localExpenseLabel.Text.ToString();

                Label tourExpenseLabel = item.FindControl("tourExpenseLabel") as Label;
                dr["tourExpense"] = tourExpenseLabel.Text.ToString();
                Label prosdistLabel = item.FindControl("lblpros") as Label;
                dr["ProsDist"] = prosdistLabel.Text.ToString();
                Label lblDsrTme = item.FindControl("lbldsrtme") as Label;
                dr["DsrTime"] = lblDsrTme.Text.ToString();
                //Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                //dr["syncId"] = syncIdLabel.Text.ToString(); 
                Label lblFirstCall = item.FindControl("lblFirstCall") as Label;
                dr["FirstCall"] = lblFirstCall.Text.ToString();

                Label lblLastCall = item.FindControl("lblLastCall") as Label;
                dr["LastCall"] = lblLastCall.Text.ToString();



                Label appRemarkLabel = item.FindControl("appRemarkLabel") as Label;
                dr["appRemark"] = appRemarkLabel.Text.ToString().Replace("\n", "").Replace("\r", ""); ;


                //Label TotalDistanceTravelled = item.FindControl("lblNewColumn") as Label;
                //dr["TotalDistanceTravelled"] = TotalDistanceTravelled.Text.ToString();

                Label withwhom = item.FindControl("lblWithWhom") as Label;
                dr["wthwhm"] = withwhom.Text.ToString();

                Label lgntm = item.FindControl("lgntme") as Label;
                dr["lgntm"] = lgntm.Text.ToString();

                Label lgotm = item.FindControl("lgottme") as Label;
                dr["lgottm"] = lgotm.Text.ToString();

                Label endrem = item.FindControl("endrm") as Label;
                dr["endrem"] = endrem.Text.ToString();

                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "VDate desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[25];
            try
            {
                for (int j = 0; j < udtNew.Rows.Count; j++)
                {
                    for (int k = 0; k < udtNew.Columns.Count; k++)
                    {

                        if (udtNew.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 0)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                                //Total For Columns
                                if (k == 9 || k == 10 || k == 11 || k == 12 || k == 13 || k == 14 || k == 15 || k == 16 || k == 17 || k == 18 || k == 19 || k == 20 || k == 21 || k == 22 || k == 23 || k == 24)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                                //End
                            }

                        }
                        else if (udtNew.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                                //Total For Columns
                                if (k == 9 || k == 10 || k == 11 || k == 12 || k == 13 || k == 14 || k == 15 || k == 16 || k == 17 || k == 18 || k == 19 || k == 20 || k == 21 || k == 22 || k == 23 || k == 24)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                                //End
                            }

                        }
                        else
                        {
                            //if (k == 3)
                            //{
                            //    if (udtNew.Rows[j][3].ToString() == "9")
                            //    {
                            //        sb.Append("DSR" + ',');
                            //    }
                            //    else if (udtNew.Rows[j][3].ToString() == "8")
                            //    {
                            //        sb.Append("LEAVE" + ',');
                            //    }
                            //    else if (udtNew.Rows[j][3].ToString() == "7")
                            //    {
                            //        sb.Append("EXPENSE" + ',');
                            //    }
                            //    else
                            //    {
                            //        sb.Append("HOLIDAY" + ',');
                            //    }
                            //}
                            //else
                            //{
                            if (k == 0)
                            {
                                sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            }
                            else
                            {
                                sb.Append(udtNew.Rows[j][k].ToString() + ',');
                                //Total For Columns
                                if (k == 9 || k == 10 || k == 11 || k == 12 || k == 13 || k == 14 || k == 15 || k == 16 || k == 17 || k == 18 || k == 19 || k == 20 || k == 21 || k == 22 || k == 23 || k == 24)
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
                for (int x = 9; x < totalVal.Count(); x++)
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
                sb.Append(",,,,,,,,Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        //Added By- Abhishek 01-06-2016
        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }
        //End

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {


                    HiddenField hdnType = (HiddenField)e.Item.FindControl("hdnType");
                    Label lblType = (Label)e.Item.FindControl("lblType");

                    if (hdnType.Value == "9")
                    {
                        lblType.Text = "DSR";
                    }
                    else if (hdnType.Value == "8")
                    {
                        lblType.Text = "LEAVE";
                    }
                    else if (hdnType.Value == "7")
                    {
                        lblType.Text = "EXPENSE";
                    }
                    else
                    {
                        lblType.Text = "HOLIDAY";
                    }

                    Label lblTotalSale = (Label)e.Item.FindControl("lblTotalOrder");
                    //Label lblTotalPartyCollection = (Label)e.Item.FindControl("lblTotalpartyColl");
                    Label lblPerCallAvgSale = (Label)e.Item.FindControl("lblPerCallAvg");
                    Label lblCallVisited = (Label)e.Item.FindControl("lblCallsVisited");



                    decimal sale = Decimal.Parse(lblTotalSale.Text);
                    totalsale += sale;
                    decimal Callvisited = Decimal.Parse(lblCallVisited.Text);
                    totalcallvisited += Callvisited;
                    //         Label lblTotalCallAvgSal = (Label)e.Item.FindControl("lblTotalCallAvgSal");
                    if (Callvisited != 0)
                    {
                        //decimal PerCallAvgSale = decimal.Parse((sale / Callvisited).ToString()); //Decimal.Parse(lblPerCallAvgSale.Text);
                        //totalPerCallAvgSale += PerCallAvgSale;
                        //lblPerCallAvgSale.Text = PerCallAvgSale.ToString("#.##");
                        //lblTotalCallAvgSal.Text = PerCallAvgSale.ToString("#.##");
                    }

                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hdnType = (HiddenField)e.Item.FindControl("hdnType");
                    Label lblType = (Label)e.Item.FindControl("lblType");

                    if (hdnType.Value == "9")
                    {
                        lblType.Text = "DSR";
                    }
                    else if (hdnType.Value == "8")
                    {
                        lblType.Text = "LEAVE";
                    }
                    else if (hdnType.Value == "7")
                    {
                        lblType.Text = "EXPENSE";
                    }
                    else
                    {
                        lblType.Text = "HOLIDAY";
                    }

                    Label lblTotalSale = (Label)e.Item.FindControl("lblTotalOrder");
                    //Label lblTotalPartyCollection = (Label)e.Item.FindControl("lblTotalpartyColl");
                    Label lblPerCallAvgSale = (Label)e.Item.FindControl("lblPerCallAvg");
                    Label lblCallVisited = (Label)e.Item.FindControl("lblCallsVisited");

                    decimal sale = Decimal.Parse(lblTotalSale.Text);
                    totalsale += sale;
                    decimal Callvisited = Decimal.Parse(lblCallVisited.Text);
                    totalcallvisited += Callvisited;
                    //         Label lblTotalCallAvgSal = (Label)e.Item.FindControl("lblTotalCallAvgSal");
                    if (Callvisited != 0)
                    {
                        //decimal PerCallAvgSale = decimal.Parse((sale / Callvisited).ToString()); //Decimal.Parse(lblPerCallAvgSale.Text);
                        //totalPerCallAvgSale += PerCallAvgSale;
                        //lblPerCallAvgSale.Text = PerCallAvgSale.ToString("#.##");
                        //lblTotalCallAvgSal.Text = PerCallAvgSale.ToString("#.##");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                HiddenField hdnDate = (HiddenField)e.Item.FindControl("hdnDate");
                HiddenField hdnSmiD = (HiddenField)e.Item.FindControl("hdnSMId");
                HiddenField hdnDsrType = (HiddenField)e.Item.FindControl("hdnDsrType");
                HiddenField hdncityname = (HiddenField)e.Item.FindControl("hdncityname");
                //string status = ddlDsrType.SelectedItem.Value;
                //Response.Redirect("DSRReport.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value + "&Recstatus=" + status);
                Response.Redirect("DSRReport.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value + "&Recstatus=" + hdnDsrType.Value);
            }
        }
        public string EncryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        //public DataTable BindDSRDetail()
        //{
        //    #region Variable Declaration
        //    string str = "", QryChk = "";
        //    string status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;
        //    double TotalCall = 0, PCall = 0; double productivity = 0.00;
        //    DataTable gvdt = new DataTable();
        //    DataTable dtOrder = new DataTable();
        //    #endregion

        //    if (ddlDsrType.SelectedItem.Text == "Lock")
        //    {
        //        QryChk = " and isnull(tv.lock,0)=1";
        //    }
        //    if (ddlDsrType.SelectedItem.Text == "UnLock")
        //    {
        //        QryChk = " and isnull(tv.lock,0)=0";
        //    }

        //    //if (ddlType.SelectedValue != "3")
        //    //{
        //    //    if (QryChk == "")
        //    //    {
        //    //        QryChk += "where a.Type='" + ddlType.SelectedValue + "'";
        //    //    }
        //    //    else
        //    //    {
        //    //        QryChk += " and a.Type='" + ddlType.SelectedValue + "'";
        //    //    }
        //    //}

        //    if (ddlStatus.SelectedValue != "3")
        //    {
        //        if (QryChk == "")
        //        {
        //            QryChk += " and ISNULL(tv.AppStatus,'Pending')='" + ddlStatus.SelectedValue + "'";
        //        }
        //        else
        //        {
        //            QryChk += " and ISNULL(tv.AppStatus,'Pending')'" + ddlStatus.SelectedValue + "'";
        //        }
        //    }

        //    try
        //    {
        //        string smIDStr = "", smIDStr1 = "";
        //        foreach (TreeNode node in trview.CheckedNodes)
        //        {
        //            smIDStr1 = node.Value;
        //            {
        //                smIDStr += node.Value + ",";
        //            }
        //        }
        //        smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

        //        if (smIDStr1 == "")
        //        {
        //            DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
        //            DataView dv = new DataView(dtSMId);
        //            dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";

        //            if (dv.ToTable().Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dv.ToTable().Rows)
        //                {
        //                    smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
        //                }
        //                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
        //            }
        //        }

        //        string mDate1 = Settings.dateformat(frmTextBox.Text);
        //        string mDate2 = Settings.dateformat(toTextBox.Text);

        //        gvdt.Columns.Add(new DataColumn("Date", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Reporting Manager", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("SMName", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Region", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("City", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("SMStatus", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Designation", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("HeadQuater", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("DSR Status", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("DSR Type", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Login", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("LogOut", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("LogOut Reason", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("First Call", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Last Call", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("With Whom", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Remark", typeof(String)));

        //        gvdt.Columns.Add(new DataColumn("Type", typeof(String)));


        //        gvdt.Columns.Add(new DataColumn("New Outlet", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Outlet Visited", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Productive Call", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Total Order", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Demo", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("No Productive", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Competitor", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Ret. Discuss", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Ret. Collection", typeof(String)));

        //        gvdt.Columns.Add(new DataColumn("Avg. Call Sale", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Claim Exp.", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Approved Exp.", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Dist. Discuss", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Dist. Non Productive", typeof(String)));
        //        gvdt.Columns.Add(new DataColumn("Dist. Collection", typeof(String)));



        //        gvdt.Columns.Add(new DataColumn("Approved Remark", typeof(String)));



        //        //gvdt.Columns.Add(new DataColumn("Assigned Beat", typeof(String)));
        //        //gvdt.Columns.Add(new DataColumn("Selected Beat", typeof(String)));
        //        //gvdt.Columns.Add(new DataColumn("Total Call", typeof(String)));

        //        //gvdt.Columns.Add(new DataColumn("Productivity", typeof(String)));
        //        //gvdt.Columns.Add(new DataColumn("Net Value", typeof(String)));

        //        //gvdt.Columns.Add(new DataColumn("Total Time", typeof(String)));

        //        //gvdt.Columns.Add(new DataColumn("Total Retail Time", typeof(String)));
        //        //gvdt.Columns.Add(new DataColumn("Total Call Time (hh:mm)", typeof(String)));
        //        //gvdt.Columns.Add(new DataColumn("First Activity Time", typeof(String)));
        //        //gvdt.Columns.Add(new DataColumn("Last Activity Time", typeof(String)));
        //        //gvdt.Columns.Add(new DataColumn("Activity Time Spent(HH:MM)", typeof(String)));




        //        DataRow pDataRow = gvdt.NewRow();
        //        string mSRepCode = "", strTour = String.Empty;

        //        mSRepCode = @"select  sr.smname as ReportingManager,emp.SMId,emp.SMName,emp.SyncId,emp.Mobile as ContactNo,case when isnull(emp.Active,0)=1 then 'Active' else 'Deactive' end SMStatus,mdst.DesName as Designation,isnull(hq.HeadquarterName,'')HeadQuater,mr.areaname as Region,mct.areaname as city from MastSalesRep emp left join mastsalesrep sr on sr.smid=emp.underid  left join MastHeadquarter hq on hq.Id=emp.HeadquarterId left join MastLogin ml on ml.id=emp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  left join mastarea mct on mct.areaid=emp.cityid left join mastarea md on md.areaid=mct.underid  left join  mastarea ms on ms.areaid=md.underid left join mastarea mr on mr.areaid=ms.underid   where emp.SMId in (" + smIDStr1 + ") and mct.areatype='CITY' and md.areatype='DISTRICT' and ms.areatype='STATE' and mr.areatype='REGION'  order by emp.smname";
        //        DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, mSRepCode);
        //        DataView dvSales = new DataView(ssdt);

        //        dsrQuery = "select VisId,TV.SMId,VDate,DSR_Type,ISNULL(AppStatus,'') as DSRStatus,'DSR' AS Type,Remark,isnull(frTime1,'00:00') FirstCall ,isnull(toTime1,'00:00') LastCall, isnull(CONVERT(char(5), Mobile_Created_date,108),'') as logintime,isnull(convert(char(5), Mobile_End_date, 108),'') logouttime,isnull(EndRemark,'') EndRemark,MSP.SMNAME AS WITHwHOM  from TransVisit TV LEFT JOIN MASTSALESREP MSP ON MSP.SMID=TV.WithUserId  where vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  " + QryChk + "   order by TV.vdate,TV.SMId";
        //        DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
        //        DataView dvdsrData = new DataView(dsrData);

        //        //strTour = "select VDate,SMId,MCityName from TransTourPlan  where vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  order by VDate";
        //        //DataTable dtTour = DbConnectionDAL.GetDataTable(CommandType.Text, strTour);
        //        //DataView dvTour = new DataView(dtTour);

        //        //strTour = "select tb.PlannedDate,tb.SMId,tb.AppStatus,mb.AreaName +'('+ ma.AreaName+')' assignedBeat from TransBeatPlan tb left join MastArea mb on mb.AreaId=tb.BeatId left join MastArea ma on ma.AreaId=mb.UnderId  where  mb.AreaType='BEAT' and ma.AreaType='AREA' and  tb.PlannedDate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  order by tb.PlannedDate";
        //        //DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, strTour);
        //        //DataView dvBeat = new DataView(dtBeat);


        //        string No_Days = "select cast(NoOfDays as int) NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId IN (" + smIDStr1 + ") and (lr.FromDate between '" + mDate1.ToString() + " 00:00' and '" + mDate2.ToString() + " 23:59' OR lr.ToDate between '" + mDate1.ToString() + " 00:00' and '" + mDate2.ToString() + " 23:59') and lr.appstatus in ('Approve') order by smid,FromDate";
        //        DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
        //        DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);

        //        foreach (DataRow drvst in dsrData.Rows)
        //        {
        //            dvSales.RowFilter = "SMId=" + drvst["SMId"].ToString();

        //            if (dvSales.ToTable().Rows.Count > 0)
        //            {
        //                DataTable dtsp = dvSales.ToTable();
        //                DataRow dr = dtsp.Rows[0];

        //                string mRepName = dr["SMName"].ToString();
        //                string mSync = dr["SyncId"].ToString();
        //                DataRow mDataRow = gvdt.NewRow();

        //                DsrDate = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("yyyy-MM-dd");
        //                mDataRow["Date"] = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("yyyy-MM-dd").Trim();
        //                mDataRow["SMId"] = dr["SMId"].ToString();
        //                mDataRow["Reporting Manager"] = dr["ReportingManager"].ToString();
        //                mDataRow["Region"] = dr["Region"].ToString();
        //                mDataRow["SMName"] = mRepName.ToString();
        //                mDataRow["SyncId"] = mSync.ToString();
        //                mDataRow["SMStatus"] = dr["SMStatus"].ToString();
        //                mDataRow["Designation"] = dr["Designation"].ToString();
        //                mDataRow["HeadQuater"] = dr["HeadQuater"].ToString();
        //                mDataRow["Contact No"] = dr["ContactNo"].ToString();
        //                mDataRow["City"] = dr["city"].ToString();
        //                mDataRow["With Whom"] = drvst["WITHwHOM"].ToString();

        //                mDataRow["DSR Type"] = drvst["DSR_Type"].ToString() == "P" ? "Retailing" : drvst["DSR_Type"].ToString() == "W" ? "Week Off" : drvst["DSR_Type"].ToString() == "H" ? "Holiday" : "";

        //                if (drvst["DSR_Type"].ToString() == "A")
        //                {
        //                    #region Leave
        //                    dvdt_NoOfdays.RowFilter = string.Empty;
        //                    dvdt_NoOfdays.RowFilter = "smid=" + dr["SMId"] + " and (FromDate>= '" + DsrDate + "' and FromDate<= '" + DsrDate + "') or (NoDays>='" + DsrDate + "' and NoDays<='" + DsrDate + "')";
        //                    if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
        //                    {
        //                        mDataRow["DSR Type"] = "Leave";
        //                    }
        //                    else
        //                    {
        //                        mDataRow["DSR Type"] = "Absent";
        //                    }

        //                    #endregion
        //                }

        //                mDataRow["Remark"] = drvst["Remark"].ToString();
        //                mDataRow["LogOut Reason"] = drvst["EndRemark"].ToString();
        //                mDataRow["Login"] = drvst["logintime"].ToString();
        //                mDataRow["LogOut"] = drvst["logouttime"].ToString();
        //                mDataRow["First Call"] = drvst["FirstCall"].ToString();
        //                mDataRow["Last Call"] = drvst["LastCall"].ToString();


        //                //dvBeat.RowFilter = "smid=" + dr["SMId"] + " and PlannedDate= '" + DsrDate + "'";
        //                //if (dvBeat.ToTable().Rows.Count > 0)
        //                //{
        //                //    mDataRow["Assigned Beat"] = dvBeat.ToTable().Rows[0]["assignedBeat"].ToString();
        //                //}
        //                //dvBeat.RowFilter = null;

        //                //mDataRow["Selected Beat"] = Convert.ToString(TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "B", drvst["VisId"].ToString()));

        //                string var = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "D", drvst["VisId"].ToString());

        //                string[] arr = var.Split(',');
        //                mDataRow["Dist.Non Productive"] = var[0].ToString();
        //                mDataRow["Dist.Discuss"] = var[1].ToString();
        //                mDataRow["Dist.Collection"] = var[2].ToString();

        //                str = "select mp.partyid total from MastParty mp left join transvisit tv on mp.Created_User_id=tv.UserId  and cast (mp.Created_Date as date)=cast(tv.Created_date as date)  where  tv.smid=" + dr["SMId"] + " and tv.vdate='" + DsrDate + "'  " + QryChk + "  ";
        //                dtOrder = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //                if (dtOrder.Rows.Count > 0)
        //                {
        //                    mDataRow["New Outlet"] = dtOrder.Rows.Count;
        //                }

        //                //Dist.Discuss", type
        //                //Dist.Non Productive
        //                //Dist.Collection", t
        //                //mDataRow["Selected Journey Outelets"] = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "R", drvst["VisId"].ToString());

        //                //mDataRow["Scheduled Call"] = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "BR", drvst["VisId"].ToString());

        //                str = "select sum(qty) qty,sum(OrderAmount) OrderAmount,sum(NetAmount) NetAmount,SUM( TYPE) TYPE from ( select sum(tr1.qty) as qty , max(OrderAmount) OrderAmount,max(NetAmount) NetAmount, max(CASE WHEN ISNULL(TR.ORDERTAKENtYPE,'')='Telephonic' THEN 1 ELSE 0 END) TYPE  from transorder tr left join transvisit tv on tr.visid=tv.visid left join transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId  where  tv.smid=" + dr["SMId"] + " and tv.vdate='" + DsrDate + "'  " + QryChk + " GROUP BY TR1.OrdDocId union select sum(tr1.qty) as qty , max(OrderAmount) OrderAmount,max(NetAmount) NetAmount,max(CASE WHEN ISNULL(TR.ORDERTAKENtYPE,'')='Telephonic' THEN 1 ELSE 0 END) TYPE from Temp_transorder tr left join transvisit tv on tr.visid=tv.visid left join Temp_transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId where tv.smid=" + dr["SMId"] + " and tv.vdate='" + DsrDate + "'   " + QryChk + " GROUP BY TR1.OrdDocId) a ";
        //                dtOrder = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //                if (dtOrder.Rows.Count > 0)
        //                {
        //                    mDataRow["Qnty"] = dtOrder.Rows[0]["qty"].ToString();
        //                    mDataRow["Total Value"] = dtOrder.Rows[0]["OrderAmount"].ToString();
        //                    mDataRow["Net Value"] = dtOrder.Rows[0]["NetAmount"].ToString();
        //                    mDataRow["Telephonic Orders"] = dtOrder.Rows[0]["TYPE"].ToString();
        //                }

        //                str = "select sum(qty) as qty ,sum(NetAmount) NetAmount from ( select sum(tr1.qty) as qty ,max(tr.NetAmount) NetAmount from transorder tr left join transvisit tv on tr.visid=tv.visid left join transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId where   tv.smid=" + dr["SMId"] + " and tv.vdate<='" + DsrDate + "'   " + QryChk + "  GROUP BY TR1.OrdDocId   union select sum(tr1.qty) as qty,max(tr.NetAmount) NetAmount from Temp_transorder tr left join transvisit tv on tr.visid=tv.visid left join Temp_transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId where  tv.smid=" + dr["SMId"] + " and tv.vdate<='" + DsrDate + "'  " + QryChk + "  GROUP BY TR1.OrdDocId  )a ";
        //                dtOrder = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //                if (dtOrder.Rows.Count > 0)
        //                {
        //                    mDataRow["MTD(Qnty)"] = dtOrder.Rows[0]["qty"].ToString();
        //                    mDataRow["MTD(Net Value)"] = dtOrder.Rows[0]["NetAmount"].ToString();
        //                }




        //                str = " select sum(qty) as qty,sum(NetAmount) NetAmount from  (select sum(tr1.qty) as qty,max (tr.NetAmount) NetAmount from transorder tr left join transvisit tv on tr.visid=tv.visid left join transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId where   tv.smid=" + dr["SMId"] + " and tv.vdate='" + DsrDate + "'   AND tr.PartyId IN(" + dtOrder.Rows.Count + ")  " + QryChk + "  GROUP BY TR1.OrdDocId  union select sum(tr1.qty) as qty,max (tr.NetAmount) NetAmount from Temp_transorder tr left join transvisit tv on tr.visid=tv.visid left join Temp_transorder1 tr1 on tr1.OrdDocId=tr.OrdDocId where tv.smid=" + dr["SMId"] + " and tv.vdate='" + DsrDate + "' AND tr.PartyId IN(" + dtOrder.Rows.Count + ")  " + QryChk + " GROUP BY TR1.OrdDocId )a ";
        //                dtOrder = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //                if (dtOrder.Rows.Count > 0)
        //                {
        //                    mDataRow["New Outlet Sale(Qnty)"] = dtOrder.Rows[0]["qty"].ToString();
        //                    mDataRow["New Outlet Sale(Net Value)"] = dtOrder.Rows[0]["NetAmount"].ToString();
        //                }




        //                mDataRow["Total Time"] = drvst["TotalTime"].ToString();

        //                mDataRow["Total Retail Time"] = drvst["tottime"].ToString();
        //                mDataRow["Total Call Time (HH:MM)"] = "";

        //                TotalCall = Convert.ToDouble(TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "T", drvst["VisId"].ToString()));
        //                PCall = Convert.ToDouble(TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "P", drvst["VisId"].ToString()));

        //                mDataRow["Total Call"] = TotalCall;
        //                mDataRow["Joint Working Calls"] = TotalCall;
        //                mDataRow["Productive Call"] = PCall;
        //                if (PCall != 0)
        //                {
        //                    productivity = (Convert.ToDouble(PCall) / Convert.ToDouble(TotalCall)) * 100;
        //                }
        //                mDataRow["Productivity"] = productivity + " %";

        //                string fdate = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "FA", drvst["VisId"].ToString() + ",Asc");
        //                string Ldate = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), "FA", drvst["VisId"].ToString() + ",desc");
        //                DateTime firstact, Lastact;
        //                if (fdate != "0")
        //                {
        //                    firstact = Convert.ToDateTime(fdate);
        //                    mDataRow["First Activity Time"] = Convert.ToDateTime(fdate).ToString("hh:mm:ss");

        //                    if (Ldate != "0")
        //                    {
        //                        Lastact = Convert.ToDateTime(Ldate);
        //                        mDataRow["Last Activity Time"] = Convert.ToDateTime(Ldate).ToString("hh:mm:ss");
        //                        TimeSpan totspan = Lastact - firstact;
        //                        mDataRow["Activity Time Spent(HH:MM)"] = totspan.ToString(@"hh\:mm\:ss");
        //                    }
        //                }



        //                dvSales.RowFilter = null;
        //                dvdt_NoOfdays.RowFilter = null;
        //                gvdt.Rows.Add(mDataRow);
        //                gvdt.AcceptChanges();

        //            }


        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //    return gvdt;
        //}

        //        public DataTable BindDSRActivity()
        //        {

        //            #region Variable Declaration
        //            string str = "";
        //            string[] Distributor;
        //            string DistName = "", strleave = "", status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;
        //            int TotalCall = 0, PCall = 0;
        //            DataTable gvdt = new DataTable();
        //            #endregion

        //            try
        //            {
        //                string smIDStr = "", smIDStr1 = "";
        //                foreach (TreeNode node in trview.CheckedNodes)
        //                {
        //                    smIDStr1 = node.Value;
        //                    {
        //                        smIDStr += node.Value + ",";
        //                    }
        //                }
        //                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

        //                if (smIDStr1 == "")
        //                {
        //                    DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
        //                    DataView dv = new DataView(dtSMId);
        //                    dv.RowFilter = "RoleType='AreaIncharge' and SMName<>'.'";

        //                    if (dv.ToTable().Rows.Count > 0)
        //                    {
        //                        foreach (DataRow dr in dv.ToTable().Rows)
        //                        {
        //                            smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
        //                        }
        //                        smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
        //                    }
        //                }

        //                string mDate1 = Settings.dateformat(frmTextBox.Text);
        //                string mDate2 = Settings.dateformat(toTextBox.Text);

        //                gvdt.Columns.Add(new DataColumn("Date", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Region", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Reporting Manager", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Status", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Designation", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("HeadQuater", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Category", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Remark", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Distributor SyncId", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Distributor", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Start Time", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("End Time", typeof(String)));
        //                gvdt.Columns.Add(new DataColumn("Total Time", typeof(String)));

        //                DataRow pDataRow = gvdt.NewRow();
        //                string mSRepCode = "";

        //                mSRepCode = @"select  emp.SMId,emp.SMName,emp.SyncId,emp.Mobile,case when isnull(emp.Active,0)=1 then 'Active' else 'Deactive' end Active,replace(convert(varchar,emp.CreatedDate,106),' ','/') CreatedDate,mdst.DesName,isnull(hq.HeadquarterName,'')HeadquarterName,sr.smname as reportingPerson from MastSalesRep emp left join mastsalesrep sr on sr.smid=emp.underid  left join MastHeadquarter hq on hq.Id=emp.HeadquarterId left join MastLogin ml on ml.id=emp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where emp.SMId in (" + smIDStr1 + ")  order by emp.smname";//and emp.Active=1
        //                DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, mSRepCode);
        //                DataView dvSales = new DataView(ssdt);

        //                dsrQuery = "select * from (select  tv1.vdate,tv.smid,tv.SpentfrTime ,tv.SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00') [TotalTime],'Discussion' as type from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  " +
        //"union  select  tv1.vdate,tv.smid,tv.SpentfrTime,tv.SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00') [TotalTime],'Discussion' as type from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "' " +
        // "union select  tv1.vdate,tv.smid,'00:00' SpentfrTime ,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkS,'00:00' [TotalTime],'Distributor Collection' as type from DistributerCollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'   " +
        // "union select  tv1.vdate,tv.smid,'00:00' SpentfrTime ,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkS,'00:00' [TotalTime],'Distributor Failed Visit' as type from transfailedvisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'   AND MP.PARTYDIST=1 " +
        //"union  select  tv1.vdate,tv.smid,'00:00' SpentfrTime,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkS, '00:00' [TotalTime],'Distributor Failed Visit' as type from temp_transfailedvisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'   AND MP.PARTYDIST=1 " +
        // "union select  distinct tv1.vdate,tv.smid,'00:00' SpentfrTime ,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,'' remarkS,'00:00' [TotalTime],'Distributor Stock' as type from transdiststock tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  AND MP.PARTYDIST=1 " +
        //"union  select distinct tv1.vdate,tv.smid,'00:00' SpentfrTime,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,'' remarkS, '00:00' [TotalTime],'Distributor Stock' as type from temp_transdiststock tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  AND MP.PARTYDIST=1 " +
        // "union  select  tv1.vdate,tv.smid,tv.SpentfrTime ,  tv.SpentToTime,'0' partyid,'' partyname,'' syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00') [TotalTime],'Other' as type from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid  where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)')" +
        //"union  select tv1.vdate,tv.smid,tv.SpentfrTime ,tv.SpentToTime,'0' partyid,'' partyname,'' syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00')  [TotalTime],'Other' as type from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid  where isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + mDate1.ToString() + "' and '" + mDate2.ToString() + "'  AND tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') ) a order by vdate,smid,type";
        //                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
        //                DataView dvdsrData = new DataView(dsrData);

        //                foreach (DataRow drvst in dsrData.Rows)
        //                {

        //                    dvSales.RowFilter = "SMId=" + drvst["SMId"].ToString();

        //                    if (dvSales.ToTable().Rows.Count > 0)
        //                    {
        //                        DataTable dtsp = dvSales.ToTable();
        //                        DataRow dr = dtsp.Rows[0];

        //                        string mRepName = dr["SMName"].ToString();
        //                        string mSync = dr["SyncId"].ToString();
        //                        DataRow mDataRow = gvdt.NewRow();
        //                        DsrDate = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("dd/MM/yyyy").Trim();
        //                        mDataRow["Date"] = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("yyyy-MM-dd").Trim();
        //                        mDataRow["SMId"] = dr["SMId"].ToString();
        //                        mDataRow["Reporting Manager"] = dr["reportingPerson"].ToString();
        //                        mDataRow["Name"] = mRepName.ToString();
        //                        mDataRow["SyncId"] = mSync.ToString();
        //                        mDataRow["Status"] = dr["Active"].ToString();
        //                        mDataRow["Designation"] = dr["DesName"].ToString();
        //                        mDataRow["HeadQuater"] = dr["HeadquarterName"].ToString();
        //                        mDataRow["Contact No"] = dr["Mobile"].ToString();

        //                        mDataRow["Category"] = drvst["type"].ToString();
        //                        mDataRow["Remark"] = drvst["remarkdist"].ToString();
        //                        mDataRow["Distributor"] = drvst["partyname"].ToString();
        //                        mDataRow["Distributor SyncId"] = drvst["syncid"].ToString();

        //                        mDataRow["Start Time"] = drvst["SpentfrTime"].ToString();
        //                        mDataRow["End Time"] = drvst["SpentToTime"].ToString();
        //                        mDataRow["Total Time"] = drvst["TotalTime"].ToString();

        //                        if (drvst["partyid"].ToString() != "")
        //                        {
        //                            str = "select mr.areaname from mastarea ma left join mastarea mct on mct.areaid=ma.underid left join mastarea md on md.areaid=mct.underid left join mastarea ms on ms.areaid=md.underid left join mastarea mr on mr.areaid=ms.underid    where ma.areaid in ( select areaid from mastparty where partyid=" + drvst["partyid"].ToString() + " )";
        //                            DataTable dtregion = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //                            if (dtregion.Rows.Count > 0)
        //                            {
        //                                mDataRow["Region"] = dtregion.Rows[0]["areaname"].ToString();
        //                            }

        //                        }

        //                        dvSales.RowFilter = null;
        //                        gvdt.Rows.Add(mDataRow);
        //                        gvdt.AcceptChanges();

        //                    }


        //                }


        //            }
        //            catch (Exception ex)
        //            {
        //                ex.ToString();
        //            }
        //            return gvdt;
        //        }

        //public string TotalCountPrimarySecondarySales(string smid, string date, string TotalType, string visitId)
        //{
        //    string totalReailingDays = "0"; string str = "", QryChk = "";
        //    DataTable dt = new DataTable();
        //    date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");

        //    if (ddlDsrType.SelectedItem.Text == "Lock")
        //    {
        //        QryChk = " and isnull(tv1.lock,0)=1";
        //    }
        //    if (ddlDsrType.SelectedItem.Text == "UnLock")
        //    {
        //        QryChk = " and isnull(tv1.lock,0)=0";
        //    }

        //    if (ddlStatus.SelectedValue != "3")
        //    {
        //        if (QryChk == "")
        //        {
        //            QryChk += " and ISNULL(tv1.AppStatus,'Pending')='" + ddlStatus.SelectedValue + "'";
        //        }
        //        else
        //        {
        //            QryChk += " and ISNULL(tv1.AppStatus,'Pending')'" + ddlStatus.SelectedValue + "'";
        //        }
        //    }
        //    if (TotalType.Trim().ToUpper() == "B")
        //    {
        //    //    #region SelectedBeat

        //    //    str = " select   STUFF((SELECT ', ' +area.total FROM (select distinct mb.AreaName + '('+ma.AreaName +')' total from  (select PartyId from  (select tv.PartyId from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " 	group by tv.PartyId union select tv.PartyId from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where tv1.visid=" + visitId + " and  tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " 	group by tv.PartyId )a  group by PartyId " +
        //    // "union " +
        //    // "select PartyId from  (select tv.PartyId from transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " 	group by tv.PartyId union select tv.PartyId from temp_transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " 	group by tv.PartyId)a group by PartyId " +
        //    //   "union " +
        //    //  "select PartyId from  (select tv.PartyId from TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and partydist=0 	group by tv.PartyId union select tv.PartyId from temp_TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  and partydist=0 	group by tv.PartyId )a group by PartyId  " +
        //    // "union " +
        //    //"select PartyId from (select tv.PartyId from transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  group by tv.PartyId union select tv.PartyId from temp_transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  group by tv.PartyId )a group by PartyId " +
        //    //"union  " +
        //    //"select PartyId from  (select tv.PartyId from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  and partydist=0 	group by tv.PartyId union select tv.PartyId from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and partydist=0  group by tv.PartyId)a  group by PartyId " +
        //    //"union " +
        //    //"select PartyId from (select tv.PartyId from TransSample tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  group by tv.PartyId  union select tv.PartyId from temp_TransSample tv  left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  group by tv.PartyId )a group by PartyId " +
        //    //"union " +
        //    //"select PartyId from  (select tv.DistId PartyId from transvisitdist tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and partydist=0 	group by tv.DistId union select tv.DistId PartyId from temp_transvisitdist tv   left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and partydist=0 	group by tv.DistId)a group by PartyId " +
        //    //"union " +
        //    //"select PartyId from (select tv.PartyId from TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  group by tv.PartyId  union select tv.PartyId from temp_TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  group by tv.PartyId )a group by PartyId ) b left join MastParty mp on mp.PartyId=b.PartyId left join MastArea mb on mb.AreaId=mp.BeatId left join MastArea ma on ma.AreaId=mb.UnderId  where isnull(mp.PartyDist,0)=0  and mb.AreaType='BEAT' and ma.AreaType='AREA' ) area FOR XML PATH ('')), 1, 1, '') total ";

        //    //    #endregion
        //    }
        //    else if (TotalType.Trim().ToUpper() == "D")
        //    {
        //        #region Distributor

        //        str = " select (Cast(FV as varchar) +','+Cast(Dis as varchar)+','+Cast(Coll as varchar)) AS Total from(select Sum(FV) AS FV,Sum(Dis) AS Dis,Sum(Coll) AS Coll from  (select ISnull(SUM(FV),'0') AS FV,'0' AS Dis,0 AS Coll from  (select Count(tv.partyid) AS FV,'0' AS Dis,0 AS Coll from TransFailedVisit tv  left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and mp.partydist=1 	group by tv.partyid " +

        //            " union all select Count(tv.partyid) AS FV,'0' AS Dis,0 AS Coll from temp_TransFailedVisit  tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and mp.partydist=1 	group by tv.partyid)a " +

        //           "union all select '0' AS FV,0 AS Dis,ISnull(Sum(Coll),'0') AS Coll from  (select '0' AS FV,0 AS Dis,Count(tv.partyid) AS Coll from transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  and mp.partydist=1 	group by tv.partyid "+

        //           " union all select '0' AS FV,0 AS Dis,Count(tv.partyid) AS Coll from temp_transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "	group by tv.partyid)a "+

        //           " union all select '0' AS FV,ISnull(Sum(Dis),'0') AS Dis,0 AS Coll from  (select '0' AS FV,Count(tv.DistId) AS Dis,0 AS Coll from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.DistId where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  and mp.partydist=1 group by tv.DistId"+

        //           " union all select '0' AS FV,Count(tv.DistId) AS Dis,0 AS Coll from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.DistId where tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and mp.partydist=1 group by tv.DistId)a "+

        //           " union all select '0' AS FV,ISnull(Sum(Dis),'0') AS Dis,0 AS Coll from   (select '0' AS FV,Count(tv.DistId) AS Dis,0 AS Coll from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') group by tv.DistId "+

        //           " union all select '0' AS FV,Count(tv.DistId) AS Dis,0 AS Coll from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') group by tv.DistId)a) b ) c ";

        //        #endregion
        //    }
        //    else if (TotalType.Trim().ToUpper() == "T")
        //    {
        //        #region Total Call

        //        str = " select count(totaldoc) total from( select totaldoc from  (select tv.partyid totaldoc   from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where    tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.partyid totaldoc from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where    tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "   union select tv.partyid totaldoc from transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where    tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.partyid totaldoc from temp_transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where     tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " union select tv.partyid totaldoc from TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.partyid totaldoc  from temp_TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " union select tv.partyid totaldoc from transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.partyid totaldoc from temp_transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union  select tv.partyid totaldoc from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.partyid totaldoc from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.partyid totaldoc from TransSample tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "   union select tv.partyid totaldoc from temp_TransSample tv  left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.DistId totaldoc from transvisitdist tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.DistId totaldoc from temp_transvisitdist tv   left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "   union select tv.partyid totaldoc from TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.partyid totaldoc from temp_TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.DistId totaldoc from TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.DistId totaldoc from Temp_TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + ") b group by totaldoc)a";
        //        #endregion
        //    }

        //    else if (TotalType.Trim().ToUpper() == "P")
        //    {
        //        #region Productive Call
        //        str = "select count(*) total from (select *  from  (select tv.partyid totaldoc   from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union all select tv.partyid totaldoc from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union all select tv.partyid totaldoc from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "     union all select tv.partyid totaldoc from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  ) b group by totaldoc)a";
        //        #endregion
        //    }
        //    else if (TotalType.Trim().ToUpper() == "R")
        //    {
        //        //str = "select Count(partyid) as total from mastparty where cityid in (select  * from dbo.split((select tv1.cityids from transvisit tv1 where  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' " + QryChk + ") ,',' ))";
        //        str = "select Count(mp.partyid) as total from mastparty  mp left join transvisit tv on mp.Created_User_id=tv.userid and cast (mp.Created_Date as date)=cast(tv.vdate as date) where isnull(mp.partydist,0)=0 and tv.visid=" + visitId + "";
        //    }
        //    else if (TotalType.Trim().ToUpper() == "BR")
        //    {
        //        str = "select Count(partyid) as total from mastparty where beatid in (select  * from dbo.split((select beatid from transbeatplan where  appstatus='Approve' and smid=" + smid + " and PlannedDate='" + date + "' ) ,',' ))";
        //    }
        //    else if (TotalType.Trim().ToUpper() == "FA")
        //    {
        //        #region First Activity
        //        string[] filter;
        //        filter = visitId.Split(',');
        //        str = " select totaldoc total from( select totaldoc from  (select tv.Mobile_Created_date  totaldoc   from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where    tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.Mobile_Created_date totaldoc from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where    tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "   union select tv.Mobile_Created_date totaldoc from transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where    tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.Mobile_Created_date totaldoc from temp_transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where     tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " union select tv.Mobile_Created_date totaldoc from TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.Mobile_Created_date totaldoc  from temp_TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + " union select tv.Mobile_Created_date totaldoc from transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.Mobile_Created_date totaldoc from temp_transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union  select tv.Mobile_Created_date totaldoc from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.Mobile_Created_date totaldoc from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where   tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.Mobile_Created_date totaldoc from TransSample tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "   union select tv.Mobile_Created_date totaldoc from temp_TransSample tv  left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.Mobile_Created_date totaldoc from transvisitdist tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where  tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.Mobile_Created_date totaldoc from temp_transvisitdist tv   left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where  tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "   union select tv.Mobile_Created_date totaldoc from TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.Mobile_Created_date totaldoc from temp_TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "  union select tv.Mobile_Created_date totaldoc from TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + "    union select tv.Mobile_Created_date totaldoc from Temp_TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where   tv1.visid=" + filter[0] + " and tv1.smid=" + smid + " and tv1.vdate='" + date + "' " + QryChk + ") b)a order by totaldoc " + filter[1];
        //        #endregion
        //    }
        //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //    if (dt.Rows.Count > 0)
        //    {
        //        totalReailingDays = dt.Rows[0]["total"].ToString();
        //    }

        //    return totalReailingDays;
        //}

    }
}