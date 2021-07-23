using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class RetailerNxtVst_V2 : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        TreeNode spnode;
        string Id = "";
        string type = "";
        DataTable YrTable = new DataTable("Type");
        DataColumn dtColumn;
        DataRow myDataRow;
        DataTable gvdt = new DataTable("Sales Person");
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                //BindStaffChkList()
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                YrTable.Columns.Add("TypId", typeof(int));
                YrTable.Columns.Add("TypName", typeof(string));

                //End
                //Ankita - 13/may/2016- (For Optimization)
                roleType = Settings.Instance.RoleType;
                //GetRoleType(Settings.Instance.RoleID);
                //BindSalePersonDDl();
                //fill_TreeArea();
                //btnExport.Visible = false;
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";
                //fillDDLDirect(ddlretailer, "select partyid,(partyName+'-'+Mobile) AS partyName from mastparty where active=1 and partydist=1 order by partyname asc", "partyid", "partyname", 1);
                //fillDDLDirect(ddlbeat, "select areaid,areaname from mastarea where active=1 and areatype='BEAT' order by areaname asc", "areaid", "areaname", 1);

                fillDDLDirect(ListArea, "select Distinct AreaId,AreaName from MastArea where AreaType='Area' order by AreaName asc", "AreaId", "AreaName", 1);

                fillDDLDirect(ListDist, "select Distinct PartyId,PartyName from MastParty where PartyDist=1 order by PartyName asc", "PartyId", "PartyName", 1);
                BindStaffChkList();
            }
            if (Request.QueryString["Id"] != null)
            {
                Id = Request.QueryString["Id"].ToString();
            }

            if (Request.QueryString["type"] != null)
            {
                type = Request.QueryString["type"].ToString();
            }

            if (Id != "" && type != "")
            {
                FillDetail();
            }
        }

        private void BindStaffChkList()
        {
            try
            {


                ListBox lstyr = new ListBox();
                for (int i = 1; i <= 3; i++)
                {
                    DataRow row = YrTable.NewRow();
                    row["TypId"] = i.ToString();
                    if (i == 1)
                    {
                        row["TypName"] = "Sales Person";
                    }
                    else if (i == 2)
                    {
                        row["TypName"] = "Distributor";
                    }
                    else if (i == 3)
                    {
                        row["TypName"] = "Beat";
                    }
                    YrTable.Rows.Add(row);
                }
                LstType.DataSource = YrTable;
                LstType.DataTextField = "TypName";
                LstType.DataValueField = "TypId";
                LstType.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void FillDetail()
        {
            string str = "";
            if (type == "DV")
            {
                str = "select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') AS address,msp.smname,tfs.VisDistId AS Fvid,tfs.nextvisitdate AS nextvisit,tfs.NextVisitTime AS VisitTime,tfs.DiscDocid AS Docid from transVisitDist tfs left join mastparty mp on   tfs.DistId=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where tfs.VisDistId =" + Id + "";

            }
            else if (type == "FV")
            {
                str = "select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') as address,msp.smname,tfs.fvid,tfs.nextvisit,tfs.VisitTime ,tfs.FVDocId AS Docid from TransFailedVisit tfs left join mastparty mp on   tfs.partyid=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where tfs.fvid =" + Id + "";
            }

            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                leavereportrpt.DataSource = dt;
                leavereportrpt.DataBind();
                //btnExport.Visible = true;
            }
            else
            {
                leavereportrpt.DataSource = null;
                leavereportrpt.DataBind();
                //btnExport.Visible = false;
            }
            str = null;

            dt.Dispose();
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



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RetailerNxtVst_V2.aspx");
        }



        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";//SrNo
            Response.AddHeader("content-disposition", "attachment;filename=ReatilerNextVisit.csv");
            string headertext = "S.No.".TrimStart('"').TrimEnd('"') + ",Salesperson".TrimStart('"').TrimEnd('"') + "," + "Retailer".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Next Visit Date".TrimStart('"').TrimEnd('"') + "," + "Visit Time".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable(); dtParams.Columns.Add(new DataColumn("SNO", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Salesperson", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Retailer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
            dtParams.Columns.Add(new DataColumn("NextVisitDate", typeof(String)));

            dtParams.Columns.Add(new DataColumn("VisitTime", typeof(String)));

            //
            foreach (RepeaterItem item in leavereportrpt.Items)
            {
                DataRow dr = dtParams.NewRow(); Label ssnoLabel = item.FindControl("lblsno") as Label;
                dr["SNO"] = ssnoLabel.Text;
                Label smNameLabel = item.FindControl("smnameLabel") as Label;
                dr["Salesperson"] = smNameLabel.Text;
                Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                dr["Retailer"] = syncIdLabel.Text.ToString();
                Label nofdaysLabel = item.FindControl("Label1") as Label;
                dr["Address"] = nofdaysLabel.Text.ToString();
                Label fromDateLabel = item.FindControl("nofdaysLabel") as Label;
                dr["NextVisitDate"] = fromDateLabel.Text.ToString();

                Label todateLabel = item.FindControl("fromDateLabel") as Label;
                dr["VisitTime"] = todateLabel.Text.ToString();
                //Label reasonLabel = item.FindControl("reasonLabel") as Label;
                //dr["Reason"] = reasonLabel.Text.ToString();
                //Label appstatusLabel = item.FindControl("appstatusLabel") as Label;
                //dr["Status"] = appstatusLabel.Text.ToString();
                //Label appbynameLabel = item.FindControl("appbynameLabel") as Label;
                //dr["ApprovedBy"] = appbynameLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 4)
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
                        if (k == 4)
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
                        if (k == 4 || k == 4)
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
            Response.AddHeader("content-disposition", "attachment;filename=ReatilerNextVisit.csv");
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();

            dtParams.Dispose();


        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }




        public static void fillDDLDirect(ListBox xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            //if (sele == 1)
            //{
            //    if (xdt.Rows.Count >= 1)
            //        xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            //    else if (xdt.Rows.Count == 0)
            //        xddl.Items.Insert(0, new ListItem("---", "0"));
            //}
            //else if (sele == 2)
            //{
            //    xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            //}
            xdt.Dispose();
        }

        protected void btnsaleperson_Click(object sender, EventArgs e)
        {
            //spinner.Visible = true;
            if (LstType.SelectedIndex == 0)
            {
                GetDetail("S");
            }
            else if (LstType.SelectedIndex == 1)
            {
                GetDetail("D");
            }
            else if (LstType.SelectedIndex == 2)
            {
                GetDetail("B");
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Type');", true);
            }
        }

        protected void btndist_Click(object sender, EventArgs e)
        {

        }

        protected void btnbeat_Click(object sender, EventArgs e)
        {

        }
        private void GetDetail(string flag)
        {
            DataTable dt = new DataTable(); string sql = "", Qrychk = "";
            DataSet dataSet = new DataSet("Report");
            if (flag == "S")
            {
                String smIDStr1 = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 += node.Value + ",";
                }
                smIDStr1 = smIDStr1.TrimEnd(',');
                if (smIDStr1 != "")
                {
                    Qrychk = "(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                    string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                    DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtdesig.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdesig.Rows.Count; i++)
                        {
                            gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                        }
                    }

                    gvdt.Columns.Add(new DataColumn("ReportPerson", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("SalesPersonName", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("State", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("City", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Area", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Beat", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Outlet", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Mobile", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Address", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Next Visit", typeof(DateTime)));
                    gvdt.Columns.Add(new DataColumn("Visit Time", typeof(DateTime)));

                    sql = "SELECT a.Report,a.smid,a.smname,a.State,a.City,a.Area,a.Beat,a.PartyName,a.Mobile,a.address,a.nextvisit,a.VisitTime,a.Docid  FROM   ( " +
                    " select (mp.PartyName+'-'+mp.SyncId) As PartyName,mp.Mobile,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') as address,(msp2.smname+'-'+msp2.SyncId) As Report,msp.smid,msp.smname,tfs.fvid,tfs.nextvisit,tfs.VisitTime,tfs.FVDocId AS Docid,(mss.AreaName+''+mss.SyncId) As State,(msc.AreaName+''+msc.SyncId) As City,(msa.AReaName+''+msa.SyncId) As Area,(msb.AreaName+''+msb.SyncId) AS Beat from TransFailedVisit tfs left join mastparty mp on tfs.partyid=mp.partyid Left Join MastArea msa on msa.AreaId=mp.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId Left Join MastArea msb on msb.AreaId=mp.BeatId left join mastsalesrep msp on msp.smid=tfs.smid left join MastSalesRep msp2 on msp2.SMId=msp.UnderId where tfs.smid in (" + smIDStr1 + ") and tfs.Nextvisit='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'" +
                    " and partydist=0)AS a";

                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataView dvSales = new DataView(dt);
                        foreach (DataRow drvst in dt.Rows)
                        {
                            dvSales.RowFilter = "SMID=" + drvst["smid"].ToString();
                            if (dvSales.ToTable().Rows.Count > 0)
                            {
                                DataTable dtsp = dvSales.ToTable();
                                DataRow dr = dtsp.Rows[0];
                                DataRow mDataRow = gvdt.NewRow();

                                str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMID"].ToString() + " and maingrp<>" + dr["SMID"].ToString() + ")";
                                DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                                if (dtsr.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtsr.Rows.Count; j++)
                                    {
                                        if (dtsr.Rows[j]["DesName"].ToString() != "")
                                        {
                                            mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SyncId"].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < dtdesig.Rows.Count; i++)
                                {
                                    if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                    {
                                        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                    }
                                }
                                /*a.Report,a.smname,a.State,a.City,a.Area,a.Beat,a.PartyName,a.Mobile,a.address,a.nextvisit,a.VisitTime*/
                                mDataRow["ReportPerson"] = dr["Report"].ToString();
                                mDataRow["SalesPersonName"] = dr["smname"].ToString();
                                mDataRow["State"] = dr["State"].ToString();
                                mDataRow["City"] = dr["City"].ToString();
                                mDataRow["Area"] = dr["Area"].ToString();
                                mDataRow["Beat"] = dr["Beat"].ToString();
                                mDataRow["Outlet"] = dr["PartyName"].ToString();
                                mDataRow["Mobile"] = dr["Mobile"].ToString();
                                mDataRow["Address"] = dr["address"].ToString();
                                mDataRow["Next Visit"] = dr["nextvisit"].ToString();
                                mDataRow["Visit Time"] = dr["VisitTime"].ToString();


                                dvSales.RowFilter = null;
                                gvdt.Rows.Add(mDataRow);
                                gvdt.AcceptChanges();
                            }
                        }

                        dataSet.Tables.Add(gvdt);
                        dtdesig.Dispose();
                        gvdt.Dispose();
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                }

            }
            else if (flag == "B")
            {
                string area = "", beat = "", Qry = "";
                foreach (ListItem item in ListArea.Items)
                {
                    if (item.Selected)
                    {
                        area += item.Value + ",";
                    }
                }
                area = area.TrimEnd(',');

                foreach (ListItem item in ListBeat.Items)
                {
                    if (item.Selected)
                    {
                        beat += item.Value + ",";
                    }
                }
                beat = beat.TrimEnd(',');

                if (beat != "")
                {
                    Qry = Qry + "and beatid in (" + beat + ")";
                }

                if (area != "")
                {
                    Qrychk = "(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (select msp.smid from TransFailedVisit tfs left join mastparty mp on  tfs.partyid=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid left join MastSalesRep msp2 on msp2.SMId=msp.UnderId where  tfs.Nextvisit='2021-02-20' and tfs.partyid in (select partyid from mastparty where areaid in(" + area + ")" + Qry + "and partydist=0))) and Active=1)";

                    string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                    DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtdesig.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdesig.Rows.Count; i++)
                        {
                            gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                        }
                    }

                    gvdt.Columns.Add(new DataColumn("ReportPerson", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("SalesPersonName", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("State", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("City", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Area", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Beat", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Outlet", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Mobile", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Address", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Next Visit", typeof(DateTime)));
                    gvdt.Columns.Add(new DataColumn("Visit Time", typeof(DateTime)));

                    sql = "select (msp2.smname+'-'+msp2.SyncId) As Report,msp.smid,(msp.smname+'-'+msp.SyncId) as smname,(mss.AreaName+'-'+mss.SyncId) As State,(msc.AreaName+'-'+msc.SyncId) As City,(msa.AReaName+'-'+msa.SyncId) As Area,(msb.AreaName+'-'+msb.SyncId) AS Beat,(mp.PartyName+'-'+mp.SyncId) as PartyName ,mp.Mobile,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') As address,tfs.nextvisit,tfs.VisitTime from TransFailedVisit tfs left join mastparty mp on  tfs.partyid=mp.partyid Left Join MastArea msa on msa.AreaId=mp.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId Left Join MastArea msb on msb.AreaId=mp.BeatId left join mastsalesrep msp on msp.smid=tfs.smid left join MastSalesRep msp2 on msp2.SMId=msp.UnderId where  tfs.Nextvisit='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and tfs.partyid in (select partyid from mastparty where areaid in (" + area + ")" + Qry + " and partydist=0)";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);

                    DataView dvSales = new DataView(dt);
                    foreach (DataRow drvst in dt.Rows)
                    {
                        dvSales.RowFilter = "SMID=" + drvst["smid"].ToString();
                        if (dvSales.ToTable().Rows.Count > 0)
                        {
                            DataTable dtsp = dvSales.ToTable();
                            DataRow dr = dtsp.Rows[0];
                            DataRow mDataRow = gvdt.NewRow();

                            str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMID"].ToString() + "  and Level not in (select top 2 level from MastSalesRepGrp where  SMId=" + dr["SMID"].ToString() + "))";
                            DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    if (dtsr.Rows[j]["DesName"].ToString() != "")
                                    {
                                        mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SyncId"].ToString();
                                    }
                                }
                            }

                            for (int i = 0; i < dtdesig.Rows.Count; i++)
                            {
                                if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                {
                                    mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                }
                            }
                            /*a.Report,a.smname,a.State,a.City,a.Area,a.Beat,a.PartyName,a.Mobile,a.address,a.nextvisit,a.VisitTime*/
                            mDataRow["ReportPerson"] = dr["Report"].ToString();
                            mDataRow["SalesPersonName"] = dr["smname"].ToString();
                            mDataRow["State"] = dr["State"].ToString();
                            mDataRow["City"] = dr["City"].ToString();
                            mDataRow["Area"] = dr["Area"].ToString();
                            mDataRow["Beat"] = dr["Beat"].ToString();
                            mDataRow["Outlet"] = dr["PartyName"].ToString();
                            mDataRow["Mobile"] = dr["Mobile"].ToString();
                            mDataRow["Address"] = dr["address"].ToString();
                            mDataRow["Next Visit"] = dr["nextvisit"].ToString();
                            mDataRow["Visit Time"] = dr["VisitTime"].ToString();


                            dvSales.RowFilter = null;
                            gvdt.Rows.Add(mDataRow);
                            gvdt.AcceptChanges();
                        }
                    }

                    dataSet.Tables.Add(gvdt);

                    dtdesig.Dispose();
                    gvdt.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Area/Beat');", true);
                }
            }
            else if (flag == "D")
            {
                string distrb = "";
                foreach (ListItem item in ListDist.Items)
                {
                    if (item.Selected)
                    {
                        distrb += item.Value + ",";
                    }
                }
                distrb = distrb.TrimEnd(',');
                if (distrb != "")
                {
                    Qrychk = "select msp.smid from TransFailedVisit tfs left join mastparty mp on tfs.partyid=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid left join MastSalesRep msp2 on msp2.SMId=msp.UnderId where  tfs.Nextvisit='2021-02-20' and tfs.partyid in (select partyid from mastparty where beatid in (select areaid from mastarea where underid in (select areaid from mastparty where partyid in (" + distrb + "))))";

                    string str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                    DataTable dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtdesig.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdesig.Rows.Count; i++)
                        {
                            gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                        }
                    }

                    gvdt.Columns.Add(new DataColumn("ReportPerson", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("SalesPersonName", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("State", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("City", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Area", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Beat", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Outlet", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Mobile", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Address", typeof(String)));
                    gvdt.Columns.Add(new DataColumn("Next Visit", typeof(DateTime)));
                    gvdt.Columns.Add(new DataColumn("Visit Time", typeof(DateTime)));

                    sql = "select (msp2.smname+'-'+msp2.SyncId) As Report,msp.smid,(msp.smname+'-'+msp.SyncId) as smname,(mss.AreaName+'-'+mss.SyncId) As State,(msc.AreaName+'-'+msc.SyncId) As City,(msa.AReaName+'-'+msa.SyncId) As Area,(msb.AreaName+'-'+msb.SyncId) AS Beat,(mp.PartyName+'-'+mp.SyncId) as PartyName,mp.Mobile,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') As address,tfs.nextvisit,tfs.VisitTime from TransFailedVisit tfs left join mastparty mp on tfs.partyid=mp.partyid Left Join MastArea msa on msa.AreaId=mp.AreaId Left Join MastArea msc on msc.AreaId = msa.UnderId Left Join MastArea msd on msd.AreaId = msc.UnderId Left Join MastArea mss on mss.AreaId = msd.UnderId Left Join MastArea msb on msb.AreaId=mp.BeatId left join mastsalesrep msp on msp.smid=tfs.smid left join MastSalesRep msp2 on msp2.SMId=msp.UnderId where  tfs.Nextvisit='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and tfs.partyid in (select partyid from mastparty where beatid in (select areaid from mastarea where underid in (select areaid from mastparty where partyid in (" + distrb + "))))";

                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);

                    DataView dvSales = new DataView(dt);
                    foreach (DataRow drvst in dt.Rows)
                    {
                        dvSales.RowFilter = "SMID=" + drvst["smid"].ToString();
                        if (dvSales.ToTable().Rows.Count > 0)
                        {
                            DataTable dtsp = dvSales.ToTable();
                            DataRow dr = dtsp.Rows[0];
                            DataRow mDataRow = gvdt.NewRow();

                            str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMID"].ToString() + "  and Level not in (select top 2 level from MastSalesRepGrp where  SMId=" + dr["SMID"].ToString() + "))";
                            DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                            if (dtsr.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtsr.Rows.Count; j++)
                                {
                                    if (dtsr.Rows[j]["DesName"].ToString() != "")
                                    {
                                        mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SyncId"].ToString();
                                    }
                                }
                            }

                            for (int i = 0; i < dtdesig.Rows.Count; i++)
                            {
                                if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                                {
                                    mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                                }
                            }
                            /*a.Report,a.smname,a.State,a.City,a.Area,a.Beat,a.PartyName,a.Mobile,a.address,a.nextvisit,a.VisitTime*/
                            mDataRow["ReportPerson"] = dr["Report"].ToString();
                            mDataRow["SalesPersonName"] = dr["smname"].ToString();
                            mDataRow["State"] = dr["State"].ToString();
                            mDataRow["City"] = dr["City"].ToString();
                            mDataRow["Area"] = dr["Area"].ToString();
                            mDataRow["Beat"] = dr["Beat"].ToString();
                            mDataRow["Outlet"] = dr["PartyName"].ToString();
                            mDataRow["Mobile"] = dr["Mobile"].ToString();
                            mDataRow["Address"] = dr["address"].ToString();
                            mDataRow["Next Visit"] = dr["nextvisit"].ToString();
                            mDataRow["Visit Time"] = dr["VisitTime"].ToString();


                            dvSales.RowFilter = null;
                            gvdt.Rows.Add(mDataRow);
                            gvdt.AcceptChanges();
                        }
                    }

                    dataSet.Tables.Add(gvdt);

                    dtdesig.Dispose();
                    gvdt.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Distributor');", true);
                }
            }

            if (dataSet.Tables.Count > 0)
            {
                try
                {
                    Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "";

                    if (flag == "S")
                    {
                        filename = "SP Wise Next Visit.xlsx";
                    }
                    else if (flag == "B")
                    {
                        filename = "Beat Wise Next Visit.xlsx";
                    }
                    else if (flag == "D")
                    {
                        filename = "Distributor Wise Next Visit.xlsx";
                    }
                    if (File.Exists(path + filename))
                    {
                        File.Delete(path + filename);
                    }

                    //Excel.Application excelApp = new Excel.Application();
                    string strPath = HttpContext.Current.Server.MapPath("ExportedFiles//" + filename);
                    Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel.Range chartRange;
                    Excel.Range range;
                    Excel.Sheets xlSheets = null;
                    Excel.Worksheet xlWorksheet = null;
                    // Loop over DataTables in DataSet.
                    DataTableCollection collection = dataSet.Tables;



                    for (int i = collection.Count; i > 0; i--)
                    {
                        //Create Excel Sheets
                        xlSheets = ExcelApp.Sheets;
                        xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                                       Type.Missing, Type.Missing, Type.Missing);

                        System.Data.DataTable table = collection[i - 1];

                        if (flag == "S")
                        {
                            xlWorksheet.Name = "SP Next Visit";
                        }
                        else if (flag == "B")
                        {
                            xlWorksheet.Name = "Beat Next Visit";
                        }
                        else if (flag == "D")
                        {
                            xlWorksheet.Name = "Dist Next Visit";
                        }

                        for (int j = 1; j < table.Columns.Count + 1; j++)
                        {
                            ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;

                            range = xlWorksheet.Cells[1, j] as Excel.Range;
                            range.Cells.Font.Name = "Calibri";
                            range.Cells.Font.Bold = true;
                            range.Cells.Font.Size = 11;
                            range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                        }

                        // Storing Each row and column value to excel sheet
                        for (int k = 0; k < table.Rows.Count; k++)
                        {
                            for (int l = 0; l < table.Columns.Count; l++)
                            {
                                ExcelApp.Cells[k + 2, l + 1] =
                                table.Rows[k].ItemArray[l].ToString();
                            }
                        }
                        ExcelApp.Columns.AutoFit();
                        xlWorksheet.Activate();
                        xlWorksheet.Application.ActiveWindow.SplitRow = 1;
                        xlWorksheet.Application.ActiveWindow.FreezePanes = true;

                        Excel.Range last = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                        chartRange = xlWorksheet.get_Range("A1", last);
                        foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                        {
                            cell.BorderAround2();
                        }
                        Excel.FormatConditions fcs = chartRange.FormatConditions;
                        Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
        (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                        format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                    }
                    xlWorkbook.SaveAs(strPath);
                    xlWorkbook.Close();
                    ExcelApp.Quit();
                    Response.ContentType = "Application/xlsx";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.TransmitFile(strPath);
                    Response.End();

                    //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                    //ExcelApp.Visible = true;
                }
                catch (Exception ex)
                {

                }

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record File Generated Successfully');", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Record Found');", true);
            }

            //if (dt.Rows.Count > 0)
            //{
            //    leavereportrpt.DataSource = dt;
            //    leavereportrpt.DataBind();
            //    btnExport.Visible = true;
            //}
            //else
            //{
            //    leavereportrpt.DataSource = null;
            //    leavereportrpt.DataBind();
            //    btnExport.Visible = false;
            //}
            sql = null;
            dt.Dispose();
            dataSet.Dispose();
            // return dt;
        }

        protected void LstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstType.SelectedIndex == 0)
            {
                ListDist.SelectedIndex = -1;
                ListArea.SelectedIndex = -1;
                ListBeat.SelectedIndex = -1;

                sprw.Visible = true;
                dstrw.Visible = false;
                btrw.Visible = false;
            }
            else if (LstType.SelectedIndex == 1)
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }

                ListArea.SelectedIndex = -1;
                ListBeat.SelectedIndex = -1;
                //LstSecPrsn.SelectedIndex = -1;

                sprw.Visible = false;
                dstrw.Visible = true;
                btrw.Visible = false;
            }
            else if (LstType.SelectedIndex == 2)
            {
                foreach (TreeNode node in trview.Nodes)
                {
                    CheckItems(node);
                }
                trview_TreeNodeCheckChanged(trview, null);
                ListDist.SelectedIndex = -1;
                //lstPrmPrsn.SelectedIndex = -1;

                sprw.Visible = false;
                dstrw.Visible = false;
                btrw.Visible = true;
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Type');", true);
            }
        }

        private void CheckItems(TreeNode node)
        {
            node.Checked = false;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                childNode.Checked = false;
                CheckItems(childNode);
            }
        }

        protected void ListArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string areastr = "";
                //         string message = "";
                foreach (ListItem areagrp in ListArea.Items)
                {
                    if (areagrp.Selected)
                    {
                        areastr += areagrp.Value + ",";
                    }
                }
                areastr = areastr.TrimStart(',').TrimEnd(',');
                if (areastr != "")
                {
                    string beatqry = @"select AreaId,AreaName from mastarea where Underid in ( " + areastr + " ) and areatype='Beat' and Active=1 order by AreaName";
                    DataTable dtbeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatqry);
                    if (dtbeat.Rows.Count > 0)
                    {
                        ListBeat.DataSource = dtbeat;
                        ListBeat.DataTextField = "AreaName";
                        ListBeat.DataValueField = "AreaId";
                        ListBeat.DataBind();
                    }

                    dtbeat.Dispose();
                }
                else
                {
                    ListBeat.Items.Clear();
                }
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
                ViewState["tree"] = smiMStr;
            }

            cnt = cnt + 1;
            return;
        }
    }
}