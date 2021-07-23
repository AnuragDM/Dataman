using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DAL
{
    class Settings
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
        public String UserID
        {
            get { return (HttpContext.Current.Session["UserID"] != null ? Convert.ToString(HttpContext.Current.Session["UserID"]) : "0"); }
            set { HttpContext.Current.Session["UserID"] = value; }
        }
        public String ConPerID
        {
            get { return (HttpContext.Current.Session["ConPerID"] != null ? Convert.ToString(HttpContext.Current.Session["ConPerID"]) : "0"); }
            set { HttpContext.Current.Session["ConPerID"] = value; }
        }
        public String UserName
        {
            get { return Convert.ToString(HttpContext.Current.Session["UserName"]); }
            set { HttpContext.Current.Session["UserName"] = value; }
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

        public String DefaultPage
        {
            get { return String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["DefaultPage"])) ? "LogIn.aspx" : Convert.ToString(HttpContext.Current.Session["DefaultPage"]); }
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

        public String DefaultTheme
        {
            get { return Convert.ToString(HttpContext.Current.Session["DefaultTheme"]); }
            set { HttpContext.Current.Session["DefaultTheme"] = value; }//Set Default Theme here
        }
        public String UserIP
        {
            get { return IPAddress(); }
        }


        private string IPAddress()
        {
            string strIpAddress;
            strIpAddress =HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
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
            string script = "top.window.location = '" + ctl.ResolveUrl("~/" + Url) + "';";
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

        public String EntryLocked
        {
            get { return (HttpContext.Current.Session["EntryLocked"] != null ? Convert.ToString(HttpContext.Current.Session["EntryLocked"]) : "0"); }
            set { HttpContext.Current.Session["EntryLocked"] = value; }
        }


        public static string GetDocID(string Name, DateTime da)
        {
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, Name);
            dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime, 8, da);
            dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
            return Convert.ToString(dbParam[2].Value);
        }

        public static void SetDocID(string Name, string docID)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, Name);
            dbParam[1] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, docID);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Setdocid", dbParam);

        }

        public static DateTime GetUTCTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime newDate = dt.AddHours(+5.30);
            return newDate;
        }

        public static string dateformat(string da)
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
        public static string dateformat1(string da)
        {
            DateTime todt = DateTime.Parse(da);
            string d1 = todt.ToString("MM/dd/yyyy");

            //string d1 = todt.ToString("MM/dd/yyyy");
            return d1;
        }





        //Ankita - 03/may/2016- (For Optimization)
        public string CheckPagePermissions(string pageName, string UID)
        {
            string allPerm = string.Empty;
            if (!string.IsNullOrEmpty(UID))
            {
                int PageID = PageIDfromPageName(pageName);

                string PagePermission = @"select  case ViewP when 1 then 'true' else 'false' end +','+ case AddP when 1 then 'true' else 'false' end 
+','+ case EditP when 1 then 'true' else 'false' end +','+ case DeleteP when 1 then 'true' else 'false' end +','
+ case ExportP when 1 then 'true' else 'false' end  as Permission from MastRolePermission where PageId=" + PageID + " and RoleId=" + Settings.Instance.Role + "";
                object onj = DbConnectionDAL.GetScalarValue(CommandType.Text, PagePermission);
                if (onj != null)
                {
                    allPerm = onj.ToString();
                }
                else
                {
                    allPerm = "false,false,false,false,false";
                }

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
    }
}
