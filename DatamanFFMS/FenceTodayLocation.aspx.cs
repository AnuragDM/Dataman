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
    public partial class FenceTodayLocation : System.Web.UI.Page
    {
        UploadData upd = new UploadData();
        Settings SetObj = Settings.Instance;
        Common cm = new Common();
        static string MarkerData = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
               // FillType();
                txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                Txt_FromTime.Text = "00:01";
                TextBox2.Text = "23:59";
                //txt_fromdate.ReadOnly = true;
                BindPerson();
                txtaccu.Text = SetObj.Accuracy;
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
        private void GetPartyNames()
        {
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dtr = new DataTable();
                dtr.Columns.Add("lat");
                dtr.Columns.Add("lng");
                dtr.Columns.Add("Title");

                if (ddlType.SelectedIndex > 0)
                {
                    //string strq = "select Title=Address,lat=clat,lng=clong from FenceAddress where GroupId='" + ddlType.SelectedValue + "'";
                    string strq = "select  Title=REPLACE(REPLACE(' ['+ replace (Address, '''', '') +']',CHAR(13), ''), CHAR(10), ''),lat=clat,lng=clong from FenceAddress where GroupId='" + ddlType.SelectedValue + "'";
                    dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
                    if (dt1.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(txtradius.Text))
                            txtradius.Text = "0";
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            double radius = Calculate(Convert.ToDouble(hdnFlat.Value), Convert.ToDouble(hdnFlng.Value), Convert.ToDouble(dt1.Rows[i]["lat"]), Convert.ToDouble(dt1.Rows[i]["lng"]));
                            if (radius < Convert.ToInt32(txtradius.Text))
                            {
                                DataRow dr = dtr.NewRow();
                                dr["Title"] = dt1.Rows[i]["Title"];
                                dr["lat"] = dt1.Rows[i]["lat"];
                                dr["lng"] = dt1.Rows[i]["lng"];
                                dtr.Rows.Add(dr);
                            }
                        }

                        dtr.AcceptChanges();

                        rptMarkers1.DataSource = dtr;
                        rptMarkers1.DataBind();
                    }
                    else
                    {
                        rptMarkers1.DataSource = null;
                        rptMarkers1.DataBind();
                    }
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }
        private void GetData()
        {
            DataTable dt = new DataTable();
            string strper = string.Empty;
            string str = string.Empty;
            string qry = "";

            dt.Clear();
            if (DropDownList2.SelectedItem.Text != "--Select--")
            {
                if (strper == string.Empty) { strper = "'" + DropDownList2.SelectedValue + "'"; }
            }
            if (strper != "") { str = str + "PersonMaster.ID in (" + strper + ")"; }
            if (txt_fromdate.Text != "" && str != "")
            {
                str = str + " and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + txt_fromdate.Text + " " + TextBox2.Text + "' as DateTime)";
            }
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

            if (str != "")
            {
                //if(MarkerData=="1")
                //    cmd = new SqlCommand("select Title=description ,lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc", con);

                //    else

                //02/mar/2017
                //qry = "select Title=cast(CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108) as varchar)+' ['+ description +']',lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc";
                qry = "select Title=REPLACE(REPLACE(cast(CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108) as varchar)+''+ description +'',CHAR(13), ''), CHAR(10), ''),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc";

            }

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {
                hdnFlat.Value = dt.Rows[0]["lat"].ToString();
                hdnFlng.Value = dt.Rows[0]["lng"].ToString();
                rptMarkers.DataSource = dt;
                rptMarkers.DataBind();
            }
            else
            {
                ShowAlert("No Record Found!!");
                rptMarkers.DataSource = null;
                rptMarkers.DataBind();
            }
            GetPartyNames();
        }
        public void FillType()
        {
            try
            {
                string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
                fillDDLDirect(ddlType, str, "Code", "Description", 1);
                DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception)
            {

            }
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            xddl.DataSource = null;
            xddl.DataBind();
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
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
        private void BindPerson()
        {
            ////string str = "select PersonMaster.PersonName as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            //string str = "select concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )' as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            //DataTable dt = new DataTable();
            //dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //DropDownList2.DataSource = dt;
            //DropDownList2.DataBind();
            //DropDownList2.DataTextField = "Person";
            //DropDownList2.DataValueField = "ID";
            //DropDownList2.DataBind();
            //DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));

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
        protected void Btn1_Click(object sender, EventArgs e)
        {
            GetData();
            GetDatalocation();
        }
        protected void ddlloc_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvData.DataSource = null;
            gvData.DataBind();
            if (ddlloc.SelectedValue != "G")
                txtaccu.Text = "5000";
            else
            {
                if (DropDownList2.SelectedIndex > 0)
                { txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue); }
            }
            SetObj.Accuracy = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);

        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.PersonID = DropDownList2.SelectedValue;
            if (ddlloc.SelectedValue == "G")
                txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
            else { txtaccu.Text = "5000"; }
            SetObj.Accuracy = txtaccu.Text.Trim();

        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.GroupID = ddlType.SelectedValue;
            BindPerson();
            string StrMarkerData = "select MarkerData from GroupMaster where GroupID = '" + ddlType.SelectedValue + "'";
            MarkerData = DbConnectionDAL.GetStringScalarVal(StrMarkerData);
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        public void GetDatalocation()
        {
            try
            {
                string query = "";
                string strper = string.Empty;
                string str = string.Empty;
                DataTable dt = new DataTable();
                if (DropDownList2.SelectedItem.Text != "--Select--")
                {
                    if (strper == string.Empty) { strper = "'" + DropDownList2.SelectedValue + "'"; }
                }
                if (strper != "") { str = str + "p.ID in (" + strper + ")"; }
                if (txt_fromdate.Text != "" && str != "")
                {
                    str = str + "and cast(ld.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(ld.CurrentDate as DateTime) < = cast('" + txt_fromdate.Text + " " + TextBox2.Text + "' as DateTime)";
                }


                if (ddlloc.SelectedValue != "0")
                {
                    if (ddlloc.SelectedValue == "C")
                        str = str + " and Log_m in ('C','N')";
                    else str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
                }

                if (!string.IsNullOrEmpty(txtaccu.Text))
                { str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim(); }
                if (str != "")
                {
                    query = "select Area=ld.Description,ld.Latitude as Latitude,ld.Longitude as Longitude,ld.Id as Id,dbo.ConvertToDate(ld.CurrentDate) as CDate,Gps_accuracy as Accuracy from LocationDetails ld right Outer join PersonMaster p on ld.DeviceNo =p.DeviceNo where " + str + " order by ld.CurrentDate asc";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string addr = dt.Rows[i]["Area"].ToString();
                            if (addr == "" || addr == "-1")
                            {
                                ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
                                addr = DMT.FetchAddress(dt.Rows[i]["latitude"].ToString().Substring(0, 7), dt.Rows[i]["longitude"].ToString().Substring(0, 7));
                                if (string.IsNullOrEmpty(addr))
                                    addr = DMT.InsertAddress(dt.Rows[i]["latitude"].ToString().Substring(0, 7), dt.Rows[i]["longitude"].ToString().Substring(0, 7));
                                dt.Rows[i].SetField("Area", "" + addr + "");
                            }
                        }
                        gvData.DataSource = dt;
                        gvData.DataBind();
                    }
                    else
                    {
                        dt.Clear();
                        gvData.DataSource = dt;
                        gvData.DataBind();
                        ShowAlert("No Records Found !");
                        return;
                    }
                }

            }

            catch (Exception)
            {
                ShowAlert("There are some problems while loading records!");
            }
        }
    }
}