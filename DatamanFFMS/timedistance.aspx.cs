using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
//using DataAccessLayer;
using System.Device.Location;
using System.Web.Services;
using System.Text;
using Newtonsoft.Json;
using DAL;
using System.Globalization;
//using System.Web.Script.Serialization;
using BAL;
using AstralFFMS.ServiceReferenceDMTracker;
using System.IO;


public partial class timedistance : System.Web.UI.Page
{
    SqlConnection con = Connection.Instance.GetConnection();
    //UploadData upd = new UploadData();
    SqlCommand cmd = new SqlCommand();
    DataTable dt = new DataTable();
    Settings SetObj = Settings.Instance;
    Common cm = new Common();
    protected void Page_Load(object sender, EventArgs e)
    {

        string pageName = Path.GetFileName(Request.Path);
        string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
        lblPageHeader.Text = Pageheader;
        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnExport);      
        if (!IsPostBack)
        {
            List<DistributorsMonthlyItem> distMonthlyItem = new List<DistributorsMonthlyItem>();
            distMonthlyItem.Add(new DistributorsMonthlyItem());
            salevaluerpt.DataSource = distMonthlyItem;            
            salevaluerpt.DataBind();
            //txt_fromdate.Text = "01-May-2017";  txt_todate.Text = "01-May-2017";

            txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            txt_todate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            //txt_todate.Text = DateTime.Now.ToUniversalTime().AddDays(1).AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            //   TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            GetGroupName();
            BindPerson();
            txtfromtime.Text = "09:00";
            txttotime.Text = "18:00";
            txtaccu.Text = SetObj.Accuracy;
            if (txtaccu.Text == "0") { txtaccu.Text = "50"; }
        }

        if (SetObj.GroupID != "0" && SetObj.GroupID != "")
        {
            if (!IsPostBack)
            {
                ddlType.SelectedValue = SetObj.GroupID;
                BindPerson();
            }
        }
        else
        {
            SetObj.GroupID = ddlType.SelectedValue;
        }
       
    }
    public class DistributorsMonthlyItem
    {
        public string person { get; set; }
        public string empcode { get; set; }
        public string address { get; set; }
        public string cdate { get; set; }
        public string time { get; set; }
        public string timediff { get; set; }
        public string distance { get; set; }
        public string speed { get; set; }
        public string battery { get; set; }
        public string signal { get; set; }
        public string accuracy { get; set; }
        public string homepage { get; set; }

    }

    [WebMethod(EnableSession = true)]
    public static string GetData(string persons, string loc, string accuracy, string timeinterval, string fromdate, string todate, string fromtime, string totime)
    {
        DataTable dt1 = new DataTable();
        DataTable main = new DataTable();
        try
        {
            string strper = string.Empty;
            string str = string.Empty;
            double timeinterval1 = Convert.ToDouble(timeinterval);
            //double timeinterval2 = timeinterval1 / 100 * 10;
            //double finaltimeinterval = timeinterval1 + timeinterval2;
            GetAddress ga = new GetAddress();
            string addDeviceNo = persons;
            
            if (loc != "0")
            {
                if (loc == "C")
                    str = str + " and Log_m in ('C','N')";
                else str = str + " and Log_m='" + loc + "'";
            }
            if (!string.IsNullOrEmpty(accuracy))
            {
                str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + accuracy;
            }

            string strlog = "SELECT CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate,log_tracker.currentdate AS dateC, '' as battery,'' as Accuracy,personmaster.personname AS Person,personmaster.empcode,'' AS Signal, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem' WHEN 'AO' THEN 'Application/Mobile On' when 'WD' then 'No Network/Data' END AS  address,'' as latitude,'' as longitude,'' as homeflag,'0' AS TimeDiff,'0' AS distance,'0' " + " AS speed,  'LT' as flag FROM log_tracker RIGHT JOIN personmaster ON   log_tracker.deviceno=personmaster.deviceno WHERE log_tracker.status not in ('DR','TN') and  log_tracker.deviceno in (" + addDeviceNo + ") " + " and cast(log_tracker.CurrentDate as datetime) BETWEEN CAST('" + fromdate + " " + fromtime + "' AS datetime) AND CAST('" + todate + " " + totime + "' AS datetime) ORDER BY PersonMaster.PersonName,log_tracker.currentdate ";
            DataTable dtlog = new DataTable();
            dtlog = DbConnectionDAL.getFromDataTable(strlog);

            string str1 = "Select CONVERT(VARCHAR(8), CurrentDate, 108) as Time,convert(varchar(18),currentdate,106) as Cdate,currentdate as dateC,Battery,Gps_accuracy as Accuracy,PersonMaster.PersonName as Person,personmaster.empcode,Locationdetails.Log_m as Signal,description AS address,locationdetails.latitude,locationdetails.longitude,LocationDetails.HomeFlag,'0' as TimeDiff,'0' as distance,'0' as speed,  'LD' as flag from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.DeviceNo in (" + addDeviceNo + ") and cast(locationdetails.CurrentDate as datetime) BETWEEN CAST('" + fromdate + " " + fromtime + "' AS datetime) AND  CAST('" + todate + " " + totime + "' AS datetime) " + str + "  group BY PersonMaster.PersonName,currentdate,Battery,Gps_accuracy ,personmaster.empcode,Locationdetails.Log_m ,description,locationdetails.latitude,locationdetails.longitude,LocationDetails.HomeFlag ORDER BY PersonMaster.PersonName,currentdate";
            dt1 = DbConnectionDAL.getFromDataTable(str1);

            string mLong = "";
            string mLat = "";
            string mAdd = ""; 
            string prevlat = "";
            string prevlong = "";
            int counter = 0;
            if (dt1.Rows.Count > 0)
            {
                double distdiff = 0, timediff = 0;
         
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    int cnt = 0;
                   
                    if (!string.IsNullOrEmpty(dt1.Rows[i]["latitude"].ToString()))
                        prevlat = dt1.Rows[i]["latitude"].ToString();
                    if (!string.IsNullOrEmpty(dt1.Rows[i]["longitude"].ToString()))
                        prevlong = dt1.Rows[i]["longitude"].ToString();
                    if (i > 0)
                    {
                        try
                        {
                          
                            string bn = dt1.Rows[i]["Time"].ToString();
                            //if (bn.Equals("20:46:40"))
                            //{

                            //}
                            if (Convert.ToDateTime(dt1.Rows[i - 1]["Cdate"]) != Convert.ToDateTime(dt1.Rows[i]["Cdate"]) || (dt1.Rows[i - 1]["Person"].ToString()) != (dt1.Rows[i]["Person"]).ToString())
                            {
                                dt1.Rows[i]["TimeDiff"] = 0; dt1.Rows[i]["distance"] = 0; prevlat = ""; prevlong = "";
                            }
                           
                            else
                            {
                               
                                timediff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(dt1.Rows[i]["dateC"])));
                                double ti = (timeinterval1 + (timeinterval1/100*10));
                                if (timediff < 0)
                                {
                                    string r = dt1.Rows[i]["dateC"].ToString();
                                    string h = dt1.Rows[i]["person"].ToString();
                                }
                                if (timediff > ti)
                                {
                                    //if(i>552)
                                    //{

                                    //}
                                    //if (dt1.Rows[i]["person"].ToString() == "VASUDEVAN N")
                                    //{
                                    //    string s = dt1.Rows[i]["person"].ToString();
                                       
                                    //}
                                    
                                    //if (dt1.Rows[i]["dateC"].ToString() == "4/1/2017 5:04:00 AM")
                                    //{
                                    //    string d = dt1.Rows[i]["dateC"].ToString();
                                    //    if (dt1.Rows[i]["person"].ToString() == "VASUDEVAN N")
                                    //    {
                                    //        d = dt1.Rows[i]["dateC"].ToString();
                                    //        string s = "2";
                                    //    }
                                    //}
                                    DateTime dt11 = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]);
                                    DateTime dt2 = Convert.ToDateTime(dt1.Rows[i]["dateC"]);
                                   // string filter = "dateC > '" + Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]) + "' AND dateC <= '" + Convert.ToDateTime(dt1.Rows[i]["dateC"]) + "'";
                                    string filter = "dateC > '" + Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]) + "' AND dateC <= '" + Convert.ToDateTime(dt1.Rows[i]["dateC"]) + "' and person = '" + dt1.Rows[i]["person"].ToString() + "' ";
                                    DataRow[] dr = dtlog.Select(filter);
                                    DataView dv = new DataView(dtlog, filter, "", DataViewRowState.CurrentRows);


                                    DataTable new_table = dv.ToTable("NewTableName");

                                    if (new_table.Rows.Count > 1)
                                    { 
                                    
                                    }
                                    DataRow[] filteredRows = new_table.Select(string.Format("{0} LIKE '%{1}%'", "address", "Application/Mobile On"));
                                    if (filteredRows.Length > 0)
                                    {                                   
                                    
                                    }
                                  
                                    if (new_table.Rows.Count > 0)
                                    {                                       
                                        string name = new_table.Rows[0]["address"].ToString();
                                        DataRow dr1 = null;
                                        double tdiff = 0;
                                        if (filteredRows.Length > 0)
                                        {
                                           
                                            dr1 = dt1.NewRow();
                                            dr1["Person"] = dt1.Rows[i]["Person"].ToString();
                                            dr1["empcode"] = dt1.Rows[i]["empcode"].ToString();
                                            if (filteredRows.Length > 1)
                                            {
                                                int lent = Convert.ToInt32(filteredRows[filteredRows.Length - 1]);
                                                dr1["address"] = filteredRows[lent]["address"].ToString();

                                                dr1["Cdate"] = Convert.ToDateTime(filteredRows[lent]["Cdate"]).ToString("dd MMM yyyy");
                                                dr1["dateC"] = filteredRows[lent]["dateC"].ToString();
                                                dr1["Time"] =  Convert.ToDateTime(filteredRows[lent]["Time"]).ToString("HH:mm:ss");
                                                tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(filteredRows[lent]["dateC"])));
                                            }
                                            else {
                                                dr1["address"] = filteredRows[0]["address"].ToString();
                                                dr1["Cdate"] = Convert.ToDateTime(filteredRows[0]["Cdate"]).ToString("dd MMM yyyy");
                                                dr1["dateC"] = filteredRows[0]["dateC"].ToString();
                                                dr1["Time"] = Convert.ToDateTime(filteredRows[0]["Time"]).ToString("HH:mm:ss");
                                                tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(filteredRows[0]["dateC"])));
                                            }                                                                                 
                                           
                                          
                                            dr1["TimeDiff"] = tdiff;
                                            dr1["distance"] = dt1.Rows[i - 1]["distance"].ToString();
                                            dr1["speed"] = dt1.Rows[i - 1]["speed"].ToString();
                                            dr1["Battery"] = dt1.Rows[i - 1]["Battery"].ToString();
                                            dr1["Signal"] = dt1.Rows[i - 1]["Signal"].ToString();
                                            dr1["Accuracy"] = dt1.Rows[i - 1]["Accuracy"].ToString();
                                            dr1["HomeFlag"] = dt1.Rows[i - 1]["HomeFlag"].ToString();
                                            dr1["latitude"] = dt1.Rows[i - 1]["latitude"].ToString();
                                            dr1["longitude"] = dt1.Rows[i - 1]["longitude"].ToString();
                                            dt1.Rows.InsertAt(dr1, i);
                                        
                                        }
                                        else
                                        {
                                            if (name.Replace(" ", "") != "NoNetwork/Data")
                                            {
                                                if (name.Replace(" ","") == "InternetProblem")
                                                {
                                                    string k = dt1.Rows[i]["Cdate"].ToString(); string h = new_table.Rows[new_table.Rows.Count - 1]["dateC"].ToString();
                                                    if (dt2.ToString() == new_table.Rows[new_table.Rows.Count - 1]["dateC"].ToString())
                                                    {

                                                    }
                                                    else
                                                    {
                                                    if (Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]).ToString() == "4/17/2017 8:17:01 PM")
                                                    {
                                                        string g = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]).ToString();
                                                        string g1 = new_table.Rows[new_table.Rows.Count - 1]["Person"].ToString();
                                                    }
                                                dr1 = dt1.NewRow();
                                                tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"])));
                                                dr1["Person"] = new_table.Rows[new_table.Rows.Count - 1]["Person"].ToString();
                                                dr1["empcode"] = new_table.Rows[new_table.Rows.Count - 1]["empcode"].ToString();
                                                dr1["address"] = name;
                                                dr1["Cdate"] = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["Cdate"]).ToString("dd MMM yyyy");
                                                dr1["dateC"] = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]);
                                                dr1["Time"] = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]).ToString("HH:mm:ss");
                                                dr1["TimeDiff"] = tdiff;
                                                dr1["distance"] = new_table.Rows[new_table.Rows.Count - 1]["distance"].ToString();
                                                dr1["speed"] = new_table.Rows[new_table.Rows.Count - 1]["speed"].ToString();
                                                dr1["Battery"] = new_table.Rows[new_table.Rows.Count - 1]["Battery"].ToString();
                                                dr1["Signal"] = new_table.Rows[new_table.Rows.Count - 1]["Signal"].ToString();
                                                dr1["Accuracy"] = new_table.Rows[new_table.Rows.Count - 1]["Accuracy"].ToString();
                                                dr1["HomeFlag"] = new_table.Rows[new_table.Rows.Count - 1]["HomeFlag"].ToString();
                                                dr1["latitude"] = new_table.Rows[new_table.Rows.Count - 1]["latitude"].ToString();
                                                dr1["longitude"] = new_table.Rows[new_table.Rows.Count - 1]["longitude"].ToString();
                                              
                                               dt1.Rows.InsertAt(dr1, i);
                                                }
                                                }
                                                else
                                                {
                                                    for (int j = 0; j < new_table.Rows.Count; j++)
                                                    {
                                                        dr1 = dt1.NewRow();
                                                        DateTime dt;
                                                        if (j == 0)
                                                        {
                                                            DateTime.TryParseExact(new_table.Rows[j]["Cdate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                                                            tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[j]["dateC"])));
                                                            dr1["Person"] = new_table.Rows[j]["Person"].ToString();
                                                            dr1["empcode"] = new_table.Rows[j]["empcode"].ToString();
                                                            dr1["address"] = name;
                                                            dr1["Cdate"] = Convert.ToDateTime(new_table.Rows[j]["Cdate"]).ToString("dd MMM yyyy");
                                                            dr1["dateC"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]);
                                                            dr1["Time"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]).ToString("HH:mm:ss");
                                                            dr1["TimeDiff"] = tdiff;
                                                            dr1["distance"] = new_table.Rows[j]["distance"].ToString();
                                                            dr1["speed"] = new_table.Rows[j]["speed"].ToString();
                                                            dr1["Battery"] = new_table.Rows[j]["Battery"].ToString();
                                                            dr1["Signal"] = new_table.Rows[j]["Signal"].ToString();
                                                            dr1["Accuracy"] = new_table.Rows[j]["Accuracy"].ToString();
                                                            dr1["HomeFlag"] = new_table.Rows[j]["HomeFlag"].ToString();
                                                            dr1["latitude"] = new_table.Rows[j]["latitude"].ToString();
                                                            dr1["longitude"] = new_table.Rows[j]["longitude"].ToString();
                                                            dt1.Rows.InsertAt(dr1, i);
                                                        }
                                                        else
                                                        {
                                                            double timediff1 = Math.Ceiling(TimeCalc(Convert.ToDateTime(new_table.Rows[j - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[j]["dateC"])));
                                                            // double tim = (timeinterval1 + (timeinterval1 / 100 * 10));

                                                            if (timediff1 > ti)
                                                            {
                                                                DateTime.TryParseExact(new_table.Rows[j]["Cdate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                                                                tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[j]["dateC"])));
                                                                dr1["Person"] = new_table.Rows[j]["Person"].ToString();
                                                                dr1["empcode"] = new_table.Rows[j]["empcode"].ToString();
                                                                dr1["address"] = name;
                                                                dr1["Cdate"] = Convert.ToDateTime(new_table.Rows[j]["Cdate"]).ToString("dd MMM yyyy");
                                                                dr1["dateC"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]);
                                                                dr1["Time"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]).ToString("HH:mm:ss");
                                                                dr1["TimeDiff"] = tdiff;
                                                                dr1["distance"] = new_table.Rows[j]["distance"].ToString();
                                                                dr1["speed"] = new_table.Rows[j]["speed"].ToString();
                                                                dr1["Battery"] = new_table.Rows[j]["Battery"].ToString();
                                                                dr1["Signal"] = new_table.Rows[j]["Signal"].ToString();
                                                                dr1["Accuracy"] = new_table.Rows[j]["Accuracy"].ToString();
                                                                dr1["HomeFlag"] = new_table.Rows[j]["HomeFlag"].ToString();
                                                                dr1["latitude"] = new_table.Rows[j]["latitude"].ToString();
                                                                dr1["longitude"] = new_table.Rows[j]["longitude"].ToString();
                                                                dt1.Rows.InsertAt(dr1, i);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }                                  
                                    }
                                    else
                                    {                                       
                                        DataRow dr1 = dt1.NewRow();
                                        dr1["Person"] = dt1.Rows[i]["Person"].ToString();
                                        dr1["empcode"] = dt1.Rows[i]["empcode"].ToString();
                                        dr1["address"] = "No Network/Data";
                                        dr1["Cdate"] = dt1.Rows[i]["Cdate"].ToString();
                                        dr1["dateC"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(timeinterval1);
                                        dr1["Time"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(timeinterval1).ToString("HH:mm:ss");
                                        dr1["TimeDiff"] = timeinterval1;
                                        dr1["distance"] = dt1.Rows[i - 1]["distance"].ToString();
                                        dr1["speed"] = dt1.Rows[i - 1]["speed"].ToString();
                                        dr1["Battery"] = dt1.Rows[i - 1]["Battery"].ToString();
                                        dr1["Signal"] = dt1.Rows[i - 1]["Signal"].ToString();
                                        dr1["Accuracy"] = dt1.Rows[i - 1]["Accuracy"].ToString();
                                        dr1["HomeFlag"] = dt1.Rows[i - 1]["HomeFlag"].ToString();
                                        dr1["latitude"] = dt1.Rows[i - 1]["latitude"].ToString();
                                        dr1["longitude"] = dt1.Rows[i - 1]["longitude"].ToString();
                                        dt1.Rows.InsertAt(dr1, i);
                                    }                                   
                                }
                                else
                                {
                                    if (dt1.Rows[i]["flag"].Equals("LD"))
                                    {
                                        Double dist = 0;
                                        counter = 0;
                                        if (!string.IsNullOrEmpty(prevlat) && !string.IsNullOrEmpty(prevlong))
                                        {
                                            if (!string.IsNullOrEmpty(dt1.Rows[i - 1]["longitude"].ToString()) && !string.IsNullOrEmpty(dt1.Rows[i - 1]["longitude"].ToString()))
                                            { dist = Calculate(Convert.ToDouble(dt1.Rows[i - 1]["latitude"]), Convert.ToDouble(dt1.Rows[i - 1]["longitude"]), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"])); }
                                            else
                                            { dist = Calculate(Convert.ToDouble(prevlat), Convert.ToDouble(prevlong), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"])); }
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
            }

            else
            { }
        }
        catch (Exception ex)  { ex.ToString(); }
        return JsonConvert.SerializeObject(dt1);
    }
   
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetObj.GroupID = ddlType.SelectedValue;
        BindPerson();

    }
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    public void ClearData()
    {
        Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageNameWithQueryString(), this);
    }

    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
        Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
        Match mtEndDt = Regex.Match(txt_todate.Text, regexDt.ToString());
        if (mtStartDt.Success && mtEndDt.Success)
        {
            if (txt_fromdate.Text != "")
            {
                DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim() + " " + txtfromtime.Text);
                DateTime dt2 = Convert.ToDateTime(txt_todate.Text.Trim() + " " + txttotime.Text);
               
                if (dt1 <= dt2)
                {
                }
                else
                {
                    ShowAlert("From Date cannot be greater than To Date");
                    txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                    txt_todate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                    txtfromtime.Text = "09:00";
                    txttotime.Text = "18:00";
                    return;

                }
            }
            else
            {
                ShowAlert("Pls Select From Date before selecting To Date");
            }
        }
        else
        {
            ShowAlert("Invalid Date!");
        }
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
        Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
        Match mtEndDt = Regex.Match(txt_todate.Text, regexDt.ToString());
        if (mtStartDt.Success && mtEndDt.Success)
        {
            if (txt_fromdate.Text != "")
            {
                DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim()).Date;
                DateTime dt2 = Convert.ToDateTime(txt_todate.Text.Trim()).Date;
                if (dt1 <= dt2)
                {
                }
                else
                {
                    ShowAlert("From Date cannot be greater than To Date");
                    txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                    txt_todate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");

                    return;

                }
            }
            else
            {
                ShowAlert("Pls Select From Date before selecting To Date");
            }
        }
        else
        {
            ShowAlert("Invalid Date!");
        }
    }
    protected void ddlloc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlloc.SelectedValue != "G")
            txtaccu.Text = "5000";
        else
        {
            txtaccu.Text = "50";
            //if (ddlperson.SelectedIndex > 0)
            //{ txtaccu.Text = cm.GetAccuracyByPerson(ddlperson.SelectedValue); }
        }
        //  SetObj.Accuracy = cm.GetAccuracyByPerson(ddlperson.SelectedValue);
        if (txtaccu.Text == "0") { txtaccu.Text = "50"; }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        try
        {
            string strper = string.Empty;
            string str = string.Empty;
            GetAddress ga = new GetAddress();
            string addDeviceNo = "";
            gvData.DataSource = null;
            gvData.DataBind();
            foreach (ListItem item in ddlperson.Items)
            {
                if (ddlperson.SelectedIndex < 0)
                {
                    addDeviceNo += item.Value + "," + "";
                }
                else
                {
                    if (item.Selected)
                    {
                        addDeviceNo += item.Value + "," + "";
                    }
                }
            }
            if (string.IsNullOrEmpty(addDeviceNo))
            {
                ShowAlert("Please Select atleast one Person");
                gvData.DataSource = null; gvData.DataBind();
                return;
            }
            addDeviceNo = addDeviceNo.Substring(0, addDeviceNo.Length - 1);
            if (ddlloc.SelectedValue != "0")
            {
                if (ddlloc.SelectedValue == "C")
                    str = str + " and Log_m in ('C','N')";
                else str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
            }
            if (!string.IsNullOrEmpty(txtaccu.Text))
            {
                str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim();
            } 

            // Nishu 02/09/2016   ( problem of Other date data show)

  //          string str1 = "select * from (select CONVERT(VARCHAR(8), CurrentDate, 108) as Time,convert(varchar(18),currentdate,106) as Cdate,currentdate as dateC,Battery,Gps_accuracy as Accuracy,PersonMaster.PersonName as Person,personmaster.empcode,Locationdetails.Log_m as Signal,description AS address,locationdetails.latitude,locationdetails.longitude,LocationDetails.HomeFlag,'0' as TimeDiff,'0' as distance,'0' as speed,  'LD' as flag from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.DeviceNo in (" + addDeviceNo + ") and cast(locationdetails.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND  DATEADD(DAY, 1, CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime)) " + str +
  //          "   UNION " +
  //" SELECT     CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate,log_tracker.currentdate AS dateC," +
  //    " '' as battery,'' as Accuracy,personmaster.personname AS Person,personmaster.empcode,'' AS Signal, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem'" + " WHEN 'TN' THEN ' Tower not in reach'  WHEN 'AO' THEN ' Application/Mobile Off' END AS  address,'' as latitude,'' as longitude,'' as homeflag,'0' AS TimeDiff,'0' AS distance,'0' " + " AS speed,  'LT' as flag FROM       log_tracker RIGHT JOIN personmaster ON   log_tracker.deviceno=personmaster.deviceno WHERE log_tracker.status not in ('DR') and  log_tracker.deviceno in (" + addDeviceNo + ") " + " and cast(log_tracker.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND  DATEADD(DAY, 1, CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime)) " +
  //          " ) atbl order by atbl.Person,CAST(atbl.datec AS datetime) asc";


            string str1 = "select * from (select CONVERT(VARCHAR(8), CurrentDate, 108) as Time,convert(varchar(18),currentdate,106) as Cdate,currentdate as dateC,Battery,Gps_accuracy as Accuracy,PersonMaster.PersonName as Person,personmaster.empcode,Locationdetails.Log_m as Signal,description AS address,locationdetails.latitude,locationdetails.longitude,LocationDetails.HomeFlag,'0' as TimeDiff,'0' as distance,'0' as speed,  'LD' as flag from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.DeviceNo in (" + addDeviceNo + ") and cast(locationdetails.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND  CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime) " + str +
            "   UNION " +
  " SELECT     CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate,log_tracker.currentdate AS dateC," +
      " '' as battery,'' as Accuracy,personmaster.personname AS Person,personmaster.empcode,'' AS Signal, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem'" + " WHEN 'TN' THEN ' Tower not in reach'  WHEN 'AO' THEN ' Application/Mobile Off' END AS  address,'' as latitude,'' as longitude,'' as homeflag,'0' AS TimeDiff,'0' AS distance,'0' " + " AS speed,  'LT' as flag FROM       log_tracker RIGHT JOIN personmaster ON   log_tracker.deviceno=personmaster.deviceno WHERE log_tracker.status not in ('DR') and  log_tracker.deviceno in (" + addDeviceNo + ") " + " and cast(log_tracker.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime) " +
            " ) atbl order by atbl.Person,CAST(atbl.datec AS datetime) asc";

            //

            DataTable dt1 = new DataTable();
            dt1 =DbConnectionDAL.getFromDataTable(str1);
            string mLong = "";
            string mLat = "";
            string mAdd = "";
            string prevlat = "";
            string prevlong = "";

            if (dt1.Rows.Count > 0)
            {
                double distdiff = 0, timediff = 0;
                //btnExport.Visible = true;
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

                gvData.DataSource = dt1;
                gvData.DataBind();
            }
            else
            {
                ShowAlert("No Record Found !!"); gvData.DataSource = null; gvData.DataBind();
               // btnExport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.ToString());
            ShowAlert("There are some problems while loading records!");
        }
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
    private void BindPerson()
    {
        //string str = "select PersonMaster.PersonName as Person,PersonMaster.deviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
        string str = "select concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )'  as Person,PersonMaster.deviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
        DataTable dt = new DataTable();
        dt = DbConnectionDAL.getFromDataTable(str);
        ddlperson.DataSource = dt;
        ddlperson.DataTextField = "Person";
        ddlperson.DataValueField = "ID";
        ddlperson.DataBind();
    }
    private void GetGroupName()
    {
        ddlType.Items.Clear();
        string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";


        cmd = new SqlCommand(str, con);
        con.Open();
        SqlDataAdapter adpt = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adpt.Fill(dt);
        ddlType.DataSource = dt;
        ddlType.DataBind();
        ddlType.DataTextField = "Description";
        ddlType.DataValueField = "Code";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("--Select--", "0"));
        //ddlperson.Items.Clear();
        //ddlperson.Items.Insert(0, new ListItem("--Select--", "0"));
        con.Close();
    } 

  
    protected void btnExportCSV_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "text/csv";
        Response.AddHeader("content-disposition", "attachment;filename=TimeDistanceLogReport.csv");
        string headertext = "Person".TrimStart('"').TrimEnd('"') + "," + "Emp Code".TrimStart('"').TrimEnd('"') + "," + "Address".TrimStart('"').TrimEnd('"') + "," + "Date".TrimStart('"').TrimEnd('"') + "," + "Time".TrimStart('"').TrimEnd('"') + "," + "Time Diff".TrimStart('"').TrimEnd('"') + "," + "Distance".TrimStart('"').TrimEnd('"') + "," + "Speed".TrimStart('"').TrimEnd('"') + "," + "Battery".TrimStart('"').TrimEnd('"') + "," + "Signal".TrimStart('"').TrimEnd('"') + "," + "Accuracy".TrimStart('"').TrimEnd('"') + "," + "Home".TrimStart('"').TrimEnd('"');

        StringBuilder sb = new StringBuilder();
        sb.Append(headertext);
        sb.AppendLine(System.Environment.NewLine);
        string dataText = string.Empty;
        DataTable dt1 = new DataTable();      
        double timeinterval1 = Convert.ToDouble(txttimediff.Text);      

        {
            string strper = string.Empty;
            string str = string.Empty;
            GetAddress ga = new GetAddress();
            string addDeviceNo = "";
          
            foreach (ListItem item in ddlperson.Items)
            {
                if (ddlperson.SelectedIndex < 0)
                {
                    addDeviceNo += item.Value + "," + "";
                }
                else
                {
                    if (item.Selected)
                    {
                        addDeviceNo += item.Value + "," + "";
                    }
                }
            }
            if (string.IsNullOrEmpty(addDeviceNo))
            {
                ShowAlert("Please Select atleast one Person");
                gvData.DataSource = null; gvData.DataBind();
                //return;
            }
            addDeviceNo = addDeviceNo.Substring(0, addDeviceNo.Length - 1);
            if (ddlloc.SelectedValue != "0")
            {
                if (ddlloc.SelectedValue == "C")
                    str = str + " and Log_m in ('C','N')";
                else str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
            }
            if (!string.IsNullOrEmpty(txtaccu.Text))
            {
                str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim();
            }

            //string str1 = "select * from (select PersonMaster.PersonName as Person,personmaster.empcode,description AS address,convert(varchar(18),currentdate,106) as Cdate,currentdate as dateC,CONVERT(VARCHAR(8), CurrentDate, 108) as Time,'0' as TimeDiff,'0' as distance,'0' as speed,Battery,Locationdetails.Log_m as Signal,Gps_accuracy as Accuracy,LocationDetails.HomeFlag,locationdetails.latitude,locationdetails.longitude,  'LD' as flag from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.DeviceNo in (" + addDeviceNo + ") and cast(locationdetails.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND  CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime) " + str +
            //" UNION " +
            //" SELECT  personmaster.personname AS Person,personmaster.empcode, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem' WHEN 'TN' THEN ' Tower not in reach'  WHEN 'AO' THEN ' Application/Mobile Off' when 'WD' then 'No Network/Data' END AS  address, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate,log_tracker.currentdate AS dateC,CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time,'0' AS TimeDiff,'0' AS distance,'0'  AS speed,'' as battery,'' as Accuracy,'' AS Signal,'' as homeflag,'' as latitude,'' as longitude, 'LT' as flag FROM log_tracker RIGHT JOIN personmaster ON   log_tracker.deviceno=personmaster.deviceno WHERE log_tracker.status not in ('DR') and  log_tracker.deviceno in (" + addDeviceNo + ") " + " and cast(log_tracker.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime) " +
            //" ) atbl order by atbl.Person,CAST(atbl.datec AS datetime) asc";  

            string strlog = "SELECT CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate,log_tracker.currentdate AS dateC, '' as battery,'' as Accuracy,personmaster.personname AS Person,personmaster.empcode,'' AS Signal, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem' WHEN 'AO' THEN 'Application/Mobile On' when 'WD' then 'No Network/Data' END AS  address,'' as latitude,'' as longitude,'' as homeflag,'0' AS TimeDiff,'0' AS distance,'0' " + " AS speed,  'LT' as flag FROM       log_tracker RIGHT JOIN personmaster ON   log_tracker.deviceno=personmaster.deviceno WHERE log_tracker.status not in ('DR','TN') and  log_tracker.deviceno in (" + addDeviceNo + ") " + " and cast(log_tracker.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime) ORDER BY PersonMaster.PersonName,log_tracker.currentdate ";
            DataTable dtlog = new DataTable();
            dtlog = DbConnectionDAL.getFromDataTable(strlog);

            //string strlog = "SELECT CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate,log_tracker.currentdate AS dateC, '' as battery,'' as Accuracy,personmaster.personname AS Person,personmaster.empcode,'' AS Signal, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem'" + " WHEN 'TN' THEN ' Tower not in reach'  WHEN 'AO' THEN 'Application/Mobile On' when 'WD' then 'No Network/Data' END AS  address,'' as latitude,'' as longitude,'' as homeflag,'0' AS TimeDiff,'0' AS distance,'0' " + " AS speed,  'LT' as flag FROM       log_tracker RIGHT JOIN personmaster ON   log_tracker.deviceno=personmaster.deviceno WHERE log_tracker.status not in ('DR') and  log_tracker.deviceno in (" + addDeviceNo + ") " + " and cast(log_tracker.CurrentDate as datetime) between CAST('" + dt1.Rows[i - 1]["dateC"] + "' AS datetime) AND  CAST('" + dt1.Rows[i]["dateC"] + "' AS datetime) ";
            //DataTable dtlog = new DataTable();
            //dtlog = DbConnectionDAL.getFromDataTable(strlog);


            string str1 = "Select PersonMaster.PersonName as Person,personmaster.empcode,description AS address,convert(varchar(18),currentdate,106) as Cdate,currentdate as dateC, CONVERT(VARCHAR(8), CurrentDate, 108) as Time,'0' as TimeDiff,'0' as distance,'0' as speed,Battery,Locationdetails.Log_m as Signal,Gps_accuracy as Accuracy,LocationDetails.HomeFlag,locationdetails.latitude,locationdetails.longitude, 'LD' as flag from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.DeviceNo in (" + addDeviceNo + ") and cast(locationdetails.CurrentDate as datetime) BETWEEN CAST('" + txt_fromdate.Text + " " + txtfromtime.Text + "' AS datetime) AND  CAST('" + txt_todate.Text + " " + txttotime.Text + "' AS datetime) " + str + " group BY PersonMaster.PersonName,currentdate,Battery,Gps_accuracy ,personmaster.empcode,Locationdetails.Log_m ,description,locationdetails.latitude,locationdetails.longitude,LocationDetails.HomeFlag ORDER BY PersonMaster.PersonName,currentdate ";
            dt1 = DbConnectionDAL.getFromDataTable(str1);

            string mLong = "";
            string mLat = "";
            string mAdd = "";
            string prevlat = "";
            string prevlong = "";
            int counter = 0;
            if (dt1.Rows.Count > 0)
            {
                double distdiff = 0, timediff = 0;

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    int cnt = 0;
                    if (!string.IsNullOrEmpty(dt1.Rows[i]["latitude"].ToString()))
                        prevlat = dt1.Rows[i]["latitude"].ToString();
                    if (!string.IsNullOrEmpty(dt1.Rows[i]["longitude"].ToString()))
                        prevlong = dt1.Rows[i]["longitude"].ToString();
                    if (i > 0)
                    {
                        try
                        {
                            string bn = dt1.Rows[i]["Time"].ToString();

                            //if (bn.Equals("11:40:00"))
                            //{ 
                            //}
                            if (Convert.ToDateTime(dt1.Rows[i - 1]["Cdate"]) != Convert.ToDateTime(dt1.Rows[i]["Cdate"]) || (dt1.Rows[i - 1]["Person"].ToString()) != (dt1.Rows[i]["Person"]).ToString())
                            {
                                dt1.Rows[i]["TimeDiff"] = 0; dt1.Rows[i]["distance"] = 0; prevlat = ""; prevlong = "";
                            }
                            else
                            {
                                timediff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(dt1.Rows[i]["dateC"])));
                                double ti = (timeinterval1 + (timeinterval1 / 100 * 10));
                                if (timediff > ti)
                                {
                                    DateTime dt11 = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]);
                                    DateTime dt2 = Convert.ToDateTime(dt1.Rows[i]["dateC"]);
                                   // string filter = "dateC > '" + Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]) + "' AND dateC <= '" + Convert.ToDateTime(dt1.Rows[i]["dateC"]) + "'";
                                    string filter = "dateC > '" + Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]) + "' AND dateC <= '" + Convert.ToDateTime(dt1.Rows[i]["dateC"]) + "' and person = '" + dt1.Rows[i]["person"].ToString() + "' ";
                                    DataRow[] dr = dtlog.Select(filter);
                                    DataView dv = new DataView(dtlog, filter, "", DataViewRowState.CurrentRows);


                                    DataTable new_table = dv.ToTable("NewTableName");

                                    if (new_table.Rows.Count > 1)
                                    {

                                    }
                                    DataRow[] filteredRows = new_table.Select(string.Format("{0} LIKE '%{1}%'", "address", "Application/Mobile On"));
                                    if (filteredRows.Length > 0)
                                    {

                                    }

                                    if (new_table.Rows.Count > 0)
                                    {
                                        string name = new_table.Rows[0]["address"].ToString();
                                        DataRow dr1 = null;
                                        double tdiff = 0;
                                        if (filteredRows.Length > 0)
                                        {

                                            dr1 = dt1.NewRow();
                                            dr1["Person"] = dt1.Rows[i]["Person"].ToString();
                                            dr1["empcode"] = dt1.Rows[i]["empcode"].ToString();
                                            if (filteredRows.Length > 1)
                                            {
                                                int lent = Convert.ToInt32(filteredRows[filteredRows.Length - 1]);
                                                dr1["address"] = filteredRows[lent]["address"].ToString();

                                                dr1["Cdate"] = Convert.ToDateTime(filteredRows[lent]["Cdate"]).ToString("dd MMM yyyy");
                                                dr1["dateC"] = filteredRows[lent]["dateC"].ToString();
                                                dr1["Time"] = Convert.ToDateTime(filteredRows[lent]["Time"]).ToString("HH:mm:ss");
                                                tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(filteredRows[lent]["dateC"])));
                                            }
                                            else
                                            {
                                                dr1["address"] = filteredRows[0]["address"].ToString();
                                                dr1["Cdate"] = Convert.ToDateTime(filteredRows[0]["Cdate"]).ToString("dd MMM yyyy");
                                                dr1["dateC"] = filteredRows[0]["dateC"].ToString();
                                                dr1["Time"] = Convert.ToDateTime(filteredRows[0]["Time"]).ToString("HH:mm:ss");
                                                tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(filteredRows[0]["dateC"])));
                                            }


                                            dr1["TimeDiff"] = tdiff;
                                            dr1["distance"] = dt1.Rows[i - 1]["distance"].ToString();
                                            dr1["speed"] = dt1.Rows[i - 1]["speed"].ToString();
                                            dr1["Battery"] = dt1.Rows[i - 1]["Battery"].ToString();
                                            dr1["Signal"] = dt1.Rows[i - 1]["Signal"].ToString();
                                            dr1["Accuracy"] = dt1.Rows[i - 1]["Accuracy"].ToString();
                                            dr1["HomeFlag"] = dt1.Rows[i - 1]["HomeFlag"].ToString();
                                            dr1["latitude"] = dt1.Rows[i - 1]["latitude"].ToString();
                                            dr1["longitude"] = dt1.Rows[i - 1]["longitude"].ToString();
                                            dt1.Rows.InsertAt(dr1, i);

                                        }
                                        else
                                        {
                                            if (name.Replace(" ", "") != "NoNetwork/Data")
                                            {
                                                if (name.Replace(" ", "") == "InternetProblem")
                                                {
                                                    string k = dt1.Rows[i]["Cdate"].ToString(); string h = new_table.Rows[new_table.Rows.Count - 1]["dateC"].ToString();
                                                    if (dt2.ToString() == new_table.Rows[new_table.Rows.Count - 1]["dateC"].ToString())
                                                    {

                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]).ToString() == "4/17/2017 8:17:01 PM")
                                                        {
                                                            string g = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]).ToString();
                                                            string g1 = new_table.Rows[new_table.Rows.Count - 1]["Person"].ToString();
                                                        }
                                                        dr1 = dt1.NewRow();
                                                        tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"])));
                                                        dr1["Person"] = new_table.Rows[new_table.Rows.Count - 1]["Person"].ToString();
                                                        dr1["empcode"] = new_table.Rows[new_table.Rows.Count - 1]["empcode"].ToString();
                                                        dr1["address"] = name;
                                                        dr1["Cdate"] = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["Cdate"]).ToString("dd MMM yyyy");
                                                        dr1["dateC"] = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]);
                                                        dr1["Time"] = Convert.ToDateTime(new_table.Rows[new_table.Rows.Count - 1]["dateC"]).ToString("HH:mm:ss");
                                                        dr1["TimeDiff"] = tdiff;
                                                        dr1["distance"] = new_table.Rows[new_table.Rows.Count - 1]["distance"].ToString();
                                                        dr1["speed"] = new_table.Rows[new_table.Rows.Count - 1]["speed"].ToString();
                                                        dr1["Battery"] = new_table.Rows[new_table.Rows.Count - 1]["Battery"].ToString();
                                                        dr1["Signal"] = new_table.Rows[new_table.Rows.Count - 1]["Signal"].ToString();
                                                        dr1["Accuracy"] = new_table.Rows[new_table.Rows.Count - 1]["Accuracy"].ToString();
                                                        dr1["HomeFlag"] = new_table.Rows[new_table.Rows.Count - 1]["HomeFlag"].ToString();
                                                        dr1["latitude"] = new_table.Rows[new_table.Rows.Count - 1]["latitude"].ToString();
                                                        dr1["longitude"] = new_table.Rows[new_table.Rows.Count - 1]["longitude"].ToString();

                                                        dt1.Rows.InsertAt(dr1, i);
                                                    }

                                                }
                                                else
                                                {
                                                    for (int j = 0; j < new_table.Rows.Count; j++)
                                                    {
                                                        dr1 = dt1.NewRow();
                                                        DateTime dt;
                                                        if (j == 0)
                                                        {
                                                            DateTime.TryParseExact(new_table.Rows[j]["Cdate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                                                            tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[j]["dateC"])));
                                                            dr1["Person"] = new_table.Rows[j]["Person"].ToString();
                                                            dr1["empcode"] = new_table.Rows[j]["empcode"].ToString();
                                                            dr1["address"] = name;
                                                            dr1["Cdate"] = Convert.ToDateTime(new_table.Rows[j]["Cdate"]).ToString("dd MMM yyyy");
                                                            dr1["dateC"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]);
                                                            dr1["Time"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]).ToString("HH:mm:ss");
                                                            dr1["TimeDiff"] = tdiff;
                                                            dr1["distance"] = new_table.Rows[j]["distance"].ToString();
                                                            dr1["speed"] = new_table.Rows[j]["speed"].ToString();
                                                            dr1["Battery"] = new_table.Rows[j]["Battery"].ToString();
                                                            dr1["Signal"] = new_table.Rows[j]["Signal"].ToString();
                                                            dr1["Accuracy"] = new_table.Rows[j]["Accuracy"].ToString();
                                                            dr1["HomeFlag"] = new_table.Rows[j]["HomeFlag"].ToString();
                                                            dr1["latitude"] = new_table.Rows[j]["latitude"].ToString();
                                                            dr1["longitude"] = new_table.Rows[j]["longitude"].ToString();
                                                            dt1.Rows.InsertAt(dr1, i);
                                                        }
                                                        else
                                                        {
                                                            double timediff1 = Math.Ceiling(TimeCalc(Convert.ToDateTime(new_table.Rows[j - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[j]["dateC"])));
                                                            // double tim = (timeinterval1 + (timeinterval1 / 100 * 10));

                                                            if (timediff1 > ti)
                                                            {
                                                                DateTime.TryParseExact(new_table.Rows[j]["Cdate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                                                                tdiff = Math.Ceiling(TimeCalc(Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]), Convert.ToDateTime(new_table.Rows[j]["dateC"])));
                                                                dr1["Person"] = new_table.Rows[j]["Person"].ToString();
                                                                dr1["empcode"] = new_table.Rows[j]["empcode"].ToString();
                                                                dr1["address"] = name;
                                                                dr1["Cdate"] = Convert.ToDateTime(new_table.Rows[j]["Cdate"]).ToString("dd MMM yyyy");
                                                                dr1["dateC"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]);
                                                                dr1["Time"] = Convert.ToDateTime(new_table.Rows[j]["dateC"]).ToString("HH:mm:ss");
                                                                dr1["TimeDiff"] = tdiff;
                                                                dr1["distance"] = new_table.Rows[j]["distance"].ToString();
                                                                dr1["speed"] = new_table.Rows[j]["speed"].ToString();
                                                                dr1["Battery"] = new_table.Rows[j]["Battery"].ToString();
                                                                dr1["Signal"] = new_table.Rows[j]["Signal"].ToString();
                                                                dr1["Accuracy"] = new_table.Rows[j]["Accuracy"].ToString();
                                                                dr1["HomeFlag"] = new_table.Rows[j]["HomeFlag"].ToString();
                                                                dr1["latitude"] = new_table.Rows[j]["latitude"].ToString();
                                                                dr1["longitude"] = new_table.Rows[j]["longitude"].ToString();
                                                                dt1.Rows.InsertAt(dr1, i);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataRow dr1 = dt1.NewRow();
                                        dr1["Person"] = dt1.Rows[i]["Person"].ToString();
                                        dr1["empcode"] = dt1.Rows[i]["empcode"].ToString();
                                        dr1["address"] = "No Network/Data";
                                        dr1["Cdate"] = dt1.Rows[i]["Cdate"].ToString();
                                        dr1["dateC"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(timeinterval1);
                                        dr1["Time"] = Convert.ToDateTime(dt1.Rows[i - 1]["dateC"]).AddMinutes(timeinterval1).ToString("HH:mm:ss");
                                        dr1["TimeDiff"] = timeinterval1;
                                        dr1["distance"] = dt1.Rows[i - 1]["distance"].ToString();
                                        dr1["speed"] = dt1.Rows[i - 1]["speed"].ToString();
                                        dr1["Battery"] = dt1.Rows[i - 1]["Battery"].ToString();
                                        dr1["Signal"] = dt1.Rows[i - 1]["Signal"].ToString();
                                        dr1["Accuracy"] = dt1.Rows[i - 1]["Accuracy"].ToString();
                                        dr1["HomeFlag"] = dt1.Rows[i - 1]["HomeFlag"].ToString();
                                        dr1["latitude"] = dt1.Rows[i - 1]["latitude"].ToString();
                                        dr1["longitude"] = dt1.Rows[i - 1]["longitude"].ToString();
                                        dt1.Rows.InsertAt(dr1, i);
                                    }
                                }
                                else
                                {
                                    if (dt1.Rows[i]["flag"].Equals("LD"))
                                    {
                                        Double dist = 0;
                                        counter = 0;
                                        if (!string.IsNullOrEmpty(prevlat) && !string.IsNullOrEmpty(prevlong))
                                        {
                                            if (!string.IsNullOrEmpty(dt1.Rows[i - 1]["longitude"].ToString()) && !string.IsNullOrEmpty(dt1.Rows[i - 1]["longitude"].ToString()))
                                            { dist = Calculate(Convert.ToDouble(dt1.Rows[i - 1]["latitude"]), Convert.ToDouble(dt1.Rows[i - 1]["longitude"]), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"])); }
                                            else
                                            { dist = Calculate(Convert.ToDouble(prevlat), Convert.ToDouble(prevlong), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"])); }
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
            }
            else
            {
                ShowAlert("No Record Found !!"); gvData.DataSource = null; gvData.DataBind();
                // btnExport.Visible = false;
            }
        }
        dt1.Columns.Remove("dateC");
        dt1.Columns.Remove("latitude");
        dt1.Columns.Remove("longitude");
        dt1.Columns.Remove("flag");
        dt1.AcceptChanges();
      
            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                for (int k = 0; k < dt1.Columns.Count; k++)
                {
                    if (dt1.Rows[j][k].ToString().Contains(","))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dt1.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dt1.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else if (dt1.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                    {
                        if (k == 0)
                        {
                            sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dt1.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                        }
                        else
                        {
                            sb.Append(String.Format("\"{0}\"", dt1.Rows[j][k].ToString()) + ',');
                        }
                    }
                    else
                    {
                        //if (k == 0 || k == 0)
                        //{
                        //    sb.Append(Convert.ToDateTime(dt1.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                        //}
                        //else
                        {
                            sb.Append(dt1.Rows[j][k].ToString() + ',');
                        }

                    }
                }
                sb.Append(Environment.NewLine);
            }       
      
        //Response.Clear();
        //Response.ContentType = "text/csv";
        //Response.AddHeader("content-disposition", "attachment;filename=SaleValueBreakupReport.xls");
        Response.Write(sb.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

     
}
        