using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class PendingDSRList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["SMId"] != null)
                {
                    GetDetailDSRData(Convert.ToInt32(Request.QueryString["SMId"]));
                }
            }
        }

        private void GetDetailDSRData(int p)
        {
            try
            {
                string dsrQuery = @"select *,MastSalesRep.SMName from TransVisit left join MastSalesRep on TransVisit.SMId=MastSalesRep.SMId where TransVisit.SMId=" + p + " and TransVisit.AppStatus='Pending'";
                DataTable pendingDSRdt = DbConnectionDAL.GetDataTable(CommandType.Text, dsrQuery);
                if (pendingDSRdt.Rows.Count > 0)
                {
                    rptDSR.DataSource = pendingDSRdt;
                    rptDSR.DataBind();
                }
                else
                {
                    rptDSR.DataSource = pendingDSRdt;
                    rptDSR.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");
        }
    }
}