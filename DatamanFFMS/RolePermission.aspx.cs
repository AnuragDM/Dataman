using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BusinessLayer;
using DAL;
using System.IO;
namespace AstralFFMS
{
    public partial class RolePermission : System.Web.UI.Page
    {
       Data d = new Data();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                BindRoles(); BindParentMenu();
            }

        }

        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt =DbConnectionDAL.GetDataTable(CommandType.Text,xmQry);
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
        #region Bind Dropdowns
        private void BindRoles()
        {
            try
            {
                DataTable dtrole = new DataTable();
                //Ankita - 20/may/2016- (For Optimization)
                //string strole = "select * from MastRole order by RoleName";
                string strole = "select RoleId,RoleName from MastRole order by RoleName";
                fillDDLDirect(ddlRole, strole, "RoleId", "RoleName", 1);
            }
            catch (Exception ex) { ex.ToString(); }
        }
        private void BindParentMenu()
        {
            try
            {//Ankita - 20/may/2016- (For Optimization)
                DataTable dtPmenu = new DataTable();
                //string stPmenu = "select * from MastPage where Level_Idx=1 and DisplayYN=1 order by DisplayName";
                string stPmenu = "select PageId,DisplayName from MastPage where Level_Idx=1 and DisplayYN=1 and android='N' order by DisplayName";
                fillDDLDirect(ddlModule, stPmenu, "PageId", "DisplayName", 1);
            }
            catch (Exception ex) { ex.ToString(); }
        }
        #endregion
        protected void btnshow_Click(object sender, EventArgs e)
        {
            try
            {
                //string qry = "select * from MastRolePermission mrp Inner join MastPage mp On mrp.PageId=mp.PageId " +
                //    "where mrp.RoleId=" + ddlRole.SelectedValue + " and Parent_Id=" + ddlModule.SelectedValue + " and mp.DisplayYN =1 order by mp.DisplayName";
                BindGvData();
            }
            catch (Exception ex) { ex.ToString(); }
        }

        private void BindGvData()
        {
            string qry = @" SELECT * from(SELECT DisplayName,ViewP,AddP,EditP,DeleteP,ExportP,mrp.PageId FROM   mastrolepermission mrp INNER JOIN mastpage mp ON mrp.pageid = mp.pageid WHERE  mrp.roleid = " + ddlRole.SelectedValue + " AND parent_id = " + ddlModule.SelectedValue + " AND mp.displayyn = 1 " +
                 " UNION ALL " +
           " SELECT DisplayName,  CAST	( 0 AS BIT) as ViewP,  CAST	( 0 AS BIT) as AddP,  CAST	( 0 AS BIT) as EditP,  CAST	( 0 AS BIT) as DeleteP,CAST	( 0 AS BIT) as ExportP,PageId FROM MastPage WHERE PageId NOT IN (SELECT pageid FROM MastRolePermission WHERE RoleId=" + ddlRole.SelectedValue + " )AND Parent_Id=" + ddlModule.SelectedValue + "  AND DisplayYN=1  ) a ORDER BY a.DisplayName ";

            DataTable dt =DbConnectionDAL.GetDataTable(CommandType.Text,qry);
            if (dt.Rows.Count > 0)
            {
                divbtns.Visible = true;
                gvData.DataSource = dt;
                 gvData.DataBind();
                gvData.Visible = true;
                foreach (GridViewRow row in gvData.Rows)
                {
                    if (ddlModule.SelectedItem.Text == "Reports")
                    {
                        gvData.Columns[3].Visible = false;
                        gvData.Columns[4].Visible = false;
                        gvData.Columns[5].Visible = false;
                    }
                    else
                    {
                        gvData.Columns[3].Visible = true;
                        gvData.Columns[4].Visible = true;
                        gvData.Columns[5].Visible = true;
                    }

                   
                }
            }
            else
            {
                divbtns.Visible = false;
                gvData.DataSource = null;
                gvData.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int res = 0; int cnt = 0; int page_ID = 0;
                foreach (GridViewRow gvr in gvData.Rows)
                {
                    if (((CheckBox)gvr.FindControl("ckView")).Checked == true)
                    { cnt = 1; }
                    page_ID = Convert.ToInt32(gvData.DataKeys[gvr.RowIndex]["PageId"].ToString());

                    res = d.InsertUpdatePermission
                         (
                             Convert.ToInt32(ddlRole.SelectedValue),
                            Convert.ToInt32(gvData.DataKeys[gvr.RowIndex]["PageId"].ToString()),
                             ((CheckBox)gvr.FindControl("ckView")).Checked,
                             ((CheckBox)gvr.FindControl("ckAdd")).Checked,
                             ((CheckBox)gvr.FindControl("ckEdit")).Checked,
                             ((CheckBox)gvr.FindControl("ckDelete")).Checked,
                             ((CheckBox)gvr.FindControl("ckExport")).Checked
                         );
                }
                if (cnt < 1)
                {
                    string qry = "update MastRolePermission set ViewP=0 where roleid=" + ddlRole.SelectedValue + " and pageid=(select Parent_Id from MastPage where PageId=" + page_ID + ")";
                    DbConnectionDAL.GetScalarValue(CommandType.Text,qry);
                }
                if (res > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully!');", true);
                    gvData.DataSource = null;
                    gvData.DataBind();
                    gvData.Visible = false;
                    ddlModule.SelectedIndex = 0;
                    ddlRole.SelectedIndex = 0;
                    divbtns.Visible = false;
                    //  Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageName(), this);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Errormessage", "Errormessage('There are some errors while saving records!');", true);

                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            BindGvData();
        }
    }
}