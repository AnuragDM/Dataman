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
using System.Globalization;
using System.Collections;
using Telerik.Web.UI;
using System.Web.Services;
using Newtonsoft.Json;
namespace AstralFFMS
{
    public partial class DashboardTotalMember : System.Web.UI.Page
    {
        string roleType = "", total = "";
        DataTable dtEmployee = null;
        string sql = "";
        string secondarySql = "";
        string PrimarySql = "";
        string UnApprovedSql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FromDate.Text = Request.QueryString["Date"];
                this.fillAllRecord();
            }
        }
        public void fillAllRecord()
        {
            roleType = Settings.Instance.RoleType;
            //if (roleType.Equals("Admin")) sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson ,mr.RoleType,case when a.Active=1 then 'Yes' else 'No' end Active from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where  a.smname not in ('.') order by a.smname asc";
            //else sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson ,mr.RoleType,case when a.Active=1 then 'Yes' else 'No' end Active from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where a.smname not in ('.') and a.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) order by a.smname asc";
            if (roleType.Equals("Admin")) sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson ,mr.RoleType,case when a.Active=1 then 'Yes' else 'No' end Active from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where  a.smid not in (" + Settings.Instance.SMID +" ) and a.active=1 order by a.smname asc";
            else sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson ,mr.RoleType,case when a.Active=1 then 'Yes' else 'No' end Active from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join mastrole mr on mr.roleid=a.roleid where a.smid not in (" + Settings.Instance.SMID + " ) and a.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and a.active=1 order by a.smname asc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

    }
}