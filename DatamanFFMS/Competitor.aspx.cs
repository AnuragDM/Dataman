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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace AstralFFMS
{
    public partial class Competitor : System.Web.UI.Page
    {
        BAL.Competitor.CompetitorBAL dp = new BAL.Competitor.CompetitorBAL();
         int PartyId = 0;
         int AreaId = 0;
        string parameter = "";
        string VisitID = "";
        string CityID = "";
        String Level = "0"; string pageSalesName = "";
        string discount = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            parameter = Request["__EVENTARGUMENT"];

            if (parameter != "")
            {
                string active = Request.Form["chkotheractvity"];  
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
                    try
                    {                       
                         
                        lblVisitDate5.Text = DateTime.Parse(Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString()).ToString("dd/MMM/yyyy");
                    }
                    catch { }
                    //lblVisitDate5.Text = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Settings.GetVisitDate(Convert.ToInt32(VisitID))).ToShortDateString());
                    lblAreaName5.Text = Settings.Instance.AreaName;
                    lblBeatName5.Text = Settings.Instance.BeatName;
                }
                catch
                {
                }
                GetPartyData(PartyId);
                divdocid.Visible = false;
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "block");
            }
        }

        private void FillDeptControls(int OrdId)
        {
            try
            {
                string str = @"select * from Temp_TransCompetitor  where ComptId=" + OrdId;
                DataTable deptValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (deptValueDt.Rows.Count > 0)
                {
                    txtitem.Text = deptValueDt.Rows[0]["Item"].ToString();
                    txtQuantity.Text = deptValueDt.Rows[0]["Qty"].ToString();
                    txtRate.Text = deptValueDt.Rows[0]["Rate"].ToString();
                    txtDiscount.Text = deptValueDt.Rows[0]["Discount"].ToString();
                    btnsave.Text = "Update";
                    btnDelete.Visible = true;
                    divdocid.Visible = true;
                    lbldocno.Text = deptValueDt.Rows[0]["DocId"].ToString();
                    compTextBox.Text = deptValueDt.Rows[0]["CompName"].ToString();
                    //Added
                    compTextBox.Text = deptValueDt.Rows[0]["CompName"].ToString();
                    Remark.Text = deptValueDt.Rows[0]["Remarks"].ToString();
                    if (deptValueDt.Rows[0]["ImgUrl"] != string.Empty)
                    {
                        imgpreview.Src = deptValueDt.Rows[0]["ImgUrl"].ToString();
                        imgpreview.Style.Add("display", "block");
                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }
                    txtbrand.Text = deptValueDt.Rows[0]["BrandActivity"].ToString();
                    txtmeet.Text = deptValueDt.Rows[0]["MeetActivity"].ToString();
                    txtroadshow.Text = deptValueDt.Rows[0]["RoadShow"].ToString();
                    txtscheme.Text = deptValueDt.Rows[0]["Scheme/offers"].ToString();
                    txtother.Text = deptValueDt.Rows[0]["OtherGeneralInfo"].ToString();
                    string checkvalue = deptValueDt.Rows[0]["OtherActivity"].ToString();
                    if (checkvalue == "True")
                    {
                        chkotheractvity.Checked = true;
                        dvbrand.Style.Add("display", "block");
                        dvmeet.Style.Add("display", "block");
                        dvroadshow.Style.Add("display", "block");
                        dvscheme.Style.Add("display", "block");
                        dvotherinfo.Style.Add("display", "block");
                    }
                    else
                    {
                        chkotheractvity.Checked = false;
                        dvbrand.Style.Add("display", "none");
                        dvmeet.Style.Add("display", "none");
                        dvroadshow.Style.Add("display", "none");
                        dvscheme.Style.Add("display", "none");
                        dvotherinfo.Style.Add("display", "none");
                    }
                   
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
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

        private void clearcontrols()
        {            
            txtmeet.Text = "";
            txtroadshow.Text = "";
            txtbrand.Text = "";
            txtscheme.Text = "";
            txtother.Text = "";
            btnsave.Text = "Save";
            btnDelete.Visible = false;
            divdocid.Visible = false;
            txtRate.Text = "0.00";
            txtitem.Text = "";
            txtQuantity.Text = "0.00";
            compTextBox.Text = "";
            Remark.Text = "";
            txtDiscount.Text = "0.00";
            
            
            imgpreview.Style.Add("display", "none");
        }
        private void fillRepeter()
        {

            string str = @"select Comptid,DocId,VDate,Compname,Item,Rate,Qty,Discount,OtherActivity=(Case when OtherActivity=1 then 'Yes' else 'No' end) from Temp_TransCompetitor where  VisId=" + VisitID + " and UserId=" + Settings.Instance.UserID + " and SMID=" + Settings.Instance.DSRSMID + " and PartyId=" + PartyId;
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();
        }
        private void InsertOrder()
        {
            try
            {               
                 string active = "0";
                 if (chkotheractvity.Checked)
                     active = "1";
                 if (txtDiscount.Text == "")
                 {
                     discount = txtDiscount.Text = "0";
                 }
                 else
                 {
                     discount = txtDiscount.Text;
                 }
                string docID = Settings.GetDocID("COMPT", DateTime.Now);
                Settings.SetDocID("COMPT", docID);
               
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
                        dsrImgFileUpload.SaveAs(Server.MapPath("~/DSRImages" + "/C_" + filename));

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
                        //bitMAP1.Save(Server.MapPath("~/DSRImages" + "/C_" + filename), ImageFormat.Jpeg);
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
                        //    if (f.Name == "C_" + filename)
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

                        imgurl = "~/DSRImages" + "/C_" + filename;
                    }
                    //End

                    int RetSave = dp.InsertComptitorEntry(Convert.ToInt64(VisitID), docID, Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), PartyId, txtitem.Text, Convert.ToDecimal(txtQuantity.Text), Convert.ToDecimal(txtRate.Text), Convert.ToInt32(Settings.Instance.DSRSMID), imgurl, Remark.Text, compTextBox.Text, Convert.ToDecimal(txtDiscount.Text), txtbrand.Text, txtmeet.Text, txtroadshow.Text, txtscheme.Text, txtother.Text, active);
                    if (RetSave > 0)
                    {
                        string updateandroidid = "update temp_transcompetitor set android_id='" + docID + "' where docid='" + docID + "'";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updateandroidid);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                        this.clearcontrols();
                        btnDelete.Visible = false;
                        divdocid.Visible = false;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                    }               
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Insert');", true);
                ex.ToString();
            }
        }

        private void UpdateOrder()
        {
            try
            {
                string imgurl = "", active = "0";
               
                if (txtDiscount.Text == "")
                { discount = txtDiscount.Text = "0"; }
                else
                { discount = txtDiscount.Text; }
                    
                if (chkotheractvity.Checked)
                    active = "1";
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
                        dsrImgFileUpload.SaveAs(Server.MapPath("~/DSRImages" + "/C_" + filename));
                        //        imgurl = Path.Combine("~/ProductImages" + "/C_", filename);
                        //   imgurl = filename;

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
                        //bitMAP1.Save(Server.MapPath("~/DSRImages" + "/C_" + filename), ImageFormat.Jpeg);
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
                        //    if (f.Name == "C_" + filename)
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

                        imgurl = "~/DSRImages" + "/C_" + filename;
                    }
                    else
                    {
                        imgurl = imgpreview.Src;
                    }

                    int RetSave = dp.UpdateComptitorEntry(Convert.ToInt64(ViewState["CollId"]), Convert.ToInt64(VisitID), Settings.GetVisitDate(Convert.ToInt32(VisitID)), Convert.ToInt32(Settings.Instance.UserID), PartyId, txtitem.Text, Convert.ToDecimal(txtQuantity.Text), Convert.ToDecimal(txtRate.Text), Convert.ToInt32(Settings.Instance.DSRSMID), imgurl, Remark.Text, compTextBox.Text, Convert.ToDecimal(discount), txtbrand.Text, txtmeet.Text, txtroadshow.Text, txtscheme.Text, txtother.Text, active);
                    if (RetSave > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                        this.clearcontrols();
                        btnDelete.Visible = false;
                        divdocid.Visible = false;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Cannot be Update');", true);
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

        protected void btnsave_Click(object sender, EventArgs e)
        {

            try
            {
                if (btnsave.Text == "Update")
                {
                    UpdateOrder();
                    chkotheractvity.Checked = false;
                    dvbrand.Style.Add("display", "none");
                    dvmeet.Style.Add("display", "none");
                    dvroadshow.Style.Add("display", "none");
                    dvscheme.Style.Add("display", "none");
                    dvotherinfo.Style.Add("display", "none");
                }
                else
                {
                    InsertOrder();
                    chkotheractvity.Checked = false;
                    dvbrand.Style.Add("display", "none");
                    dvmeet.Style.Add("display", "none");
                    dvroadshow.Style.Add("display", "none");
                    dvscheme.Style.Add("display", "none");
                    dvotherinfo.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            clearcontrols();
            chkotheractvity.Checked = false;
            dvbrand.Style.Add("display", "none");
            dvmeet.Style.Add("display", "none");
            dvroadshow.Style.Add("display", "none");
            dvscheme.Style.Add("display", "none");
            dvotherinfo.Style.Add("display", "none");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = dp.delete(Convert.ToString(ViewState["CollId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    btnDelete.Visible = false;
                    btnsave.Text = "Save";
                    clearcontrols();
                    chkotheractvity.Checked = false;
                    dvbrand.Style.Add("display", "none");
                    dvmeet.Style.Add("display", "none");
                    dvroadshow.Style.Add("display", "none");
                    dvscheme.Style.Add("display", "none");
                    dvotherinfo.Style.Add("display", "none");

                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }


        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            btnDelete.Visible = true;
            btnsave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchCompetitor()
        { //Ankita - 18/may/2016- (For Optimization)
            //string str = "select distinct CompName from TransCompetitor ORDER BY CompName";
            string str = "select distinct CompName,ComptId from TransCompetitor ORDER BY CompName";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            List<string> customers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["CompName"].ToString(), dt.Rows[i]["ComptId"].ToString());
                customers.Add(item);
            }
            return customers;

        }
        protected void btnBack_Click1(object sender, EventArgs e)
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
