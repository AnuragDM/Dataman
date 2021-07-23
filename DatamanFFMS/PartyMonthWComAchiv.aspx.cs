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
    public partial class PartyMonthWComAchiv : System.Web.UI.Page
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

                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    TopRetailer.DataSource = new DataTable();
                    TopRetailer.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }
                DateTime curr_dt = Convert.ToDateTime(txttodate.Text);
                DateTime prev_dt = curr_dt.AddMonths(-1);
                DateTime last_dt = prev_dt.AddMonths(-2);


                int yr = Convert.ToInt32((DateTime.ParseExact(last_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
                int mnth = Convert.ToInt32((DateTime.ParseExact(last_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
                DateTime ldate = new DateTime(yr, mnth, 1);

                yr = Convert.ToInt32((DateTime.ParseExact(prev_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy"));
                mnth = Convert.ToInt32((DateTime.ParseExact(prev_dt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture)).ToString("MM"));
                DateTime pdate = new DateTime(yr, mnth, DateTime.DaysInMonth(yr, mnth));


                Qrychk = " WHERE os.VDate <='" + Settings.dateformat(txttodate.Text) + "' ";
                Qrychk = Qrychk + " and cp.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                string Qty2 = " WHERE os.VDate BETWEEN '" + ldate.ToString("dd/MMM/yyyy") + "' AND '" + pdate.ToString("dd/MMM/yyyy") + "' ";
                Qty2 = Qty2 + " and cp.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                string Qty3 = " WHERE os.VDate BETWEEN '" + Settings.dateformat(txtfmDate.Text) + "' AND '" + Settings.dateformat(txttodate.Text) + "' ";
                Qty3 = Qty3 + " and cp.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smIDStr1 + ")) and Active=1)";

                //   Qrychk = " os.VDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and os.VDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";

                string query = @"SELECT A.AreaId,PartyName, a.areaname AS Area,sum(Pendency) [Pendency], SalePerson,sum(PrevMonth) AS PrevMonth,sum(CurrentMnth) AS CurrentMnth, 
                                   ISNULL( SUM((CurrentMnth*100)/nullif(prevmonth,0)),100) as Percentage,q.Mobile,q.Beat,q.Conper FROM (
                   select p.PartyId,p.PartyName,p.AreaId,SUM(os.Qty * os.Rate) as Pendency,max(cp.SMName) as SalePerson,0 AS PrevMonth,0 AS CurrentMnth,0 Percentage,p.Mobile,
                   p.ContactPerson as Conper,b.AreaName as Beat from TransOrder1 os inner join MastParty p on P.PartyId=os.PartyId left JOIN mastarea b on b.AreaId=p.BeatId 
                   left join MastSalesRep cp on cp.SMId=os.SMId " + Qrychk + " " + beat_Filter + " Group by p.PartyId,p.PartyName,p.AreaId,p.Mobile,p.ContactPerson,b.AreaName " +
   " union all select p.PartyId,p.PartyName, p.AreaId,0 Pendency,max(cp.SMName) as SalePerson,sum(((isnull(os.Qty,0))*isnull(os.Rate,0))/3) as PrevMonth,0 AS CurrentMnth, " +
              " 0 Percentage,p.Mobile,p.ContactPerson as Conper,b.AreaName as Beat from TransOrder1 os inner join MastParty p on P.PartyId=os.PartyId left join MastSalesRep " +
              " cp on cp.SMId=os.SMId left JOIN mastarea b on b.AreaId=p.BeatId " + Qty2 + " " + beat_Filter + " Group by p.PartyId,p.PartyName,p.AreaId,p.Mobile,p.ContactPerson,b.AreaName " +
  " union all  select p.PartyId,p.PartyName, p.AreaId,0 AS Pendency,max(cp.SMName) as SalePerson,0 AS PrevMonth,sum((isnull(os.Qty,0))*isnull(os.Rate,0)) as CurrentMnth, " +
               "0 Percentage ,p.Mobile,p.ContactPerson as Conper,b.AreaName as Beat from TransOrder1 os inner join MastParty p on p.PartyId=os.PartyId left join MastSalesRep " +
               " cp on cp.SMId =os.SMId left JOIN mastarea b on b.AreaId=p.BeatId " + Qty3 + " " + beat_Filter + " Group by p.PartyId,p.PartyName,p.AreaId,p.Mobile,p.ContactPerson,b.AreaName " +
              " ) q LEFT JOIN MastArea a ON q.areaid = a.AreaId GROUP BY PartyName, a.AreaName, SalePerson,q.Mobile,q.Conper,q.Beat ,A.areaid order by PartyName";

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
            Response.AddHeader("content-disposition", "attachment;filename=PartyMonthWComAchiv.csv");
            string headertext = "Sr.No.".TrimStart('"').TrimEnd('"') + "," + "Retailer".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Contact Person".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Total Sale till Today".TrimStart('"').TrimEnd('"') + "," + "Avg Monthly Sale In last 3 Months".TrimStart('"').TrimEnd('"') + "," + "Current Month sale".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("SNo", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Area", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Beat", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ConPer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Pendency", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PrevMonth", typeof(String)));
            dtParams.Columns.Add(new DataColumn("CurrentMnth", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Saleperson", typeof(String)));

            foreach (RepeaterItem item in TopRetailer.Items)
            {
                DataRow dr = dtParams.NewRow();

                Label lblSno = item.FindControl("lblSno") as Label;
                dr["SNo"] = lblSno.Text.ToString();
                Label lblPartyName = item.FindControl("lblPartyName") as Label;
                dr["PartyName"] = lblPartyName.Text.ToString();
                Label lblArea = item.FindControl("lblArea") as Label;
                dr["Area"] = lblArea.Text.ToString();
                Label lblBeat = item.FindControl("lblBeat") as Label;
                dr["Beat"] = lblBeat.Text.ToString();
                Label lblConPer = item.FindControl("lblConPer") as Label;
                dr["ConPer"] = lblConPer.Text.ToString();
                Label lblMobile = item.FindControl("lblMobile") as Label;
                dr["Mobile"] = lblMobile.Text.ToString();
                Label lblPendency = item.FindControl("lblPendency") as Label;
                dr["Pendency"] = lblPendency.Text.ToString();
                Label lblPrevMonth = item.FindControl("lblPrevMonth") as Label;
                dr["PrevMonth"] = lblPrevMonth.Text.ToString();
                Label lblCurrentMnth = item.FindControl("lblCurrentMnth") as Label;
                dr["CurrentMnth"] = lblCurrentMnth.Text.ToString();
                Label lblSalePerson = item.FindControl("lblSalePerson") as Label;
                dr["Saleperson"] = lblSalePerson.Text.ToString();
                dtParams.Rows.Add(dr);
            }
            decimal[] totalVal = new decimal[9];
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
                            if (k == 6 || k == 7 || k == 8)
                            {
                                if (dtParams.Rows[j][k].ToString() != "")
                                {
                                    totalVal[k] += Convert.ToDecimal(dtParams.Rows[j][k].ToString());
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
                            if (k == 6 || k == 7 || k == 8)
                            {
                                if (dtParams.Rows[j][k].ToString() != "")
                                {
                                    totalVal[k] += Convert.ToDecimal(dtParams.Rows[j][k].ToString());
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
                            if (k == 6 || k == 7 || k == 8)
                            {
                                if (dtParams.Rows[j][k].ToString() != "")
                                {
                                    totalVal[k] += Convert.ToDecimal(dtParams.Rows[j][k].ToString());
                                }
                            }
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


            //Response.Clear();
            //Response.ContentType = "text/csv";
            //Response.AddHeader("content-disposition", "attachment;filename=PartyMonthWComAchiv.csv");
            //Response.Write(sb.ToString());
            //Response.End();

            sb.Clear();
            dtParams.Dispose();
        }
    }
}

