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
    public partial class DistributorDiscussion : System.Web.UI.Page
    {
        BAL.DSRLevel1BAL dp = new BAL.DSRLevel1BAL();
        string parameter = "";
        string VisitID = "0";
        string CityID = "0";
        string Level = "";
        string PartyId = "0";
        string Visid = "";
        string distID = "0";
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
            
            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
            }
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
            }
           
            if (Request.QueryString["Level"] != null)
            {
                Level = Request.QueryString["Level"].ToString();
            }
            if (Request.QueryString["PartyId"] != null)
            {
                PartyId = Request.QueryString["PartyId"].ToString();
            }
            if (Request.QueryString["DistID"] != null)
            {
                distID = Request.QueryString["DistID"].ToString();
            }
           
          
            if (!IsPostBack)
            {
                try
                {
                    Settings.Instance.BindTimeToDDL(DistFrTimeDDL);
                    Settings.Instance.BindTimeToDDL(DistToTimeDDL);
                    Settings.Instance.BindTimeToDDL(NVTimeDDL);
                    btnDelete.Visible = false;
                    DistFrTimeDDL.SelectedValue = "09:30";
                    DistToTimeDDL.SelectedValue = "18:30";
                    NVTimeDDL.SelectedValue = "11:00";                   
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
       
        private void fillRepeter()
        {

            //string str = "select * FROM Temp_TransFailedVisit inner join MastFailedVisitRemark on MastFailedVisitRemark.FVId=Temp_TransFailedVisit.ReasonID  where  VisId=" + VisitID + " and UserID=" + Settings.Instance.UserID + " and PartyId=" + PartyId;
            //DataTable dt = new DataTable();
            //dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //rpt.DataSource = dt;
            //rpt.DataBind();

            string str = @"select * from Temp_TransVisitDist tvd left join MastParty MP on tvd.DistId=MP.PartyId where VisId=" + VisitID + " and Distid = " + distID + " and Active=1";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = dt;
            rpt.DataBind();
        }
        private void Insert()
        {
            try
            {
                string docID = Settings.GetDocID("DDISC", DateTime.Now);
                Settings.SetDocID("DDISC", docID);
                    if (Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))) < Convert.ToDateTime(txnextVisitDate.Text))
                    {
                       string imgurl = "";  
               
                    if (dsrImgFileUpload.HasFile)
                    {
                        string directoryPath = Server.MapPath(string.Format("~/{0}/", "DSRImages"));
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        string filename = Path.GetFileName(dsrImgFileUpload.FileName);
                        bool k = ValidateImageSize();
                        if (k != true)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                            return;
                        }
                        dsrImgFileUpload.SaveAs(Server.MapPath("~/DSRImages" + "/DistDisc_" + filename));
                        imgurl = "~/DSRImages" + "/DistDisc_" + filename;
                    }
                         int RetSave = dp.InsertDiscussionWithDistributor(Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt64(VisitID), 1, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(GetDistributorCity(distID)), Convert.ToInt32(Settings.Instance.DSRSMID), Convert.ToInt32(distID), 0, txtRemark.Text, "", "", txnextVisitDate.Text, NVTimeDDL.SelectedValue, DistFrTimeDDL.SelectedValue, DistToTimeDDL.SelectedValue, imgurl,docID);
                         string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'DistributorDiscussion','','DistributorDiscussion','','','" + Convert.ToInt64(VisitID) + "')";
                         DAL.DbConnectionDAL.ExecuteQuery(strinslog);
                         string updateandroidid = "update temp_transvisitdist set android_id='" + docID + "' where DISCDOCID='" + docID + "'";
                         DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                        if (RetSave > 0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                            ClearControls();
                            divdocid.Visible = false;
                            if (Request.QueryString["DistID"] != null)
                            {
                                HtmlMeta meta = new HtmlMeta();
                                meta.HttpEquiv = "Refresh";
                                //Added 24-09-2016 Abhishek
                                if (Level == "1")
                                {
                                    meta.Content = "3;url=DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1";
                                    //       Response.Redirect("DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");
                                }
                                else if (Level == "2")
                                {
                                    meta.Content = "3;url=DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=2";
                                    //       Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=2");
                                }
                                else
                                {
                                    meta.Content = "3;url=DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=3";
                                    //       Response.Redirect("DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=3");
                                }
                                //End
                      //          meta.Content = "3;url=DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID;
                                this.Page.Controls.Add(meta);
                            }
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

            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }
       
        protected bool ValidateImageSize()
        {
            int fileSize = dsrImgFileUpload.PostedFile.ContentLength;
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
        private string GetVisitDate(int VisiID)
        {
            string st = "select VDate from TransVisit where VisId=" + VisitID;
            string VisitDate = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return VisitDate;
        }
        private string GetDistributorCity(string DistId)
        {
            string st = "select CityId from mastparty where PartyId=" + DistId;
            string CityId = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return CityId;
        }
        private void UpdateRecord()
        {
            try
            {

                //string st = "delete from Temp_TransVisitDist where DistId=" + distID + "and VisId=" + VisitID;

                //  int a =
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, st);

                string imgurl = "";              
                if (dsrImgFileUpload.HasFile)
                {
                    string directoryPath = Server.MapPath(string.Format("~/{0}/", "DSRImages"));
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    string filename = Path.GetFileName(dsrImgFileUpload.FileName);
                    bool k = ValidateImageSize();
                    if (k != true)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                        return;
                    }
                    dsrImgFileUpload.SaveAs(Server.MapPath("~/DSRImages" + "/DistDisc_" + filename));                 

                    imgurl = "~/DSRImages" + "/DistDisc_" + filename;
                }
                else
                {
                    imgurl = imgpreview.Src;
                }
                int RetUpdate = dp.UpdateDiscussionWithDistributor(Convert.ToInt64(ViewState["CollId"]), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt64(VisitID), 1, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(GetDistributorCity(distID)), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(distID), 0, txtRemark.Text, "", "", txnextVisitDate.Text, NVTimeDDL.SelectedValue, DistFrTimeDDL.SelectedValue, DistToTimeDDL.SelectedValue, imgurl);              
                if (RetUpdate > 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);                    
                    ClearControls();
                    btnDelete.Visible = false;
                    divdocid.Visible = false;
                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    //Added 24-09-2016 Abhishek
                    if (Level == "1")
                    {
                        meta.Content = "3;url=DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1";
                 //       Response.Redirect("DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");
                    }
                    else if (Level == "2")
                    {
                        meta.Content = "3;url=DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=2";
                 //       Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=2");
                    }
                    else
                    {
                        meta.Content = "3;url=DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=3";
                 //       Response.Redirect("DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=3");
                    }
                    //End
                //    meta.Content = "3;url=DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID;
                    this.Page.Controls.Add(meta);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Updated');", true);
                }              
                
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Updated');", true);
                ex.ToString();
            }
            //try
            //{
            //    if (Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))) < Convert.ToDateTime(txnextVisitDate.Text))
            //    {

            //        int RetSave = dp.UpdateFaildVisit(Convert.ToInt32(ViewState["CollId"]), Convert.ToInt64(VisitID), Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(PartyId), txtRemark.Text, txnextVisitDate.Text, Convert.ToInt32(ddlreason.SelectedValue), NVTimeDDL.SelectedValue, Convert.ToString(Settings.DMInt32(Settings.Instance.AreaID)));
            //        string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'PartyFailedvisit','','PartyFailedvisit','','','" + Convert.ToInt64(VisitID) + "')";
            //        DAL.DbConnectionDAL.ExecuteQuery(strinslog);
            //        if (RetSave > 0)
            //        {
            //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
            //            btnSave.Text = "Save";
            //            ClearControls();
            //            divdocid.Visible = false;

            //            if (Request.QueryString["DistID"] != null)
            //            {
            //                HtmlMeta meta = new HtmlMeta();
            //                meta.HttpEquiv = "Refresh";
            //                meta.Content = "3;url=DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID;
            //                this.Page.Controls.Add(meta);
            //            }
            //        }
            //        else
            //        {
            //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Update');", true);
            //        }
            //    }
            //    else
            //    {
            //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Next Visit date Cannot less than visit Date');", true);

            //    }
            //}

            //catch (Exception ex)
            //{
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
            //    ex.ToString();
            //}
        }
        private void FillDeptControls(int depId)
        {
            try
            {
                string str = "select * from Temp_TransVisitDist where VisDistId=" + depId;
                DataTable remark = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (remark.Rows.Count > 0)
                {
                    txtRemark.Text = remark.Rows[0]["remarkDist"].ToString();
                    txtDistFrTime.Value = remark.Rows[0]["SpentfrTime"].ToString();
                    txtDistToTime.Value = remark.Rows[0]["SpentToTime"].ToString();
                    txtNVTime.Value = remark.Rows[0]["NextVisitTime"].ToString();                   
                    DistFrTimeDDL.SelectedValue = remark.Rows[0]["SpentfrTime"].ToString();
                    DistToTimeDDL.SelectedValue = remark.Rows[0]["SpentToTime"].ToString();
                    NVTimeDDL.SelectedValue = remark.Rows[0]["NextVisitTime"].ToString();                
                    txnextVisitDate.Text = DateTime.Parse(Convert.ToDateTime(remark.Rows[0]["NextVisitDate"]).ToShortDateString()).ToString("dd/MMM/yyyy");
                    if (remark.Rows[0]["ImgUrl"] != string.Empty)
                    {
                        imgpreview.Src = remark.Rows[0]["ImgUrl"].ToString();
                        imgpreview.Style.Add("display", "block");
                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                   
                }
                else
                {
                    txtNVTime.Value = "11:00am";
                    txtRemark.Text = string.Empty;
                    txtDistFrTime.Value = string.Empty;
                    txtDistToTime.Value = string.Empty;                    
                    DistFrTimeDDL.SelectedValue = "09:30";
                    DistToTimeDDL.SelectedValue = "18:30";
                    NVTimeDDL.SelectedValue = "11:00";

                    txnextVisitDate.Text = DateTime.Parse(Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(7.00).ToShortDateString()).ToString("dd/MMM/yyyy");
                   
                }
                //End
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
            btnDelete.Visible = false;
            imgpreview.Src = "";
            imgpreview.Style.Add("display", "none");
            btnSave.Text = "Save";

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            //Response.Redirect("~/DistributorDiscussion.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {

                int retdel = dp.deleteDiscussionWithDistributor (Convert.ToString(ViewState["CollId"]));
                //int retdel = dp.delete(Request.QueryString["CollId"]);
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    btnDelete.Visible = false;
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
            if (Level == "1")
            {
                Response.Redirect("DSREntryForm1.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");
            }
            else if (Level == "2")
            {
                Response.Redirect("DistributorDashboardLevel2.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=2");
            }
            else
            {
                Response.Redirect("DistributorDashboardLevel3.aspx?CityID=" + CityID + "&VisitID=" + VisitID + "&DistID=" + PartyId + "&Level=3");
            }

        }

    }
}

       