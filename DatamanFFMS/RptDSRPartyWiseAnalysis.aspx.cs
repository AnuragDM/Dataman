using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using System.Web.Services;

namespace AstralFFMS
{
    public partial class RptDSRPartyWiseAnalysis : System.Web.UI.Page
    {
        int smID = 0;
        int msg = 0;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            frmTextBox.Attributes.Add("readonly", "readonly");
            toTextBox.Attributes.Add("readonly", "readonly");
            if (!Page.IsPostBack)
            {
                Bindstate();
                List<PartyWisedeatils> PartyWisedeatil = new List<PartyWisedeatils>();
                PartyWisedeatil.Add(new PartyWisedeatils());
                rpt.DataSource = PartyWisedeatil;
                rpt.DataBind();               
                if (Request.QueryString["Image"] != null)
                {
                    try
                    {
                        string Image = Convert.ToString(Request.QueryString["Image"]);
                        string sType = Convert.ToString(Request.QueryString["Stype"]);                       
                            Response.ContentType = ContentType;
                            if (Image != "")
                            {                               
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(Image));
                                Response.WriteFile(Image);
                                Response.End();
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('No Image Available');", true); 
                                string Image1 = "~/DSRImages/DistDisc_NoImage.jpg";
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(Image1));
                                Response.WriteFile(Image1);
                                Response.End();
                                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('No Image Available');", true); 
                            }                        

                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }                    
                }
                //string pageName = Path.GetFileName(Request.Path);
                btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
                frmTextBox.Text = Settings.GetUTCTime().AddDays(-6).ToString("dd/MMM/yyyy");
                toTextBox.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                roleType = Settings.Instance.RoleType;
                trview.Attributes.Add("onclick", "fireCheckChanged(event)");
               // fill_TreeArea();      
                BindTreeViewControl();
            }
        }
        private void Bindstate()
        {
            try
            {
                if (Settings.Instance.RoleType == "Admin")
                {
                    string stateQry = @"select distinct stateid,statename from viewgeo where stateAct=1 order by statename";
                    DataTable dtMastState = DbConnectionDAL.GetDataTable(CommandType.Text, stateQry);
                    if (dtMastState.Rows.Count > 0)
                    {
                       
                        ddlState.DataSource = dtMastState;
                        ddlState.DataTextField = "statename";
                        ddlState.DataValueField = "stateid";
                        ddlState.DataBind();
                    }
                    ddlState.Items.Insert(0, new ListItem("--Please select--", "0"));
                }
                else
                {
                    string stateQry = @"select distinct stateid,statename from viewgeo where stateAct=1 and areaid in (SELECT distinct linkcode FROM MastLink WHERE primcode in
                   (SELECT smid FROM MastSalesRep WHERE SMId IN (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN ('" + Settings.Instance.SMID + "')) and Active=1)) order by statename";
                    DataTable dtMastState = DbConnectionDAL.GetDataTable(CommandType.Text, stateQry);
                    if (dtMastState.Rows.Count > 0)
                    {
                        ddlState.DataSource = dtMastState;
                        ddlState.DataTextField = "statename";
                        ddlState.DataValueField = "stateid";
                        ddlState.DataBind();
                    }
                    ddlState.Items.Insert(0, new ListItem("--Please select--", "0"));
                }
               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public class PartyWisedeatils
        {
            public string Mobile_Created_date { get; set; }
            public string VisitDate { get; set; }
            public string SyncId  { get; set; }
            public string Smname { get; set; }
            public string SActive { get; set; }
            public string Beat { get; set; }
            public string PartyId { get; set; }
            public string Party { get; set; }
            public string Address1 { get; set; }
            public string PActive { get; set; }
            public string Stype { get; set; }
            public string productClass { get; set; }
            public string Segment { get; set; }
            public string MaterialGroup { get; set; }
            public string Value { get; set; }
            public string CompItem { get; set; }
            public string CompQty { get; set; }
            public string ComRate { get; set; }
            public string NextVisitDate { get; set; }
            public string NextVisitTime { get; set; }
            public string CompName { get; set; }
            public string BrandActivity { get; set; }
            public string MeetActivity { get; set; }
            public string OtherActivity { get; set; }
            public string OtherGeneralInfo { get; set; }
            public string RoadShow { get; set; }
            public string Scheme { get; set; }
            public string Discount { get; set; }
            public string Remarks { get; set; }
            public string Image { get; set; }

        }

        [WebMethod(EnableSession = true)]
        public static string Getpartywisedetails(string View, string StateId,string CityId, string PartyId, string PartyType, string SalesPersonType, string DsrType, string Status, string Fromdate, string Todate,string type)
        {
            string data = string.Empty, Query = "", QryChk = "";
            string partyids = "";
            string Smid = "";
            string a = "";
            string b = "";
            if (View != "Party")
            {
                Smid = HttpContext.Current.Session["treenodes"].ToString();
            }           
          
           if (CityId !="0" && PartyId == "")
           {
                string partyQry = @"select partyid from mastparty WHERE Areaid IN (SELECT DISTINCT areaid  FROM ViewGeo WHERE cityid ='" + CityId + "') AND partydist=0";
                DataTable dtMastParty = DbConnectionDAL.GetDataTable(CommandType.Text, partyQry);
                if (dtMastParty.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtMastParty.Rows.Count - 1; i++)
                    {
                        partyids = dtMastParty.Rows[i]["partyid"].ToString();
                        a += partyids + ",";
                    }
                }

                b = a.TrimStart(',').TrimEnd(',');
            }

           if (PartyId != "") 
           {
               b = PartyId;
           }
            
                if (DsrType == "Lock")
                { QryChk = "and vl1.lock =1"; }
                if (DsrType == "UnLock")
                { QryChk = "and vl1.lock = 0"; }
                if (DsrType == "All")
                { QryChk = "and vl1.lock  in (1,0)"; }

                if (PartyType != "All")
                {
                    if (PartyType == "Active")
                    { QryChk += "and p.Active =1"; }
                    else
                    { QryChk += "and p.Active =0"; }
                }
                if (SalesPersonType != "All")
                {
                    if (SalesPersonType == "Active")
                    { QryChk += "and sn.Active =1"; }
                    else
                    { QryChk += "and sn.Active =0"; }
                }

                if (Status != "3")
                {
                    if (Status == "Pending")
                    { QryChk += " and vl1.AppStatus is null "; }
                    else
                    { QryChk += " and vl1.AppStatus='" + Status + "'"; }
                }
                if (Smid != "")
                {
//                    string QryOrder = @"SELECT case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive,'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
//                                                      sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty, os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
//                                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount,os.latitude,os.longitude,os.[address] FROM TransOrder os  LEFT JOIN TransOrder1 os1 ON os.OrdId=os1.OrdId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
//                                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.SMId in (" + Smid + ") and os.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " " +
//                                      " GROUP BY sn.active,p.Active,p.PartyName, os.VDate, sn.SMId, os1.Qty,os1.Rate,os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,mi.ItemName,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string QryOrder = @"SELECT case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive,'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, max(itmcls.Name) AS productClass, max(itmseg.Name) AS Segment, max(igrp.ItemName) AS MaterialGroup, sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty, os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount,os.latitude,os.longitude,os.[address] FROM TransOrder os  LEFT JOIN TransOrder1 os1 ON os.OrdDocId=os1.OrdDocId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId   left join MastItem igrp on igrp.ItemId=mi.Underid left join MastItemClass itmcls on itmcls.id=mi.ClassId left join MastItemSegment itmseg on itmseg.Id=mi.SegmentId where os.SMId in (" + Smid + ") and os.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " " +
                                      " GROUP BY sn.active,p.Active,p.PartyName, os.VDate, sn.SMId, os1.Qty,os1.Rate,os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,mi.ItemName,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";



//                    string TempQryOrder = @"SELECT case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
//                                      sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty, os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
//                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount,os.latitude,os.longitude,os.[address]   FROM Temp_TransOrder os  LEFT JOIN Temp_TransOrder1 os1 ON os.OrdId=os1.OrdId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
//                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.SMId in (" + Smid + ") and os.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + "" +
//                                          " GROUP BY sn.active,p.Active,p.PartyName,os1.Qty,os1.Rate, os.VDate, sn.SMId, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,mi.ItemName,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string TempQryOrder = @"SELECT case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date ,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive,  max(itmcls.Name) AS productClass, max(itmseg.Name) AS Segment, max(igrp.ItemName) AS MaterialGroup, sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty, os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount,os.latitude,os.longitude,os.[address]   FROM Temp_TransOrder os  LEFT JOIN Temp_TransOrder1 os1 ON os.OrdDocId=os1.OrdDocId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId left join MastItem igrp on igrp.ItemId=mi.Underid left join MastItemClass itmcls on itmcls.id=mi.ClassId left join MastItemSegment itmseg on itmseg.Id=mi.SegmentId where os.SMId in (" + Smid + ") and os.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + "" +
                                        " GROUP BY sn.active,p.Active,p.PartyName,os1.Qty,os1.Rate, os.VDate, sn.SMId, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,mi.ItemName,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string Demo = @"SELECT DISTINCT case when d.Mobile_Created_date is null then d.VDate else d.Mobile_Created_date end Mobile_Created_date ,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                               vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,d.latitude,d.longitude,d.[address]  FROM TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN 
                                MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                               TransVisit vl1 ON vl1.SMId = d .SMId and d .VDate = vl1.VDate where d.SMId in (" + Smid + ") and d.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + "";

                    string tempDemo = @"SELECT DISTINCT case when d.Mobile_Created_date is null then d.VDate else d.Mobile_Created_date end Mobile_Created_date ,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                                 vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,d.latitude,d.longitude,d.[address]  FROM Temp_TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN
                                 MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                                 TransVisit vl1 ON vl1.SMId = d .SMId AND d .VDate = vl1.VDate where d.SMId in (" + Smid + ") and d.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + "";

                    string FailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date ,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                  fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount,fv.latitude,fv.longitude,fv.[address]  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                                  TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.SMId in (" + Smid + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string tempFailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date ,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                     fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName,
                                     vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                     TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.SMId in (" + Smid + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string Comp = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date ,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value,
                              tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.Discount ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + Smid + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string tempComp = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value, 
                                  tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.Discount ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + Smid + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    //collection add in details 07-07-2021----- anurag

                    string Coll = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date ,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount))  AS Value,
                              tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + Smid + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    string tempColl = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount)) AS Value, 
                                  tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + Smid + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    //complete 07-07-2021----- anurag

                    string dcollection = @"SELECT case when Dc.Mobile_Created_date is null then Dc.PaymentDate else Dc.Mobile_Created_date end Mobile_Created_date ,Dc.PaymentDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Collection' AS Stype, p.PartyId, p.partyname AS partyname,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, sum(CONVERT(numeric(18, 2), dc.Amount)) AS Value, max(dc.Remarks) AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, 
                                     '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,Dc.latitude,Dc.longitude,Dc.[address]  FROM DistributerCollection Dc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = Dc.SMId LEFT JOIN mastparty p ON Dc.DistId = p.PartyId LEFT JOIN 
                                      MastArea b ON b.AreaId = p.cityid LEFT JOIN TransVisit vl1 ON vl1.SMId = dc.SMId AND vl1.VDate = dc.PaymentDate where Dc.SMId in (" + Smid + ") and Dc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,dc.PaymentDate, sn.SMId, p.PartyId, p.Partyname,p.Address1, b.AreaName, dc.Amount, vl1.AppStatus, vl1.Lock,Dc.Mobile_Created_date,Dc.latitude,Dc.longitude,Dc.[address]";

                    string dFailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1, case P.Active when 1 then 'Yes' else 'No' end as PActive,'' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                    0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.SMId in (" + Smid + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string temdFailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                     0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount,fv.latitude,fv.longitude,fv.[address]   FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN
                                     MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.SMId in (" + Smid + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string discussion = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,tv.latitude,tv.longitude,tv.[address]  FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + Smid + ") and tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string DepoMeetHeadOffice = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + Smid + ") and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and  tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string tempdiscussion = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1, case P.Active when 1 then 'Yes' else 'No' end as PActive,'' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + Smid + ") and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";


                    string tempDepoMeetHeadOffice = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1, case P.Active when 1 then 'Yes' else 'No' end as PActive,'' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + Smid + ") and tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";


                    Query = " (" + QryOrder + " union all " + TempQryOrder + " union all " + Demo + " union all " + tempDemo + " union all " + FailedV + " union all " + tempFailedV + "  union all " + Comp + " union all " + tempComp + "  union all " + Coll + " union all " + tempColl + " union all " + dcollection + " union all " + dFailedV + " union all " + temdFailedV + "  union all " + discussion + " union all " + tempdiscussion + " union all " + DepoMeetHeadOffice + " union all " + tempDepoMeetHeadOffice + ")";
                }
                else
                {
                    string QryOrder = @"SELECT case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                  sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,os.latitude,os.longitude,os.[address]  FROM TransOrder os LEFT JOIN Temp_TransOrder1 os1 ON os.OrdDocId=os1.OrdDocId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                  Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate  LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.partyid in (" + b + ") and os.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " " +
                                     " GROUP BY sn.active,p.Active,p.PartyName, os.VDate, sn.SMId,os1.Qty,os1.Rate, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,mi.ItemName,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string TempQryOrder = @"SELECT case when os.Mobile_Created_date is null then os.VDate else os.Mobile_Created_date end Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                      sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,os.latitude,os.longitude,os.[address]  FROM Temp_TransOrder os LEFT JOIN Temp_TransOrder1 os1 ON os.OrdDocId=os1.OrdDocId  LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate  LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.partyid in (" + b + ") and os.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + "" +
                                          " GROUP BY sn.active,p.Active,p.PartyName, os.VDate, sn.SMId,os1.Qty,os1.Rate, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,mi.ItemName,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string Demo = @"SELECT DISTINCT case when d.Mobile_Created_date is null then d.VDate else d.Mobile_Created_date end Mobile_Created_date,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                               vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,d.latitude,d.longitude,d.[address]   FROM TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN 
                                MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                               TransVisit vl1 ON vl1.SMId = d .SMId and d .VDate = vl1.VDate where d.partyid in (" + b + ") and d.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + "";

                    string tempDemo = @"SELECT DISTINCT case when d.Mobile_Created_date is null then d.VDate else d.Mobile_Created_date end Mobile_Created_date,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                                 vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount   ,d.latitude,d.longitude,d.[address]  FROM Temp_TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN
                                 MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                                 TransVisit vl1 ON vl1.SMId = d .SMId AND d .VDate = vl1.VDate where d.partyid in (" + b + ") and d.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + "";

                    string FailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1, case P.Active when 1 then 'Yes' else 'No' end as PActive,'' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                  fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                                  TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string tempFailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                     fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName,
                                     vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                     TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string Comp = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value,
                              tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.Discount  ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string tempComp = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value, 
                                  tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.Discount  ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    //collection add in details 07-07-2021----- anurag

                    string Coll = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date ,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount)) AS Value,
                              tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    string tempColl = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount)) AS Value, 
                                  tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    //complete 07-07-2021----- anurag

                    string dcollection = @"SELECT case when Dc.Mobile_Created_date is null then Dc.PaymentDate else Dc.Mobile_Created_date end Mobile_Created_date,Dc.PaymentDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Collection' AS Stype, p.PartyId, p.partyname AS partyname,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, sum(CONVERT(numeric(18, 2), dc.Amount)) AS Value, max(dc.Remarks) AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, 
                                     '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,Dc.latitude,Dc.longitude,Dc.[address]   FROM DistributerCollection Dc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = Dc.SMId LEFT JOIN mastparty p ON Dc.DistId = p.PartyId LEFT JOIN 
                                      MastArea b ON b.AreaId = p.cityid LEFT JOIN TransVisit vl1 ON vl1.SMId = dc.SMId AND vl1.VDate = dc.PaymentDate where Dc.distid in (" + b + ") and Dc.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,dc.PaymentDate, sn.SMId, p.PartyId, p.Partyname,p.Address1, b.AreaName, dc.Amount, vl1.AppStatus, vl1.Lock,Dc.Mobile_Created_date,Dc.latitude,Dc.longitude,Dc.[address]";

                    string dFailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                    0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,fv.latitude,fv.longitude,fv.[address]  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string temdFailedV = @"SELECT case when fv.Mobile_Created_date is null then fv.VDate else fv.Mobile_Created_date end Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                     0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,fv.latitude,fv.longitude,fv.[address]  FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN
                                     MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string discussion = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,tv.latitude,tv.longitude,tv.[address]   FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string tempdiscussion = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";


                    string DepoMeetHead = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount   ,tv.latitude,tv.longitude,tv.[address]  FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";

                    string tempDepoMeetHead = @"SELECT case when tv.Mobile_Created_date is null then tv.VDate else tv.Mobile_Created_date end Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,tv.latitude,tv.longitude,tv.[address]   FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and tv.VDate between '" + Settings.dateformat(Fromdate) + " 00:00' and '" + Settings.dateformat(Todate) + " 23:59' " + QryChk + " ";



                    if (type == "Order")
                    {
                        Query = " (" + QryOrder + " union all " + TempQryOrder +")";
                    }
                    else
                    {
                        Query = " (" + QryOrder + " union all " + TempQryOrder + " union all " + Demo + " union all " + tempDemo + " union all " + FailedV + " union all " + tempFailedV + "  union all " + Comp + " union all " + tempComp + "  union all " + Coll + " union all " + tempColl + " union all " + dcollection + " union all " + dFailedV + " union all " + temdFailedV + "  union all " + discussion + " union all " + tempdiscussion + " union all " + DepoMeetHead + " union all " + tempDepoMeetHead + ")";

                    }
                }

                DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                return JsonConvert.SerializeObject(dtItem);               
           
                

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

        //void fill_TreeArea()
        //{
        //    int lowestlvl = 0;
        //    DataTable St = new DataTable();
        //    if (roleType == "Admin")
        //    {
        //        St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
        //    }
        //    else
        //    {               
        //        St = DbConnectionDAL.GetDataTable(CommandType.Text, "SELECT mastrole.rolename,mastsalesrep.smid,smname + ' (' + mastsalesrep.syncid + ' - '+ mastrole.rolename + ')' AS smname, mastsalesrep.lvl,mastrole.roletype FROM   mastsalesrep LEFT JOIN mastrole ON mastrole.roleid = mastsalesrep.roleid WHERE  smid =" + Settings.Instance.SMID + "");
        //    }


        //    trview.Nodes.Clear();

        //    if (St.Rows.Count <= 0)
        //    {
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found !');", true);
        //        return;
        //    }
        //    foreach (DataRow row in St.Rows)
        //    {
        //        TreeNode tnParent = new TreeNode();
        //        tnParent.Text = row["SMName"].ToString();
        //        tnParent.Value = row["SMId"].ToString();
        //        trview.Nodes.Add(tnParent);              
        //        tnParent.CollapseAll();                
        //        getchilddata(tnParent, tnParent.Value);
        //    }
        //}
        //private void getchilddata(TreeNode parent, string ParentId)
        //{

        //    string SmidVar = string.Empty;
        //    string GetFirstChildData = string.Empty;
        //    int levelcnt = 0;
        //    if (Settings.Instance.RoleType == "Admin")
        //        //levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 2;
        //        levelcnt = Convert.ToInt16("0") + 2;
        //    else
        //        levelcnt = Convert.ToInt16(Settings.Instance.SalesPersonLevel) + 1;


        //    //GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " and msr.Active=1 order by SMName,lvl desc ";
        //    GetFirstChildData = "select msrg.smid,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp =" + ParentId + " and msr.lvl=" + (levelcnt) + " and msrg.smid <> " + ParentId + " Order by SMName,lvl desc ";
        //    DataTable FirstChildDataDt = DbConnectionDAL.GetDataTable(CommandType.Text, GetFirstChildData);

        //    if (FirstChildDataDt.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < FirstChildDataDt.Rows.Count; i++)
        //        {
        //            SmidVar += FirstChildDataDt.Rows[i]["smid"].ToString() + ",";
        //            FillChildArea(parent, ParentId, FirstChildDataDt.Rows[i]["smid"].ToString(), FirstChildDataDt.Rows[i]["smname"].ToString());
        //        }
        //        SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);

        //        for (int j = levelcnt + 1; j <= 6; j++)
        //        {
        //            //string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + "  and msr.Active=1 order by SMName,lvl desc ";
        //            string AreaQueryChild = "select msrg.smid,msr.lvl,maingrp,Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname from mastsalesrepgrp msrg left join mastsalesrep msr on msrg.smid=msr.smid  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msrg.maingrp  in (" + SmidVar + ") and msr.lvl=" + j + " Order by SMName,lvl desc ";
        //            DataTable dtChild = DbConnectionDAL.GetDataTable(CommandType.Text, AreaQueryChild);
                  
        //            int mTotRows = dtChild.Rows.Count;
        //            if (mTotRows > 0)
        //            {
        //                SmidVar = string.Empty;
        //                var str = "";
        //                for (int k = 0; k < mTotRows; k++)
        //                {
        //                    SmidVar += dtChild.Rows[k]["smid"].ToString() + ",";
        //                }

        //                TreeNode Oparent = parent;
        //                switch (j)
        //                {
        //                    case 3:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        break;
        //                    case 4:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }
        //                        break;
        //                    case 5:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                            {
        //                                foreach (TreeNode child in Pchild.ChildNodes)
        //                                {
        //                                    str += child.Value + ","; parent = child;
        //                                    DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                    for (int l = 0; l < dr.Length; l++)
        //                                    {
        //                                        FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                    }
        //                                    dtChild.Select();
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }


        //                        break;
        //                    case 6:
        //                        if (Oparent.Parent != null)
        //                        {
        //                            if (Settings.Instance.RoleType == "Admin")
        //                            {
        //                                foreach (TreeNode Pchild in Oparent.Parent.Parent.ChildNodes)
        //                                {
        //                                    foreach (TreeNode Qchild in Pchild.ChildNodes)
        //                                    {
        //                                        foreach (TreeNode child in Qchild.ChildNodes)
        //                                        {
        //                                            str += child.Value + ","; parent = child;
        //                                            DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                            for (int l = 0; l < dr.Length; l++)
        //                                            {
        //                                                FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                            }
        //                                            dtChild.Select();
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            else
        //                            {
        //                                foreach (TreeNode Pchild in Oparent.Parent.ChildNodes)
        //                                {
        //                                    foreach (TreeNode child in Pchild.ChildNodes)
        //                                    {
        //                                        str += child.Value + ","; parent = child;
        //                                        DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                        for (int l = 0; l < dr.Length; l++)
        //                                        {
        //                                            FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                        }
        //                                        dtChild.Select();
        //                                    }
        //                                }
        //                            }

        //                        }

        //                        else
        //                        {
        //                            foreach (TreeNode child in Oparent.ChildNodes)
        //                            {
        //                                str += child.Value + ","; parent = child;
        //                                DataRow[] dr = dtChild.Select("maingrp =" + child.Value);
        //                                for (int l = 0; l < dr.Length; l++)
        //                                {
        //                                    FillChildArea(child, child.Value, dr[l]["smid"].ToString(), dr[l]["smname"].ToString());
        //                                }
        //                                dtChild.Select();
        //                            }
        //                        }


        //                        break;
        //                }

        //                SmidVar = SmidVar.Substring(0, SmidVar.Length - 1);
        //            }
        //        }
        //    }
        //}
        //public void FillChildArea(TreeNode parent, string ParentId, string Smid, string SMName)
        //{
        //    TreeNode child = new TreeNode();
        //    child.Text = SMName;
        //    child.Value = Smid;
        //    child.SelectAction = TreeNodeSelectAction.Expand;
        //    parent.ChildNodes.Add(child);
        //    child.CollapseAll();
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDSRPartyWiseAnalysis.aspx", true);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            rptmain.Style.Add("display", "block");
            string smIDStr = "", smIDStr1 = "", PartyIdStr1="";
       
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');

          

            if (smIDStr1 != "")
            {              

                    if (Convert.ToDateTime(frmTextBox.Text) <= Convert.ToDateTime(toTextBox.Text))
                    {
                        GetDailyWorkingReport(smIDStr1, frmTextBox.Text, toTextBox.Text);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To date cannot be less than From Date.');", true);
                        rptmain.Style.Add("display", "none");
                    }                
            }
            else
            {
                rptmain.Style.Add("display", "none");
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "alert('Please select salesperson');", true);
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string partyids = "";          
            string a = "";
            string b = "";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=DSRPartywiseAnalysis.csv");
            string headertxt = "DSR Date".TrimStart('"').TrimEnd('"') + ", " + "Mobile Date".TrimStart('"').TrimEnd('"') + "," + "Emp Code".TrimStart('"').TrimEnd('"') + "," + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "Active".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "PartyId".TrimStart('"').TrimEnd('"') + "," + "Party".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Active".TrimStart('"').TrimEnd('"') + "," + "Stype".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + "," + "Product Segment".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"') + "," + "Item".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + "," + "NextVisitDate".TrimStart('"').TrimEnd('"') + "," + "NextVisitTime".TrimStart('"').TrimEnd('"') + "," + "Competitor Name".TrimStart('"').TrimEnd('"') + "," + "Brand Activity".TrimStart('"').TrimEnd('"') + "," + "Meet Activity".TrimStart('"').TrimEnd('"') + "," + "Other Activity".TrimStart('"').TrimEnd('"') + "," + "Other GeneralInfo".TrimStart('"').TrimEnd('"') + "," + "Road Show".TrimStart('"').TrimEnd('"') + "," + "Scheme".TrimStart('"').TrimEnd('"') + "," + "Discount".TrimStart('"').TrimEnd('"') + "," + "Remark".TrimStart('"').TrimEnd('"') + "," + "Latitude".TrimStart('"').TrimEnd('"') + "," + "Longitude".TrimStart('"').TrimEnd('"') + "," + "Geo Address".TrimStart('"').TrimEnd('"');

            StringBuilder sb = new StringBuilder();
            sb.Append(headertxt);
            sb.AppendLine(System.Environment.NewLine);  
            string dataText = string.Empty;
            DataTable udtNew = new DataTable();

            //dtParams.Columns.Add(new DataColumn("DSRDate", typeof(DateTime)));
            //dtParams.Columns.Add(new DataColumn("EmpCode", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("SalesPerson", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("City", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("PartyId", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Party", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("Stype", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("ProductClass", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("ProductSegment", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("ProductGroup", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Amount", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("Item", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Qty", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Rate", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("NextVisitDate", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("NextVisitTime", typeof(String)));

            //dtParams.Columns.Add(new DataColumn("CompetitorName", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("BrandActivity", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("MeetActivity", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("OtherActivity", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("OtherGeneralInfo", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("RoadShow", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Scheme", typeof(String)));
            //dtParams.Columns.Add(new DataColumn("Remark", typeof(String)));
          
            //foreach (RepeaterItem item in rpt.Items)
            //{
            //    DataRow dr = dtParams.NewRow();
            //    Label VisitDateLabel = item.FindControl("VisitDateLabel") as Label;
            //    dr["DSRDate"] = Convert.ToDateTime(VisitDateLabel.Text).ToShortDateString();
            //    Label SyncIdlevelLabel = item.FindControl("SyncIdlevelLabel") as Label;
            //    dr["EmpCode"] = SyncIdlevelLabel.Text.ToString();
            //    Label SmnameLabel = item.FindControl("SmnameLabel") as Label;
            //    dr["SalesPerson"] = SmnameLabel.Text.ToString();
            //    Label BeatLabel = item.FindControl("BeatLabel") as Label;
            //    dr["City"] = BeatLabel.Text.ToString();

            //    Label PartyIdLabel = item.FindControl("PartyIdLabel") as Label;
            //    dr["PartyId"] = PartyIdLabel.Text.ToString();
            //    Label PartyLabel = item.FindControl("PartyLabel") as Label;
            //    dr["Party"] = PartyLabel.Text.ToString();
            //    Label StypeLabel = item.FindControl("StypeLabel") as Label;
            //    dr["Stype"] = StypeLabel.Text.ToString();
            //    Label productClassLabel = item.FindControl("productClassLabel") as Label;
            //    dr["ProductClass"] = productClassLabel.Text.ToString();
            //    Label SegmentLabel = item.FindControl("SegmentLabel") as Label;
            //    dr["ProductSegment"] = SegmentLabel.Text.ToString();

            //    Label MaterialGroupLabel = item.FindControl("MaterialGroupLabel") as Label;
            //    dr["ProductGroup"] = MaterialGroupLabel.Text.ToString();
            //    Label ValueLabel = item.FindControl("ValueLabel") as Label;
            //    dr["Amount"] = ValueLabel.Text.ToString();
            //    Label CompItemLabel = item.FindControl("CompItemLabel") as Label;
            //    dr["Item"] = CompItemLabel.Text.ToString();
            //    Label CompQtyLabel = item.FindControl("CompQtyLabel") as Label;
            //    dr["Qty"] = CompQtyLabel.Text.ToString();

            //    Label ComRateLabel = item.FindControl("ComRateLabel") as Label;
            //    dr["Rate"] = ComRateLabel.Text.ToString();
            //    Label NextVisitDateLabel = item.FindControl("NextVisitDateLabel") as Label;
            //    dr["NextVisitDate"] = NextVisitDateLabel.Text.ToString();
            //    Label NextVisitTimeLabel = item.FindControl("NextVisitTimeLabel") as Label;
            //    dr["NextVisitTime"] = NextVisitTimeLabel.Text.ToString();
            //    Label CompNameLabel = item.FindControl("CompNameLabel") as Label;
            //    dr["CompetitorName"] = CompNameLabel.Text.ToString();

            //    Label BrandActivityLabel = item.FindControl("BrandActivityLabel") as Label;
            //    dr["BrandActivity"] = BrandActivityLabel.Text.ToString();
            //    Label MeetActivityLabel = item.FindControl("MeetActivityLabel") as Label;
            //    dr["MeetActivity"] = MeetActivityLabel.Text.ToString();
            //    Label OtherActivityLabel = item.FindControl("OtherActivityLabel") as Label;
            //    dr["OtherActivity"] = OtherActivityLabel.Text.ToString();
            //    Label OtherGeneralInfoLabel = item.FindControl("OtherGeneralInfoLabel") as Label;
            //    dr["OtherGeneralInfo"] = OtherGeneralInfoLabel.Text.ToString();
            //    Label RoadShowLabel = item.FindControl("RoadShowLabel") as Label;
            //    dr["RoadShow"] = RoadShowLabel.Text.ToString();
            //    Label RemarksLabel = item.FindControl("RemarksLabel") as Label;
            //    dr["Remark"] = RemarksLabel.Text.ToString();

            //    Label SchemeLabel = item.FindControl("SchemeLabel") as Label;
            //    dr["Scheme"] = SchemeLabel.Text.ToString();               
               
            //    dtParams.Rows.Add(dr); 
            //}
            //DataView dv = dtParams.DefaultView;
            //dv.Sort = "DSRDate desc";
            //DataTable udtNew = dv.ToTable();
            //decimal[] totalVal = new decimal[11];
            string smId = "", smIDStr = "";
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smId = node.Value;
                {
                    smIDStr += node.Value + ",";
                }
            }
            smId = smIDStr.TrimStart(',').TrimEnd(',');
            if (ddlCity.SelectedValue != "0" && ddlParty.SelectedValue == "")
            {             
                string partyQry = @"select partyid from mastparty WHERE Areaid IN (SELECT DISTINCT areaid  FROM ViewGeo WHERE cityid ='" + ddlCity.SelectedValue + "') AND partydist=0";
                DataTable dtMastParty = DbConnectionDAL.GetDataTable(CommandType.Text, partyQry);
                if (dtMastParty.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtMastParty.Rows.Count - 1; i++)
                    {
                        partyids = dtMastParty.Rows[i]["partyid"].ToString();
                        a += partyids + ",";
                    }
                }

                b = a.TrimStart(',').TrimEnd(',');

            }
            if (ddlParty.SelectedValue != "0" || ddlParty.SelectedValue !="")
            {
                string partyQry = @"select partyid from mastparty where partydist=0  and partyid='" + ddlParty.SelectedValue + "'";
                DataTable dtMastParty = DbConnectionDAL.GetDataTable(CommandType.Text, partyQry);
                if (dtMastParty.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtMastParty.Rows.Count - 1; i++)
                    {
                        partyids = dtMastParty.Rows[i]["partyid"].ToString();
                        a += partyids + ",";
                    }
                }

                b = a.TrimStart(',').TrimEnd(',');
            }           
            string FromDate = frmTextBox.Text;
            string ToDate = toTextBox.Text;

            string data = string.Empty, beat = "", Query = "", QryChk = "";
            DataTable dtLocTraRep = new DataTable();
            
                if (ddlDsrType.SelectedItem.Text == "Lock")
                { QryChk = "and vl1.lock =1"; }
                if (ddlDsrType.SelectedItem.Text == "UnLock")
                { QryChk = "and vl1.lock = 0"; }
                if (ddlDsrType.SelectedItem.Text == "All")
                { QryChk = "and vl1.lock  in (1,0)"; }

                if (ddlPType.SelectedItem.Text != "All")
                {
                    if (ddlPType.SelectedItem.Text == "Active")
                    { QryChk += "and p.Active =1"; }
                    else
                    { QryChk += "and p.Active =0"; }
                }
                if (ddlSType.SelectedItem.Text != "All")
                {
                    if (ddlSType.SelectedItem.Text == "Active")
                    { QryChk += "and sn.Active =1"; }
                    else
                    { QryChk += "and sn.Active =0"; }
                }


                if (ddlStatus.SelectedValue != "3")
                {
                    if (ddlStatus.SelectedItem.Text == "Pending")
                    { QryChk += " and vl1.AppStatus is null "; }
                    else
                    { QryChk += " and vl1.AppStatus='" + ddlStatus.SelectedValue + "'"; }
                }

                if (smId != "")
                {

//                    string QryOrder = @"SELECT os.Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
//                                  sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty,os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
//                                  MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,os.latitude,os.longitude,os.[address]  FROM TransOrder os LEFT JOIN TransOrder1 os1 ON os.OrdId=os1.OrdId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
//                                  Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.SMId in (" + smId + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " " +
//                                     " GROUP BY sn.active,p.Active,p.PartyName,mi.ItemName, os.VDate,os1.Qty,os1.Rate, sn.SMId, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string QryOrder = @"SELECT os.Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                  sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty,os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,os.latitude,os.longitude,os.[address]  FROM TransOrder os LEFT JOIN TransOrder1 os1 ON os.OrdDocId=os1.OrdDocId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                  Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.SMId in (" + smId + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " " +
                                      " GROUP BY sn.active,p.Active,p.PartyName,mi.ItemName, os.VDate,os1.Qty,os1.Rate, sn.SMId, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string TempQryOrder = @"SELECT os.Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                      sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty,os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,os.latitude,os.longitude,os.[address]  FROM Temp_TransOrder os LEFT JOIN Temp_TransOrder1 os1 ON os.OrdDocId=os1.OrdDocId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.SMId in (" + smId + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "" +
                                          " GROUP BY sn.active,p.Active,p.PartyName,mi.ItemName, os.VDate,os1.Qty,os1.Rate, sn.SMId, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

//                    string TempQryOrder = @"SELECT os.Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
//                                      sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty,os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
//                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,os.latitude,os.longitude,os.[address]  FROM Temp_TransOrder os LEFT JOIN Temp_TransOrder1 os1 ON os.OrdId=os1.OrdId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
//                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.SMId in (" + smId + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "" +
//                                        " GROUP BY sn.active,p.Active,p.PartyName,mi.ItemName, os.VDate,os1.Qty,os1.Rate, sn.SMId, os.PartyId, os.Remarks, p.PartyId,p.Address1, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock,os.Mobile_Created_date,os.latitude,os.longitude,os.[address]";

                    string Demo = @"SELECT DISTINCT d.Mobile_Created_date,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                               vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,d.latitude,d.longitude,d.[address]  FROM TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN 
                                MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                               TransVisit vl1 ON vl1.SMId = d .SMId and d .VDate = vl1.VDate where d.SMId in (" + smId + ") and d.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "";

                    string tempDemo = @"SELECT DISTINCT d.Mobile_Created_date,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                                 vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,d.latitude,d.longitude,d.[address]  FROM Temp_TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN
                                 MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                                 TransVisit vl1 ON vl1.SMId = d .SMId AND d .VDate = vl1.VDate where d.SMId in (" + smId + ") and d.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "";

                    string FailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                  fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                                  TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempFailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                     fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName,
                                     vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                     TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string Comp = @"SELECT tc.Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value,
                              tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.Discount ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + smId + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempComp = @"SELECT tc.Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value, 
                                  tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc. Discount ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + smId + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string dcollection = @"SELECT Dc.Mobile_Created_date,Dc.PaymentDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Collection' AS Stype, p.PartyId, p.partyname AS partyname,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, sum(CONVERT(numeric(18, 2), dc.Amount)) AS Value, max(dc.Remarks) AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, 
                                     '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,Dc.latitude,Dc.longitude,Dc.[address]   FROM DistributerCollection Dc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = Dc.SMId LEFT JOIN mastparty p ON Dc.DistId = p.PartyId LEFT JOIN 
                                      MastArea b ON b.AreaId = p.cityid LEFT JOIN TransVisit vl1 ON vl1.SMId = dc.SMId AND vl1.VDate = dc.PaymentDate where Dc.SMId in (" + smId + ") and Dc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,dc.PaymentDate, sn.SMId, p.PartyId, p.Partyname,p.Address1, b.AreaName, dc.Amount, vl1.AppStatus, vl1.Lock,Dc.Mobile_Created_date,Dc.latitude,Dc.longitude,Dc.[address]";

                    //collection add in details 07-07-2021----- anurag

                    string Coll = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date ,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount))  AS Value,
                              tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + smId + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    string tempColl = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount)) AS Value, 
                                  tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + smId + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    //complete 07-07-2021----- anurag

                    string dFailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                    0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,fv.latitude,fv.longitude,fv.[address]  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string temdFailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                     0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,fv.latitude,fv.longitude,fv.[address]  FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN
                                     MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string discussion = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,tv.latitude,tv.longitude,tv.[address]   FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + smId + ") and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempdiscussion = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + smId + ") and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string depomeethead = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + smId + ") and tv.[type] in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and  tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempdepomeethead = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + smId + ") and tv.[type] in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and  tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    Query = " (" + QryOrder + " union all " + TempQryOrder + " union all " + Demo + " union all " + tempDemo + " union all " + FailedV + " union all " + tempFailedV + "  union all " + Comp + " union all " + tempComp + "  union all " + Coll + " union all " + tempColl + " union all " + dcollection + " union all " + dFailedV + " union all " + temdFailedV + "  union all " + discussion + " union all " + tempdiscussion + " union all " + depomeethead + " union all " + tempdepomeethead + ") ORDER BY Mobile_Created_date";
                }
                else
                {
                    string QryOrder = @"SELECT os.Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                  sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty,os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,os.latitude,os.longitude,os.[address]  FROM TransOrder os LEFT JOIN TransOrder1 os1 ON os.OrdId=os1.OrdId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                  Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.partyid in (" + b + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " " +
                                    " GROUP BY sn.active,p.Active,p.PartyName,mi.ItemName, os.VDate,os1.Qty,os1.Rate, sn.SMId, os.PartyId, os.Remarks, p.PartyId, b.AreaName, p.Created_Date,p.Address1, vl1.AppStatus, vl1.Lock,os.Mobile_Created_date,os.latitude,os.longitude,os.[address] ";

                    string TempQryOrder = @"SELECT os.Mobile_Created_date,os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Order' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                      sum((os1.Qty * os1.Rate)) AS Value, os.Remarks, mi.ItemName AS CompItem, os1.Qty AS CompQty,os1.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,os.latitude,os.longitude,os.[address]  FROM Temp_TransOrder os LEFT JOIN TransOrder1 os1 ON os.OrdId=os1.OrdId LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate LEFT JOIN mastitem mi ON mi.ItemId=os1.ItemId where os.partyid in (" + b + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "" +
                                          " GROUP BY sn.active,p.Active,p.PartyName,mi.ItemName, os.VDate,os1.Qty,os1.Rate, sn.SMId, os.PartyId, os.Remarks, p.PartyId, b.AreaName, p.Created_Date,p.Address1, vl1.AppStatus, vl1.Lock,os.Mobile_Created_date,os.latitude,os.longitude,os.[address] ";

                    string Demo = @"SELECT DISTINCT d.Mobile_Created_date,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                               vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,d.latitude,d.longitude,d.[address]   FROM TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN 
                                MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                               TransVisit vl1 ON vl1.SMId = d .SMId and d .VDate = vl1.VDate where d.partyid in (" + b + ") and d.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "";

                    string tempDemo = @"SELECT DISTINCT d.Mobile_Created_date,d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                                 vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,d.latitude,d.longitude,d.[address]   FROM Temp_TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN
                                 MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                                 TransVisit vl1 ON vl1.SMId = d .SMId AND d .VDate = vl1.VDate where d.partyid in (" + b + ") and d.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "";

                    string FailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                  fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                                  TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempFailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                     fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName,
                                     vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]  FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                     TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string Comp = @"SELECT tc.Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value,
                              tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.Discount ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempComp = @"SELECT tc.Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value, 
                                  tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme,tc.Discount ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string dcollection = @"SELECT Dc.Mobile_Created_date,Dc.PaymentDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Collection' AS Stype, p.PartyId, p.partyname AS partyname,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, sum(CONVERT(numeric(18, 2), dc.Amount)) AS Value, max(dc.Remarks) AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, 
                                     '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,Dc.latitude,Dc.longitude,Dc.[address]   FROM DistributerCollection Dc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = Dc.SMId LEFT JOIN mastparty p ON Dc.DistId = p.PartyId LEFT JOIN 
                                      MastArea b ON b.AreaId = p.cityid LEFT JOIN TransVisit vl1 ON vl1.SMId = dc.SMId AND vl1.VDate = dc.PaymentDate where Dc.distid in (" + b + ") and Dc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,dc.PaymentDate, sn.SMId, p.PartyId, p.Partyname,p.Address1, b.AreaName, dc.Amount, vl1.AppStatus, vl1.Lock,Dc.Mobile_Created_date,Dc.latitude,Dc.longitude,Dc.[address] ";

                    //collection add in details 07-07-2021----- anurag

                    string Coll = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date ,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount)) AS Value,
                              tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                              FROM TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    string tempColl = @"SELECT case when tc.Mobile_Created_date is null then tc.VDate else tc.Mobile_Created_date end Mobile_Created_date,tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Collection' AS Stype, tc.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], sum(CONVERT(numeric(18, 2), tc.Amount)) AS Value, 
                                  tc.Remarks AS Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' OtherGeneralInfo,'' RoadShow,'' as Scheme,0 as Discount ,tc.latitude,tc.longitude,tc.[address] 
                                  FROM Temp_TransCollection tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.partyid in (" + b + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " GROUP BY sn.active,p.Active,tc.PaymentDate, p.Partyname,p.Address1, b.AreaName, tc.Amount, vl1.AppStatus, vl1.Lock,tc.Mobile_Created_date,tc.latitude,tc.longitude,tc.[address],tc.VDate,sn.SyncId, sn.SMName,sn.SMId, tc.PartyId,tc.Remarks,tc.Mobile_Created_date";

                    //complete 07-07-2021----- anurag

                    string dFailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                    0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]   FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string temdFailedV = @"SELECT fv.Mobile_Created_date,fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                     0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,fv.latitude,fv.longitude,fv.[address]   FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN
                                     MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.partyid in (" + b + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string discussion = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempdiscussion = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,tv.latitude,tv.longitude,tv.[address]   FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";
                    string DepoMeetHead = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount ,tv.latitude,tv.longitude,tv.[address]   FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    string tempDepoMeetHead = @"SELECT tv.Mobile_Created_date,tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname,case sn.Active when 1 then 'Yes' else 'No' end as SActive, tv.type AS Stype, p.PartyId, p.PartyName AS Party,p.Address1,case P.Active when 1 then 'Yes' else 'No' end as PActive, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme,0 as Discount  ,tv.latitude,tv.longitude,tv.[address]  FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.distid in (" + b + ") and tv.type in ('Depo','Head Office','Meet (Dealer/Counter/Umbrella)') and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                    if (ddltype.SelectedValue == "Order")
                    {
                        Query = " (" + QryOrder + " union all " + TempQryOrder + " )";
                    }
                    else
                    {
                        Query = " (" + QryOrder + " union all " + TempQryOrder + " union all " + Demo + " union all " + tempDemo + " union all " + FailedV + " union all " + tempFailedV + "  union all " + Comp + " union all " + tempComp + "  union all " + Coll + " union all " + tempColl + " union all " + dcollection + " union all " + dFailedV + " union all " + temdFailedV + "  union all " + discussion + " union all " + tempdiscussion + " union all " + DepoMeetHead + " union all " + tempDepoMeetHead + ")";
                    }
                }

                udtNew = DbConnectionDAL.GetDataTable(CommandType.Text, Query);


            //    "DSR Date".TrimStart('"').TrimEnd('"') + "," + "Emp Code".TrimStart('"').TrimEnd('"') + ",
            //" + "Sales Person".TrimStart('"').TrimEnd('"') + "," + "City".TrimStart('"').TrimEnd('"') + "," + "PartyId".TrimStart('"').TrimEnd('"') + ",
            //" + "Party".TrimStart('"').TrimEnd('"') + "," + "Stype".TrimStart('"').TrimEnd('"') + "," + "Product Class".TrimStart('"').TrimEnd('"') + ",
            //" + "Product Segment".TrimStart('"').TrimEnd('"') + "," + "Product Group".TrimStart('"').TrimEnd('"') + "," + "Amount".TrimStart('"').TrimEnd('"') + ",
            //" + "Item".TrimStart('"').TrimEnd('"') + "," + "Qty".TrimStart('"').TrimEnd('"') + "," + "Rate".TrimStart('"').TrimEnd('"') + ",
            //" + "NextVisitDate".TrimStart('"').TrimEnd('"') + "," + "NextVisitTime".TrimStart('"').TrimEnd('"') + "," + "Competitor Name".TrimStart('"').TrimEnd('"') + 
            //"," + "Brand Activity".TrimStart('"').TrimEnd('"') + "," + "Meet Activity".TrimStart('"').TrimEnd('"') + "," + "Other Activity".TrimStart('"').TrimEnd('"')
            //    + "," + "Other GeneralInfo".TrimStart('"').TrimEnd('"') + "," + "Road Show".TrimStart('"').TrimEnd('"') + "," + "Scheme".TrimStart('"').TrimEnd('"') +
            //    "," + "Remark".TrimStart('"').TrimEnd('"');

            //    udtNew.Columns["VisitDate"].SetOrdinal(0);
            //    udtNew.Columns["Syncid"].SetOrdinal(1);
            //    udtNew.Columns["Smname"].SetOrdinal(2);
            //    udtNew.Columns["SActive"].SetOrdinal(3);
            //    udtNew.Columns["Beat"].SetOrdinal(4);
            // udtNew.Columns["PartyId"].SetOrdinal(5);
            // udtNew.Columns["Party"].SetOrdinal(6);
            // udtNew.Columns["PActive"].SetOrdinal(7);
            //    udtNew.Columns["Stype"].SetOrdinal(8);
            //    udtNew.Columns["productClass"].SetOrdinal(9);
            //    udtNew.Columns["Segment"].SetOrdinal(10);
            //    udtNew.Columns["MaterialGroup"].SetOrdinal(11);
            //    udtNew.Columns["Value"].SetOrdinal(12);
            // udtNew.Columns["CompItem"].SetOrdinal(13);
            //  udtNew.Columns["CompQty"].SetOrdinal(14);
            //   udtNew.Columns["ComRate"].SetOrdinal(15);
            // udtNew.Columns["NextVisitDate"].SetOrdinal(16);
            // udtNew.Columns["NextVisitTime"].SetOrdinal(17);
            //   udtNew.Columns["Compname"].SetOrdinal(18);
            // udtNew.Columns["Brandactivity"].SetOrdinal(19);
            //udtNew.Columns["MeetActivity"].SetOrdinal(20);
            //udtNew.Columns["OtherActivity"].SetOrdinal(21); 
            //udtNew.Columns["OtherGeneralInfo"].SetOrdinal(22);
            //    udtNew.Columns["RoadShow"].SetOrdinal(23);
            //    udtNew.Columns["Scheme"].SetOrdinal(24);
            //    udtNew.Columns["Discount"].SetOrdinal(25); 
            //    udtNew.Columns["Remarks"].SetOrdinal(26);
            //    udtNew.Columns.Remove("image");
            //    udtNew.Columns.Remove("Smid");
            //    udtNew.Columns.Remove("AppStatus");
            //    udtNew.Columns.Remove("lock");
            //    udtNew.AcceptChanges();

                udtNew.Columns["VisitDate"].SetOrdinal(0);
                udtNew.Columns["Mobile_Created_date"].SetOrdinal(1);
                udtNew.Columns["Syncid"].SetOrdinal(2);
                udtNew.Columns["Smname"].SetOrdinal(3);
                udtNew.Columns["SActive"].SetOrdinal(4);
                udtNew.Columns["Beat"].SetOrdinal(5);
                udtNew.Columns["PartyId"].SetOrdinal(6);
                udtNew.Columns["Party"].SetOrdinal(7);
                udtNew.Columns["Address1"].SetOrdinal(8);
                udtNew.Columns["PActive"].SetOrdinal(9);
                udtNew.Columns["Stype"].SetOrdinal(10);
                udtNew.Columns["productClass"].SetOrdinal(11);
                udtNew.Columns["Segment"].SetOrdinal(12);
                udtNew.Columns["MaterialGroup"].SetOrdinal(13);
                udtNew.Columns["Value"].SetOrdinal(14);
                udtNew.Columns["CompItem"].SetOrdinal(15);
                udtNew.Columns["CompQty"].SetOrdinal(16);
                udtNew.Columns["ComRate"].SetOrdinal(17);
                udtNew.Columns["NextVisitDate"].SetOrdinal(18);
                udtNew.Columns["NextVisitTime"].SetOrdinal(19);
                udtNew.Columns["Compname"].SetOrdinal(20);
                udtNew.Columns["Brandactivity"].SetOrdinal(21);
                udtNew.Columns["MeetActivity"].SetOrdinal(22);
                udtNew.Columns["OtherActivity"].SetOrdinal(23);
                udtNew.Columns["OtherGeneralInfo"].SetOrdinal(24);
                udtNew.Columns["RoadShow"].SetOrdinal(25);
                udtNew.Columns["Scheme"].SetOrdinal(26);
                udtNew.Columns["Discount"].SetOrdinal(27);
                udtNew.Columns["Remarks"].SetOrdinal(28);
                udtNew.Columns["Latitude"].SetOrdinal(29);
                udtNew.Columns["Longitude"].SetOrdinal(30);
                udtNew.Columns["Address"].SetOrdinal(31);
                udtNew.Columns.Remove("image");
                udtNew.Columns.Remove("Smid");
                udtNew.Columns.Remove("AppStatus");
                udtNew.Columns.Remove("lock");
                udtNew.AcceptChanges();

                for (int j = 0; j < udtNew.Rows.Count; j++)
            {
                for (int k = 0; k < udtNew.Columns.Count; k++)
                {
                    if (udtNew.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            string h = udtNew.Rows[j][k].ToString();                            
                            string d = h.Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                            udtNew.Rows[j][k] = "";
                            udtNew.Rows[j][k] = d;
                            udtNew.AcceptChanges();
                            sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                            //Total For Columns
                            if (k == 10)
                            {
                                if (udtNew.Rows[j][k].ToString() != "")
                                {
                                    //totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                }
                            }
                            //End
                        }
                    }
                    else if (udtNew.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(udtNew.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            string h = udtNew.Rows[j][k].ToString();
                            string d = h.Replace("\n", "").Replace("\r", "").Replace(',', ' ');
                            udtNew.Rows[j][k] = "";
                            udtNew.Rows[j][k] = d;
                            udtNew.AcceptChanges();    
                            sb.Append(String.Format("\"{0}\"", udtNew.Rows[j][k].ToString()) + ',');
                            //Total For Columns
                            if (k == 10)
                            {
                                if (udtNew.Rows[j][k].ToString() != "")
                                {
                                    //totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
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
                            if (k == 10)
                            {
                                if (udtNew.Rows[j][k].ToString() != "")
                                {
                                    //totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                }
                            }
                            //End
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }
            //string totalStr = string.Empty;
            //for (int x = 10; x < totalVal.Count(); x++)
            //{
            //    if (totalVal[x] == 0)
            //    {
            //        totalStr += "0" + ',';
            //    }
            //    else
            //    {
            //        totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
            //    }
            //}
            //sb.Append(",,,,,,,,,Total," + totalStr);
            Response.ContentEncoding = System.Text.Encoding.Default;    
            Response.Write(sb.ToString());          
            Response.End();
           
       
        }

        public void GetDailyWorkingReport(string smId, string FromDate, string ToDate)
        {
            string data = string.Empty, beat = "", Query = "", QryChk = "";
            DataTable dtLocTraRep = new DataTable();
            try
            {                     
                if (ddlDsrType.SelectedItem.Text == "Lock")
                { QryChk = "and vl1.lock =1"; }
                if (ddlDsrType.SelectedItem.Text == "UnLock")
                { QryChk = "and vl1.lock = 0"; }
                if (ddlDsrType.SelectedItem.Text == "All")
                { QryChk = "and vl1.lock  in (1,0)"; }


                if (ddlStatus.SelectedValue != "3")
                {
                    if (ddlStatus.SelectedItem.Text == "Pending")
                    { QryChk += " and vl1.AppStatus is null "; }
                    else
                    { QryChk += " and vl1.AppStatus='" + ddlStatus.SelectedValue + "'"; }
                }

                //Query = @" Select * from [View_DSR_PartywiseAnalysis] where [View_DSR_PartywiseAnalysis].Visitdate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' and  [View_DSR_PartywiseAnalysis].Smid in (" + smId + ") " + QryChk + " order by View_DSR_PartywiseAnalysis.Visitdate ";
                string QryOrder = @"SELECT os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname, 'Order' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                  sum(CONVERT(numeric(18, 2), os.OrderAmount)) AS Value, os.Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme FROM TransOrder os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                  Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate where os.SMId in (" + smId + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " " +
                                  " GROUP BY p.PartyName, os.VDate, sn.SMId, os.PartyId, os.Remarks, p.PartyId, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock";

                string TempQryOrder = @"SELECT os.VDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname, 'Order' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                      sum(CONVERT(numeric(18, 2), os.OrderAmount)) AS Value, os.Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, '' AS Image, '' AS CompName, 
                                      MAX(vl1.AppStatus) AS AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM Temp_TransOrder os LEFT OUTER JOIN dbo.mastsalesrep AS sn ON sn.SMId = os.SMId LEFT JOIN
                                      Mastparty p ON os.PartyId = p.PartyId LEFT JOIN  MastArea b ON  p.CityId = b.AreaId  LEFT JOIN TransVisit vl1 ON os.SMId = vl1.SMId AND os.VDate = vl1.VDate where os.SMId in (" + smId + ") and os.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "" +
                                      " GROUP BY p.PartyName, os.VDate, sn.SMId, os.PartyId, os.Remarks, p.PartyId, b.AreaName, p.Created_Date, vl1.AppStatus, vl1.Lock";

                string Demo = @"SELECT DISTINCT d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                               vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN 
                                MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                               TransVisit vl1 ON vl1.SMId = d .SMId and d .VDate = vl1.VDate where d.SMId in (" + smId + ") and d.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "";

                string tempDemo = @"SELECT DISTINCT d .VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Demo' AS Stype, d .PartyId, p.PartyName AS Party, ic.name AS productClass, ms.name AS Segment, i.ItemName AS [MaterialGroup], 0 AS Value, d .Remarks, '' AS CompItem, 0 AS compQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, d .ImgUrl AS Image, '' AS CompName, 
                                 vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM Temp_TransDemo d LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = d .SMId LEFT JOIN MastItemClass ic ON d .ProductClassId = ic.Id LEFT JOIN  mastitemsegment ms ON d .ProductSegmentId = ms.Id LEFT JOIN
                                 MastItem i ON i.ItemId = d .ProductMatGrp LEFT JOIN MastParty p ON p.PartyId = d .PartyId LEFT JOIN  MastArea b ON b.AreaId = p.CityId LEFT JOIN TransCompetitor c ON c.PartyId = d .PartyId AND c.VDate = d .VDate LEFT JOIN
                                 TransVisit vl1 ON vl1.SMId = d .SMId AND d .VDate = vl1.VDate where d.SMId in (" + smId + ") and d.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + "";

                string FailedV = @"SELECT fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                  fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName, 
                                  vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                                  TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                string tempFailedV = @"SELECT fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'party Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 0 AS value,
                                     fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image, '' AS CompName,
                                     vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                     TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate LEFT JOIN MastSalesRep cp ON cp.SMId = vl1.smid and fv.VDate = vl1.VDate WHERE p.partydist = 0 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                string Comp = @"SELECT tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value,
                              tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme
                              FROM TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN 
                              TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + smId + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                string tempComp = @"SELECT tc.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Competitor' AS Stype, tc.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS [MaterialGroup], 0 AS Value, 
                                  tc.Remarks AS Remarks, tc.item AS CompItem, tc.Qty AS compQty, tc.Rate AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, '' AS NextVisitTime, tc.ImgUrl AS Image, tc.CompName AS CompName, vl1.AppStatus, vl1.Lock,tc.BrandActivity,tc.MeetActivity,tc.OtherActivity,tc.OtherGeneralInfo,tc.RoadShow,tc.[Scheme/offers] as Scheme
                                  FROM Temp_TransCompetitor tc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tc.SMId LEFT JOIN MastParty p ON p.PartyId = tc.PartyId LEFT JOIN MastArea b ON b.AreaId = p.CityId LEFT JOIN
                                  TransVisit vl1 ON vl1.SMId = tc.SMId AND tc.VDate = vl1.VDate where tc.SMId in (" + smId + ") and tc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                string dcollection = @"SELECT Dc.PaymentDate AS VisitDate, sn.SMId, MAX(sn.SyncId) AS SyncId, MAX(sn.SMName) AS Smname, 'Distributor Collection' AS Stype, p.PartyId, p.partyname AS partyname, '' AS productClass, '' AS Segment, '' AS MaterialGroup, sum(CONVERT(numeric(18, 2), dc.Amount)) AS Value, max(dc.Remarks) AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, '' AS NextVisitDate, 
                                     '' AS NextVisitTime, '' AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM DistributerCollection Dc LEFT JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = Dc.SMId LEFT JOIN mastparty p ON Dc.DistId = p.PartyId LEFT JOIN 
                                      MastArea b ON b.AreaId = p.cityid LEFT JOIN TransVisit vl1 ON vl1.SMId = dc.SMId AND vl1.VDate = dc.PaymentDate where Dc.SMId in (" + smId + ") and Dc.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " GROUP BY dc.PaymentDate, sn.SMId, p.PartyId, p.Partyname, b.AreaName, dc.Amount, vl1.AppStatus, vl1.Lock";

                string dFailedV = @"SELECT fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                    0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                string temdFailedV = @"SELECT fv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Distributor Non-Productive' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                     0 AS value, fv.Remarks AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, fv.Nextvisit, 106) AS NextVisitDate, fv.VisitTime AS NextVisitTime, '' AS Image,
                                    '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM Temp_TransFailedVisit fv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = fv.SMId LEFT JOIN MastParty p ON p.PartyId = fv.PartyId LEFT JOIN
                                     MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = fv.SMId AND fv.VDate = vl1.VDate WHERE p.partydist = 1 and fv.SMId in (" + smId + ") and fv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                string discussion = @"SELECT tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup, 
                                    0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                    tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN 
                                    MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + smId + ") and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                string tempdiscussion = @"SELECT tv.VDate AS VisitDate, sn.SMId, sn.SyncId AS SyncId, sn.SMName AS Smname, 'Distributor Discussion' AS Stype, p.PartyId, p.PartyName AS Party, '' AS productClass, '' AS Segment, '' AS MaterialGroup,
                                        0 AS value, tv.remarkDist AS Remarks, '' AS CompItem, 0 AS CompQty, 0 AS ComRate, b.AreaName AS Beat, CONVERT(varchar, tv.NextVisitDate, 106) AS NextVisitDate, tv.NextVisitTime AS NextVisitTime,
                                        tv.ImgUrl AS Image, '' AS CompName, vl1.AppStatus, vl1.Lock,'' as BrandActivity,'' as MeetActivity,'' as OtherActivity,'' as OtherGeneralInfo,'' as RoadShow,'' as Scheme  FROM Temp_TransVisitDist tv left JOIN dbo.View_SalesRepRole AS sn ON sn.SMId = tv.SMId LEFT JOIN MastParty p ON p.PartyId = tv.DistId LEFT JOIN
                                        MastArea b ON b.AreaId = p.CityId LEFT JOIN TransVisit vl1 ON vl1.SMId = tv.SMId AND tv.VDate = vl1.VDate where tv.SMId in (" + smId + ") and tv.VDate between '" + Settings.dateformat(FromDate) + " 00:00' and '" + Settings.dateformat(ToDate) + " 23:59' " + QryChk + " ";

                Query = " (" + QryOrder + " union all " + TempQryOrder + " union all " + Demo + " union all " + tempDemo + " union all " + FailedV + " union all " + tempFailedV + "  union all " + Comp + " union all " + tempComp + " union all " + dcollection + " union all " + dFailedV + " union all " + temdFailedV + "  union all " + discussion + " union all " + tempdiscussion + ")";

                dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                        if (dtLocTraRep.Rows.Count > 0)
                        {
                            rpt.DataSource = dtLocTraRep;
                            rpt.DataBind();
                            btnExport.Visible = true;
                        }
                        else
                        {
                            rpt.DataSource = null;
                            rpt.DataBind();
                        }                    
                } 
          
            catch (Exception)
            {

            }
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

        protected void trview_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {

            string smIDStr = "", smIDStr12 = "";

            {
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr12 = node.Value;
                    {
                        smIDStr += node.Value + ",";
                    }
                }
                smIDStr12 = smIDStr.TrimStart(',').TrimEnd(',');
                Session["treenodes"] = smIDStr12;                

            }

        }

        protected void rblview_SelectedIndexChanged(object sender, EventArgs e)
        {
          if(rblview.SelectedValue == "Party")
          {
              Partyview.Visible = true;
              divtrview.Visible = false;
          }
          else
          {
              Partyview.Visible = false;
              divtrview.Visible = true;
          }
         
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string cityQry = @"select distinct cityid,cityname from viewgeo where stateid=" + ddlState.SelectedValue + " and CityAct=1 order by Cityname";
                DataTable dtMastCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
                if (dtMastCity.Rows.Count > 0)
                {
                    ddlParty.Items.Clear();
                    ddlCity.DataSource = dtMastCity;
                    ddlCity.DataTextField = "cityname";
                    ddlCity.DataValueField = "cityid";
                    ddlCity.DataBind();
                }
                ddlCity.Items.Insert(0, new ListItem("--Please select--", "0"));

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string partyQry = @"select partyid,Partyname from mastparty where partydist=0 and areaid in (select distinct areaid from viewgeo where cityid='" + ddlCity.SelectedValue + "' ) Order by partyname";
                DataTable dtMastParty = DbConnectionDAL.GetDataTable(CommandType.Text, partyQry);
                if (dtMastParty.Rows.Count > 0)
                {
                    ddlParty.DataSource = dtMastParty;
                    ddlParty.DataTextField = "Partyname";
                    ddlParty.DataValueField = "partyid";
                    ddlParty.DataBind();
                }
                //ddlParty.Items.Insert(0, new ListItem("--Please select--", "0"));

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
    }
}