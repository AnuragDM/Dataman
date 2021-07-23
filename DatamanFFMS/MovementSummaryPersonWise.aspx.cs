using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DataAccessLayer;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Services;
using System.Net;
using System.Text;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Runtime.Remoting.Contexts;
//using localhost;
using BusinessLayer;
using DAL;
using BAL;
using System.Text.RegularExpressions;
namespace AstralFFMS
{
    public partial class MovementSummaryPersonWise : System.Web.UI.Page
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
            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnexport);
            if (!IsPostBack)
            {
                TextBox3.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                TextBox5.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
               // GetGroupName();
                TextBox4.Text = "09:00";
                TextBox6.Text = "18:00";
                //   Button4_Click(sender, e);
                  BindPerson();
                txtaccu.Value = SetObj.Accuracy;
            }
            if (SetObj.GroupID != "0" && SetObj.GroupID != "")
            {

                if (!IsPostBack)
                {
                    // DropDownList1.SelectedValue = SetObj.GroupID;
                    // BindPerson();
                }
            }
            else
            {
                //SetObj.GroupID = DropDownList1.SelectedValue;
            }
            if (SetObj.PersonID != "0" && SetObj.PersonID != "")
            {
                // if (!IsPostBack)

                // DropDownList3.SelectedValue = SetObj.PersonID;
                // SetObj.DeviceNo = cm.GetDeviceNoByPersonID(DropDownList3.SelectedValue);
            }
            else
            {
                // SetObj.PersonID = DropDownList3.SelectedValue;
                //  SetObj.DeviceNo = cm.GetDeviceNoByPersonID(DropDownList3.SelectedValue);
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
        private void GetGroupName()
        {
            string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
            fillDDLDirect(DropDownList1, str, "Code", "Description", 1);
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


        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetObj.GroupID = DropDownList1.SelectedValue;
            //  BindPerson();
            Session["SummaryPersonWiseGroupid"] = DropDownList1.SelectedValue;
        }
        //protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetObj.PersonID = DropDownList3.SelectedValue;
        //    SetObj.Accuracy = cm.GetAccuracyByPerson(DropDownList3.SelectedValue);
        //    SetObj.DeviceNo = cm.GetDeviceNoByPersonID(DropDownList3.SelectedValue);
        //}
        protected void TextBox3_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(TextBox3.Text, regexDt.ToString());
            Match mtEndDt = Regex.Match(TextBox5.Text, regexDt.ToString());
            if (mtStartDt.Success && mtEndDt.Success)
            {
                if (TextBox3.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(TextBox3.Text.Trim() + " " + TextBox4.Text);
                    DateTime dt2 = Convert.ToDateTime(TextBox5.Text.Trim() + " " + TextBox6.Text);
                    if (dt1 <= dt2)
                    {
                    }
                    else
                    {
                        ShowAlert("From DateTime Greater Than To DateTime");
                        TextBox3.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox5.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox4.Text = "09:00";
                        TextBox6.Text = "18:00";
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
        protected void TextBox5_TextChanged(object sender, EventArgs e)
        {
            Regex regexDt = new Regex("(^(((([1-9])|([0][1-9])|([1-2][0-9])|(30))\\-([A,a][P,p][R,r]|[J,j][U,u][N,n]|[S,s][E,e][P,p]|[N,n][O,o][V,v]))|((([1-9])|([0][1-9])|([1-2][0-9])|([3][0-1]))\\-([J,j][A,a][N,n]|[M,m][A,a][R,r]|[M,m][A,a][Y,y]|[J,j][U,u][L,l]|[A,a][U,u][G,g]|[O,o][C,c][T,t]|[D,d][E,e][C,c])))\\-[0-9]{4}$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-8]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][1235679])|([13579][01345789]))$)|(^(([1-9])|([0][1-9])|([1][0-9])|([2][0-9]))\\-([F,f][E,e][B,b])\\-[0-9]{2}(([02468][048])|([13579][26]))$)");
            Match mtStartDt = Regex.Match(TextBox3.Text, regexDt.ToString());
            Match mtEndDt = Regex.Match(TextBox5.Text, regexDt.ToString());
            if (mtStartDt.Success && mtEndDt.Success)
            {
                if (TextBox3.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(TextBox3.Text.Trim() + " " + TextBox4.Text);
                    DateTime dt2 = Convert.ToDateTime(TextBox5.Text.Trim() + " " + TextBox6.Text);
                    if (dt1 <= dt2)
                    {
                    }
                    else
                    {
                        ShowAlert("From DateTime Greater Than To DateTime");
                        TextBox3.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox5.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");
                        TextBox4.Text = "09:00";
                        TextBox6.Text = "18:00";
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
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(SetObj.DeviceNo) || SetObj.DeviceNo == "0")
                {
                    ShowAlert("Please Select the Person again");
                    return;
                }
                DataTable dttemp = new DataTable();
                dttemp.Columns.Add("Slot"); dttemp.Columns.Add("MinSlot");
                dttemp.Columns.Add("MaxSlot");
                DateTime Fdate = DateTime.MinValue;
                DateTime Tdate = DateTime.MinValue;
                string frmtm = TextBox4.Text.Replace(":", ".");
                string totm = TextBox6.Text.Replace(":", ".");
                double tmdiff = Convert.ToDouble(totm) - Convert.ToDouble(frmtm);
                string[] spdiff = tmdiff.ToString().Split('.');
                int tmhr = 0;
                if (spdiff.Length > 1)
                {
                    tmhr = (Convert.ToInt32(spdiff[0]) * 60) + Convert.ToInt32(spdiff[1]);
                }
                else
                    tmhr = (Convert.ToInt32(spdiff[0]) * 60);
                int loops = Convert.ToInt32(tmhr / Convert.ToInt16(DropDownList4.SelectedValue));
                string addQry = "select (Person) as Person,(date) as Date, ";
                string Fqry = "";//"select '"+DropDownList3.SelectedItem.Text+"' as Person,convert(varchar(12),vdate,113) as Date, ";
                string mtimeStamp = "";
                int addmin = 0, Tomin = 0;
                string hrs = TextBox4.Text.ToString().Replace(":", ".");
                string[] spthrs = hrs.Split('.');
                if (spthrs.Length > 1)
                    addmin = (Convert.ToInt32(spthrs[0]) * 60) + (Convert.ToInt32(spthrs[1]));
                else
                    addmin = (Convert.ToInt32(spthrs[0]) * 60);

                string Tohrs = TextBox6.Text.ToString().Replace(":", ".");
                string[] sptTohrs = Tohrs.Split('.');
                if (sptTohrs.Length > 1)
                    Tomin = (Convert.ToInt32(sptTohrs[0]) * 60) + (Convert.ToInt32(sptTohrs[1]));
                else
                    Tomin = (Convert.ToInt32(sptTohrs[0]) * 60);

                string mdate = Convert.ToDateTime(TextBox3.Text).ToShortDateString();
                for (int i = 0; i <= loops; i++)
                {
                    DataRow dr = dttemp.NewRow();
                    if (i == 0)
                    {

                        Fdate = Convert.ToDateTime(TextBox3.Text + " " + TextBox4.Text);
                        mtimeStamp = String.Format("{0:HH:mm}", Fdate);
                        addQry += "max([" + mtimeStamp + "]) as '" + mtimeStamp + "'";
                        dr["Slot"] = mtimeStamp;
                        if (DropDownList4.SelectedValue == "30")
                        {
                            dr["MinSlot"] = Convert.ToInt32(addmin - 15);
                            dr["MaxSlot"] = Convert.ToInt32(addmin) + 15;
                        }
                        else
                        {
                            dr["MinSlot"] = Convert.ToInt32(addmin - 30);
                            dr["MaxSlot"] = Convert.ToInt32(addmin) + 30;
                        }

                        //    Fqry="select '"+DropDownList3.SelectedItem.Text+"' as Person,  convert(varchar(12),vdate,113) as Date,  description as '09:00',''          as '09:30',''          as '10:00',''          as '10:30',''          as '11:00',''          as '11:30',''          as '12:00',''          as '12:30',''          as '13:00',''          as '13:30',''          as '14:00',''          as '14:30',''          as '15:00',''          as '15:30',''          as '16:00',''          as '16:30',''          as '17:00',''          as '17:30',''          as '18:00' from locationdetails where deviceno='865622026355623' and   cast(CurrentDate as DateTime) >=cast('02-May-2015 09:00' as DateTime) AND cast(CurrentDate as DateTime) < = cast('02-Jun-2015 18:00' as DateTime) and slot between 510 and 540 union
                        // addQry += " dbo.RetAdd(deviceno,vdate," + Convert.ToInt32(addmin - Convert.ToInt16(DropDownList4.SelectedValue)) + "," + Convert.ToInt32(addmin) + ")  as '" + mtimeStamp + "'";
                        //  Fqry += " dbo.RetAdd(deviceno,vdate," + Convert.ToInt32(addmin - Convert.ToInt16(DropDownList4.SelectedValue)) + "," + Convert.ToInt32(addmin) + ")  as '" + mtimeStamp + "'";
                    }
                    else
                    {
                        Tdate = Convert.ToDateTime(Fdate).AddMinutes(Convert.ToInt16(DropDownList4.SelectedValue));
                        mtimeStamp = String.Format("{0:HH:mm}", Tdate);
                        addQry += ",max([" + mtimeStamp + "]) as '" + mtimeStamp + "'";
                        dr["Slot"] = mtimeStamp;


                        //  addQry+=",dbo.RetAdd(deviceno,vdate," + Convert.ToInt32(addmin) + "," + Convert.ToInt32(addmin + Convert.ToInt16(DropDownList4.SelectedValue)) + ")  as '" + mtimeStamp + "'";
                        //Fqry += ",dbo.RetAdd(deviceno,vdate," + Convert.ToInt32(addmin) + "," + Convert.ToInt32(addmin + Convert.ToInt16(DropDownList4.SelectedValue)) + ")  as '" + mtimeStamp + "'";
                        addmin = addmin + Convert.ToInt16(DropDownList4.SelectedValue);
                        if (DropDownList4.SelectedValue == "30")
                        {
                            dr["MinSlot"] = Convert.ToInt32(addmin) - 15;
                            dr["MaxSlot"] = Convert.ToInt32(addmin + 15);
                        }
                        else
                        {
                            dr["MinSlot"] = Convert.ToInt32(addmin) - 30;
                            dr["MaxSlot"] = Convert.ToInt32(addmin + 30);
                        }
                        Fdate = Tdate;

                    }
                    dttemp.Rows.Add(dr);
                }
                dttemp.AcceptChanges();
                Fqry += addQry + " from (";
                for (int p = 0; p <= loops; p++)
                {
                    Fqry += "select '" + txtSearch.Text + "' as Person,  convert(varchar(12),vdate,113) as Date,";
                    for (int k = 0; k < dttemp.Rows.Count; k++)
                    {
                        if (k == p)
                        {
                            Fqry += " description as '" + dttemp.Rows[k]["Slot"] + "',";
                        }
                        else
                        {
                            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
                        }

                    }
                    Fqry = Fqry.Substring(0, Fqry.Length - 1);
                    Fqry += " from locationdetails where deviceno='" + SetObj.DeviceNo + "' and   cast(CurrentDate as DateTime) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as DateTime)" +
                     " and cast(CurrentDate as Time) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as Time) AND cast(CurrentDate as Time) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as Time)" +
                        " and slot between " + dttemp.Rows[p]["MinSlot"] + " and " + dttemp.Rows[p]["MaxSlot"] + " and log_m in('G')";
                    Fqry += " union ";
                }
                for (int p = 0; p <= loops; p++)
                {
                    Fqry += "select '" + txtSearch.Text + "' as Person,  convert(varchar(12),vdate,113) as Date,";
                    for (int k = 0; k < dttemp.Rows.Count; k++)
                    {
                        if (k == p)
                        {
                            Fqry += "'*'+ description as '" + dttemp.Rows[k]["Slot"] + "',";
                        }
                        else
                        {
                            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
                        }

                    }
                    Fqry = Fqry.Substring(0, Fqry.Length - 1);
                    Fqry += " from locationdetails where deviceno='" + SetObj.DeviceNo + "' and   cast(CurrentDate as DateTime) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as DateTime)" +
                      " and cast(CurrentDate as Time) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as Time) AND cast(CurrentDate as Time) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as Time)" +
                        " and slot between " + dttemp.Rows[p]["MinSlot"] + " and " + dttemp.Rows[p]["MaxSlot"] + " and log_m in('C','N')";
                    Fqry += " union ";
                }
                for (int p = 0; p <= loops; p++)
                {
                    Fqry += "  select '" + txtSearch.Text + "' as Person,  convert(varchar(12),vdate,113) as Date,";
                    for (int k = 0; k < dttemp.Rows.Count; k++)
                    {
                        if (k == p)
                        {
                            Fqry += " case status when 'GO' then ' GPS Off' when 'LO' then ' Location Off' when 'MO' then ' Internet Problem' when 'M' then ' Datetime is set manually' when 'TN' then ' Time Not In Range' end as '" + dttemp.Rows[k]["Slot"] + "',";
                        }
                        else
                        {
                            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
                        }

                    }
                    Fqry = Fqry.Substring(0, Fqry.Length - 1);
                    Fqry += " from log_tracker where deviceno='" + SetObj.DeviceNo + "' and   cast(CurrentDate as DateTime) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as DateTime)" +
                       " and cast(CurrentDate as Time) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as Time) AND cast(CurrentDate as Time) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as Time)" +
                        " and slot between " + dttemp.Rows[p]["MinSlot"] + " and " + dttemp.Rows[p]["MaxSlot"] + "";
                    Fqry += " union ";
                }



                for (int p = 0; p <= loops; p++)
                {
                    Fqry += "  select '" + txtSearch.Text + "' as Person,  convert(varchar(12),vdate,113) as Date,";
                    for (int k = 0; k < dttemp.Rows.Count; k++)
                    {
                        if (k == p)
                        {
                            Fqry += " case status when 'LV' then 'On Leave' end as '" + dttemp.Rows[k]["Slot"] + "',";
                        }
                        else
                        {
                            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
                        }

                    }
                    Fqry = Fqry.Substring(0, Fqry.Length - 1);
                    Fqry += " from log_tracker where deviceno='" + SetObj.DeviceNo + "' and   cast(CurrentDate as date) >=cast('" + TextBox3.Text + "' as Date) AND cast(CurrentDate as date) < = cast('" + TextBox5.Text + " ' as date) and status='LV'";
                    Fqry += " union ";
                }




                for (int p = 0; p <= loops; p++)
                {
                    Fqry += "  select  '" + txtSearch.Text + "' as Person,  convert(varchar(12),vdate,113) as Date,";
                    for (int k = 0; k < dttemp.Rows.Count; k++)
                    {
                        if (k == p)
                        {
                            Fqry += "' Application /Mobile Off' as '" + dttemp.Rows[k]["Slot"] + "',";
                        }
                        else
                        {
                            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
                        }

                    }
                    Fqry = Fqry.Substring(0, Fqry.Length - 1);
                    Fqry += " from log_tracker where deviceno='" + SetObj.DeviceNo + "' and   cast(CurrentDate as DateTime) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as DateTime) and " +
                           " cast(CurrentDate as Time) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as Time) AND cast(CurrentDate as Time) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as Time)" +
                        " and cast(rtrim(cast(vdate as varchar(15)))+' '+case when len(cast(cast((" + dttemp.Rows[p]["MinSlot"] + "/60) as int) as varchar(2)))=1 then '0' else '' end +cast(cast((" + dttemp.Rows[p]["MinSlot"] + "/60) as int) as varchar(2))+':'" +
                        " +cast(cast(" + dttemp.Rows[p]["MinSlot"] + "-cast((" + dttemp.Rows[p]["MinSlot"] + "/60) as int)*60 as int) as varchar(2)) as datetime) " +
                        " between fromtime and totime and status='AO'";
                    if (p <= loops - 1) Fqry += " union ";
                }


                //            select top 1 @result='Application /Mobile Off' from Log_Tracker where deviceno=@deviceno and 
                //cast(rtrim(cast(@date as varchar(15)))+' '+case when len(cast(cast((@Min/60) as int) as varchar(2)))=1 then '0' else '' end +cast(cast((@Min/60) as int) as varchar(2))+':'
                //+cast(cast(@Min-cast((@Min/60) as int)*60 as int) as varchar(2)) as datetime) 
                //between fromtime and totime
                //and status='AO'

                Fqry += ") a group by person,date order by person,cast(date as date)";

                // addQry += " from locationdetails where deviceno='" + SetObj.DeviceNo + "' and " +
                //             "  cast(CurrentDate as DateTime) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as DateTime)" +
                //         " group by deviceno,vdate" +
                //         " Union ";
                // addQry += Fqry+" from Log_Tracker where  vDate not IN (SELECT vDate FROM LocationDetails WHERE "+
                //" deviceno='" + SetObj.DeviceNo + "' and  cast(CurrentDate as DateTime) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as DateTime))" +
                // " and deviceno='" + SetObj.DeviceNo + "' and " +
                //             "  cast(CurrentDate as DateTime) >=cast('" + TextBox3.Text + " " + TextBox4.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + TextBox5.Text + " " + TextBox6.Text + "' as DateTime)" +
                //         " group by deviceno,vdate) a ";
                // addQry += "order by cast(a.Date as date)";




                // order by cast(vDate as date) ,deviceno "
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, Fqry);
                if (dt.Rows.Count > 0)
                {

                    for (int m = 0; m < dt.Rows.Count; m++)
                    {
                        for (int n = 0; n < dt.Columns.Count; n++)
                        {
                            if (string.IsNullOrEmpty(dt.Rows[m][n].ToString()) || dt.Rows[m][n].ToString().Length < 3)
                            {

                                string sql = "select '00:00' as Time,convert(varchar(18),vdate,103) as Cdate,'0' Battery,'0' as Accuracy,concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,'0'  as Signal,case when status='LV' Then PersonMaster.personname+' Is On Leave ' +convert(varchar(18),vdate,103) else '' end [address],'0' latitude,'0' longitude from PersonMaster inner join log_tracker on log_tracker.DeviceNo=PersonMaster.DeviceNo WHERE PersonMaster.ID=(select top 1 id from personmaster where deviceno='" + SetObj.DeviceNo + "' order by id desc) And  cast(log_tracker.vdate as date) >=cast('" + dt.Rows[m]["Date"].ToString() + "'  as date) AND cast(log_tracker.vdate as date) < = cast('" + dt.Rows[m]["Date"].ToString() + "' as date)  and Log_Tracker.Status='LV' ";

                                DataTable dtsql = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                                if (dtsql.Rows.Count > 0)
                                {
                                    dt.Rows[m][n] = "On leave";
                                }
                                else
                                {
                                    dt.Rows[m][n] = "No Network / Data";

                                }

                            }
                        }
                    }
                    gdw.DataSource = dt; gdw.DataBind(); btnexport.Visible = true;
                }
                else
                {
                    //DataRow []dr=dt.Select(


                    gdw.DataSource = null;
                    gdw.DataBind(); btnexport.Visible = false;
                    ShowAlert("No Records Found !!");
                }

            }
            catch (Exception ex) { ex.ToString(); }
            dt.Clear();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                GetData();
            }

            catch (Exception)
            {
                ShowAlert("There are some problems while loading records!");
            }
        }
        private void BindPerson()
        {
            string str = "select concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )'  as Person,PersonMaster.ID as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + DropDownList1.SelectedValue + "' order by PersonMaster.PersonName";
            // DataAccessLayer.DAL.fillDDLDirect(DropDownList3, str, "Id", "Person", 1);
            //DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            //DataView dv = new DataView(dt);
            //dv.RowFilter = "SMId<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            //ddlperson.DataSource = dv.ToTable();
            //ddlperson.DataTextField = "SMName";
            //ddlperson.DataValueField = "deviceNo";
            //ddlperson.DataBind();
            ////Add Default Item in the DropDownList
            //ddlperson.Items.Insert(0, new ListItem("--Please select--"));
            

        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewExportUtil.Export("MovementSummaryPersonWise", gdw);
            }
            catch (Exception ex)
            {
                ShowAlert("There are some errors while loading records!");
            }
        }
        protected void gdw_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToDateTime(e.Row.Cells[1].Text).DayOfWeek.ToString() == "Sunday")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;//FromArgb(196, 242, 205);// "#C4F2CD";
                }
                if (e.Row.Cells[2].Text == "On Leave")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;//FromArgb(196, 242, 205);// "#C4F2CD";
                }
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.ToString() + "</br>" + "(" + Convert.ToDateTime(e.Row.Cells[1].Text).DayOfWeek.ToString() + ")";

            }
        }
        protected void gdw_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdw.PageIndex = e.NewPageIndex;
            GetData();
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {
            List<string> customers = new List<string>();
            //string SummaryPersonWiseGroupid = Convert.ToString(HttpContext.Current.Session["SummaryPersonWiseGroupid"]);
            //if (!string.IsNullOrEmpty(SummaryPersonWiseGroupid))
            {
                string str = "select distinct PersonMaster.ID as id, concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )'  as Person from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where   personmaster.personname like '%" + prefixText + "%' order by PersonMaster.id";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Person"].ToString(), dt.Rows[i]["id"].ToString());
                    //string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["RetailerName"].ToString() + ")" + " - " + dt.Rows[i]["Area"].ToString() + " " + "(" + dt.Rows[i]["MobileNo"].ToString() + ")", dt.Rows[i]["MobileNo"].ToString());
                    customers.Add(item);
                }
            }
            return customers;
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {

            SetObj.PersonID = hfpersonid.Value;
            SetObj.Accuracy = cm.GetAccuracyByPerson(hfpersonid.Value);
            SetObj.DeviceNo = cm.GetDeviceNoByPersonID(hfpersonid.Value);
        }
    }
}