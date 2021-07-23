using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

namespace AstralFFMS
{
    public partial class DashboardNoSalesReason : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);
            if (!IsPostBack)
            {
                FromDate.Text = Request.QueryString["Date"];
                ToDate.Text = Request.QueryString["Date"];

                fillUser(FromDate.Text, ToDate.Text);
                fillFailedVisit();
               // fillUser(ToDate.Text);
                this.fillAllRecord();
              
                lblHeading.InnerText = "No sales Reason Details";
               // btnExport.Visible = true;
                btnExport.CssClass = "btn btn-primary";
            }
        }


        private void fillUser(string SearchDate, string SDate)
        {

            //DataTable dt = new DataTable();
            //dt = DbConnectionDAL.GetDataTable(CommandType.Text, "(select distinct tfv.smid,mas.smname from transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid  left join mastarea ma on mp.areaid=ma.areaid WHERE mp.partydist=0 and VDate='" + SearchDate.Replace('-', '/').ToString() + "')Union All(select tfv.smid,mas.smname  from temp_transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid WHERE mp.partydist=0 and VDate='" + SearchDate.Replace('-', '/').ToString() + "')");

            string str = "(select distinct tfv.smid,mas.smname from transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid  left join mastarea ma on mp.areaid=ma.areaid WHERE mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "') Union (select distinct tfv.smid,mas.smname  from temp_transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid WHERE mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "')";
            fillDDLDirect(lstUndeUser, str, "SMId", "smname", 1);

            //if (dt != null)
            //{
            //    if (dt.Rows.Count > 0)
            //    {
            //        fillDDLDirect(lstUndeUser, str, "SMId", "smname", 1);
            //        //lstUndeUser.DataSource = dt;
            //        //lstUndeUser.DataTextField = "smname";
            //        //lstUndeUser.DataValueField = "SMId";
            //        //lstUndeUser.DataBind();
            //        //lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));
            //    }
                
            //}
             //lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            xddl.DataSource = null;
            xddl.DataBind();
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("All", "0"));
            }
            xdt.Dispose();
        }
        public void fillAllRecord(int SalesType = 0)
        {
            string SearchDate = FromDate.Text;
            string SDate = ToDate.Text;
            string ReasonName = Request.QueryString["Name"];
            string FailedVisit = "",query="";
            string _search = "";

            if (ddlFailedVisit.SelectedValue != "0") _search = "and tfv.ReasonId in(" + ddlFailedVisit.SelectedValue.ToString() + ")";
            if (lstUndeUser.SelectedValue != "0") _search =_search + "and tfv.smid in(" + lstUndeUser.SelectedValue.ToString() + ")";
            //if (ddlFailedVisit.SelectedValue != "")
            //{
            //    FailedVisit = "and tfv.ReasonId in(" + ddlFailedVisit.SelectedValue.ToString() + ")";

            //}

            DataTable dt = new DataTable(); DataTable dtt = new DataTable(); DataRow[] rowArray = null;

            //if (ddlFailedVisit.SelectedValue != "0")
            //{
            //    query = "select * from((select mfvr.FVName Reason,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,convert(varchar(20),tfv.VDate,106) [VDate],mp.beatid,ma.AreaName,ma1.AreaName BeatName from transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' " + FailedVisit + " and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate )Union All(select mfvr.FVName,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,tfv.VDate,mp.beatid,ma.AreaName,ma1.AreaName BeatName   from temp_transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' " + FailedVisit + " and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate ))a order by convert(datetime,  a.VDate, 103) asc";
            //}
            //else {
            //    query = "select * from((select mfvr.FVName Reason,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,convert(varchar(20),tfv.VDate,106) [VDate],mp.beatid,ma.AreaName,ma1.AreaName BeatName from transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate )Union All(select mfvr.FVName,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,tfv.VDate,mp.beatid,ma.AreaName,ma1.AreaName BeatName   from temp_transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate ))a order by convert(datetime,  a.VDate, 103) asc";
            //}

            query = "select * from((select mfvr.FVName Reason,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,convert(varchar(20),tfv.VDate,106) [VDate],mp.beatid,ma.AreaName,ma1.AreaName BeatName from transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' " + _search + " and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate )Union All(select mfvr.FVName,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,tfv.VDate,mp.beatid,ma.AreaName,ma1.AreaName BeatName   from temp_transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' " + _search + " and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate ))a order by convert(datetime,  a.VDate, 103) asc";

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            rpt.DataSource = dt;
            rpt.DataBind();
            //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "(select mfvr.FVName Reason,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,tfv.VDate,mp.beatid,ma.AreaName,ma1.AreaName BeatName from transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' " + FailedVisit + " and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate )Union All(select mfvr.FVName,tfv.smid,mas.smname,mp.PartyName,mp.PartyId,mp.mobile,mp.areaid,mp.ImgUrl,tfv.VDate,mp.beatid,ma.AreaName,ma1.AreaName BeatName   from temp_transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mfvr.FVName='" + ReasonName + "' " + FailedVisit + " and mp.partydist=0 and VDate between '" + SearchDate.Replace('-', '/').ToString() + "' and '" + SDate.Replace('-', '/').ToString() + "' and tfv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName,PartyName,mp.PartyId,mp.mobile,mp.ImgUrl,tfv.VDate )");
            
            //dtt = dt.Copy();
            //if (SalesType > 0)
            //{
            //    rowArray = dt.Select("SMId=" + SalesType + "");
            //    dtt = dt.Clone();
            //    foreach (DataRow row in rowArray)
            //        dtt.ImportRow(row);
            //}

            //rpt.DataSource = dtt;
            //rpt.DataBind();
            if (Convert.ToDateTime(FromDate.Text) > Convert.ToDateTime(ToDate.Text))
            {
                rpt.DataSource = new DataTable();
                rpt.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }

        }

        private void fillFailedVisit()
        {
            try
            {
                string strvisit = " select Fvid, FVName from MastFailedVisitRemark";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strvisit);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        
                        ddlFailedVisit.DataSource = dt;
                        ddlFailedVisit.DataTextField = "FVName";
                        ddlFailedVisit.DataValueField = "Fvid";
                        ddlFailedVisit.DataBind();
                    }
                }
                ddlFailedVisit.Items.Insert(0, new ListItem("-- All --", "0"));

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void FailedVisit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                
                this.fillAllRecord();
               
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }
            
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DashboardNoSalesReason.csv");
                string headertext = "Date".TrimStart('"').TrimEnd('"') + "," + "Name".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Reason".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;

                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("VDate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
                dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("mobile", typeof(String)));
                dtParams.Columns.Add(new DataColumn("areaname", typeof(String)));
                dtParams.Columns.Add(new DataColumn("beatname", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Reason", typeof(String)));

                foreach (RepeaterItem item in rpt.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label lbldate = item.FindControl("lbldate") as Label;
                    dr["VDate"] = lbldate.Text.ToString();
                    Label lblsmname = item.FindControl("lblsmname") as Label;
                    dr["smname"] = lblsmname.Text.ToString();
                    Label lblpartyname = item.FindControl("lblpartyname") as Label;
                    dr["PartyName"] = lblpartyname.Text.ToString();
                    Label lblmobile = item.FindControl("lblmobile") as Label;
                    dr["mobile"] = lblmobile.Text.ToString();
                    Label lblareaname = item.FindControl("lblareaname") as Label;
                    dr["areaname"] = lblareaname.Text.ToString();
                    Label lblbeatname = item.FindControl("lblbeatname") as Label;
                    dr["beatname"] = lblbeatname.Text.ToString();
                    Label lblreason = item.FindControl("lblreason") as Label;
                    dr["Reason"] = lblreason.Text.ToString();

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
                                // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                                // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
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
                Response.AddHeader("content-disposition", "attachment;filename=DashboardNoSalesReason.csv");
                Response.Write(sb.ToString());
                Response.End();
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

        protected void lnkViewDemoImg_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                var item = (RepeaterItem)btn.NamingContainer;
                HiddenField hdnId = (HiddenField)item.FindControl("linkHiddenField");
               
                    Response.ContentType = ContentType;
                    if (hdnId.Value != "")
                    {
                        btn.Visible = true;
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdnId.Value));
                        Response.WriteFile(hdnId.Value);
                        Response.End();
                    }
                    else
                    {
                        btn.Visible = false;
                    }
              
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                fillUser(FromDate.Text,ToDate.Text);
                lstUndeUser.SelectedValue = "0";
                ddlFailedVisit.SelectedValue = "0";
                this.fillAllRecord(Convert.ToInt32(lstUndeUser.SelectedValue));
                this.fillAllRecord(Convert.ToInt32(ddlFailedVisit.SelectedValue));
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch(Exception ex)
            { ex.ToString(); }
        }
        protected void ToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                fillUser(FromDate.Text, ToDate.Text);
                lstUndeUser.SelectedValue = "0";
                ddlFailedVisit.SelectedValue = "0";
                this.fillAllRecord(Convert.ToInt32(lstUndeUser.SelectedValue));
                this.fillAllRecord(Convert.ToInt32(ddlFailedVisit.SelectedValue));
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }
        }
        protected void lstUndeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                this.fillAllRecord(Convert.ToInt32(lstUndeUser.SelectedValue));
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch(Exception ex)
            { ex.ToString(); }
        }
    }
}