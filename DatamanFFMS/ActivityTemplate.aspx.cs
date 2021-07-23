using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Data;
using System.IO;

namespace AstralFFMS
{
    public partial class ActivityTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            //btnExport.CssClass = "btn btn-primary";
            //btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            //_exportp = Convert.ToBoolean(SplitPerm[4]);
            if (Convert.ToBoolean(SplitPerm[0]) == false)
            {
                Response.Redirect("~/Logout.aspx");
            }

            if (Convert.ToBoolean(SplitPerm[1]) == true)
            {
                hidsave.Value = "true";
            }
            else
            {
                hidsave.Value = "false";

            }


            if (Convert.ToBoolean(SplitPerm[2]) == true)
            {
                hidupdate.Value = "true";
            }
            else
            {
                hidupdate.Value = "false";

            }



        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
         
        }
        protected void btnFind_Click1(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender,EventArgs e)
        {

        }
        protected void btncancel1_Click(object sender, EventArgs e)
        { 
        
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string col = "", str = "";
            str = "SELECT STUFF( (SELECT ', ' +quotename(t2.COL) FROM (select  MAC.AttributeField  AS  COL from TransActivity TA LEFT JOIN MastActivityCustom MAC ON MAC.Custom_Id=TA.CustomFieldiD) t2 FOR XML PATH ('')), 1, 1, '') as name";
            col = DbConnectionDAL.GetStringScalarVal(str);
            str = "SELECT * from   ( select  MAC.AttributeField,TA.CustomValue   from TransActivity TA LEFT JOIN MastActivityCustom MAC ON MAC.Custom_Id=TA.CustomFieldiD ) x pivot  ( max(CustomValue) for AttributeField in (" + col  + ") ) p";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);



        }
    }
}