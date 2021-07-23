using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Net;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;

namespace AstralFFMS
{
    public partial class UnlockDSR : System.Web.UI.Page
    {
        BAL.LeaveRequest.LeaveAll lvAll = new BAL.LeaveRequest.LeaveAll();
        protected void Page_Load(object sender, EventArgs e)
        {
            txtfmDate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                if(Request.QueryString["Date"] !=null)
                txtfmDate.Text = Request.QueryString["Date"];
                // txtfmDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                BindSalesPersons();

                if (Request.QueryString["SMId"] != null)
                    lstSalesPerson.SelectedValue = Request.QueryString["SMId"];
            }
        }

        private void BindSalesPersons()
        {
            string qry = "select SMID,SMName AS SMName from MastSalesRep where smname not in ('.') and Active=1  order by SMName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, qry);
            if (dt.Rows.Count > 0)
            {
                lstSalesPerson.DataSource = dt;
                lstSalesPerson.DataTextField = "SMName";
                lstSalesPerson.DataValueField = "SMID";
                lstSalesPerson.DataBind();
            }
            else
            {
                lstSalesPerson.Items.Clear();
            }
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int Lock = DbConnectionDAL.GetIntScalarVal("select Lock from wip_transvisit where smid=" + lstSalesPerson.SelectedValue + " and vdate='" + Settings.dateformat(txtfmDate.Text) + "' and Lock=0");
                if (Convert.ToInt32(Lock) > 0)
                {
                    int delqry = DbConnectionDAL.GetIntScalarVal("delete from wip_transvisit where smid=" + lstSalesPerson.SelectedValue + " and vdate='" + Settings.dateformat(txtfmDate.Text) + "'");
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('DSR UnLocked Successfully');", true);

                }
                else
                {
                    int VisitID = DbConnectionDAL.GetIntScalarVal("Select VisId from Transvisit where smid=" + lstSalesPerson.SelectedValue + " and vdate='" + Settings.dateformat(txtfmDate.Text) + "'");
                    if (Convert.ToInt32(VisitID) > 0)
                    {
                        fillTemp_TransOrderItemWise(Convert.ToInt32(VisitID));
                        TransOrderDelete1ItemWise(Convert.ToInt32(VisitID));
                        fillTemp_TransDemo(Convert.ToInt32(VisitID));
                        TransDemoDelete1(Convert.ToInt32(VisitID));
                        fillTemp_Collection(Convert.ToInt32(VisitID));
                        CollectionDelete1(Convert.ToInt32(VisitID));
                        fillTemp_FailedVisite(Convert.ToInt32(VisitID));
                        FailedVisiteDelete1(Convert.ToInt32(VisitID));
                        fillTemp_Copetitor(Convert.ToInt32(VisitID));
                        CopetitorDelete1(Convert.ToInt32(VisitID));
                        fillTemp_Discussion(Convert.ToInt32(VisitID));
                        DiscussionDelete1(Convert.ToInt32(VisitID));
                        fillTemp_DistributorOpeningStock(Convert.ToInt32(VisitID));
                        DeleteTemp_DistributorOpeningStock(Convert.ToInt32(VisitID));
                        fillTemp_TransSample(Convert.ToInt32(VisitID));
                        TransSampleDelete(Convert.ToInt32(VisitID));
                        fillTemp_TransSalesReturn(Convert.ToInt32(VisitID));
                        TransSalesReturnDelete(Convert.ToInt32(VisitID));

                        updateDSR1(Convert.ToInt32(VisitID));
                        string str = "Update TransVisit set UnlockRequest='approved' where VisId=" + VisitID + "";
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                       string compcode = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select CompCode From MastEnviro"));
             
                        string pro_id = "DSRUnlocked";
                        string displaytitle = "";
                        DataTable dtgrpsmid = DbConnectionDAL.getFromDataTable("select smid,SMName,userid from mastsalesrep where smid in (" + Settings.Instance.SMID + ")");
                        string sql_getsenior = "select smid,Mobile,userid from mastsalesrep ms left join mastrole mr on ms.roleid=mr.RoleId where smid in (" + lstSalesPerson.SelectedValue + ")";
                        DataTable dt_getsenior = DbConnectionDAL.GetDataTable(CommandType.Text, sql_getsenior);
                        displaytitle = "DSR Unlocked by " + dtgrpsmid.Rows[0]["SMName"].ToString() + " - Dsr date " + txtfmDate.Text + " ";

                        lvAll.InsertTransNotification(pro_id, Convert.ToInt32(dt_getsenior.Rows[0]["userid"].ToString()), Settings.GetUTCTime(), "DSRUnlocked?SMId=" + lstSalesPerson.SelectedValue + "&VisId=" + VisitID + "&Date=" + txtfmDate.Text + "", displaytitle, 0, Convert.ToInt32(dtgrpsmid.Rows[0]["userid"].ToString()), Convert.ToInt32(dtgrpsmid.Rows[0]["SMID"].ToString()), Convert.ToInt32(dt_getsenior.Rows[0]["smid"].ToString()));

                        pushnotificationonorderdispatchcancel(displaytitle, compcode,
                                            dt_getsenior.Rows[0]["mobile"].ToString(),
                                            "DSRUnlocked", dtgrpsmid.Rows[0]["SMID"].ToString(),
                                            dtgrpsmid.Rows[0]["SMName"].ToString(), "FFMS", "", "", "");

                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('DSR UnLocked Successfully');", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('DSR not available!!!');", true);
                    }
                }
            }           

        }

        public string pushnotificationonorderdispatchcancel(string msg, string compcode, string mobileno, string title, string createdbysmid, string smname, string ProductType, string Docid = "", string partyname = "", string Dispatchcancelstatus = "")
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
                                docid = Docid,
                                PartyName = partyname,
                                // image = "http://lakshya.goldiee.com/SalespersonImages/defaultspimg.png",//"http://sfmstest.dataman.net.in/ThumbnailImage/VID-20170808-WA0009.jpg"
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

        private static void fillTemp_TransOrderItemWise(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransOrder (VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,android_id,created_date,Longitude,Latitude,Lat_long_datetime,address,[Mobile_Created_date],[ImgUrl],[DispatchRemarks],[DispatchCancelType],[DispatchCancel_userd])
                             SELECT VisId,OrdDocId,UserId,VDate,SMId,PartyId,AreaId,Remarks,OrderAmount,OrderStatus,MeetFlag,MeetDocId,OrderType,android_id,created_date,Longitude,Latitude,Lat_long_datetime,address,[Mobile_Created_date],[ImgUrl],[DispatchRemarks],[DispatchCancelType],[DispatchCancel_userd]
                            FROM TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"INSERT INTO dbo.Temp_TransOrder1 (OrdId,VisId, OrdDocId, Sno, UserId, VDate, SMId, PartyId, AreaId, ItemId, Qty, FreeQty, Rate, Discount, Remarks, 
                             MeetFlag, MeetDocId, amount,android_id,android_id1,created_date,cases,unit,Longitude,Latitude,Lat_long_datetime,address,[Mobile_Created_date],[DistId],[DispatchQty])
                             SELECT  OrdId, VisId, OrdDocId, Sno, UserId, VDate, SMId, PartyId, AreaId, ItemId, Qty, FreeQty, Rate,
                             Discount, Remarks, MeetFlag, MeetDocId, amount,android_id,android_id1,created_date,cases,unit,Longitude,Latitude,Lat_long_datetime,address,[Mobile_Created_date],[DistId],[DispatchQty]
                             FROM dbo.TransOrder1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);


                str = "update Temp_TransOrder set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransOrder1 set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransOrderDelete1ItemWise(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransOrder where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"delete  FROM TransOrder1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillTemp_TransDemo(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransDemo ([VisId],[DemoDocId],[UserId],[VDate] ,[SMId] ,[PartyId] ,[Remarks]  ,[AreaId]  ,[CompleteAppDetail]  ,[AvailablityShop]  ,[IsPartyConverted]
      ,[NewAppArea]  ,[TechAdvantage]  ,[TechSuggestion]  ,[NewApp]   ,[OrderType]  ,[ProductClassId]  ,[ProductSegmentId]  ,[ProductMatGrp] ,[ItemId],[ImgURL],android_id,created_date,Longitude,Latitude,Lat_long_datetime,address,[Mobile_Created_date] )
    SELECT [VisId] ,[DemoDocId] ,[UserId] ,[VDate] ,[SMId] ,[PartyId] ,[Remarks] ,[AreaId] ,[CompleteAppDetail] ,[AvailablityShop]  ,[IsPartyConverted],[NewAppArea] ,[TechAdvantage]  ,[TechSuggestion]
      ,[NewApp] ,[OrderType],[ProductClassId]   ,[ProductSegmentId]   ,[ProductMatGrp]      ,[ItemId],[ImgUrl],android_id,created_date,Longitude,Latitude,Lat_long_datetime ,address,[Mobile_Created_date] FROM TransDemo where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                str = "update Temp_TransDemo set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransDemoDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransDemo where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillTemp_Collection(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransCollection ([VisId] ,[CollDocId] ,[UserId] ,[VDate] ,[PartyId] ,[SMId] ,[AreaId] ,[ItemId] ,[Mode] ,[Amount] ,[PaymentDate] ,[Cheque_DDNo]
                ,[Cheque_DD_Date] ,[Bank] ,[Branch]  ,[Remarks],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date])
	            SELECT [VisId] ,[CollDocId]  ,[UserId]   ,[VDate]  ,[PartyId]   ,[SMId]   ,[AreaId]  ,[ItemId]  ,[Mode]  ,[Amount]
                ,[PaymentDate] ,[Cheque_DDNo] ,[Cheque_DD_Date] ,[Bank] ,[Branch] ,[Remarks],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date]
	            from TransCollection where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

                str = "update Temp_TransCollection set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void CollectionDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransCollection where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillTemp_FailedVisite(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO [Temp_TransFailedVisit] ([VisId] ,[FVDocId] ,[VDate]  ,[UserID]  ,[SMId] ,[PartyId]  ,[Remarks]
      ,[AreaId]   ,[Nextvisit]   ,[ReasonID],[VisitTime],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[ImgUrl] )
SELECT [VisId]  ,[FVDocId]   ,[VDate]   ,[UserID]   ,[SMId]   ,[PartyId]   ,[Remarks]   ,[AreaId]   ,[Nextvisit]   ,[ReasonID],[VisitTime],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[ImgUrl]  FROM [TransFailedVisit] where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransFailedVisit set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void FailedVisiteDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransFailedVisit where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void fillTemp_Copetitor(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO [Temp_TransCompetitor] ([VisId] ,[DocId]  ,[VDate] ,[UserId] ,[PartyId]  ,[Item]  ,[Qty]  ,[Rate],[SMID],[CompName],[ImgUrl],[Remarks],[Discount],[BrandActivity],[MeetActivity],[RoadShow],[Scheme/offers],[OtherGeneralInfo],[OtherActivity],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date] )
SELECT [VisId] ,[DocId]  ,[VDate] ,[UserId] ,[PartyId]  ,[Item]  ,[Qty]  ,[Rate],[SMID],[CompName],[ImgUrl],[Remarks],[Discount],[BrandActivity],[MeetActivity],[RoadShow],[Scheme/offers],[OtherGeneralInfo],[OtherActivity],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date] FROM [TransCompetitor] where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransCompetitor set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void CopetitorDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransCompetitor where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void fillTemp_Discussion(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO [Temp_TransVisitDist] ([UserId],[VisId] ,[Sno] ,[VDate] ,[cityId] ,[SMId] ,[DistId] ,[areaId] ,[remarkDist] ,[remarkArea]  ,[remarkL1] ,[L1UserId],[DissType],[NextVisitDate],[NextVisitTime],[SpentfrTime],[SpentToTime],[ImgUrl],[android_id],[created_date],[DiscDocid],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[Type])
SELECT [UserId],[VisId] ,[Sno] ,[VDate] ,[cityId] ,[SMId] ,[DistId] ,[areaId] ,[remarkDist] ,[remarkArea] ,[remarkL1] ,[L1UserId],[DissType],[NextVisitDate],[NextVisitTime],[SpentfrTime],[SpentToTime],[ImgUrl],[android_id],[created_date],[DiscDocid],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[Type]  FROM [TransVisitDist] where [VisId]=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransVisitDist set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private static void DiscussionDelete1(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransVisitDist where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void fillTemp_DistributorOpeningStock(Int64 VisitID)
        {
            try
            {
                string str = @" INSERT INTO Temp_TransDistStock ([VisId],[STKDocId],[UserId],[VDate],[SMId],[DistId],[DistCode],[AreaId],[ItemId],[Qty],[Android_Id],[Created_date],[unit],[cases],[Address],[Latitude],[Longitude],[Lat_long_datetime],[Mobile_Created_date],[ImgUrl],[MRP])
                SELECT [VisId],[STKDocId],[UserId],[VDate],[SMId],[DistId],[DistCode],[AreaId],[ItemId],[Qty],[Android_Id],[Created_date],[unit],[cases],[Address],[Latitude],[Longitude],[Lat_long_datetime],[Mobile_Created_date],[ImgUrl],[MRP] FROM TransDistStock where [VisId]=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransDistStock set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private static void DeleteTemp_DistributorOpeningStock(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransDistStock where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void fillTemp_TransSample(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransSample ([VisId], [SampleDocId], [UserId], [VDate], [SMId], [PartyId], [AreaId], [Remarks], [Amount],[Status],[MeetFlag],[MeetDocId],[Type],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[ImgUrl],[Sample])
                             SELECT [VisId],[SampleDocId],[UserId],[VDate],[SMId],[PartyId],[AreaId],[Remarks],[Amount],[Status],[MeetFlag],[MeetDocId],[Type],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[ImgUrl],[Sample]
                            FROM TransSample where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"INSERT INTO dbo.Temp_TransSample1 ([SampleId], [VisId], [SampleDocId], [Sno], [UserId], [VDate], [SMId], [PartyId], [AreaId], [ItemId], [Qty], [FreeQty], [Rate], [Discount], [Remarks], 
                             [MeetFlag], [MeetDocId], [amount], [android_id] ,[android_id1],[created_date],[cases],[unit],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[DistId])
                             SELECT  [SampleId], [VisId], [SampleDocId], [Sno], [UserId], [VDate], [SMId], [PartyId], [AreaId], [ItemId], [Qty], [FreeQty], [Rate], [Discount], [Remarks], 
                             [MeetFlag], [MeetDocId], [amount], [android_id] ,[android_id1],[created_date],[cases],[unit],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[DistId]
                             FROM dbo.TransSample1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);


                str = "update Temp_TransSample set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransSample1 set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransSampleDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransSample where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"delete  FROM TransSample1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private static void fillTemp_TransSalesReturn(Int64 VisitID)
        {
            try
            {
                string str = @"INSERT INTO Temp_TransSalesReturn ([VisId], [SRetDocId], [UserId], [VDate], [SMId], [PartyId], [AreaId], [Remarks], [Amount],[Status],[MeetFlag],[MeetDocId],[Type],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[ImgUrl],[RRId],[InvoiceName])
                             SELECT [VisId], [SRetDocId], [UserId], [VDate], [SMId], [PartyId], [AreaId], [Remarks], [Amount],[Status],[MeetFlag],[MeetDocId],[Type],[android_id],[created_date],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[ImgUrl],[RRId],[InvoiceName]
                            FROM TransSalesReturn where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"INSERT INTO dbo.Temp_TransSalesReturn1 ([SRetId], [VisId], [SRetDocId], [Sno], [UserId], [VDate], [SMId], [PartyId], [AreaId], [ItemId], [Qty], [FreeQty], [Rate], [Discount], [Remarks], 
                             [MeetFlag], [MeetDocId], [amount], [android_id] ,[android_id1],[created_date],[cases],[unit],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[DistId],[RCId],[MfDate],[BatchNo])
                             SELECT  [SRetId], [VisId], [SRetDocId], [Sno], [UserId], [VDate], [SMId], [PartyId], [AreaId], [ItemId], [Qty], [FreeQty], [Rate], [Discount], [Remarks], 
                             [MeetFlag], [MeetDocId], [amount], [android_id] ,[android_id1],[created_date],[cases],[unit],[Longitude],[Latitude],[Lat_long_datetime],[address],[Mobile_Created_date],[DistId],[RCId],[MfDate],[BatchNo]
                             FROM dbo.TransSalesReturn1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);


                str = "update Temp_TransSalesReturn set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = "update Temp_TransSalesReturn1 set created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
        private static void TransSalesReturnDelete(Int64 VisitID)
        {
            try
            {
                string str = @"delete  FROM TransSalesReturn where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
                str = @"delete  FROM TransSalesReturn1 where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, str);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void updateDSR1(Int64 VisitID)
        {
            try
            {
                string sr = "update TransVisit set Lock=0,AppStatus=NULL,AppBy=NULL,AppRemark=NULL,AppBySMId=NULL,created_date=	DateAdd(minute,330,getutcdate()) where VisId=" + VisitID;
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sr);

            }
            catch
            {

            }
        }

    }
}
