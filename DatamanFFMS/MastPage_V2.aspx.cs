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
using Newtonsoft.Json;
using System.Web.Services;
using System.Text;
using System.Web.Script.Services;
using System.Data.SqlClient;
using BAL.LeaveRequest;
using System.Net;
using System.Web.Script.Serialization;

namespace AstralFFMS
{
    public partial class MastPage_V2 : System.Web.UI.Page
    {
        BAL.MastPage.MastPageBAL mp = new BAL.MastPage.MastPageBAL();
        string parameter = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "" && parameter != null)
            {
                ViewState["PageId"] = parameter;
                FillDesigControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            if (parameter == "" || parameter == null)
            {
                //  divactive.Style.Add("display", "none");
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }

            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');

            //btnExport.CssClass = "btn btn-primary";
            //btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            //_exportp = Convert.ToBoolean(SplitPerm[4]);

            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            // btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            //if (Request.QueryString["PartyId"] != null)
            //{
            //    PartyId = Request.QueryString["PartyId"].ToString();
            //    oldappstatus = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AppStatus FROM MastParty where PartyID=" + PartyId  + ""));
            //}

            //if (Request.QueryString["VisitID"] != null)
            //{
            //    VisitID = Request.QueryString["VisitID"].ToString();
            //}
            //if (Request.QueryString["CityID"] != null)
            //{
            //    CityID = Request.QueryString["CityID"].ToString();
            //}
            //if (Request.QueryString["AreaId"] != null)
            //{
            //    AreaId = Convert.ToInt32(Request.QueryString["AreaId"].ToString());
            //}
            //if (Request.QueryString["Level"] != null)
            //{
            //    Level = Request.QueryString["Level"].ToString();
            //}
            if (ViewState["PageId"] != null)
            {
                string s = ViewState["PageId"].ToString();
            }
            if (!IsPostBack)
            {
                //  BindState();
                //List<Party> partydetails = new List<Party>();
                //partydetails.Add(new Party());
                //rpt12.DataSource = partydetails;
                //rpt12.DataBind();
                //txtDOA.Attributes.Add("readonly", "readonly");
                //txtDOB.Attributes.Add("readonly", "readonly");
                //Button1.Visible = false;
                //if (RetailerCreationApproval == true)
                //{
                //    chkIsAdmin.Checked = false;
                //}
                //else
                //{
                //    chkIsAdmin.Checked = true;
                //}
                //divblockchk.Style.Add("display", "none");
                btnDelete.Visible = false;
                // BindPartyType(); 
                // BindState();

                getidx();
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["PageId"] != null)
                {
                    ViewState["PageId"] = Request.QueryString["PageId"];
                    //FillPartyControls(Convert.ToInt32(Request.QueryString["PageId"]));

                }
                else
                {
                    //  BindCity(0); BindIndustry(0); BindDistributors(0);
                }
            }
            if (Request.QueryString["REQ"] != null)
            {
                ViewState["PageId"] = Request.QueryString["PageId"];
                //REQ = Request.QueryString["REQ"].ToString();
                //if (REQ == "PRA")
                //{
                //    TextBox1.Enabled = false;
                //    approveStatusRadioButtonList.Enabled = false;
                //}
                //conditonaldiv.Style.Add("display", "block");
                //HiddenPartyId.Value = Convert.ToString(ViewState["PartyId"]);
                //// divactive.Style.Add("display", "block");
                //btnFind.Visible = false;
                //Button1.Visible = false;
                //divmaploc.Style.Add("display", "block");

            }
        }

        private void FillDesigControls(int PageId)
        {
            try
            {
                string Query = @"select * from MastPage where pageid=" + PageId;

                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);

                if (dt.Rows.Count > 0)
                {
                    pgenme.Value = dt.Rows[0]["PageName"].ToString();
                    pgenme.Attributes.Add("readonly", "readonly");

                    dispnme.Value = dt.Rows[0]["DisplayName"].ToString();
                    if (Convert.ToBoolean(dt.Rows[0]["DisplayYN"]) == true)
                    {
                        chkIsDisplay.Checked = true;
                    }
                    else
                    {
                        chkIsDisplay.Checked = false;
                    }
                    prntId.Value = dt.Rows[0]["Parent_Id"].ToString();
                    ddlmodule.SelectedValue = dt.Rows[0]["Module"].ToString() + "-" + dt.Rows[0]["Parent_Id"].ToString();
                    HiddeModuleID.Value = dt.Rows[0]["Parent_Id"].ToString();
                    HiddenModuleName.Value = dt.Rows[0]["Module"].ToString() + "-" + dt.Rows[0]["Parent_Id"].ToString();
                    ddltype.SelectedValue = dt.Rows[0]["Level_Idx"].ToString();
                    idxtxt.Text = dt.Rows[0]["Idx"].ToString();

                    if (dt.Rows[0]["Android"].ToString() == "Y")
                    { chkIsAndroid.Checked = true; }
                    else { chkIsAndroid.Checked = false; }

                    androidform.Value = dt.Rows[0]["Android_Form"].ToString();
                    menuicon.Value = dt.Rows[0]["MenuIcon"].ToString();
                    txtapp.Value = dt.Rows[0]["App"].ToString();

                    if (dt.Rows[0]["Img"].ToString() != string.Empty && dt.Rows[0]["Img"].ToString().Trim() != "")
                    {
                        imgpreview.Src = dt.Rows[0]["Img"].ToString();
                        imgpreview.Style.Add("display", "block");
                    }
                    else
                    {
                        imgpreview.Style.Add("display", "none");
                    }

                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string checkUserName(string IDVal)
        {
            string result = string.Empty;
            //Get your connection string here
            //string conString = System.Configuration.ConfigurationManager.ConnectionStrings["AdventureWorks2008R2ConnectionString2"].ConnectionString;
            //Change your query here
            string qry = "Select parent_id from mastpage Where parent_id =" + IDVal + "";
            //SqlDataAdapter da = new SqlDataAdapter(qry, conString);
            //Pass the value to paramter
            //da.SelectCommand.Parameters.AddWithValue("@AddressID", IDVal.Trim());
            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            //da.Fill(ds, "IDTable");
            //Check if dataset is having any value
            if (dtItem.Rows.Count > 0)
            {
                // User Name Not Available
                result = "ID already in use";
            }
            else
            {
                //User_Name is available
                result = "ID is available, you can use it";
            }
            //Return the result
            return result;
        }

        private void fillRepeter()
        {
            string str = @"select PageId,DisplayName,Module,PageName,CASE WHEN DisplayYN = 1 THEN 'Yes' ELSE 'No'  END as Display,CASE WHEN Android = 'Y' THEN 'Yes' ELSE 'No'  END as Android,Level_Idx,Idx  from MastPage";
            DataTable depdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = depdt;
            rpt.DataBind();

            depdt.Dispose();
        }

        private void getidx()
        {
            string qry = "Select max(Idx) as Idx from mastpage Where Idx Is Not Null";
            DataTable dtItem = DbConnectionDAL.GetDataTable(CommandType.Text, qry);

            int s = Convert.ToInt32(dtItem.Rows[0]["Idx"].ToString()) + 1;
            idxtxt.Text = s.ToString();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            pgenme.Value = string.Empty;
            ddlmodule.SelectedIndex = 0;
            dispnme.Value = string.Empty;
            prntId.Value = string.Empty;
            ddltype.SelectedIndex = 0;
            idxtxt.Text = string.Empty;

            androidform.Value = string.Empty;
            menuicon.Value = string.Empty;
            txtapp.Value = string.Empty;

            //HiddenGradeUnderID.Value = string.Empty;
            // ddlgrade.SelectedIndex = 0;
            chkIsDisplay.Checked = false;
            chkIsAndroid.Checked = true;
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
            getidx();
            chkIsAndroid.Checked = false;
            chkIsDisplay.Checked = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdatePage();
                }
                else
                {
                    InsertPage();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        protected bool ValidateImageSize(long contentlength)
        {
            long fileSize = contentlength;
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

        protected bool ValidateImageSize()
        {
            int fileSize = partyImgFileUpload.PostedFile.ContentLength;
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

        private void InsertPage()
        {
            string android = ""; string Imgurl = "";
            string ModuleID = Request.Form[HiddeModuleID.UniqueID];
            string ModuleN = Request.Form[HiddenModuleName.UniqueID];

            string[] authorsList = ModuleN.Split('-');

            string[] imgurls = new string[5];
            string imgurl = "";

            if (chkIsAndroid.Checked == true)
            {
                android = "Y";
            }
            else
            {
                android = "N";
            }

            if (partyImgFileUpload.HasFile)
            {
                string directoryPath = Server.MapPath(string.Format("~/{0}/", "PageImage"));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filename = Path.GetFileName(partyImgFileUpload.FileName);
                bool k = ValidateImageSize();
                if (k != true)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                    return;
                }
                partyImgFileUpload.SaveAs(Server.MapPath("~/PageImage" + "/Page_" + filename));
                Imgurl = "~/PageImage" + "/Page_" + filename;
            }
            else
            {
                if (!string.IsNullOrEmpty(imgpreview.Src.ToString()))
                    Imgurl = imgpreview.Src.ToString();
            }

            string str = @"select Count(*) from MastPage where PageName='" + pgenme.Value + "'";
            int val = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

            if (val == 0)
            {
                int val1 = 0;
                if (prntId.Value != "" || prntId.Value != string.Empty)
                {
                    string str1 = @"select Count(*) from MastPage where Parent_Id='" + prntId.Value + "'";
                    val1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
                }
                if (val1 > 0)
                {
                    int retsave = mp.Insert(pgenme.Value, authorsList[0], dispnme.Value, chkIsDisplay.Checked, Imgurl, Convert.ToInt32(prntId.Value), Convert.ToInt32(ddltype.SelectedValue), Convert.ToInt32(idxtxt.Text), android, androidform.Value, menuicon.Value, txtapp.Value);

                    if (retsave != 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);

                        pgenme.Value = string.Empty;
                        ddlmodule.SelectedIndex = 0;
                        HiddeModuleID.Value = string.Empty;
                        HiddenModuleName.Value = string.Empty;
                        dispnme.Value = string.Empty;
                        chkIsDisplay.Checked = true;
                        chkIsAndroid.Checked = false;
                        prntId.Value = string.Empty;
                        androidform.Value = string.Empty;
                        menuicon.Value = string.Empty;
                        txtapp.Value = string.Empty;
                        btnDelete.Visible = false;
                        ddltype.SelectedIndex = 0;
                        pgenme.Focus();
                        getidx();
                        imgpreview.Style.Add("display", "none");
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Parent Id Not Exists');", true);
                    prntId.Focus();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exist');", true);
                pgenme.Focus();
            }
        }

        private void UpdatePage()
        {
            string android = ""; string Imgurl = "";
            string ModuleID = Request.Form[HiddeModuleID.UniqueID];
            string ModuleN = Request.Form[HiddenModuleName.UniqueID];

            string[] authorsList = ModuleN.Split('-');

            string[] imgurls = new string[5];
            string imgurl = "";

            if (chkIsAndroid.Checked == true)
            {
                android = "Y";
            }
            else
            {
                android = "N";
            }

            if (partyImgFileUpload.HasFile)
            {
                string directoryPath = Server.MapPath(string.Format("~/{0}/", "PageImage"));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filename = Path.GetFileName(partyImgFileUpload.FileName);
                bool k = ValidateImageSize();
                if (k != true)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                    return;
                }
                partyImgFileUpload.SaveAs(Server.MapPath("~/PageImage" + "/Page_" + filename));
                Imgurl = "~/PageImage" + "/Page_" + filename;
            }
            else
            {
                if (!string.IsNullOrEmpty(imgpreview.Src.ToString()))
                    Imgurl = imgpreview.Src.ToString();
            }

            string strupd = @"select Count(*) from MastPage where PageName='" + pgenme.Value + "' and PageId<>" + Convert.ToInt32(ViewState["PageId"]) + "";

            int valupd = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd));

            if (valupd == 0)
            {
                int retsave = mp.Update(ViewState["PageId"].ToString(), pgenme.Value, authorsList[0], dispnme.Value, chkIsDisplay.Checked, Imgurl, Convert.ToInt32(prntId.Value), Convert.ToInt32(ddltype.SelectedValue), Convert.ToInt32(idxtxt.Text), android, androidform.Value, menuicon.Value, txtapp.Value);

                //string PageId, string PageName, string Module, string DisplayName, bool DisplayYN, string Img, int Parent_Id, int Level_Idx, int Idx, string Android, string Android_Form, string MenuIcon, string App

                if (retsave >0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                    pgenme.Value = string.Empty;
                    ddlmodule.SelectedIndex = 0;
                    HiddeModuleID.Value = string.Empty;
                    HiddenModuleName.Value = string.Empty;
                    hidimg.Value = string.Empty;
                    dispnme.Value = string.Empty;
                    chkIsDisplay.Checked = true;
                    chkIsAndroid.Checked = false;
                    prntId.Value = string.Empty;
                    androidform.Value = string.Empty;
                    menuicon.Value = string.Empty;
                    txtapp.Value = string.Empty;
                    btnDelete.Visible = false;
                    ddltype.SelectedIndex = 0;
                    pgenme.Focus();
                    getidx();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    pgenme.Attributes.Remove("readonly");
                    imgpreview.Style.Add("display", "none");
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Something went wrong');", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exists');", true);
                pgenme.Focus();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int valupd2 = 0;
                string strupd1 = @"select COUNT(*) FROM MastPage where PageId=" + Convert.ToInt32(ViewState["PageId"]) + "";

                valupd2 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strupd1));
                if (valupd2 == 1)
                {
                    //     this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                    int retdel = mp.delete(Convert.ToString(ViewState["PageId"]));
                    if (retdel == 1)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                        pgenme.Value = string.Empty;
                        ddlmodule.SelectedIndex = 0;
                        HiddeModuleID.Value = string.Empty;
                        HiddenModuleName.Value = string.Empty;
                        hidimg.Value = string.Empty;
                        dispnme.Value = string.Empty;
                        chkIsDisplay.Checked = true;
                        chkIsAndroid.Checked = false;
                        prntId.Value = string.Empty;
                        androidform.Value = string.Empty;
                        menuicon.Value = string.Empty;
                        txtapp.Value = string.Empty;
                        btnDelete.Visible = false;
                        ddltype.SelectedIndex = 0;
                        pgenme.Focus();
                        getidx();
                        btnDelete.Visible = false;
                        btnSave.Text = "Save";
                        pgenme.Attributes.Remove("readonly");
                        imgpreview.Style.Add("display", "none");
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('SomeThing wen wrong');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Not Exist');", true);
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You click no');", true);
            }
        }
    }
}