using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Telerik.Web.UI;
using System.Web.Services;
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class DistWiseODDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cmbPerson.SelectedIndexChanged += new EventHandler(cmbPerson_SelectedIndexChanged);
            
            if (!IsPostBack)
            {
                //cmbPerson.Attributes.Add("onchange", "updateStats();");
                txtTDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                txtFDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                SqlDataSource1.SelectCommand = String.Format("select SMId,SMName from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1 and smid<> {0} order by smname", Settings.Instance.SMID);
                LoadStat2(PieChart1);
                //PieSeries _ps = new PieSeries();
                //_ps.NameField = "Label";
                //_ps.DataFieldY = "Value";
                //_ps.ColorField = "Color";
                //_ps.ExplodeField = "IsExploded";
                //_ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
                //_ps.LabelsAppearance.DataField = "valabel";
                //_ps.LabelsAppearance.TextStyle.Bold = true;
                //_ps.LabelsAppearance.Color = System.Drawing.Color.Black;
               
                //PieChart1.PlotArea.Series.Add(_ps);
              
                //levelGrid1.Visible = false;

            }
            if (txtTDate.Text != null)
            {
               // LoadStat2(PieChart1);
            }
        }

        [WebMethod(EnableSession = true)]
        public static string GetAttendanceStats(int smId, int day, int month, int year, string chartselector)
        {
            ///levelGrid1.DataSource = null;

            //DateTime mDate1 = DateTime.Parse(day + "/" + month + "/" + year);
            //string sql = "select  count(*) as Person, 'Active' as Status from TransVisit with (nolock) where vdate = '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' and SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " ) and IsNull(appstatus,'DSR') !='Reject' Union All select count(*) as Person, 'InActive' as Status from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' Union All select  SMId from TransLeaveRequest with (nolock) where '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' between FromDate and ToDate ) Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + smId + ") and active=1 and smid<> " + smId + " ) and '" + mDate1.Date.ToString("dd/MMM/yyyy") + "' between FromDate and ToDate  and IsNull(appstatus,'DSR') !='Reject'";
            
            //DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            //List<workstatmodel> lt = new List<workstatmodel>();
            //int cnt = 3;
            //List<string> status = new List<string> { "Active", "InActive", "Leave" };

            //bool IsExploded = false, noData = true;
            //for (int i = 0; i < cnt; i++)
            //{
            //    IsExploded = false;
            //    if (i < dt1.Rows.Count)
            //    {
            //        if (chartselector == Convert.ToString(dt1.Rows[i]["Status"]).Trim())
            //            IsExploded = true;
            //        if (Convert.ToDouble(dt1.Rows[i]["Person"]) > 0)
            //            noData = false;
            //        lt.Add(new workstatmodel { Value = Convert.ToDouble(dt1.Rows[i]["Person"]), Label = Convert.ToString(dt1.Rows[i]["Status"]).Trim(), Color = GetColor(Convert.ToString(dt1.Rows[i]["Status"]).Trim()), IsExploded = IsExploded, valabel = Convert.ToString(dt1.Rows[i]["Status"]).Trim() + "-" + Convert.ToDouble(dt1.Rows[i]["Person"]) });
            //        status.Remove(Convert.ToString(dt1.Rows[i]["Status"]).Trim());
            //    }
            //    else
            //    {
            //        if (chartselector == status[0].Trim())
            //            IsExploded = true;
            //        lt.Add(new workstatmodel { Value = 0, Label = status[0], Color = GetColor(Convert.ToString(status[0]).Trim()), IsExploded = IsExploded, valabel = status[0] + "- 0" });
            //        status.Remove(status[0]);
            //    }
            //}
            //ResponseModel lt1 = new ResponseModel { noData = noData, data = lt };
          
            //if (noData == true) {
               
            //}
            //return JsonConvert.SerializeObject(lt1);
            return "od";
        }



        [WebMethod(EnableSession = true)]
        public static string GetDist(string FromDate, string ToDate, string chartselector)
        {
            ///levelGrid1.DataSource = null;

            DateTime fDate = DateTime.Parse(FromDate);
            DateTime tDate = DateTime.Parse(ToDate);
            string sql = "select mp.PartyId,mp.PartyName,dbo.fngetOrderAmount(mp.PartyId) as Amount from MastParty mp where mp.PartyId in (select UnderId from MastParty where PartyId in (select PartyId from TransOrder where VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and VDate<='" + tDate.ToString("dd/MMM/yyyy") + "' ) and UnderId<>0) and dbo.fngetOrderAmount(mp.PartyId) >0";

            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            List<workstatmodel> lt = new List<workstatmodel>();
            int cnt = 3;
            List<string> status = new List<string> { "Active", "InActive", "Leave" };

            bool IsExploded = false, noData = true;

           
            
            //IsExploded = false;
            //foreach (DataRow dr in dt1.Rows )
            //{
            //    if (chartselector == Convert.ToString(dr["PartyId"]))
            //        IsExploded = true;
            //        noData = false;
            //        lt.Add(new workstatmodel { Value = Convert.ToDouble(dr["Amount"]), Label = Convert.ToString(dr["PartyName"]), Color = GetColor(), IsExploded = IsExploded, valabel = Convert.ToString(dr["PartyName"]) });
               
            //}

           
            //ResponseModel lt1 = new ResponseModel { noData = noData, data = dt1 };

            //if (noData == true)
            //{

            //}
            return JsonConvert.SerializeObject(dt1);
        }

        protected void dummybtn_Click(object sender, EventArgs e)
        {
            PieChart1.Visible = true;
            levelGrid1.Visible = false;
            DateTime fDate = DateTime.Parse(txtFDate.Text);
            DateTime tDate = DateTime.Parse(txtTDate.Text);
            int df =Convert.ToInt32((tDate - fDate).TotalDays);
            if (df > 30)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Date Range Between One Month Only.');", true);

            }
            else
            {
                string sql = "select mp.PartyId,mp.PartyName,dbo.fngetOrderAmount(mp.PartyId) as Amount from MastParty mp where mp.PartyId in (select UnderId from MastParty where PartyId in (select PartyId from TransOrder where VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and VDate<='" + tDate.ToString("dd/MMM/yyyy") + "' ) and UnderId<>0) and dbo.fngetOrderAmount(mp.PartyId) >0 ";

                // string sql = "select mp.PartyId,mp.PartyName,dbo.fngetOrderAmount(mp.PartyId) as Amount from MastParty mp where mp.PartyId in (select UnderId from MastParty where PartyId in (select PartyId from TransOrder where VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and VDate<='" + tDate.ToString("dd/MMM/yyyy") + "' ) and UnderId<>0) and dbo.fngetOrderAmount(mp.PartyId) >0 And PartyId in (5170,5124,5479,83309,4984,79396,5425,5828,5047,5974,4947,19751,5439,5041,5239,5585,5562,5064,5133,4852,5184,4592,4586,4609,4635,5940,5276,9847,4572,4878,5270,5997,5605,5213,83315,5027,6023,83312,4695,5551,4835,4935,4812,4981,5104,4858,4998,6017,5236,4987,4643,4603,5777,4795,5536,5230,5894,4683,4849,5800,5279,5322,4781,5445,5204,5130,5153,5010,5994,83318,4620,5210,5024,4924,4669,4675,4775,5099,76815,4859,5617,5053,5546,5669,4844,4750,19738,5886,4704,5926,5826,5583,5348,5039,5062,19718,4876,5889,5866,5448,4784,5325,5354,4678,5603,19726,4584,4976,5305,4770,5262,4621,5162,4970,4601,4933,5242,5835,5689,4979)";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);

                PieChart1.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                PieChart1.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                PieChart1.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                PieChart1.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                PieChart1.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                PieChart1.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                PieChart1.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                PieChart1.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                PieChart1.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                PieChart1.PlotArea.YAxis.LabelsAppearance.Step = 1;
                // PieChart1.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                PieChart1.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                PieChart1.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                PieChart1.PlotArea.YAxis.TitleAppearance.Text = "Amount";

                PieChart1.PlotArea.XAxis.AxisCrossingValue = 0;
                PieChart1.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                PieChart1.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                PieChart1.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                PieChart1.PlotArea.XAxis.Reversed = false;
                PieChart1.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

                // PieChart1.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

                PieChart1.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                PieChart1.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                PieChart1.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                //PieChart1.ChartTitle.Text = "Zone Wise Interactive Dashboard";



                PieChart1.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                PieChart1.PlotArea.XAxis.LabelsAppearance.Step = 1;
                PieChart1.PlotArea.Series.Clear();

                PieSeries _ps = new PieSeries();
                _ps.StartAngle = 360;
                _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
                _ps.LabelsAppearance.DataFormatString = "{0} Lac";
                _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
                _ps.TooltipsAppearance.DataFormatString = "{0} Lac";
                _ps.ExplodeField = "IsExploded";
                int cnt = 0;
                foreach (DataRow dr in dt1.Rows)
                {
                    PieSeriesItem _psItem = new PieSeriesItem();
                    if (cnt == 0) _psItem.Exploded = false;
                    else _psItem.Exploded = false;
                    if (Convert.ToDecimal(dr["Amount"]) > 0)
                    {
                        decimal amount = Convert.ToDecimal(dr["Amount"]) / 100000;
                        _psItem.Y = amount;
                    }
                    Random randonGen = new Random();
                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    for (int ij = 0; ij < 1000000; ij++)
                    {
                        randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    }
                    _psItem.BackgroundColor = randomColor;

                    _psItem.Name = dr["PartyName"].ToString();
                    _ps.SeriesItems.Add(_psItem);
                    cnt++;
                }

                PieChart1.PlotArea.Series.Add(_ps);
            }
        }
      
        private static string GetColor()
        {
            string color = "";
            //switch (label.ToLower())
            //{
            //    case "active":
            //        color = "#6abf8a";
            //        break;
            //    case "inactive":
            //        color = "#bf2235";
            //        break;
            //    case "leave":
            //        color = "#356abf";
            //        break;
            //}
            Random randonGen = new Random();
            System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
            for (int ij = 0; ij < 1000000; ij++)
            {
                randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
            }
            color =Convert.ToString(randomColor);
            return color;
        }
        protected void txtFDate_TextChanged(object sender, EventArgs e)
         {
         
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

            DateTime dsrdate = Convert.ToDateTime(txtFDate.Text);
          
            PieChart1.Visible = true;
            DateTime fDate = DateTime.Parse(txtFDate.Text);
            DateTime tDate = DateTime.Parse(txtTDate.Text);
            string sql = "select mp.PartyId,mp.PartyName,dbo.fngetOrderAmount(mp.PartyId) as Amount from MastParty mp where mp.PartyId in (select UnderId from MastParty where PartyId in (select PartyId from TransOrder where VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and VDate<='" + tDate.ToString("dd/MMM/yyyy") + "' ) and UnderId<>0) and dbo.fngetOrderAmount(mp.PartyId) >0";
           // string sql = "select mp.PartyId,mp.PartyName,dbo.fngetOrderAmount(mp.PartyId) as Amount from MastParty mp where mp.PartyId in (select UnderId from MastParty where PartyId in (select PartyId from TransOrder where VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and VDate<='" + tDate.ToString("dd/MMM/yyyy") + "' ) and UnderId<>0) and dbo.fngetOrderAmount(mp.PartyId) >0 And PartyId in (5170,5124,5479,83309,4984,79396,5425,5828,5047,5974,4947,19751,5439,5041,5239,5585,5562,5064,5133,4852,5184,4592,4586,4609,4635,5940,5276,9847,4572,4878,5270,5997,5605,5213,83315,5027,6023,83312,4695,5551,4835,4935,4812,4981,5104,4858,4998,6017,5236,4987,4643,4603,5777,4795,5536,5230,5894,4683,4849,5800,5279,5322,4781,5445,5204,5130,5153,5010,5994,83318,4620,5210,5024,4924,4669,4675,4775,5099,76815,4859,5617,5053,5546,5669,4844,4750,19738,5886,4704,5926,5826,5583,5348,5039,5062,19718,4876,5889,5866,5448,4784,5325,5354,4678,5603,19726,4584,4976,5305,4770,5262,4621,5162,4970,4601,4933,5242,5835,5689,4979)";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);

            PieChart1.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
            PieChart1.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
            PieChart1.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
            PieChart1.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

            PieChart1.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
            PieChart1.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
            PieChart1.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
            PieChart1.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
            PieChart1.PlotArea.YAxis.LabelsAppearance.Skip = 0;
            PieChart1.PlotArea.YAxis.LabelsAppearance.Step = 1;
            // PieChart1.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
            PieChart1.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
            PieChart1.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
            PieChart1.PlotArea.YAxis.TitleAppearance.Text = "Amount";

            PieChart1.PlotArea.XAxis.AxisCrossingValue = 0;
            PieChart1.PlotArea.XAxis.Color = System.Drawing.Color.Black;
            PieChart1.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
            PieChart1.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
            PieChart1.PlotArea.XAxis.Reversed = false;
            PieChart1.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

            // PieChart1.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

            PieChart1.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
            PieChart1.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
            PieChart1.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
            //PieChart1.ChartTitle.Text = "Zone Wise Interactive Dashboard";



            PieChart1.PlotArea.XAxis.LabelsAppearance.Skip = 0;
            PieChart1.PlotArea.XAxis.LabelsAppearance.Step = 1;
            PieChart1.PlotArea.Series.Clear();

            PieSeries _ps = new PieSeries();
            _ps.StartAngle = 90;
            _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
            _ps.LabelsAppearance.DataFormatString = "{0} Lac";
            _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
            _ps.TooltipsAppearance.DataFormatString = "{0} Lac";

            int cnt = 0;
            foreach (DataRow dr in dt1.Rows)
            {
                PieSeriesItem _psItem = new PieSeriesItem();
                if (cnt == 0) _psItem.Exploded = true;
                else _psItem.Exploded = false;
                if (Convert.ToDecimal(dr["Amount"]) > 0)
                {
                    decimal amount = Convert.ToDecimal(dr["Amount"]) / 100000;
                    _psItem.Y = amount;
                }
                Random randonGen = new Random();
                System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                for (int ij = 0; ij < 1000000; ij++)
                {
                    randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                }
                _psItem.BackgroundColor = randomColor;

                _psItem.Name = dr["PartyName"].ToString();
                _ps.SeriesItems.Add(_psItem);
                cnt++;
            }

            PieChart1.PlotArea.Series.Add(_ps);

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

            levelGrid1.DataSource = null;
            levelGrid1.Visible = true;
            DateTime fDate = DateTime.Parse(txtFDate.Text);
            DateTime tDate = DateTime.Parse(txtTDate.Text);
            string selector = chartselector.Value;
            //int smId = cmbPerson.SelectedValue == "" ? 0 : Convert.ToInt16(cmbPerson.SelectedValue);
            string sql = "";
            if (chartselector.Value != "")
            {
                string strsql = "Select PartyId From MastParty where PartyName ='" + selector + "'";
                int partyId = DbConnectionDAL.GetIntScalarVal(strsql);
                sql = "select ot.*,MastArea.AreaName,mp.PartyName from TransOrder ot inner join MastArea on ot.AreaId=MastArea.AreaId inner join MastParty mp on ot.PartyId=mp.PartyId where ot.PartyId in (Select PartyId from MastParty where Underid=" + partyId + " ) And ot.VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and ot.VDate<='" + tDate.ToString("dd/MMM/yyyy") + "'   And OrderAmount >0 ";
                //levelGrid1.MasterTableView.DataKeyNames = new string[] { "SMId", "VisId" };

                if (!String.IsNullOrEmpty(sql))
                {
                    SqlDataSource3.SelectCommand = sql;
                }
            }




            PieChart1.Visible = true;
           
           // sql = "select mp.PartyId,mp.PartyName,dbo.fngetOrderAmount(mp.PartyId) as Amount from MastParty mp where mp.PartyId in (select UnderId from MastParty where PartyId in (select PartyId from TransOrder where VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and VDate<='" + tDate.ToString("dd/MMM/yyyy") + "' ) and UnderId<>0) and dbo.fngetOrderAmount(mp.PartyId) >0 And PartyId in (5170,5124,5479,83309,4984,79396,5425,5828,5047,5974,4947,19751,5439,5041,5239,5585,5562,5064,5133,4852,5184,4592,4586,4609,4635,5940,5276,9847,4572,4878,5270,5997,5605,5213,83315,5027,6023,83312,4695,5551,4835,4935,4812,4981,5104,4858,4998,6017,5236,4987,4643,4603,5777,4795,5536,5230,5894,4683,4849,5800,5279,5322,4781,5445,5204,5130,5153,5010,5994,83318,4620,5210,5024,4924,4669,4675,4775,5099,76815,4859,5617,5053,5546,5669,4844,4750,19738,5886,4704,5926,5826,5583,5348,5039,5062,19718,4876,5889,5866,5448,4784,5325,5354,4678,5603,19726,4584,4976,5305,4770,5262,4621,5162,4970,4601,4933,5242,5835,5689,4979)";
            sql = "select mp.PartyId,mp.PartyName,dbo.fngetOrderAmount(mp.PartyId) as Amount from MastParty mp where mp.PartyId in (select UnderId from MastParty where PartyId in (select PartyId from TransOrder where VDate>='" + fDate.ToString("dd/MMM/yyyy") + "' and VDate<='" + tDate.ToString("dd/MMM/yyyy") + "' ) and UnderId<>0) and dbo.fngetOrderAmount(mp.PartyId) >0 ";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);

            PieChart1.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
            PieChart1.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
            PieChart1.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
            PieChart1.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

            PieChart1.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
            PieChart1.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
            PieChart1.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
            PieChart1.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
            PieChart1.PlotArea.YAxis.LabelsAppearance.Skip = 0;
            PieChart1.PlotArea.YAxis.LabelsAppearance.Step = 1;
            // PieChart1.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
            PieChart1.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
            PieChart1.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
            PieChart1.PlotArea.YAxis.TitleAppearance.Text = "Amount";

            PieChart1.PlotArea.XAxis.AxisCrossingValue = 0;
            PieChart1.PlotArea.XAxis.Color = System.Drawing.Color.Black;
            PieChart1.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
            PieChart1.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
            PieChart1.PlotArea.XAxis.Reversed = false;
            PieChart1.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";

            // PieChart1.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

            PieChart1.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
            PieChart1.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
            PieChart1.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
            //PieChart1.ChartTitle.Text = "Zone Wise Interactive Dashboard";



            PieChart1.PlotArea.XAxis.LabelsAppearance.Skip = 0;
            PieChart1.PlotArea.XAxis.LabelsAppearance.Step = 1;
            PieChart1.PlotArea.Series.Clear();

            PieSeries _ps = new PieSeries();
            _ps.StartAngle = 90;
            _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
            _ps.LabelsAppearance.DataFormatString = "{0} Lac";
            _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
            _ps.TooltipsAppearance.DataFormatString = "{0} Lac";

            int cnt = 0;
            foreach (DataRow dr in dt1.Rows)
            {
                PieSeriesItem _psItem = new PieSeriesItem();
                if (cnt == 0) _psItem.Exploded = true;
                else _psItem.Exploded = false;
                if (Convert.ToDecimal(dr["Amount"]) > 0)
                {
                    decimal amount = Convert.ToDecimal(dr["Amount"]) / 100000;
                    _psItem.Y = amount;
                }
                Random randonGen = new Random();
                System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                for (int ij = 0; ij < 1000000; ij++)
                {
                    randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                }
                _psItem.BackgroundColor = randomColor;

                _psItem.Name = dr["PartyName"].ToString();
                _ps.SeriesItems.Add(_psItem);
                cnt++;
            }

            PieChart1.PlotArea.Series.Add(_ps);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "focusgrid", "focusgrid();", true);
        }
        protected void levelGrid1_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            int index = levelGrid1.CurrentPageIndex;
            string selector = chartselector.Value;
            //int smId = cmbPerson.SelectedValue == "" ? 0 : Convert.ToInt16(cmbPerson.SelectedValue);
            string sql = "";
            if (chartselector.Value != "")
            {
                string strsql = "Select PartyId From MastParty where PartyName ='" + selector + "'";
                int partyId = DbConnectionDAL.GetIntScalarVal(strsql);
                sql = "select ot.*,MastArea.AreaName,mp.PartyName from TransOrder ot inner join MastArea on ot.AreaId=MastArea.AreaId inner join MastParty mp on ot.PartyId=mp.PartyId where ot.PartyId in (Select PartyId from MastParty where Underid=" + partyId + ") And OrderAmount >0";
                //levelGrid1.MasterTableView.DataKeyNames = new string[] { "SMId", "VisId" };

                if (!String.IsNullOrEmpty(sql))
                {
                    SqlDataSource3.SelectCommand = sql;
                }
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "focusgrid", "focusgrid();", true);
        }
        protected void levelGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            string selector = chartselector.Value;
            //int smId = cmbPerson.SelectedValue == "" ? 0 : Convert.ToInt16(cmbPerson.SelectedValue);
            string sql = "";
            if (chartselector.Value != "")
            {
                string strsql = "Select PartyId From MastParty where PartyName ='" + selector + "'";
                int partyId = DbConnectionDAL.GetIntScalarVal(strsql);
                sql = "select ot.*,MastArea.AreaName,mp.PartyName from TransOrder ot inner join MastArea on ot.AreaId=MastArea.AreaId inner join MastParty mp on ot.PartyId=mp.PartyId where ot.PartyId in (Select PartyId from MastParty where Underid=" + partyId + ") ";
                //levelGrid1.MasterTableView.DataKeyNames = new string[] { "SMId", "VisId" };

                if (!String.IsNullOrEmpty(sql))
                {
                    SqlDataSource3.SelectCommand = sql;
                }
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "focusgrid", "focusgrid();", true);
        }
        protected void levelGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string selector = chartselector.Value;
            string sql = "";
            string OrdId = dataItem.GetDataKeyValue("OrdDocId").ToString();
            sql = "select ot1.*,mi.ItemName from TransOrder1 ot1 inner join MastItem mi on ot1.ItemId=mi.ItemId where OrdDocId='" + OrdId + "'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            e.DetailTableView.DataSource = dt;
            //SqlDataSource2.SelectCommand = sql;
         
        }
        protected void btnAttaBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DashBoard.aspx");
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

        protected void PieChart1_PreRender(object sender, EventArgs e)
        {
            //
        }
        protected void cmbPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
          //cmbPerson.Attributes.Add("onchange", "updateStats();");
        }
    }
}