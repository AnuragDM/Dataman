using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BusinessLayer;
using DAL;
using BAL;
public partial class ActivityCompareRpt : System.Web.UI.Page
{
    
    SqlConnection con = Connection.Instance.GetConnection();
    UploadData upd = new UploadData();
    SqlCommand cmd = new SqlCommand();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGroup();
            txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
            Txt_FromTime.Text = "00:01";
            TextBox2.Text = "23:59";
        }
    }
    public void FillGroup()
    {
        try
        {
            string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
            fillDDLDirect(ddlfgrp, str, "Code", "Description", 1);
            fillDDLDirect(ddlSgrp, str, "Code", "Description", 1);
            fillDDLDirect(ddltgrp, str, "Code", "Description", 1);
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while loading records!");
        }
    }
    public void FillPerson()
    {
        try
        {
            string str = "select PersonMaster.deviceNo as id, PersonName as Person from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID Inner Join UserGrp on GrpMapp.GroupID=UserGrp.GroupID where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by PersonMaster.PersonName";
           fillDDLDirect(ddlfirstperson, str, "id", "Person", 1);
            fillDDLDirect(ddlsecondperson, str, "id", "Person", 1);
          fillDDLDirect(ddlthirdperson, str, "id", "Person", 1);
        }
        catch (Exception)
        {

        }
    }
    public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
    {
        DataTable xdt = new DataTable();
        xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);// DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
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
            xddl.Items.Insert(0, new ListItem("--Others--", "0"));
        }
        xdt.Dispose();
    }
    protected void Btn1_Click(object sender, EventArgs e)
    {
        GetData();
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
        Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
        Match mtEndDt = Regex.Match(TextBox1.Text, regexDt.ToString());
        if (mtStartDt.Success && mtEndDt.Success)
        {
            if (txt_fromdate.Text != "")
            {
                DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim()).Date;
                DateTime dt2 = Convert.ToDateTime(TextBox1.Text.Trim()).Date;
                if (dt1 <= dt2)
                {
                }
                else
                {
                    ShowAlert("From Date cannot be greater than To Date");
                    txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                    TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");

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
    protected void ddltgrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddltgrp.SelectedValue != "0")
            {
                //string str = "select PersonMaster.PersonName as Person,PersonMaster.DeviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddltgrp.SelectedValue + "' order by PersonMaster.PersonName";
                string str = "select concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )' as Person,PersonMaster.DeviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddltgrp.SelectedValue + "' order by PersonMaster.PersonName";

                fillDDLDirect(ddlthirdperson, str, "id", "Person", 1);
            }
        }
        catch (Exception)
        {

        }
    }
    protected void ddlSgrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSgrp.SelectedValue != "0")
            {
                //string str = "select PersonMaster.PersonName as Person,PersonMaster.DeviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlSgrp.SelectedValue + "' order by PersonMaster.PersonName";
                string str = "select concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )' as Person,PersonMaster.DeviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlSgrp.SelectedValue + "' order by PersonMaster.PersonName";

                fillDDLDirect(ddlsecondperson, str, "id", "Person", 1);
            }
        }
        catch (Exception)
        {

        }
    }
    protected void ddlfgrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlfgrp.SelectedValue != "0")
            {
                //string str = "select PersonMaster.PersonName as Person,PersonMaster.DeviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlfgrp.SelectedValue + "' order by PersonMaster.PersonName";
                string str = "select concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )' as Person,PersonMaster.DeviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlfgrp.SelectedValue + "' order by PersonMaster.PersonName";
               fillDDLDirect(ddlfirstperson, str, "id", "Person", 1);
            }

        }
        catch (Exception)
        {

        }
    }
    private void GetData()
    {
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        string strper = string.Empty;
        string str = string.Empty;
        string str1 = string.Empty;
        string str2 = string.Empty;
        dt.Clear();

        str = " and LocationDetails.DeviceNo='" + ddlfirstperson.SelectedValue + "'";
        if (!string.IsNullOrEmpty(txtaccu.Text))
        {
            str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim();
        }
        if (txt_fromdate.Text != "")
        { str = str + " and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime)"; }
        if (str != "")
        { cmd = new SqlCommand("select Person=PersonMaster.Personname+' ' +CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108), Title=cast(cast(locationdetails.CurrentDate as Time(0)) as varchar(5)),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)) from LocationDetails  right Outer join PersonMaster on    LocationDetails.DeviceNo =PersonMaster.DeviceNo where 1=1 and Log_m='G' " + str + " order by LocationDetails.Currentdate asc", con); }
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);


        dt1.Clear();
        str = "";
        str = "and LocationDetails.DeviceNo='" + ddlsecondperson.SelectedValue + "'";
        if (!string.IsNullOrEmpty(txtaccu.Text))
        {
            str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim();
        }
        if (txt_fromdate.Text != "")
        { str = str + " and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime)"; }

        if (str != "")
        { cmd = new SqlCommand("select Person=PersonMaster.Personname+' ' +CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108), Title=cast(cast(locationdetails.CurrentDate as Time(0)) as varchar(5)),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)) from LocationDetails  right Outer join PersonMaster on    LocationDetails.DeviceNo =PersonMaster.DeviceNo where  1=1 and Log_m='G' " + str + " order by LocationDetails.Currentdate asc", con); }
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        da1.Fill(dt1);
        //if (dt1.Rows.Count > 0)
        //{
        //    rptMarkers1.DataSource = dt1;
        //    rptMarkers1.DataBind();
        //}


        dt2.Clear();
        str = "";
        str = "and LocationDetails.DeviceNo='" + ddlthirdperson.SelectedValue + "'";
        if (!string.IsNullOrEmpty(txtaccu.Text))
        {
            str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim();
        }
        if (txt_fromdate.Text != "")
        { str = str + " and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime)"; }
        if (str != "")
        { cmd = new SqlCommand("select Person=PersonMaster.Personname+' ' +CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108),Title=cast(cast(locationdetails.CurrentDate as Time(0)) as varchar(5)),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)) from LocationDetails  right Outer join PersonMaster on    LocationDetails.DeviceNo =PersonMaster.DeviceNo where 1=1 and Log_m='G' " + str + " order by LocationDetails.Currentdate asc", con); }
        SqlDataAdapter da2 = new SqlDataAdapter(cmd);
        da2.Fill(dt2);
        //if (dt2.Rows.Count > 0)
        //{
        //    rptMarkers2.DataSource = dt2;
        //    rptMarkers2.DataBind();
        //}
        if (dt.Rows.Count <= 0 && dt1.Rows.Count <= 0 && dt2.Rows.Count <= 0)
        {
            ShowAlert("No Record Found!!");
            rptMarkers.DataSource = null;
            rptMarkers.DataBind();
            rptMarkers1.DataSource = null;
            rptMarkers1.DataBind();
            rptMarkers2.DataSource = null;
            rptMarkers2.DataBind();
        }
        else
        {
            if (dt.Rows.Count > 0)
            {
                rptMarkers.DataSource = dt;
                rptMarkers.DataBind();
            }
            if (dt1.Rows.Count > 0)
            {
                rptMarkers1.DataSource = dt1;
                rptMarkers1.DataBind();
            }
            if (dt2.Rows.Count > 0)
            {
                rptMarkers2.DataSource = dt2;
                rptMarkers2.DataBind();
            }
        }


    }
}