using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BusinessLayer;
using System.Web.Configuration;

namespace AstralFFMS
{
    public partial class FFMS : System.Web.UI.MasterPage
    {
         string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            getCss();
            if (!Page.IsPostBack)
            {
               
                //lblTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss tt");//String.Format("{0:F}", DateTime.Now);
                //    Session["user_name"] = "Sa";
                if (Session["user_name"] != null)
                {
                    LoadSideBar();
                    Label4.DataBind();
                    //Ankita - 10/may/2016- (For Optimization)
                   // roleType= GetRoleType(Settings.Instance.RoleID);
                    roleType = Settings.Instance.RoleType;
                    if (roleType == "Distributor")
                    { //userNameLabel.Text = Settings.Instance.LoggedDistName; 
                        Label4.Text = greeting() + ' ' + Settings.Instance.LoggedDistName; lidist.Visible = false;
                        if (Session["ShowGreeting"] != null)
                        {
                            //ModalPopupExtender1.Show();
                            //lblmszgm.Text = greeting()+' ' + Settings.Instance.LoggedDistName;
                            //Session["ShowGreeting"] = null;

                        }
                    }//

                    else
                    { //userNameLabel.Text = Settings.Instance.SmLogInName;
                        Label4.Text = greeting() + ' ' + Settings.Instance.EmpName; lidist.Visible = true;
                        if (Session["ShowGreeting"] != null)
                        {
                            //ModalPopupExtender1.Show();
                            //lblmszgm.Text = greeting() + ' ' + Settings.Instance.EmpName;
                            //Session["ShowGreeting"] = null;
                        }
                    }//
                }
                else
                {
                    Response.Redirect("~/Loginn.aspx", true);
                }
            }

            string imgurl = "";            
                imgurl = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select SpImagepath From mastsalesrep where userid='" + Settings.Instance.UserID + "'"));
                if (string.IsNullOrEmpty(imgurl))
                imgurl = "SalespersonImages/defaultspimg.png";
           
            // ImageMasterPage.ImageUrl = WebConfigurationManager.AppSettings["CompanyFolderMasterPage"].ToString();
            ImageMasterPage.ImageUrl = imgurl;
           // Label1.Text = WebConfigurationManager.AppSettings["CompanyFolder1"].ToString();
            logoimg.ImageUrl = WebConfigurationManager.AppSettings["CompanyFolder"].ToString();
            Image1.ImageUrl = WebConfigurationManager.AppSettings["CompanyFolderMasterPage"].ToString();
            CheckNotifications();
        }



        private void getCss()
        {
            //throw new NotImplementedException();
            String qry = "select ID from theme where isActive=1";
            string bodyCls = "";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {
                string z = Convert.ToString(dt.Rows[0]["ID"]);

                switch (z)
                {
                    case "1":
                        bodyCls = "skin-blue sidebar-mini sidebar-collapse";
                        break;
                    case "2":
                        bodyCls = "skin-blue-light sidebar-mini sidebar-collapse";
                        break;
                    case "3":
                        bodyCls = "skin-red sidebar-mini sidebar-collapse";
                        break;
                    case "4":
                        bodyCls = "skin-red-light sidebar-mini sidebar-collapse";
                        break;
                    case "5":
                        bodyCls = "skin-yellow sidebar-mini sidebar-collapse";
                        break;
                    case "6":
                        bodyCls = "skin-yellow-light sidebar-mini sidebar-collapse";
                        break;
                    case "7":
                        bodyCls = "skin-green sidebar-mini sidebar-collapse";
                        break;
                    case "8":
                        bodyCls = "skin-green-light sidebar-mini sidebar-collapse";
                        break;
                    case "9":
                        bodyCls = "skin-purple sidebar-mini sidebar-collapse";
                        break;
                    case "10":
                        bodyCls = "skin-purple-light sidebar-mini sidebar-collapse";
                        break;
                    case "11":
                        bodyCls = "skin-black sidebar-mini sidebar-collapse";
                        break;
                    case "12":
                        bodyCls = "skin-black-light sidebar-mini sidebar-collapse";
                        break;
                }

            }
            myHidden.Value = bodyCls;
            //return bodyCls;
        }




        //private string GetRoleType(string p)
        //{
        //    try
        //    {
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);               
        //        return roledt.Rows[0]["RoleType"].ToString();              
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //        return string.Empty;
        //    }
        //}

        public void CheckNotifications()
        {
            try
            {
                //Ankita - 02/may/2016- (For Optimization)

    // string query = @"select top 5 trnsnot.NotiId, trnsnot.displayTitle, trnsnot.FromUserId, trnsnot.msgURL, trnsnot.pro_id, trnsnot.Status, trnsnot.userid,trnsnot.VDate,convert(date, trnsnot.VDate) as V1Date, convert(varchar(8), convert(time, trnsnot.VDate),100) as V1Time, salesrep.SMName, salesrep.SMId from TransNotification trnsnot
//                                left join MastSalesRep salesrep on trnsnot.FromUserId=salesrep.UserId
//                                Where trnsnot.userid =" + Convert.ToInt32(Settings.Instance.UserID) + " and Status=0 order by trnsnot.NotiId desc";
                string query = @"select top(5)* from Vw_Notification
                                Where Vw_Notification.userid =" + Convert.ToInt32(Settings.Instance.UserID) + " order by NotiId desc";

                DataTable transNotDt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                


               

                //string queryCount = @"select * from TransNotification where userid=" + Convert.ToInt32(Settings.Instance.UserID) + " and Status=0 ";
                //DataTable transNotCountDt = DbConnectionDAL.GetDataTable(CommandType.Text, queryCount);
                //int count = 0;
                //if (transNotCountDt.Rows.Count > 0)
                //{
                //    count = Convert.ToInt32(transNotCountDt.Rows.Count);
                //}



                string queryCount = @"select count(*) as norec from TransNotification where userid=" + Convert.ToInt32(Settings.Instance.UserID) + " and Status=0 ";
                int count =Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, queryCount));
                msgcountLabel.Text = count.ToString();
                notifications.Text = "You have " + msgcountLabel.Text + " unread notifications ";
                Repeater1.DataSource = transNotDt;
                Repeater1.DataBind();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public string setClass(int Status)
        {
            string classToApply = string.Empty;
            if (Status == 0)
            {
                classToApply = "messagelabel";
            }
            else
            {
                classToApply = "messagelabel1";
            }

            return classToApply;

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {

            CheckNotifications();
        }

        //for current time abhishek jaiswal

        //protected void GetTime(object sender, EventArgs e)
        //{
        //    lblTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy") +' '+ DateTime.Now.ToString("hh:mm:ss tt");//String.Format("{0:F}", DateTime.Now);
        //        //DateTime.Now.ToString("hh:mm:ss tt");
        //}

        // Added by Gaurav Shukla    13-Jul-2015
        private void LoadSideBar()
        {
            string str1 = "";
            //Ankita - 02/may/2016- (For Optimization)

            //string str1 = "select * from MastLogin M where  M.Name = '" + Convert.ToString(Session["user_name"]) + "'";
            // DataTable RollDT = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            // int rolID = Convert.ToInt32(RollDT.Rows[0]["RoleId"].ToString());
            string name = Settings.Instance.RoleType;
            string str = "select PageId,DisplayName,[Parent_Id],MenuIcon from MastPage where DisplayYN = 1 and android='N' and  Parent_Id=0 order by Parent_Id,Level_Idx,Idx asc";
            if (name.Equals("Tracker"))
            {
                str1 = string.Format("SELECT [MastPage].[PageId],[PageName],[Module],[DisplayName],[DisplayYN],[Img] ,[Parent_Id],[Level_Idx],[Idx] \n" +
                                   "FROM [MastPage] where  android='N' and [PageId] In (Select Page_Id from StorePermission Where viewP=1  And UserId={0} )", Settings.DMInt32(Settings.Instance.UserID));
            }
            else
            {

                str1 =
                        @"SELECT [MastPage].[PageId],[PageName],[Module],[DisplayName],[DisplayYN],[Img] ,[Parent_Id],[Level_Idx],[Idx] FROM [MastPage]
                         inner join MastRolePermission on MastPage.[PageId]=MastRolePermission.[PageId] where DisplayYN = 1 and
                         Parent_Id >-1 and RoleId='" + Settings.Instance.RoleID + "' and viewP=1 order by Parent_Id,Level_Idx,Idx asc";
                //str1=string.Format("select * from (\n" +
                //                                   "select * from (\n" +
                //                                   "SELECT [MastPage].[PageId],[PageName],[Module],[DisplayName],[DisplayYN],[Img] ,[Parent_Id],[Level_Idx],[Idx],RoleId,viewP \n"+
                //                                  "FROM [MastPage] \n"+
                //                                   "inner join MastRolePermission on MastPage.[PageId]=MastRolePermission.[PageId] \n"+
                //                                   "where DisplayYN = 1  and android='N' and Parent_Id >-1 and RoleId ='{0}' and viewP=1 and Module<>'Tracker' ) as a \n" +
                //                                   "union \n"+ 
                //                                   "SELECT [MastPage].[PageId],[PageName],[Module],[DisplayName],[DisplayYN],[Img] ,[Parent_Id],[Level_Idx],[Idx],'' as RoleId,'1' as viewP \n"+ 
                //                                   "FROM [MastPage] \n"+
                //                                   "where  android='N' and [PageId] In (Select Page_Id from StorePermission Where  UserId={1} And ViewP=1 )) as s  order by Parent_Id,Level_Idx,Idx asc", Settings.Instance.RoleID, Settings.DMInt32(Settings.Instance.UserID));

            }
            DataTable ParentDT = new DataTable();
            DataTable DtAll = new DataTable();
            ParentDT = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            DtAll = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            if (ParentDT.Rows.Count > 0)
            {

                StringBuilder strMenu = new StringBuilder();
                strMenu.AppendLine();
                for (int i = 0; i < ParentDT.Rows.Count; i++)
                {
                    //                    str = @"SELECT [MastPage].[PageId],[PageName],[Module],[DisplayName],[DisplayYN],[Img] ,[Parent_Id],[Level_Idx],[Idx] FROM [MastPage]
                    //                         inner join MastRolePermission on MastPage.[PageId]=MastRolePermission.[PageId] where DisplayYN = 1 and  
                    //                         Parent_Id>-1 and RoleId='" + Settings.Instance.RoleID + "' and Parent_Id='" + ParentDT.Rows[i]["PageId"].ToString() + "' and viewP=1 order by Parent_Id,Level_Idx,Idx asc";

                    DataTable PagesDT = new DataTable();
                    //PagesDT = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    DtAll.DefaultView.RowFilter = "Parent_Id='" + ParentDT.Rows[i]["PageId"].ToString() + "' ";
                    PagesDT = DtAll.DefaultView.ToTable();
                    if (PagesDT.Rows.Count > 0)
                    {
                        string fontawemose = ParentDT.Rows[i]["MenuIcon"].ToString();
                        if (string.IsNullOrEmpty(Convert.ToString(ParentDT.Rows[i]["MenuIcon"])))
                        {
                            ParentDT.Rows[i]["MenuIcon"] = "fas fa-th";
                        }
                        strMenu.AppendLine("<li class=\"treeview\"><a href=\"#\" " + (ParentDT.Select("Parent_Id = " + Convert.ToString(ParentDT.Rows[i]["PageId"])).Length > 0 ? "rel=\"" + Convert.ToString(ParentDT.Rows[i]["PageId"]) : "") +
                  "\"><i class='" + ParentDT.Rows[i]["MenuIcon"] + "'></i><span>" + ParentDT.Rows[i]["DisplayName"] + " </span><i class=\"fa fa-angle-left pull-right\"></i></a>");
                        strMenu.AppendLine("<ul class=\"treeview-menu\">");
                        //fa fa-th
                        for (int j = 0; j < PagesDT.Rows.Count; j++)
                        {
                            strMenu.AppendLine("<li><a href=\"" + (String.IsNullOrEmpty(Convert.ToString(PagesDT.Rows[j]["PageName"])) ? "" : ResolveUrl(Convert.ToString("~/" + PagesDT.Rows[j]["PageName"])) + "\" ") + "><i class=\"far fa-circle\"></i>" + Convert.ToString(PagesDT.Rows[j]["DisplayName"]) + "</a></li>");
                        }
                        strMenu.AppendLine("</ul>");
                        strMenu.AppendLine("</li>");

                    }


                }
                strMenu.AppendLine("<ul class=\"treeview-menu\">");
                strMenu.AppendLine("</ul>");
                strMenu.AppendLine("</li>");
                strMenu.AppendLine("</ul>");
                LSidebar.InnerHtml = strMenu.ToString();
            }
        }

        protected void DSREntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DSREntryForm.aspx");
        }

        protected void DistOrder_Click(object sender, EventArgs e)
        {

        }

        protected void BeatEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatPlanEntry.aspx");
        }

        protected void Downloads_Click(object sender, EventArgs e)
        {

        }

        protected void LocalExp_Click(object sender, EventArgs e)
        {

        }

        protected void DSREntryL2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DSREntryFormLevel2.aspx");
        }

        protected void TourExpEntryL2_Click(object sender, EventArgs e)
        {

        }
        protected void BeatPlanAppL2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatPlanApproval.aspx");
        }

        protected void DownloadL2_Click(object sender, EventArgs e)
        {

        }

        protected void LocalExpL2_Click(object sender, EventArgs e)
        {

        }

        protected void DSRLevel3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DSRVistEntry.aspx");
        }

        protected void TourExpL3_Click(object sender, EventArgs e)
        {

        }

        protected void BeatPlanAppL3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BeatPlanApproval.aspx");
        }

        protected void DownlaodsL3_Click(object sender, EventArgs e)
        {

        }

        protected void LocalExpenseL3_Click(object sender, EventArgs e)
        {

        }

        protected void OrderEntry_Click(object sender, EventArgs e)
        {
            
        }


        protected void DownloadsDist_Click(object sender, EventArgs e)
        {

        }

        protected void DistComplaint_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistComplaint.aspx");
        }

        protected void DistSuggestion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DistSuggestion.aspx");
        }
        public string greeting()
        {
            string greet = null;
            if (DateTime.Now.Hour < 12)
            {
                greet = "Good Morning!";
              
            }
            else if (DateTime.Now.Hour < 17)
            {
                greet = "Good Afternoon!";
               
            }
            else
            {
                greet = "Good Evening!";
                
            }
            return greet;
        }
    }
}