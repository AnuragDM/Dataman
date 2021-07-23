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
    public partial class CollectionReport : System.Web.UI.Page
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

                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Visible = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", Qrychk = ""; string query = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }

            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
            {
                TopRetailer.DataSource = new DataTable();
                TopRetailer.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }
            Qrychk = "  and dc.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and dc.VDate<='" + Settings.dateformat(txttodate.Text) + "'";
            if (smIDStr1 != "")
            {
                Qrychk = Qrychk + " and dc.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
            }
            if (ddlCollectiontype.SelectedValue == "0")
            {
                query = @"Select dc.VDate,ms.smname,mp.partyName,mp.SyncId,mp.Mobile,dc.Mode,dc.Amount From DistributerCollection dc Left Join MastParty mp on dc.DistId=Mp.PartyId 
                         Left Join MastSalesRep ms on dc.smid=ms.smid where mp.partyDist=1 " + Qrychk + " order by dc.VDate desc";
            }
            else
            {
                query = @"Select * from (Select dc.VDate,ms.smname,mp.partyName,mp.SyncId,mp.Mobile,dc.Mode,dc.Amount From Temp_Transcollection dc Left Join MastParty mp on dc.PartyId=Mp.PartyId 
                        Left Join MastSalesRep ms on dc.smid=ms.smid where mp.partyDist=0 " + Qrychk + " " +
                       " UNION ALL Select dc.VDate,ms.smname,mp.partyName,mp.SyncId,mp.Mobile,dc.Mode,dc.Amount From Transcollection dc Left Join MastParty mp on dc.PartyId=Mp.PartyId " +
                       " Left Join MastSalesRep ms on dc.smid=ms.smid where mp.partyDist=0 " + Qrychk + " ) a order by a.VDATE desc";
            }

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                rptmain.Style.Add("display", "block");
                TopRetailer.DataSource = dt;
                TopRetailer.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                rptmain.Style.Add("display", "block");
                TopRetailer.DataSource = dt;
                TopRetailer.DataBind();
                btnExport.Visible = false;
            }
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

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CollectionReport.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CollectionReport.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=CollectionReport.csv");
            string headerprevious = "Collection Type: " + ddlCollectiontype.SelectedItem.Text + "" + "," + "FromDate:" + txtfmDate.Text + "" + "," + "ToDate:" + txttodate.Text + "";

            string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Retailer/Distributor".TrimStart('"').TrimEnd('"') + "," + "Sync Id".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Payment Mode".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headerprevious);
            sb.AppendLine(System.Environment.NewLine);

            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);

            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Retailer/Distributor", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SyncId", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PaymentMode", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));

            foreach (RepeaterItem item in TopRetailer.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblVDate = item.FindControl("lblVDate") as Label;
                dr["Date"] = lblVDate.Text.ToString();
                Label lblsmname = item.FindControl("lblsmname") as Label;
                dr["SalesPerson"] = lblsmname.Text.ToString();
                Label lblpartyName = item.FindControl("lblpartyName") as Label;
                dr["Retailer/Distributor"] = lblpartyName.Text.ToString();
                Label lblSyncId = item.FindControl("lblSyncId") as Label;
                dr["SyncId"] = lblSyncId.Text.ToString();
                Label lblMobile = item.FindControl("lblMobile") as Label;
                dr["Mobile"] = lblMobile.Text.ToString();
                Label lblMode = item.FindControl("lblMode") as Label;
                dr["PaymentMode"] = lblMode.Text.ToString();
                Label lblAmount = item.FindControl("lblAmount") as Label;
                dr["Amount"] = lblAmount.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
            dv.Sort = "Date desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[7];
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
                                if (k == 6)
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
                                if (k == 6)
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
                                if (k == 6)
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
                for (int x = 6; x < totalVal.Count(); x++)
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
                sb.Append(",,,,,Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

                sb.Clear();
                dtParams.Dispose();
                dv.Dispose();
                udtNew.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

    }
}