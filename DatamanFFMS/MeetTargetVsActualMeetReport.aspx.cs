using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.Services;
using DAL;

namespace AstralFFMS
{
    public partial class MeetTargetVsActualMeetReport : System.Web.UI.Page
    {
       

      public static int uid = 0;
        public static int smID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currDateLabel.Text = DateTime.Parse(DateTime.UtcNow.ToShortDateString()).ToString("dd/MMM/yyyy");
                fillUnderUsers();
                //    GetUserID(Convert.ToString(Session["user_name"]));
                //    smID = GetSalesPerId(uid);
            }
            if (Request.QueryString["Date"] != null && Request.QueryString["SMID"] != null)
            {
                dateHiddenField.Value = Request.QueryString["Date"];
                smIDHiddenField.Value = Request.QueryString["SMID"];
                smID =Convert.ToInt32(Request.QueryString["SMID"]);//GetSalesPerId(vistIDHiddenField.Value);
              //  saleRepName.Text = GetSalesPersonName(smID);
              //  dateLabel.Text = Convert.ToDateTime(dateHiddenField.Value).ToString("dd/MMM/yyyy");
            }

        }
        private void fillUnderUsers()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                }
                catch { }

            }
        }

         [WebMethod]
        public static string GetDailyWorkingReport(string SMID)
        {
            string data = string.Empty;
                //beat = "",Query = "", QryDemo = "", QryFv = "", QryOrder = "";
            DataTable dtLocTraRep = new DataTable();
            try
            {
                
                string str = @"select sum(JanValue)+sum(FebValue)+sum(MarValue)+sum(AprValue)+sum(MayValue)+sum(JunValue)+sum(JulValue)+sum(AugValue)+sum(SepValue)+sum(OctValue)+sum(NovValue)+sum(DecValue) as Total, T.Name as MeetType,'Taget' as Target,'' as MeetName
from MeetTragetFromHO M left join MastMeetType T on M.PartyTypeId=T.Id where SMID="+SMID+" group by T.Name union all select 1 as Total,  T.Name as MeetType,'Actual' as Target,TM.MeetName  from TransMeetPlanEntry TM left join MeetTragetFromHO E on TM.SMId =E.SMId left join MastMeetType T on TM.MeetTypeId=T.Id  where TM.SMId="+SMID+"group by T.Name,TM.MeetName";

               dtLocTraRep = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dtLocTraRep.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dtLocTraRep.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }


                data = serializer.Serialize(rows);
                return data;
            }
            catch (Exception)
            {
                return data;
            }

        }

        private string GetSalesPersonName(int smID)
        {
            //var query = from u in context.MastSalesReps
            //            where u.SMId == smID
            //            select new { u.SMId, u.SMName };
            string salesrepqry1 = @"select * from MastSalesRep where SMId=" + smID + "";
            DataTable dtsalesrepqry1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry1);
            if (dtsalesrepqry1.Rows.Count > 0)
            {
                return Convert.ToString(dtsalesrepqry1.Rows[0]["SMName"]);
            }
            else
            {
                return string.Empty;
            }
        }

        private int GetSalesPerId(string p)
        {
            //var query = from u in context.TransVisits
            //            where u.VisId == Convert.ToInt64(p)
            //            select new { u.SMId };
            string salesrepqry = @"select * from TransVisit where VisId=" + Convert.ToInt64(p) + "";
            DataTable dtsalesrepqry = DbConnectionDAL.GetDataTable(CommandType.Text, salesrepqry);
            if (dtsalesrepqry.Rows.Count>0)
            {
                return Convert.ToInt32(dtsalesrepqry.Rows[0]["SMId"]); 
            }
            else
            {
                return 0;
            }
        }

        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
       
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptDailyWorkingSummaryL1.aspx");
        }

        protected void btnLock_Click(object sender, EventArgs e)
        {

        }
    }
}