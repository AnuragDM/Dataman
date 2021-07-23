using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace AstralFFMS
{
    public partial class rpt_AttendanceReport : System.Web.UI.Page
    {
        DateTime mDate1 = DateTime.Now, mDate2 = DateTime.Now;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                roleType = Settings.Instance.RoleType;
                BindTreeViewControl();
                BindDDLMonth();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                yearDDL.SelectedValue = System.DateTime.Now.Year.ToString();


            }
        }
        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    monthDDL.Items.Add(new ListItem(monthName.Substring(0, 3), month.ToString().PadLeft(2, '0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDDL.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
                //    DataSet ds = GetDataSet("Select smid,smname,underid,lvl from mastsalesrep where active=1 and underid<>0 order by smname");


                DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    root.CollapseAll();
                    CreateNode(root, St);
                    trview.Nodes.Add(root);
                }
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, DataTable Dt)
        {
            DataRow[] Rows = Dt.Select("underid =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(Childnode);
                Childnode.CollapseAll();
                CreateNode(Childnode, Dt);
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            dt = BindAttendenceActivity();
            ds.Tables.Add(dt);
            dt = null;
            dt = BindAttendenceDetail();
            ds.Tables.Add(dt);
            dt = null;
            dt = BindAttendenceSummary();
            ds.Tables.Add(dt);

            ExportDataSetToExcel(ds);
        }

        public DataTable BindAttendenceSummary()
        {
            #region Variable Declaration
            string str = "";
            string dayName = "";
            string status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;

            DataTable gvdt = new DataTable();
            DataTable dtsr = new DataTable();
            DataTable dtdesig = new DataTable();
            #endregion

            try
            {
                string smIDStr = "", smIDStr1 = "";

                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    smIDStr += node.Value + ",";
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                if (smIDStr1 == "")
                {
                    DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dtSMId);
                    dv.RowFilter = "RoleType='AreaIncharge' and SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        foreach (DataRow dr in dv.ToTable().Rows)
                        {
                            smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                        }
                        smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                    }
                    dtSMId.Dispose();
                }
                mDate1 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue);
                mDate2 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).AddDays(-1);
                int year = Convert.ToInt32(yearDDL.SelectedValue);
                int Month = Convert.ToInt32(monthDDL.SelectedValue);
                int dyasInMonth = DateTime.DaysInMonth(year, Month);


                str = "select DesName from MastDesignation  where DesType='SALES' order by sorttype";//where Type='SALES' order by sort
                dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtdesig.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                    }
                }

                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reporting Manager", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Status", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Designation", typeof(String)));
                gvdt.Columns.Add(new DataColumn("HeadQuater", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Date Of Joining", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Date Of Leaving", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Month", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Year", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Total Days", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Working Days", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Retailing Days(R)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Other Official Work(O)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Leave(L)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("HoliDay(H)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Absent(A)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Weekly Off(W)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Avg Retail Time", typeof(String)));


                for (int ii = 1; ii <= dyasInMonth; ii++)
                {
                    dayName = DateTime.Parse(ii + "/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).DayOfWeek.ToString();
                    gvdt.Columns.Add(new DataColumn(Convert.ToString(ii).Trim() + "(" + dayName.Substring(0, 3) + ")", typeof(String)));
                }

                DataRow pDataRow = gvdt.NewRow();
                string mSRepCode = "", strholiday = String.Empty; int totAbsent = 0;

                strholiday = "select distinct smid,DAY(holiday_date) as day1,holiday_date,Reason,areaID,AreaType from View_Holiday where smid in ("+ smIDStr1 + ") and holiday_date between  '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59'";
                DataTable dt_holiday = DbConnectionDAL.GetDataTable(CommandType.Text, strholiday);
                DataView dvdt_holiday = new DataView(dt_holiday);
                Session["holiday"] = dvdt_holiday;


                mSRepCode = @"select  emp.SMId,emp.SMName,emp.SyncId,emp.Mobile,case when isnull(emp.Active,0)=1 then 'Active' else 'Deactive' end Active,replace(convert(varchar,emp.CreatedDate,106),' ','/') CreatedDate,mdst.DesName,isnull(hq.HeadquarterName,'')HeadquarterName,(sr.smname+'-'+IsNull(sr.SyncId,'')) as reportingPerson from MastSalesRep emp left join mastsalesrep sr on sr.smid=emp.underid  left join MastHeadquarter hq on hq.Id=emp.HeadquarterId left join MastLogin ml on ml.id=emp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where emp.SMId in (" + smIDStr1 + ")  order by emp.smname";//and emp.Active=1
                DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, mSRepCode);

                dsrQuery = "select VisId,SMId,VDate,DSR_Type,ISNULL(AppStatus,'') as Status from TransVisit  where SMId in (" + smIDStr1 + ") AND vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "' order by SMId,vdate";
                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
                DataView dvdsrData = new DataView(dsrData);

                string No_Days = "select cast(NoOfDays as int) NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId IN (" + smIDStr1 + ") and (lr.FromDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59' OR lr.ToDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59') and lr.appstatus in ('Approve') order by smid,FromDate";
                DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
                DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);

                foreach (DataRow dr in ssdt.Rows)
                {

                    string mRepName = dr["SMName"].ToString();
                    string mSync = dr["SyncId"].ToString();
                    DataRow mDataRow = gvdt.NewRow();

                    str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and maingrp<>" + dr["SMId"].ToString() + ")";
                    dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtsr.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtsr.Rows.Count; j++)
                        {
                            //for (int i = 0; i < dtdesig.Rows.Count; i++)
                            //{
                            //    if (dtdesig.Rows[i]["DesName"].ToString() == dtsr.Rows[j]["DesName"].ToString())
                            //    {
                            //        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                            //    }
                            //}
                            if (dtsr.Rows[j]["DesName"].ToString() != "")
                            {
                                mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SyncId"].ToString();
                            }
                        }
                    }
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                        {
                            mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                        }
                    }

                    mDataRow["SMId"] = dr["SMId"].ToString();
                    mDataRow["Reporting Manager"] = dr["reportingPerson"].ToString();
                    mDataRow["Name"] = mRepName.ToString();
                    mDataRow["SyncId"] = mSync.ToString();
                    mDataRow["Status"] = dr["Active"].ToString();
                    mDataRow["Designation"] = dr["DesName"].ToString();
                    mDataRow["HeadQuater"] = dr["HeadquarterName"].ToString();
                    mDataRow["Contact No"] = dr["Mobile"].ToString();
                    mDataRow["Date Of Joining"] = dr["CreatedDate"].ToString();
                    mDataRow["Date Of Leaving"] = "";
                    mDataRow["Month"] = monthDDL.SelectedItem.Text.ToString();
                    mDataRow["Year"] = yearDDL.SelectedValue.ToString();

                    mDataRow["Total Days"] = dyasInMonth.ToString();

                    dvdsrData.RowFilter = "smid=" + dr["SMId"] + " and DSR_Type='P' and (Status='' or Status='Approve')";
                    mDataRow["Working Days"] = dvdsrData.ToTable().Rows.Count;
                    dvdsrData.RowFilter = string.Empty;

                    dvdsrData.RowFilter = "smid=" + dr["SMId"] + " and DSR_Type='H'";
                    mDataRow["HoliDay(H)"] = dvdsrData.ToTable().Rows.Count;
                    dvdsrData.RowFilter = string.Empty;

                    dvdsrData.RowFilter = "smid=" + dr["SMId"] + " and DSR_Type='A'";
                    totAbsent = dvdsrData.ToTable().Rows.Count;
                    dvdsrData.RowFilter = string.Empty;

                    dvdsrData.RowFilter = "smid=" + dr["SMId"] + " and DSR_Type='P' and Status='Reject' ";
                    totAbsent += dvdsrData.ToTable().Rows.Count;

                    mDataRow["Absent(A)"] = totAbsent;
                    dvdsrData.RowFilter = string.Empty;

                    dvdsrData.RowFilter = "smid=" + dr["SMId"] + " and DSR_Type='W'";
                    mDataRow["Weekly Off(W)"] = dvdsrData.ToTable().Rows.Count;
                    dvdsrData.RowFilter = string.Empty;

                    mDataRow["Retailing Days(R)"] = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), mDate1.ToString(), mDate2.ToString(), "S", "");

                    mDataRow["Other Official Work(O)"] = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), mDate1.ToString(), mDate2.ToString(), "O", "");

                    mDataRow["Avg Retail Time"] = "00:00";

                    ///Leave Calc

                    dvdt_NoOfdays.RowFilter = "smid=" + dr["SMId"];
                    mDataRow["Leave(L)"] = "";
                    int leave = 0;
                    for (int i = 0; i < dvdt_NoOfdays.ToTable().Rows.Count; i++)
                    {
                        dvdsrData.RowFilter = "smid=" + dr["SMId"] + " and VDate>='" + dvdt_NoOfdays.ToTable().Rows[i]["FromDate"].ToString() + "' and VDate<='" + dvdt_NoOfdays.ToTable().Rows[i]["NoDays"].ToString() + "'";
                        leave += Convert.ToInt32(dvdt_NoOfdays.ToTable().Rows[i]["NoOfDays"].ToString()) - dvdsrData.ToTable().Rows.Count;

                        dvdsrData.RowFilter = string.Empty;
                    }
                    mDataRow["Leave(L)"] = leave;

                    ///


                    dvdsrData.RowFilter = string.Empty;
                    dvdsrData.RowFilter = "smid=" + dr["SMId"];
                    dvdsrData.Sort = "VDate asc";
                    dvdt_NoOfdays.RowFilter = string.Empty;
                    string dateCol = "";
                    foreach (DataRow dsr1 in dvdsrData.ToTable().Rows)
                    {
                        DsrDate = Convert.ToDateTime(dsr1["vdate"].ToString()).ToString("dd/MM/yyyy").Trim();
                        dateCol = Convert.ToInt32(Convert.ToDateTime(DsrDate).ToString("dd").Trim()) + "(" + Convert.ToDateTime(DsrDate).DayOfWeek.ToString().Trim().Substring(0, 3) + ")";
                        if (dsr1["SMId"].ToString() == dr["SMId"].ToString())
                        {

                            #region Leave
                            dvdt_NoOfdays.RowFilter = string.Empty;
                            dvdt_NoOfdays.RowFilter = "smid=" + dr["SMId"] + " and (FromDate>= '" + DsrDate + "' and FromDate<= '" + DsrDate + "') or (NoDays>='" + DsrDate + "' and NoDays<='" + DsrDate + "')";
                            if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
                            {
                                mDataRow[dateCol] = "L";
                            }
                            #endregion

                            if (dsr1["DSR_Type"].ToString() == "P")
                            {
                                if (dsr1["Status"].ToString() == "Reject")
                                {
                                    mDataRow[dateCol] = "A";
                                }
                                else
                                {
                                    mDataRow[dateCol] = "P";
                                }
                            }
                            else
                            {
                                mDataRow[dateCol] = dsr1["DSR_Type"].ToString();
                            }

                        }
                    }




                    dvdt_holiday.RowFilter = null;
                    dvdsrData.RowFilter = null;
                    dvdt_NoOfdays.RowFilter = null;
                    gvdt.Rows.Add(mDataRow);
                    gvdt.AcceptChanges();
                }

                dt_holiday.Dispose();
                dvdt_holiday.Dispose();
                ssdt.Dispose();
                dsrData.Dispose();
                dvdsrData.Dispose();
                dt_NoOfdays.Dispose();
                dvdt_NoOfdays.Dispose();
                dtsr.Dispose();
                dtdesig.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return gvdt;
        }

        public DataTable BindAttendenceDetail()
        {

            #region Variable Declaration
            string str = "";
            string status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;
            int TotalCall = 0, PCall = 0; double productivity = 0.00;
            DataTable gvdt = new DataTable();
            DataTable dtdesig = new DataTable();
            DataTable dtsp = new DataTable();
            #endregion

            try
            {
                string smIDStr = "", smIDStr1 = "";

                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    smIDStr += node.Value + ",";
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                if (smIDStr1 == "")
                {
                    DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dtSMId);
                    dv.RowFilter = "RoleType='AreaIncharge' and SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        foreach (DataRow dr in dv.ToTable().Rows)
                        {
                            smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                        }
                        smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                    }
                    dtSMId.Dispose();
                }
                mDate1 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue);
                mDate2 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).AddDays(-1);
                int year = Convert.ToInt32(yearDDL.SelectedValue);
                int Month = Convert.ToInt32(monthDDL.SelectedValue);
                int dyasInMonth = DateTime.DaysInMonth(year, Month);

                gvdt.Columns.Add(new DataColumn("Date", typeof(String)));

                str = "select DesName from MastDesignation where  DesType='SALES' order by sorttype";
                dtdesig = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtdesig.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdesig.Rows.Count; i++)
                    {
                        gvdt.Columns.Add(new DataColumn(dtdesig.Rows[i]["DesName"].ToString(), typeof(String)));
                    }
                }

                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reporting Manager", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Status", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Designation", typeof(String)));
                gvdt.Columns.Add(new DataColumn("HeadQuater", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Tour Plan", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Type", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reason", typeof(String)));
                gvdt.Columns.Add(new DataColumn("LogOut Reason", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Assigned Beat", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Selected Beat", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Distributor(SyncID)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Login", typeof(String)));
                gvdt.Columns.Add(new DataColumn("LogOut", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Total Time", typeof(String)));
                gvdt.Columns.Add(new DataColumn("First Call", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Last Call", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Retail Time", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Total Call Time (hh:mm)", typeof(String)));
                gvdt.Columns.Add(new DataColumn("TC", typeof(String)));
                gvdt.Columns.Add(new DataColumn("PC", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Productivity", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Retailing Grade", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Retailing Grade Definition", typeof(String)));


                DataRow pDataRow = gvdt.NewRow();
                string mSRepCode = "", strTour = String.Empty;

                mSRepCode = @"select  emp.SMId,emp.SMName,emp.SyncId,emp.Mobile,case when isnull(emp.Active,0)=1 then 'Active' else 'Deactive' end Active,replace(convert(varchar,emp.CreatedDate,106),' ','/') CreatedDate,mdst.DesName,isnull(hq.HeadquarterName,'')HeadquarterName,(sr.smname+'-'+IsNull(sr.SyncId,'')) as reportingPerson from MastSalesRep emp left join mastsalesrep sr on sr.smid=emp.underid  left join MastHeadquarter hq on hq.Id=emp.HeadquarterId left join MastLogin ml on ml.id=emp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where emp.SMId in (" + smIDStr1 + ")  order by emp.smname";//and emp.Active=1
                DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, mSRepCode);
                DataView dvSales = new DataView(ssdt);

                dsrQuery = "select  VisId,TV.SMId,VDate,DSR_Type,ISNULL(AppStatus,'') as Status,Remark,isnull(frTime1,'00:00') frTime1 ,isnull(toTime1,'00:00') toTime1,Mobile_Created_date,Mobile_End_date, isnull(CONVERT(char(5), Mobile_Created_date,108),'') as logintime,isnull(convert(char(5), Mobile_End_date, 108),'') logouttime,isnull(EndRemark,'') EndRemark,CAST( DATEDIFF(minute, cast(isnull(frTime1,'00:00') as datetime), cast(isnull(toTime1,'00:00') as datetime))/60 AS VARCHAR(5))+ ':'+ RIGHT('0' + CAST( DATEDIFF(minute, cast(isnull(frTime1,'00:00') as datetime), cast(isnull(toTime1,'00:00') as datetime))%60 AS VARCHAR(2)), 2)  as tottime , isnull( CAST( DATEDIFF(minute, Mobile_Created_date, Mobile_End_date)/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, Mobile_Created_date, Mobile_End_date)%60 AS VARCHAR(2)), 2),'00') [TotalTime],MSP.SMNAME AS WITHwHOM  from TransVisit TV LEFT JOIN MASTSALESREP MSP ON MSP.SMID=TV.WithUserId  where TV.SMId in (" + smIDStr1 + ") and vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  and ISNULL(AppStatus,'')='Approve' order by TV.vdate,TV.SMId";
                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
                DataView dvdsrData = new DataView(dsrData);

                strTour = "select VDate,SMId,MCityName from TransTourPlan  where SMId in (" + smIDStr1 + ") and vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  order by VDate";
                DataTable dtTour = DbConnectionDAL.GetDataTable(CommandType.Text, strTour);
                DataView dvTour = new DataView(dtTour);

                strTour = "select tb.PlannedDate,tb.SMId,tb.AppStatus,mb.AreaName +'('+ ma.AreaName+')' assignedBeat from TransBeatPlan tb left join MastArea mb on mb.AreaId=tb.BeatId left join MastArea ma on ma.AreaId=mb.UnderId  where tb.SMId in (" + smIDStr1 + ") and mb.AreaType='BEAT' and ma.AreaType='AREA' and  tb.PlannedDate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  order by tb.PlannedDate";
                DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, strTour);
                DataView dvBeat = new DataView(dtBeat);


                string No_Days = "select cast(NoOfDays as int) NoOfDays,SMId,FromDate ,ToDate as NoDays,AppStatus,LeaveString from TransLeaveRequest lr where lr.SMId IN (" + smIDStr1 + ") and (lr.FromDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59' OR lr.ToDate between '" + Settings.dateformat(mDate1.ToString()) + " 00:00' and '" + Settings.dateformat(mDate2.ToString()) + " 23:59') and lr.appstatus in ('Approve') order by smid,FromDate";
                DataTable dt_NoOfdays = DbConnectionDAL.GetDataTable(CommandType.Text, No_Days);
                DataView dvdt_NoOfdays = new DataView(dt_NoOfdays);

                foreach (DataRow drvst in dsrData.Rows)
                {

                    dvSales.RowFilter = "SMId=" + drvst["SMId"].ToString();

                    if (dvSales.ToTable().Rows.Count > 0)
                    {
                        dtsp = dvSales.ToTable();
                        DataRow dr = dtsp.Rows[0];

                        string mRepName = dr["SMName"].ToString();
                        string mSync = dr["SyncId"].ToString();
                        DataRow mDataRow = gvdt.NewRow();

                        str = "select msp.SMName,msp.SyncId,mdst.DesName from MastSalesRep msp left join MastLogin ml on ml.id=msp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where msp.smid in  (select MainGrp from MastSalesRepgrp where SMId=" + dr["SMId"].ToString() + "  and MainGrp<>" + dr["SMId"].ToString() + " )";
                        DataTable dtsr = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtsr.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtsr.Rows.Count; j++)
                            {
                                //for (int i = 0; i < dtdesig.Rows.Count; i++)
                                //{
                                //    if (dtdesig.Rows[i]["DesName"].ToString() == dtsr.Rows[j]["DesName"].ToString())
                                //    {
                                //        mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString();
                                //    }
                                //}
                                if (dtsr.Rows[j]["DesName"].ToString() != "")
                                {
                                    mDataRow[dtsr.Rows[j]["DesName"].ToString()] = dtsr.Rows[j]["SMName"].ToString()+"-"+ dtsr.Rows[j]["SyncId"].ToString();
                                }
                            }
                        }

                        for (int i = 0; i < dtdesig.Rows.Count; i++)
                        {
                            if (mDataRow[dtdesig.Rows[i]["DesName"].ToString()].ToString() == "")
                            {
                                mDataRow[dtdesig.Rows[i]["DesName"].ToString()] = "Vacant";
                            }
                        }

                        DsrDate = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("dd/MM/yyyy").Trim();
                        mDataRow["Date"] = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("yyyy-MM-dd").Trim();
                        mDataRow["SMId"] = dr["SMId"].ToString();
                        mDataRow["Reporting Manager"] = dr["reportingPerson"].ToString();
                        mDataRow["Name"] = mRepName.ToString();
                        mDataRow["SyncId"] = mSync.ToString();
                        mDataRow["Status"] = dr["Active"].ToString();
                        mDataRow["Designation"] = dr["DesName"].ToString();
                        mDataRow["HeadQuater"] = dr["HeadquarterName"].ToString();
                        mDataRow["Contact No"] = dr["Mobile"].ToString();

                        dvTour.RowFilter = "smid=" + dr["SMId"] + " and VDate='" + DsrDate + "'";
                        if (dvTour.ToTable().Rows.Count > 0)
                        {
                            mDataRow["Tour Plan"] = dvTour.ToTable().Rows[0]["MCityName"].ToString();
                        }
                        else
                        {
                            mDataRow["Tour Plan"] = "";
                        }

                        dvTour.RowFilter = null;

                        mDataRow["Type"] = drvst["DSR_Type"].ToString() == "P" ? "Retailing" : drvst["DSR_Type"].ToString() == "W" ? "Week Off" : drvst["DSR_Type"].ToString() == "H" ? "Holiday" : "";

                        if (drvst["DSR_Type"].ToString() == "A")
                        {
                            #region Leave
                            dvdt_NoOfdays.RowFilter = string.Empty;
                            dvdt_NoOfdays.RowFilter = "smid=" + dr["SMId"] + " and (FromDate>= '" + DsrDate + "' and FromDate<= '" + DsrDate + "') or (NoDays>='" + DsrDate + "' and NoDays<='" + DsrDate + "')";
                            if (dvdt_NoOfdays.ToTable().Rows.Count > 0)
                            {
                                mDataRow["Type"] = "Leave";
                            }
                            else
                            {
                                mDataRow["Type"] = "Absent";
                            }

                            #endregion
                        }

                        mDataRow["Reason"] = drvst["Remark"].ToString();
                        mDataRow["LogOut Reason"] = drvst["EndRemark"].ToString();

                        dvBeat.RowFilter = "smid=" + dr["SMId"] + " and PlannedDate= '" + DsrDate + "'";
                        if (dvBeat.ToTable().Rows.Count > 0)
                        {
                            mDataRow["Assigned Beat"] = dvBeat.ToTable().Rows[0]["assignedBeat"].ToString();
                        }
                        dvBeat.RowFilter = null;

                        mDataRow["Selected Beat"] = Convert.ToString(TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), DsrDate.ToString(), "B", drvst["VisId"].ToString()));

                        mDataRow["Distributor(SyncID)"] = TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate.ToString(), DsrDate.ToString(), "D", drvst["VisId"].ToString());

                        mDataRow["Login"] = drvst["logintime"].ToString();
                        mDataRow["LogOut"] = drvst["logouttime"].ToString();
                        mDataRow["Total Time"] = drvst["TotalTime"].ToString();
                        mDataRow["First Call"] = drvst["frTime1"].ToString();
                        mDataRow["Last Call"] = drvst["toTime1"].ToString();
                        mDataRow["Retail Time"] = drvst["tottime"].ToString();
                        mDataRow["Total Call Time (hh:mm)"] = "";

                        TotalCall = Convert.ToInt32(TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate, DsrDate, "T", drvst["VisId"].ToString()));
                        PCall = Convert.ToInt32(TotalCountPrimarySecondarySales(dr["SMId"].ToString(), DsrDate, DsrDate, "P", drvst["VisId"].ToString()));

                        mDataRow["TC"] = TotalCall;
                        mDataRow["PC"] = PCall;
                        if (PCall != 0)
                        {
                            productivity = (Convert.ToDouble(PCall) / Convert.ToDouble(TotalCall)) * 100;
                        }



                        mDataRow["Productivity"] = productivity + " %";

                        string TotretTime = drvst["tottime"].ToString();
                        TotretTime = TotretTime.Substring(0, TotretTime.IndexOf(':'));
                        if (Convert.ToInt32(TotretTime) > 6)
                        {
                            mDataRow["Retailing Grade"] = "A";
                            mDataRow["Retailing Grade Definition"] = "More than 6 Hours";
                        }
                        else if (Convert.ToInt32(TotretTime) >= 3 && Convert.ToInt32(TotretTime) <= 6)
                        {
                            mDataRow["Retailing Grade"] = "B";
                            mDataRow["Retailing Grade Definition"] = "3 to 6 Hours";
                        }
                        else if (Convert.ToInt32(TotretTime) < 3)
                        {
                            mDataRow["Retailing Grade"] = "C";
                            mDataRow["Retailing Grade Definition"] = "More than 3 Hours";
                        }



                        dvSales.RowFilter = null;
                        dvdt_NoOfdays.RowFilter = null;
                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();

                    }


                }
                dtdesig.Dispose();
                ssdt.Dispose();
                dvSales.Dispose();
                dsrData.Dispose();
                dvdsrData.Dispose();
                dtTour.Dispose();
                dvTour.Dispose();
                dtBeat.Dispose();
                dvBeat.Dispose();
                dt_NoOfdays.Dispose();
                dvdt_NoOfdays.Dispose();


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return gvdt;
        }

        public DataTable BindAttendenceActivity()
        {

            #region Variable Declaration
            string str = "";

            string status_pr = string.Empty, ls_val = string.Empty, dsr_val = string.Empty, DsrDate = string.Empty, dsrQuery = string.Empty;
            DataTable gvdt = new DataTable();
            DataTable dtsp = new DataTable();
            #endregion

            try
            {
                string smIDStr = "", smIDStr1 = "";

                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                    smIDStr += node.Value + ",";
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                if (smIDStr1 == "")
                {
                    DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
                    DataView dv = new DataView(dtSMId);
                    dv.RowFilter = "RoleType='AreaIncharge' and SMName<>.";
                    dv.Sort = "SMName asc";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        foreach (DataRow dr in dv.ToTable().Rows)
                        {
                            smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                        }
                        smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                    }
                }
                mDate1 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue);
                mDate2 = DateTime.Parse("01/" + monthDDL.SelectedValue + "/" + yearDDL.SelectedValue).AddMonths(1).AddDays(-1);
                int year = Convert.ToInt32(yearDDL.SelectedValue);
                int Month = Convert.ToInt32(monthDDL.SelectedValue);
                int dyasInMonth = DateTime.DaysInMonth(year, Month);

                gvdt.Columns.Add(new DataColumn("Date", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Region", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SMId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Reporting Manager", typeof(String)));
                gvdt.Columns.Add(new DataColumn("SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Name", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Status", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Designation", typeof(String)));
                gvdt.Columns.Add(new DataColumn("HeadQuater", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Contact No", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Category", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Remark", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Distributor SyncId", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Distributor", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Start Time", typeof(String)));
                gvdt.Columns.Add(new DataColumn("End Time", typeof(String)));
                gvdt.Columns.Add(new DataColumn("Total Time", typeof(String)));

                DataRow pDataRow = gvdt.NewRow();
                string mSRepCode = "";

                mSRepCode = @"select  emp.SMId,emp.SMName,emp.SyncId,emp.Mobile,case when isnull(emp.Active,0)=1 then 'Active' else 'Deactive' end Active,replace(convert(varchar,emp.CreatedDate,106),' ','/') CreatedDate,mdst.DesName,isnull(hq.HeadquarterName,'')HeadquarterName,(sr.smname+'-'+IsNull(sr.SyncId,'')) as reportingPerson from MastSalesRep emp left join mastsalesrep sr on sr.smid=emp.underid  left join MastHeadquarter hq on hq.Id=emp.HeadquarterId left join MastLogin ml on ml.id=emp.UserId left join MastDesignation mdst  on mdst.DesId=ml.DesigId  where emp.SMId in (" + smIDStr1 + ")  order by emp.smname";//and emp.Active=1
                DataTable ssdt = DbConnectionDAL.GetDataTable(CommandType.Text, mSRepCode);
                DataView dvSales = new DataView(ssdt);

                dsrQuery = "select * from (select  tv1.vdate,tv.smid,tv.SpentfrTime ,tv.SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00') [TotalTime],'Discussion' as type from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  " +
"union  select  tv1.vdate,tv.smid,tv.SpentfrTime,tv.SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00') [TotalTime],'Discussion' as type from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "' " +
 "union select  tv1.vdate,tv.smid,'00:00' SpentfrTime ,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkS,'00:00' [TotalTime],'Distributor Collection' as type from DistributerCollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'   " +
 "union select  tv1.vdate,tv.smid,'00:00' SpentfrTime ,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkS,'00:00' [TotalTime],'Distributor Failed Visit' as type from transfailedvisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'   AND MP.PARTYDIST=1 " +
"union  select  tv1.vdate,tv.smid,'00:00' SpentfrTime,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,tv.remarkS, '00:00' [TotalTime],'Distributor Failed Visit' as type from temp_transfailedvisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'   AND MP.PARTYDIST=1 " +
 "union select  distinct tv1.vdate,tv.smid,'00:00' SpentfrTime ,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,'' remarkS,'00:00' [TotalTime],'Distributor Stock' as type from transdiststock tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  AND MP.PARTYDIST=1 " +
"union  select distinct tv1.vdate,tv.smid,'00:00' SpentfrTime,'00:00' SpentToTime,isnull(mp.partyid,0) partyid,mp.partyname,mp.syncid,'' remarkS, '00:00' [TotalTime],'Distributor Stock' as type from temp_transdiststock tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.distid where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  AND MP.PARTYDIST=1 " +
 "union  select  tv1.vdate,tv.smid,tv.SpentfrTime ,tv.SpentToTime,'0' partyid,'' partyname,'' syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00') [TotalTime],'Other' as type from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid  where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)')" +
"union  select tv1.vdate,tv.smid,tv.SpentfrTime ,tv.SpentToTime,'0' partyid,'' partyname,'' syncid,tv.remarkdist, isnull( CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))/60 AS VARCHAR(5)),'00')+ ':'+ isnull( RIGHT('0' + CAST( DATEDIFF(minute, cast(tv.SpentfrTime as datetime), cast(tv.SpentToTime as datetime))%60 AS VARCHAR(2)), 2),'00')  [TotalTime],'Other' as type from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid  where tv.smid in (" + smIDStr1 + ") and isnull(tv1.appstatus,'')<>'Approve' and tv1.vdate between '" + Settings.dateformat(mDate1.ToString()) + "' and '" + Settings.dateformat(mDate2.ToString()) + "'  AND tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') ) a order by vdate,smid,type";
                DataTable dsrData = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
                DataView dvdsrData = new DataView(dsrData);

                foreach (DataRow drvst in dsrData.Rows)
                {

                    dvSales.RowFilter = "SMId=" + drvst["SMId"].ToString();

                    if (dvSales.ToTable().Rows.Count > 0)
                    {
                        dtsp = dvSales.ToTable();
                        DataRow dr = dtsp.Rows[0];

                        string mRepName = dr["SMName"].ToString();
                        string mSync = dr["SyncId"].ToString();
                        DataRow mDataRow = gvdt.NewRow();
                        DsrDate = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("dd/MM/yyyy").Trim();
                        mDataRow["Date"] = Convert.ToDateTime(drvst["vdate"].ToString()).ToString("yyyy-MM-dd").Trim();
                        mDataRow["SMId"] = dr["SMId"].ToString();
                        mDataRow["Reporting Manager"] = dr["reportingPerson"].ToString();
                        mDataRow["Name"] = mRepName.ToString();
                        mDataRow["SyncId"] = mSync.ToString();
                        mDataRow["Status"] = dr["Active"].ToString();
                        mDataRow["Designation"] = dr["DesName"].ToString();
                        mDataRow["HeadQuater"] = dr["HeadquarterName"].ToString();
                        mDataRow["Contact No"] = dr["Mobile"].ToString();

                        mDataRow["Category"] = drvst["type"].ToString();
                        mDataRow["Remark"] = drvst["remarkdist"].ToString();
                        mDataRow["Distributor"] = drvst["partyname"].ToString();
                        mDataRow["Distributor SyncId"] = drvst["syncid"].ToString();

                        mDataRow["Start Time"] = drvst["SpentfrTime"].ToString();
                        mDataRow["End Time"] = drvst["SpentToTime"].ToString();
                        mDataRow["Total Time"] = drvst["TotalTime"].ToString();

                        if (drvst["partyid"].ToString() != "")
                        {
                            str = "select (mr.areaname+'-'+Isnull(mr.SyncId,'')) As areaname from mastarea ma left join mastarea mct on mct.areaid=ma.underid left join mastarea md on md.areaid=mct.underid left join mastarea ms on ms.areaid=md.underid left join mastarea mr on mr.areaid=ms.underid  where ma.areaid in ( select areaid from mastparty where partyid=" + drvst["partyid"].ToString() + " )";
                            DataTable dtregion = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                            if (dtregion.Rows.Count > 0)
                            {
                                mDataRow["Region"] = dtregion.Rows[0]["areaname"].ToString();
                            }

                        }

                        dvSales.RowFilter = null;
                        gvdt.Rows.Add(mDataRow);
                        gvdt.AcceptChanges();

                    }


                }
                ssdt.Dispose();
                dvSales.Dispose();
                dsrData.Dispose();
                dvdsrData.Dispose();
                dtsp.Dispose();


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return gvdt;
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rpt_AttendanceReport.aspx");
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }



        private void ExportDataSetToExcel(DataSet ds)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();
            string path = Server.MapPath("ExportedFiles//");

            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
            {
                Directory.CreateDirectory(path);
            }
            string filename = "Attendence Report " + monthDDL.SelectedItem + "-" + yearDDL.SelectedItem + "-" + DateTime.Now.GetDateTimeFormats('d')[0].Replace('/', '-') + "-" + DateTime.Now.ToLongTimeString().ToString().Replace(':', '_') + ".xlsx";

            if (File.Exists(path + filename))
            {
                File.Delete(path + filename);
            }


            string strPath = Server.MapPath("ExportedFiles//" + filename);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.Range range;

            for (int k = 0; k < ds.Tables.Count; k++)
            {
                DataTable table = ds.Tables[k];

                table.Columns.Remove("SMID");
                //Add a new worksheet to workbook with the Datatable name
                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
                if (k == 0)
                {
                    excelWorkSheet.Name = "Activity";

                }
                if (k == 1)
                {
                    excelWorkSheet.Name = "Attendance Detailed";
                }
                if (k == 2)
                {
                    excelWorkSheet.Name = "Attendance Summary";
                }

                int colindex = 0, GradeCol = 0;

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                    if (table.Columns[i - 1].ColumnName == "Avg Retail Time")
                    {
                        colindex = i;
                    }

                    if (table.Columns[i - 1].ColumnName == "Retailing Grade")
                    {
                        GradeCol = i;
                    }
                    range = excelWorkSheet.Cells[1, i] as Excel.Range;
                    range.Cells.Font.Name = "Calibri";
                    range.Cells.Font.Bold = true;
                    range.Cells.Font.Size = 11;
                    range.Cells.Interior.Color = System.Drawing.Color.FromArgb(120, 102, 178, 255);
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int l = 0; l < table.Columns.Count; l++)
                    {
                        excelWorkSheet.Cells[j + 2, l + 1] = table.Rows[j].ItemArray[l].ToString();
                        if (k == 2)
                        {
                            if (l >= colindex)
                            {
                                range = excelWorkSheet.Cells[j + 2, l + 1] as Excel.Range;

                                if (table.Rows[j].ItemArray[l].ToString() == "P")
                                {
                                    range.Cells.Interior.Color = Excel.XlRgbColor.rgbYellowGreen;
                                }
                                else if (table.Rows[j].ItemArray[l].ToString() == "A")
                                {
                                    range.Cells.Interior.Color = Excel.XlRgbColor.rgbRed;
                                }
                                else if (table.Rows[j].ItemArray[l].ToString() == "L" || table.Rows[j].ItemArray[l].ToString() == "H" || table.Rows[j].ItemArray[l].ToString() == "W")
                                {
                                    range.Cells.Interior.Color = Excel.XlRgbColor.rgbYellow;
                                }
                                else
                                {
                                    range.Cells.Interior.Color = Excel.XlRgbColor.rgbYellowGreen;

                                }
                            }

                        }
                    }
                }



                Excel.Range last = excelWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                chartRange = excelWorkSheet.get_Range("A1", last);
                foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                {
                    cell.BorderAround2();
                }


                if (k != 2)
                {
                    Excel.FormatConditions fcs = chartRange.FormatConditions;
                    Excel.FormatCondition format = (Excel.FormatCondition)fcs.Add
    (Excel.XlFormatConditionType.xlExpression, Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                    format.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                }

                if (k == 1)
                {
                    //  chartRange = excelWorkSheet.get_Range("AH1", "AH" + table.Rows.Count);

                    Excel.Range c1 = (Excel.Range)excelWorkSheet.Cells[1, GradeCol];
                    Excel.Range c2 = (Excel.Range)excelWorkSheet.Cells[table.Rows.Count + 1, table.Columns.Count + 1];

                    chartRange = excelWorkSheet.get_Range(c1, c2) as Excel.Range;
                    foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Cells)
                    {
                        if (cell.Text == "A" || cell.Text == "More than 6 Hours")
                        {
                            cell.ClearFormats();
                            cell.BorderAround2();
                            cell.Interior.Color = Excel.XlRgbColor.rgbGreen;
                        }
                        else if (cell.Text == "B" || cell.Text == "3 to 6 Hours")
                        {
                            cell.ClearFormats();
                            cell.BorderAround2();
                            cell.Interior.Color = Excel.XlRgbColor.rgbYellow;
                        }
                        else if (cell.Text == "C")
                        {
                            cell.ClearFormats();
                            cell.BorderAround2();
                            cell.Interior.Color = Excel.XlRgbColor.rgbRed;
                        }

                    }
                }
                excelWorkSheet.Columns.AutoFit();

                excelWorkSheet.Application.ActiveWindow.SplitRow = 1;
                excelWorkSheet.Application.ActiveWindow.FreezePanes = true;
            }
            Excel.Worksheet worksheet = (Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
            worksheet.Delete();

            excelWorkBook.SaveAs(strPath);
            excelWorkBook.Close();
            excelApp.Quit();
            // excelApp.Visible = true;
            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.TransmitFile(strPath);
            Response.End();



        }




        public string TotalCountPrimarySecondarySales(string smid, string date, string date1, string TotalType, string visitId)
        {
            string totalReailingDays = "0"; string str = "";
            DataTable dt = new DataTable();
            date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            date1 = Convert.ToDateTime(date1).ToString("yyyy-MM-dd");
            if (TotalType.Trim().ToUpper() == "S")
            {
                #region totalReailingDays

                str = "select sum(visid) total from  (select COUNT(visid) VisId from  (select tv.visid from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' 	group by tv.VisId union select tv.visid from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId )a " +
             "union " +
             "select COUNT(visid) VisId from (select tv.visid from transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId  union select tv.visid from temp_transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId)a " +
               "union " +
              "select COUNT(visid) VisId from  (select tv.visid from TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.VisId union select tv.visid from temp_TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.VisId )a " +
             "union " +
            "select COUNT(visid) VisId from (select tv.visid from transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId union select tv.visid from temp_transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId )a " +
            "union  " +
            "select COUNT(visid) VisId from  (select tv.visid from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and  tv.smid=" + smid + " and tv.vdate>='" + date + "' and tv.vdate<='" + date1 + "' and partydist=0 	group by tv.VisId union select tv.visid from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and  tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0  group by tv.VisId)a " +
            "union " +
            "select COUNT(visid) VisId from (select tv.visid from TransSample tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId  union select tv.visid from temp_TransSample tv  left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId )a " +
            "union " +
            "select COUNT(visid) VisId from  (select tv.visid from transvisitdist tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.VisId union select tv.visid from temp_transvisitdist tv   left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.VisId)a " +
            "union " +
            "select COUNT(visid) VisId from (select tv.visid from TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId  union select tv.visid from temp_TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId )a) b";

                #endregion
            }
            else if (TotalType.Trim().ToUpper() == "O")
            {
                #region TotalOtherActivityDays
                str = "select sum(visid) total from  (select COUNT(visid) VisId from  (select tv.visid from TransFailedVisit tv  left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 	group by tv.VisId union select tv.visid from temp_TransFailedVisit  tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 	group by tv.VisId)a " +
            "union " +
           "select COUNT(visid) VisId from (select tv.visid from TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId union select tv.visid from Temp_TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.VisId)a " +
           "union " +
           "select COUNT(visid) VisId from  (select tv.visid from transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv.smid=" + smid + " and tv.vdate>='" + date + "' and tv.vdate<='" + date1 + "' and mp.partydist=1 	group by tv.VisId union select tv.visid from temp_transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv.smid=" + smid + " and tv.vdate>='" + date + "' and tv.vdate<='" + date1 + "' and mp.partydist=1 	group by tv.VisId)a " +
           "union " +
           "select COUNT(visid) VisId from  (select tv.visid from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 	group by tv.VisId union select tv.visid from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 group by tv.VisId)a " +
           "union " +
           "select COUNT(visid) VisId from (select tv.visid from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') group by tv.VisId union select tv.visid from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') group by tv.VisId)a) b";

                #endregion
            }
            else if (TotalType.Trim().ToUpper() == "B")
            {
                #region SelectedBeat

                str = " select   STUFF((SELECT ', ' +area.total FROM (select distinct mb.AreaName + '('+ma.AreaName +')' total from  (select PartyId from  (select tv.PartyId from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate between'" + date + "'  and '" + date1 + "' 	group by tv.PartyId union select tv.PartyId from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' 	group by tv.PartyId )a  group by PartyId " +
             "union " +
             "select PartyId from  (select tv.PartyId from transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' 	group by tv.PartyId union select tv.PartyId from temp_transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' 	group by tv.PartyId)a group by PartyId " +
               "union " +
              "select PartyId from  (select tv.PartyId from TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.PartyId union select tv.PartyId from temp_TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.PartyId )a group by PartyId  " +
             "union " +
            "select PartyId from (select tv.PartyId from transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + "  and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.PartyId union select tv.PartyId from temp_transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + "   and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.PartyId )a group by PartyId " +
            "union  " +
            "select PartyId from  (select tv.PartyId from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + "  and  tv.smid=" + smid + " and tv.vdate>='" + date + "' and tv.vdate<='" + date1 + "' and partydist=0 	group by tv.PartyId union select tv.PartyId from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + "   and  tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0  group by tv.PartyId)a  group by PartyId " +
            "union " +
            "select PartyId from (select tv.PartyId from TransSample tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + "  and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.PartyId  union select tv.PartyId from temp_TransSample tv  left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + "  and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.PartyId )a group by PartyId " +
            "union " +
            "select PartyId from  (select tv.DistId PartyId from transvisitdist tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + "  and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.DistId union select tv.DistId PartyId from temp_transvisitdist tv   left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + "  and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and partydist=0 	group by tv.DistId)a group by PartyId " +
            "union " +
            "select PartyId from (select tv.PartyId from TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + "  and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.PartyId  union select tv.PartyId from temp_TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + "  and  tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' group by tv.PartyId )a group by PartyId ) b left join MastParty mp on mp.PartyId=b.PartyId left join MastArea mb on mb.AreaId=mp.BeatId left join MastArea ma on ma.AreaId=mb.UnderId  where isnull(mp.PartyDist,0)=0  and mb.AreaType='BEAT' and ma.AreaType='AREA' ) area FOR XML PATH ('')), 1, 1, '') total ";

                #endregion
            }
            else if (TotalType.Trim().ToUpper() == "D")
            {
                #region Distributor

                str = " select   STUFF((SELECT ', ' +dist.total FROM (select distinct (mp.PartyName+'('+ mp.SyncId+')') as total from  (select partyid from  (select tv.partyid from TransFailedVisit tv  left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 	group by tv.partyid union select tv.partyid from temp_TransFailedVisit  tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 	group by tv.partyid)a union select partyid from (select tv.DistId partyid from TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' group by tv.Distid	  union select tv.DistId partyid from Temp_TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' group by tv.DistId)a union select partyid from  (select tv.partyid from transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 	group by tv.partyid union select tv.partyid from temp_transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "'	group by tv.partyid)a union select partyid from  (select tv.DistId partyid from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 	group by tv.DistId union select tv.DistId partyid from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and mp.partydist=1 group by tv.DistId)a union select partyid from (select tv.DistId partyid from transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') group by tv.DistId union select tv.DistId partyid from temp_transvisitdist tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve'  and tv1.visid=" + visitId + " and tv.smid=" + smid + " and tv.vdate between'" + date + "' and '" + date1 + "' and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') group by tv.DistId)a) b left join MastParty mp on mp.PartyId=b.PartyId where isnull(mp.PartyDist,0)=1) dist FOR XML PATH ('')), 1, 1, '') total  ";

                #endregion
            }
            else if (TotalType.Trim().ToUpper() == "T")
            {
                str = "select sum(totaldoc) total from  (select COUNT(totaldoc) totaldoc from  (select tv.OrdDocId totaldoc   from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' union select tv.OrdDocId totaldoc from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from (select tv.demodocid totaldoc from transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.demodocid totaldoc from temp_transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from  (select tv.fvdocid totaldoc from TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' union select tv.fvdocid totaldoc  from temp_TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from (select tv.docid totaldoc from transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.docid totaldoc from temp_transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union  select COUNT(totaldoc) totaldoc from  (select tv.collDocid totaldoc from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.collDocid totaldoc from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  )a union select COUNT(totaldoc) totaldoc from (select tv.SampleDociD totaldoc from TransSample tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.SampleDociD totaldoc from temp_TransSample tv  left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from  (select tv.discDocId totaldoc from transvisitdist tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'   union select tv.discDocId totaldoc from temp_transvisitdist tv   left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  )a union select COUNT(totaldoc) totaldoc from (select tv.sretDocid totaldoc from TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.sretDocid totaldoc from temp_TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from (select tv.StKDocid totaldoc from TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'   union select tv.StKDocid totaldoc from Temp_TransDistStock tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  )a ) b";
            }

            else if (TotalType.Trim().ToUpper() == "P")
            {
                str = "select sum(totaldoc) total from  (select COUNT(totaldoc) totaldoc from  (select tv.OrdDocId totaldoc   from TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' union select tv.OrdDocId totaldoc from temp_TransOrder tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from (select tv.demodocid totaldoc from transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.demodocid totaldoc from temp_transdemo tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from  (select tv.fvdocid totaldoc from TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' and partydist=0  union select tv.fvdocid totaldoc  from temp_TransFailedVisit tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate between'" + date + "' and '" + date1 + "' and partydist=0  )a union select COUNT(totaldoc) totaldoc from (select tv.docid totaldoc from transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.docid totaldoc from temp_transcompetitor tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union  select COUNT(totaldoc) totaldoc from  (select tv.collDocid totaldoc from transcollection tv left join transvisit tv1 on tv.visid=tv1.visid left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' and partydist=0   union select tv.collDocid totaldoc from temp_transcollection tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.partyid where isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' and partydist=0   )a union select COUNT(totaldoc) totaldoc from (select tv.SampleDociD totaldoc from TransSample tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'  union select tv.SampleDociD totaldoc from temp_TransSample tv  left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a union select COUNT(totaldoc) totaldoc from  (select tv.discDocId totaldoc from transvisitdist tv  left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' and partydist=0    union select tv.discDocId totaldoc from temp_transvisitdist tv   left join transvisit tv1 on tv.visid=tv1.visid  left join mastparty mp on mp.partyid=tv.DistId where isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' and partydist=0  )a union select COUNT(totaldoc) totaldoc from (select tv.sretDocid totaldoc from TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "'   union select tv.sretDocid totaldoc from temp_TransSalesreturn tv left join transvisit tv1 on tv.visid=tv1.visid where  isnull(tv1.AppStatus,'')='Approve' and  tv1.visid=" + visitId + " and tv1.smid=" + smid + " and tv1.vdate ='" + date + "' )a ) b";
            }

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                totalReailingDays = dt.Rows[0]["total"].ToString();
            }
            return totalReailingDays;
        }




    }
}