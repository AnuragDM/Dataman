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
using AstralFFMS.ServiceReferenceDMTracker;
using System.Text;

namespace AstralFFMS
{
    public partial class DistributorMaster : System.Web.UI.Page
    {
        string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
        string area_visibility = "";
        DistributorBAL DB = new DistributorBAL();
        string parameter = "";
        ImportData upd = new ImportData();
        bool _exportp = false; 
        protected void Page_Load(object sender, EventArgs e)
        {
          
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                //string query = "select AreaWiseDistributor from mastenviro  ";
                //string Areawiseddl = DbConnectionDAL.GetScalarValue(CommandType.Text, query).ToString();
                //if(Areawiseddl == "Y")
                //{ Areadiv.Visible = true; }
                ViewState["PartyId"] = parameter;
                FillPartyControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');

            btnExport.CssClass = "btn btn-primary";
            btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            _exportp = Convert.ToBoolean(SplitPerm[4]);

            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
              //  btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
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
                areaVisibility();
                txtDOA.Attributes.Add("readonly", "readonly");
                txtDOB.Attributes.Add("readonly", "readonly");
                chkIsAdmin.Checked = true;
                btnDelete.Visible = false;
                //BindRoles();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["PartyId"] != null)
                {
                    FillPartyControls(Convert.ToInt32(Request.QueryString["PartyId"]));
                }
                else {
                   /// BindCity(0); BindSalemen(0); 
                }
            }
        }
        public void areaVisibility()
        {
            area_visibility = DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreawiseDistributor from mastenviro").ToString();
            if(area_visibility=="Y")
            {
                divarea.Visible = true;
               
            }
            else
            {
                divarea.Visible = false;
                HiddenDistributorArea.Value = "0";
            }

        }
        #region Bind Dropdowns

        private void BindCity(int CityId)
        {
            string strq = "";
            if (Settings.Instance.UserName.ToUpper() == "SA")
            {
                //if (CityId > 0) { strq = "select AreaId,AreaName from MastArea where areatype in ('CITY')  and (Active='1' or Areaid in (" + CityId + "))    order by AreaName"; }
                //else { strq = "select AreaId,AreaName from MastArea where areatype in ('CITY') and Active='1' order by AreaName"; }

                if (CityId > 0) 
                {
                    strq = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and (T.cityAct=1  OR T.cityid=" + CityId + ") ORDER BY cityName"; 
                }
                else
                {
                    strq = "SELECT DISTINCT T.cityid,T.cityName+' - '+T.districtName AS cityName FROM ViewGeo AS T WHERE T.cityid>0 and T.cityAct=1 ORDER BY cityName"; 
                }
            }
            else
            {
                if (CityId > 0)
                {
                    strq = @"SELECT DISTINCT cityid,cityName+' - '+districtName AS cityName FROM ViewGeo  where cityid!=0 and cityAct=1 and cityName!='' and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1)  ORDER BY cityName";
                }
                else
                {
                    strq = @"SELECT DISTINCT cityid,cityName+' - '+districtName AS cityName FROM ViewGeo  where cityid!=0 and (cityAct=1 OR cityid=" + CityId + ") and cityName!='' and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
               " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1)  ORDER BY cityName";
                }
            }
            fillDDLDirect(ddlCity, strq, "cityid", "cityName", 1);
            
        }
        private void BindSalemen(int SMID)
        {
            string strSM = "";
           
                if (SMID > 0) { strSM = "select SMID,SMName from MastSalesRep where Active=1 or SMID in (" + SMID + ")    order by SMName"; }
                else { strSM = "select SMID,SMName from MastSalesRep where Active=1  order by SMName"; }

                fillDDLDirect(ddlsalesperson, strSM, "SMID", "SMName", 1);
        }
        private void BindRoles()
        { //Ankita - 20/may/2016- (For Optimization)
           // string str = "select * from mastrole where roletype='Distributor' order by rolename";
            string str = "select roleid,rolename from mastrole where roletype='Distributor' order by rolename";
            fillDDLDirect(ddlRole, str, "roleid", "rolename", 1);
        }

        private void BindType()
        { 
            string str = "select Text,Value from MastDistributorType order by sort";
            fillDDLDirect(ddltype, str, "Value", "Text", 1);
        }

        private void BindSuperDistributor()
        {
            string condition = "";
            string disttype = HiddenDistType.Value;
            if (disttype == "UNDERSD")
                condition = " and isnull(mp.DistType,'')='SUPERDIST' ";
            else if (disttype == "SUPERDIST")
            {
                condition = " and isnull(mp.DistType,'') in (select value from mastdistributortype where sort < (select sort from mastdistributortype where value ='" + disttype + "') ) ";
            }
            //condition = " and isnull(mp.DistType,'')='C&F' ";
            else if (disttype == "C&F")
            {
                condition = " and isnull(mp.DistType,'') in (select value from mastdistributortype where sort < (select sort from mastdistributortype where value ='" + disttype + "') ) ";
            }

            string strQ = "select mp.PartyId,mp.PartyName + ' - ' + md.Text + ' ( ' + ma.AreaName + ' )' as PartyName from MastParty mp left join MastDistributorType md on md.Value=mp.DistType left join MastArea ma on ma.AreaId=mp.CityId where isnull(mp.PartyDist,0)=1 " + condition;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            if (dt.Rows.Count > 0)
            {
                lstSDParty.DataSource = dt;
                lstSDParty.DataTextField = "PartyName";
                lstSDParty.DataValueField = "PartyId";
                lstSDParty.DataBind();
            }
            else
            {
                DataTable dt1 = new DataTable();
                lstSDParty.DataSource = dt1;
                lstSDParty.DataBind();
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
        #endregion
        private void fillRepeter()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"SELECT *,city.AreaName AS city,Ml.Name as UserName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId,Mr.RoleName FROM MastParty Mp LEFT JOIN MastArea city ON Mp.CityId=city.AreaId Left Join MastLogin Ml On Mp.UserId=Ml.Id Left Join MastRole Mr On Ml.RoleId=Mr.RoleId WHERE Mp.PartyDist=1 and PartyName<>'.' ";

            //(case when ISNULL(DistType,'')='SUPERDIST' then 'Super Distributor' when ISNULL(DistType,'')='DIST' then 'Distributor' when ISNULL(DistType,'')='UNDERSD' then 'Under Super Distributor' end)
            string str = @"SELECT Mp.PartyId,Mp.PartyName,Mp.Mobile,isnull(mp.GSTINNo,'') as GSTIN,stt.AreaName as StateName,city.AreaName AS city,area.AreaName AS Area,Ml.Name as UserName,ms.smname,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId,isnull(distributorapp,0)as distributorapp,md.Text as [DistributorType], (select PartyName  from  MastParty where partyid=mp.SD_ID and isnull(PartyDist,0)=1 ) as SuperDistributor FROM MastParty Mp LEFT JOIN MastArea city ON Mp.CityId=city.AreaId left join MastArea dst on dst.AreaId=city.UnderId left join MastArea stt on stt.AreaId=dst.UnderId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId Left Join MastLogin Ml On Mp.UserId=Ml.Id left join mastsalesrep ms on mp.smid=ms.smid left join MastDistributorType md on md.Value=mp.DistType WHERE Mp.PartyDist=1 and PartyName<>'.' ";
            DataTable Distdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = Distdt;
            rpt.DataBind();
        }
        private void FillPartyControls(int PartyId)
        {
            try
            {
                string Partyquery = @"select ISNUll(Distributorapp,0) as distributorapp1,*,isnull(SMID,0) as SalesmenId from MastParty Mp  Left Join MastLogin Ml On Mp.UserId=Ml.Id left join mastdistributortype md on md.Value=mp.DistType where Mp.PartyDist=1 and partyId=" + PartyId;

                DataTable PartyValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Partyquery);
                if (PartyValueDt.Rows.Count > 0)
                {
                    Username.Disabled = true;
                    Distributor.Value = PartyValueDt.Rows[0]["PartyName"].ToString();
                    SyncId.Value = PartyValueDt.Rows[0]["SyncId"].ToString();
                    Address1.Value = PartyValueDt.Rows[0]["Address1"].ToString();
                    Address2.Value = PartyValueDt.Rows[0]["Address2"].ToString();
                    Pin.Value = PartyValueDt.Rows[0]["Pin"].ToString();
                    contactPerson.Value = PartyValueDt.Rows[0]["ContactPerson"].ToString();
                    Mobile.Value = PartyValueDt.Rows[0]["Mobile"].ToString();
                    Phone.Value = PartyValueDt.Rows[0]["Phone"].ToString();
                    CSTNo.Value = PartyValueDt.Rows[0]["CSTNo"].ToString();
                    VatTin.Value = PartyValueDt.Rows[0]["VatTin"].ToString();
                    GSTINid.Value = PartyValueDt.Rows[0]["GSTINNo"].ToString();
                    PanNo.Value = PartyValueDt.Rows[0]["PanNo"].ToString();
                    Remark.Value = PartyValueDt.Rows[0]["Remark"].ToString();

                 //   BindCity(Convert.ToInt32(PartyValueDt.Rows[0]["cityid"]));
                    HiddenCityID.Value = PartyValueDt.Rows[0]["cityid"].ToString();
                    ddlCity.SelectedValue = PartyValueDt.Rows[0]["cityid"].ToString();
                    Distributor2.Value = PartyValueDt.Rows[0]["DistributorName2"].ToString();
                    Telex.Value = PartyValueDt.Rows[0]["Telex"].ToString();
                    Fax.Value = PartyValueDt.Rows[0]["Fax"].ToString();
                    Username.Value = PartyValueDt.Rows[0]["Name"].ToString();

                 //   BindSalemen(Convert.ToInt32(PartyValueDt.Rows[0]["SalesmenId"]));
                    HiddenSalesPersonID.Value = PartyValueDt.Rows[0]["SalesmenId"].ToString();
                    ddlsalesperson.SelectedValue = PartyValueDt.Rows[0]["SalesmenId"].ToString();
                    HiddenDistributorArea.Value = PartyValueDt.Rows[0]["Areaid"].ToString();
                   //BindRoles();
                    HiddenRoleID.Value = PartyValueDt.Rows[0]["RoleID"].ToString();
                    ddlRole.SelectedValue=PartyValueDt.Rows[0]["RoleID"].ToString();
                    Email.Value = PartyValueDt.Rows[0]["Email"].ToString();
                    OpenOrder.Value = PartyValueDt.Rows[0]["OpenOrder"].ToString();
                    Outstanding.Value = PartyValueDt.Rows[0]["OutStanding"].ToString();
                    CreditLimit.Value = PartyValueDt.Rows[0]["CreditLimit"].ToString();
                    CreditDays.Value = PartyValueDt.Rows[0]["CreditDays"].ToString();
                    if (Convert.ToBoolean(PartyValueDt.Rows[0]["Active"]) == true)
                    {
                        chkIsAdmin.Checked = true;
                        BlockReason.Value = "";
                    }
                    else
                    {
                        divblock.Attributes.Remove("class");
                        chkIsAdmin.Checked = false;
                        BlockReason.Value = PartyValueDt.Rows[0]["BlockReason"].ToString();
                    }

                    if (PartyValueDt.Rows[0]["DOA"] != DBNull.Value)
                    {
                        txtDOA.Text = string.Format("{0:dd/MMM/yyyy}", PartyValueDt.Rows[0]["DOA"]);
                    }
                    if (PartyValueDt.Rows[0]["DOB"] != DBNull.Value)
                    {
                        txtDOB.Text = string.Format("{0:dd/MMM/yyyy}", PartyValueDt.Rows[0]["DOB"]);
                    }
                    ddlArea.SelectedValue = PartyValueDt.Rows[0]["AreaId"].ToString();

                    BindType();
                    if (PartyValueDt.Rows[0]["DistType"] != DBNull.Value)
                    {
                        HiddenDistType.Value = PartyValueDt.Rows[0]["DistType"].ToString();
                        ddltype.SelectedValue = PartyValueDt.Rows[0]["DistType"].ToString();
                        BindSuperDistributor();

                        if(PartyValueDt.Rows[0]["DistType"].ToString() == "DEPOT" || PartyValueDt.Rows[0]["DistType"].ToString() == "DIST")
                        {
                            divSuperDist.Attributes.Add("class", "hidden");
                        }
                        else
                        {
                            divSuperDist.Attributes.Remove("class");
                        }

                    }
                    if (PartyValueDt.Rows[0]["SD_ID"] != DBNull.Value && PartyValueDt.Rows[0]["SD_ID"].ToString() != "-1")
                    {
                        lstSDParty.SelectedValue = PartyValueDt.Rows[0]["SD_ID"].ToString();
                        HiddenSDPartyID.Value = PartyValueDt.Rows[0]["SD_ID"].ToString(); 
                    }

                     
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                    hdnoldMobile.Value = PartyValueDt.Rows[0]["Mobile"].ToString();

                    if (Convert.ToBoolean(PartyValueDt.Rows[0]["distributorapp1"]) == true)
                    { chkmobAccess.Checked = true; }
                    else { chkmobAccess.Checked = false; }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
               System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }
        private void InsertDist()
        {
            bool active = false;
            string CityID = Request.Form[HiddenCityID.UniqueID], SalesManID = Request.Form[HiddenSalesPersonID.UniqueID], RoleID = Request.Form[HiddenRoleID.UniqueID],
                
                SuperDistID=Request.Form[HiddenSDPartyID.UniqueID],
                DistType = Request.Form[HiddenDistType.UniqueID];// ddltype.SelectedValue;

            //if (DistType != "UNDERSD")
            //{
            //    SuperDistID = "0";
            //}

            if (string.IsNullOrEmpty(SuperDistID))
                SuperDistID = "0";

            if (chkIsAdmin.Checked)
                active = true;

            if (txtDOB.Text != "" && txtDOA.Text != "")
            {
                if (Convert.ToDateTime(txtDOB.Text) >= Convert.ToDateTime(txtDOA.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Date of Anniversary should be greater than Date of Birth!');", true); return;
                }
            }

            if(active==false)
                if (string.IsNullOrEmpty(BlockReason.Value))
                {
                    divblock.Attributes.Remove("class");
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Blocked Reason');", true); return; 
                }
            decimal AOpenOrder = 0, ACreditLimit = 0, AOutstanding = 0;
            int ACreditdays = 0, UserId = 0, Areaid = 0;
            if (!string.IsNullOrEmpty(OpenOrder.Value))
                AOpenOrder = Convert.ToDecimal(OpenOrder.Value);
            if (!string.IsNullOrEmpty(CreditDays.Value))
                ACreditdays = Convert.ToInt32(CreditDays.Value);
            if (!string.IsNullOrEmpty(CreditLimit.Value))
                ACreditLimit = Convert.ToDecimal(CreditLimit.Value);
            if (!string.IsNullOrEmpty(Outstanding.Value))
                AOutstanding = Convert.ToDecimal(Outstanding.Value);
            UserId =Convert.ToInt32(Settings.Instance.UserID);
            Areaid = Convert.ToInt32(Request.Form[HiddenDistributorArea.UniqueID]);
            int retval = DB.InsertDistributors(Distributor.Value, Address1.Value, Address2.Value, (CityID), Pin.Value, Email.Value, Mobile.Value, Remark.Value, SyncId.Value, BlockReason.Value, Username.Value, active, Phone.Value, Convert.ToInt32(RoleID), contactPerson.Value, CSTNo.Value, VatTin.Value, PanNo.Value, AOpenOrder, ACreditLimit, AOutstanding, ACreditdays, UserId, Telex.Value, Fax.Value, Distributor2.Value, Convert.ToInt32(SalesManID), Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()), Areaid, GSTINid.Value, "", 0, DistType, SuperDistID, "");
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate User Name Exists');", true);
                Distributor.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else if (retval == -3)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Distributor Exists');", true);
                SyncId.Focus();
            }
            else if (retval == -4)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email Exists');", true);
                SyncId.Focus();
            }
            else if (retval == -5)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Mobile Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastParty set SyncId='" + retval + "' where partyId=" + retval + " And PartyDist=1";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                string mastenviro = "select * from mastenviro";
                DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                if (dtenviro.Rows.Count > 0)
                {
                    //if (dtenviro.Rows[0]["CompCode"].ToString() == "Goldiee")
                    //{
                    //    if (InsertLicense(dtenviro.Rows[0]["compcode"].ToString(), Mobile.Value, Distributor.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value) != "Yes")
                    //    {
                    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;
                    //    }
                    //}
                    if (chkmobAccess.Checked == true)
                    {
                        if (upd.Insert_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), Mobile.Value, Distributor.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, "GOLDIEE"))
                        {
                            int cnt = DbConnectionDAL.GetIntScalarVal("update MastParty set distributorapp='" + chkmobAccess.Checked + "' where partyId=" + retval + " And PartyDist=1");
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

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                Distributor.Focus();
            }

        }

        private void UpdateDist()
        {
            bool active = false;
            string CityID = Request.Form[HiddenCityID.UniqueID], SalesManID = Request.Form[HiddenSalesPersonID.UniqueID], RoleID = Request.Form[HiddenRoleID.UniqueID],
                SuperDistID=Request.Form[HiddenSDPartyID.UniqueID],
                DistType = Request.Form[HiddenDistType.UniqueID];// ddltype.SelectedValue;

            //if (DistType != "UNDERSD" && SuperDistID != "0")
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('SuperDistributor only allowed for UnderSuperDistributor Type.');", true);
            //}

            //if (DistType != "UNDERSD")
            //{
            //    SuperDistID = "0";
            //}

            if (string.IsNullOrEmpty(SuperDistID))
                SuperDistID = "-1";

            if (chkIsAdmin.Checked)
                active = true;

            if (txtDOB.Text != "" && txtDOA.Text != "")
            {
                if (Convert.ToDateTime(txtDOB.Text) >= Convert.ToDateTime(txtDOA.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Date of Anniversary should be greater than Date of Birth!');", true); return;
                }
            }


            if (active == false)
                if (string.IsNullOrEmpty(BlockReason.Value))
                { divblock.Attributes.Remove("class"); System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Blocked Reason');", true); return; }
            decimal BOpenOrder = 0, BCreditLimit = 0, BOutstanding = 0;
            int BCreditdays = 0, UserId = 0, AreaID = 0;
            if (!string.IsNullOrEmpty(OpenOrder.Value))
                BOpenOrder = Convert.ToDecimal(OpenOrder.Value);
            if (!string.IsNullOrEmpty(CreditDays.Value))
                BCreditdays = Convert.ToInt16(CreditDays.Value);
            if (!string.IsNullOrEmpty(CreditLimit.Value))
                BCreditLimit = Convert.ToDecimal(CreditLimit.Value);
            if (!string.IsNullOrEmpty(Outstanding.Value))
                BOutstanding = Convert.ToDecimal(Outstanding.Value);
            UserId = Convert.ToInt32(Settings.Instance.UserID);
            AreaID = Convert.ToInt32(Request.Form[HiddenDistributorArea.UniqueID]);

            int retval = DB.UpdateDistributors(Convert.ToInt32(ViewState["PartyId"]), Distributor.Value, Address1.Value, Address2.Value, (CityID), Pin.Value, Email.Value, Mobile.Value, Remark.Value, SyncId.Value, BlockReason.Value, Username.Value, active, Phone.Value, Convert.ToInt32(RoleID), "", CSTNo.Value, VatTin.Value, PanNo.Value, BOpenOrder, BCreditLimit, BOutstanding, BCreditdays, UserId, Telex.Value, Fax.Value, Distributor2.Value, Convert.ToInt32(SalesManID), Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()), AreaID, GSTINid.Value, "", 0, DistType, SuperDistID, "");
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate User Name Exists');", true);
                Distributor.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
           
            else if (retval == -3)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Email Exists');", true);
                Email.Focus();
            }
            else if (retval == -5)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Mobile Exists');", true);
                Mobile.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastParty set SyncId='" + retval + "' where partyId=" + ViewState["PartyId"] + " And PartyDist=1";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }

                string activechk = "0";
                if (chkIsAdmin.Checked)
                    activechk = "1";
                string mastenviro = "select * from mastenviro";
                DataTable dtenviro = DbConnectionDAL.GetDataTable(CommandType.Text, mastenviro);
                if (dtenviro.Rows.Count > 0)
                {
                    //if (dtenviro.Rows[0]["CompCode"].ToString() == "Goldiee")
                    //{
                    //    if (UpdateLicense(dtenviro.Rows[0]["compcode"].ToString(), dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, Distributor.Value) != "Yes")
                    //    {
                    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;
                    //    }
                    //}  

                        if (upd.Update_GrahaakLicense(dtenviro.Rows[0]["compcode"].ToString(), "", Distributor.Value, dtenviro.Rows[0]["compurl"].ToString(), Mobile.Value, activechk, "", chkmobAccess.Checked, "GOLDIEE", hdnoldMobile.Value))
                        {
                            if (activechk == "1")
                            {
                                int cnt = DbConnectionDAL.GetIntScalarVal("update MastParty set distributorapp='" + chkmobAccess.Checked + "' where partyId=" + retval + "");
                            }
                            else
                            {
                                int cnt = DbConnectionDAL.GetIntScalarVal("update MastParty set distributorapp='" + false + "' where partyId=" + retval + "");
                            }
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! While Creating License');", true); return;

                        }
                   
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error!! Enviro Table Have No Data');", true); return;
                }
                
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
                Distributor.Focus();
            }

        }

        public string InsertLicense(string compcode, string DeviceID, string PersonName, string URL, string mob)
        {
            string result = "No";
            try
            {
                string insqry = "SELECT count(*) FROM LineMaster WHERE Mobile='" + mob + "' and upper(Product)='GOLDIEE' and compcode='" + compcode + "'";
                int count = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
                if (count < 1)
                {
                    string insLine = "insert into LineMaster (CompCode,DeviceID,Active,LineDate,PersonName,Product,URL,mobile)" +
                                            "values('" + compcode + "','" + DeviceID + "','Y','" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "','" + PersonName + "','GOLDIEE','" + URL + "','" + mob + "')";

                    DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insLine);
                    result = "Yes";
                }
            }
            catch (Exception ex) { ex.ToString(); }
            return result;
        }

        public string UpdateLicense(string compcode, string URL, string mob, string distributorname)
        {
            string result = "No";

            try
            {               
                
                string insqry = "SELECT count(*) FROM LineMaster WHERE Mobile='" + hdnoldMobile.Value + "' and upper(Product)='Goldiee' and compcode='" + compcode + "'";
                int count = DbConnectionDAL.GetDemoLicenseIntScalarVal(insqry);
                if (count >= 1)
                {
                    insqry = " update linemaster set mobile='" + mob + "' where Mobile='" + hdnoldMobile.Value + "' and upper(Product)='Goldiee' and compcode='" + compcode + "'";
                    DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insqry);
                    result = "Yes";
                }
                else
                {
                    string insLine = "insert into LineMaster (CompCode,Active,LineDate,PersonName,Product,URL,mobile)" +
                                       "values('" + compcode + "','Y','" + DateTime.Now.ToUniversalTime().AddSeconds(19800).ToString("yyyy-MM-dd") + "','" + distributorname + "','GOLDIEE','" + URL + "','" + mob + "')";

                    DbConnectionDAL.ExecuteNonQueryforlicence(constrDmLicense, CommandType.Text, insLine);
                    result = "Yes";
                }
            }

            catch (Exception ex) { ex.ToString(); }
            return result;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Username.Disabled = false;
            ClearControls();
            Response.Redirect("~/DistributorMaster.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                areaVisibility();
                if (btnSave.Text == "Update")
                {
                    UpdateDist();
                }
                else
                {
                    InsertDist();
                }
            }
           catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = DB.delete(Convert.ToString(ViewState["PartyId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Distributor.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record //cannot be deleted as it is in use');", true);
                 //   ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Distributor.Focus();
                }
            }
           
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
           // BindCity(0);
            Username.Disabled = false;
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
           // BindSuperDistributor();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }
        private void ClearControls()
        {
            Distributor.Value = "";
            Address1.Value = "";
            Address2.Value = "";
            Email.Value = "";
            Pin.Value = ""; Phone.Value = "";
            Mobile.Value = "";
            CSTNo.Value = "";
            VatTin.Value = "";
            GSTINid.Value = "";
            PanNo.Value = "";
            SyncId.Value = "";
            Remark.Value = "";
            //ddlCity.SelectedIndex = 0;
            //ddlRole.SelectedIndex = 0;
            Username.Value = "";
            chkIsAdmin.Checked = true;
            Distributor2.Value = "";
            contactPerson.Value = "";
            //ddlsalesperson.SelectedIndex = 0;
            ddltype.SelectedIndex = 0;
            Telex.Value = "";
            Fax.Value = "";
            CreditDays.Value = "";
            CreditLimit.Value = "";
            OpenOrder.Value = "";
            Outstanding.Value = "";
            txtDOA.Text = "";
            txtDOB.Text = "";
            HiddenRoleID.Value = string.Empty;
            HiddenSalesPersonID.Value = string.Empty;
            HiddenCityID.Value = string.Empty;
            HiddenDistributorArea.Value = string.Empty;
            HiddenSDPartyID.Value = string.Empty;
            HiddenDistType.Value = string.Empty;
            chkmobAccess.Checked = false;
            lstSDParty.Items.Clear();
           // BindSuperDistributor();
        }

        private void filldt()
        {
            string str = "", straddqry = ""; StringBuilder sb = new StringBuilder(); string col = "";
            if (_exportp == true)
            {

                str = @"SELECT ROW_NUMBER() over (order by mp.PartyId desc ) as [Sr. No],Mp.PartyName as [Distributor Name],stt.AreaName as StateName,city.AreaName AS city,area.AreaName AS Area,Mp.Mobile,isnull(mp.GSTINNo,'') as GSTIN,Ml.Name as UserName,ms.smname as SalesPerson,Mp.SyncId,case Mp.Active when 1 then 'Yes' else 'No' end as Active,md.Text  as [DistributorType], (select PartyName  from  MastParty where partyid=mp.SD_ID and isnull(PartyDist,0)=1) as SuperDistributor FROM MastParty Mp LEFT JOIN MastArea city ON Mp.CityId=city.AreaId left join MastArea dst on dst.AreaId=city.UnderId left join MastArea stt on stt.AreaId=dst.UnderId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId Left Join MastLogin Ml On Mp.UserId=Ml.Id left join mastsalesrep ms on mp.smid=ms.smid left join MastDistributorType md on md.Value=mp.DistType WHERE Mp.PartyDist=1 and PartyName<>'.' ";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
               // DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
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
                Response.AddHeader("content-disposition", "attachment;filename=DistributorMaster.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You do not have permission to Export');", true);


        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            filldt();
        }
    }
}