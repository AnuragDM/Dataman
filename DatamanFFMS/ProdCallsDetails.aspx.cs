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
    public partial class Prod_Calls_Details : System.Web.UI.Page
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
                lblformDate.Text = From;
                lbltdate.Text = To;
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

           

            Qty1 = Qty1 + " and p.Created_User_id in ( SELECT userid FROM MastSalesRep ms LEFT JOIN mastlogin ml ON ms.UserId=ml.Id WHERE ms.SMId IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smid + ")) and Active=1)";

            string query = @"SELECT a.smid,a.vdate,a.Visid,a.partyid,a.beatid,a.SMName,a.PartyName,a.Address1,a.Mobile FROM (SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM TransOrder om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId UNION ALL SELECT om.SMID,om.VDate,om.VisId, om.PartyId,P.BeatId,ms.SMName,p.PartyName,p.Address1,p.Mobile FROM Temp_TransOrder om LEFT JOIN mastparty p ON p.PartyId=om.PartyId LEFT JOIN mastsalesrep ms ON om.SMId=ms.SMId)a LEFT JOIN mastsalesrep ms ON a.smid=ms.SMId WHERE a.VDate>='" + From + " 00:00' and VDate<='" + To + " 23:59' AND a.smid IN (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smid + ")) and Active=1)";


            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            Topprodcalls.DataSource = dt;
            Topprodcalls.DataBind();

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
            Response.AddHeader("content-disposition", "attachment;filename=ProdCallsDetails.csv");
            string headertext = "Retailer".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Visited Sales Person".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertext);
            sb.AppendLine(System.Environment.NewLine);


            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Address1", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SMName", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("Pendency", typeof(String)));

            foreach (RepeaterItem item in Topprodcalls.Items)
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
            Response.AddHeader("content-disposition", "attachment;filename=ProdCallsDetails.csv");
            Response.Write(sb.ToString());
            Response.End();

            sb.Clear();
            dtParams.Dispose();
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