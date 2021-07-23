using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class DSRReportL3 : System.Web.UI.Page
    {
         int uid = 0;
         int smID = 0;
         string pageName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            pageName = string.Empty;
            if (Request.QueryString["Page"] != null)
            {
                pageName = Request.QueryString["Page"];
            }
            if (!IsPostBack)
            {
                currDateLabel.Text = GetUTCTime().ToString("dd/MMM/yyyy");
                
            }
            //if (Request.QueryString["Date"] != null && Request.QueryString["SMID"] != null && Request.QueryString["Recstatus"] != null)
            if (Request.QueryString["Date"] != null && Request.QueryString["SMID"] != null)
            {
                dateHiddenField.Value = Request.QueryString["Date"];
                smIDHiddenField.Value = Request.QueryString["SMID"];
                //string status = Request.QueryString["Recstatus"];
                statusHiddenField.Value = Request.QueryString["Recstatus"];
                smID = Convert.ToInt32(Request.QueryString["SMID"]);
                saleRepName.Text = GetSalesPersonName(smID);
                dateLabel.Text = Convert.ToDateTime(dateHiddenField.Value).ToString("dd/MMM/yyyy");
                //if (status == "Lock")
                //{
                //    GetDailyWorkingReport(dateHiddenField.Value, smIDHiddenField.Value);
                //}
                //else
                //{
                //    GetDailyWorkingReport1(dateHiddenField.Value, smIDHiddenField.Value);

                //}
                if (statusHiddenField.Value == "Lock")
                {
                    GetDailyWorkingReport(dateHiddenField.Value, smIDHiddenField.Value);
                }
                else if (pageName == "APPROVAL-L3")
                { GetDailyWorkingReport(dateHiddenField.Value, smIDHiddenField.Value); }
                else
                {
                    GetDailyWorkingReport1(dateHiddenField.Value, smIDHiddenField.Value);

                }
            }
        }
        //[WebMethod]
        public void GetDailyWorkingReport(string VDate, string smId)
        {
            string data = string.Empty, beat = "", Query = "", QryDemo = "", QryDistCollection = "", QryPartyCollection = "", QryFv = "", QryComp = "", QryDistFv = "", QryOrder = "", QryDistDisc = "";
            DataTable dtLocTraRep = new DataTable();
            try
            {
                if (smId != "")
                {
                    string str = @"select  a.City_Name,a.Beat_Id,a.Description from (
                      select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransOrder  om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                      " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                      " union All select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName ,p.AreaId " +
                      " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=0 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId  " +
                      " UNION All select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                      " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                      " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from DistributerCollection om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
                      " UNION all select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from MastParty p inner join MastArea b on b.AreaId=p.AreaId where p.UserId in (" + smId + ") and p.Created_Date='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName,p.AreaId )a Group by a.City_Name,a.Beat_Id,a.Description Order by a.Description";

                    DataTable dtbeats = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dtbeats.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtbeats.Rows.Count; i++)
                        {
                            beat += dtbeats.Rows[i]["Beat_Id"].ToString() + ",";
                        }
                        beat = beat.TrimStart(',').TrimEnd(',');

                        if (Settings.Instance.OrderEntryType == "1")
                        {
                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.OrderAmount)) as Value,
                   os.Remarks,'' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address from TransOrder os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId 
                   left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                       " group by p.PartyName, os.VDate,os.PartyId,os.Remarks ,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";

                        }
                        else
                        {
                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address from TransOrder1 os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName, os.VDate,os.PartyId,os.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";
                        }

                        QryDemo = @"select '' AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName as Party,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
                   '' as CompItem,0 as compQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address  from TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate='" + Settings.dateformat(VDate) + "' and d.SMId in (" + smId + ")";

                        QryFv = @"select '' AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'party FailedVisit' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
                   b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,''  as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address  from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") and pp.PartyDist=0) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryComp = @"select TC.COMPTID,CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName as Party, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,'' AS Remarks,
                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,tc.OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address  from TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId 
                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and tc.VDate='" + Settings.dateformat(VDate) + "' and tc.SMId in (" + smId + ")";


                   QryPartyCollection = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Party Collection' as Stype,p.PartyId, p.partyname AS partyname,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, b.AreaName as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address FROM TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=1 group by tc.PaymentDate,p.PartyId,p.partyName,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address";


                   QryDistCollection = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.partyname AS partyname,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address  FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.cityid LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.lock=1 group by dc.PaymentDate,p.PartyId,p.Partyname,b.AreaName,dc.Amount,Dc.Latitude,Dc.Longitude,Dc.Address ";

                   QryDistFv = @"select '' AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor FailedVisit' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address  from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                   QryDistDisc = @"select '' AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName as Party, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                    Query = @"select *,'' as Match from (select * from (" + QryOrder + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection + " union all " + QryDistCollection + "  union all " + QryDistFv + " union all " + QryDistDisc + ") a )b Order by b.visitDate,b.partyid ";

                        
                        
                   dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                   
                    if (dtLocTraRep.Rows.Count > 0)
                        {                           
                            rpt.DataSource = dtLocTraRep;
                            rpt.DataBind();
                        }
                        else
                        {
                            rpt.DataSource = dtLocTraRep;
                            rpt.DataBind();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        public void GetDailyWorkingReport1(string VDate, string smId)
        {
            string data = string.Empty, beat = "", Query = "", QryDemo = "", QryDistCollection = "", QryPartyCollection = "", QryFv = "", QryComp = "", QryDistFv = "", QryOrder = "", QryDistDisc = "";
            DataTable dtLocTraRep = new DataTable();
            try
            {
                if (smId != "")
                {
                    string str = @"select  a.City_Name,a.Beat_Id,a.Description from (
                      select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransOrder  om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                     " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                     " union All select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName ,p.AreaId " +
                     " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=0 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId  " +
                     " UNION All select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                     " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from DistributerCollection om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
                     " UNION all select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from MastParty p inner join MastArea b on b.AreaId=p.AreaId where p.UserId in (" + smId + ") and p.Created_Date='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName,p.AreaId )a Group by a.City_Name,a.Beat_Id,a.Description Order by a.Description";

                    DataTable dtbeats = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    if (dtbeats.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtbeats.Rows.Count; i++)
                        {
                            beat += dtbeats.Rows[i]["Beat_Id"].ToString() + ",";
                        }
                        beat = beat.TrimStart(',').TrimEnd(',');

                        if (Settings.Instance.OrderEntryType == "1")
                        {

                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.OrderAmount)) as Value,
                   os.Remarks,'' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address from Temp_TransOrder os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId 
                   left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName, os.VDate,os.PartyId,os.Remarks ,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";
                        }
                        else
                        {
                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address from Temp_TransOrder1 os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                         " group by p.PartyName, os.VDate,os.PartyId,os.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";

                        }

                        QryDemo = @"select '' AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName as Party,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
                   '' as CompItem,0 as compQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address from Temp_TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join Temp_TransCompetitor c ON
                   c.PartyId=d.PartyId and c.VDate=d.VDate left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate='" + Settings.dateformat(VDate) + "' and d.SMId in (" + smId + ")";

                        QryFv = @"select '' AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'party FailedVisit' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
                   b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") and pp.PartyDist=0) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryComp = @"select TC.COMPTID, CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName as Party, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,tc.Remarks AS Remarks,
                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,tc.OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address from Temp_TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId 
                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and tc.VDate='" + Settings.dateformat(VDate) + "' and tc.SMId in (" + smId + ")";


                        QryPartyCollection = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Party Collection' as Stype,p.PartyId, p.partyname AS partyname,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, b.AreaName as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address FROM Temp_TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=0 group by tc.PaymentDate,p.PartyId,p.partyName,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address";


                        QryDistCollection = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.partyname AS partyname,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.cityid LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=0 group by dc.PaymentDate,p.PartyId,p.Partyname,b.AreaName,dc.Amount,Dc.Latitude,Dc.Longitude,Dc.Address";

                        QryDistFv = @"select '' AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor FailedVisit' as Stype,p.PartyId,p.PartyName as Party,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistDisc = @"select '' AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName as Party, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                        Query = @"select *,'' as Match from (select * from (" + QryOrder + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection + " union all " + QryDistCollection + "  union all " + QryDistFv + " union all " + QryDistDisc + ") a )b Order by b.visitDate,b.partyid ";


                        dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                        if (dtLocTraRep.Rows.Count > 0)
                        {

                            rpt.DataSource = dtLocTraRep;
                            rpt.DataBind();
                        }
                        else
                        {

                            rpt.DataSource = null;
                            rpt.DataBind();

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private string GetSalesPersonName(int smID)
        {
            //Ankita - 17/may/2016- (For Optimization)
            //string salesrepqry1 = @"select * from MastSalesRep where SMId=" + smID + "";
            string salesrepqry1 = @"select SMName from MastSalesRep where SMId=" + smID + "";
            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry1);
            if (dtsalesrepqry1.Rows.Count > 0)
            {
                return Convert.ToString(dtsalesrepqry1.Rows[0]["SMName"]);
            }
            else
            {
                return string.Empty;
            }
        }
        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDailyWorkingSummaryL3.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DSRReport-L3.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            rpt.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }


        protected void lnkViewDemoImg_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                var item = (RepeaterItem)btn.NamingContainer;
                HiddenField hdnId = (HiddenField)item.FindControl("linkHiddenField");
                HiddenField sTypeHdf = (HiddenField)item.FindControl("sTypeHdf");
                if (sTypeHdf.Value == "Distributor Discussion" || sTypeHdf.Value == "Competitor" || sTypeHdf.Value == "Demo")
                {
                    Response.ContentType = ContentType;
                    if (hdnId.Value != "")
                    {
                        btn.Visible = true;
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdnId.Value));
                        Response.WriteFile(hdnId.Value);
                        Response.End();
                    }
                    else
                    {
                        btn.Visible = false;
                    }
                }
                else
                {
                    btn.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}