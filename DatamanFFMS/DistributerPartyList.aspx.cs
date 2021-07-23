using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using DAL;
using BAL;
using BusinessLayer;

namespace AstralFFMS
{
    public partial class DistributerPartyList : System.Web.UI.Page
    {
        BAL.DSRLevel1BAL dp = new DSRLevel1BAL();
        string VisitID = "";
        string CityID = "";
        string Level = "";
        string PageButton = "";
        string PageButton2 = "";
        string PageButton3 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
                hidVis.Value = Convert.ToString(VisitID);
            }
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
                hidDist.Value = Convert.ToString(CityID);
            }
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
                hidlevel.Value = Level;
            }
            if (Request.QueryString["PageV"] !=null)
            {
                PageButton = Request.QueryString["PageV"].ToString();
                hidPageName.Value = PageButton;
            }
            if (Request.QueryString["PageL2"] != null)
            {
                PageButton2 = Request.QueryString["PageL2"].ToString();
                hidPageName.Value = PageButton2;
            }
            if (Request.QueryString["PageL3"] != null)
            {
                PageButton3 = Request.QueryString["PageL3"].ToString();
                hidPageName.Value = PageButton3;
            }
            

            if (!IsPostBack)
            {
                BindArea();

                //Added as per UAT on 14-Dec-2015
                if (Session["AreaIDBackBtnDSRL1"] != null && Session["BeatIDBackBtnDSRL1"] != null)
                {
                    string areid = Session["AreaIDBackBtnDSRL1"].ToString();
                    string beatid = Session["BeatIDBackBtnDSRL1"].ToString();
                    ddlarea.SelectedValue = areid;
                    this.BindBeatInArea(areid);
                    ddlbeat.SelectedValue = beatid;
                    fillGrid();
                    Session.Remove("AreaIDBackBtnDSRL1");
                    Session.Remove("BeatIDBackBtnDSRL1");
                }
                if (Settings.Instance.AreaIDDistPartyList != "0" && Settings.Instance.BeatIDDistPartyList != "0")
                {
                    ddlarea.SelectedValue = Settings.Instance.AreaIDDistPartyList;
                    this.BindBeatInArea(Settings.Instance.AreaIDDistPartyList);
                    ddlbeat.SelectedValue = Settings.Instance.BeatIDDistPartyList;
                    fillGrid();
                }
               
            }
            

        }
        //private string GetDistributorCity(int VisiID)
        //{
        //    string st = "select CityId from TransVisit where VisId=" + VisitID;
        //    string CityId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
        //    return CityId;
        //}
        private string GetDistributorCity(string DistId)
        {
            string st = "select CityId from mastparty where PartyId=" + DistId;
            string CityId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return CityId;
        }

        private void BindArea()
        { //Ankita - 10/may/2016- (For Optimization)
//            string str = @"select * from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and
//PrimCode=" + Settings.Instance.DSRSMID + ") and UnderId in (" + CityID + " ) and  areatype='Area' and Active=1 order by AreaName";
            string str = @"select AreaId,AreaName from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and
PrimCode=" + Settings.Instance.DSRSMID + ") and UnderId in (" + CityID + " ) and  areatype='Area' and Active=1 order by AreaName";
            //            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.DSRSMID+ ")) and UnderId="+GetDistributorCity(Convert.ToInt32(VisitID))+" and areatype='Area' and Active=1 order by AreaName ";

            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                ddlarea.DataSource = obj;
                ddlarea.DataTextField = "AreaName";
                ddlarea.DataValueField = "AreaId";
                ddlarea.DataBind();
            }
            ddlarea.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private void BindBeatInArea(string AreaID)
        {//Ankita - 10/may/2016- (For Optimization)
            ddlbeat.Items.Clear();
            //string str = @"select * from mastarea where UnderId=" + AreaID + " order by AreaName";
            string str = @"select AreaId,AreaName from mastarea where UnderId=" + AreaID + " and areatype='Beat' and Active=1 order by AreaName";
            //           string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
            //                      in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.DSRSMID + ")) and UnderId=" + AreaID + " and areatype='Beat' and Active=1 order by AreaName ";
            //  string str = @"select M.AreaId,M.AreaName from mastarea M inner join [TransBeatPlan] P on M.AreaId=p.BeatId  where   P.AppStatus='Approve' and P.SMId=" + Settings.Instance.DSRSMID + "  and  M.UnderId=" + AreaID;

            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                ddlbeat.DataSource = obj;
                ddlbeat.DataTextField = "AreaName";
                ddlbeat.DataValueField = "AreaId";
                ddlbeat.DataBind();
            }
            ddlbeat.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void ddlarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlarea.SelectedIndex > 0)
            {
                this.BindBeatInArea(ddlarea.SelectedValue);
            }
        }
        private void fillGrid()
        {
            if (ddlarea.SelectedIndex > 0)
            {
                string str = "";
                // string str = "select * from MastParty where AreaId="+ddlarea.SelectedValue+" and Active=1 and PartyDist=1 and UnderId=" + DistID;
                if (ddlbeat.SelectedIndex > 0)
                {//Ankita - 10/may/2016- (For Optimization)
                    //str = "select * from MastParty where BeatId=" + ddlbeat.SelectedValue + " and Active=1 and PartyDist=0";
                    str = "select PartyId,AreaId,PartyName,ContactPerson,Mobile from MastParty where BeatId=" + ddlbeat.SelectedValue + " and Active=1 and PartyDist=0";
                    DataTable obj = new DataTable();
                    obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (obj.Rows.Count > 0)
                    {
                        rpt.DataSource = obj;
                        rpt.DataBind();
                        Settings.Instance.AreaName = ddlbeat.SelectedItem.Text;
                        Settings.Instance.BeatName = ddlarea.SelectedItem.Text;
                        //Added 
                        Settings.Instance.AreaIDDistPartyList = ddlarea.SelectedValue;
                        Settings.Instance.BeatIDDistPartyList = ddlbeat.SelectedValue;
                        //End
                    }
                    else
                    {
                        rpt.DataSource = null;
                        rpt.DataBind();
                        //Settings.Instance.AreaName = ddlarea.SelectedItem.Text;
                        //Settings.Instance.BeatName = ddlbeat.SelectedItem.Text;
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select the Beat');", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select the Area');", true);
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            fillGrid();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {

            if (Level == "1")
            {
                Response.Redirect("DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
           
            else if (Level == "2")
            {
                Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
            else
            {
                Response.Redirect("DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
            // Response.Redirect("~/DSREntryForm1.aspx?CityID=" + CityID + " &VisitID=" + VisitID);
        }

        protected void btnBack_Click1(object sender, EventArgs e)
        {
            if (Level == "1" && PageButton == "Secondary")
            {
                Response.Redirect("DSREntryForm.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageV=Secondary");
            }
            if (Level == "1")
            {
                Response.Redirect("DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
            if (Level == "2" && PageButton2 == "Secondary")
            {
                Response.Redirect("DSREntryFormLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageL2=Secondary");
            }
           if (Level == "2")
            {
                Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
           else if (Level == "3" && PageButton3 == "Secondary")
           {
               Response.Redirect("DSRVistEntry.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageL3=Secondary");
           }
            else
            {
                Response.Redirect("DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void ddlbeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlarea.SelectedValue = "0";
            ddlbeat.SelectedValue = "0";
            rpt.DataSource = new DataTable();
            rpt.DataBind();
        }

        protected void btnAddnewParty_Click(object sender, EventArgs e)
        {
            Session["AreaIDDSRL1"] = string.Empty;
            Session["BeatIDDSRL1"] = string.Empty;
            if (ddlarea.SelectedValue != "0" && ddlbeat.SelectedValue != "0")
            {
                this.Session["mySelectedValue"] = ddlarea.SelectedValue.ToString();
                Session["AreaIDDSRL1"] = ddlarea.SelectedValue.ToString();
                Session["BeatIDDSRL1"] = ddlbeat.SelectedValue.ToString();
                Response.Redirect("~/AddNewParty.aspx?CityID=" + hidDist.Value + " &VisitID=" + hidVis.Value + "&Level=" + hidlevel.Value);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select area and beat.');", true);
                return;
            }
        }       


    }
}