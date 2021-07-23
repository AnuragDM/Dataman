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
    public partial class TopProductGroupDetails : System.Web.UI.Page
    {
        string item = string.Empty;
        string From = string.Empty;
        string To = string.Empty;
        string Sm = string.Empty;
        // string ItemName = string.Empty;
        string outputStr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["ItemId"] != null)
                {
                    item = Request.QueryString["ItemId"];
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
                lblfmDate.Text = From.ToString();
                lbltodate.Text = To.ToString();
                // lblprodct.Text = item.ToString();
                fillRepeter();
                fillRepeter1();
            }
        }
        private void fillRepeter()
        {
            string sql = string.Empty;


            //sql = "SELECT TOP 99 convert(int,sum(os.Qty)) AS qty, convert(int,sum(os.rate)) AS rate,convert(decimal,sum(os.Qty*os.Rate)) AS amount,mp.PartyName FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId WHERE ItemId='" + item + "' and os.VDate>='" + From + "' and os.VDate<='" + To + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputStr + ")) and Active=1)  GROUP BY mp.PartyName,rate ORDER BY sum(os.Qty*os.Rate) desc";
            // Modification Dashboard 23/09/2019

            //sql = "SELECT convert(int,sum(os.Qty)) AS qty, convert(int,sum(os.rate)) AS rate,convert(decimal,convert(int,sum(os.Qty))*convert(int,sum(os.rate))) AS amount,mp.PartyName FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId LEFT JOIN mastitem pg ON pg.ItemId=mi.Underid WHERE pg.ItemId='" + item + "' and os.VDate>='" + From + "' and os.VDate<='" + To + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputStr + ")) and Active=1)  GROUP BY mp.PartyName,rate Union ALl SELECT convert(int,sum(os.Qty)) AS qty, convert(int,sum(os.rate)) AS rate,convert(decimal,convert(int,sum(os.Qty))*convert(int,sum(os.rate))) AS amount,mp.PartyName FROM Temp_TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId LEFT JOIN mastitem pg ON pg.ItemId=mi.Underid WHERE pg.ItemId='" + item + "' and os.VDate>='" + From + "' and os.VDate<='" + To + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputStr + ")) and Active=1) GROUP BY mp.PartyName,rate";

            sql = "SELECT convert(int,sum(os.Qty)) AS qty, sum(os.rate) AS rate,convert(Numeric(38, 2),convert(int,sum(os.Qty))*sum(os.rate)) AS amount,mp.PartyName FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId LEFT JOIN mastitem pg ON pg.ItemId=mi.Underid WHERE pg.ItemId='" + item + "' and os.VDate>='" + From + "' and os.VDate<='" + To + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputStr + ")) and Active=1)  GROUP BY mp.PartyName,rate Union ALl SELECT convert(int,sum(os.Qty)) AS qty, sum(os.rate) AS rate,convert(Numeric(38, 2),convert(int,sum(os.Qty))*sum(os.rate)) AS amount,mp.PartyName FROM Temp_TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId LEFT JOIN mastitem mi ON os.ItemId=mi.ItemId LEFT JOIN mastitem pg ON pg.ItemId=mi.Underid WHERE pg.ItemId='" + item + "' and os.VDate>='" + From + "' and os.VDate<='" + To + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputStr + ")) and Active=1) GROUP BY mp.PartyName,rate";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            Topsaleproduct.DataSource = dt;
            Topsaleproduct.DataBind();
        }

        private void fillRepeter1()
        {

            string mastItemQry1 = string.Empty;

            mastItemQry1 = @"select ItemId,ItemName from mastitem where ItemId in (" + item + ") and Active=1 order by itemname";
            // mastItemQry1 = @"SELECT mastitem.ItemName, TransOrder1.ItemId
            //FROM mastitem LEFT JOIN TransOrder1 ON mastitem.ItemId = TransOrder1.ItemId  where TransOrder1.ItemId=210 ORDER BY mastitem.ItemName";


            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
            lblprodct.Text = dt.Rows[0][1].ToString();


            //Topsaleproduct.DataSource = dt;
            //Topsaleproduct.DataBind();
        }


        protected void btnExport(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TopProductDetail.csv");
            string headerprevious = "ReportForProduct: " + lblprodct.Text + "" + "," + "FromDate:" + lblfmDate.Text + "" + "," + "ToDate:" + lbltodate.Text + "";

            string headertext = "PartyName".TrimStart('"').TrimEnd('"') + "," + "Quantity".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headerprevious);
            sb.Append(Environment.NewLine);
            sb.Append(headertext);
            sb.Append(Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
            dtParams.Columns.Add(new DataColumn("qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("rate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("amount", typeof(String)));

            foreach (RepeaterItem item in Topsaleproduct.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblProductparty = item.FindControl("lblProductparty") as Label;
                dr["PartyName"] = lblProductparty.Text.ToString();
                Label lblqty = item.FindControl("lblqty") as Label;
                dr["qty"] = lblqty.Text.ToString();
                Label lblrate = item.FindControl("lblrate") as Label;
                dr["rate"] = lblrate.Text.ToString();
                Label lblamount = item.FindControl("lblamount") as Label;
                dr["amount"] = lblamount.Text.ToString();




                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "PartyName desc";
            DataTable udtNew = dv.ToTable();
            decimal[] totalVal = new decimal[4];
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
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                if (k == 1 || k == 2 || k == 3 || k == 4)
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
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                if (k == 1 || k == 2 || k == 3 || k == 4)
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
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                                //Total For Columns
                                if (k == 1 || k == 2 || k == 3 || k == 4)
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
                        totalStr += "0" + ',';
                    }

                    else
                    {
                        if (x == 2)
                        {
                            totalStr += "" + ',';
                        }
                        else
                        {
                            totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                        }

                    }
                }
                sb.Append("Total," + totalStr);
                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}


