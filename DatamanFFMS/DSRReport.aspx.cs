using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.Services;
using DAL;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;

namespace AstralFFMS
{
    public partial class DSRReport : System.Web.UI.Page
    {
        int uid = 0;
        int smID = 0;
        string pageName = string.Empty;
        string pageName1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //divOrder.Style.Add("display", "none");
            //divDemo.Style.Add("display", "none");
            //divPFV.Style.Add("display", "none");
            //divPartyCollection.Style.Add("display", "none");
            //divDistributorCollection.Style.Add("display", "none");
            //divDistributorFailedvisit.Style.Add("display", "none");
            //divDistributorDiscussion.Style.Add("display", "none");
            //divRetailerDiscussion.Style.Add("display", "none");
            //divDistributorStock.Style.Add("display", "none");
            //divCompetitor.Style.Add("display", "none");
            //ItemDetail.Style.Add("display", "none");
            pageName = string.Empty;
            if (Request.QueryString["Page"] != null)
            {
                pageName = Request.QueryString["Page"];
            }

            if (!IsPostBack)
            {
                List<Order> OrderRecord1 = new List<Order>();
                OrderRecord1.Add(new Order());
                //Repeater2.DataSource = OrderRecord1;
                //Repeater2.DataBind();

                currDateLabel.Text = GetUTCTime().ToString("dd/MMM/yyyy");

            }
            if (Request.QueryString["Date"] != null && Request.QueryString["SMID"] != null)
            {
                //lblcity.Text = Request.QueryString["Citynm"];
                dateHiddenField.Value = Request.QueryString["Date"];
                smIDHiddenField.Value = Request.QueryString["SMID"];
                string Citynm = DbConnectionDAL.GetStringScalarVal("select cityname from Transvisit where smid=" + smIDHiddenField.Value + " and vdate=' " + dateHiddenField.Value + "'");
                var newString = Citynm.Replace(",", ", ");
                lblcity.Text = newString;
                pageName = Request.QueryString["PAGE"];
                statusHiddenField.Value = Request.QueryString["Recstatus"];
                smID = Convert.ToInt32(Request.QueryString["SMID"]);//GetSalesPerId(vistIDHiddenField.Value);
                saleRepName.Text = GetSalesPersonName(smID);

                string imgurl = "";
                imgurl = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SpImagepath From mastsalesrep where smid='" + smID + "'"));
                if (string.IsNullOrEmpty(imgurl))
                    imgurl = "SalespersonImages/defaultspimg.png";
                ImageMasterPage.ImageUrl = imgurl;

                dateLabel.Text = Convert.ToDateTime(dateHiddenField.Value).ToString("dd/MMM/yyyy");
                lblSalesPerson.Text = GetSalesPersonName(smID);
                lblVisitDate.Text = Convert.ToDateTime(dateHiddenField.Value).ToString("dd/MMM/yyyy");

                if (statusHiddenField.Value == "Lock")
                {
                    GetDailyWorkingReport(dateHiddenField.Value, smIDHiddenField.Value);
                }
                else if (pageName == "APPROVAL-L1")
                {
                    GetDailyWorkingReport(dateHiddenField.Value, smIDHiddenField.Value);
                }
                else if (pageName == "APPROVAL-L2")
                {
                    GetDailyWorkingReport(dateHiddenField.Value, smIDHiddenField.Value);
                }
                else if (pageName == "APPROVAL-L3")
                {
                    GetDailyWorkingReport(dateHiddenField.Value, smIDHiddenField.Value);
                }
                else
                {
                    GetDailyWorkingReport1(dateHiddenField.Value, smIDHiddenField.Value);
                }

            }

        }
        public class Order
        {
            public string CompItem { get; set; }
            public string CompQty { get; set; }
            public string ComRate { get; set; }
            public string Value { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public static string GetOrder1(string OrdId)
        {

            string str = @"select Qty,Rate from TransOrder1 where OrdId=" + OrdId + " ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            DataView dv = new DataView(dt);
            return JsonConvert.SerializeObject(dv.ToTable());
        }
        public void GetDailyWorkingReport(string VDate, string smId)
        {
            string data = string.Empty, beat = "", Query = "", QryDemo = "", QryDistCollection = "", QryPartyCollection = "", QryFv = "", QryComp = "", QryDistFv = "", QryOrder = "", QryDistDisc = "", QryDistStock = "", QryRetailerDisc = "", QryOrder1 = "", QryPartyCollection1 = "", QryDistCollection1 = "", QryDistStock1 = "";
            string QryOtherActivity = "", Query1 = "", QrySample = "", QrySample1 = "", QrySalesReturn1 = "", QrySalesReturn = "", QryProsDist="";
            DataTable dtLocTraRep = new DataTable();
            try
            {
                if (smId != "")
                {
                    string str = @"select  a.City_Name,a.Beat_Id,a.Description from (
                      select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransOrder  om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                       " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransSample om inner join MastParty p on      p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'    group by b.AreaName,b.AreaId,p.AreaId " +
                                                                                                                                                                                                  " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransSalesReturn om inner join MastParty p on                          p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'                             group by b.AreaName,b.AreaId,p.AreaId " +
                     " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +

                     " union All select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName ,p.AreaId " +

                     " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=0 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId  " +

                     " UNION All select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +

                      " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Transcompetitor om inner join MastParty p on p.PartyId=om.partyid inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and om.vdate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +

                     " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +

                      " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=0 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +

                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from DistributerCollection om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +

                      " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from TransDistStock om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +

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

                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.OrderAmount)) as Value,
                            os.Remarks,'' as CompItem,0 as CompQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from TransOrder os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId
                            left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName, os.VDate,os.PartyId,os.Remarks ,p.PartyId,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,mb.areaname";
                        }
                        else
                        {
                            QryOrder1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,
Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end as DescriptionQty,
Isnull(os.MarginPercentage,0) as Margin,ISNULL(os.DiscountType,'') as DiscountType,Isnull(os.DiscountAmount,0) as DiscountAmount,
(sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) -  Isnull(os.DiscountAmount,0)) as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,mb.areaname,os.Discount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.MarginPercentage,os.DiscountType,os.DiscountAmount,os1.OrderTakenType,os1.ExpectedDD";


                            QryOrder = @"select os.OrdId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,
'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by os.OrdId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname,os1.OrderTakenType,os1.ExpectedDD";


                        }

                        QrySample1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransSample1 os LEFT JOIN transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,os.Discount,mb.areaname";


                        QrySample = @"select os.SampleId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os1.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransSample1 os LEFT JOIN transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                       " group by os.SampleId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname";

                        QrySalesReturn1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD  from TransSalesReturn1 os LEFT JOIN TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                         " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,os.Discount,mb.areaname";


                        QrySalesReturn = @"select os.SRetId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os1.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransSalesReturn1 os LEFT JOIN TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                       " group by os.SRetId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Discount,mb.areaname";



                        QryDemo = @"select DemoId AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
                   '' as CompItem,0 as compQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address,d.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId
                   left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate='" + Settings.dateformat(VDate) + "' and d.SMId in (" + smId + ")";

                        QryFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
                   (b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") and pp.PartyDist=0) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryComp = @"select TC.COMPTID, CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,tc.Remarks AS Remarks,
                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,tc.OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId 
                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and tc.VDate='" + Settings.dateformat(VDate) + "' and tc.SMId in (" + smId + ")";

                        QryPartyCollection1 = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=1 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,mb.areaname ";

                        QryPartyCollection = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,max(tc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=1 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Latitude,tc.Longitude,tc.Address,mb.areaname";

                        QryRetailerDisc = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Retailer Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD  from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=0) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";


                        QryDistCollection1 = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,Dc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=1 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,dc.Amount,Dc.Latitude,Dc.Longitude,Dc.Address,DC.Mobile_Created_date ";

                        QryDistCollection = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,max(Dc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=1 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,Dc.Latitude,Dc.Longitude,Dc.Address";

                        QryDistFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistDisc = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistStock1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                   " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl ";

                        QryDistStock = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,max(os.ImgUrl) as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                 " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";


                        string qw = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as OrderTakenType,'' as ExpectedDD FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
               " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl ";

                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,'' as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                 CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD  from TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL";

                        //anurag-----07-07-2021 add
                        QryProsDist = @"select tv.PartyId AS COMPTID, CONVERT (varchar,tv.Insert_Date,106) as VisitDate
,'Prospect Distributor' as Stype,tv.PartyId AS PartyId,tv.PartyName as Party,'' AS Address1,tv.Mobile AS Mobile,contactPersonName AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remark as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,'' as L2Name,'' as L3Name,
                 '' as NextVisitDate,'' AS NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity,0 as stock,tv.Latitude,tv.Longitude,'' as Address,tv.mobile_created_date_time as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD
 from MastProspect_Distributor tv left join mastsalesrep msp on msp.userid=tv.[Created UserId] 
left join TransVisit vl1 on vl1.SMId=msp.SMId
                and CONVERT(DATE, tv.Insert_Date)=CONVERT(DATE, vl1.VDate) left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where vl1.SMId in (" + smId + ") and vl1.VDate='" + Settings.dateformat(VDate) + "' ";

                        Query1 = @"select *,'' as Match from (select * from (" + QryOrder1 + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection1 + " union all " + QryDistCollection1 + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock1 + " union all " + QryOtherActivity + " union all " + QrySample1 + " union all " + QrySalesReturn1 + " union all " + QryProsDist + " ) a )b Order by b.Mobile_Created_date";

                        Query = @"select *,'' as Match from (select * from (" + QryOrder + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection + " union all " + QryDistCollection + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock + " union all " + QryOtherActivity + " union all " + QrySample + " union all " + QrySalesReturn + " union all " + QryProsDist + " ) a )b Order by b.Mobile_Created_date";

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
                    else
                    {
                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,'' as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                        '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                        CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                         '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                         and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL";

                        QryProsDist = @"select tv.PartyId AS COMPTID, CONVERT (varchar,tv.Insert_Date,106) as VisitDate
,'Prospect Distributor' as Stype,tv.PartyId AS PartyId,tv.PartyName as Party,'' AS Address1,tv.Mobile AS Mobile,contactPersonName AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remark as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,'' as L2Name,'' as L3Name,
                 '' as NextVisitDate,'' AS NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity,0 as stock,tv.Latitude,tv.Longitude,'' as Address,tv.mobile_created_date_time as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD
 from MastProspect_Distributor tv left join mastsalesrep msp on msp.userid=tv.[Created UserId] 
left join TransVisit vl1 on vl1.SMId=msp.SMId
                and CONVERT(DATE, tv.Insert_Date)=CONVERT(DATE, vl1.VDate) left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where vl1.SMId in (" + smId + ") and vl1.VDate='" + Settings.dateformat(VDate) + "' ";

                        Query = @"select *,'' as Match from (select * from (" + QryOtherActivity + " union all " + QryProsDist + " ) a )b Order by b.Mobile_Created_date";
                        Query1 = @"select *,'' as Match from (select * from (" + QryOtherActivity + " union all " + QryProsDist + " ) a )b Order by b.Mobile_Created_date";
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
                    Session["Query"] = Query1;
                    Session["MainQuery"] = Query;
                }
                dtLocTraRep.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void GetDailyWorkingReport1(string VDate, string smId)
        {
            string data = string.Empty, beat = "", Query = "", QryDemo = "", QryDistCollection = "", QryPartyCollection = "", QryFv = "", QryComp = "", QryDistFv = "", QryOrder = "", QryDistDisc = "", QryDistStock = "", QryRetailerDisc = "",
           QryOrder1 = "", QryPartyCollection1 = "", QryDistCollection1 = "", QryDistStock1 = "";
            string QryOtherActivity = "", Query1 = "", QrySample = "", QrySample1 = "", QrySalesReturn = "", QrySalesReturn1 = "",QryProsDist="";
            DataTable dtLocTraRep = new DataTable();
            try
            {
                if (smId != "")
                {
                    string str = @"select  a.City_Name,a.Beat_Id,a.Description from (
                      select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransOrder  om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                                                                                                                                                                                                  " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransSample om inner join MastParty p on                               p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'                             group by b.AreaName,b.AreaId,p.AreaId " +
                                                                                                                                                                                                  " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransSalesReturn om inner join MastParty p on                               p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "'                             group by b.AreaName,b.AreaId,p.AreaId " +
                     " union all  select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransDemo om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                     " union All select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' group by  b.AreaName,b.AreaId,b.AreaName ,p.AreaId " +
                     " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_TransCollection om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=0 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId  " +
                     " UNION ALL select b.AreaName as City_Name,p.AreaId as Beat_Id,(b.AreaName) as Description from Temp_Transcompetitor om inner join MastParty p on p.PartyId=om.partyid inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") and om.vdate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.AreaId " +
                     " UNION All select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransFailedVisit om inner join MastParty p on p.PartyId=om.PartyId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                     " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=1 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +
                " UNION ALL select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransVisitDist om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId  where om.SMId in (" + smId + ") and VDate='" + Settings.dateformat(VDate) + "' AND p.PartyDist=0 group by  b.AreaName,b.AreaId,b.AreaName ,p.CityId " +


                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from DistributerCollection om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.PaymentDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransDistStock om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.CityId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
                     " UNION all select b.AreaName as City_Name,p.CityId as Beat_Id,(b.AreaName) as Description from Temp_TransDistStock om inner join MastParty p on p.PartyId=om.DistId inner join MastArea b on b.AreaId=p.AreaId where om.smid in (" + smId + ") AND p.PartyDist=1 and om.VDate='" + Settings.dateformat(VDate) + "' group by b.AreaName,b.AreaId,p.cityid " +
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
                            QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.OrderAmount)) as Value,
                   os.Remarks,'' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from Temp_TransOrder os LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId 
                   left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                           " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os.Remarks ,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date ";
                        }
                        else
                        {
                            QryOrder1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,
Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end as DescriptionQty,
Isnull(os.MarginPercentage,0) as Margin,ISNULL(os.DiscountType,'') as DiscountType,Isnull(os.DiscountAmount,0) as DiscountAmount,
(sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) -  Isnull(os.DiscountAmount,0)) as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.OrdId=os1.OrdId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                          " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os1.ImgUrl,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,mb.areaname,os.Discount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.MarginPercentage,os.DiscountType,os.DiscountAmount,os1.OrderTakenType,os1.ExpectedDD";

                            QryOrder = @"select os.OrdId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,IsNull(os1.OrderTakenType,'') as OrderTakenType,convert (varchar,os1.ExpectedDD,106) as ExpectedDD from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                          " group by os.OrdId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname,os1.OrderTakenType,os1.ExpectedDD";

                        }


                        QrySample1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSample1 os LEFT JOIN Temp_transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                         " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os1.ImgUrl,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,mb.areaname";

                        QrySample = @"select os.SampleId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSample1 os LEFT JOIN Temp_transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                      " group by os.SampleId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,mb.areaname";



                        QrySalesReturn1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSalesReturn1 os LEFT JOIN Temp_TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                        " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.PartyId,os1.ImgUrl,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os1.Mobile_Created_date,os.Discount,mb.areaname";

                        QrySalesReturn = @"select os.SRetId AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                           os1.Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,max(os.Rate) as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os1.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransSalesReturn1 os LEFT JOIN Temp_TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") and os.VDate='" + Settings.dateformat(VDate) + "' and os.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) " +
                      " group by os.SRetId,p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,mb.areaname";

                        QryDemo = @"select DemoId AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
                   '' as CompItem,0 as compQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as  Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address,d.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where d.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") ) and d.VDate='" + Settings.dateformat(VDate) + "' and d.SMId in (" + smId + ")";

                        QryFv = @"select FVId AS COMPTID,CONVERT (varchar,fv.VDate,106) as VisitDate,'Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
                   (b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ") and pp.PartyDist=0) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryComp = @"select TC.COMPTID,CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,tc.Remarks AS Remarks,
                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,OtherActivity=(Case when tc.OtherActivity=1 then 'Yes' else 'No' end), 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId
                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.PartyId in 
                   (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and tc.VDate='" + Settings.dateformat(VDate) + "' and tc.SMId in (" + smId + ")";


                        QryPartyCollection1 = @" SELECT '' AS COMPTID,convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile AS partyname,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName,0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=0 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,mb.areaname";

                        QryPartyCollection = @" SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
                   max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, (b.AreaName + ' - ' + mb.AreaName) as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,max(tc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
                   AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where tc.SMId in (" + smId + ") and tc.PaymentDate='" + Settings.dateformat(VDate) + "' and tc.PartyId in (select pp.PartyId from MastParty pp where pp.AreaId in (" + beat + ")) and p.partyDist=0 and vl1.Lock=0 group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Latitude,tc.Longitude,tc.Address,mb.areaname";

                        QryRetailerDisc = @"select VisDistId AS COMPTID,CONVERT (varchar,tv.VDate,106) as VisitDate,'Retailer Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,(b.AreaName + ' - ' + mb.AreaName) as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity,tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId left join MastArea mb on mb.AreaId=p.BeatId left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=0) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistCollection1 = @" SELECT '' AS COMPTID,convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile AS partyname,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,Dc.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=0 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,dc.Amount,Dc.Latitude,Dc.Longitude,Dc.Address,Dc.Mobile_Created_date";

                        QryDistCollection = @" SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,max(Dc.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Dc.SMId in (" + smId + ") and dc.PaymentDate='" + Settings.dateformat(VDate) + "' and dc.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")) and p.PartyDist=1 and vl1.Lock=0 group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,Dc.Latitude,Dc.Longitude,Dc.Address";


                        QryDistFv = @"select FVId AS COMPTID,CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor Non-Productive' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.PartyId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and fv.SMId in (" + smId + ") and fv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistDisc = @"select VisDistId AS COMPTID,CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity,tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=tv.SMId
                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.DistId in (select pp.PartyId from MastParty pp where pp.CityId in (" + beat + ")and pp.PartyDist=1) and tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' ";

                        QryDistStock1 = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                  " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl ";

                        QryDistStock = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,max(i.Itemname) as CompItem,max(os.Qty) as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,max(os.ImgUrl) as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
                   '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,max(os.Mobile_Created_date) as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD FROM Temp_TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SMId in (" + smId + ") " +
                 " and os.VDate='" + Settings.dateformat(VDate) + "' and os.DistId in (select pp.PartyId from MastParty pp where pp.Cityid in (" + beat + ")) group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, os.VDate,os.DistId,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address";

                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,'' as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                 CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL ";

                        QryProsDist = @"select tv.PartyId AS COMPTID, CONVERT (varchar,tv.Insert_Date,106) as VisitDate
,'Prospect Distributor' as Stype,tv.PartyId AS PartyId,tv.PartyName as Party,'' AS Address1,tv.Mobile AS Mobile,contactPersonName AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remark as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,'' as L2Name,'' as L3Name,
                 '' as NextVisitDate,'' AS NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity,0 as stock,tv.Latitude,tv.Longitude,'' as Address,tv.mobile_created_date_time as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD
 from MastProspect_Distributor tv left join mastsalesrep msp on msp.userid=tv.[Created UserId] 
left join TransVisit vl1 on vl1.SMId=msp.SMId
                and CONVERT(DATE, tv.Insert_Date)=CONVERT(DATE, vl1.VDate) left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where vl1.SMId in (" + smId + ") and vl1.VDate='" + Settings.dateformat(VDate) + "' ";

                        Query1 = @"select *,'' as Match from (select * from (" + QryOrder1 + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection1 + " union all " + QryDistCollection1 + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock1 + " union all " + QryOtherActivity + " union all " + QrySample1 + " union all " + QrySalesReturn1 + " union all " + QryProsDist + ") a ) b Order by b.Mobile_Created_date";

                        Query = @"select *,'' as Match from (select * from (" + QryOrder + " union all " + QryDemo + " union all " + QryFv + " union all " + QryComp + " union all " + QryPartyCollection + " union all " + QryDistCollection + "  union all " + QryDistFv + " union all " + QryDistDisc + " union all " + QryRetailerDisc + " union all " + QryDistStock + " union all " + QryOtherActivity + " union all " + QrySample + " union all " + QrySalesReturn + " union all " + QryProsDist + ") a ) b Order by b.Mobile_Created_date";


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
                    else
                    {
                        QryOtherActivity = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,mp.partyname as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
                        '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
                        CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                         '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD from Temp_TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
                         and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId  left join mastparty mp on mp.partyid=tv.distid  where tv.SMId in (" + smId + ") and tv.VDate='" + Settings.dateformat(VDate) + "' AND tv.Type IS NOT NULL";

                        QryProsDist = @"select tv.PartyId AS COMPTID, CONVERT (varchar,tv.Insert_Date,106) as VisitDate
,'Prospect Distributor' as Stype,tv.PartyId AS PartyId,tv.PartyName as Party,'' AS Address1,tv.Mobile AS Mobile,contactPersonName AS ContactPerson, 
                 '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remark as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,'' as L2Name,'' as L3Name,
                 '' as NextVisitDate,'' AS NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
                '' as OtherGeneralInfo,'' as OtherActivity,0 as stock,tv.Latitude,tv.Longitude,'' as Address,tv.mobile_created_date_time as Mobile_Created_date,'' as DescriptionQty,0 as Margin,'' as DiscountType,0 as DiscountAmount,0 as NetAmount,'' as OrderTakenType,'' as ExpectedDD
 from MastProspect_Distributor tv left join mastsalesrep msp on msp.userid=tv.[Created UserId] 
left join TransVisit vl1 on vl1.SMId=msp.SMId
                and CONVERT(DATE, tv.Insert_Date)=CONVERT(DATE, vl1.VDate) left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where vl1.SMId in (" + smId + ") and vl1.VDate='" + Settings.dateformat(VDate) + "' ";

                        Query = @"select *,'' as Match from (select * from (" + QryOtherActivity + " union all " + QryProsDist + " ) a )b Order by b.Mobile_Created_date";
                        Query1 = @"select *,'' as Match from (select * from (" + QryOtherActivity + " union all " + QryProsDist + " ) a )b Order by b.Mobile_Created_date";
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
                    Session["Query"] = Query1;
                    Session["MainQuery"] = Query;
                }

                dtLocTraRep.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                //HiddenField sType = (HiddenField)e.Item.FindControl("sTypeHdf");
                //HiddenField COMPTID = (HiddenField)e.Item.FindControl("hfCOMPTID");
                //HiddenField PartyId = (HiddenField)e.Item.FindControl("hfParty");
                //HiddenField VisitDate = (HiddenField)e.Item.FindControl("hfVisitDate");
                //dateHiddenField.Value = Request.QueryString["Date"];
                //smIDHiddenField.Value = Request.QueryString["SMID"];
                //statusHiddenField.Value = Request.QueryString["Recstatus"];

                //if (sType.Value.Replace(" ", string.Empty).ToLower() == "order") GetOrder(Convert.ToString(COMPTID.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "demo") GetDemo(Convert.ToString(COMPTID.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "partyfailedvisit") GetPFV(Convert.ToString(COMPTID.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "competitor") GetCompetitor(Convert.ToString(COMPTID.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "partycollection") GetPartyCollection(Convert.ToString(VisitDate.Value), Convert.ToString(PartyId.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "distributorcollection") GetDistributorCollection(Convert.ToString(VisitDate.Value), Convert.ToString(PartyId.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "distributorfailedvisit") GetDistributorFailedvisit(Convert.ToString(COMPTID.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "distributordiscussion") GetDistributorDiscussion(Convert.ToString(COMPTID.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "retailerdiscussion") GetRetailerDiscussion(Convert.ToString(COMPTID.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "distributorstock") GetDistributorStock(Convert.ToString(VisitDate.Value), Convert.ToString(PartyId.Value), Convert.ToString(smIDHiddenField.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "sample") GetSample(Convert.ToString(COMPTID.Value));
                //else if (sType.Value.Replace(" ", string.Empty).ToLower() == "salesreturn") GetSalesReturn(Convert.ToString(COMPTID.Value));
                //else GetOtherActivity(Convert.ToString(COMPTID.Value), Convert.ToString(smIDHiddenField.Value));
                //////string status = ddlDsrType.SelectedItem.Value;
                //////Response.Redirect("DSRReport.aspx?SMID=" + hdnSmiD.Value + "&Date=" + hdnDate.Value + "&Recstatus=" + status);
                ////Response.Redirect("DWSReport.aspx?SMID=" + smIDHiddenField.Value + "&Date=" + dateHiddenField.Value + "&sType=" + sType.Value + "&status=" + statusHiddenField.Value);
            }

        }

        [WebMethod(EnableSession = true)]
        public static string GetOrder(string OrdId, string Type)
        {
            string QryOrder = "";
            if (Type == "Lock")
            {
                QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
                          os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + OrdId + "" +
                               " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
            }
            else
            {
                QryOrder = @"select i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + OrdId + "" +
                                   " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
            }

            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryOrder);
            DataView dv = new DataView(dtsalesrepqry1);
            return JsonConvert.SerializeObject(dv.ToTable());
            //rptItemDetail.DataSource = dtsalesrepqry1;
            //rptItemDetail.DataBind();
            //divOrder.Style.Add("display", "block");
            //ItemDetail.Style.Add("display", "block");
            //divDemo.Style.Add("display", "none");
            //divPFV.Style.Add("display", "none");
        }

        //        public void GetOrder(string OrdId)
        //        {
        //            string QryOrder = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                 QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
        //                          os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
        //                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + OrdId + "" +
        //                                " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
        //            }
        //            else {
        //                QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Order' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
        //                          os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
        //                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + OrdId + "" +
        //                                   " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
        //            }

        //           DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryOrder);
        //           rptItemDetail.DataSource = dtsalesrepqry1;
        //           rptItemDetail.DataBind();
        //           divOrder.Style.Add("display", "block");
        //           ItemDetail.Style.Add("display", "block");
        //           divDemo.Style.Add("display", "none");
        //           divPFV.Style.Add("display", "none");
        //        }


        //        public void GetSample(string SampleId)
        //        {
        //            string QryOrder = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
        //                          os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from TransSample1 os LEFT JOIN transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
        //                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SampleId=" + SampleId + "" +
        //                               " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
        //            }
        //            else
        //            {
        //                QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Sample' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
        //                          os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from Temp_TransSample1 os LEFT JOIN Temp_transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
        //                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SampleId=" + SampleId + "" +
        //                                   " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
        //            }

        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryOrder);
        //            rptItemDetail.DataSource = dtsalesrepqry1;
        //            rptItemDetail.DataBind();
        //            divOrder.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //            divDemo.Style.Add("display", "none");
        //            divPFV.Style.Add("display", "none");
        //        }
        //        public void GetSalesReturn(string SRetId)
        //        {
        //            string QryOrder = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
        //                          os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from TransSalesReturn1 os LEFT JOIN TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
        //                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SRetId=" + SRetId + "" +
        //                               " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
        //            }
        //            else
        //            {
        //                QryOrder = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'SalesReturn' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), os.Qty*os.Rate)) as Value,
        //                          os1.Remarks,i.Itemname as CompItem,os.Qty as CompQty,os.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os1.ImgUrl as Image,'' as CompName, Case When os.Discount is NULL then 0 else os.Discount end as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date from Temp_TransSalesReturn1 os LEFT JOIN Temp_transSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId
        //                           left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SRetId=" + SRetId + "" +
        //                                   " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
        //            }

        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryOrder);
        //            rptItemDetail.DataSource = dtsalesrepqry1;
        //            rptItemDetail.DataBind();
        //            divOrder.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //            divDemo.Style.Add("display", "none");
        //            divPFV.Style.Add("display", "none");
        //        }
        //        public void GetDemo(string DemoId,string SMId)
        //        {
        //            string QryDemo = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryDemo = @"select DemoId AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
        //                   '' as CompItem,0 as compQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address,d.Mobile_Created_date from TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
        //                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId 
        //                   left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where DemoId=" + DemoId + "";
        //            }
        //            else
        //            {
        //                QryDemo = @"select DemoId AS COMPTID, CONVERT (varchar, d.VDate,106) as VisitDate,'Demo' as Stype,d.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,ic.name AS productClass,ms.name AS Segment,i.ItemName as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,d.Remarks,
        //                   '' as CompItem,0 as compQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,d.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,d.Latitude,d.Longitude,d.Address,d.Mobile_Created_date from Temp_TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN
        //                   mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join MastArea b on b.AreaId=p.AreaId 
        //                   left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where DemoId=" + DemoId + "";

        //            }
        //          DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryDemo);
        //            rptDemo.DataSource = dtsalesrepqry1;
        //            rptDemo.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divPFV.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }

        //        public void GetPFV(string FVId, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Party FailedVisit' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
        //                   b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
        //                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where FVId=" + FVId + "";
        //            }
        //            else {
        //                QryFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Party FailedVisit' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,
        //                   b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId
        //                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where FVId=" + FVId + "";
        //            }
        //           DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //           rptFV.DataSource = dtsalesrepqry1;
        //           rptFV.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divPFV.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        public void GetCompetitor(string COMPTID, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"select TC.COMPTID, CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,tc.Remarks AS Remarks,
        //                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,tc.OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date from TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId 
        //                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where TC.COMPTID=" + COMPTID + "";
        //            }
        //            else {
        //                QryFv = @"select TC.COMPTID,CONVERT (varchar, tc.VDate,106) as VisitDate,'Competitor' as Stype,tc.PartyId ,p.PartyName as Party,p.Address1,p.Mobile,p.ContactPerson, '' AS productClass,'' AS Segment,'' as [MaterialGroup],0 as Qty,0 as Rate,0 as Value,tc.Remarks AS Remarks,
        //                   tc.item as CompItem,tc.Qty as compQty,tc.Rate as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name,'' as NextVisitDate, '' as NextVisitTime,tc.ImgUrl as Image,tc.CompName as CompName,tc.Discount,tc.BrandActivity, tc.MeetActivity,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.OtherGeneralInfo,OtherActivity=(Case when tc.OtherActivity=1 then 'Yes' else 'No' end), 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date from Temp_TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId 
        //                   left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where TC.COMPTID=" + COMPTID + "";
        //            }
        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptCompetitor.DataSource = dtsalesrepqry1;
        //            rptCompetitor.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divCompetitor.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        public void GetPartyCollection(string VisitDate, string PartyId, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
        //                  max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, b.AreaName as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date FROM TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
        //                 AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Convert(varchar,tc.PaymentDate,106)='" + VisitDate + "' and tc.PartyId=" + PartyId + " And tc.SMId="+SMId+" group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date";
        //            }
        //            else {
        //                QryFv = @"SELECT '' AS COMPTID, convert (varchar,tc.PaymentDate,106) as VisitDate,'Retailer Collection' as Stype,p.PartyId, p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(Convert(numeric(18,2), tc.Amount)) AS Value,
        //                  max(tc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate, b.AreaName as Beat, max(cp.SMName) as L2Name, max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date FROM Temp_TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId
        //                 AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where convert(varchar,tc.PaymentDate,106)='" + VisitDate + "' and tc.PartyId=" + PartyId + " And tc.SMId=" + SMId + " group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date";
        //            }
        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptPartyCollection.DataSource = dtsalesrepqry1;
        //            rptPartyCollection.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divPartyCollection.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        public void GetDistributorCollection(string VisitDate, string PartyId, string SMId)
        //        {
        //            string QryFv = "";
        //            //if (statusHiddenField.Value == "Lock")
        //            //{
        //                QryFv = @"SELECT '' AS COMPTID, convert (varchar,Dc.PaymentDate,106) as VisitDate,'Distributor Collection' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' AS MaterialGroup,0 AS Qty, '' AS Rate,sum(CONVERT(numeric(18,2), dc.Amount)) as Value,
        //                   max(dc.Remarks) AS Remarks,'' AS CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat ,max(cp.SMName) as L2Name,max(cp1.SMName) as L3Name,'' as NextVisitDate, '' as NextVisitTime,'' as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,Dc.Latitude,Dc.Longitude,Dc.Address,max(Dc.Mobile_Created_date) as Mobile_Created_date FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.cityid LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId
        //                   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId Where CONVERT (varchar,Dc.PaymentDate,106)='" + VisitDate + "' and PartyId=" + PartyId + " And dc.SMId=" + SMId + " group by dc.PaymentDate,p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,Dc.Latitude,Dc.Longitude,Dc.Address ";
        //            //}
        ////            else {
        ////                QryFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor FailedVisit' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
        ////                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=fv.SMId
        ////                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId Where CONVERT (varchar,fv.VDate,106)='" + VisitDate + "' and PartyId=" + PartyId + "";
        ////            }
        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptDistributorCollection.DataSource = dtsalesrepqry1;
        //            rptDistributorCollection.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divDistributorCollection.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        public void GetDistributorFailedvisit(string FVId, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor FailedVisit' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
        //                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date from TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=fv.SMId
        //                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where FVId=" + FVId + "";
        //            }
        //            else
        //            {
        //                QryFv = @"select FVId AS COMPTID, CONVERT (varchar,fv.VDate,106) as VisitDate,'Distributor FailedVisit' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,fv.Remarks as Remarks,
        //                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName L3Name,CONVERT (varchar,fv.Nextvisit,106) as NextVisitDate,fv.VisitTime AS NextVisitTime,fv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,fv.Latitude,fv.Longitude,fv.Address,fv.Mobile_Created_date from Temp_TransFailedVisit fv inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=fv.SMId
        //                   and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where FVId=" + FVId + "";
        //            }
        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptDistributorFailedvisit.DataSource = dtsalesrepqry1;
        //            rptDistributorFailedvisit.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divDistributorFailedvisit.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        public void GetDistributorDiscussion(string VisDistId, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
        //                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId
        //                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + VisDistId + "";
        //            }
        //            else {
        //                QryFv = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Distributor Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
        //                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId
        //                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + VisDistId + "";
        //            }
        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptDistributorDiscussion.DataSource = dtsalesrepqry1;
        //            rptDistributorDiscussion.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divDistributorDiscussion.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        public void GetRetailerDiscussion(string VisDistId, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Retailer Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
        //                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date  from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId
        //                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + VisDistId + "";
        //            }
        //            else
        //            {
        //                QryFv = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,'Retailer Discussion' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-'+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson, '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,
        //                   '' as CompItem,0 as CompQty,0 as ComRate,b.AreaName as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date  from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId
        //                   and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + VisDistId + "";
        //            }
        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptRetailerDiscussion.DataSource = dtsalesrepqry1;
        //            rptRetailerDiscussion.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divRetailerDiscussion.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        public void GetDistributorStock(string VisitDate, string PartyId, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
        //                  '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where convert (varchar,os.VDate,106)='" + VisitDate + "' and PartyId=" + PartyId + " And os.SMId=" + SMId + "" +
        //                        " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, " + "os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl";
        //            }
        //            else {
        //                QryFv = @"select '' AS COMPTID, convert (varchar,os.VDate,106) as VisitDate,'Distributor Stock' as Stype,p.PartyId,p.PartyName+'-'+p.Address1+'-  '+p.Mobile as Party,p.Address1,p.Mobile,p.ContactPerson,'' as productClass,'' as Segment, '' as MaterialGroup,0 AS Qty, '' AS Rate,0 as Value,'' AS Remarks,i.Itemname as CompItem,os.Qty as CompQty,0 as ComRate,b.AreaName as Beat ,cp.SMName as L2Name,cp1.SMName as L3Name, '' as NextVisitDate, '' as NextVisitTime,os.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,
        //                  '' as RoadShow,'' as Scheme,'' as OtherGeneralInfo,'' as OtherActivity, 0 AS stock,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date FROM Temp_TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where convert (varchar,os.VDate,106)='" + VisitDate + "' and PartyId=" + PartyId + " And os.SMId=" + SMId + "" +
        //                            " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, " + "os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl";
        //            }

        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptDistributorStock.DataSource = dtsalesrepqry1;
        //            rptDistributorStock.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divDistributorStock.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        private string GetSalesPersonName(int smID)
        {
            //var query = from u in context.MastSalesReps
            //            where u.SMId == smID
            //            select new { u.SMId, u.SMName };
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
        //        public void GetOtherActivity(string COMPTID, string SMId)
        //        {
        //            string QryFv = "";
        //            if (statusHiddenField.Value == "Lock")
        //            {
        //                QryFv = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,'' as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
        //                        '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
        //                        CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
        //                         '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date from TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
        //                         and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + COMPTID + "";
        //            }
        //            else
        //            {
        //                QryFv = @"select VisDistId AS COMPTID, CONVERT (varchar,tv.VDate,106) as VisitDate,tv.Type as Stype,0 AS PartyId,mp.partyname as Party,'' AS Address1,'' AS Mobile,'' AS ContactPerson, 
        //                        '' as productClass,'' as Segment,'' as MaterialGroup,0 as Qty,0 as Rate,0 as value,tv.remarkDist as Remarks,'' as CompItem,0 as CompQty,0 as ComRate,'' as Beat,cp.SMName as L2Name,cp1.SMName as L3Name,
        //                        CONVERT (varchar,tv.NextVisitDate,106) as NextVisitDate,tv.NextVisitTime AS NextVisitTime,tv.ImgUrl as Image,'' as CompName, 0 as Discount,'' as BrandActivity, '' as MeetActivity,'' as RoadShow,'' as Scheme,
        //                         '' as OtherGeneralInfo,'' as OtherActivity, tv.stock,tv.Latitude,tv.Longitude,tv.Address,tv.Mobile_Created_date from Temp_TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId
        //                         and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId  left join mastparty mp on mp.partyid=tv.distid where VisDistId=" + COMPTID + "";
        //            }
        //            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, QryFv);
        //            rptOtherActivity.DataSource = dtsalesrepqry1;
        //            rptOtherActivity.DataBind();
        //            divOrder.Style.Add("display", "none");
        //            divDemo.Style.Add("display", "none");
        //            divCompetitor.Style.Add("display", "block");
        //            ItemDetail.Style.Add("display", "block");
        //        }
        //        private int GetSalesPerId(string p)
        //        {
        //            //var query = from u in context.TransVisits
        //            //            where u.VisId == Convert.ToInt64(p)
        //            //            select new { u.SMId };
        //            //Ankita - 17/may/2016- (For Optimization)
        //            //string salesrepqry = @"select * from TransVisit where VisId=" + Convert.ToInt64(p) + "";
        //            string salesrepqry = @"select SMId from TransVisit where VisId=" + Convert.ToInt64(p) + "";
        //            DataTable dtsalesrepqry = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry);
        //            if (dtsalesrepqry.Rows.Count>0)
        //            {
        //                return Convert.ToInt32(dtsalesrepqry.Rows[0]["SMId"]); 
        //            }
        //            else
        //            {
        //                return 0;
        //            }

        //        }

        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (pageName == "APPROVAL-L1")
            {
                Response.Redirect("~/RptDailyWorkingApprovalL1.aspx");
            }
            else
            {
                Response.Redirect("~/RptDailyWorkingSummaryL1.aspx");
            }

            //    Response.Redirect("~/RptDailyWorkingSummaryL1.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string status = "Y";
            if (items.SelectedValue == "1") status = "Y";
            else status = "N";
            if (status == "Y")
            {
                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("Vdate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("CityName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Mobile", typeof(String)));

                dtParams.Columns.Add(new DataColumn("ContactPerson", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Stype", typeof(String)));


                dtParams.Columns.Add(new DataColumn("ProductClass", typeof(String)));
                dtParams.Columns.Add(new DataColumn("ProductSegment", typeof(String)));

                dtParams.Columns.Add(new DataColumn("Productgroup", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Item", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Stock", typeof(String)));
                //dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Rate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));

                dtParams.Columns.Add(new DataColumn("NextVisitDate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("NextVisitTime", typeof(String)));
                dtParams.Columns.Add(new DataColumn("CompetitorName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Discount", typeof(String)));
                dtParams.Columns.Add(new DataColumn("OtherActivity", typeof(String)));

                dtParams.Columns.Add(new DataColumn("Remarks", typeof(String)));

                dtParams.Columns.Add(new DataColumn("Latitude", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Longitude", typeof(String)));
                dtParams.Columns.Add(new DataColumn("GAddress", typeof(String)));
                dtParams.Columns.Add(new DataColumn("MobileCreateddate", typeof(DateTime)));
                dtParams.Columns.Add(new DataColumn("DescriptionQty", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Margin", typeof(String)));
                dtParams.Columns.Add(new DataColumn("DiscountType", typeof(String)));
                dtParams.Columns.Add(new DataColumn("DiscountAmount", typeof(String)));
                dtParams.Columns.Add(new DataColumn("NetAmount", typeof(String)));


                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DSRReport.csv");
                string headertxt = "VisitDate".TrimStart('"').TrimEnd('"') + "," + "Area - Beat".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Party's Address".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"') + "," + "ContactPerson".TrimStart('"').TrimEnd('"') + "," + "Activity".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Segment".TrimStart('"').TrimEnd('"') + "," + "Product group".TrimStart('"').TrimEnd('"') + "," + "Item".TrimStart('"').TrimEnd('"') + "," + "Stock".TrimStart('"').TrimEnd('"') + ", " +
                  //  " + "Qty".TrimStart('"').TrimEnd('"') + ",
                  "Rate".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"') + "," + "NextVisitDate".TrimStart('"').TrimEnd('"') + "," + "NextVisitTime".TrimStart('"').TrimEnd('"') + "," + "CompetitorName".TrimStart('"').TrimEnd('"') + "," + "Discount".TrimStart('"').TrimEnd('"') + "," + "OtherActivity".TrimStart('"').TrimEnd('"') + "," + "Remarks".TrimStart('"').TrimEnd('"') + "," + "Latitude".TrimStart('"').TrimEnd('"') + "," + "Longitude".TrimStart('"').TrimEnd('"') + "," + "Google Address".TrimStart('"').TrimEnd('"') + "," + "Mobile Created Date".TrimStart('"').TrimEnd('"') + "," + "Description Qty".TrimStart('"').TrimEnd('"') + "," + "Margin".TrimStart('"').TrimEnd('"') + "," + "Discount Type".TrimStart('"').TrimEnd('"') + "," + "Discount Amount".TrimStart('"').TrimEnd('"') + "," + "Net Amount".TrimStart('"').TrimEnd('"');
                string _Query = Convert.ToString(Session["Query"]);

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _Query);

                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow dr = dtParams.NewRow();
                    DateTime _date = Convert.ToDateTime(dr1["Mobile_Created_date"]);
                    string _time = _date.ToString("HH:mm:ss");
                    dr["VDate"] = Convert.ToString(dr1["VisitDate"]); ;
                    dr["CityName"] = Convert.ToString(dr1["Beat"]);
                    dr["PartyName"] = Convert.ToString(dr1["Party"]);
                    dr["Address"] = Convert.ToString(dr1["Address1"]);
                    dr["Mobile"] = Convert.ToString(dr1["Mobile"]);
                    dr["ContactPerson"] = Convert.ToString(dr1["ContactPerson"]);
                    dr["Stype"] = Convert.ToString(dr1["Stype"]);
                    dr["ProductClass"] = Convert.ToString(dr1["ProductClass"]);
                    dr["ProductSegment"] = Convert.ToString(dr1["Segment"]);
                    dr["Productgroup"] = Convert.ToString(dr1["MaterialGroup"]);
                    dr["Item"] = Convert.ToString(dr1["CompItem"]);
                    dr["Stock"] = Convert.ToString(dr1["Stock"]);
                    //dr["Qty"] = Convert.ToString(dr1["CompQty"]);
                    dr["Rate"] = Convert.ToString(dr1["ComRate"]);
                    dr["Amount"] = Convert.ToString(dr1["Value"]);
                    dr["NextVisitDate"] = Convert.ToString(dr1["NextVisitDate"]);
                    dr["NextVisitTime"] = Convert.ToString(dr1["NextVisitTime"]);
                    dr["CompetitorName"] = Convert.ToString(dr1["CompName"]);
                    dr["Discount"] = Convert.ToString(dr1["Discount"]);
                    dr["OtherActivity"] = Convert.ToString(dr1["OtherActivity"]);
                    dr["Remarks"] = Convert.ToString(dr1["Remarks"]).Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                    dr["Latitude"] = Convert.ToString(dr1["Latitude"]);
                    dr["Longitude"] = Convert.ToString(dr1["Longitude"]);
                    dr["GAddress"] = Convert.ToString(dr1["Address"]);
                    dr["MobileCreateddate"] = Convert.ToDateTime(dr1["Mobile_Created_date"]);
                    dr["DescriptionQty"] = Convert.ToString(dr1["DescriptionQty"]);
                    dr["Margin"] = Convert.ToString(dr1["Margin"]);
                    dr["DiscountType"] = Convert.ToString(dr1["DiscountType"]);
                    dr["DiscountAmount"] = Convert.ToString(dr1["DiscountAmount"]);
                    dr["NetAmount"] = Convert.ToString(dr1["NetAmount"]);
                    dtParams.Rows.Add(dr);
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("Sales Person :," + saleRepName.Text);
                sb.AppendLine(System.Environment.NewLine);
                sb.Append("City :," + lblcity.Text);
                sb.AppendLine(System.Environment.NewLine);
                sb.Append(headertxt);
                sb.AppendLine(System.Environment.NewLine);

                DataView dv = dtParams.DefaultView;
                //dv.Sort = "VDate desc";
                DataTable udtNew = dv.ToTable();
                decimal[] totalVal = new decimal[29];
                try
                {
                    for (int j = 0; j < udtNew.Rows.Count; j++)
                    {
                        for (int k = 0; k < udtNew.Columns.Count; k++)
                        {

                            if (udtNew.Rows[j][k].ToString().Contains(","))
                            {
                                if (k == 0)
                                {
                                    sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                }
                                else
                                {
                                    sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                                    //Total For Columns
                                    if (k == 11 || k == 13 || k == 28)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                    //End
                                }

                            }
                            else if (udtNew.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                            {
                                if (k == 0)
                                {
                                    sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                }
                                else
                                {
                                    sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                                    //Total For Columns
                                    if (k == 11 || k == 13 || k == 28)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                    //End
                                }

                            }
                            else
                            {

                                if (k == 0)
                                {
                                    sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                }
                                else
                                {
                                    sb.Append(udtNew.Rows[j][k].ToString() + ',');
                                    //Total For Columns
                                    if (k == 11 || k == 13 || k == 28)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                    //End
                                    // }
                                }

                            }
                        }
                        sb.Append(Environment.NewLine);
                    }
                    string totalStr = string.Empty;
                    for (int x = 11; x < totalVal.Count(); x++)
                    {
                        if (x == 11 || x == 12 || x == 13 || x == 28)
                        {

                            if (totalVal[x] == 0)
                            {
                                if (x == 12) totalStr += " " + ',';
                                else totalStr += "0" + ',';
                            }
                            else
                            {
                                totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                            }
                        }
                        else
                        {
                            totalStr += " " + ',';
                        }
                        //        arr[x] = Int32.Parse(Console.ReadLine());
                    }
                    sb.Append(",,,,,,,,,,Total," + totalStr);
                    Response.Write(sb.ToString());
                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.End();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                dtParams.Dispose();
                sb.Clear();
                dt.Dispose();
                dv.Dispose();
                udtNew.Dispose();
            }
            else
            {
                string headertxt = "Time".TrimStart('"').TrimEnd('"') + "," + "Area - Beat".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Activity".TrimStart('"').TrimEnd('"') + "," + "Latitude".TrimStart('"').TrimEnd('"') + "," + "Longitude".TrimStart('"').TrimEnd('"') + "," + "Google Address".TrimStart('"').TrimEnd('"');

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DSRReport.csv");

                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("MobileCreateddate", typeof(String)));
                dtParams.Columns.Add(new DataColumn("CityName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("PartyName", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Stype", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Latitude", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Longitude", typeof(String)));
                dtParams.Columns.Add(new DataColumn("Address", typeof(String)));
                string _MainQuery = Convert.ToString(Session["MainQuery"]);
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _MainQuery);

                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow dr = dtParams.NewRow();
                    DateTime _date = Convert.ToDateTime(dr1["Mobile_Created_date"]);
                    string _time = _date.ToString("HH:mm:ss");
                    dr["MobileCreateddate"] = _time;
                    dr["CityName"] = Convert.ToString(dr1["Beat"]);
                    dr["PartyName"] = Convert.ToString(dr1["Party"]);
                    dr["Stype"] = Convert.ToString(dr1["Stype"]);
                    dr["Address"] = Convert.ToString(dr1["Address"]);
                    dr["Latitude"] = Convert.ToString(dr1["Latitude"]);
                    dr["Longitude"] = Convert.ToString(dr1["Longitude"]);

                    dtParams.Rows.Add(dr);
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("Sales Person :," + saleRepName.Text);
                sb.AppendLine(System.Environment.NewLine);
                sb.Append("VDate :," + dateLabel.Text);
                sb.AppendLine(System.Environment.NewLine);
                sb.Append("City :," + lblcity.Text);
                sb.AppendLine(System.Environment.NewLine);
                sb.Append(headertxt);
                sb.AppendLine(System.Environment.NewLine);


                DataView dv = dtParams.DefaultView;
                //  dv.Sort = "VDate desc";
                DataTable udtNew = dv.ToTable();
                for (int j = 0; j < udtNew.Rows.Count; j++)
                {
                    for (int k = 0; k < udtNew.Columns.Count; k++)
                    {

                        if (udtNew.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 0)
                            {
                                //sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(udtNew.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');

                            }

                        }
                        else if (udtNew.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                //sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(udtNew.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');

                            }

                        }
                        else
                        {

                            if (k == 0)
                            {
                                // sb.Append(Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(udtNew.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(udtNew.Rows[j][k].ToString() + ',');

                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }


                Response.Write(sb.ToString());
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
                //Response.Clear();
                //Response.Buffer = true;
                ////Response.AddHeader("content-disposition", "attachment;filename=DSRReport-L1.xls");
                //Response.AddHeader("content-disposition", "attachment;filename=DSRReport.xls");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);

                //rpt.RenderControl(hw);

                //Response.Output.Write(sw.ToString());
                //Response.Flush();
                //Response.End();

                dtParams.Dispose();
                sb.Clear();
                dt.Dispose();
                dv.Dispose();
                udtNew.Dispose();
            }
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