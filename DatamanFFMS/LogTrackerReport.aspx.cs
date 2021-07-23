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
    public partial class LogTrackerReport : System.Web.UI.Page
    {
        SqlConnection con = Connection.Instance.GetConnection();
        UploadData upd = new UploadData();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
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
               // GetGroupName(); 
                FillStatus();
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
        private void GetGroupName()
        {
            ddlType.Items.Clear();
            string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
            cmd = new SqlCommand(str, con);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlType.DataSource = dt;
            ddlType.DataBind();
            ddlType.DataTextField = "Description";
            ddlType.DataValueField = "Code";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("--Select--", "0"));
            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));
            con.Close();
        }
        private void BindPerson()
        {
            //SqlCommand cmd = new SqlCommand("select PersonMaster.PersonName as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName", con);
            //SqlCommand cmd = new SqlCommand("select distinct PersonMaster.ID as id,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,PersonMaster.PersonName from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName", con);
            //con.Open();
            //SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            //DataTable dt1 = new DataTable();
            //adpt.Fill(dt1);
            //DropDownList2.DataSource = dt1;
            //DropDownList2.DataBind();
            //DropDownList2.DataTextField = "Person";
            //DropDownList2.DataValueField = "id";
            //DropDownList2.DataBind();
          


            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            dv.RowFilter = "SMName<>.";
            dv.Sort = "SMName asc";
            if (dv.ToTable().Rows.Count > 0)
            {
                DropDownList2.DataSource = dv.ToTable();
                DropDownList2.DataTextField = "SMName";
                DropDownList2.DataValueField = "SMId";
                DropDownList2.DataBind();
            }
            DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));
           
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //GridViewExportUtil.Export("LogReport", gvData);
                GridViewExportUtil.ExportGridToCSV(gvData, "LogReport");
            }
            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }

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
        protected void btngenerate_Click(object sender, EventArgs e)
        {
            try
            {
                Common cs = new Common();
                string PersonDeviceNo = string.Empty;
                string stradd = "";
                if (DropDownList2.SelectedValue != "0" && !string.IsNullOrEmpty(DropDownList2.SelectedValue))
                {
                    PersonDeviceNo = cs.GetDeviceNoByPersonID(DropDownList2.SelectedValue);
                    stradd = " and Log_Tracker.DeviceNo='" + PersonDeviceNo + "'";
                }
                if (ddlstatus.SelectedValue != "0")
                {
                    stradd = stradd + " and  Log_Tracker_Master.Name='" + ddlstatus.SelectedValue + "'";
                }
                //string str1 = " SELECT Log_Tracker.CurrentDate,PersonName,Log_Tracker_Master.Name FROM Log_Tracker_Master INNER JOIN Log_Tracker ON Log_Tracker_Master.Status=Log_Tracker.Status INNER JOIN PersonMaster ON Log_Tracker.DeviceNo=PersonMaster.DeviceNo INNER JOIN GrpMapp ON PersonMaster.ID=GrpMapp.PersonID WHERE  GrpMapp.GroupID='" + ddlType.SelectedValue + "'" +
                //    stradd + " AND ( cast(Log_Tracker.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(Log_Tracker.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime) ) order by Currentdate desc";

                string str1 = " SELECT Log_Tracker.CurrentDate,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as PersonName,Log_Tracker_Master.Name FROM Log_Tracker_Master INNER JOIN Log_Tracker ON Log_Tracker_Master.Status=Log_Tracker.Status INNER JOIN PersonMaster ON Log_Tracker.DeviceNo=PersonMaster.DeviceNo INNER JOIN GrpMapp ON PersonMaster.ID=GrpMapp.PersonID WHERE 1=1 "+
                  stradd + " AND ( cast(Log_Tracker.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(Log_Tracker.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime) ) order by Currentdate desc";
                cmd = new SqlCommand(str1, con);
                con.Open();
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataTable dt1 = new DataTable();
                adpt.Fill(dt1);

                if (dt1.Rows.Count > 0)
                {
                    btnExport.Visible = true;
                    gvData.DataSource = dt1;
                    gvData.DataBind();
                }
                else
                {
                    ShowAlert("No Record Found !"); gvData.DataSource = null; gvData.DataBind();
                    btnExport.Visible = false;
                }

            }
            catch (Exception)
            {
                ShowAlert("There are some problems while loading records!");
            }
        }
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        private void FillStatus()
        {
            string qry = "select Id, Name from Log_Tracker_Master order by Name";
           fillDDLDirect(ddlstatus, qry, "Name", "Name", 2);
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
        protected void TextBox1_TextChanged(object sender, EventArgs e)
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
    }
}