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
    public partial class RoleMatrix : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                BindRole(); BindType(); BindModule(0);// Binddata();
            }
            string pageName = Path.GetFileName(Request.Path);
            string Headername = fillDisplayname(pageName);
            lblrolematrix.Text = Headername;
        }
        private string fillDisplayname(string pgname)
        {
            string displayname = string.Empty;
            string mastPartyQry1 = string.Empty;

            mastPartyQry1 = @"select DisplayName from mastpage where PageName in ('" + pgname + "') ";
            displayname = DbConnectionDAL.GetStringScalarVal(mastPartyQry1);
            return displayname;

        }
        #region BindDropdowns
        private void BindRole()
        {
            string strrole = "select roleId,rolename from MastRole order by rolename";
            fillDDLDirect(ddlrole, strrole, "roleId", "rolename", 1);
        }
        private void BindModule(int value)
        {
            
            //string strmodule = "select PageId,DisplayName from MastPage  where Level_Idx=1 and DisplayYN=1 and android='N' order by DisplayName";
            //fillDDLDirect(ddlmodule, strmodule, "PageId", "DisplayName", 1);
            string strdata = "";
            if (value == 0)
                strdata = "select PageId,DisplayName from MastPage  where Level_Idx=1 and DisplayYN=1 and android='N' order by DisplayName";
            else
                strdata = "select PageId,DisplayName from MastPage  where Level_Idx=1 and DisplayYN=1 and android='Y' order by DisplayName";
                fillDDLDirect(ddlmodule, strdata, "PageId", "DisplayName", 1);
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
        #endregion
        private void GetRoleMatrixList()
        {
            string straddqry = "";
            if (ddlrole.SelectedIndex > 0) { straddqry += " and mrp.roleid="+ ddlrole.SelectedValue +""; }
            if (ddlmodule.SelectedIndex > 0) { straddqry += " and mp.Parent_Id='" + ddlmodule.SelectedValue + "'"; }
            //if (ddltype.SelectedIndex > 0) { straddqry += " and mp.Parent_Id='" + ddltype.SelectedValue + "'"; }
            DataTable dt = new DataTable();
            string str = "select DisplayName,case ViewP when 1 then 'Yes' else 'No' end as ViewP,case AddP when 1 then 'Yes' else 'No' end as AddP,case EditP when 1 then 'Yes' else 'No' end as EditP,case DeleteP when 1 then 'Yes' else 'No' end as DeleteP,Module,RoleName from MastRolePermission Mrp inner join MastPage Mp on Mrp.PageId=Mp.PageId inner join MastRole Mr on Mrp.RoleId=Mr.RoleId where DisplayYN=1 "+ straddqry +"  order by RoleName ";

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            rpt.DataSource = dt;
            rpt.DataBind();
           
        }
        private void GetRoleMatrixforandroid()
        {
            string stradqry = "";
            if (ddlrole.SelectedIndex > 0) { stradqry += " and mrp.roleid=" + ddlrole.SelectedValue + ""; }
            if (ddlmodule.SelectedIndex > 0) { stradqry += " and mp.Parent_Id='" + ddlmodule.SelectedValue + "'"; }
            //if (ddltype.SelectedIndex > 0) { stradqry += " and mp.Parent_Id='" + ddltype.SelectedValue + "'"; }
            DataTable dt = new DataTable();
            string strtype = "select DisplayName,case ViewP when 1 then 'Yes' else 'No' end as ViewP,case AddP when 1 then 'Yes' else 'No' end as AddP,case EditP when 1 then 'Yes' else 'No' end as EditP,case DeleteP when 1 then 'Yes' else 'No' end as DeleteP,Module,RoleName from MastRolePermission_android Mrp inner join MastPage Mp on Mrp.PageId=Mp.PageId inner join MastRole Mr on Mrp.RoleId=Mr.RoleId where DisplayYN=1 " + stradqry + " order by RoleName ";

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strtype);

            rpt.DataSource = dt;
            rpt.DataBind();
            
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            try {
                if (ddltype.SelectedIndex == 0)
                GetRoleMatrixList();
                if (ddltype.SelectedIndex == 1)
                GetRoleMatrixforandroid();
                
           }
            catch (Exception ex) { ex.ToString(); }
        }
        private void BindType()
        {
              try {
            
            ddltype.Items.Insert(0, new ListItem("Web", "0"));
            ddltype.Items.Insert(1, new ListItem("Android", "1"));
              }
              catch (Exception ex) { ex.ToString(); }
        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindModule(ddltype.SelectedIndex);
           
        }
       
    }
}