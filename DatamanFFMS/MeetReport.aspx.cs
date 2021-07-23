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
using System.Drawing;
using System.Text;
using System.Net;

namespace AstralFFMS
{
    public partial class MeetReport : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                txtmDate.Text = DateTime.Parse(DateTime.UtcNow.AddMonths(-1).ToShortDateString()).ToString("dd/MMM/yyyy");
                txttodate.Text = DateTime.Parse(DateTime.UtcNow.ToShortDateString()).ToString("dd/MMM/yyyy");
                //Ankita - 18/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                fillmeetType();
                //filllevel1();
               // fill_TreeArea();
                BindTreeViewControl();
                Repeater1.Visible = false;
                lblnouser.Visible = false;
                btnNOUExport.Visible = false;
                lblproductist.Visible = false;
                //string pageName = Path.GetFileName(Request.Path);

                if (btnexport.Text == "Export")
                {
                    btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnexport.CssClass = "btn btn-primary";
                    btnNOUExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnNOUExport.CssClass = "btn btn-primary";
                }
            }
        }

        private void fillmeetType()
        { //Ankita - 18/may/2016- (For Optimization)
            //string query = "select * from MastMeetType";
            string query = "select Id,Name from MastMeetType";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void filllevel1()
        {
            //DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            //if (d.Rows.Count > 0)
            //{
            //    try
            //    {
            //        DataView dv = new DataView(d);
            //        dv.RowFilter = "RoleName='Level 1'";
            //        if (dv.Table.Rows.Count > 0)
            //        {
            //            ddllevel1.DataSource = dv;
            //            ddllevel1.DataTextField = "SMName";
            //            ddllevel1.DataValueField = "SMId";
            //            ddllevel1.DataBind();
            //        }
            //        ddllevel1.Items.Insert(0, new ListItem("-- Select --", "0"));
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
            try
            {
                if (roleType == "Admin")
                {//Ankita - 18/may/2016- (For Optimization)
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    DataTable dtcheckrole = new DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";

                    ddllevel1.DataSource = dv1.ToTable();
                    ddllevel1.DataTextField = "SMName";
                    ddllevel1.DataValueField = "SMId";
                    ddllevel1.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ddllevel1.DataSource = dv.ToTable();
                        ddllevel1.DataTextField = "SMName";
                        ddllevel1.DataValueField = "SMId";
                        ddllevel1.DataBind();
                        //Ankita - 20/may/2016- (For Optimization)
                        //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                        //DataTable dtRole = new DataTable();
                        //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                        //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                        string RoleTy = Settings.Instance.RoleType;
                        if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                        {
                            ddllevel1.SelectedValue = Settings.Instance.SMID;
                        }
                    }
                    
                }
                ddllevel1.Items.Insert(0, new ListItem("-- Select --", "0"));

            }
            catch (Exception ex)
            {
                ex.ToString();
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

        private void fillMeetPLan()
        {
            try
            {

                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtmDate.Text))
                {
                    string sm = "", smIDStr1 = "", smIDStr = "";
                    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                    for (int i = 0; i < d.Rows.Count; i++)
                    {
                        sm += d.Rows[i]["SmId"].ToString() + ",";
                    }
                    string sm1 = sm.TrimStart(',').TrimEnd(',');

                    foreach (TreeNode node in trview.CheckedNodes)
                    {
                        smIDStr1 = node.Value;
                        {
                            smIDStr += node.Value + ",";
                        }
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

                    string str = "";
                    if (smIDStr1 != "")
                    {
                        if (ddlstatus.SelectedIndex > 0 && trview.CheckedNodes.Count > 0 && ddlmeetType.SelectedIndex > 0)
                        {                           
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId,MP.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct ,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId]  left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id  left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
                             where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlstatus.SelectedIndex > 0 && ddlmeetType.SelectedIndex > 0)
                        {                          
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId,MPD.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id  left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
 where MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (trview.CheckedNodes.Count > 0 && ddlmeetType.SelectedIndex > 0)
                        {                         
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId,MPD.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id  left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
 where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlstatus.SelectedIndex > 0 && trview.CheckedNodes.Count > 0)
                        {                            
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId,MPD.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
 where M.SMId in (SELECT smid FROM mastsalesrepgrp  WHERE  maingrp IN (" + smIDStr1 + ")) and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlstatus.SelectedIndex > 0)
                        {                            
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where  M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId,MPD.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
  where  M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (trview.CheckedNodes.Count > 0)
                        {                           
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId, MPD.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id  left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
 where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlmeetType.SelectedIndex > 0)
                        {                            
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId,MPD.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId  left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id  left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
 where MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else
                        {                           
//                            str = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,m.typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,dbo.getMeetusercount(m.MeetPlanId) AS Actualuser,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId where [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "' and  M.SMID in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))";
                            str = @"SELECT distinct (m.MeetPlanId) AS MeetPlanId,MPD.PartyId, MPD.PartyName + '-' + [city].areaName + '-' +[state].areaName 
                                       AS Distributor1,MST.SMName,m.MeetDate,m.MeetName,m.Venue,dbo.getPartName(m.MeetPlanId) as PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,mct.name as CityType,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty,case when mig.imgname is null then 'No' else 'Yes' end as imageupload
                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join TransMeetImage mig on m.meetplanid=mig.meetplanid left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastCityType mct on mct.id=MastArea.Citytype left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id  left join mastarea as [city] on [city].areaid=MPD.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
 where [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "' and  M.SMID in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))";
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select salesperson.');", true);
                    }

                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt.Rows.Count > 0)
                    {
                        rpt.DataSource = dt;
                        rpt.DataBind();
                    }
                    else
                    {
                        rpt.DataSource = null;
                        rpt.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            catch { }
        }
        
        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MeetEdit")
            {
                Repeater1.Visible = true;
                Repeater1.DataSource = null;
                Repeater1.DataBind();
                rptnoofusers.DataSource = null;
                rptnoofusers.DataBind();
                rptproductList.DataSource = null;
                rptproductList.DataBind();
                lblnouser.Visible = false;
                lblproductist.Visible = false;
                lblproductist.Visible = false;
                string str = @"select MI.Name as IndName,dbo.getPartName(m.MeetPlanId) as PartyName, M.FromTime,M.ToTime,M.MeetDate,M.MeetName,MastArea.AreaName,M.MeetPlanId,M.*
 from TransMeetPlanEntry M 
  left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId 
  	left join MastItemClass MI on m.IndId=MI.Id  where  M.MeetPlanId=" + e.CommandArgument.ToString();
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();

                }

                string str1 = "  select l.*,MST.SMName,E.MeetDate,E.MeetName,ma.AreaName,mb.AreaName as BeatName from TransAddMeetUser l left join TransMeetPlanEntry E on l.[MeetId]=e.MeetPlanId  left join PartyType P on l.PartyType=p.PartyTypeId Left Join MastSalesRep MST on MST.SMId = E.SMId  Left Join MastArea ma on l.AreaId=ma.AreaId     Left Join MastArea mb on l.AreaId=mb.AreaId  where l.MeetId=" + Settings.DMInt32(e.CommandArgument.ToString()) + " order by l.ContactPersonName";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

//                string str1 = @"SELECT MST.SMName,m.MeetPlanId,m.MeetDate,m.MeetName,m.Venue,MP.PartyName,m.NoOfUser,mmtg.Name as typeOfGiftEnduser,m.ExpShareDist,m.ExpShareSelf,MI.Name as IndName,m.LambBudget,m.AppAmount,me.ExpenseApprovedAmount,me.ExpenseApprovedRemark,me.FinalApprovedAmount,me.FinalApprovedRemark,m.AppStatus,m.AppRemark,m.Appdate,m.MeetTypeId,dbo.getproduct(m.MeetPlanId) AS meetproduct,MPD.PartyName as Distributor,MPD.[SyncId] as DisSyncld,MST.[SyncId] as SAPCode,MastArea.AreaName as City,MastArea.[SyncId] as CityCode,MST.SMName as SalesPerson,MMT.Name as MeetTypeName,m.[NoStaff],m.[Comments],mb.AreaName as BeatName,m.[valueofRetailer],dbo.getActualusercount(m.MeetPlanId) as Actualusercount,m.[valueofRetailer]-dbo.getActualusercount(m.MeetPlanId) as BalanceQty
//                              from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id WHERE  [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
//                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                if (dt1.Rows.Count > 0)
                {
                    rptnoofusers.DataSource = dt1;
                    rptnoofusers.DataBind();
                    lblproductist.Visible = true;
                    btnNOUExport.Visible = true;
                    lblnouser.Visible = true;

                }

                string str2 = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + Settings.DMInt32(e.CommandArgument.ToString());
                DataTable dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str2);
                if (dt2.Rows.Count > 0)
                {
                    rptproductList.DataSource = dt2;
                    rptproductList.DataBind();
                    lblproductist.Visible = true;
                }

                string str3 = "  select l.*,E.MeetName as Meet from TransMeetImage l left join TransMeetPlanEntry E on l.[MeetPlanID]=E.MeetPlanId  where l.MeetPlanID=" + Settings.DMInt32(e.CommandArgument.ToString()) + " order by E.MeetName";
                DataTable dt3 = DbConnectionDAL.GetDataTable(CommandType.Text, str3);
                if (dt3.Rows.Count > 0)
                {
                    rptImageUpload.DataSource = dt3;
                    rptImageUpload.DataBind();
                   // lblproductist.Visible = true;
                }

            }
        }
        protected void rptImageUpload_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ShowImage")
            {
                try
                {
                    Control control;
                    control = e.Item.FindControl("ImageId");
                    string str = string.Format("Select imgUrl,imgName from TransMeetImage Where Id={0}", e.CommandArgument.ToString());
                    DataTable getdata = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    string url = Convert.ToString(getdata.Rows[0]["imgUrl"]);
                    string imgName = Convert.ToString(getdata.Rows[0]["imgName"]);
                    Response.ContentType = "image/jpg";
                    string filePath = url;
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + imgName + "\"");
                    Response.TransmitFile(Server.MapPath(filePath));
                    Response.End();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
               
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillMeetPLan();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        //public void ExportToExecl(object DS, string fileName)
        //{
        //    fileName += ".xls";
        //    string worksheetName = fileName;
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "");
        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        //    StringWriter stringWriter = new StringWriter();
        //    HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);

        //  //  htmlWrite.WriteLine("<b><u><font size='4'><text-align='center'> " + "OLAM" + "</br>");
        //   // htmlWrite.WriteLine("<b><u><font size='3'><horizontalalign='center'> " + "CourseName" + "");
        //    DataGrid dataExportExcel = new DataGrid();
        //   // dataExportExcel.ItemDataBound += new DataGridItemEventHandler(dataExportExcel_ItemDataBound);
        //    dataExportExcel.DataSource = DS;
        //    dataExportExcel.DataBind();
        //    dataExportExcel.RenderControl(htmlWrite);
        //    StringBuilder sbResponseString = new StringBuilder();
        //    sbResponseString.Append("<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"> <head><meta http-equiv=\"Content-Type\" content=\"text/html;charset=windows-1252\"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>" + worksheetName + "</x:Name><x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head> <body>");
        //    sbResponseString.Append(stringWriter + "</body></html>");

        //    HttpContext.Current.Response.Write(sbResponseString.ToString());
        //    HttpContext.Current.Response.End();
        //}


        protected void btnexport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=MeetReport.csv");
            string headertext = "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Sales Person Code".TrimStart('"').TrimEnd('"') + "," + "Meet Date".TrimStart('"').TrimEnd('"') + "," + "MeetName".TrimStart('"').TrimEnd('"') + "," + "Venue".TrimStart('"').TrimEnd('"') + "," + "Party Name".TrimStart('"').TrimEnd('"') + "," + "Distributor".TrimStart('"').TrimEnd('"') + "," + "Distributor Code".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "City Code".TrimStart('"').TrimEnd('"') + "," + "City Type".TrimStart('"').TrimEnd('"') + "," + "Planned Users".TrimStart('"').TrimEnd('"') + "," + "Type Of Gift".TrimStart('"').TrimEnd('"') + "," + "Distributor Sharing %".TrimStart('"').TrimEnd('"') + "," + "Astral Sharing %".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Approx Budget".TrimStart('"').TrimEnd('"') + "," + "Approved Amount".TrimStart('"').TrimEnd('"') + "," + "Expense Amount".TrimStart('"').TrimEnd('"') + "," + "Expense Remark".TrimStart('"').TrimEnd('"') + "," + "Expense Approved Amount".TrimStart('"').TrimEnd('"') + "," + "Expense Approved Remarks".TrimStart('"').TrimEnd('"') + "," + "Meet Status".TrimStart('"').TrimEnd('"') + "," + "Approval Remark".TrimStart('"').TrimEnd('"') + "," + "Approval Date".TrimStart('"').TrimEnd('"') + "," + "Meet Type".TrimStart('"').TrimEnd('"') + "," + "No Of Staff".TrimStart('"').TrimEnd('"') + "," + "Comments".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Qty Required".TrimStart('"').TrimEnd('"') + "," + "Actual User Count".TrimStart('"').TrimEnd('"') + "," + "Balance Qty".TrimStart('"').TrimEnd('"') + "," + "Image Upload".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SalesPersonCode", typeof(String)));
            dtParams.Columns.Add(new DataColumn("MeetDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("MeetName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Venue", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));

            dtParams.Columns.Add(new DataColumn("Distributor", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DistributorCode", typeof(String)));
            dtParams.Columns.Add(new DataColumn("City", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CityCode", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CityType", typeof(String)));

            dtParams.Columns.Add(new DataColumn("NoofUsers", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TypeOfGift", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpShareDist", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpShareSelf", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProductClass", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ApproxBudget", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ApprovedAmount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseAmount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseRemark", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseApprovedAmount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ExpenseApprovedRemarks", typeof(String)));
            dtParams.Columns.Add(new DataColumn("MeetStatus", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Approval/CancelRemark", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Approval/CancelDate", typeof(String)));

            dtParams.Columns.Add(new DataColumn("MeetTypeName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("NoStaff", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Comments", typeof(String)));
            dtParams.Columns.Add(new DataColumn("BeatName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("valueofRetailer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Actualusercount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("BalanceQty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ImageUpload", typeof(String)));
            foreach (RepeaterItem item in rpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label SMNameLabel = item.FindControl("SMNameLabel") as Label;
                dr["SalesPerson"] = SMNameLabel.Text;
                Label SAPCodeLabel = item.FindControl("SAPCodeLabel") as Label;
                dr["SalesPersonCode"] = SAPCodeLabel.Text;
                Label MeetDateLabel = item.FindControl("MeetDateLabel") as Label;
                dr["MeetDate"] = MeetDateLabel.Text.ToString();
                Label MeetNameLabel = item.FindControl("MeetNameLabel") as Label;
                dr["MeetName"] = MeetNameLabel.Text.ToString();
                Label VenueLabel = item.FindControl("VenueLabel") as Label;
                dr["Venue"] = VenueLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label PartyNameLabel = item.FindControl("PartyNameLabel") as Label;
                dr["PartyName"] = PartyNameLabel.Text.ToString();
                Label NoOfUserLabel = item.FindControl("NoOfUserLabel") as Label;

                Label DistributorLabel = item.FindControl("DistributorLabel") as Label;
                dr["Distributor"] = DistributorLabel.Text.ToString();
                Label DisSyncldLabel = item.FindControl("DisSyncldLabel") as Label;
                dr["DistributorCode"] = DisSyncldLabel.Text.ToString();
                Label CityLabel = item.FindControl("CityLabel") as Label;
                dr["City"] = CityLabel.Text.ToString();
                Label CityCodeLabel = item.FindControl("CityCodeLabel") as Label;
                dr["CityCode"] = CityCodeLabel.Text.ToString();
                Label lblCityType = item.FindControl("lblCityType") as Label;
                dr["CityType"] = lblCityType.Text.ToString();

                dr["NoofUsers"] = NoOfUserLabel.Text.ToString();
                Label typeOfGiftEnduserLabel = item.FindControl("typeOfGiftEnduserLabel") as Label;
                dr["TypeOfGift"] = typeOfGiftEnduserLabel.Text.ToString();

                Label Label1 = item.FindControl("Label1") as Label;
                dr["ExpShareDist"] = Label1.Text.ToString();
                Label Label2 = item.FindControl("Label2") as Label;
                dr["ExpShareSelf"] = Label2.Text.ToString();
                //Label IndNameLabel = item.FindControl("IndNameLabel") as Label;
                //dr["ProductClass"] = IndNameLabel.Text.ToString();
                Label meetproductLabel = item.FindControl("meetproductLabel") as Label;
                dr["ProductClass"] = meetproductLabel.Text.ToString();
                Label LambBudgetLabel = item.FindControl("LambBudgetLabel") as Label;
                dr["ApproxBudget"] = LambBudgetLabel.Text.ToString();
                Label AppAmountLabel = item.FindControl("AppAmountLabel") as Label;
                dr["ApprovedAmount"] = AppAmountLabel.Text.ToString();
                Label ExpenseApprovedAmountLabel = item.FindControl("ExpenseApprovedAmountLabel") as Label;
                dr["ExpenseAmount"] = ExpenseApprovedAmountLabel.Text.ToString();
                Label ExpenseApprovedRemarkLabel = item.FindControl("ExpenseApprovedRemarkLabel") as Label;
                dr["ExpenseRemark"] = ExpenseApprovedRemarkLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label FinalApprovedAmountLabel = item.FindControl("FinalApprovedAmountLabel") as Label;
                dr["ExpenseApprovedAmount"] = FinalApprovedAmountLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label FinalApprovedRemarkLabel = item.FindControl("FinalApprovedRemarkLabel") as Label;
                dr["ExpenseApprovedRemarks"] = FinalApprovedRemarkLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label MeetStatus_label = item.FindControl("lblAppStatus") as Label;
                dr["MeetStatus"] = MeetStatus_label.Text.ToString();
                Label AppRemarkLabel = item.FindControl("AppRemarkLabel") as Label;
                dr["Approval/CancelRemark"] = AppRemarkLabel.Text.ToString();   
                Label AppdateLabel = item.FindControl("AppdateLabel") as Label;
                dr["Approval/CancelDate"] = AppdateLabel.Text.ToString();

                Label MeetTypeNameLabel = item.FindControl("MeetTypeNameLabel") as Label;
                dr["MeetTypeName"] = MeetTypeNameLabel.Text.ToString();
                Label NoStaffLabel = item.FindControl("NoStaffLabel") as Label;
                dr["NoStaff"] = NoStaffLabel.Text.ToString();
                Label CommentsLabel = item.FindControl("CommentsLabel") as Label;
                dr["Comments"] = CommentsLabel.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label BeatNameLabel = item.FindControl("BeatNameLabel") as Label;
                dr["BeatName"] = BeatNameLabel.Text.ToString();
                Label valueofRetailerLabel = item.FindControl("valueofRetailerLabel") as Label;
                dr["valueofRetailer"] = valueofRetailerLabel.Text.ToString();
                Label ActualusercountLabel = item.FindControl("ActualusercountLabel") as Label;
                dr["Actualusercount"] = ActualusercountLabel.Text.ToString();
                Label BalanceQtyLabel = item.FindControl("BalanceQtyLabel") as Label;
                dr["BalanceQty"] = BalanceQtyLabel.Text.ToString();
                Label lblImageUpload = item.FindControl("lblImageUpload") as Label;
                dr["imageupload"] = lblImageUpload.Text.ToString();
                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 2)
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
                        if (k == 2)
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
                        if (k == 2)
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
            Response.AddHeader("content-disposition", "attachment;filename=MeetReport.csv");
            Response.Write(sb.ToString());
            Response.End();

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=MeetReport.xls");
            //Response.ContentType = "applicatio/excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htm = new HtmlTextWriter(sw);

            //StringWriter sw1 = new StringWriter();
            //HtmlTextWriter htm1 = new HtmlTextWriter(sw1);

            //StringWriter sw2 = new StringWriter();
            //HtmlTextWriter htm2 = new HtmlTextWriter(sw2);

            //Repeater MeetReport = this.Repeater1;
            //Repeater MeetReport1 = this.rptnoofusers;
            //Repeater MeetReport2 = this.rptproductList;

            //MeetReport.RenderControl(htm);
           
            //MeetReport1.RenderControl(htm1);
            //MeetReport2.RenderControl(htm2);

            //Response.Write("<b><u><font size='4'><horizontalalign='center'>Meet Name");
            //Response.Write("<font size='4'><horizontalalign='center'>"+sw.ToString());

            //if (rptnoofusers.Items.Count>0)
            //{
            //    Response.Write("<b><u><font size='4'><horizontalalign='center'>Meet User's List");
            //}

            //Response.Write("<font size='4'><horizontalalign='center'>"+sw1.ToString());

            //if (rptproductList.Items.Count > 0)
            //{
            //    Response.Write("<b><u><font size='4'><horizontalalign='center'>Meet Product List");
            //}

            //Response.Write("<font size='4'><horizontalalign='center'>" + sw2.ToString());

            //Response.End();
        } 
        protected void btnNouexport_Click(object sender, EventArgs e)
        {

            try
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=NoOfUser.csv");
                string headertext = "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Meet Date".TrimStart('"').TrimEnd('"') + "," + "Meet Name".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Contact Person".TrimStart('"').TrimEnd('"') + "," + "Mobile No".TrimStart('"').TrimEnd('"') + "," + "EmailId".TrimStart('"').TrimEnd('"') + "," + "Potential".TrimStart('"').TrimEnd('"') + "," + "DOB".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;
                //
                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("SMName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("MeetDate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("MeetName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("AreaName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("BeatName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("ContactPersonName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("MobileNo", typeof(String)));
                dtParams.Columns.Add(new DataColumn("EmailId", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Potential", typeof(String)));
                dtParams.Columns.Add(new DataColumn("DOB", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
                foreach (RepeaterItem item in rptnoofusers.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label SMNam = item.FindControl("SMName") as Label;
                    dr["SMName"] = SMNam.Text;
                    Label MeetDate = item.FindControl("MeetDate") as Label;
                    dr["MeetDate"] = MeetDate.Text;
                    Label MeetName = item.FindControl("MeetName") as Label;
                    dr["MeetName"] = MeetName.Text;
                    Label AreaName = item.FindControl("AreaName") as Label;
                    dr["AreaName"] = AreaName.Text;
                    Label BeatName = item.FindControl("BeatName") as Label;
                    dr["BeatName"] = BeatName.Text;
                    Label Name = item.FindControl("Name") as Label;
                    dr["PartyName"] = Name.Text;
                    Label ContactPersonName = item.FindControl("ContactPersonName") as Label;
                    dr["ContactPersonName"] = ContactPersonName.Text;
                    Label MobileNo = item.FindControl("MobileNo") as Label;
                    dr["MobileNo"] = MobileNo.Text;
                    Label EmailId = item.FindControl("EmailId") as Label;
                    dr["EmailId"] = EmailId.Text;
                    Label Potential = item.FindControl("Potential") as Label;
                    dr["Potential"] = Potential.Text;
                    Label DOB = item.FindControl("DOB") as Label;
                    dr["DOB"] = DOB.Text;
                    Label Address = item.FindControl("Address") as Label;
                    dr["Address"] = Address.Text;
                    dtParams.Rows.Add(dr);
                }
                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 10 || k == 1)
                            {
                                if (Convert.ToString(dtParams.Rows[j][k]) != "")
                                {
                                    if (Convert.ToString(dtParams.Rows[j][k]) != "-") sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                }
                                else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');

                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 10 || k == 1)
                            {
                                if (Convert.ToString(dtParams.Rows[j][k]) != "")
                                {
                                    if (Convert.ToString(dtParams.Rows[j][k]) != "-") sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                }
                                else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 1)
                            {
                                if (Convert.ToString(dtParams.Rows[j][k]) != "")
                                {
                                    if (Convert.ToString(dtParams.Rows[j][k]) != "-") sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                }
                                else sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
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
                Response.AddHeader("content-disposition", "attachment;filename=NoOfUser.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            catch(Exception ex)
            { ex.ToString(); }
        }
        
        public override void VerifyRenderingInServerForm(Control control) { } 
    }

}
