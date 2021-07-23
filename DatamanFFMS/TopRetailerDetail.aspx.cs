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


    public partial class TopRetailerDetail : System.Web.UI.Page
    {
        string PartyId = string.Empty;
        string FromDate = string.Empty;
        string ToDate = string.Empty;
        string smidstr = string.Empty;
        string outputSt1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //divrepeater1.Style.Add("display", "none");
                //divrepeater.Style.Add("display", "block");
                if (Request.QueryString["PartyId"] != null)
                {
                    PartyId = Request.QueryString["PartyId"];
                }
                if (Request.QueryString["FromDate"] != null)
                {
                    FromDate = Request.QueryString["FromDate"];
                }
                if (Request.QueryString["ToDate"] != null)
                {
                    ToDate = Request.QueryString["ToDate"];
                }
                if (Request.QueryString["smidstr"] != null)
                {
                    smidstr = Request.QueryString["smidstr"];
                }
                // Sm = Sm.TrimStart(',').TrimEnd(',');
                if (!string.IsNullOrEmpty(smidstr))
                {
                    outputSt1 = smidstr.Trim('\'');
                    lblfmDate.Text = FromDate;
                    lbltodate.Text = ToDate;
                }

                fillRepeter();
                fillRepeter1();
                fillRepete();
                // fillRepeter2();
            }

        }
        private void fillRepeter()
        {
            string sql = string.Empty;

            sql = "  SELECT TOP 99 i.ItemName [Product], convert(int,sum(os.Qty)) [Qty],convert(decimal, sum(os.Qty * os.Rate))/convert(int,sum(os.Qty)) as  [rate],convert(decimal, sum(os.Qty * os.Rate)) [Amount] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId where os.VDate>='" + Settings.dateformat(FromDate) + "' and os.VDate<='" + Settings.dateformat(ToDate) + "' AND os.PartyId='" + PartyId + "' and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputSt1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate Order by sum(os.Qty * os.Rate) DESC";


            // sql = " SELECT TOP 99 i.ItemName [Product],i.ItemId, convert(int,sum(os.Qty)) [Qty],convert(decimal, sum(os.Qty * os.Rate)) [Amount]  FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId where os.VDate>='" + From1 + "' and os.VDate<='" + To1 + "' AND os.AreaId='" + Party1 + "' and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputSt1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName Order by sum(os.Qty * os.Rate) DESC";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            Topsaleproduct.DataSource = dt;
            Topsaleproduct.DataBind();
            dt.Dispose();
        }

        private void fillRepeter1()
        {

            string mastPartyQry1 = string.Empty;

            mastPartyQry1 = @"select PartyId,PartyName from mastparty where partydist=0 and partyid in (" + PartyId + ") and Active=1 order by PartyName";
            // mastItemQry1 = @"SELECT mastitem.ItemName, TransOrder1.ItemId
            //FROM mastitem LEFT JOIN TransOrder1 ON mastitem.ItemId = TransOrder1.ItemId  where TransOrder1.ItemId=210 ORDER BY mastitem.ItemName";


            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, mastPartyQry1);
            lbldist.Text = dt.Rows[0][1].ToString();

            dt.Dispose();
        }

        private void fillRepete()
        {
            string sql = string.Empty;

            sql = "  SELECT TOP 99 i.ItemName [Product], convert(int,sum(os.Qty)) [Qty],convert(int,sum(os.rate)) [rate],convert(decimal, sum(os.Qty * os.Rate)) [Amount], convert(date, (os.VDate)) [VDate] FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId where os.VDate>='" + Settings.dateformat(FromDate) + "' and os.VDate<='" + Settings.dateformat(ToDate) + "' AND os.PartyId='" + PartyId + "' and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputSt1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName,rate,vdate Order by os.VDate DESC";

            // sql = " SELECT TOP 99 i.ItemName [Product],i.ItemId, convert(int,sum(os.Qty)) [Qty],convert(decimal, sum(os.Qty * os.Rate)) [Amount]  FROM TransOrder1 os left JOIN MastItem i ON os.ItemId = i.ItemId where os.VDate>='" + From1 + "' and os.VDate<='" + To1 + "' AND os.AreaId='" + Party1 + "' and os.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + outputSt1 + ")) and Active=1) GROUP BY i.ItemId, i.ItemName Order by sum(os.Qty * os.Rate) DESC";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
            dt.Dispose();
        }

        //private void fillRepeter2()
        //{

        //    string mastPartyQry1 = string.Empty;

        //    mastPartyQry1 = @"select PartyId,PartyName from mastparty where partydist=0 and partyid in (" + PartyId + ") and Active=1 order by PartyName";
        //    // mastItemQry1 = @"SELECT mastitem.ItemName, TransOrder1.ItemId
        //    //FROM mastitem LEFT JOIN TransOrder1 ON mastitem.ItemId = TransOrder1.ItemId  where TransOrder1.ItemId=210 ORDER BY mastitem.ItemName";


        //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, mastPartyQry1);
        //    lbldist.Text = dt.Rows[0][1].ToString();


        //}


        protected void btnExport(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TopRetailerDetail.csv");
            string headerprevious = "RetailerName: " + lbldist.Text + "" + "," + "FromDate:" + lblfmDate.Text + "" + "," + "ToDate:" + lbltodate.Text + "";

            string headertext = "Product".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Avg.rate".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"');
            //string headerprevious = "RetailerName: '" + lbldist.Text + "'" + "," + "FromDate:'" + lblfmDate.Text + "'" + "," + "ToDate:'" + lbltodate.Text + "'";
            StringBuilder sb = new StringBuilder();
            sb.Append(headerprevious);
            sb.Append(Environment.NewLine);
            sb.Append(headertext);
            sb.Append(Environment.NewLine);
            // sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("rate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));


            foreach (RepeaterItem item in Topsaleproduct.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblProduct = item.FindControl("lblProduct") as Label;
                dr["Product"] = lblProduct.Text.ToString();
                Label lblqty = item.FindControl("lblqty") as Label;
                dr["Qty"] = lblqty.Text.ToString();
                Label lblrate = item.FindControl("lblrate") as Label;
                dr["rate"] = lblrate.Text.ToString();
                Label lblamount = item.FindControl("lblamount") as Label;
                dr["Amount"] = lblamount.Text.ToString();



                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            // dv.Sort = "Product desc";
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
                                if (k == 1 || k == 2 || k == 3 || k == 4)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
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

                dtParams.Dispose();
                udtNew.Dispose();
                dv.Dispose();
                sb.Clear();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=RepeaterExport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //Topsaleproduct.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
        }

        protected void Button1(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=TopRetailerDetail.csv");
            string headerprevious = "RetailerName: " + lbldist.Text + "" + "," + "FromDate:" + lblfmDate.Text + "" + "," + "ToDate:" + lbltodate.Text + "";

            string headertext = "Product".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "rate".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"') + "," + "Date Wise".TrimStart('"').TrimEnd('"');
            //string headerprevious = "RetailerName: '" + lbldist.Text + "'" + "," + "FromDate:'" + lblfmDate.Text + "'" + "," + "ToDate:'" + lbltodate.Text + "'";
            StringBuilder sb = new StringBuilder();
            sb.Append(headerprevious);
            sb.Append(Environment.NewLine);
            sb.Append(headertext);
            sb.Append(Environment.NewLine);
            // sb.AppendLine(System.Environment.NewLine);
            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Product", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("rate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));
            dtParams.Columns.Add(new DataColumn("VDate", typeof(String)));

            foreach (RepeaterItem item in Repeater1.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblProduct = item.FindControl("lblProduct") as Label;
                dr["Product"] = lblProduct.Text.ToString();
                Label lblqty = item.FindControl("lblqty") as Label;
                dr["Qty"] = lblqty.Text.ToString();
                Label lblrate = item.FindControl("lblrate") as Label;
                dr["rate"] = lblrate.Text.ToString();
                Label lblamount = item.FindControl("lblamount") as Label;
                dr["Amount"] = lblamount.Text.ToString();
                Label lblvdate = item.FindControl("lblvdate") as Label;
                dr["VDate"] = lblvdate.Text.ToString();


                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            //dv.Sort = "VDate desc";
            dtParams = dv.ToTable();
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
                            if (k == 4)
                            {
                                //sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                                sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                if (k == 1 || k == 2 || k == 3)
                                {
                                    if (udtNew.Rows[j][k].ToString() != "")
                                    {
                                        totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                    }
                                }
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                if (k == 1 || k == 2 || k == 3)
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
                            if (k == 4)
                            {
                                //sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                                sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 4)
                            {
                                //sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                                sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                                //Total For Columns
                                if (k == 1 || k == 2 || k == 3)
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

                dtParams.Dispose();
                udtNew.Dispose();
                dv.Dispose();
                sb.Clear();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void chkItemWise_CheckedChanged(object sender, EventArgs e)
        {
            chkDatewise.Checked = false;
            divrepeater.Style.Add("display", "block");
            divrepeater1.Style.Add("display", "none");
        }

        protected void chkDatewise_CheckedChanged(object sender, EventArgs e)
        {
            chkItemWise.Checked = false;
            divrepeater.Style.Add("display", "none");
            divrepeater1.Style.Add("display", "block");
        }
    }
}
