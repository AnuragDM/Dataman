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

namespace AstralFFMS
{
    public partial class TodaysLocation : System.Web.UI.Page
    {
        SqlConnection con = Connection.Instance.GetConnection();
        UploadData upd = new UploadData();
        SqlCommand cmd = new SqlCommand();
        Settings SetObj = Settings.Instance;
        Common cm = new Common();
        string MarkerData = ""; string paramsmid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            
                txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                TextBox5.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                Txt_FromTime.Text = "00:01";
                TextBox2.Text = "23:59";
                BindPerson();
                txtaccu.Text = SetObj.Accuracy;


                bool hasKeys = Request.QueryString.HasKeys();
                if (hasKeys)
                {
                    persondiv.Style.Add("display", "none");
                    string paramDeviceNo = Request.QueryString["DeviceNo"];
                    FillType();
                    if (!string.IsNullOrEmpty(paramDeviceNo))
                    {
                        string str = "select smid from mastsalesrep where deviceno='" + paramDeviceNo + "'";
                        DataTable dtgrp = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        if (dtgrp.Rows.Count > 0)
                        {
                            
                            DropDownList2.DataSource = null;
                            DropDownList2.DataBind();
                            BindPerson();
                            //ddlloc.SelectedIndex = 0;
                            txtaccu.Text = "5000";
                            string sql = "select top 1 id,personname from personmaster where deviceno='" + paramDeviceNo + "' order by id desc";
                            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                            if (dt.Rows.Count > 0)
                            {
                                DropDownList2.SelectedValue = dt.Rows[0]["id"].ToString();
                                DropDownList2.Enabled = false;
                                paramsmid = dt.Rows[0]["id"].ToString();
                                hidsmid.Value = dt.Rows[0]["id"].ToString();
                                lblHeading.Text = dt.Rows[0]["personname"].ToString()+"'s Location";
                                Btn1_Click(null, null);
                            }
                            else
                            {
                                Response.Write("<script>alert('Invalid User Name or Password.');</script>");

                            }
                        }

                    }

                }
                else
                {
                  
                    persondiv.Style.Add("display", "block");
                    personchange();
                }
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
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }

        //private DataTable GetData()
        //{
        //    DataTable dt = new DataTable();
        //    string strper = string.Empty;
        //    string str = string.Empty;
        //    string str1 = string.Empty;
        //    string str2 = string.Empty;

        //    dt.Clear();
        //    if (DropDownList2.SelectedItem.Text != "--Select--")
        //    {
        //        if (strper == string.Empty) { strper = "'" + DropDownList2.SelectedValue + "'"; }
        //    }
        //    if (strper != "") { str = str + "PersonMaster.ID in (" + strper + ")"; }
        //    if (txt_fromdate.Text != "" && str != "")
        //    {
        //        str = str + " and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox2.Text + "' as DateTime)";
        //    }
        //    if (ddlloc.SelectedValue != "0")
        //    {
        //        if (ddlloc.SelectedValue == "C")
        //            str = str + " and Log_m in ('C','N')";
        //        else str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
        //    }
        //    if (!string.IsNullOrEmpty(txtaccu.Text))
        //    {
        //        str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim();
        //    }
        //    str = str + " and LocationDetails.Latitude !='' and LocationDetails.Latitude  !='' and Gps_accuracy  !='' ";
        //    if (str != "")
        //    {
        //        //if(MarkerData=="1")
        //        //    cmd = new SqlCommand("select Title=description ,lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc", con);

        //        //    else

        //        //02/mar/2016
        //        //cmd = new SqlCommand("select Title=cast(CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108) as varchar)+' ['+ description +']',lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc", con);
        //        cmd = new SqlCommand("select Title=REPLACE(REPLACE(cast(CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108) as varchar)+' ['+ description +']',CHAR(13), ''), CHAR(10), ''),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc", con);
        //    }
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);

        //    if (dt.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            rptMarkers.DataSource = dt;
        //            rptMarkers.DataBind();
        //        }
        //        catch (Exception ex) { }
        //    }


        //    else
        //    {
        //        ShowAlert("No Record Found!!");
        //        rptMarkers.DataSource = null;
        //        rptMarkers.DataBind();
        //    }
        //    return dt;
        //}
        private void personchange()
        {
            DropDownList2_SelectedIndexChanged1(null,null);
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string strper = string.Empty;
            string str = string.Empty;
            string str1 = string.Empty;
            string str2 = string.Empty;
            string logstr = string.Empty;

            dt.Clear();
            
            //if (DropDownList2.SelectedItem.Text != "--Select--")
            //{

            //    if (strper == string.Empty) {
            //        strper = paramsmid;
            //        //strper = "'" + DropDownList2.SelectedValue + "'";
            //    }
            //}
            if (string.IsNullOrEmpty(hidsmid.Value))
            {
                ShowAlert("Please Select Person");
            }
            strper = hidsmid.Value;
            if (strper != "")
            {
                str = str + "personmaster.ID in (" + strper + ")";
                logstr = logstr + "p.ID in (" + strper + ")";
            }
            if (txt_fromdate.Text != "" && str != "")
            {
                str = str + " and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox2.Text + "' as DateTime)";

                logstr = logstr + "and cast(lt.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(lt.CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox2.Text + "' as DateTime)";
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
            str = str + " and LocationDetails.Latitude !='' and LocationDetails.Latitude  !='' and Gps_accuracy  !='' ";
            if (str != "")
            {
                //if(MarkerData=="1")
                //    cmd = new SqlCommand("select Title=description ,lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc", con);

                //    else

                //02/mar/2016
                //cmd = new SqlCommand("select Title=cast(CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108) as varchar)+' ['+ description +']',lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc", con);


                string dd = "select Title=REPLACE(REPLACE(cast(CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108) as varchar)+' ['+ description +']',CHAR(13), ''), CHAR(10), ''),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy as Accuracy,battery from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo where  " + str + " order by LocationDetails.Currentdate asc";
                cmd = new SqlCommand(dd, con);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                try
                {
                    rptMarkers.DataSource = dt;
                    rptMarkers.DataBind();
                }
                catch (Exception ex) { }
            }


            else
            {
                ShowAlert("No Record Found!!");
                rptMarkers.DataSource = null;
                rptMarkers.DataBind();
            }
            return dt;
        }
        public void FillType()
        {
            try
            {
                //string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
                //fillDDLDirect(ddlType, str, "Code", "Description", 1);
                //DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));

                DataTable dt = Settings.UnderUsers(Settings.Instance.SMID); string underusers = "";
                if (dt.Rows.Count > 0)
                {
                    DropDownList2.DataSource = dt;
                    DropDownList2.DataTextField = "Sname";
                    DropDownList2.DataValueField = "Smid";
                }
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
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text,xmQry);
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
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.GroupID = ddlType.SelectedValue;
            BindPerson();
            string StrMarkerData = "select MarkerData from GroupMaster where GroupID = '" + ddlType.SelectedValue + "'";
            MarkerData = DbConnectionDAL.GetStringScalarVal(StrMarkerData);
        }
        //public void GetDatalocation()
        //{
        //    try
        //    {
        //        string query = "";
        //        string strper = string.Empty;
        //        string str = string.Empty;
        //        DataTable dt = new DataTable();
        //        if (DropDownList2.SelectedItem.Text != "--Select--")
        //        {
        //            if (strper == string.Empty) { strper = "'" + DropDownList2.SelectedValue + "'"; }
        //        }
        //        if (strper != "") { str = str + "p.ID in (" + strper + ")"; }
        //        if (txt_fromdate.Text != "" && str != "")
        //        {
        //            str = str + "and cast(ld.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(ld.CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox2.Text + "' as DateTime)";
        //        }


        //        if (ddlloc.SelectedValue != "0")
        //        {
        //            if (ddlloc.SelectedValue == "C")
        //                str = str + " and Log_m in ('C','N')";
        //            else str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
        //        }

        //        if (!string.IsNullOrEmpty(txtaccu.Text))
        //        { str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim(); }
        //        str = str + " and ld.Latitude !='' and ld.Latitude  !='' and ld.Gps_accuracy  !='' ";
        //        if (str != "")
        //        {
        //            query = "select Area=ld.Description,ld.Latitude as Latitude,ld.Longitude as Longitude,ld.Id as Id,dbo.ConvertToDate(ld.CurrentDate) as CDate,Gps_accuracy as Accuracy from LocationDetails ld right Outer join PersonMaster p on ld.DeviceNo =p.DeviceNo where " + str + " order by ld.CurrentDate asc";
        //            cmd = new SqlCommand(query, con);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //            if (dt.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dt.Rows.Count; i++)
        //                {
        //                    string addr = dt.Rows[i]["Area"].ToString();
        //                    if (addr == "")
        //                    {
        //                        ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
        //                        addr = DMT.FetchAddress(dt.Rows[i]["latitude"].ToString().Substring(0, 7), dt.Rows[i]["longitude"].ToString().Substring(0, 7));
        //                        if (string.IsNullOrEmpty(addr))
        //                            addr = DMT.InsertAddress(dt.Rows[i]["latitude"].ToString().Substring(0, 7), dt.Rows[i]["longitude"].ToString().Substring(0, 7));
        //                        dt.Rows[i].SetField("Area", "" + addr + "");
        //                    }
        //                }
        //                gvData.DataSource = dt;
        //                gvData.DataBind();
        //            }
        //            else
        //            {
        //                dt.Clear();
        //                gvData.DataSource = dt;
        //                gvData.DataBind();
        //                ShowAlert("No Records Found !");
        //                return;
        //            }
        //        }

        //    }

        //    catch (Exception)
        //    {
        //        ShowAlert("There are some problems while loading records!");
        //    }
        //}

        public void GetDatalocation()
        {
            try
            {
              
                string query = "";
                string strper = string.Empty;
                string str = string.Empty; string logstr = string.Empty;
                DataTable dt = new DataTable();
                //if (DropDownList2.SelectedItem.Text != "--Select--")
                //{
                //    if (strper == string.Empty) { strper = "'" + DropDownList2.SelectedValue + "'"; }
                //}
                if (string.IsNullOrEmpty(hidsmid.Value))
                {
                    ShowAlert("Please Select Person");
                }
                strper = hidsmid.Value;
                
                if (strper != "") { str = str + "p.ID in (" + strper + ")"; logstr = logstr + "p.ID in (" + strper + ")"; }
                if (txt_fromdate.Text != "" && str != "")
                {
                    str = str + "and cast(ld.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(ld.CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox2.Text + "' as DateTime)";
                    logstr = logstr + "and cast(lt.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(lt.CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox2.Text + "' as DateTime)";
                }


                if (ddlloc.SelectedValue != "0")
                {
                    if (ddlloc.SelectedValue == "C")
                        str = str + " and Log_m in ('C','N')";
                    else str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
                }

                if (!string.IsNullOrEmpty(txtaccu.Text))
                { str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim(); }
                str = str + " and ld.Latitude !='' and ld.Latitude  !='' and ld.Gps_accuracy  !='' ";
                if (str != "")
                {
                    //query = "select Area=ld.Description,ld.Latitude as Latitude,ld.Longitude as Longitude,ld.Id as Id,dbo.ConvertToDate(ld.CurrentDate) as CDate,Gps_accuracy as Accuracy,battery from LocationDetails ld right Outer join PersonMaster p on ld.DeviceNo =p.DeviceNo where " + str + " order by ld.CurrentDate asc";
                    //query = " select * from (select Area=ld.Description+' - At: '+ Convert(varchar(20),ld.currentdate ) ,ld.Latitude as Latitude,ld.Longitude as Longitude,ld.Id as Id,dbo.ConvertToDate(ld.CurrentDate) as CDate,Gps_accuracy as Accuracy,battery,p.personname from LocationDetails ld right Outer join PersonMaster p on ld.DeviceNo =p.DeviceNo where " + str + " union   select ltm.name+' - At: '+ Convert(varchar(20),lt.currentdate ) as area,'' Latitude,'' Longitude,lt.id,lt.currentdate as cdate,'' Accuracy,'' battery,p.personname   from log_tracker lt left join log_tracker_master ltm on ltm.status=lt.Status  right Outer join PersonMaster p on lt.DeviceNo =p.DeviceNo where " + logstr + " ) tbl  order by cdate desc";
                    query = " select * from (select Area=case when ld.LocationType='DS' Then 'Visit Start - ' + ld.Description when ld.LocationType='LS' Then 'Lunch Start - ' + ld.Description when ld.LocationType='LE' Then 'Lunch End - ' + ld.Description  when ld.LocationType='DE' Then 'Visit Completed - ' + ld.Description else ld.Description end +' - At: '+ Convert(varchar(20),ld.currentdate )  ,ld.Latitude as Latitude,ld.Longitude as Longitude,ld.Id as Id,dbo.ConvertToDate(ld.CurrentDate) as CDate,Gps_accuracy as Accuracy,battery,p.personname,case when image is null then 'N/A' when image= '' then 'N/A' else image end  image from LocationDetails ld right Outer join PersonMaster p on ld.DeviceNo =p.DeviceNo where " + str + " union   select ltm.name+' - At: '+ Convert(varchar(20),lt.currentdate ) as area,'' Latitude,'' Longitude,lt.id,lt.currentdate as cdate,'' Accuracy,'' battery,p.personname,'N/A' as image   from log_tracker lt left join log_tracker_master ltm on ltm.status=lt.Status  right Outer join PersonMaster p on lt.DeviceNo =p.DeviceNo where " + logstr + " ) tbl  order by cdate desc";
                    cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string addr = dt.Rows[i]["Area"].ToString();
                            if (addr == "")
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
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DropDownList2.SelectedItem.Text.Contains("Select"))
            {
                SetObj.PersonID = DropDownList2.SelectedValue;
                string deviceid = "select deviceno from mastsalesrep where smid =" + DropDownList2.SelectedValue + "";
                 deviceid = DbConnectionDAL.GetStringScalarVal(deviceid);
                Context.RewritePath("TodaysLocation.aspx?DeviceNo=" + deviceid + "");
              //  Response.Redirect("TodaysLocation.aspx?DeviceNo=" + deviceid + "");
                if (ddlloc.SelectedValue == "G")
                    txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
                else { txtaccu.Text = "5000"; }
                SetObj.Accuracy = txtaccu.Text.Trim();
                gvData.DataSource = null;
                gvData.DataBind();
            }
        }
        protected void txt_fromdate_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
            Match mtEndDt = Regex.Match(TextBox5.Text, regexDt.ToString());
            if (mtStartDt.Success && mtEndDt.Success)
            {
                if (txt_fromdate.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim() + " " + Txt_FromTime.Text);
                    DateTime dt2 = Convert.ToDateTime(TextBox5.Text.Trim() + " " + TextBox2.Text);
                    if (dt1 <= dt2)
                    {
                    }
                    else
                    {
                        ShowAlert("From DateTime Greater Than To DateTime");
                        txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox5.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        Txt_FromTime.Text = "00:01";
                        TextBox2.Text = "23:59";
                        return;
                    }
                }
                else
                {
                    ShowAlert("Pls Select From DateTime After Than Select To DateTime");
                }
            }
            else
            {
                // ShowAlert("Invalid Date!");
            }
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
            DropDownList2.Items.Insert(0, new ListItem("--Please select--","0"));
            
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
                { txtaccu.Text = cm.GetAccuracyByPerson(hidsmid.Value); }
            }
            SetObj.Accuracy = cm.GetAccuracyByPerson(hidsmid.Value);
        }
        protected void Btn1_Click(object sender, EventArgs e)
        {
            GetData();
            GetDatalocation();

        }

        protected void DropDownList2_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //if (!DropDownList2.SelectedItem.Text.Contains("Select"))
            if ((Convert.ToInt32(DropDownList2.SelectedValue) > 0) || (DropDownList2.SelectedValue!=""))
            {
                SetObj.PersonID = DropDownList2.SelectedValue;
                string deviceid = "select deviceno from mastsalesrep where smid =" + DropDownList2.SelectedValue + "";
                deviceid = DbConnectionDAL.GetStringScalarVal(deviceid);
                Context.RewritePath("TodaysLocation.aspx?DeviceNo=" + deviceid + "");
                //  Response.Redirect("TodaysLocation.aspx?DeviceNo=" + deviceid + "");
                if (ddlloc.SelectedValue == "G")
                    txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
                else { txtaccu.Text = "5000"; }
                SetObj.Accuracy = txtaccu.Text.Trim();
                gvData.DataSource = null;
                gvData.DataBind();

                hidsmid.Value = DropDownList2.SelectedValue;
            }
            else
            {
                hidsmid.Value = "";
            }
        }
    }
}