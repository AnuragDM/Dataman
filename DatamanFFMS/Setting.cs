using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Collections.Specialized;
using System.Web.UI;
using DAL;
using System.Globalization;
using System.Data.SqlTypes;
using System.Web.UI.WebControls;

namespace BusinessLayer
{
    #region

    public class Settings
    {
        public Settings() { }
        ~Settings() { }
        static Settings objSettings;
        static readonly object padlock = new object();

        /// <summary>
        /// Returns instance of class object
        /// </summary>
        public static Settings Instance
        {
            get
            {
                return GetInstance();
            }
        }
        private static Settings GetInstance()
        {
            lock (padlock)
            {
                //if (objSettings == null)
                objSettings = new Settings();
            }

            return objSettings;
        }
        private static String _CultureCookiesName;

        public static bool EnableHttpCompression
        {
            get { return true; }
        }
        public static String CultureCookiesName
        {
            get { return _CultureCookiesName; }
            set { _CultureCookiesName = value; }
        }
        public static String DefaultLanguage { get { return "_"; } }
        public static Exception GetLastException;
        public String SessionDataID
        {
            get { return Convert.ToString(HttpContext.Current.Session["DataID"]); }
            set { HttpContext.Current.Session["DataID"] = value; }
        }
        public String InstanceID
        {
            get { return Convert.ToString(HttpContext.Current.Session["InstanceID"]); }
            set { HttpContext.Current.Session["InstanceID"] = value; }
        }
        public String Role
        {
            get { return (HttpContext.Current.Session["Role"] != null ? Convert.ToString(HttpContext.Current.Session["Role"]) : "0"); }
            set { HttpContext.Current.Session["Role"] = value; }
        }
        //Ankita - 03/may/2016- (For Optimization)
        public String RoleType
        {
            get { return (HttpContext.Current.Session["RoleType"] != null ? Convert.ToString(HttpContext.Current.Session["RoleType"]) : "0"); }
            set { HttpContext.Current.Session["RoleType"] = value; }
        }
        public String EmpName
        {
            get { return (HttpContext.Current.Session["EmpName"] != null ? Convert.ToString(HttpContext.Current.Session["EmpName"]) : "0"); }
            set { HttpContext.Current.Session["EmpName"] = value; }
        }

        public String Track_userid
        {
            get { return (HttpContext.Current.Session["Track_userid"] != null ? Convert.ToString(HttpContext.Current.Session["Track_userid"]) : "0"); }
            set { HttpContext.Current.Session["Track_userid"] = value; }
        }

        public String UserID
        {
            get { return (HttpContext.Current.Session["UserID"] != null ? Convert.ToString(HttpContext.Current.Session["UserID"]) : "0"); }
            set { HttpContext.Current.Session["UserID"] = value; }
        }
        public String SMID
        {
            get { return (HttpContext.Current.Session["SMID"] != null ? Convert.ToString(HttpContext.Current.Session["SMID"]) : "0"); }
            set { HttpContext.Current.Session["SMID"] = value; }
        }
        public String RoleID
        {
            get { return (HttpContext.Current.Session["RoleID"] != null ? Convert.ToString(HttpContext.Current.Session["RoleID"]) : "0"); }
            set { HttpContext.Current.Session["RoleID"] = value; }
        }
        public String ConPerID
        {
            get { return (HttpContext.Current.Session["ConPerID"] != null ? Convert.ToString(HttpContext.Current.Session["ConPerID"]) : "0"); }
            set { HttpContext.Current.Session["ConPerID"] = value; }
        }
        public String SalesPersonLevel
        {
            get { return (HttpContext.Current.Session["SalesPersonLevel"] != null ? Convert.ToString(HttpContext.Current.Session["SalesPersonLevel"]) : "0"); }
            set { HttpContext.Current.Session["SalesPersonLevel"] = value; }
        }
        public String UserName
        {
            get { return Convert.ToString(HttpContext.Current.Session["UserName"]); }
            set { HttpContext.Current.Session["UserName"] = value; }
        }
        public String Store
        {
            get { return String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["Store"])) ? "" : Convert.ToString(HttpContext.Current.Session["Store"]); }
            set { HttpContext.Current.Session["Store"] = value; }
        }
        public String DefaultPage
        {
            get { return String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["DefaultPage"])) ? "Default.aspx" : Convert.ToString(HttpContext.Current.Session["DefaultPage"]); }
            set { HttpContext.Current.Session["DefaultPage"] = String.IsNullOrEmpty(value) ? null : value.Trim().ToLower(); }//Set Default Page here
        }
        public String ErrorPage
        {
            get { return String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["ErrorPage"])) ? "error.aspx" : Convert.ToString(HttpContext.Current.Session["ErrorPage"]); }
            set { HttpContext.Current.Session["ErrorPage"] = value; }//Set Error Page here
        }
        public String Theme
        {
            get { return Convert.ToString(HttpContext.Current.Session["Theme"]); }
            set { HttpContext.Current.Session["Theme"] = value; }
        }
        public String IsPartyDist
        {
            get { return (HttpContext.Current.Session["IsPartyDist"] != null ? Convert.ToString(HttpContext.Current.Session["IsPartyDist"]) : "0"); }
            set { HttpContext.Current.Session["IsPartyDist"] = value; }
        }
        public String DefaultTheme
        {
            get { return Convert.ToString(HttpContext.Current.Session["DefaultTheme"]); }
            set { HttpContext.Current.Session["DefaultTheme"] = value; }//Set Default Theme here
        }
        public String UserIP
        {
            get { return IPAddress(); }
        }
        public String DistributorID
        {
            get { return (HttpContext.Current.Session["DistributorID"] != null ? Convert.ToString(HttpContext.Current.Session["DistributorID"]) : "0"); }
            set { HttpContext.Current.Session["DistributorID"] = value; }
        }

        public String AreaName
        {
            get { return (HttpContext.Current.Session["AreaName"] != null ? Convert.ToString(HttpContext.Current.Session["AreaName"]) : "0"); }
            set { HttpContext.Current.Session["AreaName"] = value; }
        }

        public String BeatName
        {
            get { return (HttpContext.Current.Session["BeatName"] != null ? Convert.ToString(HttpContext.Current.Session["BeatName"]) : "0"); }
            set { HttpContext.Current.Session["BeatName"] = value; }
        }
        public String DesigID
        {
            get { return (HttpContext.Current.Session["DesigID"] != null ? Convert.ToString(HttpContext.Current.Session["DesigID"]) : "0"); }
            set { HttpContext.Current.Session["DesigID"] = value; }
        }
        public String OrderEntryType
        {
            get { return (HttpContext.Current.Session["OrderEntryType"] != null ? Convert.ToString(HttpContext.Current.Session["OrderEntryType"]) : "0"); }
            set { HttpContext.Current.Session["OrderEntryType"] = value; }
        }
        public String AreaWiseDistributor
        {
            get { return (HttpContext.Current.Session["AreaWiseDistributor"] != null ? Convert.ToString(HttpContext.Current.Session["AreaWiseDistributor"]) : "0"); }
            set { HttpContext.Current.Session["AreaWiseDistributor"] = value; }
        }

        private string IPAddress()
        {
            string strIpAddress;
            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIpAddress == null)
                strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return strIpAddress;
        }
        public void KillSession(string Url, Page page)
        {
            string script = "windows.history.clear(); window.onload = 'history.go(+1)';";
            ScriptManager.RegisterClientScriptBlock(page, GetType(), "HistoryKey", script, true);
            KillSession("Loged Out");
            RedirectTopPage(Url, page);
        }
        public void KillSession(string Remarks)
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddSeconds(19800).AddSeconds(-1));
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.AppendHeader("Pragma", "no-cache");
            objSettings = null;
        }
     
        public void RedirectCurrentPage(string Url, Page page)
        {
            Control ctl = new Control();
            string script = "window.location = '" + ctl.ResolveUrl(Url) + "';";
            //string script = "top.window.location = '" + Url + "';";
            ScriptManager.RegisterClientScriptBlock(page, GetType(), "RedirectCurrKey", script, true);
        }
        public void RedirectTopPage(string Url, Page page)
        {
            Control ctl = new Control();
            string script = "top.window.location = '" + ctl.ResolveUrl("~/" +Url) + "';";
            //string script = "top.window.location = '" + Url + "';";
            ScriptManager.RegisterClientScriptBlock(page, GetType(), "RedirectTopKey", script, true);
        }
        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name.ToLower();
            return sRet;
        }
        public string GetCurrentPageNameWithQueryString()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            string sQuery = System.Web.HttpContext.Current.Request.Url.Query;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet + sQuery;
        }
        public String VistID
        {
            get { return (HttpContext.Current.Session["VistID"] != null ? Convert.ToString(HttpContext.Current.Session["VistID"]) : "0"); }
            set { HttpContext.Current.Session["VistID"] = value; }
        }

        public String DSRSMID
        {
            get { return (HttpContext.Current.Session["DSRSMID"] != null ? Convert.ToString(HttpContext.Current.Session["DSRSMID"]) : "0"); }
            set { HttpContext.Current.Session["DSRSMID"] = value; }
        }
        public String AreaID
        {
            get { return (HttpContext.Current.Session["AreaID"] != null ? Convert.ToString(HttpContext.Current.Session["AreaID"]) : "0"); }
            set { HttpContext.Current.Session["AreaID"] = value; }
        }
        public String OrderID
        {
            get { return (HttpContext.Current.Session["OrderID"] != null ? Convert.ToString(HttpContext.Current.Session["OrderID"]) : "0"); }
            set { HttpContext.Current.Session["OrderID"] = value; }
        }
        public String DemoID
        {
            get { return (HttpContext.Current.Session["DemoID"] != null ? Convert.ToString(HttpContext.Current.Session["DemoID"]) : "0"); }
            set { HttpContext.Current.Session["DemoID"] = value; }
        }
        public String VistIDDist
        {
            get { return (HttpContext.Current.Session["VistIDDist"] != null ? Convert.ToString(HttpContext.Current.Session["VistIDDist"]) : "0"); }
            set { HttpContext.Current.Session["VistIDDist"] = value; }
        }

        public String EntryLocked
        {
            get { return (HttpContext.Current.Session["EntryLocked"] != null ? Convert.ToString(HttpContext.Current.Session["EntryLocked"]) : "0"); }
            set { HttpContext.Current.Session["EntryLocked"] = value; }
        }
        public String DSRDATE
        {
            get { return (HttpContext.Current.Session["DSRDATE"] != null ? Convert.ToString(HttpContext.Current.Session["DSRDATE"]) : "0"); }
            set { HttpContext.Current.Session["DSRDATE"] = value; }
        }
        public String SmLogInName
        {
            get { return (HttpContext.Current.Session["SmLogInName"] != null ? Convert.ToString(HttpContext.Current.Session["SmLogInName"]) : "0"); }
            set { HttpContext.Current.Session["SmLogInName"] = value; }
        }
        public String LoggedDistName
        {
            get { return (HttpContext.Current.Session["LoggedDistName"] != null ? Convert.ToString(HttpContext.Current.Session["LoggedDistName"]) : "0"); }
            set { HttpContext.Current.Session["LoggedDistName"] = value; }
        }

        public static string  GetDocID(string Name, DateTime da)
        {
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5,Name);
            dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime,8,da);
            dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
            return Convert.ToString(dbParam[2].Value);
        }

        public static void  SetDocID(string Name, string docID)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5,Name);
            dbParam[1] = new DbParameter("@mDocId", DbParameter.DbType.VarChar,35,docID);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Setdocid", dbParam);
            
        }
        public String GroupID
        {
            get { return (HttpContext.Current.Session["GroupID"] != null ? Convert.ToString(HttpContext.Current.Session["GroupID"]) : "0"); }
            set { HttpContext.Current.Session["GroupID"] = value; }
        }
        public String Accuracy
        {
            get { return (HttpContext.Current.Session["Accuracy"] != null ? Convert.ToString(HttpContext.Current.Session["Accuracy"]) : "0"); }
            set { HttpContext.Current.Session["Accuracy"] = value; }
        }
        public String PersonID
        {
            get { return (HttpContext.Current.Session["PersonID"] != null ? Convert.ToString(HttpContext.Current.Session["PersonID"]) : "0"); }
            set { HttpContext.Current.Session["PersonID"] = value; }
        }
        public String DeviceNo
        {
            get { return (HttpContext.Current.Session["DeviceNo"] != null ? Convert.ToString(HttpContext.Current.Session["DeviceNo"]) : "0"); }
            set { HttpContext.Current.Session["DeviceNo"] = value; }
        }
        public String CompCode
        {
            get { return (HttpContext.Current.Session["CompCode"] != null ? Convert.ToString(HttpContext.Current.Session["CompCode"]) : "0"); }
            set { HttpContext.Current.Session["CompCode"] = value; }
        }
        public String CompUrl
        {
            get { return (HttpContext.Current.Session["CompUrl"] != null ? Convert.ToString(HttpContext.Current.Session["CompUrl"]) : "0"); }
            set { HttpContext.Current.Session["CompUrl"] = value; }
        }
      
        public static string GetVisitDate(int VisiID)
        {
            string VisitDate = "";
            if (VisiID > 0)
            {
                string st = "select VDate from TransVisit where VisId=" + VisiID;
                VisitDate = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            }
            return VisitDate;
        }
        public static bool GetVisitLocked(int VisiID)
        {
            bool Locked =false;
            if (VisiID > 0)
            {
                string st = "select Lock from TransVisit where VisId=" + VisiID;
                Locked = Convert.ToBoolean(DbConnectionDAL.GetScalarValue(CommandType.Text, st));
            }
            return Locked;
        }

        public static string GetVisitRejected(int VisiID)
        {
            string Locked = "";
            if (VisiID > 0)
            {
                string st = "select Appstatus from TransVisit where VisId=" + VisiID;
                Locked = DbConnectionDAL.GetScalarValue(CommandType.Text,st).ToString();
            }
            return Locked;
        }

        public static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            //DateTime newDate = dt.AddHours(+5.30);
            DateTime newDate = dt.AddHours(+5.50);
            return newDate;
        }

        public static string  dateformat(string  da)
        {

            DateTime todt = DateTime.Parse(da);
            string d1 = todt.ToString("MM/dd/yyyy");

            //string d1 = todt.ToString("MM/dd/yyyy");
            return d1;
          //  string d = string.Format("{0:dd/MMM/yyyy}", da);
          //  d=  d.Replace("12:00:00 AM", "").Trim();
          ////  d = "09/09/2015";
          //  DateTime dt = DateTime.ParseExact(d, "dd/MM/yyyy", CultureInfo.InvariantCulture);
          //  // for both "1/1/2000" or "25/1/2000" formats
          //  DateTime d1 = Convert.ToDateTime(dt.ToString("MM/dd/yyyy"));

          //  //DateTime todt = DateTime.Parse(da);


          //  //string d1 = todt.ToString("MM/dd/yyyy");
          //  return d1;
        }

        public static string dateformatprv(string da)
        {

            DateTime todt = DateTime.Parse(da);
            //string d1 = todt.ToString("MM/dd/yyyy");
            string d1 = todt.ToString();

            int year = todt.Year;

            DateTime sdf = new DateTime();         
           

            //string d1 = todt.ToString("MM/dd/yyyy");
            return d1;
            //  string d = string.Format("{0:dd/MMM/yyyy}", da);
            //  d=  d.Replace("12:00:00 AM", "").Trim();
            ////  d = "09/09/2015";
            //  DateTime dt = DateTime.ParseExact(d, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //  // for both "1/1/2000" or "25/1/2000" formats
            //  DateTime d1 = Convert.ToDateTime(dt.ToString("MM/dd/yyyy"));

            //  //DateTime todt = DateTime.Parse(da);


            //  //string d1 = todt.ToString("MM/dd/yyyy");
            //  return d1;
        }
        public static string dateformat1(string da)
        {
            DateTime todt = DateTime.Parse(da);
            string d1 = todt.ToString("MM/dd/yyyy");

            //string d1 = todt.ToString("MM/dd/yyyy");
            return d1;
        }

        public static DataTable UnderUsersforlevels(string LoginSMID, int UnderLevel)
        {

            //            string str1 = @"select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            //            where smid in (select distinct smid from MastSalesRepGrp where mastsalesrep.lvl="+ UnderLevel+" and smid in (select smid from MastSalesRepGrp where MainGrp in (" + LoginSMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + LoginSMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
            string str1 = @"select mastrole.RoleName,MastSalesRep.SMId,Smname +' ('+ MastSalesRep.Syncid + ' - ' + mastrole.RoleName + ')' as smname,MastSalesRep.Lvl,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            WHERE mastrole.RoleType='admin' AND MastSalesRep.Active=1 AND smname<>'.' AND mastsalesrep.SMId NOT IN (" + LoginSMID + ") and lvl=1 order by MastSalesRep.SMName";
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            return dt1;
        }

        public static int UnderUsersforlowest(string LoginSMID)
        {

            string str1 = @"select min(lvl) from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + LoginSMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + LoginSMID + " ))) and MastSalesRep.Active=1";
            int level = Convert.ToInt16(DAL.DbConnectionDAL.GetScalarValue(CommandType.Text, str1));
            return level;
        }
        public static DataTable UnderUsersforlowerlevels(string LoginSMID,int UnderLevel)
        {

//            string str1 = @"select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
//            where smid in (select distinct smid from MastSalesRepGrp where mastsalesrep.lvl="+ UnderLevel+" and smid in (select smid from MastSalesRepGrp where MainGrp in (" + LoginSMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + LoginSMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
            string str1 = @"select mastrole.RoleName,MastSalesRep.SMId,Smname +' ('+ MastSalesRep.Syncid + ' - ' + mastrole.RoleName + ')' as smname,MastSalesRep.Lvl,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where mastsalesrep.lvl=" + UnderLevel + " and smid in (select smid from MastSalesRepGrp where MainGrp in (" + LoginSMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + LoginSMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            return dt1;
        }
        public static DataTable UnderUsers(string LoginSMID)
        {

            string str1 = @"select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + LoginSMID + ")) and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + LoginSMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            return dt1;
        }

        public static DataTable FindunderUsers(string LoginSMID)
        {

            string str1 = @"select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + LoginSMID + "))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            return dt1;
        }

        public static DataTable BindUnderUsersApproval(string LoginSMID)
        {

            string str1 = @"select mastrole.RoleName,MastSalesRep.*,mastrole.RoleType from MastSalesRep left join MastRole on MastRole.RoleId=MastSalesRep.RoleId
            where smid in (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + LoginSMID + ")) and  level> (select distinct level from MastSalesRepGrp where MainGrp in (" + LoginSMID + " ))) and MastSalesRep.Active=1 order by MastSalesRep.SMName";
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            return dt1;
        }

        //Ankita - 03/may/2016- (For Optimization)
        public string CheckPagePermissions(string pageName, string UID)
        {
            if (Convert.ToInt32(Settings.Instance.UserID) == 0)
            {
                HttpContext.Current.Response.Redirect("~/Logout.aspx");

            }
            if (Convert.ToInt32(Settings.Instance.SMID) == 0 && Convert.ToInt32(Settings.Instance.DistributorID) == 0)
            {
                HttpContext.Current.Response.Redirect("~/Logout.aspx");

            }
            string allPerm = string.Empty;
            if (!string.IsNullOrEmpty(UID))
            {
                int PageID = PageIDfromPageName(pageName);

                string PagePermission = @"select  case ViewP when 1 then 'true' else 'false' end +','+ case AddP when 1 then 'true' else 'false' end 
+','+ case EditP when 1 then 'true' else 'false' end +','+ case DeleteP when 1 then 'true' else 'false' end +','
+ case ExportP when 1 then 'true' else 'false' end  as Permission from MastRolePermission where PageId=" + PageID + " and RoleId=" + Settings.Instance.RoleID + "";
                object onj = DbConnectionDAL.GetScalarValue(CommandType.Text, PagePermission);
                if (onj != null)
                {
                    allPerm = onj.ToString();
                }
                else
                {
                    allPerm = "false,false,false,false,false";
                }
                //   
                //DataTable PDT = DbConnectionDAL.GetDataTable(CommandType.Text, PagePermission);
                //if (PDT.Rows.Count > 0)
                //{
                //    allPerm = PDT.Rows[0]["Permission"].ToString();
                //}
                // return allPerm;
            }
            else
            {
                allPerm = "false,false,false,false,false";
            }
            return allPerm;
        }
        public int RollID(string UID)
        {
            int rolID = 0;
            string envObj = @"select * from MastLogin where Name='" + Convert.ToString(UID) + "'";
            DataTable RollDT = DbConnectionDAL.GetDataTable(CommandType.Text, envObj);
            //var str1 = from M in op.MastLogins.Where(M => M.Name == Convert.ToString(UID))
            //           select M;
            //     DataTable RollDT = BusinessClass.LINQResultToDataTable(str1);
            if (RollDT.Rows.Count > 0)
            {
                rolID = Convert.ToInt32(RollDT.Rows[0]["RoleId"].ToString());
            }
            return rolID;
        }
        public bool CheckViewPermission(string pageName, string UID)
        {//Ankita - 11/may/2016- (For Optimization)
            Boolean edit = false;
            int PageID = PageIDfromPageName(pageName);
            int rollID = RollID(UID);
           // string rolePermission = @"select * from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            string rolePermission = @"select ViewP from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            //     MastRolePermission MRP = op.MastRolePermissions.FirstOrDefault(u => u.PageId == PageID && u.RoleId == rollID);
            DataTable rolePDT = DbConnectionDAL.GetDataTable(CommandType.Text, rolePermission);
            if (rolePDT.Rows.Count > 0)
            {
                edit = Convert.ToBoolean(rolePDT.Rows[0]["ViewP"]);

            }
            return edit;
        }
       
        public bool CheckAddPermission(string pageName, string UID)
        {  //Ankita - 11/may/2016- (For Optimization)
            Boolean add = false;
            int PageID = PageIDfromPageName(pageName);
            int rollID = RollID(UID);
          //  string rolePermission = @"select * from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            string rolePermission = @"select AddP from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            DataTable rolePDT = DbConnectionDAL.GetDataTable(CommandType.Text, rolePermission);
            if (rolePDT.Rows.Count > 0)
            {
                add = Convert.ToBoolean(rolePDT.Rows[0]["AddP"]);

            }
            return add;
        }
        public bool CheckEditPermission(string pageName, string UID)
        { //Ankita - 11/may/2016- (For Optimization)
            Boolean edit = false;
            int PageID = PageIDfromPageName(pageName);
            int rollID = RollID(UID);
           // string rolePermission = @"select * from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            string rolePermission = @"select EditP from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            DataTable rolePDT = DbConnectionDAL.GetDataTable(CommandType.Text, rolePermission);
            if (rolePDT.Rows.Count > 0)
            {
                edit = Convert.ToBoolean(rolePDT.Rows[0]["EditP"]);

            }
            return edit;
        }
        public bool CheckDeletePermission(string pageName, string UID)
        {//Ankita - 11/may/2016- (For Optimization)
            Boolean delete = false;
            int PageID = PageIDfromPageName(pageName);
            int rollID = RollID(UID);
           // string rolePermission = @"select * from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            string rolePermission = @"select DeleteP from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            DataTable rolePDT = DbConnectionDAL.GetDataTable(CommandType.Text, rolePermission);
            if (rolePDT.Rows.Count > 0)
            {
                delete = Convert.ToBoolean(rolePDT.Rows[0]["DeleteP"]);

            }
            return delete;

        }

        public int PageIDfromPageName(string PageName)
        {
            string getPageIdQry = @"select PageId from MastPage where PageName='" + PageName + "' ";
            DataTable getPageDT = DbConnectionDAL.GetDataTable(CommandType.Text, getPageIdQry);
            //return (from h in op.MastPages.Where(u => u.PageName == PageName)
            //        select h.PageId).SingleOrDefault();
            if (getPageDT.Rows.Count > 0)
            {
                return Convert.ToInt32(getPageDT.Rows[0]["PageId"]);
            }
            else
            {
                return 0;
            }
        }
        public string GetPageHeaderName(string PageName)
        {
            string getPageIdQry = @"select DisplayName from MastPage where PageName='" + PageName + "' ";
            DataTable getPageDT = DbConnectionDAL.GetDataTable(CommandType.Text, getPageIdQry);
            //return (from h in op.MastPages.Where(u => u.PageName == PageName)
            //        select h.PageId).SingleOrDefault();
            if (getPageDT.Rows.Count > 0)
            {
                return getPageDT.Rows[0]["DisplayName"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static int GetDsrDays(int p)
        {
            try
            {
                string getPageIdQry = @"select DSRAllowDays from MastSalesRep where [SMId]=" + p + "";
                DataTable getPageDT = DbConnectionDAL.GetDataTable(CommandType.Text, getPageIdQry);
                return Convert.ToInt32(getPageDT.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }
        }
        public static int GetMeetDays(int p)
        {
            try
            {
                string getPageIdQry = @"select MeetAllowDays from MastSalesRep where [SMId]=" + p + "";
                DataTable getPageDT = DbConnectionDAL.GetDataTable(CommandType.Text, getPageIdQry);
                return Convert.ToInt32(getPageDT.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
            }
        }

        public static DataTable DetailDistLedger(int DistId)
        {
            DateTime toTime = Convert.ToDateTime(GetUTCTime().ToShortDateString());
            DateTime fromTime = toTime.AddMonths(-6);
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@Distributor_Id", DbParameter.DbType.Int, 1, DistId);
            dbParam[1] = new DbParameter("@From_Date", DbParameter.DbType.DateTime, 1, fromTime);
            dbParam[2] = new DbParameter("@To_Date", DbParameter.DbType.DateTime, 1, toTime);
            return DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_selectDistributorLedger", dbParam);
        }
        public void BindRole(DropDownList Dropdown)
        {
            string dropdowndata = string.Empty;
            try
            {
                //var obj = (from r in context.MastRoles.OrderBy(x => x.RoleName) select new { r.RoleId, r.RoleName }).ToList();
                string roleQury = @"select RoleId, RoleName from MastRole order by RoleName";
                DataTable roleQurydt = DbConnectionDAL.GetDataTable(CommandType.Text, roleQury);
                if (roleQurydt.Rows.Count > 0)
                {
                    Dropdown.DataSource = roleQurydt;
                    Dropdown.DataTextField = "RoleName";
                    Dropdown.DataValueField = "RoleId";
                    Dropdown.DataBind();
                }
                Dropdown.Items.Insert(0, new ListItem("-- Select Role --", "0"));
            }
            catch
            {

            }
        }
        public static DataTable UnderUsers1(string LoginSMID)
        {

            string str1 = @"select * from MastSalesRep where UnderId=" + LoginSMID;
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
            return dt1;
        }
        public void BindDesignation(DropDownList Dropdown)
        {
            string dropdowndata = string.Empty;
            try
            {
                //        var obj = (from r in context.MastDesignations.Where(x => x.Active == true).OrderBy(t => t.DesName) select new { r.DesId, r.DesName }).ToList();
                string desQuery = @"select DesId, DesName from MastDesignation where Active=1 order by DesName";
                DataTable desQuerydt = DbConnectionDAL.GetDataTable(CommandType.Text, desQuery);
                if (desQuerydt.Rows.Count > 0)
                {
                    Dropdown.DataSource = desQuerydt;
                    Dropdown.DataTextField = "DesName";
                    Dropdown.DataValueField = "DesId";
                    Dropdown.DataBind();
                }
                Dropdown.Items.Insert(0, new ListItem("-- Select Designation --", "0"));
            }
            catch
            {

            }
        }

        public void BindDepartment(DropDownList Dropdown)
        {
            string dropdowndata = string.Empty;
            try
            {
                //var obj = (from r in context.MastDepartments.Where(x => x.Active == true).OrderBy(t => t.DepName) select new { r.DepId, r.DepName }).ToList();
                string deptQury = @"select DepId, DepName  from MastDepartment where Active=1 order by DepName ";
                DataTable deptQurydt = DbConnectionDAL.GetDataTable(CommandType.Text, deptQury);
                if (deptQurydt.Rows.Count > 0)
                {
                    Dropdown.DataSource = deptQurydt;
                    Dropdown.DataTextField = "DepName";
                    Dropdown.DataValueField = "DepId";
                    Dropdown.DataBind();
                }
                Dropdown.Items.Insert(0, new ListItem("-- Select Department --", "0"));
            }
            catch
            {

            }
        }
        public static string GetStartFinancialYearfrmDate(string Fyear)
        {
            string fyear = "";
            if (Fyear != "")
            {
                try
                {


                    string[] arr = Fyear.Split('-');
                    fyear = "01/04/" + arr[0].ToString();
                    // string Endate = "31/03/" + arr[1].ToString();
                }
                catch { }

            }
            return dateformat(fyear);

        }
        public static string GetStartFinancialYearToDate(string Fyear)
        {
            string fyear = "";
            if (Fyear != "")
            {
                try
                {


                    string[] arr = Fyear.Split('-');
                    // string stdate = "01/04/" + arr[0].ToString();
                    fyear = "31/03/" + arr[1].ToString();
                }
                catch
                {

                }

            }
            return dateformat(fyear);

        }

        public static Int32 DMInt32(string val)
        {
            Int32 a = 0;
            try { a = Convert.ToInt32(val); }
            catch { }
            return a;
        }
        public static Int64 DMInt64(string val)
        {
            Int64 a = 0;
            try { a = Convert.ToInt64(val); }
            catch { }
            return a;
        }
        public static int DMInt(string val)
        {
            int a = 0;
            try { a = Convert.ToInt32(val); }
            catch { }
            return a;
        }
        public static decimal DMDecimal(string val)
        {
            decimal a = 0;
            try { a = Convert.ToDecimal(val); }
            catch { }
            return a;
        }
        public String AreaIDDistPartyList
        {
            get { return (HttpContext.Current.Session["AreaIDDP"] != null ? Convert.ToString(HttpContext.Current.Session["AreaIDDP"]) : "0"); }
            set { HttpContext.Current.Session["AreaIDDP"] = value; }
        }
        public String BeatIDDistPartyList
        {
            get { return (HttpContext.Current.Session["BeatIDDP"] != null ? Convert.ToString(HttpContext.Current.Session["BeatIDDP"]) : "0"); }
            set { HttpContext.Current.Session["BeatIDDP"] = value; }

        }
        public bool CheckExportPermission(string pageName, string UID)
        {//Ankita - 13/may/2016- (For Optimization)
            Boolean Export = false;
            int PageID = PageIDfromPageName(pageName);
            int rollID = RollID(UID);
            //string rolePermission = @"select * from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            string rolePermission = @"select ExportP from MastRolePermission where PageId=" + PageID + " and RoleId=" + rollID + "";
            DataTable rolePDT = DbConnectionDAL.GetDataTable(CommandType.Text, rolePermission);
            if (rolePDT.Rows.Count > 0)
            {
                Export = Convert.ToBoolean(rolePDT.Rows[0]["ExportP"]);

            }
            return Export;

        }
        //Added 07/01/2015
        public void BindTimeToDDL(DropDownList Dropdown)
        {
            List<string> time24hr = new List<string>();
            DateTime start = DateTime.ParseExact("00:00", "HH:mm", null);
            //Set the End Time Value
            DateTime end = DateTime.ParseExact("23:59", "HH:mm", null);
            //Set the interval time 
            int interval = 30;
            //List to hold the values of intervals
            List<string> lstTimeIntervals = new List<string>();
            //Populate the list with 15 minutes interval values
            for (DateTime i = start; i <= end; i = i.AddMinutes(interval))
            {
                time24hr.Add(i.ToString("HH:mm"));
            }
            Dropdown.DataSource = time24hr.ToList();
            Dropdown.DataBind();
        }
        //End
       
    }
    #endregion
}
