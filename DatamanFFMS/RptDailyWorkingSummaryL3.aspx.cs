using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;

namespace AstralFFMS
{
    public partial class RptDailyWorkingSummaryL3 : System.Web.UI.Page
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
        public static string roleType = "";

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
                btnExport.CssClass = "btn btn-primary";
                //frmTextBox.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                frmTextBox.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                toTextBox.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                //Ankita - 18/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalePersonDDl();
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
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 and mr.Roletype in ('StateHead','RegionHead') order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) and mr.Roletype in ('StateHead','RegionHead') order by msr.smname";
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


        //private void BindSalePersonDDl()
        //{

        //    if (roleType == "Admin")
        //    {
        //        //Ankita - 18/may/2016- (For Optimization)
        //        //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //        string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //        DataTable dtcheckrole = new DataTable();
        //        dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //        DataView dv1 = new DataView(dtcheckrole);
        //        dv1.RowFilter = "RoleType='RegionHead' or RoleType='StateHead' and SMName<>'.'";
        //        dv1.Sort = "SMName asc";

        //        ListBox1.DataSource = dv1.ToTable();
        //        ListBox1.DataTextField = "SMName";
        //        ListBox1.DataValueField = "SMId";
        //        ListBox1.DataBind();
        //    }
        //    else
        //    {
        //        DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
        //        DataView dv = new DataView(dt);

        //        dv.RowFilter = "RoleType='RegionHead' or RoleType='StateHead' and SMName<>'.'";
        //        ListBox1.DataSource = dv.ToTable();
        //        ListBox1.DataTextField = "SMName";
        //        ListBox1.DataValueField = "SMId";
        //        ListBox1.DataBind();
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
        //    {
        //        //Ankita - 18/may/2016- (For Optimization)
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
        //        //Ankita - 18/may/2016- (For Optimization)s
        //        // FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
        //        getchilddata(tnParent, tnParent.Value);
        //    }
        //}
        //Ankita - 18/may/2016- (For Optimization)
        //private void getchilddata(TreeNode parent, string ParentId)
        //{

        //    string SmidVar = string.Empty;
        //    string GetFirstChildData = string.Empty;
        //    int levelcnt = 0;
        //    if (Settings.Instance.RoleType == "Admin")
        //        //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
        //        levelcnt = Convert.ToInt16("0") + 2;
        //    else
        //        levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel)+1;               


        //    GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 and mr.roletype in ('RegionHead','StateHead') order by SMName,lvl desc ";
        //    DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

        //    if (FirstChildDataDt.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
        //        {
        //            SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
        //            FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
        //        }
        //        SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

        //       // for (int j = levelcnt + 1; j <= 5; j++)
        //        for (int j = levelcnt + 1; j <= 9; j++)
        //        {
        //            string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 and mr.roletype in ('RegionHead','StateHead') order by SMName,lvl desc ";
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
        //        //var AreaQueryChild = "select * from Mastsalesrep where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " order by SMName,lvl";
        //    var AreaQueryChild = "SELECT SMId,Smname +' ('+ ms.Syncid + ' - ' + mr.RoleName + ')' as smname,Lvl from Mastsalesrep ms LEFT JOIN mastrole mr ON mr.RoleId=ms.RoleId where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " and ms.Active=1 AND mr.RoleType IN ('RegionHead','StateHead') order by SMName,lvl";
        //        DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
        //        parent.ChildNodes.Clear();
        //        foreach (DataRow dr in dtChild.Rows)
        //        {
        //            TreeNode child = new TreeNode();
        //            child.Text = dr["SMName"].ToString().Trim();
        //            child.Value = dr["SMId"].ToString().Trim();
        //            child.SelectAction = TreeNodeSelectAction.Expand;
        //            parent.ChildNodes.Add(child);
        //            //child.ExpandAll();
        //            child.CollapseAll();
        //            FillChildArea(child, child.Value, (Convert.ToInt32(dr["Lvl"])), Convert.ToInt32(dr["SMId"].ToString()));
        //        }            

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

        private void GetDailyWorkingSummaryL3(string SPID, string FromDate, string ToDate, string userID, string isExport)
        {
            try
            {
                string query = "";

                int totalworkingDays = 0;
                string QryChk = "";
                string gettotalparty = "";

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
                        if (ddlStatus.SelectedValue != "Pending")
                        {
                            QryChk += " where a.AType='" + ddlStatus.SelectedValue + "'";
                        }
                        else
                        {
                            QryChk += " where a.AType IS NULL";
                        }
                    }
                    else
                    {
                        if (ddlStatus.SelectedValue != "Pending")
                        {
                            QryChk += " and a.AType='" + ddlStatus.SelectedValue + "'";
                        }
                        else
                        {
                            QryChk += " and a.AType IS NULL";
                        }
                    }
                }

                if (ddlDsrType.SelectedItem.Text == "All")
                {

                    gettotalparty = "(SELECT Count(*) FROM MastParty WHERE BeatId IN ( SELECT Distinct T.BeatId FROM GetSalesDatewiseBeat AS T WHERE T.VDate = a.VDate AND T.SMId= a.SMID) AND active=1 AND partydist=0)";

                    query = "select a.SMID as Id,convert(varchar,a.VDate,106) as [VDate],(a.Level1) as Level1,(a.SyncId) as SyncId,Format(sum(a.TotalOrder),'N2') as TotalOrder,sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],sum(a.DistDiscuss) as DistDiscuss,sum(a.retailerdiscuss) as retailerdiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],sum(a.Competitor) [Competitor],0 as [Collections]," + gettotalparty + " as TotalParty,max(a.Remarks) as Remarks,CASE WHEN max(a.type)='9' THEN max(a.beat) Else '' END  AS beatname,max(a.cityname) as cityname,max(a.AppRemark) as AppRemark11,AType= (case when max(a.atype) is null then 'Pending' WHEN max(a.atype) ='Approve' THEN 'Approved' WHEN max(a.atype) ='Reject' THEN 'Rejected'  else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,a.EmpName, max(a.type) AS type,Max(CASE WHEN Type='9' THEN CASE WHEN ( a.lock1 IS NULL OR a.lock1 = 0 ) THEN 'UnLock' WHEN ( a.lock1 = 1 ) THEN 'Lock' END ELSE  NULL END)  AS lock,max(a.RoleType) as RoleType,a.Dsr_Time,a.FirstCall,a.LastCall,isnull(a.WithWhom,'') as WithWhom,a.TPD as ProsDist  from ( " +

                         "SELECT View_DSR.SMID,View_DSR.VDate,View_DSR.Level1 AS Level1,View_DSR.SyncId,View_DSR.TotalOrder as TotalOrder,View_DSR.OrderAmountMail as OrderAmountMail, View_DSR.OrderAmountPhone as OrderAmountPhone,View_DSR.DistributorCollection as DistributorCollection,View_DSR.PartyCollection AS PartyCollection,View_DSR.PerCallAvgCell as [PerCallAvgCell],View_DSR.CallsVisited as [CallsVisited],View_DSR.RetailerProCalls as [RetailerProCalls],View_DSR.FailedVisit as [FailedVisit],View_DSR.DistFailVisit as DistFailVisit,View_DSR.DistDiscuss as DistDiscuss,View_DSR.retailerDiscuss as retailerDiscuss,View_DSR.NewParties as NewParties,View_DSR.LocalExpenses as [LocalExpenses], View_DSR.TourExpenses as [TourExpenses],View_DSR.Demo as [Demo],View_DSR.Competitor AS Competitor,0 as TotalParty,View_DSR.Remarks as Remarks,View_DSR.cityname,View_DSR.AppRemark as AppRemark,View_DSR.AType as AType,View_DSR.EmpName as EmpName, View_DSR.Type AS Type,View_DSR.Lock1,View_DSR.Lock2,View_DSR.RoleType,(Select Isnull(frtime1,'') from Transvisit where smid=View_DSR.SMID and vdate=View_DSR.VDate) as FirstCall,(Select Isnull(totime1,'') from Transvisit where smid=View_DSR.SMID and vdate=View_DSR.VDate and lock=1) as LastCall,(Select ms.smname from Transvisit tr left join MastSalesRep ms on ms.smid=tr.WithUserId where tr.smid=View_DSR.SMID and tr.vdate=View_DSR.VDate) as WithWhom,(Select convert(char(5), tr.Mobile_Created_date, 108) from Transvisit tr where tr.smid=View_DSR.SMID and tr.vdate=View_DSR.VDate) as Dsr_Time" +


                     ",isnull((select STUFF((SELECT ', ' +ANM from(select distinct ma.AreaName as ANM from mastparty mp left join mastarea ma on mp.beatid=ma.areaid where partyid in (select DISTINCT PartyId from transorder where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransCollection where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransCompetitor where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransDemo where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from TransFailedVisit where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_transorder where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransCollection where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransCompetitor where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransDemo where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT PartyId from temp_TransFailedVisit where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT DistId from TransVisitDist where smid = View_DSR.smid and vdate = View_DSR.vdate union select DISTINCT DistId from temp_TransVisitDist where smid = View_DSR.smid and vdate = View_DSR.vdate))tbl FOR XML PATH ('')) , 1, 1, '') ),'') AS Beat" +

                      ",(Select Count(md.PartyId) from MastProspect_Distributor md left join mastsalesrep msp on msp.userid=md.[Created UserId] where msp.Smid=View_DSR.SMID and CONVERT(DATE, md.Insert_Date)=CONVERT(DATE, View_DSR.VDate)) as TPD" +

                         " FROM View_DSR WHERE View_DSR.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND View_DSR.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('StateHead','RegionHead')) " +
                         "UNION ALL select [View_Holiday].smid as SMID,CONVERT (varchar, [View_Holiday].holiday_date,106) as [VisitDate] ,[View_Holiday].smname as [Level1],'' as SyncId,0 as [TotalOrder],0 as OrderAmountMail, 0 as OrderAmountPhone,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited],0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as retailerDiscuss,0 as [NewParties],0 as [LocalExpenses],0 as [TourExpenses],0 as [Demo],'' AS Competitor,0 as TotalParty, [View_Holiday].Reason as Remarks,'' as Cityname,''  as AppRemark,'' as AType,'' as EmpName,'HOLIDAY' AS Type,0 as lock1,0 as lock2,'' as RoleType,'' as FirstCall,'' as LastCall,'' as WithWhom,'' as Dsr_Time,'' as Beat,0 as TPD from [View_Holiday] where [View_Holiday].holiday_date Between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and [View_Holiday].smid IN (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('StateHead','RegionHead'))) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName,a.FirstCall,a.LastCall,a.WithWhom,a.Dsr_Time,a.TPD Order by a.VDate";
                }
                else if (ddlDsrType.SelectedItem.Text == "Lock")
                {

                    gettotalparty = "(SELECT Count(*) FROM MastParty WHERE BeatId IN ( SELECT Distinct T.BeatId FROM GetSalesDatewiseBeat AS T WHERE T.VDate = a.VDate AND T.SMId= a.SMID) AND active=1 AND partydist=0)";
                    query = "select a.SMID as Id,convert(varchar,a.VDate,106) as [VDate],(a.Level1) as Level1,(a.SyncId) as SyncId,Format(sum(a.TotalOrder),'N2') as TotalOrder,sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],sum(a.DistDiscuss) as DistDiscuss,sum(a.retailerdiscuss) as retailerdiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],sum(a.Competitor) [Competitor],0 as [Collections]," + gettotalparty + " as TotalParty,max(a.Remarks) as Remarks,CASE WHEN max(a.type)='9' THEN max(a.beat) Else '' END  AS beatname,max(a.cityname) as cityname,max(a.AppRemark) as AppRemark11,AType= (case when max(a.atype) is null then 'Pending' WHEN max(a.atype) ='Approve' THEN 'Approved' WHEN max(a.atype) ='Reject' THEN 'Rejected'  else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,a.EmpName, max(a.type) AS type,Max(CASE WHEN Type='9' THEN CASE WHEN ( a.lock1 IS NULL OR a.lock1 = 0 ) THEN 'UnLock' WHEN ( a.lock1 = 1 ) THEN 'Lock' END ELSE  NULL END)  AS lock,max(a.RoleType) as RoleType,a.Dsr_Time,a.FirstCall,a.LastCall,isnull(a.WithWhom,'') as WithWhom,a.TPD as ProsDist  from ( " +

                         "SELECT View_DSR_Lock.SMID,View_DSR_Lock.VDate,View_DSR_Lock.Level1 AS Level1,View_DSR_Lock.SyncId,View_DSR_Lock.TotalOrder as TotalOrder,View_DSR_Lock.OrderAmountMail as OrderAmountMail, View_DSR_Lock.OrderAmountPhone as OrderAmountPhone,View_DSR_Lock.DistributorCollection as DistributorCollection,View_DSR_Lock.PartyCollection AS PartyCollection,View_DSR_Lock.PerCallAvgCell as [PerCallAvgCell],View_DSR_Lock.CallsVisited as [CallsVisited],View_DSR_Lock.RetailerProCalls as [RetailerProCalls],View_DSR_Lock.FailedVisit as [FailedVisit],View_DSR_Lock.DistFailVisit as DistFailVisit,View_DSR_Lock.DistDiscuss as DistDiscuss,View_DSR_Lock.retailerDiscuss as retailerDiscuss,View_DSR_Lock.NewParties as NewParties,View_DSR_Lock.LocalExpenses as [LocalExpenses], View_DSR_Lock.TourExpenses as [TourExpenses],View_DSR_Lock.Demo as [Demo],View_DSR_Lock.Competitor AS Competitor,0 as TotalParty,View_DSR_Lock.Remarks as Remarks,View_DSR_Lock.cityname,View_DSR_Lock.AppRemark as AppRemark,View_DSR_Lock.AType as AType,View_DSR_Lock.EmpName as EmpName, View_DSR_Lock.Type AS Type,View_DSR_Lock.Lock1,View_DSR_Lock.Lock2,View_DSR_Lock.RoleType,(Select Isnull(frtime1,'') from Transvisit where smid=View_DSR_Lock.SMID and vdate=View_DSR_Lock.VDate) as FirstCall,(Select Isnull(totime1,'') from Transvisit where smid=View_DSR_Lock.SMID and vdate=View_DSR_Lock.VDate and lock=1) as LastCall,(Select ms.smname from Transvisit tr left join MastSalesRep ms on ms.smid=tr.WithUserId where tr.smid=View_DSR_Lock.SMID and tr.vdate=View_DSR_Lock.VDate) as WithWhom,(Select convert(char(5), tr.Mobile_Created_date, 108) from Transvisit tr where tr.smid=View_DSR_Lock.SMID and tr.vdate=View_DSR_Lock.VDate) as Dsr_Time" +


                     ",isnull((select STUFF((SELECT ', ' +ANM from(select distinct ma.AreaName as ANM from mastparty mp left join mastarea ma on mp.beatid=ma.areaid where partyid in (select DISTINCT PartyId from transorder where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransCollection where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransCompetitor where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransDemo where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from TransFailedVisit where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_transorder where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransCollection where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransCompetitor where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransDemo where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT PartyId from temp_TransFailedVisit where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT DistId from TransVisitDist where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate union select DISTINCT DistId from temp_TransVisitDist where smid = View_DSR_Lock.smid and vdate = View_DSR_Lock.vdate))tbl FOR XML PATH ('')) , 1, 1, '') ),'') AS Beat" +

                      ",(Select Count(md.PartyId) from MastProspect_Distributor md left join mastsalesrep msp on msp.userid=md.[Created UserId] where msp.Smid=View_DSR_Lock.SMID and CONVERT(DATE, md.Insert_Date)=CONVERT(DATE, View_DSR_Lock.VDate)) as TPD" +

                         " FROM View_DSR_Lock WHERE View_DSR_Lock.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND View_DSR_Lock.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('StateHead','RegionHead')) " +
                         "UNION ALL select [View_Holiday].smid as SMID,CONVERT (varchar, [View_Holiday].holiday_date,106) as [VisitDate] ,[View_Holiday].smname as [Level1],'' as SyncId,0 as [TotalOrder],0 as OrderAmountMail, 0 as OrderAmountPhone,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited],0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as retailerDiscuss,0 as [NewParties],0 as [LocalExpenses],0 as [TourExpenses],0 as [Demo],'' AS Competitor,0 as TotalParty, [View_Holiday].Reason as Remarks,'' as Cityname,''  as AppRemark,'' as AType,'' as EmpName,'HOLIDAY' AS Type,0 as lock1,0 as lock2,'' as RoleType,'' as FirstCall,'' as LastCall,'' as WithWhom,'' as Dsr_Time,'' as Beat,0 as TPD from [View_Holiday] where [View_Holiday].holiday_date Between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and [View_Holiday].smid IN (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('StateHead','RegionHead'))) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName,a.FirstCall,a.LastCall,a.WithWhom,a.Dsr_Time,a.TPD Order by a.VDate";
                }
                else
                {

                    gettotalparty = "(SELECT Count(*) FROM MastParty WHERE BeatId IN ( SELECT Distinct T.BeatId FROM GetSalesDatewiseBeat AS T WHERE T.VDate = a.VDate AND T.SMId= a.SMID) AND active=1 AND partydist=0)";
                    query = "select a.SMID as Id,convert(varchar,a.VDate,106) as [VDate],(a.Level1) as Level1,(a.SyncId) as SyncId,Format(sum(a.TotalOrder),'N2') as TotalOrder,sum(a.OrderAmountMail) as OrderAmountMail,sum(a.OrderAmountPhone) as OrderAmountPhone,sum(a.DistributorCollection) as DistributorCollection,sum(a.PartyCollection) as PartyCollection,iif(max(a.CallsVisited) <>0,isnull(sum(a.TotalOrder)/sum(a.CallsVisited),''),0) as PerCallAvgCell,sum(a.CallsVisited) as [CallsVisited],sum(a.RetailerProCalls) as RetailerProCalls,SUM(a.FailedVisit) as [FailedVisit],SUM(a.DistFailVisit) as [DistFailVisit],sum(a.DistDiscuss) as DistDiscuss,sum(a.retailerdiscuss) as retailerdiscuss,sum(a.NewParties) as NewParties,SUM(a.LocalExpenses) as [LocalExpenses],sum(a.TourExpenses) as [TourExpenses],SUM(a.Demo) [Demo],sum(a.Competitor) [Competitor],0 as [Collections]," + gettotalparty + " as TotalParty,max(a.Remarks) as Remarks,CASE WHEN max(a.type)='9' THEN max(a.beat) Else '' END  AS beatname,max(a.cityname) as cityname,max(a.AppRemark) as AppRemark11,AType= (case when max(a.atype) is null then 'Pending' WHEN max(a.atype) ='Approve' THEN 'Approved' WHEN max(a.atype) ='Reject' THEN 'Rejected'  else '' end),(case when (a.AppRemark IS NULL OR a.AppRemark='') then a.AppRemark else a.AppRemark end) as AppRemark,a.EmpName, max(a.type) AS type,Max(CASE WHEN Type='9' THEN CASE WHEN ( a.lock1 IS NULL OR a.lock1 = 0 ) THEN 'UnLock' WHEN ( a.lock1 = 1 ) THEN 'Lock' END ELSE  NULL END)  AS lock,max(a.RoleType) as RoleType,a.Dsr_Time,a.FirstCall,a.LastCall,isnull(a.WithWhom,'') as WithWhom,a.TPD as ProsDist  from ( " +

                         "SELECT View_DSR_UnLock.SMID,View_DSR_UnLock.VDate,View_DSR_UnLock.Level1 AS Level1,View_DSR_UnLock.SyncId,View_DSR_UnLock.TotalOrder as TotalOrder,View_DSR_UnLock.OrderAmountMail as OrderAmountMail, View_DSR_UnLock.OrderAmountPhone as OrderAmountPhone,View_DSR_UnLock.DistributorCollection as DistributorCollection,View_DSR_UnLock.PartyCollection AS PartyCollection,View_DSR_UnLock.PerCallAvgCell as [PerCallAvgCell],View_DSR_UnLock.CallsVisited as [CallsVisited],View_DSR_UnLock.RetailerProCalls as [RetailerProCalls],View_DSR_UnLock.FailedVisit as [FailedVisit],View_DSR_UnLock.DistFailVisit as DistFailVisit,View_DSR_UnLock.DistDiscuss as DistDiscuss,View_DSR_UnLock.retailerDiscuss as retailerDiscuss,View_DSR_UnLock.NewParties as NewParties,View_DSR_UnLock.LocalExpenses as [LocalExpenses], View_DSR_UnLock.TourExpenses as [TourExpenses],View_DSR_UnLock.Demo as [Demo],View_DSR_UnLock.Competitor AS Competitor,0 as TotalParty,View_DSR_UnLock.Remarks as Remarks,View_DSR_UnLock.cityname,View_DSR_UnLock.AppRemark as AppRemark,View_DSR_UnLock.AType as AType,View_DSR_UnLock.EmpName as EmpName, View_DSR_UnLock.Type AS Type,View_DSR_UnLock.Lock1,View_DSR_UnLock.Lock2,View_DSR_UnLock.RoleType,(Select Isnull(frtime1,'') from Transvisit where smid=View_DSR_UnLock.SMID and vdate=View_DSR_UnLock.VDate) as FirstCall,(Select Isnull(totime1,'') from Transvisit where smid=View_DSR_UnLock.SMID and vdate=View_DSR_UnLock.VDate and lock=1) as LastCall,(Select ms.smname from Transvisit tr left join MastSalesRep ms on ms.smid=tr.WithUserId where tr.smid=View_DSR_UnLock.SMID and tr.vdate=View_DSR_UnLock.VDate) as WithWhom,(Select convert(char(5), tr.Mobile_Created_date, 108) from Transvisit tr where tr.smid=View_DSR_UnLock.SMID and tr.vdate=View_DSR_UnLock.VDate) as Dsr_Time" +


                     ",isnull((select STUFF((SELECT ', ' +ANM from(select distinct ma.AreaName as ANM from mastparty mp left join mastarea ma on mp.beatid=ma.areaid where partyid in (select DISTINCT PartyId from transorder where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransCollection where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransCompetitor where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransDemo where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from TransFailedVisit where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_transorder where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransCollection where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransCompetitor where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransDemo where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT PartyId from temp_TransFailedVisit where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT DistId from TransVisitDist where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate union select DISTINCT DistId from temp_TransVisitDist where smid = View_DSR_UnLock.smid and vdate = View_DSR_UnLock.vdate))tbl FOR XML PATH ('')) , 1, 1, '') ),'') AS Beat" +

                      ",(Select Count(md.PartyId) from MastProspect_Distributor md left join mastsalesrep msp on msp.userid=md.[Created UserId] where msp.Smid=View_DSR_UnLock.SMID and CONVERT(DATE, md.Insert_Date)=CONVERT(DATE, View_DSR_UnLock.VDate)) as TPD" +

                         " FROM View_DSR_UnLock WHERE View_DSR_UnLock.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' AND View_DSR_UnLock.SMID in (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('StateHead','RegionHead')) " +
                         "UNION ALL select [View_Holiday].smid as SMID,CONVERT (varchar, [View_Holiday].holiday_date,106) as [VisitDate] ,[View_Holiday].smname as [Level1],'' as SyncId,0 as [TotalOrder],0 as OrderAmountMail, 0 as OrderAmountPhone,0 as DistributorCollection,0 AS PartyCollection,0 as [PerCallAvgCell],0 as [CallsVisited],0 as [RetailerProCalls],0 as [FailedVisit],0 as DistFailVisit,0 as DistDiscuss,0 as retailerDiscuss,0 as [NewParties],0 as [LocalExpenses],0 as [TourExpenses],0 as [Demo],'' AS Competitor,0 as TotalParty, [View_Holiday].Reason as Remarks,'' as Cityname,''  as AppRemark,'' as AType,'' as EmpName,'HOLIDAY' AS Type,0 as lock1,0 as lock2,'' as RoleType,'' as FirstCall,'' as LastCall,'' as WithWhom,'' as Dsr_Time,'' as Beat,0 as TPD from [View_Holiday] where [View_Holiday].holiday_date Between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and [View_Holiday].smid IN (SELECT ms.smid from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId= mr.RoleId WHERE ms.SMId IN (" + SPID + ") AND mr.RoleType IN ('StateHead','RegionHead'))) a " + QryChk + " Group by a.VDate,a.SMID,a.Level1,a.SyncId,a.AppRemark,a.lock1,a.smid,a.EmpName,a.FirstCall,a.LastCall,a.WithWhom,a.Dsr_Time,a.TPD Order by a.VDate";
                }

                DataTable udt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (udt.Rows.Count > 0)
                {
                    ViewState["GridData"] = udt;
                    udt.AsEnumerable().Where(r => r.Field<String>("AType") == "Rejected" && r.Field<String>("type") == "8").ToList().ForEach(row => row.Delete());
                    udt.AcceptChanges();
                    rpt.DataSource = udt;
                    rpt.DataBind();
                    udt.Select();
                    btnExport.Visible = true;
                    //string date_new = "", date_Old = "";
                    //for (int td = 0; td < udt.Rows.Count; td++)
                    //{
                    //    date_Old = udt.Rows[td]["VDate"].ToString();
                    //    if (date_new != date_Old && udt.Rows[td]["Level1"].ToString() != "" && udt.Rows[td]["Type"].ToString() == Convert.ToInt32(9).ToString())
                    //    {
                    //        date_new = udt.Rows[td]["VDate"].ToString();
                    //        totalworkingDays = totalworkingDays + 1;
                    //    }
                    //}
                    lblTotalWorkingdays.Text = Convert.ToString(totalworkingDays);
                }
                else
                {

                    rpt.DataSource = null;
                    rpt.DataBind();
                    lblTotalWorkingdays.Text = "";
                }
                udt.Dispose();
                //}
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        decimal totalsale = 0M;
        decimal totalcallvisited = 0M;

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDailyWorkingSummaryL3.aspx", true);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            rptmain.Style.Add("display", "block");
            string smIDStr = "";
            string smIDStr1 = "", userIdStr = "";
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
            if (smIDStr1 == "")
            {
                DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dtSMId);
                dv.RowFilter = "RoleType='RegionHead' or RoleType='StateHead' and SMName<>'.'";

                if (dv.ToTable().Rows.Count > 0)
                {
                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);

                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                }
                dtSMId.Dispose();
                dv.Dispose();
            }


            if (smIDStr1 != "")
            {
                string salesRepUserIdQry = @"select UserId from MastSalesRep where SMId in (" + smIDStr1 + ")";

                DataTable userdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepUserIdQry);
                if (userdt.Rows.Count > 0)
                {
                    for (int i = 0; i < userdt.Rows.Count; i++)
                    {
                        userIdStr += userdt.Rows[i]["UserId"] + ",";
                    }
                }
                userIdStr = userIdStr.TrimStart(',').TrimEnd(',');

                if (frmTextBox.Text != string.Empty && toTextBox.Text != string.Empty)
                {
                    if (Convert.ToDateTime(frmTextBox.Text) <= Convert.ToDateTime(toTextBox.Text))
                    {
                        GetDailyWorkingSummaryL3(smIDStr1, frmTextBox.Text, toTextBox.Text, userIdStr, "0");
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
                        rptmain.Style.Add("display", "none");
                    }
                }

                userdt.Dispose();
            }
            else
            {
                rptmain.Style.Add("display", "none");
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('Please select salesperson');", true);
            }
        }

        //protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "select")
        //    {
        //        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //        int index = gvRow.RowIndex;
        //        GridViewRow row = gvData.Rows[index];
        //        HiddenField hdnDate = (HiddenField)row.FindControl("hdnDate");
        //        HiddenField hdnSmiD = (HiddenField)row.FindControl("hdnSMId");
        //        Response.Redirect("DSRReportL3.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value);
        //    }
        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=DailyWorkingSummary-Level3.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //Repeater1.RenderControl(hw);          
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DailyWorkingSummary-Level3.csv");
            string headertxt = "Visit Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Type".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Total Retailer".TrimStart('"').TrimEnd('"') + "," + "Retailer Visited".TrimStart('"').TrimEnd('"') + "," + "Retailer Productive Calls".TrimStart('"').TrimEnd('"') + "," + "Total Order".TrimStart('"').TrimEnd('"') + "," + "Demo".TrimStart('"').TrimEnd('"') + "," + "Non-Productive".TrimStart('"').TrimEnd('"') + "," + "Competitor".TrimStart('"').TrimEnd('"') + "," + "Retai. Discuss".TrimStart('"').TrimEnd('"') + "," + "Retailer Collection".TrimStart('"').TrimEnd('"') + "," + "New Retailer".TrimStart('"').TrimEnd('"') + "," + "Retailer Per Call Avg Sale".TrimStart('"').TrimEnd('"') + "," + "Claim Exp.".TrimStart('"').TrimEnd('"') + "," + "Approved Exp.".TrimStart('"').TrimEnd('"') + "," + "Dist. Discuss".TrimStart('"').TrimEnd('"') + "," + "Dist. Non-Productive".TrimStart('"').TrimEnd('"') + "," + "Dist. Collection".TrimStart('"').TrimEnd('"') + "," + "Prospect Distributor".TrimStart('"').TrimEnd('"') + "," + "Start DSR Time".TrimStart('"').TrimEnd('"') + "," + "First Call".TrimStart('"').TrimEnd('"') + "," + "Last Call".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"') + "," + "DSR Type".TrimStart('"').TrimEnd('"') + "," + "Approved Remarks".TrimStart('"').TrimEnd('"') + "," + "With Whom".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertxt);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Vdate", typeof(DateTime)));
            dtParams.Columns.Add(new DataColumn("level", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("empName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Type", typeof(String)));
            dtParams.Columns.Add(new DataColumn("remarks", typeof(String)));
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
            dtParams.Columns.Add(new DataColumn("atype", typeof(String)));
            dtParams.Columns.Add(new DataColumn("lock", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("OrderMail", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("OrderPhone", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("syncId", typeof(String)));

            dtParams.Columns.Add(new DataColumn("appRemark", typeof(String)));
            dtParams.Columns.Add(new DataColumn("WithWhom", typeof(String)));
            //

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
                Label prosdistLabel = item.FindControl("lblpros") as Label;
                dr["ProsDist"] = prosdistLabel.Text.ToString();
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
                //Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                //dr["syncId"] = syncIdLabel.Text.ToString(); 

                Label lblDsrTme = item.FindControl("lbldsrtme") as Label;
                dr["DsrTime"] = lblDsrTme.Text.ToString();
                Label lblFirstCall = item.FindControl("lblFirstCall") as Label;
                dr["FirstCall"] = lblFirstCall.Text.ToString();

                Label lblLastCall = item.FindControl("lblLastCall") as Label;
                dr["LastCall"] = lblLastCall.Text.ToString();

                Label atypeLabel = item.FindControl("atypeLabel") as Label;
                dr["atype"] = atypeLabel.Text.ToString();
                Label lockLabel = item.FindControl("lockLabel") as Label;
                dr["lock"] = lockLabel.Text.ToString();
                Label appRemarkLabel = item.FindControl("appRemarkLabel") as Label;
                dr["appRemark"] = appRemarkLabel.Text.ToString().Replace("\n", "").Replace("\r", "");

                Label WithWhom = item.FindControl("lblWithWhom") as Label;
                dr["WithWhom"] = WithWhom.Text.ToString();

                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "VDate desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[22];
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
                                if (k == 6 || k == 7 || k == 8 || k == 9 || k == 10 || k == 11 || k == 12 || k == 13 || k == 14 || k == 15 || k == 16 || k == 17 || k == 18 || k == 19 || k == 20 || k == 21)
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
                                if (k == 6 || k == 7 || k == 8 || k == 9 || k == 10 || k == 11 || k == 12 || k == 13 || k == 14 || k == 15 || k == 16 || k == 17 || k == 18 || k == 19 || k == 20 || k == 21)
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
                                if (k == 6 || k == 7 || k == 8 || k == 9 || k == 10 || k == 11 || k == 12 || k == 13 || k == 14 || k == 15 || k == 16 || k == 17 || k == 18 || k == 19 || k == 20 || k == 21)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                                //End
                                // }
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                string totalStr = string.Empty;
                for (int x = 6; x < totalVal.Count(); x++)
                {
                    if (totalVal[x] == 0)
                    {
                        totalStr += "0" + ',';
                    }
                    else
                    {
                        totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                    }

                    //        arr[x] = Int32.Parse(Console.ReadLine());
                }
                sb.Append(",,,,,Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            sb.Clear();
            dtParams.Dispose();
            dv.Dispose();
            udtNew.Dispose();
            //string smIDStrNewL3 = "";
            //string smIDStr1NewL3 = "", userIdStrNewL3 = "";
            //foreach (TreeNode node in trview.CheckedNodes)
            //{
            //    smIDStr1NewL3 = node.Value;
            //    {
            //        smIDStrNewL3 += node.Value + ",";
            //    }
            //}
            //smIDStr1NewL3 = smIDStrNewL3.TrimStart(',').TrimEnd(',');
            //if (smIDStr1NewL3 == "")
            //{
            //    DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
            //    DataView dv = new DataView(dtSMId);
            //    dv.RowFilter = "RoleType='RegionHead' or RoleType='StateHead' and SMName<>'.'";

            //    if (dv.ToTable().Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dv.ToTable().Rows)
            //        {
            //            smIDStrNewL3 = smIDStrNewL3 + "," + Convert.ToString(dr["SMId"]);

            //        }
            //        smIDStr1NewL3 = smIDStrNewL3.TrimStart(',').TrimEnd(',');
            //    }
            //}


            //if (smIDStr1NewL3 != "")
            //{
            //    string salesRepUserIdQry = @"select UserId from MastSalesRep where SMId in (" + smIDStr1NewL3 + ")";

            //    DataTable userdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepUserIdQry);
            //    if (userdt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < userdt.Rows.Count; i++)
            //        {
            //            userIdStrNewL3 += userdt.Rows[i]["UserId"] + ",";
            //        }
            //    }
            //    userIdStrNewL3 = userIdStrNewL3.TrimStart(',').TrimEnd(',');

            //    if (frmTextBox.Text != string.Empty && toTextBox.Text != string.Empty)
            //    {
            //        if (Convert.ToDateTime(frmTextBox.Text) <= Convert.ToDateTime(toTextBox.Text))
            //        {
            //            GetDailyWorkingSummaryL3(smIDStr1NewL3, frmTextBox.Text, toTextBox.Text, userIdStrNewL3, "1");
            //        }
            //        else
            //        {
            //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
            //            rptmain.Style.Add("display", "none");
            //        }
            //    }
            //}
            //else
            //{
            //    rptmain.Style.Add("display", "none");
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('Please select salesperson');", true);
            //}
        }
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

                    //Label lblTotalParty = (Label)e.Item.FindControl("lblTotalParty");
                    //Label lblTotalDistCollection = (Label)e.Item.FindControl("lblTotalDistColl");
                    //Label lblTotalDistDiscuss = (Label)e.Item.FindControl("lblTotalDistDiscuss");

                    Label lblTotalSale = (Label)e.Item.FindControl("lblTotalOrder");
                    //Label lblTotalPartyCollection = (Label)e.Item.FindControl("lblTotalpartyColl");
                    //             Label lblPerCallAvgSale = (Label)e.Item.FindControl("lblPerCallAvg");
                    Label lblCallVisited = (Label)e.Item.FindControl("lblCallsVisited");



                    decimal sale = Decimal.Parse(lblTotalSale.Text);
                    totalsale += sale;
                    decimal Callvisited = Decimal.Parse(lblCallVisited.Text);
                    totalcallvisited += Callvisited;

                    if (Callvisited != 0)
                    {
                        //decimal PerCallAvgSale = decimal.Parse((sale / Callvisited).ToString()); //Decimal.Parse(lblPerCallAvgSale.Text);
                        //totalPerCallAvgSale += PerCallAvgSale;
                        //lblPerCallAvgSale.Text = PerCallAvgSale.ToString("#.##");
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
                //Response.Redirect("DSRReport.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value + "&Recstatus=" + hdnDsrType.Value);
                Response.Redirect("DSRReport.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value + "&Recstatus=" + hdnDsrType.Value);
            }

        }
    }
}