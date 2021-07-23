using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class DetailDistLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["DistId"] != null)
                {
                    BindDDLMonth();
                    DateTime date1 = Convert.ToDateTime("1/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                    DateTime date2 = Convert.ToDateTime(DateTime.DaysInMonth(Convert.ToInt32(Settings.GetUTCTime().Year), Convert.ToInt32(Settings.GetUTCTime().Month)) + "/" + Settings.GetUTCTime().Month + "/" + Settings.GetUTCTime().Year);
                    monthDropDownList.SelectedValue = Settings.GetUTCTime().Month.ToString();
                    yearDropDownList.SelectedValue = Settings.GetUTCTime().Year.ToString();
                    string str = " select PartyName from MastParty where partyid= '" + Request.QueryString["DistId"] + "'";
                    DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt1.Rows.Count > 0)
                        lblDist.Text = dt1.Rows[0]["PartyName"].ToString();
                    GetDetailLedgerData(Convert.ToInt32(Request.QueryString["DistId"]), date1, date2);
                }
            }
        }
        private void BindDDLMonth()
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    monthDropDownList.Items.Add(new ListItem(monthName.Substring(0, 3), (month.ToString().PadLeft(2, '0')).TrimStart('0')));
                }
                for (int i = System.DateTime.Now.Year - 10; i <= (System.DateTime.Now.Year); i++)
                {
                    yearDropDownList.Items.Add(new ListItem(i.ToString()));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void GetDetailLedgerData(int distId, DateTime date1, DateTime date2)
        {
            try
            {
                //         DataTable dt = Settings.DetailDistLedger(distId);
                DbParameter[] dbParam = new DbParameter[3];
                dbParam[0] = new DbParameter("@Distributor_Id", DbParameter.DbType.Int, 1, distId);
                dbParam[1] = new DbParameter("@From_Date", DbParameter.DbType.DateTime, 1, date1);
                dbParam[2] = new DbParameter("@To_Date", DbParameter.DbType.DateTime, 1, date2);
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_selectDistributorLedger", dbParam);
                if (dt.Rows.Count > 0)
                {
                    rptDistLedger.DataSource = dt;
                    rptDistLedger.DataBind();
                }
                else
                {
                    rptDistLedger.DataSource = dt;
                    rptDistLedger.DataBind();
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

        protected void GetLedgerDeatil_Click(object sender, EventArgs e)
        {
            DateTime date1 = Convert.ToDateTime("1/" + monthDropDownList.SelectedValue + "/" + yearDropDownList.SelectedValue);
            DateTime date2 = Convert.ToDateTime(DateTime.DaysInMonth(Convert.ToInt32(yearDropDownList.SelectedValue), Convert.ToInt32(monthDropDownList.SelectedValue)) + "/" + monthDropDownList.SelectedValue + "/" + yearDropDownList.SelectedValue);
            GetDetailLedgerData(Convert.ToInt32(Request.QueryString["DistId"]), date1, date2);
           
        }
    }
}