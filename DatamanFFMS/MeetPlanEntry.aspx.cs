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
using System.Collections;
using System.IO;
namespace AstralFFMS
{
    public partial class MeetPlanEntry : System.Web.UI.Page
    {
        BAL.Meet.MeetPlanEntryBAL ME = new BAL.Meet.MeetPlanEntryBAL();
        string mode = "";
        string MeetPlanID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["MeetPlanID"]!=null) MeetPlanID = Request.QueryString["MeetPlanID"];
            if (txtVenue.Text != "")
            {
                string Venue = txtVenue.Text;
                txtVenue.Attributes.Add("value", Venue);
            }

            if (txtComments.Text != "")
            {
                string Comments = txtComments.Text;
                txtComments.Attributes.Add("value", Comments);
            }
            txtMeetDate.Attributes.Add("readonly", "readonly");
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            mode = Request.QueryString["Mode"];
            CalendarExtender1.StartDate = DateTime.Now;
           
           
            if (btnsave.Text == "Save")
            {
                btnsave.Enabled = Convert.ToBoolean(SplitPerm[1]);
              //  btnsave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnsave.CssClass = "btn btn-primary";
            }
            else
            {
                btnsave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                //btnsave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnsave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btnDelete.CssClass = "btn btn-primary";
         
           // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
           // btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                try
                {              
                    fillUnderUsers();
                    txtMeetDate.Text = DateTime.Parse(DateTime.UtcNow.AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                    fillArea();
                    BindIndustry();
                    fillMeetType();
                    fillAreaType();
                    fillscheme();
                    BindDDlCity();
                    fillGiftType();
                    //fillBeat();
                    //this.fillDist();
                    btnDelete.Visible = false;
                    fillMeetType1();
                    fillUnderUsers1();
                    fillDist();
                    FillClass();
                    FillGroup();
                    FillSegment();
                    // fillInitialRecords();
                    SetInitialRow();
                  //  ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
                    mainDiv.Style.Add("display", "block");
                    secDiv.Style.Add("display", "none");
            //        fillPartyByBeat();                   
                    //txtParty.ReadOnly = true;

                 
                   
                    if (mode == "Edit")
                    {
                        FillMeet(MeetPlanID);
                    }
                    
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                CalendarExtender1.StartDate = DateTime.UtcNow.AddDays(-Settings.GetMeetDays(Settings.DMInt32(ddlunderUser.SelectedValue)));
            }
            if (mode == "Edit")
            {
                string str = "select * from TransMeetPlanEntry  where MeetPlanId=" + MeetPlanID;
                DataTable Meetdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (Meetdt.Rows.Count > 0)
                {
                    if (Meetdt.Rows[0]["AppStatus"].ToString() == "Approve" || Meetdt.Rows[0]["AppStatus"].ToString() == "Reject")
                    {
                        btnsave.Enabled = false;
                        btnDelete.Visible = true;
                        btnDelete.Enabled = false;
                        btnsave.CssClass = "btn btn-primary";
                        btnDelete.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        //        btnSave.Enabled = false;
                        //Added 08-12-2015
                        btnsave.Enabled = true;
                        btnsave.Text = "Update";
                        //End

                        btnDelete.Enabled = true;
                        btnsave.CssClass = "btn btn-primary";
                        // btnFind.Visible = true;
                        btnDelete.Visible = true;
                    }
                }
            }
        }
        private void FillMeet(string MeetPlanID)
        {
            try
            {string strNew ="";
                DataTable dt=null;
                //string str = "select ma.AreaName as BeatName,ci.AreaName as City,ms.SMName, tp.* from TransMeetPlanEntry tp 	inner join MastArea ma on tp.AreaId=ma.AreaId	inner join MastArea ci on tp.AreaId=ci.AreaId	inner join MastSalesRep ms on tp.SMId=ms.SMId where MeetPlanId=" + MeetPlanID;
                string str = "select m.*,mmtg.Id as TypeId,mmtg.Name as typeOfGiftEnduser from TransMeetPlanEntry m left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id   where m.MeetPlanId=" + MeetPlanID;
                DataTable Meetdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (Meetdt.Rows.Count > 0)
                {
                    

                    ddlmeetType.SelectedValue = Meetdt.Rows[0]["MeetTypeId"].ToString();
                    ddlindrustry.SelectedValue = Meetdt.Rows[0]["IndId"].ToString();
                    Hidparty.Value = Meetdt.Rows[0]["RetailerPartyID"].ToString();
                    txtNoOfUsers.Text = Meetdt.Rows[0]["NoOfUser"].ToString();
                    txtVenue.Text = Meetdt.Rows[0]["Venue"].ToString();
                    txtComments.Text = Meetdt.Rows[0]["Comments"].ToString();
                    ddlTOG.SelectedValue = Meetdt.Rows[0]["TypeOfGiftId"].ToString();
                    ddlTOG.Text = Meetdt.Rows[0]["typeOfGiftEnduser"].ToString();
                    txtvalueforenduser.Text = Meetdt.Rows[0]["valueofEnduser"].ToString();
             
                    txtgiftqty.Text = Meetdt.Rows[0]["valueofRetailer"].ToString();



                    this.fillDist();
                    //FillClass();
                    //FillGroup();
                    //FillSegment();

                    //Added As Per UAT -  07-12-2015
                   
                    //End

                    txtMeetDate.Text = DateTime.Parse(Meetdt.Rows[0]["MeetDate"].ToString()).ToString("dd/MMM/yyyy");
                  
                    txtApproxBudget.Text = Meetdt.Rows[0]["LambBudget"].ToString();
                    txtVenue.Text = Meetdt.Rows[0]["VenueId"].ToString();
                    txtDistributerSharing.Text = Meetdt.Rows[0]["ExpShareDist"].ToString();
                    txtastralSharing.Text = Meetdt.Rows[0]["ExpShareSelf"].ToString();
                    ddlscheme.SelectedItem.Value = Meetdt.Rows[0]["SchId"].ToString();
                    //ddlscheme.SelectedValue = Meetdt.Rows[0]["SchId"].ToString();
                    txtNoofStaf.Text = Meetdt.Rows[0]["NoStaff"].ToString();
                    if (Meetdt.Rows[0]["SMId"] != DBNull.Value)
                    {
                        ddlunderUser.SelectedItem.Value = Meetdt.Rows[0]["SMId"].ToString();
                         strNew = " select SMName from MastSalesRep where SMId=" + Meetdt.Rows[0]["SMId"].ToString();
                         dt = new DataTable();
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, strNew);
                       // ddlunderUser.SelectedItem.Text = dt.Rows[0]["SMName"].ToString();
                    }
                   
                    this.BindDDlCity();
                    if (Meetdt.Rows[0]["MeetLoaction"] != DBNull.Value)
                    {
                        ddlmeetCity.SelectedItem.Value = Meetdt.Rows[0]["MeetLoaction"].ToString();
                         strNew = "Select AreaName as City from MastArea where AreaId=" + Meetdt.Rows[0]["MeetLoaction"].ToString() + " and AreaType='City'";
                         dt = new DataTable();
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, strNew);
                       ddlmeetCity.SelectedItem.Text = dt.Rows[0]["City"].ToString();
                    }
                    this.fillBeat();
                   

                  //  fillDisName(Convert.ToInt32(Meetdt.Rows[0]["RetailerPartyID"].ToString()));
                    if (Meetdt.Rows[0]["AreaId"] != DBNull.Value)
                    {
                        ddlbeat5.SelectedItem.Value = Meetdt.Rows[0]["AreaId"].ToString();
                         strNew = "Select AreaName as Beat from MastArea where AreaId=" + Meetdt.Rows[0]["AreaId"].ToString() + " and AreaType='Beat'";
                         dt = new DataTable();
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, strNew);
                        if (dt.Rows[0]["Beat"].ToString() != "0") ddlbeat5.SelectedItem.Text = dt.Rows[0]["Beat"].ToString();
                      
                        
                    }
                    fillPartyByBeat();

                    //if (Meetdt.Rows[0]["RetailerPartyID"] != DBNull.Value)
                    //{
                    //    ddlParty.SelectedItem.Value = Meetdt.Rows[0]["RetailerPartyID"].ToString();
                    //     strNew = "Select PartyName from MastParty where PartyId=" + Meetdt.Rows[0]["RetailerPartyID"].ToString();
                    //     dt = new DataTable();
                    //    dt = DbConnectionDAL.GetDataTable(CommandType.Text, strNew);
                    //   if( dt.Rows.Count>0)ddlParty.SelectedItem.Text = dt.Rows[0]["PartyName"].ToString();
                    //}
                    ddlParty.ClearSelection();
                    string[] accStaffAll = new string[50];
                    string PartyId = Meetdt.Rows[0]["PartyId"].ToString();
                    accStaffAll = PartyId.Split(',');
                    if (accStaffAll.Length > 0)
                    {
                        foreach (ListItem item in ddlParty.Items)
                        {
                            for (int i = 0; i < accStaffAll.Length; i++)
                            {
                                if (item.Value == accStaffAll[i].ToString())
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }

                    str = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
                 left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
                 left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + MeetPlanID;
                    DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    ViewState["CurrentTable"] = dt1;
                    if (dt1.Rows.Count > 0)
                    {
                        GridView2.DataSource = dt1;
                        GridView2.DataBind();
                    }
                    //ddlmeetCity.SelectedItem.Value = Meetdt.Rows[0]["MeetLoaction"].ToString();
                    //ddlmeetCity.SelectedItem.Text = Meetdt.Rows[0]["MeetLoaction"].ToString();
                    ddlunderUser.Enabled = false;
                    this.fillDist();
                    if (Meetdt.Rows[0]["DistId"] != DBNull.Value)
                    {
                        int partyId = Convert.ToInt32(Meetdt.Rows[0]["DistId"]);
                        hfDistId.Value = partyId.ToString();
                         strNew = "  Select PartyName from MastParty where PartyId=" + partyId + " and PartyDist=1";
                         dt = new DataTable();
                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, strNew);
                        if(partyId==0)
                        {

                            //Ddldistributor.Items.Insert(0, new ListItem("-- Select --", "0"));
                            ////Ddldistributor.SelectedItem.Text = dt.Rows[0]["PartyName"].ToString();
                            ////Ddldistributor.SelectedItem.Value = Convert.ToString(partyId);
                        }
                        else {
                            Ddldistributor.SelectedItem.Value = Convert.ToString(partyId);
                            Ddldistributor.SelectedItem.Text = dt.Rows[0]["PartyName"].ToString();
                       
                        }
                        //string strNew = "select  mp.*,ma.AreaName FROM MastParty mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyId=" + partyId + " and mp.PartyDist=1";
                        //DataTable dt = new DataTable();

                        //dt = DbConnectionDAL.GetDataTable(CommandType.Text, strNew);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    txtDist.Text = "(" + dt.Rows[0]["PartyName"].ToString() + ")" + " " + dt.Rows[0]["SyncId"].ToString() + " " + "(" + dt.Rows[0]["AreaName"].ToString() + ")";
                        //}
                    }
                    else { Ddldistributor.Text = string.Empty; }

                   
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void fillUnderUsers()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    //Ankita - 20/may/2016- (For Optimization)
                    //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                    //DataTable dtRole = new DataTable();
                    //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                    //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                    string RoleTy = Settings.Instance.RoleType;
                    if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                       {
                           ddlunderUser.SelectedValue = Settings.Instance.SMID;
                       }
                }
                catch { }

            }
        }
        private void fillDist()
        {
                try
                {
                    Ddldistributor.Items.Clear();
                   // string distqry = @"select PartyId,PartyName from MastParty where  PartyDist=1 and Active=1 order by PartyName";
                    string distqry = @"select  mastparty.PartyId, mastparty.PartyName + '-' + [city].areaName + '-' +[state].areaName + '-' +mastparty.SyncId 
                                             AS PartyName from mastparty 
                                          left join mastarea as [city] on [city].areaid=mastparty.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid
                                          where partydist=1 order by PartyName";
                    DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
                    if (dtDist.Rows.Count > 0)
                    {
                        Ddldistributor.DataSource = dtDist;
                        Ddldistributor.DataTextField = "PartyName";
                        Ddldistributor.DataValueField = "PartyId";
                        Ddldistributor.DataBind();
                    }
                   
                }
                catch { }

                Ddldistributor.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void BindDDlCity()
        {
            ddlmeetCity.Items.Clear();
//            string str = @"select * from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
//                          and PrimCode=" + ddlunderUser.SelectedValue + ")) and  areatype='city' and Active=1 order by AreaName";
            //Ankita - 20/may/2016- (For Optimization)
            string str = @"select AreaId,AreaName from mastarea where areaid in (select underid from mastarea where areaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA'
                          and PrimCode=" + ddlunderUser.SelectedValue + ")) and  areatype='city' and Active=1 order by AreaName";
//            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
//                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlunderUser.SelectedValue + ")) and areatype='City' and Active=1 order by AreaName ";

            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                try
                {
                    ddlmeetCity.DataSource = obj;
                    ddlmeetCity.DataTextField = "AreaName";
                    ddlmeetCity.DataValueField = "AreaId";
                    ddlmeetCity.DataBind();

                    //Added AS Per UAT 07-12-2015
                    string cityIDStr = "";
                    foreach (DataRow dr in obj.Rows)
                    {
                        cityIDStr = cityIDStr + "," + Convert.ToString(dr["AreaId"]);
                        //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                    }
                    cityIDStr = cityIDStr.TrimStart(',').TrimEnd(',');
                    hdfCityIdStr.Value = cityIDStr;
                    //End
                
                }
                catch { }
            }
           // if (mode != "Edit") 
                ddlmeetCity.Items.Insert(0, new ListItem("-- Select --", "0"));
           
        }

        private void BindIndustry()
        {//Ankita - 20/may/2016- (For Optimization)
            ddlindrustry.Items.Clear();
            //string str = "select * from MastItemClass order by Name";
            string str = "select Id,Name from MastItemClass order by Name";
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (obj.Rows.Count > 0)
            {
                ddlindrustry.DataSource = obj;
                ddlindrustry.DataTextField = "Name";
                ddlindrustry.DataValueField = "Id";
                ddlindrustry.DataBind();
            }
            ddlindrustry.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillscheme()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = "select * from MastScheme order by Name";
            string str = "select Id,Name from MastScheme order by Name";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlscheme.DataSource = dt;
                ddlscheme.DataTextField = "Name";
                ddlscheme.DataValueField = "Id";
                ddlscheme.DataBind();
            }
            ddlscheme.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
       
        private void fillArea()
        {
            //string beatQuery2 = "select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode="
            //          +Settings.Instance.SMID + ")) and areatype='Area' order by AreaName";
            //DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, beatQuery2);
            //if (dtBeat.Rows.Count > 0)
            //{
            //    ddlarea5.DataSource = dtBeat;
            //    ddlarea5.DataTextField = "AreaName";
            //    ddlarea5.DataValueField = "AreaId";
            //    ddlarea5.DataBind();
            //}
            //ddlarea5.Items.Insert(0, new ListItem("-- Select Area --", "0"));
        }

        private void fillBeat()
        {//Ankita - 20/may/2016- (For Optimization)
//            string str = @" select * from mastarea where UnderId in 
//(select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlunderUser.SelectedValue + ") and  areatype='beat' and Active=1 order by AreaName";
//            string str = @" select AreaId,AreaName from mastarea where UnderId in 
//(select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + ddlunderUser.SelectedValue + ") and  areatype='beat' and Active=1 order by AreaName";

//            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
//                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.SMID+ ")) and areatype='Beat' and Active=1 order by AreaName ";
            ddlbeat5.Items.Clear();
            string str = @" select BeatId,BeatName from ViewGeo where CityId in(" + ddlmeetCity.SelectedValue + ") order by BeatName";
            DataTable dtBeat = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dtBeat.Rows.Count > 0)
            {
                ddlbeat5.DataSource = dtBeat;
                ddlbeat5.DataTextField = "BeatName";
                ddlbeat5.DataValueField = "BeatId";
                ddlbeat5.DataBind();
            }
           // if (mode != "Edit") 
                ddlbeat5.Items.Insert(0, new ListItem("-- Select Beat --", "0"));
        }
        private void fillPartyByBeat()
        {            //Ankita - 20/may/2016- (For Optimization)
            //string str = "select * FROM MastParty where PartyDist=0 and BeatId=" + ddlbeat5.SelectedValue + " Order by Partyname";
            ddlParty.Items.Clear();
            string str = "select PartyId,PartyName FROM MastParty where PartyDist=0 and Active=1 and BeatId=" + ddlbeat5.SelectedValue + " Order by Partyname";
            DataTable dtParty = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dtParty.Rows.Count > 0)
            {
                ddlParty.DataSource = dtParty;
                ddlParty.DataTextField = "PartyName";
                ddlParty.DataValueField = "PartyId";
                ddlParty.DataBind();
            }
           // ddlParty.Items.Insert(0, new ListItem("-- Select Party --", "0"));
        }
        private void fillAreaType()
        {
            //string str = "select * from MastMeetAreaType";

            //DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //if (dt.Rows.Count > 0)
            //{
            //    ddlmeetLocation.DataSource = dt;
            //    ddlmeetLocation.DataTextField = "Name";
            //    ddlmeetLocation.DataValueField = "Id";
            //    ddlmeetLocation.DataBind();
            //}
            //ddlmeetLocation.Items.Insert(0, new ListItem("-- Select Area Type --", "0"));
        }
        private void fillGiftType()
        {
            string str = "select * from MastMeetTypeOfGift Order By Name";

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlTOG.DataSource = dt;
                ddlTOG.DataTextField = "Name";
                ddlTOG.DataValueField = "Id";
                ddlTOG.DataBind();
            }
            ddlTOG.Items.Insert(0, new ListItem("-- Select Gift Type --", "0"));
        }
        private void fillMeetType()
        {  //Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetType order by Name";
            string query = "select Id,Name from MastMeetType order by Name";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }



        protected void btnAddparty_Click(object sender, EventArgs e)
        {

        }
        private void fillParty()
        {
            //var query = from h in context.MastParties.Where(u => u.AreaId == Convert.ToInt32(ddlareaSearch.SelectedValue))
            //            select h;
            //DataTable dt = BusinessClass.LINQResultToDataTable(query);
            //if (dt.Rows.Count > 0)
            //{
            //    GridView1.DataSource = dt;
            //    GridView1.DataBind();

            //}
            //else
            //{
            //    GridView1.DataSource = null;
            //    GridView1.DataBind();
            //}

        }

        //private void fillPartyBeat()
        //{
        //    string query = "select * from MastParty where BeatId="+ddlbeat5.SelectedValue;
        //    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
        //    if (dt.Rows.Count > 0)
        //    {
               
        //    }
        //    else
        //    {
              
        //    }

        //}

        //protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    int index = Convert.ToInt32(e.RowIndex);
        //    DataTable dt = (DataTable)ViewState["CurrentTable"];
        //    dt.Rows[index].Delete();
        //    dt.AcceptChanges();
        //    ViewState["CurrentTable"] = dt;
        //}
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = (DataTable)ViewState["CurrentTable"];

            dt.Rows[index].Delete();
            dt.AcceptChanges();
            ViewState["CurrentTable"] = dt;
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }
        private int  CheckMeet()
        {
            try
            {
                if (mode != "Edit")
                {
                    string str = "select count(*) from TransMeetPlanEntry where SMId=" + ddlunderUser.SelectedValue + " and MeetTypeId=" + ddlmeetType.SelectedValue + " and MeetDate='" + Settings.dateformat(txtMeetDate.Text) + "' And AppStatus !='Cancel'";
                    int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                    return exists;
                }
                else
                { return 0; }
            }
            catch
            {
                return 0;

            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //rpt.DataSource = null;
            //rpt.DataBind();

            ////Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            //txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            //txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            ////End

            ////txtfmDate.Text = string.Empty;
            ////txttodate.Text = string.Empty;
            //mainDiv.Style.Add("display", "none");
            //rptmain.Style.Add("display", "block");
            Response.Redirect("~/MeetPlanList.aspx?Pagename=Meetplanentry");

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            //if (GridView2.Rows.Count == 0)
            //{
            //  //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter meet product before save entry');", true);
            //    mainDiv.Style.Add("display", "none");
            //    secDiv.Style.Add("display", "block");
            //   // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
            //    return;
            //}
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            int RetSave =1;
            btnsave.Enabled = false;
            string partyId = getSelectedParty();
            try
            {
                decimal s = Settings.DMDecimal(txtastralSharing.Text) + Settings.DMDecimal(txtDistributerSharing.Text);
                if (s != 100)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Total sharing percentage should be 100 %');", true);
                }
                else
                {
                    
                    if (CheckMeet() > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This meet already Exists');", true);
                    }
                   
                    else
                    {
                        string docID = Settings.GetDocID("MPLAN", DateTime.Now);
                        Settings.SetDocID("MPLAN", docID);

                        string DistributorpartyId = "0";

                        //  DistributorpartyId =hfCustomerId.Value;

                        //Added As Per UAT 07-12-2015 
                         string distId1 = "";
                        if (Ddldistributor.SelectedValue != string.Empty)
                        {
                            //DistributorpartyId = hfDistId.Value;
                            DistributorpartyId =Ddldistributor.SelectedValue;
                        }
                        //End

                        if (mode != "Edit")
                        {

                            if (GridView2.Rows.Count > 0)
                            {
                                RetSave = ME.Insert(docID, Settings.DMInt32(Settings.Instance.UserID), Settings.DMInt32(ddlunderUser.SelectedValue), Settings.DMInt32(ddlbeat5.SelectedValue), Settings.DMInt32(ddlmeetType.SelectedValue), Settings.DMInt32("0"), Settings.DMInt32(ddlindrustry.SelectedValue), Settings.DMInt32(DistributorpartyId), Settings.DMInt32(txtNoOfUsers.Text), "", "", txtVenue.Text, txtComments.Text, Settings.DMInt32(ddlTOG.SelectedValue), Settings.DMDecimal(txtvalueforenduser.Text), "", Settings.DMDecimal(txtgiftqty.Text), txtMeetDate.Text, (ddlmeetType.SelectedItem.Text + " " + txtMeetDate.Text), ddlmeetCity.SelectedValue, Settings.DMDecimal(txtApproxBudget.Text), Settings.DMInt32("0"), txtVenue.Text, Settings.DMDecimal(txtDistributerSharing.Text), Settings.DMDecimal(txtastralSharing.Text), Settings.DMInt32(ddlscheme.SelectedValue), Settings.DMInt32(txtNoofStaf.Text), true, "Pending", Settings.DMInt32(ddlParty.SelectedValue), partyId);
                                this.InsertMeetProduct(RetSave);
                                this.InsertMeetRequest(RetSave);
                            }
                            else {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter meet product before save entry');", true);
                            }
                          
                        }
                        else
                        {
                           
                           // this.InsertMeetRequest(Settings.DMInt32(Request.QueryString["MeetPlanID"]));
                            if (GridView2.Rows.Count > 0)
                            {
                                RetSave = ME.Update(Settings.DMInt32(Request.QueryString["MeetPlanID"]), docID, Settings.DMInt32(Settings.Instance.UserID), Settings.DMInt32(ddlunderUser.SelectedValue), Settings.DMInt32(ddlbeat5.SelectedValue), Settings.DMInt32(ddlmeetType.SelectedValue), Settings.DMInt32("0"), Settings.DMInt32(ddlindrustry.SelectedValue), Settings.DMInt32(DistributorpartyId), Settings.DMInt32(txtNoOfUsers.Text), "", "", txtVenue.Text, txtComments.Text, Settings.DMInt32(ddlTOG.SelectedValue), Settings.DMDecimal(txtvalueforenduser.Text), "", Settings.DMDecimal(txtgiftqty.Text), txtMeetDate.Text, (ddlmeetType.SelectedItem.Text + " " + txtMeetDate.Text), ddlmeetCity.SelectedValue, Settings.DMDecimal(txtApproxBudget.Text), Settings.DMInt32("0"), txtVenue.Text, Settings.DMDecimal(txtDistributerSharing.Text), Settings.DMDecimal(txtastralSharing.Text), Settings.DMInt32(ddlscheme.SelectedValue), Settings.DMInt32(txtNoofStaf.Text), true, "Pending", Settings.DMInt32(ddlParty.SelectedValue), partyId);
                                 this.InsertMeetProduct(Convert.ToInt32(Request.QueryString["MeetPlanID"]));
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please enter meet product before save entry');", true);
                            }
                        }
                        
                        if (RetSave > 0)
                        {
                            //if (btnsave.Text == "Save") System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            //else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                            //ViewState["CurrentTable"] = null;
                            var page = HttpContext.Current.CurrentHandler as Page;
                            if (btnsave.Text == "Save") ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Record Inserted Successfully');window.location ='MeetPlanEntry.aspx';", true);
                            else ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Record Updated Successfully');window.location ='MeetPlanEntry.aspx';", true);
                            //Response.Redirect("MeetPlanEntry.aspx");
                            //reset();
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);

                        }
                    }
                }
               
            }
            catch (Exception ex) { ex.ToString(); }
            btnsave.Enabled = true;

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            //string confirmValue = Request.Form["confirm_value"];
            //if (confirmValue == "Yes")
            //{
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                //Ankita - 09/may/2016- (For Optimization)
                // string appStatus = @"select * from TransLeaveRequest where LVRQId='" + Convert.ToString(ViewState["lvrQId"]) + "' and AppStatus='Pending'";
                //DataTable deleteTLRdt = DbConnectionDAL.GetDataTable(CommandType.Text, appStatus);
               string appStatus = @"select count(*) from [TransMeetPlanEntry] where MeetPlanId='" + Convert.ToString(Request.QueryString["MeetPlanID"]) + "' and AppStatus='Pending'";
                int deleteTLR = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, appStatus));
                //   if (deleteTLRdt.Rows.Count > 0)
                if (deleteTLR > 0)
                {

                   // int retdel = ME.delete(Convert.ToString(Request.QueryString["MeetPlanID"]));
                         string strDelete = "Delete from TransMeetPlanEntry Where MeetPlanId=" + Settings.DMInt32(Request.QueryString["MeetPlanID"]) + "";
                         DbConnectionDAL.ExecuteNonQuery(CommandType.Text,strDelete);
                        string msgUrl = "MeetApproval.aspx?SMId=" + ddlunderUser.SelectedValue + "&MeetPlanId=" + Convert.ToString(Request.QueryString["MeetPlanID"]);
                        string qry = "delete from TransNotification where msgURL= '" + msgUrl + "'";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errormessage", "errormessage('Record Deleted Successfully');window.location ='MeetPlanEntry.aspx';", true);
                       // System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                        reset();
                       
                }
                else
                {

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Meet is either Approved or Rejected.');", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
                    FillMeet(Request.QueryString["MeetPlanID"]);
                    //FillLeaveControls(Convert.ToInt32(ViewState["lvrQId"]));
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
                //Response.Redirect("~/MeetPlanEntry.aspx");
            //}
            //else
            //{
            //    FillMeet(Request.QueryString["MeetPlanID"]);
            //    //      this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            //    // FillLeaveControls(Convert.ToInt32(ViewState["lvrQId"]));
            //}
        }


        private void InsertMeetRequest(int RetSave)
        {
            try
            {
                //Int64 retsave = lvAll.Insert(docId, Convert.ToInt32(Settings.Instance.UserID), newDate, Convert.ToDecimal(NoOfDays.Text), Convert.ToDateTime(calendarTextBox.Text), Convert.ToDateTime(Reason1.Text), Reason.Value, "Pending", 0, string.Empty, Convert.ToInt32(DdlSalesPerson.SelectedValue), 0, strLF, leaveString);

                string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);


                string salesRepqueryNew1 = "";
                if (salesRepqueryNewdt.Rows.Count > 0)
                {
                    salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                }

                string getSeniorSMId = @"select UnderId,UserId,SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(ddlunderUser.SelectedValue) + "";
                DataTable salesRepqryForSManNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId);
                int senSMid = 0; string seniorName = string.Empty;
                if (salesRepqryForSManNewdt.Rows.Count > 0)
                {
                    string getSeniorSMId12 = @"select SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UnderId"]) + "";
                    DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
                    if (salesRepqryForSManNewdt12.Rows.Count > 0)
                    {
                        senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
                        seniorName = salesRepqryForSManNewdt12.Rows[0]["SMName"].ToString();
                    }
                }
                DateTime newDate12 = GetUTCTime();
                DataTable salesRepqueryNewdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew1);

                string msgUrl = "MeetApproval.aspx?SMId=" + ddlunderUser.SelectedValue + "&MeetPlanId=" + RetSave;

                //Check Is Senior

                int smiDDDl = Convert.ToInt32(ddlunderUser.SelectedValue);
                string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);
                int IsSenior = 0, ISSenSmId = 0;
                if (smIDNewDDlQryDT.Rows.Count > 0)
                {
                    ISSenSmId = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
                }
                string meetAdmin = @"select UserId, SMId from MastMeetLogin where Active=1";
                DataTable dtMeetAdmin = DbConnectionDAL.GetDataTable(CommandType.Text, meetAdmin);
                if (dtMeetAdmin.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMeetAdmin.Rows)
                    {
                        if (dr["SMId"] != null) ME.InsertTransNotification("MEETREQUEST", Convert.ToInt32(dr["UserId"]), newDate12, msgUrl, "Meet Plan Request By - " + salesRepqryForSManNewdt.Rows[0]["SMName"] + " " + " " + ",Meet Name -" + ddlmeetType.SelectedItem.Text +" " + txtMeetDate.Text + " ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlunderUser.SelectedValue), Convert.ToInt32(dr["SMId"]));
                        else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Sales Id not found');", true);
                    }
                }


                ME.InsertTransNotification("MEETREQUEST", Convert.ToInt32(salesRepqueryNewdt1.Rows[0]["UserId"]), newDate12, msgUrl, "Meet Plan Request By - " + salesRepqryForSManNewdt.Rows[0]["SMName"] + " " + " " + ",Meet Name -" + ddlmeetType.SelectedItem.Text + " " + txtMeetDate.Text + " ", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(ddlunderUser.SelectedValue), senSMid);
                //if (smiDDDl != ISSenSmId)
                //{
                //    IsSenior = 1;
                //}
                //if (IsSenior == 1)
                //{

                    //string updateLeaveStatusQry = @"update TransMeetPlanEntry set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ",AppRemark='Approved By Senior' where MeetPlanId='" + RetSave + "' ";
                    //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateLeaveStatusQry);


                    //ME.InsertTransNotification("MEETAPPROVED", Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]), newDate12, msgUrl, "Leave Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), senSMid, Convert.ToInt32(ddlunderUser.SelectedValue));

                    //string msgUrl1 = "LeaveRequest.aspx?SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]) + "&LVRQId=" + Convert.ToInt64(retsave) + "&LeaveCase=AC";

                    //int salesRepID = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]);
                    //int UserId = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]);
                    ////

                    //string senSMNameQryL3 = @"select UnderId from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                    //int UnderID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQryL3));

                    //string senQryL3 = @"select SMId from MastSalesRep where SMId=" + UnderID + "";
                    //int SMIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senQryL3));

                    //string senUserQryL3 = @"select UserId from MastSalesRep where SMId=" + SMIDL3 + "";
                    //int UserIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL3));

                    //ME.InsertTransNotificationL3("MEETAPPROVED", UserIDL3, newDate12, msgUrl1, "Meet Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), SMIDL3, Convert.ToInt32(salesRepID));

                    //End
                //}
                //else
                //{
                    
                //}
            }
            catch { }
        }
        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
        private void InsertProduct(int meetPlanID,int srNo,int ItemGrpId,int ItemId,int ClassiD,int SegmentId)
        {
            try {int resaveProduct= ME.InsertMeetProduct(meetPlanID, srNo, ItemGrpId, ItemId, ClassiD, SegmentId); }
            catch(Exception ex) { }
        }

        private void InsertrMeetParty(int meetPlanID, int srNo, int PartyID, string MobileNo ,string Address1)
        {
            try { int resaveParty = ME.InsertMeetParty(meetPlanID, srNo, PartyID, MobileNo, Address1, ""); }
            catch (Exception ex) { }
        }
        public void EmptyControlData(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if ((c.GetType() == typeof(TextBox)))
                {
                    ((TextBox)c).Text = "";
                }
                if ((c.GetType() == typeof(DropDownList)))
                {
                    ((DropDownList)c).SelectedIndex = -1;
                    //((DropDownList)c).Items.Clear();
                }
                if (c.HasControls())
                {
                    EmptyControlData(c);
                }
            }
        }
        private void reset()
        {
            
            this.EmptyControlData(this);
          //  txtComments.Text = "";
          // // hfCustomerId.Value = "0";
          ////  txtdistName.Text="";
          //  txtDistributerSharing.Text = "";
          //  txtastralSharing.Text = "";
          //  ddlindrustry.SelectedValue = "0";
          //  txtApproxBudget.Text = "";
          //  txtgiftqty.Text = "";
          //  ddlmeetType.SelectedValue = "0";
          //  ddlmeetType.SelectedValue = "0";
          //  txtNoOfUsers.Text = "";
          //  txtVenue.Text = "";
          //  txtNoofStaf.Text = "";
           
          //  //Added As Per UAT 07-12-2015
          // // Ddldistributor.SelectedValue = "0"; 
          //  //Ddldistributor.Text = string.Empty;
          //  ddlbeat5.SelectedValue = null;
          //  ddlscheme.SelectedValue = null;
          //  ddlParty.SelectedValue = null;
          //  Ddldistributor.SelectedValue = null;
          //  ddlmeetCity.SelectedValue = null;
          //  txtvalueforenduser.Text = "";
         
          //  ddlscheme.SelectedIndex = 0;
       
          //  txtVenue.Text = "";
          //  Hidparty.Value = "0";
       
          //  txtParty.Text = "";
          //  txtParty.ReadOnly = true;
          //  ddlParty.SelectedIndex = 0;
          //  Ddldistributor.Items.Clear();
          //  ddlbeat5.Items.Clear();
          //  ddlParty.Items.Clear();
          //  ddlTOG.Items.Clear();
          //  ddlTOG.Text = "";
          //  ddlTOG.SelectedValue = "0";
          //  this.BindDDlCity();
            this.fillMeetType();

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
           Response.Redirect("~/MeetPlanEntry.aspx");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchParty(string prefixText, string contextKey)
        {//Ankita - 20/may/2016- (For Optimization)
           // string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=0 and BeatId=" + contextKey + "";
            string str = "select PartyId,PartyName FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=0 and active=1 and BeatId=" + contextKey + "";
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText, string contextKey)
       {
           string str = @"SELECT T1.PartyId,T1.PartyName FROM MastParty AS T1 WHERE (PartyName like '%" + prefixText + "%') and  T1.PartyDist=1 AND T1.Active=1  and T1.CityId in (select AreaId from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp " +
                 "in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and areatype='City' and Active=1) ORDER BY PartyName";
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


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
    

        protected void ddlbeat5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtParty.Text = "";
            //if(ddlbeat5.SelectedIndex>0)
            //{ txtParty.ReadOnly = false; }
            //else { txtParty.ReadOnly = true;
            //Hidparty.Value = "0";
            //}  
            ddlParty.Items.Clear();           
            if (ddlbeat5.SelectedIndex>0)
            { fillPartyByBeat(); }
            else { }
            Hidparty.Value = "0";
        }

        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {
           // this.EmptyControlData(this);
            BindDDlCity();
        }

        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fillInitialRecords();
        }

        //Added As Per UAT 07-12-2015
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchDist(string prefixText, string contextKey)
        {
            //string str = "select * FROM MastParty where (PartyName like '%" + prefixText + "%') and PartyDist=1 and Active=1 and CityId=" + Convert.ToInt32(contextKey) + "";
            //Ankita - 20/may/2016- (For Optimization)
            //string str = "select mp.*,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId in (" + contextKey + ")";
            string str = "select mp.PartyName,mp.SyncId,mp.PartyId,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId in (" + contextKey + ")";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["PartyName"].ToString() + ")" + " " + dt.Rows[i]["SyncId"].ToString() + " " + "(" + dt.Rows[i]["AreaName"].ToString() + ")", dt.Rows[i]["PartyId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

        protected void ddlmeetCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfCityIdStr.Value = ddlmeetCity.SelectedValue;
            ddlbeat5.Items.Clear();
            //Ddldistributor.Items.Clear();
            ddlParty.Items.Clear();
            this.fillBeat();
           // this.fillDist();
        }

        protected void btnMeetprdct_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "none");
            secDiv.Style.Add("display", "block");
         
            //Response.Redirect("~/MeetProducts.aspx?Pagename=Meetplanentry");
        }


        private void fillMeetType1()
        {//Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetType order by Name";
            string query = "select Id,Name from MastMeetType order by Name";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        //private void fillUnderUsers()
        //{
        //    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
        //    if (d.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            DataView dv = new DataView(d);
        //            dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead'";
        //            ddlunderUser.DataSource = dv;
        //            ddlunderUser.DataTextField = "SMName";
        //            ddlunderUser.DataValueField = "SMId";
        //            ddlunderUser.DataBind();
        //        }
        //        catch { }

        //    }
        //}

        public string getSelectedParty()
        {
            string PTypeStr1 = "";
            foreach (ListItem item in ddlParty.Items)
            {

                if (item.Selected)
                {
                    PTypeStr1 += item.Value + ",";
                    // PTypeStr1 += item.Text + ",";
                }
            }

            PTypeStr1 = PTypeStr1.TrimStart(',').TrimEnd(',');
            return PTypeStr1;
        }
        private void fillUnderUsers1()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    //Ankita - 20/may/2016- (For Optimization)
                    //string Role = "Select RoleType from Mastrole mr left join Mastsalesrep ms on mr.RoleId=ms.RoleId where ms.Smid in (" + Settings.Instance.SMID + ") ";
                    //DataTable dtRole = new DataTable();
                    //dtRole = DbConnectionDAL.GetDataTable(CommandType.Text, Role);
                    //string RoleTy = dtRole.Rows[0]["RoleType"].ToString();
                    string RoleTy = Settings.Instance.RoleType;
                    if (RoleTy == "CityHead" || RoleTy == "DistrictHead")
                    {
                        ddlunderUser.SelectedValue = Settings.Instance.SMID;
                    }
                }
                catch { }

            }
        }

        private void FillClass()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from MastItemClass order by Name";
            string str = @"select Id,Name from MastItemClass order by Name";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ddlclass.DataSource = deptValueDt;
                ddlclass.DataTextField = "Name";
                ddlclass.DataValueField = "Id";
                ddlclass.DataBind();
            }
            ddlclass.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
        private void FillSegment()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from MastItemSegment order by Name";
            string str = @"select Id,Name from MastItemSegment order by Name";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ddlsegment.DataSource = deptValueDt;
                ddlsegment.DataTextField = "Name";
                ddlsegment.DataValueField = "Id";
                ddlsegment.DataBind();
            }
            ddlsegment.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void FillGroup()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
            string str = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
            DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (deptValueDt.Rows.Count > 0)
            {
                ddlgroup.DataSource = deptValueDt;
                ddlgroup.DataTextField = "ItemName";
                ddlgroup.DataValueField = "ItemId";
                ddlgroup.DataBind();
            }
            ddlgroup.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        

        private void fillInitialRecords()
        {
            ddlmeet.Items.Clear();
            //Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from TransMeetPlanEntry where SmId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId="+ddlmeetType.SelectedValue+"  and [AppBy] is Null order by MeetDate desc";
            string str = @"select MeetPlanId,MeetName from TransMeetPlanEntry where SmId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId=" + ddlmeetType.SelectedValue + "  and [AppBy] is Null order by MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlmeet.DataSource = dt;
                ddlmeet.DataTextField = "MeetName";
                ddlmeet.DataValueField = "MeetPlanId";
                ddlmeet.DataBind();
            }
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            fillProduct(ddlmeet.SelectedValue);
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //dt.Columns.Add(new DataColumn("ProdctName", typeof(string)));
            //dt.Columns.Add(new DataColumn("ProdctID", typeof(string)));
            dt.Columns.Add(new DataColumn("ProdctGroupId", typeof(string)));
            dt.Columns.Add(new DataColumn("ProdctGroup", typeof(string)));
            dt.Columns.Add(new DataColumn("MatrialClassId", typeof(string)));
            dt.Columns.Add(new DataColumn("MatrialClass", typeof(string)));
            dt.Columns.Add(new DataColumn("SegmentId", typeof(string)));
            dt.Columns.Add(new DataColumn("Segment", typeof(string)));
            ViewState["CurrentTable"] = dt;
            GridView2.DataSource = dt;
            GridView2.DataBind();

        }
        private void AddNewRowToGrid()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > -1)
                {
                    for (int i = 0; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        //drCurrentRow["ProdctName"] = txtItem.Text;
                        //drCurrentRow["ProdctID"] = hfitemid.Value;
                        if (ddlgroup.SelectedIndex > 0)
                        {
                            drCurrentRow["ProdctGroup"] = ddlgroup.SelectedItem.Text;
                        }
                        else
                        {
                            drCurrentRow["ProdctGroup"] = "";
                        }
                        if (ddlclass.SelectedIndex > 0)
                        {
                            drCurrentRow["MatrialClass"] = ddlclass.SelectedItem.Text;
                        }
                        else
                        {
                            drCurrentRow["MatrialClass"] = "";
                        }
                        if (ddlsegment.SelectedIndex > 0)
                        {
                            drCurrentRow["Segment"] = ddlsegment.SelectedItem.Text;
                        }
                        else
                        {
                            drCurrentRow["Segment"] = "";
                        }

                        drCurrentRow["ProdctGroupId"] = ddlgroup.SelectedValue;
                        drCurrentRow["MatrialClassId"] = ddlclass.SelectedValue;
                        drCurrentRow["SegmentId"] = ddlsegment.SelectedValue;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView2.DataSource = dtCurrentTable;
                    GridView2.DataBind();

                    ddlclass.SelectedIndex = 0;
                    ddlgroup.SelectedIndex = 0;
                    ddlsegment.SelectedIndex = 0;
                }
            }
            else
            {
                //  Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                GridView2.DataSource = dt;
                GridView2.DataBind();

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];


                    //DataRow[] dr = dt.Select("ProdctID=" + hfitemid.Value);
                    //if (dr.Length > 0)
                    //{
                    //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "error", "errormessage('This Product is Already Added');", true);
                    //    return;
                    //}
                    //else
                    //{
                    //if (ddlmeet.SelectedIndex > 0)
                    //{
                        if (ddlsegment.SelectedIndex > 0 || ddlgroup.SelectedIndex > 0 || ddlclass.SelectedIndex > 0)
                        {
                            AddNewRowToGrid();
                            //hfitemid.Value = "0";
                            //txtItem.Text = "";
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select Material Group/Segment/Material Class');", true);

                        }
                    //}
                    //else
                    //{
                    //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select the Meet');", true);
                    //}



                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
      
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem1(string prefixText)
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            string str = "select SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                customers.Add(item);
            }
            return customers;
        }

        private void InsertProduct1(int meetPlanID, int srNo, int ItemGrpId, int ItemId, int ClassiD, int SegmentId)
        {
            try { int resaveProduct = ME.InsertMeetProduct(meetPlanID, srNo, ItemGrpId, ItemId, ClassiD, SegmentId); }
            catch (Exception ex) { }
        }

        protected void btnsave1_Click(object sender, EventArgs e)
        {

            this.InsertMeetProduct(0);
        }
        private void InsertMeetProduct(int RetSave )
        {
            string Query="";
            try
            {
                if (GridView2.Rows.Count > 0)
                {
                    if(mode=="Edit")
                    {
                        Query = "delete from TransMeetPlanProduct where MeetPlanId=" + Settings.DMInt32(Request.QueryString["MeetPlanID"]);
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, Query);
                    }

                    for (int i = 0; i < GridView2.Rows.Count; i++)
                    {
                        try
                        {
                            HiddenField hid = (HiddenField)GridView2.Rows[i].FindControl("hidProduct");
                            Label lbl = (Label)GridView2.Rows[i].FindControl("lblPName");

                            HiddenField hidMaterialClass = (HiddenField)GridView2.Rows[i].FindControl("hidMaterialClass");
                            HiddenField hidSegment = (HiddenField)GridView2.Rows[i].FindControl("hidSegment");
                            HiddenField hidProductgroup = (HiddenField)GridView2.Rows[i].FindControl("hidProductgroup");
                            if (mode == "Edit")
                            {
                                InsertProduct1(RetSave, i + 1, Settings.DMInt32(hidProductgroup.Value), Settings.DMInt32("0"), Settings.DMInt32(hidMaterialClass.Value), Settings.DMInt32(hidSegment.Value));
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                            }
                            else {
                                InsertProduct1(RetSave, i + 1, Settings.DMInt32(hidProductgroup.Value), Settings.DMInt32("0"), Settings.DMInt32(hidMaterialClass.Value), Settings.DMInt32(hidSegment.Value));
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            }
                           


                        }
                        catch
                        {

                        }
                    }
                    //ddlmeet.SelectedIndex = 0;
                    //ddlmeetType.SelectedValue = "0";
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Add atleast one product');", true);
                }
            }
            catch
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
            }

        }
        private void fillProduct(string MeetId)
        {
            string str = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + MeetId;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                //ViewState["CurrentTable"] = dt;
               
            }
            else
            {
                SetInitialRow();
            }
        }

        protected void ddlmeet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeet.SelectedIndex > 0)
            {
               // fillProduct(ddlmeet.SelectedValue);
            }
            else
            {

            }
        }
        protected void ddlmeetType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeetType.SelectedIndex > 0)
            {
              //  fillInitialRecords();
            }
            else
            {
                ddlmeet.Items.Clear();
                ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            reset();
            ddlunderUser.SelectedIndex = 0;
            ddlmeetType.SelectedIndex = 0;
            ddlmeet.Items.Clear();
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            GridView2.DataSource = null;
            GridView2.DataBind();
            ViewState["CurrentTable"] = null;
            ddlsegment.SelectedIndex = 0;
            ddlgroup.SelectedIndex = 0;
            ddlclass.SelectedIndex = 0;
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            mainDiv.Style.Add("display", "block");
            secDiv.Style.Add("display", "none");
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        //End

        protected void ddlTOG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTOG.SelectedValue == "1")
            {
                txtgiftqty.Text = "0";
                txtgiftqty.Enabled = false;
            }
            else {
                txtgiftqty.Enabled = true;
                txtgiftqty.Text = "";
            }
        }
    }
}