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
    public partial class DashboardTotalLeave : System.Web.UI.Page
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

            //sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson,tl.fromdate,tl.todate,tl.reason,tl.AppStatus from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join transleaverequest tl on tl.smid=a.smid WHERE a.smid IN (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and a.active=1 and a.smid<> " + Settings.Instance.SMID + " and replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') between FromDate and ToDate  and a.smid in (select smid from TransLeaveRequest with (nolock) where SMId in  (select SMId from mastsalesrep with (nolock) where  active=1  ) and replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') between FromDate and ToDate  and  IsNull(appstatus,'DSR') !='Reject'  and AppStatus not in ('Reject' ,'Pending')) Order By a.smname";
            sql = "select a.smid,a.smname,a.mobile,b.smname as reportingPerson,tl.fromdate,tl.todate,tl.reason,tl.AppStatus,case when a.Active='1' then 'Yes' else 'No' end as Active from mastsalesrep a left join mastsalesrep b on a.underid=b.smid left join transleaverequest tl on tl.smid=a.smid WHERE a.smid IN (select smid from mastsalesrepgrp with (nolock)  where maingrp = " + Settings.Instance.SMID + ") and a.smid<> " + Settings.Instance.SMID + " and replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') between FromDate and ToDate  and a.smid in (select smid from TransLeaveRequest with (nolock) where SMId in  (select SMId from mastsalesrep with (nolock)) and replace(convert(NVARCHAR, DateAdd(minute,330,'" + FromDate.Text + "'), 106), ' ', '/') between FromDate and ToDate  and  IsNull(appstatus,'DSR') !='Reject') and AppStatus not in ('Reject') Order By a.smname";
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