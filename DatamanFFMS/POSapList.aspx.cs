using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DAL;
using System.Web.UI.HtmlControls;
using System.Text;
using Newtonsoft.Json;
using System.Web.Services;

namespace AstralFFMS
{
    public partial class POSapList : System.Web.UI.Page
    {
        BAL.PurchaseOrder.PurchaseOrder dp = new BAL.PurchaseOrder.PurchaseOrder();       
        string roleType = "";
        int cnt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                txtfmDate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
                //trview.Attributes.Add("onclick", "postBackByObject()");
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
                if (!IsPostBack)
                {
                    //Ankita - 18/may/2016- (For Optimization)
                    //GetRoleType(Settings.Instance.RoleID);
                    List<Distributors> distributors = new List<Distributors>();
                    distributors.Add(new Distributors());
                    rpt.DataSource = distributors;
                    rpt.DataBind();    
                    roleType = Settings.Instance.RoleType;
                    //BindSalePersonDDl();
                    //fill_TreeArea();
                    BindTreeViewControl();
                    //Added By - Nishu 06/12/2015 
                    txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                    txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                    //End
                    btnExport.Visible = true;
                    //string pageName = Path.GetFileName(Request.Path);
                    btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                    btnExport.CssClass = "btn btn-primary";
                }
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
        public class Distributors
        {

            public string PODocId { get; set; }
            public string VDate { get; set; }
            public string DistName { get; set; }
            public string SyncId { get; set; }
            public string PortalNo { get; set; }
            public string ResCenName { get; set; }
            public string ItemwiseTotal { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public static string getdistributorledger(string Distid, string Fromdate, string Todate)
        {

            //string Qrychk = " t1.VDate>='" + Settings.dateformat(Fromdate) + " 00:00' and t1.VDate<='" + Settings.dateformat(Todate) + " 23:59'";
            //            string query = @"SELECT t1.VDate,da.PartyName AS Distributor,max(mi1.ItemName) AS MaterialGROUP, mi.ItemName AS [Item],sum(qty) as Qty,sum(Amount) as Amount from TransDistInv1 t1 left outer join MastItem mi
            //                           on mi.ItemId=t1.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid left outer join MastParty da on da.PartyId=t1.DistId where (" + Qrychk + ") group BY  t1.VDate,da.PartyName, mi.ItemName order BY  t1.VDate desc";

            
            string query = @"SELECT DISTINCT max(T1.PODocId) as PODocId,max(t1.Vdate) as VDate,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS ResCenName,sum(T1.ItemwiseTotal) AS ItemwiseTotal,max(mp.PartyName) as DistName,max(mp.SyncId) as SyncId
                      FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId  left join MastParty mp on mp.PartyId=T1.DistId where t1.Vdate>='" + Settings.dateformat1(Fromdate) + "' and mp.PartyDist=1 and t1.Vdate<='" + Settings.dateformat1(Todate) + "' " +
                     " and T1.DistId in (" + Distid + ") GROUP BY T1.PODocId  ORDER BY VDate DESC ";

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            return JsonConvert.SerializeObject(dtItem);


        }
        private void BindDistributorDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {

                    string citystr = "";
                    string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
                    DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        citystr += dtCity.Rows[i]["AreaId"] + ",";
                    }
                    citystr = citystr.TrimStart(',').TrimEnd(',');
                    //string distqry = @"select * from MastParty where CityId in (" + citystr + ") and PartyDist=1 and Active=1 order by PartyName";
                    string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + ")))  and PartyDist=1 and Active=1 order by PartyName";                                                    
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        ListBox1.DataSource = dtDist;
                        ListBox1.DataTextField = "PartyName";
                        ListBox1.DataValueField = "PartyId";
                        ListBox1.DataBind();

                    }

                    dtCity.Dispose();
                    dtDist.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //private void BindSalePersonDDl()
        //{
        //    try
        //    {
        //        if (roleType == "Admin")
        //        {  //Ankita - 18/may/2016- (For Optimization)
        //            //string strrole = "select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
        //            DataTable dtcheckrole = new DataTable();
        //            dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
        //            DataView dv1 = new DataView(dtcheckrole);
        //            dv1.RowFilter = "SMName<>.";
        //            dv1.Sort = "SMName asc";

        //            salespersonListBox.DataSource = dv1.ToTable();
        //            salespersonListBox.DataTextField = "SMName";
        //            salespersonListBox.DataValueField = "SMId";
        //            salespersonListBox.DataBind();
        //        }
        //        else
        //        {
        //            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
        //            DataView dv = new DataView(dt);
        //            //     dv.RowFilter = "RoleName='Level 1'";
        //            dv.RowFilter = "SMName<>.";
        //            dv.Sort = "SMName asc";
        //            if (dv.ToTable().Rows.Count > 0)
        //            {
        //                salespersonListBox.DataSource = dv.ToTable();
        //                salespersonListBox.DataTextField = "SMName";
        //                salespersonListBox.DataValueField = "SMId";
        //                salespersonListBox.DataBind();
        //            }
        //        }
        //        //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
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

            St.Dispose();
        }
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

                    dtChild.Dispose();
                }
            }
            FirstChildDataDt.Dispose();
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
            string str = "", smIDStr1 = "";
           
            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
          
            if (smIDStr1 != "")
            {
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    rpt.DataSource = new DataTable();
                    rpt.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                str = @"SELECT DISTINCT max(T1.PODocId) as PODocId,max(t1.Vdate) as VDate,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS ResCenName,sum(T1.ItemwiseTotal) AS ItemwiseTotal,max(mp.PartyName) as DistName,max(mp.SyncId) as SyncId
                      FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId  left join MastParty mp on mp.PartyId=T1.DistId where t1.Vdate>='" + Settings.dateformat1(txtfmDate.Text) + "' and mp.PartyDist=1 and t1.Vdate<='" + Settings.dateformat1(txttodate.Text) + "' " +
                     " and T1.DistId in (" + smIDStr1 + ") GROUP BY T1.PODocId  ORDER BY VDate DESC ";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    rpt.DataSource = dt;
                    rpt.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                    btnExport.Visible = false;
                }
                dt.Dispose();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select distributor');", true);
                rpt.DataSource = null;
                rpt.DataBind();
            }
           
        }

        private void fillRepeter()
        {
            string str = @"SELECT DISTINCT T1.PODocId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,
                                T1.PortalNo,T2.ResCenName,T1.ItemwiseTotal 
                                FROM PurchaseOrderImport AS T1
                                Left JOIN MastResCentre AS T2
                                ON T1.LocationID=T2.ResCenId
                                ORDER BY VDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if(dt!=null && dt.Rows.Count>0)
            {
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
            dt.Dispose();
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void ImgDownload_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField POID = (HiddenField)item.FindControl("HiddenField1");
            HiddenField PODocID = (HiddenField)item.FindControl("HiddenField2");
            string strDocId = PODocID.Value;
            strDocId = strDocId.Replace(" ", "_");
            string strFileName = "Dealer_Order_" + strDocId;

            Response.Clear();
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName + "");
            Response.WriteFile(Server.MapPath(@"~/TextFileFolder/Dealer_Order_" + strDocId + ".txt"));
            Response.End();
        }

        //protected void salespersonListBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string smIDStr12 = "";            
        //    foreach (ListItem saleperson in salespersonListBox.Items)
        //    {
        //        if (saleperson.Selected)
        //        {
        //            smIDStr12 += saleperson.Value + ",";
        //        }
        //    }
        //    smIDStr12 = smIDStr12.TrimStart(',').TrimEnd(',');
        //    BindDistributorDDl(smIDStr12);
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/POSapList.aspx");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDistributor(string prefixText)
        {          
            string str = @"select T1.*,T2.AreaName FROM MastParty AS T1 INNER JOIN MastArea AS T2
                            ON T1.CityId=T2.AreaId  where ((T2.AreaName like '%" + prefixText + "%') OR (T1.PartyName like '%" + prefixText + "%') OR (T1.SyncId like '%" + prefixText + "%')) and T1.PartyDist=1 and T1.Active=1  " +
                          " and T1.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                  " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
                      
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["AreaName"].ToString() + ")" + " " + dt.Rows[i]["PartyName"].ToString() + " " + "(" + dt.Rows[i]["SyncId"].ToString() + ")", dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

        //protected void btnExport_Click(object sender, EventArgs e)
        //{

        //    Response.Clear();
        //    Response.ContentType = "text/csv";
        //    Response.AddHeader("content-disposition", "attachment;filename=PendingOrderReport.csv");
        //    string headertext = "Order No.".TrimStart('"').TrimEnd('"') + "," + "Date".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "SyncId".TrimStart('"').TrimEnd('"') + "," + "PortalNo".TrimStart('"').TrimEnd('"') + "," + "Branch Name".TrimStart('"').TrimEnd('"') + "," + "Net".TrimStart('"').TrimEnd('"');

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(headertext);
        //    sb.AppendLine(System.Environment.NewLine);
        //    string dataText = string.Empty;
        //    //
        //    DataTable dtParams = new DataTable();
        //    dtParams.Columns.Add(new DataColumn("OrderNo", typeof(String)));
        //    dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
        //    dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
        //    dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));
        //    dtParams.Columns.Add(new DataColumn("PortalNo", typeof(String)));
        //    dtParams.Columns.Add(new DataColumn("BranchName", typeof(String)));
        //    dtParams.Columns.Add(new DataColumn("NetAmount", typeof(String)));

        //    foreach (RepeaterItem item in rpt.Items)
        //    {
        //        DataRow dr = dtParams.NewRow();
        //        Label PODocIdLabel = item.FindControl("PODocIdLabel") as Label;
        //        dr["OrderNo"] = PODocIdLabel.Text;
        //        Label VDateLabel = item.FindControl("VDateLabel") as Label;
        //        dr["Date"] = VDateLabel.Text.ToString();
        //        Label DistNameLabel = item.FindControl("DistNameLabel") as Label;
        //        dr["DistributorName"] = DistNameLabel.Text.ToString();
        //        Label SyncIdLabel = item.FindControl("SyncIdLabel") as Label;
        //        dr["SyncId"] = SyncIdLabel.Text.ToString();
        //        Label PortalNoLabel = item.FindControl("PortalNoLabel") as Label;
        //        dr["PortalNo"] = PortalNoLabel.Text.ToString();
        //        Label ResCenNameLabel = item.FindControl("ResCenNameLabel") as Label;
        //        dr["BranchName"] = ResCenNameLabel.Text.ToString();
        //        Label ItemwiseTotalLabel = item.FindControl("ItemwiseTotalLabel") as Label;
        //        dr["NetAmount"] = ItemwiseTotalLabel.Text.ToString();

        //        dtParams.Rows.Add(dr);
        //    }

        //    for (int j = 0; j < dtParams.Rows.Count; j++)
        //    {
        //        for (int k = 0; k < dtParams.Columns.Count; k++)
        //        {
        //            if (dtParams.Rows[j][k].ToString().Contains(","))
        //            {
        //                if (k == 1)
        //                {
        //                    sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
        //                }
        //                else
        //                {
        //                    sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
        //                }
        //            }
        //            else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
        //            {
        //                if (k == 1)
        //                {
        //                    sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
        //                }
        //                else
        //                {
        //                    sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
        //                }
        //            }
        //            else
        //            {
        //                if (k == 1 || k == 1)
        //                {
        //                    sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
        //                }
        //                else
        //                {
        //                    sb.Append(dtParams.Rows[j][k].ToString() + ',');
        //                }

        //            }
        //        }
        //        sb.Append(Environment.NewLine);
        //    }
        //    Response.Clear();
        //    Response.ContentType = "text/csv";
        //    Response.AddHeader("content-disposition", "attachment;filename=PendingOrderReport.csv");
        //    Response.Write(sb.ToString());
        //    Response.End();         

        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=PendingOrderReport.csv");
            string headertext = "Order No.".TrimStart('"').TrimEnd('"') + "," + "Date".TrimStart('"').TrimEnd('"') + "," + "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "SyncId".TrimStart('"').TrimEnd('"') + "," + "PortalNo".TrimStart('"').TrimEnd('"') + "," + "Branch Name".TrimStart('"').TrimEnd('"') + "," + "Net".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            //DataTable dtParams = new DataTable();
            //dtParams.Columns.Add(new DataColumn("OrderNo", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("PortalNo", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("BranchName", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("NetAmount", typeof(String)));

            //foreach (RepeaterItem item in rpt.Items)
            //{
            //    DataRow dr = dtParams.NewRow();
            //    Label PODocIdLabel = item.FindControl("PODocIdLabel") as Label;
            //    dr["OrderNo"] = PODocIdLabel.Text;
            //    Label VDateLabel = item.FindControl("VDateLabel") as Label;
            //    dr["Date"] = VDateLabel.Text.ToString();
            //    Label DistNameLabel = item.FindControl("DistNameLabel") as Label;
            //    dr["DistributorName"] = DistNameLabel.Text.ToString();
            //    Label SyncIdLabel = item.FindControl("SyncIdLabel") as Label;
            //    dr["SyncId"] = SyncIdLabel.Text.ToString();
            //    Label PortalNoLabel = item.FindControl("PortalNoLabel") as Label;
            //    dr["PortalNo"] = PortalNoLabel.Text.ToString();
            //    Label ResCenNameLabel = item.FindControl("ResCenNameLabel") as Label;
            //    dr["BranchName"] = ResCenNameLabel.Text.ToString();
            //    Label ItemwiseTotalLabel = item.FindControl("ItemwiseTotalLabel") as Label;
            //    dr["NetAmount"] = ItemwiseTotalLabel.Text.ToString();

            //    dtParams.Rows.Add(dr);
            //}
            string str = "", smIDStr1 = "";

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

            if (smIDStr1 != "")
            {
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    rpt.DataSource = new DataTable();
                    rpt.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                str = @"SELECT DISTINCT max(T1.PODocId) as PODocId,max(t1.Vdate) as Date,max(mp.PartyName) as Distributor_Name,max(mp.SyncId) as SyncId,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS ResCenName,sum(T1.ItemwiseTotal) AS ItemwiseTotal
                      FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId  left join MastParty mp on mp.PartyId=T1.DistId where t1.Vdate>='" + Settings.dateformat1(txtfmDate.Text) + "' and mp.PartyDist=1 and t1.Vdate<='" + Settings.dateformat1(txttodate.Text) + "' " +
                     " and T1.DistId in (" + smIDStr1 + ") GROUP BY T1.PODocId  ORDER BY Date DESC ";

                DataTable dtParams = DbConnectionDAL.GetDataTable(CommandType.Text, str);


                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 1)
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
                            if (k == 1)
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
                            if (k == 1 || k == 1)
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
                Response.AddHeader("content-disposition", "attachment;filename=PendingOrderReport.csv");
                Response.Write(sb.ToString());

                sb.Clear();
                dtParams.Dispose();

                Response.End();

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
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
        }
        protected void ExportCSV(object sender, EventArgs e)
        {
            string str = "", smIDStr1 = "";

            foreach (ListItem item in ListBox1.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

            if (smIDStr1 != "")
            {
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    rpt.DataSource = new DataTable();
                    rpt.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                str = @"SELECT DISTINCT max(T1.PODocId) as PODocId,max(t1.Vdate) as Date,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS ResCenName,sum(T1.ItemwiseTotal) AS ItemwiseTotal,max(mp.PartyName) as Distributor_Name,max(mp.SyncId) as SyncId
                      FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId  left join MastParty mp on mp.PartyId=T1.DistId where t1.Vdate>='" + Settings.dateformat1(txtfmDate.Text) + "' and mp.PartyDist=1 and t1.Vdate<='" + Settings.dateformat1(txttodate.Text) + "' " +
                     " and T1.DistId in (" + smIDStr1 + ") GROUP BY T1.PODocId  ORDER BY Date DESC ";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                {


                    //Build the CSV file data as a Comma separated string.
                    string csv = string.Empty;

                    foreach (DataColumn column in dt.Columns)
                    {
                        //Add the Header row for CSV file.
                        csv += column.ColumnName + ',';
                    }

                    //Add new line.
                    csv += "\r\n";

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            //Add the Data rows.
                            csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                        }

                        //Add new line.
                        csv += "\r\n";
                    }

                    //Download the CSV file.
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=POSapList.csv");
                    string headertext = "Distributor Name".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Year".TrimStart('"').TrimEnd('"') + "," + "Jan".TrimStart('"').TrimEnd('"') + "," + "Feb".TrimStart('"').TrimEnd('"') + "," + "Mar".TrimStart('"').TrimEnd('"') + "," + "Apr".TrimStart('"').TrimEnd('"') + "," + "May".TrimStart('"').TrimEnd('"') + "," + "Jun".TrimStart('"').TrimEnd('"') + "," + "Jul".TrimStart('"').TrimEnd('"') + "," + "Aug".TrimStart('"').TrimEnd('"') + "," + "Sep".TrimStart('"').TrimEnd('"') + "," + "Oct".TrimStart('"').TrimEnd('"') + "," + "Nov".TrimStart('"').TrimEnd('"') + "," + "Dec".TrimStart('"').TrimEnd('"') + "," + "Grand Total".TrimStart('"').TrimEnd('"');
                    StringBuilder sb = new StringBuilder();
                    sb.Append(headertext);
                    sb.AppendLine(System.Environment.NewLine);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(csv);
                    Response.Flush();

                    sb.Clear();
                    Response.End();
                }

                dt.Dispose();
            }
        }
        
            
    }
}