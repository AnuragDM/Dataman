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
using System.Data.SqlClient;
using System.Globalization;

namespace AstralFFMS
{
    public partial class Navasion_pending_order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            fillRepeter();
        }
        private void fillRepeter()
        {
            try
            {
                string sql = string.Empty;
                {
                    sql = "select mp.PartyName,td.PODocId,td.vdate as [order date]  from TransPurchOrder1 td left join mastparty mp on mp.partyid=td.distid where orderdownloaded is null";
                }
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    rpt.DataSource = dt;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('No Record Found');", true);
                }

            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error While Loading Record');", true);

            }
        }
    }
}