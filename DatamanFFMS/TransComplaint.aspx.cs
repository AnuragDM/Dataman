using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BusinessLayer;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net.Mime;

namespace AstralFFMS
{
    public partial class TransComplaint : System.Web.UI.Page
    {
        //   public static OpeartionDataContext context = new OpeartionDataContext();
        //    UserPermission UP = new UserPermission();
        BAL.TransComplaint.TComplAll tcAll = new BAL.TransComplaint.TComplAll();
        string parameter = "";
        int msg = 0;
        int uid = 0;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {                       
            calendarTextBox_CalendarExtender.EndDate = DateTime.Now;
            calendarTextBox.Attributes.Add("readonly", "readonly");
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["complId"] = parameter;
                FillComplControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (!IsPostBack)
            {
                if (Session["user_name"] != null)
                {
                    //    complaintBy.Value = Session["user_name"].ToString();
                    //         BindComplaintNature();
                    BindPartyType();
                    BindComplaintNatureFilter();
                    BindSalePersonDDl();
                    DdlSalesPerson.SelectedValue = Settings.Instance.SMID;
                    //       BindDistributor();
                    //               GetUserID(Session["user_name"].ToString());
                    Settings.Instance.BindDepartment(ddldept);
                    //Ankita - 12/may/2016- (For Optimization)
                    roleType = Settings.Instance.RoleType;
                    //GetRoleType(Settings.Instance.RoleID);
                    btnDelete.Visible = false;
                    mainDiv.Style.Add("display", "block");
                }
            }
            //Ankita - 12/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
               // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
        }
        private void BindPartyType() 
        {
            try
            { //Ankita - 12/may/2016- (For Optimization)
             //   string partytypequery = @"select * from PartyType order by PartyTypeName";
                string partytypequery = @"select PartyTypeId,PartyTypeName from PartyType order by PartyTypeName";
                DataTable partytypedt = DbConnectionDAL.GetDataTable(CommandType.Text, partytypequery);

                if (partytypedt.Rows.Count > 0)
                {
                    ddlpartytype.DataSource = partytypedt;
                    ddlpartytype.DataTextField = "PartyTypeName";
                    ddlpartytype.DataValueField = "PartyTypeId";
                    ddlpartytype.DataBind();
                }
                ddlpartytype.Items.Insert(0, new ListItem("-- Select --", "0"));
                ddlpartytype.Items.Insert(1, new ListItem("DISTRIBUTOR", null));
                //ddlpartytype.Items.Add("DISTRIBUTOR");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindComplaintNatureFilter()
        {
            try
            {//Ankita - 12/may/2016- (For Optimization)
                //string suggNaturequery = @"select * from MastComplaintNature where NatureType='Complaint' and Active=1 order by Name";
                string suggNaturequery = @"select Id,Name from MastComplaintNature where NatureType='Complaint' and Active=1 order by Name";
                DataTable suggValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, suggNaturequery);

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
        private void BindSalePersonDDl()
        {
            DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            DataView dv = new DataView(dt);
            dv.RowFilter = "SMName<>'.'";
            DdlSalesPerson.DataSource = dv.ToTable();
            DdlSalesPerson.DataTextField = "SMName";
            DdlSalesPerson.DataValueField = "SMId";
            DdlSalesPerson.DataBind();

            //Addes as per UAT - on 12-Dec-2015
            salesPersonDdl2.DataSource = dv.ToTable();
            salesPersonDdl2.DataTextField = "SMName";
            salesPersonDdl2.DataValueField = "SMId";
            salesPersonDdl2.DataBind();
            //End
            //Add Default Item in the DropDownList
            salesPersonDdl2.Items.Insert(0, new ListItem("--Please select--"));
        }

        //private void GetRoleType(string p)
        //{
        //    try
        //    {
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
        
        private void FillComplControls(int complId)
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
                    BindPartyType();
                    if (complValueDt.Rows[0]["DistId"] != DBNull.Value)
                    {
                        string SPtype = "select PartyDist from MastParty where partyId=" + complValueDt.Rows[0]["DistId"].ToString() + "";
                        int partydist =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, SPtype));
                        if (partydist > 0)
                        {
                            //Distributor
                           //Nishu 16/06/2017
                            //string qry = "select PartyTypeId,PartyTypeName from partytype where PartyTypeName='DISTRIBUTOR'";
                            //DataTable dtdist = new DataTable();
                            //dtdist = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
                            //if (dtdist.Rows.Count > 0)
                            //{
                            //    ddlpartytype.SelectedValue = (dtdist.Rows[0]["PartyTypeId"].ToString());
                            //    ddlpartytype.SelectedItem.Text = (dtdist.Rows[0]["PartyTypeName"].ToString());
                            //}
                            ddlpartytype.SelectedValue = null;
                            ddlpartytype.SelectedItem.Text = "DISTRIBUTOR";
                        }
                        else {
                            //Others
                            string qry1 = "select PartyType from MastParty where partyId=" + complValueDt.Rows[0]["DistId"].ToString() + "";
                            DataTable dtdist1 = new DataTable();
                            dtdist1 = DbConnectionDAL.GetDataTable(CommandType.Text, qry1);
                            if (dtdist1.Rows.Count > 0)
                            {
                                ddlpartytype.SelectedValue = (dtdist1.Rows[0]["PartyType"].ToString());
                            }
                        }  
                  
                        BindPartyTypePersons();

                        ddlpartytypepersons.SelectedValue = complValueDt.Rows[0]["DistId"].ToString();

                        //int partyId = Convert.ToInt32(complValueDt.Rows[0]["DistId"]);
                        //hfDistId.Value = partyId.ToString();
                        //string str = "select  mp.*,ma.AreaName FROM MastParty mp left join MastArea ma on mp.CityId=ma.AreaId where mp.PartyId=" + partyId + "";
                        //DataTable dt = new DataTable();

                        //dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        //txtDist.Text = "(" + dt.Rows[0]["PartyName"].ToString() + ")" + " " + dt.Rows[0]["SyncId"].ToString() + " " + "(" + dt.Rows[0]["AreaName"].ToString() + ")";
                    }

                    if (complValueDt.Rows[0]["ItemId"] != DBNull.Value || complValueDt.Rows[0]["ItemId"] != "0")
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
                    calendarTextBox.Text = string.Format("{0:dd/MMM/yyyy}", complValueDt.Rows[0]["ManufactureDate"]);
                    TextArea1.Value = complValueDt.Rows[0]["Remark"].ToString();
                    DdlSalesPerson.SelectedValue = complValueDt.Rows[0]["SMId"].ToString();
                    if (complValueDt.Rows[0]["ImgUrl"] != string.Empty)
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
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while populating records');", true);
            }
        }
//        private void fillRepeter()
//        {

//            string compltquery = @"select  r.ComplId, r.DocId, r.Vdate, r.UserId, r.ComplNatId, r.Category, r.ItemId, r.ImgUrl, 
//                                Dist.PartyName, CNature.Name, Item.ItemName, r.Remark, r.BatchNo, r.ManufactureDate, r.DistId,r.SMId from TransComplaint r
//                                left join MastComplaintNature CNature on r.ComplNatId=CNature.Id
//                                left join MastItem Item on r.ItemId=Item.ItemId
//                                left join MastParty Dist on r.DistId=Dist.PartyId where r.UserId=" + Settings.Instance.UserID + " order by Vdate desc";
//            DataTable complaintdt = DbConnectionDAL.GetDataTable(CommandType.Text, compltquery);
//            rpt.DataSource = complaintdt;
//            rpt.DataBind();
//        }


        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {
            //Ankita - 12/may/2016- (For Optimization)
          //  string str = "select * FROM MastItem where (ItemName like '%" + prefixText + "%' or SyncId like '%" + prefixText + "%' or ItemCode like '%" + prefixText + "%') and ItemType='Item' and Active=1";
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

        private void BindComplaintNature(int deptID)
        {//Ankita - 12/may/2016- (For Optimization)
            string dropdowndata = string.Empty;
            ddlComplaintNature.Items.Clear();
            try
            {
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateTransComplaint();
                }
                else
                {
                    InsertTransComplaint();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        private void InsertTransComplaint()
        {
            try
            {
                DateTime newDate = GetUTCTime();
                string docId = tcAll.GetDociId(newDate);
                string imgurl = "", disId = "", itemId = "", getItemName = "", getDistName = "";
               
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
                        comImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/C_" + filename));

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
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/C_" + filename), ImageFormat.Jpeg);
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

                        imgurl = "~/ProductImages" + "/C_" + filename;                        
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

                    int retsave = tcAll.Insert(ddlComplaintNature.SelectedValue, calendarTextBox.Text, ddlpartytypepersons.SelectedValue, itemId, BatchNo.Value,
                        TextArea1.Value, imgurl, Convert.ToInt32(Settings.Instance.UserID), newDate, docId, Convert.ToInt32(DdlSalesPerson.SelectedValue), "S");

                    tcAll.SetDociId(docId);

                 //   SenEmailTemplate(ddlComplaintNature.SelectedValue, ddlComplaintNature.SelectedItem.Text, calendarTextBox.Text, getDistName, getItemName, BatchNo.Value, TextArea1.Value, docId, newDate);

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
                string smName = "", getLoginSMName = "";
                string[] emailToAll = new string[20];
                string[] emailCCAll = new string[20];
                string emailNew = "";
                string emailCC = "";
                string smEmail = "";
                //              string smQry = @"select * from MastSalesRep where UserId=" + Settings.Instance.UserID + " and Active=1";
                //Ankita - 11/may/2016- (For Optimization)
                //string smQry = @"select * from MastSalesRep where SMId=" + DdlSalesPerson.SelectedValue + " and Active=1";
                string smQry = @"select SMName from MastSalesRep where SMId=" + DdlSalesPerson.SelectedValue + " and Active=1";
                DataTable smQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, smQry);
                if (smQryDt.Rows.Count > 0)
                {
                    getLoginSMName = smQryDt.Rows[0]["SMName"].ToString();

                }
                //Ankita - 12/may/2016- (For Optimization)
              //  string complNatQry = @"select * from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1 and NatureType='Complaint'";
                string complNatQry = @"select EmailTo,EmailCC from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1 and NatureType='Complaint'";
                DataTable complNatQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, complNatQry);
                if (complNatQryDt.Rows.Count > 0)
                {
                    emailNew = complNatQryDt.Rows[0]["EmailTo"].ToString();
                    string[] delimiters = new string[] { ",", ";" };
                    emailToAll = emailNew.Split(delimiters, StringSplitOptions.None);
                    emailCC = complNatQryDt.Rows[0]["EmailCC"].ToString();
                    emailCCAll = emailCC.Split(delimiters, StringSplitOptions.None);

                    SendEmail(ComplaintNature, manufactureDate, getLoginSMName, DistName, ItemName, BatchNo, Remark, docId, vDate, emailToAll, emailCCAll);
                    //if (emailToAll.Length > 0)
                    //{
                        
                    //    SendEmail(ComplaintNature, manufactureDate, getLoginSMName, DistName, ItemName, BatchNo, Remark, docId, vDate, emailToAll);
                    //}
                    //if (emailCCAll.Length > 0)
                    //{
                    //    SendEmail(ComplaintNature, manufactureDate, getLoginSMName, DistName, ItemName, BatchNo, Remark, docId, vDate, emailCCAll);
                    //}
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + ex.Message + "');", true);
            }
        }



        public void SendEmail(string ComplaintNature, string manufactureDate, string userName, string DistName, string ItemName, string BatchNo, string Remark, string docId, DateTime vDate, string[] emailTo, string[] emailCC)
        {
            try
            {
                //Added As Per UAT- On 11-Dec-2015
                string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.MailServer
                            FROM MastEnviro AS T1";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='Complaint'";
                    DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                    string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='Complaint'";
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
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{DocId}")
                                {
                                    strMailBody = strMailBody.Replace("{{DocId}}", docId);
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
                                if (DistName != "")
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{DistName}")
                                    {
                                        strMailBody = strMailBody.Replace("{{DistName}}", DistName);
                                    }
                                }
                                else
                                {
                                    strMailBody = strMailBody.Replace("Distributor Name -  {{DistName}}", string.Empty);
                                }
                                if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{Remark}")
                                {
                                    strMailBody = strMailBody.Replace("{{Remark}}", Remark);
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
                        for (int j = 0; j < emailCC.Length; j++)
                        {
                            mail.CC.Add(new MailAddress(emailCC[j]));
                        }
                    }

                    if (comImgFileUpload.HasFile)
                    {
                        string filename = Path.GetFileName(comImgFileUpload.FileName);
                        string attachmentPath = Server.MapPath("~/ProductImages" + "/C_" + filename);

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
          //  txtDist.Text = string.Empty;
            ddlpartytypepersons.SelectedIndex = 0;
            BatchNo.Value = string.Empty;
            calendarTextBox.Text = string.Empty;
            DdlSalesPerson.SelectedIndex = 0;
            TextArea1.Value = string.Empty;
            txtSearch.Text = string.Empty;
            ddlpartytype.SelectedIndex = 0;
            //Added
            ddldept.SelectedIndex = 0;
            //End
            // comImgFileUpload.Attributes.Clear();
            imgpreview.Style.Add("display", "none");
            btnDelete.Visible = false;
            btnSave.Text = "Save";
        }
        private void UpdateTransComplaint()
        {
            try
            {
                DateTime newDate = GetUTCTime();

                string getDistNameNew = string.Empty;
                string getItemNameNew = string.Empty;

                string imgurl = "";
                string itemId = "", disId = "";
              
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
                        comImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/C_" + filename));
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
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/C_" + filename), ImageFormat.Jpeg);
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
                        imgurl = "~/ProductImages" + "/C_" + filename;
                    }
                    else
                    {
                        imgurl = imgpreview.Src;
                    }
                    if (txtSearch.Text != string.Empty)
                    {
                        itemId = Request.Form[hfItemId.UniqueID];                       
                        string getItemQry = @"select ItemName from MastItem where ItemId=" + itemId + "";
                        DataTable itemNameQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, getItemQry);
                        if (itemNameQryDt.Rows.Count > 0)
                        {
                            getItemNameNew = itemNameQryDt.Rows[0]["ItemName"].ToString();
                        }
                       
                    }
                    //if (txtDist.Text != string.Empty)
                    //{
                    //    disId = Request.Form[hfDistId.UniqueID];
                    //    //Added as Per UAT - on 12-Dec-2015

                    //    string getdistQry = @"select PartyName from MastParty where PartyId=" + disId + " and Active=1 and PartyDist=1";
                    //    DataTable distNameQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, getdistQry);
                    //    if (distNameQryDt.Rows.Count > 0)
                    //    {
                    //        getDistNameNew = distNameQryDt.Rows[0]["PartyName"].ToString();
                    //    }
                    //    //End
                    //}
                    int retsave = tcAll.Update(Convert.ToInt64(ViewState["complId"]), ddlComplaintNature.SelectedValue, calendarTextBox.Text, ddlpartytypepersons.SelectedValue, itemId, BatchNo.Value, TextArea1.Value, imgurl, Convert.ToInt32(Settings.Instance.UserID), newDate, Convert.ToInt32(DdlSalesPerson.SelectedValue), "S");
                    if (retsave != 0)
                    {
                       System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);                    
                    // SenEmailTemplate(ddlComplaintNature.SelectedValue, ddlComplaintNature.SelectedItem.Text, calendarTextBox.Text, getDistNameNew, getItemNameNew, BatchNo.Value, TextArea1.Value, docIDHdf.Value, newDate);
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
            Response.Redirect("~/TransComplaint.aspx");
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

        protected void btnFind_Click(object sender, EventArgs e)
        {
            //    fillRepeter();
            rpt.DataSource = null;
            rpt.DataBind();
            //txtfmDate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            //txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");

            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End

            DropDownList1.SelectedIndex = 0;
            btnSave.Text = "Save";
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
        public static List<string> SearchDist(string prefixText, string contextKey)
        { //Ankita - 12/may/2016- (For Optimization)
            //string str = "select mp.*,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId in (select UnderId from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(contextKey) + ") and Active=1) ORDER BY PartyName";
            string str = "select mp.PartyName,mp.SyncId,mp.PartyId,ma.AreaName FROM MastParty  mp left join MastArea ma on mp.CityId=ma.AreaId where (mp.PartyName like '%" + prefixText + "%' or mp.SyncId like '%" + prefixText + "%' or ma.AreaName like '%" + prefixText + "%' ) and mp.PartyDist=1 and mp.Active=1 and mp.CityId in (select UnderId from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(contextKey) + ") and Active=1) ORDER BY PartyName";
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
            string smIDStr = "";
            string smIDStr1 = "", mainQry = "";
            if (dtSMId.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSMId.Rows)
                {
                    smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                    //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            }

            mainQry = " and Vdate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

            if (DropDownList1.SelectedIndex != 0)
            {
                mainQry = mainQry + " and ComplNatId=" + Convert.ToInt32(DropDownList1.SelectedValue) + "";
            }

            //Added as per UAT - 12-Dec-2015
            if (salesPersonDdl2.SelectedIndex!=0)
            {
                mainQry = mainQry + " and SMId=" + Convert.ToInt32(salesPersonDdl2.SelectedValue) + "";
            }
            else
            {
                mainQry = mainQry + " and SMId in (" + smIDStr1 + ")";
            }
            //End
            


            //if (txttodate.Text != string.Empty && txtfmDate.Text != string.Empty && DropDownList1.SelectedIndex != 0)
            //{
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            { //Ankita - 12/may/2016- (For Optimization)
//                string compltquery1 = @"select  r.ComplId, r.DocId, r.Vdate, r.UserId, r.ComplNatId, r.Category, r.ItemId, r.ImgUrl, 
//                                                Dist.PartyName, CNature.Name, Item.ItemName, r.Remark, r.BatchNo, r.ManufactureDate, r.DistId,r.SMId,msr.SMName from TransComplaint r
//                                                left join MastComplaintNature CNature on r.ComplNatId=CNature.Id
//                                                left join MastSalesRep msr on msr.SMId=r.SMId
//                                                left join MastItem Item on r.ItemId=Item.ItemId
//                                                left join MastParty Dist on r.DistId=Dist.PartyId where CNature.NatureType='Complaint' " + mainQry + " order by Vdate desc";
                string compltquery1 = @"select * from [View_Complaint] where NatureType='Complaint' " + mainQry + " order by Vdate desc";
                DataTable complaintdt2 = DbConnectionDAL.GetDataTable(CommandType.Text, compltquery1);
                if (complaintdt2.Rows.Count > 0)
                {
                    rpt.DataSource = complaintdt2;
                    rpt.DataBind();
                }
                else
                {
                    rpt.DataSource = null;
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

        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            //txtfmDate.Text = string.Empty;
            //txttodate.Text = string.Empty;

            //Added By - Abhishek 02/12/2015 UAT. Dated-08-12-2015
            txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
            txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
            //End

            DropDownList1.SelectedIndex = 0;
            rpt.DataSource = null;
            rpt.DataBind();
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

        //Added 14-Dec-2015
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {

                string str1 = @"SELECT T1.ItemId,T1.ItemName,T1.Unit,T1.MRP,T1.StdPack,T1.PriceGroup  
                                    FROM MastItem AS T1 WHERE T1.ItemId=" + Convert.ToInt32(hfItemId.Value) + "";
                DataTable obj1 = new DataTable();
                obj1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            }
        }
        //Added 15-Feb-2016
        private void BindPartyTypePersons()
        {
             DataTable dtptype = new DataTable(); string str = "";
                switch (ddlpartytype.SelectedItem.Text.ToUpper())
                {
                    case "RETAILER":
                    //    lblpartytypepersons.InnerText = "Retailer Name";
                        break;
                    case "PLUMBER":
                    //    lblpartytypepersons.InnerText = "Plumber Name";
                        break;
                    case "DISTRIBUTOR":
                    //    lblpartytypepersons.InnerText = "Distributor Name";
                        str = "1";
                        break;
                    case "ARCHITECT":
                     //   lblpartytypepersons.InnerText = "Architect Name";
                        break;
                    case "ELECTRICIAN":
                     //   lblpartytypepersons.InnerText = "Electrician Name";
                        break;
                    case "PROJECT":
                  //      lblpartytypepersons.InnerText = "Project Name";
                        break;
                    case "FARMER":
                     //   lblpartytypepersons.InnerText = "Farmer Name";
                        break;
                    //case default:
                    //    lblpartytypepersons.InnerText="Retailer Name";
                    //    break;
                }
                //Ankita - 12/may/2016- (For Optimization)
                string diststr = "";
                diststr = Getdistributorid(Convert.ToInt32(Settings.DMInt32(DdlSalesPerson.SelectedValue)));
                if (str == "1")
                    //str = "select mp.* FROM MastParty mp where mp.PartyDist=1 and mp.Active=1 and mp.CityId in (select UnderId from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(DdlSalesPerson.SelectedValue) + ") and Active=1) ORDER BY PartyName";                    
                    str = "select mp.PartyId,mp.PartyName FROM MastParty mp where mp.PartyDist=1 and mp.Active=1 and mp.Partyid in (" + diststr + ")  ORDER BY PartyName";    
            else
                    //str = "select mp.* FROM MastParty mp where mp.PartyDist=0 and mp.Active=1 and mp.partytype=" + ddlpartytype.SelectedValue + " and mp.AreaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(DdlSalesPerson.SelectedValue) + ") ORDER BY PartyName";
                    str = "select mp.PartyId,mp.PartyName FROM MastParty mp where mp.PartyDist=0 and mp.Active=1 and mp.partytype=" + ddlpartytype.SelectedValue + " and mp.AreaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(DdlSalesPerson.SelectedValue) + ") ORDER BY PartyName";
                    dtptype = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                ddlpartytypepersons.Items.Clear();
                if (dtptype.Rows.Count > 0)
                {
                    ddlpartytypepersons.DataSource = dtptype;
                    ddlpartytypepersons.DataTextField = "PartyName";
                    ddlpartytypepersons.DataValueField = "PartyId";
                    ddlpartytypepersons.DataBind();
                }
                ddlpartytypepersons.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        protected void ddlpartytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpartytype.SelectedIndex > 0)
            {
                BindPartyTypePersons();
            }
        }

        public string Getdistributorid(int smid)
        {
            string citystr = "";
            string diststr = "";
            string cityQry = @"  select AreaId from mastarea where areaid in (select distinct underid from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode in (SELECT smid FROM mastsalesrepgrp WHERE  maingrp IN (" + smid + "))) and Active=1 ) and areatype='City' and Active=1 order by AreaName";
            DataTable dtCity = DbConnectionDAL.GetDataTable(CommandType.Text, cityQry);
            for (int i = 0; i < dtCity.Rows.Count; i++)
            {
                citystr += dtCity.Rows[i]["AreaId"] + ",";
            }
            citystr = citystr.TrimStart(',').TrimEnd(',');
            string distqry = @"select PartyId,PartyName from MastParty where CityId in (" + citystr + ") and smid in ((select maingrp from mastsalesrepgrp where smid in (" + smid + ") union SELECT smid FROM mastsalesrepgrp WHERE  maingrp in (" + Settings.Instance.SMID + ")))  and PartyDist=1 and Active=1 order by PartyName";
            DataTable dtDist = DbConnectionDAL.GetDataTable(CommandType.Text, distqry);
            for (int i = 0; i < dtDist.Rows.Count; i++)
            {
                diststr += dtDist.Rows[i]["PartyId"] + ",";

            }
            diststr = diststr.TrimStart(',').TrimEnd(',');
            return diststr;

        }

        
    }
}