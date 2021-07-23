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
    public partial class VisitedDistributor : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            string DashBoardDate = Request.QueryString["Date"];
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
                BindTreeViewControl();
                string pageName = Path.GetFileName(Request.Path);
               // btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
                this.checkNode();
                if (!string.IsNullOrEmpty(DashBoardDate))
                {
                    //  orderType = Request.QueryString["type"];
                    txtfmDate.Text = Convert.ToDateTime(DashBoardDate).ToString("dd/MMM/yyyy");
                    txttodate.Text = Convert.ToDateTime(DashBoardDate).ToString("dd/MMM/yyyy");
                    btnGo_Click(null, null);
                }
            }
        }
        private TreeNode FindRootNode(TreeNode treeNode)
        {
            while (treeNode.Parent != null)
            {
                treeNode = treeNode.Parent;
            }
            return treeNode;
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
        protected void checkNode()
        {
            //foreach (TreeNode tr in trview.)
            //{


            //}
            foreach (var node in Collect(trview.Nodes))
            {
                node.Checked = true;
                // you will see every child node here
            }
        }
        IEnumerable<TreeNode> Collect(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (var child in Collect(node.ChildNodes))
                    yield return child;
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", Qrychk = "", matBeatNew = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

          //  foreach (ListItem Beat in Lstbeatbox.Items)
            {
              //  if (Beat.Selected)
                {
                 //   matBeatNew += Beat.Value + ",";
                }
            }
            matBeatNew = matBeatNew.TrimStart(',').TrimEnd(',');
            string beat_Filter = "";
            if (matBeatNew != "" && matBeatNew != "0")
                beat_Filter = " and p.BeatId in (" + matBeatNew + ") ";
            else
                beat_Filter = "";

            if (smIDStr1 != "")
            {

                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    rptnewrtl.DataSource = new DataTable();
                    rptnewrtl.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }
                string Qty2 = "";
                string Qty1 = "";
                //string Qty2 = " and om.VDate BETWEEN '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";
                Qty2 = Qty2 + " and om.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                Qrychk = " and P.Created_Date between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

                Qty1 = Qty1 + " and p.Created_User_id in ( SELECT userid FROM MastSalesRep ms LEFT JOIN mastlogin ml ON ms.UserId=ml.Id WHERE ms.SMId IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                string query = @"SELECT a.smid,a.vdate,a.Visid,a.DistId,a.CityId,a.SMName,a.PartyName,a.Address1,a.Mobile FROM (
SELECT om.SMID,om.VDate,0 as VisId,om.DistId,P.CityId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransPurchOrder om LEFT JOIN mastparty p ON p.PartyId=om.DistId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE p.PartyDist=1 UNION ALL
SELECT om.SMID,om.VDate,om.VisId,om.DistId,P.CityId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransVisitDist om LEFT JOIN mastparty p ON p.PartyId=om.DistId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE p.PartyDist=1 and om.Type IS NULL UNION ALL SELECT om.SMID,om.VDate,om.VisId,om.DistId,P.CityId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM Temp_TransVisitDist om LEFT JOIN mastparty p ON om.DistId=p.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE p.PartyDist=1 and om.Type IS NULL UNION ALL SELECT om.SMID,om.VDate,om.VisId,om.PartyId AS DistId,P.CityId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransFailedVisit om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE p.PartyDist=1 UNION ALL SELECT om.SMID,om.VDate,om.VisId,om.PartyId AS DistId,P.CityId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM Temp_TransFailedVisit om LEFT JOIN mastparty p ON om.PartyId=p.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId WHERE p.PartyDist=1)a LEFT JOIN mastsalesrep ms ON a.smid=ms.SMId WHERE a.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59' AND a.smid IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

//                string query = @"select a.PartyId,a.Name,a.Address,a.Mobile,max (a.beat) as beat,max (a.[By]) as [By],sum(a.Business) as Business,sum(a.Pendency) as Pendency,max(a.Date) as Date from (
//                  select distinct p.PartyId, p.PartyName as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,max(b.AreaName) AS beat,max(ms.SMName) AS [By],0 AS Business,0 AS Pendency,max(p.created_date) as Date 
//                  FROM MastParty p left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id 
//                  WHERE p.partydist=0 " + Qrychk + " " + Qty1 + " " + beat_Filter + " ) group by p.PartyName,p.PartyId " +
//              " union all select distinct p.PartyId,p.PartyName as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,max(b.AreaName) AS beat,max(ms.SMName) AS [By],sum(isnull(om.OrderAmount,0)) AS Business,sum(isnull(om.OrderAmount,0)) AS Pendency,max(p.Created_Date) as Date from TransOrder om inner join MastParty p on p.PartyId=om.PartyId left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id WHERE p.PartyDist=0 " + Qrychk + " " + Qty2 + " " + beat_Filter + " group by p.PartyId,p.PartyName) a Group by a.PartyId,a.Name,a.Address,a.Mobile Order by Business desc,MAX( a.Date) DESC,name";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    rptnewrtl.DataSource = dtItem;
                    rptnewrtl.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    rptnewrtl.DataSource = dtItem;
                    rptnewrtl.DataBind();
                    btnExport.Visible = false;
                }
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                rptnewrtl.DataSource = null;
                rptnewrtl.DataBind();
            }
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
                     //   Lstbeatbox.DataSource = dtBeat;
                      //  Lstbeatbox.DataTextField = "AreaName";
                      //  Lstbeatbox.DataValueField = "AreaId";
                      //  Lstbeatbox.DataBind();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    rptnewrtl.DataSource = null;
                    rptnewrtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TopPotentialcustomer.aspx");
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
                  //  Lstbeatbox.Items.Clear();
                  //  Lstbeatbox.DataBind();

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
            Response.AddHeader("content-disposition", "attachment;filename=VisitedDistributor.csv");
            string headertext = "Retailer".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Visited Sales Person".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);


            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Address1", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SMName", typeof(String)));
           

            foreach (RepeaterItem item in rptnewrtl.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblName = item.FindControl("lblName") as Label;
                dr["PartyName"] = lblName.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblAddress = item.FindControl("lblAddress") as Label;
                dr["Address1"] = lblAddress.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblMobile = item.FindControl("lblMobile") as Label;
                dr["Mobile"] = lblMobile.Text.ToString();
                Label lblvisitedperson = item.FindControl("lblvisitedperson") as Label;
                dr["SMName"] = lblvisitedperson.Text.ToString();
                
                
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
            Response.AddHeader("content-disposition", "attachment;filename=VisitedDistributor.csv");
            Response.Write(sb.ToString());
            Response.End();

        }
    }
}

