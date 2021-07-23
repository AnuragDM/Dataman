using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.IO;
namespace AstralFFMS
{
    public partial class MeetCancellation : System.Web.UI.Page
    {
        BAL.Meet.MeetPlanEntryBAL ME = new BAL.Meet.MeetPlanEntryBAL();
        int cnt = 0;
        int isExist = 0;
        int statuscnt = 0;
        string sr = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            sr = "select Count(*) from [MastMeetLogin] where userId in(" + Convert.ToInt32(Settings.Instance.UserID) + ")  And IsAdmin2Type=1";
            cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));
            hfCnt.Value =Convert.ToString(cnt);
            sr = "select Count(*) from [MastMeetLogin] where userId in(" + Convert.ToInt32(Settings.Instance.UserID) + ")";
            isExist = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));
            txtVisitDate.Text = DateTime.Today.ToString("dd/MMM/yyyy");
            if (Request.QueryString["MeetPlanID"] != null)
            {
                sr = "select Count(*) from [TransMeetPlanEntry] where MeetPlanId=" + Settings.DMInt32(Request.QueryString["MeetPlanID"]) + " And AppStatus !='Pending' ";
                statuscnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));

            }
            if (txtmeetId.Text != null)
            {
                sr = "select Count(*) from [TransMeetPlanEntry] where MeetPlanId=" + Settings.DMInt32(txtmeetId.Text) + " And AppStatus !='Pending' ";
                statuscnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));
            }
            if (!IsPostBack)
            {
                   // this.checkMeetAdmin();
                fillGrid();
                FillClass();
                FillGroup();
                FillSegment();
                btnUpdate.Visible = false;
            }
        }

        private void fillInitialRecords()
        {//Ankita - 20/may/2016- (For Optimization)
            //string s = "select * from [TransMeetPlanEntry] where [MeetPlanId]="+Settings.DMInt32(txtmeetId.Text);
            string s = "select LambBudget,AppStatus,AppRemark from [TransMeetPlanEntry] where [MeetPlanId]=" + Settings.DMInt32(txtmeetId.Text);
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if(dt.Rows.Count>0)
            {
                txtPlannedBudget.Text = dt.Rows[0]["LambBudget"].ToString();
                txtApprovedBudget.Text = dt.Rows[0]["LambBudget"].ToString();
                ddlApp.SelectedValue=dt.Rows[0]["AppStatus"].ToString();
                txtRemarks.Text=dt.Rows[0]["AppRemark"].ToString();
            }
        }

        private void fillGrid()
         {
             string str1 = "";
             string str = "";
             string MeetPlanID = Request.QueryString["MeetPlanID"];
               if(isExist >0)
               {
                   str1 = "select MastSalesRep.SmId from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId where MastRole.RoleType in ('AreaIncharge','CityHead','DistrictHead','RegionHead','StateHead')"; 
               }
                else {
                     str1 = @"select MastSalesRep.SmId from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + ")) and  level> (select distinct level from MastSalesRepGrp where MainGrp in (" + Settings.Instance.SMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
                }
            
             DataTable d = new DataTable();
             d = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
        
            string sm = "";
            for (int i = 0; i < d.Rows.Count; i++)
            {
                sm +=d.Rows[i]["SmId"].ToString()+",";
            }
          string  sm1 = sm.TrimStart(',').TrimEnd(',');
          if (MeetPlanID !=null)
          {
              str = @"select *,A.AreaName as CityName,A.[SyncId] as CityCode,MST.SMName,mp.PartyName + '-' + [city].areaName + '-' +[state].areaName  as Distributor,ms.Name as Scheme,MPP.PartyName,mb.AreaName as BeatName,mmtg.Name as typeOfGiftEnduser,dbo.getproduct(m.MeetPlanId) AS meetproduct from TransMeetPlanEntry m 
                            left join MastArea A on m.MeetLoaction=A.AreaId Left Join MastSalesRep MST on MST.SMId = m.SMId Left Join MastParty mp on m.DistId=mp.PartyId  Left Join MastScheme ms on m.SchId=ms.Id left join MastParty MPP on m.RetailerPartyID=MP.PartyId left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id left join mastarea as [city] on [city].areaid=mp.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid where  MeetPlanId=" + MeetPlanID + "  order by m.MeetDate desc";
          }
          else
          {
              str = @"select *,A.AreaName as CityName,A.[SyncId] as CityCode,MST.SMName,mp.PartyName + '-' + [city].areaName + '-' +[state].areaName  as Distributor,ms.Name as Scheme,MPP.PartyName,mb.AreaName as BeatName,mmtg.Name as typeOfGiftEnduser,dbo.getproduct(m.MeetPlanId) AS meetproduct from TransMeetPlanEntry m 
                            left join MastArea A on m.MeetLoaction=A.AreaId Left Join MastSalesRep MST on MST.SMId = m.SMId Left Join MastParty mp on m.DistId=mp.PartyId  Left Join MastScheme ms on m.SchId=ms.Id left join MastParty MPP on m.RetailerPartyID=MP.PartyId left join MastArea mb on mb.AreaId=M.[AreaId] left join MastMeetTypeOfGift mmtg on m.TypeOfGiftId=mmtg.Id left join mastarea as [city] on [city].areaid=mp.cityid and [city].areatype='CITY' left join mastarea as [district] on [city].underid=[district].areaid left join mastarea as [state] on [district].underid=[state].areaid where MeetPlanId not in (select MeetPlanId from [dbo].[TransMeetExpense]) And MeetPlanId not in(Select MeetId from TransAddMeetUser) And  m.SMId in (" + sm1 + ") And AppStatus not in ('Cancel','Reject')  order by m.MeetDate desc";
          }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                //gvdetails.DataSource = dt;
                //gvdetails.DataBind();
                rpt.DataSource=  dt;
                rpt.DataBind();
            }
            else
            {
                //gvdetails.DataSource = null;
                //gvdetails.DataBind();

                rpt.DataSource = null;
                rpt.DataBind();
                
            }
        }

        protected void lnkDist_Click(object sender, EventArgs e)
        {
            LinkButton btndetails = sender as LinkButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            HiddenField did = (HiddenField)gvrow.FindControl("did");
            if (did.Value != null)
            {
                txtmeetId.Text = did.Value;
                fillProduct(did.Value);
                fillInitialRecords();
            }
            this.ModalPopupExtender1.Show();

            //Added
            //string meetPlanQry = "select * from [TransMeetPlanEntry] where [MeetPlanId]="+Settings.DMInt32(txtmeetId.Text);
            //DataTable dtMeetPlan = DbConnectionDAL.GetDataTable(CommandType.Text, meetPlanQry);
            //if(dtMeetPlan.Rows.Count>0)
            //{
            //    if(dtMeetPlan.Rows[0]["AppStatus"].ToString() == "Approved")
            //    {
            //        txtApprovedBudget.Enabled = false;
            //        ddlApp.Enabled = false;
            //        txtRemarks.ReadOnly = true;
            //        ddlclass.Enabled = false;
            //        ddlsegment.Enabled = false;
            //        ddlgroup.Enabled = false;
            //        btnAdd.Enabled = false;
            //        btnAdd.CssClass = "btn btn-primary";
            //        GridView2.Enabled = false;
            //        btnUpdate.Enabled = false;
            //        btnUpdate.CssClass = "btn btn-primary";
            //    }
            //    else
            //    {
            //        txtApprovedBudget.Enabled = true;
            //        ddlApp.Enabled = true;
            //        txtRemarks.ReadOnly = false;
            //        ddlclass.Enabled = true;
            //        ddlsegment.Enabled = true;
            //        ddlgroup.Enabled = true;
            //        btnAdd.Enabled = true;
            //        btnAdd.CssClass = "btn btn-primary";
            //        GridView2.Enabled = true;
            //        btnUpdate.Enabled = true;
            //        btnUpdate.CssClass = "btn btn-primary";
            //    }
            //}
            //End
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
           
            //string confirmValue = Request.Form["confirm_value"];
            //if (confirmValue == "Yes")
            //{
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
                try
                {
                    bool a = true;
                    //if (ddlApp.SelectedIndex == 0)
                    //{
                    //    if (txtApprovedBudget.Text == "")
                    //    {
                    //        lblerror.Text = "Please enter the Approved Budget";
                    //        a = false;
                    //        this.ModalPopupExtender1.Show();
                    //        //  return;
                    //    }
                    //    else if (txtApprovedBudget.Text == Convert.ToString(0))
                    //    {
                    //        lblerror.Text = "Approved budget cannot be 0";
                    //        a = false;
                    //        this.ModalPopupExtender1.Show();
                    //        //  return;
                    //    }
                    //    else if (txtApprovedBudget.Text == Convert.ToString(0.00))
                    //    {
                    //        lblerror.Text = "Approved budget cannot be 0";
                    //        a = false;
                    //        this.ModalPopupExtender1.Show();
                    //        //  return;
                    //    }
                    //    else if (Convert.ToDecimal(txtApprovedBudget.Text) == 0)
                    //    {
                    //        lblerror.Text = "Approved budget cannot be 0";
                    //        a = false;
                    //        this.ModalPopupExtender1.Show();
                    //        //  return;
                    //    }

                    //    else if (txtRemarks.Text == "")
                    //    {
                    //        lblerror.Text = "Please enter the Remark";
                    //        a = false;
                    //        this.ModalPopupExtender1.Show();
                    //        //  return;
                    //    }
                       
                    //}
                    //else
                    //{
                        if (txtRemarks.Text == "")
                        {
                            lblerror.Text = "Please enter the Remark";
                            a = false;
                            this.ModalPopupExtender1.Show();
                            //   return;
                        }
                    //}
                        if (a)
                        {
                            if (Settings.Instance.SMID != "0")
                            {
                                string s = " Update TransMeetPlanEntry set AppStatus='" + ddlApp.SelectedValue + "' ,AppRemark='" + txtRemarks.Text + "',AppBy=" + Settings.Instance.SMID + ", Appdate='" + Settings.dateformat1(DateTime.Now.ToShortDateString()) + "' , AppAmount=" + Settings.DMDecimal(txtApprovedBudget.Text) + "  where MeetPlanId=" + Settings.DMInt32(txtmeetId.Text);
                                DbConnectionDAL.GetDataTable(CommandType.Text, s);
                                txtRemarks.Text = "";
                                if (GridView2.Rows.Count > 0)
                                {
                                    string s1 = "delete from TransMeetPlanProduct where MeetPlanId=" + Settings.DMInt32(txtmeetId.Text);
                                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s1);
                                }

                                for (int i = 0; i < GridView2.Rows.Count; i++)
                                {
                                    HiddenField hidProductId = (HiddenField)GridView2.Rows[i].FindControl("hidProduct");
                                    HiddenField hidMaterialClass = (HiddenField)GridView2.Rows[i].FindControl("hidMaterialClass");
                                    HiddenField hidSegment = (HiddenField)GridView2.Rows[i].FindControl("hidSegment");
                                    HiddenField hidProductgroup = (HiddenField)GridView2.Rows[i].FindControl("hidProductgroup");

                                    InsertProduct(Settings.DMInt32(txtmeetId.Text), i + 1, Settings.DMInt32(hidProductgroup.Value), Settings.DMInt32("0"), Settings.DMInt32(hidMaterialClass.Value), Settings.DMInt32(hidSegment.Value));


                                }
                                this.InsertNotification();
                                fillGrid();
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Meet Cancel Successfully');", true);
                                btnUpdate.Enabled = false;

                            }
                            else
                            {
                                Response.Redirect("~/LogIn.aspx", true);
                            }
                        }
                        else
                        {
                            this.ModalPopupExtender1.Show();
                        }
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
                }
                catch (Exception ex) { this.ModalPopupExtender1.Show(); }
            //}
        }
        private void InsertProduct(int meetPlanID, int srNo, int ItemGrpId, int ItemId, int ClassiD, int SegmentId)
        {
            try { int resaveProduct = ME.InsertMeetProduct(meetPlanID, srNo, ItemGrpId, ItemId, ClassiD, SegmentId); }
            catch (Exception ex) { }
        }
        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
        //private void InsertNotification()
        //{
        //    try
        //    {
        //        string salesRepqueryNew = @"select UnderId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
        //        DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);

        //        string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
        //        DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);

        //        string getSMId = @"select SMId from TransMeetPlanEntry r where r.MeetPlanId=" + Convert.ToInt32(txtmeetId.Text) + " ";
        //        DataTable USMID = DbConnectionDAL.GetDataTable(CommandType.Text, getSMId);

        //        string salesRepqueryNew1 = "";
        //        if (salesRepqueryNewdt.Rows.Count > 0)
        //        {
        //            salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
        //        }

        //        string getSeniorSMId = @"select UnderId,UserId,SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(USMID.Rows[0]["SMId"]) + "";
        //        DataTable salesRepqryForSManNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId);
        //        int senSMid = 0; string seniorName = string.Empty;
        //        if (salesRepqryForSManNewdt.Rows.Count > 0)
        //        {
        //            string getSeniorSMId12 = @"select SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UnderId"]) + "";
        //            DataTable salesRepqryForSManNewdt12 = DbConnectionDAL.GetDataTable(CommandType.Text, getSeniorSMId12);
        //            if (salesRepqryForSManNewdt12.Rows.Count > 0)
        //            {
        //                senSMid = Convert.ToInt32(salesRepqryForSManNewdt12.Rows[0]["SMId"]);
        //                seniorName = salesRepqryForSManNewdt12.Rows[0]["SMName"].ToString();
        //            }
        //        }
        //        DateTime newDate12 = GetUTCTime();
        //        DataTable salesRepqueryNewdt1 = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew1);

        //        string msgUrl = "MeetApproval.aspx?SMId=" + 254 + "&MeetPlanId=" + txtmeetId.Text;

        //        //Check Is Senior

        //        int smiDDDl = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
        //        //string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
        //        //DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);
        //        int IsSenior = 0, ISSenSmId = 0;
        //        if (smIDNewDDlQryDT.Rows.Count > 0)
        //        {
        //            ISSenSmId = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
        //        }
        //        //string updateLeaveStatusQry = @"update TransMeetPlanEntry set AppStatus='Approve',AppBy=" + Settings.Instance.UserID + ",AppBySMId=" + Settings.Instance.SMID + ",AppRemark='Approved By Senior' where MeetPlanId='" + txtmeetId.Text + "' ";
        //        //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateLeaveStatusQry);

        //        if (ddlApp.Text == "Approved") ME.InsertTransNotification("MEETAPPROVED", Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]), newDate12, msgUrl, "Meet Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), senSMid, Convert.ToInt32(USMID.Rows[0]["SMId"]));
        //        else ME.InsertTransNotification("MEETREJECTED", Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]), newDate12, msgUrl, "Meet  Rejected By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), senSMid, Convert.ToInt32(USMID.Rows[0]["SMId"]));

        //        string msgUrl1 = "MeetApproval.aspx?SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]) + "& MeetPlanId=" + Convert.ToInt64(txtmeetId.Text);

        //        int salesRepID = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]);
        //        int UserId = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]);
        //        //

        //        string senSMNameQryL3 = @"select UnderId from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
        //        int UnderID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQryL3));

        //        string senQryL3 = @"select SMId from MastSalesRep where SMId=" + UnderID + "";
        //        int SMIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senQryL3));

        //        string senUserQryL3 = @"select UserId from MastSalesRep where SMId=" + SMIDL3 + "";
        //        int UserIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL3));
        //        if (ddlApp.Text == "Approved") ME.InsertTransNotificationL3("MEETAPPROVED", UserIDL3, newDate12, msgUrl1, "Meet Approved By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), SMIDL3, Convert.ToInt32(salesRepID));
        //        else ME.InsertTransNotificationL3("MEETREJECTED", UserIDL3, newDate12, msgUrl1, "Meet Rejected By - " + seniorName, 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), SMIDL3, Convert.ToInt32(salesRepID));
        //    }
        //    catch { }
        //}

        private void InsertNotification()
        {
            try
            {
                string salesRepqueryNew = @"select UnderId,SMName from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable salesRepqueryNewdt = DbConnectionDAL.GetDataTable(CommandType.Text, salesRepqueryNew);

                string smIDNewDDlQry = @"select SMId from MastSalesRep r where r.UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " ";
                DataTable smIDNewDDlQryDT = DbConnectionDAL.GetDataTable(CommandType.Text, smIDNewDDlQry);

                string getSMId = @"select SMId from TransMeetPlanEntry r where r.MeetPlanId=" + Convert.ToInt32(txtmeetId.Text) + " ";
                DataTable USMID = DbConnectionDAL.GetDataTable(CommandType.Text, getSMId);

                string salesRepqueryNew1 = "";
                if (salesRepqueryNewdt.Rows.Count > 0)
                {
                    salesRepqueryNew1 = @"select UserId from MastSalesRep r where r.SMId=" + Convert.ToInt32(salesRepqueryNewdt.Rows[0]["UnderId"]) + " ";
                }

                string getSeniorSMId = @"select UnderId,UserId,SMId,SMName from MastSalesRep where SMId=" + Convert.ToInt32(USMID.Rows[0]["SMId"]) + "";
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

                string msgUrl = "MeetCancellation.aspx?SMId=" + senSMid + "&MeetPlanId=" + txtmeetId.Text;
                string meetstr = "select MeetName from TransMeetPlanEntry where MeetPlanId="+txtmeetId.Text+"";
                string MeetName = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, meetstr));
                string strcancel = "select CONVERT(VarCHAR(20),AppDate, 101) as AppDate,AppRemark from TransMeetPlanEntry where MeetPlanId=" + txtmeetId.Text + "";
                DataTable dtCancel = DbConnectionDAL.GetDataTable(CommandType.Text, strcancel);
               
                //Check Is Senior

                int smiDDDl = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
               
                int IsSenior = 0, ISSenSmId = 0;
                if (smIDNewDDlQryDT.Rows.Count > 0)
                {
                    ISSenSmId = Convert.ToInt32(smIDNewDDlQryDT.Rows[0]["SMId"]);
                }


                ME.InsertTransNotification("MEETCANCEL", Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]), newDate12, msgUrl, "Meet Cancel By - " + salesRepqueryNewdt.Rows[0]["SMName"] + " " + " " + "MeetName-" + MeetName + " " + " " + " " + "Cancel Date-" + dtCancel.Rows[0]["AppDate"] + " " + " " + " " + "Cancel Remark-" + dtCancel.Rows[0]["AppRemark"] + " " + " " + " " + "Status- Cancel" , 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(USMID.Rows[0]["SMId"]));

                string msgUrl1 = "MeetCancellation.aspx?SMId=" + Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]) + "&MeetPlanId=" + Convert.ToInt64(txtmeetId.Text);

                int salesRepID = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["SMId"]);
                int UserId = Convert.ToInt32(salesRepqryForSManNewdt.Rows[0]["UserId"]);
                //

                string senSMNameQryL3 = @"select UnderId from MastSalesRep where SMId=" + Convert.ToInt32(Settings.Instance.SMID) + "";
                int UnderID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senSMNameQryL3));

                string senQryL3 = @"select SMId from MastSalesRep where SMId=" + UnderID + "";
                int SMIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senQryL3));

                string senUserQryL3 = @"select UserId from MastSalesRep where SMId=" + senSMid + "";
                int UserIDL3 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, senUserQryL3));

                ME.InsertTransNotificationL3("MEETCANCEL", UserIDL3, newDate12, msgUrl1, "Meet Cancel By - " + salesRepqueryNewdt.Rows[0]["SMName"] + " " + " " + "MeetName-" + MeetName + " " + " " + " " + "Cancel Date-" + dtCancel.Rows[0]["AppDate"] + " " + " " + " " + "Cancel Remark-" + dtCancel.Rows[0]["AppRemark"] + " " + " " + " " + "Status- Cancel", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), senSMid, Convert.ToInt32(salesRepID));

                //Code for insert MastMeetLogin for all meetAdmin and Admin2Type
                 string slqMeetAdmin = @"select UserId,SMID from MastMeetLogin";
                 DataTable MeetAdmin = DbConnectionDAL.GetDataTable(CommandType.Text, slqMeetAdmin);
                 foreach (DataRow dr in MeetAdmin.Rows)
                 {
                     if (dr["UserId"] != Settings.Instance.UserID) ME.InsertTransNotificationL3("MEETCANCEL", Convert.ToInt32(dr["UserId"]), newDate12, msgUrl1, "Meet Cancel By - " + salesRepqueryNewdt.Rows[0]["SMName"] + " " + " " + "MeetName-" + MeetName + " " + " " + " " + "Cancel Date-" + dtCancel.Rows[0]["AppDate"] + " " + " " + " " + "Cancel Remark-" + dtCancel.Rows[0]["AppRemark"] + " " + " " + " " + "Status- Cancel", 0, Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(dr["SMId"]), Convert.ToInt32(salesRepID));
                    
                 }
            }
            catch { }
        }

        private void fillProduct(string MeetId)
        {
            string str = @"select g.ItemName as ProdctGroup,I.ItemName as ProdctName,c.Name as MatrialClass,s.Name as Segment,P.ClassID as MatrialClassId,p.ItemGrpId as ProdctGroupId,p.ItemId as ProdctID,p.SegmentID  from TransMeetPlanProduct p
             left join MastItemClass c on p.ClassID=c.Id  left join MastItemSegment s on p.SegmentID=s.Id  left join MastItem g on p.ItemGrpId=g.ItemId
             left join MastItem I on p.ItemId=I.ItemId  where p.MeetPlanId=" + MeetId;
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dt;
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
            else
            {
                SetInitialRow();
            }
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
                        //drCurrentRow["ProdctName"] =txtItem.Text;
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

                        //drCurrentRow["ProdctGroup"] = ddlgroup.SelectedItem.Text;
                        //drCurrentRow["MatrialClass"] = ddlclass.SelectedItem.Text;
                        //drCurrentRow["Segment"] = ddlsegment.SelectedItem.Text;
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
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            
            dt.Rows[index].Delete();
            dt.AcceptChanges();
            ViewState["CurrentTable"] = dt;
            GridView2.DataSource = dt;
            GridView2.DataBind();
            this.ModalPopupExtender1.Show();
        }
        private void FillClass()
        {//Ankita - 20/may/2016- (For Optimization)
            //string str = @"select * from MastItemClass Order by Name";
            string str = @"select Id,Name from MastItemClass Order by Name";
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
            //string str = @"select * from MastItemSegment Order by Name";
            string str = @"select Id,Name from MastItemSegment Order by Name";
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
            //string str = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 Order by ItemName";
            string str = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 Order by ItemName";
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction1", "myFunction1();", true);
            try
            {
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
                    //  AddNewRowToGrid();
                    //hfitemid.Value = "0";
                    //txtItem.Text = "";
                    //}

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
               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            this.ModalPopupExtender1.Show();
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem1(string prefixText)
        {
            //Ankita - 20/may/2016- (For Optimization)
            //string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            string str = "select SyncId,ItemName,ItemCode,ItemId FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
            DataTable dt = new DataTable();

            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            List<string> customers = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")", dt.Rows[i]["ItemId"].ToString());
                customers.Add(item);
                //customers.Add("(" + dt.Rows[i]["SyncId"].ToString() + ")" + " " + dt.Rows[i]["ItemName"].ToString() + " " + "(" + dt.Rows[i]["ItemCode"].ToString() + ")");
            }
            return customers;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtRemarks.Text = "";
            fillInitialRecords();
            this.fillGrid();
           
        }

        protected void lnkDistRpt_Click(object sender, EventArgs e)
        {
            LinkButton btndetails = sender as LinkButton;
            RepeaterItem gvrow = (RepeaterItem)btndetails.NamingContainer;
            HiddenField did = (HiddenField)gvrow.FindControl("did");           
            if (did.Value != null)
            {
                 txtmeetId.Text = did.Value;
                //fillProduct(did.Value);
                //fillInitialRecords();
                this.checkMeetAdmin();
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showspinner", "showspinner();", true);
            this.ModalPopupExtender1.Show();
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "hidespinner", "hidespinner();", true);
        }
        private void checkMeetAdmin()
        {
            try
            {
                string pageName12 = Path.GetFileName(Request.Path);
                string PermAll = Settings.Instance.CheckPagePermissions(pageName12, Convert.ToString(Session["user_name"]));
                string[] SplitPerm = PermAll.Split(',');

                if (btnUpdate.Text == "Save")
                {
                    //  btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName12, Convert.ToString(Session["user_name"]));
                    btnUpdate.Enabled = Convert.ToBoolean(SplitPerm[1]);
                    btnUpdate.CssClass = "btn btn-primary";
                }
                else
                {
                    //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName12, Convert.ToString(Session["user_name"]));
                    btnUpdate.Enabled = Convert.ToBoolean(SplitPerm[2]);
                    btnUpdate.CssClass = "btn btn-primary";
                }

                string sr = "select UnderId from mastsalesrep where SMID In(Select SMId from TransMeetPlanEntry where [MeetPlanId]=" + Settings.DMInt32(txtmeetId.Text) + ")";
                int pUserId = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));

                sr = "select Count(*) from [MastMeetLogin] where userId in(" + Convert.ToInt32(Settings.Instance.UserID) + ") And IsAdmin2Type=1";
                int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));

                //if (pUserId == Convert.ToInt32(Settings.Instance.SMID)) btnUpdate.Visible = true;
                //if (cnt > 0)
                //{
                    // Changes Nishu 15/03/2017 
                    //sr = "select Count(*) from [TransMeetPlanEntry] where MeetPlanId=" + Settings.DMInt32(txtmeetId.Text) + " And AppStatus='Pending' ";
                    sr = "select Count(*) from [TransMeetPlanEntry] where MeetPlanId=" + Settings.DMInt32(txtmeetId.Text) + " And AppStatus in ('Pending','Approved','Reject') ";
                    int statuscnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, sr));
                    if (statuscnt > 0){
                        btnUpdate.Visible = true;
                        btnUpdate.Enabled = true;
                        btnUpdate.CssClass = "btn btn-primary";
                    } 
                    else {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Meet expense already Cancel');", true);
                        btnUpdate.Enabled = false;
                        btnUpdate.CssClass = "btn btn-primary";
                    }
                //}
               // else { btnUpdate.Visible = false; }
            }
            catch { }
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            foreach (RepeaterItem rptItem in rpt.Items)
            {
                //if (cnt == 0) ((LinkButton)rptItem.FindControl("lnkDistRpt")).Enabled = false;
                //if (statuscnt > 0) ((LinkButton)rptItem.FindControl("lnkDistRpt")).Enabled = false;
             
            }
        }
    }
}