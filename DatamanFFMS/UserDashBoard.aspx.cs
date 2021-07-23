using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DataAccessLayer;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Services;
using System.Net;
using System.Text;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Runtime.Remoting.Contexts;
//using localhost;
using BusinessLayer;
using DAL;
using BAL;

namespace AstralFFMS
{
    public partial class UserDashBoard : System.Web.UI.Page
    {
        SMSAdapter sms = new SMSAdapter();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!Page.IsPostBack)
                //  GetPersonDetails();
                GetPersonDetailsStatus(0);

            if (!IsPostBack)
            {
                fillgridmsz();
                CalendarExtender3.StartDate = DateTime.Now;
                CalendarExtender5.StartDate = DateTime.Now;
                GetCallingStatus();
            }
        }
        public void GetPersonDetails()
        {
            try
            {
                string qry = string.Empty;

                //qry = " select * from (select LastActive.DeviceNo,   (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE  (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title, convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate,LastActive.CurrentDate AS DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112) >= convert(varchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                //                  " select LastActive.DeviceNo, (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title,convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate ,LastActive.CurrentDate AS DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112)<convert(nvarchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                //                  "select DeviceNo,Title=(concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )'),'' as CurrentDate , NULL AS DDate,lat=cast('00.00' as numeric(12,6)),lng=cast('00.00' as numeric(12,6))from PersonMaster inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and  PersonMaster.DeviceNo not in (select deviceno from LastActive)) tbl order by dDate desc ";

                // Nishu Change 24/Jan/2018 (Multiple name show in dashboard)
                //qry = " select  tbl.*,vm.*,personmaster.Mobile  from (select LastActive.DeviceNo,   (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE  (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title, convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate,LastActive.CurrentDate AS DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112) >= convert(varchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                //                  " select LastActive.DeviceNo, (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title,convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate ,LastActive.CurrentDate AS DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112)<convert(nvarchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                //                  "select DeviceNo,Title=(concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )'),'' as CurrentDate , NULL AS DDate,lat=cast('00.00' as numeric(12,6)),lng=cast('00.00' as numeric(12,6))from PersonMaster inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and  PersonMaster.DeviceNo not in (select deviceno from LastActive)) tbl left join Version_Mast vm on vm.DeviceId=tbl.DeviceNo  left join personmaster on PersonMaster.DeviceNo=tbl.DeviceNo order by dDate desc ";


                qry = " select  tbl.*,vm.*,personmaster.Mobile from (select personmaster.Mobile,LastActive.DeviceNo,   (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE  (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title, convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate,LastActive.CurrentDate AS DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112) >= convert(varchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                                 " select personmaster.Mobile,LastActive.DeviceNo, (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title,convert(varchar(20),LastActive.CurrentDate,113) as CurrentDate ,LastActive.CurrentDate AS DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112)<convert(nvarchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                                 "select personmaster.Mobile,DeviceNo,Title=(concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )'),'' as CurrentDate , NULL AS DDate,lat=cast('00.00' as numeric(12,6)),lng=cast('00.00' as numeric(12,6))from PersonMaster inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and  PersonMaster.DeviceNo not in (select deviceno from LastActive)) tbl left join Version_Mast vm on vm.DeviceId=tbl.DeviceNo left join personmaster on PersonMaster.DeviceNo=tbl.DeviceNo AND PersonMaster.Mobile=tbl.Mobile where personmaster.active is null or PersonMaster.Active='Y' order by dDate desc ";

                DataTable dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, qry, null);
                GridView1.DataSource = dtbl;
                GridView1.DataBind();
                mpePop.PopupControlID = pnlItem.ClientID;
                mpePop.Hide();

            }
            catch (Exception)
            {
                //  ShowAlert("There are some problems while loading records!");
            }
        }
        private void GetCallingStatus()
        {
            string str = "select [Status] ,id from mastUserCallingStatus order by [Status] ";
            //fillDDLDirect(ddlgrp, str, "Code", "Description", 1);
           fillDDLDirect(ddlcallingstatus, str, "id", "Status", 1);
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            xddl.DataSource = null;
            xddl.DataBind();
            DataTable xdt = new DataTable();
            xdt =DbConnectionDAL.GetDataTable(CommandType.Text,xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("All", "0"));
            }
            xdt.Dispose();
        }
        public int GetPersonDetailsStatus(int status)
        {
            try
            {
                string qry = string.Empty;

                string stradd = ""; string str2 = "";
                if (status == 0)
                {
                    stradd = "where personmaster.active is null or PersonMaster.Active='Y'";
                }
                else if (status == 1)
                {
                    stradd = "where personmaster.active = 'N'";
                }
                else
                {
                    stradd = "";
                }
                DataTable dt = Settings.UnderUsers(Settings.Instance.SMID); string underusers = "";
                if(dt.Rows.Count>0)
                {
                   for(int i=0;i<dt.Rows.Count;i++)
                   {
                       underusers+=dt.Rows[i]["smid"].ToString()+",";
                   }
                   underusers = underusers.TrimEnd(',');
                   str2 = "and Personmaster.Deviceno in (select Deviceno from MastSalesRep where smid in ("+underusers+"))";
                }
               
                //and Personmaster.Deviceno in (select Deviceno from MastSalesRep where smid in (172)) 
                //'http://dmfft.dataman.in/img/TodayRemarkIcon.png'
                qry = " select  ( select top 1 cast(createddate as varchar)+'-'+remark from usercallingremark where deviceid=tbl.mobile order by createddate desc ) as comments ,tbl.*,vm.*,personmaster.Mobile,case when DATEDIFF(minute,(select max(mobdatetime) from PushHeartChk where deviceno=tbl.deviceno ),getdate())<=30 then '~/img/greenheart.png' else '~/img/RedHeart.png' end as HeartPath ,vm.company+' - '+vm.model as Phone ,case when (select isnull(max(Id),0) from UserCallingRemark where cast(CreatedDate as date)=cast(getdate() as date) and deviceid=personmaster.Mobile) !=0 then '~/img/todayRemarkIcon.png' else '~/img/PastRemarkIcon.png' end as Remakicon ,              case when (select count(*) from Log_Tracker where deviceno=personmaster.deviceno and [Status]='LV' and vdate=cast(getdate() as date))>0 then 'Leave' when ( select count (*)  from transleaverequest  where transleaverequest.deviceno=personmaster.deviceno and transleaverequest.personname=personmaster.personname and     ((transleaverequest.fromdate>=CAST(getdate() AS DATE) and transleaverequest.todate<=CAST(getdate() AS DATE)) or (transleaverequest.todate>=CAST(getdate() AS DATE)  and transleaverequest.fromdate<=CAST(getdate() AS DATE) )))>0  then 'Leave'  when logstatus is not null then logstatus        else Null end as [Status] from (select personmaster.Mobile,LastActive.DeviceNo,   (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE  (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title,  case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select  convert(varchar(20),max(currentdate),113) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else convert(varchar(20),LastActive.CurrentDate,113)  end as CurrentDate, case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select   max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else LastActive.CurrentDate  end as DDate,  case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select top 1 status from Log_Tracker where CurrentDate=(select  top 1(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate order by currentdate desc ) and deviceno=LastActive.deviceno )  else null  end as Logstatus,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive with (nolock) left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo  WHERE  convert(varchar(12),LastActive.CurrentDate,112) >= convert(varchar(12),DATEadd(second,19800,GetUTCdate()),112) " + str2 + "  UNION " +
                                 " select personmaster.Mobile,LastActive.DeviceNo, (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE  (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title,  case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select  convert(varchar(20),max(currentdate),113) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else convert(varchar(20),LastActive.CurrentDate,113)  end as CurrentDate, case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select   max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else LastActive.CurrentDate  end as DDate,  case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select top 1 status from Log_Tracker where CurrentDate=(select  top 1(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate order by currentdate desc ) and deviceno=LastActive.deviceno )  else null  end as Logstatus,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo  WHERE  convert(varchar(12),LastActive.CurrentDate,112)<convert(nvarchar(12),DATEadd(second,19800,GetUTCdate()),112) " + str2 + "  UNION " +
                                 "select personmaster.Mobile,DeviceNo,Title=(concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )'),'' as CurrentDate , NULL AS DDate,'' as Logstatus,lat=cast('00.00' as numeric(12,6)),lng=cast('00.00' as numeric(12,6))from PersonMaster  with (nolock)  WHERE  PersonMaster.DeviceNo not in (select deviceno from LastActive) " + str2 + ") tbl left join Version_Mast vm on vm.DeviceId=tbl.DeviceNo left join personmaster on PersonMaster.DeviceNo=tbl.DeviceNo AND PersonMaster.Mobile=tbl.Mobile " + stradd + " order by dDate desc ";

                DataTable dtbl = DbConnectionDAL.GetDataTable(CommandType.Text,qry);
                GridView1.DataSource = dtbl;
                GridView1.DataBind();
                mpePop.PopupControlID = pnlItem.ClientID;
                mpePop.Hide();

            }
            catch (Exception)
            {
                ShowAlert("There are some problems while loading records!");
            }
            return 0;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string hdid = GridView1.DataKeys[GridView1.SelectedIndex][0].ToString();
            string hdname = GridView1.DataKeys[GridView1.SelectedIndex][1].ToString();
            lblPerson.Text = hdname;
            this.mpePop.PopupControlID = pnlItem.ID;
            //this.mpePop.Show();
            hidurl.Value = "TodaysLocation.aspx?DeviceNo=" + hdid + "";
          //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "popupWindow1", "popupWindow1('TodaysLocation.aspx?DeviceNo=" + hdid + "');", true);
            // GetDistance(hdid);
            Response.Redirect("TodaysLocation.aspx?DeviceNo=" + hdid + "");
    
            mpePop.PopupControlID = pnlItem.ClientID;
           // mpePop.Show();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    DateTime startdate = new DateTime();
                    DateTime Enddate = new DateTime(); DateTime Enddate1 = new DateTime();
                    startdate = DateTime.ParseExact(DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                    if (!string.IsNullOrEmpty(e.Row.Cells[2].Text) && (e.Row.Cells[2].Text) != "&nbsp;")
                        Enddate = DateTime.ParseExact(Convert.ToDateTime(e.Row.Cells[2].Text).ToString("dd-MMM-yyyy HH:mm"), "dd-MMM-yyyy HH:mm", null);
                    TimeSpan diff = (startdate - Enddate);
                    double MINS = diff.TotalMinutes;
                    DateTime startdate1 = DateTime.ParseExact(DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy"), "dd-MMM-yyyy", null);
                    if (!string.IsNullOrEmpty(e.Row.Cells[2].Text) && (e.Row.Cells[2].Text) != "&nbsp;")
                        Enddate1 = DateTime.ParseExact(Convert.ToDateTime(e.Row.Cells[2].Text).ToString("dd-MMM-yyyy"), "dd-MMM-yyyy", null);
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        LinkButton lnkbtn = (LinkButton)e.Row.FindControl("PersonID");
                        string name = lnkbtn.Text;
                        HiddenField hidcomments = (HiddenField)e.Row.FindControl("hidcommentdata");
                        HiddenField hidappinstalldate = (HiddenField)e.Row.FindControl("hidappinstalldate");

                        if (MINS > 0)
                        {

                            if (MINS <= 60)
                            {

                                lnkbtn.ForeColor = Color.Green;
                            }
                            else if (61 <= MINS && MINS <= 180)
                            {

                                lnkbtn.ForeColor = Color.DarkOrange;
                            }
                            else if (MINS >= 181)
                            {

                                lnkbtn.ForeColor = Color.Blue;
                            }
                            if (startdate1 != Enddate1)
                            {

                                lnkbtn.ForeColor = Color.Red; lnkbtn.Enabled = false;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(e.Row.Cells[2].Text))
                            {

                                lnkbtn.ForeColor = Color.Red; lnkbtn.Enabled = false;
                            }
                            else
                            {

                                lnkbtn.ForeColor = Color.Green;
                            }
                        }

                        if (!String.IsNullOrEmpty(hidcomments.Value))
                            //  e.Row.ToolTip
                            e.Row.Cells[1].ToolTip = "Last Remark of-  " + name + " :- " + hidcomments.Value;
                        e.Row.Cells[5].ToolTip = name + " Installed This Version On- :- " + hidappinstalldate.Value;
                        if (e.Row.Cells[10].Text == "Leave")
                        {

                            e.Row.BackColor = System.Drawing.Color.Moccasin;
                            //  e.Row.ToolTip =name+" said he is on leave today";
                            e.Row.Cells[1].ToolTip = name + " said he is on leave today";

                        }
                        else if (e.Row.Cells[10].Text == "LO")
                        {

                            e.Row.BackColor = System.Drawing.Color.Salmon;
                            e.Row.Cells[10].ToolTip = name + " Location is off";
                            lnkbtn.ForeColor = Color.White;
                        }
                        else if (e.Row.Cells[10].Text == "MO")//nternet problem
                        {
                            e.Row.Cells[10].ToolTip = name + " Internet Off";
                            e.Row.BackColor = System.Drawing.Color.Lavender;
                            lnkbtn.ForeColor = Color.White;
                        }
                        else if (e.Row.Cells[10].Text == "NL")//not reachable
                        {
                            e.Row.Cells[10].ToolTip = name + " Either Location not found or not reachable";
                            e.Row.BackColor = System.Drawing.Color.PowderBlue;
                            lnkbtn.ForeColor = Color.White;

                        }


                    }
                }
                catch (Exception)
                {
                    ShowAlert("There are some problems while loading records!");
                }
            }
        }
        private void GetAllRoutes(string deviceno)
        {
            try
            {
                string qry = "select Title=convert(nvarchar(20),locationdetails.CurrentDate,113)," +
                          " lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6))" +
                          " from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo INNER JOIN GrpMapp ON " +
                          " PersonMaster.ID=GrpMapp.PersonID INNER JOIN UserGrp ON GrpMapp.GroupID=UserGrp.GroupID " +
                          "  WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and PersonMaster.DeviceNo='" + deviceno.Trim() + "' and" +
                         "  cast(LocationDetails.CurrentDate as Date) = cast( ('" + DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy") + "') As Date) ";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry, null);
            }
            catch (Exception ex) { ex.ToString(); }
        }
        public void GetDistance(string getdeviceid)
        {
            int errorrow = 0;
            try
            {
                double cal = 0.00;
                string strper = string.Empty;
                string str = string.Empty;
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                if (getdeviceid != "")
                {
                    str = "DeviceNo ='" + getdeviceid + "'";
                }
                if (str != "")
                {
                    str = str + " and cast(CurrentDate as Date) = cast( '" + DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy") + "' as Date)";
                }
                if (str != "")
                {
                    dt2.Columns.Add("title");
                    dt2.Columns.Add("CurrDate");
                    //dt2.Columns.Add("Distance");
                    dt2.AcceptChanges();
                    //string qry = " select * from  (select '' as title,Case when mobilecreateddate is null then convert(nvarchar(20),currentdate,113) else convert(nvarchar(20),mobilecreateddate,113) end as CurrDate,'0.00' as Distance from LocationDetails where " + str + "  union all select '' as title,convert(nvarchar(20),currentdate,113) as CurrDate,'0.00' as Distance from Log_Tracker where " + str + "      ) tbl order by CurrDate asc ";
                    string qry = " select * from  (select '' as title,convert(nvarchar(20),currentdate,113) as CurrDate,'0.00' as Distance from LocationDetails where " + str + "  union all select '' as title,convert(nvarchar(20),currentdate,113) as CurrDate,'0.00' as Distance from Log_Tracker where " + str + "      ) tbl order by CurrDate asc ";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry, null);
                    if (dt.Rows.Count > 0)
                    {
                        string qry1 = "select * from  (select Locationdetails.Latitude as Latitude,Locationdetails.Longitude as Longitude,Locationdetails.Description as address,     case when Locationdetails.LocationType='DS' Then 'Visit Start - ' + Locationdetails.Description when Locationdetails.LocationType='LS' Then 'Lunch Start - ' + Locationdetails.Description when Locationdetails.LocationType='LE' Then 'Lunch End - ' + Locationdetails.Description  when Locationdetails.LocationType='DE' Then 'Visit Completed - ' + Locationdetails.Description else Locationdetails.Description end as address1,Case when mobilecreateddate is null then convert(nvarchar(20),currentdate,113) else convert(nvarchar(20),mobilecreateddate,113) end as Locationdate , convert(nvarchar(20),currentdate,113) as CurrDate from Locationdetails where DeviceNo = '" + getdeviceid + "' and cast(CurrentDate as Date) = cast('" + DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy") + "' as Date) ";


                        qry1 += " union all    select '0' as Latitude, '0' as Longitude,ltm.Name as address  ,ltm.Name as address1,convert(nvarchar(20),lt.currentdate,113) as CurrDate,convert(nvarchar		(20),lt.currentdate,113) as Locationdate from Log_Tracker lt left join Log_Tracker_Master ltm on ltm.status=lt.status left join personmaster pm on pm.deviceno=lt.DeviceNo where lt.DeviceNo='" + getdeviceid + "'  AND lt.vdate ='" + DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("yyyy-MM-dd") + "'  and pm.active='Y'  ) tbl  order by currdate asc";

                        dt3 = DbConnectionDAL.GetDataTable(CommandType.Text, qry1, null);
                    }
                    string Fromlat = ""; string Tolat = ""; string fromLong = ""; string ToLong = "";
                    for (int k = 0; k < dt3.Rows.Count; k++)
                    {
                        try
                        {
                            cal = 0.00;
                            errorrow = k;
                            if (k > 0)
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(dt3.Rows[k]["latitude"])))
                                {
                                    if (Convert.ToString(dt3.Rows[k]["latitude"]) != "0")
                                    {
                                        Tolat = Convert.ToString(dt3.Rows[k]["latitude"]);
                                        ToLong = Convert.ToString(dt3.Rows[k]["latitude"]);
                                    }
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt3.Rows[k - 1]["latitude"])))
                                {
                                    if (Convert.ToString(dt3.Rows[k - 1]["latitude"]) != "0")
                                    {
                                        Fromlat = Convert.ToString(dt3.Rows[k - 1]["latitude"]);
                                        fromLong = Convert.ToString(dt3.Rows[k - 1]["latitude"]);

                                        Double abc = Calculate(Convert.ToDouble(Fromlat), Convert.ToDouble(fromLong), Convert.ToDouble(Tolat), Convert.ToDouble(ToLong));
                                        cal = cal + Math.Round(abc * 1609 / 1000, 2);
                                    }


                                }

                                dt.Rows[k]["Distance"] = cal;
                            }
                            string addr = dt3.Rows[k]["address1"].ToString();
                            if (addr == "" || addr == "-1" || addr == "Updated Soon..")
                            {
                                AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient DMT = new AstralFFMS.ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                addr = DMT.FetchAddress(dt3.Rows[k]["latitude"].ToString().Substring(0, 7), dt3.Rows[k]["longitude"].ToString().Substring(0, 7));
                                if (string.IsNullOrEmpty(addr))
                                    addr = DMT.InsertAddress(dt3.Rows[k]["latitude"].ToString().Substring(0, 7), dt3.Rows[k]["longitude"].ToString().Substring(0, 7));

                            }

                            dt.Rows[k].SetField("title", "" + addr + " --- At :" + dt3.Rows[k]["Locationdate"].ToString());
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                    }
                }
                gvData.DataSource = dt;
                gvData.DataBind();
            }
            catch (Exception)
            {
                gvData.DataSource = null;
                gvData.DataBind();
                ShowAlert("There are some problems while loading records!" + errorrow);
            }
        }

        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        protected void imgclose_Click(object sender, ImageClickEventArgs e)
        {
            this.mpePop.PopupControlID = pnlItem.ClientID;
            this.mpePop.Hide();

            //   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToRow", "ScrollToRow(" + HidGoTorow.Value + ");", true);

        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetPersonDetails();
        }
        public void fillgridmsz()
        {
            string qry = "select msz from enviro";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text,qry);
            gridmsz.DataSource = dt;
            gridmsz.DataBind();


        }
        protected void btnmszpopup_Click(object sender, EventArgs e)
        {

            this.Modalpopupextender1.PopupControlID = pnlmszpopup.ID;
            this.Modalpopupextender1.Show();

        }
        protected void chkmsz_CheckedChanged(object sender, EventArgs e)
        {
            string text = "";
            string text1 = "";
            //Create a new DataTable.

            foreach (GridViewRow row in GridView1.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chk") as CheckBox);
                    if (chkRow.Checked)
                    {
                        //string name = row.Cells[1].Text;
                        //string country = (row.Cells[2].FindControl("lblCountry") as Label).Text;

                        {
                            text1 += row.Cells[2].Text + ',';
                        }
                    }
                }
            }
            if (text1 == "")
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Atleast One Receipient');", true);
                fillgridmsz();
                //txtmsz.Text = "";
                return;
            }
            this.Modalpopupextender1.PopupControlID = pnlmszpopup.ID;
            this.Modalpopupextender1.Show();
        }
        protected void btnsend_Click(object sender, EventArgs e)
        {
            string script = "alert('Are You Sure To Send Message ?');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            string mobile = "";
            string check = "";
            string Personname = "";
            string b = "";
            string companycode = Settings.Instance.CompCode;
            foreach (GridViewRow row in GridView1.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chk") as CheckBox);
                    if (chkRow.Checked)
                    {
                        //string name = row.Cells[1].Text;
                        //string country = (row.Cells[2].FindControl("lblCountry") as Label).Text;

                        {
                            LinkButton person = (row.Cells[0].FindControl("PersonID") as LinkButton);
                            check += row.Cells[5].Text;
                            mobile = row.Cells[7].Text;
                            Personname = person.Text;
                            foreach (RepeaterItem item in gridmsz.Items)
                            {
                                TextBox txt = (TextBox)item.FindControl("txtmsz");
                                b = txt.Text;

                            }
                            if (b != "")
                            {
                                string qry = "select smskey from smsapikey where ComCode = '" + companycode + "'";
                                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry, null);
                                if (dt.Rows.Count > 0)
                                {
                                    string smsapi = dt.Rows[0]["smskey"].ToString();
                                    //sms.sendSms(mobile, "Dear " + Personname + "," + b);
                                    sms.sendSmsbyTable(smsapi, mobile, "Dear " + Personname + "," + b);
                                    string sqldeviceno = "  select isnull(deviceid,0) from  [dbo].[PersonMaster] where PersonName='" + Personname + "' ";
                                    // string deviceno = DataAccessLayer.DAL.GetStringScalarVal(sqldeviceno).ToString();
                                    //     HiddenField Device = FindControl("hiddeviceno") as HiddenField;
                                    HiddenField Device = (row.Cells[0].FindControl("hiddeviceno") as HiddenField);
                                    string deviceno = Device.Value;
                                    //  JSPushNotification(deviceno, b);
                                    string mszupdate = "update enviro set msz='" + b + "'";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, mszupdate);
                                    
                                }
                                else
                                { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('SMS API Not found');", true); return; }

                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Message First');", true);
                                this.Modalpopupextender1.PopupControlID = pnlmszpopup.ID;
                                this.Modalpopupextender1.Show();
                                return;
                            }


                        }


                    }
                }
            }

            fillgridmsz();
            //txtmsz.Text = "";
            if (check == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Atleast One Receipient');", true);

            }
            else
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Message Sent Succecfully ');", true);
            }
            GetPersonDetails();

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

        //public NotificationManager()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}

        public void JSPushNotification(string DeviceNo, string val)
        {
            string Query = "select PersonName,cast(FromTime as varchar(9)) as FromTime,cast(ToTime as varchar(9)) as ToTime,Interval,UploadInterval,RetryInterval,Degree,Sys_Flag,GpsLoc,MobileLoc,SrMobile,SendSmsSenior,DeviceNo,mobile from PersonMaster where DeviceNo in (" + DeviceNo + ")";
            DataTable dtbl = new DataTable();
            dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, Query, null);

            if (dtbl.Rows.Count > 0)
            {
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    try
                    {
                        // your RegistrationID paste here which is received from GCM server.                                                               
                        DeviceNo = Convert.ToString(dtbl.Rows[i]["DeviceNo"]);
                        string mobile = Convert.ToString(dtbl.Rows[i]["mobile"]);
                        // string regId = "APA91bHV3hTRRvUsbR2RKVdVjNDWpYPKRlVGqPLokaTvXCH54x4vfUrqrBPCjdWQcKWe8PoL4kMTgDlu76C24ltVasG0tyvvYW78GTRVQiJmDEGkfQEulQ7yxu_cbVkaFQM68dDwbH5JEEXhVGry50w5ZHpMVW7bRA";
                        string regid_query = "select Reg_id from LineMaster where deviceid='" + DeviceNo + "'  and mobile='" + mobile + "'  and Upper(Product)='TRACKER'";
                        string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
                        string Query1 = "select case (select 1 from LineMaster WHERE DeviceId='" + DeviceNo + "'   and mobile='" + mobile + "' and Upper(Product)='TRACKER')" +
                       " when 1 THEN (select case when( select URL from LineMaster lm WHERE lm.DeviceId='" + DeviceNo + "'   and mobile='" + mobile + "' and Upper(lm.Product)='TRACKER')='demotracker.dataman.in'" +
                      " THEN (SELECT CASE (SELECT 1 FROM LineMaster where  DeviceId='" + DeviceNo + "'  and mobile='" + mobile + "' and Upper(Product)='TRACKER' AND Validdate < dateadd(ss,19800,getutcdate())) WHEN 1 THEN 'N' ELSE 'Y' END )" +
                      "ELSE (SELECT CASE(SELECT 1 FROM LineMaster WHERE DeviceId='" + DeviceNo + "'  and mobile='" + mobile + "' and Upper(Product)='TRACKER' AND Active ='Y')" +
                      " WHEN 1 THEN 'Y' ELSE 'N' END )END ) ELSE 'N' END ";
                        string us = DbConnectionDAL.GetStringScalarVal(Query1);
                        SqlConnection cn = new SqlConnection(constrDmLicense);
                        SqlCommand cmd = new SqlCommand(regid_query, cn);
                        SqlCommand cmd1 = new SqlCommand(Query1, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd1.CommandType = CommandType.Text;
                        cn.Open();
                        //string regId = cmd.ExecuteScalar().ToString();
                        string regId = cmd.ExecuteScalar() as string;
                        string licenceinfo = cmd1.ExecuteScalar().ToString();
                        cn.Close();
                        cmd = null;
                        if (!string.IsNullOrEmpty(regId))
                        {
                            // applicationID means google Api key                                                                                                     
                            //var applicationID = "AAAAPO0XRwQ:APA91bGmOCAGDFKFFqZ2DDJ83fkFRw0OzdGu7KAlVHk6bEEnPtML4-HXDAjNaoJynpKd_bBFEs2yYBM9CZBY1RTZ4ahSo7VvKA3E7UjWN32W2BAQca4JdyMTm1y9qfEa4md1xhOK8hjx";//Tracker

                            // var SENDER_ID = "261675763460";

                            // var applicationID = "AAAARYN8Wpc:APA91bGUBn9LbbqpDPKvmcT8yq7xo0g4Bcnhoe_l4zJwC8K0erh0pLv82-fI39NUgDTzZGYzfAg6vnsVOs-zjd-H9vVq-BPCUR_mxr3gS1JxXEfzBMINUx_-0Rviv9Ji7hjUqjN-qoSg";//dmtTracker
                            // var SENDER_ID = "298558708375";

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
                            //var value = val; //message text box                                                                               
                            //var title = "Testing";
                            //var openpage = "NotificationService.html";OUTPUT INSERTED.Contact_Id
                            //var subtitle = "subtital.html";
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

                        }
                    }
                    catch (Exception ex)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(" + ex.ToString() + ");", true);
                    }
                }
            }

            // Context.Response.Write(JsonConvert.SerializeObject(rst));
        }
        protected void ImageButton1_Click1(object sender, EventArgs e)
        {
            this.Modalpopupextender1.PopupControlID = pnlmszpopup.ClientID;
            this.Modalpopupextender1.Hide();
            GetPersonDetails();
            fillgridmsz();
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "SendPush")
                {
                    ;
                    string device = e.CommandArgument.ToString();
                    int restart = 0;
                    //start
                    string devicenos = string.Empty;
                    foreach (GridViewRow row in GridView1.Rows)
                    {

                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chk") as CheckBox);
                            if (chkRow.Checked)
                            {
                                restart = restart + 1;
                                {
                                    HiddenField DeviceNo = (row.Cells[0].FindControl("hiddeviceno") as HiddenField);

                                    devicenos = devicenos + "'" + DeviceNo.Value + "'" + ",";

                                    chkRow.Checked = false;
                                }


                            }
                        }

                    }
                    JSPushNotification(devicenos.TrimEnd(','), "Start Services");
                    if (restart == 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select Atleast One Checkbox to Restart');", true);
                    }
                    //End

                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Push Send To " + restart + " Devices');", true);

                    }
                    CheckBox chk1 = GridView1.HeaderRow.Cells[8].FindControl("checkAll") as CheckBox;
                    chk1.Checked = false;
                    //JSPushNotification(device, "Start Services");
                }
                if (e.CommandName == "Remark")
                {
                    //this.ModalPopupExtender2.PopupControlID = Panelremark.ID;
                    //this.ModalPopupExtender2.Show();
                    //string device = Convert.ToString(e.CommandArgument);
                    //string sql = "select personname from personmaster where mobile='" + device + "'";
                    //lblremark.Text = "Enter Remark For : "+DataAccessLayer.DAL.GetStringScalarVal(sql).ToString();
                    //getremark(device);

                }
                if (e.CommandName == "Select")
                {
                    HiddenField hdid = (((e.CommandSource as LinkButton).Parent.FindControl("hiddeviceno")) as HiddenField);
                    Response.Redirect("TodaysLocation.aspx?DeviceNo=" + hdid.Value + "", false);
                }
            }
            catch (Exception ex)
            { ex.ToString(); }
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue == "0")
            {
                GetPersonDetailsStatus(0);
            }
            else if (ddlStatus.SelectedValue == "1")
            {
                GetPersonDetailsStatus(1);
            }
            else
            {
                GetPersonDetailsStatus(2);
            }

        }
        private void getremark(string deviceid)
        {
            hiddendeviceid.Value = deviceid;
            string sql = "select Remark+'--'+CONVERT(varchar, createddate) CallingRemark from UserCallingRemark where deviceid='" + deviceid + "' order by createddate desc";
            DataTable dtremark = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dtremark.Rows.Count > 0)
            {
                GridView2.DataSource = dtremark;
                GridView2.DataBind();
            }
            else
            {
                GridView2.DataSource = null;
                GridView2.DataBind();

            }
        }


        protected void btnsaveremrk_Click(object sender, EventArgs e)
        {
            if (ddlcallingstatus.SelectedValue != "0")
            {
                txtremark.Text = ddlcallingstatus.SelectedItem.Text;
            }
            if (!String.IsNullOrEmpty(txtremark.Text))
            {
                if (!String.IsNullOrEmpty(hiddendeviceid.Value))
                {

                    string sql = "insert into UserCallingRemark(deviceid,remark) values('" + hiddendeviceid.Value + "','" + txtremark.Text.Trim().Replace("'", " ") + "')";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                    this.ModalPopupExtender2.PopupControlID = Panelremark.ID;
                    this.ModalPopupExtender2.Show();
                    string device = Convert.ToString(hiddendeviceid.Value);
                    getremark(device);
                    txtremark.Text = null;
                    // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToRow", "ScrollToRow(" + HidGoTorow.Value + ");", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error While Saving Record');", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Enter Remark');", true);
            }

        }
        protected void btnleave_Click(object sender, EventArgs e)
        {
            string sql = string.Empty; string deviceid = string.Empty;
            if (!String.IsNullOrEmpty(txt_fromdate.Text))
            {
                if (!String.IsNullOrEmpty(TextBox5.Text))
                {
                    if (!String.IsNullOrEmpty(hiddendeviceid.Value))
                    {
                        deviceid = DbConnectionDAL.GetStringScalarVal("select deviceno from personmaster where mobile='" + hiddendeviceid.Value + "'");
                        if (txt_fromdate.Text != TextBox5.Text)
                        {
                            for (DateTime dt = Convert.ToDateTime(txt_fromdate.Text); dt <= Convert.ToDateTime(TextBox5.Text); dt = dt.AddDays(1))
                            {
                                sql = "insert into Log_Tracker(currentdate,[Status],DeviceNo,vdate) values ('" + dt.ToString("yyyy-MM-dd") + "','LV','" + deviceid + "','" + dt.ToString("yyyy-MM-dd") + "')";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text,sql);

                            }
                            sql = "insert into UserCallingRemark(deviceid,remark) values('" + hiddendeviceid.Value + "','On Leave from " + Convert.ToDateTime(txt_fromdate.Text).ToString("yyyy-MM-dd") + "  To   " + Convert.ToDateTime(TextBox5.Text).ToString("yyyy-MM-dd") + "')";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                        }
                        else
                        {

                            sql = "insert into Log_Tracker(currentdate,[Status],DeviceNo,vdate) values ('" + Convert.ToDateTime(txt_fromdate.Text).ToString("yyyy-MM-dd") + "','LV','" + deviceid + "','" + Convert.ToDateTime(txt_fromdate.Text).ToString("yyyy-MM-dd") + "')";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                           
                            sql = "insert into UserCallingRemark(deviceid,remark) values('" + hiddendeviceid.Value + "','On Leave   " + Convert.ToDateTime(txt_fromdate.Text).ToString("yyyy-MM-dd") + "')";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                            DbConnectionDAL.GetStringScalarVal("update LastActive set CurrentDate='" + Convert.ToDateTime(txt_fromdate.Text).ToString("yyyy-MM-dd") + "', smsFlag ='1'  where DeviceNo='" + deviceid + "'");
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToRow", "ScrollToRow(" + HidGoTorow.Value + ");", true);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Leave Marked Successfully');", true);
                        txt_fromdate.Text = string.Empty;
                        TextBox5.Text = string.Empty;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error While Saving Record');", true);
                        this.ModalPopupExtender2.PopupControlID = Panelremark.ID;
                        this.ModalPopupExtender2.Show();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select To Date');", true);
                    this.ModalPopupExtender2.PopupControlID = Panelremark.ID;
                    this.ModalPopupExtender2.Show();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select From Date');", true);
                this.ModalPopupExtender2.PopupControlID = Panelremark.ID;
                this.ModalPopupExtender2.Show();
            }
        }
        protected void imagebtnremark_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ibtn1 = sender as ImageButton;
            ImageButton btnButton = sender as ImageButton;
            GridViewRow gvRow = (GridViewRow)btnButton.NamingContainer;
            LinkButton lblTitle = (LinkButton)gvRow.FindControl("PersonID");
            HidGoTorow.Value = lblTitle.ClientID;
            //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToRow", "ScrollToRow(" + HidGoTorow.Value + ");", true);
            this.ModalPopupExtender2.PopupControlID = Panelremark.ID;
            this.ModalPopupExtender2.Show();
            string device = gvRow.Cells[7].Text;
            string sql = "select personname from personmaster where mobile='" + device + "'";
            lblremark.Text = "Enter Remark For : " +DbConnectionDAL.GetStringScalarVal(sql).ToString();
            getremark(device);
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

        protected void btnrefe_Click(object sender, EventArgs e)
        {
            try
            {
                //lblprogress.Visible = true;
                //localhost.Jwebservice service = new localhost.Jwebservice();
                //service.LocationInsert_Schedulear_V6_1_reverseOrder();
                //lblprogress.Visible = false;
                string qry = "select Isnull(url,0) from [dbo].[enviro]";
                string url = DbConnectionDAL.GetStringScalarVal(qry);
                if (url != "0")
                {
                    if (!url.Contains("http"))
                    {
                        url = "http://" + url;
                    }
                    using (WebClient client = new WebClient())
                    {

                        url = url + "/Jwebservice.asmx/LocationInsert_Schedulear_V6_1_reverseOrder";
                        {
                            var request = (HttpWebRequest)WebRequest.Create(url);

                            request.Method = "GET";
                            // request.UserAgent = RequestConstants.UserAgentValue;
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                            var content = string.Empty;

                            using (var response = (HttpWebResponse)request.GetResponse())
                            {
                                using (var stream = response.GetResponseStream())
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        content = sr.ReadToEnd();
                                        // objResponse = JsonConvert.DeserializeObject(content);
                                    }
                                }
                            }



                        }

                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Response.Redirect("userdashboard.aspx");
            }
        }
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            this.mpePop.PopupControlID = pnlItem.ClientID;
            this.mpePop.Hide();

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToRow", "ScrollToRow(" + HidGoTorow.Value + ");", true);
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            string qry = string.Empty;
            string status = ddlStatus.SelectedValue;
            string stradd = "";
            if (status == "0")
            {
                stradd = "where personmaster.active is null or PersonMaster.Active='Y'";
            }
            else if (status == "1")
            {
                stradd = "where personmaster.active = 'N'";
            }
            else
            {
                stradd = "";
            }

            qry = " select  ROW_NUMBER() OVER (ORDER BY dDate desc ) AS Serial ,personmaster.personname,personmaster.Mobile   from (select personmaster.Mobile,LastActive.DeviceNo,   (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE  (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title,  case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select  convert(varchar(20),max(currentdate),113) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else convert(varchar(20),LastActive.CurrentDate,113)  end as CurrentDate, case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select   max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else LastActive.CurrentDate  end as DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive with (nolock) left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112) >= convert(varchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                             " select personmaster.Mobile,LastActive.DeviceNo, (   CASE isnull(personmaster.empcode,'0') WHEN '0' then personmaster.personname when'' THEN personmaster.personname ELSE  (concat(personmaster.personname, ' ('+ personmaster.empcode ) + ' )')END )as title,  case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select  convert(varchar(20),max(currentdate),113) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else convert(varchar(20),LastActive.CurrentDate,113)  end as CurrentDate, case when (select  max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate ) is not null then (select   max(currentdate) from log_tracker where vdate=cast(DATEadd(second,19800,GetUTCdate()) as Date ) and deviceno=LastActive.deviceno and currentdate>LastActive.CurrentDate) else LastActive.CurrentDate  end as DDate,lat=cast(LastActive.Lat as numeric(12,6)),lng=cast(LastActive.Long as numeric(12,6)) from LastActive left join PersonMaster on LastActive.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and convert(varchar(12),LastActive.CurrentDate,112)<convert(nvarchar(12),DATEadd(second,19800,GetUTCdate()),112) UNION " +
                             "select personmaster.Mobile,DeviceNo,Title=(concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )'),'' as CurrentDate , NULL AS DDate,lat=cast('00.00' as numeric(12,6)),lng=cast('00.00' as numeric(12,6))from PersonMaster  with (nolock) inner join GrpMapp ON  PersonMaster.ID=GrpMapp.PersonID inner join UserGrp ON GrpMapp.GroupID=UserGrp.GroupID WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and  PersonMaster.DeviceNo not in (select deviceno from LastActive)) tbl left join Version_Mast vm on vm.DeviceId=tbl.DeviceNo left join personmaster on PersonMaster.DeviceNo=tbl.DeviceNo AND PersonMaster.Mobile=tbl.Mobile " + stradd + " order by dDate desc ";

            DataTable dtParams =DbConnectionDAL.GetDataTable(CommandType.Text,qry);

            if (dtParams.Rows.Count > 0)
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=UsedDashboard.csv");
                string headertext = "Serial No.".TrimStart('"').TrimEnd('"') + "," + "Person Name".TrimStart('"').TrimEnd('"') + "," + "Mobile".TrimStart('"').TrimEnd('"');
                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);

                for (int w = 0; w < dtParams.Rows.Count; w++)
                {
                    if (w > 0)
                    {

                    }
                }

                for (int j = 0; j < dtParams.Rows.Count; j++)
                {

                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            string h = dtParams.Rows[j][k].ToString();
                            string d = h.Replace(",", " ");
                            dtParams.Rows[j][k] = "";
                            dtParams.Rows[j][k] = d;
                            dtParams.AcceptChanges();
                            if (k == 0)
                            {
                                //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                //sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=UsedDashboard.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found.');", true);
                rptmain.Style.Add("display", "none");
                // ShowAlert("No Record Found");
            }
        }
    }
}