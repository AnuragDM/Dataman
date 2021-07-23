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
using System.Web.UI.HtmlControls;

namespace AstralFFMS
{
    public partial class MISReportPW : System.Web.UI.Page
    {
        int uid = 0;
        int smID = 0;
        string pageName = string.Empty;
        string pageName1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            pageName = string.Empty;
            //if (Request.QueryString["Page"] != null)
            //{
            //    pageName = Request.QueryString["Page"];
            //}

            //if (!IsPostBack)
            //{
            //    currDateLabel.Text = GetUTCTime().ToString("dd/MMM/yyyy");                
            //}
            if (!IsPostBack)
            {
                if (Request.QueryString["FromDate"] != null && Request.QueryString["ToDate"] != null && Request.QueryString["AreaId"] != null && Request.QueryString["Type"] != null)
                {
                    dateHiddenField.Value = Request.QueryString["FromDate"];
                    dateHiddenField1.Value = Request.QueryString["ToDate"];
                    smIDHiddenField.Value = Request.QueryString["AreaId"];
                    //smID = Convert.ToInt32(Request.QueryString["AreaId"]);//GetSalesPerId(vistIDHiddenField.Value);
                    //saleRepName.Text = GetPartyName(smID);
                    string DistIdStr1 = Convert.ToString(Session["DistIdStr1"]);
                    string PartyIdStr1 = Convert.ToString(Session["PartyIdStr1"]);
                    string ProdctGrp = Convert.ToString(Session["ProdctGrp"]);
                    string Product = Convert.ToString(Session["Product"]);
                    GetDailyWorkingReport(dateHiddenField.Value, dateHiddenField1.Value, smIDHiddenField.Value, Convert.ToString(Request.QueryString["Type"]), Request.QueryString["SaleType"], DistIdStr1, PartyIdStr1, ProdctGrp, Product);

                }
            }
        }

        public void GetDailyWorkingReport(string fromdate, string toDate, string AreaId, string Type, string SaleType, string DistIdStr1, string PartyIdStr1, string ProdctGrp, string Product)
        {
            string data = string.Empty, _search = "", Query = "", _areaId = "", _areaId1 = "", _search1 = "";
            DataTable dtLocTraRep = new DataTable();
            DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');
            PartyIdStr1 = PartyIdStr1.TrimStart(',').TrimEnd(',');
            ProdctGrp = ProdctGrp.TrimStart(',').TrimEnd(',');
            Product = Product.TrimStart(',').TrimEnd(',');

            Session["SaleType"] = SaleType;
            Session["fromdate"] = fromdate;
            Session["toDate"] = toDate;
            Session["AreaId"] = AreaId;
            Session["Type"] = Type;
            if (Type == "S")
            {
                if (SaleType == "P")
                {
                    _areaId = @" and mp.areaid in (select areaid from mastarea where underid in (select areaid from mastarea where underid in (select areaid from mastarea where underid in (" + AreaId + "))))";
                }
                else
                {
                    //_areaId = @" Select Distinct AreaId from ViewGeo where StateId IN(" + AreaId + ")";
                    _areaId1 = @" and mp.areaid in (select areaid from mastarea where underid in (select areaid from mastarea where underid in (select areaid from mastarea where underid in (" + AreaId + "))))";
                    _areaId = @" and ti.areaid in (select areaid from mastarea where underid in (select areaid from mastarea where underid in (select areaid from mastarea where underid in (" + AreaId + "))))";
                }

                //_areaId = AreaId;
            }
            else if (Type == "C")
            {
                if (SaleType == "P")
                {
                    _areaId = @" and mp.areaid in (select areaid from mastarea where underid in (select areaid from mastarea where areaid in (" + AreaId + ")))";
                }
                else
                {
                    //_areaId = @" Select Distinct AreaId from ViewGeo where CityId IN(" + AreaId + ")";
                    _areaId = @" and ti.areaid in (select areaid from mastarea where underid in (select areaid from mastarea where areaid in (" + AreaId + ")))";
                    _areaId1 = @" and mp.areaid in (select areaid from mastarea where underid in (select areaid from mastarea where areaid in (" + AreaId + ")))";
                    //  _areaId = AreaId;
                }
            }
            else
            {
                if (SaleType == "P")
                {
                    _areaId = @" and mp.areaid in (" + AreaId + ")";
                }
                else
                {
                    //_areaId = AreaId;

                    _areaId = @" and ti.areaid in (" + AreaId + ")";
                    _areaId1 = @" and mp.areaid in (" + AreaId + ")";
                }


            }

            if (SaleType == "P")
            {
                if (!string.IsNullOrEmpty(DistIdStr1))
                {
                    _search += " And ti.DistId in (" + DistIdStr1 + ")";
                }
            }
            if (SaleType == "S")
            {
                if (!string.IsNullOrEmpty(PartyIdStr1))
                {
                    _search += " And ti.PartyId in (" + PartyIdStr1 + ")";
                    _search1 = @" and mp.PartyId in (" + PartyIdStr1 + ")";
                }
            }
            if (!string.IsNullOrEmpty(ProdctGrp) && string.IsNullOrEmpty(Product))
            {
                _search += " And ti.ItemId in (Select ItemId from mastItem where Underid in (" + ProdctGrp + "))";
            }
            if (!string.IsNullOrEmpty(Product))
            {
                _search += " And ti.ItemId in (" + Product + ")";
            }
            try
            {
                if (SaleType == "S")
                {
                    //Query = @"select  mp.PartyId,mp.PartyName,isnull(mp1.PartyName,'') as Distributor,mp.Mobile,mp.Address1 as Address,isnull(sum(ti.Qty*ti.Rate),0) as OrderAmount from mastparty mp left join transorder1 ti on ti.partyid=mp.partyid and ti.VDate>='" + fromdate + "' and ti.VDate<='" + toDate + "' " + _areaId + " " + _search + " left join mastparty mp1 on mp1.PartyId=mp.UnderId and mp1.PartyDist=1 where mp.partydist=0 " + _areaId1 + " " + _search1 + " group by mp.PartyId,mp.PartyName,mp1.PartyName,mp.Mobile,mp.Address1 order by OrderAmount desc"; //with distributor name and all retailers      

                    Query = @"select  mp.PartyId,mp.PartyName,isnull(mp1.PartyName,'') as Distributor,mp.Mobile,mp.Address1 as Address,isnull(sum(ti.Qty*ti.Rate),0) as OrderAmount from mastparty mp left join transorder1 ti on ti.partyid=mp.partyid and ti.VDate>='" + fromdate + "' and ti.VDate<='" + toDate + "' " + _areaId + " " + _search + " left join mastparty mp1 on mp1.PartyId=ti.DistId and mp1.PartyDist=1 where mp.partydist=0 " + _areaId1 + " " + _search1 + " group by mp.PartyId,mp.PartyName,mp1.PartyName,mp.Mobile,mp.Address1 order by OrderAmount desc"; //with distributor name and all retailers       
                }
                else if (SaleType == "P")
                {
                    Query = @"Select mp.PartyId,mp.PartyName,'' as Distributor,mp.Mobile,mp.Address1 as Address,sum(ti.Qty*ti.Rate) as OrderAmount from TransDistInv1 ti
left join MastParty mp on ti.DistId=mp.PartyId left join Mastarea ma on mp.AreaId=ma.areaId where mp.partydist=1 and ti.VDate>='" + fromdate + "' and ti.VDate<='" + toDate + "' " + _areaId + " " + _search + " group by mp.PartyId,mp.PartyName,mp.Mobile,mp.Address1 order by mp.PartyName";

                }
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                if (dt.Rows.Count > 0)
                {
                    //var newDt = dt.AsEnumerable()
                    // .GroupBy(r => new { Col1 = r["PartyId"], Col2 = r["PartyName"], Col3 = r["Mobile"], Col4 = r["Address"] })
                    // .Select(g =>
                    // {
                    //     var row = dt.NewRow();

                    //     row["PartyId"] = g.Key.Col1;
                    //     row["PartyName"] = g.Key.Col2;
                    //     row["Mobile"] = g.Key.Col3;
                    //     row["Address"] = g.Key.Col4;
                    //     row["OrderAmount"] = g.Sum(r => r.Field<decimal>("OrderAmount")).ToString();                    
                    //     return row;
                    // }).CopyToDataTable();

                    //rpt.DataSource = newDt;
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
            //dtsalesrepqry1.Dispose();
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
            string SaleType = Convert.ToString(Session["SaleType"]);

            if (SaleType == "P")
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=MISReportParty/Distributor_wise.csv");

                //string info = "From Date: " + Request.QueryString["FromDate"] + ", To Date: " + Request.QueryString["ToDate"] + ", Sale Type: Primary" ;
                string AreaName = DbConnectionDAL.GetScalarValue(CommandType.Text, "select AreaName from MastArea where AreaId=" + Request.QueryString["AreaId"] + "").ToString();

                string info = "From Date: " + Convert.ToDateTime(Request.QueryString["FromDate"]).ToString("dd/MMM/yyyy") + ", To Date: " + Convert.ToDateTime(Request.QueryString["ToDate"]).ToString("dd/MMM/yyyy") + ", Sale Type: Secondary , Area: " + AreaName;
                StringBuilder sb = new StringBuilder();
                sb.Append(info);
                sb.AppendLine(System.Environment.NewLine);

                string headertext = "Party".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"');

                //StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;

                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("Party", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
                dtParams.Columns.Add(new DataColumn("OrderAmount", typeof(String)));

                foreach (RepeaterItem item in rpt.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label lblParty = item.FindControl("lblParty") as Label;
                    dr["Party"] = lblParty.Text.ToString();
                    Label lblMobile = item.FindControl("lblMobile") as Label;
                    dr["Mobile"] = lblMobile.Text.ToString();
                    Label lblAddress = item.FindControl("lblAddress") as Label;
                    dr["Address"] = lblAddress.Text.ToString();
                    Label lblOrderAmount = item.FindControl("lblOrderAmount") as Label;
                    dr["OrderAmount"] = lblOrderAmount.Text.ToString();

                    dtParams.Rows.Add(dr);
                }
                DataView dv = dtParams.DefaultView;
                dv.Sort = "Party asc";
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
                                    if (k == 3)
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
                                    if (k == 3)
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
                                    if (k == 3)
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
                    for (int x = 2; x < totalVal.Count(); x++)
                    {
                        if (totalVal[x] == 0)
                        {
                            //  totalStr += "0" + ',';
                        }
                        else
                        {
                            totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                        }
                    }
                    sb.Append(",,Total," + totalStr);
                    Response.Write(sb.ToString());
                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.End();

                    sb.Clear();
                    dtParams.Dispose();
                    udtNew.Dispose();
                    dv.Dispose();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else if (SaleType == "S")
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=MISReportParty/Distributor_wise.csv");

                string AreaName = DbConnectionDAL.GetScalarValue(CommandType.Text, "select AreaName from MastArea where AreaId=" + Request.QueryString["AreaId"] + "").ToString();

                string info = "From Date: " + Request.QueryString["FromDate"] + ", To Date: " + Request.QueryString["ToDate"] + ", Sale Type: Secondary , Area: " + AreaName;
                StringBuilder sb = new StringBuilder();
                sb.Append(info);
                sb.AppendLine(System.Environment.NewLine);


                //string headertext = "Party".TrimStart('"').TrimEnd('"') + "," + "DistributorName".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"');
                string headertext = "DistributorName".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Order Amount".TrimStart('"').TrimEnd('"');

                //StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;

                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("DistributorName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Party", typeof(String)));

                dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
                dtParams.Columns.Add(new DataColumn("OrderAmount", typeof(String)));

                foreach (RepeaterItem item in rpt.Items)
                {
                    DataRow dr = dtParams.NewRow();

                    Label lblDistributor = item.FindControl("lblDistributor") as Label;
                    dr["DistributorName"] = lblDistributor.Text.ToString();
                    Label lblParty = item.FindControl("lblParty") as Label;
                    dr["Party"] = lblParty.Text.ToString();
                    Label lblMobile = item.FindControl("lblMobile") as Label;
                    dr["Mobile"] = lblMobile.Text.ToString();
                    Label lblAddress = item.FindControl("lblAddress") as Label;
                    dr["Address"] = lblAddress.Text.ToString();
                    Label lblOrderAmount = item.FindControl("lblOrderAmount") as Label;
                    dr["OrderAmount"] = lblOrderAmount.Text.ToString();

                    dtParams.Rows.Add(dr);
                }
                DataView dv = dtParams.DefaultView;
                dv.Sort = "OrderAmount desc";
                DataTable udtNew = dv.ToTable();
                decimal[] totalVal = new decimal[5];
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
                                    if (k == 4)
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
                                    if (k == 4)
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
                                    if (k == 4)
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
                    for (int x = 3; x < totalVal.Count(); x++)
                    {
                        if (totalVal[x] == 0)
                        {
                            //  totalStr += "0" + ',';
                        }
                        else
                        {
                            totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                        }
                    }
                    sb.Append(",,,Total," + totalStr);
                    Response.Write(sb.ToString());
                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.End();

                    sb.Clear();
                    dtParams.Dispose();
                    udtNew.Dispose();
                    dv.Dispose();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
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

        //protected void rpt_ItemDataBound(object source, RepeaterCommandEventArgs e)
        protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string SaleType = Convert.ToString(Session["SaleType"]);

            if (SaleType == "P")
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    HtmlTableCell tdTableCell = (HtmlTableCell)e.Item.FindControl("tdTableCell");
                    tdTableCell.Visible = false;
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlTableCell tdTableCell1 = (HtmlTableCell)e.Item.FindControl("distname");
                    tdTableCell1.Visible = false;
                }
            }
        }
        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string DistIdStr1 = "", PartyIdStr1 = "", ProdctGrp = "", Product = "", SaleType = "", fromDate = "", todate = "", Type = "", AreaId = "";



            if (e.CommandName == "select")
            {
                HiddenField hdPartyId = (HiddenField)e.Item.FindControl("hfPartyId");


                //HiddenField hfColumn = (HiddenField)e.Item.FindControl("hfColumn");
                //HiddenField hdnDsrType = (HiddenField)e.Item.FindControl("hdnDsrType");
                //string status = ddlDsrType.SelectedItem.Value;

                SaleType = Convert.ToString(Session["SaleType"]);


                //DistIdStr1=Convert.ToString(Session["DistIdStr1"]);
                //PartyIdStr1= Convert.ToString(Session["PartyIdStr1"]) ;
                //ProdctGrp=Convert.ToString(Session["ProdctGrp"]) ;
                //Product= Convert.ToString(Session["Product"]) ;
                fromDate = Convert.ToString(Session["fromdate"]);
                todate = Convert.ToString(Session["toDate"]);
                Type = Convert.ToString(Session["Type"]);

                DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');
                PartyIdStr1 = PartyIdStr1.TrimStart(',').TrimEnd(',');
                ProdctGrp = ProdctGrp.TrimStart(',').TrimEnd(',');
                Product = Product.TrimStart(',').TrimEnd(',');
                Response.Redirect("MISReportItemW.aspx?PartyId=" + hdPartyId.Value + "&FromDate=" + fromDate + "&ToDate=" + todate + "&Type=" + Type + "&SaleType=" + SaleType);// + "&DistIdStr1=" + DistIdStr1 + "&PartyIdStr1=" + PartyIdStr1 + "&ProdctGrp=" + ProdctGrp + "&Product=" + Product + "&AreaId=" + AreaId
            }

        }


    }
}