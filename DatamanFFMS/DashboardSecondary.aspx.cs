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
using System.Text;

namespace AstralFFMS
{
    public partial class DashboardSecondary : System.Web.UI.Page
    {
        string roleType = "";      
        string sql = "";       
        string name = "";
        string Date = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);
            if (!IsPostBack)
            {
                FromDate.Text = Request.QueryString["Date"];
                fillUser();
             // this.BindWithSalesPersonDDl(Convert.ToInt32(Settings.Instance.SMID));
                this.fillAllRecord();
            }
        }
        private void BindWithSalesPersonDDl(int smID)
        {
            string query = "select * from MastSalesRep where smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ") and SMName<>'.'  and smid<>" + smID + " order by SMName";
            DataTable envobj1 = new DataTable();
            envobj1 = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (envobj1.Rows.Count > 0)
            {
                lstUndeUser.DataSource = envobj1;
                lstUndeUser.DataTextField = "SMName";
                lstUndeUser.DataValueField = "SMId";
                lstUndeUser.DataBind();
            }
            lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private void fillUser()
        {
            DataTable dt = null;
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            Date = FromDate.Text;

            if (roleType.Equals("Admin"))
            {
                if (name.Equals("TotalOrder"))
                {
                   
                    sql = "select * from (select distinct ttv.SMId,(smname), count (distinct(ttv.orddocid)) as TotalParty,count (distinct(ttv.orddocid)) as TotalOrder from transorder  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.orddocid)) as TotalParty,count (distinct(ttv.orddocid)) as TotalOrder  from temp_transorder ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname  ";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
                else if (name.Equals("Demo"))
                {
                   
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalDemo from transdemo  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalDemo  from temp_transdemo ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    
                }
                else if (name.Equals("FailedVisit"))
                {

                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalFailedVisit from temp_TransFailedVisit  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=0 group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalFailedVisit  from TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=0 group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    
                }
                else if (name.Equals("Competitor"))
                {
                    
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCompetitor from temp_transcompetitor  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCompetitor  from transcompetitor ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    
                }
                else if (name.Equals("Collection"))
                {
                   
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCollection from temp_transcollection  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCollection  from transcollection ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                   
                }
            }
            else
            {
                if (name.Equals("TotalOrder"))
                {
                   
                    sql = "select * from (select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalOrder from transorder  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalOrder  from temp_transorder ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and  vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                   
                }
                else if (name.Equals("Demo"))
                {
                  
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalDemo from transdemo  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalDemo  from temp_transdemo ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    
                }
                else if (name.Equals("FailedVisit"))
                {
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalFailedVisit from temp_TransFailedVisit  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalFailedVisit  from TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                   
                }
                else if (name.Equals("Competitor"))
                {
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCompetitor from temp_transcompetitor  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCompetitor  from transcompetitor ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    
                }
                else if (name.Equals("Collection"))
                {
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCollection from temp_transcollection  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCollection from transcollection  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                   
                }
            }

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    lstUndeUser.DataSource = dt;
                    lstUndeUser.DataTextField = "smname";
                    lstUndeUser.DataValueField = "SMId";
                    lstUndeUser.DataBind();
                }
                else
                {
                    lstUndeUser.Items.Clear();
                    lstUndeUser.DataBind();
                }
            }
            lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        public void fillAllRecord()
        {
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            Date = FromDate.Text;
            string _search = "";
            if (lstUndeUser.SelectedValue!="0") _search = "And ttv.SMId=" + lstUndeUser.SelectedValue + "";
            if (roleType.Equals("Admin"))
            {
                if (name.Equals("TotalOrder")) 
                {
                    lblHeading.InnerText = "DateWise Orders Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname), count (distinct(ttv.orddocid)) as TotalParty,count (distinct(ttv.orddocid)) as TotalOrder from transorder  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') " + _search + "  group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.orddocid)) as TotalParty,count (distinct(ttv.orddocid)) as TotalOrder  from temp_transorder ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname  ";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (lstUndeUser.SelectedValue != "0")
                //        {
                //            var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                //            dt = result.CopyToDataTable();
                //        }
                //    }
                //}
                rptCollection.DataSource = dt;
                rptCollection.DataBind();
                rptDemo.Visible = false;
                rptFaildvisit.Visible = false;
                rptCompetitor.Visible = false;
                //rptCollection.Visible = false;
                }
                else if (name.Equals("Demo")){
                    lblHeading.InnerText = "DateWise Demos Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalDemo from transdemo  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalDemo  from temp_transdemo ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptDemo.DataSource = dt;
                    rptDemo.DataBind();
                    rptCollection.Visible = false;
                    rptFaildvisit.Visible = false;
                    rptCompetitor.Visible = false;
                    rptTotalOrder.Visible = false;
                }
                else if (name.Equals("Non-Productive"))
                {
                    lblHeading.InnerText = "DateWise Non-Productive Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalFailedVisit from temp_TransFailedVisit  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and  partydist=0 group by smname,ttv.SMId union select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalFailedVisit  from TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=0 group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptFaildvisit.DataSource = dt;
                    rptFaildvisit.DataBind();
                    rptCollection.Visible = false;
                    rptCompetitor.Visible = false;
                    rptTotalOrder.Visible = false;
                    rptDemo.Visible = false;
                }
                else if (name.Equals("Competitor"))
                {
                    lblHeading.InnerText = "DateWise Competitors Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCompetitor from temp_transcompetitor  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCompetitor  from transcompetitor ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptCompetitor.DataSource = dt;
                    rptCompetitor.DataBind();
                    rptCollection.Visible = false;
                    rptFaildvisit.Visible = false;
                    rptTotalOrder.Visible = false;
                    rptDemo.Visible = false;
                }
                else if (name.Equals("Collection"))
                {
                    lblHeading.InnerText = "DateWise Collections Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCollection from temp_transcollection  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCollection  from transcollection ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptCollection.DataSource = dt;
                    rptCollection.DataBind();
                    rptFaildvisit.Visible = false;
                    rptCompetitor.Visible = false;
                    rptTotalOrder.Visible = false;
                    rptDemo.Visible = false;
                }
            }
            else
            {
                if (name.Equals("TotalOrder"))
                {
                    lblHeading.InnerText = "DateWise Orders Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalOrder from transorder  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and  vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalOrder  from temp_transorder ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and  vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptTotalOrder.DataSource = dt;
                    rptTotalOrder.DataBind();
                    rptDemo.Visible = false;
                    rptFaildvisit.Visible = false;
                    rptCompetitor.Visible = false;
                    rptCollection.Visible = false;
                }
                else if (name.Equals("Demo"))
                {
                    lblHeading.InnerText = "DateWise Demos Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalDemo from transdemo  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalDemo  from temp_transdemo ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptDemo.DataSource = dt;
                    rptDemo.DataBind();
                    rptCollection.Visible = false;
                    rptFaildvisit.Visible = false;
                    rptCompetitor.Visible = false;
                    rptTotalOrder.Visible = false;
                }
                else if (name.Equals("Non-Productive"))
                {
                    lblHeading.InnerText = "DateWise Non-Productive Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalFailedVisit from temp_TransFailedVisit  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=0 group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalFailedVisit  from TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=0 group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptFaildvisit.DataSource = dt;
                    rptFaildvisit.DataBind();
                    rptCollection.Visible = false;
                    rptCompetitor.Visible = false;
                    rptTotalOrder.Visible = false;
                    rptDemo.Visible = false;
                }
                else if (name.Equals("Competitor"))
                {
                    lblHeading.InnerText = "DateWise Competitors Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCompetitor from temp_transcompetitor  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname) , count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCompetitor  from transcompetitor ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptCompetitor.DataSource = dt;
                    rptCompetitor.DataBind();
                    rptCollection.Visible = false;
                    rptFaildvisit.Visible = false;
                    rptTotalOrder.Visible = false;
                    rptDemo.Visible = false;
                }
                else if (name.Equals("Collection"))
                {
                    lblHeading.InnerText = "DateWise Collections Detail";
                    sql = "select * from (select distinct ttv.SMId,(smname),count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid))as TotalCollection from temp_transcollection  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId   union  select distinct ttv.SMId,(smname), count (distinct(ttv.partyid)) as TotalParty,count (distinct(ttv.partyid)) as TotalCollection from transcollection  ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where ttv.smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+")) and level>= (select distinct level  from MastSalesRepGrp where MainGrp in ("+Settings.Instance.SMID+"))) and vdate=replace(convert(NVARCHAR, DateAdd(minute,330,'"+Date+"'), 106), ' ', '/') group by smname,ttv.SMId ) a order by smname";
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (lstUndeUser.SelectedValue != "0")
                            {
                                var result = dt.Select("SMId=" + lstUndeUser.SelectedValue + "");
                                dt = result.CopyToDataTable();
                            }
                        }
                    }
                    rptCollection.DataSource = dt;
                    rptCollection.DataBind();
                    rptFaildvisit.Visible = false;
                    rptCompetitor.Visible = false;
                    rptTotalOrder.Visible = false;
                    rptDemo.Visible = false;
                }
            }
            
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DashboardSecondry.csv");
                string headertext = "Sale Person".TrimStart('"').TrimEnd('"') + "," + "Party Count".TrimStart('"').TrimEnd('"');

                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;

                DataTable dtParams = new DataTable();
                dtParams.Columns.Add(new DataColumn("smname", typeof(String)));
                dtParams.Columns.Add(new DataColumn("TotalParty", typeof(String)));
                //dtParams.Columns.Add(new DataColumn("TotalOrder", typeof(String)));


                foreach (RepeaterItem item in rptCollection.Items)
                {
                    DataRow dr = dtParams.NewRow();
                    Label lblsmname = item.FindControl("lblsmname") as Label;
                    dr["smname"] = lblsmname.Text.ToString();
                    Label lbltotalparty = item.FindControl("lbltotalparty") as Label;
                    dr["TotalParty"] = lbltotalparty.Text.ToString();
                    //Label lbltotalorder = item.FindControl("lbltotalorder") as Label;
                   // dr["TotalOrder"] = lbltotalorder.Text.ToString();
                    

                    dtParams.Rows.Add(dr);
                }

                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            string h = dtParams.Rows[j][k].ToString();
                            string d = h.Replace(",", " ");
                            dtParams.Rows[j][k] = "";
                            dtParams.Rows[j][k] = d;
                            dtParams.AcceptChanges();
                            if (k == 0)
                            {
                                // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k]).ToString() + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                //sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=DashboardSecondry.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void FromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                this.fillAllRecord();
                this.fillUser();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            }
            catch(Exception ex)
            { ex.ToString(); }
        }
        protected void lstUndeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();
           // this.fillUser();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }

    }
}