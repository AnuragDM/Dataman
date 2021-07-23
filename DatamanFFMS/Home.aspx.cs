using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Web.Script.Serialization;
using DAL;
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class Home : System.Web.UI.Page
    {
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string str = "SELECT compcode FROM MastEnviro";
                string comcode = DbConnectionDAL.GetStringScalarVal(str);
                if (comcode == "Goldiee")
                {
                    DistributorUC.Visible = false;
                    SalesPersonL2UC.Visible = false;
                    SalesPersonL3UC.Visible = false;
                    SalesPersonL1UC.Visible = false;
                }
                else
                {
                    roleType = Settings.Instance.RoleType;
                    GetDashboardAssigned();
                }
            }
        }

        private void GetDashboardAssigned()
        {
            if (roleType == "AreaIncharge")
            {
                SalesPersonL1UC.Visible = true;
                SalesPersonL2UC.Visible = false;
                SalesPersonL3UC.Visible = false;
                DistributorUC.Visible = false;

            }
            else if (roleType == "CityHead" || roleType == "DistrictHead")
            {
                SalesPersonL2UC.Visible = true;
                SalesPersonL1UC.Visible = false;
                SalesPersonL3UC.Visible = false;
                DistributorUC.Visible = false;

            }
            //else if (roleType == "RegionHead" || roleType == "StateHead")
            //{
            //    SalesPersonL3UC.Visible = true;
            //    SalesPersonL2UC.Visible = false;
            //    SalesPersonL1UC.Visible = false;
            //    DistributorUC.Visible = false;

            //}
            else if (roleType == "Distributor")
            {
                DistributorUC.Visible = true;
                SalesPersonL2UC.Visible = false;
                SalesPersonL3UC.Visible = false;
                SalesPersonL1UC.Visible = false;

            }
            else
            {
                DistributorUC.Visible = false;
                SalesPersonL2UC.Visible = false;
                SalesPersonL3UC.Visible = false;
                SalesPersonL1UC.Visible = false;
                Response.Redirect("/DailyDashboard.aspx", true);
            }
        }

        [WebMethod]
        public static string CheckNotifications()
        {
            string data = string.Empty;
            try
            {
                string notQuery1 = @"select top 5 trnsnot.NotiId, trnsnot.displayTitle, trnsnot.FromUserId, trnsnot.msgURL, trnsnot.pro_id, trnsnot.Status, trnsnot.userid, trnsnot.VDate, salesrep.SMName, salesrep.SMId from TransNotification trnsnot left join MastSalesRep salesrep on trnsnot.FromUserId =salesrep.UserId where trnsnot.userid=" + Convert.ToInt32(Settings.Instance.UserID) + " order by trnsnot.NotiId desc";
                DataTable St = DbConnectionDAL.GetDataTable(CommandType.Text, notQuery1);

                string countQuery1 = @"select * from TransNotification where userid =" + Convert.ToInt32(Settings.Instance.UserID) + " and Status=0";
                DataTable St12 = DbConnectionDAL.GetDataTable(CommandType.Text, countQuery1);
                St.Columns.Add("Count");
                St.Rows.Add();
                St.Rows[0]["Count"] = St12.Rows.Count;
                St.AcceptChanges();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in St.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in St.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }


                data = serializer.Serialize(rows);
                return data;
            }
            catch
            {
                return data;
            }
        }
        [WebMethod]
        public static void UpdateTransNotification(string notID)
        {
            if (!string.IsNullOrEmpty(notID))
            {
                try
                {
                    if (Convert.ToInt32(notID) > 0)
                    {
                        string updateNotquery = @"update TransNotification set Status=1 where NotiId=" + Convert.ToInt32(notID) + "";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateNotquery);
                    }
                }
                catch
                {

                }
            }
        }
    }

}
