using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
//using DataAccessLayer;
using BusinessLayer;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Device.Location;
using DAL;
using BAL;
using AstralFFMS.ServiceReferenceDMTracker;
/// <summary>
/// Summary description for WebService2
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebService2 : System.Web.Services.WebService {

    public WebService2 () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    static string Glong = "", Glat = "";
    #region Person Location Data
    public class Location
    {
        public Location()
        {
        }
        public string Time
        { get; set; }
        public string Lat
        { get; set; }
        public string Lng
        { get; set; }
    }

    [WebMethod]
    public string GetRouteByPerson(string DeviceNo)
    {

        DataTable dtroutes=new DataTable();
        List<Location> list = new List<Location>();
        string qry = "select Time=CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6))" +
                     " from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo " +
                     "  WHERE PersonMaster.DeviceNo='" + DeviceNo.Trim() + "' and LocationDetails.Log_m='G' and " +
                    "  cast(LocationDetails.CurrentDate as Date) = cast( ('" + DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy") + "') As Date ) ";
        dtroutes = DbConnectionDAL.GetDataTable(CommandType.Text, qry);// DataAccessLayer.DAL.getFromDataTable(qry);
        if (dtroutes.Rows.Count < 1)
        {  
            dtroutes=new DataTable();
            string qry1 = "select Time=CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6))" +
                        " from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo " +
                        "  WHERE PersonMaster.DeviceNo='" + DeviceNo.Trim() + "' and LocationDetails.Log_m='G' and " +
                       "  cast(LocationDetails.CurrentDate as Date) = cast( ('" + DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy") + "') As Date ) ";
            dtroutes = DbConnectionDAL.GetDataTable(CommandType.Text, qry1);// DataAccessLayer.DAL.getFromDataTable(qry1);
        }
        for (int i = 0; i < dtroutes.Rows.Count; i++)
        {
            Location ob = new Location();
            ob.Time = dtroutes.Rows[i]["Time"].ToString();
            ob.Lat = dtroutes.Rows[i]["lat"].ToString();
            ob.Lng = dtroutes.Rows[i]["lng"].ToString();
            list.Add(ob);
        }

        System.Web.Script.Serialization.JavaScriptSerializer obj = new System.Web.Script.Serialization.JavaScriptSerializer();
        return obj.Serialize(list);
    }
    #endregion

    #region GetSaveAddress
     [WebMethod]
    public string GetAddressFence(string hdnlatlng)
    {
        string addr = "";
        try
        {
            string mlatLong = "";
            //-2 for spaces in between of brackets
           
            mlatLong = (hdnlatlng.Substring(1, hdnlatlng.Length - 2));
            string[] latlong = mlatLong.Split(',');
            if (latlong.Length > 0)
            {
                latlong[1] = latlong[1].ToString().Substring(1, latlong[1].Length - 2);
                if (latlong[0].ToString().Length > 8)
                    Glat = latlong[0].ToString().Trim().Substring(0, 7);
                else
                    Glat = latlong[0].ToString().Trim();
                if (latlong[1].ToString().Length > 8)
                    Glong = latlong[1].ToString().Substring(0, 7).TrimStart();
                else
                    Glong = latlong[1].ToString().Trim();
               WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
               addr = DMT.InsertAddress(Glat, Glong);
                
            }
         
        }
        catch (Exception ex) { ex.ToString(); }
        return addr;
    }
     [WebMethod]
     public string SaveFenceAddress(string Fradius,string Faddr,string FgroupId)
     {
         try
         {
             Fradius = Math.Round((Convert.ToDecimal(Fradius)), 3).ToString();
             string sql = "insert into FenceAddress(CLat,CLong,Radius,Address,GroupId)values('" + Glat + "','" + Glong + "'," + Fradius + ",'" + Faddr.Trim() + "','" + FgroupId + "')";
            DbConnectionDAL.ExecuteQuery(sql);
         }
         catch (Exception ex) { ex.ToString(); }
         return "Upadated Successfully";
     }
    #endregion

    #region TimeDistanceLogRpt
      [WebMethod]
     public string GetData(string persons, string loc, string accuracy, string fromdate, string todate, string fromtime, string totime)
     {   DataTable dt1 = new DataTable();
         try
         {
             string strper = string.Empty;
             string str = string.Empty;
             GetAddress ga = new GetAddress();
             string addDeviceNo = persons;
         
             //foreach (ListItem item in ddlperson.Items)
             //{
             //    if (ddlperson.SelectedIndex < 0)
             //    {
             //        addDeviceNo += item.Value + "," + "";
             //    }
             //    else
             //    {
             //        if (item.Selected)
             //        {
             //            addDeviceNo += item.Value + "," + "";
             //        }
             //    }
             //}
             //if (string.IsNullOrEmpty(addDeviceNo))
             //{
             //    ShowAlert("Please Select atleast one Person");
             //    gvData.DataSource = null; gvData.DataBind();
             //    return;
             //}
         //    addDeviceNo = addDeviceNo.Substring(0, addDeviceNo.Length - 1);
             if (loc != "0")
             {
                 if (loc == "C")
                     str = str + " and Log_m in ('C','N')";
                 else str = str + " and Log_m='" + loc  + "'";
             }
             if (!string.IsNullOrEmpty(accuracy))
             {
                 str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + accuracy;
             }

             string str1 = "select * from (select CONVERT(VARCHAR(8), CurrentDate, 108) as Time,convert(varchar(18),currentdate,106) as Cdate,currentdate as dateC,Battery,Gps_accuracy as Accuracy,PersonMaster.PersonName as Person,personmaster.empcode,Locationdetails.Log_m as Signal,description AS address,locationdetails.latitude,locationdetails.longitude,LocationDetails.HomeFlag,'0' as TimeDiff,'0' as distance,'0' as speed,  'LD' as flag from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.DeviceNo in (" + addDeviceNo + ") and cast(locationdetails.CurrentDate as datetime) BETWEEN CAST('" + fromdate + " " + fromtime + "' AS datetime) AND  CAST('" + todate + " " + totime + "' AS datetime) " + str +
             "   UNION " +
   " SELECT     CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate,log_tracker.currentdate AS dateC," +
       " '' as battery,'' as Accuracy,personmaster.personname AS Person,personmaster.empcode,'' AS Signal, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem'" + " WHEN 'TN' THEN ' Tower not in reach'  WHEN 'AO' THEN ' Application/Mobile Off' END AS  address,'' as latitude,'' as longitude,'' as homeflag,'0' AS TimeDiff,'0' AS distance,'0' " + " AS speed,  'LT' as flag FROM       log_tracker RIGHT JOIN personmaster ON   log_tracker.deviceno=personmaster.deviceno WHERE log_tracker.status not in ('DR') and  log_tracker.deviceno in (" + addDeviceNo + ") " + " and cast(log_tracker.CurrentDate as datetime) BETWEEN CAST('" + fromdate + " " + fromtime + "' AS datetime) AND CAST('" + todate + " " + totime + "' AS datetime) " +
             " ) atbl order by atbl.Person,CAST(atbl.datec AS datetime) asc";

             dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);// DataAccessLayer.DAL.getFromDataTable(str1);
             string mLong = "";
             string mLat = "";
             string mAdd = "";
             string prevlat = "";
             string prevlong = "";

             if (dt1.Rows.Count > 0)
             {
                 double distdiff = 0, timediff = 0;
            //     btnExport.Visible = true;
                 for (int i = 0; i < dt1.Rows.Count; i++)
                 {
                     if (!string.IsNullOrEmpty(dt1.Rows[i]["latitude"].ToString()))
                         prevlat = dt1.Rows[i]["latitude"].ToString();
                     if (!string.IsNullOrEmpty(dt1.Rows[i]["longitude"].ToString()))
                         prevlong = dt1.Rows[i]["longitude"].ToString();
                     if (i > 0)
                     {
                         try
                         {
                             if (Convert.ToDateTime(dt1.Rows[i - 1]["Cdate"]) != Convert.ToDateTime(dt1.Rows[i]["Cdate"]) || (dt1.Rows[i - 1]["Person"].ToString()) != (dt1.Rows[i]["Person"]).ToString())
                             {
                                 dt1.Rows[i]["TimeDiff"] = 0; dt1.Rows[i]["distance"] = 0; prevlat = ""; prevlong = "";
                             }
                             else
                             {
                                 timediff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(dt1.Rows[i]["dateC"])));
                                 if (timediff > 5)
                                 {
                                     //if (dt1.Rows[i]["flag"] == "LD")
                                     //{
                                     DataRow dr1 = dt1.NewRow();
                                     dr1["Person"] = dt1.Rows[i]["Person"].ToString();
                                     dr1["empcode"] = dt1.Rows[i]["empcode"].ToString();
                                     dr1["address"] = "No Network/Data";
                                     dr1["Cdate"] = dt1.Rows[i]["Cdate"].ToString();
                                     dr1["dateC"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(5);
                                     dr1["Time"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(5).ToString("HH:mm:ss");
                                     dr1["TimeDiff"] = "5";
                                     dr1["distance"] = dt1.Rows[i - 1]["distance"].ToString();
                                     dr1["speed"] = dt1.Rows[i - 1]["speed"].ToString();
                                     dr1["Battery"] = dt1.Rows[i - 1]["Battery"].ToString();
                                     dr1["Signal"] = dt1.Rows[i - 1]["Signal"].ToString();
                                     dr1["Accuracy"] = dt1.Rows[i - 1]["Accuracy"].ToString();
                                     dr1["HomeFlag"] = dt1.Rows[i - 1]["HomeFlag"].ToString();
                                     dr1["latitude"] = dt1.Rows[i - 1]["latitude"].ToString();
                                     dr1["longitude"] = dt1.Rows[i - 1]["longitude"].ToString();
                                     // }

                                     dt1.Rows.InsertAt(dr1, i);
                                 }
                                 else
                                 {
                                     if (dt1.Rows[i]["flag"].Equals("LD"))
                                     {
                                         Double dist = 0;
                                         if (!string.IsNullOrEmpty(prevlat) && !string.IsNullOrEmpty(prevlong))
                                         {
                                             if (!string.IsNullOrEmpty(dt1.Rows[i - 1]["longitude"].ToString()) && !string.IsNullOrEmpty(dt1.Rows[i - 1]["longitude"].ToString()))
                                             {
                                                 dist = Calculate(Convert.ToDouble(dt1.Rows[i - 1]["latitude"]), Convert.ToDouble(dt1.Rows[i - 1]["longitude"]), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"]));
                                             }
                                             else
                                             {
                                                 dist = Calculate(Convert.ToDouble(prevlat), Convert.ToDouble(prevlong), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"]));
                                             }
                                             distdiff = (Math.Truncate((dist / 1000) * 100) / 100);
                                             dt1.Rows[i]["speed"] = Math.Round((distdiff / (timediff)), 2).ToString();
                                             dt1.Rows[i]["TimeDiff"] = timediff.ToString();
                                             dt1.Rows[i]["distance"] = distdiff.ToString();
                                         }
                                     }
                                     else
                                     {
                                         dt1.Rows[i]["TimeDiff"] = timediff.ToString();
                                     }
                                 }

                                 #region OldCode

                                 //if (dt1.Rows[i]["address"].ToString().Trim() == "GPS Off" || dt1.Rows[i]["address"].ToString().Trim() == "Internet Problem" || dt1.Rows[i]["address"].ToString().Trim() == "Tower not in reach")
                                 //{
                                 //    Double dist = Calculate(Convert.ToDouble(dt1.Rows[i - 1]["latitude"]), Convert.ToDouble(dt1.Rows[i - 1]["longitude"]), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"]));
                                 //    distdiff = (Math.Truncate((dist / 1000) * 100) / 100);
                                 //    dt1.Rows[i]["speed"] = Math.Round((distdiff / (timediff)), 2).ToString();
                                 //    dt1.Rows[i]["TimeDiff"] = timediff.ToString();
                                 //    dt1.Rows[i]["distance"] = distdiff.ToString();
                                 //}
                                 //else
                                 //{
                                 //    if (timediff > 5)
                                 //    {
                                 //        DataRow dr1 = dt1.NewRow();
                                 //        dr1["Person"] = dt1.Rows[i]["Person"].ToString();
                                 //        dr1["empcode"] = dt1.Rows[i]["empcode"].ToString();
                                 //        dr1["address"] = "No Network";
                                 //        dr1["Cdate"] = dt1.Rows[i]["Cdate"].ToString();
                                 //        dr1["dateC"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(5);
                                 //        dr1["Time"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(5).ToString("HH:mm:ss");
                                 //        dr1["TimeDiff"] = "5";
                                 //        dr1["distance"] = dt1.Rows[i - 1]["distance"].ToString();
                                 //        dr1["speed"] = dt1.Rows[i - 1]["speed"].ToString();
                                 //        dr1["Battery"] = dt1.Rows[i - 1]["Battery"].ToString();
                                 //        dr1["Signal"] = dt1.Rows[i - 1]["Signal"].ToString();
                                 //        dr1["Accuracy"] = dt1.Rows[i - 1]["Accuracy"].ToString();
                                 //        dr1["HomeFlag"] = dt1.Rows[i - 1]["HomeFlag"].ToString();
                                 //        dr1["latitude"] = dt1.Rows[i - 1]["latitude"].ToString();
                                 //        dr1["longitude"] = dt1.Rows[i - 1]["longitude"].ToString();

                                 //        dt1.Rows.InsertAt(dr1, i);
                                 //    }
                                 //    else
                                 //    {
                                 //        Double dist = Calculate(Convert.ToDouble(dt1.Rows[i - 1]["latitude"]), Convert.ToDouble(dt1.Rows[i - 1]["longitude"]), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"]));
                                 //        distdiff = (Math.Truncate((dist / 1000) * 100) / 100);
                                 //        dt1.Rows[i]["speed"] = Math.Round((distdiff / (timediff)), 2).ToString();
                                 //        dt1.Rows[i]["TimeDiff"] = timediff.ToString();
                                 //        dt1.Rows[i]["distance"] = distdiff.ToString();

                                 //  }
                                 //}
                                 #endregion
                             }
                         }
                         catch (Exception ex) { ex.ToString(); }
                     }


                     string addr = dt1.Rows[i]["address"].ToString();
                     if (addr == "")
                     {
                         string Glat = "", Glong = "";
                         if (dt1.Rows[i]["longitude"].ToString().Length > 8)
                             Glong = dt1.Rows[i]["longitude"].ToString().Substring(0, 7);
                         else
                             Glong = dt1.Rows[i]["longitude"].ToString();
                         if (dt1.Rows[i]["latitude"].ToString().Length > 8)
                             Glat = dt1.Rows[i]["latitude"].ToString().Substring(0, 7);
                         else
                             Glat = dt1.Rows[i]["latitude"].ToString();

                         if (mLat != Glat || mLong != Glong)
                         {
                             mLat = Glat;
                             mLong = Glong;
                             WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                             mAdd = DMT.FetchAddress(Glat.ToString(), Glong.ToString());
                             if (string.IsNullOrEmpty(mAdd))
                                 mAdd = DMT.InsertAddress(Glat, Glong);
                         }
                         addr = mAdd;
                         dt1.Rows[i]["address"] = addr;
                     }


                     dt1.AcceptChanges();
                 }
                 
                 //gvData.DataSource = dt1;
                 //gvData.DataBind();
             }
             else
             {
                 //ShowAlert("No Record Found !!"); gvData.DataSource = null; gvData.DataBind();
                 //btnExport.Visible = false;
             }
         }
         catch (Exception ex)
         {
            
         }
         return JsonConvert.SerializeObject(dt1);
        
     }
     public static double TimeCalc(DateTime d1, DateTime d2)
     {
         double timediff = 0;
         return timediff = d2.Subtract(d1).TotalMinutes;
     }
     public static double Calculate(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
     {
         var sCoord = new GeoCoordinate(sLatitude, sLongitude);
         var eCoord = new GeoCoordinate(eLatitude, eLongitude);
         return sCoord.GetDistanceTo(eCoord);
     }
    #endregion




}
