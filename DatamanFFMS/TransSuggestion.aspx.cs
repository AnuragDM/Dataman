using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL;
using BusinessLayer;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Mime;

namespace AstralFFMS
{
    public partial class TransSuggestion : System.Web.UI.Page
    {
        //   UserPermission UP = new UserPermission();
        BAL.TransSuggestion.TSuggAll tsAll = new BAL.TransSuggestion.TSuggAll();
        string parameter = "";
        int msg = 0;
        int uid = 0;
        string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");            
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["sugglId"] = parameter;
                FillComplControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (!IsPostBack)
            {
                if (Session["user_name"] != null)
                {
                    //   complaintBy.Value = Session["user_name"].ToString();
                    BindComplaintNatureFilter();
                    BindPartyType();
                    ddlpartytypepersons.Items.Insert(0, new ListItem("-- Select --", "0"));
                    BindSalePersonDDl();
                    DdlSalesPerson.SelectedValue = Settings.Instance.SMID;
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
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
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
                //string partytypequery = @"select * from PartyType order by PartyTypeName";
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
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void BindComplaintNatureFilter()
        {
            try
            { //Ankita - 12/may/2016- (For Optimization)
                //string suggNaturequery = @"select * from MastComplaintNature where NatureType='Suggestion' and Active=1 order by Name";
                string suggNaturequery = @"select Id,Name from MastComplaintNature where NatureType='Suggestion' and Active=1 order by Name";
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
        
        private void FillComplControls(int suggId)
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
                    if (suggValueDt.Rows[0]["ItemId"] != DBNull.Value || suggValueDt.Rows[0]["ItemId"] != "0")
                    {
                        int itemId = Convert.ToInt32(suggValueDt.Rows[0]["ItemId"]);
                        hfItemId.Value = itemId.ToString();
                        string str = "select * FROM MastItem where ItemId=" + itemId + "";
                        DataTable dt = new DataTable();

                        dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                        txtSearch.Text = "(" + dt.Rows[0]["SyncId"].ToString() + ")" + " " + dt.Rows[0]["ItemName"].ToString() + " " + "(" + dt.Rows[0]["ItemCode"].ToString() + ")";
                    }
                    TextArea1.Value = suggValueDt.Rows[0]["NewApplicationArea"].ToString();
                    TextArea2.Value = suggValueDt.Rows[0]["TechnicalAdvantage"].ToString();
                    TextArea3.Value = suggValueDt.Rows[0]["MakeProductBetter"].ToString();
                    DdlSalesPerson.SelectedValue = suggValueDt.Rows[0]["SMId"].ToString();
                      BindPartyType();
                      if (suggValueDt.Rows[0]["DistId"] != DBNull.Value)
                      {
                          string SPtype = "select PartyDist from MastParty where partyId=" + suggValueDt.Rows[0]["DistId"].ToString() + "";
                          int partydist = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, SPtype));
                          if (partydist > 0)
                          {                              
                              //Distributor
                              //string qry = "select * from partytype where PartyTypeName='DISTRIBUTOR'";
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
                          else
                          {
                              //Others
                              string qry1 = "select PartyType from MastParty where partyId=" + suggValueDt.Rows[0]["DistId"].ToString() + "";
                              DataTable dtdist1 = new DataTable();
                              dtdist1 = DbConnectionDAL.GetDataTable(CommandType.Text, qry1);
                              if (dtdist1.Rows.Count > 0)
                              {
                                  ddlpartytype.SelectedValue = (dtdist1.Rows[0]["PartyType"].ToString());
                              }
                          }
                          BindPartyTypePersons();
                          ddlpartytypepersons.SelectedValue = suggValueDt.Rows[0]["DistId"].ToString();
                      }
                    if (suggValueDt.Rows[0]["ImgUrl"] != string.Empty)
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchItem(string prefixText)
        {//Ankita - 12/may/2016- (For Optimization)
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


        private static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

//        private void fillRepeter()
//        {

//            string suggquery = @"select r.SuggId, r.DocId, r.Vdate, r.UserId, r.ComplNatId, r.Category, r.ItemId,CNature.Name, 
//                              Item.ItemName, r.NewApplicationArea, r.TechnicalAdvantage, r.MakeProductBetter, r.BatchNo, r.ManufactureDate 
//                              from TransSuggestion r left join MastComplaintNature CNature on r.ComplNatId=CNature.Id
//                              left join MastItem Item on r.ItemId= Item.ItemId where r.UserId=" + Settings.Instance.UserID + " order by Vdate desc";
//            DataTable suggdt = DbConnectionDAL.GetDataTable(CommandType.Text, suggquery);
//            rpt.DataSource = suggdt;
//            rpt.DataBind();
//        }


        //private void GetUserID(string p)
        //{
        //    var query = from u in context.MastLogins
        //                where u.Name == p
        //                //    && p.Pwd == password
        //                select new { u.Id };

        //    if (query.Any())
        //    {
        //        uid = query.FirstOrDefault().Id;
        //    }
        //}

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
                }
                ddlComplaintNature.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
            catch
            {
            }
        }
        private void ClearControls()
        {
            ddlComplaintNature.SelectedIndex = 0;
            ddlpartytypepersons.SelectedIndex = 0;
            ddlpartytype.SelectedIndex = 0;
            TextArea1.Value = string.Empty;
            TextArea2.Value = string.Empty;
            TextArea3.Value = string.Empty;
            txtSearch.Text = string.Empty;
            //Added
            ddldept.SelectedIndex = 0;
            //End
            //     sugImgFileUpload.Attributes.Clear();
            DdlSalesPerson.SelectedIndex = 0;
            imgpreview.Style.Add("display", "none");
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            hfItemId.Value = string.Empty;
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //      fillRepeter();
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
                        sugImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/S_" + filename));

                        //#region Image Compression Code
                        //sugImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
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
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/S_" + filename), ImageFormat.Jpeg);
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
                        //    if (f.Name == "S_" + filename)
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
                        imgurl = "~/ProductImages" + "/S_" + filename;
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
                    int retsave = tsAll.Insert(newDate, docId, Convert.ToInt32(Settings.Instance.UserID), ddlComplaintNature.SelectedValue, hfItemId.Value, TextArea1.Value, TextArea2.Value, TextArea3.Value, imgurl, Convert.ToInt32(DdlSalesPerson.SelectedValue), Convert.ToInt32(ddlpartytypepersons.SelectedValue), "S");

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
                string smName = "", getLoginSMName = "";
                string[] emailToAll = new string[20];
                string[] emailCCAll = new string[20];
                string emailNew = "";
                string emailCC = "";
                string smEmail = "";
                //             string smQry = @"select * from MastSalesRep where UserId=" + Settings.Instance.UserID + " and Active=1";
                //Ankita - 12/may/2016- (For Optimization)
                //string smQry = @"select * from MastSalesRep where SMId=" + DdlSalesPerson.SelectedValue + " and Active=1";
                string smQry = @"select SMName from MastSalesRep where SMId=" + DdlSalesPerson.SelectedValue + " and Active=1";
                DataTable smQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, smQry);
                if (smQryDt.Rows.Count > 0)
                {
                    getLoginSMName = smQryDt.Rows[0]["SMName"].ToString();

                }

                //string complNatQry = @"select * from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1 and NatureType='Suggestion'";
                string complNatQry = @"select EmailTo,EmailCC from MastComplaintNature where Id=" + Convert.ToInt32(ComplNatId) + " and Active=1 and NatureType='Suggestion'";
                DataTable complNatQryDt = DbConnectionDAL.GetDataTable(CommandType.Text, complNatQry);
                if (complNatQryDt.Rows.Count > 0)
                {
                    emailNew = complNatQryDt.Rows[0]["EmailTo"].ToString();
                    string[] delimiters = new string[] { ",", ";" };
                    emailToAll = emailNew.Split(delimiters, StringSplitOptions.None);
                    emailCC = complNatQryDt.Rows[0]["EmailCC"].ToString();
                    emailCCAll = emailCC.Split(delimiters, StringSplitOptions.None);

                    SendEmail(ComplaintNature, getLoginSMName, getItemName, NewAppArea, TechAdv, productBetter, docId, vDate, emailToAll, emailCCAll);

                    //if (emailToAll.Length > 0)
                    //{
                    //    SendEmail(ComplaintNature, getLoginSMName, getItemName, NewAppArea, TechAdv, productBetter, docId, vDate, emailToAll);
                    //}
                    //if (emailCCAll.Length > 0)
                    //{
                    //    SendEmail(ComplaintNature, getLoginSMName, getItemName, NewAppArea, TechAdv, productBetter, docId, vDate, emailCCAll);
                    //}
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + ex.Message + "');", true);
            }
        }

        public void SendEmail(string ComplaintNature, string userName, string ItemName, string NewAppArea, string TechAdv, string productBetter, string docId, DateTime vDate, string[] emailTo,string[] emailCC)
        {
            try
            {
                //Added As Per UAT- On 11-Dec-2015
                string str = @"SELECT T1.SenderEmailId,T1.SenderPassword,T1.Port,T1.MailServer
                            FROM MastEnviro AS T1";

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                if(dt != null && dt.Rows.Count > 0)
                {
                    string strVar = @"SELECT * FROM MastEmailVariable WHERE EmailKey='Suggestion'";
                    DataTable dtVar = DbConnectionDAL.GetDataTable(CommandType.Text, strVar);
                    string strEmailTemplate = @"SELECT * FROM EmailTemplate WHERE EmialKey='Suggestion'";
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
                                //if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ItemName}")
                                //{
                                //    strMailBody = strMailBody.Replace("{{ItemName}}", ItemName);
                                //}
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
                                //if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{NewAppArea}")
                                //{
                                //    strMailBody = strMailBody.Replace("{{NewAppArea}}", NewAppArea);
                                //}
                                if(NewAppArea !="")
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{NewAppArea}")
                                    {
                                        strMailBody = strMailBody.Replace("{{NewAppArea}}", NewAppArea);
                                    }
                                    
                                }
                                else
                                {
                                    strMailBody = strMailBody.Replace("New Application Area -  {{NewAppArea}}", string.Empty);
                                }
                                //if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{TechAdv}")
                                //{
                                //    strMailBody = strMailBody.Replace("{{TechAdv}}", TechAdv);
                                //}
                                if (TechAdv != "")
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{TechAdv}")
                                    {
                                        strMailBody = strMailBody.Replace("{{TechAdv}}", TechAdv);
                                    }
                                    
                                }
                                else
                                {
                                    strMailBody = strMailBody.Replace("Technical Advantage - {{TechAdv}}", string.Empty);
                                }
                                //if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ProductBetter}")
                                //{
                                //    strMailBody = strMailBody.Replace("{{ProductBetter}}", productBetter);
                                //}
                                if (productBetter != "")
                                {
                                    if (Convert.ToString(dtVar.Rows[j]["Name"]) == "{ProductBetter}")
                                    {
                                        strMailBody = strMailBody.Replace("{{ProductBetter}}", productBetter);
                                    }
                                   
                                }
                                else
                                {
                                    strMailBody = strMailBody.Replace("How To Make Product Better - {{ProductBetter}}", string.Empty);
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
                            if (emailCC[j].Length.ToString() != "0")
                            {
                                mail.CC.Add(new MailAddress(emailCC[j]));
                            }
                           
                        }
                    }


                    //string[] toEmail = emailTo;
                    //foreach (var m in toEmail)
                    //{
                    //    mail.CC.Add(m);
                    //}
                    //if (sugImgFileUpload.HasFile)
                    //{
                    //    mail.Attachments.Add(new Attachment(sugImgFileUpload.FileContent, System.IO.Path.GetFileName(sugImgFileUpload.FileName)));
                    //}
                    if (sugImgFileUpload.HasFile)
                    {
                        string filename = Path.GetFileName(sugImgFileUpload.FileName);
                        string attachmentPath = Server.MapPath("~/ProductImages" + "/S_" + filename);
                       
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
                //End


                StringBuilder strBody = new StringBuilder();
           
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

                string getItemNameNew = string.Empty;

                string imgurl = "";
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
                        sugImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/S_" + filename));

                        //#region Image Compression Code
                        //sugImgFileUpload.SaveAs(Server.MapPath("TempImages" + filename));
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
                        //bitMAP1.Save(Server.MapPath("~/ProductImages" + "/S_" + filename), ImageFormat.Jpeg);
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
                        //    if (f.Name == "S_" + filename)
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


                        imgurl = "~/ProductImages" + "/S_" + filename;
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
                    int retsave = tsAll.Update(Convert.ToInt64(ViewState["sugglId"]), newDate, Convert.ToInt32(Settings.Instance.UserID), ddlComplaintNature.SelectedValue, itemId, TextArea1.Value, TextArea2.Value, TextArea3.Value, imgurl, Convert.ToInt32(DdlSalesPerson.SelectedValue), Convert.ToInt32(ddlpartytypepersons.SelectedValue), "S");
                    if (retsave != 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);

                        //Added As per UAT - 12-Dec-2015
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
            Response.Redirect("~/TransSuggestion.aspx");
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
            DataTable dtSMId = Settings.UnderUsers(Settings.Instance.SMID);
            string smIDStr = "";
            string smIDStr1 = "", mastQry = "";
            if (dtSMId.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSMId.Rows)
                {
                    smIDStr = smIDStr + "," + Convert.ToString(dr["SMId"]);
                    //        smIDStr +=string.Join(",",dtSMId.Rows[i]["SMId"].ToString());
                }
                smIDStr1 = smIDStr.TrimStart(',').TrimEnd(',');
            }

            mastQry = " and Vdate between '" + Settings.dateformat(txtfmDate.Text) + " 00:00' and '" + Settings.dateformat(txttodate.Text) + " 23:59'";

            if (DropDownList1.SelectedIndex != 0)
            {
                mastQry = mastQry + " and ComplNatId=" + Convert.ToInt32(DropDownList1.SelectedValue) + "";
            }

            //Added as per UAT - on 12-Dec-2015
            if (salesPersonDdl2.SelectedIndex != 0)
            {
                mastQry = mastQry + " and SMId=" + Convert.ToInt32(salesPersonDdl2.SelectedValue) + "";
            }
            else
            {
                mastQry = mastQry + " and SMId in (" + smIDStr1 + ")";
            }
            //End

           
            if (Convert.ToDateTime(txttodate.Text) >= Convert.ToDateTime(txtfmDate.Text))
            { //Ankita - 12/may/2016- (For Optimization)
//                string suggquery1 = @"select r.SuggId, r.DocId, r.Vdate, r.UserId, r.ComplNatId, r.Category, r.ItemId,CNature.Name, 
//                                              Item.ItemName, r.NewApplicationArea, r.TechnicalAdvantage, r.MakeProductBetter, r.BatchNo,       r.ManufactureDate,msr.SMName 
//                                              from TransSuggestion r left join MastComplaintNature CNature on r.ComplNatId=CNature.Id
//                                              left join MastSalesRep msr on msr.SMId=r.SMId
//                                              left join MastItem Item on r.ItemId= Item.ItemId where CNature.NatureType='Suggestion' " + mastQry + " order by r.Vdate desc";
                string suggquery1 = @"select * from [View_Suggestion] where NatureType='Suggestion' " + mastQry + " order by Vdate desc";
                DataTable suggdt2 = DbConnectionDAL.GetDataTable(CommandType.Text, suggquery1);
                if (suggdt2.Rows.Count > 0)
                {
                    rpt.DataSource = suggdt2;
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
        //Added 15-Feb-2016
        private void BindPartyTypePersons()
        {
            DataTable dtptype = new DataTable(); string str = "";
            switch (ddlpartytype.SelectedItem.Text.ToUpper())
            {
                case "RETAILER":
              //      lblpartytypepersons.InnerText = "Retailer Name";
                    break;
                case "PLUMBER":
                  //  lblpartytypepersons.InnerText = "Plumber Name";
                    break;
                case "DISTRIBUTOR":
                 //   lblpartytypepersons.InnerText = "Distributor Name";
                    str = "1";
                    break;
                case "ARCHITECT":
                //    lblpartytypepersons.InnerText = "Architect Name";
                    break;
                case "ELECTRICIAN":
                 //   lblpartytypepersons.InnerText = "Electrician Name";
                    break;
                case "PROJECT":
               //     lblpartytypepersons.InnerText = "Project Name";
                    break;
                case "FARMER":
               //     lblpartytypepersons.InnerText = "Farmer Name";
                    break;
                //case default:
                //    lblpartytypepersons.InnerText="Retailer Name";
                //    break;
            }
            //Ankita - 12/may/2016- (For Optimization)
            string diststr = "";
            diststr = Getdistributorid(Convert.ToInt32(Settings.DMInt32(DdlSalesPerson.SelectedValue)));
            if (str == "1")
                str = "select mp.PartyId,mp.PartyName FROM MastParty mp where mp.PartyDist=1 and mp.Active=1 and mp.Partyid in (" + diststr + ")  ORDER BY PartyName";    
                //str = "select mp.* FROM MastParty mp where mp.PartyDist=1 and mp.Active=1 and mp.CityId in (select UnderId from mastarea where areaid in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(DdlSalesPerson.SelectedValue) + ") and Active=1) ORDER BY PartyName";
            else
                str = "select mp.PartyId,mp.PartyName FROM MastParty mp where mp.PartyDist=0 and mp.Active=1 and mp.partytype=" + ddlpartytype.SelectedValue + " and mp.AreaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(DdlSalesPerson.SelectedValue) + ") ORDER BY PartyName";
                //str = "select mp.* FROM MastParty mp where mp.PartyDist=0 and mp.Active=1 and mp.partytype=" + ddlpartytype.SelectedValue + " and mp.AreaId in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.DMInt32(DdlSalesPerson.SelectedValue) + ") ORDER BY PartyName";
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