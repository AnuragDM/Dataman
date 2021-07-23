using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class TopSalesPerson : System.Web.UI.Page
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

            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {

                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;

                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", Qrychk = "", Qrychk1 = "";

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
                if (txt_noofrecords.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill no of records');", true);
                    TopRetailer.DataSource = null;
                    TopRetailer.DataBind();
                    return;
                }
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    TopRetailer.DataSource = new DataTable();
                    TopRetailer.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                Qrychk = " where d.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and d.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                Qrychk = Qrychk + " and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                Qrychk1 = " d.created_date>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and d.created_date<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                string query = "SELECT top " + txt_noofrecords.Text + "  cp.smid,Isnull(cp.smname,'') [SPerson], '' [SDesignation],sum([PrimaryOrder]) [PrimaryOrder], sum([RecOrdr]) [OrderAmount],sum([TotalNewParty]) [TotalNewParty], ";
                query = query + "sum([TotalCalls]) [TotalCalls],sum([ProdCalls]) [ProdCalls] FROM (SELECT d.smid,sum(tp.qty*rate) [PrimaryOrder],0 as [RecOrdr],0 [TotalNewParty], 0 [TotalCalls], 0 [ProdCalls] FROM transpurchorder d Left Join transpurchorder1 tp on d.PODocId=tp.podocid " + Qrychk + " GROUP BY d.smid ";


                query = query + " UNION ALL SELECT d.smid,0 as PrimaryOrder,sum(d.Amount) [RecOrdr],0 [TotalNewParty],0 [TotalCalls], 0 [ProdCalls] FROM Transorder1 d  " + Qrychk + " GROUP BY d.smid ";

                query = query + " UNION ALL SELECT ms.SMId,0 as PrimaryOrder, 0 [RecOrdr], count(d.PartyId) [TotalNewParty],0 [TotalCalls],0 [ProdCalls] FROM MastParty d ";
                query = query + " LEFT JOIN mastsalesrep ms ON d.Created_User_id=ms.UserId WHERE  " + Qrychk1 + "  and ms.smid in (" + smIDStr1 + ")  and d.PartyDist=0 Group By ms.SMId ";

                query = query + " UNION ALL SELECT mQ.smid,0 as PrimaryOrder, 0 [RecOrdr], 0 [TotalNewParty],sum([Count]) [CallsVisited],sum(proCount) [Retailer] from ( ";
                query = query + " SELECT iQ.smid,isnull(sum([Count]), 0) [Count],isnull(sum([proCount]), 0) [proCount] FROM ( SELECT d.SMId,count(d.OrdDocId) [Count],count(d.OrdDocId) [ProCount] FROM TransOrder d LEFT JOIN MastParty p ON d.PartyId = p.PartyId";
                query = query + " " + Qrychk + " GROUP BY d.SMId";

                query = query + " UNION ALL SELECT d.SMId,(select case when Count(*)>1 then 1 else 1 end FROM TransDemo om1 where om1.VDate=d.VDate  and om1.smid in (" + smIDStr1 + ") ";

                query = query + " and om1.PartyId=p.PartyId) as [Count],0 [ProCount] FROM TransDemo d LEFT JOIN MastParty p ON d.PartyId = p.PartyId";
                query = query + " " + Qrychk + " GROUP BY d.SMId,d.VDate,p.PartyId ";

                query = query + " union all  SELECT d.SMId,count(*) [Count],0 [ProCount] FROM TransFailedVisit d LEFT JOIN MastParty p ON d.PartyId = p.PartyId";
                query = query + " " + Qrychk + " GROUP BY d.SMId) iQ GROUP BY smid ) mQ GROUP BY smid  ";

                query = query + " ) a LEFT JOIN MastSalesRep cp ON a.smid = cp.SMId GROUP BY cp.smname,cp.smid ORDER BY OrderAmount DESC";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    TopRetailer.DataSource = dtItem;
                    TopRetailer.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    TopRetailer.DataSource = dtItem;
                    TopRetailer.DataBind();
                    btnExport.Visible = false;
                }

                dtItem.Dispose();
            }



            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                TopRetailer.DataSource = null;
                TopRetailer.DataBind();
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

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopSalesPerson.aspx");
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

                ViewState["tree"] = smIDStr12;

            }

            cnt = cnt + 1;
            return;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TopSalesPerson.csv");
            string headertext = "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"') + "," + "Total New Party".TrimStart('"').TrimEnd('"') + "," + "Total Calls".TrimStart('"').TrimEnd('"') + "," + "Pro Calls".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("SPerson", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OrderAmount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotalNewParty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotalCalls", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProdCalls", typeof(String)));

            foreach (RepeaterItem item in TopRetailer.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblSPerson = item.FindControl("lblSPerson") as Label;
                dr["SPerson"] = lblSPerson.Text.ToString();
                Label lblOrderAmount = item.FindControl("lblOrderAmount") as Label;
                dr["OrderAmount"] = lblOrderAmount.Text.ToString();
                Label lblTotalNewParty = item.FindControl("lblTotalNewParty") as Label;
                dr["TotalNewParty"] = lblTotalNewParty.Text.ToString();
                Label lblTotalCalls = item.FindControl("lblTotalCalls") as Label;
                dr["TotalCalls"] = lblTotalCalls.Text.ToString();
                Label lblProdCalls = item.FindControl("lblProdCalls") as Label;
                dr["ProdCalls"] = lblProdCalls.Text.ToString();

                dtParams.Rows.Add(dr);
            }


            for (int j = 0; j < dtParams.Rows.Count; j++)
            {
                for (int k = 0; k < dtParams.Columns.Count; k++)
                {
                    if (dtParams.Rows[j][k].ToString().Contains(","))
                    {
                        string h = dtParams.Rows[j][k].ToString();
                        string d = h.Replace(",", " ");
                        dtParams.Rows[j][k] = "";
                        dtParams.Rows[j][k] = d;
                        dtParams.AcceptChanges();
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 0)
                        {
                            //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
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
            Response.AddHeader("content-disposition", "attachment;filename=TopSalesPerson.csv");
            Response.Write(sb.ToString());

            sb.Clear();
            dtParams.Dispose();
            Response.End();

        }

        protected void TopRetailer_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField hdnsmid = (HiddenField)e.Item.FindControl("hdnsmid");
            if (e.CommandName == "PrimaryOrder")
            {
                //ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                //ViewState["Todate"] = Settings.dateformat(txttodate.Text);


                Response.Redirect("OrderAmountDetails.aspx?smid=" + hdnsmid.Value + " &FromDate=" + txtfmDate.Text + "&ToDate=" + txttodate.Text + "&ClickOn=PrimaryOrder");

            }
            if (e.CommandName == "SecondaryOrder")
            {
                //ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                //ViewState["Todate"] = Settings.dateformat(txttodate.Text);


                Response.Redirect("OrderAmountDetails.aspx?smid=" + hdnsmid.Value + " &FromDate=" + txtfmDate.Text + "&ToDate=" + txttodate.Text + "&ClickOn=SecondaryOrder");

            }

            if (e.CommandName == "Partyselect")
            {
                //ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                //ViewState["Todate"] = Settings.dateformat(txttodate.Text);


                Response.Redirect("TotalNewPartyDetails.aspx?smid=" + hdnsmid.Value + " &FromDate=" + txtfmDate.Text + "&ToDate=" + txttodate.Text + "");

            }

            if (e.CommandName == "TotalCallsselect")
            {
                //ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                //ViewState["Todate"] = Settings.dateformat(txttodate.Text);


                Response.Redirect("TotalCallsDetails.aspx?smid=" + hdnsmid.Value + " &FromDate=" + txtfmDate.Text + "&ToDate=" + txttodate.Text + "");

            }

            if (e.CommandName == "ProdCallsselect")
            {
                //ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                //ViewState["Todate"] = Settings.dateformat(txttodate.Text);


                Response.Redirect("ProdCallsDetails.aspx?smid=" + hdnsmid.Value + " &FromDate=" + txtfmDate.Text + "&ToDate=" + txttodate.Text + "");
            }
        }
    }
}
