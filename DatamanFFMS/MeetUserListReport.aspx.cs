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

namespace AstralFFMS
{
    public partial class MeetUserListReport : System.Web.UI.Page
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
                fill_TreeArea();
             
                //lblnouser.Visible = false;
                //btnNOUExport.Visible = false;
              
                //string pageName = Path.GetFileName(Request.Path);
                if (btnexport.Text == "Export")
                {
                    //btnexport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnexport.CssClass = "btn btn-primary";
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

       
        protected void btnGo_Click(object sender, EventArgs e)
        {
            this.fillUserList(); 
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        public void fillUserList()
        {
            try
            {

                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtmDate.Text))
                {
                    string sm = "", smIDStr1 = "", smIDStr = "",strMeetPlanId="";
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
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId]  left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlstatus.SelectedIndex > 0 && ddlmeetType.SelectedIndex > 0)
                        {
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (trview.CheckedNodes.Count > 0 && ddlmeetType.SelectedIndex > 0)
                        {
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and m.appstatus not in ('Cancel') and MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlstatus.SelectedIndex > 0 && trview.CheckedNodes.Count > 0)
                        {
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where M.SMId in (SELECT smid FROM mastsalesrepgrp  WHERE  maingrp IN (" + smIDStr1 + ")) and M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlstatus.SelectedIndex > 0)
                        {
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where  M.AppStatus='" + ddlstatus.SelectedValue + "' and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (trview.CheckedNodes.Count > 0)
                        {
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where M.SMId in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and m.appstatus not in ('Cancel') and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else if (ddlmeetType.SelectedIndex > 0)
                        {
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName  from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId  left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " and [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "'";
                        }
                        else
                        {
                            str = @"SELECT m.MeetPlanId,m.MeetDate,m.MeetName from TransMeetPlanEntry M LEFT JOIN transmeetexpense me ON M.MeetPlanId=me.MeetPlanId left join MastArea on MastArea.AreaId=M.MeetLoaction left join MastParty MP on m.RetailerPartyID=MP.PartyId left join MastItemClass MI on m.IndId=MI.Id  Left Join MastSalesRep MST on MST.SMId = M.SMId left join MastParty MPD on m.DistId=MPD.PartyId left Join MastMeetType MMT on m.MeetTypeId=MMT.Id left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id where [MeetDate]>='" + Settings.dateformat1(txtmDate.Text) + "' and [MeetDate]<='" + Settings.dateformat1(txttodate.Text) + "' and  M.SMID in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))";
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select salesperson.');", true);
                    }

                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    foreach (DataRow dr in dt.Rows)
                    {
                        strMeetPlanId += dr["MeetPlanId"]+",";
                    }
                    strMeetPlanId += strMeetPlanId.TrimEnd(',');
                    if (strMeetPlanId != "")
                    {
                        string str1 = "  select l.*,MST.SMName,E.MeetDate,E.MeetName,E.AppStatus,ma.AreaName,mb.AreaName as BeatName from TransAddMeetUser l left join TransMeetPlanEntry E on l.[MeetId]=e.MeetPlanId  left join PartyType P on l.PartyType=p.PartyTypeId Left Join MastSalesRep MST on MST.SMId = E.SMId  Left Join MastArea ma on l.AreaId=ma.AreaId     Left Join MastArea mb on l.AreaId=mb.AreaId  where l.MeetId IN (" + strMeetPlanId + ") order by l.ContactPersonName";
                        DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                        if (dt1.Rows.Count > 0)
                        {
                            rptnoofusers.DataSource = dt1;
                            rptnoofusers.DataBind();
                            // lblproductist.Visible = true;
                            //btnNOUExport.Visible = true;
                            //lblnouser.Visible = true;

                        }
                    }  
                    else
                    {
                        rptnoofusers.DataSource = null;
                        rptnoofusers.DataBind();

                    }

                   
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                    rptnoofusers.DataSource = null;
                    rptnoofusers.DataBind();
                }
            }
            catch { }
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


   
        protected void btnNouexport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=MeetUserList.csv");
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
                        if (k == 10 || k == 1)
                        {
                            if (Convert.ToString(dtParams.Rows[j][k]) != "")
                            {
                                if (Convert.ToString(dtParams.Rows[j][k]) != "-")
                                    //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
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
            Response.AddHeader("content-disposition", "attachment;filename=MeetUserList.csv");
            Response.Write(sb.ToString());
            Response.End();

        }
        
        public override void VerifyRenderingInServerForm(Control control) { } 
    }

}
