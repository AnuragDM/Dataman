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
    public partial class View_EmplyeeExpenseTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (!IsPostBack)
            {
                BindSalesPersons(); BindLodgingBoarding(); BindLocalConveyanceLimt(); BindCityConveyanceType(); BindConveyanceMode(); BindTravelMode(); BindMetroCity(); BindACity(); BindBCity();
            }
        }
        #region BindDropdown
        private void BindSalesPersons()
        {
            string str = "";
            if (Settings.Instance.UserName.ToUpper() == "SA") { str = "select SMID,SMName from MastSalesRep order by SMNAme"; }
            else
            {
                str = "select SMID,SMName from MastSalesRep where UnderId =" + Settings.Instance.SMID + " or smid=" + Settings.Instance.SMID + " order by SMID ";
            }
            fillDDLDirect(ddlsalesman, str, "SMID", "SMName", 1);
            ddlsalesman.SelectedValue = Settings.Instance.SMID;
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

        #region BindGrids
        private void BindLodgingBoarding()
        {
            string strLB = "";
            if (Settings.Instance.UserName.ToUpper() == "SA") { strLB = "select Mct.Name,DesName,Amount,lbl.Remarks from MastLocalLodgingBoardingLimit Lbl inner join MastDesignation Md on lbl.DesId=Md.DesId inner join MastCityType Mct on Lbl.CityTypeId=Mct.Id where Lbl.Active=1 order by Mct.Name"; }
            else
            {
                strLB = "select Mct.Name,DesName,Amount,Lbl.Remarks from MastLocalLodgingBoardingLimit Lbl inner join MastDesignation Md on lbl.DesId=Md.DesId inner join MastCityType Mct on Lbl.CityTypeId=Mct.Id where Lbl.Active=1 and lbl.DesId=" + Settings.Instance.DesigID + " order by Mct.Name";
            }
            gdvloadgibgboarding.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strLB);
            gdvloadgibgboarding.DataBind();
          
        }
        private void BindLocalConveyanceLimt()
        {
            string strLB = "";
            if (Settings.Instance.UserName.ToUpper() == "SA") { strLB = "select Mct.Name,DesName,Amount,lbl.Remarks from MastLocalConveyanceLimt Lbl inner join MastDesignation Md on lbl.DesId=Md.DesId inner join MastCityType Mct on Lbl.CityTypeId=Mct.Id where Lbl.Active=1 order by Mct.Name"; }
            else
            {
                strLB = "select Mct.Name,DesName,Amount,Lbl.Remarks from MastLocalConveyanceLimt Lbl inner join MastDesignation Md on lbl.DesId=Md.DesId inner join MastCityType Mct on Lbl.CityTypeId=Mct.Id where Lbl.Active=1 and  lbl.DesId=" + Settings.Instance.DesigID + " order by Mct.Name";
            }
            gdvLocalConveyanceLimt.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strLB);
            gdvLocalConveyanceLimt.DataBind();
        }
        private void BindTravelMode()
        {//Ankita - 11/may/2016- (For Optimization)
           // string strLB = "select * from MastTravelMode where Active=1 and IsTravelConveyance=1 Order By Name";
            string strLB = "select Name from MastTravelMode where Active=1 and IsTravelConveyance=1 Order By Name";
            gdvModeofTravel.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strLB);
            gdvModeofTravel.DataBind();
        }
        private void BindConveyanceMode()
        {//Ankita - 11/may/2016- (For Optimization)
            //string strLCM = "select * from MastTravelMode where Active=1 and IsTravelConveyance=0 Order By Name";
            string strLCM = "select Name,PerKmRate from MastTravelMode where Active=1 and IsTravelConveyance=0 Order By Name";
            gdvConvMode.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strLCM);
            gdvConvMode.DataBind();
        }
        private void BindCityConveyanceType()
        {
           //Ankita - 11/may/2016- (For Optimization)
            //string strLB = "select * from MastCityConveyanceType Order By Name";
            string strLB = "select Name from MastCityConveyanceType Order By Name";
            gdvCityConveyanceType.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strLB);
            gdvCityConveyanceType.DataBind();
        }
        private void BindMetroCity()
        {
            string strMT = "SELECT Distinct statename,cityname FROM ViewGeo WHERE cityid IN ( SELECT areaid FROM MastArea ma INNER JOIN MastCityType mct ON ma.CityType=mct.Id WHERE AreaType ='CITY' AND mct.Name='MEGA METRO') ORDER BY stateName,cityname";

            gdvmetrocity.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strMT);
            gdvmetrocity.DataBind();
        }
        private void BindACity()
        {
            string strAT = "SELECT Distinct statename,cityname FROM ViewGeo WHERE cityid IN ( SELECT areaid FROM MastArea ma INNER JOIN MastCityType mct ON ma.CityType=mct.Id WHERE AreaType ='CITY' AND mct.Name='A') ORDER BY stateName,cityname";

            gdvAcity.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strAT);
            gdvAcity.DataBind();
        }
        #endregion
        private void BindBCity()
        {
            string strBT = "SELECT Distinct statename,cityname FROM ViewGeo WHERE cityid IN ( SELECT areaid FROM MastArea ma INNER JOIN MastCityType mct ON ma.CityType=mct.Id WHERE AreaType ='CITY' AND mct.Name='B') ORDER BY stateName,cityname";

            gdvBcity.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, strBT);
            gdvBcity.DataBind();
        }
    }
}