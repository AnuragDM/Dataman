using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using DAL;
using BAL;
using System.Text.RegularExpressions;


namespace AstralFFMS
{
    public partial class LocationMap : System.Web.UI.Page
    {
        private static int SMID = 0;
        private static string Vdate;
        protected void Page_Load(object sender, EventArgs e)
        
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            if (!this.IsPostBack)
            {
                try
                {
                    Txt_FromTime.Text = "00:01";
                    TextBox2.Text = "23:59";
                    //String strmap = "select '['+name+']' as name1 ,* from Locations";
                    //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strmap);
                    //rptMarkers.DataSource = dt;
                    //rptMarkers.DataBind();
                    //GetData();


                    if (!string.IsNullOrEmpty(Request.QueryString["smid"]))
                    {
                        SMID = Convert.ToInt32(Request.QueryString["smid"]);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["VDate"]))
                    {
                        Vdate = Convert.ToString(Request.QueryString["VDate"]);
                        txttodate.Text = Vdate;
                        txtfmDate.Text = Vdate;
                    }
                    else
                    {
                        txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                        txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                    }
                    GetData();
                    LocationData();
                }
                catch(Exception ex)
                {
                    ex.ToString();
                }
            }
          
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
                {
                    string smIDStr1 = "", Qrychk = "", Query = "";

                    //Qrychk = " a.CurrentDate>='" + Settings.dateformat(txtfmDate.Text) + " 00:00' and a.CurrentDate<='" + Settings.dateformat(txttodate.Text) + " 23:59'";
                    Qrychk = " a.CurrentDate>='" + Settings.dateformat(txtfmDate.Text) + " " + Txt_FromTime.Text + "'   and a.CurrentDate<='" + Settings.dateformat(txttodate.Text) + " " + TextBox2.Text + "'";
                    //GetData();
                    LocationData();
                    GetData();

                }

                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                    distlocrpt.DataSource = null;
                    distlocrpt.DataBind();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LocationTrackingReport.aspx");
        }
        protected void LocationData()
        {
            string Qrychk = "", Query = "";
            string str = string.Empty;
            string addDeviceNo = "";
            str = str + " and Log_m in ('C','N','G')";
            string strdevice = "select deviceno from mastsalesrep where smid in (" + SMID + ")";
            DataTable dtdevice = new DataTable();
            dtdevice = DbConnectionDAL.GetDataTable(CommandType.Text, strdevice);
            if (dtdevice.Rows.Count > 0)
            {
                addDeviceNo = dtdevice.Rows[0]["deviceno"].ToString();
            }          

            Qrychk = " CurrentDate>='" + Settings.dateformat(txtfmDate.Text) + " " + Txt_FromTime.Text + "'   and CurrentDate<='" + Settings.dateformat(txttodate.Text) + " " + TextBox2.Text + "'";          
//            Query = @"Select a.deviceNo, a.latitude,a.Longitude , dbo.ConvertToDate(a.currentDate) as currentDate , a.Description , b.SMId , b.SMName from viewlocationdetails a 
//                    inner join MastSalesRep b on a.DeviceNo = b.DeviceNo
//                    where b.SMId in (" + SMID + ") and " + Qrychk + "";
            //DataTable dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            //if (dtLocTraRep.Rows.Count > 0)
            //{
            //    spname.InnerText = dtLocTraRep.Rows[0]["SMName"].ToString();
            //    detailDistOutDiv.Style.Add("display", "block");
            //    distlocrpt.DataSource = dtLocTraRep;
            //    distlocrpt.DataBind();
        //}


            string str1 = "select * from (select CONVERT(VARCHAR(8), CurrentDate, 108) as Time,convert(varchar(18),currentdate,106) as Cdate,dbo.ConvertToDate(currentdate) as dateC,Battery,Gps_accuracy as Accuracy,PersonMaster.PersonName as Person,personmaster.empcode,Locationdetails.Log_m as Signal,description AS address,locationdetails.latitude,locationdetails.longitude,LocationDetails.HomeFlag,'0' as TimeDiff,'0' as distance,'0' as speed,  'LD' as flag from dbo.LocationDetails inner join dbo.PersonMaster on dbo.LocationDetails.DeviceNo=dbo.PersonMaster.DeviceNo WHERE dbo.PersonMaster.DeviceNo in ('" + addDeviceNo + "') and " + Qrychk + " " + str +
             "   UNION " +
   " SELECT CONVERT(VARCHAR(8), log_tracker.currentdate, 108)  AS Time, CONVERT(VARCHAR(18),log_tracker.currentdate, 106) AS Cdate, dbo.ConvertToDate(log_tracker.currentdate) AS dateC," +
       " '' as battery,'' as Accuracy,personmaster.personname AS Person,personmaster.empcode,'' AS Signal, CASE status  WHEN 'GO' THEN ' GPS Off' WHEN 'MO' THEN ' Internet Problem'" + " WHEN 'TN' THEN ' Tower not in reach'  WHEN 'AO' THEN ' Application/Mobile Off' END AS  address,'' as latitude,'' as longitude,'' as homeflag,'0' AS TimeDiff,'0' AS distance,'0' " + " AS speed,  'LT' as flag FROM dbo.log_tracker RIGHT JOIN dbo.personmaster ON dbo.log_tracker.deviceno=dbo.personmaster.deviceno WHERE dbo.log_tracker.status not in ('DR') and  dbo.log_tracker.deviceno in ('" + addDeviceNo + "') " + "  and " + Qrychk + " " +
             " ) atbl order by atbl.Person,CAST(atbl.datec AS datetime) asc";

            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            string mLong = "";
            string mLat = "";
            string mAdd = "";
            string prevlat = "";
            string prevlong = "";

            if (dt1.Rows.Count > 0)
            {
                double distdiff = 0, timediff = 0;               
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
                                                //dist = Calculate(Convert.ToDouble(dt1.Rows[i - 1]["latitude"]), Convert.ToDouble(dt1.Rows[i - 1]["longitude"]), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"]));
                                            }
                                            else
                                            {
                                                //dist = Calculate(Convert.ToDouble(prevlat), Convert.ToDouble(prevlong), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"]));
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
                            ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                            mAdd = DMT.FetchAddress(Glat.ToString(), Glong.ToString());
                            if (string.IsNullOrEmpty(mAdd))
                                mAdd = DMT.InsertAddress(Glat, Glong);
                        }
                        addr = mAdd;
                        dt1.Rows[i]["address"] = addr;
                    }


                    dt1.AcceptChanges();
                }
                detailDistOutDiv.Style.Add("display", "block");
                distlocrpt.DataSource = dt1;
                distlocrpt.DataBind();
            }   
           
          
            //else
            //{
            //    //spname.InnerText = "";
            //    //detailDistOutDiv.Style.Add("display", "block");
            //    //distlocrpt.DataSource = dtLocTraRep;
            //    //distlocrpt.DataBind();
            //}
            Vdate = string.Empty;

        }

        public static double TimeCalc(DateTime d1, DateTime d2)
        {
            double timediff = 0;
            return timediff = d2.Subtract(d1).TotalMinutes;
        }

        //public static double Calculate(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        //{
        //    var sCoord = new GeoCoordinate(sLatitude, sLongitude);
        //    var eCoord = new GeoCoordinate(eLatitude, eLongitude);
        //    return sCoord.GetDistanceTo(eCoord);
        //}
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            string Qrychk1 = "", strmap = "";
            Qrychk1 = " a.CurrentDate>='" + Settings.dateformat(txtfmDate.Text) + " " + Txt_FromTime.Text + "'   and a.CurrentDate<='" + Settings.dateformat(txttodate.Text) + " " + TextBox2.Text + "'";

            strmap = @"Select Title=a.Title, lat=a.lat, lng=a.lng,a.Accuracy from ViewLocationMapdata a left join MastSalesRep b on a.DeviceNo = b.DeviceNo where b.SMId in (" + SMID + ") and " + Qrychk1 + "";
            DataTable dtmap = DbConnectionDAL.GetDataTable(CommandType.Text, strmap);
            if (dtmap.Rows.Count > 0)
            {
                try
                {
                    rptMarkers.DataSource = dtmap;
                    rptMarkers.DataBind();
                }
                catch (Exception ex) { }
            }

            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found!!');", true);
                rptMarkers.DataSource = dt;
                rptMarkers.DataBind();
            }
            return dt;
        }
    }
}