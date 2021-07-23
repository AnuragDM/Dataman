using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DAL;
using BusinessLayer;
using BAL;
using System.IO;

namespace AstralFFMS
{
    public partial class AttendanceRpt : System.Web.UI.Page
    {
        Settings SetObj = Settings.Instance;
        Common cm = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnExport);
            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                GetGroupName();
                BindPerson();
                txtaccu.Value = SetObj.Accuracy;

            }
            if (SetObj.GroupID != "0" && SetObj.GroupID != "")
            {

                if (!IsPostBack)
                {
                    ddlType.SelectedValue = SetObj.GroupID;
                    BindPerson();
                }
            }
            else
            {
                SetObj.GroupID = ddlType.SelectedValue;
            }
            if (SetObj.PersonID != "0" && SetObj.PersonID != "")
            {
                if (!IsPostBack)

                    DropDownList2.SelectedValue = SetObj.PersonID;
            }
            else
            {
                SetObj.PersonID = DropDownList2.SelectedValue;
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //GridViewExportUtil.Export("AttendanceReport", gvData);
                GridViewExportUtil.ExportGridToCSV(gvData, "AttendanceReport");
            }
            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }
        }
        private void GetGroupName()
        {
            string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
           fillDDLDirect(ddlType, str, "Code", "Description", 1);
        }
        private void BindPerson()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DropDownList2.DataSource = dv.ToTable();
            DropDownList2.DataTextField = "SMName";
            DropDownList2.DataValueField = "SMId";
            DropDownList2.DataBind();
            //Add Default Item in the DropDownList
            DropDownList2.Items.Insert(0, new ListItem("--Please select--"));

            ////string strq = "select PersonMaster.PersonName as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            //string strq = "select distinct PersonMaster.ID as id,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,PersonMaster.PersonName from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            //fillDDLDirect(DropDownList2, strq, "id", "Person", 2);
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            xddl.DataSource = null;
            xddl.DataBind();
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("All", "0"));
            }
            xdt.Dispose();
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.GroupID = ddlType.SelectedValue;
            BindPerson();
        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.PersonID = DropDownList2.SelectedValue;
            SetObj.Accuracy = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }

        public void ClearData()
        {
            Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageNameWithQueryString(), this);
        }
        protected void txt_fromdate_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
            Match mtEndDt = Regex.Match(TextBox1.Text, regexDt.ToString());
            if (mtStartDt.Success && mtEndDt.Success)
            {
                if (txt_fromdate.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim());
                    DateTime dt2 = Convert.ToDateTime(TextBox1.Text.Trim());
                    if (dt1 <= dt2)
                    {
                    }
                    else
                    {
                        ShowAlert("From Date cannot be greater than To Date");
                        txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        return;
                    }
                }
                else
                {
                    ShowAlert("Pls Select From Date before selecting To Date");
                }
            }
            else
            {
                ShowAlert("Invalid Date!");
            }
        }
        private void GetData()
        {
            try
            {
                Common cs = new Common();
                string PersonDeviceNo = string.Empty;
                string addqry = ""; DataTable dtbl = new DataTable();
                if (DropDownList2.SelectedIndex > 0)
                {
                    PersonDeviceNo = cs.GetDeviceNoByPersonID(DropDownList2.SelectedValue);
                    addqry = " and ld.DeviceNo='" + PersonDeviceNo + "'";
                }
                //string qry = "select min(currentdate) As StartTime, max(currentdate) As EndTime,PersonName,ld.DeviceNo,p.mobile "+
                //    " from LocationDetails ld left join PersonMaster p on ld.deviceno=p.deviceno inner join GrpMapp gmp on p.ID=gmp.PersonID " +
                //           " where ld.CurrentDate BETWEEN CAST('" + txt_fromdate.Text + "' AS DATE) AND  DATEADD(DAY, 1, CAST('" + TextBox1.Text + "' AS DATE))" +
                //           " and gmp.GroupID="+ddlType.SelectedValue + addqry+ " "+
                //           " group by CAST(currentdate AS DATE),PersonName,ld.DeviceNo,mobile " +
                //            " order by CAST(currentdate AS DATE)";
                string qry = " select * from ( select min(currentdate) As StartTime, max(currentdate) As EndTime,concat(p.PersonName, ' ('+ p.empcode ) + ' )' as PersonName,ld.DeviceNo,p.mobile ,'Present' as Remark  " +
                  " from LocationDetails ld  left join PersonMaster p on ld.deviceno=p.deviceno inner join GrpMapp gmp on p.ID=gmp.PersonID " +
                         " where ld.CurrentDate BETWEEN CAST('" + txt_fromdate.Text + "' AS DATE) AND  DATEADD(DAY, 1, CAST('" + TextBox1.Text + "' AS DATE))" +
                         " and ld.deviceno not in (select transleaverequest.DeviceNo from transleaverequest left join PersonMaster p on transleaverequest.deviceno=p.deviceno  and transleaverequest.mobile=p.mobile  where ((transleaverequest.fromdate>=CAST('" + txt_fromdate.Text + "' AS DATE) and transleaverequest.todate<=CAST('" + TextBox1.Text + "' AS DATE)) or (transleaverequest.todate>=CAST('" + txt_fromdate.Text + "' AS DATE) and transleaverequest.fromdate<=CAST('" + TextBox1.Text + "' AS DATE)))) " + addqry + " " +
                         " group by CAST(currentdate AS DATE),PersonName,empcode,ld.DeviceNo,mobile   union select transleaverequest.fromdate as StartTime,transleaverequest.todate as endtime,transleaverequest.personname as PersonName,transleaverequest.DeviceNo,p.mobile,'On Leave Reason :   '+transleaverequest.Reason as Remark  from transleaverequest left join PersonMaster p on transleaverequest.deviceno=p.deviceno  and transleaverequest.mobile=p.mobile  where ((transleaverequest.fromdate>=CAST('" + txt_fromdate.Text + "' AS DATE) and transleaverequest.todate<=CAST('" + TextBox1.Text + "' AS DATE)) or (transleaverequest.todate>=CAST('" + txt_fromdate.Text + "' AS DATE) and transleaverequest.fromdate<=CAST('" + TextBox1.Text + "' AS DATE))) ) tbl  order by StartTime";

                dtbl = DbConnectionDAL.GetDataTable(CommandType.Text, qry);

                if (dtbl.Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    gvData.DataSource = dtbl;
                    gvData.DataBind();
                }
                else
                {
                    ShowAlert("No Record Found !!"); gvData.DataSource = null; gvData.DataBind(); btnExport.Visible = false;
                }

            }
            catch (Exception)
            {
                ShowAlert("There are some problems while loading records!");
            }
        }
        protected void btngen_Click(object sender, EventArgs e)
        {
            GetData();
        }
        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
            Match mtEndDt = Regex.Match(TextBox1.Text, regexDt.ToString());
            if (mtStartDt.Success && mtEndDt.Success)
            {
                if (txt_fromdate.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim());
                    DateTime dt2 = Convert.ToDateTime(TextBox1.Text.Trim());
                    if (dt1 <= dt2)
                    {
                    }
                    else
                    {
                        ShowAlert("From Date cannot be greater than To Date");
                        txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        return;
                    }
                }
                else
                {
                    ShowAlert("Pls Select From Date before selecting To Date");
                }
            }
            else
            {
                ShowAlert("Invalid Date!");
            }
        }
        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            GetData();
        }
    }
}