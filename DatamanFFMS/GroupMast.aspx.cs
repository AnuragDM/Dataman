using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.IO;
using DAL;
using System.Data;
using System.Text.RegularExpressions;

public partial class GroupMast : System.Web.UI.Page
{
    UploadData upd = new UploadData();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["GroupName"].ToString() != "")
            {
                getdata();

            }
        }
    }
    int res;
    protected void btnback_Click(object sender, EventArgs e)
    {
        ClearData();
        Settings.Instance.RedirectCurrentPage("~/GroupMaster.aspx", this);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Regex myRegularExpression = new Regex("^[0-9]+$");
            if (myRegularExpression.IsMatch(txtType.Text))
            {
                ShowAlert("Group Name should be alphanumeric!");
                return;
            }
            int mbllen = txtmobile.Text.Length;
            if (mbllen < 10) { ShowAlert("Please enter 10 digit mobile number"); return; }
            string MarkerData = "0";
            if (rdbAddress.Checked)
                MarkerData = "1";


            if (hdfCode.Value == "")
            {
                string us =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Description from GroupMaster where Description='" + txtType.Text.Trim() + "'"));
                //string us = DataAccessLayer.DAL.GetStringScalarVal("select Description from GroupMaster where Description='" + txtType.Text.Trim() + "'");
                if (string.IsNullOrEmpty(us))
                {
                    res = upd.InsertUpdateProdType("0", txtType.Text, "A", txtmobile.Text.Trim(), MarkerData);
                    ShowAlert("Record saved successfully!");
                    txtType.Text = ""; txtmobile.Text = "";
                }
                else
                {
                    ShowAlert("This Group Already Exist");
                }
            }
            else
            {
                string us =Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Description from GroupMaster where Description='" + txtType.Text.Trim() + "' and GroupID <> '" + hdfCode.Value + "'"));// DataAccessLayer.DAL.GetStringScalarVal("select Description from GroupMaster where Description='" + txtType.Text.Trim() + "' and GroupID <> '" + hdfCode.Value + "'");
                if (string.IsNullOrEmpty(us))
                {
                    res = upd.InsertUpdateProdType(hdfCode.Value, txtType.Text, "A", txtmobile.Text.Trim(), MarkerData);
                    ShowAlert("Record Updated successfully!");
                    txtType.Text = ""; txtmobile.Text = "";
                    Settings.Instance.RedirectCurrentPage("~/GroupMaster.aspx", this);
                }
                else
                {
                    ShowAlert("Already Exist Group Name");
                }
            }
        }
        catch (Exception)
        {
            ShowAlert("There are some errors while saving records!");

        }
    }

    public void ClearData()
    {
        Session["GroupName"] = "";
        Session["Mobile"] = "";
        Session["ID"] = "";
    }
    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Settings.Instance.RedirectCurrentPage("~/GroupMaster.aspx", this);
    }
    protected void getdata()
    {
        txtType.Text = Session["GroupName"].ToString();
        txtmobile.Text = Session["Mobile"].ToString();
        hdfCode.Value = Session["ID"].ToString();
    }
}