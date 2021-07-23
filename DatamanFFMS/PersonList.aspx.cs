using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BusinessLayer;
using System.Data;
using BAL;
using System.Reflection;
//using BusinessLayer;
using System.IO;
using DAL;
using AstralFFMS.ServiceReferenceDMTracker;

public partial class PersonList : System.Web.UI.Page
{
    UploadData upd = new UploadData();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        string pageName = Path.GetFileName(Request.Path);
        string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
        lblPageHeader.Text = Pageheader;

        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(Request.QueryString["edit"]))
                Session["PersonDDl"] = "";
            FillGroup();
        }
        if (!string.IsNullOrEmpty(Request.QueryString["edit"]))
        {
            //ddlgrpname.SelectedValue = Session["Group"].ToString();

            if (!IsPostBack)
            {
                ddlgrpname.SelectedValue = Session["Group"].ToString();
                if (Session["PersonDDl"].ToString() != "")
                FillType();
                DropDownList2.SelectedValue = Session["PersonDDl"].ToString();
                FillData();
            }
        }
    }
    private void FillGroup()
    {

        string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
        fillDDLDirect(ddlgrpname, str, "Code", "Description", 1);

    }
    public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
    {
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
            xddl.Items.Insert(0, new ListItem("--Others--", "0"));
        }
        xdt.Dispose();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
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
        Settings.Instance.RedirectCurrentPage("PersonMast.aspx", this);
    }
    protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "";
            //string code = gvData.DataKeys[e.RowIndex].Value.ToString();
            //string strmaildetails = "SELECT PersonName,DeviceNo FROM PersonMaster WHERE ID=" + code + "";
            //DataTable dtmail = DbConnectionDAL.getFromDataTable(strmaildetails);
            //int i = upd.DeletePerson(code);
            //if (i > 0)
            //{ upd.SendDeletionEmail(dtmail.Rows[0]["PersonName"].ToString(), dtmail.Rows[0]["DeviceNo"].ToString(), baseUrl); FillData(); }
            //else { ShowAlert("Record cannot be deleted"); }
            
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "";
            string code = gvData.DataKeys[e.RowIndex].Value.ToString();
            string strmaildetails = "SELECT PersonName,DeviceNo FROM PersonMaster WHERE ID=" + code + "";
            DataTable dtmail = DbConnectionDAL.getFromDataTable(strmaildetails);
            int i = upd.DeletePerson(code, "N");
           if (i > 0)
            {
                WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
                DMT.DeletePersonLicense(dtmail.Rows[0]["DeviceNo"].ToString());
                upd.SendDeletionEmail(dtmail.Rows[0]["PersonName"].ToString(), dtmail.Rows[0]["DeviceNo"].ToString(), baseUrl); FillData();}
           else { ShowAlert("Record cannot be disabled"); }        
        
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while deleting records!");
        }
    }
    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvData.PageIndex = e.NewPageIndex;
        FillData();
    }
    protected void gvData_SelectedIndexChanged(object sender, EventArgs e)
    {
        string code = gvData.SelectedDataKey.Value.ToString();
        string str = "SELECT Alarm,AlarmDurationMins,SendSms,PersonName as PersonName, DeviceNo as DeviceNo,fromtime as fromtime,ToTime as ToTime,Interval as Interval,UploadInterval,RetryInterval,Degree,GpsLoc,MobileLoc,Sys_Flag,Mobile,SrMobile,SendSmsPerson,SendSmsSenior,Empcode,Long,Lat FROM PersonMaster where PersonMaster.ID ='" + code + "'";
        dt = DbConnectionDAL.getFromDataTable(str);
        if (dt.Rows.Count > 0)
        {
            Session["Group"] = ddlgrpname.SelectedValue.ToString();
            Session["PersonDDl"] = DropDownList2.SelectedValue.ToString();
            Session["DeviceNo"] = dt.Rows[0]["DeviceNo"].ToString();
            Session["PersonName"] = dt.Rows[0]["PersonName"].ToString();
            Session["FromTime"] = dt.Rows[0]["FromTime"].ToString();
            Session["ToTime"] = dt.Rows[0]["ToTime"].ToString();
            Session["Interval"] = dt.Rows[0]["Interval"].ToString();
            Session["ID"] = code;
            Session["UploadInterval"] = dt.Rows[0]["UploadInterval"].ToString();
            Session["RetryInterval"] = dt.Rows[0]["RetryInterval"].ToString();
            Session["Degree"] = dt.Rows[0]["Degree"].ToString();
            Session["GpsLoc"] = dt.Rows[0]["GpsLoc"].ToString();
            Session["MobileLoc"] = dt.Rows[0]["MobileLoc"].ToString();
            Session["Mobile"] = dt.Rows[0]["Mobile"].ToString();
            Session["Alarm"] = dt.Rows[0]["Alarm"].ToString();
            Session["AlarmDurationMins"] = dt.Rows[0]["AlarmDurationMins"].ToString();
            Session["SendSms"] = dt.Rows[0]["SendSms"].ToString();
            Session["SrMobile"] = dt.Rows[0]["SrMobile"].ToString();
            Session["SendSmsPerson"] = dt.Rows[0]["SendSmsPerson"].ToString();
            Session["SendSmsSenior"] = dt.Rows[0]["SendSmsSenior"].ToString();
            Session["Lat"] = dt.Rows[0]["Lat"].ToString();
            Session["Long"] = dt.Rows[0]["Long"].ToString();
            Session["EmpCode"] = dt.Rows[0]["Empcode"].ToString();
            if (dt.Rows[0]["Sys_Flag"].ToString() == "Y")
                Session["Sys_Flag"] = true;
            else
                Session["Sys_Flag"] = false;


            Settings.Instance.RedirectCurrentPage("PersonMast.aspx", this);
        }
    }
  
    protected void ddlgrpname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgrpname.SelectedValue == "0")
        {
            ShowAlert("please select a Group"); return;
        }
        FillType();
        gvData.DataSource = null;
        gvData.DataBind();
    }
    protected void btnfill_Click(object sender, EventArgs e)
    {

        FillData();
    }
    public void FillType()
    {
        try
        {
            // string str = "SELECT ID as id, PersonName as Person FROM PersonMaster order by PersonName";

            string str = "select PersonMaster.ID as id, PersonName as Person from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where Active is null and GrpMapp.GroupID = '" + ddlgrpname.SelectedValue + "' order by PersonMaster.PersonName";
           
            fillDDLDirect(DropDownList2, str, "id", "Person", 1);
        }
        catch (Exception)
        {

        }
    }
    public void FillData()
    {
        try
        {
            if (ddlgrpname.SelectedValue == "0")
            {
                ShowAlert("please select a Group"); return;
            }
            string addqry = "";
            if (!string.IsNullOrEmpty(DropDownList2.SelectedValue) && DropDownList2.SelectedValue != "0")
                addqry = " and PersonMaster.ID='" + DropDownList2.SelectedValue + "'";

            string str = "SELECT PersonMaster.ID, PersonName, DeviceNo FROM PersonMaster INNER JOIN GrpMapp ON PersonMaster.ID=GrpMapp.PersonID where Active is Null and GrpMapp.GroupID=" + ddlgrpname.SelectedValue + addqry + " order by PersonMaster.PersonName";
            dt = DbConnectionDAL.getFromDataTable(str);
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while loading records!");
        }
    }

    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
}