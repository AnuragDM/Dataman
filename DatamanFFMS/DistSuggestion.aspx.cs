using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BusinessLayer;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Mime;

namespace AstralFFMS
{
    public partial class DistSuggestion : System.Web.UI.Page
    {
        BAL.TransSuggestion.TSuggAll tsAll = new BAL.TransSuggestion.TSuggAll();
        string parameter = "";
         int msg = 0;
         int uid = 0;
         string roleType = "";
        // int disId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["sugglId"] = parameter;
                FillSuggControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (!IsPostBack)
            {
                if (Session["user_name"] != null)
                {
                    //            BindComplaintNature();
                    BindComplaintNatureFilter();
                    Settings.Instance.BindDepartment(ddldept);

                 //   disId = GetDistId(Settings.Instance.UserID);
                    btnDelete.Visible = false;
                    mainDiv.Style.Add("display", "block");
                }
            }
            //Ankita - 12/may/2016- (For Optimization)
            //string pageName = Path.GetFileName(Request.Path);
            //string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            //string[] SplitPerm = PermAll.Split(',');
            //if (btnSave.Text == "Save")
            //{
            //   // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
            //    btnSave.CssClass = "btn btn-primary";
            //}
            //else
            //{
            //    //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
            //    btnSave.CssClass = "btn btn-primary";
            //}
            //btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            ////btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            //btnDelete.CssClass = "btn btn-primary";
        }
        private void BindComplaintNatureFilter()
        {
            try
            {//Ankita - 12/may/2016- (For Optimization)
               // string distSuggNaturequery = @"select * from MastComplaintNature where NatureType='Suggestion' and Active=1 order by Name";
                string distSuggNaturequery = @"select Id,Name from MastComplaintNature where NatureType='Suggestion' and Active=1 order by Name";
                DataTable suggValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, distSuggNaturequery);

                if (suggValueDt.Rows.Count > 0)
                {
                    DropDownList1.DataSource = suggValueDt;
                    DropDownList1.DataTextField = "Name";
                    DropDownList1.DataValueField = "Id";
                    DropDownList1.DataBind();
                }
                DropDownList1.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private int GetDistId(string userId)
        {
            try
            {
                string distquery = @"select PartyId from MastParty where UserId=" + userId + " and PartyDist=1";
                return Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, distquery));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }
        }

        private void FillSuggControls(int suggId)
        {
            try
            {
                string suggquery = @"select * from TransSuggestion where SuggId=" + suggId;

                DataTable suggValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, suggquery);
                if (suggValueDt.Rows.Count > 0)
                {
                    string strcq = @"select DeptId from MastComplaintNature WHERE Id= " + Convert.ToInt32(suggValueDt.Rows[0]["ComplNatId"]);
                    DataTable dtcq = DbConnectionDAL.GetDataTable(CommandType.Text, strcq);

                    ddldept.SelectedValue = Convert.ToString(dtcq.Rows[0]["DeptId"]);

                    BindComplaintNature(Convert.ToInt32(dtcq.Rows[0]["DeptId"]));                   

                    ddlComplaintNature.SelectedValue = suggValueDt.Rows[0]["ComplNatId"].ToString();

                    //Added as per UAT - on 12-Dec-2015
                    docIDHdf.Value = suggValueDt.Rows[0]["DocId"].ToString();
                    //End

                    if (suggValueDt.Rows[0]["ItemId"] != DBNull.Value)
                    {
                        int itemId = Convert.ToInt32(suggValueDt.Rows[0]["ItemId"]);
                        hfItemId.Value = itemId.ToString();
                        //Ankita - 12/may/2016- (For Optimization)
                        string str = "select SyncId,ItemName,ItemCode FROM MastItem where ItemId=" + itemId + "";
                        //string str = "select * FROM MastItem where ItemId=" + itemId + "";
                        DataTable dt = new DataTable();

                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        txtSearch.Text = "(" + dt.Rows[0]["SyncId"].ToString() + ")" + " " + dt.Rows[0]["ItemName"].ToString() + " " + "(" + dt.Rows[0]["ItemCode"].ToString() + ")";
                    }
                    TextArea1.Value = suggValueDt.Rows[0]["NewApplicationArea"].ToString();
                    TextArea2.Value = suggValueDt.Rows[0]["TechnicalAdvantage"].ToString();
                    TextArea3.Value = suggValueDt.Rows[0]["MakeProductBetter"].ToString();
                    if (suggValueDt.Rows[0]["ImgUrl"].ToString() != string.Empty)
                    {
                        imgpreview.Src = suggValueDt.Rows[0]["ImgUrl"].ToString();
                        imgpreview.Style.Add("display", "block");
                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while populating records');", true);
            }
        }

        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        private void BindComplaintNature(int deptID)
        {
            string dropdowndata = string.Empty;
            ddlComplaintNature.Items.Clear();
            try
            {//Ankita - 12/may/2016- (For Optimization)
                //string complNaturequery = @"select * from MastComplaintNature where DeptId=" + deptID + " and NatureType='Suggestion' and Active=1 order by Name";
                string complNaturequery = @"select Id,Name from MastComplaintNature where DeptId=" + deptID + " and NatureType='Suggestion' and Active=1 order by Name";
                DataTable complValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, complNaturequery);
                if (complValueDt.Rows.Count > 0)
                {
                    ddlComplaintNature.DataSource = complValueDt;
                    ddlComplaintNature.DataTextField = "Name";
                    ddlComplaintNature.DataValueField = "Id";
                    ddlComplaintNature.DataBind();

                    DropDownList1.DataSource = complValueDt;
                    DropDownList1.DataTextField = "Name";
                    DropDownList1.DataValueField = "Id";
                    DropDownList1.DataBind();
                }
                ddlComplaintNature.Items.Insert(0, new ListItem("-- Select --", "0"));
                DropDownList1.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch
            {
            }
        }
        private void ClearControls()
        {
            ddlComplaintNature.SelectedIndex = 0;
            TextArea1.Value = string.Empty;
            TextArea2.Value = string.Empty;
            TextArea3.Value = string.Empty;
            txtSearch.Text = string.Empty;

            //Added
            ddldept.SelectedIndex = 0;
            //End
            imgpreview.Style.Add("display", "none");
            btnDelete.Visible = false;
            btnSave.Text = "Save";
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //      fillRepeter();
            rpt.DataSource = null;
            rpt.DataBind();
            //txtfmDate.Text = string.Empty;
            //txttodate.Text = string.Empty;
            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End

            DropDownList1.SelectedIndex = 0;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateTransSuggestion();
                }
                else
                {
                    InsertTransSuggestion();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertTransSuggestion()
        {
            try
            {
                DateTime newDate = GetUTCTime();
                string docId = tsAll.GetDociId(newDate);
                string imgurl = "", getItemName = "";
                
                    if (sugImgFileUpload.HasFile)
                    {
                        string directoryPath = Server.MapPath(string.Format("~/{0}/", "ProductImages"));
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        string filename = Path.GetFileName(sugImgFileUpload.FileName);
                        bool k = ValidateImageSize();
                        if (k != true)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                            return;
                        }
                        sugImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/D_S_" + filename));

                        //#region Image Compression Code
                        //sugImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
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
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/D_S_" + filename), ImageFormat.Jpeg);
                        //bitMAP1.Dispose();
                        //bitMAP1.Dispose();
                        //image.Dispose();

                        //// start image size validation after image compression > Priyanka 02/03/2016
                        //#region
                        //DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/ProductImages"));
                        //// Get a reference to each file in that directory.
                        //FileInfo[] fiArr = di.GetFiles();
                        //// Display the names and sizes of the files.
                        ////Console.WriteLine("The directory {0} contains the following files:", di.Name);
                        //foreach (FileInfo f in fiArr)
                        //{
                        //    if (f.Name == "D_S_" + filename)
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


                        imgurl = "~/ProductImages" + "/D_S_" + filename;
                    }
                    if (hfItemId.Value != "")
                    {
                        string getItemQry = @"select ItemName from MastItem where ItemId=" + Convert.ToInt32(hfItemId.Value) + "";
                        DataTable itemNameQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, getItemQry);
                        if (itemNameQryDt.Rows.Count > 0)
                        {
                            getItemName = itemNameQryDt.Rows[0]["ItemName"].ToString();
                        }

                    }
                    int retsave = tsAll.Insert(newDate, docId, Convert.ToInt32(Settings.Instance.UserID), ddlComplaintNature.SelectedValue, hfItemId.Value, TextArea1.Value, TextArea2.Value, TextArea3.Value, imgurl, Convert.ToInt32(0), Convert.ToInt32(Settings.Instance.DistributorID), "D");

                    tsAll.SetDociId(docId);

                    SenEmailTemplate(ddlComplaintNature.SelectedValue, ddlComplaintNature.SelectedItem.Text, getItemName, TextArea1.Value, TextArea2.Value, TextArea3.Value, docId, newDate);

                    if (retsave != 1)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully <br/> DocNo-" + docId + "');", true);
                        ClearControls();
                    }                
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected bool ValidateImageSize()
        {
            int fileSize = sugImgFileUpload.PostedFile.ContentLength;
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


        private void SenEmailTemplate(string ComplNatId, string ComplaintNature, string getItemName, string NewAppArea, string TechAdv, string productBetter, string docId, DateTime vDate)
        {
            try
            {
                string smName = "", getLoginDealerame = "";
                string[] emailToAll = new string[20];
                string[] emailCCAll = new string[20];
                string emailNew = "";
                string emailCC = "";
                string smEmail = "";
                string loginDealerqry = @"select PartyName from MastParty where UserId=" + Convert.ToInt32(Settings.Instance.UserID) + " and Active=1 and PartyDist=1";
                DataTable distNAmeQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, loginDealerqry);
                if (distNAmeQryDt.Rows.Count > 0)
                {
                    getLoginDealerame = distNAmeQryDt.Rows[0]["PartyName"].ToString();
                }
                //Ankita - 12/may/2016- (For Optimization)
               // string complNatQry = @"select * from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1";
                string complNatQry = @"select EmailTo,EmailCC from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1";
                DataTable complNatQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, complNatQry);
                if (complNatQryDt.Rows.Count > 0)
                {
                    emailNew = complNatQryDt.Rows[0]["EmailTo"].ToString();
                    string[] delimiters = new string[] { ",", ";" };
                    emailToAll = emailNew.Split(delimiters, StringSplitOptions.None);
                    emailCC = complNatQryDt.Rows[0]["EmailCC"].ToString();
                    emailCCAll = emailCC.Split(delimiters, StringSplitOptions.None);

                    SendEmail(ComplaintNature, getLoginDealerame, getItemName, NewAppArea, TechAdv, productBetter, docId, vDate, emailToAll, emailCCAll);
                    //if (emailToAll.Length > 0)
                    //{
                    //    SendEmail(ComplaintNature, getLoginDealerame, getItemName, NewAppArea, TechAdv, productBetter, docId, vDate, emailToAll);
                    //}
                    //if (emailCCAll.Length > 0)
                    //{
                    //    SendEmail(ComplaintNature, getLoginDealerame, getItemName, NewAppArea, TechAdv, productBetter, docId, vDate, emailCCAll);
                    //}
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + ex.Message + "');", true);
            }
        }

        public void SendEmail(string ComplaintNature, string userName, string ItemName, string NewAppArea, string TechAdv, string productBetter, string docId, DateTime vDate, string[] emailTo, string[] emailCC)
        {
            try
            {
                //Added As Per UAT- On 11-Dec-2015
                string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.MailServer
                            FROM MastEnviro AS T1";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='DistSuggestion'";
                    DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                    string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='DistSuggestion'";
                    DataTable dtEmailTemplate = DbConnectionDAL.GetDataTable(CommandType.Text, strEmailTemplate);

                    string strSubject = Convert.ToString(dtEmailTemplate.Rows[0]["Subject"]);
                    string strMailBody = Convert.ToString(dtEmailTemplate.Rows[0]["TemplateValue"]);

                    if (dtVar != null && dtVar.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtVar.Rows.Count; j++)
                        {
                            if (strSubject.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                            {
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{UserName}")
                                {
                                    strSubject = strSubject.Replace("{{UserName}}", userName);
                                }
                            }

                            ///////////////////////////////////////////
                            if (strMailBody.Contains(Convert.ToString(dtVar.Rows[j]["Name"])))
                            {
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{VDate}")
                                {
                                    strMailBody = strMailBody.Replace("{{VDate}}", vDate.ToString());
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{DocID}")
                                {
                                    strMailBody = strMailBody.Replace("{{DocID}}", docId);
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ComplNature}")
                                {
                                    strMailBody = strMailBody.Replace("{{ComplNature}}", ComplaintNature);
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemName}")
                                {
                                    strMailBody = strMailBody.Replace("{{ItemName}}", ItemName);
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{NewAppArea}")
                                {
                                    strMailBody = strMailBody.Replace("{{NewAppArea}}", NewAppArea);
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{TechAdv}")
                                {
                                    strMailBody = strMailBody.Replace("{{TechAdv}}", TechAdv);
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ProductBetter}")
                                {
                                    strMailBody = strMailBody.Replace("{{ProductBetter}}", productBetter);
                                }
                            }
                        }
                    }

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(Convert.ToString(dt.Rows[0]["SenderEmailId"]));
                    mail.Subject = strSubject;
                    mail.Body = strMailBody;


                    if (emailTo.Length > 0)
                    {
                        for (int i = 0; i < emailTo.Length; i++)
                        {
                            mail.To.Add(new MailAddress(emailTo[i]));
                        }
                    }
                    if (emailCC.Length > 0)
                    {
                        // Nishu 09/08/2016   (If emailcc value null then mail not go 
                        //for (int j = 0; j < emailCC.Length; j++)
                        //{
                        //    mail.CC.Add(new MailAddress(emailCC[j]));
                        //}
                        for (int j = 0; j < emailCC.Length; j++)
                        {
                            if (emailCC[j].Length.ToString() != "0")
                            {
                                mail.CC.Add(new MailAddress(emailCC[j]));
                            }

                        }
                    }

                    //if (sugImgFileUpload.HasFile)
                    //{
                    //    mail.Attachments.Add(new Attachment(sugImgFileUpload.FileContent, System.IO.Path.GetFileName(sugImgFileUpload.FileName)));
                    //}
                    if (sugImgFileUpload.HasFile)
                    {
                        string filename = Path.GetFileName(sugImgFileUpload.FileName);
                        string attachmentPath = Server.MapPath("~/ProductImages" + "/D_S_" + filename);

                        Attachment inline = new Attachment(attachmentPath);
                        inline.ContentDisposition.Inline = true;
                        inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                        inline.ContentId = Guid.NewGuid().ToString();
                        inline.ContentType.MediaType = "image/png";
                        inline.ContentType.Name = Path.GetFileName(attachmentPath);

                        mail.Attachments.Add(inline);
                    }
                    NetworkCredential mailAuthenticaion = new NetworkCredential(Convert.ToString(dt.Rows[0]["SenderEmailId"]), Convert.ToString(dt.Rows[0]["SenderPassword"]));

                    SmtpClient mailclient = new SmtpClient(Convert.ToString(dt.Rows[0]["MailServer"]), Convert.ToInt32(dt.Rows[0]["Port"]));
                    mailclient.EnableSsl = false;
                    mailclient.UseDefaultCredentials = false;
                    mailclient.Credentials = mailAuthenticaion;
                    mail.IsBodyHtml = true;
                    mailclient.Send(mail);
                }
             
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void UpdateTransSuggestion()
        {
            try
            {
                DateTime newDate = GetUTCTime();

                string imgurl = "",getItemNameNew="";
                string itemId = "";
               
                    if (sugImgFileUpload.HasFile)
                    {
                        string directoryPath = Server.MapPath(string.Format("~/{0}/", "ProductImages"));
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        string filename = Path.GetFileName(sugImgFileUpload.FileName);
                        bool k = ValidateImageSize();
                        if (k != true)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                            return;
                        }
                        sugImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/D_S_" + filename));

                        //#region Image Compression Code
                        //sugImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
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
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/D_S_" + filename), ImageFormat.Jpeg);
                        //bitMAP1.Dispose();
                        //bitMAP1.Dispose();
                        //image.Dispose();

                        //// start image size validation after image compression > Priyanka 02/03/2016
                        //#region
                        //DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/ProductImages"));
                        //// Get a reference to each file in that directory.
                        //FileInfo[] fiArr = di.GetFiles();
                        //// Display the names and sizes of the files.
                        ////Console.WriteLine("The directory {0} contains the following files:", di.Name);
                        //foreach (FileInfo f in fiArr)
                        //{
                        //    if (f.Name == "D_S_" + filename)
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
                        imgurl = "~/ProductImages" + "/D_S_" + filename;
                    }
                    else
                    {
                        imgurl = imgpreview.Src;
                    }
                    if (txtSearch.Text != string.Empty)
                    {
                        itemId = Request.Form[hfItemId.UniqueID];

                        //Added as per UAT - 12-Dec-2015
                        string getItemQry = @"select ItemName from MastItem where ItemId=" + itemId + "";
                        DataTable itemNameQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, getItemQry);
                        if (itemNameQryDt.Rows.Count > 0)
                        {
                            getItemNameNew = itemNameQryDt.Rows[0]["ItemName"].ToString();
                        }
                        //End
                    }
                    int retsave = tsAll.Update(Convert.ToInt64(ViewState["sugglId"]), newDate, Convert.ToInt32(Settings.Instance.UserID), ddlComplaintNature.SelectedValue, itemId, TextArea1.Value, TextArea2.Value, TextArea3.Value, imgurl, Convert.ToInt32(0), Convert.ToInt32(Settings.Instance.DistributorID), "D");
                    if (retsave != 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                        //Added As Per UAT - 12-Dec-2015
                        SenEmailTemplate(ddlComplaintNature.SelectedValue, ddlComplaintNature.SelectedItem.Text, getItemNameNew, TextArea1.Value, TextArea2.Value, TextArea3.Value, docIDHdf.Value, newDate);
                        //End

                        ClearControls();
                    }                
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while updating records');", true);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistSuggestion.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                int retdel = tsAll.delete(Convert.ToString(ViewState["sugglId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                }
            }
            else
            {
                //      this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string mainQry = "";
            if (DropDownList1.SelectedIndex != 0)
            {
                mainQry = " and ComplNatId=" + Convert.ToInt32(DropDownList1.SelectedValue) + "";
            }

            //if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty && DropDownList1.SelectedIndex != 0)
            //{
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            {
//                string suggquery1 = @"select r.SuggId, r.DocId, r.Vdate, r.UserId, r.ComplNatId, r.Category,r.DistId, r.ItemId,CNature.Name, 
//                                              Item.ItemName, r.NewApplicationArea, r.TechnicalAdvantage, r.MakeProductBetter, r.BatchNo, r.ManufactureDate 
//                                              from TransSuggestion r left join MastComplaintNature CNature on r.ComplNatId=CNature.Id
//                                              left join MastItem Item on r.ItemId= Item.ItemId where r.DistId=" + disId + " " + mainQry + " and r.Vdate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' order by r.Vdate desc";
                string suggquery1 = "select * from [View_DistSuggestion] where DistId=" + Settings.Instance.DistributorID + " " + mainQry + " and Vdate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' order by Vdate desc";
                DataTable suggdt2 = DbConnectionDAL.GetDataTable(CommandType.Text, suggquery1);
                if (suggdt2.Rows.Count > 0)
                {
                    rpt.DataSource = suggdt2;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = suggdt2;
                    rpt.DataBind();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('To Date Cannot be less than from date.');", true);
                rpt.DataSource = null;
                rpt.DataBind();
            }

        }

        protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddldept.SelectedIndex != 0)
            {
                BindComplaintNature(Convert.ToInt32(ddldept.SelectedValue));
            }
            else
            {
                ddlComplaintNature.Items.Clear();
            }
        }

    }
}