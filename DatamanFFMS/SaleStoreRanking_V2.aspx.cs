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
    public partial class SaleStoreRanking_V2 : System.Web.UI.Page
    {
        string roleType = "";
        TreeNode spnode;
        int cnt = 0;
        DataTable gvdt = new DataTable("Party Month Wise Achievement");

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
                //btnExport.Visible = false;
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";
                //  BindRetailer();
                fillDDLDirect(lstAreaBox, "select Distinct AreaId,AreaName from MastArea where AreaType='Area' order by AreaName asc", "AreaId", "AreaName", 1);
            }
        }

        private void BindRetailer()
        {
            try
            {

                string prodClassQry = @"select partyid,partyname from mastparty where partydist=0 and active=1 order by partyname asc";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {

                    //listretailer.DataSource = dtProdRep;
                    //listretailer.DataTextField = "partyname";
                    //listretailer.DataValueField = "partyid";
                    //listretailer.DataBind();

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
        void fill_TreeArea()
        {
            int lowestlvl = 0;
            DataTable St = new DataTable();
            if (roleType == "Admin")
            {

                St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
            }
            else
            {  //Ankita - 13/may/2016- (For Optimization)
                //  lowestlvl = Settings.UnderUsersforlowest(Settings.Instance.SMID);
                // St = Settings.UnderUsersforlowerlevels(Settings.Instance.SMID,Convert.ToInt16(Settings.Instance.SalesPersonLevel));
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


                // FillChildArea(tnParent, tnParent.Value, (Convert.ToInt32(row["Lvl"])), Convert.ToInt32(row["SMId"].ToString()));
            }
        }
        //Ankita - 16/may/2016- (For Optimization)
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



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SaleStoreRanking_V2.aspx");
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




        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void btnsalespersonranking_Click(object sender, EventArgs e)
        {
            spinner.Visible = true;
            String smIDStr1 = "";
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 += node.Value + ",";
            }
            string smid = smIDStr1.TrimEnd(',');
            //GetPartyNames(smid, Convert.ToDateTime(txtfmDate.Text).ToString("yyyy/MM/dd"));
            DataSet dataSet = new DataSet("SPRanking");
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
            gvdt.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            gvdt.Columns.Add(new DataColumn("OrderedParty", typeof(String)));
            gvdt.Columns.Add(new DataColumn("OrderAmount", typeof(String)));
            gvdt.Columns.Add(new DataColumn("FailedVisit", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Demo", typeof(String)));
            gvdt.Columns.Add(new DataColumn("TotalVisitedParty", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Efficiencyinpercent", typeof(String)));
            gvdt.Columns.Add(new DataColumn("Efficiency", typeof(String)));

            string sql = " select ROW_NUMBER()  OVER (ORDER BY   Efficiency desc) As SrNo,* from ( select * , orderedparty+FailedVisit+Demo TotalVisitedParty ,case when orderedparty+FailedVisit+Demo > 0 then         convert(varchar,convert(decimal(10,2) ,cast (ROUND(CAST(case when orderedparty+Demo  =0 then 1 else  orderedparty+Demo  end AS DECIMAL(10,2))/CAST((case when orderedparty+FailedVisit+Demo =0 then 1 else  orderedparty+FailedVisit+Demo end ) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2))  )) +'%'  else 'Never Visited' end  as Efficiencyinpercent, case when orderedparty+FailedVisit+Demo > 0 then       cast (ROUND(CAST(case when orderedparty+Demo  =0 then 1 else  orderedparty+Demo  end AS DECIMAL(10,2))/CAST((case when orderedparty+FailedVisit+Demo =0 then 1 else  orderedparty+FailedVisit+Demo end) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2)) else 0 end as Efficiency  from (";

            sql += " select (msp1.SMName+'-'+IsNull(msp1.SyncId,'')) As SalesPerson,msp1.smid AS SMID,(msp2.SMName+'-'+IsNull(msp2.SyncId,'')) AS ReportPerson, ";

            sql += " (select count(Tro.partyid) from TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid =msp1.smid ";

            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid) AS OrderedParty, ";

            sql += " (select sum(Tro.OrderAmount)  from TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid =msp1.smid ";

            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid) OrderAmount ,";

            sql += " (select (select count(partyid) failedvisit from TransFailedVisit  where smid IN (tro.SMId) and  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') FailedVisit  from  ";

            sql += " TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid = msp1.smid ";

            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid)  FailedVisit  ,";

            sql += "  (select (select count(partyid) AS Demo from Transdemo  where smid IN (tro.SMId) and  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "') AS Demo  from ";

            sql += " TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where tro.smid = msp1.smid ";

            sql += " and  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' GROUP BY msp.smname,tro.smid)  AS Demo  ";

            sql += " from MastSalesRep msp1 left join MastSalesRep msp2 on msp2.SMId=msp1.UnderId where msp1.smid in (" + smid + ") ) tbl    ) tbl2 order by Efficiency desc";

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            DataView dvSales = new DataView(dt);

            foreach (DataRow drvst in dt.Rows)
            {
                dvSales.RowFilter = "SMID=" + drvst["SMID"].ToString();
                if (dvSales.ToTable().Rows.Count > 0)
                {
                    DataTable dtsp = dvSales.ToTable();
                    DataRow dr = dtsp.Rows[0];
                    DataRow mDataRow = gvdt.NewRow();

                    str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMID"].ToString() + "  and maingrp<>" + dr["SMID"].ToString() + ")";
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

                    mDataRow["ReportPerson"] = dr["ReportPerson"].ToString();
                    mDataRow["SalesPerson"] = dr["SalesPerson"].ToString();
                    mDataRow["OrderedParty"] = dr["OrderedParty"].ToString();
                    mDataRow["OrderAmount"] = dr["OrderAmount"].ToString();
                    mDataRow["FailedVisit"] = dr["FailedVisit"].ToString();
                    mDataRow["Demo"] = dr["Demo"].ToString();
                    mDataRow["TotalVisitedParty"] = dr["TotalVisitedParty"].ToString();
                    mDataRow["Efficiencyinpercent"] = dr["Efficiencyinpercent"].ToString();
                    mDataRow["Efficiency"] = dr["Efficiency"].ToString();

                    dvSales.RowFilter = null;
                    gvdt.Rows.Add(mDataRow);
                    gvdt.AcceptChanges();
                }
            }

            dataSet.Tables.Add(gvdt);

            try
            {
                Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                {
                    Directory.CreateDirectory(path);
                }
                string filename = "Sales Person Ranking.xlsx";

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
                    xlWorksheet.Name = "Sales Person Ranking";

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
                    //Excel.FormatCondition format1 = (Excel.FormatCondition)xlWorksheet.Rows.FormatConditions.Add(Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                    format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                }
                xlWorkbook.SaveAs(strPath);
                xlWorkbook.Close();
                ExcelApp.Quit();
                Response.ContentType = "Application/xlsx";
                Response.AppendHeader("Content-Disposition", "attachment; filename="+ filename);
                Response.TransmitFile(strPath);
                Response.End();
                //xlWorkbook.Close();
                //ExcelApp.Quit();
                //((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                //ExcelApp.Visible = true;
            }
            catch (Exception ex)
            {

            }

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record File Generated Successfully');", true);

            spinner.Visible = false;
        }

        protected void btnstoreranking_Click(object sender, EventArgs e)
        {
            spinner.Visible = true;
            DataSet dataSet = new DataSet("RetRanking");
            DataTable dt = new DataTable();
            string retailer = "", Qry = "", beat = "", area = "";

            // Items collection
            foreach (ListItem item in lstAreaBox.Items)
            {
                if (item.Selected)
                {
                    area += item.Value + ",";
                }
            }
            area = area.TrimEnd(',');

            // Items collection
            foreach (ListItem item in Lstbeatbox.Items)
            {
                if (item.Selected)
                {
                    beat += item.Value + ",";
                }
            }
            beat = beat.TrimEnd(',');

            // Items collection
            foreach (ListItem item in lstretailer.Items)
            {
                if (item.Selected)
                {
                    retailer += item.Value + ",";
                }
            }
            retailer = retailer.TrimEnd(',');

            //if (beat != "" && beat != "0")
            //{
            //    Qry = Qry + " and msp1.BeatId In (" + beat + ")";
            //}
            //else
            //{
            //    Qry = Qry + " and msp1.BeatId In (select AreaId from MastArea where AreaType='BEAT' and UnderId In ("+area+"))";
            //}

            if (beat != "" && beat != "0")
                Qry = Qry + " and msp1.BeatId in (" + beat + ")";

            if (retailer != "" && retailer != "0")
                Qry = Qry + " and msp1.PartyId in (" + retailer + ")";




            //         string message = "";
            //foreach (ListItem matGrp in listretailer.Items)
            //{
            //    if (matGrp.Selected)
            //    {
            //        retailer += matGrp.Value + ",";
            //    }
            //}
            //retailer = retailer.TrimStart(',').TrimEnd(',');
            //string sql = " select ROW_NUMBER()  OVER (ORDER BY   efficiency desc) As SrNo,* from (  select * ,'" + ddlretailer.SelectedItem.Text + "' as smname, orderdparty+FailedVisit+demo TotalVisitedParty , case when orderdparty+FailedVisit+demo > 0 then convert(varchar,convert(decimal(10,2) ,cast (ROUND(CAST(case when orderdparty+demo =0 then 1 else  orderdparty+demo end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit+demo =0 then 1 else  orderdparty+FailedVisit+demo end ) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2))  )) +'%'   else 'Never Visited' end  as Efficiencyinpercent,   case when orderdparty+FailedVisit+demo > 0 then cast (ROUND(CAST(case when orderdparty+demo =0 then 1 else  orderdparty+demo end AS DECIMAL(10,2))/CAST((case when orderdparty+FailedVisit+demo =0 then 1 else  orderdparty+FailedVisit+demo end) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2)) else 0 end as efficiency  from  ( select count(tro.ordid) as orderdparty  ,sum(Tro.OrderAmount) as OrderAmount , (select count(fvid) as failedvisit from TransFailedVisit  where  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and partyid=" + ddlretailer.SelectedItem.Value + ") FailedVisit , (select count(demoid) as demo from transdemo  where  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and partyid=" + ddlretailer.SelectedItem.Value + ") demo from TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId  where  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and tro.PartyId=" + ddlretailer.SelectedItem.Value + "  ) tbl group by orderdparty,OrderAmount,FailedVisit,demo  ) tbl2 order by efficiency desc";

            string sql = " select ROW_NUMBER()  OVER (ORDER BY   efficiency desc) As SrNo,* from (select * , OrderedParty+FailedVisit+demo TotalVisitedParty , case when OrderedParty+FailedVisit+demo > 0 then convert(varchar,convert(decimal(10,2) ,cast (ROUND(CAST(case when OrderedParty+demo =0 then 1 else  OrderedParty+demo end AS DECIMAL(10,2))/CAST((case when OrderedParty+FailedVisit+demo =0 then 1 else  OrderedParty+FailedVisit+demo end ) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2))  )) +'%'   else 'Never Visited' end  as Efficiencyinpercent,   case when OrderedParty+FailedVisit+demo > 0 then cast (ROUND(CAST(case when OrderedParty+demo =0 then 1 else  OrderedParty+demo end AS DECIMAL(10,2))/CAST((case when OrderedParty+FailedVisit+demo =0 then 1 else  OrderedParty+FailedVisit+demo end) AS DECIMAL(10,2))* 100, 2) As Decimal(10,2)) else 0 end as Efficiency  from  ( select (max(msp4.AreaName)+'-'+IsNull(Max(msp4.SyncId),'')) AS StateName,(max(mst1.areaName)+'-'+IsNull(Max(mst1.SyncId),'')) AS City,(Max(msp2.AreaName)+'-'+IsNull(Max(msp2.SyncId),'')) AS Area,(Max(msp3.AreaName)+'-'+IsNull(Max(msp3.SyncId),'')) AS Beat,Tro.PartyId,(Max(msp1.PartyName)+'-'+IsNull(Max(msp1.SyncId),'')) AS Outlet,count(tro.ordid) as OrderedParty,sum(Tro.OrderAmount) as OrderAmount , (select count(fvid) as failedvisit from TransFailedVisit trv where  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and Tro.partyid=trv.PartyId) FailedVisit , (select count(demoid) as Demo from transdemo  where  vdate  between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and Tro.partyid=transdemo.PartyId) as Demo from TransOrder Tro LEFT JOIN mastsalesrep msp on msp.smid=Tro.SMId LEFT JOIN MastParty msp1 on msp1.PartyId=Tro.PartyId LEFT JOIN MastArea msp2 on msp1.AreaId=msp2.AreaId LEFT JOIN MastArea msp3 on msp1.BeatId=msp3.AreaId left join MastArea ms on ms.AreaId=msp2.UnderId left join MastArea mst on mst.AreaId=ms.UnderId left join MastArea mst1 on mst1.AreaId=msp1.CityId left join MastArea msp4 on msp4.AreaId=mst.UnderId where  tro.vdate between '" + Convert.ToDateTime(txtfmDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd") + "' and tro.AreaId In (" + area + ") " + Qry + " group by Tro.PartyId ) tbl ) tbl2 order by Efficiency desc";

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                dataSet.Tables.Add(dt);

                try
                {
                    Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    string path = HttpContext.Current.Server.MapPath("ExportedFiles//");

                    if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = "Retailer Ranking.xlsx";

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
                        xlWorksheet.Name = "Retailer Ranking";

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
                    //xlWorkbook.Close();
                    //ExcelApp.Quit();
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
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' No Recod Found');", true);
                leavereportrpt.DataSource = null;
                leavereportrpt.DataBind();
            }
            spinner.Visible = false;
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

        protected void listretailer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BindArea(string SMIDStr)
        {
            try
            {
                string areaqry = @"select AreaId,AreaName from mastarea where AreaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Area' and Active=1 order by AreaName";
                DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, areaqry);
                if (dtArea.Rows.Count > 0)
                {
                    lstAreaBox.DataSource = dtArea;
                    lstAreaBox.DataTextField = "AreaName";
                    lstAreaBox.DataValueField = "AreaId";
                    lstAreaBox.DataBind();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void lstAreaBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string areastr = "";
                //         string message = "";
                foreach (ListItem areagrp in lstAreaBox.Items)
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
                        Lstbeatbox.DataSource = dtbeat;
                        Lstbeatbox.DataTextField = "AreaName";
                        Lstbeatbox.DataValueField = "AreaId";
                        Lstbeatbox.DataBind();
                    }
                }
                else
                {
                    Lstbeatbox.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Lstbeatbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strparty = string.Empty;
            foreach (ListItem party in Lstbeatbox.Items)
            {
                if (party.Selected)
                {
                    strparty += party.Value + ",";
                }
            }
            strparty = strparty.TrimStart(',').TrimEnd(',');
            if (strparty != "")
            {
                string str = @"select PartyId,(partyName+'-'+Mobile) AS partyName from Mastparty where Beatid in (" + strparty + ") and partydist=0 and active=1";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    lstretailer.DataSource = dt;
                    lstretailer.DataTextField = "partyName";
                    lstretailer.DataValueField = "PartyId";
                    lstretailer.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                lstretailer.Items.Clear();
            }
        }
    }
}