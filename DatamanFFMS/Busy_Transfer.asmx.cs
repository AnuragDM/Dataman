using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services.Protocols;
using BusinessLayer;
using System.Xml;
using System.Data;
using System.IO;
using DAL;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections;
using BAL;
using System.Globalization;
using System.IO.Compression;
using BAL.AdvanceReq;
using AstralFFMS.ServiceReferenceDMTracker;
using System.Net.Mime;
using System.Drawing;
using System.Diagnostics;
using System.Dynamic;
using AstralFFMS;
using Newtonsoft.Json.Linq;
//using AstralFFMS.SalesOrder;
//using AstralFFMS.Nav_SalesPrice_ItemDiscountReference;
using AstralFFMS.SaleOrderReference;
using System.Transactions;
using System.Net.Http;
using System.Net.Http.Headers;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using AstralFFMS.ClassFiles;

namespace AstralFFMS
{
    /// <summary>
    /// Summary description for Busy_Transfer
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Busy_Transfer : System.Web.Services.WebService
    {

        String Query = "";
        BAL.LeaveRequest.LeaveAll lvAll = new BAL.LeaveRequest.LeaveAll();
        BAL.ExpensesGroupBAL EXG = new ExpensesGroupBAL();
        BAL.Busy_Integration.BusyIntegrate bsy = new BAL.Busy_Integration.BusyIntegrate();
        BAL.AdvanceReq.AdvanceReqBAL obj = new BAL.AdvanceReq.AdvanceReqBAL();
        DistributorBAL DB = new DistributorBAL();
        MasterOperation _mo = new MasterOperation();
        SMSAdapter sms = new SMSAdapter();

        [WebMethod]
        public bool CheckWebServiceResponse()
        {
            System.Threading.Thread.Sleep(1000);
            return true;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertItems_Busy(string ItemName, string Unit, bool Active, decimal StdPack, decimal Mrp, decimal Dp, decimal Rp, string ParentName, string ItemCode, string Syncid, string ItemType, string DispName, string PriceGroup, string primaryunit, string Secondaryunit, decimal PrimaryUnitfactor, decimal SecondaryUnitfactor, decimal MOQ, bool Promoted, decimal cgstper, decimal sgstper, decimal igstper, string Segment, string ProductClass, string Type)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.InsertItems_Busy(ItemName.Replace("-","&"), Unit, Active, StdPack, Mrp, Dp, Rp, ParentName, ItemCode, Syncid, ItemType, DispName, PriceGroup, primaryunit, Secondaryunit, PrimaryUnitfactor, SecondaryUnitfactor, MOQ, Promoted, cgstper, sgstper, igstper, Segment, ProductClass);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }
            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateItems_Busy(Int32 ItemId, string ItemName, string Unit, bool Active, decimal StdPack, decimal Mrp, decimal Dp, decimal Rp, string ParentName, string ItemCode, string Syncid, string ItemType, string DispName, string PriceGroup, string primaryunit, string Secondaryunit, decimal PrimaryUnitfactor, decimal SecondaryUnitfactor, decimal MOQ, bool Promoted, decimal cgstper, decimal sgstper, decimal igstper, string Segment, string ProductClass, string Type)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.UpdateItems_Busy(ItemId, ItemName.Replace("-", "&"), Unit, Active, StdPack, Mrp, Dp, Rp, ParentName, ItemCode, Syncid, ItemType, DispName, PriceGroup, primaryunit, Secondaryunit, PrimaryUnitfactor, SecondaryUnitfactor, MOQ, Promoted, cgstper, sgstper, igstper, Segment, ProductClass);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Insert_Login(string UserName, string Password, string email, int Roleid, bool isAdmin, string DeptId, string DesgId, string empName, decimal CostCentre, string empSyncId)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.Insert(UserName, Password, email, Roleid, isAdmin, DeptId, DesgId, empName, CostCentre, empSyncId);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Insert_SalesPerson(string SMName, string Pin, string SalesPerType, string DeviceNo, string DSRAllowDays, string isAdmin, string Address1, string Address2, string CityId, string Email, string Mobile, string Remarks, string RoleId, Int32 UserId, string SyncId, string UnderId, int GradeId, int DeptId, int DesgId, string DOB, string DOA, string ResCenID, string BlockReason, int BlockBy, string EmpName, bool AllowChangeCity, int MeetAllowDays, bool MobileAccess, string FromTime, string ToTime, string Interval, string Uploadinterval, string Accuracy, string Sendpushntification, string BatteryRecord, string groupcode, string retryinterval, bool gpsloc, bool mobileloc, string sys_flag, string Alarm, int alarmduration, string sendsms, string sendsmsperson, string lat, string longi)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.InsertSalespersons(SMName, Pin, SalesPerType, DeviceNo, DSRAllowDays, isAdmin, Address1, Address2, CityId, Email, Mobile, Remarks, RoleId, UserId, SyncId, UnderId, GradeId, DeptId, DesgId, DOB, DOA, ResCenID, BlockReason, BlockBy, EmpName, AllowChangeCity, MeetAllowDays, MobileAccess, FromTime, ToTime, Interval, Uploadinterval, Accuracy, Sendpushntification, BatteryRecord, groupcode, retryinterval, gpsloc, mobileloc, sys_flag, Alarm, alarmduration, sendsms, sendsmsperson, lat, longi);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDistributorBusy(string Compcode, string PartyName, string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, bool Active, string Phone, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal CreditLimit, decimal OutStanding, Int32 SMID, string DOA, string DOB, int Areaid, string AreaName, string BeatName, string TCountryName, string TStateName, string CityName, int countryid, int stateid, int regionid, int distictid, int citytypeid, int cityconveyancetype, int Beatid, int Roleid, string UserName, string DistType, int createduserid, string UnderId, int PartyType, string Partygstin)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.InsertDistributorBusy(Compcode, PartyName, DistName, Address1, Address2, CityId, Pin, Email, Mobile, Remark, SyncId, BlockReason, Active, Phone, ContactPerson, CSTNo, VatTin, ServiceTax, PanNo, CreditLimit, OutStanding, SMID, DOA, DOB, Areaid, AreaName, BeatName, TCountryName, TStateName, CityName, countryid, stateid, regionid, distictid, citytypeid, cityconveyancetype, Beatid, Roleid, UserName, DistType, createduserid, UnderId, PartyType, Partygstin);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateDistributorBusy(int DistID, string Compcode, string PartyName, string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, bool Active, string Phone, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal CreditLimit, decimal OutStanding, Int32 SMID, string DOA, string DOB, int Areaid, string AreaName, string BeatName, string TCountryName, string TStateName, string CityName, int countryid, int stateid, int regionid, int distictid, int citytypeid, int cityconveyancetype, int Beatid, int Roleid, string UserName, string DistType, int createduserid, string UnderId, int PartyType, string Partygstin)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.UpdateDistributorBusy(DistID, Compcode, PartyName, DistName, Address1, Address2, CityId, Pin, Email, Mobile, Remark, SyncId, BlockReason, Active, Phone, ContactPerson, CSTNo, VatTin, ServiceTax, PanNo, CreditLimit, OutStanding, SMID, DOA, DOB, Areaid, AreaName, BeatName, TCountryName, TStateName, CityName, countryid, stateid, regionid, distictid, citytypeid, cityconveyancetype, Beatid, Roleid, UserName, DistType, createduserid, UnderId, PartyType, Partygstin);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetSMID(string SMID, string UID)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                if (SMID == "" && UID == "")
                {
                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMid from MastSalesRep Where SMName='DIRECTOR'"));
                }
                else if (SMID != "" && UID == "")
                {
                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select SMid from MastSalesRep Where SyncId='" + SMID + "'"));
                }
                else if (SMID == "" && UID != "")
                {
                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Userid from MastSalesRep Where SMName='DIRECTOR'"));
                }
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetLgn(string LGNID, string SYNC)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Count(*) from MastLogin where Name='" + LGNID + "' and EmpSyncId='" + SYNC + "'"));
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdLgn(string ACTIVE, string ID, string EMPSYNC)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                //result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Count(*) from MastLogin where Name='" + LGNID + "'"));
                if (EMPSYNC == "")
                {
                    string updLoginQry = @"update MastLogin set Active='" + ACTIVE + "' where Id=" + ID + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, updLoginQry);
                }
                else
                {
                    string syncid = "update Mastsalesrep set SyncId='" + EMPSYNC + "' where SMId=" + EMPSYNC + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);

                    string empsyncid = "update MastLogin set EmpSyncId='" + EMPSYNC + "' where Id=" + ID + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, empsyncid);
                }
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetPID(string PID)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select PartyId from MastParty Where SyncId='" + PID + "'"));
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCID(string CONTID)
        {
            int result = 0, res = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='COUNTRY' AND AreaName='" + CONTID + "'"));
                if (result == 0)
                {
                    res = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select AreaId from MastArea where AreaName='.'"));

                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastArea(AreaName,AreaType,SyncID,UnderID,Lvl,AreaDesc,Active,CityType,CityConveyanceType,CreatedDate,STDCode,ISDCode,BuisnessPlace,Section,CostCentre) OUTPUT INSERTED.AREAID values('" + CONTID + "','COUNTRY','" + res + "','','',1,0,0,'','','" + result + "',0,0,0)"));
                }
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetSID(string STATID, int CONTRID)
        {
            int result = 0, res = 0, reg = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                reg = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select AreaId from MastArea where AreaType='REGION' AND AreaName='Blank'"));
                if (reg == 0)
                {
                    reg = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastArea(AreaName,AreaType,SyncID,UnderID,Lvl,AreaDesc,Active,CityType,CityConveyanceType,CreatedDate,STDCode,ISDCode,BuisnessPlace,Section,CostCentre) OUTPUT INSERTED.AREAID values('Blank',' REGION ','" + CONTRID + "','','',1,0,0,'','','" + reg + "',0,0,0)"));
                }

                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='STATE' AND AreaName='" + STATID + "'"));
                if (result == 0)
                {
                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastArea(AreaName,AreaType,SyncID,UnderID,Lvl,AreaDesc,Active,CityType,CityConveyanceType,CreatedDate,STDCode,ISDCode,BuisnessPlace,Section,CostCentre) OUTPUT INSERTED.AREAID values('" + STATID + "','STATE','" + reg + "','','',1,0,0,'','','" + result + "',0,0,0)"));
                }
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDISTID(string DISTID, int STATEID)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from MastArea where Areatype='DISTRICT' And AreaName='" + DISTID + "'"));
                if (result == 0)
                {
                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastArea(AreaName,AreaType,SyncID,UnderID,Lvl,AreaDesc,Active,CityType,CityConveyanceType,CreatedDate,STDCode,ISDCode,BuisnessPlace,Section,CostCentre) OUTPUT INSERTED.AREAID values('" + DISTID + "','DISTRICT','" + STATEID + "','','',1,0,0,'','','" + result + "',0,0,0)"));
                }
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCTID(string CITYID, int DISTID)
        {
            int result = 0, citytypeid = 0, cityconveyancetype = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select ID from MastCityType where Name='Other'"));
                if (citytypeid == 0)
                {
                    citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastCityType(Name) OUTPUT INSERTED.ID values('Other')"));
                }

                cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select ID from MastCityConveyanceType where Name='Other'"));
                if (cityconveyancetype == 0)
                {
                    cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastCityConveyanceType(Name) OUTPUT INSERTED.ID values('Other')"));
                }

                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from MastArea where Areatype='CITY' And AreaName='" + CITYID + "'"));
                if (result == 0)
                {
                    result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastArea(AreaName,AreaType,SyncID,UnderID,Lvl,AreaDesc,Active,CityType,CityConveyanceType,CreatedDate,STDCode,ISDCode,BuisnessPlace,Section,CostCentre) OUTPUT INSERTED.AREAID values('" + CITYID + "','CITY','" + DISTID + "','','',1,'" + citytypeid + "','" + cityconveyancetype + "','','01','" + result + "',0,0,0)"));
                }
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetAREAID(string AREAID)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='AREA-" + AREAID + "'"));
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetRoll()
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select RoleId from MastRole where RoleType='Distributor'"));
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCountLedger(string DISTID, string Vdate)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Count(*) from TransDistributerLedger where VDate='" + Vdate + "' and Narration='Opening Balance' and DistId=" + DISTID + ""));
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDistLed()
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Isnull(Max(DistLedId),0)+1 from  TransDistributerLedger"));
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDistLedger(string docID, string Date, string result, string Amount, string AmountCr, string AmountDr, string open, string Compcode, string Entryno)
        {
            int res = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                res = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into TransDistributerLedger(DLDocId,Vdate,DistID,Amount,AmtCr,AmtDr,Narration,COMPANYCODE,EntryNo) values('" + docID + "','" + Date + "', " + result + "," + Amount + "," + Convert.ToDecimal(AmountCr) + "," + Convert.ToDecimal(AmountDr) + ",'" + open + "','" + Compcode + "'," + Entryno + ")"));
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(res));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertBusyLog(string DistName, string DistSync, string SmSync, string InsTime)
        {
            int res = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                res = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into Busy_Log(DistName,DistSync,SmSync,InsTime) values('" + DistName + "','" + DistSync + "', " + SmSync + "," + InsTime + ")"));
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(res));
        }


        [DataContract]
        public class GETLOG
        {
            [DataMember]
            public string DistName { get; set; }
            [DataMember]
            public string DistSync { get; set; }
            [DataMember]
            public string SmName { get; set; }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetBsyLog(string SYNCID, string SMSYNC)
        {
            int res = 0;
            DataTable dt = new DataTable();
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            List<GETLOG> rst = new List<GETLOG>();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                dt = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from Busy_Log where DistSync='" + SYNCID + "' and ChngTime IS NULL");

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["SmSync"].ToString()) != Convert.ToInt32(SMSYNC))
                    {
                        res = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "update Busy_Log set ChngTime=DateAdd(minute,330,getutcdate()) OUTPUT INSERTED.LogId where DistSync='" + SYNCID + "' and ChngTime IS NULL"));
                        //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, TransDistInv);
                    }
                }
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(res));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetItemId(string ITEMTYPE, string SYNCID)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select ItemId from MastItem where ItemType='" + ITEMTYPE + "' and [SyncId]='" + SYNCID + "'"));
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void DelData(string YR2, string COMPCODE)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                //result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Count(*) from MastLogin where Name='" + LGNID + "'"));

                string TransDistInv = @"Delete from TransDistInv where DistInvDocId like '%" + YR2.ToString() + "' and Compcode='" + COMPCODE.ToString() + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, TransDistInv);

                string TransDistInv1 = @"Delete from TransDistInv1 where DistInvDocId like '%" + YR2.ToString() + "' and Compcode='" + COMPCODE.ToString() + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, TransDistInv1);

                string TransDistInv2 = @"Delete from TransDistInv2 where DistInvDocId like '%" + YR2.ToString() + "' and Compcode='" + COMPCODE.ToString() + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, TransDistInv2);

                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDistId(string DISTINVDOCID, string COMPCODE)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select DistInvId from TransDistInv where DistInvDocId='" + DISTINVDOCID + "' and Compcode='" + COMPCODE + "'"));
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertsaleinvheader_Busy(string Docid, string VDate, decimal taxamt, decimal Billamount, decimal Roundoff, decimal discamo, string SyncId, string compcode)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.insertsaleinvheader_Busy(Docid, VDate, taxamt, Convert.ToDecimal(Billamount), Roundoff, discamo, SyncId, compcode); ;
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertsaleinvdetail_Busy(int DistInvId, int sno, string Docid, string VDate, decimal taxamt, decimal Qty, decimal Rate, decimal Amount, string SyncId, string itemname, string ItemMasterid, string Unit, string compcode)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.insertsaleinvdetail_Busy(DistInvId, sno, Docid, VDate, taxamt, Qty, Rate, Amount, SyncId, itemname, ItemMasterid, Unit, compcode);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertRetailersaleinvexpensedetail(string Docid, string Desc, decimal Amt, string compcode)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.insertRetailersaleinvexpensedetail(Docid, Desc, Amt, compcode);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void DelLedger(int YR, string COMPANYCODE, string TYP)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                //result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Count(*) from MastLogin where Name='" + LGNID + "'"));

                string TransDistInv = @"Delete from TransDistributerLedger where DLDocId Like 'BU%' and ((Year (VDate)=" + YR + " AND (Month (VDate)>=4 And Month (VDate)<=12)) OR (Year (VDate)=" + (YR + 1) + " AND (Month (VDate)>=1 And Month (VDate)<=3))) and COMPANYCODE='" + COMPANYCODE.ToString() + "' and Narration!='" + TYP.ToString() + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, TransDistInv);


                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void insertdistledger_Busy(string Syncid, string VDate, decimal amtdr, decimal amtcr, decimal amt, string narr, string VID, string VTYPE, string compcode)
        {
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            string Query = "";
            int result = 0;
            string minDate1 = "", mobiletime = "", mobiletimehm = "";
            try
            {
                result = bsy.insertdistledger_Busy(Syncid, VDate, amtdr, amtcr, amt, narr, VID, VTYPE, compcode);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDistSync()
        {
            string result = "";
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                result = Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, "select STUFF((SELECT ', ' +SyncId from(select replace(SyncId,'BU#','') AS SyncId from MastParty where SyncId like '%BU#%')tbl FOR XML PATH ('')) , 1, 1, '') "));

            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }
            //return dt_rol;
            Context.Response.Write(JsonConvert.SerializeObject(result));
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void DelAgileData(string COMPCODE)
        {
            int result = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                //string _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                //result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "select Count(*) from MastLogin where Name='" + LGNID + "'"));

                string TransDistInv = @"Delete from [MastAgile] where Compcode='" + COMPCODE.ToString() + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, TransDistInv);
                //str = ex.Message;
                //err = err + 1;
                //LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                //dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3);
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertAgile(string PartySyncId, string Date, string DueDate, string VchNo, string Payment, string CompCode, string Mode)
        {
            int result = 0, res = 0;
            string createText = "";
            createText = " @@@@@@ Start DateTime @@@@@" + "" + System.DateTime.Now + " @@@@@@  " + Environment.NewLine;
            DataTable dt_rol = new DataTable();
            try
            {
                result = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Insert into MastAgile(PartySyncId,Date,DueDate,VchNo,Payment,CompCode,mode) OUTPUT INSERTED.ID values('" + PartySyncId + "','" + Convert.ToDateTime(Date).ToString("MM/dd/yyyy") + "','" + Convert.ToDateTime(DueDate).ToString("MM/dd/yyyy") + "','" + VchNo + "','" + Payment + "','" + CompCode + "','" + Mode + "')"));
            }
            catch (Exception ex)
            {
                createText += "@@@Exception in _InsertTemp_Sample1_ @@@@@@@@@ " + "" + ex.Message + "" + Environment.NewLine;
            }
            finally
            {
            }

            Context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}
