using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
//using DataAccessLayer;
using System.Data;
using System.IO;
using System.Configuration;
using System.Net;
using BusinessLayer;
using System.Text;
using BAL;
using DAL;
using AstralFFMS.ServiceReferenceDMTracker;
/// <summary>
/// Summary description for WBS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WBS : System.Web.Services.WebService {
    SMSAdapter sms = new SMSAdapter();
    Common cm = new Common();
    public WBS () {

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
    [WebMethod]
    public XmlDocument GetTimeStamp()
    {
    //since epoch used - no need to add GMT hours
        long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        return getXMLForSingleMsg(epoch.ToString());
    }
    [WebMethod]
    public XmlDocument GPD(string DeviceNo)
    {
        string Query = "select Id as PersonID from PersonMaster where DeviceNo='" + DeviceNo + "'";
        string Pid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetStringScalarVal(Query);
        if(!string.IsNullOrEmpty(Pid))
            return getXMLForSingleMsg(Pid);
        else
        return getXMLForSingleMsg("N");
    }
    [WebMethod]
    
    private void CheckFenceAlert(string Lat, string Long, string PersonID, string Fdate)
    {
        bool Flag = false;
        try {
            string strgrp = "select GroupId from GrpMapp where PersonId='" + PersonID + "'";
            DataTable dtgrp = new DataTable();
            dtgrp = DbConnectionDAL.getFromDataTable(strgrp);
            for (int i = 0; i < dtgrp.Rows.Count; i++)
            {
                DataTable dtFa = new DataTable();
                string Faqry = "select * from FenceAddressMsg where GroupId='" + dtgrp.Rows[i]["GroupId"] + "' and PersonId ='" + PersonID + "' and Flag='0'";
                dtFa = DbConnectionDAL.getFromDataTable(Faqry);
                for (int p = 0; p < dtFa.Rows.Count; p++)
                {
                    double Dist = Calculate(Convert.ToDouble(dtFa.Rows[p]["Clat"]), Convert.ToDouble(dtFa.Rows[p]["Clong"]), Convert.ToDouble(Lat), Convert.ToDouble(Long));
                    if (Dist >= Convert.ToDouble(dtFa.Rows[p]["radius"]) )
                     if (dtFa.Rows[p]["Flag"].ToString() == "0")
                        {
                            string mbl = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select mobile from GroupMaster where groupid='" + dtFa.Rows[p]["groupid"] + "'"));// DataAccessLayer.DAL.GetStringScalarVal("select mobile from GroupMaster where groupid='" + dtFa.Rows[p]["groupid"] + "'");
                            if (!string.IsNullOrEmpty(mbl))
                            {
                                string person = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select personname+'('+deviceno+')' from personmaster where id='" + PersonID + "'"));// DataAccessLayer.DAL.GetStringScalarVal("select personname+'('+deviceno+')' from personmaster where id='" + PersonID + "'");
                                sms.sendSms(mbl, "Dear Sir,This is to notify you that " + person + " has left " + dtFa.Rows[p]["address"].ToString() + " at " + Fdate + ". Regards, Dataman Computer Systems");
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text,"update FenceAddressMsg set Flag='1' where id=" + dtFa.Rows[p]["id"] + "");// DataAccessLayer.DAL.ExecuteQuery("update FenceAddressMsg set Flag='1' where id=" + dtFa.Rows[p]["id"] + "");
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

    private string  GetFenceAddress(string Lat,string Long,string  PersonID)
    {
        string FenceAddr = "";
        try
        {
            string strgrp = "select GroupId from GrpMapp where PersonId='"+PersonID+"'";
            DataTable dtgrp = new DataTable();
            dtgrp = DbConnectionDAL.getFromDataTable(strgrp);
            for (int i = 0; i < dtgrp.Rows.Count; i++)
            {
                DataTable dtFa = new DataTable();
                string Faqry = "select Clat,Clong,Radius,Address from FenceAddress where GroupId='" + dtgrp.Rows[i]["GroupId"] + "' and CLat LIKE '" + Lat.Substring(0, 5) + "%' and CLong like '" + Long.Substring(0, 5) + "%'";
                dtFa = DbConnectionDAL.getFromDataTable(Faqry);
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

    //[WebMethod]
    //public XmlDocument SendSms()
    //{
    //    DataTable dt = new DataTable();
    //    String str = "select lastactive.DeviceNo,personmaster.PersonName as Personname,personmaster.Mobile,Personmaster.SrMobile,Personmaster.SendSmsPerson,Personmaster.SendSmsSenior,convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate,LastActive.CurrentDate AS DDate from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo order by lastactive.currentdate desc";
    //    dt = DataAccessLayer.DAL.getFromDataTable(str);
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        string Personname = dt.Rows[i]["Personname"].ToString();
    //        String Mobile = dt.Rows[i]["Mobile"].ToString();
    //        String SrMobile = dt.Rows[i]["SrMobile"].ToString();
    //        String SmsP = dt.Rows[i]["SendSmsPerson"].ToString();
    //        String SmsS = dt.Rows[i]["SendSmsSenior"].ToString();
    //        DateTime startdate = new DateTime();
    //        DateTime Enddate = new DateTime(); 
    //        startdate = DateTime.ParseExact(DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
    //        Enddate = DateTime.ParseExact(Convert.ToDateTime(dt.Rows[i]["DDate"]).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
    //        TimeSpan diff = (startdate - Enddate);
    //        double MINS = diff.TotalMinutes;
    //        if (MINS >= 61)
    //        {
               
    //            if (!string.IsNullOrEmpty(Mobile) && SmsP == "Y")
    //            {
    //                sms.sendSms(Mobile, "Dear Sir,This is to notify you that data of " + Personname + " has Not Uploaded since last one hour in FFT Application. Regards, Dataman Computer Systems Pvt.Ltd");

    //            }
    //            else { }
    //            if (!string.IsNullOrEmpty(SrMobile) && SmsS == "Y")
    //            {
    //                sms.sendSms(SrMobile, "Dear Sir,This is to notify you that data of " + Personname + " has Not Uploaded since last one hour in FFT Application. Regards, Dataman Computer Systems Pvt.Ltd");
                   
    //            }
    //            else { }
    //        }               
                   
    //         }       
    //    return getXMLForSingleMsg("Y");
            
    //}    
    [WebMethod]
    public void SendSmsToPerson()
    {
        try
        {
            DataTable dt = new DataTable();
            String str = "select lastactive.DeviceNo,personmaster.PersonName as Personname,personmaster.Mobile,Personmaster.SendSmsPerson,convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate,CONVERT(DATETIME,CONVERT(VARCHAR(10), GETDATE(), 112))+[FromTime] as Fromtime,CONVERT(DATETIME,CONVERT(VARCHAR(10), GETDATE(), 112))+[ToTime]as ToTime, getdate() as Currdt,LastActive.CurrentDate AS DDate from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo order by lastactive.currentdate desc";
            dt = DbConnectionDAL.getFromDataTable(str);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Personname = dt.Rows[i]["Personname"].ToString();
                String Mobile = dt.Rows[i]["Mobile"].ToString();
                String SmsP = dt.Rows[i]["SendSmsPerson"].ToString();
                DateTime startdate = new DateTime();
                DateTime Enddate = new DateTime();
                startdate = DateTime.ParseExact(DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                Enddate = DateTime.ParseExact(Convert.ToDateTime(dt.Rows[i]["DDate"]).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                TimeSpan diff = (startdate - Enddate);
                double MINS = diff.TotalMinutes;
                if (MINS >= 61)
                {
                    if (Convert.ToDateTime(dt.Rows[i]["Fromtime"]) < Convert.ToDateTime(dt.Rows[i]["Currdt"]) && Convert.ToDateTime(dt.Rows[i]["ToTime"]) > Convert.ToDateTime(dt.Rows[i]["Currdt"]))
                    {
                        if (!string.IsNullOrEmpty(Mobile) && SmsP == "Y")
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" Your data has Not Uploaded since last one hour in FFT Application. Regards, Dataman Computer Systems Pvt.Ltd");
                            sms.sendSms(Mobile, "Dear " + Personname + "," + sb.ToString());
                        }
                    }

                }
            }
        }
        catch (Exception ex) { ex.ToString(); }
    }
 
         
    [WebMethod]
    public void SendSmsToSenior()
    {
        DataTable dtbl = new DataTable(); DataTable dt = new DataTable();
        string strbl = "select distinct mobile from PersonMaster where isnull(Active,'Y')<>'N' and personmaster.Mobile=personmaster.srmobile union select distinct mobile from PersonMaster where isnull(Active,'Y')<>'N' and personmaster.Mobile<>personmaster.srmobile or personmaster.srmobile is null";
        dtbl = DbConnectionDAL.getFromDataTable(strbl);
        String str = "select lastactive.DeviceNo,personmaster.PersonName as Personname,personmaster.Mobile as Pmobile,Personmaster.SrMobile,isnull(Personmaster.SendSmsPerson,'Y')as SendSmsPerson,isnull(Personmaster.SendSmsSenior,'Y')as SendSmsSenior,convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate,LastActive.CurrentDate AS DDate,isnull(LastActive.smsFlag,0) as smsFlag,DateDiff(hh, LastActive.Currentdate,dateadd(ss,19800,getutcdate())) as HrDiff from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo where isnull(PersonMaster.Active,'Y')<>'N' order by lastactive.currentdate desc";
        dt = DbConnectionDAL.getFromDataTable(str);
        for (int i = 0; i < dtbl.Rows.Count; i++)
        {
            string Personname = string.Empty, TPersonname = string.Empty;
            DataRow[] dr = dt.Select("srmobile='" + dtbl.Rows[i]["mobile"] + "'");
            if (dr.Length > 0)
            {
                foreach (DataRow dr1 in dr)
                {
                    DateTime startdate = new DateTime();
                    DateTime Enddate = new DateTime();
                    startdate = DateTime.ParseExact(DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                    Enddate = DateTime.ParseExact(Convert.ToDateTime(dr1["DDate"]).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                    TimeSpan diff = (startdate - Enddate);
                    double MINS = diff.TotalMinutes;
                    if (MINS >= 61 && dr1["smsFlag"].ToString()=="0")
                    {
                        if (dr1["SendSmsSenior"].ToString() == "Y")
                        {
                            string strupd = "update LastActive set smsFlag ='1' where DeviceNo='" + dr1["DeviceNo"] + "'";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text,strupd);
                            Personname += dr1["Personname"].ToString() + ",";
                        }
                    }
                    if (Convert.ToInt32(dr1["HrDiff"]) >= 12)
                    {
                        if (dr1["SendSmsSenior"].ToString() == "Y")
                        {
                            TPersonname += dr1["Personname"].ToString() + ",";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Personname))
                {
                    Personname = Personname.Substring(0,Personname.Length - 1); 
                    SendSmsDataToSenior(Personname, dtbl.Rows[i]["mobile"].ToString());
                }
                if (!string.IsNullOrEmpty(TPersonname))
                {
                    TPersonname = TPersonname.Substring(0,TPersonname.Length - 1);
                    SendSmsDataToSenior(TPersonname, dtbl.Rows[i]["mobile"].ToString());
                }
            }
        }
     }
   
    private void SendSmsDataToSenior(string Persons, string SeniorMobile)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Dear Sir,This is to notify you that data of following Person(s)" + Persons + " has Not Uploaded since last one hour in FFT Application. Regards, Dataman Computer Systems Pvt.Ltd");
              sms.sendSms(SeniorMobile+",9415128075", sb.ToString());
        }
        catch (Exception ex) { ex.ToString(); }
    }
   
    [WebMethod]
    public XmlDocument InsC(string Pi, string MC, string MN, string LA, string CI, Int64 CD,string Bt)
    { 
        // Not In Use
        int retVal = 0;
        string Latitude = string.Empty;
        string Longitude = string.Empty;
         try
        {
            string getdevid="select DeviceNo from PersonMaster where ID="+Pi+"";
            string DeviceNo = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, getdevid));// DataAccessLayer.DAL.GetStringScalarVal(getdevid);
            if (!string.IsNullOrEmpty(DeviceNo))
            {
                DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CD + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm")); 
                string createText = "";
                string[] s = GoogleMapsApi.GetLatLng(Convert.ToInt32(MC.Trim()), Convert.ToInt32(MN.Trim()), Convert.ToInt32(LA.Trim()), Convert.ToInt32(CI.Trim()));
                if (!string.IsNullOrEmpty(s[0].ToString()) && !string.IsNullOrEmpty(s[1].ToString()))
                {
                    Latitude = (s[0].ToString());
                    Longitude = (s[1].ToString());
                    decimal argLat = Convert.ToDecimal(Longitude);
                    decimal argLng = Convert.ToDecimal(LA);
                    int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
                    int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
                    if (cntLat > 4 && cntLng > 4)
                    {
                        createText = "" + DeviceNo + "," + MC + "," + MN + "," + LA + "," + CI + "," + cdate + ",C" + "," + Bt + ",0," + Latitude + "," + Longitude +
                        "" + Environment.NewLine;

                        if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,"select 1 from LastActive where DeviceNo='" + DeviceNo + "'"))<= 0)// DataAccessLayer.DAL.GetIntScalarVal("select 1 from LastActive where DeviceNo='" + DeviceNo + "'") <= 0)
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long)" +
                                      "values ('" + DeviceNo + "','" + cdate + "','" + Latitude + "','" + Longitude + "')";
                            retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, insqry));// DataAccessLayer.DAL.GetIntScalarVal(insqry);
                        }
                        else
                        {
                            DbConnectionDAL.GetScalarValue(CommandType.Text, "update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'" + " where DeviceNo='" + DeviceNo + "'");// DataAccessLayer.DAL.GetStringScalarVal("update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'" + " where DeviceNo='" + DeviceNo + "'");
                        }
                        
                         CheckFenceAlert(Latitude, Longitude, Pi,cdate);
                         //string Address = GetFenceAddress(Latitude, Longitude, Pi);
                         string FAddress = GetFenceAddress(Latitude, Longitude, Pi);
                         string Address = "";

                        //if (string.IsNullOrEmpty(Address))
                        //{
                            WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Latitude, Longitude);
                        //}
                            if (!string.IsNullOrEmpty(FAddress)) { Address = Address + "(" + FAddress + ")"; }
                        if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)))<= 0)//  DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                        {
                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot)" +
                                     "values ('" + DeviceNo + "','" + Latitude + "','" + Longitude + "','" + Address + "','" + cdate + "','C','" + Bt + "','" + 0 + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'))";
                            retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                        }
                    }
                    else
                           retVal = 0;          
                }
                else
                {
                    retVal = 0;
                    createText = "" + DeviceNo + "," + MC + "," + MN + "," + LA + "," + CI + "," + cdate + ",C" + "," + Bt + ",0" +
                ",Location Not Found By Google" + Environment.NewLine;
                }
                //using (System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFileCID.txt"), true))
                //{
                //    TextFileCID.WriteLine(createText);
                //    TextFileCID.Close();
                //}
            }            
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            return getXMLForSingleMsg("Y");
        else
            return getXMLForSingleMsg("N");
    }

    [WebMethod]
    public XmlDocument InsD(string Pi, string Lt, string Lg, Int64 CD, string Bt, string Ga)
    {
        int retVal = 0;
        try
        {
            string HomeFlag = "N";
            decimal argLat = Convert.ToDecimal(Lt);
            decimal argLng = Convert.ToDecimal(Lg);
            int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
            int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
            if (cntLat > 4 && cntLng > 4)
            {
            
                string getdevid = "select DeviceNo from PersonMaster where ID=" + Pi + "";
                string DeviceNo =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, getdevid));// DataAccessLayer.DAL.GetStringScalarVal(getdevid);
                if (!string.IsNullOrEmpty(DeviceNo))
                {
                    DateTime mDate = DateTime.Parse("1970-01-01");
                    mDate = mDate.AddSeconds(CD + 19800);
                    string cdate = (mDate.ToString("yyyy-MM-dd HH:mm"));
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
                        dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        if (dtla.Rows.Count > 0)
                        {
                            Common cm = new Common();
                            if (cm.GetValidLocationTicks(dtla.Rows[0]["lat"].ToString(), dtla.Rows[0]["long"].ToString(), Lt, Lg, Convert.ToDateTime(dtla.Rows[0]["currentdate"]), mDate, "G") == true)
                            {
                                DbConnectionDAL.GetScalarValue(CommandType.Text, "update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");// DataAccessLayer.DAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "', smsFlag ='1'  where DeviceNo='" + DeviceNo + "'");
                                CheckFenceAlert(Lt, Lg, Pi, cdate);
                                string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                string Address = "";
                                WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                                Address = DMT.InsertAddress(Lt, Lg);
                                if (!string.IsNullOrEmpty(FAddress))
                                { Address = Address + "(" + FAddress + ")"; HomeFlag = "H"; }
                                if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)))<= 0)// DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                {
                                    string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                              "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                    retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                                }

                            }
                        }

                        else
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                            retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, insqry));// DataAccessLayer.DAL.GetIntScalarVal(insqry);
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                            Address = DMT.InsertAddress(Lt, Lg);
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + "(" + FAddress + ")"; }
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)))<= 0)// DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','G','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "')'" + HomeFlag + "')";
                                retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                            }
                        }

                        //using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                        //{
                        //    file2.WriteLine(createText);
                        //    file2.Close();
                        //}
                    }
                }
                else
                {
                    string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                    retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                }
              }
               
            }
            else
            {
                retVal = 0;
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            return getXMLForSingleMsg("Y");
        else
            return getXMLForSingleMsg("N");
    }
    private bool GetValidCountry(string clat,string clong){
        bool retval = true;
        string strcountries ="select count(*) from BlackListCountries where Clat='"+clat+"' AND Clong='"+clong+"'";
        if ( Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcountries)) > 0)//DataAccessLayer.DAL.GetIntScalarVal(strcountries) > 0)
        {
            retval = false;
        }
        return retval;
    }
    [WebMethod]
    public XmlDocument InsN(string Pi, string Lt, string Lg, Int64 CD, string Bt, string Ga)
    {
        int retVal = 0;
        try
        {
            string HomeFlag = "N";
            decimal argLat = Convert.ToDecimal(Lt);
            decimal argLng = Convert.ToDecimal(Lg);
            int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
            int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
            if (cntLat > 4 && cntLng > 4)
            {
               
                    string getdevid = "select DeviceNo from PersonMaster where ID=" + Pi + "";
                    string DeviceNo = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, getdevid));// DataAccessLayer.DAL.GetStringScalarVal(getdevid);
                    if (!string.IsNullOrEmpty(DeviceNo))
                    {
                        DateTime mDate = DateTime.Parse("1970-01-01");
                        mDate = mDate.AddSeconds(CD + 19800);
                        string cdate = (mDate.ToString("yyyy-MM-dd HH:mm"));

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
                                dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                                if (dtla.Rows.Count > 0)
                                {
                                    Common cm = new Common();
                                    if (cm.GetValidLocationTicks(dtla.Rows[0]["lat"].ToString(), dtla.Rows[0]["long"].ToString(), Lt, Lg, Convert.ToDateTime(dtla.Rows[0]["currentdate"]), mDate, "N") == true)
                                    {
                                        DbConnectionDAL.GetScalarValue(CommandType.Text, "update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");// DataAccessLayer.DAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");
                                        CheckFenceAlert(Lt, Lg, Pi, cdate);
                                        string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                        string Address = "";
                                        WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                                        Address = DMT.InsertAddress(Lt, Lg);
                                        if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + "(" + FAddress + ")"; }
                                        //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                                        //if (string.IsNullOrEmpty(Address))
                                        //{
                                        //    ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                        //    Address = DMT.InsertAddress(Lt, Lg);
                                        //}
                                        if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)))<= 0)// DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                        {
                                            string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                      "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                            retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                                        }

                                    }
                                }

                                else
                                {
                                    string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag ) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                                    retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, insqry));// DataAccessLayer.DAL.GetIntScalarVal(insqry);
                                    CheckFenceAlert(Lt, Lg, Pi, cdate);
                                    string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                    string Address = "";
                                    if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + "(" + FAddress + ")"; }
                                    //                            ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                    //   Address = DMT.InsertAddress(Lt, Lg);
                                    //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                                    //if (string.IsNullOrEmpty(Address))
                                    //{
                                    //    ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                    //    Address = DMT.InsertAddress(Lt, Lg);
                                    //}
                                    if ( Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo))) <= 0)//DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                    {
                                        string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                                  "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','N','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                        retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                                    }
                                }

                                //using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                                //{
                                //    file2.WriteLine(createText);
                                //    file2.Close();
                                //}
                            }
                        }
                        else
                        {
                            string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + CurrDate + "','WD','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

                            retVal = retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                        }
                    }
                
                else
                {
                    retVal = 0;
                }
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            return getXMLForSingleMsg("Y");
        else
            return getXMLForSingleMsg("N");
    }

    [WebMethod]
    public XmlDocument InsW(string Pi, string Lt, string Lg, Int64 CD, string Bt, string Ga)
    {
        int retVal = 0;
        try
        {
            string HomeFlag = "N";
            decimal argLat = Convert.ToDecimal(Lt);
            decimal argLng = Convert.ToDecimal(Lg);
            int cntLat = BitConverter.GetBytes(decimal.GetBits(argLat)[3])[2];
            int cntLng = BitConverter.GetBytes(decimal.GetBits(argLng)[3])[2];
            if (cntLat > 4 && cntLng > 4)
            {
                string getdevid = "select DeviceNo from PersonMaster where ID=" + Pi + "";
                string DeviceNo =  Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, getdevid));// DataAccessLayer.DAL.GetStringScalarVal(getdevid);
                if (!string.IsNullOrEmpty(DeviceNo))
                {
                    DateTime mDate = DateTime.Parse("1970-01-01");
                    mDate = mDate.AddSeconds(CD + 19800);
                    string cdate = (mDate.ToString("yyyy-MM-dd HH:mm"));

                    DateTime CurrDate = DateTime.UtcNow.AddSeconds(19800);

                    if (CurrDate >= mDate)
                    {
                        string createText = "" + DeviceNo + "," + Lt + "," + Lg + "," + cdate + ",W" + "," + Bt + "," +
                            Ga + "" + Environment.NewLine;
                        string approx = "0";
                        if (!string.IsNullOrEmpty(Ga) && Ga != "null")
                        { approx = Ga; }
                        DataTable dtla = new DataTable();
                        dtla = DbConnectionDAL.getFromDataTable("select top(1)* from LastActive where DeviceNo='" + DeviceNo + "' order by CurrentDate");
                        if (dtla.Rows.Count > 0)
                        {
                            Common cm = new Common();
                            if (cm.GetValidLocationTicks(dtla.Rows[0]["lat"].ToString(), dtla.Rows[0]["long"].ToString(), Lt, Lg, Convert.ToDateTime(dtla.Rows[0]["currentdate"]), mDate, "W") == true)
                            {
                                DbConnectionDAL.GetScalarValue(CommandType.Text, "update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");// DataAccessLayer.DAL.GetStringScalarVal("update LastActive set Lat='" + Lt + "',Long='" + Lg + "',CurrentDate='" + cdate + "',smsFlag ='1'" + " where DeviceNo='" + DeviceNo + "'");
                                CheckFenceAlert(Lt, Lg, Pi, cdate);
                                string FAddress = GetFenceAddress(Lt, Lg, Pi);
                                string Address = "";
                                WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                                Address = DMT.InsertAddress(Lt, Lg);
                                if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + "(" + FAddress + ")"; }
                                //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                                //if (string.IsNullOrEmpty(Address))
                                //{
                                //    ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                //    Address = DMT.InsertAddress(Lt, Lg);
                                //}
                                if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo))) <= 0)// DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                                {
                                    string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                              "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','W','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                    retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                                }

                            }
                        }

                        else
                        {
                            string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long,smsFlag ) values ('" + DeviceNo + "','" + cdate + "','" + Lt + "','" + Lg + "','1')";
                            retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, insqry));// DataAccessLayer.DAL.GetIntScalarVal(insqry);
                            CheckFenceAlert(Lt, Lg, Pi, cdate);
                            string FAddress = GetFenceAddress(Lt, Lg, Pi);
                            string Address = "";
                            if (!string.IsNullOrEmpty(FAddress)) { HomeFlag = "H"; Address = Address + "(" + FAddress + ")"; }
                            //                            ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //   Address = DMT.InsertAddress(Lt, Lg);
                            //if (!string.IsNullOrEmpty(Address)) { HomeFlag = "H"; }
                            //if (string.IsNullOrEmpty(Address))
                            //{
                            //    ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            //    Address = DMT.InsertAddress(Lt, Lg);
                            //}
                            if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo))) <= 0)// DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                            {
                                string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy,vdate,slot,HomeFlag)" +
                                          "values ('" + DeviceNo + "','" + Lt + "','" + Lg + "','" + Address + "','" + cdate + "','W','" + Bt + "','" + approx + "','" + cdate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + cdate + "'), 0), '" + cdate + "'),'" + HomeFlag + "')";
                                retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                            }
                        }

                        //using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                        //{
                        //    file2.WriteLine(createText);
                        //    file2.Close();
                        //}
                    }
                }
            }
            else
            {
                retVal = 0;
            }
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            return getXMLForSingleMsg("Y");
        else
            return getXMLForSingleMsg("N");
    }

     [WebMethod]
    public bool ChkData(string DeviceNo, DateTime cdate, string elat, string elng)
    {
        bool iflag = false;
        string qry = "SELECT TOP(1) latitude,longitude,CurrentDate FROM LocationDetails WHERE DeviceNo='" + DeviceNo + "' AND CurrentDate <'" + cdate + "' ORDER BY CurrentDate DESC";
        DataTable dt = new DataTable();
        dt = DbConnectionDAL.getFromDataTable(qry);
        if (dt.Rows.Count > 0)
        {
           double dist= Calculate(Convert.ToDouble(dt.Rows[0]["latitude"]),Convert.ToDouble(dt.Rows[0]["latitude"]),Convert.ToDouble(elat),Convert.ToDouble(elng));
           double time=(Convert.ToDateTime(dt.Rows[0]["CurrentDate"])-cdate).Minutes;
           double speed = dist / time;
           if (speed > 1.5)
               iflag = true;
        }
       
        return iflag;
    }
     [WebMethod]
     public string InsertMobileLog(string DeviceNo, Int64 CurrentDate, string Status, Int64 FromTime, Int64 ToTime)
     {
         int retVal = 0;
         string retInfo = "";
         try
         {
             DateTime mDate = DateTime.Parse("1970-01-01");
             mDate = mDate.AddSeconds(CurrentDate + 19800);
             DateTime FDate = DateTime.Parse("1970-01-01");
             FDate = FDate.AddSeconds(FromTime + 19800);
             DateTime TDate = DateTime.Parse("1970-01-01");
             TDate = TDate.AddSeconds(ToTime + 19800);

             string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,FromTime,ToTime,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + FDate + "','" + TDate + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";

             retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
         }
         catch (Exception ex)
         {
             retVal = 1;
         }
         if (retVal == 0)
             retInfo = "Record Inserted";
         else
             retInfo = "Record Not Inserted";
         return retInfo;
     }
     [WebMethod]
     public XmlDocument InsertFenceAddress(Int64 PersonID,string latitude,string longitude,string PartyName)
     {
         int retVal = 0;
         DataTable dtgrps = DbConnectionDAL.getFromDataTable("select GroupId from GrpMapp where personid=" + PersonID + "");
         for (int i = 0; i < dtgrps.Rows.Count; i++)
         {
             string str = "insert into fenceaddress (groupId,clat,clong,radius,address,PersonID_createdFence,Created_date)values" +
                 "('" + dtgrps.Rows[i]["GroupId"] + "','" + latitude + "','" + longitude + "',0.02,'" + PartyName + "'," + PersonID + ",'" + DateTime.Now.ToUniversalTime().AddSeconds(19800) + "')";
             retVal =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
         }

         if (retVal == 0)
             return getXMLForSingleMsg("Y");
         else
             return getXMLForSingleMsg("N");
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

   public static XmlDocument getXMLForSingleMsg(string msg)
    {
        string tblnm = "GetValue";
        string dsnm = "Data";
        DataTable dtbl = new DataTable();
        dtbl.Columns.Add("Value");
        DataRow dr= dtbl.NewRow();
        dr["Value"] = msg;
        dtbl.Rows.Add(dr);
        dtbl.AcceptChanges();
        DataSet dds = new DataSet();
        dds.Tables.Add(dtbl);
        dds.Tables[0].TableName = tblnm;
        dds.DataSetName = dsnm;
        StringWriter sw = new StringWriter();
        XmlTextWriter xtw = new XmlTextWriter(sw);
        XmlDocument xd = new XmlDocument();
        dds.WriteXml(xtw, XmlWriteMode.IgnoreSchema);
        string str = sw.ToString();
        xd.LoadXml(str);
        XmlNode nDate = xd.CreateElement("DateTime");
        nDate.InnerText = (DateTime.Now.Subtract(Convert.ToDateTime("01/01/1980"))).TotalMilliseconds.ToString();
        XmlElement y = xd.DocumentElement;
        XmlNode x = xd.GetElementsByTagName(tblnm)[0];
        y.InsertBefore(nDate, x);
        sw.Close();
        xtw.Close();
        dds.Clear();
        dtbl.Clear();
        return xd;
    }

}
