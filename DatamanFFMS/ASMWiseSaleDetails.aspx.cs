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
using System.IO;

namespace AstralFFMS
{
    public partial class ASMWiseSaleDetails : System.Web.UI.Page
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
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

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
                outputStr = Sm.Trim('\'');
                ViewState["Fromdate"] = From;
                ViewState["Todate"] = To;
                ViewState["smid"] = smid;
                ViewState["outputStr"] = outputStr;
                lblfmDate.Text = From.ToString();
                lbltodate.Text = To.ToString(); 
               // lblfmDate.Text = Convert.ToDateTime(From).ToString("dd/MMM/yyyy");
                //lbltodate.Text = Convert.ToDateTime(To).ToString("dd/MMM/yyyy");
                fillRepeter();
                fillRepeter1();
            }
        }
        private void fillRepeter()
        {
            string sql = string.Empty;

            sql = "SELECT ProductGroup,vdate,smname,partyname,itemid,Product,a.roleid,a.smid,a.underid,convert(int,sum(Qty)) [Qty],a.[Rate],convert(decimal, sum(Amount)) [Amount] FROM (SELECT pg.ItemName [ProductGroup],os.VDate,ms.SMName,mp.PartyName, i.itemid,i.ItemName [Product],ms.RoleId,ms.SMId,ms.UnderId,convert(int,(os.Qty)) [Qty],os.Rate,convert(decimal, (os.Qty * os.Rate)) [Amount] From  TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left JOIN MastItem pg ON pg.ItemId = i.Underid LEFT JOIN MastSalesRep ms ON ms.SMId=os.smid LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId where os.VDate>='" + ViewState["Fromdate"] + "' and os.VDate<='" + ViewState["Todate"] + "' and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + ViewState["outputStr"].ToString() + ")) and Active=1) Union All  SELECT pg.ItemName [ProductGroup],os.VDate,ms.SMName,mp.PartyName,i.itemid,i.ItemName [Product],ms.RoleId,ms.SMId,ms.UnderId,convert(int,(os.Qty)) [Qty],os.Rate,convert(decimal, (os.Qty * os.Rate)) [Amount] From Temp_TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId left JOIN MastItem pg ON pg.ItemId = i.Underid LEFT JOIN MastSalesRep ms ON ms.SMId=os.smid LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId where os.VDate>='" + ViewState["Fromdate"] + "' and os.VDate<='" + ViewState["Todate"] + "' and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + ViewState["outputStr"].ToString() + ")) and Active=1) ) a  WHERE UnderId=" + ViewState["smid"] + " GROUP BY a.ItemId, a.ProductGroup,a.[Rate], a.Product,a.roleid,a.smid,a.underid,vdate,smname,partyname order by vdate asc";
                  
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            Topsaleproduct.DataSource = dt;
            Topsaleproduct.DataBind();
        }

        private void fillRepeter1()
        {          
            String str = @"select smid,SMName from Mastsalesrep where SMId in (" + smid + ") and Active=1";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            lblSalesperson.Text = dt.Rows[0][1].ToString();
        }


        protected void btnExport(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=ASMWiseSecondarySalesDetails.csv");       

            string headertext = "Order Date".TrimStart('"').TrimEnd('"') + "," + "Sales Person Name".TrimStart('"').TrimEnd('"') + "," + "Retailer Name".TrimStart('"').TrimEnd('"') + "," + "Product".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + "," + "Quantity".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
          //  sb.Append(headerprevious);
           // sb.Append(Environment.NewLine);
            sb.Append(headertext);
            sb.Append(Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("VDate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("SMName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("ItemName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("rate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("amount", typeof(String)));

            foreach (RepeaterItem item in Topsaleproduct.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblvdate = item.FindControl("lblvdate") as Label;
                dr["VDate"] = lblvdate.Text.ToString();
                Label lblsmname = item.FindControl("lblsmname") as Label;
                dr["SMName"] = lblsmname.Text.ToString();
                Label lblpartyname = item.FindControl("lblpartyname") as Label;
                dr["PartyName"] = lblpartyname.Text.ToString();
                Label lblitemname = item.FindControl("lblitemname") as Label;
                dr["ItemName"] = lblitemname.Text.ToString();
                Label lblrate = item.FindControl("lblrate") as Label;
                dr["rate"] = lblrate.Text.ToString();
                Label lblqty = item.FindControl("lblqty") as Label;
                dr["qty"] = lblqty.Text.ToString();
                Label lblamount = item.FindControl("lblamount") as Label;
                dr["amount"] = lblamount.Text.ToString();


                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "VDate desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[7];
            try
            {
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
                                if (k == 5 || k == 6 || k == 7)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
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
                                if (k == 5 || k == 6 || k == 7)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
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
                                //Total For Columns
                                if (k == 5 || k == 6 || k == 7)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                                //End
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                string totalStr = string.Empty;
                for (int x = 1; x < totalVal.Count(); x++)
                {
                    if (totalVal[x] == 0)
                    {
                        // totalStr += "0" + ',';
                    }

                    else
                    {
                        if (x == 1)
                        {
                            totalStr += "" + ',';
                        }
                        else
                        {
                            totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                        }

                    }
                }
                sb.Append(",,,,Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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