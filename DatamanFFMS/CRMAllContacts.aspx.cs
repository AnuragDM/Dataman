using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.IO;
using System.Data;
namespace AstralFFMS
{
    public partial class CRMAllContacts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                GetAllContacts();
            }
        }
        private void GetAllContacts()
        {
            try {
                
                string strcontacts = "select mct.Contact_Id,FirstName +' '+ LastName as Name,Compname,(select top(1) Email from CRM_ContactEmail where Contact_Id=mct.Contact_Id) as Email,(select top(1) Phone from CRM_ContactMobile where Contact_Id=mct.Contact_Id) as Mobile from CRM_MastContact mct Inner Join CRM_MastCompany mcn on mct.companyId=mcn.comp_Id order by FirstName";
                DataTable dtcont = new DataTable();
                dtcont = DAL.DbConnectionDAL.GetDataTable(CommandType.Text, strcontacts);
                rpt.DataSource = dtcont;
                rpt.DataBind();
            }
            catch (Exception ex) { ex.ToString(); }
        }
    }
}