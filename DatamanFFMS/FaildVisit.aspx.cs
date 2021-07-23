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
using System.IO;

namespace AstralFFMS
{
    public partial class FaildVisit : System.Web.UI.Page
    {
        BAL.DSRLevel1BAL dp = new BAL.DSRLevel1BAL();
         int PartyId = 0;
         int AreaId = 0;
        string parameter = "";
        string VisitID = "";
        string CityID = "";
        string Level = "0"; string pageSalesName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            txnextVisitDate.Attributes.Add("readonly", "readonly");
            if (parameter != "")
            {
                ViewState["CollId"] = parameter;
                FillDeptControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
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

                    lblVisitDate5.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString()).ToString("dd/MMM/yyyy");
                    lblAreaName5.Text = Settings.Instance.AreaName;
                    lblBeatName5.Text = Settings.Instance.BeatName;

                    //Added as Per UAT -12-Dec-2015
                    basicExample.Value = "11:00am";
                    txnextVisitDate.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).AddDays(7.00).ToShortDateString()).ToString("dd/MMM/yyyy");
                    //End
                }
                catch
                {

                }
                divdocid.Visible = false;
                // btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
                BindFailReason();
                GetPartyData(PartyId);
                if (Request.QueryString["CollId"] != null)
                {
                    FillDeptControls(Convert.ToInt32(Request.QueryString["CollId"]));
                }
            }
            try
            {
                calendarTextBox_CalendarExtender.StartDate = Settings.GetUTCTime(); //DateTime.Now;
            }
            catch { }

        }
        private void BindFailReason()
        {
            try
            {//Ankita - 20/may/2016- (For Optimization)
               // string str = "select * from MastFailedVisitRemark where Active=1 order by FVName";
                string str = "select FVId,FVName from MastFailedVisitRemark where Active=1 order by FVName";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                ddlreason.DataSource = dt;
                ddlreason.DataTextField = "FVName";
                ddlreason.DataValueField = "FVId";
                ddlreason.DataBind();
                ddlreason.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch
            {
            }
        }
        private void GetPartyData(int PartyId)
        {//Ankita - 20/may/2016- (For Optimization)
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

            string str = "select * FROM Temp_TransFailedVisit inner join MastFailedVisitRemark on MastFailedVisitRemark.FVId=Temp_TransFailedVisit.ReasonID  where  VisId=" + VisitID + " and UserID=" + Settings.Instance.UserID + " and PartyId=" + PartyId;
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        private void Insert()
        {
            try
            {
                if (check() == 0)
                {
                    if (Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))) < Convert.ToDateTime(txnextVisitDate.Text))
                    {
                        string docID = Settings.GetDocID("FAILV", Settings.GetUTCTime());//DateTime.Now
                        Settings.SetDocID("FAILV", docID);
                        int RetSave = dp.InsertFaildVisit(Convert.ToInt64(VisitID), docID, Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.DSRSMID), Convert.ToInt32(PartyId), txtRemark.Text, txnextVisitDate.Text, Convert.ToInt32(ddlreason.SelectedValue), basicExample.Value, Convert.ToString(Settings.DMInt32(Settings.Instance.AreaID)));
                        string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'PartyFailedvisit','','PartyFailedvisit','','','" + Convert.ToInt64(VisitID) + "')";
                        DAL.DbConnectionDAL.ExecuteQuery(strinslog);
                        if (RetSave > 0)
                        {
                            string updateandroidid = "update temp_TransFailedVisit set android_id='" + docID + "' where fvdocid='" + docID + "'";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            ClearControls();                           
                            divdocid.Visible = false;
                            HtmlMeta meta = new HtmlMeta();
                            meta.HttpEquiv = "Refresh";
                            if (pageSalesName == "Secondary")
                            {
                                Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName);
                            }
                            else
                            {
                                meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                            }
                            this.Page.Controls.Add(meta);
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                        }
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date Cannot less than visit Date');", true);

                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                }
            }

            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }
        private int check()
        {
            string s = "select count(*) from Temp_TransFailedVisit where VisId=" + VisitID + "and PartyId=" + PartyId + "and SMId=" + Settings.Instance.DSRSMID;
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, s));
            return exists;
        }
        private void UpdateRecord()
        {
            try
            {
                if (Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))) < Convert.ToDateTime(txnextVisitDate.Text))
                {

                    int RetSave = dp.UpdateFaildVisit(Convert.ToInt32(ViewState["CollId"]), Convert.ToInt64(VisitID), Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(PartyId), txtRemark.Text, txnextVisitDate.Text, Convert.ToInt32(ddlreason.SelectedValue), basicExample.Value, Convert.ToString(Settings.DMInt32(Settings.Instance.AreaID)));
                    string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'PartyFailedvisit','','PartyFailedvisit','','','" + Convert.ToInt64(VisitID) + "')";
                    DAL.DbConnectionDAL.ExecuteQuery(strinslog);
                    if (RetSave > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);                       
                        btnSave.Text = "Save";
                        ClearControls();
                        divdocid.Visible = false;

                        HtmlMeta meta = new HtmlMeta();
                        meta.HttpEquiv = "Refresh";
                        if (pageSalesName == "Secondary")
                        {
                            Response.Redirect("~/PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level + "&PageView=" + pageSalesName);
                        }
                        else
                        {
                            meta.Content = "2;url=PartyDashboard.aspx?PartyId=" + PartyId + "&AreaId=" + AreaId + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=" + Level;
                        }
                        this.Page.Controls.Add(meta);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Update');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date Cannot less than visit Date');", true);

                }
            }

            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }
        private void FillDeptControls(int depId)
        {
            try
            {//Ankita - 20/may/2016- (For Optimization)
                string str = @"select Remarks,VisitTime,Nextvisit,ReasonID,FVDocId from Temp_TransFailedVisit where   FVId=" + depId;
                //string str = @"select * from Temp_TransFailedVisit where   FVId=" + depId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {

                    txtRemark.Text = deptValueDt.Rows[0]["Remarks"].ToString();
                    basicExample.Value = deptValueDt.Rows[0]["VisitTime"].ToString();
                    try
                    {
                        txnextVisitDate.Text = DateTime.Parse(deptValueDt.Rows[0]["Nextvisit"].ToString()).ToString("dd/MMM/yyyy");

                    }
                    catch { }
                    btnSave.Text = "Update";
                    ddlreason.SelectedValue = deptValueDt.Rows[0]["ReasonID"].ToString();
                    // btnDelete.Visible = true;
                    lbldocno.Text = deptValueDt.Rows[0]["FVDocId"].ToString();
                    divdocid.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateRecord();
                }
                else
                {
                    Insert();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private void ClearControls()
        {
            txtRemark.Text = "";
            txtRemark.Text = "";
            ddlreason.SelectedIndex = 0;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            // Response.Redirect("~/FaildVisit.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Request.QueryString["CollId"]);
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    // btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ClearControls();

                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();


            //  btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void Back_Click(object sender, EventArgs e)
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

    }
}