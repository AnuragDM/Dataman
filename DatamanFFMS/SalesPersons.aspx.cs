using DAL;
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
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class SalesRep_List : System.Web.UI.Page
    {
        string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
        SalesPersonsBAL SP = new SalesPersonsBAL();
        BAL.UserMaster.userall usall = new BAL.UserMaster.userall();
        BAL.MastLink.MastLinkBAL ML = new BAL.MastLink.MastLinkBAL();
        string parameter = "";

        ImportData upd = new ImportData();
        bool _exportp = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            assgn.Visible = false;
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            Page.Form.DefaultButton = btnSave.UniqueID;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["SMId"] = parameter;
                //   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FillSMControls", "FillSMControls(" + parameter + ");", true);
                FillSMControls(Convert.ToInt32(parameter));

                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            CalendarExtender2.EndDate = DateTime.Now;
            CalendarExtender1.EndDate = DateTime.Now;
            DOA.Attributes.Add("readonly", "readonly");
            DOB.Attributes.Add("readonly", "readonly");
            //Ankita - 20/may/2016- (For Optimization)
            //string pageName = Path.GetFileName(Request.Path);
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            btnExport.CssClass = "btn btn-primary";
            btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            _exportp = Convert.ToBoolean(SplitPerm[4]);
            if (Convert.ToBoolean(SplitPerm[0]) == false)
            {
                Response.Redirect("~/Logout.aspx");
            }

            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";

            if (!IsPostBack)
            {
                Bindroletype(); BindCity(); BindResCentre(); BindDesignation(); BindDepartment(); BindRole();
                BindReportTo();
                BindHeadquarter();
                // BindRole();
                //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindRole", "BindRole();", true);
                chkIsActive.Checked = true;
                btnDelete.Visible = false;
                // Username.Disabled = false;
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["SMId"] != null)
                {
                    FillSMControls(Convert.ToInt32(Request.QueryString["SMId"]));
                }
                else
                {
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindUser", "BindUser();", true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindReportTo", "BindReportTo();", true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindDepartment", "BindDepartment();", true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindeDesignation", "BindeDesignation();", true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindResCentre", "BindResCentre();", true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindCity", "BindCity();", true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindRole", "BindRole();", true);
                    //CallDropdowns(); 
                }

                rptchk.DataSource = new DataTable();
                rptchk.DataBind();


            }

        }

        #region BindDropdowns

        private void Bindroletype()
        {
            string str = "";
            str = "select  roletype,rolevalue from mastroletype where rolevalue in ('AreaIncharge','CityHead','DistrictHead','RegionHead','StateHead')  ";
            fillDDLDirect(ddlSalesPerType, str, "rolevalue", "roletype", 1);
        }
        private void CallDropdowns()
        {
            //   BindReportTo(0);
            //  BindDepartment(0);
            //  BindDesignation(0);
            //   BindResCentre(0);
            ///    BindEmployee(0);
            //  BindCity(0);

        }
        private void BindEmployee(int EmpId)
        {
            string str = "";
            //if (EmpId > 0)
            //{
            //    str = "select Id,Name from MastLogin where (DeptId is not null) and (DesigId is not null) and (Active=1 or Id in (" + EmpId + "))  order by Name";
            //}
            //else
            //{
            //    str = "select Id,Name from MastLogin where (DeptId is not null) and (DesigId is not null) and Active=1 order by Name";
            //}

            //fillDDLDirect(ddlusers, str, "Id", "Name", 1);
            // bind Role Type
            str = "select  roletype,rolevalue from mastroletype ";
            fillDDLDirect(ddlSalesPerType, str, "rolevalue", "roletype", 1);
        }
        private void BindReportTo()
        {
            string sameuerfilter = "";
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["SMId"])) && (Convert.ToString(ViewState["SMId"]) != "0"))
            {
                sameuerfilter = " and smid not in (" + ViewState["SMId"] + " ) ";
            }
            Int32 ReportTo = 0;
            string str = "";
            if (ReportTo > 0)
            { str = "select SMID,SMName from MastSalesRep inner join MastLogin Ml on MastSalesRep.userId=Ml.Id Inner Join MastRole Mr on Ml.RoleId=Mr.RoleId where 1=1 and RoleType<>'AreaIncharge' and (MastSalesRep.Active='1' or SMID in (" + ReportTo + ")) " + sameuerfilter + " order by SMName "; }
            else { str = "select SMID,SMName from MastSalesRep  inner join MastLogin Ml on MastSalesRep.userId=Ml.Id Inner Join MastRole Mr on Ml.RoleId=Mr.RoleId where 1=1 and RoleType<>'AreaIncharge' and MastSalesRep.Active='1' " + sameuerfilter + " order by SMName "; }
            fillDDLDirect(ddlUnderSales, str, "SMID", "SMName", 1);
        }
        private void BindDesignation()
        {
            int DesigId = 0;
            string str = "";
            if (DesigId > 0)
                str = "select DesId,DesName from MastDesignation where (active='1' or DesId in(" + DesigId + ")) order by Desname";
            else
                str = "select DesId,DesName from MastDesignation where Active='1' order by Desname";
            fillDDLDirect(ddldesg, str, "DesId", "DesName", 1);
        }
        private void BindDepartment()
        {
            int DeptId = 0;
            string str = "";
            if (DeptId > 0)
                str = "select DepId,DepName from MastDepartment where (active='1' or DepId in(" + DeptId + ")) order by Depname";
            else
                str = "select DepId,DepName from MastDepartment where active='1' order by Depname";
            fillDDLDirect(ddldept, str, "DepId", "DepName", 1);
        }
        //private void BindGrade(int GradeId)
        //{
        //    string str = "";
        //    if (GradeId > 0)
        //    str = "select Id,Name from MastGrade where (Active='1' or Id not in ("+GradeId +")) order by name";
        //    else
        //        str = "select Id,Name from MastGrade where Active='1' order by name";
        //    fillDDLDirect(ddlgrade, str, "Id", "Name", 1);
        //}
        private void BindResCentre()
        {
            string str = ""; int ResCenId = 0;
            if (ResCenId > 0)
                str = "select ResCenId,ResCenName from MastResCentre where (Active='1' or ResCenId in (" + ResCenId + ")) order by ResCenName";
            else
                str = "select ResCenId,ResCenName from MastResCentre where Active='1' order by ResCenName";
            fillDDLDirect(ddlResCenId, str, "ResCenId", "ResCenName", 1);
        }
        private void BindCity()
        {
            string str = ""; Int32 CityId = 0;
            if (CityId > 0)
                str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and (T.cityAct=1  OR T.cityid=" + CityId + ") ORDER BY cityName";
            else
                str = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and T.cityAct=1 ORDER BY cityName";
            fillDDLDirect(ddlcity, str, "cityid", "cityName", 1);
        }
        private void BindRole()
        {
            string str = "select RoleId,RoleName from MastRole order by RoleName";
            fillDDLDirect(ddlRole, str, "RoleId", "RoleName", 1);
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
        #endregion

        private void BindHeadquarter()
        {
            string str = "";
            str = "select  Id,HeadquarterName from MastHeadquarter Order By  HeadquarterName ";
            fillDDLDirect(ddlHeadquarter, str, "Id", "HeadquarterName", 1);
        }

        private void fillRepeter()
        {
            string str = @"select msr.SMID,msr.SMName,stt.AreaName as StateName,msr.DeviceNo,msr.DSRAllowDays,case msr.Active when 1 then 'Yes' else 'No' end as Active,msr.Address1,msr.Address2,msr.CityId,msr.Pin,msr.Email,msr.Mobile,msr.Remarks,	msr.BlockDate,msr.RoleId,msr.UserId,msr.LoginCreated,msr.SyncId,msr.Lvl,msr.UnderId,msr.DisplayName,msr.GradeId,msr.SalesRepType,msr.CreatedDate,msr.DOB,msr.DOA,msr.ResCenId,msr.Phone,RoleName,DepName,DesName,Mg.Name as grade,ResCenName,Msr1.SmName As Parent,Ml.Name as UserName,Ma.Areaname As City,Ml.DesigId,Ml.DeptId,isnull(mh.HeadquarterName,'') as HeadquarterName from MastSalesRep Msr inner join MastSalesRep Msr1 on Msr.underId=Msr1.SMID left join MastLogin Ml On Msr.UserId=Ml.id left join MastRole Mr on Mr.RoleId=Ml.RoleId left join MastDepartment Md on Ml.DeptId=Md.Depid left join MastDesignation Mdsg on Ml.DesigId=Mdsg.DesId left join MastGrade Mg on Msr.GradeId=Mg.Id left join Mastarea Ma on Msr.CityId=Ma.AreaId left join MastArea dst on dst.AreaId=Ma.UnderId left join MastArea stt on stt.AreaId=dst.UnderId left join MastResCentre Mrc On Msr.ResCenId=Mrc.ResCenId left join MastHeadquarter mh on mh.id=msr.HeadquarterId where msr.SMName <> '.'";
            DataTable Itemdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = Itemdt;
            rpt.DataBind();

            Itemdt.Dispose();
        }
        private void FillSMControls(int SmId)
        {
            try
            {

                //Username.Disabled = true;
                //string Smquery = @"select * from MastSalesRep msr left join MastLogin Ml On Msr.UserId=Ml.id where SMId=" + SmId;
                string Smquery = @"select *,isnull(AllowChangeCity,0)as AllowChangeCity1,isnull(MobileAccess,0)as MobileAccess1,isnull(Managerapp,0)as Managerapp1,isnull(DMTApp,0)as DMTApp1,isnull(HeadquarterId,0) as HeadquarterId1 from MastSalesRep msr left join MastLogin Ml On Msr.UserId=Ml.id where SMId=" + SmId;

                DataTable SmValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Smquery);
                if (SmValueDt.Rows.Count > 0)
                {
                    assgn.Visible = true;
                    string roletype = SmValueDt.Rows[0]["SalesRepType"].ToString();
                    if ((roletype == "AreaIncharge") || (roletype == "CityHead") || (roletype == "DistrictHead") || (roletype == "RegionHead") || (roletype == "StateHead"))
                    {
                        divsalestype.Visible = true;
                        ddlSalesPerType.SelectedValue = SmValueDt.Rows[0]["SalesRepType"].ToString();
                    }
                    else
                    {
                        divsalestype.Visible = false;
                    }
                    txtfromtime.Value = SmValueDt.Rows[0]["fromtime"].ToString();
                    txttotime.Value = SmValueDt.Rows[0]["totime"].ToString();
                    txtrecordinterval.Value = SmValueDt.Rows[0]["interval"].ToString();
                    txtuploadinterval.Value = SmValueDt.Rows[0]["uploadinterval"].ToString();
                    txtaccuracy.Value = SmValueDt.Rows[0]["degree"].ToString();
                    ddlpushnotification.SelectedValue = SmValueDt.Rows[0]["sendpushnotification"].ToString();
                    ddlbattery.SelectedValue = SmValueDt.Rows[0]["batteryrecord"].ToString();


                    hdnoldMobile.Value = SmValueDt.Rows[0]["Mobile"].ToString();
                    hidolddeviceforlicence.Value = SmValueDt.Rows[0]["DeviceNo"].ToString();
                    txtusername.Value = SmValueDt.Rows[0]["name"].ToString();
                    txtusername.Attributes.Add("readonly", "readonly");
                    SMName.Value = SmValueDt.Rows[0]["SMName"].ToString();
                    SyncId.Value = SmValueDt.Rows[0]["SyncId"].ToString();
                    if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["DOA"].ToString()))
                        DOA.Text = Convert.ToDateTime(SmValueDt.Rows[0]["DOA"]).ToString("dd/MMM/yyyy");
                    Address1.Value = SmValueDt.Rows[0]["Address1"].ToString();
                    Address2.Value = SmValueDt.Rows[0]["Address2"].ToString();
                    EmpName.Value = SmValueDt.Rows[0]["EmpName"].ToString();
                    Pin.Value = SmValueDt.Rows[0]["Pin"].ToString();
                    Mobile.Value = SmValueDt.Rows[0]["Mobile"].ToString();
                    DeviceNo.Value = SmValueDt.Rows[0]["DeviceNo"].ToString();
                    if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["DOB"].ToString()))
                        DOB.Text = Convert.ToDateTime(SmValueDt.Rows[0]["DOB"]).ToString("dd/MMM/yyyy");
                    Email.Value = SmValueDt.Rows[0]["Email"].ToString();
                    Remarks.Value = SmValueDt.Rows[0]["Remarks"].ToString();
                    DSRAllowDays.Value = SmValueDt.Rows[0]["DSRAllowDays"].ToString();
                    HiddenRoleID.Value = SmValueDt.Rows[0]["RoleID"].ToString();
                    ddlRole.SelectedValue = SmValueDt.Rows[0]["RoleID"].ToString();
                    HiddenDesID.Value = SmValueDt.Rows[0]["DesigId"].ToString();
                    ddldesg.SelectedValue = SmValueDt.Rows[0]["DesigId"].ToString();
                    HiddenDeptID.Value = SmValueDt.Rows[0]["DeptId"].ToString();
                    ddldept.SelectedValue = SmValueDt.Rows[0]["DeptId"].ToString();
                    HiddenReportToID.Value = SmValueDt.Rows[0]["UnderId"].ToString();
                    HiddenSMID.Value = SmValueDt.Rows[0]["SMId"].ToString();
                    this.BindReportTo();
                    ddlUnderSales.SelectedValue = SmValueDt.Rows[0]["UnderId"].ToString();
                    txtmeetallowdays.Value = SmValueDt.Rows[0]["MeetAllowDays"].ToString();
                    HiddenResCentre.Value = SmValueDt.Rows[0]["ResCenId"].ToString();
                    //ddlResCenId.SelectedValue = SmValueDt.Rows[0]["ResCenId"].ToString();
                    HiddenCityID.Value = SmValueDt.Rows[0]["CityId"].ToString();
                    if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["CityId"].ToString()))
                    {
                        ddlcity.SelectedValue = SmValueDt.Rows[0]["CityId"].ToString();
                    }
                    HiddenUserID.Value = SmValueDt.Rows[0]["UserId"].ToString();
                    ddlusers.SelectedValue = SmValueDt.Rows[0]["UserId"].ToString();

                    HiddenEmailID.Value = SmValueDt.Rows[0]["Email"].ToString();
                    HiddenEmployee.Value = SmValueDt.Rows[0]["EmpName"].ToString();
                    EmpName.Value = SmValueDt.Rows[0]["EmpName"].ToString();
                    if (Convert.ToBoolean(SmValueDt.Rows[0]["AllowChangeCity1"]) == true)
                    { chkhomecity.Checked = true; }
                    else { chkhomecity.Checked = false; }
                    if (Convert.ToBoolean(SmValueDt.Rows[0]["Active"]) == true)
                    {
                        chkIsActive.Checked = true; BlockReason.Value = "";
                    }
                    else
                    {
                        divblock.Attributes.Remove("class");
                        BlockReason.Value = SmValueDt.Rows[0]["BlockReason"].ToString();
                        chkIsActive.Checked = false;
                    }
                    if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["spimagepath"].ToString()))
                    {
                        imgpreview.Src = SmValueDt.Rows[0]["spimagepath"].ToString();
                        imgpreview.Style.Add("display", "block");

                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }
                    if (Convert.ToBoolean(SmValueDt.Rows[0]["MobileAccess1"]) == true)
                    { chkmobAccess.Checked = true; }
                    else { chkmobAccess.Checked = false; }


                    if (Convert.ToBoolean(SmValueDt.Rows[0]["Managerapp1"]) == true)
                    { chkmanager.Checked = true; }
                    else { chkmanager.Checked = false; }

                    if (Convert.ToBoolean(SmValueDt.Rows[0]["DMTApp1"]) == true)
                    { chkDMT.Checked = true; }
                    else { chkDMT.Checked = false; }

                    string roletype1 = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select roletype from mastrole where roleid='" + Convert.ToInt32(ddlRole.SelectedItem.Value) + "'"));

                    if ((roletype1 == "CityHead") || (roletype1 == "DistrictHead") || (roletype1 == "RegionHead") || (roletype1 == "StateHead"))
                    {
                        //dvchk.Visible = true;
                        chkmanager.Visible = true;
                        lblmanager.Visible = true;
                    }
                    else
                    {
                        //dvchk.Visible = false; 
                        chkmanager.Visible = false;
                        lblmanager.Visible = false;
                    }

                    ddlHeadquarter.SelectedValue = SmValueDt.Rows[0]["HeadquarterId1"].ToString();
                    if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["Joining"].ToString()))
                        txtJoin.Text = Convert.ToDateTime(SmValueDt.Rows[0]["Joining"]).ToString("dd/MMM/yyyy");
                    if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["Leaving"].ToString()))
                        txtLeav.Text = Convert.ToDateTime(SmValueDt.Rows[0]["Leaving"]).ToString("dd/MMM/yyyy");

                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
                SmValueDt.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }

        //changes for V2 add Headquarter joining date and leaving date--- 14-07-2021 anurag
        private void InsertSalesPersons()
        {
            try
            {
                string str = @"select Count(*) from MastLogin where Name='" + txtusername.Value.ToUpper() + "'";
                string active = "0";
                int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                string DesgID1 = ddlRole.SelectedItem.Value;
                if (val == 0)
                {
                    if (chkIsActive.Checked)
                        active = "1";
                    if (active == "0")
                        if (string.IsNullOrEmpty(BlockReason.Value))
                        {
                            divblock.Attributes.Remove("class");
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Blocked Reason');", true); return;
                        }
                    int retsave = usall.Insert(txtusername.Value, "12345678", Email.Value, Convert.ToInt32(ddlRole.SelectedItem.Value), chkIsActive.Checked, ddldept.SelectedItem.Value, ddldesg.SelectedItem.Value, SMName.Value, Convert.ToDecimal(0), SyncId.Value);

                    if (retsave == -2)
                    { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate EmpSyncId Exists');", true); return; }
                    if (retsave == -3)
                    { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email Id Exists');", true); return; }
                    if (retsave > 0)
                    {
                        //if (SyncId.Value == "")
                        //{
                        //    string syncid = "update MastLogin set EmpSyncId='" + retsave + "' where Id=" + retsave + "";
                        //    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                        //}

                        // Entry to mastsalesrep
                        string UserID = retsave.ToString(), ReportTo = ddlUnderSales.SelectedItem.Value, DeptID = ddldept.SelectedItem.Value, DesgID = ddldesg.SelectedItem.Value,
                                RoleID = ddlRole.SelectedItem.Value, EmailID = Email.Value, Employee = SMName.Value, Rescentre = "None", CityID = ddlcity.SelectedItem.Value;

                        string SalesPerType = ddlSalesPerType.SelectedItem.Value != null ? ddlSalesPerType.SelectedItem.Value : "";
                        int retval = SP.InsertSalespersons(SMName.Value, Pin.Value, SalesPerType, DeviceNo.Value, "0", active, Address1.Value, Address2.Value, CityID, EmailID, Mobile.Value, Remarks.Value, RoleID, Convert.ToInt32(UserID), SyncId.Value, ReportTo, Convert.ToInt32(0), Convert.ToInt32(DeptID), Convert.ToInt32(DesgID), DOB.Text, " ", Rescentre, BlockReason.Value, Convert.ToInt32(Settings.Instance.UserID), Employee, chkhomecity.Checked, Convert.ToInt32(0), chkmobAccess.Checked, txtfromtime.Value, txttotime.Value, txtrecordinterval.Value, txtuploadinterval.Value, txtaccuracy.Value, ddlpushnotification.SelectedValue, ddlbattery.SelectedValue, "0", "0", Convert.ToBoolean(1), Convert.ToBoolean(1), "N", "Y", 0, "Y", "Y", "", "", Convert.ToInt32(ddlHeadquarter.SelectedValue), txtJoin.Text);
                        if (active == "1")
                        {
                            string updLoginQry = @"update MastLogin set Active=1 where Id=" + UserID + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updLoginQry);
                        }
                        else
                        {
                            string updLoginQry = @"update MastLogin set Active=0 where Id=" + UserID + "";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updLoginQry);
                        }
                        if (retval == -1)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SalesPersons Exists');", true);
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                            SMName.Focus();
                        }
                        else if (retval == -2)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true); DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                            SyncId.Focus();
                        }
                        else if (retval == -3)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Device No Exists');", true); DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                            DeviceNo.Focus();
                        }
                        else if (retval == -4)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email ID Exists');", true); DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                            Email.Focus();
                        }
                        else if (retval == -5)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Mobile No Exists');", true); DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "delete from mastlogin where id=" + Convert.ToInt32(UserID) + "");
                            Mobile.Focus();
                        }
                        else
                        {
                            if (SyncId.Value == "")
                            {
                                string syncid = "update Mastsalesrep set SyncId='" + retval + "' where SMId=" + retval + "";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);

                                string empsyncid = "update MastLogin set EmpSyncId='" + retval + "' where Id=" + UserID + "";
                                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, empsyncid);
                            }

                            string mastenviro = "select * from mastenviro";
                            DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                            if (dtenviro.Rows.Count > 0)
                            {
                                if (chkmobAccess.Checked == true)
                                {
                                    if (upd.Insert_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), Mobile.Value, SMName.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, "FFMS"))
                                    {

                                    }
                                    else
                                    {

                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;

                                    }
                                }
                                if (chkmanager.Checked == true)
                                {
                                    if (upd.Insert_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), Mobile.Value, SMName.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, "CRM MANAGER"))
                                    {
                                        int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set Managerapp='" + chkmanager.Checked + "' where SMId=" + retval + "");
                                    }
                                    else
                                    {

                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;

                                    }

                                }
                                if (chkDMT.Checked == true)
                                {
                                    if (upd.Insert_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), Mobile.Value, SMName.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, "TRACKER"))
                                    {
                                        int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set DMTApp='" + chkDMT.Checked + "' where SMId=" + retval + "");
                                    }
                                    else
                                    {

                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;

                                    }
                                }
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! Enviro Table Have No Data');", true); return;
                            }
                            if (FileUpload1.PostedFile != null)
                            {
                                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                                string extension = Path.GetExtension(FileName);
                                if (!string.IsNullOrEmpty(extension))
                                {
                                    string filepath = "SalespersonImages/" + UserID + '-' + FileName;
                                    FileInfo file1 = new FileInfo(filepath);
                                    if (file1.Exists)// to check whether file exist or not ,if exist rename it
                                    {
                                        file1.Delete();
                                    }
                                    FileUpload1.SaveAs(Server.MapPath(filepath));
                                    string qry = @"update mastsalesrep set SpImagepath='" + filepath + "' where UserID=" + UserID + "";
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, qry);
                                }
                            }
                            ClearControls();
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            SMName.Focus();
                            this.BindReportTo();
                        }
                    }
                    else
                    { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exist');", true); }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //changes for V2 add Headquarter joining date and leaving date--- 14-07-2021 anurag
        private void UpdateSalesPersons()
        {
            string active = "0";
            if (chkIsActive.Checked)
                active = "1";
            if (active == "0")
            {

                if (string.IsNullOrEmpty(BlockReason.Value))
                {
                    divblock.Attributes.Remove("class");
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Blocked Reason');", true); return;
                }
            }

            string userid = UpdateMastLogin();//update entry to mastlogin
            string UserID = userid, ReportTo = ddlUnderSales.SelectedItem.Value,
                DeptID = ddldept.SelectedItem.Value, DesgID = ddldesg.SelectedItem.Value, RoleID = ddlRole.SelectedItem.Value, EmailID = Email.Value, Employee = SMName.Value,
                           Rescentre = "None", CityID = ddlcity.SelectedItem.Value;

            string SalesPerType = ddlSalesPerType.SelectedItem.Value != null ? ddlSalesPerType.SelectedItem.Value : "";
            //int retval = SP.InsertSalespersons(SMName.Value, Pin.Value, SalesPerType, DeviceNo.Value, "0", active, Address1.Value, Address2.Value, CityID, EmailID, Mobile.Value, Remarks.Value, RoleID, Convert.ToInt32(UserID), SyncId.Value, ReportTo, Convert.ToInt32(0), Convert.ToInt32(DeptID), Convert.ToInt32(DesgID), DOB.Text, " ", Rescentre, BlockReason.Value, Convert.ToInt32(Settings.Instance.UserID), Employee, chkhomecity.Checked, Convert.ToInt32(0));

            int retval = SP.UpdateSalespersons(Convert.ToInt32(ViewState["SMId"]), SMName.Value, Pin.Value, SalesPerType, DeviceNo.Value, "0", active, Address1.Value, Address2.Value, CityID, EmailID, Mobile.Value, Remarks.Value, RoleID, Convert.ToInt32(UserID), SyncId.Value, ReportTo, Convert.ToInt32(0), Convert.ToInt32(DeptID), Convert.ToInt32(DesgID), DOB.Text, DOA.Text, Rescentre, BlockReason.Value, Convert.ToInt32(Settings.Instance.UserID), Employee, chkhomecity.Checked, Convert.ToInt32(0), chkmobAccess.Checked, txtfromtime.Value, txttotime.Value, txtrecordinterval.Value, txtuploadinterval.Value, txtaccuracy.Value, ddlpushnotification.SelectedValue, ddlbattery.SelectedValue, "0", "0", Convert.ToBoolean(1), Convert.ToBoolean(1), "N", "Y", 0, "Y", "Y", "", "", Convert.ToInt32(ddlHeadquarter.SelectedValue), txtJoin.Text, txtLeav.Text);


            //            string  FromTime,string ToTime,string Interval,string Uploadinterval,string Accuracy,string Sendpushntification,string BatteryRecord,
            //string groupcode,string retryinterval,string gpsloc,bool mobileloc,string sys_flag,string Alarm,int alarmduration,string sendsms,string sendsmsperson,string lat,string longi

            //Update MastLogin
            if (active == "1")
            {
                string updLoginQry1 = @"update MastLogin set Active=1 where Id=" + UserID + "";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updLoginQry1);
            }
            else
            {
                string updLoginQry1 = @"update MastLogin set Active=0 where Id=" + UserID + "";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updLoginQry1);
            }
            //End

            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SalesPersons Exists');", true);
                SMName.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else if (retval == -3)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Device No Exists');", true);
                DeviceNo.Focus();
            }
            else if (retval == -4)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email ID Exists');", true);
                Email.Focus();
            }
            else if (retval == -5)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Mobile No Exists');", true);
                Mobile.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update Mastsalesrep set SyncId='" + retval + "' where SMId=" + ViewState["SMId"] + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);

                    string empsyncid = "update MastLogin set EmpSyncId='" + retval + "' where Id=" + UserID + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, empsyncid);
                }
                string mastenviro = "select * from mastenviro";
                DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                if (dtenviro.Rows.Count > 0)
                {
                    //if (chkmobAccess.Checked == true)
                    //{
                    if (upd.Update_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), DeviceNo.Value, SMName.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, active, hidolddeviceforlicence.Value, chkmobAccess.Checked, "FFMS", hdnoldMobile.Value))
                    {
                        if (active == "1")
                        {
                            int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set mobileaccess='" + chkmobAccess.Checked + "' where SMId=" + retval + "");
                        }
                        else
                        {
                            int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set mobileaccess='" + false + "' where SMId=" + retval + "");
                        }
                    }
                    else
                    {

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;

                    }
                    //}
                    //if (chkmanager.Checked == true)
                    //{
                    if (upd.Update_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), DeviceNo.Value, SMName.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, active, hidolddeviceforlicence.Value, chkmanager.Checked, "CRM MANAGER", hdnoldMobile.Value))
                    {
                        if (active == "1")
                        {
                            int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set Managerapp='" + chkmanager.Checked + "' where SMId=" + retval + "");
                        }
                        else
                        {
                            int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set Managerapp='" + false + "' where SMId=" + retval + "");
                        }
                    }
                    else
                    {

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;

                    }
                    //}
                    //if (chkDMT.Checked == true)
                    //{
                    if (upd.Update_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), DeviceNo.Value, SMName.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, active, hidolddeviceforlicence.Value, chkDMT.Checked, "TRACKER", hdnoldMobile.Value))
                    {
                        if (active == "1")
                        {
                            int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set DMTApp='" + chkDMT.Checked + "' where SMId=" + retval + "");
                        }
                        else
                        {
                            int cnt = DbConnectionDAL.GetIntScalarVal("update Mastsalesrep set DMTApp='" + false + "' where SMId=" + retval + "");
                        }
                    }
                    else
                    {

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;

                    }
                    //}
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! Enviro Table Have No Data');", true); return;
                }

                if (FileUpload1.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        string filepath = "SalespersonImages/" + UserID + '-' + FileName;
                        FileInfo file1 = new FileInfo(filepath);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload1.SaveAs(Server.MapPath(filepath));
                        string qry = @"update mastsalesrep set SpImagepath='" + filepath + "' where UserID=" + UserID + "";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, qry);
                    }
                }
                ClearControls();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                SMName.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
                this.BindReportTo();

                dtenviro.Dispose();
            }
        }

        private string UpdateMastLogin()
        {
            string retsaveforreturn = "0";
            try
            {
                string RoleID = Request.Form[HiddenRoleID.UniqueID], DeptID = Request.Form[HiddenDeptID.UniqueID], DesnID = Request.Form[HiddenDesID.UniqueID];
                //  if (UserName.Value != "")
                {
                    string userid = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select userid from mastsalesrep where smid='" + Convert.ToInt32(ViewState["SMId"]) + "'"));
                    string strupd = @"select Count(*) from MastLogin where Name='" + txtusername.Value.ToUpper() + "' and Id<>" + Convert.ToInt32(userid) + "";

                    int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));
                    if (valupd == 0)
                    {
                        int retsave = usall.Update(Convert.ToInt32(userid), txtusername.Value.ToUpper(), "", Email.Value, Convert.ToInt32(ddlRole.SelectedItem.Value), chkIsActive.Checked, ddldept.SelectedItem.Value, ddldesg.SelectedItem.Value, SMName.Value, Convert.ToDecimal(0), SyncId.Value);
                        if (retsave == -2)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate EmpSyncId Exists');", true);
                        }
                        if (retsave == -3)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email Id Exists');", true);
                        }
                        if (retsave == 1)
                        {
                            if (SyncId.Value == "")
                            {
                                //string syncid = "update MastLogin set EmpSyncId='" + userid + "' where Id=" + userid + "";
                                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                            }
                            // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                        }
                        retsaveforreturn = userid;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return retsaveforreturn;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlSalesPerType.Enabled == true)
                {
                    if (ddlSalesPerType.SelectedIndex == 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Salesperson Type');", true);
                        return;
                    }
                }
                if (btnSave.Text == "Update")
                {
                    UpdateSalesPersons();
                }
                else
                {
                    InsertSalesPersons();

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/SalesPersons.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = SP.delete(Convert.ToString(ViewState["SMId"]));
                if (retdel == 1)
                {
                    string mastenviro = "select * from mastenviro";
                    DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                    if (dtenviro.Rows.Count > 0)
                    {
                        if (DeleteLicense(dtenviro.Rows[0]["compcode"].ToString(), DeviceNo.Value, SMName.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, "0", hidolddeviceforlicence.Value) != "Yes")
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Deactivated License');", true); return;
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! Enviro Table Have No Data');", true); return;
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    HiddenbtnTest.Value = "Save";
                    SMName.Focus();

                    dtenviro.Dispose();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    SMName.Focus();
                }
            }
            else
            {
            }
        }
        private void ClearControls()
        {
            //   System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "ClearControls", "ClearControls();", true);
            HiddenReportToID.Value = string.Empty;
            HiddenResCentre.Value = string.Empty;
            HiddenCityID.Value = string.Empty;
            HiddenDeptID.Value = string.Empty;
            HiddenDesID.Value = string.Empty;
            HiddenUserID.Value = string.Empty;
            HiddenRoleID.Value = string.Empty;
            HiddenEmailID.Value = string.Empty;
            HiddenEmployee.Value = string.Empty;
            SMName.Value = string.Empty;
            Email.Value = string.Empty;
            Mobile.Value = string.Empty;
            Pin.Value = string.Empty;
            Address1.Value = string.Empty;
            Address2.Value = string.Empty;
            DSRAllowDays.Value = string.Empty;
            ddldept.SelectedIndex = 0;
            ddlSalesPerType.SelectedIndex = 0;
            DOA.Text = string.Empty;
            DOB.Text = string.Empty;
            Remarks.Value = string.Empty;
            SyncId.Value = string.Empty;
            chkIsActive.Checked = true;
            BlockReason.Value = "";
            DeviceNo.Value = "";
            txtmeetallowdays.Value = "";
            EmpName.Value = "";
            txtusername.Value = null;
            ddlSalesPerType.SelectedIndex = 0;
            ddlRole.SelectedIndex = 0;
            ddlUnderSales.SelectedIndex = 0;
            ddldesg.SelectedIndex = 0;
            ddlcity.SelectedIndex = 0;
            ddlHeadquarter.SelectedIndex = 0;
            txtusername.Attributes.Remove("readonly");
            imgpreview.Style.Add("display", "none");
            chkmobAccess.Checked = false;
            chkmanager.Checked = false;
            chkDMT.Checked = false;
            txtJoin.Text = string.Empty;
            txtLeav.Text = string.Empty;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            CallDropdowns();

            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void ddlusers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlusers.SelectedIndex > 0)
                {  //Ankita - 20/may/2016- (For Optimization)
                    //string str = "select * from mastlogin where Id=" + ddlusers.SelectedValue + "";
                    string str = "select RoleId,DeptId,DesigId,EmpName,Email from mastlogin where Id=" + ddlusers.SelectedValue + "";
                    DataTable dtbl = DbConnectionDAL.getFromDataTable(str);
                    if (dtbl.Rows.Count > 0)
                    {
                        ddlRole.SelectedValue = dtbl.Rows[0]["RoleId"].ToString();
                        ddldept.SelectedValue = dtbl.Rows[0]["DeptId"].ToString();
                        ddldesg.SelectedValue = dtbl.Rows[0]["DesigId"].ToString();
                        EmpName.Value = dtbl.Rows[0]["EmpName"].ToString();
                        Email.Value = dtbl.Rows[0]["Email"].ToString();
                    }

                    dtbl.Dispose();
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]

        public static AstralFFMS.Models.Models.SalesPersonDetails[] FillDetailUser(int UserId)
        {
            List<AstralFFMS.Models.Models.SalesPersonDetails> ListSDetails = new List<AstralFFMS.Models.Models.SalesPersonDetails>();
            AstralFFMS.Models.Models.SalesPersonDetails SDetails = new AstralFFMS.Models.Models.SalesPersonDetails();
            int i = 0;
            if (UserId > 0)
            {  //Ankita - 20/may/2016- (For Optimization)
                //string str = "select * from mastlogin where Id=" + ddlusers.SelectedValue + "";
                string str = "select RoleId,DeptId,DesigId,EmpName,Email from mastlogin where Id=" + UserId + "";
                DataTable dtbl = DbConnectionDAL.getFromDataTable(str);
                if (dtbl.Rows.Count > 0)
                {
                    SDetails.Role = dtbl.Rows[0]["RoleId"].ToString();
                    SDetails.Department = dtbl.Rows[0]["DeptId"].ToString();
                    SDetails.Designation = dtbl.Rows[0]["DesigId"].ToString();
                    SDetails.Employee = dtbl.Rows[0]["EmpName"].ToString();
                    SDetails.Email = dtbl.Rows[0]["Email"].ToString();
                    ListSDetails.Add(SDetails);
                }

                dtbl.Dispose();
            }
            //string Role = ListSDetails[0].Role;
            return ListSDetails.ToArray();

        }
        ////////////////////FillSMSControls
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static AstralFFMS.Models.Models.FillSalePersonControls[] FillSMControls_Web(int SmId)
        {
            List<AstralFFMS.Models.Models.FillSalePersonControls> ListFillSDetail = new List<AstralFFMS.Models.Models.FillSalePersonControls>();
            AstralFFMS.Models.Models.FillSalePersonControls FillSDetails = new AstralFFMS.Models.Models.FillSalePersonControls();
            //Username.Disabled = true;
            //string Smquery = @"select * from MastSalesRep msr left join MastLogin Ml On Msr.UserId=Ml.id where SMId=" + SmId;
            string Smquery = @"select *,isnull(AllowChangeCity,0)as AllowChangeCity1 from MastSalesRep msr left join MastLogin Ml On Msr.UserId=Ml.id where SMId=" + SmId;

            DataTable SmValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Smquery);
            if (SmValueDt.Rows.Count > 0)
            {

                FillSDetails.SMName = SmValueDt.Rows[0]["SMName"].ToString();
                FillSDetails.SyncId = SmValueDt.Rows[0]["SyncId"].ToString();

                if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["DOA"].ToString()))
                    FillSDetails.DOA = Convert.ToDateTime(SmValueDt.Rows[0]["DOA"]).ToString("dd/MMM/yyyy");
                //DOA.Text = (DateTime.Parse(SmValueDt.Rows[0]["DOA"].ToString())).ToString();
                FillSDetails.Address1 = SmValueDt.Rows[0]["Address1"].ToString();
                FillSDetails.Address2 = SmValueDt.Rows[0]["Address2"].ToString();
                FillSDetails.EmpName = SmValueDt.Rows[0]["EmpName"].ToString();
                FillSDetails.Pin = SmValueDt.Rows[0]["Pin"].ToString();
                FillSDetails.Mobile = SmValueDt.Rows[0]["Mobile"].ToString();
                FillSDetails.DeviceNo = SmValueDt.Rows[0]["DeviceNo"].ToString();
                if (!string.IsNullOrEmpty(SmValueDt.Rows[0]["DOB"].ToString()))
                    FillSDetails.DOB = Convert.ToDateTime(SmValueDt.Rows[0]["DOB"]).ToString("dd/MMM/yyyy");
                FillSDetails.Email = SmValueDt.Rows[0]["Email"].ToString();
                FillSDetails.Remarks = SmValueDt.Rows[0]["Remarks"].ToString();
                FillSDetails.DSRAllowDays = SmValueDt.Rows[0]["DSRAllowDays"].ToString();
                FillSDetails.RoleID = SmValueDt.Rows[0]["RoleID"].ToString();
                FillSDetails.DesID = SmValueDt.Rows[0]["DesigId"].ToString();
                FillSDetails.DeptID = SmValueDt.Rows[0]["DeptId"].ToString();
                FillSDetails.ReportToID = SmValueDt.Rows[0]["UnderId"].ToString();

                //  BindResCentre(Convert.ToInt32(SmValueDt.Rows[0]["ResCenId"]));
                FillSDetails.ResCentre = SmValueDt.Rows[0]["ResCenId"].ToString();
                FillSDetails.CityID = SmValueDt.Rows[0]["CityId"].ToString();
                FillSDetails.UserID = SmValueDt.Rows[0]["UserId"].ToString();
                FillSDetails.SalesRepType = SmValueDt.Rows[0]["SalesRepType"].ToString();
                FillSDetails.EmpName = SmValueDt.Rows[0]["EmpName"].ToString();
                if (Convert.ToBoolean(SmValueDt.Rows[0]["AllowChangeCity1"]) == true)
                { FillSDetails.chkhomecity = "true"; }
                else { FillSDetails.chkhomecity = "false"; }
                if (Convert.ToBoolean(SmValueDt.Rows[0]["Active"]) == true)
                {
                    FillSDetails.chkIsActive = "true"; FillSDetails.BlockReason = "";
                }
                else
                {
                    FillSDetails.divblock = "class";
                    FillSDetails.BlockReason = SmValueDt.Rows[0]["BlockReason"].ToString();
                    FillSDetails.chkIsActive = "false";
                }
                FillSDetails.save = "Update";
                FillSDetails.delete = "true";
                ListFillSDetail.Add(FillSDetails);
            }
            return ListFillSDetail.ToArray();
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string roletype = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select roletype from mastrole where roleid='" + Convert.ToInt32(ddlRole.SelectedItem.Value) + "'"));
            if ((roletype == "AreaIncharge") || (roletype == "CityHead") || (roletype == "DistrictHead") || (roletype == "RegionHead") || (roletype == "StateHead"))
            {
                //divsalestype.Visible = true;
                ddlSalesPerType.Enabled = true;
                //dvchk.Visible = false;
                chkmanager.Visible = false;
                lblmanager.Visible = false;
                if ((roletype == "CityHead") || (roletype == "DistrictHead") || (roletype == "RegionHead") || (roletype == "StateHead"))
                {
                    //dvchk.Visible = true;
                    chkmanager.Visible = true;
                    lblmanager.Visible = true;

                }

            }
            else
            {
                //divsalestype.Visible = false;
                ddlSalesPerType.Enabled = false;
                //dvchk.Visible = false;
                chkmanager.Visible = false;
                lblmanager.Visible = false;
                ddlSalesPerType.SelectedIndex = 0;
            }
        }

        //public bool AddPersonLicence(string Name, string DeviceID)
        //{
        //    WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
        //    string val = DMT.InsertPersonLicense(Settings.Instance.CompCode, DeviceID, Name, Settings.Instance.CompUrl);
        //    if (val == "1")
        //        return true;
        //    else
        //        return false;
        //}

        public string InsertLicense(string compcode, string DeviceID, string PersonName, string URL, string mob, string product)
        {


            string result = "No";
            try
            {
                //  string product = DbConnectionDAL.GetStringScalarVal("select isnull(productcode,'') from mastenviro");
                string insqry = "SELECT count(*) FROM LineMaster WHERE Mobile='" + mob + "' and upper(Product)='" + product + "' and compcode='" + compcode + "'";
                int count = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
                if (count < 1)
                {
                    string insLine = "insert into LineMaster (CompCode,DeviceID,Active,LineDate,PersonName,Product,URL,mobile)" +
                                            "values('" + compcode + "','" + DeviceID + "','Y','" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "','" + PersonName + "','" + product + "','" + URL + "','" + mob + "')";

                    DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insLine);
                    result = "Yes";
                }
            }
            catch (Exception ex) { ex.ToString(); }
            return result;
        }

        public string UpdateLicense(string compcode, string DeviceID, string PersonName, string URL, string mob, string active, string olddevice, bool mobileaccess, string product)
        {
            string result = "No";

            try
            {
                // string product = DbConnectionDAL.GetStringScalarVal("select isnull(productcode,'') from mastenviro");
                string insqry = "SELECT count(*) FROM LineMaster WHERE Mobile='" + hdnoldMobile.Value + "' and upper(Product)='" + product + "' and compcode='" + compcode + "'";
                int count = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
                if (mobileaccess == true)
                {
                    if (count < 1)
                    {
                        string insLine = "insert into LineMaster (CompCode,DeviceID,Active,LineDate,PersonName,Product,URL,mobile)" +
                                        "values('" + compcode + "','" + DeviceID + "','Y','" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "','" + PersonName + "','" + product + "','" + URL + "','" + mob + "')";

                        DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insLine);
                        result = "Yes";
                    }
                    else
                    {
                        if (active == "1")
                        {
                            active = "Y";
                            insqry = " update linemaster set Personname='" + PersonName + "', mobile='" + mob + "' ,active='" + active + "',deviceid='" + DeviceID + "' ,validdate=null,CompCode='" + compcode + "',URL='" + URL + "'   where Mobile='" + hdnoldMobile.Value + "' and upper(Product)='" + product + "' and    compcode='" + compcode + "'";

                        }
                        else
                        {
                            active = "N";
                            insqry = " update linemaster set Personname='" + PersonName + "', mobile='" + mob + "' ,active='" + active + "',deviceid='" + DeviceID + "' ,validdate='" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "' where Mobile='" + hdnoldMobile.Value + "' and upper(Product)='" + product + "' and compcode='" + compcode + "'";

                        }
                        DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insqry);
                        result = "Yes";
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        active = "N";
                        insqry = " update linemaster set Personname='" + PersonName + "', mobile='" + mob + "' ,active='" + active + "',deviceid='" + DeviceID + "' ,validdate='" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "' where Mobile='" + hdnoldMobile.Value + "' and upper(Product)='" + product + "' and compcode='" + compcode + "'";
                        DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insqry);
                    }
                    result = "Yes";
                }
            }
            catch (Exception ex) { ex.ToString(); }
            return result;
        }

        public string DeleteLicense(string compcode, string DeviceID, string PersonName, string URL, string mob, string active, string olddevice)
        {
            string result = "No";
            try
            {
                string insqry = "";

                if (active == "1")
                {
                    active = "Y";
                    insqry = " update linemaster set Personname='" + PersonName + "', mobile='" + mob + "' ,active='" + active + "',deviceid='" + DeviceID + "' ,validdate=null  where Mobile='" + hdnoldMobile.Value + "' and upper(Product)='FFMS' and    compcode='" + compcode + "'";
                }
                else
                {
                    active = "N";
                    insqry = " update linemaster set Personname='" + PersonName + "', mobile='" + mob + "' ,active='" + active + "',deviceid='" + DeviceID + "' ,validdate='" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "' where Mobile='" + hdnoldMobile.Value + "' and upper(Product)='FFMS' and compcode='" + compcode + "'";
                }

                DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insqry);
                result = "Yes";
            }
            catch (Exception Ex) { Ex.ToString(); }
            return result;
        }

        private void filldt()
        {
            string str = "", straddqry = ""; StringBuilder sb = new StringBuilder(); string col = "";
            if (_exportp == true)
            {
                str = @"select ROW_NUMBER() over (order by msr.SMID desc ) as [Sr. No],msr.SMName as [Sales Person Name],msr.SalesRepType as [Sales Person Type],Msr1.SmName as [Reporting Person],DepName as Department,DesName as Designation,Mg.Name as Grade,msr.DeviceNo as [Device No],msr.Mobile as [Mobile No],stt.AreaName as State,Ma.Areaname As City,msr.Email as [Email Id],Ml.Name as [User Name],RoleName as Role,msr.SyncId,case msr.Active when 1 then 'Yes' else 'No' end as Active 
from MastSalesRep Msr inner join MastSalesRep Msr1 on Msr.underId=Msr1.SMID left join MastLogin Ml On Msr.UserId=Ml.id left join MastRole Mr on Mr.RoleId=Ml.RoleId left join MastDepartment Md on Ml.DeptId=Md.Depid left join MastDesignation Mdsg on Ml.DesigId=Mdsg.DesId left join MastGrade Mg on Msr.GradeId=Mg.Id left join Mastarea Ma on Msr.CityId=Ma.AreaId left join MastArea dst on dst.AreaId=Ma.UnderId left join MastArea stt on stt.AreaId=dst.UnderId left join MastResCentre Mrc On Msr.ResCenId=Mrc.ResCenId where msr.SMName <> '.'";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                foreach (DataColumn dc in dt.Columns)
                {
                    col += dc.ColumnName + ",";
                }

                sb.AppendLine(col.Trim(','));
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                      string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sb.AppendLine(string.Join(",", fields));
                }

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=SalesPersonMaster.csv");
                Response.Write(sb.ToString());

                sb.Clear();
                dt.Dispose();
                Response.End();
            }
            else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You do not have permission to Export');", true);


        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            filldt();
        }

        [WebMethod(EnableSession = true)]
        public static string PopulateState()
        {
            string str = "";
            str = "select AreaName Text ,AreaID Value from mastarea where   AreaType='State' and Active='1' order by AreaName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string PopulateSmname(string SMID)
        {
            string str = "";
            str = "select SMName from MastSalesRep where  Smid='" + SMID + "'";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string PopulateCityByMultiState(string StateID)
        {
            DataTable dt = new DataTable();
            if (StateID != "")
            {
                string strQ = "select mct.AreaId Value,mct.AreaName Text from MastArea mct left join mastarea mdt on mdt.AreaId=mct.UnderId where  mct.areatype='CITY' and mdt.areatype='DISTRICT' and mdt.UnderId in (" + StateID + ")";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string PopulateAreaByMultiState(string CityID)
        {
            DataTable dt = new DataTable();
            if (CityID != "")
            {
                string strQ = "select AreaName Text ,AreaID Value from mastarea where AreaType='Area' and Active='1' and UnderId in (" + CityID + ") order by AreaName";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string GetDetail(string StateId, string Cityid, string Area, string SMID)
        {
            string areaQry = "";
            string smid = SMID;
            string strStID = "";
            string _whr = "";
            //foreach (ListItem item in StateId.Items)
            //{
            //    if (item.Selected)
            //    {
            //        strStID += item.Value + ",";
            //    }
            //}
            strStID = StateId.TrimStart(',').TrimEnd(',');

            //if (strStID == "")
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select State.');", true);
            //    return;
            //}

            string strStCity = "";
            //foreach (ListItem item in lstCity.Items)
            //{
            //    if (item.Selected)
            //    {
            //        strStCity += item.Value + ",";
            //    }
            //}
            strStCity = Cityid.TrimStart(',').TrimEnd(',');

            string strStArea = "";
            //foreach (ListItem item in lstArea.Items)
            //{
            //    if (item.Selected)
            //    {
            //        strStArea += item.Value + ",";
            //    }
            //}
            strStArea = Area.TrimStart(',').TrimEnd(',');

            switch (strStID)
            {
                case "":
                    _whr = " ";
                    break;
                default:
                    switch (strStCity)
                    {
                        case "":
                            switch (strStArea)
                            {
                                case "":
                                    _whr = "state.AreaId in (" + strStID + ") and  ";
                                    break;
                            }
                            break;
                        default:
                            switch (strStArea)
                            {
                                case "":
                                    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and ";
                                    break;
                                default:
                                    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and area.AreaId IN (" + strStArea + ")  and";
                                    break;

                            }
                            break;
                    }
                    break;
            }

            /*SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId,case when (select Id from MastLink where PrimCode=286 and LinkCode in (area.AreaId) and ECode='SA') IS NOT NULL then '1' else '0'end as LinkCde, ISNULL(area.AreaName, '') AS areaName
                        FROM  dbo.MastArea AS state  LEFT OUTER JOIN
                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' 
						 --left join MastLink as link on area.AreaId=link.LinkCode
                         WHERE ( state.AreaId in (8,9,10,11,12,14,13,16,15,17,18,20,19,22,21,23,24,25,29,26,28,27,30,32,31,33,35,34,36,37,39,38,40,42,41) and   state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName*/

            areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId,case when (select Id from MastLink where PrimCode in (" + SMID + ") and LinkCode in (area.AreaId) and ECode='SA') IS NOT NULL then 'true' else 'false' end as LinkCde, ISNULL(area.AreaName, '') AS areaName FROM  dbo.MastArea AS state  LEFT OUTER JOIN dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' WHERE ( " + _whr + " state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 ) order by LinkCde desc, stateName,districtName,cityName,areaName ";
            //DataTable areaStatedt = DbConnectionDAL.GetDataTable(CommandType.Text, areaQry);

            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, areaQry);
            return JsonConvert.SerializeObject(dtItem);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string areaQry = "";

            string strStID = "";
            string _whr = "";
            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                {
                    strStID += item.Value + ",";
                }
            }
            strStID = strStID.TrimStart(',').TrimEnd(',');

            if (strStID == "")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select State.');", true);
                return;
            }

            string strStCity = "";
            foreach (ListItem item in lstCity.Items)
            {
                if (item.Selected)
                {
                    strStCity += item.Value + ",";
                }
            }
            strStCity = strStCity.TrimStart(',').TrimEnd(',');

            string strStArea = "";
            foreach (ListItem item in lstArea.Items)
            {
                if (item.Selected)
                {
                    strStArea += item.Value + ",";
                }
            }
            strStArea = strStArea.TrimStart(',').TrimEnd(',');





            // if (strStID != "" && ddlregion.SelectedIndex > 0 && strStArea != "")           
            // {

            // int region = Convert.ToInt32(ddlregion.SelectedValue);

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'                        
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and area.AreaId IN (" + strStArea + ") and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";


            //}

            // else if (strStID != "" && ddlregion.SelectedIndex > 0 && strStCity!="")

            //{

            // int region = Convert.ToInt32(ddlregion.SelectedValue);              

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'                        
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and city.AreaId IN (" + strStCity + ") and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";

            //}
            //else if (strStID != "" && ddlregion.SelectedIndex > 0)

            // {
            //int state = Convert.ToInt32(ddlstate.SelectedValue);
            //int region = Convert.ToInt32(ddlregion.SelectedValue);

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' LEFT OUTER JOIN
            //                         dbo.MastArea AS beat ON area.AreaId = beat.UnderId AND beat.AreaType = 'beat'
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 and beat.Active=1) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'
            //                       
            //                         WHERE        (country.AreaType = 'country' and state.AreaId in (" + strStID + ") and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";

            // }
            //            else if (ddlregion.SelectedIndex > 0)
            //            {              
            //                int region = Convert.ToInt32(ddlregion.SelectedValue);

            ////                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            ////                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            ////                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            ////                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            ////                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            ////                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            ////                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' LEFT OUTER JOIN
            ////                         dbo.MastArea AS beat ON area.AreaId = beat.UnderId AND beat.AreaType = 'beat'
            ////                         WHERE        (country.AreaType = 'country' and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 and beat.Active=1) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area'                        
            //                         WHERE        (country.AreaType = 'country' and region.AreaId=" + region + " and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1) order by stateName,districtName,cityName,areaName";


            //            }
            //else
            //{
            //gvPartyData.DataSource = null;
            //gvPartyData.DataBind();
            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' LEFT OUTER JOIN
            //                         dbo.MastArea AS beat ON area.AreaId = beat.UnderId AND beat.AreaType = 'beat'
            //                         WHERE        (country.AreaType = 'country' and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1
            //						 and beat.Active=1) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM            dbo.MastArea AS country LEFT OUTER JOIN
            //                         dbo.MastArea AS region ON country.AreaId = region.UnderId AND region.AreaType = 'region' LEFT OUTER JOIN
            //                         dbo.MastArea AS state ON region.AreaId = state.UnderId AND state.AreaType = 'state' LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' 
            //                        
            //                         WHERE (country.AreaType = 'country' and region.Active=1 and state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 ) order by stateName,districtName,cityName,areaName";

            //                areaQry = @" SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
            //                        FROM  dbo.MastArea AS state  LEFT OUTER JOIN
            //                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
            //                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
            //                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' 
            //                         WHERE ( state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 ) order by stateName,districtName,cityName,areaName";


            // }

            //if (strStID != "" && strStCity != "" && strStArea != "")
            //{
            //    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and area.AreaId IN (" + strStArea + ")  and";
            //}
            //else if (strStID != "" && strStCity != "" && strStArea == "")
            //{
            //    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and ";
            //}
            //else if (strStID != "" && strStCity == "" && strStArea == "")
            //{
            //    _whr = "state.AreaId in (" + strStID + ") and  ";
            //}
            //else
            //{
            //    _whr = " ";
            //}

            switch (strStID)
            {
                case "":
                    _whr = " ";
                    break;
                default:
                    switch (strStCity)
                    {
                        case "":
                            switch (strStArea)
                            {
                                case "":
                                    _whr = "state.AreaId in (" + strStID + ") and  ";
                                    break;
                            }
                            break;
                        default:
                            switch (strStArea)
                            {
                                case "":
                                    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and ";
                                    break;
                                default:
                                    _whr = "state.AreaId in (" + strStID + ") and  city.AreaId IN (" + strStCity + ") and area.AreaId IN (" + strStArea + ")  and";
                                    break;

                            }
                            break;
                    }
                    break;
            }

            areaQry = @"SELECT distinct ISNULL(state.AreaName, '') AS stateName, ISNULL(district.AreaName, '')  AS districtName, ISNULL(city.AreaName, '') AS cityName, ISNULL(area.AreaId, 0) AS areaId, ISNULL(area.AreaName, '') AS areaName
                        FROM  dbo.MastArea AS state  LEFT OUTER JOIN
                         dbo.MastArea AS district ON state.AreaId = district.UnderId AND district.AreaType = 'district' LEFT OUTER JOIN
                         dbo.MastArea AS city ON district.AreaId = city.UnderId AND city.AreaType = 'city' LEFT OUTER JOIN
                         dbo.MastArea AS area ON city.AreaId = area.UnderId AND area.AreaType = 'area' 
                         WHERE ( " + _whr + " state.Active=1 and district.Active=1 and city.Active=1 and area.Active=1 ) order by stateName,districtName,cityName,areaName";
            DataTable areaStatedt = DbConnectionDAL.GetDataTable(CommandType.Text, areaQry);

            if (areaStatedt.Rows.Count > 0)
            {
                mainDiv.Style.Add("display", "block");
                rpt.DataSource = areaStatedt;
                rpt.DataBind();
                rpt.Visible = true;
                //btnsave.Visible = true;
                //btncancel.Visible = true;
            }
            else
            {
                mainDiv.Style.Add("display", "block");
                rpt.DataSource = areaStatedt;
                rpt.DataBind();
                //btnsave.Visible = false;
                //btncancel.Visible = false;
            }
        }

        protected void btngrdsve_Click(object sender, EventArgs e)
        {
            string areaIDStr = ""; string strdelete = "";
            string s = HiddenarIds.Value;


            strdelete = "delete from mastlink where Ecode='SA' and PrimCode=" + HiddenSMID.Value + " ";
            DbConnectionDAL.ExecuteQuery(strdelete);
            if (s != "")
            {
                string strStCity = s.TrimStart(',').TrimEnd(',');
                string[] authorsList = strStCity.Split(',');
                foreach (string author in authorsList)
                {
                    //foreach (RepeaterItem item in rpt.Items)
                    //{
                    //    CheckBox chk = (CheckBox)item.FindControl("chkItem");
                    ////HiddenField areaId = (HiddenField)item.FindControl("areaIdHiddenField");
                    //Label areaId = (Label)item.FindControl("lblHF");
                    //if (chk.Checked == true)
                    //{
                    ML.Insert(Convert.ToInt32(HiddenSMID.Value), Convert.ToInt32(author));
                    //    }
                    //}
                    //else
                    //{

                    //}
                }
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Sucessfully!');", true);
            //ddlprimecode.SelectedIndex = 0;
            ClearControls();
        }
    }
}