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
using System.Drawing;

namespace AstralFFMS
{
    public partial class FortDistance : System.Web.UI.Page
    {
        SqlConnection con = Connection.Instance.GetConnection();
        UploadData upd = new UploadData();
        SqlCommand cmd = new SqlCommand();
        Settings SetObj = Settings.Instance;
        Common cm = new Common();
        private decimal totalcal = 0;
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
                BindPerson();
                Txt_FromTime.Text = "00:01";
                TextBox2.Text = "23:59";

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
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DistanceCoverageReport.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    //To Export all pages
                    gvData.AllowPaging = false;
                    this.GetData();

                    gvData.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in gvData.HeaderRow.Cells)
                    {
                        cell.BackColor = gvData.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in gvData.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = gvData.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = gvData.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }

                    gvData.RenderControl(hw);

                    //style to format numbers to string
                    string style = @"<style> .textmode { } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }

            }

            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
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
                        ShowAlert("From DateTime Greater Than To DateTime");
                        txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        Txt_FromTime.Text = "00:01";
                        TextBox2.Text = "23:59";
                        return;
                    }
                }
                else
                {
                    ShowAlert("Pls Select From DateTime After Than Select To DateTime");
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
                        ShowAlert("From DateTime Greater Than To DateTime");
                        txt_fromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox1.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        Txt_FromTime.Text = "00:01";
                        TextBox2.Text = "23:59";
                        return;
                    }
                }
                else
                {
                    ShowAlert("Pls Select From DateTime After Than Select To DateTime");
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
                totalcal = 0;
                string strper = string.Empty;
                string str = string.Empty;
                DataTable dt3 = new DataTable();
                Common cs = new Common();
                if (DropDownList2.SelectedItem.Text != "--Select--") { if (strper == string.Empty) { strper = "'" + DropDownList2.SelectedValue + "'"; } }
                if (strper != "") { str = str + "cast(CurrentDate as Date) > = cast( '" + txt_fromdate.Text + "' as Date)"; }
                if (TextBox1.Text != "" && str != "") { str = str + " and cast(CurrentDate as Date) < = cast('" + TextBox1.Text + "' as Date)"; }
                if (Txt_FromTime.Text != "" && str != "") { str = str + " and cast(CurrentDate as time) > = cast('" + Txt_FromTime.Text + "' as time)"; }
                if (TextBox2.Text != "" && str != "") { str = str + " and cast(CurrentDate as time) < = cast('" + TextBox2.Text + "' as time)"; }
                //if (ddlloc.SelectedValue != "0")
                //{
                //    str = str + " and Log_m='" + ddlloc.SelectedValue + "'";
                //}
                string PersonDeviceNo = string.Empty;
                if (DropDownList2.SelectedIndex > 0)
                {
                    PersonDeviceNo = cs.GetDeviceNoByPersonID(DropDownList2.SelectedValue);
                }
                if (str != "")
                {
                    //DataTable dt1 = new DataTable();
                    //cmd = new SqlCommand("select PersonMaster.DeviceNo as DeviceNo from GrpMapp inner join PersonMaster on GrpMapp.PersonID =PersonMaster.ID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName", con);
                    //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                    //da1.Fill(dt1);
                    DataTable dt;
                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("Currentdate", typeof(DateTime));
                    dt2.Columns.Add("title");
                    dt2.Columns.Add("Distance");
                    dt2.AcceptChanges();
                    //for (int l = 0; l < dt1.Rows.Count; l++)
                    //{
                    //dt = new DataTable();
                    //dt.Clear();
                    //cmd = new SqlCommand("select * from (select max(a.title) as title,a.CurrDate as CurrDate,a.Distance as Distance from (select title=PersonMaster.PersonName, (select top 1 CurrentDate from LocationDetails where LocationDetails.DeviceNo = '" + PersonDeviceNo + "' AND Log_m ='G' order by CurrentDate desc) as CurrDate,'0.00' as Distance from LocationDetails  inner join PersonMaster on LocationDetails.DeviceNo =PersonMaster.DeviceNo inner join GrpMapp on GrpMapp.PersonID = PersonMaster.ID where LocationDetails.DeviceNo = '" + PersonDeviceNo + "' AND Log_m ='G') a   group by a.title,a.CurrDate,a.Distance) s order by s.CurrDate asc", con);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //da.Fill(dt);
                    //if (dt.Rows.Count > 0)
                    //{
                    //cmd = new SqlCommand("select Latitude as Latitude,Longitude as Longitude,'' as Distance, Currentdate, (select personname from personmaster where deviceno='" + PersonDeviceNo + "') as title from Locationdetails where " + str + " and DeviceNo = '" + PersonDeviceNo + "' AND Log_m ='G' GROUP BY CurrentDate,Latitude,Longitude order By CurrentDate asc", con);
                    cmd = new SqlCommand("select Latitude as Latitude,Longitude as Longitude,'' as Distance, Currentdate, (select concat(PersonMaster.PersonName, ' ('+ PersonMaster.empcode ) + ' )' as personname from personmaster where deviceno='" + PersonDeviceNo + "') as title from Locationdetails where " + str + " and DeviceNo = '" + PersonDeviceNo + "' AND Log_m ='G' and ( LocationDetails.locationtype is null or LocationDetails.locationtype ='') GROUP BY CurrentDate,Latitude,Longitude order By CurrentDate asc", con);

                    SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                    da2.Fill(dt3);
                    if (dt3.Rows.Count > 0)
                    {
                        double cal = 0.00;
                        DataRow dr = dt2.NewRow(); dr[0] = DateTime.Now.Date;
                        for (int k = 0; k <= dt3.Rows.Count - 2; k++)
                        {

                            if (k == 0) { dr = dt2.NewRow(); }
                            else
                            {
                                if (Convert.ToDateTime(dt3.Rows[k]["Currentdate"]).ToShortDateString() != Convert.ToDateTime(dt3.Rows[k - 1]["Currentdate"]).ToShortDateString())
                                {
                                    dr["Currentdate"] = Convert.ToDateTime(dt3.Rows[k - 1]["Currentdate"]).ToShortDateString().ToString();
                                    dr["title"] = dt3.Rows[0]["title"].ToString();
                                    dr["Distance"] = Math.Round(cal, 2);
                                    dt2.Rows.Add(dr);
                                    if (k > 0)
                                    {
                                        dr = dt2.NewRow(); dr[0] = DateTime.Now.Date; cal = 0.0;
                                    }
                                }
                            }

                            Double abc = Calculate(Convert.ToDouble(dt3.Rows[k].ItemArray[0]), Convert.ToDouble(dt3.Rows[k].ItemArray[1]), Convert.ToDouble(dt3.Rows[k + 1].ItemArray[0]), Convert.ToDouble(dt3.Rows[k + 1].ItemArray[1]));
                            cal = cal + Math.Round(abc * 1609 / 1000, 2);
                            //totalcal+=totalcal+cal;
                            //totalcal = Math.Round(totalcal, 3);

                            if (dt3.Rows.Count > 2)
                            {
                                if (k == dt3.Rows.Count - 2)
                                {
                                    dr["Currentdate"] = Convert.ToDateTime(dt3.Rows[k - 1]["Currentdate"]).ToShortDateString().ToString();
                                    dr["title"] = dt3.Rows[0]["title"].ToString();
                                    dr["Distance"] = Math.Round(cal, 2);
                                    dt2.Rows.Add(dr);
                                }
                            }

                        }


                        dt3.Clear();

                    }

                    if (dt2.Rows.Count > 0)
                    {
                        btnExport.Visible = true; td_totaldist.Visible = true;

                    }
                    else
                    {
                        dt2.Rows.Clear();
                        ShowAlert("No Record Found !");
                        btnExport.Visible = false;
                        td_totaldist.Visible = false;
                    }
                    DataView dv = new DataView(dt2);
                    dv.Sort = "Currentdate asc";
                    gvData.DataSource = dv;
                    gvData.DataBind();
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
        public static double Calculate(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
            var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
            var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
            var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
        }
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int i = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                foreach (TableCell cell in e.Row.Cells)
                {
                    i++;
                    if (cell.Text.Length > 12 && (i == 2))
                        cell.Text = cell.Text.Substring(0, 12);

                }
                totalcal += Convert.ToDecimal(e.Row.Cells[3].Text);
            }
            lbldistTotal.Text = totalcal.ToString();
        }
        private void GetGroupName()
        {
            string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
            cmd = new SqlCommand(str, con);
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlType.DataSource = dt;
            ddlType.DataBind();
            ddlType.DataTextField = "Description";
            ddlType.DataValueField = "Code";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("--Select--", "0"));
            con.Close();
        }
        private void BindPerson()
        {
            ////string str = "select PersonMaster.PersonName as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
            //string str = "select distinct PersonMaster.ID as id,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,PersonMaster.PersonName from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlType.SelectedValue + "' order by PersonMaster.PersonName";
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
        public void ShowAlert(string Message)
        {
            string script = "window.alert(\"" + Message.Normalize() + "\");";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UniqueKey", script, true);
        }

        public void ClearData()
        {
            Settings.Instance.RedirectCurrentPage(Settings.Instance.GetCurrentPageNameWithQueryString(), this);
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            GetData();
        }
    }
}