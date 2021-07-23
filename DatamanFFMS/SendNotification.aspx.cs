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
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace AstralFFMS
{
    public partial class SendNotification : System.Web.UI.Page
    {
        BAL.Uploads.UploadBAL up = new BAL.Uploads.UploadBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
        }

        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
        private int Save(string path,string forwhom)
        {
            string docID = Settings.GetDocID("VISSN", DateTime.Now);
            Settings.SetDocID("VISSN", docID);
            int Retsave = up.Insert(DateTime.Now.ToString(), docID, txtMessage.Text, path,true, forwhom, Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(Settings.Instance.UserID), "","");
            return Retsave;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string value = string.Empty; string path = string.Empty;
            try
            {
                int tosmid = 0, userid = 0;
                int distid = 0;
                String varname1 = "";
                string displaytitle = "",notificationtype = "";     
                displaytitle = txtMessage.Text;
                string msgUrl = "";

                string CompanyCode = DbConnectionDAL.GetStringScalarVal("select CompCode from mastenviro").ToString();
               
                if(ddlmsgType.SelectedValue == "1")
                { notificationtype = "GENERALNOTIFICATION"; }
                if(ddlmsgType.SelectedValue == "2")
                { notificationtype = "MOBILEAPPUPDATE"; }
                if(ddlmsgType.SelectedValue == "3")
                { notificationtype = "Collateral"; }
                string host = DbConnectionDAL.GetStringScalarVal("select compurl from mastenviro").ToString();
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (ddlCategory.SelectedValue == "1" )              
                {
                   // value = PushNotificationForLakshya(displaytitle, CompanyCode);
                    string Query = "Select * from Mastparty where partydist=1 ORDER BY Partyid";
                    //string Query = "Select * from Mastparty where partydist=1 and mobile='7906767390' ORDER BY Partyid";
                    DataTable dt = new DataTable();
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            distid = Convert.ToInt32(dt.Rows[i]["Partyid"]);
                            userid = Convert.ToInt32(dt.Rows[i]["UserId"]);
                            msgUrl = "SendNotification.aspx?DistId=" + distid + "";
                            varname1 = "INSERT INTO TransNotification (pro_id, userid, VDate, msgURL, displayTitle, Status,FromUserId, DistId) VALUES ('" + notificationtype + "', " + userid + ", getdate(),'" + msgUrl + "', N'" + displaytitle + "', 0, " + Settings.Instance.UserID + ", " + distid + ")";
                            DAL.DbConnectionDAL.ExecuteQuery(varname1);
                           
                            if (FileUpload1.PostedFile != null & notificationtype == "Collateral")
                            {
                                try
                                {
                                    //int retsave = Save("~/" + FileUpload1.FileName, "Distributor");
                                    //if (retsave > 0)
                                    //{
                                        string strDestPath = Server.MapPath("~/UploadDocuments/" + timeStamp + "-" + FileUpload1.FileName);
                                        FileUpload1.PostedFile.SaveAs(strDestPath);

                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('File Uploaded Successfully');", true);

                                        path = "http://" + host + ("/UploadDocuments/" + timeStamp + "-" + FileUpload1.FileName);
                                    //}
                                    //else
                                    //{
                                    //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + FileUpload1.FileName + "');", true);
                                    //}

                                }
                                catch (Exception ex)
                                {
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + FileUpload1.FileName + "');", true);
                                }
                            }
                        }
                    }

                    value = PushNotificationForLakshya(displaytitle, CompanyCode, path);
                }

                if (ddlCategory.SelectedValue == "2" )              
                {
                    //string Query = "Select * from Mastsalesrep where Mobile='7906767390' ORDER BY SMId";                  
                    string Query = "Select * from Mastsalesrep where smname <> '.' ORDER BY SMId";
                    DataTable dt = new DataTable();
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tosmid = Convert.ToInt32(dt.Rows[i]["SMID"]);
                            userid = Convert.ToInt32(dt.Rows[i]["UserId"]);
                            msgUrl = "SendNotification.aspx?SMId=" + Settings.Instance.SMID + "";
                            varname1 = "INSERT INTO TransNotification (pro_id, userid, VDate, msgURL, displayTitle, Status, FromUserId, SMId, ToSMId) VALUES ('" + notificationtype + "', " + userid + ", getdate(),'" + msgUrl + "', N'" + displaytitle + "', 0, " + Settings.Instance.UserID + ", " + Settings.Instance.SMID + ", " + tosmid + ")";
                            DAL.DbConnectionDAL.ExecuteQuery(varname1);
                        }

                    }
                    if (FileUpload1.PostedFile != null & notificationtype == "Collateral")
                    {
                        try
                        {
                            //int retsave = Save("~/" + FileUpload1.FileName, "Sales Team");
                            //if (retsave > 0)
                            //{
                            string strDestPath = Server.MapPath("~/UploadDocuments/" + timeStamp + "-" + FileUpload1.FileName);
                                FileUpload1.PostedFile.SaveAs(strDestPath);

                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('File Uploaded Successfully');", true);
                                //ClearControls();
                               

                                path = "http://" + host + ("/UploadDocuments/" + timeStamp + "-" + FileUpload1.FileName);
                            //}
                            //else
                            //{
                            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + FileUpload1.FileName + "');", true);
                            //}

                        }
                        catch (Exception ex)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + FileUpload1.FileName + "');", true);
                        }
                    }
                    value = PushNotificationForLakshya_MKT(displaytitle, CompanyCode,path);
                }
                if (ddlCategory.SelectedValue == "3")
                {
                    //string Query = "Select * from Mastsalesrep where deviceno='911531150035766' ORDER BY SMId";
                    
                    string Query = "Select * from Mastsalesrep where smname <> '.' ORDER BY SMId";
                    DataTable dt = new DataTable();
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tosmid = Convert.ToInt32(dt.Rows[i]["SMID"]);
                            userid = Convert.ToInt32(dt.Rows[i]["UserId"]);
                            msgUrl = "SendNotification.aspx?SMId=" + Settings.Instance.SMID + "";
                            varname1 = "INSERT INTO TransNotification (pro_id, userid, VDate, msgURL, displayTitle, Status, FromUserId, SMId, ToSMId) VALUES ('" + notificationtype + "', " + userid + ", getdate(),'" + msgUrl + "', N'" + displaytitle + "', 0, " + Settings.Instance.UserID + ", " + Settings.Instance.SMID + ", " + tosmid + ")";
                            DAL.DbConnectionDAL.ExecuteQuery(varname1);
                        }
                    }
                    if (FileUpload1.PostedFile != null & notificationtype == "Collateral")
                    {
                        try
                        {
                            //int retsave = Save("~/" + FileUpload1.FileName, "Sales Team");
                            //if (retsave > 0)
                            //{
                                string strDestPath = Server.MapPath("~/UploadDocuments/" + timeStamp + "-" + FileUpload1.FileName);
                                FileUpload1.PostedFile.SaveAs(strDestPath);

                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('File Uploaded Successfully');", true);
                                //ClearControls();

                                path = "http://" + host + ("/UploadDocuments/" + timeStamp + "-" + FileUpload1.FileName);
                            //}
                            //else
                            //{
                            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + FileUpload1.FileName + "');", true);
                            //}

                        }
                        catch (Exception ex)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while uploading the " + FileUpload1.FileName + "');", true);
                        }   
                    }
                    value = PushNotificationForLakshya_FFMS(displaytitle, CompanyCode, path);
                }    
                
                ClearControls();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Notification Sent Successfully');", true);
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while Notification Sent');", true);
            }
        }
        public class Notifi
        {
        
            string image { get; set; }
        } 
        public string PushNotificationForLakshya(string msg, string compcode,string imgurl)
        {
            var result = "-1";
            string Query = "Select Mobile as Deviceno from MastParty WHERE PartyDist=1";
            //string Query = "Select Mobile as Deviceno from MastParty WHERE PartyDist=1 and mobile='7906767390'";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            if (dt.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceNo = dt.Rows[i]["Deviceno"].ToString();                      
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            string regid_query = "select Reg_id from LineMaster where Mobile='" + DeviceNo + "' and Upper(Product)='GOLDIEE' and CompCode='" + compcode + "'";
                            string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";

                            string Query1 = "select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='GOLDIEE' AND Active ='Y'";
                            string us = DbConnectionDAL.GetStringScalarVal(Query1);

                            SqlConnection cn = new SqlConnection(constrDmLicense);
                            SqlCommand cmd = new SqlCommand(regid_query, cn);
                            SqlCommand cmd1 = new SqlCommand(Query1, cn);
                            cmd.CommandType = CommandType.Text;
                            cmd1.CommandType = CommandType.Text;
                            cn.Open();
                            string regId = cmd.ExecuteScalar() as string;
                            string licenceinfo = cmd1.ExecuteScalar() as string;
                            cn.Close();
                            cmd = null;
                            if (!string.IsNullOrEmpty(regId))
                            {
                                 Notifi notification =new Notifi();
                                string serverKey = "AAAAzCuqvwg:APA91bHcJehTm7IFNeWmx85d73c8oTtHtmCBPLPoxzTO3G7ua-IyCz9lxlEDIZdWWVZkhkJg1kWEK9C2m2V6K6z7lOgg_sm6iZwP1NMiEnFGifYPFpXcJK0wb2nQM7JEY-8P3tw4tRZpUch3l-eZqMVQVc0DObThnw";//s
                                string senderId = "876905938696";
                                string webAddr = "https://fcm.googleapis.com/fcm/send";
                                string title = "Lakshya";
                                //var result = "-1";
                                var tRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                                tRequest.ContentType = "application/json";
                                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                                tRequest.Method = "POST";
                                var payload = new
                                {
                                    to = regId,
                                    priority = "high",
                                    content_available = true,
                                    image = "http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg",
                                    //notification = new
                                    //{
                                    //    body = msg,
                                    //    title = title,
                                    //    image = "http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg"
                                    //},
                                    data = new
                                    {
                                        body = msg,
                                        title = title,
                                        image = imgurl,//"http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg"
                                        msg = msg
                                    }
                                };

                                var serializer = new JavaScriptSerializer();
                                using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                                {
                                    string json = serializer.Serialize(payload);
                               
                                    streamWriter.Write(json);
                                    streamWriter.Flush();
                                }

                                var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    result = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = "N";
                }
            }
            return result;
        }

        [DataContract]
        public class NewNotification
        {
            [DataMember]
            public string msg { get; set; }
            [DataMember]
            public string image { get; set; }
     

        }
        public string PushNotificationForLakshya1(string msg,string imgurl)
        {
            NewNotification rst = new NewNotification();
            string Delivered = "N";
            try
            {
                string deviceId = "fqCKz8TC78s:APA91bGfbOp5MbUtvxmhQh5Jloqtt-gysPWZzOfPdcO_cxrYgH8DV5wk5aw9oBR5yvlK9ENQyQTrfdbh4s2tzJVFFY1VD2vuFUanHl8hXniQR1_cKpeIMQ1NLDAVotOJOnyEMNDIT2Mv";

                //  string applicationID = "AAAAntdgxq8:APA91bHnndECIcsis8Z3LIT8QJwhcdLkyxJm4fg27n-pr3nu2FQLAekJEyDbIGixfuR1d67UzxD3YiSuCmunWeVp4vrcGPpiR111p41L8pOe2MH0FL09CfR8Y3FjETr_6u-0jSE6xEuf"; 

                //     string senderId = "682218276527"; chat sever
                string applicationID = "AAAAzCuqvwg:APA91bHcJehTm7IFNeWmx85d73c8oTtHtmCBPLPoxzTO3G7ua-IyCz9lxlEDIZdWWVZkhkJg1kWEK9C2m2V6K6z7lOgg_sm6iZwP1NMiEnFGifYPFpXcJK0wb2nQM7JEY-8P3tw4tRZpUch3l-eZqMVQVc0DObThnw";//s
                string senderId = "876905938696"; //dmcrm senderid
                string title = "You've Message From " ;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
               
                string qq="msz:123,image:http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg";
                 string[] aa=qq.Split(',');
                //aa[0]="msz:123";
                //aa[1]="image:http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg";

                 rst.msg = msg;
                 rst.image = "http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg";

                var payload = new
                {
                    to = deviceId,
                    priority = "high",
                    content_available = true,
                    image = "http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg",
                    //notification = new
                    //{
                    //    body = rst,
                    //    title = "Test message",
                    //    icon = "http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg"
                    //},
                    data = new
                    {
                        body = msg,
                        title = "Test message",
                        image = imgurl,//"http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg"
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(payload);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                // tRequest.ContentLength = json.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                                Delivered = "Y";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return Delivered;
        }
        public string PushNotificationForLakshya_MKT(string msg, string compcode, string imgurl)
        {       
            var result = "-1";
            //string Query = "SELECT * FROM MastSalesRep WHERE Mobile='7906767390'";
              string Query = "Select Deviceno from Mastsalesrep";       
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            if (dt.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceNo = dt.Rows[i]["Deviceno"].ToString();
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            string regid_query = "select Reg_id from LineMaster where deviceid='" + DeviceNo + "' and Upper(Product)='GOLDIEE SALES' and CompCode='" + compcode + "'";
                            string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";

                            string Query1 = "select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='GOLDIEE SALES' AND Active ='Y'";                         
                            string us = DbConnectionDAL.GetStringScalarVal(Query1);

                            SqlConnection cn = new SqlConnection(constrDmLicense);
                            SqlCommand cmd = new SqlCommand(regid_query, cn);
                            SqlCommand cmd1 = new SqlCommand(Query1, cn);
                            cmd.CommandType = CommandType.Text;
                            cmd1.CommandType = CommandType.Text;
                            cn.Open();
                            string regId = cmd.ExecuteScalar() as string;
                            string licenceinfo = cmd1.ExecuteScalar() as string;
                            cn.Close();
                            cmd = null;
                            if (!string.IsNullOrEmpty(regId))
                            {
                                string serverKey = "AAAAGmgBKmE:APA91bGhywq0On9VncehIFPDorXSe59jP4rC-asBGLlnObDf2kF79_GRV3zf9zplDZ_Vyn8SNbr1UFIPM9Fb4bjy-a-Lx70BjQOmsJcRA5BINxTi15W8sANIXALjwaDN6l0nex919eJI9s_C4q46aYpa3feESG2TOg";//s
                                string senderId = "113414056545";
                                string webAddr = "https://fcm.googleapis.com/fcm/send";
                                string title = "Lakshya";
                                //var result = "-1";
                                var tRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                                tRequest.ContentType = "application/json";
                                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                                tRequest.Method = "POST";
                                var payload = new
                                {
                                    to = regId,
                                    priority = "high",
                                    content_available = true,
                                    //notification = new
                                    //{
                                    //    body = msg,
                                    //    title = title
                                    //},
                                    data = new
                                    {
                                        body = msg,
                                        title = title,
                                        image = imgurl,//"http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg"
                                        msg = msg
                                    }
                                };

                                var serializer = new JavaScriptSerializer();
                                using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                                {
                                    string json = serializer.Serialize(payload);
                                    streamWriter.Write(json);
                                    streamWriter.Flush();
                                }

                                var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    result = streamReader.ReadToEnd();
                                }                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = "N";
                }
            }
            return result;           
        }

        public string PushNotificationForLakshya_FFMS(string msg, string compcode, string imgurl)
        {
            var result = "-1";
            //string Query = "Select Deviceno from Mastsalesrep where deviceno='911531150035766'";
            string Query = "Select Deviceno from Mastsalesrep";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            if (dt.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceNo = dt.Rows[i]["Deviceno"].ToString();
                        if (!string.IsNullOrEmpty(DeviceNo))
                        {
                            string regid_query = "select Reg_id from LineMaster where deviceid='" + DeviceNo + "' and Upper(Product)='FFMS' and CompCode='" + compcode + "'";
                            string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";

                            string Query1 = "select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "' and Upper(Product)='FFMS' AND Active ='Y'";
                            string us = DbConnectionDAL.GetStringScalarVal(Query1);

                            SqlConnection cn = new SqlConnection(constrDmLicense);
                            SqlCommand cmd = new SqlCommand(regid_query, cn);
                            SqlCommand cmd1 = new SqlCommand(Query1, cn);
                            cmd.CommandType = CommandType.Text;
                            cmd1.CommandType = CommandType.Text;
                            cn.Open();
                            string regId = cmd.ExecuteScalar() as string;
                            string licenceinfo = cmd1.ExecuteScalar() as string;
                            cn.Close();
                            cmd = null;
                            if (!string.IsNullOrEmpty(regId))
                            {
                                string serverKey = "AAAAuypKwjs:APA91bFqPh91aD5kGSJW9rMp0hdzZJcjR0b96safkW6XNeYBCIeCM2yRaAe3ehLVpYbQ7B1kmxfa5WSBokJiXFKJ7RtrLK2OSIF87nep8HNpYb-8X0AH08jp_INuwoJ0ZnrSka_IHgPkUeHrqIExdPMtBeYQqsHlzg";//s
                                string senderId = "803868426811";
                                string webAddr = "https://fcm.googleapis.com/fcm/send";
                                string title = "FFMS";
                                //var result = "-1";
                                var tRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                                tRequest.ContentType = "application/json";
                                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                                tRequest.Method = "POST";
                                var payload = new
                                {
                                    to = regId,
                                    priority = "high",
                                    content_available = true,
                                    image="",
                                    //notification = new
                                    //{
                                    //    body = msg,
                                    //    title = title
                                    //},
                                    data = new
                                    {
                                        body = msg,
                                        title = title,
                                        image = imgurl,//"http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg"
                                        msg = msg
                                    }
                                };

                                var serializer = new JavaScriptSerializer();
                                using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                                {
                                    string json = serializer.Serialize(payload);
                                    streamWriter.Write(json);
                                    streamWriter.Flush();
                                }

                                var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    result = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = "N";
                }
            }
            return result;
        }   


        private void ClearControls()
        {
            ddlmsgType.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            txtMessage.Text = string.Empty;           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/SendNotification.aspx");
        }        
    }
}