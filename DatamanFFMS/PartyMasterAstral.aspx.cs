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
using Newtonsoft.Json;
using System.Web.Services;
using System.Text;

namespace AstralFFMS
{
    public partial class PartyMasterAstral : System.Web.UI.Page
    {
        PartyBAL PB = new PartyBAL();
        string parameter = "";
        string VisitID = "0";
        string CityID = "0";
        int AreaId = 0;
        string Level = "0";
        string  PartyId = "0";
        bool _exportp = false; 
        DistributorBAL DB = new DistributorBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            SyncId.Attributes.Add("Disabled", "Disabled");
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
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
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
               // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
           // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
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
            if (!IsPostBack)
            {
              //  BindState();
                List<Party> partydetails = new List<Party>();
                partydetails.Add(new Party());
                rpt12.DataSource = partydetails;
                rpt12.DataBind();
                txtDOA.Attributes.Add("readonly", "readonly");
                txtDOB.Attributes.Add("readonly", "readonly");
                Button1.Visible = false;
                chkIsAdmin.Checked = true;
                btnDelete.Visible = false;
               // BindPartyType(); 
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["PartyId"] != null)
                {
                    FillPartyControls(Convert.ToInt32(Request.QueryString["PartyId"]));
                    Button1.Visible = true;
                }
                else { 
                  //  BindCity(0); BindIndustry(0); BindDistributors(0);
                }
            }
        }

        public class Party
        {           
            public string PartyName { get; set; }
          
            public string Distributor { get; set; }
            public string city { get; set; }
            public string Area { get; set; }
            public string Beat { get; set; }
            public string ContactPerson { get; set; }
            public string PartyTypeName { get; set; }
            public string IndName { get; set; }
            public string Mobile { get; set; }
            public string SyncId { get; set; }
            public string Active { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string GetPartyDetails(string State, string City, string Area)
        {  
           
            string str = "", straddqry = "";
            if ((!string.IsNullOrEmpty(Area)) && (Area != "0"))
            {
                straddqry = " and mp.AreaId=" + Area + "";
            }
            else if ((!string.IsNullOrEmpty(City)) && (City != "0"))
            {
                straddqry = " and mp.cityId=" + City + "";
            }
            else if (State != "0")
            {
                straddqry = " and mp.cityId in (select Distinct(CityID) from ViewGeo VG where VG.StateId = " + State + ")";
            }

            if (Settings.Instance.RoleType == "Admin")
            {
                str = @"SELECT mp.partyid,mp.partyname,mp.Mobile,city.AreaName AS city,area.AreaName AS Area,beat.AreaName AS beat,Ind.IndName,Mp.ContactPerson,  mp1.PartyName as Distributor, pt.PartyTypeName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId,Mp.CreditLimit,Mp.OutStanding FROM MastParty Mp  LEFT JOIN mastparty mp1
	          ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId WHERE Mp.PartyDist=0 AND Mp.PartyName<>'.' " + straddqry + "";

            }
            else
            {
                str = @"SELECT mp.partyid,mp.partyname,mp.Mobile,city.AreaName AS city,area.AreaName AS Area,beat.AreaName AS beat,Ind.IndName,Mp.ContactPerson,  mp1.PartyName as Distributor, pt.PartyTypeName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId,Mp.CreditLimit,Mp.OutStanding FROM MastParty Mp  LEFT JOIN mastparty mp1
               ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId 
               LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId 
               Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId WHERE Mp.PartyDist=0 and Mp.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
            " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) AND Mp.PartyName<>'.'" + straddqry + "";
            }


            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            return JsonConvert.SerializeObject(dtItem);


        }

        #region Bind Dropdowns
        //Added By Ankita 1 June 2016
        private void BindState()
        {
            string strQ = "select AreaID,AreaName from mastarea MA where AreaType='State' and Active='1' order by AreaName";
            fillDDLDirect(ddlstatelist, strQ, "AreaID", "AreaName", 1);
        }

        //private void BindCity(int CityId)
        //{
        //    string strq = "";
        //    if (Settings.Instance.UserName.ToUpper() == "SA")
        //    {
        //        if (CityId > 0) { strq = "select AreaId,AreaName from MastArea where areatype in ('CITY')  and (Active='1' or Areaid in (" + CityId + "))    order by AreaName"; }
        //        else { strq = "select AreaId,AreaName from MastArea where areatype in ('CITY') and Active='1' order by AreaName"; }
        //    }
        //    else
        //    {
        //        if (CityId > 0)
        //        {
        //            strq = "select AreaId,AreaName from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
        //                " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('CITY')  and (Active='1' or Areaid in (" + CityId + "))     order by AreaName";
        //        }
        //        else
        //        {
        //            strq = "select AreaId,AreaName from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
        //            " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('CITY') and MastArea.Active='1' order by AreaName";
        //        }
        //    }
        //    fillDDLDirect(ddlCity, strq, "AreaId", "AreaName", 1);
        //}


        private void BindCity(int CityId)
        {
            string strq = "";
            if (Settings.Instance.RoleType == "Admin")
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
        private void BindArea(Int32 CityId,Int32 AreaID)
        {
            string strq = "";
            if (Settings.Instance.RoleType == "Admin")
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
        private void BindBeat(Int32 AreaId,Int32 BeatId)
        {   string strq = "";
             //if (Settings.Instance.UserName.ToUpper() == "SA")
             //{
                 if (BeatId > 0)
                 {
                     strq = "select AreaId,AreaName from MastArea where areatype in ('BEAT') and UnderId=" + AreaId + " and (Active='1' or Areaid in (" + BeatId + ")) order by AreaName ";
                 }
                 else { strq = "select AreaId,AreaName from MastArea where areatype in ('BEAT') and UnderId=" + AreaId + " and Active='1' order by AreaName "; }
            // }
           //else
           //  {
           //      if (BeatId > 0)
           //      {
           //          strq = "select AreaId,AreaName from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
           //         " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('BEAT') and underId=" + AreaId + " and (Active='1' or Areaid in (" + BeatId + "))  order by AreaName";
           //      }
           //      else
           //      {
           //          strq = "select AreaId,AreaName from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
           //              " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('BEAT') and underId=" + AreaId + " and Active='1'  order by AreaName";
           //      }
           //   }
            fillDDLDirect(ddlBeat, strq, "AreaId", "AreaName", 1);
        }
        private void BindDistributors(Int32 DistId)
        {
            string str = "";
            if (DistId > 0)
            {
                if (Settings.Instance.RoleType == "Admin")
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
                if (Settings.Instance.RoleType == "Admin")
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
                str = "select IndId,IndName from MastIndustry where (Active='1' or IndId in ("+IndId+" ))  order by IndName"; 
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
        //    if(ddlCity.SelectedValue !="0")
        //    {
        //        ddlArea.Items.Clear();
        //        ddlBeat.Items.Clear();
        //        BindArea(Convert.ToInt32(ddlCity.SelectedValue),0);
        //    }
        //}

        //protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlArea.SelectedValue != "0")
        //    {
        //        BindBeat(Convert.ToInt32(ddlArea.SelectedValue),0);
              
        //    }
        //}
        private void fillRepeter()
        {
            //string RoleType = DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleType from MastRole where RoleId="
            //     + Settings.Instance.RoleID + "").ToString();
            string str = "",straddqry="";
            if ((!string.IsNullOrEmpty(Request.Form[ddlAreaPartylist.UniqueID])) && (Request.Form[ddlAreaPartylist.UniqueID] != "0"))
            {               
                    straddqry = " and mp.AreaId=" + Request.Form[ddlAreaPartylist.UniqueID] + "";               
            }           

            else if (Request.Form[ddlcitylist.UniqueID] != "0")
            {
                straddqry = " and mp.cityId=" + Request.Form[ddlcitylist.UniqueID] + "";
            }

            else if (Request.Form[ddlstatelist.UniqueID] != "0")
            {               
               straddqry = " and mp.cityId in (select Distinct(CityID) from ViewGeo VG where VG.StateId = " + Request.Form[ddlstatelist.UniqueID] + ")";                
            }           
           
            if(Settings.Instance.RoleType == "Admin")
            {
                // optimized Nishu 30/09/2016

//              str = @"SELECT *,city.AreaName AS city,area.AreaName AS Area,beat.AreaName AS beat,Ind.IndName,Mp.ContactPerson,  mp1.PartyName as Distributor, pt.PartyTypeName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId FROM MastParty Mp  LEFT JOIN mastparty mp1
//	          ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId WHERE Mp.PartyDist=0 AND Mp.PartyName<>'.' "+straddqry+"" ;

                str = @"SELECT mp.partyid,mp.partyname,mp.Mobile,city.AreaName AS city,area.AreaName AS Area,beat.AreaName AS beat,Ind.IndName,Mp.ContactPerson,  mp1.PartyName as Distributor, pt.PartyTypeName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId FROM MastParty Mp  LEFT JOIN mastparty mp1
	          ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId WHERE Mp.PartyDist=0 AND Mp.PartyName<>'.' " + straddqry + "";
              
            }
            else
            {
//                str = @"SELECT *,city.AreaName AS city,area.AreaName AS Area,beat.AreaName AS beat,Ind.IndName,Mp.ContactPerson,  mp1.PartyName as Distributor, pt.PartyTypeName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId FROM MastParty Mp  LEFT JOIN mastparty mp1
//	                      ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId 
//                        LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId 
//                        Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId WHERE Mp.PartyDist=0 and Mp.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
//                   " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) AND Mp.PartyName<>'.'" + straddqry + "";

                str = @"SELECT mp.partyid,mp.partyname,mp.Mobile,city.AreaName AS city,area.AreaName AS Area,beat.AreaName AS beat,Ind.IndName,Mp.ContactPerson,  mp1.PartyName as Distributor, pt.PartyTypeName,case Mp.Active when 1 then 'Yes' else 'No' end as Active,Mp.SyncId FROM MastParty Mp  LEFT JOIN mastparty mp1
	                      ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId 
                        LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId 
                        Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId WHERE Mp.PartyDist=0 and Mp.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                  " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) AND Mp.PartyName<>'.'" + straddqry + "";
            }

            // End
            DataTable Partydt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            rpt12.DataSource = Partydt;
            rpt12.DataBind();

            Partydt.Dispose();
        }

        private void FillPartyControls(int PartyId)
        {
            try
            {
                string Partyquery = @"select * from MastParty Mp where Mp.PartyDist=0 and partyId=" + PartyId;

                DataTable PartyValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Partyquery);
                if (PartyValueDt.Rows.Count > 0)
                {
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                    PartyName.Value = PartyValueDt.Rows[0]["PartyName"].ToString();
                    SyncId.Value = PartyValueDt.Rows[0]["SyncId"].ToString();
                    SyncId.Attributes.Add("Disabled", "Disabled");
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

                    HiddenPartyCity.Value = PartyValueDt.Rows[0]["cityid"].ToString();
                    //BindCity(Convert.ToInt32(PartyValueDt.Rows[0]["cityid"]));
                    ddlCity.SelectedValue = PartyValueDt.Rows[0]["cityid"].ToString();

                    HiddenPartyArea.Value = PartyValueDt.Rows[0]["AreaId"].ToString();

                    if (PartyValueDt.Rows[0]["ImgUrl"] != string.Empty)
                    {
                        imgpreview.Src = PartyValueDt.Rows[0]["ImgUrl"].ToString();
                        imgpreview.Style.Add("display", "block");
                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }
                //   BindArea(Convert.ToInt32(PartyValueDt.Rows[0]["cityid"]), Convert.ToInt32(PartyValueDt.Rows[0]["AreaId"]));
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindPartyArea", "BindPartyArea(0);", true);
                    ddlArea.SelectedValue = PartyValueDt.Rows[0]["AreaId"].ToString();

                    HiddenPartyBeat.Value = PartyValueDt.Rows[0]["BeatId"].ToString();

                    // BindPartyArea   BindBeat(Convert.ToInt32(PartyValueDt.Rows[0]["AreaId"]), Convert.ToInt32(PartyValueDt.Rows[0]["BeatId"]));

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindPartyBeat", "BindPartyBeat(0);", true);
                    ddlBeat.SelectedValue = PartyValueDt.Rows[0]["BeatId"].ToString();

                    HiddemPartyType.Value = PartyValueDt.Rows[0]["PartyType"].ToString();
                    ddlpartytype.SelectedValue = PartyValueDt.Rows[0]["PartyType"].ToString();

                    HiddenPartyIndustry.Value = PartyValueDt.Rows[0]["IndId"].ToString();
                   // BindIndustry(Convert.ToInt32(PartyValueDt.Rows[0]["IndId"]));
                    DdlIndustry.SelectedValue = PartyValueDt.Rows[0]["IndId"].ToString();
                    //BindDistributors(Convert.ToInt32(PartyValueDt.Rows[0]["UnderId"]));
                    if (PartyValueDt.Rows[0]["UnderId"].ToString() != "")
                    {
                      //  BindDistributors(Convert.ToInt32(PartyValueDt.Rows[0]["UnderId"]));
                        if (Convert.ToInt32(PartyValueDt.Rows[0]["UnderId"]) > 0)
                            HiddenPartyDistributor.Value = PartyValueDt.Rows[0]["UnderId"].ToString();
                            ddldistributor.SelectedValue = PartyValueDt.Rows[0]["UnderId"].ToString();
                    }
                    HiddenPartyDistributor.Value = PartyValueDt.Rows[0]["UnderId"].ToString();
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
                    GSTINNo.Value = PartyValueDt.Rows[0]["GSTINNo"].ToString();
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindPartyArea", "BindPartyArea();", true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "BindPartyBeat", "BindPartyBeat();", true);
                 
                }
                PartyValueDt.Dispose();
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

       //     string partytypename = Request.Form[HiddemPartyTypetext.UniqueID];
            string PartyType = Request.Form[HiddemPartyType.UniqueID], CityID = Request.Form[HiddenPartyCity.UniqueID], AreaId = Request.Form[HiddenPartyArea.UniqueID],
           BeatID = Request.Form[HiddenPartyBeat.UniqueID], IndustryId = Request.Form[HiddenPartyIndustry.UniqueID], DistributorId = Request.Form[HiddenPartyDistributor.UniqueID], StateId = Request.Form[HiddenStateID.UniqueID];


        string    Partytype_Name = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select PartyTypeName From PartyType where PartyTypeId=" + PartyType + ""));
            if (chkIsAdmin.Checked)
                active = true;

            if (txtDOB.Text != "" && txtDOA.Text != "")
            {
                if (Convert.ToDateTime(txtDOB.Text) >= Convert.ToDateTime(txtDOA.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Date of Anniversary should be greater than Date of Birth!');", true); return;
                }
            }

            //if (!active)
            //    if (string.IsNullOrEmpty(BlockReason.Value))
            //    {
            //        divblock.Attributes.Remove("class");
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter Blocked Reason');", true); return;
            //    }
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
            int retval = 0;
            int RoleId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT RoleId FROM MastRole WHERE RoleType='Distributor'"));
            if (Partytype_Name.ToUpper() == "INSTITUTIONAL")
            {


                retval = DB.InsertDistributors(PartyName.Value, Address1.Value, Address2.Value, Convert.ToString(CityID), Pin.Value, 
                    Email.Value, Mobile.Value, Remark.Value,
                    SyncId.Value, BlockReason.Value, Mobile.Value,active, Phone.Value, RoleId, ContactPerson.Value, CSTNo.Value, VatTin.Value, PanNo.Value, 0, 0, 0, 0, UserId, "", "", PartyName.Value,Convert.ToInt32(Settings.Instance.SMID), Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()),
                    Convert.ToInt32(AreaId), GSTINNo.Value, Imgurl, Convert.ToInt32(PartyType),
                    ServiceTax.Value);

                if (SyncId.Value == "")
                {
                    string syncid = "update MastParty set SyncId='" + retval + "' where partyId=" + retval + " And PartyDist=1";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }

            }
            else
            {

            
             retval = PB.InsertParty(PartyName.Value, Address1.Value, Address2.Value, Convert.ToInt32(CityID), Convert.ToInt32(AreaId),
            Convert.ToInt32(BeatID), Convert.ToInt32(DistributorId), Pin.Value, Mobile.Value, Phone.Value, Remark.Value,
            SyncId.Value, IndustryId, APotential, active, BlockReason.Value, Convert.ToInt32(PartyType),
            ContactPerson.Value, CSTNo.Value, VatTin.Value, ServiceTax.Value, PanNo.Value, UserId,
            Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()), Email.Value, Imgurl,GSTINNo.Value);
            }
            //int retval = PB.InsertParty(PartyName.Value, Address1.Value, Address2.Value, Convert.ToInt32(ddlCity.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue), 
            //    Convert.ToInt32(ddlBeat.SelectedValue),DistID, Pin.Value, Mobile.Value, Phone.Value, Remark.Value, 
            //    SyncId.Value, DdlIndustry.SelectedValue, APotential, active, BlockReason.Value, Convert.ToInt32(ddlpartytype.SelectedValue),
            //    ContactPerson.Value, CSTNo.Value, VatTin.Value, ServiceTax.Value, PanNo.Value, UserId,Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()));
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
                //Mobile.Focus();
            }
            else
            {
                if (SyncId.Value == "")
                {
                    string syncid = "update MastParty set SyncId='" + retval + "' where partyId=" + retval + " And PartyDist=0";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                PartyName.Focus();
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
            string PartyType = Request.Form[HiddemPartyType.UniqueID], CityID = Request.Form[HiddenPartyCity.UniqueID], AreaId = Request.Form[HiddenPartyArea.UniqueID],
            BeatID = Request.Form[HiddenPartyBeat.UniqueID], IndustryId = Request.Form[HiddenPartyIndustry.UniqueID], DistributorId = Request.Form[HiddenPartyDistributor.UniqueID], StateId = Request.Form[HiddenStateID.UniqueID];
            if (chkIsAdmin.Checked)
                 active = true;


            string Partytype_Name = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select PartyTypeName From PartyType where PartyTypeId=" + PartyType + ""));
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
              if (ddldistributor.SelectedValue != "0" && !string.IsNullOrEmpty(ddldistributor.SelectedValue))
              
              { DistID = Convert.ToInt32(ddldistributor.SelectedValue); }
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

              int retval = 0;
              int RoleId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT RoleId FROM MastRole WHERE RoleType='Distributor'"));
              if (Partytype_Name.ToUpper() == "INSTITUTIONAL")
              {


                  retval = DB.UpdateDistributors(Convert.ToInt32(ViewState["PartyId"]), PartyName.Value, Address1.Value, Address2.Value, Convert.ToString(CityID), Pin.Value,
                      Email.Value, Mobile.Value, Remark.Value,
                      SyncId.Value, BlockReason.Value, Mobile.Value, active, Phone.Value, RoleId, ContactPerson.Value, CSTNo.Value, VatTin.Value, PanNo.Value, 0, 0, 0, 0, UserId, "", "", PartyName.Value, Convert.ToInt32(Settings.Instance.SMID), Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()),
                      Convert.ToInt32(AreaId), GSTINNo.Value, Imgurl, Convert.ToInt32(PartyType),
                      "");

                  if (SyncId.Value == "")
                  {
                      string syncid = "update MastParty set SyncId='" + retval + "' where partyId=" + Convert.ToInt32(ViewState["PartyId"]) + " And PartyDist=1";
                      DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                  }

              }
              else
              {
                  retval = PB.UpdateParty(Convert.ToInt32(ViewState["PartyId"]), PartyName.Value, Address1.Value, Address2.Value,
                     Convert.ToInt32(CityID), Convert.ToInt32(AreaId), Convert.ToInt32(BeatID),
                    Convert.ToInt32(DistributorId), Pin.Value, Mobile.Value, Phone.Value, Remark.Value, SyncId.Value,
                     IndustryId, APotential, active, BlockReason.Value, Convert.ToInt32(PartyType),
                     ContactPerson.Value, CSTNo.Value, VatTin.Value, ServiceTax.Value, PanNo.Value, UserId, Convert.ToString(txtDOA.Text.Trim()), Convert.ToString(txtDOB.Text.Trim()), Email.Value, Imgurl, GSTINNo.Value);

              }
            //int retval = PB.UpdateParty(Convert.ToInt32(ViewState["PartyId"]), PartyName.Value, Address1.Value, Address2.Value, 
            //      Convert.ToInt32(ddlCity.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue), Convert.ToInt32(ddlBeat.SelectedValue), 
            //     DistID, Pin.Value, Mobile.Value, Phone.Value, Remark.Value, SyncId.Value,
            //      DdlIndustry.SelectedValue, APotential, active, BlockReason.Value, Convert.ToInt32(ddlpartytype.SelectedValue), 
            //      ContactPerson.Value, CSTNo.Value, VatTin.Value, ServiceTax.Value, PanNo.Value, UserId,Convert.ToString(txtDOA.Text.Trim()),Convert.ToString(txtDOB.Text.Trim()));
          
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
                if (SyncId.Value == "")
                {
                    string syncid = "update MastParty set SyncId='" + retval + "' where partyId=" + ViewState["PartyId"] + " And PartyDist=0";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                PartyName.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
        }
        private void ClearControls()
        {
            //ddlpartytype.SelectedIndex = 0;
            ddlArea.Items.Clear();
            ddlBeat.Items.Clear();
            //ddlCity.SelectedIndex = 0;
            //DdlIndustry.SelectedIndex = 0;
            HiddemPartyType.Value = string.Empty;
            HiddenPartyCity.Value = string.Empty;
            HiddenPartyIndustry.Value = string.Empty;
            HiddenPartyArea.Value = string.Empty;
            HiddenPartyBeat.Value = string.Empty;
            HiddenPartyDistributor.Value = string.Empty;
            HiddenStateID.Value = string.Empty;

            ddldistributor.Items.Clear();
            PartyName.Value = "";
            Potential.Value = "";
            Address1.Value = "";
            Address2.Value = "";
            Pin.Value = "";
            Mobile.Value = "";
            Phone.Value="";
            Remark.Value = "";
            SyncId.Value = "";
            chkIsAdmin.Checked = true;
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
            //Response.Redirect("~/PartyMasterAstral.aspx");
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
                }
                else
                {
                    InsertParty();
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

            BindCity(0);
            BindIndustry(0);
            BindArea(0, 0);
            BindBeat(0, 0);
            BindDistributors(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            //fillRepeter();
            //Request.Form[ddlcitylist.UniqueID] = string.Empty;
            //rpt.DataSource = null;
            //rpt.DataBind();
            //BindState();
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string SelctdArea = Request.Form[ddlAreaPartylist.UniqueID];
            string SelctdCity = Request.Form[ddlcitylist.UniqueID];
            string SelctdState = Request.Form[ddlstatelist.UniqueID];
            if (SelctdState == "0")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please Select States');", true);
                return;
            }           
            fillRepeter();
            if (SelctdArea != "0")
            {
                string strQarea = "select Distinct(AreaID),AreaName from ViewGeo VG where VG.CityId=" + Request.Form[ddlcitylist.UniqueID] + " order by AreaName";
                fillDDLDirect(ddlAreaPartylist, strQarea, "AreaID", "AreaName", 1);
                ddlAreaPartylist.SelectedValue = SelctdArea;
            }
            if (SelctdCity != "0")
            {
                string strQcity = "select Distinct(CityID),CityName from ViewGeo VG where VG.StateId=" + Request.Form[ddlstatelist.UniqueID] + " order by CityName";
                fillDDLDirect(ddlcitylist, strQcity, "CityID", "CityName", 1);
                ddlcitylist.SelectedValue = SelctdCity;
            }
            if (SelctdState != "0")
            {
                string strQstate = "select Distinct(StateID),StateName from ViewGeo VG where VG.StateId=" + Request.Form[ddlstatelist.UniqueID] + " order by stateName";
                fillDDLDirect(ddlstatelist, strQstate, "StateID", "StateName", 1);
                ddlstatelist.SelectedValue = SelctdState;
            }           
            HiddenStateID.Value = SelctdState;
        }

        private void filldt()
        {
            string str = "", straddqry = ""; StringBuilder sb = new StringBuilder(); string col = "";
            if (_exportp == true)
            {
                if ((!string.IsNullOrEmpty(Request.Form[ddlAreaPartylist.UniqueID])) && (Request.Form[ddlAreaPartylist.UniqueID] != "0"))
                {
                    straddqry = " and mp.AreaId=" + Request.Form[ddlAreaPartylist.UniqueID] + "";
                }

                else if (Request.Form[ddlcitylist.UniqueID] != "0")
                {
                    straddqry = " and mp.cityId=" + Request.Form[ddlcitylist.UniqueID] + "";
                }

                else if (Request.Form[ddlstatelist.UniqueID] != "0")
                {
                    straddqry = " and mp.cityId in (select Distinct(CityID) from ViewGeo VG where VG.StateId = " + Request.Form[ddlstatelist.UniqueID] + ")";
                }

                if (Settings.Instance.RoleType == "Admin")
                {

                    str = @"SELECT ROW_NUMBER() over (order by mp.PartyId desc ) as [Sr. No] ,mp.PartyId as [Grahaak_Party_Id],mp.Partyname as [Party Name],city.AreaName AS City,area.AreaName AS Area,beat.AreaName AS Beat,mp.Mobile,Mp.SyncId,Mp.CreditLimit,Mp.OutStanding,FORMAT (mp.Insert_date, 'dd/MMM/yyyy ') as [Created Date],
                ms.Smname as [Created By] FROM MastParty Mp  LEFT JOIN mastparty mp1
	            ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId 
                LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId 
                Left join MastSalesrep ms on mp.created_user_id=ms.userid
                WHERE Mp.PartyDist=0 AND Mp.PartyName<>'.' " + straddqry + " Order By mp.Insert_date desc ";

                }
                else
                {
                    str = @"SELECT ROW_NUMBER() over (order by mp.PartyId desc ) as [Sr. No] ,mp.PartyId as [Grahaak_Party_Id],mp.Partyname as [Party Name],city.AreaName AS City,area.AreaName AS Area,beat.AreaName AS Beat,mp.Mobile,Mp.SyncId,Mp.CreditLimit,Mp.OutStanding,FORMAT (mp.Insert_date, 'dd/MMM/yyyy ') as [Created Date],
                ms.Smname as [Created By] FROM MastParty Mp  LEFT JOIN mastparty mp1
	                      ON mp1.PartyId=mp.UnderId and mp1.PartyDist=1 LEFT JOIN MastArea city ON Mp.CityId=city.AreaId LEFT JOIN MastArea area ON Mp.AreaId=area.AreaId 
                        LEFT JOIN MastArea beat ON Mp.BeatId=beat.AreaId LEFT JOIN MastIndustry Ind ON Mp.IndId=Ind.IndId 
                        Left Join PartyType Pt On Mp.PartyType=Pt.PartyTypeId Left join MastSalesrep ms on mp.created_user_id=ms.userid WHERE Mp.PartyDist=0 and Mp.CityId in (select AreaId from mastarea where areaid in (select underid from mastarea where areaId " +
                      " in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) AND Mp.PartyName<>'.'" + straddqry + " Order By mp.Insert_date desc ";
                }
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
                Response.AddHeader("content-disposition", "attachment;filename=PartyMaster.csv");
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

    }
}