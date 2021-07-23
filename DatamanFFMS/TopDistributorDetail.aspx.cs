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
    public partial class TopDistributorDetail : System.Web.UI.Page
    {
        string Area1 = string.Empty;
        string Distributor = string.Empty;
        string From1 = string.Empty;
        string To1 = string.Empty;
        string Sm1 = string.Empty;
        string outputSt1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["AreaId"] != null)
                {
                    Area1 = Request.QueryString["AreaId"];
                }
                if (Request.QueryString["Distributor_Id"] != null)
                {
                    Distributor = Request.QueryString["Distributor_Id"];
                }
                if (Request.QueryString["FromDate"] != null)
                {
                    From1 = Request.QueryString["FromDate"];
                }
                if (Request.QueryString["ToDate"] != null)
                {
                    To1 = Request.QueryString["ToDate"];
                }
                if (Request.QueryString["smidstr"] != null)
                {
                    Sm1 = Request.QueryString["smidstr"];
                }
                // Sm = Sm.TrimStart(',').TrimEnd(',');
                if (!string.IsNullOrEmpty(Sm1))
                {
                    outputSt1 = Sm1.Trim('\'');
                    lblfmDate.Text = From1.ToString();
                    lbltodate.Text = To1.ToString();
                }

                fillRepeter();
                fillRepeter1();
            }
        }

        private void fillRepeter()
        {
            string sql = string.Empty;

            sql = " SELECT TOP 99 i.ItemName [Product],i.ItemId, convert(int,sum(os.Qty)) [Qty],convert(int,sum(os.rate)) [rate],convert(decimal, sum(os.Qty * os.Rate)) [Amount]  FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId where os.VDate>='" + From1 + "' and os.VDate<='" + To1 + "' AND os.AreaId='" + Area1 + "' and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputSt1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate Order by sum(os.Qty * os.Rate) DESC";

            // sql = "SELECT TOP 99 convert(int,sum(os.Qty)) AS qty, convert(decimal,sum(os.Qty*os.Rate)) AS amount,mp.PartyName FROM TransOrder1 os LEFT JOIN mastparty mp ON os.PartyId=mp.PartyId WHERE ItemId='" + item + "' and os.VDate>='" + From + "' and os.VDate<='" + To + "' AND mp.PartyDist=0 and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputStr + ")) and Active=1)  GROUP BY mp.PartyName ORDER BY sum(os.Qty*os.Rate) desc";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            TopDistributor.DataSource = dt;
            TopDistributor.DataBind();

            dt.Dispose();
        }

        private void fillRepeter1()
        {

            string mastPartyQry1 = string.Empty;

            mastPartyQry1 = @"select PartyId,PartyName from mastparty where partydist=1 and partyid in (" + Distributor + ") and Active=1 order by PartyName";
            // mastItemQry1 = @"SELECT mastitem.ItemName, TransOrder1.ItemId
            //FROM mastitem LEFT JOIN TransOrder1 ON mastitem.ItemId = TransOrder1.ItemId  where TransOrder1.ItemId=210 ORDER BY mastitem.ItemName";


            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, mastPartyQry1);
            lbldist.Text = dt.Rows[0][1].ToString();

            dt.Dispose();
        }

        protected void btnExport(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TopDistributorDetail.csv");
            string headerprevious = "DistributerName: " + lbldist.Text + "" + "," + "FromDate:" + lblfmDate.Text + "" + "," + "ToDate:" + lbltodate.Text + "";

            string headertext = "Product".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "rate".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headerprevious);
            sb.Append(Environment.NewLine);
            sb.Append(headertext);
            sb.Append(Environment.NewLine);
            //sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("ItemId", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("rate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));


            foreach (RepeaterItem item in TopDistributor.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblProduct = item.FindControl("lblProduct") as Label;
                dr["Product"] = lblProduct.Text.ToString();
                // Label lblItemId = item.FindControl("lblItemId") as Label;
                // dr["ItemId"] = lblItemId.Text.ToString();
                Label lblQty = item.FindControl("lblQty") as Label;
                dr["Qty"] = lblQty.Text.ToString();
                Label lblrate = item.FindControl("lblrate") as Label;
                dr["rate"] = lblrate.Text.ToString();
                Label lblAmount = item.FindControl("lblAmount") as Label;
                dr["Amount"] = lblAmount.Text.ToString();


                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "Product desc";
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

                sb.Clear();
                udtNew.Dispose();
                dtParams.Dispose();
                dv.Dispose();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

    }
}