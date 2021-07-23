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
    public partial class PendingDsrReport : System.Web.UI.Page
    {
        DateTime mDate1 = DateTime.Now, mDate2 = DateTime.Now;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {//Ankita - 13/may/2016- (For Optimization)
                // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindSalePersonDDl();
                //dadtext.Text = "2";            
                //fill_TreeArea();
                BindDDLMonth();
                BindTreeViewControl();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                yearDDL.SelectedValue = System.DateTime.Now.Year.ToString();
                btnExport.Visible = false;
                noteLabel.Visible = false;
            }
        }

        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    monthDDL.Items.Add(new ListItem(monthName.Substring(0, 3), month.ToString().PadLeft(2, '0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDDL.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
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

        private void BindSalePersonDDl()
        {
            try
            {
                if (roleType == "Admin")
                {
                    //Ankita - 13/may/2016- (For Optimization)
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    DataTable dtcheckrole = new DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";

                    ListBox1.DataSource = dv1.ToTable();
                    ListBox1.DataTextField = "SMName";
                    ListBox1.DataValueField = "SMId";
                    ListBox1.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "SMName<>.";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ListBox1.DataSource = dv.ToTable();
                        ListBox1.DataTextField = "SMName";
                        ListBox1.DataValueField = "SMId";
                        ListBox1.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        void fill_TreeArea()
        {           
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {               
                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {//Ankita - 17/may/2016- (For Optimization)
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
                //Ankita - 17/may/2016- (For Optimization)
                //FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
                getchilddata(tnParent, tnParent.Value);
            }
        }
        //Ankita - 17/may/2016- (For Optimization)
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
        
        protected void btnGo_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        public void BindGrid()
        {
            try
            {               
               string smIDStr = "",smIDStr1 = "";         
               foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    smIDStr += node.Value + ",";
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                if (smIDStr1 == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select sales person.');", true);
                    return;
                }
                mDate1 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue);
                mDate2 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).AddDays(-1);
                int year = Convert.ToInt32(yearDDL.SelectedValue);
                int Month = Convert.ToInt32(monthDDL.SelectedValue);
                int dyasInMonth = DateTime.DaysInMonth(year, Month);
                if (dyasInMonth == 30)
                { gvData.Columns[37].Visible = false; }
                else if (dyasInMonth == 29)
                {
                    gvData.Columns[37].Visible = false;
                    gvData.Columns[36].Visible = false;
                    gvData.Columns[35].Visible = true;
                }

                else if (dyasInMonth == 28)
                {
                    gvData.Columns[37].Visible = false;
                    gvData.Columns[36].Visible = false;
                    gvData.Columns[35].Visible = false;
                    gvData.Columns[34].Visible = false;
                }
                else
                {
                    gvData.Columns[37].Visible = true;
                    gvData.Columns[36].Visible = true;
                    gvData.Columns[35].Visible = true;
                    gvData.Columns[34].Visible = true;
                }

                DataTable gvdt = new DataTable();
                gvdt.Columns.Add(new DataColumn("Head", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reporting", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Month", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Year", typeof(String)));
                gvdt.Columns.Add(new DataColumn("EmpName", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
                for (int ii = 1; ii <= dyasInMonth; ii++)
                {
                    gvdt.Columns.Add(new DataColumn("d" + Convert.ToString(ii).Trim(), typeof(String)));
                }               
                gvdt.Columns.Add(new DataColumn("Enter", typeof(Int32)));
                gvdt.Columns.Add(new DataColumn("Approve", typeof(Int32)));
                DataRow pDataRow = gvdt.NewRow();
                string stradd = "";
                if (DsrAllow.Value == "") { DsrAllow.Value = "0"; }
                int DsrallowDays = Convert.ToInt32(DsrAllow.Value);
                string dsrallowdate = Settings.GetUTCTime().AddDays(-DsrallowDays).ToString("dd/MMM/yyyy");
                string currentdate = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                string status = ddldsrtype.SelectedValue;
                if (status == "0")
                { stradd += " and vl.AppStatus is null"; }
                
                string str = String.Empty;
                str = "select distinct smid,DAY(holiday_date) as day1,holiday_date,Reason,areaID,AreaType from View_Holiday where holiday_date between  '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59'";
                DataTable dt_holiday = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                DataView dvdt_holiday = new DataView(dt_holiday);
                if (chkdsr.Checked != true)
                {
                    if (DsrallowDays != 0)
                    { str = "select vl.SMId,vdate,ISNULL(AppStatus,'') as Status from TransVisit vl where vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "' " + stradd + " and vdate not between '" + dsrallowdate + "' and '" + currentdate + "' Order by vl.SMId,vdate";}
                    else
                    { str = "select vl.SMId,vdate,ISNULL(AppStatus,'') as Status from TransVisit vl where vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "' " + stradd + " Order by vl.SMId,vdate"; }
                }
                else
                { str = "select vl.SMId,vdate,ISNULL(AppStatus,'') as Status from TransVisit vl where vdate between '" + dsrallowdate + "' and '" + currentdate + "' " + stradd + " Order by vl.SMId,vdate"; }

                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                DataView dvdsrData = new DataView(dsrData);

                string No_Days = "select NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId IN (" + smIDStr1 + ") and (lr.FromDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59' OR lr.ToDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59') and lr.appstatus in ('Approve') order by smid,FromDate";

                DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
                DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);
                //str = @"select ms.SMId,ms2.smname as Head,ms1.SMName as Reporting,ms.smname,ms.SyncId from MastSalesRep ms left join MastSalesRep ms1 on ms.UnderId =ms1.SMId left join MastSalesRep ms2 on ms1.UnderId=ms2.SMId where ms.SMId in (" + smIDStr1 + ") order by SMId";
                str = "select ms.SMId, case when mr.RoleType in ('AreaIncharge') and mr1.RoleType in ('CityHead','DistrictHead') then ms2.smname when mr.RoleType in ('AreaIncharge') and mr1.RoleType in ('RegionHead','StateHead') then ms1.SMName when mr.RoleType in ('CityHead','DistrictHead') then ms1.SMName else ms.SMName end as Head,ms1.SMName as Reporting,ms.smname,ms.SyncId from MastSalesRep ms left join MastSalesRep ms1 on ms.UnderId =ms1.SMId left join MastSalesRep ms2 on ms1.UnderId=ms2.SMId left join mastrole mr on mr.RoleId = ms.RoleId left join MastRole mr1 on mr1.RoleId = ms1.RoleId left join MastRole mr2 on mr2.RoleId=ms2.RoleId where ms.SMId in (" + smIDStr1 + ") and ms.Active=1 order by SMId";
                DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                int pday = 0; int dayloop = 0; 
                string DsrDate = string.Empty;
                string Leave_str = "", strleave = "", status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, transTemp = "", dsrQuery = string.Empty;
                pday = dyasInMonth;
                if (mDate2 > Settings.GetUTCTime()) { pday = Settings.GetUTCTime().Day; }
                int chkday = 0;
                if (mDate2 > Settings.GetUTCTime()) { chkday = pday - DsrallowDays; }
                foreach (DataRow dr in ssdt.Rows)
                {
                    DataRow mDataRow = gvdt.NewRow();
                    mDataRow["Head"] = dr["Head"].ToString();
                    mDataRow["Reporting"] = dr["Reporting"].ToString();
                    mDataRow["Name"] = dr["SMName"].ToString();
                    mDataRow["SyncId"] = dr["SyncId"].ToString();
                    mDataRow["EmpName"] = dr["SMName"].ToString();
                    mDataRow["SMId"] = dr["SMId"].ToString();
                    //mDataRow["Enter"] = en.ToString();
                    //mDataRow["Approve"] = ap.ToString();
                    mDataRow["Month"] = monthDDL.SelectedItem.Text.ToString();
                    mDataRow["Year"] = yearDDL.SelectedValue.ToString();


                    for (int k = 1; k <= dyasInMonth; k++)
                    {
                        if (DateTime.Parse(k + "/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).DayOfWeek.ToString().Trim() == "Sunday")
                        {
                            mDataRow["d" + k.ToString()] = "Off"; //Filling Sunday
                        }
                    }


                    if (status == "1" || status == "2")
                    {
                        if (chkdsr.Checked != true)
                        {
                            if (DsrallowDays != 0)
                            {
                                for (int k = 1; k <= pday - (DsrallowDays + 1); k++)
                                {
                                    if (mDataRow["d" + k.ToString()] != "Off") { mDataRow["d" + k.ToString()] = "A"; }
                                }
                            }
                            else
                            {
                                for (int k = 1; k <= pday - (DsrallowDays); k++)
                                {
                                    if (mDataRow["d" + k.ToString()] != "Off") { mDataRow["d" + k.ToString()] = "A"; }
                                }
                            }
                        }
                        else
                        {
                            for (int c = chkday; c <= pday; c++)
                            {
                                if (mDataRow["d" + c.ToString()] != "Off") { mDataRow["d" + c.ToString()] = "A"; }
                            }
                        }
                    }

                    dvdt_holiday.RowFilter = "smid=" + dr["SMId"];
                    foreach (DataRow dsr1 in dvdt_holiday.ToTable().Rows)
                    {
                        DsrDate = Convert.ToDateTime(dsr1["holiday_date"].ToString()).Day.ToString().Trim();
                        mDataRow["d" + DsrDate] = "H";
                    }
                    
                    dvdsrData.RowFilter = "smid=" + dr["SMId"];
                    foreach (DataRow dsr1 in dvdsrData.ToTable().Rows)
                    {
                        DsrDate = Convert.ToDateTime(dsr1["vdate"].ToString()).Day.ToString().Trim();
                        if (status == "1" || status == "2") { mDataRow["d" + DsrDate] = ""; }
                        if (dsr1["SMId"].ToString() == dr["SMId"].ToString())
                        {
                            if (status == "0")
                            {
                                if (dsr1["Status"].ToString() == "") { mDataRow["d" + DsrDate] = "E"; }
                                if (dsr1["Status"].ToString() == "Approve") { mDataRow["d" + DsrDate] = " "; }
                                if (dsr1["Status"].ToString() == "Reject") { mDataRow["d" + DsrDate] = "A"; }
                            }
                            else if (status == "1")
                            {
                                if (dsr1["Status"].ToString() == "") { mDataRow["d" + DsrDate] = " "; }
                                if (dsr1["Status"].ToString() == "Approve") { mDataRow["d" + DsrDate] = " "; }
                                if (dsr1["Status"].ToString() == "Reject") { mDataRow["d" + DsrDate] = "A"; }
                            }
                            else
                            {
                                if (dsr1["Status"].ToString() == "") { mDataRow["d" + DsrDate] = "E"; }
                                if (dsr1["Status"].ToString() == "Approve") { mDataRow["d" + DsrDate] = " "; }
                                if (dsr1["Status"].ToString() == "Reject") { mDataRow["d" + DsrDate] = "A"; }
                            }
                        }
                    }

                    dvdt_NoOfdays.RowFilter = "smid=" + dr["SMId"];
                    if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
                    {
                        for (int cc = 0; cc < dvdt_NoOfdays.ToTable().Rows.Count; cc++)
                        {
                            if (dvdt_NoOfdays.ToTable().Rows[cc]["SMId"].ToString() == dr["SMId"].ToString())
                            {
                                double daysNo = Convert.ToDouble(dvdt_NoOfdays.ToTable().Rows[cc]["NoOfDays"].ToString());
                                DateTime fromdate = new DateTime();
                                DateTime todate = new DateTime();
                                todate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["NoDays"].ToString());
                                fromdate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());

                                dayloop = Convert.ToInt32((todate - fromdate).TotalDays);
                                for (int c1 = 0; c1 <= dayloop; c1++)
                                {
                                    DateTime dateTime1 = new DateTime();
                                    dateTime1 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                                    dateTime1 = dateTime1.AddDays(c1);

                                    if (dateTime1 > mDate2)
                                    {
                                    }
                                    else if (dateTime1 < mDate1)
                                    {
                                    }
                                    else
                                    {                                       
                                        DateTime dateTime2 = new DateTime();
                                        DateTime dateTime3 = new DateTime();
                                        dateTime2 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                                        dateTime3 = Convert.ToDateTime(dateTime1.ToString());
                                        double NrOfDays = ((dateTime3 - dateTime2).TotalDays) * 2;                                        
                                        strleave = dvdt_NoOfdays.ToTable().Rows[cc]["LeaveString"].ToString();
                                        status_pr = dvdt_NoOfdays.ToTable().Rows[cc]["AppStatus"].ToString();
                                        string str1 = strleave.Substring((Convert.ToInt32(NrOfDays)), 2);                                      
                                       
                                        if (status_pr == "Approve")
                                        {
                                            if (str1.Substring(0, 1) == "L" && str1.Substring(1, 1) == "L") { ls_val += " "; }
                                        }
                                       
                                        mDataRow["d" + dateTime3.Day.ToString().Trim()] = ls_val;
                                        ls_val = "";
                                    }
                                }

                            }
                        }

                    }
                    dvdt_holiday.RowFilter = null;

                    Int16 mBlankRow = 0;
                    if (chkdsr.Checked != true)
                    {
                        if (DsrallowDays != 0)
                        {
                            for (int k = 1; k <= pday - (DsrallowDays + 1); k++)
                            {
                                if (status == "1" || status == "2") { if (mDataRow["d" + k.ToString()] == "A") mBlankRow = 1; }
                                if (status == "0" || status == "2") { if (mDataRow["d" + k.ToString()] == "E") mBlankRow = 1; }
                            }
                        }
                        else
                        {
                            for (int k = 1; k <= pday - (DsrallowDays); k++)
                            {
                                if (status == "1" || status == "2") { if (mDataRow["d" + k.ToString()] == "A") mBlankRow = 1; }
                                if (status == "0" || status == "2") { if (mDataRow["d" + k.ToString()] == "E") mBlankRow = 1; }
                            }
                        }
                    }
                    else
                    {
                        for (int c = chkday; c <= pday ; c++)
                        {
                            if (status == "1" || status == "2") { if (mDataRow["d" + c.ToString()] == "A") mBlankRow = 1; }
                            if (status == "0" || status == "2") { if (mDataRow["d" + c.ToString()] == "E") mBlankRow = 1; }
                        }
                    }
                    dvdt_holiday.RowFilter = null;
                    dvdsrData.RowFilter = null;
                    if (mBlankRow == 1)
                    {
                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();
                    }

                }                               

                if (gvdt.Rows.Count > 0)
                {
                    gvData.DataSource = null;
                    gvData.DataSource = gvdt;
                    gvData.DataBind();
                    noteDiv.Style.Add("display", "block");
                    noteLabel.Visible = true;
                    btnExport.Visible = true;
                }
                else
                {
                    gvData.DataSource = gvdt;
                    gvData.DataBind();
                    btnExport.Visible = false;
                    noteDiv.Style.Add("display", "none");
                    noteLabel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PendingDsrReport.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string headertxt = string.Empty;
            StringBuilder sb = new StringBuilder();
            int r = 42; int r1 = 42; int r2 = 42; int r3 = 42;
            int Month = System.DateTime.DaysInMonth(Convert.ToInt32(yearDDL.SelectedValue), Convert.ToInt32(monthDDL.SelectedValue));
            if (gvData.Rows.Count != 0)
            {
                if (Month == 30)
                { r = 37; }
                else if (Month == 29)
                {
                    r = 35; r1 = 34;
                }
                else if (Month == 28)
                {
                    r = 34;
                    r1 = 35;
                    r2 = 36;
                    r3 = 37;
                }
                //Forloop for header
                for (int i = 0; i < gvData.HeaderRow.Cells.Count; i++)
                {
                    //          dt.Columns.Add(gvData.HeaderRow.Cells[i].Text);
                    if (!(i == 6 || i == 40 || i == r || i == r1 || i == r2 || i == r3))
                    {
                        headertxt += gvData.HeaderRow.Cells[i].Text.TrimStart('"').TrimEnd('"') + ",";
                    }
                }
                sb.Append(headertxt);
                sb.Append(System.Environment.NewLine);
                //foreach for datarow
                foreach (GridViewRow row in gvData.Rows)
                {
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        if (!(j == 6 || j == 40 || j == r || j == r1 || j == r2 || j == r3))
                        {
                            if (row.Cells[j].Text.ToString().Contains(""))
                            {
                                if (row.Cells[j].Text != "&nbsp;")
                                {
                                    sb.Append(String.Format("\"{0}\"", row.Cells[j].Text.ToString()) + ',');
                                }
                                else
                                { sb.Append(String.Format("\"{0}\"", "") + ','); }                                
                            }
                           
                            else if (row.Cells[j].Text.ToString().Contains(System.Environment.NewLine))
                            {
                               sb.Append(String.Format("\"{0}\"", row.Cells[j].Text.ToString()) + ',');                                
                            }
                            else
                            {
                                sb.Append(row.Cells[j].Text + ",");
                            }
                            //     sb.Append(row.Cells[j].Text + ",");
                        }
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=Absent & Non Approval - DSR.csv");
            Response.Write(sb.ToString());
            Response.End();          
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string mDay = ""; string str = String.Empty; string filledVal = String.Empty; 
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    mDate1 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue);
                    mDate2 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).AddDays(-1);
                    for (int ii = 1; ii <= mDate2.Day; ii++)
                    {
                        mDay = Convert.ToString(ii).Trim();
                        if (mDay.Length == 1)
                            mDay = "0" + mDay;
                        if (DateTime.Parse(mDay + "/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).DayOfWeek.ToString().Trim() == "Sunday")
                        {
                                e.Row.Cells[ii + 6].Text = "Off";
                                e.Row.Cells[ii + 6].BackColor = Color.FromName("#EED690");                           
                        }
                        if(e.Row.Cells[ii + 6].Text == "H")
                        {
                            e.Row.Cells[ii + 6].ForeColor = Color.FromName("#fff");
                            e.Row.Cells[ii + 6].BackColor = Color.Red;
                        }                       
                    }                    
                }
            }
            catch (Exception ex)
            {

                ex.ToString();
            }
        }
    }
}