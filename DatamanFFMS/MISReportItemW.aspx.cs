using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.Services;
using DAL;
using System.IO;
using System.Text;

namespace AstralFFMS
{
    public partial class MISReportItemW : System.Web.UI.Page
    {
        int uid = 0;
        int smID = 0;
        string pageName = string.Empty;
        string pageName1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            pageName = string.Empty;

            if (Request.QueryString["FromDate"] != null && Request.QueryString["ToDate"] != null && Request.QueryString["PartyId"] != null && Request.QueryString["Type"] != null)
            {
                dateHiddenField.Value = Request.QueryString["FromDate"];
                dateHiddenField1.Value = Request.QueryString["ToDate"];
                smIDHiddenField.Value = Request.QueryString["PartyId"];

                string mastPartyQry1 = @"select PartyId,PartyName from mastparty where partyid in (" + smIDHiddenField.Value + ")";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, mastPartyQry1);
                lbldist.Text = dt.Rows[0][1].ToString();

                GetDailyWorkingReport(dateHiddenField.Value, dateHiddenField1.Value, smIDHiddenField.Value, Convert.ToString(Request.QueryString["Type"]), Request.QueryString["SaleType"]);

            }
        }

        public void GetDailyWorkingReport(string fromdate, string toDate, string PartyId, string Type, string SaleType)
        {
            string data = string.Empty, _search = "", Query = "", _areaId = "";
            DataTable dtLocTraRep = new DataTable();
            string DistIdStr1 = "", PartyIdStr1 = "", ProdctGrp = "", Product = "", AreaId = "";
            AreaId = Convert.ToString(Session["AreaId"]);
            DistIdStr1 = Convert.ToString(Session["DistIdStr1"]);
            PartyIdStr1 = Convert.ToString(Session["PartyIdStr1"]);
            ProdctGrp = Convert.ToString(Session["ProdctGrp"]);
            Product = Convert.ToString(Session["Product"]);


            if (Type == "S")
            {
                _areaId = @" Select Distinct AreaId from ViewGeo where StateId IN(" + AreaId + ")";
            }
            else if (Type == "C") _areaId = @" Select Distinct AreaId from ViewGeo where CityId IN(" + AreaId + ")";
            else _areaId = AreaId;
            if (SaleType == "P")
            {
                if (!string.IsNullOrEmpty(DistIdStr1))
                {
                    _search = " And ti.DistId in (" + DistIdStr1 + ")";
                }
            }
            if (SaleType == "S")
            {
                if (!string.IsNullOrEmpty(PartyIdStr1))
                {
                    _search = " And ti.PartyId in (" + PartyIdStr1 + ")";
                }
            }
            if (!string.IsNullOrEmpty(ProdctGrp) && string.IsNullOrEmpty(Product))
            {
                _search = " And ti.ItemId in (Select ItemId from mastItem where Underid in (" + ProdctGrp + "))";
            }
            if (!string.IsNullOrEmpty(Product))
            {
                _search = " And ti.ItemId in (" + Product + ")";
            }
            try
            {
                //                Query = @"select ti.PartyId,mp.PartyName,max(mp.Mobile) as Mobile,max(mp.Address+mp.Address1+mp.Address2) as Address,Sum(ti1.Qty*ti1.Rate) as OrderAmount from TransOrder ti
                //                            inner join MastParty mp on ti.PartyId=mp.PartyId
                //                            inner join TransOrder1 ti1 on ti.OrdId=ti1.OrdId 
                //                            where ti.AreaId IN(" + _areaId + ") and ti.VDate>='" + fromdate + "' and ti.VDate<='" + toDate + "' group by ti.PartyId,mp.PartyName";
                if (SaleType == "S")
                {
                    Query = @"Select ti.PartyId,mi.ItemName,Sum(ti.Qty) as Qty,max(ti.Rate) as Rate,sum(ti.Qty*ti.Rate) as OrderAmount,ti.ItemId from TransOrder1 ti
inner join MastItem mi on ti.ItemId=mi.ItemId where ti.partyid IN (" + PartyId + ") and ti.VDate>='" + fromdate + "' and ti.VDate<='" + toDate + "' " + _search + " group by ti.PartyId,mi.ItemName,ti.ItemId order by mi.ItemName";
                }
                else if (SaleType == "P")
                {
                    Query = @"Select ti.DistId as PartyId,mi.ItemName,Sum(ti.Qty) as Qty,max(ti.Rate) as Rate,sum(ti.Qty*ti.Rate) as OrderAmount,ti.ItemId  from TransDistInv1 ti inner join MastItem mi on ti.ItemId=mi.ItemId where ti.distid IN (" + PartyId + ") and ti.VDate>='" + fromdate + "' and ti.VDate<='" + toDate + "' " + _search + " group by ti.DistId,mi.ItemName,ti.ItemId order by mi.ItemName";

                }
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dt.Rows.Count > 0)
                {
                    //var newDt = dt.AsEnumerable()
                    // .GroupBy(r => new { Col1 = r["PartyId"], Col2 = r["ItemName"], Col3 = r["Mobile"], Col4 = r["Address"] })
                    // .Select(g =>
                    // {
                    //     var row = dt.NewRow();

                    //     row["PartyId"] = g.Key.Col1;
                    //     row["PartyName"] = g.Key.Col2;
                    //     row["Mobile"] = g.Key.Col3;
                    //     row["Address"] = g.Key.Col4;
                    //     row["OrderAmount"] = g.Sum(r => r.Field<decimal>("OrderAmount")).ToString();
                    //     //row["Amount 2"] = g.Sum(r => r.Field<int>("Amount 2"));
                    //     //row["Amount 3"] = g.Sum(r => r.Field<int>("Amount 3"));

                    //     return row;
                    // }).CopyToDataTable();

                    rpt.DataSource = dt;
                    rpt.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                    btnExport.Visible = false;
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        private string GetPartyName(int smID)
        {
            string salesrepqry1 = @"select PartyName from MastParty where PartyId=" + smID + "";
            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry1);
            if (dtsalesrepqry1.Rows.Count > 0)
            {
                return Convert.ToString(dtsalesrepqry1.Rows[0]["PartyName"]);
            }
            else
            {
                return string.Empty;
            }
        }

        private int GetSalesPerId(string p)
        {
            string salesrepqry = @"select SMId from TransVisit where VisId=" + Convert.ToInt64(p) + "";
            DataTable dtsalesrepqry = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry);
            if (dtsalesrepqry.Rows.Count > 0)
            {
                return Convert.ToInt32(dtsalesrepqry.Rows[0]["SMId"]);
            }
            else
            {
                return 0;
            }

        }

        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (pageName == "APPROVAL-L1")
            {
                Response.Redirect("~/RptDailyWorkingApprovalL1.aspx");
            }
            else
            {
                Response.Redirect("~/RptDailyWorkingSummaryL1.aspx");
            }

            //    Response.Redirect("~/RptDailyWorkingSummaryL1.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=MISReportItem_wise.csv");
            string headerprevious = "Party/Distributor: " + lbldist.Text + "";

            string headertext = "Item".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headerprevious);
            sb.Append(Environment.NewLine);
            sb.Append(headertext);
            sb.Append(Environment.NewLine);

            string dataText = string.Empty;

            DataTable dtParams = new DataTable();
            dtParams.Columns.Add(new DataColumn("Item", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            dtParams.Columns.Add(new DataColumn("Rate", typeof(String)));
            dtParams.Columns.Add(new DataColumn("OrderAmount", typeof(String)));

            foreach (RepeaterItem item in rpt.Items)
            {
                DataRow dr = dtParams.NewRow();
                Label lblItemName = item.FindControl("lblItemName") as Label;
                dr["Item"] = lblItemName.Text.ToString();
                Label lblQty = item.FindControl("lblQty") as Label;
                dr["Qty"] = lblQty.Text.ToString();
                Label lblRate = item.FindControl("lblRate") as Label;
                dr["Rate"] = lblRate.Text.ToString();
                Label lblOrderAmount = item.FindControl("lblOrderAmount") as Label;
                dr["OrderAmount"] = lblOrderAmount.Text.ToString();

                dtParams.Rows.Add(dr);
            }
            DataView dv = dtParams.DefaultView;
            dv.Sort = "Item asc";
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
                                if (k == 1 || k == 3)
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
                                if (k == 1 || k == 3)
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
                                if (k == 1 || k == 3)
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
                        //totalStr += "0" + ',';
                        totalStr += "" + ',';
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
                Response.End();

                sb.Clear();
                dtParams.Dispose();
                dv.Dispose();
                udtNew.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=DSRReportPW.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //rpt.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
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
                HiddenField sTypeHdf = (HiddenField)item.FindControl("sTypeHdf");
                if (sTypeHdf.Value == "Distributor Discussion" || sTypeHdf.Value == "Competitor" || sTypeHdf.Value == "Demo")
                {
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
    }
}