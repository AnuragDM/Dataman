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
    public partial class StoreAgeingReport : System.Web.UI.Page
    {
        string roleType = "";
        TreeNode spnode;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                btnExport.Visible = false;
                // BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                // btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
                fillDDLDirect(ddlretailer, "select partyid,partyname from mastparty where active=1 and partydist=1 order by partyname asc", "partyid", "partyname", 1);
                fillDDLDirect(ddlbeat, "select areaid,areaname from mastarea where active=1 and areatype='BEAT' order by areaname asc", "areaid", "areaname", 1);
                fillDDLDirect(ddlitem, "select Itemid,Itemname from mastitem where active =1 and ItemType='ITEM' order by Itemname asc", "Itemid", "Itemname", 1);
            }
        }


        //private void BindTreeViewControl()
        //{
        //    try
        //    {
        //        DataTable St = new DataTable();
        //        if (roleType == "Admin")
        //        {
        //            //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
        //            St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
        //        }
        //        else
        //        {
        //            string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
        //            St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
        //        }
        //        //    DataSet ds = GetDataSet("Select smid,smname,underid,lvl from mastsalesrep where active=1 and underid<>0 order by smname");


        //        DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
        //        for (int i = 0; i < Rows.Length; i++)
        //        {
        //            TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
        //            root.SelectAction = TreeNodeSelectAction.Expand;
        //            root.CollapseAll();
        //            CreateNode(root, St);
        //            trview.Nodes.Add(root);
        //        }
        //    }
        //    catch (Exception Ex) { throw Ex; }
        //}
        //public void CreateNode(TreeNode node, DataTable Dt)
        //{
        //    DataRow[] Rows = Dt.Select("underid =" + node.Value);
        //    if (Rows.Length == 0) { return; }
        //    for (int i = 0; i < Rows.Length; i++)
        //    {
        //        TreeNode Childnode = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
        //        Childnode.SelectAction = TreeNodeSelectAction.Expand;
        //        node.ChildNodes.Add(Childnode);
        //        Childnode.CollapseAll();
        //        CreateNode(Childnode, Dt);
        //    }
        //}
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
            Response.Redirect("~/StoreAgeingReport.aspx");
        }



        protected void btnExport_Click(object sender, EventArgs e)
        {

            if (leavereportrpt.Visible == true)
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=StoreAgeingItemWise.csv");
                string headertext = "S.No.".TrimStart('"').TrimEnd('"') + "," + "Retailer".TrimStart('"').TrimEnd('"') + "," + "Item Name".TrimStart('"').TrimEnd('"') + "," + "Last Order Date".TrimStart('"').TrimEnd('"') + "," + "Days".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;
                //
                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("SNO", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Retailer", typeof(String)));
                dtParams.Columns.Add(new DataColumn("ItemNAme", typeof(String)));
                dtParams.Columns.Add(new DataColumn("LastOrderDate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Days", typeof(String)));


                foreach (RepeaterItem item in leavereportrpt.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label lblsno = item.FindControl("lblsno") as Label;
                    dr["SNO"] = lblsno.Text;
                    Label smNameLabel = item.FindControl("smnameLabel") as Label;
                    dr["Retailer"] = smNameLabel.Text;
                    Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                    dr["ItemNAme"] = syncIdLabel.Text.ToString();
                    Label nofdaysLabel = item.FindControl("nofdaysLabel") as Label;
                    dr["LastOrderDate"] = nofdaysLabel.Text.ToString();
                    Label fromDateLabel = item.FindControl("fromDateLabel") as Label;
                    dr["Days"] = fromDateLabel.Text.ToString();


                    dtParams.Rows.Add(dr);
                }
                DataView dv = dtParams.DefaultView;
                dv.Sort = "Retailer Asc";
                dtParams = new DataTable();
                dtParams = dv.ToTable();
                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 4)
                            {
                                //  sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
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
                                // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
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
                                //   sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
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
                Response.AddHeader("content-disposition", "attachment;filename=StoreAgeingItemWise.csv");
                Response.Write(sb.ToString());
                Response.End();

                sb.Clear();
                dtParams.Dispose();
                dv.Dispose();

            }
            if (Repeater1.Visible == true)
            {
                Response.Clear();
                Response.ContentType = "text/csv";//
                Response.AddHeader("content-disposition", "attachment;filename=StoreAgeingReport.csv");
                string headertext = "S.No.".TrimStart('"').TrimEnd('"') + "," + "Retailer".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Last Order Date".TrimStart('"').TrimEnd('"') + "," + "Days".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;
                //
                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("SNO", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Retailer", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
                dtParams.Columns.Add(new DataColumn("LastOrderDate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Days", typeof(String)));


                foreach (RepeaterItem item in Repeater1.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label lblsno = item.FindControl("lblsno") as Label;
                    dr["SNO"] = lblsno.Text;
                    Label smNameLabel = item.FindControl("smnameLabel") as Label;
                    dr["Retailer"] = smNameLabel.Text;
                    Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                    dr["Address"] = syncIdLabel.Text.ToString();
                    Label nofdaysLabel = item.FindControl("nofdaysLabel") as Label;
                    dr["LastOrderDate"] = nofdaysLabel.Text.ToString();
                    Label fromDateLabel = item.FindControl("fromDateLabel") as Label;
                    dr["Days"] = fromDateLabel.Text.ToString();


                    dtParams.Rows.Add(dr);
                }
                DataView dv = dtParams.DefaultView;
                dv.Sort = "Retailer Asc";
                dtParams = new DataTable();
                dtParams = dv.ToTable();
                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 3)
                            {
                                //  sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 3)
                            {
                                // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 3 || k == 4)
                            {
                                // sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
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
                Response.AddHeader("content-disposition", "attachment;filename=StoreAgeingReport.csv");
                Response.Write(sb.ToString());
                Response.End();
                sb.Clear();
                dtParams.Dispose();
                dv.Dispose();
            }

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
            GetDetail("D");
        }

        protected void btndist_Click(object sender, EventArgs e)
        {
            GetDetail("B");
        }

        protected void btnbeat_Click(object sender, EventArgs e)
        {
            GetDetail("I");
        }
        private void GetDetail(string flag)
        {
            DataTable dt = new DataTable(); string sql = ""; ;
            if (flag == "I")
            {
                sql = "	Select ROW_NUMBER()  OVER (ORDER BY   PartyName asc) As SrNo,* ,'" + ddlitem.SelectedItem.Text + "' as itname,case when DayGap is null then 'Never Ordered' else  CAST(DayGap AS varchar) end as DayGap1 from (";
                sql += "select mp.partyname,mp.partyid, (select itemname from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) as ItemName,";
                sql += "(select partyname from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) as PartyNameOreder,";
                sql += "(select partyid from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) PartyidOrder,";
                sql += "(select [last order] from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) DayGap,";
                sql += "(select [Last order date] from vw_LalMahalItemWisePartyOrder where itemid=" + ddlitem.SelectedItem.Value + " and partyid=mp.partyid) LastOrderDate";
                sql += " from mastparty mp where mp.partydist=0 and mp.active=1  )tbl order by partyname asc";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    leavereportrpt.Visible = true;
                    leavereportrpt.DataSource = dt;
                    leavereportrpt.DataBind();
                    btnExport.Visible = true;
                    Repeater1.Visible = false;
                    // Button1.Visible = true;

                }
                else
                {
                    leavereportrpt.DataSource = null;
                    leavereportrpt.DataBind();
                    btnExport.Visible = false; Repeater1.Visible = false;

                }
            }
            else if (flag == "B")
            {
                sql = "select ROW_NUMBER()  OVER (ORDER BY   PartyName asc) As SrNo,* , case when [LastOrder] is null then 'Never Ordered' else CAST([LastOrder] AS varchar) + ' days ago' end Gap1 from ( ";
                sql += "select mp.beatid,mp.partyid,mp.PartyName,max(mp.Address) [add],max(tro.OrderAmount) [amt],max(tro.vdate) [date]";
                sql += "        , DATEDIFF(day, max(tro.vdate), getdate()) AS [LastOrder]";
                sql += "        from mastparty mp ";
                sql += "        left join transorder tro on mp.partyid=tro.PartyId";
                sql += "        left join transorder1 tro1 on tro1.ordid=tro.ordid";
                sql += "         where PartyDist=0 and Active=1 and mp.beatid in (" + ddlbeat.SelectedItem.Value + ") and mp.partyid in (select partyid from mastparty where beatid=" + ddlbeat.SelectedItem.Value + " and partydist=0)";
                sql += "            group by  mp.partyid,mp.PartyName,mp.beatid ) tbl ";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    Repeater1.Visible = true;
                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();
                    btnExport.Visible = true; leavereportrpt.Visible = false;
                }
                else
                {
                    Repeater1.DataSource = null;
                    Repeater1.DataBind();
                    btnExport.Visible = false; leavereportrpt.Visible = false;
                }
            }
            if (flag == "D")
            {
                sql = "select ROW_NUMBER()  OVER (ORDER BY   PartyName asc) As SrNo,* , case when [LastOrder] is null then 'Never Ordered' else CAST([LastOrder] AS varchar) + ' days ago' end Gap1 from ( ";
                sql += "select mp.beatid,mp.partyid,mp.PartyName,max(mp.Address) [add],max(tro.OrderAmount) [amt],max(tro.vdate) [date]";
                sql += "        , DATEDIFF(day, max(tro.vdate), getdate()) AS [LastOrder]";
                sql += "        from mastparty mp ";
                sql += "        left join transorder tro on mp.partyid=tro.PartyId";
                sql += "        left join transorder1 tro1 on tro1.ordid=tro.ordid";
                sql += "         where PartyDist=0 and Active=1  and mp.partyid in (select partyid from mastparty where beatid in (select areaid  from mastarea where underid in (select areaid from mastparty where partyid=" + ddlretailer.SelectedItem.Value + "))  and partydist=0)";
                sql += "            group by  mp.partyid,mp.PartyName,mp.beatid ) tbl";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    Repeater1.Visible = true;
                    Repeater1.DataSource = dt;
                    Repeater1.DataBind();
                    btnExport.Visible = true; leavereportrpt.Visible = false;
                }
                else
                {
                    Repeater1.DataSource = null;
                    Repeater1.DataBind();
                    btnExport.Visible = false; leavereportrpt.Visible = false;
                }

            }

            sql = null;
            // return dt;
            dt.Dispose();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=StoreAgeingItemWise.csv");
            string headertext = "Retailer".TrimStart('"').TrimEnd('"') + "," + "Item Name".TrimStart('"').TrimEnd('"') + "," + "Last Order Date".TrimStart('"').TrimEnd('"') + "," + "Days".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Retailer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ItemNAme", typeof(String)));
            dtParams.Columns.Add(new DataColumn("LastOrderDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Days", typeof(String)));


            foreach (RepeaterItem item in leavereportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label smNameLabel = item.FindControl("smnameLabel") as Label;
                dr["Retailer"] = smNameLabel.Text;
                Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                dr["ItemNAme"] = syncIdLabel.Text.ToString();
                Label nofdaysLabel = item.FindControl("nofdaysLabel") as Label;
                dr["LastOrderDate"] = nofdaysLabel.Text.ToString();
                Label fromDateLabel = item.FindControl("fromDateLabel") as Label;
                dr["Days"] = fromDateLabel.Text.ToString();


                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "Retailer Asc";
            dtParams = new DataTable();
            dtParams = dv.ToTable();
            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 3)
                        {
                            //  sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 3)
                        {
                            // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 3 || k == 4)
                        {
                            //   sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            sb.Append(dtParams.Rows[j][k].ToString() + ',');
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
            Response.AddHeader("content-disposition", "attachment;filename=StoreAgeingItemWise.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
            dv.Dispose();
        }

    }
}