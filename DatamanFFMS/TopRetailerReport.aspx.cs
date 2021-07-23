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
    public partial class TopRetailerReport : System.Web.UI.Page
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
                //fill_TreeArea();
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

            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "", Querybeat = "", matBeatNew = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }

            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            ViewState["smIDStr1"] = smIDStr1;
            foreach (ListItem Beat in Lstbeatbox.Items)
            {
                if (Beat.Selected)
                {
                    matBeatNew += Beat.Value + ",";
                }
            }
            matBeatNew = matBeatNew.TrimStart(',').TrimEnd(',');

            if (smIDStr1 == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                TopRetailer.DataSource = null;
                TopRetailer.DataBind();
                return;
            }
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
            Qrychk = "  where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
            if (smIDStr1 != "")
            {
                Qrychk = Qrychk + " and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
            }
            if (matBeatNew != "")
            {
                Querybeat = "AND ma.AreaId in (" + matBeatNew + ")";
            }
            ViewState["Fromdate"] = txtfmDate.Text;
            ViewState["Todate"] = txttodate.Text;
            //ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
            //ViewState["Todate"] = Settings.dateformat(txttodate.Text);
            string query = "SELECT TOP " + txt_noofrecords.Text + " mp.partyid, mp.PartyName,Max(mp.Address1) As Address1,sum(isnull(os.Qty,0)*isnull(os.Rate,0)) AS Amount,Max(ms.SMName) As SalesPerson,Max(ma.AreaName) AS Beat,ma.AreaId,Max(pt.PartyTypeName) As PartyTypeName ";
            query = query + "FROM MastParty mp LEFT JOIN TransOrder1 os ON mp.PartyId=os.PartyId ";
            query = query + "Inner JOIN mastsalesrep ms ON ms.SMId=os.SMId LEFT JOIN PartyType pt ON pt.PartyTypeId=mp.PartyType ";
            query = query + "LEFT JOIN Mastarea ma ON ma.AreaId=mp.BeatId " + Qrychk + " " + Querybeat + " GROUP BY mp.partyid, mp.PartyName,ma.AreaId ORDER BY Amount desc ";

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

        private void BindbeatDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string cityQry = @"  select AreaId,AreaName from mastarea where underId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Beat' and Active=1 order by AreaName";
                    DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                    if (dtBeat.Rows.Count > 0)
                    {
                        Lstbeatbox.DataSource = dtBeat;
                        Lstbeatbox.DataTextField = "AreaName";
                        Lstbeatbox.DataValueField = "AreaId";
                        Lstbeatbox.DataBind();
                    }
                    dtBeat.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    TopRetailer.DataSource = null;
                    TopRetailer.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopRetailerReport.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopRetailerDetail.aspx");
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
                    BindbeatDDl(smIDStr12);
                }
                else
                {
                    Lstbeatbox.Items.Clear();
                    Lstbeatbox.DataBind();

                }
                ViewState["tree"] = smiMStr;

            }
            cnt = cnt + 1;
            return;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TopRetailer.csv");
            string headertext = "PartyName".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Type".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Address1", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PartyTypeName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Beat", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));

            foreach (RepeaterItem item in TopRetailer.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblPartyName = item.FindControl("lblPartyName") as Label;
                dr["PartyName"] = lblPartyName.Text.ToString();
                Label lblAddress1 = item.FindControl("lblAddress1") as Label;
                dr["Address1"] = lblAddress1.Text.ToString();
                Label lblPartyTypeName = item.FindControl("lblPartyTypeName") as Label;
                dr["PartyTypeName"] = lblPartyTypeName.Text.ToString();
                Label lblBeat = item.FindControl("lblBeat") as Label;
                dr["Beat"] = lblBeat.Text.ToString();
                Label lblAmount = item.FindControl("lblAmount") as Label;
                dr["Amount"] = lblAmount.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
            dv.Sort = "PartyName desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[5];
            try
            {

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
                                if (k == 4 || k == 5)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
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
                                if (k == 4 || k == 5)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
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
                                //Total For Columns
                                if (k == 4 || k == 5)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                                //End
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                string totalStr = string.Empty;
                for (int x = 4; x < totalVal.Count(); x++)
                {
                    if (totalVal[x] == 0)
                    {
                        totalStr += "0" + ',';
                    }
                    else
                    {
                        totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                    }
                }
                sb.Append(",,,Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

                dtParams.Dispose();
                udtNew.Dispose();
                dv.Dispose();
                sb.Clear();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void Topsaleproduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                HiddenField hdnitemid = (HiddenField)e.Item.FindControl("hdnitemid");
                //  string gh = ViewState["viewsmidstr"].ToString();
                Response.Redirect("TopRetailerDetail.aspx?PartyId=" + hdnitemid.Value + "&FromDate=" + ViewState["Fromdate"] + "&ToDate=" + ViewState["Todate"] + "&smidstr='" + ViewState["smIDStr1"].ToString() + "'");
            }
        }
    }
}
