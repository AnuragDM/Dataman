using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class LeaveReport : System.Web.UI.Page
    {
        string roleType = "";
        TreeNode spnode;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                //Ankita - 13/may/2016- (For Optimization)
                roleType = Settings.Instance.RoleType;
                //GetRoleType(Settings.Instance.RoleID);
                //BindSalePersonDDl();
                //fill_TreeArea();
                btnExport.Visible = false;
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }

        //private void BindSalePersonDDl()
        //{
        //    try
        //    {
        //        if (roleType == "Admin")
        //        { //Ankita - 13/may/2016- (For Optimization)
        //            string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            DataTable dtcheckrole = new DataTable();
        //            dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //            DataView dv1 = new DataView(dtcheckrole);
        //            dv1.RowFilter = "SMName<>.";
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
        //            dv.RowFilter = "SMName<>.";
        //            dv.Sort = "SMName asc";
        //            if (dv.ToTable().Rows.Count > 0)
        //            {
        //                ListBox1.DataSource = dv.ToTable();
        //                ListBox1.DataTextField = "SMName";
        //                ListBox1.DataValueField = "SMId";
        //                ListBox1.DataBind();
        //            }
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
        void fill_TreeArea()
        {
            int lowestlvl = 0;
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
            {  //Ankita - 13/may/2016- (For Optimization)
                //  lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                // St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,Convert.ToInt16(Settings.Instance.SalesPersonLevel));
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
                getchilddata(tnParent, tnParent.Value);


                // FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
            }

            St.Dispose();
        }
        //Ankita - 16/may/2016- (For Optimization)
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

                for (int j = levelcnt + 1; j <= 6; j++)
                {
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
                    DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
                  //  SmidVar = string.Empty;
                    int mTotRows = dtChild.Rows.Count;
                    if (mTotRows > 0)
                    {
                        SmidVar = string.Empty;
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
                    dtChild.Dispose();
                }
            }

            FirstChildDataDt.Dispose();
            //if (dtChild.Rows.Count > 0)
            //{

            //    for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
            //    {
            //        spnode = parent;
            //        DataRow[] dr = dtChild.Select("maingrp =" + FirstChildDataDt.Rows[i]["smid"].ToString() + " and smchild <> " + FirstChildDataDt.Rows[i]["smid"].ToString());
            //        for (int j = 0; j < dr.Length; j++)
            //        {
            //          //  if (spnode.Value == dr[j]["smchild"].ToString()) { }
            //            FillChildArea(spnode, FirstChildDataDt.Rows[i]["smid"].ToString(), dr[j]["smchild"].ToString(), dr[j]["smname"].ToString());
            //        }
            //    }
            //}

        }

        public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        {
            //var AreaQueryChild = "select * from Mastsalesrep where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " order by SMName,lvl";
            //var AreaQueryChild = "SELECT SMId,Smname +' ('+ ms.Syncid + ' - ' + mr.RoleName + ')' as smname,Lvl from Mastsalesrep ms LEFT JOIN mastrole mr ON mr.RoleId=ms.RoleId where lvl=" + (LVL + 1) + " and UnderId=" + SMId + " and ms.Active=1 order by SMName,lvl";
            //var AreaQueryChild = "select smid,level,maingrp,(select smname from mastsalesrep msr where msr.smid=mst.smid)as smname from mastsalesrepgrp mst where maingrp in( select smid from mastsalesrepgrp where maingrp =" + ParentId + ") order by level";

            //DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
            //parent.ChildNodes.Clear();
            //foreach (DataRow dr in dtChild.Rows)
            //{


            TreeNode child = new TreeNode();
            child.Text = SMName;
            child.Value = Smid;
            child.SelectAction = TreeNodeSelectAction.Expand;
            parent.ChildNodes.Add(child);
            child.CollapseAll();

            //if (spnode.Value != child.Value)
            //{
            //    child.SelectAction = TreeNodeSelectAction.Expand;
            //    parent.ChildNodes.Add(child);
            //    spnode = child;
            //    //child.ExpandAll();
            //    child.CollapseAll();
            //}
            //else
            //{

            //}
            // FillChildArea(child, child.Value, (Convert.ToInt32(dr["Lvl"])), Convert.ToInt32(dr["SMId"].ToString()));
            //}

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LeaveReport.aspx");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string smIDStr = "", Qrychk = "", Query = "";
            string smIDStr1 = "";
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
            //Qrychk = " lr.FromDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.ToDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            Qrychk = " (lr.FromDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.FromDate<='" + Settings.dateformat(txttodate.Text) + " 23:59' or lr.ToDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.ToDate<='" + Settings.dateformat(txttodate.Text) + " 23:59')";

            if (ddlLeavStatus.SelectedValue != "0" && ddlLeavStatus.SelectedValue != "")
                Qrychk = Qrychk + " and lr.AppStatus='" + ddlLeavStatus.SelectedValue + "'";

            if (smIDStr1 != "")
            {
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    leavereportrpt.DataSource = new DataTable();
                    leavereportrpt.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }
                //Query = "select lr.FromDate, lr.Reason, lr.AppStatus,lr.NoOfDays, cp1.SMName as AppByName, lr.ToDate,cp.SMName,cp.SyncId from TransLeaveRequest lr left join MastSalesRep cp on cp.SMId=lr.SMId left join MastSalesRep cp1 on cp1.SMId=lr.AppBySMId where " + Qrychk + " and lr.SMId in (" + smIDStr1 + ") ";
                Query = "select lr.FromDate, lr.Reason, lr.AppStatus,lr.NoOfDays, cp1.SMName as AppByName, lr.ToDate,cp.SMName,cp.SyncId from TransLeaveRequest lr left join MastSalesRep cp on cp.SMId=lr.SMId left join MastSalesRep cp1 on cp1.SMId=lr.AppBySMId where " + Qrychk + " and lr.SMId in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))) order by SMname";
                DataTable dtLeaveRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dtLeaveRep.Rows.Count > 0)
                {
                    leavereportrpt.DataSource = dtLeaveRep;
                    leavereportrpt.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    leavereportrpt.DataSource = dtLeaveRep;
                    leavereportrpt.DataBind();
                    btnExport.Visible = false;
                }
                dtLeaveRep.Dispose();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select sales person');", true);
                leavereportrpt.DataSource = null;
                leavereportrpt.DataBind();
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Addded - 03-06-2016 - Abhishek
            //string smIDStrNewCSV = "", QrychkNewCSV = "", QueryNewCSV = "";
            //string smIDStr1NewCSV = "";
            //foreach (ListItem item in ListBox1.Items)
            //{
            //    if (item.Selected)
            //    {
            //        smIDStr1NewCSV += item.Value + ",";
            //    }
            //}
            //smIDStr1NewCSV = smIDStr1NewCSV.TrimStart(',').TrimEnd(',');
            //foreach (TreeNode node in trview.CheckedNodes)
            //{
            //    smIDStr1NewCSV = node.Value;
            //    {
            //        smIDStrNewCSV += node.Value + ",";
            //    }
            //}
            //smIDStr1NewCSV = smIDStrNewCSV.TrimStart(',').TrimEnd(',');
            //QrychkNewCSV = " lr.FromDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.ToDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

            //if (ddlLeavStatus.SelectedValue != "0" && ddlLeavStatus.SelectedValue != "")
            //    QrychkNewCSV = QrychkNewCSV + " and lr.AppStatus='" + ddlLeavStatus.SelectedValue + "'";

            //if (smIDStr1NewCSV != "")
            //{
            //    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
            //    {
            //        leavereportrpt.DataSource = new DataTable();
            //        leavereportrpt.DataBind();
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
            //        return;
            //    }
            //    QueryNewCSV = "select cp.SMName,cp.SyncId,lr.NoOfDays,lr.FromDate,lr.ToDate, lr.Reason, lr.AppStatus, cp1.SMName as AppByName from TransLeaveRequest lr left join MastSalesRep cp on cp.SMId=lr.SMId left join MastSalesRep cp1 on cp1.SMId=lr.AppBySMId where " + QrychkNewCSV + " and lr.SMId in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1NewCSV + ")))";
            //    DataTable udtTabNewCSV = DbConnectionDAL.GetDataTable(CommandType.Text, QueryNewCSV);
            //    string headertext = "Name".TrimStart('"').TrimEnd('"') + "," + "Sync Id".TrimStart('"').TrimEnd('"') + "," + "For Days".TrimStart('"').TrimEnd('"') + "," + "From Date".TrimStart('"').TrimEnd('"') + "," + "To Date".TrimStart('"').TrimEnd('"') + "," + "Reason".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"') + "," + "Approved By".TrimStart('"').TrimEnd('"');
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append(headertext);
            //    sb.Append(System.Environment.NewLine);
            //    for (int j = 0; j < udtTabNewCSV.Rows.Count; j++)
            //    {
            //        for (int k = 0; k < udtTabNewCSV.Columns.Count; k++)
            //        {
            //            if (udtTabNewCSV.Rows[j][k].ToString().Contains(","))
            //            {
            //                if (k == 3)
            //                {
            //                    sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(udtTabNewCSV.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
            //                }
            //                else
            //                {
            //                    sb.Append(String.Format("\"{0}\"", udtTabNewCSV.Rows[j][k].ToString()) + ',');
            //                }
            //            }
            //            else if (udtTabNewCSV.Rows[j][k].ToString().Contains(System.Environment.NewLine))
            //            {
            //                if (k == 3)
            //                {
            //                    sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(udtTabNewCSV.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
            //                }
            //                else
            //                {
            //                    sb.Append(String.Format("\"{0}\"", udtTabNewCSV.Rows[j][k].ToString()) + ',');
            //                }
            //            }
            //            else
            //            {
            //                if (k == 3 || k==4)
            //                {
            //                    sb.Append(Convert.ToDateTime(udtTabNewCSV.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
            //                }
            //                else
            //                {
            //                    sb.Append(udtTabNewCSV.Rows[j][k].ToString() + ',');
            //                }

            //            }
            //        }
            //        sb.Append(Environment.NewLine);
            //    }
            //    Response.Clear();
            //    Response.ContentType = "text/csv";
            //    Response.AddHeader("content-disposition", "attachment;filename=LeaveReport.csv");
            //    Response.Write(sb.ToString());
            //    // HttpContext.Current.ApplicationInstance.CompleteRequest();
            //    Response.End();
            //}
            //else
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select sales person');", true);
            //    leavereportrpt.DataSource = null;
            //    leavereportrpt.DataBind();
            //}


            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=LeaveReport.csv");
            string headertext = "Name".TrimStart('"').TrimEnd('"') + "," + "Sync Id".TrimStart('"').TrimEnd('"') + "," + "For Days".TrimStart('"').TrimEnd('"') + "," + "From Date".TrimStart('"').TrimEnd('"') + "," + "To Date".TrimStart('"').TrimEnd('"') + "," + "Reason".TrimStart('"').TrimEnd('"') + "," + "Status".TrimStart('"').TrimEnd('"') + "," + "Approved By".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Name", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ForDays", typeof(String)));
            dtParams.Columns.Add(new DataColumn("FromDate", typeof(String)));

            dtParams.Columns.Add(new DataColumn("ToDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Reason", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Status", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ApprovedBy", typeof(String)));
            //
            foreach (RepeaterItem item in leavereportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label smNameLabel = item.FindControl("smnameLabel") as Label;
                dr["Name"] = smNameLabel.Text;
                Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                dr["SyncId"] = syncIdLabel.Text.ToString();
                Label nofdaysLabel = item.FindControl("nofdaysLabel") as Label;
                dr["ForDays"] = nofdaysLabel.Text.ToString();
                Label fromDateLabel = item.FindControl("fromDateLabel") as Label;
                dr["FromDate"] = fromDateLabel.Text.ToString();

                Label todateLabel = item.FindControl("todateLabel") as Label;
                dr["ToDate"] = todateLabel.Text.ToString();
                Label reasonLabel = item.FindControl("reasonLabel") as Label;
                dr["Reason"] = reasonLabel.Text.ToString();
                Label appstatusLabel = item.FindControl("appstatusLabel") as Label;
                dr["Status"] = appstatusLabel.Text.ToString();
                Label appbynameLabel = item.FindControl("appbynameLabel") as Label;
                dr["ApprovedBy"] = appbynameLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 3)
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
                        if (k == 3)
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
                        if (k == 3 || k == 4)
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
            Response.AddHeader("content-disposition", "attachment;filename=LeaveReport.csv");
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();
            sb.Clear();
            dtParams.Dispose();

            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=LeaveReport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //leavereportrpt.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }
}