using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using DAL;
using System.Data;
using System.Web.Services;
using System.Collections;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Transactions;

namespace AstralFFMS
{
    public partial class ActivityTemplateMapping : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            //btnExport.CssClass = "btn btn-primary";
            //btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            //_exportp = Convert.ToBoolean(SplitPerm[4]);
            if (Convert.ToBoolean(SplitPerm[0]) == false)
            {
                Response.Redirect("~/Logout.aspx");
            }

           
                if (Convert.ToBoolean(SplitPerm[1]) == true)
                {
                    hidsave.Value = "true";
                }
                else
                {
                    hidsave.Value = "false";

                }
       

                if (Convert.ToBoolean(SplitPerm[2]) == true)
                {
                    hidupdate.Value = "true";
                }
                else
                {
                    hidupdate.Value = "false";

                }

            
            //}
            //btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            ////btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            //btnDelete.CssClass = "btn btn-primary";



            List<Distributors> distributors = new List<Distributors>();
            distributors.Add(new Distributors());
            rpt.DataSource = distributors;
            rpt.DataBind();
        }
        public class Distributors
        {


            public string Date { get; set; }
            public string SalesRepName { get; set; }

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click1(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        protected void btncancel1_Click(object sender, EventArgs e)
        {

        }


        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string col = "", str = "";
            str = "SELECT STUFF( (SELECT ', ' +quotename(t2.COL) FROM (select  MAC.AttributeField  AS  COL from TransActivity TA LEFT JOIN MastActivityCustom MAC ON MAC.Custom_Id=TA.CustomFieldiD) t2 FOR XML PATH ('')), 1, 1, '') as name";
            col = DbConnectionDAL.GetStringScalarVal(str);
            str = "SELECT * from   ( select  MAC.AttributeField,TA.CustomValue   from TransActivity TA LEFT JOIN MastActivityCustom MAC ON MAC.Custom_Id=TA.CustomFieldiD ) x pivot  ( max(CustomValue) for AttributeField in (" + col + ") ) p";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

        }

        [WebMethod(EnableSession = true)]
        public static string PopulateSalesPersonSearch(string AreaId, string Status)
        {
            string str = "";

            if (Status == "S")
            {
                str = "select SMId Value,SMName+'('+isnull(SyncId,'')+')- '+(select areaname from mastarea where areaId=MastSalesRep.CityId and AreaType='CITY') as Text from MastSalesRep where  Active=1 and CityId in (select AreaId from MastArea where UnderId IN(select AreaId  from MastArea where UnderId IN(" + Convert.ToString(AreaId) + ") And AreaType='DISTRICT') and AreaType='CITY') order by Text ";
            }
            else if (Status == "C")
            {
                str = "select SMId Value,SMName+'('+isnull(SyncId,'')+')- '+(select areaname from mastarea where areaId=MastSalesRep.CityId and AreaType='CITY') as Text from MastSalesRep where  Active=1 and CityId in (" + Convert.ToString(AreaId) + ") order by Text ";
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            return JsonConvert.SerializeObject(dt);

        }


        [WebMethod(EnableSession = true)]
        public static string PopulatePartyDistributorSearch(string AreaId, string Status)
        {
            string str = "";
            if (Status == "S")
            {
                str = "select PartyId Value,PartyName+'('+isnull(SyncId,'')+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as Text from MastParty where PartyDist=1 and Active=1 and AreaId in (Select AreaId from MastArea where UnderId IN (select AreaId from MastArea where UnderId IN(select AreaId  from MastArea where UnderId IN(" + Convert.ToString(AreaId) + ") And AreaType='DISTRICT') and AreaType='City') and AreaType='Area') order by Text ";
            }
            else if (Status == "C")
            {
                str = "select PartyId Value,PartyName+'('+isnull(SyncId,'')+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as Text from MastParty where PartyDist=1 and Active=1 and AreaId in (Select AreaId From MastArea where UnderId IN(" + AreaId + ")) order by Text ";
            }
            else if (Status == "SP")
            {
                str = "select PartyId Value,PartyName+'('+isnull(SyncId,'')+')- '+(select areaname from mastarea where areaId=MastParty.CityId and AreaType='City') as Text from MastParty where PartyDist=1 and Active=1 and smid in (" + AreaId + ") order by Text ";
            }
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            return JsonConvert.SerializeObject(dt);

        }

        [WebMethod(EnableSession = true)]
        public static string PopulateState()
        {
            string str = "";
            str = "select AreaName Text ,AreaID Value from mastarea where   AreaType='State' and Active='1' order by AreaName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            return JsonConvert.SerializeObject(dt);
        }
        [WebMethod(EnableSession = true)]
        public static string PopulateCityByMultiState(string StateID)
        {
            DataTable dt = new DataTable();
            if (StateID != "")
            {
                string strQ = "select mct.AreaId Value,mct.AreaName Text from MastArea mct left join mastarea mdt on mdt.AreaId=mct.UnderId where  mct.areatype='CITY' and mdt.areatype='DISTRICT' and mdt.UnderId in (" + StateID + ")";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string BindTitle()
        {
            string str = "";

            str = "select Header_Id Value,Title Text from MastActivityCustomHeader where ISNULL(Active,0)=1";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            return JsonConvert.SerializeObject(dt);

        }
        [WebMethod(EnableSession = true)]
        public static string SaveActivityMapping(string TempHeaderId, string fromdate, string todate, string visibleDistid, string visibleSMID, string EditFlg, string MapId, string Remark)
        {
            #region VariableDeclaration
            string Msg = "";
            string str = "", HeaderTemplateName = "", notificationMsg = "";
            string fDate = "", tDate = ""; int count = 0, Id = 0;
            string retval = ""; string regid_query = ""; string compcode = ""; string distMobile = "", smidMobile = "";
            string regID = "";
            DataTable dt = new DataTable();
            DataTable dtDistLicense = new DataTable("DistLicense");
            DataTable dtSMIDLicense = new DataTable("SMIDLicense");
            DataView dvDistLicense = new DataView();
            DataView dvSMIDLicense = new DataView();

            #endregion

            try
            {
                #region License
                str = "select  CompCode from Mastenviro";
                compcode = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));

                string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
                SqlConnection cn = new SqlConnection(constrDmLicense);
                SqlCommand cmd;

                if (!string.IsNullOrEmpty(visibleDistid) && string.IsNullOrEmpty(visibleSMID))
                {
                    str = "SELECT STUFF( (SELECT ',' + (''''+t2.mobile+'''') FROM (select  mobile from mastparty where partyid in (" + visibleDistid + ")) t2 FOR XML PATH ('')), 1, 1, '') as name";
                    distMobile = DbConnectionDAL.GetStringScalarVal(str);
                    if (distMobile != "0")
                    {
                        regid_query = "select Reg_id,Mobile from LineMaster where Upper(Product)='GOLDIEE' and  CompCode='" + compcode + "' and Mobile in (" + distMobile + ") ";
                        cmd = new SqlCommand(regid_query, cn);
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtDistLicense);
                        cn.Close();
                        da.Dispose();
                        cmd.Dispose();
                        dvDistLicense.Table = dtDistLicense;
                    }

                }
                if (!string.IsNullOrEmpty(visibleSMID) && string.IsNullOrEmpty(visibleDistid))
                {
                    str = "SELECT STUFF( (SELECT ',' + (''''+t2.mobile+'''') FROM (select  mobile from mastsalesrep where smid in (" + visibleSMID + ")) t2 FOR XML PATH ('')), 1, 1, '') as name";
                    smidMobile = DbConnectionDAL.GetStringScalarVal(str);
                    if (smidMobile != "0")
                    {
                        regid_query = "select Reg_id,Mobile from LineMaster where Upper(Product)='GOLDIEE SALES' and  CompCode='" + compcode + "' and Mobile in (" + smidMobile + ") ";
                        cmd = new SqlCommand(regid_query, cn);
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtSMIDLicense);
                        cn.Close();
                        da.Dispose();
                        cmd.Dispose();
                        dvSMIDLicense.Table = dtSMIDLicense;
                    }


                }




                #endregion


                str = "select Title,Fromdate,Todate from MastActivityCustomHeader where Header_Id=" + TempHeaderId + "";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                HeaderTemplateName = dt.Rows[0]["Title"].ToString();
                fDate = dt.Rows[0]["Fromdate"].ToString();
                tDate = dt.Rows[0]["Todate"].ToString();

                if (Convert.ToDateTime(fDate) <= Convert.ToDateTime(fromdate) && Convert.ToDateTime(tDate) >= Convert.ToDateTime(todate))
                {
                    //using (TransactionScope transactionScope = new TransactionScope())
                    //{

                    #region SaveTemplate
                    if (EditFlg == "0")
                    {
                        if (visibleSMID != "")
                        {
                            str = "select COUNT(tmap.ID) from TransAcivityMapping tmap left join ActivityMapDistSales aSD on aSD.ActivityMapID=tmap.ID where tmap.TemplateHeaderId=" + TempHeaderId + " and aSD.smid in (" + visibleSMID + ")   and FromDate<='" + fromdate + "' and ToDate>='" + todate + "' ";
                        }
                        if (visibleDistid != "")
                        {
                            str = "select COUNT(tmap.ID) from TransAcivityMapping tmap left join ActivityMapDistSales aSD on aSD.ActivityMapID=tmap.ID where tmap.TemplateHeaderId=" + TempHeaderId + " and aSD.DistId in (" + visibleDistid + ")    and FromDate<='" + fromdate + "' and ToDate>='" + todate + "' ";
                        }

                        count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                        if (count > 0)
                        {
                            Msg = "-2";
                            return Msg;

                        }

                        str = "Insert Into TransAcivityMapping(TemplateHeaderId,[FromDate],[ToDate],[CreatedBySMID],Remark) output inserted.ID values(" + TempHeaderId + ",'" + Settings.dateformat(fromdate) + "','" + Settings.dateformat(todate) + "'," + Settings.Instance.SMID + ",'" + Remark.Replace("'", " ") + "')";
                        Id = DbConnectionDAL.GetIntScalarVal(str);
                        if (Id > 0)
                        {
                            if (!string.IsNullOrEmpty(visibleDistid) && string.IsNullOrEmpty(visibleSMID))
                            {
                                string multiCharString = visibleDistid;
                                string[] multiArray = multiCharString.Split(',');

                                foreach (string distID in multiArray)
                                {
                                    if (!string.IsNullOrEmpty(distID))
                                    {
                                        str = "Insert Into ActivityMapDistSales output inserted.id  Values(" + Id + "," + distID + ",NULL)";
                                        retval = DbConnectionDAL.GetStringScalarVal(str);
                                        if (!string.IsNullOrEmpty(retval))
                                        {
                                            str = "select mobile from mastparty where partyid=" + distID;
                                            distMobile = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                                            if (distMobile != "")
                                            {
                                                dvDistLicense.RowFilter = "mobile='" + distMobile + "'";
                                                if (dvDistLicense.ToTable().Rows.Count > 0)
                                                {
                                                    regID = dvDistLicense.ToTable().Rows[0]["Reg_id"].ToString();
                                                    if (!string.IsNullOrEmpty(regID))
                                                    {
                                                        notificationMsg = "Header Template -" + HeaderTemplateName + ", Till " + todate + "  is approved for you.Please fill this form-" + HeaderTemplateName;
                                                        InsertPushNotification(distID, notificationMsg, "Dist", TempHeaderId, regID);
                                                    }

                                                }
                                                dvDistLicense.RowFilter = null;

                                            }

                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(visibleSMID) && string.IsNullOrEmpty(visibleDistid))
                            {
                                string multiCharString = visibleSMID;
                                string[] multiArray = multiCharString.Split(',');

                                foreach (string SMID in multiArray)
                                {
                                    if (!string.IsNullOrEmpty(SMID))
                                    {
                                        str = "Insert Into ActivityMapDistSales output inserted.id Values(" + Id + ",NULL," + SMID + ")";
                                        retval = DbConnectionDAL.GetStringScalarVal(str);
                                        if (!string.IsNullOrEmpty(retval))
                                        {
                                            str = "select mobile from mastsalesrep where smid=" + SMID;
                                            smidMobile = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                                            if (smidMobile != "")
                                            {
                                                dvSMIDLicense.RowFilter = "mobile='" + smidMobile + "'";
                                                if (dvSMIDLicense.ToTable().Rows.Count > 0)
                                                {
                                                    regID = dvSMIDLicense.ToTable().Rows[0]["Reg_id"].ToString();
                                                    if (!string.IsNullOrEmpty(regID))
                                                    {
                                                        notificationMsg = "Header Template -" + HeaderTemplateName + ", Till " + todate + "  is approved for you.Please fill this form-" + HeaderTemplateName;
                                                        InsertPushNotification(SMID, notificationMsg, "SP", TempHeaderId, regID);
                                                    }
                                                }
                                                dvSMIDLicense.RowFilter = null;

                                            }
                                        }

                                    }
                                }
                            }
                        }

                    }

                    #endregion

                    #region UpdateTemplate
                    if (EditFlg == "1")
                    {

                        str = "Update TransAcivityMapping Set TemplateHeaderId=" + TempHeaderId + ",[UpdateBySMID]=" + Settings.Instance.SMID + ",[FromDate]='" + Settings.dateformat(fromdate) + "',[ToDate]='" + Settings.dateformat(todate) + "',Remark='" + Remark.Replace("'", " ") + "' output inserted.ID  Where Id=" + MapId + "";
                        Id = DbConnectionDAL.GetIntScalarVal(str);
                        if (Id > 0)
                        {
                            str = "Delete from ActivityMapDistSales where ActivityMapID=" + MapId + "";
                            int i = DbConnectionDAL.ExecuteQuery(str);
                            if (i > 0)
                            {
                                if (visibleSMID != "")
                                {
                                    str = "select COUNT(tmap.ID) from TransAcivityMapping tmap left join ActivityMapDistSales aSD on aSD.ActivityMapID=tmap.ID where tmap.TemplateHeaderId=" + TempHeaderId + " and aSD.smid in (" + visibleSMID + ")   and FromDate<='" + fromdate + "' and ToDate>='" + todate + "' ";
                                }
                                else if (visibleDistid != "")
                                {
                                    str = "select COUNT(tmap.ID) from TransAcivityMapping tmap left join ActivityMapDistSales aSD on aSD.ActivityMapID=tmap.ID where tmap.TemplateHeaderId=" + TempHeaderId + " and aSD.DistId in (" + visibleDistid + ")    and FromDate<='" + fromdate + "' and ToDate>='" + todate + "' ";
                                }

                                count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                                if (count > 0)
                                {
                                    Msg = "-2";
                                    return Msg;

                                }
                                if (!string.IsNullOrEmpty(visibleDistid) && string.IsNullOrEmpty(visibleSMID))
                                {
                                    string multiCharString = visibleDistid;
                                    string[] multiArray = multiCharString.Split(',');

                                    foreach (string distID in multiArray)
                                    {
                                        if (!string.IsNullOrEmpty(distID))
                                        {
                                            str = "Insert Into ActivityMapDistSales output inserted.id  Values(" + Id + "," + distID + ",NULL)";
                                            retval = DbConnectionDAL.GetStringScalarVal(str);
                                            if (!string.IsNullOrEmpty(retval))
                                            {
                                                str = "select mobile from mastparty where partyid=" + distID;
                                                distMobile = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                                                if (distMobile != "")
                                                {
                                                    dvDistLicense.RowFilter = "mobile='" + distMobile + "'";
                                                    if (dvDistLicense.ToTable().Rows.Count > 0)
                                                    {
                                                        regID = dvDistLicense.ToTable().Rows[0]["Reg_id"].ToString();
                                                        if (!string.IsNullOrEmpty(regID))
                                                        {
                                                            notificationMsg = "Header Template -" + HeaderTemplateName + ", Till " + todate + "  is approved for you.Please fill this form-" + HeaderTemplateName;
                                                            InsertPushNotification(distID, notificationMsg, "Dist", TempHeaderId, regID);
                                                        }

                                                    }
                                                    dvDistLicense.RowFilter = null;

                                                }

                                            }
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(visibleSMID) && string.IsNullOrEmpty(visibleDistid))
                                {
                                    string multiCharString = visibleSMID;
                                    string[] multiArray = multiCharString.Split(',');

                                    foreach (string SMID in multiArray)
                                    {
                                        if (!string.IsNullOrEmpty(SMID))
                                        {
                                            str = "Insert Into ActivityMapDistSales output inserted.id Values(" + Id + ",NULL," + SMID + ")";
                                            retval = DbConnectionDAL.GetStringScalarVal(str);
                                            if (!string.IsNullOrEmpty(retval))
                                            {

                                                str = "select mobile from mastsalesrep where smid=" + SMID;
                                                smidMobile = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                                                if (smidMobile != "")
                                                {
                                                    dvSMIDLicense.RowFilter = "mobile='" + smidMobile + "'";
                                                    if (dvSMIDLicense.ToTable().Rows.Count > 0)
                                                    {
                                                        regID = dvSMIDLicense.ToTable().Rows[0]["Reg_id"].ToString();
                                                        if (!string.IsNullOrEmpty(regID))
                                                        {
                                                            notificationMsg = "Header Template -" + HeaderTemplateName + ", Till " + todate + "  is approved for you.Please fill this form-" + HeaderTemplateName;
                                                            InsertPushNotification(SMID, notificationMsg, "SP", TempHeaderId, regID);
                                                        }
                                                    }
                                                    dvSMIDLicense.RowFilter = null;

                                                }


                                            }

                                        }
                                    }
                                }
                            }
                        }


                    }


                    #endregion
                    //}
                }
                else
                {
                    Msg = "-1";
                }

            }
            catch (Exception)
            {
                Msg = "0";
            }

            return Msg;
        }


        public static void InsertPushNotification(string DistId, string DisplayTitle, string type, string HeaderId, string regID)
        {
            try
            {
                string _sql = "", _result = "";
                string title = "Activity Template Approved";
                string pro_id = "ActivityMapped";
                string retval = "";
                DataTable dt = new DataTable();

                if (type == "Dist")
                {
                    _sql = "select UserId , mobile from MastParty where PartyId=" + DistId + "";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, _sql);


                    _sql = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[ToSmid]) output inserted.NotiId values ('" + pro_id + "'," + Convert.ToInt32(dt.Rows[0]["userid"].ToString()) + ",getdate(),'','" + DisplayTitle + "','" + 0 + "'," + Settings.Instance.UserID + "," + Settings.Instance.SMID + "," + DistId + ") ";

                    retval = DbConnectionDAL.GetStringScalarVal(_sql);
                    if (!string.IsNullOrEmpty(retval))
                    {

                        _result = PushNotificationForDistAndManager(DisplayTitle, dt.Rows[0]["UserId"].ToString(), "GOLDIEE", title, HeaderId, DistId, regID);
                    }
                }
                else if (type == "SP")
                {
                    _sql = "select UserId,mobile from MastSalesRep where SMID=" + DistId + "";
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, _sql);

                    _sql = " INSERT INTO TransNotification ([pro_id],[userid],[VDate],[msgURL],[displayTitle],[Status],[FromUserId],[SMId],[ToSmid]) output inserted.NotiId values ('" + pro_id + "'," + Convert.ToInt32(dt.Rows[0]["userid"].ToString()) + ",getdate(),'','" + DisplayTitle + "','" + 0 + "'," + Settings.Instance.UserID + "," + Settings.Instance.SMID + "," + DistId + ") ";

                    retval = DbConnectionDAL.GetStringScalarVal(_sql);

                    if (!string.IsNullOrEmpty(retval))
                    {

                        _result = PushNotificationForDistAndManager(DisplayTitle, dt.Rows[0]["UserId"].ToString(), "GOLDIEE SALES", title, HeaderId, DistId, regID);
                    }
                }
            }
            catch (Exception ex)
            {

            }


        }

        public static string PushNotificationForDistAndManager(string msg, string userid, string ProductType, string title, string Docid, string Distid, string regID)
        {

            var result = "";
            string Query = "", Query1 = "";
            DataTable dt = new DataTable();
            string serverKey = "", senderId = "";
            try
            {
                DataTable dtserverdetail = DbConnectionDAL.GetDataTable(CommandType.Text, "Select DistApp_FireBase_ServerKey,DistApp_FireBase_SenderID,MktApp_FireBase_ServerKey,MktApp_FireBase_SenderID,compurl,CompCode from Mastenviro ");

                if (!string.IsNullOrEmpty(regID))
                {
                    Query1 = "insert into TransPushNotification(userid,[Subject],Content,WebFlag) output inserted.id " +
                        "values (" + userid + ",'" + title + "','" + msg + "','Y')";
                    string Id = DbConnectionDAL.GetStringScalarVal(Query1);

                    if (ProductType == "GOLDIEE")
                    {
                        serverKey = dtserverdetail.Rows[0]["DistApp_FireBase_ServerKey"].ToString();
                        senderId = dtserverdetail.Rows[0]["DistApp_FireBase_SenderID"].ToString();
                    }
                    else if (ProductType == "GOLDIEE SALES")
                    {
                        serverKey = dtserverdetail.Rows[0]["MktApp_FireBase_ServerKey"].ToString();
                        senderId = dtserverdetail.Rows[0]["MktApp_FireBase_SenderID"].ToString();
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
                        to = regID,
                        priority = "high",
                        content_available = true,

                        data = new
                        {
                            body = msg,
                            title = title,
                            docid = Docid,
                            distid = Distid,
                            partyName = "",
                            orderStatus = "",
                            orderDate = "",
                            image = "",
                            msg = msg
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
                else
                {
                    result += "Registration No Is Null";
                }

            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public static string GetMappedActivityTemplate()
        {
            DataTable dt = new DataTable();
            string strQ = "select  ROW_NUMBER() over(order by id ) SNO,tMap.ID,header.Title,replace(convert(varchar(12),tMap.FromDate,106),' ','/') As Fromdate, replace(convert(varchar(12),tMap.ToDate,106),' ','/') As Todate,isnull(remark,'') remark from TransAcivityMapping tMap left join MastActivityCustomHeader header on header.Header_Id=tMap.TemplateHeaderId order by tMap.ID";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod(EnableSession = true)]
        public static string GetMappedActivityTemplateById(string TempId)
        {
            DataTable dt = new DataTable();
            string strQ = "select isnull(distid,'') as distid,ISNULL(smid,'') smid,mapID,TemplateHeaderId,replace(convert(varchar(12),FromDate,106),' ','/') As FromDate, replace(convert(varchar(12),ToDate,106),' ','/') As ToDate,isnull(remark,'') Remark,isnull(SMIDcityid,'') as SMIDcityid,isnull(Distcityid,'') as Distcityid,isnull(sPState,'') sPState,isnull(DistState,'') DistState  from ( select distinct STUFF( (SELECT ', ' +convert(varchar, t1.distid) FROM (select  distid from ActivityMapDistSales where ActivityMapID=tmap.id) t1 FOR XML PATH ('')), 1, 1, '') distid ,STUFF( (SELECT ', ' +convert(varchar, t1.smid) FROM (select  smid from ActivityMapDistSales where ActivityMapID=tmap.id) t1 FOR XML PATH ('')), 1, 1, '') smid ,tmap.ID as mapID,tmap.TemplateHeaderId,tmap.FromDate,tmap.ToDate,tmap.Remark ,STUFF( (SELECT ', ' +convert(varchar, t1.CityId) FROM (select CityId from MastSalesRep where smid in (select  SMId from ActivityMapDistSales where ActivityMapID=tmap.id)) t1 FOR XML PATH ('')), 1, 1, '') as SMIDcityid,STUFF( (SELECT ', ' +convert(varchar, t1.CityId) FROM (select CityId from MastParty where PartyId in (select  DistId from ActivityMapDistSales where ActivityMapID=tmap.id)) t1 FOR XML PATH ('')), 1, 1, '') Distcityid,STUFF( (SELECT ', ' +convert(varchar, t1.sPState) FROM ( select  mdt.UnderId as sPState from MastSalesRep msp  left join  mastarea mct on mct.AreaId=msp.CityId left join mastarea mdt on mdt.AreaId=mct.UnderId  where msp.smid in  (aDS.smid)) t1 FOR XML PATH ('')), 1, 1, '') sPState,STUFF( (SELECT ', ' +convert(varchar, t1.DistState) FROM ( select  mdt.UnderId as DistState from MastParty msp  left join  mastarea mct on mct.AreaId=msp.CityId left join mastarea mdt on mdt.AreaId=mct.UnderId  where msp.PartyId in  (aDS.distid)) t1 FOR XML PATH ('')), 1, 1, '') DistState from TransAcivityMapping tmap left join ActivityMapDistSales aDS on aDS.ActivityMapID=tmap.ID  where tmap.id=" + TempId + ") a";
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, strQ);
            return JsonConvert.SerializeObject(dt);
        }



    }
}