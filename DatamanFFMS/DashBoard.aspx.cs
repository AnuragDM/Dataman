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
    public partial class DashBoard : System.Web.UI.Page
    {
        string searchCriteria = "";
        string smIDStr1 = "";
        string roleType = "";
        string search = ""; string smIDStr = ""; string  userIdStr = ""; string month = ""; string state = ""; string dist = ""; string city = ""; string distibutor = ""; string sqlStr = ""; string distId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                this.fillfyear();
            
                //this.fillfyear1();
                this.BindDDLMonth();
                this.fillGrowthRegion();
                this.fillGrowthByARegion();
                this.fillSalesRegion();
                this.fillTrendRegion();
                this.fillUPRegion();
                this.fillUPState(0);
                this.fillTransactionRegion();
                this.fillZoneRegion();
                this.fillReguge();
                this.fillEll();
                this.fillVariance();
                this.FillGroup();
                this.fillUnderUsers1();
                this.PopulateState(0);
                rhc.Visible = false;
                this.categoryPopulateState(0);
                this.AnalyticsPopulateState(0);
                int tilesCount = RadTileList1.GetAllTiles().Count;
                int height = tilesCount * 160; //a tile is 150px tall and we add some for the padding between them
                // int wei = tilesCount * 160;
                //RadTileList1.Width = wei;
                //RadTileList1.Height = height;
                RadTileList1.TileRows = tilesCount/4; //TileRows defauilts to 3
                CWSD.Style.Add("display", "none");
                CWSDiv.Style.Add("display", "none");
                CategorySaleDiv.Style.Add("display", "none");
                CategorySaleDashboard.Style.Add("display", "none");
                ddlYear.SelectedValue = "2017";

                AnalyticstxtFromDate.Text = DateTime.Parse(DateTime.Now.AddMonths(-1).AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                AnalyticstxtTo.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");

                txtFromtDate.Text = DateTime.Parse(DateTime.Now.AddMonths(-1).AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                txtTo.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");

                categorytxtFromDate.Text = DateTime.Parse(DateTime.Now.AddMonths(-1).AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                categorytxtTo.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");

                txtUPDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                txtDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");

                txtVisitDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            CWSD.Style.Add("display", "none");
            CWSDiv.Style.Add("display", "none");
            CategorySaleDiv.Style.Add("display", "none");
            CategorySaleDashboard.Style.Add("display", "none");
            AnalyticsDiv.Style.Add("display", "none");
            AnalyticsDashboard.Style.Add("display", "none");
            GrowthByRegionDiv.Style.Add("display", "none");
            GrowthByRegionDashBoard.Style.Add("display", "none");
            GrowthByAreaDiv.Style.Add("display", "none");
            GrowthByAreaDashBoard.Style.Add("display", "none");
            SalesDiv.Style.Add("display", "none");
            SalesDashBoard.Style.Add("display", "none");
            TrendDiv.Style.Add("display", "none");
            TrendDashBoard.Style.Add("display", "none");

            TransAmtZoneDiv.Style.Add("display", "none");
            TransAmtDashboard.Style.Add("display", "none");
            TransStateDiv.Style.Add("display", "none");
            TransStateDashBoard.Style.Add("display", "none");
            StockDashBoardDiv.Style.Add("display", "none");
            OutletsCoverageDashBoardDiv.Style.Add("display", "none");
            TargetAchievementDiv.Style.Add("display", "none");
            VarianceDiv.Style.Add("display", "none");
            ELLDiv.Style.Add("display", "none");
            TLightDiv.Style.Add("display", "none");
            UserPerformanceDiv.Style.Add("display", "none");
            ZoneDiv.Style.Add("display", "none");
            ZoneDashboard.Style.Add("display", "none");
            AttendanceDiv.Style.Add("display", "none");
            lblTotal.InnerText = "";

        }

        protected void btnTransBack_Click(object sender, EventArgs e)
        {
            TransStateDiv.Style.Add("display", "none");
            TransStateDashBoard.Style.Add("display", "none");
            TransAmtZoneDiv.Style.Add("display", "block");
            TransRegionDiv.Style.Add("display", "none");
        }
        protected void RadTextTile11_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/NewDashBoard4.aspx");
        }
        protected void btnAmtState_Click(object sender, EventArgs e)
        {
            TransStateDiv.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            TransAmtZoneDiv.Style.Add("display", "none");
            TransAmtDashboard.Style.Add("display", "none");
            if (ddlTransRegion.SelectedItem.Value != "0")
            {
                this.fillTransactionState(Settings.DMInt32(ddlTransRegion.SelectedItem.Value));
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Region');", true);
                TransStateDiv.Style.Add("display", "none");
                mainDiv.Style.Add("display", "none");
                TransAmtZoneDiv.Style.Add("display", "block");
                return;
            }
        }
        private void fillfyear()
        {
            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                //txtcurrentyear.DataSource = dt;
                //txtcurrentyear.DataTextField = "Yr";
                //txtcurrentyear.DataValueField = "id";
                //txtcurrentyear.DataBind();
            }

        }

        protected void OnTileClick(object sender, TileListEventArgs e)
        {
            string name = e.Tile.Name;
            //if (name.Equals("OutletsCoverage"))
            //{
            //    RadRadialGauge rrgActiveOutlets = new RadRadialGauge();
            //    //rrgActiveOutlets.Pointer.Value = 500;
            //    //// rrgActiveOutlets.Pointer.Cap.Size="0.1";
            //    //rrgActiveOutlets.Scale.Min = 400;
            //    //rrgActiveOutlets.Scale.Max = 600;
            //    //rrgActiveOutlets.Scale.MajorUnit=100;
            //    //rrgActiveOutlets.Scale.Labels.Format = "{0}";
            //    //int from = 400;
            //    //int to =500;
            //    //Random randonGen = new Random();
            //    //System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
            //    //for (int i = 1; i < 3; i++)
            //    //{

            //    //    for (int ij = 0; ij < 1000000; ij++)
            //    //    {
            //    //        randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
            //    //    }
            //    //    GaugeRange gr = new GaugeRange();
            //    //    gr.Color =randomColor;
            //    //    gr.From = from;
            //    //    gr.To = to;
            //    //    rrgActiveOutlets.Scale.Ranges.Add(gr);
            //    //    from = to;
            //    //    to += 100;
            //    //}
            //    rrgActiveOutlets.Pointer.Value = (decimal)0.95;
            //    rrgActiveOutlets.Pointer.Cap.Size = (float)0.10;
            //    rrgActiveOutlets.Pointer.Cap.Color = System.Drawing.Color.Blue;
            //    rrgActiveOutlets.Pointer.Color = System.Drawing.Color.Blue;

            //    //Set Min and Max values of the Scale
            //    rrgActiveOutlets.Scale.Min = 0;

            //    //In order the Max value to be displayed it should be multiple of the MajorUnit
            //    rrgActiveOutlets.Scale.Max = (decimal)1.2;
            //    rrgActiveOutlets.Scale.MinorUnit = (decimal)0.5;
            //    rrgActiveOutlets.Scale.MajorUnit = (decimal)0.2;

            //    //Set Minor and Major Ticks properties
            //    rrgActiveOutlets.Scale.MinorTicks.Visible = false;
            //    rrgActiveOutlets.Scale.MajorTicks.Size = 10;

            //    //Set Scale Labels properties
            //    rrgActiveOutlets.Scale.Labels.Visible = true;
            //    rrgActiveOutlets.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            //    rrgActiveOutlets.Scale.Labels.Color = System.Drawing.Color.Black;
            //    rrgActiveOutlets.Scale.Labels.Format = "P0";
            //    rrgActiveOutlets.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            //    //Create new GaugeRange object
            //    GaugeRange gr1 = new GaugeRange();

            //    //Set the properties of the new object
            //    gr1.From = 0;
            //    gr1.To = (decimal)0.4;
            //    gr1.Color = System.Drawing.Color.Green;

            //    GaugeRange gr2 = new GaugeRange();
            //    gr2.From = (decimal)0.4;
            //    gr2.To = (decimal)0.8;
            //    gr2.Color = System.Drawing.Color.Yellow;

            //    GaugeRange gr3 = new GaugeRange();
            //    gr3.From = (decimal)0.8;
            //    gr3.To = (decimal)1.2;
            //    gr3.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            //    //Add Gauge objects to the RadialGauge
            //    rrgActiveOutlets.Scale.Ranges.Add(gr1);
            //    rrgActiveOutlets.Scale.Ranges.Add(gr2);
            //    rrgActiveOutlets.Scale.Ranges.Add(gr3);
            //    PHActiveOutlets.Controls.Add(rrgActiveOutlets);
            //}
            // if (name.Equals("Channel"))
            // { 
            //   CWSDiv.Style.Add("display", "block");
            //   mainDiv.Style.Add("display", "none");
            // } 
            //if (name.Equals("Category")) CategorySaleDiv.Style.Add("display", "block");
            //if (name.Equals("DSR")) DSR.Style.Add("display", "block");
            //if (name.Equals("Expences")) Expences.Style.Add("display", "block");
            //if (name.Equals("Leave")) Leave.Style.Add("display", "block");
        }

       
        #region
        private void BindDDLMonth()
        {
            try
            {

                for (int month = 4; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                  //  listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                }
                for (int month = 1; month <= 3; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                 //   listboxmonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void FillGroup()
        {
            string str = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                //ddlgroup.DataSource = deptValueDt;
                //ddlgroup.DataTextField = "ItemName";
                //ddlgroup.DataValueField = "ItemId";
                //ddlgroup.DataBind();
            }
            //ddlgroup.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void FillProduct(int UnderId)
        {
            string str = @"select ItemId,ItemName from MastItem where ItemType='Item' and Active=1 And UnderId=" + UnderId + " order by ItemName";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                //ddlProduct.DataSource = deptValueDt;
                //ddlProduct.DataTextField = "ItemName";
                //ddlProduct.DataValueField = "ItemId";
                //ddlProduct.DataBind();
            }
            // ddlProduct.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void PopulateState(int StateId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (StateId > 0)
                str = "select AreaID,AreaName from mastarea where AreaType='State' and (Active='1' or Areaid in (" + StateId + "))  order by AreaName";
            else
                str = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' order by AreaName";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
            //    }
            //}
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            if (dt.Rows.Count > 0)
            {
                lstDState.DataSource = dt;
                lstDState.DataTextField = "AreaName";
                lstDState.DataValueField = "AreaID";
                lstDState.DataBind();
            }

        }


        protected void txtcurrentyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grd.DataSource = null;
            //grd.DataBind();
        }

        protected void ddlgroup_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        [WebMethod(EnableSession = true)]
        public static void FillState()
        {
           // ClassOfNonStaticFunction obj = new ClassOfNonStaticFunction();
            DashBoard ds = new DashBoard();
            ds.fillDistCity();
        }

        public void fillDistCity()
        {
            if (lstDState.SelectedValue != string.Empty)
            {
                foreach (ListItem item in lstDState.Items)
                {

                    item.Selected = true;
                    state += item.Value + ",";
                }
                state = state.TrimStart(',').TrimEnd(',');

            }
            this.PopulateDISTRICT(state);
        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDistrict.Items.Clear();
            if (lstDState.SelectedValue != string.Empty)
            {
                foreach (ListItem item in lstDState.Items)
                {
                    if (item.Selected)
                    {
                         state += item.Value + ",";
                    }
                                       
                }
                state = state.TrimStart(',').TrimEnd(',');

            }
            
            this.PopulateDISTRICT(state);
            //mainDiv.Style.Add("display", "none");
            //CWSDiv.Style.Add("CWSDiv", "block");
        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCity.Items.Clear();
            if (ddlDistrict.SelectedValue != string.Empty)
            {
                foreach (ListItem item in ddlDistrict.Items)
                {
                    if (item.Selected)
                    {
                        dist += item.Value + ",";
                    }
                                       
                }
                dist = dist.TrimStart(',').TrimEnd(',');

            }
            this.PopulateCity(dist);

        }
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDistributor.Items.Clear();
            if (ddlCity.SelectedValue != string.Empty)
            {
                foreach (ListItem item in ddlCity.Items)
                {
                    if (item.Selected)
                    {
                        city += item.Value + ",";
                    }
                }
                city = city.TrimStart(',').TrimEnd(',');

            }
            this.PopulateDistributor(city);
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            lblTotal.InnerText = "";
            CWSD.Style.Add("display", "block");
            CWSDiv.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            // RadHtmlChart rch = new RadHtmlChart();
            rhc.PlotArea.Series.Clear();
            rhc.PlotArea.XAxis.Items.Clear();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            try
            {

                if (lstDState.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in lstDState.Items)
                    {

                        if (item.Selected)
                        {
                            state += item.Value + ",";
                        }
                    }
                    state = state.TrimStart(',').TrimEnd(',');
                    //searchCriteria += string.Format("And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                }

                if (ddlDistrict.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in ddlDistrict.Items)
                    {

                        if (item.Selected)
                        {
                            dist += item.Value + ",";
                        }
                    }
                    dist = dist.TrimStart(',').TrimEnd(',');
                   // searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);
                }
                if (ddlCity.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in ddlCity.Items)
                    {

                        if (item.Selected)
                        {
                            city += item.Value + ",";
                        }
                    }
                    city = city.TrimStart(',').TrimEnd(',');
                  //  searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                }
                

                if (state != "")
                {
                    if (dist != "")
                    {
                        if (city != "") searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                        else searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);

                    }
                    else searchCriteria += string.Format("And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                    
                }


                if (ddlDistributor.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in ddlDistributor.Items)
                    {

                        item.Selected = true;
                        distibutor += item.Value + ",";
                    }
                    distibutor = distibutor.TrimStart(',').TrimEnd(',');
                    searchCriteria += string.Format(" And DistId In({0})", distibutor);
                }

                if (rblType.SelectedValue == "PG")
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,mi1.Itemname as ProductGroup,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitem mi1 on mi.underid = mi1.Itemid where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by mi1.Itemname ", Convert.ToDateTime(txtFromtDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(txtTo.Text).ToString("dd-MMM-yyyy"));
                }

                else if (rblType.SelectedValue == "PS")
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,ms.Name as ProductSegment,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitemsegment ms on ms.id=mi.segmentid where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by ms.Name ", Convert.ToDateTime(txtFromtDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(txtTo.Text).ToString("dd-MMM-yyyy"));
                }
                else
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,mc.Name as ProductClass,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitemclass mc on mc.id=mi.classid  where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by mc.Name ", Convert.ToDateTime(txtFromtDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(txtTo.Text).ToString("dd-MMM-yyyy"));
                }


                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);

                if (dt.Rows.Count > 0)
                {
                    //string selectedMode = "Quantity";
                    string group = rblType.SelectedItem.Text.Replace(" ", string.Empty);
                   // string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode = "";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty)) + 500;
                    maxLavel = maxLavel + (maxLavel / 2);
                    decimal step = Math.Round(maxLavel / 10);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblTotal.InnerText = Convert.ToString(amt);
                    ShowDiv.Style.Add("display", "block");
                    rhc.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhc.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    rhc.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    rhc.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    rhc.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhc.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    rhc.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhc.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhc.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhc.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    rhc.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    rhc.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    rhc.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    rhc.PlotArea.YAxis.TitleAppearance.Text = "Amount";
                    // rhc.PlotArea.YAxis.TitleAppearance.Text = viewBy.SelectedItem.Text;
                    rhc.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhc.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhc.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhc.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhc.PlotArea.XAxis.Reversed = false;
                    rhc.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    rhc.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";
                    // rhc.PlotArea.XAxis.DataLabelsField = objTable.Rows[ax.xAxis][0].ToString();
                    rhc.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    rhc.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    rhc.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                   // rhc.ChartTitle.Text = "Distribution Channel " + selectedMode + " Wise Sale";

                    rhc.PlotArea.YAxis.MaxValue = maxLavel;
                    rhc.PlotArea.YAxis.MinValue = minLavel;
                    rhc.PlotArea.YAxis.Step = step;

                    rhc.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    rhc.PlotArea.XAxis.LabelsAppearance.Step = 1;

                    //           rhc.Width=Unit.Pixel(800);
                    //           rhc.Height = Unit.Pixel(500);
                    //           rhc.Transitions=true;
                    //           rhc.Skin="Silk";
                    //           rhc.PlotArea.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;

                    //           rhc.PlotArea.XAxis.AxisCrossingValue = 0;
                    //           rhc.PlotArea.XAxis.Color = System.Drawing.Color.Black; 
                    //           rhc.PlotArea.XAxis.MajorTickType= Telerik.Web.UI.HtmlChart.TickType.Outside;
                    //              rhc.PlotArea.XAxis.MinorTickType= Telerik.Web.UI.HtmlChart.TickType.None;
                    //           rhc.PlotArea.YAxis.AxisCrossingValue=0;
                    //           rhc.PlotArea.YAxis.Color = System.Drawing.Color.Black; 
                    //           rhc.PlotArea.YAxis.MajorTickSize=1;
                    //            rhc.PlotArea.YAxis.MajorTickType= Telerik.Web.UI.HtmlChart.TickType.Outside;
                    //              rhc.PlotArea.YAxis.MajorTickType= Telerik.Web.UI.HtmlChart.TickType.None;
                    //            rhc.PlotArea.YAxis.Reversed=false;
                    //          rhc.PlotArea.YAxis.LabelsAppearance.DataFormatString="{0}";
                    //            rhc.PlotArea.YAxis.LabelsAppearance.RotationAngle=0;
                    //            rhc.PlotArea.YAxis.LabelsAppearance.Skip=0;
                    //            rhc.PlotArea.YAxis.LabelsAppearance.Step=1;
                    //            rhc.PlotArea.YAxis.TitleAppearance.Position= Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    //            rhc.PlotArea.YAxis.TitleAppearance.RotationAngle=0;
                    //            rhc.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    //            rhc.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    //rhc.Legend.Appearance.Position= Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;

                    rhc.PlotArea.Series.Clear();
                    //    ColumnSeries _cs = new ColumnSeries();
                    ////    _cs.Name =ddlgroup.SelectedItem.Text;
                    //    _cs.Stacked = false;
                    //    _cs.Gap = 1.5;
                    //    _cs.Spacing = 0.4;
                    //    _cs.Appearance.FillStyle.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#d5a2bb");
                    //    _cs.LabelsAppearance.DataFormatString = "Sales";
                    //    _cs.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.OutsideEnd;
                    //    _cs.TooltipsAppearance.DataFormatString = "Sales";
                    //    _cs.TooltipsAppearance.Color = System.Drawing.Color.White;

                    //    Random randonGen = new Random();
                    //    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));


                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        CategorySeriesItem csi = new CategorySeriesItem();
                    //        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                    //        for (int ij = 0; ij < 1000000; ij++)
                    //        {
                    //            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    //        }
                    //        _cs.Appearance.FillStyle.BackgroundColor = randomColor;

                    //        csi.BackgroundColor = randomColor;
                    //        _cs.SeriesItems.Add(csi);

                    //    }
                    //    rhc.PlotArea.Series.Add(_cs);

                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        AxisItem ai = new AxisItem();
                    //        ai.LabelText = dr[""+group+""].ToString();
                    //        rhc.PlotArea.XAxis.Items.Add(ai);
                    //    }
                    //    rhc.Visible = true;
                    //    dt.Rows.Clear();

                    BarSeries bs = new BarSeries();
                    bs.Stacked = false;
                    bs.Gap = 1.5;
                    bs.Spacing = 0.4;
                    bs.Appearance.FillStyle.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#c5d291");
                    bs.LabelsAppearance.DataFormatString = "{0}";
                    bs.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.OutsideEnd;
                    bs.TooltipsAppearance.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#c5d291");
                    bs.TooltipsAppearance.DataFormatString = "{0}";
                    bs.LabelsAppearance.Color = System.Drawing.Color.Black;
                    Random randonGen = new Random();
                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    foreach (DataRow dr in dt.Rows)
                    {
                        CategorySeriesItem csi = new CategorySeriesItem();
                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        bs.Appearance.FillStyle.BackgroundColor = randomColor;

                        csi.BackgroundColor = randomColor;
                        bs.SeriesItems.Add(csi);

                    }

                    rhc.PlotArea.Series.Add(bs);

                    foreach (DataRow dr in dt.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["" + group + ""].ToString();
                        rhc.PlotArea.XAxis.Items.Add(ai);
                    }
                    rhc.Visible = true;
                    dt.Rows.Clear();
                }
                else
                {
                    rhc.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblGrowthByRegionDiv.InnerText = "";
                }

            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

        public void PopulateDISTRICT(string StateId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (StateId != "")
            {
                str = "select AreaID,AreaName from mastarea where AreaType='DISTRICT' and UnderId In(" + StateId + ")  order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    ddlDistrict.DataSource = dt;
                    ddlDistrict.DataTextField = "AreaName";
                    ddlDistrict.DataValueField = "AreaID";
                    ddlDistrict.DataBind();
                }
            }

        }
        public void PopulateCity(string DISTRICTId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (DISTRICTId != "")
            {
                str = "select AreaID,AreaName from mastarea where AreaType='City' and UnderId In(" + DISTRICTId + ")  order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    ddlCity.DataSource = dt;
                    ddlCity.DataTextField = "AreaName";
                    ddlCity.DataValueField = "AreaID";
                    ddlCity.DataBind();
                }
            }

        }
        public void PopulateDistributor(string CityId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (CityId != "")
            {
                str = "select PartyID,PartyName from mastParty where PartyDist=1 and CityId In(" + CityId + ")  order by PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    ddlDistributor.DataSource = dt;
                    ddlDistributor.DataTextField = "PartyName";
                    ddlDistributor.DataValueField = "PartyID";
                    ddlDistributor.DataBind();
                }
            }
        }


        protected void ddlState_TextChanged(object sender, EventArgs e)
        {

            foreach (ListItem item in lstDState.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
            this.PopulateDISTRICT(smIDStr1);
        }

        protected void ddlDistrict_TextChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in ddlDistrict.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
            this.PopulateCity(smIDStr1);
        }

        protected void ddlCity_TextChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in ddlCity.Items)
            {
                if (item.Selected)
                {
                    smIDStr1 += item.Value + ",";
                }
            }
            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
            this.PopulateDistributor(smIDStr1);

        }
        #endregion
        #region
        public void categoryPopulateState(int StateId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (StateId > 0)
                str = "select AreaID,AreaName from mastarea where AreaType='State' and (Active='1' or Areaid in (" + StateId + "))  order by AreaName";
            else
                str = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' order by AreaName";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
            //    }
            //}
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            if (dt.Rows.Count > 0)
            {
                categoryddlState.DataSource = dt;
                categoryddlState.DataTextField = "AreaName";
                categoryddlState.DataValueField = "AreaID";
                categoryddlState.DataBind();
            }

        }
        public void categoryPopulateDISTRICT(string StateId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (StateId != "")
            {
                str = "select AreaID,AreaName from mastarea where AreaType='DISTRICT' and UnderId In(" + StateId + ")  order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    categoryddlDistrict.DataSource = dt;
                    categoryddlDistrict.DataTextField = "AreaName";
                    categoryddlDistrict.DataValueField = "AreaID";
                    categoryddlDistrict.DataBind();
                }
            }

        }
        public void categoryPopulateCity(string DISTRICTId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (DISTRICTId != "")
            {
                str = "select AreaID,AreaName from mastarea where AreaType='City' and UnderId In(" + DISTRICTId + ")  order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    categoryddlCity.DataSource = dt;
                    categoryddlCity.DataTextField = "AreaName";
                    categoryddlCity.DataValueField = "AreaID";
                    categoryddlCity.DataBind();
                }
            }

        }
        public void categoryPopulateDistributor(string CityId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (CityId != "")
            {
                str = "select PartyID,PartyName from mastParty where PartyDist=1 and CityId In(" + CityId + ")  order by PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    categoryddlDistributor.DataSource = dt;
                    categoryddlDistributor.DataTextField = "PartyName";
                    categoryddlDistributor.DataValueField = "PartyID";
                    categoryddlDistributor.DataBind();
                }
            }
        }
        protected void categoryddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoryddlDistrict.Items.Clear();
            if (categoryddlState.SelectedValue != string.Empty)
            {
                foreach (ListItem item in categoryddlState.Items)
                {

                    if (item.Selected)
                    {
                        state += item.Value + ",";
                    }
                }
                state = state.TrimStart(',').TrimEnd(',');

            }
            this.categoryPopulateDISTRICT(state);
        }
        protected void categoryddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoryddlCity.Items.Clear();
            if (categoryddlDistrict.SelectedValue != string.Empty)
            {
                foreach (ListItem item in categoryddlDistrict.Items)
                {

                    if (item.Selected)
                    {
                        dist += item.Value + ",";
                    }
                }
                dist = dist.TrimStart(',').TrimEnd(',');

            }
            this.categoryPopulateCity(dist);

        }
        protected void categoryddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoryddlDistrict.Items.Clear();
            if (categoryddlCity.SelectedValue != string.Empty)
            {
                foreach (ListItem item in categoryddlCity.Items)
                {

                    if (item.Selected)
                    {
                        city += item.Value + ",";
                    }
                }
                city = city.TrimStart(',').TrimEnd(',');

            }
            this.categoryPopulateDistributor(city);
        }
        protected void categorybtnShow_Click(object sender, EventArgs e)
        {
            lblTotal.InnerText = "";

            CategorySaleDashboard.Style.Add("display", "block");
            CategorySaleDiv.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            // RadHtmlChart rch = new RadHtmlChart();
            pieChart.PlotArea.Series.Clear();
            pieChart.PlotArea.XAxis.Items.Clear();

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            try
            {

                if (categoryddlState.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in categoryddlState.Items)
                    {

                        // item.Selected = true;
                        if (item.Selected)
                        {
                            state += item.Value + ",";
                        }

                    }
                    state = state.TrimStart(',').TrimEnd(',');
                    //searchCriteria += string.Format("And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                }

                if (categoryddlDistrict.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in categoryddlDistrict.Items)
                    {

                        if (item.Selected)
                        {
                            dist += item.Value + ",";
                        }

                    }
                    dist = dist.TrimStart(',').TrimEnd(',');
                    //searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);
                }
                if (categoryddlCity.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in categoryddlCity.Items)
                    {

                        if (item.Selected)
                        {
                            city += item.Value + ",";
                        }

                    }
                    city = city.TrimStart(',').TrimEnd(',');
                    //searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                }
                if (categoryddlDistributor.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in categoryddlDistributor.Items)
                    {

                        if (item.Selected)
                        {
                            distibutor += item.Value + ",";
                        }

                    }
                    distibutor = distibutor.TrimStart(',').TrimEnd(',');
                    searchCriteria += string.Format(" And DistId In({0})", distibutor);
                }




                if (state != "")
                {
                    if (dist != "")
                    {

                        if (city != "")
                        {
                            searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                        }
                        else
                        {
                            searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);
                        }

                    }
                    else
                    {
                        searchCriteria += string.Format("And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                    }

                }


                if (Categoryrbl.SelectedValue == "PG")
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,mi1.Itemname as ProductGroup,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitem mi1 on mi.underid = mi1.Itemid where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by mi1.Itemname ", Convert.ToDateTime(categorytxtFromDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(categorytxtTo.Text).ToString("dd-MMM-yyyy"));
                }

                else if (Categoryrbl.SelectedValue == "PS")
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,ms.Name as ProductSegment,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitemsegment ms on ms.id=mi.segmentid where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by ms.Name ", Convert.ToDateTime(categorytxtFromDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(categorytxtTo.Text).ToString("dd-MMM-yyyy"));
                }
                else
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,mc.Name as ProductClass,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitemclass mc on mc.id=mi.classid  where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by mc.Name ", Convert.ToDateTime(categorytxtFromDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(categorytxtTo.Text).ToString("dd-MMM-yyyy"));
                }

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);
                if (dt.Rows.Count > 0)
                {
                    //string selectedMode = "Quantity";
                    string group = Categoryrbl.SelectedItem.Text.Replace(" ", string.Empty);
                   // string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode = "";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));

                    decimal step = Math.Round(maxLavel / 10);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblCategory.InnerText = Convert.ToString(amt);
                    CategoryDiv.Style.Add("display","block");

                    pieChart.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    pieChart.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    pieChart.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    pieChart.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    pieChart.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    pieChart.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    pieChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    pieChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    pieChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    pieChart.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    pieChart.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    pieChart.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    pieChart.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    pieChart.PlotArea.YAxis.TitleAppearance.Text = "Amount";
                    // pieChart.PlotArea.YAxis.TitleAppearance.Text = viewBy.SelectedItem.Text;
                    pieChart.PlotArea.XAxis.AxisCrossingValue = 0;
                    pieChart.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    pieChart.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    pieChart.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    pieChart.PlotArea.XAxis.Reversed = false;
                    pieChart.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    pieChart.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";
                    // pieChart.PlotArea.XAxis.DataLabelsField = objTable.Rows[ax.xAxis][0].ToString();
                    pieChart.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    pieChart.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    pieChart.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    //pieChart.ChartTitle.Text = "Distribution Channel " + selectedMode + " Wise Sale";

                    pieChart.PlotArea.YAxis.MaxValue = maxLavel;
                    pieChart.PlotArea.YAxis.MinValue = 0;
                    pieChart.PlotArea.YAxis.Step = step;

                    pieChart.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    pieChart.PlotArea.XAxis.LabelsAppearance.Step = 1;
                    pieChart.PlotArea.Series.Clear();

                    PieSeries _ps = new PieSeries();
                    _ps.StartAngle = 90;
                    _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
                    _ps.LabelsAppearance.DataFormatString = "{0}";
                    _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
                    _ps.TooltipsAppearance.DataFormatString = "{0}";
                    Random randonGen = new Random();
                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    int cnt = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        PieSeriesItem _psItem = new PieSeriesItem();
                        if (cnt == 0) _psItem.Exploded = true;
                        else _psItem.Exploded = false;
                        if (Convert.ToDecimal(dr["Amount"]) > 0) _psItem.Y = Convert.ToDecimal(dr["Amount"]);
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        _psItem.BackgroundColor = randomColor;

                        _psItem.Name = dr["" + group + ""].ToString();
                        _ps.SeriesItems.Add(_psItem);
                        cnt++;
                    }

                    pieChart.PlotArea.Series.Add(_ps);
                }
                else
                {
                    pieChart.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblCategory.InnerText = "";
                    CategoryDiv.Style.Add("display", "none");
                }

            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        #endregion

        #region
        public void AnalyticsPopulateState(int StateId)
        {
            ArrayList list = new ArrayList();
            string str = "";

            if (StateId > 0)
                str = "select AreaID,AreaName from mastarea where AreaType='State' and (Active='1' or Areaid in (" + StateId + "))  order by AreaName";
            else
                str = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' order by AreaName";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        list.Add(new ListItem(dr["AreaName"].ToString(), dr["AreaID"].ToString()));
            //    }
            //}
            //if (list.Count == 1)
            //    PopulateCities(dt.Rows[0]["Id"].ToString());
            if (dt.Rows.Count > 0)
            {
                AnalyticsddlState.DataSource = dt;
                AnalyticsddlState.DataTextField = "AreaName";
                AnalyticsddlState.DataValueField = "AreaID";
                AnalyticsddlState.DataBind();
            }

        }
        protected void AnalyticsddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnalyticsddlDistrict.Items.Clear();
            if (AnalyticsddlState.SelectedValue != string.Empty)
            {
                foreach (ListItem item in AnalyticsddlState.Items)
                {

                    if (item.Selected)
                    {
                        state += item.Value + ",";
                    }
                   
                }
                state = state.TrimStart(',').TrimEnd(',');

            }
            this.AnalyticsPopulateDISTRICT(state);
        }
        protected void AnalyticsddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnalyticsddlCity.Items.Clear();
            if (AnalyticsddlDistrict.SelectedValue != string.Empty)
            {
                foreach (ListItem item in AnalyticsddlDistrict.Items)
                {
                    if (item.Selected)
                    {
                        dist += item.Value + ",";
                    }
                                     
                }
                dist = dist.TrimStart(',').TrimEnd(',');

            }
            this.AnalyticsPopulateCity(dist);

        }
        protected void AnalyticsddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnalyticsddlDistributor.Items.Clear();
            if (AnalyticsddlCity.SelectedValue != string.Empty)
            {
                foreach (ListItem item in AnalyticsddlCity.Items)
                {

                    if (item.Selected)
                    {
                        city += item.Value + ",";
                    }
                }
                city = city.TrimStart(',').TrimEnd(',');

            }
            this.AnalyticsPopulateDistributor(city);
        }
        protected void AnalyticsbtnShow_Click(object sender, EventArgs e)
        {
            lblTotal.InnerText = "";

            CWSD.Style.Add("display", "none");
            CWSDiv.Style.Add("display", "none");
            CategorySaleDashboard.Style.Add("display", "none");
            CategorySaleDiv.Style.Add("display", "none");
            mainDiv.Style.Add("display", "none");
            AnalyticsDashboard.Style.Add("display", "block");
            AnalyticsDiv.Style.Add("display", "block");

            // RadHtmlChart rch = new RadHtmlChart();
            LineChart.PlotArea.Series.Clear();
            LineChart.PlotArea.XAxis.Items.Clear();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;

            try
            {

                if (AnalyticsddlState.SelectedValue != string.Empty)
                {
                    distId = "";
                    foreach (ListItem item in AnalyticsddlState.Items)
                    {


                        if (item.Selected)
                        {
                            state += item.Value + ",";
                        }
                    }
                    state = state.TrimStart(',').TrimEnd(',');
                    //searchCriteria += string.Format("And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                    //distId += string.Format("partyid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                }

                if (AnalyticsddlDistrict.SelectedValue != string.Empty)
                {
                    distId = "";
                    foreach (ListItem item in AnalyticsddlDistrict.Items)
                    {

                        if (item.Selected)
                        {
                            dist += item.Value + ",";
                        }

                    }
                    dist = dist.TrimStart(',').TrimEnd(',');
                    //searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);
                    //distId += string.Format("partyid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);
                }
                if (AnalyticsddlCity.SelectedValue != string.Empty)
                {
                    distId = "";
                    foreach (ListItem item in AnalyticsddlCity.Items)
                    {
                        if (item.Selected)
                        {
                            city += item.Value + ",";
                        }
                    }
                    city = city.TrimStart(',').TrimEnd(',');
                    //searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                    //distId += string.Format("partyid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                }


                if (state != "")
                {
                    if (dist != "")
                    {
                        
                        if (city != "")
                        {
                            distId = "";
                            searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                            distId += string.Format("partyid in (select partyid from mastparty where partydist=1 and  CityId In({0}))", city);
                        }
                        else {
                            distId = "";
                            searchCriteria += string.Format(" And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);
                            distId += string.Format("partyid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  districtid In({0})))", dist);
                        }

                    }
                    else {
                        distId = "";
                        searchCriteria += string.Format("And distid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                        distId += string.Format("partyid in (select partyid from mastparty where partydist=1 and cityid in (select distinct cityid from viewgeo where  StateId In({0})))", state);
                    }

                }

                if (AnalyticsddlDistributor.SelectedValue != string.Empty)
                {
                    distId = "";
                    foreach (ListItem item in AnalyticsddlDistributor.Items)
                    {

                        item.Selected = true;
                        distibutor += item.Value + ",";
                    }
                    distibutor = distibutor.TrimStart(',').TrimEnd(',');
                    searchCriteria += string.Format(" And DistId In({0})", distibutor);
                    distId += string.Format("partyid In({0})", distibutor);
                }

                if (Analyticsrbl.SelectedValue == "PG")
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,mi1.Itemname as ProductGroup,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitem mi1 on mi.underid = mi1.Itemid where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by mi1.Itemname ", Convert.ToDateTime(AnalyticstxtFromDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(AnalyticstxtTo.Text).ToString("dd-MMM-yyyy"));
                }

                else if (Analyticsrbl.SelectedValue == "PS")
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,ms.Name as ProductSegment,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitemsegment ms on ms.id=mi.segmentid where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by ms.Name ", Convert.ToDateTime(AnalyticstxtFromDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(AnalyticstxtTo.Text).ToString("dd-MMM-yyyy"));
                }
                else
                {
                    sqlStr = string.Format("select sum (qty * rate) as Amount,mc.Name as ProductClass,max(td1.Vdate) as Vdate from Transdistinv1 td1 left join mastitem mi on td1.Itemid = mi.itemid left join mastitemclass mc on mc.id=mi.classid  where td1.Vdate between '{0}' and '{1}' \n" + searchCriteria + " group by mc.Name ", Convert.ToDateTime(AnalyticstxtFromDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(AnalyticstxtTo.Text).ToString("dd-MMM-yyyy"));
                }


                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);
                if (dt.Rows.Count > 0)
                {
                    DataTable distr = new DataTable();
                    if (distId != "")
                    {
                        string str = string.Format("select PartyName from MastParty Where " + distId + "");
                        distr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    }
                    //string selectedMode = "Quantity";
                    string group = Analyticsrbl.SelectedItem.Text.Replace(" ", string.Empty);
                    //string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode = "";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));
                    maxLavel = maxLavel + (maxLavel / 2);
                    decimal step = Math.Round(maxLavel / 10);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblAnalytics.InnerText = Convert.ToString(amt);
                  
                    AnalyticsTotal.Style.Add("display", "block");
                    LineChart.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    LineChart.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    LineChart.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    LineChart.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    LineChart.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    LineChart.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    LineChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    LineChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    LineChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    LineChart.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    LineChart.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    LineChart.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    LineChart.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    LineChart.PlotArea.YAxis.TitleAppearance.Text = "Amount";
                    // LineChart.PlotArea.YAxis.TitleAppearance.Text = viewBy.SelectedItem.Text;
                    LineChart.PlotArea.XAxis.AxisCrossingValue = 0;
                    LineChart.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    LineChart.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    LineChart.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    LineChart.PlotArea.XAxis.Reversed = false;
                    LineChart.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    LineChart.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";
                    // LineChart.PlotArea.XAxis.DataLabelsField = objTable.Rows[ax.xAxis][0].ToString();
                    LineChart.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    LineChart.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    LineChart.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                   // LineChart.ChartTitle.Text = "Distribution Channel " + selectedMode + " Wise Sale";

                    LineChart.PlotArea.YAxis.MaxValue = maxLavel;
                    LineChart.PlotArea.YAxis.MinValue = 0;
                    LineChart.PlotArea.YAxis.Step = step;
                    LineChart.PlotArea.YAxis.LabelsAppearance.Visible = false;
                    LineChart.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    LineChart.PlotArea.XAxis.LabelsAppearance.Step = 1;
                    LineChart.PlotArea.Series.Clear();
                    //  LineChart.Width = (dt.Rows.Count) * 55;
                    LineSeries _cs = new LineSeries();
                    //BarSeries _cs = new BarSeries();
                    //    _cs.Name =ddlgroup.SelectedItem.Text;
                    _cs.Stacked = false;
                    //_cs.Gap = 1.5;
                    //_cs.Spacing = 0.4;
                    _cs.Appearance.FillStyle.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#d5a2bb");
                    _cs.LabelsAppearance.DataFormatString = "Sales";
                    //_cs.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.OutsideEnd;
                    _cs.TooltipsAppearance.DataFormatString = "Sales";
                    _cs.TooltipsAppearance.Color = System.Drawing.Color.White;

                    Random randonGen = new Random();
                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));


                    foreach (DataRow dr in dt.Rows)
                    {
                        CategorySeriesItem csi = new CategorySeriesItem();
                        
                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        _cs.Appearance.FillStyle.BackgroundColor = randomColor;

                        csi.BackgroundColor = randomColor;
                        _cs.SeriesItems.Add(csi);

                    }
                    LineChart.PlotArea.Series.Add(_cs);

                    foreach (DataRow dr in dt.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["" + group + ""].ToString();
                        LineChart.PlotArea.XAxis.Items.Add(ai);
                    }
                    LineChart.Visible = true;
                    dt.Rows.Clear();

                    //LineChart.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    //LineChart.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    //LineChart.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    //LineChart.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    //LineChart.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    //LineChart.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    //LineChart.PlotArea.YAxis.AxisCrossingValue =0;
                    //LineChart.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    //LineChart.PlotArea.YAxis.MajorTickType=Telerik.Web.UI.HtmlChart.TickType.Outside;
                    //LineChart.PlotArea.YAxis.MaxValue = maxLavel;
                    //LineChart.PlotArea.YAxis.MinValue = 0;
                    //LineChart.PlotArea.YAxis.Step = step;
                    //LineChart.PlotArea.YAxis.Reversed=false;
                    //LineChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    //LineChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    //LineChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    //LineChart.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    //LineChart.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    //LineChart.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    //LineChart.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    //LineChart.PlotArea.YAxis.TitleAppearance.Text = "Amount";




                    //LineChart.PlotArea.Appearance.FillStyle.BackgroundColor=System.Drawing.Color.Transparent;
                    //// LineChart.PlotArea.YAxis.TitleAppearance.Text = viewBy.SelectedItem.Text;
                    //LineChart.PlotArea.XAxis.AxisCrossingValue = 0;
                    //LineChart.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    //LineChart.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    //LineChart.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    //LineChart.PlotArea.XAxis.Reversed = false;
                    //LineChart.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    //LineChart.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";
                    //// LineChart.PlotArea.XAxis.DataLabelsField = objTable.Rows[ax.xAxis][0].ToString();
                    //LineChart.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    //LineChart.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    //LineChart.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    //LineChart.ChartTitle.Text = "Distribution Channel " + selectedMode + " Wise Sale";
                    //LineChart.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    ////LineChart.PlotArea.XAxis.LabelsAppearance.Step = 1;



                    //LineSeries _cs = new LineSeries();
                    ////    _cs.Name =ddlgroup.SelectedItem.Text;
                    //_cs.Stacked = false;

                    //_cs.Appearance.FillStyle.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#5ab7de");
                    //_cs.LabelsAppearance.DataFormatString = "Sales";
                    //_cs.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                    //_cs.LineAppearance.Width = 1;
                    //_cs.TooltipsAppearance.DataFormatString = "Sales";
                    //_cs.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                    //_cs.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                    //_cs.MarkersAppearance.Size = 8;
                    //_cs.MarkersAppearance.BorderColor = System.Drawing.ColorTranslator.FromHtml("#5ab7de");
                    //_cs.MarkersAppearance.BorderWidth = 2;

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    AxisItem ai = new AxisItem();
                    //    ai.LabelText = dr["" + group + ""].ToString();
                    //    LineChart.PlotArea.XAxis.Items.Add(ai);
                    //}



                    //Random randonGen = new Random();
                    //System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    //if (distr!=null)
                    //{ 
                    //foreach (DataRow dr in distr.Rows)
                    //{
                    //    _cs.Name = Convert.ToString(dr["PartyName"]);
                    //}
                    //}
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    CategorySeriesItem csi = new CategorySeriesItem();
                    //    if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                    //    for (int ij = 0; ij < 1000000; ij++)
                    //    {
                    //        randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    //    }
                    //    _cs.Appearance.FillStyle.BackgroundColor = randomColor;

                    //    csi.BackgroundColor = randomColor;
                    //    _cs.SeriesItems.Add(csi);

                    //}

                    //LineChart.PlotArea.Series.Add(_cs);


                    //LineChart.Visible = true;
                    //dt.Rows.Clear();
                }
                else
                {
                    LineChart.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblAnalytics.InnerText = "";
                }
              
            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

        public void AnalyticsPopulateDISTRICT(string StateId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (StateId != "")
            {
                str = "select AreaID,AreaName from mastarea where AreaType='DISTRICT' and UnderId In(" + StateId + ")  order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    AnalyticsddlDistrict.DataSource = dt;
                    AnalyticsddlDistrict.DataTextField = "AreaName";
                    AnalyticsddlDistrict.DataValueField = "AreaID";
                    AnalyticsddlDistrict.DataBind();
                }
            }

        }
        public void AnalyticsPopulateCity(string DISTRICTId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (DISTRICTId != "")
            {
                str = "select AreaID,AreaName from mastarea where AreaType='City' and UnderId In(" + DISTRICTId + ")  order by AreaName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    AnalyticsddlCity.DataSource = dt;
                    AnalyticsddlCity.DataTextField = "AreaName";
                    AnalyticsddlCity.DataValueField = "AreaID";
                    AnalyticsddlCity.DataBind();
                }
            }

        }
        public void AnalyticsPopulateDistributor(string CityId)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (CityId != "")
            {
                str = "select PartyID,PartyName from mastParty where PartyDist=1 and CityId In(" + CityId + ")  order by PartyName";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt.Rows.Count > 0)
                {
                    AnalyticsddlDistributor.DataSource = dt;
                    AnalyticsddlDistributor.DataTextField = "PartyName";
                    AnalyticsddlDistributor.DataValueField = "PartyID";
                    AnalyticsddlDistributor.DataBind();
                }
            }
        }

        #endregion

        #region
        private void fillfyear1()
        {//Ankita - 18/may/2016- (For Optimization)
            // string str = "select * from Financialyear ";
            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlYear.DataSource = dt;
                ddlYear.DataTextField = "Yr";
                ddlYear.DataValueField = "id";
                ddlYear.DataBind();
            }
        }
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fillGrowthRegionState(Settings.DMInt32(ddlRegion.SelectedValue));
        }
        private void fillGrowthRegionState(int UnderId)
        {
            if (UnderId != null)
            {
                //string str = "select AreaId,AreaName from MastArea where AreaType='State' And UnderId=" + UnderId + "";
                //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //if (dt.Rows.Count > 0)
                //{
                //    lstState.DataSource = dt;
                //    lstState.DataTextField = "AreaName";
                //    lstState.DataValueField = "AreaId";
                //    lstState.DataBind();
                //}
                //lstState.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }
        private void fillGrowthRegion()
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='REGION'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlRegion.DataSource = dt;
                ddlRegion.DataTextField = "AreaName";
                ddlRegion.DataValueField = "AreaId";
                ddlRegion.DataBind();
            }
        }

        protected void btnGrowthByRegion_Click(object sender, EventArgs e)
        {
            lblTotal.InnerText = "";
            GrowthByRegionDiv.Style.Add("display", "block");
            GrowthByRegionDashBoard.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            // RadHtmlChart rch = new RadHtmlChart();
            AreaChart.PlotArea.Series.Clear();
            AreaChart.PlotArea.XAxis.Items.Clear();
            
            AreaChart.Visible = true;
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            string stateId = "";
            try
            {

                searchCriteria += string.Format("Where YearName={0}", ddlYear.SelectedValue);
                //if (lstState.SelectedValue != string.Empty)
                //{
                //    foreach (ListItem item in lstState.Items)
                //    {

                //        item.Selected = true;
                //        state += item.Value + ",";
                //    }
                //    state = state.TrimStart(',').TrimEnd(',');
                //    searchCriteria += string.Format("And StateId In({1})", state);
                //}
                string sqlstr = string.Format("Select AreaId From MastArea Where UnderId In({0})", Settings.DMInt32(ddlRegion.SelectedValue));
                DataTable dtstate = DbConnectionDAL.GetDataTable(CommandType.Text,sqlstr);
                foreach (DataRow dr in dtstate.Rows)
                {
                    stateId += dr["AreaId"] + ",";
                }
                stateId = stateId.TrimEnd(',');
                if (stateId != "")
                {
                    searchCriteria += "And StateId In(" + stateId + ")";
                }
                sqlStr = "select Sum(Amount) as Amount,StateId,StateName,MonthFullName,ShortMonthName,YearName,MonthNo from (select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate, year(td1.Vdate) as YearName,vg.stateid,vg.statename, year(td1.Vdate) As Year,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid group by vg.stateid,vg.statename,td1.Vdate)a " + searchCriteria + " \n" +
                         "group by StateName,MonthFullName,ShortMonthName,YearName,StateId,MonthNo order by a.StateName,a.MonthNo";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);
                if (dt.Rows.Count > 0)
                {
                    DataTable month = dt.DefaultView.ToTable(true, "ShortMonthName");
                    DataTable stateName = dt.DefaultView.ToTable(true, "StateName");
                    //string selectedMode = "Quantity";
                    string group = rblType.SelectedItem.Text.Replace(" ", string.Empty);
                   // string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode = "";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));

                    maxLavel = maxLavel + (maxLavel / 2);
                    decimal step = Math.Round(maxLavel / 10);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblGrowthByRegionDiv.InnerText = Convert.ToString(amt);

                    amtDiv.Style.Add("display", "block");

                    AreaChart.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    AreaChart.PlotArea.XAxis.AxisCrossingValue = 0;
                    AreaChart.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    AreaChart.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    AreaChart.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    AreaChart.PlotArea.XAxis.Reversed = false;
                  //  AreaChart.ChartTitle.Text = "Growth By Region DashBoard";
                    AreaChart.ChartTitle.Appearance.TextStyle.Color = System.Drawing.Color.Black;
                    AreaChart.ChartTitle.Appearance.TextStyle.FontSize = Unit.Pixel(24);
                    AreaChart.ChartTitle.Appearance.TextStyle.FontFamily = "Verdana";
                    AreaChart.ChartTitle.Appearance.TextStyle.Margin = "11";
                    AreaChart.ChartTitle.Appearance.TextStyle.Padding = "22";

                    AreaChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    AreaChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    AreaChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    AreaChart.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    AreaChart.PlotArea.YAxis.MaxValue = maxLavel;
                    AreaChart.PlotArea.YAxis.MinValue = minLavel;
                    AreaChart.PlotArea.YAxis.Step = step;
                    AreaChart.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    AreaChart.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    AreaChart.PlotArea.YAxis.MajorTickSize = 4;
                    AreaChart.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                    AreaChart.PlotArea.YAxis.Reversed = false;
                    AreaChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    AreaChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    AreaChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    AreaChart.PlotArea.YAxis.LabelsAppearance.Step = 1;

                    AreaChart.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    AreaChart.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    //AreaSeries _as=new AreaSeries();
                    //_as.Appearance.FillStyle.BackgroundColor=System.Drawing.Color.Blue;
                    //_as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                    //_as.LineAppearance.Width=1;
                    //_as.MarkersAppearance.MarkersType= Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                    //_as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                    //_as.MarkersAppearance.Size=6;
                    //_as.MarkersAppearance.BorderColor = System.Drawing.Color.Blue;
                    //_as.MarkersAppearance.BorderWidth=2;
                    //_as.TooltipsAppearance.Color = System.Drawing.Color.White;


                    //Random randonGen = new Random();
                    //System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    foreach (DataRow dr1 in stateName.Rows)
                    {
                        AreaSeries _as = new AreaSeries();
                        _as.Name = dr1["StateName"].ToString();

                        foreach (DataRow dr in dt.Rows)
                        {


                            string csta = dr["StateName"].ToString();
                            string psta = dr1["StateName"].ToString();
                            if (csta.Equals(psta))
                            {
                                Random randonGen = new Random();
                                System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                for (int ij = 0; ij < 1000000; ij++)
                                {
                                    randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                }
                                _as.Appearance.FillStyle.BackgroundColor = randomColor;

                                _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                                _as.LineAppearance.Width = 1;
                                _as.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                _as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                                _as.MarkersAppearance.Size = 6;
                                _as.MarkersAppearance.BorderColor = randomColor;
                                _as.MarkersAppearance.BorderWidth = 2;
                                _as.TooltipsAppearance.Color = System.Drawing.Color.White;

                                CategorySeriesItem csi = new CategorySeriesItem();
                                if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);



                                // csi.BackgroundColor = randomColor;
                                _as.SeriesItems.Add(csi);
                            }
                        }
                        AreaChart.PlotArea.Series.Add(_as);
                    }




                    foreach (DataRow dr in month.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["ShortMonthName"].ToString();
                        AreaChart.PlotArea.XAxis.Items.Add(ai);

                    }
                   
                    dt.Rows.Clear();

                }
                else
                {
                    AreaChart.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblGrowthByRegionDiv.InnerText = "";
                }

             
            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        #endregion

        #region
       
        private void fillGrowthByARegion()
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='REGION'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlGBAR.DataSource = dt;
                ddlGBAR.DataTextField = "AreaName";
                ddlGBAR.DataValueField = "AreaId";
                ddlGBAR.DataBind();
            }
        }

        protected void btnGrowthByArea_Click(object sender, EventArgs e)
        {
            lblTotal.InnerText = "";
            GrowthByAreaDiv.Style.Add("display", "block");
            GrowthByAreaDashBoard.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            // RadHtmlChart rch = new RadHtmlChart();
            AAreaChart.PlotArea.Series.Clear();
            AAreaChart.PlotArea.XAxis.Items.Clear();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            string stateId = "";
            try
            {

                searchCriteria += string.Format("Where  year(td1.Vdate)={0}", ddlGBAYear.SelectedValue);
                //if (lstState.SelectedValue != string.Empty)
                //{
                //    foreach (ListItem item in lstState.Items)
                //    {

                //        if (item.Selected)
                //        {
                //            state += item.Value + ",";
                //        }
                //    }
                //    state = state.TrimStart(',').TrimEnd(',');
                //    searchCriteria += string.Format(" And StateId In({1})", state);
                //}
                string sqlstr = string.Format("Select AreaId From MastArea Where UnderId In({0})", Settings.DMInt32(ddlGBAR.SelectedValue));
                DataTable dtstate = DbConnectionDAL.GetDataTable(CommandType.Text, sqlstr);
                foreach (DataRow dr in dtstate.Rows)
                {
                    stateId += dr["AreaId"] + ",";
                }
                stateId = stateId.TrimEnd(',');
                if (stateId != "")
                {
                    searchCriteria += " And StateId In(" + stateId + ")";
                }

                if (rblGBA.SelectedValue == "PG")
                {
                    sqlStr = "select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename,mi1.ItemId,mi1.ItemName from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid LEFT JOIN mastitem mi ON td1.ItemId=mi.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid " + searchCriteria + " \n" +
                        "group by vg.stateid,vg.statename,mi1.ItemId,mi1.ItemName order by vg.statename";
                }

                else if (rblGBA.SelectedValue == "PS")
                {
                    sqlStr = "select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename,ms.Id,ms.Name as ItemName from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid LEFT JOIN mastitem mi ON td1.ItemId=mi.ItemId LEFT JOIN MastItemSegment ms ON ms.Id=mi.SegmentId " + searchCriteria + "\n" +
                         "group by vg.stateid,vg.statename,ms.Id,ms.Name order by vg.statename";
                }
                else
                {
                    sqlStr = "select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename,mc.Id,mc.Name as ItemName from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid LEFT JOIN mastitem mi ON td1.ItemId=mi.ItemId LEFT JOIN MastItemClass mc ON mc.Id=mi.ClassId " + searchCriteria + " \n" +
                        "group by vg.stateid,vg.statename,mc.Id,mc.Name order by vg.statename";
                }
               
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);

                if (dt.Rows.Count > 0)
                {
                    DataTable dtItem = dt.DefaultView.ToTable(true, "ItemName");
                    DataTable stateName = dt.DefaultView.ToTable(true, "StateName");


                    string group = rblType.SelectedItem.Text.Replace(" ", string.Empty);
                   // string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode = "";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));

                    maxLavel = maxLavel + (maxLavel / 2);

                    decimal step = Math.Round(maxLavel / 10);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblGBA.InnerText = Convert.ToString(amt);
                    GrowthByAreaDivTotal.Style.Add("display", "block");
                    AAreaChart.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    AAreaChart.PlotArea.XAxis.AxisCrossingValue = 0;
                    AAreaChart.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    AAreaChart.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    AAreaChart.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    AAreaChart.PlotArea.XAxis.Reversed = false;
                    //AAreaChart.ChartTitle.Text = "Growth By Area DashBoard";
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    AAreaChart.PlotArea.YAxis.MaxValue = maxLavel;
                    AAreaChart.PlotArea.YAxis.MinValue = minLavel;
                    AAreaChart.PlotArea.YAxis.Step = step;
                    AAreaChart.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    AAreaChart.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    AAreaChart.PlotArea.YAxis.MajorTickSize = 4;
                    AAreaChart.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                    AAreaChart.PlotArea.YAxis.Reversed = false;
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    AAreaChart.PlotArea.YAxis.LabelsAppearance.Step = 1;

                    AAreaChart.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    AAreaChart.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    foreach (DataRow dr1 in dtItem.Rows)
                    {
                        AreaSeries _as = new AreaSeries();
                        _as.Name = dr1["ItemName"].ToString();


                        IEnumerable<String> countryIDs =
                                               dt
                                               .AsEnumerable()
                                               .Where(row => row.Field<String>("ItemName") == dr1["ItemName"].ToString())
                                               .Select(row => row.Field<String>("StateName"));


                        var itemname = countryIDs.ToList();
                        foreach (string value in itemname)
                        {
                            //string name = value;

                            foreach (DataRow dr in dt.Rows)
                            {


                                string csta = dr["ItemName"].ToString();
                                string psta = dr1["ItemName"].ToString();
                                string cstate = dr["StateName"].ToString();
                                string pstate = value;
                                if (cstate.Equals(pstate))
                                {
                                    if (csta.Equals(psta))
                                    {
                                        Random randonGen = new Random();
                                        System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        for (int ij = 0; ij < 1000000; ij++)
                                        {
                                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        }
                                        _as.Appearance.FillStyle.BackgroundColor = randomColor;

                                        _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                                        _as.LineAppearance.Width = 1;
                                        _as.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                        _as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                                        _as.MarkersAppearance.Size = 6;
                                        _as.MarkersAppearance.BorderColor = randomColor;
                                        _as.MarkersAppearance.BorderWidth = 2;
                                        _as.TooltipsAppearance.Color = System.Drawing.Color.White;

                                        CategorySeriesItem csi = new CategorySeriesItem();
                                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                                        csi.BackgroundColor = randomColor;
                                        _as.SeriesItems.Add(csi);
                                    }
                                }
                            }
                        }
                        AAreaChart.PlotArea.Series.Add(_as);
                    }




                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    AxisItem ai = new AxisItem();
                    //    ai.LabelText = dr["ShortMonthName"].ToString();
                    //    AreaChart.PlotArea.XAxis.Items.Add(ai);
                    //}

                    foreach (DataRow dr in stateName.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["StateName"].ToString();
                        AAreaChart.PlotArea.XAxis.Items.Add(ai);

                    }
                    AAreaChart.Visible = true;
                    dt.Rows.Clear();

                }
                else
                {
                    AAreaChart.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblGBA.InnerText = "";
                }


            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        #endregion

        #region

        private void fillSalesRegion()
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='REGION'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlSalesRegion.DataSource = dt;
                ddlSalesRegion.DataTextField = "AreaName";
                ddlSalesRegion.DataValueField = "AreaId";
                ddlSalesRegion.DataBind();
            }
        }

        protected void btnSales_Click(object sender, EventArgs e)
        {
            lblSales.InnerText = "";
            SalesDiv.Style.Add("display", "block");
            SalesDashBoard.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            // RadHtmlChart rch = new RadHtmlChart();
            rhcPrimary.PlotArea.Series.Clear();
            rhcPrimary.PlotArea.XAxis.Items.Clear();
            rhcSecondry.PlotArea.Series.Clear();
            rhcSecondry.PlotArea.XAxis.Items.Clear();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            string stateId = "";
            try
            {

                searchCriteria += string.Format("Where YearName={0} And Amount !=0 And StateId IS NOT NULL", ddlSales.SelectedValue);
                //if (lstState.SelectedValue != string.Empty)
                //{
                //    foreach (ListItem item in lstState.Items)
                //    {

                //        item.Selected = true;
                //        state += item.Value + ",";
                //    }
                //    state = state.TrimStart(',').TrimEnd(',');
                //    searchCriteria += string.Format(" And StateId In({1})", state);
                //}
                string sqlstr = string.Format("Select AreaId From MastArea Where UnderId In({0})", Settings.DMInt32(ddlSalesRegion.SelectedValue));
                DataTable dtstate = DbConnectionDAL.GetDataTable(CommandType.Text, sqlstr);
                foreach (DataRow dr in dtstate.Rows)
                {
                    stateId += dr["AreaId"] + ",";
                }
                stateId = stateId.TrimEnd(',');
                if (stateId != "")
                {
                    searchCriteria += " And StateId In(" + stateId + ")";
                }
                sqlStr = "select Sum(Amount) as Amount,StateId,StateName,MonthFullName,ShortMonthName,YearName,MonthNo from (select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate, year(td1.Vdate) as YearName,vg.stateid,vg.statename, year(td1.Vdate) As Year,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid group by vg.stateid,vg.statename,td1.Vdate)a " + searchCriteria + " \n" +
                        "group by StateName,MonthFullName,ShortMonthName,YearName,StateId,MonthNo order by a.StateName,a.MonthNo";

              string sqlStr1 = "Select Sum(Amount) as Amount,StateId,StateName,MonthFullName,ShortMonthName,YearName,MonthNo from (select sum (td1.OrderAmount) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename, year(td1.Vdate) as YearName,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from TransOrder td1 left join mastparty mp on td1.partyid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid group by vg.stateid,vg.statename,td1.Vdate) a " + searchCriteria + " \n" +
                      "group by StateName,MonthFullName,ShortMonthName,YearName,StateId,MonthNo order by a.StateName,a.MonthNo";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);


                if (dt.Rows.Count > 0)
                {

                    DataTable stateName = dt.DefaultView.ToTable(true, "StateName");

                    DataTable month = dt.DefaultView.ToTable(true, "ShortMonthName");



                    string group = rblType.SelectedItem.Text.Replace(" ", string.Empty);
                    //string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode = "";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));

                    decimal step = Math.Round(maxLavel / 20);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblSales.InnerText = Convert.ToString(amt);

                    SalesDiv.Style.Add("display", "block");
                    rhcPrimary.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcPrimary.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcPrimary.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcPrimary.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcPrimary.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcPrimary.PlotArea.XAxis.Reversed = false;
                   // rhcPrimary.ChartTitle.Text = "Sales Disribution vs. Consumption -Region Wise DashBoard";
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    rhcPrimary.PlotArea.YAxis.MaxValue = maxLavel;
                    rhcPrimary.PlotArea.YAxis.MinValue = minLavel;
                    rhcPrimary.PlotArea.YAxis.Step = step;
                    rhcPrimary.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    rhcPrimary.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcPrimary.PlotArea.YAxis.MajorTickSize = 4;
                    rhcPrimary.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                    rhcPrimary.PlotArea.YAxis.Reversed = false;
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcPrimary.PlotArea.YAxis.LabelsAppearance.Step = 1;

                    rhcPrimary.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcPrimary.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    foreach (DataRow dr1 in stateName.Rows)
                    {
                        AreaSeries _as = new AreaSeries();
                        _as.Name = dr1["StateName"].ToString();
                        IEnumerable<String> countryIDs =
                                           dt
                                           .AsEnumerable()
                                           .Where(row => row.Field<String>("StateName") == dr1["StateName"].ToString())
                                           .Select(row => row.Field<String>("MonthFullName"));


                        var itemname = countryIDs.ToList();
                        foreach (string value in itemname)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                string csta = dr["StateName"].ToString();
                                string psta = dr1["StateName"].ToString();

                                string cmonth = dr["MonthFullName"].ToString();
                                string pmonth = value;
                                if (cmonth.Equals(pmonth))
                                {
                                    if (csta.Equals(psta))
                                    {
                                        Random randonGen = new Random();
                                        System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        for (int ij = 0; ij < 1000000; ij++)
                                        {
                                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        }
                                        _as.Appearance.FillStyle.BackgroundColor = randomColor;

                                        _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                                        _as.LineAppearance.Width = 1;
                                        _as.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                        _as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                                        _as.MarkersAppearance.Size = 6;
                                        _as.MarkersAppearance.BorderColor = randomColor;
                                        _as.MarkersAppearance.BorderWidth = 2;
                                        _as.TooltipsAppearance.Color = System.Drawing.Color.White;

                                        CategorySeriesItem csi = new CategorySeriesItem();
                                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);



                                        // csi.BackgroundColor = randomColor;
                                        _as.SeriesItems.Add(csi);
                                    }
                                }
                            }
                        }
                        rhcPrimary.PlotArea.Series.Add(_as);
                    }

                    foreach (DataRow dr in month.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["ShortMonthName"].ToString();
                        rhcPrimary.PlotArea.XAxis.Items.Add(ai);

                    }
                    rhcPrimary.Visible = true;
                    dt.Rows.Clear();

                }
                else {
                    rhcPrimary.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblSales.InnerText = "";
                }

                DataTable dtSecondary = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr1);
                if (dtSecondary.Rows.Count > 0)
                {
                    //  DataTable dtItem = dt.DefaultView.ToTable(true, "ItemName");
                    DataTable dtSecondarystateName = dtSecondary.DefaultView.ToTable(true, "StateName");

                    DataTable Smonth = dtSecondary.DefaultView.ToTable(true, "ShortMonthName");

                    decimal SminLavel = Convert.ToDecimal(dtSecondary.Compute("min([Amount])", string.Empty));
                    decimal SmaxLavel = Convert.ToDecimal(dtSecondary.Compute("max([Amount])", string.Empty));

                    SmaxLavel = SmaxLavel + (SmaxLavel / 2);
                    decimal Sstep = Math.Round(SmaxLavel / 20);
                    double Samt = Convert.ToDouble(dtSecondary.Compute("Sum([Amount])", string.Empty));
                    lblSales.InnerText = Convert.ToString(Samt);

                    rhcSecondry.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcSecondry.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcSecondry.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcSecondry.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcSecondry.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcSecondry.PlotArea.XAxis.Reversed = false;
                    //rhcSecondry.ChartTitle.Text = "Growth By Area DashBoard";
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    rhcSecondry.PlotArea.YAxis.MaxValue = SmaxLavel;
                    rhcSecondry.PlotArea.YAxis.MinValue = SminLavel;
                    rhcSecondry.PlotArea.YAxis.Step = Sstep;
                    rhcSecondry.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    rhcSecondry.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcSecondry.PlotArea.YAxis.MajorTickSize = 4;
                    rhcSecondry.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                    rhcSecondry.PlotArea.YAxis.Reversed = false;
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcSecondry.PlotArea.YAxis.LabelsAppearance.Step = 1;

                    rhcSecondry.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcSecondry.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    foreach (DataRow dr1 in dtSecondarystateName.Rows)
                    {
                        AreaSeries _as = new AreaSeries();
                        _as.Name = dr1["StateName"].ToString();
                        IEnumerable<String> countryIDs =
                                           dtSecondary
                                           .AsEnumerable()
                                           .Where(row => row.Field<String>("StateName") == dr1["StateName"].ToString())
                                           .Select(row => row.Field<String>("MonthFullName"));


                        var itemname = countryIDs.ToList();
                        foreach (string value in itemname)
                        {

                            foreach (DataRow dr in dtSecondary.Rows)
                            {
                                string csta = dr["StateName"].ToString();
                                string psta = dr1["StateName"].ToString();

                                string cmonth = dr["MonthFullName"].ToString();
                                string pmonth = value;
                                if (cmonth.Equals(pmonth))
                                {
                                    if (csta.Equals(psta))
                                    {
                                        Random randonGen = new Random();
                                        System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        for (int ij = 0; ij < 1000000; ij++)
                                        {
                                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        }
                                        _as.Appearance.FillStyle.BackgroundColor = randomColor;

                                        _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                                        _as.LineAppearance.Width = 1;
                                        _as.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                        _as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                                        _as.MarkersAppearance.Size = 6;
                                        _as.MarkersAppearance.BorderColor = randomColor;
                                        _as.MarkersAppearance.BorderWidth = 2;
                                        _as.TooltipsAppearance.Color = System.Drawing.Color.White;

                                        CategorySeriesItem csi = new CategorySeriesItem();
                                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);



                                        // csi.BackgroundColor = randomColor;
                                        _as.SeriesItems.Add(csi);
                                    }
                                }
                            }
                        }
                        rhcSecondry.PlotArea.Series.Add(_as);
                    }

             


                //foreach (DataRow dr in dt.Rows)
                //{
                //    AxisItem ai = new AxisItem();
                //    ai.LabelText = dr["ShortMonthName"].ToString();
                //    AreaChart.PlotArea.XAxis.Items.Add(ai);
                //}

                foreach (DataRow dr in Smonth.Rows)
                {
                    AxisItem ai = new AxisItem();
                    ai.LabelText = dr["ShortMonthName"].ToString();
                    rhcSecondry.PlotArea.XAxis.Items.Add(ai);

                }
                rhcSecondry.Visible = true;
                dt.Rows.Clear();
                }
                else
                {
                    rhcSecondry.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblSales.InnerText = "";

                }

            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        #endregion

        #region

        private void fillTrendRegion()
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='REGION'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlTrendregion.DataSource = dt;
                ddlTrendregion.DataTextField = "AreaName";
                ddlTrendregion.DataValueField = "AreaId";
                ddlTrendregion.DataBind();
            }
        }

        protected void btnTrend_Click(object sender, EventArgs e)
        {
            lblSales.InnerText = "";
            TrendDiv.Style.Add("display", "block");
            TrendDashBoard.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            RadHtmlChart rhcTrend = new RadHtmlChart();
            RadDock dock = new RadDock();
            rhcTrend.PlotArea.Series.Clear();
            rhcTrend.PlotArea.XAxis.Items.Clear();
            dock.EnableViewState = false;
            dock.DockMode = DockMode.Default;
            dock.AutoPostBack = true;
            dock.RenderMode = RenderMode.Lightweight;
            dock.CommandsAutoPostBack = true;
            dock.EnableDrag = false;
            dock.Resizable = true;
            dock.CssClass = "linedock";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            string stateId = "";
            try
            {

                searchCriteria += string.Format("Where YearName={0} And Amount !=0 And StateId IS NOT NULL", ddlTrendYear.SelectedValue);
                //if (lstState.SelectedValue != string.Empty)
                //{
                //    foreach (ListItem item in lstState.Items)
                //    {

                //        item.Selected = true;
                //        state += item.Value + ",";
                //    }
                //    state = state.TrimStart(',').TrimEnd(',');
                //    searchCriteria += string.Format(" And StateId In({1})", state);
                //}
                string sqlstr = string.Format("Select AreaId From MastArea Where UnderId In({0})", Settings.DMInt32(ddlTrendregion.SelectedValue));
                DataTable dtstate = DbConnectionDAL.GetDataTable(CommandType.Text, sqlstr);
                foreach (DataRow dr in dtstate.Rows)
                {
                    stateId += dr["AreaId"] + ",";
                }
                stateId = stateId.TrimEnd(',');
                if (stateId != "")
                {
                    searchCriteria += " And StateId In(" + stateId + ")";
                }

                if (rblTrend.SelectedValue == "PG")
                {
                    sqlStr = "select Sum(Amount) as Amount,stateid,statename,ItemName,YearName,MonthFullName,ShortMonthName,MonthNo from (select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename,mi1.ItemId,mi1.ItemName,year(td1.Vdate) as YearName, year(td1.Vdate) As Year,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from TransOrder1 td1 left join mastparty mp on td1.partyid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid LEFT JOIN mastitem mi ON td1.ItemId=mi.ItemId LEFT JOIN mastitem mi1 ON mi1.ItemId=mi.Underid group by vg.stateid,vg.statename,mi1.ItemId,mi1.ItemName,td1.Vdate ) a \n" + searchCriteria +
                        "group by stateid,statename,ItemId,ItemName,YearName,MonthFullName,ShortMonthName,MonthNo order by statename,MonthNo";
                }

                else if (rblTrend.SelectedValue == "PS")
                {
                    sqlStr = "select Sum(Amount) as Amount,stateid,statename,ItemId,ItemName,YearName,MonthFullName,ShortMonthName,MonthNo from (select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename,ms.Id as ItemId,ms.Name as ItemName,year(td1.Vdate) as YearName, year(td1.Vdate) As Year,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from TransOrder1 td1 left join mastparty mp on td1.partyid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid LEFT JOIN mastitem mi ON td1.ItemId=mi.ItemId LEFT JOIN MastItemSegment ms ON ms.Id=mi.SegmentId  group by vg.stateid,vg.statename,ms.Id,ms.Name,td1.Vdate) a \n" + searchCriteria +
                        " group by stateid,statename,ItemId,ItemName,YearName,MonthFullName,ShortMonthName,MonthNo   order by statename,MonthNo";
                }
                else
                {
                    sqlStr = "select Sum(Amount) as Amount,stateid,statename,ItemId,ItemName,YearName,MonthFullName,ShortMonthName,MonthNo from (select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename,ms.Id as ItemId,ms.Name as ItemName,year(td1.Vdate) as YearName, year(td1.Vdate) As Year,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from TransOrder1 td1 left join mastparty mp on td1.partyid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid LEFT JOIN mastitem mi ON td1.ItemId=mi.ItemId LEFT JOIN MastItemClass ms ON ms.Id=mi.ClassId  group by vg.stateid,vg.statename,ms.Id,ms.Name,td1.Vdate )a \n" + searchCriteria +
                        "group by stateid,statename,ItemId,ItemName,YearName,MonthFullName,ShortMonthName,MonthNo order by statename,MonthNo";
                }

               
               

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);

                if (dt.Rows.Count > 0)
                {

                    DataTable stateName = dt.DefaultView.ToTable(true, "StateName");

                    DataTable month = dt.DefaultView.ToTable(true, "ShortMonthName");



                    string group = rblType.SelectedItem.Text.Replace(" ", string.Empty);
                   // string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode = "";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));

                    maxLavel = maxLavel + (maxLavel / 2);

                    decimal step = Math.Round(maxLavel / 20);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblTrend.InnerText = Convert.ToString(amt);

                    TrendDivTotal.Style.Add("Display", "block");
                    rhcTrend.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcTrend.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcTrend.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcTrend.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcTrend.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcTrend.PlotArea.XAxis.Reversed = false;
                    //rhcTrend.ChartTitle.Text = "Sales Disribution vs. Consumption -Region Wise DashBoard";
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    rhcTrend.PlotArea.YAxis.MaxValue = maxLavel;
                    rhcTrend.PlotArea.YAxis.MinValue = minLavel;
                    rhcTrend.PlotArea.YAxis.Step = step;
                    rhcTrend.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    rhcTrend.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcTrend.PlotArea.YAxis.MajorTickSize = 4;
                    rhcTrend.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                    rhcTrend.PlotArea.YAxis.Reversed = false;
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcTrend.PlotArea.YAxis.LabelsAppearance.Step = 1;

                    rhcTrend.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcTrend.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    //foreach (DataRow dr1 in stateName.Rows)
                    //{
                    //    AreaSeries _as = new AreaSeries();
                    //    _as.Name = dr1["StateName"].ToString();
                    //    IEnumerable<String> countryIDs =
                    //                       dt
                    //                       .AsEnumerable()
                    //                       .Where(row => row.Field<String>("StateName") == dr1["StateName"].ToString())
                    //                       .Select(row => row.Field<String>("MonthFullName"));


                    //    var itemname = countryIDs.ToList();
                    //    foreach (string value in itemname)
                    //    {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ColumnSeries _as = new ColumnSeries();
                        _as.Name = dr["ItemName"].ToString();
                        //string csta = dr["StateName"].ToString();
                        //string psta = dr1["StateName"].ToString();

                        //string cmonth = dr["MonthFullName"].ToString();
                        //string pmonth = value;
                        //if (cmonth.Equals(pmonth))
                        //{
                        //    if (csta.Equals(psta))
                        //    {
                        Random randonGen = new Random();
                        System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        _as.Appearance.FillStyle.BackgroundColor = randomColor;

                        _as.Stacked = false;
                        _as.Gap = 1.5;
                        _as.Spacing = 0.4;
                        _as.Appearance.FillStyle.BackgroundColor = randomColor;
                        _as.LabelsAppearance.DataFormatString = "{0}";
                        _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.OutsideEnd;
                        _as.TooltipsAppearance.DataFormatString = "{0}";
                        _as.TooltipsAppearance.Color = System.Drawing.Color.White;
                        CategorySeriesItem csi = new CategorySeriesItem();
                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);



                        // csi.BackgroundColor = randomColor;
                        _as.SeriesItems.Add(csi);
                        //    }
                        //    rhcTrend.PlotArea.Series.Add(_as);
                        //}
                        //    }
                        //}
                        rhcTrend.PlotArea.Series.Add(_as);
                    }

                    foreach (DataRow dr in month.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["ShortMonthName"].ToString();
                        rhcTrend.PlotArea.XAxis.Items.Add(ai);

                    }
              
                    dock.Dock(RadDockZone2);
                    dock.ContentContainer.Controls.Add(rhcTrend);
                    RadDockLayout1.Controls.Add(dock);
                    rhcTrend.Visible = true;
                    dt.Rows.Clear();
                }
                else
                {
                    rhcTrend.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblTrend.InnerText = "";
                }
               
            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        #endregion

        #region

        private void fillTransactionRegion()
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='REGION'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlTransRegion.DataSource = dt;
                ddlTransRegion.DataTextField = "AreaName";
                ddlTransRegion.DataValueField = "AreaId";
                ddlTransRegion.DataBind();
            }
            ddlTransRegion.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void btnTransRegion_Click(object sender, EventArgs e)
        {
            lblSales.InnerText = "";
            TransAmtZoneDiv.Style.Add("display", "block");
            TransAmtDashboard.Style.Add("display", "block");
            TransStateDiv.Style.Add("display", "none");
            mainDiv.Style.Add("display", "none");
            RadDock rdTransRegion = new RadDock();
            RadHtmlChart rhcTransRegion = new RadHtmlChart(); rhcTransRegion.CssClass = "customchart";
            //rhcTransRegion.Width = Unit.Percentage(100);
            //rhcTransRegion.Height = Unit.Pixel(500);
            rhcTransRegion.Width = Unit.Percentage(100);
            rhcTransRegion.Height = Unit.Pixel(500);
            rhcTransRegion.PlotArea.Series.Clear();
            rhcTransRegion.PlotArea.XAxis.Items.Clear();
           
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            string stateId = "";
            try
            {

                searchCriteria += string.Format("Where  year(td1.Vdate)={0} And Amount !=0 ", ddlTransYear.SelectedValue);
               
                if (ddlTransRegion.SelectedValue == "0")
                {
                    //searchCriteria = "";
                }
                else {
                    searchCriteria += string.Format(" And vg.regionName In('{0}')", ddlTransRegion.SelectedItem.Text);
                    
                }

                //string sqlstr = string.Format("Select AreaId From MastArea Where UnderId In({0})", Settings.DMInt32(ddlTransRegion.SelectedValue));
                //DataTable dtstate = DbConnectionDAL.GetDataTable(CommandType.Text, sqlstr);

                sqlStr = "select Sum(Amount) as Amount,regionName,MonthFullName,ShortMonthName,YearName,MonthNo from (select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.regionName,year(td1.Vdate) As Year,year(td1.Vdate) As YearName,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid " + searchCriteria + " group by vg.regionName,td1.Vdate) a \n" +
                        "group by regionName,MonthFullName,ShortMonthName,YearName,MonthNo order by a.regionName,a.MonthNo";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);

                if (dt.Rows.Count > 0)
                {
                    DataTable stateName = dt.DefaultView.ToTable(true, "regionName");

                    DataTable month = dt.DefaultView.ToTable(true, "ShortMonthName");

                    TransStateTotalDiv.Style.Add("display", "block");

                    string group = rblType.SelectedItem.Text.Replace(" ", string.Empty);
                    //string selectedMode = viewBy.SelectedItem.Text;
                    string selectedMode ="";
                    if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                    else selectedMode = "Amount";
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));
                    maxLavel = maxLavel + (maxLavel / 2);
                    decimal step = Math.Round(maxLavel / 20);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblTransRegion.InnerText = Convert.ToString(amt);

                    TransRegionDiv.Style.Add("display", "block");
                    rhcTransRegion.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcTransRegion.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcTransRegion.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcTransRegion.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcTransRegion.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcTransRegion.PlotArea.XAxis.Reversed = false;
                    //rhcTransRegion.ChartTitle.Text = "Transaction Amounts Zone Wise";
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    rhcTransRegion.PlotArea.YAxis.MaxValue = maxLavel;
                    rhcTransRegion.PlotArea.YAxis.MinValue = minLavel;
                    rhcTransRegion.PlotArea.YAxis.Step = step;
                    rhcTransRegion.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    rhcTransRegion.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcTransRegion.PlotArea.YAxis.MajorTickSize = 4;
                    rhcTransRegion.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                    rhcTransRegion.PlotArea.YAxis.Reversed = false;
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcTransRegion.PlotArea.YAxis.LabelsAppearance.Step = 1;

                    rhcTransRegion.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcTransRegion.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    foreach (DataRow dr1 in stateName.Rows)
                    {
                        LineSeries _as = new LineSeries();
                        _as.Name = dr1["regionName"].ToString();
                        IEnumerable<String> countryIDs =
                                           dt
                                           .AsEnumerable()
                                           .Where(row => row.Field<String>("regionName") == dr1["regionName"].ToString())
                                           .Select(row => row.Field<String>("MonthFullName"));


                        var itemname = countryIDs.ToList();
                        foreach (string value in itemname)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                string csta = dr["regionName"].ToString();
                                string psta = dr1["regionName"].ToString();

                                string cmonth = dr["MonthFullName"].ToString();
                                string pmonth = value;
                                if (cmonth.Equals(pmonth))
                                {
                                    if (csta.Equals(psta))
                                    {
                                        Random randonGen = new Random();
                                        System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        for (int ij = 0; ij < 1000000; ij++)
                                        {
                                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                        }
                                        _as.Appearance.FillStyle.BackgroundColor = randomColor;

                                        _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                                        _as.LineAppearance.Width = 1;
                                        _as.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                        _as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                                        _as.MarkersAppearance.Size = 6;
                                        _as.MarkersAppearance.BorderColor = randomColor;
                                        _as.MarkersAppearance.BorderWidth = 2;
                                        _as.TooltipsAppearance.Color = System.Drawing.Color.White;

                                        CategorySeriesItem csi = new CategorySeriesItem();
                                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);



                                        // csi.BackgroundColor = randomColor;
                                        _as.SeriesItems.Add(csi);
                                    }
                                }
                            }
                        }

                        rhcTransRegion.PlotArea.Series.Add(_as);
                    }

                    foreach (DataRow dr in month.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["ShortMonthName"].ToString();
                        rhcTransRegion.PlotArea.XAxis.Items.Add(ai);

                    }
                    rdTransRegion.Dock(rdzTransRegion);
                    rdTransRegion.Controls.Add(rhcTransRegion);

                    rdlTransRegion.Controls.Add(rdTransRegion);

                    rhcTransRegion.Visible = true;
                    dt.Rows.Clear();
                }
                    else
                {
                    rhcTransRegion.Visible = false;
                    TransStateTotalDiv.Style.Add("display", "none");
                    lblTransRegion.InnerText = "";
                }



            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        #endregion

        #region

        private void fillTransactionState(int Id)
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='State' And UnderId="+Id+"";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                lstTransState.DataSource = dt;
                lstTransState.DataTextField = "AreaName";
                lstTransState.DataValueField = "AreaId";
                lstTransState.DataBind();
            }
        }

        protected void btnTransState_Click(object sender, EventArgs e)
        {
            lblSales.InnerText = "";
            TransStateDiv.Style.Add("display", "block");
            TransStateDashBoard.Style.Add("display", "block");
            mainDiv.Style.Add("display", "none");
            TransAmtZoneDiv.Style.Add("display", "none");
            TransAmtDashboard.Style.Add("display", "none");
            

            RadHtmlChart rhcTransState = new RadHtmlChart();
            rhc.Height = Unit.Percentage(100);
            RadDock rdTransState = new RadDock();
        
            rhcTransState.PlotArea.Series.Clear();
            rhcTransState.PlotArea.XAxis.Items.Clear();
           
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            string stateId = "";
            try
            {

                searchCriteria += string.Format("Where year(td1.Vdate)={0} And Amount !=0 And StateId IS NOT NULL", ddlTransYear.SelectedValue);
                if (lstTransState.SelectedValue != string.Empty)
                {
                    foreach (ListItem item in lstTransState.Items)
                    {

                        item.Selected = true;
                        state += item.Value + ",";
                    }
                    state = state.TrimStart(',').TrimEnd(',');
                    searchCriteria += string.Format(" And StateId In({0})", state);
                }
                else
                {
                    string sqlstr = string.Format("Select AreaId From MastArea Where UnderId In({0})", Settings.DMInt32(ddlTransYear.SelectedValue));
                    DataTable dtstate = DbConnectionDAL.GetDataTable(CommandType.Text, sqlstr);
                    foreach (DataRow dr in dtstate.Rows)
                    {
                        stateId += dr["AreaId"] + ",";
                    }
                    stateId = stateId.TrimEnd(',');
                    if (stateId != "")
                    {
                        searchCriteria += " Where StateId In(" + stateId + ")";
                    }
                }
                sqlStr = "select Sum(Amount) as Amount,StateId,StateName,MonthFullName,ShortMonthName,YearName,MonthNo from (select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate, year(td1.Vdate) as YearName,vg.stateid,vg.statename, year(td1.Vdate) As Year,datename(month, td1.Vdate) MonthFullName,LEFT(DATENAME(MONTH,td1.Vdate),3) as ShortMonthName, DATEPART(m, td1.Vdate) as MonthNo from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid " + searchCriteria + "  group by vg.stateid,vg.statename,td1.Vdate)a \n" +
                        "group by StateName,MonthFullName,ShortMonthName,YearName,StateId,MonthNo order by a.StateName,a.MonthNo";

      

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);


                DataTable stateName = dt.DefaultView.ToTable(true, "StateName");

                DataTable month = dt.DefaultView.ToTable(true, "ShortMonthName");



                string group = rblType.SelectedItem.Text.Replace(" ", string.Empty);
                //string selectedMode = viewBy.SelectedItem.Text;
                string selectedMode = "";
                if (selectedMode.Equals("Quantity")) selectedMode = "Qty";
                else selectedMode = "Amount";
                decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));

                decimal step = Math.Round(maxLavel / 20);
                double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                lblSales.InnerText = Convert.ToString(amt);

                rhcTransState.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                rhcTransState.PlotArea.XAxis.AxisCrossingValue = 0;
                rhcTransState.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                rhcTransState.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                rhcTransState.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                rhcTransState.PlotArea.XAxis.Reversed = false;
               // rhcTransState.ChartTitle.Text = "State Wise Transaction Amount DashBoard";
                rhcTransState.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
               
                rhcTransState.PlotArea.Appearance.TextStyle.Margin ="1";
                rhcTransState.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                rhcTransState.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                rhcTransState.PlotArea.YAxis.LabelsAppearance.Step = 1;
                rhcTransState.PlotArea.YAxis.MaxValue = maxLavel;
                rhcTransState.PlotArea.YAxis.MinValue = minLavel;
                rhcTransState.PlotArea.YAxis.Step = step;
                rhcTransState.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                rhcTransState.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                rhcTransState.PlotArea.YAxis.MajorTickSize = 4;
                rhcTransState.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                rhcTransState.PlotArea.YAxis.Reversed = false;
                rhcTransState.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                rhcTransState.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                rhcTransState.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                rhcTransState.PlotArea.YAxis.LabelsAppearance.Step = 1;

                rhcTransState.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                rhcTransState.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                decimal margin =0;
                foreach (DataRow dr1 in stateName.Rows)
                {
                    LineSeries _as = new LineSeries();
                    _as.Name = dr1["StateName"].ToString();
                    IEnumerable<String> countryIDs =
                                       dt
                                       .AsEnumerable()
                                       .Where(row => row.Field<String>("StateName") == dr1["StateName"].ToString())
                                       .Select(row => row.Field<String>("MonthFullName"));


                    var itemname = countryIDs.ToList();
                    foreach (string value in itemname)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string csta = dr["StateName"].ToString();
                            string psta = dr1["StateName"].ToString();

                            string cmonth = dr["MonthFullName"].ToString();
                            string pmonth = value;
                          
                            if (cmonth.Equals(pmonth))
                            {
                                if (csta.Equals(psta))
                                {
                                    Random randonGen = new Random();
                                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                    for (int ij = 0; ij < 1000000; ij++)
                                    {
                                        randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                    }
                                    _as.Appearance.FillStyle.BackgroundColor = randomColor;

                                    _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                                    _as.LineAppearance.Width = 1;
                                    _as.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                    _as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                                    _as.MarkersAppearance.Size = 6;
                                    _as.MarkersAppearance.BorderColor = randomColor;
                                    _as.MarkersAppearance.BorderWidth = 2;
                                    _as.TooltipsAppearance.Color = System.Drawing.Color.White;
                                    _as.LabelsAppearance.TextStyle.Margin=Convert.ToString(margin);
                                  
                                    CategorySeriesItem csi = new CategorySeriesItem();
                                    if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);



                                    // csi.BackgroundColor = randomColor;
                                    _as.SeriesItems.Add(csi);
                                    margin =Convert.ToDecimal(margin)+Convert.ToDecimal(0.5);
                                }
                            }
                        }
                    }
                    rhcTransState.PlotArea.Series.Add(_as);
                }

                foreach (DataRow dr in month.Rows)
                {
                    AxisItem ai = new AxisItem();
                    ai.LabelText = dr["ShortMonthName"].ToString();
                    rhcTransState.PlotArea.XAxis.Items.Add(ai);

                }

             
                rdTransState.Controls.Add(rhcTransState);
                rdTransState.Dock(rdzTransState);
                rdlTransState.Controls.Add(rdTransState);

                rhcTransState.Visible = true;
                dt.Rows.Clear();

            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        #endregion

        #region
        public void fillVariance()
        {
            rrgOutletsTransacted.Pointer.Value = (decimal)290;
            rrgOutletsTransacted.Pointer.Cap.Size = (float)0.30;
            rrgOutletsTransacted.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgOutletsTransacted.Pointer.Color = System.Drawing.Color.Blue;


            rrgOutletsTransacted.Scale.Min = 0;


            rrgOutletsTransacted.Scale.Max = (decimal)300;
            rrgOutletsTransacted.Scale.MinorUnit = (decimal)0;
            rrgOutletsTransacted.Scale.MajorUnit = (decimal)100;


            rrgOutletsTransacted.Scale.MinorTicks.Visible = false;
            rrgOutletsTransacted.Scale.MajorTicks.Size = 10;


            rrgOutletsTransacted.Scale.Labels.Visible = true;
            rrgOutletsTransacted.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgOutletsTransacted.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgOutletsTransacted.Scale.Labels.Format = "{0}";
            rrgOutletsTransacted.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;


            GaugeRange gr1 = new GaugeRange();


            gr1.From = 0;
            gr1.To = (decimal)290;

            gr1.Color = System.Drawing.Color.Green;



            GaugeRange gr3 = new GaugeRange();
            gr3.From = (decimal)290;
            gr3.To = (decimal)300;
            gr3.Color = System.Drawing.Color.FromArgb(225, 0, 0);


            rrgOutletsTransacted.Scale.Ranges.Add(gr1);

            rrgOutletsTransacted.Scale.Ranges.Add(gr3);




            rrgProductiveTransactions.Pointer.Value = (decimal)0.7;
            rrgProductiveTransactions.Pointer.Cap.Size = (float)0.30;
            rrgProductiveTransactions.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgProductiveTransactions.Pointer.Color = System.Drawing.Color.Blue;


            rrgProductiveTransactions.Scale.Min = 0;


            rrgProductiveTransactions.Scale.Max = (decimal)1.2;
            rrgProductiveTransactions.Scale.MinorUnit = (decimal)0.5;
            rrgProductiveTransactions.Scale.MajorUnit = (decimal)0.8;


            rrgProductiveTransactions.Scale.MinorTicks.Visible = false;
            rrgProductiveTransactions.Scale.MajorTicks.Size = 10;


            rrgProductiveTransactions.Scale.Labels.Visible = true;
            rrgProductiveTransactions.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgProductiveTransactions.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgProductiveTransactions.Scale.Labels.Format = "P0";
            rrgProductiveTransactions.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            //Create new GaugeRange object
            GaugeRange gr4 = new GaugeRange();

            //Set the properties of the new object
            gr4.From = 0;
            gr4.To = (decimal)0.8;

            gr4.Color = System.Drawing.Color.Green;



            GaugeRange gr5 = new GaugeRange();
            gr5.From = (decimal)0.8;
            gr5.To = (decimal)1.2;
            gr5.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            //Add Gauge objects to the RadialGauge
            rrgProductiveTransactions.Scale.Ranges.Add(gr4);
            //rrgBuyers.Scale.Ranges.Add(gr2);
            rrgProductiveTransactions.Scale.Ranges.Add(gr5);
            //PHActiveOutlets.Controls.Add(rrgBuyers);




            rrgProductivity.Pointer.Value = (decimal)0.7;
            rrgProductivity.Pointer.Cap.Size = (float)0.30;
            rrgProductivity.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgProductivity.Pointer.Color = System.Drawing.Color.Blue;

            //Set Min and Max values of the Scale
            rrgProductivity.Scale.Min = 0;

            //In order the Max value to be displayed it should be multiple of the MajorUnit
            rrgProductivity.Scale.Max = (decimal)1.2;
            rrgProductivity.Scale.MinorUnit = (decimal)0.5;
            rrgProductivity.Scale.MajorUnit = (decimal)0.8;

            //Set Minor and Major Ticks properties
            rrgProductivity.Scale.MinorTicks.Visible = false;
            rrgProductivity.Scale.MajorTicks.Size = 10;

            //Set Scale Labels properties
            rrgProductivity.Scale.Labels.Visible = true;
            rrgProductivity.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgProductivity.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgProductivity.Scale.Labels.Format = "P0";
            rrgProductivity.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;


            GaugeRange gr6 = new GaugeRange();


            gr6.From = 0;
            gr6.To = (decimal)0.8;

            gr6.Color = System.Drawing.Color.Green;

            GaugeRange gr7 = new GaugeRange();
            gr7.From = (decimal)0.8;
            gr7.To = (decimal)1.2;
            gr7.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            rrgProductivity.Scale.Ranges.Add(gr6);

            rrgProductivity.Scale.Ranges.Add(gr7);



            rrgRemoteProductivity.Pointer.Value = (decimal)0.7;
            rrgRemoteProductivity.Pointer.Cap.Size = (float)0.30;
            rrgRemoteProductivity.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgRemoteProductivity.Pointer.Color = System.Drawing.Color.Blue;

            //Set Min and Max values of the Scale
            rrgRemoteProductivity.Scale.Min = 0;

            //In order the Max value to be displayed it should be multiple of the MajorUnit
            rrgRemoteProductivity.Scale.Max = (decimal)1.2;
            rrgRemoteProductivity.Scale.MinorUnit = (decimal)0.5;
            rrgRemoteProductivity.Scale.MajorUnit = (decimal)0.8;

            //Set Minor and Major Ticks properties
            rrgRemoteProductivity.Scale.MinorTicks.Visible = false;
            rrgRemoteProductivity.Scale.MajorTicks.Size = 10;

            //Set Scale Labels properties
            rrgRemoteProductivity.Scale.Labels.Visible = true;
            rrgRemoteProductivity.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgRemoteProductivity.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgRemoteProductivity.Scale.Labels.Format = "P0";
            rrgRemoteProductivity.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            GaugeRange gr8 = new GaugeRange();


            gr8.From = 0;
            gr8.To = (decimal)0.8;

            gr8.Color = System.Drawing.Color.Green;

            GaugeRange gr9 = new GaugeRange();
            gr9.From = (decimal)0.8;
            gr9.To = (decimal)1.2;
            gr7.Color = System.Drawing.Color.FromArgb(225, 0, 0);


            rrgRemoteProductivity.Scale.Ranges.Add(gr8);

            rrgRemoteProductivity.Scale.Ranges.Add(gr9);

        }
        public void fillReguge()
        {

            rrgActiveOutlets.Pointer.Value = (decimal)0.8;
            rrgActiveOutlets.Pointer.Cap.Size = (float)0.30;
            rrgActiveOutlets.Pointer.Cap.Color = System.Drawing.Color.Transparent;
            rrgActiveOutlets.Pointer.Color = System.Drawing.Color.Transparent;

            //Set Min and Max values of the Scale
            rrgActiveOutlets.Scale.Min = 0;

            //In order the Max value to be displayed it should be multiple of the MajorUnit
            rrgActiveOutlets.Scale.Max = (decimal)1.2;
            rrgActiveOutlets.Scale.MinorUnit = (decimal)0.5;
            rrgActiveOutlets.Scale.MajorUnit = (decimal)0.8;

            //Set Minor and Major Ticks properties
            rrgActiveOutlets.Scale.MinorTicks.Visible = false;
            rrgActiveOutlets.Scale.MajorTicks.Size = 10;

            //Set Scale Labels properties
            rrgActiveOutlets.Scale.Labels.Visible = true;
            rrgActiveOutlets.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgActiveOutlets.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgActiveOutlets.Scale.Labels.Format = "P0";
            rrgActiveOutlets.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            //Create new GaugeRange object
            GaugeRange gr1 = new GaugeRange();

            //Set the properties of the new object
            gr1.From = 0;
            gr1.To = (decimal)0.8;

            gr1.Color = System.Drawing.Color.Green;

            //GaugeRange gr2 = new GaugeRange();
            //gr2.From = (decimal)0.4;
            //gr2.To = (decimal)0.8;
            //gr2.Color = System.Drawing.Color.Yellow;

            GaugeRange gr3 = new GaugeRange();
            gr3.From = (decimal)0.8;
            gr3.To = (decimal)1.2;
            gr3.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            //Add Gauge objects to the RadialGauge
            rrgActiveOutlets.Scale.Ranges.Add(gr1);
            //rrgActiveOutlets.Scale.Ranges.Add(gr2);
            rrgActiveOutlets.Scale.Ranges.Add(gr3);
            //PHActiveOutlets.Controls.Add(rrgActiveOutlets);



            rrgBuyers.Pointer.Value = (decimal)0.7;
            rrgBuyers.Pointer.Cap.Size = (float)0.30;
            rrgBuyers.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgBuyers.Pointer.Color = System.Drawing.Color.Blue;

            //Set Min and Max values of the Scale
            rrgBuyers.Scale.Min = 0;

            //In order the Max value to be displayed it should be multiple of the MajorUnit
            rrgBuyers.Scale.Max = (decimal)1.2;
            rrgBuyers.Scale.MinorUnit = (decimal)0.5;
            rrgBuyers.Scale.MajorUnit = (decimal)0.8;

            //Set Minor and Major Ticks properties
            rrgBuyers.Scale.MinorTicks.Visible = false;
            rrgBuyers.Scale.MajorTicks.Size = 10;

            //Set Scale Labels properties
            rrgBuyers.Scale.Labels.Visible = true;
            rrgBuyers.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgBuyers.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgBuyers.Scale.Labels.Format = "P0";
            rrgBuyers.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            //Create new GaugeRange object
            GaugeRange gr4 = new GaugeRange();

            //Set the properties of the new object
            gr4.From = 0;
            gr4.To = (decimal)0.8;

            gr4.Color = System.Drawing.Color.Green;

            //GaugeRange gr2 = new GaugeRange();
            //gr2.From = (decimal)0.4;
            //gr2.To = (decimal)0.8;
            //gr2.Color = System.Drawing.Color.Yellow;

            GaugeRange gr5 = new GaugeRange();
            gr5.From = (decimal)0.8;
            gr5.To = (decimal)1.2;
            gr5.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            //Add Gauge objects to the RadialGauge
            rrgBuyers.Scale.Ranges.Add(gr4);
            //rrgBuyers.Scale.Ranges.Add(gr2);
            rrgBuyers.Scale.Ranges.Add(gr5);
            //PHActiveOutlets.Controls.Add(rrgBuyers);




            rrgOutletsNotVisited.Pointer.Value = (decimal)0.7;
            rrgOutletsNotVisited.Pointer.Cap.Size = (float)0.30;
            rrgOutletsNotVisited.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgOutletsNotVisited.Pointer.Color = System.Drawing.Color.Blue;

            //Set Min and Max values of the Scale
            rrgOutletsNotVisited.Scale.Min = 0;

            //In order the Max value to be displayed it should be multiple of the MajorUnit
            rrgOutletsNotVisited.Scale.Max = (decimal)1.2;
            rrgOutletsNotVisited.Scale.MinorUnit = (decimal)0.5;
            rrgOutletsNotVisited.Scale.MajorUnit = (decimal)0.8;

            //Set Minor and Major Ticks properties
            rrgOutletsNotVisited.Scale.MinorTicks.Visible = false;
            rrgOutletsNotVisited.Scale.MajorTicks.Size = 10;

            //Set Scale Labels properties
            rrgOutletsNotVisited.Scale.Labels.Visible = true;
            rrgOutletsNotVisited.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgOutletsNotVisited.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgOutletsNotVisited.Scale.Labels.Format = "P0";
            rrgOutletsNotVisited.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            //Create new GaugeRange object
            GaugeRange gr6 = new GaugeRange();

            //Set the properties of the new object
            gr6.From = 0;
            gr6.To = (decimal)0.8;

            gr6.Color = System.Drawing.Color.Green;

            //GaugeRange gr2 = new GaugeRange();
            //gr2.From = (decimal)0.4;
            //gr2.To = (decimal)0.8;
            //gr2.Color = System.Drawing.Color.Yellow;

            GaugeRange gr7 = new GaugeRange();
            gr7.From = (decimal)0.8;
            gr7.To = (decimal)1.2;
            gr7.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            //Add Gauge objects to the RadialGauge
            rrgOutletsNotVisited.Scale.Ranges.Add(gr6);
            //rrgOutletsNotVisited.Scale.Ranges.Add(gr2);
            rrgOutletsNotVisited.Scale.Ranges.Add(gr7);
            //PHActiveOutlets.Controls.Add(rrgOutletsNotVisited);
        }
        public void fillEll()
        {
            rrgOutletsVisited.Pointer.Value = (decimal)290;
            rrgOutletsVisited.Pointer.Cap.Size = (float)0.30;
            rrgOutletsVisited.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgOutletsVisited.Pointer.Color = System.Drawing.Color.Blue;


            rrgOutletsVisited.Scale.Min = 0;


            rrgOutletsVisited.Scale.Max = (decimal)300;
            rrgOutletsVisited.Scale.MinorUnit = (decimal)0;
            rrgOutletsVisited.Scale.MajorUnit = (decimal)100;


            rrgOutletsVisited.Scale.MinorTicks.Visible = false;
            rrgOutletsVisited.Scale.MajorTicks.Size = 10;


            rrgOutletsVisited.Scale.Labels.Visible = true;
            rrgOutletsVisited.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgOutletsVisited.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgOutletsVisited.Scale.Labels.Format = "{0}";
            rrgOutletsVisited.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;


            GaugeRange gr1 = new GaugeRange();


            gr1.From = 0;
            gr1.To = (decimal)290;

            gr1.Color = System.Drawing.Color.Green;



            GaugeRange gr3 = new GaugeRange();
            gr3.From = (decimal)290;
            gr3.To = (decimal)300;
            gr3.Color = System.Drawing.Color.FromArgb(225, 0, 0);


            rrgOutletsVisited.Scale.Ranges.Add(gr1);

            rrgOutletsVisited.Scale.Ranges.Add(gr3);




            rrgEProductiveTransactions.Pointer.Value = (decimal)0.7;
            rrgEProductiveTransactions.Pointer.Cap.Size = (float)0.30;
            rrgEProductiveTransactions.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgEProductiveTransactions.Pointer.Color = System.Drawing.Color.Blue;


            rrgEProductiveTransactions.Scale.Min = 0;


            rrgEProductiveTransactions.Scale.Max = (decimal)1.2;
            rrgEProductiveTransactions.Scale.MinorUnit = (decimal)0.5;
            rrgEProductiveTransactions.Scale.MajorUnit = (decimal)0.8;


            rrgEProductiveTransactions.Scale.MinorTicks.Visible = false;
            rrgEProductiveTransactions.Scale.MajorTicks.Size = 10;


            rrgEProductiveTransactions.Scale.Labels.Visible = true;
            rrgEProductiveTransactions.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgEProductiveTransactions.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgEProductiveTransactions.Scale.Labels.Format = "P0";
            rrgEProductiveTransactions.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            //Create new GaugeRange object
            GaugeRange gr4 = new GaugeRange();

            //Set the properties of the new object
            gr4.From = 0;
            gr4.To = (decimal)0.8;

            gr4.Color = System.Drawing.Color.Green;



            GaugeRange gr5 = new GaugeRange();
            gr5.From = (decimal)0.8;
            gr5.To = (decimal)1.2;
            gr5.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            //Add Gauge objects to the RadialGauge
            rrgEProductiveTransactions.Scale.Ranges.Add(gr4);
            //rrgBuyers.Scale.Ranges.Add(gr2);
            rrgEProductiveTransactions.Scale.Ranges.Add(gr5);
            //PHActiveOutlets.Controls.Add(rrgBuyers);




            rrgEProductivity.Pointer.Value = (decimal)0.7;
            rrgEProductivity.Pointer.Cap.Size = (float)0.30;
            rrgEProductivity.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgEProductivity.Pointer.Color = System.Drawing.Color.Blue;

            //Set Min and Max values of the Scale
            rrgEProductivity.Scale.Min = 0;

            //In order the Max value to be displayed it should be multiple of the MajorUnit
            rrgEProductivity.Scale.Max = (decimal)1.2;
            rrgEProductivity.Scale.MinorUnit = (decimal)0.5;
            rrgEProductivity.Scale.MajorUnit = (decimal)0.8;

            //Set Minor and Major Ticks properties
            rrgEProductivity.Scale.MinorTicks.Visible = false;
            rrgEProductivity.Scale.MajorTicks.Size = 10;

            //Set Scale Labels properties
            rrgEProductivity.Scale.Labels.Visible = true;
            rrgEProductivity.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgEProductivity.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgEProductivity.Scale.Labels.Format = "P0";
            rrgEProductivity.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;


            GaugeRange gr6 = new GaugeRange();


            gr6.From = 0;
            gr6.To = (decimal)0.8;

            gr6.Color = System.Drawing.Color.Green;

            GaugeRange gr7 = new GaugeRange();
            gr7.From = (decimal)0.8;
            gr7.To = (decimal)1.2;
            gr7.Color = System.Drawing.Color.FromArgb(225, 0, 0);

            rrgEProductivity.Scale.Ranges.Add(gr6);

            rrgEProductivity.Scale.Ranges.Add(gr7);



            rrgERemoteProductivity.Pointer.Value = (decimal)0.7;
            rrgERemoteProductivity.Pointer.Cap.Size = (float)0.30;
            rrgERemoteProductivity.Pointer.Cap.Color = System.Drawing.Color.Blue;
            rrgERemoteProductivity.Pointer.Color = System.Drawing.Color.Blue;

            //Set Min and Max values of the Scale
            rrgERemoteProductivity.Scale.Min = 0;

            //In order the Max value to be displayed it should be multiple of the MajorUnit
            rrgERemoteProductivity.Scale.Max = (decimal)1.2;
            rrgERemoteProductivity.Scale.MinorUnit = (decimal)0.5;
            rrgERemoteProductivity.Scale.MajorUnit = (decimal)0.8;

            //Set Minor and Major Ticks properties
            rrgERemoteProductivity.Scale.MinorTicks.Visible = false;
            rrgERemoteProductivity.Scale.MajorTicks.Size = 10;

            //Set Scale Labels properties
            rrgERemoteProductivity.Scale.Labels.Visible = true;
            rrgERemoteProductivity.Scale.Labels.Font = "15px Arial,Helvetica,sans-serif";
            rrgERemoteProductivity.Scale.Labels.Color = System.Drawing.Color.Black;
            rrgERemoteProductivity.Scale.Labels.Format = "P0";
            rrgERemoteProductivity.Scale.Labels.Position = Telerik.Web.UI.Gauge.ScaleLabelsPosition.Outside;

            GaugeRange gr8 = new GaugeRange();


            gr8.From = 0;
            gr8.To = (decimal)0.8;

            gr8.Color = System.Drawing.Color.Green;

            GaugeRange gr9 = new GaugeRange();
            gr9.From = (decimal)0.8;
            gr9.To = (decimal)1.2;
            gr7.Color = System.Drawing.Color.FromArgb(225, 0, 0);


            rrgERemoteProductivity.Scale.Ranges.Add(gr8);

            rrgERemoteProductivity.Scale.Ranges.Add(gr9);

        }
        #endregion
        #region
        private void getData()
        {
            txtDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
            if (!IsPostBack)
            {
                SqlDataSource1.SelectCommand = String.Format("select SMId,SMName from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1 and smid<> {0} order by smname", Settings.Instance.SMID);
                PieSeries _ps = new PieSeries();
                _ps.NameField = "Label";
                _ps.DataFieldY = "Value";
                _ps.ColorField = "Color";
                _ps.ExplodeField = "IsExploded";
                _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.Center;
                _ps.LabelsAppearance.DataField = "valabel";
                _ps.LabelsAppearance.TextStyle.Bold = true;
                _ps.LabelsAppearance.Color = System.Drawing.Color.White;
                PieChart1.PlotArea.Series.Add(_ps);

            }
            if (cmbPerson.SelectedValue != "" && txtDate.Text != null)
            {
                LoadStat2(PieChart1);
            }
        }
       
        public static string GetAttendanceStats(int smId, int day, int month, int year, string chartselector)
        {

            DateTime mDate1 = DateTime.Parse(day + "/" + month + "/" + year);
            string sql = "select  count(*) as Person, 'Active' as Status from TransVisit with (nolock) where vdate = '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' and SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " ) and IsNull(appstatus,'DSR') !='Reject' Union All select count(*) as Person, 'InActive' as Status from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' Union All select  SMId from TransLeaveRequest with (nolock) where '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' between FromDate and ToDate ) Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " ) and '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' between FromDate and ToDate  and IsNull(appstatus,'DSR') !='Reject'";

            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            List<workstatmodel> lt = new List<workstatmodel>();
            int cnt = 3;
            List<string> status = new List<string> { "Active", "InActive", "Leave" };

            bool IsExploded = false, noData = true;
            for (int i = 0; i < cnt; i++)
            {
                IsExploded = false;
                if (i < dt1.Rows.Count)
                {
                    if (chartselector == Convert.ToString(dt1.Rows[i]["Status"]).Trim())
                        IsExploded = true;
                    if (Convert.ToDouble(dt1.Rows[i]["Person"]) > 0)
                        noData = false;
                    lt.Add(new workstatmodel { Value = Convert.ToDouble(dt1.Rows[i]["Person"]), Label = Convert.ToString(dt1.Rows[i]["Status"]).Trim(), Color = GetColor(Convert.ToString(dt1.Rows[i]["Status"]).Trim()), IsExploded = IsExploded, valabel = Convert.ToString(dt1.Rows[i]["Status"]).Trim() + "-" + Convert.ToDouble(dt1.Rows[i]["Person"]) });
                    status.Remove(Convert.ToString(dt1.Rows[i]["Status"]).Trim());
                }
                else
                {
                    if (chartselector == status[0].Trim())
                        IsExploded = true;
                    lt.Add(new workstatmodel { Value = 0, Label = status[0], Color = GetColor(Convert.ToString(status[0]).Trim()), IsExploded = IsExploded, valabel = status[0] + "- 0" });
                    status.Remove(status[0]);
                }
            }
            ResponseModel lt1 = new ResponseModel { noData = noData, data = lt };
            return JsonConvert.SerializeObject(lt1);
        }

        private static string GetColor(string label)
        {
            string color = "";
            switch (label.ToLower())
            {
                case "active":
                    color = "#6abf8a";
                    break;
                case "inactive":
                    color = "#bf2235";
                    break;
                case "leave":
                    color = "#356abf";
                    break;
            }
            return color;
        }

        public class ResponseModel
        {
            public List<workstatmodel> data { get; set; }
            public bool noData { get; set; }
        }
        public class workstatmodel
        {
            public double Value { get; set; }
            public string Label { get; set; }
            public string Color { get; set; }
            public string valabel { get; set; }
            public bool IsExploded { get; set; }
        }

        private void LoadStat2(RadHtmlChart rhc)
        {

            DateTime dsrdate = Convert.ToDateTime(txtDate.Text);
            var dy = dsrdate.Day.ToString();
            var mn = dsrdate.Month.ToString();
            var yy = dsrdate.Year.ToString();
            //var ltobj1 = JsonConvert.DeserializeObject<ResponseModel>(GetAttendanceStats(cmbPerson.SelectedValue != "" ? Convert.ToInt16(cmbPerson.SelectedValue) : 0, RadDatePicker1.SelectedDate.Value.Day, RadDatePicker1.SelectedDate.Value.Month, RadDatePicker1.SelectedDate.Value.Year, chartselector.Value));
            var ltobj1 = JsonConvert.DeserializeObject<ResponseModel>(GetAttendanceStats(cmbPerson.SelectedValue != "" ? Convert.ToInt16(cmbPerson.SelectedValue) : 0,Convert.ToInt32(dy),Convert.ToInt32(mn), Convert.ToInt32(yy), chartselector.Value));
            var ltobj = ltobj1.data;
            List<workstatmodel> lt = (List<workstatmodel>)ltobj;

            rhc.DataSource = lt;
        }


        public static int setcolor(int i, int y, int z)
        {
            i = i - y + z;
            int loop = 0;
            while (i < 1 || i > 254)
            {
                loop++;
                if (i < 1)
                {
                    i = i + y + 12;
                }

                if (i > 254)
                {
                    i = (i - 253) / 3 + z;
                }
                if (loop > 3)
                {
                    i = 180;
                }
            }
            return i;
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            string selector = chartselector.Value;
            int smId = cmbPerson.SelectedValue == "" ? 0 : Convert.ToInt16(cmbPerson.SelectedValue);
            string sql = "";
            if (selector == "Active")
            {
                sql = "select c.VisId,a.SMId,a.SMName as [SalesPerson],a.Mobile,b.SMName as [ReportingPerson],b.Mobile as [ReportingPersonMobile] from TransVisit c with (nolock) left join mastsalesrep a on a.SMId = c.SMId left Join mastsalesrep b on b.SMId = a.UnderId where vdate = '" + txtDate.Text + "' and c.SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " ) and IsNull(appstatus,'DSR') !='Reject' ";
                levelGrid1.MasterTableView.DataKeyNames = new string[] { "SMId", "VisId" };

            }
            if (selector == "InActive")
            {
                sql = "select a.SMId,a.SMName as [SalesPerson],a.Mobile,b.SMName as [ReportingPerson],b.Mobile as [ReportingPersonMobile] from mastsalesrep  a with (nolock) left join mastsalesrep b on b.SMId=a.UnderId where a.SMId in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and a.active=1 and a.SMId<> " + smId + " and a.SMId not in (select SMId from TransVisit with (nolock) where vdate = '" + txtDate.Text + "' Union All select  SMId from TransLeaveRequest with (nolock) where '" + txtDate.Text + "' between FromDate and ToDate )";
                //levelGrid1.MasterTableView.DataKeyNames = new string[] { "SMId" };
                levelGrid1.MasterTableView.DataKeyNames = new string[] { };
            }
            if (selector == "Leave")
            {

                sql = "select a.SMId,b.SMName as [SalesPerson],b.Mobile as [Mobile],d.SMName as [ReportingPerson],d.Mobile as [RPMobile],NoOfDays as [Days],FromDate,ToDate,Reason,AppStatus as [Status],c.SMName as [ApprovedBy],AppRemark from TransLeaveRequest a with (nolock) left join mastsalesrep b on b.SMId = a.SMId left join mastsalesrep c on c.SMId = a.AppBySMId left join mastsalesrep d on d.SMId = b.UnderId where a.SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " ) and '" + txtDate.Text + "' between FromDate and ToDate  and IsNull(appstatus,'DSR') !='Reject'";
                //levelGrid1.MasterTableView.DataKeyNames = new string[] { "SMId" };
                levelGrid1.MasterTableView.DataKeyNames = new string[] { };
            }
            if (!String.IsNullOrEmpty(sql))
            {
                SqlDataSource3.SelectCommand = sql;
            }

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "focusgrid", "focusgrid();", true);
        }

        protected void levelGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string selector = chartselector.Value;
            string sql = "";
            sql = "SELECT * FROM MastSalesRep with (nolock) WHERE 1 = 0";
            switch (selector)
            {
                //case "Leave":
                //    {
                //       // sql = "select a.Mobile,a.SalesRepType,b.SMName as [ReportingPerson],b.Mobile as [RPMobile],b.SalesRepType as [RPSalesRepType]  from mastsalesrep a Left Join mastsalesrep b on b.SMId=a.UnderId where a.SMId=" + SMId;


                //        sql = "select b.PartyName as [Customer],a.OrderAmount as [OrderValue],a.Created_date as [OrderTime],c.AreaName as [Location] from TransOrder a with (nolock) left join MastParty b  with (nolock) on b.PartyId=a.PartyId left join MastArea c  with (nolock) on c.AreaId=a.AreaId where 1=0";

                //        break;
                //    }
                //case "InActive":
                //    {
                //        sql = "select a.SMName,a.Mobile,a.SalesRepType,b.SMName,b.Mobile,b.SalesRepType  from mastsalesrep a Left Join mastsalesrep b on a.SMId=b.UnderId where a.SMId=" + SMId;

                //        break;
                //    }
                case "Active":
                    {
                        string VisId = dataItem.GetDataKeyValue("VisId").ToString(), SMId = dataItem.GetDataKeyValue("SMId").ToString();
                        sql = "select b.PartyName as [Customer],a.OrderAmount as [OrderValue],a.Created_date as [OrderTime],c.AreaName as [Location] from TransOrder a with (nolock) left join MastParty b  with (nolock) on b.PartyId=a.PartyId left join MastArea c  with (nolock) on c.AreaId=a.AreaId where a.VisId=" + VisId + " and a.SMId=" + SMId;
                        SqlDataSource2.SelectCommand = sql;

                        break;
                    }
            }
        }

        protected void levelGrid1_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (chartselector.Value == "InActive" || chartselector.Value == "Leave")
            {
                if (e.Column.UniqueName == "ExpandColumn")
                    e.Column.Display = false;
            }
            else
            {
                if (e.Column.UniqueName == "ExpandColumn")
                    e.Column.Display = true;
            }
            if (e.Column.UniqueName == "SMId")
                e.Column.Display = false;
            if (e.Column.UniqueName == "SM Id")
                e.Column.Display = false;
            if (e.Column.UniqueName == "VisId")
                e.Column.Display = false;
            if (e.Column is GridBoundColumn && (e.Column as GridBoundColumn).DataTypeName == typeof(DateTime).FullName)
            {
                string dateFormat = "dd/MMM/yyyy";
                if (!String.IsNullOrEmpty(dateFormat))
                {
                    (e.Column as GridBoundColumn).DataFormatString = "{0:" + dateFormat + "}";
                }
            }
        }
       
         
        #endregion
        #region
        private void fillUnderUsers1()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                    ddlSalesPerson.DataSource = dv;
                    ddlSalesPerson.DataTextField = "SMName";
                    ddlSalesPerson.DataValueField = "SMId";
                    ddlSalesPerson.DataBind();
                    //Ankita - 20/may/2016- (For Optimization)
                    //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                    //DataTable dtRole = new DataTable();
                    //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                    //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                    string RoleTy = Settings.Instance.RoleType;
                    if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                    {
                        ddlSalesPerson.SelectedValue = Settings.Instance.SMID;
                    }
                }
                catch { }

            }
        }
      
         
          
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
           // ddlSalesPerson.Items.Clear();
           // txtVisitDate.Text = "";
            rgTLight.DataSource =null;
            foreach (ListItem item in ddlSalesPerson.Items)
            {
                if (item.Selected)
                {
                    smIDStr += item.Value + ",";
                }
            }
            smIDStr = smIDStr.TrimStart(',').TrimEnd(',');
            if (smIDStr != "")
            {
                search += " Where SMId In(" + smIDStr + ")";
            }
            search += " And vdate='" + txtVisitDate.Text + "' And Beat is not null";
            sqlStr = "select a.smid,max(a.vdate) as vdate,a.smname as SRName,a.beat as Beat,max(a.Mobile) as Mobile,max(a.totalparty) as totalparty,max(a.callsvisited) as callsVisited,max(a.retailerprocalls) as ProductiveCall,max(a.totalorder) as TotalOrder from (select vd.smid,vd.VDate,vd.Mobile,Vd.Level1 as SmName, vd.beat,vd.totalParty,vd.callsvisited,vd.RetailerproCalls,vd.Totalorder from [View_DSR_For_Dashboard] vd \n" + search +
                      ") a group by a.smid,a.smname,a.beat order by vdate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);
            rgTLight.DataSource = dt;
            rgTLight.DataBind();
            mainDiv.Style.Add("display", "none");
            TLightDiv.Style.Add("display", "block");
        }

        
        #endregion
        #region
        private void fillL3(string stateId)
        {
                try
                {
                    sqlStr = string.Format("select msr.SMId,SMName,mr.RoleType from MastSalesRep msr inner join MastRole mr on msr.RoleId=mr.RoleId where mr.RoleType in ('RegionHead','StateHead') And CityId In (Select CityId from viewgeo where stateid in ({0}))", stateId);
                    DataTable dt=DbConnectionDAL.GetDataTable(CommandType.Text,sqlStr);
                    lstL3.DataSource = dt;
                    lstL3.DataTextField = "SMName";
                    lstL3.DataValueField = "SMId";
                    lstL3.DataBind();
                   
                }
                catch { }

            
        }
        private void fillL2(string UnderId)
        {
            try
            {
                sqlStr = string.Format("select msr.SMId,SMName,mr.RoleType from MastSalesRep msr inner join MastRole mr on msr.RoleId=mr.RoleId where mr.RoleType in ('CityHead','DistrictHead') And UnderId In ({0})", UnderId);
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);
                lstL2.DataSource = dt;
                lstL2.DataTextField = "SMName";
                lstL2.DataValueField = "SMId";
                lstL2.DataBind();
            }
            catch { }
        }

        private void fillL1(string UnderId)
        {
            try
            {
                sqlStr = string.Format("select msr.SMId,SMName,mr.RoleType from MastSalesRep msr inner join MastRole mr on msr.RoleId=mr.RoleId where mr.RoleType in ('AreaIncharge') And UnderId In ({0})", UnderId);
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);
                lstL1.DataSource = dt;
                lstL1.DataTextField = "SMName";
                lstL1.DataValueField = "SMId";
                lstL1.DataBind();
            }
            catch { }
        }

        private void fillUPRegion()
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='REGION'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlUPRegion.DataSource = dt;
                ddlUPRegion.DataTextField = "AreaName";
                ddlUPRegion.DataValueField = "AreaId";
                ddlUPRegion.DataBind();
            }
        }
        private void fillUPState(int UnderId)
        {
            string str="";
            if(UnderId ==0) str = "select AreaId,AreaName from MastArea where AreaType='State' And UnderId in ("+Settings.DMInt32(ddlUPRegion.SelectedValue)+")";
            else str = "select AreaId,AreaName from MastArea where AreaType='State' And UnderId in (" + UnderId + ")";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                lstUPSate.DataSource = dt;
                lstUPSate.DataTextField = "AreaName";
                lstUPSate.DataValueField = "AreaId";
                lstUPSate.DataBind();
            }
        }
        protected void lstUPSate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stateId = "";
            lstL3.Items.Clear();
            foreach (ListItem item in lstUPSate.Items)
            {
                if (item.Selected)
                {
                    stateId += item.Value + ",";
                }
            }
            stateId = stateId.TrimStart(',').TrimEnd(',');
            this.fillL3(stateId);
            mainDiv.Style.Add("display", "none");
            UserPerformanceDiv.Style.Add("display", "block");
        }
        protected void lstL3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string UnderId = "";
            lstL2.Items.Clear();
            foreach (ListItem item in lstL3.Items)
            {
                if (item.Selected)
                {
                    UnderId += item.Value + ",";
                }
            }
            UnderId = UnderId.TrimStart(',').TrimEnd(',');
            this.fillL2(UnderId);
            mainDiv.Style.Add("display", "none");
            UserPerformanceDiv.Style.Add("display", "block");
        }

        protected void lstL2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string UnderId = "";
            lstL1.Items.Clear();
            foreach (ListItem item in lstL2.Items)
            {
                if (item.Selected)
                {
                    UnderId += item.Value + ",";
                }
            }
            UnderId = UnderId.TrimStart(',').TrimEnd(',');
            this.fillL1(UnderId);
            mainDiv.Style.Add("display", "none");
            UserPerformanceDiv.Style.Add("display", "block");
        }

        protected void ddlUPRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fillUPState(Settings.DMInt32(ddlUPRegion.SelectedValue));
            mainDiv.Style.Add("display", "none");
            UserPerformanceDiv.Style.Add("display", "block");
        }
        protected void btnUPUpdate_Click(object sender, EventArgs e)
        {
            string l3Id = "",l2Id="",l1Id="",stateId="",SMId="";
            ddlSalesPerson.Items.Clear();
            txtVisitDate.Text = "";
            rgTLight.DataSource = null;

            foreach (ListItem item in lstUPSate.Items)
            {
                if (item.Selected)
                {
                    stateId += item.Value + ",";
                }
            }
            stateId = stateId.TrimStart(',').TrimEnd(',');
            string str = string.Format("select SMID from MastSalesRep where CityId In (Select CityId from viewgeo where stateid in (" + stateId + "))");
            DataTable dtSMID = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            foreach (DataRow dr in dtSMID.Rows)
            {
                SMId += Convert.ToString(dr["SMID"]) + ",";
            }
            SMId = SMId.TrimStart(',').TrimEnd(',');

           

            foreach (ListItem item in lstL3.Items)
            {
                if (item.Selected)
                {
                    l3Id += item.Value + ",";
                }
            }
            l3Id = l3Id.TrimStart(',').TrimEnd(',');
          

            foreach (ListItem item in lstL2.Items)
            {
                if (item.Selected)
                {
                    l2Id += item.Value + ",";
                }
            }
            l2Id = l2Id.TrimStart(',').TrimEnd(',');
           

            
            foreach (ListItem item in lstL1.Items)
            {
                if (item.Selected)
                {
                    l1Id += item.Value + ",";
                }
            }
            l1Id = l1Id.TrimStart(',').TrimEnd(',');

            if (l1Id != "")
            {
                search += " Where SMId In(" + l1Id + ")";
            }


            if (l1Id == "")
            {
                if (l2Id != "")
                {
                    search += " Where SMId In(" + l2Id + ")";
                }
            }


            if (l2Id == "")
            {
                if (l3Id != "")
                {
                    search += " Where SMId In(" + l3Id + ")";
                }
            }

            if (l3Id == "")
            {
                if (SMId != "")
                {
                    search += " Where SMId In(" + SMId + ")";
                }
            }

            search += " And vdate='" + txtUPDate.Text + "' And Beat is not null";
            sqlStr = "select a.smid,max(a.vdate) as vdate,a.smname as SRName,a.beat as Beat,max(a.Mobile) as Mobile,max(a.totalparty) as totalparty,max(a.callsvisited) as callsVisited,max(a.retailerprocalls) as ProductiveCall,max(a.totalorder) as TotalOrder from (select vd.smid,vd.VDate,vd.Mobile,Vd.Level1 as SmName, vd.beat,vd.totalParty,vd.callsvisited,vd.RetailerproCalls,vd.Totalorder from [View_DSR_For_Dashboard] vd \n" + search +
                      ") a group by a.smid,a.smname,a.beat order by vdate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);
            rptUP.DataSource = dt;
            rptUP.DataBind();
            string Orders = Convert.ToString(dt.Compute("sum([totalparty])", string.Empty));
            string pc = Convert.ToString(dt.Compute("sum([ProductiveCall])", string.Empty));
            string tc = Convert.ToString(dt.Compute("sum([callsVisited])", string.Empty));
            lblUPOrders.Text = Orders;
            lblUPTC.Text ="TC :"+ tc;
            lblUPPC.Text ="PC :"+pc;
            mainDiv.Style.Add("display", "none");
            UserPerformanceDiv.Style.Add("display", "block");
        }
        #endregion
        #region
        private void fillZoneRegion()
        {
            string str = "select AreaId,AreaName from MastArea where AreaType='REGION'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlZoneRegion.DataSource = dt;
                ddlZoneRegion.DataTextField = "AreaName";
                ddlZoneRegion.DataValueField = "AreaId";
                ddlZoneRegion.DataBind();
            }
        }
        protected void btnZoneShow_Click(object sender, EventArgs e)
        {

            try
            {
               
                 ZoneDashboard.Style.Add("display", "block");
                 ZoneDiv.Style.Add("display", "block");
                 mainDiv.Style.Add("display", "none");

                searchCriteria += string.Format(" Where year(td1.Vdate)={0} ", ddlZoneYear.SelectedValue);
                searchCriteria += string.Format(" And StateId In(select AreaId from Mastarea where UnderId={0})",Settings.DMInt32(ddlZoneRegion.SelectedValue));
                sqlStr = "select sum (qty * rate) as Amount,max(td1.Vdate) as Vdate,vg.stateid,vg.statename from Transdistinv1 td1 left join mastparty mp on td1.distid=mp.partyid left join viewgeo vg on mp.cityid=vg.cityid \n" + searchCriteria +
                    "group by vg.stateid,vg.statename order by vg.statename";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sqlStr);

                RadHtmlChart rhcZone = new RadHtmlChart();
                RadDock rdZone = new RadDock();

                RadHtmlChart rhcPieZone = new RadHtmlChart();
                RadDock rdPieZone = new RadDock();

                RadHtmlChart rhcLineZone = new RadHtmlChart();
                RadDock rdLineZone = new RadDock();
                if (dt.Rows.Count > 0)
                {
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([Amount])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([Amount])", string.Empty));

                    maxLavel = maxLavel + (maxLavel / 2);

                    decimal step = Math.Round(maxLavel / 10);
                    double amt = Convert.ToDouble(dt.Compute("Sum([Amount])", string.Empty));
                    lblZoneShowTotal.InnerText = Convert.ToString(amt);
                    ZoneShowDiv.Style.Add("display", "block");
                    // RadHtmlChart rhcColumnChart = new RadHtmlChart();
                    // rhcColumnChart.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    // rhcColumnChart.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    // rhcColumnChart.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    // rhcColumnChart.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    // rhcColumnChart.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    // rhcColumnChart.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    // rhcColumnChart.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    // rhcColumnChart.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    // rhcColumnChart.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    // rhcColumnChart.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    //// rhcColumnChart.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    // rhcColumnChart.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    // rhcColumnChart.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    // rhcColumnChart.PlotArea.YAxis.TitleAppearance.Text = "Amount";
                    // // rhcColumnChart.PlotArea.YAxis.TitleAppearance.Text = viewBy.SelectedItem.Text;
                    // rhcColumnChart.PlotArea.XAxis.AxisCrossingValue = 0;
                    // rhcColumnChart.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    // rhcColumnChart.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    // rhcColumnChart.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    // rhcColumnChart.PlotArea.XAxis.Reversed = false;
                    // rhcColumnChart.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    //   rhcColumnChart.PlotArea.XAxis.TitleAppearance.Text = "State Name";
                    // // rhcColumnChart.PlotArea.XAxis.DataLabelsField = objTable.Rows[ax.xAxis][0].ToString();
                    // rhcColumnChart.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    // rhcColumnChart.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    // rhcColumnChart.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    // rhcColumnChart.ChartTitle.Text = "Zone";

                    // rhcColumnChart.PlotArea.YAxis.MaxValue = maxLavel;
                    // rhcColumnChart.PlotArea.YAxis.MinValue = 0;
                    // rhcColumnChart.PlotArea.YAxis.Step = step;

                    // rhcColumnChart.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    // rhcColumnChart.PlotArea.XAxis.LabelsAppearance.Step = 1;
                    // rhcColumnChart.PlotArea.Series.Clear();
                    // //  rhcColumnChart.Width = (dt.Rows.Count) * 55;
                    // LineSeries _cs = new LineSeries();
                    // //BarSeries _cs = new BarSeries();
                    // //    _cs.Name =ddlgroup.SelectedItem.Text;
                    // _cs.Stacked = false;
                    // //_cs.Gap = 1.5;
                    // //_cs.Spacing = 0.4;
                    // _cs.Appearance.FillStyle.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#d5a2bb");
                    // _cs.LabelsAppearance.DataFormatString = "Sales";
                    // //_cs.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.OutsideEnd;
                    // _cs.TooltipsAppearance.DataFormatString = "Sales";
                    // _cs.TooltipsAppearance.Color = System.Drawing.Color.White;

                    // Random randonGen = new Random();
                    // System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));


                    // foreach (DataRow dr in dt.Rows)
                    // {
                    //     CategorySeriesItem csi = new CategorySeriesItem();
                    //     if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                    //     for (int ij = 0; ij < 1000000; ij++)
                    //     {
                    //         randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    //     }
                    //     _cs.Appearance.FillStyle.BackgroundColor = randomColor;

                    //      csi.BackgroundColor = randomColor;
                    //     _cs.SeriesItems.Add(csi);


                    // }
                    // rhcColumnChart.PlotArea.Series.Add(_cs);

                    // foreach (DataRow dr in dt.Rows)
                    // {
                    //     AxisItem ai = new AxisItem();
                    //     ai.LabelText = dr["StateName"].ToString();
                    //     rhcColumnChart.PlotArea.XAxis.Items.Add(ai);
                    // }



                    // rdZone.Dock(rdzZone);
                    // rdZone.Controls.Add(rhcColumnChart);
                    // rdlZone.Controls.Add(rdZone);
                    // rhcColumnChart.Visible = true;
                    // dt.Rows.Clear();


                   

                    rhcPieZone.PlotArea.Series.Clear();
                    rhcPieZone.PlotArea.XAxis.Items.Clear();
                    rhcZone.PlotArea.Series.Clear();
                    rhcZone.PlotArea.XAxis.Items.Clear();
                    rhcLineZone.PlotArea.Series.Clear();
                    rhcLineZone.PlotArea.XAxis.Items.Clear();

                    rhcZone.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcZone.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    rhcZone.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    rhcZone.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    rhcZone.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcZone.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcZone.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcZone.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcZone.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcZone.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    // rhcZone.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    rhcZone.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    rhcZone.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    rhcZone.PlotArea.YAxis.TitleAppearance.Text = "Amount";

                    rhcZone.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcZone.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcZone.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcZone.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcZone.PlotArea.XAxis.Reversed = false;
                    rhcZone.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    // rhcZone.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

                    rhcZone.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    rhcZone.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    rhcZone.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                   // rhcZone.ChartTitle.Text = "Zone Wise Interactive Dashboard";

                    rhcZone.PlotArea.YAxis.MaxValue = maxLavel;
                    rhcZone.PlotArea.YAxis.MinValue = minLavel;
                    rhcZone.PlotArea.YAxis.Step = step;

                    rhcZone.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    rhcZone.PlotArea.XAxis.LabelsAppearance.Step = 1;



                    ColumnSeries cs = new ColumnSeries();
                    cs.Stacked = false;
                    cs.Gap = 1.5;
                    cs.Spacing = 0.4;
                    cs.Appearance.FillStyle.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#c5d291");
                    cs.LabelsAppearance.DataFormatString = "{0}";
                    cs.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.OutsideEnd;
                    cs.TooltipsAppearance.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#c5d291");
                    cs.TooltipsAppearance.DataFormatString = "{0}";
                    cs.LabelsAppearance.Color = System.Drawing.Color.Black;
                    Random randonGen = new Random();
                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    foreach (DataRow dr in dt.Rows)
                    {
                        CategorySeriesItem csi = new CategorySeriesItem();
                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        cs.Appearance.FillStyle.BackgroundColor = randomColor;

                        csi.BackgroundColor = randomColor;
                        cs.SeriesItems.Add(csi);

                    }

                    rhcZone.PlotArea.Series.Add(cs);

                    foreach (DataRow dr in dt.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["stateName"].ToString();
                        rhcZone.PlotArea.XAxis.Items.Add(ai);
                    }

                    rdZone.Dock(rdzZone);
                    rdZone.Controls.Add(rhcZone);
                    rdlZone.Controls.Add(rdZone);



                    rhcLineZone.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcLineZone.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    rhcLineZone.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    rhcLineZone.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    rhcLineZone.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcLineZone.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcLineZone.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcLineZone.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcLineZone.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcLineZone.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    // rhcLineZone.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    rhcLineZone.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    rhcLineZone.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    rhcLineZone.PlotArea.YAxis.TitleAppearance.Text = "Amount";

                    rhcLineZone.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcLineZone.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcLineZone.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcLineZone.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcLineZone.PlotArea.XAxis.Reversed = false;
                    rhcLineZone.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    // rhcLineZone.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

                    rhcLineZone.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    rhcLineZone.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    rhcLineZone.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    //rhcLineZone.ChartTitle.Text = "Zone Wise Interactive Dashboard";

                    rhcLineZone.PlotArea.YAxis.MaxValue = maxLavel;
                    rhcLineZone.PlotArea.YAxis.MinValue = minLavel;
                    rhcLineZone.PlotArea.YAxis.Step = step;

                    rhcLineZone.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    rhcLineZone.PlotArea.XAxis.LabelsAppearance.Step = 1;





                    BarSeries bs = new BarSeries();
                    bs.Stacked = false;
                    bs.Gap = 1.5;
                    bs.Spacing = 0.4;
                    bs.Appearance.FillStyle.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#c5d291");
                    bs.LabelsAppearance.DataFormatString = "{0}";
                    bs.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.BarColumnLabelsPosition.OutsideEnd;
                    bs.TooltipsAppearance.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#c5d291");
                    bs.TooltipsAppearance.DataFormatString = "{0}";
                    bs.LabelsAppearance.Color = System.Drawing.Color.Black;

                    foreach (DataRow dr in dt.Rows)
                    {
                        CategorySeriesItem csi = new CategorySeriesItem();
                        if (Convert.ToDecimal(dr["Amount"]) > 0) csi.Y = Convert.ToDecimal(dr["Amount"]);
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        bs.Appearance.FillStyle.BackgroundColor = randomColor;

                        csi.BackgroundColor = randomColor;
                        bs.SeriesItems.Add(csi);

                    }

                    rhcLineZone.PlotArea.Series.Add(bs);

                    foreach (DataRow dr in dt.Rows)
                    {
                        AxisItem ai = new AxisItem();
                        ai.LabelText = dr["stateName"].ToString();
                        rhcLineZone.PlotArea.XAxis.Items.Add(ai);
                    }
                    rdLineZone.Dock(rdzBarZone);
                    rdLineZone.Controls.Add(rhcLineZone);
                    rdlZone.Controls.Add(rdLineZone);



                    rhcPieZone.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcPieZone.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                    rhcPieZone.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                    rhcPieZone.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                    rhcPieZone.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcPieZone.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcPieZone.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    rhcPieZone.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcPieZone.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcPieZone.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    // rhcPieZone.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                    rhcPieZone.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    rhcPieZone.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                    rhcPieZone.PlotArea.YAxis.TitleAppearance.Text = "Amount";

                    rhcPieZone.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcPieZone.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcPieZone.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcPieZone.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcPieZone.PlotArea.XAxis.Reversed = false;
                    rhcPieZone.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                    // rhcPieZone.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

                    rhcPieZone.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                    rhcPieZone.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    rhcPieZone.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    //rhcPieZone.ChartTitle.Text = "Zone Wise Interactive Dashboard";

                    rhcPieZone.PlotArea.YAxis.MaxValue = maxLavel;
                    rhcPieZone.PlotArea.YAxis.MinValue = 0;
                    rhcPieZone.PlotArea.YAxis.Step = step;

                    rhcPieZone.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                    rhcPieZone.PlotArea.XAxis.LabelsAppearance.Step = 1;
                    rhcPieZone.PlotArea.Series.Clear();

                    PieSeries _ps = new PieSeries();
                    _ps.StartAngle = 90;
                    _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
                    _ps.LabelsAppearance.DataFormatString = "{0}";
                    _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
                    _ps.TooltipsAppearance.DataFormatString = "{0}";

                    int cnt = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        PieSeriesItem _psItem = new PieSeriesItem();
                        if (cnt == 0) _psItem.Exploded = true;
                        else _psItem.Exploded = false;
                        if (Convert.ToDecimal(dr["Amount"]) > 0) _psItem.Y = Convert.ToDecimal(dr["Amount"]);
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        _psItem.BackgroundColor = randomColor;

                        _psItem.Name = dr["stateName"].ToString();
                        _ps.SeriesItems.Add(_psItem);
                        cnt++;
                    }

                    rhcPieZone.PlotArea.Series.Add(_ps);

                    rdPieZone.Dock(rdzPieZone);
                    rdPieZone.Controls.Add(rhcPieZone);
                    rdlZone.Controls.Add(rdPieZone);
                    rhcPieZone.Visible = true;
                    dt.Rows.Clear();
                }
                else
                {
                    rhcPieZone.Visible = false;
                    rhcLineZone.Visible = false;
                    //Div6.InnerHtml = "No Record Found";
                    lblGrowthByRegionDiv.InnerText = "";
                }

            }
            catch { }
        }
        #endregion

        protected void OutletsCoverage_Click(object sender, EventArgs e)
        {
           // rrgActiveOutlets.Pointer.Value=500;
           //// rrgActiveOutlets.Pointer.Cap.Size="0.1";
           // rrgActiveOutlets.Scale.Min=400;
           //  rrgActiveOutlets.Scale.Max=600;
           ////   rrgActiveOutlets.Scale.MajorUnit="1";
           //   rrgActiveOutlets.Scale.Labels.Format="{0}";
           // GaugeRange gr=new GaugeRange();
           // gr.Color = System.Drawing.ColorTranslator.FromHtml("#8dcb2a");
           // gr.From = 400;
           // gr.To = 600;
           // rrgActiveOutlets.Scale.Ranges.Add(gr);
          
        }

    }
}