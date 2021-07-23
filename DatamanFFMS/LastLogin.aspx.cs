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
    public partial class LastLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!Page.IsPostBack)
            {
                string currentMonth = DateTime.Now.Month.ToString();
                DropDownList1.SelectedValue = currentMonth;           
               
            }
            string sMonth = DropDownList1.SelectedValue;
            DropDownList1.SelectedValue = sMonth;
            //sMonth = "select Column1 from TableName where month(DateColumn) = month(dateadd(dd, -1, GetDate()))";
            fillRepeter();
            //string pageName = Path.GetFileName(Request.Path);
            //string Headername = fillDisplayname(pageName);
           // lbllastlogin.Text = Headername;
        }
        private int Month
        {
            get
            {
                return int.Parse(DropDownList1.SelectedItem.Value);
            }
            set
            {
                //this.PopulateMonth();
                DropDownList1.ClearSelection();
                DropDownList1.Items.FindByValue(value.ToString()).Selected = true;
            }
        }
        private string fillDisplayname(string pgname)
        {
            string displayname = string.Empty;
            string mastPartyQry1 = string.Empty;

            mastPartyQry1 = @"select DisplayName from mastpage where PageName in ('" + pgname + "') ";
            displayname = DbConnectionDAL.GetStringScalarVal(mastPartyQry1);
            return displayname;

        }
      
        private void fillRepeter()
        {
            string sMonth = DropDownList1.SelectedValue;
            string sql=string.Empty;
            if(DdlSalesPerson.SelectedValue=="Distributor")
            {
                sql = "select Partyname as EmpName,(select [LoginDateTime] from [LastLoginDetails] where Product='Grahaak_Distributor' and UserId=Mastparty.Userid and  month(logindatetime)=" + sMonth + " ) as LastLogin,(select ApkVersion from [LastLoginDetails] where Product='Grahaak_Distributor' and UserId=Mastparty.Userid and month(logindatetime)=" + sMonth + " ) as Version,'Grahaak-Distributor' App from mastparty where partydist=1 and active=1 order by LastLogin desc,partyname asc";
            }
            else
            {
                string filter = string.Empty;
                //if(DdlSalesPerson.SelectedValue=="Manager")
                //{
                //    filter = " and roleid in (select roleid from mastrole where roletype in ('DistrictHead','StateHead'))";
                //}
                //if (DdlSalesPerson.SelectedValue == "Field")
                {
                    filter = " and roleid in (select roleid from mastrole where roletype in ('DistrictHead','AreaIncharge','StateHead'))";
                }

                sql = "select  Smname+' - '+mobile  as EmpName,(select [LoginDateTime] from [LastLoginDetails] where Product='" + DdlSalesPerson.SelectedValue + "'  and UserId=mastsalesrep.Userid  and  month(logindatetime)=" + sMonth + " ) as LastLogin,(select ApkVersion from [LastLoginDetails] where Product='" + DdlSalesPerson.SelectedValue + "' and UserId=mastsalesrep.Userid  and  month(logindatetime)=" + sMonth + ") as Version,'Grahaak-" + DdlSalesPerson.SelectedValue + "' App  from mastsalesrep  where active=1 and smname not in ('.')  " + filter + "  order by LastLogin desc,Smname asc";
            }

            DataTable dt=DbConnectionDAL.GetDataTable(CommandType.Text,sql);
            rpt.DataSource = dt;
            rpt.DataBind();

            dt.Dispose();
        }

        protected void DdlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRepeter();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRepeter();

        }
    }
}