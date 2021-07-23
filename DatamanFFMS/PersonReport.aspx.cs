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
using System.Text.RegularExpressions;

namespace AstralFFMS
{
    public partial class PersonReport : System.Web.UI.Page
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
                txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                //   TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                GetGroupName();
                Txt_FromTime.Text = "00:01";
                txt_Totime.Text = "23:59";
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewExportUtil.ExportGridToCSV(gvData, "DailyActivityReport");
                //GridViewExportUtil.Export("Daily Activity Report", gvData);
            }
            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }
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
            txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
            SetObj.Accuracy = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);

        }
        protected void txt_fromdate_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
            // Match mtEndDt = Regex.Match(TextBox1.Text, regexDt.ToString());
            if (mtStartDt.Success)
            {
            }
            else
            {
                ShowAlert("Invalid Date!");
            }
        }
        private void GetData()
        {
            try
            {
                //double cal = 0.00;
                string strper = string.Empty;
                string str = string.Empty;
                GetAddress ga = new GetAddress();

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
                //string str1 = "select CONVERT(VARCHAR(8), CurrentDate, 108) as Time,convert(varchar(18),currentdate,103) as Cdate,Battery,Gps_accuracy as Accuracy,PersonMaster.PersonName as Person,Locationdetails.Log_m as Signal,description AS address,locationdetails.latitude,locationdetails.longitude from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.ID='" + Convert.ToInt32(DropDownList2.SelectedValue) + "' And cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + txt_fromdate.Text + " " + txt_Totime.Text + "' as DateTime) " + str + " order by Currentdate asc";
                string sql = "select '00:00' as Time,convert(varchar(18),vdate,103) as Cdate,'0' Battery,'0' as Accuracy,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,'0'  as Signal,case when status='LV' Then PersonMaster.personname+' Is On Leave ' +convert(varchar(18),vdate,103) else '' end [address],'0' latitude,'0' longitude from PersonMaster inner join log_tracker on log_tracker.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.ID=" + Convert.ToInt32(DropDownList2.SelectedValue) + " And cast(log_tracker.vdate as date) =cast('" + txt_fromdate.Text + "' as date)     and Log_Tracker.Status='LV' ";

                DataTable dtsql = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dtsql.Rows.Count > 0)
                {
                    gvData.DataSource = dtsql;
                    gvData.DataBind();
                }
                else
                {
                    string str1 = "select CONVERT(VARCHAR(8), CurrentDate, 108) as Time,convert(varchar(18),currentdate,103) as Cdate,Battery,Gps_accuracy as Accuracy,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,Locationdetails.Log_m as Signal,description AS address,locationdetails.latitude,locationdetails.longitude from LocationDetails inner join PersonMaster on LocationDetails.DeviceNo=PersonMaster.DeviceNo WHERE locationdetails.latitude!='' and PersonMaster.ID=" + Convert.ToInt32(DropDownList2.SelectedValue) + " And cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + txt_fromdate.Text + " " + txt_Totime.Text + "' as DateTime) " + str + " order by Currentdate asc";

                    DataTable dt1 = new DataTable();
                    dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);

                    string mLong = "";
                    string mLat = "";
                    string mAdd = "";


                    if (dt1.Rows.Count > 0)
                    {
                        btnExport.Visible = true;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string addr = dt1.Rows[i]["address"].ToString();
                            if (addr == "" || addr == "-1")
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
                                    //mLat=dt1.Rows[i]["latitude"].ToString().Substring(0,7);
                                    //mLong=dt1.Rows[i]["longitude"].ToString().Substring(0,7);
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

                            //if (mLat1 != dt1.Rows[i]["latitude"].ToString()|| mLong1 != dt1.Rows[i]["longitude"].ToString())
                            //{
                            //    mLat1 = dt1.Rows[i]["latitude"].ToString();
                            //    mLong1 = dt1.Rows[i]["longitude"].ToString();
                            //    if (i < dt1.Rows.Count - 1)
                            //    {
                            //        Double dist = Calculate(Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["longitude"]), Convert.ToDouble(dt1.Rows[i + 1]["latitude"]), Convert.ToDouble(dt1.Rows[i + 1]["longitude"]));
                            //        cal = cal + Math.Round(dist * 1609 / 1000, 2); dt1.Rows[i + 1]["distance"] = cal;
                            //    }
                            //}
                            //else
                            //{
                            //    if (i < dt1.Rows.Count - 1)
                            //    dt1.Rows[i + 1]["distance"] = Math.Round(cal,2);
                            //}
                            //else
                            //{
                            //    Double dist = Calculate(Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i]["latitude"]), Convert.ToDouble(dt1.Rows[i + 1]["latitude"]), Convert.ToDouble(dt1.Rows[i + 1]["latitude"]));
                            //    cal = cal + Math.Round(dist * 1609 / 1000, 2); dt1.Rows[i]["distance"] = cal;
                            //}
                            dt1.AcceptChanges();
                        }

                        gvData.DataSource = dt1;
                        gvData.DataBind();
                    }
                    else
                    {
                        ShowAlert("No Record Found !!"); gvData.DataSource = null; gvData.DataBind();
                        btnExport.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.ToString());
                ShowAlert("There are some problems while loading records!");
            }
        }
        protected void ddlloc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlloc.SelectedValue != "G")
                txtaccu.Text = "5000";
            else
            {
                if (DropDownList2.SelectedIndex > 0)
                { txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue); }
            }
            SetObj.Accuracy = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);

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
        protected void btngenerate_Click(object sender, EventArgs e)
        {
            GetData();
        }
        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GetData();
            gvData.PageIndex = e.NewPageIndex;
            gvData.DataBind();
        }
    }
}