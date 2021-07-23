using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Web.Services;
using System.Threading;

namespace AstralFFMS
{
    public partial class NewDashboard3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlDataSource1.SelectCommand = String.Format("select SMId,SMName from mastsalesrep where smid in (select smid from mastsalesrepgrp where maingrp = {0}) and active=1 and smid<> {0} order by smname", Settings.Instance.SMID);
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
           // BindGrid();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DashBoard.aspx");
        }

        [WebMethod(EnableSession = true)]
        public static List<object> GetDetail(int smId)
        {
            string sql = "select a.SMName,a.DisplayName,b.DisplayName as Reporting,a.Email,a.Mobile,a.DeviceNo,c.AreaName as City, RTRIM(CAST(a.Address1+' '+a.Address2 as varchar)) as Address from mastsalesrep a LEFT Join mastsalesrep b on b.SMId=a.UnderId Inner join MastArea c on c.AreaId=a.CityId where a.SMId=" + smId;
            DataTable detail = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            return detail.Rows[0].ItemArray.ToList();
        }
        [WebMethod(EnableSession = true)]
        public static List<object> BindGrid(int smId, int Month, int year)
        {
            try
            {
                DateTime mDate1 = DateTime.Parse("01/" + Month + "/" + year);
                DateTime mDate2 = DateTime.Parse("01/" + Month + "/" + year).AddMonths(1).AddDays(-1);

                int dyasInMonth = DateTime.DaysInMonth(year, Month);
                DataTable gvData = new DataTable();
                for (int i = 1; i <= dyasInMonth; i++)
                {
                    gvData.Columns.Add(i.ToString());
                }

                string transTemp = "", strholiday = String.Empty;
                strholiday = "select distinct DAY(holiday_date) as day1 from View_Holiday where smid=" + smId + " and holiday_date between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59'";

                DataTable dt_holiday = DbConnectionDAL.GetDataTable(CommandType.Text, strholiday);
                DataView dvdt_holiday = new DataView(dt_holiday);
               // Session["holiday"] = dvdt_holiday;

                string strleave = "", status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;
                int dayloop = 0;

                dsrQuery = "select vl.SMId,vdate,ISNULL(AppStatus,'') as Status from TransVisit vl where  smid=" + smId + " and vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "' order by vl.SMId,vdate";

                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
                DataView dvdsrData = new DataView(dsrData);

                string No_Days = "select NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId =" + smId + " and (lr.FromDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59' OR lr.ToDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59') and lr.appstatus in ('Approve','Pending') order by smid,FromDate";
                DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
                DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);

                DataRow mDataRow = gvData.NewRow();

                foreach (DataRow dsr1 in dvdsrData.ToTable().Rows)
                {
                    DsrDate = Convert.ToDateTime(dsr1["vdate"].ToString()).Day.ToString().Trim();
                    if (dsr1["SMId"].ToString() == smId.ToString())
                    {
                        if (dsr1["Status"].ToString() == "Approve") { mDataRow[DsrDate] = "P"; }
                        if (dsr1["Status"].ToString() == "Reject") { mDataRow[DsrDate] = "E/R"; }
                        if (dsr1["Status"].ToString() == "") { mDataRow[DsrDate] = "E"; }
                    }
                }

                if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
                {
                    for (int cc = 0; cc < dvdt_NoOfdays.ToTable().Rows.Count; cc++)
                    {
                        if (dvdt_NoOfdays.ToTable().Rows[cc]["SMId"].ToString() == smId.ToString())
                        {
                            double daysNo = Convert.ToDouble(dvdt_NoOfdays.ToTable().Rows[cc]["NoOfDays"].ToString());
                            DateTime fromdate = new DateTime();
                            DateTime todate = new DateTime();
                            todate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["NoDays"].ToString());
                            fromdate = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());

                            dayloop = Convert.ToInt32((todate - fromdate).TotalDays);
                            for (int c1 = 0; c1 <= dayloop; c1++)
                            {
                                DateTime dateTime1 = new DateTime();
                                dateTime1 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                                dateTime1 = dateTime1.AddDays(c1);

                                if (dateTime1 > mDate2)
                                {
                                }
                                else if (dateTime1 < mDate1)
                                {
                                }
                                else
                                {
                                    DateTime dateTime2 = new DateTime();
                                    DateTime dateTime3 = new DateTime();
                                    dateTime2 = Convert.ToDateTime(dvdt_NoOfdays.ToTable().Rows[cc]["FromDate"].ToString());
                                    dateTime3 = Convert.ToDateTime(dateTime1.ToString());
                                    double NrOfDays = ((dateTime3 - dateTime2).TotalDays) * 2;

                                    strleave = dvdt_NoOfdays.ToTable().Rows[cc]["LeaveString"].ToString();
                                    status_pr = dvdt_NoOfdays.ToTable().Rows[cc]["AppStatus"].ToString();
                                    string str = strleave.Substring((Convert.ToInt32(NrOfDays)), 2);

                                    foreach (DataRow dsr in dvdsrData.ToTable().Rows)
                                    {
                                        if (dsr["SMId"].ToString() == smId.ToString())
                                        {
                                            if (dateTime3.Day.ToString().Trim() == Convert.ToDateTime(dsr["vdate"].ToString()).Day.ToString().Trim())
                                            {
                                                if (dsr["Status"].ToString() == "Approve")
                                                { ls_val += "P"; }
                                                if (dsr["Status"].ToString() == "Reject")
                                                { ls_val += "E/R"; }
                                                if (dsr["Status"].ToString() == "")
                                                { ls_val += "E"; }
                                            }
                                        }
                                    }
                                    dvdsrData.RowFilter = null;
                                    if (ls_val != "") { ls_val += ", "; }
                                    if (status_pr == "Pending")
                                    {
                                        if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL"; }
                                        if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                        {
                                            transTemp = ls_val.Replace(", ", "");
                                            if (transTemp != "")
                                            { ls_val = "FHL" + ", " + transTemp; }
                                            else
                                            { ls_val += "FHL"; }
                                        }
                                        if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L"; }
                                    }
                                    if (status_pr == "Approve")
                                    {
                                        if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL/A"; }
                                        if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                        {
                                            transTemp = ls_val.Replace(", ", "");
                                            if (transTemp != "")
                                            { ls_val = "FHL/A" + ", " + transTemp; }
                                            else
                                            { ls_val += "FHL/A"; }
                                        }
                                        if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L/A"; }
                                    }
                                    if (status_pr == "Reject")
                                    {
                                        if (str.Substring(0, 1) == " " && str.Substring(1, 1) == "L") { ls_val += "SHL/R"; }
                                        if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == " ")
                                        {
                                            transTemp = ls_val.Replace(", ", "");
                                            if (transTemp != "")
                                            { ls_val = "FHL/R" + ", " + transTemp; }
                                            else
                                            { ls_val += "FHL/R"; }
                                        }
                                        if (str.Substring(0, 1) == "L" && str.Substring(1, 1) == "L") { ls_val += "L/R"; }
                                    }
                                    mDataRow[dateTime3.Day.ToString().Trim()] = ls_val;
                                    ls_val = "";
                                }
                            }

                        }
                    }

                }
                List<DateTime> dates = new List<DateTime>();
                DayOfWeek day = DayOfWeek.Sunday;
                System.Globalization.CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                for (int i = 1; i <= currentCulture.Calendar.GetDaysInMonth(year, Month); i++)
                {
                    DateTime d = new DateTime(year, Month, i);
                    if (d.DayOfWeek == day)
                    {
                        dates.Add(d);
                    }
                }
                if (dates.Count > 0) {
                    foreach (var item in dates)
                    {
                        mDataRow[item.Day-1]="Off";
                    }
                }
                if (dvdt_holiday.ToTable().Rows.Count > 0)
                {
                    for (int l = 0; l < dvdt_holiday.ToTable().Rows.Count; l++)
                    {
                        if (Convert.ToInt32(dvdt_holiday.ToTable().Rows[l]["day1"].ToString()) != 0)
                        {
                            mDataRow[dvdt_holiday.ToTable().Rows[l]["day1"].ToString().Trim()] = "H";
                        }
                    }
                }
                gvData.Rows.Add(mDataRow);
                return gvData.Rows[0].ItemArray.ToList();

            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

    }
}