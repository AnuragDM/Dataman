using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using BusinessLayer;
using System.Data;
using System.Data.SqlClient;
using DAL;
using BAL;
using System.IO;
public partial class UserGrpMapp : System.Web.UI.Page
{
    UploadData upd = new UploadData();
    SqlConnection con = Connection.Instance.GetConnection();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        string pageName = Path.GetFileName(Request.Path);
        string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
        lblPageHeader.Text = Pageheader;
        if (!IsPostBack)
        {
            FillType();
        }
    }
    public void FillType()
    {
        try
        {
            string str = "select id as  UserId,Name+' ( '+ cast(Id as Varchar(Max))+' )' as Name from MastLogin order by Name";
            fillDDLDirect(ddlType, str, "UserId", "Name", 1);
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
    public void FillData()
    {
        try
        {
            if (string.IsNullOrEmpty(ddlType.SelectedValue) || ddlType.SelectedValue == "0")
            {
                ShowAlert("Please select a User");
                return;
            }

            string str = "select GroupID as Code,Description from GroupMaster order by Description";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);// DataAccessLayer.DAL.getFromDataTable(str);
            tr_sc.Attributes.Remove("style");
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while loading records!");
        }
    }

    protected void btnfill_Click(object sender, EventArgs e)
    {
        try
        {

            FillData();
            string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + ddlType.SelectedValue + "' order by GroupMaster.Description";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);// DataAccessLayer.DAL.getFromDataTable(str);
            foreach (DataRow row in dt.Rows)
            {
                for (int k = 0; k < gvData.Rows.Count; k++)
                {
                    string value = row[1].ToString();
                    if (gvData.Rows[k].Cells[1].Text == value)
                    {
                        CheckBox cb = (CheckBox)(gvData.Rows[k].FindControl("chk"));
                        cb.Checked = true;
                    }
                }

            }
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while loading records!");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int j = 0;
        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
        try
        {
            if (ddlType.SelectedItem.Text == "--Select--")
            {
                ShowAlert("Please select a User");
                ddlType.SelectedItem.Text = "--Select--"; gvData.DataSource = null;
                gvData.DataBind();
                return;
            }
            con.Open();
            upd.DeleteUserMapping(ddlType.SelectedValue);
            foreach (GridViewRow row in gvData.Rows)
            {
                j = row.RowIndex;
                CheckBox cb = (CheckBox)row.FindControl("chk");
                if (cb.Checked == true)
                {
                  //  string GroupID = DataAccessLayer.DAL.GetStringScalarVal("select GroupID from GroupMaster where Description = '" + gvData.Rows[j].Cells[1].Text + "'");
                    string GroupID =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select GroupID from GroupMaster where Description = '" + gvData.Rows[j].Cells[1].Text + "'"));// DataAccessLayer.DAL.GetStringScalarVal("select GroupID from GroupMaster where Description = '" + gvData.Rows[j].Cells[1].Text + "'");
                    int i = upd.InsertUserMapping(0, ddlType.SelectedValue, GroupID);
                }
            }
            ShowAlert("Records saved successfully!");
            clear();
            gvData.DataSource = null;
            gvData.DataBind();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while saving records!");
        }
    }
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    public void clear()
    {
        FillType();
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Settings.Instance.RedirectCurrentPage("~/UserGrpMapp.aspx", this);
    }
}