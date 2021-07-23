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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AstralFFMS
{
    public partial class DSREntryForm1 : System.Web.UI.Page
    {
        BAL.DSRLevel1BAL dp = new DSRLevel1BAL();
        string VisitID = "";
        string CityID = "";
        string v_date = "";
         int discFlag = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            txtNextVisitdate.Attributes.Add("readonly", "readonly");
            txtNextVisitDateDist.Attributes.Add("readonly", "readonly");
            if (Request.QueryString["VisitID"] != null)
            {
                VisitID = Request.QueryString["VisitID"].ToString();
           //     CalendarExtender3.StartDate = Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID)));
            }
            if (Request.QueryString["CityID"] != null)
            {
                CityID = Request.QueryString["CityID"].ToString();
            }
            if (!IsPostBack)
            {
               //Added
                HypUploadStatus.Style.Add("display", "none");
                Settings.Instance.BindTimeToDDL(DistFrTimeDDL);
                Settings.Instance.BindTimeToDDL(DistFrTimeDDL);
                Settings.Instance.BindTimeToDDL(DistToTimeDDL);
                Settings.Instance.BindTimeToDDL(NVTimeDDL);
                Settings.Instance.BindTimeToDDL(basicExampleDDL);
                //End
                BindFailReason();
                fillGrid();

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
            }
        }

       
        private void fillGrid()
        {
            if (Request.QueryString["CityID"] != null)
            {
               
                //string str = "select * FROM MastParty where Active=1 and PartyDist=1 and CityID=" + Request.QueryString["CityID"].ToString()+" order by  PartyName";

                //Added 08-12-2015
                //string str = "select MastParty.*,MastArea.AreaName as CityName FROM MastParty left join MastArea on MastArea.AreaId=MastParty.CityID where MastParty.Active=1 and PartyDist=1 and MastParty.CityID in (" + Request.QueryString["CityID"].ToString() + ") order by  PartyName";
                //string str = "select MastParty.*,MastArea.AreaName as CityName FROM MastParty left join MastArea on MastArea.AreaId=MastParty.CityID where MastParty.Active=1 and PartyDist=1 and MastParty.CityID in (" + CityID + ") order by  PartyName";

                //End
                string str = "";
                //Added Nishu 25/06/2016 - (Distributor fill salesman permission wise)
                if (Settings.Instance.AreaWiseDistributor=="Y")
                {

                    str = "select MastParty.PartyId,MastParty.SyncId,MastParty.PartyName,MastArea.AreaName as CityName FROM MastParty left join MastArea on MastArea.AreaId=MastParty.CityID where MastParty.Active=1 and PartyDist=1 and MastParty.CityID in (" + CityID + ") order by  PartyName";
                }
                else
                {
                    string SMIDStr = "";
                    string smid = "Select SMId from Transvisit where Visid = " + VisitID + "";
                    DataTable dtsmid = DbConnectionDAL.GetDataTable(CommandType.Text, smid);
                    SMIDStr = dtsmid.Rows[0]["SMId"].ToString();
                     str = "select MastParty.PartyId,MastParty.SyncId,MastParty.PartyName,MastArea.AreaName as CityName FROM MastParty left join MastArea on MastArea.AreaId=MastParty.CityID where MastParty.Active=1 and PartyDist=1 and MastParty.CityID in (" + CityID + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + SMIDStr + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + SMIDStr + "))) order by  PartyName";
                }
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    //gvdetails.DataSource = dt;
                    //gvdetails.DataBind();

                    //Added
                    rpt.DataSource = dt;
                    rpt.DataBind();

                    //End
                }
                else
                {
                    //gvdetails.DataSource = null;
                    //gvdetails.DataBind();
                    rpt.DataSource = dt;
                    rpt.DataBind();

                }

                //if (Settings.GetVisitLocked(Convert.ToInt32(VisitID)))
                //{
                //    gvdetails.Enabled = false;
                //}
                //else
                //{ gvdetails.Enabled = true ; }

            }
        }

        protected void imgbtn_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void lnkdiss_Click(object sender, EventArgs e)
        {
            //CalendarExtender1.StartDate = Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(1);
            this.ModalPopupExtender1.Show();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //LinkButton btndetails = sender as LinkButton;
                //GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
                //HiddenField did = sender as HiddenField;
                //GridViewRow gvrow1 = (GridViewRow)did.NamingContainer;

                //HiddenField did = (HiddenField)gvrow.FindControl("did");
                if (txtdiscussion.Text != "")
                {
                    if (CheckDiscussion(hidDisID.Value) > 0)
                    {
                        UpdateDiscussion();
                    }
                    else
                    {
                        InsertDiscussion();
                      
                    }
                    fillGrid();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
      

        private void BindFailReason()
        {
            try
            {
                string str = "select * from MastFailedVisitRemark where Active=1 order by FVName ";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                ddlReason.DataSource = dt;
                ddlReason.DataTextField = "FVName";
                ddlReason.DataValueField = "FVId";
                ddlReason.DataBind();
                ddlReason.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch
            {
            }
        }

        protected void imgbtn_Click1(object sender, EventArgs e)
        {
            LinkButton btndetails = sender as LinkButton;
   //         GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            RepeaterItem gvrow = (RepeaterItem)btndetails.NamingContainer;

            HiddenField did = (HiddenField)gvrow.FindControl("did");

   //         lblusername.Text = gvrow.Cells[1].Text;
            hidDisID.Value = did.Value;
            FillInitialDiscussion(hidDisID.Value);
            CalendarExtender1.StartDate = Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(1);
            this.ModalPopupExtender1.Show();
            if (Settings.GetVisitLocked(Convert.ToInt32(VisitID)))
            {
                txtdiscussion.Enabled = false;
                //Added
                DistFrTimeDDL.Enabled = false; DistToTimeDDL.Enabled = false; NVTimeDDL.Enabled = false;
                //End
                txtDistFrTime.Disabled = true; txtDistToTime.Disabled = true; txtNextVisitDateDist.Enabled = false;  txtNVTime.Disabled = true;
                btnUpdate.Enabled = false; btnUpdate.CssClass = "btn btn-primary";
            }
            else
            {
                txtdiscussion.Enabled = true;
                //Added
                DistFrTimeDDL.Enabled = true; DistToTimeDDL.Enabled = true; NVTimeDDL.Enabled = true;
                //End
                txtDistFrTime.Disabled = false; txtDistToTime.Disabled = false; txtNextVisitDateDist.Enabled = true; txtNVTime.Disabled = false;
                btnUpdate.Enabled = true; btnUpdate.CssClass = "btn btn-primary";
            }
        }

        protected void imgbtn_Click2(object sender, EventArgs e)
        {
            LinkButton btndetails = sender as LinkButton;
       //     GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            RepeaterItem gvrow = (RepeaterItem)btndetails.NamingContainer;

            HiddenField did = (HiddenField)gvrow.FindControl("did");
            FillInitialFaild(did.Value);
            hidDisID.Value = did.Value;
            CalendarExtender3.StartDate = Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(1);
            this.ModalPopupExtender2.Show();
            if (Settings.GetVisitLocked(Convert.ToInt32(VisitID)))
            {
                ddlReason.Enabled = false;
                TextBox1.Enabled = false;
                //Added
                basicExampleDDL.Enabled = false;
                //End
                basicExample.Disabled = true; txtNextVisitDateDist.Enabled = false;
                Button2.Enabled = false; Button2.CssClass = "btn btn-primary";
            }
            else
            {
                ddlReason.Enabled = true; TextBox1.Enabled = true;
                //Added
                basicExampleDDL.Enabled = true;
                //End
                basicExample.Disabled = false; txtNextVisitDateDist.Enabled = true;
                Button2.Enabled = true; Button2.CssClass = "btn btn-primary";
            }

        }
        private string GetVisitDate(int VisiID)
        {
            string st = "select VDate from TransVisit where VisId=" + VisitID;
            string VisitDate = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            return VisitDate;
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
        private void FillInitialDiscussion(string DistId)
        {
       //     string str = "select remarkDist from Temp_TransVisitDist where VisId=" + VisitID + " and  DistId=" + DistId;
       //     string remark = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            //       txtdiscussion.Text=remark;

            //Added 12-Dec-2015
            try
            {
                string str = "select * from Temp_TransVisitDist where VisId=" + VisitID + " and  DistId=" + DistId;
                DataTable remark = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (remark.Rows.Count > 0)
                {
                    txtdiscussion.Text = remark.Rows[0]["remarkDist"].ToString();
                    txtDistFrTime.Value = remark.Rows[0]["SpentfrTime"].ToString();
                    txtDistToTime.Value = remark.Rows[0]["SpentToTime"].ToString();
                    txtNVTime.Value = remark.Rows[0]["NextVisitTime"].ToString();
                    //Added
                    DistFrTimeDDL.SelectedValue = remark.Rows[0]["SpentfrTime"].ToString();
                    DistToTimeDDL.SelectedValue = remark.Rows[0]["SpentToTime"].ToString();
                    NVTimeDDL.SelectedValue = remark.Rows[0]["NextVisitTime"].ToString();
                    CalendarExtender1.StartDate = Settings.GetUTCTime().AddDays(1);
                    HypUploadStatus.Style.Add("display", "block");
                    HypUploadStatus.Text = remark.Rows[0]["ImgUrl"].ToString().Substring(remark.Rows[0]["ImgUrl"].ToString().LastIndexOf('/') + 1);
                    HypUploadStatus.NavigateUrl = remark.Rows[0]["ImgUrl"].ToString();
                    //End
                    txtNextVisitDateDist.Text = DateTime.Parse(Convert.ToDateTime(remark.Rows[0]["NextVisitDate"]).ToShortDateString()).ToString("dd/MMM/yyyy");
                    discFlag = 1;
                }
                else
                {
                    txtNVTime.Value = "11:00am";
                    txtdiscussion.Text = string.Empty;
                    txtDistFrTime.Value = string.Empty;
                    txtDistToTime.Value = string.Empty;
                    //Added
                    DistFrTimeDDL.SelectedValue = "09:30";
                    DistToTimeDDL.SelectedValue = "18:30";
                    NVTimeDDL.SelectedValue = "11:00";
                    HypUploadStatus.Style.Add("display", "none");
                    //End
                    CalendarExtender1.StartDate = Settings.GetUTCTime().AddDays(1);
                    txtNextVisitDateDist.Text = DateTime.Parse(Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(7.00).ToShortDateString()).ToString("dd/MMM/yyyy");
                    discFlag = 0;
                }
                //End
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
           
        }

        private void FillInitialFaild(string DistId)
        {
            try
            {
                string str = "select * from Temp_TransFailedVisit where VisId=" + VisitID + " and  PartyId=" + DistId;
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    ddlReason.SelectedValue = dt.Rows[0]["ReasonID"].ToString();
                    TextBox1.Text = dt.Rows[0]["Remarks"].ToString();
                    basicExample.Value = dt.Rows[0]["VisitTime"].ToString();
                    //Added
                    basicExampleDDL.SelectedValue = dt.Rows[0]["VisitTime"].ToString();
                    CalendarExtender3.StartDate = Settings.GetUTCTime().AddDays(1);
                    //End
                    txtNextVisitdate.Text = string.Format("{0:dd/MMM/yyyy}",Convert.ToDateTime(dt.Rows[0]["Nextvisit"].ToString()));
                }
                else
                {
                    ddlReason.SelectedIndex = 0;
                    //txtNextVisitdate.Text = "";
                    TextBox1.Text = "";
                    //basicExample.Value = "";
                    basicExample.Value = "11:00am";
                    //Added
                    basicExampleDDL.SelectedValue = "11:00";
                    CalendarExtender3.StartDate = Settings.GetUTCTime().AddDays(1);
                    //End
                    txtNextVisitdate.Text = DateTime.Parse(Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(7.00).ToShortDateString()).ToString("dd/MMM/yyyy");

                }
            }
            catch
            {

            }
        }
        private int CheckDiscussion(string DistId)
        {
            string str = "select count(*) from Temp_TransVisitDist where VisId=" + VisitID+ " and  DistId=" + DistId;
            int exists = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
            return exists;
        }

        private void InsertDiscussion()
        {
            try
            {
                    //int RetSave = dp.InsertDiscussionWithDistributor(Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt64(VisitID), 1, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(GetDistributorCity(Convert.ToInt32(VisitID))), Convert.ToInt32(Settings.Instance.DSRSMID), Convert.ToInt32(hidDisID.Value), 0, txtdiscussion.Text, "", "", txtNextVisitDateDist.Text, txtNVTime.Value, txtDistFrTime.Value, txtDistToTime.Value);
                //Added on 12/01/2015
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

                        //#region Image Compression Code
                        //dsrImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
                        //System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("TempImages" + filename));
                        //int newwidthimg = 220;
                        //float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                        //float Ht = 220;
                        //int newHeight = Convert.ToInt32(Ht / AspectRatio);
                        //Bitmap bitMAP1 = new Bitmap(newwidthimg, newHeight);
                        //Graphics imgGraph = Graphics.FromImage(bitMAP1);
                        ////imgGraph.imgQuality = CompositingQuality.HighQuality;
                        //imgGraph.SmoothingMode = SmoothingMode.HighQuality;
                        //imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //var imgDimesions = new Rectangle(0, 0, newwidthimg, newHeight);
                        //imgGraph.DrawImage(image, 0, 0, newwidthimg, newHeight);
                        //bitMAP1.Save(Server.MapPath("~/DSRImages" + "/DistDisc_" + filename), ImageFormat.Jpeg);
                        //bitMAP1.Dispose();
                        //bitMAP1.Dispose();
                        //image.Dispose();

                        //// start image size validation after image compression > Priyanka 02/03/2016
                        //#region
                        //DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/DSRImages"));
                        //// Get a reference to each file in that directory.
                        //FileInfo[] fiArr = di.GetFiles();
                        //// Display the names and sizes of the files.
                        ////Console.WriteLine("The directory {0} contains the following files:", di.Name);
                        //foreach (FileInfo f in fiArr)
                        //{
                        //    if (f.Name == "DistDisc_" + filename)
                        //    {
                        //        if (f.Length >= 512000)
                        //        {
                        //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "error", "errormessage('Image size should be less than 512KB');", true);
                        //            return;
                        //        }
                        //    }
                        //}
                        //#endregion
                        //// end image size validation after image compression > Priyanka 02/03/2016

                        //if (System.IO.File.Exists(Server.MapPath("TempImages" + filename)))
                        //{
                        //    System.IO.File.Delete(Server.MapPath("TempImages" + filename));
                        //}
                        //#endregion

                        imgurl = "~/DSRImages" + "/DistDisc_" + filename;
                    }

                    string docID = Settings.GetDocID("DDISC", DateTime.Now);
                    Settings.SetDocID("DDISC", docID);
                    //End

                    int RetSave = dp.InsertDiscussionWithDistributor(Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt64(VisitID), 1, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(GetDistributorCity(hidDisID.Value)), Convert.ToInt32(Settings.Instance.DSRSMID), Convert.ToInt32(hidDisID.Value), 0, txtdiscussion.Text, "", "", txtNextVisitDateDist.Text, NVTimeDDL.SelectedValue, DistFrTimeDDL.SelectedValue, DistToTimeDDL.SelectedValue, imgurl, docID);
                    string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'DistributorDiscussion','','DistributorDiscussion','','','" + Convert.ToInt64(VisitID) + "')";
                    DAL.DbConnectionDAL.ExecuteQuery(strinslog);
                    if (RetSave > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Inserted');", true);
                    }
                    fillGrid();
                
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Inserted');", true);
                ex.ToString();
            }
        }
        private void UpdateDiscussion()
        {
            try
            {
                
                    string st = "delete from Temp_TransVisitDist where DistId=" + hidDisID.Value + "and VisId=" + VisitID;

                    //  int a =
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, st);

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
                            { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                            return;
                            }
                            dsrImgFileUpload.SaveAs(Server.MapPath("~/DSRImages" + "/DistDisc_" + filename));


                            //#region Image Compression Code
                            //dsrImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
                            //System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("TempImages" + filename));
                            //int newwidthimg = 220;
                            //float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                            //float Ht = 220;
                            //int newHeight = Convert.ToInt32(Ht / AspectRatio);
                            //Bitmap bitMAP1 = new Bitmap(newwidthimg, newHeight);
                            //Graphics imgGraph = Graphics.FromImage(bitMAP1);
                            ////imgGraph.imgQuality = CompositingQuality.HighQuality;
                            //imgGraph.SmoothingMode = SmoothingMode.HighQuality;
                            //imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            //var imgDimesions = new Rectangle(0, 0, newwidthimg, newHeight);
                            //imgGraph.DrawImage(image, 0, 0, newwidthimg, newHeight);
                            //bitMAP1.Save(Server.MapPath("~/DSRImages" + "/DistDisc_" + filename), ImageFormat.Jpeg);
                            //bitMAP1.Dispose();
                            //bitMAP1.Dispose();
                            //image.Dispose();

                            //// start image size validation after image compression > Priyanka 02/03/2016
                            //#region
                            //DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/DSRImages"));
                            //// Get a reference to each file in that directory.
                            //FileInfo[] fiArr = di.GetFiles();
                            //// Display the names and sizes of the files.
                            ////Console.WriteLine("The directory {0} contains the following files:", di.Name);
                            //foreach (FileInfo f in fiArr)
                            //{
                            //    if (f.Name == "DistDisc_" + filename)
                            //    {
                            //        if (f.Length >= 512000)
                            //        {
                            //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "error", "errormessage('Image size should be less than 512KB');", true);
                            //            return;
                            //        }
                            //    }
                            //}
                            //#endregion
                            //// end image size validation after image compression > Priyanka 02/03/2016

                            //if (System.IO.File.Exists(Server.MapPath("TempImages" + filename)))
                            //{
                            //    System.IO.File.Delete(Server.MapPath("TempImages" + filename));
                            //}
                            //#endregion

                            imgurl = "~/DSRImages" + "/DistDisc_" + filename;
                        }
                        //End

                        string docID = Settings.GetDocID("DDISC", DateTime.Now);
                        Settings.SetDocID("DDISC", docID);
                        //int RetUpdate = dp.InsertDiscussionWithDistributor(Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt64(VisitID), 1, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(GetDistributorCity(Convert.ToInt32(VisitID))), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(hidDisID.Value), 0, txtdiscussion.Text, "", "", txtNextVisitDateDist.Text, NVTimeDDL.SelectedValue, DistFrTimeDDL.SelectedValue, DistToTimeDDL.SelectedValue,imgurl);
                        int RetUpdate = dp.InsertDiscussionWithDistributor(Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt64(VisitID), 1, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(GetDistributorCity(hidDisID.Value)), Convert.ToInt32(Settings.Instance.SMID), Convert.ToInt32(hidDisID.Value), 0, txtdiscussion.Text, "", "", txtNextVisitDateDist.Text, NVTimeDDL.SelectedValue, DistFrTimeDDL.SelectedValue, DistToTimeDDL.SelectedValue, imgurl,docID);
                        string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'DistributorDiscussion','','DistributorDiscussion','','','" + Convert.ToInt64(VisitID) + "')";
                        DAL.DbConnectionDAL.ExecuteQuery(strinslog);
                        if (RetUpdate > 0)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Updated');", true);
                        }
                        fillGrid();
                    //}
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Updated');", true);
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
        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlReason.SelectedIndex > 0)
                {
                    string st = "delete from tEMP_TransFailedVisit where PartyId=" + hidDisID.Value + "and VisId=" + VisitID;
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, st);

                    string docID = Settings.GetDocID("FAILV", Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))));
                    Settings.SetDocID("FAILV", docID);
                    //int RetSave = dp.InsertFaildVisit(Convert.ToInt64(VisitID), docID, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.DSRSMID), Convert.ToInt32(hidDisID.Value), TextBox1.Text, txtNextVisitdate.Text, Convert.ToInt32(ddlReason.SelectedValue),basicExample.Value,Convert.ToString(CityID));
                    //Added Nishu 01/03/2016
                    //int RetSave = dp.InsertFaildVisit(Convert.ToInt64(VisitID), docID, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.DSRSMID), Convert.ToInt32(hidDisID.Value), TextBox1.Text, txtNextVisitdate.Text, Convert.ToInt32(ddlReason.SelectedValue), basicExampleDDL.SelectedValue, Convert.ToString(CityID));
                    int RetSave = dp.InsertFaildVisit(Convert.ToInt64(VisitID), docID, GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), Convert.ToInt32(Settings.Instance.DSRSMID), Convert.ToInt32(hidDisID.Value), TextBox1.Text, txtNextVisitdate.Text, Convert.ToInt32(ddlReason.SelectedValue), basicExampleDDL.SelectedValue,GetDistributorCity(hidDisID.Value));
                    string strinslog = "insert into [LogUserActivity] ( [PageName],[UserId],[UsrActDateTime],[UsrAct],[OldInfo],[NewInfo],[A_E_D],[Title],[DocId]) values ('" + Path.GetFileName(Request.Path) + "'," + Settings.Instance.UserID + ",DateAdd(ss,19800,GetUtcdate()),'DistributorFailedvisit','','DistributorFailedvisit','','','" + Convert.ToInt64(VisitID) + "')";
                    DAL.DbConnectionDAL.ExecuteQuery(strinslog);                    
                    if (RetSave > 0)
                    {
                        string updateandroidid = "update temp_TransFailedVisit set android_id='" + docID + "' where Fvdocid='" + docID + "'";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                        ddlReason.SelectedIndex = 0;
                        txtNextVisitdate.Text = "";
                        TextBox1.Text = "";
                        basicExample.Value = "";
                        basicExampleDDL.SelectedValue = "10:00";
                        //lblerrorFV.InnerHtml = "Record Inserted Successfully";
                        //lblerrorFV.Style.Add("color", "green");
                        //lblerrorFV.Attributes.Remove("hidden");
                        //this.ModalPopupExtender2.Show();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Failed Visit Inserted Successfully');", true);
                      
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select the Reason');", true);
                }
                fillGrid();
            }

            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Response.Redirect("DistributerPartyList.aspx?VisitID=" + VisitID + "&CityID=" + CityID+"&Level=1");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Response.Redirect("DSREntryForm.aspx?VisitID=" + VisitID + "&CityID=" + CityID);
        }

        protected void gvdetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Coll")
            {
                Response.Redirect("DistributorCollection.aspx?DistID=" + e.CommandArgument.ToString()+"&CityID=" + CityID + "&VisitID=" + VisitID+"&Level=1");
            }
          
        }

        protected void gvdetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["CityID"] != null)
            {

                string str = "select MastParty.*,MastArea.AreaName AS CityName  FROM MastParty Left JOIN MastArea ON MastParty.CityId=MastArea.AreaId where MastParty.Active=1 and MastParty.PartyDist=1 and MastParty.CityID in (" + Request.QueryString["CityID"].ToString() + ") and MastParty.PartyName like '%" + txtsearch.Text + "%'  order by  MastParty.PartyName";
                DataTable dt = new DataTable();
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    //gvdetails.DataSource = dt;
                    //gvdetails.DataBind();
                    rpt.DataSource = dt;
                    rpt.DataBind();
                }
                else
                {
                    //gvdetails.DataSource = null;
                    //gvdetails.DataBind();
                    rpt.DataSource = dt;
                    rpt.DataBind();
                }

                if (Settings.GetVisitLocked(Convert.ToInt32(VisitID)))
                {
                //    gvdetails.Enabled = false;
                }
                else
                { 
                    //gvdetails.Enabled = true; 
                }

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Coll")
            {
                Response.Redirect("DistributorCollection.aspx?DistID=" + e.CommandArgument.ToString() + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");
            }
            if (e.CommandName == "DDisc")
            {
                 Response.Redirect("DistributorDiscussion.aspx?DistID=" + e.CommandArgument.ToString() + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");                
            }
            if (e.CommandName == "StockTemplate")
            {
                Response.Redirect("DistributorItemTemplate.aspx?DistID=" + e.CommandArgument.ToString() + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");
            }
            if (e.CommandName == "Stock")
            {
                Response.Redirect("DistributorItemStock.aspx?DistID=" + e.CommandArgument.ToString() + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");
            }
            if (e.CommandName == "Activity")
            {
                Response.Redirect("ActivityMast.aspx?DistID=" + e.CommandArgument.ToString() + "&CityID=" + CityID + "&VisitID=" + VisitID + "&Level=1");
            }
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("did");

                LinkButton lnkDiscWithDist = (LinkButton)e.Item.FindControl("imgRptDisc");
                LinkButton lnkFailV = (LinkButton)e.Item.FindControl("imgRptFail1");
                LinkButton lnkColl = (LinkButton)e.Item.FindControl("lnkRptDist");
                HtmlTableCell tdDiscLinkCell = (HtmlTableCell)e.Item.FindControl("discLinkCell");
                HtmlTableCell tdFailVCell = (HtmlTableCell)e.Item.FindControl("discFailVCell");
                HtmlTableCell tdCollLink = (HtmlTableCell)e.Item.FindControl("collLinkCell");
              

                string str1 = "select * from Temp_TransFailedVisit where VisId=" + VisitID + " and PartyId=" + hid.Value;
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
               
                if (dt1.Rows.Count > 0)
                {
                    tdFailVCell.Attributes.Add("style", "background-color:Honeydew;");
             //       lnkFailV.BackColor = Color.Honeydew;
           //        e.Item.Cells[4].BackColor = Color.Honeydew;
                    lnkDiscWithDist.Enabled = false;
                }
               


                string str = "select * from Temp_TransVisitDist where VisId=" + VisitID + " and DistId=" + hid.Value;
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["remarkDist"].ToString() != "")
                    {
                        tdDiscLinkCell.Attributes.Add("style", "background-color:Honeydew;");
                       // lnkDiscWithDist.BackColor = Color.Honeydew;
                       // e.Item.Cells[3].BackColor = Color.Honeydew;
                    }
                    lnkFailV.Enabled = false;
                //    e.Item.Cells[4].Enabled = false;
                }


                //Added 15.12.2015
                string strDistColl = "select * from DistributerCollection where VisId=" + VisitID + " and DistId=" + hid.Value;
                DataTable dtDistColl = DbConnectionDAL.GetDataTable(CommandType.Text, strDistColl);
                if (dtDistColl.Rows.Count > 0)
                {
                    lnkFailV.Enabled = false;
                 //  e.Row.Cells[4].Enabled = false;
                }

            }
        }

        //protected void imgRptDisc_Click(object sender, EventArgs e)
        //{
        //    LinkButton btndetails = sender as LinkButton;            
        //    RepeaterItem gvrow = (RepeaterItem)btndetails.NamingContainer;
        //    //HiddenField did = (HiddenField)gvrow.FindControl("did");            
        //    //hidDisID.Value = did.Value;
        //    hidDisID.Value = btndetails.CommandArgument.ToString();
        //    FillInitialDiscussion(hidDisID.Value);
        //    CalendarExtender1.StartDate = Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(1);
        //    this.ModalPopupExtender1.Show();
        //    if (Settings.GetVisitLocked(Convert.ToInt32(VisitID)))
        //    {
        //        txtdiscussion.Enabled = false;
        //        //Added
        //        DistFrTimeDDL.Enabled = false; DistToTimeDDL.Enabled = false; NVTimeDDL.Enabled = false;
        //        //End
        //        txtDistFrTime.Disabled = true; txtDistToTime.Disabled = true; txtNextVisitDateDist.Enabled = false; txtNVTime.Disabled = true;
        //        btnUpdate.Enabled = false; btnUpdate.CssClass = "btn btn-primary";
        //    }
        //    else
        //    {
        //        txtdiscussion.Enabled = true;
        //        //Added
        //        DistFrTimeDDL.Enabled = true; DistToTimeDDL.Enabled = true; NVTimeDDL.Enabled = true;
        //        //End
        //        txtDistFrTime.Disabled = false; txtDistToTime.Disabled = false; txtNextVisitDateDist.Enabled = true; txtNVTime.Disabled = false;
        //        btnUpdate.Enabled = true; btnUpdate.CssClass = "btn btn-primary";
        //    }
        //}

        protected void imgRptFail1_Click(object sender, EventArgs e)
        {

            LinkButton btndetails = sender as LinkButton;            
            RepeaterItem gvrow = (RepeaterItem)btndetails.NamingContainer;
            //HiddenField did = (HiddenField)gvrow.FindControl("did");
            //hidDisID.Value = did.Value;
            hidDisID.Value = btndetails.CommandArgument.ToString();
            FillInitialFaild(hidDisID.Value);          
          
            CalendarExtender3.StartDate = Convert.ToDateTime(GetVisitDate(Convert.ToInt32(VisitID))).AddDays(1);
            this.ModalPopupExtender2.Show();
            if (Settings.GetVisitLocked(Convert.ToInt32(VisitID)))
            {
                ddlReason.Enabled = false;
                TextBox1.Enabled = false;
                basicExampleDDL.Enabled = false;
                basicExample.Disabled = true; txtNextVisitDateDist.Enabled = false;
                Button2.Enabled = false; Button2.CssClass = "btn btn-primary";
            }
            else
            {
                ddlReason.Enabled = true; TextBox1.Enabled = true; basicExampleDDL.Enabled = true;
                basicExample.Disabled = false; txtNextVisitDateDist.Enabled = true;
                Button2.Enabled = true; Button2.CssClass = "btn btn-primary";
            }
        }
    }
}