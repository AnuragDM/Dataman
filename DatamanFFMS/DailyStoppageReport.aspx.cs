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
    public partial class DailyStoppageReport : System.Web.UI.Page
    {
        SqlConnection con = Connection.Instance.GetConnection();
        UploadData upd = new UploadData();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        Settings SetObj = Settings.Instance;
        Common cm = new Common();
        private Int32 rowcnt = 0;
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
               // GetGroupName();
                Txt_FromTime.Text = "00:01";
                TextBox2.Text = "23:59";
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
        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            GetData();
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

           // //string strq = "select PersonMaster.PersonName as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
           // string strq = "select distinct PersonMaster.ID as id,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,PersonMaster.PersonName from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
           //fillDDLDirect(DropDownList2, strq, "id", "Person", 1);
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
        protected void btngen_Click(object sender, EventArgs e)
        {
            GetData();
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
        protected void txt_fromdate_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(txt_fromdate.Text, regexDt.ToString());
            Match mtEndDt = Regex.Match(TextBox1.Text, regexDt.ToString());
            if (mtStartDt.Success && mtEndDt.Success)
            {
                if (txt_fromdate.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim() + " " + Txt_FromTime.Text);
                    DateTime dt2 = Convert.ToDateTime(TextBox1.Text.Trim() + " " + TextBox2.Text);
                    if (dt1 <= dt2)
                    {
                    }
                    else
                    {
                        ShowAlert("From DateTime cannot be greater than To DateTime");
                        txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        Txt_FromTime.Text = "00:01";
                        TextBox2.Text = "23:59";
                        return;
                    }
                }
                else
                {
                    ShowAlert("Pls Select From DateTime before selecting To DateTime");
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
                string addqry = ""; DataTable dtbl = new DataTable(); DataTable dtbl1 = new DataTable();
                if (!string.IsNullOrEmpty(DropDownList2.SelectedValue))
                {
                    PersonDeviceNo = cs.GetDeviceNoByPersonID(DropDownList2.SelectedValue);
                    addqry = " and DeviceNo='" + PersonDeviceNo + "'";
                }
                string qry1 = "select SUBSTRING(latitude, 0, 7) as latitude,SUBSTRING(longitude, 0, 7) longitude,CurrentDate,description from LocationDetails where 1=1 " + addqry + "  and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime) order by CurrentDate";
                dtbl1 = DbConnectionDAL.GetDataTable(CommandType.Text, qry1);
                if (dtbl1.Rows.Count > 0)
                {
                    DataTable newtbl = new DataTable();
                    newtbl.Columns.Add("FromDate");
                    newtbl.Columns.Add("ToDate");
                    newtbl.Columns.Add("description");
                    newtbl.Columns.Add("latitude");
                    newtbl.Columns.Add("longitude");
                    newtbl.Columns.Add("count");
                    for (int i = 0; i < dtbl1.Rows.Count; i++)
                    {

                        if (i == 0)
                        {
                            DataRow dr = newtbl.NewRow();
                            dr["FromDate"] = Convert.ToDateTime(dtbl1.Rows[i]["CurrentDate"]).ToString("dd-MMM-yyyy");
                            dr["ToDate"] = dtbl1.Rows[i]["CurrentDate"].ToString();
                            dr["count"] = 0;
                            dr["latitude"] = dtbl1.Rows[i]["latitude"].ToString();
                            dr["longitude"] = dtbl1.Rows[i]["longitude"].ToString();
                            dr["description"] = dtbl1.Rows[i]["description"].ToString();
                            newtbl.Rows.Add(dr);
                            newtbl.AcceptChanges();
                        }
                        else
                        {
                            bool hasitem = false;
                            //string ss = dtbl1.Rows[i]["latitude"].ToString();
                            //string dd = dtbl1.Rows[i]["longitude"].ToString();
                            for (int M = 0; M < newtbl.Rows.Count; M++)
                            {
                                if (dtbl1.Rows[i]["latitude"].ToString() == newtbl.Rows[M]["latitude"].ToString() && dtbl1.Rows[i]["longitude"].ToString() == newtbl.Rows[M]["longitude"].ToString())
                                {
                                    if (dtbl1.Rows[i]["latitude"].ToString() == dtbl1.Rows[i - 1]["latitude"].ToString() && dtbl1.Rows[i]["longitude"].ToString() == dtbl1.Rows[i - 1]["longitude"].ToString())
                                    {
                                        DateTime myDate1 = Convert.ToDateTime(dtbl1.Rows[i - 1]["currentdate"].ToString());
                                        DateTime myDate2 = Convert.ToDateTime(dtbl1.Rows[i]["currentdate"].ToString());
                                        TimeSpan difference = myDate2.Subtract(myDate1);

                                        double totalMinutes = difference.TotalMinutes;

                                        newtbl.Rows[newtbl.Rows.Count - 1]["count"] = Convert.ToInt16(newtbl.Rows[newtbl.Rows.Count - 1]["count"]) + Math.Round(totalMinutes);
                                        // newtbl.Rows[newtbl.Rows.Count - 1]["count"] = Convert.ToInt16(newtbl.Rows[newtbl.Rows.Count - 1]["count"]) + 1;
                                        newtbl.Rows[newtbl.Rows.Count - 1]["ToDate"] = dtbl1.Rows[i]["CurrentDate"];
                                        newtbl.Rows[newtbl.Rows.Count - 1]["description"] = dtbl1.Rows[i]["description"];
                                        newtbl.AcceptChanges();
                                        hasitem = true;
                                        break;
                                    }
                                }
                            }
                            if (!hasitem)
                            {
                                DataRow dr = newtbl.NewRow();
                                dr["FromDate"] = dtbl1.Rows[i]["CurrentDate"].ToString();
                                dr["ToDate"] = dtbl1.Rows[i]["CurrentDate"].ToString(); ;
                                dr["count"] = 1;
                                dr["latitude"] = dtbl1.Rows[i]["latitude"].ToString();
                                dr["longitude"] = dtbl1.Rows[i]["longitude"].ToString();
                                dr["description"] = dtbl1.Rows[i]["description"].ToString();
                                newtbl.Rows.Add(dr);
                                newtbl.AcceptChanges();

                            }
                        }
                    }

                    DataView dv = new DataView(newtbl);
                    if (!string.IsNullOrEmpty(txtstpg.Text.Trim()))
                        dv.RowFilter = "count >=" + txtstpg.Text.Trim();
                    else
                        dv.RowFilter = "count >=0";
                    btnExport.Visible = true;
                    gvData.DataSource = dv;
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
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    DateTime startdate = new DateTime();
                    DateTime enddate = new DateTime();
                    if (!string.IsNullOrEmpty(e.Row.Cells[1].Text) && (e.Row.Cells[1].Text) != "&nbsp;")
                        startdate = (Convert.ToDateTime(e.Row.Cells[1].Text));
                    if (!string.IsNullOrEmpty(e.Row.Cells[2].Text) && (e.Row.Cells[2].Text) != "&nbsp;")
                        enddate = (Convert.ToDateTime(e.Row.Cells[2].Text));
                    TimeSpan objTimeSpan = enddate - startdate;
                    double TotalMinutes = objTimeSpan.TotalMinutes;
                    e.Row.Cells[6].Text = (TotalMinutes + 1).ToString();
                }
                catch (Exception ex) { ex.ToString(); }
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewExportUtil.ExportGridToCSV(gvData, "DailyStoppageReport");
            }
            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }
        }
    }
}