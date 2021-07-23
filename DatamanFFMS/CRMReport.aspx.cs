using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class CRMReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                Bindgvdata();
            }
        }

        private void Bindgvdata()
        {
            string strQ = "";

            string strsubquery = "";
            strsubquery = @"select SMID from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 ";
         


            strQ = " Select Max(a.SalesPerson) As SalesPerson,Max(a.SMID) as SMID ,Sum(Tstatusopen) As Tstatusopen, Sum(Tstatusclose) As Tstatusclose ,Count(Leadcount) As Leadcount,Sum(LeadStatusCold) As LeadStatusCold,Sum(LeadStatusWarm) as LeadStatusWarm ,Sum(LeadStatuslost) as LeadStatuslost ";
            strQ = strQ + ",Sum(LeadStatuswin) as LeadStatuswin,Sum(LeadStatushot) as LeadStatushot  From(Select MastSalesRep.SMName As SalesPerson,(Case when CRM_MastStatus.Status='' then 0 else 1 end) As LeadStatusCold,mct.Manager As SMID , (select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)  from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='o') as Tstatusopen,";
            strQ = strQ + " (select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)  from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='c') as Tstatusclose,	 mct.Contact_Id As Leadcount,'' as LeadStatusWarm, '' as LeadStatuslost,'' as LeadStatuswin,'' as LeadStatushot, (mct.Contact_Id) from CRM_MastContact mct ";
            strQ = strQ + "Left Join MastSalesRep on MastSalesRep.SMID=mct.Manager Left Join CRM_MastStatus on CRM_MastStatus.Status_Id = mct.Status_Id where Manager in ( " + strsubquery + " )  and CRM_MastStatus.Status ='COLD' and mct.Flag='L' Union all ";
            strQ = strQ + " Select MastSalesRep.SMName As SalesPerson,'' As LeadStatusCold,mct.Manager As SMID , (select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)  from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='o') as Tstatusopen, (select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)   from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='c') as Tstatusclose,	 mct.Contact_Id As Leadcount,(Case when CRM_MastStatus.Status='' then 0 else 1 end) as LeadStatusWarm,";
            strQ = strQ + " '' as LeadStatuslost,'' as LeadStatuswin,'' as LeadStatushot,(mct.Contact_Id) from CRM_MastContact mct Left Join MastSalesRep on MastSalesRep.SMID=mct.Manager Left Join CRM_MastStatus on CRM_MastStatus.Status_Id = mct.Status_Id where Manager in (" + strsubquery + ") and CRM_MastStatus.Status ='WARM' and mct.Flag='L' Union all ";
            strQ = strQ + " Select MastSalesRep.SMName As SalesPerson,'' As LeadStatusCold,mct.Manager As SMID , (select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)   from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='o') as Tstatusopen,(select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)   from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='c') as Tstatusclose,	 mct.Contact_Id As Leadcount,'' as LeadStatusWarm,(Case when CRM_MastStatus.Status='' then 0 else 1 end) as LeadStatuslost,'' as LeadStatuswin,'' as LeadStatushot,(mct.Contact_Id) from CRM_MastContact mct Left Join MastSalesRep on MastSalesRep.SMID=mct.Manager Left Join CRM_MastStatus on CRM_MastStatus.Status_Id = mct.Status_Id ";

            strQ = strQ + " where Manager in ( " + strsubquery + ") and CRM_MastStatus.Status ='LOST' and mct.Flag='L'	 UNION ALL ";

            strQ = strQ + "	Select MastSalesRep.SMName As SalesPerson,'' As LeadStatusCold,mct.Manager As SMID , (select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)   from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='o') as Tstatusopen,(select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)   from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='c') as Tstatusclose,	 mct.Contact_Id As Leadcount,'' as LeadStatusWarm,";
            strQ = strQ + "'' as LeadStatuslost,(Case when CRM_MastStatus.Status='' then 0 else 1 end) as LeadStatuswin,'' as LeadStatushot,(mct.Contact_Id) from CRM_MastContact mct Left Join MastSalesRep on MastSalesRep.SMID=mct.Manager Left Join CRM_MastStatus on CRM_MastStatus.Status_Id = mct.Status_Id where Manager in ( " + strsubquery + " ) and CRM_MastStatus.Status ='WIN' and mct.Flag='L' Union all ";

            strQ = strQ + "	Select MastSalesRep.SMName As SalesPerson,'' As LeadStatusCold,mct.Manager As SMID , (select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)  from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='o') as Tstatusopen,(select Count(CASE ct.status  WHEN 'c' THEN 'Close' when 'o' then 'Open' else ''  END)   from crm_task ct where ct.Contact_Id=mct.Contact_Id  and ct.status='c') as Tstatusclose,	 mct.Contact_Id As Leadcount,'' as LeadStatusWarm,";
            strQ = strQ + "	'' as LeadStatuslost,'' as LeadStatuswin,(Case when CRM_MastStatus.Status='' then 0 else 1 end) as LeadStatushot,(mct.Contact_Id) from CRM_MastContact mct Left Join MastSalesRep on MastSalesRep.SMID=mct.Manager Left Join CRM_MastStatus on CRM_MastStatus.Status_Id = mct.Status_Id where Manager in (" + strsubquery + " )	 and CRM_MastStatus.Status ='HOT' and mct.Flag='L') as a Group by a.SMID";
             
            

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
              //  divbtns.Visible = true;
                gvData.DataSource = dt;
                gvData.DataBind();
                gvData.Visible = true;
                //foreach (GridViewRow row in gvData.Rows)
                //{


                //        gvData.Columns[3].Visible = true;
                //        gvData.Columns[4].Visible = true;
                //        gvData.Columns[5].Visible = true;
                //    }


                //}
            }
            else
            {
                // divbtns.Visible = false;
                gvData.DataSource = null;
                gvData.DataBind();
            }
        }

    }
}