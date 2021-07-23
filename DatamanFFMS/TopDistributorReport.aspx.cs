using BusinessLayer;
using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class TopDistributorReport : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopDistributorReport.aspx");
        }
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
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();             
                btnExport.Visible = true;
                btnExport1.Visible = false;
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }

        }
        public class Products
        {
            public string ProductGroup { get; set; }
            public string Product { get; set; }
            public string Qty { get; set; }
            public string Amount { get; set; }
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

        private void ClearControls()
        {
            try
            {
                TopDistributor.DataSource = null;
                TopDistributor.DataBind();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", distIdStr1 = "", Qrychk = "";
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            ViewState["smIDStr1"] = smIDStr1;
            if (smIDStr1 != "")
            {

                if (txt_noofrecords.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill no of records');", true);
                    TopDistributor.DataSource = null;
                    TopDistributor.DataBind();
                    return;

                }
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    TopDistributor.DataSource = new DataTable();
                    TopDistributor.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                Qrychk = " and om.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and om.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                if (smIDStr1 != "")
                {
                    Qrychk = Qrychk + " and om.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                }

                ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                ViewState["Todate"] = Settings.dateformat(txttodate.Text);
                string query = "Select  top " + txt_noofrecords.Text + " visit, distributor_id as Distributor_Id ,[DistName] as DName ,[areaid], max([DArea]) [DArea],max(DCity) [DCity] ,sum([DSale]) [DSecSale] , sum([primsale]) [Dprimsale]  From ( ";
                //query = query + " select distinct da.PartyId as Distributor_Id,da.PartyName as [DistName],da.areaid,a.AreaName as DArea, ma.AreaName as DCity,convert(decimal, sum(om.Qty * om.Rate)) [DSale],0 AS primsale,dbo.getVisitCount(da.PartyId,'" + Settings.dateformat(txtfmDate.Text) + "','" + Settings.dateformat(txttodate.Text) + "') as visit from TransOrder1 om left join MastParty da on da.AreaId=om.AreaId left join MastArea a on a.AreaId=da.AreaId ";
                query = query + " select distinct da.PartyId as Distributor_Id,da.PartyName as [DistName],da.areaid,a.AreaName as DArea, ma.AreaName as DCity,convert(decimal, sum(om.Qty * om.Rate)) [DSale],0 AS primsale,dbo.getVisitCount(da.PartyId,'" + Settings.dateformat(txtfmDate.Text) + "','" + Settings.dateformat(txttodate.Text) + "') as visit from TransOrder1 om left join MastParty da on da.PartyId=om.DistId left join MastArea a on a.AreaId=da.AreaId ";
                query = query + " LEFT JOIN mastarea ma ON ma.AreaId=a.UnderId WHERE da.PartyDist=1 " + Qrychk + " group by da.PartyId,da.areaid,da.PartyName,a.AreaName,ma.AreaName";
                query = query + " union all select DISTINCT ts1.DistId as Distributor_Id,d.PartyName as [DistName],d.areaid,'' AS DArea, '' AS DCity,0 AS [DSale],SUM(ts1.Amount) AS primsale,0 as visit  FROM TransDistInv1 ts1 left join MastParty d on d.PartyId=ts1.DistId ";
                query = query + " WHERE d.PartyDist=1 AND ts1.VDate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' and ts1.DistId in (select da.PartyId from MastParty da where da.AreaId in (select distinct ua.LinkCode from MastLink ua where ua.LinkCode in (select  distinct ua.LinkCode from MastLink ua  where ua.PrimCode in  (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1))) ) ";
                query = query + " group by ts1.DistId ,d.areaid,d.PartyName) main group by distributor_id,[DistName],[areaid],visit order by DSecSale desc,DName";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    TopDistributor.DataSource = dtItem;
                    TopDistributor.DataBind();
                    btnExport.Visible = true;
                    btnExport1.Visible = false;
                    Repeater3.DataSource = null;
                    Repeater3.DataBind();
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    TopDistributor.DataSource = dtItem;
                    TopDistributor.DataBind();
                    btnExport.Visible = false;
                }
                dtItem.Dispose();
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                TopDistributor.DataSource = null;
                TopDistributor.DataBind();
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=Topdistributor.csv");
            string headertext = "Distributor".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "Sales Person Wise".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"') + "," + "Total Primary Sales".TrimStart('"').TrimEnd('"') + "," + "Total Sec.Sales".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("DistName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DCity", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DArea", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Dprimsale", typeof(String)));
            dtParams.Columns.Add(new DataColumn("DSecSale", typeof(String)));

            foreach (RepeaterItem item in TopDistributor.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblDistName = item.FindControl("lblDistName") as Label;
                dr["DistName"] = lblDistName.Text.ToString();
                Label lblDCity = item.FindControl("lblDCity") as Label;
                dr["DCity"] = lblDCity.Text.ToString();
                Label lblissuecoupan = item.FindControl("lblissuecoupan") as Label;
                dr["DName"] = lblissuecoupan.Text.ToString();
                Label lblDArea = item.FindControl("lblDArea") as Label;
                dr["DArea"] = lblDArea.Text.ToString();
                Label lblDprimsale = item.FindControl("lblDprimsale") as Label;
                dr["Dprimsale"] = lblDprimsale.Text.ToString();
                Label lblDSecSale = item.FindControl("lblDSecSale") as Label;
                dr["DSecSale"] = lblDSecSale.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
            dv.Sort = "DistName desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[6];
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

                sb.Clear();
                udtNew.Dispose();
                dtParams.Dispose();
                dv.Dispose();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }


        protected void btnExportDetail_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TopdistributorDetails.csv");
            string headertext = "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Visit Date".TrimStart('"').TrimEnd('"');
            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
            dtParams.Columns.Add(new DataColumn("VDate", typeof(String)));

            foreach (RepeaterItem item in Repeater3.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblsmname = item.FindControl("lblsmname") as Label;
                dr["smname"] = lblsmname.Text.ToString();
                Label lblmobile = item.FindControl("lblmobile") as Label;
                dr["Mobile"] = lblmobile.Text.ToString();
                Label lblvdate = item.FindControl("lblvdate") as Label;
                dr["VDate"] = lblvdate.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
            dv.Sort = "VDate asc";
            dtParams = dv.ToTable();
            //  DataTable udtNew = dv.ToTable();         
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

                Response.Write(sb.ToString());
                Response.End();

                sb.Clear();
                //udtNew.Dispose();
                dtParams.Dispose();
                dv.Dispose();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopDistributorDetail.aspx");
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

        protected void Topsaleproduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {

                HiddenField hdnAreaId = (HiddenField)e.Item.FindControl("hdnAreaId");
                HiddenField hdnDistributorId = (HiddenField)e.Item.FindControl("hdnDistributorId");
                //  string gh = ViewState["viewsmidstr"].ToString();
                Response.Redirect("TopDistributorDetail.aspx?AreaId=" + hdnAreaId.Value + "&Distributor_Id=" + hdnDistributorId.Value + " &FromDate=" + ViewState["Fromdate"] + "&ToDate=" + ViewState["Todate"] + "&smidstr='" + ViewState["smIDStr1"].ToString() + "'");

            }

            if (e.CommandName == "smidcount")
            {
                string Qrychk = "";

                Qrychk = " and VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                ViewState["Fromdate"] = Settings.dateformat(txtfmDate.Text);
                ViewState["Todate"] = Settings.dateformat(txttodate.Text);
                string str = @"SELECT ms.SMName,ms.Mobile,VDate FROM TransVisitDist om LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE DistId=" + e.CommandArgument.ToString() + " " + Qrychk + " UNION ALL SELECT ms.SMName,ms.Mobile,VDate FROM TransFailedVisit om LEFT JOIN mastparty mp ON om.PartyId=mp.PartyId  LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE mp.PartyId=" + e.CommandArgument.ToString() + " " + Qrychk + " UNION ALL SELECT ms.SMName,ms.Mobile,VDate FROM TransPurchOrder om LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE DistId=" + e.CommandArgument.ToString() + " " + Qrychk + "";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    Repeater3.DataSource = dt;
                    Repeater3.DataBind();
                    btnExport1.Visible = true;
                }

                dt.Dispose();
            }
        }
    }
}