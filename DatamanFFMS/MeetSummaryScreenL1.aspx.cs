using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.IO;

namespace AstralFFMS
{
    public partial class MeetSummaryScreenL1 : System.Web.UI.Page
    {
        BAL.MeetTarget.MeetTargetBAL MT = new BAL.MeetTarget.MeetTargetBAL();
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if(!IsPostBack)
            {//Ankita - 18/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                fillfyear();
               // fillUnderUsers();
               // fill_TreeArea();               
               // fillUserType();
                //GridView1.DataSource = null;
                //GridView1.DataBind();
                BindTreeViewControl();
            }
        }

        /////// Abhishek jaiswal 23may2017 start--------------------------------------------------------
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
        /////// Abhishek jaiswal 23may2017 End--------------------------------------------------------

        private void fillfyear()
        {//Ankita - 18/may/2016- (For Optimization)
            //string str = "select * from Financialyear ";
            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                txtcurrentyear.DataSource = dt;
                txtcurrentyear.DataTextField = "Yr";
                txtcurrentyear.DataValueField = "id";
                txtcurrentyear.DataBind();
            }
            txtcurrentyear.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillrpt(string meetType)
        {
            string IDStr = "", IDStr1 = "";
            foreach (TreeNode node in trview.CheckedNodes)
            {
                IDStr1 = node.Value;
                {
                    IDStr += node.Value + ",";
                }
            }
            IDStr1 = IDStr.TrimStart(',').TrimEnd(',');
//            string str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*,MAR.AreaName as Location from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.AreaId left join MastParty MP on m.RetailerPartyID=MP.PartyId 
//  	left join MastItemClass MI on m.IndId=MI.Id
//
//	left join MastArea MAR on MAR.AreaId=M.MeetLoaction where  M.SMId=" + ddlunderUser.SelectedValue + " and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and MeetTypeId=" + Settings.DMInt32(meetType);

            string str = @"select MI.Name as IndName,MP.PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*,MAR.AreaName as Location from TransMeetPlanEntry M   left join MastArea on MastArea.AreaId=M.AreaId left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id

	left join MastArea MAR on MAR.AreaId=M.MeetLoaction where  M.SMId in (" + IDStr1 + ") and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and MeetTypeId=" + Settings.DMInt32(meetType);

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                lblPartTypeName.Text = "";
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }
        private void fillUnderUsers()
        {
            if (roleType == "Admin")
            {//Ankita - 18/may/2016- (For Optimization)
                //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                DataTable dtcheckrole = new DataTable();
                dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                DataView dv1 = new DataView(dtcheckrole);
                dv1.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                dv1.Sort = "SMName asc";

                ddlunderUser.DataSource = dv1.ToTable();
                ddlunderUser.DataTextField = "SMName";
                ddlunderUser.DataValueField = "SMId";
                ddlunderUser.DataBind();
            }
            else
            {
                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                if (d.Rows.Count > 0)
                {
                    try
                    {
                        DataView dv = new DataView(d);
                        dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                        ddlunderUser.DataSource = dv;
                        ddlunderUser.DataTextField = "SMName";
                        ddlunderUser.DataValueField = "SMId";
                        ddlunderUser.DataBind();
                        //Ankita - 20/may/2016- (For Optimization)
                        //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                        //DataTable dtRole = new DataTable();
                        //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                        //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                        string RoleTy = Settings.Instance.RoleType;
                        if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                        {
                            ddlunderUser.SelectedValue = Settings.Instance.SMID;
                        }
                    }
                    catch { }

                }
            }
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
            {
                //Ankita - 18/may/2016- (For Optimization)
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


        private void fillUserType()
        {
            string s = "select * from MastMeetType";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
              //  GridView2.DataSource = null;
               // GridView2.DataBind();

               // btnsave.Visible = false;
              //  lblPartTypeName.Visible = false;
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
        protected void txtcurrentyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtcurrentyear.SelectedIndex > 0)
            {
                fillUserType();
              //  lblPartTypeName.Visible = false;
               // GridView2.DataSource = null;
               // GridView2.DataBind();
            }
            else
            {
              //  lblPartTypeName.Visible = false;
                GridView1.DataSource = null;
                GridView1.DataBind();
                rpt.DataSource = null;
                rpt.DataBind();
               // GridView2.DataSource = null;
               // GridView2.DataBind();
            }
        }

        private int GetSalesPerId(int uid)
        {
            string st = "select SMId from MastSalesRep where UserId=" + uid;
            int SID = Settings.DMInt32(Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st)));
            return SID;


        }
        private DataTable GetSUnderPerSMId(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
        }

        private DataTable GetSUnderPerSMId1(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);
            string strqw = "select * from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);
            return ds;
        }
        private DataTable GetSUnderPerSMId2(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);
            return ds1;
        }

        private DataTable GetSUnderPerSMId6(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            string r = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);

            for (int k = 0; k < ds1.Rows.Count; k++)
            {
                r += ds1.Rows[k]["SMId"].ToString() + ",";
            }
            r = r.Remove(r.Length - 1);

            string strqw2 = "select SMId from MastSalesRep where UnderId in (" + r + ")";
            DataTable ds2 = new DataTable();
            ds2 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw2);

            return ds2;



        }
        private DataTable GetSUnderPerSMId7(int uid)
        {
            int s1 = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s1;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            string r = "";
            string s = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);

            for (int k = 0; k < ds1.Rows.Count; k++)
            {
                r += ds1.Rows[k]["SMId"].ToString() + ",";
            }
            r = r.Remove(r.Length - 1);

            string strqw2 = "select SMId from MastSalesRep where UnderId in (" + r + ")";
            DataTable ds2 = new DataTable();
            ds2 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw2);


            for (int l = 0; l < ds1.Rows.Count; l++)
            {
                s += ds2.Rows[l]["SMId"].ToString() + ",";
            }
            s = s.Remove(r.Length - 1);

            string strqw3 = "select SMId from MastSalesRep where UnderId in (" + s + ")";
            DataTable ds3 = new DataTable();
            ds3 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw3);

            return ds3;
        }

        private int GetSalesPerUserId(int uid)
        {
            string st = "select UserId from MastSalesRep where SMId=" + uid;
            int SID = Settings.DMInt32(Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st)));
            return SID;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lk = (Label)e.Row.Cells[0].FindControl("lblTargetFromHo");
                Label lnkTarget = (Label)e.Row.FindControl("lnkTarget");
                Label lblRejected = (Label)e.Row.FindControl("lblRejected");
                Label lblActualmeet = (Label)e.Row.FindControl("lblActualmeet");
                Label lblexpenses = (Label)e.Row.FindControl("lblexpenses");
                Label lblpendingforapproval = (Label)e.Row.FindControl("lblpendingforapproval");
                Label lblremaining = (Label)e.Row.FindControl("lblremaining");
                
                HiddenField hid = (HiddenField)e.Row.Cells[0].FindControl("hidPartyTypeId");

                string IDStr = "", IDStr1 = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    IDStr1 = node.Value;
                    {
                        IDStr += node.Value + ",";
                    }
                }
                IDStr1 = IDStr.TrimStart(',').TrimEnd(',');

                //DataTable d2 = Settings.UnderUsers(ddlunderUser.SelectedValue);
                DataTable d2 = Settings.FindunderUsers(IDStr1);
                DataView dv2 = new DataView(d2);
                //dv2.RowFilter = "RoleType='AreaIncharge' or RoleType = 'DistrictHead'";

                DataTable gauravDT1 = dv2.ToTable();

                string sm = "";
                for (int i = 0; i < gauravDT1.Rows.Count; i++)
                {
                    sm += gauravDT1.Rows[i]["SmId"].ToString() + ",";
                }
                string sm1 = sm.TrimStart(',').TrimEnd(',');

                if (lk != null)
                {
                    //string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMID=" +ddlunderUser.SelectedValue + "  and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMID in (" + IDStr1 + ")  and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr = new DataTable();
                    dtr = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    if (dtr.Rows.Count > 0)
                    {
                        try
                        {
                            object sum1 = dtr.Compute("Sum(JanValue)", "");
                            object sum2 = dtr.Compute("Sum(FebValue)", "");
                            object sum3 = dtr.Compute("Sum(MarValue)", "");
                            object sum4 = dtr.Compute("Sum(AprValue)", "");
                            object sum5 = dtr.Compute("Sum(MayValue)", "");
                            object sum6 = dtr.Compute("Sum(JunValue)", "");
                            object sum7 = dtr.Compute("Sum(JulValue)", "");
                            object sum8 = dtr.Compute("Sum(AugValue)", "");
                            object sum9 = dtr.Compute("Sum(SepValue)", "");
                            object sum10 = dtr.Compute("Sum(OctValue)", "");
                            object sum11 = dtr.Compute("Sum(NovValue)", "");
                            object sum12 = dtr.Compute("Sum(DecValue)", "");
                            decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                            lk.Text = df.ToString();
                        }
                        catch
                        {
                            lk.Text = "0";
                        }

                    }
                    else
                    {
                         lk.Text = "0";
                    }

                }
                if (lnkTarget != null)
                {

                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;
                    //Ankita - 18/may/2016- (For Optimization)
                    string s = @"select count(*) from TransMeetPlanEntry where SMId In (" + sm1 + ") and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and MeetTypeId=" + hid.Value + "";
                    //string s = @"select * from TransMeetPlanEntry where SMId In (" + sm1 + ") and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and MeetTypeId=" + hid.Value + "";
                    int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,s));
                  //  DataTable dtr2 = new DataTable();
                   // dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    try
                    {

                        //lnkTarget.Text = dtr2.Rows.Count.ToString();
                        lnkTarget.Text = val.ToString();
                    }

                    catch
                    {
                        lnkTarget.Text = "0";
                    }

                }
                if (lblRejected != null)
                {

                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;
                    //Ankita - 18/may/2016- (For Optimization)
                    string s = @"select count(*) from TransMeetPlanEntry where AppStatus='Reject' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and  SMId In (" + sm1 + ") and MeetTypeId=" + hid.Value + "";
                    //string s = @"select * from TransMeetPlanEntry where AppStatus='Reject' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and  SMId In (" + sm1 + ") and MeetTypeId=" + hid.Value + "";
                    //DataTable dtr2 = new DataTable();
                    //dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
                    try
                    {
                        lblRejected.Text = val.ToString();
                      //  lblRejected.Text = dtr2.Rows.Count.ToString();
                    }

                    catch
                    {
                        lblRejected.Text = "0";
                    }

                }

                if (lblActualmeet != null)
                {

                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;
                    //Ankita - 18/may/2016- (For Optimization)
                    string s = @"select count(*) from TransMeetPlanEntry where MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and AppStatus='Approved' and SMId In (" + sm1 + ") and MeetTypeId=" + hid.Value + "";
                    //string s = @"select * from TransMeetPlanEntry where MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and AppStatus='Approved' and SMId In (" + sm1 + ") and MeetTypeId=" + hid.Value + "";
                    //DataTable dtr2 = new DataTable();
                    //dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
                    try
                    {
                        lblActualmeet.Text = val.ToString();
                       // lblActualmeet.Text = dtr2.Rows.Count.ToString();
                    }

                    catch
                    {
                        lblActualmeet.Text = "0";
                    }

                }

                if (lblexpenses != null)
                {
                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;
                    string s = @"select sum(AppAmount) from TransMeetPlanEntry where  MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and  SMId In (" + sm1 + ") and MeetTypeId=" + hid.Value + "";
                    string bug = "";
                    bug =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
                    try
                    {

                        lblexpenses.Text = bug;
                    }

                    catch
                    {
                        lblexpenses.Text = "0";
                    }

                }

                if (lblpendingforapproval != null)
                {

                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;
                    //Ankita - 18/may/2016- (For Optimization)
                    string s = @"select count(*) from TransMeetPlanEntry where  AppStatus='Pending' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId In (" + sm1 + ") and MeetTypeId=" + hid.Value + "";
                    //string s = @"select * from TransMeetPlanEntry where  AppStatus='Pending' and MeetDate>='" + Settings.GetStartFinancialYearfrmDate(txtcurrentyear.SelectedItem.Text) + "' and MeetDate<='" + Settings.GetStartFinancialYearToDate(txtcurrentyear.SelectedItem.Text) + "' and SMId In (" + sm1 + ") and MeetTypeId=" + hid.Value + "";
                    //DataTable dtr2 = new DataTable();
                    //dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
                    try
                    {
                        lblpendingforapproval.Text = val.ToString();
                      //  lblpendingforapproval.Text = dtr2.Rows.Count.ToString();
                    }

                    catch
                    {
                        lblpendingforapproval.Text = "0";
                    }

                }

                lblremaining.Text = Convert.ToString(Settings.DMDecimal(lk.Text) - Settings.DMDecimal(lblActualmeet.Text));

            }

        }
        


        private int LoginUserLevel()
        {
            int lvl = 0;
            string str = "select Lvl  from MastSalesRep where UserId=" + Settings.Instance.UserID;
            DataTable LVLDT = new DataTable();
            LVLDT = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (LVLDT.Rows.Count > 0)
            {
                lvl = Convert.ToInt32(LVLDT.Rows[0][0].ToString()) + 1;
            }
            return lvl;

        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MeetEdit")
            {

                GridView2.DataSource = null;
                GridView2.DataBind();
                string str = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + e.CommandArgument.ToString();
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }

                this.ModalPopupExtender1.Show();
            }
           
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Meet")
            {
                string str1 = "select Name,Id from MastMeetType where Id=" + e.CommandArgument.ToString();
                DataTable PartyDT = new DataTable();
                PartyDT = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                lblPartTypeName.Text = PartyDT.Rows[0][0].ToString();
                lblPartTypeID.Text = PartyDT.Rows[0][1].ToString();
                fillrpt(PartyDT.Rows[0][1].ToString());
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetSummaryScreenL1.aspx");
        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (txtcurrentyear.SelectedIndex > 0)
            {
                string IDStr = "", IDStr1 = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    IDStr1 = node.Value;
                    {
                        IDStr += node.Value + ",";
                    }
                }
                IDStr1 = IDStr.TrimStart(',').TrimEnd(',');
                //Added on 30-05-2016 - Abhishek
                if (IDStr1 == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Salesperson');", true);
                    return;
                }
                //End
                else
                {
                    fillUserType();
                }
                trview.Enabled = false;
               // ddlunderUser.Enabled = false;
                txtcurrentyear.Enabled = false;
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                rpt.DataSource = null;
                rpt.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Year');", true);
            }
        }

        protected void btncancel1_Click(object sender, EventArgs e)
        {
            trview.Enabled = true;
           // ddlunderUser.Enabled = true;
            txtcurrentyear.SelectedIndex = 0;
            txtcurrentyear.Enabled = true;
            ddlunderUser.SelectedIndex = 0;

            GridView1.DataSource = null;
            GridView1.DataBind();
            rpt.DataSource = null;
            rpt.DataBind();
        }

     

    }
}