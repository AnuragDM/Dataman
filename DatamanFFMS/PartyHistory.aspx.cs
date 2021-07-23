using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class PartyHistory : System.Web.UI.Page
    {
          int PartyId = 0;
          int AreaId = 0;
         string VisitID = "";
         string CityID = "";
         String Level = "0"; string pageSalesName = "";
         protected void Page_Load(object sender, EventArgs e)
         {
             if (Request.QueryString["PartyId"] != null)
             {
                 PartyId = Convert.ToInt32(Request.QueryString["PartyId"].ToString());
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
             //Added
             if (Request.QueryString["PageView"] != null)
             {
                 pageSalesName = Request.QueryString["PageView"].ToString();
             }
             //End
             if (!IsPostBack)
             {
                 try
                 {
                     try
                     {
                         lblVisitDate5.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString()).ToString("dd/MMM/yyyy");
                     }
                     catch { }
                     lblAreaName5.Text = Settings.Instance.AreaName;
                     lblBeatName5.Text = Settings.Instance.BeatName;
                 }
                 catch
                 {
                 }
                 GetPartyData(PartyId);
                 fillRepeter();
                 fillRepeterDemo();
                 fillRepeterCollection();
                 fillRepeterFailedVisit();
                 fillRepeterCompetitor();

                 fillRepeter1();
                 fillRepeterDemo1();
                 fillRepeterCollection1();
                 fillRepeterFailedVisit1();
                 fillRepeterCompetitorLocked();
             }

         }
        private void GetPartyData(int PartyId)
         { //Ankita - 18/may/2016- (For Optimization)
            //string str = "select * from MastParty where PartyId =" + Convert.ToInt32(PartyId);
             string str = "select PartyName,Address1,Address2,Mobile,Pin from MastParty where PartyId =" + Convert.ToInt32(PartyId);
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt1.Rows.Count > 0)
            {
                partyName.Text = dt1.Rows[0]["PartyName"].ToString();
                address.Text = dt1.Rows[0]["Address1"].ToString() + "" + dt1.Rows[0]["Address2"].ToString();
                mobile.Text = dt1.Rows[0]["Mobile"].ToString();
                lblzipcode.Text = dt1.Rows[0]["Pin"].ToString();
            }
        }
        private void fillRepeter()
        {

          //  string str = @"select * from Temp_TransOrder where VisId=" + VisitID + " and UserId=" + Settings.Instance.UserID + " and SmId=" + Settings.Instance.DSRSMID + "  and PartyId=" + PartyId;
            string str = @"select * from Temp_TransOrder where  PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rpt.DataSource = depdt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource =null;
                rpt.DataBind();
                rpt.Visible = false;
            }
        }

        private void fillRepeterDemo()
        {

//            string str = @"select MI.VDate, MI.Remarks, I.ItemName as GroupName, MI.ItemId,MI.DemoId,MI.DemoDocId,MC.Name as Classname,MS.Name as SegmentName from Temp_TransDemo MI
//                 left join MastItemClass MC on MI.ProductClassId=MC.Id left join MastItemSegment MS on MI.ProductSegmentId=MS.Id left join MastItem I on I.ItemId=MI.ProductMatGrp
//               where VisId=" + VisitID + " and MI.SMId=" + Settings.Instance.DSRSMID + " and  UserId=" + Settings.Instance.UserID + " and PartyId=" + PartyId;
            string str = @"select MI.VDate, MI.Remarks, I.ItemName as GroupName,MI.ImgUrl, MI.ItemId,MI.DemoId,MI.DemoDocId,MC.Name as Classname,MS.Name as SegmentName from Temp_TransDemo MI
                 left join MastItemClass MC on MI.ProductClassId=MC.Id left join MastItemSegment MS on MI.ProductSegmentId=MS.Id left join MastItem I on I.ItemId=MI.ProductMatGrp
               where  PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rptDemo.DataSource = depdt;
                rptDemo.DataBind();
                
            }
            else
            {
                rptDemo.DataSource = null;
                rptDemo.DataBind();
                rptDemo.Visible = false;
            }
        }

        private void fillRepeterCollection()
        {

           // string str = @"select * from Temp_TransCollection  DC inner join MastParty MP on DC.PartyId=MP.PartyId where VisId=" + VisitID + " and  DC.SMId=" + Settings.Instance.DSRSMID + " and  DC.UserId=" + Settings.Instance.UserID + " and DC.PartyId=" + PartyId;
            string str = @"select * from Temp_TransCollection  DC inner join MastParty MP on DC.PartyId=MP.PartyId where DC.PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                rptCollection.DataSource = depdt;
                rptCollection.DataBind();
            }
            else
            {
                rptCollection.DataSource =null;
                rptCollection.DataBind();
                rptCollection.Visible = false;
            }
        }
        private void fillRepeterFailedVisit()
        {

            //string str = "select * FROM Temp_TransFailedVisit where VisId="+VisitID+" and  UserID=" + Settings.Instance.UserID + " and SMId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            string str = "select Temp_TransFailedVisit.*,MastFailedVisitRemark.FVName as Reason FROM Temp_TransFailedVisit left join MastFailedVisitRemark on Temp_TransFailedVisit.ReasonID=MastFailedVisitRemark.FVId where  Temp_TransFailedVisit.PartyId=" + PartyId;
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                rptFailedVisit.DataSource = dt;
                rptFailedVisit.DataBind();
            }
            else
            {
                rptFailedVisit.DataSource =null;
                rptFailedVisit.DataBind();
                rptFailedVisit.Visible = false;
            }
        }


        /// <summary>
        /// ///////////  Locked Entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void fillRepeter1()
        {

           // string str = @"select * from TransOrder where VisId=" + VisitID + " and UserId=" + Settings.Instance.UserID + " and SmId=" + Settings.Instance.DSRSMID + "  and PartyId=" + PartyId;
            string str = @"select * from TransOrder where  PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                Repeater1.DataSource = depdt;
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.DataSource = null;
                Repeater1.DataBind();
                Repeater1.Visible = false;
            }
        }

        private void fillRepeterDemo1()
        {

//            string str = @"select MI.VDate, MI.Remarks, I.ItemName as GroupName, MI.ItemId,MI.DemoId,MI.DemoDocId,MC.Name as Classname,MS.Name as SegmentName from TransDemo MI
//                 left join MastItemClass MC on MI.ProductClassId=MC.Id left join MastItemSegment MS on MI.ProductSegmentId=MS.Id left join MastItem I on I.ItemId=MI.ProductMatGrp
//               where VisId=" + VisitID + " and MI.SMId=" + Settings.Instance.DSRSMID + " and  UserId=" + Settings.Instance.UserID + " and PartyId=" + PartyId;

            string str = @"select MI.VDate, MI.Remarks, I.ItemName as GroupName,MI.ImgUrl, MI.ItemId,MI.DemoId,MI.DemoDocId,MC.Name as Classname,MS.Name as SegmentName from TransDemo MI
                 left join MastItemClass MC on MI.ProductClassId=MC.Id left join MastItemSegment MS on MI.ProductSegmentId=MS.Id left join MastItem I on I.ItemId=MI.ProductMatGrp
               where  PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                Repeater2.DataSource = depdt;
                Repeater2.DataBind();
            }
            else
            {
                Repeater2.DataSource =null;
                Repeater2.DataBind();
                Repeater2.Visible = false;
            }
        }

        private void fillRepeterCollection1()
        {

          //  string str = @"select * from TransCollection  DC inner join MastParty MP on DC.PartyId=MP.PartyId where VisId=" + VisitID + " and  DC.SMId=" + Settings.Instance.DSRSMID + " and  DC.UserId=" + Settings.Instance.UserID + " and DC.PartyId=" + PartyId;
            string str = @"select * from TransCollection  DC inner join MastParty MP on DC.PartyId=MP.PartyId where  DC.PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (depdt.Rows.Count > 0)
            {
                Repeater3.DataSource = depdt;
                Repeater3.DataBind();
            }
            else
            {
                Repeater3.DataSource =null;
                Repeater3.DataBind();
                Repeater3.Visible = false;
            }
        }
        private void fillRepeterFailedVisit1()
        {

            //string str = "select * FROM TransFailedVisit where VisId=" + VisitID + " and  UserID=" + Settings.Instance.UserID + " and SMId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            string str = "select TransFailedVisit.*,MastFailedVisitRemark.FVName as Reason FROM TransFailedVisit left join MastFailedVisitRemark on TransFailedVisit.ReasonID=MastFailedVisitRemark.FVId where  TransFailedVisit.PartyId=" + PartyId;
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                Repeater4.DataSource = dt;
                Repeater4.DataBind();
            }
            else
            {
                Repeater4.DataSource = null;
                Repeater4.DataBind();
                Repeater4.Visible = false;
            }
        }

        private void fillRepeterCompetitor()
        {

           // string str = "select * FROM Temp_TransCompetitor where VisId=" + VisitID + " and  UserID=" + Settings.Instance.UserID + " and SMId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            //string str = "select * FROM Temp_TransCompetitor where  PartyId=" + PartyId;
            string str = "select Comptid,DocId,VDate,Compname,Item,Rate,Qty,Remarks,ImgURL,Discount,OtherActivity=(Case when OtherActivity=1 then 'Yes' else 'No' end) FROM Temp_TransCompetitor where  PartyId=" + PartyId;
            
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                rptCompetitor.DataSource = dt;
                rptCompetitor.DataBind();
            }
            else
            {
                rptCompetitor.DataSource = null;
                rptCompetitor.DataBind();
                rptCompetitor.Visible = false;
            }
        }

        private void fillRepeterCompetitorLocked()
        {

          //  string str = "select * FROM TransCompetitor where VisId=" + VisitID + " and  UserID=" + Settings.Instance.UserID + " and SMId=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            string str = "select Comptid,DocId,VDate,Compname,Item,Rate,Qty,Remarks,ImgURL,Discount,OtherActivity=(Case when OtherActivity=1 then 'Yes' else 'No' end) FROM TransCompetitor where PartyId=" + PartyId;
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                Repeater5.DataSource = dt;
                Repeater5.DataBind();
            }
            else
            {
                Repeater5.DataSource = null;
                Repeater5.DataBind();
                Repeater5.Visible = false;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (pageSalesName == "Secondary")
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName);
            }
            else
            {
                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }
           
        }

        protected void lnkViewDemoImg_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                var item = (RepeaterItem)btn.NamingContainer;
                HiddenField hdnDemoIdCode = (HiddenField)item.FindControl("HiddenField1");
                HiddenField hdnId = (HiddenField)item.FindControl("linkHiddenField");
                Response.ContentType = ContentType;
                if (hdnId.Value!="")
                {
                    btn.Visible = true;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdnId.Value));
                    Response.WriteFile(hdnId.Value);
                    Response.End();
                }
                else
                {
                    btn.Visible = false;
                }
             
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void lnkViewCompImg_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                var item = (RepeaterItem)btn.NamingContainer;
                HiddenField hdnId = (HiddenField)item.FindControl("linkHdFComp");
                Response.ContentType = ContentType;
                if (hdnId.Value != "")
                {
                    btn.Visible = true;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdnId.Value));
                    Response.WriteFile(hdnId.Value);
                    Response.End();
                }
                else
                {
                    btn.Visible = false ;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void lnkViewDemoLockImg_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                var item = (RepeaterItem)btn.NamingContainer;
                HiddenField hdnDemoIdCode = (HiddenField)item.FindControl("HiddenField1");
                HiddenField hdnId = (HiddenField)item.FindControl("linkLockHdF");
                Response.ContentType = ContentType;
                if (hdnId.Value != "")
                {
                    btn.Visible = true;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdnId.Value));
                    Response.WriteFile(hdnId.Value);
                    Response.End();
                }
                else
                {
                    btn.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        protected void lnkViewDemoCompLockImg_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                var item = (RepeaterItem)btn.NamingContainer;
                HiddenField hdnId = (HiddenField)item.FindControl("linkHdFLockComp");
                Response.ContentType = ContentType;
                if (hdnId.Value != "")
                {
                    btn.Visible = true;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(hdnId.Value));
                    Response.WriteFile(hdnId.Value);
                    Response.End();
                }
                else
                {
                    btn.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
    }
}