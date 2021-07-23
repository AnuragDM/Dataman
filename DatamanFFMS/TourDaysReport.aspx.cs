using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DAL;
using BusinessLayer;
using BAL;
using System.IO;

namespace AstralFFMS
{
    public partial class TourDaysReport : System.Web.UI.Page
    {
        SqlConnection con = Connection.Instance.GetConnection();
        UploadData upd = new UploadData();
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
               // GetGroupName();
                BindPerson();
                txtaccu.Value = SetObj.Accuracy;
                // Clear_Click(sender, e);
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
            if (SetObj.PersonID != "0" && SetObj.PersonID != "")
            {
                if (!IsPostBack)

                    DropDownList2.SelectedValue = SetObj.PersonID;
            }
            else
            {
                SetObj.PersonID = DropDownList2.SelectedValue;
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //GridViewExportUtil.Export("TourDaysReport", gvData);
                GridViewExportUtil.ExportGridToCSV(gvData, "TourDaysReport");
            }
            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }
        }
        private void GetGroupName()
        {
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
            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));
            con.Close();
        }
        private void BindPerson()
        {
            ////SqlCommand cmd = new SqlCommand("select PersonMaster.PersonName as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName", con);
            //SqlCommand cmd = new SqlCommand("select distinct PersonMaster.ID as id,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,PersonMaster.PersonName from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName", con);
            //con.Open();
            //SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            //DataTable dt1 = new DataTable();
            //adpt.Fill(dt1);
            //DropDownList2.DataSource = dt1;
            //DropDownList2.DataBind();
            //DropDownList2.DataTextField = "Person";
            //DropDownList2.DataValueField = "id";
            //DropDownList2.DataBind();
            //DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));
            //con.Close();
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DropDownList2.DataSource = dv.ToTable();
            DropDownList2.DataTextField = "SMName";
            DropDownList2.DataValueField = "SMId";
            DropDownList2.DataBind();
            //Add Default Item in the DropDownList
            DropDownList2.Items.Insert(0, new ListItem("--Please select--"));
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.GroupID = ddlType.SelectedValue;
            BindPerson();
        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.PersonID = DropDownList2.SelectedValue;
            SetObj.Accuracy = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
        }
        protected void btngenerate_Click(object sender, EventArgs e)
        {
            try
            {
                Common cs = new Common();
                string a = getdays(ddlmonth.SelectedItem.Text);
                string startDt = "1-" + ddlmonth.SelectedItem.Text + "- " + ddlyear.SelectedItem.Text + "";
                string endDt = "" + a + "-" + ddlmonth.SelectedItem.Text + "-" + ddlyear.SelectedItem.Text + "";
                DateTime datet = Convert.ToDateTime(startDt);
                DateTime datet2 = Convert.ToDateTime(endDt);
                string name = Convert.ToDateTime(datet).ToString();
                string PersonDeviceNo = string.Empty;
                string str = string.Empty;
                dt.Clear();

                if (DropDownList2.SelectedItem.Text != "--Select--")
                {
                    PersonDeviceNo = cs.GetDeviceNoByPersonID(DropDownList2.SelectedValue);
                    str = " and LocationDetails.DeviceNo='" + PersonDeviceNo + "'";
                }
                //if (strper != "")
                //{
                //    str = str + "PersonMaster.ID in (" + strper + ")";
                //}
                if (str != "")
                {
                    str = str + " and cast(LocationDetails.CurrentDate as Date) > = cast('" + startDt + "' as Date) and cast(LocationDetails.CurrentDate as Date) < = cast('" + endDt + "' as Date)";
                }
                if (str != "")
                {
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("select * from (select max(a.cdate) as cdate ,a.days as days,a.distance from (select cast(locationdetails.CurrentDate as date) as Cdate,'' as days,0.00 AS distance from LocationDetails where 1=1 " + str + " and Log_m='G' ) a   group by a.cdate,a.days,a.distance) s order by s.cdate asc", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataTable dt3 = new DataTable();
                            cmd = new SqlCommand("select Latitude as Latitude,Longitude as Longitude from Locationdetails where convert(VARCHAR(12),currentdate,105)= convert(VARCHAR(12),cast('" + dt.Rows[i]["Cdate"] + "' as Date), 105) and DeviceNo ='" + PersonDeviceNo + "' AND Log_m ='G' order By CurrentDate asc", con);
                            SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                            da2.Fill(dt3);
                            double cal = 0.00;
                            for (int k = 0; k < dt3.Rows.Count - 1; k++)
                            {
                                Double abc = Calculate(Convert.ToDouble(dt3.Rows[k].ItemArray[0]), Convert.ToDouble(dt3.Rows[k].ItemArray[1]), Convert.ToDouble(dt3.Rows[k + 1].ItemArray[0]), Convert.ToDouble(dt3.Rows[k + 1].ItemArray[1]));
                                cal = cal + Math.Round(abc * 1609 / 1000, 2);
                            }
                            dt.Rows[i].SetField("Distance", Math.Round(cal, 2));


                        }
                        btnExport.Visible = true;
                    }
                    else
                    {
                        ShowAlert("No Record Found !");
                        btnExport.Visible = false;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string date = Convert.ToDateTime(dt.Rows[i].ItemArray[0]).ToString();
                        string createddate = Convert.ToDateTime(date).ToString("yyyy-MM-dd h:mm tt");
                        DateTime date1 = DateTime.ParseExact(createddate, "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        dt.Rows[i].SetField("days", "" + date1.DayOfWeek + "");
                    }
                    gvData.DataSource = dt;
                    gvData.DataBind();
                }
            }
            catch (Exception)
            {
                ShowAlert("There are some problems while loading records!");
            }
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
        public string getdays(string a)
        {
            if (ddlmonth.SelectedItem.Text != "")
            {
                if (a == "January")
                {
                    a = "30";
                }
                else if (a == "February")
                {
                    a = "28";
                }
                else if (a == "March")
                {
                    a = "31";
                }
                else if (a == "April")
                {
                    a = "30";
                }
                else if (a == "May")
                {
                    a = "31";
                }
                else if (a == "June")
                {
                    a = "30";
                }
                else if (a == "July")
                {
                    a = "31";
                }
                else if (a == "August")
                {
                    a = "31";
                }
                else if (a == "September")
                {
                    a = "30";
                }
                else if (a == "October")
                {
                    a = "31";
                }
                else if (a == "November")
                {
                    a = "30";
                }
                else if (a == "December")
                {
                    a = "31";
                }
            }

            else
            {
                ShowAlert("Pls Select The Month");
            }
            return a;
        }
    }
}