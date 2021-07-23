using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class DistributorUC : System.Web.UI.UserControl
    {
         int distID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                distID = GetDistId(Convert.ToInt32(Settings.Instance.UserID));
                BindDDLMonth();
                monthDDL.SelectedValue = System.DateTime.Now.Month.ToString();
                yearDDL.SelectedValue = System.DateTime.Now.Year.ToString();

                //Added By - Abhishek 02/12/2015 UAT. Dated-04-12-2015
                txttodate1.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtmDate1.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");

                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                //End

            }
            CheckMessages();
        }
        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    monthDDL.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                    monthDropDownList.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDDL.Items.Add(new ListItem(i.ToString()));
                    yearDropDownList.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void CheckMessages()
        {
            try
            {
                string areaqry = @"select CityId from MastParty where UserId=" + Settings.Instance.UserID + " and PartyDist=1 and Active=1";
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
        private int GetDistId(int p)
        {
            try
            {
                //var envObj1 = (from e in context.MastParties.Where(x => x.UserId == p && x.PartyDist == true)
                //               select new { e.PartyId, e.PartyName }).FirstOrDefault();
                string distQry = @"select e.PartyId, e.PartyName from MastParty e where e.UserId=" + p + " and e.PartyDist=1 and Active=1";
                DataTable DtNew2 = DbConnectionDAL.GetDataTable(CommandType.Text, distQry);
                if (DtNew2.Rows.Count > 0)
                {
                    return Convert.ToInt32(DtNew2.Rows[0]["PartyId"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }
        }

        protected void MaterialMaster_Click(object sender, EventArgs e)
        {
            try
            {
                //                string matDetQry = @"SELECT mi1.ItemName, convert(NUMERIC(18,2),sum(Net_Total)) [Amt]  FROM TransDistInv1 t 
                //                INNER JOIN mastitem mi ON t.ItemId=mi.ItemId LEFT JOIN mastitem mi1 ON mi.Underid=mi1.ItemId
                //                WHERE year(t.VDate)='" + Settings.GetUTCTime().Year + "' and month(t.VDate)='" + Settings.GetUTCTime().Month + "' AND t.DistId=" + distID + "  GROUP BY mi1.ItemName ORDER BY Amt DESC,mi1.ItemName";

                string matDetQry = @"SELECT mi1.ItemName AS MaterialGroup,MAX(mi1.ItemId) AS MatGrpId,MAX(mp.PartyId) AS DistId,convert(NUMERIC(18,2),sum(Net_Total)) [Amt]  FROM TransDistInv1 t INNER JOIN mastitem mi ON t.ItemId=mi.ItemId LEFT JOIN mastitem mi1 ON mi.Underid=mi1.ItemId LEFT JOIN mastparty mp on mp.PartyId=t.DistId
                  WHERE year(t.VDate)='" + Settings.GetUTCTime().Year + "' and month(t.VDate)='" + Settings.GetUTCTime().Month + "' AND mp.PartyDist=1 AND t.DistId=" + distID + " GROUP BY mi1.ItemName ORDER BY Amt DESC,mi1.ItemName";

                DataTable matDetDt = DbConnectionDAL.GetDataTable(CommandType.Text, matDetQry);
                if (matDetDt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "block");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    matmasterRep.DataSource = matDetDt;
                    matmasterRep.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "block");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    matmasterRep.DataSource = matDetDt;
                    matmasterRep.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        protected void PendingOrders_Click(object sender, EventArgs e)
        {
            try
            {
                fillRepeter();
                txtmDate.Attributes.Add("ReadOnly", "true");
                txttodate.Attributes.Add("ReadOnly", "true");

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string str = "";
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtmDate.Text))
            {
                if (txtmDate.Text != "" && txttodate.Text != "")
                {                 

                    str = @"SELECT DISTINCT max(T1.PODocId) as PODocId,max(t1.Vdate) as VDate,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS ResCenName,sum(T1.ItemwiseTotal) AS ItemwiseTotal
                      FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId where t1.Vdate>='" + Settings.dateformat1(txtmDate.Text) + "' and t1.Vdate<='" + Settings.dateformat1(txttodate.Text) + "' " +
                    " and T1.DistId in (" + Settings.Instance.DistributorID + ") GROUP BY T1.PODocId  ORDER BY VDate DESC ";
                }
                else
                {                 

                    str = @"SELECT DISTINCT max(T1.PODocId) as PODocId,max(t1.Vdate) as VDate,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS ResCenName,sum(T1.ItemwiseTotal) AS ItemwiseTotal
                      FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId where T1.DistId in (" + Settings.Instance.DistributorID + ") GROUP BY T1.PODocId  ORDER BY VDate DESC ";

                }

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    PendingJQX.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    rpt.DataSource = dt;
                    rpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    PendingJQX.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }

        private void fillRepeter()
        {
             //string str = @"SELECT DISTINCT T1.PODocId,Convert(varchar(15),CONVERT(date,T1.VDate,103),106) AS VDate,T1.PortalNo,T2.ResCenName,T1.ItemwiseTotal FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId WHERE Convert(VARCHAR(10),T1.VDate,101)>='" + Settings.dateformat1(txtmDate.Text) + "' and Convert(VARCHAR(10),T1.VDate,101)<='" + Settings.dateformat1(txttodate.Text) + "' and T1.DistID=" + Settings.Instance.DistributorID + " ORDER BY VDate desc";
               string str = @"SELECT DISTINCT max(T1.PODocId) as PODocId,max(t1.Vdate) as VDate,max(T1.PortalNo) AS PortalNo ,max(T2.ResCenName) AS ResCenName,sum(T1.ItemwiseTotal) AS ItemwiseTotal FROM PurchaseOrderImport AS T1 Left JOIN MastResCentre AS T2 ON T1.LocationID=T2.ResCenId 
               where t1.Vdate>='" + Settings.dateformat1(txtmDate.Text) + "' and t1.Vdate<='" + Settings.dateformat1(txttodate.Text) + "' and T1.DistId in (" + Settings.Instance.DistributorID + ") GROUP BY T1.PODocId  ORDER BY VDate desc ";

              DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (dt != null && dt.Rows.Count > 0)
            {
                reportcontainer.Style.Add("display", "block");
                PendingJQX.Style.Add("display", "block");
                MaterialMasterJQX.Style.Add("display", "none");
                InvoiceJQX.Style.Add("display", "none");
                LedgerPosJQX.Style.Add("display", "none");
                NewPartyJQX.Style.Add("display", "none");
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                reportcontainer.Style.Add("display", "block");
                PendingJQX.Style.Add("display", "block");
                MaterialMasterJQX.Style.Add("display", "none");
                InvoiceJQX.Style.Add("display", "none");
                LedgerPosJQX.Style.Add("display", "none");
                NewPartyJQX.Style.Add("display", "none");
                rpt.DataSource = null;
                rpt.DataBind();
            }

        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string matDetQry1 = @"SELECT mi1.ItemName AS MaterialGroup,mp.PartyId as DistId, convert(NUMERIC(18,2),sum(Net_Total)) [Amt]  FROM TransDistInv1 t 
                LEFT JOIN mastitem mi ON t.ItemId=mi.ItemId left JOIN mastitem mi1 ON mi1.ItemId=mi.Underid 
                LEFT JOIN mastparty mp on mp.PartyId=t.DistId
                WHERE year(t.VDate)='" + yearDDL.SelectedValue + "' and month(t.VDate)='" + monthDDL.SelectedValue + "' AND t.DistId=" + distID + " and mp.PartyDist=1 GROUP BY mi1.ItemName,mp.PartyId ORDER BY Amt DESC, mi1.ItemName";
                DataTable matDetDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, matDetQry1);
                if (matDetDt1.Rows.Count > 0)
                {
                    matmasterRep.DataSource = matDetDt1;
                    matmasterRep.DataBind();
                }
                else
                {
                    matmasterRep.DataSource = matDetDt1;
                    matmasterRep.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string str = "";
            if (Convert.ToDateTime(txttodate1.Text) >= Convert.ToDateTime(txtmDate1.Text))
            {
                if (txtmDate1.Text != "" && txttodate1.Text != "")
                {
                    string mainQry = " and tdv.Vdate between '" + Settings.dateformat(txtmDate1.Text) + " 00:00' and '" + Settings.dateformat(txttodate1.Text) + " 23:59'";
                    str = @"SELECT tdv.distid,
                                       tdv.distinvdocid,
                                       CONVERT(VARCHAR(15), CONVERT(DATE, tdv.vdate, 103), 106) AS VDate,
                                       isnull(Sum(tdv1.Amount),0) AS Amount,
                                       mrc.rescenname   AS BranchName
                                FROM   transdistinv tdv
                                       INNER JOIN transdistinv1 tdv1
                                               ON tdv.distinvdocid = tdv1.distinvdocid
                                       LEFT JOIN mastrescentre mrc
                                             ON mrc.rescenid = tdv1.locationid
                                    where tdv.DistId=" + Settings.Instance.DistributorID + " " + mainQry + " Group BY tdv.distid,tdv.distinvdocid,tdv.vdate,mrc.rescenname";
                }
                else
                {
                    str = @"SELECT tdv.distid,
                                       tdv.distinvdocid,
                                       CONVERT(VARCHAR(15), CONVERT(DATE, tdv.vdate, 103), 106) AS VDate,
                                       isnull(Sum(tdv1.Amount),0) AS Amount,
                                       mrc.rescenname   AS BranchName
                                FROM   transdistinv tdv
                                       INNER JOIN transdistinv1 tdv1
                                               ON tdv.distinvdocid = tdv1.distinvdocid
                                       LEFT JOIN mastrescentre mrc
                                             ON mrc.rescenid = tdv1.locationid
                                WHERE tdv.DistID=" + Settings.Instance.DistributorID + " Group BY tdv.distid,tdv.distinvdocid,tdv.vdate,mrc.rescenname";
                }
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "block");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    invrpt.DataSource = dt;
                    invrpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "block");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    invrpt.DataSource = null;
                    invrpt.DataBind();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                invrpt.DataSource = null;
                invrpt.DataBind();
            }
        }

        protected void Invoice_Click(object sender, EventArgs e)
        {
            try
            {
                
                txtmDate1.Attributes.Add("ReadOnly", "true");
                txttodate1.Attributes.Add("ReadOnly", "true");

                string mainQry = " and tdv.Vdate between '" + Settings.dateformat(txtmDate1.Text) + " 00:00' and '" + Settings.dateformat(txttodate1.Text) + " 23:59'";
                string invqry = @"SELECT tdv.distid,tdv.distinvdocid,CONVERT(VARCHAR(15), CONVERT(DATE, tdv.vdate, 103), 106) AS VDate, isnull(Sum(tdv1.Amount),0) AS Amount,mrc.rescenname AS BranchName FROM transdistinv tdv
                INNER JOIN transdistinv1 tdv1 ON tdv.distinvdocid = tdv1.distinvdocid LEFT JOIN mastrescentre mrc ON mrc.rescenid = tdv1.locationid where tdv.DistId=" + Settings.Instance.DistributorID + " " + mainQry + " Group BY tdv.distid,tdv.distinvdocid,tdv.vdate,mrc.rescenname";

                DataTable invqryDt = DbConnectionDAL.GetDataTable(CommandType.Text, invqry);
                if (invqryDt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "block");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    invrpt.DataSource = invqryDt;
                    invrpt.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "block");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "none");
                    invrpt.DataSource = null;
                    invrpt.DataBind();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void PurForMonth_Click(object sender, EventArgs e)
        {
            reportcontainer.Style.Add("display", "block");
            MaterialMasterJQX.Style.Add("display", "none");
            InvoiceJQX.Style.Add("display", "block");
            LedgerPosJQX.Style.Add("display", "none");
            NewPartyJQX.Style.Add("display", "none");
        }

        protected void NewParty_Click(object sender, EventArgs e)
        {
            try
            {
              //  int distid = GetDistId(Convert.ToInt32(Settings.Instance.UserID));
                string newPartyQry = @"select r.PartyName, Address = r.Address1 + r.Address2, r.Mobile, r.PartyId, r.Created_Date,r.ContactPerson,cty.AreaName as City,are.AreaName as Area,beat.AreaName as beat from MastParty r left join mastarea cty on r.CityId=cty.AreaId
                  left join mastarea are on r.AreaId=are.AreaId
                  left join mastarea beat on r.BeatId=beat.AreaId where 
                  r.UnderId=" + Settings.Instance.DistributorID + " and year(r.Created_Date)='" + Settings.GetUTCTime().Year + "' and month(r.Created_Date)='" + Settings.GetUTCTime().Month + "' and r.PartyDist=0 and r.Active=1";
                DataTable newPartyQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, newPartyQry);
                if (newPartyQryDt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "block");
                    newpartyrpt.DataSource = newPartyQryDt;
                    newpartyrpt.DataBind();

                    ddltypeFilter.SelectedValue = "New";
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "none");
                    NewPartyJQX.Style.Add("display", "block");
                    newpartyrpt.DataSource = newPartyQryDt;
                    newpartyrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void LedgerPos_Click(object sender, EventArgs e)
        {
            try
            {
                string outrptquery = @"select TransDistributerLedger.DistId, partyname,max(MastParty.CreditLimit) AS CreditLimit,max(MastParty.CreditDays) AS CreditDays,
               (sum(AmtDr)-sum(AmtCr)) [Balance] from TransDistributerLedger left join MastParty on MastParty.PartyId=TransDistributerLedger.DistId left join MastArea on MastArea.AreaId=MastParty.CityId  where MastParty.PartyDist=1 and TransDistributerLedger.DistId=" + Settings.Instance.DistributorID + " group by DistId,PartyName";
                DataTable outrptdt = DbConnectionDAL.GetDataTable(CommandType.Text, outrptquery);
                if (outrptdt.Rows.Count > 0)
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "block");
                    NewPartyJQX.Style.Add("display", "none");
                    rptLedgerRep.DataSource = outrptdt;
                    rptLedgerRep.DataBind();
                }
                else
                {
                    reportcontainer.Style.Add("display", "block");
                    MaterialMasterJQX.Style.Add("display", "none");
                    InvoiceJQX.Style.Add("display", "none");
                    PendingJQX.Style.Add("display", "none");
                    LedgerPosJQX.Style.Add("display", "block");
                    NewPartyJQX.Style.Add("display", "none");
                    rptLedgerRep.DataSource = outrptdt;
                    rptLedgerRep.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void OrderEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PurchaseOrderForDistributor.aspx");
        }

        protected void DownloadsDist_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Downloads.aspx");

        }

        protected void DistComplaint_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistComplaint.aspx");
        }

        protected void DistSuggestion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistSuggestion.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");

            DateTime date1 = Convert.ToDateTime("1/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
            DateTime date2 = Convert.ToDateTime(DateTime.DaysInMonth(Convert.ToInt32(Settings.GetUTCTime().Year), Convert.ToInt32(Settings.GetUTCTime().Month)) + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);

            monthDropDownList.SelectedValue = Settings.GetUTCTime().Month.ToString();
            yearDropDownList.SelectedValue = Settings.GetUTCTime().Year.ToString();
            DataTable dt = GetDetailLedgerData(Convert.ToInt32(hdnVisItCode.Value), date1, date2);
            if (dt.Rows.Count > 0)
            {
                detailDiv.Style.Add("display", "block");
                detailDistrpt.DataSource = dt;
                detailDistrpt.DataBind();
            }
            else
            {
                detailDiv.Style.Add("display", "block");
                detailDistrpt.DataSource = dt;
                detailDistrpt.DataBind();
            }
        }
        public static DataTable GetDetailLedgerData(int DistId, DateTime date1, DateTime date2)
        {

            //DateTime toTime = Convert.ToDateTime(GetUTCTime().ToShortDateString());
            //DateTime fromTime = toTime.AddMonths(-6);
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@Distributor_Id", DbParameter.DbType.Int, 1, DistId);
            dbParam[1] = new DbParameter("@From_Date", DbParameter.DbType.DateTime, 1, date1);
            dbParam[2] = new DbParameter("@To_Date", DbParameter.DbType.DateTime, 1, date2);
            return DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_selectDistributorLedger", dbParam);
        }

        public static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        protected void GetLedgerDeatil_Click(object sender, EventArgs e)
        {
            DateTime date1 = Convert.ToDateTime("1/" + monthDropDownList.SelectedValue + "/" + yearDropDownList.SelectedValue);
            DateTime date2 = Convert.ToDateTime(DateTime.DaysInMonth(Convert.ToInt32(yearDropDownList.SelectedValue), Convert.ToInt32(monthDropDownList.SelectedValue)) + "/" + monthDropDownList.SelectedValue + "/" + yearDropDownList.SelectedValue);
            string distID = Settings.Instance.DistributorID;
            DataTable dt1 = GetDetailLedgerData(Convert.ToInt32(distID), date1, date2);
            if (dt1.Rows.Count > 0)
            {
                detailDiv.Style.Add("display", "block");
                detailDistrpt.DataSource = dt1;
                detailDistrpt.DataBind();
            }
            else
            {
                detailDiv.Style.Add("display", "block");
                detailDistrpt.DataSource = dt1;
                detailDistrpt.DataBind();
            }
        }

        protected void btnGoPurch_Click(object sender, EventArgs e)
        {
            try
            {
              
                //                string matDetQry1 = @"SELECT mi1.ItemName AS MaterialGroup,mp.PartyId as DistId, convert(NUMERIC(18,2),sum(Net_Total)) [Amt]  FROM TransDistInv1 t 
                //                LEFT JOIN mastitem mi ON t.ItemId=mi.ItemId left JOIN mastitem mi1 ON mi1.ItemId=mi.Underid 
                //                LEFT JOIN mastparty mp on mp.PartyId=t.DistId
                //                WHERE year(t.VDate)='" + yearDDL.SelectedValue + "' and month(t.VDate)='" + monthDDL.SelectedValue + "' AND t.DistId=" + distID + " and mp.PartyDist=1 GROUP BY mi1.ItemName,mp.PartyId ORDER BY Amt DESC, mi1.ItemName";

                string matDetQry1 = @"SELECT max(DATEPART(yyyy,t.VDate)) AS Year,max(DATEPART(mm,t.VDate)) AS Month, mi1.ItemName AS MaterialGroup,MAX(mi1.ItemId) AS MatGrpId,MAX(mp.PartyId) AS DistId,convert(NUMERIC(18,2),sum(Net_Total)) [Amt]  FROM TransDistInv1 t INNER JOIN mastitem mi ON t.ItemId=mi.ItemId LEFT JOIN mastitem mi1 ON mi.Underid=mi1.ItemId LEFT JOIN mastparty mp on mp.PartyId=t.DistId
                  WHERE year(t.VDate)='" + yearDDL.SelectedValue + "' and month(t.VDate)='" + monthDDL.SelectedValue + "' AND mp.PartyDist=1 AND t.DistId=" + distID + " GROUP BY mi1.ItemName ORDER BY Amt DESC,mi1.ItemName";

                DataTable matDetDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, matDetQry1);
                if (matDetDt1.Rows.Count > 0)
                {
                    matmasterRep.DataSource = matDetDt1;
                    matmasterRep.DataBind();
                }
                else
                {
                    matmasterRep.DataSource = matDetDt1;
                    matmasterRep.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnNewParty_Click(object sender, EventArgs e)
        {
            string newPartyQry1 = string.Empty;
            if (ddltypeFilter.SelectedValue == "New")
            {
                newPartyQry1 = @"select r.PartyName, Address = r.Address1 + r.Address2, r.Mobile, r.PartyId, r.Created_Date,r.ContactPerson,cty.AreaName as City,are.AreaName as Area,beat.AreaName as beat from MastParty r left join mastarea cty on r.CityId=cty.AreaId
                  left join mastarea are on r.AreaId=are.AreaId
                  left join mastarea beat on r.BeatId=beat.AreaId where 
                  r.UnderId=" + distID + " and year(r.Created_Date)='" + Settings.GetUTCTime().Year + "' and month(r.Created_Date)='" + Settings.GetUTCTime().Month + "' and r.PartyDist=0 and r.Active=1";

                ddltypeFilter.SelectedValue = "New";
            }
            else if (ddltypeFilter.SelectedValue == "Old")
            {
                newPartyQry1 = @"select r.PartyName, Address = r.Address1 + r.Address2, r.Mobile, r.PartyId, r.Created_Date,r.ContactPerson,cty.AreaName as City,are.AreaName as Area,beat.AreaName as beat from MastParty r left join mastarea cty on r.CityId=cty.AreaId
                  left join mastarea are on r.AreaId=are.AreaId
                  left join mastarea beat on r.BeatId=beat.AreaId where 
                  r.UnderId=" + distID + " and year(r.Created_Date)='" + Settings.GetUTCTime().Year + "' and month(r.Created_Date)<'" + Settings.GetUTCTime().Month + "' and r.PartyDist=0 and r.Active=1";

                ddltypeFilter.SelectedValue = "Old";
            }
            else
            {
                newPartyQry1 = @"select r.PartyName, Address = r.Address1 + r.Address2, r.Mobile, r.PartyId, r.Created_Date,r.ContactPerson,cty.AreaName as City,are.AreaName as Area,beat.AreaName as beat from MastParty r left join mastarea cty on r.CityId=cty.AreaId
                  left join mastarea are on r.AreaId=are.AreaId
                  left join mastarea beat on r.BeatId=beat.AreaId where 
                  r.UnderId=" + distID + " and year(r.Created_Date)='" + Settings.GetUTCTime().Year + "' and r.PartyDist=0 and r.Active=1";

                ddltypeFilter.SelectedValue = "All";
            }

            DataTable newPartyQryDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, newPartyQry1);
            if (newPartyQryDt1.Rows.Count > 0)
            {
                newpartyrpt.DataSource = newPartyQryDt1;
                newpartyrpt.DataBind();
            }
            else
            {
                newpartyrpt.DataSource = newPartyQryDt1;
                newpartyrpt.DataBind();
            }
        }

        protected void ddltypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = ddltypeFilter.SelectedValue.ToString();
            string newPartyQryNew = string.Empty;
            if (val == "New")
            {
                newPartyQryNew = @"select r.PartyName, Address = r.Address1 + r.Address2, r.Mobile, r.PartyId, r.Created_Date,r.ContactPerson,cty.AreaName as City,are.AreaName as Area,beat.AreaName as beat from MastParty r left join mastarea cty on r.CityId=cty.AreaId
                  left join mastarea are on r.AreaId=are.AreaId
                  left join mastarea beat on r.BeatId=beat.AreaId where 
                  r.UnderId=" + distID + " and year(r.Created_Date)='" + Settings.GetUTCTime().Year + "' and month(r.Created_Date)='" + Settings.GetUTCTime().Month + "' and r.PartyDist=0 and r.Active=1";

                ddltypeFilter.SelectedValue = "New";
            }
            else if (val == "Old")
            {
                newPartyQryNew = @"select r.PartyName, Address = r.Address1 + r.Address2, r.Mobile, r.PartyId, r.Created_Date,r.ContactPerson,cty.AreaName as City,are.AreaName as Area,beat.AreaName as beat from MastParty r left join mastarea cty on r.CityId=cty.AreaId
                  left join mastarea are on r.AreaId=are.AreaId
                  left join mastarea beat on r.BeatId=beat.AreaId where 
                  r.UnderId=" + distID + " and year(r.Created_Date)='" + Settings.GetUTCTime().Year + "' and month(r.Created_Date)<'" + Settings.GetUTCTime().Month + "' and r.PartyDist=0 and r.Active=1";

                ddltypeFilter.SelectedValue = "Old";
            }
            else
            {
                newPartyQryNew = @"select r.PartyName, Address = r.Address1 + r.Address2, r.Mobile, r.PartyId, r.Created_Date,r.ContactPerson,cty.AreaName as City,are.AreaName as Area,beat.AreaName as beat from MastParty r left join mastarea cty on r.CityId=cty.AreaId
                  left join mastarea are on r.AreaId=are.AreaId
                  left join mastarea beat on r.BeatId=beat.AreaId where 
                  r.UnderId=" + distID + " and year(r.Created_Date)='" + Settings.GetUTCTime().Year + "' and r.PartyDist=0 and r.Active=1";

                ddltypeFilter.SelectedValue = "All";
            }

            DataTable newPartyQryDt1 = DbConnectionDAL.GetDataTable(CommandType.Text, newPartyQryNew);
            if (newPartyQryDt1.Rows.Count > 0)
            {
                newpartyrpt.DataSource = newPartyQryDt1;
                newpartyrpt.DataBind();
            }
            else
            {
                newpartyrpt.DataSource = newPartyQryDt1;
                newpartyrpt.DataBind();
            }
        }

        protected void Diststock_Click(object sender, EventArgs e)
        {
            distID = GetDistId(Convert.ToInt32(Settings.Instance.UserID));
            Response.Redirect("~/DistributorStockReport.aspx?DistId=" + distID);
        }

        protected void btnsecondaryorder_Click(object sender, EventArgs e)
        {
            distID = GetDistId(Convert.ToInt32(Settings.Instance.UserID));
            
            Response.Redirect("~/Retailerdispatchorderform.aspx?DistId=" + distID);
        }


    }
}