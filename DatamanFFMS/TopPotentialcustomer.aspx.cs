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
    public partial class TopPotentialcustomer : System.Web.UI.Page
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

            foreach (ListItem Beat in Lstbeatbox.Items)
            {
                if (Beat.Selected)
                {
                    matBeatNew += Beat.Value + ",";
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

                if (txt_noofrecords.Text == "")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill no of records');", true);
                    rptTopPotentials.DataSource = null;
                    rptTopPotentials.DataBind();
                    return;

                }
                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    rptTopPotentials.DataSource = new DataTable();
                    rptTopPotentials.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                Qrychk = " where os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                if (smIDStr1 != "")
                {
                    Qrychk = Qrychk + " and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";
                }
                string query = "select top " + txt_noofrecords.Text + " CustName,max(area) as area,max(Beat) as Beat,max(City) as City,max(State) as State,max(SalePer) as SalePer,Potential,sum(TotSale) [TotSale],sum(a.Potential-a.TotSale) [Diff]  from ( select CustName,max(SalePer) as SalePer,sum(b.Potential) as Potential,";
                query = query + " sum(b.TotSale) [TotSale],sum(b.Diff) as  [Diff],max(City) as City,max(State) as State,max(area) as area,max(Beat) as Beat from ( select os.PartyId,p.PartyName AS CustName,'' AS SalePer,0 As Potential,sum(ISNULL(os.Qty*os.Rate,0)) AS TotSale,isnull(p.Potential,0)-(SUM(isnull(os.Qty*os.Rate ,0))) AS Diff,";
                query = query + " '' AS City,'' AS State,'' as area,'' as Beat FROM TransOrder1 os left join MastParty p on p.PartyId =os.PartyId " + Qrychk + " " + beat_Filter + " Group by p.PartyName,p.Potential,os.PartyId union all ";
                query = query + " select p.PartyId,p.PartyName AS CustName,cp.SMName AS SalePer,p.Potential As Potential,0 AS TotSale,0 AS Diff,vg.cityName AS City,vg.stateName AS State,vg.areaName AS area,b.AreaName as Beat ";
                query = query + " FROM MastParty p left JOIN mastlink ua on ua.LinkCode=p.AreaId INNER JOIN mastsalesrep cp ON cp.SMId=ua.PrimCode INNER JOIN mastarea b ON b.AreaId=p.BeatId LEFT JOIN viewgeo vg ON vg.beatid=b.AreaId ";
                query = query + " Where ua.PrimCode in ( " + smIDStr1 + " ) " + beat_Filter + "  )b Group by b.CustName ) a group by a.CustName,a.Potential order by a.Potential desc,a.CustName ";

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                if (dtItem.Rows.Count > 0)
                {
                    rptmain.Style.Add("display", "block");
                    rptTopPotentials.DataSource = dtItem;
                    rptTopPotentials.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rptmain.Style.Add("display", "block");
                    rptTopPotentials.DataSource = dtItem;
                    rptTopPotentials.DataBind();
                    btnExport.Visible = false;
                }

                dtItem.Dispose();
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                rptTopPotentials.DataSource = null;
                rptTopPotentials.DataBind();
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
                    rptTopPotentials.DataSource = null;
                    rptTopPotentials.DataBind();
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
            Response.AddHeader("content-disposition", "attachment;filename=TopPotential.csv");
            string headertext = "Customer".TrimStart('"').TrimEnd('"') + "," + "State".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Potential".TrimStart('"').TrimEnd('"') + "," + "Total Sales".TrimStart('"').TrimEnd('"') + "," + "Diff".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("CustName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("State", typeof(String)));
            dtParams.Columns.Add(new DataColumn("City", typeof(String)));
            dtParams.Columns.Add(new DataColumn("area", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Beat", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SalePer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Potential", typeof(String)));
            dtParams.Columns.Add(new DataColumn("TotSale", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Diff", typeof(String)));

            foreach (RepeaterItem item in rptTopPotentials.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblCustName = item.FindControl("lblCustName") as Label;
                dr["CustName"] = lblCustName.Text.ToString();
                Label lblState = item.FindControl("lblState") as Label;
                dr["State"] = lblState.Text.ToString();
                Label lblCity = item.FindControl("lblCity") as Label;
                dr["City"] = lblCity.Text.ToString();
                Label lblarea = item.FindControl("lblarea") as Label;
                dr["area"] = lblarea.Text.ToString();
                Label lblBeat = item.FindControl("lblBeat") as Label;
                dr["Beat"] = lblBeat.Text.ToString();
                Label lblSalePer = item.FindControl("lblSalePer") as Label;
                dr["SalePer"] = lblSalePer.Text.ToString();
                Label lblPotential = item.FindControl("lblPotential") as Label;
                dr["Potential"] = lblPotential.Text.ToString();
                Label lblTotSale = item.FindControl("lblTotSale") as Label;
                dr["TotSale"] = lblTotSale.Text.ToString();
                Label lblDiff = item.FindControl("lblDiff") as Label;
                dr["Diff"] = lblDiff.Text.ToString();

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
            Response.AddHeader("content-disposition", "attachment;filename=TopPotential.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
        }
    }
}
