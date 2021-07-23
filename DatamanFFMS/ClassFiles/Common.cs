using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
//using DataAccessLayer;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using DAL;
using System.Web.Script.Serialization;
namespace BusinessLayer
{
    public class Common
    {
        static Common objCommon;
        static readonly object padlock = new object();

        public static Common Instance
        {
            get
            {
                return GetInstance();
            }
        }
        private static Common GetInstance()
        {
            lock (padlock)
            {
                //if (objCommon == null)
                objCommon = new Common();
            }
            return objCommon;
        }
        public string GetAccuracyByPerson(string personId)
        {
            string val = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select degree from PersonMaster where Id =" + personId + ""));
            // string val = DataAccessLayer.DAL.GetStringScalarVal("select degree from PersonMaster where Id =" + personId + "");
            return val;
        }
        public bool GetValidLocationTicks(string plat, string plng, string clat, string clng, DateTime pdate, DateTime cdate, String Signal)
        {
            Int16 Signal_Dist = 0;
            double dist = Calculate(Convert.ToDouble(plat), Convert.ToDouble(plng), Convert.ToDouble(clat), Convert.ToDouble(clng));
            TimeSpan timediff = (cdate.Subtract(pdate));
            if (Signal == "G")//For GPS
                Signal_Dist = 2;
            else
                Signal_Dist = 5;//For Tower
            //Valid Tick Rule = 1 min =Signal_Dist.
            if (Convert.ToDouble((timediff.TotalMinutes) * Signal_Dist) >= dist)
                return true;
            else
                return false;
        }
        public double Calculate(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
            var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
            var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
            var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
        }
        public void CreateText(string strText)
        {
            //string fileLoc = "~/TextFileFolder/11.txt";


            //FileStream fs = null;
            //if (!File.Exists(fileLoc))
            //{
            //    using (fs = File.Create(fileLoc))
            //    {
            //        using (StreamWriter sw = new StreamWriter(fileLoc))
            //        {
            //            sw.Write("Some sample text for the file");
            //        }
            //    }
            //}
            //else
            //{
            //    using (StreamWriter sw = new StreamWriter(fileLoc))
            //    {
            //        sw.Write("Some sample text for the file");
            //    }
            //}


            //using (StreamWriter sw = new StreamWriter(strText))
            //   {
            //       sw.Write(strText);
            //   }

        }
        //public void SetUserHeader(Label lbl, string user, string date, string dateFormat)
        //{
        //    DateTime dt = DateTime.Now.ToIndianDateTime();

        //    if (!String.IsNullOrEmpty(date))
        //        dt = ConvertStringToDateTime(date, dateFormat);

        //    SetUserHeader(lbl, user, dt);
        //}
        //public void SetUserHeader(Label lbl, string user, DateTime date)
        //{
        //    //lbl.Text = (String.IsNullOrEmpty(user) ? "NA" : user) + " | " + date.ToString("dd/MM/yyyy hh:mm tt");
        //    lbl.Text = String.IsNullOrEmpty(user) ? "NA" : user;
        //}.
        public void SetPageHeaders(Label lbl, string pageName)
        {
            string strHeading = "";
            SqlConnection con = Connection.Instance.GetConnection();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT [DisplayName] FROM  [PageMast] WHERE [PageName] = ltrim(rtrim('" + pageName + "'))AND DisplayYN <> 'N'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    strHeading = dr.GetString(0);
                }
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (!String.IsNullOrEmpty(strHeading))
                lbl.Text = strHeading;
        }

        public int GetPageId(string pageName)
        {
            int page_id = 0;
            SqlConnection con = Connection.Instance.GetConnection();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT [Id] FROM  [PageMast] WHERE [PageName] ='" + pageName + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    page_id = dr.GetInt32(0);
                }
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return page_id;
        }

        //public void fillDDLDatatable(DropDownList xddl, DataTable xdt, string xvalue, string xtext)
        //{
        //    xddl.DataSource = xdt;
        //    xddl.DataTextField = xtext.Trim();
        //    xddl.DataValueField = xvalue.Trim();
        //    xddl.DataBind();
        //    if (xdt.Rows.Count >= 1)
        //        xddl.Items.Insert(0, new ListItem("--Select--", "0"));
        //    else if (xdt.Rows.Count == 0)
        //        xddl.Items.Insert(0, new ListItem("---", "0"));
        //    xdt.Dispose();
        //}

        public void CascadingDDLDefaultHeadder(DataTable dt, List<CascadingDropDownNameValue> values)
        {
            if (dt.Rows.Count > 1)
                values.Add(new CascadingDropDownNameValue("--Select--", "0"));
            if (dt.Rows.Count == 0)
                values.Add(new CascadingDropDownNameValue("---", "0"));
        }




        public DataSet MyDataset(string Sql)
        {
            DataSet ds = new DataSet();
            SqlConnection con = Connection.Instance.GetConnection();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(Sql, con);
                Connection.Instance.OpenConnection(con);
                da.Fill(ds);
                Connection.Instance.CloseConnection(con);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return ds;
        }

        //public int ExecuteQuery(string Sql)
        //{
        //    int res;
        //    SqlConnection con = Connection.Instance.GetConnection();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(Sql, con);
        //        Connection.Instance.OpenConnection(con);
        //        res = cmd.ExecuteNonQuery();
        //        Connection.Instance.CloseConnection(con);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Connection.Instance.CloseConnection(con);
        //    }
        //    return res;
        //}

        //public DataTable MyDataTable(string Sql)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = Connection.Instance.GetConnection();
        //    try
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter(Sql, con);
        //        Connection.Instance.OpenConnection(con);
        //        da.Fill(dt);
        //        Connection.Instance.CloseConnection(con);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Connection.Instance.CloseConnection(con);
        //    }
        //    return dt;
        //}


        public string UploadFile(FileUpload UploadControl, string FileName, string Path)
        {
            string str = String.Empty;
            string extension = UploadControl.FileName.Substring(UploadControl.FileName.LastIndexOf('.')).ToLower();
            str = FileName + extension;
            //int i = 0;
            //while (File.Exists(HttpContext.Current.Server.MapPath(Path) + str))
            //{
            //    str = FileName + i + extension;
            //    i++;
            //}
            UploadControl.SaveAs(HttpContext.Current.Server.MapPath(Path) + str);
            return str;
        }
        //public string GenerateDocName()
        //{
        //    Random ran = new Random();
        //    string fileName = string.Empty;

        //    return fileName = DateTime.Now.ToIndianDateTime().ToOADate().ToString() + ran.Next(99).ToString();
        //}
        public DataTable ReadXLS(string FilePath, string FileName, string Sheet)
        {
            DataTable dt = new DataTable();
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(FilePath + "\\" + FileName) +
                      @";Extended Properties=""Excel 8.0;HDR=YES;IMEX=1\""";
            OleDbConnection con = new OleDbConnection(conStr);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + Sheet + "]", con);
            da.Fill(dt);
            return dt;
        }
        public DataTable ReadXLS(string FilePath, string FileName, string Sheet, bool IsHeader)
        {
            string HDR = IsHeader ? "Yes" : "No";
            DataTable dt = new DataTable();
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(FilePath + "\\" + FileName) +
                      @";Extended Properties=""Excel 8.0;HDR=" + HDR;
            conStr += @";IMEX=1\""";
            OleDbConnection con = new OleDbConnection(conStr);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + Sheet + "]", con);
            da.Fill(dt);
            return dt;
        }
        public DataTable ReadXLS(string FullFilePath, string Sheet)
        {
            DataTable dt = new DataTable();
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(FullFilePath) +
                      @";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1\""";
            OleDbConnection con = new OleDbConnection(conStr);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + Sheet + "]", con);
            da.Fill(dt);
            return dt;
        }
        public string GetRowErrs(DataTable table)
        {
            string errors = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                if (row.HasErrors)
                {
                    errors += row.RowError + ", ";
                }
            }
            if (!String.IsNullOrEmpty(errors))
                errors = errors.Substring(0, errors.Length - 2);

            return errors;
        }

        public string ConvertDataTableToString(DataTable table, int clmIndex, string separator)
        {
            string data = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                data += Convert.ToString(row[clmIndex]) + separator;
            }
            if (!String.IsNullOrEmpty(data))
                data = data.Substring(0, data.Length - separator.Length);

            return data;
        }

        //public void FillWeekDDL(int Year, int Month, DropDownList ddl)
        //{
        //    ddl.Items.Clear();
        //    ddl.Items.Add(new ListItem("--Select--", "0"));
        //    if (Year != 0 && Month != 0)
        //    {
        //        int days = System.DateTime.DaysInMonth(Year, Month);
        //        int weeks = Convert.ToInt32(decimal.Ceiling(Convert.ToDecimal(Convert.ToDecimal(days) / 7)));
        //        //ddl.Items.Add(new ListItem("--Please Select--", "0"));
        //        int cnt = 0;
        //        for (int i = 1; i <= weeks; i++)
        //        {
        //            cnt = i > 4 ? days : cnt + 7;
        //            ddl.Items.Add(new ListItem(i.ToString(), cnt.ToString()));
        //        }
        //        if (Year == DateTime.Now.ToIndianDateTime().Year && Month == DateTime.Now.ToIndianDateTime().Month)
        //            ddl.Items.FindByText((decimal.Ceiling(Convert.ToDecimal(Convert.ToDecimal(DateTime.Now.ToIndianDateTime().Day) / 7))).ToString()).Selected = true;
        //    }
        //}

        //public void FillGridView(GridView gv, string sql)
        //{
        //    DataTable dt = new DataTable();
        //    FillDataTable(dt, sql);

        //    if (dt.Rows.Count > 0)
        //    {
        //        gv.DataSource = dt;
        //        gv.DataBind();
        //    }
        //    else
        //    {

        //    }
        //}

        public void FillDataTable(DataTable xdt, string xSql)
        {
            try
            {
                SqlConnection con = Connection.Instance.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(xSql, con);
                Connection.Instance.OpenConnection(con);
                da.Fill(xdt);
                Connection.Instance.CloseConnection(con);
            }
            catch (Exception ex) { throw ex; }
        }

        //public object GetSingleRecord(string sql)
        //{
        //    SqlConnection con = Connection.Instance.GetConnection();
        //    SqlCommand com = new SqlCommand(sql, con);
        //    com.Connection.Open();
        //    object obj = com.ExecuteScalar();
        //    com.Connection.Close();
        //    return obj;
        //}
        //public void fillListDatatable(ListBox xlst, DataTable xdt, string xvalue, string xtext)
        //{
        //    xlst.DataSource = xdt;
        //    xlst.DataTextField = xtext;
        //    xlst.DataValueField = xvalue;
        //    xlst.DataBind();
        //    xdt.Dispose();
        //}
        public void fillDDLDatatableFinal(DropDownList xddl, string xmQry, string xvalue, string xtext)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            // xdt =DbConnectionDAL.getFromDataTable() DataAccessLayer.DAL.getFromDataTable(xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (xdt.Rows.Count > 1)
                xddl.Items.Insert(0, new ListItem("--Select--", "0"));
            else if (xdt.Rows.Count == 0)
                xddl.Items.Insert(0, new ListItem("---", "0"));
            xdt.Dispose();
        }

        public string GetDeviceNoByPersonID(string PersonID)
        {
            string DeviceNo = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select deviceno from mastsalesrep where smId =" + PersonID + "")); //DataAccessLayer.DAL.GetStringScalarVal("select deviceno from PersonMaster where Id =" + PersonID + "");
            return DeviceNo;
        }
        public string GetFYear(DateTime fdt)
        {
            string year = "";
            string mnt = fdt.Month.ToString();
            if (int.Parse(mnt) >= 4 && int.Parse(mnt) <= 12)
            {
                year = fdt.Year.ToString();
            }
            else
            {
                year = fdt.AddYears(-1).ToString("yyyy");
            }
            return year;
        }

        public string getSubOrdinates(int ConPerID)
        {
            string query = "";
            if (ConPerID == 1)
                query = "select Srep_code from AllLevelSRep where ParentSrep_code in (select Srep_code from AllLevelSRep where ParentSrep_code=" + ConPerID + " and lvl=2)";
            else
                query = "select Srep_code from AllLevelSRep where ParentSrep_code=" + ConPerID + " and ParentSrep_code<>Srep_code";
            string subOrdinates = ConvertDataTableToString(DbConnectionDAL.GetDataTable(CommandType.Text, query), 0, ",");
            subOrdinates = String.IsNullOrEmpty(subOrdinates) ? "0" : subOrdinates;
            return subOrdinates;
        }
        public static void SendMailMessage(string from, string to, string bcc, string cc, string subject, string body)
        {
            try
            {
                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();
                // Set the sender address of the mail message
                mMailMessage.From = new MailAddress(from);
                // Set the recepient address of the mail message
                mMailMessage.To.Add(new MailAddress(to));

                // Check if the bcc value is null or an empty string
                if ((bcc != null) && (bcc != string.Empty))
                {
                    // Set the Bcc address of the mail message
                    mMailMessage.Bcc.Add(new MailAddress(bcc));
                }
                // Check if the cc value is null or an empty value
                if ((cc != null) && (cc != string.Empty))
                {
                    // Set the CC address of the mail message
                    string[] MailCC = cc.Split(',');
                    foreach (var item in MailCC)
                    {
                        if (!string.IsNullOrEmpty(item))
                            mMailMessage.CC.Add(new MailAddress(item.ToString()));
                    }

                }
                //mail.Attachments.Add(new Attachment(FileUpload1.PostedFile.InputStream, FileUpload1.FileName));


                // Set the subject of the mail message
                mMailMessage.Subject = subject;
                // Set the body of the mail message
                mMailMessage.Body = body;

                // Set the format of the mail message body as HTML
                mMailMessage.IsBodyHtml = true;
                // Set the priority of the mail message to normal
                mMailMessage.Priority = MailPriority.Normal;

                // Instantiate a new instance of SmtpClient
                SmtpClient mSmtpClient = new SmtpClient();
                // Send the mail message


                mSmtpClient.Send(mMailMessage);
            }
            catch (Exception ex) { ex.ToString(); }
        }
        public void pushnotificationforOrderDispatchCancel(string orderDocId, string orderType, string OrderRemark)
        {
            try
            {
                string str = ""; string _smname = ""; string _smUserid = ""; string _smMobile = ""; string _smId = "";
                string _compcode, _msg = "";
                string retval = "";
                string pro_id = "PURCHASEORDER";

                str = "select CompCode from MastEnviro";
                DataTable dtComp = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dtComp.Rows.Count > 0)
                {
                    _compcode = dtComp.Rows[0]["CompCode"].ToString();

                }
                else
                {
                    return;
                }

                str = "select distid,smid,vdate from TransPurchOrder where PODocId='" + orderDocId + "'";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                if (dt.Rows.Count > 0)
                {
                    string vdate = dt.Rows[0]["vdate"].ToString();

                    str = "select smid,smname,mobile,userid from mastsalesrep where smid=" + dt.Rows[0]["smid"].ToString();
                    DataTable dtSales = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtSales.Rows.Count > 0)
                    {
                        _smId = dtSales.Rows[0]["smid"].ToString().Trim();
                        _smname = dtSales.Rows[0]["smname"].ToString().Trim();
                        _smMobile = dtSales.Rows[0]["mobile"].ToString().Trim();
                        _smUserid = dtSales.Rows[0]["userid"].ToString().Trim();
                    }

                    str = "select PartyName,Mobile,isnull(SD_ID,0) as SD_ID,userid from mastparty where partyid=" + dt.Rows[0]["distid"].ToString().Trim() + " and isnull(partydist,0)=1";
                    DataTable dtdist = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dtdist.Rows.Count > 0)
                    {
                        string sd_id = dtdist.Rows[0]["SD_ID"].ToString().Trim();

                        if (!string.IsNullOrEmpty(sd_id) && sd_id != "0")
                        {
                            str = "select partyid,PartyName,Mobile,userid from mastparty where partyid=" + sd_id + " and isnull(partydist,0)=1";
                            DataTable dtSD = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                            if (dtSD.Rows.Count > 0)
                            {
                                //Notification for SuperDistributor   

                                if (!string.IsNullOrEmpty(orderType) && orderType == "D")
                                {

                                    _msg = "Order DocId - " + orderDocId + " of Distributor ( " + dtdist.Rows[0]["PartyName"].ToString().Trim() + " ) has been Dispatched.";

                                    str = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[Distid]) output inserted.NotiId values ('" + pro_id + "'," + Convert.ToInt32(dtSD.Rows[0]["userid"].ToString().Trim()) + ",DateAdd(minute,330,getutcdate()),'" + "DistributerDispatchOrderViewForm.aspx?Docid=" + orderDocId.Replace("-", " ") + "','" + _msg + "','" + 0 + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString()) + "," + dt.Rows[0]["smid"].ToString() + "," + dtSD.Rows[0]["partyid"].ToString() + ") ";

                                    retval = DbConnectionDAL.GetStringScalarVal(str);
                                    if (!string.IsNullOrEmpty(retval))
                                    {
                                        return;
                                    }

                                    pushnotificationonPurchaseorderdispatchcancel(_msg, _compcode, dtSD.Rows[0]["Mobile"].ToString().Trim(), "Distributor Order Dispatched", dt.Rows[0]["SMID"].ToString(), dtSales.Rows[0]["SMName"].ToString(), "GOLDIEE", orderDocId, dtdist.Rows[0]["PartyName"].ToString().Trim(), "Dispatched");
                                }
                                else
                                {
                                    _msg = "Order DocId - " + orderDocId + " of Distributor ( " + dtdist.Rows[0]["PartyName"].ToString().Trim() + " ) has been Cancelled.";

                                    str = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[Distid]) values ('" + pro_id + "'," + Convert.ToInt32(dtSD.Rows[0]["userid"].ToString().Trim()) + ",DateAdd(minute,330,getutcdate()),'" + "DistributerDispatchOrderViewForm.aspx?Docid=" + orderDocId.Replace("-", " ") + "','" + _msg + "','" + 0 + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString()) + "," + dt.Rows[0]["smid"].ToString() + "," + dtSD.Rows[0]["partyid"].ToString() + ") ";

                                    retval = DbConnectionDAL.GetStringScalarVal(str);
                                    if (!string.IsNullOrEmpty(retval))
                                    {
                                        return;
                                    }
                                    pushnotificationonPurchaseorderdispatchcancel(_msg, _compcode, dtSD.Rows[0]["Mobile"].ToString().Trim(), "Distributor Order Cancelled", dt.Rows[0]["SMID"].ToString(), dtSales.Rows[0]["SMName"].ToString(), "GOLDIEE", orderDocId, dtdist.Rows[0]["PartyName"].ToString().Trim(), "Cancelled");
                                }


                            }
                        }
                        if (!string.IsNullOrEmpty(orderType) && orderType == "D")
                        {
                            //Notification for distributor
                            _msg = "Your order of DocId - " + orderDocId + " has been Dispatched.";

                            str = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[Distid]) values ('" + pro_id + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString().Trim()) + ",DateAdd(minute,330,getutcdate()),'" + "DistributerDispatchOrderViewForm.aspx?Docid=" + orderDocId.Replace("-", " ") + "','" + _msg + "','" + 0 + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString().Trim()) + "," + dt.Rows[0]["smid"].ToString() + "," + dt.Rows[0]["distid"].ToString() + ") ";

                            retval = DbConnectionDAL.GetStringScalarVal(str);
                            if (!string.IsNullOrEmpty(retval))
                            {
                                return;
                            }
                            pushnotificationonPurchaseorderdispatchcancel(_msg, _compcode, dtdist.Rows[0]["Mobile"].ToString().Trim(), "Distributor Order Dispatched", dt.Rows[0]["SMID"].ToString(), dtSales.Rows[0]["SMName"].ToString(), "GOLDIEE", orderDocId, dtdist.Rows[0]["PartyName"].ToString().Trim(), "Dispatched");
                        }
                        else
                        {
                            //Notification for distributor
                            _msg = "Your order of DocId - " + orderDocId + " has been cancelled.";

                            str = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[Distid]) values ('" + pro_id + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString().Trim()) + ",DateAdd(minute,330,getutcdate()),'" + "DistributerDispatchOrderViewForm.aspx?Docid=" + orderDocId.Replace("-", " ") + "','" + _msg + "','" + 0 + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString().Trim()) + "," + dt.Rows[0]["smid"].ToString() + "," + dt.Rows[0]["distid"].ToString() + ") ";

                            retval = DbConnectionDAL.GetStringScalarVal(str);
                            if (!string.IsNullOrEmpty(retval))
                            {
                                return;
                            }

                            pushnotificationonPurchaseorderdispatchcancel(_msg, _compcode, dtdist.Rows[0]["Mobile"].ToString().Trim(), "Distributor Order Cancelled", dt.Rows[0]["SMID"].ToString(), dtSales.Rows[0]["SMName"].ToString(), "GOLDIEE", orderDocId, dtdist.Rows[0]["PartyName"].ToString().Trim(), "Cancelled");
                        }


                        if (dt.Rows[0]["smid"].ToString() != "0")
                        {
                            if (!string.IsNullOrEmpty(orderType) && orderType == "D")
                            {
                                //Notification for SalesPerson
                                _msg = "Order DocId - " + orderDocId + " of Distributor ( " + dtdist.Rows[0]["PartyName"].ToString().Trim() + " ) has been Dispatched.";

                                str = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[ToSmid]) values ('" + pro_id + "'," + Convert.ToInt32(dtSales.Rows[0]["userid"].ToString().Trim()) + ",DateAdd(minute,330,getutcdate()),'" + "DistributerDispatchOrderViewForm.aspx?Docid=" + orderDocId.Replace("-", " ") + "','" + _msg + "','" + 0 + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString().Trim()) + "," + dt.Rows[0]["smid"].ToString() + "," + dtSales.Rows[0]["smid"].ToString() + ") ";

                                retval = DbConnectionDAL.GetStringScalarVal(str);
                                if (!string.IsNullOrEmpty(retval))
                                {
                                    return;
                                }

                                pushnotificationonPurchaseorderdispatchcancel(_msg, _compcode, dtSales.Rows[0]["Mobile"].ToString().Trim(), "Distributor Order Dispatched", dt.Rows[0]["SMID"].ToString(), dtSales.Rows[0]["SMName"].ToString(), "FFMS", orderDocId, dtdist.Rows[0]["PartyName"].ToString().Trim(), "Dispatched");
                            }
                            else
                            {
                                //Notification for SalesPerson
                                _msg = "Order DocId - " + orderDocId + " of Distributor ( " + dtdist.Rows[0]["PartyName"].ToString().Trim() + " ) has been Cancelled.";

                                str = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[ToSmid]) values ('" + pro_id + "'," + Convert.ToInt32(dtSales.Rows[0]["userid"].ToString().Trim()) + ",DateAdd(minute,330,getutcdate()),'" + "DistributerDispatchOrderForm.aspx?Docid=" + orderDocId.Replace("-", " ") + "','" + _msg + "','" + 0 + "'," + Convert.ToInt32(dtdist.Rows[0]["userid"].ToString().Trim()) + "," + dt.Rows[0]["smid"].ToString() + "," + dtSales.Rows[0]["smid"].ToString() + ") ";

                                retval = DbConnectionDAL.GetStringScalarVal(str);
                                if (!string.IsNullOrEmpty(retval))
                                {
                                    return;
                                }

                                pushnotificationonPurchaseorderdispatchcancel(_msg, _compcode, dtSales.Rows[0]["Mobile"].ToString().Trim(), "Distributor Order Cancelled", dt.Rows[0]["SMID"].ToString(), dtSales.Rows[0]["SMName"].ToString(), "FFMS", orderDocId, dtdist.Rows[0]["PartyName"].ToString().Trim(), "Cancelled");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        private string pushnotificationonPurchaseorderdispatchcancel(string msg, string compcode, string mobileno, string title, string createdbysmid, string smname, string ProductType, string Docid = "", string partyname = "", string Dispatchcancelstatus = "")
        {
            var result = "-1";

            string Query = "Select Deviceno from Mastsalesrep where mobile='" + mobileno + "'";
            DataTable dt = new DataTable();
            string serverKey = "";
            string senderId = "";
            DataTable dtserverdetail = new DataTable();

            try
            {

                dtserverdetail = DbConnectionDAL.GetDataTable(CommandType.Text, "Select DistApp_FireBase_ServerKey,DistApp_FireBase_SenderID,serverkey,senderid from Mastenviro ");

                string regid_query = "select Reg_id  from LineMaster where  Upper(Product)='" + ProductType + "' and CompCode='" + compcode + "' and mobile='" + mobileno + "'";
                string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
                string Query1 = "";

                SqlConnection cn = new SqlConnection(constrDmLicense);
                SqlCommand cmd = new SqlCommand(regid_query, cn);

                cmd.CommandType = CommandType.Text;

                cn.Open();
                string regId = cmd.ExecuteScalar() as string;

                //regId = "cuejHIp6mvo:APA91bHeN7m_aOW73JO8DAaGLvBsjTC51hmrPbO1hMqiKxfQf5qDT7937vAcDn-YVPMxue6mGhOQGUtrKXSJgfC74gIM0k-Rhplw111JBSz0AGndAGY0_Ab2Gxwxs30nhjJKMZq4dPQM"; 

                cn.Close();
                cmd = null;
                if (dtserverdetail.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(regId))
                    {

                        Query1 = "insert into TransPushNotification(smid,[Subject],Content,WebFlag) output inserted.id " +
                            "values (" + createdbysmid + ",'" + title + "','" + msg + "','Y')";
                        string Id = DbConnectionDAL.GetStringScalarVal(Query1);

                        if (ProductType == "FFMS")
                        {
                            serverKey = dtserverdetail.Rows[0]["serverkey"].ToString();
                            senderId = dtserverdetail.Rows[0]["senderid"].ToString();
                        }

                        else if (ProductType == "GOLDIEE")
                        {

                            serverKey = dtserverdetail.Rows[0]["DistApp_FireBase_ServerKey"].ToString();
                            senderId = dtserverdetail.Rows[0]["DistApp_FireBase_SenderID"].ToString();
                        }


                        string webAddr = "https://fcm.googleapis.com/fcm/send";

                        var tRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                        tRequest.ContentType = "application/json";
                        tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                        tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                        tRequest.Method = "POST";

                        var payload = new
                        {
                            to = regId,
                            priority = "high",
                            content_available = true,

                            data = new
                            {
                                body = msg,
                                title = title,
                                docid = Docid,
                                PartyName = partyname,

                                msg = msg,
                                smid = createdbysmid,
                                Dispatchcancelstatus = Dispatchcancelstatus,
                                smname = smname
                            }
                        };


                        var serializer = new JavaScriptSerializer();
                        using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                        {
                            string json = serializer.Serialize(payload);
                            streamWriter.Write(json);
                            streamWriter.Flush();
                        }

                        var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();

                            Query1 = "update TransPushNotification set serverresponse='" + result + "' where id=" + Id + "";
                            DbConnectionDAL.ExecuteQuery(Query1);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = "N";
            }

            return result;
        }

        #region V2

        public string pushnotificationforBeatDeviation(string msg, string compcode, string mobileno, string title, string createdbysmid, string smname, string ProductType, string changebeatid, string plannedbeatid, string visid)
        {
            var result = "-1";
            //string Query = "SELECT * FROM MastSalesRep WHERE Mobile='7906767390'";
            string Query = "Select Deviceno from Mastsalesrep where mobile='" + mobileno + "'";
            DataTable dt = new DataTable();
            string serverKey = "";
            string senderId = "";
            DataTable dtserverdetail = new DataTable();
            //dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            //if (dt.Rows.Count > 0)
            //{
            try
            {

                dtserverdetail = DbConnectionDAL.GetDataTable(CommandType.Text, "Select serverkey,senderid from Mastenviro ");

                string regid_query = "select Reg_id  from LineMaster where  Upper(Product)='" + ProductType + "' and CompCode='" + compcode + "' and mobile='" + mobileno + "'";
                string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
                string Query1 = "";

                SqlConnection cn = new SqlConnection(constrDmLicense);
                SqlCommand cmd = new SqlCommand(regid_query, cn);

                cmd.CommandType = CommandType.Text;

                cn.Open();
                string regId = cmd.ExecuteScalar() as string;

                ///regId = "dhlIGmAIT2Q:APA91bHX_9SaxJVgADxA_h653QXxQqEjVlUB1B7twiI9zotPn8upCbPSyyGZjzW2gmSTRaL0kx8yVlThrYnGBkzQejHj_OyCoL8gan5jjMO8hL9K_LGBLxRFOQzqCz9PttlO-W94XOtw";

                cn.Close();
                cmd = null;
                if (dtserverdetail.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(regId))
                    {

                        Query1 = "insert into TransPushNotification(smid,[Subject],Content,WebFlag) output inserted.id " +
    "values (" + createdbysmid + ",'" + title + "','" + msg + "','Y')";
                        string Id = DbConnectionDAL.GetStringScalarVal(Query1);
                        if (ProductType == "FFMS")
                        {
                            serverKey = "AAAAg3ziCCE:APA91bG2ambp-VLvSJSL8cbmiAygmgTXMQcoWk6kzunAWZ10UJT92wZt06NuJZpRnIGq1XgZYfHG_EpVE7qxGoey2oLCxo5g_me9AS85ouEu8T9bWz6XWFovfpsSoIdSjoibxl4iQHit";//dtserverdetail.Rows[0]["serverkey"].ToString();   // "AAAAGmgBKmE:APA91bGhywq0On9VncehIFPDorXSe59jP4rC-asBGLlnObDf2kF79_GRV3zf9zplDZ_Vyn8SNbr1UFIPM9Fb4bjy-a-Lx70BjQOmsJcRA5BINxTi15W8sANIXALjwaDN6l0nex919eJI9s_C4q46aYpa3feESG2TOg";//s


                            senderId = "564735903777";//dtserverdetail.Rows[0]["senderid"].ToString(); //"113414056545";
                        }
                        else if (ProductType == "CRM MANAGER")
                        {
                            serverKey = "AAAAU9r9dNQ:APA91bGQnNQK0uiNjNZA_sapid9yItgbClKquZRTHubkjGG1IUcCIiNKa57TNulr2BaS8NWqdE_hklLneTQdmfESwTL3n_eBDFm2jInksd-C5jYzmgdjqqrh-1vN8F6e79_hDiVoSe5p";//s


                            senderId = "360156329172";
                        }
                        else if (ProductType == "GOLDIEE")
                        {

                            serverKey = "AAAAwt64dG0:APA91bFMf2SOqW_CcfRnIKIMMQbysoz5i7ckOtALo8rzJnN75PIqRxmSSVJ9biH7RZCC8oVN0uLmE7cnDTGQaWJ65GOHtnxyEWv3u4GalxCvwRNwi-hZnfVt0zXdU-S9YE_-WI-L6cKQ";//s


                            senderId = "836960285805";
                        }


                        string webAddr = "https://fcm.googleapis.com/fcm/send";

                        //var result = "-1";
                        var tRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                        tRequest.ContentType = "application/json";
                        tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                        tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                        tRequest.Method = "POST";

                        var payload = new
                        {
                            to = regId,
                            priority = "high",
                            content_available = true,
                            //notification = new
                            //{
                            //    body = msg,
                            //    title = title
                            //},
                            data = new
                            {
                                body = msg,
                                title = title,
                                changebeatid = changebeatid,
                                plannedbeatid = plannedbeatid,
                                visid = visid,
                                msg = msg,
                                smid = createdbysmid,

                                smname = smname
                            }
                        };



                        var serializer = new JavaScriptSerializer();
                        using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                        {
                            string json = serializer.Serialize(payload);
                            streamWriter.Write(json);
                            streamWriter.Flush();
                        }

                        var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();


                            Query1 = "update TransPushNotification set serverresponse='" + result + "' where id=" + Id + "";
                            DbConnectionDAL.ExecuteQuery(Query1);
                        }
                    }
                }
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = "N";
            }
            // }
            return result;
        }

        public string pushnotificationforPartyCreation(string msg, string compcode, string mobileno, string title, string createdbysmid, string smname, string ProductType, string PartyId)
        {
            var result = "-1";
            //string Query = "SELECT * FROM MastSalesRep WHERE Mobile='7906767390'";
            string Query = "Select Deviceno from Mastsalesrep where mobile='" + mobileno + "'";
            DataTable dt = new DataTable();
            string serverKey = "";
            string senderId = "";
            DataTable dtserverdetail = new DataTable();
            //dt = DbConnectionDAL.GetDataTable(CommandType.Text, Query);
            //if (dt.Rows.Count > 0)
            //{
            try
            {

                dtserverdetail = DbConnectionDAL.GetDataTable(CommandType.Text, "Select serverkey,senderid from Mastenviro ");

                string regid_query = "select Reg_id  from LineMaster where  Upper(Product)='" + ProductType + "' and CompCode='" + compcode + "' and mobile='" + mobileno + "'";
                string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
                string Query1 = "";

                SqlConnection cn = new SqlConnection(constrDmLicense);
                SqlCommand cmd = new SqlCommand(regid_query, cn);

                cmd.CommandType = CommandType.Text;

                cn.Open();
                string regId = cmd.ExecuteScalar() as string;

                ///regId = "dhlIGmAIT2Q:APA91bHX_9SaxJVgADxA_h653QXxQqEjVlUB1B7twiI9zotPn8upCbPSyyGZjzW2gmSTRaL0kx8yVlThrYnGBkzQejHj_OyCoL8gan5jjMO8hL9K_LGBLxRFOQzqCz9PttlO-W94XOtw";

                cn.Close();
                cmd = null;
                if (dtserverdetail.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(regId))
                    {

                        Query1 = "insert into TransPushNotification(smid,[Subject],Content,WebFlag) output inserted.id " +
    "values (" + createdbysmid + ",'" + title + "','" + msg + "','Y')";
                        string Id = DbConnectionDAL.GetStringScalarVal(Query1);
                        if (ProductType == "FFMS")
                        {
                            serverKey = "AAAAg3ziCCE:APA91bG2ambp-VLvSJSL8cbmiAygmgTXMQcoWk6kzunAWZ10UJT92wZt06NuJZpRnIGq1XgZYfHG_EpVE7qxGoey2oLCxo5g_me9AS85ouEu8T9bWz6XWFovfpsSoIdSjoibxl4iQHit";//dtserverdetail.Rows[0]["serverkey"].ToString();   // "AAAAGmgBKmE:APA91bGhywq0On9VncehIFPDorXSe59jP4rC-asBGLlnObDf2kF79_GRV3zf9zplDZ_Vyn8SNbr1UFIPM9Fb4bjy-a-Lx70BjQOmsJcRA5BINxTi15W8sANIXALjwaDN6l0nex919eJI9s_C4q46aYpa3feESG2TOg";//s


                            senderId = "564735903777";//dtserverdetail.Rows[0]["senderid"].ToString(); //"113414056545";
                        }
                        else if (ProductType == "CRM MANAGER")
                        {
                            serverKey = "AAAAU9r9dNQ:APA91bGQnNQK0uiNjNZA_sapid9yItgbClKquZRTHubkjGG1IUcCIiNKa57TNulr2BaS8NWqdE_hklLneTQdmfESwTL3n_eBDFm2jInksd-C5jYzmgdjqqrh-1vN8F6e79_hDiVoSe5p";//s


                            senderId = "360156329172";
                        }
                        else if (ProductType == "GOLDIEE")
                        {

                            serverKey = "AAAAwt64dG0:APA91bFMf2SOqW_CcfRnIKIMMQbysoz5i7ckOtALo8rzJnN75PIqRxmSSVJ9biH7RZCC8oVN0uLmE7cnDTGQaWJ65GOHtnxyEWv3u4GalxCvwRNwi-hZnfVt0zXdU-S9YE_-WI-L6cKQ";//s


                            senderId = "836960285805";
                        }


                        string webAddr = "https://fcm.googleapis.com/fcm/send";

                        //var result = "-1";
                        var tRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                        tRequest.ContentType = "application/json";
                        tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                        tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                        tRequest.Method = "POST";

                        var payload = new
                        {
                            to = regId,
                            priority = "high",
                            content_available = true,
                            //notification = new
                            //{
                            //    body = msg,
                            //    title = title
                            //},
                            data = new
                            {
                                body = msg,
                                title = title,
                                PartyId = PartyId,
                                msg = msg,
                                smid = createdbysmid,

                                smname = smname
                            }
                        };



                        var serializer = new JavaScriptSerializer();
                        using (var streamWriter = new StreamWriter(tRequest.GetRequestStream()))
                        {
                            string json = serializer.Serialize(payload);
                            streamWriter.Write(json);
                            streamWriter.Flush();
                        }

                        var httpResponse = (HttpWebResponse)tRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();


                            Query1 = "update TransPushNotification set serverresponse='" + result + "' where id=" + Id + "";
                            DbConnectionDAL.ExecuteQuery(Query1);
                        }
                    }
                }
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = "N";
            }
            // }
            return result;
        }

        #endregion

    }
}
