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
using System.Globalization;


namespace AstralFFMS
{
    public partial class NotVisitedOutletReport : System.Web.UI.Page
    {
        string roleType = "";
        int cnt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            trview.Attributes.Add("onclick", "fireCheckChanged(event)");
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                btnExport.Visible = true;
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

            string smIDStr = "", Query = "", smIDStr1 = "", Qrychk = "", matAreanew = "", matBeatNew = "", strparty = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            if (txt_noofrecords.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please fill no of records');", true);
                rptRETNVIST.DataSource = null;
                rptRETNVIST.DataBind();
                return;

            }
            if (smIDStr1 == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select Sales Person');", true);
                rptRETNVIST.DataSource = null;
                rptRETNVIST.DataBind();
                return;
            }
            foreach (ListItem Area in Lstareabox.Items)
            {
                if (Area.Selected)
                {
                    matAreanew += Area.Value + ",";
                }
            }
            matAreanew = matAreanew.TrimStart(',').TrimEnd(',');

            foreach (ListItem Beat in Lstbeatbox.Items)
            {
                if (Beat.Selected)
                {
                    matBeatNew += Beat.Value + ",";
                }
            }
            matBeatNew = matBeatNew.TrimStart(',').TrimEnd(',');
            foreach (ListItem PartyItem in Lstpartybox.Items)
            {
                if (PartyItem.Selected)
                {
                    strparty += PartyItem.Value + ",";
                }
            }
            strparty = strparty.TrimStart(',').TrimEnd(',');

            if (strparty != "")
            {
                Qrychk = Qrychk + "AND p.partyid in (" + strparty + ")";
            }
            else if (matBeatNew != "")
            {
                Qrychk = Qrychk + "AND p.BeatId in (" + matBeatNew + ")";
            }
            else if (matAreanew != "")
            {
                Qrychk = Qrychk + "AND p.AreaId in (" + matAreanew + ")";
            }
            else { }

            if (ddlPType.SelectedValue == "Active")
                Qrychk = Qrychk + " and (p.Active=1)";

            if (ddlPType.SelectedValue == "Not Active")
                Qrychk = Qrychk + " and p.Active=0 ";

            DateTime dt_Now = Settings.GetUTCTime();
            DateTime dt_preYear = dt_Now.AddYears(-1);
            DateTime dt_preMonth = dt_Now.AddMonths(-1);
            //string dt_dd = "01/" + dt_preMonth.Month.ToString() + "/" + dt_preMonth.Year.ToString();
            DateTime date1 = new DateTime(Convert.ToInt32((DateTime.ParseExact(dt_preMonth.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy")), Convert.ToInt32((DateTime.ParseExact(dt_preMonth.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM")), 1);

            int dt_months = DateTime.DaysInMonth(dt_preMonth.Year, dt_preMonth.Month);
            DateTime dt_dt_preStartDate = date1;//Convert.ToDateTime(dt_dd);
            DateTime date2 = new DateTime(Convert.ToInt32((DateTime.ParseExact(dt_preMonth.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy")), Convert.ToInt32((DateTime.ParseExact(dt_preMonth.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM")), dt_months);

            // string dt_enddate = dt_months.ToString() + "/" + dt_preMonth.Month.ToString() + "/" + dt_preMonth.Year.ToString();
            DateTime dt_EnddatePre = date2;
            Query = @"select max(a.DateDiff) as DateDiff,a.PartyName,a.Address,a.Area,a.beat,a.Partyid,max(a.lastvisit) as lastvisit,max(a.visitBy)as visitBy,max(a.SmId) AS Smid,sum(a.Amount) as Amount,max(a.Potential) as Potential,max(a.LastDate) as LastDate ,sum(a.AvgMonth) as AvgMonth,sum(a.LastMonth) as LastMonth from (
            SELECT min (DATEDIFF(day,pv.VisitDate,getdate())) AS DateDiff, p.PartyName,p.Address1 AS Address ,b.AreaName AS Area,b.AreaName AS Beat,p.PartyId,max(isnull(convert(varchar,pv.visitDate,103),'NA')) [lastvisit],isnull(case when cp.SMName IS not null THEN cp.SMName when cp1.SMName IS not null then cp1.SMName when cp2.SMName IS not null THEN cp2.SMName end,'NA')[visitBy],isnull(case when cp.SMName IS not null THEN cp.SMId when cp1.SMName IS not null then cp1.SMId when cp2.SMName IS not null THEN cp2.SMId end,0)[Smid],sum(isnull(om.OrderAmount,0)) [Amount],max (p.Potential) as Potential,MAX (om.VDate) as LastDate,0 as AvgMonth ,0 as LastMonth FROM MastParty p inner JOIN MastArea b ON p.BeatId = b.AreaId left join partyvisit pv on pv.PartyId=p.PartyId left JOIN transorder om on p.PartyId = om.PartyId and om.VDate=pv.visitDate left join TransDemo dm on p.PartyId = dm.PartyId and dm.VDate=pv.visitDate left JOIN transfailedVisit fv on p.PartyId = fv.PartyId and fv.VDate=pv.visitDate left join MastSalesRep cp on om.SMId = cp.SMId left  join MastSalesRep cp1 on dm.SMId = cp1.SMId left join MastSalesRep cp2 on fv.SMId = cp2.SMId WHERE ((p.BlockBy = 0) OR (p.BlockBy IS NULL)) AND p.PartyDist=0 AND  p.PartyId NOT IN (SELECT PartyId FROM PartyVisit WHERE VisitDate>DATEADD(day, -" + txt_noofrecords.Text + ", getdate())) and p.AreaId in (select ua.LinkCode FROM MastLink ua where  ua.PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)  " + Qrychk + " ) AND p.PartyDist=0  group by p.PartyName, b.AreaName, p.PartyId,cp.SMName,cp1.SMName,cp2.SMName,cp.SMId,cp1.SMId,cp2.SMId,p.Address1 union all " +
          " select 0 AS DateDiff, p.PartyName,p.Address1 AS Address ,b.AreaName AS Area ,b.AreaName [beat],p.PartyId,'' as[lastvisit],'' as [visitBy],om.SMId AS SmId,0 as  [Amount],max (p.potential  ) as Potential,'' as LastDate,sum(isnull(om.OrderAmount,0)/12) as AvgMonth ,0 as LastMonth from MastParty p inner JOIN MastArea b ON p.BeatId = b.AreaId inner JOIN transorder om on om.PartyId=p.PartyId LEFT JOIN mastsalesrep ms ON ms.SMId=om.SMId where om.VDate between '" + dt_preYear.ToString("dd/MMM/yyyy") + " 00:00'  and '" + dt_Now.ToString("dd/MMM/yyyy") + " 23:59' and ((p.BlockBy = 0) OR (p.BlockBy IS NULL)) and p.PartyId NOT IN (SELECT partyId FROM PartyVisit WHERE VisitDate>DATEADD(day, -" + txt_noofrecords.Text + ", getdate())) and p.AreaId in (select ua.LinkCode from MastLink ua where  ua.PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) " + Qrychk + ") group by p.PartyName,p.Address1,b.AreaName,b.AreaName,p.PartyId,om.smid union all " +
          " select 0 AS DateDiff, p.PartyName,p.Address1 AS Address ,b.AreaName AS Area ,b.AreaName [beat],p.PartyId,'' as[lastvisit],'' as [visitBy],0 AS SmId,0 as  [Amount],max (p.potential  ) as Potential,'' as LastDate,0 as AvgMonth ,SUM(om.OrderAmount) as LastMonth FROM MastParty p inner JOIN MastArea b ON p.BeatId = b.AreaId inner JOIN TransOrder om on om.PartyId=p.PartyId where om.VDate between '" + dt_dt_preStartDate.ToString("dd/MMM/yyyy") + " 00:00' and '" + dt_EnddatePre.ToString("dd/MMM/yyyy") + " 23:59' and p.PartyId NOT IN (SELECT Partyid FROM PartyVisit WHERE VisitDate>DATEADD(day, -" + txt_noofrecords.Text + ", getdate())) and p.AreaId in (select ua.LinkCode from MastLink ua where ua.PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) " + Qrychk + ") group by p.PartyName,p.Address1,b.AreaName,b.AreaName,p.PartyId ) a LEFT JOIN mastsalesrep mst ON mst.SMId=a.smid WHERE a.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1) group by a.PartyName,a.Address,a.Area,a.beat,a.PartyId  ORDER BY a.Partyname";

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            if (dtItem.Rows.Count > 0)
            {
                rptmain.Style.Add("display", "block");
                rptRETNVIST.DataSource = dtItem;
                rptRETNVIST.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                rptmain.Style.Add("display", "block");
                rptRETNVIST.DataSource = dtItem;
                rptRETNVIST.DataBind();
                btnExport.Visible = false;
            }

            dtItem.Dispose();
        }

        private void BindareaDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string cityQry = @"select AreaId,AreaName from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Area' and Active=1 order by AreaName";
                    DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);

                    if (dtArea.Rows.Count > 0)
                    {
                        Lstareabox.DataSource = dtArea;
                        Lstareabox.DataTextField = "AreaName";
                        Lstareabox.DataValueField = "AreaId";
                        Lstareabox.DataBind();
                    }

                    dtArea.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
                    rptRETNVIST.DataSource = null;
                    rptRETNVIST.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindbeatDDl(string SMIDStr)
        {
            try
            {
                if (SMIDStr != "")
                {
                    string cityQry = @"select AreaId,AreaName from mastarea where underId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Beat' and Active=1 order by AreaName";
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
                    rptRETNVIST.DataSource = null;
                    rptRETNVIST.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/NotVisitedOutletReport.aspx");
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
                    //BindbeatDDl(smIDStr12);
                    BindareaDDl(smIDStr12);
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
            Response.AddHeader("content-disposition", "attachment;filename=RetailerNotVisited.csv");
            string headertext = "Retailer".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + ", " + "Area".TrimStart('"').TrimEnd('"') + ", " + "Beat".TrimStart('"').TrimEnd('"') + "," + "Potential".TrimStart('"').TrimEnd('"') + "," + "Visit By".TrimStart('"').TrimEnd('"') + "," + "Total Order Amount".TrimStart('"').TrimEnd('"') + "," + "Last 12 Month Avg".TrimStart('"').TrimEnd('"') + "," + "Last Order Date".TrimStart('"').TrimEnd('"') + "," + "Last Visit".TrimStart('"').TrimEnd('"') + "," + "No Of Days".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Area", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Beat", typeof(string)));
            dtParams.Columns.Add(new DataColumn("Potential", typeof(string)));
            dtParams.Columns.Add(new DataColumn("VisitBy", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(string)));
            dtParams.Columns.Add(new DataColumn("AvgMonth", typeof(string)));
            dtParams.Columns.Add(new DataColumn("LastDate", typeof(string)));
            dtParams.Columns.Add(new DataColumn("LastVisit", typeof(string)));
            dtParams.Columns.Add(new DataColumn("DateDiff", typeof(string)));

            foreach (RepeaterItem item in rptRETNVIST.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblPartyName = item.FindControl("lblPartyName") as Label;
                dr["PartyName"] = lblPartyName.Text.ToString();
                Label lblAddress = item.FindControl("lblAddress") as Label;
                dr["Address"] = lblAddress.Text.ToString();
                Label lblArea = item.FindControl("lblArea") as Label;
                dr["Area"] = lblArea.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblBeat = item.FindControl("lblBeat") as Label;
                dr["Beat"] = lblBeat.Text.ToString();
                Label lblPotential = item.FindControl("lblPotential") as Label;
                dr["Potential"] = lblPotential.Text.ToString();
                Label lblVisitBy = item.FindControl("lblVisitBy") as Label;
                dr["VisitBy"] = lblVisitBy.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblAmount = item.FindControl("lblAmount") as Label;
                dr["Amount"] = lblAmount.Text.ToString();
                Label lblAvgMonth = item.FindControl("lblAvgMonth") as Label;
                dr["AvgMonth"] = lblAvgMonth.Text.ToString();
                Label lblLastDate = item.FindControl("lblLastDate") as Label;
                dr["LastDate"] = lblLastDate.Text.ToString();
                Label lblLastVisit = item.FindControl("lblLastVisit") as Label;
                dr["LastVisit"] = lblLastVisit.Text.ToString();
                Label lblDateDiff = item.FindControl("lblDateDiff") as Label;
                dr["DateDiff"] = lblDateDiff.Text.ToString();

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
            Response.AddHeader("content-disposition", "attachment;filename=RetailerNotVisited.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
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
                string str = @"select PartyId,partyName from Mastparty where Beatid in (" + strparty + ") and partydist=0 and active=1 order by partyname";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    Lstpartybox.DataSource = dt;
                    Lstpartybox.DataTextField = "partyName";
                    Lstpartybox.DataValueField = "PartyId";
                    Lstpartybox.DataBind();
                }
                //     ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
            }
            else
            {
                ClearControls();
            }
        }
        private void ClearControls()
        {
            try
            {
                Lstpartybox.Items.Clear();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Lstareabox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strarea = string.Empty;
                foreach (ListItem area in Lstareabox.Items)
                {
                    if (area.Selected)
                    {
                        strarea += area.Value + ",";
                    }
                }
                strarea = strarea.TrimStart(',').TrimEnd(',');

                if (strarea != "")
                {
                    string cityQry = @"select AreaId,AreaName from mastarea where underId in (" + strarea + ") and Active=1 and areatype='Beat' and Active=1 order by AreaName";
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

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}