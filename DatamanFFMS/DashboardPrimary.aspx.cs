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
    public partial class DashboardPrimary : System.Web.UI.Page
    {
        string roleType = "", total = "";
        DataTable dtEmployee = null;
        string sql = "";
        string secondarySql = "";
        string PrimarySql = "";
        string UnApprovedSql = "";
        string name = "";
        string Date = "";
        string Partytype = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FromDate.Text = Request.QueryString["Date"];
                this.fillUser();
                this.fillAllRecord();

            }
        }


        private void fillUser()
        {
            DataTable dt = null;
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            Date = FromDate.Text;
            Partytype = Request.QueryString["PT"];
            if (roleType.Equals("Admin"))
            {
                if (name.Equals("Collection"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
                            count (distinct(ttv.distid)) as Totalcollection from  distributercollection ttv 
                            left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid
                            where vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and  Isnull(MP.PartyType,0)=0 group by smname,ttv.SMID order by smname,ttv.SMID  ";
                    }
                    else
                    {


                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
                                count (distinct(ttv.distid)) as Totalcollection from  distributercollection ttv 
                                left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid
                                LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                                where PT.PartyTypeName='INSTITUTIONAL' and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMID order by smname,ttv.SMID  ";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
                else if (name.Equals("Discussion"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
                         count (distinct(ttv.distid)) as TotalDiscussion from  temp_transvisitdist ttv 
                         left join mastparty mp on ttv.distid=mp.partyid 
                         left join  mastsalesrep msr on msr.smid=ttv.smid  
                         where vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and  Isnull(MP.PartyType,0)=0 AND ttv.Type IS null group by smname,ttv.smid  union  select  distinct ttv.smid, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and  Isnull(MP.PartyType,0)=0 AND ttv.Type IS null group by smname,ttv.SMID) a order by smname ";
                    }
                    else
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
                         count (distinct(ttv.distid)) as TotalDiscussion from  temp_transvisitdist ttv 
                         left join mastparty mp on ttv.distid=mp.partyid 
                         left join  mastsalesrep msr on msr.smid=ttv.smid  
                         LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType
                         where PT.PartyTypeName='INSTITUTIONAL' and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')  AND ttv.Type IS null group by smname,ttv.smid  union  select  distinct ttv.smid, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid        LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType  where PT.PartyTypeName='INSTITUTIONAL' and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') AND ttv.Type IS null group by smname,ttv.SMID) a order by smname ";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
                else if (name.Equals("Non-Productive"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.partyid)) as TotalDistributors
                             ,count (distinct(ttv.partyid)) as TotalFailedvisit from  temp_TransFailedVisit ttv 
                             left join mastparty mp on ttv.partyid=mp.partyid
                             left join  mastsalesrep msr on msr.smid=ttv.smid  
                             where vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 and  Isnull(MP.PartyType,0)=0  group by smname,ttv.SMID  union  select  distinct ttv.SMID,(smname) ,count (ttv.partyid) as TotalDistributors,count (ttv.partyid) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and  Isnull(MP.PartyType,0)=0  and partydist=1 group by smname,ttv.SMID) a order by smname ";
                    }
                    else
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.partyid)) as TotalDistributors
                             ,count (distinct(ttv.partyid)) as TotalFailedvisit from  temp_TransFailedVisit ttv 
                             left join mastparty mp on ttv.partyid=mp.partyid
                             left join  mastsalesrep msr on msr.smid=ttv.smid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType
                             where PT.PartyTypeName='INSTITUTIONAL' and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1   group by smname,ttv.SMID  union  select  distinct ttv.SMID,(smname) ,count (ttv.partyid) as TotalDistributors,count (ttv.partyid) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid     LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType  where PT.PartyTypeName='INSTITUTIONAL' and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 group by smname,ttv.SMID) a order by smname ";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
                else if (name.Equals("Productive"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.DistId)) as TotalDistributors,count (distinct(ttv.DistId)) as TotalProductive
                             from  TransPurchOrder ttv left join mastparty mp on ttv.DistId=mp.partyid 
                            left join  mastsalesrep msr on msr.smid=ttv.smid  where vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1 and  Isnull(MP.PartyType,0)=0   group by smname,ttv.SMID ) a order by smname ";
                    }
                    else
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.DistId)) as TotalDistributors,count (distinct(ttv.DistId)) as TotalProductive
                            from  TransPurchOrder ttv left join mastparty mp on ttv.DistId=mp.partyid 
                            left join  mastsalesrep msr on msr.smid=ttv.smid 
                            LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType
                            where vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1 and PT.PartyTypeName='INSTITUTIONAL'  group by smname,ttv.SMID ) a order by smname ";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
            }
            else
            {
                if (name.Equals("Collection"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors
                        ,count (distinct(ttv.distid)) as Totalcollection from  distributercollection ttv 
                        left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid
                        where   Isnull(MP.PartyType,0)=0 and ttv.smid in (  select MastSalesRep.smid from MastSalesRep 
                        left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid 
                        from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')  group by smname,ttv.SMID order by smname,ttv.SMID ";
                    }
                    else
                    {
                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors
                        ,count (distinct(ttv.distid)) as Totalcollection from  distributercollection ttv 
                        left join mastparty mp on ttv.distid=mp.partyid 
                        LEFT JOIN PartyType PT ON PT.PartytypeID=mp.PartyType
                        left join  mastsalesrep msr on msr.smid=ttv.smid
                        where  PT.PartyTypeName='INSTITUTIONAL' and ttv.smid in (  select MastSalesRep.smid from MastSalesRep 
                        left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid 
                        from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')  group by smname,ttv.SMID order by smname,ttv.SMID ";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
                else if (name.Equals("Discussion"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select  distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion
                                 from  temp_transvisitdist ttv left join mastparty mp on ttv.distid=mp.partyid 
                               left join  mastsalesrep msr on msr.smid=ttv.smid  where Isnull(MP.PartyType,0)=0 and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') AND ttv.Type IS null group by smname ,ttv.SMID union select  distinct ttv.SMID, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   where ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and  Isnull(MP.PartyType,0)=0 AND ttv.Type IS null group by smname,ttv.SMID) a order by smname,a.SMID";
                    }
                    else
                    {
                        sql = @"select * from (select  distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion
                    from  temp_transvisitdist ttv left join mastparty mp on ttv.distid=mp.partyid 
                    left join  mastsalesrep msr on msr.smid=ttv.smid  
                    LEFT JOIN PartyType PT ON PT.PartytypeID=mp.PartyType
                    where PT.PartyTypeName='INSTITUTIONAL' and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') AND ttv.Type IS null group by smname ,ttv.SMID union select  distinct ttv.SMID, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType     where PT.PartyTypeName='INSTITUTIONAL' and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') AND ttv.Type IS null group by smname,ttv.SMID) a order by smname,a.SMID";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
                else if (name.Equals("Non-Productive"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select  distinct ttv.SMID,(smname) ,count (distinct(ttv.partyid)) as TotalDistributors,
                        count (distinct(ttv.partyid)) as TotalFailedvisit from  temp_TransFailedVisit ttv
                        left join mastparty mp on ttv.partyid=mp.partyid 
                        left join  mastsalesrep msr on msr.smid=ttv.smid  
                        where  Isnull(MP.PartyType,0)=0 and  ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1  group by smname,ttv.SMID union select   distinct ttv.SMID, (smname) ,count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where  ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 ) and Isnull(MP.PartyType,0)=0  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 group by smname,ttv.SMID) a order by smname,a.SMID";
                    }
                    else
                    {


                        sql = @"select * from (select  distinct ttv.SMID,(smname) ,count (distinct(ttv.partyid)) as TotalDistributors,
                        count (distinct(ttv.partyid)) as TotalFailedvisit from  temp_TransFailedVisit ttv
                        left join mastparty mp on ttv.partyid=mp.partyid 
                        left join  mastsalesrep msr on msr.smid=ttv.smid  LEFT JOIN PartyType PT ON PT.PartytypeID=mp.PartyType
                        where PT.PartyTypeName='INSTITUTIONAL' and   ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1  group by smname,ttv.SMID union select   distinct ttv.SMID, (smname) ,count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where  ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId   LEFT JOIN PartyType PT ON PT.PartytypeID=mp.PartyType             where PT.PartyTypeName='INSTITUTIONAL' and smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 group by smname,ttv.SMID) a order by smname,a.SMID";
                    }
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                }
                else if (name.Equals("Productive"))
                {
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.DistId)) as TotalDistributors,count (distinct(ttv.DistId)) as TotalProductive from  TransPurchOrder ttv 
                             left join mastparty mp on ttv.DistId=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid
                            where  Isnull(MP.PartyType,0)=0 and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1  group by smname,ttv.SMID ) a order by smname ";

                    }
                    else
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.DistId)) as TotalDistributors,count (distinct(ttv.DistId)) as TotalProductive from  TransPurchOrder ttv 
                        left join mastparty mp on ttv.DistId=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid
                         LEFT JOIN PartyType PT ON PT.PartytypeID=mp.PartyType
                        where PT.PartyTypeName='INSTITUTIONAL' 
                        and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1  group by smname,ttv.SMID ) a order by smname ";

                    }

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
                lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            //lstUndeUser.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void lstUndeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.fillAllRecord();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        public void fillAllRecord()
        {
            roleType = Settings.Instance.RoleType;
            name = Request.QueryString["Name"];
            Date = FromDate.Text;
            Partytype = Request.QueryString["PT"];
            if (roleType.Equals("Admin"))
            {
                if (name.Equals("Collection"))
                {
                    lblHeading.InnerText = "DateWise Collections Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
                    count (distinct(ttv.distid)) as Totalcollection,'P' As Partytype1 from  distributercollection ttv 
                    left join mastparty mp on ttv.distid=mp.partyid 
                    left join  mastsalesrep msr on msr.smid=ttv.smid  
                    where vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and  Isnull(MP.PartyType,0)=0  group by smname,ttv.SMID order by smname,ttv.SMID  ";

                    }
                    else
                    {
                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
                    count (distinct(ttv.distid)) as Totalcollection,'I' As Partytype1 from  distributercollection ttv 
                    left join mastparty mp on ttv.distid=mp.partyid 
                    left join  mastsalesrep msr on msr.smid=ttv.smid  
                    LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and 
vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')   group by smname,ttv.SMID order by smname,ttv.SMID  ";

                    }
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

                    rptDiscussion.Visible = false;
                    rptFaildvisit.Visible = false;
                    rptProductive.Visible = false;
                }
                else if (name.Equals("Discussion"))
                {
                    lblHeading.InnerText = "DateWise Discussions Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select *,'P' As Partytype1 from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion
                          from  temp_transvisitdist ttv left join mastparty mp on ttv.distid=mp.partyid  
left join  mastsalesrep msr on msr.smid=ttv.smid 
where  Isnull(MP.PartyType,0)=0 and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') And ttv.Type IS null  group by smname,ttv.smid  union  select  distinct ttv.smid, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where  Isnull(MP.PartyType,0)=0 and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') And ttv.Type IS null  group by smname,ttv.SMID) a order by smname ";

                    }
                    else
                    {
                        sql = @"select *,'I' As Partytype1 from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion
                          from  temp_transvisitdist ttv left join mastparty mp on ttv.distid=mp.partyid  
left join  mastsalesrep msr on msr.smid=ttv.smid   LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') And ttv.Type IS null  group by smname,ttv.smid  union  select  distinct ttv.smid, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType  where PT.PartyTypeName='INSTITUTIONAL' and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') And ttv.Type IS null  group by smname,ttv.SMID) a order by smname ";

                    }
                    
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
                    rptDiscussion.DataSource = dt;
                    rptDiscussion.DataBind();

                    rptCollection.Visible = false;
                    rptFaildvisit.Visible = false;
                    rptProductive.Visible = false;
                }
                else if (name.Equals("Non-Productive"))
                {
                    lblHeading.InnerText = "DateWise Non-Productive Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select *,'P' As Partytype1 from (select distinct ttv.SMID,(smname) ,
                     count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit
                    from  temp_TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid 
           left join  mastsalesrep msr on msr.smid=ttv.smid 
  where  Isnull(MP.PartyType,0)=0 and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1  group by smname,ttv.SMID  union  select  distinct ttv.SMID,(smname) ,count (ttv.partyid) as TotalDistributors,count (ttv.partyid) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where  Isnull(MP.PartyType,0)=0 and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 group by smname,ttv.SMID)  a order by smname ";
                    }
                    else
                    {
                        sql = @"select *,'I' As Partytype1 from (select distinct ttv.SMID,(smname) ,
                     count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit
                    from  temp_TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid 
           left join  mastsalesrep msr on msr.smid=ttv.smid  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1  group by smname,ttv.SMID  union  select  distinct ttv.SMID,(smname) ,count (ttv.partyid) as TotalDistributors,count (ttv.partyid) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid   LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType  where PT.PartyTypeName='INSTITUTIONAL' and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 group by smname,ttv.SMID)  a order by smname ";
                    }
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
                    rptDiscussion.Visible = false;
                    rptProductive.Visible = false;

                }
                else if (name.Equals("Productive"))
                {
                    lblHeading.InnerText = "DateWise Productive Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select *,'P' As Partytype1 from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalFailedvisit 
from  TransPurchOrder ttv left join mastparty mp on ttv.distid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid 
where  Isnull(MP.PartyType,0)=0 and vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1  group by smname,ttv.SMID)  a order by smname ";

                    }
                    else
                    {
                        sql = @"select *,'I' As Partytype1 from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalFailedvisit
from  TransPurchOrder ttv left join mastparty mp on ttv.distid=mp.partyid
left join  mastsalesrep msr on msr.smid=ttv.smid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and  vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1  group by smname,ttv.SMID)  a order by smname ";
                    }
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
                    rptProductive.DataSource = dt;
                    rptProductive.DataBind();
                    rptFaildvisit.Visible = false;
                    rptCollection.Visible = false;
                    rptDiscussion.Visible = false;

                }
            }
            else
            {
                if (name.Equals("Collection"))
                {
                    lblHeading.InnerText = "DateWise Collections Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
        count (distinct(ttv.distid)) as Totalcollection,'P' As Partytype1 from  distributercollection ttv 
left join mastparty mp on ttv.distid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid  
where  Isnull(MP.PartyType,0)=0 and   ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')  group by smname,ttv.SMID order by smname,ttv.SMID ";
                    }
                    else
                    {
                        sql = @"select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,
        count (distinct(ttv.distid)) as Totalcollection,'I' As Partytype1 from  distributercollection ttv 
left join mastparty mp on ttv.distid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')  group by smname,ttv.SMID order by smname,ttv.SMID ";
                    }
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

                }
                else if (name.Equals("Discussion"))
                {
                    lblHeading.InnerText = "DateWise Discussions Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select  distinct ttv.SMID,(smname) ,
         count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion,'P' As Partytype1
from  temp_transvisitdist ttv left join mastparty mp on ttv.distid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid  
where  Isnull(MP.PartyType,0)=0 and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')  group by smname,ttv.SMID union select  distinct ttv.SMID, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where  Isnull(MP.PartyType,0)=0 and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMID) a order by smname,a.SMID";
                    }
                    else
                    {
                        sql = @"select * from (select  distinct ttv.SMID,(smname) ,
count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion ,'I' As Partytype1
from  temp_transvisitdist ttv left join mastparty mp on ttv.distid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/')  group by smname,ttv.SMID union select  distinct ttv.SMID, (smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalDiscussion from  transvisitdist   ttv left join mastparty mp on ttv.distid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType   where PT.PartyTypeName='INSTITUTIONAL' and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and  vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') group by smname,ttv.SMID) a order by smname,a.SMID";
                    }
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
                    rptDiscussion.DataSource = dt;
                    rptDiscussion.DataBind();

                }
                else if (name.Equals("Non-Productive"))
                {
                    lblHeading.InnerText = "DateWise Non-Productive Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select  distinct ttv.SMID,(smname) ,count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit,'P' As Partytype1
from  temp_TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid 
where Isnull(MP.PartyType,0)=0 and ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1  group by smname,ttv.SMID union select  distinct ttv.SMID, (smname) ,count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  where Isnull(MP.PartyType,0)=0 and  ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 group by smname,ttv.SMID) a order by smname,a.SMID";
                    }
                    else
                    {
                        sql = @"select * from (select  distinct ttv.SMID,(smname) 
,count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit,'I' As Partytype1
from  temp_TransFailedVisit ttv left join mastparty mp on ttv.partyid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and  ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1  group by smname,ttv.SMID union select  distinct ttv.SMID, (smname) ,count (distinct(ttv.partyid)) as TotalDistributors,count (distinct(ttv.partyid)) as TotalFailedvisit from  TransFailedVisit   ttv left join mastparty mp on ttv.partyid=mp.partyid left join  mastsalesrep msr on msr.smid=ttv.smid  LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType    where PT.PartyTypeName='INSTITUTIONAL' and  ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from  MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 )  and vdate =replace(convert(NVARCHAR, DateAdd(minute,330,'" + Date + "'), 106), ' ', '/') and partydist=1 group by smname,ttv.SMID) a order by smname,a.SMID";

                    }
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


                }
                else if (name.Equals("Productive"))
                {
                    lblHeading.InnerText = "DateWise Productive Detail";
                    if (Partytype == "P")
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalFailedvisit,'P' As Partytype1
from  TransPurchOrder ttv left join mastparty mp on ttv.distid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid 
where Isnull(MP.PartyType,0)=0 and   ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 ) and vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1  group by smname,ttv.SMID)  a order by smname ";
                    }
                    else
                    {
                        sql = @"select * from (select distinct ttv.SMID,(smname) ,count (distinct(ttv.distid)) as TotalDistributors,count (distinct(ttv.distid)) as TotalFailedvisit,'I' As Partytype1
from  TransPurchOrder ttv left join mastparty mp on ttv.distid=mp.partyid 
left join  mastsalesrep msr on msr.smid=ttv.smid LEFT JOIN PartyType PT ON PT.PartytypeID=MP.PartyType 
                    where PT.PartyTypeName='INSTITUTIONAL' and   ttv.smid in (  select MastSalesRep.smid from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where smid in (select distinct smid from   MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in  (" + Settings.Instance.SMID + "))) and MastSalesRep.Active=1 ) and vdate>='" + Date + " 00:00" + "' and VDate<='" + Date + " 23:59" + "' and partydist=1  group by smname,ttv.SMID)  a order by smname ";
                    }
                    
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
                    rptProductive.DataSource = dt;
                    rptProductive.DataBind();

                }
            }

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
            catch (Exception ex)
            { ex.ToString(); }
        }

    }
}