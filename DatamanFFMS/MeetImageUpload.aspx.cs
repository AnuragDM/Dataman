using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.IO;
using System.Data;
using DAL;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AstralFFMS
{
    public partial class MeetImageUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
               // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";

            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
               // btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            if (!IsPostBack)
            {
                fillUnderUsers();
                fillMeetType();
               // fillInitialRecords();
            }
        }

        private void fillMeetType()
        {//Ankita - 20/may/2016- (For Optimization)
            //string query = "select * from MastMeetType order by Name";
            string query = "select Id,Name from MastMeetType order by Name";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlmeetType.DataSource = dt;
                ddlmeetType.DataTextField = "Name";
                ddlmeetType.DataValueField = "Id";
                ddlmeetType.DataBind();
            }
            ddlmeetType.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        protected void ddlmeetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeetType.SelectedIndex > 0)
            {
                fillInitialRecords();
            }
            else
            {
                ddlmeet.Items.Clear();
                ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }
        //private void fillUnderUsers()
        //{
        //    DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
        //    if (d.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            DataView dv = new DataView(d);
        //            dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead'";
        //            ddlunderUser.DataSource = dv;
        //            ddlunderUser.DataTextField = "SMName";
        //            ddlunderUser.DataValueField = "SMId";
        //            ddlunderUser.DataBind();
        //        }
        //        catch { }

        //    }
        //}
        private void fillUnderUsers()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleType='AreaIncharge' OR RoleType='CityHead' OR RoleType='DistrictHead'";
                    ddlunderUser.DataSource = dv;
                    ddlunderUser.DataTextField = "SMName";
                    ddlunderUser.DataValueField = "SMId";
                    ddlunderUser.DataBind();
                    ddlunderUser.SelectedValue = Settings.Instance.SMID;
                }
                catch { }

            }
        }


        private void fillInitialRecords()
        {//Ankita - 20/may/2016- (For Optimization)
            ddlmeet.Items.Clear();
            //string str = @"select * from TransMeetPlanEntry   where AppStatus='Approved' and  SmId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId=" + ddlmeetType.SelectedValue + " order by MeetDate desc";
            //string str = @"select MeetPlanId,MeetName from TransMeetPlanEntry   where AppStatus='Approved' and  SmId=" + Settings.DMInt32(ddlunderUser.SelectedValue) + " and MeetTypeId=" + ddlmeetType.SelectedValue + " order by MeetDate desc";
            string str = @"select mp.MeetPlanId,mp.MeetName from TransMeetPlanEntry mp LEFT JOIN TransMeetExpense me  ON mp.MeetPlanId = me.MeetPlanId where mp.AppStatus='Approved' and mp.SMId=" + ddlunderUser.SelectedValue + " and mp.MeetTypeId=" + Settings.DMInt32(ddlmeetType.SelectedValue) + " AND me.FinalApprovedAmount IS null  order by mp.MeetDate desc";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                ddlmeet.DataSource = dt;
                ddlmeet.DataTextField = "MeetName";
                ddlmeet.DataValueField = "MeetPlanId";
                ddlmeet.DataBind();
            }
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlmeet.SelectedIndex > 0)
                {
                    string imgurl = "";
                   
                        if (FileUpload1.HasFile)
                        {
                            string directoryPath = Server.MapPath(string.Format("~/{0}/", "MeetImages"));
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            string filename = Path.GetFileName(FileUpload1.FileName);
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            filename = Settings.Instance.SMID + '-' + timeStamp + '-' + filename;
                            bool k = ValidateImageSize();
                            if (k != true)
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                                return;
                            }
                            // FileUpload1.SaveAs(Server.MapPath("~/MeetImages" + "/S_" + filename));   
                            FileUpload1.SaveAs(Server.MapPath("~/MeetImages" + "/S_" + filename)); //file saved with Smid + filename
                            //#region Image Compression Code
                            //FileUpload1.SaveAs(Server.MapPath("TempImages" + filename));
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
                            //bitMAP1.Save(Server.MapPath("~/MeetImages" + "/S_" + filename), ImageFormat.Jpeg);
                            //bitMAP1.Dispose();
                            //bitMAP1.Dispose();
                            //image.Dispose();

                            //// start image size validation after image compression > Priyanka 02/03/2016
                            //#region
                            //DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/MeetImages"));
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

                            imgurl = "~/MeetImages" + "/S_"+ filename;

                            string str = "insert into TransMeetImage([MeetPlanID],ImgUrl,ImgName) values('" + Settings.DMInt32(ddlmeet.SelectedValue) + "','" + imgurl + "','" + FileUpload1.FileName + "')";
                            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                            fillRepeater();
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record inserted Successfully');", true);
                        }
                    
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select the Image');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please select the Meet');", true);
                }

            }
            catch (Exception ex) { System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true); }

        }

        protected bool ValidateImageSize()
        {
            int fileSize = FileUpload1.PostedFile.ContentLength;
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
        private void fillRepeater()
        {
            string str = "select * from [TransMeetImage] where [MeetPlanID]="+Settings.DMInt32(ddlmeet.SelectedValue);
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if(dt.Rows.Count>0)
            {
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            else
            {
                rpt.DataSource =null;
                rpt.DataBind();
            }
        }

        protected void ddlmeet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeet.SelectedIndex > 0)
            {
               fillRepeater();
            }
        }

        protected void lnk_Click(object sender, EventArgs e)
        {

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                LinkButton btn = (LinkButton)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hdnVisItCode = (HiddenField)item.FindControl("HiddenField1");

            if (hdnVisItCode != null)
            {
                string str = "delete from TransMeetImage where Id=" + Settings.DMInt32(hdnVisItCode.Value);
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                fillRepeater();
            }

                
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            }
           
        }

        protected void ddlunderUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlmeetType.SelectedIndex = 0;
            ddlmeet.Items.Clear();
            ddlmeet.Items.Insert(0, new ListItem("-- Select --", "0"));
            rpt.DataSource = null;
            rpt.DataBind();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetImageUpload.aspx");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            fillRepeater();

        }


         //if (suggValueDt.Rows[0]["ImgUrl"] != string.Empty)
         //           {
         //               imgpreview.Src = suggValueDt.Rows[0]["ImgUrl"].ToString();
         //               imgpreview.Style.Add("display", "block");
         //           }
         //           else
         //           {
         //               imgpreview.Style.Add("display", "none");
         //           }
    }
}