using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.IO;

namespace AstralFFMS
{
    public partial class TargetReportCrm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            //if (btnsave.Text == "Save")
            //{
            //    //btnsave.Enabled = Convert.ToBoolean(SplitPerm[1]);
            //    //btnsave.CssClass = "btn btn-primary";
            //}
            //else
            //{
            //    //btnsave.Enabled = Convert.ToBoolean(SplitPerm[2]);
            //    //btnsave.CssClass = "btn btn-primary";
            //}
            if (!IsPostBack)
            {
                //btnsave.Visible = false;
                fillfyear(); fillddlforsp();
            }
        }
        private void fillfyear()
        {
            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlyear.DataSource = dt;
                ddlyear.DataTextField = "Yr";
                ddlyear.DataValueField = "id";
                ddlyear.DataBind();
            }
            ddlyear.Items.Insert(0, new ListItem("-- Select --", "0"));
        }


        private string checkRole()
        {
            return Settings.Instance.RoleType;
        }
        private void fillddlforsp()
        {

            string query = @"select roletype,rolevalue from mastroletype";
            fillDDLDirect(ddlroleforsp, query, "rolevalue", "roletype", 1);
            query = @"select areaname,areaid from mastarea where areatype='state'";
            fillDDLDirect(ddlstate, query, "areaid", "areaname", 1);

        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            //function to fill drop down
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
        protected void ddlroleforsp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = @"select smid ,smname from mastsalesrep where salesreptype='" + ddlroleforsp.SelectedItem.Value + "'";
            fillDDLDirect(ddlsp, qry, "smid", "smname", 1);
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  string query = @"select areaname,areaid from mastarea where areatype='city' and underid="+ddlstate.SelectedItem.Value+"";
            string query = @"select cityid,cityname from viewgeo  where stateid=" + ddlstate.SelectedItem.Value + "";
            fillDDLDirect(ddlcity, query, "cityid", "cityname", 1);
        }

        protected void btngo_Click(object sender, EventArgs e)
        {


            string[] year = ddlyear.SelectedItem.Text.Split('-'); string city = null; string city1 = null;
            if (ddlstate.SelectedItem.Value != "0")
            {
                if (ddlcity.SelectedItem.Value != "0")
                {
                    city = "where msp.cityid in (" + ddlcity.SelectedItem.Value + ")";
                    city1 = "and msp.cityid in (" + ddlcity.SelectedItem.Value + ")";
                }
                else
                {
                    //string sql = "select cityid from viewgeo where stateid=" + ddlstate.SelectedItem.Value + "";
                    city = "where msp.cityid in (select cityid from viewgeo where stateid=" + ddlstate.SelectedItem.Value + ")";
                    city1 = "and msp.cityid in (select cityid from viewgeo where stateid=" + ddlstate.SelectedItem.Value + ")";
                }
            }
            if (ddlperiod.SelectedItem.Value == "1")
            {
                if (ddlroleforsp.SelectedItem.Value != "0")
                {
                    if (ddlsp.SelectedItem.Value != "0")
                    {
                        MonthlyPW(city, city1); statuswisemonthlyPW(city, city1);
                    }
                    else
                    {
                        MonthlyRW(city, city1); statuswisemonthlyRW(city, city1);
                    }
                }
                gridvisible(1);
            }
            else if (ddlperiod.SelectedItem.Value == "2")
            {
                if (ddlroleforsp.SelectedItem.Value != "0")
                {
                    if (ddlsp.SelectedItem.Value != "0")
                    {
                        quarterlyPW(city, city1); statuswiseQuarterlyPW(city, city1);
                    }
                    else
                    {
                        quarterlyRW(city, city1); statuswiseQuarterlyRW(city, city1);
                    }
                }
                gridvisible(2);
            }
            else if (ddlperiod.SelectedItem.Value == "3")
            {
                if (ddlroleforsp.SelectedItem.Value != "0")
                {
                    if (ddlsp.SelectedItem.Value != "0")
                    {
                        HalfyearlyPW(city, city1); statuswiseHalfyearlyPW(city, city1);
                    }
                    else
                    {
                        HalfyearlyRW(city, city1); statuswisehalfyearlyRW(city, city1);
                    }
                }
                gridvisible(3);
            }
            DealReport();
        }
        private void MonthlyRW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');


            string sql = @"select isnull(apr,'0.00') apr,isnull(may,'0.00') may,isnull(jun,'0.00') jun,isnull(jul,'0.00') jul,isnull(aug,'0.00') aug,isnull(sep,'0.00') sep,isnull(oct,'0.00') oct,isnull(nov,'0.00') nov,isnull(dec,'0.00') dec,isnull(jan,'0.00') jan,isnull(feb,'0.00') feb,isnull(mar,'0.00') mar ,year,roletype,Entry, Total

from (
 select (select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   " + city + " group by manager ,mon,year,msp.salesreptype ) t2 where mon=04 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as apr,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=05 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as may,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=06 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jun,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=07 and year=" + year[0] + "  and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jul,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=08  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as aug,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=09  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as sep ,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=10  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as oct,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=11  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as nov,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=12  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as dec,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=01  and year=" + year[1] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jan,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=02 and  year=" + year[1] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as feb,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=03 and   year=" + year[1] + " and  salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as mar ,'" + ddlyear.SelectedItem.Text + "' as year,'" + ddlroleforsp.SelectedItem.Value + "' as roletype ,'Target Accomplished' Entry,'Expected Total' as Total";

            sql += @"   union    
	
  select  sum (t1.apr) apr,sum (t1.may) may,sum (t1.jun) jun,sum (t1.july) july,sum (t1.aug) aug,sum (t1.sep) sep,sum (t1.oct) oct,sum (t1.nov) nov,sum (t1.dec) dec,sum (t1.jan) jan,sum (t1.feb) feb,sum (t1.mar) mar,year,roletype,'Target' Entry,'Target' as Total from
   (
  select sum (tc.apr) apr,sum (tc.may) may,sum (tc.jun) jun,sum (tc.july) july,sum (tc.aug) aug,sum (tc.sep) sep,sum (tc.oct) oct,sum (tc.nov) nov,sum (tc.dec) dec,sum (tc.jan) jan,sum (tc.feb) feb,sum (tc.mar) mar,year,roletype from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
 where tc.entrytype='RW' and tc.year='" + ddlyear.SelectedItem.Text + "' and roletype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "";

            sql += @"and tc.smid not in (select smid from tbl_crmtarget where year='" + ddlyear.SelectedItem.Text + "' and entrytype='PW' and roletype='" + ddlroleforsp.SelectedItem.Value + "')group by year,roletype union select sum (tc.apr) apr,sum (tc.may) may,sum (tc.jun) jun,sum (tc.july) july,sum (tc.aug) aug,sum (tc.sep) sep,sum (tc.oct) oct,sum (tc.nov) nov,sum (tc.dec) dec,sum (tc.jan) jan,sum (tc.feb) feb,sum (tc.mar) mar,year,roletype from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid where tc.entrytype='PW' and tc.year='" + ddlyear.SelectedItem.Text + "' and roletype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + " group by year,roletype) t1  group by year,roletype ) t";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 1)
            {
                if (dt.Rows[0]["Total"].ToString() == "Total")
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(No Record Found);", true); return;
                }
                GridView4.DataSource = dt;
                GridView4.DataBind();

            }
            else
            {
                GridView4.DataSource = null;
                GridView4.DataBind();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found');", true); return;
               
            }

        }
        private void quarterlyRW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');

            string sql = @"select sum(isnull(apr,'0.00') +isnull(may,'0.00') + isnull(jun,'0.00') ) Q1 ,sum( isnull(jul,'0.00')+  isnull(aug,'0.00') +isnull(sep,'0.00') ) Q2, sum (isnull(oct,'0.00') + isnull(nov,'0.00') + isnull(dec,'0.00') ) Q3, sum (isnull(jan,'0.00') + isnull(feb,'0.00') + isnull(mar,'0.00') ) Q4, year, roletype , Entry,Total 
 from ( select (select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=04 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as apr,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=05 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as may,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=06 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jun,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=07 and year=" + year[0] + "  and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jul,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=08  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as aug,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=09  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as sep ,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=10  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as oct,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=11  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as nov,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=12  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as dec,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=01  and year=" + year[1] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jan,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=02 and  year=" + year[1] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as feb,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=03 and   year=" + year[1] + " and  salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as mar ,'" + ddlyear.SelectedItem.Text + "' as year,'" + ddlroleforsp.SelectedItem.Value + "' as roletype ,'Target Accomplished' Entry,'Expected Total' as Total";

            sql += @"   union    
	
  select  sum (t1.apr) apr,sum (t1.may) may,sum (t1.jun) jun,sum (t1.july) july,sum (t1.aug) aug,sum (t1.sep) sep,sum (t1.oct) oct,sum (t1.nov) nov,sum (t1.dec) dec,sum (t1.jan) jan,sum (t1.feb) feb,sum (t1.mar) mar,year,roletype,'Target' Entry,'Target' as Total from
   (
  select sum (tc.apr) apr,sum (tc.may) may,sum (tc.jun) jun,sum (tc.july) july,sum (tc.aug) aug,sum (tc.sep) sep,sum (tc.oct) oct,sum (tc.nov) nov,sum (tc.dec) dec,sum (tc.jan) jan,sum (tc.feb) feb,sum (tc.mar) mar,year,roletype from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
 where tc.entrytype='RW' and tc.year='" + ddlyear.SelectedItem.Text + "' and roletype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + " ";

            sql += @"and tc.smid not in (select smid from tbl_crmtarget where year='" + ddlyear.SelectedItem.Text + "' and entrytype='PW' and roletype='" + ddlroleforsp.SelectedItem.Value + "')group by year,roletype union select sum (tc.apr) apr,sum (tc.may) may,sum (tc.jun) jun,sum (tc.july) july,sum (tc.aug) aug,sum (tc.sep) sep,sum (tc.oct) oct,sum (tc.nov) nov,sum (tc.dec) dec,sum (tc.jan) jan,sum (tc.feb) feb,sum (tc.mar) mar,year,roletype from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid where tc.entrytype='PW'  " + city1 + "  and tc.year='" + ddlyear.SelectedItem.Text + "' and roletype='" + ddlroleforsp.SelectedItem.Value + "' group by year,roletype) t1  group by year,roletype ) t6 group by apr,may, jun ,jul, aug,sep,oct,nov,dec,jan,feb,mar,year, roletype , Entry,Total";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                gridquarterly.DataSource = dt;
                gridquarterly.DataBind();

            }
            else
            {
                gridquarterly.DataSource = null;
                gridquarterly.DataBind();
            }

        }
        private void HalfyearlyRW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');

            string sql = @"select sum(isnull(apr,'0.00') +isnull(may,'0.00') + isnull(jun,'0.00') +isnull(jul,'0.00')+  isnull(aug,'0.00') +isnull(sep,'0.00') ) Q1, sum(isnull(oct,'0.00') +isnull(nov,'0.00') + isnull(dec,'0.00') +isnull(jan,'0.00')+  isnull(feb,'0.00') +isnull(mar,'0.00') ) Q2, year, roletype , Entry,Total 
 from ( select (select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=04 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as apr,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=05 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as may,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=06 and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jun,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=07 and year=" + year[0] + "  and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jul,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=08  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as aug,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=09  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as sep ,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=10  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as oct,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=11  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as nov,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=12  and year=" + year[0] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as dec,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=01  and year=" + year[1] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jan,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=02 and  year=" + year[1] + " and salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as feb,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=03 and   year=" + year[1] + " and  salesreptype='" + ddlroleforsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as mar ,'" + ddlyear.SelectedItem.Text + "' as year,'" + ddlroleforsp.SelectedItem.Value + "' as roletype ,'Target Accomplished' Entry,'Expected Total' as Total";

            sql += @"   union    
	
  select  sum (t1.apr) apr,sum (t1.may) may,sum (t1.jun) jun,sum (t1.july) july,sum (t1.aug) aug,sum (t1.sep) sep,sum (t1.oct) oct,sum (t1.nov) nov,sum (t1.dec) dec,sum (t1.jan) jan,sum (t1.feb) feb,sum (t1.mar) mar,year,roletype,'Target' Entry,'Target' as Total from
   (
  select sum (tc.apr) apr,sum (tc.may) may,sum (tc.jun) jun,sum (tc.july) july,sum (tc.aug) aug,sum (tc.sep) sep,sum (tc.oct) oct,sum (tc.nov) nov,sum (tc.dec) dec,sum (tc.jan) jan,sum (tc.feb) feb,sum (tc.mar) mar,year,roletype from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
 where tc.entrytype='RW' and tc.year='" + ddlyear.SelectedItem.Text + "' and roletype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + " ";

            sql += @"and tc.smid not in (select smid from tbl_crmtarget where year='" + ddlyear.SelectedItem.Text + "' and entrytype='PW' and roletype='" + ddlroleforsp.SelectedItem.Value + "')group by year,roletype union select sum (tc.apr) apr,sum (tc.may) may,sum (tc.jun) jun,sum (tc.july) july,sum (tc.aug) aug,sum (tc.sep) sep,sum (tc.oct) oct,sum (tc.nov) nov,sum (tc.dec) dec,sum (tc.jan) jan,sum (tc.feb) feb,sum (tc.mar) mar,year,roletype from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid where tc.entrytype='PW'  " + city1 + "  and tc.year='" + ddlyear.SelectedItem.Text + "' and roletype='" + ddlroleforsp.SelectedItem.Value + "' group by year,roletype) t1  group by year,roletype ) t6 group by apr,may, jun ,jul, aug,sep,oct,nov,dec,jan,feb,mar,year, roletype , Entry,Total";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                gridhalfyearly.DataSource = dt;
                gridhalfyearly.DataBind();

            }
            else
            {
                gridhalfyearly.DataSource = null;
                gridhalfyearly.DataBind();
            }
        }

        private void MonthlyPW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');

            string sql = @"select *
 from ( select (select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=04 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as apr,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=05 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as may,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=06 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jun,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=07 and year=" + year[0] + "  and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jul,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=08  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as aug,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=09  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as sep ,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=10  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as oct,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=11  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as nov,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=12  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as dec,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=01  and year=" + year[1] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jan,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=02 and  year=" + year[1] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as feb,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=03 and   year=" + year[1] + " and  manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as mar ,'" + ddlyear.SelectedItem.Text + "' as year,'" + ddlroleforsp.SelectedItem.Value + "' as roletype ,'Target Accomplished' Entry,'Expected Total' as Total   union ";

            string sql1 = @"      select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='PW' and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "' ";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql1);
            if (dt1.Rows.Count > 0)
            {
                sql += @"      select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='PW'  " + city1 + "  and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "' ";
            }
            else
            {
                sql += @"      select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='RW'  " + city1 + "  and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "' ";
            }
            sql += ") t1  group by apr,may, jun ,jul, aug,sep,oct,nov,dec,jan,feb,mar,year, roletype , Entry,Total";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                GridView4.DataSource = dt;
                GridView4.DataBind();

            }
            else
            {
                GridView4.DataSource = null;
                GridView4.DataBind();
            }

        }
        private void quarterlyPW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');

            string sql = @"select sum(isnull(apr,'0.00') +isnull(may,'0.00') + isnull(jun,'0.00') ) Q1 ,sum( isnull(jul,'0.00')+  isnull(aug,'0.00') +isnull(sep,'0.00') ) Q2, sum (isnull(oct,'0.00') + isnull(nov,'0.00') + isnull(dec,'0.00') ) Q3, sum (isnull(jan,'0.00') + isnull(feb,'0.00') + isnull(mar,'0.00') ) Q4, year, roletype , Entry,Total 
 from ( select (select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=04 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as apr,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=05 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as may,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=06 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jun,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=07 and year=" + year[0] + "  and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jul,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=08  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as aug,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=09  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as sep ,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=10  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as oct,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=11  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as nov,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=12  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as dec,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=01  and year=" + year[1] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jan,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=02 and  year=" + year[1] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as feb,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=03 and   year=" + year[1] + " and  manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as mar ,'" + ddlyear.SelectedItem.Text + "' as year,'" + ddlroleforsp.SelectedItem.Value + "' as roletype ,'Target Accomplished' Entry,'Expected Total' as Total  union    ";

            string sql1 = @"  select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='PW' and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "'";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql1);
            if (dt1.Rows.Count > 0)
            {
                sql += @"      select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='PW'  " + city1 + "  and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "' ";
            }
            else
            {
                sql += @"  select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='RW'  " + city1 + "  and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "'";
            }
            sql += ") t1  group by apr,may, jun ,jul, aug,sep,oct,nov,dec,jan,feb,mar,year, roletype , Entry,Total";



            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                gridquarterly.DataSource = dt;
                gridquarterly.DataBind();

            }
            else
            {
                gridquarterly.DataSource = null;
                gridquarterly.DataBind();

            }

        }
        private void HalfyearlyPW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');

            string sql = @"select sum(isnull(apr,'0.00') +isnull(may,'0.00') + isnull(jun,'0.00') +isnull(jul,'0.00')+  isnull(aug,'0.00') +isnull(sep,'0.00') ) Q1, sum(isnull(oct,'0.00') +isnull(nov,'0.00') + isnull(dec,'0.00') +isnull(jan,'0.00')+  isnull(feb,'0.00') +isnull(mar,'0.00') ) Q2, year, roletype , Entry,Total 
 from ( select (select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=04 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as apr,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=05 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as may,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=06 and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jun,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=07 and year=" + year[0] + "  and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jul,";


            sql += @"(select sum (amount) amount from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=08  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as aug,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=09  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as sep ,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=10  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as oct,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=11  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as nov,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=12  and year=" + year[0] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as dec,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=01  and year=" + year[1] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype) as jan,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=02 and  year=" + year[1] + " and manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as feb,";


            sql += @"(select sum (amount) amount  from 
	(select sum (amount) as amount,manager,mon,year,msp.salesreptype from 
	(
	  select sum (amount) as amount,crm_taskdocid,dealdate,ct.contact_id,cmc.manager ,month(dealdate) mon, year(dealdate) year from       [dbo].[CRM_TaskDeals] ctd left join   crm_task ct on ct.docid=ctd.crm_taskdocid left join [CRM_mastcontact] cmc on   cmc.contact_id=ct.contact_id group by crm_taskdocid,dealdate,ct.contact_id ,cmc.manager
    ) t left join mastsalesrep msp on msp.smid=t.manager   " + city + "   group by manager ,mon,year,msp.salesreptype ) t2 where mon=03 and   year=" + year[1] + " and  manager='" + ddlsp.SelectedItem.Value + "' group by mon,year,    salesreptype)  as mar ,'" + ddlyear.SelectedItem.Text + "' as year,'" + ddlroleforsp.SelectedItem.Value + "' as roletype ,'Target Accomplished' Entry,'Expected Total' as Total   union  ";


            string sql1 = @"    select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='PW' and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "'";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql1);
            if (dt1.Rows.Count > 0)
            {
                sql += @"    select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='PW'  " + city1 + "  and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "' ";
            }
            else
            {
                sql += @"    select tc.apr apr,tc.may may,tc.jun jun,tc.july jul,tc.aug aug,tc.sep sep,tc.oct oct,tc.nov nov,tc.dec dec,tc.jan jan,tc.feb feb,tc.mar mar,year,roletype,'Target ' Entry,'Target' as Total from tbl_crmtarget tc left join mastsalesrep msp on tc.smid=msp.smid 
              where tc.entrytype='RW' " + city1 + " and tc.year='" + ddlyear.SelectedItem.Text + "' and tc.smid='" + ddlsp.SelectedItem.Value + "'";
            }
            sql += ") t1  group by apr,may, jun ,jul, aug,sep,oct,nov,dec,jan,feb,mar,year, roletype , Entry,Total";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                gridhalfyearly.DataSource = dt;
                gridhalfyearly.DataBind();

            }
            else
            {
                gridhalfyearly.DataSource = null;
                gridhalfyearly.DataBind();
            }

        }

        private void gridvisible(int no)
        {
            if (no == 1)
            {
                GridView4.Visible = true; gridstatuswise.Visible = true;
                gridhalfyearly.Visible = false; gridstatusqarterly.Visible = false;
                gridquarterly.Visible = false; gridstatuswisehalfyearly.Visible = false;
            }
            if (no == 2)
            {
                GridView4.Visible = false; gridstatuswise.Visible = false;
                gridhalfyearly.Visible = false; gridstatusqarterly.Visible = true;
                gridquarterly.Visible = true; gridstatuswisehalfyearly.Visible = false;

            }
            if (no == 3)
            {
                GridView4.Visible = false; gridstatuswise.Visible = false;
                gridhalfyearly.Visible = true; gridstatusqarterly.Visible = false;
                gridquarterly.Visible = false; gridstatuswisehalfyearly.Visible =true ;

            }
        }

        private void clear()
        {
            ddlyear.SelectedValue = "0";
            ddlroleforsp.SelectedValue = "0";
            ddlstate_SelectedIndexChanged(null, null);
            ddlroleforsp_SelectedIndexChanged(null, null);
            ddlperiod.SelectedValue = "0";
            GridView4.Visible = false;
            gridhalfyearly.Visible = false;
            gridquarterly.Visible = false;
        }
        private void statuswisemonthlyRW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');
            string qry = @"select status ,isnull(apr,0.00) apr,isnull(may,0.00) may,isnull(jun,0.00) jun,isnull(jul,0.00) jul,isnull(aug,0.00) aug,isnull(sep,0.00) sep,isnull(oct,0.00) oct,isnull(nov,0.00) nov,isnull(dec,0.00) dec,isnull(jan,0.00) jan,isnull(feb,0.00) feb,isnull(mar,0.00) mar

from (


 select 'COLD' status, (select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') may, ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jun , ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jul , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='COLD' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') mar  union ";


            qry += @" select 'WARM' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='WARM') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM') mar  union ";


            qry += @" select 'HOT' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) dec ,";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + "  and cms.status='HOT') jan , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT' ) feb , ";


            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT') mar )  t";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {

                gridstatuswise.DataSource = dt;
                gridstatuswise.DataBind();
                //Calculate Sum and display in Footer Row

                decimal totalapr = dt.AsEnumerable().Sum(row => row.Field<decimal>("apr"));
                decimal totalmay = dt.AsEnumerable().Sum(row => row.Field<decimal>("may"));
                decimal totaljun = dt.AsEnumerable().Sum(row => row.Field<decimal>("jun"));
                decimal totaljul = dt.AsEnumerable().Sum(row => row.Field<decimal>("jul"));
                decimal totalaug = dt.AsEnumerable().Sum(row => row.Field<decimal>("aug"));
                decimal totalsep = dt.AsEnumerable().Sum(row => row.Field<decimal>("sep"));
                decimal totaloct = dt.AsEnumerable().Sum(row => row.Field<decimal>("oct"));
                decimal totalnov = dt.AsEnumerable().Sum(row => row.Field<decimal>("nov"));
                decimal totaldec = dt.AsEnumerable().Sum(row => row.Field<decimal>("dec"));
                decimal totaljan = dt.AsEnumerable().Sum(row => row.Field<decimal>("jan"));
                decimal totalfeb = dt.AsEnumerable().Sum(row => row.Field<decimal>("feb"));
                decimal totalmar = dt.AsEnumerable().Sum(row => row.Field<decimal>("mar"));
                gridstatuswise.FooterRow.Cells[0].Text = "Total";
                gridstatuswise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                gridstatuswise.FooterRow.Cells[1].Text = totalapr.ToString("N2");
                gridstatuswise.FooterRow.Cells[2].Text = totalmay.ToString("N2");
                gridstatuswise.FooterRow.Cells[3].Text = totaljun.ToString("N2");
                gridstatuswise.FooterRow.Cells[4].Text = totaljul.ToString("N2");
                gridstatuswise.FooterRow.Cells[5].Text = totalaug.ToString("N2");
                gridstatuswise.FooterRow.Cells[6].Text = totalsep.ToString("N2");
                gridstatuswise.FooterRow.Cells[7].Text = totaloct.ToString("N2");
                gridstatuswise.FooterRow.Cells[8].Text = totalnov.ToString("N2");
                gridstatuswise.FooterRow.Cells[9].Text = totaldec.ToString("N2");
                gridstatuswise.FooterRow.Cells[10].Text = totaljan.ToString("N2");
                gridstatuswise.FooterRow.Cells[11].Text = totalfeb.ToString("N2");
                gridstatuswise.FooterRow.Cells[12].Text = totalmar.ToString("N2");
            }
        }
        private void statuswiseQuarterlyRW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');
            string qry = @"select status ,sum(isnull(apr,0.00) +isnull(may,0.00)+isnull(jun,0.00)) Q1,sum(isnull(jul,0.00)+isnull(aug,0.00) +isnull(sep,0.00)) Q2,sum(isnull(oct,0.00)+isnull(nov,0.00) +isnull(dec,0.00)) Q3,sum(isnull(jan,0.00) +isnull(feb,0.00) +isnull(mar,0.00)) Q4 

from (


 select 'COLD' status, (select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') may, ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jun , ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jul , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='COLD' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') mar  union ";


            qry += @" select 'WARM' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='WARM') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM') mar  union ";


            qry += @" select 'HOT' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) dec ,";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + "  and cms.status='HOT') jan , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT' ) feb , ";


            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT') mar )  t  GROUP BY status";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {

                gridstatusqarterly.DataSource = dt;
                gridstatusqarterly.DataBind();
                //Calculate Sum and display in Footer Row

                decimal Q1 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q1"));
                decimal Q2 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q2"));
                decimal Q3 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q3"));
                decimal Q4 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q4"));

                gridstatusqarterly.FooterRow.Cells[0].Text = "Total";
                gridstatusqarterly.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                gridstatusqarterly.FooterRow.Cells[1].Text = Q1.ToString("N2");
                gridstatusqarterly.FooterRow.Cells[2].Text = Q2.ToString("N2");
                gridstatusqarterly.FooterRow.Cells[3].Text = Q3.ToString("N2");
                gridstatusqarterly.FooterRow.Cells[4].Text = Q4.ToString("N2");

            }
        }
        private void statuswisehalfyearlyRW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');
            string qry = @"select status ,sum(isnull(apr,0.00) +isnull(may,0.00)+isnull(jun,0.00)) +sum(isnull(jul,0.00)+isnull(aug,0.00) +isnull(sep,0.00)) Q1,sum(isnull(oct,0.00)+isnull(nov,0.00) +isnull(dec,0.00)) +sum(isnull(jan,0.00) +isnull(feb,0.00) +isnull(mar,0.00)) Q2 

from (


 select 'COLD' status, (select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') may, ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jun , ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jul , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='COLD' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') mar  union ";


            qry += @" select 'WARM' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='WARM') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM') mar  union ";


            qry += @" select 'HOT' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) dec ,";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'  " + city1 + "   and year(dealdate)=" + year[1] + "  and cms.status='HOT') jan , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT' ) feb , ";


            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and salesreptype='" + ddlroleforsp.SelectedItem.Value + "'   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT') mar )  t GROUP BY status";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {

                gridstatuswisehalfyearly.DataSource = dt;
                gridstatuswisehalfyearly.DataBind();
                //Calculate Sum and display in Footer Row

                decimal Q1 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q1"));
                decimal Q2 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q2"));

                gridstatuswisehalfyearly.FooterRow.Cells[0].Text = "Total";
                gridstatuswisehalfyearly.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                gridstatuswisehalfyearly.FooterRow.Cells[1].Text = Q1.ToString("N2");
                gridstatuswisehalfyearly.FooterRow.Cells[2].Text = Q2.ToString("N2");
                
            }
            else
            {
                gridstatuswisehalfyearly.DataSource = null;
                gridstatuswisehalfyearly.DataBind();
            }
        }

        private void statuswiseQuarterlyPW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');
            string qry = @"select status ,sum(isnull(apr,0.00) +isnull(may,0.00)+isnull(jun,0.00)) Q1,sum(isnull(jul,0.00)+isnull(aug,0.00) +isnull(sep,0.00)) Q2,sum(isnull(oct,0.00)+isnull(nov,0.00) +isnull(dec,0.00)) Q3,sum(isnull(jan,0.00) +isnull(feb,0.00) +isnull(mar,0.00)) Q4 

from (select 'COLD' status, (select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') may, ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jun , ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jul , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='COLD' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') mar  union ";


            qry += @" select 'WARM' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='WARM') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM') mar  union ";


            qry += @" select 'HOT' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) dec ,";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[1] + "  and cms.status='HOT') jan , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT' ) feb , ";


            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT') mar ) t  GROUP BY status";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {

                gridstatusqarterly.DataSource = dt;
                gridstatusqarterly.DataBind();
                decimal Q1 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q1"));
                decimal Q2 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q2"));
                decimal Q3 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q3"));
                decimal Q4 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q4"));

                gridstatusqarterly.FooterRow.Cells[0].Text = "Total";
                gridstatusqarterly.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                gridstatusqarterly.FooterRow.Cells[1].Text = Q1.ToString("N2");
                gridstatusqarterly.FooterRow.Cells[2].Text = Q2.ToString("N2");
                gridstatusqarterly.FooterRow.Cells[3].Text = Q3.ToString("N2");
                gridstatusqarterly.FooterRow.Cells[4].Text = Q4.ToString("N2");

            }
            else
            {
                gridstatusqarterly.DataSource = null;
                gridstatusqarterly.DataBind();
            }
        }
        private void statuswiseHalfyearlyPW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');
            string qry = @"select status ,sum(isnull(apr,0.00) +isnull(may,0.00)+isnull(jun,0.00)) +sum(isnull(jul,0.00)+isnull(aug,0.00) +isnull(sep,0.00)) Q1,sum(isnull(oct,0.00)+isnull(nov,0.00) +isnull(dec,0.00)) +sum(isnull(jan,0.00) +isnull(feb,0.00) +isnull(mar,0.00)) Q2 

from (select 'COLD' status, (select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') may, ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jun , ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jul , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='COLD' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') mar  union ";


            qry += @" select 'WARM' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='WARM') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM') mar  union ";


            qry += @" select 'HOT' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) dec ,";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[1] + "  and cms.status='HOT') jan , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT' ) feb , ";


            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT') mar ) t  GROUP BY status";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {

                gridstatuswisehalfyearly.DataSource = dt;
                gridstatuswisehalfyearly.DataBind();
                decimal Q1 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q1"));
                decimal Q2 = dt.AsEnumerable().Sum(row => row.Field<decimal>("Q2"));

                gridstatuswisehalfyearly.FooterRow.Cells[0].Text = "Total";
                gridstatuswisehalfyearly.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                gridstatuswisehalfyearly.FooterRow.Cells[1].Text = Q1.ToString("N2");
                gridstatuswisehalfyearly.FooterRow.Cells[2].Text = Q2.ToString("N2");

            }
            else
            {
                gridstatuswisehalfyearly.DataSource = null;
                gridstatuswisehalfyearly.DataBind();
            }
        }

        private void statuswisemonthlyPW(string city, string city1)
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');
            string qry = @"select status ,isnull(apr,0.00) apr,isnull(may,0.00) may,isnull(jun,0.00) jun,isnull(jul,0.00) jul,isnull(aug,0.00) aug,isnull(sep,0.00) sep,isnull(oct,0.00) oct,isnull(nov,0.00) nov,isnull(dec,0.00) dec,isnull(jan,0.00) jan,isnull(feb,0.00) feb,isnull(mar,0.00) mar

from (select 'COLD' status, (select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD') may, ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "   and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jun , ";


            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "and cms.status='COLD' ) jul , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='COLD' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + "  and cms.status='COLD' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='COLD' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='COLD') mar  union ";


            qry += @" select 'WARM' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='WARM') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='WARM' ) dec, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + " and cms.status='WARM') jan, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM' ) feb, ";

            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[1] + " and cms.status='WARM') mar  union ";


            qry += @" select 'HOT' status,(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=04 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') apr, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=05 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT') may, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=06 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jun, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=07 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) jul, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=08 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) aug, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=09 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT' ) sep, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=10 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) oct, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=11 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[0] + " and cms.status='HOT') nov, ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=12 and cmc.manager=" + ddlsp.SelectedItem.Value + " " + city1 + "   and year(dealdate)=" + year[0] + " and cms.status='HOT' ) dec ,";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=01 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "   and year(dealdate)=" + year[1] + "  and cms.status='HOT') jan , ";

            qry += @"(select sum (ctd.amount)   from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=02 and cmc.manager=" + ddlsp.SelectedItem.Value + "   " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT' ) feb , ";


            qry += @"(select sum (ctd.amount)  from crm_taskdeals ctd left join crm_task ct on ct.docid=ctd.crm_taskdocid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join crm_maststatus cms on cms.status_id=cmc.status_id
left join mastsalesrep msp on msp.smid=cmc.manager  where month(dealdate)=03 and cmc.manager=" + ddlsp.SelectedItem.Value + "  " + city1 + "  and year(dealdate)=" + year[1] + "  and cms.status='HOT') mar ) t";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {

                gridstatuswise.DataSource = dt;
                gridstatuswise.DataBind();
                decimal totalapr = dt.AsEnumerable().Sum(row => row.Field<decimal>("apr"));
                decimal totalmay = dt.AsEnumerable().Sum(row => row.Field<decimal>("may"));
                decimal totaljun = dt.AsEnumerable().Sum(row => row.Field<decimal>("jun"));
                decimal totaljul = dt.AsEnumerable().Sum(row => row.Field<decimal>("jul"));
                decimal totalaug = dt.AsEnumerable().Sum(row => row.Field<decimal>("aug"));
                decimal totalsep = dt.AsEnumerable().Sum(row => row.Field<decimal>("sep"));
                decimal totaloct = dt.AsEnumerable().Sum(row => row.Field<decimal>("oct"));
                decimal totalnov = dt.AsEnumerable().Sum(row => row.Field<decimal>("nov"));
                decimal totaldec = dt.AsEnumerable().Sum(row => row.Field<decimal>("dec"));
                decimal totaljan = dt.AsEnumerable().Sum(row => row.Field<decimal>("jan"));
                decimal totalfeb = dt.AsEnumerable().Sum(row => row.Field<decimal>("feb"));
                decimal totalmar = dt.AsEnumerable().Sum(row => row.Field<decimal>("mar"));
                gridstatuswise.FooterRow.Cells[0].Text = "Total";
               // e.Row.Cells[i].Style.Add("text-align", "right"); ;
               // gridstatuswise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                gridstatuswise.FooterRow.Cells[1].Text = totalapr.ToString("N2");
                gridstatuswise.FooterRow.Cells[2].Text = totalmay.ToString("N2");
                gridstatuswise.FooterRow.Cells[3].Text = totaljun.ToString("N2");
                gridstatuswise.FooterRow.Cells[4].Text = totaljul.ToString("N2");
                gridstatuswise.FooterRow.Cells[5].Text = totalaug.ToString("N2");
                gridstatuswise.FooterRow.Cells[6].Text = totalsep.ToString("N2");
                gridstatuswise.FooterRow.Cells[7].Text = totaloct.ToString("N2");
                gridstatuswise.FooterRow.Cells[8].Text = totalnov.ToString("N2");
                gridstatuswise.FooterRow.Cells[9].Text = totaldec.ToString("N2");
                gridstatuswise.FooterRow.Cells[10].Text = totaljan.ToString("N2");
                gridstatuswise.FooterRow.Cells[11].Text = totalfeb.ToString("N2");
                gridstatuswise.FooterRow.Cells[12].Text = totalmar.ToString("N2");


                //gridstatuswise.FooterRow.Cells[1].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[2].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[3].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[4].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[5].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[6].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[7].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[8].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[9].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[10].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[11].Style.Add("text-align", "right");
                //gridstatuswise.FooterRow.Cells[12].Style.Add("text-align", "right"); 
            }
        }
        private DataTable DealReport()
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-'); string filter = null;
            if(ddlroleforsp.SelectedValue!="0")
            {
                if (ddlsp.SelectedValue != "0")
                {
                    filter = "and msp.salesreptype='" + ddlroleforsp.SelectedItem.Value + "' and cmc.manager='" + ddlsp.SelectedItem.Value + "' ";
                }
                else
                {
                    filter = "and msp.salesreptype='" + ddlroleforsp.SelectedItem.Value + "'";
                }
                
            }
            if (ddlstate.SelectedValue != "0")
            {
                if (ddlcity.SelectedValue != "0")
                {
                    filter += "and msp.cityid=" + ddlcity.SelectedItem.Value + "  ";
                }
                else
                {
                    filter += "and msp.cityid in (select cityid from viewgeo where statid="+ddlstate.SelectedItem.Value+")";
                }

            }
          
            string sql = @"select msp.smname as Owner,ctd.dealname DealName,ctd.amount Amount,cmc.firstname+' '+cmc.firstname as Contact ,
CONVERT(VARCHAR(12), ctd.DealDate, 107) AS Date,CONVERT(VARCHAR(12), ctd.expclsdt, 107) AS ClosedDate,cms.status
,msp.salesreptype from crm_taskdeals ctd left join crm_task ct on ctd.crm_taskdocid=ct.docid left join crm_mastcontact cmc
on cmc.contact_id=ct.contact_id left join mastsalesrep msp on msp.smid=cmc.manager left join crm_maststatus cms 
on cms.status_Id=cmc.status_id where ctd.DealDate between '" + year[0]+"-03-01' AND '" + year[1]+"-03-31'  "+filter+" ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {

                GridDealReport.DataSource = dt;
                GridDealReport.DataBind();
               // btnexcel.Visible = true;
            }
            else
            {
                GridDealReport.DataSource = null;
                GridDealReport.DataBind();
                btnexcel.Visible =false;
            }
            return dt;
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void Excel()
        {
            string[] year = ddlyear.SelectedItem.Text.Split('-');
            DataTable dt = DealReport();

            string csv = string.Empty;

            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + ',';
            }

            //Add new line.
            csv += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            //Download the CSV file.
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PendingDeals"+ddlyear.SelectedItem.Text+".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(csv);
            Response.Flush();
            Response.End();
        }

        protected void btnexcel_Click(object sender, EventArgs e)
        {
            //Excel();
        }

        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                for (int i = 1; i <= 12; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right");;
                }
                   
        }

        protected void gridquarterly_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
                for (int i = 1; i <= 5; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }
          
        }

        protected void gridhalfyearly_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                for (int i = 1; i <= 3; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }

        }

        protected void gridstatuswise_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                for (int i = 1; i <= 12; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 1; i <= 12; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }
            }
        }

        protected void gridstatusqarterly_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                for (int i = 1; i <= 5; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }
            if (e.Row.RowType == DataControlRowType.Footer)
                for (int i = 1; i <= 5; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }
        }

        protected void gridstatuswisehalfyearly_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                for (int i = 1; i <= 3; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }
            if (e.Row.RowType == DataControlRowType.Footer)
                for (int i = 1; i <= 3; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right"); ;
                }
        }

        protected void GridDealReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
               // for (int i = 1; i <= 3; i++)
                {
                    e.Row.Cells[2].Style.Add("text-align", "right"); ;
                }
        }
    }
}