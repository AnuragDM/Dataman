using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using BAL;
using System.Data;
using System.IO;
public partial class StorePermission : System.Web.UI.Page
{
    UploadData upd = new UploadData();
    protected void Page_Load(object sender, EventArgs e)
    {
        string pageName = Path.GetFileName(Request.Path);
        string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
        lblPageHeader.Text = Pageheader;
        if (!IsPostBack)
        {
            FillUserId();
        }
    }
    protected void btnfill_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(ddlUserId.SelectedValue) || ddlUserId.SelectedValue == "0")
            {
                ShowAlert("Please select a User");
                return;
            }
            string str = "select * From ( \n"+
            "SELECT sp.[StoreCode],pm.[PageId] as Page_Id,pm.[DisplayName],convert(bit, isnull(sp.[ViewP], 0)) [ViewP],convert(bit, isnull(sp.[AddP], 0)) [AddP],convert(bit, isnull(sp.[EditP], 0)) [EditP],convert(bit, isnull(sp.[DeleteP], 0)) [DeleteP],convert(bit, isnull(sp.[ExportP], 0)) [ExportP] from MastPage pm \n" +
             "LEFT OUTER JOIN StorePermission sp on pm.PageId = sp.Page_Id \n"+
              "WHERE (UserId='217') and pm.idx <> -1 and Module='Tracker' \n"+
               "union \n"+
               "SELECT distinct null ,pm.[PageId] ,pm.[DisplayName] ,convert(bit, 0) [ViewP],convert(bit, 0) [AddP],convert(bit, 0) [EditP],convert(bit, 0) [DeleteP],convert(bit, 0) [ExportP] from MastPage pm \n"+
               "LEFT OUTER JOIN StorePermission sp on pm.[PageId] = sp.Page_Id \n"+
               "where (page_id not in (select Page_Id from StorePermission WHERE UserId='217')) \n"+
               "and pm.idx <> -1 and Module='Tracker') a order by [DisplayName] \n";
          //  string str = "SELECT * from(SELECT sp.[StoreCode],pm.Id [Page_Id],pm.[DisplayName],convert(bit, isnull(sp.[ViewP], 0)) [ViewP],convert(bit, isnull(sp.[AddP], 0)) [AddP],convert(bit, isnull(sp.[EditP], 0)) [EditP],convert(bit, isnull(sp.[DeleteP], 0)) [DeleteP],convert(bit, isnull(sp.[ExportP], 0)) [ExportP] from PageMast pm LEFT OUTER JOIN StorePermission sp on pm.Id = sp.Page_Id WHERE (UserId='" + ddlUserId.SelectedValue + "') and pm.idx <> -1 union SELECT distinct null ,pm.Id [Page_Id] ,pm.[DisplayName] ,convert(bit, 0) [ViewP],convert(bit, 0) [AddP],convert(bit, 0) [EditP],convert(bit, 0) [DeleteP],convert(bit, 0) [ExportP] from PageMast pm LEFT OUTER JOIN StorePermission sp on pm.Id = sp.Page_Id where (page_id not in (select Page_Id from StorePermission WHERE UserId='" + ddlUserId.SelectedValue + "')) and pm.idx <> -1) a order by [DisplayName]";
            gvData.DataSource =DbConnectionDAL.getFromDataTable(str);
            gvData.DataBind();
            tr_btns.Attributes.Remove("Style");
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while loading records!");
        }
    }
    public void FillUserId()
    {
        try
        {
            string str = "select Id,Name+' ( '+cast(Id as Varchar(Max))+' )' as Name from MastLogin order by Name";
            fillDDLDirect(ddlUserId, str, "Id", "Name", 1);
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while loading records!");
        }
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
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        ClearData(); Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageName(), this);
    }
    public void ClearData()
    {
        Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageName(), this);
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlUserId.SelectedItem.Text == "--Select--")
            {
                ShowAlert("Please select a User");
                ddlUserId.SelectedItem.Text = "--Select--";
                gvData.DataSource = null;
                gvData.DataBind();
                return;
            }
            upd.DeleteUserPermission(ddlUserId.SelectedValue);
            foreach (GridViewRow gvr in gvData.Rows)
            {
                upd.InsertUpdatePermission
                    (
                    "",
                        ddlUserId.SelectedValue,
                        Convert.ToInt32(gvData.DataKeys[gvr.RowIndex]["Page_Id"]),
                        ((CheckBox)gvr.FindControl("ckView")).Checked,
                        ((CheckBox)gvr.FindControl("ckAdd")).Checked,
                        ((CheckBox)gvr.FindControl("ckEdit")).Checked,
                        ((CheckBox)gvr.FindControl("ckDelete")).Checked,
                        ((CheckBox)gvr.FindControl("ckExport")).Checked
                        
                    );
            }
            ShowAlert("Record saved successfully");
            gvData.DataSource = null;
            gvData.DataBind();
            FillUserId();
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while saving records!");
        }
    }
   
}