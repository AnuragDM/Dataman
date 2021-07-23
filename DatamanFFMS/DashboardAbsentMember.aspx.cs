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
    public partial class DashboardAbsentMember : System.Web.UI.Page
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
            if (roleType.Equals("Admin")) /*sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson from mastsalesrep a left join mastsalesrep b on a.underid=b.smid where a.smname not in ('.') and a.smid in (select smid from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock) ) and active=1  and  SMId not in (select  SMId from TransVisit with (nolock) where vdate =  replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') ) AND SMId NOT IN (SELECT SMId from TransLeaveRequest with (nolock) where Fromdate = replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') and AppStatus not in ('Reject' ,'Pending'))) Order By a.smname";*/

            sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson,'Absent' as Status,case when a.Active='1' then 'Yes' else 'No' end as Active from mastsalesrep a left join mastsalesrep b on a.underid = b.smid where a.smname not in ('.') and a.active=1  and a.smid in (select smid from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock) ) and SMId not in (select  SMId from TransVisit with (nolock) where vdate = replace(convert(NVARCHAR, DateAdd(minute, 330, '" + FromDate.Text + "'), 106), ' ', '/') ) AND SMId NOT IN (SELECT SMId from TransLeaveRequest with (nolock) where replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') between FromDate and ToDate and AppStatus not in ('Reject' ,'Pending'))) Union All select a.smid,a.smname,a.mobile,b.smname as reportingPerson,'Reject' as Status,case when a.Active='1' then 'Yes' else 'No' end as Active from TransVisit trv left join mastsalesrep a on trv.SMId = a.SMId left join mastsalesrep b on a.underid = b.smid where trv.vdate = '" + FromDate.Text + "' and IsNull(trv.appstatus, 'DSR') = 'Reject' and a.smname not in ('.') Order By Status asc,Active desc,a.smname";
            else
                //sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson from mastsalesrep a left join mastsalesrep b on a.underid=b.smid where a.smname not in ('.') and a.smid in (select smid from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and active=1 and smid<> " + Settings.Instance.SMID + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/'))AND SMId NOT IN (SELECT SMId from TransLeaveRequest with (nolock) where Fromdate = replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') and AppStatus not in ('Reject' ,'Pending'))) Order By a.smname";

            sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson,'Absent' as Status,case when a.Active='1' then 'Yes' else 'No' end as Active from mastsalesrep a left join mastsalesrep b on a.underid=b.smid where a.smname not in ('.') and a.active=1  and a.smid in (select smid from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and smid<> " + Settings.Instance.SMID + " and SMId not in (select  SMId from TransVisit with (nolock) where vdate = replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/')) AND SMId NOT IN (SELECT SMId from TransLeaveRequest with (nolock) where replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') between FromDate and ToDate and AppStatus not in ('Reject' ,'Pending'))) Union All select  a.smid,a.smname,a.mobile,b.smname as reportingPerson,'Reject' as Status,case when a.Active='1' then 'Yes' else 'No' end as Active from TransVisit trv left join mastsalesrep a on trv.SMId=a.SMId left join mastsalesrep b on a.underid=b.smid where vdate ='" + FromDate.Text + "' and trv.SMId in (select SMId from mastsalesrep with (nolock) where smid in (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and smid<> " + Settings.Instance.SMID + " ) and IsNull(appstatus,'DSR') ='Reject' Order By Status asc,Active desc,a.smname";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                this.fillAllRecord();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch(Exception ex)
            { ex.ToString(); }
        }

    }
}