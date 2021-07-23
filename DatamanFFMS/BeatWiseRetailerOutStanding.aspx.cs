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
    public partial class BeatWiseRetailerOutStanding : System.Web.UI.Page
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
                btnExport.Visible = true;
                BindTreeViewControl();
                BindAreaDDl();
                //  BindbeatDDl();                
                //btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                //btnExport.CssClass = "btn btn-primary";
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {

            string smIDStr = "", smIDStr1 = "", areastr = "", matBeatNew = "";

            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

            foreach (ListItem Area in AreaListbox.Items)
            {
                if (Area.Selected)
                {
                    areastr += Area.Value + ",";
                }
            }
            areastr = areastr.TrimStart(',').TrimEnd(',');

            foreach (ListItem Beat in Lstbeatbox.Items)
            {
                if (Beat.Selected)
                {
                    matBeatNew += Beat.Value + ",";
                }
            }
            matBeatNew = matBeatNew.TrimStart(',').TrimEnd(',');

            string area_Filter = "";
            if (areastr != "" && areastr != "0")
                area_Filter = " and mp.AreaId in (" + areastr + ") ";
            else
                area_Filter = "";

            string beat_Filter = "";
            if (matBeatNew != "" && matBeatNew != "0")
                beat_Filter = " and mp.BeatId in (" + matBeatNew + ") ";
            else
                beat_Filter = "";


            if (smIDStr1 != "")
            {
                string query = @"Select mp.partyid,mp.PartyName,ma.AreaName as Area,ma1.AreaName as Beat,ms.SMName,mp.OutStanding from mastparty mp 
               left join MastArea ma on mp.Areaid=ma.areaid left join MastArea ma1 on mp.BeatId=ma1.Areaid 
               left join mastsalesrep ms on mp.SalespersonSyncId=ms.SyncId
               where mp.Partydist=0 " + area_Filter + " " + beat_Filter + " and mp.OutStanding is not NULL and mp.OutStanding not in (0.00) ";

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
        private void BindAreaDDl()
        {
            try
            {

                string AreaQry = @"select AreaId,AreaName from mastarea where areatype='Area' and Active=1 Order by AreaName";
                DataTable dtArea = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQry);
                if (dtArea.Rows.Count > 0)
                {
                    AreaListbox.DataSource = dtArea;
                    AreaListbox.DataTextField = "AreaName";
                    AreaListbox.DataValueField = "AreaId";
                    AreaListbox.DataBind();
                }
                dtArea.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindbeatDDl(string AreaStr)
        {
            try
            {

                string cityQry = @"select AreaId,AreaName from mastarea where areatype='Beat' and Active=1 and underid in (" + AreaStr + ") Order by AreaName";
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
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        //private void BindbeatDDl(string SMIDStr)
        //{
        //    try
        //    {
        //        if (SMIDStr != "")
        //        {
        //            //string cityQry = @"  select AreaId,AreaName from mastarea where underId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + SMIDStr + ")) and Active=1 )) and areatype='Beat' and Active=1 order by AreaName";
        //            string cityQry = @"select AreaId,AreaName from mastarea where areatype='Beat' and Active=1";
        //            DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
        //            if (dtBeat.Rows.Count > 0)
        //            {
        //                Lstbeatbox.DataSource = dtBeat;
        //                Lstbeatbox.DataTextField = "AreaName";
        //                Lstbeatbox.DataValueField = "AreaId";
        //                Lstbeatbox.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select salesperson');", true);
        //            rptTopPotentials.DataSource = null;
        //            rptTopPotentials.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatWiseRetailerOutStanding.aspx");
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
                    //  BindbeatDDl(smIDStr12);
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
            Response.AddHeader("content-disposition", "attachment;filename=BeatWiseRetailerOutStanding.csv");
            string headertext = "Retailer".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "SalesPerson".TrimStart('"').TrimEnd('"') + "," + "Out Standing".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Retailer", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Area", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Beat", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OutStanding", typeof(decimal)));

            foreach (RepeaterItem item in rptTopPotentials.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblPartyName = item.FindControl("lblPartyName") as Label;
                dr["Retailer"] = lblPartyName.Text.ToString();

                Label lblArea = item.FindControl("lblArea") as Label;
                dr["Area"] = lblArea.Text.ToString();

                Label lblBeat = item.FindControl("lblBeat") as Label;
                dr["Beat"] = lblBeat.Text.ToString();

                Label lblSmName = item.FindControl("lblSmName") as Label;
                dr["SalesPerson"] = lblSmName.Text.ToString();

                Label lblOutStanding = item.FindControl("lblOutStanding") as Label;
                dr["OutStanding"] = lblOutStanding.Text.ToString();

                dtParams.Rows.Add(dr);
            }

            DataView dv = dtParams.DefaultView;
            dv.Sort = "OutStanding desc";
            DataTable udtNew = dv.ToTable();

            for (int j = 0; j < udtNew.Rows.Count; j++)
            {
                for (int k = 0; k < udtNew.Columns.Count; k++)
                {
                    if (udtNew.Rows[j][k].ToString().Contains(","))
                    {
                        string h = udtNew.Rows[j][k].ToString();
                        string d = h.Replace(",", " ");
                        udtNew.Rows[j][k] = "";
                        udtNew.Rows[j][k] = d;
                        udtNew.AcceptChanges();
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (udtNew.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k]).ToString() + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        if (k == 0)
                        {
                            //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                            sb.Append(udtNew.Rows[j][k].ToString() + ',');
                        }
                        else
                        {
                            sb.Append(udtNew.Rows[j][k].ToString() + ',');
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=BeatWiseRetailerOutStanding.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
            dv.Dispose();
            udtNew.Dispose();
        }

        protected void AreaListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strarea = "";
            foreach (ListItem Area in AreaListbox.Items)
            {
                if (Area.Selected)
                {
                    strarea += Area.Value + ",";
                }
            }
            strarea = strarea.TrimStart(',').TrimEnd(',');

            BindbeatDDl(strarea);
        }
    }
}
