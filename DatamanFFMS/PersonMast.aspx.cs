using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BusinessLayer;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using DAL;
using BAL;

public partial class PersonMast : System.Web.UI.Page
{
    UploadData upd = new UploadData();
 
    DataTable dt = new DataTable();
    private  string frmTime = "";
    private  string toTime = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["PersonName"].ToString() == "")
            {
                txtName.Enabled = false; ;
                Txt_FromTime.Text = "10:00";
                Txt_ToTime.Text = "19:00";
            }
            else
            {
                getdata();
                
            }
        }
    }
    private void SetTime()
    {
        frmTime = Txt_FromTime.Text.Trim();
        toTime = Txt_ToTime.Text.Trim();

        string[] splitfrmTime = frmTime.Split(':');
        string[] splittoTime = toTime.Split(':');

        if (splitfrmTime[0].Length < 2)
        {
            frmTime = "0" + splitfrmTime[0] + ":" + splitfrmTime[1];
        }
        if (splittoTime[0].Length < 2)
        {
            toTime = "0" + splittoTime[0] + ":" + splittoTime[1];
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            char Sys_Flag;
         

            if (chkSys_Flag.Checked == true)
                Sys_Flag = 'Y';
            else
                Sys_Flag = 'N';
            SetTime();
            if (Convert.ToInt16(Txt_min.Text) < 5)
            {
                ShowAlert("Record Interval Should be minimum of 5 min");
                return;
            }
            int mbllen = txtmblnumber.Text.Length;
            if (mbllen < 10) { ShowAlert("Please enter 10 digit mobile number"); return; }
            int srmbllen = txtsrmblnumber.Text.Length;
            if (srmbllen < 10) { ShowAlert("Please enter 10 digit mobile number"); return; }
            if (string.IsNullOrEmpty(Settings.Instance.CompCode) || string.IsNullOrEmpty(Settings.Instance.CompUrl))
            {
                ShowAlert("Please Contact Dataman for this Issue!!");
                return;
            }
            if (hdfCode.Value == "")
            {
                string mobilenochk = DbConnectionDAL.GetStringScalarVal("select mobile from personmaster where mobile ='" + txtmblnumber.Text.Trim() + "'");
                if (!string.IsNullOrEmpty(mobilenochk))
                {
                    ShowAlert("Mobile No already exist . Please enter another Mobile No.!");
                    return;
                }
                string us = DbConnectionDAL.GetStringScalarVal("select DeviceNo from personmaster where DeviceNo='" + txtName.Text.Trim() + "'");
                if (string.IsNullOrEmpty(us))
                {
                    
                    if (upd.AddPersonLicence(Txt_PersonName.Text.Trim(), txtmblnumber.Text))
                    {

                        string insqry = " update linemaster set mobile='" + txtmblnumber.Text + "' where personname='" + Txt_PersonName.Text.Trim() + "' and compcode="+Convert.ToInt32(Settings.Instance.CompCode)+"";
                        int val1 = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
                        int i = upd.InsertUpdatePerson(0, Txt_PersonName.Text.Trim(), txtName.Text, frmTime, toTime, Txt_min.Text, "0", Txt_Upload.Text.Trim(), Txt_retry.Text.Trim(), txtdegree.Text.Trim(), chk1.Checked, Chk2.Checked, Sys_Flag, txtmblnumber.Text.Trim(), ddlsendAlarm.SelectedValue, Convert.ToInt32(0), ddlsendSMS.SelectedValue, txtsrmblnumber.Text.Trim(), ddlsendsmsP.SelectedValue, ddlsendsmsS.SelectedValue, txtlat.Text, txtlong.Text, txtempcode.Text);
                        txtName.Enabled = true;
                        Txt_FromTime.Text = "10:00";
                        Txt_ToTime.Text = "19:00";
                        Txt_min.Text = "";
                        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "";
                        string strmaildetails = "SELECT PersonName,DeviceNo FROM PersonMaster WHERE ID=" + i + "";
                        DataTable dtmail = DbConnectionDAL.getFromDataTable(strmaildetails);
                        upd.SendInsertionEmail(dtmail.Rows[0]["PersonName"].ToString(), dtmail.Rows[0]["DeviceNo"].ToString(), baseUrl);
                        ShowAlert("Records saved successfully!");
                        JSPushNotification(dtmail.Rows[0]["DeviceNo"].ToString(), "");
                        ClearData();
                    }
                    else
                    {
                        ShowAlert("Please Contact Dataman for Person Adding Issue!!");
                        return;
                    }
                }
                else
                {
                    ShowAlert("Device No already exist. Please enter another Device No.!");
                }
            }
            else
            {
                string us = DbConnectionDAL.GetStringScalarVal("select DeviceNo from personmaster where DeviceNo = '" + txtName.Text + "' and ID <> '" + hdfCode.Value + "' ");
                if (string.IsNullOrEmpty(us))
                {
                    string OldDeviceId = DbConnectionDAL.GetStringScalarVal("select DeviceNo from personmaster where ID ='" + hdfCode.Value + "'");
                    if (upd.UpdatePersonLicence(Txt_PersonName.Text.Trim(), OldDeviceId, txtName.Text.Trim()))
                    {
                        int i = upd.InsertUpdatePerson(Convert.ToInt32(hdfCode.Value), Txt_PersonName.Text.Trim(), txtName.Text, frmTime, toTime, Txt_min.Text, "0", Txt_Upload.Text.Trim(), Txt_retry.Text.Trim(), txtdegree.Text.Trim(), chk1.Checked, Chk2.Checked, Sys_Flag, txtmblnumber.Text.Trim(), ddlsendAlarm.SelectedValue, Convert.ToInt32((0)), ddlsendSMS.SelectedValue, txtsrmblnumber.Text.Trim(), ddlsendsmsP.SelectedValue, ddlsendsmsS.SelectedValue, txtlat.Text, txtlong.Text, txtempcode.Text);

                        if (OldDeviceId.Trim() != txtName.Text.Trim())
                        {
                            upd.UpdateDeviceNo(OldDeviceId.Trim(), txtName.Text.Trim());
                        }
                        ShowAlert("Records Updated successfully!");
                        JSPushNotification(txtName.Text, "");
                        //     ClearData();
                        if (Session["Group"].ToString() != "") { Settings.Instance.RedirectCurrentPage("~/PersonList.aspx?Edit=y", this); }
                        else
                            Settings.Instance.RedirectCurrentPage("~/PersonList.aspx", this);
                        //Settings.Instance.RedirectCurrentPage("~/PersonList.aspx", this);
                    }
                    else
                    {
                        ShowAlert("Please Contact Dataman for Person Adding Issue!!");
                        return;
                    }
                }
                else
                {
                    ShowAlert("Device No already exist. Please enter another Device No.!");
                }
            }
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while saving records!");
        }
    }
    public void JSPushNotification(string DeviceNo, string val)
    {
        string Query = "select PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc from PersonMaster where DeviceNo='" + DeviceNo + "'";
        DataTable dtbl = new DataTable();
        dtbl = DbConnectionDAL.getFromDataTable(Query);



        // your RegistrationID paste here which is received from GCM server.                                                               

        // string regId = "APA91bHV3hTRRvUsbR2RKVdVjNDWpYPKRlVGqPLokaTvXCH54x4vfUrqrBPCjdWQcKWe8PoL4kMTgDlu76C24ltVasG0tyvvYW78GTRVQiJmDEGkfQEulQ7yxu_cbVkaFQM68dDwbH5JEEXhVGry50w5ZHpMVW7bRA";
        string regid_query = "select Reg_id from LineMaster where deviceid='" + DeviceNo + "' and Upper(Product)='TRACKER'";
        string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
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
        string regId = cmd.ExecuteScalar().ToString();
        string licenceinfo = cmd1.ExecuteScalar().ToString();
        cn.Close();
        cmd = null;
        if (!string.IsNullOrEmpty(regId))
        {
            // applicationID means google Api key                                                                                                     
            var applicationID = "AAAAPO0XRwQ:APA91bGmOCAGDFKFFqZ2DDJ83fkFRw0OzdGu7KAlVHk6bEEnPtML4-HXDAjNaoJynpKd_bBFEs2yYBM9CZBY1RTZ4ahSo7VvKA3E7UjWN32W2BAQca4JdyMTm1y9qfEa4md1xhOK8hjx";//Tracker

            // SENDER_ID is nothing but your ProjectID (from API Console- google code)//                                          
            var SENDER_ID = "261675763460";

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
            var value = new JavaScriptSerializer().Serialize(nm);
            //var value = val; //message text box                                                                               
            //var title = "Testing";
            //var openpage = "NotificationService.html";
            //var subtitle = "subtital.html";
            WebRequest tRequest;

            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

            tRequest.Method = "post";

            //  tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

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
        }

        // Context.Response.Write(JsonConvert.SerializeObject(rst));
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
    }
    public void ClearData()
    {
        Session["DeviceNo"] = "";
        Session["PersonName"] = "";
        Session["FromTime"] = "";
        Session["ToTime"] = "";
        Session["Interval"] = "";
        Session["ID"] = "";
        Session["UploadInterval"] = "";
        Session["RetryInterval"] = "";
        Session["Degree"] = "";
        Session["GpsLoc"] = "";
        Session["MobileLoc"] = "";
        Session["Sys_Flag"] = "";
        Session["Mobile"] = "";
        Session["Alarm"] = "";
        Session["AlarmDurationMins"] = "";
        Session["SendSms"] = "";
        Session["SrMobile"] = "";
        Session["SendSmsPerson"] = "";
        Session["SendSmsSenior"] = "";
        Session["Lat"] = "";
        Session["Long"] = "";
        Session["EmpCode"] = "";
        Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageName(), this);
    }
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Session["Group"].ToString() != "") { Settings.Instance.RedirectCurrentPage("~/PersonList.aspx?Edit=y", this); }
        else
            Settings.Instance.RedirectCurrentPage("~/PersonList.aspx", this);
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        Session["DeviceNo"] = "";
        Session["PersonName"] = "";
        Session["FromTime"] = "";
        Session["ToTime"] = "";
        Session["Interval"] = "";
        Session["ID"] = "";
        Session["UploadInterval"] = "";
        Session["RetryInterval"] = "";
        Session["Degree"] = "";
        Session["GpsLoc"] = "";
        Session["MobileLoc"] = "";
        Session["Sys_Flag"] = "";
        Session["Mobile"] = "";
        Session["Alarm"] = "";
        Session["AlarmDurationMins"] = "";
        Session["SendSms"] = "";
        Session["SrMobile"] = "";
        Session["SendSmsPerson"] = "";
        Session["SendSmsSenior"] = "";
        Session["Lat"] = "";
        Session["Long"] = "";
        Session["EmpCode"] = "";
        txtName.Enabled = true;
        Settings.Instance.RedirectCurrentPage("~/PersonList.aspx", this);
    }
    protected void getdata()
    {
        Txt_PersonName.Text = Session["PersonName"].ToString();
        txtName.Text = Session["DeviceNo"].ToString();
        Txt_FromTime.Text = Session["FromTime"].ToString();
        Txt_ToTime.Text = Session["ToTime"].ToString();
        Txt_min.Text = Session["Interval"].ToString();
        hdfCode.Value = Session["ID"].ToString();
        Txt_Upload.Text = Session["UploadInterval"].ToString();
        Txt_retry.Text = Session["RetryInterval"].ToString();
        txtdegree.Text = Session["Degree"].ToString();
        chk1.Checked = Convert.ToBoolean(Session["GpsLoc"].ToString());
        Chk2.Checked = Convert.ToBoolean(Session["MobileLoc"].ToString());
        chkSys_Flag.Checked = Convert.ToBoolean(Session["Sys_Flag"].ToString());
        txtmblnumber.Text = Session["Mobile"].ToString();
        ddlsendAlarm.SelectedValue = Session["Alarm"].ToString();
        txtalarmmins.Text = Session["AlarmDurationMins"].ToString();
        ddlsendSMS.SelectedValue = Session["SendSms"].ToString();
        txtsrmblnumber.Text = Session["SrMobile"].ToString();
        ddlsendsmsP.SelectedValue = Session["SendSmsPerson"].ToString();
        ddlsendsmsS.SelectedValue = Session["SendSmsSenior"].ToString();
        txtlat.Text = Session["Lat"].ToString();
        txtlong.Text = Session["Long"].ToString();
        txtempcode.Text = Session["EmpCode"].ToString();
        //txtName.Enabled = false;
    }
    protected void ddlsendAlarm_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsendAlarm.SelectedValue == "N")
        {
            txtalarmmins.Visible = false; span_min.Visible = false; txtalarmmins.Text = "0";
        }
        else { txtalarmmins.Visible = true; span_min.Visible = true; }
    }
}