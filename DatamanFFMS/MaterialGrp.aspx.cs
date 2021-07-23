using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.IO;

namespace AstralFFMS
{
    public partial class MaterialGrp : System.Web.UI.Page
    {
        BAL.MaterialGroup MG = new MaterialGroup();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["ItemId"] = parameter;
                FillMatGrpControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);         
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
               //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                chkIsAdmin.Checked = true;
                btnDelete.Visible = false;

                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["ItemId"] != null)
                {
                    FillMatGrpControls(Convert.ToInt32(Request.QueryString["ItemId"]));
                }
            }
        }
        private void fillRepeter()
        {
            string str = @"select *,case Active when 1 then 'Yes' else 'No' end as Active1 from MastItem where ItemType='MATERIALGROUP'";
            DataTable MaterialGrpdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = MaterialGrpdt;
            rpt.DataBind();

            MaterialGrpdt.Dispose();
        }
        private void FillMatGrpControls(int ItemId)
        {
            try
            { //Ankita - 20/may/2016- (For Optimization)
                //string MatGrpQry = @"select * from MastItem where ItemId=" + ItemId;
                string MatGrpQry = @"select ItemName,SyncId,Active from MastItem where ItemId=" + ItemId;
                DataTable MatGrpValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, MatGrpQry);
                if (MatGrpValueDt.Rows.Count > 0)
                {
                    MaterialGroup.Value = MatGrpValueDt.Rows[0]["ItemName"].ToString();
                    SyncId.Value = MatGrpValueDt.Rows[0]["SyncId"].ToString();
                   // ItemCode.Value = MatGrpValueDt.Rows[0]["ItemCode"].ToString();
                    if (Convert.ToBoolean(MatGrpValueDt.Rows[0]["Active"]) == true)
                    {
                        chkIsAdmin.Checked = true;
                    }
                    else
                    {
                        chkIsAdmin.Checked = false;
                    }
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
                MatGrpValueDt.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private void InsertMatGrp()
        {
            string active = "0";
            if (chkIsAdmin.Checked)
                active = "1";


            int retval = MG.InsertMatGrp(MaterialGroup.Value, SyncId.Value, active, ItemCode.Value);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate MaterialGroup Exists');", true);
                MaterialGroup.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastItem set SyncId='" + retval + "' where Itemid=" + retval + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                MaterialGroup.Focus();
            }

        }
        private void UpdateMatGrp()
        {
           string active = "0";
            if (chkIsAdmin.Checked)
                active = "1";


            int retval = MG.UpdateMatGrp(Convert.ToInt32(ViewState["ItemId"]),MaterialGroup.Value, SyncId.Value, active, ItemCode.Value);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate MaterialGroup Exists');", true);
                MaterialGroup.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastItem set SyncId='" + retval + "' where Itemid=" + Convert.ToInt32(ViewState["ItemId"]) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                MaterialGroup.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
       
       

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        private void ClearControls()
        {
            MaterialGroup.Value = string.Empty; SyncId.Value = string.Empty; ItemCode.Value = string.Empty;
            chkIsAdmin.Checked = false;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = MG.delete(Convert.ToString(ViewState["ItemId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    MaterialGroup.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    MaterialGroup.Focus();
                }
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/MaterialGrp.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateMatGrp();
                }
                else
                {
                   InsertMatGrp();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");

        }
    }
}