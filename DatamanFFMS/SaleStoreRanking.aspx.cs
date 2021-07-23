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
    public partial class SaleStoreRanking : System.Web.UI.Page
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
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
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
                //  BindRetailer();
                fillDDLDirect(ddlretailer, "select partyid,partyname from mastparty where partydist=0 and active=1 order by partyname asc", "partyid", "partyname", 1);
            }
        }

        //private void BindRetailer()
        //{
        //    try
        //    { 

        //        string prodClassQry = @"select partyid,partyname from mastparty where partydist=0 and active=1 order by partyname asc";
        //        DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
        //        if (dtProdRep.Rows.Count > 0)
        //        {

        //            //listretailer.DataSource = dtProdRep;
        //            //listretailer.DataTextField = "partyname";
        //            //listretailer.DataValueField = "partyid";
        //            //listretailer.DataBind();

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

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
        //    {  //Ankita - 13/may/2016- (For Optimization)
        //        //  lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
        //        // St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,Convert.ToInt16(Settings.Instance.SalesPersonLevel));
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


        //        // FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
        //    }
        //}
        ////Ankita - 16/may/2016- (For Optimization)
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
            Response.Redirect("~/SaleStoreRanking.aspx");
        }

        //protected void btnGo_Click(object sender, EventArgs e)
        //{
        //    string smIDStr = "", Qrychk = "", Query = "";
        //    string smIDStr1 = "";
        //    //         string message = "";
        //    foreach (ListItem item in ListBox1.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            smIDStr1 += item.Value + ",";
        //        }
        //    }
        //    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
        //    foreach (TreeNode node in trview.CheckedNodes)
        //    {
        //        smIDStr1 = node.Value;
        //        {
        //            smIDStr += node.Value + ",";
        //        }
        //    }
        //    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
        //    //Qrychk = " lr.FromDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.ToDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
        //    Qrychk = " (lr.FromDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.FromDate<='" + Settings.dateformat(txttodate.Text) + " 23:59' or lr.ToDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and lr.ToDate<='" + Settings.dateformat(txttodate.Text) + " 23:59')";

        //    if (ddlLeavStatus.SelectedValue != "0" && ddlLeavStatus.SelectedValue != "")
        //        Qrychk = Qrychk + " and lr.AppStatus='" + ddlLeavStatus.SelectedValue + "'";

        //    if (smIDStr1 != "")
        //    {
        //        if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
        //        {
        //            leavereportrpt.DataSource = new DataTable();
        //            leavereportrpt.DataBind();
        //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
        //            return;
        //        }

        //        Query = "select lr.FromDate, lr.Reason, lr.AppStatus,lr.NoOfDays, cp1.SMName as AppByName, lr.ToDate,cp.SMName,cp.SyncId from TransLeaveRequest lr left join MastSalesRep cp on cp.SMId=lr.SMId left join MastSalesRep cp1 on cp1.SMId=lr.AppBySMId where " + Qrychk + " and lr.SMId in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + "))) order by SMname";
        //        DataTable dtLeaveRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
        //        if (dtLeaveRep.Rows.Count > 0)
        //        {
        //            leavereportrpt.DataSource = dtLeaveRep;
        //            leavereportrpt.DataBind();
        //            btnExport.Visible = true;
        //        }
        //        else
        //        {
        //            leavereportrpt.DataSource = dtLeaveRep;
        //            leavereportrpt.DataBind();
        //            btnExport.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select sales person');", true);
        //        leavereportrpt.DataSource = null;
        //        leavereportrpt.DataBind();
        //    }

        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {


            Response.Clear();//lblsrno
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=Sale&StoreRanking.csv");
            string headertext = "S.No.".TrimStart('"').TrimEnd('"') + "," + "Salesperson/Retailer".TrimStart('"').TrimEnd('"') + "," + "Total Visited Party".TrimStart('"').TrimEnd('"') + "," + "Orderd Party".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"') + "," + "Failed Visit".TrimStart('"').TrimEnd('"') + "," + "Demo".TrimStart('"').TrimEnd('"') + "," + "Efficiency".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;
            //
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Sno", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Salesperson/Retailer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotalVisitedParty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OrderdParty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OrderAmount", typeof(String)));

            dtParams.Columns.Add(new DataColumn("FailedVisit", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Demo", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Efficiency", typeof(String)));

            //
            foreach (RepeaterItem item in leavereportrpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblsrno = item.FindControl("lblsrno") as Label;
                dr["Sno"] = lblsrno.Text;
                Label smNameLabel = item.FindControl("smnameLabel") as Label;
                dr["Salesperson/Retailer"] = smNameLabel.Text;
                Label syncIdLabel = item.FindControl("syncIdLabel") as Label;
                dr["TotalVisitedParty"] = syncIdLabel.Text.ToString();
                Label nofdaysLabel = item.FindControl("Label1") as Label;
                dr["OrderdParty"] = nofdaysLabel.Text.ToString();
                Label fromDateLabel = item.FindControl("nofdaysLabel") as Label;
                dr["OrderAmount"] = fromDateLabel.Text.ToString();

                Label todateLabel = item.FindControl("fromDateLabel") as Label;
                dr["FailedVisit"] = todateLabel.Text.ToString();
                Label demoLabel = item.FindControl("lbldemo") as Label;
                dr["Demo"] = demoLabel.Text.ToString();
                Label EfficiencyLabel = item.FindControl("EfficiencyLabel") as Label;
                dr["Efficiency"] = EfficiencyLabel.Text.ToString();

                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "efficiency desc";
            DataTable sortedDT = dv.ToTable();
            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 3)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                            //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
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
            Response.AddHeader("content-disposition", "attachment;filename=Sale&StoreRanking.csv");
            Response.Write(sb.ToString());
            // HttpContext.Current.ApplicationInstance.CompleteRequest();
            Response.End();

            sb.Clear();
            dtParams.Dispose();
            sortedDT.Dispose();
            dv.Dispose();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void btnsalespersonranking_Click(object sender, EventArgs e)
        {
            String smIDStr1 = "";
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 += node.Value + ",";
            }
            string smid = smIDStr1.TrimEnd(',');
            //GetPartyNames(smid, Convert.ToDateTime(txtfmDate.Text).ToString("yyyy/MM/dd"));
            DataTable dt = new DataTable();
            //string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand("sp_salesreportLalMahal"))
            //    {
            //        cmd.Connection = con;
            //        cmd.Parameters.AddWithValue("@SMID", smid);
            //        cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(txtfmDate.Text).ToString("yyyy/MM/dd"));
            //        cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(txttodate.Text).ToString("yyyy/MM/dd"));
            //        cmd.Parameters.AddWithValue("@SaleOrStore", "Sale");
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //        {
            //            dt = new DataTable();
            //            sda.Fill(dt);
            //        }
            //    }
            //}

            //string sql = "select * , orderdparty+FailedVisit TotalVisitedParty ,case when orderdparty+FailedVisit > 0 then";
            //       sql+="         convert(varchar,convert(decimal(10,2) ,cast (ROUND(CAST(case when orderdparty =0 then 1 else  orderdparty end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit =0 then 1 else  orderdparty+FailedVisit end ) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2))  )) +'%'  else 'Never Visited' end  as Efficiencyinpercent,";

            //       sql += "            case when orderdparty+FailedVisit > 0 then";
            //         sql+="       cast (ROUND(CAST(case when orderdparty =0 then 1 else  orderdparty end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit =0 then 1 else  orderdparty+FailedVisit end) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2)) else 0 end as efficiency  from ";
            //    sql+="(";
            //    sql += "select count(Tro.partyid) orderdparty,sum(Tro.OrderAmount) OrderAmount, msp.smname,tro.smid,";
            //    sql+="(select count(partyid) failedvisit from TransFailedVisit ";
            //    sql += " where smid IN (tro.SMId) and  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') FailedVisit ";
            //    sql+="from TransOrder Tro";
            //    sql+=" LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId ";
            //    sql += " where tro.smid IN ("+smid+") and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "'";
            //    sql+=" GROUP BY msp.smname,tro.smid";
            //    sql += ") tbl order by efficiency desc";
            string sql = "  select ROW_NUMBER()  OVER (ORDER BY   efficiency desc) As SrNo,* from ( select * , orderdparty+FailedVisit+Demo TotalVisitedParty ,case when orderdparty+FailedVisit+Demo > 0 then         convert(varchar,convert(decimal(10,2) ,cast (ROUND(CAST(case when orderdparty+Demo  =0 then 1 else  orderdparty+Demo  end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit+Demo =0 then 1 else  orderdparty+FailedVisit+Demo end ) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2))  )) +'%'  else 'Never Visited' end  as Efficiencyinpercent,            case when orderdparty+FailedVisit+Demo > 0 then       cast (ROUND(CAST(case when orderdparty+Demo  =0 then 1 else  orderdparty+Demo  end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit+Demo =0 then 1 else  orderdparty+FailedVisit+Demo end) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2)) else 0 end as efficiency  from (";
            sql += " select msp1.SMName,msp1.smid, ";
            sql += " (select count(Tro.partyid) from TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid =msp1.smid";
            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid) orderdparty,";
            sql += " (select sum(Tro.OrderAmount)  from TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid =msp1.smid";
            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid) OrderAmount";
            sql += " ,";
            sql += " (select (select count(partyid) failedvisit from TransFailedVisit  where smid IN (tro.SMId) and  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') FailedVisit  from ";
            sql += " TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid = msp1.smid";
            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid)  FailedVisit ";


            sql += " ,";
            sql += " (select (select count(partyid) demo from Transdemo  where smid IN (tro.SMId) and  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') demo  from ";
            sql += " TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid = msp1.smid";
            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid)  demo ";

            sql += " from MastSalesRep msp1 where msp1.smid in (" + smid + ") ) tbl    ) tbl2 order by efficiency desc";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "efficiency desc";
                DataTable sortedDT = dv.ToTable();
                btnExport.Visible = true;
                btnExport.Visible = true;
                leavereportrpt.DataSource = sortedDT;
                leavereportrpt.DataBind();
                sortedDT.Dispose();
                dv.Dispose();
            }
            else
            {
                leavereportrpt.DataSource = dt;
                leavereportrpt.DataBind();
                btnExport.Visible = false;
            }
            dt.Dispose();
        }

        protected void btnstoreranking_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            //string retailer = "";
            ////         string message = "";
            //foreach (ListItem matGrp in listretailer.Items)
            //{
            //    if (matGrp.Selected)
            //    {
            //        retailer += matGrp.Value + ",";
            //    }
            //}
            //retailer = retailer.TrimStart(',').TrimEnd(',');
            string sql = " select ROW_NUMBER()  OVER (ORDER BY   efficiency desc) As SrNo,* from (  select * ,'" + ddlretailer.SelectedItem.Text + "' as smname, orderdparty+FailedVisit+demo TotalVisitedParty , case when orderdparty+FailedVisit+demo > 0 then convert(varchar,convert(decimal(10,2) ,cast (ROUND(CAST(case when orderdparty+demo =0 then 1 else  orderdparty+demo end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit+demo =0 then 1 else  orderdparty+FailedVisit+demo end ) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2))  )) +'%'   else 'Never Visited' end  as Efficiencyinpercent,   case when orderdparty+FailedVisit+demo > 0 then cast (ROUND(CAST(case when orderdparty+demo =0 then 1 else  orderdparty+demo end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit+demo =0 then 1 else  orderdparty+FailedVisit+demo end) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2)) else 0 end as efficiency  from  ( select count(tro.ordid) as orderdparty  ,sum(Tro.OrderAmount) as OrderAmount , (select count(fvid) as failedvisit from TransFailedVisit  where  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and partyid=" + ddlretailer.SelectedItem.Value + ") FailedVisit , (select count(demoid) as demo from transdemo  where  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and partyid=" + ddlretailer.SelectedItem.Value + ") demo from TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and tro.PartyId=" + ddlretailer.SelectedItem.Value + "  ) tbl group by orderdparty,OrderAmount,FailedVisit,demo  ) tbl2 order by efficiency desc";


            dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "efficiency desc";
                DataTable sortedDT = dv.ToTable();
                btnExport.Visible = true;
                leavereportrpt.DataSource = sortedDT;
                leavereportrpt.DataBind();

                dv.Dispose();
                sortedDT.Dispose();
            }
            else
            {
                leavereportrpt.DataSource = dt;
                leavereportrpt.DataBind();
                btnExport.Visible = false;
            }
            dt.Dispose();
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

        protected void listretailer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}