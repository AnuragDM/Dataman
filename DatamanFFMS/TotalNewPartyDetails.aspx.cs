using System;
using BusinessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using DAL;
using System.Data;
using BAL;
using Newtonsoft.Json;
using System.Web.Services;
using System.Text;

namespace AstralFFMS
{
    public partial class TotalNewPartyDetails : System.Web.UI.Page
    {
        string smid = string.Empty;
        string From = string.Empty;
        string To = string.Empty;
        string Sm = string.Empty;

        // string ItemName = string.Empty;
        string outputStr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            // txtfmDate.Attributes.Add("readonly", "readonly");
            // txttodate.Attributes.Add("readonly", "readonly");
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["smid"] != null)
                {
                    smid = Request.QueryString["smid"];
                }
                if (Request.QueryString["FromDate"] != null)
                {
                    From = Request.QueryString["FromDate"];
                }
                if (Request.QueryString["ToDate"] != null)
                {
                    To = Request.QueryString["ToDate"];
                }
                if (Request.QueryString["smidstr"] != null)
                {
                    Sm = Request.QueryString["smidstr"];
                }
                // Sm = Sm.TrimStart(',').TrimEnd(',');
                // txtfmDate.Text = From;
                // txttodate.Text = To;
                //outputStr = Sm.Trim('\'');
                lblfDate.Text = From;
                lbltoodate.Text = To;
                lbltotalparty.Text = smid.ToString();
                fillRepeter();
                fillRepeter1();
            }
        }
        private void fillRepeter()
        {
            string sql = string.Empty;


            string Qty2 = "";
            string Qty1 = "";
            string Qrychk = "";
            Qty2 = Qty2 + " and om.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smid + ")) and Active=1)";

            Qrychk = " and P.Created_Date between '" + From + " 00:00' and '" + To + " 23:59'";

            Qty1 = Qty1 + " and p.Created_User_id in ( SELECT userid FROM MastSalesRep ms LEFT JOIN mastlogin ml ON ms.UserId=ml.Id WHERE ms.SMId IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smid + ")) and Active=1)";

            //            string query = @"select a.PartyId,a.Name,a.Address,a.Mobile,max (a.beat) as beat,max (a.[By]) as [By],sum(a.Business) as Business,sum(a.Pendency) as Pendency,max(a.Date) as Date from (
            //                  select distinct p.PartyId, p.PartyName as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,max(b.AreaName) AS beat,max(ms.SMName) AS [By],0 AS Business,0 AS Pendency,max(p.created_date) as Date 
            //                  FROM MastParty p left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id 
            //                  WHERE p.partydist=0 " + Qrychk + " " + Qty1 + " ) group by p.PartyName,p.PartyId " +
            //          " union all select distinct p.PartyId,p.PartyName as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,max(b.AreaName) AS beat,max(ms.SMName) AS [By],sum(isnull(om.OrderAmount,0)) AS Business,sum(isnull(om.OrderAmount,0)) AS Pendency,max(p.Created_Date) as Date from TransOrder om inner join MastParty p on p.PartyId=om.PartyId left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id WHERE p.PartyDist=0 " + Qrychk + " " + Qty2 + " group by p.PartyId,p.PartyName) a Group by a.PartyId,a.Name,a.Address,a.Mobile Order by Business desc,MAX( a.Date) DESC,name";

            string query = @"select a.PartyId,a.Name,a.Address,a.Mobile,max (a.beat) as beat,max (a.[By]) as [By],sum(a.Business) as Business,sum(a.Pendency) as Pendency,max(a.Date) as Date from (
                  select distinct p.PartyId, p.PartyName as Name, max(p.Address1) AS Address,max(p.Mobile) as Mobile,max(b.AreaName) AS beat,max(ms.SMName) AS [By],(SELECT sum(OrderAmount) FROM TransOrder WHERE PartyId=p.PartyId) AS Business,0 AS Pendency,max(p.created_date) as Date 
                  FROM MastParty p left JOIN MASTAREA b on b.AreaId=p.BeatId LEFT JOIN mastsalesrep ms ON ms.UserId=p.Created_User_id 
                  WHERE p.partydist=0 " + Qrychk + " " + Qty1 + " ) group by p.PartyName,p.PartyId " +
                  " ) a Group by a.PartyId,a.Name,a.Address,a.Mobile Order by MAX( a.Date) asc,name";


            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            Topnewparty.DataSource = dt;
            Topnewparty.DataBind();

            dt.Dispose();
        }

        private void fillRepeter1()
        {

            string mastItemQry1 = string.Empty;
            mastItemQry1 = @"select smid,SMName from MastSalesRep where SMId in (" + smid + ") and Active=1";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
            lbltotalparty.Text = dt.Rows[0][1].ToString();

            dt.Dispose();
        }


        protected void btnExport(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TotalNewPartyDetails.csv");
            string headertext = "Retailer".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Beat".TrimStart('"').TrimEnd('"') + "," + "Created By".TrimStart('"').TrimEnd('"') + "," + "Created Date".TrimStart('"').TrimEnd('"') + "," + "Buisness".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);


            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Name", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Beat", typeof(String)));
            dtParams.Columns.Add(new DataColumn("By", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Date", typeof(string)));
            dtParams.Columns.Add(new DataColumn("Business", typeof(string)));
            //dtParams.Columns.Add(new DataColumn("Pendency", typeof(String)));

            foreach (RepeaterItem item in Topnewparty.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblName = item.FindControl("lblName") as Label;
                dr["Name"] = lblName.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblAddress = item.FindControl("lblAddress") as Label;
                dr["Address"] = lblAddress.Text.ToString().Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                Label lblMobile = item.FindControl("lblMobile") as Label;
                dr["Mobile"] = lblMobile.Text.ToString();
                Label lblBeat = item.FindControl("lblBeat") as Label;
                dr["Beat"] = lblBeat.Text.ToString();
                Label lblBy = item.FindControl("lblBy") as Label;
                dr["By"] = lblBy.Text.ToString();
                Label lblDate = item.FindControl("lblDate") as Label;
                dr["Date"] = Convert.ToDateTime(lblDate.Text).ToString("dd/MMM/yyyy");
                Label lblBusiness = item.FindControl("lblBusiness") as Label;
                dr["Business"] = lblBusiness.Text.ToString();
                //Label lblPendency = item.FindControl("lblPendency") as Label;
                //dr["Pendency"] = lblPendency.Text.ToString();
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
                            // sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
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
            Response.AddHeader("content-disposition", "attachment;filename=TotalNewPartyDetails.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
            //dv.Dispose();
            //udtNew.Dispose();
        }


        protected void txtfmDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                this.fillRepeter();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }
        }

        protected void txttodate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                this.fillRepeter();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch (Exception ex)
            { ex.ToString(); }
        }
    }
}