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
    public partial class Distributorlastorderreport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            fillRepeter(null,null);
        }
         private void fillRepeter(object sender, EventArgs e)
        {  
            string sql=string.Empty;            
            {
                sql = "select partyname,orderdate,case when DATEDIFF(day,orderdate,getdate())=0 then 'Order Placed Today' else cast ( DATEDIFF(day,orderdate,getdate()) as varchar) end  diff, DATEDIFF(day,orderdate,getdate()) dd  from (select mp.partyname,distid,max(vdate) orderdate from TransPurchOrder left join mastparty mp on mp.PartyId=TransPurchOrder.distid group by mp.partyname,distid) tbl order by dd desc ";
            }           
            DataTable dt=DbConnectionDAL.GetDataTable(CommandType.Text,sql);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        }
    }
