using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Net.Mail;
using System.Text;
using DAL;
using BAL;
using AstralFFMS.ServiceReferenceDMTracker;
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
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
    [WebMethod]
    public string InsertVersion(string version, Int64 CurrentDate, string DeviceNo)
    {
        int retVal = 0;
        string retInfo = "";
        try
        {
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            string strexist = "select 1 from Version_Mast where DeviceId ='" + DeviceNo + "'";
            int gtval =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,strexist));// DataAccessLayer.DAL.GetIntScalarVal(strexist);
            if (gtval > 0) {
                DbConnectionDAL.GetScalarValue(CommandType.Text, "delete from Version_Mast where DeviceId ='" + DeviceNo + "'");// DataAccessLayer.DAL.GetIntScalarVal("delete from Version_Mast where DeviceId ='" + DeviceNo + "'");
            }
            string Query = "insert into Version_Mast(Version,DeviceId,Created_Date)" +
                           "values ('" + version + "','" + DeviceNo + "','" + mDate + "')";

            retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
        }
        catch (Exception ex)
        {
            retVal = 1;
        }
        if (retVal == 0)
            retInfo = "Version Inserted";
        else
            retInfo = "Version Not Inserted";
        return retInfo;
    }

    [WebMethod]
    public int UpdateAddress(string Latitude, string Longitude, string Address)
    {

        string Query = "select 1 from Addresses where Latitude='" + Latitude.ToString().Substring(0, 7) + "' and Longitude='" + Longitude.ToString().Substring(0, 7) + "'";
        int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
        if (val > 0)
        {
            string updtqry = "Update Addresses set Address='" + Address.Trim() + "' where Latitude='" + Latitude.ToString().Substring(0, 7) + "' and Longitude='" + Longitude.ToString().Substring(0, 7) + "'";
            Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, updtqry)); //DataAccessLayer.DAL.GetStringScalarVal(updtqry);
        }
        else
        {

            string instdtls = "Insert into Addresses (Latitude,Longitude,Address) values('" + Latitude.Trim() + "','" + Longitude.Trim() + "'," + Address + ")";
            Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, instdtls)); //DataAccessLayer.DAL.GetIntScalarVal(instdtls);
        }
        int val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query)); //DataAccessLayer.DAL.GetIntScalarVal(Query);
        if (val1 > 0) return 1;
        else return 0;
    }
    [WebMethod]
    public void FillEmptyAddress(string FromDate, string ToDate)
    {

        string ToDate1 = Convert.ToDateTime(ToDate).ToShortDateString();
        string FromDate1 = Convert.ToDateTime(FromDate).ToShortDateString();
        string Query = "select * from LocationDetails where (description='' or description is null) and vdate >='" + FromDate1 + "'  and vdate <='" + ToDate1 + "'";
        DataTable dtaddr = new DataTable();
        dtaddr = DbConnectionDAL.getFromDataTable(Query);
        if (dtaddr.Rows.Count > 0)
        {
            string gaddress = "";
            for (int i = 0; i < dtaddr.Rows.Count; i++)
            {
                WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                gaddress = DMT.InsertAddress(dtaddr.Rows[i]["Latitude"].ToString(), dtaddr.Rows[i]["Longitude"].ToString());
                if (!string.IsNullOrEmpty(gaddress))
                {
                    string UpdtGaddress = "update locationdetails set description='" + gaddress + "' where id ='" + dtaddr.Rows[i]["Id"] + "'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text,UpdtGaddress); //DataAccessLayer.DAL.ExecuteQuery(UpdtGaddress);
                }
            }
        }
    }
    [WebMethod]
    public XmlDocument GetPersonDetail(string DeviceNo)
    {
        string Query = "select PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc from PersonMaster where DeviceNo='" + DeviceNo + "'";
        return getXML(Query, "Detail", "GetDetail",getConStr);
    }
    [WebMethod]
    public XmlDocument GetGroupMobileNo(string PersonID)
    {
        string str = "SELECT mobile FROM GroupMaster INNER JOIN GrpMapp ON GroupMaster.GroupID=GrpMapp.GroupID WHERE PersonID=" + PersonID + "";
        return getXML(str, "Group", "MobileNo", getConStr);
    }
    [WebMethod]
    public XmlDocument GetPersonAlarmSMS(string DeviceNo)
    {
        string Query = "select Alarm,AlarmDurationMins,SendSMS from PersonMaster where DeviceNo='" + DeviceNo + "'";
        return getXML(Query, "Detail", "GetDetail", getConStr);
    }
    [WebMethod]
    public string InsertLogTracker(string DeviceNo, Int64 CurrentDate, string Status)
    {
        int retVal = 0;
        string retInfo = "";
        try
        {  
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from Log_Tracker where CurrentDate='{0}' AND Status='{1}'", mDate, Status)))<= 0)// DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from Log_Tracker where CurrentDate='{0}' AND Status='{1}'", mDate, Status)) <= 0)
            {
                string Query = "insert into Log_Tracker(DeviceNo,CurrentDate,Status,vdate,slot) values ('" + DeviceNo + "','" + mDate + "','" + Status + "','" + mDate + "',DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, '" + mDate + "'), 0), '" + mDate + "'))";
                retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
            }
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
    public XmlDocument InsertCIDDetail(string DeviceNo,string MCC,string MNC,string LAC,string CID, Int64 CurrentDate, string Log_m, string Battery, string Gps_accuracy)
    {
        int retVal = 0;
        string Latitude = string.Empty;
        string Longitude = string.Empty;    
        try
        {
            DateTime mDate = DateTime.Parse("1970-01-01");
            mDate = mDate.AddSeconds(CurrentDate + 19800);
            string cdate = (mDate.ToString("yyyy-MM-dd HH:mm"));
            string createText = "";
      
                string[] s = GoogleMapsApi.GetLatLng(Convert.ToInt32(MCC.Trim()), Convert.ToInt32(MNC.Trim()), Convert.ToInt32(LAC.Trim()),Convert.ToInt32(CID.Trim()));
                if (!string.IsNullOrEmpty(s[0].ToString()) && !string.IsNullOrEmpty(s[1].ToString()))
                {
                    Latitude = (s[0].ToString());
                    Longitude = (s[1].ToString());

                    createText = "" + DeviceNo + "," + MCC + "," + MNC + "," + LAC + "," + CID + "," + cdate + "," + Log_m + "," + Battery + "," +
                    Gps_accuracy + "" + Environment.NewLine;


                    string approx = "0";
                    if (!string.IsNullOrEmpty(Gps_accuracy) && Gps_accuracy != "null")
                    { approx = Gps_accuracy; }
                  
                   
                    if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,"select 1 from LastActive where DeviceNo='" + DeviceNo + "'")) <= 0)// DataAccessLayer.DAL.GetIntScalarVal("select 1 from LastActive where DeviceNo='" + DeviceNo + "'") <= 0)
                    {
                        string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long)" +
                                  "values ('" + DeviceNo + "','" + cdate + "','" + Latitude + "','" + Longitude + "')";
                        retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, insqry));// DataAccessLayer.DAL.GetIntScalarVal(insqry);
                    }
                    else
                    {
                        DbConnectionDAL.GetScalarValue(CommandType.Text, "update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'" + " where DeviceNo='" + DeviceNo + "'"); 
                        //DataAccessLayer.DAL.GetStringScalarVal("update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'"
                       //     + " where DeviceNo='" + DeviceNo + "'");
                    }
                    WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                    string Address = DMT.InsertAddress(Latitude, Longitude);
                    if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)))<= 0)//  DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
                    {
                        string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy)" +
                                 "values ('" + DeviceNo + "','" + Latitude + "','" + Longitude + "','" + Address + "','" + cdate + "','C','" + Battery + "','" + 0 + "')";
                        retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
                    }
                    //if (!string.IsNullOrEmpty(Address))
                    //{
                    //    if (DataAccessLayer.DAL.GetIntScalarVal("select 1 from GAdd1 where Address='" + Address.Trim() + "'") <= 0)
                    //    {
                    //        DataAccessLayer.DAL.GetIntScalarVal("Insert into GAdd1 (Address) values('" + Address.Trim() + "')");
                    //        Int64 addId = DataAccessLayer.DAL.GetIntScalarVal("select AddId from GAdd1 where Address='" + Address.Trim() + "'");

                    //        if (addId > 0)
                    //        {
                    //            DataAccessLayer.DAL.GetIntScalarVal("Insert into GAdd (Lat,Long,AddId) values('" + Latitude.Substring(0, 7) + "','" + Longitude.Substring(0, 7) + "'," + addId + ")");
                    //        }
                    //    }
                    //    else
                    //        if (DataAccessLayer.DAL.GetIntScalarVal("select 1 from GAdd where Lat='" + Latitude.Substring(0, 7) + "' and Long='" + Longitude.Substring(0, 7) + "'") <= 0)
                    //        {
                    //            Int64 addId = DataAccessLayer.DAL.GetIntScalarVal("select AddId from GAdd1 where Address='" + Address.Trim() + "'");
                    //            if (addId > 0)
                    //            {
                    //                DataAccessLayer.DAL.GetIntScalarVal("Insert into GAdd (Lat,Long,AddId) values('" + Latitude.Substring(0, 7) + "','" + Longitude.Substring(0, 7) + "'," + addId + ")");
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
            return getXMLForSingleMsg("Y");
        else
            return getXMLForSingleMsg("N");
    }

    [WebMethod]
    public XmlDocument InsertDetail(string DeviceNo, string Latitude, string Longitude, Int64 CurrentDate, string Log_m, string Battery, string Gps_accuracy)
    {
       int retVal = 0;

       try
       {
           if (Log_m.ToUpper() !="C")
           {
               DateTime mDate = DateTime.Parse("1970-01-01");
               mDate = mDate.AddSeconds(CurrentDate + 19800);
               string cdate = (mDate.ToString("yyyy-MM-dd HH:mm"));
               string createText = "" + DeviceNo + "," + Latitude + "," + Longitude + "," + cdate + "," + Log_m + "," + Battery + "," +
               Gps_accuracy + "" + Environment.NewLine;

              
               string approx = "0";
               if (!string.IsNullOrEmpty(Gps_accuracy) && Gps_accuracy != "null")
               { approx = Gps_accuracy; }
             
             
               if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,"select 1 from LastActive where DeviceNo='" + DeviceNo + "'"))<= 0)// DataAccessLayer.DAL.GetIntScalarVal("select 1 from LastActive where DeviceNo='" + DeviceNo + "'") <= 0)
               {
                   string insqry = "insert into LastActive(DeviceNo,CurrentDate,Lat,Long)" +
                             "values ('" + DeviceNo + "','" + cdate + "','" + Latitude + "','" + Longitude + "')";
                   retVal = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, insqry));// DataAccessLayer.DAL.GetIntScalarVal(insqry);
               }
               else
               {
                  DbConnectionDAL.GetScalarValue(CommandType.Text,"update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'"  + " where DeviceNo='" + DeviceNo + "'");
                   //DataAccessLayer.DAL.GetStringScalarVal("update LastActive set Lat='" + Latitude + "',Long='" + Longitude + "',CurrentDate='" + cdate + "'"
                   //    + " where DeviceNo='" + DeviceNo + "'");
               }

               WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
               string Address = DMT.InsertAddress(Latitude, Longitude);
               if (Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo))) <= 0)//  DataAccessLayer.DAL.GetIntScalarVal(string.Format("select 1 from LocationDetails where CurrentDate='{0}' AND DeviceNo='{1}'", cdate, DeviceNo)) <= 0)
               {
                   string Query = "insert into LocationDetails(DeviceNo,Latitude,Longitude,Description,CurrentDate,Log_m,Battery,Gps_accuracy)" +
                            "values ('" + DeviceNo + "','" + Latitude + "','" + Longitude + "','" + Address + "','" + cdate + "','G','" + Battery + "','" + approx + "')";
                   retVal =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text,Query));// DataAccessLayer.DAL.GetIntScalarVal(Query);
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
            return getXMLForSingleMsg("Y");
        else
            return getXMLForSingleMsg("N");
    }

    #region Andriod Tracker

    [WebMethod]
    public XmlDocument GetValidUser(String DeviceNo)
    {
        //string str = "select (case when URL IS NULL then 'N' else URL End) As URL from LineMaster WHERE DeviceId='" + DeviceNo.Trim() + "'";
        string str = "select case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') when 1 then " +
" (select case (select 1 from LineMaster where (Validdate < dateadd(ss,19800,getutcdate())) and  DeviceId='" + DeviceNo + "' and Product='TRACKER') " +
" when 1 then 'EX' else (select case When (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') IS NULL then 'N' else " +
   " (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') end) end) " +
  " else 'N' End as URL";

        return getXML(str, "ConnectionString", "GetUserDetail",getConStrDmLicense);
    }
    [WebMethod]
    public XmlDocument GetNewValidUser(String DeviceNo)
    {
        string str = "select (SELECT isnull(ShowFence,'N') FROM CompMaster WHERE CompCode=(SELECT CompCode FROM LineMaster WHERE "+ " DeviceId='"+DeviceNo+"')"+
")as ShowFence, case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') when 1 then " +
" (select case (select 1 from LineMaster where (Validdate < dateadd(ss,19800,getutcdate())) and  DeviceId='" + DeviceNo + "' and Product='TRACKER') " +
" when 1 then 'EX' else (select case When (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') IS NULL then 'N' else " +
   " (select Url from LineMaster WHERE DeviceId='" + DeviceNo + "' and Product='TRACKER') end) end) " +
  " else 'N' End as URL";

        return getXML(str, "ConnectionString", "GetUserDetail", getConStrDmLicense);
    }

    [WebMethod]
    public XmlDocument GetValidUserInDemoDB(String DeviceNo)
    {
        string str = "select case when(select count(*) from Temp_Reg)  >  0 then (select case (select 1 from Temp_Reg WHERE DeviceId='"+DeviceNo+"') when 1 then 'Y' else 'N' end) else 'N' end As UserExist";
        return getXML(str, "ValidUser", "GetUserDetailfrmDemoDB", getConStr);
    }

    [WebMethod]
    public XmlDocument ReSendActivationMail(String DeviceNo)
    {
      string status = "";
      try
       {
        string str = "select Name,Email from Temp_Reg Where DeviceID='"+ DeviceNo +"'";
        DataTable dtbl= getFromDataTable(str, getConStr);
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
        return getXMLForSingleMsg(status);
    }

    [WebMethod]
    public XmlDocument GetRegistrationDetail(string Name, string MobileNo, string Email, string Password, string DeviceID)
    {
        string Query = "select case(select 1 from Temp_Reg where deviceid='" + DeviceID + "') when 1 then 'Y' else 'N' end as DataInserted";
        string date11 = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("yyyy-MM-dd");

        string str = "INSERT INTO Temp_Reg (Name,Mobile,Email,Pwd,DeviceId,Created_Date) Values ('" + Name + "','" + MobileNo + "','" + Email + "','" + Password + "','" + DeviceID + "','" + Convert.ToDateTime(date11) + "')";
        string retval = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));// DataAccessLayer.DAL.GetStringScalarVal(str);
        if (retval != "-1")
        {
            SendEmail(Email, Name,DeviceID);
        }
        return getXML(Query, "Detail1", "GetRegistration",getConStr);
    }


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

        catch (Exception ex) { ex.ToString();  }
    }

     [WebMethod]
    public XmlDocument SendConfirmationMail(string Email, string Name)
    {
        try {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table style='width:100%; color: #000000; font: 12px/17px Verdana,Arial,Helvetica,sans-serif;'><tr><td colspan='2' style='width: 150px;'><strong>Welcome to Dataman Tracker Services Account!</strong></td></tr><tr>");
            sb.AppendLine("<td>Dear " + Name + ", </td></tr><tr style='height:30px;'>");
            sb.AppendLine("<td>Thank you for verifying your email ID</td></tr><tr>");
            sb.AppendLine("<td>Regards</td></tr><tr>");
            sb.AppendLine("<td>Dataman Web Team</td></tr>");
            //sb.AppendLine("<td style='width: 250px;'>Approval remarks</td><td style='width: 300px;'>" + dtb.Rows[0]["Appr_Remarks_latest"].ToString() + "</td></tr><tr>");
            sb.AppendLine("</table>");
            SendMailMessage("DmTracker@dataman.in", Email, "", "", "Your Email ID is now verified With Dataman Tracker! ", sb.ToString().Trim());
            return getXMLForSingleMsg("Confirmation Mail Sent");
        }
        catch (Exception ex) { ex.ToString(); return getXMLForSingleMsg("Confirmation Mail Not Sent"); }
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
        catch (Exception ex) { ex.ToString();  }
    }
    #endregion
    #endregion
    public static DataTable getFromDataTable(string command,string constr)
    {
        SqlConnection con = new SqlConnection(constr);
        SqlDataAdapter da = new SqlDataAdapter(command, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }
    public static XmlDocument getXMLForCaseMsg(string msg)
    {
        string tblnm = "GetStatus";
        string dsnm = "GetVal";
        DataTable dtbl = new DataTable();
        dtbl.Columns.Add("Status");
        DataRow dr = dtbl.NewRow();
        dr["Status"] = msg;
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
        //nDate.Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        XmlElement y = xd.DocumentElement;
        XmlNode x = xd.GetElementsByTagName(tblnm)[0];
        y.InsertBefore(nDate, x);

        //XmlNode y = xd.GetElementsByTagName(dsnm)[0].ChildNodes[1];
        //y.InnerText = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

        sw.Close();
        xtw.Close();
        dds.Clear();
        dtbl.Clear();
        return xd;
    }
    public static XmlDocument getXMLForSingleMsg(string msg)
    {
        string tblnm = "GetStatus";
        string dsnm = "GetMailStatus";
        DataTable dtbl = new DataTable();
        dtbl.Columns.Add("MailStatus");
        DataRow dr= dtbl.NewRow();
        dr["MailStatus"] = msg;
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
        //nDate.Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        XmlElement y = xd.DocumentElement;
        XmlNode x = xd.GetElementsByTagName(tblnm)[0];
        y.InsertBefore(nDate, x);

        //XmlNode y = xd.GetElementsByTagName(dsnm)[0].ChildNodes[1];
        //y.InnerText = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

        sw.Close();
        xtw.Close();
        dds.Clear();
        dtbl.Clear();
        return xd;
    }
    public static XmlDocument getXML(string Query, string tblnm, string dsnm,string constr)
    {
        DataTable dst = getFromDataTable(Query,constr);
        DataSet dds = new DataSet();
        dds.Tables.Add(dst);
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
        //nDate.Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        XmlElement y = xd.DocumentElement;
        XmlNode x = xd.GetElementsByTagName(tblnm)[0];
        y.InsertBefore(nDate, x);

        //XmlNode y = xd.GetElementsByTagName(dsnm)[0].ChildNodes[1];
        //y.InnerText = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

        sw.Close();
        xtw.Close();
        dds.Clear();
        dst.Clear();
        return xd;
    }

}


