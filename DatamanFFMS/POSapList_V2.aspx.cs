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
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class POSapList_V2 : System.Web.UI.Page
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
                     
                    roleType = Settings.Instance.RoleType;
                   
                    BindTreeViewControl();
                   
                    txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                    txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                  
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
      
        [WebMethod(EnableSession = true)]
        public static string getdistributorledger(string Distid, string Fromdate, string Todate)
        {
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
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                   
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
                { 
                    string strrole = "select mastrole.RoleName,MastSalesRep.SMId,MastSalesRep.SMName,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastSalesRep.Active=1 and SMName<>'.' order by MastSalesRep.SMName";
                    DataTable dtcheckrole = new DataTable();
                    dtcheckrole = DbConnectionDAL.GetDataTable(CommandType.Text, strrole);
                    DataView dv1 = new DataView(dtcheckrole);
                    dv1.RowFilter = "SMName<>.";
                    dv1.Sort = "SMName asc";

                    salespersonListBox.DataSource = dv1.ToTable();
                    salespersonListBox.DataTextField = "SMName";
                    salespersonListBox.DataValueField = "SMId";
                    salespersonListBox.DataBind();
                }
                else
                {
                    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dt);
                  
                    dv.RowFilter = "SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        salespersonListBox.DataSource = dv.ToTable();
                        salespersonListBox.DataTextField = "SMName";
                        salespersonListBox.DataValueField = "SMId";
                        salespersonListBox.DataBind();
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
                tnParent.CollapseAll();
                getchilddata(tnParent, tnParent.Value);
            }
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
                   
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                str = @"SELECT DISTINCT max(T1.PODocId) as[Order No.],max(t1.Vdate) as [Date],max(mp.PartyName) as [Distributor Name],max(mp.SyncId) as SyncId,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS [Branch Name],sum(T1.ItemwiseTotal) AS [Net Amount]
                      FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId  left join MastParty mp on mp.PartyId=T1.DistId  where t1.Vdate>='" + Settings.dateformat1(txtfmDate.Text) + "' and mp.PartyDist=1 and t1.Vdate<='" + Settings.dateformat1(txttodate.Text) + "' " +
                     " and T1.DistId in (" + smIDStr1 + ") GROUP BY T1.PODocId  ORDER BY [Date] DESC  ";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    ExportDataSetToExcel(dt);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found');", true);

                }
                            
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select distributor');", true);
              
            }
           
        }

      
        protected void salespersonListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string smIDStr12 = "";            
            foreach (ListItem saleperson in salespersonListBox.Items)
            {
                if (saleperson.Selected)
                {
                    smIDStr12 += saleperson.Value + ",";
                }
            }
            smIDStr12 = smIDStr12.TrimStart(',').TrimEnd(',');
            BindDistributorDDl(smIDStr12);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/POSapList_V2.aspx");
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
                   smIDStr += node.Value + ",";               
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
        private void ExportDataSetToExcel(DataTable table)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();
            string path = Server.MapPath("ExportedFiles//");

            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
            {
                Directory.CreateDirectory(path);
            }
            string filename = "Pending Order Details Report  " + txtfmDate.Text.Replace('/', ' ') + "-" + txttodate.Text.Replace('/', ' ') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            if (table.Rows.Count > 0)
            {
                //Add a new worksheet to workbook with the Datatable name

                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = "Pending Order Details";

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                    range = excelWorkSheet.Cells[1, i] as Excel.Range;
                    range.Cells.Font.Name = "Calibri";
                    range.Cells.Font.Bold = true;
                    range.Cells.Font.Size = 11;
                    range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int l = 0; l < table.Columns.Count; l++)
                    {
                        excelWorkSheet.Cells[j + 2, l + 1] = table.Rows[j].ItemArray[l].ToString();
                    }
                }

                Excel.Range last = excelWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                chartRange = excelWorkSheet.get_Range("A1", last);
                foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                {
                    cell.BorderAround2();
                }


                Excel.FormatConditions fcs = chartRange.FormatConditions;
                Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                format.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                excelWorkSheet.Columns.AutoFit();

                excelWorkSheet.Application.ActiveWindow.SplitRow = 1;
                excelWorkSheet.Application.ActiveWindow.FreezePanes = true;

                Excel.Worksheet worksheet = (Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
                worksheet.Delete();

                excelWorkBook.SaveAs(strPath);
                excelWorkBook.Close();
                excelApp.Quit();
                Response.ContentType = "application/xlsx";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.TransmitFile(strPath);
                Response.End();

            }



        }
  
    }
}