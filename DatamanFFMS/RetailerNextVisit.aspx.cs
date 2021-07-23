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

namespace AstralFFMS
{
    public partial class RetailerNextVisit : System.Web.UI.Page
    {
        string roleType = "";
        TreeNode spnode;
        string Id = "";
        string type = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {

                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();

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
                fillDDLDirect(ddlretailer, "select partyid,partyname from mastparty where active=1 and partydist=1 order by partyname asc", "partyid", "partyname", 1);
                fillDDLDirect(ddlbeat, "select areaid,areaname from mastarea where active=1 and areatype='BEAT' order by areaname asc", "areaid", "areaname", 1);
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

        private void FillDetail()
        {
            string str = "";
            if (type =="DV")
            {
                str = "select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') AS address,msp.smname,tfs.VisDistId AS Fvid,tfs.nextvisitdate AS nextvisit,tfs.NextVisitTime AS VisitTime,tfs.DiscDocid AS Docid from transVisitDist tfs left join mastparty mp on   tfs.DistId=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where tfs.VisDistId =" + Id + "";
            
            }
            else if (type =="FV")
            {
                str = "select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') as address,msp.smname,tfs.fvid,tfs.nextvisit,tfs.VisitTime ,tfs.FVDocId AS Docid from TransFailedVisit tfs left join mastparty mp on   tfs.partyid=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where tfs.fvid =" + Id + "";
            }

            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                leavereportrpt.DataSource = dt;
                leavereportrpt.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                leavereportrpt.DataSource = null;
                leavereportrpt.DataBind();
                btnExport.Visible = false;
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
        //void fill_TreeArea()
        //{
        //    int lowestlvl = 0;
        //    DataTable St = new DataTable();
        //    if (roleType == "Admin")
        //    {

        //        St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
        //    }
        //    else
        //    {
        //        St = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT mastrole.rolename,mastsalesrep.smid,smname + ' (' + mastsalesrep.syncid + ' - '+ mastrole.rolename + ')' AS smname, mastsalesrep.lvl,mastrole.roletype FROM   mastsalesrep LEFT JOIN mastrole ON mastrole.roleid = mastsalesrep.roleid WHERE  smid =" + Settings.Instance.SMID + "");
        //    }


        //    trview.Nodes.Clear();

        //    if (St.Rows.Count <= 0)
        //    {
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found !');", true);
        //        return;
        //    }
        //    foreach (DataRow row in St.Rows)
        //    {

        //        TreeNode tnParent = new TreeNode();
        //        tnParent.Text = row["SMName"].ToString();
        //        tnParent.Value = row["SMId"].ToString();
        //        trview.Nodes.Add(tnParent);
        //        //tnParent.ExpandAll();
        //        tnParent.CollapseAll();
        //        getchilddata(tnParent, tnParent.Value);
        //    }
        //}

        //private void getchilddata(TreeNode parent, string ParentId)
        //{

        //    string SmidVar = string.Empty;
        //    string GetFirstChildData = string.Empty;
        //    int levelcnt = 0;
        //    if (Settings.Instance.RoleType == "Admin")
        //        //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
        //        levelcnt = Convert.ToInt16("0") + 2;
        //    else
        //        levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 1;


        //    GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 order by SMName,lvl desc ";
        //    DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

        //    if (FirstChildDataDt.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
        //        {
        //            SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
        //            FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
        //        }
        //        SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

        //        for (int j = levelcnt + 1; j <= 6; j++)
        //        {
        //            string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
        //            DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
        //            //  SmidVar = string.Empty;
        //            int mTotRows = dtChild.Rows.Count;
        //            if (mTotRows > 0)
        //            {
        //                SmidVar = string.Empty;
        //                var str = "";
        //                for (int k = 0; k < mTotRows; k++)
        //                {
        //                    SmidVar += dtChild.Rows[k]["smid"].ToString() + ",";
        //                }

        //                TreeNode Oparent = parent;
        //                switch (j)
        //                {
        //                    case 3:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        break;
        //                    case 4:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        break;
        //                    case 5:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                            {
        //                                foreach (TreeNode child in Pchild.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }


        //                        break;
        //                    case 6:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            if (Settings.Instance.RoleType == "Admin")
        //                            {
        //                                foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
        //                                {
        //                                    foreach (TreeNode Qchild in Pchild.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode child in Qchild.ChildNodes)
        //                                        {
        //                                            str += child.Value + ","; parent = child;
        //                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                            for (int l = 0; l < dr.Length; l++)
        //                                            {
        //                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                            }
        //                                            dtChild.Select();
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            else
        //                            {
        //                                foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                                {
        //                                    foreach (TreeNode child in Pchild.ChildNodes)
        //                                    {
        //                                        str += child.Value + ","; parent = child;
        //                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                        for (int l = 0; l < dr.Length; l++)
        //                                        {
        //                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                        }
        //                                        dtChild.Select();
        //                                    }
        //                                }
        //                            }

        //                        }

        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }

        //                        break;
        //                }

        //                SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);
        //            }
        //        }
        //    }


        //}

        //public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        //{


        //    TreeNode child = new TreeNode();
        //    child.Text = SMName;
        //    child.Value = Smid;
        //    child.SelectAction = TreeNodeSelectAction.Expand;
        //    parent.ChildNodes.Add(child);
        //    child.CollapseAll();


        //}



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RetailerNextVisit.aspx");
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


            sb.Clear();
            dtParams.Dispose();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }




        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }

        protected void btnsaleperson_Click(object sender, EventArgs e)
        {
            GetDetail("S");
        }

        protected void btndist_Click(object sender, EventArgs e)
        {
            GetDetail("D");
        }

        protected void btnbeat_Click(object sender, EventArgs e)
        {
            GetDetail("B");
        }
        private void GetDetail(string flag)
        {
            DataTable dt = new DataTable(); string sql = "";

            if (flag == "S")
            {
                String smIDStr1 = "";
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 += node.Value + ",";
                }
                string smid = smIDStr1.TrimEnd(',');
                sql = "SELECT   Row_number() OVER(ORDER BY Docid DESC, PartyName ) SrNo ,a.PartyName,a.address,a.smname,a.fvid,a.nextvisit,a.VisitTime,a.Docid  FROM   (select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') as address,msp.smname,tfs.fvid,tfs.nextvisit,tfs.VisitTime,tfs.FVDocId AS Docid from TransFailedVisit tfs left join mastparty mp on   tfs.partyid=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where tfs.smid in (" + smid + ") and tfs.Nextvisit='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "'";
                sql = sql + " union all ";
                sql = sql + " select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') AS address,msp.smname,tfs.VisDistId AS Fvid,tfs.nextvisitdate AS nextvisit,tfs.NextVisitTime AS VisitTime,tfs.DiscDocid AS Docid from transVisitDist tfs left join mastparty mp on   tfs.DistId=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where tfs.smid in (" + smid + ") and tfs.NextvisitDate='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' AND Isnull(tfs.Type,'')='')AS a";


            }
            if (flag == "B")
            {
                sql = "select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') As address,msp.smname,tfs.fvid,tfs.nextvisit,tfs.VisitTime from TransFailedVisit tfs left join mastparty mp on  tfs.partyid=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where  tfs.Nextvisit='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and tfs.partyid in (select partyid from mastparty where beatid=" + ddlbeat.SelectedItem.Value + " and partydist=0)";
            }
            if (flag == "D")
            {
                sql = "select ROW_NUMBER()  OVER (ORDER BY   mp.PartyName asc) As SrNo,mp.PartyName,Isnull(MP.Address1,'')+' '+Isnull(MP.Address2,'') As address,msp.smname,tfs.fvid,tfs.nextvisit,tfs.VisitTime from TransFailedVisit tfs left join mastparty mp on                           tfs.partyid=mp.partyid left join mastsalesrep msp on msp.smid=tfs.smid where  tfs.Nextvisit='" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and tfs.partyid in (select partyid from mastparty where beatid in (select areaid  from mastarea where underid in (select areaid from mastparty where partyid=" + ddlretailer.SelectedItem.Value + ")) )";
            }
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                leavereportrpt.DataSource = dt;
                leavereportrpt.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                leavereportrpt.DataSource = null;
                leavereportrpt.DataBind();
                btnExport.Visible = false;
            }
            sql = null;
            // return dt;
            dt.Dispose();
        }
    }
}