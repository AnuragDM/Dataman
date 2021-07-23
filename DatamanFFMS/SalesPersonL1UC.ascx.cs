using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Data;
using System.Globalization;
using DAL;

namespace AstralFFMS
{
    public partial class SalesPersonL1UC : System.Web.UI.UserControl
    {
      //  public int smLogID = GetSMId(Settings.Instance.UserID);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                BindDDLMonth();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                yearDDL.SelectedValue = System.DateTime.Now.Year.ToString();
                ddlMonthSecSale.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlYearSecSale.SelectedValue = System.DateTime.Now.Year.ToString();
                //ddlMonthBeatplan.SelectedValue = System.DateTime.Now.Month.ToString();
                //ddlbeatYearPlan.SelectedValue = System.DateTime.Now.Year.ToString();Expenses_Click
                ddlMonthExpense.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlyearExpenses.SelectedValue = System.DateTime.Now.Year.ToString();
                ddlMonthRemark.SelectedValue = System.DateTime.Now.Month.ToString();
                ddlyearRemark.SelectedValue = System.DateTime.Now.Year.ToString();
            }
            CheckMessages();
        }

        private void CheckMessages()
        {
            try
            {

                string areaqry = @"select CityId from MastSalesRep where SMId=" + Settings.Instance.SMID + "";
                int cityId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, areaqry));
                //Ankita - 07/may/2016- (For Optimization)
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
                    ddlMonthExpense.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    ddlMonthRemark.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDDL.Items.Add(new ListItem(i.ToString()));
                    ddlYearSecSale.Items.Add(new ListItem(i.ToString()));
                    ddlyearExpenses.Items.Add(new ListItem(i.ToString()));
                    ddlyearRemark.Items.Add(new ListItem(i.ToString()));
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
                //Ankita - 07/may/2016- (For Optimization)
                //string leaveqry = @"select * from TransLeaveRequest where SMId=" + Settings.Instance.SMID + " and FromDate >='" + Settings.dateformat(dateLeave1.ToString()) + "' order by VDate desc";
                string leaveqry = @"select AppStatus from TransLeaveRequest where SMId=" + Settings.Instance.SMID + " and FromDate >='" + Settings.dateformat(dateLeave1.ToString()) + "' order by VDate desc";
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

        protected void LeaveReq_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateLeave = Convert.ToDateTime(Settings.GetUTCTime().Day + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                GetLeaveData();
                //var obj = (from r in context.TransLeaveRequests.Where(x => x.UserId == Convert.ToInt32(Settings.Instance.UserID)).OrderBy(x => x.LVRQId)
                //           select new { r.LVRQId, r.FromDate, r.ToDate, r.NoOfDays, r.VDate, r.Reason, r.AppStatus }).ToList();
                string leaveQry = @"select r.LVRQId,msr.SMName, r.FromDate, r.ToDate, r.NoOfDays, r.VDate, r.Reason, r.AppStatus from TransLeaveRequest r 
                                 left join MastSalesRep msr on msr.SMId=r.SMId where r.SMId=" + Convert.ToInt32(Settings.Instance.SMID) + " AND  r.FromDate>='" + Settings.dateformat(dateLeave.ToString()) + "' order by r.VDate desc";
                DataTable leave1dt = DbConnectionDAL.GetDataTable(CommandType.Text, leaveQry);
                if (leave1dt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "block");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    leaverpt.DataSource = leave1dt;
                    leaverpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "block");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
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

        protected void BeatPlan_Click(object sender, EventArgs e)
        {
            try
            {
                //ddlbeatYearPlan.SelectedValue = (Settings.GetUTCTime().Year).ToString();
                //ddlMonthBeatplan.SelectedValue = (Settings.GetUTCTime().Month).ToString();
                DateTime dateBeatplan = Convert.ToDateTime(Settings.GetUTCTime().Day + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                string beatDataQuery = @"select t.PlannedDate,t.BeatId,a1.AreaName,(Select Count(PartyId) from MastParty where BeatId=t.BeatId and PartyDist=0) as CustCount from TransBeatPlan t left join MastArea a1 on t.BeatId=a1.AreaId where t.SMId=" + Settings.Instance.SMID + " and t.StartDate>='" + Settings.dateformat(dateBeatplan.ToString()) + "' and t.AppStatus='Approve'";
                DataTable beatPlandt = DbConnectionDAL.GetDataTable(CommandType.Text, beatDataQuery);
                DataView dv = new DataView(beatPlandt);
                dv.RowFilter = "BeatId<>0";
                if (dv.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "block");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    beatplanrpt.DataSource = dv.ToTable();
                    beatplanrpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "block");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    beatplanrpt.DataSource = dv.ToTable();
                    beatplanrpt.DataBind();
                }

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
                ddlyearRemark.SelectedValue = (Settings.GetUTCTime().Year).ToString();
                ddlMonthRemark.SelectedValue = (Settings.GetUTCTime().Month).ToString();
                DateTime date1 = Convert.ToDateTime("1/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                DateTime date2 = Convert.ToDateTime(Settings.GetUTCTime().Day + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);

                //string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus from TransVisit r where r.SMId=" + smLogID + " and r.VDate between '" + Settings.dateformat(date1.ToString()) + "' and '" + Settings.dateformat(date2.ToString()) + "' and r.AppStatus='Approve' and r.AppRemark<>'' order by r.VDate desc";

                //Added 08-12-2015
                string dsrquery = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus,msr.SMName from TransVisit r left join MastSalesRep msr on r.SMId=msr.SMId where r.SMId=" + Settings.Instance.SMID + " and r.VDate between '" + Settings.dateformat(date1.ToString()) + "' and '" + Settings.dateformat(date2.ToString()) + "' and r.AppStatus IN ('Approve','Reject') and r.AppRemark<>'' order by r.VDate desc";
                //End

                DataTable dsrDatadt = DbConnectionDAL.GetDataTable(CommandType.Text, dsrquery);
                if (dsrDatadt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "block");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    dsrrpt.DataSource = dsrDatadt;
                    dsrrpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "block");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
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

        protected void OutstandingReport_Click(object sender, EventArgs e)
        {
            try
            {
//              string outrptquery = @"select TransDistributerLedger.DistId, partyname, (sum(AmtDr)-sum(AmtCr)) [Balance] from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId left join MastArea on MastArea.AreaId=MastParty.CityId where mastparty.PartyDist=1 and MastParty.CityId in (
//              select AreaId from MastArea where AreaId in (select UnderId from MastArea where AreaId in (SELECT linkcode FROM mastlink WHERE  primtable = 'SALESPERSON' AND linktable = 'AREA' AND primcode = " + Settings.Instance.SMID + "))) group by DistId,PartyName having (sum(AmtDr)-sum(AmtCr))>0 order by Balance desc";
                string diststr = Getdistributorid(Convert.ToInt32(Settings.Instance.SMID));
                string outrptquery = @"select TransDistributerLedger.DistId, partyname, (sum(AmtDr)-sum(AmtCr)) [Balance] from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId left join MastArea on MastArea.AreaId=MastParty.CityId where mastparty.PartyDist=1 and TransDistributerLedger.DistId in (" + diststr + ") group by DistId,PartyName having (sum(AmtDr)-sum(AmtCr)) <>0 order by Balance desc";
                DataTable outrptdt = DbConnectionDAL.GetDataTable(CommandType.Text, outrptquery);
                if (outrptdt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    rptDistOutRep.DataSource = outrptdt;
                    rptDistOutRep.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "block");
                    PrimaryJQX.Style.Add("display", "none");
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

        protected void Expenses_Click(object sender, EventArgs e)
        {
            try
            {               
                ddlyearExpenses.SelectedValue = (Settings.GetUTCTime().Year).ToString();
                ddlMonthExpense.SelectedValue = (Settings.GetUTCTime().Month).ToString();
                string expenseqry = @"SELECT sum(ED.billamount) AS billamount,ED.billdate AS BillDate,max(MET.Name) AS ExpenseName,sum(ED.claimamount) AS claimamount,Sum(Ed.ApprovedAmount) AS ApprovedAmount,msr.SMName FROM expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id Inner JOIN ExpenseGroup EG On Ed.ExpenseGroupID=EG.ExpenseGroupID left join MastSalesRep msr on msr.SMID=Eg.SMID WHERE year(ED.billdate)='" + Settings.GetUTCTime().Year + "' and month(ED.billdate)='" + Settings.GetUTCTime().Month + "' AND Eg.IsSubmitted=1 and Eg.SMID=" + Settings.Instance.SMID + " GROUP  BY ED.billdate,SMName order by BillDate DESC";

                DataTable expensedt = DbConnectionDAL.GetDataTable(CommandType.Text, expenseqry);
                if (expensedt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "block");
                    exprpt.DataSource = expensedt;
                    exprpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "block");
                    exprpt.DataSource = expensedt;
                    exprpt.DataBind();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
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
                
                string diststr = Getdistributorid(Convert.ToInt32(Settings.Instance.SMID));
                string primSaleQry = @"SELECT a.materialGroup AS materialGroup ,a.PartyName,a.SMName,a.ItemClass,SUM(a.Amount) as Amount,SUM(a.preAmt) as PreAmt,SUM(a.MonthAvg) as MonthAvg FROM (
                select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,convert(numeric(18, 2), sum(Amount)) [Amount],0 as preAmt,0 as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") and year(tdi.VDate)='" + Settings.GetUTCTime().Year + "' and month(tdi.VDate)='" + Settings.GetUTCTime().Month + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
               " union all select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],convert(numeric(18, 2), sum(Amount)) as preAmt,0 as MonthAvg from TransDistInv1 tdi left JOIN MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP ON mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") and year(tdi.VDate)='" + (Settings.GetUTCTime().Year - 1) + "' and month(tdi.VDate)='" + Settings.GetUTCTime().Month + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName " +
               " union ALL select mp.PartyName,ms.SMName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],0 as preAmt,convert(numeric(18, 2), sum(Amount/6)) as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left JOIN mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid LEFT JOIN mastsalesrep ms ON ms.SMId = tdin.SMID WHERE  mig.ItemType='materialGroup' and tdin.Distid in (" + diststr + ") AND tdi.VDate between  '" + Settings.dateformat(dt_1.ToString()) + " 00:00'  and '" + Settings.dateformat(predt1.ToString()) + " 23:59' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name,ms.SMName )a group BY a.PartyName,a.materialGroup,a.itemClass,a.SMName";

                DataTable primsaledt = DbConnectionDAL.GetDataTable(CommandType.Text, primSaleQry);
                if (primsaledt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "block");
                    SecondaryJQX.Style.Add("display", "none");
                    ExpenseJQX.Style.Add("display", "none");
                    rptprimsale.DataSource = primsaledt;
                    rptprimsale.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "block");
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

        protected void Secondary_Click(object sender, EventArgs e)
        {
            try
            {
                ddlYearSecSale.SelectedValue = (Settings.GetUTCTime().Year).ToString();
                ddlMonthSecSale.SelectedValue = (Settings.GetUTCTime().Month).ToString();
                string secSaleQry = @"select max(Isnull(tord.OrderAmount,0)) as OrderAmount, mp.PartyName
                from TransOrder tord 
                left join MastParty mp on mp.PartyId=tord.PartyId where tord.SMId=" + Settings.Instance.SMID + " and mp.PartyDist=0 and year(tord.VDate)='" + Settings.GetUTCTime().Year + "' and month(tord.VDate)='" + Settings.GetUTCTime().Month + "' GROUP BY mp.PartyName Order by OrderAmount DESC,PartyName";
                DataTable secsaledt1 = DbConnectionDAL.GetDataTable(CommandType.Text, secSaleQry);
                if (secsaledt1.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
                    SecondaryJQX.Style.Add("display", "block");
                    ExpenseJQX.Style.Add("display", "none");
                    secsalerpt.DataSource = secsaledt1;
                    secsalerpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    BeatJQX.Style.Add("display", "none");
                    LeaveJQX.Style.Add("display", "none");
                    DSRJQX.Style.Add("display", "none");
                    DistJQX.Style.Add("display", "none");
                    PrimaryJQX.Style.Add("display", "none");
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

        protected void btnGo_Click(object sender, EventArgs e)
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

//            string primSaleQry = @"SELECT a.materialGroup AS materialGroup ,a.PartyName,a.ItemClass,SUM(a.Amount) as Amount,SUM(a.preAmt) as PreAmt,SUM(a.MonthAvg) as MonthAvg FROM (
//                                 select mp.PartyName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,convert(numeric(18, 2), sum(Amount)) [Amount],0 as preAmt,0 as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid where mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)='" + yearselected + "' and month(tdi.VDate)='" + monthselected + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name " +
//                                 " union all select mp.PartyName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],convert(numeric(18, 2), sum(Amount)) as preAmt,0 as MonthAvg from TransDistInv1 tdi left JOIN MastItem mi on tdi.ItemId=mi.ItemId left join mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP ON mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid where mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") and year(tdi.VDate)='" + (Convert.ToInt32(yearselected) - 1) + "' and month(tdi.VDate)='" + monthselected + "' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name " +
//                                 " union ALL select mp.PartyName,MAX(mig.ItemName) AS MaterialGroup,mic.Name AS ItemClass,0 as [Amount],0 as preAmt,convert(numeric(18, 2), sum(Amount/6)) as MonthAvg from TransDistInv1 tdi left join MastItem mi on tdi.ItemId=mi.ItemId left JOIN mastitem mig on mi.Underid=mig.ItemId left join MastItemClass mic on mic.Id=mi.ClassId left join MastItemSegment mis on mis.Id=mi.SegmentId left join MastParty MP on mp.PartyId=tdi.DistId  and mp.PartyDist=1 left join MastArea on MastArea.AreaId=MP.CityId left join transdistinv tdin on tdin.distinvdocid=tdi.distinvdocid where mig.ItemType='materialGroup' and tdin.smid in (select smid from mastsalesrepgrp where maingrp = " + Settings.Instance.SMID + ") AND tdi.VDate between  '" + Settings.dateformat(dt_1.ToString()) + " 00:00'  and '" + Settings.dateformat(predt1.ToString()) + " 23:59' GROUP BY PartyName,mig.ItemName,mic.Name,mis.Name )a group BY a.PartyName,a.materialGroup,a.itemClass";
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

        protected void btnGoSecSale_Click(object sender, EventArgs e)
        {
            string monthselectedSecSale = ddlMonthSecSale.SelectedValue;
            string yearselectedSecSale = ddlYearSecSale.SelectedValue;

            string secSaleQry = @"select max(Isnull(tord.OrderAmount,0)) as OrderAmount, mp.PartyName
                from TransOrder tord 
                left join MastParty mp on mp.PartyId=tord.PartyId 
                where tord.SMId=" + Settings.Instance.SMID + " and year(tord.VDate)='" + yearselectedSecSale + "' and month(tord.VDate)='" + monthselectedSecSale + "' and mp.PartyDist=0 GROUP BY mp.PartyName Order by OrderAmount DESC,PartyName";
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

        protected void DSREntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DSREntryForm.aspx");
        }

        protected void DistOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CreateNewPurchaseOrder.aspx");
        }

        protected void BeatEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatPlanEntry.aspx");
        }

        protected void Downloads_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Downloads.aspx");
        }

        protected void LocalExp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExpenseGrp.aspx");
        }

        //protected void btnGoBeatplan_Click(object sender, EventArgs e)
        //{
        //    string Monthselectedbeatplan = ddlMonthBeatplan.SelectedValue;
        //    string yearselectedBeatplan = ddlbeatYearPlan.SelectedValue;
        //    string beatDataQuery = @"select t.PlannedDate,t.BeatId,a1.AreaName,(Select Count(PartyId) from MastParty where BeatId=t.BeatId and PartyDist=0) as CustCount from TransBeatPlan t left join MastArea a1 on t.BeatId=a1.AreaId where t.SMId=" + smLogID + " and year(t.StartDate)='" + yearselectedBeatplan + "' and month(t.StartDate)='" + Monthselectedbeatplan + "' and t.AppStatus='Approve'";
        //    DataTable beatPlandt1 = DbConnectionDAL.GetDataTable(CommandType.Text, beatDataQuery);
        //    DataView dv = new DataView(beatPlandt1);
        //    dv.RowFilter = "BeatId<>0";
        //    if (dv.Count > 0)
        //    {
        //        beatplanrpt.DataSource = beatPlandt1;
        //        beatplanrpt.DataBind();
        //    }
        //    else
        //    {
        //        beatplanrpt.DataSource = beatPlandt1;
        //        beatplanrpt.DataBind();
        //    }
        //}

        protected void btnGoExpenses_Click(object sender, EventArgs e)
        {
            try
            {
                string MonthselectedExpense = ddlMonthExpense.SelectedValue;
                string YearselectedExpense = ddlyearExpenses.SelectedValue;
                string expenseqry = @"SELECT sum(ED.billamount) AS billamount,ED.billdate AS BillDate,max(MET.Name) AS ExpenseName,sum(ED.claimamount) AS claimamount,Sum(Ed.ApprovedAmount) AS ApprovedAmount,msr.SMName FROM expensedetails ED INNER JOIN mastexpensetype MET ON ED.expensetypeid = MET.id Inner JOIN ExpenseGroup EG On Ed.ExpenseGroupID=EG.ExpenseGroupID left join MastSalesRep msr on msr.SMID=Eg.SMID WHERE year(ED.billdate)='" + YearselectedExpense + "' and month(ED.billdate)='" + MonthselectedExpense + "' AND Eg.IsSubmitted=1 and Eg.SMID=" + Settings.Instance.SMID + " GROUP  BY ED.billdate,SMName order by BillDate DESC";

                DataTable expensedt1 = DbConnectionDAL.GetDataTable(CommandType.Text, expenseqry);
                if (expensedt1.Rows.Count > 0)
                {
                    exprpt.DataSource = expensedt1;
                    exprpt.DataBind();
                }
                else
                {
                    exprpt.DataSource = expensedt1;
                    exprpt.DataBind();

                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void btnGoDsrRemark_Click(object sender, EventArgs e)
        {
            string MonthselectedDsrRemark = ddlMonthRemark.SelectedValue;
            string YearselectedDsrRemark = ddlyearRemark.SelectedValue;
            string dsrquery1 = @"select r.VisId, r.VDate,r.AppRemark, r.Remark,r.AppStatus,msr.SMName from TransVisit r left JOIN MastSalesRep msr on r.SMId=msr.SMId 
                                where r.SMId=" + Settings.Instance.SMID + " and year(r.Vdate)='" + YearselectedDsrRemark + "' and month(r.Vdate)='" + MonthselectedDsrRemark + "' and r.AppStatus IN ('Approve','Reject') and r.AppRemark<>'' order by r.VDate desc";
            DataTable dsrDatadt1 = DbConnectionDAL.GetDataTable(CommandType.Text, dsrquery1);
            if (dsrDatadt1.Rows.Count > 0)
            {
                dsrrpt.DataSource = dsrDatadt1;
                dsrrpt.DataBind();
            }
            else
            {
                dsrrpt.DataSource = dsrDatadt1;
                dsrrpt.DataBind();
            }
        }

        public string Getdistributorid(int smid)
        {
            string citystr = "";
            string diststr = "";
            string cityQry = @"select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smid + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            for (int i = 0; i < dtCity.Rows.Count; i++)
            {
                citystr += dtCity.Rows[i]["AreaId"] + ",";
            }
            citystr = citystr.TrimStart(',').TrimEnd(',');
            string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in (select maingrp from mastsalesrepgrp where smid in (" + smid + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + Settings.Instance.SMID + ")) and PartyDist=1 and Active=1 order by PartyName";
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