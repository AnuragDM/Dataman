using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BAL;
using System.Data;
using System.Web.Script.Serialization;

namespace AstralFFMS
{
    public partial class DepartmentMasterList : System.Web.UI.Page
    {
        BAL.Department.deptAll dp = new BAL.Department.deptAll();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
          //      GetDeptData();
                fillRepeter();
            }
        }
        private void fillRepeter()
        {

            string str = @"select DepId,DepName,SyncId,CASE WHEN Active = 1 
        THEN 'Yes' ELSE 'No'  END as active from MastDepartment";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
        }
        [WebMethod]
        public static string GetDeptData()
        {
            string data = string.Empty;
            try
            {
                string str = @"select DepId,DepName,SyncId,CASE WHEN Active = 1 
        THEN 'Yes' ELSE 'No'  END as active from MastDepartment";

                DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in depdt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in depdt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                data = serializer.Serialize(rows);
                return data;

            }
            catch (Exception ex)
            {
                ex.ToString();
                return data;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DepartmentMaster.aspx");
        }
    }
}