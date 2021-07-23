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
    public partial class NewDashboard4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cmbPerson.SelectedIndexChanged += new EventHandler(cmbPerson_SelectedIndexChanged);
            
            if (!IsPostBack)
            {
                cmbPerson.Attributes.Add("onchange", "updateStats();");
                txtDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
           
                //SqlDataSource1.SelectCommand = String.Format("select SMId,SMName from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1 and smid<> {0} order by smname", Settings.Instance.SMID);
                SqlDataSource1.SelectCommand = String.Format("select SMId,SMName from mastsalesrep ms LEFT JOIN MastRole mr ON ms.RoleId=mr.RoleId  where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1 and smid<> {0} AND mr.RoleType<>'AreaIncharge' order by smname", Settings.Instance.SMID);
          
                PieSeries _ps = new PieSeries();
                _ps.NameField = "Label";
                _ps.DataFieldY = "Value";
                _ps.ColorField = "Color";
                _ps.ExplodeField = "IsExploded";
                _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
                _ps.LabelsAppearance.DataField = "valabel";
                _ps.LabelsAppearance.TextStyle.Bold = true;
                _ps.LabelsAppearance.Color = System.Drawing.Color.Black;
               
                PieChart1.PlotArea.Series.Add(_ps);

            }
            if (cmbPerson.SelectedValue != "" && txtDate.Text != null)
            {
                LoadStat2(PieChart1);
            }
        }

        [WebMethod(EnableSession = true)]
        public static string GetAttendanceStats(int smId, int day, int month, int year, string chartselector)
        {
            ///levelGrid1.DataSource = null;

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
          
            if (noData == true) {
               
            }
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
            PieChart1.PlotArea.Series.Clear();
            PieChart1.PlotArea.XAxis.Items.Clear();
          //  PieSeries _ps = (PieSeries)PieChart1.PlotArea.Series[0];
           PieSeries _ps = new PieSeries();
            _ps.NameField = "Label";
            _ps.DataFieldY = "Value";
            _ps.ColorField = "Color";
            _ps.ExplodeField = "IsExploded";
            _ps.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
            _ps.LabelsAppearance.DataField = "valabel";
            _ps.LabelsAppearance.TextStyle.Bold = true;
            _ps.LabelsAppearance.Color = System.Drawing.Color.Black;
           
         //   _ps.SeriesItems[0].Exploded = true;
          
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
                        sql = "select b.PartyName as [Customer],a.OrderAmount as [OrderValue],a.Created_date as [OrderTime],c.AreaName as [Location],a.address as [Address] from TransOrder a with (nolock) left join MastParty b  with (nolock) on b.PartyId=a.PartyId left join MastArea c  with (nolock) on c.AreaId=a.AreaId where a.VisId=" + VisId + " and a.SMId=" + SMId + " UNION ALL select b.PartyName as [Customer],a.OrderAmount as [OrderValue],a.Created_date as [OrderTime],c.AreaName as [Location],a.address as [Address] from Temp_TransOrder a with (nolock) left join MastParty b  with (nolock) on b.PartyId=a.PartyId left join MastArea c  with (nolock) on c.AreaId=a.AreaId where a.VisId=" + VisId + " and a.SMId=" + SMId ; 
                        SqlDataSource2.SelectCommand = sql;

                        break;
                    }
            }
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