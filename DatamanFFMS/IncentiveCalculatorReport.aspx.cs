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

namespace AstralFFMS
{
    public partial class IncentiveCalculatorReport : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                BindSalePersonDDl();
                //fill_TreeArea();
                BindDDLMonth();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                yearDDL.SelectedValue = System.DateTime.Now.Year.ToString();             
             
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
                for (int i = System.DateTime.Now.Year - 4; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDDL.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindSalePersonDDl()
        {
            try
            {
                if (roleType == "Admin")
                {//Ankita - 13/may/2016- (For Optimization)
                    //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMName,MastSalesRep.SMId,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    DataTable dtcheckrole = new DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";
                    DdlSalesPerson.DataSource = dv1.ToTable();
                    DdlSalesPerson.DataTextField = "SMName";
                    DdlSalesPerson.DataValueField = "SMId";
                    DdlSalesPerson.DataBind();
                    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
                    //ListBox1.DataSource = dv1.ToTable();
                    //ListBox1.DataTextField = "SMName";
                    //ListBox1.DataValueField = "SMId";
                    //ListBox1.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    //     dv.RowFilter = "RoleName='Level 1'";
                    dv.RowFilter = "SMName<>.";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        DdlSalesPerson.DataSource = dv.ToTable();
                        DdlSalesPerson.DataTextField = "SMName";
                        DdlSalesPerson.DataValueField = "SMId";
                        DdlSalesPerson.DataBind();
                        DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
                        //ListBox1.DataSource = dv.ToTable();
                        //ListBox1.DataTextField = "SMName";
                        //ListBox1.DataValueField = "SMId";
                        //ListBox1.DataBind();
                    }
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
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

        private void GetRoleType(string p)
        {
            try
            {
                string roleqry = @"select mr.RoleType from MastRole mr Left join MastSalesRep ms on ms.roleid = mr.roleid where ms.smid=" + Convert.ToInt32(p) + "";
                DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
                if (roledt.Rows.Count > 0)
                {
                    roleType = roledt.Rows[0]["RoleType"].ToString();
                }
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
                salesTextBox.Text = "";
                string smIDStr = "";
                smIDStr = DdlSalesPerson.SelectedValue.ToString();
                if (DdlSalesPerson.SelectedIndex > 0)
                {                   
                    string roleqry = @"select mr.RoleType from MastRole mr Left join MastSalesRep ms on ms.roleid = mr.roleid where ms.smid=" + Convert.ToInt32(smIDStr) + "";
                    DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
                    if (roledt.Rows.Count > 0)
                    {
                        roleType = roledt.Rows[0]["RoleType"].ToString();
                    }

                    decimal decGrowth = 0M;
                    if (growthTextBox.Text != "")
                    {
                        decGrowth = Convert.ToDecimal(growthTextBox.Text);
                    }

                    string query = "select * from mastincentive where role='" + roleType + "'";
                    DataTable incentivedt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                    if (incentivedt.Rows.Count > 0)
                    {                        
                        //string incQry = @"SELECT DISTINCT a.NAME,a.SyncId,a.roletype,a.Incentive,max(a.DistributorName) as DistributorName,a.ItemName,Sum(a.LAST6MonthAmount) AS LAST6MonthAmount,Sum(a.LAstYearAmount) AS LAstYearAmount,(Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100)) AS ProposedSale,Sum(a.CurrentMonthAmount) AS CurrentMonthAmount,(Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100))) AS GAP,Incentiveamount = ( case when ((Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*7/100))) * Incentive/100) > 0 then ((Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100))) * Incentive/100) else 0 end) FROM " +
                        //" (SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,Sum(Amount) as CurrentMonthAmount,0 AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                        //" UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName, 0 as CurrentMonthAmount, Sum(Amount) AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + (Convert.ToInt32(yearDDL.SelectedValue) - 1) + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                        //" UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,0 as CurrentMonthAmount,0 AS LAstYearAmount, Sum(Amount) AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate) between '" + (Convert.ToInt32(monthDDL.SelectedValue) - 6) + "' and '" + (Convert.ToInt32(monthDDL.SelectedValue) - 1) + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive )a GROUP BY a.NAME,a.SyncId,a.ItemName,a.roletype,a.incentive ORDER BY a.name  ";
                        string incQry = @"SELECT DISTINCT a.NAME,a.SyncId,a.roletype,a.Incentive,max(a.DistributorName) as DistributorName,a.ItemName,Sum(a.LAST6MonthAmount) AS LAST6MonthAmount,Sum(a.LAstYearAmount) AS LAstYearAmount,(Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100)) AS ProposedSale,Sum(a.CurrentMonthAmount) AS CurrentMonthAmount,(Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100))) AS GAP FROM " +
                       " (SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,Sum(Amount) as CurrentMonthAmount,0 AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                       " UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName, 0 as CurrentMonthAmount, Sum(Amount) AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + (Convert.ToInt32(yearDDL.SelectedValue) - 1) + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                       " UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,0 as CurrentMonthAmount,0 AS LAstYearAmount, Sum(Amount) AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate) between '" + (Convert.ToInt32(monthDDL.SelectedValue) - 6) + "' and '" + (Convert.ToInt32(monthDDL.SelectedValue) - 1) + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive )a GROUP BY a.NAME,a.SyncId,a.ItemName,a.roletype,a.incentive ORDER BY a.name  ";
                        DataTable incqry1dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, incQry);
                        if (incqry1dt1.Rows.Count > 0)
                        {
                            incqry1dt1.Columns.Add("Incentive_1");
                            incqry1dt1.AcceptChanges();
                            for (int k = 0; k < incqry1dt1.Rows.Count; k++)
                            {

                                decimal proposed = Convert.ToDecimal(incqry1dt1.Rows[k]["proposedsale"].ToString());
                                decimal last = Convert.ToDecimal(incqry1dt1.Rows[k]["LAstYearAmount"].ToString());
                                if (last <= 0)
                                {
                                    incqry1dt1.Rows[k]["Incentive_1"] = 0.00;
                                    continue;
                                }
                                decimal result_subtract = proposed - last;
                                decimal result_multi = (result_subtract * (Convert.ToDecimal(incentivedt.Rows[0]["incentive"].ToString()))) / 100;
                                decimal Finalresult_multi = Convert.ToDecimal(result_multi.ToString("0,0.00"));                               
                                incqry1dt1.Rows[k]["Incentive_1"] = Finalresult_multi;
                               
                            }
                            incqry1dt1.AcceptChanges();
                            increportrpt.DataSource = incqry1dt1;
                            increportrpt.DataBind();
                            decimal decCurrentAmt = 0;
                            decimal incentiveAmt = 0;
                            for (int i = 0; i < incqry1dt1.Rows.Count; i++)
                            {
                                decCurrentAmt += Convert.ToDecimal(incqry1dt1.Rows[i]["CurrentMonthAmount"]);
                                incentiveAmt += Convert.ToDecimal(incqry1dt1.Rows[i]["Incentive_1"]);
                            }

                            salesTextBox.Text = Convert.ToString(decCurrentAmt);
                            txtincentiveAmount.Text = incentiveAmt.ToString("0.00");
                            btnExport.Visible = true;
                        }
                        else
                        {
                            increportrpt.DataSource = null;
                            increportrpt.DataBind();
                            btnExport.Visible = false;
                        }                       

                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select salespesron');", true);
                    increportrpt.DataSource = null;
                    increportrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/IncentiveCalculatorReport.aspx");
        }

        protected void ProposedSaletxt_TextChanged(object sender, EventArgs e)
        {      


        }

        protected void growthTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string smIDStr1 = "";             

                smIDStr1 = DdlSalesPerson.SelectedValue.ToString();               
                string roleqry = @"select mr.RoleType from MastRole mr Left join MastSalesRep ms on ms.roleid = mr.roleid where ms.smid=" + Convert.ToInt32(smIDStr1) + "";
                DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
                if (roledt.Rows.Count > 0)
                {
                    roleType = roledt.Rows[0]["RoleType"].ToString();
                }
                decimal decGrowth = Convert.ToDecimal(growthTextBox.Text);
                string query = "select * from mastincentive where role='"+roleType+"'";
                DataTable incentivedt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if(incentivedt.Rows.Count>0)
                {
                    if (smIDStr1 != "")
                    {                 
                     
                     // string incQry = @"SELECT DISTINCT a.NAME,a.SyncId,a.roletype,a.Incentive,max(a.DistributorName) as DistributorName,a.ItemName,Sum(a.LAST6MonthAmount) AS LAST6MonthAmount,Sum(a.LAstYearAmount) AS LAstYearAmount,(Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100)) AS ProposedSale,Sum(a.CurrentMonthAmount) AS CurrentMonthAmount,(Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100))) AS GAP,Incentiveamount = ( case when ((Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*7/100))) * Incentive/100) > 0 then ((Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100))) * Incentive/100) else 0 end) FROM " +
                     //" (SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,Sum(Amount) as CurrentMonthAmount,0 AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr1 + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                     //" UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName, 0 as CurrentMonthAmount, Sum(Amount) AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + (Convert.ToInt32(yearDDL.SelectedValue) - 1) + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr1 + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                     //" UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,0 as CurrentMonthAmount,0 AS LAstYearAmount, Sum(Amount) AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate) between '" + (Convert.ToInt32(monthDDL.SelectedValue) - 6) + "' and '" + (Convert.ToInt32(monthDDL.SelectedValue) - 1) + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr1 + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive )a GROUP BY a.NAME,a.SyncId,a.ItemName,a.roletype,a.incentive ORDER BY a.name  ";
                     string incQry = @"SELECT DISTINCT a.NAME,a.SyncId,a.roletype,a.Incentive,max(a.DistributorName) as DistributorName,a.ItemName,Sum(a.LAST6MonthAmount) AS LAST6MonthAmount,Sum(a.LAstYearAmount) AS LAstYearAmount,(Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100)) AS ProposedSale,Sum(a.CurrentMonthAmount) AS CurrentMonthAmount,(Sum(a.CurrentMonthAmount) - (Sum(a.LAstYearAmount)+(Sum(a.LAstYearAmount)*" + decGrowth + "/100))) AS GAP FROM " +
                     " (SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,Sum(Amount) as CurrentMonthAmount,0 AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr1 + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                     " UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName, 0 as CurrentMonthAmount, Sum(Amount) AS LAstYearAmount,0 AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + (Convert.ToInt32(yearDDL.SelectedValue) - 1) + "' and month(t1.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr1 + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive " +
                     " UNION ALL SELECT ms.SMName as NAME,ms.Syncid,mr.roletype,mc.incentive,mp.PartyName AS DistributorName,mi1.ItemName,0 as CurrentMonthAmount,0 AS LAstYearAmount, Sum(Amount) AS LAST6MonthAmount FROM TransDistInv1 t1 Left join TransdistInv t on t1.DistInvDocId=t.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId =t1.ItemId LEFT JOIN mastitem mi1 ON mi.underid=mi1.ItemId LEFT JOIN mastparty mp ON mp.PartyId=t1.DistId LEFT JOIN mastsalesrep ms ON ms.SMId=t.SMID left join mastrole mr on ms.roleid=mr.roleid left join [MastIncentive] mc on mr.roletype=mc.role and mc.Itemgroup = mi1.itemname Where year(t1.VDate)='" + yearDDL.SelectedValue + "' and month(t1.VDate) between '" + (Convert.ToInt32(monthDDL.SelectedValue) - 6) + "' and '" + (Convert.ToInt32(monthDDL.SelectedValue) - 1) + "' AND mp.PartyDist=1 and t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr1 + ")) GROUP BY ms.smname,ms.Syncid,mp.partyname,mi1.itemname,mr.roletype,mc.incentive )a GROUP BY a.NAME,a.SyncId,a.ItemName,a.roletype,a.incentive ORDER BY a.name  ";

                        DataTable incqry1dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, incQry);

                        if (incqry1dt1.Rows.Count > 0)
                        {
                            incqry1dt1.Columns.Add("Incentive_1");
                            incqry1dt1.AcceptChanges();
                            for (int k = 0; k < incqry1dt1.Rows.Count; k++)
                            {

                                decimal proposed = Convert.ToDecimal(incqry1dt1.Rows[k]["proposedsale"].ToString());
                                decimal last = Convert.ToDecimal(incqry1dt1.Rows[k]["LAstYearAmount"].ToString());
                                
                                if (last <= 0)
                                {
                                    incqry1dt1.Rows[k]["Incentive_1"] = 0.00;
                                    continue;
                                }
                                decimal result_subtract = proposed - last;
                                decimal result_multi = (result_subtract * (Convert.ToDecimal(incentivedt.Rows[0]["incentive"].ToString()))) / 100;
                                decimal Finalresult_multi = Convert.ToDecimal(result_multi.ToString("0,0.00"));                               
                                incqry1dt1.Rows[k]["Incentive_1"] = Finalresult_multi;
                                
                            }
                            incqry1dt1.AcceptChanges();
                            increportrpt.DataSource = incqry1dt1;
                            increportrpt.DataBind();
                            decimal decCurrentAmt = 0;
                            decimal incentiveAmt = 0;
                            for (int i = 0; i < incqry1dt1.Rows.Count; i++)
                            {
                                decCurrentAmt += Convert.ToDecimal(incqry1dt1.Rows[i]["CurrentMonthAmount"]);
                                incentiveAmt += Convert.ToDecimal(incqry1dt1.Rows[i]["Incentive_1"]);
                            }

                            salesTextBox.Text = Convert.ToString(decCurrentAmt);
                            txtincentiveAmount.Text = incentiveAmt.ToString("0.00");
                            btnExport.Visible = true;
                        }
                        else
                        {
                            increportrpt.DataSource = null;
                            increportrpt.DataBind();
                            btnExport.Visible = false;
                        }
                    }

                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select salespesron');", true);
                    increportrpt.DataSource = null;
                    increportrpt.DataBind();


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
            Response.AddHeader("content-disposition", "attachment;filename=IncentiveCalculatorReport.csv");
            //string headertext = "Name".TrimStart('"').TrimEnd('"') + ", " + "Product".TrimStart('"').TrimEnd('"') + "," + "Last 6 Month".TrimStart('"').TrimEnd('"') + "," + "Last Year".TrimStart('"').TrimEnd('"') + "," + "Proposed Sale".TrimStart('"').TrimEnd('"') + "," + "Current Sale".TrimStart('"').TrimEnd('"') + "," + "GAP".TrimStart('"').TrimEnd('"') + ", " + "Incentive".TrimStart('"').TrimEnd('"');
            string headertext = "Name".TrimStart('"').TrimEnd('"') + ", " + "Product".TrimStart('"').TrimEnd('"') + "," + "Last 6 Month".TrimStart('"').TrimEnd('"') + "," + "Last Year".TrimStart('"').TrimEnd('"') + "," + "Proposed Sale".TrimStart('"').TrimEnd('"') + "," + "Current Sale".TrimStart('"').TrimEnd('"') + "," + "GAP".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Name", typeof(String)));                       
            dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Last6Month", typeof(String)));
            dtParams.Columns.Add(new DataColumn("LastYear", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProposedSale", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CurrentSale", typeof(String)));
            dtParams.Columns.Add(new DataColumn("GAP", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Incentive", typeof(String)));

            foreach (RepeaterItem item in increportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label NameLabel = item.FindControl("NameLabel") as Label;
                dr["Name"] = NameLabel.Text;              
                Label materialGroupLabel = item.FindControl("materialGroupLabel") as Label;
                dr["Product"] = materialGroupLabel.Text.ToString();
                Label LAST6MonthAmountLabel = item.FindControl("LAST6MonthAmountLabel") as Label;
                dr["Last6Month"] = LAST6MonthAmountLabel.Text.ToString();
                Label LAstYearAmountLabel = item.FindControl("LAstYearAmountLabel") as Label;
                dr["LastYear"] = LAstYearAmountLabel.Text.ToString();
                TextBox ProposedSaletxt = item.FindControl("ProposedSaletxt") as TextBox;
                dr["ProposedSale"] = ProposedSaletxt.Text.ToString();
                Label CurrentMonthAmountLabel = item.FindControl("CurrentMonthAmountLabel") as Label;
                dr["CurrentSale"] = CurrentMonthAmountLabel.Text.ToString();
                Label GAPLabel = item.FindControl("GAPLabel") as Label;
                dr["GAP"] = GAPLabel.Text.ToString();
                //Label IncentiveamountLabel = item.FindControl("IncentiveamountLabel") as Label;
                //dr["Incentive"] = IncentiveamountLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {                       
                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');                      
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {                       
                        sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');                      
                    }
                    else
                    {                       
                        sb.Append(dtParams.Rows[j][k].ToString() + ',');                       
                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=IncentiveCalculatorReport.csv");
            Response.Write(sb.ToString());
            Response.End();           

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }
}