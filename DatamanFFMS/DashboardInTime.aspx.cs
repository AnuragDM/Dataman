using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Text;
using System.Reflection;
using BusinessLayer;
using System.IO;
using System.Globalization;
using System.Collections;
using Telerik.Web.UI;
using System.Web.Services;
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class DashboardInTime : System.Web.UI.Page
    {
        //  string roleType = "", total = "";
        //DataTable dtEmployee = null;
        //string sql = "";
        //string secondarySql = "";
        //string PrimarySql = "";
        //string UnApprovedSql = "";
        //string name = "Competitor";
        //string Date = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);

            if (!IsPostBack)
            {
                FromDate.Text = Request.QueryString["Date"];
                ToDate.Text = Request.QueryString["Date"];
                fillUser(FromDate.Text, ToDate.Text);
               
               // fillUser(ToDate.Text);
                //  this.BindWithSalesPersonDDl(Convert.ToInt32(Settings.Instance.SMID));
                this.fillAllRecord();
                lblHeading.InnerText = "DateWise In-Time Statistics Detail";
            }
        }

        private void fillUser(string SearchDate,string ToDate)
        {

            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select distinct tv.smid,sr.smname from transvisit tv inner join mastsalesrep sr on tv.smid=sr.smid where CONVERT(VARCHAR(20),created_date,101) BETWEEN  '" + Convert.ToDateTime(SearchDate).ToString("MM/dd/yyyy") + "' And '" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "'");

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    lstUndeUser.DataSource = dt;
                    lstUndeUser.DataTextField = "smname";
                    lstUndeUser.DataValueField = "SMId";
                    lstUndeUser.DataBind();
                    lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
               
            }
           
        }
        public void fillAllRecord(int SalesType = 0)
        {
            string SearchDate = FromDate.Text;
            string SDate = ToDate.Text;
            //roleType = Settings.Instance.RoleType;
            //int SalesType = 0;
            string ChartType = Request.QueryString["Name"];

            DataTable dt = new DataTable(); DataTable dtt = new DataTable(); DataRow[] rowArray = null;
           
            //dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select * from((select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'Before 9:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(created_date As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As Time) < CAST('9:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smname,smid,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'9:00-10:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(created_date As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As Time) between CAST('9:00' As Time) and CAST('10:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'10:00-11:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(created_date As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As Time) between CAST('10:00' As Time) and CAST('11:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'11:00-12:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(created_date As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As Time) between CAST('11:00' As Time) and CAST('12:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select count(visid) InTime,'After 12:00' Name,tv.smid,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(created_date As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and CAST(created_date As Time) > CAST('12:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)) a order by convert(datetime,  a.date, 103) asc");

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select * from (select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select tv.smid,count(visid) InTime,'Before 9:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(vDate As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(vDate As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and frTime1 < '09:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smname,smid,vDate,frTime1 " +

              " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select tv.smid,count(visid) InTime,'9:00-10:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(vDate As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(vDate As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and frTime1 >= '09:00' and frTime1 < '10:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1 " +

            " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select tv.smid,count(visid) InTime,'10:00-11:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(vDate As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(vDate As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and frTime1 >= '10:00' and frTime1 < '11:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1 " +

            " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select tv.smid,count(visid) InTime,'11:00-12:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(vDate As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(vDate As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and frTime1 >= '11:00' and frTime1 < '12:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1 " +

           " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select count(visid) InTime,'After 12:00' Name,tv.smid,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CAST(vDate As date) >= cast('" + SearchDate.Replace('-', '/').ToString() + "' as Date) and CAST(vDate As date) <= cast('" + SDate.Replace('-', '/').ToString() + "' as Date) and frTime1 >= '12:00' and tv.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1) a order by convert(datetime,  a.date, 103) asc");

            if (SalesType == 0)
            {
                if (ChartType == "Before 9:00")
                    rowArray = dt.Select("Name='Before 9:00'");
                else if (ChartType == "9:00-10:00")
                    rowArray = dt.Select("Name='9:00-10:00'");
                else if (ChartType == "10:00-11:00")
                    rowArray = dt.Select("Name='10:00-11:00'");
                else if (ChartType == "11:00-12:00")
                    rowArray = dt.Select("Name='11:00-12:00'");
                else if (ChartType == "After 12:00")
                    rowArray = dt.Select("Name='After 12:00'");
                dtt = dt.Clone();
                foreach (DataRow row in rowArray)
                    dtt.ImportRow(row);
            }

            if (SalesType > 0)
            {
                if (ChartType == "Before 9:00")
                    rowArray = dt.Select("Name='Before 9:00' and SMId='" + SalesType + "'");
                else if (ChartType == "9:00-10:00")
                    rowArray = dt.Select("Name='9:00-10:00' and SMId='" + SalesType + "'");
                else if (ChartType == "10:00-11:00")
                    rowArray = dt.Select("Name='10:00-11:00' and SMId='" + SalesType + "'");
                else if (ChartType == "11:00-12:00")
                    rowArray = dt.Select("Name='11:00-12:00' and SMId='" + SalesType + "'");
                else if (ChartType == "After 12:00")
                    rowArray = dt.Select("Name='After 12:00' and SMId=" + SalesType + "");
                dtt = dt.Clone();
                foreach (DataRow row in rowArray)
                    dtt.ImportRow(row);
            }

            rptt.DataSource = dtt;
            rptt.DataBind();

            if (Convert.ToDateTime(FromDate.Text) > Convert.ToDateTime(ToDate.Text))
            {
                rptt.DataSource = new DataTable();
                rptt.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }
        }
        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                fillUser(FromDate.Text,ToDate.Text);
                lstUndeUser.SelectedValue = "0";
                this.fillAllRecord(Convert.ToInt32(lstUndeUser.SelectedValue));
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }
        }
        protected void ToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                fillUser(FromDate.Text, ToDate.Text);
                //lstUndeUser.SelectedValue = "0";
                this.fillAllRecord(Convert.ToInt32(lstUndeUser.SelectedValue));
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
            catch (Exception ex)
            { ex.ToString(); }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DashboardInTime.csv");
            string headertext = "Name".TrimStart('"').TrimEnd('"') + "," + "Date".TrimStart('"').TrimEnd('"') + "," + "In-Time".TrimStart('"').TrimEnd('"');
           
            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
            dtParams.Columns.Add(new DataColumn("date", typeof(String)));
            dtParams.Columns.Add(new DataColumn("time", typeof(String)));


            foreach (RepeaterItem item in rptt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblsmname = item.FindControl("lblsmname") as Label;
                dr["smname"] = lblsmname.Text.ToString();
                Label lbldate = item.FindControl("lbldate") as Label;
                dr["date"] = lbldate.Text.ToString();
                Label lbltime = item.FindControl("lbltime") as Label;
                dr["time"] = lbltime.Text.ToString();

                dtParams.Rows.Add(dr);
            }
            //DataView dv = dtParams.DefaultView;
            //dv.Sort = "date desc";
            //dtParams = dv.ToTable();

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
            Response.AddHeader("content-disposition", "attachment;filename=DashboardInTime.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

    }

}

