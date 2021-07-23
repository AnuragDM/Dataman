using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using DAL;
using System.IO;

namespace AstralFFMS
{
    public partial class DSREntryFormLevel2 : System.Web.UI.Page
    {
         int msg = 0;
         int uid = 0;
         int smID = 0;
         int dsrDays = 0;
        string VisitID = "0";

        BAL.DSRLevel1BAL dp = new DSRLevel1BAL();
        string parameter = "";
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            parameter = Request["__EVENTARGUMENT"];
            txtVisitDate.Attributes.Add("readonly", "readonly");
            txtmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            txtNextVisitDate.Attributes.Add("readonly", "readonly");

            if (parameter != "")
            {
                ViewState["VisId"] = parameter;

                divdocid.Visible = false;
                Settings.Instance.VistID = Convert.ToString(ViewState["VisId"]);
                CalendarExtender3.StartDate = Settings.GetUTCTime().AddDays(1); //DateTime.Now;
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");

                LockedValues(Settings.GetVisitLocked(Convert.ToInt32(ViewState["VisId"])));
                RejectedValues(Settings.GetVisitRejected(Convert.ToInt32(ViewState["VisId"])));
                if (Settings.Instance.RoleType == "Admin")
                {
                    btnUnlock.Visible = true;
                    btnUnlock.CssClass = "btn btn-primary";
                    btnUnlock.Enabled = true;
                    btnUnlock.CssClass = "btn btn-primary";
                }
            }
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            //Ankita - 10/may/2016- (For Optimization)
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
              //  btnNext.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnNext.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnNext.CssClass = "btn btn-primary";
            }
            else if (btnSave.Text == "Update")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            int userid = Settings.DMInt32(Settings.Instance.UserID);
            if (userid == 0)
            { Response.Redirect("~/LogIn.aspx", true); }

            if (!IsPostBack)
            {
                basicExample.Value = "09:30am";
                basicExample1.Value = "06:30pm";
                //Added
                Settings.Instance.BindTimeToDDL(basicExampleDDL);
                Settings.Instance.BindTimeToDDL(basicExample1DDL);
                roleType = Settings.Instance.RoleType;
                //  GetRoleType(Settings.Instance.RoleID);
                //Added
                basicExampleDDL.SelectedValue = "09:30";
                basicExample1DDL.SelectedValue = "18:30";
                //End
                //End
                txtVisitDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                CalendarExtender1.EndDate = Settings.GetUTCTime();
                // fillIndustry();
                FillClass();
                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                txtNextVisitDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).AddDays(7.00).ToShortDateString()).ToString("dd/MMM/yyyy");

                #region Show Planned Beat
                string strPB = @"SELECT T2.AreaName FROM TransBeatPlan AS T1 LEFT JOIN MastArea AS T2
                                        ON T1.BeatId=T2.AreaId WHERE T1.PlannedDate='" + Convert.ToString(txtVisitDate.Text) + "' and T1.SMId=" + Convert.ToInt32(Settings.Instance.SMID);
                DataTable dtPB = DbConnectionDAL.GetDataTable(CommandType.Text, strPB);

                if (dtPB != null && dtPB.Rows.Count > 0)
                {
                    lblPlanedbeat.Text = Convert.ToString(dtPB.Rows[0]["AreaName"]);
                }
                #endregion

                if (d.Rows.Count > 0)
                {
                    try
                    {
                        DataView dv = new DataView(d);
                        dv.RowFilter = "RoleType='CityHead' OR RoleType='DistrictHead'";
                        ddlUndeUser.DataSource = dv;
                        ddlUndeUser.DataTextField = "SMName";
                        ddlUndeUser.DataValueField = "SMId";
                        ddlUndeUser.DataBind();

                        ddlUser1.DataSource = dv;
                        ddlUser1.DataTextField = "SMName";
                        ddlUser1.DataValueField = "SMId";
                        ddlUser1.DataBind();
                    }
                    catch { }
                    DIVUnder.Visible = true;
                }
                if (Request.QueryString["VisId"] != null)
                {
                    FillDeptControls(Convert.ToInt32(Request.QueryString["VisId"]));
                }

                mainDiv.Style.Add("display", "block");
                try
                {
                    BindWithSalesPersonDDl(Convert.ToInt32(ddlUndeUser.SelectedValue));
                }
                catch { }
                BindDDlCity();

                if (Request.QueryString["VisitID"] != null)
                {
                    try
                    {
                        ViewState["VisId"] = Request.QueryString["VisitID"];
                        FillDeptControls(Convert.ToInt32(Request.QueryString["VisitID"].ToString()));
                        LockedValues(Settings.GetVisitLocked(Convert.ToInt32(ViewState["VisId"])));
                        RejectedValues(Settings.GetVisitRejected(Convert.ToInt32(ViewState["VisId"])));
                    }
                    catch
                    {

                    }
                }
            }
            CalendarExtender1.StartDate = DateTime.Now.AddDays(-Settings.GetDsrDays(Convert.ToInt32(ddlUndeUser.SelectedValue)));
        }
        private void FillClass()
        {//Ankita - 10/may/2016- (For Optimization)
         //  string str = @"select * from MastItemClass order by Name";
            string str = @"select Id,Name from MastItemClass order by Name";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ddlmaterialclass.DataSource = deptValueDt;
                ddlmaterialclass.DataTextField = "Name";
                ddlmaterialclass.DataValueField = "Id";
                ddlmaterialclass.DataBind();
            }
            //  ddlmaterialclass.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        protected int DSRExported(int visid)
        {
            int Exported = 0;
            if (visid == 0)
            {

            }
            else
            {
                string Query = "Select isnull(ExportedToAndroid,0) from transvisit where visid=" + visid + "";
                Exported = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, Query).ToString());
            }

            return Exported;
        }
        //private void GetRoleType(string p)
        //{
        //    try
        //    {
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        private void LockedValues(bool Lock)
        {
            if (Lock)
            {
                //btnSave.Text = "Locked";
                //btnSave.Enabled = false;

                btnSave.Text = "Locked";
                btnSave.Enabled = false;
                btnLock.Enabled = false;
                btnLock.CssClass = "btn btn-primary";               
                //ddlcity.Enabled = false;              

                //Added
                basicExample.Disabled = true;
                basicExample1.Disabled = true;
                //Added
                basicExampleDDL.Enabled = false;
                basicExample1DDL.Enabled = false;
                //End
                ddlmodeoftransport.Enabled = false;
                ddlvehicle.Enabled = false;
                ddlWith.Enabled = false;
                ddlnextwith.Enabled = false;
                ddlnextcity.Enabled = false;
                txtNextVisitDate.Enabled = false;
                this.ddlmaterialclass.Attributes.Add("disabled", "");
                //ddlmaterialclass.Enabled = false;
                txtRemarks.ReadOnly = true;
                //End

            }
            else
            {
                //  btnSave.Enabled = true;
                btnLock.Enabled = true;
                //Added
                //ddlcity.Enabled = true;               
                basicExample.Disabled = false;
                basicExample1.Disabled = false;
                //Added
                basicExampleDDL.Enabled = true;
                basicExample1DDL.Enabled = true;
                //End
                ddlmodeoftransport.Enabled = true;
                ddlvehicle.Enabled = true;
                ddlWith.Enabled = true;
                ddlnextwith.Enabled = true;
                ddlnextcity.Enabled = true;
                txtNextVisitDate.Enabled = true;
                this.ddlmaterialclass.Attributes.Remove("disabled");
                //     ddlmaterialclass.Enabled = true;
                txtRemarks.ReadOnly = false;
                //End
            }
        }
        private void RejectedValues(string Lock)
        {
            if (Lock == "Reject")
            {
                btnUnlock.Visible = true;
                btnUnlock.CssClass = "btn btn-primary";
                btnUnlock.Enabled = true;
                btnUnlock.CssClass = "btn btn-primary";

            }
            else
            {
                btnUnlock.Visible = false;
                btnUnlock.CssClass = "btn btn-primary";
            }
        }


        private void fillRepeter()
        {

            string str = @"select * from TransVisit where SMId=" + ddlUndeUser.SelectedValue;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
                rpt.DataBind();
            }
        }

        private void BindWithSalesPersonDDl(int smID)
        {
            //string query = "select * from MastSalesRep where smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ") and SMName<>'.'  and smid<>" + smID + " order by SMName";
            //Ankita - 10/may/2016- (For Optimization)
            //string query = "select * from MastSalesRep where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) and SMName<>'.'  and smid<>" + smID + " order by SMName";
            string query = "select SMId,SMName from MastSalesRep where (smid in (select maingrp from MastSalesRepGrp where smid=" + smID + ")  or smid in (select smid from MastSalesRepGrp where maingrp=" + smID + ") ) and SMName<>'.'  and smid<>" + smID + " order by SMName";
            DataTable envobj1 = new DataTable();
            envobj1 = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (envobj1.Rows.Count > 0)
            {
                ddlWith.DataSource = envobj1;
                ddlWith.DataTextField = "SMName";
                ddlWith.DataValueField = "SMId";
                ddlWith.DataBind();

                ddlnextwith.DataSource = envobj1;
                ddlnextwith.DataTextField = "SMName";
                ddlnextwith.DataValueField = "SMId";
                ddlnextwith.DataBind();

            }
            ddlnextwith.Items.Insert(0, new ListItem("-- Select --", "0"));
            ddlWith.Items.Insert(0, new ListItem("-- Select --", "0"));
            //}
        }
        private void BindDDlCity()
        {//Ankita - 10/may/2016- (For Optimization)
            ddlcity.Items.Clear();
//            string str = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
//                          and PrimCode=" + ddlUndeUser.SelectedValue + ")) and  areatype='city' and Active=1 order by AreaName";
            string str = @"select AreaId,AreaName from mastarea where areaid in (select Distinct underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
                          and PrimCode=" + ddlUndeUser.SelectedValue + ") AND Active=1) and  areatype='city' and Active=1 order by AreaName";
            //            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlUndeUser.SelectedValue + ")) and areatype='City' and Active=1 order by AreaName ";


            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                try
                {
                    ddlcity.DataSource = obj;
                    ddlcity.DataTextField = "AreaName";
                    ddlcity.DataValueField = "AreaId";
                    ddlcity.DataBind();


                    ddlnextcity.DataSource = obj;
                    ddlnextcity.DataTextField = "AreaName";
                    ddlnextcity.DataValueField = "AreaId";
                    ddlnextcity.DataBind();
                }
                catch
                {

                }

            }
            ddlcity.Items.Add(new ListItem("OTHER CITY", "-1"));
            ddlnextcity.Items.Add(new ListItem("OTHER CITY", "-1"));
            //ddlcity.Items.Insert(0, new ListItem("-- Select --", "0"));
            //ddlnextcity.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private string FillDistributorByID(int Partyid)
        {
            string str = "select PartyName from MastParty where PartyId=" + Partyid;
            return Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
        }

        //private void fillIndustry()
        //{
        //    string str = "select * from MastIndustry";
        //    DataTable obj = new DataTable();
        //    obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
        //    if(obj.Rows.Count>0)
        //    {
        //        chkIndustry.DataSource = obj;
        //        chkIndustry.DataTextField = "IndName";
        //        chkIndustry.DataValueField = "IndId";
        //        chkIndustry.DataBind();
        //    }
        //    else
        //    {

        //    }

        //}
        private void BindDDlCity(int salesRepId)
        {
            ddlcity.Items.Clear();
            string str = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
                          and PrimCode=" + salesRepId + ") AND Active=1) and  areatype='city' and Active=1 order by AreaName";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                try
                {
                    ddlcity.DataSource = obj;
                    ddlcity.DataTextField = "AreaName";
                    ddlcity.DataValueField = "AreaId";
                    ddlcity.DataBind();


                    ddlnextcity.DataSource = obj;
                    ddlnextcity.DataTextField = "AreaName";
                    ddlnextcity.DataValueField = "AreaId";
                    ddlnextcity.DataBind();
                }
                catch
                {

                }
            }
            ddlcity.Items.Add(new ListItem("OTHER CITY", "-1"));
        }

        private void FillDeptControls(int VisId)
        {
            try
            {
                string str = @"select * from TransVisit  where VisId=" + VisId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                string strcitydisable = @"select underid as City from Temp_TransOrder om left join mastarea ma on om.areaid=ma.areaid where VisId=" + VisId +
                " union select underid as City from Temp_TransDemo  om left join mastarea ma on om.areaid=ma.areaid where VisId=" + VisId +
                " union select om.AreaId as City from Temp_TransFailedVisit om left join mastparty mp on om.Partyid=mp.partyid where mp.partydist=1 and VisId=" + VisId +
                " union select om.Cityid as City from Temp_transvisitdist om left join mastparty mp on om.distid=mp.partyid where mp.partydist=1 and VisId=" + VisId +
                " union select ma.underid as City from Temp_TransFailedVisit om left join mastparty mp on om.Partyid=mp.partyid left join mastarea ma on om.areaid=ma.areaid where mp.partydist=0 and VisId=" + VisId +
                " union select ma.underid as City from Temp_TransCompetitor om left join mastparty mp on om.Partyid=mp.partyid left join mastarea ma on mp.areaid=ma.areaid where mp.partydist=0 and VisId=" + VisId;
                DataTable dtcity = DbConnectionDAL.GetDataTable(CommandType.Text, strcitydisable);
                if (deptValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";
                    txtVisitDate.ReadOnly = true;
                    txtVisitDate.Enabled = false;
                    divdocid.Visible = true;
                    //           ddlcity.Enabled = false;
                    lbldocno.Text = deptValueDt.Rows[0]["VisitDocId"].ToString();
                    try
                    {
                        ddlUndeUser.SelectedValue = deptValueDt.Rows[0]["SMId"].ToString();
                    }
                    catch { }

                    txtVisitDate.Text = DateTime.Parse(deptValueDt.Rows[0]["VDate"].ToString()).ToString("dd/MMM/yyyy");
                    BindWithSalesPersonDDl(Convert.ToInt32(deptValueDt.Rows[0]["SMId"]));
                    ddlWith.SelectedValue = deptValueDt.Rows[0]["WithUserId"].ToString();
                    ddlnextwith.SelectedValue = deptValueDt.Rows[0]["nWithUserId"].ToString();
                    ddlmodeoftransport.SelectedValue = deptValueDt.Rows[0]["ModeOfTransport"].ToString();
                    ddlvehicle.SelectedValue = deptValueDt.Rows[0]["VehicleUsed"].ToString();
                    //Added
                    if (deptValueDt.Rows[0]["SMId"].ToString() != "")
                    {
                        BindDDlCity(Convert.ToInt32(deptValueDt.Rows[0]["SMId"]));
                    }
                    //End

                    if (deptValueDt.Rows[0]["CityIdS"].ToString() == "0")
                    {
                        ddlcity.SelectedValue = "-1";
                        ddlcity.Enabled = true;
                    }
                    //else
                    //{
                    //    ddlcity.SelectedValue = deptValueDt.Rows[0]["CityIdS"].ToString();
                    //    ddlcity.Enabled = false;
                    //}
                    else
                    {
                        try
                        {
                            string a = string.Empty;
                            ddlcity.ClearSelection();
                            string[] CityAll = new string[50];
                            string CityStaff = deptValueDt.Rows[0]["CityIdS"].ToString();
                            CityAll = CityStaff.Split(',');
                            if (CityAll.Length > 0)
                            {
                                foreach (ListItem item in ddlcity.Items)
                                {
                                    for (int i = 0; i < CityAll.Length; i++)
                                    {
                                        if (item.Value == CityAll[i].ToString())
                                            item.Selected = true;
                                            a += item.Value + ",";          
                                    }
                                }
                                hdncitylock.Value = a.TrimStart(',').TrimEnd(',');      
                            }

                        }
                        catch { }
                    }
                    if (dtcity.Rows.Count > 0)
                    {
                        try
                        {
                            hdnCityId.Value = "";
                            hdnCityname.Value = "";
                            string city = string.Empty, concatecity = string.Empty, concatecityStr1 = string.Empty, x = string.Empty, y = string.Empty;
                            string[] cityAll = new string[100];
                            for (int a = 0; a < dtcity.Rows.Count; a++)
                            {
                                city = dtcity.Rows[a]["City"].ToString();
                                concatecity += city + ",";

                            }
                            concatecityStr1 = concatecity.TrimStart(',').TrimEnd(',');
                            //string city = dtcity.Rows[0]["City"].ToString();
                            //cityAll = city.Split(',');
                            cityAll = concatecityStr1.Split(',');
                            if (cityAll.Length > 0)
                            {
                                foreach (ListItem itemCity in ddlcity.Items)
                                {
                                    for (int i = 0; i < cityAll.Length; i++)
                                    {
                                        if (itemCity.Value == cityAll[i].ToString())
                                        {
                                            //hdnCityId.Value += itemCity.Value + ",";
                                            //hdnCityId.Value += itemCity.Text + ",";
                                            x += itemCity.Value + ",";
                                            y += itemCity.Text + ",";
                                            itemCity.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                }
                                hdnCityId.Value = x.TrimStart(',').TrimEnd(',');
                                hdnCityname.Value = y.TrimStart(',').TrimEnd(',');
                            }
                        }
                        catch { }
                    }
                    if (deptValueDt.Rows[0]["nCityId"].ToString() == "0")
                    {
                        ddlnextcity.SelectedItem.Text = "OTHER CITY";
                    }
                    else
                    {
                        ddlnextcity.SelectedValue = deptValueDt.Rows[0]["nCityId"].ToString();
                    }

                    txtRemarks.Text = deptValueDt.Rows[0]["Remark"].ToString();
                    basicExample.Value = deptValueDt.Rows[0]["frTime1"].ToString();
                    basicExample1.Value = deptValueDt.Rows[0]["toTime1"].ToString();
                    //Added
                    basicExampleDDL.SelectedValue = deptValueDt.Rows[0]["frTime1"].ToString();
                    basicExample1DDL.SelectedValue = deptValueDt.Rows[0]["toTime1"].ToString();
                    if (deptValueDt.Rows[0]["OrderAmountMail"].ToString() != "")
                    {
                        txtAmountEmail.Text = deptValueDt.Rows[0]["OrderAmountMail"].ToString();
                    }
                    else { txtAmountEmail.Text = "0.00"; }
                    if (deptValueDt.Rows[0]["OrderAmountMail"].ToString() != "")
                    {
                        txtAmountPhone.Text = deptValueDt.Rows[0]["OrderAmountPhone"].ToString();
                    }
                    else { txtAmountPhone.Text = "0.00"; }
                    //End
                    try
                    {
                        ddlmaterialclass.ClearSelection();

                        string[] accStaffAll = new string[50];
                        string accStaff = deptValueDt.Rows[0]["Industry"].ToString();
                        accStaffAll = accStaff.Split(',');
                        if (accStaffAll.Length > 0)
                        {
                            foreach (ListItem item in ddlmaterialclass.Items)
                            {
                                for (int i = 0; i < accStaffAll.Length; i++)
                                {
                                    if (item.Value == accStaffAll[i].ToString())
                                        item.Selected = true;
                                }
                            }
                        }


                        //  ddlmaterialclass.Items.FindByText(deptValueDt.Rows[0]["Industry"].ToString()).Selected = true;
                    }
                    catch { }
                    ddlUndeUser.Enabled = false;
                    try
                    {
                        txtNextVisitDate.Text = DateTime.Parse(deptValueDt.Rows[0]["NextVisitDate"].ToString()).ToString("dd/MMM/yyyy");
                    }
                    catch { }

                    CalendarExtender3.StartDate = Convert.ToDateTime(txtVisitDate.Text).AddDays(1);

                    //ddlUndeUser.SelectedValue = deptValueDt.Rows[0]["SMId"].ToString();
                    // BindList(deptValueDt.Rows[0]["Industry"].ToString());
                    //  btnDelete.Visible = true;
                }
                else
                {
                    //  ddlUndeUser.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateRecord();
                }
                else
                {
                    InsertRecord();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        //private string  IND()
        //{
        //    string accDistStaffNew = "";
        //    foreach (ListItem item in chkIndustry.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            accDistStaffNew += item.Value + ",";
        //        }
        //    }
        //    return accDistStaffNew;
        //}

        private void InsertRecord()
        {
            if (Datecompare())
            {
                //Added
                //string fromtime = txtVisitDate.Text + " " + basicExample.Value;
                //string totime = txtVisitDate.Text + " " + basicExample1.Value;
                string fromtime = txtVisitDate.Text + " " + basicExampleDDL.SelectedValue;
                string totime = txtVisitDate.Text + " " + basicExample1DDL.SelectedValue;
                DateTime dsrdate = Convert.ToDateTime(txtVisitDate.Text);
                DateTime strdate = dsrdate.AddDays(-Settings.GetDsrDays(Settings.DMInt32(ddlUndeUser.SelectedValue)));

                if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('End time Should be greater than Start Time');", true);
                }//End
                else
                {
                    if (checkDateForUserExists(Convert.ToDateTime(txtVisitDate.Text)) == 0)
                    {
                        string docID = Settings.GetDocID("VISSN", DateTime.Now);
                        Settings.SetDocID("VISSN", docID);
                        string chdate = "";
                        if (txtVisitDate.Text != "")
                        {
                            chdate = Convert.ToDateTime(txtVisitDate.Text).ToShortDateString();
                        }
                        string Nextchdate = "";
                        if (txtNextVisitDate.Text != "")
                        {
                            Nextchdate = Convert.ToDateTime(txtNextVisitDate.Text).ToShortDateString();
                        }
                        if (checkDSRAllowdays(strdate) != 0)
                        { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('you can not enter new dsr because your previous date dsr entry not locked or contact crm admin.');", true); }
                        else
                        {
                            string CityStr1 = "", CityStrtext = "";
                            //foreach (ListItem item in ddlcity.Items)
                            //{
                            //    if (item.Selected)
                            //    {
                            //        CityStr1 += item.Value + ",";
                            //        CityStrtext += item.Text + ",";
                            //    }
                            //}
                            //CityStr1 = CityStr1.TrimStart(',').TrimEnd(',');
                            //CityStrtext = CityStrtext.TrimStart(',').TrimEnd(',');                           
                            foreach (ListItem item in ddlcity.Items)
                            {

                                if (item.Selected)
                                {
                                    CityStr1 += item.Value + ",";
                                    CityStrtext += item.Text + ",";
                                }
                            }

                            CityStr1 = CityStr1.TrimStart(',').TrimEnd(',');
                            if (hdnCityId.Value != "")
                            {
                                //if (CityStr1 == "" || CityStr1 == "-1")
                                if (CityStr1 == "")
                                { CityStr1 += hdnCityId.Value; }
                                else
                                { CityStr1 += "," + hdnCityId.Value; }
                            }
                            else { CityStr1 = CityStr1.TrimStart(',').TrimEnd(','); }

                            CityStrtext = CityStrtext.TrimStart(',').TrimEnd(',');
                            if (hdnCityname.Value != "")
                            {
                                //if (CityStrtext == "" || CityStrtext == "OTHER CITY")
                                if (CityStrtext == "")
                                { CityStrtext += hdnCityname.Value; }
                                else
                                { CityStrtext += "," + hdnCityname.Value; }

                            }
                            else
                            { CityStrtext = CityStrtext.TrimStart(',').TrimEnd(','); }
                            if (CityStr1 != "")
                            {
                                string smIDStr1 = "";
                                //         string message = "";
                                foreach (ListItem item in ddlmaterialclass.Items)
                                {
                                    if (item.Selected)
                                    {
                                        smIDStr1 += item.Value + ",";
                                    }
                                }
                                smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                                //int retsave = dp.Insert(docID, Convert.ToInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Convert.ToInt32(ddlUndeUser.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(0), Convert.ToInt32(ddlnextcity.SelectedValue), basicExample.Value, "", basicExample1.Value, "", Convert.ToInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, false, Convert.ToInt32(ddlnextwith.SelectedValue));
                                int cityValue = 0, nCityValue = 0;
                                if (CityStrtext != "OTHER CITY")
                                {
                                    cityValue = Convert.ToInt32(ddlcity.SelectedValue);
                                }
                                if (ddlnextcity.SelectedItem.Text != "OTHER CITY")
                                {
                                    nCityValue = Convert.ToInt32(ddlnextcity.SelectedValue);
                                }
                                int retsave = dp.InsertVisit(docID, Convert.ToInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Convert.ToInt32(ddlUndeUser.SelectedValue), 0, Convert.ToInt32(0), nCityValue, basicExampleDDL.SelectedValue, "", basicExample1DDL.SelectedValue, "", Convert.ToInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, false, Convert.ToInt32(ddlnextwith.SelectedValue), CityStr1, CityStrtext, Convert.ToDecimal(txtAmountEmail.Text), Convert.ToDecimal(txtAmountPhone.Text));
                                string updateandroidid = "update transvisit set android_id='" + docID + "' where Visitdocid='" + docID + "'";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                                if (retsave != 0)
                                {
                                    hdnCityId.Value = "";
                                    hdnCityname.Value = "";
                                    Settings.Instance.VistID = Convert.ToString(retsave);
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully-" + retsave + "');", true);
                                    ClearControls();
                                    divdocid.Visible = false;
                                    Settings.Instance.DSRSMID = ddlUndeUser.SelectedValue;
                                    // btnDelete.Visible = false;
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Inserted');", true);
                                }
                            }
                            else
                            { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select City');", true); }

                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('DSR Already Exists');", true);
                    }
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date cannot be less than visit Date');", true);
            }
        }

        private int checkDateForUserExists(DateTime Date)
        {
            string str = "select count(*) from TransVisit  where VDate='" + Settings.dateformat1(Date.ToShortDateString()) + "' and SMId='" + ddlUndeUser.SelectedValue + "'";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }
        private int checkDSRAllowdays(DateTime Date)
        {
            string str = "select count(*) from TransVisit  where VDate<'" + Settings.dateformat1(Date.ToShortDateString()) + "' and SMId='" + ddlUndeUser.SelectedValue + "' and lock=0";
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }


        private void ClearControls()
        {
            txtVisitDate.Text = "";
            ddlvehicle.SelectedIndex = 0;
            ddlmodeoftransport.SelectedIndex = 0;
            txtRemarks.Text = "";
            //  btnDelete.Visible = false;
            btnSave.Text = "Save";
            ddlnextwith.SelectedIndex = 0;
            ddlnextcity.SelectedIndex = 0;
            // hfCustomerId.Value = "0";
            // txtdistName.Text = "";
            txtNextVisitDate.Text = "";
            txtRemarks.Text = "";
            divdocid.Visible = false;
            txtmDate.Text = "";
            txttodate.Text = "";
            ddlWith.SelectedIndex = 0;
         //   ddlcity.SelectedIndex = 0;
            txtAmountEmail.Text = "0.00";
            txtAmountPhone.Text = "0.00";
            //Added by Ankita 12/05/2016
            foreach (ListItem item in ddlcity.Items)
            {
                item.Selected = false;
            }
            foreach (ListItem item in ddlmaterialclass.Items)
            {
                item.Selected = false;
            }
            // txtdistName.Text = "";
            // chkIndustry.ClearSelection();
        }

        //private void BindList(string st)
        //{
        //    string[] accStaffAll = new string[20];
        //    string accStaff = st;
        //    accStaffAll = accStaff.Split(',');
        //    if (accStaffAll.Length > 0)
        //    {
        //        for (int i = 0; i < accStaffAll.Length; i++)
        //        {
        //            ListItem currentCheckBox = chkIndustry.Items.FindByValue(accStaffAll[i].ToString());
        //            if (currentCheckBox != null)
        //            {
        //                currentCheckBox.Selected = true;
        //            }
        //        }
        //    }
        //}


        private void UpdateRecord()
        {
            int Exported = DSRExported(Convert.ToInt32(ViewState["VisId"]));
            if (Exported == 1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Your DSR Is Exported To Mobile/Tab,So You Can Only Proceed From That Mobile/Tab');", true);
            }
            else
            {
                if (Datecompare())
                {
                    //Added
                    //string fromtime = txtVisitDate.Text + " " + basicExample.Value;
                    //string totime = txtVisitDate.Text + " " + basicExample1.Value;
                    string fromtime = txtVisitDate.Text + " " + basicExampleDDL.SelectedValue;
                    string totime = txtVisitDate.Text + " " + basicExample1DDL.SelectedValue;
                    if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('End time Should be greater than Start Time');", true);
                    }//End
                    else
                    {
                        string chdate = "";
                        if (txtVisitDate.Text != "")
                        {
                            chdate = Convert.ToDateTime(txtVisitDate.Text).ToShortDateString();
                        }
                        string Nextchdate = "";
                        if (txtNextVisitDate.Text != "")
                        {
                            Nextchdate = Convert.ToDateTime(txtNextVisitDate.Text).ToShortDateString();
                        }
                        string CityStr1 = "", CityStrtext = "";

                        //foreach (ListItem item in ddlcity.Items)
                        //{
                        //    if (item.Selected)
                        //    {
                        //        CityStr1 += item.Value + ",";
                        //        CityStrtext += item.Text + ",";
                        //    }
                        //}
                        //CityStr1 = CityStr1.TrimStart(',').TrimEnd(',');
                        //CityStrtext = CityStrtext.TrimStart(',').TrimEnd(',');

                        foreach (ListItem item in ddlcity.Items)
                        {

                            if (item.Selected)
                            {
                                CityStr1 += item.Value + ",";
                                CityStrtext += item.Text + ",";
                            }
                        }

                        CityStr1 = CityStr1.TrimStart(',').TrimEnd(',');
                        if (hdnCityId.Value != "")
                        {
                            //if (CityStr1 == "" || CityStr1 == "-1")
                            if (CityStr1 == "")
                            { CityStr1 += hdnCityId.Value; }
                            else
                            { CityStr1 += "," + hdnCityId.Value; }
                        }
                        else { CityStr1 = CityStr1.TrimStart(',').TrimEnd(','); }

                        CityStrtext = CityStrtext.TrimStart(',').TrimEnd(',');
                        if (hdnCityname.Value != "")
                        {
                            //if (CityStrtext == "" || CityStrtext == "OTHER CITY")
                            if (CityStrtext == "")
                            { CityStrtext += hdnCityname.Value; }
                            else
                            { CityStrtext += "," + hdnCityname.Value; }

                        }
                        else
                        { CityStrtext = CityStrtext.TrimStart(',').TrimEnd(','); }
                        if (CityStr1 != "")
                        {
                            string smIDStr1 = "";
                            //         string message = "";
                            foreach (ListItem item in ddlmaterialclass.Items)
                            {
                                if (item.Selected)
                                {
                                    smIDStr1 += item.Value + ",";
                                }
                            }
                            smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                            //int retsave = dp.Update(Convert.ToInt32(ViewState["VisId"]), Convert.ToInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Convert.ToInt32(ddlUndeUser.SelectedValue), Convert.ToInt32(ddlcity.SelectedValue), Convert.ToInt32(0), Convert.ToInt32(ddlnextcity.SelectedValue), basicExample.Value, "", basicExample1.Value, "", Convert.ToInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, Convert.ToInt32(ddlnextwith.SelectedValue));
                            int cityVal = 0, nCityVal = 0;
                            if (CityStrtext == "OTHER CITY")
                            {
                                cityVal = Convert.ToInt32(ddlcity.SelectedValue);
                            }
                            if (ddlnextcity.SelectedItem.Text != "OTHER CITY")
                            {
                                nCityVal = Convert.ToInt32(ddlnextcity.SelectedValue);
                            }
                            int retsave = dp.UpdateVisit(Convert.ToInt32(ViewState["VisId"]), Convert.ToInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Convert.ToInt32(ddlUndeUser.SelectedValue), 0, Convert.ToInt32(0), nCityVal, basicExampleDDL.SelectedValue, "", basicExample1DDL.SelectedValue, "", Convert.ToInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, Convert.ToInt32(ddlnextwith.SelectedValue), CityStr1, CityStrtext, Convert.ToDecimal(txtAmountEmail.Text), Convert.ToDecimal(txtAmountPhone.Text));

                            if (retsave == 1)
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                                // btnDelete.Visible = false;
                                btnSave.Text = "Save";
                                ClearControls();
                            }

                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                            }
                        }
                        else
                        { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select City');", true); }

                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date cannot be less than visit Date');", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // ClearControls();
            Response.Redirect("~/DSREntryFormLevel2.aspx");
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    string confirmValue = Request.Form["confirm_value"];
        //    if (confirmValue == "Yes")
        //    {
        //        int retdel = dp.delete(Request.QueryString["CollId"]);
        //        if (retdel == 1)
        //        {
        //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
        //            btnDelete.Visible = false;
        //            btnSave.Text = "Save";
        //            ClearControls();

        //        }
        //    }
        //    else
        //    {
        //        // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
        //    }


        //}
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //fillRepeter();
            rpt.DataSource = null;
            rpt.DataBind();
            txtmDate.Text = DateTime.Parse(Settings.GetUTCTime().ToString()).AddMonths(-1).ToString("dd/MMM/yyyy");
            txttodate.Text = DateTime.Parse(Settings.GetUTCTime().ToString()).ToString("dd/MMM/yyyy");
            // btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText, string contextKey)
        {//Ankita - 10/may/2016- (For Optimization)
           // string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and CityId=" + contextKey + "";
            string str = "select PartyId,PartyName FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and CityId=" + contextKey + "";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["PartyName"].ToString(), dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }
        private bool Datecompare()
        {
            bool re = true;
            if (txtNextVisitDate.Text != "")
            {
                if (Convert.ToDateTime(txtVisitDate.Text) > Convert.ToDateTime(txtNextVisitDate.Text))
                {
                    re = false;
                }
            }
            return re;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int Exported = DSRExported(Convert.ToInt32(ViewState["VisId"]));
            if (Exported == 1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Your DSR Is Exported To Mobile/Tab,So You Can Only Proceed From That Mobile/Tab');", true);
            }
            else
            {
                string CityStr1 = "", CityStrtext = "";
                foreach (ListItem item in ddlcity.Items)
                {

                    if (item.Selected)
                    {
                        CityStr1 += item.Value + ",";
                        CityStrtext += item.Text + ",";
                    }
                }

                CityStr1 = CityStr1.TrimStart(',').TrimEnd(',');
                if (hdnCityId.Value != "")
                {
                    //if (CityStr1 == "" || CityStr1 == "-1")
                    if (CityStr1 == "")
                    { CityStr1 += hdnCityId.Value; }
                    else
                    { CityStr1 += "," + hdnCityId.Value; }
                }
                else { CityStr1 = CityStr1.TrimStart(',').TrimEnd(','); }

                CityStrtext = CityStrtext.TrimStart(',').TrimEnd(',');
                if (hdnCityname.Value != "")
                {
                    //if (CityStrtext == "" || CityStrtext == "OTHER CITY")
                    if (CityStrtext == "")
                    { CityStrtext += hdnCityname.Value; }
                    else
                    { CityStrtext += "," + hdnCityname.Value; }

                }
                else
                { CityStrtext = CityStrtext.TrimStart(',').TrimEnd(','); }
                if (CityStr1 != "" || hdncitylock.Value != "")
                {

                    string smIDStr1 = "";
                    //         string message = "";
                    foreach (ListItem item in ddlmaterialclass.Items)
                    {
                        if (item.Selected)
                        {
                            smIDStr1 += item.Value + ",";
                        }
                    }
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');

                    if (Datecompare())
                    {
                        //Added
                        //string fromtime = txtVisitDate.Text + " " + basicExample.Value;
                        //string totime = txtVisitDate.Text + " " + basicExample1.Value;
                        string fromtime = txtVisitDate.Text + " " + basicExampleDDL.SelectedValue;
                        string totime = txtVisitDate.Text + " " + basicExample1DDL.SelectedValue;
                        DateTime dsrdate = Convert.ToDateTime(txtVisitDate.Text);
                        DateTime strdate = dsrdate.AddDays(-Settings.GetDsrDays(Settings.DMInt32(ddlUndeUser.SelectedValue)));
                        if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('End time Should be greater than Start Time');", true);
                        }//End
                        else
                        {
                            if (checkDateForUserExists(Convert.ToDateTime(txtVisitDate.Text)) == 0)
                            {
                                string docID = Settings.GetDocID("VISSN", DateTime.Now);
                                Settings.SetDocID("VISSN", docID);
                                string chdate = "";
                                if (txtVisitDate.Text != "")
                                {
                                    chdate = Convert.ToDateTime(txtVisitDate.Text).ToShortDateString();
                                }
                                string Nextchdate = "";
                                if (txtNextVisitDate.Text != "")
                                {
                                    Nextchdate = Convert.ToDateTime(txtNextVisitDate.Text).ToShortDateString();
                                }
                                if (checkDSRAllowdays(strdate) != 0)
                                { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('you can not enter new dsr because your previous date dsr entry not locked or contact crm admin.');", true); }
                                else
                                {
                                    //int retsave = dp.Insert(docID, Settings.DMInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Settings.DMInt32(ddlUndeUser.SelectedValue), Settings.DMInt32(ddlcity.SelectedValue), Convert.ToInt32(0), Settings.DMInt32(ddlnextcity.SelectedValue), basicExample.Value, "", basicExample1.Value, "", Settings.DMInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, false, Settings.DMInt32(ddlnextwith.SelectedValue));
                                    //if (ddlcity.SelectedItem.Text == "OTHER CITY")
                                    if (CityStrtext == "OTHER CITY")
                                    {
                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                    }
                                    else if (ddlnextcity.SelectedItem.Text == "OTHER CITY")
                                    {
                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                    }
                                    else
                                    {
                                        int retsave = dp.InsertVisit(docID, Settings.DMInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Settings.DMInt32(ddlUndeUser.SelectedValue), 0, Convert.ToInt32(0), Settings.DMInt32(ddlnextcity.SelectedValue), basicExampleDDL.SelectedValue, "", basicExample1DDL.SelectedValue, "", Settings.DMInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, false, Settings.DMInt32(ddlnextwith.SelectedValue), CityStr1, CityStrtext, Convert.ToDecimal(txtAmountEmail.Text), Convert.ToDecimal(txtAmountPhone.Text));
                                        string updateandroidid = "update transvisit set android_id='" + docID + "' where Visitdocid='" + docID + "'";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                                        if (retsave != 0)
                                        {
                                            ViewState["VisId"] = Convert.ToString(retsave);
                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                                            //  ClearControls();
                                            Settings.Instance.DSRSMID = ddlUndeUser.SelectedValue;
                                            VisitID = Convert.ToString(ViewState["VisId"]);
                                            hdnCityId.Value = "";
                                            hdnCityname.Value = "";
                                            Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityStr1 + "&VisitID=" + VisitID + "&DistID=" + 0);

                                        }
                                        else
                                        {
                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Inserted');", true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Settings.Instance.DSRSMID = ddlUndeUser.SelectedValue;
                                VisitID = Convert.ToString(ViewState["VisId"]);
                                if (VisitID == "")
                                {
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists please select from list');", true);
                                }
                                else
                                {
                                    if (btnSave.Text != "Locked")
                                    {
                                        if (Datecompare())
                                        {
                                            string chdate = "";
                                            if (txtVisitDate.Text != "")
                                            {
                                                chdate = Convert.ToDateTime(txtVisitDate.Text).ToShortDateString();
                                            }
                                            string Nextchdate = "";
                                            if (txtNextVisitDate.Text != "")
                                            {
                                                Nextchdate = Convert.ToDateTime(txtNextVisitDate.Text).ToShortDateString();
                                            }

                                            //int retsave = dp.Update(Convert.ToInt32(ViewState["VisId"]), Settings.DMInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Settings.DMInt32(ddlUndeUser.SelectedValue), Settings.DMInt32(ddlcity.SelectedValue), Convert.ToInt32(0), Settings.DMInt32(ddlnextcity.SelectedValue), basicExample.Value, "", basicExample1.Value, "", Settings.DMInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, Settings.DMInt32(ddlnextwith.SelectedValue));
                                            //if (ddlcity.SelectedItem.Text == "OTHER CITY")
                                            if (CityStrtext == "OTHER CITY")
                                            {
                                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                            }
                                            else if (ddlnextcity.SelectedItem.Text == "OTHER CITY")
                                            {
                                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                            }
                                            else
                                            {
                                                int retsave = dp.UpdateVisit(Convert.ToInt32(ViewState["VisId"]), Settings.DMInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Settings.DMInt32(ddlUndeUser.SelectedValue), 0, Convert.ToInt32(0), Settings.DMInt32(ddlnextcity.SelectedValue), basicExampleDDL.SelectedValue, "", basicExample1DDL.SelectedValue, "", Settings.DMInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, Settings.DMInt32(ddlnextwith.SelectedValue), CityStr1, CityStrtext, Convert.ToDecimal(txtAmountEmail.Text), Convert.ToDecimal(txtAmountPhone.Text));

                                                if (retsave == 1)
                                                {

                                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                                                    // btnDelete.Visible = false;
                                                    btnSave.Text = "Save";
                                                    //ClearControls();
                                                    hdnCityId.Value = "";
                                                    hdnCityname.Value = "";
                                                }

                                                else
                                                {
                                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                                                }
                                                Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityStr1 + "&VisitID=" + VisitID + "&DistID=" + 0);
                                            }

                                        }

                                        else
                                        {
                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date cannot be less than visit Date');", true);
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + hdncitylock.Value + "&VisitID=" + VisitID + "&DistID=" + 0);
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date cannot be less than visit Date');", true);
                    }
                }
                else
                { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select City');", true); }

            }
        }


        protected void ddlUndeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWithSalesPersonDDl(Convert.ToInt32(ddlUndeUser.SelectedValue));
            BindDDlCity();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (txtmDate.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select From Date.');", true);
            }
            if (txttodate.Text == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select To Date.');", true);
            }
            if (ddlUser1.SelectedValue == "0")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select user.');", true);
            }

            string str = "";
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtmDate.Text))
            {
                if (txtmDate.Text != "" && txttodate.Text != "")
                {//Ankita - 10/may/2016- (For Optimization)
                    //str = @"select * from TransVisit where  SMId=" + ddlUser1.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' and VDate<='" + Settings.dateformat1(txttodate.Text) + "' order by VDate desc";
                    str = @"select VisId,VDate,VisitDocId,frTime1,toTime1,Remark,Lock,AppStatus = (case when (Appstatus) = 'Approve' then 'Approved' WHEN (Appstatus) = 'Reject' THEN 'Reject' ELSE Appstatus end) from TransVisit where  SMId=" + ddlUser1.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' and VDate<='" + Settings.dateformat1(txttodate.Text) + "' order by VDate desc";
                }
                else if (txtmDate.Text != "")
                {
                    //str = @"select * from TransVisit where  SMId=" + ddlUser1.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";
                    str = @"select VisId,VDate,VisitDocId,frTime1,toTime1,Remark,Lock,AppStatus = (case when (Appstatus) = 'Approve' then 'Approved' WHEN (Appstatus) = 'Reject' THEN 'Reject' ELSE Appstatus end) from TransVisit where  SMId=" + ddlUser1.SelectedValue + " and VDate>='" + Settings.dateformat1(txtmDate.Text) + "' order by VDate desc";
                }
                else
                {
                    //str = @"select * from TransVisit  where   SMId=" + ddlUser1.SelectedValue + " order by VDate desc";
                    str = @"select VisId,VDate,VisitDocId,frTime1,toTime1,Remark,Lock,AppStatus = (case when (Appstatus) = 'Approve' then 'Approved' WHEN (Appstatus) = 'Reject' THEN 'Reject' ELSE Appstatus end) from TransVisit  where   SMId=" + ddlUser1.SelectedValue + " order by VDate desc";
                }
                DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (depdt.Rows.Count > 0)
                {
                    rpt.DataSource = depdt;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = null;
                    rpt.DataBind();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                rpt.DataSource = null;
                rpt.DataBind();
            }
        }


        //---------------- Lock DSR Entry --------------------




        private static void fillTransOrder(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO TransOrder (VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,created_date,android_id,Longitude,Latitude,Lat_long_datetime,address)
                             SELECT VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,created_date,android_id,Longitude,Latitude,Lat_long_datetime,address
                            FROM Temp_TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update TransOrder set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransOrderDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void fillTransOrderItemwise(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO TransOrder (VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,created_date,android_id,Longitude,Latitude,Lat_long_datetime,address)
                             SELECT VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,created_date,android_id,Longitude,Latitude,Lat_long_datetime,address
                            FROM Temp_TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"INSERT INTO dbo.TransOrder1 (OrdId,VisId, OrdDocId, Sno, UserId, VDate, SMId, PartyId, AreaId, ItemId, Qty, FreeQty, Rate, Discount, Remarks, 
                             MeetFlag, MeetDocId, amount,created_date,cases,unit,android_id,Longitude,Latitude,Lat_long_datetime,address)
                             SELECT  OrdId, VisId, OrdDocId, Sno, UserId, VDate, SMId, PartyId, AreaId, ItemId, Qty, FreeQty, Rate,
                             Discount, Remarks, MeetFlag, MeetDocId, amount,created_date,cases,unit,android_id,Longitude,Latitude,Lat_long_datetime,address
                             FROM dbo.Temp_TransOrder1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                 str = "update TransOrder set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update TransOrder1 set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransOrderDeleteItemwise(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"delete  FROM Temp_TransOrder1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void fillTransDemo(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO TransDemo ([VisId],[DemoDocId],[UserId],[VDate] ,[SMId] ,[PartyId] ,[Remarks]  ,[AreaId]  ,[CompleteAppDetail]  ,[AvailablityShop]  ,[IsPartyConverted]
      ,[NewAppArea]  ,[TechAdvantage]  ,[TechSuggestion]  ,[NewApp]   ,[OrderType]  ,[ProductClassId]  ,[ProductSegmentId]  ,[ProductMatGrp] ,[ItemId],[ImgURL] ,created_date,android_id,Longitude,Latitude,Lat_long_datetime,address)
    SELECT [VisId] ,[DemoDocId] ,[UserId] ,[VDate] ,[SMId] ,[PartyId] ,[Remarks] ,[AreaId] ,[CompleteAppDetail] ,[AvailablityShop]  ,[IsPartyConverted],[NewAppArea] ,[TechAdvantage]  ,[TechSuggestion]
      ,[NewApp] ,[OrderType],[ProductClassId]   ,[ProductSegmentId]   ,[ProductMatGrp]      ,[ItemId],[ImgURL],created_date,android_id,Longitude,Latitude,Lat_long_datetime,address  FROM Temp_TransDemo where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                str = "update TransDemo set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update distributercollection set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransDemoDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransDemo where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillCollection(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO TransCollection([VisId] ,[CollDocId] ,[UserId] ,[VDate] ,[PartyId] ,[SMId] ,[AreaId] ,[ItemId] ,[Mode] ,[Amount] ,[PaymentDate] ,[Cheque_DDNo]
      ,[Cheque_DD_Date] ,[Bank] ,[Branch]  ,[Remarks],created_date,android_id,Longitude,Latitude,Lat_long_datetime,address)
	  SELECT [VisId] ,[CollDocId]  ,[UserId]   ,[VDate]  ,[PartyId]   ,[SMId]   ,[AreaId]  ,[ItemId]  ,[Mode]  ,[Amount]
      ,[PaymentDate] ,[Cheque_DDNo] ,[Cheque_DD_Date] ,[Bank] ,[Branch] ,[Remarks],created_date,android_id,Longitude,Latitude,Lat_long_datetime,address
	  from [Temp_TransCollection] where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update TransCollection set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void CollectionDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransCollection where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillFailedVisite(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO [TransFailedVisit] ([VisId] ,[FVDocId] ,[VDate]  ,[UserID]  ,[SMId] ,[PartyId]  ,[Remarks]
      ,[AreaId]   ,[Nextvisit]   ,[ReasonID],[VisitTime],created_date ,android_id,Longitude,Latitude,Lat_long_datetime,address)
SELECT [VisId]  ,[FVDocId]   ,[VDate]   ,[UserID]   ,[SMId]   ,[PartyId]   ,[Remarks]   ,[AreaId]   ,[Nextvisit]   ,[ReasonID],[VisitTime],created_date,android_id,Longitude,Latitude,Lat_long_datetime,address FROM [Temp_TransFailedVisit] where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                str = "update TransFailedVisit set created_date=DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void FailedVisiteDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransFailedVisit where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void fillCopetitor(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO [TransCompetitor] ([VisId] ,[DocId]  ,[VDate] ,[UserId] ,[PartyId]  ,[Item]  ,[Qty]  ,[Rate],[SMID],[CompName],[ImgUrl],[Remarks],[Discount],[BrandActivity],[MeetActivity],[RoadShow],[Scheme/offers],[OtherGeneralInfo],[OtherActivity],created_date,android_id,Longitude,Latitude,Lat_long_datetime,address)
SELECT [VisId] ,[DocId]  ,[VDate] ,[UserId] ,[PartyId]  ,[Item]  ,[Qty]  ,[Rate],[SMID],[CompName],[ImgUrl],[Remarks],[Discount],[BrandActivity],[MeetActivity],[RoadShow],[Scheme/offers],[OtherGeneralInfo],[OtherActivity],created_date,android_id,Longitude,Latitude,Lat_long_datetime,address FROM [Temp_TransCompetitor] where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update TransCompetitor set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void CopetitorDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransCompetitor where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        private static void fillDiscussion(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO [TransVisitDist] ([UserId],[VisId] ,[Sno] ,[VDate] ,[cityId] ,[SMId] ,[DistId] ,[areaId] ,[remarkDist] ,[remarkArea]  ,[remarkL1] ,[L1UserId],[DissType],[NextVisitDate],[NextVisitTime],[SpentfrTime],[SpentToTime],[ImgUrl],created_date,DiscDocid,android_id,Longitude,Latitude,Lat_long_datetime,address)
SELECT [UserId],[VisId] ,[Sno] ,[VDate] ,[cityId] ,[SMId] ,[DistId] ,[areaId] ,[remarkDist] ,[remarkArea]  ,[remarkL1] ,[L1UserId],[DissType],[NextVisitDate],[NextVisitTime],[SpentfrTime],[SpentToTime],[ImgUrl],created_date,DiscDocid,android_id,Longitude,Latitude,Lat_long_datetime,address FROM [Temp_TransVisitDist] where [VisId]=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update TransVisitDist set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void DiscussionDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransVisitDist where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private static void fillDistributorOpeningStock(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO TransDistStock ([VisId],[STKDocId],[UserId],[VDate],[SMId],[DistId],[DistCode],[AreaId],[ItemId],[Qty],[Android_Id],[Created_date],[unit],[cases],[Address],[Latitude],[Longitude],[Lat_long_datetime])
                SELECT [VisId],[STKDocId],[UserId],[VDate],[SMId],[DistId],[DistCode],[AreaId],[ItemId],[Qty],[Android_Id],[Created_date],[unit],[cases],[Address],[Latitude],[Longitude],[Lat_long_datetime] FROM TEMP_TransDistStock where [VisId]=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update TransDistStock set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private static void DeleteDistributorOpeningStock(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM Temp_TransDistStock where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void updateDSR(Int64 VisitID)
        {
            try
            {
                string sr = "update TransVisit set Lock=1,created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sr);
            }
            catch
            {

            }
        }



        protected void btnLock_Click(object sender, EventArgs e)
        {
            try
            {
                int Exported = DSRExported(Convert.ToInt32(ViewState["VisId"]));
                if (Exported == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Your DSR Is Exported To Mobile/Tab,So You Can Only Proceed From That Mobile/Tab');", true);
                }
                else
                {
                    //Added on 17-12-2015
                    if (ViewState["VisId"] != null)
                    {
                        string lockCountQry = @"select Count(*) from TransVisit where UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " and VDate < '" + Settings.dateformat(txtVisitDate.Text) + "' and Lock=0 and SMId=" + ddlUndeUser.SelectedValue + "";
                        int lockCount = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, lockCountQry));
                        //End
                        if (lockCount == 0)
                        {
                            string confirmValue = Request.Form["confirm_value"];
                            if (confirmValue == "Yes")
                            {

                                VisitID = Convert.ToString(ViewState["VisId"]);
                                string cityCountQry = @"select Count(*) from TransVisit where VisId=" + VisitID + " and Lock=0 and (CityIds='0' or nCityId=0) and SMId=" + ddlUndeUser.SelectedValue + "";
                                int cityCount = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, cityCountQry));
                                string query = "select OrderEnrty from mastenviro  ";
                                int orderEntry = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, query));
                                if (cityCount == 0)
                                {
                                    if (Convert.ToInt32(VisitID) > 0)
                                    {
                                        if (orderEntry != 1)
                                        {
                                            fillTransOrderItemwise(Convert.ToInt32(VisitID));
                                            TransOrderDeleteItemwise(Convert.ToInt32(VisitID));
                                        }
                                        else
                                        {
                                            fillTransOrder(Convert.ToInt32(VisitID));
                                            TransOrderDelete(Convert.ToInt32(VisitID));
                                        }
                                        //fillTransOrder(Convert.ToInt32(VisitID));
                                        //TransOrderDelete(Convert.ToInt32(VisitID));
                                        fillTransDemo(Convert.ToInt32(VisitID));
                                        TransDemoDelete(Convert.ToInt32(VisitID));
                                        fillCollection(Convert.ToInt32(VisitID));
                                        CollectionDelete(Convert.ToInt32(VisitID));
                                        fillFailedVisite(Convert.ToInt32(VisitID));
                                        FailedVisiteDelete(Convert.ToInt32(VisitID));
                                        fillCopetitor(Convert.ToInt32(VisitID));
                                        CopetitorDelete(Convert.ToInt32(VisitID));

                                        fillDiscussion(Convert.ToInt32(VisitID));
                                        DiscussionDelete(Convert.ToInt32(VisitID));

                                        fillDistributorOpeningStock(Convert.ToInt32(VisitID));
                                        DeleteDistributorOpeningStock(Convert.ToInt32(VisitID));

                                        updateDSR(Convert.ToInt32(VisitID));
                                        string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'LockedDSR','','LockedDSR','','','" + VisitID + "')";
                                        DAL.DbConnectionDAL.ExecuteQuery(strinslog);

                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('DSR Locked Successfully');", true);

                                        btnSave.Text = "Locked";
                                        btnSave.Enabled = false;
                                        btnLock.Enabled = false;
                                        btnLock.CssClass = "btn btn-primary";
                                        ddlcity.Enabled = false;
                                    }
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city and then lock entry.');", true);
                                }
                            }

                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('DSR not locked for previous dates');", true);
                            //       return;
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select a DSR entry from List to Lock');", true);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void txtVisitDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string visid = string.Empty;
                string strdate = @"select * from TransVisit  where Vdate='" + txtVisitDate.Text + "' and smid = '" + ddlUndeUser.SelectedValue + "'";
                DataTable dtvisitdate = DbConnectionDAL.GetDataTable(CommandType.Text, strdate);
                if (dtvisitdate.Rows.Count > 0)
                {
                    visid = dtvisitdate.Rows[0]["VisId"].ToString();
                    ViewState["VisId"] = visid;
                    FillDeptControls(Convert.ToInt32(visid));
                    LockedValues(Settings.GetVisitLocked(Convert.ToInt32(ViewState["VisId"])));
                    RejectedValues(Settings.GetVisitRejected(Convert.ToInt32(ViewState["VisId"])));
                    if (Settings.Instance.RoleType == "Admin")
                    {
                        btnUnlock.Visible = true;
                        btnUnlock.CssClass = "btn btn-primary";
                        btnUnlock.Enabled = true;
                        btnUnlock.CssClass = "btn btn-primary";
                    }
                }
                else
                { }
                txtNextVisitDate.Text = DateTime.Parse(Convert.ToDateTime(txtVisitDate.Text).AddDays(7.00).ToShortDateString()).ToString("dd/MMM/yyyy");
                CalendarExtender3.StartDate = Convert.ToDateTime(txtVisitDate.Text).AddDays(1);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnSecondary_Click(object sender, EventArgs e)
        {
            int Exported = DSRExported(Convert.ToInt32(ViewState["VisId"]));
            if (Exported == 1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Your DSR Is Exported To Mobile/Tab,So You Can Only Proceed From That Mobile/Tab');", true);
            }
            else
            {
                string CityStr1 = "", CityStrtext = "";
                DateTime dsrdate = Convert.ToDateTime(txtVisitDate.Text);
                DateTime strdate = dsrdate.AddDays(-Settings.GetDsrDays(Settings.DMInt32(ddlUndeUser.SelectedValue)));
                //foreach (ListItem item in ddlcity.Items)
                //{
                //    if (item.Selected)
                //    {
                //        CityStr1 += item.Value + ",";
                //        CityStrtext += item.Text + ",";
                //    }
                //}
                //CityStr1 = CityStr1.TrimStart(',').TrimEnd(',');
                //CityStrtext = CityStrtext.TrimStart(',').TrimEnd(',');
                foreach (ListItem item in ddlcity.Items)
                {

                    if (item.Selected)
                    {
                        CityStr1 += item.Value + ",";
                        CityStrtext += item.Text + ",";
                    }
                }

                CityStr1 = CityStr1.TrimStart(',').TrimEnd(',');
                if (hdnCityId.Value != "")
                {
                    //if (CityStr1 == "" || CityStr1 == "-1")
                    if (CityStr1 == "")
                    { CityStr1 += hdnCityId.Value; }
                    else
                    { CityStr1 += "," + hdnCityId.Value; }
                }
                else { CityStr1 = CityStr1.TrimStart(',').TrimEnd(','); }

                CityStrtext = CityStrtext.TrimStart(',').TrimEnd(',');
                if (hdnCityname.Value != "")
                {
                    //if (CityStrtext == "" || CityStrtext == "OTHER CITY")
                    if (CityStrtext == "")
                    { CityStrtext += hdnCityname.Value; }
                    else
                    { CityStrtext += "," + hdnCityname.Value; }

                }
                else
                { CityStrtext = CityStrtext.TrimStart(',').TrimEnd(','); }

                if (CityStr1 != "" || hdncitylock.Value != "")
                {
                    string smIDStr1 = "";
                    foreach (ListItem item in ddlmaterialclass.Items)
                    {
                        if (item.Selected)
                        {
                            smIDStr1 += item.Value + ",";
                        }
                    }
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                    if (Datecompare())
                    {
                        string fromtime = txtVisitDate.Text + " " + basicExampleDDL.SelectedValue;
                        string totime = txtVisitDate.Text + " " + basicExample1DDL.SelectedValue;
                        if (Convert.ToDateTime(fromtime) > Convert.ToDateTime(totime))
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('End time Should be greater than Start Time');", true);
                        }
                        else
                        {
                            if (checkDateForUserExists(Convert.ToDateTime(txtVisitDate.Text)) == 0)
                            {
                                string docID = Settings.GetDocID("VISSN", DateTime.Now);
                                Settings.SetDocID("VISSN", docID);
                                string chdate = "";
                                if (txtVisitDate.Text != "")
                                {
                                    chdate = Convert.ToDateTime(txtVisitDate.Text).ToShortDateString();
                                }
                                string Nextchdate = "";
                                if (txtNextVisitDate.Text != "")
                                {
                                    Nextchdate = Convert.ToDateTime(txtNextVisitDate.Text).ToShortDateString();
                                }
                                if (checkDSRAllowdays(strdate) != 0)
                                { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('you can not enter new dsr because your previous date dsr entry not locked or contact crm admin.');", true); }
                                else
                                {
                                    //if (ddlcity.SelectedItem.Text == "OTHER CITY")
                                    if (CityStrtext == "OTHER CITY")
                                    {
                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                    }
                                    else if (ddlnextcity.SelectedItem.Text == "OTHER CITY")
                                    {
                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                    }
                                    else
                                    {
                                        int retsave = dp.InsertVisit(docID, Settings.DMInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Settings.DMInt32(ddlUndeUser.SelectedValue), 0, Convert.ToInt32(0), Settings.DMInt32(ddlnextcity.SelectedValue), basicExampleDDL.SelectedValue, "", basicExample1DDL.SelectedValue, "", Settings.DMInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, false, Settings.DMInt32(ddlnextwith.SelectedValue), CityStr1, CityStrtext, Convert.ToDecimal(txtAmountEmail.Text), Convert.ToDecimal(txtAmountPhone.Text));
                                        string updateandroidid = "update transvisit set android_id='" + docID + "' where Visitdocid='" + docID + "'";
                                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                                        if (retsave != 0)
                                        {
                                            ViewState["VisId"] = Convert.ToString(retsave);
                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                                            //  ClearControls();
                                            Settings.Instance.DSRSMID = ddlUndeUser.SelectedValue;
                                            VisitID = Convert.ToString(ViewState["VisId"]);
                                            // Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + ddlcity.SelectedValue + "&VisitID=" + VisitID + "&DistID=" + 0);
                                            hdnCityId.Value = "";
                                            hdnCityname.Value = "";
                                            Response.Redirect("DistributerPartyList.aspx?VisitID=" + VisitID + "&CityID=" + CityStr1 + "&Level=2" + "&PageL2=Secondary");

                                        }
                                        else
                                        {
                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                Settings.Instance.DSRSMID = ddlUndeUser.SelectedValue;
                                VisitID = Convert.ToString(ViewState["VisId"]);
                                if (VisitID == "")
                                {
                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists please select from list');", true);
                                }
                                else
                                {
                                    if (btnSave.Text != "Locked")
                                    {
                                        if (Datecompare())
                                        {
                                            string chdate = "";
                                            if (txtVisitDate.Text != "")
                                            {
                                                chdate = Convert.ToDateTime(txtVisitDate.Text).ToShortDateString();
                                            }
                                            string Nextchdate = "";
                                            if (txtNextVisitDate.Text != "")
                                            {
                                                Nextchdate = Convert.ToDateTime(txtNextVisitDate.Text).ToShortDateString();
                                            }
                                            if (CityStrtext == "OTHER CITY")
                                            {
                                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                            }
                                            else if (ddlnextcity.SelectedItem.Text == "OTHER CITY")
                                            {
                                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please contact CRM-Admin to add new city.');", true);
                                            }
                                            else
                                            {
                                                int retsave = dp.UpdateVisit(Convert.ToInt32(ViewState["VisId"]), Settings.DMInt32(Settings.Instance.UserID), chdate, Nextchdate, txtRemarks.Text, Settings.DMInt32(ddlUndeUser.SelectedValue), 0, Convert.ToInt32(0), Settings.DMInt32(ddlnextcity.SelectedValue), basicExampleDDL.SelectedValue, "", basicExample1DDL.SelectedValue, "", Settings.DMInt32(ddlWith.SelectedValue), ddlmodeoftransport.SelectedValue, ddlvehicle.SelectedValue, smIDStr1, Settings.DMInt32(ddlnextwith.SelectedValue), CityStr1, CityStrtext, Convert.ToDecimal(txtAmountEmail.Text), Convert.ToDecimal(txtAmountPhone.Text));

                                                if (retsave == 1)
                                                {

                                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                                                    // btnDelete.Visible = false;
                                                    btnSave.Text = "Save";
                                                    hdnCityId.Value = "";
                                                    hdnCityname.Value = "";
                                                    //ClearControls();
                                                }

                                                else
                                                {
                                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                                                }
                                                Response.Redirect("DistributerPartyList.aspx?VisitID=" + VisitID + "&CityID=" + CityStr1 + "&Level=2" + "&PageL2=Secondary");
                                            }

                                        }

                                        else
                                        {
                                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date cannot be less than visit Date');", true);
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("DistributerPartyList.aspx?VisitID=" + VisitID + "&CityID=" + hdncitylock.Value + "&Level=2" + "&PageL2=Secondary");
                                    }
                                    // Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + ddlcity.SelectedValue + "&VisitID=" + VisitID + "&DistID=" + 0);

                                    //Response.Redirect("DistributerPartyList.aspx?VisitID=" + VisitID + "&CityID=" + ddlcity.SelectedValue + "&Level=2" + "&PageL2=Secondary");

                                }
                            }
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date cannot be less than visit Date');", true);
                    }
                }
                else
                { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage(' Please select City');", true); }
            }
        }

        //----------------UnLock DSR Entry --------------------


        private static void fillTemp_TransOrder(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransOrder (VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,created_date,android_id,Longitude,Latitude,Lat_long_datetime,address)
                             SELECT VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,created_date,android_id,Longitude,Latitude,Lat_long_datetime,address
                            FROM TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransOrder set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransOrderDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void fillTemp_TransOrderItemWise(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransOrder (VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,android_id,created_date,Longitude,Latitude,Lat_long_datetime,address)
                             SELECT VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,android_id,created_date,Longitude,Latitude,Lat_long_datetime,address
                            FROM TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"INSERT INTO dbo.Temp_TransOrder1 (OrdId,VisId, OrdDocId, Sno, UserId, VDate, SMId, PartyId, AreaId, ItemId, Qty, FreeQty, Rate, Discount, Remarks, 
                             MeetFlag, MeetDocId, amount,android_id,android_id1,created_date,cases,unit,Longitude,Latitude,Lat_long_datetime,address)
                             SELECT  OrdId, VisId, OrdDocId, Sno, UserId, VDate, SMId, PartyId, AreaId, ItemId, Qty, FreeQty, Rate,
                             Discount, Remarks, MeetFlag, MeetDocId, amount,android_id,android_id1,created_date,cases,unit,Longitude,Latitude,Lat_long_datetime,address
                             FROM dbo.TransOrder1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransOrder set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransOrder1 set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransOrderDelete1ItemWise(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"delete  FROM TransOrder1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void fillTemp_TransDemo(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransDemo ([VisId],[DemoDocId],[UserId],[VDate] ,[SMId] ,[PartyId] ,[Remarks]  ,[AreaId]  ,[CompleteAppDetail]  ,[AvailablityShop]  ,[IsPartyConverted]
      ,[NewAppArea]  ,[TechAdvantage]  ,[TechSuggestion]  ,[NewApp]   ,[OrderType]  ,[ProductClassId]  ,[ProductSegmentId]  ,[ProductMatGrp] ,[ItemId],[ImgURL],android_id,created_date ,Longitude,Latitude,Lat_long_datetime,address)
    SELECT [VisId] ,[DemoDocId] ,[UserId] ,[VDate] ,[SMId] ,[PartyId] ,[Remarks] ,[AreaId] ,[CompleteAppDetail] ,[AvailablityShop]  ,[IsPartyConverted],[NewAppArea] ,[TechAdvantage]  ,[TechSuggestion]
      ,[NewApp] ,[OrderType],[ProductClassId]   ,[ProductSegmentId]   ,[ProductMatGrp] ,[ItemId],[ImgUrl],android_id,created_date,Longitude,Latitude,Lat_long_datetime,address  FROM TransDemo where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransDemo set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransDemoDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransDemo where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillTemp_Collection(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransCollection ([VisId] ,[CollDocId] ,[UserId] ,[VDate] ,[PartyId] ,[SMId] ,[AreaId] ,[ItemId] ,[Mode] ,[Amount] ,[PaymentDate] ,[Cheque_DDNo]
      ,[Cheque_DD_Date] ,[Bank] ,[Branch]  ,[Remarks],android_id,created_date,Longitude,Latitude,Lat_long_datetime,address
)
	  SELECT [VisId] ,[CollDocId]  ,[UserId]   ,[VDate]  ,[PartyId]   ,[SMId]   ,[AreaId]  ,[ItemId]  ,[Mode]  ,[Amount]
      ,[PaymentDate] ,[Cheque_DDNo] ,[Cheque_DD_Date] ,[Bank] ,[Branch] ,[Remarks],android_id,created_date,Longitude,Latitude,Lat_long_datetime,address

	  from TransCollection where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransCollection set created_date=DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void CollectionDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransCollection where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillTemp_FailedVisite(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO [Temp_TransFailedVisit] ([VisId] ,[FVDocId] ,[VDate]  ,[UserID]  ,[SMId] ,[PartyId]  ,[Remarks]
      ,[AreaId]   ,[Nextvisit]   ,[ReasonID],[VisitTime],android_id,created_date,Longitude,Latitude,Lat_long_datetime,address
 )
SELECT [VisId]  ,[FVDocId]   ,[VDate]   ,[UserID]   ,[SMId]   ,[PartyId]   ,[Remarks]   ,[AreaId]   ,[Nextvisit]   ,[ReasonID],[VisitTime],android_id,created_date,Longitude,Latitude,Lat_long_datetime,address
 FROM [TransFailedVisit] where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                str = "update Temp_TransFailedVisit set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void FailedVisiteDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransFailedVisit where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void fillTemp_Copetitor(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO [Temp_TransCompetitor] ([VisId] ,[DocId]  ,[VDate] ,[UserId] ,[PartyId]  ,[Item]  ,[Qty]  ,[Rate],[SMID],[CompName],[ImgUrl],[Remarks],[Discount],[BrandActivity],[MeetActivity],[RoadShow],[Scheme/offers],[OtherGeneralInfo],[OtherActivity],[android_id],created_date ,Longitude,Latitude,Lat_long_datetime,address
)
SELECT [VisId] ,[DocId]  ,[VDate] ,[UserId] ,[PartyId]  ,[Item]  ,[Qty]  ,[Rate],[SMID],[CompName],[ImgUrl],[Remarks],[Discount],[BrandActivity],[MeetActivity],[RoadShow],[Scheme/offers],[OtherGeneralInfo],[OtherActivity],[android_id],created_date,Longitude,Latitude,Lat_long_datetime,address
 FROM [TransCompetitor] where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransCompetitor set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void CopetitorDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransCompetitor where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void fillTemp_Discussion(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO [Temp_TransVisitDist] ([UserId],[VisId] ,[Sno] ,[VDate] ,[cityId] ,[SMId] ,[DistId] ,[areaId] ,[remarkDist] ,[remarkArea]  ,[remarkL1] ,[L1UserId],[DissType],[NextVisitDate],[NextVisitTime],[SpentfrTime],[SpentToTime],[ImgUrl],android_id,created_date,DiscDocid,Longitude,Latitude,Lat_long_datetime,address)
SELECT [UserId],[VisId] ,[Sno] ,[VDate] ,[cityId] ,[SMId] ,[DistId] ,[areaId] ,[remarkDist] ,[remarkArea]  ,[remarkL1] ,[L1UserId],[DissType],[NextVisitDate],[NextVisitTime],[SpentfrTime],[SpentToTime],[ImgUrl],android_id,created_date,DiscDocid,Longitude,Latitude,Lat_long_datetime,address
 FROM [TransVisitDist] where [VisId]=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransVisitDist set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void DiscussionDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransVisitDist where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private static void fillTemp_DistributorOpeningStock(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO Temp_TransDistStock ([VisId],[STKDocId],[UserId],[VDate],[SMId],[DistId],[DistCode],[AreaId],[ItemId],[Qty],[Android_Id],[Created_date],[unit],[cases],[Address],[Latitude],[Longitude],[Lat_long_datetime])
                SELECT [VisId],[STKDocId],[UserId],[VDate],[SMId],[DistId],[DistCode],[AreaId],[ItemId],[Qty],[Android_Id],[Created_date],[unit],[cases],[Address],[Latitude],[Longitude],[Lat_long_datetime] FROM TransDistStock where [VisId]=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransDistStock set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private static void DeleteTemp_DistributorOpeningStock(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransDistStock where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        private void updateDSR1(Int64 VisitID)
        {
            try
            {
                string sr = "update TransVisit set Lock=0,AppStatus=NULL,AppBy=NULL,AppRemark=NULL,AppBySMId=NULL,created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sr);
            }
            catch
            {

            }
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            try
            {
                int Exported = DSRExported(Convert.ToInt32(ViewState["VisId"]));
                if (Exported == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Your DSR Is Exported To Mobile/Tab,So You Can Only Proceed From That Mobile/Tab');", true);
                }
                else
                {
                    if (ViewState["VisId"] != null)
                    {
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
                        {
                            VisitID = Convert.ToString(ViewState["VisId"]);
                            string lockCountQry = @"select Count(*) from TransVisit WHERE AppStatus='reject' and visid = " + VisitID + " ";
                            int RejectCount = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, lockCountQry));
                            string query = "select OrderEnrty from mastenviro  ";
                            int orderEntry = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, query));
                            if (RejectCount == 1)
                            {
                                if (Convert.ToInt32(VisitID) > 0)
                                {
                                    if (orderEntry != 1)
                                    {
                                        fillTemp_TransOrderItemWise(Convert.ToInt32(VisitID));
                                        TransOrderDelete1ItemWise(Convert.ToInt32(VisitID));
                                    }
                                    else
                                    {
                                        fillTemp_TransOrder(Convert.ToInt32(VisitID));
                                        TransOrderDelete1(Convert.ToInt32(VisitID));
                                    }
                                    //fillTemp_TransOrder(Convert.ToInt32(VisitID));
                                    //TransOrderDelete1(Convert.ToInt32(VisitID));
                                    fillTemp_TransDemo(Convert.ToInt32(VisitID));
                                    TransDemoDelete1(Convert.ToInt32(VisitID));
                                    fillTemp_Collection(Convert.ToInt32(VisitID));
                                    CollectionDelete1(Convert.ToInt32(VisitID));
                                    fillTemp_FailedVisite(Convert.ToInt32(VisitID));
                                    FailedVisiteDelete1(Convert.ToInt32(VisitID));
                                    fillTemp_Copetitor(Convert.ToInt32(VisitID));
                                    CopetitorDelete1(Convert.ToInt32(VisitID));
                                    fillTemp_Discussion(Convert.ToInt32(VisitID));
                                    DiscussionDelete1(Convert.ToInt32(VisitID));
                                    fillTemp_DistributorOpeningStock(Convert.ToInt32(VisitID));
                                    DeleteTemp_DistributorOpeningStock(Convert.ToInt32(VisitID));

                                    updateDSR1(Convert.ToInt32(VisitID));

                                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('DSR UnLocked Successfully');", true);
                                    btnUnlock.Enabled = false;
                                    btnUnlock.CssClass = "btn btn-primary";
                                    btnSave.Text = "Update";
                                    btnSave.Enabled = true;
                                    btnLock.Enabled = true;
                                    btnLock.CssClass = "btn btn-primary";



                                    basicExample.Disabled = false;
                                    basicExample1.Disabled = false;
                                    basicExampleDDL.Enabled = true;
                                    basicExample1DDL.Enabled = true;
                                    ddlmodeoftransport.Enabled = true;
                                    ddlvehicle.Enabled = true;
                                    ddlWith.Enabled = true;
                                    ddlnextwith.Enabled = true;
                                    ddlnextcity.Enabled = true;
                                    txtNextVisitDate.Enabled = true;
                                    this.ddlmaterialclass.Attributes.Remove("disabled");
                                    txtRemarks.ReadOnly = false;
                                    FillDeptControls(Convert.ToInt32(VisitID));

                                }
                            }
                            else
                            {
                                if (Settings.Instance.RoleType == "Admin")
                                {
                                    if (Convert.ToInt32(VisitID) > 0)
                                    {
                                        if (orderEntry != 1)
                                        {
                                            fillTemp_TransOrderItemWise(Convert.ToInt32(VisitID));
                                            TransOrderDelete1ItemWise(Convert.ToInt32(VisitID));
                                        }
                                        else
                                        {
                                            fillTemp_TransOrder(Convert.ToInt32(VisitID));
                                            TransOrderDelete1(Convert.ToInt32(VisitID));
                                        }
                                        //fillTemp_TransOrder(Convert.ToInt32(VisitID));
                                        //TransOrderDelete1(Convert.ToInt32(VisitID));
                                        fillTemp_TransDemo(Convert.ToInt32(VisitID));
                                        TransDemoDelete1(Convert.ToInt32(VisitID));
                                        fillTemp_Collection(Convert.ToInt32(VisitID));
                                        CollectionDelete1(Convert.ToInt32(VisitID));
                                        fillTemp_FailedVisite(Convert.ToInt32(VisitID));
                                        FailedVisiteDelete1(Convert.ToInt32(VisitID));
                                        fillTemp_Copetitor(Convert.ToInt32(VisitID));
                                        CopetitorDelete1(Convert.ToInt32(VisitID));
                                        fillTemp_Discussion(Convert.ToInt32(VisitID));
                                        DiscussionDelete1(Convert.ToInt32(VisitID));
                                        fillTemp_DistributorOpeningStock(Convert.ToInt32(VisitID));
                                        DeleteTemp_DistributorOpeningStock(Convert.ToInt32(VisitID));

                                        updateDSR1(Convert.ToInt32(VisitID));

                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('DSR UnLocked Successfully');", true);
                                        btnUnlock.Enabled = false;
                                        btnUnlock.CssClass = "btn btn-primary";
                                        btnSave.Text = "Update";
                                        btnSave.Enabled = true;
                                        btnLock.Enabled = true;
                                        btnLock.CssClass = "btn btn-primary";

                                        basicExample.Disabled = false;
                                        basicExample1.Disabled = false;
                                        basicExampleDDL.Enabled = true;
                                        basicExample1DDL.Enabled = true;
                                        ddlmodeoftransport.Enabled = true;
                                        ddlvehicle.Enabled = true;
                                        ddlWith.Enabled = true;
                                        ddlnextwith.Enabled = true;
                                        ddlnextcity.Enabled = true;
                                        txtNextVisitDate.Enabled = true;
                                        this.ddlmaterialclass.Attributes.Remove("disabled");
                                        txtRemarks.ReadOnly = false;
                                        FillDeptControls(Convert.ToInt32(VisitID));

                                    }

                                }
                                else
                                { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Contact to administrator');", true); }
                            }

                        }
                    }

                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select a DSR entry from List to Lock');", true);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }    
    }
}









