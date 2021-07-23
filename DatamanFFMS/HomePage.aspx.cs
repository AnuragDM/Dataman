using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.Services;
using DAL;
public partial class HomePage : System.Web.UI.Page
{
    SqlConnection con = Connection.Instance.GetConnection();
    UploadData upd = new UploadData();
    SqlCommand cmd = new SqlCommand();
    private  DataTable dtroutes = null;
    private  DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData(string.Empty);
        }
    }
    private DataTable GetData(string deviceno)
    {
        dt.Clear();
        //string str = "select * from(select Title=(PersonMaster.PersonName) +' ('+convert(nvarchar(20),locationdetails.CurrentDate,113)+')', convert(nvarchar(20),locationdetails.CurrentDate,113) as CurrentDate,PersonMaster.DeviceNo," +
        //               " lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6))," +
        //               " row_number() over (partition by PersonMaster.DeviceNo order by LocationDetails.Currentdate desc) as rn " +
        //               " from LocationDetails  right Outer join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo INNER JOIN GrpMapp ON " +
        //               " PersonMaster.ID=GrpMapp.PersonID INNER JOIN UserGrp ON GrpMapp.GroupID=UserGrp.GroupID " +
        //               "  WHERE UserGrp.UserID='" + Settings.Instance.UserID + "' and " +
        //              "  cast(LocationDetails.CurrentDate as Date) = cast( '" + DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy") + "' as Date)" +
        //            " ) as rm where rn=1";
        SqlParameter[] parameter = { new SqlParameter("@UserID", Settings.Instance.UserID) };


        dt = DbConnectionDAL.getFromDataTable("Sp_GetHomePageData", parameter);// DataAccessLayer.DAL.getFromDataTable("Sp_GetHomePageData", parameter);
          
        rptMarkers.DataSource = dt;
        rptMarkers.DataBind();
        return dt;
    }

    public void ShowAlert(string Message)
    {
        string script = "window.alert(\"" + Message.Normalize() + "\");";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
    }

}