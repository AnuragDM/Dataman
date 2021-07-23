using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace AstralFFMS
{
    public partial class rptBuisnessReport : System.Web.UI.Page
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
                //Added By - Nishu 06/12/2015 
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy"); // System.DateTime.Now.ToShortDateString();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");// System.DateTime.Now.ToShortDateString();
                //End
                btnExport.Visible = true;
                BindTreeViewControl();
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
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

            Qrychk = "(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

            if (smIDStr1 != "")
            {

                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    TopRetailer.DataSource = new DataTable();
                    TopRetailer.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                string query = "SELECT cp.SmName [EmpName], sum([Amt]) [Amt], sum([NewParties]) [NewParties], sum([TotCallR]) [TotCallR],SUM([TotCallE])[TotCallE], sum([ProdCallsR]) [ProdCallsR],sum([ProdCallsE]) [ProdCallsE] FROM (" +
                              "select om.smid,sum(Amount) as Amt,0 [NewParties], 0 [TotCallR],0 [ProdCallsR],0 [TotCallE], 0 [ProdCallsE] from Transorder1 om where smid in " + Qrychk + " and vDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' group by om.smid " +
                              " UNION ALL select ms.SMId,0 as Amt,count(p.partyid) as [NewParties], 0 [TotCallR], 0 [ProdCallsR],0 [TotCallE], 0 [ProdCallsE] from MastParty p LEFT JOIN mastlogin ml ON p.Created_User_id=ml.Id LEFT JOIN mastsalesrep ms ON ms.UserId=ml.Id where p.Created_Date  BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59'and p.Created_User_Id in (SELECT ml.Id FROM MastSalesRep ms LEFT JOIN mastlogin ml ON ms.UserId=ml.Id WHERE ms.SMId IN " + Qrychk + ") group by ms.SMId UNION ALL  " +
                              " SELECT fv.smid [ConPer_Id],0 [Amt],0 [NewParties], count(fv.FvId) [TotCallR], 0 [ProdCallsR],0 [TotCallE],0 [ProdCallsE] FROM TransFailedVisit fv inner JOIN MastParty p ON fv.PartyId = p.PartyId inner JOIN MastArea on p.AreaId=MastArea.AreaId where fv.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59'and fv.smid in " + Qrychk + " GROUP BY fv.smid UNION ALL " +
                              " SELECT d.smid  [ConPer_Id],0 [Amt],0 [NewParties],count(d.DemoId) [TotCallR], 0 [ProdCallsR],0 [TotCallE],0 [ProdCallsE] FROM TransDemo d LEFT JOIN MastParty p ON d.PartyId = p.PartyId left JOIN MastArea on p.AreaId=MastArea.AreaId where d.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' and d.smid in " + Qrychk + " GROUP BY d.smid union all " +
                              " SELECT om.smid, 0 [Amt], 0 [NewParties],case when (count(om.OrdDocId))>1 then 1 else 1 end [TotCallR],0 [ProdCallsR],0 [TotCallE],0 [ProdCallsE] from((Transorder om LEFT JOIN mastparty p ON om.partyid = p.partyid))left JOIN mastArea on p.AreaId=mastArea.AreaId where om.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' and om.smid in " + Qrychk + " GROUP BY om.smid,om.areaid,mastArea.AreaName,om.partyid,om.VDate UNION ALL " +
                              " SELECT om.smid, 0 [Amt], 0 [NewParties],0 [TotCallR],case when (count(om.OrdDocId))>1 then 1 else 1 end [ProdCallsR],0 [TotCallE],0 [ProdCallsE] from((Transorder om LEFT JOIN MastParty p ON om.PartyId = p.PartyId))left JOIN MastArea on p.AreaId=MastArea.AreaId where om.VDate BETWEEN '" + txtfmDate.Text + " 00:00' AND '" + txttodate.Text + " 23:59' and om.smid in " + Qrychk + " GROUP BY om.smid,om.AreaId,MastArea.AreaName,om.partyid,om.VDate ) t " +
                              " INNER JOIN Mastsalesrep cp ON t.smid = cp.smid GROUP BY cp.smname ORDER BY [Amt] DESC, [EmpName]";

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
                ViewState["tree"] = smiMStr;
            }

            cnt = cnt + 1;
            return;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BuisnessReport.csv");
            string headertext = "Employee name".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"') + "," + "New Parties".TrimStart('"').TrimEnd('"') + "," + "Total Call".TrimStart('"').TrimEnd('"') + "," + "Productive Call".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);


            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("EmpName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amt", typeof(String)));
            dtParams.Columns.Add(new DataColumn("NewParties", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotCallR", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ProdCallsR", typeof(string)));


            foreach (RepeaterItem item in TopRetailer.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblEmpName = item.FindControl("lblEmpName") as Label;
                dr["EmpName"] = lblEmpName.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblAmt = item.FindControl("lblAmt") as Label;
                dr["Amt"] = lblAmt.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblNewParties = item.FindControl("lblNewParties") as Label;
                dr["NewParties"] = lblNewParties.Text.ToString();
                Label lblTotCallR = item.FindControl("lblTotCallR") as Label;
                dr["TotCallR"] = lblTotCallR.Text.ToString();
                Label lblProdCallsR = item.FindControl("lblProdCallsR") as Label;
                dr["ProdCallsR"] = lblProdCallsR.Text.ToString();
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
            Response.AddHeader("content-disposition", "attachment;filename=BuisnessReport.csv");
            Response.Write(sb.ToString());

            sb.Clear();
            dtParams.Dispose();
            Response.End();
        }
    }
}

