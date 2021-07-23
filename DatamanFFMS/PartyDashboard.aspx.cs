using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Data;
using DAL;

namespace AstralFFMS
{
    public partial class PartyDashboard : System.Web.UI.Page
    {
        int PartyId = 0;
        string VisitID = "0";
        string CityID = "0";
        int AreaId = 0;
        string v_date = "";
        string Level = "0"; string pageSaleName = "";
        int DistID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowItemtemplate();
            if (Request.QueryString["PartyId"] != null)
            {
                PartyId = Convert.ToInt32(Request.QueryString["PartyId"]);
                HiddenField1.Value = PartyId.ToString();
                GetPartyData(PartyId);
                string vistId = Settings.Instance.VistID;
                Settings.Instance.AreaID = Request.QueryString["AreaId"];
                //  ShowCompetitor();
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
            if (Request.QueryString["PageView"] != null)
            {
                pageSaleName = Request.QueryString["PageView"].ToString();
            }


            if (!IsPostBack)
            {
                try
                {
                    try
                    {
                        lblVisitDate5.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString()).ToString("dd/MMM/yyyy");
                    }
                    catch { }
                    // string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString());
                    lblAreaName5.Text = Settings.Instance.AreaName;
                    lblBeatName5.Text = Settings.Instance.BeatName;

                }
                catch
                {

                }

                LockedValues(Settings.GetVisitLocked(Convert.ToInt32(VisitID)));
                #region Show Planned Beat
                v_date = (Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).ToString("dd/MMM/yyyy"));




                string strPB = @"SELECT T2.AreaName FROM TransBeatPlan AS T1 LEFT JOIN MastArea AS T2
                                        ON T1.BeatId=T2.AreaId WHERE T1.PlannedDate='" + v_date + "' and T1.SMId=" + Convert.ToInt32(Settings.Instance.DSRSMID);
                DataTable dtPB = DbConnectionDAL.GetDataTable(CommandType.Text, strPB);

                if (dtPB != null && dtPB.Rows.Count > 0)
                {
                    lblPlanedbeat.Text = Convert.ToString(dtPB.Rows[0]["AreaName"]);
                }
                #endregion
                // Settings.Instance.OrderID = null;
            }
        }

        private void LockedValues(bool Lock)
        {
            if (Lock)
            {
                bookOrder.Enabled = false;
                demoEntry.Enabled = false;
                collection.Enabled = false;
                failedVisit.Enabled = false;
                btnCompetitor.Enabled = false;
                editParty.Enabled = false;
            }
            else
            {
                bookOrder.Enabled = true;
                demoEntry.Enabled = true;
                collection.Enabled = true;
                failedVisit.Enabled = true;
                btnCompetitor.Enabled = true;
                editParty.Enabled = true;
                CheckFaildVisit();
                CheckOrder();
            }
        }
        private string GetVisitDate(int VisiID)
        {
            string st = "select VDate from TransVisit where VisId=" + VisitID;
            string VisitDate = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return VisitDate;
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            if (Level == "1" && pageSaleName == "Secondary")
            {
                Response.Redirect("~/DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level + "&PageV=Secondary");
            }
            if (Level == "2" && pageSaleName == "Secondary")
            {
                Response.Redirect("~/DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level + "&PageL2=Secondary");
            }
            if (Level == "3" && pageSaleName == "Secondary")
            {
                Response.Redirect("~/DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level + "&PageL3=Secondary");
            }
            else
            {
                Response.Redirect("~/DistributerPartyList.aspx?CityID=" + CityID + " &VisitID=" + VisitID + "&Level=" + Level);
            }


        }


        public void CheckFaildVisit()
        {
            string str = "select count(*) from Temp_TransFailedVisit where VisId=" + Convert.ToInt64(VisitID) + " and PartyId=" + Convert.ToInt32(PartyId);

            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (exists > 0)
            {
                bookOrder.Enabled = false;
                collection.Enabled = false;
                demoEntry.Enabled = false;
                //Added 15-12-2015
                btnCompetitor.Enabled = false;
                //End
            }
            else
            {
                bookOrder.Enabled = true;
                collection.Enabled = true;
                demoEntry.Enabled = true;
                //Added 15-12-2015
                btnCompetitor.Enabled = true;
                //End
            }

        }

        public void CheckOrder()
        {
            string str = "select count(*) from Temp_TransOrder where VisId=" + Convert.ToInt64(VisitID) + " and PartyId=" + Convert.ToInt32(PartyId);

            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            if (exists > 0)
            {
                failedVisit.Enabled = false;

            }
            else
            {
                string str1 = "select count(*) from Temp_TransDemo where VisId=" + Convert.ToInt64(VisitID) + " and PartyId=" + Convert.ToInt32(PartyId);

                int existsDemo = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));

                if (existsDemo > 0)
                {
                    failedVisit.Enabled = false;

                }
                else
                {
                    string str2 = "select count(*) from Temp_TransCollection where VisId=" + Convert.ToInt64(VisitID) + " and PartyId=" + Convert.ToInt32(PartyId);
                    int existsCollection = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                    if (existsCollection > 0)
                    {
                        failedVisit.Enabled = false;

                    }
                    else
                    {
                        failedVisit.Enabled = true;
                    }
                }
            }

        }
        public bool EntryLockB()
        {

            //Ankita - 18/may/2016- (For Optimization)
            //string str = "select * from TransVisit where VisId =" + Convert.ToInt64(VisitID);
            string str = "select Lock from TransVisit where VisId =" + Convert.ToInt64(VisitID);
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["Lock"].ToString() == "False")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public bool ShowCompetitor()
        {
            bool addP = false;
            //if (Settings.Instance.IsPartyDist != "0")
            //{
            //    addP = true;
            //    return addP;
            //}
            return addP;
        }


        private void GetPartyData(int PartyId)
        {  //Ankita - 18/may/2016- (For Optimization)
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

        public void ShowItemtemplate()
        {
            if (Settings.Instance.OrderEntryType != "1")
            {
                btnadditem.Visible = true;
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                if (Settings.Instance.OrderEntryType == "1")
                { Response.Redirect("~/OrderEntry.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName); }
                if (Settings.Instance.OrderEntryType == "2")
                { Response.Redirect("~/OrderEntryItemWise.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName); }
                else
                { }
            }
            else
            {
                if (Settings.Instance.OrderEntryType == "1")
                { Response.Redirect("~/OrderEntry.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level); }
                if (Settings.Instance.OrderEntryType == "2")
                { Response.Redirect("~/OrderEntryItemWise.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level); }
                else
                { }
            }
        }
        protected void SaleReturn_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                //if (Settings.Instance.OrderEntryType == "1")
                //{ Response.Redirect("~/OrderEntry.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName); }
                //if (Settings.Instance.OrderEntryType == "2")
                //{
                Response.Redirect("~/OrderReturnEntryItemWise.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
                //else
                //{ }
            }
            else
            {
                //if (Settings.Instance.OrderEntryType == "1")
                //{ Response.Redirect("~/OrderEntry.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level); }
                //if (Settings.Instance.OrderEntryType == "2")
                Response.Redirect("~/OrderReturnEntryItemWise.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);

            }
        }
        protected void btnDemo_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                Response.Redirect("~/DemoEntry.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
            }
            else
            {
                Response.Redirect("~/DemoEntry.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }


        }

        protected void failedVisit_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                Response.Redirect("~/FaildVisit.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
            }
            else
            {
                Response.Redirect("~/FaildVisit.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

        }

        protected void collection_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                Response.Redirect("~/PartyCollection.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
            }
            else
            {
                Response.Redirect("~/PartyCollection.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

        }

        protected void partyHist_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                Response.Redirect("~/PartyHistory.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
            }
            else
            {
                Response.Redirect("~/PartyHistory.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

        }

        protected void editParty_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                Response.Redirect("~/EditParty.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
            }
            else
            {
                Response.Redirect("~/EditParty.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

        }

        protected void btnCompetitor_Click(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                Response.Redirect("~/Competitor.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
            }
            else
            {
                Response.Redirect("~/Competitor.aspx?PartyId=" + HiddenField1.Value + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

        }

        protected void editParty_Click1(object sender, EventArgs e)
        {
            if (pageSaleName == "Secondary")
            {
                Response.Redirect("~/EditPartyMaster.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
            }
            else
            {
                Response.Redirect("~/EditPartyMaster.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
            }

        }

        protected void btnadditem_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/OrderTemplates.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSaleName);
        }


        protected void btnActivityTrans_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ActivityMast.aspx?PartyID=" + PartyId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level);
        }

        //protected void BtnCancel_Click1(object sender, EventArgs e)
        //{
        //    bool addP = false;
        //    if (Settings.Instance.IsPartyDist != "0")
        //    {
        //        Response.Redirect("~/DSREntryForm1.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("~/DSREntryForm1.aspx");
        //    }
        //}
    }
}