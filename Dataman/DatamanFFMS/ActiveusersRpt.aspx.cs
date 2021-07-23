using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
//using DataAccessLayer;
using System.Data;
using System.Text.RegularExpressions;
using DAL;
using BAL;
public partial class ActiveusersRpt : System.Web.UI.Page
{
    UploadData upd = new UploadData();
    Settings SetObj = Settings.Instance;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnExport);
        if (!IsPostBack)
        {
            //txtfordate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");

            GetGroupName();
            //       Button4_Click(sender, e);
        }
        if (SetObj.GroupID != "0" && SetObj.GroupID != "")
        {
            if (!IsPostBack)
                ddlgrp.SelectedValue = SetObj.GroupID;
        }
        else
        {
            SetObj.GroupID = ddlgrp.SelectedValue;
        }
    }
    private void GetGroupName()
    {
        string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
      fillDDLDirect(ddlgrp, str, "Code", "Description", 1);
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
    private void BindGrid()
    {
        try
        {
            string qry = "";
            //if (ddlstatus.SelectedValue == "1")
            //{
            //    qry = "select status='Active',personname+' ('+deviceno+')' as Person,(select description from groupmaster where groupid =" + ddlgrp.SelectedValue + ") as name from personmaster Inner join grpmapp on personmaster.id=grpmapp.personid where " +
            //          " groupid='" + ddlgrp.SelectedValue + "' and deviceno  in(select distinct(deviceno) from locationdetails  where convert(varchar(12),currentdate,105)= convert(varchar(12),cast('" + txtfordate.Text.Trim() + "' as datetime),105)) order by personname asc";
            //}
            //else
            //{

            //qry = "select 'Active' as status,personname+' ('+PM.deviceno+')' as Person,'" + ddlgrp.SelectedItem.Text + "' as name,isnull(Version,'') as version,PM.Mobile,VM.created_date as Versiondate from personmaster PM left join Version_Mast VM on PM.deviceno=VM.deviceid where pm.ID in (select personid from GrpMapp where groupid='" + ddlgrp.SelectedValue + "') Order by Version desc,person";
            qry = "select 'Active' as status,concat(PM.PersonName, ' ('+PM.deviceno +')',' ('+PM.empcode ) + ' )' as Person,'" + ddlgrp.SelectedItem.Text + "' as name,isnull(Version,'') as version,PM.Mobile,VM.created_date as Versiondate from personmaster PM left join Version_Mast VM on PM.deviceno=VM.deviceid where pm.ID in (select personid from GrpMapp where groupid='" + ddlgrp.SelectedValue + "') Order by Version desc,person";


            //qry = "select status='Inactive',personname+' ('+deviceno+')' as Person,(select description from groupmaster where groupid =" + ddlgrp.SelectedValue + ") as name from personmaster Inner join grpmapp on personmaster.id=grpmapp.personid where " +
            //           " groupid='" + ddlgrp.SelectedValue + "' and deviceno not in(select distinct(deviceno) from locationdetails  where convert(varchar(12),currentdate,105)= convert(varchar(12),cast('" + txtfordate.Text.Trim() + "' as datetime),105)) order by personname asc";


            //}
            DataTable dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, qry);// DataAccessLayer.DAL.getFromDataTable(qry);
            if (dtbl.Rows.Count > 0)
            {
                GridView1.DataSource = dtbl;
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
        catch (Exception ex) { ex.ToString(); }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindGrid();
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    public void ClearData()
    {
        Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageNameWithQueryString(), this);
    }
    protected void ddlgrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetObj.GroupID = ddlgrp.SelectedValue;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            // GridViewExportUtil.Export("ActiveUsersReport", GridView1);
            GridViewExportUtil.ExportGridToCSV(GridView1, "ActiveUsersReport");
        }
        catch (Exception ex)
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