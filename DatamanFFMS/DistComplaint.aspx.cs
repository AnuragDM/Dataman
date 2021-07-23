using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class DistComplaint : System.Web.UI.Page
    {
        BAL.TransComplaint.TComplAll tcAll = new BAL.TransComplaint.TComplAll();
        string parameter = "";
         string roleType = "";
         int disId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            calendarTextBox.Attributes.Add("readonly", "readonly");
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            calendarTextBox_CalendarExtender.EndDate = DateTime.Now;
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["complId"] = parameter;
                FillDistComplControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (!IsPostBack)
            {
                if (Session["user_name"] != null)
                {
                    //    complaintBy.Value = Session["user_name"].ToString();
                    //           BindComplaintNature();
                    BindComplaintNatureFilter();
                    //Ankita - 12/may/2016- (For Optimization)
                    roleType = Settings.Instance.RoleType;
                    //    GetRoleType(Settings.Instance.RoleID);

                    //Added By Abhishek As UAT
                    Settings.Instance.BindDepartment(ddldept);
                    //End

                    disId = GetDistId(Settings.Instance.UserID);
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
            //    // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
            //    btnSave.CssClass = "btn btn-primary";
            //}
            //else
            //{
            //    // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
            //    btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
            //    //btnSave.CssClass = "btn btn-primary";
            //}
            //btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            ////btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            //btnDelete.CssClass = "btn btn-primary";
        }
        private void BindComplaintNatureFilter()
        {
            try
            {//Ankita - 12/may/2016- (For Optimization)
                //string distComNaturequery = @"select * from MastComplaintNature where NatureType='Complaint' and Active=1 order by Name";
                string distComNaturequery = @"select Id,Name from MastComplaintNature where NatureType='Complaint' and Active=1 order by Name";
                DataTable suggValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, distComNaturequery);

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

        private void FillDistComplControls(int complId)
        {
            try
            {
                string complquery = @"select * from TransComplaint where ComplId=" + complId;

                DataTable complValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, complquery);
                if (complValueDt.Rows.Count > 0)
                {
                    string strcq = @"select DeptId from MastComplaintNature WHERE Id= " + Convert.ToInt32(complValueDt.Rows[0]["ComplNatId"]);
                     DataTable dtcq = DbConnectionDAL.GetDataTable(CommandType.Text, strcq);

                     ddldept.SelectedValue = Convert.ToString(dtcq.Rows[0]["DeptId"]);

                     BindComplaintNature(Convert.ToInt32(dtcq.Rows[0]["DeptId"]));
                    ddlComplaintNature.SelectedValue = complValueDt.Rows[0]["ComplNatId"].ToString();
                   
                    //Added as per UAT - on 12-Dec-2015
                    docIDHdf.Value = complValueDt.Rows[0]["DocId"].ToString();
                    //End

                    if (complValueDt.Rows[0]["ItemId"] != DBNull.Value)
                    {
                        int itemId = Convert.ToInt32(complValueDt.Rows[0]["ItemId"]);
                        hfItemId.Value = itemId.ToString();
                        //Ankita - 12/may/2016- (For Optimization)
                        //string str = "select * FROM MastItem where ItemId=" + itemId + "";
                        string str = "select SyncId,ItemName,ItemCode FROM MastItem where ItemId=" + itemId + "";
                        DataTable dt = new DataTable();

                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        txtSearch.Text = "(" + dt.Rows[0]["SyncId"].ToString() + ")" + " " + dt.Rows[0]["ItemName"].ToString() + " " + "(" + dt.Rows[0]["ItemCode"].ToString() + ")";
                    }
                    BatchNo.Value = complValueDt.Rows[0]["BatchNo"].ToString();
                    calendarTextBox.Text = string.Format("{0:dd/MM/yyyy}", complValueDt.Rows[0]["ManufactureDate"]);
                    TextArea1.Value = complValueDt.Rows[0]["Remark"].ToString();
                    if (complValueDt.Rows[0]["ImgUrl"].ToString() != string.Empty)
                    {
                        imgpreview.Src = complValueDt.Rows[0]["ImgUrl"].ToString();
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
            }
        }
        private void BindComplaintNature(int deptID)
        {
            string dropdowndata = string.Empty;
            ddlComplaintNature.Items.Clear();
            try
            {  //Ankita - 12/may/2016- (For Optimization)
                //string complNaturequery = @"select * from MastComplaintNature where DeptId=" + deptID + " and NatureType='Complaint' and Active=1 order by Name";
                string complNaturequery = @"select Id,Name from MastComplaintNature where DeptId=" + deptID + " and NatureType='Complaint' and Active=1 order by Name";
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
            {//Ankita - 12/may/2016- (For Optimization)
//                string compltquery1 = @"select  r.ComplId, r.DocId, r.Vdate, r.UserId, r.ComplNatId, r.Category, r.ItemId, r.ImgUrl, 
//                                                Dist.PartyName, CNature.Name, Item.ItemName, r.Remark, r.BatchNo, r.ManufactureDate, r.DistId,r.SMId from TransComplaint r
//                                                left join MastComplaintNature CNature on r.ComplNatId=CNature.Id
//                                                left join MastItem Item on r.ItemId=Item.ItemId
//                                                left join MastParty Dist on r.DistId=Dist.PartyId where CNature.NatureType='Complaint' and r.DistId=" + disId + " " + mainQry + " and Vdate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' order by Vdate desc";
                disId = GetDistId(Settings.Instance.UserID);
                string compltquery1 = @"select * from [View_DistComplaint] where NatureType='Complaint' and DistId=" + disId + " " + mainQry + " and Vdate between '" + Settings.dateformat(txtfmDate.Text) + "' and '" + Settings.dateformat(txttodate.Text) + "' order by Vdate desc";
                DataTable complaintdt2 = DbConnectionDAL.GetDataTable(CommandType.Text, compltquery1);
                if (complaintdt2.Rows.Count > 0)
                {
                    rpt.DataSource = complaintdt2;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = complaintdt2;
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                int retdel = tcAll.delete(Convert.ToString(ViewState["complId"]));
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistComplaint.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateDistDistTransComplaint();
                }
                else
                {
                    InsertDistTransComplaint();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }
        private void InsertDistTransComplaint()
        {
            try
            {
                DateTime newDate = GetUTCTime();
                string docId = tcAll.GetDociId(newDate);
                string imgurl = "", itemId = "", getItemName = "", getDistName = ""; int smID = 0;
               
                    if (comImgFileUpload.HasFile)
                    {
                        string directoryPath = Server.MapPath(string.Format("~/{0}/", "ProductImages"));
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        string filename = Path.GetFileName(comImgFileUpload.FileName);
                        bool k = ValidateImageSize();
                        if (k != true)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                            return;
                        }
                        comImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/D_C_" + filename));

                        //#region Image Compression Code
                        //comImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
                        //System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("TempImages" + filename));
                        //int newwidthimg = 220;
                        //float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                        //float Ht = 220;
                        //int newHeight = Convert.ToInt32(Ht / AspectRatio);
                        //Bitmap bitMAP1 = new Bitmap(newwidthimg, newHeight);
                        //Graphics imgGraph = Graphics.FromImage(bitMAP1);                  
                        //imgGraph.SmoothingMode = SmoothingMode.HighQuality;
                        //imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //var imgDimesions = new Rectangle(0, 0, newwidthimg, newHeight);
                        //imgGraph.DrawImage(image, 0, 0, newwidthimg, newHeight);
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/D_C_" + filename), ImageFormat.Jpeg);
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
                        //    if (f.Name == "D_C_" + filename)
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

                        imgurl = "~/ProductImages" + "/D_C_" + filename;
                        //   imgurl = filename;
                    }

                    if (txtSearch.Text != string.Empty)
                    {
                        itemId = hfItemId.Value;
                        string getItemQry = @"select ItemName from MastItem where ItemId=" + itemId + "";
                        DataTable itemNameQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, getItemQry);
                        if (itemNameQryDt.Rows.Count > 0)
                        {
                            getItemName = itemNameQryDt.Rows[0]["ItemName"].ToString();
                        }

                    }
                    disId = GetDistId(Settings.Instance.UserID);
                    int retsave = tcAll.Insert(ddlComplaintNature.SelectedValue, calendarTextBox.Text, disId.ToString(), itemId, BatchNo.Value,
                        TextArea1.Value, imgurl, Convert.ToInt32(Settings.Instance.UserID), newDate, docId, Convert.ToInt32(smID), "D");

                    tcAll.SetDociId(docId);

                    SenEmailTemplate(ddlComplaintNature.SelectedValue, ddlComplaintNature.SelectedItem.Text, calendarTextBox.Text, getDistName, getItemName, BatchNo.Value, TextArea1.Value, docId, newDate);

                    if (retsave != 1)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully <br/> DocNo-" + docId + "');", true);
                        ClearControls();
                    }                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        protected bool ValidateImageSize()
        {
            int fileSize = comImgFileUpload.PostedFile.ContentLength;
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

        private void SenEmailTemplate(string ComplNatId, string ComplaintNature, string manufactureDate, string DistName, string ItemName, string BatchNo, string Remark, string docId, DateTime vDate)
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
              //  string complNatQry = @"select * from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1";
                string complNatQry = @"select EmailTo,EmailCC from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1";
                DataTable complNatQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, complNatQry);
                if (complNatQryDt.Rows.Count > 0)
                {
                    emailNew = complNatQryDt.Rows[0]["EmailTo"].ToString();
                    string[] delimiters = new string[] { ",", ";" };
                    emailToAll = emailNew.Split(delimiters, StringSplitOptions.None);
                    emailCC = complNatQryDt.Rows[0]["EmailCC"].ToString();
                    emailCCAll = emailCC.Split(delimiters, StringSplitOptions.None);

                    SendEmail(ComplaintNature, manufactureDate, getLoginDealerame, ItemName, BatchNo, Remark, docId, vDate, emailToAll, emailCCAll);
                    //if (emailToAll.Length > 0)
                    //{
                    //    SendEmail(ComplaintNature, manufactureDate, getLoginDealerame, ItemName, BatchNo, Remark, docId, vDate, emailToAll);
                    //}
                    //if (emailCCAll.Length > 0)
                    //{
                    //    SendEmail(ComplaintNature, manufactureDate, getLoginDealerame, ItemName, BatchNo, Remark, docId, vDate, emailCCAll);
                    //}
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + ex.Message + "');", true);
            }
        }

        public void SendEmail(string ComplaintNature, string manufactureDate, string userName, string ItemName, string BatchNo, string Remark, string docId, DateTime vDate, string[] emailTo, string[] emailCC)
        {
            try
            {
                //Added As Per UAT- On 11-Dec-2015
                string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.MailServer
                            FROM MastEnviro AS T1";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='DistComplaint'";
                    DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                    string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='DistComplaint'";
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
                                if (ItemName != "")
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemName}")
                                    {
                                        strMailBody = strMailBody.Replace("{{ItemName}}", ItemName);
                                    }
                                }
                                else
                                {
                                    strMailBody = strMailBody.Replace("Product Name - {{ItemName}}", string.Empty);
                                }
                                if (BatchNo != "")
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{BatchNo}")
                                    {
                                        strMailBody = strMailBody.Replace("{{BatchNo}}", BatchNo);
                                    }
                                }
                                else
                                {
                                    strMailBody = strMailBody.Replace("Batch No - {{BatchNo}}", string.Empty);
                                }
                                if (manufactureDate != "")
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{MfgDate}")
                                    {
                                        strMailBody = strMailBody.Replace("{{MfgDate}}", manufactureDate);
                                    }
                                }
                                else
                                {
                                    strMailBody = strMailBody.Replace("Mfg. Date -  {{MfgDate}}", string.Empty);
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remark}")
                                {
                                    strMailBody = strMailBody.Replace("{{Remark}}", Remark);
                                }
                                //if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Status}")
                                //{
                                //    strMailBody = strMailBody.Replace("{Status}", "Pending");
                                //}
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
                        for (int j = 0; j < emailCC.Length; j++)
                        {
                            mail.CC.Add(new MailAddress(emailCC[j]));
                        }
                    }


                    //string[] toEmail = emailTo;
                    //foreach (var m in toEmail)
                    //{
                    //    mail.CC.Add(m);
                    //}
                    //if (comImgFileUpload.HasFile)
                    //{
                    //    mail.Attachments.Add(new Attachment(comImgFileUpload.FileContent, System.IO.Path.GetFileName(comImgFileUpload.FileName)));
                    //}
                    if (comImgFileUpload.HasFile)
                    {
                        string filename = Path.GetFileName(comImgFileUpload.FileName);
                        string attachmentPath = Server.MapPath("~/ProductImages" + "/D_C_" + filename);

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

        private void ClearControls()
        {
            ddlComplaintNature.SelectedIndex = 0;
            BatchNo.Value = string.Empty;
            calendarTextBox.Text = string.Empty;
            TextArea1.Value = string.Empty;

            //Added
            ddldept.SelectedIndex = 0;
            //End
            txtSearch.Text = string.Empty;
            comImgFileUpload.Attributes.Clear();
            imgpreview.Style.Add("display", "none");
            btnDelete.Visible = false;
            btnSave.Text = "Save";
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {
            //Ankita - 12/may/2016- (For Optimization)
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

        private void UpdateDistDistTransComplaint()
        {
            try
            {
                DateTime newDate = GetUTCTime();

                string imgurl = "", getDistName = "", getItemNameNew="";
                string itemId = ""; int smID = 0;
               
                    if (comImgFileUpload.HasFile)
                    {
                        string directoryPath = Server.MapPath(string.Format("~/{0}/", "ProductImages"));
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        string filename = Path.GetFileName(comImgFileUpload.FileName);
                        bool k = ValidateImageSize();
                        if (k != true)
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                            return;
                        }

                        //#region Image Compression Code
                        //comImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
                        //System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("TempImages" + filename));
                        //int newwidthimg = 200;
                        //float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                        //int newHeight = 200;
                        //Bitmap bitMAP1 = new Bitmap(newwidthimg, newHeight);
                        //Graphics imgGraph = Graphics.FromImage(bitMAP1);
                        ////imgGraph.imgQuality = CompositingQuality.HighQuality;
                        //imgGraph.SmoothingMode = SmoothingMode.HighQuality;
                        //imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //var imgDimesions = new Rectangle(0, 0, newwidthimg, newHeight);
                        //imgGraph.DrawImage(image, 0, 0, newwidthimg, newHeight);
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/D_C_" + filename), ImageFormat.Jpeg);
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
                        //    if (f.Name == "D_C_" + filename)
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

                        imgurl = "~/ProductImages" + "/D_C_" + filename;
                    }
                    else
                    {
                        imgurl = imgpreview.Src;
                    }
                    if (txtSearch.Text != string.Empty)
                    {
                        itemId = Request.Form[hfItemId.UniqueID];

                        //Added as per UAT - 12-De-2015
                        string getItemQry = @"select ItemName from MastItem where ItemId=" + itemId + "";
                        DataTable itemNameQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, getItemQry);
                        if (itemNameQryDt.Rows.Count > 0)
                        {
                            getItemNameNew = itemNameQryDt.Rows[0]["ItemName"].ToString();
                        }

                        //End
                    }
                    disId = GetDistId(Settings.Instance.UserID);
                    int retsave = tcAll.Update(Convert.ToInt64(ViewState["complId"]), ddlComplaintNature.SelectedValue, calendarTextBox.Text, disId.ToString(), itemId, BatchNo.Value, TextArea1.Value, imgurl, Convert.ToInt32(Settings.Instance.UserID), newDate, Convert.ToInt32(smID), "D");
                    if (retsave != 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                        //Added as per UAT - on 12-Dec-2015
                        SenEmailTemplate(ddlComplaintNature.SelectedValue, ddlComplaintNature.SelectedItem.Text, calendarTextBox.Text, getDistName, getItemNameNew, BatchNo.Value, TextArea1.Value, docIDHdf.Value, newDate);
                        //End

                        ClearControls();
                    }               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
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