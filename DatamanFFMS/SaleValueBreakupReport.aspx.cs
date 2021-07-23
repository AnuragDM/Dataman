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
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class SaleValueBreakupReport : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
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
                //BindSalePersonDDl();
                List<DistributorsMonthlyItem> distMonthlyItem = new List<DistributorsMonthlyItem>();
                distMonthlyItem.Add(new DistributorsMonthlyItem());
                salevaluerpt.DataSource = distMonthlyItem;
                salevaluerpt.DataBind();
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
                //fill_TreeArea();
                BindTreeViewControl();
                BindMaterialGroup();
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); //System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
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
        public class DistributorsMonthlyItem
        { 
            public string VDate { get; set; }
            //public string Name { get; set; }
            //public string SyncId { get; set; }
            public string DistributorName { get; set; }
            public string ItemName { get; set; }
            public string CurrentMonthAmount { get; set; }
            public string Discount { get; set; }
            public string NetAmount { get; set; }
     
        }
        [WebMethod(EnableSession = true)]
        public static string GetSaleValue(string Distid, string ProductGroup, string Product, string Fromdate, string Todate)
        {
            //string Query; string Qrychk; string Query1 = string.Empty;
            string qry = "";
            //smIDStr1 = HttpContext.Current.Session["treenodes"].ToString();
            if (ProductGroup != "" && Product == "")
            { qry = "and mi1.itemid in(" + ProductGroup + ")"; }
            if (Product != "" && Product != "")
            { qry = qry + "and mi.itemid in (" + Product + ")"; }
            string Qrychk = " tdv1.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and tdv1.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
//            string Query = @"select tdv.DistInvDocId as Docid,Convert(varchar(15),CONVERT(date,tdv.VDate,103),106) AS VDate,ms.SMName AS Name,ms.SyncId, mp.PartyName AS DistributorName,max(mi1.ItemName) AS MaterialGroup,mi.ItemName,mi.syncid as Item_Syncid, Sum(tdv1.Amount) as NetAmount from TransDistInv tdv Left join TransDistInv1 tdv1 on tdv.DistInvId=tdv1.DistInvId and tdv.DistInvDocId=tdv1.DistInvDocId 
//            LEFT JOIN mastparty mp ON mp.PartyId=tdv.DistId left join mastsalesrep ms on ms.SMId=tdv.SMID LEFT JOIN mastitem mi ON mi.ItemId=tdv1.ItemId LEFT JOIN MastItem mi1 ON mi.Underid=mi1.ItemId where tdv1.DistId in (" + Distid + ") and mp.PartyDist=1 " + qry + "  and " + Qrychk + " group by tdv.DistInvDocId,ms.SMName,ms.SyncId,mp.SyncId,mp.PartyName,tdv.VDate,mi.ItemName,mi.syncid ORDER BY tdv.VDate desc";
            string Query = @"select tdv1.DistInvDocId as Docid,Convert(varchar(15),CONVERT(date,tdv1.VDate,103),106) AS VDate,mp.PartyName AS DistributorName,max(mi1.ItemName) AS MaterialGroup,mi.ItemName,mi.syncid as Item_Syncid, Sum(tdv1.Amount) as NetAmount from TransDistInv1 tdv1 LEFT JOIN mastparty mp ON mp.PartyId=tdv1.DistId 
           LEFT JOIN mastitem mi ON mi.ItemId=tdv1.ItemId LEFT JOIN MastItem mi1 ON mi.Underid=mi1.ItemId where tdv1.DistId in (" + Distid + ") and mp.PartyDist=1 " + qry + "  and " + Qrychk + " group by tdv1.DistInvDocId,mp.SyncId,mp.PartyName,tdv1.VDate,mi.ItemName,mi.syncid ORDER BY tdv1.VDate desc";

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            return JsonConvert.SerializeObject(dtItem);

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

                    ListBox1.DataSource = dv1.ToTable();
                    ListBox1.DataTextField = "SMName";
                    ListBox1.DataValueField = "SMId";
                    ListBox1.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                    //     dv.RowFilter = "RoleName='Level 1'";
                    dv.RowFilter = "SMName<>.";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        ListBox1.DataSource = dv.ToTable();
                        ListBox1.DataTextField = "SMName";
                        ListBox1.DataValueField = "SMId";
                        ListBox1.DataBind();
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

                // for (int j = levelcnt + 1; j <= 5; j++)
                for (int j = levelcnt + 1; j <= 9; j++)
                {
                    string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
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
        private void BindMaterialGroup()
        {
            try
            {//Ankita - 13/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();
                }
          //      ddlMatGrp.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void ddlMatGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";          
            foreach (ListItem matGrp in matGrpListBox.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
            if (matGrpStr != "")
            { 
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }               
            }
            else
            {
                ClearControls();
            }
        }
        private void ClearControls()
        {
            try
            {         
                salevaluerpt.DataSource = null;
                salevaluerpt.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr = "";
                string smIDStr1 = "", Qrychk = "", Query = "";
                //         string message = "";
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                    }
                }
                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                if (smIDStr1 != "")
                {
                     if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                    {
                        if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                        {

                    Qrychk = " t.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and t.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                    string matGrpStr = "";
                    //         string message = "";
                    foreach (ListItem matGrp in matGrpListBox.Items)
                    {
                        if (matGrp.Selected)
                        {
                            matGrpStr += matGrp.Value + ",";
                        }
                    }
                    matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

                    if (matGrpStr != "")
                    {
                        Query = " and mi1.ItemId in (" + matGrpStr + ") ";
                    }
                    //if (ddlProduct.SelectedIndex != 0)
                    //{
                    //    Query = Query + "and mi.UnderId=" + ddlProduct.SelectedValue + "";
                    //}

                    string qry = @"SELECT Convert(Varchar, t1.VDate,106) as VDate,ms.SMName AS Name,ms.SyncId, mp.PartyName AS DistributorName,mi.ItemName,max(mi1.ItemName) AS MaterialGroup, Sum(t1.Amount) as CurrentMonthAmount,t1.Disc_Amt AS Discount,Sum(t1.Amount) AS NetAmount
                    from TransDistInv t LEFT JOIN Transdistinv1 t1 ON t.DistInvDocId=t1.DistInvDocId LEFT JOIN mastitem mi ON mi.ItemId=t1.ItemId
                    LEFT JOIN MastItem mi1 ON mi.Underid=mi1.ItemId LEFT JOIN mastparty mp ON t.DistId=mp.PartyId inner JOIN mastsalesrep ms ON ms.SMId=t.SMID
                    Where t.smid in (select smid from mastsalesrepgrp where maingrp in (" + smIDStr1 + "))  and mp.PartyDist=1 " + Query + "  and " + Qrychk + " GROUP BY mp.PartyName,mp.AreaId,t1.Disc_Amt,mi.ItemName,ms.SMName,ms.SyncId,t1.Vdate ORDER BY t1.VDate desc";

                    DataTable dtMastItem12 = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
                    if (dtMastItem12.Rows.Count > 0)
                    {
                        rptmain.Style.Add("display", "block");
                        salevaluerpt.DataSource = dtMastItem12;
                        salevaluerpt.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        rptmain.Style.Add("display", "block");
                        salevaluerpt.DataSource = dtMastItem12;
                        salevaluerpt.DataBind();
                        btnExport.Visible = false;
                    }
                }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
                            salevaluerpt.DataSource = null;
                            salevaluerpt.DataBind();
                        }
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    salevaluerpt.DataSource = null;
                    salevaluerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SaleValueBreakupReport.aspx");
        }

        protected void matGrpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            //         string message = "";
            foreach (ListItem matGrp in matGrpListBox.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

            if (matGrpStr != "")
            { //Ankita - 13/may/2016- (For Optimization)
                //string mastItemQry1 = @"select * from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                ClearControls();
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=SaleValueBreakupReport.csv");
            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "DocId".TrimStart('"').TrimEnd('"') + ", " + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Item Sync Id".TrimStart('"').TrimEnd('"') + "," + "Net Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            DataTable dtParams = new DataTable();
            //dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("ProductGroup", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("CurrentMonthAmount", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Discount", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("NetAmount", typeof(String)));

            //foreach (RepeaterItem item in salevaluerpt.Items)
            //{
            //    DataRow dr = dtParams.NewRow();
            //    Label VDateLabel = item.FindControl("VDateLabel") as Label;
            //    dr["Date"] = VDateLabel.Text;
            //    Label NameLabel = item.FindControl("NameLabel") as Label;
            //    dr["SalesPerson"] = NameLabel.Text.ToString();
            //    Label SyncIdLabel = item.FindControl("SyncIdLabel") as Label;
            //    dr["SyncId"] = SyncIdLabel.Text.ToString();
            //    Label DistributorNameLabel = item.FindControl("DistributorNameLabel") as Label;
            //    dr["DistributorName"] = DistributorNameLabel.Text.ToString();

            //    Label MaterialGroupLabel = item.FindControl("MaterialGroupLabel") as Label;
            //    dr["ProductGroup"] = MaterialGroupLabel.Text.ToString();
            //    Label ItemNameLabel = item.FindControl("ItemNameLabel") as Label;
            //    dr["Product"] = ItemNameLabel.Text.ToString();
            //    Label CurrentMonthAmountLabel = item.FindControl("CurrentMonthAmountLabel") as Label;
            //    dr["CurrentMonthAmount"] = CurrentMonthAmountLabel.Text.ToString();
            //    Label DiscountLabel = item.FindControl("DiscountLabel") as Label;
            //    dr["Discount"] = DiscountLabel.Text.ToString();
            //    Label NetAmountLabel = item.FindControl("NetAmountLabel") as Label;
            //    dr["NetAmount"] = NetAmountLabel.Text.ToString();

            //    dtParams.Rows.Add(dr);
            //}
            string Qrychk = "", Query = "", distIdStr1 = "", QueryMatGrp = "", matGrpStrNew = "", matProStrNew = "";
            //foreach (ListItem item in ListBox1.Items)
            //{
            //    if (item.Selected)
            //    {
            //        smIDStr1 += item.Value + ",";
            //    }
            //}
            //smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
            //foreach (TreeNode node in trview.CheckedNodes)
            //{
            //    smIDStr1 = node.Value;
            //    {
            //        smIDStr += node.Value + ",";
            //    }
            //}
            //smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    distIdStr1 += item.Value + ",";
                }
            }
            distIdStr1 = distIdStr1.TrimStart(',').TrimEnd(',');
             //For MatGrp
            foreach (ListItem matGrpItems in matGrpListBox.Items)
            {
                if (matGrpItems.Selected)
                {
                    matGrpStrNew += matGrpItems.Value + ",";
                }
            }
            matGrpStrNew = matGrpStrNew.TrimStart(',').TrimEnd(',');

            //For Product
            foreach (ListItem product in productListBox.Items)
            {
                if (product.Selected)
                {
                    matProStrNew += product.Value + ",";
                }
            }
            matProStrNew = matProStrNew.TrimStart(',').TrimEnd(',');
            if (matGrpStrNew != "" && matProStrNew != "")
            {
                QueryMatGrp = " and mi.ItemId in (" + matProStrNew + ") ";
            }
            if (matGrpStrNew != "" && matProStrNew == "")
            {
                QueryMatGrp = " and mi1.ItemId in (" + matGrpStrNew + ") ";
            }
            if (distIdStr1 != "")
            {
                if (txtfmDate.Text != string.Empty && txttodate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                    {
                        salevaluerpt.DataSource = new DataTable();
                        salevaluerpt.DataBind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                        return;
                    }
                    if (Convert.ToDateTime(txtfmDate.Text) <= Convert.ToDateTime(txttodate.Text))
                    {

                        Qrychk = " tdv1.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and tdv1.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                        string matGrpStr = "";                       
                        foreach (ListItem matGrp in matGrpListBox.Items)
                        {
                            if (matGrp.Selected)
                            {
                                matGrpStr += matGrp.Value + ",";
                            }
                        }
                        matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');

                        if (matGrpStr != "")
                        {
                            Query = " and mi1.ItemId in (" + matGrpStr + ") ";
                        }    
//                        string qry = @"select Convert(varchar(15),CONVERT(date,tdv.VDate,103),106) AS VDate,tdv.DistInvDocId as Docid,ms.SMName AS Name,ms.SyncId, mp.PartyName AS DistributorName,max(mi1.ItemName) AS MaterialGroup,mi.ItemName,mi.syncid as Item_Syncid, Sum(tdv1.Amount) as NetAmount from TransDistInv tdv Left join TransDistInv1 tdv1 on tdv.DistInvId=tdv1.DistInvId and tdv.DistInvDocId=tdv1.DistInvDocId 
//                        LEFT JOIN mastparty mp ON mp.PartyId=tdv.DistId left join mastsalesrep ms on ms.SMId=tdv.SMID LEFT JOIN mastitem mi ON mi.ItemId=tdv1.ItemId LEFT JOIN MastItem mi1 ON mi.Underid=mi1.ItemId where tdv1.DistId in (" + distIdStr1 + ") and mp.PartyDist=1 " + Query + "  and " + Qrychk + " group by tdv.DistInvDocId,ms.SMName,ms.SyncId,mp.SyncId,mp.PartyName,tdv.VDate,mi.ItemName,mi.syncid ORDER BY tdv.VDate desc";
                        string qry = @"select Convert(varchar(15),CONVERT(date,tdv1.VDate,103),106) AS VDate,tdv1.DistInvDocId as Docid,mp.PartyName AS DistributorName,max(mi1.ItemName) AS MaterialGroup,mi.ItemName,mi.syncid as Item_Syncid, Sum(tdv1.Amount) as NetAmount from TransDistInv1 tdv1 LEFT JOIN mastparty mp ON mp.PartyId=tdv1.DistId 
                        LEFT JOIN mastitem mi ON mi.ItemId=tdv1.ItemId LEFT JOIN MastItem mi1 ON mi.Underid=mi1.ItemId where tdv1.DistId in (" + distIdStr1 + ") and mp.PartyDist=1 " + Query + "  and " + Qrychk + " group by tdv1.DistInvDocId,mp.SyncId,mp.PartyName,tdv1.VDate,mi.ItemName,mi.syncid ORDER BY tdv1.VDate desc";

                        dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, qry);

                        for (int j = 0; j < dtParams.Rows.Count; j++)
                        {
                            for (int k = 0; k < dtParams.Columns.Count; k++)
                            {
                                if (dtParams.Rows[j][k].ToString().Contains(","))
                                {
                                    if (k == 0)
                                    {
                                        string h = dtParams.Rows[j][k].ToString();
                                        string d = h.Replace(",", " ");
                                        dtParams.Rows[j][k] = "";
                                        dtParams.Rows[j][k] = d;
                                        dtParams.AcceptChanges();
                                        sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                    }
                                    else
                                    {
                                        string h = dtParams.Rows[j][k].ToString();
                                        string d = h.Replace(",", " ");
                                        dtParams.Rows[j][k] = "";
                                        dtParams.Rows[j][k] = d;
                                        dtParams.AcceptChanges();
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
                        Response.AddHeader("content-disposition", "attachment;filename=SaleValueBreakupReport.csv");
                        Response.Write(sb.ToString());
                        Response.End();                       
                    }
                }
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string citystr = "";
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    { citystr += dtCity.Rows[i]["AreaId"] + ","; }
                    citystr = citystr.TrimStart(',').TrimEnd(',');                  
                    string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();
                    }
                }
                else
                { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);  salevaluerpt.DataSource = null;  salevaluerpt.DataBind(); }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            string smIDStr = "", smIDStr12 = "";
            if (cnt == 0)
            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                string smiMStr = smIDStr12;
                if (smIDStr12 != "")
                {
                    BindDistributorDDl(smIDStr12);
                }
                else
                {
                    ListBox1.Items.Clear();
                    ListBox1.DataBind();

                }
                ViewState["tree"] = smiMStr;

            }

            cnt = cnt + 1;
            return;
            //string smIDStr = "", smIDStr12 = "";

            //{
            //    foreach (TreeNode node in trview.CheckedNodes)
            //    {
            //        smIDStr12 = node.Value;
            //        {
            //            smIDStr += node.Value + ",";
            //        }
            //    }
            //    smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
            //    Session["treenodes"] = smIDStr12;

            //}


        }

    }
}