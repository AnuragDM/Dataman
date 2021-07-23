using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Data;
using System.Globalization;

namespace AstralFFMS
{
    public partial class SalesPersonL3UC : System.Web.UI.UserControl
    {
        int smLogID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Ankita - 09/may/2016- (For Optimization)
                // GetSMId(Settings.Instance.UserID);
                smLogID = Convert.ToInt32(Settings.Instance.SMID);
                BindDDLMonth();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                yearDDL.SelectedValue = System.DateTime.Now.Year.ToString();
                ddlMonthSecSale.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlYearSecSale.SelectedValue = System.DateTime.Now.Year.ToString();
                ddlMonthExp.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlYearExp.SelectedValue = System.DateTime.Now.Year.ToString();

                ddlDSRMonth.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlDSRYear.SelectedValue = System.DateTime.Now.Year.ToString();
            }
            CheckMessages();
        }
        private void CheckMessages()
        {
            try
            {

                string areaqry = @"select CityId from MastSalesRep where SMId=" + Settings.Instance.SMID + "";
                int cityId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, areaqry));
                //Ankita - 09/may/2016- (For Optimization)
                //string msgqry = @"select * from MastMessage where Active=1 and RoleId=" + Settings.Instance.RoleID + " and AreaId in (select maingrp from mastareagrp where areaid =" + cityId + ")";
                string msgqry = @"select Message from MastMessage where Active=1 and RoleId=" + Settings.Instance.RoleID + " and AreaId in (select maingrp from mastareagrp where areaid =" + cityId + ") Order by id desc";
                DataTable msgqrydt = DbConnectionDAL.GetDataTable(CommandType.Text, msgqry);
                if (msgqrydt.Rows.Count > 0)
                {
                    Repeater1.DataSource = msgqrydt;
                    Repeater1.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    monthDDL.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    ddlMonthSecSale.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    ddlMonthExp.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));

                    ddlDSRMonth.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDDL.Items.Add(new ListItem(i.ToString()));
                    ddlYearSecSale.Items.Add(new ListItem(i.ToString()));
                    ddlYearExp.Items.Add(new ListItem(i.ToString()));

                    ddlDSRYear.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        //private void GetSMId(string p)
        //{
        //    try
        //    {
        //        string smIDUserQry = @"select * from MastSalesRep where UserId=" + p + "";
        //        DataTable smIddt = DbConnectionDAL.GetDataTable(CommandType.Text, smIDUserQry);
        //        if (smIddt.Rows.Count > 0)
        //        {
        //            smLogID = Convert.ToInt32(smIddt.Rows[0]["SMId"]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
        private void GetLeaveData()
        {
            try
            {
                DateTime dateLeave1 = Convert.ToDateTime(Settings.GetUTCTime().Day + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                //string leaveqry = @"select * from TransLeaveRequest where SMId=" + smLogID + " and FromDate >='" + Settings.dateformat(dateLeave1.ToString()) + "' order by VDate desc";
                //Ankita - 09/may/2016- (For Optimization)
                //string leaveqry = @"select * from TransLeaveRequest where SMId IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) and FromDate >='" + Settings.dateformat(dateLeave1.ToString()) + "' order BY VDate desc";
                string leaveqry = @"select AppStatus from TransLeaveRequest where SMId IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) and FromDate >='" + Settings.dateformat(dateLeave1.ToString()) + "' order BY VDate desc";
                DataTable leavedt = DbConnectionDAL.GetDataTable(CommandType.Text, leaveqry);
                if (leavedt.Rows.Count > 0)
                {
                    DataView dv = new DataView(leavedt);
                    dv.RowFilter = "AppStatus='Pending'";
                    if (dv.Count > 0)
                    {
                        LeavesLabel.Text = dv.Count.ToString();
                    }
                    else
                    {
                        LeavesLabel.Text = "0";
                    }
                    dv.RowFilter = string.Empty;
                    DataView dv1 = new DataView(leavedt);
                    dv1.RowFilter = "AppStatus='Approve'";
                    if (dv1.Count > 0)
                    {
                        TotalLeavesLabel.Text = dv1.Count.ToString();
                    }
                    else
                    {
                        TotalLeavesLabel.Text = "0";
                    }
                }
                else
                {
                    LeavesLabel.Text = "0";
                    TotalLeavesLabel.Text = "0";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected void Secondary_Click(object sender, EventArgs e)
        {
            try
            {
                ddlYearSecSale.SelectedValue = (Settings.GetUTCTime().Year).ToString();
                ddlMonthSecSale.SelectedValue = (Settings.GetUTCTime().Month).ToString();

                string secSaleQry = @"select DISTINCT a.SMName,SUM(a.OrderAmount) as OrderAmount,sum(a.Sale) [Sale],MAX(EmpName) as EmpName,(PartyName) as PartyName from (select msr.SMName,SUM(tord.OrderAmount) as OrderAmount,0 [Sale],msr.EmpName AS EmpName,(mp.PartyName) as PartyName from TransOrder tord left join MastParty mp on mp.PartyId=tord.PartyId left join MastSalesRep msr ON msr.SMId=tord.SMId where msr.smid IN (SELECT DISTINCT smid FROM   mastsalesrepgrp WHERE  smid IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " )) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) AND mp.PartyDist=0 and year(tord.VDate)='" + Settings.GetUTCTime().Year + "'" + " and month(tord.VDate)='" + Settings.GetUTCTime().Month + "' group BY msr.SMName,msr.EmpName,mp.PartyName UNION ALL" +
                " select msr.SMName,0 [OrderAmount],sum(tdi.Net_Total) [sale],msr.EmpName AS EmpName,'' as PartyName FROM  TransDistInv1 tdi LEFT JOIN Transdistinv t ON tdi.DistInvDocId = t.DistInvDocId LEFT JOIN Mastsalesrep msr ON msr.SMId=t.SMID where t.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)= '" + Settings.GetUTCTime().Year + "' and month(t.VDate)='" + Settings.GetUTCTime().Month + "' group BY msr.SMName,msr.EmpName )a group by a.SMName,a.PartyName having SUM(a.OrderAmount)<>0  ORDER BY Smname ";
                DataTable secsaledt1 = DbConnectionDAL.GetDataTable(CommandType.Text, secSaleQry);
                if (secsaledt1.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "block");
                    ExpenseJQX.Style.Add("display", "none");
                    secsalerpt.DataSource = secsaledt1;
                    secsalerpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "block");
                    ExpenseJQX.Style.Add("display", "none");
                    secsalerpt.DataSource = secsaledt1;
                    secsalerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void LeaveReq_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateLeave = Convert.ToDateTime(Settings.GetUTCTime().Day + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                GetLeaveData();
                //var obj = (from r in context.TransLeaveRequests.Where(x => x.UserId == Convert.ToInt32(Settings.Instance.UserID)).OrderBy(x => x.LVRQId)
                //           select new { r.LVRQId, r.FromDate, r.ToDate, r.NoOfDays, r.VDate, r.Reason, r.AppStatus }).ToList();
                string leaveQry = @"select r.LVRQId,msr.SMName, r.FromDate, r.ToDate, r.NoOfDays, r.VDate, r.Reason, r.AppStatus from TransLeaveRequest r left join MastSalesRep msr on msr.SMId=r.SMId where r.SMId IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) AND  r.FromDate>='" + Settings.dateformat(dateLeave.ToString()) + "' order by r.VDate desc";
                DataTable leave1dt = DbConnectionDAL.GetDataTable(CommandType.Text, leaveQry);
                if (leave1dt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "block");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    leaverpt.DataSource = leave1dt;
                    leaverpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "block");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    leaverpt.DataSource = leave1dt;
                    leaverpt.DataBind();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnExpGo_Click(object sender, EventArgs e)
        {
            string smIDStr = "",  QryChk = "";

            foreach (ListItem item in salespersonListBox.Items)
            {
                if (item.Selected)
                {
                    smIDStr += item.Value + ",";
                }
            }
            smIDStr = smIDStr.TrimStart(',').TrimEnd(',');
            if (smIDStr != "")
            {
                QryChk = "and smid in (" + smIDStr + ")";
            }

            //string expenseqry = @"SELECT sum(ED.billamount) AS billamount,ED.billdate AS BillDate,sum(ED.claimamount) AS claimamount,Sum(Ed.ApprovedAmount) AS ApprovedAmount,msr.SMName FROM expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id Inner JOIN ExpenseGroup EG On Ed.ExpenseGroupID=EG.ExpenseGroupID left join MastSalesRep msr on msr.SMID=Eg.SMID WHERE year(ED.billdate)='" + ddlYearExp.SelectedValue + "' and month(ED.billdate)='" + ddlMonthExp.SelectedValue + "' and Eg.IsSubmitted=1 and Eg.SMID IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " )) " + QryChk + ") GROUP  BY ED.billdate,SMName order by BillDate DESC";
            string expenseqry = @"SELECT sum(ED.billamount) AS billamount,ED.billdate AS BillDate,max(MET.Name) AS ExpenseName,sum(ED.claimamount) AS claimamount,Sum(Ed.ApprovedAmount) AS ApprovedAmount,msr.SMName FROM expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id Inner JOIN ExpenseGroup EG On Ed.ExpenseGroupID=EG.ExpenseGroupID left join MastSalesRep msr on msr.SMID=Eg.SMID WHERE year(ED.billdate)='" + ddlYearExp.SelectedValue + "' and month(ED.billdate)='" + ddlMonthExp.SelectedValue + "' and Eg.IsSubmitted=1 and Eg.SMID IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " )) " + QryChk + ") GROUP  BY ED.billdate,SMName order by BillDate DESC";
           
            DataTable expensedt = DbConnectionDAL.GetDataTable(CommandType.Text, expenseqry);
            if (expensedt.Rows.Count > 0)
            {
                reportcontainer.Style.Add("display", "block");
                PrimaryJQX.Style.Add("display", "none");
                LeaveJQX.Style.Add("display", "none");
                PendingDSRJQX.Style.Add("display", "none");
                DistJQX.Style.Add("display", "none");
                DSRJQX.Style.Add("display", "none");
                L2LocationJQX.Style.Add("display", "none");
                SecondaryJQX.Style.Add("display", "none");
                ExpenseJQX.Style.Add("display", "block");
                exprpt.DataSource = expensedt;
                exprpt.DataBind();
            }
            else
            {
                reportcontainer.Style.Add("display", "block");
                PrimaryJQX.Style.Add("display", "none");
                LeaveJQX.Style.Add("display", "none");
                PendingDSRJQX.Style.Add("display", "none");
                DistJQX.Style.Add("display", "none");
                DSRJQX.Style.Add("display", "none");
                L2LocationJQX.Style.Add("display", "none");
                SecondaryJQX.Style.Add("display", "none");
                ExpenseJQX.Style.Add("display", "block");
                exprpt.DataSource = expensedt;
                exprpt.DataBind();
            }
        }

        protected void ExpensesL3_Click(object sender, EventArgs e)
        {
            ddlYearExp.SelectedValue = (Settings.GetUTCTime().Year).ToString();
            ddlMonthExp.SelectedValue = (Settings.GetUTCTime().Month).ToString();

            BindSalePersonDDl();
            //string expenseqry = @"SELECT sum(ED.billamount) AS billamount,ED.billdate AS BillDate,sum(ED.claimamount) AS claimamount,Sum(Ed.ApprovedAmount) AS ApprovedAmount,msr.SMName FROM expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id Inner JOIN ExpenseGroup EG On Ed.ExpenseGroupID=EG.ExpenseGroupID left join MastSalesRep msr on msr.SMID=Eg.SMID WHERE year(ED.billdate)='" + Settings.GetUTCTime().Year + "' and month(ED.billdate)='" + Settings.GetUTCTime().Month + "' and Eg.IsSubmitted=1 and Eg.SMID IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) GROUP  BY ED.billdate,SMName order by BillDate DESC";
            string expenseqry = @"SELECT sum(ED.billamount) AS billamount,ED.billdate AS BillDate,max(MET.Name) AS ExpenseName,sum(ED.claimamount) AS claimamount,Sum(Ed.ApprovedAmount) AS ApprovedAmount,msr.SMName FROM expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id Inner JOIN ExpenseGroup EG On Ed.ExpenseGroupID=EG.ExpenseGroupID left join MastSalesRep msr on msr.SMID=Eg.SMID WHERE year(ED.billdate)='" + Settings.GetUTCTime().Year + "' and month(ED.billdate)='" + Settings.GetUTCTime().Month + "' and Eg.IsSubmitted=1 and Eg.SMID IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) GROUP  BY ED.billdate,SMName order by BillDate DESC";
            DataTable expensedt = DbConnectionDAL.GetDataTable(CommandType.Text, expenseqry);
            if (expensedt.Rows.Count > 0)
            {
                reportcontainer.Style.Add("display", "block");
                PrimaryJQX.Style.Add("display", "none");
                LeaveJQX.Style.Add("display", "none");
                PendingDSRJQX.Style.Add("display", "none");
                DistJQX.Style.Add("display", "none");
                DSRJQX.Style.Add("display", "none");
                L2LocationJQX.Style.Add("display", "none");
                SecondaryJQX.Style.Add("display", "none");
                ExpenseJQX.Style.Add("display", "block");
                exprpt.DataSource = expensedt;
                exprpt.DataBind();
            }
            else
            {
                reportcontainer.Style.Add("display", "block");
                PrimaryJQX.Style.Add("display", "none");
                LeaveJQX.Style.Add("display", "none");
                PendingDSRJQX.Style.Add("display", "none");
                DistJQX.Style.Add("display", "none");
                DSRJQX.Style.Add("display", "none");
                L2LocationJQX.Style.Add("display", "none");
                SecondaryJQX.Style.Add("display", "none");
                ExpenseJQX.Style.Add("display", "block");
                exprpt.DataSource = expensedt;
                exprpt.DataBind();
            }
        }

        protected void Primary_Click(object sender, EventArgs e)
        {
            try
            {
                yearDDL.SelectedValue = (Settings.GetUTCTime().Year).ToString();
                monthDDL.SelectedValue = (Settings.GetUTCTime().Month).ToString();

                string yy = monthDDL.SelectedValue;
                int Month_new = Convert.ToInt32(yy);
                string year1 = yearDDL.SelectedValue;
                int Year = Convert.ToInt32(year1);
                DateTime dt_1 = new DateTime();
                DateTime predt = new DateTime(Year, Month_new, 1);
                predt = predt.AddMonths(-1);
                dt_1 = predt.AddMonths(-5);
                dt_1 = new DateTime(Convert.ToInt32(DateTime.ParseExact(dt_1.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy")), Convert.ToInt32(DateTime.ParseExact(dt_1.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM")), 1);

                DateTime predt1 = new DateTime();
                //predt1 = new DateTime(dt_1.Year, dt_1.Month, DateTime.DaysInMonth(dt_1.Year, dt_1.Month));
                int dys = DateTime.DaysInMonth(Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy")),
    Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM")));
                predt1 = new DateTime(Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy")),
    Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM")), dys);

//                string primSaleQry = @"SELECT a.materialGroup AS materialGroup ,a.PartyName,a.SMName,a.ItemClass,SUM(a.Amount) as Amount,SUM(a.preAmt) as PreAmt,SUM(a.MonthAvg) as MonthAvg FROM (//
//                select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,convert(numeric(18, 2), sum(Amount)) [Amount],0 as preAmt,0 as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)='" + Settings.GetUTCTime().Year + "' and month(tdi.VDate)='" + Settings.GetUTCTime().Month + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
//                " union all select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],convert(numeric(18, 2), sum(Amount)) as preAmt,0 as MonthAvg from TransDistInv1 tdi left JOIN MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP ON mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)='" + (Settings.GetUTCTime().Year - 1) + "' and month(tdi.VDate)='" + Settings.GetUTCTime().Month + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
//                " union ALL select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],0 as preAmt,convert(numeric(18, 2), sum(Amount/6)) as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left JOIN mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") AND tdi.VDate between  '" + Settings.dateformat(dt_1.ToString()) + " 00:00'  and '" + Settings.dateformat(predt1.ToString()) + " 23:59' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName )a group BY a.PartyName,a.materialGroup,a.itemClass,a.SMName";
                string diststr = Getdistributorid(Convert.ToInt32(Settings.Instance.SMID));
                string primSaleQry = @"SELECT a.materialGroup AS materialGroup ,a.PartyName,a.SMName,a.ItemClass,SUM(a.Amount) as Amount,SUM(a.preAmt) as PreAmt,SUM(a.MonthAvg) as MonthAvg FROM (
                select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,convert(numeric(18, 2), sum(Amount)) [Amount],0 as preAmt,0 as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") and year(tdi.VDate)='" + Settings.GetUTCTime().Year + "' and month(tdi.VDate)='" + Settings.GetUTCTime().Month + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
                " union all select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],convert(numeric(18, 2), sum(Amount)) as preAmt,0 as MonthAvg from TransDistInv1 tdi left JOIN MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP ON mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") and year(tdi.VDate)='" + (Settings.GetUTCTime().Year - 1) + "' and month(tdi.VDate)='" + Settings.GetUTCTime().Month + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
                " union ALL select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],0 as preAmt,convert(numeric(18, 2), sum(Amount/6)) as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left JOIN mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") AND tdi.VDate between  '" + Settings.dateformat(dt_1.ToString()) + " 00:00'  and '" + Settings.dateformat(predt1.ToString()) + " 23:59' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName )a group BY a.PartyName,a.materialGroup,a.itemClass,a.SMName";
        
                DataTable primsaledt = DbConnectionDAL.GetDataTable(CommandType.Text, primSaleQry);
                if (primsaledt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "block");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    rptprimsale.DataSource = primsaledt;
                    rptprimsale.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "block");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    rptprimsale.DataSource = primsaledt;
                    rptprimsale.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void PendingDSRApp_Click(object sender, EventArgs e)
        {
            try
            {
                string str = @"select * from (select *,(select count(*) from TransVisit where TransVisit.SMId=a.SMId and (TransVisit.AppStatus='Pending' or TransVisit.AppStatus is null AND Transvisit.Lock=1)) as Pending from 
                   (select distinct TransVisit.SMId,MastSalesRep.SMName,MastSalesRep.EmpName
                  from TransVisit left join MastSalesRep on TransVisit.SMId=MastSalesRep.SMId where [TransVisit].SMId in (select ms.SMId from [MastSalesRep] ms LEFT JOIN MastRole mr ON ms.roleid=mr.roleid WHERE mr.roletype IN ('CityHead','DistrictHead') and UnderId=" + Settings.Instance.SMID + ") and (TransVisit.AppStatus='Pending' or TransVisit.AppStatus is null))a)t where t.Pending<>0 ";
                DataTable Dt2 = DbConnectionDAL.GetDataTable(CommandType.Text, str);                
                if (Dt2.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "block");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    penddsrrpt.DataSource = Dt2;
                    penddsrrpt.DataBind();
                }   
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "block");
                    //PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    penddsrrpt.DataSource = Dt2;
                    penddsrrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void TodayL2Loc_Click(object sender, EventArgs e)
        {
            try
              {
                  string getL2SMNameQry = @"select SMId,SMName from MastSalesRep where UnderId=" + Settings.Instance.SMID + "";
                DataTable getL2SMNamedt = DbConnectionDAL.GetDataTable(CommandType.Text, getL2SMNameQry);
                string smIDStr = "";
                string smIDStr1 = "";
                if (getL2SMNamedt.Rows.Count > 0)
                {
                    foreach (DataRow dr in getL2SMNamedt.Rows)
                    {
                        smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                        //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                    }
                    smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
                }

                //Ankita - 09/may/2016- (For Optimization)
                //string todayL2LocQry = @"select *,msr.SMName,ma.AreaName from TransVisit t left join MastSalesRep msr on t.SMId=msr.SMId left join MastArea ma on t.nCityId=ma.AreaId where t.SMId in (" + smIDStr1 + ")";
                //string todayL2LocQry = @"select *,msr.SMName,ma.AreaName from TransVisit t left join MastSalesRep msr on t.SMId=msr.SMId left join MastArea ma on t.nCityId=ma.AreaId where t.SMId in (" + smIDStr1 + ") and NextVisitDate between '" + Settings.dateformat(Settings.GetUTCTime().ToString()) + " 00:00' and '" + Settings.dateformat(Settings.GetUTCTime().ToString()) + " 23:59'";
                string todayL2LocQry = @"select msr.SMName,ma.AreaName from TransVisit t left join MastSalesRep msr on t.SMId=msr.SMId left join MastArea ma on t.nCityId=ma.AreaId where t.SMId in (" + smIDStr1 + ") and NextVisitDate between '" + Settings.dateformat(Settings.GetUTCTime().ToString()) + " 00:00' and '" + Settings.dateformat(Settings.GetUTCTime().ToString()) + " 23:59'";
                DataTable l2locrptdt = DbConnectionDAL.GetDataTable(CommandType.Text, todayL2LocQry);
                DataView dv = new DataView(l2locrptdt);
              //  dv.RowFilter = "NextVisitDate='" + Settings.GetUTCTime().ToShortDateString() + "'";
                if (l2locrptdt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "block");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    l2locrpt.DataSource = dv.ToTable();
                    l2locrpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "block");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    l2locrpt.DataSource = dv.ToTable();
                    l2locrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void OutstandingReport_Click(object sender, EventArgs e)
        {
            try
            {
//                string outrptquery = @"select TransDistributerLedger.DistId, partyname, (sum(AmtDr)-sum(AmtCr)) [Balance] from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId left join MastArea on MastArea.AreaId=MastParty.CityId where MastParty.PartyDist=1 and MastParty.CityId in (
//                select AreaId from MastArea where AreaId in (select UnderId from MastArea where AreaId in (SELECT linkcode FROM mastlink WHERE  primtable = 'SALESPERSON' AND linktable = 'AREA' AND primcode = " + Settings.Instance.SMID + "))) group by DistId,PartyName having (sum(AmtDr)-sum(AmtCr))>0 order by Balance desc";
                string diststr = Getdistributorid(Convert.ToInt32(Settings.Instance.SMID));
                string outrptquery = @"select TransDistributerLedger.DistId, partyname, (sum(AmtDr)-sum(AmtCr)) [Balance] from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId left join MastArea on MastArea.AreaId=MastParty.CityId where mastparty.PartyDist=1 and TransDistributerLedger.DistId in (" + diststr + ") group by DistId,PartyName having (sum(AmtDr)-sum(AmtCr)) <>0 order by Balance desc";
                DataTable outrptdt = DbConnectionDAL.GetDataTable(CommandType.Text, outrptquery);
                if (outrptdt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "block");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    rptDistOutRep.DataSource = outrptdt;
                    rptDistOutRep.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "block");
                    DSRJQX.Style.Add("display", "none");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    rptDistOutRep.DataSource = outrptdt;
                    rptDistOutRep.DataBind();
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string monthselected = monthDDL.SelectedValue;
                string yearselected = yearDDL.SelectedValue;

                string yy = monthDDL.SelectedValue;
                int Month_new = Convert.ToInt32(yy);
                string year1 = yearDDL.SelectedValue;
                int Year = Convert.ToInt32(year1);
                DateTime dt_1 = new DateTime();
                DateTime predt = new DateTime(Year, Month_new, 1);
                predt = predt.AddMonths(-1);
                dt_1 = predt.AddMonths(-5);
                dt_1 = new DateTime(Convert.ToInt32(DateTime.ParseExact(dt_1.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy")), Convert.ToInt32(DateTime.ParseExact(dt_1.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM")), 1);

                DateTime predt1 = new DateTime();
                //predt1 = new DateTime(dt_1.Year, dt_1.Month, DateTime.DaysInMonth(dt_1.Year, dt_1.Month));
                int dys = DateTime.DaysInMonth(Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy")),
    Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM")));
                predt1 = new DateTime(Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy")),
    Convert.ToInt32(DateTime.ParseExact(predt.ToString("dd/MMM/yyyy"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM")), dys);

//                string primSaleQry = @"SELECT a.materialGroup AS materialGroup ,a.PartyName,a.SMName,a.ItemClass,SUM(a.Amount) as Amount,SUM(a.preAmt) as PreAmt,SUM(a.MonthAvg) as MonthAvg FROM (
//
//                 select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,convert(numeric(18, 2), sum(Amount)) [Amount],0 as preAmt,0 as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID where mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)='" + yearselected + "' and month(tdi.VDate)='" + monthselected + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +

//                 " union all select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],convert(numeric(18, 2), sum(Amount)) as preAmt,0 as MonthAvg from TransDistInv1 tdi left JOIN MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP ON mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID where mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)='" + (Convert.ToInt32(yearselected) - 1) + "' and month(tdi.VDate)='" + monthselected + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +

//                 " union ALL select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],0 as preAmt,convert(numeric(18, 2), sum(Amount/6)) as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left JOIN mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID where mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") AND tdi.VDate between  '" + Settings.dateformat(dt_1.ToString()) + " 00:00'  and '" + Settings.dateformat(predt1.ToString()) + " 23:59' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName )a group BY a.PartyName,a.materialGroup,a.itemClass,a.SMName";

                string diststr = Getdistributorid(Convert.ToInt32(Settings.Instance.SMID));
                string primSaleQry = @"SELECT a.materialGroup AS materialGroup ,a.PartyName,a.SMName,a.ItemClass,SUM(a.Amount) as Amount,SUM(a.preAmt) as PreAmt,SUM(a.MonthAvg) as MonthAvg FROM (
                 select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,convert(numeric(18, 2), sum(Amount)) [Amount],0 as preAmt,0 as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID where mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") and year(tdi.VDate)='" + yearselected + "' and month(tdi.VDate)='" + monthselected + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
                " union all select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],convert(numeric(18, 2), sum(Amount)) as preAmt,0 as MonthAvg from TransDistInv1 tdi left JOIN MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP ON mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID where mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") and year(tdi.VDate)='" + (Convert.ToInt32(yearselected) - 1) + "' and month(tdi.VDate)='" + monthselected + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
                " union ALL select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],0 as preAmt,convert(numeric(18, 2), sum(Amount/6)) as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left JOIN mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID where mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") AND tdi.VDate between  '" + Settings.dateformat(dt_1.ToString()) + " 00:00'  and '" + Settings.dateformat(predt1.ToString()) + " 23:59' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName )a group BY a.PartyName,a.materialGroup,a.itemClass,a.SMName";

                DataTable primsaledt = DbConnectionDAL.GetDataTable(CommandType.Text, primSaleQry);
                if (primsaledt.Rows.Count > 0)
                {
                    rptprimsale.DataSource = primsaledt;
                    rptprimsale.DataBind();
                }
                else
                {
                    rptprimsale.DataSource = primsaledt;
                    rptprimsale.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGoSecSale_Click(object sender, EventArgs e)
        {
            try
            {
                string monthselectedSecSale = ddlMonthSecSale.SelectedValue;
                string yearselectedSecSale = ddlYearSecSale.SelectedValue;

                string secSaleQry = @"select DISTINCT a.SMName,SUM(a.OrderAmount) as OrderAmount,sum(a.Sale) [Sale],MAX(EmpName) as EmpName,(PartyName) as PartyName from (select msr.SMName,SUM(tord.OrderAmount) as OrderAmount,0 [Sale],msr.EmpName AS EmpName,(mp.PartyName) as PartyName from TransOrder tord left join MastParty mp on mp.PartyId=tord.PartyId left join MastSalesRep msr ON msr.SMId=tord.SMId where msr.smid IN (SELECT DISTINCT smid FROM   mastsalesrepgrp WHERE  smid IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " )) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) AND mp.PartyDist=0 and year(tord.VDate)='" + yearselectedSecSale + "'" + " and month(tord.VDate)='" + monthselectedSecSale + "' group BY msr.SMName,msr.EmpName,mp.PartyName UNION ALL" +
                  " select msr.SMName,0 [OrderAmount],sum(tdi.Net_Total) [sale],msr.EmpName AS EmpName,'' as PartyName FROM  TransDistInv1 tdi LEFT JOIN Transdistinv t ON tdi.DistInvDocId = t.DistInvDocId LEFT JOIN Mastsalesrep msr ON msr.SMId=t.SMID where t.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)= '" + yearselectedSecSale + "' and month(t.VDate)='" + monthselectedSecSale + "' group BY msr.SMName,msr.EmpName )a group by a.SMName,a.PartyName having SUM(a.OrderAmount)<>0  ORDER BY Smname ";

                DataTable secsaledt = DbConnectionDAL.GetDataTable(CommandType.Text, secSaleQry);
                if (secsaledt.Rows.Count > 0)
                {
                    secsalerpt.DataSource = secsaledt;
                    secsalerpt.DataBind();
                }
                else
                {
                    secsalerpt.DataSource = secsaledt;
                    secsalerpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void DSRLevel3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DSRVistEntry.aspx");
        }

        protected void TourExpL3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExpenseGrp.aspx");
        }

        protected void BeatPlanAppL3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatPlanApproval.aspx");
        }

        protected void DownloadsL3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Downloads.aspx");
        }

        protected void LocalExpenseL3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExpenseGrp.aspx");
        }

        protected void LeaveApprvL2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LeaveApproval.aspx");
        }

        protected void btnLocationTracker_Click(object sender, EventArgs e)
        {
            //Response.Redirect("http://67.202.116.175:8082/");
            Response.Redirect("http://fft.dataman.in/");
        }
        private void BindSalePersonDDl()
        {
            try
            {
                DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt);
                //     dv.RowFilter = "RoleName='Level 1'";
                dv.RowFilter = "SMName<>.";
                dv.Sort = "SMName asc";
                if (dv.ToTable().Rows.Count > 0)
                {
                    salespersonListBox.DataSource = dv.ToTable();
                    salespersonListBox.DataTextField = "SMName";
                    salespersonListBox.DataValueField = "SMId";
                    salespersonListBox.DataBind();
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindSalePersonDDl_DSR()
        {
            try
            {
                DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
                DataView dv = new DataView(dt);
                //     dv.RowFilter = "RoleName='Level 1'";
                dv.RowFilter = "SMName<>.";
                dv.Sort = "SMName asc";
                if (dv.ToTable().Rows.Count > 0)
                {
                    LstSalesperson.DataSource = dv.ToTable();
                    LstSalesperson.DataTextField = "SMName";
                    LstSalesperson.DataValueField = "SMId";
                    LstSalesperson.DataBind();
                }
                //    DdlSalesPerson.Items.Insert(0, new ListItem("--Please select--"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void DSRREMARK_Click(object sender, EventArgs e)
        {
            try
            {
                ddlDSRYear.SelectedValue = (Settings.GetUTCTime().Year).ToString();
                ddlDSRMonth.SelectedValue = (Settings.GetUTCTime().Month).ToString();

                 BindSalePersonDDl_DSR();

                DateTime date1 = Convert.ToDateTime("1/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                DateTime date2 = Convert.ToDateTime(Settings.GetUTCTime().Day + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                //-------Commented 08-12-2015-----------//

                //                string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus from TransVisit r 
                //            where r.smid IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + smLogID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + smLogID + " ))) and r.VDate between '" + Settings.dateformat(date1.ToString()) + "' and '" + Settings.dateformat(date2.ToString()) + "' and r.AppStatus='Approve' and r.AppRemark<>'' order by r.VDate desc";

                //------End Comment-------//

                //Added 08-12-2015

                string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus,msr.SMName from TransVisit r left join MastSalesRep msr on r.SMId=msr.SMId
            where r.smid IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ))) and r.VDate between '" + Settings.dateformat(date1.ToString()) + "' and '" + Settings.dateformat(date2.ToString()) + "' and r.AppStatus IN ('Approve','Reject') and r.AppRemark<>'' order by r.VDate desc";
                //End

                //string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus from TransVisit r where r.SMId=" + smLogID + " and r.AppRemark<>'' order by r.VDate desc";
                DataTable dsrDatadt = DbConnectionDAL.GetDataTable(CommandType.Text, dsrquery);
                if (dsrDatadt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "block");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    dsrrpt.DataSource = dsrDatadt;
                    dsrrpt.DataBind();
                }
                else
                {
                     reportcontainer.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    PendingDSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "block");
                    L2LocationJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    dsrrpt.DataSource = dsrDatadt;
                    dsrrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnSalesperson_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr = "", QryChk = "";

                foreach (ListItem item in LstSalesperson.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr += item.Value + ",";
                    }
                }
                smIDStr = smIDStr.TrimStart(',').TrimEnd(',');

                if (smIDStr != "")
                {
                    QryChk = "and smid in (" + smIDStr + ")";
                }

                DateTime date1 = Convert.ToDateTime("1/" + ddlDSRMonth.SelectedValue + "/" + ddlDSRYear.SelectedValue);
                DateTime date2 = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(Convert.ToInt32(ddlDSRYear.SelectedValue), Convert.ToInt32(ddlDSRMonth.SelectedValue))) + "/" + ddlDSRMonth.SelectedValue + "/" + ddlDSRYear.SelectedValue);

                //                string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus from TransVisit r 
                //            where r.smid IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + smLogID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + smLogID + " ))) and r.VDate between '" + Settings.dateformat(date1.ToString()) + "' and '" + Settings.dateformat(date2.ToString()) + "' and r.AppStatus='Approve' and r.AppRemark<>'' order by r.VDate desc";

                string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus,msr.SMName from TransVisit r left join MastSalesRep msr on r.SMId=msr.SMId 
            where r.smid IN (SELECT smid FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " ) AND level >= (SELECT DISTINCT level FROM   mastsalesrepgrp WHERE  maingrp IN ( " + Settings.Instance.SMID + " )) " + QryChk + ") and r.VDate between '" + Settings.dateformat(date1.ToString()) + "' and '" + Settings.dateformat(date2.ToString()) + "' and r.AppStatus IN ('Approve','Reject') and r.AppRemark<>'' order by r.VDate desc";

                //string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus from TransVisit r where r.SMId=" + smLogID + " and r.AppRemark<>'' order by r.VDate desc";
                DataTable dsrDatadt = DbConnectionDAL.GetDataTable(CommandType.Text, dsrquery);
                if (dsrDatadt.Rows.Count > 0)
                {
                    dsrrpt.DataSource = dsrDatadt;
                    dsrrpt.DataBind();
                }
                else
                {
                    dsrrpt.DataSource = dsrDatadt;
                    dsrrpt.DataBind();
                }
                //              
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void L3RemarkEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DSREntryFormLevel3.aspx");
        }
        public string Getdistributorid(int smid)
        {
            string citystr = "";
            string diststr = "";
            string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smid + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            for (int i = 0; i < dtCity.Rows.Count; i++)
            {
                citystr += dtCity.Rows[i]["AreaId"] + ",";
            }
            citystr = citystr.TrimStart(',').TrimEnd(',');
            string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + smid + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + Settings.Instance.SMID + ")))  and PartyDist=1 and Active=1 order by PartyName";
            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
            for (int i = 0; i < dtDist.Rows.Count; i++)
            {
                diststr += dtDist.Rows[i]["PartyId"] + ",";

            }
            diststr = diststr.TrimStart(',').TrimEnd(',');
            return diststr;

        }

     
    }
}