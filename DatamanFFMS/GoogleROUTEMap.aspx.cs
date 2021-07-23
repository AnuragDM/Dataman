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
    public partial class GoogleROUTEMap : System.Web.UI.Page
    {
        SqlConnection con = Connection.Instance.GetConnection();
        UploadData upd = new UploadData();
        SqlCommand cmd = new SqlCommand();
        Settings SetObj = Settings.Instance;
        Common cm = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                FillType();
                txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                Txt_FromTime.Text = "00:01";
                TextBox2.Text = "23:59";
                BindPerson();
                txtaccu.Text = SetObj.Accuracy;
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
        public string ConvertDataTabletoString()
        {
            DataTable dt = new DataTable();
            string strper = string.Empty;
            string str = string.Empty;
            dt.Clear();

            if (DropDownList2.SelectedItem.Text != "--Please select--")
            {
                if (strper == string.Empty)
                {
                    strper = "'" + DropDownList2.SelectedValue + "'";
                }
            }
            else
            {
                return "";
            }
            if (strper != "")
            {
                str = str + " and PersonMaster.ID in (" + strper + ")";
            }
            if (!string.IsNullOrEmpty(txtaccu.Text))
            {
                str = str + " and cast(Gps_accuracy as numeric(20,0)) <=" + txtaccu.Text.Trim();
            }
            if (!string.IsNullOrEmpty(str))
                str = str + " and cast(LocationDetails.CurrentDate as DateTime) >=cast('" + txt_fromdate.Text + " " + Txt_FromTime.Text + "' as DateTime) AND cast(LocationDetails.CurrentDate as DateTime) < = cast('" + TextBox1.Text + " " + TextBox2.Text + "' as DateTime)";
            if (ddlloc.SelectedValue != "0")
            {
                if (ddlloc.SelectedValue == "C")
                    str = str + " and Log_m in ('C','N')";
                else str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
            }
            if (str == "")
            {
                cmd = new SqlCommand("select Title=CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy from LocationDetails  right Outer join PersonMaster on    LocationDetails.DeviceNo =PersonMaster.DeviceNo where PersonMaster.DeviceNo=null   and  LocationDetails.locationtype is null ", con);
            }
            else
            {
                cmd = new SqlCommand("select Title=CONVERT(VARCHAR(8), locationdetails.CurrentDate, 108),lat=cast(LocationDetails.Latitude as numeric(12,6)),lng=cast(LocationDetails.Longitude as numeric(12,6)),Gps_accuracy from LocationDetails  right Outer join PersonMaster on    LocationDetails.DeviceNo =PersonMaster.DeviceNo where 1=1  " + str + "   and LocationDetails.locationtype is null  order by LocationDetails.Currentdate asc", con);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //serializer.MaxJsonLength = int.MaxValue;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            rows.Clear();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            return serializer.Serialize(rows);

        }

        public void FillType()
        {
            try
            {
                string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
                fillDDLDirect(ddlType, str, "Code", "Description", 1);
                DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception)
            {

            }
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
        private void BindPerson()
        {
            ////string str = "select PersonMaster.PersonName as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            //string str = "select concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )' as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            //DataTable dt = new DataTable();
            //dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //DropDownList2.DataSource = dt;
            //DropDownList2.DataBind();
            //DropDownList2.DataTextField = "Person";
            //DropDownList2.DataValueField = "ID";
            //DropDownList2.DataBind();
            //DropDownList2.Items.Insert(0, new ListItem("--Select--", "0"));
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DropDownList2.DataSource = dv.ToTable();
            DropDownList2.DataTextField = "SMName";
            DropDownList2.DataValueField = "SMId";
            DropDownList2.DataBind();
            //Add Default Item in the DropDownList
            DropDownList2.Items.Insert(0, new ListItem("--Please select--"));
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.GroupID = ddlType.SelectedValue;
            BindPerson();

        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.PersonID = DropDownList2.SelectedValue;
            txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
            SetObj.Accuracy = txtaccu.Text.Trim();

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
                    DateTime dt1 = Convert.ToDateTime(txt_fromdate.Text.Trim()).Date;
                    DateTime dt2 = Convert.ToDateTime(TextBox1.Text.Trim()).Date;
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
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }
        protected void ddlloc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlloc.SelectedValue != "G")
                txtaccu.Text = "5000";
            else
            {
                if (DropDownList2.SelectedIndex > 0)
                { txtaccu.Text = cm.GetAccuracyByPerson(DropDownList2.SelectedValue); }
            }
            SetObj.Accuracy = cm.GetAccuracyByPerson(DropDownList2.SelectedValue);
        }
    }
}