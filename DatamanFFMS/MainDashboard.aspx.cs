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
    public partial class MainDashBoard : System.Web.UI.Page
    {
        string roleType = "", total = "";
        DataTable dtEmployee = null;
        string sql = "";
        string secondarySql = "";
        string PrimarySql = "";
        string UnApprovedSql = "";
        string OrderSql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FromDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                this.fillAllRecord();
            }
        }
        public void fillAllRecord()
        {
            roleType = Settings.Instance.RoleType;
            if (roleType.Equals("Admin"))
            {
                sql = "select  count(*) as Person, 'Present' as Status from TransVisit with (nolock) where vdate ='" + FromDate.Text + "' and IsNull(appstatus,'DSR') !='Reject' Union All  select count(*) as Person, 'Absent' as Status from mastsalesrep with (nolock) where smname not in ('.') and smid in  (select smid from mastsalesrepgrp with (nolock) ) and active=1  and  SMId not in (select  SMId from TransVisit with (nolock) where vdate ='" + FromDate.Text + "' Union All select  SMId  from TransLeaveRequest with (nolock) where '" + FromDate.Text + "' between FromDate and ToDate )  Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in  (select SMId from mastsalesrep with (nolock) where  active=1  ) and '" + FromDate.Text + "' between FromDate and ToDate  and  IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending')";
                PrimarySql = "select * from ((select count (distinct distid) as Amount,  'Collection' as Name, 1 as displayorder   from  distributercollection  where vdate ='" + FromDate.Text + "' ) union ( select sum (partyid)  as Amount,  'Failedvisit' as Name, 2 as displayorder   from  (select count (distinct a.partyid)  as partyid from temp_TransFailedVisit  A  RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where  a.vdate ='" + FromDate.Text + "'and b.partydist=1  union select   count (distinct a.partyid)   as partyid  from  TransFailedVisit   A  RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  where a.vdate ='" + FromDate.Text + "' and b.partydist=1) a) union (select isnull(sum (distid),0)  as Amount,  'Discussion' as Name, 3 as displayorder  from  (select count (distinct distid) as distid from  temp_transvisitdist  where vdate ='" + FromDate.Text + "' union select  count (distinct distid)  as distid  from  transvisitdist  where vdate ='" + FromDate.Text + "') a))b order by displayorder";
                secondarySql = "select * from ((select isnull(sum(totalorder),0) as Amount,  'TotalOrder' as Name, 1 as displayorder  from ( select count (distinct orddocid)  as totalorder from transorder  where vdate='" + FromDate.Text + "'  union  select count (distinct orddocid) as totalorder from temp_transorder  where vdate='" + FromDate.Text + "') a)  union ( select  isnull(sum (partyid),0) as Amount,'Demo' as Name, 3 as displayorder from (select count (distinct partyid)  partyid from temp_transdemo where vdate='" + FromDate.Text + "' union select count (distinct partyid)  partyid  from transdemo where vdate='" + FromDate.Text + "'  ) a) union  (select isnull(sum (partyid),0) as Amount,'FailedVisit' as name, 2 as displayorder   from  (select count (distinct A.PArtyid) as partyid from  temp_TransFailedVisit  A  RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where a.vdate ='" + FromDate.Text + "' and b.partydist=0  union  select  count (distinct A.PArtyid)  as partyid  from  TransFailedVisit   A  RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  where a.vdate ='" + FromDate.Text + "' and b.partydist=0) a)  union  (select isnull(sum (partyid),0) as Amount,'Competitor' as Name, 4 as displayorder from (select count (distinct partyid) partyid  from temp_transcompetitor where  vdate='" + FromDate.Text + "' union  select count (distinct partyid ) as  partyid from transcompetitor where vdate='" + FromDate.Text + "' ) a ) union (select IsNull(sum (partyid),0) as Amount,'Collection' as Name, 5 as displayorder from (select count (distinct partyid) partyid  from temp_transcollection where  vdate='" + FromDate.Text + "' union select  count (distinct partyid) partyid  from transcollection  where  vdate='" + FromDate.Text + "' ) a) )b  order by displayorder";
                UnApprovedSql = "select * from (select isNull(count (*) ,0) as Amount,  'Tour' as Name from TransTourPlanheader where convert(date,VDate) <='" + FromDate.Text + "' and appstatus='Pending' union select count (*) as Amount,  'BeatPlan' as Name from TransBeatPlan  where   convert(date,created_date) <='" + FromDate.Text + "' and appstatus='Pending' union select count(*)  as Amount,  'Leave' as Name from TransLeaveRequest where  VDate <='" + FromDate.Text + "' and appstatus='Pending' UNION   select count (*)  as Amount,  'DSR' as Name from TransVisit   where lock=1 and VDate <='" + FromDate.Text + "' and  appstatus is null) b";
                OrderSql = "select workhour,cast(workhour as varchar(20))+'.00' as WHour,convert(decimal(10, 2), sum(orderamount)/1000) as OrdAmt,SMId FROM (SELECT orderamount, datepart(hh,Created_date) as workhour,SMId  frOM transorder      where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') and orderamount<>0 union SELECT orderamount, datepart(hh,Created_date) as workhour,SMId  FROM temp_transorder where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') and orderamount<>0) a group by a.workhour,SMId";
            }
            else
            {
                sql = "select  count(*) as Person, 'Present' as Status from TransVisit with (nolock) where vdate ='" + FromDate.Text + "' and SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and IsNull(appstatus,'DSR') !='Reject' Union All select count(*) as Person, 'Absent' as Status from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = '" + FromDate.Text + "' Union All select  SMId from TransLeaveRequest with (nolock) where '" + FromDate.Text + "' between FromDate and ToDate ) Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and '" + FromDate.Text + "' between FromDate and ToDate  and IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending')";
                PrimarySql = "select * from (( select isnull(count (distinct distid) ,0)  as Amount,  'Collection' as Name, 1 as displayorder   from  distributercollection where smid in(select MastSalesRep.smid  from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in  (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 )  and  vdate ='" + FromDate.Text + "' union (select isnull(sum (partyid),0) as Amount,  'Failedvisit' as Name, 2 as displayorder from  (select count (distinct a.partyid)  as partyid from  temp_TransFailedVisit A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where a.vdate ='" + FromDate.Text + "' and b.partydist=1 and a. smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and a.vdate ='" + FromDate.Text + "'  union select count (distinct a.partyid)  as partyid from TransFailedVisit  A 	 RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where a.vdate ='" + FromDate.Text + "' and b.partydist=1  and a.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 ) and a.vdate ='" + FromDate.Text + "') c)  union (select IsNull(sum (distid),0)   as Amount, 'Discussion'  as Name, 3 as displayorder from  (select isnull(count (distinct distid) ,0) as distid  from  temp_transvisitdist  where smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in   (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + FromDate.Text + "' union  select  isnull(count (distinct distid) ,0)  as distid  from transvisitdist where smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + FromDate.Text + "') a))) f order by displayorder";
                secondarySql = " select * from ((select isnull(sum(partyid),0) as Amount,  'TotalOrder' as Name, 1 as displayorder from ( select count (distinct orddocid)  as partyid from transorder where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + FromDate.Text + "' union select count (distinct orddocid) as totalorder from temp_transorder  where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp   where MainGrp in (" + Settings.Instance.SMID + "))) and vdate='" + FromDate.Text + "') a) union select sum (partyid) as Amount,'Demo' as Name, 3 as displayorder from (select count (distinct partyid)  partyid from temp_transdemo where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + FromDate.Text + "' union select count (distinct partyid)   partyid from transdemo where smid in (select distinct smid from MastSalesRepGrp where smid in(select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + "))) and vdate='" + FromDate.Text + "' ) a union select IsNull(sum (partyid),0) as Amount,'FailedVisit' as name, 2 as displayorder from  (select count (distinct A.PArtyid) as partyid from  temp_TransFailedVisit  A RIGHT JOIN MASTPARTY  B ON A.PArtyid=b.partyid   where  a.vdate ='" + FromDate.Text + "' and b.partydist=0 and a. smid in (select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp  where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 ) and a.vdate ='" + FromDate.Text + "' union select  count (distinct A.PArtyid) as partyid  from TransFailedVisit  A  RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  where a.vdate ='" + FromDate.Text + "' and b.partydist=0 and a.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and   level>= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 )  and  a.vdate ='" + FromDate.Text + "') a   union  (select IsNull(sum (partyid),0) as Amount,'Competitor' as Name, 4 as displayorder from (select count (distinct partyid) partyid  from temp_transcompetitor  where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + FromDate.Text + "'  union select count (distinct partyid) partyid   from transcompetitor where smid in (select distinct smid from MastSalesRepGrp where smid in(select smid from MastSalesRepGrp  where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + FromDate.Text + "'  ) a  )  union   (select IsNull(sum (partyid),0) as Amount,'Collection' as Name, 5 as displayorder from (select count (distinct partyid) partyid  from temp_transcollection where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + FromDate.Text + "' union select  count (distinct partyid) partyid  from transcollection  where smid in (select distinct smid from MastSalesRepGrp where smid in(select smid from MastSalesRepGrp  where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate='" + FromDate.Text + "' ) a  ))b order by displayorder";
                UnApprovedSql = "select * from (select  isNull(count (*) ,0) as Amount,  'Leave' as Name from TransLeaveRequest where VDate <='" + FromDate.Text + "' and appstatus='Pending' and smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp   in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp   where MainGrp in ( " + Settings.Instance.SMID + " ))) union  select count (*) as Amount,  'BeatPlan' as Name  from TransBeatPlan  where convert(date,created_date) <='" + FromDate.Text + "' and appstatus='Pending' and smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp   in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in ( " + Settings.Instance.SMID + " ))) union  select count (*) as Amount,  'Tour' as Name from TransTourPlanheader  where convert(date,VDate) <='" + FromDate.Text + "' and appstatus='Pending' and smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in ( " + Settings.Instance.SMID + " ))) union select count (*) as Amount,  'DSR' as Name from TransVisit  where lock=1 and VDate <='" + FromDate.Text + "' and appstatus is null and smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in ( " + Settings.Instance.SMID + " )))) b";
                OrderSql = "select workhour,cast(workhour as varchar(20))+'.00' as WHour,convert(decimal(10, 2), sum(orderamount)/1000) as OrdAmt,SMId FROM (SELECT orderamount, datepart(hh,Created_date) as workhour,SMId  frOM transorder      where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') and orderamount<>0 union SELECT orderamount, datepart(hh,Created_date) as workhour,SMId  FROM temp_transorder where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') and orderamount<>0) a where  smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) group by a.workhour,SMId";
            }

            dtEmployee = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dtEmployee.Rows.Count > 0)
            {
                string filter = "Status='Present'";
                DataRow[] drP = dtEmployee.Select(filter);
                lblPresent.InnerText = Convert.ToString(drP[0]["Person"]);
                filter = "Status='Absent'";
                DataRow[] drA = dtEmployee.Select(filter);
                lblAbsent.InnerText = Convert.ToString(drA[0]["Person"]);
                filter = "Status='Leave'";
                DataRow[] drL = dtEmployee.Select(filter);
                lblLeave.InnerText = Convert.ToString(drL[0]["Person"]);
                total = Convert.ToString(dtEmployee.Compute("Sum(Person)", ""));
                lblTotal.InnerText = total;
            }
            LoadStat2(PieChart1, PrimarySql);
            LoadStatSecondary(PieChart2, secondarySql);
            fillUnApproved(PieChart3, UnApprovedSql);
            this.LoadOrder(OrderSql);
        }
        [WebMethod(EnableSession = true)]
        public static List<trafficSourceData> getTrafficSourceData()
        {
            List<trafficSourceData> t = new List<trafficSourceData>();
            string[] arrColor = new string[] { "#231F20", "#FFC200", "#F44937", "#16F27E", "#FC9775", "#5A69A6" };


            string myQuery = "select  count(*) as Person, 'Present' as Status from TransVisit with (nolock) where vdate = '" + DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy") + "' and SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and IsNull(appstatus,'DSR') !='Reject' Union All select count(*) as Person, 'Absent' as Status from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = '" + DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy") + "' Union All select  SMId from TransLeaveRequest with (nolock) where '" + DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy") + "' between FromDate and ToDate ) Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and '" + DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy") + "' between FromDate and ToDate  and IsNull(appstatus,'DSR') !='Reject'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, myQuery);
            foreach (DataRow dr in dt.Rows)
            {
                int counter = 0;
                trafficSourceData tsData = new trafficSourceData();
                tsData.value = dr["Person"].ToString();
                tsData.label = dr["Status"].ToString();
                Random randonGen = new Random();
                System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                for (int ij = 0; ij < 1000000; ij++)
                {
                    randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                }
                //tsData.color = arrColor[counter];
                tsData.color = Convert.ToString(randomColor);
                t.Add(tsData);
                counter++;
            }

            return t;
        }

        [WebMethod]
        public static void MyMethod(string Param1)
        {
            try
            {
                //MainDashBoard md = new MainDashBoard();
                //md.getData(Param1);
            }
            catch (Exception)
            {
                throw;
            }
        } 
        public class trafficSourceData
        {
            public string label { get; set; }
            public string value { get; set; }
            public string color { get; set; }
            public string hightlight { get; set; }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            // BindGrid();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DashBoard.aspx");
        }

       
        private void LoadStat2(RadHtmlChart rhc, string sql)
        {

            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            PieChart1.PlotArea.Series.Clear();
            PieChart1.PlotArea.XAxis.Items.Clear();
            if (dt1.Rows.Count > 0)
            {
                PieChart1.Visible = true;
                PieChart1.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                PieChart1.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                PieChart1.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                PieChart1.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                PieChart1.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Right;
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
                PieChart1.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = Unit.Pixel(120);
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
                _ps.LabelsAppearance.DataFormatString = "{0}";
                _ps.TooltipsAppearance.Visible = false;
                _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
                _ps.TooltipsAppearance.DataFormatString = "{0}";

                int cnt = 0;
                decimal amount = 0;
                foreach (DataRow dr in dt1.Rows)
                {

                    PieSeriesItem _psItem = new PieSeriesItem();

                    //if (cnt == 0) _psItem.Exploded = true;
                    //else _psItem.Exploded = false;
                    //if (Convert.ToDecimal(dr["Amount"]) > 0)
                    if (Convert.ToDecimal(dr["Amount"]) > 0)
                    {
                        amount = Convert.ToDecimal(dr["Amount"]);
                        // amount = amount+10;
                        _psItem.Y = amount;

                        Random randonGen = new Random();
                        System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        for (int ij = 0; ij < 1000000; ij++)
                        {
                            randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                        }
                        _psItem.BackgroundColor = randomColor;

                        _psItem.Name = dr["Name"].ToString();
                        _ps.SeriesItems.Add(_psItem);
                        cnt++;
                    }
                }

                PieChart1.PlotArea.Series.Add(_ps);
               PieChart1.Attributes.Add("Style", "Cursor:Pointer");
            }
            else
            { PieChart1.Visible = false;
            PieChart1.Attributes.Add("Style", "Cursor:Default");
            }

        }
        private void LoadStatSecondary(RadHtmlChart rhc, string sql)
        {

            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            PieChart2.PlotArea.Series.Clear();
            PieChart2.PlotArea.XAxis.Items.Clear();
            if (dt1.Rows.Count > 0)
            {
                PieChart2.Visible = true;
                PieChart2.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                PieChart2.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                PieChart2.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                PieChart2.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                PieChart2.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Right;
                PieChart2.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                PieChart2.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                PieChart2.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                PieChart2.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                PieChart2.PlotArea.YAxis.LabelsAppearance.Step = 1;
                // PieChart2.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                PieChart2.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                PieChart2.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                PieChart2.PlotArea.YAxis.TitleAppearance.Text = "Amount";

                PieChart2.PlotArea.XAxis.AxisCrossingValue = 0;
                PieChart2.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                PieChart2.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                PieChart2.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                PieChart2.PlotArea.XAxis.Reversed = false;
                PieChart2.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";
                PieChart2.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = Unit.Pixel(120);
                // PieChart2.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

                PieChart2.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                PieChart2.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                PieChart2.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                //PieChart2.ChartTitle.Text = "Zone Wise Interactive Dashboard";



                PieChart2.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                PieChart2.PlotArea.XAxis.LabelsAppearance.Step = 1;
                PieChart2.PlotArea.Series.Clear();

                PieSeries _ps = new PieSeries();
                _ps.StartAngle = 90;
                _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
                _ps.LabelsAppearance.DataFormatString = "{0}";
                _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
                _ps.TooltipsAppearance.Visible = false;
                _ps.TooltipsAppearance.DataFormatString = "{0}";

                int cnt = 0;
                decimal amount = 0;
                foreach (DataRow dr in dt1.Rows)
                {
                    PieSeriesItem _psItem = new PieSeriesItem();
                    //if (cnt == 0) _psItem.Exploded = true;
                    //else _psItem.Exploded = false;
                    //if (Convert.ToDecimal(dr["Amount"]) > 0)
                    if (Convert.ToDecimal(dr["Amount"]) > 0)
                    {
                        amount = Convert.ToDecimal(dr["Amount"]);
                        //amount = amount + 40;
                        _psItem.Y = amount;
                   
                    Random randonGen = new Random();
                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    for (int ij = 0; ij < 1000000; ij++)
                    {
                        randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    }
                    _psItem.BackgroundColor = randomColor;

                    _psItem.Name = dr["Name"].ToString();
                    _ps.SeriesItems.Add(_psItem);
                    cnt++;
                    }
                }

                PieChart2.PlotArea.Series.Add(_ps);
               
           PieChart2.Attributes.Add("Style", "Cursor:Pointer");
            }
            else
            { PieChart2.Visible = false;
            PieChart2.Attributes.Add("Style", "Cursor:Default");
            }
           

        }

        private void fillUnApproved(RadHtmlChart rhc, string sql)
        {
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            PieChart3.PlotArea.Series.Clear();
            PieChart3.PlotArea.XAxis.Items.Clear();
            
            if (dt1.Rows.Count > 0)
            {
                PieChart3.Visible = true;
                PieChart3.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                PieChart3.ChartTitle.Appearance.Align = Telerik.Web.UI.HtmlChart.ChartTitleAlign.Center;
                PieChart3.ChartTitle.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartTitlePosition.Top;
                PieChart3.ChartTitle.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                PieChart3.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Right;
                PieChart3.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;
                PieChart3.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                PieChart3.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                PieChart3.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                PieChart3.PlotArea.YAxis.LabelsAppearance.Step = 1;
                // PieChart3.PlotArea.XAxis.DataLabelsField = "" + selectedMode + "";
                PieChart3.PlotArea.YAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                PieChart3.PlotArea.YAxis.TitleAppearance.RotationAngle = 0;
                PieChart3.PlotArea.YAxis.TitleAppearance.Text = "Amount";

                PieChart3.PlotArea.XAxis.AxisCrossingValue = 0;
                PieChart3.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                PieChart3.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                PieChart3.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                PieChart3.PlotArea.XAxis.Reversed = false;
                PieChart3.PlotArea.XAxis.LabelsAppearance.DataFormatString = "{0}";
                PieChart3.PlotArea.XAxis.LabelsAppearance.TextStyle.FontSize = Unit.Pixel(120);
                // PieChart3.PlotArea.XAxis.TitleAppearance.Text = "" + group + "";

                PieChart3.PlotArea.XAxis.LabelsAppearance.RotationAngle = 0;
                PieChart3.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                PieChart3.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                //PieChart3.ChartTitle.Text = "Zone Wise Interactive Dashboard";



                PieChart3.PlotArea.XAxis.LabelsAppearance.Skip = 0;
                PieChart3.PlotArea.XAxis.LabelsAppearance.Step = 1;
                PieChart3.PlotArea.Series.Clear();

                DonutSeries _ps = new DonutSeries();
                _ps.StartAngle = 90;
                _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
                _ps.LabelsAppearance.DataFormatString = "{0}";
                _ps.TooltipsAppearance.Color = System.Drawing.Color.White;
                _ps.TooltipsAppearance.DataFormatString = "{0}";
                _ps.TooltipsAppearance.Visible = false;
                int cnt = 0;
                decimal amount = 0;
                foreach (DataRow dr in dt1.Rows)
                {
                    PieSeriesItem _psItem = new PieSeriesItem();
                    //if (cnt == 0) _psItem.Exploded = true;
                    //else _psItem.Exploded = false;
                    if (Convert.ToDecimal(dr["Amount"]) > 0)
                    {
                        amount = Convert.ToDecimal(dr["Amount"]);
                        _psItem.Y = amount;
                    }
                    Random randonGen = new Random();
                    System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    for (int ij = 0; ij < 1000000; ij++)
                    {
                        randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                    }
                    _psItem.BackgroundColor = randomColor;
                    
                    _psItem.Name = dr["Name"].ToString();
                    _ps.SeriesItems.Add(_psItem);
                    cnt++;
                }

                PieChart3.PlotArea.Series.Add(_ps);
            PieChart3.Attributes.Add("Style", "Cursor:Pointer");
            }
            else
            { PieChart3.Visible = false;
            PieChart3.Attributes.Add("Style", "Cursor:Default");
            }
           
        }

        private void LoadOrder(string sql)
        {
            rhcOrder.PlotArea.Series.Clear();
            rhcOrder.PlotArea.XAxis.Items.Clear();

         
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave = 0;
            string stateId = "";
            try
            {

                AreaSeries _as = new AreaSeries();
               // _as.Name = FromDate.Text;
                //sql = "select Sum(Qty) as Qty,Sum(Qty)*Rate as Amount from transorder1 where  VDate='" + FromDate.Text + "'  group by OrdDocId,Rate union all  select Sum(Qty) as Qty,Sum(Qty)*Rate as Amount from Temp_TransOrder1 where  VDate='" + FromDate.Text + "'  group by OrdDocId,Rate";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    rhcOrder.Visible = true;
                    decimal minLavel = Convert.ToDecimal(dt.Compute("min([OrdAmt])", string.Empty));
                    decimal maxLavel = Convert.ToDecimal(dt.Compute("max([OrdAmt])", string.Empty));

                    maxLavel = maxLavel + (maxLavel / 10);
                    decimal step = Math.Round((maxLavel / 10));
                    double amt = Convert.ToDouble(dt.Compute("Sum([OrdAmt])", string.Empty));
                  

                    rhcOrder.Appearance.FillStyle.BackgroundColor = System.Drawing.Color.Transparent;
                    rhcOrder.PlotArea.XAxis.AxisCrossingValue = 0;
                    rhcOrder.PlotArea.XAxis.Color = System.Drawing.Color.Black;
                    rhcOrder.PlotArea.XAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcOrder.PlotArea.XAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcOrder.PlotArea.XAxis.Reversed = false;
                    rhcOrder.PlotArea.XAxis.TitleAppearance.RotationAngle = 0;
                    rhcOrder.PlotArea.XAxis.TitleAppearance.Position = Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                    rhcOrder.PlotArea.XAxis.TitleAppearance.Text = "Time Span";
                    //  rhcOrder.ChartTitle.Text = "Growth By Region DashBoard";
                    rhcOrder.ChartTitle.Appearance.TextStyle.Color = System.Drawing.Color.Black;
                    rhcOrder.ChartTitle.Appearance.TextStyle.FontSize = Unit.Pixel(24);
                    rhcOrder.ChartTitle.Appearance.TextStyle.FontFamily = "Verdana";
                    rhcOrder.ChartTitle.Appearance.TextStyle.Margin = "11";
                    rhcOrder.ChartTitle.Appearance.TextStyle.Padding = "22";

                    rhcOrder.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0}";
                    //rhcOrder.PlotArea.YAxis.LabelsAppearance.Visible = false;
                    rhcOrder.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcOrder.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcOrder.PlotArea.YAxis.LabelsAppearance.Step = 1;
                    rhcOrder.PlotArea.YAxis.MaxValue = maxLavel;
                    rhcOrder.PlotArea.YAxis.MinValue = minLavel;
                    rhcOrder.PlotArea.YAxis.Step = step;
                    rhcOrder.PlotArea.YAxis.Color = System.Drawing.Color.Black;
                    rhcOrder.PlotArea.YAxis.MajorTickType = Telerik.Web.UI.HtmlChart.TickType.Outside;
                    rhcOrder.PlotArea.YAxis.MajorTickSize = 4;
                    rhcOrder.PlotArea.YAxis.MinorTickType = Telerik.Web.UI.HtmlChart.TickType.None;
                    rhcOrder.PlotArea.YAxis.Reversed = false;
                    rhcOrder.PlotArea.YAxis.LabelsAppearance.DataFormatString = "{0} K";
                    rhcOrder.PlotArea.YAxis.LabelsAppearance.RotationAngle = 0;
                    rhcOrder.PlotArea.YAxis.LabelsAppearance.Skip = 0;
                    rhcOrder.PlotArea.YAxis.LabelsAppearance.Step = 1;
                     rhcOrder.PlotArea.YAxis.TitleAppearance.RotationAngle=0;
                     rhcOrder.PlotArea.YAxis.TitleAppearance.Position= Telerik.Web.UI.HtmlChart.AxisTitlePosition.Center;
                     rhcOrder.PlotArea.YAxis.TitleAppearance.Text="Amount";
                    rhcOrder.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
                    rhcOrder.Legend.Appearance.BackgroundColor = System.Drawing.Color.Transparent;

                     
                    
                            

                        foreach (DataRow dr in dt.Rows)
                        {

                               
                           
                                Random randonGen = new Random();
                                System.Drawing.Color randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                for (int ij = 0; ij < 1000000; ij++)
                                {
                                    randomColor = System.Drawing.Color.FromArgb(randonGen.Next(0, 256), randonGen.Next(0, 256), randonGen.Next(0, 256));
                                }
                                _as.Appearance.FillStyle.BackgroundColor = randomColor;


                                //_as.LabelsAppearance.Visible = false;
                                _as.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.LineAndScatterLabelsPosition.Above;
                                _as.LineAppearance.Width = 1;
                                _as.MarkersAppearance.MarkersType = Telerik.Web.UI.HtmlChart.MarkersType.Circle;
                                _as.MarkersAppearance.BackgroundColor = System.Drawing.Color.White;
                                _as.MarkersAppearance.Size = 10;
                                _as.MarkersAppearance.BorderColor = randomColor;
                                _as.MarkersAppearance.BorderWidth =5;
                                _as.TooltipsAppearance.Color = System.Drawing.Color.White;

                                CategorySeriesItem csi = new CategorySeriesItem();
                                if (Convert.ToDecimal(dr["OrdAmt"]) > 0)
                                {
                                    decimal amount=Convert.ToDecimal(dr["OrdAmt"]);
                                    csi.Y = amount;
                                }
                                // csi.BackgroundColor = randomColor;
                                _as.SeriesItems.Add(csi);
                                rhcOrder.PlotArea.Series.Add(_as);
                                AxisItem ai = new AxisItem();
                                ai.LabelText = dr["workhour"].ToString();
                                rhcOrder.PlotArea.XAxis.Items.Add(ai);
                        }
                      
                   
                    //foreach (DataRow dr in month.Rows)
                    //{
                    //    AxisItem ai = new AxisItem();
                    //    ai.LabelText = dr["ShortMonthName"].ToString();
                    //    rhcOrder.PlotArea.XAxis.Items.Add(ai);

                    //}

                    dt.Rows.Clear();

                }
                else
                {
                    rhcOrder.Visible = false;
                }


            }
            catch { }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

    }
}