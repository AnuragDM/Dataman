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

namespace AstralFFMS
{
    public partial class AddNewParty : System.Web.UI.Page
    {
        PartyBAL PB = new PartyBAL();
        string parameter = "";
        string VisitID = "0";
        string CityID = "0";
        int AreaId = 0;
        string Level = "0";
        public string PartyId = "0";
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SyncId.Attributes.Add("Disabled", "Disabled");
            string pageName = Path.GetFileName(Request.Path);
          
            if (Request.QueryString["PartyId"] != null)
            {
                PartyId = Request.QueryString["PartyId"].ToString();
            }

            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();

            }
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();

            }
            if (Request.QueryString["AreaId"] != null)
            {
                AreaId = Convert.ToInt32(Request.QueryString["AreaId"].ToString());
            }
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }
            int area = Convert.ToInt32(Session["AreaIDDSRL1"]);

            if (!IsPostBack)
            {

                txtDOA.Attributes.Add("readonly", "readonly");
                txtDOB.Attributes.Add("readonly", "readonly");               
                chkIsAdmin.Checked = true;
                btnDelete.Visible = false;
                BindPartyType();
                mainDiv.Style.Add("display", "block");
                //Added
                //Ankita - 18/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //BindCity(0);
                BindCity1(area);
                //End
                BindDistributors(0);
                BindArea();
                if (Request.QueryString["PartyId"] != null)
                {
                    FillPartyControls(Convert.ToInt32(Request.QueryString["PartyId"]));
                    //Button1.Visible = true;
                }
                else { BindIndustry(0); }//BindCity(0); 
            }
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
        private string GetDistributorCity(int VisiID)
        {
            string st = "select CityIds from TransVisit where VisId=" + VisitID;
            string CityId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return CityId;
        }

        private void BindArea()
        {  //Ankita - 18/may/2016- (For Optimization)
//            string str = @"select * from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and
//PrimCode=" + Settings.Instance.DSRSMID + ") and UnderId in (" + GetDistributorCity(Convert.ToInt32(VisitID)) + ") and  areatype='Area' and Active=1 order by AreaName";
            string str = @"select AreaId,AreaName from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and
PrimCode=" + Settings.Instance.DSRSMID + ") and UnderId in (" + GetDistributorCity(Convert.ToInt32(VisitID)) + ") and  areatype='Area' and Active=1 order by AreaName";
            //            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.DSRSMID+ ")) and UnderId="+GetDistributorCity(Convert.ToInt32(VisitID))+" and areatype='Area' and Active=1 order by AreaName ";

            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                ddlArea.DataSource = obj;
                ddlArea.DataTextField = "AreaName";
                ddlArea.DataValueField = "AreaId";
                ddlArea.DataBind();
                ddlArea.SelectedValue = Convert.ToString(Session["AreaIDDSRL1"]);
                ddlArea.Items.Insert(0, new ListItem("-- Select --", "0"));

                if (ddlArea.SelectedIndex > 0)
                {
                    this.BindBeatInArea(ddlArea.SelectedValue);
                }
            }
         

        }

        private void BindBeatInArea(string AreaID)
        {//Ankita - 18/may/2016- (For Optimization)
            ddlBeat.Items.Clear();
            //string str = @"select * from mastarea where UnderId=" + AreaID + " order by AreaName";
            string str = @"select AreaId,AreaName from mastarea where UnderId=" + AreaID + " order by AreaName";
            //           string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                      in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.DSRSMID + ")) and UnderId=" + AreaID + " and areatype='Beat' and Active=1 order by AreaName ";
            //  string str = @"select M.AreaId,M.AreaName from mastarea M inner join [TransBeatPlan] P on M.AreaId=p.BeatId  where   P.AppStatus='Approve' and P.SMId=" + Settings.Instance.DSRSMID + "  and  M.UnderId=" + AreaID;

            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                ddlBeat.DataSource = obj;
                ddlBeat.DataTextField = "AreaName";
                ddlBeat.DataValueField = "AreaId";
                ddlBeat.DataBind();
                ddlBeat.SelectedValue = Convert.ToString(Session["BeatIDDSRL1"]);

            }
            ddlBeat.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        #region Bind Dropdowns

        private void BindCity(int CityId)
        {
            string strq = "";
            //       if (Settings.Instance.UserName.ToUpper() == "SA")
            if (roleType == "Admin")
            {
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
            //         fillDDLDirect(ddlCity, strq, "cityid", "cityName", 1);
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
            if (dt.Rows.Count > 0)
            {
                ddlCity.DataSource = dt;
                ddlCity.DataTextField = "cityName";
                ddlCity.DataValueField = "cityid";
                ddlCity.DataBind();
                ddlCity.SelectedValue = Request.QueryString["CityID"].ToString();
            }

        }

        private void BindCity1(int AreaID)
        {
            string strq = "";
            strq = "select Underid from MastArea where areatype in ('AREA') and Areaid=" + AreaID + "  and  Active='1'  order by AreaName ";                      
            DataTable dtarea = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
            int idarea = Convert.ToInt32(dtarea.Rows[0]["Underid"]);

            string strqarea = "select AreaId,AreaName from MastArea where areatype in ('City') and AreaId=" + idarea + "  and  Active='1'  order by AreaName ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strqarea);
            if (dt.Rows.Count > 0)
            {
                ddlCity.DataSource = dt;
                ddlCity.DataTextField = "AreaName";
                ddlCity.DataValueField = "AreaId";
                ddlCity.DataBind();
               // ddlCity.SelectedValue = Request.QueryString["CityID"].ToString();
            }               

        }

        private void BindArea(Int32 CityId, Int32 AreaID)
        {
            string strq = "";            
            if (roleType == "Admin")
            {
                if (AreaID > 0)
                { strq = "select AreaId,AreaName from MastArea where areatype in ('AREA') and UnderId=" + CityId + " and (Active='1' or Areaid in (" + AreaID + "))    order by AreaName "; }
                else
                {
                    strq = "select AreaId,AreaName from MastArea where areatype in ('AREA') and UnderId=" + CityId + "  and  Active='1'  order by AreaName ";
                }
            }
            else
            {
                if (AreaID > 0)
                {
                    strq = "select AreaId,AreaName  from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
                  " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('AREA') and underId=" + CityId + " and (Active='1' or Areaid in (" + AreaID + "))  order by AreaName";
                }
                else
                {
                    strq = "select AreaId,AreaName  from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
                        " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('AREA') and underId=" + CityId + " and  Active='1'  order by AreaName";
                }
            }

            fillDDLDirect(ddlArea, strq, "AreaId", "AreaName", 1);
        }
        private void BindBeat(Int32 AreaId, Int32 BeatId)
        {
            string strq = "";

            if (BeatId > 0)
            {
                strq = "select AreaId,AreaName from MastArea where areatype in ('BEAT') and UnderId=" + AreaId + " and (Active='1' or Areaid in (" + BeatId + ")) order by AreaName ";
            }
            else { strq = "select AreaId,AreaName from MastArea where areatype in ('BEAT') and UnderId=" + AreaId + " and Active='1' order by AreaName "; }

            fillDDLDirect(ddlBeat, strq, "AreaId", "AreaName", 1);
        }
        private void BindDistributors(Int32 DistId)
        {
            string str = "";
            if (DistId > 0)
            {
                if (Settings.Instance.UserName.ToUpper() == "SA")
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and (Active='1' or PartyId in (" + DistId + ")) order by PartyName";
                }
                else
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and (Active='1' or PartyId in (" + DistId + ")) and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                   " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) order by PartyName ";
                }
            }
            else
            {
                if (Settings.Instance.UserName.ToUpper() == "SA")
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and Active=1 order by PartyName";
                }
                else
                {
                    str = "select PartyId,PartyName+'('+SyncId+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as PartyName from MastParty where PartyDist=1 and Active=1 and CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                     " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) order by PartyName ";
                }
            }
            fillDDLDirect(ddldistributor, str, "PartyId", "PartyName", 1);
        }

        private void BindIndustry(int IndId)
        {
            string str = "";
            if (IndId > 0)
            {
                str = "select IndId,IndName from MastIndustry where (Active='1' or IndId in (" + IndId + " ))  order by IndName";
            }
            else
            {
                str = "select IndId,IndName from MastIndustry where Active='1' order by IndName";
            }
            fillDDLDirect(DdlIndustry, str, "IndId", "IndName", 1);
        }
        private void BindPartyType()
        {
            string str = "select PartyTypeId,PartyTypeName from PartyType order by PartyTypeName";
            fillDDLDirect(ddlpartytype, str, "PartyTypeId", "PartyTypeName", 1);
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

        //protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlCity.SelectedValue != "0")
        //    {
        //        //ddlArea.Items.Clear();
        //        //ddlBeat.Items.Clear();
        //        ddldistributor.Items.Clear();
        //        //BindArea(Convert.ToInt32(ddlCity.SelectedValue), 0);
        //        BindDistributors(0);
        //    }
        //}

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedIndex > 0)
            {
                //         this.BindBeatInArea(ddlArea.SelectedValue);
                this.BindBeatFromArea(ddlArea.SelectedValue);
            }
        }

        private void BindBeatFromArea(string p)
        {
            try
            {
                ddlBeat.Items.Clear();
                //Ankita - 18/may/2016- (For Optimization)
                //string strNewQry = @"select * from mastarea where UnderId=" + p + " order by AreaName";
                string strNewQry = @"select AreaId,AreaName from mastarea where UnderId=" + p + " And Active=1 order by AreaName";
                DataTable obj = new DataTable();
                obj = DbConnectionDAL.GetDataTable(CommandType.Text, strNewQry);

                if (obj.Rows.Count > 0)
                {
                    ddlBeat.DataSource = obj;
                    ddlBeat.DataTextField = "AreaName";
                    ddlBeat.DataValueField = "AreaId";
                    ddlBeat.DataBind();
                }
                ddlBeat.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void fillRepeter()
        {
            string str = @"SELECT *,city.AreaName AS city,area.AreaName AS Area,beat.AreaName AS beat,Ind.IndName,Mp.ContactPerson, pt.PartyTypeName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId FROM MastParty Mp LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId WHERE Mp.PartyDist=0 AND Mp.PartyName<>'.'";
            DataTable Partydt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = Partydt;
            rpt.DataBind();
        }
        private void FillPartyControls(int PartyId)
        {
            try
            {
                string Partyquery = @"select * from MastParty Mp where Mp.PartyDist=0 and partyId=" + PartyId;

                DataTable PartyValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Partyquery);
                if (PartyValueDt.Rows.Count > 0)
                {
                    PartyName.Value = PartyValueDt.Rows[0]["PartyName"].ToString();
                    SyncId.Value = PartyValueDt.Rows[0]["SyncId"].ToString();
                    Potential.Value = PartyValueDt.Rows[0]["Potential"].ToString();
                    Address1.Value = PartyValueDt.Rows[0]["Address1"].ToString();
                    Address2.Value = PartyValueDt.Rows[0]["Address2"].ToString();
                    Pin.Value = PartyValueDt.Rows[0]["Pin"].ToString();
                    ContactPerson.Value = PartyValueDt.Rows[0]["ContactPerson"].ToString();
                    Mobile.Value = PartyValueDt.Rows[0]["Mobile"].ToString();
                    Phone.Value = PartyValueDt.Rows[0]["Phone"].ToString();
                    CSTNo.Value = PartyValueDt.Rows[0]["CSTNo"].ToString();
                    VatTin.Value = PartyValueDt.Rows[0]["VatTin"].ToString();
                    ServiceTax.Value = PartyValueDt.Rows[0]["ServiceTax"].ToString();
                    PanNo.Value = PartyValueDt.Rows[0]["PanNo"].ToString();
                    Remark.Value = PartyValueDt.Rows[0]["Remark"].ToString();
                    BindCity(Convert.ToInt32(PartyValueDt.Rows[0]["cityid"]));
                    ddlCity.SelectedValue = PartyValueDt.Rows[0]["cityid"].ToString();
                    BindArea(Convert.ToInt32(PartyValueDt.Rows[0]["cityid"]), Convert.ToInt32(PartyValueDt.Rows[0]["AreaId"]));
                    ddlArea.SelectedValue = PartyValueDt.Rows[0]["AreaId"].ToString();
                    BindBeat(Convert.ToInt32(PartyValueDt.Rows[0]["AreaId"]), Convert.ToInt32(PartyValueDt.Rows[0]["BeatId"]));
                    ddlBeat.SelectedValue = PartyValueDt.Rows[0]["BeatId"].ToString();
                    ddlpartytype.SelectedValue = PartyValueDt.Rows[0]["PartyType"].ToString();
                    BindIndustry(Convert.ToInt32(PartyValueDt.Rows[0]["IndId"]));
                    DdlIndustry.SelectedValue = PartyValueDt.Rows[0]["IndId"].ToString();
                    BindDistributors(Convert.ToInt32(PartyValueDt.Rows[0]["UnderId"]));
                    ddldistributor.SelectedValue = PartyValueDt.Rows[0]["UnderId"].ToString();
                    Email.Value = PartyValueDt.Rows[0]["EMail"].ToString();
                    if (PartyValueDt.Rows[0]["DOA"] != DBNull.Value)
                    {
                        txtDOA.Text = string.Format("{0:dd/MMM/yyyy}", PartyValueDt.Rows[0]["DOA"]);
                    }
                    if (PartyValueDt.Rows[0]["DOB"] != DBNull.Value)
                    {
                        txtDOB.Text = string.Format("{0:dd/MMM/yyyy}", PartyValueDt.Rows[0]["DOB"]);
                    }
                    if (Convert.ToBoolean(PartyValueDt.Rows[0]["Active"]) == true)
                    {
                        chkIsAdmin.Checked = true;
                    }
                    else
                    {
                        divblock.Attributes.Remove("class");
                        BlockReason.Value = PartyValueDt.Rows[0]["BlockReason"].ToString();
                        chkIsAdmin.Checked = false;
                    }
                    if (PartyValueDt.Rows[0]["ImgUrl"] != string.Empty)
                    {
                        imgpreview.Src = PartyValueDt.Rows[0]["ImgUrl"].ToString();
                        imgpreview.Style.Add("display", "block");
                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }
                    GSTINNo.Value = PartyValueDt.Rows[0]["GSTINNo"].ToString();
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing records');", true);
            }
        }

        private void InsertParty()
        {
            bool active = false; string Imgurl = ""; 
            if (chkIsAdmin.Checked)
                active = true;

            if (txtDOB.Text != "" && txtDOA.Text != "")
            {
                if (Convert.ToDateTime(txtDOB.Text) >= Convert.ToDateTime(txtDOA.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Date of Anniversary should be greater than Date of Birth!');", true); return;
                }
            }

            if (!active)
                if (string.IsNullOrEmpty(BlockReason.Value))
                {
                    divblock.Attributes.Remove("class");
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Blocked Reason');", true); return;
                }
            decimal APotential = 0;
            if (!string.IsNullOrEmpty(Potential.Value))
                APotential = Convert.ToDecimal(Potential.Value);
            int UserId = Convert.ToInt32(Settings.Instance.UserID);
            int DistID = 0;
            if (ddldistributor.SelectedValue != "0" && !string.IsNullOrEmpty(ddldistributor.SelectedValue)) { DistID = Convert.ToInt32(ddldistributor.SelectedValue); }

            if (partyImgFileUpload.HasFile)
            {
                string directoryPath = Server.MapPath(string.Format("~/{0}/", "PartyImages"));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filename = Path.GetFileName(partyImgFileUpload.FileName);
                bool k = ValidateImageSize();
                if (k != true)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                    return;
                }
                partyImgFileUpload.SaveAs(Server.MapPath("~/PartyImages" + "/Party_" + filename));
                Imgurl = "~/PartyImages" + "/Party_" + filename;
            }
            int retval = PB.InsertParty(PartyName.Value, Address1.Value, Address2.Value, Convert.ToInt32(ddlCity.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue),
                Convert.ToInt32(ddlBeat.SelectedValue), DistID, Pin.Value, Mobile.Value, Phone.Value, Remark.Value,
                SyncId.Value, DdlIndustry.SelectedValue, APotential, active, BlockReason.Value, Convert.ToInt32(ddlpartytype.SelectedValue),
                ContactPerson.Value, CSTNo.Value, VatTin.Value, ServiceTax.Value, PanNo.Value, UserId, Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()), Email.Value,Imgurl,GSTINNo.Value);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Party Exists');", true); 
                PartyName.Focus();
            }
            //else if (retval == -2)
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
            //    SyncId.Focus();
            //}
            else if (retval == -3)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Mobile Exists');", true);
                Mobile.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true); 
                ClearControls();
                PartyName.Focus();
                // Added 29/09/2016  Nishu
                string areaid = Session["AreaIDDSRL1"].ToString();
                string beatid = Session["BeatIDDSRL1"].ToString();
                Session["AreaIDBackBtnDSRL1"] = areaid.ToString();
                Session["BeatIDBackBtnDSRL1"] = beatid.ToString();

                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "2;url=DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level;
                this.Page.Controls.Add(meta);  
                //End
            }

        }

        protected bool ValidateImageSize()
        {
            int fileSize = partyImgFileUpload.PostedFile.ContentLength;
            //Limit size to approx 2mb for image
            if ((fileSize > 0 & fileSize < 1048576))
            {
                return true;
            }
            else
            {
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);               
                return false;
            }
        }

        private void UpdateParty()
        {
            bool active = false; string Imgurl = "";
            if (chkIsAdmin.Checked)
                active = true;
            if (txtDOB.Text != "" && txtDOA.Text != "")
            {
                if (Convert.ToDateTime(txtDOB.Text) >= Convert.ToDateTime(txtDOA.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Date of Anniversary should be greater than Date of Birth!');", true); return;
                }
            }
            if (!active)
                if (string.IsNullOrEmpty(BlockReason.Value))
                {
                    divblock.Attributes.Remove("class");
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Blocked Reason');", true); return;
                }
            decimal APotential = 0;
            if (!string.IsNullOrEmpty(Potential.Value))
                APotential = Convert.ToDecimal(Potential.Value);
            int UserId = Convert.ToInt32(Settings.Instance.UserID);
            int DistID = 0;
            if (ddldistributor.SelectedValue != "0" && !string.IsNullOrEmpty(ddldistributor.SelectedValue)) { DistID = Convert.ToInt32(ddldistributor.SelectedValue); }
            if (partyImgFileUpload.HasFile)
            {
                string directoryPath = Server.MapPath(string.Format("~/{0}/", "PartyImages"));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filename = Path.GetFileName(partyImgFileUpload.FileName);
                bool k = ValidateImageSize();
                if (k != true)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                    return;
                }
                partyImgFileUpload.SaveAs(Server.MapPath("~/PartyImages" + "/Party_" + filename));
                Imgurl = "~/PartyImages" + "/Party_" + filename;
            }
            int retval = PB.UpdateParty(Convert.ToInt32(ViewState["PartyId"]), PartyName.Value, Address1.Value, Address2.Value,
                Convert.ToInt32(ddlCity.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue), Convert.ToInt32(ddlBeat.SelectedValue),
                DistID, Pin.Value, Mobile.Value, Phone.Value, Remark.Value, SyncId.Value,
                DdlIndustry.SelectedValue, APotential, active, BlockReason.Value, Convert.ToInt32(ddlpartytype.SelectedValue),
                ContactPerson.Value, CSTNo.Value, VatTin.Value, ServiceTax.Value, PanNo.Value, UserId, Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()), Email.Value,Imgurl,GSTINNo.Value);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Party Exists');", true);
                PartyName.Focus();
            }
            //else if (retval == -2)
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
            //    SyncId.Focus();
            //}
            else if (retval == -3)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Mobile Exists');", true);
                Mobile.Focus();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                PartyName.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";

                // Added 29/09/2016  Nishu
                string areaid = Session["AreaIDDSRL1"].ToString();
                string beatid = Session["BeatIDDSRL1"].ToString();
                Session["AreaIDBackBtnDSRL1"] = areaid.ToString();
                Session["BeatIDBackBtnDSRL1"] = beatid.ToString();

                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "2;url=DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level;
                this.Page.Controls.Add(meta);
                // End
            }
        }
        private void ClearControls()
        {
            ddlpartytype.SelectedIndex = 0;
            ddlArea.Items.Clear();
            ddlBeat.Items.Clear();
            ddlCity.SelectedIndex = 0;
            DdlIndustry.SelectedIndex = 0;
            ddldistributor.Items.Clear();
            PartyName.Value = "";
            Potential.Value = "";
            Address1.Value = "";
            Address2.Value = "";
            Pin.Value = "";
            Mobile.Value = "";
            Phone.Value = "";
            Remark.Value = "";
            SyncId.Value = "";
            chkIsAdmin.Checked = false;
            BlockReason.Value = "";
            ContactPerson.Value = "";
            CSTNo.Value = "";
            VatTin.Value = "";
            ServiceTax.Value = "";
            PanNo.Value = "";
            txtDOA.Text = "";
            txtDOB.Text = "";
            Email.Value = "";
            GSTINNo.Value = "";
            imgpreview.Style.Add("display", "none");
            imgpreview.Src = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            //    Response.Redirect("~/AddNewParty.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = PB.delete(Convert.ToString(ViewState["PartyId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    PartyName.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    PartyName.Focus();
                }

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateParty();  
                
                    //string areaid = Session["AreaIDDSRL1"].ToString();
                    //string beatid = Session["BeatIDDSRL1"].ToString();
                    //Session["AreaIDBackBtnDSRL1"] = areaid.ToString();
                    //Session["BeatIDBackBtnDSRL1"] = beatid.ToString();
                  
                    //HtmlMeta meta = new HtmlMeta();
                    //meta.HttpEquiv = "Refresh";
                    //meta.Content = "2;url=DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level;
                    //this.Page.Controls.Add(meta); 
                    
                }
                else
                {
                    InsertParty();
                   
                    //string areaid = Session["AreaIDDSRL1"].ToString();
                    //string beatid = Session["BeatIDDSRL1"].ToString();
                    //Session["AreaIDBackBtnDSRL1"] = areaid.ToString();
                    //Session["BeatIDBackBtnDSRL1"] = beatid.ToString();
                   
                    //HtmlMeta meta = new HtmlMeta();
                    //meta.HttpEquiv = "Refresh";
                    //meta.Content = "2;url=DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level;                   
                    //this.Page.Controls.Add(meta);                     
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //Added as per UAt on - 14-Dec-2015
            string areaid = Session["AreaIDDSRL1"].ToString();
            string beatid = Session["BeatIDDSRL1"].ToString();
            Session["AreaIDBackBtnDSRL1"] = areaid.ToString();
            Session["BeatIDBackBtnDSRL1"] = beatid.ToString();
            //End
            Response.Redirect("~/DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level);
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
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);

        }

    }
}