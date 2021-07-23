using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
//using DataAccessLayer;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using BusinessLayer;
using System.Data.SqlClient;
using DAL;
using BAL;
using AstralFFMS.ServiceReferenceDMTracker;
using System.Net;
using System.IO;
/// <summary>
/// Summary description for Jwebservice
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Jwebservice : System.Web.Services.WebService {
    SMSAdapter sms = new SMSAdapter();
    Common cm = new Common();
    public Jwebservice()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    public static string getConStr
    {
        get
        {
            string constr = ConfigurationManager.AppSettings["constr"].ToString();
            return constr;
        }
    }
    public static string getConStrDmLicense
    {
        get
        {
            string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";

            return constrDmLicense;
        }
    }
    #region UseVar

    [DataContract]
    public class Result
    {
        [DataMember]
        public string ResultMsg { get; set; }

    }
    [DataContract]
    public class messagemodal
    {
        [DataMember]
        public string message { get; set; }

    }

    [DataContract]
    public class getotpforweb
    {
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public string Otp { get; set; }
        [DataMember]
        public string ValidTill { get; set; }

    }
    [DataContract]
    public class Url
    {
        [DataMember]
        public string UrlMsg { get; set; }

    }
    public class UrlNew
    {
        [DataMember]
        public string UrlMsgnew { get; set; }
        [DataMember]
        public string ShowFence { get; set; }
    }

    [DataContract]
    public class Exist
    {
        [DataMember]
        public string ExistMsg { get; set; }

    }
    public class Persons
    {

        [DataMember]
        public List<Person> PersonList { get; set; }
    }

    [DataContract]
    public class Person
    {
        [DataMember]
        public string Pid { get; set; }
        [DataMember]
        public string PersonName { get; set; }
        [DataMember]
        public string FromTime { get; set; }
        [DataMember]
        public string ToTime { get; set; }
        [DataMember]
        public string Interval { get; set; }
        [DataMember]
        public string UploadInterval { get; set; }
        [DataMember]
        public string RetryInterval { get; set; }
        [DataMember]
        public string Degree { get; set; }
        [DataMember]
        public string Sys_Flag { get; set; }
        [DataMember]
        public string GpsLoc { get; set; }
        [DataMember]
        public string MobileLoc { get; set; }
        [DataMember]
        public string SrMobile { get; set; }
        [DataMember]
        public string SrSMS { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string MarkAttendanceTxt { get; set; }
        [DataMember]
        public string LeaveModule { get; set; }
        [DataMember]
        public string support_number { get; set; }
        [DataMember]
        public string StartEndImg { get; set; }        

    }
    public class PersonAlarmSMS
    {
        [DataMember]
        public List<PersonAlarmS> PersonAlarmSMSList { get; set; }
    }

    [DataContract]
    public class PersonAlarmS
    {
        [DataMember]
        public string Alarm { get; set; }
        [DataMember]
        public string AlarmDurationMins { get; set; }
        [DataMember]
        public string SendSMS { get; set; }
    }


    public class Lcation_N
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string CT { get; set; }

    }
    public class Lcation_N_V6_0
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string CT { get; set; }
    }
    public class Lcation_D
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string CT { get; set; }
    }
    public class Lcation_D_V6_0
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string CT { get; set; }

    }
    public class LcationAttendance
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string LType { get; set; }
        public string CT { get; set; }



    }
    public class MobileLog
    {
        public string DeviceNo { get; set; }
        public string CurrentDate { get; set; }
        public string Status { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }

    }
    public class TrackerLog
    {
        public string DeviceNo { get; set; }
        public string CurrentDate { get; set; }
        public string Status { get; set; }

    }
    public class Fence
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string PNm { get; set; }

    }

    #endregion

    #region JServices

    private void CheckFenceAlert(string Lat, string Long, string PersonID, string Fdate)
    {
        bool Flag = false;
        try
        {
            string strgrp = "select GroupId from GrpMapp where PersonId='" + PersonID + "'";
            DataTable dtgrp = new DataTable();
            dtgrp =DbConnectionDAL.GetDataTable(CommandType.Text,strgrp);
            for (int i = 0; i < dtgrp.Rows.Count; i++)
            {
                DataTable dtFa = new DataTable();
                string Faqry = "select * from FenceAddressMsg where GroupId='" + dtgrp.Rows[i]["GroupId"] + "' and PersonId ='" + PersonID + "' and Flag='0'";
                dtFa = DbConnectionDAL.GetDataTable(CommandType.Text, Faqry);
                for (int p = 0; p < dtFa.Rows.Count; p++)
                {
                    double Dist = Calculate(Convert.ToDouble(dtFa.Rows[p]["Clat"]), Convert.ToDouble(dtFa.Rows[p]["Clong"]), Convert.ToDouble(Lat), Convert.ToDouble(Long));
                    if (Dist >= Convert.ToDouble(dtFa.Rows[p]["radius"]))
                        if (dtFa.Rows[p]["Flag"].ToString() == "0")
                        {
                            string mbl = DbConnectionDAL.GetStringScalarVal("select mobile from GroupMaster where groupid='" + dtFa.Rows[p]["groupid"] + "'");
                            if (!string.IsNullOrEmpty(mbl))
                            {
                                string person = DbConnectionDAL.GetStringScalarVal("select personname+'('+deviceno+')' from personmaster where id='" + PersonID + "'");
                                sms.sendSms(mbl, "Dear Sir,This is to notify you that " + person + " has left " + dtFa.Rows[p]["address"].ToString() + " at " + Fdate + ". Regards, Dataman Computer Systems");
                                DbConnectionDAL.ExecuteQuery("update FenceAddressMsg set Flag='1' where id=" + dtFa.Rows[p]["id"] + "");
                                //Flag = true;
                            }
                            break;
                        }
                }
                //if (Flag)
                //    break;
            }
        }
        catch (Exception x) { x.ToString(); }

    }
    private bool GetValidCountry(string clat, string clong)
    {
        bool retval = true;
        string strcountries = "select count(*) from BlackListCountries where Clat='" + clat + "' AND Clong='" + clong + "'";
        if (DbConnectionDAL.GetIntScalarVal(strcountries) > 0)
        {
            retval = false;
        }
        return retval;
    }
    public static double Calculate(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
    {
        var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
        var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
        var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
        var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

        var dLongitude = eLongitudeRadians - sLongitudeRadians;
        var dLatitude = eLatitudeRadians - sLatitudeRadians;

        var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                      Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                      Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

        // Using 3956 as the number of miles around the earth
        var result2 = 3956.0 * 2.0 *
                      Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

        return result2;
    }
    private string GetFenceAddress(string Lat, string Long, string PersonID)
    {
        string FenceAddr = "";
        try
        {
            string strgrp = "select GroupId from GrpMapp where PersonId='" + PersonID + "'";
            DataTable dtgrp = new DataTable();
            dtgrp = DbConnectionDAL.GetDataTable(CommandType.Text, strgrp);
            for (int i = 0; i < dtgrp.Rows.Count; i++)
            {
                DataTable dtFa = new DataTable();
                string Faqry = "select Clat,Clong,Radius,Address from FenceAddress where GroupId='" + dtgrp.Rows[i]["GroupId"] + "' and CLat LIKE '" + Lat.Substring(0, 5) + "%' and CLong like '" + Long.Substring(0, 5) + "%'";
                dtFa = DbConnectionDAL.GetDataTable(CommandType.Text, Faqry);
                for (int p = 0; p < dtFa.Rows.Count; p++)
                {
                    double Dist = Calculate(Convert.ToDouble(dtFa.Rows[p]["Clat"]), Convert.ToDouble(dtFa.Rows[p]["Clong"]), Convert.ToDouble(Lat), Convert.ToDouble(Long));
                    if (Dist <= Convert.ToDouble(dtFa.Rows[p]["radius"]) && Dist >= -1)
                        FenceAddr = dtFa.Rows[p]["address"].ToString();
                    if (!string.IsNullOrEmpty(FenceAddr))
                        break;
                }
                if (!string.IsNullOrEmpty(FenceAddr))
                    break;
            }
        }
        catch (Exception ex) { ex.ToString(); }
        return FenceAddr;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsD(string Pi, string Lt, string Lg, Int64 CD, string Bt, string Ga)
    {
        int retVal = 0;
        try
        {
            string HomeFlag = "N";
            decimal argLat = Convert.ToDecimal(Lt);
            decimal argLng = Convert.ToDecimal(Lg);
            int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
            int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
            //if (cntLat > 4 && cntLng > 4)
            //{
            string getdevid = "select DeviceNo from PersonMaster where ID=" + Pi + "";
            string DeviceNo =DbConnectionDAL.GetStringScalarVal(getdevid);
            if (!string.IsNullOrEmpty(DeviceNo))
            {
                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CD + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);

                if (GetValidCountry(Lt, Lg))
                {
                    if (CurrDate >= mDate)
                    {
                        string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",G" + "," + Bt + "," +
                            Ga + "" + Environment.NewLine;
                        string approx = "0";
                        if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                        { approx = Ga; }
                        DataTable dtla = new DataTable();
                        dtla = DbConnectionDAL.GetDataTable(CommandType.Text, "select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        if (dtla.Rows.Count > 0)
                        {
                            string LActiveDate = dtla.Rows[0]["Currentdate"].ToString();
                            Common cm = new Common();
                            // 29/07/2017  Nishu
                            //if (cm.GetValidLocationTicks(dtla.Rows[0]["lat"].ToString(), dtla.Rows[0]["long"].ToString(), Lt, Lg, Convert.ToDateTime(dtla.Rows[0]["currentdate"]), mDate, "G") == true)
                            //{
                            if (Convert.ToDateTime(cdate) > Convert.ToDateTime(LActiveDate))
                            {
                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                            }


                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Lt, Lg);
                            if (!string.IsNullOrEmpty(FAddress))
                            { Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }
                            //if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            }

                            //}
                        }

                        else
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Lt, Lg);
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            //if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal =DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                        //if (DeviceNo == "911531150035766")
                        //{
                        using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                        {
                            file2.WriteLine(createText);
                            file2.Close();
                        }
                        //}
                    }

                }
                else
                {
                    string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                }
            }
            //}
            //else
            //{
            //    retVal = 0;
            //}
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        string msg = "N";
        if (retVal == 0)
            msg = "Y";
        Result rst = new Result
        {
            ResultMsg = msg

        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsD_New(string DeviceNo, string Lt, string Lg, Int64 CD, string Bt, string Ga)
    {
        int retVal = 0;
        try
        {
            string HomeFlag = "N";
            decimal argLat = Convert.ToDecimal(Lt);
            decimal argLng = Convert.ToDecimal(Lg);
            int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
            int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
            //if (cntLat > 4 && cntLng > 4)
            //{
            //string getdevid = "select DeviceNo from PersonMaster where ID=" + Pi + "";
            //string DeviceNo = DbConnectionDAL.GetStringScalarVal(getdevid);
            string Pi = DbConnectionDAL.GetStringScalarVal("select Id from Personmaster where deviceno='" + DeviceNo + "'");
            if (!string.IsNullOrEmpty(DeviceNo))
            {
                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CD + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                if (GetValidCountry(Lt, Lg))
                {
                    if (CurrDate >= mDate)
                    {
                        string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",G" + "," + Bt + "," +
                            Ga + "" + Environment.NewLine;
                        string approx = "0";
                        if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                        { approx = Ga; }
                        DataTable dtla = new DataTable();
                        dtla = DbConnectionDAL.GetDataTable(CommandType.Text, "select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        if (dtla.Rows.Count > 0)
                        {
                            string LActiveDate = dtla.Rows[0]["Currentdate"].ToString();
                            Common cm = new Common();
                            // 29/07/2017  Nishu
                            //if (cm.GetValidLocationTicks(dtla.Rows[0]["lat"].ToString(), dtla.Rows[0]["long"].ToString(), Lt, Lg, Convert.ToDateTime(dtla.Rows[0]["currentdate"]), mDate, "G") == true)
                            //{
                            if (Convert.ToDateTime(cdate) > Convert.ToDateTime(LActiveDate))
                            {
                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                            }
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Lt, Lg);
                            if (!string.IsNullOrEmpty(FAddress))
                            { Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }
                            //if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal =DbConnectionDAL.GetIntScalarVal(Query);
                            }

                            //}
                        }

                        else
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                            retVal =DbConnectionDAL.GetIntScalarVal(insqry);
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Lt, Lg);
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            //if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal =DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                        //if (DeviceNo == "911531150035766")
                        //{
                        using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                        {
                            file2.WriteLine(createText);
                            file2.Close();
                        }
                        //}
                    }

                }
                else
                {
                    string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                }
            }
            //}
            //else
            //{
            //    retVal = 0;
            //}
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        string msg = "N";
        if (retVal == 0)
            msg = "Y";
        Result rst = new Result
        {
            ResultMsg = msg

        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsLocationData(string Location_D, string Location_N, string MobileLog, string TrackerLog, string Fence)
    {
        int retVal = 0;
        try
        {
            if (Location_D != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_D>>(Location_D);

                    for (int i = 0; i < objResponse.Count; i++)
                    {

                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);

                        string HomeFlag = "N";
                        decimal argLat = Convert.ToDecimal(Lt);
                        decimal argLng = Convert.ToDecimal(Lg);
                        int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                        int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
                        string Pi =DbConnectionDAL.GetStringScalarVal("select Id from PersonMaster where deviceno='" + DeviceNo + "'");
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CD + 19800);
                            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                            DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                            if (GetValidCountry(Lt, Lg))
                            {
                                if (CurrDate >= mDate)
                                {
                                    string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",G" + "," + Bt + "," +
                                        Ga + "" + Environment.NewLine;
                                    string approx = "0";
                                    if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                                    { approx = Ga; }
                                    DataTable dtla = new DataTable();
                                    dtla = DbConnectionDAL.GetDataTable(CommandType.Text, "select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                    if (dtla.Rows.Count > 0)
                                    {
                                        Common cm = new Common();
                                        DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        Address = DMT.InsertAddress(Lt, Lg);
                                        if (!string.IsNullOrEmpty(FAddress))
                                        { Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }

                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal =DbConnectionDAL.GetIntScalarVal(Query);
                                        }

                                    }
                                    else
                                    {
                                        string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                                        retVal =DbConnectionDAL.GetIntScalarVal(insqry);
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        Address = DMT.InsertAddress(Lt, Lg);
                                        if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }

                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal =DbConnectionDAL.GetIntScalarVal(Query);
                                        }
                                    }

                                    using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                                    {
                                        file2.WriteLine(createText);
                                        file2.Close();
                                    }

                                }

                            }
                            else
                            {
                                string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                                retVal =DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                    }
                }
            }

            if (Location_N != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_N>>(Location_N);

                    for (int i = 0; i < objResponse.Count; i++)
                    {

                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);

                        string HomeFlag = "N";
                        decimal argLat = Convert.ToDecimal(Lt);
                        decimal argLng = Convert.ToDecimal(Lg);
                        int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                        int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];


                        string Pi =DbConnectionDAL.GetStringScalarVal("select Id from PersonMaster where deviceno='" + DeviceNo + "'");
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CD + 19800);
                            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                            DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                            if (GetValidCountry(Lt, Lg))
                            {
                                if (CurrDate >= mDate)
                                {
                                    string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",N" + "," + Bt + "," +
                                        Ga + "" + Environment.NewLine;
                                    string approx = "0";
                                    if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                                    { approx = Ga; }
                                    DataTable dtla = new DataTable();
                                    dtla = DbConnectionDAL.GetDataTable(CommandType.Text, "select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                    if (dtla.Rows.Count > 0)
                                    {
                                        Common cm = new Common();
                                        DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        Address = DMT.InsertAddress(Lt, Lg);
                                        if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                                        //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                                        //if (string.IsNullOrEmpty(Address))
                                        //{
                                        //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        //    Address = DMT.InsertAddress(Lt, Lg);
                                        //}
                                        //  if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal =DbConnectionDAL.GetIntScalarVal(Query);
                                        }

                                        //}
                                    }

                                    else
                                    {
                                        string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag ) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                                        retVal =DbConnectionDAL.GetIntScalarVal(insqry);
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                                        //                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        //   Address = DMT.InsertAddress(Lt, Lg);
                                        //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                                        //if (string.IsNullOrEmpty(Address))
                                        //{
                                        //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        //    Address = DMT.InsertAddress(Lt, Lg);
                                        //}
                                        if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                        }
                                    }

                                    using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                                    {
                                        file2.WriteLine(createText);
                                        file2.Close();
                                    }

                                }
                                else
                                {
                                    string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                }
                            }
                        }
                    }
                }
            }

            if (MobileLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<MobileLog>>(MobileLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string retInfo = "Record Not Inserted";
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0, FromTime = 0, ToTime = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        FromTime = Convert.ToInt32(objResponse[i].FromTime);
                        ToTime = Convert.ToInt32(objResponse[i].ToTime);
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
                        try
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CurrentDate + 19800);
                            DateTime FDate = DateTime.Parse("1970-01-01");
                            FDate = FDate.AddSeconds(FromTime + 19800);
                            DateTime TDate = DateTime.Parse("1970-01-01");
                            TDate = TDate.AddSeconds(ToTime + 19800);
                            if (Status == "M")
                            { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                            else
                            { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }

                            // string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                            retVal =DbConnectionDAL.GetIntScalarVal(Query);
                        }
                        catch (Exception ex)
                        { ex.ToString(); }
                    }
                }
            }

            if (TrackerLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<TrackerLog>>(TrackerLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string retInfo = "Record Not Inserted";
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
                        try
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CurrentDate + 19800);
                            if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from Log_Tracker where CurrentDate='{0}' AND Status='{1}'", mDate, Status)) <= 0)
                            {
                                if (Status == "M")
                                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                                else
                                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                                //Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";
                                retVal =DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                        catch (Exception ex)
                        { ex.ToString(); }
                    }
                }

            }

            if (Fence != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Fence>>(Fence);

                    for (int j = 0; j < objResponse.Count; j++)
                    {
                        string retInfo = "Record Not Inserted";
                        string DeviceNo = "", latitude = "", longitude = "", PartyName = "";


                        DeviceNo = objResponse[j].DeviceNo.ToString();
                        latitude = objResponse[j].Lt.ToString();
                        longitude = objResponse[j].Lg.ToString();
                        PartyName = objResponse[j].PNm.ToString();
                        try
                        {
                            string PersonID =DbConnectionDAL.GetStringScalarVal("select Id from Person master where deviceno='" + DeviceNo + "'");
                            DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text, "select GroupId from GrpMapp where personid=" + PersonID + "");
                            for (int i = 0; i < dtgrps.Rows.Count; i++)
                            {
                                string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                                    "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                                retVal =DbConnectionDAL.GetIntScalarVal(str);
                            }
                        }
                        catch (Exception ex)
                        { ex.ToString(); }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        string msg = "N";
        if (retVal == 0)
            msg = "Y";
        Result rst = new Result
        {
            ResultMsg = msg

        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsLocationData_New(string Location_D, string Location_N, string MobileLog, string TrackerLog, string Fence)
    {
        int retVal = 0;
        try
        {
            if (Location_D != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_D>>(Location_D);

                    for (int i = 0; i < objResponse.Count; i++)
                    {

                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0; Int64 CT = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);


                        string HomeFlag = "N";
                        decimal argLat = Convert.ToDecimal(Lt);
                        decimal argLng = Convert.ToDecimal(Lg);
                        int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                        int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
                        string Pi =DbConnectionDAL.GetStringScalarVal("select Id from PersonMaster where deviceno='" + DeviceNo + "'");
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CD + 19800);
                            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                            DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                            if (GetValidCountry(Lt, Lg))
                            {
                                if (CurrDate >= mDate)
                                {
                                    string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",G" + "," + Bt + "," +
                                        Ga + "" + Environment.NewLine;
                                    string approx = "0";
                                    if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                                    { approx = Ga; }
                                    DataTable dtla = new DataTable();
                                    dtla = DbConnectionDAL.GetDataTable(CommandType.Text, "select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                    if (dtla.Rows.Count > 0)
                                    {
                                        Common cm = new Common();
                                        DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        Address = DMT.InsertAddress(Lt, Lg);
                                        if (!string.IsNullOrEmpty(FAddress))
                                        { Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }

                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                        }

                                    }
                                    else
                                    {
                                        string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                                        retVal =DbConnectionDAL.GetIntScalarVal(insqry);
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        Address = DMT.InsertAddress(Lt, Lg);
                                        if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }

                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal =DbConnectionDAL.GetIntScalarVal(Query);
                                        }
                                    }

                                    using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                                    {
                                        file2.WriteLine(createText);
                                        file2.Close();
                                    }

                                }

                            }
                            else
                            {
                                string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                    }
                }
            }

            if (Location_N != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_N>>(Location_N);

                    for (int i = 0; i < objResponse.Count; i++)
                    {

                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);

                        string HomeFlag = "N";
                        decimal argLat = Convert.ToDecimal(Lt);
                        decimal argLng = Convert.ToDecimal(Lg);
                        int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                        int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];


                        string Pi = DbConnectionDAL.GetStringScalarVal("select Id from PersonMaster where deviceno='" + DeviceNo + "'");
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CD + 19800);
                            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                            DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                            if (GetValidCountry(Lt, Lg))
                            {
                                if (CurrDate >= mDate)
                                {
                                    string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",N" + "," + Bt + "," +
                                        Ga + "" + Environment.NewLine;
                                    string approx = "0";
                                    if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                                    { approx = Ga; }
                                    DataTable dtla = new DataTable();
                                    dtla = DbConnectionDAL.GetDataTable(CommandType.Text, "select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                    if (dtla.Rows.Count > 0)
                                    {
                                        Common cm = new Common();
                                        DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        Address = DMT.InsertAddress(Lt, Lg);
                                        if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                                        //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                                        //if (string.IsNullOrEmpty(Address))
                                        //{
                                        //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        //    Address = DMT.InsertAddress(Lt, Lg);
                                        //}
                                        //  if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal =DbConnectionDAL.GetIntScalarVal(Query);
                                        }

                                        //}
                                    }

                                    else
                                    {
                                        string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag ) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                                        retVal =DbConnectionDAL.GetIntScalarVal(insqry);
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                                        //   AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        //   Address = DMT.InsertAddress(Lt, Lg);
                                        //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                                        //if (string.IsNullOrEmpty(Address))
                                        //{
                                        //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        //    Address = DMT.InsertAddress(Lt, Lg);
                                        //}
                                        if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                        }
                                    }

                                    using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                                    {
                                        file2.WriteLine(createText);
                                        file2.Close();
                                    }

                                }
                                else
                                {
                                    string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                }
                            }
                        }
                    }
                }
            }

            if (MobileLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<MobileLog>>(MobileLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string retInfo = "Record Not Inserted";
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0, FromTime = 0, ToTime = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        FromTime = Convert.ToInt32(objResponse[i].FromTime);
                        ToTime = Convert.ToInt32(objResponse[i].ToTime);
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
                        try
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CurrentDate + 19800);
                            DateTime FDate = DateTime.Parse("1970-01-01");
                            FDate = FDate.AddSeconds(FromTime + 19800);
                            DateTime TDate = DateTime.Parse("1970-01-01");
                            TDate = TDate.AddSeconds(ToTime + 19800);
                            if (Status == "M")
                            { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                            else
                            { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }

                            // string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                            retVal =DbConnectionDAL.GetIntScalarVal(Query);
                        }
                        catch (Exception ex)
                        { ex.ToString(); }
                    }
                }
            }

            if (TrackerLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<TrackerLog>>(TrackerLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string retInfo = "Record Not Inserted";
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
                        try
                        {
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CurrentDate + 19800);
                            if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from Log_Tracker where CurrentDate='{0}' AND Status='{1}'", mDate, Status)) <= 0)
                            {
                                if (Status == "M")
                                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                                else
                                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                                //Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";
                                retVal =DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                        catch (Exception ex)
                        { ex.ToString(); }
                    }
                }

            }

            if (Fence != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Fence>>(Fence);

                    for (int j = 0; j < objResponse.Count; j++)
                    {
                        string retInfo = "Record Not Inserted";
                        string DeviceNo = "", latitude = "", longitude = "", PartyName = "";


                        DeviceNo = objResponse[j].DeviceNo.ToString();
                        latitude = objResponse[j].Lt.ToString();
                        longitude = objResponse[j].Lg.ToString();
                        PartyName = objResponse[j].PNm.ToString();
                        try
                        {
                            string PersonID = DbConnectionDAL.GetStringScalarVal("select Id from Person master where deviceno='" + DeviceNo + "'");
                            DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text, "select GroupId from GrpMapp where personid=" + PersonID + "");
                            for (int i = 0; i < dtgrps.Rows.Count; i++)
                            {
                                string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                                    "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(str);
                            }
                        }
                        catch (Exception ex)
                        { ex.ToString(); }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

        //Result rst = new Result
        //{
        //    ResultMsg = msg

        //};
        //Context.Response.Write(JsonConvert.SerializeObject(rst));

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLocationDetails(string Location_D, string Location_N, string MobileLog, string TrackerLog, string Fence)
    {
        int retVal = 0;
        try
        {
            if (Location_D != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_D>>(Location_D);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0; Int64 CT = 0; string cdate1 = "";
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        CD = Convert.ToInt64(objResponse[i].CD);
                        if (!string.IsNullOrEmpty(objResponse[i].CD))
                        {
                            CT = Convert.ToInt64(objResponse[i].CT);
                            DateTime mDate1 = DateTime.Parse("1970-01-01");
                            mDate1 = mDate1.AddSeconds(CT + 19800);
                            cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            cdate1 = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        // to chk how data either inserted to temp  start





                        string Query = string.Empty;
                        string _chkduplicate = string.Empty;
                        //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
                        //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                        //if (_chkduplicate == "0")
                        {
                            Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description)" +
                                                         "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..')";
                            retVal = DbConnectionDAL.GetIntScalarVal(Query);

                            DataTable dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                            if (dtla.Rows.Count > 0)
                            {
                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                            }
                            else
                            {
                                string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                            }
                        }
                        // End chking
                        //string Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy)" +
                        //   "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + CD + "','G','" + Bt + "','" + Ga + "')";
                        //retVal = DbConnectionDAL.GetIntScalarVal(Query);                        
                    }
                }
            }

            if (Location_N != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_N>>(Location_N);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0; Int64 CT = 0; string cdate1 = "";
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);


                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        CD = Convert.ToInt64(objResponse[i].CD);
                        if (!string.IsNullOrEmpty(objResponse[i].CD))
                        {
                            CT = Convert.ToInt64(objResponse[i].CT);
                            DateTime mDate1 = DateTime.Parse("1970-01-01");
                            mDate1 = mDate1.AddSeconds(CT + 19800);
                            cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            cdate1 = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        // to chk how data either inserted to temp  start
                        string Query = string.Empty;
                        string _chkduplicate = string.Empty;
                        //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
                        //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                        //if (_chkduplicate == "0")
                        {
                            Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description)" +
                                                         "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','N','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..')";
                            retVal = DbConnectionDAL.GetIntScalarVal(Query);

                            DataTable dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                            if (dtla.Rows.Count > 0)
                            {
                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                            }
                            else
                            {
                                string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                            }
                        }
                        // End chking

                        //string Query = @"insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy)" +
                        //               "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + CD + "','N','" + Bt + "','" + Ga + "')";
                        //retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    }
                }
            }

            if (MobileLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<MobileLog>>(MobileLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0, FromTime = 0, ToTime = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        FromTime = Convert.ToInt32(objResponse[i].FromTime);
                        ToTime = Convert.ToInt32(objResponse[i].ToTime);
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);

                        Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime) values " +
                                " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "','" + FromTime + "','" + ToTime + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    }
                }
            }

            if (TrackerLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<TrackerLog>>(TrackerLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
                        Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status) values " +
                               " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    }
                }
            }

            if (Fence != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Fence>>(Fence);

                    for (int j = 0; j < objResponse.Count; j++)
                    {
                        string DeviceNo = "", latitude = "",
                               longitude = "", PartyName = "";

                        DeviceNo = objResponse[j].DeviceNo.ToString();
                        latitude = objResponse[j].Lt.ToString();
                        longitude = objResponse[j].Lg.ToString();
                        PartyName = objResponse[j].PNm.ToString();

                        string str = "insert into Temp_FenceAddress (DeviceNo,Latitude,Longitude,PartyName,Created_date) values" +
                                         "('" + DeviceNo + "','" + latitude + "','" + longitude + "','" + PartyName + "','" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(str);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLocationDetails_V6_0(string Location_D, string Location_N, string MobileLog, string TrackerLog, string Fence)
    {
        int retVal = 0;
        try
        {
            if (Location_D != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_D_V6_0>>(Location_D);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0; Int64 CT = 0;// cd location time    and ct current time
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt64(objResponse[i].CD);
                        CT = Convert.ToInt64(objResponse[i].CT);

                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                        DateTime mDate1 = DateTime.Parse("1970-01-01");
                        mDate1 = mDate1.AddSeconds(CT + 19800);
                        string cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));
                        //string Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate)" +
                        //                              "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + mDate + "','G','" + Bt + "','" + Ga + "','" + mDate1 + "')";
                        string Query = string.Empty;
                        string _chkduplicate = string.Empty;
                        //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
                        //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                        //if (_chkduplicate == "0")
                        {
                            Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description)" +
                                                         "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..')";
                            retVal = DbConnectionDAL.GetIntScalarVal(Query);

                            //DataTable dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                            //if (dtla.Rows.Count > 0)
                            //{
                            //    DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                            //}
                            //else
                            //{
                            //    string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                            //    retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                            //}

                            DataTable dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                            if (!string.IsNullOrEmpty(Lt))
                            {
                                if (dtla.Rows.Count > 0)
                                {
                                    DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                }
                                else
                                {
                                    string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                    retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                                }
                            }
                            else
                            {
                                if (dtla.Rows.Count > 0)
                                {
                                    DbConnectionDAL.GetStringScalarVal("update LastActive set CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                }
                            }

                        }
                    }
                }
            }

            if (Location_N != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_N_V6_0>>(Location_N);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "";
                        Int64 CD = 0; Int64 CT = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);
                        CT = Convert.ToInt32(objResponse[i].CT);
                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                        DateTime mDate1 = DateTime.Parse("1970-01-01");
                        mDate1 = mDate1.AddSeconds(CT + 19800);
                        string cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));

                        //string Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate)" +
                        //                                "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + cdate1 + "')";
                        string Query = string.Empty;
                        string _chkduplicate = string.Empty;
                        //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
                        //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                        //if (_chkduplicate == "0")
                        {
                            Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description)" +
                                                         "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..')";
                            retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            DataTable dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                            if (dtla.Rows.Count > 0)
                            {
                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                            }
                            else
                            {
                                string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                            }
                        }
                    }
                }
            }

            if (MobileLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<MobileLog>>(MobileLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0, FromTime = 0, ToTime = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        FromTime = Convert.ToInt32(objResponse[i].FromTime);
                        ToTime = Convert.ToInt32(objResponse[i].ToTime);
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
                        string _chkduplicate = string.Empty;
                        //Query = "Select isnull(count(*),0) from Temp_Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + CurrentDate + "' ";
                        //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                        //if (_chkduplicate == "0")
                        {
                            Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime) values " +
                                    " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "','" + FromTime + "','" + ToTime + "')";
                            retVal = DbConnectionDAL.GetIntScalarVal(Query);
                        }
                    }
                }
            }

            if (TrackerLog != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<TrackerLog>>(TrackerLog);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Status = "";
                        Int64 CurrentDate = 0;
                        string Query = string.Empty;

                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Status = objResponse[i].Status.ToString();
                        CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
                        string _chkduplicate = string.Empty;
                        //Query = "Select isnull(count(*),0) from Temp_Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + CurrentDate + "' ";
                        //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                        //if (_chkduplicate == "0")
                        {
                            Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status) values " +
                                   " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "')";
                            retVal = DbConnectionDAL.GetIntScalarVal(Query);
                        }
                    }
                }
            }

            if (Fence != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Fence>>(Fence);

                    for (int j = 0; j < objResponse.Count; j++)
                    {
                        string DeviceNo = "", latitude = "",
                               longitude = "", PartyName = "";

                        DeviceNo = objResponse[j].DeviceNo.ToString();
                        latitude = objResponse[j].Lt.ToString();
                        longitude = objResponse[j].Lg.ToString();
                        PartyName = objResponse[j].PNm.ToString();

                        string str = "insert into Temp_FenceAddress (DeviceNo,Latitude,Longitude,PartyName,Created_date) values" +
                                         "('" + DeviceNo + "','" + latitude + "','" + longitude + "','" + PartyName + "','" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(str);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLocationDetails_Attendance(string Attendance)
    {
        int retVal = 0;
        try
        {
            if (Attendance != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<LcationAttendance>>(Attendance);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", LType = "", createddt = "";
                        Int64 CD = 0, Mcd = 0, CT = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);
                        CT = Convert.ToInt32(objResponse[i].CD);
                        LType = Convert.ToString(objResponse[i].LType);
                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                        DateTime mDate1 = DateTime.Parse("1970-01-01");
                        mDate1 = mDate1.AddSeconds(CT + 19800);
                        string cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));

                        if (!string.IsNullOrEmpty(LType))
                        {
                            string Query = "";
                            Query = "select * from Temp_LocationDetails where deviceno='" + DeviceNo + "' and LocationType='" + LType + "' and Cd='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                            DataTable dt = new DataTable();
                            dt = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
                            if (dt.Rows.Count == 0)
                            {
                                Query = "select * from LocationDetails where deviceno='" + DeviceNo + "' and vdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and LocationType='" + LType + "'";

                                dt = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
                                if (dt.Rows.Count == 0)
                                {

                                    //       Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,Cd,mobilecreateddate)" +
                                    //"values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + mDate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + mDate1 + "')";


                                    // this is used bcoz 6.2.2 is revised after 6.2.8
                                    Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,vdate,mobilecreateddate,description,insertdate)" +
                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "','Updated Soon..',getdate())";
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                }
                            }
                        }
                    }
                }
            }






        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLocationDetails_Attendance1(string Attendance)
    {
        int retVal = 0;
        try
        {
            if (Attendance != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<LcationAttendance>>(Attendance);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", LType = "", createddt = "";
                        Int64 CD = 0, Mcd = 0, CT = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        CD = Convert.ToInt32(objResponse[i].CD);
                        CT = Convert.ToInt32(objResponse[i].CD);
                        LType = Convert.ToString(objResponse[i].LType);
                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                        DateTime mDate1 = DateTime.Parse("1970-01-01");
                        mDate1 = mDate1.AddSeconds(CT + 19800);
                        string cdate1 = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (!string.IsNullOrEmpty(LType))
                        {
                            string Query = "";
                            Query = "select * from Temp_LocationDetails where deviceno='" + DeviceNo + "' and LocationType='" + LType + "' and Cd='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                            DataTable dt = new DataTable();
                            dt = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
                            if (dt.Rows.Count == 0)
                            {
                                Query = "select * from LocationDetails where deviceno='" + DeviceNo + "' and vdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and LocationType='" + LType + "'";

                                dt = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
                                if (dt.Rows.Count == 0)
                                {
                                    //       Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,Cd,mobilecreateddate)" +
                                    //"values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "')";
                                    Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,vdate,mobilecreateddate,description,insertdate)" +
                            "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "','Updated Soon..',getdate())";
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                    // DbConnectionDAL.ExecuteQuery(Query);

                                    DataTable dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                    if (!string.IsNullOrEmpty(Lt))
                                    {
                                        if (dtla.Rows.Count > 0)
                                        {
                                            DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                        }
                                        else
                                        {
                                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                                        }
                                    }
                                    else
                                    {
                                        if (dtla.Rows.Count > 0)
                                        {
                                            DbConnectionDAL.GetStringScalarVal("update LastActive set CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }






        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void ChkPushHeartV6_0(string Device, string CurrentDate, string heartbeat, string PushId)
    {

        int retVal = 0;
        try
        {
            string Query = string.Empty;
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(Convert.ToInt32(CurrentDate) + 19800);
            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Query = "update PushHeartChk set FlagByMOB='" + heartbeat + "',Mobdatetime='" + cdate + "' where deviceNo='" + Device + "' and Id=" + PushId + "";
            retVal = DbConnectionDAL.GetIntScalarVal(Query);
            Query = "update lastactive set currentdate=isnull((select max(currentdate) from locationdetails where cast(currentdate as date)=cast(GETDATE() as date) and deviceno=lastactive.deviceno and deviceno in (select DeviceNo from LastActive)),currentdate)";
            retVal = DbConnectionDAL.GetIntScalarVal(Query);
        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void LocationInsert_Schedulear()
    {

        int retVal = 0;
        string strLocation = "select  ld.Id,ld.DeviceNo,ld.Latitude,ld.Longitude,ld.Battery,ld.Gps_accuracy,ld.CurrentDate,ld.Log_m,pm.ID AS personid, ld.LocationType From Temp_LocationDetails ld LEFT JOIN PersonMaster pm ON ld.DeviceNo=pm.DeviceNo ";
        DataTable dtLocation = DbConnectionDAL.GetDataTable(CommandType.Text,strLocation);
        //   return;
        string strLog = "select Id,DeviceNo,Status,FromTime,ToTime,CurrentDate From Temp_Log_Tracker";
        DataTable dtLog = DbConnectionDAL.GetDataTable(CommandType.Text,strLog);

        string strFence = "select Id,DeviceNo,Latitude,Longitude,PartyName,Created_date From Temp_FenceAddress";
        DataTable dtFence = DbConnectionDAL.GetDataTable(CommandType.Text,strFence);

        if (dtLocation.Rows.Count > 0)
        {
            for (int i = 0; i < dtLocation.Rows.Count; i++)
            {
                string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Log_m = "", LocationAtt = "";
                Int64 CD = 0;
                Int32 Id = 0;

                DeviceNo = dtLocation.Rows[i]["DeviceNo"].ToString();
                Lt = dtLocation.Rows[i]["Latitude"].ToString();
                Lg = dtLocation.Rows[i]["Longitude"].ToString();
                Bt = dtLocation.Rows[i]["Battery"].ToString();
                Ga = dtLocation.Rows[i]["Battery"].ToString();
                Log_m = dtLocation.Rows[i]["Log_m"].ToString();
                CD = Convert.ToInt32(dtLocation.Rows[i]["CurrentDate"]);
                Id = Convert.ToInt32(dtLocation.Rows[i]["Id"]);
                LocationAtt = Convert.ToString(dtLocation.Rows[i]["LocationType"]);
                string HomeFlag = "N";
                decimal argLat = Convert.ToDecimal(Lt);
                decimal argLng = Convert.ToDecimal(Lg);
                int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];

                //string Pi = DbConnectionDAL.GetStringScalarVal("select Id from PersonMaster where deviceno='" + DeviceNo + "'");
                string Pi = dtLocation.Rows[i]["personid"].ToString();

                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CD + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                if (GetValidCountry(Lt, Lg))
                {
                    if (CurrDate >= mDate)
                    {
                        string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",G" + "," + Bt + "," +
                            Ga + "" + Environment.NewLine;
                        string approx = "0";
                        if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                        { approx = Ga; }
                        //DataTable dtla = new DataTable();
                        //dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        //if (dtla.Rows.Count > 0)
                        //{
                        //    string LActiveDate = dtla.Rows[0]["Currentdate"].ToString(); 
                        //    Common cm = new Common();
                        //    if (Convert.ToDateTime(cdate) > Convert.ToDateTime(LActiveDate))
                        //    {
                        //        DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                        //    }

                        DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");

                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                        string Address = "";
                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                        Address = DMT.InsertAddress(Lt, Lg);
                        if (!string.IsNullOrEmpty(FAddress))
                        { Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }

                        {
                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag,LocationType)" +
                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','" + Log_m + "','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "','" + LocationAtt + "')";
                            retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            DbConnectionDAL.GetIntScalarVal("delete from Temp_LocationDetails where Id =" + Id + "");

                        }

                    }
                }
                else
                {
                    string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";
                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                }
            }
        }

        if (dtLog.Rows.Count > 0)
        {
            for (int j = 0; j < dtLog.Rows.Count; j++)
            {
                string DeviceNo = "", Status = "", Query = "";
                Int64 CurrentDate = 0, FromTime = 0, ToTime = 0, Id = 0;

                DeviceNo = dtLog.Rows[j]["DeviceNo"].ToString();
                Status = dtLog.Rows[j]["Status"].ToString();
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["FromTime"].ToString()))
                { FromTime = Convert.ToInt32(dtLog.Rows[j]["FromTime"]); }
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["ToTime"].ToString()))
                { ToTime = Convert.ToInt32(dtLog.Rows[j]["ToTime"]); }

                CurrentDate = Convert.ToInt32(dtLog.Rows[j]["CurrentDate"]);
                Id = Convert.ToInt32(dtLog.Rows[j]["Id"]);

                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CurrentDate + 19800);

                DateTime FDate = DateTime.Parse("1970-01-01");
                FDate = FDate.AddSeconds(FromTime + 19800);

                DateTime TDate = DateTime.Parse("1970-01-01");
                TDate = TDate.AddSeconds(ToTime + 19800);

                if (Status == "M")
                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                else
                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_Log_Tracker where Id =" + Id + "");
            }
        }

        if (dtFence.Rows.Count > 0)
        {
            for (int k = 0; k < dtFence.Rows.Count; k++)
            {
                int Id = 0;
                string DeviceNo = "", latitude = "", longitude = "", PartyName = "";

                DeviceNo = dtFence.Rows[k]["DeviceNo"].ToString();
                latitude = dtFence.Rows[k]["Latitude"].ToString();
                longitude = dtFence.Rows[k]["Longitude"].ToString();
                PartyName = dtFence.Rows[k]["PartyName"].ToString();
                Id = Convert.ToInt32(dtFence.Rows[k]["Id"]);
                try
                {
                    string PersonID = DbConnectionDAL.GetStringScalarVal("select Id from Person master where deviceno='" + DeviceNo + "'");
                    DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text,"select GroupId from GrpMapp where personid=" + PersonID + "");
                    for (int i = 0; i < dtgrps.Rows.Count; i++)
                    {
                        string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                            "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(str);
                        int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_FenceAddress where Id =" + Id + "");
                    }
                }
                catch (Exception ex)
                { ex.ToString(); }

            }
        }
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void LocationInsert_Schedulear_V6_0()
    {
        int retVal = 0;

        DbConnectionDAL.GetStringScalarVal("update Temp_LocationDetails set MobileCreatedDate=currentdate where MobileCreatedDate is null");
        string strLocation = "select  ld.Id,ld.DeviceNo,ld.Latitude,ld.Longitude,ld.Battery,ld.Gps_accuracy,ld.CurrentDate,ld.Log_m,pm.ID AS personid, ld.LocationType,case when ld.mobilecreateddate is null then ld.CurrentDate else ld.mobilecreateddate  end as mobilecrdate From Temp_LocationDetails ld LEFT JOIN PersonMaster pm ON ld.DeviceNo=pm.DeviceNo where ld.mobilecreateddate is not null ";
        DataTable dtLocation = DbConnectionDAL.GetDataTable(CommandType.Text,strLocation);

        string strLog = "select Id,DeviceNo,Status,FromTime,ToTime,CurrentDate From Temp_Log_Tracker";
        DataTable dtLog = DbConnectionDAL.GetDataTable(CommandType.Text,strLog);

        string strFence = "select Id,DeviceNo,Latitude,Longitude,PartyName,Created_date From Temp_FenceAddress";
        DataTable dtFence = DbConnectionDAL.GetDataTable(CommandType.Text,strFence);

        if (dtLocation.Rows.Count > 0)
        {
            for (int i = 0; i < dtLocation.Rows.Count; i++)
            {
                string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Log_m = "", LocationAtt = "";
                Int64 CD = 0; Int64 CT = 0;
                Int32 Id = 0;

                DeviceNo = dtLocation.Rows[i]["DeviceNo"].ToString();
                Lt = dtLocation.Rows[i]["Latitude"].ToString();
                Lg = dtLocation.Rows[i]["Longitude"].ToString();
                Bt = dtLocation.Rows[i]["Battery"].ToString();
                Ga = dtLocation.Rows[i]["Battery"].ToString();
                Log_m = dtLocation.Rows[i]["Log_m"].ToString();
                CD = Convert.ToInt32(dtLocation.Rows[i]["CurrentDate"]);
                CT = Convert.ToInt32(dtLocation.Rows[i]["mobilecrdate"]);
                Id = Convert.ToInt32(dtLocation.Rows[i]["Id"]);
                LocationAtt = Convert.ToString(dtLocation.Rows[i]["LocationType"]);

                string HomeFlag = "N";
                decimal argLat = Convert.ToDecimal(Lt);
                decimal argLng = Convert.ToDecimal(Lg);
                int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];

                //string Pi = DbConnectionDAL.GetStringScalarVal("select Id from PersonMaster where deviceno='" + DeviceNo + "'");
                string Pi = dtLocation.Rows[i]["personid"].ToString();

                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CD + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateTime mDatecd = DateTime.Parse("1970-01-01");
                mDatecd = mDatecd.AddSeconds(CT + 19800);
                string mcdate = (mDatecd.ToString("yyyy-MM-dd HH:mm:ss"));

                DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                if (GetValidCountry(Lt, Lg))
                {
                    if (CurrDate >= mDate)
                    //if (CurrDate >= Convert.ToDateTime(dtLocation.Rows[i]["CurrentDate"]))
                    {
                        string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",G" + "," + Bt + "," +
                            Ga + "" + Environment.NewLine;
                        string approx = "0";
                        if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                        { approx = Ga; }

                        DataTable dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        if (dtla.Rows.Count > 0)
                        {
                            DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                        }
                        else
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                        }
                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                        string Address = "";
                        AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                        Address = DMT.InsertAddress(Lt, Lg);
                        if (!string.IsNullOrEmpty(FAddress))
                        { Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }


                        string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag,LocationType,mobilecreateddate)" +
                                  "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','" + Log_m + "','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "','" + LocationAtt + "','" + mcdate + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                        DbConnectionDAL.GetIntScalarVal("delete from Temp_LocationDetails where Id =" + Id + "");

                    }
                }
                else
                {
                    string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";
                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                }
            }
        }

        if (dtLog.Rows.Count > 0)
        {
            for (int j = 0; j < dtLog.Rows.Count; j++)
            {
                string DeviceNo = "", Status = "", Query = "";
                Int64 CurrentDate = 0, FromTime = 0, ToTime = 0, Id = 0;

                DeviceNo = dtLog.Rows[j]["DeviceNo"].ToString();
                Status = dtLog.Rows[j]["Status"].ToString();
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["FromTime"].ToString()))
                { FromTime = Convert.ToInt32(dtLog.Rows[j]["FromTime"]); }
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["ToTime"].ToString()))
                { ToTime = Convert.ToInt32(dtLog.Rows[j]["ToTime"]); }

                CurrentDate = Convert.ToInt32(dtLog.Rows[j]["CurrentDate"]);
                Id = Convert.ToInt32(dtLog.Rows[j]["Id"]);

                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CurrentDate + 19800);

                DateTime FDate = DateTime.Parse("1970-01-01");
                FDate = FDate.AddSeconds(FromTime + 19800);

                DateTime TDate = DateTime.Parse("1970-01-01");
                TDate = TDate.AddSeconds(ToTime + 19800);

                if (Status == "M")
                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                else
                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_Log_Tracker where Id =" + Id + "");
            }
        }

        if (dtFence.Rows.Count > 0)
        {
            for (int k = 0; k < dtFence.Rows.Count; k++)
            {
                int Id = 0;
                string DeviceNo = "", latitude = "", longitude = "", PartyName = "";

                DeviceNo = dtFence.Rows[k]["DeviceNo"].ToString();
                latitude = dtFence.Rows[k]["Latitude"].ToString();
                longitude = dtFence.Rows[k]["Longitude"].ToString();
                PartyName = dtFence.Rows[k]["PartyName"].ToString();
                Id = Convert.ToInt32(dtFence.Rows[k]["Id"]);
                try
                {
                    string PersonID = DbConnectionDAL.GetStringScalarVal("select Id from Person master where deviceno='" + DeviceNo + "'");
                    DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text,"select GroupId from GrpMapp where personid=" + PersonID + "");
                    for (int i = 0; i < dtgrps.Rows.Count; i++)
                    {
                        string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                            "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(str);
                        int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_FenceAddress where Id =" + Id + "");
                    }
                }
                catch (Exception ex)
                { ex.ToString(); }

            }
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void LocationInsert_Schedulear_V6_1()
    {
        int retVal = 0; string updateatt = "";
        //string updateatt = "update LocationDetails set Description='Updated Soon..' where description='' ";
        //DbConnectionDAL.ExecuteQuery(updateatt);
        //updateatt = "update locationdetails set description='Attendance Marked' where description='Updated Soon..' and Longitude='' and latitude='' ";
        //DbConnectionDAL.ExecuteQuery(updateatt);

        //string strLocation = "select top 10000 ld.Id,ld.DeviceNo,ld.Latitude,ld.Longitude,ld.Battery,ld.Gps_accuracy,ld.CurrentDate,ld.Log_m,pm.ID AS personid, ld.LocationType,case when ld.mobilecreateddate is null then ld.CurrentDate else ld.mobilecreateddate  end as mobilecrdate From LocationDetails ld WITH (NOLOCK) LEFT JOIN PersonMaster pm ON ld.DeviceNo=pm.DeviceNo where ld.description in ('Updated Soon..','')  and ld.Longitude!='' and ld.latitude!='' order by ld.id desc ";

        string strLocation = "select top 1 ld.Id,ld.DeviceNo,ld.Latitude,ld.Longitude,ld.CurrentDate From LocationDetails ld WITH (NOLOCK) where ld.description in ('Updated Soon..','','-1')  and ld.Longitude!='' and ld.latitude!='' ";
        //and cast(currentdate as date)>'2019-03-13'
        DataTable dtLocation = DbConnectionDAL.GetDataTable(CommandType.Text,strLocation);

        updateatt = "update lastactive set currentdate=isnull((select max(currentdate) from locationdetails where cast(currentdate as date)=cast(GETDATE() as date) and deviceno=lastactive.deviceno and deviceno in (select DeviceNo from LastActive)),currentdate)";
        DbConnectionDAL.ExecuteQuery(updateatt);
        string strLog = "select Id,DeviceNo,Status,FromTime,ToTime,CurrentDate,personid From Temp_Log_Tracker where deviceno='866896048779759'";
        DataTable dtLog = DbConnectionDAL.GetDataTable(CommandType.Text,strLog);

        string strFence = "select Id,DeviceNo,Latitude,Longitude,PartyName,Created_date From Temp_FenceAddress";
        DataTable dtFence = DbConnectionDAL.GetDataTable(CommandType.Text,strFence);
        int AddressTobeUpdated = 0;
        int UpdatedAddress = 0;
        int LogTobeUpdated = 0;
        int Updatedlog = 0;
        int FenceTobeUpdated = 0;
        int UpdatedFence = 0;
        if (dtLocation.Rows.Count > 0)
        {
            AddressTobeUpdated = dtLocation.Rows.Count;
            for (int i = 0; i < dtLocation.Rows.Count; i++)
            {
                try
                {


                    UpdatedAddress = UpdatedAddress + 1;
                    string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Log_m = "", LocationAtt = "";
                    Int64 CD = 0; Int64 CT = 0;
                    Int32 Id = 0;

                    DeviceNo = dtLocation.Rows[i]["DeviceNo"].ToString();
                    Lt = dtLocation.Rows[i]["Latitude"].ToString();
                    Lg = dtLocation.Rows[i]["Longitude"].ToString();
                    //Bt = dtLocation.Rows[i]["Battery"].ToString();
                    //Ga = dtLocation.Rows[i]["Gps_accuracy"].ToString();
                    //Log_m = dtLocation.Rows[i]["Log_m"].ToString();

                    Id = Convert.ToInt32(dtLocation.Rows[i]["Id"]);
                  //  LocationAtt = Convert.ToString(dtLocation.Rows[i]["LocationType"]);

                    string HomeFlag = "N";
                    decimal argLat = Convert.ToDecimal(Lt);
                    decimal argLng = Convert.ToDecimal(Lg);
                    int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                    int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
                  //  string Pi = dtLocation.Rows[i]["personid"].ToString();

                    DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                    if (GetValidCountry(Lt, Lg))
                    {
                        //if (CurrDate >= mDate)
                        {

                            string Address = "";
                            //string addqry = "select top 1 address from [dbo].[GAddMerge] where lat= SUBSTRING('" + Lt + "', 0, 8)  and long=SUBSTRING('" + Lg + "', 0, 8)";
                            //Address = DbConnectionDAL.GetStringScalarVal(addqry);
                            //if (!String.IsNullOrEmpty(Address))
                            //{
                            //    string Query = "update LocationDetails set Description='" + Address + "' ,slot=DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0,CurrentDate), 0), CurrentDate),HomeFlag='" + HomeFlag + "',vdate=cast(currentdate as date) where Id=" + Id + "";
                            //    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            //}
                            //else
                            //{
                                //string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + dtLocation.Rows[i]["CurrentDate"].ToString() + ",G" + "," + Bt + "," +
                                //    Ga + "" + Environment.NewLine;
                                string approx = "0";
                                if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                                { approx = Ga; }

                                //CheckFenceAlert(Lt, Lg, Pi, dtLocation.Rows[i]["CurrentDate"].ToString());
                                //string FAddress = GetFenceAddress(Lt, Lg, Pi);

                                //AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                //Address = DMT.InsertAddress(Lt, Lg);
                                AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient
                                    ("WebServiceSoap");
                            
                                Address = DMT.InsertAddress(Lt, Lg);
                                //if (!string.IsNullOrEmpty(FAddress))
                                //{ Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }

                                {
                                    if (Address != "")
                                    {
                                        string Query = "update LocationDetails set Description='" + Address + "' ,slot=DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0,CurrentDate), 0), CurrentDate),HomeFlag='" + HomeFlag + "',vdate=cast(currentdate as date) where Id=" + Id + "";
                                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                    }
                                    // if address is blank then do not insert in to gaddmerge  13/Nov/2019
                                    //if (Address != "")
                                    //{
                                    //    Query = "insert into [GAddMerge](lat,Long,[address])values (SUBSTRING('" + Lt + "', 0, 8),SUBSTRING('" + Lg + "', 0, 8),'" + Address + "')";
                                    //    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                    //}
                                }

                            //}
                        }
                    }
                    else
                    {
                        string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + dtLocation.Rows[i]["CurrentDate"].ToString() + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + dtLocation.Rows[i]["CurrentDate"].ToString() + "'), 0), '" + dtLocation.Rows[i]["CurrentDate"].ToString() + "'))";
                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
        }

        if (dtLog.Rows.Count > 0)
        {
            LogTobeUpdated = dtLog.Rows.Count;
            for (int j = 0; j < dtLog.Rows.Count; j++)
            {
                string DeviceNo = "", Status = "", Query = "",personid="";
                Int64 CurrentDate = 0, FromTime = 0, ToTime = 0, Id = 0;

                DeviceNo = dtLog.Rows[j]["DeviceNo"].ToString();
                Status = dtLog.Rows[j]["Status"].ToString();
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["FromTime"].ToString()))
                { FromTime = Convert.ToInt32(dtLog.Rows[j]["FromTime"]); }
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["ToTime"].ToString()))
                { ToTime = Convert.ToInt32(dtLog.Rows[j]["ToTime"]); }

                CurrentDate = Convert.ToInt32(dtLog.Rows[j]["CurrentDate"]);
                Id = Convert.ToInt32(dtLog.Rows[j]["Id"]);
                personid = Convert.ToString(dtLog.Rows[j]["personid"]);

                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CurrentDate + 19800);
            
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateTime FDate = DateTime.Parse("1970-01-01");
                FDate = FDate.AddSeconds(FromTime + 19800);

                DateTime TDate = DateTime.Parse("1970-01-01");
                TDate = TDate.AddSeconds(ToTime + 19800);
                string _chkduplicate = string.Empty; string _chkduplicateQry = string.Empty;
                if (Status == "M")
                {

                    Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot,personid) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + FDate + "','" + TDate + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "')," + personid + ")";
                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    Updatedlog = Updatedlog + 1;
                }
                else
                {
                    //_chkduplicateQry = "Select isnull(count(*),0) from Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + mDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    //_chkduplicate = DbConnectionDAL.GetStringScalarVal(_chkduplicateQry);
                    //if (_chkduplicate == "0")
                    {
                        //Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot,personid) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "')," + personid + ")";
                        Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot,personid) values ('" + DeviceNo + "','" + cdate + "','" + Status + "','" + FDate + "','" + TDate + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "')," + personid + ")";
                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                        Updatedlog = Updatedlog + 1;
                    }

                }

                int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_Log_Tracker where Id =" + Id + "");

            }
        }

        if (dtFence.Rows.Count > 0)
        {
            FenceTobeUpdated = dtFence.Rows.Count;
            for (int k = 0; k < dtFence.Rows.Count; k++)
            {
                int Id = 0;
                string DeviceNo = "", latitude = "", longitude = "", PartyName = "";

                DeviceNo = dtFence.Rows[k]["DeviceNo"].ToString();
                latitude = dtFence.Rows[k]["Latitude"].ToString();
                longitude = dtFence.Rows[k]["Longitude"].ToString();
                PartyName = dtFence.Rows[k]["PartyName"].ToString();
                Id = Convert.ToInt32(dtFence.Rows[k]["Id"]);
                try
                {
                    string PersonID = DbConnectionDAL.GetStringScalarVal("select Id from Personmaster where deviceno='" + DeviceNo + "' and active='Y'");
                    if (!string.IsNullOrEmpty(PersonID))
                    {
                        DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text,"select GroupId from GrpMapp where personid=" + PersonID + "");
                        for (int i = 0; i < dtgrps.Rows.Count; i++)
                        {
                            string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                                "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                            retVal = DbConnectionDAL.GetIntScalarVal(str);
                            int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_FenceAddress where Id =" + Id + "");
                            UpdatedFence = UpdatedFence + 1;
                        }
                    }

                }
                catch (Exception ex)
                { ex.ToString(); continue; }

            }
        }
        Context.Response.Write(JsonConvert.SerializeObject(UpdatedAddress + " Address Updated Out Of " + AddressTobeUpdated + " " + Updatedlog + " Log Updated Out Of " + LogTobeUpdated + " " + UpdatedFence + " Fence Updated out of " + FenceTobeUpdated + " "));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void Utility_DeleteDuplicateLog(string date)
    {
        string status = ""; string message = "";
        try
        {

            if (!string.IsNullOrEmpty(date))
            {
                string selectsql = " select DeviceNo from personmaster group by deviceno having len(deviceno)>14 ";
                DataTable dtselect = DbConnectionDAL.GetDataTable(CommandType.Text,selectsql);
                if (dtselect.Rows.Count > 0)
                {
                    for (int i = 0; i < dtselect.Rows.Count; i++)
                    {
                        int retsave = 0; int totalduplicatesave = 0;
                        string chklogdate = " select max(id) as Id,currentdate from Log_Tracker where   deviceno='" + Convert.ToString(dtselect.Rows[i]["deviceno"]) + "' and vdate='" + Convert.ToDateTime(date).ToString("yyyy-MM-dd") + "' and   CurrentDate in (select CurrentDate from Log_Tracker where deviceno='" + Convert.ToString(dtselect.Rows[i]["deviceno"]) + "' and vdate='" + Convert.ToDateTime(date).ToString("yyyy-MM-dd") + "'    group by CurrentDate having count(CurrentDate)>1) group by CurrentDate order by CurrentDate";
                        DataTable dtlog = DbConnectionDAL.GetDataTable(CommandType.Text,chklogdate);
                        if (dtlog.Rows.Count > 0)
                        {
                            totalduplicatesave += dtlog.Rows.Count;
                            for (int j = 0; j < dtlog.Rows.Count; j++)
                            {
                                string deletelog = "delete from log_tracker where  deviceno='" + Convert.ToString(dtselect.Rows[i]["deviceno"]) + "' and vdate='" + Convert.ToDateTime(date).ToString("yyyy-MM-dd") + "' and currentdate='" + dtlog.Rows[j]["currentdate"].ToString() + "' and id !=" + dtlog.Rows[j]["Id"].ToString() + "";
                                retsave += DbConnectionDAL.ExecuteQuery(deletelog);
                                if (retsave == totalduplicatesave)
                                {
                                    status = "2";
                                    message = retsave.ToString() + " Data Deleted ";
                                }
                                else
                                {
                                    status = "3";
                                    message = retsave.ToString() + " Data Deleted ";
                                }
                            }
                        }


                    }
                }
                else
                {
                    status = "1";
                    message = "No Data To Check";
                }

            }
            else
            {
                status = "0";
                message = "Date Is Not In Correct Format (yyyy-MM-dd)";
            }
        }

        catch (Exception ex)
        {
            status = "0";
            message = "Error";
        }
        string[] array = new string[1];
        array[0] = string.Empty;
        List<ModelDataInsertLeave> rsttarget = new List<ModelDataInsertLeave>();
        rsttarget.Add(
                      new ModelDataInsertLeave
                      {
                          result = date
                      }
                  );
        ModelInsertLeave rst = new ModelInsertLeave
        {
            Status = status,
            Message = message,
            Data = rsttarget
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void LocationInsert_Schedulear_V6_1_reverseOrder()
    {
        int retVal = 0;

        //string strLocation = "select ld.Id,ld.DeviceNo,ld.Latitude,ld.Longitude,ld.Battery,ld.Gps_accuracy,ld.CurrentDate,ld.Log_m,pm.ID AS personid, ld.LocationType,case when ld.mobilecreateddate is null then ld.CurrentDate else ld.mobilecreateddate  end as mobilecrdate From LocationDetails ld  WITH (NOLOCK) LEFT JOIN PersonMaster pm ON ld.DeviceNo=pm.DeviceNo where ld.description='Updated Soon..' and ld.Longitude!='' and ld.latitude!=''  and cast(ld.currentdate as date)>'2019-03-13' order by id desc ";
        string strLocation = "select  top (select RecordAmt from enviro) ld.Id,ld.DeviceNo,ld.Latitude,ld.Longitude,ld.Battery,ld.Gps_accuracy,ld.CurrentDate,ld.Log_m,pm.ID AS personid, ld.LocationType,case when ld.mobilecreateddate is null then ld.CurrentDate else ld.mobilecreateddate  end as mobilecrdate From LocationDetails ld  WITH (NOLOCK) LEFT JOIN PersonMaster pm ON ld.DeviceNo=pm.DeviceNo where ld.description='Updated Soon..' and ld.Longitude!='' and ld.latitude!='' order by id desc ";
        DataTable dtLocation = DbConnectionDAL.GetDataTable(CommandType.Text,strLocation);
        string updateatt = "update locationdetails set description='Attendance Marked' where description='Updated Soon..' and Longitude='' and latitude='' ";
        DbConnectionDAL.ExecuteQuery(updateatt);
        string strLog = "select Id,DeviceNo,Status,FromTime,ToTime,CurrentDate From Temp_Log_Tracker";
        DataTable dtLog = DbConnectionDAL.GetDataTable(CommandType.Text,strLog);

        string strFence = "select Id,DeviceNo,Latitude,Longitude,PartyName,Created_date From Temp_FenceAddress";
        DataTable dtFence = DbConnectionDAL.GetDataTable(CommandType.Text,strFence);
        int AddressTobeUpdated = 0;
        int UpdatedAddress = 0;

        if (dtLocation.Rows.Count > 0)
        {
            AddressTobeUpdated = dtLocation.Rows.Count;
            for (int i = 0; i < dtLocation.Rows.Count; i++)
            {
                try
                {

                    UpdatedAddress = UpdatedAddress + 1;
                    string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Log_m = "", LocationAtt = "";
                    Int64 CD = 0; Int64 CT = 0;
                    Int32 Id = 0;

                    DeviceNo = dtLocation.Rows[i]["DeviceNo"].ToString();
                    Lt = dtLocation.Rows[i]["Latitude"].ToString();
                    Lg = dtLocation.Rows[i]["Longitude"].ToString();
                    Bt = dtLocation.Rows[i]["Battery"].ToString();
                    Ga = dtLocation.Rows[i]["Battery"].ToString();
                    Log_m = dtLocation.Rows[i]["Log_m"].ToString();

                    Id = Convert.ToInt32(dtLocation.Rows[i]["Id"]);
                    LocationAtt = Convert.ToString(dtLocation.Rows[i]["LocationType"]);

                    string HomeFlag = "N";
                    decimal argLat = Convert.ToDecimal(Lt);
                    decimal argLng = Convert.ToDecimal(Lg);
                    int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                    int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
                    string Pi = dtLocation.Rows[i]["personid"].ToString();

                    DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                    if (GetValidCountry(Lt, Lg))
                    {
                        //if (CurrDate >= mDate)
                        {
                            string Address = "";
                            string addqry = "select top 1 address from [dbo].[GAddMerge] where lat= SUBSTRING('" + Lt + "', 0, 8)  and long=SUBSTRING('" + Lg + "', 0, 8)";
                            Address = DbConnectionDAL.GetStringScalarVal(addqry);
                            if (!String.IsNullOrEmpty(Address))
                            {
                                string Query = "update LocationDetails set Description='" + Address + "' ,slot=DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0,CurrentDate), 0), CurrentDate),HomeFlag='" + HomeFlag + "',vdate=cast(currentdate as date) where Id=" + Id + "";
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            }
                            else
                            {
                                string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + dtLocation.Rows[i]["CurrentDate"].ToString() + ",G" + "," + Bt + "," +
                                    Ga + "" + Environment.NewLine;
                                string approx = "0";
                                if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                                { approx = Ga; }

                                CheckFenceAlert(Lt, Lg, Pi, dtLocation.Rows[i]["CurrentDate"].ToString());
                                string FAddress = GetFenceAddress(Lt, Lg, Pi);

                                AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                Address = DMT.InsertAddress(Lt, Lg);
                                if (!string.IsNullOrEmpty(FAddress))
                                { Address = Address + " (" + FAddress + ")"; HomeFlag = "H"; }

                                {
                                    string Query = "update LocationDetails set Description='" + Address + "' ,slot=DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0,CurrentDate), 0), CurrentDate),HomeFlag='" + HomeFlag + "',vdate=cast(currentdate as date) where Id=" + Id + "";
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                    Query = "insert into [GAddMerge](lat,Long,[address])values (SUBSTRING('" + Lt + "', 0, 8),SUBSTRING('" + Lg + "', 0, 8),'" + Address + "')";
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                }

                            }
                        }
                    }
                    else
                    {
                        string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + dtLocation.Rows[i]["CurrentDate"].ToString() + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + dtLocation.Rows[i]["CurrentDate"].ToString() + "'), 0), '" + dtLocation.Rows[i]["CurrentDate"].ToString() + "'))";
                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    }
                }

                catch (Exception ex)
                {
                    continue;
                }


            }
        }

        if (dtLog.Rows.Count > 0)
        {
            for (int j = 0; j < dtLog.Rows.Count; j++)
            {
                string DeviceNo = "", Status = "", Query = "";
                Int64 CurrentDate = 0, FromTime = 0, ToTime = 0, Id = 0;

                DeviceNo = dtLog.Rows[j]["DeviceNo"].ToString();
                Status = dtLog.Rows[j]["Status"].ToString();
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["FromTime"].ToString()))
                { FromTime = Convert.ToInt32(dtLog.Rows[j]["FromTime"]); }
                if (!string.IsNullOrEmpty(dtLog.Rows[j]["ToTime"].ToString()))
                { ToTime = Convert.ToInt32(dtLog.Rows[j]["ToTime"]); }

                CurrentDate = Convert.ToInt32(dtLog.Rows[j]["CurrentDate"]);
                Id = Convert.ToInt32(dtLog.Rows[j]["Id"]);

                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CurrentDate + 19800);

                DateTime FDate = DateTime.Parse("1970-01-01");
                FDate = FDate.AddSeconds(FromTime + 19800);

                DateTime TDate = DateTime.Parse("1970-01-01");
                TDate = TDate.AddSeconds(ToTime + 19800);

                if (Status == "M")
                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                else
                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_Log_Tracker where Id =" + Id + "");
            }
        }

        if (dtFence.Rows.Count > 0)
        {
            for (int k = 0; k < dtFence.Rows.Count; k++)
            {
                int Id = 0;
                string DeviceNo = "", latitude = "", longitude = "", PartyName = "";

                DeviceNo = dtFence.Rows[k]["DeviceNo"].ToString();
                latitude = dtFence.Rows[k]["Latitude"].ToString();
                longitude = dtFence.Rows[k]["Longitude"].ToString();
                PartyName = dtFence.Rows[k]["PartyName"].ToString();
                Id = Convert.ToInt32(dtFence.Rows[k]["Id"]);
                try
                {
                    string PersonID = DbConnectionDAL.GetStringScalarVal("select Id from Person master where deviceno='" + DeviceNo + "'");
                    DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text,"select GroupId from GrpMapp where personid=" + PersonID + "");
                    for (int i = 0; i < dtgrps.Rows.Count; i++)
                    {
                        string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                            "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                        retVal = DbConnectionDAL.GetIntScalarVal(str);
                        int retdel = DbConnectionDAL.GetIntScalarVal("delete from Temp_FenceAddress where Id =" + Id + "");
                    }
                }
                catch (Exception ex)
                { ex.ToString(); }

            }
        }
        Context.Response.Write(JsonConvert.SerializeObject(UpdatedAddress + " Address Updated Out Of " + AddressTobeUpdated + " "));
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void ExportUserLog_V6_0()
    {
        string flag = "No";

        var httpRequest = HttpContext.Current.Request;
        if (httpRequest.Files.Count > 0)
        {

            string docfiles = "";
            foreach (string file in httpRequest.Files)
            {
                try
                {
                    string DeviceNo = httpRequest.Params["DeviceNo"].ToString();
                    var postedFile = httpRequest.Files[file];
                    string fileName = Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                    string strQ = "select * from UserMobileLog where DeviceNo='" + DeviceNo + "' ";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,strQ);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            string path = dt.Rows[i]["Filename"].ToString();
                            string strupdtstatus = "delete from UserMobileLog where DeviceNo=" + DeviceNo + " ";
                            int retVal1 = DbConnectionDAL.GetIntScalarVal(strupdtstatus);
                            System.IO.File.Delete(Server.MapPath(path));
                        }
                    }
                    var filePath = HttpContext.Current.Server.MapPath("~/UserMobileLog/" + timeStamp + '-' + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    docfiles += fileName + ",";

                    String varname15 = "";
                    varname15 = varname15 + "INSERT INTO [dbo].[UserMobileLog] " + "\n";
                    varname15 = varname15 + "           ([DeviceNo] " + "\n";
                    varname15 = varname15 + "           ,[Filename]) " + "\n";
                    varname15 = varname15 + "     VALUES " + "\n";
                    varname15 = varname15 + "             ('" + DeviceNo + "',\n";
                    varname15 = varname15 + "             '/UserMobileLog/" + timeStamp + '-' + postedFile.FileName + "')";
                    int retVal = DbConnectionDAL.GetIntScalarVal(varname15);
                    flag = "Yes";
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }
        Context.Response.Write(JsonConvert.SerializeObject(flag));

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsN(string Pi, string Lt, string Lg, Int64 CD, string Bt, string Ga)
    {
        int retVal = 0;
        try
        {
            string HomeFlag = "N";
            decimal argLat = Convert.ToDecimal(Lt);
            decimal argLng = Convert.ToDecimal(Lg);
            int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
            int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
            //if (cntLat > 4 && cntLng > 4)
            //{

            string getdevid = "select DeviceNo from PersonMaster where ID=" + Pi + "";
            string DeviceNo = DbConnectionDAL.GetStringScalarVal(getdevid);
            if (!string.IsNullOrEmpty(DeviceNo))
            {
                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CD + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);

                if (GetValidCountry(Lt, Lg))
                {
                    if (CurrDate >= mDate)
                    {
                        string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",N" + "," + Bt + "," +
                            Ga + "" + Environment.NewLine;
                        string approx = "0";
                        if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                        { approx = Ga; }
                        DataTable dtla = new DataTable();
                        dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        if (dtla.Rows.Count > 0)
                        {
                            string LActiveDate = dtla.Rows[0]["Currentdate"].ToString();
                            Common cm = new Common();
                            // 31/07/2017  Nishu
                            //if (cm.GetValidLocationTicks(dtla.Rows[0]["lat"].ToString(), dtla.Rows[0]["long"].ToString(), Lt, Lg, Convert.ToDateTime(dtla.Rows[0]["currentdate"]), mDate, "N") == true)
                            //{
                            if (Convert.ToDateTime(cdate) > Convert.ToDateTime(LActiveDate))
                            {
                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");
                            }
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Lt, Lg);
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            //  if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            }

                            //}
                        }

                        else
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag ) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                            //                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //   Address = DMT.InsertAddress(Lt, Lg);
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                        //if (DeviceNo == "911531150035766")
                        //{
                        using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                        {
                            file2.WriteLine(createText);
                            file2.Close();
                        }
                        //}
                    }
                    else
                    {
                        string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    }
                }
                //}
                //else
                //{
                //    retVal = 0;
                //}
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        string msg = "N";
        if (retVal == 0)
            msg = "Y";

        Result rst = new Result
        {
            ResultMsg = msg

        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsN_New(string DeviceNo, string Lt, string Lg, Int64 CD, string Bt, string Ga)
    {
        int retVal = 0;
        try
        {
            string HomeFlag = "N";
            decimal argLat = Convert.ToDecimal(Lt);
            decimal argLng = Convert.ToDecimal(Lg);
            int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
            int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
            //if (cntLat > 4 && cntLng > 4)
            //{

            //string getdevid = "select DeviceNo from PersonMaster where ID=" + Pi + "";
            //string DeviceNo = DbConnectionDAL.GetStringScalarVal(getdevid);
            string Pi = DbConnectionDAL.GetStringScalarVal("select Id from PersonMaster where deviceno='" + DeviceNo + "'");
            if (!string.IsNullOrEmpty(DeviceNo))
            {
                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CD + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                if (GetValidCountry(Lt, Lg))
                {
                    if (CurrDate >= mDate)
                    {
                        string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",N" + "," + Bt + "," +
                            Ga + "" + Environment.NewLine;
                        string approx = "0";
                        if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                        { approx = Ga; }
                        DataTable dtla = new DataTable();
                        dtla = DbConnectionDAL.GetDataTable(CommandType.Text,"select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        if (dtla.Rows.Count > 0)
                        {
                            string LActiveDate = dtla.Rows[0]["Currentdate"].ToString();
                            Common cm = new Common();
                            // 31/07/2017  Nishu
                            //if (cm.GetValidLocationTicks(dtla.Rows[0]["lat"].ToString(), dtla.Rows[0]["long"].ToString(), Lt, Lg, Convert.ToDateTime(dtla.Rows[0]["currentdate"]), mDate, "N") == true)
                            //{
                            if (Convert.ToDateTime(cdate) > Convert.ToDateTime(LActiveDate))
                            {
                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");
                            }
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Lt, Lg);
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            //  if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            }

                            //}
                        }

                        else
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag ) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + " (" + FAddress + ")"; }
                            //                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //   Address = DMT.InsertAddress(Lt, Lg);
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            }
                        }
                        //if (DeviceNo == "911531150035766")
                        //{
                        using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                        {
                            file2.WriteLine(createText);
                            file2.Close();
                        }
                        //}
                    }
                    else
                    {
                        string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
                    }
                }
                //}
                //else
                //{
                //    retVal = 0;
                //}
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        string msg = "N";
        if (retVal == 0)
            msg = "Y";

        Result rst = new Result
        {
            ResultMsg = msg

        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetValidUser(string DeviceNo)
    {
        string str = "select case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') when 1 then " +
 " (select case (select 1 from LineMaster where (Validdate < dateadd(ss,19800,getutcdate())) and  DeviceId='" + DeviceNo + "' and Product='TRACKER') " +
 " when 1 then 'EX' else (select case When (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') IS NULL then 'N' else " +
    " (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') end) end) " +
   " else 'N' End as URL";
        DataTable dt = new DataTable();
        dt = DbConnectionDAL.getFromDataTableDmLicence(str);
        Url rst = new Url
        {
            UrlMsg = dt.Rows[0]["URL"].ToString()
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetenterdeviceid(string device, string mobile)
    {
        string msz = "N";
        //string str = "select * from LineMaster WHERE Mobile='" + mobile + "'  and otp='"+otp+"'";
        //DataTable dtcheck = new DataTable();
        //dtcheck = DbConnectionDAL.getFromDataTableDmLicence(str);
        //if (dtcheck.Rows.Count > 0)
        {
            //string insqry = " update linemaster set deviceid='" + device + "' where mobile='" + mobile + "'";
            //int val1 = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);

            string insqry = " update mastsalesrep set deviceno='" + device + "' where mobile='" + mobile + "' and active=1 ";
            int insert = DbConnectionDAL.ExecuteQuery(insqry);
            insqry = "select * from LastActive where deviceno='" + device + "'";
            DataTable dtchk = DbConnectionDAL.GetDataTable(CommandType.Text,insqry);

            if (dtchk.Rows.Count == 0)
            {
                int retVal = 0;
                DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);
                string inslastactive = "insert into LastActive(DeviceNo,CurrentDate,smsFlag) values ('" + device + "','" + CurrDate + "', '26.449923','80.331871','1')";
                retVal = DbConnectionDAL.GetIntScalarVal(inslastactive);
            }
            if (insert > 0)
            {
                msz = "Y";
            }
        }
        messagemodal rst = new messagemodal
        {
            message = msz
        };
        Context.Response.Write(JsonConvert.SerializeObject(msz));
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetOtpForWeb(string mobile)
    {
        string msz = "N";
        List<getotpforweb> rst = new List<getotpforweb>();
        {
            //string insqry = " update linemaster set deviceid='" + device + "' where mobile='" + mobile + "'";
            //int val1 = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
            string insqry = " select url,product,otp,otp_date from linemaster where mobile='" + mobile + "' ";
            DataTable dtchk = DbConnectionDAL.getFromDataTableDmLicence(insqry);

            if (dtchk.Rows.Count > 0)
            {
                for (int i = 0; i < dtchk.Rows.Count; i++)
                {
                    rst.Add(
                                new getotpforweb
                                {
                                    Url = dtchk.Rows[i]["url"].ToString(),
                                    Product = dtchk.Rows[i]["product"].ToString(),
                                    Otp = dtchk.Rows[i]["otp"].ToString(),
                                    ValidTill = Convert.ToDateTime(dtchk.Rows[i]["otp_date"].ToString()).AddMinutes(14).ToString()
                                }
                            );
                }
            }

        }

        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetNewValidUser(string DeviceNo)
    {
        string str = "select (SELECT isnull(ShowFence,'N') FROM CompMaster WHERE CompCode=(SELECT CompCode FROM LineMaster WHERE " + " DeviceId='" + DeviceNo + "' and Product='TRACKER')" +
  ")as ShowFence, case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') when 1 then " +
  " (select case (select 1 from LineMaster where (Validdate < dateadd(ss,19800,getutcdate())) and  DeviceId='" + DeviceNo + "' and Product='TRACKER') " +
  " when 1 then 'EX' else (select case When (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') IS NULL then 'N' else " +
     " (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') end) end) " +
    " else 'N' End as URL";
        DataTable dt = new DataTable();
        dt = DbConnectionDAL.getFromDataTableDmLicence(str);
        UrlNew rst = new UrlNew
        {
            UrlMsgnew = dt.Rows[0]["URL"].ToString(),
            ShowFence = dt.Rows[0]["ShowFence"].ToString()



        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void UpdatePersonmasterByPush()
    {
        string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
        string Query = "select  DeviceNo from PersonMaster ";
        DataTable dtbl1 = new DataTable();
        dtbl1 = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
        int count = dtbl1.Rows.Count; int cousend = 0;
        for (int i = 0; i < dtbl1.Rows.Count - 1; i++)
        {
            string DeviceNo = dtbl1.Rows[0]["DeviceNo"].ToString();

            Query = "select PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc,SrMobile,SendSmsSenior from PersonMaster where DeviceNo='" + DeviceNo + "'";
            DataTable dtbl = new DataTable();
            dtbl = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
            string regid_query = "select Reg_id from LineMaster where deviceid='" + DeviceNo + "' and Upper(Product)='TRACKER'";

            string Query1 = "select case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER')" +
           " when 1 THEN (select case when( select URL from LineMaster lm WHERE lm.DeviceId='" + DeviceNo + "' and Upper(lm.Product)='TRACKER')='demotracker.dataman.in'" +
          " THEN (SELECT CASE (SELECT 1 FROM LineMaster where  DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER' AND Validdate < dateadd(ss,19800,getutcdate())) WHEN 1 THEN 'N' ELSE 'Y' END )" +
          "ELSE (SELECT CASE(SELECT 1 FROM LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER' AND Active ='Y')" +
          " WHEN 1 THEN 'Y' ELSE 'N' END )END ) ELSE 'N' END ";
            string us = DbConnectionDAL.GetStringScalarVal(Query1);
            SqlConnection cn = new SqlConnection(constrDmLicense);
            SqlCommand cmd = new SqlCommand(regid_query, cn);
            SqlCommand cmd1 = new SqlCommand(Query1, cn);
            cmd.CommandType = CommandType.Text;
            cmd1.CommandType = CommandType.Text;
            cn.Open();
            //string regId = cmd.ExecuteScalar().ToString();
            string regId = Convert.ToString(cmd.ExecuteScalar());
            //string licenceinfo = cmd1.ExecuteScalar().ToString();
            string licenceinfo = Convert.ToString(cmd1.ExecuteScalar());
            cn.Close();
            cmd = null;
            if (!string.IsNullOrEmpty(regId))
            {
                string appid = DbConnectionDAL.GetStringScalarVal("select serverkey from enviro ");
                string senderid = DbConnectionDAL.GetStringScalarVal("select Senderid from enviro ");
                NotificationMessage nm = new NotificationMessage();
                nm.Title = "Testing";
                nm.Message = "";
                nm.DeviceNo = DeviceNo;
                nm.licenceinfo = licenceinfo;
                nm.PersonName = dtbl.Rows[0]["PersonName"].ToString();
                nm.FromTime = dtbl.Rows[0]["FromTime"].ToString();
                nm.ToTime = dtbl.Rows[0]["ToTime"].ToString();
                nm.Interval = dtbl.Rows[0]["Interval"].ToString();
                nm.UploadInterval = dtbl.Rows[0]["UploadInterval"].ToString();
                nm.RetryInterval = dtbl.Rows[0]["RetryInterval"].ToString();
                nm.Degree = dtbl.Rows[0]["Degree"].ToString();
                nm.GpsLoc = dtbl.Rows[0]["GpsLoc"].ToString();
                nm.Sys_Flag = dtbl.Rows[0]["Sys_Flag"].ToString();
                nm.MobileLoc = dtbl.Rows[0]["MobileLoc"].ToString();
                nm.SrMobile = dtbl.Rows[0]["SrMobile"].ToString();
                nm.SendSmsSr = dtbl.Rows[0]["SendSmsSenior"].ToString();
                var value = new JavaScriptSerializer().Serialize(nm);

                WebRequest tRequest;

                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");

                tRequest.Method = "post";

                //  tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", appid));

                tRequest.Headers.Add(string.Format("Sender: id={0}", senderid));

                //Data post to server  

                string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + value + ",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + regId + "\"]}";
                ////string postData =
                ////    "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message="
                ////    + value + "&data.title="
                ////    + title + "&data.subtitle="
                ////    + subtitle + "&data.openpage="
                ////    + openpage + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" +
                //        regId + "";

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);

                String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.

                //Label1.Text = sResponseFromServer;      //Assigning GCM response to Label text 

                tReader.Close();

                dataStream.Close();
                tResponse.Close();
                cousend = cousend + 1;
            }
        }

        Context.Response.Write(JsonConvert.SerializeObject("Push Send to" + cousend + " devices"));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetPersonAlarmSMS(string DeviceNo)
    {
        string Query = "select Alarm,AlarmDurationMins,SendSMS from PersonMaster where DeviceNo='" + DeviceNo + "'";
        DataTable dtbl = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
        if (dtbl.Rows.Count > 0)
        {
            PersonAlarmS p = new PersonAlarmS
            {
                Alarm = dtbl.Rows[0]["Alarm"].ToString(),
                AlarmDurationMins = dtbl.Rows[0]["AlarmDurationMins"].ToString(),
                SendSMS = dtbl.Rows[0]["SendSMS"].ToString()
            };
            Context.Response.Write(JsonConvert.SerializeObject(p));
        }
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetPersonDetail(string DeviceNo)
    {
        string Query = "select PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc,SrMobile,SendSmsSenior from PersonMaster where DeviceNo='" + DeviceNo + "'";
        DataTable dtbl = new DataTable();
        dtbl = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
        if (dtbl.Rows.Count > 0)
        {
            Person p = new Person
            {
                PersonName = dtbl.Rows[0]["PersonName"].ToString(),
                FromTime = dtbl.Rows[0]["FromTime"].ToString(),
                ToTime = dtbl.Rows[0]["ToTime"].ToString(),
                Interval = dtbl.Rows[0]["Interval"].ToString(),
                UploadInterval = dtbl.Rows[0]["UploadInterval"].ToString(),
                RetryInterval = dtbl.Rows[0]["RetryInterval"].ToString(),
                Degree = dtbl.Rows[0]["Degree"].ToString(),
                Sys_Flag = dtbl.Rows[0]["Sys_Flag"].ToString(),
                GpsLoc = dtbl.Rows[0]["GpsLoc"].ToString(),
                MobileLoc = dtbl.Rows[0]["MobileLoc"].ToString(),
                SrMobile = dtbl.Rows[0]["SrMobile"].ToString(),
                SrSMS = dtbl.Rows[0]["SendSmsSenior"].ToString()
            };
            //List<Person> d = new List<Person>();
            //d.Add(new Person { 

            //    PersonName = dtbl.Rows[0]["PersonName"].ToString(),
            //    FromTime = dtbl.Rows[0]["FromTime"].ToString(),
            //    ToTime = dtbl.Rows[0]["ToTime"].ToString(),
            //    Interval = dtbl.Rows[0]["Interval"].ToString(),
            //    UploadInterval = dtbl.Rows[0]["UploadInterval"].ToString(),
            //    RetryInterval = dtbl.Rows[0]["RetryInterval"].ToString(),
            //    Degree = dtbl.Rows[0]["Degree"].ToString(),
            //    Sys_Flag = dtbl.Rows[0]["Sys_Flag"].ToString(),
            //    GpsLoc = dtbl.Rows[0]["GpsLoc"].ToString(),
            //    MobileLoc = dtbl.Rows[0]["MobileLoc"].ToString()
            //});
            //    Persons Person = new Persons
            //    {
            //        PersonList = new List<Person>() {
            //new Person { PersonName = dtbl.Rows[0]["PersonName"].ToString(), FromTime = dtbl.Rows[0]["FromTime"].ToString() ,ToTime=dtbl.Rows[0]["ToTime"].ToString(),
            //    Interval=dtbl.Rows[0]["Interval"].ToString(),UploadInterval=dtbl.Rows[0]["UploadInterval"].ToString(),RetryInterval=dtbl.Rows[0]["RetryInterval"].ToString(),
            //    Degree=dtbl.Rows[0]["Degree"].ToString(),Sys_Flag=dtbl.Rows[0]["Sys_Flag"].ToString(),GpsLoc=dtbl.Rows[0]["GpsLoc"].ToString(),MobileLoc=dtbl.Rows[0]["MobileLoc"].ToString()}
            //     }
            //    };
            Context.Response.Write(JsonConvert.SerializeObject(p));
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetRegistrationDetail(string Name, string MobileNo, string Email, string Password, string DeviceID)
    {
        string Query = "select case(select 1 from Temp_Reg where deviceid='" + DeviceID + "') when 1 then 'Y' else 'N' end as DataInserted";
        string date11 = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("yyyy-MM-dd");

        string str = "INSERT INTO Temp_Reg (Name,Mobile,Email,Pwd,DeviceId,Created_Date) Values ('" + Name + "','" + MobileNo + "','" + Email + "','" + Password + "','" + DeviceID + "','" + Convert.ToDateTime(date11) + "')";
        string retval = DbConnectionDAL.GetStringScalarVal(str);
        if (retval != "-1")
        {
            SendEmail(Email, Name, DeviceID);
        }
        retval = DbConnectionDAL.GetStringScalarVal(Query);
        Result rst = new Result
        {
            ResultMsg = retval
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetPersonDetailCalledByLicense(string DeviceNo, string mobile)
    {
        string Query = "select id,PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc,SrMobile,SendSmsSenior,(select url from enviro) as url,(select MarkAtten_Txt from enviro) as MarkAttenTxt,(select LeaveModule from enviro) as LeaveModule,(select appleavebutton from enviro) as appleavebutton ,(select telecallerno from enviro) as telecallerno,(select isnull(IsStartEndImg,'N')  from mastenviro) IsStartEndImg from  PersonMaster  where DeviceNo='" + DeviceNo + "' and mobile='" + mobile + "' and active='Y'";
        DataTable dtbl = new DataTable();
        dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
        if (dtbl.Rows.Count > 0)
        {
            Person p = new Person
            {
                Pid = dtbl.Rows[0]["id"].ToString(),
                PersonName = dtbl.Rows[0]["PersonName"].ToString(),
                FromTime = dtbl.Rows[0]["FromTime"].ToString(),
                ToTime = dtbl.Rows[0]["ToTime"].ToString(),
                Interval = dtbl.Rows[0]["Interval"].ToString(),
                UploadInterval = dtbl.Rows[0]["UploadInterval"].ToString(),
                RetryInterval = dtbl.Rows[0]["RetryInterval"].ToString(),
                Degree = dtbl.Rows[0]["Degree"].ToString(),
                Sys_Flag = dtbl.Rows[0]["Sys_Flag"].ToString(),
                GpsLoc = dtbl.Rows[0]["GpsLoc"].ToString(),
                MobileLoc = dtbl.Rows[0]["MobileLoc"].ToString(),
                SrMobile = dtbl.Rows[0]["SrMobile"].ToString(),
                SrSMS = dtbl.Rows[0]["SendSmsSenior"].ToString(),
                Url = dtbl.Rows[0]["url"].ToString(),
                MarkAttendanceTxt = dtbl.Rows[0]["MarkAttenTxt"].ToString(),
                LeaveModule = dtbl.Rows[0]["appleavebutton"].ToString(),
                support_number = dtbl.Rows[0]["telecallerno"].ToString(),
                StartEndImg = dtbl.Rows[0]["IsStartEndImg"].ToString()
            };
            Context.Response.Write(JsonConvert.SerializeObject(p));
        }
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetValidUserInDemoDB(string DeviceNo)
    {
        string str = "select case when(select count(*) from Temp_Reg)  >  0 then (select case (select 1 from Temp_Reg WHERE DeviceId='" + DeviceNo + "') when 1 then 'Y' else 'N' end) else 'N' end As UserExist";
        string retval = DbConnectionDAL.GetStringScalarVal(str);
        Result rst = new Result
        {
            ResultMsg = retval
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertLogTracker(string DeviceNo, Int64 CurrentDate, string Status)
    {
        int retVal = 0;
        string Query = "";
        string retInfo = "Record Not Inserted";
        try
        {
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from Log_Tracker where CurrentDate='{0}' AND Status='{1}'", mDate, Status)) <= 0)
            {
                if (Status == "M")
                { Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))"; }
                else
                {
                    Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";
                }
                retVal = DbConnectionDAL.GetIntScalarVal(Query);
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            retInfo = "Record Inserted";
        Result rst = new Result
        {
            ResultMsg = retInfo
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertVersion(string version, Int64 CurrentDate, string DeviceNo)
    {
        int retVal = 0;
        string retInfo = "Version Not Inserted";
        try
        {
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            string strexist = "select 1 from Version_Mast where DeviceId ='" + DeviceNo + "'";
            int gtval = DbConnectionDAL.GetIntScalarVal(strexist);
            if (gtval > 0)
            {
                DbConnectionDAL.GetIntScalarVal("delete from Version_Mast where DeviceId ='" + DeviceNo + "'");
            }
            string Query = "insert into Version_Mast(Version,DeviceId,Created_Date)" +
                           "values ('" + version + "','" + DeviceNo + "','" + mDate + "')";

            retVal = DbConnectionDAL.GetIntScalarVal(Query);
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            retInfo = "Version Inserted";

        Result rst = new Result
        {
            ResultMsg = retInfo
        };

        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertVersion_v6_0_2(string version, Int64 CurrentDate, string DeviceNo, string mcomp, string model, string Reg_ID)
    {
        int retVal = 4;
        string Query = "";
        string retInfo = "Version Not Inserted";
        try
        {
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
            string val = DMT.DMTApp_updateRegId(Reg_ID, DeviceNo);
            if (!string.IsNullOrEmpty(val))
            //string Query = "Update LineMaster set Reg_ID='" + Reg_ID + "' where DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER'";
            // DbConnectionDAL.GetDemoLicenseIntScalarVal(Query);
            // Query = "select 1 from LineMaster where DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER'";
            // retVal = DbConnectionDAL.GetDemoLicenseIntScalarVal(Query);
            // if (retVal == 1)
            {
                string strexist = "select 1 from Version_Mast where DeviceId ='" + DeviceNo + "'";
                int gtval = DbConnectionDAL.GetIntScalarVal(strexist);
                if (gtval > 0)
                {
                    DbConnectionDAL.GetIntScalarVal("delete from Version_Mast where DeviceId ='" + DeviceNo + "'");
                }
                Query = "insert into Version_Mast(Version,DeviceId,Created_Date,model,company)" +
                              "values ('" + version + "','" + DeviceNo + "','" + mDate + "','" + model + "','" + mcomp + "')";

                retVal = DbConnectionDAL.GetIntScalarVal(Query);
            }

            else { retVal = -1; }
            if (retVal == 0)
            {
                retInfo = "Version Inserted";
            }
            else if (retVal == -1)
            { retInfo = "License Problem"; }
            else if (retVal == 1)
            { retInfo = "Rcl connection problem"; }
            else
            { retInfo = "xyz"; }
        }
        catch (Exception ex)
        {
            //retVal = 1;
            retInfo = ex.ToString();
        }
        Result rst = new Result
        {
            ResultMsg = retInfo
        };

        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertVersionWithModel(string version, Int64 CurrentDate, string DeviceNo, string Model, string Company)
    {
        int retVal = 0;
        string retInfo = "Version Not Inserted";
        try
        {
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            string strexist = "select 1 from Version_Mast where DeviceId ='" + DeviceNo + "'";
            int gtval = DbConnectionDAL.GetIntScalarVal(strexist);
            if (gtval > 0)
            {
                DbConnectionDAL.GetIntScalarVal("delete from Version_Mast where DeviceId ='" + DeviceNo + "'");
            }
            string Query = "insert into Version_Mast(Version,DeviceId,Created_Date,Model,Company)" +
                           "values ('" + version + "','" + DeviceNo + "','" + mDate + "','" + Model + "','" + Company + "')";

            retVal = DbConnectionDAL.GetIntScalarVal(Query);
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            retInfo = "Version Inserted";

        Result rst = new Result
        {
            ResultMsg = retInfo
        };

        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JReSendActivationMail(String DeviceNo)
    {
        string status = "";
        try
        {
            string str = "select Name,Email from Temp_Reg Where DeviceID='" + DeviceNo + "'";
            DataTable dtbl = DbConnectionDAL.GetDataTable(CommandType.Text,str);
            if (dtbl.Rows.Count > 0)
            {
                SendEmail(dtbl.Rows[0]["Email"].ToString(), dtbl.Rows[0]["Name"].ToString(), DeviceNo);
                status = "Re-Activation Mail Sent";
                string createText = DeviceNo + "," + dtbl.Rows[0]["Email"].ToString() + "," + dtbl.Rows[0]["Name"].ToString() + Environment.NewLine;

                using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile2.txt"), true))
                {
                    file2.WriteLine(createText);
                    file2.Close();
                }
            }
        }
        catch (Exception ex) { status = "Re-Activation Mail Not Sent"; }
        Result rst = new Result { ResultMsg = status }
            ;
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGPD(string DeviceNo)
    {
        string msg = "N";
        string Query = "select Id as PersonID from PersonMaster where DeviceNo='" + DeviceNo + "'";
        string Pid = DbConnectionDAL.GetStringScalarVal(Query);
        if (!string.IsNullOrEmpty(Pid))
            msg = Pid;
        Result rst = new Result
        {
            ResultMsg = msg
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetTimeStamp()
    {
        //since epoch used - no need to add GMT hours
        long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        Result rst = new Result { ResultMsg = epoch.ToString() };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertFenceAddress(Int64 PersonID, string latitude, string longitude, string PartyName)
    {
        int retVal = 0;
        string msg = "N";
        DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text,"select GroupId from GrpMapp where personid=" + PersonID + "");
        for (int i = 0; i < dtgrps.Rows.Count; i++)
        {
            string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
            retVal = DbConnectionDAL.GetIntScalarVal(str);
        }

        if (retVal == 0)
            msg = "Y";
        Result rst = new Result
        {
            ResultMsg = msg
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertFenceAddress_New(string DeviceNo, string latitude, string longitude, string PartyName)
    {
        int retVal = 0;
        string msg = "N";
        string PersonID = DbConnectionDAL.GetStringScalarVal("select Id from Person master where deviceno='" + DeviceNo + "'");
        DataTable dtgrps = DbConnectionDAL.GetDataTable(CommandType.Text,"select GroupId from GrpMapp where personid=" + PersonID + "");
        for (int i = 0; i < dtgrps.Rows.Count; i++)
        {
            string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
            retVal = DbConnectionDAL.GetIntScalarVal(str);
        }

        if (retVal == 0)
            msg = "Y";
        Result rst = new Result
        {
            ResultMsg = msg
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertMobileLog(string DeviceNo, Int64 CurrentDate, string Status, Int64 FromTime, Int64 ToTime)
    {
        int retVal = 0;
        string retInfo = "Record Not Inserted";
        try
        {
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            DateTime FDate = DateTime.Parse("1970-01-01");
            FDate = FDate.AddSeconds(FromTime + 19800);
            DateTime TDate = DateTime.Parse("1970-01-01");
            TDate = TDate.AddSeconds(ToTime + 19800);

            string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

            retVal = DbConnectionDAL.GetIntScalarVal(Query);
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            retInfo = "Record Inserted";
        Result rst = new Result
        {
            ResultMsg = retInfo
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JUpdatePhoneType(string DeviceNo, string PhoneType)
    {
        int retVal = 0;
        string retInfo = "Record Not Inserted";
        try
        {
            string Query = "update personmaster set PhoneType='" + PhoneType + "' where DeviceNo='" + DeviceNo + "'";
            retVal = DbConnectionDAL.GetIntScalarVal(Query);
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            retInfo = "Record Inserted";
        Result rst = new Result
        {
            ResultMsg = retInfo
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendMszToL10AndSeniors()
    {
        // SMSAdapter sms = new SMSAdapter();

        int retVal = 0; string chksql = ""; string response = "";
        string api = "http://zapsms.co.in/vendorsms/pushsms.aspx?user=rclknp&password=rcl15785&msisdn={0}&sid=RCLKNP&msg={1}&fl=0&gwid=2";
        int mszcount = 0;
        try
        {
            string Query = "select DeviceNo,Mobile,SrMobile,PersonName,active from personmaster WITH (NOLOCK) where active='Y' and personname like '%10%'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
            if (dt.Rows.Count > 0)
            {
                string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["active"].ToString() == "Y")
                    {
                        chksql = "select count(*) from  locationdetails WITH (NOLOCK) where currentdate>='" + date + " 09:00:00.000' and currentdate<='" + date + " 10:30:00.000' and deviceno='" + dt.Rows[i]["DeviceNo"].ToString() + "'";
                        int count = DbConnectionDAL.GetIntScalarVal(chksql);
                        if (count == 0)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["mobile"])))
                            {
                                if (dt.Rows[i]["PersonName"].ToString().Contains("L30"))
                                {
                                    Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["mobile"].ToString() + "','Dear " + dt.Rows[i]["PersonName"].ToString() + ", kindly start your visit on DMT-App','" + response + "','SMS To L30')";
                                    DbConnectionDAL.ExecuteQuery(Query);
                                }
                                else
                                {
                                    response = sms.sendSmsbyTableWithResponse(api, dt.Rows[i]["mobile"].ToString(), "Dear " + dt.Rows[i]["PersonName"].ToString() + ", kindly start your visit on DMT-App");
                                    Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["mobile"].ToString() + "','Dear " + dt.Rows[i]["PersonName"].ToString() + ", kindly start your visit on DMT-App','" + response + "','Attendance Not Marked')";
                                    DbConnectionDAL.ExecuteQuery(Query);
                                }


                                mszcount = mszcount + 1;
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SrMobile"])))
                            {
                                if (dt.Rows[i]["PersonName"].ToString().Contains("L30"))
                                {
                                    Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["SrMobile"].ToString() + "','Dear " + dt.Rows[i]["PersonName"].ToString() + ", kindly start your visit on DMT-App','" + response + "','SMS To L30')";
                                    DbConnectionDAL.ExecuteQuery(Query);
                                }
                                else
                                {
                                    response = sms.sendSmsbyTableWithResponse(api, dt.Rows[i]["SrMobile"].ToString(), "Dear Sir, Mr. " + dt.Rows[i]["PersonName"].ToString() + ", still have not started his visit today.");
                                    Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["SrMobile"].ToString() + "','Dear Sir, Mr.  " + dt.Rows[i]["PersonName"].ToString() + ", still have not started his visit today.','" + response + "','Attendance Not Marked')";
                                    DbConnectionDAL.ExecuteQuery(Query);
                                    mszcount = mszcount + 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        Context.Response.Write(JsonConvert.SerializeObject("Message sent to " + mszcount + " users"));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JGetPersonDetailCalledByLicenseGfft(string DeviceNo, string mobile)
    {
        string Query = "select PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc,SrMobile,SendSmsSenior,(select compurl from mastenviro) as url,(select MarkAtten_Txt from enviro) as MarkAttenTxt,(select LeaveModule from enviro) as LeaveModule from PersonMaster where DeviceNo='" + DeviceNo + "' and mobile='" + mobile + "' and active='Y'";
        DataTable dtbl = new DataTable();
        dtbl = DbConnectionDAL.GetDataTable(CommandType.Text,Query); //String[] Address = { }; 
        string Address = "";
        if (dtbl.Rows.Count > 0)
        {
            Address = dtbl.Rows[0]["url"].ToString();
            string url = Address;
            if (!url.Contains("http"))
            {
                url = "http://" + url;
            }
            Person p = new Person
            {

                PersonName = dtbl.Rows[0]["PersonName"].ToString(),
                FromTime = dtbl.Rows[0]["FromTime"].ToString(),
                ToTime = dtbl.Rows[0]["ToTime"].ToString(),
                Interval = dtbl.Rows[0]["Interval"].ToString(),
                UploadInterval = dtbl.Rows[0]["UploadInterval"].ToString(),
                RetryInterval = dtbl.Rows[0]["RetryInterval"].ToString(),
                Degree = dtbl.Rows[0]["Degree"].ToString(),
                Sys_Flag = dtbl.Rows[0]["Sys_Flag"].ToString(),
                GpsLoc = dtbl.Rows[0]["GpsLoc"].ToString(),
                MobileLoc = dtbl.Rows[0]["MobileLoc"].ToString(),
                SrMobile = dtbl.Rows[0]["SrMobile"].ToString(),
                SrSMS = dtbl.Rows[0]["SendSmsSenior"].ToString(),
                Url = url,
                MarkAttendanceTxt = dtbl.Rows[0]["MarkAttenTxt"].ToString(),
                LeaveModule = dtbl.Rows[0]["LeaveModule"].ToString()
            };
            Context.Response.Write(JsonConvert.SerializeObject(p));
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendMszToL10AndSuperSeniors()
    {
        // SMSAdapter sms = new SMSAdapter();

        int retVal = 0; string chksql = ""; string response = "";
        string api = "http://zapsms.co.in/vendorsms/pushsms.aspx?user=rclknp&password=rcl15785&msisdn={0}&sid=RCLKNP&msg={1}&fl=0&gwid=2";
        int mszcount = 0;
        try
        {
            string Query = "select pm1.deviceno,pm1.PersonName L10,pm1.Mobile as l10mob,pm1.active as L10Active,pm2.PersonName L20,pm2.Mobile as l20mob,pm3.PersonName L30,pm3.Mobile as l30mob,pm4.PersonName L40,pm4.Mobile as l40mob from PersonMaster pm1 left join PersonMaster pm2 on pm1.SrMobile=pm2.Mobile left join PersonMaster pm3 on pm2.SrMobile=pm3.Mobile left join PersonMaster pm4 on pm3.SrMobile=pm4.Mobile where pm1.Active='Y' and pm1.PersonName like '%10%'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
            if (dt.Rows.Count > 0)
            {
                string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["L10Active"].ToString() == "Y")
                    {
                        chksql = "select count(*) from  locationdetails WITH (NOLOCK) where currentdate>='" + date + " 09:00:00.000' and currentdate<='" + date + " 11:05:00.000' and deviceno='" + dt.Rows[i]["DeviceNo"].ToString() + "'";
                        int count = DbConnectionDAL.GetIntScalarVal(chksql);
                        if (count == 0)
                        {

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["L30"])))
                            {
                                if (dt.Rows[i]["L30"].ToString().Contains("30"))
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["l30mob"])))
                                    {
                                        if (dt.Rows[i]["L10"].ToString().Contains("L30"))
                                        {
                                            Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l30mob"].ToString() + "','Dear " + dt.Rows[i]["L10"].ToString() + ", kindly start your visit on DMT-App','" + response + "','SMS To L30')";
                                            DbConnectionDAL.ExecuteQuery(Query);
                                        }
                                        else
                                        {
                                            response = sms.sendSmsbyTableWithResponse(api, dt.Rows[i]["l30mob"].ToString(), "Dear Sir, Mr. " + dt.Rows[i]["L10"].ToString() + ", still have not started his visit today.");
                                            Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l30mob"].ToString() + "','Dear Sir, Mr.  " + dt.Rows[i]["L10"].ToString() + ", still have not started his visit today.','" + response + "','Attendance Not Marked')";
                                            DbConnectionDAL.ExecuteQuery(Query);
                                            mszcount = mszcount + 1;
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["L40"])))
                            {
                                if (dt.Rows[i]["L40"].ToString().Contains("40"))
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["l40mob"])))
                                    {
                                        if (dt.Rows[i]["L10"].ToString().Contains("L30"))
                                        {
                                            Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l40mob"].ToString() + "','Dear " + dt.Rows[i]["L10"].ToString() + ", kindly start your visit on DMT-App','" + response + "','SMS To L30')";
                                            DbConnectionDAL.ExecuteQuery(Query);
                                        }
                                        else
                                        {
                                            response = sms.sendSmsbyTableWithResponse(api, dt.Rows[i]["l40mob"].ToString(), "Dear Sir, Mr. " + dt.Rows[i]["L10"].ToString() + ", still have not started his visit today.");
                                            Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l40mob"].ToString() + "','Dear Sir, Mr.  " + dt.Rows[i]["L10"].ToString() + ", still have not started his visit today.','" + response + "','Attendance Not Marked')";
                                            DbConnectionDAL.ExecuteQuery(Query);
                                            mszcount = mszcount + 1;
                                        }
                                    }
                                }
                            }



                        }

                    }


                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        Context.Response.Write(JsonConvert.SerializeObject("Message sent to " + mszcount + " users"));
    }




    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendMszToL20AndSeniorsNew()
    {
        // SMSAdapter sms = new SMSAdapter();

        int retVal = 0; string chksql = ""; string response = "";
        string api = "http://zapsms.co.in/vendorsms/pushsms.aspx?user=rclknp&password=rcl15785&msisdn={0}&sid=RCLKNP&msg={1}&fl=0&gwid=2";
        int mszcount = 0;
        try
        {
            //string Query = "select DeviceNo,Mobile,SrMobile,PersonName from personmaster WITH (NOLOCK) where active='Y'  and personname like '%20%'";
            string Query = "select pm1.deviceno,pm1.PersonName L10,pm1.Mobile as l10mob,pm1.active L10Active,pm2.PersonName L20,pm2.Mobile as l20mob,pm3.PersonName L30,pm3.Mobile as l30mob,pm4.PersonName L40,pm4.Mobile as l40mob from PersonMaster pm1 left join PersonMaster pm2 on pm1.SrMobile=pm2.Mobile left join PersonMaster pm3 on pm2.SrMobile=pm3.Mobile left join PersonMaster pm4 on pm3.SrMobile=pm4.Mobile where pm1.Active='Y' and pm1.PersonName like '%20%'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
            if (dt.Rows.Count > 0)
            {
                string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["L10Active"].ToString() == "Y")
                    {
                        if (dt.Rows[i]["l10"].ToString().Contains("20"))
                        {
                            chksql = "select count(*) from locationdetails WITH (NOLOCK) where currentdate>='" + date + " 09:00:00.000' and currentdate<='" + date + " 11:10:00.000' and deviceno='" + dt.Rows[i]["deviceno"].ToString() + "'";
                            int count = DbConnectionDAL.GetIntScalarVal(chksql);
                            if (count == 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["l10mob"])))
                                {
                                    if (dt.Rows[i]["L10"].ToString().Contains("L30"))
                                    {
                                        Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l10mob"].ToString() + "','Dear " + dt.Rows[i]["L10"].ToString() + ", kindly start your visit on DMT-App','" + response + "','SMS To L30')";
                                        DbConnectionDAL.ExecuteQuery(Query);
                                    }
                                    else
                                    {
                                        response = sms.sendSmsbyTableWithResponse(api, dt.Rows[i]["l10mob"].ToString(), "Dear " + dt.Rows[i]["L10"].ToString() + ", kindly start your visit on DMT-App");
                                        Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l10mob"].ToString() + "','Dear " + dt.Rows[i]["L10"].ToString() + ", kindly start your visit on DMT-App','" + response + "','Attendance Not Marked')";
                                        DbConnectionDAL.ExecuteQuery(Query);
                                        mszcount = mszcount + 1;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["l20mob"])))
                                {
                                    if (dt.Rows[i]["L10"].ToString().Contains("L30"))
                                    {
                                        Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l20mob"].ToString() + "','Dear " + dt.Rows[i]["L10"].ToString() + ", kindly start your visit on DMT-App','" + response + "','SMS To L30')";
                                        DbConnectionDAL.ExecuteQuery(Query);
                                    }
                                    else
                                    {
                                        response = sms.sendSmsbyTableWithResponse(api, dt.Rows[i]["l20mob"].ToString(), "Dear Sir, Mr. " + dt.Rows[i]["l10"].ToString() + ", still have not started his visit today.");
                                        Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,Reason) values('" + dt.Rows[i]["l20mob"].ToString() + "','Dear Sir, Mr.  " + dt.Rows[i]["l10"].ToString() + " ,still have not started his visit today.','" + response + "','Attendance Not Marked')";
                                        DbConnectionDAL.ExecuteQuery(Query);
                                        mszcount = mszcount + 1;
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["l30mob"])))
                                {
                                    if (Convert.ToString(dt.Rows[i]["l30"]).Contains("40"))
                                    {
                                        if (dt.Rows[i]["L10"].ToString().Contains("L30"))
                                        {
                                            Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,reason) values('" + dt.Rows[i]["l30mob"].ToString() + "','Dear " + dt.Rows[i]["L10"].ToString() + ", kindly start your visit on DMT-App','" + response + "','SMS To L30')";
                                            DbConnectionDAL.ExecuteQuery(Query);
                                        }
                                        else
                                        {
                                            response = sms.sendSmsbyTableWithResponse(api, dt.Rows[i]["l30mob"].ToString(), "Dear Sir, Mr. " + dt.Rows[i]["l10"].ToString() + ", still have not started his visit today.");
                                            Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,Reason) values('" + dt.Rows[i]["l30mob"].ToString() + "','Dear Sir, Mr.  " + dt.Rows[i]["l10"].ToString() + " ,still have not started his visit today.','" + response + "','Attendance Not Marked')";
                                            DbConnectionDAL.ExecuteQuery(Query);
                                            mszcount = mszcount + 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }

        Context.Response.Write(JsonConvert.SerializeObject("Message sent to " + mszcount + " users"));
    }
    private class NotificationMessage
    {
        public string Title;
        public string Message;
        public string DeviceNo;
        public string licenceinfo;
        public string PersonName;
        public string FromTime;
        public string ToTime;
        public string Interval;
        public string UploadInterval;
        public string RetryInterval;
        public string Degree;
        public string GpsLoc;
        public string Sys_Flag;
        public string MobileLoc;
        public string StartService;
        public string SrMobile;
        public string SendSmsSr;
        // public string PushId;
    }

    private class HeartNotificationMessage
    {
        public string Title;
        public string Message;
        public string DeviceNo;
        public string licenceinfo;
        public string PersonName;
        public string FromTime;
        public string ToTime;
        public string Interval;
        public string UploadInterval;
        public string RetryInterval;
        public string Degree;
        public string GpsLoc;
        public string Sys_Flag;
        public string MobileLoc;
        public string StartService;
        public string SrMobile;
        public string SendSmsSr;
        public string PushId;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendSmsToSeniorsFor30MinStoppage()
    {
        int message = 0; string response = "";
        try
        {
            string description = ""; string lat = ""; string longi = "";
            //LocationInsert_Schedulear_V6_1();

            string sql = "select deviceno,mobile,personname,srmobile from personmaster where active='Y' and len(deviceno)>12";
            DataTable dtalluser = DbConnectionDAL.GetDataTable(CommandType.Text,sql);
            if (dtalluser.Rows.Count > 0)
            {
                for (int r = 0; r < dtalluser.Rows.Count; r++)
                {
                    Common cs = new Common();
                    string PersonDeviceNo = string.Empty;
                    string addqry = ""; DataTable dtbl = new DataTable(); DataTable dtbl1 = new DataTable();
                    addqry = " and DeviceNo='" + dtalluser.Rows[r]["deviceno"].ToString() + "'";
                    string todaydate = DateTime.Today.ToString("yyyy-MM-dd");
                    string ftime = "09:00";
                    string Ttime = "11:59";
                    string qry1 = "select SUBSTRING(latitude, 0, 7) as latitude,SUBSTRING(longitude, 0, 7) longitude,CurrentDate,description from LocationDetails where 1=1 " + addqry + "  and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + todaydate + " " + ftime + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + todaydate + " " + Ttime + "' as DateTime) and latitude!='' and longitude!='' order by CurrentDate";
                    dtbl1 = DbConnectionDAL.GetDataTable(CommandType.Text,qry1);
                    if (dtbl1.Rows.Count > 0)
                    {
                        DataTable newtbl = new DataTable();
                        newtbl.Columns.Add("FromDate");
                        newtbl.Columns.Add("ToDate");
                        newtbl.Columns.Add("description");
                        newtbl.Columns.Add("latitude");
                        newtbl.Columns.Add("longitude");
                        newtbl.Columns.Add("count");
                        for (int i = 0; i < dtbl1.Rows.Count; i++)
                        {

                            if (i == 0)
                            {
                                DataRow dr = newtbl.NewRow();
                                dr["FromDate"] = Convert.ToDateTime(dtbl1.Rows[i]["CurrentDate"]).ToString("dd-MMM-yyyy");
                                dr["ToDate"] = dtbl1.Rows[i]["CurrentDate"].ToString();
                                dr["count"] = 0;
                                dr["latitude"] = dtbl1.Rows[i]["latitude"].ToString();
                                dr["longitude"] = dtbl1.Rows[i]["longitude"].ToString();
                                dr["description"] = dtbl1.Rows[i]["description"].ToString();
                                newtbl.Rows.Add(dr);
                                newtbl.AcceptChanges();
                            }
                            else
                            {
                                bool hasitem = false;
                                //string ss = dtbl1.Rows[i]["latitude"].ToString();
                                //string dd = dtbl1.Rows[i]["longitude"].ToString();
                                for (int M = 0; M < newtbl.Rows.Count; M++)
                                {
                                    if (dtbl1.Rows[i]["latitude"].ToString() == newtbl.Rows[M]["latitude"].ToString() && dtbl1.Rows[i]["longitude"].ToString() == newtbl.Rows[M]["longitude"].ToString() || dtbl1.Rows[i]["description"].ToString() == newtbl.Rows[M]["description"].ToString())
                                    {
                                        description = dtbl1.Rows[i]["description"].ToString();
                                        lat = dtbl1.Rows[i]["latitude"].ToString();
                                        longi = dtbl1.Rows[i]["longitude"].ToString();
                                        if (dtbl1.Rows[i]["latitude"].ToString() == dtbl1.Rows[i - 1]["latitude"].ToString() && dtbl1.Rows[i]["longitude"].ToString() == dtbl1.Rows[i - 1]["longitude"].ToString() || dtbl1.Rows[i]["description"].ToString() == dtbl1.Rows[i - 1]["description"].ToString())
                                        {
                                            DateTime myDate1 = Convert.ToDateTime(dtbl1.Rows[i - 1]["currentdate"].ToString());
                                            DateTime myDate2 = Convert.ToDateTime(dtbl1.Rows[i]["currentdate"].ToString());
                                            TimeSpan difference = myDate2.Subtract(myDate1);

                                            double totalMinutes = difference.TotalMinutes;

                                            newtbl.Rows[newtbl.Rows.Count - 1]["count"] = Convert.ToInt16(newtbl.Rows[newtbl.Rows.Count - 1]["count"]) + Math.Round(totalMinutes);
                                            // newtbl.Rows[newtbl.Rows.Count - 1]["count"] = Convert.ToInt16(newtbl.Rows[newtbl.Rows.Count - 1]["count"]) + 1;
                                            newtbl.Rows[newtbl.Rows.Count - 1]["ToDate"] = dtbl1.Rows[i]["CurrentDate"];
                                            newtbl.Rows[newtbl.Rows.Count - 1]["description"] = dtbl1.Rows[i]["description"];
                                            newtbl.AcceptChanges();
                                            hasitem = true;
                                            break;
                                        }
                                    }
                                }
                                if (!hasitem)
                                {
                                    DataRow dr = newtbl.NewRow();
                                    dr["FromDate"] = dtbl1.Rows[i]["CurrentDate"].ToString();
                                    dr["ToDate"] = dtbl1.Rows[i]["CurrentDate"].ToString(); ;
                                    dr["count"] = 1;
                                    dr["latitude"] = dtbl1.Rows[i]["latitude"].ToString();
                                    dr["longitude"] = dtbl1.Rows[i]["longitude"].ToString();
                                    dr["description"] = dtbl1.Rows[i]["description"].ToString();
                                    newtbl.Rows.Add(dr);
                                    newtbl.AcceptChanges();

                                }
                            }
                        }

                        DataView dv = new DataView(newtbl);

                        dv.RowFilter = "count >30";


                        foreach (DataRowView drV in dv)
                        {
                            string sValue = drV["count"].ToString();
                            description = drV["description"].ToString();
                            lat = drV["latitude"].ToString();
                            longi = drV["longitude"].ToString();
                            string Query = "";

                            if (description.Contains("Updated Soon"))
                            {
                                AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                string Address = DMT.InsertAddress(lat, longi);
                                description = Address;
                            }

                            if ((!string.IsNullOrEmpty(description)) || description != "Updated Soon..")
                            {

                                Query = "select count (*) from SentSmsToUsers where tomobile='" + dtalluser.Rows[r]["SrMobile"].ToString() + "' and SMS='Dear Sir, Mr.  " + dtalluser.Rows[r]["personname"].ToString() + " , is at " + description + " for " + sValue + " Minutes' and cast (createddate as date)=cast(getdate() as date)";
                                int count = DbConnectionDAL.GetIntScalarVal(Query);
                                if (count == 0)
                                {
                                    string api = "http://zapsms.co.in/vendorsms/pushsms.aspx?user=rclknp&password=rcl15785&msisdn={0}&sid=RCLKNP&msg={1}&fl=0&gwid=2";
                                    response = sms.sendSmsbyTableWithResponse(api, dtalluser.Rows[r]["SrMobile"].ToString(), "Dear Sir, Mr. " + dtalluser.Rows[r]["personname"].ToString() + ", is at " + description + " for " + sValue + " Minutes");
                                    Query = "insert into SentSmsToUsers(ToMobile,SMS,Response,Reason) values('" + dtalluser.Rows[r]["SrMobile"].ToString() + "','Dear Sir, Mr.  " + dtalluser.Rows[r]["personname"].ToString() + " , is at " + description + " for " + sValue + " Minutes','" + response + "','30 Min Stoppage')";
                                    DbConnectionDAL.ExecuteQuery(Query);
                                    message = message + 1;
                                }
                            }

                            sValue = null;
                            description = null;
                        }

                    }


                }
            }
        }
        catch (Exception)
        {

        }
        Context.Response.Write(JsonConvert.SerializeObject(message));

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JSPushNotificationForSchedulear()
    {

        string val = "Service is running";
        string retInfo = "Y";
        string Query = "select lastactive.DeviceNo,personmaster.PersonName as Personname,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc,SrMobile,SendSmsSenior ,convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate,CONVERT(DATETIME,CONVERT(VARCHAR(10), GETDATE(), 112))+[FromTime] as Fromtime,CONVERT(DATETIME,CONVERT(VARCHAR(10), GETDATE(), 112))+[ToTime]as ToTime, getdate() as Currdt,LastActive.CurrentDate AS DDate from LastActive left join PersonMaster on LastActive.DeviceNo = PersonMaster.DeviceNo WHERE Active IS NULL and PersonMaster.SendPushNotification='Y' order by lastactive.currentdate desc";
        DataTable dtbl = new DataTable();
        dtbl = DbConnectionDAL.GetDataTable(CommandType.Text,Query);
        if (dtbl.Rows.Count > 0)
        {
            try
            {
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    int retVal = 0;
                    DateTime startdate = new DateTime();
                    DateTime Enddate = new DateTime();
                    startdate = DateTime.ParseExact(DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                    Enddate = DateTime.ParseExact(Convert.ToDateTime(dtbl.Rows[i]["DDate"]).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                    TimeSpan diff = (startdate - Enddate);
                    double MINS = diff.TotalMinutes;
                    if (MINS >= 40)
                    {
                        string DeviceNo = dtbl.Rows[i]["Deviceno"].ToString();
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            //string regid_query = "select Reg_id from LineMaster where deviceid='" + DeviceNo + "' and Upper(Product)='TRACKER'";
                            //string constrDmLicense = "data source=103.13.99.131; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
                            //string Query1 = "select case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER')" +
                            //" when 1 THEN (select case when( select URL from LineMaster lm WHERE lm.DeviceId='" + DeviceNo + "' and Upper(lm.Product)='TRACKER')='demotracker.dataman.in'" +
                            //" THEN (SELECT CASE (SELECT 1 FROM LineMaster where  DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER' AND Validdate < dateadd(ss,19800,getutcdate())) WHEN 1 THEN 'N' ELSE 'Y' END )" +
                            //"ELSE (SELECT CASE(SELECT 1 FROM LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER' AND Active ='Y')" +
                            //" WHEN 1 THEN 'Y' ELSE 'N' END )END ) ELSE 'N' END ";
                            //string us = DbConnectionDAL.GetStringScalarVal(Query1);
                            //SqlConnection cn = new SqlConnection(constrDmLicense);
                            //SqlCommand cmd = new SqlCommand(regid_query, cn);
                            //SqlCommand cmd1 = new SqlCommand(Query1, cn);
                            //cmd.CommandType = CommandType.Text;
                            //cmd1.CommandType = CommandType.Text;
                            //cn.Open();
                            //string regId = cmd.ExecuteScalar() as string;
                            //string licenceinfo = cmd1.ExecuteScalar().ToString();
                            //cn.Close();
                            //cmd = null;
                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            string regId = DMT.DMTApp_GetRegistrationId(DeviceNo);
                            string licenceinfo = DMT.DMTApp_GetLicenseInfo(DeviceNo);

                            if (!string.IsNullOrEmpty(regId))
                            {


                                string appid = DbConnectionDAL.GetStringScalarVal("select serverkey from enviro ");
                                string senderid = DbConnectionDAL.GetStringScalarVal("select Senderid from enviro ");
                                NotificationMessage nm = new NotificationMessage();
                                nm.Title = "Testing";
                                nm.Message = val;
                                nm.DeviceNo = DeviceNo;
                                nm.licenceinfo = licenceinfo;
                                nm.PersonName = dtbl.Rows[0]["PersonName"].ToString();
                                nm.FromTime = dtbl.Rows[0]["FromTime"].ToString();
                                nm.ToTime = dtbl.Rows[0]["ToTime"].ToString();
                                nm.Interval = dtbl.Rows[0]["Interval"].ToString();
                                nm.UploadInterval = dtbl.Rows[0]["UploadInterval"].ToString();
                                nm.RetryInterval = dtbl.Rows[0]["RetryInterval"].ToString();
                                nm.Degree = dtbl.Rows[0]["Degree"].ToString();
                                nm.GpsLoc = dtbl.Rows[0]["GpsLoc"].ToString();
                                nm.Sys_Flag = dtbl.Rows[0]["Sys_Flag"].ToString();
                                nm.MobileLoc = dtbl.Rows[0]["MobileLoc"].ToString();
                                nm.StartService = "Y";
                                nm.SrMobile = dtbl.Rows[0]["SrMobile"].ToString();
                                nm.SendSmsSr = dtbl.Rows[0]["SendSmsSenior"].ToString();
                                //nm.PushId = Id;
                                var value = new JavaScriptSerializer().Serialize(nm);

                                WebRequest tRequest;

                                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");

                                tRequest.Method = "post";
                                tRequest.ContentType = "application/json";
                                tRequest.Headers.Add(string.Format("Authorization: key={0}", appid));

                                tRequest.Headers.Add(string.Format("Sender: id={0}", senderid));
                                //Data post to server  

                                string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + value + ",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + regId + "\"]}";

                                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                                tRequest.ContentLength = byteArray.Length;

                                Stream dataStream = tRequest.GetRequestStream();

                                dataStream.Write(byteArray, 0, byteArray.Length);

                                dataStream.Close();

                                WebResponse tResponse = tRequest.GetResponse();

                                dataStream = tResponse.GetResponseStream();

                                StreamReader tReader = new StreamReader(dataStream);

                                String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.
                                //Label1.Text = sResponseFromServer;      //Assigning GCM response to Label text 

                                string insert = "insert into PushNotification(DeviceId,Created_Date)" +
                                "values ('" + DeviceNo + "','" + startdate + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(insert);
                                tReader.Close();
                                dataStream.Close();
                                tResponse.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //retInfo = "N";
                retInfo = ex.ToString();
            }
        }
        Result rst = new Result
        {
            ResultMsg = retInfo
        };

        Context.Response.Write(JsonConvert.SerializeObject(rst));

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JSPushHeartCheckForScheduler()
    {

        string val = "Service is running";
        string retInfo = "Y";
        string Query = "select personmaster.DeviceNo,personmaster.PersonName as Personname,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc,SrMobile,SendSmsSenior ,CONVERT(DATETIME,CONVERT(VARCHAR(10), GETDATE(), 112))+[FromTime] as Fromtime,CONVERT(DATETIME,CONVERT(VARCHAR(10), GETDATE(), 112))+[ToTime]as ToTime, getdate() as Currdt from  PersonMaster WHERE Active ='Y' and PersonMaster.SendPushNotification='Y' ";
        DataTable dtbl = new DataTable();
        dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
        if (dtbl.Rows.Count > 0)
        {
            try
            {
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    int retVal = 0;
                    DateTime startdate = new DateTime();
                    startdate = DateTime.ParseExact(DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                    {
                        string DeviceNo = dtbl.Rows[i]["Deviceno"].ToString();
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {

                            AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            string regId = DMT.DMTApp_GetRegistrationId(DeviceNo);
                            string licenceinfo = DMT.DMTApp_GetLicenseInfo(DeviceNo);
                            if (!string.IsNullOrEmpty(regId))
                            {
                                string Query1 = "insert into PushHeartChk(DeviceNo,Task,FlagByweb,webDatetime) output inserted.id " +
                                         "values ('" + DeviceNo + "','Dmt HeartCheck Scheduler','Y',getdate())";
                                string Id = DbConnectionDAL.GetStringScalarVal(Query1);
                                if (string.IsNullOrEmpty(Id))
                                {
                                    Id = "0";
                                }
                                string appid = DbConnectionDAL.GetStringScalarVal("select serverkey from enviro ");
                                string senderid = DbConnectionDAL.GetStringScalarVal("select Senderid from enviro ");
                                HeartNotificationMessage nm = new HeartNotificationMessage();
                                nm.Title = "Testing";
                                nm.Message = val;
                                nm.DeviceNo = DeviceNo;
                                nm.licenceinfo = licenceinfo;
                                nm.PersonName = dtbl.Rows[0]["PersonName"].ToString();
                                nm.FromTime = dtbl.Rows[0]["FromTime"].ToString();
                                nm.ToTime = dtbl.Rows[0]["ToTime"].ToString();
                                nm.Interval = dtbl.Rows[0]["Interval"].ToString();
                                nm.UploadInterval = dtbl.Rows[0]["UploadInterval"].ToString();
                                nm.RetryInterval = dtbl.Rows[0]["RetryInterval"].ToString();
                                nm.Degree = dtbl.Rows[0]["Degree"].ToString();
                                nm.GpsLoc = dtbl.Rows[0]["GpsLoc"].ToString();
                                nm.Sys_Flag = dtbl.Rows[0]["Sys_Flag"].ToString();
                                nm.MobileLoc = dtbl.Rows[0]["MobileLoc"].ToString();
                                nm.StartService = "Y";
                                nm.SrMobile = dtbl.Rows[0]["SrMobile"].ToString();
                                nm.SendSmsSr = dtbl.Rows[0]["SendSmsSenior"].ToString();
                                nm.PushId = Id;
                                var value = new JavaScriptSerializer().Serialize(nm);

                                WebRequest tRequest;

                                // tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                                tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                                //WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                                //tRequest.Method = "post";  

                                tRequest.Method = "post";
                                tRequest.ContentType = "application/json";
                                tRequest.Headers.Add(string.Format("Authorization: key={0}", appid));

                                tRequest.Headers.Add(string.Format("Sender: id={0}", senderid));
                                //Data post to server  

                                string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + value + ",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + regId + "\"]}";

                                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                                tRequest.ContentLength = byteArray.Length;

                                Stream dataStream = tRequest.GetRequestStream();

                                dataStream.Write(byteArray, 0, byteArray.Length);

                                dataStream.Close();

                                WebResponse tResponse = tRequest.GetResponse();

                                dataStream = tResponse.GetResponseStream();

                                StreamReader tReader = new StreamReader(dataStream);

                                String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.
                                //Label1.Text = sResponseFromServer;      //Assigning GCM response to Label text 

                                string insert = "insert into PushNotification(DeviceId,Created_Date)" +
                                "values ('" + DeviceNo + "','" + startdate + "')";
                                retVal = DbConnectionDAL.GetIntScalarVal(insert);
                                tReader.Close();
                                dataStream.Close();
                                tResponse.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //retInfo = "N";
                retInfo = ex.ToString();
            }
        }
        Result rst = new Result
        {
            ResultMsg = retInfo
        };

        Context.Response.Write(JsonConvert.SerializeObject(rst));

    }

    public class ModelInsertLeave
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<ModelDataInsertLeave> Data { get; set; }
    }
    public class ModelDataInsertLeave
    {
        public string result { get; set; }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLeave(string deviceno, string mobile, string NoOfDays, string FromDate, string ToDate, string Reason, string LeaveFlag, string Personname)
    {

        string status = "0"; string message = "Error"; string date = null;
        try
        {
            if (deviceno.Length == 15)
            {
                if (mobile.Length == 10)
                {
                    string sql = "select * from [TransLeaveRequest] where DeviceNo='" + deviceno + "' and mobile='" + mobile + "' and (( FromDate>='" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "' and  todate<='" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') Or ( FromDate>='" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' and  todate<='" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "')) ";


                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,sql);
                    if (dt.Rows.Count == 0)
                    {

                        string strLF = "";
                        string leaveString = "";
                        int leaveLoop = Convert.ToInt32(Convert.ToDecimal(NoOfDays) * 2);

                        strLF = LeaveFlag;

                        if (strLF == "C")
                        {
                            for (int loop = 0; loop < leaveLoop; loop++) { leaveString += "L"; }

                            if (leaveLoop % 2 == 1) { leaveString += " "; }
                        }

                        if (strLF == "S")
                        {
                            leaveString = " ";
                            for (int loop = 0; loop < leaveLoop; loop++) { leaveString += "L"; }

                            if (leaveLoop % 2 == 0) { leaveString += " "; }
                        }

                        if (strLF == "F")
                        {
                            for (int loop = 0; loop < leaveLoop; loop++) { leaveString += "L "; }
                        }


                        sql = "insert into [TransLeaveRequest] (DeviceNo,Mobile,VDate,NoOfDays,FromDate,ToDate,Reason,LeaveFlag,LeaveString,Personname) values('" + deviceno + "','" + mobile + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + NoOfDays + "','" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "','" + Reason + "','" + LeaveFlag + "','" + leaveString + "','" + Personname + "')";
                        int res = DbConnectionDAL.ExecuteQuery(sql);  //1 On success
                        if (res == 1)
                        {
                            status = "1";
                            message = "Leave Submitted Successfully";
                            sql = "select deviceno,mobile,personname,srmobile from personmaster where active='Y' and deviceno='" + deviceno + "' and mobile='" + mobile + "'";
                            DataTable dtalluser = DbConnectionDAL.GetDataTable(CommandType.Text,sql);
                            string api = "http://zapsms.co.in/vendorsms/pushsms.aspx?user=rclknp&password=rcl15785&msisdn={0}&sid=RCLKNP&msg={1}&fl=0&gwid=2"; string response = "";
                            response = sms.sendSmsbyTableWithResponse(api, dtalluser.Rows[0]["SrMobile"].ToString(), "Dear Sir, Mr. " + dtalluser.Rows[0]["personname"].ToString() + ", is on leave for " + NoOfDays + "  days from " + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + " To  " + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                            sql = "insert into SentSmsToUsers(ToMobile,SMS,Response,Reason) values('" + dtalluser.Rows[0]["SrMobile"].ToString() + "','Dear Sir, Mr. " + dtalluser.Rows[0]["personname"].ToString() + ", is on leave for " + NoOfDays + "  days from " + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + " To  " + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "','" + response + "','Leave SMS')";
                            DbConnectionDAL.ExecuteQuery(sql);
                        }
                        else
                        {
                            status = "2";
                            message = "Leave Not Submitted";
                        }

                    }
                    else
                    {
                        status = "3";
                        message = "Leave Already Exist";
                        date = "Leave Exist From " + dt.Rows[0]["FromDate"].ToString() + " To " + dt.Rows[0]["Todate"].ToString();
                    }

                }
                else
                {
                    status = "4";
                    message = "Invalid Mobile No.";
                }
            }
            else
            {
                status = "5";
                message = "Invalid Deviceno";
            }
        }
        catch (Exception ex)
        {
            status = "0";
            message = "Error";
        }
        string[] array = new string[1];
        array[0] = string.Empty;
        List<ModelDataInsertLeave> rsttarget = new List<ModelDataInsertLeave>();
        rsttarget.Add(
                      new ModelDataInsertLeave
                      {
                          result = date
                      }
                  );
        ModelInsertLeave rst = new ModelInsertLeave
        {
            Status = status,
            Message = message,
            Data = rsttarget
        };

        //      string ignored = JsonConvert.SerializeObject(rst,
        //Formatting.Indented,
        //new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SentSmsOnLog(string deviceno, string mobile, string Log, string Personname, string LogCaptureTimestamp)
    {

        string status = "0"; string message = "Error"; string date = null;
        try
        {
            if (deviceno.Length == 15)
            {
                if (mobile.Length == 10)
                {
                    DateTime mDate = DateTime.Parse("1970-01-01");
                    mDate = mDate.AddSeconds(Convert.ToInt64(LogCaptureTimestamp) + 19800);
                    string logdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    string logstatus = DbConnectionDAL.GetStringScalarVal("select name from [dbo].[Log_Tracker_Master] where status='" + Log + "'");
                    string sql = "select deviceno,mobile,personname,srmobile from personmaster where active='Y' and deviceno='" + deviceno + "' and mobile='" + mobile + "'";
                    DataTable dtalluser = DbConnectionDAL.GetDataTable(CommandType.Text,sql);
                    string api = "http://zapsms.co.in/vendorsms/pushsms.aspx?user=rclknp&password=rcl15785&msisdn={0}&sid=RCLKNP&msg={1}&fl=0&gwid=2"; string response = "";
                    response = sms.sendSmsbyTableWithResponse(api, dtalluser.Rows[0]["SrMobile"].ToString(), "Dear Sir, Mr. " + dtalluser.Rows[0]["personname"].ToString() + ",  Mobile/Tab  " + logstatus + "  at " + logdate);
                    sql = "insert into SentSmsToUsers(ToMobile,SMS,Response,Reason) values('" + dtalluser.Rows[0]["SrMobile"].ToString() + "','Dear Sir, Mr. " + dtalluser.Rows[0]["personname"].ToString() + ",  Mobile/Tab " + logstatus + "  at " + logdate + "','" + response + "','Log Status SMS')";
                    int res = DbConnectionDAL.ExecuteQuery(sql);
                    if (res == 1)
                    {
                        status = "1";
                        message = "Message Sent Successfully";
                    }
                    else
                    {
                        status = "0";
                        message = "Error";
                    }

                }
                else
                {
                    status = "4";
                    message = "Invalid Mobile No.";
                }
            }
            else
            {
                status = "5";
                message = "Invalid Deviceno";
            }
        }
        catch (Exception ex)
        {
            status = "0";
            message = "Error";
        }
        string[] array = new string[1];
        array[0] = string.Empty;
        List<ModelDataInsertLeave> rsttarget = new List<ModelDataInsertLeave>();
        rsttarget.Add(
                      new ModelDataInsertLeave
                      {
                          result = date
                      }
                  );
        ModelInsertLeave rst = new ModelInsertLeave
        {
            Status = status,
            Message = message,
            Data = rsttarget
        };

        //      string ignored = JsonConvert.SerializeObject(rst,
        //Formatting.Indented,
        //new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }





    public class TrackerLog1
    {
        public string DeviceNo { get; set; }
        public string CurrentDate { get; set; }
        public string Status { get; set; }
        public string Pid { get; set; }
    }


    public class MobileLog1
    {
        public string DeviceNo { get; set; }
        public string CurrentDate { get; set; }
        public string Status { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Pid { get; set; }

    }

    public class Lcation_N_V6_1
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string CT { get; set; }
        public string Pid { get; set; }
        public string address { get; set; }
    }


    public class Lcation_D_V6_1
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string CT { get; set; }
        public string Pid { get; set; }
        public string address { get; set; }

    }


    public class LcationAttendance1
    {
        public string DeviceNo { get; set; }
        public string Lt { get; set; }
        public string Lg { get; set; }
        public string CD { get; set; }
        public string Bt { get; set; }
        public string Ga { get; set; }
        public string LType { get; set; }
        public string CT { get; set; }
        public string Pid { get; set; }
        public string address { get; set; }
        public string image { get; set; }       


    }
	
	
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JInsertVersion_v6_0_3(string version, Int64 CurrentDate, string DeviceNo, string mcomp, string model, string Reg_ID, string personid, string personname)
    {
        int retVal = 0;
        string Query = "";
        string mDate = string.Empty;
        string retInfo = "Version Not Inserted";
        try
        {
            //DateTime mDate = DateTime.Parse("1970-01-01");
            //mDate = mDate.AddSeconds(CurrentDate + 19800);
            mDate = DateTime.Now.ToString("yyyy-MMM-dd  HH:mm:ss");


            //ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
            string val = DMT.DMTApp_updateRegId(Reg_ID, DeviceNo);
            if (!string.IsNullOrEmpty(val))

            //string Query = "Update LineMaster set Reg_ID='" + Reg_ID + "' where DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER'";
            // DataAccessLayer.DAL.GetDemoLicenseIntScalarVal(Query);
            // Query = "select 1 from LineMaster where DeviceId='" + DeviceNo + "' and Upper(Product)='TRACKER'";
            // retVal = DataAccessLayer.DAL.GetDemoLicenseIntScalarVal(Query);
            // if (retVal == 1)
            {
                string strversion = "select count(*) from Version_Mast where DeviceId ='" + DeviceNo + "' and Version='" + version + "' and personid=" + personid + "";
                int count = DbConnectionDAL.GetIntScalarVal(strversion);
                if (count == 0)
                {
                    string strexist = "select 1 from Version_Mast where DeviceId ='" + DeviceNo + "' and personid=" + personid + "";
                    int gtval = DbConnectionDAL.GetIntScalarVal(strexist);
                    if (gtval > 0)
                    {
                        DbConnectionDAL.GetIntScalarVal("delete from Version_Mast where DeviceId ='" + DeviceNo + "' and personid=" + personid + "");
                    }
                    Query = "insert into Version_Mast(Version,DeviceId,Created_Date,model,company,personid,personname)" +
                                  "values ('" + version + "','" + DeviceNo + "','" + mDate + "','" + model + "','" + mcomp + "'," + personid + ",'" + personname + "')";

                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                }
            }
            else { retVal = -1; }
            if (retVal == 0)
            {
                retInfo = "Version Inserted";
            }
            else if (retVal == -1)
            { retInfo = "License Problem"; }
            else if (retVal == 1)
            { retInfo = "Rcl connection problem"; }
            else
            { retInfo = "xyz"; }
        }
        catch (Exception ex)
        {
            //retVal = 1;
            retInfo = ex.ToString();
        }
        Result rst = new Result
        {
            ResultMsg = retInfo
        };

        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }






    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLeave_V1(string deviceno, string mobile, string NoOfDays, string FromDate, string ToDate, string Reason, string LeaveFlag, string Personname, string Personid)
    {

        string status = "0"; string message = "Error"; string date = null;
        try
        {
            if (deviceno.Length == 15)
            {
                if (mobile.Length == 10)
                {
                    string sql = "select * from [TransLeaveRequest] where Personid=" + Personid + " and (( FromDate>='" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "' and  todate<='" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') Or ( FromDate>='" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' and  todate<='" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "')) ";


                    DataTable dt = DbConnectionDAL.getFromDataTable(sql);
                    if (dt.Rows.Count == 0)
                    {

                        string strLF = "";
                        string leaveString = "";
                        int leaveLoop = Convert.ToInt32(Convert.ToDecimal(NoOfDays) * 2);

                        strLF = LeaveFlag;

                        if (strLF == "C")
                        {
                            for (int loop = 0; loop < leaveLoop; loop++) { leaveString += "L"; }

                            if (leaveLoop % 2 == 1) { leaveString += " "; }
                        }

                        if (strLF == "S")
                        {
                            leaveString = " ";
                            for (int loop = 0; loop < leaveLoop; loop++) { leaveString += "L"; }

                            if (leaveLoop % 2 == 0) { leaveString += " "; }
                        }

                        if (strLF == "F")
                        {
                            for (int loop = 0; loop < leaveLoop; loop++) { leaveString += "L "; }
                        }


                        sql = "insert into [TransLeaveRequest] (DeviceNo,Mobile,VDate,NoOfDays,FromDate,ToDate,Reason,LeaveFlag,LeaveString,Personname,Personid) values('" + deviceno + "','" + mobile + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + NoOfDays + "','" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "','" + Reason + "','" + LeaveFlag + "','" + leaveString + "','" + Personname + "'," + Personid + ")";
                        int res = DbConnectionDAL.ExecuteQuery(sql);  //1 On success
                        if (res == 1)
                        {
                            status = "1";
                            message = "Leave Submitted Successfully";
                            string resinova = DbConnectionDAL.GetStringScalarVal("select url from enviro");
                            if (resinova.Contains("resinova"))
                            {
                                sql = "select deviceno,mobile,personname,srmobile from personmaster where active='Y' and deviceno='" + deviceno + "' and mobile='" + mobile + "'";
                                DataTable dtalluser = DbConnectionDAL.getFromDataTable(sql);
                                string api = "http://zapsms.co.in/vendorsms/pushsms.aspx?user=rclknp&password=rcl15785&msisdn={0}&sid=RCLKNP&msg={1}&fl=0&gwid=2"; string response = "";
                                // response = sms.sendSmsbyTableWithResponse(api, dtalluser.Rows[0]["SrMobile"].ToString(), "Dear Sir, Mr. " + dtalluser.Rows[0]["personname"].ToString() + ", is on leave for " + NoOfDays + "  days from " + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + " To  " + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                                sql = "insert into SentSmsToUsers(ToMobile,SMS,Response,Reason) values('" + dtalluser.Rows[0]["SrMobile"].ToString() + "','Dear Sir, Mr. " + dtalluser.Rows[0]["personname"].ToString() + ", is on leave for " + NoOfDays + "  days from " + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + " To  " + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "','" + response + "','Leave SMS')";
                                DbConnectionDAL.ExecuteQuery(sql);
                            }
                        }
                        else
                        {
                            status = "2";
                            message = "Leave Not Submitted";
                        }

                    }
                    else
                    {
                        status = "3";
                        message = "Leave Already Exist";
                        date = "Leave Exist From " + dt.Rows[0]["FromDate"].ToString() + " To " + dt.Rows[0]["Todate"].ToString();
                    }

                }
                else
                {
                    status = "4";
                    message = "Invalid Mobile No.";
                }
            }
            else
            {
                status = "5";
                message = "Invalid Deviceno";
            }
        }
        catch (Exception ex)
        {
            status = "0";
            message = "Error";
        }
        string[] array = new string[1];
        array[0] = string.Empty;
        List<ModelDataInsertLeave> rsttarget = new List<ModelDataInsertLeave>();
        rsttarget.Add(
                      new ModelDataInsertLeave
                      {
                          result = date
                      }
                  );
        ModelInsertLeave rst = new ModelInsertLeave
        {
            Status = status,
            Message = message,
            Data = rsttarget
        };

        //      string ignored = JsonConvert.SerializeObject(rst,
        //Formatting.Indented,
        //new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }







    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLocationDetails_Attendance2(string Attendance)
    {
        int retVal = 0; string error = "", address = "";
        try
        {
            if (Attendance != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<LcationAttendance1>>(Attendance);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", LType = "", createddt = "", Pid = "";
                        Int64 CD = 0, Mcd = 0, CT = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        Pid = objResponse[i].Pid.ToString();
                        if (!string.IsNullOrEmpty(objResponse[i].address))
                        {
                            address = objResponse[i].address.ToString();
                        }
                        CD = Convert.ToInt32(objResponse[i].CD);
                        CT = Convert.ToInt32(objResponse[i].CD);
                        LType = Convert.ToString(objResponse[i].LType);
                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                        DateTime mDate1 = DateTime.Parse("1970-01-01");
                        mDate1 = mDate1.AddSeconds(CT + 19800);
                        string cdate1 = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (!string.IsNullOrEmpty(LType))
                        {
                            string Query = "";
                            Query = "select * from Temp_LocationDetails where deviceno='" + DeviceNo + "' and LocationType='" + LType + "' and Cd='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                            DataTable dt = new DataTable();
                            dt = DbConnectionDAL.getFromDataTable(Query);
                            if (dt.Rows.Count == 0)
                            {
                                Query = "select * from LocationDetails where deviceno='" + DeviceNo + "' and vdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and LocationType='" + LType + "'";

                                dt = DbConnectionDAL.getFromDataTable(Query);
                                if (dt.Rows.Count == 0)
                                {
                                    //       Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,Cd,mobilecreateddate)" +
                                    //"values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "')";
                                    if (address != "")
                                    {
                                        Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,vdate,mobilecreateddate,description,insertdate,personid,slot,HomeFlag)" +
                                "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "','" + address + "',getdate()," + Pid + ",DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate1 + "'), 0), '" + cdate1 + "'),'N')";
                                    }
                                    else
                                    {
                                        Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,vdate,mobilecreateddate,description,insertdate,personid)" +
                                "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "','Updated Soon..',getdate()," + Pid + ")";
                                    }
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                    // DataAccessLayer.DAL.ExecuteQuery(Query);

                                    DataTable dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                    if (!string.IsNullOrEmpty(Lt))
                                    {
                                        if (dtla.Rows.Count > 0)
                                        {
                                            if (Convert.ToDateTime(dtla.Rows[0]["CurrentDate"]) < Convert.ToDateTime(cdate1))
                                            {
                                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                            }
                                        }
                                        else
                                        {
                                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                                        }
                                    }
                                    else
                                    {
                                        if (dtla.Rows.Count > 0)
                                        {
                                            if (Convert.ToDateTime(dtla.Rows[0]["CurrentDate"]) < Convert.ToDateTime(cdate1))
                                            {
                                                DbConnectionDAL.GetStringScalarVal("update LastActive set CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
            error = ex.ToString();
        }

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }




    //[WebMethod]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public void InsertLocationDetails_V6_1(string Location_D, string Location_N, string MobileLog, string TrackerLog, string Fence)
    //{
    //    //created on 19072019
    //    int retVal = 0;
    //    try
    //    {
    //        if (Location_D != "")
    //        {
    //            using (WebClient client = new WebClient())
    //            {
    //                var objResponse = JsonConvert.DeserializeObject<List<Lcation_D_V6_1>>(Location_D);

    //                for (int i = 0; i < objResponse.Count; i++)
    //                {
    //                    string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Pid = "";
    //                    Int64 CD = 0; Int64 CT = 0;// cd location time    and ct current time
    //                    DeviceNo = objResponse[i].DeviceNo.ToString();
    //                    Lt = objResponse[i].Lt.ToString();
    //                    Lg = objResponse[i].Lg.ToString();
    //                    Bt = objResponse[i].Bt.ToString();
    //                    Ga = objResponse[i].Ga.ToString();
    //                    CD = Convert.ToInt64(objResponse[i].CD);
    //                    CT = Convert.ToInt64(objResponse[i].CT);
    //                    Pid = objResponse[i].Pid.ToString();

    //                    DateTime mDate = DateTime.Parse("1970-01-01");
    //                    mDate = mDate.AddSeconds(CD + 19800);
    //                    string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


    //                    DateTime mDate1 = DateTime.Parse("1970-01-01");
    //                    mDate1 = mDate1.AddSeconds(CT + 19800);
    //                    string cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));
    //                    //string Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate)" +
    //                    //                              "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + mDate + "','G','" + Bt + "','" + Ga + "','" + mDate1 + "')";
    //                    string Query = string.Empty;
    //                    string _chkduplicate = string.Empty;
    //                    //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
    //                    //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
    //                    //if (_chkduplicate == "0")
    //                    {
    //                        Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description,personid)" +
    //                                                     "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..'," + Pid + ")";
    //                        retVal = DbConnectionDAL.GetIntScalarVal(Query);


    //                        DataTable dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
    //                        if (!string.IsNullOrEmpty(Lt))
    //                        {
    //                            if (dtla.Rows.Count > 0)
    //                            {
    //                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
    //                            }
    //                            else
    //                            {
    //                                string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
    //                                retVal = DbConnectionDAL.GetIntScalarVal(insqry);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (dtla.Rows.Count > 0)
    //                            {
    //                                DbConnectionDAL.GetStringScalarVal("update LastActive set CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
    //                            }
    //                        }

    //                    }
    //                }
    //            }
    //        }

    //        if (Location_N != "")
    //        {
    //            using (WebClient client = new WebClient())
    //            {
    //                var objResponse = JsonConvert.DeserializeObject<List<Lcation_N_V6_1>>(Location_N);

    //                for (int i = 0; i < objResponse.Count; i++)
    //                {
    //                    string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Pid = "";
    //                    Int64 CD = 0; Int64 CT = 0;
    //                    DeviceNo = objResponse[i].DeviceNo.ToString();
    //                    Lt = objResponse[i].Lt.ToString();
    //                    Lg = objResponse[i].Lg.ToString();
    //                    Bt = objResponse[i].Bt.ToString();
    //                    Ga = objResponse[i].Ga.ToString();
    //                    Pid = objResponse[i].Pid.ToString();
    //                    CD = Convert.ToInt32(objResponse[i].CD);
    //                    CT = Convert.ToInt32(objResponse[i].CT);
    //                    DateTime mDate = DateTime.Parse("1970-01-01");
    //                    mDate = mDate.AddSeconds(CD + 19800);
    //                    string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


    //                    DateTime mDate1 = DateTime.Parse("1970-01-01");
    //                    mDate1 = mDate1.AddSeconds(CT + 19800);
    //                    string cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));

    //                    //string Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate)" +
    //                    //                                "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + cdate1 + "')";
    //                    string Query = string.Empty;
    //                    string _chkduplicate = string.Empty;
    //                    //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
    //                    //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
    //                    //if (_chkduplicate == "0")
    //                    {
    //                        Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description,personid)" +
    //                                                     "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..'," + Pid + ")";
    //                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
    //                        DataTable dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
    //                        if (dtla.Rows.Count > 0)
    //                        {
    //                            DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
    //                        }
    //                        else
    //                        {
    //                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
    //                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        if (MobileLog != "")
    //        {
    //            using (WebClient client = new WebClient())
    //            {
    //                var objResponse = JsonConvert.DeserializeObject<List<MobileLog1>>(MobileLog);

    //                for (int i = 0; i < objResponse.Count; i++)
    //                {
    //                    string DeviceNo = "", Status = "", pid = "";
    //                    Int64 CurrentDate = 0, FromTime = 0, ToTime = 0;
    //                    string Query = string.Empty;

    //                    DeviceNo = objResponse[i].DeviceNo.ToString();
    //                    Status = objResponse[i].Status.ToString();
    //                    pid = objResponse[i].Pid.ToString();
    //                    FromTime = Convert.ToInt32(objResponse[i].FromTime);
    //                    ToTime = Convert.ToInt32(objResponse[i].ToTime);
    //                    CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
    //                    string _chkduplicate = string.Empty;
    //                    //Query = "Select isnull(count(*),0) from Temp_Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + CurrentDate + "' ";
    //                    //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
    //                    //if (_chkduplicate == "0")
    //                    {
    //                        Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,personid) values " +
    //                                " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "','" + FromTime + "','" + ToTime + "'," + pid + ")";
    //                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
    //                    }
    //                }
    //            }
    //        }

    //        if (TrackerLog != "")
    //        {
    //            using (WebClient client = new WebClient())
    //            {
    //                var objResponse = JsonConvert.DeserializeObject<List<TrackerLog1>>(TrackerLog);

    //                for (int i = 0; i < objResponse.Count; i++)
    //                {
    //                    string DeviceNo = "", Status = "", pid = "";
    //                    Int64 CurrentDate = 0;
    //                    string Query = string.Empty;

    //                    DeviceNo = objResponse[i].DeviceNo.ToString();
    //                    Status = objResponse[i].Status.ToString();
    //                    pid = objResponse[i].Pid.ToString();
    //                    CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);
    //                    string _chkduplicate = string.Empty;
    //                    //Query = "Select isnull(count(*),0) from Temp_Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + CurrentDate + "' ";
    //                    //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
    //                    //if (_chkduplicate == "0")
    //                    {
    //                        Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status,personid) values " +
    //                               " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "'," + pid + ")";
    //                        retVal = DbConnectionDAL.GetIntScalarVal(Query);
    //                    }
    //                }
    //            }
    //        }

    //        if (Fence != "")
    //        {
    //            using (WebClient client = new WebClient())
    //            {
    //                var objResponse = JsonConvert.DeserializeObject<List<Fence>>(Fence);

    //                for (int j = 0; j < objResponse.Count; j++)
    //                {
    //                    string DeviceNo = "", latitude = "",
    //                           longitude = "", PartyName = "";

    //                    DeviceNo = objResponse[j].DeviceNo.ToString();
    //                    latitude = objResponse[j].Lt.ToString();
    //                    longitude = objResponse[j].Lg.ToString();
    //                    PartyName = objResponse[j].PNm.ToString();

    //                    string str = "insert into Temp_FenceAddress (DeviceNo,Latitude,Longitude,PartyName,Created_date) values" +
    //                                     "('" + DeviceNo + "','" + latitude + "','" + longitude + "','" + PartyName + "','" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
    //                    retVal = DbConnectionDAL.GetIntScalarVal(str);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        retVal = 1;
    //    }

    //    string msg = "No";
    //    if (retVal == 0)
    //        msg = "Yes";

    //    Context.Response.Write(JsonConvert.SerializeObject(msg));

    //}

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLocationDetails_V6_1(string Location_D, string Location_N, string MobileLog, string TrackerLog, string Fence)
    {
        //created on 19072019
        int retVal = 0;       
            try
            {
                if (Location_D != "")
                {
                    using (WebClient client = new WebClient())
                    {
                        var objResponse = JsonConvert.DeserializeObject<List<Lcation_D_V6_1>>(Location_D);

                        for (int i = 0; i < objResponse.Count; i++)
                        {
                            string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Pid = "";
                            string address = "";
                            Int64 CD = 0; Int64 CT = 0;// cd location time    and ct current time
                            DeviceNo = objResponse[i].DeviceNo.ToString();
                            Lt = objResponse[i].Lt.ToString();
                            Lg = objResponse[i].Lg.ToString();
                            Bt = objResponse[i].Bt.ToString();
                            Ga = objResponse[i].Ga.ToString();
                            CD = Convert.ToInt64(objResponse[i].CD);
                            CT = Convert.ToInt64(objResponse[i].CT);
                            Pid = objResponse[i].Pid.ToString();
                            if (!string.IsNullOrEmpty(objResponse[i].address))
                            {
                                address = objResponse[i].address.ToString();
                            }

                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CD + 19800);
                            string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                            DateTime mDate1 = DateTime.Parse("1970-01-01");
                            mDate1 = mDate1.AddSeconds(CT + 19800);
                            string cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));
                            //string Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate)" +
                            //                              "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + mDate + "','G','" + Bt + "','" + Ga + "','" + mDate1 + "')";
                            string Query = string.Empty;
                            string _chkduplicate = string.Empty;
                            //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
                            //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                            //if (_chkduplicate == "0")
                            {
                                if (address != "")
                                {
                                    Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description,personid,vdate,slot,HomeFlag)" +
                                                                "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'" + address + "'," + Pid + " , cast('" + cdate1 + "' as date),DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate1 + "'), 0), '" + cdate1 + "'),'N')";
                                }
                                else
                                {
                                    Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description,personid)" +
                                                                 "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..'," + Pid + ")";
                                }
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);


                                DataTable dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                if (!string.IsNullOrEmpty(Lt))
                                {
                                    if (dtla.Rows.Count > 0)
                                    {
                                        if (Convert.ToDateTime(dtla.Rows[0]["CurrentDate"]) < Convert.ToDateTime(cdate1))
                                        {
                                            DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                        }
                                    }
                                    else
                                    {
                                        string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                        retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                                    }
                                }
                                else
                                {
                                    if (dtla.Rows.Count > 0)
                                    {
                                        if (Convert.ToDateTime(dtla.Rows[0]["CurrentDate"]) < Convert.ToDateTime(cdate1))
                                        {
                                            DbConnectionDAL.GetStringScalarVal("update LastActive set CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

                if (Location_N != "")
                {
                    using (WebClient client = new WebClient())
                    {
                        var objResponse = JsonConvert.DeserializeObject<List<Lcation_N_V6_1>>(Location_N);

                        for (int i = 0; i < objResponse.Count; i++)
                        {
                            string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", Pid = "";
                            string address = "";

                            Int64 CD = 0; Int64 CT = 0;
                            DeviceNo = objResponse[i].DeviceNo.ToString();
                            Lt = objResponse[i].Lt.ToString();
                            Lg = objResponse[i].Lg.ToString();
                            Bt = objResponse[i].Bt.ToString();
                            Ga = objResponse[i].Ga.ToString();
                            Pid = objResponse[i].Pid.ToString();
                            if (!string.IsNullOrEmpty(objResponse[i].address))
                            {
                                address = objResponse[i].address.ToString();
                            }
                            CD = Convert.ToInt32(objResponse[i].CD);
                            CT = Convert.ToInt32(objResponse[i].CT);
                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CD + 19800);
                            string gpsdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                            DateTime mDate1 = DateTime.Parse("1970-01-01");
                            mDate1 = mDate1.AddSeconds(CT + 19800);
                            string cdate1 = (mDate1.ToString("yyyy-MM-dd HH:mm:ss"));

                            //string Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate)" +
                            //                                "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + cdate1 + "')";
                            string Query = string.Empty;
                            string _chkduplicate = string.Empty;
                            //Query = "Select isnull(count(*),0) from LocationDetails where DeviceNo='" + DeviceNo + "' and CurrentDate='" + cdate1 + "' ";
                            //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                            //if (_chkduplicate == "0")
                            {
                                if (address != "")
                                {
                                    Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description,personid,vdate,slot,HomeFlag)" +
                                                                 "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'" + address + "'," + Pid + ", cast('" + cdate1 + "' as date),DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate1 + "'), 0), '" + cdate1 + "'),'N')";
                                }
                                else
                                {
                                    Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,mobilecreateddate,insertdate,description,personid)" +
                                                                 "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate1 + "','G','" + Bt + "','" + Ga + "','" + gpsdate + "',getdate(),'Updated Soon..'," + Pid + ")";
                                }
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                DataTable dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                if (dtla.Rows.Count > 0)
                                {
                                    if (Convert.ToDateTime(dtla.Rows[0]["CurrentDate"]) < Convert.ToDateTime(cdate1))
                                    {
                                        DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                    }
                                }
                                else
                                {
                                    string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                    retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                                }
                            }
                        }
                    }
                }

                if (MobileLog != "")
                {
                    using (WebClient client = new WebClient())
                    {
                        var objResponse = JsonConvert.DeserializeObject<List<MobileLog1>>(MobileLog);

                        for (int i = 0; i < objResponse.Count; i++)
                        {
                            string DeviceNo = "", Status = "", pid = "";
                            Int64 CurrentDate = 0, FromTime = 0, ToTime = 0;
                            string Query = string.Empty;
                            string _chkduplicate = string.Empty; string _chkduplicateQry = string.Empty;

                            DeviceNo = objResponse[i].DeviceNo.ToString();
                            Status = objResponse[i].Status.ToString();
                            pid = objResponse[i].Pid.ToString();

                            FromTime = Convert.ToInt32(objResponse[i].FromTime);
                            ToTime = Convert.ToInt32(objResponse[i].ToTime);
                            CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);

                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CurrentDate + 19800);
                            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                            DateTime FDate = DateTime.Parse("1970-01-01");
                            FDate = FDate.AddSeconds(FromTime + 19800);

                            DateTime TDate = DateTime.Parse("1970-01-01");
                            TDate = TDate.AddSeconds(ToTime + 19800);

                            //Query = "Select isnull(count(*),0) from Temp_Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + CurrentDate + "' ";
                            //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                            //if (_chkduplicate == "0")
                            //{
                            //    Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,personid) values " +
                            //            " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "','" + FromTime + "','" + ToTime + "'," + pid + ")";
                            //    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            //}


                            //_chkduplicateQry = "Select isnull(count(*),0) from Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + mDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                            //_chkduplicate = DbConnectionDAL.GetStringScalarVal(_chkduplicateQry);
                            //if (_chkduplicate == "0")
                            //{
                                Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot,personid) values ('" + DeviceNo + "','" + cdate + "','" + Status + "','" + FDate + "','" + TDate + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "')," + pid + ")";
                                retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            //}
                        }
                    }
                }

                if (TrackerLog != "")
                {
                    using (WebClient client = new WebClient())
                    {
                        var objResponse = JsonConvert.DeserializeObject<List<TrackerLog1>>(TrackerLog);

                        for (int i = 0; i < objResponse.Count; i++)
                        {
                            string DeviceNo = "", Status = "", pid = "";
                            Int64 CurrentDate = 0, FromTime = 0, ToTime = 0, Id = 0;
                            string Query = string.Empty;
                            string _chkduplicate = string.Empty; string _chkduplicateQry = string.Empty;

                            DeviceNo = objResponse[i].DeviceNo.ToString();
                            Status = objResponse[i].Status.ToString();
                            pid = objResponse[i].Pid.ToString();
                            CurrentDate = Convert.ToInt32(objResponse[i].CurrentDate);

                            DateTime mDate = DateTime.Parse("1970-01-01");
                            mDate = mDate.AddSeconds(CurrentDate + 19800);
                            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));

                            DateTime FDate = DateTime.Parse("1970-01-01");
                            FDate = FDate.AddSeconds(FromTime + 19800);

                            DateTime TDate = DateTime.Parse("1970-01-01");
                            TDate = TDate.AddSeconds(ToTime + 19800);
                            //Query = "Select isnull(count(*),0) from Temp_Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + CurrentDate + "' ";
                            //_chkduplicate = DbConnectionDAL.GetStringScalarVal(Query);
                            //if (_chkduplicate == "0")
                            //{
                            //    Query = @"insert into Temp_Log_Tracker(DeviceNo,CurrentDate,Status,personid) values " +
                            //           " ('" + DeviceNo + "','" + CurrentDate + "','" + Status + "'," + pid + ")";
                            //    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                            //}



                            //_chkduplicateQry = "Select isnull(count(*),0) from Log_Tracker where DeviceNo='" + DeviceNo + "' and CurrentDate='" + mDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                            //_chkduplicate = DbConnectionDAL.GetStringScalarVal(_chkduplicateQry);
                            //if (_chkduplicate == "0")
                            //{

                                if (Status == "M")
                                {
                                    Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot,personid) values ('" + DeviceNo + "',getdate(),'" + Status + "','" + FDate + "','" + TDate + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "')," + pid + ")";
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                }
                                else
                                {
                                    Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot,personid) values ('" + DeviceNo + "','" + cdate + "','" + Status + "','" + FDate + "','" + TDate + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "')," + pid + ")";
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                }
                            //}
                        }
                    }
                }

                if (Fence != "")
                {
                    using (WebClient client = new WebClient())
                    {
                        var objResponse = JsonConvert.DeserializeObject<List<Fence>>(Fence);

                        for (int j = 0; j < objResponse.Count; j++)
                        {
                            string DeviceNo = "", latitude = "",
                                   longitude = "", PartyName = "";

                            DeviceNo = objResponse[j].DeviceNo.ToString();
                            latitude = objResponse[j].Lt.ToString();
                            longitude = objResponse[j].Lg.ToString();
                            PartyName = objResponse[j].PNm.ToString();

                            string str = "insert into Temp_FenceAddress (DeviceNo,Latitude,Longitude,PartyName,Created_date) values" +
                                             "('" + DeviceNo + "','" + latitude + "','" + longitude + "','" + PartyName + "','" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
                            retVal = DbConnectionDAL.GetIntScalarVal(str);
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                retVal = 1;                
            }       

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }

    #endregion

    #region Send Email
    public void SendEmail(string Email, string Name, string DeviceID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table style='width:100%; color: #000000; font: 12px/17px Verdana,Arial,Helvetica,sans-serif;'><tr><td colspan='2' style='width: 150px;'><strong>Welcome to Dataman Tracker Services Account!</strong></td></tr><tr>");
            sb.AppendLine("<td>Dear " + Name + ", </td></tr><tr>");
            //sb.AppendLine("<td style='width: 300px;'>Congratulations! You have successfully created a new account with Dataman Tracker.</td><td></br></td></tr><tr>");
            //sb.AppendLine("<td style='width: 300px;'>As a registered user you can now enjoy the services of Dm Tracker</td><td></td></tr><tr>");
            sb.AppendLine("<td align=''>To activate your account, you'll need to verify your email address by clicking below :</td></tr><br><tr>");
            sb.AppendLine("<td align='left' style='height:40px;'><a target='_blank' href='http://demotracker.dataman.in/Activate.aspx?Registration=Y&Name=" + Name + "&DeviceID=" + DeviceID + "&Email=" + Email + "' style='width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px'>verify My Account</a></td></tr>");
            sb.AppendLine("<tr ><td >Or copy and paste this URL into your browser: </td></tr>");
            sb.AppendLine("<tr><td><a target='_blank' href='http://demotracker.dataman.in/Activate.aspx?Registration=Y&Name=" + Name + "&DeviceID=" + DeviceID + "&Email=" + Email + "'>http://demotracker.dataman.in/Activate.aspx?Registration=Y&Name=" + Name + "&DeviceID=" + DeviceID + "&Email=" + Email + "</a></td></tr>");
            //sb.AppendLine("<td style='width: 250px;'>Approval remarks</td><td style='width: 300px;'>" + dtb.Rows[0]["Appr_Remarks_latest"].ToString() + "</td></tr><tr>");
            sb.AppendLine("</table>");
            SendMailMessage("DmTracker@dataman.in", Email, "", "", "Dataman Tracker Activation", sb.ToString().Trim());

        }

        catch (Exception ex) { ex.ToString(); }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void JSendConfirmationMail(string Email, string Name)
    {
        string msg = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table style='width:100%; color: #000000; font: 12px/17px Verdana,Arial,Helvetica,sans-serif;'><tr><td colspan='2' style='width: 150px;'><strong>Welcome to Dataman Tracker Services Account!</strong></td></tr><tr>");
            sb.AppendLine("<td>Dear " + Name + ", </td></tr><tr style='height:30px;'>");
            sb.AppendLine("<td>Thank you for verifying your email ID</td></tr><tr>");
            sb.AppendLine("<td>Regards</td></tr><tr>");
            sb.AppendLine("<td>Dataman Web Team</td></tr>");
            //sb.AppendLine("<td style='width: 250px;'>Approval remarks</td><td style='width: 300px;'>" + dtb.Rows[0]["Appr_Remarks_latest"].ToString() + "</td></tr><tr>");
            sb.AppendLine("</table>");
            SendMailMessage("DmTracker@dataman.in", Email, "", "", "Your Email ID is now verified With Dataman Tracker! ", sb.ToString().Trim());
            msg = "Confirmation Mail Sent";
        }
        catch (Exception ex) { ex.ToString(); msg = "Confirmation Mail Not Sent"; }

        Result rst = new Result
        {
            ResultMsg = msg
        };
        Context.Response.Write(JsonConvert.SerializeObject(rst));
    }

    public static void SendMailMessage(string from, string to, string bcc, string cc, string subject, string body)
    {
        try
        {
            // Instantiate a new instance of MailMessage
            MailMessage mMailMessage = new MailMessage();
            // Set the sender address of the mail message
            mMailMessage.From = new MailAddress(from);
            // Set the recepient address of the mail message
            mMailMessage.To.Add(new MailAddress(to));

            // Check if the bcc value is null or an empty string
            if ((bcc != null) && (bcc != string.Empty))
            {
                // Set the Bcc address of the mail message
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }
            // Check if the cc value is null or an empty value
            if ((cc != null) && (cc != string.Empty))
            {
                // Set the CC address of the mail message
                string[] MailCC = cc.Split(',');
                foreach (var item in MailCC)
                {
                    if (!string.IsNullOrEmpty(item))
                        mMailMessage.CC.Add(new MailAddress(item.ToString()));
                }

            }
            //mail.Attachments.Add(new Attachment(FileUpload1.PostedFile.InputStream, FileUpload1.FileName));


            // Set the subject of the mail message
            mMailMessage.Subject = subject;
            // Set the body of the mail message
            mMailMessage.Body = body;

            // Set the format of the mail message body as HTML
            mMailMessage.IsBodyHtml = true;
            // Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal;

            // Instantiate a new instance of SmtpClient
            SmtpClient mSmtpClient = new SmtpClient();
            // Send the mail message


            mSmtpClient.Send(mMailMessage);
        }
        catch (Exception ex) { ex.ToString(); }
    }
    #endregion

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void InsertLocationDetails_Attendance2_New()
    {
        int retVal = 0; string error = "", address = "", imagePath = "";
        try
        {
            var httprequest = HttpContext.Current.Request;
            string Attendance = httprequest.Params["Attendance"];

            if (Attendance != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<LcationAttendance1>>(Attendance);

                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string image1 = "";
                        string imgurl = "";
                        image1 = objResponse[i].image;

                        if (image1 != "N/A")
                        {
                            byte[] bytes = Convert.FromBase64String(image1);

                            System.Drawing.Image image;
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = System.Drawing.Image.FromStream(ms);
                            }
                            string directoryPath = Server.MapPath(string.Format("~/{0}/", "AttendanceImages"));
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            string filename = Path.GetFileName(objResponse[i].DeviceNo.ToString() + '-' + timeStamp);
                            {
                                string filePath = Server.MapPath("~/AttendanceImages" + "/C_" + filename);
                                File.WriteAllBytes(filePath + ".png", bytes);

                                string fl_path = "/AttendanceImages" + "/C_" + filename + ".png";
                                imagePath = fl_path;
                                string host = DbConnectionDAL.GetStringScalarVal("select compurl from mastenviro").ToString();
                                imgurl = "http://" + host + fl_path;
                                //imgurl = "https://" + host + fl_path;

                            }

                        }
                        string DeviceNo = "", Lt = "", Lg = "", Bt = "", Ga = "", LType = "", createddt = "", Pid = "";
                        Int64 CD = 0, Mcd = 0, CT = 0;
                        DeviceNo = objResponse[i].DeviceNo.ToString();
                        Lt = objResponse[i].Lt.ToString();
                        Lg = objResponse[i].Lg.ToString();
                        Bt = objResponse[i].Bt.ToString();
                        Ga = objResponse[i].Ga.ToString();
                        Pid = objResponse[i].Pid.ToString();
                        if (!string.IsNullOrEmpty(objResponse[i].address))
                        {
                            address = objResponse[i].address.ToString();
                        }
                        CD = Convert.ToInt32(objResponse[i].CD);
                        CT = Convert.ToInt32(objResponse[i].CD);
                        LType = Convert.ToString(objResponse[i].LType);
                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string cdate = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));


                        DateTime mDate1 = DateTime.Parse("1970-01-01");
                        mDate1 = mDate1.AddSeconds(CT + 19800);
                        string cdate1 = (mDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (!string.IsNullOrEmpty(LType))
                        {
                            string Query = "";


                            Query = "select * from Temp_LocationDetails where deviceno='" + DeviceNo + "' and LocationType='" + LType + "' and Cd='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                            DataTable dt = new DataTable();
                            dt = DbConnectionDAL.getFromDataTable(Query);
                            if (dt.Rows.Count == 0)
                            {
                                Query = "select * from LocationDetails where deviceno='" + DeviceNo + "' and vdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and LocationType='" + LType + "'";

                                dt = DbConnectionDAL.getFromDataTable(Query);
                                if (dt.Rows.Count == 0)
                                {
                                    //       Query = "insert into Temp_LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,Cd,mobilecreateddate)" +
                                    //"values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "')";
                                    if (address != "")
                                    {

                                        Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,vdate,mobilecreateddate,description,insertdate,personid,slot,HomeFlag,Image)" + "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "','" + address + "',getdate()," + Pid + ",DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate1 + "'), 0), '" + cdate1 + "'),'N','" + imagePath + "')";
                                    }
                                    else
                                    {
                                        Query = "insert into LocationDetails (DeviceNo,Latitude,Longitude,CurrentDate,Log_m,Battery,Gps_accuracy,LocationType,vdate,mobilecreateddate,description,insertdate,personid,Image)" +
                                "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + cdate + "','G','" + Bt + "','" + Ga + "','" + LType + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + cdate1 + "','Updated Soon..',getdate()," + Pid + ",'" + imagePath + "')";
                                    }
                                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                                    // DataAccessLayer.DAL.ExecuteQuery(Query);

                                    DataTable dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                    if (!string.IsNullOrEmpty(Lt))
                                    {
                                        if (dtla.Rows.Count > 0)
                                        {
                                            if (Convert.ToDateTime(dtla.Rows[0]["CurrentDate"]) < Convert.ToDateTime(cdate1))
                                            {
                                                DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                            }
                                        }
                                        else
                                        {
                                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate1 + "','" + Lt + "','" + Lg + "','1')";
                                            retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                                        }
                                    }
                                    else
                                    {
                                        if (dtla.Rows.Count > 0)
                                        {
                                            if (Convert.ToDateTime(dtla.Rows[0]["CurrentDate"]) < Convert.ToDateTime(cdate1))
                                            {
                                                DbConnectionDAL.GetStringScalarVal("update LastActive set CurrentDate='" + cdate1 + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
            error = ex.ToString();
        }

        string msg = "No";
        if (retVal == 0)
            msg = "Yes";

        Context.Response.Write(JsonConvert.SerializeObject(msg));

    }
  
  
}
