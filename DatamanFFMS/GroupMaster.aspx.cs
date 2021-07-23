using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DAL;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.Text.RegularExpressions;
using System.IO;

    public partial class GroupMaster : System.Web.UI.Page
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
                FillData();
            }
        }
        public void FillData()
        {
            try
            {
                string str = "select GroupID as Code,Description,Mobile from GroupMaster order by Description";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                //  dt = DataAccessLayer.DAL.getFromDataTable(str);
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Session["GroupName"] = "";
            Session["Mobile"] = "";
            Settings.Instance.RedirectCurrentPage("GroupMast.aspx", this);
        }
        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string code = gvData.DataKeys[e.RowIndex].Value.ToString();
                int i = upd.DeleteProdType(code, "D");
                if (i > 0) { FillData(); }
                else
                {
                    ShowAlert("Record cannot be deleted!");
                }

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
            string GroupID = gvData.SelectedDataKey.Value.ToString();
            string str = "select GroupID,Description,Mobile from GroupMaster where GroupID='" + GroupID + "'";
            //   dt = DataAccessLayer.DAL.getFromDataTable(str);
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                Session["GroupName"] = dt.Rows[0]["Description"].ToString();
                Session["Mobile"] = dt.Rows[0]["Mobile"].ToString();
                Session["ID"] = dt.Rows[0]["GroupID"].ToString();
                Settings.Instance.RedirectCurrentPage("GroupMast.aspx", this);
            }
        }

    }
