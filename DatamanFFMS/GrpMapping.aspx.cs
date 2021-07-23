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
using System.Data.SqlClient;
using System.Data;
using DAL;
using BAL;
using System.IO;
public partial class GrpMapping : System.Web.UI.Page
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
            string str = "select GroupID as Code,Description from GroupMaster order by Description";
            fillDDLDirect(ddlType, str, "Code", "Description", 1);
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
                ShowAlert("Please select a Group");
                return;
            }
            string str = "SELECT ID, PersonName,deviceno as Device FROM PersonMaster order by PersonName";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);// DataAccessLayer.DAL.getFromDataTable(str);
            gvData.DataSource = dt;
            gvData.DataBind();
            tr_sc.Attributes.Remove("style");
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while loading records!");
        }
    }
    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        FillData();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int j = 0;
        try
        {
            if (ddlType.SelectedItem.Text == "--Select--")
            {
                ShowAlert("Pleas select a Group");
                ddlType.SelectedItem.Text = "--Select--";
                gvData.DataSource = null;
                gvData.DataBind();
                return;
            }
            con.Open();
            upd.DeleteGroupMapping(ddlType.SelectedValue);
            foreach (GridViewRow row in gvData.Rows)
            {
                j = row.RowIndex;
                CheckBox cb = (CheckBox)row.FindControl("chk");
                if (cb.Checked == true)
                {
                   // string PartyID = DataAccessLayer.DAL.GetStringScalarVal("select ID from personmaster where DeviceNo = '" + gvData.Rows[j].Cells[2].Text.Trim() + "'");
                    string PartyID =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select ID from personmaster where DeviceNo = '" + gvData.Rows[j].Cells[2].Text.Trim() + "'"));// GetStringScalarVal("select ID from personmaster where DeviceNo = '" + gvData.Rows[j].Cells[2].Text.Trim() + "'");
                    int i = upd.InsertUpdateGroupMapping(0, PartyID, ddlType.SelectedValue);
                }
            }
            ShowAlert("Records saved successfully!");
            clear();
            gvData.DataSource = null;
            gvData.DataBind();
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
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Settings.Instance.RedirectCurrentPage("~/GrpMapping.aspx", this);
    }
    protected void btnfill_Click(object sender, EventArgs e)
    {
        try
        {
            FillData();
            string str = "select PersonMaster.DeviceNo from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);// DataAccessLayer.DAL.getFromDataTable(str);
            foreach (DataRow row in dt.Rows)
            {
                for (int k = 0; k < gvData.Rows.Count; k++)
                {
                    string value = row[0].ToString();
                    if (gvData.Rows[k].Cells[2].Text == value)
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
    public void clear()
    {
        FillType();
    }
}