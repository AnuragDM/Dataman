using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace AstralFFMS
{
    public partial class TourPlanVsActualReport : System.Web.UI.Page
    {
        string roleType = "";
        string rptTemp = "rptTemp_TourActual";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {//Ankita - 16/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalesPerson();
                BindTreeViewControl();
                //fill_TreeArea();
                BindDDLMonth();
                ddlMonthSecSale.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlYearSecSale.SelectedValue = System.DateTime.Now.Year.ToString();
                btnExport.Visible = false;

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
        //private void BindSalesPerson()
        //{
        //    try
        //    {

        //        if (roleType == "Admin")
        //        {
        //            //Ankita - 16/may/2016- (For Optimization)
        //            //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            DataTable dtcheckrole = new DataTable();
        //            dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //            DataView dv1 = new DataView(dtcheckrole);
        //            dv1.RowFilter = "((RoleType='RegionHead' or RoleType='StateHead')  or (RoleType='CityHead' or RoleType='DistrictHead')) and SMName<>'.'";
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
        //            dv.RowFilter = "((RoleType='RegionHead' or RoleType='StateHead')  or (RoleType='CityHead' or RoleType='DistrictHead')) and SMName<>'.'";
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
            Response.Redirect("~/TourPlanVsActualReport.aspx");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {

               
                string smIDStr = "";
                string smIDStr1 = "", filter = "";
                //         string message = "";
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

                Qrychk = " year(vl.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(vl.VDate)='" + ddlMonthSecSale.SelectedValue + "'";


                if (ddlFilter.SelectedIndex != 0 && ddlFilter.SelectedValue != "1")
                {
                    filter = "where isnull(TourDistributor,'')<>isnull(VisitDistributor,'')";
                }

                if (smIDStr1 != "")
                {


                    //               query = @"select case when [Date1] is null then [Date2] else [Date1] end [Date],mdistname as [TourDistributor],mpurposename as [Purpose],mcityname as [TourCity],[VisitDistributor],[VisitCity],[Srep],[Remarks] ,emp_id, SalesRepName,SyncId,TourRemark from (
                    //               select a.[Date] [Date1], a.[TourDistributor], a.[Purpose],a.[TourCity], a.TourRemark, b.[Date] [Date2], b.[VisitDistributor], b.[VisitCity],b.[Srep], b.[Remarks], CASE WHEN a.SalesRepName IS NULL THEN b.SalesRepName ELSE a.SalesRepName END SalesRepName, 
                    //               case when a.[emp_id] is null then b.[emp_id] else a.[emp_id] end emp_id,case when a.[syncid] is null then b.[syncid] else a.[syncid] end syncid,a.mcityname,a.mpurposename,a.mdistname from ( 
                    //
                    //               select Vdate [Date],  tp.Remarks as TourRemark, d.PartyName [TourDistributor], mpv.PurposeName [Purpose],c.AreaName [TourCity],'' [VisitDistributor],'' [VisitCity],'' [Remarks],tp.SMId [emp_id], msr.SMName as SalesRepName,msr.SyncId,tp.mcityname,tp.mpurposename,mdistname
                    //               from TransTourPlan tp left outer join MastParty d on d.PartyId=tp.DistId left outer join MastArea c on c.AreaId=tp.CityId left join MastPurposeVisit mpv on mpv.Id=Convert(varchar,tp.Purpose) left join MastSalesRep msr on msr.SMId=tp.SMId
                    //               where (tp.SMId in (" + smIDStr1 + ")) AND tp.AppStatus NOT IN ('Reject') and year(tp.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(tp.VDate)='" + ddlMonthSecSale.SelectedValue + "' )a FULL JOIN ( " +

                    //               " select  x.Date,max(x.TourDistributor) AS TourDistributor,max(x.[Purpose]) AS [purpose],max(x.[TourCity]) AS [TourCity] ,x.VisitDistributor,max(x.VisitCity) AS VisitCity ,max(x.Srep) AS Srep, max(x.Remarks) AS Remarks, max(x.[emp_id]) AS emp_id, max(x.SalesRepName) AS SalesRepName,max(x.SyncId) AS SyncId,'' as mcityname,'' as mpurposename,'' as mdistname from ( " +
                    //               " select om.Vdate [Date],'' [TourDistributor], '' [Purpose],'' [TourCity], getdistributor1(year(VDate)='" + ddlYearSecSale.SelectedValue + "' and month(VDate)='" + ddlMonthSecSale.SelectedValue + "') as [VisitDistributor], max(tv.cityName) [VisitCity], max(cp.SMNAme) [Srep],max(om.remarkDist) [Remarks],max(tv.smid) [emp_id], max(cp1.smname) as SalesRepName,max(cp1.SyncId) as SyncId from  TransVisitDist om  left outer join MastParty d on d.PartyId=om.DistId left outer join MastArea c on c.AreaId=d.CityId LEFT JOIN TransVisit tv ON tv.SMId=om.SMId left outer join MastSalesRep cp on cp.smid=tv.WithUserId  LEFT JOIN mastsalesrep cp1 ON cp1.SMId=tv.SMId where om.SMID  in (" + smIDStr1 + ") AND d.PartyDist=1 and year(om.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(om.VDate)='" + ddlMonthSecSale.SelectedValue + "'  group BY om.VDate,d.PartyName " +
                    //               " UNION ALL select om.Vdate [Date],'' [TourDistributor], '' [Purpose],'' [TourCity], max(d.PartyName) as [VisitDistributor], max(tv.cityName) [VisitCity], max(cp.SMNAme) [Srep],max(om.Remarks) [Remarks],max(tv.smid) [emp_id], max(cp1.smname) as SalesRepName,max(cp1.SyncId) as SyncId from  TransFailedVisit om  left outer join MastParty d on d.PartyId=om.PartyId left outer join MastArea c on c.AreaId=d.CityId LEFT JOIN TransVisit tv ON tv.SMId=om.SMId left outer join MastSalesRep cp on cp.smid=tv.WithUserId  LEFT JOIN mastsalesrep cp1 ON cp1.SMId=tv.SMId where om.SMID  in (" + smIDStr1 + ") AND d.PartyDist=1 and year(om.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(om.VDate)='" + ddlMonthSecSale.SelectedValue + "'  group BY om.Vdate,d.PartyName " +
                    //               " UNION ALL select om.Vdate [Date],'' [TourDistributor], '' [Purpose],'' [TourCity], max(d.PartyName) as [VisitDistributor], max(tv.cityName) [VisitCity], max(cp.SMNAme) [Srep],max(om.Remarks) [Remarks],max(tv.smid) [emp_id], max(cp1.smname) as SalesRepName,max(cp1.SyncId) as SyncId from  DistributerCollection om  left outer join MastParty d on d.PartyId=om.DistId left outer join MastArea c on c.AreaId=d.CityId LEFT JOIN TransVisit tv ON tv.SMId=om.SMId left outer join MastSalesRep cp on cp.smid=tv.WithUserId  LEFT JOIN mastsalesrep cp1 ON cp1.SMId=tv.SMId where om.SMID  in (" + smIDStr1 + ") AND d.PartyDist=1 and year(om.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(om.VDate)='" + ddlMonthSecSale.SelectedValue + "'  group BY om.VDate,d.PartyName )x Group by  x.Date,x.VisitDistributor " +
                    //               ") b ON a.date = b.date and a.emp_id=b.emp_id) main " + filter + " Order by Date, [TourDistributor],[Purpose],[TourCity],[VisitDistributor],[VisitCity],[Remarks]";


                    //Commented By Akanksha on 14/09/2020<<<<<<
                    // Added Nishu 10/03/2016
//                    query = @"select case when [Date1] is null then [Date2] else [Date1] end [Date],mdistname as [TourDistributor],mpurposename as [Purpose],mcityname as [TourCity],[VisitDistributor],[VisitCity],[Srep],[Remarks] ,emp_id, SalesRepName,SyncId,TourRemark from (
//                                   select a.[Date] [Date1], a.[TourDistributor], a.[Purpose],a.[TourCity], a.TourRemark, b.[Date] [Date2], b.[VisitDistributor], b.[VisitCity],b.[Srep], b.[Remarks], CASE WHEN a.SalesRepName IS NULL THEN b.SalesRepName ELSE a.SalesRepName END SalesRepName, 
//                                   case when a.[emp_id] is null then b.[emp_id] else a.[emp_id] end emp_id,case when a.[syncid] is null then b.[syncid] else a.[syncid] end syncid,a.mcityname,a.mpurposename,a.mdistname from ( 
//                    
//                                   select Vdate [Date],  tp.Remarks as TourRemark, d.PartyName [TourDistributor], mpv.PurposeName [Purpose],c.AreaName [TourCity],'' [VisitDistributor],'' [VisitCity],'' [Remarks],tp.SMId [emp_id], msr.SMName as SalesRepName,msr.SyncId,tp.mcityname,tp.mpurposename,mdistname
//                                   from TransTourPlan tp left outer join MastParty d on d.PartyId=tp.DistId left outer join MastArea c on c.AreaId=tp.CityId left join MastPurposeVisit mpv on mpv.Id=Convert(varchar,tp.Purpose) left join MastSalesRep msr on msr.SMId=tp.SMId
//                                   where (tp.SMId in (" + smIDStr1 + ")) AND tp.AppStatus NOT IN ('Reject') and year(tp.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(tp.VDate)='" + ddlMonthSecSale.SelectedValue + "' )a FULL JOIN ( " +

//             " select  x.Date,max(x.TourDistributor) AS TourDistributor,max(x.[Purpose]) AS [purpose],max(x.[TourCity]) AS [TourCity] ,x.VisitDistributor,max(x.VisitCity) AS VisitCity ,max(x.Srep) AS Srep, max(x.Remarks) AS Remarks, max(x.[emp_id]) AS emp_id, max(x.SalesRepName) AS SalesRepName,max(x.SyncId) AS SyncId,'' as mcityname,'' as mpurposename,'' as mdistname from ( " +
//             "select tv.Vdate [Date],'' [TourDistributor], tv.VisId, '' [Purpose],'' [TourCity],  dbo.getdistributor(tv.visid) as [VisitDistributor], max(tv.cityName) [VisitCity], max(cp.SMNAme) [Srep],max(tv.Remark) [Remarks],max(tv.smid) [emp_id], max(cp1.smname) as SalesRepName,max(cp1.SyncId) as SyncId from  TransVisit tv left outer join MastSalesRep cp on cp.smid=tv.WithUserId LEFT JOIN mastsalesrep cp1 ON cp1.SMId=tv.SMId where tv.SMID   in (" + smIDStr1 + ") and year(tv.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(tv.VDate)='" + ddlMonthSecSale.SelectedValue + "'  group BY tv.VDate,tv.VisId )x Group by  x.Date,x.VisitDistributor " +
//                        //" select om.Vdate [Date],'' [TourDistributor], '' [Purpose],'' [TourCity], getdistributor1(year(VDate)='" + ddlYearSecSale.SelectedValue + "' and month(VDate)='" + ddlMonthSecSale.SelectedValue + "') as [VisitDistributor], max(tv.cityName) [VisitCity], max(cp.SMNAme) [Srep],max(om.remarkDist) [Remarks],max(tv.smid) [emp_id], max(cp1.smname) as SalesRepName,max(cp1.SyncId) as SyncId from  TransVisitDist om  left outer join MastParty d on d.PartyId=om.DistId left outer join MastArea c on c.AreaId=d.CityId LEFT JOIN TransVisit tv ON tv.SMId=om.SMId left outer join MastSalesRep cp on cp.smid=tv.WithUserId  LEFT JOIN mastsalesrep cp1 ON cp1.SMId=tv.SMId where om.SMID  in (" + smIDStr1 + ") AND d.PartyDist=1 and year(om.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(om.VDate)='" + ddlMonthSecSale.SelectedValue + "'  group BY om.VDate,d.PartyName " +
//                        //" UNION ALL select om.Vdate [Date],'' [TourDistributor], '' [Purpose],'' [TourCity], max(d.PartyName) as [VisitDistributor], max(tv.cityName) [VisitCity], max(cp.SMNAme) [Srep],max(om.Remarks) [Remarks],max(tv.smid) [emp_id], max(cp1.smname) as SalesRepName,max(cp1.SyncId) as SyncId from  TransFailedVisit om  left outer join MastParty d on d.PartyId=om.PartyId left outer join MastArea c on c.AreaId=d.CityId LEFT JOIN TransVisit tv ON tv.SMId=om.SMId left outer join MastSalesRep cp on cp.smid=tv.WithUserId  LEFT JOIN mastsalesrep cp1 ON cp1.SMId=tv.SMId where om.SMID  in (" + smIDStr1 + ") AND d.PartyDist=1 and year(om.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(om.VDate)='" + ddlMonthSecSale.SelectedValue + "'  group BY om.Vdate,d.PartyName " +
//                        //" UNION ALL select om.Vdate [Date],'' [TourDistributor], '' [Purpose],'' [TourCity], max(d.PartyName) as [VisitDistributor], max(tv.cityName) [VisitCity], max(cp.SMNAme) [Srep],max(om.Remarks) [Remarks],max(tv.smid) [emp_id], max(cp1.smname) as SalesRepName,max(cp1.SyncId) as SyncId from  DistributerCollection om  left outer join MastParty d on d.PartyId=om.DistId left outer join MastArea c on c.AreaId=d.CityId LEFT JOIN TransVisit tv ON tv.SMId=om.SMId left outer join MastSalesRep cp on cp.smid=tv.WithUserId  LEFT JOIN mastsalesrep cp1 ON cp1.SMId=tv.SMId where om.SMID  in (" + smIDStr1 + ") AND d.PartyDist=1 and year(om.VDate)='" + ddlYearSecSale.SelectedValue + "' and month(om.VDate)='" + ddlMonthSecSale.SelectedValue + "'  group BY om.VDate,d.PartyName )x Group by  x.Date,x.VisitDistributor " +
//             ") b ON a.date = b.date and a.emp_id=b.emp_id) main " + filter + " Order by Date, [TourDistributor],[Purpose],[TourCity],[VisitDistributor],[VisitCity],[Remarks]";

                    //////end >>>>>>



                    // Modification By Akanksha Bais on 15-09-2020  <<<<<< Start

                    Create_temp(rptTemp, "transtourplan", " SMId in (" + smIDStr1 + ") AND AppStatus NOT IN ('Reject') and year(VDate)='" + ddlYearSecSale.SelectedValue + "' and month(VDate)='" + ddlMonthSecSale.SelectedValue + "'");
                     
                    query = "alter table " + rptTemp + " add visid varchar(50)null ,SNAME varchar(200) null,VDIST varchar(max) null,VCITY varchar(max) null,VREMK varchar(max) null,Vdate1 varchar(50) null";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, query);

                    query = "update temp set vdist=dbo.getdistributor(tv.visid),VCITY=dbo.getVisitedCity(tv.cityIdS),VREMK=tv.Remark from " + rptTemp + " as temp inner join transvisit as tv on temp.smid=tv.smid and temp.vdate=tv.vdate";
                   DbConnectionDAL.ExecuteNonQuery(CommandType.Text, query);

                    //query = "select smid,vdate from " + rptTemp + "";
                    //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < dt.Rows.Count; i++)
                    //    {
                    //        query = "select dbo.getdistributor(visid) as  vdist ,dbo.getVisitedCity(cityIdS) as VCITY,Remark from transvisit where SMId = " + dt.Rows[i]["smid"].ToString() + " and VDate='" + Convert.ToDateTime(dt.Rows[i]["vdate"].ToString()).ToString("yyyy-MM-dd") + "'";
                    //        DataTable dtdist = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    //        if(dtdist.Rows.Count>0)
                    //        {
                    //            query = "update  " + rptTemp + " set vdist = '" + dtdist.Rows[0]["vdist"].ToString() + "'  ,  VCITY = '" + dtdist.Rows[0]["VCITY"].ToString() + "', VREMK = '" + dtdist.Rows[0]["Remark"].ToString() + "'  where SMId = " + dt.Rows[i]["smid"].ToString() + " and VDate='" + Convert.ToDateTime(dt.Rows[i]["vdate"].ToString()).ToString("yyyy-MM-dd") + "'";
                    //            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, query);
                    //        }
                            
                    //    }
                    //}

                    query = "update tp set SNAME=ms.smname + '[' + ms.syncid  FROM " + rptTemp + " tp   left join   MastSalesRep ms on tp.smid=ms.smid";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, query);

                    query = "select   [vdate] as  [Date],mpurposename as [Purpose],mdistname as [TourDistributor],mcityname as [TourCity],vdist as [VisitDistributor],VCITY as [VisitCity],'' as [Srep],VREMK as [Remarks] ,SMId as emp_id, substring(SNAME,0,charindex('[',SNAME)) as  SalesRepName,substring(SNAME,charindex('[',SNAME)+1,len(SNAME)) as SyncId, Remarks as TourRemark from " + rptTemp + " Order by Date, [TourDistributor],[Purpose],[TourCity],[VisitDistributor],[VisitCity],[Remarks]";
                    
                    //////>>>>>>End
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
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select salespesron');", true);
                    tourvsactrpt.DataSource = null;
                    tourvsactrpt.DataBind();
                }
               //////Akanksha
                Delete_temp(rptTemp);
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
            Response.AddHeader("content-disposition", "attachment;filename=TourPlanvsActualReport.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Day".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Sync Id".TrimStart('"').TrimEnd('"') + "," + "Planned Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Planned City".TrimStart('"').TrimEnd('"') + "," + "Purpose Of Visit".TrimStart('"').TrimEnd('"') + "," + "Tour Remark".TrimStart('"').TrimEnd('"') + "," + "Visited Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Visited City".TrimStart('"').TrimEnd('"') + "," + "Sales Person Worked With".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Day", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));

            dtParams.Columns.Add(new DataColumn("PlannedDistributorName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PlannedCity", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PurposeOfVisit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TourRemark", typeof(String)));
            dtParams.Columns.Add(new DataColumn("VisitedDistributorName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("VisitedCity", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SalesPersonWorkedWith", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Remark", typeof(String)));
            //
            foreach (RepeaterItem item in tourvsactrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label DateLabel = item.FindControl("DateLabel") as Label;
                dr["Date"] = DateLabel.Text;
                Label DayLabel = item.FindControl("DayLabel") as Label;
                dr["Day"] = DayLabel.Text.ToString();
                Label SalesRepNameLabel = item.FindControl("SalesRepNameLabel") as Label;
                dr["SalesPerson"] = SalesRepNameLabel.Text.ToString();
                Label SyncIdLabel = item.FindControl("SyncIdLabel") as Label;
                dr["SyncId"] = SyncIdLabel.Text.ToString();

                Label TourDistributorLabel = item.FindControl("TourDistributorLabel") as Label;
                dr["PlannedDistributorName"] = TourDistributorLabel.Text.ToString();
                Label TourCityLabel = item.FindControl("TourCityLabel") as Label;
                dr["PlannedCity"] = TourCityLabel.Text.ToString();
                Label PurposeLabel = item.FindControl("PurposeLabel") as Label;
                dr["PurposeOfVisit"] = PurposeLabel.Text.ToString();
                Label TourRemarkLabel = item.FindControl("TourRemarkLabel") as Label;
                dr["TourRemark"] = TourRemarkLabel.Text.ToString();
                Label VisitDistributorLabel = item.FindControl("VisitDistributorLabel") as Label;
                dr["VisitedDistributorName"] = VisitDistributorLabel.Text.ToString();
                Label VisitCityLabel = item.FindControl("VisitCityLabel") as Label;
                dr["VisitedCity"] = VisitCityLabel.Text.ToString();
                Label SrepLabel = item.FindControl("SrepLabel") as Label;
                dr["SalesPersonWorkedWith"] = SrepLabel.Text.ToString();
                Label RemarksLabel = item.FindControl("RemarksLabel") as Label;
                dr["Remark"] = RemarksLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
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
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
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
                        if (k == 0 || k == 0)
                        {
                            sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
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
            Response.AddHeader("content-disposition", "attachment;filename=TourPlanvsActualReport.csv");
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();
            sb.Clear();
            dtParams.Dispose();
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

        private void Create_temp(string tempTable, string mainTable, string _condition = "")
        {
            string str = "";
            str = "if OBJECT_ID('" + tempTable + "') is not null  drop table " + tempTable + "";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            if (_condition == "")
            {
                str = "select * into " + tempTable + " from " + mainTable + "";
            }
            else
            {
                str = "select * into " + tempTable + " from " + mainTable + " where " + _condition + " ";
            }
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
        }

        private void Delete_temp(string tempTable)
        {
            string str = "";
            str = "if OBJECT_ID('" + tempTable + "') is not null  drop table " + tempTable + "";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
        }

        

    }
}