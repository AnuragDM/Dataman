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
//using Telerik.Web.UI;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Script.Services;
namespace AstralFFMS
{
    public partial class DailyDashBoard : System.Web.UI.Page
    {
        DataTable dtamount = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // lblAmount.Text = dtamount.Rows[0]["tamt"].ToString();
                // Totalsale();
            }
        }

        //private void Totalsale()
        //{
        //    if (FromDate.Text != "")
        //    {
        //        String str = "select sum (TotalAmount) as tamt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Temp_transorder1 where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and VDate='" + Settings.dateformat(FromDate.Text) + "'" +
        //          "union All select Isnull(sum(Qty*Rate),0) as TotalAmount from transorder1 where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and VDate='" + Settings.dateformat(FromDate.Text) + "')a";

        //        DataTable dtamount = DbConnectionDAL.getFromDataTable(str);
        //        lblAmount.Text = dtamount.Rows[0]["tamt"].ToString();
        //    }
        //}

        public string GetTPriSale(string filterdate)
        {


            string amt = DbConnectionDAL.GetStringScalarVal("select sum (TotalAmount) as totalPrimAmt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Transpurchorder tp left join Transpurchorder1 tp1 on tp.PODocId=tp1.PODocId where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and tp.VDate between '" + filterdate + " 00:00:01' and '" + filterdate + " 23:59:59')a");
            return amt;
        }
        public string GetTSecSale(string filterdate)
        {

            string amt = DbConnectionDAL.GetStringScalarVal("select sum (TotalAmount) as totalSecAmt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Temp_transorder1 where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and VDate='" + filterdate + "'" +
                  "union All select Isnull(sum(Qty*Rate),0) as TotalAmount from transorder1 where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and VDate='" + filterdate + "')a");
            return amt;
        }
        //[WebMethod(EnableSession = true)]
        //public static string FillTotalPrimarysale(string FromDate)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string filterdate = FromDate.Replace('-', '/').ToString();

        //        dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum (TotalAmount) as totalPrimAmt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Transpurchorder tp left join Transpurchorder1 tp1 on tp.PODocId=tp1.PODocId where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and tp.VDate between '" + filterdate + " 00:00:01' and '" + filterdate + " 23:59:59')a");

        //        return JsonConvert.SerializeObject(dt);
        //    }
        //    catch (Exception ex)
        //    { ex.ToString(); }
        //    return JsonConvert.SerializeObject(dt);
        //}
        [WebMethod(EnableSession = true)]
        public static string FillTotalPrimarysale(string FromDate)
        {
            DataTable dt = new DataTable();
            try
            {


                string str = "";

                ///old
                ///str="select sum (TotalAmount) as totalPrimAmt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Transpurchorder tp left join Transpurchorder1 tp1 on tp.PODocId=tp1.PODocId where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and tp.VDate between '" + filterdate + " 00:00:01' and '" + filterdate + " 23:59:59')a"




                string filterdate = FromDate.Replace('-', '/').ToString();

                str = "select sum (TotalAmount) as totalPrimAmt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Transpurchorder tp ";
                str = str + " left join Transpurchorder1 tp1 on tp.PODocId=tp1.PODocId ";
                str = str + " LEFT JOIN MastParty MP ON MP.PartyId=tp.DistId  ";
                str = str + " where tp.smid in ";
                str = str + " (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  ";
                str = str + " where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.')  AND Isnull(MP.PartyType,0)=0 and ";
                str = str + " tp.VDate between '" + filterdate + " 00:00:01' and '" + filterdate + " 23:59:59')a";

                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public static string FillTotalSecondarysale(string FromDate)
        {
            DataTable dt = new DataTable();
            try
            {
                string filterdate = FromDate.Replace('-', '/').ToString();

                dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum (TotalAmount) as totalSecAmt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Temp_transorder1 where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and VDate='" + filterdate + "'" +
                  "union All select Isnull(sum(Qty*Rate),0) as TotalAmount from transorder1 where smid in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.') and VDate='" + filterdate + "')a");

                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string FillMembers(string FromDate)
        {
            DataTable dt = new DataTable();
            try
            {
                string filterdate = FromDate.Replace('-', '/').ToString();

                string smid = Settings.Instance.SMID;


                //if (Settings.Instance.RoleType == "Admin")
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum(Person) as Person,'Total' as Status from (select count(*) as Person from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where  a.smname not in ('.') group by a.smname ) as Person Union All select  count(*) as Person, 'Present' as Status from TransVisit with (nolock) where vdate ='" + filterdate + "' and IsNull(appstatus,'DSR') !='Reject' Union All  select count(*) as Person, 'Absent' as Status from mastsalesrep with (nolock) where smname not in ('.') and smid in  (select smid from mastsalesrepgrp with (nolock) ) and active=1  and  SMId not in (select  SMId from TransVisit with (nolock) where vdate ='" + filterdate + "' Union All select  SMId  from TransLeaveRequest with (nolock) where '" + filterdate + "' between FromDate and ToDate  and  AppStatus='Approve' )  Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in  (select SMId from mastsalesrep with (nolock) where  active=1  ) and '" + filterdate + "' between FromDate and ToDate  and  IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending')");
                //else
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum(Person) as Person,'Total' as Status from (select count(*) as Person from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where  a.smname not in ('.') group by a.smname ) as Person Union All select  count(*) as Person, 'Present' as Status from TransVisit with (nolock) where vdate ='" + filterdate + "' and SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and IsNull(appstatus,'DSR') !='Reject' Union All select count(*) as Person, 'Absent' as Status from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = '" + filterdate + "' Union All select  SMId from TransLeaveRequest with (nolock) where '" + filterdate + "' between FromDate and ToDate and IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending') ) Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and '" + filterdate + "' between FromDate and ToDate  and IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending')");


                if (Settings.Instance.RoleType == "Admin")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum(Person) as Person,'Total' as Status from (select count(*) as Person from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where  a.smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and a.smid<> " + Settings.Instance.SMID + " AND a.Active=1 group by a.smname ) as Person Union All select  count(*) as Person, 'Present' as Status from TransVisit with (nolock) left join mastsalesrep a on TransVisit.SMId=a.SMId where vdate ='" + filterdate + "' and IsNull(appstatus,'DSR') !='Reject' and a.Active=1 Union All select  count(*) as Person, 'Reject' as Status from TransVisit with (nolock) left join mastsalesrep a on TransVisit.SMId=a.SMId where vdate ='" + filterdate + "' and IsNull(appstatus,'DSR') ='Reject' and a.Active=1 Union All  select count(*) as Person, 'Absent' as Status from mastsalesrep with (nolock) where smname not in ('.') and smid in  (select smid from mastsalesrepgrp with (nolock) ) and active=1  and  SMId not in (select  SMId from TransVisit with (nolock) where vdate ='" + filterdate + "' Union All select  SMId  from TransLeaveRequest with (nolock) where '" + filterdate + "' between FromDate and ToDate  and  AppStatus='Approve' )  Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in  (select SMId from mastsalesrep with (nolock) where  active=1  ) and '" + filterdate + "' between FromDate and ToDate  and  IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject','Pending')  Union All select count(*) as Person, 'Total Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where  active=1  ) and '" + filterdate + "' between FromDate and ToDate  and  IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject')");
                else
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum(Person) as Person,'Total' as Status from (select count(*) as Person from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where  a.smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and a.smid<> " + Settings.Instance.SMID + " AND a.Active=1 group by a.smname ) as Person Union All select  count(*) as Person, 'Present' as Status from TransVisit with (nolock) where vdate ='" + filterdate + "' and SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and IsNull(appstatus,'DSR') !='Reject' Union All select  count(*) as Person, 'Reject' as Status from TransVisit with (nolock) left join mastsalesrep a on TransVisit.SMId=a.SMId where vdate ='" + filterdate + "' and TransVisit.SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = 597) and active=1 and smid<> " + Settings.Instance.SMID + " ) and IsNull(appstatus,'DSR') ='Reject' and a.Active=1 Union All select count(*) as Person, 'Absent' as Status from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = '" + filterdate + "' Union All select  SMId from TransLeaveRequest with (nolock) where '" + filterdate + "' between FromDate and ToDate and IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending') ) Union All select count(*) as Person, 'Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and '" + filterdate + "' between FromDate and ToDate  and IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending') Union All select count(*) as Person, 'Total Leave' as Status from TransLeaveRequest with (nolock) where SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " ) and '" + filterdate + "' between FromDate and ToDate  and  IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject')");
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string FillProductive(string FromDate)
        {
            DataTable dt = new DataTable();
            try
            {
                string filterdate = FromDate.Replace('-', '/').ToString();

                string str = "SELECT Case when sum([Count])=0 then 0 Else sum(proCount)*100/sum([Count]) END AS Productivity, sum([Count]) [VisitedRetailer],sum(proCount) [ProductiveRetailer], isnull(sum(NewRetailer),0) AS NewRetailer from (SELECT isnull(sum([Count]), 0) [Count],isnull(sum([proCount]), 0) [proCount],sum(NewRetailer) AS NewRetailer FROM ( " +
               " SELECT d.SMId, 0 [Count],count(d.OrdDocId) [ProCount],0 AS NewRetailer FROM Temp_TransOrder d LEFT JOIN MastParty p ON d.PartyId = p.PartyId where d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId " +
               " UNION ALL SELECT d.SMId, 0 [Count],count(d.OrdDocId) [ProCount],0 AS NewRetailer FROM TransOrder d LEFT JOIN MastParty p ON d.PartyId = p.PartyId where d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId " +
               " UNION ALL SELECT d.SMId, count(*) [Count],0 as [ProCount],0 AS NewRetailer FROM [DailyCallvisitedParty] d LEFT JOIN MastParty p ON d.PartyId = p.PartyId where d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId " +
               " UNION ALL SELECT 0 AS Smid, 0 AS [Count],0 [ProCount], COUNT(PartyId) AS NewRetailer FROM MastParty mp LEFT JOIN View_SalesRepRole sn ON sn.UserId=mp.Created_User_id LEFT JOIN Transvisit tv ON CONVERT(VARCHAR(12), tv.VDate, 106) = CONVERT(VARCHAR(12), mp.insert_date, 106) AND tv.SMId = sn.SMId WHERE mp.PartyDist=0 AND mp.insert_date>='" + filterdate + " 00:00" + "' and mp.insert_date<='" + filterdate + " 23:59" + "' and sn.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY mp.insert_date) iQ ) mQ";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //  dt = DbConnectionDAL.GetDataTable(CommandType.Text, 


            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string FillProduct(string FromDate)

        {
            DataTable dt = new DataTable();
            try
            {
                string filterdate = FromDate.Replace('-', '/').ToString();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT Case when sum([Count])=0 then 0 Else sum(proCount)*100/sum([Count]) END AS Productivity, sum([Count]) [VisitedDistributor],sum(proCount) [ProductiveDistributor], isnull(sum(NewRetailer),0) AS NewRetailer from (SELECT isnull(sum([Count]), 0) [Count],isnull(sum([proCount]), 0) [proCount],sum(NewRetailer) AS NewRetailer FROM (SELECT d.SMId, count(d.PODocId) [Count],count(d.PODocId) [ProCount],0 AS NewRetailer FROM TransPurchOrder d LEFT JOIN MastParty p ON d.DistId = p.PartyId WHERE p.PartyDist=1 and d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId UNION ALL SELECT d.SMId, count(d.DiscDocid) [Count],0 AS [ProCount],0 AS NewRetailer FROM Temp_TransVisitDist d LEFT JOIN MastParty p ON d.DistId = p.PartyId WHERE p.PartyDist=1 and d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId union all SELECT d.SMId, count(d.DiscDocid) [Count],0 AS [ProCount],0 AS NewRetailer FROM TransVisitDist d LEFT JOIN MastParty p ON d.DistId = p.PartyId WHERE p.PartyDist=1 and d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId union all SELECT d.SMId , count(*) [Count],0 [ProCount],0 AS NewRetailer FROM Temp_TransFailedVisit d LEFT JOIN MastParty p ON d.PartyId = p.PartyId WHERE p.PartyDist=1 and d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId union all SELECT d.SMId , count(*) [Count],0 [ProCount],0 AS NewRetailer FROM TransFailedVisit d LEFT JOIN MastParty p ON d.PartyId = p.PartyId WHERE p.PartyDist=1 and d.VDate>='" + filterdate + " 00:00" + "' and d.VDate<='" + filterdate + " 23:59" + "' and d.smid in (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) GROUP BY d.SMId ) iQ ) mQ");

            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string FillAllRecord(string FromDate, string Type)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                string filterdate = FromDate.Replace('-', '/').ToString();
                //if (Type == "Primary")
                //{
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select * from ((select IsNull(sum (distid),0)   as Amount, 'Productive'  as Name, 1 as displayorder from (select isnull(count (distinct distid) ,0) as distid from TransPurchOrder where smid in (select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1) and VDate>='" + filterdate + " 00:00" + "' and VDate<='" + filterdate + " 23:59" + "') a) union ( select isnull(count (distinct distid) ,0)  as Amount,  'Collection' as Name, 3 as displayorder from distributercollection where smid in(select MastSalesRep.smid  from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in  (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 )  and  vdate ='" + filterdate + "' union (select isnull(sum (partyid),0) as Amount,  'Failedvisit' as Name, 2 as displayorder from  (select count (distinct a.partyid)  as partyid from  temp_TransFailedVisit A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where a.vdate ='" + filterdate + "' and b.partydist=1 and a. smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and a.vdate ='" + filterdate + "'  union select count (distinct a.partyid)  as partyid from TransFailedVisit  A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where a.vdate ='" + filterdate + "' and b.partydist=1  and a.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 ) and a.vdate ='" + filterdate + "') c)  union (select IsNull(sum (distid),0)   as Amount, 'Discussion'  as Name, 1 as displayorder from  (select isnull(count (distinct distid) ,0) as distid  from  temp_transvisitdist  where smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in   (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "' union  select  isnull(count (distinct distid) ,0)  as distid  from transvisitdist where smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "') a))) f order by displayorder");

                //    //string vb = dt.OrderBy(t => t.displayorder).Take(1);

                //   // DataTable dt1 = dt.AsEnumerable().Reverse().Take(1).CopyToDataTable();
                //   // string vb = Convert.ToString(dt1.Rows[0]["displayorder"]);
                //   // string T1=Convert.ToString(Convert.ToInt32(vb)+1);
                //   // DailyDashBoard fg = new DailyDashBoard();
                //   // string TAmount = fg.GetTSecSale(filterdate);
                //   //// string f = Convert.ToInt32(TAmount).ToString();
                //   // DataRow dr = dt.NewRow();
                //   // dr["Amount"] = "101010"; //string
                //   // dr["Name"] = "TAmount";
                //   // dr["displayorder"] = T1;
                //   // dt.Rows.Add(dr);
                //}


                if (Type == "Primary")
                {
                    //str = "select * from ((select IsNull(sum (distid),0)   as Amount, 'Productive'  as Name, 1 as displayorder ";
                    //str=str+" from (select isnull(count (distinct distid) ,0) as distid from TransPurchOrder ";
                    //str=str+ " where smid in (select MastSalesRep.smid from MastSalesRep ";
                    //str=str+" left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    //str=str+" where smid in (select distinct smid from MastSalesRepGrp where smid in ";
                    //str=str+" (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  ";
                    //str = str + " level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) ";
                    //str=str+" and MastSalesRep.Active=1) and VDate>='" + filterdate + " 00:00" + "' and VDate<='" + filterdate + " 23:59" + "') a) ";
                    //str=str+" union ( select isnull(count (distinct distid) ,0)  as Amount,  'Collection' as Name, 3 as displayorder ";
                    //str=str+" from distributercollection where smid in(select MastSalesRep.smid  from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    //str=str+" where smid in (select distinct smid from MastSalesRepGrp where smid in  (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) ";
                    //str=str+" and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 )  ";
                    //str=str+" and  vdate ='" + filterdate + "' union (select isnull(sum (partyid),0) as Amount,  'Failedvisit' as Name, 2 as displayorder from  (select count (distinct a.partyid)  as partyid ";
                    //str=str+" from  temp_TransFailedVisit A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where a.vdate ='" + filterdate + "' and b.partydist=1 and ";
                    //str=str+" a. smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    //str=str+" where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and a.vdate ='" + filterdate + "'  union select count (distinct a.partyid)  as partyid from TransFailedVisit  A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid   where a.vdate ='" + filterdate + "' and b.partydist=1  and a.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 ) and a.vdate ='" + filterdate + "') c)  union (select IsNull(sum (distid),0)   as Amount, 'Discussion'  as Name, 1 as displayorder from  (select isnull(count (distinct distid) ,0) as distid  from  temp_transvisitdist  where smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in   (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "' union  select  isnull(count (distinct distid) ,0)  as distid  from transvisitdist ";
                    //str=str+" where smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "') a))) f order by displayorder";


                    str = "select * from ((select IsNull(sum (distid),0)   as Amount, 'Productive'  as Name, 1 as displayorder ";
                    str = str + " from (select isnull(count (distinct distid) ,0) as distid from TransPurchOrder ";

                    str = str + " LEFT JOIN MastParty MP ON MP.PartyId=TransPurchOrder.DistId ";

                    ///    str = str + "  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   ";


                    str = str + " where Isnull(MP.PartyType,0)=0 and TransPurchOrder.smid in (select MastSalesRep.smid from MastSalesRep ";
                    str = str + " left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    str = str + " where smid in (select distinct smid from MastSalesRepGrp where smid in ";
                    str = str + " (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  ";
                    str = str + " level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) ";
                    str = str + " and MastSalesRep.Active=1) and VDate>='" + filterdate + " 00:00" + "' and VDate<='" + filterdate + " 23:59" + "') a) ";
                    str = str + " union ( select isnull(count (distinct distid) ,0)  as Amount,  'Collection' as Name, 3 as displayorder ";
                    str = str + " from distributercollection ";
                    str = str + "  LEFT JOIN MastParty MP ON MP.PartyId=distributercollection.DistId ";
                    //     str = str + " LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   ";
                    str = str + " where  Isnull(MP.PartyType,0)=0 and distributercollection.smid in(select MastSalesRep.smid  from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    str = str + " where smid in (select distinct smid from MastSalesRepGrp where smid in  (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) ";
                    str = str + " and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 )  ";
                    str = str + " and  vdate ='" + filterdate + "' union (select isnull(sum (partyid),0) as Amount,  'Non-Productive' as Name, 2 as displayorder from  (select count (distinct a.partyid)  as partyid ";
                    str = str + " from  temp_TransFailedVisit A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  ";
                    //      str = str + "  LEFT JOIN PartyType PT ON PT.PartytypeID=B.PartyType  ";
                    str = str + "  where Isnull(B.PartyType,0)=0 and a.vdate ='" + filterdate + "' and b.partydist=1 and ";
                    str = str + " a. smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    str = str + " where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and a.vdate ='" + filterdate + "'  union select count (distinct a.partyid)  as partyid ";
                    str = str + " from TransFailedVisit  A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  ";
                    //   str = str + "      LEFT JOIN PartyType PT ON PT.PartytypeID=B.PartyType ";
                    str = str + " where  Isnull(B.PartyType,0)=0 and  a.vdate ='" + filterdate + "' and b.partydist=1  and a.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 ) and a.vdate ='" + filterdate + "') c)  union (select IsNull(sum (distid),0)   as Amount, 'Discussion'  as Name, 1 as displayorder from  (select isnull(count (distinct distid) ,0) as distid ";
                    str = str + " from  temp_transvisitdist ";
                    str = str + "    LEFT JOIN MastParty MP ON MP.PartyId=temp_transvisitdist.DistId ";

                    str = str + " where Isnull(MP.PartyType,0)=0  and   temp_transvisitdist.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in   (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "' and Type IS NULL union  select  isnull(count (distinct distid) ,0)  as distid";
                    str = str + " from transvisitdist ";
                    str = str + "   LEFT JOIN MastParty MP ON MP.PartyId=transvisitdist.DistId    ";
                    str = str + " where  Isnull(MP.PartyType,0)=0  and  transvisitdist.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "' and Type IS NULL) a))) f order by displayorder";



                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    //string vb = dt.OrderBy(t => t.displayorder).Take(1);

                    // DataTable dt1 = dt.AsEnumerable().Reverse().Take(1).CopyToDataTable();
                    // string vb = Convert.ToString(dt1.Rows[0]["displayorder"]);
                    // string T1=Convert.ToString(Convert.ToInt32(vb)+1);
                    // DailyDashBoard fg = new DailyDashBoard();
                    // string TAmount = fg.GetTSecSale(filterdate);
                    //// string f = Convert.ToInt32(TAmount).ToString();
                    // DataRow dr = dt.NewRow();
                    // dr["Amount"] = "101010"; //string
                    // dr["Name"] = "TAmount";
                    // dr["displayorder"] = T1;
                    // dt.Rows.Add(dr);
                }
                if (Type == "Institutnal")
                {


                    str = "select * from ((select IsNull(sum (distid),0)   as Amount, 'Productive'  as Name, 1 as displayorder ";
                    str = str + " from (select isnull(count (distinct distid) ,0) as distid from TransPurchOrder ";

                    str = str + " LEFT JOIN MastParty MP ON MP.PartyId=TransPurchOrder.DistId ";

                    str = str + "  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   ";


                    str = str + " where PT.PartyTypeName='INSTITUTIONAL' and TransPurchOrder.smid in (select MastSalesRep.smid from MastSalesRep ";
                    str = str + " left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    str = str + " where smid in (select distinct smid from MastSalesRepGrp where smid in ";
                    str = str + " (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  ";
                    str = str + " level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) ";
                    str = str + " and MastSalesRep.Active=1) and VDate>='" + filterdate + " 00:00" + "' and VDate<='" + filterdate + " 23:59" + "') a) ";
                    str = str + " union ( select isnull(count (distinct distid) ,0)  as Amount,  'Collection' as Name, 3 as displayorder ";
                    str = str + " from distributercollection ";
                    str = str + "  LEFT JOIN MastParty MP ON MP.PartyId=distributercollection.DistId ";
                    str = str + " LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   ";
                    str = str + " where  PT.PartyTypeName='INSTITUTIONAL' and distributercollection.smid in(select MastSalesRep.smid  from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    str = str + " where smid in (select distinct smid from MastSalesRepGrp where smid in  (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) ";
                    str = str + " and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 )  ";
                    str = str + " and  vdate ='" + filterdate + "' union (select isnull(sum (partyid),0) as Amount,  'Non-Productive' as Name, 2 as displayorder from  (select count (distinct a.partyid)  as partyid ";
                    str = str + " from  temp_TransFailedVisit A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  ";
                    str = str + "  LEFT JOIN PartyType PT ON PT.PartytypeID=B.PartyType  ";
                    str = str + "  where  PT.PartyTypeName='INSTITUTIONAL' and a.vdate ='" + filterdate + "' and b.partydist=1 and ";
                    str = str + " a. smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId ";
                    str = str + " where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and a.vdate ='" + filterdate + "'  union select count (distinct a.partyid)  as partyid ";
                    str = str + " from TransFailedVisit  A RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  ";
                    str = str + "      LEFT JOIN PartyType PT ON PT.PartytypeID=B.PartyType ";
                    str = str + " where   PT.PartyTypeName='INSTITUTIONAL' and  a.vdate ='" + filterdate + "' and b.partydist=1  and a.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 ) and a.vdate ='" + filterdate + "') c)  union (select IsNull(sum (distid),0)   as Amount, 'Discussion'  as Name, 1 as displayorder from  (select isnull(count (distinct distid) ,0) as distid ";
                    str = str + " from  temp_transvisitdist ";
                    str = str + "    LEFT JOIN MastParty MP ON MP.PartyId=temp_transvisitdist.DistId  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   ";

                    str = str + " where PT.PartyTypeName='INSTITUTIONAL'  and  temp_transvisitdist.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in   (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "' union  select  isnull(count (distinct distid) ,0)  as distid";
                    str = str + " from transvisitdist ";
                    str = str + "   LEFT JOIN MastParty MP ON MP.PartyId=transvisitdist.DistId  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   ";
                    str = str + " where  PT.PartyTypeName='INSTITUTIONAL'  and  transvisitdist.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate ='" + filterdate + "') a))) f order by displayorder";

                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                }
                if (Type == "Secondary")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, " select * from ((select isnull(sum(partyid),0) as Amount,  'TotalOrder' as Name, 1 as displayorder from ( select count (distinct orddocid)  as partyid from transorder where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + filterdate + "' union select count (distinct orddocid) as totalorder from temp_transorder  where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp   where MainGrp in (" + Settings.Instance.SMID + "))) and vdate='" + filterdate + "') a) union select sum (partyid) as Amount,'Demo' as Name, 3 as displayorder from (select count (distinct partyid)  partyid from temp_transdemo where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from  MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + filterdate + "' union select count (distinct partyid)   partyid from transdemo where smid in (select distinct smid from MastSalesRepGrp where smid in(select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + "))) and vdate='" + filterdate + "' ) a union select IsNull(sum (partyid),0) as Amount,'Non-Productive' as name, 2 as displayorder from  (select count (distinct A.PArtyid) as partyid from  temp_TransFailedVisit  A RIGHT JOIN MASTPARTY  B ON A.PArtyid=b.partyid   where  a.vdate ='" + filterdate + "' and b.partydist=0 and a. smid in (select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from MastSalesRepGrp  where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 ) and a.vdate ='" + filterdate + "' union select  count (distinct A.PArtyid) as partyid  from TransFailedVisit  A  RIGHT JOIN MASTPARTY B ON A.PArtyid=b.partyid  where a.vdate ='" + filterdate + "' and b.partydist=0 and a.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and   level>= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 )  and  a.vdate ='" + filterdate + "') a   union  (select IsNull(sum (partyid),0) as Amount,'Competitor' as Name, 4 as displayorder from (select count (distinct partyid) partyid  from temp_transcompetitor  where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + filterdate + "'  union select count (distinct partyid) partyid   from transcompetitor where smid in (select distinct smid from MastSalesRepGrp where smid in(select smid from MastSalesRepGrp  where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + filterdate + "'  ) a  )  union   (select IsNull(sum (partyid),0) as Amount,'Collection' as Name, 5 as displayorder from (select count (distinct partyid) partyid  from temp_transcollection where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate='" + filterdate + "' union select  count (distinct partyid) partyid  from transcollection  where smid in (select distinct smid from MastSalesRepGrp where smid in(select smid from MastSalesRepGrp  where MainGrp  in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate='" + filterdate + "' ) a  ))b order by displayorder");
                if (Type == "UnApproved")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select * from (select 1 AS display, isNull(count (*) ,0) as Amount,'Leave' as Name from TransLeaveRequest left join MastSalesRep on TransLeaveRequest.SMId=MastSalesRep.SMId where VDate <='" + filterdate + "' and appstatus='Pending' and TransLeaveRequest.SMId in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp   in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp   where MainGrp in ( " + Settings.Instance.SMID + " )) and MastSalesRep.Active=1) union  select 2 AS display,count (*) as Amount,  'BeatPlan' as Name  from TransBeatPlan left join MastSalesRep on TransBeatPlan.SMId=MastSalesRep.SMId  where convert(date,created_date) <='" + filterdate + "' and appstatus='Pending' and TransBeatPlan.SMId in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp   in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in ( " + Settings.Instance.SMID + " )) and MastSalesRep.Active=1) union  select 3 AS display,count (*) as Amount,  'Tour' as Name from TransTourPlanheader left join MastSalesRep on TransTourPlanheader.SMId=MastSalesRep.SMId  where convert(date,VDate) <='" + filterdate + "' and appstatus='Pending' and TransTourPlanheader.SMId in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in ( " + Settings.Instance.SMID + " )) and MastSalesRep.Active=1) union select 4 AS display,count (*) as Amount,  'DSR' as Name from TransVisit left join MastSalesRep on TransVisit.SMId=MastSalesRep.SMId  where lock=1 and VDate <='" + filterdate + "' and appstatus is null and TransVisit.SMId in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp  in ( " + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp  where MainGrp in ( " + Settings.Instance.SMID + " )) and MastSalesRep.Active=1)) b Order by display");
                if (Type == "Order")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select workhour,cast(workhour as varchar(20))+'.00' as WHour,convert(decimal(10, 2), sum(orderamount)/1000) as OrdAmt,SMId FROM (SELECT orderamount, datepart(hh,Created_date) as workhour,SMId  frOM transorder where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + filterdate + "'), 106), ' ', '/') and orderamount<>0 union SELECT orderamount, datepart(hh,Created_date) as workhour,SMId  FROM temp_transorder where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + filterdate + "'), 106), ' ', '/') and orderamount<>0) a where  smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) group by a.workhour,SMId");
                if (Type == "Intime")
                    //dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum(InTime) InTime,Name,[date],max([time]) AS [time] from((select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'Before 9:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),created_date,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and CAST(created_date As Time) < CAST('9:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smname,smid,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'9:00-10:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),created_date,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and CAST(created_date As Time) between CAST('9:00' As Time) and CAST('10:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'10:00-11:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),created_date,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and CAST(created_date As Time) between CAST('10:00' As Time) and CAST('11:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select tv.smid,count(visid) InTime,'11:00-12:00' Name,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),created_date,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and CAST(created_date As Time) between CAST('11:00' As Time) and CAST('12:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)Union(select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),created_date,106) [date],convert(char(5), created_date, 108) [time] from(select count(visid) InTime,'After 12:00' Name,tv.smid,smname,created_date from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),created_date,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and CAST(created_date As Time) > CAST('12:00' As Time) and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,created_date) tbl group by name,smid,smname,created_date)) tbl group by Name,[date] ORDER BY [date], [time] asc");
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum(InTime) InTime,Name,[date],max([time]) AS [time] from (select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from (select tv.smid,count(visid) InTime,'Before 9:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),vDate,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and frTime1 < '09:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smname,smid,vDate,frTime1 " +

                   " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select tv.smid,count(visid) InTime,'9:00-10:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),vDate,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and frTime1 >= '09:00' and frTime1 < '10:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1 " +

                  " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select tv.smid,count(visid) InTime,'10:00-11:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),vDate,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and frTime1 >= '10:00' and frTime1 < '11:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1 " +

                  " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from (select tv.smid,count(visid) InTime,'11:00-12:00' Name,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),vDate,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and frTime1 >= '11:00' and frTime1 < '12:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1 " +

                  " Union select distinct smid,sum(InTime) InTime,Name,smname,convert(varchar(20),vDate,106) [date],convert(char(5), frTime1, 108) [time] from(select count(visid) InTime,'After 12:00' Name,tv.smid,smname,vDate,frTime1 from transvisit tv inner join MastSalesRep mp on mp.smid = tv.smid where CONVERT(VARCHAR(20),vDate,101)='" + Convert.ToDateTime(filterdate).ToString("MM/dd/yyyy") + "' and frTime1 >= '12:00' and tv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by tv.smid,smname,vDate,frTime1) tbl group by name,smid,smname,vDate,frTime1) tbl group by Name,[date] ORDER BY [date], [time] asc");
                if (Type == "NoSales")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select sum(NoSalesCount) NoSalesCount,FVName from((select count(*)NoSalesCount,mfvr.FVName,tfv.smid,mas.smname,mp.areaid,mp.beatid,ma.AreaName,ma1.AreaName BeatName from transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mp.partydist=0 and VDate='" + filterdate + "' and tfv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName )Union All(select count(*)NoSalesCount,mfvr.FVName,tfv.smid,mas.smname,mp.areaid,mp.beatid,ma.AreaName,ma1.AreaName BeatName   from temp_transfailedvisit tfv inner join MastFailedVisitRemark mfvr on tfv.ReasonId=mfvr.FVId LEFT JOIN MastParty mp ON tfv.partyid=mp.partyid left join mastsalesrep mas on tfv.smid=mas.smid left join mastarea ma on mp.areaid=ma.areaid left join mastarea ma1 on mp.beatid=ma1.areaid WHERE mp.partydist=0 and VDate='" + filterdate + "' and tfv.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1) group by FVName,tfv.smid,smname,mp.areaid,beatid,ma.areatype,ma.AreaName,ma1.AreaName )) tbl group by FVName");
                if (Type == "ProductClassWise")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select top 5 classid,convert(int,sum(Qty)) [Qty],classname from (((select Qty,(select distinct name from mastitem m inner join mastitemclass c on m.classid=c.id where itemid=t.itemid) as classname,(select distinct c.id from mastitem m inner join mastitemclass c on m.classid=c.id where itemid=t.itemid) as classid from transorder1 t where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)) UNION ALL(select Qty,(select distinct name from mastitem m inner join mastitemclass c1 on m.classid=c1.id where itemid=t1.itemid) as classname,(select distinct c1.id from mastitem m inner join mastitemclass c1 on m.classid=c1.id where itemid=t1.itemid) as classid  from Temp_TransOrder1 t1 where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)))) te group by classid,classname order by Qty desc");
                //if (Type == "ProductClassWise")
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select top 5   ms.id as classid,sum(qty) as Qty, ms.name as classname from transdistinv1 t1 left join mastitem mi on t1.itemid=mi.itemid left join mastitemsegment ms on mi.segmentid=ms.id group by ms.id, ms.name order by Qty desc");
                if (Type == "ProductSegmentWise")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select top 5 segmentid,convert(int,sum(Qty)) [Qty],segmentname from (((select Qty,(select distinct name from mastitem m inner join mastitemsegment c on m.segmentid=c.id where itemid=t.itemid) as segmentname,(select distinct c.id from mastitem m inner join mastitemsegment c on m.segmentid=c.id where itemid=t.itemid) as segmentid from transorder1 t where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)) UNION ALL (select Qty,(select distinct name from mastitem m inner join mastitemsegment c1 on m.segmentid=c1.id where itemid=t1.itemid) as segmentname,(select distinct c1.id from mastitem m inner join mastitemsegment c1 on m.segmentid=c1.id where itemid=t1.itemid) as segmentid  from Temp_TransOrder1 t1 where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)))) te group by segmentid,segmentname order by Qty desc");
                if (Type == "ASMSales")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT DISTINCT mp.smid,mp.smname,a.vdate,sum(a.qty) AS qty,a.roleid from (SELECT ms.SMId,ms.UnderId,ms.RoleId,T1.vdate,sum(qty) AS Qty FROM Temp_TransOrder1 T1 LEFT JOIN MastSalesRep ms ON ms.SMId=t1.smid WHERE T1.vdate = '" + filterdate + "' and t1.smid in(select SMId from MastSalesRep where smid in (select maingrp from MastSalesRepGrp where smid in (" + Settings.Instance.SMID + "))  or smid in (select smid from MastSalesRepGrp where maingrp in (" + Settings.Instance.SMID + "))and SMName<>'.'  and Active=1)GROUP BY ms.SMId,ms.UnderId,ms.RoleId,t1.vdate UNION all SELECT ms.SMId,ms.UnderId,ms.RoleId, T1.vdate,sum(qty) AS Qty FROM TransOrder1 T1 LEFT JOIN MastSalesRep ms ON ms.SMId=t1.smid WHERE T1.vdate = '" + filterdate + "' and t1.smid in(select SMId from MastSalesRep where smid in (select maingrp from MastSalesRepGrp where smid in (" + Settings.Instance.SMID + "))  or smid in (select smid from MastSalesRepGrp where maingrp in (" + Settings.Instance.SMID + "))and SMName<>'.'  and Active=1)GROUP BY ms.SMId,ms.UnderId,ms.RoleId,t1.vdate) a LEFT JOIN mastsalesrep mp ON mp.SMId=a.underid LEFT JOIN mastrole rm ON mp.RoleId=rm.roleid WHERE roletype IN ('DistrictHead','CityHead')GROUP BY mp.smid,mp.SMName,a.vdate,a.roleid");
                if (Type == "TopSKU")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select top 5 itemid,Sum(Qty) Qty,SUBSTRING(itemname, 1, 20) itemname from (((select t.itemid,Qty,(select itemname from mastitem where itemid=t.itemid) as itemname from transorder1 t where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1))) Union All(select t1.itemid,Qty,(select itemname from mastitem where itemid=t1.itemid) as itemname from Temp_TransOrder1 t1 where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1))) te where itemid not in (select underid from mastitem) group by itemid,itemname order by Qty desc");
                if (Type == "BottomSKU")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select top 5 itemid,Sum(Qty) Qty,SUBSTRING(itemname, 1, 20) itemname from (((select t.itemid,Qty,(select itemname from mastitem where itemid=t.itemid) as itemname from transorder1 t where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1))) Union All(select t1.itemid,Qty,(select itemname from mastitem where itemid=t1.itemid) as itemname from Temp_TransOrder1 t1 where VDate='" + filterdate + "' and smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1))) te where itemid not in (select underid from mastitem) group by itemid,itemname order by Qty asc");
                if (Type == "TopSecondarySales")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select top 5 itemname,sum(qty) qty from((SELECT mig.itemname,qty FROM Temp_TransOrder1 t1 LEFT JOIN mastitem mi ON mi.ItemId=t1.itemid LEFT JOIN mastitem mig ON mi.Underid=mig.ItemId WHERE VDate='" + filterdate + "' and t1.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)) UNION ALL(SELECT mig.itemname,qty FROM TransOrder1 t1 LEFT JOIN mastitem mi ON mi.ItemId=t1.itemid LEFT JOIN mastitem mig ON mi.Underid=mig.ItemId WHERE VDate='" + filterdate + "' and t1.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)))  tbl group by itemname order by Qty desc");
                if (Type == "BottomSecondarySales")
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, "select top 5 itemname,sum(qty) qty from((SELECT mig.itemname,qty FROM Temp_TransOrder1 t1 LEFT JOIN mastitem mi ON mi.ItemId=t1.itemid LEFT JOIN mastitem mig ON mi.Underid=mig.ItemId WHERE VDate='" + filterdate + "' and t1.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)) UNION ALL(SELECT mig.itemname,qty FROM TransOrder1 t1 LEFT JOIN mastitem mi ON mi.ItemId=t1.itemid LEFT JOIN mastitem mig ON mi.Underid=mig.ItemId WHERE VDate='" + filterdate + "' and t1.smid in(SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + Settings.Instance.SMID + ")) and Active=1)))  tbl group by itemname order by Qty asc");
            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(dt);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DashBoard.aspx");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        [WebMethod(EnableSession = true)]
        public static string Getcountofinstitutnalparty()
        {
            DataTable dt = new DataTable();
            int cnt = 0;
            string res = "A";
            string str = "";
            try
            {
                str = "SELECT count(*) FROM MastParty MP LEFT JOIN partytype PT ON PT.PartyTypeId=MP.PartyType ";
                str = str + " WHERE  PT.PartyTypeName='INSTITUTIONAL' ";

                cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                if (cnt > 0)
                {
                    res = "P";
                }
                //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(res);
        }

        [WebMethod(EnableSession = true)]
        public static string FillTotalInsPrimarysale(string FromDate)
        {
            DataTable dt = new DataTable();
            try
            {
                string filterdate = FromDate.Replace('-', '/').ToString();

                string str = "";

                str = "select sum (TotalAmount) as totalPrimAmt from (select Isnull(sum(Qty*Rate),0) as TotalAmount from Transpurchorder tp ";
                str = str + " left join Transpurchorder1 tp1 on tp.PODocId=tp1.PODocId ";
                str = str + " LEFT JOIN MastParty MP ON MP.PartyId=tp.DistId  LEFT JOIN PartyType PT ON PT.PartyTypeId=MP.PartyType ";
                str = str + " where tp.smid in ";
                str = str + " (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  ";
                str = str + " where maingrp = " + Settings.Instance.SMID + ") and active=1 and smname<> '.')  AND PT.PartyTypeName='INSTITUTIONAL'  and ";
                str = str + " tp.VDate between '" + filterdate + " 00:00:01' and '" + filterdate + " 23:59:59')a";

                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            { ex.ToString(); }
            return JsonConvert.SerializeObject(dt);
        }
    }
}