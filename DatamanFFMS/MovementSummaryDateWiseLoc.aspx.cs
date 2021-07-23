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
using BAL;
public partial class MovementSummaryDateWiseLoc : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    Settings SetObj = Settings.Instance;
    Common cm = new Common();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(this).RegisterPostBackControl(Button5);
        if (!IsPostBack)
        {
            txtfromdate.Text = DateTime.Now.ToUniversalTime().AddSeconds(Convert.ToInt32(19800)).ToString("dd-MMM-yyyy");

            GetGroupName();
            txtfromtime.Text = "09:00";
            txttotime.Text = "18:00";
            //   Button4_Click(sender, e);
            BindPerson();
            txtaccu.Value = SetObj.Accuracy;
        }
        if (SetObj.GroupID != "0" && SetObj.GroupID != "")
        {

            if (!IsPostBack)
            {
                ddlgroup.SelectedValue = SetObj.GroupID;
                BindPerson();
            }
        }
        else
        {
            SetObj.GroupID = ddlgroup.SelectedValue;
        }
    }
    private void GetGroupName()
    {
        string str = "select GroupMaster.GroupID as Code, GroupMaster.Description as Description from UserGrp inner Join GroupMaster on GroupMaster.GroupID = UserGrp.GroupId where UserGrp.UserID = '" + Settings.Instance.UserID + "' order by GroupMaster.Description";
        fillDDLDirect(ddlgroup, str, "Code", "Description", 1);
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewExportUtil.Export("MovementSummaryDateWiseLocation", gdw);
        }
        catch (Exception ex)
        {
            ShowAlert("There are some errors while loading records!");
        }
    }
    public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
    {
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
            xddl.Items.Insert(0, new ListItem("--Others--", "0"));
        }
        xdt.Dispose();
    }
    protected void ddlgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetObj.GroupID = ddlgroup.SelectedValue;
        BindPerson();
    }
    protected void btnSave_Click(object sender, EventArgs e)
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
    protected void gdw_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToDateTime(e.Row.Cells[1].Text).DayOfWeek.ToString() == "Sunday")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;//FromArgb(196, 242, 205);// "#C4F2CD";
            }
            e.Row.Cells[1].Text = e.Row.Cells[1].Text.ToString() + "</br>" + "(" + Convert.ToDateTime(e.Row.Cells[1].Text).DayOfWeek.ToString() + ")";
        }
    }
    private void BindPerson()
    {
        //string str = "select PersonMaster.PersonName as Person,PersonMaster.deviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlgroup.SelectedValue + "' order by PersonMaster.PersonName";
        string str = "select concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )'  as Person,PersonMaster.deviceNo as id from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + ddlgroup.SelectedValue + "' order by PersonMaster.PersonName";
        DataTable dt = new DataTable();
        dt =DbConnectionDAL.getFromDataTable(str);
        ddlperson.DataSource = dt;
        ddlperson.DataTextField = "Person";
        ddlperson.DataValueField = "ID";
        ddlperson.DataBind();
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
    private void GetData()
    {
        DataTable dt = new DataTable();
        try
        {
            //if (string.IsNullOrEmpty(SetObj.DeviceNo) || SetObj.DeviceNo == "0")
            //{
            //    ShowAlert("Please Select the Person again");
            //    return;
            //}
            string strloc = "";
            if (ddlloc.SelectedValue == "C")
                strloc = " and Log_m in ('C','N')";
            else strloc = " and Log_m='" + ddlloc.SelectedValue + "'";
            string addDeviceNo = "";
            foreach (ListItem item in ddlperson.Items)
            {
                if (ddlperson.SelectedIndex < 0)
                {
                    addDeviceNo += item.Value + "," + "";
                }
                else
                {
                    if (item.Selected)
                    {
                        addDeviceNo += item.Value + "," + "";
                    }
                }
            }
            if (string.IsNullOrEmpty(addDeviceNo))
            {
                ShowAlert("Please Select atleast one Person");
                gdw.DataSource = null; gdw.DataBind();
                return;
            }
            addDeviceNo = addDeviceNo.Substring(0, addDeviceNo.Length - 1);
            DataTable dttemp = new DataTable();
            dttemp.Columns.Add("Slot"); dttemp.Columns.Add("MinSlot");
            dttemp.Columns.Add("MaxSlot");
            DateTime Fdate = DateTime.MinValue;
            DateTime Tdate = DateTime.MinValue;
            string frmtm = txtfromtime.Text.Replace(":", ".");
            string totm = txttotime.Text.Replace(":", ".");
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
            string hrs = txtfromtime.Text.ToString().Replace(":", ".");
            string[] spthrs = hrs.Split('.');
            if (spthrs.Length > 1)
                addmin = (Convert.ToInt32(spthrs[0]) * 60) + (Convert.ToInt32(spthrs[1]));
            else
                addmin = (Convert.ToInt32(spthrs[0]) * 60);

            string Tohrs = txttotime.Text.ToString().Replace(":", ".");
            string[] sptTohrs = Tohrs.Split('.');
            if (sptTohrs.Length > 1)
                Tomin = (Convert.ToInt32(sptTohrs[0]) * 60) + (Convert.ToInt32(sptTohrs[1]));
            else
                Tomin = (Convert.ToInt32(sptTohrs[0]) * 60);

            string mdate = Convert.ToDateTime(txtfromdate.Text).ToShortDateString();
            for (int i = 0; i <= loops; i++)
            {
                DataRow dr = dttemp.NewRow();
                if (i == 0)
                {

                    Fdate = Convert.ToDateTime(txtfromdate.Text + " " + txtfromtime.Text);
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
                //Fqry += "select Personname as Person,  convert(varchar(12),vdate,113) as Date,";
                Fqry += "select concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,  convert(varchar(12),vdate,113) as Date,";
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
                Fqry += " from locationdetails RIGHT JOIN PersonMaster ON LocationDetails.DeviceNo=PersonMaster.DeviceNo where locationdetails.deviceno in (" + addDeviceNo + ") and   cast(CurrentDate as DateTime) >=cast('" + txtfromdate.Text + " " + txtfromtime.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + txtfromdate.Text + " " + txttotime.Text + "' as DateTime)" +
                    " and slot between " + dttemp.Rows[p]["MinSlot"] + " and " + dttemp.Rows[p]["MaxSlot"] + " " + strloc + "";
                if (p <= loops - 1) Fqry += " union ";
                //  Fqry += " union ";
            }
            //for (int p = 0; p <= loops; p++)
            //{
            //    //Fqry += "select Personname as Person,  convert(varchar(12),vdate,113) as Date,";
            //    Fqry += "select concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,  convert(varchar(12),vdate,113) as Date,";
            //    for (int k = 0; k < dttemp.Rows.Count; k++)
            //    {
            //        if (k == p)
            //        {
            //            Fqry += "'*'+ description as '" + dttemp.Rows[k]["Slot"] + "',";
            //        }
            //        else
            //        {
            //            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
            //        }

            //    }
            //    Fqry = Fqry.Substring(0, Fqry.Length - 1);
            //    Fqry += " from locationdetails RIGHT JOIN PersonMaster ON LocationDetails.DeviceNo=PersonMaster.DeviceNo where locationdetails.deviceno in (" + addDeviceNo + ") and   cast(CurrentDate as DateTime) >=cast('" + txtfromdate.Text + " " + txtfromtime.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + txtfromdate.Text + " " + txttotime.Text + "' as DateTime)" +
            //       " and slot between " + dttemp.Rows[p]["MinSlot"] + " and " + dttemp.Rows[p]["MaxSlot"] + " and log_m in('C','N')";
            //    Fqry += " union ";
            //}
            //for (int p = 0; p <= loops; p++)
            //{
            //    //Fqry += "select Personname as Person,  convert(varchar(12),vdate,113) as Date,";
            //    Fqry += "select concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,  convert(varchar(12),vdate,113) as Date,";
            //    for (int k = 0; k < dttemp.Rows.Count; k++)
            //    {
            //        if (k == p)
            //        {
            //            Fqry += " case status when 'GO' then ' GPS Off' when 'MO' then ' Internet Problem' when 'TN' then ' Tower not in reach' end as '" + dttemp.Rows[k]["Slot"] + "',";
            //        }
            //        else
            //        {
            //            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
            //        }

            //    }
            //    Fqry = Fqry.Substring(0, Fqry.Length - 1);
            //    Fqry += " from log_tracker RIGHT JOIN PersonMaster ON log_tracker.DeviceNo=PersonMaster.DeviceNo  " +
            //        " where log_tracker.deviceno in (" + addDeviceNo + ") and   cast(CurrentDate as DateTime) >=cast('" + txtfromdate.Text + " " + txtfromtime.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + txtfromdate.Text + " " + txttotime.Text + "' as DateTime)" +
            //        " and slot between " + dttemp.Rows[p]["MinSlot"] + " and " + dttemp.Rows[p]["MaxSlot"] + "";
            //    Fqry += " union ";
            //}

            //for (int p = 0; p <= loops; p++)
            //{
            //    //Fqry += "select Personname as Person,  convert(varchar(12),vdate,113) as Date,";
            //    Fqry += "select concat(PersonMaster.PersonName, ' (' + PersonMaster.empcode ) + ' )' as Person,  convert(varchar(12),vdate,113) as Date,";
            //    for (int k = 0; k < dttemp.Rows.Count; k++)
            //    {
            //        if (k == p)
            //        {
            //            Fqry += "' Application /Mobile Off' as '" + dttemp.Rows[k]["Slot"] + "',";
            //        }
            //        else
            //        {
            //            Fqry += "' ' as '" + dttemp.Rows[k]["Slot"] + "',";
            //        }

            //    }
            //    Fqry = Fqry.Substring(0, Fqry.Length - 1);
            //    Fqry += " from log_tracker RIGHT JOIN PersonMaster ON log_tracker.DeviceNo=PersonMaster.DeviceNo where log_tracker.deviceno in (" + addDeviceNo + ") and  cast(CurrentDate as DateTime) >=cast('" + txtfromdate.Text + " " + txtfromtime.Text + "' as DateTime) AND cast(CurrentDate as DateTime) < = cast('" + txtfromdate.Text + " " + txttotime.Text + "' as DateTime)" +
            //        " and cast(rtrim(cast(vdate as varchar(15)))+' '+case when len(cast(cast((" + dttemp.Rows[p]["MinSlot"] + "/60) as int) as varchar(2)))=1 then '0' else '' end +cast(cast((" + dttemp.Rows[p]["MinSlot"] + "/60) as int) as varchar(2))+':'" +
            //        " +cast(cast(" + dttemp.Rows[p]["MinSlot"] + "-cast((" + dttemp.Rows[p]["MinSlot"] + "/60) as int)*60 as int) as varchar(2)) as datetime) " +
            //        " between log_tracker.fromtime and log_tracker.totime and status='AO'";
            //    if (p <= loops - 1) Fqry += " union ";
            //}


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
            dt = DbConnectionDAL.getFromDataTable(Fqry);
            if (dt.Rows.Count > 0)
            {

                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    for (int n = 0; n < dt.Columns.Count; n++)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[m][n].ToString()) || dt.Rows[m][n].ToString().Length < 3)
                        {
                            dt.Rows[m][n] = "No Network / Data";
                        }
                    }
                }
                gdw.DataSource = dt; gdw.DataBind(); Button5.Visible = true;
            }
            else
            {
                //DataRow []dr=dt.Select(


                gdw.DataSource = null;
                gdw.DataBind(); Button5.Visible = false;
                ShowAlert("No Records Found !!");
            }

        }
        catch (Exception ex) { ex.ToString(); }
        dt.Clear();
    }
    public void FillData(object sender, EventArgs e)
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
}