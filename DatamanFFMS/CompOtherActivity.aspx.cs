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

namespace AstralFFMS
{
    public partial class CompOtherActivity : System.Web.UI.Page
    {
        int compid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ComptId"] != null)
            {
                compid = Convert.ToInt32(Request.QueryString["ComptId"].ToString());
            }
            fillRepeter();
        }

        private void fillRepeter()
        {         

             string str = "select Brandactivity,MeetActivity,RoadShow,[Scheme/offers],OtherGeneralInfo from Temp_TransCompetitor where ComptId=" + compid + " " +
                     " union select Brandactivity,MeetActivity,RoadShow,[Scheme/offers],OtherGeneralInfo from TransCompetitor where  ComptId=" + compid + "";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
        }
    }
}