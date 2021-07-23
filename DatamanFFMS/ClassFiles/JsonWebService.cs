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
using BAL;
using DAL;
using AstralFFMS.ServiceReferenceDMTracker;
/// <summary>
/// Summary description for JsonWebService
/// </summary>
[WebService]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class JsonWebService : System.Web.Services.WebService {
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
    #region Andriod Tracker
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
    public void SendConfirmationMail(string Email, string Name)
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
           msg="Confirmation Mail Sent";
        }
        catch (Exception ex) { ex.ToString();  msg="Confirmation Mail Not Sent"; }
        Results rst = new Results
        {
            ResultList = new List<Result>() {
        new Result { ResultMsg = msg}
             }
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

    #region Json WebService
    #region Person
      public class Persons
     {

         [DataMember]
         public List<Person> PersonList { get; set; }
     }

     [DataContract]
     public class Person
     {
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
     }
     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void GetPersonDetail(string DeviceNo)
     {
         string Query = "select PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc from PersonMaster where DeviceNo='" + DeviceNo + "'";
         DataTable dtbl = new DataTable();
         dtbl =DbConnectionDAL.getFromDataTable(Query);
         if (dtbl.Rows.Count > 0)
         {
             Persons Person = new Persons
             {
                 PersonList = new List<Person>() {
        new Person { PersonName = dtbl.Rows[0]["PersonName"].ToString(), FromTime = dtbl.Rows[0]["FromTime"].ToString() ,ToTime=dtbl.Rows[0]["ToTime"].ToString(),
            Interval=dtbl.Rows[0]["Interval"].ToString(),UploadInterval=dtbl.Rows[0]["UploadInterval"].ToString(),RetryInterval=dtbl.Rows[0]["RetryInterval"].ToString(),
            Degree=dtbl.Rows[0]["Degree"].ToString(),Sys_Flag=dtbl.Rows[0]["Sys_Flag"].ToString(),GpsLoc=dtbl.Rows[0]["GpsLoc"].ToString(),MobileLoc=dtbl.Rows[0]["MobileLoc"].ToString()}
             }
             };
             Context.Response.Write(JsonConvert.SerializeObject(Person));
             // return JsonConvert.SerializeObject(cars);
         }
     }
      #endregion
    #region Common Msg
     public class Results
     {
         [DataMember]
         public List<Result> ResultList { get; set; }
     }
     [DataContract]
     public class Result
     {
         [DataMember]
         public string ResultMsg { get; set; }
       
     }
     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void InsertVersion(string version, Int64 CurrentDate, string DeviceNo)
     {
         int retVal = 0;
         string msg="";
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
             string Query = "insert into Version_Mast(Version,DeviceId,Created_Date) values ('" + version + "','" + DeviceNo + "','" + mDate + "')";
             retVal = DbConnectionDAL.GetIntScalarVal(Query);
                        
             // return JsonConvert.SerializeObject(cars);
         }
         
         catch (Exception ex) { retVal = 1; }
         if (retVal == 0)
             msg="Version Inserted";
         else
            msg="Version Not Inserted";

            Results rst = new Results
             {
                 ResultList = new List<Result>() {
        new Result { ResultMsg = msg}
             }
             };
             Context.Response.Write(JsonConvert.SerializeObject(rst));

     }

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void UpdateAddress(string Latitude, string Longitude, string Address)
     {
         string msg = "";
         string Query = "select 1 from Addresses where Latitude='" + Latitude.ToString().Substring(0, 7) + "' and Longitude='" + Longitude.ToString().Substring(0, 7) + "'";
         int val = DbConnectionDAL.GetIntScalarVal(Query);
         if (val > 0)
         {
             string updtqry = "Update Addresses set Address='" + Address.Trim() + "' where Latitude='" + Latitude.ToString().Substring(0, 7) + "' and Longitude='" + Longitude.ToString().Substring(0, 7) + "'";
             DbConnectionDAL.GetStringScalarVal(updtqry);
         }
         else
         {
             string instdtls = "Insert into Addresses (Latitude,Longitude,Address) values('" + Latitude.Trim() + "','" + Longitude.Trim() + "'," + Address + ")";
             DbConnectionDAL.GetIntScalarVal(instdtls);
         }
         int val1 = DbConnectionDAL.GetIntScalarVal(Query);
         if (val1 > 0) msg = "1";
         else msg="0";
         
         Results rst = new Results
         {
             ResultList = new List<Result>() {
        new Result { ResultMsg = msg}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
      }

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void InsertLogTracker(string DeviceNo, Int64 CurrentDate, string Status)
     {
         int retVal = 0;
         string msg = "";
         try
         {
             DateTime mDate = DateTime.Parse("1970-01-01");
             mDate = mDate.AddSeconds(CurrentDate + 19800);
             string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status) values ('" + DeviceNo + "','" + mDate + "','" + Status + "')";
             retVal = DbConnectionDAL.GetIntScalarVal(Query);
         }
         catch (Exception ex)
         {
             retVal = 1;
         }
         if (retVal == 0)
             msg = "Record Inserted";
         else
             msg = "Record Not Inserted";

         Results rst = new Results
         {
             ResultList = new List<Result>() {
        new Result { ResultMsg = msg}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
     }

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void InsertCIDDetail(string DeviceNo, string MCC, string MNC, string LAC, string CID, Int64 CurrentDate, string Log_m, string Battery, string Gps_accuracy)
     {
         int retVal = 0;
         string msg = "";
         string Latitude = string.Empty;
         string Longitude = string.Empty;
         try
         {
               DateTime mDate = DateTime.Parse("1970-01-01");
                mDate = mDate.AddSeconds(CurrentDate + 19800);
                string cdate = (mDate.ToString("yyyy-MM-dd HH:mm"));
                string createText = "";
            string[] s = GoogleMapsApi.GetLatLng(Convert.ToInt32(MCC.Trim()), Convert.ToInt32(MNC.Trim()), Convert.ToInt32(LAC.Trim()), Convert.ToInt32(CID.Trim()));
            if (!string.IsNullOrEmpty(s[0].ToString()) && !string.IsNullOrEmpty(s[1].ToString()))
            {
                Latitude = (s[0].ToString());
                Longitude = (s[1].ToString());


                createText = "" + DeviceNo + "," + MCC + "," + MNC + "," + LAC + "," + CID + "," + cdate + "," + Log_m + "," + Battery + "," +
                Gps_accuracy + "" + Environment.NewLine;


                string approx = "0";
                if (!string.IsNullOrEmpty(Gps_accuracy) && Gps_accuracy != "null")
                { approx = Gps_accuracy; }
                
                if (DbConnectionDAL.GetIntScalarVal("select 1 from LastActive where DeviceNo='" + DeviceNo + "'") <= 0)
                {
                    string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long)" +
                              "values ('" + DeviceNo + "','" + cdate + "','" + Latitude + "','" + Longitude + "')";
                    retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                }
                else
                {
                    DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'"
                        + " where DeviceNo='" + DeviceNo + "'");
                }
                WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                string Address = DMT.InsertAddress(Latitude, Longitude);
                if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                {
                    string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy)" +
                             "values ('" + DeviceNo + "','" + Latitude + "','" + Longitude + "','" + Address + "','" + cdate + "','C','" + Battery + "','" + 0 + "')";
                    retVal = DbConnectionDAL.GetIntScalarVal(Query);
                }
                //if (!string.IsNullOrEmpty(Address))
                //{
                //    if (DbConnectionDAL.GetIntScalarVal("select 1 from GAdd1 where Address='" + Address.Trim() + "'") <= 0)
                //    {
                //        DbConnectionDAL.GetIntScalarVal("Insert into GAdd1 (Address) values('" + Address.Trim() + "')");
                //        Int64 addId = DbConnectionDAL.GetIntScalarVal("select AddId from GAdd1 where Address='" + Address.Trim() + "'");

                //        if (addId > 0)
                //        {
                //            DbConnectionDAL.GetIntScalarVal("Insert into GAdd (Lat,Long,AddId) values('" + Latitude.Substring(0, 7) + "','" + Longitude.Substring(0, 7) + "'," + addId + ")");
                //        }
                //    }
                //    else
                //        if (DbConnectionDAL.GetIntScalarVal("select 1 from GAdd where Lat='" + Latitude.Substring(0, 7) + "' and Long='" + Longitude.Substring(0, 7) + "'") <= 0)
                //        {
                //            Int64 addId = DbConnectionDAL.GetIntScalarVal("select AddId from GAdd1 where Address='" + Address.Trim() + "'");
                //            if (addId > 0)
                //            {
                //                DbConnectionDAL.GetIntScalarVal("Insert into GAdd (Lat,Long,AddId) values('" + Latitude.Substring(0, 7) + "','" + Longitude.Substring(0, 7) + "'," + addId + ")");
                //            }
                //        }
                //}

            }
            else
            {
                retVal = 0;
                createText = "" + DeviceNo + "," + MCC + "," + MNC + "," + LAC + "," + CID + "," + cdate + "," + Log_m + "," + Battery + "," +
                Gps_accuracy + ",Location Not Found By Google" + Environment.NewLine;
            }
            using (System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFileCID.txt"), true))
            {
                TextFileCID.WriteLine(createText);
                TextFileCID.Close();
            }
         }
         catch (Exception ex)
         {
             retVal = 1;
         }
         if (retVal == 0)
            msg="Y";
         else
             msg = "N";

         Results rst = new Results
         {
             ResultList = new List<Result>() {
        new Result { ResultMsg = msg}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
     }

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void InsertDetail(string DeviceNo, string Latitude, string Longitude, Int64 CurrentDate, string Log_m, string Battery, string Gps_accuracy)
     {
         int retVal = 0;
         string msg = "";
         try
         {
             if (Log_m.ToUpper() != "C")
             {
                 DateTime mDate = DateTime.Parse("1970-01-01");
                 mDate = mDate.AddSeconds(CurrentDate + 19800);
                 string cdate = (mDate.ToString("yyyy-MM-dd HH:mm"));
                 string createText = "" + DeviceNo + "," + Latitude + "," + Longitude + "," + cdate + "," + Log_m + "," + Battery + "," +
                 Gps_accuracy + "" + Environment.NewLine;

                 
                 string approx = "0";
                 if (!string.IsNullOrEmpty(Gps_accuracy) && Gps_accuracy != "null")
                 { approx = Gps_accuracy; }
                  if (DbConnectionDAL.GetIntScalarVal("select 1 from LastActive where DeviceNo='" + DeviceNo + "'") <= 0)
                 {
                     string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long) values ('" + DeviceNo + "','" + cdate + "','" + Latitude + "','" + Longitude + "')";
                     retVal = DbConnectionDAL.GetIntScalarVal(insqry);
                 }
                 else
                 {
                     DbConnectionDAL.GetStringScalarVal("update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'"
                         + " where DeviceNo='" + DeviceNo + "'");
                 }
                  WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                 string Address = DMT.InsertAddress(Latitude, Longitude);
                 if (DbConnectionDAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                 {
                     string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy)" +
                              "values ('" + DeviceNo + "','" + Latitude + "','" + Longitude + "','" + Address + "','" + cdate + "','G','" + Battery + "','" + approx + "')";
                     retVal = DbConnectionDAL.GetIntScalarVal(Query);
                 }
                 using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/TextFile.txt"), true))
                 {
                     file2.WriteLine(createText);
                     file2.Close();
                 }
             }
             
         }
         catch (Exception ex)
         {
             retVal = 1;
         }
         if (retVal == 0)
             msg = "Y";
         else
             msg = "N";

         Results rst = new Results
         {
             ResultList = new List<Result>() {
        new Result { ResultMsg = msg}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
     }

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void ReSendActivationMail(String DeviceNo)
     {
         string status = "";
         try
         {
             string str = "select Name,Email from Temp_Reg Where DeviceID='" + DeviceNo + "'";
             DataTable dtbl = DbConnectionDAL.getFromDataTable(str);
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
         Results rst = new Results
         {
             ResultList = new List<Result>() {
        new Result { ResultMsg = status}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
     }

     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
     public void GetRegistrationDetail(string Name, string MobileNo, string Email, string Password, string DeviceID)
     {
         string Query = "select case(select 1 from Temp_Reg where deviceid='" + DeviceID + "') when 1 then 'Y' else 'N' end as DataInserted";
         string date11 = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("yyyy-MM-dd");

         string str = "INSERT INTO Temp_Reg (Name,Mobile,Email,Pwd,DeviceId,Created_Date) Values ('" + Name + "','" + MobileNo + "','" + Email + "','" + Password + "','" + DeviceID + "','" + Convert.ToDateTime(date11) + "')";
         string retval = DbConnectionDAL.GetStringScalarVal(str);
         if (retval != "-1")
         {
             SendEmail(Email, Name, DeviceID);
         }
         DataTable dtr = new DataTable(); dtr =DbConnectionDAL.getFromDataTable(Query);
         Results rst = new Results
         {
             ResultList = new List<Result>() {
        new Result { ResultMsg = dtr.Rows[0]["DataInserted"].ToString()}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
     }


    #endregion
    #region Url
     public class UrlExist
     {
         [DataMember]
         public List<Url> UrlExistList { get; set; }
     }
     [DataContract]
     public class Url
     {
         [DataMember]
         public string UrlMsg { get; set; }

     }
     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    
     public void GetValidUser(String DeviceNo)
     { string str = "select case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "') when 1 then " +
             " (select case (select 1 from LineMaster where (Validdate < dateadd(ss,19800,getutcdate())) and  DeviceId='" + DeviceNo + "') " +
             " when 1 then 'EX' else (select case When (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "') IS NULL then 'N' else " +
             " (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "') end) end) " +
             " else 'N' End as URL";
         DataTable dt = new DataTable();
         dt = DbConnectionDAL.getFromDataTableDmLicence(str);
         UrlExist rst = new UrlExist
         {
             UrlExistList = new List<Url>() {
        new Url { UrlMsg = dt.Rows[0]["URL"].ToString()}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
     }

    #endregion
    #region UserExist
     public class UserExist
     {
         [DataMember]
         public List<Exist> UserExistList { get; set; }
     }
     [DataContract]
     public class Exist
     {
         [DataMember]
         public string ExistMsg { get; set; }

     }
     [WebMethod]
     [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    
     public void GetValidUserInDemoDB(String DeviceNo)
     {
         string str = "select case when(select count(*) from Temp_Reg)  >  0 then (select case (select 1 from Temp_Reg WHERE DeviceId='" + DeviceNo + "') when 1 then 'Y' else 'N' end) else 'N' end As UserExist";
         DataTable dt = new DataTable();
         dt = DbConnectionDAL.getFromDataTable(str);
         UserExist rst = new UserExist
         {
             UserExistList = new List<Exist>()
             {
                new Exist { ExistMsg = dt.Rows[0]["UserExist"].ToString()}
             }
         };
         Context.Response.Write(JsonConvert.SerializeObject(rst));
     }

     #endregion
    #endregion

    #endregion
}
