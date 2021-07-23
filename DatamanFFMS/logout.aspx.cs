using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AstralFFMS;
using DAL;
using BusinessLayer;
using System.Web.Configuration;


namespace AstralFFMS
{
    public partial class logout : System.Web.UI.Page
    {
        BAL.ProjectMaster.ProjectMasterBAL dp = new BAL.ProjectMaster.ProjectMasterBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["Reset"])))
            {
                int retval = 0;
                DbParameter[] dbParam = new DbParameter[1];
                dbParam[0] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_ReSetCRMData", dbParam);
                retval = Convert.ToInt32(dbParam[0].Value);
                if (retval == 1)
            {
                Session["msz"] = "Data Reset Successfully!";
            }
            else
            {
                Session["msz"] = " There are some errors while Data Reset!!";
            }
        
            }
            else
            {
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
            }
            Response.Redirect("~/Loginn.aspx", true);
           
        }
    }
}